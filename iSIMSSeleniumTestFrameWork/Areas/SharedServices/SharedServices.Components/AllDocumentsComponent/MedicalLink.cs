using System;
using OpenQA.Selenium;
using SharedComponents.BaseFolder;
using SharedComponents.Helpers;
using SharedServices.Components.Common;
using WebDriverRunner.webdriver;

namespace SharedServices.Components.AllDocumentsComponent
{
    public class MedicalLink: BaseSeleniumComponents
    {
        public IWebElement Medical
        {
            get { return WebContext.WebDriver.FindElement(SeleniumHelper.SelectByDataAutomationID("section_menu_Medical")); }
        }

        public void ClickMedicalLink()
        {
            WaitForAndClick(new TimeSpan(0,0,0,5), SharedServicesElements.CommonElements.DataMaintenance );
            Medical.Click();
        }
    }
}
