using System.Collections.Generic;
using System.Linq;

using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using OpenQA.Selenium.Support.UI;
using POM.Helper;
using SharedComponents.BaseFolder;
using TestSettings;
using WebDriverRunner.webdriver;
using SeleniumHelper = SharedComponents.Helpers.SeleniumHelper;

namespace Pupil.BulkUpdate.Tests.ApplicationStatusComponents
{
    public class ApplicationStatusDetail : BaseSeleniumComponents
    {
        public static string ApplicationStatusDefaultColumnNumber = "5"; // ApplicationStatus will be column 5 (providing no columns have been added or removed)
        public static string ApplicantNamesDefaultColumnNumber = "1";

        private static readonly By IdentifierButton = By.CssSelector("a[data-invoke-dialog='pupil-bulkUpdate-identifiers-dialog']");

        public static readonly By FiltersButton = By.XPath("//div[@data-detail-section-header]//span[text()='Filters']//..");

        public static By ApplicationStatus = By.CssSelector("div[class='webix_ss_header'] td[column='" + ApplicationStatusDefaultColumnNumber + "'] span");
        public static By ApplicantName = By.CssSelector("div[class='webix_ss_header'] td[column='" + ApplicantNamesDefaultColumnNumber + "'] span");

        public static By ApplicationStatusFloodFill = By.CssSelector("div[class='webix_ss_header'] td[column='" + ApplicationStatusDefaultColumnNumber + "'] [class='fa fa-angle-down fa-fw']");

        public static readonly By ApplicationStatusFloodFillCheckbox = By.CssSelector("div[data-menu-column-id='_CurrentApplicationStatus'] input[type='checkbox']");
        public static readonly By ApplicationStatusFloodFillApplyToSelected = By.CssSelector("div[data-menu-column-id='_CurrentApplicationStatus'] [data-automation-id='control_button_bulkupdate_applicant_applicationstatus_floodfill_applyselection']");

        public static readonly By DeleteFloodFillValues = By.CssSelector("div[data-menu-column-id='_CurrentApplicationStatus'] [data-bulkupdate-delete-floodfill-values]");
        public static readonly By DeleteFloodFillConfirmationDailog = By.CssSelector("div[data-section-id='custom-confirm-dialog']");
        public static readonly By DeleteFloodFillConfirmationDailogButtonOK = By.CssSelector("div[data-section-id='custom-confirm-dialog'] button[data-automation-id='ok_button']");

        public const string CellsDivSelector = "[class='webix_ss_center']";

        public const string EditorSelector = "//div[contains(@class,'webix_dt_editor')]/input[@type='text']";
        public static readonly By ApplicationStatusSelector = By.CssSelector("div[class='webix_dt_editor pretend-dropdown']");
        public static readonly By SaveButton = By.CssSelector("a[data-automation-id='well_know_action_save']");

        public IWebElement IdentifierMenuButton = SeleniumHelper.Get(IdentifierButton);

        public ApplicationStatusDetail()
        {
            PageFactory.InitElements(WebContext.WebDriver, this);
        }

        public ApplicationStatusIdentifierDialog ClickOnIdentifierDialogButton()
        {
            SeleniumHelper.WaitForElementClickableThenClick(IdentifierButton);
            return new ApplicationStatusIdentifierDialog();
        }

        public ApplicationStatusConfirmDialogOnSave ClickOnSave()
        {
            SeleniumHelper.WaitForElementClickableThenClick(SaveButton);
            return new ApplicationStatusConfirmDialogOnSave();
        }

        private void ClickOnDeleteFloodFillLink()
        {
            SeleniumHelper.WaitForElementClickableThenClick(DeleteFloodFillValues);
            SeleniumHelper.Get(DeleteFloodFillConfirmationDailog);
            SeleniumHelper.WaitForElementClickableThenClick(DeleteFloodFillConfirmationDailogButtonOK);
        }

        public static List<IWebElement> FindcellsForColumn(string columnNo)
        {
            var wait = new WebDriverWait(WebContext.WebDriver, BrowserDefaults.TimeOut);
            WaitLogic(wait, By.CssSelector(CellsDivSelector));
            IWebElement cellDiv = WebContext.WebDriver.FindElement(By.CssSelector(CellsDivSelector));
            IWebElement parnetDiv = cellDiv.FindChild(By.ClassName("webix_ss_center_scroll"));

            return
                parnetDiv.FindElement(By.CssSelector("[column='" + columnNo + "']"))
                    .FindElements(By.ClassName("webix_cell"))
                    .ToList();
        }

