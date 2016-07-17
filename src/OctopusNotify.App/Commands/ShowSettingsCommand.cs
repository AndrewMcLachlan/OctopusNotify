using System;
using System.Windows.Input;

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
            _window.ShowDialog();
            _window = null;
            CommandManager.InvalidateRequerySuggested();
        }

        private void OnCanExceuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
