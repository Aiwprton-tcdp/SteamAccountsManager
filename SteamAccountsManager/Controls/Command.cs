using System;
using System.Windows.Input;

namespace SteamAccountsManager.Controls;

internal class Command<T> : ICommand
{
    readonly Action<T> _execute;
    readonly Predicate<T> _canExecute;

    
    public Command(Action<T> execute) : this(execute, null)
    {
        _execute = execute;
    }

    public Command(Action<T> execute, Predicate<T> canExecute = null)
    {
        _execute = execute;
        _canExecute = canExecute;
    }


    public event EventHandler CanExecuteChanged
    {
        add => CommandManager.RequerySuggested += value;
        remove => CommandManager.RequerySuggested -= value;
    }

    public bool CanExecute(object parameter) =>
        _canExecute == null || _canExecute((T)parameter);

    public void Execute(object parameter) =>
        _execute((T)parameter);
}
