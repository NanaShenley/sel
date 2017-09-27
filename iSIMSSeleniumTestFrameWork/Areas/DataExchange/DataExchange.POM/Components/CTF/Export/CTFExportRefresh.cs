using System;
using DataExchange.POM.Components.Common;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using SharedComponents.CRUD;
using SharedComponents.Helpers;
using TestSettings;
using WebDriverRunner.webdriver;

namespace DataExchange.POM.Components.CTF.Export
{
    public class CtfExportRefresh : AutoRefreshSeleniumComponents
    {
        private const string CreateButtonToFind = "create_button";

        [FindsBy(How = How.CssSelector, Using = DataExchangeElements.CtfExport.SearchCtfDestinationButton)]
        private IWebElement _searchCtfDestinationButton;
        
        [FindsBy(How = How.CssSelector, Using = "form[id='editableData'] input[name='CtfPurpose.dropdownImitator']")]
        private IWebElement _ctfExportTypeTypeDropdown;

        private IWebElement CreateButton
        {
            get
            {
                WaitUntilEnabled(SeleniumHelper.SelectByDataAutomationID(CreateButtonToFind));
                return SeleniumHelper.Get(SeleniumHelper.SelectByDataAutomationID(CreateButtonToFind));
            }
        }

        public string CtfExportTypeTypeDropdown
        {
            get { return _ctfExportTypeTypeDropdown.GetAttribute("value"); }
            set { DataExchange.POM.Helper.SeleniumHelper.EnterForDropDown(_ctfExportTypeTypeDropdown, value); }
        }


        public CtfExportRefresh()
        {
            WebContext.WebDriver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(1000));
            PageFactory.InitElements(WebContext.WebDriver, this);
        }

        /// <summary>
        /// generates the ctf export file
        /// </summary>
        public void GenerateCtfExport()
        {
            DataExchange.POM.Helper.SeleniumHelper.ClickByJS(CreateButton);
            WaitTillDetailPanelIsLoaded();

            WaitUntilDisplayed(By.CssSelector(DataExchangeElements.CtfExport.SearchCtfDestinationButton));
            Helper.SeleniumHelper.ClickByJS(_searchCtfDestinationButton);

            WaitTillPickExportDestinationDialogIsShown();

            PickExportDestination exportDestination = new PickExportDestination();
            exportDestination.ClickSearchButton();
            DataExchange.POM.Helper.Wait.WaitTillAllAjaxCallsComplete();
            exportDestination.ClickSearchResultItemIfAny();
            DataExchange.POM.Helper.Wait.WaitTillAllAjaxCallsComplete();
            exportDestination.ClickOkButton();

            DataExchange.POM.Helper.Wait.WaitTillAllAjaxCallsComplete();

            WaitUntilValueGetsPopulated(DataExchangeElements.CtfExport.CtfDestinationSelector);
            WaitElementToBeClickable(DataExchangeElements.CtfExport.CtfExportTypeSelector);
            CtfExportTypeTypeDropdown = "Pupil Data Transfer";
        }

        public void WaitTillDetailPanelIsLoaded()
        {
            string jsPredicate = "return $(\"#"+ DataExchangeElements.CtfExport.DETAIL_PANEL_FORM_ID + "\").is(':visible')";
            Console.WriteLine("Waiting for Detail panel to load");
            DataExchange.POM.Helper.Wait.WaitTillConditionIsMet(jsPredicate, 60);
        }
        public void WaitTillPickExportDestinationDialogIsShown()
        {
            string jsPredicate = "return $(\"[data-automation-id='"+ DataExchangeElements.CtfExport.EXPORT_DESTINATION_DIALOG_TITLE_AUTOMATION_ID + "']\").is(':visible')";
            Console.WriteLine("Waiting for Export Destination dialog to load");
            DataExchange.POM.Helper.Wait.WaitTillConditionIsMet(jsPredicate, 60);
        }

        public void NavigateToOtherScreen()
        {
            string jsClick = "return $(\"[data-automation-id='" + DataExchangeElements.CtfExport.PUPIL_RECORD_QUICK_LINK_AUTOMATION_ID + "'] span\").click();";
            ((IJavaScriptExecutor)WebContext.WebDriver).ExecuteScript(jsClick);
        }

        /// <summary>
        /// selects the pupil
        /// </summary>
        public void Pupilselector()
        {
            DataExchange.POM.Helper.Wait.WaitForAndClick(BrowserDefaults.TimeOut, DataExchangeElements.CtfExport.AddPupilLink);
            WaitElementToBeClickable(DataExchangeElements.CtfExport.SearchPupilButton);
            DataExchange.POM.Helper.Wait.WaitForAndClick(BrowserDefaults.TimeOut, DataExchangeElements.CtfExport.SearchPupilButton);
            DataExchange.POM.Helper.Wait.WaitForAndClick(BrowserDefaults.TimeOut, DataExchangeElements.CtfExport.SearchRecordsToFindtext);
            DataExchange.POM.Helper.Wait.WaitForAndClick(BrowserDefaults.TimeOut, DataExchangeElements.CtfExport.AddButton);
            DataExchange.POM.Helper.Wait.WaitForAndClick(BrowserDefaults.TimeOut, DataExchangeElements.CtfExport.PupilSelectorOkButton);
            DataExchange.POM.Helper.Wait.WaitUntilDisplayed(DataExchangeElements.CommonElements.PupilGridCheckBox);
            DataExchange.POM.Helper.Wait.WaitForAndClick(BrowserDefaults.TimeOut, DataExchangeElements.CtfExport.GenerateButton);
        }
    }
}