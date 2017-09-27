using AddressBook.Components;
using AddressBook.Components.Pages;
using AddressBook.Test;
using NUnit.Framework;
using OpenQA.Selenium;
using Selene.Support.Attributes;
using SeSugar;
using SeSugar.Automation;
using SeSugar.Data;
using SharedComponents.BaseFolder;
using SharedComponents.Helpers;
using SharedComponents.HomePages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TestSettings;
using WebDriverRunner.internals;
using Pupil.Data;
using OpenQA.Selenium.Interactions;
using WebDriverRunner.webdriver;
using System.Globalization;
using Facilities.Components.FacilitiesPages;

namespace AddressBook.CurrentPupils.Test
{
    public class LinksForCurrentPupils
    {
   

        #region Story 1238- Verify Link to Pupil Record
        [WebDriverTest(Enabled = true, Groups = new[] { AddressBookTestGroups.CurrentPupilQuickSearch }, Browsers = new[] { BrowserDefaults.Chrome })]
        [Variant(Variant.NorthernIrelandStatePrimary)]
        public void LinkToPupilRecordFromPupilInfo()
        {
            #region Data Pupil Setup

            #region Create a new pupil, so that it can be attached later to the newly created contact1

            AutomationSugar.Log("Data Creation started");
            Guid pupilId = Guid.NewGuid();
            DataPackage dataPackage = this.BuildDataPackage();
            var pupilSurname = Utilities.GenerateRandomString(10, "LinkForPupil");
            var pupilForename = Utilities.GenerateRandomString(10, "LinkForPupil" + Thread.CurrentThread.ManagedThreadId);
            dataPackage.AddBasicLearner(pupilId, pupilSurname, pupilForename, new DateTime(2005, 10, 01), new DateTime(2012, 08, 01));

            #endregion
            DataSetup DataSetPupil = new DataSetup(purgeBeforeInsert: false, purgeAfterTest: true, packages: dataPackage);


            #endregion

            AddressBookSearchPage searchBox = QuickSearch.QuickSearchNavigation();
            searchBox.ClearText();
            searchBox.EnterSearchTextForPupils(pupilForename);
            AddressBookPopup popup = searchBox.ClickOnFirstPupilRecord();
            popup.ClickPupilDetailsLink();
            bool value = SeleniumHelper.Get(AddressBookElements.OpenedPupilRecordTab).Displayed;
            Assert.IsTrue(value);
        }
        #endregion



        #region Story 1238- Prompted to save changes to any dirty Pupil Record or Pupil Log screens currently open
        [WebDriverTest(Enabled = true, Groups = new[] { AddressBookTestGroups.CurrentPupilQuickSearch }, Browsers = new[] { BrowserDefaults.Chrome })]
        [Variant(Variant.NorthernIrelandStatePrimary)]
        public void PromptToSaveChangesOnNavigationToOtherScreen()
        {
            #region Data Pupil Setup

            #region Create a new pupil, so that it can be attached later to the newly created contact1

            AutomationSugar.Log("Data Creation started");
            Guid pupilId = Guid.NewGuid();
            DataPackage dataPackage = this.BuildDataPackage();
            var pupilSurname = Utilities.GenerateRandomString(10, "LinkForPupil2");
            var pupilForename = Utilities.GenerateRandomString(10, "LinkForPupil2" + Thread.CurrentThread.ManagedThreadId);
            dataPackage.AddBasicLearner(pupilId, pupilSurname, pupilForename, new DateTime(2005, 10, 01), new DateTime(2012, 08, 01));

            #endregion
            DataSetup DataSetPupil = new DataSetup(purgeBeforeInsert: false, purgeAfterTest: true, packages: dataPackage);


            #endregion

            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);
            NavigateToOtherScreen.GoToRoomScreen();
            RoomPage SchoolRoomPage = new RoomPage();
            SchoolRoomPage.CreateSchoolRoom();
            SchoolRoomPage.EnterShortName("Rm60");
            AddressBookSearchPage searchBox = new AddressBookSearchPage();
            searchBox.ClearText();
            searchBox.EnterSearchTextForPupils(pupilForename);
            AddressBookPopup popup = searchBox.ClickOnFirstPupilRecord();
            popup.ClickPupilDetailsLink();
            popup.WaitForConfirmationDialogToAppear();
            WebContext.Screenshot();
            bool DialogDisplayed = SeleniumHelper.Get("save_continue_commit_dialog").Displayed;
            Assert.IsTrue(DialogDisplayed, "Failure to popup confirmation Dialog");
        }
        #endregion
    }
}
