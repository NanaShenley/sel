using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assessment.Components.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using SharedComponents.BaseFolder;
using OpenQA.Selenium.Support.PageObjects;
using OpenQA.Selenium.Support.UI;
using SharedComponents.Helpers;
using WebDriverRunner.webdriver;
using System.Threading;
using OpenQA.Selenium.Interactions;
using SharedComponents.Helpers;
using SeSugar.Automation;

namespace Assessment.Components.PageObject
{
    public class POSSearchPannel : BaseSeleniumComponents
    {
        public POSSearchPannel()
        {
            WebContext.WebDriver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(MarksheetConstants.Timeout));
            PageFactory.InitElements(WebContext.WebDriver, this);
        }

        [FindsBy(How = How.CssSelector, Using = "button[data-automation-id='search_criteria_submit']")]
        private IWebElement SearchButton;

        [FindsBy(How = How.CssSelector, Using = "input[name='LearningLevel.dropdownImitator']")]
        private IWebElement GroupSelector;

        [FindsBy(How = How.CssSelector, Using = "input[name='ViewOptions.dropdownImitator']")]
        private IWebElement ViewSelector;

        [FindsBy(How = How.CssSelector, Using = "input[name='TopicSelector.dropdownImitator']")]
        private IWebElement TopicSelector;

        [FindsBy(How = How.CssSelector, Using = "input[name='LearningLevel.dropdownImitator']")]
        private IWebElement PhaseSelector;

        [FindsBy(How = How.CssSelector, Using = "input[name='SchemeSelector.dropdownImitator']")]
        private IWebElement SchemeSelector;


        [FindsBy(How = How.CssSelector, Using = "input[name='Subject.dropdownImitator']")]
        private IWebElement SubjectSelector;

        [FindsBy(How = How.CssSelector, Using = "input[name='Strands.dropdownImitator']")]
        private IWebElement StrandSelector;

        [FindsBy(How = How.CssSelector, Using = "input[name='AssessmentPeriod.dropdownImitator']")]
        private IWebElement AssessmentPeriodSelector;

        [FindsBy(How = How.CssSelector, Using = "a[data-automation-id='section_menu_Year Group']")]
        private IWebElement OpenYearGroupSelectorLink;

        [FindsBy(How = How.CssSelector, Using = "a[data-automation-id='section_menu_Group Selection']")]
        private IWebElement OpenGroupSelectionLink;

        [FindsBy(How = How.CssSelector, Using = "li[class='show-left-panel-toolbar layout-two-column-master-toolbar-hidden display-only']")]
        private IWebElement OpenSearchPanelLink;

        [FindsBy(How = How.CssSelector, Using = "input[name='YearGroupCohert.dropdownImitator']")]
        private IWebElement OpenYearGroupSelector;

        [FindsBy(How = How.CssSelector, Using = "input[name='Classess.dropdownImitator']")]
        private IWebElement OpenClassSelector;

        WebDriverWait waiter = new WebDriverWait(WebContext.WebDriver, TimeSpan.FromSeconds(MarksheetConstants.Timeout));

        private static By YearGroupsFilterList = By.CssSelector("input[name='YearGroupCohert.SelectedIds']");

        POSDataMaintainanceScreen posDataMaintainanceScreen = new POSDataMaintainanceScreen();

        //Get the POS title
        public String GetPOSTitle()
        {
            String POSText = WebContext.WebDriver.FindElement(By.CssSelector("span[data-automation-id = 'programme_of_study_tracking_header_title']")).Text;
            return POSText;
        }


        /// <summary>
        /// Clicks on the Search Button
        /// </summary>
        public POSDataMaintainanceScreen Search()
        {
            Thread.Sleep(2000);
            waiter.Until(ExpectedConditions.ElementToBeClickable(SearchButton)).Click();
         
            // This method allows user to wait until the results are getting displayed after click of serach button
            while (true)
            {
                if (SearchButton.GetAttribute("disabled") != "true")
                    break;
            }

            WaitUntillAjaxRequestCompleted();
            return new POSDataMaintainanceScreen();
        }

