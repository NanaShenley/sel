using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Assessment.Components.Common;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using OpenQA.Selenium.Support.UI;
using SharedComponents.BaseFolder;
using SharedComponents.Helpers;
using WebDriverRunner.webdriver;

namespace Assessment.Components.PageObject
{
    public class AddAspects : BaseSeleniumComponents
    {

        WebDriverWait waiter = new WebDriverWait(WebContext.WebDriver, TimeSpan.FromSeconds(MarksheetConstants.Timeout));

        [FindsBy(How = How.CssSelector, Using = "input[name='AspectName']")]
        private readonly IWebElement _assessmentName = null;

        [FindsBy(How = How.CssSelector, Using = "button[data-ajax-url$='/Assessment/CreateMarkSheet/SearchAspect']")]
        private readonly IWebElement _btnSearch = null;

        [FindsBy(How = How.CssSelector, Using = "i[data-automation-id='aspect-back']")]
        private readonly IWebElement _btnAspectBack = null;

        [FindsBy(How = How.CssSelector, Using = "button[data-automation-id='next-aspects-periods']")]
        private readonly IWebElement _btnAspectNextButton = null;


        //[FindsBy(How = How.CssSelector, Using = "i[data-automation-id='aspectclosebutton']")]
        //private readonly IWebElement _btnAspectClose = null;


        private static By AspectResult = By.CssSelector("div[data-automation-id='marksheets-aspect-searchResults']  div.list-group-item.search-result-tile");
        private static By AspectSearchResult = By.CssSelector("div[data-section-id='marksheets-aspect-searchResults'] > div.panel.panel-default > div.panel-heading > span.result-counter");
        private static readonly By AspectElementSelection = By.CssSelector("div[data-automation-id='marksheets-aspect-searchResults'] a[assessment-createmarksheet-aspect]");

        public AddAspects()
        {
            WebContext.WebDriver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(MarksheetConstants.Timeout));
            PageFactory.InitElements(WebContext.WebDriver, this);
        }

        /// <summary>
        /// This allows us to select the Assessment and move next
        /// </summary>
        public AddAssessmentPeriod SelectAssessmentsAndMoveNext(int NumberofAspects)
        {
            waiter.Until(ExpectedConditions.ElementIsVisible(AspectResult));
            IWebElement SearchResultElement = WebContext.WebDriver.FindElement(AspectSearchResult);
            ReadOnlyCollection<IWebElement> AspectElements = WebContext.WebDriver.FindElements(AspectResult);
            int i = 0;
            List<string> selectedResults = new List<string>();
            List<string> columns = new List<string>();
            foreach (IWebElement aspectElem in AspectElements)
            {

                if (aspectElem.Text != "" && NumberofAspects != 0)
                {
                    aspectElem.WaitUntilState(ElementState.Displayed);
                    aspectElem.Click();

                    i++;
                }
                if (i == NumberofAspects)
                    break;
            }

            return AspectNextButton();
        }


        /// <summary>
        /// Clicks on the Next button and moves the control on the Add Assessment Screen
        /// </summary>
        public AddAssessmentPeriod AspectNextButton()
        {
            waiter.Until(ExpectedConditions.ElementToBeClickable(_btnAspectNextButton));
            _btnAspectNextButton.Click();
            while (true)
            {
                if (_btnAspectNextButton.GetAttribute("disabled") != "true")
                    break;
            }

            return new AddAssessmentPeriod();
        }

        /// <summary>
        /// Clicks on the back button and moves the control on the Add Assessment Screen
        /// </summary>
        public AddAssessments AspectBackButton()
        {

            waiter.Until(ExpectedConditions.ElementToBeClickable(_btnAspectBack));
            _btnAspectBack.Click();
            Thread.Sleep(2000);
            return new AddAssessments();
        }


        /// <summary>
        /// This allows us to select the Assessment and returns the list of selected assessments
        /// </summary>
        public List<String> SelectNReturnSelectedAssessments(int NumberofAspects)
        {
            ReadOnlyCollection<IWebElement> AspectList = GetAspectList();
            List<String> selectedAspects = new List<String>();
            int i = 0;
            foreach (IWebElement aspectElem in AspectList)
            {
                String CheckSelction = aspectElem.GetAttribute("data-selected");
                if (aspectElem.Text != "" && NumberofAspects != 0 && CheckSelction == "false")
                {
                    aspectElem.WaitUntilState(ElementState.Displayed);
                    aspectElem.Click();
                    selectedAspects.Add(aspectElem.Text);
                    i++;
                }
                if (i == NumberofAspects)
                    break;
            }
            return selectedAspects;
        }




        public ReadOnlyCollection<IWebElement> GetAspectList()
        {
            waiter.Until(ExpectedConditions.ElementIsVisible(AspectElementSelection));
            ReadOnlyCollection<IWebElement> AspectElements = WebContext.WebDriver.FindElements(AspectElementSelection);
            return AspectElements;
        }

        public String checkAspectIsNotSelcted()
        {
            ReadOnlyCollection<IWebElement> AspectList = GetAspectList();
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

        public List<String> getSelectedAspectList()
        {
            ReadOnlyCollection<IWebElement> AspectList = GetAspectList();
            String CheckSelction = "";
            List<String> aspectSelectionList = new List<string>();
            foreach (IWebElement aspectElem in AspectList)
            {
                if (aspectElem.Text != "")
                {
                    CheckSelction = aspectElem.GetAttribute("data-selected");
                    if (CheckSelction == "true")
                        aspectSelectionList.Add(aspectElem.Text);
                }
            }
            return aspectSelectionList;
        }

        /// <summary>
        /// Method is used to enter the AssessmentName
        /// </summary>
        /// <param name="AssessmentName"></param>
        public void EnterAssessmentName(string assessmentName)
        {
            _assessmentName.Clear();
            _assessmentName.SendKeys(assessmentName);
        }

        /// <summary>
        /// Used for search click for Assessment
        /// </summary>
        public AddAspects AspectSearch()
        {
            waiter.Until(ExpectedConditions.ElementToBeClickable(_btnSearch));
            _btnSearch.Click();
            while (true)
            {
                if (_btnSearch.GetAttribute("disabled") != "true")
                    break;
            }

            return new AddAspects();
        }

    }
}
