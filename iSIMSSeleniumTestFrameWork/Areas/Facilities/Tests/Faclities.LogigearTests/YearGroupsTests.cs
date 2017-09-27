using Facilities.Data;
using NUnit.Framework.Constraints;
using POM.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using TestSettings;
using WebDriverRunner.internals;
using NUnit.Framework;
using System.Globalization;
using POM.Components.SchoolGroups;
using POM.Components.Common;
using POM.Components.Pupil;
using Selene.Support.Attributes;
using SeSugar.Data;
using SeSugar;


namespace Faclities.LogigearTests
{
    public class YearGroupsTests
    {
        /// <summary>
        /// Author: Huy Vo
        /// Description: Create new Year Groups with an Active History starting in the Next Academic Year and associate with the new Year Groups created for the next academic year
        /// </summary>
        [WebDriverTest(TimeoutSeconds = 2000, Enabled = true, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { "g1" , "P2"}, DataProvider = "TC_YG001_Data")]
        public void TC_YG001_Create_Year_Groups_With_An_Active_History_Starting_In_The_Next_Academic_Year(string[] basicDetails, string[] activeHistoryDetails, string curriculumYear,
                                                                                                          string[] classDetails, string[] headOfYearDetails, string[] staffDetails, string futureAcademicYear)
        {
            #region TEST STEPS

            //Login as School Administrator and navigate to Manage Year Groups
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);
            Wait.WaitForDocumentReady();
            SeleniumHelper.NavigateMenu("Tasks", "School Groups", "Manage Year Groups");
            Wait.WaitForDocumentReady();

            //Select Academic Year by next academic
            var manageYearGroupTriplet = new ManageYearGroupsTriplet();
            Wait.WaitForDocumentReady();
            manageYearGroupTriplet.SearchCriteria.AcademicYear = futureAcademicYear;


            //Add new Year Group for next academic
            var manageYearGroupDetail = manageYearGroupTriplet.AddYearGroup();

            //Input basic information
            manageYearGroupDetail.FullName = basicDetails[0];
            manageYearGroupDetail.ShortName = basicDetails[1];
            manageYearGroupDetail.DisplayOrder = basicDetails[2];

            //Input active history with 'Start Date' and 'End Date'
            manageYearGroupDetail.AddActivehistory();
            manageYearGroupDetail.ActiveHistory[0].StartDate = activeHistoryDetails[0];
            manageYearGroupDetail.ActiveHistory[0].EndDate = activeHistoryDetails[1];

            //Scroll to Associated Groups
            manageYearGroupDetail.ScrollToAssociatedGroup();

            //Input information to Associated Groups
            manageYearGroupDetail.CurriculumYear = curriculumYear;

            DataPackage dataPackage = this.BuildDataPackage();

            //Create School NC Year
            //Create YearGroup and its set memberships
            var classId = Guid.NewGuid();
            var classShortName = Utilities.GenerateRandomString(3, "SNADD");
            var classFullName = Utilities.GenerateRandomString(10, "FNADD");
            dataPackage.AddClasses(classId, classShortName, classFullName, 1);

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
                manageYearGroupDetail.AddClass();
                manageYearGroupDetail.Classes[0].Class = classFullName;
                manageYearGroupDetail.Classes[0].StartDate = classDetails[1];
                manageYearGroupDetail.Classes[0].EndDate = classDetails[2];

                //Scroll to Staff Details
                manageYearGroupDetail.ScrollToStaffDetails();
                manageYearGroupDetail.AddHeadOfYear();

                //Input information to Head of Year grid
                var headstaff = string.Concat(surname, ", ", forename);
                manageYearGroupDetail.HeadOfYear[0].HeadOfYear = headstaff;
                manageYearGroupDetail.HeadOfYear[0].StartDate = headOfYearDetails[1];
                manageYearGroupDetail.HeadOfYear[0].EndDate = headOfYearDetails[2];

                //Input information to Staff grid
                manageYearGroupDetail.AddStaff();
                manageYearGroupDetail.Staff[0].Staff = headstaff;
                manageYearGroupDetail.Staff[0].PastoralRole = staffDetails[1];
                manageYearGroupDetail.Staff[0].StartDate = staffDetails[2];
                manageYearGroupDetail.Staff[0].EndDate = staffDetails[3];

                //Click Save
                manageYearGroupDetail.Save();

                //Confirm success message appears
                Assert.AreEqual(true, manageYearGroupDetail.IsMessageSuccessAppear(), "Success message does not appear");

                //Re-select year group from search results to confirm Year Group was added successfully

                // Search new class to verify that data was saved successfully
                manageYearGroupDetail.Refresh();
                manageYearGroupTriplet.SearchCriteria.AcademicYear = futureAcademicYear;
                var yearGroupResults = manageYearGroupTriplet.SearchCriteria.Search();
                var yearTile = yearGroupResults.SingleOrDefault(t => t.FullName.Equals(basicDetails[0]));
                manageYearGroupDetail = yearTile.Click<ManageYearGroupsPage>();

                //Confirm basic information was updated successfully
                Assert.AreEqual(basicDetails[0], manageYearGroupDetail.FullName, "Full name is not correct");
                Assert.AreEqual(basicDetails[1], manageYearGroupDetail.ShortName, "Short name is not correct");
                Assert.AreEqual(basicDetails[2], manageYearGroupDetail.DisplayOrder, "Display order is not correct");

                //Confirm 'Active History' information was updated successfully 
                Assert.AreEqual(activeHistoryDetails[0], manageYearGroupDetail.ActiveHistory[0].StartDate,
                    "'Start Date' in 'Active History' Grid is not correct");
                Assert.AreEqual(activeHistoryDetails[1], manageYearGroupDetail.ActiveHistory[0].EndDate,
                    "'End Date' in 'Active History' Grid is not correct");

                //Scroll to Associated Groups
                manageYearGroupDetail.ScrollToAssociatedGroup();

                //Confirm information in Associated Groups was updated successfully
                Assert.AreEqual(curriculumYear, manageYearGroupDetail.CurriculumYear,
                    "'Curriculum Year' in Associated Groups is not correct");
                Assert.AreEqual(classFullName, manageYearGroupDetail.Classes[0].Class,
                    "'Class' in Associated Groups is not correct");
                Assert.AreEqual(classDetails[1], manageYearGroupDetail.Classes[0].StartDate,
                    "'Start Date' in Associated Groups is not correct");
                Assert.AreEqual(classDetails[2], manageYearGroupDetail.Classes[0].EndDate,
                    "'End Date' in Associated Groups is not correct");

                //Scroll to Staff Details
                manageYearGroupDetail.ScrollToStaffDetails();

                //Confirm information in  Head of Year grid was updated successfully
                Assert.AreEqual(headstaff, manageYearGroupDetail.HeadOfYear[0].HeadOfYear,
                    "'Head Of Year' in Staff grid is not correct");
                Assert.AreEqual(headOfYearDetails[1], manageYearGroupDetail.HeadOfYear[0].StartDate,
                    "'Start Date' in Staff grid is not correct");
                Assert.AreEqual(headOfYearDetails[2], manageYearGroupDetail.HeadOfYear[0].EndDate,
                    "'End Date' in Staff grid is not correct");

                //Confirm information in Staff grid
                Assert.AreEqual(headstaff, manageYearGroupDetail.Staff[0].Staff,
                    "'Staff' in Staff grid is not correct");
                Assert.AreEqual(staffDetails[1], manageYearGroupDetail.Staff[0].PastoralRole,
                    "'Pastoral Role' in Staff grid is not correct");
                Assert.AreEqual(staffDetails[2], manageYearGroupDetail.Staff[0].StartDate,
                    "'Start Date' in Staff grid is not correct");
                Assert.AreEqual(staffDetails[3], manageYearGroupDetail.Staff[0].EndDate,
                    "'End Date' in Staff grid is not correct");

                //Confirm the create new year group reflected on the Search Options on Screens 'Promote Pupils'
                SeleniumHelper.NavigateMenu("Tasks", "School Groups", "Promote Pupils");
                Wait.WaitForDocumentReady();
                var promotePupilTriplet = new PromotePupilsTriplet();
                var yearGroup = promotePupilTriplet.SearchCriteria.YearGroups;
                var yearItem = yearGroup[basicDetails[0]];
                Assert.AreNotEqual(null, yearItem, "The Year Group does not display in Promote Pupil page");

                //Confirm the create new year group reflected on the Search Options on Screens 'Allocate Future Pupils'
                SeleniumHelper.NavigateMenu("Tasks", "School Groups", "Allocate Future Pupils");
                Wait.WaitForDocumentReady();
                var newIntake = new AllocateFuturePupilsTriplet();
                yearGroup = newIntake.SearchCriteria.YearGroups;
                yearItem = yearGroup[basicDetails[0]];

                // Bug or question: future Academic is not displayed in Allocate Future Pupils
                Assert.AreNotEqual(null, yearItem, "The Year Group does not display in Allocate Future Pupils");

                //Confirm the create new year group reflected on the Search Options on Screens 'Allocate Pupils to Groups'
                SeleniumHelper.NavigateMenu("Tasks", "School Groups", "Allocate Pupils to Groups");
                Wait.WaitForDocumentReady();
                var allocatePupilToGroup = new AllocatePupilsToGroupsTriplet();
                yearGroup = allocatePupilToGroup.SearchCriteria.YearGroups;
                yearItem = yearGroup[basicDetails[0]];

                // Bug or question: future Academic is not displayed in Allocate Future Pupils
                Assert.AreNotEqual(null, yearItem, "The year group does not display in Allocate Pupils to Groups page");

            #endregion

                #region POS-CONDITION

                //Back to Year group details page to delete year group was added
                SeleniumHelper.NavigateMenu("Tasks", "School Groups", "Manage Year Groups");
                Wait.WaitForDocumentReady();
                manageYearGroupTriplet = new ManageYearGroupsTriplet();
                manageYearGroupTriplet.SearchCriteria.AcademicYear = futureAcademicYear;
                var yearGroupListResult = manageYearGroupTriplet.SearchCriteria.Search();
                var yearGroupResult = yearGroupListResult.FirstOrDefault(p => p.FullName.Equals(basicDetails[0]));
                manageYearGroupDetail = yearGroupResult.Click<ManageYearGroupsPage>();

                //Remove the records in Staff Details section
                manageYearGroupDetail.ScrollToStaffDetails();
                manageYearGroupDetail.Staff[0].DeleteRow();
                manageYearGroupDetail.HeadOfYear[0].DeleteRow();

                //Remove the records in Associated Groups section
                manageYearGroupDetail.ScrollToAssociatedGroup();
                manageYearGroupDetail.Classes[0].DeleteRow();

                //Save the changes
                manageYearGroupDetail.Save();

                //Delete Year Group
                manageYearGroupDetail.Delete();

