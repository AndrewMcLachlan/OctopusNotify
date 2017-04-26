using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using OctopusNotify.App.Ioc;
using OctopusNotify.App.Views;

namespace OctopusNotify.App.Commands
{
    public class SignOutCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            Container.Current.Resolve<IDeploymentRepositoryAdapter>().SignOut();
            CommandManager.InvalidateRequerySuggested();
        }

        private void OnCanExceuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
