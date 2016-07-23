using System;
using System.Drawing;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Microsoft.Practices.Unity;
using OctopusNotify.App.Ioc;
using OctopusNotify.App.Properties;
using OctopusNotify.App.ViewModels;

namespace OctopusNotify.App
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class NotifyIconWindow : Window
    {
        #region Fields
        private IDeploymentRepositoryAdapter _adapter;

        private static readonly Icon ErrorNotifyIcon;
        private static readonly Icon DisconnectedNotifyIcon;
        private static readonly Icon ConnectedNotifyIcon;

        private static readonly BitmapImage GreenTick = new BitmapImage(new Uri("pack://application:,,,/OctopusNotify;component/Images/Green Tick.png"));
        #endregion

        #region Constructors
        static NotifyIconWindow()
        {
            DisconnectedNotifyIcon = new Icon(typeof(NotifyIconWindow), "NotifyIcons.Disconnected.ico");
            ConnectedNotifyIcon = new Icon(typeof(NotifyIconWindow), "NotifyIcons.Connected.ico");
            ErrorNotifyIcon = new Icon(typeof(NotifyIconWindow), "NotifyIcons.Red.ico");
        }

        public NotifyIconWindow()
        {
            Visibility = Visibility.Hidden;
            InitializeComponent();

            SetIconState(NotifyIconState.Disconnected);

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
        private void Adapter_ConnectionError(object sender, EventArgs e)
        {
            DispatcherHelper.Run(SetIconState, NotifyIconState.Disconnected);
        }

        private void Adapter_ConnectionRestored(object sender, EventArgs e)
        {
            DispatcherHelper.Run(SetIconState, NotifyIconState.Connected);
        }

        private void Adapter_ErrorsCleared(object sender, EventArgs e)
        {
            DispatcherHelper.Run(SetIconState, NotifyIconState.Connected);
        }

        private void Adapter_ErrorsFound(object sender, EventArgs e)
        {
            DispatcherHelper.Run(SetIconState, NotifyIconState.Error);
        }

        private void Adapter_DeploymentSucceeded(object sender, DeploymentEventArgs e)
        {
            if (Settings.Default.AlertOnSuccessfulBuild)
            {
                DispatcherHelper.RunEventHandler(ShowSucceededBalloon, sender, e);
            }
        }

        private void Adapter_DeploymentFailed(object sender, DeploymentEventArgs e)
        {
            if (Settings.Default.AlertOnFailedBuild)
            {
                DispatcherHelper.Run(SetIconState, NotifyIconState.Error);
                DispatcherHelper.RunEventHandler(ShowFailedBalloon, sender, e);
            }
        }

        private void Adapter_DeploymentFailedNew(object sender, DeploymentEventArgs e)
        {
            if (Settings.Default.AlertOnNewFailedBuild)
            {
                DispatcherHelper.Run(SetIconState, NotifyIconState.Error);
                DispatcherHelper.RunEventHandler(ShowFailedNewBalloon, sender, e);
            }
        }

        private void Adapter_DeploymentFixed(object sender, DeploymentEventArgs e)
        {
            if (Settings.Default.AlertOnFixedBuild)
            {
                DispatcherHelper.RunEventHandler(ShowFixedBalloon, sender, e);
            }
        }

        private void Adapter_DeploymentManualStep(object sender, DeploymentEventArgs e)
        {
            if (Settings.Default.AlertOnManualStep)
            {
                DispatcherHelper.RunEventHandler(ShowManualStepBalloon, sender, e);
            }
        }

        private void Adapter_DeploymentGuidedFailure(object sender, DeploymentEventArgs e)
        {
            if (Settings.Default.AlertOnGuidedFailure)
            {
                DispatcherHelper.RunEventHandler(ShowGuidedFailureBalloon, sender, e);
            }
        }

        /// <summary>
        /// Handles changes to IOC configuration.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Container_Configured(object sender, EventArgs e)
        {
            CreateAdapter();
        }

        private void Window_StateChanged(object sender, EventArgs e)
        {
            if (WindowState == WindowState.Minimized)
            {
                Hide();
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
            WindowState = WindowState.Minimized;
            Hide();
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
                _adapter.DeploymentSucceeded -= Adapter_DeploymentSucceeded;
                _adapter.DeploymentFailed -= Adapter_DeploymentFailed;
                _adapter.DeploymentFailedNew -= Adapter_DeploymentFailedNew;
                _adapter.DeploymentFixed -= Adapter_DeploymentFixed;
                _adapter.DeploymentManualStep -= Adapter_DeploymentManualStep;
                _adapter.DeploymentGuidedFailure -= Adapter_DeploymentGuidedFailure;

                _adapter.ErrorsCleared -= Adapter_ErrorsCleared;
                _adapter.ErrorsFound -= Adapter_ErrorsFound;
                _adapter.ConnectionError -= Adapter_ConnectionError;
                _adapter.ConnectionRestored -= Adapter_ConnectionRestored;
                _adapter = null;
            }

            try
            {
                _adapter = Container.Current.Resolve<IDeploymentRepositoryAdapter>();

                _adapter.DeploymentSucceeded += Adapter_DeploymentSucceeded;
                _adapter.DeploymentFailed += Adapter_DeploymentFailed;
                _adapter.DeploymentFailedNew += Adapter_DeploymentFailedNew;
                _adapter.DeploymentFixed += Adapter_DeploymentFixed;
                _adapter.DeploymentManualStep += Adapter_DeploymentManualStep;
                _adapter.DeploymentGuidedFailure += Adapter_DeploymentGuidedFailure;

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
                    NotifyIcon.Icon = ConnectedNotifyIcon;
                    NotifyIcon.ToolTipText = "Octopus Notify (Connected)";
                    break;
                case NotifyIconState.Disconnected:
                    NotifyIcon.Icon = DisconnectedNotifyIcon;
                    NotifyIcon.ToolTipText = "Octopus Notify (Disconnected)";
                    break;
                case NotifyIconState.Error:
                    NotifyIcon.Icon = ErrorNotifyIcon;
                    NotifyIcon.ToolTipText = "Octopus Notify (Failed Deployments)";
                    break;
                default:
                    throw new InvalidOperationException("Unknown icon type");
            }
        }

        private void ShowSucceededBalloon(object sender, DeploymentEventArgs e)
        {
            ShowBalloon(sender, e, "Deployment Succeeded", GreenTick);
        }

        private void ShowFailedBalloon(object sender, DeploymentEventArgs e)
        {
            ShowBalloon(sender, e, "Deployment Failed", SystemIcons.Error);
        }

        private void ShowFailedNewBalloon(object sender, DeploymentEventArgs e)
        {
            ShowBalloon(sender, e, "Deployment Failed", SystemIcons.Error);
        }

        private void ShowFixedBalloon(object sender, DeploymentEventArgs e)
        {
            ShowBalloon(sender, e, "Deployment Fixed", GreenTick);
        }

        private void ShowManualStepBalloon(object sender, DeploymentEventArgs e)
        {
            ShowBalloon(sender, e, "Manual Action Required", SystemIcons.Question);
        }
        private void ShowGuidedFailureBalloon(object sender, DeploymentEventArgs e)
        {
            ShowBalloon(sender, e, "Guided Failure Requires Attention", SystemIcons.Warning);
        }

        private void ShowBalloon(object sender, DeploymentEventArgs e, string title, Icon icon)
        {
            var item = e.Items.First();
            NotifyIcon.ShowCustomBalloon(new Balloon(title, item.Project.Name, item.Version, item.Environment.Name, icon), System.Windows.Controls.Primitives.PopupAnimation.Slide, 10000);
        }

        private void ShowBalloon(object sender, DeploymentEventArgs e, string title, ImageSource icon)
        {
            var item = e.Items.First();
            NotifyIcon.ShowCustomBalloon(new Balloon(title, item.Project.Name, item.Version, item.Environment.Name, icon), System.Windows.Controls.Primitives.PopupAnimation.Slide, 10000);
        }
        #endregion
    }
}
