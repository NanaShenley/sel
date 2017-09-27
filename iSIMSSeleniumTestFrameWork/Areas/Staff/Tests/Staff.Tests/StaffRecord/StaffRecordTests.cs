using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using Selene.Support.Attributes;
using SeSugar;
using SeSugar.Automation;
using SeSugar.Data;
using Staff.POM.Components.Staff;
using Staff.POM.Components.Staff.Dialogs;
using Staff.POM.Helper;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using WebDriverRunner.internals;
using Environment = SeSugar.Environment;

namespace Staff.Tests.StaffRecord
{
    [TestClass]
    public class StaffRecordTests
    {
        private readonly string coresidentMatchedAutomationID = "update_button";
        #region MS Unit Testing support
        public TestContext TestContext { get; set; }

        [TestInitialize]
        public void Init()
        {
            TestRunner.VSSeleniumTest.Init(this, TestContext);
        }
        [TestCleanup]
        public void Cleanup()
        {
            TestRunner.VSSeleniumTest.Cleanup(TestContext);
        }
        #endregion

        #region Staff Record Tests

        [TestMethod]
        [ChromeUiTest("StaffRecord", "P1")]
        public void Can_create_staff_record_as_personnel_officer()
        {
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.PersonnelOfficer);
            Can_create_staff_record_common();
        }

