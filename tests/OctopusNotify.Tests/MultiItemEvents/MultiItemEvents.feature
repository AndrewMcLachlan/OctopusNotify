Feature: Multi Item Events
	Test that the deployment and summary evetn fire correctly

@DeploymentSummary
Scenario Outline: Should fire the deployment summary event
	Given I have <Number of Builds> builds with a state of '<Task State>'
	And the build has pending interruptions: '<Has Pending Interruptions>'
	And the build has warning or errors: '<Has Warnings or Errors>'
	When the repository is polled
	Then the Deployment Summary event fires
	And the deployment summary dictionary has 1 entries
	And the deployment summary dictionary has an entry for '<Deployment Status>' with a count of <Number of Builds>

Examples:
| Number of Builds | Deployment Status | Task State | Has Pending Interruptions | Has Warnings or Errors |
| 1                | Queued            | Queued     | false                     | false                  |
| 2                | Executing         | Executing  | false                     | false                  |
| 3                | Failed            | Failed     | false                     | true                  |
| 4                | Cancelled         | Canceled   | false                     | false                  |
| 5                | TimedOut          | TimedOut   | false                     | true                  |
| 6                | Success           | Success    | false                     | false                  |
| 7                | Cancelling        | Cancelling | false                     | false                  |
| 10               | Queued            | Queued     | false                     | false                  |
| 3                | ManualStep        | Executing  | true                      | false                  |
| 3                | GuidedFailure     | Executing  | true                      | true                   |

#Scenario: Should set the state summary with multiple states
#	Given I have a ViewModel
#	And I have a dashboard with 1 'Success'
#	And I have a dashboard with 2 'Failed'
#	When the DeploymentSummaryChanged event is raised
#	Then the StateSummary is set to '1 Successful\r\n2 Failed'