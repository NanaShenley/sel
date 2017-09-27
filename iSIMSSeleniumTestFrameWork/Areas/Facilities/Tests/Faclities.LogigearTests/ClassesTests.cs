using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using POM.Helper;
using WebDriverRunner.internals;
using NUnit.Framework;
using System.Globalization;
using TestSettings;
using POM.Components.SchoolGroups;
using POM.Components.Pupil;
using SeSugar;
using SeSugar.Data;
using POM.Components.Common;
using Selene.Support.Attributes;
using Facilities.Data;
using WebDriverRunner.webdriver;
using Environment = SeSugar.Environment;
using Facilities.Data.Entities;
using SeSugar.Automation;

namespace Faclities.LogigearTests
{
    public class ClassesTests
    {

        /// <summary>
        /// Author: Huy.Vo
        /// Description: Create new Classes with an Active History starting in the Next Academic Year and associate with the new Year Groups created for the next academic year
        /// </summary>
        [WebDriverTest(TimeoutSeconds = 2400, Enabled = true, DataProvider = "TC_MC001_Data", Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { "g1" })]
        public void TC_MC001_Add_New_Classes_Next_Academic_Year(string nextAcademicYear, string classFullName, string classShortName,
                                             string displayOrder, string activeHistoryStartDate, string activeHistoryEndDate,
                                             string yearGroup, string yearGroupStartDate, string yearGroupEndDate,
                                             string classTearcher, string classTeacherStartDate, string classTeacherEndDate,
                                             string staff, string pastoraleRole, string staffStartDate, string staffEndate)
        {

            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);
            Wait.WaitForDocumentReady();
            AutomationSugar.NavigateMenu("Tasks", "School Groups", "Manage Classes");
            Wait.WaitForDocumentReady();
            #region STEPS

            //  Search and delete if Class is existing
            var manageClassesTriplet = new ManageClassesTriplet();
            manageClassesTriplet.SearchCriteria.SearchByAcademicYear = nextAcademicYear;
            var classesResults = manageClassesTriplet.SearchCriteria.Search();
            var classesTile = classesResults.SingleOrDefault(t => t.ClassFullName.Equals(classFullName));
            manageClassesTriplet.SelectSearchTile(classesTile);
            manageClassesTriplet.Delete();

            // Create new Classe with Year Group
            var manageClassesPage = manageClassesTriplet.Create();
            manageClassesPage.ClassFullName = classFullName;
            manageClassesPage.ClassShortName = classShortName;
            manageClassesPage.DisplayOrder = displayOrder;
            manageClassesPage.AddActivehistory();

            //Active History table
            var activeHistoryTable = manageClassesPage.ActiveHistoryTable;
            activeHistoryTable[0].StartDate = activeHistoryStartDate;
            activeHistoryTable[0].EndDate = activeHistoryEndDate;
            manageClassesPage.Save();
            manageClassesPage.Refresh();
            Wait.WaitForDocumentReady();

            DataPackage dataPackage = this.BuildDataPackage();

            //Create School NC Year
            var schoolNcYearId = Guid.NewGuid();
            dataPackage.AddSchoolNCYearLookup(schoolNcYearId);
            //Create YearGroup and its set memberships
            var yearGroupId = Guid.NewGuid();
            var yearGroupShortName = Utilities.GenerateRandomString(3, "SNADD");
            var yearGroupFullName = Utilities.GenerateRandomString(10, "FNADD");
            dataPackage.AddYearGroupLookup(yearGroupId, schoolNcYearId, yearGroupShortName, yearGroupFullName, 1);

            //Employee Details Data Injection
            var employeeId = Guid.NewGuid();
            dataPackage.AddEmployee(employeeId);

            //Staff Details Data Injection
            var staffId = Guid.NewGuid();
            var surname = Utilities.GenerateRandomString(10, "Surname");
            var forename = Utilities.GenerateRandomString(3, "Forename");
            dataPackage.AddStaff(staffId, employeeId, surname, forename);

            var serviceRecordId = Guid.NewGuid();
            var staffDOA = DateTime.Now;
            dataPackage.AddServiceRecord(serviceRecordId, staffId, staffDOA, null);

            using (new DataSetup(purgeBeforeInsert: false, purgeAfterTest: true, packages: dataPackage))
            {

                //// Scroll to Associated Group
                manageClassesPage.ScrollToAssociatedGroup();
                Wait.WaitForDocumentReady();
                manageClassesPage.AddYearGroup();

                //// Add new record for Year Group
                var yearGroupTable = manageClassesPage.YearGroupsTable;
                yearGroupTable[0].YearGroup = yearGroupFullName;
                yearGroupTable[0].StartDate = yearGroupStartDate;
                yearGroupTable[0].EndDate = yearGroupEndDate;

                // Scoll to Staff Details
                manageClassesPage.ScrollToStaffDetails();
                manageClassesPage.AddClassTeacher();

                // Add new record for Class Teacher
                var clstaff = string.Concat(surname, ", ", forename);
                var classTeacherTable = manageClassesPage.ClassTeacherTable;
                classTeacherTable[0].SelectClassTeacher = clstaff;
                classTeacherTable[0].StartDate = classTeacherStartDate;
                classTeacherTable[0].EndDate = classTeacherEndDate;

                // Add new record for Staff
                manageClassesPage.AddStaff();
                var staffTable = manageClassesPage.StaffTable;
                staffTable[0].SelectStaff = clstaff;
                staffTable[0].PastoralRoleDropDown = pastoraleRole;
                staffTable[0].StartDate = staffStartDate;
                staffTable[0].EndDate = staffEndate;

                // Save data
                manageClassesPage.Save();
                //manageClassesPage.Refresh();

                // Verify that save message succesfully display
                Assert.AreEqual(true, manageClassesPage.IsSuccessMessageDisplayed(), "Create new Class unsuccessfully");

                // Search new class to verify that data was saved successfully
                manageClassesTriplet.SearchCriteria.SearchByAcademicYear = nextAcademicYear;
                classesResults = manageClassesTriplet.SearchCriteria.Search();
                classesTile = classesResults.SingleOrDefault(t => t.ClassFullName.Equals(classFullName));
                manageClassesPage = classesTile.Click<ManageClassesPage>();

                // Verify that new informations of Class are displayed on screen
                Assert.AreEqual(classFullName, manageClassesPage.ClassFullName, "Class fullname is not equal");
                Assert.AreEqual(classShortName, manageClassesPage.ClassShortName, "Class shortname is not equal");
                Assert.AreEqual(displayOrder, manageClassesPage.DisplayOrder, "Display order is not equal");

                //Verify Active history data
                activeHistoryTable = manageClassesPage.ActiveHistoryTable;
                Assert.AreNotEqual(null,
                    (activeHistoryTable.Rows.SingleOrDefault(x => x.StartDate == activeHistoryStartDate)),
                    "Active history is not equal");

                //Verify Year Group data
                yearGroupTable = manageClassesPage.YearGroupsTable;
                Assert.AreNotEqual(null,
                    (yearGroupTable.Rows.SingleOrDefault(
                        x => x.YearGroup == yearGroupFullName && x.StartDate == yearGroupStartDate &&
                             x.EndDate == yearGroupEndDate)), "Year group is not equal");

                //Verify Class Teacher data
                classTeacherTable = manageClassesPage.ClassTeacherTable;
                Assert.AreNotEqual(null,
                    (classTeacherTable.Rows.SingleOrDefault(
                        x => x.SelectClassTeacher == clstaff && x.StartDate == classTeacherStartDate &&
                             x.EndDate == classTeacherEndDate)), "Class Teacher is not equal");

                // Verify Staff data
                staffTable = manageClassesPage.StaffTable;
                Assert.AreNotEqual(null,
                    (staffTable.Rows.SingleOrDefault(
                        x => x.SelectStaff == clstaff && x.PastoralRoleDropDown == pastoraleRole &&
                             x.StartDate == staffStartDate && x.EndDate == staffEndate)), "Class Teacher is not equal");

                // Verify new class displays in Promote Pupils
                AutomationSugar.NavigateMenu("Tasks", "School Groups", "Promote Pupils");
                Wait.WaitForDocumentReady();
                //PromotePupilsTriplet promotePupilTriplet = new PromotePupilsTriplet();
                //var classGroup = promotePupilTriplet.SearchCriteria.Classes;
                //var classItem = classGroup[classFullName];
                //Assert.AreNotEqual(null, classItem, "The Class does not display in Promote Pupil page");

                //// Verify new class in Allocate Pupils in Allocate Future Pupils
                AutomationSugar.NavigateMenu("Tasks", "School Groups", "Allocate Future Pupils");
                Wait.WaitForDocumentReady();
               // var allocateFuture = new AllocateFuturePupilsTriplet();
                //classGroup = allocateFuture.SearchCriteria.Classes;
                //classItem = classGroup[classFullName];
                //Assert.AreNotEqual(null, classItem, "The Class doesn't display in Allocate Future Pupils");

                // Verify new class in Allocate Pupils to Groups
                AutomationSugar.NavigateMenu("Tasks", "School Groups", "Allocate Pupils to Groups");
                Wait.WaitForDocumentReady();
                var allowCatePupilToGroup = new AllocatePupilsToGroupsTriplet();
               var classGroup = allowCatePupilToGroup.SearchCriteria.Classes;
               var classItem = classGroup[classFullName];
                Assert.AreNotEqual(null, classItem, "The Class doesn't display in Allocate Pupils to Groups");

            #endregion STEPS

                #region POS-CONDITIONS

                //Re-select Manage Classes to delete data
                AutomationSugar.NavigateMenu("Tasks", "School Groups", "Manage Classes");
                Wait.WaitForDocumentReady();
                manageClassesTriplet = new ManageClassesTriplet();
                manageClassesTriplet.SearchCriteria.SearchByAcademicYear = nextAcademicYear;
                classesResults = manageClassesTriplet.SearchCriteria.Search();
                classesTile = classesResults.SingleOrDefault(t => t.ClassFullName.Equals(classFullName));
                manageClassesPage = classesTile.Click<ManageClassesPage>();

                // Remove record in Year Group
                yearGroupTable = manageClassesPage.YearGroupsTable;
                var yearGroupRow = yearGroupTable.Rows.FirstOrDefault(t => t.YearGroup.Equals(yearGroupFullName));
                yearGroupRow.DeleteRow();
                yearGroupTable.Refresh();

                //Remove record in Class Teacher
                classTeacherTable = manageClassesPage.ClassTeacherTable;
                var classTeacherRow = classTeacherTable.Rows.FirstOrDefault(t => t.SelectClassTeacher.Equals(clstaff));
                classTeacherRow.DeleteRow();
                classTeacherTable.Refresh();

                //Remove record in Staff
                staffTable = manageClassesPage.StaffTable;
                var staffRow = staffTable.Rows.FirstOrDefault(t => t.SelectStaff.Equals(clstaff) && t.PastoralRoleDropDown.Equals(pastoraleRole));
                staffRow.DeleteRow();
                staffTable.Refresh();

                manageClassesPage.Save();
                manageClassesPage.Refresh();

                // Delete Class
                var confirmDeleteDialog = manageClassesPage.Delete();
                confirmDeleteDialog.ConfirmDelete();

                #endregion POS-CONDITIONS
            }

        }

        /// <summary>
        /// Author: Huy.Vo
        /// Description: Amend a Class with an Active History starting in the Next Academic Year associated with year Groups for Next academic year
        /// </summary>
        [WebDriverTest(TimeoutSeconds = 2400, Enabled = false, DataProvider = "TC_MC002_Data", Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { "g1" })]
        public void TC_MC002_Udate_Classes_Next_Academic_Year(string nextAcademicYear, string classFullName, string classShortName, string displayOrder,
                                             string activeHistoryStartDate, string activeHistoryEndDate,
                                             string yearGroup, string yearGroupStartDate, string yearGroupEndDate,
                                             string classTearcher, string classTeacherStartDate, string classTeacherEndDate,
                                             string staff, string pastoraleRole, string staffStartDate, string staffEndate,
                                             string classFullNameUpdate, string classShortNameUpdate, string displayOrderUpdate,
                                             string activeHistoryStartDateUpdate, string activeHistoryEndDateUpdate,
                                             string yearGroupUpdate, string yearGroupStartDateUpdate, string yearGroupEndDateUpdate,
                                             string classTearcherUpdate, string classTeacherStartDateUpdate, string classTeacherEndDateUpdate,
                                             string staffUpdate, string pastoraleRoleUpdate, string staffStartDateUpdate, string staffEndateUpdate)
        {

            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);
            Wait.WaitForDocumentReady();
            AutomationSugar.NavigateMenu("Tasks", "School Groups", "Manage Classes");
            Wait.WaitForDocumentReady();
            #region STEPS

            //  Search and delete if Class is existing
            var manageClassesTriplet = new ManageClassesTriplet();
            manageClassesTriplet.SearchCriteria.SearchByAcademicYear = nextAcademicYear;
            var classesResults = manageClassesTriplet.SearchCriteria.Search();
            var classesTile = classesResults.SingleOrDefault(t => t.ClassFullName.Equals(classFullName));
            manageClassesTriplet.SelectSearchTile(classesTile);
            manageClassesTriplet.Delete();

            // Create new Classe with Year Group
            var manageClassesPage = manageClassesTriplet.Create();
            manageClassesPage.ClassFullName = classFullName;
            manageClassesPage.ClassShortName = classShortName;
            manageClassesPage.DisplayOrder = displayOrder;
            manageClassesPage.AddActivehistory();

            //Active History table
            var activeHistoryTable = manageClassesPage.ActiveHistoryTable;
            activeHistoryTable[0].StartDate = activeHistoryStartDate;
            activeHistoryTable[0].EndDate = activeHistoryEndDate;
            manageClassesPage.Save();
            manageClassesPage.Refresh();
            Wait.WaitForDocumentReady();

            DataPackage dataPackage = this.BuildDataPackage();

            //Create School NC Year
            var schoolNcYearId = Guid.NewGuid();
            dataPackage.AddSchoolNCYearLookup(schoolNcYearId);
            //Create YearGroup and its set memberships
            var yearGroupId = Guid.NewGuid();
            var yearGroupShortName = Utilities.GenerateRandomString(3, "SNADD");
            var yearGroupFullName = Utilities.GenerateRandomString(10, "FNADD");
            dataPackage.AddYearGroupLookup(yearGroupId, schoolNcYearId, yearGroupShortName, yearGroupFullName, 1);

            //Employee Details Data Injection
            var employeeId = Guid.NewGuid();
            dataPackage.AddEmployee(employeeId);

            //Staff Details Data Injection
            var staffId = Guid.NewGuid();
            var surname = Utilities.GenerateRandomString(10, "Surname");
            var forename = Utilities.GenerateRandomString(3, "Forename");
            dataPackage.AddStaff(staffId, employeeId, surname, forename);

            var serviceRecordId = Guid.NewGuid();
            var staffDOA = DateTime.Now;
            dataPackage.AddServiceRecord(serviceRecordId, staffId, staffDOA, null);

            //Add the Updated Record

            //Create School NC Year
            var schoolNcYearIdUpdated = Guid.NewGuid();
            dataPackage.AddSchoolNCYearLookup(schoolNcYearIdUpdated);
            //Create YearGroup and its set memberships
            var yearGroupIdUpdated = Guid.NewGuid();
            var yearGroupShortNameUpdated = Utilities.GenerateRandomString(3, "SNADD");
            var yearGroupFullNameUpdated = Utilities.GenerateRandomString(10, "FNADD");
            dataPackage.AddYearGroupLookup(yearGroupIdUpdated, schoolNcYearIdUpdated, yearGroupShortNameUpdated, yearGroupFullNameUpdated, 2);

            //Employee Details Data Injection
            var employeeIdUpdated = Guid.NewGuid();
            dataPackage.AddEmployee(employeeIdUpdated);

            //Staff Details Data Injection
            var staffIdUpdated = Guid.NewGuid();
            var surnameUpdated = Utilities.GenerateRandomString(10, "Surname");
            var forenameUpdated = Utilities.GenerateRandomString(3, "Forename");
            dataPackage.AddStaff(staffIdUpdated, employeeIdUpdated, surnameUpdated, forenameUpdated);

            var serviceRecordIdUpdated = Guid.NewGuid();
            var staffDOAUpdated = DateTime.Now;
            dataPackage.AddServiceRecord(serviceRecordIdUpdated, staffIdUpdated, staffDOAUpdated, null);

