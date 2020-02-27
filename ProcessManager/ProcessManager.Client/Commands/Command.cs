using ProcessManager.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace ProcessManager.Client.Commands
{
    public class Command : ICommand
    {
        private Action executeAction;
        private Func<bool> canExecuteAction;

        public event EventHandler CanExecuteChanged;

        public Command(Action executeAction, Func<bool> canExecuteAction = null)
        {
            this.executeAction = executeAction;
            this.canExecuteAction = canExecuteAction;
        }

        public bool CanExecute(object parameter)
        {
            return canExecuteAction?.Invoke() ?? true;
        }

        public void Execute(object parameter)
        {
            executeAction.Invoke();
        }
    }
}
