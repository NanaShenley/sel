using AddressBook.Components;
using AddressBook.Components.Pages;

using NUnit.Framework;
using SharedComponents.Helpers;
using TestSettings;
using WebDriverRunner.internals;
using WebDriverRunner.webdriver;
using AddressBook.Test;
using SharedComponents.HomePages;
using Selene.Support.Attributes;
using OpenQA.Selenium;
using SeSugar;
using SeSugar.Automation;
using SeSugar.Data;
using SharedComponents.BaseFolder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Pupil.Data;
using Facilities.Components.FacilitiesPages;

namespace AddressBook.PupilContact.Test
{
    public class LinksForPupilContacts
    {


        #region Story 5932- Verify Link to Pupil Contact record
        [WebDriverTest(Enabled = true, Groups = new[] { AddressBookTestGroups.PupilContactsQuickSearch }, Browsers = new[] { BrowserDefaults.Chrome })]
        public void LinkPupilContactRecord()
        {

            #region Data Pupil Setup

            #region Create a new pupil, so that it can be attached later to the newly created contact1

            AutomationSugar.Log("Data Creation started");
            Guid pupilId = Guid.NewGuid();
            DataPackage dataPackage = this.BuildDataPackage();
            var pupilSurname = Utilities.GenerateRandomString(10, "TestData_Pupil");
            var pupilForename = Utilities.GenerateRandomString(10, "TestData_Pupil" + Thread.CurrentThread.ManagedThreadId);
            dataPackage.AddBasicLearner(pupilId, pupilSurname, pupilForename, new DateTime(2005, 10, 01), new DateTime(2012, 08, 01));

            #endregion

            #region Pre-Condition: Create new contact 1 and refer to pupil

            //Arrange
            AutomationSugar.Log("***Pre-Condition: Create new contact1 and refer to pupil");
            Guid pupilContactId1 = Guid.NewGuid();
            Guid pupilContactRelationshipId1 = Guid.NewGuid();
            //Add pupil contact
            var contactSurname1 = Utilities.GenerateRandomString(10, "Steve" + Thread.CurrentThread.ManagedThreadId);
            var contactForename1 = Utilities.GenerateRandomString(10, "Jonathan" + Thread.CurrentThread.ManagedThreadId);
            dataPackage.AddPupilContact(pupilContactId1, contactSurname1, contactForename1);
            dataPackage.AddPupilContactRelationship(pupilContactRelationshipId1, pupilId, pupilContactId1);
            #endregion


            DataSetup DataSetPupil = new DataSetup(purgeBeforeInsert: false, purgeAfterTest: true, packages: dataPackage);


            #endregion
            
            AddressBookSearchPage searchBox = QuickSearch.QuickSearchNavigation();
            searchBox.ClearText();
            searchBox.EnterSearchTextForPupilContacts(contactSurname1);
            searchBox.ClickOnFirstPupilContactRecord();
            AddressBookPopup popup = new AddressBookPopup();
            popup.ClickPupilContactsDetailsLink();               //Bug 30498
            Assert.IsTrue(SeleniumHelper.Get(AddressBookElements.OpenedPupilContactTab).IsExist());                          
        }
        #endregion

        #region Story 5932- Prompted to save changes to any dirty Pupil Contact Record or Pupil Log screens currently open
        [WebDriverTest(Enabled = true, Groups = new[] { AddressBookTestGroups.PupilContactsQuickSearch }, Browsers = new[] { BrowserDefaults.Chrome })]
        public void PromptSaveChangesPupilContact()
        {
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);
            NavigateToOtherScreen.GoToRoomScreen();
            RoomPage SchoolRoomPage = new RoomPage();
            SchoolRoomPage.CreateSchoolRoom();
            SchoolRoomPage.EnterShortName("Rm60");
              AddressBookSearchPage searchBox = new AddressBookSearchPage();
            searchBox.ClearText();
            searchBox.EnterSearchTextForPupilContacts("Jonathan");
            searchBox.ClickOnFirstPupilContactRecord();    //Bug 30498
            AddressBookPopup popup = new AddressBookPopup();
            popup.ClickPupilContactsDetailsLink();
            popup.WaitForConfirmationDialogToAppear();
            WebContext.Screenshot();
            bool DialogDisplayed = SeleniumHelper.Get("save_continue_commit_dialog").Displayed;
            Assert.IsTrue(DialogDisplayed, "Failure to popup confirmation Dialog");
        }
        #endregion

      
    }
}
        