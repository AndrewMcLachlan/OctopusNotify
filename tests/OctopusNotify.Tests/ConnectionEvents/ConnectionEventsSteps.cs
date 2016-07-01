using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Octopus.Client;
using Octopus.Client.Model;
using Octopus.Client.Repositories;
using TechTalk.SpecFlow;

namespace OctopusNotify.Tests.ConnectionEvents
{
    [Binding]
    [Scope(Feature = "Connection Events")]
    public class ConnectionEventsSteps
    {
        OctopusAdapter adapter;
        DashboardResource dashboard;
        Mock<IDashboardRepository> dashboardMock = new Mock<IDashboardRepository>();

        bool connectionLostFired;
        bool connectionRestoredFired;

        [Given(@"an adapter with a broken connection '(.*)'")]
        public void GivenAnAdapterWithABrokenConnection(bool brokenCconnection)
        {
            dashboard = new DashboardResource
            {
                Environments = new List<DashboardEnvironmentResource>
                {
                    new DashboardEnvironmentResource
                    {
                        Id = "1234",
                        Name = "1234"
                    },
                },
                Projects = new List<DashboardProjectResource>
                {
                    new DashboardProjectResource
                    {
                        Id = "1234",
                        Name = "1234",
                    }
                },
                Id = "1234",
                Items = new List<DashboardItemResource>(),
                PreviousItems = new List<DashboardItemResource>(),
            };

            Mock<IOctopusRepository> repoMock = new Mock<IOctopusRepository>();

            repoMock.SetupGet(r => r.Dashboards).Returns(dashboardMock.Object);

            adapter = new OctopusAdapter(repoMock.Object, 15000);

            if (brokenCconnection)
            {
                dashboardMock.Setup(d => d.GetDashboard()).Throws(new Exception("Test"));
                adapter.Poll();
            }

            adapter.ConnectionError += Adapter_ConnectionError;
            adapter.ConnectionRestored += Adapter_ConnectionRestored;
        }

        [Given(@"a current connection is broken '(.*)'")]
        public void GivenACurrentConnectionIsBroken(bool broken)
        {
            if (broken)
            {
                dashboardMock.Setup(d => d.GetDashboard()).Throws(new Exception("Test"));
            }
            else
            {
                dashboardMock.Setup(d => d.GetDashboard()).Returns(dashboard);
            }
        }
        
        [When(@"the repository is polled")]
        public void WhenTheRepositoryIsPolled()
        {
            adapter.Poll();
        }
        
        [Then(@"the connection lost event will have fired '(.*)'")]
        public void ThenTheConnectionLostEventWillHaveFired(bool fired)
        {
            Assert.AreEqual(fired, connectionLostFired);
        }
        
        [Then(@"the connection restored event will have fired '(.*)'")]
        public void ThenTheConnectionRestoredEventWillHaveFired(bool fired)
        {
            Assert.AreEqual(fired, connectionRestoredFired);
        }

        private void Adapter_ConnectionRestored(object sender, EventArgs e)
        {
            connectionRestoredFired = true;
        }

        private void Adapter_ConnectionError(object sender, EventArgs e)
        {
            connectionLostFired = true;
        }

    }
}