        public void SetApplicationStatusDropDown(string status)
        {
            var wait = new WebDriverWait(WebContext.WebDriver, BrowserDefaults.TimeOut);
            WaitLogic(wait, By.CssSelector("[name='CurrentApplicationStatus']"));
            var selectElement = WebContext.WebDriver.FindElement(By.CssSelector("[name='CurrentApplicationStatus']"));
            var mySelect = new SelectElement(selectElement);
            mySelect.SelectByText(status);
        }

        public void SetCellApplicationStatusDropDown(string status)
        {
            var wait = new WebDriverWait(WebContext.WebDriver, BrowserDefaults.TimeOut);
            WebContext.WebDriver.FindElement(By.CssSelector("div[class='webix_dt_editor']")).Click();
            var autocompleteListSelector = By.XPath("//div[@class='webix_dt_editor']/ul");            
            WaitLogic(wait, autocompleteListSelector);
            var autocompleteElement = WebContext.WebDriver.FindElement(autocompleteListSelector);
            var elements = autocompleteElement.FindElements(By.CssSelector("li"));
            foreach (IWebElement e in elements)
            {
                if (e.GetAttribute("data-value") == status)
                {
                    e.Click();
                    break;
                }
            }
        }

        public static void ClickFirstCellofColumn(string columnposition)
        {
            WaitForAndClick(BrowserDefaults.TimeOut, By.XPath("//div[@column='" + columnposition + "']/div"));
        }

        public static void ScrollToApplicationStatus()
        {
            var applicationStatusHeader = WebContext.WebDriver.FindElement(ApplicationStatus);
            SeleniumHelper.ScrollToByAction(applicationStatusHeader);
            WaitUntillAjaxRequestCompleted();
        }

        public static void ScrollToApplicantName()
        {
            var applicationStatusHeader = WebContext.WebDriver.FindElement(ApplicantName);
            SeleniumHelper.ScrollToByAction(applicationStatusHeader);
            WaitUntillAjaxRequestCompleted();
        }

        public static IWebElement GetEditor()
        {
            return WebContext.WebDriver.FindElement(By.XPath(EditorSelector));
        }

        public void FloodFillApplicationStatusColumnWith(string value)
        {
            ExecuteJavaScriptToBulkSelectApplicationStatus();

            SetApplicationStatusDropDown(value);

            IWebElement checkBox = SeleniumHelper.Get(ApplicationStatusFloodFillCheckbox);
            var checkedValue = checkBox.GetAttribute("checked");

            if (checkedValue == null)
                checkBox.Click();

            checkedValue = checkBox.GetAttribute("checked");

            IWebElement applyToSelected =
                SeleniumHelper.Get(ApplicationStatusFloodFillApplyToSelected);
            applyToSelected.Click();
        }

        private void ExecuteJavaScriptToBulkSelectApplicationStatus()
        {
            var applicationStatusHeader = WebContext.WebDriver.FindElement(ApplicationStatus);
            SeleniumHelper.ScrollToByAction(applicationStatusHeader);
            WaitUntillAjaxRequestCompleted();
            var js = (IJavaScriptExecutor)WebContext.WebDriver;

            string script = "$('div[class=\"webix_ss_header\"] td[column=\"5\"] span:contains(\"Application Status\")').click();" +
                            "$('div[class=\"webix_ss_header\"] td[column=\"5\"] span:contains(\"Application Status\")').closest('div').find('i.high-volume-grid-spreadsheet-menu')[0].click();";
            js.ExecuteScript(script);
        }

        public void DeleteApplicationStatusColumnValues()
        {
            ExecuteJavaScriptToBulkSelectApplicationStatus();
            ClickOnDeleteFloodFillLink();
        }

        public struct Detail
        {
            public struct GridColumns
            {
                public static By DateOfBirth = By.CssSelector("div[data-menu-column-id='_DateOfBirth']");
                public static By Gender = By.CssSelector("div[data-menu-column-id='_Gender']");
                public static By AdmissionGroup = By.CssSelector("div[data-menu-column-id='_AdmissionGroup']");
                public static By ApplicantApplicationStatus = By.CssSelector("div[data-menu-column-id='_ApplicantApplicationStatus']");
                public static By CurrentApplicationStatus = By.CssSelector("div[data-menu-column-id='_CurrentApplicationStatus']");
                public static By DateOfAmission = By.CssSelector("div[data-menu-column-id='_DOA']");
                public static By SchoolIntake = By.CssSelector("div[data-menu-column-id='_SchoolIntake']");
                public static By YearGroup = By.CssSelector("div[data-menu-column-id='_YearGroup']");
                public static By Class = By.CssSelector("div[data-menu-column-id='_PrimaryClass']");
                public static By ApplicantName = By.CssSelector("div[data-menu-column-id='_ApplicantName']");
            }
        }
    }
}