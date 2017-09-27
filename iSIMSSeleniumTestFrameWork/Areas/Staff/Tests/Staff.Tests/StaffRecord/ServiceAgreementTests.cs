using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using Selene.Support.Attributes;
using SeSugar;
using SeSugar.Automation;
using SeSugar.Data;
using Staff.POM.Base;
using Staff.POM.Components.Staff;
using Staff.POM.Components.Staff.Dialogs;
using Staff.POM.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebDriverRunner.internals;
using WebDriverRunner.webdriver;

namespace Staff.Tests.StaffRecord
{
    /// <summary>
    /// Service Agreement Tests
    /// </summary>
    [TestClass]
    public class ServiceAgreementTests
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

        #region Tests

        /// <summary>
        /// Chrome test - create new service agreement for existing staff as po.
        /// </summary>
        [TestMethod]
        [ChromeUiTest("Chrome", "ServiceAgreement", "P1", "Chrome_Create_new_Service_Agreement_for_existing_Staff_as_PO")]
        public void Chrome_Create_new_Service_Agreement_for_existing_Staff_as_PO()
        {
            Create_new_Service_Agreement_for_existing_Staff_as_PO();
        }

        /// <summary>
        /// Chrome test - the read existing service agreement for existing staff as po.
        /// </summary>
        [TestMethod]
        [ChromeUiTest("Chrome", "ServiceAgreement", "P1", "Chrome_Read_existing_Service_Agreement_for_existing_Staff_as_PO")]
        public void Chrome_Read_existing_Service_Agreement_for_existing_Staff_as_PO()
        {
            Read_existing_Service_Agreement_for_existing_Staff_as_PO();
        }

        /// <summary>
        /// Chrome test -  update existing service agreement for existing staff as po.
        /// </summary>
        [TestMethod]
        [ChromeUiTest("Chrome", "ServiceAgreement", "P1", "Chrome_Update_existing_Service_Agreement_for_existing_Staff_as_PO")]
        public void Chrome_Update_existing_Service_Agreement_for_existing_Staff_as_PO()
        {
            Update_existing_Service_Agreement_for_existing_Staff_as_PO();
        }


        /// <summary>
        /// Chrome test - delete service agreement for existing staff as po.
        /// </summary>
        [TestMethod]
        [ChromeUiTest("Chrome", "ServiceAgreement", "P1", "Chrome_Delete_Service_Agreement_for_existing_Staff_as_PO")]
        public void Chrome_Delete_Service_Agreement_for_existing_Staff_as_PO()
        {
            Delete_Service_Agreement_for_existing_Staff_as_PO();
        }

        /// <summary>
        /// IE tests - create new service agreement for existing staff as po.
        /// </summary>
        //[IeUiTest("IE","ServiceAgreement", "P1", "Ie_Create_new_Service_Agreement_for_existing_Staff_as_PO")]
        public void Ie_Create_new_Service_Agreement_for_existing_Staff_as_PO()
        {
            Create_new_Service_Agreement_for_existing_Staff_as_PO();
        }

        /// <summary>
        /// IE tests - read existing service agreement for existing staff as po.
        /// </summary>
        //[IeUiTest("IE", "ServiceAgreement", "P1", "Ie_Read_existing_Service_Agreement_for_existing_Staff_as_PO")]
        public void Ie_Read_existing_Service_Agreement_for_existing_Staff_as_PO()
        {
            Read_existing_Service_Agreement_for_existing_Staff_as_PO();
        }
        /// <summary>
        /// IE tests - update existing service agreement for existing staff as po.
        /// </summary>
        //[IeUiTest("IE", "ServiceAgreement", "P1", "Ie_Update_existing_Service_Agreement_for_existing_Staff_as_PO")]
        public void Ie_Update_existing_Service_Agreement_for_existing_Staff_as_PO()
        {
            Update_existing_Service_Agreement_for_existing_Staff_as_PO();
        }
        /// <summary>
        /// IE tests - delete service agreement for existing staff as po.
        /// </summary>
        //[IeUiTest("IE", "ServiceAgreement", "P1", "Ie_Delete_Service_Agreement_for_existing_Staff_as_PO")]
        public void Ie_Delete_Service_Agreement_for_existing_Staff_as_PO()
        {
            Delete_Service_Agreement_for_existing_Staff_as_PO();
        }

