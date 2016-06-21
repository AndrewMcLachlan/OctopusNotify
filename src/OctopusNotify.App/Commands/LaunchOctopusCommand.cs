using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using OctopusNotify.App.Properties;

namespace OctopusNotify.App.Commands
{
    public class LaunchOctopusCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        private bool _canExecute = false;

        public LaunchOctopusCommand()
        {
            CommandManager.RequerySuggested += CommandManager_RequerySuggested;
        }

        private void CommandManager_RequerySuggested(object sender, EventArgs e)
        {
            var result = CanExecute(null);
            if (_canExecute != result)
            {
                OnCanExecuteChanged();
            }
        }

        public bool CanExecute(object parameter)
        {
            return !String.IsNullOrEmpty(Settings.Default.ServerUrl.ToString());
        }

        public void Execute(object parameter)
        {
            System.Diagnostics.Process.Start(Settings.Default.ServerUrl.ToString());
        }

        private void OnCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
