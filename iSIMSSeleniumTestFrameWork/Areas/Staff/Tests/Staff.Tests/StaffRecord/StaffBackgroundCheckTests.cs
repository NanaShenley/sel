using Microsoft.VisualStudio.TestTools.UnitTesting;
using Staff.POM.Components.Staff;
using Staff.POM.Helper;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using TestSettings;
using WebDriverRunner.internals;
using SeSugar;
using SeSugar.Automation;
using SeSugar.Data;
using Selene.Support.Attributes;

namespace Staff.Tests.StaffRecord
{
    [TestClass]
    public class StaffBackgroundCheckTests
    {
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
        [TestMethod]
        [ChromeUiTest("StaffBackgroundCheck", "P1", "Create")]
        public void Create_new_background_check_for_existing_Staff_as_PO()
        {
            #region Arrange
            var staffId = Guid.NewGuid();
            var forename = SeSugar.Utilities.GenerateRandomString(10, "SeBackGroundCheckFN");
            var surname = SeSugar.Utilities.GenerateRandomString(10, "SeBackGroundCheckSN");
            var startDate = DateTime.Today.AddDays(-1);

            StaffRecordPage staffRecordPage = null;

            var staffRecordData = CreateStaffRecord(staffId, forename, surname, startDate);
            var staffBackgroundChecksData = new StaffBackgroundChecksData(0);
            #endregion

            #region Act
            using (new DataSetup(staffRecordData))
            {
                //Login as School Personnel Officer and navigate to staff record page
                LoginAndNavigate(SeleniumHelper.iSIMSUserType.PersonnelOfficer);

                var staffName = String.Format("{0}, {1}", surname, forename);
                staffRecordPage = CompleteStaffBackgroundCheckDetails(staffName, staffBackgroundChecksData);
                staffRecordPage.ClickSave();

                staffRecordPage = StaffRecordPage.LoadStaffDetail(staffId);
            };

            var staffBackgroundCheckRow = staffRecordPage.BackgroundCheckTable.Rows.FirstOrDefault(t => t.Check.Equals(staffBackgroundChecksData.CheckType));
            var backgroundChecksSavedSuccessfully = VerifyBackgroundCheckSDetails(staffRecordPage, staffBackgroundChecksData);
            #endregion

            #region Assert
            //Check the BackgroundCheck grid in staff record for data upon dialog closing
            Assert.IsNotNull(staffBackgroundCheckRow);
            Assert.AreEqual(staffBackgroundCheckRow.ClearanceLevel, staffBackgroundChecksData.ClearanceLevel);
            Assert.AreEqual(staffBackgroundCheckRow.ClearanceDate, staffBackgroundChecksData.ClearanceDate);
            Assert.AreEqual(staffBackgroundCheckRow.ExpiryDate, staffBackgroundChecksData.ExpiryDate);
            Assert.IsTrue(backgroundChecksSavedSuccessfully, "Saved Staff background checks do not match inserted data");
            #endregion
        }

