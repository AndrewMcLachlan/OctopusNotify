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

namespace OctopusNotify.Tests.MultiItemEvents
{
    [Binding]
    public class MultiItemEventsSteps
    {
        private DeploymentSummaryEventArgs _args;
        private DashboardResource dashboard;
        private OctopusAdapter adapter;

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
                Items = new List<DashboardItemResource>(),
            };

            Mock<IDashboardRepository> dashboardMock = new Mock<IDashboardRepository>();

            dashboardMock.Setup(d => d.GetDashboard()).Returns(dashboard);

            Mock<IInterruptionRepository> interruptionMock = new Mock<IInterruptionRepository>();

            interruptionMock.Setup(i => i.List(It.IsAny<int>(), true, It.IsAny<string>())).Returns((int i, bool pending, string t) => new ResourceCollection<InterruptionResource>(new List<InterruptionResource>
            {
                new InterruptionResource
                {
                    TaskId = t,
                    Id = t,
                    IsPending = pending,
                    Created = DateTime.Now,
                }
            }, new LinkCollection()));

            Mock<IOctopusRepository> repoMock = new Mock<IOctopusRepository>();

            repoMock.SetupGet(r => r.Dashboards).Returns(dashboardMock.Object);
            repoMock.SetupGet(r => r.Interruptions).Returns(interruptionMock.Object);

            adapter = new OctopusAdapter(repoMock.Object, 15000);
            adapter.DeploymentSummaryChanged += Adapter_DeploymentSummaryChanged;
        }

        [Given(@"I have (.*) builds with a state of '(.*)'")]
        public void GivenIHaveBuildsWithAStateOf(int numberOfBuilds, TaskState state)
        {
            for(int i=0;i<numberOfBuilds;i++)
            {
                string istr = i.ToString();

                dashboard.Projects.Add(new DashboardProjectResource
                {
                    Id = istr,
                    Name = istr,
                });

                dashboard.Environments.Add(new DashboardEnvironmentResource
                {
                    Id = istr,
                    Name = istr,
                });

                TaskState[] nonComplete = { TaskState.Queued, TaskState.Executing, TaskState.Cancelling };
                TaskState[] errors = { TaskState.Failed, TaskState.TimedOut };

                dashboard.Items.Add(new DashboardItemResource
                {
                    CompletedTime = !nonComplete.Contains(state) ? DateTimeOffset.Now : (DateTimeOffset?)null,
                    DeploymentId = istr,
                    TaskId = istr,
                    State = state,
                    IsCompleted = !nonComplete.Contains(state),
                    ProjectId = istr,
                    EnvironmentId = istr,
                    IsCurrent = true,
                    HasWarningsOrErrors = errors.Contains(state),
                });
            }
        }

        [Given(@"the build has pending interruptions: '(.*)'")]
        public void GivenTheBuildHasPendingInterruptions(bool hasPendingInterruptions)
        {
            dashboard.Items.ForEach(i => i.HasPendingInterruptions = hasPendingInterruptions);
        }

        [Given(@"the build has warning or errors: '(.*)'")]
        public void GivenTheBuildHasWarningOrErrors(bool hasWarningsOrErrors)
        {
            dashboard.Items.ForEach(i => i.HasWarningsOrErrors = hasWarningsOrErrors);
        }

        [When(@"the repository is polled")]
        public void WhenTheRepositoryIsPolled()
        {
            adapter.Poll();
        }

        [Then(@"the Deployment Summary event fires")]
        public void ThenTheDeploymentSummaryEventFires()
        {
            Assert.IsNotNull(_args);
        }

        [Then(@"the deployment summary dictionary has (.*) entries")]
        public void ThenTheDeploymentSummaryDictionaryHasEntries(int numberOfEntries)
        {
            Assert.IsNotNull(_args);
            Assert.AreEqual(numberOfEntries, _args.Summary.Count);
        }

        [Then(@"the deployment summary dictionary has an entry for '(.*)' with a count of (.*)")]
        public void ThenTheDeploymentSummaryDictionaryHasAnEntryForWithACountOf(DeploymentStatus status, int count)
        {
            Assert.IsNotNull(_args);
            Assert.IsTrue(_args.Summary.ContainsKey(status));
            Assert.AreEqual(count, _args.Summary[status]);
        }

        private void Adapter_DeploymentSummaryChanged(object sender, DeploymentSummaryEventArgs e)
        {
            _args = e;
        }
    }
}
