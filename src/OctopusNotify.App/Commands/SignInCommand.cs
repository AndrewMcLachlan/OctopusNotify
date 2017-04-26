using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using OctopusNotify.App.Views;

namespace OctopusNotify.App.Commands
{
    public class SignInCommand : ICommand
    {
        private Signin _window;

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

            _window = new Signin();
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