        [TestMethod]
        [ChromeUiTest("StaffBackgroundCheck", "P1", "Read")]
        public void Read_existing_background_check_from_Staff_as_PO()
        {
            #region Arrange
            var staffId = Guid.NewGuid();
            var forename = SeSugar.Utilities.GenerateRandomString(10, "SeBackGroundCheckFN");
            var surname = SeSugar.Utilities.GenerateRandomString(10, "SeBackGroundCheckSN");
            var startDate = DateTime.Today.AddDays(-1);

            StaffRecordPage staffRecordPage = null;
            var staffBackgroundChecksData = new StaffBackgroundChecksData();
            var staffCheckTypes = DataAccessor.GetLookupItems("StaffCheckType");
            var staffClearanceLevels = DataAccessor.GetLookupItems("StaffCheckClearanceLevel");
            staffBackgroundChecksData.CheckType = staffCheckTypes.ElementAt(0).Description;
            staffBackgroundChecksData.ClearanceLevel = staffClearanceLevels.ElementAt(0).Description;

            var staffRecordDataPackage = CreateStaffRecord(staffId, forename, surname, startDate);

            var staffCheckDataPackage = new DataPackage().BuildDataPackage()
               .AddData("StaffCheck", new
               {
                   Id = staffBackgroundChecksData.Id,
                   RequestedDate = DateTime.Parse(staffBackgroundChecksData.RequestedDate),
                   ClearanceDate = DateTime.Parse(staffBackgroundChecksData.ClearanceDate),
                   ExpiryDate = DateTime.Parse(staffBackgroundChecksData.ExpiryDate),
                   ReferenceNumber = staffBackgroundChecksData.ReferenceNumber,
                   DocumentNumber = staffBackgroundChecksData.DocumentNumber,
                   AuthenticatedBy = staffBackgroundChecksData.AuthenticatedBy,
                   Notes = staffBackgroundChecksData.Notes,
                   Staff = staffId,
                   StaffCheckType = staffCheckTypes.ElementAt(0).ID,
                   StaffCheckClearanceLevel = staffClearanceLevels.ElementAt(0).ID,
                   TenantID = SeSugar.Environment.Settings.TenantId
               });
            #endregion

            #region Act
            using (new DataSetup(staffRecordDataPackage, staffCheckDataPackage))
            {
                //Login as School Personnel Officer and navigate to staff record
                LoginAndNavigate(SeleniumHelper.iSIMSUserType.PersonnelOfficer);
                staffRecordPage = GetStaffRecord(staffId);
            };

            //Retrieve Background checks data from backgroundchecks dialog and check they match created data
            var bgroundChecksMatch = VerifyBackgroundCheckSDetails(staffRecordPage, staffBackgroundChecksData);
            #endregion

            #region Assert
            Assert.IsTrue(bgroundChecksMatch, "Saved Staff background checks do not match inserted data");
            #endregion
        }


        [TestMethod]
        [ChromeUiTest("StaffBackgroundCheck", "P1", "Update")]
        public void Update_existing_background_check_from_Staff_as_PO()
        {
            #region Arrange
            var staffId = Guid.NewGuid();
            var forename = SeSugar.Utilities.GenerateRandomString(10, "SeBackGroundCheckFN");
            var surname = SeSugar.Utilities.GenerateRandomString(10, "SeBackGroundCheckSN");
            var startDate = DateTime.Today.AddDays(-1);

            StaffRecordPage staffRecordPage = null;
            var originalBgroundChecksData = new StaffBackgroundChecksData();
            var staffCheckTypes = DataAccessor.GetLookupItems("StaffCheckType");
            var staffClearanceLevels = DataAccessor.GetLookupItems("StaffCheckClearanceLevel");
            originalBgroundChecksData.CheckType = staffCheckTypes.ElementAt(0).Description;
            originalBgroundChecksData.ClearanceLevel = staffClearanceLevels.ElementAt(0).Description;

            var staffRecordDataPackage = CreateStaffRecord(staffId, forename, surname, startDate);

            var staffCheckDataPackage = new DataPackage().BuildDataPackage()
               .AddData("StaffCheck", new
               {
                   Id = originalBgroundChecksData.Id,
                   RequestedDate = DateTime.Parse(originalBgroundChecksData.RequestedDate),
                   ClearanceDate = DateTime.Parse(originalBgroundChecksData.ClearanceDate),
                   ExpiryDate = DateTime.Parse(originalBgroundChecksData.ExpiryDate),
                   ReferenceNumber = originalBgroundChecksData.ReferenceNumber,
                   DocumentNumber = originalBgroundChecksData.DocumentNumber,
                   AuthenticatedBy = originalBgroundChecksData.AuthenticatedBy,
                   Notes = originalBgroundChecksData.Notes,
                   Staff = staffId,
                   StaffCheckType = staffCheckTypes.ElementAt(0).ID,
                   StaffCheckClearanceLevel = staffClearanceLevels.ElementAt(0).ID,
                   TenantID = SeSugar.Environment.Settings.TenantId
               });
            #endregion

            #region Act
            using (new DataSetup(staffRecordDataPackage, staffCheckDataPackage))
            {
                //Login as School Personnel Officer and navigate to staff record
                LoginAndNavigate(SeleniumHelper.iSIMSUserType.PersonnelOfficer);
                staffRecordPage = GetStaffRecord(staffId);
            };

            //Retrieve Background checks data from backgroundchecks dialog and check they match created data
            bool originalBgroundChecksMatch = VerifyBackgroundCheckSDetails(staffRecordPage, originalBgroundChecksData);

            //Create new background checks test data
            var updatedStaffBgroundChecksData = new StaffBackgroundChecksData();
            updatedStaffBgroundChecksData.RequestedDate = DateTime.Today.AddDays(-10).ToShortDateString();
            updatedStaffBgroundChecksData.ClearanceDate = DateTime.Today.AddDays(-10).ToShortDateString();
            updatedStaffBgroundChecksData.ExpiryDate = DateTime.Today.AddDays(-9).ToShortDateString();
            updatedStaffBgroundChecksData.CheckType = staffCheckTypes.ElementAt(1).Description;
            updatedStaffBgroundChecksData.ClearanceLevel = staffClearanceLevels.ElementAt(1).Description;

            var staffName = String.Format("{0}, {1}", surname, forename);
            staffRecordPage = CompleteStaffBackgroundCheckDetails(staffName, updatedStaffBgroundChecksData, originalBgroundChecksData.CheckType);

            staffRecordPage.ClickSave();

            bool updatedBgroundChecksMatch = VerifyBackgroundCheckSDetails(staffRecordPage, updatedStaffBgroundChecksData);
            #endregion

            #region Assert
            Assert.IsTrue(originalBgroundChecksMatch, "Original Staff background checks do not match inserted data");
            Assert.IsTrue(updatedBgroundChecksMatch, "Saved Staff background checks do not match inserted data");
            #endregion
        }

