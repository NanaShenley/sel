using AddressBook.Components;
using NUnit.Framework;
using SharedComponents.Helpers;
using TestSettings;
using WebDriverRunner.internals;
using AddressBook.Test;
using SharedComponents.HomePages;
using Selene.Support.Attributes;
using SeSugar.Automation;
using System;
using SeSugar;
using System.Threading;
using SeSugar.Data;
using Pupil.Data;

namespace AddressBook.CurrentPupil.Test
{
    public class AssociatedPupilContacts
    {

        #region Verify Basic Information of Associated Pupil contacts
        [WebDriverTest(Enabled = true, Groups = new[] { AddressBookTestGroups.PupilContactsQuickSearch }, Browsers = new[] { BrowserDefaults.Chrome })]
         [Variant(Variant.NorthernIrelandStatePrimary)] 
 
        public void DisplayPupilContactAssociatedToPupilDetails()
        {
            #region Data Pupil Setup

            #region Create a new pupil, so that it can be attached later to the newly created contact1
          
            AutomationSugar.Log("Data Creation started");
            Guid pupilId = Guid.NewGuid();
            DataPackage dataPackage = this.BuildDataPackage();
            var pupilSurname = Utilities.GenerateRandomString(10, "Communications_Surname");
            var pupilForename = Utilities.GenerateRandomString(10, "Communications_Forename" + Thread.CurrentThread.ManagedThreadId);
            dataPackage.AddBasicLearner(pupilId, pupilSurname, pupilForename, new DateTime(2005, 01, 01), new DateTime(2012, 09, 01));
         
            #endregion

            #region Pre-Condition: Create new contact 1 and refer to pupil

            //Arrange
            AutomationSugar.Log("***Pre-Condition: Create new contact1 and refer to pupil");
            Guid pupilContactId1 = Guid.NewGuid();
            Guid pupilContactRelationshipId1 = Guid.NewGuid();
            //Add pupil contact
            var contactSurname1 = Utilities.GenerateRandomString(10, "SeleniumPupilContact1_Surname" + Thread.CurrentThread.ManagedThreadId);
            var contactForename1 = Utilities.GenerateRandomString(10, "SeleniumPupilContact1_Forename" + Thread.CurrentThread.ManagedThreadId);
            var titleContact1 = "Mrs";
            dataPackage.AddPupilContact(pupilContactId1, contactSurname1, contactForename1);
            dataPackage.AddPupilContactRelationship(pupilContactRelationshipId1, pupilId, pupilContactId1);
            #endregion


            #endregion

            DataSetup DataSetPupil = new DataSetup(purgeBeforeInsert: false, purgeAfterTest: true, packages: dataPackage);

            AddressBookSearchPage searchBox = QuickSearch.QuickSearchNavigation();
            searchBox.ClearText();
            searchBox.EnterSearchTextForPupils(pupilForename);
            AddressBookPopup popup = searchBox.ClickOnFirstPupilRecord();
            Assert.Greater(popup.HaveAssociatedPupil(), 1);
        }
        #endregion
    }
}

