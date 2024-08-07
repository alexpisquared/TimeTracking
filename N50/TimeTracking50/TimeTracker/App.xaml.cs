﻿namespace TimeTracker;
public partial class App : Application
{
  public static DateTime Today = DateTime.Today;
  public static DateTime AppStartAt = DateTime.Now;
  private static SpeechSynth? _synth = null; public static SpeechSynth Synth => _synth ??= new(new ConfigurationBuilder().AddUserSecrets<App>().Build()["AppSecrets:MagicSpeech"] ?? throw new ArgumentNullException("###################"), true, CC.EnusAriaNeural.Voice);
  public static async Task SpeakAsync(string msg) => await Synth.SpeakAsync(msg);
  public static void SpeakFaF(string msg) => Synth.SpeakFAF(msg);
  public static void SpeakFree(string msg) => Synth.SpeakFree(msg);

  protected override async void OnStartup(StartupEventArgs e)
  {
    try
    {
      base.OnStartup(e);                                                                                                                                                                                                  // /**/ await Task.Delay(333);
      Current.DispatcherUnhandledException += WpfUserControlLib.Helpers.UnhandledExceptionHndlrUI.OnCurrentDispatcherUnhandledException;

      EventManager.RegisterClassHandler(typeof(TextBox), UIElement.GotFocusEvent, new RoutedEventHandler((s, re) => { (s as TextBox)?.SelectAll(); })); //tu: TextBox                                                     // /**/ await Task.Delay(333);
      AAV.Sys.Helpers.Tracer.SetupTracingOptions("TimeTracker", new TraceSwitch("OnlyUsedWhenInConfig", "This is the trace for all               messages... but who cares?   See ScrSvr for a model.") { Level = TraceLevel.Verbose });  // /**/ await Task.Delay(333);
      ShutdownMode = ShutdownMode.OnExplicitShutdown;

#if !!TDD
      HolidayProcessor.Test();

    //TimeTracker.Common.Emailer.PerpAndShow("trgEmail", "subj", "body", "hardcopy");
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
      //Bpr.Click();
      new MainSwitchboard(e.Args.ToList().Contains("-audible"), "-scheduler -audible").ShowDialog();

      //Bpr.BeepEnd6();
      App.Current.Shutdown();
#endif
      await Task.Yield();
    }
    catch (Exception ex) { ex.Pop(); }
  }
}