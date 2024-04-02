namespace TimeTracker.VwMdl;

internal class TimesheetPreviewVM : BindableBaseViewModel
{
  readonly Bpr _bpr = new();
  A0DbContext? _db;
  readonly Stopwatch _sw = Stopwatch.StartNew();
  DateTime _today = DateTime.Today;
  readonly TimePerDay[] _week = new TimePerDay[7];
  readonly bool _autoStart = false;
  bool _skipDbSave = false;
  const string _same = "F1 same: ", __new = "F1 ***: ";
#if DEBUG
  const bool isDbg = true;
#else
  const bool isDbg = false;
#endif

  public TimesheetPreviewVM(bool autoStart) => _autoStart = autoStart;
  protected override void AutoExec()
  {
    base.AutoExec();
    Bpr.Beep1of2();

    WinTitle = $"TimeTracker - {App.AppStartAt:ddd HH:mm}";

    var rnd = new Random(DateTime.Now.Second);
    Signre = $"/TimeTracker;component/Assets/Sign-re{rnd.Next(10)}.png";
    SigSkw = -10 - rnd.Next(15);

    try
    {
      dbLoad();
      setWeeklyDefaultHours(_today);

      //Appender = string.Format("{0} / {1} feeds/dnlds    {2} \r\n", FeedList.Count, DnLdList.Count, _autoStart ? "Auto staritng ..." : "");

      if (!_autoStart)
        return;

      onF9All4Steps(null);

      WinTitle = $"TimeTracker - {App.AppStartAt:ddd HH:mm}";
    }
    catch (Exception ex) { _ = ex.Log(); Appender = "\r\nDownloads failed.\r\n" + ex.ToString(); }

    Bpr.Beep2of2();
  }

  protected override async Task ClosingVM() { if (_skipDbSave) App.SpeakFaF("Skipped saving to DB."); else onDbSave(); await Task.Delay(99); }
  void setWeeklyDefaultHours(DateTime today)
  {
    //return; //todo: remove duplication on every period change clisk event: 

    var dd =
        today.DayOfWeek == DayOfWeek.Friday ? 0 :
        today.DayOfWeek == DayOfWeek.Thursday ? 1 :
        today.DayOfWeek == DayOfWeek.Wednesday ? 2 :
        today.DayOfWeek == DayOfWeek.Tuesday ? 3 :
        today.DayOfWeek == DayOfWeek.Monday ? 4 :
        today.DayOfWeek == DayOfWeek.Sunday ? 5 :
        today.DayOfWeek == DayOfWeek.Saturday ? 6 : 0;

    DaySatDate = today.AddDays(dd - 6);
    DaySunDate = today.AddDays(dd - 5);
    DayMonDate = today.AddDays(dd - 4);
    DayTueDate = today.AddDays(dd - 3);
    DayWedDate = today.AddDays(dd - 2);
    DayThuDate = today.AddDays(dd - 1);
    DayFriDate = today.AddDays(dd - 0);
    WeekEnding =
    SignedDate = today.AddDays(dd - 0);

    var h1 = 8.0;
    var we = 0.0;
    var jc = Settings.DefaultJobCategoryId;
    var nt = _db?.lkuJobCategories.Find(jc).Description;
    var d = 0;
    _week[d++] = DaySat = getCreateTimePerDay(DaySatDate, h1, we, jc, nt);
    _week[d++] = DaySun = getCreateTimePerDay(DaySunDate, h1, we, jc, nt);
    _week[d++] = DayMon = getCreateTimePerDay(DayMonDate, h1, we, jc, nt);
    _week[d++] = DayTue = getCreateTimePerDay(DayTueDate, h1, we, jc, nt);
    _week[d++] = DayWed = getCreateTimePerDay(DayWedDate, h1, we, jc, nt);
    _week[d++] = DayThu = getCreateTimePerDay(DayThuDate, h1, we, jc, nt);
    _week[d++] = DayFri = getCreateTimePerDay(DayFriDate, h1, we, jc, nt);

    TtlWkHours = _week.ToList().Sum(r => r.WorkedHours);
  }

  TimePerDay getCreateTimePerDay(DateTime dayDate, double h1, double we, string jc, string nt)
  {
    var a = _db.TimePerDays.FirstOrDefault(r => r.WorkedOn == dayDate);
    a ??= _db.TimePerDays.Add(new TimePerDay { WorkedOn = dayDate, HourStarted = h1, WorkedHours = we, JobCategoryId = jc, Note = nt, CreatedAt = App.AppStartAt });

    return a;
  }

