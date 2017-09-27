using Attendance.Components.Common;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using POM.Helper;
using SharedComponents.BaseFolder;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using TestSettings;
using WebDriverRunner.webdriver;

namespace Attendance.Components.AttendancePages
{
    public class AdditionalCoulmnPage : BaseSeleniumComponents
    {
#pragma warning disable 0649
        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='select_columns_to_display_popup_header_title']")]
        public readonly IWebElement additionalColumHeader;
        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='section_menu_Additional']")]
        public readonly IWebElement additionalColumSection;
        [FindsBy(How = How.CssSelector, Using = "div[webix_tm_id='Personal Details'] .webix_tree_checkbox")]
        public readonly IWebElement personalDetailsSection;
        [FindsBy(How = How.CssSelector, Using = "div[webix_tm_id='Registration Details'] .webix_tree_checkbox")]
        public readonly IWebElement registerationDetailsSection;
        [FindsBy(How = How.CssSelector, Using = "div[webix_tm_id='Attendance'] .webix_tree_checkbox")]
        public readonly IWebElement attendanceSection;
        [FindsBy(How = How.CssSelector, Using = "[data-clear-container-id='editablecolumntreenode']")]
        public readonly IWebElement clearSelectionLink;
        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='ok_button']")]
        public readonly IWebElement AdditionalColumOKButton;
        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='cancel_button']")]
        public readonly IWebElement CancelButton;
        [FindsBy(How = How.CssSelector, Using = ".webix_ss_left .webix_cell.webix_cell_select")]
        public IWebElement _pupil;


        public string additionalColumnNameSelector = "[webix_tm_id='{0}']";
        public string checkboxTag = "input";

        public AdditionalCoulmnPage()
        {
            PageFactory.InitElements(WebContext.WebDriver, this);
            WaitForElement(By.CssSelector("[data-automation-id='additional_columns_button']"));
        }


        public int GetDialogAdditionalColumnCount()
        {
            ReadOnlyCollection<IWebElement> checkBoxList = WebContext.WebDriver.FindElements(By.CssSelector(".webix_tree_checkbox"));
            WebContext.Screenshot();
            return checkBoxList.Count;
        }

        public void ClickParentCheckboxes()
        {
            WaitUntilDisplayed(EditMarksElements.AdditionalColumn.PersonalDetails);
            personalDetailsSection.ClickByAction();
            //registerationDetailsSection.ClickByAction();
            //attendanceSection.Click();
        }

        public void ClickPersonalDetailsCheckbox()
        {
            WaitUntilDisplayed(EditMarksElements.AdditionalColumn.PersonalDetails);
            personalDetailsSection.Click();
        }

        public AttendanceDetails ClickOkButton()
        {
            var additionalColumOKButton = WebContext.WebDriver.FindElements(By.CssSelector("[data-automation-id='ok_button']"));
            additionalColumOKButton[1].Click();
            Wait.WaitForAjaxReady(By.CssSelector(".locking-mask"));
            Thread.Sleep(1000);
            return new AttendanceDetails();
        }

        public AdditionalCoulmnPage CancelDialog()
        {
            WaitForElement(By.CssSelector("[data-automation-id='cancel_button']"));
            IWebElement columnsDialogElement =
                WebContext.WebDriver.FindElements(By.CssSelector("[data-automation-id='cancel_button']"))
                    .FirstOrDefault(ele => ele.Displayed);
            columnsDialogElement.Click();
            return this;
        }

        public void ClearAdditionalColumnSelection()
        {
            //WaitUntilEnabled(EditMarksElements.AdditionalColumn.ClearSelection);
            WaitForAndClick(BrowserDefaults.TimeOut, EditMarksElements.AdditionalColumn.ClearSelection);
        }
   

        public bool isAdditionalColumnToolBarIcon()
        {
            //WaitUntilDisplayed(By.CssSelector("[data-automation-id='additional_columns_button']"));
            WaitForElement(By.CssSelector("[data-automation-id='additional_columns_button']"));
            IWebElement additionalColumnToolbarIcon =
                WebContext.WebDriver.FindElement(By.CssSelector("[data-automation-id='additional_columns_button']"));

            return additionalColumnToolbarIcon != null;
        }


        public bool isAdditionalColumnDialogOpen()
        {
            //WaitUntilDisplayed(By.CssSelector("[data-automation-id='additional_pupil_columns_popup_header_title']"));
            WaitForElement(By.CssSelector("[data-automation-id='additional_pupil_columns_popup_header_title']"));

            IWebElement additionalColumnDialog =
              WebContext.WebDriver.FindElement(By.CssSelector("[data-automation-id='additional_pupil_columns_popup_header_title']"));

            return additionalColumnDialog != null;
        }

    }
}
