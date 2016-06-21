using System.Collections.Generic;
using System.Linq;
using Octopus.Client.Model;
using OctopusNotify.Model;

namespace OctopusNotify.Utilities
{
    internal static class DashboardItemResourceExtensions
    {
        public static DeploymentResult ToDeploymentResult(this DashboardItemResource item, IEnumerable<Project> projects, IEnumerable<DeploymentEnvironment> environments)
        {
            return new DeploymentResult
            {
                HasError = item.HasWarningsOrErrors,
                ErrorMessage = item.ErrorMessage,
                Version = item.ReleaseVersion,
                Project = projects.Where(p => p.ProjectId == item.ProjectId).SingleOrDefault(),
                Environment = environments.Where(e => e.EnvironmentId == item.EnvironmentId).SingleOrDefault(),
            };
        }

        public static DeploymentResult ToDeploymentResult(this DashboardItemResource item, DashboardResource dashboard)
        {
            return new DeploymentResult
            {
                HasError = item.HasWarningsOrErrors,
                ErrorMessage = item.ErrorMessage,
                Version = item.ReleaseVersion,
                Project = dashboard.Projects.Where(p => p.Id == item.ProjectId).Select(p => p.ToProject()).SingleOrDefault(),
                Environment = dashboard.Environments.Where(e => e.Id == item.EnvironmentId).Select(e => e.ToDeploymentEnvironment()).SingleOrDefault(),
            };
        }
    }
}
