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

namespace Assessment.Components.PageObject
{
    public class GradesetSearchPanel : BaseSeleniumComponents
    {
        public GradesetSearchPanel()
        {
            WebContext.WebDriver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(MarksheetConstants.Timeout));
            PageFactory.InitElements(WebContext.WebDriver, this);
        }

        //[FindsBy(How = How.CssSelector, Using = "form[data-automation-id='search_criteria'] input[name='Name']")]
        //private IWebElement GradesetNameTextField;

        [FindsBy(How = How.CssSelector, Using = "form[data-section-id='searchCriteria'] input[name='AssessmentGradesetType.dropdownImitator']")]
        private IWebElement GradesetTypeDropdownInitiator;

        [FindsBy(How = How.CssSelector, Using = "form[data-section-id='dialog-searchCriteria'] button[data-automation-id='search_criteria_submit']")]
        private IWebElement SearchButtonViaAssignGradeset;

        [FindsBy(How = How.CssSelector, Using = "button[data-automation-id='search_criteria_submit']")]
        private IWebElement SearchButton;

        [FindsBy(How = How.CssSelector, Using = "span[data-automation-id='search_results_counter']")]
        private IWebElement SearchResultCount;

        [FindsBy(How = How.CssSelector, Using = "button[data-ajax-url*='/Assessment/SIMS8GradesetSearchAssessmentGradeset/Search']")]
        private IWebElement GradesetPalletteSearch;

        [FindsBy(How = How.CssSelector, Using = "button[data-automation-id='ok_button']")]
        private IWebElement GradesetPalletteOkButton;

        //[FindsBy(How = How.CssSelector, Using = "form[data-automation-id='search_criteria'] h2.search-criteria-header")]
        //private IWebElement GradesetHeader;

        private static By GradesetNameTextField = By.CssSelector("form[data-automation-id='search_criteria'] input[name='Name']");

        WebDriverWait waiter = new WebDriverWait(WebContext.WebDriver, TimeSpan.FromSeconds(MarksheetConstants.Timeout));


        // Search Result List
        private static By SearchResultNameList = By.CssSelector("div[data-automation-id='search_result'] a[class='search-result h1-result'] span[class='search-result-detail']");
        private static By SearchResultGradeSetTypeList = By.CssSelector("div[data-automation-id='search_results'] a[title='Gradeset Type']");
        private static By GradeSetResultsList = By.CssSelector("div.select2-result-label");
        private static By GradesetNameField = By.CssSelector("form[id='dialog-editableData'] input[name='Name']");
        /// <summary>
        /// Clicks on the Search Button
        /// </summary>
        public GradesetSearchPanel Search(bool ViaAssignGradesetScreen)
        {
            if(ViaAssignGradesetScreen == true)
            {
                WaitUntilDisplayed(GradesetNameTextField);
                //waiter.Until(ExpectedConditions.ElementToBeClickable(SearchButtonViaAssignGradeset));
                SearchButtonViaAssignGradeset.Click();
                while (true)
                {
                    if (SearchButtonViaAssignGradeset.GetAttribute("disabled") != "true")
                        break;
                }
            }
            else
            {
                //  waiter.Until(ExpectedConditions.TextToBePresentInElement(GradesetHeader, "Filter By"));
                Thread.Sleep(3000);
                waiter.Until(ExpectedConditions.ElementToBeClickable(SearchButton));
                SearchButton.Click();
                Thread.Sleep(3000);
                while (true)
                {
                    if (SearchButton.GetAttribute("disabled") != "true")
                        break;
                }
            }

            // This method allows user to wait until the results are getting displayed after click of serach button

            return new GradesetSearchPanel();
        }

        /// <summary>
        /// Returns the result count for the search parameter and returns "No Gradesets Found" in case of No Results are found for the search parameter.
        /// </summary>
        public string GetSearchResultCount()
        {
            //Explicit Sleep is used over here because we are not sure till when should we wait to get those search result, as the serach criteria is different for diffrent cases. So, have to use Thread.Sleep over here
            //System.Threading.Thread.Sleep(5000);
            try
            {
                return SearchResultCount.Text;
            }
            catch
            {
                return "No Gradesets Found";
            }
        }

        /// <summary>
        /// Returns all the GradeSet Name Results as a part of List and returns "No Grade Set Name Found" in case of No Result found.
        /// </summary>
        public List<string> GetGradeSetNameResult()
        {
            //Explicit Sleep is used over here because we are not sure till when should we wait to get those search result, as the serach criteria is different for diffrent cases. So, have to use Thread.Sleep over here
            //System.Threading.Thread.Sleep(5000);
            List<string> GradeSetNameResults = new List<string>();
            ReadOnlyCollection<IWebElement> GradeSetNameResultList = WebContext.WebDriver.FindElements(SearchResultNameList);
            if (GradeSetNameResultList.Count == 0)
            {
                GradeSetNameResults.Add("No Grade Set Name Found");
                return GradeSetNameResults;
            }

            else
            {
                foreach (IWebElement eachelement in GradeSetNameResultList)
                {
                    GradeSetNameResults.Add(eachelement.Text);
                }
                return GradeSetNameResults;
            }
        }

