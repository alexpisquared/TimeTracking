using AAV.Sys.Helpers;
using AsLink;
using Db.TimeTrack.DbModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using TimeTracker.AsLink;
using TimeTracker.Common;
using static AsLink.EvLogHelper;

namespace TimeTracker.View
{
  public partial class FromTillCtgrTaskNote : AAV.WPF.Base.WindowBase
  {
    A0DbContext _db = A0DbContext.Create();
    public const string _svcs = "Software development services."; // "Software engineering services.";

    public FromTillCtgrTaskNote()
    {
      InitializeComponent();

      KeyDown += (s, e) =>
      {
        switch (e.Key)
        {
          case Key.Escape: Close(); break;
          case Key.Left: movePayPrd(-1); break;
          case Key.Right: movePayPrd(+1); break;
        }
      };

      Closing += onClosing;
      DataContext = this;
      CurVer.Text = $"{_db.ServerDatabase()}   {VerHelper.CurVerStr()}";

      //AppSettings.RestoreSizePosition(this, Settings.Default.PeyPVw);
    }

    public static readonly DependencyProperty PayPrdBgnProperty = DependencyProperty.Register("PayPrdBgn", typeof(DateTime), typeof(FromTillCtgrTaskNote), new PropertyMetadata(DateTime.Today)); public DateTime PayPrdBgn { get => (DateTime)GetValue(PayPrdBgnProperty); set => SetValue(PayPrdBgnProperty, value); }
    public static readonly DependencyProperty PayPrdEndProperty = DependencyProperty.Register("PayPrdEnd", typeof(DateTime), typeof(FromTillCtgrTaskNote), new PropertyMetadata(DateTime.Today)); public DateTime PayPrdEnd { get => (DateTime)GetValue(PayPrdEndProperty); set => SetValue(PayPrdEndProperty, value); }
    public static readonly DependencyProperty UnInvdHrsProperty = DependencyProperty.Register("UnInvdHrs", typeof(decimal), typeof(FromTillCtgrTaskNote), new PropertyMetadata(0.0m)); public decimal UnInvdHrs { get => (decimal)GetValue(UnInvdHrsProperty); set => SetValue(UnInvdHrsProperty, value); }
    public static readonly DependencyProperty InvcedHrsProperty = DependencyProperty.Register("InvcedHrs", typeof(decimal), typeof(FromTillCtgrTaskNote), new PropertyMetadata(0.0m)); public decimal InvcedHrs { get => (decimal)GetValue(InvcedHrsProperty); set => SetValue(InvcedHrsProperty, value); }
    public static readonly DependencyProperty IsDfltAddProperty = DependencyProperty.Register("IsDfltAdd", typeof(bool), typeof(FromTillCtgrTaskNote), new PropertyMetadata(true)); public bool IsDfltAdd { get => (bool)GetValue(IsDfltAddProperty); set => SetValue(IsDfltAddProperty, value); }
    public static readonly DependencyProperty InfoMessgProperty = DependencyProperty.Register("InfoMessg", typeof(string), typeof(FromTillCtgrTaskNote), new PropertyMetadata(null)); public string InfoMessg { get => (string)GetValue(InfoMessgProperty); set => SetValue(InfoMessgProperty, value); }
    void movePayPrd(int isFwd)
    {
      switch (InvoiceE.PayPeriodMode)
      {
        case "m15": movePayPrd_m15(isFwd); break; // 	Semi-Monthly	
        case "mon": movePayPrd_mon(isFwd); break; // 	Monthly	
        case "wk1": movePayPrd_wk1(isFwd); break; // 	Weekly	
        case "wk2": movePayPrd_wk2(isFwd); break; // 	ByWeekly	
        case "flx": PayPrdBgn = firstUn_OrLastInvoced(); PayPrdEnd = PayPrdBgn.AddMonths(isFwd); break; // 	Flexible	NULL
        default: break;
      }

      loadPayPrd();
    }
    void movePayPrd_wk1(int isFwd) => throw new NotImplementedException();
    void movePayPrd_mon(int isFwd)
    {
      if (isFwd > 0)
      {
        PayPrdBgn = PayPrdBgn.AddMonths(1);
        PayPrdEnd = PayPrdBgn.AddMonths(1).AddDays(-1);
      }
      else if (isFwd < 0)
      {
        PayPrdEnd = PayPrdBgn.AddDays(-1);
        PayPrdBgn = PayPrdBgn.AddMonths(-1);
      }
      else // if (isFwd == 0) - use cur month on startup:
      {
        var n = _db.TimePerDays.Local.Max(r => r.WorkedOn).AddDays(7);
        PayPrdBgn = new DateTime(n.Year, n.Month, n.Day < 19 ? 1 : 16);
        PayPrdEnd = PayPrdBgn.AddMonths(1).AddDays(-1);
      }
    }
    void movePayPrd_wk2(int isFwd)
    {
      if (isFwd == 0) // on app start:
      {
        var lastINvoicedDay = _db.TimePerDays.Local.Where(r => r.InvoiceId != null).Max(r => r.WorkedOn);
        PayPrdBgn = lastINvoicedDay < InvoiceE.StartDate ? InvoiceE.StartDate.AddDays(InvoiceE.PayPeriodStart) : PayPrdBgn = lastINvoicedDay.AddDays(1);  // if first time for the invocee
      }
      else
        PayPrdBgn = PayPrdBgn.AddDays(isFwd > 0 ? InvoiceE.PayPeriodLength : -InvoiceE.PayPeriodLength);

      PayPrdEnd = PayPrdBgn.AddDays(InvoiceE.PayPeriodLength - 1);
    }
    void movePayPrd_m15(int isFwd)
    {
      if (isFwd > 0)
      {
        if (PayPrdBgn.Day == 1)
        {
          PayPrdEnd = PayPrdBgn.AddMonths(1).AddDays(-1);
          PayPrdBgn = PayPrdBgn.AddDays(15);
        }
        else //if (PayPrdBgn.Day == 16)
        {
          PayPrdBgn = PayPrdEnd.AddDays(1);
          PayPrdEnd = PayPrdBgn.AddDays(14);
        }
      }
      else if (isFwd < 0)
      {
        if (PayPrdBgn.Day == 1)
        {
          PayPrdEnd = PayPrdBgn.AddDays(-1);
          PayPrdBgn = PayPrdBgn.AddMonths(-1).AddDays(15);
        }
        else //if (PayPrdBgn.Day == 16)
        {
          PayPrdEnd = PayPrdBgn.AddDays(-1);
          PayPrdBgn = PayPrdBgn.AddDays(-15);
        }
      }
      else // if (isFwd == 0)
      {
        var n = _db.TimePerDays.Local.Max(r => r.WorkedOn).AddDays(7);
        PayPrdBgn = new DateTime(n.Year, n.Month, n.Day < 19 ? 1 : 16);
        PayPrdEnd = new DateTime(n.Year, n.Month, n.Day < 19 ? 15 : DateTime.DaysInMonth(n.Year, n.Month));
      }
    }

