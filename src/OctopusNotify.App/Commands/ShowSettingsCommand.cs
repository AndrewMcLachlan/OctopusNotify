using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace OctopusNotify.App.Commands
{
    public class ShowSettingsCommand : ICommand
    {
        private static SettingsWindow window;

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            //return window.Visibility != System.Windows.Visibility.Visible;
            return window == null;
        }

        public void Execute(object parameter)
        {
            /*
            window.Show();
            window.Focus();*/

            window = new SettingsWindow();
            window.ShowDialog();
            window = null;
            CommandManager.InvalidateRequerySuggested();
        }

        private void OnCanExceuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
