using System;
using System.Configuration;
using System.Reflection;
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
        private Uri _serverUrl;
        private bool _isValid;
        private bool _canTest;
        private bool _runOnStartup;

        private bool _alertOnFailedBuild;
        private bool _alertOnNewFailedBuild;
        private bool _alertOnFixedBuild;
        private bool _alertOnSuccessfulBuild;

        private bool _alertOnGuidedFailure;
        private bool _alertOnManualStep;

        private int _pollingInterval;
        private int _balloonTimeout;
        #endregion

        #region Properties
        public Uri ServerUrl
        {
            get { return _serverUrl; }
            set { Set(ref _serverUrl, value); }
        }

        public bool IsValid
        {
            get { return _isValid; }
            set { Set(ref _isValid, value); }
        }

        public bool CanTest
        {
            get { return _canTest; }
            set { Set(ref _canTest, value); }
        }

        public bool RunOnStartup
        {
            get { return _runOnStartup; }
            set { Set(ref _runOnStartup, value); }
        }

        public bool DisableFailedBuildAlerts
        {
            get { return !_alertOnFailedBuild && !_alertOnNewFailedBuild; }
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
            get { return !_alertOnSuccessfulBuild && !_alertOnFixedBuild; }
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
            get { return _alertOnFailedBuild; }
            set { Set(ref _alertOnFailedBuild, value); }
        }

        public bool AlertOnNewFailedBuild
        {
            get { return _alertOnNewFailedBuild; }
            set { Set(ref _alertOnNewFailedBuild, value); }
        }

        public bool AlertOnFixedBuild
        {
            get { return _alertOnFixedBuild; }
            set { Set(ref _alertOnFixedBuild, value); }
        }

        public bool AlertOnSuccessfulBuild
        {
            get { return _alertOnSuccessfulBuild; }
            set { Set(ref _alertOnSuccessfulBuild, value); }
        }

        public bool AlertOnGuidedFailure
        {
            get { return _alertOnGuidedFailure; }
            set { Set(ref _alertOnGuidedFailure, value); }
        }

        public bool AlertOnManualStep
        {
            get { return _alertOnManualStep; }
            set { Set(ref _alertOnManualStep, value); }
        }

        public int PollingInterval
        {
            get { return _pollingInterval; }
            set { Set(ref _pollingInterval, value); }
        }

        public int BalloonTimeout
        {
            get { return _balloonTimeout; }
            set { Set(ref _balloonTimeout, value); }
        }

        public Uri ApiKeyUri
        {
            get
            {
                return String.IsNullOrEmpty(ConfigurationManager.AppSettings["doc:HowToCreateApiKey"]) ?
                    new Uri("http://docs.octopusdeploy.com/display/OD/How+to+create+an+API+key") :
                    new Uri(ConfigurationManager.AppSettings["doc:HowToCreateApiKey"]);
            }
        }
        #endregion

        #region Constructors
        public SettingsViewModel()
        {
            ServerUrl = String.IsNullOrWhiteSpace(Settings.Default.ServerUrl) ? null : new Uri(Settings.Default.ServerUrl);

            AlertOnNewFailedBuild = Settings.Default.AlertOnNewFailedBuild && !Settings.Default.AlertOnFailedBuild;
            AlertOnFailedBuild = Settings.Default.AlertOnFailedBuild;
            AlertOnFixedBuild = Settings.Default.AlertOnFixedBuild && !Settings.Default.AlertOnSuccessfulBuild;
            AlertOnSuccessfulBuild = Settings.Default.AlertOnSuccessfulBuild;

            DisableFailedBuildAlerts = !AlertOnFailedBuild && !AlertOnNewFailedBuild;
            DisableSuccessfulBuildAlerts = !AlertOnSuccessfulBuild && !AlertOnFixedBuild;

            AlertOnGuidedFailure = Settings.Default.AlertOnGuidedFailure;
            AlertOnManualStep = Settings.Default.AlertOnManualStep;

            PollingInterval = Settings.Default.PollingInterval;
            BalloonTimeout = Settings.Default.BalloonTimeout;

            RunOnStartup = GetRunOnStartup();

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
            Settings.Default.ServerUrl = _serverUrl == null ? null : _serverUrl.ToString();

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
        protected override void OnPropertyChanged(string propertyName)
        {
            if (propertyName != nameof(IsValid) && propertyName != nameof(CanTest))
            {
                Validate();
            }

            base.OnPropertyChanged(propertyName);
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

        public void Validate()
        {
            CanTest = ServerUrl != null && !String.IsNullOrEmpty(ServerUrl.ToString());
            IsValid = ServerUrl != null && !String.IsNullOrEmpty(ServerUrl.ToString()) && PollingInterval > 0 && BalloonTimeout > 0;
        }
        #endregion
    }
}
