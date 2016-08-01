Feature: Connection Events
	Test that the connection events fire correctly

Scenario Outline: Detect connection events
	Given an adapter with a broken connection '<previousBrokenConnection>'
	And a current connection is broken '<currentBrokenConnection>'
	When the repository is polled
	Then the connection lost event will have fired '<connectionLostEventFired>'
	And the connection restored event will have fired '<connectionRestoredEventFired>'

	Examples: 
	| previousBrokenConnection | currentBrokenConnection | connectionLostEventFired | connectionRestoredEventFired |
	| false                    | false                   | false                    | false                        |
	| true                     | false                   | false                    | true                         |
	| false                    | true                    | true                     | false                        |
	| true                     | true                    | false                    | false                        |