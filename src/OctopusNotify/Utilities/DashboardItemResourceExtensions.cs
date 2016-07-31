using System;
using System.Collections.Generic;
using System.Linq;
using Octopus.Client.Model;
using OctopusNotify.Model;

namespace OctopusNotify.Utilities
{
    internal static class DashboardItemResourceExtensions
    {
        /// <summary>
        /// Converts a <see cref="DashboardItemResource"/> to a <see cref="DeploymentResult"/>.
        /// </summary>
        /// <param name="item">The item to convert.</param>
        /// <param name="projects">A list of projects.</param>
        /// <param name="environments">A list of environments.</param>
        /// <param name="status">The deployment status to set.</param>
        /// <returns>A new deployment result.</returns>
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
                EventTime = item.CompletedTime ?? DateTimeOffset.Now,
            };
        }

        /// <summary>
        /// Converts a <see cref="DashboardItemResource"/> to a <see cref="DeploymentResult"/>.
        /// </summary>
        /// <param name="item">The item to convert.</param>
        /// <param name="dashboard">The dashboard containing all project and environment information.</param>
        /// <param name="status">The deployment status to set.</param>
        /// <returns>A new deployment result.</returns>
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
                EventTime = item.CompletedTime ?? DateTimeOffset.Now,
            };
        }

        /// <summary>
        /// Converts a <see cref="TaskState"/> into a <see cref="DeploymentStatus"/>.
        /// </summary>
        /// <param name="state">The state to convert.</param>
        /// <returns>The converted status.</returns>
        public static DeploymentStatus ToDeploymentStatus(this TaskState state)
        {
            return (DeploymentStatus)(int)state;
        }

        /// <summary>
        /// Converts a <see cref="TaskState"/> into a <see cref="DeploymentStatus"/>.
        /// </summary>
        /// <remarks>
        /// For convenience, more detailed versions of existing enumeration values (such as <see cref="DeploymentStatus.FailedNew"/>)
        /// can be derived from the original value by means of applying an offset to the integer representation of the original value.
        /// </remarks>
        /// <param name="state">The state to convert.</param>
        /// <param name="offset">Offsets the original enumeration to give it a new meaning.</param>
        /// <returns>The converted status.</returns>
        public static DeploymentStatus ToDeploymentStatus(this TaskState state, int offset)
        {
            return (DeploymentStatus)((int)state+offset);
        }
    }
}