        /// <summary>
        /// Select a Group on Gradeset Pannel
        /// </summary>
        public POSSearchPannel SelectGroup(string GroupName)
        {
            SeleniumHelper.ChooseSelectorOption(GroupSelector, GroupName);
            WaitUntillAjaxRequestCompleted();
            Thread.Sleep(6000);
            return new POSSearchPannel();
        }
        /// <summary>
        /// Select a view (Topic/scheme)
        ///  </summary>
        public POSSearchPannel SelectView(string view)
        {
            waiter.Until(ExpectedConditions.ElementToBeClickable(ViewSelector));
            SeleniumHelper.ChooseSelectorOption(ViewSelector, view);
            return new POSSearchPannel();
        }

        /// <summary>
        /// Select a Scheme
        ///  </summary>
        public POSSearchPannel SelectScheme(string schemeName)
        {
            SeleniumHelper.ChooseSelectorOption(SchemeSelector, schemeName);
            return new POSSearchPannel();
        }

        /// <summary>
        /// Select a Topic
        ///  </summary>
        public POSSearchPannel SelectTopic(string topicName)
        {

            waiter.Until(ExpectedConditions.ElementToBeClickable(TopicSelector));
            SeleniumHelper.ChooseSelectorOption(TopicSelector, topicName);
            return new POSSearchPannel();
        }

        /// <summary>
        /// Select phase
        /// </summary>
        /// <param name="Phase Name"></param>
        /// <returns></returns>
        public bool SelectPhase(string Pahse)
        {

            waiter.Until(ExpectedConditions.ElementToBeClickable(PhaseSelector));
            SeleniumHelper.ChooseSelectorOption(PhaseSelector, Pahse);
            return true;
        }

        /// <summary>
        /// Select a Subject on Gradeset Pannel
        /// </summary>
        public POSSearchPannel SelectSubject(string SubjectName)
        {
            SeleniumHelper.ChooseSelectorOption(SubjectSelector, SubjectName);
            WaitUntillAjaxRequestCompleted();
            return new POSSearchPannel();
        }

        /// <summary>
        /// Select a Strand on Gradeset Pannel
        /// </summary>
        public POSSearchPannel SelectStrand(string StrandName)
        {
            SeleniumHelper.ChooseSelectorOption(StrandSelector, StrandName);
            WaitUntillAjaxRequestCompleted();
            return new POSSearchPannel();
        }

        /// <summary>
        /// Select a Assessment Period on Gradeset Pannel
        /// </summary>
        public POSSearchPannel SelectAssessmentPeriod(string APName)
        {
            SeleniumHelper.ChooseSelectorOption(AssessmentPeriodSelector, APName);
            WaitUntillAjaxRequestCompleted();
            return new POSSearchPannel();
        }

        /// <summary>
        /// Open a Year Group selection dropdown
        /// </summary>
        public POSSearchPannel OpenYearGroupSelectionDropdown()
        {
            OpenYearGroupSelectorLink.Click();
            while (true)
            {
                if (OpenYearGroupSelectorLink.GetAttribute("aria-expanded") == "true")
                    break;
            }
            return new POSSearchPannel();
        }

        /// <summary>
        /// Opens Filter/Search Criteria panel
        /// </summary>
        public POSSearchPannel OpenSearchPanel()
        {
            OpenSearchPanelLink.Click();
            return new POSSearchPannel();
        }
        /// <summary>
        /// Select a Year Group
        /// </summary>
        public void SelectYearGroup(string YearGroupName)
        {
            ReadOnlyCollection<IWebElement> YearGroupsList = WebContext.WebDriver.FindElements(YearGroupsFilterList);
            foreach (IWebElement eachelement in YearGroupsList)
            {
                if (!eachelement.Selected && WebContext.WebDriver.FindElement(By.CssSelector("label[for='" + eachelement.GetAttribute("id") + "']")).Text == YearGroupName)
                {
                    WebContext.WebDriver.FindElement(By.CssSelector("label[for='" + eachelement.GetAttribute("id") + "']")).Click();
                }
            }
        }

        public POSSearchPannel OpenYearGroupSelectionDropdown(string YearGroup)
        {

            SeleniumHelper.ChooseSelectorOption(OpenYearGroupSelector, YearGroup);
            return new POSSearchPannel();

            }

        public POSSearchPannel OpenClassDropdown(string Class)
        {

            SeleniumHelper.ChooseSelectorOption(OpenClassSelector, Class);
            return new POSSearchPannel();

        }
        /// <summary>
        /// Verifies the selected class from the cohort selection
        /// </summary>
        /// <param name="Class"></param>
        /// <returns></returns>
        public bool VerifySelectedClass(string Class)
        {
            return OpenClassSelector.GetValue().Equals(Class);
        }

    }
}