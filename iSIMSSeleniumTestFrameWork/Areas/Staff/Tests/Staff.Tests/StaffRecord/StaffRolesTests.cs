using Microsoft.VisualStudio.TestTools.UnitTesting;
using Staff.POM.Components.Staff;
using Staff.POM.Helper;
using System;
using System.Linq;
using SeSugar.Automation;
using SeSugar.Data;
using WebDriverRunner.internals;
using Selene.Support.Attributes;

namespace Staff.Tests.StaffRecord
{
    [TestClass]
    public class StaffRolesTests
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
        #region Private Parameters

        private readonly int tenantID = SeSugar.Environment.Settings.TenantId;
        private const string staffRoleDescription = "Other School Admin";

        private readonly DateTime startDate = DateTime.Today.AddDays(-1);
        private readonly DateTime endDate = DateTime.Today;

        #endregion

        #region Tests

        [NotDone]
        [TestMethod]
        [ChromeUiTest("StaffRoles", "P1", "Create")]
        public void Create_new_Role_for_existing_Staff_as_PO()
        {
            //Arrange
            Guid id = Guid.NewGuid();
            string forename = CoreQueries.GetColumnUniqueString("Staff", "LegalForename", 10, tenantID);
            string surname = CoreQueries.GetColumnUniqueString("Staff", "LegalSurname", 10, tenantID);

            using (new DataSetup(GetStaffRecord(id, forename, surname)))
            {
                //Act
                LoginAndNavigate();
                StaffRecordPage staff = Search(surname);
                staff.ClickAddStaffRole();

                var gridRow = staff.StaffRoleStandardTable.Rows[0];
                gridRow.StaffRole = staffRoleDescription;
                gridRow.Refresh();
                gridRow.StartDate = startDate.ToShortDateString();
                gridRow.Refresh();
                gridRow.EndDate = endDate.ToShortDateString();

                staff.SaveStaff();
                staff = Search(surname);
                gridRow = staff.StaffRoleStandardTable.Rows.First(x => x.StaffRole == staffRoleDescription);

                //Assert
                Assert.AreEqual(staffRoleDescription, gridRow.StaffRole);
                Assert.AreEqual(startDate.ToShortDateString(), gridRow.StartDate);
                Assert.AreEqual(endDate.ToShortDateString(), gridRow.EndDate);
            }
        }

        [NotDone]
        [TestMethod]
        [ChromeUiTest("StaffRoles", "P1", "Read")]
        public void Read_existing_Role_from_Staff_as_PO()
        {
            //Arrange
            Guid id = Guid.NewGuid();
            string forename = CoreQueries.GetColumnUniqueString("Staff", "LegalForename", 10, tenantID);
            string surname = CoreQueries.GetColumnUniqueString("Staff", "LegalSurname", 10, tenantID);

            using (new DataSetup(GetStaffRecord(id, forename, surname), GetStaffRoles(id)))
            {
                //Act
                LoginAndNavigate();
                StaffRecordPage staff = Search(surname);
                var gridRow = staff.StaffRoleStandardTable.Rows.FirstOrDefault(x => x.StaffRole == staffRoleDescription);

                //Assert
                Assert.AreEqual(staffRoleDescription, gridRow.StaffRole);
                Assert.AreEqual(startDate.ToShortDateString(), gridRow.StartDate);
                Assert.AreEqual(endDate.ToShortDateString(), gridRow.EndDate);
            }
        }

