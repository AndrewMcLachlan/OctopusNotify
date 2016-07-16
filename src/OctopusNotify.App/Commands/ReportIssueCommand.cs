using System;
using System.Windows.Input;

namespace OctopusNotify.App.Commands
{
    public class ReportIssueCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        private readonly static string IssueUri = "https://github.com/AndrewMcLachlan/OctopusNotify/issues/new";

        private bool _canExecute = false;

        public ReportIssueCommand()
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
            return !String.IsNullOrEmpty(IssueUri);
        }

        public void Execute(object parameter)
        {
            System.Diagnostics.Process.Start(IssueUri);
        }

        private void OnCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
