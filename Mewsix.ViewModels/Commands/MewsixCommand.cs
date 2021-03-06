﻿using System;
using System.Windows.Input;

namespace Mewsix.ViewModels.Commands
{
    class MewsixCommand : ICommand
    {
        /// <summary>
        /// The action to execute
        /// </summary>
        private readonly Action _actionToExecute;

        /// <summary>
        /// The action to check if we can execute the command
        /// </summary>
        private readonly Func<object, bool> _checkCanExecute;

        /// <summary>
        /// Initialize a default <see cref="MewsixCommand"/>
        /// </summary>
        /// <param name="actionToExecute">The action to execute</param>
        /// <param name="actionToCheckExecute">The action who check if we can execute</param>
        public MewsixCommand(Action actionToExecute, Func<object, bool> actionToCheckExecute)
        {
            _actionToExecute = actionToExecute;
            _checkCanExecute = actionToCheckExecute;
        }




        /// <summary>
        /// Occurs when the <see cref="CanExecute(object)"/> changed
        /// </summary>
        /// 
        private event EventHandler _MyEvent;
        public event EventHandler CanExecuteChanged
        {
            add
            {
                /* WE ADD A COMMAND MANAGER AND ITS RequerySuggested
                 * EVENT WHICH IS GOING TO CHECK IF SOME CONDITIONS MAY 
                 * AFFECT THE POSSIBILITY OF THE COMMAND TO EXECUTE HENCE
                 * RE-EXECUTING THE COMMAND. */
                _MyEvent += value;
                if (_checkCanExecute != null) CommandManager.RequerySuggested += value;
            }
            remove
            {
                _MyEvent -= value;
                if (_checkCanExecute != null) CommandManager.RequerySuggested -= value;
            }
        }

        /// <summary>
        /// Check if we can execute the current command
        /// </summary>
        /// <param name="parameter">The parameter used to check the command</param>
        /// <returns>True if we can check the command, else false</returns>
        public bool CanExecute(object parameter)
        {
            return _checkCanExecute.Invoke(parameter);
        }

        /// <summary>
        /// Execute the command
        /// </summary>
        /// <param name="parameter">The parameter to pass to the command</param>
        public void Execute(object parameter)
        {
            _actionToExecute.Invoke();
        }

        public void RaiseCanExecute()
        {

            _MyEvent?.Invoke(this, new EventArgs());
        }
    }
}