    void loadPayPrd()
    {
      var showStart = PayPrdBgn;// PayPrdEnd.AddMonths(-1).AddDays(1);
                                //_db.TimePerDays.Where(r => showStart <= r.WorkedOn && r.WorkedOn <= PayPrdEnd).OrderBy(r => r.WorkedOn).Load();

      recalcUnInvdHrs();

      var view = new ListCollectionView(_db.TimePerDays.Local)
      {
        Filter = (r => showStart <= ((TimePerDay)r).WorkedOn && ((TimePerDay)r).WorkedOn <= PayPrdEnd) // http://blogs.msdn.com/b/efdesign/archive/2010/09/08/data-binding-with-dbcontext.aspx
      };

      timeDg1.DataContext = view;

      if (timeDg1.Items.Count > 0) timeDg1.ScrollIntoView(timeDg1.Items[0]); //to show selected row as NOT single row on top.
      if (timeDg1.SelectedItem != null) timeDg1.ScrollIntoView(timeDg1.SelectedItem);
    }

    DateTime firstUn_OrLastInvoced()
    {
      try
      {
        var tm = _db.TimePerDays.Local.OrderBy(r => r.WorkedOn).FirstOrDefault(r => r.InvoiceId == null);
        if (tm != null)
          return tm.WorkedOn;

        var iv = _db.Invoices.Local.OrderByDescending(r => r.PeriodUpTo).FirstOrDefault();
        if (iv != null)
          return iv.PeriodUpTo.AddDays(1);

        return App.AppStartAt;
      }
      catch (Exception ex) { MessageBox.Show(ex.ToString()); } // if (Debugger.IsAttached) Debugger.Break(); 
      return App.AppStartAt;
    }
    DateTime getFuzzyTrgWkDay()
    {
      var last = _db.TimePerDays.Local.Where(r => PayPrdBgn <= r.WorkedOn && r.WorkedOn <= PayPrdEnd).LastOrDefault();
      var trgWkDay = last == null ? PayPrdBgn : last.WorkedOn < DateTime.Today ? last.WorkedOn.AddDays(1) : DateTime.Today;
      return trgWkDay;
    }

