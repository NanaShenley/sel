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

namespace AddressBook.CurrentPupil.Test
{
    public class SearchCurrentPupil
    {
        private readonly string textForSearch = "ad";
        // private Guid _learnerID;



        //Use of Data Provider to Pass more than one Parameter

        //TODO: Use of Test suite for Test data creation
        #region Data Provider for Current Pupil Name options(Legal Surname,Legal Forename,Preferred Surname,Preferred Forename,partial or full name)
        public List<object[]> OtherPupils()
        {
            var res = new List<object[]>
            {
               new object[] {"TestData_Pupil"},
                new object[] {"TestData_Pupil TestData_Pupil"},
                new object[] {"Forename"},
                new object[] {"TestData"}
               
            };
            return res;
        }
        #endregion

        [WebDriverTest(DataProvider = "OtherPupils", Enabled = true, Groups = new[] { AddressBookTestGroups.PupilContactsQuickSearch }, Browsers = new[] { BrowserDefaults.Chrome })]
        #region Story 1232- Search for Current Pupil by Name(Verify if results appear in less than 2 sec)
        public void SearchCurrentPupilByName(string textForSearch)
        {
            #region Data Pupil Setup

            #region Create a new pupil, so that it can be attached later to the newly created contact1

            AutomationSugar.Log("Data Creation started");
            Guid pupilId = Guid.NewGuid();
            DataPackage dataPackage = this.BuildDataPackage();
            var pupilSurname = Utilities.GenerateRandomString(10, "TestData_Pupil");
            var pupilForename = Utilities.GenerateRandomString(10, "Forename_TestData_Pupil" + Thread.CurrentThread.ManagedThreadId);
            dataPackage.AddBasicLearner(pupilId, pupilSurname, pupilForename, new DateTime(2005, 10, 01), new DateTime(2012, 08, 01));

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

        #region Story 1232- Verify if Navigation using Down keys is possible
        public static bool KeyDownNavigationOnPupilResults(SeleniumHelper.iSIMSUserType user)
        {
            AddressBookSearchPage searchBox = QuickSearch.QuickSearchNavigationByUserType(user);
            SearchResultTile srt = searchBox.EnterSearchTextForPupils("ad");
            int resultCount = srt.tileCount();
            bool traversed = false;
            Actions action = new Actions(WebContext.WebDriver);
            action.SendKeys(Keys.Tab).Perform();
            action.SendKeys(Keys.Tab).Perform();
            for (var index = 0; index < resultCount; index++)
            {
                action.SendKeys(OpenQA.Selenium.Keys.Down).Perform();
                String classNameForStrongTags = srt.getClassForStrongname(index);
                if (classNameForStrongTags.Contains("tt-selectable"))
                {
                    traversed = true;
                }
                else
                {
                    return false;
                }
                
            }
            return traversed;
        }
        #endregion

        #region Story 1232- Verify if Navigation using Up keys is possible
        [WebDriverTest(Enabled = true, Groups = new[] { AddressBookTestGroups.CurrentPupilQuickSearch }, Browsers = new[] { BrowserDefaults.Chrome })]
        [Variant(Variant.NorthernIrelandStatePrimary)]
        public void KeyUpNavigationOnPupilResults()
        {
            AddressBookSearchPage searchBox = QuickSearch.QuickSearchNavigation();
            SearchResultTile srt = searchBox.EnterSearchTextForPupils(textForSearch);
            int resultCount = srt.tileCount();
            Actions action = new Actions(WebContext.WebDriver);
            action.SendKeys(Keys.Tab).Perform();
            action.SendKeys(Keys.Tab).Perform();
            for (var index = 0; index < resultCount; index++)
            {
                action.SendKeys(OpenQA.Selenium.Keys.Down).Perform();
            }

            for (var i = resultCount - 2; i >= 0; i--)
            {
                action.SendKeys(OpenQA.Selenium.Keys.Up).Perform();
                String classNameForStrongTags = srt.getClassForStrongname(i);
                Assert.That(classNameForStrongTags.Contains("tt-selectable"));
            }
        }
        #endregion

        #region  Story 1234- Check if Basic information of Current Pupils are displayed on hitting enter
        [WebDriverTest(Enabled = true, Groups = new[] { AddressBookTestGroups.CurrentPupilQuickSearch }, Browsers = new[] { BrowserDefaults.Chrome })]
        [Variant(Variant.NorthernIrelandStatePrimary)]
        public void DisplayPupilBasicDetailsOnEnter()
        {
            #region Data Pupil Setup

            #region Create a new pupil, so that it can be attached later to the newly created contact1

            AutomationSugar.Log("Data Creation started");
            Guid pupilId = Guid.NewGuid();
            DataPackage dataPackage = this.BuildDataPackage();
            var pupilSurname = Utilities.GenerateRandomString(10, "PupilEnterKey_Test");
            var pupilForename = Utilities.GenerateRandomString(10, "PupilEnterKey_Test" + Thread.CurrentThread.ManagedThreadId);
            dataPackage.AddBasicLearner(pupilId, pupilSurname, pupilForename, new DateTime(2005, 10, 01), new DateTime(2012, 08, 01));

            #endregion
            DataSetup DataSetPupil = new DataSetup(purgeBeforeInsert: false, purgeAfterTest: true, packages: dataPackage);

            AddressBookSearchPage searchBox = QuickSearch.QuickSearchNavigation();
            searchBox.ClearText();
            searchBox.EnterSearchTextForPupils("PupilEnterKey_Test");
            searchBox.HitEnteronFirstPupilRecord();
            AddressBookPopup popup = new AddressBookPopup();
            popup.WaitForDialogToAppear();
            Assert.IsTrue(popup.GetPupilBasicDetails());
        }
        #endregion

        #region Data Provider for DisplayPupilEmailDetails
        public List<object[]> PupilsList()
        {
            string pattern = "M/d/yyyy";
            string pupilSurname = "BFeatie" + POM.Helper.SeleniumHelper.GenerateRandomString(3);
            string pupilForename = "CGhris" + POM.Helper.SeleniumHelper.GenerateRandomString(3);
            string dateOfBirth = DateTime.ParseExact("10/06/2006", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            string DateOfAdmission = DateTime.ParseExact("10/09/2010", pattern, CultureInfo.InvariantCulture).ToString(pattern);

            var res = new List<object[]>
            {
                new object[] {pupilSurname, pupilForename, "Female", dateOfBirth,
                DateOfAdmission, "Year 2"}
            };
            return res;
        }
        #endregion



        #region Story 1232- Check Results appearing in ascending order
        [WebDriverTest(DataProvider="PupilsList",Enabled = true, Groups = new[] { AddressBookTestGroups.CurrentPupilQuickSearch }, Browsers = new[] { BrowserDefaults.Chrome })]
        [Variant(Variant.NorthernIrelandStatePrimary)]
        public void CheckPupilResultsOrder(string forenameSetup, string surnameSetup, string gender, string dateOfBirth, string DateOfAdmission, string yearGroup)
        {
            #region Data Preparation
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);
            TaskMenuBar taskMenuInstance = new TaskMenuBar();
            taskMenuInstance.WaitForTaskMenuBarButton();
            DateTime dobSetup = Convert.ToDateTime(dateOfBirth);
            DateTime dateOfAdmissionSetup = Convert.ToDateTime(DateOfAdmission);

            var learnerIdSetup = Guid.NewGuid();
            var BuildPupilRecord = this.BuildDataPackage();
            #endregion

            BuildPupilRecord.AddBasicLearner(learnerIdSetup, surnameSetup, forenameSetup, dobSetup, dateOfAdmissionSetup, genderCode: "1", enrolStatus: "C");
            DataSetup DataSetPupil = new DataSetup(purgeBeforeInsert: false, purgeAfterTest: true, packages: BuildPupilRecord);

            AddressBookSearchPage searchBox = new AddressBookSearchPage();
            searchBox.ClearText();
            searchBox.EnterSearchTextForPupils(surnameSetup);
            POM.Helper.SeleniumHelper.Sleep(2);
            searchBox.ClearText();
            searchBox.EnterSearchTextForPupils(surnameSetup);
            var selectedElements = WebContext.WebDriver.FindElements(By.CssSelector(SeleniumHelper.AutomationId("PreferredListName_Learner")));
            Assert.That(selectedElements[0].Text.Contains(forenameSetup));
        }
        #endregion
    }
}
        #endregion