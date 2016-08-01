using System;
using System.Collections.Generic;
using System.Linq;
using OctopusNotify.Model;

namespace OctopusNotify.Model
{
    public class DeploymentEventArgs : EventArgs
    {
        public IReadOnlyList<DeploymentResult> Items { get; private set; }

        public DeploymentEventArgs(IEnumerable<DeploymentResult> items)
        {
            Items = items.ToList();
        }
    }
}
