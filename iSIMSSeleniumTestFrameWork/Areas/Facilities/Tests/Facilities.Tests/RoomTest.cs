using Facilities.Components.Common;
using Facilities.Components.FacilitiesPages;
using Facilities.Components.FacilitiesTestData;
using NUnit.Framework;
using SharedComponents.CRUD;
using System;
using TestSettings;
using WebDriverRunner.internals;
using WebDriverRunner.webdriver;
using OpenQA.Selenium;
using SharedComponents.BaseFolder;
using SharedComponents.Helpers;
using Selene.Support.Attributes;


namespace Facilities.Tests
{
    public class RoomTest
    {
        //private Guid _roomId;

        //[BeforeSuiteWebTest(Groups = new[] { "Room", "Facilities" }, Browsers = new[] { BrowserDefaults.Chrome })]
        //public void BeforeSuite()
        //{
        //    //TODO:
        //    //List<Guid> ids = StaffDetail.AddStaff("Test", "Staff", "06/09/2003");
        //    //_roomId = ids[0];
        //    var ids = RoomTestData.AddRoom("tr1", "Test Room 1");
        //    _roomId = ids[0];
        //}

        //[AfterSuiteWebTest(Groups = new[] { "Room", "Facilities" }, Browsers = new[] { BrowserDefaults.Chrome })]
        //public void AfterSuite()
        //{
        //    //TODO:
        //    //SeleniumHelper.Login();
        //    //StaffDetail.DeleteStaff(_staffID);
        //    RoomTestData.DeleteRoom(_roomId);
        //}


        #region Story ID :- 923 Create Room Tests
        [WebDriverTest(Enabled = true, Groups = new[] { "All", "P2" }, Browsers = new[] { BrowserDefaults.Chrome })]
        [Variant(Variant.NorthernIrelandStatePrimary)] 
        public void CreateRoom()
        {
            var schoolRoomPage = FacilitiesNavigation.NavigateToRoomPage();
            schoolRoomPage.CreateSchoolRoom();
            schoolRoomPage.EnterShortName("CRM22");
            schoolRoomPage.EnterLongName("Test for Create Room 22");
            schoolRoomPage.EnterTelephoneNo("9527725936");
            schoolRoomPage.EnterRoomArea("10000");
            schoolRoomPage.EnterMaxaxGroupSize("10000");
            schoolRoomPage.Save();
            Assert.IsTrue(schoolRoomPage.HasConfirmedSave("school room record saved"));
            //Currently deleting the created room here itself in this test.
            SearchAndClickRoom(schoolRoomPage, "CRM22", "Test for Create Room 22");
            schoolRoomPage.DeleteRoomRecord();
        }
        #endregion

        #region Story ID :- 922 Search Room Tests by entering both the parameter Room Short Name and Room Long Name(Combination).
        [WebDriverTest(Enabled = true, Groups = new[] { "All", "P2" }, Browsers = new[] { BrowserDefaults.Chrome })]
        public void SearchRoom()
        {
            var schoolRoomPage = FacilitiesNavigation.NavigateToRoomPage();
            #region CREATE THIS USING WEB SERVICES
            //CREATE ROOM TO SEARCH
            schoolRoomPage.CreateSchoolRoom(); 
            schoolRoomPage.EnterShortName("SRM50");
            schoolRoomPage.EnterLongName("Test for Search Room 50");
            schoolRoomPage.EnterTelephoneNo("9527725936");
            schoolRoomPage.EnterRoomArea("10000");
            schoolRoomPage.EnterMaxaxGroupSize("10000");
            schoolRoomPage.Save();
            Assert.IsTrue(schoolRoomPage.HasConfirmedSave("School room record saved"));
            #endregion
            //Actual test i.e. Search the room created by the Web Service
            schoolRoomPage.EnterShortNameSearchPanel("SRM50");
            schoolRoomPage.EnterLongNameSearchPanel("Test for Search Room 50");
            schoolRoomPage.ClickSeachRoomButton();
            SearchResults.WaitForResults();
            Assert.IsTrue(SearchResults.HasResults(1));
            //Currently deleting the created room here itself in this test.
            schoolRoomPage.DeleteRoomRecord();
        }
        #endregion

