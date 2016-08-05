Feature: Settings View Model

#TODO Complex scenarios where properties interact.

@NotifyChange
Scenario: Should trigger property changed event for ServerUrl
	Given a SettingsViewModel instance
	When the ServerURL property is changed
	Then the property changed event at index '0' fired with name 'CanTest'
	And the property changed event at index '1' fired with name 'ServerUrl'
	And the number of fired events will be 2

@NotifyChange
Scenario: Should trigger property changed event for IsValid
	Given a SettingsViewModel instance
	When the IsValid property is changed
	Then the property changed event at index '0' fired with name 'IsValid'
	And the number of fired events will be 1

@NotifyChange
Scenario: Should trigger property changed event for RunOnStartup
	Given a SettingsViewModel instance
	When the RunOnStartup property is changed
	Then the property changed event at index '0' fired with name 'RunOnStartup'
	And the number of fired events will be 1

@NotifyChange
Scenario: Should trigger property changed event for AlertOnFailedBuild
	Given a SettingsViewModel instance
	When the AlertOnFailedBuild property is changed
	Then the property changed event at index '0' fired with name 'AlertOnFailedBuild'
	And the number of fired events will be 1

@NotifyChange
Scenario: Should trigger property changed event for AlertOnNewFailedBuild
	Given a SettingsViewModel instance
	When the AlertOnNewFailedBuild property is changed
	Then the property changed event at index '0' fired with name 'AlertOnNewFailedBuild'
	And the number of fired events will be 1

@NotifyChange
Scenario: Should trigger property changed event for AlertOnFixedBuild
	Given a SettingsViewModel instance
	When the AlertOnFixedBuild property is changed
	Then the property changed event at index '0' fired with name 'AlertOnFixedBuild'
	And the number of fired events will be 1

@NotifyChange
Scenario: Should trigger property changed event for AlertOnSuccessfulBuild
	Given a SettingsViewModel instance
	When the AlertOnSuccessfulBuild property is changed
	Then the property changed event at index '0' fired with name 'AlertOnSuccessfulBuild'
	And the number of fired events will be 1

@NotifyChange
Scenario: Should trigger property changed event for AlertOnGuidedFailure
	Given a SettingsViewModel instance
	When the AlertOnGuidedFailure property is changed
	Then the property changed event at index '0' fired with name 'AlertOnGuidedFailure'
	And the number of fired events will be 1

@NotifyChange
Scenario: Should trigger property changed event for AlertOnManualStep
	Given a SettingsViewModel instance
	When the AlertOnManualStep property is changed
	Then the property changed event at index '0' fired with name 'AlertOnManualStep'
	And the number of fired events will be 1

@NotifyChange
Scenario: Should trigger property changed event for PollingInterval
	Given a SettingsViewModel instance
	When the PollingInterval property is changed
	Then the property changed event at index '0' fired with name 'PollingInterval'
	And the number of fired events will be 1

@NotifyChange
Scenario: Should trigger property changed event for BalloonTimeout
	Given a SettingsViewModel instance
	When the BalloonTimeout property is changed
	Then the property changed event at index '0' fired with name 'BalloonTimeout'
	And the number of fired events will be 1

Scenario: Should set the failure alerts when never is selected
	Given a SettingsViewModel instance
	And the AlertOnFailedBuild property is set to 'true'
	And the AlertOnNewFailedBuild property is set to 'true'
	When the DisableFailedBuildAlerts property is set to 'true'
	Then the AlertOnFailedBuild property is set to 'false'
	And the AlertOnNewFailedBuild property is set to 'false'

Scenario: Should set the success alerts when never is selected
	Given a SettingsViewModel instance
	And the AlertOnSuccessfulBuild property is set to 'true'
	And the AlertOnFixedBuild property is set to 'true'
	When the DisableSuccessfulBuildAlerts property is set to 'true'
	Then the AlertOnSuccessfulBuild property is set to 'false'
	And the AlertOnFixedBuild property is set to 'false'