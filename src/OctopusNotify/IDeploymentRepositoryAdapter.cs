using System;

namespace OctopusNotify
{
    public interface IDeploymentRepositoryAdapter
    {
        event EventHandler ConnectionError;
        event EventHandler ConnectionRestored;

        event EventHandler<DeploymentEventArgs> DeploymentsChanged;

        event EventHandler<DeploymentEventArgs> DeploymentSucceeded;
        event EventHandler<DeploymentEventArgs> DeploymentFailed;
        event EventHandler<DeploymentEventArgs> DeploymentFailedNew;
        event EventHandler<DeploymentEventArgs> DeploymentFixed;

        event EventHandler<DeploymentEventArgs> DeploymentManualStep;
        event EventHandler<DeploymentEventArgs> DeploymentGuidedFailure;

        event EventHandler ErrorsCleared;
        event EventHandler ErrorsFound;

        void StartPolling();
        void StartPolling(double interval);
        void StopPolling();
    }
}
