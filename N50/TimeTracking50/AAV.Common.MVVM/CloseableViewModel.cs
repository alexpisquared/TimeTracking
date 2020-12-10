using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using MVVM.Common;

namespace AAV.Common.MVVM.AsLink
{
    public abstract class CloseableViewModel : BindableBase, ICloseable
    {
        ICommand _CloseCmd;
        public ICommand CloseCmd
        {
            get
            {
                return _CloseCmd ?? (_CloseCmd = new RelayCommand(async x => await OnClosing(), param => CloseCanExecute())
                {
#if DEBUG
                    GestureKey = System.Environment.UserName.ToLower().StartsWith("a") ? Key.Escape : Key.F4,
                    GestureModifier = System.Environment.UserName.ToLower().StartsWith("a") ? ModifierKeys.None : ModifierKeys.Alt
#else
                    GestureKey = Key.F4, 
                    GestureModifier = ModifierKeys.Alt 
#endif
                });
            }
        }

        protected virtual bool CloseCanExecute() => true;

        /// <summary>
        /// Raised when CloseCmd is called. This event is async so returns Task instead of void in case of EventDelegate
        /// </summary>
        public event WindowClosingEventHandler Closing;

        protected virtual async Task OnClosing()
        {
            if (Closing != null) await Closing.Invoke(this, new CloseEventArgs());
        }

        public virtual async Task BeforeClosing(CloseEventArgs e) { await Task.Delay(99); }

        public virtual void Closed(ClosedEventArgs e) { }


        protected static async Task RefreshUiAsync() { await Application.Current.Dispatcher.BeginInvoke(new ThreadStart(RefreshUi)); }
        protected static void RefreshUi() { CommandManager.InvalidateRequerySuggested(); } //tu: Sticky UI state fix for MVVM (May2015)
    }
}
