Feature: Single Item Events
	Test that the various item events fire when expected

@CompletedEvent
Scenario Outline: Detect completed build
	Given a build with an id of '1234'
	And a completed time of now plus '<seconds>' seconds
	And has errors or warnings '<hasErrors>'
	And a previous build with an id of '<previousId>'
	And a previous build with errors or warnings '<previousHasErrors>'
	When the repository is polled
	Then an event fires: '<eventFires>'
	And the event type is '<eventType>'
	And the event has <numberOfBuilds> builds
	And the event has a build with an id of '<outputId>'

Examples: 
| outputId | previousId | seconds | hasErrors | previousHasErrors | eventFires | numberOfBuilds | eventType |
| 1234     | 1234       | 100     | false     | false             | true       | 1              | Completed |
|          | 1234       | -10     | false     | false             | false      | 0              |           |
|          | 1234       | 100     | true      | false             | false      | 0              |           |
|          | 1234       | 100     | false     | true              | false      | 0              |           |
|          | 1234       | 100     | true      | true              | false      | 0              |           |
| 1234     | 5678       | 100     | false     | true              | true       | 1              | Completed |

@FailedNewEvent
Scenario Outline: Detect newly failed build
	Given a build with an id of '1234'
	And a completed time of now plus '<seconds>' seconds
	And has errors or warnings '<hasErrors>'
	And a previous build with an id of '<previousId>'
	And a previous build with errors or warnings '<previousHasErrors>'
	When the repository is polled
	Then an event fires: '<eventFires>'
	And the event type is '<eventType>'
	And the event has <numberOfBuilds> builds
	And the event has a build with an id of '<outputId>'

Examples: 
| outputId | previousId | seconds | hasErrors | previousHasErrors | eventFires | numberOfBuilds | eventType |
|          | 1234       | 100     | false     | false             | false      | 0              |           |
|          | 1234       | -10     | true      | false             | false      | 0              |           |
| 1234     | 1234       | 100     | true      | false             | true       | 1              | FailedNew |
|          | 1234       | 100     | false     | true              | false      | 0              |           |
|          | 1234       | 100     | true      | true              | false      | 0              |           |
| 1234     | 5678       | 100     | true      | true              | true       | 1              | FailedNew |

@FailedEvent
Scenario Outline: Detect failed build
	Given a build with an id of '1234'
	And a completed time of now plus '<seconds>' seconds
	And has errors or warnings '<hasErrors>'
	And a previous build with an id of '<previousId>'
	And a previous build with errors or warnings '<previousHasErrors>'
	When the repository is polled
	Then an event fires: '<eventFires>'
	And the event type is '<eventType>'
	And the event has <numberOfBuilds> builds
	And the event has a build with an id of '<outputId>'

Examples: 
| outputId | previousId | seconds | hasErrors | previousHasErrors | eventFires | numberOfBuilds | eventType |
|          | 1234       | 100     | false     | false             | false      | 0              |           |
|          | 1234       | -10     | true      | true              | false      | 0              |           |
| 1234     | 1234       | 100     | true      | true              | true       | 1              | Failed    |
|          | 1234       | 100     | false     | true              | false      | 0              |           |
|          | 1234       | 100     | true      | false             | false      | 0              |           |
|          | 5678       | 100     | true      | true              | false      | 0              |           |

@FixedEvent
Scenario Outline: Detect fixed build
	Given a build with an id of '1234'
	And a completed time of now plus '<seconds>' seconds
	And has errors or warnings '<hasErrors>'
	And a previous build with an id of '<previousId>'
	And a previous build with errors or warnings '<previousHasErrors>'
	When the repository is polled
	Then an event fires: '<eventFires>'
	And the event type is '<eventType>'
	And the event has <numberOfBuilds> builds
	And the event has a build with an id of '<outputId>'

Examples: 
| outputId | previousId | seconds | hasErrors | previousHasErrors | eventFires | numberOfBuilds | eventType |
|          | 1234       | 100     | false     | false             | false      | 0              |           |
|          | 1234       | -10     | true      | true              | false      | 0              |           |
|          | 1234       | 100     | true      | true              | false      | 0              |           |
| 1234     | 1234       | 100     | false     | true              | true       | 1              | Fixed     |
|          | 1234       | 100     | true      | false             | false      | 0              |           |
|          | 5678       | 100     | false     | true              | false      | 0              |           |