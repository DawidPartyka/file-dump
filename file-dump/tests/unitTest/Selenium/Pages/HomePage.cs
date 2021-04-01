using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Text;
using TechTalk.SpecFlow;

namespace SpecFlowSeleniumTests.Pages
{
    class HomePage
    {
        public IWebDriver webdriver;
        public HomePage(IWebDriver driver)
        {
            webdriver = driver;
        }
        public IWebElement popup => webdriver.FindElement(By.XPath("/ html / body / div[2] / div / div[2] / div[3] / div / button[2]"));

        //Poniższe przykłady nie działały. W rezygnacji użyłem ścieżki CSS która z niewyjaśnionych przyczyn działa bez problemu w przeciwieństwie do XPath
        //public IWebElement mail => webdriver.FindElement(By.XPath("//*[@id=\"site - header\"]/div[2]/div[2]/a[1]"));
        //public IWebElement mail => webdriver.FindElement(By.XPath("/html/body/div[2]/div[5]/div[2]/div[3]/a[1]"));
        public IWebElement mail => webdriver.FindElement(By.CssSelector("#site-header > div.sc-15t10uw-1.eIQcOF > div.sc-15t10uw-3.iIzGiz > a:nth-child(1)"));
    }
}