    static TimePerDay inferNewDay(DateTime trgWkDay, string jobCategoryId)
    {
      var h1 = ((int)((FuzzyLogic.FirstPowerOnTimeForTheDay(trgWkDay).TimeOfDay.TotalHours + .125) * 4.0)) / 4.0;
      var h2 = ((int)((FuzzyLogic.LastPowerOffTimeForTheDay(trgWkDay).TimeOfDay.TotalHours + .125) * 4.0)) / 4.0;
      if (h2 < h1) h2 = h1 + .25;
      if ((h2 - h1) > 6) h2 -= .5; // mandatory lunch break
      var newTimePerDay = new TimePerDay
      {
        CreatedAt = App.AppStartAt,
        WorkedOn = trgWkDay,
        HourStarted = h1,
        WorkedHours = h2 - h1,
        JobCategoryId = jobCategoryId,
        Note = _svcs
      };
      return newTimePerDay;
    }
    static TimePerDay inferNewDayAsync(DateTime trgWkDay, string jobCategoryId)
    {
      TimePerDay rv = null;
      Task<TimePerDay>.Factory.StartNew(() =>
      {
        var h1 = ((int)((FuzzyLogic.FirstPowerOnTimeForTheDay(trgWkDay).TimeOfDay.TotalHours + .125) * 4.0)) / 4.0;
        var h2 = ((int)((FuzzyLogic.LastPowerOffTimeForTheDay(trgWkDay).TimeOfDay.TotalHours + .125) * 4.0)) / 4.0;
        return new TimePerDay
        {
          CreatedAt = App.AppStartAt,
          WorkedOn = trgWkDay,
          HourStarted = h1,
          WorkedHours = h2 - h1,
          JobCategoryId = jobCategoryId,
          Note = _svcs
        };
      }).ContinueWith(_ => { return _.Result; }, TaskScheduler.FromCurrentSynchronizationContext());

      return rv;
    }
    async Task<bool> tryLoadCreate(bool createDb)
    {
      try
      {
#if DEBUG
        if (createDb)
          TimeTrackDbCtx_Code1st_DbInitializer.DbIni();//.DropCreateDb();
        App.SpeakAsync("DB creation is for Debug mode only (to preserve the existing data)");
#else
#endif

        await _db.Invoices.LoadAsync();
        await _db.TimePerDays.OrderBy(r => r.WorkedOn).LoadAsync();
        return true;
      }
      catch (EntityException eex)
      {
        _db.Database.Connection.Close();
        _db = A0DbContext.Create();

        switch (MessageBox.Show(eex.ToString(), "No Database Found - OK to generate new database?", MessageBoxButton.YesNo, MessageBoxImage.Question))
        {
          case MessageBoxResult.Yes: return await tryLoadCreate(true);
          case MessageBoxResult.No: return false;
        }
        return false;
      }
      catch (Exception ex) { MessageBox.Show(ex.ToString()); return false; } // if (Debugger.IsAttached) Debugger.Break(); 
    }
    List<TimePerDay> get8hrTimes(string ctgId, List<TimePerDay> TimePerDays, DateTime payPrdBgn, DateTime payPrdEnd, double dayStartHour, double hoursPerDay)
    {
      var tpd = new List<TimePerDay>();

      for (var day = payPrdBgn; day <= payPrdEnd; day = day.AddDays(1))
        //if (day.IsBizDay())
        if (tpd.Where(r => r.WorkedOn == day).FirstOrDefault() == null) // is already set to some time
          tpd.Add(new TimePerDay
          {
            CreatedAt = App.AppStartAt,
            WorkedOn = day,
            HourStarted = dayStartHour,
            WorkedHours = day.IsBizDay() ? hoursPerDay : 0d,
            JobCategoryId = ctgId,
            Note = day.IsBizDay() ? _svcs : ""
          });

      return tpd;
    }
    List<TimePerDay> getNewTimes(string ctgId, List<TimePerDay> TimePerDays, DateTime payPrdBgn, DateTime payPrdEnd)
    {
      var tpd = new List<TimePerDay>();

      for (var day = payPrdBgn; day <= payPrdEnd && day <= DateTime.Today; day = day.AddDays(1))
        if (tpd.Where(r => r.WorkedOn == day).FirstOrDefault() == null)
          tpd.Add(inferNewDay(day, ctgId));

      return tpd;
    }

