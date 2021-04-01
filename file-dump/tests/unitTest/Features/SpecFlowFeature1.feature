Feature: CashBagTests
	Testing CashBag class

@unitTestCashBag
Scenario: CashBag Negate Value test
	Given CashBag is (5, CHF) (4, PLN)
	When CashBag is Negated
	Then result shoud be (-5, CHF) (-4, PLN)