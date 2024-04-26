using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Schnacc.UserInterface.Infrastructure.Commands
{
    public abstract class AsyncCommandBase<T> : ICommand
    {
        /// <summary>Occurs when changes occur that affect whether or not the command should execute. </summary>
        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;

            remove => CommandManager.RequerySuggested -= value;
        }

        protected abstract Task ExecuteAsync(T parameter);

        protected abstract bool CanExecuteTask(T parameter);

        bool ICommand.CanExecute(object parameter)
        {
            if (typeof(T).IsValueType && parameter == null)
            {
                return false;
            }

            return this.CanExecute((T)parameter);
        }

        void ICommand.Execute(object parameter)
        {
            if (typeof(T).IsValueType && parameter == null)
            {
                return;
            }

            this.Execute((T)parameter);
        }

        /// <summary>Gets a value indicating whether the command can execute in its current state. </summary>
        /// <param name="parameter">The parameter for the command.</param>
        /// <returns>A value indicating whether the command can execute in its current state.</returns>
        private bool CanExecute(T parameter) => this.CanExecuteTask(parameter);

        /// <summary>Defines the method to be called when the command is invoked. </summary>
        /// <param name="parameter">The parameter for the command.</param>
        private async void Execute(T parameter)
        {
            if (!this.CanExecuteTask(parameter))
            {
                return;
            }

            this.RaiseCanExecuteChanged();

            try
            {
                await this.ExecuteAsync(parameter);
            }
            finally
            {
                this.RaiseCanExecuteChanged();
            }
        }

        /// <summary>Triggers the CanExecuteChanged event</summary>
        private void RaiseCanExecuteChanged()
        {
            CommandManager.InvalidateRequerySuggested();
        }
    }
}