                #endregion POS-CONDITION
            }
        }

        /// <summary>
        /// Author: BaTruong
        /// Description: School Administrator Amend the Year Group created in the previous test with an Active History starting in the Next Academic Year 
        ///              and associate with the new Year Groups created for the next academic year.
        ///              Including the amendment of the staff allocated as Head of Year, Classes, Staff and Year Group Description Information
        ///                         
        /// </summary>
        [WebDriverTest(TimeoutSeconds = 3000, Enabled = false, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { "g1" }, DataProvider = "TC_YG002_Data")]
        public void TC_YG002_Amend_The_Year_Group_With_An_Active_History_Starting_In_The_Next_Academic_Year(string[] basicDetails, string[] activeHistoryDetails, string curriculumYear,
                                                                                                            string[] updateBasicDetails, string[] updateActiveHistoryDetails, string updateCurriculumYear,
                                                                                                            string[] updateClassDetails, string[] updateHeadOfYearDetails, string[] updateStaffDetails,
                                                                                                            string futureAcademicYear)
        {
            #region PRE-CONDITION

            //Login and navigate to Manage Year Groups
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);
            Wait.WaitForDocumentReady();
            SeleniumHelper.NavigateMenu("Tasks", "School Groups", "Manage Year Groups");
            Wait.WaitForDocumentReady();

            //Select Academic Year by next academic
            var manageYearGroupTriplet = new ManageYearGroupsTriplet();
            Wait.WaitForDocumentReady();
            manageYearGroupTriplet.SearchCriteria.AcademicYear = futureAcademicYear;

            //Add new Year Group for next academic
            var manageYearGroupDetail = manageYearGroupTriplet.AddYearGroup();

            //Input basic information
            manageYearGroupDetail.FullName = basicDetails[0];
            manageYearGroupDetail.ShortName = basicDetails[1];
            manageYearGroupDetail.DisplayOrder = basicDetails[2];

            //Input active history with 'Start Date' and 'End Date'
            manageYearGroupDetail.AddActivehistory();
            manageYearGroupDetail.ActiveHistory[0].StartDate = activeHistoryDetails[0];
            manageYearGroupDetail.ActiveHistory[0].EndDate = activeHistoryDetails[1];

            //Scroll to Associated Groups
            manageYearGroupDetail.ScrollToAssociatedGroup();

            //Input information to Associated Groups
            manageYearGroupDetail.CurriculumYear = curriculumYear;

            //Click Save
            manageYearGroupDetail.Save();

            #endregion

            #region TEST STEPS

            //Select year group was added at pre-condition
            manageYearGroupTriplet.Refresh();
            var yearGroupTile = manageYearGroupTriplet.SearchCriteria.Search().FirstOrDefault(x => x.FullName.Equals(basicDetails[0]));
            manageYearGroupDetail = yearGroupTile.Click<ManageYearGroupsPage>();

            //Amend basic informations
            manageYearGroupDetail.FullName = updateBasicDetails[0];
            manageYearGroupDetail.ShortName = updateBasicDetails[1];
            manageYearGroupDetail.DisplayOrder = updateBasicDetails[2];

            //Amend active history with 'Start Date' and 'End Date'
            manageYearGroupDetail.ActiveHistory[0].StartDate = updateActiveHistoryDetails[0];
            manageYearGroupDetail.ActiveHistory[0].EndDate = updateActiveHistoryDetails[1];

            //Scroll to Associated Groups
            manageYearGroupDetail.ScrollToAssociatedGroup();

            DataPackage dataPackage = this.BuildDataPackage();

            //Create School NC Year
            //Create YearGroup and its set memberships
            var classId = Guid.NewGuid();
            var classShortName = Utilities.GenerateRandomString(3, "SNADD");
            var classFullName = Utilities.GenerateRandomString(10, "FNADD");
            dataPackage.AddClasses(classId, classShortName, classFullName, 1);

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
                manageYearGroupDetail.AddClass();

                //Amend information in Associated Groups
                manageYearGroupDetail.CurriculumYear = updateCurriculumYear;
                manageYearGroupDetail.Classes[0].Class = classFullName;
                manageYearGroupDetail.Classes[0].StartDate = updateClassDetails[1];
                manageYearGroupDetail.Classes[0].EndDate = updateClassDetails[2];

                //Scroll to Staff Details
                manageYearGroupDetail.ScrollToStaffDetails();
                manageYearGroupDetail.AddHeadOfYear();
                var headstaff = string.Concat(surname, ", ", forename);

                //Amend information in Head of Year grid
                manageYearGroupDetail.HeadOfYear[0].HeadOfYear = headstaff;
                manageYearGroupDetail.HeadOfYear[0].StartDate = updateHeadOfYearDetails[1];
                manageYearGroupDetail.HeadOfYear[0].EndDate = updateHeadOfYearDetails[2];

                //Amend information in Staff grid
                manageYearGroupDetail.AddStaff();
                manageYearGroupDetail.Staff[0].Staff = headstaff;
                manageYearGroupDetail.Staff[0].PastoralRole = updateStaffDetails[1];
                manageYearGroupDetail.Staff[0].StartDate = updateStaffDetails[2];
                manageYearGroupDetail.Staff[0].EndDate = updateStaffDetails[3];

                //Click Save
                manageYearGroupDetail.Save();

                //Confirmation the changes
                var confirmDialog = new ConfirmRequiredDialog();
                manageYearGroupTriplet = confirmDialog.ClickOK<ManageYearGroupsTriplet>();
                //Confirm success message appears
                //Assert.AreEqual(true, manageYearGroupDetail.IsMessageSuccessAppear(), "Success message does not appear");

                //Re-select year group from search results to confirm Year Group was updated successfully
                manageYearGroupTriplet.Refresh();
                manageYearGroupTriplet.SearchCriteria.AcademicYear = futureAcademicYear;
                yearGroupTile =
                    manageYearGroupTriplet.SearchCriteria.Search()
                        .FirstOrDefault(x => x.FullName.Equals(updateBasicDetails[0]));
                manageYearGroupDetail = yearGroupTile.Click<ManageYearGroupsPage>();

                //Confirm basic information was updated successfully
                Assert.AreEqual(updateBasicDetails[0], manageYearGroupDetail.FullName, "Full name was not updated");
                Assert.AreEqual(updateBasicDetails[1], manageYearGroupDetail.ShortName, "Short name was not updated");
                Assert.AreEqual(updateBasicDetails[2], manageYearGroupDetail.DisplayOrder,
                    "Display order was not updated");

                //Confirm 'Active History' information was updated successfully 
                Assert.AreEqual(updateActiveHistoryDetails[0], manageYearGroupDetail.ActiveHistory[0].StartDate,
                    "'Start Date' in 'Active History' Grid was not updated");
                Assert.AreEqual(updateActiveHistoryDetails[1], manageYearGroupDetail.ActiveHistory[0].EndDate,
                    "'End Date' in 'Active History' Grid was not updated");

                //Scroll to Associated Groups
                manageYearGroupDetail.ScrollToAssociatedGroup();

                //Confirm information in Associated Groups was updated successfully
                Assert.AreEqual(updateCurriculumYear, manageYearGroupDetail.CurriculumYear,
                    "'Curriculum Year' in Associated Groups was not updated");
                Assert.AreEqual(classFullName, manageYearGroupDetail.Classes[0].Class,
                    "'Class' in Associated Groups was not updated");
                Assert.AreEqual(updateClassDetails[1], manageYearGroupDetail.Classes[0].StartDate,
                    "'Start Date' in Associated Groups was not updated");
                Assert.AreEqual(updateClassDetails[2], manageYearGroupDetail.Classes[0].EndDate,
                    "'End Date' in Associated Groups was not updated");

                //Scroll to Staff Details
                manageYearGroupDetail.ScrollToStaffDetails();

                //Confirm information in  Head of Year grid was updated successfully
                Assert.AreEqual(headstaff, manageYearGroupDetail.HeadOfYear[0].HeadOfYear,
                    "'Head Of Year' in Staff grid was not updated");
                Assert.AreEqual(updateHeadOfYearDetails[1], manageYearGroupDetail.HeadOfYear[0].StartDate,
                    "'Start Date' in Staff grid was not updated");
                Assert.AreEqual(updateHeadOfYearDetails[2], manageYearGroupDetail.HeadOfYear[0].EndDate,
                    "'End Date' in Staff grid was not updated");

                //Confirm information in Staff grid
                Assert.AreEqual(headstaff, manageYearGroupDetail.Staff[0].Staff,
                    "'Staff' in Staff grid was not updated");
                Assert.AreEqual(updateStaffDetails[1], manageYearGroupDetail.Staff[0].PastoralRole,
                    "'Pastoral Role' in Staff grid was not updated");
                Assert.AreEqual(updateStaffDetails[2], manageYearGroupDetail.Staff[0].StartDate,
                    "'Start Date' in Staff grid was not updated");
                Assert.AreEqual(updateStaffDetails[3], manageYearGroupDetail.Staff[0].EndDate,
                    "'End Date' in Staff grid was not updated");

                //Confirm the changes of year group reflected on the Search Options on Screens 'Promote Pupils'
                SeleniumHelper.NavigateMenu("Tasks", "School Groups", "Promote Pupils");
                Wait.WaitForDocumentReady();
                PromotePupilsTriplet promotePupilTriplet = new PromotePupilsTriplet();
                var yearGroups = promotePupilTriplet.SearchCriteria.YearGroups;
                var yearGroup = yearGroups[updateBasicDetails[0]];
                Assert.AreNotEqual(null, yearGroup, "The year group displays on 'Promote Pupil' page");

                //Confirm the changes of year group reflected on the Search Options on Screens 'Allocate New Intake'
                SeleniumHelper.NavigateMenu("Tasks", "School Groups", "Allocate Future Pupils");
                Wait.WaitForDocumentReady();
                var newIntake = new AllocateFuturePupilsTriplet();
                yearGroups = newIntake.SearchCriteria.YearGroups;
                yearGroup = yearGroups[updateBasicDetails[0]];
                Assert.AreNotEqual(null, yearGroup, "The year group displays on 'Allocate New Intake' page");

                //Confirm the changes of year group reflected on the Search Options on Screens 'Allocate Pupils to Groups'
                SeleniumHelper.NavigateMenu("Tasks", "School Groups", "Allocate Pupils To Groups");
                Wait.WaitForDocumentReady();
                var allocatePupilToGroup = new AllocatePupilsToGroupsTriplet();
                yearGroups = allocatePupilToGroup.SearchCriteria.YearGroups;
                yearGroup = yearGroups[updateBasicDetails[0]];
                Assert.AreNotEqual(null, yearGroup, "The year group displays on 'Allocate Pupils To Groups' page");

            #endregion

                #region POS-CONDITION

                //Back to Year group details page to delete year group was added
                SeleniumHelper.NavigateMenu("Tasks", "School Groups", "Manage Year Groups");
                Wait.WaitForDocumentReady();
                manageYearGroupTriplet = new ManageYearGroupsTriplet();
                manageYearGroupTriplet.SearchCriteria.AcademicYear = futureAcademicYear;
                var yearGroupListResult = manageYearGroupTriplet.SearchCriteria.Search();
                var yearGroupResult = yearGroupListResult.FirstOrDefault(p => p.FullName.Equals(updateBasicDetails[0]));
                manageYearGroupDetail = yearGroupResult.Click<ManageYearGroupsPage>();

                //Remove the records in Staff Details section
                manageYearGroupDetail.ScrollToStaffDetails();
                manageYearGroupDetail.Staff[0].DeleteRow();
                manageYearGroupDetail.HeadOfYear[0].DeleteRow();

                //Remove the records in Associated Groups section
                manageYearGroupDetail.ScrollToAssociatedGroup();
                manageYearGroupDetail.Classes[0].DeleteRow();

                //Save the changes
                manageYearGroupDetail.Save();

                //Delete the year group was added
                manageYearGroupDetail.Delete();

                #endregion
            }
        }

        /// <summary>
        /// Author: BaTruong
        /// Description: Create new Year Group  and associate to the current Academic Year 
        ///              with an active History Starting in the current Academic Year but with an Open End Date (null)
        /// </summary>
        [WebDriverTest(TimeoutSeconds = 2000, Enabled = false, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { "g1" }, DataProvider = "TC_YG003_Data")]
        public void TC_YG003_Create_Year_Groups_With_An_Active_History_Starting_In_The_Current_Academic_Year_With_Open_Ended_Active_History(string[] basicDetails, string[] activeHistoryDetails, string curriculumYear,
                                                                                                             string[] classDetails, string[] headOfYearDetails, string[] staffDetails,
                                                                                                             string currentAcademicYear)
        {
            #region TEST STEPS

            //Login as School Administrator and navigate to Manage Year Groups
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);
            Wait.WaitForDocumentReady();
            SeleniumHelper.NavigateMenu("Tasks", "School Groups", "Manage Year Groups");
            Wait.WaitForDocumentReady();

            //Select Academic Year by current academic
            var manageYearGroupTriplet = new ManageYearGroupsTriplet();
            Wait.WaitForDocumentReady();
            manageYearGroupTriplet.SearchCriteria.AcademicYear = currentAcademicYear;

            //Add new Year Group for current academic
            var manageYearGroupDetail = manageYearGroupTriplet.AddYearGroup();

            //Input basic information
            manageYearGroupDetail.FullName = basicDetails[0];
            manageYearGroupDetail.ShortName = basicDetails[1];
            manageYearGroupDetail.DisplayOrder = basicDetails[2];

            //Input active history with 'Start Date' and 'End Date' is empty
            manageYearGroupDetail.AddActivehistory();
            manageYearGroupDetail.ActiveHistory[0].StartDate = activeHistoryDetails[0];

            //Scroll to Associated Groups
            manageYearGroupDetail.ScrollToAssociatedGroup();

            //Input information to Associated Groups with null value into 'End Date'
            manageYearGroupDetail.CurriculumYear = curriculumYear;

            DataPackage dataPackage = this.BuildDataPackage();

            //Create School NC Year
            //Create YearGroup and its set memberships
            var classId = Guid.NewGuid();
            var classShortName = Utilities.GenerateRandomString(3, "SNADD");
            var classFullName = Utilities.GenerateRandomString(10, "FNADD");
            dataPackage.AddClasses(classId, classShortName, classFullName, 1);

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
                manageYearGroupDetail.AddClass();
                manageYearGroupDetail.Classes[0].Class = classFullName;
                manageYearGroupDetail.Classes[0].StartDate = classDetails[1];


                //Scroll to Staff Details
                manageYearGroupDetail.ScrollToStaffDetails();
                manageYearGroupDetail.AddHeadOfYear();
                var headstaff = string.Concat(surname, ", ", forename);
                //Input information to Head of Year grid with null value into 'End Date'
                manageYearGroupDetail.HeadOfYear[0].HeadOfYear = headstaff;
                manageYearGroupDetail.HeadOfYear[0].StartDate = headOfYearDetails[1];

                //Input information to Staff grid with null value into 'End Date'
                manageYearGroupDetail.AddStaff();
                manageYearGroupDetail.Staff[0].Staff = headstaff;
                manageYearGroupDetail.Staff[0].PastoralRole = staffDetails[1];
                manageYearGroupDetail.Staff[0].StartDate = staffDetails[2];

                //Click Save
                manageYearGroupDetail.Save();

                //Confirm success message appears
                Assert.AreEqual(true, manageYearGroupDetail.IsMessageSuccessAppear(), "Success message does not appear");

                //Re-select year group from search results to confirm Year Group was added successfully
                manageYearGroupDetail.Refresh();
                var yearGroupTile =
                    manageYearGroupTriplet.SearchCriteria.Search()
                        .FirstOrDefault(x => x.FullName.Equals(basicDetails[0]));
                manageYearGroupDetail = yearGroupTile.Click<ManageYearGroupsPage>();

                //Confirm basic information was updated successfully
                Assert.AreEqual(basicDetails[0], manageYearGroupDetail.FullName, "Full name is not correct");
                Assert.AreEqual(basicDetails[1], manageYearGroupDetail.ShortName, "Short name is not correct");
                Assert.AreEqual(basicDetails[2], manageYearGroupDetail.DisplayOrder, "Display order is not correct");

                //Confirm 'Active History' information was updated successfully 
                Assert.AreEqual(activeHistoryDetails[0], manageYearGroupDetail.ActiveHistory[0].StartDate,
                    "'Start Date' in 'Active History' Grid is not correct");
                Assert.AreEqual(true, String.IsNullOrEmpty(manageYearGroupDetail.ActiveHistory[0].EndDate),
                    "'End Date' in 'Active History' Grid is not correct");

                //Scroll to Associated Groups
                manageYearGroupDetail.ScrollToAssociatedGroup();

                //Confirm information in Associated Groups was updated successfully
                Assert.AreEqual(curriculumYear, manageYearGroupDetail.CurriculumYear,
                    "'Curriculum Year' in Associated Groups is not correct");
                Assert.AreEqual(classFullName, manageYearGroupDetail.Classes[0].Class,
                    "'Class' in Associated Groups is not correct");
                Assert.AreEqual(classDetails[1], manageYearGroupDetail.Classes[0].StartDate,
                    "'Start Date' in Associated Groups is not correct");
                Assert.AreEqual(true, String.IsNullOrEmpty(manageYearGroupDetail.Classes[0].EndDate),
                    "'End Date' in Associated Groups is not correct");

                //Scroll to Staff Details
                manageYearGroupDetail.ScrollToStaffDetails();

                //Confirm information in  Head of Year grid was updated successfully
                Assert.AreEqual(headstaff, manageYearGroupDetail.HeadOfYear[0].HeadOfYear,
                    "'Head Of Year' in Staff grid is not correct");
                Assert.AreEqual(headOfYearDetails[1], manageYearGroupDetail.HeadOfYear[0].StartDate,
                    "'Start Date' in Staff grid is not correct");
                Assert.AreEqual(true, String.IsNullOrEmpty(manageYearGroupDetail.HeadOfYear[0].EndDate),
                    "'End Date' in Staff grid is not correct");

                //Confirm information in Staff grid
                Assert.AreEqual(headstaff, manageYearGroupDetail.Staff[0].Staff,
                    "'Staff' in Staff grid is not correct");
                Assert.AreEqual(staffDetails[1], manageYearGroupDetail.Staff[0].PastoralRole,
                    "'Pastoral Role' in Staff grid is not correct");
                Assert.AreEqual(staffDetails[2], manageYearGroupDetail.Staff[0].StartDate,
                    "'Start Date' in Staff grid is not correct");
                Assert.AreEqual(true, String.IsNullOrEmpty(manageYearGroupDetail.Staff[0].EndDate),
                    "'End Date' in Staff grid is not correct");

                //Confirm year group has just created appears on the Search Options on Screens 'Promote Pupils'
                SeleniumHelper.NavigateMenu("Tasks", "School Groups", "Promote Pupils");
                Wait.WaitForDocumentReady();
                PromotePupilsTriplet promotePupilTriplet = new PromotePupilsTriplet();
                var yearGroups = promotePupilTriplet.SearchCriteria.YearGroups;
                var yearGroup = yearGroups[basicDetails[0]];
                Assert.AreNotEqual(null, yearGroup, "The year group does not display in Promote Pupil page");

                //Confirm year group has just created appears on the Search Options on Screens 'Allocate New Intake'
                SeleniumHelper.NavigateMenu("Tasks", "School Groups", "Allocate Future Pupils");
                Wait.WaitForDocumentReady();
                var newIntake = new AllocateFuturePupilsTriplet();
                yearGroups = newIntake.SearchCriteria.YearGroups;
                yearGroup = yearGroups[basicDetails[0]];
                Assert.AreNotEqual(null, yearGroup, "The year group does not display in Allocate New Intake page");

                //Confirm year group has just created appears on the Search Options on Screens 'Allocate Pupils to Groups'
                SeleniumHelper.NavigateMenu("Tasks", "School Groups", "Allocate Pupils To Groups");
                Wait.WaitForDocumentReady();
                var allocatePupilToGroup = new AllocatePupilsToGroupsTriplet();
                yearGroups = allocatePupilToGroup.SearchCriteria.YearGroups;
                yearGroup = yearGroups[basicDetails[0]];
                Assert.AreNotEqual(null, yearGroup, "The year group does not display in Allocate Pupils to Groups page");

            #endregion

                #region POS-CONDITION

                //Back to Year group details page to delete year group was added
                SeleniumHelper.NavigateMenu("Tasks", "School Groups", "Manage Year Groups");
                manageYearGroupTriplet = new ManageYearGroupsTriplet();
                manageYearGroupTriplet.SearchCriteria.AcademicYear = currentAcademicYear;
                var yearGroupListResult = manageYearGroupTriplet.SearchCriteria.Search();
                var yearGroupResult = yearGroupListResult.FirstOrDefault(p => p.FullName.Equals(basicDetails[0]));
                manageYearGroupDetail = yearGroupResult.Click<ManageYearGroupsPage>();

                //Remove the records in Staff Details section
                manageYearGroupDetail.ScrollToStaffDetails();
                manageYearGroupDetail.Staff[0].DeleteRow();
                manageYearGroupDetail.HeadOfYear[0].DeleteRow();

                //Remove the records in Associated Groups section
                manageYearGroupDetail.ScrollToAssociatedGroup();
                manageYearGroupDetail.Classes[0].DeleteRow();

                //Save the changes
                manageYearGroupDetail.Save();

                //Delete the year group was added
                manageYearGroupDetail.Delete();

                #endregion
            }
        }

        /// <summary>
        /// Author: BaTruong
        /// Description: School Administrator Amend a Year Group created in the previous test for the Current Academic Year 
        ///              with an Open Ended Active History date and end it in a future Academic Year.
        ///              Including the amendment of the staff allocated as Head of Year, Classes, Staff and Year Group Description Information
        /// </summary>
        [WebDriverTest(TimeoutSeconds = 2000, Enabled = false, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { "g1" }, DataProvider = "TC_YG004_Data")]
        public void TC_YG004_Amend_The_Year_Group_With_An_Active_History_Starting_In_The_Current_Academic_Year_With_Open_Ended_Active_History(string[] basicDetails,
                                                                                                            string[] activeHistoryDetails, string curriculumYear,
                                                                                                            string[] updateBasicDetails, string[] updateActiveHistoryDetails, string updateCurriculumYear,
                                                                                                            string[] updateClassDetails, string[] updateHeadOfYearDetails, string[] updateStaffDetails,
                                                                                                            string currentAcademicYear)
        {
            #region PRE-CONDITION

            //Login as School Administrator and navigate to Manage Year Groups
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);
            Wait.WaitForDocumentReady();
            SeleniumHelper.NavigateMenu("Tasks", "School Groups", "Manage Year Groups");
            Wait.WaitForDocumentReady();

            //Select Academic Year by current academic
            var manageYearGroupTriplet = new ManageYearGroupsTriplet();
            Wait.WaitForDocumentReady();
            manageYearGroupTriplet.SearchCriteria.AcademicYear = currentAcademicYear;

            //Add new Year Group for current academic
            var manageYearGroupDetail = manageYearGroupTriplet.AddYearGroup();

            //Input basic information
            manageYearGroupDetail.FullName = basicDetails[0];
            manageYearGroupDetail.ShortName = basicDetails[1];
            manageYearGroupDetail.DisplayOrder = basicDetails[2];

            //Input active history with 'Start Date' and 'End Date' is empty
            manageYearGroupDetail.AddActivehistory();
            manageYearGroupDetail.ActiveHistory[0].StartDate = activeHistoryDetails[0];

            //Scroll to Associated Groups
            manageYearGroupDetail.ScrollToAssociatedGroup();

            //Input information to Associated Groups with null value into 'End Date'
            manageYearGroupDetail.CurriculumYear = curriculumYear;

            //Click Save
            manageYearGroupDetail.Save();

            #endregion

            #region TEST STEPS

            //Select year group was added at pre-condition
            manageYearGroupTriplet.Refresh();
            var yearGroupTile = manageYearGroupTriplet.SearchCriteria.Search().FirstOrDefault(x => x.FullName.Equals(basicDetails[0]));
            manageYearGroupDetail = yearGroupTile.Click<ManageYearGroupsPage>();

            //Amend basic informations
            manageYearGroupDetail.FullName = updateBasicDetails[0];
            manageYearGroupDetail.ShortName = updateBasicDetails[1];
            manageYearGroupDetail.DisplayOrder = updateBasicDetails[2];

            //Amend active history with valid 'Start Date' and 'End Date' is in future
            manageYearGroupDetail.ActiveHistory[0].StartDate = updateActiveHistoryDetails[0];
            manageYearGroupDetail.ActiveHistory[0].EndDate = updateActiveHistoryDetails[1];

            //Scroll to Associated Groups
            manageYearGroupDetail.ScrollToAssociatedGroup();

            //Amend information in Associated Groups with valid 'Start Date' and 'End Date' is in future
            manageYearGroupDetail.CurriculumYear = updateCurriculumYear;

            DataPackage dataPackage = this.BuildDataPackage();

            //Create School NC Year
            //Create YearGroup and its set memberships
            var classId = Guid.NewGuid();
            var classShortName = Utilities.GenerateRandomString(3, "SNADD");
            var classFullName = Utilities.GenerateRandomString(10, "FNADD");
            dataPackage.AddClasses(classId, classShortName, classFullName, 1);

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
                manageYearGroupDetail.AddClass();
                manageYearGroupDetail.Classes[0].Class = classFullName;
                manageYearGroupDetail.Classes[0].StartDate = updateClassDetails[1];
                manageYearGroupDetail.Classes[0].EndDate = updateClassDetails[2];

                //Scroll to Staff Details
                manageYearGroupDetail.ScrollToStaffDetails();
                manageYearGroupDetail.AddHeadOfYear();
                var headstaff = string.Concat(surname, ", ", forename);

                //Amend information in Head of Year grid with valid 'Start Date' and 'End Date' is in future
                manageYearGroupDetail.HeadOfYear[0].HeadOfYear = headstaff;
                manageYearGroupDetail.HeadOfYear[0].StartDate = updateHeadOfYearDetails[1];
                manageYearGroupDetail.HeadOfYear[0].EndDate = updateHeadOfYearDetails[2];

                //Amend information in Staff grid with valid 'Start Date' and 'End Date' is in future
                manageYearGroupDetail.AddStaff();
                manageYearGroupDetail.Staff[0].Staff = headstaff;
                manageYearGroupDetail.Staff[0].PastoralRole = updateStaffDetails[1];
                manageYearGroupDetail.Staff[0].StartDate = updateStaffDetails[2];
                manageYearGroupDetail.Staff[0].EndDate = updateStaffDetails[3];

                //Click Save
                manageYearGroupDetail.Save();

                //Confirmation the changes
                var confirmDialog = new ConfirmRequiredDialog();
                manageYearGroupTriplet = confirmDialog.ClickOK<ManageYearGroupsTriplet>();

                //Confirm success message appears
                manageYearGroupDetail.Refresh();

                //Re-select year group from search results to confirm Year Group was updated successfully
                manageYearGroupTriplet.Refresh();
                manageYearGroupTriplet.SearchCriteria.AcademicYear = currentAcademicYear;
                yearGroupTile =
                    manageYearGroupTriplet.SearchCriteria.Search()
                        .FirstOrDefault(x => x.FullName.Equals(updateBasicDetails[0]));
                manageYearGroupDetail = yearGroupTile.Click<ManageYearGroupsPage>();

                //Confirm basic information was updated successfully
                Assert.AreEqual(updateBasicDetails[0], manageYearGroupDetail.FullName, "Full name was not updated");
                Assert.AreEqual(updateBasicDetails[1], manageYearGroupDetail.ShortName, "Short name was not updated");
                Assert.AreEqual(updateBasicDetails[2], manageYearGroupDetail.DisplayOrder,
                    "Display order was not updated");

                //Confirm 'Active History' information was updated successfully 
                Assert.AreEqual(updateActiveHistoryDetails[0], manageYearGroupDetail.ActiveHistory[0].StartDate,
                    "'Start Date' in 'Active History' Grid was not updated");
                Assert.AreEqual(updateActiveHistoryDetails[1], manageYearGroupDetail.ActiveHistory[0].EndDate,
                    "'End Date' in 'Active History' Grid was not updated");

                //Scroll to Associated Groups
                manageYearGroupDetail.ScrollToAssociatedGroup();

                //Confirm information in Associated Groups was updated successfully
                Assert.AreEqual(updateCurriculumYear, manageYearGroupDetail.CurriculumYear,
                    "'Curriculum Year' in Associated Groups was not updated");
                Assert.AreEqual(classFullName, manageYearGroupDetail.Classes[0].Class,
                    "'Class' in Associated Groups was not updated");
                Assert.AreEqual(updateClassDetails[1], manageYearGroupDetail.Classes[0].StartDate,
                    "'Start Date' in Associated Groups was not updated");
                Assert.AreEqual(updateClassDetails[2], manageYearGroupDetail.Classes[0].EndDate,
                    "'End Date' in Associated Groups was not updated");

                //Scroll to Staff Details
                manageYearGroupDetail.ScrollToStaffDetails();

                //Confirm information in  Head of Year grid was updated successfully
                Assert.AreEqual(headstaff, manageYearGroupDetail.HeadOfYear[0].HeadOfYear,
                    "'Head Of Year' in Staff grid was not updated");
                Assert.AreEqual(updateHeadOfYearDetails[1], manageYearGroupDetail.HeadOfYear[0].StartDate,
                    "'Start Date' in Staff grid was not updated");
                Assert.AreEqual(updateHeadOfYearDetails[2], manageYearGroupDetail.HeadOfYear[0].EndDate,
                    "'End Date' in Staff grid was not updated");

                //Confirm information in Staff grid
                Assert.AreEqual(headstaff, manageYearGroupDetail.Staff[0].Staff,
                    "'Staff' in Staff grid was not updated");
                Assert.AreEqual(updateStaffDetails[1], manageYearGroupDetail.Staff[0].PastoralRole,
                    "'Pastoral Role' in Staff grid was not updated");
                Assert.AreEqual(updateStaffDetails[2], manageYearGroupDetail.Staff[0].StartDate,
                    "'Start Date' in Staff grid was not updated");
                Assert.AreEqual(updateStaffDetails[3], manageYearGroupDetail.Staff[0].EndDate,
                    "'End Date' in Staff grid was not updated");

                //Confirm the changes of year group reflected on the Search Options on Screens 'Promote Pupils'
                SeleniumHelper.NavigateMenu("Tasks", "School Groups", "Promote Pupils");
                Wait.WaitForDocumentReady();
                PromotePupilsTriplet promotePupilTriplet = new PromotePupilsTriplet();
                var yearGroups = promotePupilTriplet.SearchCriteria.YearGroups;
                var yearGroup = yearGroups[updateBasicDetails[0]];
                Assert.AreNotEqual(null, yearGroup, "The year group does not display in Promote Pupil page");

                //Confirm the changes of year group reflected on the Search Options on Screens 'Allocate New Intake'
                SeleniumHelper.NavigateMenu("Tasks", "School Groups", "Allocate Future Pupils");
                Wait.WaitForDocumentReady();
                var newIntake = new AllocateFuturePupilsTriplet();
                yearGroups = newIntake.SearchCriteria.YearGroups;
                yearGroup = yearGroups[updateBasicDetails[0]];
                Assert.AreNotEqual(null, yearGroup, "The year group does not display in Allocate New Intake page");

                //Confirm the changes of year group reflected on the Search Options on Screens 'Allocate Pupils to Groups'
                SeleniumHelper.NavigateMenu("Tasks", "School Groups", "Allocate Pupils To Groups");
                Wait.WaitForDocumentReady();
                var allocatePupilToGroup = new AllocatePupilsToGroupsTriplet();
                yearGroups = allocatePupilToGroup.SearchCriteria.YearGroups;
                yearGroup = yearGroups[updateBasicDetails[0]];
                Assert.AreNotEqual(null, yearGroup, "The year group does not display in Allocate Pupils to Groups page");

            #endregion

                #region POS-CONDITION

                //Back to Year group details page to delete year group was added
                SeleniumHelper.NavigateMenu("Tasks", "School Groups", "Manage Year Groups");
                manageYearGroupTriplet = new ManageYearGroupsTriplet();
                manageYearGroupTriplet.SearchCriteria.AcademicYear = currentAcademicYear;
                var yearGroupListResult = manageYearGroupTriplet.SearchCriteria.Search();
                var yearGroupResult = yearGroupListResult.FirstOrDefault(p => p.FullName.Equals(updateBasicDetails[0]));
                manageYearGroupDetail = yearGroupResult.Click<ManageYearGroupsPage>();

                //Remove the records in Staff Details section
                manageYearGroupDetail.ScrollToStaffDetails();
                manageYearGroupDetail.Staff[0].DeleteRow();
                manageYearGroupDetail.HeadOfYear[0].DeleteRow();

                //Remove the records in Associated Groups section
                manageYearGroupDetail.ScrollToAssociatedGroup();
                manageYearGroupDetail.Classes[0].DeleteRow();

                //Save the changes
                manageYearGroupDetail.Save();

                //Delete the year group was added
                manageYearGroupDetail.Delete();

                #endregion
            }
        }

        /// <summary>
        /// Author: BaTruong
        /// Description: School Administrator Create new Year Group for the current academic year with an active History Starting and Ending in the current Academic Year
        /// </summary>
        [WebDriverTest(TimeoutSeconds = 2000, Enabled = true, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { "g1" }, DataProvider = "TC_YG005_Data")]
        public void TC_YG005_Create_Year_Groups_For_The_Current_Academic_With_Active_History_Starting_And_Ending_In_Current_Academic(string[] basicDetails,
                                                                                                          string[] activeHistoryDetails, string curriculumYear,
                                                                                                          string[] classDetails, string[] headOfYearDetails, string[] staffDetails,
                                                                                                          string currentAcademicYear)
        {
            #region TEST STEPS

            //Login as School Administrator and navigate to Manage Year Groups
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);
            Wait.WaitForDocumentReady();
            SeleniumHelper.NavigateMenu("Tasks", "School Groups", "Manage Year Groups");
            Wait.WaitForDocumentReady();

            //Select Academic Year by current academic
            var manageYearGroupTriplet = new ManageYearGroupsTriplet();
            Wait.WaitForDocumentReady();
            manageYearGroupTriplet.SearchCriteria.AcademicYear = currentAcademicYear;

            //Add new Year Group for current academic
            var manageYearGroupDetail = manageYearGroupTriplet.AddYearGroup();

            //Input basic information
            manageYearGroupDetail.FullName = basicDetails[0];
            manageYearGroupDetail.ShortName = basicDetails[1];
            manageYearGroupDetail.DisplayOrder = basicDetails[2];

            //Input active history with 'Start Date' and 'End Date'
            manageYearGroupDetail.AddActivehistory();
            manageYearGroupDetail.ActiveHistory[0].StartDate = activeHistoryDetails[0];
            manageYearGroupDetail.ActiveHistory[0].EndDate = activeHistoryDetails[1];

            //Scroll to Associated Groups
            manageYearGroupDetail.ScrollToAssociatedGroup();

            //Input information to Associated Groups
            manageYearGroupDetail.CurriculumYear = curriculumYear;

            DataPackage dataPackage = this.BuildDataPackage();

            var classId = Guid.NewGuid();
            var classShortName = Utilities.GenerateRandomString(3, "SNADD");
            var classFullName = Utilities.GenerateRandomString(10, "FNADD");
            dataPackage.AddClasses(classId, classShortName, classFullName, 1);

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
                manageYearGroupDetail.AddClass();
                manageYearGroupDetail.Classes[0].Class = classFullName;
                manageYearGroupDetail.Classes[0].StartDate = classDetails[1];
                manageYearGroupDetail.Classes[0].EndDate = classDetails[2];

                //Scroll to Staff Details
                manageYearGroupDetail.ScrollToStaffDetails();

                //Input information to Head of Year grid
                manageYearGroupDetail.AddHeadOfYear();
                var headstaff = string.Concat(surname, ", ", forename);
                manageYearGroupDetail.HeadOfYear[0].HeadOfYear = headstaff;
                manageYearGroupDetail.HeadOfYear[0].StartDate = headOfYearDetails[1];
                manageYearGroupDetail.HeadOfYear[0].EndDate = headOfYearDetails[2];

                //Input information to Staff grid
                manageYearGroupDetail.AddStaff();
                manageYearGroupDetail.Staff[0].Staff = headstaff;
                manageYearGroupDetail.Staff[0].PastoralRole = staffDetails[1];
                manageYearGroupDetail.Staff[0].StartDate = staffDetails[2];
                manageYearGroupDetail.Staff[0].EndDate = staffDetails[3];

                //Click Save
                manageYearGroupDetail.Save();

                //Confirm success message appears
                Assert.AreEqual(true, manageYearGroupDetail.IsMessageSuccessAppear(), "Success message does not appear");

                //Re-select year group from search results to confirm Year Group was added successfully
                manageYearGroupTriplet.Refresh();
                var yearGroupTile =
                    manageYearGroupTriplet.SearchCriteria.Search()
                        .FirstOrDefault(x => x.FullName.Equals(basicDetails[0]));
                manageYearGroupDetail = yearGroupTile.Click<ManageYearGroupsPage>();

                //Confirm basic information was updated successfully
                Assert.AreEqual(basicDetails[0], manageYearGroupDetail.FullName, "Full name is not correct");
                Assert.AreEqual(basicDetails[1], manageYearGroupDetail.ShortName, "Short name is not correct");
                Assert.AreEqual(basicDetails[2], manageYearGroupDetail.DisplayOrder, "Display order is not correct");

                //Confirm 'Active History' information was updated successfully 
                Assert.AreEqual(activeHistoryDetails[0], manageYearGroupDetail.ActiveHistory[0].StartDate,
                    "'Start Date' in 'Active History' Grid is not correct");
                Assert.AreEqual(activeHistoryDetails[1], manageYearGroupDetail.ActiveHistory[0].EndDate,
                    "'End Date' in 'Active History' Grid is not correct");

                //Scroll to Associated Groups
                manageYearGroupDetail.ScrollToAssociatedGroup();

                //Confirm information in Associated Groups was updated successfully
                Assert.AreEqual(curriculumYear, manageYearGroupDetail.CurriculumYear,
                    "'Curriculum Year' in Associated Groups is not correct");
                Assert.AreEqual(classFullName, manageYearGroupDetail.Classes[0].Class,
                    "'Class' in Associated Groups is not correct");
                Assert.AreEqual(classDetails[1], manageYearGroupDetail.Classes[0].StartDate,
                    "'Start Date' in Associated Groups is not correct");
                Assert.AreEqual(classDetails[2], manageYearGroupDetail.Classes[0].EndDate,
                    "'End Date' in Associated Groups is not correct");

                //Scroll to Staff Details
                manageYearGroupDetail.ScrollToStaffDetails();

                //Confirm information in  Head of Year grid was updated successfully
                Assert.AreEqual(headstaff, manageYearGroupDetail.HeadOfYear[0].HeadOfYear,
                    "'Head Of Year' in Staff grid is not correct");
                Assert.AreEqual(headOfYearDetails[1], manageYearGroupDetail.HeadOfYear[0].StartDate,
                    "'Start Date' in Staff grid is not correct");
                Assert.AreEqual(headOfYearDetails[2], manageYearGroupDetail.HeadOfYear[0].EndDate,
                    "'End Date' in Staff grid is not correct");

                //Confirm information in Staff grid
                Assert.AreEqual(headstaff, manageYearGroupDetail.Staff[0].Staff,
                    "'Staff' in Staff grid is not correct");
                Assert.AreEqual(staffDetails[1], manageYearGroupDetail.Staff[0].PastoralRole,
                    "'Pastoral Role' in Staff grid is not correct");
                Assert.AreEqual(staffDetails[2], manageYearGroupDetail.Staff[0].StartDate,
                    "'Start Date' in Staff grid is not correct");
                Assert.AreEqual(staffDetails[3], manageYearGroupDetail.Staff[0].EndDate,
                    "'End Date' in Staff grid is not correct");

                //Confirm year group has just created doesn't appears on the Search Options on Screens 'Promote Pupils'
                SeleniumHelper.NavigateMenu("Tasks", "School Groups", "Promote Pupils");
                Wait.WaitForDocumentReady();
                PromotePupilsTriplet promotePupilTriplet = new PromotePupilsTriplet();
                var yearGroups = promotePupilTriplet.SearchCriteria.YearGroups;
                var yearGroup = yearGroups[basicDetails[0]];
                Assert.AreNotEqual(null, yearGroup,
                    String.Format("The year group '{0}' displays on Promote Pupil page", basicDetails[0]));

                //Confirm year group has just created appears on the Search Options on Screens 'Allocate New Intake'
                SeleniumHelper.NavigateMenu("Tasks", "School Groups", "Allocate Future Pupils");
                Wait.WaitForDocumentReady();
                var newIntake = new AllocateFuturePupilsTriplet();
                yearGroups = newIntake.SearchCriteria.YearGroups;
                yearGroup = yearGroups[basicDetails[0]];
                Assert.AreNotEqual(null, yearGroup,
                    String.Format("The year group '{0}' does not display on Allocate New Intake page", basicDetails[0]));

                //Confirm year group has just created appears on the Search Options on Screens 'Allocate Pupils to Groups'
                SeleniumHelper.NavigateMenu("Tasks", "School Groups", "Allocate Pupils To Groups");
                Wait.WaitForDocumentReady();
                var allocatePupilToGroup = new AllocatePupilsToGroupsTriplet();
                yearGroups = allocatePupilToGroup.SearchCriteria.YearGroups;
                yearGroup = yearGroups[basicDetails[0]];
                Assert.AreNotEqual(null, yearGroup,
                    String.Format("The year group '{0}' does not display on Allocate Pupils to Groups page",
                        basicDetails[0]));

            #endregion

                #region POS-CONDITION

                //Back to Year group details page to delete year group was added
                SeleniumHelper.NavigateMenu("Tasks", "School Groups", "Manage Year Groups");
                manageYearGroupTriplet = new ManageYearGroupsTriplet();
                manageYearGroupTriplet.SearchCriteria.AcademicYear = currentAcademicYear;
                var yearGroupListResult = manageYearGroupTriplet.SearchCriteria.Search();
                var yearGroupResult = yearGroupListResult.FirstOrDefault(p => p.FullName.Equals(basicDetails[0]));
                manageYearGroupDetail = yearGroupResult.Click<ManageYearGroupsPage>();

                //Remove the records in Staff Details section
                manageYearGroupDetail.ScrollToStaffDetails();
                manageYearGroupDetail.Staff[0].DeleteRow();
                manageYearGroupDetail.HeadOfYear[0].DeleteRow();

                //Remove the records in Associated Groups section
                manageYearGroupDetail.ScrollToAssociatedGroup();
                manageYearGroupDetail.Classes[0].DeleteRow();

                //Save the changes
                manageYearGroupDetail.Save();

                //Delete the year group was added
                manageYearGroupDetail.Delete();

                #endregion
            }
        }

        /// <summary>
        /// Author: BaTruong
        /// Description: Amend a Year Group  for the current academic year with an active History Starting and Ending in the current Academic Year
        ///              Including the amendment of the staff allocated as Head of Year, Classes, Staff and Year Group Description Information
        /// </summary>
        [WebDriverTest(TimeoutSeconds = 2000, Enabled = false, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { "g1" }, DataProvider = "TC_YG006_Data")]
        public void TC_YG006_Amend_The_Year_Group_For_The_Current_Academic_With_Active_History_Starting_And_Ending_In_Current_Academic(string[] basicDetails, string[] activeHistoryDetails,
                                                                                                            string curriculumYear, string[] updateBasicDetails,
                                                                                                            string[] updateActiveHistoryDetails, string updateCurriculumYear,
                                                                                                            string[] updateClassDetails, string[] updateHeadOfYearDetails,
                                                                                                            string[] updateStaffDetails, string currentAcademicYear)
        {
            #region PRE-CONDITION

            //Login as School Administrator and navigate to Manage Year Groups
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);
            SeleniumHelper.NavigateMenu("Tasks", "School Groups", "Manage Year Groups");

            //Select Academic Year by current academic
            var manageYearGroupTriplet = new ManageYearGroupsTriplet();
            manageYearGroupTriplet.SearchCriteria.AcademicYear = currentAcademicYear;

            //Add new Year Group for current academic
            var manageYearGroupDetail = manageYearGroupTriplet.AddYearGroup();

            //Input basic information
            manageYearGroupDetail.FullName = basicDetails[0];
            manageYearGroupDetail.ShortName = basicDetails[1];
            manageYearGroupDetail.DisplayOrder = basicDetails[2];

            //Input active history with 'Start Date' and 'End Date'
            manageYearGroupDetail.ActiveHistory[0].StartDate = activeHistoryDetails[0];
            manageYearGroupDetail.ActiveHistory[0].EndDate = activeHistoryDetails[1];

            //Scroll to Associated Groups
            manageYearGroupDetail.ScrollToAssociatedGroup();

            //Input information to Associated Groups
            manageYearGroupDetail.CurriculumYear = curriculumYear;

            //Click Save
            manageYearGroupDetail.Save();

            #endregion

            #region TEST STEPS

            //Select year group was added at pre-condition
            manageYearGroupTriplet.Refresh();
            var yearGroupTile = manageYearGroupTriplet.SearchCriteria.Search().FirstOrDefault(x => x.FullName.Equals(basicDetails[0]));
            manageYearGroupDetail = yearGroupTile.Click<ManageYearGroupsPage>();

            //Amend basic informations
            manageYearGroupDetail.FullName = updateBasicDetails[0];
            manageYearGroupDetail.ShortName = updateBasicDetails[1];
            manageYearGroupDetail.DisplayOrder = updateBasicDetails[2];

            //Amend active history with 'Start Date' and 'End Date'
            manageYearGroupDetail.ActiveHistory[0].StartDate = updateActiveHistoryDetails[0];
            manageYearGroupDetail.ActiveHistory[0].EndDate = updateActiveHistoryDetails[1];

            //Scroll to Associated Groups
            manageYearGroupDetail.ScrollToAssociatedGroup();

            //Amend information in Associated Groups
            manageYearGroupDetail.CurriculumYear = updateCurriculumYear;
            manageYearGroupDetail.Classes[0].Class = updateClassDetails[0];
            manageYearGroupDetail.Classes[0].StartDate = updateClassDetails[1];
            manageYearGroupDetail.Classes[0].EndDate = updateClassDetails[2];

            //Scroll to Staff Details
            manageYearGroupDetail.ScrollToStaffDetails();

            //Amend information in Head of Year grid
            manageYearGroupDetail.HeadOfYear[0].HeadOfYear = updateHeadOfYearDetails[0];
            manageYearGroupDetail.HeadOfYear[0].StartDate = updateHeadOfYearDetails[1];
            manageYearGroupDetail.HeadOfYear[0].EndDate = updateHeadOfYearDetails[2];

            //Amend information in Staff grid
            manageYearGroupDetail.Staff[0].Staff = updateStaffDetails[0];
            manageYearGroupDetail.Staff[0].PastoralRole = updateStaffDetails[1];
            manageYearGroupDetail.Staff[0].StartDate = updateStaffDetails[2];
            manageYearGroupDetail.Staff[0].EndDate = updateStaffDetails[3];

            //Click Save
            manageYearGroupDetail.Save();

            //Confirmation the changes
            var confirmDialog = new ConfirmRequiredDialog();
            manageYearGroupTriplet = confirmDialog.ClickOK<ManageYearGroupsTriplet>();

            //Confirm success message appears
            manageYearGroupDetail.Refresh();
            Assert.AreEqual(true, manageYearGroupDetail.IsMessageSuccessAppear(), "Success message does not appear");

            //Re-select year group from search results to confirm Year Group was updated successfully
            manageYearGroupTriplet.Refresh();
            manageYearGroupTriplet.SearchCriteria.AcademicYear = currentAcademicYear;
            yearGroupTile = manageYearGroupTriplet.SearchCriteria.Search().FirstOrDefault(x => x.FullName.Equals(updateBasicDetails[0]));
            manageYearGroupDetail = yearGroupTile.Click<ManageYearGroupsPage>();

            //Confirm basic information was updated successfully
            Assert.AreEqual(updateBasicDetails[0], manageYearGroupDetail.FullName, "Full name was not updated");
            Assert.AreEqual(updateBasicDetails[1], manageYearGroupDetail.ShortName, "Short name was not updated");
            Assert.AreEqual(updateBasicDetails[2], manageYearGroupDetail.DisplayOrder, "Display order was not updated");

            //Confirm 'Active History' information was updated successfully 
            Assert.AreEqual(updateActiveHistoryDetails[0], manageYearGroupDetail.ActiveHistory[0].StartDate, "'Start Date' in 'Active History' Grid was not updated");
            Assert.AreEqual(updateActiveHistoryDetails[1], manageYearGroupDetail.ActiveHistory[0].EndDate, "'End Date' in 'Active History' Grid was not updated");

            //Scroll to Associated Groups
            manageYearGroupDetail.ScrollToAssociatedGroup();

            //Confirm information in Associated Groups was updated successfully
            Assert.AreEqual(updateCurriculumYear, manageYearGroupDetail.CurriculumYear, "'Curriculum Year' in Associated Groups was not updated");
            Assert.AreEqual(updateClassDetails[0], manageYearGroupDetail.Classes[0].Class, "'Class' in Associated Groups was not updated");
            Assert.AreEqual(updateClassDetails[1], manageYearGroupDetail.Classes[0].StartDate, "'Start Date' in Associated Groups was not updated");
            Assert.AreEqual(updateClassDetails[2], manageYearGroupDetail.Classes[0].EndDate, "'End Date' in Associated Groups was not updated");

            //Scroll to Staff Details
            manageYearGroupDetail.ScrollToStaffDetails();

            //Confirm information in  Head of Year grid was updated successfully
            Assert.AreEqual(updateHeadOfYearDetails[0], manageYearGroupDetail.HeadOfYear[0].HeadOfYear, "'Head Of Year' in Staff grid was not updated");
            Assert.AreEqual(updateHeadOfYearDetails[1], manageYearGroupDetail.HeadOfYear[0].StartDate, "'Start Date' in Staff grid was not updated");
            Assert.AreEqual(updateHeadOfYearDetails[2], manageYearGroupDetail.HeadOfYear[0].EndDate, "'End Date' in Staff grid was not updated");

            //Confirm information in Staff grid
            Assert.AreEqual(updateStaffDetails[0], manageYearGroupDetail.Staff[0].Staff, "'Staff' in Staff grid was not updated");
            Assert.AreEqual(updateStaffDetails[1], manageYearGroupDetail.Staff[0].PastoralRole, "'Pastoral Role' in Staff grid was not updated");
            Assert.AreEqual(updateStaffDetails[2], manageYearGroupDetail.Staff[0].StartDate, "'Start Date' in Staff grid was not updated");
            Assert.AreEqual(updateStaffDetails[3], manageYearGroupDetail.Staff[0].EndDate, "'End Date' in Staff grid was not updated");

            //Confirm the changes of year group reflected on the Search Options on Screens 'Promote Pupils'
            SeleniumHelper.NavigateMenu("Tasks", "School Groups", "Promote Pupils");
            PromotePupilsTriplet promotePupilTriplet = new PromotePupilsTriplet();
            var yearGroups = promotePupilTriplet.SearchCriteria.YearGroups;
            var yearGroup = yearGroups[updateBasicDetails[0]];
            Assert.AreNotEqual(null, yearGroup, "The year group does not display in Promote Pupil page");

            //Confirm the changes of year group reflected on the Search Options on Screens 'Allocate New Intake'
            SeleniumHelper.NavigateMenu("Tasks", "School Groups", "Allocate Future Pupils");
            var newIntake = new AllocateFuturePupilsTriplet();
            yearGroups = newIntake.SearchCriteria.YearGroups;
            yearGroup = yearGroups[updateBasicDetails[0]];
            Assert.AreNotEqual(null, yearGroup, "The year group does not display in Allocate New Intake page");

            //Confirm the changes of year group reflected on the Search Options on Screens 'Allocate Pupils to Groups'
            SeleniumHelper.NavigateMenu("Tasks", "School Groups", "Allocate Pupils To Groups");
            var allocatePupilToGroup = new AllocatePupilsToGroupsTriplet();
            yearGroups = allocatePupilToGroup.SearchCriteria.YearGroups;
            yearGroup = yearGroups[updateBasicDetails[0]];
            Assert.AreNotEqual(null, yearGroup, "The year group does not display in Allocate Pupils to Groups page");

            #endregion

            #region POS-CONDITION

            //Back to Year group details page to delete year group was added
            SeleniumHelper.NavigateMenu("Tasks", "School Groups", "Manage Year Groups");
            manageYearGroupTriplet = new ManageYearGroupsTriplet();
            manageYearGroupTriplet.SearchCriteria.AcademicYear = currentAcademicYear;
            var yearGroupListResult = manageYearGroupTriplet.SearchCriteria.Search();
            var yearGroupResult = yearGroupListResult.FirstOrDefault(p => p.FullName.Equals(updateBasicDetails[0]));
            manageYearGroupDetail = yearGroupResult.Click<ManageYearGroupsPage>();

            //Remove the records in Staff Details section
            manageYearGroupDetail.ScrollToStaffDetails();
            manageYearGroupDetail.Staff[0].DeleteRow();
            manageYearGroupDetail.HeadOfYear[0].DeleteRow();

            //Remove the records in Associated Groups section
            manageYearGroupDetail.ScrollToAssociatedGroup();
            manageYearGroupDetail.Classes[0].DeleteRow();

            //Save the changes
            manageYearGroupDetail.Save();

            //Delete the year group was added
            manageYearGroupDetail.Delete();

            #endregion
        }

        /// <summary>
        /// Author: Y.Ta
        /// Desscription: Remove a Year Group from a Pastoral structure for the Future academic year by entering an End Date into Associated Group record
        /// Issue: Does not found Allocate Pupils to Groups
        /// </summary>
        [WebDriverTest(TimeoutSeconds = 2000, Enabled = false, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { "g1" }, DataProvider = "TC_YG007_Data")]
        public void TC_YG007_Remove_The_Year_Group_For_The_Future_Academic_Year_By_Entering_An_End_Date_Into_Associated_Group_Record(string[] basicDetails, string[] activeHistoryDetails, string curriculumYear,
                                                                                                            string[] classDetails, string[] headOfYearDetails, string[] staffDetails, string currentAcademicYear, string futureAcademicYear, string endDate)
        {
            #region Pre-Condition: Create new Year Group for future academic year
            //Login as School Administrator and navigate to Manage Year Groups
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);
            SeleniumHelper.NavigateMenu("Tasks", "School Groups", "Manage Year Groups");

            //Select Academic Year by future academic
            var manageYearGroupTriplet = new ManageYearGroupsTriplet();
            manageYearGroupTriplet.SearchCriteria.AcademicYear = futureAcademicYear;

            //Add new Year Group for future academic
            var manageYearGroupDetail = manageYearGroupTriplet.AddYearGroup();

            //Input basic information
            manageYearGroupDetail.FullName = basicDetails[0];
            manageYearGroupDetail.ShortName = basicDetails[1];
            manageYearGroupDetail.DisplayOrder = basicDetails[2];

            //Input active history with 'Start Date' and 'End Date' is empty
            manageYearGroupDetail.ActiveHistory[0].StartDate = activeHistoryDetails[0];

            //Scroll to Associated Groups
            manageYearGroupDetail.ScrollToAssociatedGroup();

            //Input information to Associated Groups with null value into 'End Date'
            manageYearGroupDetail.CurriculumYear = curriculumYear;
            manageYearGroupDetail.Classes[0].Class = classDetails[0];
            manageYearGroupDetail.Classes[0].StartDate = classDetails[1];

            //Scroll to Staff Details
            manageYearGroupDetail.ScrollToStaffDetails();

            //Input information to Head of Year grid with null value into 'End Date'
            manageYearGroupDetail.HeadOfYear[0].HeadOfYear = headOfYearDetails[0];
            manageYearGroupDetail.HeadOfYear[0].StartDate = headOfYearDetails[1];

            //Input information to Staff grid with null value into 'End Date'
            manageYearGroupDetail.Staff[0].Staff = staffDetails[0];
            manageYearGroupDetail.Staff[0].PastoralRole = staffDetails[1];
            manageYearGroupDetail.Staff[0].StartDate = staffDetails[2];

            //Click Save
            manageYearGroupDetail.Save();

            #endregion Pre-Condition: Create new Year Group for future academic year

            #region Steps

            // Search and Open the specific year group
            manageYearGroupTriplet.Refresh();
            manageYearGroupTriplet.SearchCriteria.AcademicYear = futureAcademicYear;
            var yearGroupListResult = manageYearGroupTriplet.SearchCriteria.Search();
            var yearGroupResult = yearGroupListResult.FirstOrDefault(p => p.FullName.Equals(basicDetails[0]));
            manageYearGroupDetail = yearGroupResult.Click<ManageYearGroupsPage>();

            //Scroll to Associated Groups   
            //manageYearGroupDetail.Classes[0].EndDate = endDate;
            manageYearGroupDetail.ActiveHistory[0].EndDate = endDate;

            //Click Button 'Save'
            manageYearGroupDetail.Save();

            // Click ok for confirm required dialog
            var confirmRequireDialog = new ConfirmRequiredDialog();
            confirmRequireDialog.ClickOk();

            // Verify message succesfully
            manageYearGroupDetail.Refresh();
            Assert.AreEqual(true, manageYearGroupDetail.IsMessageSuccessAppear(), "The message success does not display");


            // Year Group should no longer appear in future academic years
            manageYearGroupTriplet.Refresh();
            manageYearGroupTriplet.SearchCriteria.AcademicYear = futureAcademicYear;
            yearGroupListResult = manageYearGroupTriplet.SearchCriteria.Search();
            yearGroupResult = yearGroupListResult.FirstOrDefault(p => p.FullName.Equals(basicDetails[0]));
            Assert.AreEqual(null, yearGroupResult, "The removed Year Group still appears in future academic year");

            // but should be visible in the current academic year
            manageYearGroupTriplet.Refresh();
            manageYearGroupTriplet.SearchCriteria.AcademicYear = currentAcademicYear;
            yearGroupListResult = manageYearGroupTriplet.SearchCriteria.Search();
            yearGroupResult = yearGroupListResult.FirstOrDefault(p => p.FullName.Equals(basicDetails[0]));
            Assert.AreNotEqual(null, yearGroupResult, "The removed Year Group does not appear in future academic year");

            //The Changes to the Year Group are reflected on the Search Options on Screens:            
            //- Allocate Pupils to Groups -> Not found
            //- Promote Pupils
            SeleniumHelper.NavigateMenu("Tasks", "School Groups", "Promote Pupils");
            PromotePupilsTriplet promotePupilTriplet = new PromotePupilsTriplet();
            var classGroup = promotePupilTriplet.SearchCriteria.Classes;
            var classItem = classGroup[classDetails[0]];
            Assert.AreNotEqual(null, classItem, "The class does not display in Promote Pupil page");

            //- Allocate New Intake
            SeleniumHelper.NavigateMenu("Tasks", "School Groups", "Allocate Future Pupils");
            var newIntake = new AllocateFuturePupilsTriplet();
            classGroup = newIntake.SearchCriteria.Classes;
            classItem = classGroup[classDetails[0]];
            Assert.AreNotEqual(null, classItem, "The class does not display in Allocate New Intake page");

            #endregion

            #region Post-Condition :Delete Year Group

            SeleniumHelper.NavigateMenu("Tasks", "School Groups", "Manage Year Groups");

            manageYearGroupDetail = new ManageYearGroupsPage();
            // Remove Class
            manageYearGroupDetail.Classes[0].DeleteRow();
            manageYearGroupDetail.Refresh();
            manageYearGroupDetail.HeadOfYear[0].DeleteRow();
            manageYearGroupDetail.Staff[0].DeleteRow();
            manageYearGroupDetail.Refresh();

            manageYearGroupDetail.Save();
            manageYearGroupDetail.Delete();

            #endregion Post-Condition :Delete Year Group

        }

        /// <summary>
        /// Author: Y.Ta
        /// Desscription: Remove a Year Group from a Pastoral structure for the Future academic year by deleting Association Group record
        /// Issue: Does not found Allocate Pupils to Groups
        /// </summary>
        [WebDriverTest(TimeoutSeconds = 2000, Enabled = false, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { "g1" }, DataProvider = "TC_YG008_Data")]
        public void TC_YG008_Remove_The_Year_Group_For_The_Future_Academic_Year_By_Deleting_An_End_Date_Into_Associated_Group_Record(string[] classInfo, string[] activeHistory, string[] basicDetails, string[] activeHistoryDetails, string curriculumYear,
                                                                                                            string[] classDetails, string[] headOfYearDetails, string[] staffDetails, string futureAcademicYear)
        {
            #region Pre-Condition: Create new Year Group for future academic year

            //Login as School Administrator and navigate to Manage Year Groups
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);
            SeleniumHelper.NavigateMenu("Tasks", "School Groups", "Manage Classes");

            // Create a CLASS
            //  Search and delete if Class is existing
            var manageClassesTriplet = new ManageClassesTriplet();
            manageClassesTriplet.SearchCriteria.SearchByAcademicYear = futureAcademicYear;
            var classesResults = manageClassesTriplet.SearchCriteria.Search();
            //var classesTile = classesResults.SingleOrDefault(t => t.ClassFullName.Equals(classFullName));
            var classesTile = classesResults.SingleOrDefault(t => t.ClassFullName.Equals(classInfo[0]));

            manageClassesTriplet.SelectSearchTile(classesTile);
            manageClassesTriplet.Delete();

            // Create new Classe with Year Group
            var manageClassesPage = manageClassesTriplet.Create();

            manageClassesPage.ClassFullName = classInfo[0];
            manageClassesPage.ClassShortName = classInfo[1];
            manageClassesPage.DisplayOrder = classInfo[2];

            //Active History table
            var activeHistoryTable = manageClassesPage.ActiveHistoryTable;
            activeHistoryTable[0].StartDate = activeHistory[0];
            activeHistoryTable[0].EndDate = activeHistory[1];
            manageClassesPage.Save();
            manageClassesPage.Refresh();


            SeleniumHelper.NavigateMenu("Tasks", "School Groups", "Manage Year Groups");

            //Select Academic Year by future academic
            var manageYearGroupTriplet = new ManageYearGroupsTriplet();
            manageYearGroupTriplet.SearchCriteria.AcademicYear = futureAcademicYear;

            //Add new Year Group for future academic
            var manageYearGroupDetail = manageYearGroupTriplet.AddYearGroup();

            //Input basic information
            manageYearGroupDetail.FullName = basicDetails[0];
            manageYearGroupDetail.ShortName = basicDetails[1];
            manageYearGroupDetail.DisplayOrder = basicDetails[2];

            //Input active history with 'Start Date' and 'End Date' is empty
            manageYearGroupDetail.ActiveHistory[0].StartDate = activeHistoryDetails[0];

            //Scroll to Associated Groups
            manageYearGroupDetail.ScrollToAssociatedGroup();

            //Input information to Associated Groups with null value into 'End Date'
            manageYearGroupDetail.CurriculumYear = curriculumYear;
            manageYearGroupDetail.Classes[0].Class = classInfo[0];
            manageYearGroupDetail.Classes[0].StartDate = classDetails[1];

            //Scroll to Staff Details
            manageYearGroupDetail.ScrollToStaffDetails();

            //Input information to Head of Year grid with null value into 'End Date'
            manageYearGroupDetail.HeadOfYear[0].HeadOfYear = headOfYearDetails[0];
            manageYearGroupDetail.HeadOfYear[0].StartDate = headOfYearDetails[1];

            //Input information to Staff grid with null value into 'End Date'
            manageYearGroupDetail.Staff[0].Staff = staffDetails[0];
            manageYearGroupDetail.Staff[0].PastoralRole = staffDetails[1];
            manageYearGroupDetail.Staff[0].StartDate = staffDetails[2];

            //Click Save
            manageYearGroupDetail.Save();

            #endregion Pre-Condition: Create new Year Group for future academic year

            #region Steps

            // Search and Open the specific year group
            manageYearGroupTriplet.Refresh();
            manageYearGroupTriplet.SearchCriteria.AcademicYear = futureAcademicYear;
            var yearGroupListResult = manageYearGroupTriplet.SearchCriteria.Search();
            var yearGroupResult = yearGroupListResult.FirstOrDefault(p => p.FullName.Equals(basicDetails[0]));
            manageYearGroupDetail = yearGroupResult.Click<ManageYearGroupsPage>();

            //Scroll to Associated Groups   
            //Click Icon 'Delete this  Row'
            var classRow = manageYearGroupDetail.Classes.Rows.FirstOrDefault(p => p.Class.Equals(classInfo[0]));
            classRow.DeleteRow();
            //Click Button 'Save'
            manageYearGroupDetail.Save();
            // Verify message succesfully
            Assert.AreEqual(true, manageYearGroupDetail.IsMessageSuccessAppear(), "The succesfully message does not display");


            manageYearGroupTriplet.Refresh();
            manageYearGroupTriplet.SearchCriteria.AcademicYear = futureAcademicYear;
            yearGroupListResult = manageYearGroupTriplet.SearchCriteria.Search();
            yearGroupResult = yearGroupListResult.FirstOrDefault(p => p.FullName.Equals(basicDetails[0]));
            manageYearGroupDetail = yearGroupResult.Click<ManageYearGroupsPage>();


            // Year Group should no longer appear in any academic year.
            classRow = manageYearGroupDetail.Classes.Rows.FirstOrDefault(p => p.Class.Equals(classInfo[0]));
            Assert.AreEqual(null, classRow, "The deleted class still display");


            // Delete class at manage class screen
            SeleniumHelper.NavigateMenu("Tasks", "School Groups", "Manage Classes");
            manageClassesTriplet = new ManageClassesTriplet();
            manageClassesTriplet.SearchCriteria.SearchByAcademicYear = futureAcademicYear;
            classesResults = manageClassesTriplet.SearchCriteria.Search();
            classesTile = classesResults.SingleOrDefault(t => t.ClassFullName.Equals(classInfo[0]));

            manageClassesTriplet.SelectSearchTile(classesTile);
            manageClassesTriplet.Delete();

            //The Changes to the Year Group are reflected on the Search Options on Screens:
            //- Allocate Pupils to Groups -> Not found
            //- Promote Pupils
            SeleniumHelper.NavigateMenu("Tasks", "School Groups", "Promote Pupils");
            PromotePupilsTriplet promotePupilTriplet = new PromotePupilsTriplet();
            var classGroup = promotePupilTriplet.SearchCriteria.Classes;
            var classItem = classGroup[classInfo[0]];
            Assert.AreEqual(null, classItem, "The deleted class still display in Promote Pupil page");

            //- Allocate New Intake
            SeleniumHelper.NavigateMenu("Tasks", "School Groups", "Allocate Future Pupils");
            var newIntake = new AllocateFuturePupilsTriplet();
            classGroup = newIntake.SearchCriteria.Classes;
            classItem = classGroup[classInfo[0]];
            Assert.AreEqual(null, classItem, "The deleted class still display in Allocate New Intake page");

            #endregion

            #region Post-Condition :Delete Year Group

            SeleniumHelper.NavigateMenu("Tasks", "School Groups", "Manage Year Groups");

            //Delete the year group was added

            manageYearGroupDetail = new ManageYearGroupsPage();
            manageYearGroupDetail.Refresh();
            manageYearGroupDetail.HeadOfYear[0].DeleteRow();
            manageYearGroupDetail.Staff[0].DeleteRow();
            manageYearGroupDetail.Refresh();

            manageYearGroupDetail.Save();
            manageYearGroupDetail.Delete();

            #endregion Post-Condition :Delete Year Group

        }

        /// <summary>
        /// TC YG-09
        /// Au : An Nguyen
        /// Description: Remove a Year Group for a Future Academic Year from the entire Pastoral structure by entering an End Date
        /// Role: School Administrator
        /// </summary>
        [WebDriverTest(TimeoutSeconds = 600, Enabled = false, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { "g1" }, DataProvider = "TC_YG009_Data")]
        public void TC_YG009_Remove_Year_Group_for_Future_Academic_Year_from_the_entire_Pastoral_structure_by_entering_an_End_Date(string fullName, string shortName, string displayOrder,
                    string startDate, string curriculumYear, string currentAcademic, string futureAcademic, string endDate)
        {
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);

            #region Pre-Condition : Add new year group

            //Navigate to Manage Year Groups
            SeleniumHelper.NavigateMenu("Tasks", "School Groups", "Manage Year Groups");
            var manageYearGroups = new ManageYearGroupsTriplet();

            //Create new year group
            var manageYearGroupDetail = manageYearGroups.AddYearGroup();
            manageYearGroupDetail.FullName = fullName;
            manageYearGroupDetail.ShortName = shortName;
            manageYearGroupDetail.DisplayOrder = displayOrder;

            //Add start date
            var activeHistoryRow = manageYearGroupDetail.ActiveHistory[0];
            activeHistoryRow.StartDate = startDate;

            //Select curriculum year
            manageYearGroupDetail.ScrollToAssociatedGroup();
            manageYearGroupDetail.CurriculumYear = curriculumYear;

            //Save
            manageYearGroupDetail.Save();

            #endregion

            #region Test steps

            //Navigate to Manage Year Groups
            SeleniumHelper.NavigateMenu("Tasks", "School Groups", "Manage Year Groups");
            manageYearGroups = new ManageYearGroupsTriplet();

            //Search Year Group of future academic year
            manageYearGroups.SearchCriteria.AcademicYear = futureAcademic;
            var yearGroupSearchResult = manageYearGroups.SearchCriteria.Search();
            var yearGroupTile = yearGroupSearchResult.FirstOrDefault(t => t.FullName.Equals(fullName));
            manageYearGroupDetail = yearGroupTile.Click<ManageYearGroupsPage>();

            //Enter end date
            activeHistoryRow = manageYearGroupDetail.ActiveHistory[0];
            activeHistoryRow.EndDate = endDate;

            //Save and confirm
            manageYearGroupDetail.Save();
            var confirmDialog = new ConfirmRequiredDialog();
            confirmDialog.ClickOk();

            //Verify success message displays
            manageYearGroupDetail.Refresh();
            Assert.AreEqual(true, manageYearGroupDetail.IsMessageSuccessAppear(), "Save year group unsuccessfull");

            //Verify Year Group should be visible in the current academic year
            manageYearGroups.Refresh();
            manageYearGroups.SearchCriteria.AcademicYear = currentAcademic;
            yearGroupSearchResult = manageYearGroups.SearchCriteria.Search();
            yearGroupTile = yearGroupSearchResult.FirstOrDefault(t => t.FullName.Equals(fullName));
            Assert.AreNotEqual(null, yearGroupTile, "The Year Group does not visible in Year Group for the Current Year");

            //Verify Year Group should no longer appear in Year group for the Future year
            manageYearGroups.Refresh();
            manageYearGroups.SearchCriteria.AcademicYear = futureAcademic;
            yearGroupSearchResult = manageYearGroups.SearchCriteria.Search();
            yearGroupTile = yearGroupSearchResult.FirstOrDefault(t => t.FullName.Equals(fullName));
            Assert.AreEqual(null, yearGroupTile, "The Year Group still appear in Year Group for the Future Year");

            //Verify year group appear on Allocate Future Pupils
            SeleniumHelper.NavigateMenu("Tasks", "School Groups", "Allocate Future Pupils");
            var allocateFutures = new AllocateFuturePupilsTriplet();
            var allocateFutureYearGroups = allocateFutures.SearchCriteria.YearGroups;
            var allocateFutureYearGroup = allocateFutureYearGroups.CheckBoxItems.FirstOrDefault(t => t.Title.Equals(fullName));
            Assert.AreNotEqual(null, allocateFutureYearGroup, "Year group does not appear on Allocate Future Pupils screen");

            //Verify year group appear on Allocate Pupils To Groups
            SeleniumHelper.NavigateMenu("Tasks", "School Groups", "Allocate Pupils To Groups");
            var allocateGroups = new AllocatePupilsToGroupsTriplet();
            var allocateGroupsYearGroups = allocateGroups.SearchCriteria.YearGroups;
            var allocateGroupsYearGroup = allocateGroupsYearGroups.CheckBoxItems.FirstOrDefault(t => t.Title.Equals(fullName));
            Assert.AreNotEqual(null, allocateGroupsYearGroup, "Year group does not appear on Allocate Pupils To Groups screen");

            //Verify year group disappear on Promote Pupils
            SeleniumHelper.NavigateMenu("Tasks", "School Groups", "Promote Pupils");
            var promote = new PromotePupilsTriplet();
            var promoteYearGroups = promote.SearchCriteria.YearGroups;
            var promoteYearGroup = promoteYearGroups.CheckBoxItems.FirstOrDefault(t => t.Title.Equals(fullName));
            Assert.AreEqual(null, promoteYearGroup, "Year group does not disappear on Promote Pupils screen");

            #endregion

            #region Post-Condition : Delete year group

            SeleniumHelper.NavigateMenu("Tasks", "School Groups", "Manage Year Groups");
            manageYearGroups = new ManageYearGroupsTriplet();
            manageYearGroups.SearchCriteria.AcademicYear = currentAcademic;
            yearGroupSearchResult = manageYearGroups.SearchCriteria.Search();
            yearGroupTile = yearGroupSearchResult.FirstOrDefault(t => t.FullName.Equals(fullName));
            manageYearGroupDetail = yearGroupTile.Click<ManageYearGroupsPage>();
            manageYearGroupDetail.Delete();

            #endregion
        }

        /// <summary>
        /// TC YG-10
        /// Au : An Nguyen
        /// Description: Remove a Year Group from a Pastoral structure for the current academic year by entering an End Date into  Associated Group record
        /// Role: School Administrator
        /// </summary>
        [WebDriverTest(TimeoutSeconds = 600, Enabled = false, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { "g1" }, DataProvider = "TC_YG010_Data")]
        public void TC_YG010_Remove_Year_Group_from_Pastoral_structure_for_the_current_academic_year_by_entering_an_End_Date_into_Associated_Group_record(string fullName, string shortName, string displayOrder,
                    string startDate, string curriculumYear, string className, string earlyAcademic, string currentAcademic, string endDate)
        {
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);

            #region Pre-Condition : Add new year group

            //Navigate to Manage Year Groups
            SeleniumHelper.NavigateMenu("Tasks", "School Groups", "Manage Year Groups");
            var manageYearGroups = new ManageYearGroupsTriplet();

            //Create new year group
            var manageYearGroupDetail = manageYearGroups.AddYearGroup();
            manageYearGroupDetail.FullName = fullName;
            manageYearGroupDetail.ShortName = shortName;
            manageYearGroupDetail.DisplayOrder = displayOrder;

            //Add start date
            var activeHistoryRow = manageYearGroupDetail.ActiveHistory[0];
            activeHistoryRow.StartDate = startDate;

            //Select curriculum year
            manageYearGroupDetail.ScrollToAssociatedGroup();
            manageYearGroupDetail.CurriculumYear = curriculumYear;

            //Add class
            var classRow = manageYearGroupDetail.Classes[0];
            classRow.Class = className;
            classRow.StartDate = startDate;

            //Save
            manageYearGroupDetail.Save();

            #endregion

            #region Test steps

            //Navigate to Manage Year Groups
            SeleniumHelper.NavigateMenu("Tasks", "School Groups", "Manage Year Groups");
            manageYearGroups = new ManageYearGroupsTriplet();

            //Search Year Group of current academic year
            manageYearGroups.SearchCriteria.AcademicYear = currentAcademic;
            var yearGroupSearchResult = manageYearGroups.SearchCriteria.Search();
            var yearGroupTile = yearGroupSearchResult.FirstOrDefault(t => t.FullName.Equals(fullName));
            manageYearGroupDetail = yearGroupTile.Click<ManageYearGroupsPage>();

            //Enter end date
            activeHistoryRow = manageYearGroupDetail.ActiveHistory[0];
            activeHistoryRow.EndDate = endDate;

            //Save and confirm
            manageYearGroupDetail.Save();
            var confirmDialog = new ConfirmRequiredDialog();
            confirmDialog.ClickOk();

            //Verify success message does not display
            manageYearGroupDetail.Refresh();
            Assert.AreEqual(true, manageYearGroupDetail.IsMessageSuccessAppear(), "Save year group unsuccessfull");

            //Verify Year Group should no longer appear in Year group for the current year
            manageYearGroups.Refresh();
            manageYearGroups.SearchCriteria.AcademicYear = currentAcademic;
            yearGroupSearchResult = manageYearGroups.SearchCriteria.Search();
            yearGroupTile = yearGroupSearchResult.FirstOrDefault(t => t.FullName.Equals(fullName));
            Assert.AreEqual(null, yearGroupTile, "The Year Group still appear in Year Group for the Current Year");

            //Verify Year Group should be visible in the early academic year
            manageYearGroups.Refresh();
            manageYearGroups.SearchCriteria.AcademicYear = earlyAcademic;
            yearGroupSearchResult = manageYearGroups.SearchCriteria.Search();
            yearGroupTile = yearGroupSearchResult.FirstOrDefault(t => t.FullName.Equals(fullName));
            Assert.AreNotEqual(null, yearGroupTile, "The Year Group does not visible in Year Group in ealier Academic Year");

            //Verify year group disappear on Allocate Future Pupils
            SeleniumHelper.NavigateMenu("Tasks", "School Groups", "Allocate Future Pupils");
            var allocateFutures = new AllocateFuturePupilsTriplet();
            var allocateFutureYearGroups = allocateFutures.SearchCriteria.YearGroups;
            var allocateFutureYearGroup = allocateFutureYearGroups.CheckBoxItems.FirstOrDefault(t => t.Title.Equals(fullName));
            Assert.AreEqual(null, allocateFutureYearGroup, "Year group does not disappear on Allocate Future Pupils screen");

            //Verify year group disappear on Allocate Pupils To Groups
            SeleniumHelper.NavigateMenu("Tasks", "School Groups", "Allocate Pupils To Groups");
            var allocateGroups = new AllocatePupilsToGroupsTriplet();
            var allocateGroupsYearGroups = allocateGroups.SearchCriteria.YearGroups;
            var allocateGroupsYearGroup = allocateGroupsYearGroups.CheckBoxItems.FirstOrDefault(t => t.Title.Equals(fullName));
            Assert.AreEqual(null, allocateGroupsYearGroup, "Year group does not disappear on Allocate Pupils To Groups screen");

            //Verify year group disappear on Promote Pupils
            SeleniumHelper.NavigateMenu("Tasks", "School Groups", "Promote Pupils");
            var promote = new PromotePupilsTriplet();
            var promoteYearGroups = promote.SearchCriteria.YearGroups;
            var promoteYearGroup = promoteYearGroups.CheckBoxItems.FirstOrDefault(t => t.Title.Equals(fullName));
            Assert.AreEqual(null, promoteYearGroup, "Year group does not disappear on Promote Pupils screen");

            #endregion

            #region Post-Condition : Delete year group

            SeleniumHelper.NavigateMenu("Tasks", "School Groups", "Manage Year Groups");
            manageYearGroups = new ManageYearGroupsTriplet();
            manageYearGroups.SearchCriteria.AcademicYear = earlyAcademic;
            yearGroupSearchResult = manageYearGroups.SearchCriteria.Search();
            yearGroupTile = yearGroupSearchResult.FirstOrDefault(t => t.FullName.Equals(fullName));
            manageYearGroupDetail = yearGroupTile.Click<ManageYearGroupsPage>();

            //Delete class
            manageYearGroupDetail.Classes[0].DeleteRow();
            manageYearGroupDetail.Save();

            //Delete Year Group
            manageYearGroupDetail.Delete();

            #endregion
        }

        /// <summary>
        /// Author: Y.Ta
        /// Desscription: Remove a Year Group from a Pastoral structure for the current academic year by deleting Association Group record
        /// Issue: Does not found Allocate Pupils to Groups
        /// </summary>
        [WebDriverTest(TimeoutSeconds = 2000, Enabled = false, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { "g1" }, DataProvider = "TC_YG011_Data")]
        public void TC_YG011_Remove_The_Year_Group_For_The_Current_Academic_Year_By_Deleting_An_End_Date_Into_Associated_Group_Record(string[] classInfo, string[] activeHistory, string[] basicDetails, string[] activeHistoryDetails, string curriculumYear,
                                                                                                            string[] classDetails, string[] headOfYearDetails, string[] staffDetails, string currentAcademicYear)
        {
            #region Pre-Condition: Create new Year Group for current academic year
            //Login as School Administrator and navigate to Manage Year Groups
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);

            SeleniumHelper.NavigateMenu("Tasks", "School Groups", "Manage Classes");

            // Create a CLASS
            //  Search and delete if Class is existing
            var manageClassesTriplet = new ManageClassesTriplet();
            manageClassesTriplet.SearchCriteria.SearchByAcademicYear = currentAcademicYear;
            var classesResults = manageClassesTriplet.SearchCriteria.Search();
            //var classesTile = classesResults.SingleOrDefault(t => t.ClassFullName.Equals(classFullName));
            var classesTile = classesResults.SingleOrDefault(t => t.ClassFullName.Equals(classInfo[0]));

            manageClassesTriplet.SelectSearchTile(classesTile);
            manageClassesTriplet.Delete();

            // Create new Classe with Year Group
            var manageClassesPage = manageClassesTriplet.Create();

            manageClassesPage.ClassFullName = classInfo[0];
            manageClassesPage.ClassShortName = classInfo[1];
            manageClassesPage.DisplayOrder = classInfo[2];

            //Active History table
            var activeHistoryTable = manageClassesPage.ActiveHistoryTable;
            activeHistoryTable[0].StartDate = activeHistory[0];
            activeHistoryTable[0].EndDate = activeHistory[1];
            manageClassesPage.Save();
            manageClassesPage.Refresh();

            SeleniumHelper.NavigateMenu("Tasks", "School Groups", "Manage Year Groups");

            //Select Academic Year by current academic
            var manageYearGroupTriplet = new ManageYearGroupsTriplet();
            manageYearGroupTriplet.SearchCriteria.AcademicYear = currentAcademicYear;

            //Add new Year Group for current academic
            var manageYearGroupDetail = manageYearGroupTriplet.AddYearGroup();

            //Input basic information
            manageYearGroupDetail.FullName = basicDetails[0];
            manageYearGroupDetail.ShortName = basicDetails[1];
            manageYearGroupDetail.DisplayOrder = basicDetails[2];

            //Input active history with 'Start Date' and 'End Date' is empty
            manageYearGroupDetail.ActiveHistory[0].StartDate = activeHistoryDetails[0];

            //Scroll to Associated Groups
            manageYearGroupDetail.ScrollToAssociatedGroup();

            //Input information to Associated Groups with null value into 'End Date'
            manageYearGroupDetail.CurriculumYear = curriculumYear;
            manageYearGroupDetail.Classes[0].Class = classInfo[0];
            manageYearGroupDetail.Classes[0].StartDate = classDetails[1];

            //Scroll to Staff Details
            manageYearGroupDetail.ScrollToStaffDetails();

            //Input information to Head of Year grid with null value into 'End Date'
            manageYearGroupDetail.HeadOfYear[0].HeadOfYear = headOfYearDetails[0];
            manageYearGroupDetail.HeadOfYear[0].StartDate = headOfYearDetails[1];

            //Input information to Staff grid with null value into 'End Date'
            manageYearGroupDetail.Staff[0].Staff = staffDetails[0];
            manageYearGroupDetail.Staff[0].PastoralRole = staffDetails[1];
            manageYearGroupDetail.Staff[0].StartDate = staffDetails[2];

            //Click Save
            manageYearGroupDetail.Save();

            #endregion Pre-Condition: Create new Year Group for current academic year

            #region Steps

            // Search and Open the specific year group
            manageYearGroupTriplet.Refresh();
            manageYearGroupTriplet.SearchCriteria.AcademicYear = currentAcademicYear;
            var yearGroupListResult = manageYearGroupTriplet.SearchCriteria.Search();
            var yearGroupResult = yearGroupListResult.FirstOrDefault(p => p.FullName.Equals(basicDetails[0]));
            manageYearGroupDetail = yearGroupResult.Click<ManageYearGroupsPage>();

            //Scroll to Associated Groups   
            //Click Icon 'Delete this  Row'
            var classRow = manageYearGroupDetail.Classes.Rows.FirstOrDefault(p => p.Class.Equals(classInfo[0]));
            classRow.DeleteRow();
            //Click Button 'Save'
            manageYearGroupDetail.Save();
            // Verify message succesfully
            Assert.AreEqual(true, manageYearGroupDetail.IsMessageSuccessAppear(), "The success message does not display");

            manageYearGroupTriplet.Refresh();
            manageYearGroupTriplet.SearchCriteria.AcademicYear = currentAcademicYear;
            yearGroupListResult = manageYearGroupTriplet.SearchCriteria.Search();
            yearGroupResult = yearGroupListResult.FirstOrDefault(p => p.FullName.Equals(basicDetails[0]));
            manageYearGroupDetail = yearGroupResult.Click<ManageYearGroupsPage>();

            // Year Group should no longer appear in any academic year.
            classRow = manageYearGroupDetail.Classes.Rows.FirstOrDefault(p => p.Class.Equals(classDetails[0]));
            Assert.AreEqual(null, classRow, "The deleted class still display");


            // Delete class at manage class screen
            SeleniumHelper.NavigateMenu("Tasks", "School Groups", "Manage Classes");
            manageClassesTriplet = new ManageClassesTriplet();
            manageClassesTriplet.SearchCriteria.SearchByAcademicYear = currentAcademicYear;
            classesResults = manageClassesTriplet.SearchCriteria.Search();
            classesTile = classesResults.SingleOrDefault(t => t.ClassFullName.Equals(classInfo[0]));

            manageClassesTriplet.SelectSearchTile(classesTile);
            manageClassesTriplet.Delete();

            //The Changes to the Year Group are reflected on the Search Options on Screens:            
            //- Allocate Pupils to Groups -> Not found
            //- Promote Pupils
            SeleniumHelper.NavigateMenu("Tasks", "School Groups", "Promote Pupils");
            PromotePupilsTriplet promotePupilTriplet = new PromotePupilsTriplet();
            var classGroup = promotePupilTriplet.SearchCriteria.Classes;
            var classItem = classGroup[classInfo[0]];
            Assert.AreEqual(null, classItem, "The deleted class still display in Promote Pupil page");

            //- Allocate New Intake
            SeleniumHelper.NavigateMenu("Tasks", "School Groups", "Allocate Future Pupils");
            var newIntake = new AllocateFuturePupilsTriplet();
            classGroup = newIntake.SearchCriteria.Classes;
            classItem = classGroup[classInfo[0]];
            Assert.AreEqual(null, classItem, "The deleted class still display in Allocate New Intake page");

            #endregion

            #region Post-Condition :Delete Year Group

            SeleniumHelper.NavigateMenu("Tasks", "School Groups", "Manage Year Groups");

            //Delete the year group was added

            manageYearGroupDetail = new ManageYearGroupsPage();
            manageYearGroupDetail.Refresh();
            manageYearGroupDetail.HeadOfYear[0].DeleteRow();
            manageYearGroupDetail.Staff[0].DeleteRow();
            manageYearGroupDetail.Refresh();

            manageYearGroupDetail.Save();
            manageYearGroupDetail.Delete();

            #endregion Post-Condition :Delete Year Group

        }

        /// <summary>
        /// Author: Ba.Truong
        /// Desscription: Remove a Year Group for the Current Academic Year from the entire Pastoral structure by entering an End Date
        /// </summary>
        [WebDriverTest(TimeoutSeconds = 2000, Enabled = false, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { "g1" }, DataProvider = "TC_YG012_Data")]
        public void TC_YG012_Remove_The_Year_Group_For_The_Current_Academic_Year_By_Entering_An_End_Date(string[] basicDetails, string[] activeHistoryDetails, string curriculumYear,
                                                                                                         string currentAcademicYear, string[] activeHistoryUpdate, string earlierAcademicYear)
        {
            #region Pre-Condition: Create new Year Group for current academic year

            //Login as School Administrator and navigate to Manage Year Groups
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);
            SeleniumHelper.NavigateMenu("Tasks", "School Groups", "Manage Year Groups");

            //Select Academic Year by current academic
            var manageYearGroupTriplet = new ManageYearGroupsTriplet();
            manageYearGroupTriplet.SearchCriteria.AcademicYear = currentAcademicYear;

            //Add new Year Group for current academic
            var manageYearGroupDetail = manageYearGroupTriplet.AddYearGroup();

            //Input basic information
            manageYearGroupDetail.FullName = basicDetails[0];
            manageYearGroupDetail.ShortName = basicDetails[1];
            manageYearGroupDetail.DisplayOrder = basicDetails[2];

            //Input active history with 'Start Date' and 'End Date' is empty
            manageYearGroupDetail.ActiveHistory[0].StartDate = activeHistoryDetails[0];

            //Scroll to Associated Groups
            manageYearGroupDetail.ScrollToAssociatedGroup();
            manageYearGroupDetail.CurriculumYear = curriculumYear;

            //Click Save
            manageYearGroupDetail.Save();

            #endregion Pre-Condition: Create new Year Group for current academic year

            #region Steps

            //Search and Open the specific year group
            manageYearGroupTriplet.Refresh();
            manageYearGroupTriplet.SearchCriteria.AcademicYear = currentAcademicYear;
            var yearGroupListResult = manageYearGroupTriplet.SearchCriteria.Search();
            var yearGroupResult = yearGroupListResult.FirstOrDefault(p => p.FullName.Equals(basicDetails[0]));
            manageYearGroupDetail = yearGroupResult.Click<ManageYearGroupsPage>();

            //In Table 'Active History', enter  into End Date a Valid date prior to the academic Year start date   
            manageYearGroupDetail.ActiveHistory[0].StartDate = activeHistoryUpdate[0];
            manageYearGroupDetail.ActiveHistory[0].EndDate = activeHistoryUpdate[1];

            //Click Save
            manageYearGroupDetail.Save();

            //Confirmation the changes
            var confirmDialog = new ConfirmRequiredDialog();
            manageYearGroupTriplet = confirmDialog.ClickOK<ManageYearGroupsTriplet>();

            //Year Group should no longer appear in Year  group for the current year
            manageYearGroupTriplet.Refresh();
            manageYearGroupTriplet.SearchCriteria.AcademicYear = currentAcademicYear;
            yearGroupResult = manageYearGroupTriplet.SearchCriteria.Search().FirstOrDefault(p => p.FullName.Equals(basicDetails[0]));
            Assert.AreEqual(null, yearGroupResult, "Year group still appears in current academic year");

            //Year Group should still be visible in earlier academic year
            manageYearGroupTriplet.SearchCriteria.AcademicYear = earlierAcademicYear;
            yearGroupResult = manageYearGroupTriplet.SearchCriteria.Search().FirstOrDefault(p => p.FullName.Equals(basicDetails[0]));
            Assert.AreNotEqual(null, yearGroupResult, "Year group doesn't appear in earlier academic year");

            //The Changes to the Year Group are reflected on the Search Options on Screens "Promote Pupils"
            SeleniumHelper.NavigateMenu("Tasks", "School Groups", "Promote Pupils");
            PromotePupilsTriplet promotePupilTriplet = new PromotePupilsTriplet();
            var yearGroups = promotePupilTriplet.SearchCriteria.YearGroups;
            Assert.AreEqual(null, yearGroups[basicDetails[0]], "The deleted year group still display in Promote Pupil page");

            //The Changes to the Year Group are reflected on the Search Options on Screens "Allocate New Intake"
            SeleniumHelper.NavigateMenu("Tasks", "School Groups", "Allocate Future Pupils");
            var newIntake = new AllocateFuturePupilsTriplet();
            yearGroups = newIntake.SearchCriteria.YearGroups;
            Assert.AreEqual(null, yearGroups[basicDetails[0]], "The deleted year group still display in Allocate New Intake page");

            //The Changes to the Year Group are reflected on the Search Options on Screens Allocate Pupils to Groups
            SeleniumHelper.NavigateMenu("Tasks", "School Groups", "Allocate Pupils To Groups");
            var allocatePupilToGroup = new AllocatePupilsToGroupsTriplet();
            yearGroups = allocatePupilToGroup.SearchCriteria.YearGroups;
            Assert.AreEqual(null, yearGroups[basicDetails[0]], "The deleted year group still display in in Allocate Pupils to Groups page");

            #endregion

            #region Post-Condition

            //Back to Year group details page to delete year group was added
            SeleniumHelper.NavigateMenu("Tasks", "School Groups", "Manage Year Groups");
            manageYearGroupTriplet = new ManageYearGroupsTriplet();
            manageYearGroupTriplet.SearchCriteria.AcademicYear = earlierAcademicYear;
            yearGroupListResult = manageYearGroupTriplet.SearchCriteria.Search();
            yearGroupResult = yearGroupListResult.FirstOrDefault(p => p.FullName.Equals(basicDetails[0]));
            manageYearGroupDetail = yearGroupResult.Click<ManageYearGroupsPage>();

            //Delete the year group was added
            manageYearGroupDetail.Delete();

            #endregion Post-Condition :Delete Year Group

        }

        /// <summary>
        /// TC_YG_13_Delete_Year_Group_With_Pupil_Linked_To_Year_Group
        /// Author: Hieu Pham
        /// Description: Delete a Year Group with pupils linked to Year Group (System should Prevent this)
        /// Status : Pending by Bug #14 : [PUPIL RECORD] Year Group record appear incorrectly in Add Pupil dialog 's 'Year Group' dropdown.
        /// </summary>
        [WebDriverTest(TimeoutSeconds = 800, Enabled = false, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { "g1" }, DataProvider = "TC_YG013_Data")]
        public void TC_YG013_Delete_Year_Group_With_Pupil_Linked_To_Year_Group(string academicYear, string[] basicDetails, string[] activeHistoryDetails, string curriculumYear,
            string[] classDetails, string[] headOfYearDetails, string[] staffDetails, string[] pupil)
        {

            #region Pre-Condition: Create new Year Group and Attach Year Group for a pupil

            //Login as School Administrator and navigate to Manage Year Groups
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);
            SeleniumHelper.NavigateMenu("Tasks", "School Groups", "Manage Year Groups");

            //Select Academic Year by current academic
            var manageYearGroupTriplet = new ManageYearGroupsTriplet();
            manageYearGroupTriplet.SearchCriteria.AcademicYear = academicYear;

            //Add new Year Group for current academic
            var manageYearGroupDetail = manageYearGroupTriplet.AddYearGroup();

            //Input basic information
            manageYearGroupDetail.FullName = basicDetails[0];
            manageYearGroupDetail.ShortName = basicDetails[1];
            manageYearGroupDetail.DisplayOrder = basicDetails[2];

            //Input active history with 'Start Date' and 'End Date' is empty
            manageYearGroupDetail.ActiveHistory[0].StartDate = activeHistoryDetails[0];

            //Scroll to Associated Groups
            manageYearGroupDetail.ScrollToAssociatedGroup();

            //Input information to Associated Groups with null value into 'End Date'
            manageYearGroupDetail.CurriculumYear = curriculumYear;
            manageYearGroupDetail.Classes[0].Class = classDetails[0];
            manageYearGroupDetail.Classes[0].StartDate = classDetails[1];

            //Scroll to Staff Details
            manageYearGroupDetail.ScrollToStaffDetails();

            //Input information to Head of Year grid with null value into 'End Date'
            manageYearGroupDetail.HeadOfYear[0].HeadOfYear = headOfYearDetails[0];
            manageYearGroupDetail.HeadOfYear[0].StartDate = headOfYearDetails[1];

            //Input information to Staff grid with null value into 'End Date'
            manageYearGroupDetail.Staff[0].Staff = staffDetails[0];
            manageYearGroupDetail.Staff[0].PastoralRole = staffDetails[1];
            manageYearGroupDetail.Staff[0].StartDate = staffDetails[2];

            //Click Save
            manageYearGroupDetail.Save();

            // Verify save successfully
            Assert.AreEqual(true, manageYearGroupDetail.IsMessageSuccessAppear(), "Create new record unsuccessfully");

            // Delete pupil if existed
            SeleniumHelper.NavigateMenu("Tasks", "Pupils", "Delete Pupil");
            var deletePupilRecordTriplet = new DeletePupilRecordTriplet();
            deletePupilRecordTriplet.SearchCriteria.PupilName = String.Format("{0}, {1}", pupil[1], pupil[0]);
            var resultPupils = deletePupilRecordTriplet.SearchCriteria.Search();
            var pupilSearchTile = resultPupils.FirstOrDefault(t => t.Name.Contains(String.Format("{0}, {1}", pupil[1], pupil[0])));
            var deletePupilRecord = pupilSearchTile == null ? new DeletePupilRecordPage() : pupilSearchTile.Click<DeletePupilRecordPage>();
            deletePupilRecord.Delete();

            // Add new pupil
            // Navigate to Pupil Record
            SeleniumHelper.NavigateMenu("Tasks", "Pupils", "Pupil Records");

            var pupilRecordTriplet = new PupilRecordTriplet();
            var addNewPupilDialog = pupilRecordTriplet.AddNewPupil();

            // Enter values
            addNewPupilDialog.Forename = pupil[0];
            addNewPupilDialog.SurName = pupil[1];
            addNewPupilDialog.Gender = pupil[2];
            addNewPupilDialog.DateOfBirth = pupil[3];
            var registrationDetailDialog = addNewPupilDialog.Continue();
            registrationDetailDialog.DateOfAdmission = pupil[4];
            registrationDetailDialog.YearGroup = basicDetails[0];
            registrationDetailDialog.CreateRecord();

            // Save values
            var pupilRecord = PupilRecordPage.Create();
            pupilRecord.SavePupil();

            #endregion

            #region Steps:

            // Navigate to Manage Year Groups
            SeleniumHelper.NavigateMenu("Tasks", "School Groups", "Manage Year Groups");

            // Search 
            var manageYearGroupsTriplet = new ManageYearGroupsTriplet();
            manageYearGroupsTriplet.SearchCriteria.AcademicYear = academicYear;
            var yearGroupResult = manageYearGroupsTriplet.SearchCriteria.Search();
            manageYearGroupDetail = yearGroupResult.FirstOrDefault(x => x.FullName.Trim().Equals(basicDetails[0])).Click<ManageYearGroupsPage>();

            // Scroll to Associate Groups and Click Icon 'Delete this Row' (Dustbin)
            manageYearGroupDetail.ScrollToAssociatedGroup();
            var classesTable = manageYearGroupDetail.Classes;
            classesTable.Rows.FirstOrDefault(x => x.Class.Trim().Equals(classDetails[0]) && x.StartDate.Trim().Equals(classDetails[1])).DeleteRow();

            // Scroll to Staff Detail and delete rows
            manageYearGroupDetail.Refresh();
            manageYearGroupDetail.ScrollToStaffDetails();

            // Delete head of year rows
            manageYearGroupDetail.HeadOfYear[0].DeleteRow();

            // Delete Staff rows
            manageYearGroupDetail.Refresh();
            manageYearGroupDetail.Staff[0].DeleteRow();

            // Save
            manageYearGroupDetail.Save();

            // Verify Save successfully
            Assert.AreEqual(true, manageYearGroupDetail.IsMessageSuccessAppear(), "Save unsuccessfully");

            // Delete Year Group
            manageYearGroupDetail.Refresh();
            manageYearGroupDetail.Delete();

            // VP : Deletion of Year Group successfully prevented.
            Assert.AreEqual(true, manageYearGroupDetail.IsWarningDeleteAppear(), "Delete is success");

            #endregion

            #region Post-Condition: Delete pupil record

            // Delete pupil if existed
            SeleniumHelper.NavigateMenu("Tasks", "Pupils", "Delete Pupil");
            deletePupilRecordTriplet = new DeletePupilRecordTriplet();
            deletePupilRecordTriplet.SearchCriteria.PupilName = String.Format("{0}, {1}", pupil[1], pupil[0]);
            resultPupils = deletePupilRecordTriplet.SearchCriteria.Search();
            pupilSearchTile = resultPupils.FirstOrDefault(t => t.Name.Contains(String.Format("{0}, {1}", pupil[1], pupil[0])));
            deletePupilRecord = pupilSearchTile == null ? new DeletePupilRecordPage() : pupilSearchTile.Click<DeletePupilRecordPage>();
            deletePupilRecord.Delete();

            #endregion

        }

        /// <summary>
        /// Author: Ba.Truong
        /// Desscription: Delete a Year Group with head staff linked to Year Group (System should Prevent this)
        /// </summary>
        [WebDriverTest(TimeoutSeconds = 2000, Enabled = false, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { "g1" }, DataProvider = "TC_YG014_Data")]
        public void TC_YG014_Delete_Year_Group_With_Head_Staff_Linked_To_Year_Group(string[] basicDetails, string[] activeHistoryDetails,
                                                                                    string curriculumYear, string[] staffDetails,
                                                                                    string currentAcademicYear)
        {
            #region Pre-Condition: Create a year group and linked a head staff to it

            //Login as School Administrator and navigate to Manage Year Groups
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);
            SeleniumHelper.NavigateMenu("Tasks", "School Groups", "Manage Year Groups");

            //Select Academic Year by current academic
            var manageYearGroupTriplet = new ManageYearGroupsTriplet();
            manageYearGroupTriplet.SearchCriteria.AcademicYear = currentAcademicYear;

            //Add new Year Group for current academic
            var manageYearGroupDetail = manageYearGroupTriplet.AddYearGroup();

            //Input basic information
            manageYearGroupDetail.FullName = basicDetails[0];
            manageYearGroupDetail.ShortName = basicDetails[1];
            manageYearGroupDetail.DisplayOrder = basicDetails[2];

            //Input active history
            manageYearGroupDetail.ActiveHistory[0].StartDate = activeHistoryDetails[0];

            //Input information to Associated Groups
            manageYearGroupDetail.ScrollToAssociatedGroup();
            manageYearGroupDetail.CurriculumYear = curriculumYear;

            //Scroll to Staff Details
            manageYearGroupDetail.ScrollToStaffDetails();

            //Input information to Staff grid
            manageYearGroupDetail.HeadOfYear[0].HeadOfYear = staffDetails[0];
            manageYearGroupDetail.HeadOfYear[0].StartDate = staffDetails[1];
            manageYearGroupDetail.HeadOfYear[0].EndDate = staffDetails[2];

            //Click Save
            manageYearGroupDetail.Save();

            #endregion Pre-Condition

            #region Steps

            //Search and Open the specific year group
            manageYearGroupTriplet = new ManageYearGroupsTriplet();
            manageYearGroupTriplet.SearchCriteria.AcademicYear = currentAcademicYear;
            var yearGroupListResult = manageYearGroupTriplet.SearchCriteria.Search();
            var yearGroupResult = yearGroupListResult.FirstOrDefault(p => p.FullName.Equals(basicDetails[0]));
            manageYearGroupDetail = yearGroupResult.Click<ManageYearGroupsPage>();

            //Click Delete
            manageYearGroupDetail.Delete();

            //Confirm the warining message appears
            manageYearGroupDetail.Refresh();
            Assert.AreEqual(true, manageYearGroupDetail.IsWarningDeleteAppear(), "Deleting a Year Group with head staff linked to Year Group is not be prevented");

            #endregion

            #region Post-Condition

            //Remove the staff that was linked
            manageYearGroupDetail.HeadOfYear[0].DeleteRow();

            //Delete the year group was added
            manageYearGroupDetail.Delete();

            #endregion

        }

        #region DATA

        public List<object[]> TC_YG001_Data()
        {
            string futureAcademicYear = String.Format("Academic Year {0}/{1}", (DateTime.Now.Year).ToString(), (DateTime.Now.Year + 1).ToString());
            var data = new List<Object[]>
            {    
                new object[]{
                    new string[] {String.Format("YG01 {0}", SeleniumHelper.GenerateRandomString(4)), SeleniumHelper.GenerateRandomString(10), "30"},
                    new string[] {(DateTime.Now.ToString("M/d/yyyy")),
                                  (DateTime.Now.AddYears(1)).ToString("M/d/yyyy")},
                    "Curriculum Year R",
                    new string[] {"SEN", (DateTime.Now.ToString("M/d/yyyy")),
                                  (DateTime.Now.AddYears(1)).ToString("M/d/yyyy")},
                    new string[] {"Brooks, Colm", (DateTime.Now.ToString("M/d/yyyy")),
                                  (DateTime.Now.AddYears(1)).ToString("M/d/yyyy")},
                    new string[] {"Brooks, Colm", "Supervisor",
                                  (DateTime.Now.ToString("M/d/yyyy")),
                                  (DateTime.Now.AddYears(1)).ToString("M/d/yyyy")},
                    futureAcademicYear
                }
            };
            return data;
        }

        public List<object[]> TC_YG002_Data()
        {
            string futureAcademicYear = String.Format("Academic Year {0}/{1}", (DateTime.Now.Year).ToString(), (DateTime.Now.Year + 1).ToString());

            var data = new List<Object[]>
            {    
                new object[]{
                    new string[] {String.Format("YG02 {0}", SeleniumHelper.GenerateRandomString(4)), SeleniumHelper.GenerateRandomString(10), "30"},
                    new string[] {(DateTime.Now.ToString("M/d/yyyy")),
                                  (DateTime.Now.AddYears(1)).ToString("M/d/yyyy")},
                    "Curriculum Year R",
                    new string[] {String.Format("YG02 {0}_Edit", SeleniumHelper.GenerateRandomString(4)),String.Format("{0}_Update", SeleniumHelper.GenerateRandomString(3)),"30"},
                    new string[] {(DateTime.Now.ToString("M/d/yyyy")),
                                  (DateTime.Now.AddYears(2).ToString("M/d/yyyy"))},
                    "Curriculum Year 1",
                    new string[] {"Nursery", (DateTime.Now.ToString("M/d/yyyy")),
                                 (DateTime.Now.AddYears(1).ToString("M/d/yyyy"))},
                    new string[] {"Brown, Pauline", (DateTime.Now.ToString("M/d/yyyy")),
                                 (DateTime.Now.AddYears(1).ToString("M/d/yyyy"))},
                    new string[] {"Grosvenor, Gillian", "Head of Year",
                                  (DateTime.Now.ToString("M/d/yyyy")),
                                  (DateTime.Now.AddYears(1).ToString("M/d/yyyy"))},
                    futureAcademicYear
                }
            };
            return data;
        }

        public List<object[]> TC_YG003_Data()
        {
            string currentAcademicYear = String.Format("Academic Year {0}/{1}", DateTime.Now.Year.ToString(), (DateTime.Now.Year + 1).ToString());

            var data = new List<Object[]>
            {    
                new object[]{
                    new string[] {String.Format("YG03 {0}", SeleniumHelper.GenerateRandomString(4)), SeleniumHelper.GenerateRandomString(10), "30"},
                    new string[] {(DateTime.Now.ToString("M/d/yyyy"))},
                    "Curriculum Year R",
                    new string[] {"SEN", (DateTime.Now.ToString("M/d/yyyy"))},
                    new string[] {"Brooks, Colm", (DateTime.Now.ToString("M/d/yyyy"))},
                    new string[] {"Brennan, Teagan", "Deputy Head",
                                 (DateTime.Now.ToString("M/d/yyyy"))},
                    currentAcademicYear
                }
            };
            return data;
        }

        public List<object[]> TC_YG004_Data()
        {
            string currentAcademicYear = String.Format("Academic Year {0}/{1}", DateTime.Now.Year.ToString(), (DateTime.Now.Year + 1).ToString());

            var data = new List<Object[]>
            {    
                new object[]{
                    new string[] {String.Format("YG04 {0}", SeleniumHelper.GenerateRandomString(4)), SeleniumHelper.GenerateRandomString(10), "30"},
                    new string[] {(DateTime.Now.ToString("M/d/yyyy"))},
                    "Curriculum Year R",
                    new string[] {String.Format("YG04 {0}_Edit", SeleniumHelper.GenerateRandomString(4)),String.Format("{0}_Update", SeleniumHelper.GenerateRandomString(3)),"30"},
                    new string[] {(DateTime.Now.ToString("M/d/yyyy")),
                                  (DateTime.Now.AddYears(2)).ToString("M/d/yyyy")},
                    "Curriculum Year 1",
                    new string[] {"SEN", (DateTime.Now.ToString("M/d/yyyy")), 
                                  (DateTime.Now.AddYears(2)).ToString("M/d/yyyy")},
                    new string[] {"Brown, Pauline", (DateTime.Now.ToString("M/d/yyyy")), 
                                  (DateTime.Now.AddYears(2)).ToString("M/d/yyyy")},
                    new string[] {"Grosvenor, Gillian", "Head of Year",
                                 (DateTime.Now.ToString("M/d/yyyy")), 
                                 (DateTime.Now.AddYears(2)).ToString("M/d/yyyy")},
                    currentAcademicYear
                }
            };
            return data;
        }

        public List<object[]> TC_YG005_Data()
        {
            string currentAcademicYear = String.Format("Academic Year {0}/{1}", DateTime.Now.Year.ToString(), (DateTime.Now.Year + 1).ToString());

            var data = new List<Object[]>
            {    
                new object[]{
                    new string[] {String.Format("YG05 {0}", SeleniumHelper.GenerateRandomString(4)), SeleniumHelper.GenerateRandomString(10), "30"},
                    new string[] {(DateTime.Now.ToString("M/d/yyyy")),
                                  (DateTime.Now.AddYears(1)).ToString("M/d/yyyy")},
                    "Curriculum Year R",
                    new string[] {"SEN", (DateTime.Now.ToString("M/d/yyyy")),
                                  (DateTime.Now.AddYears(1)).ToString("M/d/yyyy")},
                    new string[] {"Brooks, Colm", (DateTime.Now.ToString("M/d/yyyy")),
                                  (DateTime.Now.AddYears(1)).ToString("M/d/yyyy")},
                    new string[] {"Brennan, Teagan", "Deputy Head",
                                  (DateTime.Now.ToString("M/d/yyyy")),
                                 (DateTime.Now.AddYears(1)).ToString("M/d/yyyy")},
                    currentAcademicYear
                  }
             };
            return data;
        }

        public List<object[]> TC_YG006_Data()
        {
            string currentAcademicYear = String.Format("Academic Year {0}/{1}", DateTime.Now.Year.ToString(), (DateTime.Now.Year + 1).ToString());

            var data = new List<Object[]>
            {    
                new object[]{
                    new string[] {String.Format("YG06 {0}", SeleniumHelper.GenerateRandomString(4)), SeleniumHelper.GenerateRandomString(10), "30"},
                    new string[] {(new DateTime(DateTime.Today.Year, 8, 1)).ToString("M/d/yyyy"),
                                  (new DateTime(DateTime.Today.Year + 1, 7, 31)).ToString("M/d/yyyy")},
                    "Curriculum Year R",
                    new string[] {String.Format("YG06 {0}_Edit", SeleniumHelper.GenerateRandomString(4)), String.Format("{0}_Update", SeleniumHelper.GenerateRandomString(3)),"30"},
                    new string[] {(new DateTime(DateTime.Today.Year, 8, 1)).ToString("M/d/yyyy"),
                                  (new DateTime(DateTime.Today.Year + 2, 7, 31)).ToString("M/d/yyyy")},
                    "Curriculum Year 1",
                    new string[] {"Nursery", (new DateTime(DateTime.Today.Year, 8, 1)).ToString("M/d/yyyy"),
                                  (new DateTime(DateTime.Today.Year + 2, 7, 31)).ToString("M/d/yyyy")},
                    new string[] {"Brown, Pauline", (new DateTime(DateTime.Today.Year, 8, 1)).ToString("M/d/yyyy"),
                                  (new DateTime(DateTime.Today.Year + 2, 7, 31)).ToString("M/d/yyyy")},
                    new string[] {"Grosvenor, Gillian", "Head of Year",
                                  (new DateTime(DateTime.Today.Year, 8, 1)).ToString("M/d/yyyy"),
                                  (new DateTime(DateTime.Today.Year + 2, 7, 31)).ToString("M/d/yyyy")},
                    currentAcademicYear
                }
            };
            return data;
        }

        public List<object[]> TC_YG007_Data()
        {
            var theFromDate = new DateTime(DateTime.Today.Year, 8, 1);
            string currentAcademicYear = String.Format("Academic Year {0}/{1}", DateTime.Now.Year.ToString(), (DateTime.Now.Year + 1).ToString());
            string futureAcademicYear = String.Format("Academic Year {0}/{1}", (DateTime.Now.Year + 1).ToString(), (DateTime.Now.Year + 2).ToString());
            var data = new List<Object[]>
            {    
                new object[]{
                    new string[] {String.Format("YG07 {0}", SeleniumHelper.GenerateRandomString(4)), SeleniumHelper.GenerateRandomString(10), "30"},
                    new string[] {theFromDate.ToString("M/d/yyyy")},
                    "Curriculum Year R",
                    new string[] {"SEN", theFromDate.ToString("M/d/yyyy")},
                    new string[] {"Brooks, Colm", theFromDate.ToString("M/d/yyyy")},
                    new string[] {"Brennan, Teagan", "Head of Year",
                                  theFromDate.ToString("M/d/yyyy")},
                    currentAcademicYear,        
                    futureAcademicYear,
                    (new DateTime(DateTime.Today.Year+1, 7, 31)).ToString("M/d/yyyy")
                    

                }
            };
            return data;
        }

        public List<object[]> TC_YG008_Data()
        {
            string futureAcademicYear = String.Format("Academic Year {0}/{1}", (DateTime.Now.Year + 1).ToString(), (DateTime.Now.Year + 2).ToString());
            var theFromDate = new DateTime(DateTime.Today.Year, 8, 1);

            string classFullName = SeleniumHelper.GenerateRandomString(3);
            string classShortName = "SN " + SeleniumHelper.GenerateRandomString(5);
            string displayOrder = "1";

            string pattern = "M/d/yyyy";
            var activeHistoryStartDate = new DateTime(DateTime.Today.Year, 11, 12).ToString(pattern);
            var activeHistoryEndDate = new DateTime(DateTime.Today.Year, 12, 07).ToString(pattern);

            var data = new List<Object[]>
            {    
                new object[]{

                    new string [] {classFullName, classShortName, displayOrder},
                    new string [] {activeHistoryStartDate.ToString(),activeHistoryEndDate.ToString()},

                    new string[] {String.Format("YG08 {0}", SeleniumHelper.GenerateRandomString(4)), SeleniumHelper.GenerateRandomString(10), "30"},
                    new string[] {theFromDate.ToString("M/d/yyyy")},
                    "Curriculum Year R",
                    new string[] {"SEN", theFromDate.ToString("M/d/yyyy")},
                    new string[] {"Brooks, Colm", theFromDate.ToString("M/d/yyyy")},
                    new string[] {"Brennan, Teagan", "Head of Year",
                                  theFromDate.ToString("M/d/yyyy")},
                    futureAcademicYear,                    
                }
            };
            return data;
        }

        public List<object[]> TC_YG009_Data()
        {
            string pattern = "M/d/yyyy";
            string fullName = String.Format("{0} {1}_{2}", "Year", "Avn", SeleniumHelper.GenerateRandomString(8));
            string shortName = String.Format("{0}{1}", "YA", SeleniumHelper.GenerateRandomString(4));
            string displayOrder = "1";
            string startDate = DateTime.ParseExact("08/01/2008", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            string endDate = SeleniumHelper.GetFinishDateAcademicYear(DateTime.Now.AddYears(1)).ToString(pattern);
            string currentAcademic = SeleniumHelper.GetAcademicYear(DateTime.Now);
            string futureAcademic = SeleniumHelper.GetAcademicYear(DateTime.Now.AddYears(1));
            var res = new List<Object[]>
            {                
                new object[] 
                {
                    fullName, shortName, displayOrder, startDate, "Curriculum Year 7", currentAcademic, futureAcademic, endDate,
                }
            };
            return res;
        }

        public List<object[]> TC_YG010_Data()
        {
            string pattern = "M/d/yyyy";
            string fullName = String.Format("{0} {1}_{2}", "Year", "Avn", SeleniumHelper.GenerateRandomString(8));
            string shortName = String.Format("{0}{1}", "YA", SeleniumHelper.GenerateRandomString(4));
            string displayOrder = "1";
            string startDate = DateTime.ParseExact("08/01/2008", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            string endDate = SeleniumHelper.GetFinishDateAcademicYear(DateTime.Now).ToString(pattern);
            string currentAcademic = SeleniumHelper.GetAcademicYear(DateTime.Now);
            string beforeAcademicYear = SeleniumHelper.GetAcademicYear(DateTime.Now.Subtract(TimeSpan.FromDays(365)));
            var res = new List<Object[]>
            {                
                new object[] 
                {
                    fullName, shortName, displayOrder, 
                    startDate, "Curriculum Year 7", "7A", 
                    beforeAcademicYear, currentAcademic, endDate,
                }
            };
            return res;
        }

        public List<object[]> TC_YG011_Data()
        {
            string currentAcademicYear = String.Format("Academic Year {0}/{1}", (DateTime.Now.Year).ToString(), (DateTime.Now.Year + 1).ToString());
            var theFromDate = new DateTime(DateTime.Today.Year, 8, 1);

            string classFullName = SeleniumHelper.GenerateRandomString(3);
            string classShortName = "SN " + SeleniumHelper.GenerateRandomString(5);
            string displayOrder = "1";

            string pattern = "M/d/yyyy";
            var activeHistoryStartDate = new DateTime(DateTime.Today.Year, 11, 12).ToString(pattern);
            var activeHistoryEndDate = new DateTime(DateTime.Today.Year, 12, 07).ToString(pattern);

            var data = new List<Object[]>
            {    
                new object[]{   
                    new string [] {classFullName, classShortName, displayOrder},
                    new string[] {activeHistoryStartDate.ToString(),activeHistoryEndDate.ToString()},
                    new string[] {String.Format("YG11 {0}", SeleniumHelper.GenerateRandomString(4)), SeleniumHelper.GenerateRandomString(10), "30"},
                    new string[] {theFromDate.ToString("M/d/yyyy")},
                    "Curriculum Year R",
                    new string[] {"SEN", theFromDate.ToString("M/d/yyyy")},
                    new string[] {"Brooks, Colm", theFromDate.ToString("M/d/yyyy")},
                    new string[] {"Brennan, Teagan", "Head of Year",
                                  theFromDate.ToString("M/d/yyyy")},
                    currentAcademicYear,                    
                }
            };
            return data;
        }

        public List<object[]> TC_YG012_Data()
        {
            string currentAcademicYear = String.Format("Academic Year {0}/{1}", (DateTime.Now.Year).ToString(), (DateTime.Now.Year + 1).ToString());
            string earlierAcademicYear = String.Format("Academic Year {0}/{1}", (DateTime.Now.Year - 1).ToString(), (DateTime.Now.Year).ToString());

            var data = new List<Object[]>
            {    
                new object[]{
                    new string[] {String.Format("YG12 {0}", SeleniumHelper.GenerateRandomString(4)), SeleniumHelper.GenerateRandomString(10) ,"30"},
                    new string[] {(new DateTime(DateTime.Today.Year, 8, 1)).ToString("M/d/yyyy")},
                    "Curriculum Year R",
                    currentAcademicYear, 
                    new string[] {(new DateTime(DateTime.Today.Year - 1, 8, 1)).ToString("M/d/yyyy"), (new DateTime(DateTime.Today.Year - 1, 10, 10)).ToString("M/d/yyyy")}, 
                    earlierAcademicYear               
                }
            };
            return data;
        }

        public List<object[]> TC_YG013_Data()
        {

            string pattern = "M/d/yyyy";
            string currentAcademicYear = String.Format("Academic Year {0}/{1}", (DateTime.Now.Year).ToString(), (DateTime.Now.Year + 1).ToString());
            string fullName = "Logigear_" + SeleniumHelper.GenerateRandomString(8);
            string shortName = SeleniumHelper.GenerateRandomString(10);

            // Pupil Data
            string randomString = SeleniumHelper.GenerateRandomString(8);
            string surName = "SUR_" + randomString;
            string foreName = "FORE_" + randomString;
            string gender = "Male";
            string dateOfBirth = DateTime.ParseExact("10/16/2006", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            string DateOfAdmission = DateTime.ParseExact("10/19/2010", pattern, CultureInfo.InvariantCulture).ToString(pattern);

            var data = new List<Object[]>
            {    
                new object[]{ currentAcademicYear,
                    new string[] {fullName, shortName, "1"},
                    new string[] {(new DateTime(DateTime.Today.Year, 8, 1)).ToString("M/d/yyyy")},
                    "Curriculum Year R",
                    new string[] {"SEN", (new DateTime(DateTime.Today.Year, 8, 1)).ToString("M/d/yyyy")},
                    new string[] {"Brooks, Colm", (new DateTime(DateTime.Today.Year, 8, 1)).ToString("M/d/yyyy")},
                    new string[] {"Brennan, Teagan", "Head of Year",
                                  (new DateTime(DateTime.Today.Year, 8, 1)).ToString("M/d/yyyy")}, 
                    new string[] {foreName, surName, gender, dateOfBirth, DateOfAdmission}              
                }
            };
            return data;
        }

        public List<object[]> TC_YG014_Data()
        {
            string currentAcademicYear = String.Format("Academic Year {0}/{1}", (DateTime.Now.Year).ToString(), (DateTime.Now.Year + 1).ToString());

            var data = new List<Object[]>
            {    
                new object[]{
                    new string[] {String.Format("YG14 {0}", SeleniumHelper.GenerateRandomString(4)), SeleniumHelper.GenerateRandomString(10), "30"},
                    new string[] {(new DateTime(DateTime.Today.Year, 8, 1)).ToString("M/d/yyyy")},
                    "Curriculum Year R",
                    new string[] {"Brennan, Teagan", (new DateTime(DateTime.Today.Year, 8, 1)).ToString("M/d/yyyy"),
                                  (new DateTime(DateTime.Today.Year + 1, 7, 31)).ToString("M/d/yyyy")},
                    currentAcademicYear,                    
                }
            };
            return data;
        }

        #endregion
    }
}
