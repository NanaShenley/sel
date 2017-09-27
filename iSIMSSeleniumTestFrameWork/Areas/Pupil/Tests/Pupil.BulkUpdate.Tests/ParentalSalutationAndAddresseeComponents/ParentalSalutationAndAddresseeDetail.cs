using System.Collections.Generic;
using System.Linq;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using OpenQA.Selenium.Support.UI;
using SharedComponents.BaseFolder;
using SharedComponents.Helpers;
using TestSettings;
using WebDriverRunner.webdriver;
using System;
using System.Threading;

namespace Pupil.BulkUpdate.Tests.ParentalSalutationAndAddresseeComponents
{
    public class ParentalSalutationAndAddresseeDetail : BaseSeleniumComponents
    {
        #region NavigationByCss

        public static By ParentalSalutation = By.CssSelector("div[class='webix_ss_header'] td[column='2'] span");
        public static By FloodFillSelectAll = By.CssSelector("div[class='webix_ss_header'] td[column='2'] span[class='header-text']");
        public static By FloodFillAddresseeSelectAll = By.CssSelector("div[class='webix_ss_header'] td[column='3'] span[class='header-text']");
        public static By FloodFillPopup = By.CssSelector("div[class='webix_ss_header'] td[column='2'] [class='fa fa-caret-down fa-fw high-volume-grid-spreadsheet-menu']");
        public static By FloodFillAddresseePopup = By.CssSelector("div[class='webix_ss_header'] td[column='3'] [class='fa fa-caret-down fa-fw high-volume-grid-spreadsheet-menu']");
        public static By FloodFillCheckbox = By.CssSelector("div[data-menu-column-id='_ParentalSalutation'] input[type='checkbox']");
        public static By FloodFillGenerateForSelected = By.CssSelector("div[data-menu-column-id='_ParentalSalutation'] span.btn");
        public static By PupilFloodFillSalutationDeleteLink = By.CssSelector("[data-menu-column-id='_ParentalSalutation'] [data-automation-id='link_bulkupdate_pupil_parental_salutation_and_addressee_floodfill_delete']");
        public static By PupilFloodFillAddresseeDeleteLink = By.CssSelector("[data-menu-column-id='_ParentalAddressee'] [data-automation-id='link_bulkupdate_pupil_parental_salutation_and_addressee_floodfill_delete']");
        public static By FloodFillConfirmDelete = By.CssSelector("button[data-bulkupdate-delete-floodfill]");

        public static By ParentalAddressee = By.CssSelector("div[class='webix_ss_header'] td[column='3'] span");
        public static By ParentalAddresseeFloodFillPopup = By.CssSelector("div[class='webix_ss_header'] td[column='3'] [class='fa fa-caret-down fa-fw high-volume-grid-spreadsheet-menu']");
        public static By ParentalAddresseeFloodFillCheckbox = By.CssSelector("div[data-menu-column-id='_ParentalAddressee'] input[type='checkbox']");
        public static By ParentalAddresseeFloodFillGenerateForSelected = By.CssSelector("div[data-menu-column-id='_ParentalAddressee'] span.btn");
        public static readonly By DeleteFloodFillConfirmationDailog = By.CssSelector("div[data-section-id='custom-confirm-dialog']");

        public static By YearGroupDropDownList = By.CssSelector("input[name='YearGroup.dropdownImitator']");
        public static By SchoolIntakeDropDownList = By.CssSelector("input[name='IntakeGroup.dropdownImitator']");
        public static By AdmissionGroupDropDownList = By.CssSelector("input[name='AdmissionGroup.dropdownImitator']");

        public static By FirstSalutationName = By.CssSelector("div[class='webix_ss_body'] div[class='webix_ss_center'] div:first-child div:nth-child(2) div:first-child");
        public static By IsDirtyIndicator = By.CssSelector("span[class='has-changed-indicator'] [class='fa fa-fw fa-circle isdirty-indicator']");
        public const string EditorSelector = "//div[contains(@class,'webix_dt_editor')]/input[@type='text']";
        public const string CellsDivSelector = "[class='webix_ss_center']";

