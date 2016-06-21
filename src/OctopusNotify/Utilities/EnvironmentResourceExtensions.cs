using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OctopusNotify.Model;
using Octopus.Client.Model;

namespace OctopusNotify.Utilities
{
    internal static class EnvironmentResourceExtensions
    {
        public static DeploymentEnvironment ToDeploymentEnvironment(this EnvironmentResource environment)
        {
            return new DeploymentEnvironment
            {
                EnvironmentId = environment.Id,
                Name = environment.Name,
            };
        }
    }
}
