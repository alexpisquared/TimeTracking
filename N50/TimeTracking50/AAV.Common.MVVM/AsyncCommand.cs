﻿using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

public class AsyncCommand<TResult> : AsyncCommandBase, INotifyPropertyChanged
{
    readonly Func<CancellationToken, Task<TResult>> _command;
    readonly CancelAsyncCommand _cancelCommand;
    NotifyTaskCompletion<TResult> _execution;

    public AsyncCommand(Func<CancellationToken, Task<TResult>> command)
    {
        _command = command;
        _cancelCommand = new CancelAsyncCommand();
    }

    public override bool CanExecute(object parameter)
    {
        return Execution == null || Execution.IsCompleted;
    }

    public override async Task ExecuteAsync(object parameter)
    {
        _cancelCommand.NotifyCommandStarting();
        Execution = new NotifyTaskCompletion<TResult>(_command(_cancelCommand.Token));
        RaiseCanExecuteChanged();
        await Execution.TaskCompletion;
        _cancelCommand.NotifyCommandFinished();
        RaiseCanExecuteChanged();
    }

    public ICommand CancelCommand => _cancelCommand;

    public NotifyTaskCompletion<TResult> Execution
    {
        get { return _execution; }
        private set
        {
            _execution = value;
            OnPropertyChanged();
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;
    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

    sealed class CancelAsyncCommand : ICommand
    {
        CancellationTokenSource _cts = new CancellationTokenSource();
        bool _commandExecuting;

        public CancellationToken Token => _cts.Token;

        public void NotifyCommandStarting()
        {
            _commandExecuting = true;
            if (!_cts.IsCancellationRequested) return;
            _cts = new CancellationTokenSource();
            RaiseCanExecuteChanged();
        }

        public void NotifyCommandFinished()
        {
            _commandExecuting = false;
            RaiseCanExecuteChanged();
        }

        bool ICommand.CanExecute(object parameter)
        {
            return _commandExecuting && !_cts.IsCancellationRequested;
        }

        void ICommand.Execute(object parameter)
        {
            _cts.Cancel();
            RaiseCanExecuteChanged();
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        void RaiseCanExecuteChanged() => CommandManager.InvalidateRequerySuggested();
    }
}

public static class AsyncCommand
{
    public static AsyncCommand<object> Create(Func<Task> command) => new AsyncCommand<object>(async _ => { await command(); return null; });
    public static AsyncCommand<TResult> Create<TResult>(Func<Task<TResult>> command) => new AsyncCommand<TResult>(_ => command());
    public static AsyncCommand<object> Create(Func<CancellationToken, Task> command) => new AsyncCommand<object>(async token => { await command(token); return null; });
    public static AsyncCommand<TResult> Create<TResult>(Func<CancellationToken, Task<TResult>> command) => new AsyncCommand<TResult>(command);
}