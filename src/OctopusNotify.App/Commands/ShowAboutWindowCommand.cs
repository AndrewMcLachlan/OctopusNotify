using System;
using System.Windows.Input;

namespace OctopusNotify.App.Commands
{
    public class ShowAboutWindowCommand : ICommand
    {
        private static AboutWindow window;

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            window = new AboutWindow();
            window.Show();
        }

        private void OnCanExceuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
