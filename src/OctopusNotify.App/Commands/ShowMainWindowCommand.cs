using System;
using System.Windows.Input;

namespace OctopusNotify.App.Commands
{
    public class ShowMainWindowCommand : ICommand
    {
        private static MainWindow window = new MainWindow()
        {
            Visibility = System.Windows.Visibility.Hidden,
            WindowState = System.Windows.WindowState.Minimized,
        };

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return window.Visibility == System.Windows.Visibility.Hidden;
        }

        public void Execute(object parameter)
        {
            window.Show();
            window.WindowState = System.Windows.WindowState.Normal;
            window.Focus();
        }

        private void OnCanExceuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
