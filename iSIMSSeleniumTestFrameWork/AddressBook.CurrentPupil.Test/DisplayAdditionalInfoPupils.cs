using AddressBook.Components;
using AddressBook.Test;
using NUnit.Framework;
using Selene.Support.Attributes;
using SeSugar.Data;
using SharedComponents.Helpers;
using SharedComponents.HomePages;
using System;
using System.Collections.Generic;
using System.Globalization;
using TestSettings;
using WebDriverRunner.internals;
using SeSugar;
using Attendance.POM.DataHelper;
using SeSugar;
using SeSugar.Automation;
using SeSugar.Data;
using POM.Components.Pupil;
using System.Threading;
using WebDriverRunner.webdriver;

namespace AddressBook.CurrentPupil.Test
{
    public class DisplayAdditionalInfoPupils
    {
        // private readonly string textForSearch = "AddressBook_CurrentPupil";
        private readonly string NoTelephonetextForSearch = "Carter";

        #region Data Provider for Current Pupil Name options
        public List<object[]> PupilsWithEmailAndTelephoneData()
        {
            string pattern = "M/d/yyyy";
            string pupilSurname = "AddressBook_CurrentPupil" + POM.Helper.SeleniumHelper.GenerateRandomString(3);
            string pupilForename = "AddressBook_CurrentPupil" + POM.Helper.SeleniumHelper.GenerateRandomString(3);
            string dateOfBirth = DateTime.ParseExact("10/06/2006", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            string DateOfAdmission = DateTime.ParseExact("10/09/2010", pattern, CultureInfo.InvariantCulture).ToString(pattern);

            string telNo = "+44 0123456789";
            //    string location = "Other";
            string email = "learner@gmail.com";

            var res = new List<object[]>
            {
                new object[] {pupilSurname, pupilForename, "Female", dateOfBirth,
                DateOfAdmission, "Year 2",telNo,email}
            };
            return res;
        }
        #endregion

        #region  Story 1235- Verify if Phone Numbers are displayed for Current Pupils
        [WebDriverTest(DataProvider = "PupilsWithEmailAndTelephoneData", Enabled = true, Groups = new[] { AddressBookTestGroups.CurrentPupilQuickSearch }, Browsers = new[] { BrowserDefaults.Chrome })]
        [Variant(Variant.NorthernIrelandStatePrimary)]
        public void DisplayPupilTelephoneDetails(string pupilSurName, string pupilForeName, string gender, string dateOfBirth, string DateOfAdmission, string yearGroup, string telNo, string email)
        {
            #region Data Preparation
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);
            TaskMenuBar taskMenuInstance = new TaskMenuBar();
            taskMenuInstance.WaitForTaskMenuBarButton();


            DateTime dobSetup = Convert.ToDateTime(dateOfBirth);
            DateTime dateOfAdmissionSetup = Convert.ToDateTime(DateOfAdmission);
            var learnerIdSetup = Guid.NewGuid();
            var BuildPupilRecord = this.BuildDataPackage();
            BuildPupilRecord.AddBasicLearner(learnerIdSetup, pupilSurName, pupilForeName, dobSetup, dateOfAdmissionSetup, genderCode: "1", enrolStatus: "C");
            DataSetup DataSetPupil = new DataSetup(purgeBeforeInsert: false, purgeAfterTest: true, packages: BuildPupilRecord);

            AutomationSugar.NavigateMenu("Tasks", "Pupils", "Pupil Records");
            var pupilRecordTriplet = new PupilRecordTriplet();
            pupilRecordTriplet.SearchCriteria.PupilName = String.Format("{0}, {1}", pupilSurName, pupilForeName);
            POM.Helper.SeleniumHelper.Sleep(4);
            pupilRecordTriplet.SearchCriteria.Search();
            var resultPupils = pupilRecordTriplet.SearchCriteria.Search();
            int count = resultPupils.Count();
            if (count == 1)
            {
                var pupilSearchTile = resultPupils.FirstOrDefault(t => t.Name.Equals(String.Format("{0}, {1}", pupilSurName, pupilForeName)));
                var pupilRecord = pupilSearchTile == null ? null : pupilSearchTile.Click<PupilRecordPage>();
                pupilSearchTile.Click<PupilRecordPage>();
                //    Thread.Sleep(1000);
                //    Assert.AreNotEqual(null, pupilRecord, "Did not find pupil");

                // Add Pupil Telephone number
                pupilRecord.SelectPhoneEmailTab();
                pupilRecord = new PupilRecordPage();
                pupilRecord.ClickAddTelephoneNumber();
                pupilRecord.TelephoneNumberTable[0].TelephoneNumber = telNo;

                // Add Email Address
                pupilRecord.ClickAddEmailAddress();
                pupilRecord.EmailTable[0].EmailAddress = email;

                pupilRecord = PupilRecordPage.Create();
                pupilRecord.SavePupil();

                //Assert
                //    Assert.AreEqual(true, pupilRecord.IsSuccessMessageDisplayed(), "Success message is not display");
            #endregion

                AddressBookSearchPage searchBox = new AddressBookSearchPage();
                searchBox.ClearText();
                searchBox.EnterSearchTextForPupils(pupilSurName);
                AddressBookPopup popup = searchBox.ClickOnFirstPupilRecord();
                Assert.IsTrue(popup.IsPupilTelephoneDisplayed());
            }
            else
                throw new Exception();
        }
        #endregion


