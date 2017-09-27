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
using SeSugar.Automation;


namespace Assessment.Components.PageObject
{
    public class EditMarksheetTemplate : BaseSeleniumComponents
    {
        public EditMarksheetTemplate()
        {
            WebContext.WebDriver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(MarksheetConstants.Timeout));
            PageFactory.InitElements(WebContext.WebDriver, this);
        }

        WebDriverWait waiter = new WebDriverWait(WebContext.WebDriver, TimeSpan.FromSeconds(MarksheetConstants.Timeout));

        //[FindsBy(How = How.CssSelector, Using = "div[class='modal-content'] form[data-automation-id='search_criteria'] input[name='MarksheetName']")]
        //private IWebElement templateSeacrhText;

        [FindsBy(How = How.CssSelector, Using = "div[class='modal-content'] form[data-automation-id='search_criteria'] button[data-automation-id='search_criteria_submit']")]
        private IWebElement SearchButton;

        [FindsBy(How = How.CssSelector, Using = "div[class='modal-content'] button[data-automation-id='open_template_button']")]
        private IWebElement OpenTemplateButton;

        [FindsBy(How = How.CssSelector, Using = "div[class='modal-content'] button[data-automation-id='select_template_button']")]
        private IWebElement SelectTemplateButton;


        private static By TemplateSearchResult = By.CssSelector("div[class='modal-content'] a[class='search-result h1-result']");

        private static By templateSeacrhText = By.CssSelector("div[class='modal-content'] form[data-automation-id='search_criteria'] input[name='MarksheetName']");

        private static By templateKeywordSeacrhText = By.CssSelector("div[class='modal-content'] form[data-automation-id='search_criteria'] input[name='KeywordSearchText']");

        [FindsBy(How = How.CssSelector, Using = "a[data-automation-id='Button_Dropdown']")]
        private IWebElement CreateMarksheetButton;

        [FindsBy(How = How.CssSelector, Using = "div[class='modal-content'] form[data-automation-id='search_criteria'] input[name='MarksheetName']")]
        private IWebElement marksheetName;

        [FindsBy(How = How.CssSelector, Using = "input[name='MarksheetTemplateName']")]
        private IWebElement marksheetTemplateName;


        /// <summary>
        /// It sets the given Aspect name in the Name text field
        /// </summary>
        public void SearchTemplateByName(string TemplateName)
        {

            WaitForAndSetValue(TimeSpan.FromSeconds(MarksheetConstants.Timeout), templateSeacrhText, TemplateName, true);
            //templateSeacrhText.Clear();
            //templateSeacrhText.SendKeys(TemplateName);
        }

        public void SearchTemplateByKeywords(string Keyword)
        {

            WaitForAndSetValue(TimeSpan.FromSeconds(MarksheetConstants.Timeout), templateKeywordSeacrhText, Keyword, true);
            //templateSeacrhText.Clear();
            //templateSeacrhText.SendKeys(TemplateName);
        }

        /// <summary>
        /// Clicks on the Search Button
        /// </summary>
        public EditMarksheetTemplate Search()
        {
            Thread.Sleep(4000);
            waiter.Until(ExpectedConditions.ElementToBeClickable(SearchButton)).Click();
            while (true)
            {
                if (SearchButton.GetAttribute("disabled") != "true")
                    break;
            }

            return new EditMarksheetTemplate();
        }

        /// <summary>
        /// Clicks on the Open Template Button
        /// </summary>
        public CurriculumMarksheetMaintainance OpenTemplate()
        {
            
            waiter.Until(ExpectedConditions.ElementToBeClickable(OpenTemplateButton)).Click();
            //  OpenTemplateButton.Click();
            WaitUntillAjaxRequestCompleted();            
            return new CurriculumMarksheetMaintainance();
        }

        /// <summary>
        /// Clicks on the Select Template Button
        /// </summary>
        public EditMarksheetTemplate SelectTemplate()
        {
            SelectTemplateButton.WaitUntilState(ElementState.Displayed);
            SelectTemplateButton.Click();
            return this;
        }

        public bool IsNewFromExistingDialogVisible()
        {
            marksheetName.WaitUntilState(ElementState.Displayed);
            return marksheetName.Displayed;            
        }

        /// <summary>
        /// Selects a particular Template based on the Template Name
        /// </summary>
        public EditMarksheetTemplate SelectTemplateByName(string TemplateName)
        {
            waiter.Until(ExpectedConditions.PresenceOfAllElementsLocatedBy(TemplateSearchResult));
            ReadOnlyCollection<IWebElement> TemplateList = WebContext.WebDriver.FindElements(TemplateSearchResult);
            foreach (IWebElement eachelement in TemplateList)
            {
                if (eachelement.Text == TemplateName)
                {
                    eachelement.Click();
                    Thread.Sleep(2000);
                    break;
                }
            }
            return new EditMarksheetTemplate();
        }

        /// <summary>
        /// Selects a particular Template based on the Template Name
        /// </summary>
        public bool IsSearchedMarksheetTemplatePresent(string TemplateName)
        {
            ReadOnlyCollection<IWebElement> TemplateList = WebContext.WebDriver.FindElements(TemplateSearchResult);
            foreach (IWebElement eachelement in TemplateList)
            {
                if (eachelement.Text == TemplateName)
                {
                    return true;                    
                }
            }
            return false;
        }

        public string getMarksheetTemplateNameFromNewFromExistingDialog()
        {
            marksheetTemplateName.WaitUntilState(ElementState.Displayed);
            return marksheetTemplateName.GetValue();
        }

        /// <summary>
        /// Selects a particular Template based on the Template Name
        /// </summary>
        public String GetExistingTemplateNameInEditDialog(string TemplateName)
        {
            ReadOnlyCollection<IWebElement> TemplateList = WebContext.WebDriver.FindElements(TemplateSearchResult);
            String getTemplateName = "";
            foreach (IWebElement eachelement in TemplateList)
            {
                if (eachelement.Text == TemplateName)
                {
                    getTemplateName = eachelement.Text;
                    Thread.Sleep(2000);
                    break;
                }
            }
            return getTemplateName;
        }
    }
}