        [TestMethod]
        [ChromeUiTest("Can_create_staff_record_as_school_admin" ,"StaffRecord", "P2")]
        public void Can_create_staff_record_as_school_admin()
        {
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);
            Can_create_staff_record_common();
        }

        private void Can_create_staff_record_common()
        {
            AutomationSugar.NavigateMenu("Tasks", "Staff", "Staff Records");
            var staffRecordTriplet = new StaffRecordTriplet();
            var addNewStaffDialog = staffRecordTriplet.AddNewStaff();

            string forename, middlename, surname, gender, dob, doa;

            // Fill Add New Staff Dialog                           
            forename = addNewStaffDialog.Forename = "Test";
            middlename = addNewStaffDialog.MiddleName = "Record";
            surname = addNewStaffDialog.SurName = Utilities.GenerateRandomString(6, "Selenium");
            gender = addNewStaffDialog.Gender = "Male";
            dob = addNewStaffDialog.DateOfBirth = DateTime.Today.AddYears(-29).ToShortDateString();

            // Fill Service Detail Dialog
            ServiceDetailDialog serviceDetailDialog = addNewStaffDialog.Continue();
            doa = serviceDetailDialog.DateOfArrival = DateTime.Today.AddDays(-1).ToShortDateString();
            var staffRecordPage = serviceDetailDialog.CreateRecord();

            Assert.AreEqual(forename, staffRecordPage.LegalForeName);
            Assert.AreEqual(middlename, staffRecordPage.MiddleName);
            Assert.AreEqual(surname, staffRecordPage.LegalSurname);
            Assert.AreEqual(gender, staffRecordPage.Gender);
            Assert.AreEqual(dob, staffRecordPage.DOB);
            Assert.AreEqual(doa, staffRecordPage.ServiceRecordTable.Rows.First().DOA);

            Assert.IsTrue(AutomationSugar.SuccessMessagePresent(staffRecordPage.ComponentIdentifier), "Add New Staff failed");
        }

        [TestMethod]
        [ChromeUiTest("StaffRecord", "P1")]
        public void Can_delete_staff_record_as_personnel_officer()
        {
            string surname = Utilities.GenerateRandomString(6, "Selenium");
            string forename = "Test";

            using (new DataSetup(GetStaffRecord_current(Guid.NewGuid(), forename, surname)))
            {
                //Login as PO and navigate to Delete Staff Record
                SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.PersonnelOfficer);
                AutomationSugar.NavigateMenu("Tasks", "Staff", "Delete Staff Record");

                //Search for injected record and delete it
                var deleteStaffRecordTriplet = new DeleteStaffRecordTriplet();
                deleteStaffRecordTriplet.SearchCriteria.StaffName = surname;
                deleteStaffRecordTriplet.SearchCriteria.IsCurrent = true;
                var staffTiles = deleteStaffRecordTriplet.SearchCriteria.Search();
                var staffRecord = staffTiles.SingleOrDefault(x => true).Click<StaffRecordPage>();
                staffRecord.DeleteStaff();

                //Delete success when success bar appears
                Assert.IsTrue(AutomationSugar.SuccessMessagePresent(staffRecord.ComponentIdentifier), "Delete Staff failed");
            }
        }

        [TestMethod]
        [ChromeUiTest("StaffRecord", "P1")]
        public void Can_search_for_all_staff_as_personnel_officer()
        {
            string surname_current, forename_current, surname_future, forename_future, surname_former, forename_former, test_key;

            test_key = GetHashString();

            forename_current = "Current";
            surname_current = Utilities.GenerateRandomString(6, string.Format("Selenium_{0}", test_key));

            forename_future = "Future";
            surname_future = Utilities.GenerateRandomString(6, string.Format("Selenium_{0}", test_key));

            forename_former = "Former";
            surname_former = Utilities.GenerateRandomString(6, string.Format("Selenium_{0}", test_key));

            var packages = new DataPackage[]
            {
                GetStaffRecord_current(Guid.NewGuid(), forename_current, surname_current),
                GetStaffRecord_future(Guid.NewGuid(), forename_future, surname_future),
                GetStaffRecord_former(Guid.NewGuid(), forename_former, surname_former)
            };

            using (new DataSetup(packages))
            {
                SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.PersonnelOfficer);
                AutomationSugar.NavigateMenu("Tasks", "Staff", "Staff Records");
                var staffRecordTriplet = new StaffRecordTriplet();

                staffRecordTriplet.SearchCriteria.StaffName = string.Format("Selenium_{0}", test_key);
                staffRecordTriplet.SearchCriteria.IsCurrent = false;
                staffRecordTriplet.SearchCriteria.IsFuture = false;
                staffRecordTriplet.SearchCriteria.IsLeaver = false;

                var staffTiles = staffRecordTriplet.SearchCriteria.Search();

                Assert.AreEqual(3, staffTiles.Count(), "Expected 3 Staff Record results.");
            }
        }

        [TestMethod]
        [ChromeUiTest("StaffRecord", "P1")]
        public void Can_search_for_current_staff_as_personnel_officer()
        {
            string surname_current, forename_current;

            forename_current = "Test";
            surname_current = Utilities.GenerateRandomString(6, "Selenium");

            var currentStaff = GetStaffRecord_current(Guid.NewGuid(), forename_current, surname_current);

            using (new DataSetup(currentStaff))
            {
                SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.PersonnelOfficer);
                AutomationSugar.NavigateMenu("Tasks", "Staff", "Staff Records");
                var staffRecordTriplet = new StaffRecordTriplet();

                staffRecordTriplet.SearchCriteria.StaffName = surname_current;
                staffRecordTriplet.SearchCriteria.IsCurrent = true;
                staffRecordTriplet.SearchCriteria.IsFuture = false;
                staffRecordTriplet.SearchCriteria.IsLeaver = false;

                var staffTiles = staffRecordTriplet.SearchCriteria.Search();
                var searchResult = staffTiles.SingleOrDefault(x => true);

                Assert.IsNotNull(searchResult, "Staff record not found");
            }
        }

        [TestMethod]
        [ChromeUiTest("StaffRecord", "P1")]
        public void Can_search_for_future_staff_as_personnel_officer()
        {
            string surname_future, forename_future;

            forename_future = "Test";
            surname_future = Utilities.GenerateRandomString(6, "Selenium");

            var currentStaff = GetStaffRecord_future(Guid.NewGuid(), forename_future, surname_future);

            using (new DataSetup(currentStaff))
            {
                SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.PersonnelOfficer);
                AutomationSugar.NavigateMenu("Tasks", "Staff", "Staff Records");
                var staffRecordTriplet = new StaffRecordTriplet();

                staffRecordTriplet.SearchCriteria.StaffName = surname_future;
                staffRecordTriplet.SearchCriteria.IsCurrent = false;
                staffRecordTriplet.SearchCriteria.IsFuture = true;
                staffRecordTriplet.SearchCriteria.IsLeaver = false;

                var staffTiles = staffRecordTriplet.SearchCriteria.Search();
                var searchResult = staffTiles.SingleOrDefault(x => true);

                Assert.IsNotNull(searchResult, "Staff record not found");
            }
        }

        [TestMethod]
        [ChromeUiTest("StaffRecord", "P1")]
        public void Can_search_for_former_staff_as_personnel_officer()
        {
            string surname_former, forename_former;

            forename_former = "Test";
            surname_former = Utilities.GenerateRandomString(6, "Selenium");

            var currentStaff = GetStaffRecord_former(Guid.NewGuid(), forename_former, surname_former);

            using (new DataSetup(currentStaff))
            {
                SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.PersonnelOfficer);
                AutomationSugar.NavigateMenu("Tasks", "Staff", "Staff Records");
                var staffRecordTriplet = new StaffRecordTriplet();

                staffRecordTriplet.SearchCriteria.StaffName = surname_former;
                staffRecordTriplet.SearchCriteria.IsCurrent = false;
                staffRecordTriplet.SearchCriteria.IsFuture = false;
                staffRecordTriplet.SearchCriteria.IsLeaver = true;

                var staffTiles = staffRecordTriplet.SearchCriteria.Search();
                var searchResult = staffTiles.SingleOrDefault(x => true);

                Assert.IsNotNull(searchResult, "Staff record not found");
            }
        }

        [TestMethod]
        [ChromeUiTest("StaffRecord", "P1", "Read")]
        public void Read_existing_Staff_as_PO()
        {
            #region Arrange
            var startDate = DateTime.Today.AddDays(-1);
            var serviceRecordId = Guid.NewGuid();
            var staffRecordData = new StaffRecordData(3);

            var staffRecord = this.BuildDataPackage()
               .AddData("Staff", new
               {
                   Id = staffRecordData.StaffId,
                   LegalForename = staffRecordData.LegalForename,
                   LegalSurname = staffRecordData.LegalSurname,
                   LegalMiddleNames = staffRecordData.LegalMiddleNames,
                   PreferredForename = staffRecordData.PreferredForname,
                   PreferredSurname = staffRecordData.PreferredSurname,
                   QuickNote = staffRecordData.QuickNote,
                   DateOfBirth = staffRecordData.DateOfBirth,
                   Gender = staffRecordData.GenderItem.ID,
                   Title = staffRecordData.TitleItem.ID,
                   MaritalStatus = staffRecordData.MaritalStatusItem.ID,
                   PolicyACLID = CoreQueries.GetPolicyAclId("Staff"),
                   School = CoreQueries.GetSchoolId(),
                   TenantID = SeSugar.Environment.Settings.TenantId
               })
               .AddData("StaffServiceRecord", new
               {
                   Id = serviceRecordId,
                   DOA = startDate,
                   ContinuousServiceStartDate = startDate,
                   LocalAuthorityStartDate = startDate,
                   Staff = staffRecordData.StaffId,
                   TenantID = SeSugar.Environment.Settings.TenantId
               });
            #endregion

            #region Act
            using (new DataSetup(staffRecord))
            {
                LoginAndNavigate(SeleniumHelper.iSIMSUserType.PersonnelOfficer);
            }

            //Get staff record
            var staffRecordPage = GetStaffRecord(staffRecordData);
            bool detailsMatch = CheckStaffDetailsMatch(staffRecordData, staffRecordPage);

            //Get service details dialog to prove that the service record exists
            var serviceRecordDialog = staffRecordPage.ServiceRecordTable.Rows.SingleOrDefault(t => t.RowId.Equals(serviceRecordId.ToString()));
            #endregion

            #region Assert
            Assert.IsTrue(detailsMatch, "Staff record details did not match the injected SQL data.");
            Assert.IsNotNull(serviceRecordDialog, "Staff Service record not found");
            #endregion
        }

        [TestMethod]
        [ChromeUiTest("StaffRecord", "P1", "Update")]
        public void Update_existing_Staff_as_PO()
        {
            //Inject a current Staff Record
            //Navigate to Staff Records as PO
            //Load the injected Staff Record
            //Make some changes, Save
            //Assert save success, Assert changes retained

            #region Arrange
            Guid staffId = Guid.NewGuid();
            string forename = Utilities.GenerateRandomString(6, "Selenium");
            string surname = Utilities.GenerateRandomString(6, "Selenium");

            var newData = new
            {
                LegalSurname = Utilities.GenerateRandomString(6, "Selenium"),
                LegalForename = Utilities.GenerateRandomString(6, "Selenium"),
                PreferredForname = Utilities.GenerateRandomString(6, "Selenium"),
                PreferredSurname = Utilities.GenerateRandomString(6, "Selenium"),
                LegalMiddleNames = Utilities.GenerateRandomString(6, "Selenium"),
                QuickNote = Utilities.GenerateRandomString(6, "Selenium"),
                DateOfBirth = new DateTime(1995, 2, 2).ToShortDateString(),
                Title = "Mr",
                Gender = "Male",
                MaritalStatus = "Single",
            };
            #endregion


            #region Act
            using (new DataSetup(GetStaffRecord_current(staffId, forename, surname)))
            {
                LoginAndNavigate(SeleniumHelper.iSIMSUserType.PersonnelOfficer);

                //Get staff record
                var staffRecordPage = LoadStaff(staffId);



                staffRecordPage.LegalSurname = newData.LegalSurname;
                staffRecordPage.LegalForeName = newData.LegalForename;
                staffRecordPage.PreferForeName = newData.PreferredForname;
                staffRecordPage.PreferSurname = newData.PreferredSurname;
                staffRecordPage.MiddleName = newData.LegalMiddleNames;
                staffRecordPage.QuickNote = newData.QuickNote;
                staffRecordPage.DOB = newData.DateOfBirth;
                staffRecordPage.Title = newData.Title;
                staffRecordPage.Gender = newData.Gender;
                staffRecordPage.MaritalStatus = newData.MaritalStatus;
                staffRecordPage.ClickSave();
                staffRecordPage.ClickConfirmLegalNameChange("no");

                #endregion

                #region Assert
                staffRecordPage = LoadStaff(staffId);
                Assert.AreEqual(newData.LegalForename, staffRecordPage.LegalForeName, "LegalForeNames do not match.");
                Assert.AreEqual(newData.LegalSurname, staffRecordPage.LegalSurname, "LegalForeNames do not match.");
                Assert.AreEqual(newData.LegalMiddleNames, staffRecordPage.MiddleName, "LegalMiddleNames do not match.");
                Assert.AreEqual(newData.PreferredForname, staffRecordPage.PreferForeName, "PreferredForname do not match.");
                Assert.AreEqual(newData.PreferredSurname, staffRecordPage.PreferSurname, "PreferredSurname do not match.");
                Assert.AreEqual(newData.QuickNote, staffRecordPage.QuickNote, "QuickNote do not match.");
                Assert.AreEqual(newData.DateOfBirth, staffRecordPage.DOB, "DateOfBirth do not match.");
                Assert.AreEqual(newData.Gender, staffRecordPage.Gender, "Gender do not match.");
                Assert.AreEqual(newData.Title, staffRecordPage.Title, "Title do not match.");
                Assert.AreEqual(newData.MaritalStatus, staffRecordPage.MaritalStatus, "MaritalStatus do not match.");
                #endregion
            }
        }

        [TestMethod]
        [ChromeUiTest("StaffRecord", "P1", "Delete")]
        public void Delete_existing_Staff_as_PO()
        {
            //Inject a current Staff Record
            //Navigate to Staff Records as PO
            //Load the injected Staff Record
            //Delete
            //Assert delete success
            string surname = Utilities.GenerateRandomString(6, "Selenium");
            string forename = "Test";

            using (new DataSetup(GetStaffRecord_current(Guid.NewGuid(), forename, surname)))
            {
                //Login as PO and navigate to Delete Staff Record
                LoginAndNavigate(SeleniumHelper.iSIMSUserType.PersonnelOfficer);
                AutomationSugar.NavigateMenu("Tasks", "Staff", "Delete Staff Record");

                //Search for injected record and delete it
                var deleteStaffRecordTriplet = new DeleteStaffRecordTriplet();
                deleteStaffRecordTriplet.SearchCriteria.StaffName = surname;
                deleteStaffRecordTriplet.SearchCriteria.IsCurrent = true;
                var staffTiles = deleteStaffRecordTriplet.SearchCriteria.Search();
                var staffRecord = staffTiles.SingleOrDefault(x => true).Click<StaffRecordPage>();
                staffRecord.DeleteStaff();

                //Delete success when success bar appears
                Assert.IsTrue(AutomationSugar.SuccessMessagePresent(staffRecord.ComponentIdentifier), "Delete Staff failed");
            }
        }

        [TestMethod]
        [ChromeUiTest("StaffRecord", "P1", "ReAdmit")]
        public void Can_readmit_a_former_member_of_Staff_as_PO()
        {
            //Inject a former member of Staff
            //Navigate to the Staff Record as PO
            //Search for them (tick "Leaver" option in search criteria)
            //Load the record and click the Readmit button at the top
            //Fill in the Service Record form and Save
            //Assert the save success
            //Assert the presence of the member of staff when searching for "Current" staff
            //Assert the Service Record grid row reflects the expected dates
            #region Arrange
            var startDate = DateTime.Today.AddDays(-10);
            var serviceRecordId = Guid.NewGuid();
            var staffRecordData = new StaffRecordData(0);

            var staffRecord = this.BuildDataPackage()
               .AddData("Staff", new
               {
                   Id = staffRecordData.StaffId,
                   LegalForename = staffRecordData.LegalForename,
                   LegalSurname = staffRecordData.LegalSurname,
                   LegalMiddleNames = staffRecordData.LegalMiddleNames,
                   PreferredForename = staffRecordData.PreferredForname,
                   PreferredSurname = staffRecordData.PreferredSurname,
                   QuickNote = staffRecordData.QuickNote,
                   DateOfBirth = staffRecordData.DateOfBirth,
                   Gender = staffRecordData.GenderItem.ID,
                   Title = staffRecordData.TitleItem.ID,
                   MaritalStatus = staffRecordData.MaritalStatusItem.ID,
                   PolicyACLID = CoreQueries.GetPolicyAclId("Staff"),
                   School = CoreQueries.GetSchoolId(),
                   TenantID = SeSugar.Environment.Settings.TenantId
               })
               .AddData("StaffServiceRecord", new
               {
                   Id = serviceRecordId,
                   DOA = startDate,
                   DOL = startDate.AddDays(-2),
                   ContinuousServiceStartDate = startDate,
                   LocalAuthorityStartDate = startDate,
                   Staff = staffRecordData.StaffId,
                   TenantID = SeSugar.Environment.Settings.TenantId
               });
            #endregion

            #region Act
            using (new DataSetup(staffRecord))
            {
                LoginAndNavigate(SeleniumHelper.iSIMSUserType.PersonnelOfficer);
            }

            //Get staff record
            var staffRecordPage = GetStaffRecord(staffRecordData);

            //Find and click Re-admit
            var serviceDetailsDialog = staffRecordPage.ClickReAdmitStaff();
            Assert.IsNotNull(serviceDetailsDialog, "Service Details dialog was not shown after clicking Re-admit staff.");

            //Complete the service details dialog - supplying yesterday's date for arrival so the staff member is no longer a leaver
            var newDOA = DateTime.Today.AddDays(-1).ToShortDateString(); //Yesterday
            serviceDetailsDialog.DateOfArrival = newDOA;
            serviceDetailsDialog.ContinuousServiceStartDate = newDOA;
            serviceDetailsDialog.LocalAuthorityStartDate = newDOA;
            staffRecordPage = serviceDetailsDialog.SaveRecord();

            //Save success when success bar appears
            Assert.IsTrue(AutomationSugar.SuccessMessagePresent(staffRecordPage.ComponentIdentifier), "Re-admit Staff failed");
            staffRecordPage.Refresh();

            //Get the readmit staff button - it should NOT exist on the page
            AutomationSugar.WaitForAjaxCompletion();
            bool reAdmitButtonExists = SeleniumHelper.DoesElementExist(By.CssSelector("[data-automation-id='re-admit_staff_button']"));

            //Get the newly added Service Record
            var serviceRecord = staffRecordPage.ServiceRecordTable.Rows.FirstOrDefault(t => t.DOA.Equals(newDOA));
            #endregion

            #region Assert
            Assert.IsFalse(reAdmitButtonExists, "Staff re-admit still visible on screen after re-admitting staff.");
            Assert.IsNotNull(serviceRecord, "Service record does not contain the new DOA for the readmitted staff.");
            Assert.AreEqual(string.Empty, serviceRecord.DOL, "Service record does not contain an open DOL (i.e. empty text box) for the readmitted staff.");
            #endregion
        }

        #endregion

        #region Staff Address

        #region Add Address

        [TestMethod]
        [ChromeUiTest("StaffAddress", "P1", "Add", "Add_Staff_Address_Local")]
        [Variant(Variant.NorthernIrelandStatePrimary | Variant.NorthernIrelandStateSecondary | Variant.NorthernIrelandStateMultiphase |
            Variant.IndependentPrimary | Variant.IndependentSecondary | Variant.IndependentMultiphase)]
        public void Add_Staff_Address_Local()
        {
            #region Arrange

            Guid staffId = Guid.NewGuid();
            Guid addressID;

            string forename = Utilities.GenerateRandomString(6, "Selenium");
            string surname = Utilities.GenerateRandomString(6, "Selenium");

            string UPRN = Utilities.GenerateRandomString(6);
            string PAONDescription = Utilities.GenerateRandomString(6);
            string PAONRange = Utilities.GenerateRandomString(6);
            string SAON = Utilities.GenerateRandomString(6);
            string Street = Utilities.GenerateRandomString(6);
            string Locality = Utilities.GenerateRandomString(6);
            string Town = Utilities.GenerateRandomString(6);
            string AdministrativeArea = Utilities.GenerateRandomString(6);
            string PostCode =  Utilities.GenerateRandomString(6).ToUpper();
            string Country = "United Kingdom";

            const string _space = " ";
            const string _seperator = ", ";
            const string _lineSeperator = "\r\n";

            var addressDisplaySmall = string.Concat(SAON, _seperator,
                PAONDescription, _seperator,
                PAONRange, _seperator, Street, _seperator,Town);

            var addressDisplayLarge = string.Concat(PAONRange, _space,
                PostCode, _lineSeperator,
                Country);

            #endregion

            using (new DataSetup(GetStaffRecord_current(staffId, forename, surname),
                GetAddress(out addressID, UPRN, PAONDescription, PAONRange, SAON, Street, Locality, Town, AdministrativeArea, PostCode, Country)))
            {
                LoginAndNavigate(SeleniumHelper.iSIMSUserType.PersonnelOfficer, "Addresses");

                //Get staff record
                var staffRecordPage = LoadStaff(staffId);
                staffRecordPage.SelectAddressesTab();
                staffRecordPage.ClickAddAddress();

                AddAddressDialog addAddress = new AddAddressDialog();
                addAddress.PAONRangeSearch = PAONRange;
                addAddress.PostCodeSearch = PostCode;
                addAddress.ClickSearch();
                addAddress.Addresses = addressDisplaySmall;
                
                Assert.AreEqual(PAONRange, addAddress.BuildingNo);
                Assert.AreEqual(PostCode, addAddress.PostCode);
                Assert.AreEqual(Country, addAddress.Country);

                addAddress.ClickOk();
                staffRecordPage.ClickSave();

                staffRecordPage = new StaffRecordPage();
                staffRecordPage.SelectAddressesTab();

                var gridRow = staffRecordPage.AddressTable.Rows[0];
                Assert.AreEqual(addressDisplayLarge, gridRow.Address);
            }
        }      
    
        [TestMethod]
        [ChromeUiTest("StaffAddress", "P1", "Add", "Add_Staff_Address_WAV")]
        [Variant(Variant.EnglishStatePrimary | Variant.EnglishStateSecondary | Variant.EnglishStateMultiphase |
            Variant.WelshStatePrimary | Variant.WelshStateSecondary | Variant.WelshStateMultiphase)]
        public void Add_Staff_Address_WAV()
        {
            #region Arrange

            Guid staffId = Guid.NewGuid();
            string forename = Utilities.GenerateRandomString(6, "Selenium");
            string surname = Utilities.GenerateRandomString(6, "Selenium");

            string PAONRange = "22";
            string Street = "SUDELEY WALK";
            string Town = "BEDFORD";
            string PostCode = "MK41 8HS";
            string Country = "United Kingdom";

            const string _space = " ";
            const string _seperator = ", ";
            const string _lineSeperator = "\r\n";

            var addressDisplaySmall = string.Concat(
                PAONRange, _seperator, Street, _seperator, Town);

            var addressDisplayLarge = string.Concat(
                PAONRange, _space, Street, _lineSeperator,
                Town, _lineSeperator,
                Town, _lineSeperator,
                PostCode, _lineSeperator,
                Country);

            #endregion

            using (new DataSetup(GetStaffRecord_current(staffId, forename, surname)))
            {
                LoginAndNavigate(SeleniumHelper.iSIMSUserType.PersonnelOfficer, "Addresses");

                //Get staff record
                var staffRecordPage = LoadStaff(staffId);
                staffRecordPage.SelectAddressesTab();
                staffRecordPage.ClickAddAddress();

                AddAddressDialog addAddress = new AddAddressDialog();
                addAddress.PAONRangeSearch = PAONRange;
                addAddress.PostCodeSearch = PostCode;
                addAddress.ClickSearch();
                addAddress.ClickOk();
                staffRecordPage.ClickSave();

                staffRecordPage = new StaffRecordPage();
                staffRecordPage.SelectAddressesTab();

                var gridRow = staffRecordPage.AddressTable.Rows[0];
                Assert.AreEqual(addressDisplayLarge, gridRow.Address);
            }
        }

        #endregion

        #region Edit Address

        [TestMethod]
        [ChromeUiTest("StaffAddress", "P1", "Edit", "Edit_Staff_Address_Fields")]
        [Variant(Variant.NorthernIrelandStatePrimary | Variant.NorthernIrelandStateSecondary | Variant.NorthernIrelandStateMultiphase |
            Variant.IndependentPrimary | Variant.IndependentSecondary | Variant.IndependentMultiphase |
            Variant.EnglishStatePrimary | Variant.EnglishStateSecondary | Variant.EnglishStateMultiphase | 
            Variant.WelshStatePrimary | Variant.WelshStateSecondary | Variant.WelshStateMultiphase)]
        public void Edit_Staff_Address_Fields()
        {
            #region Arrange

            Guid staffId = Guid.NewGuid();
            Guid addressID, staffAddressID;

            string forename = Utilities.GenerateRandomString(6, "Selenium");
            string surname = Utilities.GenerateRandomString(6, "Selenium");

            string UPRN = Utilities.GenerateRandomString(6);
            string PAONDescription = Utilities.GenerateRandomString(6);
            string PAONRange = Utilities.GenerateRandomString(6);
            string SAON = Utilities.GenerateRandomString(6);
            string Street = Utilities.GenerateRandomString(6);
            string Locality = Utilities.GenerateRandomString(6);
            string Town = Utilities.GenerateRandomString(6);
            string AdministrativeArea = Utilities.GenerateRandomString(6);
            string PostCode = Utilities.GenerateRandomString(6);
            string Country = "United Kingdom";

            string newLocality = Utilities.GenerateRandomString(6);

            #endregion

            using (new DataSetup(
                GetStaffRecord_current(staffId, forename, surname),
                GetAddress(out addressID, UPRN, PAONDescription, PAONRange, SAON, Street, Locality, Town, AdministrativeArea, PostCode, Country),
                GetStaffAddress(out staffAddressID, DateTime.Today, "H", addressID, staffId)))
            {
                LoginAndNavigate(SeleniumHelper.iSIMSUserType.PersonnelOfficer, "Addresses");

                //Get staff record
                var staffRecordPage = LoadStaff(staffId);
                staffRecordPage.SelectAddressesTab();
                var gridRow = staffRecordPage.AddressTable.Rows[0];
                gridRow.ClickEditAddress();

                AddAddressDialog addAddress = new AddAddressDialog();
                addAddress.District = newLocality;
                addAddress.ClickOk();
                staffRecordPage.ClickSave();

                staffRecordPage = new StaffRecordPage();
                staffRecordPage.SelectAddressesTab();
                gridRow = staffRecordPage.AddressTable.Rows[0];
                gridRow.ClickEditAddress();

                addAddress = new AddAddressDialog();
                Assert.AreEqual(newLocality, addAddress.District);
            }
        }

        [TestMethod]
        [ChromeUiTest("StaffAddress", "P1", "Edit", "Edit_Staff_Address_Fields_Co_Resident_Match")]
        [Variant(Variant.NorthernIrelandStatePrimary | Variant.NorthernIrelandStateSecondary | Variant.NorthernIrelandStateMultiphase |
         Variant.IndependentPrimary | Variant.IndependentSecondary | Variant.IndependentMultiphase |
         Variant.EnglishStatePrimary | Variant.EnglishStateSecondary | Variant.EnglishStateMultiphase |
         Variant.WelshStatePrimary | Variant.WelshStateSecondary | Variant.WelshStateMultiphase)]
        public void Edit_Staff_Address_Fields_Co_Resident_Match()
        {
            #region Arrange

            Guid staffId = Guid.NewGuid();
            Guid addressID, staffAddressID;

            string forename = Utilities.GenerateRandomString(6, "Selenium");
            string surname = Utilities.GenerateRandomString(6, "Selenium");

            string UPRN = Utilities.GenerateRandomString(6);
            string PAONDescription = Utilities.GenerateRandomString(6);
            string PAONRange = Utilities.GenerateRandomString(6);
            string SAON = Utilities.GenerateRandomString(6);
            string Street = Utilities.GenerateRandomString(6);
            string Locality = Utilities.GenerateRandomString(6);
            string Town = Utilities.GenerateRandomString(6);
            string AdministrativeArea = Utilities.GenerateRandomString(6);
            string PostCode = Utilities.GenerateRandomString(6);
            string Country = "United Kingdom";

            string newLocality = Utilities.GenerateRandomString(6);

            #endregion

            using (new DataSetup(
                GetStaffRecord_current(staffId, forename, surname),
                GetAddress(out addressID, UPRN, PAONDescription, PAONRange, SAON, Street, Locality, Town, AdministrativeArea, PostCode, Country),
                GetStaffAddress(out staffAddressID, DateTime.Today, "H", addressID, staffId, DateTime.Today.AddDays(1)),
                GetAddress(out addressID, UPRN, PAONDescription, PAONRange, SAON, Street, Locality, Town, AdministrativeArea, PostCode, Country),
                GetStaffAddress(out staffAddressID, DateTime.Today.AddDays(2), "H", addressID, staffId, DateTime.Today.AddDays(3))
                ))
            {
                LoginAndNavigate(SeleniumHelper.iSIMSUserType.PersonnelOfficer, "Addresses");

                //Get staff record
                var staffRecordPage = LoadStaff(staffId);
                staffRecordPage.SelectAddressesTab();
                var gridRow = staffRecordPage.AddressTable.Rows[0];
                gridRow.ClickEditAddress();

                AddAddressDialog addAddress = new AddAddressDialog();
                addAddress.District = newLocality;
                addAddress.ClickOk();
                // co residents match dialog
                var matchesDialog = new SharedAddressDetailsMatchesDialog();
                matchesDialog.Matches.Rows[0].Selected = true;
                matchesDialog.ClickSave(coresidentMatchedAutomationID);
                //staffRecordPage.ClickSave();

                staffRecordPage = new StaffRecordPage();
                staffRecordPage.SelectAddressesTab();
                gridRow = staffRecordPage.AddressTable.Rows[0];
                gridRow.ClickEditAddress();

                addAddress = new AddAddressDialog();
                Assert.AreEqual(newLocality, addAddress.District);
            }
        }

        [TestMethod]
        [ChromeUiTest("StaffAddress", "P1", "Edit", "Edit_Staff_Address_New_Address")]
        [Variant(Variant.EnglishStatePrimary | Variant.EnglishStateSecondary | Variant.EnglishStateMultiphase |
            Variant.WelshStatePrimary | Variant.WelshStateSecondary | Variant.WelshStateMultiphase)]
        public void Edit_Staff_Address_New_Address()
        {
            #region Arrange

            Guid staffId = Guid.NewGuid();
            Guid addressID, staffAddressID;

            string forename = Utilities.GenerateRandomString(6, "Selenium");
            string surname = Utilities.GenerateRandomString(6, "Selenium");

            string UPRN = Utilities.GenerateRandomString(6);
            string PAONDescription = Utilities.GenerateRandomString(6);
            string PAONRange = Utilities.GenerateRandomString(6);
            string SAON = Utilities.GenerateRandomString(6);
            string Street = Utilities.GenerateRandomString(6);
            string Locality = Utilities.GenerateRandomString(6);
            string Town = Utilities.GenerateRandomString(6);
            string AdministrativeArea = Utilities.GenerateRandomString(6);
            string PostCode = Utilities.GenerateRandomString(6);
            string Country = "United Kingdom";

            string WAVPAONRange = "22";
            string WAVStreet = "SUDELEY WALK";
            string WAVTown = "BEDFORD";
            string WAVPostCode = "MK41 8HS";

            #endregion

            using (new DataSetup(
                GetStaffRecord_current(staffId, forename, surname),
                GetAddress(out addressID, UPRN, PAONDescription, PAONRange, SAON, Street, Locality, Town, AdministrativeArea, PostCode, Country),
                GetStaffAddress(out staffAddressID, DateTime.Today, "H", addressID, staffId)))
            {
                LoginAndNavigate(SeleniumHelper.iSIMSUserType.PersonnelOfficer, "Addresses");

                //Get staff record
                var staffRecordPage = LoadStaff(staffId);
                staffRecordPage.SelectAddressesTab();
                var gridRow = staffRecordPage.AddressTable.Rows[0];
                gridRow.ClickEditAddress();

                AddAddressDialog addAddress = new AddAddressDialog();
                addAddress.PAONRangeSearch = WAVPAONRange;
                addAddress.PostCodeSearch = WAVPostCode;
                addAddress.ClickSearch();
                addAddress.Street = WAVStreet;
                addAddress.Town = WAVTown;
                addAddress.PostCode = WAVPostCode;

                addAddress.ClickOk();
                staffRecordPage.ClickSave();

                staffRecordPage = new StaffRecordPage();
                staffRecordPage.SelectAddressesTab();
                gridRow = staffRecordPage.AddressTable.Rows[0];
                gridRow.ClickEditAddress();

                addAddress = new AddAddressDialog();

                Assert.AreEqual(WAVPAONRange, addAddress.BuildingNo);
                Assert.AreEqual(WAVStreet, addAddress.Street);
                Assert.AreEqual(WAVTown, addAddress.Town);
                Assert.AreEqual(WAVPostCode, addAddress.PostCode);
            }
        }

        #endregion

        #region Delete Address

        [TestMethod]
        [ChromeUiTest("StaffAddress", "P1", "Delete", "Delete_Staff_Address")]
        [Variant(Variant.NorthernIrelandStatePrimary | Variant.NorthernIrelandStateSecondary | Variant.NorthernIrelandStateMultiphase |
            Variant.IndependentPrimary | Variant.IndependentSecondary | Variant.IndependentMultiphase |
            Variant.EnglishStatePrimary | Variant.EnglishStateSecondary | Variant.EnglishStateMultiphase |
            Variant.WelshStatePrimary | Variant.WelshStateSecondary | Variant.WelshStateMultiphase)]
        public void Delete_Staff_Address()
        {
            #region Arrange

            Guid staffId = Guid.NewGuid();
            Guid addressID, staffAddressID;

            string forename = Utilities.GenerateRandomString(6, "Selenium");
            string surname = Utilities.GenerateRandomString(6, "Selenium");

            string UPRN = Utilities.GenerateRandomString(6);
            string PAONDescription = Utilities.GenerateRandomString(6);
            string PAONRange = Utilities.GenerateRandomString(6);
            string SAON = Utilities.GenerateRandomString(6);
            string Street = Utilities.GenerateRandomString(6);
            string Locality = Utilities.GenerateRandomString(6);
            string Town = Utilities.GenerateRandomString(6);
            string AdministrativeArea = Utilities.GenerateRandomString(6);
            string PostCode = Utilities.GenerateRandomString(6);
            string Country = "United Kingdom";

            string newLocality = Utilities.GenerateRandomString(6);

            #endregion

            using (new DataSetup(
                GetStaffRecord_current(staffId, forename, surname),
                GetAddress(out addressID, UPRN, PAONDescription, PAONRange, SAON, Street, Locality, Town, AdministrativeArea, PostCode, Country),
                GetStaffAddress(out staffAddressID, DateTime.Today, "H", addressID, staffId)))
            {
                LoginAndNavigate(SeleniumHelper.iSIMSUserType.PersonnelOfficer, "Addresses");

                //Get staff record
                var staffRecordPage = LoadStaff(staffId);
                staffRecordPage.SelectAddressesTab();
                var gridRow = staffRecordPage.AddressTable.Rows[0];
                gridRow.DeleteRow();

                staffRecordPage.ClickSave();

                staffRecordPage = new StaffRecordPage();
                staffRecordPage.SelectAddressesTab();

                int count = staffRecordPage.AddressTable.Rows.Count;

                Assert.AreEqual(0, count);
            }
        }

        #endregion

        #region Move Address

        [TestMethod]
        [ChromeUiTest("StaffAddress", "P1", "Move", "Move_Staff_Address")]
        [Variant(Variant.EnglishStatePrimary | Variant.EnglishStateSecondary | Variant.EnglishStateMultiphase |
            Variant.WelshStatePrimary | Variant.WelshStateSecondary | Variant.WelshStateMultiphase)]
        public void Move_Staff_Address()
        {
            #region Arrange

            Guid staffId = Guid.NewGuid();
            Guid addressID, staffAddressID;

            string forename = Utilities.GenerateRandomString(6, "Selenium");
            string surname = Utilities.GenerateRandomString(6, "Selenium");

            string UPRN = Utilities.GenerateRandomString(6);
            string PAONDescription = Utilities.GenerateRandomString(6);
            string PAONRange = Utilities.GenerateRandomString(6);
            string SAON = Utilities.GenerateRandomString(6);
            string Street = Utilities.GenerateRandomString(6);
            string Locality = Utilities.GenerateRandomString(6);
            string Town = Utilities.GenerateRandomString(6);
            string AdministrativeArea = Utilities.GenerateRandomString(6);
            string PostCode = Utilities.GenerateRandomString(6);
            string Country = "United Kingdom";

            string WAVPAONRange = "22";
            string WAVPostCode = "MK41 8HS";

            DateTime moveDate = DateTime.Today.AddDays(5);

            #endregion

            using (new DataSetup(
                GetStaffRecord_current(staffId, forename, surname),
                GetAddress(out addressID, UPRN, PAONDescription, PAONRange, SAON, Street, Locality, Town, AdministrativeArea, PostCode, Country),
                GetStaffAddress(out staffAddressID, DateTime.Today, "H", addressID, staffId)))
            {
                LoginAndNavigate(SeleniumHelper.iSIMSUserType.PersonnelOfficer, "Addresses");

                //Get staff record
                var staffRecordPage = LoadStaff(staffId);
                staffRecordPage.SelectAddressesTab();
                var gridRow = staffRecordPage.AddressTable.Rows[0];
                gridRow.ClickMoveAddress();

                AddAddressDialog addAddress = new AddAddressDialog();
                addAddress.MoveDate = moveDate.ToShortDateString();
                addAddress.PAONRangeSearch = WAVPAONRange;
                addAddress.PostCodeSearch = WAVPostCode;
                addAddress.ClickSearch();

                addAddress.ClickOk();
                staffRecordPage.ClickSave();

                staffRecordPage = new StaffRecordPage();
                staffRecordPage.SelectAddressesTab();

                gridRow = staffRecordPage.AddressTable.Rows[0];
                var newGridRow = staffRecordPage.AddressTable.Rows[1];

                Assert.AreEqual(moveDate.AddDays(-1).ToShortDateString(), gridRow.EndDate);
                Assert.AreEqual(moveDate.ToShortDateString(), newGridRow.StartDate);
            }
        }

        #endregion

        #endregion

        #region Staff Contact Address
        [TestMethod]
        [ChromeUiTest("StaffContactAddress", "P1", "Add", "Add_Staff_Contact_Address_Local")]
        [Variant(Variant.NorthernIrelandStatePrimary | Variant.NorthernIrelandStateSecondary | Variant.NorthernIrelandStateMultiphase |
            Variant.IndependentPrimary | Variant.IndependentSecondary | Variant.IndependentMultiphase)]
        public void Add_Staff_Contact_Address_Local()
        {
            #region IDs

            Guid staffID, staffContactID, staffContactRelationshipID;
            Guid addressID;

            #endregion

            #region Values

            string staffForename = CoreQueries.GetColumnUniqueString("Staff", "LegalForename", 10, Environment.Settings.TenantId);
            string staffSurname = CoreQueries.GetColumnUniqueString("Staff", "LegalSurname", 10, Environment.Settings.TenantId);
            string staffContactSurname = CoreQueries.GetColumnUniqueString("StaffContact", "Surname", 10, Environment.Settings.TenantId);
            string UPRN = Utilities.GenerateRandomString(6);
            string PAONDescription = Utilities.GenerateRandomString(6);
            string PAONRange = Utilities.GenerateRandomString(6);
            string SAON = Utilities.GenerateRandomString(6);
            string Street = Utilities.GenerateRandomString(6);
            string Locality = Utilities.GenerateRandomString(6);
            string Town = Utilities.GenerateRandomString(6);
            string AdministrativeArea = Utilities.GenerateRandomString(6);
            string PostCode = Utilities.GenerateRandomString(6);
            string Country = "United Kingdom";

            const string _space = " ";
            const string _seperator = ", ";
            const string _lineSeperator = "\r\n";

            var addressDisplaySmall = string.Concat(SAON, _seperator,
                PAONDescription, _seperator,
                PAONRange, _seperator, Street, _seperator, Town);

            var addressDisplayLarge = string.Concat(SAON, _seperator,
                PAONRange, _lineSeperator,
                PAONDescription, _lineSeperator, Street, _lineSeperator,
                Locality, _lineSeperator,
                Town, _lineSeperator,
                AdministrativeArea, _lineSeperator,
                PostCode, _lineSeperator,
                Country);

            #endregion

            #region Data

            DataPackage package = new DataPackage();
            package.AddData("Staff", DataPackageHelper.GenerateStaff(out staffID, surname: staffSurname, forename: staffForename));
            package.AddData("StaffContact", DataPackageHelper.GenerateStaffContact(out staffContactID, surname: staffContactSurname));
            package.AddData("StaffContactRelationship", DataPackageHelper.GenerateStaffContactRelationship(out staffContactRelationshipID, staffID: staffID, staffContactID: staffContactID));

            #endregion

            //Act
            using (new DataSetup(package,
                GetAddress(out addressID, UPRN, PAONDescription, PAONRange, SAON, Street, Locality, Town, AdministrativeArea, PostCode, Country)))
            {
                LoginAndNavigate(SeleniumHelper.iSIMSUserType.PersonnelOfficer, "Addresses");
                StaffRecordPage staff = LoadStaff(staffID);

                //Update Staff Contact Telephone
                staff.SelectNextOfKinTab();
                staff.ContactTable.Rows[0].ClickEdit();
                var contactDialog = new EditStaffContactDialog();
                var findStaffContactAddressSection = By.CssSelector("#dialog-editableData [data-automation-id='section_menu_Addresses']");
                var clickAddStaffContactAddress = By.CssSelector("#dialog-editableData [data-automation-id='add_address_button']");
                SeleniumHelper.FindAndClick(findStaffContactAddressSection);
                SeleniumHelper.FindAndClick(clickAddStaffContactAddress);
                AddAddressDialog addAddress = new AddAddressDialog();
                addAddress.PAONRangeSearch = PAONRange;
                addAddress.PostCodeSearch = PostCode;
                addAddress.ClickSearch();

                addAddress.Addresses = addressDisplaySmall;

                Assert.AreEqual(PAONDescription, addAddress.BuildingName);
                Assert.AreEqual(PAONDescription, addAddress.BuildingName);
                Assert.AreEqual(PAONRange, addAddress.BuildingNo);
                Assert.AreEqual(SAON, addAddress.Flat);
                Assert.AreEqual(Street, addAddress.Street);
                Assert.AreEqual(Locality, addAddress.District);
                Assert.AreEqual(Town, addAddress.Town);
                Assert.AreEqual(AdministrativeArea, addAddress.County);
                Assert.AreEqual(PostCode, addAddress.PostCode);
                Assert.AreEqual(Country, addAddress.Country);

                addAddress.ClickOk();
                contactDialog.ClickOk();
                staff.ClickSave();

                staff = new StaffRecordPage();
                staff.SelectNextOfKinTab();
                staff.ContactTable.Rows[0].ClickEdit();
                findStaffContactAddressSection = By.CssSelector("#dialog-editableData [data-automation-id='section_menu_Addresses']");
                clickAddStaffContactAddress = By.CssSelector("#dialog-editableData [data-automation-id='add_address_button']");
                SeleniumHelper.FindAndClick(findStaffContactAddressSection);
                var gridRow = contactDialog.StaffContactAddressTable.Rows[0];
                Assert.AreEqual(gridRow.Address, addressDisplayLarge);
            }
        }

        [TestMethod]
        [ChromeUiTest("StaffContactAddress", "P1", "Add", "Add_Staff_Contact_Address_WAV")]
        [Variant(Variant.EnglishStatePrimary | Variant.EnglishStateSecondary | Variant.EnglishStateMultiphase |
            Variant.WelshStatePrimary | Variant.WelshStateSecondary | Variant.WelshStateMultiphase)]
        public void Add_Staff_Contact_Address_WAV()
        {
            #region IDs

            Guid staffID, staffContactID, staffContactRelationshipID;

            #endregion

            #region Values

            string staffForename = CoreQueries.GetColumnUniqueString("Staff", "LegalForename", 10, Environment.Settings.TenantId);
            string staffSurname = CoreQueries.GetColumnUniqueString("Staff", "LegalSurname", 10, Environment.Settings.TenantId);
            string staffContactSurname = CoreQueries.GetColumnUniqueString("StaffContact", "Surname", 10, Environment.Settings.TenantId);

            string PAONRange = "22";
            string Street = "SUDELEY WALK";
            string Town = "BEDFORD";
            string PostCode = "MK41 8HS";
            string Country = "United Kingdom";

            const string _space = " ";
            const string _seperator = ", ";
            const string _lineSeperator = "\r\n";
            var addressDisplaySmall = string.Concat(
                          PAONRange, _seperator, Street, _seperator, Town);

            var addressDisplayLarge = string.Concat(
                PAONRange, _lineSeperator, Street, _lineSeperator,
                Town, _lineSeperator,
                PostCode, _lineSeperator,
                Country);

            #endregion

            #region Data

            DataPackage package = new DataPackage();
            package.AddData("Staff", DataPackageHelper.GenerateStaff(out staffID, surname: staffSurname, forename: staffForename));
            package.AddData("StaffContact", DataPackageHelper.GenerateStaffContact(out staffContactID, surname: staffContactSurname));
            package.AddData("StaffContactRelationship", DataPackageHelper.GenerateStaffContactRelationship(out staffContactRelationshipID, staffID: staffID, staffContactID: staffContactID));

            #endregion

            //Act
            using (new DataSetup(package))
            {
                LoginAndNavigate(SeleniumHelper.iSIMSUserType.PersonnelOfficer, "Addresses");
                StaffRecordPage staff = LoadStaff(staffID);

                //Update Staff Contact Telephone
                staff.SelectNextOfKinTab();
                staff.ContactTable.Rows[0].ClickEdit();
                var contactDialog = new EditStaffContactDialog();
                var findStaffContactAddressSection = By.CssSelector("#dialog-editableData [data-automation-id='section_menu_Addresses']");
                var clickAddStaffContactAddress = By.CssSelector("#dialog-editableData [data-automation-id='add_address_button']");
                SeleniumHelper.FindAndClick(findStaffContactAddressSection);
                SeleniumHelper.FindAndClick(clickAddStaffContactAddress);
                AddAddressDialog addAddress = new AddAddressDialog();
                addAddress.PAONRangeSearch = PAONRange;
                addAddress.PostCodeSearch = PostCode;
                addAddress.ClickSearch();

                addAddress.Addresses = addressDisplaySmall;

                Assert.AreEqual(PAONRange, addAddress.BuildingNo);
                Assert.AreEqual(Street, addAddress.Street);
                Assert.AreEqual(Town, addAddress.Town);
               // Assert.AreEqual(Town, addAddress.County);
                Assert.AreEqual(PostCode, addAddress.PostCode);

                addAddress.ClickOk();
                contactDialog.ClickOk();
                staff.ClickSave();

                staff = new StaffRecordPage();
                staff.SelectNextOfKinTab();
                staff.ContactTable.Rows[0].ClickEdit();
                findStaffContactAddressSection = By.CssSelector("#dialog-editableData [data-automation-id='section_menu_Addresses']");
                clickAddStaffContactAddress = By.CssSelector("#dialog-editableData [data-automation-id='add_address_button']");
                SeleniumHelper.FindAndClick(findStaffContactAddressSection);
                var gridRow = contactDialog.StaffContactAddressTable.Rows[0];
                Assert.AreEqual(gridRow.Address, addressDisplayLarge);
               
            }
        }

        #endregion

        #region Edit Staff Contact Address

        [TestMethod]
        [ChromeUiTest("StaffContactAddress", "P1", "Edit", "Edit_Staff_Contact_Address_Fields")]
        [Variant(Variant.NorthernIrelandStatePrimary | Variant.NorthernIrelandStateSecondary | Variant.NorthernIrelandStateMultiphase |
            Variant.IndependentPrimary | Variant.IndependentSecondary | Variant.IndependentMultiphase |
            Variant.EnglishStatePrimary | Variant.EnglishStateSecondary | Variant.EnglishStateMultiphase |
            Variant.WelshStatePrimary | Variant.WelshStateSecondary | Variant.WelshStateMultiphase)]
        public void Edit_Staff_Contact_Address_Fields()
        {
            #region IDs

            Guid staffID, staffContactID, staffContactRelationshipID;
            Guid addressID, staffAddressID;

            #endregion

            #region Values

            string staffForename = CoreQueries.GetColumnUniqueString("Staff", "LegalForename", 10, Environment.Settings.TenantId);
            string staffSurname = CoreQueries.GetColumnUniqueString("Staff", "LegalSurname", 10, Environment.Settings.TenantId);
            string staffContactSurname = CoreQueries.GetColumnUniqueString("StaffContact", "Surname", 10, Environment.Settings.TenantId);
            string UPRN = Utilities.GenerateRandomString(6);
            string PAONDescription = Utilities.GenerateRandomString(6);
            string PAONRange = Utilities.GenerateRandomString(6);
            string SAON = Utilities.GenerateRandomString(6);
            string Street = Utilities.GenerateRandomString(6);
            string Locality = Utilities.GenerateRandomString(6);
            string Town = Utilities.GenerateRandomString(6);
            string AdministrativeArea = Utilities.GenerateRandomString(6);
            string PostCode = Utilities.GenerateRandomString(6);
            string Country = "United Kingdom";

            string newLocality = Utilities.GenerateRandomString(6);

            const string _space = " ";
            const string _seperator = ", ";
            const string _lineSeperator = "\r\n";

            var addressDisplaySmall = string.Concat(SAON, _seperator,
                PAONDescription, _seperator,
                PAONRange, _seperator, Street, _seperator, Town);

            var addressDisplayLarge = string.Concat(SAON, _seperator,
                PAONDescription, _lineSeperator,
                PAONRange, _space, Street, _lineSeperator,
                Locality, _lineSeperator,
                Town, _lineSeperator,
                AdministrativeArea, _lineSeperator,
                PostCode, _lineSeperator,
                Country);

            #endregion

            #region Data

            DataPackage package = new DataPackage();
            package.AddData("Staff", DataPackageHelper.GenerateStaff(out staffID, surname: staffSurname, forename: staffForename));
            package.AddData("StaffContact", DataPackageHelper.GenerateStaffContact(out staffContactID, surname: staffContactSurname));
            package.AddData("StaffContactRelationship", DataPackageHelper.GenerateStaffContactRelationship(out staffContactRelationshipID, staffID: staffID, staffContactID: staffContactID));

            #endregion

            //Act
            using (new DataSetup(package,
                 GetAddress(out addressID, UPRN, PAONDescription, PAONRange, SAON, Street, Locality, Town, AdministrativeArea, PostCode, Country),
                GetStaffContactAddress(out staffAddressID, staffContactID, DateTime.Today, "H", addressID, staffID)))
            {
                LoginAndNavigate(SeleniumHelper.iSIMSUserType.PersonnelOfficer, "Addresses");
                StaffRecordPage staff = LoadStaff(staffID);
                //Update Staff Contact address
                staff.SelectNextOfKinTab();
                staff.ContactTable.Rows[0].ClickEdit();
                var contactDialog = new EditStaffContactDialog();
                var findStaffContactAddressSection = By.CssSelector("#dialog-editableData [data-automation-id='section_menu_Addresses']");
                var clickAddStaffContactAddress = By.CssSelector("#dialog-editableData [data-automation-id='add_address_button']");
                SeleniumHelper.FindAndClick(findStaffContactAddressSection);
                var gridRow = contactDialog.StaffContactAddressTable.Rows[0];
                gridRow.ClickEditStaffContactAddress();

                AddAddressDialog addAddress = new AddAddressDialog();
                addAddress.District = newLocality;
                addAddress.ClickOk();
                contactDialog.ClickOk();
                staff.ClickSave();

                staff = new StaffRecordPage();
                staff.SelectNextOfKinTab();
                staff.ContactTable.Rows[0].ClickEdit();
                findStaffContactAddressSection = By.CssSelector("#dialog-editableData [data-automation-id='section_menu_Addresses']");
                clickAddStaffContactAddress = By.CssSelector("#dialog-editableData [data-automation-id='add_address_button']");
                SeleniumHelper.FindAndClick(findStaffContactAddressSection);
                gridRow = contactDialog.StaffContactAddressTable.Rows[0];
                gridRow.ClickEditStaffContactAddress();

                addAddress = new AddAddressDialog();
                Assert.AreEqual(newLocality, addAddress.District);
            }


        }

        [TestMethod]
        [ChromeUiTest("StaffContactAddress", "P1", "Edit", "Edit_Staff_Contact_Address")]
        [Variant(Variant.EnglishStatePrimary | Variant.EnglishStateSecondary | Variant.EnglishStateMultiphase |
            Variant.WelshStatePrimary | Variant.WelshStateSecondary | Variant.WelshStateMultiphase)]
        public void Edit_Staff_Contact_Address()
        {
            #region IDs

            Guid staffID, staffContactID, staffContactRelationshipID;
            Guid addressID, staffAddressID;

            #endregion

            #region Values

            string staffForename = CoreQueries.GetColumnUniqueString("Staff", "LegalForename", 10, Environment.Settings.TenantId);
            string staffSurname = CoreQueries.GetColumnUniqueString("Staff", "LegalSurname", 10, Environment.Settings.TenantId);
            string staffContactSurname = CoreQueries.GetColumnUniqueString("StaffContact", "Surname", 10, Environment.Settings.TenantId);
            string UPRN = Utilities.GenerateRandomString(6);
            string PAONDescription = Utilities.GenerateRandomString(6);
            string PAONRange = Utilities.GenerateRandomString(6);
            string SAON = Utilities.GenerateRandomString(6);
            string Street = Utilities.GenerateRandomString(6);
            string Locality = Utilities.GenerateRandomString(6);
            string Town = Utilities.GenerateRandomString(6);
            string AdministrativeArea = Utilities.GenerateRandomString(6);
            string PostCode = Utilities.GenerateRandomString(6);
            string Country = "United Kingdom";

            string WAVPAONRange = "22";
            string WAVStreet = "SUDELEY WALK";
            string WAVTown = "BEDFORD";
            string WAVPostCode = "MK41 8HS";

            #endregion

            #region Data

            DataPackage package = new DataPackage();
            package.AddData("Staff", DataPackageHelper.GenerateStaff(out staffID, surname: staffSurname, forename: staffForename));
            package.AddData("StaffContact", DataPackageHelper.GenerateStaffContact(out staffContactID, surname: staffContactSurname));
            package.AddData("StaffContactRelationship", DataPackageHelper.GenerateStaffContactRelationship(out staffContactRelationshipID, staffID: staffID, staffContactID: staffContactID));

            #endregion

            //Act
            using (new DataSetup(package,
                 GetAddress(out addressID, UPRN, PAONDescription, PAONRange, SAON, Street, Locality, Town, AdministrativeArea, PostCode, Country),
                GetStaffContactAddress(out staffAddressID, staffContactID, DateTime.Today, "H", addressID, staffID)))
            {
                LoginAndNavigate(SeleniumHelper.iSIMSUserType.PersonnelOfficer, "Addresses");
                StaffRecordPage staff = LoadStaff(staffID);
                //Update Staff Contact address
                staff.SelectNextOfKinTab();
                staff.ContactTable.Rows[0].ClickEdit();
                var contactDialog = new EditStaffContactDialog();
                var findStaffContactAddressSection = By.CssSelector("#dialog-editableData [data-automation-id='section_menu_Addresses']");
                var clickAddStaffContactAddress = By.CssSelector("#dialog-editableData [data-automation-id='add_address_button']");
                SeleniumHelper.FindAndClick(findStaffContactAddressSection);
                var gridRow = contactDialog.StaffContactAddressTable.Rows[0];
                gridRow.ClickEditStaffContactAddress();

                AddAddressDialog addAddress = new AddAddressDialog();
                addAddress.PAONRangeSearch = WAVPAONRange;
                addAddress.PostCodeSearch = WAVPostCode;
                addAddress.ClickSearch();
                addAddress.ClickOk();

                // co residents match dialog
                var matchesDialog = new SharedAddressDetailsMatchesDialog();
                matchesDialog.ClickSave(coresidentMatchedAutomationID);

                staff = new StaffRecordPage();
                staff.SelectNextOfKinTab();
                staff.ContactTable.Rows[0].ClickEdit();
                findStaffContactAddressSection = By.CssSelector("#dialog-editableData [data-automation-id='section_menu_Addresses']");
                clickAddStaffContactAddress = By.CssSelector("#dialog-editableData [data-automation-id='add_address_button']");
                SeleniumHelper.FindAndClick(findStaffContactAddressSection);
                gridRow = contactDialog.StaffContactAddressTable.Rows[0];
                gridRow.ClickEditStaffContactAddress();

                addAddress = new AddAddressDialog();
                Assert.AreEqual(WAVPAONRange, addAddress.BuildingNo);
                Assert.AreEqual(WAVStreet, addAddress.Street);
                Assert.AreEqual(WAVTown, addAddress.Town);
                Assert.AreEqual(WAVPostCode, addAddress.PostCode);
            }
        }

        #endregion

        #region Staff QTS

        [TestMethod]
        [ChromeUiTest(new[] { "9611", "QualifiedTeacherStatus", "P1", "Staff", "PersonnelOfficer" })]
        public void Can_assign_qualified_teacher_status_to_Staff_as_PO()
        {
            //Arrange
            Guid Id = Guid.NewGuid();
            string forename = CoreQueries.GetColumnUniqueString("Staff", "LegalForename", 10, Environment.Settings.TenantId);
            string surname = CoreQueries.GetColumnUniqueString("Staff", "LegalSurname", 10, Environment.Settings.TenantId);

            using (new DataSetup(GetStaffRecord_current(Id, forename, surname)))
            {
                //Act
                LoginAndNavigate(SeleniumHelper.iSIMSUserType.PersonnelOfficer);
                StaffRecordPage staff = Search(surname);
                staff.QualifiedTeacherStatus = "No";
                staff.ClickSave();
                staff = LoadStaff(Id);

                //Assert
                Assert.AreEqual("No", staff.QualifiedTeacherStatus);
            }
        }

        [TestMethod]
        [ChromeUiTest(new[] { "9611", "QualifiedTeacherStatus", "P1", "Staff", "SchoolAdministrator" })]
        public void Can_assign_qualified_teacher_status_to_Staff_as_SA()
        {
            //Arrange
            Guid Id = Guid.NewGuid();
            string forename = CoreQueries.GetColumnUniqueString("Staff", "LegalForename", 10, Environment.Settings.TenantId);
            string surname = CoreQueries.GetColumnUniqueString("Staff", "LegalSurname", 10, Environment.Settings.TenantId);

            using (new DataSetup(GetStaffRecord_current(Id, forename, surname)))
            {
                //Act
                LoginAndNavigate(SeleniumHelper.iSIMSUserType.SchoolAdministrator);
                StaffRecordPage staff = Search(surname);
                staff.QualifiedTeacherStatus = "No";
                staff.ClickSave();
                staff = LoadStaff(Id);

                //Assert
                Assert.AreEqual("No", staff.QualifiedTeacherStatus);
            }
        }

        [TestMethod]
        [ChromeUiTest(new[] { "9611", "QualifiedTeacherStatus", "P1", "Staff", "PersonnelOfficer" })]
        public void Qualified_teacher_route_Enabled_if_Yes_as_PO()
        {
            //Arrange
            Guid Id = Guid.NewGuid();
            string forename = CoreQueries.GetColumnUniqueString("Staff", "LegalForename", 10, Environment.Settings.TenantId);
            string surname = CoreQueries.GetColumnUniqueString("Staff", "LegalSurname", 10, Environment.Settings.TenantId);

            using (new DataSetup(GetStaffRecord_current(Id, forename, surname)))
            {
                //Act
                LoginAndNavigate(SeleniumHelper.iSIMSUserType.PersonnelOfficer);
                StaffRecordPage staff = Search(surname);
                staff.QualifiedTeacherStatus = "Yes";

                //Assert
                Assert.IsTrue(staff.QTSRouteEnabled());
            }
        }

        [TestMethod]
        [ChromeUiTest(new[] { "9611", "QualifiedTeacherStatus", "P1", "Staff", "SchoolAdministrator" })]
        public void Qualified_teacher_route_Enabled_if_Yes_as_SA()
        {
            //Arrange
            Guid Id = Guid.NewGuid();
            string forename = CoreQueries.GetColumnUniqueString("Staff", "LegalForename", 10, Environment.Settings.TenantId);
            string surname = CoreQueries.GetColumnUniqueString("Staff", "LegalSurname", 10, Environment.Settings.TenantId);

            using (new DataSetup(GetStaffRecord_current(Id, forename, surname)))
            {
                //Act
                LoginAndNavigate(SeleniumHelper.iSIMSUserType.SchoolAdministrator);
                StaffRecordPage staff = Search(surname);
                staff.QualifiedTeacherStatus = "Yes";

                //Assert
                Assert.IsTrue(staff.QTSRouteEnabled());
            }
        }

        [TestMethod]
        [ChromeUiTest(new[] { "9611", "QualifiedTeacherRoute", "P1", "Staff", "PersonnelOfficer" })]
        public void Can_assign_qualified_teacher_route_to_Staff_as_PO()
        {
            //Arrange
            Guid Id = Guid.NewGuid();
            string forename = CoreQueries.GetColumnUniqueString("Staff", "LegalForename", 10, Environment.Settings.TenantId);
            string surname = CoreQueries.GetColumnUniqueString("Staff", "LegalSurname", 10, Environment.Settings.TenantId);

            string qtsRouteDescription = "Overseas Trained Teacher Programme";

            using (new DataSetup(GetStaffRecord_current(Id, forename, surname)))
            {
                //Act
                LoginAndNavigate(SeleniumHelper.iSIMSUserType.PersonnelOfficer);
                StaffRecordPage staff = Search(surname);
                staff.QualifiedTeacherStatus = "Yes";
                staff.QTSRoute = qtsRouteDescription;
                staff.ClickSave();
                staff = LoadStaff(Id);

                //Assert
                Assert.AreEqual("Yes", staff.QualifiedTeacherStatus);
                Assert.AreEqual(qtsRouteDescription, staff.QTSRoute);
            }
        }

        [TestMethod]
        [ChromeUiTest(new[] { "9611", "QualifiedTeacherRoute", "P1", "Staff", "SchoolAdministrator" })]
        public void Can_assign_qualified_teacher_route_to_Staff_as_SA()
        {
            //Arrange
            Guid Id = Guid.NewGuid();
            string forename = CoreQueries.GetColumnUniqueString("Staff", "LegalForename", 10, Environment.Settings.TenantId);
            string surname = CoreQueries.GetColumnUniqueString("Staff", "LegalSurname", 10, Environment.Settings.TenantId);

            string qtsRouteDescription = "Overseas Trained Teacher Programme";

            using (new DataSetup(GetStaffRecord_current(Id, forename, surname)))
            {
                //Act
                LoginAndNavigate(SeleniumHelper.iSIMSUserType.SchoolAdministrator);
                StaffRecordPage staff = Search(surname);
                staff.QualifiedTeacherStatus = "Yes";
                staff.QTSRoute = qtsRouteDescription;
                staff.ClickSave();
                staff = LoadStaff(Id);

                //Assert
                Assert.AreEqual("Yes", staff.QualifiedTeacherStatus);
                Assert.AreEqual(qtsRouteDescription, staff.QTSRoute);
            }
        }

        #endregion

        #region UDF Tests

        [TestMethod]
        [ChromeUiTest("StaffUDF", "P1", "Can_View_Edit_UDF")]
        public void Can_View_Edit_UDF()
        {
            string surname, udfTestValue;

            Guid staffId, udfId;

            surname = Utilities.GenerateRandomString(6, "Selenium");
            udfTestValue = "UDF Field Value";

            const string udfDomainDefinitionQuery = "SELECT TOP 1 ID FROM app.UDFDomainDefinition WHERE TenantID = {0} AND Code = '{1}'";
            const string udfFieldTypeQuery = "SELECT TOP 1 ID FROM app.UDFFieldType WHERE TenantID = {0} AND Code = '{1}'";

            var udfDomainDefinition = DataAccessHelpers.GetValue<Guid>(string.Format(udfDomainDefinitionQuery, Environment.Settings.TenantId, "STAFF:UDF"));
            var udfFieldType = DataAccessHelpers.GetValue<Guid>(string.Format(udfFieldTypeQuery, Environment.Settings.TenantId, "Text"));

            var package = GetStaffRecord_current(staffId = Guid.NewGuid(), "Test", surname);
            package.AddData("app.UDFDefinition", new
            {
                Id = udfId = Guid.NewGuid(),
                Description = "UDF Field",
                DisplayOrder = 1,
                IsVisible = true,
                ResourceProvider = CoreQueries.GetSchoolId(),
                TenantID = Environment.Settings.TenantId,
                UDFDomainDefinition = udfDomainDefinition,
                UDFFieldType = udfFieldType
            });

            using (new DataSetup(false, true, package))
            {
                SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.PersonnelOfficer, new [] { "UDFs" });
                AutomationSugar.NavigateMenu("Tasks", "Staff", "Staff Records");
                StaffRecordPage staff = Search(surname);
                staff.SelectUDFTab();

                var udfField = SimsBy.CssSelector("input[name*=\"UDFCollection[" + udfId + "]\"]");
                Assert.IsNotNull(udfField, "UDF Field is not found.");

                var wd = Environment.WebContext.WebDriver;
                var element = wd.FindElement(udfField);
                element.SendKeys(udfTestValue);
                element.SendKeys(Keys.Enter);

                AutomationSugar.WaitForAjaxCompletion();

                staff.ClickSave();
                staff = LoadStaff(staffId);

                var value = wd.FindElement(udfField).GetValue();
                Assert.AreEqual(udfTestValue, value);

                var sql = string.Format("DELETE FROM app.UDFValue WHERE UDFDefinition ='{0}'", udfId);
                DataAccessHelpers.Execute(sql);
            }
        }

        #endregion

        #region Data Packages

        private DataPackage GetStaffRecord_current(Guid staffId, string forename, string surname)
        {
            return this.BuildDataPackage()
               .AddData("Staff", new
               {
                   Id = staffId,
                   LegalForename = forename,
                   LegalSurname = surname,
                   LegalMiddleNames = "Middle",
                   PreferredForename = forename,
                   PreferredSurname = surname,
                   DateOfBirth = new DateTime(2000, 1, 1),
                   Gender = CoreQueries.GetLookupItem("Gender", description: "Male"),
                   PolicyACLID = CoreQueries.GetPolicyAclId("Staff"),
                   School = CoreQueries.GetSchoolId(),
                   TenantID = SeSugar.Environment.Settings.TenantId
               })
               .AddData("StaffServiceRecord", new
               {
                   Id = Guid.NewGuid(),
                   DOA = DateTime.Today.AddDays(-1),
                   ContinuousServiceStartDate = DateTime.Today.AddDays(-1),
                   LocalAuthorityStartDate = DateTime.Today.AddDays(-1),
                   Staff = staffId,
                   TenantID = SeSugar.Environment.Settings.TenantId
               });
        }

        private DataPackage GetStaffRecord_future(Guid staffId, string forename, string surname)
        {
            return this.BuildDataPackage()
               .AddData("Staff", new
               {
                   Id = staffId,
                   LegalForename = forename,
                   LegalSurname = surname,
                   LegalMiddleNames = "Middle",
                   PreferredForename = forename,
                   PreferredSurname = surname,
                   DateOfBirth = new DateTime(2000, 1, 1),
                   Gender = CoreQueries.GetLookupItem("Gender", description: "Male"),
                   PolicyACLID = CoreQueries.GetPolicyAclId("Staff"),
                   School = CoreQueries.GetSchoolId(),
                   TenantID = SeSugar.Environment.Settings.TenantId
               })
               .AddData("StaffServiceRecord", new
               {
                   Id = Guid.NewGuid(),
                   DOA = DateTime.Today.AddDays(10),
                   ContinuousServiceStartDate = DateTime.Today.AddDays(10),
                   LocalAuthorityStartDate = DateTime.Today.AddDays(10),
                   Staff = staffId,
                   TenantID = SeSugar.Environment.Settings.TenantId
               });
        }

        private DataPackage GetStaffRecord_former(Guid staffId, string forename, string surname)
        {
            return this.BuildDataPackage()
               .AddData("Staff", new
               {
                   Id = staffId,
                   LegalForename = forename,
                   LegalSurname = surname,
                   LegalMiddleNames = "Middle",
                   PreferredForename = forename,
                   PreferredSurname = surname,
                   DateOfBirth = new DateTime(2000, 1, 1),
                   Gender = CoreQueries.GetLookupItem("Gender", description: "Male"),
                   PolicyACLID = CoreQueries.GetPolicyAclId("Staff"),
                   School = CoreQueries.GetSchoolId(),
                   TenantID = SeSugar.Environment.Settings.TenantId
               })
               .AddData("StaffServiceRecord", new
               {
                   Id = Guid.NewGuid(),
                   DOA = DateTime.Today.AddDays(-100),
                   DOL = DateTime.Today.AddDays(-50),
                   ContinuousServiceStartDate = DateTime.Today.AddDays(-100),
                   LocalAuthorityStartDate = DateTime.Today.AddDays(-100),
                   Staff = staffId,
                   TenantID = SeSugar.Environment.Settings.TenantId
               });
        }

        private DataPackage GetQTSRoute(string code, string description, string displayOrder)
        {
            return this.BuildDataPackage()
                .AddData("QTSRoute", new
                {
                    ID = Guid.NewGuid(),
                    Code = code,
                    Description = description,
                    DisplayOrder = displayOrder,
                    IsVisible = true,
                    ResourceProvider = CoreQueries.GetSchoolId(),
                    TenantID = Environment.Settings.TenantId
                });
        }

        private DataPackage GetAddress(out Guid addressID, string UPRN,
                            string PAONDescription,
                            string PAONRange,
                            string SAON,
                            string Street,
                            string Locality,
                            string Town,
                            string AdministrativeArea,
                            string PostCode,
                            string Country)
        {
            return this.BuildDataPackage()
                .AddData("Address", new
                {
                    ID = addressID = Guid.NewGuid(),
                    UPRN = UPRN,
                    PAONDescription = PAONDescription,
                    PAONRange = PAONRange,
                    SAON = SAON,
                    Street = Street,
                    Locality = Locality,
                    Town = Town,
                    AdministrativeArea = AdministrativeArea,
                    PostCode = PostCode,
                    Country = CoreQueries.GetLookupItem("Country", description: Country),
                    ResourceProvider = CoreQueries.GetSchoolId(),
                    TenantID = Environment.Settings.TenantId
                });
        }

        private DataPackage GetStaffAddress(out Guid staffAddressID, DateTime startDate, string addressTypeCode, Guid address, Guid staffID, DateTime? endDate = null)
        {
            return this.BuildDataPackage()
            .AddData("StaffAddress", new
             {
                ID = staffAddressID = Guid.NewGuid(),
                StartDate = startDate,
                EndDate = endDate,
                AddressType = CoreQueries.GetLookupItem("AddressType", code: addressTypeCode),
                Address = address,
                Staff = staffID,
                TenantID = Environment.Settings.TenantId
             });
        }

        private DataPackage GetStaffContactAddress(out Guid staffAddressID, Guid staffContactID, DateTime startDate, string addressTypeCode, Guid address, Guid staffID, DateTime? endDate = null)
        {
            return this.BuildDataPackage()
            .AddData("StaffContactAddress", new
            {
                ID = staffAddressID = Guid.NewGuid(),
                StartDate = startDate,
                EndDate = endDate,
                StaffContact = staffContactID,
                AddressType = CoreQueries.GetLookupItem("AddressType", code: addressTypeCode),
                Address = address,
                TenantID = Environment.Settings.TenantId
            });
        }

        #endregion

        #region Support
        private static string GetHashString()
        {
            HashAlgorithm algorithm = MD5.Create();
            byte[] inputStringHash = algorithm.ComputeHash(Encoding.UTF8.GetBytes(Guid.NewGuid().ToString()));
            StringBuilder sb = new StringBuilder();
            foreach (byte b in inputStringHash)
                sb.Append(b.ToString("X2"));

            return sb.ToString();
        }

        /// <summary>
        /// Logs the user in for the given profile and navigates to the Staff search screen
        /// </summary>
        /// <param name="userType"></param>
        private void LoginAndNavigate(SeleniumHelper.iSIMSUserType userType, string enabledFeatures = null)
        {
            if(string.IsNullOrEmpty(enabledFeatures))
            {
                SeleniumHelper.Login(userType);
            }
            else
            {
                SeleniumHelper.Login(userType, enabledFeatures: enabledFeatures);
            }
            
            AutomationSugar.NavigateMenu("Tasks", "Staff", "Staff Records");
        }

        private StaffRecordPage GetStaffRecord(StaffRecordData staffRecordData)
        {
            return StaffRecordPage.LoadStaffDetail(staffRecordData.StaffId);
        }

        private static StaffRecordPage LoadStaff(Guid staffId)
        {
            return StaffRecordPage.LoadStaffDetail(staffId);
        }

        private static StaffRecordPage Search(string surname)
        {
            StaffRecordTriplet staffRecordTriplet = new StaffRecordTriplet();
            staffRecordTriplet.SearchCriteria.StaffName = surname;
            var result = staffRecordTriplet.SearchCriteria.Search();
            StaffRecordPage staffRecord = result.SingleOrDefault(x => true).Click<StaffRecordPage>();
            return staffRecord;
        }

        private bool CheckStaffDetailsMatch(StaffRecordData staffRecordData, StaffRecordPage staffRecordPage)
        {
            staffRecordPage.Refresh();

            Assert.AreEqual(staffRecordPage.LegalForeName, staffRecordData.LegalForename, "LegalForeNames do not match.");
            Assert.AreEqual(staffRecordPage.LegalSurname, staffRecordData.LegalSurname, "LegalForeNames do not match.");
            Assert.AreEqual(staffRecordPage.MiddleName, staffRecordData.LegalMiddleNames, "LegalMiddleNames do not match.");
            Assert.AreEqual(staffRecordPage.PreferForeName, staffRecordData.PreferredForname, "PreferredForname do not match.");
            Assert.AreEqual(staffRecordPage.PreferSurname, staffRecordData.PreferredSurname, "PreferredSurname do not match.");
            Assert.AreEqual(staffRecordPage.QuickNote, staffRecordData.QuickNote, "QuickNote do not match.");
            Assert.AreEqual(staffRecordPage.DOB, staffRecordData.DateOfBirth.ToShortDateString(), "DateOfBirth do not match.");
            Assert.AreEqual(staffRecordPage.Gender, staffRecordData.GenderItem.Description, "Gender do not match.");
            Assert.AreEqual(staffRecordPage.Title, staffRecordData.TitleItem.Description, "Title do not match.");
            Assert.AreEqual(staffRecordPage.MaritalStatus, staffRecordData.MaritalStatusItem.Description, "MaritalStatus do not match.");

            return true;
        }
        #endregion

        internal class StaffRecordData
        {
            public string LegalForename = Utilities.GenerateRandomString(6, "Selenium");
            public string LegalSurname = Utilities.GenerateRandomString(6, "Selenium");
            public string LegalMiddleNames = Utilities.GenerateRandomString(6, "Selenium");
            public string QuickNote = Utilities.GenerateRandomString(6, "Selenium");
            public string PreferredForname;
            public string PreferredSurname;
            public LookupItem TitleItem = null;
            public LookupItem MaritalStatusItem = null;
            public LookupItem GenderItem = null;

            public Guid StaffId = Guid.NewGuid();
            public DateTime DateOfBirth = new DateTime(2000, 1, 1);
            public DateTime StartDate = DateTime.Today.AddDays(-1);
            public Guid ServiceRecordId = Guid.NewGuid();

            public StaffRecordData(int getLookupValuesAtIndex)
                : this()
            {
                TitleItem = DataAccessor.GetLookupItems("Title").ElementAt(getLookupValuesAtIndex);
                MaritalStatusItem = DataAccessor.GetLookupItems("MaritalStatus").ElementAt(getLookupValuesAtIndex);
                GenderItem = DataAccessor.GetLookupItems("Gender").ElementAt(getLookupValuesAtIndex);
            }

            public StaffRecordData()
            {
                PreferredSurname = LegalSurname;
                PreferredForname = LegalForename;
            }
        }
    }
}