        #endregion NavigationByCss

        public ParentalSalutationAndAddresseeDetail()
        {
            PageFactory.InitElements(WebContext.WebDriver, this);
        }

        private static void CommonDelete(Action headerClick, By deleteLink)
        {
            headerClick.Invoke();

            SeleniumHelper.WaitForElementClickableThenClick(deleteLink);
            SeleniumHelper.Get(DeleteFloodFillConfirmationDailog);

            WebDriverWait wait = new WebDriverWait(WebContext.WebDriver, BrowserDefaults.TimeOut);

            wait.Until(ExpectedConditions.ElementIsVisible(FloodFillConfirmDelete));

            WebContext.WebDriver.FindElementSafe(FloodFillConfirmDelete).Click();

            WaitUntillAjaxRequestCompleted();
        }

        public static void DeletePupilParentalSalutationColumnValues()
        {
            CommonDelete(ExecuteJavaScriptToBulkSelectParentalSalutation, PupilFloodFillSalutationDeleteLink);
        }

        public static void DeletePupilParentalAddresseeColumnValues()
        {
            CommonDelete(ExecuteJavaScriptToBulkSelectParentalAddressee, PupilFloodFillAddresseeDeleteLink);
        }

        public static void ExecuteJavaScriptToBulkSelectParentalAddressee()
        {
            var parentalAddressHeader = WebContext.WebDriver.FindElement(ParentalAddressee);
            parentalAddressHeader.ScrollToByAction();
            var js = (IJavaScriptExecutor)WebContext.WebDriver;
            WaitUntillAjaxRequestCompleted();
            string script = "$('div[class=\"webix_ss_header\"] td[column=\"3\"] span:contains(\"Parental Addressee\")').click();" +
                                 "$('div[class=\"webix_ss_header\"] td[column=\"3\"] span:contains(\"Parental Addressee\")').closest('div').find('i.high-volume-grid-spreadsheet-menu')[0].click();";
            js.ExecuteScript(script);
            WaitUntillAjaxRequestCompleted();
        }

        public static void ExecuteJavaScriptToBulkParentalAddresseeFloodFillMenuClick()
        {
            var parentalAddressHeader = WebContext.WebDriver.FindElement(FloodFillAddresseePopup);
            parentalAddressHeader.ScrollToByAction();
            var js = (IJavaScriptExecutor)WebContext.WebDriver;
            WaitUntillAjaxRequestCompleted();
            string script = "$('div[class=\"webix_ss_header\"] td[column=\"3\"] span:contains(\"Parental Addressee\")').closest('div').find('i.high-volume-grid-spreadsheet-menu')[0].click();";
            js.ExecuteScript(script);
            WaitUntillAjaxRequestCompleted();
        }

        public static void ExecuteJavaScriptToBulkParentalSalutationFloodFillMenuClick()
        {
            var parentalSalutationHeader = WebContext.WebDriver.FindElement(FloodFillPopup);
            parentalSalutationHeader.ScrollToByAction();
            var js = (IJavaScriptExecutor)WebContext.WebDriver;
            WaitUntillAjaxRequestCompleted();
            string script = "$('div[class=\"webix_ss_header\"] td[column=\"2\"] span:contains(\"Parental Salutation\")').closest('div').find('i.high-volume-grid-spreadsheet-menu')[0].click();";
            js.ExecuteScript(script);

            WaitUntillAjaxRequestCompleted();
        }

        public static void ExecuteJavaScriptToBulkSelectParentalSalutation()
        {
            var parentalSalutationHeader = WebContext.WebDriver.FindElement(ParentalSalutation);
            parentalSalutationHeader.ScrollToByAction();
            var js = (IJavaScriptExecutor)WebContext.WebDriver;
            WaitUntillAjaxRequestCompleted();
            string script = "$('div[class=\"webix_ss_header\"] td[column=\"2\"] span:contains(\"Parental Salutation\")').click();" +
                                 "$('div[class=\"webix_ss_header\"] td[column=\"2\"] span:contains(\"Parental Salutation\")').closest('div').find('i.high-volume-grid-spreadsheet-menu')[0].click();";
            js.ExecuteScript(script);

            WaitUntillAjaxRequestCompleted();
        }

