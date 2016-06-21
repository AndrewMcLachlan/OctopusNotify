using Octopus.Client.Model;
using OctopusNotify.Model;

namespace OctopusNotify.Utilities
{
    internal static class ReferenceDataItemExtensions
    {
        public static DeploymentEnvironment ToEnvironment(this ReferenceDataItem environment)
        {
            return new DeploymentEnvironment
            {
                EnvironmentId = environment.Id,
                Name = environment.Name,
            };
        }

        public static Project ToProject(this ReferenceDataItem project)
        {
            return new Project
            {
                ProjectId = project.Id,
                Name = project.Name,
            };
        }
    }
}
