using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTalk.SpecFlow;

namespace OctopusNotify.Tests
{
    [Binding]
    [Scope(Tag = "FailedEvent")]
    public class FailedEventSteps : SingleItemEventSteps
    {
        public override void Setup()
        {
            base.Setup();

            adapter.DeploymentFailed += Adapter_DeploymentFailed;
        }

        private void Adapter_DeploymentFailed(object sender, DeploymentEventArgs e)
        {
            firedEventType = "Failed";
            deploymentEventArgs = e;
            eventFiredCount++;
        }
    }
}
