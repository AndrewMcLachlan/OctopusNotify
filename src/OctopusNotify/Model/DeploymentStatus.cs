namespace OctopusNotify.Model
{
    public enum DeploymentStatus
    {
        Queued = 1,
        Executing = 2,
        Failed = 3,
        Canceled = 4,
        TimedOut = 5,
        Success = 6,
        Cancelling = 8,
        FailedNew = 100,
        Fixed = 101,
        ManualStep = 102,
        GuidedFailure = 103,
    }
}
