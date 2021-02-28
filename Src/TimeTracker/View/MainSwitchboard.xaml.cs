using AAV.Sys.Ext;
using AAV.Sys.Helpers;
using AsLink;
using Db.TimeTrack.DbModel;
using MVVM.Common;
using System;
using System.Data.Entity;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using TimeTracker.Common;
using TimeTracker.Properties;

namespace TimeTracker.View
{
  public partial class MainSwitchboard : AAV.WPF.Base.WindowBase
  {
    const int _zeroBasedBtnCnt = 4;
    bool _keepSaying;

    public MainSwitchboard(bool checkNewEmail, bool keepSaying, string msg = null)
    {
      InitializeComponent();

      _keepSaying = keepSaying;

      Title = $"Time Tracker - {VerHelper.CurVerStr(".NET 4.8")}";

      KeyDown += (s, e) =>
      {
        switch (e.Key)
        {
          case Key.Escape: onClose(null, null); break;
          case Key.Up:   /**/ Settings.Default.LastBtnNo = Settings.Default.LastBtnNo > 0                /**/ ? --Settings.Default.LastBtnNo : 0;                /**/ setDf(Settings.Default.LastBtnNo); break;
          case Key.Down: /**/ Settings.Default.LastBtnNo = Settings.Default.LastBtnNo < _zeroBasedBtnCnt /**/ ? ++Settings.Default.LastBtnNo : _zeroBasedBtnCnt; /**/ setDf(Settings.Default.LastBtnNo); break;
          default: break;
        }
      }; //tu:

      Task.Run(() => A0DbContext.Create().lkuJobCategories.LoadAsync()); // preload to ini the EF for faster loads in views.

      setDf(Settings.Default.LastBtnNo);

      Loaded += (s, e) => { Task.Run(() => { while (_keepSaying) App.SpeakSynch(msg); }); };

      CurVer.Text = $"{A0DbContext.Create().ServerDatabase()}\n{VerHelper.CurVerStr(".NET 4.8")}";

      DataContext = this;
    }
    DefaultSetting _settingS;   /**/ public DefaultSetting DSettngS { get => _settingS ?? (_settingS = A0DbContext.Create().DefaultSettings.FirstOrDefault()); set => _settingS = value; }

    void setDf(int cb)
    {
      TS.IsDefault = PP.IsDefault = Ic.IsDefault = HR.IsDefault = AR.IsDefault = false;
      switch (cb)
      {
        default: break;
        case 0: PP.IsDefault = true; break;
        case 1: TS.IsDefault = true; break;
        case 2: Ic.IsDefault = true; break;
        case 3: HR.IsDefault = true; break;
        case 4: AR.IsDefault = true; break;
      }
    }

    protected override void OnClosed(EventArgs e) => base.OnClosed(e);
    void onClose(object s, RoutedEventArgs e) { Close(); Application.Current.Shutdown(); }
    void onTS(object s, RoutedEventArgs e) { pre(); BindableBaseViewModel.ShowModalMvvm(new TimesheetPreviewVM(true), new TimeTracker.View.TimesheetPreview()); post(); }      //new TimeTracker.View.TimesheetPreview().ShowDialog();
    void onPP(object s, RoutedEventArgs e) { pre(); new FromTillCtgrTaskNote().ShowDialog(); post(); }
    void onIc(object s, RoutedEventArgs e) { pre(); new InvoicePreview(A0DbContext.Create()).ShowDialog(); post(); }
    void onHR(object s, RoutedEventArgs e) { pre(); { MessageBox.Show("The feature is under contruction\n\nPlease come back soon", ((Button)s).Content.ToString().Replace("_", ""), MessageBoxButton.OK, MessageBoxImage.Information); } post(); }
    void onAR(object s, RoutedEventArgs e) { pre(); { MessageBox.Show("The feature is under contruction\n\nPlease come back soon", ((Button)s).Content.ToString().Replace("_", ""), MessageBoxButton.OK, MessageBoxImage.Information); } post(); }
    void onST(object s, RoutedEventArgs e) { pre(); new OptionsManager().ShowDialog(); post(); }
    void onDI(object s, RoutedEventArgs e) { pre(); TimeTrackDbCtx_Code1st_DbInitializer.DbIni(); post(); }      // NO GO at Wk&Hm????????????????                                                      //TimeTrackDbCtx_Simple__DbInitializer.DbIni();		// OK at Wk&Hm... but wrong dbx

    void onNavigate(object s, System.Windows.Navigation.RequestNavigateEventArgs e)
    {
      try
      {
        var dir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), e.Uri.OriginalString);
        Process.Start(new ProcessStartInfo(dir)); e.Handled = true;
      }
      catch (Exception ex) { ex.Log(); }
    }

    void pre() { _keepSaying = false; Hide(); }                                     //  ctrlPanelOnMarket.IsEnabled = false; WindowState = WindowState.Minimized; scrooves up focusing on the new window.   Hide(); - invokes Close */ }
    void post() { Bpr.BeepClk(); new MainSwitchboard(false, false).ShowDialog(); }  //  ctrlPanelOnMarket.IsEnabled = true;  WindowState = WindowState.Normal; Show(); }//Task.Factory.StartNew(() => Thread.Sleep(100)).ContinueWith(_ => { Close(); }, TaskScheduler.FromCurrentSynchronizationContext()); }

    private void wnd_Loaded(object sender, RoutedEventArgs e) => Bpr.Begin1FaF();
  }
}