        #region Story ID :- 922 Search Room Tests by entering parameter Room Short Name only.
        [WebDriverTest(Enabled = true, Groups = new[] { "All", "P2" }, Browsers = new[] { BrowserDefaults.Chrome })]
       public void SearchRoomByShortName()
       {
           var schoolRoomPage = FacilitiesNavigation.NavigateToRoomPage();
           #region CREATE THIS USING WEB SERVICES
           //CREATE ROOM TO SEARCH
           schoolRoomPage.CreateSchoolRoom();
           schoolRoomPage.EnterShortName("SRT00");
           schoolRoomPage.EnterLongName("Search RM Short Name 00");
           schoolRoomPage.Save();
           Assert.IsTrue(schoolRoomPage.HasConfirmedSave("School room record saved"));
           #endregion
           //Actual test i.e. Search the room by Room Short Name by created by the Web Service
           schoolRoomPage.EnterShortNameSearchPanel("SRT00");
           schoolRoomPage.ClickSeachRoomButton();
           SearchResults.WaitForResults();
           Assert.IsTrue(SearchResults.HasResults(1));
           //Currently deleting the created room here itself in this test.
           schoolRoomPage.DeleteRoomRecord();

       }
       #endregion

        #region Story ID :- 922 Search Room Tests by entering parameter Room Long Name only.
        [WebDriverTest(Enabled = true, Groups = new[] { "All", "P2" }, Browsers = new[] { BrowserDefaults.Chrome })]
      public void SearchRoomByLongName()
      {
          var schoolRoomPage = FacilitiesNavigation.NavigateToRoomPage();
          #region CREATE THIS USING WEB SERVICES
          //CREATE ROOM TO SEARCH
          schoolRoomPage.CreateSchoolRoom();
          schoolRoomPage.EnterShortName("SRT23");
          schoolRoomPage.EnterLongName("Search RM Long Name 00");
          schoolRoomPage.Save();
          Assert.IsTrue(schoolRoomPage.HasConfirmedSave("School room record saved"));
          #endregion
          //Actual test i.e. Search the room by Room Long Name by created by the Web Service
          schoolRoomPage.EnterLongNameSearchPanel("Search RM Long Name 00");
          schoolRoomPage.ClickSeachRoomButton();
          SearchResults.WaitForResults();
          Assert.IsTrue(SearchResults.HasResults(1));
          //Currently deleting the created room here itself in this test.
          schoolRoomPage.DeleteRoomRecord();

      }
        #endregion

        #region Story ID :- 923 Edit Room Test.
        [WebDriverTest(Enabled = true, Groups = new[] { "All" , "P2" }, Browsers = new[] { BrowserDefaults.Chrome })]
        public void EditRoom()
        {
            var schoolRoomPage = FacilitiesNavigation.NavigateToRoomPage();
            #region CREATE THIS USING WEB SERVICES
            schoolRoomPage.CreateSchoolRoom();
            schoolRoomPage.EnterShortName("Rm60");
            schoolRoomPage.EnterLongName("Test for Room 60");
            schoolRoomPage.EnterTelephoneNo("9527725936");
            schoolRoomPage.EnterRoomArea("10000");
            schoolRoomPage.EnterMaxaxGroupSize("10000");
            schoolRoomPage.Save();
            Assert.IsTrue(schoolRoomPage.HasConfirmedSave("School room record saved"));
            #endregion
            //Search room to edit
            SearchAndClickRoom(schoolRoomPage, "RM60", "Test for Room 60");
             //SearchResults.WaitForResults();
            ////Perform the actual test here i.e. Edit searched room
            schoolRoomPage.ReEnterShortName("ERM61");
            schoolRoomPage.ReEnterLongName("Test for Edit Room 61");
            schoolRoomPage.ReEnterTelephoneNo("9999999999");
            schoolRoomPage.ReEnterRoomArea("99999");
            schoolRoomPage.ReEnterMaxaxGroupSize("9999");
            schoolRoomPage.Save();
            Assert.IsTrue(schoolRoomPage.HasConfirmedSave("School room record saved"));
            //Currently deleting the created room here itself in this test.
            schoolRoomPage.DeleteRoomRecord();

        }