        #region Data Provider for DisplayPupilEmailDetails
        public List<object[]> PupilsWithEmailData()
        {
            string pattern = "M/d/yyyy";
            string pupilSurname = "PupilsWithEmailData" + POM.Helper.SeleniumHelper.GenerateRandomString(3);
            string pupilForename = "PupilsWithEmailData" + POM.Helper.SeleniumHelper.GenerateRandomString(3);
            string dateOfBirth = DateTime.ParseExact("10/06/2006", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            string DateOfAdmission = DateTime.ParseExact("10/09/2010", pattern, CultureInfo.InvariantCulture).ToString(pattern);

            string telNo = "+44 0123456789";
            //    string location = "Other";
            string email = "learner@gmail.com";

            var res = new List<object[]>
            {
                new object[] {pupilSurname, pupilForename, "Female", dateOfBirth,
                DateOfAdmission, "Year 2",telNo,email}
            };
            return res;
        }
        #endregion

        #region  Story 1235- Verify if emails are displayed for Current Pupils
        [WebDriverTest(DataProvider = "PupilsWithEmailData", Enabled = true, Groups = new[] { AddressBookTestGroups.CurrentPupilQuickSearch }, Browsers = new[] { BrowserDefaults.Chrome })]
        [Variant(Variant.NorthernIrelandStatePrimary)]
        public void DisplayPupilEmailDetails(string pupilSurName, string pupilForeName, string gender, string dateOfBirth, string DateOfAdmission, string yearGroup, string telNo, string email)
        {
            #region Data Preparation

          //  WebContext.WebDriver.Manage().Window.Maximize();
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);
            
            TaskMenuBar taskMenuInstance = new TaskMenuBar();
            taskMenuInstance.WaitForTaskMenuBarButton();


            DateTime dobSetup = Convert.ToDateTime(dateOfBirth);
            DateTime dateOfAdmissionSetup = Convert.ToDateTime(DateOfAdmission);
            var learnerIdSetup = Guid.NewGuid();
            var BuildPupilRecord = this.BuildDataPackage();
            BuildPupilRecord.AddBasicLearner(learnerIdSetup, pupilSurName, pupilForeName, dobSetup, dateOfAdmissionSetup, genderCode: "1", enrolStatus: "C");
            DataSetup DataSetPupil = new DataSetup(purgeBeforeInsert: false, purgeAfterTest: true, packages: BuildPupilRecord);
            WebContext.WebDriver.Manage().Window.Maximize();
            AutomationSugar.NavigateMenu("Tasks", "Pupils", "Pupil Records");
            WebContext.WebDriver.Manage().Window.Maximize();
            var pupilRecordTriplet = new PupilRecordTriplet();
            pupilRecordTriplet.SearchCriteria.PupilName = String.Format("{0}, {1}", pupilSurName, pupilForeName);
            pupilRecordTriplet.SearchCriteria.Search();
            
            POM.Helper.SeleniumHelper.Sleep(2);
            pupilRecordTriplet.SearchCriteria.PupilName = "";
            pupilRecordTriplet.SearchCriteria.PupilName = String.Format("{0}, {1}", pupilSurName, pupilForeName);
            pupilRecordTriplet.SearchCriteria.Search();

            var resultPupils = pupilRecordTriplet.SearchCriteria.Search();
            int count = resultPupils.Count();
            if (count == 1)
            {
                var pupilSearchTile = resultPupils.FirstOrDefault(t => t.Name.Equals(String.Format("{0}, {1}", pupilSurName, pupilForeName)));
                var pupilRecord = pupilSearchTile == null ? null : pupilSearchTile.Click<PupilRecordPage>();
                pupilSearchTile.Click<PupilRecordPage>();
                //    Thread.Sleep(1000);
                //    Assert.AreNotEqual(null, pupilRecord, "Did not find pupil");

                // Add Pupil Telephone number
                pupilRecord.SelectPhoneEmailTab();
                pupilRecord = new PupilRecordPage();
                pupilRecord.ClickAddTelephoneNumber();
                pupilRecord.TelephoneNumberTable[0].TelephoneNumber = telNo;

                // Add Email Address
                pupilRecord.ClickAddEmailAddress();
                pupilRecord.EmailTable[0].EmailAddress = email;

                pupilRecord = PupilRecordPage.Create();
                pupilRecord.SavePupil();

            }//if
            else
                throw new Exception();
            #endregion

            AddressBookSearchPage searchBox = new AddressBookSearchPage();
            searchBox.ClearText();
            searchBox.EnterSearchTextForPupils(pupilSurName);
            AddressBookPopup popup = searchBox.ClickOnFirstPupilRecord();
            Assert.IsTrue(popup.IsEmailDisplayed());
        }
        #endregion


