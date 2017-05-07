using System;
using System.Threading;
using System.Windows;
using OctopusNotify.App.Ioc;
using OctopusNotify.App.Properties;
using OctopusNotify.App.Views;
using Serilog;

namespace OctopusNotify.App
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private Mutex _mutex;

        public void App_Startup(object sender, StartupEventArgs e)
        {
            _mutex = new Mutex(true, "b300f8b3-e9af-466f-a4a6-9f4cb545b5ad", out bool created);
            if (!created)
            {
                _mutex.Dispose();
                _mutex = null;
                Current.Shutdown();
                return;
            }

            Log.Logger = new LoggerConfiguration().ReadFrom.AppSettings().CreateLogger();

            if (!Settings.Default.Upgraded)
            {
                Settings.Default.Upgrade();
                if (!String.IsNullOrEmpty(Settings.Default.ApiKey))
                {
                    Settings.Default.UseApiKey = true;
                }
                Settings.Default.Upgraded = true;
                Settings.Default.Save();
            }

            Current.ShutdownMode = ShutdownMode.OnExplicitShutdown;

            Container.Current = new AppContainer();
            Container.Current.ConfigureAsync();

            NotifyIconWindow window = new NotifyIconWindow();
        }

        private void App_Exit(object sender, ExitEventArgs e)
        {
            if (_mutex != null)
            {
                _mutex.ReleaseMutex();
            }
        }
    }
}
