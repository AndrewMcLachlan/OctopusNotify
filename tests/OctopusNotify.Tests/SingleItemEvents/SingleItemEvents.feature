Feature: Single Item Events
	Test that the various item events fire when expected

@CompletedEvent
Scenario Outline: Detect completed build
	Given a build with an id of '1234'
	And a completed time of now plus '<seconds>' seconds
	And has errors or warnings '<hasErrors>'
	And has a state of '<state>'
	And a previous build with an id of '<previousId>'
	And a previous build with errors or warnings '<previousHasErrors>'
	And a previous build with a state of '<previousState>'
	When the repository is polled
	Then an event fires: '<eventFires>'
	And the event type is '<eventType>'
	And the event has <numberOfBuilds> builds
	And the event has a build with an id of '<outputId>'

Examples:
| outputId | previousId | seconds | hasErrors | previousHasErrors | state   | previousState | eventFires | numberOfBuilds | eventType |
| 1234     | 1234       | 100     | false     | false             | Success | Success       | true       | 1              | Completed |
|          | 1234       | -10     | false     | false             | Success | Success       | false      | 0              |           |
|          | 1234       | 100     | true      | false             | Success | Success       | false      | 0              |           |
|          | 1234       | 100     | false     | true              | Success | Failed        | false      | 0              |           |
|          | 1234       | 100     | true      | true              | Success | Failed        | false      | 0              |           |
| 1234     | 5678       | 100     | false     | true              | Success | Failed        | true       | 1              | Completed |
|          | 1234       | 100     | false     | true              | Success | TimedOut      | false      | 0              |           |
|          | 1234       | 100     | true      | true              | Success | TimedOut      | false      | 0              |           |
| 1234     | 5678       | 100     | false     | true              | Success | TimedOut      | true       | 1              | Completed |

@FailedNewEvent
Scenario Outline: Detect newly failed build
	Given a build with an id of '1234'
	And a completed time of now plus '<seconds>' seconds
	And has errors or warnings '<hasErrors>'
	And has a state of '<state>'
	And a previous build with an id of '<previousId>'
	And a previous build with errors or warnings '<previousHasErrors>'
	And a previous build with a state of '<previousState>'
	When the repository is polled
	Then an event fires: '<eventFires>'
	And the event type is '<eventType>'
	And the event has <numberOfBuilds> builds
	And the event has a build with an id of '<outputId>'

Examples:
| outputId | previousId | seconds | hasErrors | previousHasErrors | state    | previousState | eventFires | numberOfBuilds | eventType |
|          | 1234       | 100     | false     | false             | Failed   | Success       | false      | 0              |           |
|          | 1234       | -10     | true      | false             | Failed   | Success       | false      | 0              |           |
| 1234     | 1234       | 100     | true      | false             | Failed   | Success       | true       | 1              | FailedNew |
|          | 1234       | 100     | false     | true              | Failed   | Failed        | false      | 0              |           |
|          | 1234       | 100     | true      | true              | Failed   | Failed        | false      | 0              |           |
| 1234     | 5678       | 100     | true      | true              | Failed   | Failed        | true       | 1              | FailedNew |
|          | 1234       | 100     | false     | true              | Failed   | TimedOut      | false      | 0              |           |
|          | 1234       | 100     | true      | true              | Failed   | TimedOut      | false      | 0              |           |
| 1234     | 5678       | 100     | true      | true              | Failed   | TimedOut      | true       | 1              | FailedNew |
|          | 1234       | 100     | false     | false             | TimedOut | Success       | false      | 0              |           |
|          | 1234       | -10     | true      | false             | TimedOut | Success       | false      | 0              |           |
| 1234     | 1234       | 100     | true      | false             | TimedOut | Success       | true       | 1              | FailedNew |
|          | 1234       | 100     | false     | true              | TimedOut | Failed        | false      | 0              |           |
|          | 1234       | 100     | true      | true              | TimedOut | Failed        | false      | 0              |           |
| 1234     | 5678       | 100     | true      | true              | TimedOut | Failed        | true       | 1              | FailedNew |
|          | 1234       | 100     | false     | true              | TimedOut | TimedOut      | false      | 0              |           |
|          | 1234       | 100     | true      | true              | TimedOut | TimedOut      | false      | 0              |           |
| 1234     | 5678       | 100     | true      | true              | TimedOut | TimedOut      | true       | 1              | FailedNew |

@FailedEvent
Scenario Outline: Detect failed build
	Given a build with an id of '1234'
	And a completed time of now plus '<seconds>' seconds
	And has errors or warnings '<hasErrors>'
	And has a state of '<state>'
	And a previous build with an id of '<previousId>'
	And a previous build with errors or warnings '<previousHasErrors>'
	And a previous build with a state of '<previousState>'
	When the repository is polled
	Then an event fires: '<eventFires>'
	And the event type is '<eventType>'
	And the event has <numberOfBuilds> builds
	And the event has a build with an id of '<outputId>'

