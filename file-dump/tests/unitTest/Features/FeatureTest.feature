Feature: CashBagAppend
	CashBag.Append Test

@CashBagAppendTest
Scenario: CashBagAppend Value comparison test
	Given CashBag has Values (6, CHF) (-6, CHF) (1, CAD) (7, USD)
	When CashBag Append Two CashBags
	Then result should be (1, CAD) (7, USD)