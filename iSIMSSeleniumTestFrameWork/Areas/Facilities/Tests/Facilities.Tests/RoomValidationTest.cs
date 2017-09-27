using Facilities.Components.Common;
using Facilities.Components.FacilitiesPages;
using NUnit.Framework;
using OpenQA.Selenium;
using Selene.Support.Attributes;
using SharedComponents.BaseFolder;
using SharedComponents.CRUD;
using SharedComponents.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestSettings;
using WebDriverRunner.internals;
using WebDriverRunner.webdriver;

namespace Facilities.Tests
{
    class RoomValidationTest
    {
        #region Story ID :- 922 :- Validation Message "Room Short Name is required."
        [WebDriverTest(Enabled = true, Groups = new[] { "All", "P2" }, Browsers = new[] { BrowserDefaults.Chrome })]
        public void ValidationRoomShortName()
        {
            var schoolRoomPage = FacilitiesNavigation.NavigateToRoomPage();
            schoolRoomPage.CreateSchoolRoom();
            schoolRoomPage.EnterShortName("");
            schoolRoomPage.EnterLongName("Validation Test");
            schoolRoomPage.Save();
            IWebElement ValidationWarning = SeleniumHelper.Get(RoomElements.RoomValidationWarning);
            Assert.IsTrue(ValidationWarning.Displayed, "Validation Warning");
        }
        #endregion

        #region Story ID :- 922 :- Validation Message "Room Long Name is required."
        [WebDriverTest(Enabled = true, Groups = new[] { "All", "P2" }, Browsers = new[] { BrowserDefaults.Chrome })]
        public void ValidationRoomLongName()
        {
            var schoolRoomPage = FacilitiesNavigation.NavigateToRoomPage();
            schoolRoomPage.CreateSchoolRoom();
            schoolRoomPage.EnterShortName("V20");
            schoolRoomPage.EnterLongName("");
            schoolRoomPage.Save();
            IWebElement ValidationWarning = SeleniumHelper.Get(RoomElements.RoomValidationWarning);
            Assert.IsTrue(ValidationWarning.Displayed, "Validation Warning");
        }
        #endregion

        #region Story ID :- 922 :- Validation Message "Room Short Name already exists."
        [WebDriverTest(Enabled = true, Groups = new[] { "All", "P2" }, Browsers = new[] { BrowserDefaults.Chrome })]
        public void ValidationDuplicateRoomshortName()
        {
            var schoolRoomPage = FacilitiesNavigation.NavigateToRoomPage();
            schoolRoomPage.CreateSchoolRoom();
            schoolRoomPage.EnterShortName("DURM");
            schoolRoomPage.EnterLongName("Test for duplicate room");
            schoolRoomPage.Save();
            schoolRoomPage.WaitForCreateButttonToAppear();
            schoolRoomPage.CreateSchoolRoom();
            schoolRoomPage.WaitForCancelButttonToAppear();
            schoolRoomPage.EnterShortName("DURM");
            schoolRoomPage.EnterLongName("Test for duplicate");
            schoolRoomPage.Save();
            IWebElement ValidationWarning = SeleniumHelper.Get(RoomElements.RoomValidationWarning);
            Assert.IsTrue(ValidationWarning.Displayed, "Validation Warning");
            schoolRoomPage.ClickCancelButton();
            schoolRoomPage.ClickDontSaveButton();
            schoolRoomPage.EnterShortNameSearchPanel("DURM");
            schoolRoomPage.EnterLongNameSearchPanel("Test for duplicate room");
            schoolRoomPage.ClickSeachRoomButton();
            SearchResults.WaitForResults();
            schoolRoomPage.ClickSearchResults();
            schoolRoomPage.DeleteRoomRecord();
        }
        #endregion

