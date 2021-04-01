using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Text;
using TechTalk.SpecFlow;

namespace SpecFlowSeleniumTests.Pages
{
    [Binding]
    public class UdemyLoginPage
    {
        public IWebDriver webdriver;
        public UdemyLoginPage(IWebDriver driver)
        {
            webdriver = driver;
        }
        public IWebElement email => webdriver.FindElement(By.XPath("/html/body/div[1]/div[2]/div[1]/div[3]/form/div[1]/div[1]/div/input"));
        public IWebElement pass => webdriver.FindElement(By.XPath("/html/body/div[1]/div[2]/div[1]/div[3]/form/div[1]/div[2]/div/input"));
        public IWebElement submit => webdriver.FindElement(By.XPath("/html/body/div[1]/div[2]/div[1]/div[3]/form/div[2]/div/input"));
        public IWebElement failMessage => webdriver.FindElement(By.XPath("/html/body/div[1]/div[2]/div[1]/div[3]/form/div[1]/div/div"));

    }
}