        [TestMethod]
        [ChromeUiTest("StaffBackgroundCheck", "P1", "Delete")]
        public void Delete_existing_background_check_from_Staff_as_PO()
        {
            #region Arrange
            var staffId = Guid.NewGuid();
            var forename = SeSugar.Utilities.GenerateRandomString(10, "SeBackGroundCheckFN");
            var surname = SeSugar.Utilities.GenerateRandomString(10, "SeBackGroundCheckSN");
            var startDate = DateTime.Today.AddDays(-1);

            StaffRecordPage staffRecordPage = null;
            var staffBackgroundChecksData = new StaffBackgroundChecksData();
            var staffCheckTypes = DataAccessor.GetLookupItems("StaffCheckType");
            var staffClearanceLevels = DataAccessor.GetLookupItems("StaffCheckClearanceLevel");
            staffBackgroundChecksData.CheckType = staffCheckTypes.ElementAt(0).Description;
            staffBackgroundChecksData.ClearanceLevel = staffClearanceLevels.ElementAt(0).Description;

            var staffRecordDataPackage = CreateStaffRecord(staffId, forename, surname, startDate);

            var staffCheckDataPackage = new DataPackage().BuildDataPackage()
               .AddData("StaffCheck", new
               {
                   Id = staffBackgroundChecksData.Id,
                   RequestedDate = DateTime.Parse(staffBackgroundChecksData.RequestedDate),
                   ClearanceDate = DateTime.Parse(staffBackgroundChecksData.ClearanceDate),
                   ExpiryDate = DateTime.Parse(staffBackgroundChecksData.ExpiryDate),
                   ReferenceNumber = staffBackgroundChecksData.ReferenceNumber,
                   DocumentNumber = staffBackgroundChecksData.DocumentNumber,
                   AuthenticatedBy = staffBackgroundChecksData.AuthenticatedBy,
                   Notes = staffBackgroundChecksData.Notes,
                   Staff = staffId,
                   StaffCheckType = staffCheckTypes.ElementAt(0).ID,
                   StaffCheckClearanceLevel = staffClearanceLevels.ElementAt(0).ID,
                   TenantID = SeSugar.Environment.Settings.TenantId
               });
            #endregion

            #region Act
            using (new DataSetup(staffRecordDataPackage, staffCheckDataPackage))
            {
                //Login as School Personnel Officer and navigate to staff record
                LoginAndNavigate(SeleniumHelper.iSIMSUserType.PersonnelOfficer);
                staffRecordPage = GetStaffRecord(staffId);
            };

            //Delete the bgroundcheck grid row
            staffRecordPage.BackgroundCheckTable.Rows.FirstOrDefault(t => t.Check.Equals(staffBackgroundChecksData.CheckType)).DeleteRow();
            staffRecordPage.ClickSave();

            //Ensure that the UI has removed the item after a save
            var existingBgroundCheck = staffRecordPage.BackgroundCheckTable.Rows.FirstOrDefault(t => t.Check.Equals(staffBackgroundChecksData.CheckType));
            Assert.IsNull(existingBgroundCheck, "Inserted StaffCheck still exists in UI after a deletion.");

            //Check the database has removed the item
            string sql = "SELECT TOP 1 ID FROM dbo.StaffCheck WHERE TenantID = @TenantId AND ID = @Value";
            var deletedBgroundCheckId = DataAccessHelpers.GetValue<Guid>(sql, new { TenantId = SeSugar.Environment.Settings.TenantId, Value = staffBackgroundChecksData.Id.ToString() });
            #endregion

            #region Assert
            Assert.AreEqual(deletedBgroundCheckId, Guid.Empty, "Inserted StaffCheck still exists in database after a deletion.");
            #endregion
        }

