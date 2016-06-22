using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;
using Octopus.Client;
using OctopusNotify.App.Properties;
using OctopusNotify.App.Utilities;
using OctopusNotify.Stub;

namespace OctopusNotify.App.Ioc
{
    public class Container
    {
        #region Fields
        private static Container _current = new Container();
        private IUnityContainer _unityContainer = new UnityContainer();
        public event EventHandler Configured;
        #endregion

        #region Properties
        public static Container Current
        {
            get
            {
                return _current;
            }
        }
        #endregion

        #region Constructors
        private Container()
        {
        }
        #endregion

        #region Public Methods
        public void Configure()
        {
            RegisterTypes();
            OnConfigured();
        }

        public T Resolve<T>()
        {
            return _unityContainer.Resolve<T>();
        }
        #endregion

        #region Private Methods
        private void RegisterTypes()
        {
#if STUB
            const string name = "Stub";
#else
            const string name = "Real";
#endif

            _unityContainer.RegisterType<OctopusServerEndpoint>(new[]
            {
                new InjectionConstructor(Settings.Default.ServerUrl, Settings.Default.ApiKey.Decrypt())
            });

            _unityContainer.RegisterType<IOctopusRepository, StubOctopusRepository>("Stub");

            _unityContainer.RegisterType<IOctopusRepository, OctopusRepository>("Real",new[]
            {
                new InjectionConstructor(new ResolvedParameter<OctopusServerEndpoint>())
            });

            _unityContainer.RegisterType<IDeploymentRepositoryAdapter, OctopusAdapter>(new[]
            {
                new InjectionConstructor(new ResolvedParameter<IOctopusRepository>(name), Settings.Default.PollingInterval * 1000d)
            });
        }

        private void OnConfigured()
        {
            Configured?.Invoke(this, EventArgs.Empty);
        }
        #endregion
    }
}
