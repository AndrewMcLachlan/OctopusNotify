using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Microsoft.Practices.Unity;
using OctopusNotify.App.Ioc;
using OctopusNotify.App.Models;
using OctopusNotify.App.Properties;
using OctopusNotify.App.Utilities;
using OctopusNotify.Model;

namespace OctopusNotify.App.ViewModels
{
    public class NotifyIconViewModel : ViewModel
    {
        #region Fields
        private static readonly ImageSource ErrorNotifyIcon;
        private static readonly ImageSource DisconnectedNotifyIcon;
        private static readonly ImageSource ConnectedNotifyIcon;

        private IDeploymentRepositoryAdapter _adapter;

        private ImageSource _notifyIcon;
        private string _stateSummary;
        private bool _signedIn;

        private BlockingCollection<DeploymentResult> _notificationQueue = new BlockingCollection<DeploymentResult>();
        #endregion

        #region Events
        public event EventHandler<NotificationEventArgs> Notification;
        #endregion

        #region Properties
        public ImageSource NotifyIcon
        {
            get => _notifyIcon;
            set => Set(ref _notifyIcon, value);
        }

        public string StateSummary
        {
            get => _stateSummary;
            set => Set(ref _stateSummary, value);
        }

        public bool SignedIn
        {
            get => _signedIn;
            set => Set(ref _signedIn, value);
        }

        public Visibility DebugVisible
        {
            get
            {
#if STUB
                return Visibility.Visible;
#else
                return Visibility.Collapsed;
#endif
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
            SetIconState(NotifyIconState.Disconnected);

            if (Settings.Default.ServerUrl == null || String.IsNullOrEmpty(Settings.Default.ServerUrl.ToString()))
            {
#if DEBUG
                if (!System.ComponentModel.DesignerProperties.GetIsInDesignMode(new DependencyObject()))
#endif
                    AppCommands.ShowSettings.Execute(null);
            }

#if DEBUG
            if (!System.ComponentModel.DesignerProperties.GetIsInDesignMode(new DependencyObject()))
            {
#endif
                Container.Current.Configured += Container_Configured;
                CreateAdapter();
#if DEBUG
            }
#endif
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
            //CreateAdapter().NoWait();
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

        private void Adapter_DeploymentSummaryChanged(object sender, DeploymentSummaryEventArgs e)
        {
            StringBuilder sb = new StringBuilder();

            foreach(DeploymentStatus ds in e.Summary.Keys)
            {
                sb.AppendFormat("{0} {1}", e.Summary[ds], ds.ToDisplayString());
                sb.AppendLine();
            }

            sb.Remove(sb.Length - 2, 2);

            StateSummary = sb.ToString();
        }

        private void Adapter_SignedIn(object sender, EventArgs e)
        {
            _adapter.StartPolling();
            SignedIn = true;
        }

        private void Adapter_SignedOut(object sender, EventArgs e)
        {
            _adapter.StopPolling();
            SetIconState(NotifyIconState.Disconnected);
            SignedIn = false;
        }
#endregion

#region Private Methods
        /// <summary>
        /// Creates the adapter and starts listening for deployment events.
        /// </summary>
        private void CreateAdapter()
        {
            /*if (_adapter != null)
            {
                _adapter.StopPolling();
                /*_adapter.DeploymentsChanged -= Adapter_DeploymentsChanged;
                _adapter.DeploymentSummaryChanged -= Adapter_DeploymentSummaryChanged;

                _adapter.ErrorsCleared -= Adapter_ErrorsCleared;
                _adapter.ErrorsFound -= Adapter_ErrorsFound;
                _adapter.ConnectionError -= Adapter_ConnectionError;
                _adapter.ConnectionRestored -= Adapter_ConnectionRestored;

                _adapter = null;* /
            }*/

            try
            {
                /*IConnectionTester tester = Container.Current.Resolve<IConnectionTester>();
                (bool, string) testResult = await tester.Test();

                if (!testResult.Item1)
                {
                    //_adapter = null;
                    SetIconState(NotifyIconState.Disconnected);
                    return;
                }*/

                _adapter = Container.Current.Resolve<IDeploymentRepositoryAdapter>();

                _adapter.DeploymentsChanged += Adapter_DeploymentsChanged;
                _adapter.DeploymentSummaryChanged += Adapter_DeploymentSummaryChanged;

                _adapter.ErrorsCleared += Adapter_ErrorsCleared;
                _adapter.ErrorsFound += Adapter_ErrorsFound;

                _adapter.ConnectionError += Adapter_ConnectionError;
                _adapter.ConnectionRestored += Adapter_ConnectionRestored;

                _adapter.SignedIn += Adapter_SignedIn;
                _adapter.SignedOut += Adapter_SignedOut;

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
                    break;
                case NotifyIconState.Disconnected:
                    NotifyIcon = DisconnectedNotifyIcon;
                    StateSummary = "Octopus Notify (Disconnected)";
                    break;
                case NotifyIconState.Error:
                    NotifyIcon = ErrorNotifyIcon;
                    break;
                default:
                    throw new InvalidOperationException("Unknown icon type");
            }
        }

        private bool ShouldShowNotification(DeploymentResult item)
        {
            return (item.Status == DeploymentStatus.Success && Settings.Default.AlertOnSuccessfulBuild) ||
                  ((item.Status == DeploymentStatus.Failed || item.Status == DeploymentStatus.TimedOut) && Settings.Default.AlertOnFailedBuild) ||
                  ((item.Status == DeploymentStatus.FailedNew || item.Status == DeploymentStatus.TimedOutNew) && (Settings.Default.AlertOnNewFailedBuild || Settings.Default.AlertOnFailedBuild)) ||
                   (item.Status == DeploymentStatus.Fixed && (Settings.Default.AlertOnFixedBuild || Settings.Default.AlertOnSuccessfulBuild)) ||
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
