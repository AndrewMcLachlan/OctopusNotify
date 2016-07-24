namespace OctopusNotify.Model
{
    public class DeploymentResult
    {
        public DeploymentEnvironment Environment { get; set; }

        public Project Project { get; set; }

        public string TaskId { get; set; }

        public string DeploymentId { get; set; }

        public string Version { get; set; }

        public DeploymentStatus Status { get; set; }

        public string ErrorMessage { get; set; }
    }
}
