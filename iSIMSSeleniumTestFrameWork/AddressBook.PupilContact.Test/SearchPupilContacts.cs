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

namespace AddressBook.PupilContact.Test
{
    public class SearchPupilContacts
    {

        #region Data Provider for Pupil Contacts Name options(Surname,Forename,partial or full name)
        public List<object[]> OtherPupilContacts()
        {
            var results = new List<object[]>
            {
                new object[] {"th"},
                new object[] {"Numerone , Thorne"},
                new object[] {"Thorne"},
                new object[] {"Nu"}
            };
            return results;
        }
        #endregion

        #region Story 5650- Search for Pupil Contact by Name(Verify if results appear in less than 2 sec)
        [WebDriverTest(DataProvider = "OtherPupilContacts", Enabled = true, Groups = new[] { AddressBookTestGroups.PupilContactsQuickSearch }, Browsers = new[] { BrowserDefaults.Chrome })]
        public void SearchPupilContactByName(string textForSearch)
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
            var contactSurname1 = Utilities.GenerateRandomString(10, "Thorne" + Thread.CurrentThread.ManagedThreadId);
            var contactForename1 = Utilities.GenerateRandomString(10, "Numerone" + Thread.CurrentThread.ManagedThreadId);
            dataPackage.AddPupilContact(pupilContactId1, contactSurname1, contactForename1);
            dataPackage.AddPupilContactRelationship(pupilContactRelationshipId1, pupilId, pupilContactId1);
            #endregion

       
            DataSetup DataSetPupil = new DataSetup(purgeBeforeInsert: false, purgeAfterTest: true, packages: dataPackage);


            #endregion
            
            AddressBookSearchPage searchBox = QuickSearch.QuickSearchNavigation();
            searchBox.EnterSearchTextForPupilContacts(textForSearch);
            double millisecs = searchBox.SearchTimeInMillisecs;
            searchBox.Log("Results fetched in " + millisecs + " milliseconds");
            Assert.Less(millisecs, AddressBookConstants.MaxAcceptableSearchTimeInMillisecs);
        }
        #endregion
    }
}
