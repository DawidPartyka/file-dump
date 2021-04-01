Feature: SpecFlowFeature2
	In order to avoid silly mistakes
	As a math idiot
	I want to be told the sum of two numbers

@mytag
Scenario: As user I enter udemy.com and try login to user account
	Given I enter udemy.com
	And I press <login>
	When I fill wrong email
	And I fill incorrect password
	And I submit the data
	Then I expect to see message informing about unsuccessful login attempt