        #endregion

        #region Story ID :- 1370 Delete Room Test.
        [WebDriverTest(Enabled = true, Groups = new[] { "All" , "P2" }, Browsers = new[] { BrowserDefaults.Chrome })]
        public void DeleteRoom()
        {
            var schoolRoomPage = FacilitiesNavigation.NavigateToRoomPage();

            #region CREATE THIS USING WEB SERVICES
            schoolRoomPage.CreateSchoolRoom();
            schoolRoomPage.EnterShortName("DL88");
            schoolRoomPage.EnterLongName("Test for Delete Room 88");
            schoolRoomPage.EnterTelephoneNo("9527725936");
            schoolRoomPage.EnterRoomArea("10000");
            schoolRoomPage.EnterMaxaxGroupSize("10000");
            schoolRoomPage.Save();
            Assert.IsTrue(schoolRoomPage.HasConfirmedSave("School room record saved"));
            #endregion
            schoolRoomPage.DeleteRoomRecord();
            schoolRoomPage.EnterShortNameSearchPanel("DL88");
            schoolRoomPage.EnterLongNameSearchPanel("Test for Delete Room 88");
            schoolRoomPage.ClickSeachRoomButton();
            Assert.IsTrue(SearchResults.HasResults(0));
        }
        #endregion        

        #region Story ID :- 1367 Can I mark a Room as "Active: False"?
        [WebDriverTest(Enabled = true, Groups = new[] { "All", "P2" }, Browsers = new[] { BrowserDefaults.Chrome })]
        public void CreateInactiveRoom()
        {
            var schoolRoomPage = FacilitiesNavigation.NavigateToRoomPage();
            schoolRoomPage.CreateSchoolRoom();
            schoolRoomPage.EnterShortName("INA09");
            schoolRoomPage.EnterLongName("Test for inactive 09");
            schoolRoomPage.UncheckActiveCheckBox();
            var includeInactiveRoomCheckbox = WebContext.WebDriver.FindElement(RoomElements.IncludeInactiveRooms);
            Assert.That(includeInactiveRoomCheckbox.GetAttribute("checked") == null);
            schoolRoomPage.Save();
            Assert.IsTrue(schoolRoomPage.HasConfirmedSave("school room record saved"));
            //Currently deleting the created room here itself in this test.
            schoolRoomPage.DeleteRoomRecord();
        }
        #endregion

        #region Utilities
        private void SearchAndClickRoom(RoomPage roomPage, string shortRoomName, string longRoomName)
        {
            TestResultReporter.Log("Searching room >> " + shortRoomName);

            roomPage.EnterShortNameSearchPanel(shortRoomName);
            roomPage.EnterLongNameSearchPanel(longRoomName);
            roomPage.ClickSeachRoomButton();
            SearchResults.WaitForResults();
            Assert.IsTrue(SearchResults.HasResults(1));
            TestResultReporter.Log(string.Format("Room '{0}' FOUND!!", shortRoomName));

            roomPage.ClickSearchResults();
            SearchResults.WaitForResults();
            TestResultReporter.Log(string.Format("Opened Room '{0}'", shortRoomName));
        }

        private void AddRoom()
        {
            //schoolRoomPage.CreateSchoolRoom();
            //schoolRoomPage.EnterShortName("SRM50");
            //schoolRoomPage.EnterLongName("Test for Search Room 50");
            //schoolRoomPage.EnterTelephoneNo("9527725936");
            //schoolRoomPage.EnterRoomArea("10000");
            //schoolRoomPage.EnterMaxaxGroupSize("10000");
            //schoolRoomPage.Save();
            //Assert.IsTrue(schoolRoomPage.HasConfirmedSave("School room record saved"));
        }
        #endregion
    

    }
}

    