using System;
using System.Collections.Generic;

namespace OctopusNotify.Model
{
    public class DeploymentSummaryEventArgs : EventArgs
    {
        public IReadOnlyDictionary<DeploymentStatus, int> Summary { get; private set; }

        public DeploymentSummaryEventArgs(Dictionary<DeploymentStatus, int> summary)
        {
            Summary = summary;
        }
    }
}
