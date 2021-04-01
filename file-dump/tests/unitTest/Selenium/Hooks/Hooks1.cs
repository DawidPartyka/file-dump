using BoDi;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using TechTalk.SpecFlow;

namespace SpecFlowSeleniumTests
{
    [Binding]
    public sealed class Hooks1
    {
        // For additional details on SpecFlow hooks see http://go.specflow.org/doc-hooks
        private IWebDriver webdriver;
        private readonly IObjectContainer _objectContainer;
        public Hooks1(IObjectContainer objectContainer)
        {
            _objectContainer = objectContainer;
        }
        [BeforeScenario]
        public void BeforeScenario()
        {
            webdriver = DriverFactory.GetDriver(TypeDriver.Firefox);
            //webdriver = new ChromeDriver(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location));
            _objectContainer.RegisterInstanceAs<IWebDriver>(webdriver);
        }
        [AfterScenario]
        public void AfterScenario()
        {
            var errorClass = new SaveErrorDetails(webdriver);
            errorClass.SaveScreenshotAndLogsOnError();

            webdriver.Close();
            webdriver.Dispose();
        }
    }
}
