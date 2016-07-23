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
	And the property changed event at index '1' fired with name 'AlertOnNewFailedBuild'
	And the number of fired events will be 2

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
	Then the property changed event at index '1' fired with name 'AlertOnFixedBuild'
	And the number of fired events will be 2

@NotifyChange
Scenario: Should trigger property changed event for IntervalTime
	Given a SettingsViewModel instance
	When the IntervalTime property is changed
	Then the property changed event at index '0' fired with name 'IntervalTime'
	And the number of fired events will be 1