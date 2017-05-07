using System;
using System.Threading.Tasks;
using Microsoft.Practices.Unity;
using Octopus.Client;
using OctopusNotify.App.Properties;
using OctopusNotify.App.Utilities;
using OctopusNotify.Stub;

namespace OctopusNotify.App.Ioc
{
    public abstract class Container
    {
        #region Fields
        protected IUnityContainer UnityContainer { get; } = new UnityContainer();
        public event EventHandler Configured;
        #endregion

        #region Properties
        public static Container Current
        {
            get; set;
        }
        #endregion

        #region Constructors
        protected Container()
        {
        }
        #endregion

        #region Public Methods
        public void Configure()
        {
            RegisterTypes();
            OnConfigured();
        }

        public Task ConfigureAsync()
        {
            return Task.Factory.StartNew(() => Configure());
        }

        public virtual T Resolve<T>()
        {
            return UnityContainer.Resolve<T>();
        }
        #endregion

        #region Private Methods
        protected abstract void RegisterTypes();

        private void OnConfigured()
        {
            Configured?.Invoke(this, EventArgs.Empty);
        }
        #endregion
    }
}