        /// <summary>
        /// Returns all the GradeSet Type Results as a part of List and returns "No Gradeset Type Found" in case of No Result found.
        /// </summary>
        public List<string> GetGradeSetTypeResult()
        {
            //Explicit Sleep is used over here because we are not sure till when should we wait to get those search result, as the serach criteria is different for diffrent cases. So, have to use Thread.Sleep over here
            //System.Threading.Thread.Sleep(5000);
            List<string> GradeSetTypeResults = new List<string>();
            ReadOnlyCollection<IWebElement> GradeSetTypeResultList = WebContext.WebDriver.FindElements(SearchResultGradeSetTypeList);
            if (GradeSetTypeResultList.Count == 0)
            {
                GradeSetTypeResults.Add("No Gradeset Type Found");
                return GradeSetTypeResults;
            }

            else
            {
                foreach (IWebElement eachelement in GradeSetTypeResultList)
                {
                    GradeSetTypeResults.Add(eachelement.Text);
                }
                return GradeSetTypeResults;
            }
        }

        /// <summary>
        /// Selects a particular Gardeset based on the Gradeset Name
        /// </summary>
        public GradesetDataMaintenance SelectGradesetByName(string GradesetName)
        {
            ReadOnlyCollection<IWebElement> GradeSetNameResultList = WebContext.WebDriver.FindElements(SearchResultNameList);
            foreach (IWebElement eachelement in GradeSetNameResultList)
            {
                if (eachelement.Text == GradesetName)
                {
                    eachelement.Click();
                    Thread.Sleep(2000);
                    break;
                }                    
            }
            return new GradesetDataMaintenance();
        }


        /// <summary>
        /// It sets the given gradeset name in the Name text field
        /// </summary>
        public void SetGradeSetName(string GradesetName)
        {
            waiter.Until(ExpectedConditions.ElementIsVisible(GradesetNameTextField));
            WebContext.WebDriver.FindElement(GradesetNameTextField).Clear();
            WebContext.WebDriver.FindElement(GradesetNameTextField).SendKeys(GradesetName);
        }

        /// <summary>
        /// It sets the given gradeset type in the Name text field
        /// </summary>
        public void SetGradeSetType(string GradeSetType)
        {
            GradesetTypeDropdownInitiator.Click();
            ReadOnlyCollection<IWebElement> List = WebContext.WebDriver.FindElements(GradeSetResultsList);
            foreach (IWebElement eachelement in List)
            {
                if (eachelement.Text == GradeSetType)
                {
                    eachelement.Click();
                    break;
                }
            }

        }

        /// <summary>
        /// Click On Search Button
        /// </summary>
        public GradesetSearchPanel PalletteSearch()
        {
            waiter.Until(ExpectedConditions.ElementToBeClickable(GradesetPalletteSearch));
            GradesetPalletteSearch.Click();
            // This method allows user to wait until the results are getting displayed after click of search button
            while (true)
            {
                if (GradesetPalletteSearch.GetAttribute("disabled") != "true")
                    break;
            }
            return new GradesetSearchPanel();
        }


        /// <summary>
        /// Click on Ok Button
        /// </summary>
        public GradesetSearchPanel ClickOkButton(bool ViaAspectScreen, String GradesetName)
        {
            waiter.Until(ExpectedConditions.ElementIsVisible(GradesetNameField));
            waiter.Until(ExpectedConditions.ElementToBeClickable(GradesetPalletteOkButton));
            GradesetPalletteOkButton.Click();
            // This method allows user to wait until the results are getting displayed after click of serach button
            //while (true)
            //{
            //    if (GradesetPalletteOkButton.GetAttribute("disabled") != "true")
            //        break;
            //}

            //It will wait untill the Grdeset is loaded in the Gradeset dropdown
            if (ViaAspectScreen == true)
            {
                String GradesetSelected = "";
                int timeout = 1000;
                while (timeout != 10000)
                {
                    timeout = timeout + 1000;
                    GradesetSelected = WebContext.WebDriver.FindElement(By.CssSelector("input[name='AssessmentGradeset.dropdownImitator']")).GetAttribute("value");
                    Thread.Sleep(2000);
                    Console.WriteLine(timeout);
                    if (GradesetSelected == GradesetName)
                    {
                        Thread.Sleep(6000);
                        break;
                    }
                }
            }
            return new GradesetSearchPanel();
        }

        /// <summary>
        /// Selects a particular Gardeset based on the Gradeset Name
        /// </summary>
        public GradesetDataMaintenance SelectGradeset(int position=0)
        {
            int newposition = 1;
            ReadOnlyCollection<IWebElement> GradeSetNameResultList = WebContext.WebDriver.FindElements(SearchResultNameList);
            foreach (IWebElement eachelement in GradeSetNameResultList)
            {
                if (position == 0 || position == newposition)
                {
                    eachelement.Click();
                    break;
                }
                newposition++;
            }
            return new GradesetDataMaintenance();
        }

        /// <summary>
        /// Selects a particular Gardeset based on the Gradeset Name
        /// </summary>
        public string GetFirstGradesetName()
        {
            ReadOnlyCollection<IWebElement> GradeSetNameResultList = WebContext.WebDriver.FindElements(SearchResultNameList);
            string gradesetname = "";
            foreach (IWebElement eachelement in GradeSetNameResultList)
            {
                gradesetname = eachelement.Text;
                eachelement.Click();
                break;
            }
            return gradesetname;
        }
    }
}