            using (new DataSetup(purgeBeforeInsert: false, purgeAfterTest: true, packages: dataPackage))
            {

                //// Scroll to Associated Group
                manageClassesPage.ScrollToAssociatedGroup();
                Wait.WaitForDocumentReady();
                manageClassesPage.AddYearGroup();

                //// Add new record for Year Group
                var yearGroupTable = manageClassesPage.YearGroupsTable;
                yearGroupTable[0].YearGroup = yearGroupFullName;
                yearGroupTable[0].StartDate = yearGroupStartDate;
                yearGroupTable[0].EndDate = yearGroupEndDate;

                // Scoll to Staff Details
                manageClassesPage.ScrollToStaffDetails();
                manageClassesPage.AddClassTeacher();

                // Add new record for Class Teacher
                var clstaff = string.Concat(surname, ", ", forename);
                var classTeacherTable = manageClassesPage.ClassTeacherTable;
                classTeacherTable[0].SelectClassTeacher = clstaff;
                classTeacherTable[0].StartDate = classTeacherStartDate;
                classTeacherTable[0].EndDate = classTeacherEndDate;

                // Add new record for Staff
                manageClassesPage.AddStaff();
                var staffTable = manageClassesPage.StaffTable;
                staffTable[0].SelectStaff = clstaff;
                staffTable[0].PastoralRoleDropDown = pastoraleRole;
                staffTable[0].StartDate = staffStartDate;
                staffTable[0].EndDate = staffEndate;

                // Save data
                manageClassesPage.Save();
                manageClassesPage.Refresh();

                // Select Class to update Class
                manageClassesTriplet.SearchCriteria.SearchByAcademicYear = nextAcademicYear;
                classesResults = manageClassesTriplet.SearchCriteria.Search();
                classesTile = classesResults.SingleOrDefault(t => t.ClassFullName.Equals(classFullName));
                manageClassesPage = classesTile.Click<ManageClassesPage>();

                //Update infor
                manageClassesPage.ClassFullName = classFullNameUpdate;
                manageClassesPage.ClassShortName = classShortNameUpdate;
                manageClassesPage.DisplayOrder = displayOrderUpdate;

                // Save data
                manageClassesPage.Save();
                manageClassesPage.Refresh();

                // Update data for Active History table
                activeHistoryTable = manageClassesPage.ActiveHistoryTable;
                activeHistoryTable[0].StartDate = activeHistoryStartDateUpdate;
                activeHistoryTable[0].EndDate = activeHistoryEndDateUpdate;

              //  var dialogConfirm = manageClassesPage.SaveToDeActive();
              //  dialogConfirm.ClickOk();
                manageClassesPage.Save();
                manageClassesPage.Refresh();

                // Update data for Year group data
                yearGroupTable = manageClassesPage.YearGroupsTable;
                yearGroupTable[0].YearGroup = yearGroupFullNameUpdated;
                yearGroupTable[0].StartDate = yearGroupStartDateUpdate;
                yearGroupTable[0].EndDate = yearGroupEndDateUpdate;

                var clstaffUpdated = string.Concat(surname, ", ", forename);
                //Update data for Class Teacher table
                classTeacherTable = manageClassesPage.ClassTeacherTable;
                classTeacherTable[0].SelectClassTeacher = clstaffUpdated;
                classTeacherTable[0].StartDate = classTeacherStartDateUpdate;
                classTeacherTable[0].EndDate = classTeacherEndDateUpdate;

                // Update data for Staff table
                staffTable = manageClassesPage.StaffTable;
                staffTable[0].SelectStaff = clstaffUpdated;
                staffTable[0].PastoralRoleDropDown = pastoraleRoleUpdate;
                staffTable[0].StartDate = staffStartDateUpdate;
                staffTable[0].EndDate = staffEndateUpdate;
                
                // Save data
                manageClassesPage.Save();
                manageClassesPage.Refresh();

                // Verify that save message update succesfully display
                Assert.AreEqual(true, manageClassesPage.IsSuccessMessageDisplayed(), "Create new Class unsuccessfully");

                // Verify that create Class successfully
                manageClassesTriplet.SearchCriteria.SearchByAcademicYear = nextAcademicYear;
                classesResults = manageClassesTriplet.SearchCriteria.Search();
                classesTile = classesResults.SingleOrDefault(t => t.ClassFullName.Equals(classFullNameUpdate));
                manageClassesPage = classesTile.Click<ManageClassesPage>();

                // Verify that new informations of Class are displayed on screen
                Assert.AreEqual(classFullNameUpdate, manageClassesPage.ClassFullName,
                    "Class fullname update is not equal");
                Assert.AreEqual(classShortNameUpdate, manageClassesPage.ClassShortName,
                    "Class shortname update is not equal");
                Assert.AreEqual(displayOrderUpdate, manageClassesPage.DisplayOrder, "Display order update is not equal");

                //Verify Active history data
                activeHistoryTable = manageClassesPage.ActiveHistoryTable;
                Assert.AreNotEqual(null,
                    (activeHistoryTable.Rows.SingleOrDefault(x => x.StartDate == activeHistoryStartDateUpdate
                                                                  && x.EndDate == activeHistoryEndDateUpdate)),
                    "Active history update is not equal");
                //Verify Year Group data
                yearGroupTable = manageClassesPage.YearGroupsTable;
                Assert.AreNotEqual(null,
                    (yearGroupTable.Rows.SingleOrDefault(
                        x => x.YearGroup == yearGroupFullNameUpdated && x.StartDate == yearGroupStartDateUpdate &&
                             x.EndDate == yearGroupEndDateUpdate)), "Year group update is not equal");

                //Verify Class Teacher data
                classTeacherTable = manageClassesPage.ClassTeacherTable;
                Assert.AreNotEqual(null,
                    (classTeacherTable.Rows.SingleOrDefault(
                        x =>
                            x.SelectClassTeacher == clstaffUpdated && x.StartDate == classTeacherStartDateUpdate &&
                            x.EndDate == classTeacherEndDateUpdate)), "Class Teacher update is not equal");

                // Verify Staff data
                staffTable = manageClassesPage.StaffTable;
                Assert.AreNotEqual(null,
                    (staffTable.Rows.SingleOrDefault(
                        x => x.SelectStaff == clstaffUpdated && x.PastoralRoleDropDown == pastoraleRoleUpdate &&
                             x.StartDate == staffStartDateUpdate && x.EndDate == staffEndateUpdate)),
                    "Class Teacher update is not equal");

                //// Verify class updating is displayed in Promote Pupils
                AutomationSugar.NavigateMenu("Tasks", "School Groups", "Promote Pupils");
                Wait.WaitForDocumentReady();
                PromotePupilsTriplet promotePupilTriplet = new PromotePupilsTriplet();
                var classGroup = promotePupilTriplet.SearchCriteria.Classes;
                var classItem = classGroup[classFullNameUpdate];
                Assert.AreNotEqual(null, classItem, "The Class updating does not display in Promote Pupil page");

                // Verify class updating is not displayed in Allocate Pupils to Groups
                AutomationSugar.NavigateMenu("Tasks", "School Groups", "Allocate Pupils to Groups");
                Wait.WaitForDocumentReady();
                var allowCatePupilToGroup = new AllocatePupilsToGroupsTriplet();
                 classGroup = allowCatePupilToGroup.SearchCriteria.Classes;
                 classItem = classGroup[classFullNameUpdate];
                Assert.AreNotEqual(null, classItem, "The Class updating doesn't display in Allocate Pupils to Groups");

                //// Verify class updating in Allocate Future Pupils
                AutomationSugar.NavigateMenu("Tasks", "School Groups", "Allocate Future Pupils");
                Wait.WaitForDocumentReady();
                var allocateFuture = new AllocateFuturePupilsTriplet();
                classGroup = allocateFuture.SearchCriteria.Classes;
                classItem = classGroup[classFullNameUpdate];
                Assert.AreNotEqual(null, classItem, "The Class updating doesn't displays in Allocate Future Pupils");

            #endregion STEPS

                #region POS-CONDITIONS

                //Re-select class page to delete data
                AutomationSugar.NavigateMenu("Tasks", "School Groups", "Manage Classes");

                manageClassesTriplet = new ManageClassesTriplet();
                manageClassesTriplet.SearchCriteria.SearchByAcademicYear = nextAcademicYear;
                classesResults = manageClassesTriplet.SearchCriteria.Search();
                classesTile = classesResults.SingleOrDefault(t => t.ClassFullName.Equals(classFullNameUpdate));
                manageClassesPage = classesTile.Click<ManageClassesPage>();

                // Remove Year Group
                yearGroupTable = manageClassesPage.YearGroupsTable;
                var yearGroupRow = yearGroupTable.Rows.FirstOrDefault(t => t.YearGroup.Equals(yearGroupFullNameUpdated));
                yearGroupRow.DeleteRow();
                yearGroupTable.Refresh();

                //Remove class Teacher
                classTeacherTable = manageClassesPage.ClassTeacherTable;
                var classTeacherRow =
                    classTeacherTable.Rows.FirstOrDefault(t => t.SelectClassTeacher.Equals(clstaffUpdated));
                classTeacherRow.DeleteRow();
                classTeacherTable.Refresh();

                //Remove class Teacher
                staffTable = manageClassesPage.StaffTable;
                var staffRow =
                    staffTable.Rows.FirstOrDefault(
                        t => t.SelectStaff.Equals(clstaffUpdated) && t.PastoralRoleDropDown.Equals(pastoraleRoleUpdate));
                staffRow.DeleteRow();
                staffTable.Refresh();

                manageClassesPage.Save();
                manageClassesPage.Refresh();

                // Delete class
                var confirmDeleteDialog = manageClassesPage.Delete();
                confirmDeleteDialog.ConfirmDelete();

                #endregion POS-CONDITIONS
            }
        }


        /// <summary>
        /// Author: Hieu Pham
        /// Description: Create new Class  for the current academic year with an Active History Starting in the current Academic Year but with an Open End Date (null) .
        /// </summary>
        /// 
        [WebDriverTest(TimeoutSeconds = 2400, Enabled = true, DataProvider = "TC_MC003_Data", Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { "g1" })]
        public void TC_MC003_Add_New_Classes_With_EndDate_Is_Null(string classFullName, string classShortName, string displayOrder,
                                             string activeHistoryStartDate, string yearGroup, string yearGroupStartDate,
                                             string classTearcher, string classTeacherStartDate,
                                             string staff, string pastoraleRole, string staffStartDate)
        {

            #region STEPS

            // Login
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);
            Wait.WaitForDocumentReady();
            // Navigate to Manage classes
            AutomationSugar.NavigateMenu("Tasks", "School Groups", "Manage Classes");
            Wait.WaitForDocumentReady();

            //  Search and delete if Class is existing
            var manageClassesTriplet = new ManageClassesTriplet();
            var classesResults = manageClassesTriplet.SearchCriteria.Search();
            var classesTile = classesResults.FirstOrDefault(t => t.ClassFullName.Equals(classFullName));
            var manageClassPage = classesTile == null ? new ManageClassesPage() : classesTile.Click<ManageClassesPage>();
            manageClassPage.DeleteRecord();

            // Create new Classes with Year Group
            var manageClassesPage = manageClassesTriplet.Create();
            manageClassesPage.ClassFullName = classFullName;
            manageClassesPage.ClassShortName = classShortName;
            manageClassesPage.DisplayOrder = displayOrder;

            // Create record in Active History table
            manageClassesPage.AddActivehistory();
            var activeHistoryTable = manageClassesPage.ActiveHistoryTable;
            activeHistoryTable[0].StartDate = activeHistoryStartDate;
            manageClassesPage.Save();
            manageClassesPage.Refresh();
            Wait.WaitForDocumentReady();

                 DataPackage dataPackage = this.BuildDataPackage();

            //Create School NC Year
            var schoolNcYearId = Guid.NewGuid();
            dataPackage.AddSchoolNCYearLookup(schoolNcYearId);
            //Create YearGroup and its set memberships
            var yearGroupId = Guid.NewGuid();
            var yearGroupShortName = Utilities.GenerateRandomString(3, "SNADD");
            var yearGroupFullName = Utilities.GenerateRandomString(10, "FNADD");
            dataPackage.AddYearGroupLookup(yearGroupId, schoolNcYearId, yearGroupShortName, yearGroupFullName, 1);

            //Employee Details Data Injection
            var employeeId = Guid.NewGuid();
            dataPackage.AddEmployee(employeeId);

            //Staff Details Data Injection
            var staffId = Guid.NewGuid();
            var surname = Utilities.GenerateRandomString(10, "Surname");
            var forename = Utilities.GenerateRandomString(3, "Forename");
            dataPackage.AddStaff(staffId, employeeId, surname, forename);

            var serviceRecordId = Guid.NewGuid();
            var staffDOA = DateTime.Now;
            dataPackage.AddServiceRecord(serviceRecordId, staffId, staffDOA, null);

            using (new DataSetup(purgeBeforeInsert: false, purgeAfterTest: true, packages: dataPackage))
            {

                //// Scroll to Associated Group
                manageClassesPage.ScrollToAssociatedGroup();
                Wait.WaitForDocumentReady();
                manageClassesPage.AddYearGroup();

                // Create Record in Year Group table
                var yearGroupTable = manageClassesPage.YearGroupsTable;
                yearGroupTable[0].YearGroup = yearGroupFullName;
                yearGroupTable[0].StartDate = yearGroupStartDate;

                // Scroll to Staff Details
                manageClassesPage.ScrollToStaffDetails();
                manageClassesPage.AddClassTeacher();

                // Add new record for Class Teacher
                var clstaff = string.Concat(surname, ", ", forename);

                // Input data for Class Teacher table
                var classTeacherTable = manageClassesPage.ClassTeacherTable;
                classTeacherTable[0].SelectClassTeacher = clstaff;
                classTeacherTable[0].StartDate = classTeacherStartDate;

                // Input data for Staff table
                manageClassesPage.AddStaff();
                var staffTable = manageClassesPage.StaffTable;
                staffTable[0].SelectStaff = clstaff;
                staffTable[0].PastoralRoleDropDown = pastoraleRole;
                staffTable[0].StartDate = staffStartDate;

                // Save data
                manageClassesPage.Save();

                // Verify that save message succesfully display
                Assert.AreEqual(true, manageClassesPage.IsSuccessMessageDisplayed(), "Create new Class unsuccessfully");

                // Search record again
                manageClassesTriplet.Refresh();
                classesResults = manageClassesTriplet.SearchCriteria.Search();
                classesTile = classesResults.FirstOrDefault(t => t.ClassFullName.Equals(classFullName));
                manageClassesPage = classesTile == null
                    ? new ManageClassesPage()
                    : classesTile.Click<ManageClassesPage>();

                // Verify that new informations of Class are displayed on screen
                Assert.AreEqual(classFullName, manageClassesPage.ClassFullName, "Class fullname is not correct");
                Assert.AreEqual(classShortName, manageClassesPage.ClassShortName, "Class shortname is not correct");
                Assert.AreEqual(displayOrder, manageClassesPage.DisplayOrder, "Display order is not correct");

                // Verify Active history data is correct
                activeHistoryTable.Refresh();
                Assert.AreNotEqual(null,
                    activeHistoryTable.Rows.FirstOrDefault(x => x.StartDate.Equals(activeHistoryStartDate)),
                    "Active history is not correct");

                //Verify Year Group data is correct
                yearGroupTable.Refresh();
                Assert.AreNotEqual(null,
                    yearGroupTable.Rows.FirstOrDefault(
                        x => x.YearGroup.Equals(yearGroupFullName) && x.StartDate.Equals(yearGroupStartDate)),
                    "Year group is not correct");

                //Verify Class Teacher data
                classTeacherTable.Refresh();
                Assert.AreNotEqual(null,
                    classTeacherTable.Rows.FirstOrDefault(
                        x => x.SelectClassTeacher.Equals(clstaff) && x.StartDate.Equals(classTeacherStartDate)),
                    "Class Teacher is not correct");

                // Verify Staff data
                staffTable.Refresh();
                Assert.AreNotEqual(null,
                    staffTable.Rows.FirstOrDefault(
                        x => x.SelectStaff.Equals(clstaff) && x.PastoralRoleDropDown.Equals(pastoraleRole) &&
                             x.StartDate.Equals(staffStartDate)), "Class Teacher is not correct");



                // Navigate to Allocate Future Pupils
                AutomationSugar.NavigateMenu("Tasks", "School Groups", "Allocate Future Pupils");

                // VP : The Pastoral Changes are correctly reflected on the Search Options on Screens: Allocate New Intake
                //var allocateFuturePupilTriplet = new AllocateFuturePupilsTriplet();
                //Assert.AreNotEqual(null, allocateFuturePupilTriplet.SearchCriteria.Classes[classFullName],
                //    "Allocate New Intake does not contains class");

                // Navigate to Allocate Pupils to Groups
                AutomationSugar.NavigateMenu("Tasks", "School Groups", "Allocate Pupils to Groups");

                // VP : The Pastoral Changes are correctly reflected on the Search Options on Screens: Allocate Pupils To Groups
                var allocatePupilToGroupTriplet = new AllocatePupilsToGroupsTriplet();
                Assert.AreNotEqual(null, allocatePupilToGroupTriplet.SearchCriteria.Classes[classFullName],
                    "Allocate Pupils To Groups does not contains class");

                // Navigate to Promote Pupils
               // AutomationSugar.NavigateMenu("Tasks", "School Groups", "Promote Pupils");

                // VP : The Pastoral Changes are correctly reflected on the Search Options on Screens: Promote Pupils
                //var promotePupilsTriplet = new PromotePupilsTriplet();
                //Assert.AreNotEqual(null, promotePupilsTriplet.SearchCriteria.Classes[classFullName],
                //    "Promote Pupils does not contains class");

                #endregion

                #region POS-CONDITIONS

                // Navigate to Manage classes
                AutomationSugar.NavigateMenu("Tasks", "School Groups", "Manage Classes");

                // Search record again
                manageClassesTriplet = new ManageClassesTriplet();
                classesResults = manageClassesTriplet.SearchCriteria.Search();
                classesTile = classesResults.FirstOrDefault(t => t.ClassFullName.Equals(classFullName));
                manageClassesPage = classesTile.Click<ManageClassesPage>();

                // Remove record in Year Group
                yearGroupTable = manageClassesPage.YearGroupsTable;
                var yearGroupRow = yearGroupTable.Rows.FirstOrDefault(t => t.YearGroup.Equals(yearGroupFullName));
                yearGroupRow.DeleteRow();
                yearGroupTable.Refresh();

                //Remove record in Class Teacher
                classTeacherTable = manageClassesPage.ClassTeacherTable;
                var classTeacherRow = classTeacherTable.Rows.FirstOrDefault(t => t.SelectClassTeacher.Equals(clstaff));
                classTeacherRow.DeleteRow();
                classTeacherTable.Refresh();

                //Remove record in Staff
                staffTable = manageClassesPage.StaffTable;
                var staffRow = staffTable.Rows.FirstOrDefault(t => t.SelectStaff.Equals(clstaff) && t.PastoralRoleDropDown.Equals(pastoraleRole));
                staffRow.DeleteRow();
                staffTable.Refresh();

                // Save values
                manageClassesPage.Save();

                // Delete record
                manageClassesPage.Refresh();
                manageClassesPage.DeleteRecord();

                #endregion POS-CONDITIONS
            }

        }

        /// <summary>
        /// Author: Huy.Vo
        /// Description: Amend a Class for the Current Academic Year with an Open Ended Active History date and end it in a future Academic Year 
        /// </summary>
        [WebDriverTest(TimeoutSeconds = 2400, Enabled = false, DataProvider = "TC_MC004_Data", Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { "g1" })]
        public void TC_MC004_Udate_Classes_EndDate_Active_History_In_Future(string currentAcademicYear, string classFullName, string classShortName,
                                             string displayOrder, string activeHistoryStartDate, string activeHistoryEndDate,
                                             string yearGroup, string yearGroupStartDate, string yearGroupEndDate,
                                             string classTearcher, string classTeacherStartDate, string classTeacherEndDate,
                                             string staff, string pastoraleRole, string staffStartDate, string staffEndate, string classFullNameUpdate, string classShortNameUpdate,
                                             string displayOrderUpdate, string activeHistoryStartDateUpdate, string activeHistoryEndDateUpdate,
                                             string yearGroupUpdate, string yearGroupStartDateUpdate, string yearGroupEndDateUpdate,
                                             string classTearcherUpdate, string classTeacherStartDateUpdate, string classTeacherEndDateUpdate,
                                             string staffUpdate, string pastoraleRoleUpdate, string staffStartDateUpdate, string staffEndateUpdate)
        {

            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);
            Wait.WaitForDocumentReady();
            // Navigate to Manage classes
            AutomationSugar.NavigateMenu("Tasks", "School Groups", "Manage Classes");
            Wait.WaitForDocumentReady();

            #region PRE-CONDITIONS

            //  Search and delete if Class is existing
            var manageClassesTriplet = new ManageClassesTriplet();
            manageClassesTriplet.SearchCriteria.SearchByAcademicYear = currentAcademicYear;
            var classesResults = manageClassesTriplet.SearchCriteria.Search();
            var classesTile = classesResults.SingleOrDefault(t => t.ClassFullName.Equals(classFullName));
            manageClassesTriplet.SelectSearchTile(classesTile);
            manageClassesTriplet.Delete();

            // Create new Classe with Year Group
            var manageClassesPage = manageClassesTriplet.Create();
            manageClassesPage.ClassFullName = classFullName;
            manageClassesPage.ClassShortName = classShortName;
            manageClassesPage.DisplayOrder = displayOrder;

            //Active History table
            manageClassesPage.AddActivehistory();
            var activeHistoryTable = manageClassesPage.ActiveHistoryTable;
            activeHistoryTable[0].StartDate = activeHistoryStartDate;
            activeHistoryTable[0].EndDate = activeHistoryEndDate;
              manageClassesPage.Save();
            manageClassesPage.Refresh();
            Wait.WaitForDocumentReady();

                 DataPackage dataPackage = this.BuildDataPackage();

            //Create School NC Year
            var schoolNcYearId = Guid.NewGuid();
            dataPackage.AddSchoolNCYearLookup(schoolNcYearId);
            //Create YearGroup and its set memberships
            var yearGroupId = Guid.NewGuid();
            var yearGroupShortName = Utilities.GenerateRandomString(3, "SNADD");
            var yearGroupFullName = Utilities.GenerateRandomString(10, "FNADD");
            dataPackage.AddYearGroupLookup(yearGroupId, schoolNcYearId, yearGroupShortName, yearGroupFullName, 1);

            //Employee Details Data Injection
            var employeeId = Guid.NewGuid();
            dataPackage.AddEmployee(employeeId);

            //Staff Details Data Injection
            var staffId = Guid.NewGuid();
            var surname = Utilities.GenerateRandomString(10, "Surname");
            var forename = Utilities.GenerateRandomString(3, "Forename");
            dataPackage.AddStaff(staffId, employeeId, surname, forename);

            var serviceRecordId = Guid.NewGuid();
            var staffDOA = DateTime.Now;
            dataPackage.AddServiceRecord(serviceRecordId, staffId, staffDOA, null);

            //Add the Updated Record

            //Create School NC Year
            var schoolNcYearIdUpdated = Guid.NewGuid();
            dataPackage.AddSchoolNCYearLookup(schoolNcYearIdUpdated);
            //Create YearGroup and its set memberships
            var yearGroupIdUpdated = Guid.NewGuid();
            var yearGroupShortNameUpdated = Utilities.GenerateRandomString(3, "SNADD");
            var yearGroupFullNameUpdated = Utilities.GenerateRandomString(10, "FNADD");
            dataPackage.AddYearGroupLookup(yearGroupIdUpdated, schoolNcYearIdUpdated, yearGroupShortNameUpdated, yearGroupFullNameUpdated, 2);

            //Employee Details Data Injection
            var employeeIdUpdated = Guid.NewGuid();
            dataPackage.AddEmployee(employeeIdUpdated);

            //Staff Details Data Injection
            var staffIdUpdated = Guid.NewGuid();
            var surnameUpdated = Utilities.GenerateRandomString(10, "Surname");
            var forenameUpdated = Utilities.GenerateRandomString(3, "Forename");
            dataPackage.AddStaff(staffIdUpdated, employeeIdUpdated, surnameUpdated, forenameUpdated);

            var serviceRecordIdUpdated = Guid.NewGuid();
            var staffDOAUpdated = DateTime.Now;
            dataPackage.AddServiceRecord(serviceRecordIdUpdated, staffIdUpdated, staffDOAUpdated, null);

            using (new DataSetup(purgeBeforeInsert: false, purgeAfterTest: true, packages: dataPackage))
            {

                //// Scroll to Associated Group
                manageClassesPage.ScrollToAssociatedGroup();
                Wait.WaitForDocumentReady();
                manageClassesPage.AddYearGroup();
                // Year Group table
                var yearGroupTable = manageClassesPage.YearGroupsTable;
                yearGroupTable[0].YearGroup = yearGroupFullName;
                yearGroupTable[0].StartDate = yearGroupStartDate;
                yearGroupTable[0].EndDate = yearGroupEndDate;

                // Scroll to Staff Details
                manageClassesPage.ScrollToStaffDetails();
                manageClassesPage.AddClassTeacher();

                // Add new record for Class Teacher
                var clstaff = string.Concat(surname, ", ", forename);

                // Input data for Class Teacher table
                var classTeacherTable = manageClassesPage.ClassTeacherTable;
                classTeacherTable[0].SelectClassTeacher = clstaff;
                classTeacherTable[0].StartDate = classTeacherStartDate;
                classTeacherTable[0].EndDate = classTeacherEndDate;

                // Input data for Staff table
                manageClassesPage.AddStaff();
                var staffTable = manageClassesPage.StaffTable;
                staffTable[0].SelectStaff = clstaff;
                staffTable[0].PastoralRoleDropDown = pastoraleRole;
                staffTable[0].StartDate = staffStartDate;
                staffTable[0].EndDate = staffEndate;

                // Save data for creating new class
                manageClassesPage.Save();

                #endregion PRE-CONDITIONS

                #region TEST

                // Select Class to update
                manageClassesTriplet.SearchCriteria.SearchByAcademicYear = currentAcademicYear;
                classesResults = manageClassesTriplet.SearchCriteria.Search();
                classesTile = classesResults.SingleOrDefault(t => t.ClassFullName.Equals(classFullName));
                manageClassesPage = classesTile.Click<ManageClassesPage>();          
                
                // Update data for Year group data
                yearGroupTable = manageClassesPage.YearGroupsTable;
                yearGroupTable[0].YearGroup = yearGroupFullNameUpdated;
                yearGroupTable[0].EndDate = yearGroupEndDateUpdate;

                //Update data for Class Teacher table
                var clstaffUpdated = string.Concat(surname, ", ", forename);
                classTeacherTable = manageClassesPage.ClassTeacherTable;
                classTeacherTable[0].SelectClassTeacher = clstaffUpdated;
                classTeacherTable[0].EndDate = classTeacherEndDateUpdate;

                // Update data for Staff table
                staffTable = manageClassesPage.StaffTable;
                staffTable[0].SelectStaff = clstaffUpdated;
                staffTable[0].PastoralRoleDropDown = pastoraleRoleUpdate;
                staffTable[0].EndDate = staffEndateUpdate;
                manageClassesPage.Save();

                //var confirmRequireDialog = manageClassesPage.SaveToDeActive();
                //confirmRequireDialog.ClickOk();
                manageClassesPage.Refresh();

                // Verify that save message update succesfully display
                Assert.AreEqual(true, manageClassesPage.IsSuccessMessageDisplayed(), "Update Class unsuccessfully");

                // Verify that create Class successfully
                manageClassesTriplet.SearchCriteria.SearchByAcademicYear = currentAcademicYear;
                classesResults = manageClassesTriplet.SearchCriteria.Search();
                classesTile = classesResults.SingleOrDefault(t => t.ClassFullName.Equals(classFullName));
                manageClassesPage = classesTile.Click<ManageClassesPage>();

                // Verify that new informations of Class are displayed on screen
                Assert.AreEqual(classFullName, manageClassesPage.ClassFullName, "Class fullname is not equal");
                Assert.AreEqual(classShortName, manageClassesPage.ClassShortName, "Class shortname is not equal");
                Assert.AreEqual(displayOrder, manageClassesPage.DisplayOrder, "Display order is not equal");

                //Verify Active history data
                activeHistoryTable = manageClassesPage.ActiveHistoryTable;
                Assert.AreNotEqual(null,
                    (activeHistoryTable.Rows.SingleOrDefault(x => x.StartDate == activeHistoryStartDate
                                                                  && x.EndDate == activeHistoryEndDate)),
                    "Active history is not equal");
                //Verify Year Group data
                yearGroupTable = manageClassesPage.YearGroupsTable;
                Assert.AreNotEqual(null,
                    (yearGroupTable.Rows.SingleOrDefault(
                        x => x.YearGroup == yearGroupFullNameUpdated && x.StartDate == yearGroupStartDate &&
                             x.EndDate == yearGroupEndDate)), "Year group is not equal");

                //Verify Class Teacher data
                classTeacherTable = manageClassesPage.ClassTeacherTable;
                Assert.AreNotEqual(null,
                    (classTeacherTable.Rows.SingleOrDefault(
                        x => x.SelectClassTeacher == clstaffUpdated && x.StartDate == classTeacherStartDate &&
                             x.EndDate == classTeacherEndDateUpdate)), "Class Teacher is not equal");

                // Verify Staff data
                staffTable = manageClassesPage.StaffTable;
                Assert.AreNotEqual(null,
                    (staffTable.Rows.SingleOrDefault(
                        x => x.SelectStaff == clstaffUpdated && x.PastoralRoleDropDown == pastoraleRoleUpdate &&
                             x.StartDate == staffStartDate && x.EndDate == staffEndateUpdate)),
                    "Class Teacher is not equal");

                // Verify results at Promote Pupils
                AutomationSugar.NavigateMenu("Tasks", "School Groups", "Promote Pupils");
                Wait.WaitForDocumentReady();
                //PromotePupilsTriplet promotePupilTriplet = new PromotePupilsTriplet();
                //var classGroup = promotePupilTriplet.SearchCriteria.Classes;
                //var classItem = classGroup[classFullNameUpdate];
                //Assert.AreNotEqual(null, classItem, "The Class update does not display in Promote Pupil page");

                // Verify new class in Allocate Pupils to Groups
                AutomationSugar.NavigateMenu("Tasks", "School Groups", "Allocate Pupils to Groups");
                Wait.WaitForDocumentReady();
                var allowCatePupilToGroup = new AllocatePupilsToGroupsTriplet();
              var  classGroup = allowCatePupilToGroup.SearchCriteria.Classes;
              var classItem = classGroup[classFullName];
                Assert.AreNotEqual(null, classItem, "The Class update doesn't display in Allocate Pupils to Groups");

                // Verify new class in Allocate Pupils in Allocate Future Pupils
                AutomationSugar.NavigateMenu("Tasks", "School Groups", "Allocate Future Pupils");
                Wait.WaitForDocumentReady();
                //var allocateFuture = new AllocateFuturePupilsTriplet();
                //classGroup = allocateFuture.SearchCriteria.Classes;
                //classItem = classGroup[classFullNameUpdate];
                //Assert.AreNotEqual(null, classItem, "The Class update doesn't displays in Allocate Future Pupils");

                #endregion TEST

                #region POS-CONDITIONS

                //Re-select class page to delete data
                AutomationSugar.NavigateMenu("Tasks", "School Groups", "Manage Classes");

                manageClassesTriplet = new ManageClassesTriplet();
                manageClassesTriplet.SearchCriteria.SearchByAcademicYear = currentAcademicYear;
                classesResults = manageClassesTriplet.SearchCriteria.Search();
                classesTile = classesResults.SingleOrDefault(t => t.ClassFullName.Equals(classFullNameUpdate));
                manageClassesPage = classesTile.Click<ManageClassesPage>();

                // Remove Year Group
                yearGroupTable = manageClassesPage.YearGroupsTable;
                var yearGroupRow = yearGroupTable.Rows.FirstOrDefault(t => t.YearGroup.Equals(yearGroupFullNameUpdated));
                yearGroupRow.DeleteRow();
                yearGroupTable.Refresh();

                //Remove class Teacher
                classTeacherTable = manageClassesPage.ClassTeacherTable;
                var classTeacherRow =
                    classTeacherTable.Rows.FirstOrDefault(t => t.SelectClassTeacher.Equals(clstaffUpdated));
                classTeacherRow.DeleteRow();
                classTeacherTable.Refresh();

                //Remove class Teacher
                staffTable = manageClassesPage.StaffTable;
                var staffRow =
                    staffTable.Rows.FirstOrDefault(
                        t => t.SelectStaff.Equals(clstaffUpdated) && t.PastoralRoleDropDown.Equals(pastoraleRoleUpdate));
                staffRow.DeleteRow();
                staffTable.Refresh();

                manageClassesPage.Save();
                manageClassesPage.Refresh();

                // Delete class
                var confirmDeleteDialog = manageClassesPage.Delete();
                confirmDeleteDialog.ConfirmDelete();

                #endregion POS-CONDITIONS
            }

        }


        /// <summary>
        /// Author: Huy.Vo
        /// Description: Create new Class for the current academic year with an active History Starting and Ending in the current Academic Year
        /// </summary>
        [WebDriverTest(TimeoutSeconds = 2400, Enabled = false, DataProvider = "TC_MC005_Data", Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { "g1" })]
        public void TC_MC005_Add_New_Classes_With_StartDate_EndDate_In_Current_Academic_Year(string currentAcademicYear, string classFullName, string classShortName,
                                             string displayOrder, string activeHistoryStartDate, string activeHistoryEndDate,
                                             string yearGroup, string yearGroupStartDate, string yearGroupEndDate,
                                             string classTearcher, string classTeacherStartDate, string classTeacherEndDate,
                                             string staff, string pastoraleRole, string staffStartDate, string staffEndate)
        {

            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);
            AutomationSugar.NavigateMenu("Tasks", "School Groups", "Manage Classes");

            #region TEST

            //  Search and delete if Class is existing
            var manageClassesTriplet = new ManageClassesTriplet();
            manageClassesTriplet.SearchCriteria.SearchByAcademicYear = currentAcademicYear;
            var classesResults = manageClassesTriplet.SearchCriteria.Search();
            var classesTile = classesResults.SingleOrDefault(t => t.ClassFullName.Equals(classFullName));
            manageClassesTriplet.SelectSearchTile(classesTile);
            manageClassesTriplet.Delete();

            // Create new Classe with Year Group
            var manageClassesPage = manageClassesTriplet.Create();
            manageClassesPage.ClassFullName = classFullName;
            manageClassesPage.ClassShortName = classShortName;
            manageClassesPage.DisplayOrder = displayOrder;

            //Active History table
            var activeHistoryTable = manageClassesPage.ActiveHistoryTable;
            activeHistoryTable[0].StartDate = activeHistoryStartDate;
            activeHistoryTable[0].EndDate = activeHistoryEndDate;
            manageClassesPage.Save();
            manageClassesPage.Refresh();

            // Scroll to Associated Group
            manageClassesPage.ScrollToAssociatedGroup();

            // Year Group table
            var yearGroupTable = manageClassesPage.YearGroupsTable;
            yearGroupTable[0].YearGroup = yearGroup;
            yearGroupTable[0].StartDate = yearGroupStartDate;
            yearGroupTable[0].EndDate = yearGroupEndDate;

            // Scoll to Staff Details
            manageClassesPage.ScrollToStaffDetails();

            // Input data for Class Teacher table
            var classTeacherTable = manageClassesPage.ClassTeacherTable;
            classTeacherTable[0].SelectClassTeacher = classTearcher;
            classTeacherTable[0].StartDate = classTeacherStartDate;
            classTeacherTable[0].EndDate = classTeacherEndDate;

            // Input data for Staff table
            var staffTable = manageClassesPage.StaffTable;
            staffTable[0].SelectStaff = staff;
            staffTable[0].PastoralRoleDropDown = pastoraleRole;
            staffTable[0].StartDate = staffStartDate;
            staffTable[0].EndDate = staffEndate;

            // Save data
            manageClassesPage.Save();

            // Verify that save message succesfully display
            Assert.AreEqual(true, manageClassesPage.IsSuccessMessageDisplayed(), "Create new Class unsuccessfully in the current Academic year");

            // Verify that create Class successfully
            manageClassesTriplet.SearchCriteria.SearchByAcademicYear = currentAcademicYear;
            classesResults = manageClassesTriplet.SearchCriteria.Search();
            classesTile = classesResults.SingleOrDefault(t => t.ClassFullName.Equals(classFullName));
            manageClassesPage = classesTile.Click<ManageClassesPage>();

            // Verify that new informations of Class are displayed on screen
            Assert.AreEqual(classFullName, manageClassesPage.ClassFullName, "Class fullname is not equal");
            Assert.AreEqual(classShortName, manageClassesPage.ClassShortName, "Class shortname is not equal");
            Assert.AreEqual(displayOrder, manageClassesPage.DisplayOrder, "Display order is not equal");

            //Verify Active history data
            activeHistoryTable = manageClassesPage.ActiveHistoryTable;
            Assert.AreNotEqual(null, (activeHistoryTable.Rows.SingleOrDefault(x => x.StartDate == activeHistoryStartDate)), "Active history is not equal");

            //Verify Year Group data
            yearGroupTable = manageClassesPage.YearGroupsTable;
            Assert.AreNotEqual(null, (yearGroupTable.Rows.SingleOrDefault(x => x.YearGroup == yearGroup && x.StartDate == yearGroupStartDate &&
                x.EndDate == yearGroupEndDate)), "Year group is not equal");

            //Verify Class Teacher data
            classTeacherTable = manageClassesPage.ClassTeacherTable;
            Assert.AreNotEqual(null, (classTeacherTable.Rows.SingleOrDefault(x => x.SelectClassTeacher == classTearcher && x.StartDate == classTeacherStartDate &&
                x.EndDate == classTeacherEndDate)), "Class Teacher is not equal");

            // Verify Staff data
            staffTable = manageClassesPage.StaffTable;
            Assert.AreNotEqual(null, (staffTable.Rows.SingleOrDefault(x => x.SelectStaff == staff && x.PastoralRoleDropDown == pastoraleRole &&
                x.StartDate == staffStartDate && x.EndDate == staffEndate)), "Class Teacher is not equal");

            // Verify results at Promote Pupils
            AutomationSugar.NavigateMenu("Tasks", "School Groups", "Promote Pupils");
            PromotePupilsTriplet promotePupilTriplet = new PromotePupilsTriplet();
            var classGroup = promotePupilTriplet.SearchCriteria.Classes;
            var classItem = classGroup[classFullName];

            //Confirm By question #15
            Assert.AreEqual(null, classItem, "The Class does not display in Promote Pupil page");

            // Verify new class in Allocate Pupils to Groups
            AutomationSugar.NavigateMenu("Tasks", "School Groups", "Allocate Pupils To Groups");
            var allowCatePupilToGroup = new AllocatePupilsToGroupsTriplet();
            classGroup = allowCatePupilToGroup.SearchCriteria.Classes;
            classItem = classGroup[classFullName];

            Assert.AreEqual(null, classItem, "The Class doesn't display in Allocate Pupils to Groups");

            // Verify new class in Allocate Pupils in Allocate Future Pupils
            AutomationSugar.NavigateMenu("Tasks", "School Groups", "Allocate Future Pupils");
            var allocateFuture = new AllocateFuturePupilsTriplet();
            classGroup = allocateFuture.SearchCriteria.Classes;
            classItem = classGroup[classFullName];
            Assert.AreEqual(null, classItem, "The Class doesn't displays in Allocate Future Pupils");

            #endregion

            #region POS-CONDITIONS

            //Re-select class page to delete data
            AutomationSugar.NavigateMenu("Tasks", "School Groups", "Manage Classes");

            manageClassesTriplet = new ManageClassesTriplet();
            manageClassesTriplet.SearchCriteria.SearchByAcademicYear = currentAcademicYear;
            classesResults = manageClassesTriplet.SearchCriteria.Search();
            classesTile = classesResults.SingleOrDefault(t => t.ClassFullName.Equals(classFullName));
            manageClassesPage = classesTile.Click<ManageClassesPage>();

            //Remove Active History
            activeHistoryTable = manageClassesPage.ActiveHistoryTable;
            var activeHistoryRow = activeHistoryTable.Rows.FirstOrDefault(t => t.StartDate.Equals(activeHistoryStartDate));
            activeHistoryRow.DeleteRow();
            activeHistoryTable.Refresh();

            // Remove Year Group
            yearGroupTable = manageClassesPage.YearGroupsTable;
            var yearGroupRow = yearGroupTable.Rows.FirstOrDefault(t => t.YearGroup.Equals(yearGroup));
            yearGroupRow.DeleteRow();
            yearGroupTable.Refresh();

            //Remove class Teacher
            classTeacherTable = manageClassesPage.ClassTeacherTable;
            var classTeacherRow = classTeacherTable.Rows.FirstOrDefault(t => t.SelectClassTeacher.Equals(classTearcher));
            classTeacherRow.DeleteRow();
            classTeacherTable.Refresh();

            //Remove class Teacher
            staffTable = manageClassesPage.StaffTable;
            var staffRow = staffTable.Rows.FirstOrDefault(t => t.SelectStaff.Equals(staff) && t.PastoralRoleDropDown.Equals(pastoraleRole));
            staffRow.DeleteRow();
            staffTable.Refresh();

            manageClassesPage.Save();
            manageClassesPage.Refresh();

            // Delete Class
            var confirmDeleteDialog = manageClassesPage.Delete();
            confirmDeleteDialog.ConfirmDelete();
            #endregion POS-CONDITIONS

        }

        /// <summary>
        /// Author: Ba.Truong
        /// Description: Amend a Class for the Current Academic Year with a Active History ending in the current academic Year
        /// </summary>
        [WebDriverTest(TimeoutSeconds = 2400, Enabled = false, DataProvider = "TC_MC006_Data", Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { "g1" })]
        public void TC_MC006_Udate_Classes_Active_History_Ending_In_Current_Academic_Year(string[] classDetails, string[] activeHistories, string[] updateClassDetails,
                                                                                          string[] updateActiveHistories, string[] updateYearGroups,
                                                                                          string[] updateClassTeachers, string[] updateStaffs, string currentAcademicYear)
        {
            #region Pre-condition: Create a class for the Current Academic Year with a Active History ending in the current academic Year

            //Login and navigate to 'Manage Classes' page
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);
            AutomationSugar.NavigateMenu("Tasks", "School Groups", "Manage Classes");

            //Create a new class
            var manageClassTriplet = new ManageClassesTriplet();
            var manageClassPage = manageClassTriplet.Create();

            //Input class details
            manageClassPage.ClassFullName = classDetails[0];
            manageClassPage.ClassShortName = classDetails[1];
            manageClassPage.DisplayOrder = classDetails[2];

            //Input information for Active History grid to set this class for current year
            manageClassPage.ActiveHistoryTable[0].StartDate = activeHistories[0];
            manageClassPage.ActiveHistoryTable[0].EndDate = activeHistories[1];

            //Save the class
            manageClassPage.Save();

            #endregion

            #region Steps

            //Select Class created to amend
            manageClassTriplet.Refresh();
            manageClassTriplet.SearchCriteria.SearchByAcademicYear = currentAcademicYear;
            var classesResults = manageClassTriplet.SearchCriteria.Search();
            var classesTile = classesResults.SingleOrDefault(t => t.ClassFullName.Equals(classDetails[0]));
            manageClassPage = classesTile.Click<ManageClassesPage>();

            //Update class details
            manageClassPage.ClassFullName = updateClassDetails[0];
            manageClassPage.ClassShortName = updateClassDetails[1];
            manageClassPage.DisplayOrder = updateClassDetails[2];

            // Update data for Active History table
            manageClassPage.ActiveHistoryTable[0].StartDate = updateActiveHistories[0];
            manageClassPage.ActiveHistoryTable[0].EndDate = updateActiveHistories[1];

            //Scroll to Associated Groups
            manageClassPage.ScrollToAssociatedGroup();

            // Update data for Year group data
            manageClassPage.YearGroupsTable[0].YearGroup = updateYearGroups[0];
            manageClassPage.YearGroupsTable[0].StartDate = updateYearGroups[1];
            manageClassPage.YearGroupsTable[0].EndDate = updateYearGroups[2];

            //Scroll to Staff Details
            manageClassPage.ScrollToStaffDetails();

            //Update data for Class Teacher table
            manageClassPage.ClassTeacherTable[0].SelectClassTeacher = updateClassTeachers[0];
            manageClassPage.ClassTeacherTable[0].StartDate = updateClassTeachers[1];
            manageClassPage.ClassTeacherTable[0].EndDate = updateClassTeachers[2];

            // Update data for Staff table
            manageClassPage.StaffTable[0].SelectStaff = updateStaffs[0];
            manageClassPage.StaffTable[0].PastoralRoleDropDown = updateStaffs[1];
            manageClassPage.StaffTable[0].StartDate = updateStaffs[2];
            manageClassPage.StaffTable[0].EndDate = updateStaffs[3];

            manageClassPage.Save();

            //Confirmation the changes
            manageClassTriplet = ConfirmRequiredDialog.Confirm<ManageClassesTriplet>();

            //Verify the success message displays
            manageClassPage.Refresh();
            Assert.AreEqual(true, manageClassPage.IsSuccessMessageDisplayed(), "Success message doesn't display");

            //Verify that create Class successfully
            manageClassTriplet.Refresh();
            manageClassTriplet.SearchCriteria.SearchByAcademicYear = currentAcademicYear;
            classesResults = manageClassTriplet.SearchCriteria.Search();
            classesTile = classesResults.SingleOrDefault(t => t.ClassFullName.Equals(updateClassDetails[0]));
            manageClassPage = classesTile.Click<ManageClassesPage>();

            //Confirm the class details were updated successfully
            Assert.AreEqual(updateClassDetails[0], manageClassPage.ClassFullName, "Fullname is not updated");
            Assert.AreEqual(updateClassDetails[1], manageClassPage.ClassShortName, "Shortname is not updated");
            Assert.AreEqual(updateClassDetails[2], manageClassPage.DisplayOrder, "Display order is not updated");

            //Confirm 'Active History' information was updated successfully 
            Assert.AreEqual(updateActiveHistories[0], manageClassPage.ActiveHistoryTable[0].StartDate, "'Start Date' in Active history grid is not updated");
            Assert.AreEqual(updateActiveHistories[1], manageClassPage.ActiveHistoryTable[0].EndDate, "'End Date' Active history grid is not updated");

            //Scroll to Associated Groups
            manageClassPage.ScrollToAssociatedGroup();

            //Confirm 'Year Group' information was updated successfully 
            Assert.AreEqual(updateYearGroups[0], manageClassPage.YearGroupsTable[0].YearGroup, "'Year Group' in Year group grid is not updated");
            Assert.AreEqual(updateYearGroups[1], manageClassPage.YearGroupsTable[0].StartDate, "'Start Date' in Year group grid is not updated");
            Assert.AreEqual(updateYearGroups[2], manageClassPage.YearGroupsTable[0].EndDate, "'End Date' in Year group grid is not updated");

            //Scroll to Staff Details
            manageClassPage.ScrollToStaffDetails();

            //Confirm 'Class Teacher' information was updated successfully 
            Assert.AreEqual(updateClassTeachers[0], manageClassPage.ClassTeacherTable[0].SelectClassTeacher, "'Class Teacher' in Class Teacher grid is not updated");
            Assert.AreEqual(updateClassTeachers[1], manageClassPage.ClassTeacherTable[0].StartDate, "'Start Date' in Class Teacher grid is not updated");
            Assert.AreEqual(updateClassTeachers[2], manageClassPage.ClassTeacherTable[0].EndDate, "'End Date' in Class Teacher grid is not updated");

            //Confirm 'Staff' information was updated successfully 
            Assert.AreEqual(updateStaffs[0], manageClassPage.StaffTable[0].SelectStaff, "'Staff' in Staff grid is not updated");
            Assert.AreEqual(updateStaffs[1], manageClassPage.StaffTable[0].PastoralRoleDropDown, "'Pastoral Role' in Staff grid is not updated");
            Assert.AreEqual(updateStaffs[2], manageClassPage.StaffTable[0].StartDate, "'Start Date' in Staff grid is not updated");

            //Confirm the changes of year group reflected on the Search Options on Screens 'Promote Pupils'
            AutomationSugar.NavigateMenu("Tasks", "School Groups", "Promote Pupils");
            PromotePupilsTriplet promotePupilTriplet = new PromotePupilsTriplet();
            var classes = promotePupilTriplet.SearchCriteria.Classes;
            Assert.AreNotEqual(null, classes[updateClassDetails[0]], "The class does not display in Promote Pupil page");

            //Confirm the changes of year group reflected on the Search Options on Screens 'Allocate New Intake'
            AutomationSugar.NavigateMenu("Tasks", "School Groups", "Allocate Future Pupils");
            var newIntake = new AllocateFuturePupilsTriplet();
            classes = newIntake.SearchCriteria.Classes;
            Assert.AreNotEqual(null, classes[updateClassDetails[0]], "The class does not display in Allocate New Intake page");

            //Confirm the changes of year group reflected on the Search Options on Screens 'Allocate Pupils to Groups'
            AutomationSugar.NavigateMenu("Tasks", "School Groups", "Allocate Pupils To Groups");
            var allocatePupilToGroup = new AllocatePupilsToGroupsTriplet();
            classes = allocatePupilToGroup.SearchCriteria.Classes;
            Assert.AreNotEqual(null, classes[updateClassDetails[0]], "The class does not display in Allocate Pupils to Groups page");

            #endregion

            #region Pos-condition

            //Back to Year group details page to delete year group was added
            AutomationSugar.NavigateMenu("Tasks", "School Groups", "Manage Classes");
            manageClassTriplet = new ManageClassesTriplet();
            manageClassTriplet.SearchCriteria.SearchByAcademicYear = currentAcademicYear;
            classesResults = manageClassTriplet.SearchCriteria.Search();
            classesTile = classesResults.SingleOrDefault(t => t.ClassFullName.Equals(updateClassDetails[0]));
            manageClassPage = classesTile.Click<ManageClassesPage>();

            //Remove the records in Associated Groups section
            manageClassPage.ScrollToAssociatedGroup();
            manageClassPage.YearGroupsTable[0].DeleteRow();

            //Remove the records in Staff Details section
            manageClassPage.ScrollToStaffDetails();
            manageClassPage.StaffTable[0].DeleteRow();
            manageClassPage.ClassTeacherTable[0].DeleteRow();

            //Save the changes
            manageClassPage.Save();

            //Delete the class was added
            manageClassPage.Delete();

            #endregion POS-CONDITIONS

        }

        /// <summary>
        /// Author: Huy.Vo
        /// Description: Remove a class from a Pastoral structure for a future academic year by entering an End Date into Associated Group record.
        /// </summary>
        [WebDriverTest(TimeoutSeconds = 2400, Enabled = false, DataProvider = "TC_MC009_Data", Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { "g1" })]
        public void TC_MC009_Remove_Classes_From_Pastoral_Structure_For_A_Future_Academic(string futureAcademicYear, string classFullName, string classShortName,
                                             string displayOrder, string activeHistoryStartDate, string activeHistoryEndDate,
                                             string yearGroup, string yearGroupStartDate, string yearGroupEndDate,
                                             string classTearcher, string classTeacherStartDate, string classTeacherEndDate,
                                             string staff, string pastoraleRole, string staffStartDate, string staffEndDate,
                                             string endDateToRemove, string currentAcademic)
        {

            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);
            AutomationSugar.NavigateMenu("Tasks", "School Groups", "Manage Classes");

            #region PRE-CONDITIONS

            //  Search and delete if Class is existing
            var manageClassesTriplet = new ManageClassesTriplet();
            manageClassesTriplet.SearchCriteria.SearchByAcademicYear = futureAcademicYear;
            var classesResults = manageClassesTriplet.SearchCriteria.Search();
            var classesTile = classesResults.SingleOrDefault(t => t.ClassFullName.Equals(classFullName));
            manageClassesTriplet.SelectSearchTile(classesTile);
            manageClassesTriplet.Delete();

            // Create new Classe with Year Group
            var manageClassesPage = manageClassesTriplet.Create();
            manageClassesPage.ClassFullName = classFullName;
            manageClassesPage.ClassShortName = classShortName;
            manageClassesPage.DisplayOrder = displayOrder;

            //Active History table
            var activeHistoryTable = manageClassesPage.ActiveHistoryTable;
            activeHistoryTable[0].StartDate = activeHistoryStartDate;
            activeHistoryTable[0].EndDate = activeHistoryEndDate;
            manageClassesPage.Save();
            manageClassesPage.Refresh();

            // Scroll to Associated Group
            manageClassesPage.ScrollToAssociatedGroup();

            // Year Group table
            var yearGroupTable = manageClassesPage.YearGroupsTable;
            yearGroupTable[0].YearGroup = yearGroup;
            yearGroupTable[0].StartDate = yearGroupStartDate;
            yearGroupTable[0].EndDate = yearGroupEndDate;

            // Scoll to Staff Details
            manageClassesPage.ScrollToStaffDetails();

            // Input data for Class Teacher table
            var classTeacherTable = manageClassesPage.ClassTeacherTable;
            classTeacherTable[0].SelectClassTeacher = classTearcher;
            classTeacherTable[0].StartDate = classTeacherStartDate;
            classTeacherTable[0].EndDate = classTeacherEndDate;

            // Input data for Staff table
            var staffTable = manageClassesPage.StaffTable;
            staffTable[0].SelectStaff = staff;
            staffTable[0].PastoralRoleDropDown = pastoraleRole;
            staffTable[0].StartDate = staffStartDate;
            staffTable[0].EndDate = staffEndDate;

            // Save data
            manageClassesPage.Save();

            #endregion PRE-CONDITIONS

            #region TEST
            // Select Class to update end date for record in Associated Group
            manageClassesTriplet.SearchCriteria.SearchByAcademicYear = futureAcademicYear;
            classesResults = manageClassesTriplet.SearchCriteria.Search();
            classesTile = classesResults.SingleOrDefault(t => t.ClassFullName.Equals(classFullName));
            manageClassesPage = classesTile.Click<ManageClassesPage>();

            // Update data for Year group data
            activeHistoryTable = manageClassesPage.ActiveHistoryTable;
            activeHistoryTable[0].EndDate = endDateToRemove;

            // Save data
            var dialogConfirm = manageClassesPage.SaveToDeActive();
            dialogConfirm.ClickOk();
            manageClassesPage.Refresh();

            // Verify that save message update succesfully display
            Assert.AreEqual(true, manageClassesPage.IsSuccessMessageDisplayed(), "Create new class unsuccessfully");

            // Verify that class in future Academic year is displayed current Academic year
            manageClassesTriplet.SearchCriteria.SearchByAcademicYear = futureAcademicYear;
            classesResults = manageClassesTriplet.SearchCriteria.Search();
            classesTile = classesResults.SingleOrDefault(t => t.ClassFullName.Equals(classFullName));
            Assert.AreEqual(null, classesTile, "Class is displayed in Future Academic Year");

            // Verify class is displayed in current Academic Year
            manageClassesTriplet.SearchCriteria.SearchByAcademicYear = currentAcademic;
            classesResults = manageClassesTriplet.SearchCriteria.Search();
            classesTile = classesResults.SingleOrDefault(t => t.ClassFullName.Equals(classFullName));
            Assert.AreNotEqual(null, classesTile, "Class is not displayed in Current Academic Year");

            // Verify Class is not displayed in Allocate Future Pupils
            AutomationSugar.NavigateMenu("Tasks", "School Groups", "Allocate Future Pupils");
            var allocateFuture = new AllocateFuturePupilsTriplet();
            var classGroup = allocateFuture.SearchCriteria.Classes;
            var classItem = classGroup[classFullName];
            Assert.AreNotEqual(null, classItem, "The Class doesn't displays in Allocate Future Pupils");

            // Verify that class is displayed in Allocate Pupils To Groups
            AutomationSugar.NavigateMenu("Tasks", "School Groups", "Allocate Pupils To Groups");
            var allowCatePupilToGroup = new AllocatePupilsToGroupsTriplet();
            classGroup = allowCatePupilToGroup.SearchCriteria.Classes;
            classItem = classGroup[classFullName];
            Assert.AreEqual(null, classItem, "The Class displays in Allocate Pupils to Groups");

            // Verify class in the future Academic is not displayed in Promote Pupils
            AutomationSugar.NavigateMenu("Tasks", "School Groups", "Promote Pupils");
            var promotePupilTriplet = new PromotePupilsTriplet();
            classGroup = promotePupilTriplet.SearchCriteria.Classes;
            classItem = classGroup[classFullName];
            Assert.AreEqual(null, classItem, "The Class doesn't displays in Promote Pupil page");

            #endregion TEST

            #region POS-CONDITIONS

            //Re-select class page to delete data
            AutomationSugar.NavigateMenu("Tasks", "School Groups", "Manage Classes");
            manageClassesTriplet = new ManageClassesTriplet();
            manageClassesTriplet.SearchCriteria.SearchByAcademicYear = currentAcademic;
            classesResults = manageClassesTriplet.SearchCriteria.Search();
            classesTile = classesResults.SingleOrDefault(t => t.ClassFullName.Equals(classFullName));
            manageClassesPage = classesTile.Click<ManageClassesPage>();

            //Remove record in Active History table
            activeHistoryTable = manageClassesPage.ActiveHistoryTable;
            var activeHistoryRow = activeHistoryTable.Rows.FirstOrDefault(t => t.StartDate.Equals(activeHistoryStartDate));
            activeHistoryRow.DeleteRow();
            activeHistoryTable.Refresh();

            // Remove record in Year Group table
            yearGroupTable = manageClassesPage.YearGroupsTable;
            var yearGroupRow = yearGroupTable.Rows.FirstOrDefault(t => t.YearGroup.Equals(yearGroup));
            yearGroupRow.DeleteRow();
            yearGroupTable.Refresh();

            //Remove record in Class Teacher table
            classTeacherTable = manageClassesPage.ClassTeacherTable;
            var classTeacherRow = classTeacherTable.Rows.FirstOrDefault(t => t.SelectClassTeacher.Equals(classTearcher));
            classTeacherRow.DeleteRow();
            classTeacherTable.Refresh();

            //Remove record in Staff Table
            staffTable = manageClassesPage.StaffTable;
            var staffRow = staffTable.Rows.FirstOrDefault(t => t.SelectStaff.Equals(staff) && t.PastoralRoleDropDown.Equals(pastoraleRole));
            staffRow.DeleteRow();
            staffTable.Refresh();

            //Save after updating data
            manageClassesPage.Save();
            manageClassesPage.Refresh();

            // Delete Class
            var confirmDeleteDialog = manageClassesPage.Delete();
            confirmDeleteDialog.ConfirmDelete();

            #endregion POS-CONDITIONS

        }

        /// <summary>
        /// Author: Huy.Vo
        /// Description:Remove a class from a Pastoral structure for a future academic year by deleting Association Group record
        /// </summary>

        [WebDriverTest(TimeoutSeconds = 2400, Enabled = false, DataProvider = "TC_MC010_Data", Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { "g1" })]
        public void TC_MC010_Remove_Classes_By_Deleting_Association_Group_Record(string futureAcademicYear, string classFullName, string classShortName,
                                             string displayOrder, string activeHistoryStartDate, string activeHistoryEndDate,
                                             string yearGroup, string yearGroupStartDate, string yearGroupEndDate,
                                             string classTearcher, string classTeacherStartDate, string classTeacherEndDate,
                                             string staff, string pastoraleRole, string staffStartDate, string staffEndDate, string endDateToRemove, string currentAcademic)
        {
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);
            AutomationSugar.NavigateMenu("Tasks", "School Groups", "Manage Classes");

            #region PRE-CONDITIONS

            //  Search and delete if Class is existing
            var manageClassesTriplet = new ManageClassesTriplet();
            manageClassesTriplet.SearchCriteria.SearchByAcademicYear = futureAcademicYear;
            var classesResults = manageClassesTriplet.SearchCriteria.Search();
            var classesTile = classesResults.SingleOrDefault(t => t.ClassFullName.Equals(classFullName));
            manageClassesTriplet.SelectSearchTile(classesTile);
            manageClassesTriplet.Delete();

            // Create new Classe with Year Group
            var manageClassesPage = manageClassesTriplet.Create();
            manageClassesPage.ClassFullName = classFullName;
            manageClassesPage.ClassShortName = classShortName;
            manageClassesPage.DisplayOrder = displayOrder;

            //Active History table
            var activeHistoryTable = manageClassesPage.ActiveHistoryTable;
            activeHistoryTable[0].StartDate = activeHistoryStartDate;
            activeHistoryTable[0].EndDate = activeHistoryEndDate;
            manageClassesPage.Save();
            manageClassesPage.Refresh();

            // Scroll to Associated Group
            manageClassesPage.ScrollToAssociatedGroup();

            // Year Group table
            var yearGroupTable = manageClassesPage.YearGroupsTable;
            yearGroupTable[0].YearGroup = yearGroup;
            yearGroupTable[0].StartDate = yearGroupStartDate;
            yearGroupTable[0].EndDate = yearGroupEndDate;

            // Scoll to Staff Details
            manageClassesPage.ScrollToStaffDetails();

            // Input data for Class Teacher table
            var classTeacherTable = manageClassesPage.ClassTeacherTable;
            classTeacherTable[0].SelectClassTeacher = classTearcher;
            classTeacherTable[0].StartDate = classTeacherStartDate;
            classTeacherTable[0].EndDate = classTeacherEndDate;

            // Input data for Staff table
            var staffTable = manageClassesPage.StaffTable;
            staffTable[0].SelectStaff = staff;
            staffTable[0].PastoralRoleDropDown = pastoraleRole;
            staffTable[0].StartDate = staffStartDate;
            staffTable[0].EndDate = staffEndDate;

            // Save data
            manageClassesPage.Save();

            #endregion PRE-CONDITIONS

            #region TEST
            // Select Class to remove
            manageClassesTriplet.SearchCriteria.SearchByAcademicYear = futureAcademicYear;
            classesResults = manageClassesTriplet.SearchCriteria.Search();
            classesTile = classesResults.SingleOrDefault(t => t.ClassFullName.Equals(classFullName));
            manageClassesPage = classesTile.Click<ManageClassesPage>();

            // Scroll to Associated Group
            manageClassesPage.ScrollToAssociatedGroup();

            // Remove Year Group
            yearGroupTable = manageClassesPage.YearGroupsTable;
            var yearGroupRow = yearGroupTable.Rows.FirstOrDefault(t => t.YearGroup.Equals(yearGroup));
            yearGroupRow.DeleteRow();
            yearGroupTable.Refresh();

            manageClassesPage.Save();
            manageClassesPage.Refresh();

            // Verify that save message update succesfully display
            Assert.AreEqual(true, manageClassesPage.IsSuccessMessageDisplayed(), "Create new class unsuccessfully");

            // Verify that Class should no longer appear in Year  group  for any academic year
            AutomationSugar.NavigateMenu("Tasks", "School Groups", "Manage Year Groups");

            //Select Year Group that has class with Future Academic Year
            var manageYearGroupTriplet = new ManageYearGroupsTriplet();
            manageYearGroupTriplet.SearchCriteria.AcademicYear = futureAcademicYear;

            var yearGroupTile = manageYearGroupTriplet.SearchCriteria.Search().FirstOrDefault(x => x.FullName.Equals(yearGroup));
            var manageYearGroupDetail = yearGroupTile.Click<ManageYearGroupsPage>();

            //Scroll to Associated Groups
            manageYearGroupDetail.ScrollToAssociatedGroup();

            var classTable = manageYearGroupDetail.Classes;
            Assert.AreEqual(null, (classTable.Rows.SingleOrDefault(x => x.Class == classFullName && x.StartDate == yearGroupStartDate &&
               x.EndDate == yearGroupEndDate)), "Class is exist Year Group in Future Academic Year");

            // Verify Class is not displayed in Allocate Future Pupils
            AutomationSugar.NavigateMenu("Tasks", "School Groups", "Allocate Future Pupils");
            var allocateFuture = new AllocateFuturePupilsTriplet();
            var classGroup = allocateFuture.SearchCriteria.Classes;
            var classItem = classGroup[classFullName];
            Assert.AreNotEqual(null, classItem, "The Class is not displayed in Allocate Future Pupils");

            // Verify that class is displayed in Allocate Pupils To Groups
            AutomationSugar.NavigateMenu("Tasks", "School Groups", "Allocate Pupils To Groups");
            var allowCatePupilToGroup = new AllocatePupilsToGroupsTriplet();
            classGroup = allowCatePupilToGroup.SearchCriteria.Classes;
            classItem = classGroup[classFullName];
            Assert.AreEqual(null, classItem, "The Class displays in Allocate Pupils to Groups");

            // Verify class in the future Academic is not displayed in Promote Pupils
            AutomationSugar.NavigateMenu("Tasks", "School Groups", "Promote Pupils");
            var promotePupilTriplet = new PromotePupilsTriplet();
            classGroup = promotePupilTriplet.SearchCriteria.Classes;
            classItem = classGroup[classFullName];
            Assert.AreNotEqual(null, classItem, "The Class is not displayed in Promote Pupil page");

            #endregion TEST

            #region POS-CONDITIONS

            //Re-select class page to delete data
            AutomationSugar.NavigateMenu("Tasks", "School Groups", "Manage Classes");
            manageClassesTriplet = new ManageClassesTriplet();
            manageClassesTriplet.SearchCriteria.SearchByAcademicYear = currentAcademic;
            classesResults = manageClassesTriplet.SearchCriteria.Search();
            classesTile = classesResults.SingleOrDefault(t => t.ClassFullName.Equals(classFullName));
            manageClassesPage = classesTile.Click<ManageClassesPage>();

            //Remove Active History
            activeHistoryTable = manageClassesPage.ActiveHistoryTable;
            var activeHistoryRow = activeHistoryTable.Rows.FirstOrDefault(t => t.StartDate.Equals(activeHistoryStartDate));
            activeHistoryRow.DeleteRow();
            activeHistoryTable.Refresh();

            //Remove class Teacher
            classTeacherTable = manageClassesPage.ClassTeacherTable;
            var classTeacherRow = classTeacherTable.Rows.FirstOrDefault(t => t.SelectClassTeacher.Equals(classTearcher));
            classTeacherRow.DeleteRow();
            classTeacherTable.Refresh();

            //Remove class Teacher
            staffTable = manageClassesPage.StaffTable;
            var staffRow = staffTable.Rows.FirstOrDefault(t => t.SelectStaff.Equals(staff) && t.PastoralRoleDropDown.Equals(pastoraleRole));
            staffRow.DeleteRow();
            staffTable.Refresh();
            manageClassesPage.Save();
            manageClassesPage.Refresh();

            // Delete Class
            var confirmDeleteDialog = manageClassesPage.Delete();
            confirmDeleteDialog.ConfirmDelete();
            #endregion POS-CONDITIONS

        }

        /// <summary>
        /// Author: Huy.Vo
        /// Description: Remove a class from a Pastoral structure for a future academic year by entering an End Date into Associated Group record.
        /// </summary>
        [WebDriverTest(TimeoutSeconds = 2400, Enabled = false, DataProvider = "TC_MC011_Data", Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { "g1" })]
        public void TC_MC011_Remove_Classes_For_Future_Academic_Year_From_Entire_Pastoral_Structure_By_Entering_End_Date(string futureAcademicYear, string classFullName, string classShortName,
                                             string displayOrder, string activeHistoryStartDate, string activeHistoryEndDate, string activeHistoryUpdateEndDate, string currentAcademic)
        {
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);
            AutomationSugar.NavigateMenu("Tasks", "School Groups", "Manage Classes");

            #region STEPS PRE-CONDITIONS

            //  Search and delete if Class is existing
            var manageClassesTriplet = new ManageClassesTriplet();
            manageClassesTriplet.SearchCriteria.SearchByAcademicYear = futureAcademicYear;
            var classesResults = manageClassesTriplet.SearchCriteria.Search();
            var classesTile = classesResults.SingleOrDefault(t => t.ClassFullName.Equals(classFullName));
            manageClassesTriplet.SelectSearchTile(classesTile);
            manageClassesTriplet.Delete();

            // Create new Classe with Year Group
            var manageClassesPage = manageClassesTriplet.Create();
            manageClassesPage.ClassFullName = classFullName;
            manageClassesPage.ClassShortName = classShortName;
            manageClassesPage.DisplayOrder = displayOrder;

            //Active History table
            var activeHistoryTable = manageClassesPage.ActiveHistoryTable;
            activeHistoryTable[0].StartDate = activeHistoryStartDate;
            activeHistoryTable[0].EndDate = activeHistoryEndDate;
            manageClassesPage.Save();
            manageClassesPage.Refresh();


            #endregion PRE-CONDITIONS

            #region TEST
            // Select Class to remove
            manageClassesTriplet.SearchCriteria.SearchByAcademicYear = futureAcademicYear;
            classesResults = manageClassesTriplet.SearchCriteria.Search();
            classesTile = classesResults.SingleOrDefault(t => t.ClassFullName.Equals(classFullName));
            manageClassesPage = classesTile.Click<ManageClassesPage>();

            // Update end date to remove class out of Academic year
            activeHistoryTable = manageClassesPage.ActiveHistoryTable;
            activeHistoryTable[0].EndDate = activeHistoryUpdateEndDate;

            var dialogConfirm = manageClassesPage.SaveToDeActive();
            dialogConfirm.ClickOk();
            manageClassesPage.Refresh();

            // Verify that save message update succesfully display
            Assert.AreEqual(true, manageClassesPage.IsSuccessMessageDisplayed(), "Update class unsuccessfully");

            // Verify that remove Class successfully
            manageClassesTriplet.SearchCriteria.SearchByAcademicYear = futureAcademicYear;
            classesResults = manageClassesTriplet.SearchCriteria.Search();
            classesTile = classesResults.SingleOrDefault(t => t.ClassFullName.Equals(classFullName));

            // Verify that remove class by entering Year Group endate successfully
            Assert.AreEqual(null, classesTile, "Class is not remove to Current Academic Year");

            // Re-search to verify that new Class is displayed in current Academic year
            manageClassesTriplet.SearchCriteria.SearchByAcademicYear = currentAcademic;
            classesResults = manageClassesTriplet.SearchCriteria.Search();
            classesTile = classesResults.SingleOrDefault(t => t.ClassFullName.Equals(classFullName));
            Assert.AreNotEqual(null, classesTile, "Class is not remove to Current Academic Year");

            // Verify Class is not displayed in Allocate Future Pupils
            AutomationSugar.NavigateMenu("Tasks", "School Groups", "Allocate Future Pupils");
            var allocateFuture = new AllocateFuturePupilsTriplet();
            var classGroup = allocateFuture.SearchCriteria.Classes;
            var classItem = classGroup[classFullName];
            Assert.AreNotEqual(null, classItem, "The Class doesn't display in Allocate Future Pupils");

            // Verify that class is displayed in Allocate Pupils To Groups
            AutomationSugar.NavigateMenu("Tasks", "School Groups", "Allocate Pupils To Groups");
            var allowCatePupilToGroup = new AllocatePupilsToGroupsTriplet();
            classGroup = allowCatePupilToGroup.SearchCriteria.Classes;
            classItem = classGroup[classFullName];
            Assert.AreEqual(null, classItem, "The Class displayes in Allocate Pupils to Groups");

            // Verify class in the future Academic is not displayed in Promote Pupils
            AutomationSugar.NavigateMenu("Tasks", "School Groups", "Promote Pupils");
            var promotePupilTriplet = new PromotePupilsTriplet();
            classGroup = promotePupilTriplet.SearchCriteria.Classes;
            classItem = classGroup[classFullName];
            Assert.AreEqual(null, classItem, "The Class displays in Promote Pupil page");

            #endregion TEST

            #region POS-CONDITIONS

            //Re-select class page to delete data
            AutomationSugar.NavigateMenu("Tasks", "School Groups", "Manage Classes");

            manageClassesTriplet = new ManageClassesTriplet();
            manageClassesTriplet.SearchCriteria.SearchByAcademicYear = currentAcademic;
            classesResults = manageClassesTriplet.SearchCriteria.Search();
            classesTile = classesResults.SingleOrDefault(t => t.ClassFullName.Equals(classFullName));
            manageClassesPage = classesTile.Click<ManageClassesPage>();

            //Remove Active History
            activeHistoryTable = manageClassesPage.ActiveHistoryTable;
            var activeHistoryRow = activeHistoryTable.Rows.FirstOrDefault(t => t.StartDate.Equals(activeHistoryStartDate));
            activeHistoryRow.DeleteRow();
            activeHistoryTable.Refresh();

            manageClassesPage.Save();
            manageClassesPage.Refresh();

            // Delete Class
            var confirmDeleteDialog = manageClassesPage.Delete();
            confirmDeleteDialog.ConfirmDelete();
            #endregion POS-CONDITIONS

        }

        /// <summary>
        /// Author: Huy.Vo
        /// Description:Remove a class from a Pastoral structure for the current academic year by entering an End Date into  Associated Group record
        /// </summary>
        [WebDriverTest(TimeoutSeconds = 2400, Enabled = false, DataProvider = "TC_MC012_Data", Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { "g1" })]
        public void TC_MC012_Remove_Classes_By_Entering_An_End_Date_Into_Associated_Group_Record(string currentAcademicYear, string classFullName, string classShortName,
                                             string displayOrder, string activeHistoryStartDate, string activeHistoryEndDate,
                                             string yearGroup, string yearGroupStartDate, string yearGroupEndDate,
                                             string classTearcher, string classTeacherStartDate, string classTeacherEndDate,
                                             string staff, string pastoraleRole, string staffStartDate, string staffEndDate,
                                             string activeHistoryEndDateToRemove, string earlierAcademicYear)
        {
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);
            AutomationSugar.NavigateMenu("Tasks", "School Groups", "Manage Classes");

            #region PRE-CONDITIONS
            //  Search and delete if Class is existing
            var manageClassesTriplet = new ManageClassesTriplet();
            manageClassesTriplet.SearchCriteria.SearchByAcademicYear = currentAcademicYear;
            var classesResults = manageClassesTriplet.SearchCriteria.Search();
            var classesTile = classesResults.SingleOrDefault(t => t.ClassFullName.Equals(classFullName));
            manageClassesTriplet.SelectSearchTile(classesTile);
            manageClassesTriplet.Delete();

            // Create new Classe with Year Group
            var manageClassesPage = manageClassesTriplet.Create();
            manageClassesPage.ClassFullName = classFullName;
            manageClassesPage.ClassShortName = classShortName;
            manageClassesPage.DisplayOrder = displayOrder;

            //Active History table
            var activeHistoryTable = manageClassesPage.ActiveHistoryTable;
            activeHistoryTable[0].StartDate = activeHistoryStartDate;
            activeHistoryTable[0].EndDate = activeHistoryEndDate;
            manageClassesPage.Save();
            manageClassesPage.Refresh();

            // Scroll to Associated Group
            manageClassesPage.ScrollToAssociatedGroup();

            // Year Group table
            var yearGroupTable = manageClassesPage.YearGroupsTable;
            yearGroupTable[0].YearGroup = yearGroup;
            yearGroupTable[0].StartDate = yearGroupStartDate;
            yearGroupTable[0].EndDate = yearGroupEndDate;

            // Scoll to Staff Details
            manageClassesPage.ScrollToStaffDetails();

            // Input data for Class Teacher table
            var classTeacherTable = manageClassesPage.ClassTeacherTable;
            classTeacherTable[0].SelectClassTeacher = classTearcher;
            classTeacherTable[0].StartDate = classTeacherStartDate;
            classTeacherTable[0].EndDate = classTeacherEndDate;

            // Input data for Staff table
            var staffTable = manageClassesPage.StaffTable;
            staffTable[0].SelectStaff = staff;
            staffTable[0].PastoralRoleDropDown = pastoraleRole;
            staffTable[0].StartDate = staffStartDate;
            staffTable[0].EndDate = staffEndDate;

            // Save data
            manageClassesPage.Save();

            #endregion PRE-CONDITIONS

            #region STEPS
            // Select Class to remove
            manageClassesTriplet.SearchCriteria.SearchByAcademicYear = currentAcademicYear;
            classesResults = manageClassesTriplet.SearchCriteria.Search();
            classesTile = classesResults.SingleOrDefault(t => t.ClassFullName.Equals(classFullName));
            manageClassesPage = classesTile.Click<ManageClassesPage>();

            // Scroll to Associated Group
            manageClassesPage.ScrollToAssociatedGroup();

            // Update data for Year group data
            activeHistoryTable = manageClassesPage.ActiveHistoryTable;
            activeHistoryTable[0].EndDate = activeHistoryEndDateToRemove;

            // Save data
            var dialogConfirm = manageClassesPage.SaveToDeActive();
            dialogConfirm.ClickOk();
            manageClassesPage.Refresh();

            // Verify that save message update succesfully display
            Assert.AreEqual(true, manageClassesPage.IsSuccessMessageDisplayed(), "Remove current Academic year unsuccessfully");

            // Verify that Class is not displayed in current Academic year
            manageClassesTriplet.SearchCriteria.SearchByAcademicYear = currentAcademicYear;
            classesResults = manageClassesTriplet.SearchCriteria.Search();
            classesTile = classesResults.SingleOrDefault(t => t.ClassFullName.Equals(classFullName));
            Assert.AreEqual(null, classesTile);

            // Verify that Class is displayed in earlier Academic year
            manageClassesTriplet.SearchCriteria.SearchByAcademicYear = earlierAcademicYear;
            classesResults = manageClassesTriplet.SearchCriteria.Search();
            classesTile = classesResults.SingleOrDefault(t => t.ClassFullName.Equals(classFullName));
            Assert.AreNotEqual(null, classesTile);

            // Verify Class is not displayed in Allocate Future Pupils
            AutomationSugar.NavigateMenu("Tasks", "School Groups", "Allocate Future Pupils");
            var allocateFuture = new AllocateFuturePupilsTriplet();
            var classGroup = allocateFuture.SearchCriteria.Classes;
            var classItem = classGroup[classFullName];
            Assert.AreEqual(null, classItem, "The Class displays in Allocate Future Pupils");

            // Verify that class is not displayed in Allocate Pupils To Groups
            AutomationSugar.NavigateMenu("Tasks", "School Groups", "Allocate Pupils To Groups");
            var allowCatePupilToGroup = new AllocatePupilsToGroupsTriplet();
            classGroup = allowCatePupilToGroup.SearchCriteria.Classes;
            classItem = classGroup[classFullName];
            Assert.AreEqual(null, classItem, "The Class displays in Allocate Pupils to Groups");

            // Verify class is not displayed in Promote Pupils
            AutomationSugar.NavigateMenu("Tasks", "School Groups", "Promote Pupils");
            var promotePupilTriplet = new PromotePupilsTriplet();
            classGroup = promotePupilTriplet.SearchCriteria.Classes;
            classItem = classGroup[classFullName];
            Assert.AreEqual(null, classItem, "The Class displays in Promote Pupil page");

            #endregion TEST

            #region POS-CONDITIONS

            //Re-select class page to delete data
            AutomationSugar.NavigateMenu("Tasks", "School Groups", "Manage Classes");

            manageClassesTriplet = new ManageClassesTriplet();
            manageClassesTriplet.SearchCriteria.SearchByAcademicYear = earlierAcademicYear;
            classesResults = manageClassesTriplet.SearchCriteria.Search();
            classesTile = classesResults.SingleOrDefault(t => t.ClassFullName.Equals(classFullName));
            manageClassesPage = classesTile.Click<ManageClassesPage>();

            //Remove Active History
            activeHistoryTable = manageClassesPage.ActiveHistoryTable;
            var activeHistoryRow = activeHistoryTable.Rows.FirstOrDefault(t => t.StartDate.Equals(activeHistoryStartDate));
            activeHistoryRow.DeleteRow();
            activeHistoryTable.Refresh();

            // Remove Year Group
            yearGroupTable = manageClassesPage.YearGroupsTable;
            var yearGroupRow = yearGroupTable.Rows.FirstOrDefault(t => t.YearGroup.Equals(yearGroup));
            yearGroupRow.DeleteRow();
            yearGroupTable.Refresh();

            //Remove class Teacher
            classTeacherTable = manageClassesPage.ClassTeacherTable;
            var classTeacherRow = classTeacherTable.Rows.FirstOrDefault(t => t.SelectClassTeacher.Equals(classTearcher));
            classTeacherRow.DeleteRow();
            classTeacherTable.Refresh();

            //Remove class Teacher
            staffTable = manageClassesPage.StaffTable;
            var staffRow = staffTable.Rows.FirstOrDefault(t => t.SelectStaff.Equals(staff) && t.PastoralRoleDropDown.Equals(pastoraleRole));
            staffRow.DeleteRow();
            staffTable.Refresh();

            manageClassesPage.Save();
            manageClassesPage.Refresh();

            // Delete Class
            var confirmDeleteDialog = manageClassesPage.Delete();
            confirmDeleteDialog.ConfirmDelete();

            #endregion POS-CONDITIONS

        }

        /// <summary>
        /// Author: Huy.Vo
        /// Description:Remove a class from a Pastoral structure for the current academic year by deleting Association Group record
        /// </summary>
        [WebDriverTest(TimeoutSeconds = 2400, Enabled = false, DataProvider = "TC_MC013_Data", Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { "g1" })]
        public void TC_MC013_Remove_Class_From_Pastoral_Structure_For_Current_Academic_Year_By_Deleting_Association_Group_Record(string currentAcademicYear, string classFullName, string classShortName, string displayOrder,
                                              string activeHistoryStartDate, string activeHistoryEndDate,
                                              string yearGroup, string yearGroupStartDate, string yearGroupEndDate, string classTearcher, string classTeacherStartDate, string classTeacherEndDate,
            string staff, string pastoraleRole, string staffStartDate, string staffEndDate, string earlierAcademicYear)
        {
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);
            AutomationSugar.NavigateMenu("Tasks", "School Groups", "Manage Classes");

            #region PRE-CONDITIONS
            //  Search and delete if Class is existing
            var manageClassesTriplet = new ManageClassesTriplet();
            manageClassesTriplet.SearchCriteria.SearchByAcademicYear = currentAcademicYear;
            var classesResults = manageClassesTriplet.SearchCriteria.Search();
            var classesTile = classesResults.SingleOrDefault(t => t.ClassFullName.Equals(classFullName));
            manageClassesTriplet.SelectSearchTile(classesTile);
            manageClassesTriplet.Delete();

            // Create new Classe with Year Group
            var manageClassesPage = manageClassesTriplet.Create();
            manageClassesPage.ClassFullName = classFullName;
            manageClassesPage.ClassShortName = classShortName;
            manageClassesPage.DisplayOrder = displayOrder;

            //Active History table
            var activeHistoryTable = manageClassesPage.ActiveHistoryTable;
            activeHistoryTable[0].StartDate = activeHistoryStartDate;
            activeHistoryTable[0].EndDate = activeHistoryEndDate;
            manageClassesPage.Save();
            manageClassesPage.Refresh();

            // Scroll to Associated Group
            manageClassesPage.ScrollToAssociatedGroup();

            // Year Group table
            var yearGroupTable = manageClassesPage.YearGroupsTable;
            yearGroupTable[0].YearGroup = yearGroup;
            yearGroupTable[0].StartDate = yearGroupStartDate;
            yearGroupTable[0].EndDate = yearGroupEndDate;

            // Scoll to Staff Details
            manageClassesPage.ScrollToStaffDetails();

            // Input data for Class Teacher table
            var classTeacherTable = manageClassesPage.ClassTeacherTable;
            classTeacherTable[0].SelectClassTeacher = classTearcher;
            classTeacherTable[0].StartDate = classTeacherStartDate;
            classTeacherTable[0].EndDate = classTeacherEndDate;

            // Input data for Staff table
            var staffTable = manageClassesPage.StaffTable;
            staffTable[0].SelectStaff = staff;
            staffTable[0].PastoralRoleDropDown = pastoraleRole;
            staffTable[0].StartDate = staffStartDate;
            staffTable[0].EndDate = staffEndDate;

            // Save data
            manageClassesPage.Save();
            #endregion PRE-CONDITION

            #region TEST
            // Select Class to remove
            manageClassesTriplet.SearchCriteria.SearchByAcademicYear = currentAcademicYear;
            classesResults = manageClassesTriplet.SearchCriteria.Search();
            classesTile = classesResults.SingleOrDefault(t => t.ClassFullName.Equals(classFullName));
            manageClassesPage = classesTile.Click<ManageClassesPage>();

            // Scroll to Associated Group
            manageClassesPage.ScrollToAssociatedGroup();

            // Update endate to remove class
            yearGroupTable = manageClassesPage.YearGroupsTable;
            yearGroupTable.Rows.SingleOrDefault(x => x.YearGroup == yearGroup && x.StartDate == yearGroupStartDate &&
                x.EndDate == yearGroupEndDate).DeleteRow();

            manageClassesPage.Save();
            manageClassesPage.Refresh();

            // Verify that save message update succesfully display
            Assert.AreEqual(true, manageClassesPage.IsSuccessMessageDisplayed(), "Remove current Academic year unsuccessfully");

            // Verify that Class should no longer appear in Year  group  for any academic year
            AutomationSugar.NavigateMenu("Tasks", "School Groups", "Manage Year Groups");

            //Select Year Group that has class with Future Academic Year
            var manageYearGroupTriplet = new ManageYearGroupsTriplet();
            manageYearGroupTriplet.SearchCriteria.AcademicYear = currentAcademicYear;

            var yearGroupTile = manageYearGroupTriplet.SearchCriteria.Search().FirstOrDefault(x => x.FullName.Equals(yearGroup));
            var manageYearGroupDetail = yearGroupTile.Click<ManageYearGroupsPage>();

            //Scroll to Associated Groups
            manageYearGroupDetail.ScrollToAssociatedGroup();

            var classTable = manageYearGroupDetail.Classes;
            Assert.AreEqual(null, (classTable.Rows.SingleOrDefault(x => x.Class == classFullName && x.StartDate == yearGroupStartDate &&
               x.EndDate == yearGroupEndDate)), "Class is exist in Current Academic Year");

            // Verify Class is not displayed in Allocate Future Pupils
            AutomationSugar.NavigateMenu("Tasks", "School Groups", "Allocate Future Pupils");
            var allocateFuture = new AllocateFuturePupilsTriplet();
            var classGroup = allocateFuture.SearchCriteria.Classes;
            var classItem = classGroup[classFullName];
            Assert.AreNotEqual(null, classItem, "The Class doesn't display in Allocate Future Pupils");

            // Verify that class is not displayed in Allocate Pupils To Groups
            AutomationSugar.NavigateMenu("Tasks", "School Groups", "Allocate Pupils To Groups");
            var allowCatePupilToGroup = new AllocatePupilsToGroupsTriplet();
            classGroup = allowCatePupilToGroup.SearchCriteria.Classes;
            classItem = classGroup[classFullName];
            Assert.AreNotEqual(null, classItem, "The Class doesn't display in Allocate Pupils to Groups");

            // Verify class is not displayed in Promote Pupils
            AutomationSugar.NavigateMenu("Tasks", "School Groups", "Promote Pupils");
            var promotePupilTriplet = new PromotePupilsTriplet();
            classGroup = promotePupilTriplet.SearchCriteria.Classes;
            classItem = classGroup[classFullName];
            Assert.AreEqual(null, classItem, "The Class displays in Promote Pupil page");

            #endregion TEST

            #region POS-CONDITIONS

            //Re-select class page to delete data
            AutomationSugar.NavigateMenu("Tasks", "School Groups", "Manage Classes");

            manageClassesTriplet = new ManageClassesTriplet();
            manageClassesTriplet.SearchCriteria.SearchByAcademicYear = currentAcademicYear;
            classesResults = manageClassesTriplet.SearchCriteria.Search();
            classesTile = classesResults.SingleOrDefault(t => t.ClassFullName.Equals(classFullName));
            manageClassesPage = classesTile.Click<ManageClassesPage>();

            //Remove Active History
            activeHistoryTable = manageClassesPage.ActiveHistoryTable;
            var activeHistoryRow = activeHistoryTable.Rows.FirstOrDefault(t => t.StartDate.Equals(activeHistoryStartDate));
            activeHistoryRow.DeleteRow();
            activeHistoryTable.Refresh();

            //Remove class Teacher
            classTeacherTable = manageClassesPage.ClassTeacherTable;
            var classTeacherRow = classTeacherTable.Rows.FirstOrDefault(t => t.SelectClassTeacher.Equals(classTearcher));
            classTeacherRow.DeleteRow();
            classTeacherTable.Refresh();

            //Remove Staff
            staffTable = manageClassesPage.StaffTable;
            var staffRow = staffTable.Rows.FirstOrDefault(t => t.SelectStaff.Equals(staff) && t.PastoralRoleDropDown.Equals(pastoraleRole));
            staffRow.DeleteRow();
            staffTable.Refresh();

            manageClassesPage.Save();
            manageClassesPage.Refresh();

            // Delete Class
            var confirmDeleteDialog = manageClassesPage.Delete();
            confirmDeleteDialog.ConfirmDelete();

            #endregion POS-CONDITIONS


        }

        /// <summary>
        /// Author: Huy.Vo
        /// Description: Remove a class for the current academic year from the entire Pastoral structure by entering an End Date
        /// </summary>
        [WebDriverTest(TimeoutSeconds = 2400, Enabled = false, DataProvider = "TC_MC014_Data", Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { "g1" })]
        public void TC_MC014_Remove_Class_Current_Academic_Year_From_The_Entire_Pastoral_Structure_By_Entering_An_End_Date(
                                             string currentAcademicYear, string classFullName, string classShortName, string displayOrder,
                                             string activeHistoryStartDate, string activeHistoryEndDate,
                                             string yearGroup, string yearGroupStartDate, string yearGroupEndDate,
                                             string classTearcher, string classTeacherStartDate, string classTeacherEndDate, string staff, string pastoraleRole,
                                             string staffStartDate, string staffEndDate, string activeHistoryEndDateToRemove, string earlierAcademicYear)
        {

            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);
            AutomationSugar.NavigateMenu("Tasks", "School Groups", "Manage Classes");

            #region PRE-CONDITIONS

            //  Search and delete if Class is existing
            var manageClassesTriplet = new ManageClassesTriplet();
            manageClassesTriplet.SearchCriteria.SearchByAcademicYear = currentAcademicYear;
            var classesResults = manageClassesTriplet.SearchCriteria.Search();
            var classesTile = classesResults.SingleOrDefault(t => t.ClassFullName.Equals(classFullName));
            manageClassesTriplet.SelectSearchTile(classesTile);
            manageClassesTriplet.Delete();

            // Create new Classe with Year Group
            var manageClassesPage = manageClassesTriplet.Create();
            manageClassesPage.ClassFullName = classFullName;
            manageClassesPage.ClassShortName = classShortName;
            manageClassesPage.DisplayOrder = displayOrder;

            //Active History table
            var activeHistoryTable = manageClassesPage.ActiveHistoryTable;
            activeHistoryTable[0].StartDate = activeHistoryStartDate;
            activeHistoryTable[0].EndDate = activeHistoryEndDate;
            manageClassesPage.Save();
            manageClassesPage.Refresh();

            // Scroll to Associated Group
            manageClassesPage.ScrollToAssociatedGroup();

            // Year Group table
            var yearGroupTable = manageClassesPage.YearGroupsTable;
            yearGroupTable[0].YearGroup = yearGroup;
            yearGroupTable[0].StartDate = yearGroupStartDate;
            yearGroupTable[0].EndDate = yearGroupEndDate;

            // Scoll to Staff Details
            manageClassesPage.ScrollToStaffDetails();

            // Input data for Class Teacher table
            var classTeacherTable = manageClassesPage.ClassTeacherTable;
            classTeacherTable[0].SelectClassTeacher = classTearcher;
            classTeacherTable[0].StartDate = classTeacherStartDate;
            classTeacherTable[0].EndDate = classTeacherEndDate;

            // Input data for Staff table
            var staffTable = manageClassesPage.StaffTable;
            staffTable[0].SelectStaff = staff;
            staffTable[0].PastoralRoleDropDown = pastoraleRole;
            staffTable[0].StartDate = staffStartDate;
            staffTable[0].EndDate = staffEndDate;

            // Save data
            manageClassesPage.Save();

            #endregion PRE-CONDITIONS

            #region TEST

            // Seaching and select class to remove by add End date into Active history
            manageClassesTriplet.SearchCriteria.SearchByAcademicYear = currentAcademicYear;
            classesResults = manageClassesTriplet.SearchCriteria.Search();
            classesTile = classesResults.SingleOrDefault(t => t.ClassFullName.Equals(classFullName));
            manageClassesPage = classesTile.Click<ManageClassesPage>();

            // Update endate for active history to remove class
            activeHistoryTable = manageClassesPage.ActiveHistoryTable;
            activeHistoryTable[0].EndDate = activeHistoryEndDateToRemove;

            var confirmRequireDialog = manageClassesPage.SaveToDeActive();
            confirmRequireDialog.ClickOk();
            manageClassesPage.Refresh();

            // Verify that current class is not showing in current Academic year
            manageClassesTriplet.SearchCriteria.SearchByAcademicYear = currentAcademicYear;
            classesResults = manageClassesTriplet.SearchCriteria.Search();
            classesTile = classesResults.SingleOrDefault(t => t.ClassFullName.Equals(classFullName));
            Assert.AreEqual(null, classesTile, "Class is not moved to Current Academic Year");

            // Verify that current class is not showing in earlier Academic year
            manageClassesTriplet.SearchCriteria.SearchByAcademicYear = earlierAcademicYear;
            classesResults = manageClassesTriplet.SearchCriteria.Search();
            classesTile = classesResults.SingleOrDefault(t => t.ClassFullName.Equals(classFullName));
            Assert.AreNotEqual(null, classesTile, "Class is not moved to Current Academic Year");

            // Verify Class is not displayed in Allocate Future Pupils
            AutomationSugar.NavigateMenu("Tasks", "School Groups", "Allocate Future Pupils");
            var allocateFuture = new AllocateFuturePupilsTriplet();
            var classGroup = allocateFuture.SearchCriteria.Classes;
            var classItem = classGroup[classFullName];
            Assert.AreEqual(null, classItem, "The Class displays in Allocate Future Pupils");

            // Verify that class is not displayed in Allocate Pupils To Groups
            AutomationSugar.NavigateMenu("Tasks", "School Groups", "Allocate Pupils To Groups");
            var allowCatePupilToGroup = new AllocatePupilsToGroupsTriplet();
            classGroup = allowCatePupilToGroup.SearchCriteria.Classes;
            classItem = classGroup[classFullName];
            Assert.AreEqual(null, classItem, "The Class displays in Allocate Pupils to Groups");

            // Verify class is not displayed in Promote Pupils
            AutomationSugar.NavigateMenu("Tasks", "School Groups", "Promote Pupils");
            var promotePupilTriplet = new PromotePupilsTriplet();
            classGroup = promotePupilTriplet.SearchCriteria.Classes;
            classItem = classGroup[classFullName];
            Assert.AreEqual(null, classItem, "The Class displays in Promote Pupil page");

            #endregion TEST

            #region POS-CONDITIONS

            //  Delete class
            AutomationSugar.NavigateMenu("Tasks", "School Groups", "Manage Classes");

            manageClassesTriplet = new ManageClassesTriplet();
            manageClassesTriplet.SearchCriteria.SearchByAcademicYear = earlierAcademicYear;
            classesResults = manageClassesTriplet.SearchCriteria.Search();
            classesTile = classesResults.SingleOrDefault(t => t.ClassFullName.Equals(classFullName));
            manageClassesPage = classesTile.Click<ManageClassesPage>();

            activeHistoryTable = manageClassesPage.ActiveHistoryTable;

            // Remove Active History data
            var deleteActiveHistoryRow = activeHistoryTable.Rows.FirstOrDefault(p => p.StartDate.Contains(activeHistoryStartDate));
            deleteActiveHistoryRow.DeleteRow();

            // Remove Year Group
            yearGroupTable = manageClassesPage.YearGroupsTable;
            var yearGroupRow = yearGroupTable.Rows.FirstOrDefault(t => t.YearGroup.Equals(yearGroup));
            yearGroupRow.DeleteRow();
            yearGroupTable.Refresh();

            //Remove record in Class Teacher section
            classTeacherTable = manageClassesPage.ClassTeacherTable;
            var classTeacherRow = classTeacherTable.Rows.FirstOrDefault(p => p.SelectClassTeacher.Contains(classTearcher));
            classTeacherRow.DeleteRow();

            //Remove Staff
            staffTable = manageClassesPage.StaffTable;
            var staffRow = staffTable.Rows.FirstOrDefault(t => t.SelectStaff.Equals(staff) && t.PastoralRoleDropDown.Equals(pastoraleRole));
            staffRow.DeleteRow();
            staffTable.Refresh();

            manageClassesPage.Save();
            manageClassesPage.Refresh();

            // Delete Class
            var confirmDeleteDialog = manageClassesPage.Delete();
            confirmDeleteDialog.ConfirmDelete();


            #endregion POS-CONDITIONS

        }

        /// <summary>
        /// Author: Hieu Pham
        /// Description: Delete a Class with Learners linked to Class (System should Prevent this).
        /// Status : Pending by bug #14 : [PUPIL RECORD] Year Group record appear incorrectly in Add Pupil dialog 's 'Year Group' dropdown.
        /// </summary>
        [WebDriverTest(TimeoutSeconds = 2400, Enabled = false, DataProvider = "TC_MC015_Data", Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { "g1" })]
        public void TC_MC015_Delete_Class_With_Learner_Linked_To_Class(string classFullName, string classShortName, string displayOrder,
                                             string activeHistoryStartDate, string yearGroup, string yearGroupStartDate,
                                             string classTearcher, string classTeacherStartDate,
                                             string staff, string pastoraleRole, string staffStartDate, string[] pupil)
        {
            #region Pre-condition : Create new class link to a pupil

            // Login
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);

            // Navigate to Manage classes
            AutomationSugar.NavigateMenu("Tasks", "School Groups", "Manage Classes");

            //  Search and delete if Class is existing
            var manageClassesTriplet = new ManageClassesTriplet();
            var classesResults = manageClassesTriplet.SearchCriteria.Search();
            var classesTile = classesResults.FirstOrDefault(t => t.ClassFullName.Equals(classFullName));
            var manageClassPage = classesTile == null ? new ManageClassesPage() : classesTile.Click<ManageClassesPage>();
            manageClassPage.DeleteRecord();

            // Create new Classe with Year Group
            var manageClassesPage = manageClassesTriplet.Create();
            manageClassesPage.ClassFullName = classFullName;
            manageClassesPage.ClassShortName = classShortName;
            manageClassesPage.DisplayOrder = displayOrder;

            // Create record in Active History table
            var activeHistoryTable = manageClassesPage.ActiveHistoryTable;
            var activeHistoryRow = activeHistoryTable.Rows.FirstOrDefault(x => x.StartDate.Equals(String.Empty));
            activeHistoryRow.StartDate = activeHistoryStartDate;

            // Scroll to Associated Group
            manageClassesPage.ScrollToAssociatedGroup();

            // Create Record in Year Group table
            var yearGroupTable = manageClassesPage.YearGroupsTable;
            var yearGroupRow = yearGroupTable.Rows.FirstOrDefault(x => x.YearGroup.Equals(String.Empty));
            yearGroupRow.YearGroup = yearGroup;
            yearGroupRow.StartDate = yearGroupStartDate;

            // Scroll to Staff Details
            manageClassesPage.ScrollToStaffDetails();

            // Input data for Class Teacher table
            var classTeacherTable = manageClassesPage.ClassTeacherTable;
            var classTeacherRow = classTeacherTable.Rows.FirstOrDefault(x => x.SelectClassTeacher.Equals(String.Empty));
            classTeacherRow.SelectClassTeacher = classTearcher;
            classTeacherRow.StartDate = classTeacherStartDate;

            // Input data for Staff table
            var staffTable = manageClassesPage.StaffTable;
            var staffRow = staffTable.Rows.FirstOrDefault(x => x.SelectStaff.Equals(String.Empty));
            staffRow.SelectStaff = staff;
            staffRow.PastoralRoleDropDown = pastoraleRole;
            staffRow.StartDate = staffStartDate;

            // Save data
            manageClassesPage.Save();

            // Verify that save message succesfully display
            Assert.AreEqual(true, manageClassesPage.IsSuccessMessageDisplayed(), "Create new Class unsuccessfully");

            // Delete pupil if existed
            AutomationSugar.NavigateMenu("Tasks", "Pupils", "Delete Pupil");
            var deletePupilRecordTriplet = new DeletePupilRecordTriplet();
            deletePupilRecordTriplet.SearchCriteria.PupilName = String.Format("{0}, {1}", pupil[1], pupil[0]);
            var resultPupils = deletePupilRecordTriplet.SearchCriteria.Search();
            var pupilSearchTile = resultPupils.FirstOrDefault(t => t.Name.Contains(String.Format("{0}, {1}", pupil[1], pupil[0])));
            var deletePupilRecord = pupilSearchTile == null ? new DeletePupilRecordPage() : pupilSearchTile.Click<DeletePupilRecordPage>();
            deletePupilRecord.Delete();

            // Add new pupil
            // Navigate to Pupil Record
            AutomationSugar.NavigateMenu("Tasks", "Pupils", "Pupil Records");

            var pupilRecordTriplet = new PupilRecordTriplet();
            var addNewPupilDialog = pupilRecordTriplet.AddNewPupil();

            // Enter values
            addNewPupilDialog.Forename = pupil[0];
            addNewPupilDialog.SurName = pupil[1];
            addNewPupilDialog.Gender = pupil[2];
            addNewPupilDialog.DateOfBirth = pupil[3];
            var registrationDetailDialog = addNewPupilDialog.Continue();
            registrationDetailDialog.DateOfAdmission = pupil[4];
            registrationDetailDialog.YearGroup = pupil[5];
            registrationDetailDialog.ClassName = classFullName;
            registrationDetailDialog.CreateRecord();

            // Confirm create new pupil
            var confirmRequiredDialog = new POM.Components.Common.ConfirmRequiredDialog();
            confirmRequiredDialog.ClickOk();

            // Save values
            var pupilRecord = PupilRecordPage.Create();
            pupilRecord.SavePupil();

            #endregion

            #region STEPS

            // Search record again
            manageClassesTriplet.Refresh();
            classesResults = manageClassesTriplet.SearchCriteria.Search();
            classesTile = classesResults.FirstOrDefault(t => t.ClassFullName.Equals(classFullName));
            manageClassesPage = classesTile == null ? new ManageClassesPage() : classesTile.Click<ManageClassesPage>();

            // Delete Active History
            activeHistoryTable.Refresh();
            activeHistoryRow = activeHistoryRow = activeHistoryTable.Rows.FirstOrDefault(x => x.StartDate.Equals(activeHistoryStartDate));
            activeHistoryRow.DeleteRow();

            // Delete Year Group
            yearGroupTable.Refresh();
            yearGroupRow = yearGroupTable.Rows.FirstOrDefault(x => x.YearGroup.Equals(yearGroup));
            yearGroupRow.DeleteRow();

            // Delete Class Teacher
            classTeacherTable.Refresh();
            classTeacherRow = classTeacherTable.Rows.FirstOrDefault(x => x.SelectClassTeacher.Equals(classTearcher));
            classTeacherRow.DeleteRow();

            // Delete Staff
            staffTable.Refresh();
            staffRow = staffTable.Rows.FirstOrDefault(x => x.SelectStaff.Equals(staff));
            staffRow.DeleteRow();

            // Save values
            manageClassesPage.Save();

            // Delete record
            manageClassesPage.Refresh();
            manageClassesPage.DeleteRecord();

            // VP : Deletion of Class successfully prevented
            Assert.AreEqual(true, manageClassesPage.IsWarningMessageIsDisplayed(), "Delete record is not prevented");

            #endregion

            #region POS-CONDITIONS

            // Delete pupil if existed
            AutomationSugar.NavigateMenu("Tasks", "Pupils", "Delete Pupil");
            deletePupilRecordTriplet = new DeletePupilRecordTriplet();
            deletePupilRecordTriplet.SearchCriteria.PupilName = String.Format("{0}, {1}", pupil[1], pupil[0]);
            resultPupils = deletePupilRecordTriplet.SearchCriteria.Search();
            pupilSearchTile = resultPupils.FirstOrDefault(t => t.Name.Contains(String.Format("{0}, {1}", pupil[1], pupil[0])));
            deletePupilRecord = pupilSearchTile == null ? new DeletePupilRecordPage() : pupilSearchTile.Click<DeletePupilRecordPage>();
            deletePupilRecord.Delete();

            #endregion POS-CONDITIONS

        }

        /// <summary>
        /// TC MC-16
        /// Au : An Nguyen
        /// Description: Delete a Class with NO Learners linked to Class (System should allow this)
        /// Role: School Administrator
        /// </summary>
        [WebDriverTest(TimeoutSeconds = 800, Enabled = false, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { "g1" }, DataProvider = "TC_MC016_Data")]
        public void TC_MC016_Delete_Class_With_No_Learner_Linked_To_Class(string classFullName, string classShortName, string displayOrder, string startDate, string yearGroup, string yearGroupStartDate, string currentAcademic)
        {
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);

            #region Pre-condition : Create new class

            //Navigate to Manage classes
            AutomationSugar.NavigateMenu("Tasks", "School Groups", "Manage Classes");

            //Search and delete if Class is existing
            var manageClassesTriplet = new ManageClassesTriplet();
            manageClassesTriplet.SearchCriteria.SearchByAcademicYear = currentAcademic;
            var classesResults = manageClassesTriplet.SearchCriteria.Search();
            var classesTile = classesResults.FirstOrDefault(t => t.ClassFullName.Equals(classFullName));
            var manageClassPage = classesTile == null ? new ManageClassesPage() : classesTile.Click<ManageClassesPage>();
            manageClassPage.DeleteRecord();

            //Create new Classe with Year Group
            var manageClassesPage = manageClassesTriplet.Create();
            manageClassesPage.ClassFullName = classFullName;
            manageClassesPage.ClassShortName = classShortName;
            manageClassesPage.DisplayOrder = displayOrder;

            //Create record in Active History table
            var activeHistoryTable = manageClassesPage.ActiveHistoryTable;
            var activeHistoryRow = activeHistoryTable.Rows.FirstOrDefault(x => x.StartDate.Equals(String.Empty));
            activeHistoryRow.StartDate = startDate;

            //Scroll to Associated Group
            manageClassesPage.ScrollToAssociatedGroup();

            //Create Record in Year Group table
            var yearGroupTable = manageClassesPage.YearGroupsTable;
            var yearGroupRow = yearGroupTable.Rows.FirstOrDefault(x => x.YearGroup.Equals(String.Empty));
            yearGroupRow.YearGroup = yearGroup;
            yearGroupRow.StartDate = yearGroupStartDate;

            // Save data
            manageClassesPage.Save();

            #endregion

            #region Steps

            //Search class
            manageClassesTriplet.Refresh();
            manageClassesTriplet.SearchCriteria.SearchByAcademicYear = currentAcademic;
            classesResults = manageClassesTriplet.SearchCriteria.Search();
            classesTile = classesResults.FirstOrDefault(t => t.ClassFullName.Equals(classFullName));
            manageClassesPage = classesTile.Click<ManageClassesPage>();

            //Delete Year Group
            yearGroupRow = manageClassesPage.YearGroupsTable[0];
            yearGroupRow.DeleteRow();
            manageClassesPage.Save();

            //Delete class
            manageClassesPage.Refresh();
            manageClassesPage.DeleteRecord();

            //Search class again
            manageClassesTriplet.Refresh();
            manageClassesTriplet.SearchCriteria.SearchByAcademicYear = currentAcademic;
            classesResults = manageClassesTriplet.SearchCriteria.Search();
            classesTile = classesResults.FirstOrDefault(t => t.ClassFullName.Equals(classFullName));
            Assert.AreEqual(null, classesTile, "Delete class with no learner linked unsuccessfull");

            //Verify class disappear on Allocate Future Pupils
            AutomationSugar.NavigateMenu("Tasks", "School Groups", "Allocate Future Pupils");
            var allocateFutures = new AllocateFuturePupilsTriplet();
            var allocateFutureClasses = allocateFutures.SearchCriteria.Classes;
            var allocateFutureClass = allocateFutureClasses.CheckBoxItems.FirstOrDefault(t => t.Title.Equals(classFullName));
            Assert.AreEqual(null, allocateFutureClass, "Class does not disappear on Allocate Future Pupils screen");

            //Verify class appear on Allocate Pupils To Groups
            AutomationSugar.NavigateMenu("Tasks", "School Groups", "Allocate Pupils To Groups");
            var allocateGroups = new AllocatePupilsToGroupsTriplet();
            var allocateGroupsClasses = allocateGroups.SearchCriteria.YearGroups;
            var allocateGroupsClass = allocateGroupsClasses.CheckBoxItems.FirstOrDefault(t => t.Title.Equals(classFullName));
            Assert.AreEqual(null, allocateGroupsClass, "Class does not disappear on Allocate Pupils To Groups screen");

            //Verify class disappear on Promote Pupils
            AutomationSugar.NavigateMenu("Tasks", "School Groups", "Promote Pupils");
            var promote = new PromotePupilsTriplet();
            var promoteClasses = promote.SearchCriteria.Classes;
            var promoteClass = promoteClasses.CheckBoxItems.FirstOrDefault(t => t.Title.Equals(classFullName));
            Assert.AreEqual(null, promoteClass, "Class does not disappear on Promote Pupils screen");

            #endregion
        }

        #region DATA

        public List<object[]> TC_MC001_Data()
        {
            string pattern = "M/d/yyyy";
            string nextAcademicYear = String.Format("Academic Year {0}/{1}", (DateTime.Now.Year).ToString(), (DateTime.Now.Year + 1).ToString());
            string classFullName = SeleniumHelper.GenerateRandomString(10);
            string classShortName = "SN" + SeleniumHelper.GenerateRandomString(3);
            string displayOrder = "1";
            var activeHistoryStartDate = new DateTime(DateTime.Today.Year, 4, 1).ToString(pattern);
            var activeHistoryEndDate = new DateTime(DateTime.Today.Year + 10, 02, 22).ToString(pattern);

            // Year Group Data
            string yearGroup = "Year 1";
            var yearGroupStartDate = DateTime.Now.ToString(pattern);
            var yearGroupEndDate = new DateTime(DateTime.Today.Year + 10, 02, 22).ToString(pattern);

            // Class Teacher Data
            string classTeacher = "Avery, Helen";
            var classTeacherStartDate = DateTime.Now.ToString(pattern);
            var classTeacherEndDate = new DateTime(DateTime.Today.Year + 5, 02, 22).ToString(pattern);

            //Staff data
            string staff = "Brooks, Colm";
            string pastoralRole = "Supervisor";
            var staffStartDate = DateTime.Now.ToString(pattern);
            var staffEndDate = new DateTime(DateTime.Today.Year + 5, 02, 22).ToString(pattern);


            var data = new List<Object[]>
            {                
                new object[] {nextAcademicYear,classFullName,classShortName,displayOrder,activeHistoryStartDate,activeHistoryEndDate,
                              yearGroup,yearGroupStartDate,yearGroupEndDate,
                              classTeacher,classTeacherStartDate,classTeacherEndDate,
                              staff,pastoralRole,staffStartDate,staffEndDate}
            };
            return data;
        }
        public List<object[]> TC_MC002_Data()
        {
            string pattern = "M/d/yyyy";

            string nextAcademicYear = String.Format("Academic Year {0}/{1}", (DateTime.Now.Year).ToString(), (DateTime.Now.Year + 1).ToString());
            string classFullName = SeleniumHelper.GenerateRandomString(10);
            string classShortName = "SN" + SeleniumHelper.GenerateRandomString(3);
            string displayOrder = "1";
            var activeHistoryStartDate = new DateTime(DateTime.Today.Year, 01, 11).ToString(pattern);
            var activeHistoryEndDate = new DateTime(DateTime.Today.Year + 10, 02, 22).ToString(pattern);

            // Year Group Data
            string yearGroup = "Year 1";

            var yearGroupStartDate = DateTime.Now.ToString(pattern);
            var yearGroupEndDate = new DateTime(DateTime.Today.Year + 10, 02, 22).ToString(pattern);

            // Class Teacher Data
            string classTeacher = "Brennan, Teagan";

            var classTeacherStartDate = DateTime.Now.ToString(pattern);
            var classTeacherEndDate = new DateTime(DateTime.Today.Year + 5, 02, 22).ToString(pattern);

            //Staff data
            string staff = "Brooks, Colm";
            string pastoralRole = "Supervisor";
            var staffStartDate = DateTime.Now.ToString(pattern);
            var staffEndDate = new DateTime(DateTime.Today.Year + 10, 02, 22).ToString(pattern);

            // Data for update 

            string classFullNameUpdate = "FNU" + SeleniumHelper.GenerateRandomString(3);
            string classShortNameUpdate = "SNU" + SeleniumHelper.GenerateRandomString(3);
            string displayOrderUpdate = "5";

            // Update Active history

            var activeHistoryStartDateUpdate = new DateTime(DateTime.Today.Year, 03, 12).ToString(pattern);
            var activeHistoryEndDateUpdate = new DateTime(DateTime.Today.Year + 2, 02, 25).ToString(pattern);

            //Update Year Group
            string yearGroupUpdate = "Year 2";
            var yearGroupStartDateUpdate = new DateTime(DateTime.Today.Year + 1, 01, 12).ToString(pattern);
            var yearGroupEndDateUpdate = new DateTime(DateTime.Today.Year + 2, 02, 25).ToString(pattern);

            //Update Teacher Class
            string classTeacherUpdate = "Grosvenor, Gillian";
            var classTeacherStartDateUpdate = new DateTime(DateTime.Today.Year + 1, 01, 12).ToString(pattern);
            var classTeacherEndDateUpdate = new DateTime(DateTime.Today.Year + 2, 02, 25).ToString(pattern);

            //Update Staff 
            string staffUpdate = "Fortune, Sheila";
            string staffPastoralRoleUpdate = "Pastoral Support";
            var staffStartDateUpdate = new DateTime(DateTime.Today.Year + 1, 01, 12).ToString(pattern);
            var staffEndDateUpdate = new DateTime(DateTime.Today.Year + 2, 02, 25).ToString(pattern);

            var data = new List<Object[]>
            {                
                new object[] {nextAcademicYear,classFullName,classShortName,displayOrder,
                              activeHistoryStartDate,activeHistoryEndDate,
                              yearGroup,yearGroupStartDate,yearGroupEndDate,
                              classTeacher,classTeacherStartDate,classTeacherEndDate,
                              staff,pastoralRole,staffStartDate,staffEndDate,
                              classFullNameUpdate,classShortNameUpdate,displayOrderUpdate,
                              activeHistoryStartDateUpdate, activeHistoryEndDateUpdate,
                              yearGroupUpdate,yearGroupStartDateUpdate,yearGroupEndDateUpdate,
                              classTeacherUpdate,classTeacherStartDateUpdate, classTeacherEndDateUpdate,
                              staffUpdate,staffPastoralRoleUpdate, staffStartDateUpdate,staffEndDateUpdate }
            };
            return data;
        }
        public List<object[]> TC_MC003_Data()
        {
            string pattern = "M/d/yyyy";
            string classFullName = "Full_" + SeleniumHelper.GenerateRandomString(8);
            string classShortName = "Short_" + SeleniumHelper.GenerateRandomString(4);
            string displayOrder = "1";
            string activeHistoryStartDate = new DateTime(DateTime.Today.Year, 4, 1).ToString(pattern);

            // Year Group Data
            string yearGroup = "Year 1";
            string yearGroupStartDate = (DateTime.Now).ToString(pattern);

            // Class Teacher Data
            string classTeacher = "Brennan, Teagan";
            string classTeacherStartDate = (DateTime.Now).ToString(pattern);

            //Staff data
            string staff = "Brooks, Colm";
            string pastoralRole = "Assistant Head";
            string staffStartDate = (DateTime.Now).ToString(pattern);

            var data = new List<Object[]>
            {                
                new object[] {classFullName, classShortName, displayOrder, activeHistoryStartDate,
                              yearGroup, yearGroupStartDate, classTeacher, classTeacherStartDate,
                              staff, pastoralRole, staffStartDate}
            };
            return data;
        }
        public List<object[]> TC_MC004_Data()
        {
            string pattern = "M/d/yyyy";
            string currentAcademicYear = String.Format("Academic Year {0}/{1}", (DateTime.Now.Year).ToString(), (DateTime.Now.Year + 1).ToString());
            string classFullName = SeleniumHelper.GenerateRandomString(3);
            string classShortName = "SN " + SeleniumHelper.GenerateRandomString(5);
            string displayOrder = "1";

            var activeHistoryStartDate = new DateTime(DateTime.Today.Year, 4, 1).ToString(pattern);
            var activeHistoryEndDate = new DateTime(DateTime.Today.Year, 12, 07).ToString(pattern);
            // Year Group Data
            string yearGroup = "Year 1";
            var yearGroupStartDate = (DateTime.Now).ToString(pattern);
            var yearGroupEndDate = (DateTime.Now).ToString(pattern);

            // Class Teacher Data
            string classTeacher = "Brown, Pauline";
            var classTeacherStartDate = (DateTime.Now).ToString(pattern);
            var classTeacherEndDate = (DateTime.Now).ToString(pattern);

            //Staff data
            string staff = "Fortune, Sheila";
            string pastoralRole = "Supervisor";
            var staffStartDate = (DateTime.Now).ToString(pattern);
            var staffEndDate = (DateTime.Now).ToString(pattern);

            string classFullNameUpdate = "UP" + SeleniumHelper.GenerateRandomString(5);
            string classShortNameUpdate = "SUP" + SeleniumHelper.GenerateRandomString(3);
            string displayOrderUpdate = "5";

            // Update Active history
            var activeHistoryStartDateUpdate =  (DateTime.Now.AddDays(2)).ToString(pattern);
            var activeHistoryEndDateUpdate = (DateTime.Now.AddDays(365)).ToString(pattern);
            //Update Year Group
            string yearGroupUpdate = "Year 2";
            var yearGroupStartDateUpdate = DateTime.Now.AddDays(1).ToString(pattern);
            var yearGroupEndDateUpdate = DateTime.Now.AddDays(1).ToString(pattern);

            //Update Teacher Class
            string classTeacherUpdate = "Joyner, Oliver";

            var classTeacherStartDateUpdate = DateTime.Now.AddDays(1).ToString(pattern);
            var classTeacherEndDateUpdate = DateTime.Now.AddDays(1).ToString(pattern);

            //Update Staff 
            string staffUpdate = "Brennan, Teagan";
            string staffPastoralRoleUpdate = "Pastoral Support";
            var staffStartDateUpdate = DateTime.Now.AddDays(1).ToString(pattern);
            var staffEndDateUpdate = DateTime.Now.AddDays(1).ToString(pattern);

            var data = new List<Object[]>
            {                
                new object[] {currentAcademicYear,classFullName,classShortName,displayOrder,
                              activeHistoryStartDate,activeHistoryEndDate,
                              yearGroup,yearGroupStartDate,yearGroupEndDate,
                              classTeacher,classTeacherStartDate,classTeacherEndDate,
                              staff,pastoralRole,staffStartDate,staffEndDate,
                              classFullNameUpdate,classShortNameUpdate,displayOrderUpdate,
                              activeHistoryStartDateUpdate, activeHistoryEndDateUpdate,
                              yearGroupUpdate,yearGroupStartDateUpdate,yearGroupEndDateUpdate,
                              classTeacherUpdate,classTeacherStartDateUpdate, classTeacherEndDateUpdate,
                              staffUpdate,staffPastoralRoleUpdate, staffStartDateUpdate,staffEndDateUpdate }
            };
            return data;
        }
        public List<object[]> TC_MC005_Data()
        {
            string pattern = "M/d/yyyy";
            string currentAcademicYear = String.Format("Academic Year {0}/{1}", (DateTime.Now.Year).ToString(), (DateTime.Now.Year + 1).ToString());
            string classFullName = SeleniumHelper.GenerateRandomString(3);
            string classShortName = "SN" + SeleniumHelper.GenerateRandomString(3);
            string displayOrder = "1";
            var activeHistoryStartDate = new DateTime(DateTime.Today.Year, 09, 11).ToString(pattern);
            var activeHistoryEndDate = new DateTime(DateTime.Today.Year, 10, 22).ToString(pattern);

            // Year Group Data
            string yearGroup = "Year 1";
            var yearGroupStartDate = new DateTime(DateTime.Today.Year, 09, 11).ToString(pattern);
            var yearGroupEndDate = new DateTime(DateTime.Today.Year, 10, 22).ToString(pattern);

            // Class Teacher Data
            string classTeacher = "Brennan, Teagan";
            var classTeacherStartDate = new DateTime(DateTime.Today.Year, 09, 11).ToString(pattern);
            var classTeacherEndDate = new DateTime(DateTime.Today.Year, 10, 22).ToString(pattern);

            //Staff data
            string staff = "Brooks, Colm";
            string pastoralRole = "Supervisor";
            var staffStartDate = new DateTime(DateTime.Today.Year, 09, 11).ToString(pattern);
            var staffEndDate = new DateTime(DateTime.Today.Year, 10, 22).ToString(pattern);


            var data = new List<Object[]>
            {                
                new object[] {currentAcademicYear,classFullName,classShortName,displayOrder,activeHistoryStartDate,activeHistoryEndDate,
                              yearGroup,yearGroupStartDate,yearGroupEndDate,
                              classTeacher,classTeacherStartDate,classTeacherEndDate,
                              staff,pastoralRole,staffStartDate,staffEndDate}
            };
            return data;
        }
        public List<object[]> TC_MC006_Data()
        {
            string currentAcademicYear = String.Format("Academic Year {0}/{1}", DateTime.Now.Year.ToString(), (DateTime.Now.Year + 1).ToString());

            var data = new List<Object[]>
            {                
                new object[] 
                {
                    new string[]{SeleniumHelper.GenerateRandomString(15), SeleniumHelper.GenerateRandomString(10), "1"},
                    new string[] {(new DateTime(DateTime.Today.Year, 9, 5)).ToString("M/d/yyyy"),
                                  (new DateTime(DateTime.Today.Year, 10, 20)).ToString("M/d/yyyy")},
                    new string[]{String.Format("Update_{0}", SeleniumHelper.GenerateRandomString(10)), String.Format("Update_{0}", SeleniumHelper.GenerateRandomString(3)), "2"},
                    new string[] {(new DateTime(DateTime.Today.Year, 9, 5)).ToString("M/d/yyyy"),
                                  (new DateTime(DateTime.Today.Year + 2, 10, 20)).ToString("M/d/yyyy")},
                    new string[] {"Year 1", (new DateTime(DateTime.Today.Year, 9, 5)).ToString("M/d/yyyy"),
                                  (new DateTime(DateTime.Today.Year, 10, 20)).ToString("M/d/yyyy")},
                    new string[] {"Brooks, Colm", (new DateTime(DateTime.Today.Year, 9, 5)).ToString("M/d/yyyy"),
                                  (new DateTime(DateTime.Today.Year , 10, 20)).ToString("M/d/yyyy")},
                    new string[] {"Avery, Helen", "Learning Mentor", 
                                  (new DateTime(DateTime.Today.Year, 9, 5)).ToString("M/d/yyyy"),
                                  (new DateTime(DateTime.Today.Year , 10, 20)).ToString("M/d/yyyy")},
                    currentAcademicYear
                }
            };
            return data;
        }
        public List<object[]> TC_MC009_Data()
        {
            string pattern = "M/d/yyyy";
            string nextAcademicYear = String.Format("Academic Year {0}/{1}", (DateTime.Now.Year + 1).ToString(), (DateTime.Now.Year + 2).ToString());
            string classFullName = SeleniumHelper.GenerateRandomString(3);
            string classShortName = "SN" + SeleniumHelper.GenerateRandomString(3);
            string displayOrder = "1";
            var activeHistoryStartDate = new DateTime(DateTime.Today.Year + 1, 01, 11).ToString(pattern);
            var activeHistoryEndDate = new DateTime(DateTime.Today.Year + 2, 02, 22).ToString(pattern);

            // Year Group Data
            string yearGroup = "Year 1";
            var yearGroupStartDate = new DateTime(DateTime.Today.Year + 1, 01, 11).ToString(pattern);
            var yearGroupEndDate = new DateTime(DateTime.Today.Year + 2, 02, 20).ToString(pattern);

            // Class Teacher Data
            string classTeacher = "Brennan, Teagan";
            var classTeacherStartDate = new DateTime(DateTime.Today.Year + 1, 01, 11).ToString(pattern);
            var classTeacherEndDate = new DateTime(DateTime.Today.Year + 2, 02, 22).ToString(pattern);

            //Staff data
            string staff = "Brooks, Colm";
            string pastoralRole = "Supervisor";
            var staffStartDate = new DateTime(DateTime.Today.Year + 1, 01, 11).ToString(pattern);
            var staffEndDate = new DateTime(DateTime.Today.Year + 2, 01, 22).ToString(pattern);

            var yearGroupEndDateToRemove = new DateTime(DateTime.Today.Year + 1, 07, 31).ToString(pattern);
            //       var currentEndDateToRemove = new DateTime(DateTime.Today.Year + 1, 03, 22).ToString(pattern);

            string currentAcademic = String.Format("Academic Year {0}/{1}", (DateTime.Now.Year).ToString(), (DateTime.Now.Year + 1).ToString());

            var data = new List<Object[]>
            {                
                new object[] {nextAcademicYear,classFullName,classShortName,displayOrder,activeHistoryStartDate,activeHistoryEndDate,
                              yearGroup,yearGroupStartDate,yearGroupEndDate,
                              classTeacher,classTeacherStartDate,classTeacherEndDate,
                              staff,pastoralRole,staffStartDate,staffEndDate,yearGroupEndDateToRemove,currentAcademic}
            };
            return data;
        }
        public List<object[]> TC_MC010_Data()
        {
            string pattern = "M/d/yyyy";
            string nextAcademicYear = String.Format("Academic Year {0}/{1}", (DateTime.Now.Year + 1).ToString(), (DateTime.Now.Year + 2).ToString());
            string classFullName = SeleniumHelper.GenerateRandomString(3);
            string classShortName = "SN" + SeleniumHelper.GenerateRandomString(3);
            string displayOrder = "1";
            var activeHistoryStartDate = new DateTime(DateTime.Today.Year + 1, 01, 11).ToString(pattern);
            var activeHistoryEndDate = new DateTime(DateTime.Today.Year + 2, 02, 22).ToString(pattern);

            // Year Group Data
            string yearGroup = "Year 1";
            var yearGroupStartDate = new DateTime(DateTime.Today.Year + 1, 01, 11).ToString(pattern);
            var yearGroupEndDate = new DateTime(DateTime.Today.Year + 2, 02, 20).ToString(pattern);

            // Class Teacher Data
            string classTeacher = "Brennan, Teagan";
            var classTeacherStartDate = new DateTime(DateTime.Today.Year + 1, 01, 11).ToString(pattern);
            var classTeacherEndDate = new DateTime(DateTime.Today.Year + 2, 02, 22).ToString(pattern);

            //Staff data
            string staff = "Brooks, Colm";
            string pastoralRole = "Supervisor";
            var staffStartDate = new DateTime(DateTime.Today.Year + 1, 01, 11).ToString(pattern);
            var staffEndDate = new DateTime(DateTime.Today.Year + 2, 01, 22).ToString(pattern);
            var endDateToRemove = new DateTime(DateTime.Today.Year + 1, 07, 31).ToString(pattern);
            string currentAcademic = String.Format("Academic Year {0}/{1}", (DateTime.Now.Year).ToString(), (DateTime.Now.Year + 1).ToString());

            var data = new List<Object[]>
            {                
                new object[] {nextAcademicYear,classFullName,classShortName,displayOrder,activeHistoryStartDate,activeHistoryEndDate,
                              yearGroup,yearGroupStartDate,yearGroupEndDate,
                              classTeacher,classTeacherStartDate,classTeacherEndDate,
                              staff,pastoralRole,staffStartDate,staffEndDate,endDateToRemove,currentAcademic}
            };
            return data;
        }
        public List<object[]> TC_MC011_Data()
        {
            string pattern = "M/d/yyyy";
            string nextAcademicYear = String.Format("Academic Year {0}/{1}", (DateTime.Now.Year + 1).ToString(), (DateTime.Now.Year + 2).ToString());
            string classFullName = SeleniumHelper.GenerateRandomString(3);
            string classShortName = "SN" + SeleniumHelper.GenerateRandomString(3);
            string displayOrder = "1";
            var activeHistoryStartDate = new DateTime(DateTime.Today.Year + 1, 1, 16).ToString(pattern);
            var activeHistoryEndDate = new DateTime(DateTime.Today.Year + 1, 11, 04).ToString(pattern);

            // Year Group Data
            string yearGroup = "Year 1";
            var yearGroupStartDate = new DateTime(DateTime.Today.Year + 1, 10, 18).ToString(pattern);
            var yearGroupEndDate = new DateTime(DateTime.Today.Year + 1, 11, 04).ToString(pattern);

            // Class Teacher Data
            string classTeacher = "Brennan, Teagan";
            var classTeacherStartDate = new DateTime(DateTime.Today.Year + 1, 09, 12).ToString(pattern);
            var classTeacherEndDate = new DateTime(DateTime.Today.Year + 2, 01, 12).ToString(pattern);

            //Staff data
            string staff = "Brooks, Colm";
            string pastoralRole = "Supervisor";
            var staffStartDate = new DateTime(DateTime.Today.Year + 1, 09, 12).ToString(pattern);
            var staffEndDate = new DateTime(DateTime.Today.Year + 2, 01, 12).ToString(pattern);

            var activeHistoryUpdateEndDate = new DateTime(DateTime.Today.Year + 1, 07, 31).ToString(pattern);
            string currentAcademic = String.Format("Academic Year {0}/{1}", (DateTime.Now.Year).ToString(), (DateTime.Now.Year + 1).ToString());

            var data = new List<Object[]>
            {                
                new object[] {nextAcademicYear,classFullName,classShortName,displayOrder,activeHistoryStartDate,activeHistoryEndDate,
                              activeHistoryUpdateEndDate,currentAcademic}
            };
            return data;
        }
        public List<object[]> TC_MC012_Data()
        {
            string pattern = "M/d/yyyy";
            string currentAcademicYear = String.Format("Academic Year {0}/{1}", (DateTime.Now.Year).ToString(), (DateTime.Now.Year + 1).ToString());
            string classFullName = SeleniumHelper.GenerateRandomString(3);
            string classShortName = "SN " + SeleniumHelper.GenerateRandomString(5);
            string displayOrder = "1";

            var activeHistoryStartDate = new DateTime(DateTime.Today.Year, 02, 12).ToString(pattern);
            var activeHistoryEndDate = new DateTime(DateTime.Today.Year, 12, 07).ToString(pattern);
            // Year Group Data
            string yearGroup = "Year 3";
            var yearGroupStartDate = new DateTime(DateTime.Today.Year, 02, 12).ToString(pattern);
            var yearGroupEndDate = new DateTime(DateTime.Today.Year, 12, 07).ToString(pattern);

            // Class Teacher Data
            string classTeacher = "Brown, Pauline";
            var classTeacherStartDate = new DateTime(DateTime.Today.Year, 02, 12).ToString(pattern);
            var classTeacherEndDate = new DateTime(DateTime.Today.Year, 12, 07).ToString(pattern);

            //Staff data
            string staff = "Fortune, Sheila";
            string pastoralRole = "Supervisor";
            var staffStartDate = new DateTime(DateTime.Today.Year, 02, 12).ToString(pattern);
            var staffEndDate = new DateTime(DateTime.Today.Year, 12, 07).ToString(pattern);

            var yearGroupEndDateToRemove = new DateTime(DateTime.Today.Year, 07, 31).ToString(pattern);
            var earlierAcademicYear = String.Format("Academic Year {0}/{1}", (DateTime.Now.Year - 1).ToString(), (DateTime.Now.Year).ToString());
            var data = new List<Object[]>
            {                
                new object[] {currentAcademicYear,classFullName,classShortName,displayOrder,
                              activeHistoryStartDate,activeHistoryEndDate,
                              yearGroup,yearGroupStartDate,yearGroupEndDate,
                              classTeacher,classTeacherStartDate,classTeacherEndDate,
                              staff,pastoralRole,staffStartDate,staffEndDate,
                              yearGroupEndDateToRemove,earlierAcademicYear }
            };
            return data;
        }
        public List<object[]> TC_MC013_Data()
        {
            string pattern = "M/d/yyyy";
            string currentAcademicYear = String.Format("Academic Year {0}/{1}", (DateTime.Now.Year).ToString(), (DateTime.Now.Year + 1).ToString());
            string classFullName = SeleniumHelper.GenerateRandomString(3);
            string classShortName = "SN " + SeleniumHelper.GenerateRandomString(5);
            string displayOrder = "1";

            var activeHistoryStartDate = new DateTime(DateTime.Today.Year, 02, 12).ToString(pattern);
            var activeHistoryEndDate = new DateTime(DateTime.Today.Year, 12, 07).ToString(pattern);
            // Year Group Data
            string yearGroup = "Year 1";
            var yearGroupStartDate = new DateTime(DateTime.Today.Year, 02, 12).ToString(pattern);
            var yearGroupEndDate = new DateTime(DateTime.Today.Year, 12, 07).ToString(pattern);

            // Class Teacher Data
            string classTeacher = "Brown, Pauline";
            var classTeacherStartDate = new DateTime(DateTime.Today.Year, 02, 12).ToString(pattern);
            var classTeacherEndDate = new DateTime(DateTime.Today.Year, 12, 07).ToString(pattern);

            //Staff data
            string staff = "Fortune, Sheila";
            string pastoralRole = "Supervisor";
            var staffStartDate = new DateTime(DateTime.Today.Year, 02, 12).ToString(pattern);
            var staffEndDate = new DateTime(DateTime.Today.Year, 12, 07).ToString(pattern);

            var earlierAcademicYear = String.Format("Academic Year {0}/{1}", (DateTime.Now.Year - 1).ToString(), (DateTime.Now.Year).ToString());
            var data = new List<Object[]>
            {                
                new object[] {currentAcademicYear,classFullName,classShortName,displayOrder,
                              activeHistoryStartDate,activeHistoryEndDate,
                              yearGroup,yearGroupStartDate,yearGroupEndDate,
                              classTeacher,classTeacherStartDate,classTeacherEndDate,
                              staff,pastoralRole,staffStartDate,staffEndDate,
                              earlierAcademicYear }
            };
            return data;
        }
        public List<object[]> TC_MC014_Data()
        {
            string pattern = "M/d/yyyy";
            string currentAcademicYear = String.Format("Academic Year {0}/{1}", (DateTime.Now.Year).ToString(), (DateTime.Now.Year + 1).ToString());
            string classFullName = SeleniumHelper.GenerateRandomString(3);
            string classShortName = "SN " + SeleniumHelper.GenerateRandomString(5);
            string displayOrder = "1";

            var activeHistoryStartDate = new DateTime(DateTime.Today.Year, 02, 12).ToString(pattern);
            var activeHistoryEndDate = new DateTime(DateTime.Today.Year, 12, 07).ToString(pattern);
            // Year Group Data
            string yearGroup = "Year 1";
            var yearGroupStartDate = new DateTime(DateTime.Today.Year, 02, 12).ToString(pattern);
            var yearGroupEndDate = new DateTime(DateTime.Today.Year, 12, 07).ToString(pattern);

            // Class Teacher Data
            string classTeacher = "Brown, Pauline";
            var classTeacherStartDate = new DateTime(DateTime.Today.Year, 02, 12).ToString(pattern);
            var classTeacherEndDate = new DateTime(DateTime.Today.Year, 12, 07).ToString(pattern);

            //Staff data
            string staff = "Fortune, Sheila";
            string pastoralRole = "Supervisor";
            var staffStartDate = new DateTime(DateTime.Today.Year, 02, 12).ToString(pattern);
            var staffEndDate = new DateTime(DateTime.Today.Year, 12, 07).ToString(pattern);

            var activeHistoryEndDateToRemove = new DateTime(DateTime.Today.Year, 07, 31).ToString(pattern);

            var earlierAcademicYear = String.Format("Academic Year {0}/{1}", (DateTime.Now.Year - 1).ToString(), (DateTime.Now.Year).ToString());
            var data = new List<Object[]>
            {                
                new object[] {currentAcademicYear,classFullName,classShortName,displayOrder,
                              activeHistoryStartDate,activeHistoryEndDate,
                              yearGroup,yearGroupStartDate,yearGroupEndDate,
                              classTeacher,classTeacherStartDate,classTeacherEndDate,
                              staff,pastoralRole,staffStartDate,staffEndDate,
                              activeHistoryEndDateToRemove,earlierAcademicYear }
            };
            return data;
        }
        public List<object[]> TC_MC015_Data()
        {
            string pattern = "M/d/yyyy";
            string classFullName = "Full_" + SeleniumHelper.GenerateRandomString(8);
            string classShortName = "Short_" + SeleniumHelper.GenerateRandomString(4);
            string displayOrder = "1";
            string activeHistoryStartDate = new DateTime(DateTime.Now.Year, 8, 1).ToString(pattern);

            // Year Group Data
            string yearGroup = "Year 1";
            string yearGroupStartDate = new DateTime(DateTime.Now.Year, 8, 1).ToString(pattern);

            // Class Teacher Data
            string classTeacher = "Brennan, Teagan";
            string classTeacherStartDate = new DateTime(DateTime.Now.Year, 8, 1).ToString(pattern);

            //Staff data
            string staff = "Brooks, Colm";
            string pastoralRole = "Assistant Head";
            string staffStartDate = new DateTime(DateTime.Now.Year, 8, 1).ToString(pattern);

            // Pupil Data
            string randomString = SeleniumHelper.GenerateRandomString(8);
            string surName = "SUR_" + randomString;
            string foreName = "FORE_" + randomString;
            string gender = "Male";
            string dateOfBirth = DateTime.ParseExact("10/16/2006", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            string DateOfAdmission = DateTime.ParseExact("10/19/2010", pattern, CultureInfo.InvariantCulture).ToString(pattern);

            var data = new List<Object[]>
            {                
                new object[] {classFullName, classShortName, displayOrder, activeHistoryStartDate,
                              yearGroup, yearGroupStartDate, classTeacher, classTeacherStartDate,
                              staff, pastoralRole, staffStartDate,
                new string[] {foreName, surName, gender, dateOfBirth, DateOfAdmission, yearGroup}}
            };
            return data;
        }
        public List<object[]> TC_MC016_Data()
        {
            string pattern = "M/d/yyyy";
            string fullName = String.Format("{0} {1}_{2}", "Class", "Avn", SeleniumHelper.GenerateRandomString(8));
            string shortName = String.Format("{0}{1}", "CA", SeleniumHelper.GenerateRandomString(4));
            string displayOrder = SeleniumHelper.GenerateRandomNumberUsingDateTime();
            string startDate = SeleniumHelper.GetStartDateAcademicYear(DateTime.Now).ToString(pattern);
            string currentAcademic = SeleniumHelper.GetAcademicYear(DateTime.Now);
            var res = new List<Object[]>
            {                
                new object[] 
                {
                    fullName, shortName, displayOrder, startDate, "Year 7", startDate, currentAcademic,
                }
            };
            return res;
        }

        #endregion DATA
    }

}


