using System;

namespace OctopusNotify.Model
{
    /// <summary>
    /// Represents a deployment result.
    /// </summary>
    public class DeploymentResult
    {
        /// <summary>
        /// Gets or sets the deployment environment.
        /// </summary>
        public DeploymentEnvironment Environment { get; set; }

        /// <summary>
        /// Gets or sets the project.
        /// </summary>
        public Project Project { get; set; }

        /// <summary>
        /// Gets or sets the task ID.
        /// </summary>
        public string TaskId { get; set; }

        /// <summary>
        /// Gets or sets the deployment ID.
        /// </summary>
        public string DeploymentId { get; set; }

        /// <summary>
        /// Gets or sets the version.
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// Gets or sets the status of the deployment.
        /// </summary>
        public DeploymentStatus Status { get; set; }

        /// <summary>
        /// Gets or sets any error message associated with the deployment.
        /// </summary>
        public string ErrorMessage { get; set; }

        /// <summary>
        /// Gets or sets time of the deployment event.
        /// </summary>
        public DateTimeOffset EventTime { get; internal set; }
    }
}
