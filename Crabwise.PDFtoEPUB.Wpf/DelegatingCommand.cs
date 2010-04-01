using System;
using System.Windows.Input;

namespace Crabwise.PDFtoEPUB.Wpf
{
    public sealed class DelegatingCommand : ICommand
    {
        readonly Func<object, bool> canExecuteDelegate;
        readonly Action<object> executeDelegate;

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public DelegatingCommand(Func<object, bool> canExecuteDelegate, Action<object> executeDelegate)
        {
            this.canExecuteDelegate = canExecuteDelegate;
            this.executeDelegate = executeDelegate;
        }

        public bool CanExecute(object parameter)
        {
            return canExecuteDelegate(parameter);
        }

        public void Execute(object parameter)
        {
            executeDelegate(parameter);
        }
    }
}
