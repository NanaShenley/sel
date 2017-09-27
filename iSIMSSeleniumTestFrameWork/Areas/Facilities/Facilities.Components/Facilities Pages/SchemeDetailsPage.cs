using System;
using Facilities.Components.Common;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using SharedComponents.Helpers;
using TestSettings;
using WebDriverRunner.webdriver;
using System.Threading;

namespace Facilities.Components.Facilities_Pages
{
    public class SchemeDetailsPage : BaseFacilitiesPage
    {
#pragma warning disable 0649
        [FindsBy(How = How.Id, Using = "editableData")]
        private readonly IWebElement _main;
        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='add_button']")]
        private readonly IWebElement _create;

        public IWebElement Schemename
        {
            get
            {
                return _main.FindElementSafe(By.Name("SchemeName"));
            }
        }

        public SchemeDetailsPage()
        {
            WaitUntilDisplayed(FacilitiesCommonElements.Createbutton);
            PageFactory.InitElements(WebContext.WebDriver, this);
        }

        public void CreateScheme()
        {
            _create.Click();
            WaitUntilDisplayed(FacilitiesCommonElements.Schemename);
        }

        public void EnterSchemeName(string schemename)
        {
            Schemename.SendKeys(schemename);
        }
    }
}
