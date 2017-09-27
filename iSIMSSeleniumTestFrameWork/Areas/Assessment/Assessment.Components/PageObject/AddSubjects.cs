using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Assessment.Components.Common;
using Assessment.Components.PageObject;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using OpenQA.Selenium.Support.UI;
using SharedComponents.BaseFolder;
using SharedComponents.Helpers;
using WebDriverRunner.webdriver;

namespace Assessment.Components.PageObject
{
    public class AddSubjects: BaseSeleniumComponents
    {
        WebDriverWait waiter = new WebDriverWait(WebContext.WebDriver, TimeSpan.FromSeconds(MarksheetConstants.Timeout));

        [FindsBy(How = How.CssSelector, Using = "button[data-automation-id='next-subject-periods']")]
        private readonly IWebElement _btnSubjectNext = null;

        [FindsBy(How = How.CssSelector, Using = "div[create-marksheet-data-section-id='marksheets-subject-']")]
        private IWebElement AssessmentsviaSubjectLink;

        [FindsBy(How = How.CssSelector, Using = "i[data-automation-id='subject-back']")]
        private readonly IWebElement _btnSubjectBack = null;

        [FindsBy(How = How.CssSelector, Using = "form[data-section-id='marksheets-subject-searchCriteria'] input[name='SubjectName']")]
        private readonly IWebElement SubjectNameInput = null;

        [FindsBy(How = How.CssSelector, Using = "div[data-automation-id='marksheets-subject-searchResults'] span[data-automation-id='search_results_counter'] > strong")]
        private readonly IWebElement SubjectSearchResult = null;

        [FindsBy(How = How.CssSelector, Using = "form[data-section-id='marksheets-subject-searchCriteria'] button[data-automation-id='search_criteria_submit']")]
        private readonly IWebElement SubjectSearchButton = null;

        //[FindsBy(How = How.CssSelector, Using = "i[data-automation-id='subjectclosebutton']")]
        //private readonly IWebElement _btnSubjectClose = null;


        private static By SubjectSearchResults = By.CssSelector("div[createmarksheet-assessmentsubject-section] a[createmarksheet-assessmentsubject]");

        public AddSubjects()
        {
           PageFactory.InitElements(WebContext.WebDriver, this);
        }


        public AddModeMethodPurpose SubjectNextButton()
        {
            waiter.Until(ExpectedConditions.ElementToBeClickable(_btnSubjectNext));
            _btnSubjectNext.Click();
            //waiter.Until(ExpectedConditions.ElementExists(MarksheetConstants.ModeElementSelection));
            return new AddModeMethodPurpose();
        }

        public AddAssessments SubjectBackButton()
        {
            waiter.Until(ExpectedConditions.ElementToBeClickable(_btnSubjectBack));
            _btnSubjectBack.Click();
            waiter.Until(ExpectedConditions.ElementToBeClickable(AssessmentsviaSubjectLink));
            return new AddAssessments();
        }

        //public MarksheetBuilder SubjectCloseButton()
        //{
        //    waiter.Until(ExpectedConditions.ElementToBeClickable(_btnSubjectClose));
        //    _btnSubjectClose.Click();
        //    waiter.Until(ExpectedConditions.ElementToBeClickable(MarksheetConstants.AssessmentsLink1));  
        //    return new MarksheetBuilder();
        //}

        /// <summary>
        /// This allows us to select the Subject and move next
        /// </summary>
        public ReadOnlyCollection<IWebElement> GetSubjectList()
        {
            waiter.Until(ExpectedConditions.ElementExists(MarksheetConstants.SubjectElementSelection));            
            ReadOnlyCollection<IWebElement> SubjectElements = WebContext.WebDriver.FindElements(MarksheetConstants.SubjectElementSelection);
            return SubjectElements;        
        }

        /// <summary>
        /// This allows us to select the Subject and move next
        /// </summary>
        public void SelectSubjectResult(int NumberofSubject)
        {
            ReadOnlyCollection<IWebElement> SubjectElements = GetSubjectList();
            int i = 0;
            List<string> selectedResults = new List<string>();
            if (NumberofSubject > 0)
            {
                selectedResults.Clear();
                foreach (IWebElement subjectcodes in SubjectElements)
                {
                    if (subjectcodes.Text != "")
                    {
                        subjectcodes.WaitUntilState(ElementState.Displayed);
                        subjectcodes.Click();
                        selectedResults.Add(subjectcodes.Text);
                        i++;
                    }
                    if (i == NumberofSubject)
                        break;
                }
            }

        }

