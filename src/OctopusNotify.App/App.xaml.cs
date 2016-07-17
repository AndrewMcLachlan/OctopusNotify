using System;
using System.Threading;
using System.Windows;
using OctopusNotify.App.Ioc;
using OctopusNotify.App.Properties;
using Serilog;

namespace OctopusNotify.App
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public void App_Startup(object sender, StartupEventArgs e)
        {

            Log.Logger = new LoggerConfiguration().ReadFrom.AppSettings().CreateLogger();

            bool created;
            Mutex mutex = new Mutex(true, "b300f8b3-e9af-466f-a4a6-9f4cb545b5ad", out created);
            if (!created)
            {
                Current.Shutdown();
                return;
            }

            if (!Settings.Default.Upgraded)
            {
                Settings.Default.Upgrade();
                Settings.Default.Upgraded = true;
                Settings.Default.Save();
            }

            Current.ShutdownMode = ShutdownMode.OnExplicitShutdown;

            Container.Current.Configure();
        }
    }
}
