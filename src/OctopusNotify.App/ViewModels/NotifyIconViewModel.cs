﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Microsoft.Practices.Unity;
using OctopusNotify.App.Ioc;
using OctopusNotify.App.Models;
using OctopusNotify.App.Properties;
using OctopusNotify.App.Utilities;
using OctopusNotify.Model;

namespace OctopusNotify.App.Models
{
    public class NotifyIconViewModel : ViewModel
    {
        #region Fields
        private static readonly ImageSource ErrorNotifyIcon;
        private static readonly ImageSource DisconnectedNotifyIcon;
        private static readonly ImageSource ConnectedNotifyIcon;

        private IDeploymentRepositoryAdapter _adapter;

        private NotifyIconState _iconState;
        private ImageSource _notifyIcon;
        private string _stateSummary;

        private BlockingCollection<DeploymentResult> _notificationQueue = new BlockingCollection<DeploymentResult>();
        #endregion

        #region Events
        public event EventHandler<NotificationEventArgs> Notification;
        #endregion

        #region Properties
        public NotifyIconState IconState
        {
            get
            {
                return _iconState;
            }
            set
            {
                Set(ref _iconState, value);
            }
        }

        public ImageSource NotifyIcon
        {
            get
            {
                return _notifyIcon;
            }
            set
            {
                Set(ref _notifyIcon, value);
            }
        }

        public string StateSummary
        {
            get
            {
                return _stateSummary;
            }
            set
            {
                Set(ref _stateSummary, value);
            }
        }
        #endregion

        #region Constructors
        static NotifyIconViewModel()
        {
            DisconnectedNotifyIcon = new BitmapImage(new Uri("pack://application:,,,/OctopusNotify;component/NotifyIcons/Disconnected.ico"));
            ConnectedNotifyIcon = new BitmapImage(new Uri("pack://application:,,,/OctopusNotify;component/NotifyIcons/Connected.ico"));
            ErrorNotifyIcon = new BitmapImage(new Uri("pack://application:,,,/OctopusNotify;component/NotifyIcons/Red.ico"));
        }

        public NotifyIconViewModel()
        {
            if (String.IsNullOrEmpty(Settings.Default.ServerUrl.ToString()))
            {
                AppCommands.ShowSettings.Execute(null);
                //SettingsWindow window = new SettingsWindow();
                //window.ShowDialog();
                //window = null;
            }

            Container.Current.Configured += Container_Configured;

            CreateAdapter();
        }
        #endregion

        #region Event Handlers
        /// <summary>
        /// Handles changes to IOC configuration.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Container_Configured(object sender, EventArgs e)
        {
            CreateAdapter();
        }

        private void Adapter_ConnectionError(object sender, EventArgs e)
        {
            SetIconState(NotifyIconState.Disconnected);
        }

        private void Adapter_ConnectionRestored(object sender, EventArgs e)
        {
            SetIconState(NotifyIconState.Connected);
        }

        private void Adapter_ErrorsCleared(object sender, EventArgs e)
        {
            SetIconState(NotifyIconState.Connected);
        }

        private void Adapter_ErrorsFound(object sender, EventArgs e)
        {
            SetIconState(NotifyIconState.Error);
        }

        private void Adapter_DeploymentsChanged(object sender, DeploymentEventArgs e)
        {
            OnNotification(e.Items.Where(i => ShouldShowNotification(i)).Select(i => i.ToNotification()));
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Creates the adapter and starts listening for deployment events.
        /// </summary>
        private void CreateAdapter()
        {
            if (_adapter != null)
            {
                _adapter.StopPolling();
                _adapter.DeploymentsChanged -= Adapter_DeploymentsChanged;

                _adapter.ErrorsCleared -= Adapter_ErrorsCleared;
                _adapter.ErrorsFound -= Adapter_ErrorsFound;
                _adapter.ConnectionError -= Adapter_ConnectionError;
                _adapter.ConnectionRestored -= Adapter_ConnectionRestored;
                _adapter = null;
            }

            try
            {
                _adapter = Container.Current.Resolve<IDeploymentRepositoryAdapter>();

                _adapter.DeploymentsChanged += Adapter_DeploymentsChanged;

                _adapter.ErrorsCleared += Adapter_ErrorsCleared;
                _adapter.ErrorsFound += Adapter_ErrorsFound;

                _adapter.ConnectionError += Adapter_ConnectionError;
                _adapter.ConnectionRestored += Adapter_ConnectionRestored;

                _adapter.StartPolling();
            }
            catch (ResolutionFailedException)
            {
                _adapter = null;
                SetIconState(NotifyIconState.Disconnected);
            }
        }

        private void SetIconState(NotifyIconState state)
        {
            switch (state)
            {
                case NotifyIconState.Connected:
                    NotifyIcon = ConnectedNotifyIcon;
                    StateSummary = "Octopus Notify (Connected)";
                    break;
                case NotifyIconState.Disconnected:
                    NotifyIcon = DisconnectedNotifyIcon;
                    StateSummary = "Octopus Notify (Disconnected)";
                    break;
                case NotifyIconState.Error:
                    NotifyIcon = ErrorNotifyIcon;
                    StateSummary = "Octopus Notify (Failed Deployments)";
                    break;
                default:
                    throw new InvalidOperationException("Unknown icon type");
            }
        }

        private bool ShouldShowNotification(DeploymentResult item)
        {
            return (item.Status == DeploymentStatus.Success && Settings.Default.AlertOnSuccessfulBuild) ||
                  ((item.Status == DeploymentStatus.Failed || item.Status == DeploymentStatus.TimedOut) && Settings.Default.AlertOnFailedBuild) ||
                  ((item.Status == DeploymentStatus.FailedNew || item.Status == DeploymentStatus.TimedOutNew) && Settings.Default.AlertOnNewFailedBuild) ||
                   (item.Status == DeploymentStatus.Fixed && Settings.Default.AlertOnFixedBuild) ||
                   (item.Status == DeploymentStatus.ManualStep && Settings.Default.AlertOnManualStep) ||
                   (item.Status == DeploymentStatus.GuidedFailure && Settings.Default.AlertOnGuidedFailure);
        }

        private void OnNotification(IEnumerable<Notification> notifications)
        {
            Notification?.Invoke(this, new NotificationEventArgs(notifications));
        }
        #endregion
    }
}