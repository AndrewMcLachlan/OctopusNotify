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
	Then an event fires: 'true'
	And the event has <numberOfBuilds> builds
	And the event has a build with an id of '<outputId>'

Examples:
| outputId | previousId | seconds | hasErrors | previousHasErrors | state    | previousState | numberOfBuilds |
| 1234     | 1234       | 100     | false     | false             | Success  | Success       | 1              |
|          | 1234       | -10     | false     | false             | Success  | Success       | 0              |
|          | 1234       | 100     | true      | false             | Failed   | Success       | 0              |
|          | 1234       | 100     | true      | false             | TimedOut | Success       | 0              |
|          | 1234       | 100     | true      | false             | Success  | Success       | 0              |
|          | 1234       | 100     | false     | true              | Success  | Success       | 0              |
|          | 1234       | -10     | false     | true              | Success  | Success       | 0              |
|          | 1234       | 100     | true      | true              | Failed   | Success       | 0              |
|          | 1234       | 100     | true      | true              | TimedOut | Success       | 0              |
|          | 1234       | 100     | true      | true              | Success  | Success       | 0              |
|          | 1234       | 100     | false     | true              | Success  | Failed        | 0              |
|          | 1234       | 100     | true      | true              | Failed   | Failed        | 0              |
|          | 1234       | 100     | true      | true              | TimedOut | Failed        | 0              |
|          | 1234       | 100     | true      | true              | Success  | Failed        | 0              |
| 1234     | 5678       | 100     | false     | true              | Success  | Failed        | 1              |
|          | 1234       | 100     | false     | true              | Success  | TimedOut      | 0              |
|          | 1234       | 100     | true      | true              | Failed   | TimedOut      | 0              |
|          | 1234       | 100     | true      | true              | TimedOut | TimedOut      | 0              |
| 1234     | 5678       | 100     | false     | true              | Success  | TimedOut      | 1              |

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
	Then an event fires: 'true'
	And the event has <numberOfBuilds> builds
	And the event has a build with an id of '<outputId>'

Examples:
| outputId | previousId | seconds | hasErrors | previousHasErrors | state    | previousState | numberOfBuilds |
|          | 1234       | 100     | false     | false             | Failed   | Success       | 0              |
|          | 1234       | -10     | true      | false             | Failed   | Success       | 0              |
| 1234     | 1234       | 100     | true      | false             | Failed   | Success       | 1              |
|          | 1234       | 100     | true      | false             | Success  | Success       | 0              |
|          | 1234       | 100     | false     | true              | Failed   | Failed        | 0              |
|          | 1234       | 100     | true      | true              | Failed   | Failed        | 0              |
| 1234     | 5678       | 100     | true      | true              | Failed   | Failed        | 1              |
|          | 1234       | 100     | false     | true              | Failed   | TimedOut      | 0              |
|          | 1234       | 100     | true      | true              | Failed   | TimedOut      | 0              |
| 1234     | 5678       | 100     | true      | true              | Failed   | TimedOut      | 1              |
|          | 1234       | 100     | false     | false             | TimedOut | Success       | 0              |
|          | 1234       | -10     | true      | false             | TimedOut | Success       | 0              |
| 1234     | 1234       | 100     | true      | false             | TimedOut | Success       | 1              |
|          | 1234       | 100     | false     | true              | TimedOut | Failed        | 0              |
|          | 1234       | 100     | true      | true              | TimedOut | Failed        | 0              |
| 1234     | 5678       | 100     | true      | true              | TimedOut | Failed        | 1              |
|          | 1234       | 100     | false     | true              | TimedOut | TimedOut      | 0              |
|          | 1234       | 100     | true      | true              | TimedOut | TimedOut      | 0              |
| 1234     | 5678       | 100     | true      | true              | TimedOut | TimedOut      | 1              |

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
	Then an event fires: 'true'
	And the event has <numberOfBuilds> builds
	And the event has a build with an id of '<outputId>'

