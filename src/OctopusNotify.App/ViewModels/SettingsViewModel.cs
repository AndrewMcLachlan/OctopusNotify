using System;
using System.ComponentModel;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Win32;
using OctopusNotify.App.Properties;
using OctopusNotify.App.Utilities;
using Container = OctopusNotify.App.Ioc.Container;

namespace OctopusNotify.App.ViewModels
{

    public class SettingsViewModel : INotifyPropertyChanged
    {
        #region Constants
        private const string StartupRegistryPath = "SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run";
        private const string RegistryValueName = "OctopusNotify";
        #endregion

        #region Events
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        #region Fields
        private Uri _serverUrl;
        private bool _isValid;
        private bool _runOnStartup;

        private bool _alertOnFailedBuild;
        private bool _alertOnNewFailedBuild;
        private bool _alertOnFixedBuild;
        private bool _alertOnSuccessfulBuild;
        #endregion

        #region Properties
        public Uri ServerUrl
        {
            get
            {
                return _serverUrl;
            }

            set
            {
                _serverUrl = value;
                OnPropertyChanged(nameof(ServerUrl));
            }
        }

        public bool IsValid
        {
            get { return _isValid; }
            set
            {
                _isValid = value;
                OnPropertyChanged(nameof(IsValid));
            }

        }

        public bool RunOnStartup
        {
            get { return _runOnStartup; }
            set
            {
                _runOnStartup = value;
                OnPropertyChanged(nameof(RunOnStartup));
            }
        }

        public bool AlertOnFailedBuild
        {
            get { return _alertOnFailedBuild; }
            set
            {
                _alertOnFailedBuild = value;
                if (value) AlertOnNewFailedBuild = value;
                OnPropertyChanged(nameof(AlertOnFailedBuild));
            }
        }

        public bool AlertOnNewFailedBuild
        {
            get { return _alertOnNewFailedBuild; }
            set
            {
                _alertOnNewFailedBuild = value;
                if (!value) AlertOnFailedBuild = value;
                OnPropertyChanged(nameof(AlertOnNewFailedBuild));
            }
        }

        public bool AlertOnFixedBuild
        {
            get { return _alertOnFixedBuild; }
            set
            {
                _alertOnFixedBuild = value;
                if (!value) AlertOnSuccessfulBuild = value;
                OnPropertyChanged(nameof(AlertOnFixedBuild));
            }
        }

        public bool AlertOnSuccessfulBuild
        {
            get { return _alertOnSuccessfulBuild; }
            set
            {
                _alertOnSuccessfulBuild = value;
                if (value) AlertOnFixedBuild = value;
                OnPropertyChanged(nameof(AlertOnSuccessfulBuild));
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

            Settings.Default.Save();

            SetRunOnStartup();

            Container.Current.Configure();
        }
        #endregion

        #region Private Methods
        private void OnPropertyChanged(string propertyName)
        {
            if (propertyName != nameof(IsValid))
            {
                Validate();
            }

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
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
                    key.DeleteValue(RegistryValueName);
                }
            }
        }

        private void Validate()
        {
            IsValid = ServerUrl != null && !String.IsNullOrEmpty(ServerUrl.ToString());
        }
        #endregion
    }
}
