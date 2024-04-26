using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Schnacc.UserInterface.Infrastructure.Commands
{
#pragma warning disable SA1402 // File may only contain a single class

    /// <summary>Provides an implementation of the <see cref="ICommand"/> interface. </summary>
    /// <typeparam name="TParameter">The type of the command parameter. </typeparam>
    public class AsyncRelayCommand<TParameter> : AsyncCommandBase<TParameter>
    {
        private readonly Func<TParameter, Task> execute;

        private readonly Predicate<TParameter> canExecute;

        /// <summary>Initializes a new instance of the <see cref="AsyncRelayCommand{TParameter}"/> class. </summary>
        /// <param name="execute">The function. </param>
        public AsyncRelayCommand(Func<TParameter, Task> execute)
            : this(execute, param => true)
        {
        }

        /// <summary>Initializes a new instance of the <see cref="AsyncRelayCommand{TParameter}"/> class. </summary>
        /// <param name="execute">The function. </param>
        /// <param name="canExecute">The predicate to check whether the function can be executed. </param>
        private AsyncRelayCommand(Func<TParameter, Task> execute, Predicate<TParameter> canExecute)
        {
            this.execute = execute ?? throw new ArgumentNullException(nameof(execute));
            this.canExecute = canExecute;
        }

        protected override Task ExecuteAsync(TParameter parameter) => this.execute(parameter);

        protected override bool CanExecuteTask(TParameter parameter) => this.canExecute(parameter);
    }
}