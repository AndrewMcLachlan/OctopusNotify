using System;
using System.Configuration;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Microsoft.Win32;
using OctopusNotify.App.Properties;
using OctopusNotify.App.Utilities;
using Container = OctopusNotify.App.Ioc.Container;

namespace OctopusNotify.App.ViewModels
{

    public class SettingsViewModel : ViewModel
    {
        #region Constants
        private const string StartupRegistryPath = "SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run";
        private const string RegistryValueName = "OctopusNotify";
        #endregion

        #region Fields
        //private Uri _serverUrl;
        //private bool _useApiKey;
        private bool _canSetApiKey;
        private bool _isValid;
        private bool _canTest;
        private bool _runOnStartup;

        //private bool _alertOnFailedBuild;
      //  private bool _alertOnNewFailedBuild;
    //    private bool _alertOnFixedBuild;
  //      private bool _alertOnSuccessfulBuild;
//
       // private bool _alertOnGuidedFailure;
        //private bool _alertOnManualStep;

        //private int _pollingInterval;
        //private int _balloonTimeout;
        #endregion

        #region Properties
        public Uri ServerUrl
        {
            get => String.IsNullOrWhiteSpace(Settings.Default.ServerUrl) ? null : new Uri(Settings.Default.ServerUrl);
            set
            {
                Settings.Default.ServerUrl = value?.ToString();
                SaveAndNotify();
            }
        }

        public string ApiKey
        {
            get
            {
                return ApiKeyChanged ? Settings.Default.ApiKey.Decrypt() : InitialApiKey;
            }
            set
            {
                if (!ApiKeyChanged) return;
                Settings.Default.ApiKey = value?.Encrypt();
                Settings.Default.Save();
            }
        }

        public bool UseApiKey
        {
            get => Settings.Default.UseApiKey;
            set
            {
                Settings.Default.UseApiKey = value;
                SaveAndNotify();
                CanSetApiKey = value;
            }
        }

        public bool CanSetApiKey
        {
            get => _canSetApiKey;
            set => Set(ref _canSetApiKey, value, false);
        }

        public bool IsValid
        {
            get => _isValid;
            set => Set(ref _isValid, value, false);
        }

        public bool CanTest
        {
            get => _canTest;
            set => Set(ref _canTest, value, false);
        }

        public bool RunOnStartup
        {
            get
            {
                using (RegistryKey key = Registry.CurrentUser.OpenSubKey(StartupRegistryPath, false))
                {
                    return key.GetValue(RegistryValueName) != null;
                }
            }
            set
            {
                Set(ref _runOnStartup, value);
                using (RegistryKey key = Registry.CurrentUser.OpenSubKey(StartupRegistryPath, true))
                {
                    if (_runOnStartup)
                    {
                        key.SetValue(RegistryValueName, Assembly.GetExecutingAssembly().Location);
                    }
                    else
                    {
                        key.DeleteValue(RegistryValueName, false);
                    }
                }
            }
        }

        public bool DisableFailedBuildAlerts
        {
            get => !AlertOnFailedBuild && !AlertOnNewFailedBuild;
            set
            {
                if (value)
                {
                    AlertOnFailedBuild = false;
                    AlertOnNewFailedBuild = false;
                }
            }
        }

        public bool DisableSuccessfulBuildAlerts
        {
            get => !AlertOnSuccessfulBuild && !AlertOnFixedBuild;
            set
            {
                if (value)
                {
                    AlertOnSuccessfulBuild = false;
                    AlertOnFixedBuild = false;
                }
            }
        }

        public bool AlertOnFailedBuild
        {
            get => Settings.Default.AlertOnFailedBuild;
            set
            {
                Settings.Default.AlertOnFailedBuild = value;
                SaveAndNotify();
            }
        }

        public bool AlertOnNewFailedBuild
        {
            get => Settings.Default.AlertOnNewFailedBuild && !Settings.Default.AlertOnFailedBuild;
            set
            {
                if (value)
                {
                    Settings.Default.AlertOnFailedBuild = false;
                }
                Settings.Default.AlertOnNewFailedBuild = value;
                SaveAndNotify();
            }
        }

        public bool AlertOnFixedBuild
        {
            get => Settings.Default.AlertOnFixedBuild && !Settings.Default.AlertOnSuccessfulBuild;
            set
            {
                if (value)
                {
                    Settings.Default.AlertOnSuccessfulBuild = false;
                }
                Settings.Default.AlertOnFixedBuild = value;
                SaveAndNotify();
            }
        }

        public bool AlertOnSuccessfulBuild
        {
            get => Settings.Default.AlertOnSuccessfulBuild;
            set
            {
                Settings.Default.AlertOnSuccessfulBuild = value;
                SaveAndNotify();
            }
        }

        public bool AlertOnGuidedFailure
        {
            get => Settings.Default.AlertOnGuidedFailure;
            set
            {
                Settings.Default.AlertOnGuidedFailure = value;
                SaveAndNotify();
            }
        }