        [NotDone]
        [TestMethod]
        [ChromeUiTest("StaffRoles", "P1", "Update")]
        public void Update_existing_Role_for_Staff_as_PO()
        {
            //Arrange
            Guid id = Guid.NewGuid();
            string forename = CoreQueries.GetColumnUniqueString("Staff", "LegalForename", 10, tenantID);
            string surname = CoreQueries.GetColumnUniqueString("Staff", "LegalSurname", 10, tenantID);

            const string newStaffRole = "Other Technician";
            string newStartDate = DateTime.Today.AddDays(4).ToShortDateString();
            string newEndDate = DateTime.Today.AddDays(5).ToShortDateString();

            using (new DataSetup(GetStaffRecord(id, forename, surname), GetStaffRoles(id)))
            {
                //Act
                LoginAndNavigate();
                StaffRecordPage staff = Search(surname);

                var gridRow = staff.StaffRoleStandardTable.Rows.FirstOrDefault(x => x.StaffRole == staffRoleDescription);
                gridRow.StaffRole = newStaffRole;
                gridRow.Refresh();
                gridRow.StartDate = newStartDate;
                gridRow.Refresh();
                gridRow.EndDate = newEndDate;

                staff.SaveStaff();
                staff = Search(surname);
                gridRow = staff.StaffRoleStandardTable.Rows.FirstOrDefault(x => x.StaffRole == newStaffRole);

                //Assert
                Assert.AreEqual(newStaffRole, gridRow.StaffRole);
                Assert.AreEqual(newStartDate, gridRow.StartDate);
                Assert.AreEqual(newEndDate, gridRow.EndDate);
            }
        }

        [NotDone]
        [TestMethod]
        [ChromeUiTest("StaffRoles", "P1", "Delete")]
        public void Delete_Role_from_Staff_as_PO()
        {
            //Arrange
            Guid id = Guid.NewGuid();
            string forename = CoreQueries.GetColumnUniqueString("Staff", "LegalForename", 10, tenantID);
            string surname = CoreQueries.GetColumnUniqueString("Staff", "LegalSurname", 10, tenantID);

            using (new DataSetup(GetStaffRecord(id, forename, surname), GetStaffRoles(id)))
            {
                //Act
                LoginAndNavigate();
                StaffRecordPage staff = Search(surname);
                var gridRow = staff.StaffRoleStandardTable.Rows.FirstOrDefault(x => x.StaffRole == staffRoleDescription);
                gridRow.DeleteRow();
                staff.SaveStaff();
                staff = Search(surname);
                gridRow = staff.StaffRoleStandardTable.Rows.FirstOrDefault(x => x.StaffRole == staffRoleDescription);

                //Assert
                Assert.AreEqual(null, gridRow);
            }
        }

        #endregion

        #region Helpers

        private static void LoginAndNavigate()
        {
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.PersonnelOfficer);
            AutomationSugar.NavigateMenu("Tasks", "Staff", "Staff Records");
        }

        private static StaffRecordPage Search(string surname)
        {
            StaffRecordTriplet staffRecordTriplet = new StaffRecordTriplet();
            staffRecordTriplet.SearchCriteria.StaffName = surname;
            var result = staffRecordTriplet.SearchCriteria.Search();
            StaffRecordPage staffRecord = result.SingleOrDefault(x => true).Click<StaffRecordPage>();
            return staffRecord;
        }

        #endregion

        #region Data Setup

        private DataPackage GetStaffRecord(Guid staffId, string forename, string surname)
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
                   Gender = CoreQueries.GetLookupItem("Gender", description: "Female"),
                   PolicyACLID = CoreQueries.GetPolicyAclId("Staff"),
                   School = CoreQueries.GetSchoolId(),
                   TenantID = tenantID
               })
               .AddData("StaffServiceRecord", new
               {
                   Id = Guid.NewGuid(),
                   DOA = startDate,
                   ContinuousServiceStartDate = startDate,
                   LocalAuthorityStartDate = startDate,
                   Staff = staffId,
                   TenantID = tenantID
               });
        }

        private DataPackage GetStaffRoles(Guid staffId)
        {
            return this.BuildDataPackage()
                .AddData("StaffRoleAssignment", new
                {
                    Id = Guid.NewGuid(),
                    Staff = staffId,
                    StaffRole = CoreQueries.GetLookupItem("StaffRole", tenantID, null, staffRoleDescription),
                    StartDate = startDate,
                    EndDate = endDate,
                    TenantID = tenantID
                });
        }

        #endregion
    }
}