        #endregion

        private DataPackage CreateStaffRecord(out Guid staffId, string forename, string surname, DateTime startDate)
        {
            Guid serviceRecordId;

            DataPackage testData = new DataPackage();

            testData.AddData("Staff", DataPackageHelper.GenerateStaff(out staffId, surname, forename: forename));
            testData.AddData("StaffServiceRecord", DataPackageHelper.GenerateServiceRecord(out serviceRecordId, staffId, startDate));

            return testData;
        }

        private DataPackage CreateServiceAgreementSchoolData(string code, string description)
        {
            Guid serviceAgreementReasonId;

            DataPackage testData = new DataPackage();
            testData.AddData("ServiceAgreementReason", DataPackageHelper.GenerateServiceAgreementReason(out serviceAgreementReasonId, code, description));

            return testData;
        }

        private DataPackage CreateStaffRecordAndServiceAgreement(out Guid staffId, string forename, string surname, DateTime startDate)
        {
            Guid serviceRecordId, serviceAgreementId, serviceAgreementRoleId, postTypeId, statutoryPostTypeId;
            string postTypeCode = CoreQueries.GetColumnUniqueString("PostType", "Code", 4, SeSugar.Environment.Settings.TenantId);
            string postTypeDescription = CoreQueries.GetColumnUniqueString("PostType", "Description", 4, SeSugar.Environment.Settings.TenantId);

            DataPackage testData = new DataPackage();

            testData.AddData("StatutoryPostType", DataPackageHelper.GenerateStatutoryPostType(out statutoryPostTypeId));
            testData.AddData("PostType", DataPackageHelper.GeneratePostType(out postTypeId, null, postTypeCode, postTypeDescription, statutoryPostTypeId));
            testData.AddData("Staff", DataPackageHelper.GenerateStaff(out staffId, surname, forename: forename));
            testData.AddData("StaffServiceRecord", DataPackageHelper.GenerateServiceRecord(out serviceRecordId, staffId, startDate));
            testData.AddData("ServiceAgreement", DataPackageHelper.GenerateServiceAgreement(out serviceAgreementId, staffId, postTypeId));
            testData.AddData("ServiceAgreementRole", DataPackageHelper.GenerateServiceAgreementRole(out serviceAgreementRoleId, serviceAgreementId, DateTime.Now.AddDays(-1), DateTime.Now.AddDays(10)));

            return testData;
        }

