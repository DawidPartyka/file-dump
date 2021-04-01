using NUnit.Framework;
using NUnit.Samples.Cash;
using System;
using TechTalk.SpecFlow;

namespace SpecFlowProject.Features
{
    [Binding]
    public class CashBagTestsSteps
    {
        CashBag testCashBag;
        //CashBag testCashBagCompare;
        [Given(@"CashBag is \((.*), CHF\) \((.*), PLN\)")]
        public void GivenCashBagIsCHFPLN(int p0, int p1)
        {
            testCashBag = new CashBag(new Cash(p0, "CHF"), new Cash(p1, "PLN"));
        }

        [When(@"CashBag is Negated")]
        public void WhenCashBagIsNegated()
        {
            testCashBag = (CashBag)testCashBag.Negate();
        }

        [Then(@"result shoud be \((.*), CHF\) \((.*), PLN\)")]
        public void ThenResultShoudBeCHFPLN(int p0, int p1)
        {
            Assert.AreEqual(new CashBag(new Cash(p0, "CHF"), new Cash(p1, "PLN")), testCashBag);
        }
    }
}
