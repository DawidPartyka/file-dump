using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SpecFlowSeleniumTests.Pages;
using System;
using System.Threading;
using TechTalk.SpecFlow;

namespace SpecFlowSeleniumTests.Features
{
    [Binding]
    public class SpecFlowFeature1Steps
    {
        private IWebDriver webdriver;
        private LoginEmailPage loginPage;
        private HomePage homePage;
        
        public SpecFlowFeature1Steps(IWebDriver driver)
        {
            webdriver = driver;
            loginPage = new LoginEmailPage(webdriver);
            homePage = new HomePage(webdriver);
        }


        [Given(@"I enter wp\.pl")]
        public void GivenIEnterWp_Pl()
        {
            webdriver.Navigate().GoToUrl("http://www.wp.pl");
            homePage.popup.Click();
        }
        
        [Given(@"I click on (.*)")]
        public void GivenIClickOn(string p0)
        {
            homePage.mail.Click();
        }
        
        [When(@"I fill wrong email login")]
        public void WhenIFillWrongEmailLogin()
        {
            loginPage.login.SendKeys("Test");
        }

        [When(@"I fill wrong password")]
        public void WhenIFillWrongPassword()
        {
            loginPage.pass.SendKeys("pomidor");
        }

        [When(@"I press submit")]
        public void WhenIPressSubmit()
        {
            loginPage.submit.Click();
        }

        [Then(@"I expect to see message as „Niestety podany login lub hasło jest błędne\.”")]
        public void ThenIExpectToSeeMessageAsNiestetyPodanyLoginLubHasloJestBledne_()
        {
            string didItFail = loginPage.failMessage.Text;
            Assert.AreEqual(didItFail, "Podany login i/lub hasło są nieprawidłowe.");
        }
    }
}
