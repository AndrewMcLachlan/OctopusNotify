using System;
using System.Collections.Generic;
using System.ComponentModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OctopusNotify.App.Models;
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
            model.IntervalTime = 0;
            model.ServerUrl = null;
            model.IsValid = false;
            model.PropertyChanged += Model_PropertyChanged;
        }

        private void Model_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            eventArgsList.Add(e);
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
        
        [When(@"the IntervalTime property is changed")]
        public void WhenTheIntervalTimePropertyIsChanged()
        {
            model.IntervalTime = model.IntervalTime+1;
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

    }
}