        #region Story ID :- 922 :- Validation Message "Room Long Name already exists."
        [WebDriverTest(Enabled = true, Groups = new[] { "All", "P2" }, Browsers = new[] { BrowserDefaults.Chrome })]
        public void ValidationDuplicateRoomLongName()
        {
            var schoolRoomPage = FacilitiesNavigation.NavigateToRoomPage();
            schoolRoomPage.CreateSchoolRoom();
            schoolRoomPage.EnterShortName("TT1");
            schoolRoomPage.EnterLongName("DU Room Long Name");
            schoolRoomPage.Save();
            schoolRoomPage.WaitForCreateButttonToAppear();
            schoolRoomPage.CreateSchoolRoom();
            schoolRoomPage.WaitForCancelButttonToAppear();
            schoolRoomPage.EnterShortName("TT2");
            schoolRoomPage.EnterLongName("DU Room Long Name");
            schoolRoomPage.Save();
            IWebElement ValidationWarning = SeleniumHelper.Get(RoomElements.RoomValidationWarning);
            Assert.IsTrue(ValidationWarning.Displayed, "Validation Warning");
            schoolRoomPage.ClickCancelButton();
            schoolRoomPage.ClickDontSaveButton();
            schoolRoomPage.EnterShortNameSearchPanel("TT1");
            schoolRoomPage.EnterLongNameSearchPanel("DU Room Long Name");
            schoolRoomPage.ClickSeachRoomButton();
            SearchResults.WaitForResults();
            schoolRoomPage.ClickSearchResults();
            schoolRoomPage.DeleteRoomRecord();
        }
        #endregion

        #region Story ID :- 924 :- Area (in square metres) cannot more than 10000000.
        [WebDriverTest(Enabled = true, Groups = new[] { "All", "P2" }, Browsers = new[] { BrowserDefaults.Chrome })]
        public void ValidationAreaExceedLimit()
        {
            var schoolRoomPage = FacilitiesNavigation.NavigateToRoomPage();
            schoolRoomPage.CreateSchoolRoom();
            schoolRoomPage.EnterShortName("VA26");
            schoolRoomPage.EnterLongName("Area Exceed Limit");
            schoolRoomPage.EnterRoomArea("10000001");
            schoolRoomPage.Save();
            IWebElement ValidationWarning = SeleniumHelper.Get(RoomElements.RoomValidationWarning);
            Assert.IsTrue(ValidationWarning.Displayed, "Validation Warning");
        }
        #endregion

        #region Story ID :- 924 :- Area (in square metres) cannot be less than 0.
        [WebDriverTest(Enabled = true, Groups = new[] { "All", "P2" }, Browsers = new[] { BrowserDefaults.Chrome })]
        public void ValidationAreaBelowLimit()
        {
            var schoolRoomPage = FacilitiesNavigation.NavigateToRoomPage();
            schoolRoomPage.CreateSchoolRoom();
            schoolRoomPage.EnterShortName("VA26");
            schoolRoomPage.EnterLongName("Area Exceed Limit");
            schoolRoomPage.EnterRoomArea("-1");
            schoolRoomPage.Save();
            IWebElement ValidationWarning = SeleniumHelper.Get(RoomElements.RoomValidationWarning);
            Assert.IsTrue(ValidationWarning.Displayed, "Validation Warning");
        }
        #endregion

        #region Story ID :- 924 :- Maximum Group Size cannot more than 10000.
        [WebDriverTest(Enabled = true, Groups = new[] { "All", "P2" }, Browsers = new[] { BrowserDefaults.Chrome })]
        public void ValidationGroupSizeExceedLimit()
        {
            var schoolRoomPage = FacilitiesNavigation.NavigateToRoomPage();
            schoolRoomPage.CreateSchoolRoom();
            schoolRoomPage.EnterShortName("VA26");
            schoolRoomPage.EnterLongName("Group Exceed Limit");
            schoolRoomPage.EnterMaxaxGroupSize("10001");
            schoolRoomPage.Save();
            IWebElement ValidationWarning = SeleniumHelper.Get(RoomElements.RoomValidationWarning);
            Assert.IsTrue(ValidationWarning.Displayed, "Validation Warning");
        }
        #endregion