        /// <summary>
        /// Create the service agreement for existing staff as po.
        /// </summary>
        private void Create_new_Service_Agreement_for_existing_Staff_as_PO()
        {
            string reason = "Contractor";
            string serviceType = "Service Agreement with an Agency";
            string sourcedBy = "Agency";
            string sourceName = "X4group";
            string fte = "0.9";
            string agreementHours = "24.0";
            string weeksPerYear = "26";
            string cook = "Cook";
            string notes = "Another contract";

            Guid StaffId = Guid.Empty;

            using (new DataSetup(CreateStaffRecord(out StaffId, Utilities.GenerateRandomString(20, "Keith"), Utilities.GenerateRandomString(20, "Holme"), DateTime.Now.AddDays(-1))))
            {
                //arrange
                InitialiseToStaffRecordPage();

                StaffRecordPage staffRecord = StaffRecordPage.LoadStaffDetail(StaffId);

                var currentServiceTermCount = staffRecord.ServieAgreementStandardTable.Rows.Count();

                // start add staff record
                var serviceAgreementDialog = staffRecord.ClickAddServiceAgreement();

                var date = DateTime.Now;
                sourceName = string.Format("{0}_{1}", sourceName, date.ToString("dd_MM_yy"));

                serviceAgreementDialog.StartDate = date.ToShortDateString();
                serviceAgreementDialog.EndDate = date.AddDays(1).ToShortDateString();
                serviceAgreementDialog.OfferedDate = date.AddMonths(-1).ToShortDateString();
                serviceAgreementDialog.AcceptedDate = date.AddDays(-15).ToShortDateString();
                serviceAgreementDialog.Resason = reason;
                serviceAgreementDialog.ServiceType = serviceType;
                serviceAgreementDialog.SourcedBy = sourcedBy;
                serviceAgreementDialog.SourceName = sourceName;

                serviceAgreementDialog.AddRoleButtonClick();
                var row = serviceAgreementDialog.RolesTable.Rows[0];
                row.Role = cook;
                row.StartDate = date.ToShortDateString();
                row.EndDate = date.AddDays(1).ToShortDateString();

                serviceAgreementDialog.FTE = fte;
                serviceAgreementDialog.AgreementHours = agreementHours;
                serviceAgreementDialog.WeeksPerYear = weeksPerYear;

                serviceAgreementDialog.Notes = notes;

                serviceAgreementDialog.ClickOk();
                staffRecord.Refresh();
                staffRecord.SaveStaff();

                Assert.IsTrue(staffRecord.ServieAgreementStandardTable.Rows.Count() == currentServiceTermCount + 1);

                var newRow = staffRecord.ServieAgreementStandardTable.Rows[currentServiceTermCount];

                newRow.ClickEdit();

                ServiceAgreementDialog dlg = new ServiceAgreementDialog();

                Assert.AreEqual(sourceName, dlg.SourceName);
                Assert.AreEqual(date.ToShortDateString(), dlg.StartDate);
                Assert.AreEqual(date.AddDays(1).ToShortDateString(), dlg.EndDate);
                Assert.AreEqual(date.AddMonths(-1).ToShortDateString(), dlg.OfferedDate);
                Assert.AreEqual(date.AddDays(-15).ToShortDateString(), dlg.AcceptedDate);
                Assert.AreEqual(reason, dlg.Resason);
                Assert.AreEqual(serviceType, dlg.ServiceType);
                Assert.AreEqual(sourcedBy, dlg.SourcedBy);
                Assert.AreEqual(sourceName, dlg.SourceName);
                Assert.AreEqual(Decimal.Parse(fte), Decimal.Parse(dlg.FTE));
                Assert.AreEqual(Decimal.Parse(agreementHours), Decimal.Parse(dlg.AgreementHours));
                Assert.AreEqual(Decimal.Parse(weeksPerYear), Decimal.Parse(dlg.WeeksPerYear));
            }
        }

        /// <summary>
        /// Read existing the service agreement for existing staff as po.
        /// </summary>
        public void Read_existing_Service_Agreement_for_existing_Staff_as_PO()
        {
            Guid StaffId = Guid.Empty;

            using (new DataSetup(CreateStaffRecordAndServiceAgreement(out StaffId, Utilities.GenerateRandomString(20, "Keith"), Utilities.GenerateRandomString(20, "Holme"), DateTime.Now.AddDays(-1))))
            {
                InitialiseToStaffRecordPage();

                // Goto first staff record.
                StaffRecordPage staffRecord = StaffRecordPage.LoadStaffDetail(StaffId);

                // Get the first row.
                var newRow = staffRecord.ServieAgreementStandardTable.Rows[0];

                // open
                newRow.ClickEdit();

                ServiceAgreementDialog dlg = new ServiceAgreementDialog();
                Assert.IsTrue(!string.IsNullOrWhiteSpace(dlg.StartDate));
            }
        }

