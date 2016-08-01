using OctopusNotify.Model;

namespace OctopusNotify.App.Utilities
{
    public static class DeploymentStatusExtensions
    {
        public static string ToDisplayString(this DeploymentStatus status)
        {
            switch (status)
            {
                case DeploymentStatus.Success:
                case DeploymentStatus.Fixed:
                    return "Successful";
                case DeploymentStatus.TimedOut:
                case DeploymentStatus.TimedOutNew:
                    return "Timed Out";
                case DeploymentStatus.FailedNew:
                    return "Failed";
                case DeploymentStatus.ManualStep:
                    return "at a Manual Step";
                case DeploymentStatus.GuidedFailure:
                    return "in Guided Failure Mode";
                default:
                    return status.ToString();
            }
        }
    }
}
