using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using OctopusNotify.App.Properties;
using OctopusNotify.App.Views;

namespace OctopusNotify.App.Commands
{
    public class SignInCommand : ICommand
    {
        private Signin _window;

        public event EventHandler CanExecuteChanged;

        private bool _canExecute = false;

        public SignInCommand()
        {
            CommandManager.RequerySuggested += CommandManager_RequerySuggested;
        }

        public bool CanExecute(object parameter)
        {
            return !String.IsNullOrWhiteSpace(Settings.Default.ServerUrl) && !Settings.Default.UseApiKey;
        }

        public void Execute(object parameter)
        {
            if (_window != null)
            {
                _window.Activate();
                return;
            }

            _window = new Signin();
            _window.Closed += Window_Closed;
            _window.Show();
            CommandManager.InvalidateRequerySuggested();
        }

        private void CommandManager_RequerySuggested(object sender, EventArgs e)
        {
            var result = CanExecute(null);
            if (_canExecute != result)
            {
                _canExecute = result;
                OnCanExecuteChanged();
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            _window = null;
        }

        private void OnCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