        /// <summary>
        /// Update existing the service agreement for existing staff as po.
        /// </summary>
        private void Update_existing_Service_Agreement_for_existing_Staff_as_PO()
        {
            Guid StaffId = Guid.Empty;

            using (new DataSetup(CreateStaffRecordAndServiceAgreement(out StaffId, Utilities.GenerateRandomString(20, "Keith"), Utilities.GenerateRandomString(20, "Holme"), DateTime.Now.AddDays(-1))))
            {
                InitialiseToStaffRecordPage();

                // Goto first staff record.
                StaffRecordPage staffRecord = StaffRecordPage.LoadStaffDetail(StaffId);

                string reason = "Supply";
                string serviceType = "Service Agreement with Local Authority";
                string sourcedBy = "Local Authority";
                string sourceName = "X4group_2";
                string fte = "0.8";
                string agreementHours = "16.0";
                string weeksPerYear = "52";
                string notes = "Yet another contract";

                // Get the first row.
                var newRow = staffRecord.ServieAgreementStandardTable.Rows[0];

                // open for edit
                newRow.ClickEdit();

                ServiceAgreementDialog serviceAgreementDialog = new ServiceAgreementDialog();

                var date = DateTime.Now;
                sourceName = string.Format("{0}_{1}", sourceName, date.ToString("dd_MM_yy"));
                serviceAgreementDialog.StartDate = date.AddDays(-2).ToShortDateString();
                serviceAgreementDialog.EndDate = date.AddDays(2).ToShortDateString();
                serviceAgreementDialog.OfferedDate = date.AddMonths(-5).ToShortDateString();
                serviceAgreementDialog.AcceptedDate = date.AddDays(-4).ToShortDateString();
                serviceAgreementDialog.Resason = reason;
                serviceAgreementDialog.ServiceType = serviceType;
                serviceAgreementDialog.SourcedBy = sourcedBy;
                serviceAgreementDialog.SourceName = sourceName;

                serviceAgreementDialog.FTE = fte;
                serviceAgreementDialog.AgreementHours = agreementHours;
                serviceAgreementDialog.WeeksPerYear = weeksPerYear;

                serviceAgreementDialog.Notes = notes;

                serviceAgreementDialog.ClickOk();
                staffRecord.Refresh();
                staffRecord.SaveStaff();

                newRow = staffRecord.ServieAgreementStandardTable.Rows[0];

                // open for edit
                newRow.ClickEdit();

                ServiceAgreementDialog dlg = new ServiceAgreementDialog();

                Assert.AreEqual(sourceName, dlg.SourceName);
                Assert.AreEqual(date.AddDays(-2).ToShortDateString(), dlg.StartDate);
                Assert.AreEqual(date.AddDays(2).ToShortDateString(), dlg.EndDate);
                Assert.AreEqual(date.AddMonths(-5).ToShortDateString(), dlg.OfferedDate);
                Assert.AreEqual(date.AddDays(-4).ToShortDateString(), dlg.AcceptedDate);
                Assert.AreEqual(reason, dlg.Resason);
                Assert.AreEqual(serviceType, dlg.ServiceType);
                Assert.AreEqual(sourcedBy, dlg.SourcedBy);
                Assert.AreEqual(sourceName, dlg.SourceName);
                Assert.AreEqual(Decimal.Parse(fte), Decimal.Parse(dlg.FTE));
                Assert.AreEqual(Decimal.Parse(agreementHours), Decimal.Parse(dlg.AgreementHours));
                Assert.AreEqual(Decimal.Parse(weeksPerYear), Decimal.Parse(dlg.WeeksPerYear));
            }
        }

        /// <summary>
        /// Delete the service agreement for existing staff as po.
        /// </summary>
        private void Delete_Service_Agreement_for_existing_Staff_as_PO()
        {
            Guid StaffId = Guid.Empty;

            using (new DataSetup(CreateStaffRecordAndServiceAgreement(out StaffId, Utilities.GenerateRandomString(20, "Keith"), Utilities.GenerateRandomString(20, "Holme"), DateTime.Now.AddDays(-1))))
            {
                InitialiseToStaffRecordPage();

                // Goto first staff record.
                StaffRecordPage staffRecord = StaffRecordPage.LoadStaffDetail(StaffId);

                var serviceAgreementCount = staffRecord.ServieAgreementStandardTable.Rows.Count();

                // Get the first row and delete it.
                var newRow = staffRecord.ServieAgreementStandardTable.Rows[0];
                newRow.DeleteRow();

                staffRecord.SaveStaff();
                staffRecord.Refresh();

                Assert.AreEqual(serviceAgreementCount - 1, staffRecord.ServieAgreementStandardTable.Rows.Count());
            }
        }

        /// <summary>
        /// Initialises to staff record page.
        /// </summary>
        private void InitialiseToStaffRecordPage()
        {
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.PersonnelOfficer);
            AutomationSugar.NavigateMenu("Tasks", "Staff", "Staff Records");
        }
    }
}