  void dbLoad()
  {
    _db = A0DbContext.Create();
    CurVer = AAV.Sys.Helpers.VerHelper.CurVerStr();
    Invoicer = _db.Invoicers.FirstOrDefault(r => r.Id == Settings.CurrentInvoicerId) ?? throw new ArgumentNullException("@@@@@@@@@@@@@@@@");
    Invoicee = _db.Invoicees.FirstOrDefault(r => r.Id == Settings.CurrentInvoiceeId) ?? throw new ArgumentNullException("@@@@@@@@@@@@@@@@");
  }

  void onF9All4Steps(object? p) { }

  protected override bool CanClose() => true; //nogo:  _db == null ? true : !_db.HasUnsavedChanges();
  bool _IsBusy = false;       /**/ public bool IsBusy { get => _IsBusy; set => Set(ref _IsBusy, value); }
  string _cv = "?!@#";        /**/ public string CurVer { get => _cv; set => Set(ref _cv, value); }
  string _WinTitle = "";      /**/ public string WinTitle { get => _WinTitle; set => Set(ref _WinTitle, value); }
  string _InfoMsg = "";       /**/ public string InfoMsg { get => _InfoMsg; set => Set(ref _InfoMsg, value); }
  string _Appender = "";      /**/ public string Appender
  {
    get => _Appender;
    set {

      var l = value.Split('\n');
      if (l.Length > 6)
        value = string.Join("\n", l, 1, l.Length - 1);
      _ = Set(ref _Appender, value);
    }
  } // value.StartsWith("^") ? value : value.Contains("\n") ? value + this._InfoMsg : this._InfoMsg + value); } }
  bool _canPrevPrd = true;    /**/ public bool CanMovePrd { get => _canPrevPrd; set => Set(ref _canPrevPrd, value); }

  string _Signre = "/TimeTracker;component/Assets/Sign-re5.png"; public string Signre { get => _Signre; set => Set(ref _Signre, value); }
  double _SigSkw = 20;       /**/ public double SigSkw { get => _SigSkw; set => Set(ref _SigSkw, value); }
  double _TtlWkHours;         /**/ public double TtlWkHours { get => _TtlWkHours; set => Set(ref _TtlWkHours, value); }

  Invoicer _invoicer;         /**/ public Invoicer Invoicer { get => _invoicer; set => Set(ref _invoicer, value); }
  Invoicee _invoicee;         /**/ public Invoicee Invoicee { get => _invoicee; set => Set(ref _invoicee, value); }
  DefaultSetting? _stg = null; /**/ public DefaultSetting Settings => _stg ??= _db?.DefaultSettings?.FirstOrDefault();

  TimePerDay _DaySat;         /**/ public TimePerDay DaySat { get => _DaySat; set => Set(ref _DaySat, value); }
  TimePerDay _DaySun;         /**/ public TimePerDay DaySun { get => _DaySun; set => Set(ref _DaySun, value); }
  TimePerDay _DayMon;         /**/ public TimePerDay DayMon { get => _DayMon; set => Set(ref _DayMon, value); }
  TimePerDay _DayTue;         /**/ public TimePerDay DayTue { get => _DayTue; set => Set(ref _DayTue, value); }
  TimePerDay _DayWed;         /**/ public TimePerDay DayWed { get => _DayWed; set => Set(ref _DayWed, value); }
  TimePerDay _DayThu;         /**/ public TimePerDay DayThu { get => _DayThu; set => Set(ref _DayThu, value); }
  TimePerDay _DayFri;         /**/ public TimePerDay DayFri { get => _DayFri; set => Set(ref _DayFri, value); }