Examples:
| outputId | previousId | seconds | hasErrors | previousHasErrors | state    | previousState | eventFires | numberOfBuilds | eventType |
|          | 1234       | 100     | false     | false             | Failed   | Success       | false      | 0              |           |
|          | 1234       | -10     | true      | true              | Failed   | Failed        | false      | 0              |           |
| 1234     | 1234       | 100     | true      | true              | Failed   | Failed        | true       | 1              | Failed    |
|          | 1234       | 100     | false     | true              | Failed   | Failed        | false      | 0              |           |
|          | 1234       | 100     | true      | false             | Failed   | Success       | false      | 0              |           |
|          | 5678       | 100     | true      | true              | Failed   | Failed        | false      | 0              |           |
|          | 1234       | -10     | true      | true              | Failed   | TimedOut      | false      | 0              |           |
| 1234     | 1234       | 100     | true      | true              | Failed   | TimedOut      | true       | 1              | Failed    |
|          | 1234       | 100     | false     | true              | Failed   | TimedOut      | false      | 0              |           |
|          | 5678       | 100     | true      | true              | Failed   | TimedOut      | false      | 0              |           |
|          | 1234       | 100     | false     | false             | TimedOut | Success       | false      | 0              |           |
|          | 1234       | -10     | true      | true              | TimedOut | Failed        | false      | 0              |           |
| 1234     | 1234       | 100     | true      | true              | TimedOut | Failed        | true       | 1              | Failed    |
|          | 1234       | 100     | false     | true              | TimedOut | Failed        | false      | 0              |           |
|          | 1234       | 100     | true      | false             | TimedOut | Success       | false      | 0              |           |
|          | 5678       | 100     | true      | true              | TimedOut | Failed        | false      | 0              |           |
|          | 1234       | -10     | true      | true              | TimedOut | TimedOut      | false      | 0              |           |
| 1234     | 1234       | 100     | true      | true              | TimedOut | TimedOut      | true       | 1              | Failed    |
|          | 1234       | 100     | false     | true              | TimedOut | TimedOut      | false      | 0              |           |
|          | 5678       | 100     | true      | true              | TimedOut | TimedOut      | false      | 0              |           |

@FixedEvent
Scenario Outline: Detect fixed build
	Given a build with an id of '1234'
	And a completed time of now plus '<seconds>' seconds
	And has errors or warnings '<hasErrors>'
	And has a state of '<state>'
	And a previous build with an id of '<previousId>'
	And a previous build with errors or warnings '<previousHasErrors>'
	And a previous build with a state of '<previousState>'
	When the repository is polled
	Then an event fires: '<eventFires>'
	And the event type is '<eventType>'
	And the event has <numberOfBuilds> builds
	And the event has a build with an id of '<outputId>'

Examples:
| outputId | previousId | seconds | hasErrors | previousHasErrors | state    | previousState | eventFires | numberOfBuilds | eventType |
|          | 1234       | 100     | false     | false             | Success  | Success       | false      | 0              |           |
|          | 1234       | -10     | true      | true              | Failed   | Failed        | false      | 0              |           |
|          | 1234       | 100     | true      | true              | Failed   | Failed        | false      | 0              |           |
| 1234     | 1234       | 100     | false     | true              | Success  | Failed        | true       | 1              | Fixed     |
|          | 1234       | 100     | true      | false             | Failed   | Success       | false      | 0              |           |
|          | 5678       | 100     | false     | true              | Success  | Failed        | false      | 0              |           |
|          | 1234       | 100     | false     | false             | Success  | Success       | false      | 0              |           |
|          | 1234       | -10     | true      | true              | Failed   | Failed        | false      | 0              |           |
|          | 1234       | 100     | true      | true              | Failed   | Failed        | false      | 0              |           |
| 1234     | 1234       | 100     | false     | true              | Success  | Failed        | true       | 1              | Fixed     |
|          | 1234       | 100     | true      | false             | Failed   | Success       | false      | 0              |           |
|          | 5678       | 100     | false     | true              | Success  | Failed        | false      | 0              |           |
|          | 1234       | -10     | true      | true              | Failed   | TimedOut      | false      | 0              |           |
|          | 1234       | 100     | true      | true              | Failed   | TimedOut      | false      | 0              |           |
| 1234     | 1234       | 100     | false     | true              | Success  | TimedOut      | true       | 1              | Fixed     |
|          | 5678       | 100     | false     | true              | Success  | TimedOut      | false      | 0              |           |
|          | 1234       | -10     | true      | true              | Failed   | TimedOut      | false      | 0              |           |
|          | 1234       | 100     | true      | true              | Failed   | TimedOut      | false      | 0              |           |
| 1234     | 1234       | 100     | false     | true              | Success  | TimedOut      | true       | 1              | Fixed     |
|          | 5678       | 100     | false     | true              | Success  | TimedOut      | false      | 0              |           |
|          | 1234       | -10     | true      | true              | TimedOut | Failed        | false      | 0              |           |
|          | 1234       | 100     | true      | true              | TimedOut | Failed        | false      | 0              |           |
|          | 1234       | 100     | true      | false             | TimedOut | Success       | false      | 0              |           |
|          | 1234       | -10     | true      | true              | TimedOut | Failed        | false      | 0              |           |
|          | 1234       | 100     | true      | true              | TimedOut | Failed        | false      | 0              |           |
|          | 1234       | 100     | true      | false             | TimedOut | Success       | false      | 0              |           |
|          | 1234       | -10     | true      | true              | TimedOut | TimedOut      | false      | 0              |           |
|          | 1234       | 100     | true      | true              | TimedOut | TimedOut      | false      | 0              |           |
|          | 1234       | -10     | true      | true              | TimedOut | TimedOut      | false      | 0              |           |
|          | 1234       | 100     | true      | true              | TimedOut | TimedOut      | false      | 0              |           |