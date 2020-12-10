#define brave 
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace MVVM.Common
{
  public abstract class BindableBaseViewModel : BindableBase
  {
    static TraceSwitch appTraceLevel_Lcl = new TraceSwitch("Display nanme","Descr-n") { Level = TraceLevel.Error };

    protected BindableBaseViewModel()
    {
      CloseAppCmd = new RelayCommand(x => OnRequestClose(), param => CanClose())
      {
#if DEBUG
        GestureKey = Key.Escape
#else
        GestureKey = Key.F4,
        GestureModifier = ModifierKeys.Alt
#endif
      };
    }

    protected virtual void AutoExec() => Trace.WriteLineIf(appTraceLevel_Lcl.TraceVerbose,"AutoExec()");
    protected virtual void AutoExecSynch() => Trace.WriteLineIf(appTraceLevel_Lcl.TraceVerbose,"AutoExecSynch()");
    protected virtual async Task AutoExecAsync() { Trace.WriteLineIf(appTraceLevel_Lcl.TraceVerbose,"AutoExecAsync()"); await Task.Delay(1); }

    public ICommand CloseAppCmd { get; }

    public event EventHandler RequestClose;
    protected virtual void OnRequestClose() => RequestClose?.Invoke(this, EventArgs.Empty);
    protected virtual bool CanClose() => true;
    protected virtual async Task ClosingVM() => await Task.Delay(99);  // public abstract void Closing();

    protected static bool _cancelClosing = false;
    public static void CloseEvent(Window view, BindableBaseViewModel vwMdl)
    {
      async void handler(object sender, EventArgs e)
      {
        await vwMdl.ClosingVM();
        if (_cancelClosing) return;

        vwMdl.RequestClose -= handler;
        try
        {
          if (view != null)
          {
            if (Application.Current.Dispatcher.CheckAccess()) // if on UI thread							
              view.Close();
            else
              await Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(() => view.Close()));
          }
        }
        catch (Exception ex) { Trace.WriteLine(ex.Message, System.Reflection.MethodInfo.GetCurrentMethod().Name); if (Debugger.IsAttached) Debugger.Break(); }
      } // When the ViewModel asks to be closed, close the window.

      vwMdl.RequestClose += handler;

      ////org: replaced by code cleanup on Jan 2019.
      //EventHandler handler = null; // When the ViewModel asks to be closed, close the window.
      //handler = async delegate
      //{
      //  await vwMdl.ClosingVM();
      //  if (_cancelClosing) return;

      //  vwMdl.RequestClose -= handler;
      //  try
      //  {
      //    if (view != null)
      //    {
      //      if (Application.Current.Dispatcher.CheckAccess()) // if on UI thread							
      //        view.Close();
      //      else
      //        await Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(() => view.Close()));
      //    }
      //  }
      //  catch (Exception ex) { Trace.WriteLine(ex.Message, System.Reflection.MethodInfo.GetCurrentMethod().Name); if (Debugger.IsAttached) Debugger.Break(); }
      //};
      //vwMdl.RequestClose += handler;


    }

    public static void ShowMvvm(BindableBaseViewModel vMdl, Window view)
    {
      view.DataContext = vMdl;
      BindableBaseViewModel.CloseEvent(view, vMdl);
      Task.Run(() => vMdl.AutoExec()); //			await vMdl.AutoExec();
      view.Show();
    }
    public static bool? ShowModalMvvm(BindableBaseViewModel vMdl, Window view, Window owner = null)
    {
      if (owner != null)
      {
        view.Owner = owner;
        view.WindowStartupLocation = WindowStartupLocation.CenterOwner;
      }

      view.DataContext = vMdl;
      BindableBaseViewModel.CloseEvent(view, vMdl);

#if brave
      autoexecSafe(vMdl);
#else
            autoexecSafe(vMdl);
#endif

      return view.ShowDialog();
    }


    public static bool? ShowModalMvvmAsync(BindableBaseViewModel vMdl, Window view, Window owner = null) // public static async Task<bool?> ShowModalMvvmAsync(BindableBaseViewModel vMdl, Window view, Window owner = null)
    {
      if (owner != null)
      {
        view.Owner = owner;
        view.WindowStartupLocation = WindowStartupLocation.CenterOwner;
      }

      view.DataContext = vMdl;
      CloseEvent(view, vMdl);

#if brave
      Task.Run(async () => await autoexecAsyncSafe(vMdl)); //todo: 
#else
            await vMdl.AutoExecAsync(); //todo:             
#endif

      return view.ShowDialog();
    }

    static void autoexecSafe(BindableBaseViewModel vMdl)
    {
      try
      {
        if (Application.Current.Dispatcher.CheckAccess()) // if on UI thread
          vMdl.AutoExec();
        else
          Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(() => //todo: rejoin properly to the UI thread (Oct 2017)
          vMdl.AutoExec()));
      }
      catch (Exception ex) { Trace.WriteLine(ex.Message, System.Reflection.MethodInfo.GetCurrentMethod().Name); if (Debugger.IsAttached) Debugger.Break(); }
    }
    static async Task autoexecAsyncSafe(BindableBaseViewModel vMdl)
    {
      try
      {
        if (Application.Current.Dispatcher.CheckAccess()) // if on UI thread
          await vMdl.AutoExecAsync();
        else
          await Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(async () => //todo: rejoin properly to the UI thread (Oct 2017)
          await vMdl.AutoExecAsync()));
      }
      catch (Exception ex) { Trace.WriteLine(ex.Message, System.Reflection.MethodInfo.GetCurrentMethod().Name); if (Debugger.IsAttached) Debugger.Break(); }
    }

    protected static async Task refreshUi() => await Application.Current.Dispatcher.BeginInvoke(new ThreadStart(() => refreshUiSynch()));
    protected static void refreshUiSynch() => CommandManager.InvalidateRequerySuggested();  //tu: Sticky UI state fix for MVVM (May2015)
  }
}
