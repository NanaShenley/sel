using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using SeSugar.Automation;
using SharedComponents.BaseFolder;
using SharedComponents.Helpers;
using SharedServices.Components.Common;
using WebDriverRunner.webdriver;

namespace SharedServices.Components.PageObjects
{
    public class PickExportDestination : BaseSeleniumComponents
    {
        private const string OkButton = "ok_button";

        public PickExportDestination()
        {
            PageFactory.InitElements(WebContext.WebDriver, this);
        }

        public void ClickSearchButton()
        {
            WaitUntilEnabled(By.CssSelector(SharedServicesElements.CtfExport.SearchExportDestinationButton));
            AutomationSugarHelpers.WaitForAndClickOn(By.CssSelector(SharedServicesElements.CtfExport.SearchExportDestinationButton));
        }

        public void SelectRecordFromResults()
        {
            AutomationSugarHelpers.WaitForAndClickOn(SharedServicesElements.CtfExport.SearchRecordsToFindtext);
        }

        public void ClickOkButton()
        {
            AutomationSugarHelpers.WaitForAndClickOn(OkButton);
        }
    }
}
