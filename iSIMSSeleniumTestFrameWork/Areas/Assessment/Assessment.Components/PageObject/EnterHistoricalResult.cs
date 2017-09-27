using Assessment.Components.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.PageObjects;
using OpenQA.Selenium.Support.UI;
using SharedComponents.BaseFolder;
using System;
using WebDriverRunner.webdriver;

namespace Assessment.Components.PageObject
{
    [TestClass]
    public class EnterHistoricalResult : BaseSeleniumComponents
    {
        [FindsBy(How = How.CssSelector, Using = "input[name='AcademicYear.dropdownImitator']")]
        public IWebElement SearchByAcademicYear;

        [FindsBy(How = How.CssSelector, Using = "input[name='YearGroups.dropdownImitator']")]
        public IWebElement SearchByYearGroup;

        [FindsBy(How = How.CssSelector, Using = "input[name='YearGroups.dropdownImitator']")]
        public IWebElement SearchByClass;

        [FindsBy(How = How.CssSelector, Using = "input[name='ViewOptions.dropdownImitator']")]
        public IWebElement SearchByView;

        [FindsBy(How = How.CssSelector, Using = "input[name='LearningLevel.dropdownImitator']")]
        public IWebElement SearchByPhase;

        [FindsBy(How = How.CssSelector, Using = "input[name='Subject.dropdownImitator']")]
        public IWebElement SearchBySubject;

        [FindsBy(How = How.CssSelector, Using = "input[name='SearchCriteriaStrands.dropdownImitator']")]
        public IWebElement SearchByStrand;

        [FindsBy(How = How.CssSelector, Using = "button[data-automation-id='search_criteria_submit']")]
        private IWebElement SearchButton;

        public EnterHistoricalResult()
        {
            WebContext.WebDriver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(MarksheetConstants.Timeout));
            PageFactory.InitElements(WebContext.WebDriver, this);
        }

        /// <summary>
        /// Clicks on the Search Button
        /// </summary>
        public EnterHistoricalResult Search()
        {
            WebDriverWait waiter = new WebDriverWait(WebContext.WebDriver, TimeSpan.FromSeconds(MarksheetConstants.Timeout));

            waiter.Until(ExpectedConditions.ElementToBeClickable(SearchButton)).Click();
            
            return this;
        }
    }
}
