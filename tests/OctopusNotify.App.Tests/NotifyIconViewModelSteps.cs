using System;
using System.Collections.Generic;
using System.IO.Packaging;
using System.Windows;
using System.Windows.Media.Imaging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using OctopusNotify.App.Ioc;
using OctopusNotify.App.Models;
using OctopusNotify.App.ViewModels;
using OctopusNotify.Model;
using TechTalk.SpecFlow;

namespace OctopusNotify.App.Tests
{
    [Binding]
    [Scope(Feature = "Notify Icon View Model")]
    public class NotifyIconViewModelSteps
    {
        private MockContainer MockContainer { get; set; }
        private Mock<IDeploymentRepositoryAdapter> MockDeploymentRepositoryAdapter { get; set; }
        private NotifyIconViewModel ViewModel { get; set; }

        private DeploymentSummaryEventArgs DeploymentSummaryEventArgs { get; set; }

        Dictionary<NotifyIconState, string> IconStates = new Dictionary<NotifyIconState, string>
        {
            { NotifyIconState.Disconnected, "pack://application:,,,/OctopusNotify;component/NotifyIcons/Disconnected.ico" },
            { NotifyIconState.Connected, "pack://application:,,,/OctopusNotify;component/NotifyIcons/Connected.ico" },
            { NotifyIconState.Error, "pack://application:,,,/OctopusNotify;component/NotifyIcons/Red.ico" },
        };

        [BeforeScenario]
        public void Setup()
        {
            MockDeploymentRepositoryAdapter = new Mock<IDeploymentRepositoryAdapter>();

            MockContainer = new MockContainer(MockDeploymentRepositoryAdapter);

            // Set the server URL so the settings command isn't run
            Properties.Settings.Default.ServerUrl = "http://localhost";

            Container.Current = MockContainer;
            Container.Current.Configure();

            PackUriHelper.Create(new Uri("reliable://0"));
            new FrameworkElement();
            Application.ResourceAssembly = typeof(App).Assembly;
        }

        [Given(@"I have a ViewModel")]
        public void GivenIHaveAViewModel()
        {
            ViewModel = new NotifyIconViewModel();
        }

        [Given(@"the Adapter is connected")]
        public void GivenTheAdapterIsConnected()
        {
            MockDeploymentRepositoryAdapter.Setup(a => a.StartPolling()).Raises(a => a.ConnectionRestored += null, EventArgs.Empty);
        }

        [Given(@"the Contiainer throws a resolution error")]
        public void GivenTheContiainerThrowsAResolutionError()
        {
            MockContainer.ThrowsResolutionException = true;
            Container.Current.Configure();
        }

        [Given(@"I have a dashboard with (.*) '(.*)'")]
        public void GivenIHaveADashboardWith(int numberOfBuilds, DeploymentStatus status)
        {
            if (DeploymentSummaryEventArgs == null)
            {
                DeploymentSummaryEventArgs = new DeploymentSummaryEventArgs(new Dictionary<DeploymentStatus, int>
                {
                    {status, numberOfBuilds }
                });
            }
            else
            {
                Dictionary<DeploymentStatus, int> newDic = new Dictionary<DeploymentStatus, int>(DeploymentSummaryEventArgs.Summary as IDictionary<DeploymentStatus, int>);
                newDic.Add(status, numberOfBuilds);

                DeploymentSummaryEventArgs = new DeploymentSummaryEventArgs(newDic);
            }
        }

        [When(@"the DeploymentSummaryChanged event is raised")]
        public void WhenTheDeploymentSummaryChangedEventIsRaised()
        {
            MockDeploymentRepositoryAdapter.Raise(a => a.DeploymentSummaryChanged += null, DeploymentSummaryEventArgs);
        }


        [When(@"ConnectionError event is received")]
        public void WhenConnectionErrorEventIsReceived()
        {
            MockDeploymentRepositoryAdapter.Raise(a => a.ConnectionError += null, EventArgs.Empty);
        }

        [When(@"the VieWModel is created")]
        public void WhenTheVieWModelIsCreated()
        {
            ViewModel = new NotifyIconViewModel();
        }

        [Then(@"the NotifyIcon is set to '(.*)'")]
        public void ThenTheNotifyIconIsSetTo(NotifyIconState iconState)
        {
            Assert.IsNotNull(ViewModel);
            Assert.IsNotNull(ViewModel.NotifyIcon);
            Assert.IsInstanceOfType(ViewModel.NotifyIcon, typeof(BitmapImage));
            Assert.AreEqual(IconStates[iconState], ((BitmapImage)ViewModel.NotifyIcon).UriSource.OriginalString);
        }

        [Then(@"the StateSummary is set to '(.*)'")]
        public void ThenTheStateSummaryIsSetTo(string summary)
        {
            summary = summary.Replace(@"\r\n", "\r\n");

            Assert.IsNotNull(ViewModel);
            Assert.IsNotNull(ViewModel.StateSummary);

            Assert.AreEqual(summary, ViewModel.StateSummary);
        }

    }
}
