using System;
using System.Collections.Generic;
using System.ComponentModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OctopusNotify.App.ViewModels;
using TechTalk.SpecFlow;

namespace OctopusNotify.App.Tests.Settings
{
    [Binding]
    public class SettingsViewModelSteps
    {
        private SettingsViewModel model;
        protected List<PropertyChangedEventArgs> eventArgsList = new List<PropertyChangedEventArgs>();

        [Given(@"a SettingsViewModel instance")]
        public void GivenASettingsViewModelInstance()
        {
            model = new SettingsViewModel();
            model.AlertOnFailedBuild = false;
            model.AlertOnFixedBuild = false;
            model.AlertOnNewFailedBuild = false;
            model.AlertOnSuccessfulBuild = false;
            model.RunOnStartup = false;
            model.PollingInterval = 0;
            model.ServerUrl = null;
            model.IsValid = false;
            model.PropertyChanged += Model_PropertyChanged;
        }

        [Given(@"the AlertOnFailedBuild property is set to '(.*)'")]
        public void GivenTheAlertOnFailedBuildPropertyIsSetTo(bool value)
        {
            model.AlertOnFailedBuild = value;
        }

        [Given(@"the AlertOnNewFailedBuild property is set to '(.*)'")]
        public void GivenTheAlertOnNewFailedBuildPropertyIsSetTo(bool value)
        {
            model.AlertOnNewFailedBuild = value;
        }

        [Given(@"the AlertOnSuccessfulBuild property is set to '(.*)'")]
        public void GivenTheAlertOnSuccessfulBuildPropertyIsSetTo(bool value)
        {
            model.AlertOnSuccessfulBuild = value;
        }

        [Given(@"the AlertOnFixedBuild property is set to '(.*)'")]
        public void GivenTheAlertOnFixedBuildPropertyIsSetTo(bool value)
        {
            model.AlertOnFixedBuild = value;
        }


        [Given(@"the DisableFailedBuildAlerts property is set to '(.*)'")]
        [When(@"the DisableFailedBuildAlerts property is set to '(.*)'")]
        public void WhenTheDisableFailedBuildAlertsPropertyIsSetTo(bool value)
        {
            model.DisableFailedBuildAlerts = value;
        }

        [Given(@"the DisableSuccessfulBuildAlerts property is set to '(.*)'")]
        [When(@"the DisableSuccessfulBuildAlerts property is set to '(.*)'")]
        public void WhenTheDisableSuccessfulBuildAlertsPropertyIsSetTo(bool value)
        {
            model.DisableSuccessfulBuildAlerts = value;
        }

        [When(@"the ServerURL property is changed")]
        public void WhenTheServerURLPropertyIsChanged()
        {
            model.ServerUrl = new Uri("http://test.com/" + Guid.NewGuid().ToString());
        }

        [When(@"the IsValid property is changed")]
        public void WhenTheIsValidPropertyIsChanged()
        {
            model.IsValid = !model.IsValid;
        }

        [When(@"the RunOnStartup property is changed")]
        public void WhenTheRunOnStartupPropertyIsChanged()
        {
            model.RunOnStartup = !model.RunOnStartup;
        }

        [When(@"the AlertOnFailedBuild property is changed")]
        public void WhenTheAlertOnFailedBuildPropertyIsChanged()
        {
            model.AlertOnFailedBuild = !model.AlertOnFailedBuild;
        }

        [When(@"the AlertOnNewFailedBuild property is changed")]
        public void WhenTheAlertOnNewFailedBuildPropertyIsChanged()
        {
            model.AlertOnNewFailedBuild = !model.AlertOnNewFailedBuild;
        }

        [When(@"the AlertOnFixedBuild property is changed")]
        public void WhenTheAlertOnFixedBuildPropertyIsChanged()
        {
            model.AlertOnFixedBuild = !model.AlertOnFixedBuild;
        }

        [When(@"the AlertOnSuccessfulBuild property is changed")]
        public void WhenTheAlertOnSuccessfulBuildPropertyIsChanged()
        {
            model.AlertOnSuccessfulBuild = !model.AlertOnSuccessfulBuild;
        }

        [When(@"the PollingInterval property is changed")]
        public void WhenThePollingIntervalPropertyIsChanged()
        {
            model.PollingInterval = model.PollingInterval + 1;
        }

        [When(@"the BalloonTimeout property is changed")]
        public void WhenTheBalloonTimeoutPropertyIsChanged()
        {
            model.BalloonTimeout = model.BalloonTimeout + 1;
        }

        [When(@"the AlertOnGuidedFailure property is changed")]
        public void WhenTheAlertOnGuidedFailurePropertyIsChanged()
        {
            model.AlertOnGuidedFailure = !model.AlertOnGuidedFailure;
        }

        [When(@"the AlertOnManualStep property is changed")]
        public void WhenTheAlertOnManualStepPropertyIsChanged()
        {
            model.AlertOnManualStep = !model.AlertOnManualStep;
        }


        [Then(@"the property changed event fires with name '(.*)'")]
        [Then(@"the property changed event at index '(.*)' fired with name '(.*)'")]
        public void ThenThePropertyChangedEventAtIndexFiredWithName(int index, string name)
        {
            Assert.IsNotNull(eventArgsList);
            Assert.IsTrue(eventArgsList.Count > index, "Index out of bounds");
            Assert.AreEqual(name, eventArgsList[index].PropertyName);
        }

        [Then(@"the number of fired events will be (.*)")]
        public void ThenTheNumberOfFiredEventsWillBe(int count)
        {
            Assert.IsNotNull(eventArgsList);
            Assert.AreEqual(count, eventArgsList.Count);
        }

        [Then(@"the AlertOnFailedBuild property is set to '(.*)'")]
        public void ThenTheAlertOnFailedBuildPropertyIsSetTo(bool value)
        {
            Assert.IsNotNull(model);
            Assert.AreEqual(value, model.AlertOnFailedBuild);
        }

        [Then(@"the AlertOnNewFailedBuild property is set to '(.*)'")]
        public void ThenTheAlertOnNewFailedBuildPropertyIsSetTo(bool value)
        {
            Assert.IsNotNull(model);
            Assert.AreEqual(value, model.AlertOnNewFailedBuild);
        }

        [Then(@"the AlertOnSuccessfulBuild property is set to '(.*)'")]
        public void ThenTheAlertOnSuccessfulBuildPropertyIsSetTo(bool value)
        {
            Assert.IsNotNull(model);
            Assert.AreEqual(value, model.AlertOnSuccessfulBuild);
        }

        [Then(@"the AlertOnFixedBuild property is set to '(.*)'")]
        public void ThenTheAlertOnFixedBuildPropertyIsSetTo(bool value)
        {
            Assert.IsNotNull(model);
            Assert.AreEqual(value, model.AlertOnFixedBuild);
        }

        private void Model_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            eventArgsList.Add(e);
        }
    }
}