    void recalcUnInvdHrs()
    {
      InvcedHrs = (decimal)_db.TimePerDays.Local.Where(r => r.InvoiceId != null && PayPrdBgn <= r.WorkedOn && r.WorkedOn <= PayPrdEnd).Sum(r => r.WorkedHours);
      UnInvdHrs = (decimal)_db.TimePerDays.Local.Where(r => r.InvoiceId == null && PayPrdBgn <= r.WorkedOn && r.WorkedOn <= PayPrdEnd).Sum(r => r.WorkedHours);
    }

    public static Invoice CreateInvoiceForThePeriod(A0DbContext db, DateTime ppBgn, DateTime ppEnd, Invoicee invcee, decimal ttlHr, decimal hst)
    {
      var newInvoice = new Invoice
      {
        PeriodFrom = ppBgn,
        PeriodUpTo = ppEnd,
        InvoiceDate = ppEnd,
        TotalHours = ttlHr,
        HstPercent = hst,
        IsSubmitted = false,
        HasCleared = false,
        RateSubmitted = invcee.CorpRate,
        InvoiceeId = invcee.Id,
        Invoicee = invcee
      };
      db.Invoices.Local.Add(newInvoice);

      ///1.1. assign the invoice ID
      foreach (var tpd in db.TimePerDays.Local.Where(r => r.Invoice == null && ppBgn <= r.WorkedOn && r.WorkedOn <= ppEnd)) tpd.Invoice = newInvoice;

      return newInvoice;
    }

    async void Window_Loaded(object s, RoutedEventArgs e)
    {
      Cursor = Cursors.AppStarting;
      try
      {
        var rv = await tryLoadCreate(false);// true also works!!!!!!!!!!
        if (rv == false)
        {
          Close();
        }
        else
        {
          movePayPrd(0);
          ctrlPnl1.IsEnabled = ctrlPnl2.IsEnabled = true;
        }
      }
      catch (Exception ex) { MessageBox.Show(ex.ToString()); } // if (Debugger.IsAttached) Debugger.Break(); 
      finally { Cursor = Cursors.Arrow; ctrlPnl1.IsEnabled = ctrlPnl2.IsEnabled = true; Bpr.BeepOk(); }
    }
    void onClosing(object s, System.ComponentModel.CancelEventArgs e) => InfoMessg = DbSaveOr(_db, e);

