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
        private static readonly BitmapImage GreenTick = new BitmapImage(new Uri("pack://application:,,,/OctopusNotify;component/Images/Green Tick.png"));
        #endregion

        #region Constructors
        public NotifyIconWindow()
        {
            Visibility = Visibility.Hidden;
            InitializeComponent();

            DataContext = new NotifyIconViewModel();
        }
        #endregion

        #region Event Handlers

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
                DispatcherHelper.RunEventHandler(ShowFailedBalloon, sender, e);
            }
        }

        private void Adapter_DeploymentFailedNew(object sender, DeploymentEventArgs e)
        {
            if (Settings.Default.AlertOnNewFailedBuild)
            {
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

            Uri link = new Uri(String.Format("{0}/app#/deployments/{1}", Settings.Default.ServerUrl.TrimEnd('/'), item.DeploymentId));
            NotifyIcon.ShowCustomBalloon(new Balloon(title, item.Project.Name, item.Version, item.Environment.Name, link, icon), System.Windows.Controls.Primitives.PopupAnimation.Slide, 10000);
        }

        private void ShowBalloon(object sender, DeploymentEventArgs e, string title, ImageSource icon)
        {
            var item = e.Items.First();

            Uri link = new Uri(String.Format("{0}/app#/deployments/{1}", Settings.Default.ServerUrl.TrimEnd('/'), item.DeploymentId));
            NotifyIcon.ShowCustomBalloon(new Balloon(title, item.Project.Name, item.Version, item.Environment.Name, link, icon), System.Windows.Controls.Primitives.PopupAnimation.Slide, 10000);
        }
        #endregion
    }
}
