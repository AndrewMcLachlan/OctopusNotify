using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OctopusNotify.Model;
using TechTalk.SpecFlow;

namespace OctopusNotify.Tests
{
    [Binding]
    [Scope(Tag = "FixedEvent")]
    public class FixedEventSteps : SingleItemEventSteps
    {
        public override void Setup()
        {
            base.Setup();

            adapter.DeploymentsChanged += Adapter_DeploymentFixed;
        }

        private void Adapter_DeploymentFixed(object sender, DeploymentEventArgs e)
        {
            firedEventType = "Fixed";
            deploymentEventArgs = e;
            eventFiredCount++;
        }
    }
}
