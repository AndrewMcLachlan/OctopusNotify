using System;
using OctopusNotify.Model;

namespace OctopusNotify
{
    /// <summary>
    /// Contract for a deployment repository.
    /// </summary>
    public interface IDeploymentRepositoryAdapter
    {
        /// <summary>
        /// Raised when there is a connection error.
        /// </summary>
        event EventHandler ConnectionError;

        /// <summary>
        /// Raised when the a connection is made or restored.
        /// </summary>
        event EventHandler ConnectionRestored;

        /// <summary>
        /// Raised when the are new deployments.
        /// </summary>
        event EventHandler<DeploymentEventArgs> DeploymentsChanged;

        /// <summary>
        /// Raised when the are new deployments.
        /// </summary>
        event EventHandler<DeploymentSummaryEventArgs> DeploymentSummaryChanged;

        /// <summary>
        /// Raised when all deployments have succeeded.
        /// </summary>
        event EventHandler ErrorsCleared;

        /// <summary>
        /// Raised if any deployment have failed.
        /// </summary>
        event EventHandler ErrorsFound;

        /// <summary>
        /// Start polling with the default interval.
        /// </summary>
        void StartPolling();

        /// <summary>
        /// Start polling.
        /// </summary>
        /// <param name="interval">The polling interval.</param>
        void StartPolling(double interval);

        /// <summary>
        /// Stop polling.
        /// </summary>
        void StopPolling();
    }
}
