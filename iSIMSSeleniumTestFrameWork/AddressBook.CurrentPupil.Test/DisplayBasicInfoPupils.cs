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
    public class DisplayBasicInfoPupils
    {
        #region Data Provider for DisplayPupilEmailDetails
        public List<object[]> PupilsList()
        {
            string pattern = "M/d/yyyy";
            string pupilSurname = "Cpaig" + POM.Helper.SeleniumHelper.GenerateRandomString(3);
            string pupilForename = "Dgain" + POM.Helper.SeleniumHelper.GenerateRandomString(3);
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

        #region  Story 1234- Check if Basic information of Current Pupils are displayed
        [WebDriverTest(DataProvider = "PupilsList", Enabled = true, Groups = new[] { AddressBookTestGroups.CurrentPupilQuickSearch }, Browsers = new[] { BrowserDefaults.Chrome })]
        [Variant(Variant.NorthernIrelandStatePrimary)]
        public void DisplayPupilBasicDetails(string forenameSetup, string surnameSetup, string gender, string dateOfBirth, string DateOfAdmission, string yearGroup)
        {

            #region Data Preparation
         
            DateTime dobSetup = Convert.ToDateTime(dateOfBirth);
            DateTime dateOfAdmissionSetup = Convert.ToDateTime(DateOfAdmission);

            var learnerIdSetup = Guid.NewGuid();
            var BuildPupilRecord = this.BuildDataPackage();
            #endregion

            BuildPupilRecord.AddBasicLearner(learnerIdSetup, surnameSetup, forenameSetup, dobSetup, dateOfAdmissionSetup, genderCode: "1", enrolStatus: "C");
            DataSetup DataSetPupil = new DataSetup(purgeBeforeInsert: false, purgeAfterTest: true, packages: BuildPupilRecord);

            AddressBookSearchPage searchBox = QuickSearch.QuickSearchNavigation();
            searchBox.ClearText();
            searchBox.EnterSearchTextForPupils(surnameSetup);
            AddressBookPopup popup = searchBox.ClickOnFirstPupilRecord();
            Assert.IsTrue(popup.GetPupilBasicDetails());
        }
        #endregion
    }
}
