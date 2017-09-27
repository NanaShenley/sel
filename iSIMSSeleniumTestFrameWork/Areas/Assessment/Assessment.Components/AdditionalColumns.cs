using System.Threading;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharedComponents.BaseFolder;
using SharedComponents.Helpers;
using SharedComponents.HomePages;
using TestSettings;
using WebDriverRunner.webdriver;
using Assessment.Components.PageObject;

namespace Assessment.Components
{
    public class AdditionalColumns : BaseSeleniumComponents
    {
        public readonly By ByPupil_details_button = By.CssSelector("[data-show-additional-columns-modal='']"); // By.CssSelector("[data-automation-id='pupil_details_button']");

        [FindsBy(How = How.CssSelector, Using = "[data-show-additional-columns-modal='']")]
        public IWebElement _pupil_details_button;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='marksheet_columns_button']")]
        public IWebElement _marksheetColumnsButton;

        [FindsBy(How = How.CssSelector, Using = "[view_id='fixedcolumntreenode']")]
        public IWebElement _additionalColumnsScreenRootElement;

        [FindsBy(How = How.CssSelector, Using = "[save-additional-columns='']")]
        public IWebElement _Save;

        public string additionalColumnNameSelector = "[webix_tm_id='{0}']";

        public string checkboxTag = "input";

        public AdditionalColumns()
        {
            PageFactory.InitElements(WebContext.WebDriver, this);
        }

        public AdditionalColumns ClickAdditionalColumns()
        {
            WaitForElement(ByPupil_details_button);
            _pupil_details_button.Click();
            return this;
        }

        public AdditionalColumns ClickMarksheeyColumns()
        {
            WaitForElement(ByPupil_details_button);
            _marksheetColumnsButton.Click();
            return this;
        }

        public bool SelectAdditonalColumnCheckBox(string additionalColumnName)
        {
            switch (additionalColumnName)
            {
                case "Personal Details": break;
                case "Asylum Status": additionalColumnName = "Learner.AsylumSeeker";
                    break;
                case "Looked After (LAC)": additionalColumnName = "Learner.ExtendedFields.LearnerInCareDetail";
                    break;
                case "Percentage Attendance": additionalColumnName = "Learner.ExtendedFields.PercentAttendance";
                    break;
                case "Term of Birth": additionalColumnName = "Learner.ExtendedFields.TermOfBirth";
                    break;
            }            
            IWebElement checkbox = _additionalColumnsScreenRootElement.FindElement(By.CssSelector(string.Format(additionalColumnNameSelector, additionalColumnName))).FindElement(By.TagName(checkboxTag));            
            bool isChecked = checkbox.GetAttribute("checked") == "true";
            Thread.Sleep(1000);
            checkbox.Click();
            return isChecked;
        }

        public bool SelectAdditonalColumnForFilter(string additionalColumnName)
        {
            additionalColumnName = "Learner.ExtendedFields.TermOfBirth";
            IWebElement checkbox = _additionalColumnsScreenRootElement.FindElement(By.CssSelector(string.Format(additionalColumnNameSelector, additionalColumnName))).FindElement(By.TagName(checkboxTag));
            bool isChecked = checkbox.GetAttribute("checked") == "true";
            Thread.Sleep(1000);
            var checkedValue = checkbox.GetAttribute("checked");
            //Assuming !NULL means checked.
            if (checkedValue == null)
                    checkbox.Click();                
            
            return isChecked;
        }

        public AdditionalColumns SelectClassAdditionalColumns()
        {
            WaitUntilDisplayed(
                By.CssSelector("[webix_tm_id='Learner.LearnerPrimaryClassMemberships.PrimaryClass.FullName']"));
            WebContext.WebDriver.FindElement(
                By.CssSelector("[webix_tm_id='Learner.LearnerPrimaryClassMemberships.PrimaryClass.FullName']"))
                .FindChild(By.CssSelector("[class='webix_tree_checkbox']"))
                .Click();
            return this;
        }

        public AdditionalColumns ClickOk()
        {
            _Save.Click();
            return this;
        }

        
    }
}
