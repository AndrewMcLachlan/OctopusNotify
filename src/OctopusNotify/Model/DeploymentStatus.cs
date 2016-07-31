namespace OctopusNotify.Model
{
    /// <summary>
    /// The status of a deployment.
    /// </summary>
    public enum DeploymentStatus
    {
        Queued = 1,
        Executing = 2,
        Failed = 3,
        Canceled = 4,
        TimedOut = 5,
        Success = 6,
        Cancelling = 8,
        FailedNew = 103,
        TimedOutNew = 105,
        Fixed = 106,
        ManualStep = 200,
        GuidedFailure = 201,
    }
}