        #region Helpers

        /// <summary>
        /// Gets the staff records details for the given staff member
        /// </summary>
        /// 
        private StaffRecordPage GetStaffRecord(Guid staffId)
        {
            return StaffRecordPage.LoadStaffDetail(staffId);
        }

        /// <summary>
        /// Matches the passed-in data to the staff backgrounbd checks dialog
        /// </summary>
        private bool VerifyBackgroundCheckSDetails(StaffRecordPage staffRecordPage, StaffBackgroundChecksData staffBackgroundChecksData)
        {
            var backgroundChecksDialog = staffRecordPage.BackgroundCheckTable.Rows.FirstOrDefault(t => t.Check.Equals(staffBackgroundChecksData.CheckType)).ClickEdit();
            Assert.IsNotNull(backgroundChecksDialog, "Could not find BackgroundChecksDialog when in VerifyBackgroundCheckSDetails.");

            Assert.AreEqual(backgroundChecksDialog.Check, staffBackgroundChecksData.CheckType, "backgroundChecksDialog.Check are not the same.");

            Assert.AreEqual(backgroundChecksDialog.RequestedDate, staffBackgroundChecksData.RequestedDate, "backgroundChecksDialog.RequestedDate are not the same.");
            Assert.AreEqual(backgroundChecksDialog.ClearanceDate, staffBackgroundChecksData.ClearanceDate, "backgroundChecksDialog.ClearanceDate are not the same.");
            Assert.AreEqual(backgroundChecksDialog.ClearanceLevel, staffBackgroundChecksData.ClearanceLevel, "backgroundChecksDialog.ClearanceLevel are not the same.");
            Assert.AreEqual(backgroundChecksDialog.ExpiryDate, staffBackgroundChecksData.ExpiryDate, "backgroundChecksDialog.ExpiryDate are not the same.");
            Assert.AreEqual(backgroundChecksDialog.ReferenceNumber, staffBackgroundChecksData.ReferenceNumber, "backgroundChecksDialog.ReferenceNumber are not the same.");
            Assert.AreEqual(backgroundChecksDialog.DocumentNumber, staffBackgroundChecksData.DocumentNumber, "backgroundChecksDialog.DocumentNumber are not the same.");
            Assert.AreEqual(backgroundChecksDialog.AuthenticatedBy, staffBackgroundChecksData.AuthenticatedBy, "backgroundChecksDialog.AuthenticatedBy are not the same.");
            Assert.AreEqual(backgroundChecksDialog.Notes, staffBackgroundChecksData.Notes, "backgroundChecksDialog.Notes are not the same.");

            backgroundChecksDialog.ClickCancel();
            return true;
        }

