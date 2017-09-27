using AddressBook.Components;
using AddressBook.Components.Pages;
using AddressBook.Test;
using NUnit.Framework;
using Selene.Support.Attributes;
using SharedComponents.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestSettings;
using WebDriverRunner.internals;
using WebDriverRunner.webdriver;
using SharedComponents.HomePages;
using OpenQA.Selenium;
using SeSugar;
using SeSugar.Automation;
using SeSugar.Data;
using SharedComponents.BaseFolder;

using Pupil.Data;
using System.Threading;

namespace AddressBook.PupilContact.Test
{
    public class DisplayBasicInfoPupilContacts
    {

        #region Story 5653- Verify Basic Information of Pupil Contacts
        [WebDriverTest(Enabled = true, Groups = new[] { AddressBookTestGroups.PupilContactsQuickSearch }, Browsers = new[] { BrowserDefaults.Chrome })]
        public void DisplayPupilContactBasicDetails()
        {
            #region Data Pupil Setup

            #region Create a new pupil, so that it can be attached later to the newly created contact1

            AutomationSugar.Log("Data Creation started");
            Guid pupilId = Guid.NewGuid();
            DataPackage dataPackage = this.BuildDataPackage();
            var pupilSurname = Utilities.GenerateRandomString(10, "CurrentPUPIL");
            var pupilForename = Utilities.GenerateRandomString(10, "CurrentPUPIL" + Thread.CurrentThread.ManagedThreadId);
            dataPackage.AddBasicLearner(pupilId, pupilSurname, pupilForename, new DateTime(2005, 10, 01), new DateTime(2012, 08, 01));

            #endregion

            #region Pre-Condition: Create new contact 1 and refer to pupil

            //Arrange
            AutomationSugar.Log("***Pre-Condition: Create new contact1 and refer to pupil");
            Guid pupilContactId1 = Guid.NewGuid();
            Guid pupilContactRelationshipId1 = Guid.NewGuid();
            //Add pupil contact
            var contactSurname1 = Utilities.GenerateRandomString(10, "PupilContact_WithBasicDetails" + Thread.CurrentThread.ManagedThreadId);
            var contactForename1 = Utilities.GenerateRandomString(10, "PupilContact_WithBasicDetails" + Thread.CurrentThread.ManagedThreadId);
            dataPackage.AddPupilContact(pupilContactId1, contactSurname1, contactForename1);
            dataPackage.AddPupilContactRelationship(pupilContactRelationshipId1, pupilId, pupilContactId1);
            #endregion


            DataSetup DataSetPupil = new DataSetup(purgeBeforeInsert: false, purgeAfterTest: true, packages: dataPackage);


            #endregion

            
            AddressBookSearchPage searchBox = QuickSearch.QuickSearchNavigation();
            searchBox.ClearText();
            searchBox.EnterSearchTextForPupilContacts(contactForename1);
            searchBox.ClickOnFirstPupilContactRecord();
            AddressBookPopup popup = new AddressBookPopup();
            Assert.IsNotNull(AddressBookElements.DisplayPopup);
            Assert.IsTrue(popup.GetPupilContactBasicDetails());
        }
        #endregion
    }
}
