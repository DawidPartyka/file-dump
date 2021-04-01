using NUnit.Framework;
using NUnit.Samples.Cash;
using System;
using TechTalk.SpecFlow;

namespace SpecFlowProject.Features
{
    [Binding]
    public class CashBagToStringTestSteps
    {
        string testCashBag;
        Cash usd, chf;
        [Given(@"Currency CHF (.*)")]
        public void GivenCurrencyCHF(int p0)
        {
            chf = new Cash(p0, "CHF");
        }
        
        [Given(@"the Currency USD (.*)")]
        public void GivenTheCurrencyUSD(int p0)
        {
            usd = new Cash(p0, "USD");
        }
        
        [When(@"used method ToString")]
        public void WhenUsedMethodToString()
        {
            CashBag test = new CashBag(chf, usd);
            testCashBag = test.ToString();
        }
        
        [Then(@"the result should be (.*)")]
        public void ThenTheResultShouldBe(string p0)
        {
            Assert.AreEqual(p0, testCashBag);
        }
    }
}
