using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
using Assessment.Components.Common;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using OpenQA.Selenium.Support.UI;
using WebDriverRunner.webdriver;

namespace Assessment.Components.PageObject
{
    public class EditGradesetVersion
    {
        public EditGradesetVersion()
        {
            WebContext.WebDriver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(MarksheetConstants.Timeout));
            PageFactory.InitElements(WebContext.WebDriver, this);
        }

        WebDriverWait waiter = new WebDriverWait(WebContext.WebDriver, TimeSpan.FromSeconds(MarksheetConstants.Timeout));

        [FindsBy(How = How.CssSelector, Using = "button[data-automation-id='add_grade_button']")]
        private IWebElement addGradeButton;

        [FindsBy(How = How.CssSelector, Using = "tr[data-row-name='AssessmentGrades'] input[name*='.Code']")]
        private IWebElement gradesetCode;

        [FindsBy(How = How.CssSelector, Using = "tr[data-row-name='AssessmentGrades'] input[name*='.Description']")]
        private IWebElement gradesetDescription;

        [FindsBy(How = How.CssSelector, Using = "tr[data-row-name='AssessmentGrades'] input[name*='.Value']")]
        private IWebElement gradesetValue;

        [FindsBy(How = How.CssSelector, Using = "button[data-automation-id='ok_button']")]
        private IWebElement gradesetVersionOkButton;



        /// <summary>
        /// click to display added grade in Grade list.
        /// </summary>
        /// <returns></returns>
        public GradesetDataMaintenance ClickGratesetVersionOkButton()
        {
            gradesetVersionOkButton.Click();
            Thread.Sleep(2000);
            return new GradesetDataMaintenance();
        }

        /// <summary>
        /// clicks to add new row to insert grade.
        /// </summary>
        /// <returns></returns>
        public EditGradesetVersion ClickAddGradeButton()
        {
            addGradeButton.Click();
            waiter.Until(ExpectedConditions.ElementToBeClickable(gradesetVersionOkButton));
            while (true)
            {
                if (addGradeButton.GetAttribute("disabled") != "true")
                {
                    break;
                }
            }
            return new EditGradesetVersion();
        }

        /// <summary>
        /// sets a gradeset version code.
        /// </summary>
        /// <param name="gradeSetVersionCode"></param>
        public void SetGradeSetVersionCode(string gradeSetVersionCode)
        {
            gradesetCode.Clear();
            gradesetCode.SendKeys(gradeSetVersionCode);
        }

        /// <summary>
        /// sets a gradeset version description.
        /// </summary>
        /// <param name="gradeSetVersionCode"></param>
        public void SetGradeSetVersionDescription(string gradeSetVersionDescription)
        {
            gradesetDescription.Clear();
            gradesetDescription.SendKeys(gradeSetVersionDescription);
        }

        /// <summary>
        /// sets a gradeset version value.
        /// </summary>
        /// <param name="gradeSetVersionCode"></param>
        public void SetGradeSetVersionValue(string gradeSetVersionValue)
        {
            gradesetValue.Clear();
            gradesetValue.SendKeys(gradeSetVersionValue);
        }
    }
}
