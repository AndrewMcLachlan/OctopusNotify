using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace OctopusNotify.App.ViewModels
{
    public abstract class ViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void Set<T>(ref T field, T value, bool validate = false, [CallerMemberName]string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return;

            field = value;
            OnPropertyChanged(propertyName, validate);
        }

        protected virtual void OnPropertyChanged(string propertyName, bool validate = false)
        {
            if (validate)
            {
                Validate();
            }

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public virtual void Validate()
        {
        }
    }
}
