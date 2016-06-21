using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace OctopusNotify.App.Commands
{
    public class ShowStubWindowCommand : ICommand
    {
        private static Stub window = null;

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return window == null;
        }

        public void Execute(object parameter)
        {
            window = new Stub();
            window.ShowDialog();
            window = null;
        }

        private void OnCanExceuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
