using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AsyncAwaitDemo.MVVM
{
    public class AsyncDelegateCommand : ICommand
    {
        protected readonly Predicate<object> _canExecute;
        protected Func<object, Task> _asyncExecute;

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public AsyncDelegateCommand(Func<object, Task> execute)
            : this(execute, null)
        {
        }

        public AsyncDelegateCommand(Func<object, Task> asyncExecute,
                       Predicate<object> canExecute)
        {
            this._asyncExecute = asyncExecute;
            this._canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            if (this._canExecute == null)
            {
                return true;
            }

            return this._canExecute(parameter);
        }

        public async void Execute(object parameter)
        {
            await this.ExecuteAsync(parameter);
        }

        protected virtual async Task ExecuteAsync(object parameter)
        {
            await this._asyncExecute(parameter);
        }
    }
}
