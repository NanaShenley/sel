using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using OpenQA.Selenium.Support.UI;
using SharedComponents.BaseFolder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebDriverRunner.webdriver;
using System.Windows;
using System.Windows.Forms;
using NUnit.Framework;
using System.Windows.Controls;
using SharedComponents.Helpers;
using OpenQA.Selenium.Interactions;
using AddressBook.Components.Pages;
using SharedComponents.HomePages;
using AddressBook.Test;




namespace AddressBook.Components
{
    public class AddressBookSearchPage : BaseSeleniumComponents
    {
        //Search Text Box
        private const string cssforTextSearch = "shell_global_search_input";
        [FindsBy(How = How.Id, Using = cssforTextSearch)]
        public IWebElement textSearch;

        //  Title of results retreived- Pupils or No records found
        private const string cssforTileTitle = "global_search_heading_Learner";
        private const string cssForPupilContactsHeader = "global_search_heading_LearnerContact";
        private const string cssForStaffHeader = "global_search_heading_Staff";
        private const string cssForClear = "shell_global_search_clear";


        WebDriverWait waiter = new WebDriverWait(WebContext.WebDriver, TimeSpan.FromSeconds(AddressBookElements.Timeout));
        public AddressBookSearchPage()
        {
            // Initiate elements POM
            WaitUntilDisplayed(By.Id("shell_global_search_input"));
            POM.Helper.Wait.WaitLoading();
            PageFactory.InitElements(WebContext.WebDriver, this);
        }

        public void ClearText()
        {
            textSearch.Click();
            WaitForAndClick(new TimeSpan(0, 1, 0), AddressBookElements.ClearButton);
        }

        private TimeSpan _searchTime;
        public double SearchTimeInMillisecs
        {
            get
            {
                return _searchTime.TotalMilliseconds;
            }
        }

        public SearchResultTile EnterSearchTextForPupils(string textValue)
        {
            textSearch.SendKeys(textValue);
            DateTime searchStartTime = DateTime.Now;
            waitForPupilResultstoAppear();
            DateTime searchEndTime = DateTime.Now;
            _searchTime = searchEndTime - searchStartTime;
            return new SearchResultTile();
        }

        public void EnterSearchTextForStaff(string textValue)
        {
            textSearch.SendKeys(textValue);
            DateTime searchStartTime = DateTime.Now;
            waitForStaffResultstoAppear();
            DateTime searchEndTime = DateTime.Now;
            _searchTime = searchEndTime - searchStartTime;
        }

        public void TypeInText(string value)
        {
            textSearch.SendKeys(value);
        }

        public void EnterSearchTextForPupilContacts(string textValue)
        {
            textSearch.SendKeys(textValue);
            DateTime searchStartTime = DateTime.Now;
            waitForPupilContactsResultstoAppear();
            DateTime searchEndTime = DateTime.Now;
            _searchTime = searchEndTime - searchStartTime;
        }

        public AddressBookPopup ClickOnFirstPupilRecord()
        {           
            var elements = WebContext.WebDriver.FindElements(By.CssSelector(SeleniumHelper.AutomationId("PreferredListName_Learner")));  // Elements in result tile
            var resultCount = elements.Count;

            if (resultCount >= 1)
            {
                elements[0].Click();
            }

            return new AddressBookPopup();
        }

        public void HitEnteronFirstPupilRecord()
        {
            var elements = WebContext.WebDriver.FindElements(By.CssSelector(SeleniumHelper.AutomationId("PreferredListName_Learner")));  // Elements in result tile
            var resultCount = elements.Count;

            if (resultCount >= 1)
            {
                POM.Helper.SeleniumHelper.ClickByJS(elements[0]);
            }
        }

        public void ClickOnFirstPupilContactRecord()
        {
            var elements = WebContext.WebDriver.FindElements(By.CssSelector(SeleniumHelper.AutomationId("search_result_tile_LearnerContact")));  // Elements in result tile
            var resultCount = elements.Count;

            if (resultCount >= 1)
            {
                elements[0].Click();
            }
        }

        public AddressBookPopup ClickOnFirstStaffRecord()
        {
            var elements = WebContext.WebDriver.FindElements(By.CssSelector(SeleniumHelper.AutomationId("search_result_tile_Staff")));  // Elements in result tile
            var resultCount = elements.Count;

            if (resultCount >= 1)
            {
                elements[0].Click();
            }


            return new AddressBookPopup();
        }

        public int CheckForResultsAvailability(string textValue)
        {
            var elements = WebContext.WebDriver.FindElements(By.CssSelector(SeleniumHelper.AutomationId("PreferredListName_Learner")));  // Elements in result tile
            var resultCount = elements.Count;
            return resultCount;
        }

        public int CheckForStaffAvailability(string textValue)
        {
            var elements = WebContext.WebDriver.FindElements(By.CssSelector(SeleniumHelper.AutomationId("PreferredListName_Staff")));  // Elements in result tile
            var resultCount = elements.Count;
            return resultCount;
        }

        public void waitForPupilResultstoAppear()
        {
            WaitUntilDisplayed(AddressBookElements.PlaceHolderForResults);
        }

        public void waitForPupilContactsResultstoAppear()
        {
            WaitUntilDisplayed(AddressBookElements.PlaceHolderForResults);
        }

        public void waitForStaffResultstoAppear()
        {
            WaitUntilDisplayed(AddressBookElements.PlaceHolderForResults);
        }

        public bool GetClassYear()
        {
            var elements = WebContext.WebDriver.FindElements(By.CssSelector(SeleniumHelper.AutomationId("Learner_YearGroupClass")));
            var resultCount = elements.Count;

            if (resultCount >= 1)
            {
                String pupilClassYear = elements[0].Text;
                TestResultReporter.Log("<b>Class/Year Group of this pupil is   </b><b>" + pupilClassYear + "</b");
            }
            return (elements[0].Text != null);
        }

        public void Log(string message)
        {
            TestResultReporter.Log(message);
        }
    }
}
