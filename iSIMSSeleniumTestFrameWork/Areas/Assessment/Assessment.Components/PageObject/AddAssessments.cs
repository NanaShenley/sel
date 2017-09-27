using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assessment.Components.Common;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using OpenQA.Selenium.Support.UI;
using SharedComponents.BaseFolder;
using WebDriverRunner.webdriver;
using System.Threading;

namespace Assessment.Components.PageObject
{
    public class AddAssessments :BaseSeleniumComponents
    {
        [FindsBy(How = How.CssSelector, Using = "div[create-marksheet-data-section-id='marksheets-aspect-']")]
        private IWebElement AssessmentsviaAssessmentLink;

        [FindsBy(How = How.CssSelector, Using = "div[create-marksheet-data-section-id='marksheets-subject-'] a[class='collection-item-link collection-item-title']")]
        private IWebElement AssessmentsviaSubjectLink;

        [FindsBy(How = How.CssSelector, Using = "div[data-section-id='marksheets-aspect-searchResults'] a[data-automation-id='resultTile']")]
        private IWebElement SearchAssessmentResults;

        [FindsBy(How = How.CssSelector, Using = "div[createmarksheet-assessmentsubject-section] a[createmarksheet-assessmentsubject]")]
        private IWebElement SubjectElements;

        [FindsBy(How = How.CssSelector, Using = "div[data-automation-id='marksheets-aspect-searchResults'] a[assessment-createmarksheet-aspect]")]
        private IWebElement AspectElements;

        WebDriverWait waiter = new WebDriverWait(WebContext.WebDriver, TimeSpan.FromSeconds(MarksheetConstants.Timeout));

        public AddAssessments()
        {
           // waiter.Until(ExpectedConditions.ElementToBeClickable());
            WaitUntilDisplayed(By.CssSelector("div[create-marksheet-data-section-id='marksheets-aspect-']"));
            PageFactory.InitElements(WebContext.WebDriver, this);
        }

        /// <summary>
        /// Navigate to the assessments via assessment
        /// </summary>
        public AddAspects NavigateAssessmentsviaAssessment()
        {
            waiter.Until(ExpectedConditions.ElementToBeClickable(AssessmentsviaAssessmentLink));
            Thread.Sleep(1000);
            AssessmentsviaAssessmentLink.Click();
            waiter.Until(ExpectedConditions.ElementIsVisible(MarksheetConstants.AspectElementSelection));
            return new AddAspects();
        }


        /// <summary>
        /// Navigate to the assessments via subject
        /// </summary>
        public AddSubjects NavigateAssessmentsviaSubject()
        {
            waiter.Until(ExpectedConditions.ElementToBeClickable(AssessmentsviaSubjectLink));
            Thread.Sleep(1000);
            AssessmentsviaSubjectLink.Click();
            waiter.Until(ExpectedConditions.ElementIsVisible(MarksheetConstants.SubjectElementSelection));
            return new AddSubjects();
        }
    }
}
