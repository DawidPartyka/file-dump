using NUnit.Framework;
using NUnit.Samples.Cash;
using System;
using TechTalk.SpecFlow;

namespace SpecFlowProject.Features
{
    [Binding]
    public class CashBagAppendSteps
    {
        CashBag testedCashBag, cb1, cb2;
        [Given(@"CashBag has Values \((.*), CHF\) \((.*), CHF\) \((.*), CAD\) \((.*), USD\)")]
        public void GivenCashBagHasValuesCHFCHFCADUSD(int p0, int p1, int p2, int p3)
        {
            cb1 = new CashBag(new Cash(p0, "CHF"), new Cash(p2, "CAD"));
            cb2 = new CashBag(new Cash(p1, "CHF"), new Cash(p3, "USD"));
        }
        
        [When(@"CashBag Append Two CashBags")]
        public void WhenCashBagAppendTwoCashBags()
        {
            testedCashBag = new CashBag(cb1, cb2);
        }
        
        [Then(@"result should be \((.*), CAD\) \((.*), USD\)")]
        public void ThenResultShouldBeCADUSD(int p0, int p1)
        {
            Assert.AreEqual(testedCashBag, new CashBag(new Cash(p0, "CAD"), new Cash(p1, "USD")));
        }
    }
}
