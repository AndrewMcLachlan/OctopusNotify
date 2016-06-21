using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Octopus.Client.Model;
using OctopusNotify.Model;

namespace OctopusNotify
{
    public class DeploymentEventArgs : EventArgs
    {
        public List<DeploymentResult> Items { get; set; }

        public DeploymentEventArgs()
        {
            Items = new List<DeploymentResult>();
        }

    }
}
