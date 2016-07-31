using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTalk.SpecFlow;

namespace OctopusNotify.Tests
{
    [Binding]
    [Scope(Tag = "CompletedEvent")]
    public class CompletedEventSteps : SingleItemEventSteps
    {
        public override void Setup()
        {
            base.Setup();

            adapter.DeploymentsChanged += Adapter_DeploymentSucceeded;
        }

        private void Adapter_DeploymentSucceeded(object sender, DeploymentEventArgs e)
        {
            firedEventType = "Completed";
            deploymentEventArgs = e;
            eventFiredCount++;
        }
    }
}
