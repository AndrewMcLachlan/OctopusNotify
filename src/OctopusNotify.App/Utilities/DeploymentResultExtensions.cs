using System;
using System.Drawing;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using OctopusNotify.App.ViewModels;
using OctopusNotify.App.Properties;
using OctopusNotify.Model;
using OctopusNotify.App.Models;

namespace OctopusNotify.App.Utilities
{
    public static class DeploymentResultExtensions
    {
        public static string ToDisplayMessage(this DeploymentResult deploymentResult)
        {
            if (deploymentResult == null) return null;

            return String.Format("{0}, {1}, {2}", deploymentResult.Project.Name, deploymentResult.Version, deploymentResult.Environment.Name);
        }

        public static Notification ToNotification(this DeploymentResult deploymentResult)
        {
            var notification = new Notification
            {
                EnvironmentName = deploymentResult.Environment.Name,
                ProjectName = deploymentResult.Project.Name,
                Version = deploymentResult.Version,
                Link =  new Uri(String.Format("{0}/app#/deployments/{1}", Settings.Default.ServerUrl.TrimEnd('/'), deploymentResult.DeploymentId))
            };

            switch(deploymentResult.Status)
            {
                case DeploymentStatus.Success:
                    notification.Icon = BalloonIcon.Success;
                    notification.Message = "Deployment Succeeded";
                    break;
                case DeploymentStatus.Failed:
                case DeploymentStatus.FailedNew:
                    notification.Icon = BalloonIcon.Error;
                    notification.Message = "Deployment Failed";
                        break;
                case DeploymentStatus.TimedOut:
                case DeploymentStatus.TimedOutNew:
                    notification.Icon = BalloonIcon.Error;
                    notification.Message = "Deployment Timed out";
                    break;
                case DeploymentStatus.Fixed:
                    notification.Icon = BalloonIcon.Success;
                    notification.Message = "Deployment Fixed";
                    break;
                case DeploymentStatus.ManualStep:
                    notification.Icon = BalloonIcon.Question;
                    notification.Message = "Manual Action Required";
                    break;
                case DeploymentStatus.GuidedFailure:
                    notification.Icon = BalloonIcon.Warning;
                    notification.Message = "Guided Failure Requires Attention";
                    break;
            }

            return notification;
        }
    }
}
