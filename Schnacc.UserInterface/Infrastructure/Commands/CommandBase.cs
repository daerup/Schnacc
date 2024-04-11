//-----------------------------------------------------------------------
// <copyright file="CommandBase.cs" company="MyToolkit">
//     Copyright (c) Rico Suter. All rights reserved.
// </copyright>
// <license>https://github.com/MyToolkit/MyToolkit/blob/master/LICENSE.md</license>
// <author>Rico Suter, mail@rsuter.com</author>
//-----------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Windows.Input;

namespace Schnacc.UserInterface.Infrastructure.Commands
{
#pragma warning disable SA1402 // File may only contain a single class

    /// <summary>Provides a base implementation of the <see cref="ICommand"/> interface. </summary>
    public abstract class CommandBase : ICommand
    {
        /// <summary>Occurs when changes occur that affect whether or not the command should execute. </summary>
        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;

            remove => CommandManager.RequerySuggested -= value;
        }

        void ICommand.Execute(object parameter)
        {
            this.Execute();
        }

        bool ICommand.CanExecute(object parameter) => this.CanExecute();

        /// <summary>Defines the method that determines whether the command can execute in its current state.</summary>
        /// <returns>true if this command can be executed; otherwise, false.</returns>
        protected abstract bool CanExecute();

        /// <summary>Defines the method to be called when the command is invoked. </summary>
        protected abstract void Execute();
    }

    /// <summary>Provides an implementation of the <see cref="ICommand"/> interface. </summary>
    /// <typeparam name="T">The type of the command parameter. </typeparam>
    public abstract class CommandBase<T> : ICommand
    {
        /// <summary>Occurs when changes occur that affect whether or not the command should execute. </summary>
        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;

            remove => CommandManager.RequerySuggested -= value;
        }

        [DebuggerStepThrough]
        bool ICommand.CanExecute(object parameter) => this.CanExecute((T)parameter);

        void ICommand.Execute(object parameter)
        {
            this.Execute((T)parameter);
        }

        /// <summary>Gets a value indicating whether the command can execute in its current state. </summary>
        /// <returns>true if this command can be executed; otherwise, false.</returns>
        /// <param name="parameter">Data used by the command.</param>
        [DebuggerStepThrough]
        protected abstract bool CanExecute(T parameter);

        /// <summary>
        /// Defines the method to be called when the command is invoked.
        /// </summary>
        /// <param name="parameter">Data used by the command.</param>
        protected abstract void Execute(T parameter);
    }
}