    public static string DbSaveOr(A0DbContext _db, System.ComponentModel.CancelEventArgs e)
    {
      try
      {
        if (_db.HasUnsavedChanges())
        {
          switch (MessageBox.Show(_db.GetDbChangesReport(), "Save Changes?", MessageBoxButton.YesNoCancel, MessageBoxImage.Question))
          {
            case MessageBoxResult.Yes: return _db.TrySaveReport().report;
            case MessageBoxResult.No: return "Changes has been discarded.";
            case MessageBoxResult.Cancel: e.Cancel = true; return "Closing has been aborted.";
          }
        }
      }
      catch (Exception ex) { MessageBox.Show(ex.ToString()); }

      return "";
    }

    Invoicer _invoiceR;         /**/ public Invoicer InvoiceR { get => _invoiceR ?? (_db.Invoicers.FirstOrDefault(r => r.Id == DSettngS.CurrentInvoicerId)); set => _invoiceR = value; }
    Invoicee _invoiceE;         /**/ public Invoicee InvoiceE { get => _invoiceE ?? (_db.Invoicees.FirstOrDefault(r => r.Id == DSettngS.CurrentInvoiceeId)); set => _invoiceE = value; }
    DefaultSetting _settingS;   /**/ public DefaultSetting DSettngS { get => _settingS ?? (_settingS = _db.DefaultSettings.FirstOrDefault()); set => _settingS = value; }


