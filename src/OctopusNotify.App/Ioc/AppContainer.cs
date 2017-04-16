using Microsoft.Practices.Unity;
using Octopus.Client;
using OctopusNotify.App.Properties;
using OctopusNotify.App.Utilities;
using OctopusNotify.Stub;

namespace OctopusNotify.App.Ioc
{
    public class AppContainer : Container
    {
        protected override void RegisterTypes()
        {
#if STUB
            const string name = "Stub";
#else
            const string name = "Real";
#endif

            UnityContainer.RegisterType<OctopusServerEndpoint>(new[]
            {
                new InjectionConstructor(Settings.Default.ServerUrl, Settings.Default.ApiKey.Decrypt())
            });

            UnityContainer.RegisterType<IOctopusRepository, StubOctopusRepository>("Stub");

            UnityContainer.RegisterType<IOctopusRepository, OctopusRepository>("Real", new[]
            {
                new InjectionConstructor(new ResolvedParameter<OctopusServerEndpoint>())
            });

            UnityContainer.RegisterType<IDeploymentRepositoryAdapter, OctopusAdapter>(new[]
            {
                new InjectionConstructor(new ResolvedParameter<IOctopusRepository>(name), Settings.Default.PollingInterval * 1000d)
            });

#if STUB
            UnityContainer.RegisterType<IConnectionTester, StubConnectionTester>();
#else
            UnityContainer.RegisterType<IConnectionTester, ConnectionTester>(new[]
            {
                new InjectionConstructor(Settings.Default.ServerUrl, Settings.Default.ApiKey.Decrypt())
            });
#endif
        }
    }
}
