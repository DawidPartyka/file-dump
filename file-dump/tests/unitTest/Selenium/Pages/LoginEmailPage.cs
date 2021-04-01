using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Text;
using TechTalk.SpecFlow;

namespace SpecFlowSeleniumTests
{
    [Binding]
    public class LoginEmailPage
    {
        public IWebDriver webdriver;
        public LoginEmailPage(IWebDriver driver)
        {
            webdriver = driver;
        }
        public IWebElement login => webdriver.FindElement(By.XPath("//*[@id=\"login\"]"));
        public IWebElement pass => webdriver.FindElement(By.XPath("//*[@id=\"password\"]"));
        public IWebElement submit => webdriver.FindElement(By.XPath("//*[@id=\"loginForm\"]/div[3]/button"));
        public IWebElement failMessage => webdriver.FindElement(By.XPath("//*[@id=\"formError\"]/span[1]"));

    }
}
