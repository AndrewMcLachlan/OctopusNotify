Feature: Notify Icon View Model

@UnitTest
Scenario: Should set disconnected state
	Given the Adapter is connected
	And I have a ViewModel
	When ConnectionError event is received
	Then the NotifyIcon is set to 'Disconnected'
	And the StateSummary is set to 'Octopus Notify (Disconnected)'

Scenario: Should set disconnected state on resolution error
	Given the Contiainer throws a resolution error
	When the VieWModel is created
	Then the NotifyIcon is set to 'Disconnected'
	And the StateSummary is set to 'Octopus Notify (Disconnected)'

Scenario Outline: Should set the state summary
	Given I have a ViewModel
	And I have a dashboard with <NumberOfBuilds> '<DeploymentStatus>'
	When the DeploymentSummaryChanged event is raised
	Then the StateSummary is set to '<StateSummary>'

Examples:
| NumberOfBuilds | DeploymentStatus | StateSummary                 |
| 1              | Queued           | 1 Queued                     |
| 2              | Executing        | 2 Executing                  |
| 3              | Failed           | 3 Failed                     |
| 4              | Cancelled        | 4 Cancelled                  |
| 5              | TimedOut         | 5 Timed Out                  |
| 6              | Success          | 6 Successful                 |
| 7              | Cancelling       | 7 Cancelling                 |
| 8              | FailedNew        | 8 Failed                     |
| 9              | TimedOutNew      | 9 Timed Out                  |
| 10             | Queued           | 10 Queued                    |
| 100            | Fixed            | 100 Successful               |
| 1000           | ManualStep       | 1000 at a Manual Step        |
| 10000          | GuidedFailure    | 10000 in Guided Failure Mode |

Scenario: Should set the state summary with multiple states
	Given I have a ViewModel
	And I have a dashboard with 1 'Success'
	And I have a dashboard with 2 'Failed'
	When the DeploymentSummaryChanged event is raised
	Then the StateSummary is set to '1 Successful\r\n2 Failed'