  DateTime _DaySatDate;       /**/ public DateTime DaySatDate { get => _DaySatDate; set => Set(ref _DaySatDate, value); }
  DateTime _DaySunDate;       /**/ public DateTime DaySunDate { get => _DaySunDate; set => Set(ref _DaySunDate, value); }
  DateTime _DayMonDate;       /**/ public DateTime DayMonDate { get => _DayMonDate; set => Set(ref _DayMonDate, value); }
  DateTime _DayTueDate;       /**/ public DateTime DayTueDate { get => _DayTueDate; set => Set(ref _DayTueDate, value); }
  DateTime _DayWedDate;       /**/ public DateTime DayWedDate { get => _DayWedDate; set => Set(ref _DayWedDate, value); }
  DateTime _DayThuDate;       /**/ public DateTime DayThuDate { get => _DayThuDate; set => Set(ref _DayThuDate, value); }
  DateTime _DayFriDate;       /**/ public DateTime DayFriDate { get => _DayFriDate; set => Set(ref _DayFriDate, value); }
  DateTime _WeekEnding;       /**/ public DateTime WeekEnding { get => _WeekEnding; set => Set(ref _WeekEnding, value); }
  DateTime _SignedDate;       /**/ public DateTime SignedDate { get => _SignedDate; set => Set(ref _SignedDate, value); }

  ICommand _PrevPrd; /**/ public ICommand PrevPrdCmd => _PrevPrd ??= new RelayCommand(x => onMovePrd(-7), x => !IsBusy) { GestureKey = Key.Left, GestureModifier = ModifierKeys.None };
  ICommand _NextPrd; /**/ public ICommand NextPrdCmd => _NextPrd ??= new RelayCommand(x => onMovePrd(+7), x => !IsBusy) { GestureKey = Key.Right, GestureModifier = ModifierKeys.None };
  ICommand _OldOrgA; /**/ public ICommand OldOrgACmd => _OldOrgA ??= new RelayCommand(onOldOrgA, x => !IsBusy) { GestureKey = Key.I, GestureModifier = ModifierKeys.Control };
  ICommand _DbSave;  /**/

  [Obsolete]
  public ICommand DbSaveCmd => _DbSave ??= new RelayCommand(x => onDbSave(), x => canDbSave) { GestureKey = Key.S, GestureModifier = ModifierKeys.Control };
  ICommand _DbQuit;  /**/ public ICommand DbQuitCmd => _DbQuit ??= new RelayCommand(x => onDbQuit()) { GestureKey = Key.Q, GestureModifier = ModifierKeys.Control };
  ICommand _Print;   /**/ public ICommand PrintCmd => _Print ??= new RelayCommand(onPrint, x => !IsBusy) { GestureKey = Key.P, GestureModifier = ModifierKeys.Control };
  ICommand _Email;   /**/

  [Obsolete]
  public ICommand EmailCmd => _Email ??= new RelayCommand(onBlindEmail, x => !IsBusy) { GestureKey = Key.E, GestureModifier = ModifierKeys.Control };
  ICommand _Letter;  /**/

  [Obsolete]
  public ICommand LetterCmd => _Letter ??= new RelayCommand(onPrepAndShowLetter, x => !IsBusy) { GestureKey = Key.E, GestureModifier = ModifierKeys.Control };
  ICommand _UnLock;  /**/ public ICommand UnLockCmd => _UnLock ??= new RelayCommand(onUnLock, x => !IsBusy) { GestureKey = Key.U, GestureModifier = ModifierKeys.Control };

  void onUnLock(object x)
  {
   _bpr.Click();
    _week.ToList().ForEach(r => r.IsLocked = false);
    InfoMsg = _db?.GetDbChangesReport();
  }

  void onOldOrgA(object x) => new FromTillCtgrTaskNote().ShowDialog(); //Old/Org.

  [Obsolete]
  async void onBlindEmail(object printArea)
  {
    try
    {
      IsBusy = true; Bpr.Beep1of2();

      App.SpeakFaF("Wait... Sending email could take a while...");

      _week.ToList().ForEach(r => r.IsLocked = true);

      var hardcopy = InvoiceFilepathname(WeekEnding.ToString("yyyy-MM-dd"), TtlWkHours, Settings.InvoiceSubFolder, "Timesheet", "xps");
      XamlToXps.Export(new Uri(hardcopy, UriKind.Absolute), printArea as FrameworkElement); //review XPS: FixedViewer.Document = new XpsDocument(filename, FileAccess.Read).GetFixedDocumentSequence();

      var isSuccess = await Emailer.SmtpSend(
          Settings.Invoicer.FromEmail,
          Invoicee.TimesheetEmail,
              $"Timesheet for the period {DaySatDate:MMMM d} - {DayFriDate:MMMM d} ",
          string.Format(Invoicee.InvoiceEmailBody, DaySatDate, DayFriDate, "timesheet", "Yuvraj"),
          new string[] { hardcopy });

      DayFri.Note +=
              $"\n (timesheet {(isSuccess ? "sent" : "sending failed")} to {Invoicee.TimesheetEmail} on {App.AppStartAt:MMMd HH:mm})";

      onDbSave();
    }
    catch (Exception ex) { _ = ex.Log(); Appender = "failed.\r\n" + ex.ToString(); }
    finally { IsBusy = false; Bpr.Beep2of2(); }
  }

