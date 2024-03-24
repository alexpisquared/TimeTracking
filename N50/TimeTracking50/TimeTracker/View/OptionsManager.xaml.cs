using System.Data.Entity.Validation;
using System.Windows.Data;

namespace TimeTracker.View;

public partial class OptionsManager : AAV.WPF.Base.WindowBase
{
  public OptionsManager()
  {
    InitializeComponent();
    KeyDown += (s, e) => { if (e.Key == Key.Escape) { Close(); } };
    Closing += Window_Closing;
    DataContext = this;

    if (!string.IsNullOrEmpty(Settings.Default.OptnVw))
    {
      var stgs = Serializer.LoadFromString<AppSettings>(Settings.Default.OptnVw) as AppSettings;
      if (stgs?.Window2 != null)
      {
        Top = stgs.Window2.windowTop;
        Left = stgs.Window2.windowLeft;
        //Width = stgs.Window2.windowWidth;
        //Height = stgs.Window2.windowHeight;
      }
    }
  }

  readonly Db.TimeTrack.DbModel.A0DbContext _dbxTimeTrack = A0DbContext.Create();
  public static readonly DependencyProperty InfoMsgProperty = DependencyProperty.Register("InfoMsg", typeof(string), typeof(OptionsManager), new PropertyMetadata(null));  public string InfoMsg { get => (string)GetValue(InfoMsgProperty); set => SetValue(InfoMsgProperty, value); }
  void correctAndSaveToDb()
  {
    try
    {
      InfoMsg = $"{_dbxTimeTrack.SaveChanges()} rows saved";
      App.SpeakFaF(InfoMsg);
    }
    catch (DbEntityValidationException ex)
    {
      foreach (var er in ex.EntityValidationErrors)
      {
        Debug.WriteLine(er.Entry.Entity.GetType(), "\t");
        foreach (var ve in er.ValidationErrors)
        {
          Debug.WriteLine(ve.ErrorMessage, "\t\t");
          _ = MessageBox.Show(ve.ErrorMessage);
        }
      }
    }
    catch (InvalidOperationException ex) { _ = MessageBox.Show(ex.ToString(), "InvalidOperationException has been thrown", MessageBoxButton.OK, MessageBoxImage.Error); }
    catch (Exception ex) { if (Debugger.IsAttached) Debugger.Break(); _ = MessageBox.Show(ex.ToString(), "Exception has been thrown", MessageBoxButton.OK, MessageBoxImage.Error); }
  }
  void Window_Loaded(object sender, RoutedEventArgs e)
  {
    _dbxTimeTrack.DefaultSettings.Load();
    _dbxTimeTrack.Invoicers.Load();
    _dbxTimeTrack.Invoicees.Load();
    _dbxTimeTrack.lkuJobCategories.Load();
    _dbxTimeTrack.lkuPayPeriodModes.Load();

    ((CollectionViewSource)this.FindResource("defaultSettingViewSource")).Source = _dbxTimeTrack.DefaultSettings.Local;
    ((CollectionViewSource)this.FindResource("invoicerViewSource")).Source = _dbxTimeTrack.Invoicers.Local;
    ((CollectionViewSource)this.FindResource("invoiceeViewSource")).Source = _dbxTimeTrack.Invoicees.Local;
    ((CollectionViewSource)this.FindResource("lkuJobCategoryViewSource")).Source = _dbxTimeTrack.lkuJobCategories.Local;
    ((CollectionViewSource)this.FindResource("lkuPayPeriodModeViewSource")).Source = _dbxTimeTrack.lkuPayPeriodModes.Local;
  }
  void Window_Closing(object? sender, System.ComponentModel.CancelEventArgs e0)
  {
    try
    {
      if (_dbxTimeTrack.ChangeTracker.Entries().Any(e => e.State is EntityState.Added or EntityState.Modified or EntityState.Deleted))
      {
        var question = "Would you like to save the changes?";
        var header = "Changes detected";
        switch (MessageBox.Show(question, header, MessageBoxButton.YesNoCancel, MessageBoxImage.Question))
        {
          case MessageBoxResult.Yes: correctAndSaveToDb(); break;
          case MessageBoxResult.No: break;
          case MessageBoxResult.Cancel: e0.Cancel = true; break;
        }
      }
    }
    catch (Exception ex) { _ = MessageBox.Show(ex.ToString()); }

    var stgs = (string.IsNullOrEmpty(Settings.Default.OptnVw) || null == (Serializer.LoadFromString<AppSettings>(Settings.Default.OptnVw) as AppSettings)) ? new AppSettings() : Serializer.LoadFromString<AppSettings>(Settings.Default.OptnVw) as AppSettings;
    stgs.Window2.windowTop = Top;
    stgs.Window2.windowLeft = Left;
    stgs.Window2.windowWidth = Width;
    stgs.Window2.windowHeight = Height;
    Settings.Default.OptnVw = Serializer.SaveToString(stgs);
    Settings.Default.Save();

    //new FromTillCtgrTaskNote().Show();
  }
  void btnSave_Click(object sender, RoutedEventArgs e) { correctAndSaveToDb(); Close(); }
  void btnQuit_Click(object sender, RoutedEventArgs e) { App.SpeakFaF("Changes - if any - not saved."); Close(); }
  void PayPeriodChanged(object sender, SelectionChangedEventArgs e) => App.SpeakFaF("Do not forget to adjust the pay period length.");
}