        public static List<string> GetCellText(string columnNo)
        {
            var cellsXpath = By.XPath("//*/div[@class='webix_ss_center']/div[@class='webix_ss_center_scroll']/div[@column='" + columnNo + "']/div[contains(@class, 'webix_cell')]");

            var wait = new WebDriverWait(WebContext.WebDriver, BrowserDefaults.TimeOut);

            wait.Until(ExpectedConditions.ElementExists(cellsXpath));

            WaitUntillAjaxRequestCompleted();

            Thread.Sleep(TimeSpan.FromSeconds(2)); //below line is failing inconsistently hence added thread of sleep.

            return (from cell in WebContext.WebDriver.FindElements(cellsXpath)
                         select cell.Text).ToList();
        }

        public static void ClickFirstCellofColumn(string columnposition)
        {
            WaitUntillAjaxRequestCompleted();

            var firstCellCssSelector = "div.webix_ss_body > div.webix_ss_center > div > div.webix_column[column = \"" + columnposition + "\"] > div:nth-child(1)";

            var wait = new WebDriverWait(WebContext.WebDriver, BrowserDefaults.TimeOut);

            wait.Until(ExpectedConditions.ElementExists(By.CssSelector(firstCellCssSelector)));

            var script = "try { " +
                    "var cell = document.querySelector('" + firstCellCssSelector + "'); " +
                    "cell.click(); " +
                "} catch(ex) { " +
                    "console.log('ClickFirstCellofColumn: failed with - ' + ex); " +
                "} ";

            ((IJavaScriptExecutor)WebContext.WebDriver).ExecuteScript(script);

            WaitUntillAjaxRequestCompleted();
        }

        public static IWebElement GetEditor()
        {
            return WebContext.WebDriver.FindElement(By.XPath(EditorSelector));
        }

        public static void FloodFillSalutationColumnWithOverride()
        {
            Thread.Sleep(1000);
            IWebElement checkBox = SeleniumHelper.Get(FloodFillCheckbox);
            var checkedValue = checkBox.GetAttribute("checked");

            if (checkedValue == null) checkBox.Click();
            IWebElement applyToSelected = SeleniumHelper.Get(FloodFillGenerateForSelected);
            applyToSelected.Click();
            WaitUntillAjaxRequestCompleted();
        }

        public static void FloodFillAddresseeColumnWithOverride()
        {
            Thread.Sleep(1000);
            IWebElement checkBox = SeleniumHelper.Get(ParentalAddresseeFloodFillCheckbox);
            var checkedValue = checkBox.GetAttribute("checked");

            if (checkedValue == null) checkBox.Click();
            IWebElement applyToSelected = SeleniumHelper.Get(ParentalAddresseeFloodFillGenerateForSelected);
            applyToSelected.Click();
            WaitUntillAjaxRequestCompleted();
        }

        public struct Detail
        {
            public struct GridColumns
            {
                public static By DateOfBirth = By.CssSelector("div[data-menu-column-id='_DateOfBirth']");
                public static By Gender = By.CssSelector("div[data-menu-column-id='_Gender']");
                public static By LegalName = By.CssSelector("div[data-menu-column-id='_PreferredName']");
                public static By AdmissionNumber = By.CssSelector("div[data-menu-column-id='_AdmissionNumber']");
                public static By Class = By.CssSelector("div[data-menu-column-id='_PrimaryClass']");
                public static By DateOfAmission = By.CssSelector("div[data-menu-column-id='_DOA']");
                public static By YearGroup = By.CssSelector("div[data-menu-column-id='_YearGroup']");
                public static By ParentalSalutation = By.CssSelector("div[data-menu-column-id='_ParentalSalutation']");
                public static By ParentalAddressee = By.CssSelector("div[data-menu-column-id='_ParentalAddressee']");
            }
        }
    }
}