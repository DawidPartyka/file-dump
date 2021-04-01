using NUnit.Framework;
using OpenQA.Selenium;
using SpecFlowSeleniumTests.Pages;
using System;
using TechTalk.SpecFlow;

namespace SpecFlowSeleniumTests.Features
{
    [Binding]
    public class UdemyTestSteps
    {
        private IWebDriver webdriver;
        private UdemyMainPage mainPage;
        private UdemyLoginPage loginPage;

        public UdemyTestSteps(IWebDriver driver)
        {
            webdriver = driver;

            loginPage = new UdemyLoginPage(webdriver);
            mainPage = new UdemyMainPage(webdriver);
        }

        [Given(@"I enter udemy\.com")]
        public void GivenIEnterUdemy_Com()
        {
            webdriver.Navigate().GoToUrl("https://www.udemy.com/");
        }
        
        [Given(@"I press (.*)")]
        public void GivenIPress(string p0)
        {
            mainPage.LoginButton.Click();
        }
        
        [When(@"I fill wrong email")]
        public void WhenIFillWrongEmail()
        {
            loginPage.email.SendKeys("TestEmail@test.com");
        }
        
        [When(@"I fill incorrect password")]
        public void WhenIFillIncorrectPassword()
        {
            loginPage.pass.SendKeys("TestPassword123");
        }
        
        [When(@"I submit the data")]
        public void WhenISubmitTheData()
        {
            loginPage.submit.Click();
        }
        
        [Then(@"I expect to see message informing about unsuccessful login attempt")]
        public void ThenIExpectToSeeMessageInformingAboutUnsuccessfulLoginAttempt()
        {
            string didItFail = loginPage.failMessage.Text;
            Assert.AreEqual(didItFail, "There was a problem logging in. Check your email and password or create an account.");
        }
    }
}
