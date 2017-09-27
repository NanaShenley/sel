using System;
using System.Linq;
using System.Threading;
using NUnit.Framework;
using POM.Components.Admission;
using POM.Helper;
using Pupil.Components.Common;
using Pupil.Data;
using SeSugar;
using SeSugar.Data;
using TestSettings;
using WebDriverRunner.internals;

namespace Pupil.Admissions.Tests
{
    public class SchoolIntakeTests
    {
        /// <summary>
        /// Exercise ability to create a new active 'School Intake' with an 'Admission Group' in the next academic year.
        /// </summary>
        [WebDriverTest(TimeoutSeconds = 1200, Enabled = true, Browsers = new[] { BrowserDefaults.Chrome }, 
            Groups = new[] { PupilTestGroups.SchoolIntake.Page, PupilTestGroups.SchoolIntake.Create, PupilTestGroups.Priority.Priority2 })]
        public void Create_New_Active_School_Intake()
        {
            const string admissionTerm = "Spring";
            const int numberOfPlannedAdmission = 31;
            const int capacity = 10;
            var admissionYear = String.Format("{0}/{1}", (DateTime.Now.Year), (DateTime.Now.Year + 1));
            var dateOfAdmission = new DateTime(DateTime.Today.Year, 8, 1).ToString("M/d/yyyy");
            var admissionGroupName = Utilities.GenerateRandomString(10, "SeAdmissionGroup");
            var yearGroup = Queries.GetFirstYearGroup().FullName;
            var schoolIntakeName = admissionYear + " - " + admissionTerm + " " + yearGroup;

            #region PRE-CONDITIONS:

            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.AdmissionsOfficer);
            SeleniumHelper.NavigateMenu("Tasks", "Admissions", "School Intakes");

            // Search and delete School intake if it is existing in database
            var schoolIntakeTriplet = new SchoolIntakeTriplet();
            schoolIntakeTriplet.SearchCriteria.SearchByName = schoolIntakeName;
            schoolIntakeTriplet.SearchCriteria.SearchYearAdmissionYear = admissionYear;
            var searchResults = schoolIntakeTriplet.SearchCriteria.Search();
            var schoolIntakeTile = searchResults.SingleOrDefault(t => t.Code.Trim().Equals(schoolIntakeName));
            schoolIntakeTriplet.SelectSearchTile(schoolIntakeTile);
            schoolIntakeTriplet.Delete();

            #endregion

            #region STEPS

            // Create new School Intake
            var schoolIntakePage = schoolIntakeTriplet.Create();
            schoolIntakePage.AdmissionYear = admissionYear;
            schoolIntakePage.AdmissionTerm = admissionTerm;
            Thread.Sleep(1000);
            schoolIntakePage.YearGroup = yearGroup;
            Thread.Sleep(1000);
            schoolIntakePage.NumberOfPlannedAdmissions = numberOfPlannedAdmission.ToString();
            Thread.Sleep(1000);
            schoolIntakePage.ClickOnAddNewAdmissionGroupButton();
            var admissionGroupTable = schoolIntakePage.AdmissionGrid;
            admissionGroupTable[0].Name = admissionGroupName;
            admissionGroupTable[0].DateOfAdmission = dateOfAdmission;
            admissionGroupTable[0].Capacity = capacity.ToString();
            schoolIntakePage.ClickSaveWithNoConfirmation();            
            Assert.AreEqual(true, schoolIntakePage.IsSuccessMessageIsDisplayed(), "Creating school intake has failed");

            // Search for new school intake
            schoolIntakeTriplet.SearchCriteria.SearchByName = schoolIntakeName;
            searchResults = schoolIntakeTriplet.SearchCriteria.Search();
            schoolIntakeTriplet.SearchCriteria.SearchYearAdmissionYear = admissionYear;
            schoolIntakeTile = searchResults.SingleOrDefault(t => t.Code.Equals(schoolIntakeName));
            schoolIntakePage = schoolIntakeTile.Click<SchoolIntakePage>();

            // Verify that a new school intake was added succesffully
            Assert.AreEqual(admissionYear, schoolIntakePage.AdmissionYear, "Creating school intake has failed");
            Assert.AreEqual(admissionTerm, schoolIntakePage.AdmissionTerm, "Creating school intake has failed");
            Assert.AreEqual(yearGroup, schoolIntakePage.YearGroup, "Creating school intake has failed");
            Assert.AreEqual(numberOfPlannedAdmission.ToString(), schoolIntakePage.NumberOfPlannedAdmissions, "Creating school intake has failed");
            Assert.AreEqual(schoolIntakeName, schoolIntakePage.Name, "Creating school intake has failed");
            Assert.AreEqual(true, schoolIntakePage.Active, "Creating school intake has failed");

