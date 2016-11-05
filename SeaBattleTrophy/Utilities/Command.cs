using System;
using System.Windows.Input;

namespace Utilities
{
    public class Command : ICommand
    {
        private Action _action;

        public Command(Action action)
        {
            _action = action;
        } 

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            _action.Invoke();
        }
    }
}
