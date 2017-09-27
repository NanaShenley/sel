using Assessment.Components.Common;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using OpenQA.Selenium.Support.UI;
using SharedComponents.BaseFolder;
using System;
using System.Threading;
using WebDriverRunner.webdriver;

namespace Assessment.Components
{
    public class PupilDetails : BaseSeleniumComponents
    {
        [FindsBy(How = How.CssSelector, Using = "#pupilInfoTree > div.webix_view.webix_tree")]
        public IWebElement PupilDetailsColumnsRootElement;

        public string PupilDetailsColumnNameSelector = "[webix_tm_id='{0}']";

        public string CheckboxTag = "input";

        readonly WebDriverWait _waiter = new WebDriverWait(WebContext.WebDriver, TimeSpan.FromSeconds(MarksheetConstants.Timeout));

        public PupilDetails()
        {
            PageFactory.InitElements(WebContext.WebDriver, this);
        }

        public bool SelectPupilDetailColumnCheckBox(string additionalColumnName)
        {
            Thread.Sleep(5000);
            switch (additionalColumnName)
            {
                case "Personal Details": break;
                case "Age":
                    additionalColumnName = "Learner.Age";
                    break;
                case "Date Of Birth":
                    additionalColumnName = "Learner.DateOfBirth";
                    break;
                case "Gender":
                    additionalColumnName = "Learner.Gender.Description";
                    break;
                case "Class":
                    additionalColumnName = "Learner.LearnerPrimaryClassMemberships.PrimaryClass.FullName";
                    break;
                case "Asylum Status":
                    additionalColumnName = "Learner.AsylumSeeker";
                    break;
                case "Looked After (LAC)":
                    additionalColumnName = "Learner.ExtendedFields.LearnerInCareDetail";
                    break;
                case "Percentage Attendance":
                    additionalColumnName = "Learner.ExtendedFields.PercentAttendance";
                    break;
                case "Term of Birth":
                    additionalColumnName = "Learner.ExtendedFields.TermOfBirth";
                    break;
            }

            IWebElement chkbx = WebContext.WebDriver.FindElement(By.CssSelector(string.Format("div[id='pupilInfoTree'] div[webix_tm_id='{0}']>input", additionalColumnName)));
            bool isChecked = chkbx.GetAttribute("checked") == "true";
            _waiter.Until(ExpectedConditions.ElementToBeClickable(chkbx));
            Thread.Sleep(5000);
            chkbx.Click();
            return isChecked;
        }
    }
}