        #region Story ID :- 924 :- Maximum Group Size cannot be 0 or less than 0.
        [WebDriverTest(Enabled = true, Groups = new[] { "All" }, Browsers = new[] { BrowserDefaults.Chrome})]
        public void ValidationGroupSizeBelowLimit()
        {
            var schoolRoomPage = FacilitiesNavigation.NavigateToRoomPage();
            schoolRoomPage.CreateSchoolRoom();
            schoolRoomPage.EnterShortName("VA26");
            schoolRoomPage.EnterLongName("Group value 0");
            schoolRoomPage.EnterMaxaxGroupSize("0");
            schoolRoomPage.Save();
            IWebElement ValidationWarning = SeleniumHelper.Get(RoomElements.RoomValidationWarning);
            Assert.IsTrue(ValidationWarning.Displayed, "Validation Warning");
        }
        #endregion

        #region Story ID :- 924 :- Maximum Group Size cannot be 0 or less than 0.
        [WebDriverTest(Enabled = true, Groups = new[] { "All" }, Browsers = new[] { BrowserDefaults.Chrome})]
        public void ValidationGroupSizeBelowLimit0()
        {
            var schoolRoomPage = FacilitiesNavigation.NavigateToRoomPage();
            schoolRoomPage.CreateSchoolRoom();
            schoolRoomPage.EnterShortName("VA26");
            schoolRoomPage.EnterLongName("Group less than 0");
            schoolRoomPage.EnterMaxaxGroupSize("-1");
            schoolRoomPage.Save();
            IWebElement ValidationWarning = SeleniumHelper.Get(RoomElements.RoomValidationWarning);
            Assert.IsTrue(ValidationWarning.Displayed, "Validation Warning");
        }
        #endregion

        #region Story ID :- 1367 : AC: When a Room is created, is the Active field "True" by default?
        [WebDriverTest(Enabled = true, Groups = new[] { "All", "P2" }, Browsers = new[] { BrowserDefaults.Chrome })]
        public void ValidationActiveCheck()
        {
            var schoolRoomPage = FacilitiesNavigation.NavigateToRoomPage();
            schoolRoomPage.CreateSchoolRoom();
            schoolRoomPage.WaitForCancelButttonToAppear();
            var checkbox = WebContext.WebDriver.FindElement(RoomElements.CheckBoxActive);
            Assert.That(checkbox.GetAttribute("checked") != null);
        }
        #endregion

        #region Story ID :- 1367 : AC: When searching for a Room, can I choose to include Inactive Rooms? (Default False - i.e. by default Inactive Rooms are excluded.) 
        [WebDriverTest(Enabled = true, Groups = new[] { "All" }, Browsers = new[] { BrowserDefaults.Chrome })]
        public void ValidationInactiveCheck()
        {
            var schoolRoomPage = FacilitiesNavigation.NavigateToRoomPage();
            schoolRoomPage.CreateSchoolRoom();
            schoolRoomPage.EnterShortName("INA09");
            schoolRoomPage.EnterLongName("Test for inactive 09");
            schoolRoomPage.UncheckActiveCheckBox();
            var includeInactiveRoomCheckbox = WebContext.WebDriver.FindElement(RoomElements.IncludeInactiveRooms);
            Assert.That(includeInactiveRoomCheckbox.GetAttribute("checked") == null);
        }
        #endregion

        #region Story ID :- 923 : AC: Room Short name Max Field valiation
        [WebDriverTest(Enabled = true, Groups = new[] { "All" }, Browsers = new[] { BrowserDefaults.Chrome })]
        public void RoomShortNameMaxFieldLength()
        {
            RoomPage schoolRoomPage = FacilitiesNavigation.NavigateToRoomPage();
            schoolRoomPage.CreateSchoolRoom();
            schoolRoomPage.WaitForCancelButttonToAppear();
            Assert.IsTrue(schoolRoomPage.Shortname.GetAttribute("maxlength") == "10");
        }
        #endregion

        #region Story ID :- 923 : AC: Room Long name Max Field valiation
        [WebDriverTest(Enabled = true, Groups = new[] { "All" }, Browsers = new[] { BrowserDefaults.Chrome })]
        public void RoomLongNameMaxFieldLength()
        {
            RoomPage schoolRoomPage = FacilitiesNavigation.NavigateToRoomPage();
            schoolRoomPage.CreateSchoolRoom();
            schoolRoomPage.WaitForCancelButttonToAppear();
            Assert.IsTrue(schoolRoomPage.Longname.GetAttribute("maxlength") == "32");
        }
        #endregion
    }
}
