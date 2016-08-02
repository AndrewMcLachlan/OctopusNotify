using System;
using System.Windows.Input;
using OctopusNotify.App.Views;

namespace OctopusNotify.App.Commands
{
    public class ShowSettingsCommand : ICommand
    {
        private SettingsWindow _window;

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            if (_window != null)
            {
                _window.Activate();
                return;
            }

            _window = new SettingsWindow();
            _window.Closed += Window_Closed;
            _window.Show();
            CommandManager.InvalidateRequerySuggested();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            _window = null;
        }

        private void OnCanExceuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
