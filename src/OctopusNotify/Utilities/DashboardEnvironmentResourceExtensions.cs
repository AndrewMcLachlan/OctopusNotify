using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OctopusNotify.Model;
using Octopus.Client.Model;

namespace OctopusNotify.Utilities
{
    internal static class DashboardEnvironmentResourceExtensions
    {
        public static DeploymentEnvironment ToDeploymentEnvironment(this DashboardEnvironmentResource environment)
        {
            return new DeploymentEnvironment
            {
                EnvironmentId = environment.Id,
                Name = environment.Name,
            };
        }
    }
}
