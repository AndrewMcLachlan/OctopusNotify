using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTalk.SpecFlow;

namespace OctopusNotify.Tests
{
    [Binding]
    [Scope(Tag = "FailedNewEvent")]
    public class FailedNewEventSteps : SingleItemEventSteps
    {
        public override void Setup()
        {
            base.Setup();

            adapter.DeploymentsChanged += Adapter_DeploymentFailedNew;
        }

        private void Adapter_DeploymentFailedNew(object sender, DeploymentEventArgs e)
        {
            firedEventType = "FailedNew";
            deploymentEventArgs = e;
            eventFiredCount++;
        }
    }
}