    void onInvoice(object s, RoutedEventArgs e)
    {
      recalcUnInvdHrs();

      var w = new InvoicePreview(_db)
      {
        Invoice = CreateInvoiceForThePeriod(_db, PayPrdBgn, PayPrdEnd, InvoiceE, UnInvdHrs, DSettngS.HstPercent),
        PayPrdBgn = PayPrdBgn,
        PayPrdEnd = PayPrdEnd,
        PayPrdHrs = UnInvdHrs
      };

      w.ShowDialog();

      InfoMessg = _db.GetDbChangesReport(0);
    }
    void onAddingNewItem(object s, AddingNewItemEventArgs e) => e.NewItem = inferNewDay(getFuzzyTrgWkDay(), _db.DefaultSettings.FirstOrDefault().DefaultJobCategoryId);
    void onDgSelctnChngd(object s, SelectionChangedEventArgs e) => recalcUnInvdHrs();
    void onSavDb(object s, RoutedEventArgs e) => InfoMessg = _db.TrySaveReport().report;
    void onChkDb(object s, RoutedEventArgs e) => InfoMessg = _db.GetDbChangesReport();
    void onAdd8hrDays(object s, RoutedEventArgs e)
    {
      Cursor = Cursors.AppStarting; ctrlPnl1.IsEnabled = ctrlPnl2.IsEnabled = false;

      IsDfltAdd = false;

      var stg = _db.DefaultSettings.FirstOrDefault();
      var ctg = stg.DefaultJobCategoryId;
      var dsh = stg.DayStartHour;
      var tps = _db.TimePerDays.Local.ToList();
      var bgn = PayPrdBgn;
      var end = PayPrdEnd;
      WorkedHrs.DailyHours = (double)InvoiceE.HoursPerPeriod;//stg.;

      Task<List<TimePerDay>>.Factory.StartNew(() => get8hrTimes(ctg, tps, bgn, end, dsh, WorkedHrs.DailyHours)).ContinueWith(_ =>
      {
        try
        {
          _.Result.ForEach(tpd =>
                {
                  if (!_db.TimePerDays.Local.Any(r => r.WorkedOn == tpd.WorkedOn))
                    _db.TimePerDays.Local.Add(tpd);
                }); recalcUnInvdHrs();
        }
        finally { Cursor = Cursors.Arrow; ctrlPnl1.IsEnabled = ctrlPnl2.IsEnabled = true; }
      }, TaskScheduler.FromCurrentSynchronizationContext());
    }
    void onInfer(object s, RoutedEventArgs e)
    {
      Cursor = Cursors.AppStarting; ctrlPnl1.IsEnabled = ctrlPnl2.IsEnabled = false;
      var ctg = _db.DefaultSettings.FirstOrDefault().DefaultJobCategoryId;
      var tps = _db.TimePerDays.Local.ToList();
      var bgn = PayPrdBgn;
      var end = PayPrdEnd;

      Task<List<TimePerDay>>.Factory.StartNew(() => getNewTimes(ctg, tps, bgn, end)).ContinueWith(_ =>
      {
        try { _.Result.ForEach(tpd => _db.TimePerDays.Local.Add(tpd)); recalcUnInvdHrs(); }
        finally { Cursor = Cursors.Arrow; ctrlPnl1.IsEnabled = ctrlPnl2.IsEnabled = true; }
      }, TaskScheduler.FromCurrentSynchronizationContext());
    }
    void btnUnderConstrClk(object s, RoutedEventArgs e) => MessageBox.Show("The feature is under contruction\n\nPlease come back soon", ((Button)s).Content.ToString().Replace("_", ""));
    void onPrevPrd(object s, RoutedEventArgs e) => movePayPrd(-1);
    void onNextPrd(object s, RoutedEventArgs e) => movePayPrd(+1);
    void setCustClk(object s, RoutedEventArgs e) => loadPayPrd();
    void onShowOptionMgr(object s, RoutedEventArgs e) => new OptionsManager().ShowDialog();
    void onShowHaysBrwsr(object s, RoutedEventArgs e) => new HaysBrowser { Settings = DSettngS }.ShowDialog();
    void btnUnderConst2Clk(object s, RoutedEventArgs e)
    {
      var n = DSettngS.Invoicer.AddressDetails.Split('\n');
      var t = $"{n[n.Length - 1]}\n\n{PayPrdBgn.AddDays(1):yyyy-MM-dd}\n\n=C5	=C8+DAY(1)	=D8+DAY(1)	=E8+DAY(1)	=F8+DAY(1)	=G8+DAY(1)	=H8+DAY(1)\n=C5	=C8+DAY(1)	=D8+DAY(1)	=E8+DAY(1)	=F8+DAY(1)	=G8+DAY(1)	=H8+DAY(1)\n";

      foreach (var c in (ListCollectionView)(timeDg1.DataContext))
      {
        if (!(c is TimePerDay)) continue;
        var d = c as TimePerDay;
        if (d.WorkedOn < PayPrdBgn.AddDays(1)) continue;

        t += $"{d.WorkedHours:N2}\t";
      }

      Clipboard.SetText(t);
    }
    void on___(object s, RoutedEventArgs e) { }
    void onDeletePeriodFree(object s, RoutedEventArgs e) { _db.TimePerDays.Local.Where(r => PayPrdBgn <= r.WorkedOn && r.WorkedOn <= PayPrdEnd && r.InvoiceId == null).ToList().ForEach(dl => _db.Entry(dl).State = EntityState.Deleted); /*//tu: use State - not: SelectedFeed.DnLds.Remove(dl);*/      reconfirm(); }
    void onDeletePeriod_ALL(object s, RoutedEventArgs e) { _db.TimePerDays.Local.Where(r => PayPrdBgn <= r.WorkedOn && r.WorkedOn <= PayPrdEnd).ToList().ForEach(dl => _db.Entry(dl).State = EntityState.Deleted); /*//tu: use State - not: SelectedFeed.DnLds.Remove(dl);*/      reconfirm(); }

    void reconfirm()
    {
      if (MessageBox.Show(_db.GetDbChangesReport(), "Save?", MessageBoxButton.YesNo, MessageBoxImage.Question) != MessageBoxResult.Yes)
      {
        _db = A0DbContext.Create();
        InfoMessg = _db.GetDbChangesReport() + "  <== New DB context now!";
      }
      else
      {
        InfoMessg = _db.TrySaveReport().report;
        movePayPrd(-1);
      }
    }

    void OnClose(object sender, RoutedEventArgs e)
    {
      Close();
    }
  }
}
///todo:
///	db ini creation disregarding/discontaining SQLEXPRESS word in the con.str. (Jul19).
///	