        #region Data Provider for DisplayPupilAddressDetails
        public List<object[]> PupilsWithAddressData()
        {
            string pattern = "M/d/yyyy";
            string pupilSurname = "PupilsWithAddressData" + POM.Helper.SeleniumHelper.GenerateRandomString(3);
            string pupilForename = "PupilsWithAddressData" + POM.Helper.SeleniumHelper.GenerateRandomString(3);
            string dateOfBirth = DateTime.ParseExact("10/06/2006", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            string DateOfAdmission = DateTime.ParseExact("10/10/2010", pattern, CultureInfo.InvariantCulture).ToString(pattern);


            var res = new List<object[]>
            {
                new object[] {pupilSurname, pupilForename, "Female", dateOfBirth,
                DateOfAdmission, "Year 2",new string[]{"567", "House Name", "Flat3", "Street3", "District3", "City3", "County2", "EC1A 1BB", "United Kingdom"},}
            };
            return res;
        }
        #endregion

        #region  Story 1235- Check if addresses are displayed for Current Pupils
        [WebDriverTest(DataProvider="PupilsWithAddressData",Enabled = true, Groups = new[] { AddressBookTestGroups.CurrentPupilQuickSearch }, Browsers = new[] { BrowserDefaults.Chrome })]
        [Variant(Variant.NorthernIrelandStatePrimary)]
        public void DisplayPupilAddressDetails(string forenameSetup,string surnameSetup,string gender, string dateOfBirth, string DateOfAdmission, string yearGroup,string[] currentAddress)
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

            //Address Add
            #region

            AutomationSugar.NavigateMenu("Tasks", "Pupils", "Pupil Records");
            AutomationSugar.WaitForAjaxCompletion();

            var pupilRecordTriplet = new PupilRecordTriplet();
            
            pupilRecordTriplet.SearchCriteria.PupilName = String.Format("{0}, {1}", surnameSetup, forenameSetup);
            pupilRecordTriplet.SearchCriteria.Search();

            POM.Helper.SeleniumHelper.Sleep(2);
            pupilRecordTriplet.SearchCriteria.PupilName = "";
            pupilRecordTriplet.SearchCriteria.PupilName = String.Format("{0}, {1}", surnameSetup, forenameSetup);
            pupilRecordTriplet.SearchCriteria.Search();

            var resultPupils = pupilRecordTriplet.SearchCriteria.Search();
            int count = resultPupils.Count();
            if (count == 1)
            {
                var pupilSearchTile = resultPupils.FirstOrDefault(t => t.Name.Equals(String.Format("{0}, {1}", surnameSetup, forenameSetup)));
                var pupilRecord = pupilSearchTile == null ? null : pupilSearchTile.Click<PupilRecordPage>();
               // pupilSearchTile.Click<PupilRecordPage>();
                //    Thread.Sleep(1000);
                //    Assert.AreNotEqual(null, pupilRecord, "Did not find pupil");

                  // AutomationSugar.WaitForAjaxCompletion();


                pupilRecord.SelectAddressesTab();

                var addAddress = pupilRecord.ClickAddAddress();
                addAddress.ClickManualAddAddress();
                addAddress.BuildingNo = currentAddress[0];
                addAddress.BuildingName = currentAddress[1];
                addAddress.Flat = currentAddress[2];
                addAddress.Street = currentAddress[3];
                addAddress.District = currentAddress[4];
                addAddress.City = currentAddress[5];
                addAddress.County = currentAddress[6];
                addAddress.PostCode = currentAddress[7];
                addAddress.CountryPostCode = currentAddress[8];
                addAddress.ClickOk(5);

                //Save
                pupilRecord.SavePupil();
            }
            else
            {
                throw new Exception();
            }
            AutomationSugar.WaitForAjaxCompletion();
            #endregion
            
            AddressBookSearchPage searchBox = new AddressBookSearchPage();
            searchBox.ClearText();
            searchBox.EnterSearchTextForPupils(forenameSetup);
            AddressBookPopup popup = searchBox.ClickOnFirstPupilRecord();
            POM.Helper.SeleniumHelper.Sleep(2);
            Assert.IsTrue(popup.IsAddressDisplayed());
        }
        #endregion


        #region Story 1235 - Check if image are displayed for current Pupils
        [WebDriverTest(Enabled = true, Groups = new[] { AddressBookTestGroups.CurrentPupilQuickSearch }, Browsers = new[] { BrowserDefaults.Chrome })]
        [Variant(Variant.NorthernIrelandStatePrimary)]
        public void DisplayCurrentPupilImage()
        {
            AddressBookSearchPage searchBox = QuickSearch.QuickSearchNavigation();
            searchBox.ClearText();
            searchBox.EnterSearchTextForPupils("ad");
            AddressBookPopup popup = searchBox.ClickOnFirstPupilRecord();
            Assert.IsTrue(popup.IsPupilImageDisplayed());
        }
        #endregion
    }
}
