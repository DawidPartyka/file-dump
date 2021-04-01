using NUnit.Framework;
using NUnit.Samples.Cash;
using System;
using TechTalk.SpecFlow;

namespace SpecFlowProject.Features
{
    [Binding]
    public class CashClassUnitTestingSteps
    {
        Cash oldCash;
        Cash newCash;
        [Given(@"Cash CHF (.*)")]
        public void GivenCashCHF(int value)
        {
            oldCash = new Cash(value, "XXX");
        }

        [When(@"Multipy by (.*)")]
        public void WhenMultipyBy(int multiply)
        {
            newCash = (Cash)oldCash.Multiply(multiply);
        }

        [Then(@"Cash Value is (.*)")]
        public void ThenCashValueIs(int newValue)
        {
            Assert.AreEqual(newValue, newCash.Amount);
        }
    }
}