            // Check admission group is created
            admissionGroupTable = schoolIntakePage.AdmissionGrid;
            Assert.AreNotEqual(null, (admissionGroupTable.Rows.SingleOrDefault(x => x.Name == admissionGroupName && x.DateOfAdmission == dateOfAdmission &&
                                        x.Capacity == capacity.ToString() && x.Active)), "Creating school intake has failed");
            schoolIntakeTriplet.Delete();

            #endregion
        }
       
        /// <summary>
        /// Exercise ability to delete a school intake and associated admission group that is NOT linked to any applications.
        /// </summary>
        [WebDriverTest(TimeoutSeconds = 1200, Enabled = true, Browsers = new[] { BrowserDefaults.Chrome }, 
            Groups = new[] { PupilTestGroups.SchoolIntake.Page, PupilTestGroups.SchoolIntake.Delete, PupilTestGroups.Priority.Priority2 })]
        public void Delete_Active_School_Intake()
        {
            var schoolIntakeId = Guid.NewGuid();
            const string admissionTerm = "Spring";
            const int numberOfPlannedAdmission = 31;
            const int capacity = 10;
            var admissionYear = String.Format("{0}/{1}", (DateTime.Now.Year), (DateTime.Now.Year + 1));
            var dateOfAdmission = new DateTime(DateTime.Today.Year, 8, 1);
            var admissionGroupName = Utilities.GenerateRandomString(10, "SeAdmissionGroup");
            var yearGroup = Queries.GetFirstYearGroup();
            var schoolIntakeName = admissionYear + " - " + admissionTerm + " " + yearGroup.FullName;
            
            var dataPackage = this.BuildDataPackage();
            dataPackage.AddSchoolIntake(
                schoolIntakeId, 
                admissionYear, 
                admissionTerm, 
                yearGroup, 
                numberOfPlannedAdmission, 
                admissionGroupName, 
                dateOfAdmission, 
                capacity);

            // Act
            using (new DataSetup(purgeBeforeInsert: false, purgeAfterTest: true, packages: dataPackage))
            {
                SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.AdmissionsOfficer);
                SeleniumHelper.NavigateMenu("Tasks", "Admissions", "School Intakes");
                
                // Search and delete School intake by Year group if it is existing in database
                var schoolIntakeTriplet = new SchoolIntakeTriplet();
                schoolIntakeTriplet.SearchCriteria.SearchByName = schoolIntakeName;
                schoolIntakeTriplet.SearchCriteria.SearchByYearGroup = yearGroup.FullName;
                var searchResults = schoolIntakeTriplet.SearchCriteria.Search();
                var schoolIntakeTile = searchResults.SingleOrDefault(t => t.Code.Equals(schoolIntakeName));
                var schoolIntakePage = schoolIntakeTile.Click<SchoolIntakePage>();
                schoolIntakeTriplet.CancelDeleteSchoolIntake();

                // Verify that a new school intake was canceled to delete successfully
                Assert.AreEqual(admissionYear, schoolIntakePage.AdmissionYear, "Cancel to delete school intake has failed");
                Assert.AreEqual(admissionTerm, schoolIntakePage.AdmissionTerm, "Cancel to delete intake has failed");
                Assert.AreEqual(yearGroup.FullName, schoolIntakePage.YearGroup, "Cancel to delete school intake has failed");
                Assert.AreEqual(numberOfPlannedAdmission.ToString(), schoolIntakePage.NumberOfPlannedAdmissions, "Cancel to delete school intake has failed");
                Assert.AreEqual((schoolIntakeName), schoolIntakePage.Name, "Cancel to delete school intake has failed");
                Assert.AreEqual(true, schoolIntakePage.Active, "Cancel to delete school intake has failed");
                var admissionGroupTable = schoolIntakePage.AdmissionGrid;
                Assert.AreNotEqual(null,
                    (admissionGroupTable.Rows.SingleOrDefault(
                        x => x.Name == admissionGroupName && x.DateOfAdmission == dateOfAdmission.ToString("M/d/yyyy") &&
                             x.Capacity == capacity.ToString() && x.Active)), "Creating school intake has failed");

                // Re-delete school intake
                schoolIntakeTriplet.Delete();

                //Verify that Delete school intake successfully
                schoolIntakeTriplet.SearchCriteria.SearchByYearGroup = yearGroup.FullName;
                searchResults = schoolIntakeTriplet.SearchCriteria.Search();
                schoolIntakeTile = searchResults.SingleOrDefault(t => t.Code.Equals(schoolIntakeName));
                Assert.AreEqual(null, schoolIntakeTile, "Delete school intake has failed");
            }
        }
    }

}