        /// <summary>
        /// This allows us to select the Subject and move next
        /// </summary>
        public void SelectSubjectResultfromName(string SubjectName)
        {
            ReadOnlyCollection<IWebElement> SubjectElements = GetSubjectList();
            int i = 0;
            List<string> selectedResults = new List<string>();
            
                selectedResults.Clear();
                foreach (IWebElement subjectcodes in SubjectElements)
                {
                    if (subjectcodes.Text == SubjectName)
                    {
                        subjectcodes.WaitUntilState(ElementState.Displayed);
                        subjectcodes.Click();
                        selectedResults.Add(subjectcodes.Text);
                        i++;
                    }
                    
                }
        

        }

        public String checkSubjectIsNotSelcted()
        {
            ReadOnlyCollection<IWebElement> AspectList = GetSubjectList();
            String CheckSelction = "";
            foreach (IWebElement aspectElem in AspectList)
            {
                if (aspectElem.Text != "")
                {
                    CheckSelction = aspectElem.GetAttribute("data-selected");
                    if (CheckSelction == "true")
                        break;
                }
            }
            return CheckSelction;
        }

        /// <summary>
        ///  Is MarksheetPreview Columns Present
        /// </summary>
        public void IsMarksheetPreviewColumnsPresent()
        {
            try
            {
                Assert.IsTrue(WebContext.WebDriver.FindElement(By.CssSelector("div[view_id='gridPreview']")).Displayed, "Grid is present on screen!");
                Assert.IsTrue(WebContext.WebDriver.FindElement(By.CssSelector("div[class='webix_column  webix_first']")).Displayed, "Grid has a column!");
            }
            catch (NoSuchElementException ex)
            {
                Assert.IsTrue(true, "Grid is not preset on the screen!");
            }

        }

        /// <summary>
        /// This method allows user to fill in the text for searching subject
        /// </summary>
        public void EnterSubjectSearchCriteria(String SubjectSearchCriteria)
        {
            SubjectNameInput.Clear();
            Thread.Sleep(2000);
            SubjectNameInput.SetText(SubjectSearchCriteria);
        }

        /// <summary>
        /// This method allows user click the subject search button
        /// </summary>
        public AddSubjects ClickSubjectSearchButton()
        {
            SubjectSearchButton.Click();
            // This method allows user to wait until the results are getting displayed after click of serach button
            while (true)
            {
                if (SubjectSearchButton.GetAttribute("disabled") != "true")
                    break;
            }
            return new AddSubjects();
        }

        /// <summary>
        /// Get Subject Search Results Count
        /// </summary>
        public string GetSubjectSearchResultCount()
        {
            try
            {
                return SubjectSearchResult.Text;
            }
            catch
            {
                return "No Matches";
            }
        }

        /// <summary>
        /// Get Subject Search Results
        /// </summary>
        public List<string> GetSubjectSearchResults()
        {
            List<string> SubjectSearchResultsList = new List<string>();
            ReadOnlyCollection<IWebElement> TemplateSearchResultList = WebContext.WebDriver.FindElements(SubjectSearchResults);
            if (TemplateSearchResultList.Count == 0)
            {
                SubjectSearchResultsList.Add("No Matches");
                return SubjectSearchResultsList;
            }

            else
            {
                foreach (IWebElement eachelement in TemplateSearchResultList)
                {
                    SubjectSearchResultsList.Add(eachelement.Text);
                }
                return SubjectSearchResultsList;
            }
        }

        /// <summary>
        /// Gets the ID of the given subject
        /// </summary>
        public string GetSubjectID(string SubjectName)
        {
            ReadOnlyCollection<IWebElement> TemplateSearchResultList = WebContext.WebDriver.FindElements(SubjectSearchResults);
            string subjectID = "";
            foreach (IWebElement eachelement in TemplateSearchResultList)
            {
                if (eachelement.Text == SubjectName)
                {
                    subjectID = eachelement.GetAttribute("createmarksheet-assessmentsubject-id");
                    break;
                }
            }
            return subjectID;
        }


    }
}
