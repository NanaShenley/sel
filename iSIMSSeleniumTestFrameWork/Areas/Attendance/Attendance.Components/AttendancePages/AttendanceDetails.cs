using System.Collections.ObjectModel;
using System.Linq;
using OpenQA.Selenium;
using SharedComponents.BaseFolder;
using WebDriverRunner.webdriver;
using Attendance.Components.Common;
using TestSettings;
using OpenQA.Selenium.Support.PageObjects;
using OpenQA.Selenium.Support.UI;
using System;
using POM.Helper;

namespace Attendance.Components.AttendancePages
{
    public class AttendanceDetails: BaseSeleniumComponents
    {
#pragma warning disable 0649
        [FindsBy(How = How.Id, Using = "RegisterSave")]
        public readonly IWebElement registerSaveButton;        
        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='Button_Dropdown']")]
        public readonly IWebElement allCodesDropdownButton;
        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='Extended_Dropdown']")]
        public readonly IWebElement PreserveOverwriteButton;
        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='Keep']")]
        public readonly IWebElement preserveMode;
        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='Replace']")]
        public readonly IWebElement overwriteMode;
        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='additional_columns_button']")]
        public readonly IWebElement additionalColumnButton;
        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='_button']")]
        public readonly IWebElement filterButton;
        [FindsBy(How = How.CssSelector, Using = ".dropdown-toggle.data-label-hidden[data-automation-id='Extended_Dropdown']")]
        public readonly IWebElement OrientationButton;
        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='Vertical']")]
        public readonly IWebElement verticalOrientaion;
        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='Horizontal']")]
        public readonly IWebElement horizontalOrientaion;
        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='summary_button']")]
        public readonly IWebElement summaryLink;
        [FindsBy(How = How.Id, Using = "editmarksgrid")]
        public readonly IWebElement editMarksGrid;
        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='header_menu_Pupil']")]
        public readonly IWebElement pupilMenuHeader;
        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='header_menu_AM']")]
        public readonly IWebElement AMmenuHeader;
        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='header_menu_PM']")]
        public readonly IWebElement PMmenuHeader;
        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='header_menu_Meal']")]
        public readonly IWebElement MealmenuHeader;
        [FindsBy(How = How.CssSelector, Using = ".webix_ss_body .webix_ss_left .btn-text-afforded")]
        public IWebElement pupilLink;
        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='view_pupil_log_button']")]
        public IWebElement viewPupilLog;

        public AttendanceDetails()
        {
            PageFactory.InitElements(WebContext.WebDriver, this);
        }

        public IWebElement AttendanceRow(string name)
        {
            ReadOnlyCollection<IWebElement> attendanceRows = WebContext.WebDriver.FindElements(By.CssSelector("span[class='btn-text-afforded']"));
            return attendanceRows.First(attendanceRow => attendanceRow.Text == name);
        }

        public void ClickPreserveOverwriteButton()
        {
            WaitUntilEnabled(EditMarksElements.Toolbar.AdditionalColumnButton);
            PreserveOverwriteButton.Click();
        }

        public void SelectPreserveMode()
        {
            WaitUntilEnabled(EditMarksElements.Toolbar.AdditionalColumnButton);
            preserveMode.Click();
        }

        public void SelectOverwriteMode()
        {
            WaitUntilEnabled(EditMarksElements.Toolbar.AdditionalColumnButton);
            overwriteMode.Click();
        }

        public AdditionalCoulmnPage ClickAdditionalColumn()
        {
            WaitUntilEnabled(EditMarksElements.Toolbar.AdditionalColumnButton);
            SeleniumHelper.ClickByAction(additionalColumnButton);
            return new AdditionalCoulmnPage();
        }

        public void ClickFilterButton()
        {
            WaitUntilEnabled(EditMarksElements.Toolbar.AdditionalColumnButton);
            filterButton.Click();
        }

        public void ClickOrientationButton()
        {
            WaitUntilEnabled(EditMarksElements.Toolbar.AdditionalColumnButton);
            OrientationButton.Click();
        }

        public void SelectVerticalMode()
        {
            WaitUntilEnabled(EditMarksElements.Toolbar.AdditionalColumnButton);
            verticalOrientaion.Click();
        }

        public void SelectHorizontalMode()
        {
            WaitUntilEnabled(EditMarksElements.Toolbar.AdditionalColumnButton);
            horizontalOrientaion.Click();
        }

        public bool IsSummarySectionDisplayed()
        {
            WaitForAndClick(BrowserDefaults.TimeOut, EditMarksElements.Toolbar.SummaryLink);
            bool summarySection = WebContext.WebDriver.FindElement(By.CssSelector(".webix_ss_footer")).Displayed;
            return summarySection;
        }

        public ReadOnlyCollection<IWebElement> SummarySection()
        {
            WaitForAndClick(BrowserDefaults.TimeOut, EditMarksElements.Toolbar.SummaryLink);
            return WebContext.WebDriver.FindElements(By.CssSelector(".webix_ss_footer .webix_first.webix_last"));
        }

        public EditMarksPupilDetail ClickPupilLink()
        {
            Wait.WaitForDocumentReady();
            SeleniumHelper.ClickByAction(pupilLink);
            WaitUntilDisplayed(EditMarksElements.AttendancePLog.viewPlogNote);
            return new EditMarksPupilDetail();
        }

        public void ClickMealColumn()
        {
            MealmenuHeader.Click();
        }
    }
} 