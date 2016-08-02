using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Octopus.Client;
using Octopus.Client.Model;
using Octopus.Client.Repositories;
using OctopusNotify.Model;
using TechTalk.SpecFlow;

namespace OctopusNotify.Tests
{
    public abstract class SingleItemEventSteps
    {
        DashboardResource dashboard;
        protected OctopusAdapter adapter;
        protected DeploymentEventArgs deploymentEventArgs;
        protected int eventFiredCount = 0;
        protected string firedEventType;

        protected List<DashboardItemResource> _previousBuilds = new List<DashboardItemResource>();

        [BeforeScenario]
        public virtual void Setup()
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
            };

            Mock<IDashboardRepository> dashboardMock = new Mock<IDashboardRepository>();

            dashboardMock.Setup(d => d.GetDashboard()).Returns(dashboard);

            Mock<IOctopusRepository> repoMock = new Mock<IOctopusRepository>();

            repoMock.SetupGet(r => r.Dashboards).Returns(dashboardMock.Object);

            adapter = new OctopusAdapter(repoMock.Object, 15000);

            eventFiredCount = 0;
            deploymentEventArgs = null;
        }

        [Given(@"a build with an id of '(.*)'")]
        public void GivenABuildWithAnIdOf(string id)
        {
            dashboard.Items = new List<DashboardItemResource>
            {
                new DashboardItemResource
                {
                    Id = id,
                    EnvironmentId = id,
                    DeploymentId = id,
                    ProjectId = id,
                    IsCurrent = true,
                }
            };
        }

        [Given(@"a completed time of now plus '(.*)' seconds")]
        public void GivenACompletedTimeOfNowPlusSeconds(int seconds)
        {
            dashboard.Items[0].CompletedTime = DateTime.Now.AddSeconds(seconds);
        }

        [Given(@"has errors or warnings '(.*)'")]
        public void GivenHasErrorsOrWarnings(bool hasErrors)
        {
            dashboard.Items[0].HasWarningsOrErrors = hasErrors;
        }

        [Given(@"a previous build with errors or warnings '(.*)'")]
        public void GivenAPreviousBuildWithErrorsOrWarnings(bool hasErrors)
        {
            _previousBuilds[0].HasWarningsOrErrors = hasErrors;

            if (hasErrors)
            {
                PrivateObject privAdapter = new PrivateObject(adapter);
                privAdapter.SetField("_failedBuilds", _previousBuilds);
            }
        }

        [Given(@"a previous build with an id of '(.*)'")]
        public void GivenAPreviousBuildWithAnIdOf(string id)
        {
            _previousBuilds.Add(
                new DashboardItemResource
                {
                    Id = id,
                    EnvironmentId = id,
                    DeploymentId = id,
                    ProjectId = id,
                    IsPrevious = true,
                    IsCompleted = true,
                    CompletedTime = DateTime.Now.AddSeconds(-1),
                });
        }

        [Given(@"has a state of '(.*)'")]
        public void GivenHasAStateOf(TaskState state)
        {
            dashboard.Items[0].State = state;
        }

        [Given(@"a previous build with a state of '(.*)'")]
        public void GivenAPreviousBuildWithAStateOf(TaskState state)
        {
            _previousBuilds[0].State = state;
        }

        [When(@"the repository is polled")]
        public void WhenTheRepositoryIsPolled()
        {
            adapter.Poll();
            //            System.Threading.Thread.Sleep(1000);
        }

        [Then(@"an event fires: '(.*)'")]
        public void ThenTheEventFires(bool eventFires)
        {
            if (eventFires)
            {
                Assert.AreEqual(1, eventFiredCount);
                Assert.IsNotNull(deploymentEventArgs);
            }
            else
            {
                Assert.AreEqual(0, eventFiredCount);
                Assert.IsNull(deploymentEventArgs);
            }
        }

        [Then(@"the event type is '(.*)'")]
        public void ThenTheEventTypeIs(string eventType)
        {
            if (String.IsNullOrEmpty(eventType))
            {
                Assert.IsNull(firedEventType);
            }
            else
            {
                Assert.AreEqual(eventType, firedEventType);
            }
        }


        [Then(@"the event has (.*) builds")]
        public void ThenTheEventHasBuilds(int numberOfBuilds)
        {
            if (numberOfBuilds > 0)
            {
                Assert.IsNotNull(deploymentEventArgs);
                Assert.AreEqual(numberOfBuilds, deploymentEventArgs.Items.Count);
            }
        }

        [Then(@"the event has a build with an id of '(.*)'")]
        public void ThenTheEventHasABuildWithAnIdOf(string id)
        {
            if (String.IsNullOrEmpty(id)) return;

            Assert.IsNotNull(deploymentEventArgs);
            Assert.IsTrue(deploymentEventArgs.Items.Any(i => i.Environment.EnvironmentId == id && i.Project.ProjectId == id));
        }
    }
}
