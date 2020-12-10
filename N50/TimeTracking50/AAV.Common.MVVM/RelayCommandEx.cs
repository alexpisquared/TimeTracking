using System;
using System.Diagnostics;
using System.Windows.Input;

namespace MVVM.Common
{
    public class RelayCommand : ICommand //todo: https://youtu.be/ysWK4e2Mtco?t=1122 (Oct2017: for UWP mostly use DelegateCommand)
    {
        readonly Action<object> _execute;
        readonly Predicate<object> _canExecute;

        public RelayCommand(Action<object> execute) : this(execute, null) { } /// Creates a new command that can always execute.

        public RelayCommand(Action<object> execute, Predicate<object> canExecute)
        {
            _execute = execute ?? throw new ArgumentNullException("execute");
            _canExecute = canExecute;
        }

        //see MSDN:   MvvmDemoApp [mag200902MVVM].ziP https://msdn.microsoft.com/en-us/magazine/dd419663.aspx ... + MSDN InputBindings.
        public RelayCommand(Action<object> execute, Key gestureKey, ModifierKeys gestureModifier = ModifierKeys.None) : this(execute)
        {
            GestureKey = gestureKey;
            GestureModifier = gestureModifier;
        }
        public RelayCommand(Action<object> execute, Predicate<object> canExecute, Key gestureKey, ModifierKeys gestureModifier = ModifierKeys.None) : this(execute, canExecute)
        {
            GestureKey = gestureKey;
            GestureModifier = gestureModifier;
        }
    public RelayCommand(Action<object> execute, Predicate<object> canExecute, Key gestureKey, ModifierKeys gestureModifier, MouseAction mouseGesture) : this(execute, canExecute, gestureKey, gestureModifier) => MouseGesture = mouseGesture;

    [DebuggerStepThrough]
        public bool CanExecute(object parameter) => _canExecute == null ? true : _canExecute(parameter);
        public void Execute(object parameter) => _execute(parameter);

        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }

        //Ex part:  Specify the keys and mouse actions that invoke the command. 
        public Key GestureKey { get; set; }
        public ModifierKeys GestureModifier { get; set; }
        public MouseAction MouseGesture { get; set; }
    }
} 