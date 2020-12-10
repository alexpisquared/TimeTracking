using AAV.Sys.Helpers;
using AAV.WPF.Helpers;
using System;
using System.Diagnostics;
using System.Linq;
using System.Speech.Synthesis;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using TimeTracker.View;

namespace TimeTracker
{
  public partial class App : Application
  {
    public static DateTime Today = DateTime.Today;
    public static DateTime AppStartAt = DateTime.Now;
    static SpeechSynthesizer _synth = null; public static SpeechSynthesizer Synth { get { if (_synth == null) { _synth = new SpeechSynthesizer(); _synth.SpeakAsyncCancelAll(); _synth.Rate = 6; _synth.Volume = 25; _synth.SelectVoiceByHints(gender: VoiceGender.Female); } return _synth; } }
    public static void SpeakSynch(string msg) => Synth.Speak(msg);
    public static void SpeakAsync(string msg) { Synth.SpeakAsyncCancelAll(); Synth.SpeakAsync(msg); }

    protected override async void OnStartup(StartupEventArgs e)
    {
      Application.Current.DispatcherUnhandledException += UnhandledExceptionHndlr.OnCurrentDispatcherUnhandledException;
      EventManager.RegisterClassHandler(typeof(TextBox), UIElement.GotFocusEvent, new RoutedEventHandler((s, re) => { (s as TextBox)?.SelectAll(); })); //tu: TextBox
      Tracer.SetupTracingOptions("TimeTracker", new TraceSwitch("OnlyUsedWhenInConfig", "This is the trace for all               messages... but who cares?   See ScrSvr for a model.") { Level = TraceLevel.Verbose });
      ShutdownMode = ShutdownMode.OnExplicitShutdown;
      base.OnStartup(e);

#if !!TDD
      TimeTracker.Common.Emailer.PerpAndShow("trgEmail", "subj", "body", "hardcopy");

      //TimeTracker.Common.HolidayProcessor.Test();
      //var exitCode = Emailer.PerpAndShow("pigida@live.com", "test: subj", "test: body", @"C:\temp\path.txt");
      //TimeTrackDbCtx_Code1st_DbInitializer.DbIni();   // NO GO at Wk&Hm????????????????
      //TimeTrackDbCtx_Simple__DbInitializer.DbIni();		// OK    at Wk&Hm... but wrong dbx

#if Ofc13_
            var isSuccess = await Emailer.SmtpSend("alex.pigida@sciex.com", "alex.pigida@sciex.com", "Subj", "Body");

            var sender = new Form1();
            sender.Start();
            sender.SmtpSend("alex@there.com", "Subject ABC", "Body");
            sender.Stop();
#endif
#else
      new MainSwitchboard(true, e.Args.ToList().Contains("-audible"), "-scheduler -audible").ShowDialog();

      Bpr.BeepEnd6();
      App.Current.Shutdown();
#endif
      await Task.Yield();
    }
  }
}