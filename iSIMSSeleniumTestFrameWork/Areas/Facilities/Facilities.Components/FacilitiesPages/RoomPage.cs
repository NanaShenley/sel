using System;
using Facilities.Components.Common;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
//using SharedComponents.Helpers;
using TestSettings;
using WebDriverRunner.webdriver;
using System.Threading;
using POM.Helper;
namespace Facilities.Components.FacilitiesPages
{
    public class RoomPage : BaseFacilitiesPage
    {

#pragma warning disable 0649
        [FindsBy(How = How.Id, Using = "editableData")]
        private readonly IWebElement _main;
        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='add_button']")]
        private readonly IWebElement _create;
        
        [FindsBy(How = How.CssSelector, Using = ("[data-automation-id='search_criteria']"))]
        private readonly IWebElement _search;


#pragma warning restore 0649

        public IWebElement Shortname
        {
            get
            {
                return _main.FindElementSafe(By.Name("ShortName"));
            }
        }

        public IWebElement Longname
        {
            get
            {
                return _main.FindElementSafe(By.Name("LongName"));
            }
        }

        private IWebElement Searchshortname
        {
            get
            {
                return _search.FindElement(By.Name("ShortName"));
            }
        }
        private IWebElement Searchlongname
        {
            get
            {
                return _search.FindElementSafe(By.Name("LongName"));
            }
        }

        private IWebElement ActiveCheckBox
        {
            get
            {
                return _main.FindElementSafe(By.Name("IsActive"));
            }
        }

        public RoomPage()
        {
            PageFactory.InitElements(WebContext.WebDriver, this);
        }

        public void CreateSchoolRoom()
        {
                // _search.WaitUntilState(ElementState.Displayed);
            _create.ClickByJS();
             Wait.WaitForElementReady(By.Name("LongName"));
        }
        public void WaitForCreateButttonToAppear()
        {
            WaitUntilDisplayed(RoomElements.CreateRoom);
        }
        public void WaitForCancelButttonToAppear()
        {
            WaitUntilDisplayed(RoomElements.CancelButton);
        }

        public void EnterShortName(string roomshortname)
        {
            Shortname.SendKeys(roomshortname);
        }

        public void EnterLongName(string roomlongname)
        {
            Longname.SetText(roomlongname);
        }

        public void EnterTelephoneNo(string roomTelephonenumber)
        {
            WaitForAndSetValue(BrowserDefaults.TimeOut, RoomElements.RoomTelephonenumber, roomTelephonenumber, true);
        }
        public void EnterRoomArea(string roomarea)
        {
            WaitForAndSetValue(BrowserDefaults.TimeOut, RoomElements.Roomarea, roomarea, true);
        }
        public void EnterMaxaxGroupSize(string maxgroupsize)
        {
            WaitForAndSetValue(BrowserDefaults.TimeOut, RoomElements.Roommaxgroupsize, maxgroupsize, true);
        }
        public void UncheckActiveCheckBox()
        {
            ActiveCheckBox.Click();
        }

        //***************************************************Operation for Edit Room Panel.***************************************************

        public void ReEnterShortName(string roomshortname)
        {
            Shortname.Clear();
            Shortname.SendKeys(roomshortname);
        }

        public void ReEnterLongName(string roomlongname)
        {
            Longname.Clear();
            Longname.SetText(roomlongname);
        }

        public void ReEnterTelephoneNo(string roomTelephonenumber)
        {
            WaitForAndSetValue(BrowserDefaults.TimeOut, RoomElements.RoomTelephonenumber, roomTelephonenumber, true);
        }
        public void ReEnterRoomArea(string roomarea)
        {
            WaitForAndSetValue(BrowserDefaults.TimeOut, RoomElements.Roomarea, roomarea, true);
        }
        public void ReEnterMaxaxGroupSize(string maxgroupsize)
        {
            WaitForAndSetValue(BrowserDefaults.TimeOut, RoomElements.Roommaxgroupsize, maxgroupsize, true);
        }
        //Delete Room

        public void DeleteRoomRecord()
        {
            WaitForAndClick(BrowserDefaults.TimeOut, RoomElements.DeleteButton);
            WaitForAndClick(BrowserDefaults.TimeOut, RoomElements.ContinueWithDelete);
        }

        //***************************************************Operation for Search Room Panel.***************************************************

        public void EnterShortNameSearchPanel(string roomshortnamesearch)
        {
            Searchshortname.SendKeys(roomshortnamesearch);
        }
        public void EnterLongNameSearchPanel(string roomlongnamesearch)
        {
            Searchlongname.SendKeys(roomlongnamesearch);
        }
        public void ClickSeachRoomButton()
        {
            WaitForAndClick(BrowserDefaults.TimeOut, RoomElements.Roomsearchbutton);
        }
        public void ClickCancelButton()
        {
            WaitForAndClick(BrowserDefaults.TimeOut, RoomElements.CancelButton);
        }
        public void ClickDontSaveButton()
        {
            WaitForAndClick(BrowserDefaults.TimeOut, RoomElements.Dntsavebutton);
        }
        public void ClickSearchResults()
        {
            var searchResult = WebContext.WebDriver.FindElement(RoomElements.SearchResults);
            searchResult.Click();
        }

        [Obsolete("Use HasConfirmedSave(string expectedSaveMessage) instead")]
        public bool HasConfirmedSave()
        {
            return false;
        }
    }
}
