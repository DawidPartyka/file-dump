using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Text;
using TechTalk.SpecFlow;

namespace SpecFlowSeleniumTests.Pages
{
    class UdemyMainPage
    {
        public IWebDriver webdriver;
        public UdemyMainPage(IWebDriver driver)
        {
            webdriver = driver;
        }
        public IWebElement LoginButton => webdriver.FindElement(By.CssSelector("#udemy > div.main-content-wrapper > div.ud-app-loader.ud-component--header-v6--header.udlite-header.ud-app-loaded > div.udlite-text-sm.header--header--3sK1h.header--flex-middle--2Xqjv > div:nth-child(8) > a"));
    }
}
