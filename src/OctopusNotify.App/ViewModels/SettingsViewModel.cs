using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        private Uri _serverUrl;
        private bool _isValid;
        private bool _canTest;
        private bool _runOnStartup;

        private bool _alertOnFailedBuild;
        private bool _alertOnNewFailedBuild;
        private bool _alertOnFixedBuild;
        private bool _alertOnSuccessfulBuild;

        private int _intervalTime;
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

        public bool AlertOnFailedBuild
        {
            get { return _alertOnFailedBuild; }
            set
            {
                Set(ref _alertOnFailedBuild, value);
                if (value) AlertOnNewFailedBuild = value;
            }
        }

        public bool AlertOnNewFailedBuild
        {
            get { return _alertOnNewFailedBuild; }
            set
            {
                Set(ref _alertOnNewFailedBuild, value);
                if (!value) AlertOnFailedBuild = value;
            }
        }

        public bool AlertOnFixedBuild
        {
            get { return _alertOnFixedBuild; }
            set
            {
                Set(ref _alertOnFixedBuild, value);
                if (!value) AlertOnSuccessfulBuild = value;
            }
        }

        public bool AlertOnSuccessfulBuild
        {
            get { return _alertOnSuccessfulBuild; }
            set
            {
                Set(ref _alertOnSuccessfulBuild, value);
                if (value) AlertOnFixedBuild = value;
            }
        }

        public int IntervalTime
        {
            get { return _intervalTime; }
            set { Set(ref _intervalTime, value); }
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

            AlertOnFailedBuild = Settings.Default.AlertOnFailedBuild;
            AlertOnNewFailedBuild = Settings.Default.AlertOnNewFailedBuild;
            AlertOnFixedBuild = Settings.Default.AlertOnFixedBuild;
            AlertOnSuccessfulBuild = Settings.Default.AlertOnSuccessfulBuild;

            IntervalTime = Settings.Default.PollingInterval;

            RunOnStartup = GetRunOnStartup();

            Validate();
        }
        #endregion

        #region Public Methods
        public async Task<bool> Test(string apiKey)
        {
            ConnectionTester tester = new ConnectionTester();
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

            Settings.Default.PollingInterval = IntervalTime;

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
            IsValid = ServerUrl != null && !String.IsNullOrEmpty(ServerUrl.ToString()) && IntervalTime > 0;
        }
        #endregion
    }
}