Examples:
| outputId | previousId | seconds | hasErrors | previousHasErrors | state    | previousState | numberOfBuilds |
|          | 1234       | 100     | false     | false             | Failed   | Success       | 0              |
|          | 1234       | -10     | true      | true              | Failed   | Failed        | 0              |
| 1234     | 1234       | 100     | true      | true              | Failed   | Failed        | 1              |
|          | 1234       | 100     | false     | true              | Failed   | Failed        | 0              |
|          | 1234       | 100     | true      | false             | Failed   | Success       | 0              |
|          | 5678       | 100     | true      | true              | Failed   | Failed        | 0              |
|          | 1234       | -10     | true      | true              | Failed   | TimedOut      | 0              |
| 1234     | 1234       | 100     | true      | true              | Failed   | TimedOut      | 1              |
|          | 1234       | 100     | false     | true              | Failed   | TimedOut      | 0              |
|          | 5678       | 100     | true      | true              | Failed   | TimedOut      | 0              |
|          | 1234       | 100     | false     | false             | TimedOut | Success       | 0              |
|          | 1234       | -10     | true      | true              | TimedOut | Failed        | 0              |
| 1234     | 1234       | 100     | true      | true              | TimedOut | Failed        | 1              |
|          | 1234       | 100     | false     | true              | TimedOut | Failed        | 0              |
|          | 1234       | 100     | true      | false             | TimedOut | Success       | 0              |
|          | 5678       | 100     | true      | true              | TimedOut | Failed        | 0              |
|          | 1234       | -10     | true      | true              | TimedOut | TimedOut      | 0              |
| 1234     | 1234       | 100     | true      | true              | TimedOut | TimedOut      | 1              |
|          | 1234       | 100     | false     | true              | TimedOut | TimedOut      | 0              |
|          | 5678       | 100     | true      | true              | TimedOut | TimedOut      | 0              |

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
	Then an event fires: 'true'
	And the event has <numberOfBuilds> builds
	And the event has a build with an id of '<outputId>'

Examples:
| outputId | previousId | seconds | hasErrors | previousHasErrors | state    | previousState | numberOfBuilds |
|          | 1234       | 100     | false     | false             | Success  | Success       | 0              |
|          | 1234       | -10     | true      | true              | Failed   | Failed        | 0              |
|          | 1234       | 100     | true      | true              | Failed   | Failed        | 0              |
| 1234     | 1234       | 100     | false     | true              | Success  | Failed        | 1              |
|          | 1234       | 100     | true      | false             | Failed   | Success       | 0              |
|          | 5678       | 100     | false     | true              | Success  | Failed        | 0              |
|          | 1234       | 100     | false     | false             | Success  | Success       | 0              |
|          | 1234       | -10     | true      | true              | Failed   | Failed        | 0              |
|          | 1234       | 100     | true      | true              | Failed   | Failed        | 0              |
| 1234     | 1234       | 100     | false     | true              | Success  | Failed        | 1              |
|          | 1234       | 100     | true      | false             | Failed   | Success       | 0              |
|          | 5678       | 100     | false     | true              | Success  | Failed        | 0              |
|          | 1234       | -10     | true      | true              | Failed   | TimedOut      | 0              |
|          | 1234       | 100     | true      | true              | Failed   | TimedOut      | 0              |
| 1234     | 1234       | 100     | false     | true              | Success  | TimedOut      | 1              |
|          | 5678       | 100     | false     | true              | Success  | TimedOut      | 0              |
|          | 1234       | -10     | true      | true              | Failed   | TimedOut      | 0              |
|          | 1234       | 100     | true      | true              | Failed   | TimedOut      | 0              |
| 1234     | 1234       | 100     | false     | true              | Success  | TimedOut      | 1              |
|          | 5678       | 100     | false     | true              | Success  | TimedOut      | 0              |
|          | 1234       | -10     | true      | true              | TimedOut | Failed        | 0              |
|          | 1234       | 100     | true      | true              | TimedOut | Failed        | 0              |
|          | 1234       | 100     | true      | false             | TimedOut | Success       | 0              |
|          | 1234       | -10     | true      | true              | TimedOut | Failed        | 0              |
|          | 1234       | 100     | true      | true              | TimedOut | Failed        | 0              |
|          | 1234       | 100     | true      | false             | TimedOut | Success       | 0              |
|          | 1234       | -10     | true      | true              | TimedOut | TimedOut      | 0              |
|          | 1234       | 100     | true      | true              | TimedOut | TimedOut      | 0              |
|          | 1234       | -10     | true      | true              | TimedOut | TimedOut      | 0              |
|          | 1234       | 100     | true      | true              | TimedOut | TimedOut      | 0              |