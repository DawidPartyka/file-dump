Feature: CashBagToStringTest
	Test checking the ToString() method of
	class CashBag

@CashBagToString
Scenario: CashBag ToString test
	Given Currency CHF <amountCHF>
	And the Currency USD <amountUSD>
	When used method ToString
	Then the result should be <result>
	Examples:
	| amountCHF | amountUSD | result |
	| 5 | 6 | {[5 CHF][6 USD]} |
	| 122 | 238 | {[122 CHF][238 USD]} |