        public bool AlertOnManualStep
        {
            get => Settings.Default.AlertOnManualStep;
            set
            {
                Settings.Default.AlertOnManualStep = value;
                SaveAndNotify();
            }
        }

        public int PollingInterval
        {
            get => Settings.Default.PollingInterval;
            set
            {
                Settings.Default.PollingInterval = value;
                SaveAndNotify();
            }
        }

        public int BalloonTimeout
        {
            get => Settings.Default.BalloonTimeout;
            set
            {
                Settings.Default.BalloonTimeout = value;
                SaveAndNotify();
            }
        }

        public string InitialApiKey
        {
            get; set;
        }

        public Uri ApiKeyUri
        {
            get
            {
                return String.IsNullOrEmpty(ConfigurationManager.AppSettings["doc:HowToCreateApiKey"]) ?
                    new Uri("https://octopus.com/docs/how-to/how-to-create-an-api-key") :
                    new Uri(ConfigurationManager.AppSettings["doc:HowToCreateApiKey"]);
            }
        }

        public bool ApiKeyChanged
        {
            get;set;
        }
        #endregion

        #region Constructors
        public SettingsViewModel()
        {
            //ServerUrl = String.IsNullOrWhiteSpace(Settings.Default.ServerUrl) ? null : new Uri(Settings.Default.ServerUrl);
            //
            //AlertOnNewFailedBuild = Settings.Default.AlertOnNewFailedBuild && !Settings.Default.AlertOnFailedBuild;
            //AlertOnFailedBuild = Settings.Default.AlertOnFailedBuild;
            //AlertOnFixedBuild = Settings.Default.AlertOnFixedBuild && !Settings.Default.AlertOnSuccessfulBuild;
            //AlertOnSuccessfulBuild = Settings.Default.AlertOnSuccessfulBuild;

            DisableFailedBuildAlerts = !AlertOnFailedBuild && !AlertOnNewFailedBuild;
            DisableSuccessfulBuildAlerts = !AlertOnSuccessfulBuild && !AlertOnFixedBuild;

            //AlertOnGuidedFailure = Settings.Default.AlertOnGuidedFailure;
            //AlertOnManualStep = Settings.Default.AlertOnManualStep;

            //PollingInterval = Settings.Default.PollingInterval;
            //BalloonTimeout = Settings.Default.BalloonTimeout;

            //UseApiKey = Settings.Default.UseApiKey;

            //RunOnStartup = GetRunOnStartup();

            if (!String.IsNullOrEmpty(Settings.Default.ApiKey))
            {
                UseApiKey = true;
                InitialApiKey = "●●●●●●●●●●●●";
            }

            Validate();
        }
        #endregion

        #region Public Methods
        public async Task<(bool, string)> Test(string apiKey)
        {
            IConnectionTester tester = Container.Current.Resolve<IConnectionTester>();
            return await tester.Test(ServerUrl, apiKey);
        }

        public void Save(string apiKey)
        {
            Settings.Default.ApiKey = apiKey.Encrypt();

            Save();
        }

        public void Save()
        {
            //Settings.Default.ServerUrl = _serverUrl == null ? null : _serverUrl.ToString();

            Settings.Default.AlertOnFailedBuild = AlertOnFailedBuild;
            Settings.Default.AlertOnNewFailedBuild = AlertOnNewFailedBuild;
            Settings.Default.AlertOnFixedBuild = AlertOnFixedBuild;
            Settings.Default.AlertOnSuccessfulBuild = AlertOnSuccessfulBuild;

            Settings.Default.AlertOnGuidedFailure = AlertOnGuidedFailure;
            Settings.Default.AlertOnManualStep = AlertOnManualStep;

            Settings.Default.PollingInterval = PollingInterval;
            Settings.Default.BalloonTimeout = BalloonTimeout;

            Settings.Default.Save();

            SetRunOnStartup();

            Container.Current.Configure();
        }
        #endregion

        #region Private Methods
        protected void SaveAndNotify([CallerMemberName]string propertyName = null, bool validate = true)
        {
            Settings.Default.Save();
            Container.Current.Configure();
            OnPropertyChanged(propertyName, validate);
        }

        private bool GetRunOnStartup()
        {
            using (RegistryKey key = Registry.CurrentUser.OpenSubKey(StartupRegistryPath, false))
            {
                return key.GetValue(RegistryValueName) != null;
            }
        }

        private void SetRunOnStartup()
        {
            using (RegistryKey key = Registry.CurrentUser.OpenSubKey(StartupRegistryPath, true))
            {
                if (RunOnStartup)
                {
                    key.SetValue(RegistryValueName, Assembly.GetExecutingAssembly().Location);
                }
                else
                {
                    key.DeleteValue(RegistryValueName, false);
                }
            }
        }

        public override void Validate()
        {
            CanTest = ServerUrl != null && !String.IsNullOrEmpty(ServerUrl.ToString());
            IsValid = ServerUrl != null && !String.IsNullOrEmpty(ServerUrl.ToString()) && PollingInterval > 0 && BalloonTimeout > 0;
        }
        #endregion
    }
}
