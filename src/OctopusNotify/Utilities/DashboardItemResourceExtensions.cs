using System;
using System.Collections.Generic;
using System.Linq;
using Octopus.Client.Model;
using OctopusNotify.Model;

namespace OctopusNotify.Utilities
{
    internal static class DashboardItemResourceExtensions
    {
        public static DeploymentResult ToDeploymentResult(this DashboardItemResource item, IEnumerable<Project> projects, IEnumerable<DeploymentEnvironment> environments, DeploymentStatus status)
        {
            return new DeploymentResult
            {
                Status = status,
                ErrorMessage = item.ErrorMessage,
                Version = item.ReleaseVersion,
                Project = projects.Where(p => p.ProjectId == item.ProjectId).SingleOrDefault(),
                Environment = environments.Where(e => e.EnvironmentId == item.EnvironmentId).SingleOrDefault(),
                TaskId = item.TaskId,
                DeploymentId = item.DeploymentId,
            };
        }

        public static DeploymentResult ToDeploymentResult(this DashboardItemResource item, DashboardResource dashboard, DeploymentStatus status)
        {
            return new DeploymentResult
            {
                Status = status,
                ErrorMessage = item.ErrorMessage,
                Version = item.ReleaseVersion,
                Project = dashboard.Projects.Where(p => p.Id == item.ProjectId).Select(p => p.ToProject()).SingleOrDefault(),
                Environment = dashboard.Environments.Where(e => e.Id == item.EnvironmentId).Select(e => e.ToDeploymentEnvironment()).SingleOrDefault(),
                TaskId = item.TaskId,
                DeploymentId = item.DeploymentId,
            };
        }

        public static DeploymentStatus ToDeploymentStatus(this TaskState state)
        {
            return (DeploymentStatus)(int)state;
        }

        public static DeploymentStatus ToDeploymentStatus(this TaskState state, int offset)
        {
            return (DeploymentStatus)((int)state+offset);
        }
    }
}
