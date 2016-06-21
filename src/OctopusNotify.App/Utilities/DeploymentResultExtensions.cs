using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Octopus.Client.Model;
using OctopusNotify.Model;

namespace OctopusNotify.App.Utilities
{
    public static class DeploymentResultExtensions
    {
        public static string ToDisplayMessage(this DeploymentResult deploymentResult)
        {
            if (deploymentResult == null) return null;

            return String.Format("{0}, {1}, {2}", deploymentResult.Project.Name, deploymentResult.Version, deploymentResult.Environment.Name);
        }
    }
}