        /// <summary>
        /// Complete the details for the Staff Background Checks dialog.
        /// Note: if an existingStaffCheckToEdit is passed in the staff background check dialog with edit the existing staff check,
        /// otherwise a new staff background check row will be added.
        /// </summary>
        private StaffRecordPage CompleteStaffBackgroundCheckDetails(string staffName, StaffBackgroundChecksData backgroundChecksData, string existingStaffCheckToEdit = null)
        {
            //Select existing staff member
            var staffTriplet = new StaffRecordTriplet();
            staffTriplet.SearchCriteria.StaffName = staffName;
            var staffResultTiles = staffTriplet.SearchCriteria.Search();
            var staffRecordPage = staffResultTiles.SingleOrDefault(x => true).Click<StaffRecordPage>();
            AddBackgroundCheckDialog addbackgroundCheckDialog;

            //Edit an existing row or create new?
            if (!string.IsNullOrEmpty(existingStaffCheckToEdit))
            {
                addbackgroundCheckDialog = staffRecordPage.BackgroundCheckTable.Rows.FirstOrDefault(t => t.Check.Equals(existingStaffCheckToEdit)).ClickEdit();
            }
            else
            {
                addbackgroundCheckDialog = staffRecordPage.ClickAddBackGroudCheck();
            }

            Assert.IsNotNull(addbackgroundCheckDialog, "Could not find addbackgroundCheckDialog dialog when CompleteStaffBackgroundCheckDetails.");

            addbackgroundCheckDialog.Check = backgroundChecksData.CheckType;
            addbackgroundCheckDialog.RequestedDate = backgroundChecksData.RequestedDate;
            addbackgroundCheckDialog.ClearanceDate = backgroundChecksData.ClearanceDate;
            addbackgroundCheckDialog.ClearanceLevel = backgroundChecksData.ClearanceLevel;
            addbackgroundCheckDialog.ExpiryDate = backgroundChecksData.ExpiryDate;
            addbackgroundCheckDialog.ReferenceNumber = backgroundChecksData.ReferenceNumber;
            addbackgroundCheckDialog.DocumentNumber = backgroundChecksData.DocumentNumber;
            addbackgroundCheckDialog.AuthenticatedBy = backgroundChecksData.AuthenticatedBy;
            addbackgroundCheckDialog.Notes = backgroundChecksData.Notes;

            return addbackgroundCheckDialog.AddStaffCheck();
        }

        /// <summary>
        /// Create a staff record with the given staffId
        /// </summary>
        private DataPackage CreateStaffRecord(Guid staffId, string forename, string surname, DateTime startDate)
        {
            var staffRecordData = new DataPackage().BuildDataPackage()
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
                   DOA = startDate,
                   ContinuousServiceStartDate = startDate,
                   LocalAuthorityStartDate = startDate,
                   Staff = staffId,
                   TenantID = SeSugar.Environment.Settings.TenantId
               });
            return staffRecordData;
        }

        /// <summary>
        /// Logs the user in for the given profile and navigates to the Staff search screen
        /// </summary>
        /// <param name="userType"></param>
        private void LoginAndNavigate(SeleniumHelper.iSIMSUserType userType)
        {
            SeleniumHelper.Login(userType);
            AutomationSugar.NavigateMenu("Tasks", "Staff", "Staff Records");
        }

        /// <summary>
        /// Staff background checks data object to hold test data for which comparisons can be made
        /// </summary>
        internal class StaffBackgroundChecksData
        {
            public Guid Id = Guid.NewGuid();
            public string RequestedDate = DateTime.Today.ToShortDateString();
            public string ClearanceDate = DateTime.Today.ToShortDateString();
            public string ExpiryDate = DateTime.Today.AddDays(1).ToShortDateString();
            public readonly string Notes = Utilities.GenerateRandomString(2, "Se");
            public readonly string ReferenceNumber = Utilities.GenerateRandomString(2, "Se");
            public readonly string DocumentNumber = Utilities.GenerateRandomString(2, "Se");
            public readonly string AuthenticatedBy = Utilities.GenerateRandomString(2, "Se");
            public string CheckType = string.Empty;
            public string ClearanceLevel = string.Empty;

            public StaffBackgroundChecksData(int getLookupValuesAtIndex)
                : this()
            {
                CheckType = DataAccessor.GetLookupItems("StaffCheckType").ElementAt(getLookupValuesAtIndex).Description;
                ClearanceLevel = DataAccessor.GetLookupItems("StaffCheckClearanceLevel").ElementAt(getLookupValuesAtIndex).Description;
            }

            public StaffBackgroundChecksData()
            { }
        }
        #endregion
    }

}
