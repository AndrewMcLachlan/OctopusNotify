namespace OctopusNotify.Model
{
    public class DeploymentResult
    {
        public DeploymentEnvironment Environment { get; set; }

        public Project Project { get; set; }

        public string Version { get; set; }

        public bool HasError { get; set; }

        public string ErrorMessage { get; set; }
    }
}
