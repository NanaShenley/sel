using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using SharedComponents.BaseFolder;
using SharedComponents.Helpers;
using System.Collections.Generic;
using System.Linq;
using Assessment.Components.Common;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using WebDriverRunner.webdriver;

namespace Assessment.Components
{
    public class PredefinedAssessment : BaseSeleniumComponents
    {
        public const string AssessmentSerachFormSelector = "form[data-section-id='marksheets-aspect-searchCriteria']";
        public const string SearchButtonSelector = "button[data-automation-id='search_criteria_submit']";
        public const string PredefinedAspectDivSelector = "div[data-automation-id='marksheets-aspect-searchResults']";
        public const string AspectCheckboxSelector = "[data-automation-id='select_aspect']";
        public const string AspectSearchResultAdhocDivSelector = "div[data-section-id='marksheets-aspect-selectedResults']";
        public const string AssessmentPeriodSelector = "[data-automation-id='table_select_period']";
        public const string IsStatutory = "input[name='IsStatutory']";
        public const string PeriodSelector = "[data-automation-id='ddl_select_period']";
        public const string AssessmentPeriodRowSelector = "[data-automation-id='row_assessmentPeriod']";
        public const string CheckBxSelector = "input[type='checkbox']";

        [FindsBy(How = How.CssSelector, Using = "input[name='AspectName']")]
        private IWebElement AspectName;

        [FindsBy(How = How.CssSelector, Using = "button[data-automation-id='next_add_predefined_assessments']")]
        private IWebElement Next_Add_Predefined_Aspects;

        [FindsBy(How = How.CssSelector, Using = "button[data-automation-id='done_add_predefined_assessments']")]
        private IWebElement AssessmentPeriodAssignDone;

        [FindsBy(How = How.CssSelector, Using = "button[data-automation-id='Add_Year']")]
        private IWebElement Add_Year_Predefined_Aspects;

        [FindsBy(How = How.CssSelector, Using = "button[data-automation-id='Apply_Selected_Year']")]
        private IWebElement Apply_Selected_Year_Predefined_Aspects;

        //[FindsBy(How = How.CssSelector, Using = "input[name='IsStatutory']")]
        //private IWebElement IS_Statutory;

        WebDriverWait waiter = new WebDriverWait(WebContext.WebDriver, TimeSpan.FromSeconds(MarksheetConstants.Timeout));
        public PredefinedAssessment()
        {
            PageFactory.InitElements(WebContext.WebDriver, this);
        }

        public PredefinedAssessment SerachPredefinedAspects()
        {
            AspectName.WaitUntilState(ElementState.Displayed);
            AspectName.SendKeys("k");
            PerformEnterKeyBehavior();
            return this;
        }

        //public PredefinedAssessment SerachPredefinedStatutoryAspects()
        //{
        //    AspectName.WaitUntilState(ElementState.Displayed);
        //    AspectName.SendKeys("k");
        //    IS_Statutory.Click();
        //    PerformEnterKeyBehavior();
        //    return this;
        //}

        public static void PerformEnterKeyBehavior()
        {
            Actions action = new Actions(WebContext.WebDriver);
            action.SendKeys(Keys.Enter).Perform();
        }
        public PredefinedAssessment SelectPredefinedAspects()
        {
            IWebElement element = WebContext.WebDriver.FindElement(By.CssSelector(PredefinedAspectDivSelector));
            List<IWebElement> elements = element.FindElements(By.CssSelector(CheckBxSelector)).ToList();

            if (elements != null && elements.Count > 0)
            {
                elements[0].Click();
                elements[2].Click();
                elements[5].Click();
            }
            Next_Add_Predefined_Aspects.Click();
            return this;
        }

        public PredefinedAssessment SelectHowOftenYouAccess()
        {
            List<IWebElement> elements = WebContext.WebDriver.FindElements(By.CssSelector(AspectCheckboxSelector)).ToList();
            if (elements != null && elements.Count > 0)
            {
                for (int i = 0; i < elements.Count; i++)
                {
                    if (!elements[i].Selected)
                        elements[i].Click();
                }
                System.Threading.Thread.Sleep(1000);
                SelectYearsOfTheRow(0);
                WaitUntillAjaxRequestCompleted();
                Add_Year_Predefined_Aspects.Click();
                SelectYearsOfTheRow(1);
                WaitUntillAjaxRequestCompleted();
                Apply_Selected_Year_Predefined_Aspects.Click();
            }

            return this;
        }

        public void SelectYearsOfTheRow(int rowID)
        {
            IWebElement periodSelectorTable = WebContext.WebDriver.FindElement(By.CssSelector(AssessmentPeriodSelector));
            if (periodSelectorTable != null)
            {
                List<IWebElement> rowElements = periodSelectorTable.FindElements(By.CssSelector(PeriodSelector)).ToList();
                if (rowElements != null && rowElements.Count > 0)
                {
                    rowElements[rowID].Click();
                }
                MarksheetGridHelper.DownArrowKeyBehavior();
                MarksheetGridHelper.PerformEnterKeyBehavior();

                IWebElement YearSelector = periodSelectorTable.FindElements(By.CssSelector(AssessmentPeriodRowSelector)).ToList()[rowID];
                List<IWebElement> checkboxList = YearSelector.FindElements(By.CssSelector(CheckBxSelector)).ToList();
                if(checkboxList != null && checkboxList.Count>0)
                {
                    checkboxList[0].Click();
                }
            }
        }

        public PredefinedAssessment ClickDone()
        {
            AssessmentPeriodAssignDone.Click();
            return this;
        }
    }
}