  [Obsolete]
  void onPrepAndShowLetter(object printArea)
  {
    try
    {
      IsBusy = true; Bpr.Beep1of2();

      _week.ToList().ForEach(r => r.IsLocked = true);

      var hardcopy = InvoiceFilepathname(WeekEnding.ToString("yyyy-MM-dd"), TtlWkHours, Settings.InvoiceSubFolder, "Timesheet", "xps");
      XamlToXps.Export(new Uri(hardcopy, UriKind.Absolute), printArea as FrameworkElement); //review XPS: FixedViewer.Document = new XpsDocument(filename, FileAccess.Read).GetFixedDocumentSequence();

      var subj = $"Timesheet for the period {DaySatDate:MMMM d} - {DayFriDate:MMMM d} ";
      var body = string.Format(Invoicee.InvoiceEmailBody, DaySatDate, DayFriDate, "timesheet", "Yuvraj");

      try
      {
        var (exitCode, errMsg) = Emailer.PerpAndShow(Invoicee.TimesheetEmail, subj, body, hardcopy);

        DayFri.Note += $"\n (timesheet {(exitCode == 0 ? "sent" : $"sending failed ({errMsg})")} to {Invoicee.TimesheetEmail} on {App.AppStartAt:MMMd HH:mm})";
      }
      catch (Exception ex) { _ = ex.Log(); Appender = "failed.\r\n" + ex.ToString(); }

      onDbSave();
    }
    catch (Exception ex) { _ = ex.Log(); Appender = "failed.\r\n" + ex.ToString(); }
    finally { IsBusy = false; Bpr.Beep2of2(); }
  }

  void onPrint(object printArea)
  {
  }

  int run(string exe, string arg)
  {
    var rv = -1;
    try
    {
      var myProcessStartInfo = new ProcessStartInfo(exe, arg)
      {
        RedirectStandardError = true,
        UseShellExecute = false
      };

      var myProcess = new Process
      {
        StartInfo = myProcessStartInfo
      };
      _ = myProcess.Start();

      do
        if (!myProcess.HasExited)
          myProcess.Refresh(); // "Refresh the current process property values."
      while (!myProcess.WaitForExit(100));

      var myStreamReader = myProcess.StandardError;
      rv = myProcess.ExitCode;
      //tbLst.Text += $"{myStreamReader.ReadLine()} {rv} ";
      myProcess.Close();
    }
    catch (Exception ex) { _ = ex.Log(); Appender = "failed.\r\n" + ex.ToString(); }

    return rv;
  }

  void onDbQuit() { _skipDbSave = true; CloseAppCmd.Execute(null); }

  [Obsolete]
  void onDbSave() { IsBusy = true; _bpr.Click(); Appender = InfoMsg = _db.TrySaveReport().report; App.SpeakFree(InfoMsg); IsBusy = false; }
  void onMovePrd(int sevenDays)
  {
    IsBusy = true;
    if (sevenDays > 0 && _today > App.AppStartAt)
    {
      canDbSave = false;
      _bpr.No(); //         App.Synth.SpeakAsync("No!");
    }
    else
    {
      canDbSave = true;
      _bpr.Click();
      _today = _today.AddDays(sevenDays);
      setWeeklyDefaultHours(_today);

      InfoMsg = _db.GetDbChangesReport();
    }

    IsBusy = false;
  }

  public static string InvoiceFilepathname(string prefx, double ttlHours, string sfolder, string docname, string ext)
  {
    var dir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), sfolder);
    if (!Directory.Exists(dir))
      Directory.CreateDirectory(dir);

    var filename = "excepted...";

    try
    {
      var i = 0;
      do
      {
        filename = $"{dir}\\{docname} {prefx} - {ttlHours:0#.#} hr - Alex Pigida{(i == 0 ? "" : $" - ({i})")}.";
#if DEBUG
        filename += "DEBUG.";
#endif
        filename += ext;
        i++;
      }
      while (File.Exists(filename));
    }
    catch (Exception ex) { ex.Log(); }

    return filename;
  }
  bool canDbSave = true;
}