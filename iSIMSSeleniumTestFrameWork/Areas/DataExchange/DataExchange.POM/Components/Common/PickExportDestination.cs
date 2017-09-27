using DataExchange.POM.Helper;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using SharedComponents.BaseFolder;
using WebDriverRunner.webdriver;

namespace DataExchange.POM.Components.Common
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
            WaitUntilEnabled(By.CssSelector(DataExchangeElements.CtfExport.SearchExportDestinationButton));
            AutomationSugarHelpers.WaitForAndClickOn(By.CssSelector(DataExchangeElements.CtfExport.SearchExportDestinationButton));
        }

        public void SelectRecordFromResults()
        {
            AutomationSugarHelpers.WaitForAndClickOn(DataExchangeElements.CtfExport.SearchRecordsToFindtext);
        }

        public void ClickOkButton()
        {
            IJavaScriptExecutor jsExecutor = (IJavaScriptExecutor)WebContext.WebDriver;
            string script = "$(\"[data-automation-id = '" + DataExchangeElements.CtfExport.PICK_EXPORT_DESTINATION_OK_BUTTON_AUTOMATION_ID + "']\").click(); ";
            ((IJavaScriptExecutor)WebContext.WebDriver).ExecuteScript(script);
        }

        public bool ClickSearchResultItemIfAny()
        {
            IJavaScriptExecutor jsExecutor = (IJavaScriptExecutor)WebContext.WebDriver;

            string script = "var searchResults = $(\"[data-section-id = '"+ DataExchangeElements.CtfExport.PICK_EXPORT_DESTINATION_SEARCH_RESULTS_CONTAINER_SECTION_ID+ "']\").find(\"[data-automation-id='"+ DataExchangeElements.CtfExport.PICK_EXPORT_DESTINATION_SEARCH_RESULT_ITEM_AUTOMATION_ID+ "']\"); ";
            script += " if(searchResults.length > 0){searchResults[0].click(); return true;} else { return false;} ";
            bool isAnyResultItemExist = (bool)jsExecutor.ExecuteScript(script);

            return isAnyResultItemExist;
        }
    }
}
