using NUnit.Framework;
using POM.Components.SchoolGroups;
using POM.Helper;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using TestSettings;
using WebDriverRunner.internals;
using POM.Components.Staff;
using POM.Components.Pupil;
using Selene.Support.Attributes;
using SeSugar.Data;
using SeSugar;
using Facilities.Data;

namespace Faclities.LogigearTests
{
    public class UserDefinedGroupsTests
    {
        /// <summary>
        /// TC_UDG01
        /// Au : Hieu Pham
        /// Description: Carry out Searches using the Search Filter fields to Target results and to ensure the Search function returns correct results
        /// Role: School Adminstrator
        /// Status: PASS
        /// </summary>
        [WebDriverTest(TimeoutSeconds = 2000, Enabled = true, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { "g1" , "P2" }, DataProvider = "TC_UDG01_DATA")]
        public void TC_UDG01_Carry_Out_Seaches_Using_Search_Filter(string groupFullName, string groupShortName, string purpose, bool visibility, string groupFullNameNoAvailable)
        {

            #region Steps

            // Login
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);

            // Navigate to Manage User Defined Group
            SeleniumHelper.NavigateMenu("Tasks", "School Groups", "Manage User Defined Groups");

            // Search with Group Full Name
            var manageUserDefinedTriplet = new ManageUserDefinedTriplet();
            manageUserDefinedTriplet.SearchCriteria.FullName = groupFullName;
            var searchResult = manageUserDefinedTriplet.SearchCriteria.Search().FirstOrDefault(x => x.GroupName.Equals(groupFullName));

            // VP : Search result displays returned match the expected results.
            Assert.AreNotEqual(null, searchResult, "Search for 'Group Full Name' filter is not correct.");

            // Search with Group Short Name
            manageUserDefinedTriplet.Refresh();
            manageUserDefinedTriplet.SearchCriteria.FullName = String.Empty;
            manageUserDefinedTriplet.SearchCriteria.ShortName = groupShortName;
            searchResult = manageUserDefinedTriplet.SearchCriteria.Search().FirstOrDefault(x => x.GroupName.Equals(groupFullName));
            var manageUserDefinedPage = searchResult.Click<ManageUserDefinedPage>();

            // VP : Search result displays returned match the expected results. The detail page has correct group short name.
            Assert.AreEqual(groupShortName, manageUserDefinedPage.ShortName, "Search for 'Group Short Name' filter is not correct.");

            // Search with purpose
            manageUserDefinedTriplet.Refresh();
            manageUserDefinedTriplet.SearchCriteria.ShortName = String.Empty;
            manageUserDefinedTriplet.SearchCriteria.Purpose = purpose;
            var searchResults = manageUserDefinedTriplet.SearchCriteria.Search().All(x => x.Purpose.Equals(purpose));

            // VP : Search result displays returned match the expected results. (All is the same purpose)
            Assert.AreNotEqual(null, searchResults, "Search for 'Purpose' filter is not correct.");

            // Search with visibility
            manageUserDefinedTriplet.Refresh();

            // Clear dropdown
            manageUserDefinedTriplet.SearchCriteria.Purpose = null;

            // Enter value for purpose
            manageUserDefinedTriplet.SearchCriteria.Visibility = visibility;
            searchResults = manageUserDefinedTriplet.SearchCriteria.Search().All(x => x.Visibility.Equals(visibility));

            // VP : Search result displays returned match the expected results. (All is the same visibility)
            Assert.AreNotEqual(null, searchResults, "Search for 'Visibility' filter is not correct.");

            // Search with unavailable Group Full Name
            manageUserDefinedTriplet.Refresh();
            manageUserDefinedTriplet.SearchCriteria.FullName = groupFullNameNoAvailable;
            searchResult = manageUserDefinedTriplet.SearchCriteria.Search().FirstOrDefault(x => x.GroupName.Equals(groupFullNameNoAvailable));

            // VP : Search result displays returned match the expected results. (No result)
            Assert.AreEqual(null, searchResult, "Search for 'Group Full Name' filter with result is 'No result' is not correct.");

            #endregion

        }

        /// <summary>
        /// TC_UDG02
        /// Au : Hieu Pham
        /// Description: Verify that the 'Search for a Supervisor' function works correctly by using the Search Filter fields to Target results and to ensure the Search function returns correct results
        /// Role: School Adminstrator
        /// Status: PASS
        /// </summary>
        [WebDriverTest(TimeoutSeconds = 2000, Enabled = true, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { "g1" , "P2"}, DataProvider = "TC_UDG02_DATA")]
        public void TC_UDG02_Search_Supervisor_Correct(string surName, string foreName, string role, string unavailableSurname)
        {

            #region Steps

            // Login
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);

            // Navigate to Manage User Defined Group
            SeleniumHelper.NavigateMenu("Tasks", "School Groups", "Manage User Defined Groups");

            // Search with Group Full Name
            var manageUserDefinedTriplet = new ManageUserDefinedTriplet();

            // Click button Create
            var manageUserDefinedPage = manageUserDefinedTriplet.ClickCreateNew();

            // Scroll to supervisor
            manageUserDefinedPage.ScrollToSupervisor();

            // Click add supervisor
            var addSupervisorDialogTriplet = manageUserDefinedPage.ClickAddSupervisor();

            DataPackage dataPackage = this.BuildDataPackage();

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
                // Enter surname
                addSupervisorDialogTriplet.SearchCriteria.SurName = surname;
                addSupervisorDialogTriplet.SearchCriteria.Role = null;
                var addSupervisorDialogPage = addSupervisorDialogTriplet.SearchCriteria.Search<AddSupervisorsDialogDetail>();

                // VP : Search result displays returned match the expected results. 
                Assert.AreEqual(true, addSupervisorDialogPage.SearchResultName.All(x => x.Contains(surname)), "Search for surname is not correct.");

                // Enter forename
                addSupervisorDialogTriplet.Refresh();
                addSupervisorDialogTriplet.SearchCriteria.SurName = String.Empty;
                addSupervisorDialogTriplet.SearchCriteria.ForeName = forename;
                addSupervisorDialogTriplet.SearchCriteria.Role = null;
                addSupervisorDialogPage = addSupervisorDialogTriplet.SearchCriteria.Search<AddSupervisorsDialogDetail>();

                // VP : Search result displays returned match the expected results. 
                Assert.AreEqual(true, addSupervisorDialogPage.SearchResultName.All(x => x.Contains(forename)), "Search for forename is not correct.");

                // Enter Role 
                addSupervisorDialogTriplet.Refresh();
                addSupervisorDialogTriplet.SearchCriteria.ForeName = String.Empty;
                addSupervisorDialogTriplet.SearchCriteria.Role = role;
                addSupervisorDialogPage = addSupervisorDialogTriplet.SearchCriteria.Search<AddSupervisorsDialogDetail>();

                // VP : Search result displays returned match the expected results. 
                Assert.AreNotEqual(0, addSupervisorDialogPage.SearchResultName.Count, "Search for role is not correct.");

                // Enter unavailable surname
                addSupervisorDialogTriplet.Refresh();
                addSupervisorDialogTriplet.SearchCriteria.ForeName = String.Empty;
                addSupervisorDialogTriplet.SearchCriteria.Role = null;
                addSupervisorDialogTriplet.SearchCriteria.SurName = unavailableSurname;
                addSupervisorDialogPage = addSupervisorDialogTriplet.SearchCriteria.Search<AddSupervisorsDialogDetail>();

                // VP : Search result displays returned match the expected results. 
                Assert.AreEqual(0, addSupervisorDialogPage.SearchResultName.Count, "Search for unavailable surname is not correct.");

                #endregion
            }
        }

        /// <summary>
        /// TC_UDG03
        /// Au : Hieu Pham
        /// Description: Verify that the 'Search for a Pupil' function works correctly by using the Search Filter fields to Target results and to ensure the Search function returns correct results
        /// Role: School Adminstrator
        /// Status: PASS
        /// </summary>
        [WebDriverTest(TimeoutSeconds = 2000, Enabled = true, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { "g1" , "P2" }, DataProvider = "TC_UDG03_DATA")]
        public void TC_UDG03_Search_Add_Pupil_Correct(string pupilName, string yearGroup, string className, string unavailablePupilName, string surNameCurrent,
            string foreNameCurrent, string surNameFuture, string foreNameFuture, string surNameLeaver, string foreNameLeaver, string gender, string dateOfBirth,
            string dateOfAdmissionCurrent, string dateOfAdmissionFuture, string dateOfLeaving, string reasonForLeaving)
        {

            #region Precondition

            // Login
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);

            // Navigate to pupil record
           
            DataPackage dataPackage = this.BuildDataPackage();

            //Pupil Data
            string pattern = "M/d/yyyy";
          
            string forename = "SelPersonal" + SeleniumHelper.GenerateRandomString(5) + SeleniumHelper.GenerateRandomNumberUsingDateTime();
            string surname = "SelPersonal" + SeleniumHelper.GenerateRandomString(5) + SeleniumHelper.GenerateRandomNumberUsingDateTime();
            DateTime dob = DateTime.ParseExact("10/16/2006", pattern, CultureInfo.InvariantCulture);
            var dateOfAdmission = DateTime.ParseExact("10/19/2010", pattern, CultureInfo.InvariantCulture);
            var learnerId = Guid.NewGuid();
            var pupil = this.BuildDataPackage()
                        .AddBasicLearner(learnerId, surname, forename, dob, dateOfAdmission);

            string forenameFuture = "SelPersonal" + SeleniumHelper.GenerateRandomString(5) + SeleniumHelper.GenerateRandomNumberUsingDateTime();
            string surnameFuture = "SelPersonal" + SeleniumHelper.GenerateRandomString(5) + SeleniumHelper.GenerateRandomNumberUsingDateTime();
            DateTime dobFuture = DateTime.ParseExact("10/16/2006", pattern, CultureInfo.InvariantCulture);
            var dateofAdmissionFuture = DateTime.ParseExact(SeleniumHelper.GetDayAfter(SeleniumHelper.GetToDay(), 7), pattern, CultureInfo.InvariantCulture);
            var learnerIdFuture = Guid.NewGuid();
            var pupilFuture = this.BuildDataPackage()
                        .AddBasicLearner(learnerIdFuture, surnameFuture, forenameFuture, dobFuture, dateofAdmissionFuture);

            string forenameLeaver = "SelPersonal" + SeleniumHelper.GenerateRandomString(5) + SeleniumHelper.GenerateRandomNumberUsingDateTime();
            string surnameLeaver = "SelPersonal" + SeleniumHelper.GenerateRandomString(5) + SeleniumHelper.GenerateRandomNumberUsingDateTime();
            DateTime dobLeaver = DateTime.ParseExact("10/16/2006", pattern, CultureInfo.InvariantCulture);
            var dateofAdmissionLeaver = DateTime.ParseExact(SeleniumHelper.GetDayBeforeToday(), pattern, CultureInfo.InvariantCulture);
            var learnerIdLeaver = Guid.NewGuid();
            var pupilLeaver = this.BuildDataPackage()
                        .AddBasicLearner(learnerIdFuture, surnameFuture, forenameFuture, dobFuture, dateofAdmissionFuture);

            #endregion

            #region Steps
            using (new DataSetup(purgeBeforeInsert: false, purgeAfterTest: true, packages: dataPackage))
            { 

             
                var yeargroup = Queries.GetFirstYearGroup();

                // Navigate to Manage User Defined Group
                SeleniumHelper.NavigateMenu("Tasks", "School Groups", "Manage User Defined Groups");

                // Search with Group Full Name
                var manageUserDefinedTriplet = new ManageUserDefinedTriplet();

                // Click button Create
                var manageUserDefinedPage = manageUserDefinedTriplet.ClickCreateNew();

                // Scroll to member
                manageUserDefinedPage.ScrollToMember();

                // Click add pupils
                var addPupilDialogTriplet = manageUserDefinedPage.ClickAddPupil();

                // Enter search pupil
                string pupilNameCurrent = String.Format("{0}, {1}", surname, forename);
                addPupilDialogTriplet.SearchCriteria.SearchPupilName = pupilNameCurrent;
                var addPupilDialogPage = addPupilDialogTriplet.SearchCriteria.Search<AddPupilsDialogDetail>();

                // VP : Search result displays returned match the expected results. 
                Assert.AreEqual(true, addPupilDialogPage.PupilNameList.All(x => x.Contains(pupilNameCurrent)), "Search for pupil name is not correct.");

                // Enter year group
                addPupilDialogTriplet.Refresh();
                addPupilDialogTriplet.SearchCriteria.SearchPupilName = String.Empty;
                addPupilDialogTriplet.SearchCriteria.YearGroup = yeargroup.FullName;
                addPupilDialogTriplet.SearchCriteria.IsCurrent = true;
                addPupilDialogPage = addPupilDialogTriplet.SearchCriteria.Search<AddPupilsDialogDetail>();

                // VP : Search result displays returned match the expected results. 
                Assert.AreEqual(true, addPupilDialogPage.YearGroupList.All(x => x.Equals(yeargroup.FullName)), "Search for year group is not correct.");

                // Enter Class
                addPupilDialogTriplet.Refresh();
                addPupilDialogTriplet.SearchCriteria.YearGroup = null;
                addPupilDialogTriplet.SearchCriteria.Class = className;
                addPupilDialogTriplet.SearchCriteria.IsCurrent = true;
                addPupilDialogPage = addPupilDialogTriplet.SearchCriteria.Search<AddPupilsDialogDetail>();

                // VP : Search result displays returned match the expected results. 
                Assert.AreEqual(true, addPupilDialogPage.ClassList.All(x => x.Equals(className)), "Search for class is not correct.");

                // Enter current pupil name
                addPupilDialogTriplet.Refresh();
                addPupilDialogTriplet.SearchCriteria.SearchPupilName = pupilNameCurrent;
                addPupilDialogTriplet.SearchCriteria.Class = null;
                addPupilDialogTriplet.SearchCriteria.IsCurrent = true;
                addPupilDialogTriplet.SearchCriteria.IsFuture = false;
                addPupilDialogTriplet.SearchCriteria.IsLeaver = false;
                addPupilDialogPage = addPupilDialogTriplet.SearchCriteria.Search<AddPupilsDialogDetail>();

                // VP : Search result displays returned match the expected results. 
                Assert.AreEqual(true, addPupilDialogPage.PupilNameList.All(x => x.Contains(pupilNameCurrent)), "Search for current pupil name is not correct.");

                // Enter future pupil name
                string pupilNameFuture = String.Format("{0}, {1}", surnameFuture, forenameFuture);
                addPupilDialogTriplet.Refresh();
                addPupilDialogTriplet.SearchCriteria.SearchPupilName = pupilNameFuture;
                addPupilDialogTriplet.SearchCriteria.IsCurrent = false;
                addPupilDialogTriplet.SearchCriteria.IsFuture = true;
                addPupilDialogTriplet.SearchCriteria.IsLeaver = false;
                addPupilDialogPage = addPupilDialogTriplet.SearchCriteria.Search<AddPupilsDialogDetail>();

                // VP : Search result displays returned match the expected results. 
                Assert.AreEqual(true, addPupilDialogPage.PupilNameList.All(x => x.Contains(pupilNameFuture)), "Search for future pupil name is not correct.");

                // Enter leaver pupil name
                string pupilNameLeaver = String.Format("{0}, {1}", surnameLeaver, forenameLeaver);

                addPupilDialogTriplet.Refresh();
                addPupilDialogTriplet.SearchCriteria.SearchPupilName = pupilNameLeaver;
                addPupilDialogTriplet.SearchCriteria.IsCurrent = false;
                addPupilDialogTriplet.SearchCriteria.IsFuture = false;
                addPupilDialogTriplet.SearchCriteria.IsLeaver = true;
                addPupilDialogPage = addPupilDialogTriplet.SearchCriteria.Search<AddPupilsDialogDetail>();

                // VP : Search result displays returned match the expected results. 
                Assert.AreEqual(true, addPupilDialogPage.PupilNameList.All(x => x.Contains(pupilNameLeaver)), "Search for leaver pupil name is not correct.");

                // Enter unavailable pupil name
                addPupilDialogTriplet.Refresh();
                addPupilDialogTriplet.SearchCriteria.SearchPupilName = unavailablePupilName;
                addPupilDialogTriplet.SearchCriteria.IsCurrent = true;
                addPupilDialogTriplet.SearchCriteria.IsFuture = true;
                addPupilDialogTriplet.SearchCriteria.IsLeaver = true;
                addPupilDialogPage = addPupilDialogTriplet.SearchCriteria.Search<AddPupilsDialogDetail>();

                // VP : Search result displays returned match the expected results. (No result)
                Assert.AreEqual(null, addPupilDialogPage.PupilNameList.FirstOrDefault(x => x.Contains(unavailablePupilName)), "Search for unavailable pupil name is not correct.");

                #endregion

                #region Post-condition

                // Close the dialog
                addPupilDialogTriplet.Refresh();
                addPupilDialogTriplet.ClickCancel();

                // Cancel new record
                manageUserDefinedPage = manageUserDefinedPage.Create();
                manageUserDefinedPage.ClickCancel();
                #endregion
            }
        }

        /// <summary>
        /// TC_UDG04
        /// Au : Hieu Pham
        /// Description: Verify that the 'Search for a Member' function works correctly by using the Search Filter fields to Target results and to ensure the Search function returns correct results
        /// Role: School Adminstrator
        /// Status: PASS
        /// </summary>
        [WebDriverTest(TimeoutSeconds = 2000, Enabled = true, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { "g1" }, DataProvider = "TC_UDG04_DATA")]
        public void TC_UDG04_Search_Add_Member_Correct(string surName, string foreName, string role, string unavailableSurName)
        {

            #region Steps

            // Login
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);

            // Navigate to Manage User Defined Group
            SeleniumHelper.NavigateMenu("Tasks", "School Groups", "Manage User Defined Groups");

            // Search with Group Full Name
            var manageUserDefinedTriplet = new ManageUserDefinedTriplet();

            // Click button Create
            var manageUserDefinedPage = manageUserDefinedTriplet.ClickCreateNew();

            // Scroll to member
            manageUserDefinedPage.ScrollToMember();

            // Click add member
            var addMemberDialogTriplet = manageUserDefinedPage.ClickAddMember();

            DataPackage dataPackage = this.BuildDataPackage();
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
                // Enter surname
                addMemberDialogTriplet.SearchCriteria.SurName = surname;
                addMemberDialogTriplet.SearchCriteria.Role = null;
                var addMemberDialogPage = addMemberDialogTriplet.SearchCriteria.Search<AddMemberDialogDetail>();

                // VP : Search result displays returned match the expected results. 
                Assert.AreEqual(true, addMemberDialogPage.MemberList.All(x => x.Contains(surname)), "Search for surname is not correct.");

                // Enter forename
                addMemberDialogTriplet.Refresh();
                addMemberDialogTriplet.SearchCriteria.SurName = String.Empty;
                addMemberDialogTriplet.SearchCriteria.ForeName = forename;
                addMemberDialogTriplet.SearchCriteria.Role = null;
                addMemberDialogPage = addMemberDialogTriplet.SearchCriteria.Search<AddMemberDialogDetail>();

                // VP : Search result displays returned match the expected results. 
                Assert.AreEqual(true, addMemberDialogPage.MemberList.All(x => x.Contains(forename)), "Search for forename is not correct.");

                // Enter Role 
                addMemberDialogTriplet.Refresh();
                addMemberDialogTriplet.SearchCriteria.ForeName = String.Empty;
                addMemberDialogTriplet.SearchCriteria.Role = role;
                addMemberDialogPage = addMemberDialogTriplet.SearchCriteria.Search<AddMemberDialogDetail>();

                // VP : Search result displays returned match the expected results. 
                Assert.AreNotEqual(0, addMemberDialogPage.MemberList.Count, "Search for role is not correct.");

                // Enter unavailable surname
                addMemberDialogTriplet.Refresh();
                addMemberDialogTriplet.SearchCriteria.ForeName = String.Empty;
                addMemberDialogTriplet.SearchCriteria.Role = null;
                addMemberDialogTriplet.SearchCriteria.SurName = unavailableSurName;
                addMemberDialogPage = addMemberDialogTriplet.SearchCriteria.Search<AddMemberDialogDetail>();

                // VP : Search result displays returned match the expected results. 
                Assert.AreEqual(0, addMemberDialogPage.MemberList.Count, "Search for unavailable surname is not correct.");

                #endregion
            }

        }

        /// <summary>
        /// TC UDG-05
        /// Au : An Nguyen
        /// Description: Create a User Defined Group and allocate Supervisors and Pupils for an effective date range in the current academic Year
        /// Role: School Administrator
        /// Status : Issue on Chrome : Supervisor and member disappear after click save button
        /// </summary>
        [WebDriverTest(TimeoutSeconds = 1000, Enabled = true, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { "g1" }, DataProvider = "TC_UDG05_Data")]
        public void TC_UDG05_Create_a_User_Defined_Group_and_allocate_Supervisors_and_Pupils_for_an_effective_date_range_in_the_current_academic_Year(string fullName, string shortName, string purpose, string note,
                    string supervisorName1, string supervisorName2, string supervisorName3, string supervisorName4, string supervisorName5, string pupilName, string memberName)
        {
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);

            #region Test steps

            //Navigate to Manage User Defined Group
            SeleniumHelper.NavigateMenu("Tasks", "School Groups", "Manage User Defined Groups");
            var manageUserDefinedTriplet = new ManageUserDefinedTriplet();

            //Create new Manage User Defined Group
            var manageUserDefinedPage = manageUserDefinedTriplet.ClickCreateNew();

            //Add group details
            manageUserDefinedPage.FullName = fullName;
            manageUserDefinedPage.ShortName = shortName;
            manageUserDefinedPage.Purpose = purpose;
            manageUserDefinedPage.Visibility = true;
            manageUserDefinedPage.Notes = note;

            DataPackage dataPackage = this.BuildDataPackage();
            //Employee Details Data Injection
            var employeeId1 = Guid.NewGuid();
            dataPackage.AddEmployee(employeeId1);

            //Staff Details Data Injection
            var staffId1 = Guid.NewGuid();
            var surname1 = Utilities.GenerateRandomString(10, "Surname");
            var forename1 = Utilities.GenerateRandomString(3, "Forename");
            dataPackage.AddStaff(staffId1, employeeId1, surname1, forename1);

            var serviceRecordId1 = Guid.NewGuid();
            var staffDOA = DateTime.Now;
            dataPackage.AddServiceRecord(serviceRecordId1, staffId1, staffDOA, null);

            //Employee Details Data Injection
            var employeeId2 = Guid.NewGuid();
            dataPackage.AddEmployee(employeeId2);

            //Staff Details Data Injection
            var staffId2 = Guid.NewGuid();
            var surname2 = Utilities.GenerateRandomString(10, "Surname");
            var forename2 = Utilities.GenerateRandomString(3, "Forename");
            dataPackage.AddStaff(staffId2, employeeId2, surname2, forename2);

            var serviceRecordId2 = Guid.NewGuid();
            dataPackage.AddServiceRecord(serviceRecordId2, staffId2, staffDOA, null);

            //Employee Details Data Injection
            var employeeId3 = Guid.NewGuid();
            dataPackage.AddEmployee(employeeId3);

            //Staff Details Data Injection
            var staffId3 = Guid.NewGuid();
            var surname3 = Utilities.GenerateRandomString(10, "Surname");
            var forename3 = Utilities.GenerateRandomString(3, "Forename");
            dataPackage.AddStaff(staffId3, employeeId3, surname3, forename3);

            var serviceRecordId3 = Guid.NewGuid();
            dataPackage.AddServiceRecord(serviceRecordId3, staffId3, staffDOA, null);

            //Pupil Data
            string pattern = "M/d/yyyy";

            string forename = "SelPersonal" + SeleniumHelper.GenerateRandomString(5) + SeleniumHelper.GenerateRandomNumberUsingDateTime();
            string surname = "SelPersonal" + SeleniumHelper.GenerateRandomString(5) + SeleniumHelper.GenerateRandomNumberUsingDateTime();
            DateTime dob = DateTime.ParseExact("10/16/2006", pattern, CultureInfo.InvariantCulture);
            var dateOfAdmission = DateTime.ParseExact("10/19/2010", pattern, CultureInfo.InvariantCulture);
            var learnerId = Guid.NewGuid();
            var pupilData = dataPackage.AddBasicLearner(learnerId, surname, forename, dob, dateOfAdmission);

            using (new DataSetup(purgeBeforeInsert: false, purgeAfterTest: true, packages: dataPackage))
            {

                //Add 3 supervisors
                supervisorName1 = string.Format("{0}, {1}", surname1, forename1);
                supervisorName2 = string.Format("{0}, {1}", surname2, forename2);
                supervisorName3 = string.Format("{0}, {1}", surname3, forename3);
                var addSupervisors = manageUserDefinedPage.ClickAddSupervisor();
                addSupervisors.SearchCriteria.Role = "Staff";
                var addSupervisorsDetail = addSupervisors.SearchCriteria.Search<AddSupervisorsDialogDetail>();
                var supervisorsSearchResult = addSupervisorsDetail.SearchResult;
                var supervisor = supervisorsSearchResult.FirstOrDefault(t => t.GetText().Equals(supervisorName1));
                supervisor.ClickByJS();
                supervisor = supervisorsSearchResult.FirstOrDefault(t => t.GetText().Equals(supervisorName2));
                supervisor.ClickByJS();
                supervisor = supervisorsSearchResult.FirstOrDefault(t => t.GetText().Equals(supervisorName3));
                supervisor.ClickByJS();
                addSupervisorsDetail.AddSelectedSupervisor();
                addSupervisors.ClickOk();
                var supervisorTable = manageUserDefinedPage.SupervisorTable;
                supervisorTable.WaitUntilRowAppear(t => t.Name.Equals(supervisorName1));

                //Add role to supervisor
                manageUserDefinedPage.ScrollToSupervisor();
                supervisorTable = manageUserDefinedPage.SupervisorTable;
                var supervisorRow = supervisorTable.Rows.FirstOrDefault(t => t.Name.Equals(supervisorName1));
                supervisorRow.Role = "Activity Leader";
                supervisorRow.Main = true;
                supervisorRow = supervisorTable.Rows.FirstOrDefault(t => t.Name.Equals(supervisorName2));
                supervisorRow.Role = "Main Supervisor";
                supervisorRow = supervisorTable.Rows.FirstOrDefault(t => t.Name.Equals(supervisorName3));
                supervisorRow.Role = "Supervisor";

               
                // Add Pupil
                pupilName = string.Format("{0}, {1}", surname, forename);
                manageUserDefinedPage.ScrollToMember();
                var addPupils = manageUserDefinedPage.ClickAddPupil();
                addPupils.SearchCriteria.SearchPupilName = pupilName;
                var addPupilDetail = addPupils.SearchCriteria.Search<AddPupilsDialogDetail>();
                var pupilsSearchResult = addPupilDetail.Pupils;
                var pupil = pupilsSearchResult.FirstOrDefault(t => t.GetText().Contains(pupilName));
                pupil.ClickByJS();
                addPupilDetail.AddSelectedPupils();
                addPupils.ClickOk();
                var memberTable = manageUserDefinedPage.MemberTable;
                memberTable.WaitUntilRowAppear(t => t.Name.Equals(pupilName));

                //Add member
                memberName = string.Format("{0}, {1}", surname1, forename1);
                var addMembers = manageUserDefinedPage.ClickAddMember();
                addMembers.SearchCriteria.Role = "Staff";
                var addMemberDetail = addMembers.SearchCriteria.Search<AddMemberDialogDetail>();
                var membersSearchResult = addMemberDetail.Members;
                var member = membersSearchResult.FirstOrDefault(t => t.GetText().Equals(memberName));
                member.ClickByJS();
                addMemberDetail.AddSelectedMembers();
                addMembers.ClickOk();
                memberTable = manageUserDefinedPage.MemberTable;
                memberTable.WaitUntilRowAppear(t => t.Name.Equals(memberName));

                //Save
                manageUserDefinedPage.SaveValue();
                manageUserDefinedPage.Refresh();

                //Search created user define group
                manageUserDefinedTriplet.Refresh();
                manageUserDefinedTriplet.SearchCriteria.FullName = fullName;
                var userDefinedSearchResult = manageUserDefinedTriplet.SearchCriteria.Search();
                var userDefinedTile = userDefinedSearchResult.FirstOrDefault(t => t.GroupName.Equals(fullName));
                Assert.AreNotEqual(null, userDefinedTile, "Cannot create new user defined group");
                manageUserDefinedPage = userDefinedTile.Click<ManageUserDefinedPage>();

                //Verify group details
                Assert.AreEqual(fullName, manageUserDefinedPage.FullName, "Full name of new User defined group is incorrect");
                Assert.AreEqual(shortName, manageUserDefinedPage.ShortName, "Short name of new User defined group is incorrect");
                Assert.AreEqual(purpose, manageUserDefinedPage.Purpose, "Purpose of new User defined group is incorrect");
                Assert.AreEqual(true, manageUserDefinedPage.Visibility, "Visibility of new User defined group is incorrect");
                Assert.AreEqual(note, manageUserDefinedPage.Notes, "Notes of new User defined group is incorrect");

                //Verify supervisor
                manageUserDefinedPage.ScrollToSupervisor();
                supervisorTable = manageUserDefinedPage.SupervisorTable;
                supervisorRow = supervisorTable.Rows.FirstOrDefault(t => t.Name.Equals(supervisorName1));
                Assert.AreNotEqual(null, supervisorRow, "Add the first supervisor to the User Defined Group unsuccessfull");
                Assert.AreEqual("Activity Leader", supervisorRow.Role, "Role of the first supervisor is incorrect");
                Assert.AreEqual(true, supervisorRow.Main, "The first supervisor must be main supervisor");
                supervisorRow = supervisorTable.Rows.FirstOrDefault(t => t.Name.Equals(supervisorName2));
                Assert.AreNotEqual(null, supervisorRow, "Add the second supervisor to the User Defined Group unsuccessfull");
                Assert.AreEqual("Main Supervisor", supervisorRow.Role, "Role of the second supervisor is incorrect");
                Assert.AreEqual(false, supervisorRow.Main, "The second supervisor must be main supervisor");
                supervisorRow = supervisorTable.Rows.FirstOrDefault(t => t.Name.Equals(supervisorName3));
                Assert.AreNotEqual(null, supervisorRow, "Add the third supervisor to the User Defined Group unsuccessfull");
                Assert.AreEqual("Supervisor", supervisorRow.Role, "Role of the third supervisor is incorrect");
                Assert.AreEqual(false, supervisorRow.Main, "The third supervisor must be main supervisor");
              
                //Verify member
                manageUserDefinedPage.ScrollToMember();
                memberTable = manageUserDefinedPage.MemberTable;
                var memberRow = memberTable.Rows.FirstOrDefault(t => t.Name.Contains(pupilName));
                Assert.AreNotEqual(null, memberRow, "Add pupil to the User Defined Group unsuccessfull");
                memberRow = memberTable.Rows.FirstOrDefault(t => t.Name.Equals(memberName));
                Assert.AreNotEqual(null, memberRow, "Add member to the User Defined Group unsuccessfull");

                #endregion

                #region Post-Condition : Delete User defined group

                //Delete member
                memberRow.DeleteRow();
                memberTable.Refresh();
                memberRow = memberTable.Rows.FirstOrDefault(t => t.Name.Contains(pupilName));
                memberRow.DeleteRow();

                //Delete supervisor
                supervisorTable = manageUserDefinedPage.SupervisorTable;
                supervisorRow = supervisorTable.Rows.FirstOrDefault(t => t.Name.Equals(supervisorName1));
                supervisorRow.DeleteRow();
                supervisorTable.Refresh();
                supervisorRow = supervisorTable.Rows.FirstOrDefault(t => t.Name.Equals(supervisorName2));
                supervisorRow.DeleteRow();
                supervisorTable.Refresh();
                supervisorRow = supervisorTable.Rows.FirstOrDefault(t => t.Name.Equals(supervisorName3));
                supervisorRow.DeleteRow();
                supervisorTable.Refresh();
            
                //Save
                manageUserDefinedPage.SaveValue();
                manageUserDefinedPage.Refresh();
                manageUserDefinedPage.Delete();

                #endregion
            }
        }

        /// <summary>
        /// TC UDG-06
        /// Au : Y Ta
        /// Description: Amend a User Defined Group, and the allocation of Supervisors and Pupils for an effective date range in the  current academic Year
        /// Role: School Administrator
        /// Issue :Can not add new Supervisor in User Defined Group Page. 
        /// Issue :There are no result when searching with pupil role
        /// Enviroment: Chrome
        /// </summary>
        [WebDriverTest(TimeoutSeconds = 1600, Enabled = false, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { "g1" }, DataProvider = "TC_UDG06_Data")]
        public void TC_UDG06_Amend_a_User_Defined_Group_and_allocate_Supervisors_and_Pupils_for_an_effective_date_range_in_the_current_academic_Year(string fullName, string shortName, string purpose, string note,
                    string supervisorName1, string pupilName, string memberName, string fullNameUpdate, string noteUpdate, string shortNameUpdate, string purposeUpdate, string currentAcademicYear, string supervisorNameUpdate, string supervisorNameVerify, string memberNameUpdate, string pupilNameUpdate, string pupilNameUpdateVerify)
        {

            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);

            #region Pre-Condition: Create New User Defined Group which contains supervisors and pupils

            //Navigate to Manage User Defined Group
            SeleniumHelper.NavigateMenu("Tasks", "School Groups", "Manage User Defined Groups");
            var manageUserDefinedTriplet = new ManageUserDefinedTriplet();

            //Create new Manage User Defined Group
            var manageUserDefinedPage = manageUserDefinedTriplet.ClickCreateNew();

            //Add group details
            manageUserDefinedPage.FullName = fullName;
            manageUserDefinedPage.ShortName = shortName;
            manageUserDefinedPage.Purpose = purpose;
            manageUserDefinedPage.Visibility = true;
            manageUserDefinedPage.Notes = note;
            manageUserDefinedPage.SaveValue();
            manageUserDefinedPage.Refresh();

            //Add Supervisors
            var addSupervisors = manageUserDefinedPage.ClickAddSupervisor();
            addSupervisors.SearchCriteria.Role = "Staff";
            var addSupervisorsDetail = addSupervisors.SearchCriteria.Search<AddSupervisorsDialogDetail>();
            var supervisorsSearchResult = addSupervisorsDetail.SearchResult;
            var supervisor = supervisorsSearchResult.FirstOrDefault(t => t.GetText().Equals(supervisorName1));
            supervisor.ClickByJS();
            addSupervisorsDetail.AddSelectedSupervisor();
            addSupervisors.ClickOk();

            var supervisorTable = manageUserDefinedPage.SupervisorTable;
            supervisorTable.WaitUntilRowAppear(t => t.Name.Equals(supervisorName1));

            //Add role to supervisor            
            supervisorTable = manageUserDefinedPage.SupervisorTable;
            var supervisorRow = supervisorTable.Rows.FirstOrDefault(t => t.Name.Equals(supervisorName1));
            supervisorRow.Role = "Activity Leader";

            //Add pupil            
            var addPupils = manageUserDefinedPage.ClickAddPupil();
            addPupils.SearchCriteria.SearchPupilName = pupilName;
            var addPupilDetail = addPupils.SearchCriteria.Search<AddPupilsDialogDetail>();
            var pupilsSearchResult = addPupilDetail.Pupils;
            var pupil = pupilsSearchResult.FirstOrDefault(t => t.GetText().Equals(pupilName));
            pupil.ClickByJS();
            addPupilDetail.AddSelectedPupils();
            addPupils.ClickOk();
            var memberTable = manageUserDefinedPage.MemberTable;
            memberTable.WaitUntilRowAppear(t => t.Name.Equals(pupilName));

            //Add member
            var addMembers = manageUserDefinedPage.ClickAddMember();
            addMembers.SearchCriteria.Role = "Staff";
            var addMemberDetail = addMembers.SearchCriteria.Search<AddMemberDialogDetail>();
            var membersSearchResult = addMemberDetail.Members;
            var member = membersSearchResult.FirstOrDefault(t => t.GetText().Equals(memberName));
            member.ClickByJS();
            addMemberDetail.AddSelectedMembers();
            addMembers.ClickOk();
            memberTable = manageUserDefinedPage.MemberTable;
            memberTable.WaitUntilRowAppear(t => t.Name.Equals(memberName));

            //Save
            manageUserDefinedPage.SaveValue();
            manageUserDefinedPage.Refresh();

            #endregion

            #region Steps

            #region Edit User Group Fields

            //Search created user define group
            manageUserDefinedTriplet.Refresh();
            manageUserDefinedTriplet.SearchCriteria.FullName = fullName;
            var userDefinedSearchResult = manageUserDefinedTriplet.SearchCriteria.Search();
            var userDefinedTile = userDefinedSearchResult.FirstOrDefault(t => t.GroupName.Equals(fullName));
            manageUserDefinedPage = userDefinedTile.Click<ManageUserDefinedPage>();

            //Amend the Title in  field 'Full Name'
            manageUserDefinedPage.FullName = fullNameUpdate;
            //Select a different value from Drop Down List box 'Visibility'
            manageUserDefinedPage.Visibility = false;
            //Amend text into field 'Notes'
            manageUserDefinedPage.Notes = noteUpdate;
            //Amend the short description in field 'Short Name'
            manageUserDefinedPage.ShortName = shortNameUpdate;
            //Select a different value from Drop Down List Box 'Purpose'
            manageUserDefinedPage.Purpose = purposeUpdate;
            manageUserDefinedPage.SaveValue();
            manageUserDefinedPage.Refresh();
            //Click Button 'Select Effective Date Range' and change the  current academic year Click 'Ok'
            var selectEffectDateRangeDialog = manageUserDefinedPage.SelectEffectDateRange();
            selectEffectDateRangeDialog.AcademicYear = currentAcademicYear;
            selectEffectDateRangeDialog.ClickOk();

            #endregion Edit User Group Fields


            #region Edit Supervisor

            //Select Supervisor record and Click Icon 'Delete this Row?'
            supervisorTable = manageUserDefinedPage.SupervisorTable;
            supervisorRow = supervisorTable.Rows.FirstOrDefault(t => t.Name.Equals(supervisorName1));
            supervisorRow.DeleteRow();
            supervisorTable.Refresh();

            addSupervisors = manageUserDefinedPage.ClickAddSupervisor();
            // Issue :There are no result when searching with pupil role
            // Temp solution: Switch to Staff role
            //addSupervisors.SearchCriteria.Role = "Pupil";
            addSupervisors.SearchCriteria.Role = "Pupil";
            addSupervisorsDetail = addSupervisors.SearchCriteria.Search<AddSupervisorsDialogDetail>();
            supervisorsSearchResult = addSupervisorsDetail.SearchResult;
            supervisor = supervisorsSearchResult.FirstOrDefault(t => t.GetText().Equals(supervisorNameUpdate));
            supervisor.ClickByJS();
            addSupervisorsDetail.AddSelectedSupervisor();
            addSupervisors.ClickOk();

            supervisorTable.Refresh();
            supervisorRow = supervisorTable.Rows.FirstOrDefault(t => t.Name.Contains(supervisorNameVerify));
            supervisorRow.Role = "Activity Leader";

            #endregion Edit Supervisor

            #region Edit Memeber

            // Remove old memeber
            memberTable.Refresh();
            var memberRow = memberTable.Rows.FirstOrDefault(t => t.Name.Equals(memberName));
            memberRow.DeleteRow();


            // Add new Member            
            addMembers = manageUserDefinedPage.ClickAddMember();
            addMembers.SearchCriteria.Role = "Staff";
            addMemberDetail = addMembers.SearchCriteria.Search<AddMemberDialogDetail>();
            membersSearchResult = addMemberDetail.Members;
            member = membersSearchResult.FirstOrDefault(t => t.GetText().Equals(memberNameUpdate));
            member.ClickByJS();
            addMemberDetail.AddSelectedMembers();
            addMembers.ClickOk();
            memberTable = manageUserDefinedPage.MemberTable;
            memberTable.WaitUntilRowAppear(t => t.Name.Equals(memberNameUpdate));

            // Remove old pupil
            var pupilRow = memberTable.Rows.FirstOrDefault(t => t.Name.Contains(pupilName));
            pupilRow.DeleteRow();
            // Add new Pupil
            addPupils = manageUserDefinedPage.ClickAddPupil();
            addPupils.SearchCriteria.SearchPupilName = pupilNameUpdate;
            addPupilDetail = addPupils.SearchCriteria.Search<AddPupilsDialogDetail>();
            pupilsSearchResult = addPupilDetail.Pupils;
            pupil = pupilsSearchResult.FirstOrDefault(t => t.GetText().Equals(pupilNameUpdate));
            pupil.ClickByJS();
            addPupilDetail.AddSelectedPupils();
            addPupils.ClickOk();
            memberTable = manageUserDefinedPage.MemberTable;
            memberTable.WaitUntilRowAppear(t => t.Name.Equals(pupilNameUpdate));

            manageUserDefinedPage.SaveValue();
            #endregion Edit Member

            #region Verify
            // Verify Edit User Group succesfully
            Assert.AreEqual(fullNameUpdate, manageUserDefinedPage.FullName, "Full name of new User defined group is incorrect");
            Assert.AreEqual(shortNameUpdate, manageUserDefinedPage.ShortName, "Short name of new User defined group is incorrect");
            Assert.AreEqual(purposeUpdate, manageUserDefinedPage.Purpose, "Purpose of new User defined group is incorrect");
            Assert.AreEqual(false, manageUserDefinedPage.Visibility, "Visibility of new User defined group is incorrect");
            Assert.AreEqual(noteUpdate, manageUserDefinedPage.Notes, "Notes of new User defined group is incorrect");

            //The Supervisors and Pupils removed no longer display in the User Defined Group (unless reselected)            
            supervisorTable = manageUserDefinedPage.SupervisorTable;
            supervisorRow = supervisorTable.Rows.FirstOrDefault(t => t.Name.Equals(supervisorName1));
            Assert.AreEqual(null, supervisorRow, "The old supervisor still display");

            memberTable = manageUserDefinedPage.MemberTable;
            pupilRow = memberTable.Rows.FirstOrDefault(t => t.Name.Equals(pupilName));
            Assert.AreEqual(null, pupilRow, "The old pupil still display");

            //The Selected Supervisors and Pupils matches those stored on the User Defined Group
            supervisorRow = supervisorTable.Rows.FirstOrDefault(t => t.Name.Equals(supervisorNameVerify));
            Assert.AreNotEqual(null, supervisorRow, "The supervisor update value does not display");

            pupilRow = memberTable.Rows.FirstOrDefault(t => t.Name.Equals(pupilNameUpdateVerify));
            Assert.AreNotEqual(null, pupilRow, "The pupil update value does not display");

            #endregion Verify

            #endregion Steps

            #region Post-Condition : Delete User defined group

            //Delete member            
            memberTable.Refresh();
            memberRow = memberTable.Rows.FirstOrDefault(t => t.Name.Equals(memberNameUpdate));
            memberRow.DeleteRow();

            // Delete Pupil
            memberTable.Refresh();
            memberRow = memberTable.Rows.FirstOrDefault(t => t.Name.Equals(pupilNameUpdateVerify));
            memberRow.DeleteRow();

            //Delete supervisor
            supervisorTable = manageUserDefinedPage.SupervisorTable;
            supervisorRow = supervisorTable.Rows.FirstOrDefault(t => t.Name.Equals(supervisorNameVerify));
            supervisorRow.DeleteRow();
            supervisorTable.Refresh();

            //Save
            manageUserDefinedPage.SaveValue();
            manageUserDefinedPage.Refresh();
            manageUserDefinedPage.Delete();

            #endregion
        }

        /// <summary>        
        /// Au : Y.Ta
        /// Description: Create a User Defined Group and allocate Supervisors and Pupils for an effective date range in a Future academic Year
        /// Role: School Administrator
        /// Issue :Can not add new Supervisor in User Defined Group Page. 
        /// Issue : There are no result when searching with 'Agent' role.
        /// Enviroment: Chrome
        /// </summary>
        [WebDriverTest(TimeoutSeconds = 1000, Enabled = false, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { "g1" }, DataProvider = "TC_UDG07_Data")]
        public void TC_UDG07_Create_a_User_Defined_Group_and_allocate_Supervisors_and_Pupils_for_an_effective_date_range_in_the_future_academic_Year(string fullName, string shortName, string purpose, string note,
                    string supervisorName1, string pupilName, string memberName, string futureAcademicYear, string FromDate, string EndDate)
        {
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);

            #region Test steps

            //Navigate to Manage User Defined Group
            SeleniumHelper.NavigateMenu("Tasks", "School Groups", "Manage User Defined Groups");
            var manageUserDefinedTriplet = new ManageUserDefinedTriplet();

            //Create new Manage User Defined Group
            var manageUserDefinedPage = manageUserDefinedTriplet.ClickCreateNew();

            //Add group details
            manageUserDefinedPage.FullName = fullName;
            manageUserDefinedPage.ShortName = shortName;
            manageUserDefinedPage.Purpose = purpose;
            manageUserDefinedPage.Visibility = true;
            manageUserDefinedPage.Notes = note;

            manageUserDefinedPage.SaveValue();
            //Click Button 'Select Effective Date Range' and change the  current academic year Click 'Ok'
            var selectEffectDateRangeDialog = manageUserDefinedPage.SelectEffectDateRange();
            selectEffectDateRangeDialog.AcademicYear = futureAcademicYear;
            selectEffectDateRangeDialog.ClickOk();

            //Add supervisors
            var addSupervisors = manageUserDefinedPage.ClickAddSupervisor();
            // Issue : There are no result when searching with 'Agent' role.
            // Temp solution: Using staff role.
            //addSupervisors.SearchCriteria.Role = "Agent";
            addSupervisors.SearchCriteria.Role = "Agent";
            var addSupervisorsDetail = addSupervisors.SearchCriteria.Search<AddSupervisorsDialogDetail>();
            var supervisorsSearchResult = addSupervisorsDetail.SearchResult;
            var supervisor = supervisorsSearchResult.FirstOrDefault(t => t.GetText().Equals(supervisorName1));
            supervisor.ClickByJS();
            addSupervisorsDetail.AddSelectedSupervisor();
            addSupervisors.ClickOk();
            var supervisorTable = manageUserDefinedPage.SupervisorTable;
            supervisorTable.WaitUntilRowAppear(t => t.Name.Equals(supervisorName1));

            //Add role to supervisor            
            supervisorTable = manageUserDefinedPage.SupervisorTable;
            var supervisorRow = supervisorTable.Rows.FirstOrDefault(t => t.Name.Equals(supervisorName1));
            supervisorRow.Role = "Activity Leader";
            supervisorRow.Main = true;
            supervisorRow.From = FromDate;
            supervisorRow.To = EndDate;

            //Add pupil            
            var addPupils = manageUserDefinedPage.ClickAddPupil();
            addPupils.SearchCriteria.SearchPupilName = pupilName;
            var addPupilDetail = addPupils.SearchCriteria.Search<AddPupilsDialogDetail>();
            var pupilsSearchResult = addPupilDetail.Pupils;
            var pupil = pupilsSearchResult.FirstOrDefault(t => t.GetText().Contains(pupilName));
            pupil.ClickByJS();
            addPupilDetail.AddSelectedPupils();
            addPupils.ClickOk();
            var memberTable = manageUserDefinedPage.MemberTable;
            memberTable.WaitUntilRowAppear(t => t.Name.Equals(pupilName));

            //Add member
            var addMembers = manageUserDefinedPage.ClickAddMember();
            addMembers.SearchCriteria.Role = "Staff";
            var addMemberDetail = addMembers.SearchCriteria.Search<AddMemberDialogDetail>();
            var membersSearchResult = addMemberDetail.Members;
            var member = membersSearchResult.FirstOrDefault(t => t.GetText().Equals(memberName));
            member.ClickByJS();
            addMemberDetail.AddSelectedMembers();
            addMembers.ClickOk();
            memberTable = manageUserDefinedPage.MemberTable;
            memberTable.WaitUntilRowAppear(t => t.Name.Equals(memberName));

            //Save
            manageUserDefinedPage.SaveValue();
            manageUserDefinedPage.Refresh();

            //Search created user define group
            manageUserDefinedTriplet.Refresh();
            manageUserDefinedTriplet.SearchCriteria.FullName = fullName;
            var userDefinedSearchResult = manageUserDefinedTriplet.SearchCriteria.Search();
            var userDefinedTile = userDefinedSearchResult.FirstOrDefault(t => t.GroupName.Equals(fullName));
            Assert.AreNotEqual(null, userDefinedTile, "Cannot create new user defined group");
            manageUserDefinedPage = userDefinedTile.Click<ManageUserDefinedPage>();

            // Update to future academic
            //Click Button 'Select Effective Date Range' and change the  current academic year Click 'Ok'
            manageUserDefinedPage.Refresh();
            manageUserDefinedPage.SaveValue();
            selectEffectDateRangeDialog = manageUserDefinedPage.SelectEffectDateRange();
            selectEffectDateRangeDialog.AcademicYear = futureAcademicYear;
            selectEffectDateRangeDialog.ClickOk();


            //Verify group details
            Assert.AreEqual(fullName, manageUserDefinedPage.FullName, "Full name of new User defined group is incorrect");
            Assert.AreEqual(shortName, manageUserDefinedPage.ShortName, "Short name of new User defined group is incorrect");
            Assert.AreEqual(purpose, manageUserDefinedPage.Purpose, "Purpose of new User defined group is incorrect");
            Assert.AreEqual(true, manageUserDefinedPage.Visibility, "Visibility of new User defined group is incorrect");
            Assert.AreEqual(note, manageUserDefinedPage.Notes, "Notes of new User defined group is incorrect");

            //Verify supervisor            
            supervisorTable = manageUserDefinedPage.SupervisorTable;
            supervisorRow = supervisorTable.Rows.FirstOrDefault(t => t.Name.Equals(supervisorName1));
            Assert.AreNotEqual(null, supervisorRow, "Add the first supervisor to the User Defined Group unsuccessfull");
            Assert.AreEqual("Activity Leader", supervisorRow.Role, "Role of the first supervisor is incorrect");
            Assert.AreEqual(true, supervisorRow.Main, "The first supervisor must be main supervisor");

            Assert.AreNotEqual(null, supervisorRow, "Add the fifth supervisor to the User Defined Group unsuccessfull");
            Assert.AreEqual("Activity Leader", supervisorRow.Role, "Role of the supervisor is incorrect");
            Assert.AreEqual(true, supervisorRow.Main, "The supervisor must be main supervisor");

            //Verify member            
            memberTable = manageUserDefinedPage.MemberTable;
            var memberRow = memberTable.Rows.FirstOrDefault(t => t.Name.Contains(pupilName));
            Assert.AreNotEqual(null, memberRow, "Add pupil to the User Defined Group unsuccessfull");
            memberRow = memberTable.Rows.FirstOrDefault(t => t.Name.Equals(memberName));
            Assert.AreNotEqual(null, memberRow, "Add member to the User Defined Group unsuccessfull");

            #endregion

            #region Post-Condition : Delete User defined group

            //Delete member
            memberRow.DeleteRow();
            memberTable.Refresh();
            memberRow = memberTable.Rows.FirstOrDefault(t => t.Name.Contains(pupilName));
            memberRow.DeleteRow();

            //Delete supervisor
            supervisorTable = manageUserDefinedPage.SupervisorTable;
            supervisorRow = supervisorTable.Rows.FirstOrDefault(t => t.Name.Equals(supervisorName1));
            supervisorRow.DeleteRow();
            supervisorTable.Refresh();

            //Save
            manageUserDefinedPage.SaveValue();
            manageUserDefinedPage.Refresh();
            manageUserDefinedPage.Delete();

            #endregion
        }

        /// <summary>
        /// Author: Ba.Truong
        /// Description: School Administrator, Amend a User Defined Group, and the allocation of Supervisors and Pupils for an effective date range in a Future academic Year
        ///              Supervisors are selected from Learner Contacts
        ///              Members are selected from Agents
        ///              No Pupils selected via 'Add Pupil'
        /// Status : Pending by bug #4, #5 : Can not add new Supervisor, pupils in User Defined Group Page
        /// </summary>
        [WebDriverTest(TimeoutSeconds = 3000, Enabled = false, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { "g1" }, DataProvider = "TC_UDG008_Data")]
        public void TC_UDG008_Amend_User_Defined_Group_And_Allocation_Of_Supervisors_And_Pupils_For_Effective_Date_Range_In_Future_Academic_Year(string[] definedGroupDetails,
                                                                                                                  string[] effectiveDateRange, string[] superviserInformation,
                                                                                                                  string[] pupilInformation, string[] memberInformation,
                                                                                                                  string[] updateDefinedGroupDetails, string[] updateEffectiveDateRange,
                                                                                                                  string[] updateSupervisorInformation, string[] updatePupilInformation,
                                                                                                                  string[] updateMemberInformation)
        {
            #region Pre-Condition: Create a User Defined Group

            //Login as School Administrator and navigate to "Manage User Defined Groups"
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);
            SeleniumHelper.NavigateMenu("Tasks", "School Groups", "Manage User Defined Groups");

            //Click Create to add new an user defined group
            var userDefinedGroupTriplet = new ManageUserDefinedTriplet();
            var userDefinedGroupPage = userDefinedGroupTriplet.ClickCreateNew();

            //Input details information
            userDefinedGroupPage.FullName = definedGroupDetails[0];
            userDefinedGroupPage.ShortName = definedGroupDetails[1];
            userDefinedGroupPage.Purpose = definedGroupDetails[2];
            userDefinedGroupPage.Notes = definedGroupDetails[3];
            userDefinedGroupPage.Visibility = true;
            userDefinedGroupPage = userDefinedGroupPage.SaveValue();

            //Select Effective Date Range
            var selectEffectiveDateRange = userDefinedGroupPage.SelectEffectDateRange();
            selectEffectiveDateRange.AcademicYear = effectiveDateRange[0];
            userDefinedGroupPage = selectEffectiveDateRange.ClickOK<ManageUserDefinedPage>();
            userDefinedGroupPage = userDefinedGroupPage.SaveValue();

            //Input Supervisors
            var supervisorTriplet = userDefinedGroupPage.ClickAddSupervisor();
            supervisorTriplet.SearchCriteria.Role = superviserInformation[0];
            var supervisorDetailDialog = supervisorTriplet.SearchCriteria.Search<AddSupervisorsDialogDetail>();
            supervisorDetailDialog.SearchResult.FirstOrDefault(p => p.GetText().Contains(superviserInformation[1])).ClickByJS();
            supervisorDetailDialog.SearchResult.FirstOrDefault(p => p.GetText().Contains(superviserInformation[2])).ClickByJS();
            supervisorDetailDialog.AddSelectedSupervisor();
            userDefinedGroupPage = supervisorDetailDialog.ClickOK<ManageUserDefinedPage>();

            //Input detail information for supervisor 1
            userDefinedGroupPage.SupervisorTable[0].Role = superviserInformation[3];

            //Input detail information for supervisor 2
            userDefinedGroupPage.SupervisorTable[1].Role = superviserInformation[3];

            //Input pupils
            var pupilTriplet = userDefinedGroupPage.ClickAddPupil();
            pupilTriplet.SearchCriteria.YearGroup = pupilInformation[0];
            pupilTriplet.SearchCriteria.Class = pupilInformation[1];
            var pupilDetailDialog = pupilTriplet.SearchCriteria.Search<AddPupilsDialogDetail>();
            pupilDetailDialog.Pupils.FirstOrDefault(p => p.GetText().Contains(pupilInformation[2])).ClickByJS();
            pupilDetailDialog.AddSelectedPupils();
            userDefinedGroupPage = pupilDetailDialog.ClickOK<ManageUserDefinedPage>();

            //Input Members
            var memberTriplet = userDefinedGroupPage.ClickAddMember();
            var memberDetailDialog = memberTriplet.SearchCriteria.Search<AddMemberDialogDetail>();
            memberDetailDialog.Members.FirstOrDefault(p => p.GetText().Contains(memberInformation[0])).ClickByJS();
            memberDetailDialog.AddSelectedMembers();
            userDefinedGroupPage = memberDetailDialog.ClickOK<ManageUserDefinedPage>();

            //Save data
            userDefinedGroupPage.SaveValue();

            #endregion

            #region Steps

            //Search group was created to amend data
            userDefinedGroupTriplet = new ManageUserDefinedTriplet();
            userDefinedGroupTriplet.SearchCriteria.FullName = definedGroupDetails[0];
            var groupTile = userDefinedGroupTriplet.SearchCriteria.Search().FirstOrDefault(x => x.GroupName.Equals(definedGroupDetails[0]));
            userDefinedGroupPage = groupTile.Click<ManageUserDefinedPage>();

            //Amend details information
            userDefinedGroupPage.FullName = updateDefinedGroupDetails[0];
            userDefinedGroupPage.ShortName = updateDefinedGroupDetails[1];
            userDefinedGroupPage.Purpose = updateDefinedGroupDetails[2];
            userDefinedGroupPage.Notes = updateDefinedGroupDetails[3];
            userDefinedGroupPage.Visibility = true;
            userDefinedGroupPage = userDefinedGroupPage.SaveValue();

            //Amend Effective Date Range
            selectEffectiveDateRange = userDefinedGroupPage.SelectEffectDateRange();
            selectEffectiveDateRange.AcademicYear = updateEffectiveDateRange[0];
            userDefinedGroupPage = selectEffectiveDateRange.ClickOK<ManageUserDefinedPage>();
            userDefinedGroupPage = userDefinedGroupPage.SaveValue();

            //Amend Supervisors by deleting a supervisor record
            var supervisor = userDefinedGroupPage.SupervisorTable.Rows.FirstOrDefault(x => x.Name.Contains(superviserInformation[1]));
            supervisor.DeleteRow();

            //Amend Supervisors by adding new Supervisors
            supervisorTriplet = userDefinedGroupPage.ClickAddSupervisor();
            supervisorTriplet.SearchCriteria.Role = updateSupervisorInformation[0];
            supervisorDetailDialog = supervisorTriplet.SearchCriteria.Search<AddSupervisorsDialogDetail>();
            supervisorDetailDialog.SearchResult.FirstOrDefault(p => p.GetText().Contains(updateSupervisorInformation[1])).ClickByJS();
            supervisorDetailDialog.AddSelectedSupervisor();
            userDefinedGroupPage = supervisorDetailDialog.ClickOK<ManageUserDefinedPage>();

            //Input detail information for supervisor
            supervisor = userDefinedGroupPage.SupervisorTable.Rows.FirstOrDefault(x => x.Name.Contains(updateSupervisorInformation[1]));
            supervisor.Role = updateSupervisorInformation[2];

            //Amend pupils by deleting a pupil record
            var pupil = userDefinedGroupPage.MemberTable.Rows.FirstOrDefault(x => x.Name.Contains(pupilInformation[2]));
            pupil.DeleteRow();

            //Amend Pupils by adding new pupil
            pupilTriplet = userDefinedGroupPage.ClickAddPupil();
            pupilTriplet.SearchCriteria.YearGroup = updatePupilInformation[0];
            pupilTriplet.SearchCriteria.Class = updatePupilInformation[1];
            pupilDetailDialog = pupilTriplet.SearchCriteria.Search<AddPupilsDialogDetail>();
            pupilDetailDialog.Pupils.FirstOrDefault(p => p.GetText().Contains(updatePupilInformation[2])).ClickByJS();
            pupilDetailDialog.AddSelectedPupils();
            userDefinedGroupPage = pupilDetailDialog.ClickOK<ManageUserDefinedPage>();

            //Amend Members by deleting a member record
            var member = userDefinedGroupPage.MemberTable.Rows.FirstOrDefault(x => x.Name.Contains(memberInformation[0]));
            member.DeleteRow();

            //Amend Members by adding a new member
            memberTriplet = userDefinedGroupPage.ClickAddMember();
            memberDetailDialog = memberTriplet.SearchCriteria.Search<AddMemberDialogDetail>();
            memberDetailDialog.Members.FirstOrDefault(p => p.GetText().Contains(updateMemberInformation[0])).ClickByJS();
            memberDetailDialog.AddSelectedMembers();
            userDefinedGroupPage = memberDetailDialog.ClickOK<ManageUserDefinedPage>();

            //Click Save
            userDefinedGroupPage.SaveValue();

            //Confirm basic information was updated
            Assert.AreEqual(updateDefinedGroupDetails[0], userDefinedGroupPage.FullName, "Full name was not updated");
            Assert.AreEqual(updateDefinedGroupDetails[1], userDefinedGroupPage.ShortName, "Short name was not updated");
            Assert.AreEqual(updateDefinedGroupDetails[2], userDefinedGroupPage.Purpose, "Purpose was not updated");
            Assert.AreEqual(updateDefinedGroupDetails[3], userDefinedGroupPage.Notes, "Note was not updated");

            //Confirm Supervisors grid was updated
            Assert.AreEqual(false, userDefinedGroupPage.SupervisorTable.Rows.Any(x => x.Name.Contains(superviserInformation[1])), "Supervisor that was deleted still exists in grid");
            Assert.AreEqual(true, userDefinedGroupPage.SupervisorTable.Rows.Any(x => x.Name.Contains(updateSupervisorInformation[1])), "Supervisor that was added doesn't exist in grid");

            //Confirm Pupil grid was updated
            Assert.AreEqual(false, userDefinedGroupPage.MemberTable.Rows.Any(x => x.Name.Contains(pupilInformation[2])), "Pupil that was deleted still exists in grid");
            Assert.AreEqual(true, userDefinedGroupPage.MemberTable.Rows.Any(x => x.Name.Contains(updatePupilInformation[2])), "Pupil that was added doesn't exist in grid");

            //Confirm Members were updated
            Assert.AreEqual(false, userDefinedGroupPage.MemberTable.Rows.Any(x => x.Name.Contains(memberInformation[0])), "Member that was deleted still exists in grid");
            Assert.AreEqual(true, userDefinedGroupPage.MemberTable.Rows.Any(x => x.Name.Contains(updateMemberInformation[0])), "Member that was added doesn't exist in grid");

            #endregion

            #region End-Condition

            //Delete records in Supervisor grid
            var supervisorRecords = userDefinedGroupPage.SupervisorTable.Rows;
            supervisorRecords[0].DeleteRow();
            supervisorRecords[1].DeleteRow();

            //Delete records in members grid
            var memberRecords = userDefinedGroupPage.MemberTable.Rows;
            memberRecords[0].DeleteRow();
            memberRecords[1].DeleteRow();
            userDefinedGroupPage.SaveValue();

            //Delete group
            userDefinedGroupPage.Delete();

            #endregion
        }
        /// <summary>
        /// TC UDG-09
        /// Au : An Nguyen
        /// Description: Amend a User Defined Group: Ensure a Supervisor cannot be input twice for the same role with overlapping date ranges with the Current academic Year
        /// Role: School Administrator
        /// Status : Issue on Chrome : Supervisor and member disappear after click save button
        /// </summary>
        [WebDriverTest(TimeoutSeconds = 1000, Enabled = false, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { "g1" }, DataProvider = "TC_UDG09_Data")]
        public void TC_UDG09_Amend_User_Defined_Group_Ensure_a_Supervisor_cannot_be_input_twice_for_the_same_role_with_overlapping_date_ranges_with_the_Current_academic_Year(string fullName, string shortName, string purpose, string note,
                    string supervisorName, string startDate, string endDate)
        {
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);

            #region Pre-condtion : Create new User Defined Group

            //Navigate to Manage User Defined Group
            SeleniumHelper.NavigateMenu("Tasks", "School Groups", "Manage User Defined Groups");
            var manageUserDefinedTriplet = new ManageUserDefinedTriplet();

            //Create new Manage User Defined Group
            var manageUserDefinedPage = manageUserDefinedTriplet.ClickCreateNew();

            //Add group details
            manageUserDefinedPage.FullName = fullName;
            manageUserDefinedPage.ShortName = shortName;
            manageUserDefinedPage.Purpose = purpose;
            manageUserDefinedPage.Visibility = true;
            manageUserDefinedPage.Notes = note;

            //Add supervisor
            var addSupervisors = manageUserDefinedPage.ClickAddSupervisor();
            addSupervisors.SearchCriteria.Role = "Staff";
            var addSupervisorsDetail = addSupervisors.SearchCriteria.Search<AddSupervisorsDialogDetail>();
            var supervisorsSearchResult = addSupervisorsDetail.SearchResult;
            var supervisor = supervisorsSearchResult.FirstOrDefault(t => t.GetText().Equals(supervisorName));
            supervisor.ClickByJS();
            addSupervisorsDetail.AddSelectedSupervisor();
            addSupervisors.ClickOk();
            var supervisorTable = manageUserDefinedPage.SupervisorTable;
            supervisorTable.WaitUntilRowAppear(t => t.Name.Equals(supervisorName));

            //Add role to supervisor
            supervisorTable = manageUserDefinedPage.SupervisorTable;
            var supervisorRow = supervisorTable.Rows.FirstOrDefault(t => t.Name.Equals(supervisorName));
            supervisorRow.Role = "Activity Leader";

            //Save
            manageUserDefinedPage.SaveValue();
            manageUserDefinedPage.Refresh();

            #endregion

            #region Test steps

            //Search created user define group
            manageUserDefinedTriplet.Refresh();
            manageUserDefinedTriplet.SearchCriteria.FullName = fullName;
            var userDefinedSearchResult = manageUserDefinedTriplet.SearchCriteria.Search();
            var userDefinedTile = userDefinedSearchResult.FirstOrDefault(t => t.GroupName.Equals(fullName));
            Assert.AreNotEqual(null, userDefinedTile, "Cannot create new user defined group");
            manageUserDefinedPage = userDefinedTile.Click<ManageUserDefinedPage>();

            //Add supervisor
            addSupervisors = manageUserDefinedPage.ClickAddSupervisor();
            addSupervisors.SearchCriteria.Role = "Staff";
            addSupervisorsDetail = addSupervisors.SearchCriteria.Search<AddSupervisorsDialogDetail>();
            supervisorsSearchResult = addSupervisorsDetail.SearchResult;
            supervisor = supervisorsSearchResult.FirstOrDefault(t => t.GetText().Equals(supervisorName));
            supervisor.ClickByJS();
            addSupervisorsDetail.AddSelectedSupervisor();
            addSupervisors.ClickOk();
            supervisorTable = manageUserDefinedPage.SupervisorTable;
            supervisorTable.WaitUntilRowAppear(t => t.Name.Equals(supervisorName) && t.Role.Trim().Equals(String.Empty));

            //Edit end date of other row
            supervisorTable.Refresh();
            supervisorRow = supervisorTable.Rows.FirstOrDefault(t => t.Name.Equals(supervisorName) && t.Role.Equals("Activity Leader"));
            supervisorRow.To = endDate;

            //Add role and start date
            supervisorTable.Refresh();
            supervisorRow = supervisorTable.Rows.FirstOrDefault(t => t.Name.Equals(supervisorName) && t.Role.Trim().Equals(String.Empty));
            supervisorRow.Role = "Activity Leader";
            supervisorRow.From = startDate;

            //Save
            manageUserDefinedPage.SaveValue();

            //Verify error message display
            Assert.AreEqual(true, manageUserDefinedPage.IsErrorMessageDisplay(), "Warning error does not display");
            Assert.AreEqual(true, manageUserDefinedPage.ErrorMessage.Contains("Group membership dates cannot overlap"), "Warning message is incorrect");

            #endregion

            #region Post-Condition : Delete User defined group

            //Delete supervisor
            supervisorTable = manageUserDefinedPage.SupervisorTable;
            supervisorRow = supervisorTable.Rows.FirstOrDefault(t => t.Name.Equals(supervisorName));
            supervisorRow.DeleteRow();
            supervisorTable.Refresh();
            supervisorRow = supervisorTable.Rows.FirstOrDefault(t => t.Name.Equals(supervisorName));
            supervisorRow.DeleteRow();

            //Save
            manageUserDefinedPage.SaveValue();
            manageUserDefinedPage.Refresh();
            manageUserDefinedPage.Delete();

            #endregion
        }

        /// <summary>
        /// TC UDG-10
        /// Au : An Nguyen
        /// Description: Amend a User Defined Group: Ensure the same Supervisor can be input twice for the same role with non overlapping date ranges for the Current academic Year
        /// Role: School Administrator
        /// Status : Issue on Chrome : Supervisor and member disappear after click save button
        /// </summary>
        [WebDriverTest(TimeoutSeconds = 1000, Enabled = false, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { "g1" }, DataProvider = "TC_UDG10_Data")]
        public void TC_UDG10_Amend_User_Defined_Group_Ensure_a_Supervisor_cannot_be_input_twice_for_the_same_role_with_non_overlapping_date_ranges_with_the_Current_academic_Year(string fullName, string shortName, string purpose, string note,
                    string supervisorName, string startDate, string endDate)
        {
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);

            #region Pre-condtion : Create new User Defined Group

            //Navigate to Manage User Defined Group
            SeleniumHelper.NavigateMenu("Tasks", "School Groups", "Manage User Defined Groups");
            var manageUserDefinedTriplet = new ManageUserDefinedTriplet();

            //Create new Manage User Defined Group
            var manageUserDefinedPage = manageUserDefinedTriplet.ClickCreateNew();

            //Add group details
            manageUserDefinedPage.FullName = fullName;
            manageUserDefinedPage.ShortName = shortName;
            manageUserDefinedPage.Purpose = purpose;
            manageUserDefinedPage.Visibility = true;
            manageUserDefinedPage.Notes = note;

            //Add supervisor
            var addSupervisors = manageUserDefinedPage.ClickAddSupervisor();
            addSupervisors.SearchCriteria.Role = "Staff";
            var addSupervisorsDetail = addSupervisors.SearchCriteria.Search<AddSupervisorsDialogDetail>();
            var supervisorsSearchResult = addSupervisorsDetail.SearchResult;
            var supervisor = supervisorsSearchResult.FirstOrDefault(t => t.GetText().Equals(supervisorName));
            supervisor.ClickByJS();
            addSupervisorsDetail.AddSelectedSupervisor();
            addSupervisors.ClickOk();
            var supervisorTable = manageUserDefinedPage.SupervisorTable;
            supervisorTable.WaitUntilRowAppear(t => t.Name.Equals(supervisorName));

            //Add role to supervisor
            supervisorTable = manageUserDefinedPage.SupervisorTable;
            var supervisorRow = supervisorTable.Rows.FirstOrDefault(t => t.Name.Equals(supervisorName));
            supervisorRow.Role = "Activity Leader";

            //Save
            manageUserDefinedPage.SaveValue();
            manageUserDefinedPage.Refresh();

            #endregion

            #region Test steps

            //Search created user define group
            manageUserDefinedTriplet.Refresh();
            manageUserDefinedTriplet.SearchCriteria.FullName = fullName;
            var userDefinedSearchResult = manageUserDefinedTriplet.SearchCriteria.Search();
            var userDefinedTile = userDefinedSearchResult.FirstOrDefault(t => t.GroupName.Equals(fullName));
            Assert.AreNotEqual(null, userDefinedTile, "Cannot create new user defined group");
            manageUserDefinedPage = userDefinedTile.Click<ManageUserDefinedPage>();

            //Edit end date of other row
            supervisorTable.Refresh();
            supervisorRow = supervisorTable.Rows.FirstOrDefault(t => t.Name.Equals(supervisorName) && t.Role.Equals("Activity Leader"));
            supervisorRow.To = endDate;
            manageUserDefinedPage.SaveValue();

            //Add supervisor
            addSupervisors = manageUserDefinedPage.ClickAddSupervisor();
            addSupervisors.SearchCriteria.Role = "Staff";
            addSupervisorsDetail = addSupervisors.SearchCriteria.Search<AddSupervisorsDialogDetail>();
            supervisorsSearchResult = addSupervisorsDetail.SearchResult;
            supervisor = supervisorsSearchResult.FirstOrDefault(t => t.GetText().Equals(supervisorName));
            supervisor.ClickByJS();
            addSupervisorsDetail.AddSelectedSupervisor();
            addSupervisors.ClickOk();
            supervisorTable = manageUserDefinedPage.SupervisorTable;
            supervisorTable.WaitUntilRowAppear(t => t.Name.Equals(supervisorName) && t.Role.Trim().Equals(String.Empty));

            //Add role and start date
            supervisorTable.Refresh();
            supervisorRow = supervisorTable.Rows.FirstOrDefault(t => t.Name.Equals(supervisorName) && t.Role.Trim().Equals(String.Empty));
            supervisorRow.Role = "Activity Leader";
            supervisorRow.From = startDate;

            //Save
            manageUserDefinedPage.SaveValue();

            //Verify success message display
            Assert.AreEqual(true, manageUserDefinedPage.IsSuccessMessageDisplayed(), "Success message does not display");

            #endregion

            #region Post-Condition : Delete User defined group

            //Delete supervisor
            supervisorTable = manageUserDefinedPage.SupervisorTable;
            supervisorRow = supervisorTable.Rows.FirstOrDefault(t => t.Name.Equals(supervisorName));
            supervisorRow.DeleteRow();
            supervisorTable.Refresh();
            supervisorRow = supervisorTable.Rows.FirstOrDefault(t => t.Name.Equals(supervisorName));
            supervisorRow.DeleteRow();

            //Save
            manageUserDefinedPage.SaveValue();
            manageUserDefinedPage.Refresh();
            manageUserDefinedPage.Delete();

            #endregion
        }

        /// <summary>
        /// TC UDG-11
        /// Au : Hieu Pham
        /// Description: Amend a User Defined Group: Ensure a Supervisor can be in a different role with overlapping date ranges for a Future academic Year
        /// Role: School Administrator
        /// Status : Pending by bug #4 : Can not add new Supervisor in User Defined Group Page - Only in Chrome
        /// </summary>
        [WebDriverTest(TimeoutSeconds = 2000, Enabled = false, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { "g1" }, DataProvider = "TC_UDG11_Data")]
        public void TC_UDG11_Amend_User_Defined_Group_OverLap_Date_Range_Supervisor_Future_Academic_Year(string fullName, string shortName, string purpose, string note, string roleSearch, string academicYear, string staffName,
            string staffRoleBefore, string staffRoleAfter, string fromDateBefore, string toDateBefore, string fromDateAfter, string toDateAfter)
        {

            #region Pre-condtion

            // Login
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);

            // Navigate to Manage User Defined Group
            SeleniumHelper.NavigateMenu("Tasks", "School Groups", "Manage User Defined Groups");

            var manageUserDefinedTriplet = new ManageUserDefinedTriplet();

            // Create new Manage User Defined Group
            var manageUserDefinedPage = manageUserDefinedTriplet.ClickCreateNew();

            // Add group details
            manageUserDefinedPage.FullName = fullName;
            manageUserDefinedPage.ShortName = shortName;
            manageUserDefinedPage.Purpose = purpose;
            manageUserDefinedPage.Visibility = true;
            manageUserDefinedPage.Notes = note;

            // Save values
            manageUserDefinedPage.SaveValue();

            // Select Effective date range
            manageUserDefinedPage.Refresh();
            var effectiveDateRangeDialog = manageUserDefinedPage.SelectEffectDateRange();
            effectiveDateRangeDialog.AcademicYear = academicYear;
            effectiveDateRangeDialog.ClickOk();

            // Add supervisor
            var supervisorDialogTriplet = manageUserDefinedPage.ClickAddSupervisor();
            supervisorDialogTriplet.SearchCriteria.SurName = staffName.Split(',')[0];
            supervisorDialogTriplet.SearchCriteria.Role = roleSearch;
            var supervisorDialogPage = supervisorDialogTriplet.SearchCriteria.Search<AddSupervisorsDialogDetail>();

            // Click to record
            supervisorDialogPage.SearchResult.FirstOrDefault(x => x.Text.Replace("  ", " ").Equals(staffName)).ClickByJS();
            supervisorDialogPage.AddSelectedSupervisor();

            // Click OK
            supervisorDialogTriplet.Refresh();
            supervisorDialogTriplet.ClickOk();

            // Select role for new record
            manageUserDefinedPage.Refresh();
            var supervisorTable = manageUserDefinedPage.SupervisorTable;
            var lastInsertRow = supervisorTable.Rows.FirstOrDefault(x => x.Role.Equals(String.Empty));
            lastInsertRow.Role = staffRoleBefore;
            lastInsertRow.Main = true;
            lastInsertRow.From = fromDateBefore;
            lastInsertRow.To = toDateBefore;

            // Save values
            manageUserDefinedPage.SaveValue();

            #endregion

            #region Test steps

            // Search record again
            manageUserDefinedTriplet = new ManageUserDefinedTriplet();
            manageUserDefinedTriplet.SearchCriteria.FullName = fullName;
            var results = manageUserDefinedTriplet.SearchCriteria.Search().FirstOrDefault(x => x.GroupName.Equals(fullName));
            manageUserDefinedPage = results.Click<ManageUserDefinedPage>();

            // Select Effective date range
            manageUserDefinedPage.Refresh();
            effectiveDateRangeDialog = manageUserDefinedPage.SelectEffectDateRange();
            effectiveDateRangeDialog.AcademicYear = academicYear;
            effectiveDateRangeDialog.ClickOk();

            // Scroll to supervisor
            manageUserDefinedPage.Refresh();
            manageUserDefinedPage.ScrollToSupervisor();
            supervisorDialogTriplet = manageUserDefinedPage.ClickAddSupervisor();

            // Enter value to search
            supervisorDialogTriplet.SearchCriteria.SurName = staffName.Split(',')[0];
            supervisorDialogTriplet.SearchCriteria.Role = roleSearch;
            supervisorDialogPage = supervisorDialogTriplet.SearchCriteria.Search<AddSupervisorsDialogDetail>();

            // Click to record
            supervisorDialogPage.SearchResult.FirstOrDefault(x => x.Text.Replace("  ", " ").Equals(staffName)).ClickByJS();
            supervisorDialogPage.AddSelectedSupervisor();

            // Click OK
            supervisorDialogTriplet.Refresh();
            supervisorDialogTriplet.ClickOk();

            // Select role for new record
            manageUserDefinedPage.Refresh();
            supervisorTable = manageUserDefinedPage.SupervisorTable;
            lastInsertRow = supervisorTable.Rows.FirstOrDefault(x => x.Role.Equals(String.Empty));
            lastInsertRow.Role = staffRoleAfter;
            lastInsertRow.Main = false;
            lastInsertRow.From = fromDateAfter;
            lastInsertRow.To = toDateAfter;

            // Save values
            manageUserDefinedPage.Refresh();
            manageUserDefinedPage.SaveValue();

            // VP : Verify overlap message displays
            Assert.AreEqual(false, manageUserDefinedPage.IsErrorMessageDisplay(), "Overlap message displays");

            // Search record again
            manageUserDefinedTriplet = new ManageUserDefinedTriplet();
            manageUserDefinedTriplet.SearchCriteria.FullName = fullName;
            results = manageUserDefinedTriplet.SearchCriteria.Search().FirstOrDefault(x => x.GroupName.Equals(fullName));
            manageUserDefinedPage = results.Click<ManageUserDefinedPage>();

            // Select Effective date range
            manageUserDefinedPage.Refresh();
            effectiveDateRangeDialog = manageUserDefinedPage.SelectEffectDateRange();
            effectiveDateRangeDialog.AcademicYear = academicYear;
            effectiveDateRangeDialog.ClickOk();

            // Scroll to supervisor
            manageUserDefinedPage.Refresh();
            manageUserDefinedPage.ScrollToSupervisor();
            supervisorTable = manageUserDefinedPage.SupervisorTable;
            var row = supervisorTable.Rows.FirstOrDefault(x => x.Role.Equals(staffRoleAfter));

            // VP : User Defined Group is successfully amended for a future academic year containing details of the group, supervisors and Members.
            Assert.AreNotEqual(null, row, "Amend Supervisor unsuccessfully");

            #endregion

            #region Post-condition

            // Delete all supervisor
            row.DeleteRow();
            row = supervisorTable.Rows.FirstOrDefault(x => x.Role.Equals(staffRoleBefore));
            row.DeleteRow();

            // Save value
            manageUserDefinedPage.Refresh();
            manageUserDefinedPage.SaveValue();

            // Delete record
            manageUserDefinedPage.Refresh();
            manageUserDefinedPage.Delete();

            #endregion
        }

        /// <summary>
        /// TC UDG-12
        /// Au : Hieu Pham
        /// Description: Amend a User Defined Group: Ensure a Supervisor can be in a different role with overlapping date ranges for a current academic Year
        /// Role: School Administrator
        /// Status : Pending by bug #4 : Can not add new Supervisor in User Defined Group Page - Only in Chrome
        /// </summary>
        [WebDriverTest(TimeoutSeconds = 2000, Enabled = false, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { "g1" }, DataProvider = "TC_UDG12_Data")]
        public void TC_UDG12_Amend_User_Defined_Group_OverLap_Date_Range_Supervisor_Current_Academic_Year(string fullName, string shortName, string purpose, string note, string roleSearch, string staffName,
            string staffRoleBefore, string staffRoleAfter, string fromDateBefore, string toDateBefore, string fromDateAfter, string toDateAfter)
        {

            #region Pre-condtion

            // Login
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);

            // Navigate to Manage User Defined Group
            SeleniumHelper.NavigateMenu("Tasks", "School Groups", "Manage User Defined Groups");

            var manageUserDefinedTriplet = new ManageUserDefinedTriplet();

            // Create new Manage User Defined Group
            var manageUserDefinedPage = manageUserDefinedTriplet.ClickCreateNew();

            // Add group details
            manageUserDefinedPage.FullName = fullName;
            manageUserDefinedPage.ShortName = shortName;
            manageUserDefinedPage.Purpose = purpose;
            manageUserDefinedPage.Visibility = true;
            manageUserDefinedPage.Notes = note;

            // Save values
            manageUserDefinedPage.SaveValue();

            // Add supervisor
            var supervisorDialogTriplet = manageUserDefinedPage.ClickAddSupervisor();
            supervisorDialogTriplet.SearchCriteria.SurName = staffName.Split(',')[0];
            supervisorDialogTriplet.SearchCriteria.Role = roleSearch;
            var supervisorDialogPage = supervisorDialogTriplet.SearchCriteria.Search<AddSupervisorsDialogDetail>();

            // Click to record
            supervisorDialogPage.SearchResult.FirstOrDefault(x => x.Text.Replace("  ", " ").Equals(staffName)).ClickByJS();
            supervisorDialogPage.AddSelectedSupervisor();

            // Click OK
            supervisorDialogTriplet.Refresh();
            supervisorDialogTriplet.ClickOk();

            // Select role for new record
            manageUserDefinedPage.Refresh();
            var supervisorTable = manageUserDefinedPage.SupervisorTable;
            var lastInsertRow = supervisorTable.Rows.FirstOrDefault(x => x.Role.Equals(String.Empty));
            lastInsertRow.Role = staffRoleBefore;
            lastInsertRow.Main = true;
            lastInsertRow.From = fromDateBefore;
            lastInsertRow.To = toDateBefore;

            // Save values
            manageUserDefinedPage.SaveValue();

            #endregion

            #region Test steps

            // Search record again
            manageUserDefinedTriplet = new ManageUserDefinedTriplet();
            manageUserDefinedTriplet.SearchCriteria.FullName = fullName;
            var results = manageUserDefinedTriplet.SearchCriteria.Search().FirstOrDefault(x => x.GroupName.Equals(fullName));
            manageUserDefinedPage = results.Click<ManageUserDefinedPage>();

            // Scroll to supervisor
            manageUserDefinedPage.Refresh();
            manageUserDefinedPage.ScrollToSupervisor();
            supervisorDialogTriplet = manageUserDefinedPage.ClickAddSupervisor();

            // Enter value to search
            supervisorDialogTriplet.SearchCriteria.SurName = staffName.Split(',')[0];
            supervisorDialogTriplet.SearchCriteria.Role = roleSearch;
            supervisorDialogPage = supervisorDialogTriplet.SearchCriteria.Search<AddSupervisorsDialogDetail>();

            // Click to record
            supervisorDialogPage.SearchResult.FirstOrDefault(x => x.Text.Replace("  ", " ").Equals(staffName)).ClickByJS();
            supervisorDialogPage.AddSelectedSupervisor();

            // Click OK
            supervisorDialogTriplet.Refresh();
            supervisorDialogTriplet.ClickOk();

            // Select role for new record
            manageUserDefinedPage.Refresh();
            supervisorTable = manageUserDefinedPage.SupervisorTable;
            lastInsertRow = supervisorTable.Rows.FirstOrDefault(x => x.Role.Equals(String.Empty));
            lastInsertRow.Role = staffRoleAfter;
            lastInsertRow.Main = false;
            lastInsertRow.From = fromDateAfter;
            lastInsertRow.To = toDateAfter;

            // Save values
            manageUserDefinedPage.Refresh();
            manageUserDefinedPage.SaveValue();

            // VP : Verify overlap message displays
            Assert.AreEqual(false, manageUserDefinedPage.IsErrorMessageDisplay(), "Overlap message displays");

            // Search record again
            manageUserDefinedTriplet = new ManageUserDefinedTriplet();
            manageUserDefinedTriplet.SearchCriteria.FullName = fullName;
            results = manageUserDefinedTriplet.SearchCriteria.Search().FirstOrDefault(x => x.GroupName.Equals(fullName));
            manageUserDefinedPage = results.Click<ManageUserDefinedPage>();

            // Scroll to supervisor
            manageUserDefinedPage.Refresh();
            manageUserDefinedPage.ScrollToSupervisor();
            supervisorTable = manageUserDefinedPage.SupervisorTable;
            var row = supervisorTable.Rows.FirstOrDefault(x => x.Role.Equals(staffRoleAfter));

            // VP : User Defined Group is successfully amended for a current academic year containing details of the group, supervisors and Members.
            Assert.AreNotEqual(null, row, "Amend Supervisor unsuccessfully");

            #endregion

            #region Post-condition

            // Delete all supervisor
            row.DeleteRow();
            row = supervisorTable.Rows.FirstOrDefault(x => x.Role.Equals(staffRoleBefore));
            row.DeleteRow();

            // Save value
            manageUserDefinedPage.Refresh();
            manageUserDefinedPage.SaveValue();

            // Delete record
            manageUserDefinedPage.Refresh();
            manageUserDefinedPage.Delete();

            #endregion
        }

        /// <summary>
        /// TC UDG- 13
        /// Au : Huy Vo
        /// Description: Amend a User Defined Group: Ensure a Supervisor cannot be in a the same role with overlapping date ranges for a  Future academic Year
        /// Role: School Administrator
        /// Status : Status : Pending by bug #4 : Can not add new Supervisor in User Defined Group Page - Both Chrome and IE
        /// </summary>
        [WebDriverTest(TimeoutSeconds = 1000, Enabled = false, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { "g1" }, DataProvider = "TC_UDG13_Data")]
        public void TC_UDG013_Amend_UDG_Ensure_Supervisor_Cannot_Be_The_Same_Role_With_Overlapping_Date_For_Future_Academic_Year(
            string fullName, string shortName, string purpose, string note,
            string futureAcademicYear, string supervisorName, string fromDate, string toDate,
            string pupilName, string memberName)
        {
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);

            #region Test steps

            //Navigate to Manage User Defined Group
            SeleniumHelper.NavigateMenu("Tasks", "School Groups", "Manage User Defined Groups");
            var manageUserDefinedTriplet = new ManageUserDefinedTriplet();

            //Create new Manage User Defined Group
            var manageUserDefinedPage = manageUserDefinedTriplet.ClickCreateNew();

            //Add group details
            manageUserDefinedPage.FullName = fullName;
            manageUserDefinedPage.ShortName = shortName;
            manageUserDefinedPage.Purpose = purpose;
            manageUserDefinedPage.Visibility = true;
            manageUserDefinedPage.Notes = note;

            //Save infor
            manageUserDefinedPage.SaveValue();

            // Add new future Academic
            var selectEffectDateRangeDialog = manageUserDefinedPage.SelectEffectDateRange();
            selectEffectDateRangeDialog.AcademicYear = futureAcademicYear;
            selectEffectDateRangeDialog.ClickOk();
            manageUserDefinedPage.SaveValue();

            //Add 1 supervisors
            var addSupervisors = manageUserDefinedPage.ClickAddSupervisor();
            addSupervisors.SearchCriteria.Role = "Staff";
            var addSupervisorsDetail = addSupervisors.SearchCriteria.Search<AddSupervisorsDialogDetail>();
            var supervisorsSearchResult = addSupervisorsDetail.SearchResult;
            var supervisor = supervisorsSearchResult.FirstOrDefault(t => t.GetText().Equals(supervisorName));
            supervisor.ClickByJS();

            addSupervisorsDetail.AddSelectedSupervisor();
            addSupervisors.ClickOk();
            var supervisorTable = manageUserDefinedPage.SupervisorTable;
            supervisorTable.WaitUntilRowAppear(t => t.Name.Equals(supervisorName));

            //Add role to supervisor
            manageUserDefinedPage.ScrollToSupervisor();
            supervisorTable = manageUserDefinedPage.SupervisorTable;
            supervisorTable[0].Role = "Supervisor";
            supervisorTable[0].From = fromDate;
            supervisorTable[0].To = toDate;

            //Add pupil
            manageUserDefinedPage.ScrollToMember();
            var addPupils = manageUserDefinedPage.ClickAddPupil();
            addPupils.SearchCriteria.SearchPupilName = pupilName;
            var addPupilDetail = addPupils.SearchCriteria.Search<AddPupilsDialogDetail>();
            var pupilsSearchResult = addPupilDetail.Pupils;
            var pupil = pupilsSearchResult.FirstOrDefault(t => t.GetText().Equals(pupilName));
            pupil.ClickByJS();
            addPupilDetail.AddSelectedPupils();
            addPupils.ClickOk();

            var memberTable = manageUserDefinedPage.MemberTable;
            memberTable.WaitUntilRowAppear(t => t.Name.Contains(pupilName));

            //Save
            manageUserDefinedPage.SaveValue();
            manageUserDefinedPage.Refresh();

            //Search User Define Group that has just created to add more Supervisors
            manageUserDefinedTriplet.Refresh();
            manageUserDefinedTriplet.SearchCriteria.FullName = fullName;
            var userDefinedSearchResult = manageUserDefinedTriplet.SearchCriteria.Search();
            var userDefinedTile = userDefinedSearchResult.FirstOrDefault(t => t.GroupName.Equals(fullName));
            Assert.AreNotEqual(null, userDefinedTile, "Cannot create new user defined group");
            manageUserDefinedPage = userDefinedTile.Click<ManageUserDefinedPage>();


            //Re-select Effect date range
            selectEffectDateRangeDialog = manageUserDefinedPage.SelectEffectDateRange();
            selectEffectDateRangeDialog.AcademicYear = futureAcademicYear;
            selectEffectDateRangeDialog.ClickOk();

            //Add supervisor 2
            addSupervisors = manageUserDefinedPage.ClickAddSupervisor();
            addSupervisors.SearchCriteria.Role = "Staff";
            addSupervisorsDetail = addSupervisors.SearchCriteria.Search<AddSupervisorsDialogDetail>();
            supervisorsSearchResult = addSupervisorsDetail.SearchResult;
            supervisor = supervisorsSearchResult.FirstOrDefault(t => t.GetText().Equals(supervisorName));
            supervisor.ClickByJS();

            addSupervisorsDetail.AddSelectedSupervisor();
            addSupervisors.ClickOk();
            supervisorTable = manageUserDefinedPage.SupervisorTable;
            supervisorTable.WaitUntilRowAppear(t => t.Name.Equals(supervisorName));

            //Add role to supervisor 2
            manageUserDefinedPage.ScrollToSupervisor();
            supervisorTable = manageUserDefinedPage.SupervisorTable;
            supervisorTable[1].Role = "Supervisor";
            supervisorTable[1].From = fromDate;
            supervisorTable[1].To = toDate;

            //Save
            manageUserDefinedPage.SaveValue();
            manageUserDefinedPage.Refresh();


            //Verify error message
            Assert.AreEqual(true, manageUserDefinedTriplet.IsErrorMessageExist(), "Member is not already allocated in that role on the day.");

            #endregion

            #region Post-Condition : Delete User defined group

            //Delete member
            memberTable = manageUserDefinedPage.MemberTable;
            var memberRow = memberTable.Rows.FirstOrDefault(t => t.Name.Contains(pupilName));
            memberRow.DeleteRow();

            //Delete supervisor
            supervisorTable = manageUserDefinedPage.SupervisorTable;
            var supervisorRow = supervisorTable.Rows.FirstOrDefault(t => t.Name.Equals(supervisorName));
            supervisorRow.DeleteRow();
            supervisorTable.Refresh();
            supervisorRow = supervisorTable.Rows.FirstOrDefault(t => t.Name.Equals(supervisorName));
            supervisorRow.DeleteRow();
            supervisorTable.Refresh();
            supervisorRow.DeleteRow();

            //Save
            manageUserDefinedPage.SaveValue();
            manageUserDefinedPage.Refresh();
            manageUserDefinedPage.Delete();

            #endregion
        }

        /// <summary>
        /// TC_UDG014
        /// Au : Huy Vo
        /// Description: Amend a User Defined Group: Ensure creating more than 1 Main Supervisor on any given day is prevented in a future Academic Year 
        /// Role: School Adminstrator
        /// Status: Pending by bug #4 : Can not add new Supervisor in User Defined Group Page - Both Chrome and IE
        /// </summary>
        [WebDriverTest(TimeoutSeconds = 2000, Enabled = false, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { "g1" }, DataProvider = "TC_UDG14_Data")]
        public void TC_UDG14_Amend_UDG_Ensure_Than_A_Main_Supervisor_Future_Academic_Year(
                        string fullName, string shortName, string purpose, string note, string futureAcademicYear,
                        string supervisorName1, string supervisorName2, string supervisorName3,
                        string pupilName, string memberName, string fromDate, string toDate)
        {
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);

            #region Test steps

            //Navigate to Manage User Defined Group
            SeleniumHelper.NavigateMenu("Tasks", "School Groups", "Manage User Defined Groups");
            var manageUserDefinedTriplet = new ManageUserDefinedTriplet();

            //Create new Manage User Defined Group
            var manageUserDefinedPage = manageUserDefinedTriplet.ClickCreateNew();

            //Add group details
            manageUserDefinedPage.FullName = fullName;
            manageUserDefinedPage.ShortName = shortName;
            manageUserDefinedPage.Purpose = purpose;
            manageUserDefinedPage.Visibility = true;
            manageUserDefinedPage.Notes = note;


            // Save basic infor
            manageUserDefinedPage.SaveValue();

            //Add future Academic year
            var selectEffectDateRangeDialog = manageUserDefinedPage.SelectEffectDateRange();
            selectEffectDateRangeDialog.AcademicYear = futureAcademicYear;
            selectEffectDateRangeDialog.ClickOk();

            manageUserDefinedPage.SaveValue();

            //Add 3 supervisors
            var addSupervisors = manageUserDefinedPage.ClickAddSupervisor();
            addSupervisors.SearchCriteria.Role = "Staff";
            var addSupervisorsDetail = addSupervisors.SearchCriteria.Search<AddSupervisorsDialogDetail>();
            var supervisorsSearchResult = addSupervisorsDetail.SearchResult;
            var supervisor = supervisorsSearchResult.FirstOrDefault(t => t.GetText().Equals(supervisorName1));
            supervisor.ClickByJS();
            supervisor = supervisorsSearchResult.FirstOrDefault(t => t.GetText().Equals(supervisorName2));
            supervisor.ClickByJS();
            supervisor = supervisorsSearchResult.FirstOrDefault(t => t.GetText().Equals(supervisorName3));
            supervisor.ClickByJS();
            addSupervisorsDetail.AddSelectedSupervisor();
            addSupervisors.ClickOk();

            var supervisorTable = manageUserDefinedPage.SupervisorTable;
            supervisorTable.WaitUntilRowAppear(t => t.Name.Equals(supervisorName1));

            //Add role to supervisor
            manageUserDefinedPage.ScrollToSupervisor();
            supervisorTable = manageUserDefinedPage.SupervisorTable;
            var supervisorRow = supervisorTable.Rows.FirstOrDefault(t => t.Name.Equals(supervisorName1));
            supervisorRow.Role = "Activity Leader";
            supervisorRow = supervisorTable.Rows.FirstOrDefault(t => t.Name.Equals(supervisorName2));
            supervisorRow.Role = "Curricular Manager";
            supervisorRow = supervisorTable.Rows.FirstOrDefault(t => t.Name.Equals(supervisorName3));
            supervisorRow.Role = "Supervisor";

            //Add pupil
            manageUserDefinedPage.ScrollToMember();
            var addPupils = manageUserDefinedPage.ClickAddPupil();
            addPupils.SearchCriteria.SearchPupilName = pupilName;
            var addPupilDetail = addPupils.SearchCriteria.Search<AddPupilsDialogDetail>();
            var pupilsSearchResult = addPupilDetail.Pupils;
            var pupil = pupilsSearchResult.FirstOrDefault(t => t.GetText().Equals(pupilName));
            pupil.ClickByJS();
            addPupilDetail.AddSelectedPupils();
            addPupils.ClickOk();
            var memberTable = manageUserDefinedPage.MemberTable;
            memberTable.WaitUntilRowAppear(t => t.Name.Contains(pupilName));

            //Save
            manageUserDefinedPage.SaveValue();
            manageUserDefinedPage.Refresh();

            //Search created user define group
            manageUserDefinedTriplet.Refresh();
            manageUserDefinedTriplet.SearchCriteria.FullName = fullName;
            var userDefinedSearchResult = manageUserDefinedTriplet.SearchCriteria.Search();
            var userDefinedTile = userDefinedSearchResult.FirstOrDefault(t => t.GroupName.Equals(fullName));
            Assert.AreNotEqual(null, userDefinedTile, "Cannot create new user defined group");
            manageUserDefinedPage = userDefinedTile.Click<ManageUserDefinedPage>();

            // Re-set effecte Date range

            selectEffectDateRangeDialog = manageUserDefinedPage.SelectEffectDateRange();
            selectEffectDateRangeDialog.AcademicYear = futureAcademicYear;
            selectEffectDateRangeDialog.ClickOk();

            //Select Supervisor Table to change Role to Supervisors main
            manageUserDefinedPage.ScrollToSupervisor();
            supervisorTable = manageUserDefinedPage.SupervisorTable;

            supervisorRow = supervisorTable.Rows.FirstOrDefault(t => t.Name.Equals(supervisorName1));
            supervisorRow.Role = "Supervisor";
            supervisorRow.Main = true;
            supervisorRow.From = fromDate;
            supervisorRow.To = toDate;
            supervisorRow = supervisorTable.Rows.FirstOrDefault(t => t.Name.Equals(supervisorName2));
            supervisorRow.Role = "Supervisor";
            supervisorRow.Main = true;
            supervisorRow.From = fromDate;
            supervisorRow.To = toDate;
            supervisorRow = supervisorTable.Rows.FirstOrDefault(t => t.Name.Equals(supervisorName3));
            supervisorRow.Role = "Supervisor";
            supervisorRow.Main = true;
            supervisorRow.From = fromDate;
            supervisorRow.To = toDate;

            //Save
            manageUserDefinedPage.SaveValue();
            manageUserDefinedPage.Refresh();

            //Verify error message
            Assert.AreEqual(true, manageUserDefinedTriplet.IsErrorMessageExist(), " More Staffs Member can be a Main Supervisors on any given day");

            #endregion

            #region Post-Condition : Delete User defined group

            //Delete member
            manageUserDefinedPage.ScrollToMember();
            memberTable = manageUserDefinedPage.MemberTable;
            memberTable.Rows.FirstOrDefault(t => t.Name.Contains(pupilName)).DeleteRow();
            memberTable.Refresh();

            //Delete supervisor
            supervisorTable = manageUserDefinedPage.SupervisorTable;
            supervisorRow = supervisorTable.Rows.FirstOrDefault(t => t.Name.Equals(supervisorName1));
            supervisorRow.DeleteRow();
            supervisorTable.Refresh();
            supervisorRow = supervisorTable.Rows.FirstOrDefault(t => t.Name.Equals(supervisorName2));
            supervisorRow.DeleteRow();
            supervisorTable.Refresh();
            supervisorRow = supervisorTable.Rows.FirstOrDefault(t => t.Name.Equals(supervisorName3));
            supervisorRow.DeleteRow();

            //Save
            manageUserDefinedPage.SaveValue();
            manageUserDefinedPage.Refresh();
            manageUserDefinedPage.Delete();

            #endregion

        }

        /// <summary>
        /// TC UDG-15
        /// Au : An Nguyen
        /// Description: Amend a User Defined Group: Ensure a Staff Member can marked as the Main Supervisor on any given day for an effective date range in the  Current Academic Year
        /// Role: School Administrator
        /// Status : Issue on Chrome : Supervisor and member disappear after click save button
        /// </summary>
        [WebDriverTest(TimeoutSeconds = 1000, Enabled = false, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { "g1" }, DataProvider = "TC_UDG15_Data")]
        public void TC_UDG15_Amend_User_Defined_Group_Ensure_Staff_Member_can_marked_as_the_Main_Supervisor_on_any_given_day_in_the_Current_Academic_Year(string fullName, string shortName, string purpose, string note,
                    string supervisorName1, string supervisorName2, string supervisorName3, string endDate1, string startDate2, string startDate3)
        {
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);

            #region Pre-condtion : Create new User Defined Group

            //Navigate to Manage User Defined Group
            SeleniumHelper.NavigateMenu("Tasks", "School Groups", "Manage User Defined Groups");
            var manageUserDefinedTriplet = new ManageUserDefinedTriplet();

            //Create new Manage User Defined Group
            var manageUserDefinedPage = manageUserDefinedTriplet.ClickCreateNew();

            //Add group details
            manageUserDefinedPage.FullName = fullName;
            manageUserDefinedPage.ShortName = shortName;
            manageUserDefinedPage.Purpose = purpose;
            manageUserDefinedPage.Visibility = true;
            manageUserDefinedPage.Notes = note;

            //Save
            manageUserDefinedPage.SaveValue();
            manageUserDefinedPage.Refresh();

            #endregion

            #region Test steps

            //Search created user define group
            manageUserDefinedTriplet.Refresh();
            manageUserDefinedTriplet.SearchCriteria.FullName = fullName;
            var userDefinedSearchResult = manageUserDefinedTriplet.SearchCriteria.Search();
            var userDefinedTile = userDefinedSearchResult.FirstOrDefault(t => t.GroupName.Equals(fullName));
            Assert.AreNotEqual(null, userDefinedTile, "Cannot create new user defined group");
            manageUserDefinedPage = userDefinedTile.Click<ManageUserDefinedPage>();

            //Add 3 supervisors
            var addSupervisors = manageUserDefinedPage.ClickAddSupervisor();
            addSupervisors.SearchCriteria.Role = "Staff";
            var addSupervisorsDetail = addSupervisors.SearchCriteria.Search<AddSupervisorsDialogDetail>();
            var supervisorsSearchResult = addSupervisorsDetail.SearchResult;
            var supervisor = supervisorsSearchResult.FirstOrDefault(t => t.GetText().Equals(supervisorName1));
            supervisor.ClickByJS();
            supervisor = supervisorsSearchResult.FirstOrDefault(t => t.GetText().Equals(supervisorName2));
            supervisor.ClickByJS();
            supervisor = supervisorsSearchResult.FirstOrDefault(t => t.GetText().Equals(supervisorName3));
            supervisor.ClickByJS();
            addSupervisorsDetail.AddSelectedSupervisor();
            addSupervisors.ClickOk();
            var supervisorTable = manageUserDefinedPage.SupervisorTable;
            supervisorTable.WaitUntilRowAppear(t => t.Name.Equals(supervisorName1));

            //Add information to the first supervisor who is the main supervisor
            supervisorTable = manageUserDefinedPage.SupervisorTable;
            var supervisorRow = supervisorTable.Rows.FirstOrDefault(t => t.Name.Equals(supervisorName1));
            supervisorRow.Role = "Supervisor";
            supervisorRow.Main = true;
            supervisorRow.To = endDate1;

            //Add information to the second supervisor who is overlap with the first supervisor
            supervisorRow = supervisorTable.Rows.FirstOrDefault(t => t.Name.Equals(supervisorName2));
            supervisorRow.Role = "Supervisor";
            supervisorRow.Main = false;
            supervisorRow.From = startDate2;

            //Add information to the third supervisor who is non overlap with the first supervisor and is the main supervisor
            supervisorRow = supervisorTable.Rows.FirstOrDefault(t => t.Name.Equals(supervisorName3));
            supervisorRow.Role = "Supervisor";
            supervisorRow.Main = true;
            supervisorRow.From = startDate3;

            //Save
            manageUserDefinedPage.SaveValue();

            //Verify success message display
            Assert.AreEqual(true, manageUserDefinedPage.IsSuccessMessageDisplayed(), "Add multiple main supervisor on any given day in Current Academic Year unsuccessfull");

            //Search created user define group
            manageUserDefinedTriplet.Refresh();
            manageUserDefinedTriplet.SearchCriteria.FullName = fullName;
            userDefinedSearchResult = manageUserDefinedTriplet.SearchCriteria.Search();
            userDefinedTile = userDefinedSearchResult.FirstOrDefault(t => t.GroupName.Equals(fullName));
            Assert.AreNotEqual(null, userDefinedTile, "Cannot create new user defined group");
            manageUserDefinedPage = userDefinedTile.Click<ManageUserDefinedPage>();

            //Verify data
            supervisorTable = manageUserDefinedPage.SupervisorTable;
            supervisorRow = supervisorTable.Rows.FirstOrDefault(t => t.Name.Equals(supervisorName1));
            Assert.AreNotEqual(null, supervisorRow, "Add the first supervisor to the User Defined Group unsuccessfull");
            Assert.AreEqual("Supervisor", supervisorRow.Role, "Role of the first supervisor is incorrect");
            Assert.AreEqual(true, supervisorRow.Main, "The first supervisor must be main supervisor");
            supervisorRow = supervisorTable.Rows.FirstOrDefault(t => t.Name.Equals(supervisorName2));
            Assert.AreNotEqual(null, supervisorRow, "Add the second supervisor to the User Defined Group unsuccessfull");
            Assert.AreEqual("Supervisor", supervisorRow.Role, "Role of the second supervisor is incorrect");
            Assert.AreEqual(false, supervisorRow.Main, "The second supervisor must be main supervisor");
            supervisorRow = supervisorTable.Rows.FirstOrDefault(t => t.Name.Equals(supervisorName3));
            Assert.AreNotEqual(null, supervisorRow, "Add the third supervisor to the User Defined Group unsuccessfull");
            Assert.AreEqual("Supervisor", supervisorRow.Role, "Role of the third supervisor is incorrect");
            Assert.AreEqual(true, supervisorRow.Main, "The third supervisor must be main supervisor");

            #endregion

            #region Post-Condition : Delete User defined group

            //Delete supervisor
            supervisorTable = manageUserDefinedPage.SupervisorTable;
            supervisorRow = supervisorTable.Rows.FirstOrDefault(t => t.Name.Equals(supervisorName1));
            supervisorRow.DeleteRow();
            supervisorTable.Refresh();
            supervisorRow = supervisorTable.Rows.FirstOrDefault(t => t.Name.Equals(supervisorName2));
            supervisorRow.DeleteRow();
            supervisorTable.Refresh();
            supervisorRow = supervisorTable.Rows.FirstOrDefault(t => t.Name.Equals(supervisorName3));
            supervisorRow.DeleteRow();

            //Save
            manageUserDefinedPage.SaveValue();
            manageUserDefinedPage.Refresh();
            manageUserDefinedPage.Delete();

            #endregion
        }

        /// <summary>
        /// TC UDG-26
        /// Au : An Nguyen
        /// Description: Amend a User Defined Group: For a Staff member Leaving in the Current Academic Year, ensure they can still be selected as a  Supervisor for a User Defined Group before they leave in the Current Academic Year 
        /// Role: School Administrator
        /// Status : Issue on Chrome : Supervisor and member disappear after click save button
        /// </summary>
        [WebDriverTest(TimeoutSeconds = 1000, Enabled = false, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { "g1" }, DataProvider = "TC_UDG26_Data")]
        public void TC_UDG26_Ensure_Staff_who_Leaving_in_the_Current_Academic_Year_can_still_be_selected_as_a_Supervisor_for_a_User_Defined_Group_before_they_leave_in_the_Current_Academic_Year
                (string foreName, string surName, string gender, string dateOfBirth, string dateOfArrival, string dateOfLeaving, string reason,
                    string fullName, string shortName, string purpose, string note, string supervisorName)
        {
            #region Pre-Condition : Create leaver staff

            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.PersonnelOfficer);

            //Delete a exist record            
            SeleniumHelper.NavigateMenu("Tasks", "Staff", "Delete Staff Record");
            var deleteStaffRecords = new DeleteStaffRecordTriplet();

            //Search exists staff
            deleteStaffRecords.SearchCriteria.StaffName = supervisorName;
            deleteStaffRecords.SearchCriteria.IsCurrent = true;
            deleteStaffRecords.SearchCriteria.IsFuture = true;
            deleteStaffRecords.SearchCriteria.IsLeaver = true;
            var staffSearchResults = deleteStaffRecords.SearchCriteria.Search();

            var staffSearchTile = staffSearchResults.SingleOrDefault(t => t.Name.Equals(supervisorName));
            var staffRecord = staffSearchTile == null ? StaffRecordPage.Create() : staffSearchTile.Click<StaffRecordPage>();

            //Delete staff if exist
            staffRecord.DeleteStaff();
            staffRecord = staffRecord.ContinueDeleteStaff();

            //Create new staff
            SeleniumHelper.NavigateMenu("Tasks", "Staff", "Staff Records");
            var staffRecords = new StaffRecordTriplet();
            var addNewStaffDialog = staffRecords.CreateStaff();

            //Fill Add New Staff Dialog                           
            addNewStaffDialog.Forename = foreName;
            addNewStaffDialog.SurName = surName;
            addNewStaffDialog.Gender = gender;
            addNewStaffDialog.DateOfBirth = dateOfBirth;

            //Fill Service Detail Dialog
            var serviceDetailDialog = addNewStaffDialog.Continue();
            serviceDetailDialog.DateOfArrival = dateOfArrival;
            serviceDetailDialog.CreateRecord();

            //Save staff
            staffRecord = StaffRecordPage.Create();
            staffRecord.SaveStaff();

            //Leaver staff
            var staffLeavingDetailPage = SeleniumHelper.NavigateViaAction<StaffLeavingDetailPage>("Staff Leaving Details");
            staffLeavingDetailPage.DateOfLeaving = dateOfLeaving;
            staffLeavingDetailPage.ReasonForLeaving = reason;
            var comfirmRequiredDialog = staffLeavingDetailPage.SaveValue();
            comfirmRequiredDialog.ClickOk();
            SeleniumHelper.Logout();

            #endregion

            #region Test steps

            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);

            //Navigate to Manage User Defined Group
            SeleniumHelper.NavigateMenu("Tasks", "School Groups", "Manage User Defined Groups");
            var manageUserDefinedTriplet = new ManageUserDefinedTriplet();

            //Create new Manage User Defined Group
            var manageUserDefinedPage = manageUserDefinedTriplet.ClickCreateNew();

            //Add group details
            manageUserDefinedPage.FullName = fullName;
            manageUserDefinedPage.ShortName = shortName;
            manageUserDefinedPage.Purpose = purpose;
            manageUserDefinedPage.Visibility = true;
            manageUserDefinedPage.Notes = note;

            //Add supervisor
            var addSupervisors = manageUserDefinedPage.ClickAddSupervisor();
            addSupervisors.SearchCriteria.Role = "Staff";
            var addSupervisorsDetail = addSupervisors.SearchCriteria.Search<AddSupervisorsDialogDetail>();
            var supervisorsSearchResult = addSupervisorsDetail.SearchResult;
            var supervisor = supervisorsSearchResult.FirstOrDefault(t => t.GetText().Equals(supervisorName));
            Assert.AreNotEqual(null, supervisor, "Staff member who leaving in Current Academic Year can not selected as supervisor");
            supervisor.ClickByJS();
            addSupervisorsDetail.AddSelectedSupervisor();
            addSupervisors.ClickOk();
            var supervisorTable = manageUserDefinedPage.SupervisorTable;
            supervisorTable.WaitUntilRowAppear(t => t.Name.Equals(supervisorName));

            //Add role to supervisor
            supervisorTable = manageUserDefinedPage.SupervisorTable;
            var supervisorRow = supervisorTable.Rows.FirstOrDefault(t => t.Name.Equals(supervisorName));
            supervisorRow.Role = "Supervisor";

            //Save
            manageUserDefinedPage.SaveValue();
            manageUserDefinedPage.Refresh();

            //Verify success message display
            Assert.AreEqual(true, manageUserDefinedPage.IsSuccessMessageDisplayed(), "Create User Defined Group with Staff member who leaving in Current Academic Year unsuccessfull");

            //Search created user define group
            manageUserDefinedTriplet.Refresh();
            manageUserDefinedTriplet.SearchCriteria.FullName = fullName;
            var userDefinedSearchResult = manageUserDefinedTriplet.SearchCriteria.Search();
            var userDefinedTile = userDefinedSearchResult.FirstOrDefault(t => t.GroupName.Equals(fullName));
            Assert.AreNotEqual(null, userDefinedTile, "Cannot create new user defined group");
            manageUserDefinedPage = userDefinedTile.Click<ManageUserDefinedPage>();

            //Verify data
            supervisorTable = manageUserDefinedPage.SupervisorTable;
            supervisorRow = supervisorTable.Rows.FirstOrDefault(t => t.Name.Equals(supervisorName));
            Assert.AreNotEqual(null, supervisorRow, "Add the first supervisor to the User Defined Group unsuccessfull");
            Assert.AreEqual("Supervisor", supervisorRow.Role, "Role of the supervisor is incorrect");

            #endregion

            #region Post-Condition

            //Delete supervisor
            supervisorTable = manageUserDefinedPage.SupervisorTable;
            supervisorRow = supervisorTable.Rows.FirstOrDefault(t => t.Name.Equals(supervisorName));
            supervisorRow.DeleteRow();

            //Save
            manageUserDefinedPage.SaveValue();
            manageUserDefinedPage.Refresh();

            //Delete User Defined Group
            manageUserDefinedPage.Delete();

            //Delete Staff
            SeleniumHelper.Logout();
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.PersonnelOfficer);
            SeleniumHelper.NavigateMenu("Tasks", "Staff", "Delete Staff Record");
            deleteStaffRecords = new DeleteStaffRecordTriplet();
            deleteStaffRecords.SearchCriteria.StaffName = supervisorName;
            deleteStaffRecords.SearchCriteria.IsCurrent = true;
            deleteStaffRecords.SearchCriteria.IsFuture = true;
            deleteStaffRecords.SearchCriteria.IsLeaver = true;
            staffSearchResults = deleteStaffRecords.SearchCriteria.Search();
            staffSearchTile = staffSearchResults.SingleOrDefault(t => t.Name.Equals(supervisorName));
            staffRecord = staffSearchTile == null ? StaffRecordPage.Create() : staffSearchTile.Click<StaffRecordPage>();
            staffRecord.DeleteStaff();

            #endregion
        }

        /// <summary>
        /// Author: Ba.Truong
        /// Description: School Administrator, For a Staff member Leaving in the Current Academic Year, 
        ///              ensure they can still be selected as a Member for a User Defined Group before they leave in the Current Academic Year 
        /// Status : Pending by bug #4 : Can not add new Supervisor in User Defined Group Page
        /// </summary>
        [WebDriverTest(TimeoutSeconds = 3000, Enabled = false, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { "g1" }, DataProvider = "TC_UDG027_Data")]
        public void TC_UDG027_Ensure_Staff_Can_Be_Selected_As_A_Member_For_A_User_Defined_Group_Before_They_Leave_In_The_Current_Academic_Year(string[] staffInformation, string[] newServiceDetail,
                                                                                                                                               string[] leavingDetails, string[] definedGroupDetails,
                                                                                                                                               string[] effectiveDateRange, string[] superviserInformation)
        {
            #region Pre-Condition: Create a staff and mark this staff as leaver

            //Login as School Personnel Officer
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.PersonnelOfficer);
            SeleniumHelper.NavigateMenu("Tasks", "Staff", "Staff Records");

            //Create a staff
            var staffRecordTriplet = new StaffRecordTriplet();
            var addNewStaffDialog = staffRecordTriplet.CreateStaff();

            //Fill Add New Staff Dialog                           
            addNewStaffDialog.Forename = staffInformation[0];
            addNewStaffDialog.SurName = staffInformation[1];
            addNewStaffDialog.Gender = staffInformation[2];

            //Fill Service Detail Dialog
            var serviceDetailDialog = addNewStaffDialog.Continue();
            serviceDetailDialog.DateOfArrival = newServiceDetail[0];
            var staffRecordPage = serviceDetailDialog.CreateRecord();

            //Save staff            
            staffRecordPage.SaveStaff();

            //Mark this staff as leaver
            var leavingDetailPage = SeleniumHelper.NavigateViaAction<StaffLeavingDetailPage>("Staff Leaving Details");
            leavingDetailPage.DateOfLeaving = leavingDetails[0];
            leavingDetailPage.ReasonForLeaving = leavingDetails[1];
            staffRecordPage = leavingDetailPage.Save();

            //Save staff            
            staffRecordPage.SaveStaff();

            //Logout
            SeleniumHelper.Logout();

            #endregion

            #region Steps

            //Login as School Administrator and navigate to 'Manage User Defined Groups'
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);
            SeleniumHelper.NavigateMenu("Tasks", "School Groups", "Manage User Defined Groups");

            //Click Create
            var userDefinedGroupTriplet = new ManageUserDefinedTriplet();
            var userDefinedGroupPage = userDefinedGroupTriplet.ClickCreateNew();

            //Add basic information
            userDefinedGroupPage.FullName = definedGroupDetails[0];
            userDefinedGroupPage.ShortName = definedGroupDetails[1];
            userDefinedGroupPage.Purpose = definedGroupDetails[2];
            userDefinedGroupPage.Notes = definedGroupDetails[3];
            userDefinedGroupPage.Visibility = true;
            userDefinedGroupPage = userDefinedGroupPage.SaveValue();

            //Add Effective Date Range
            var selectEffectiveDateRange = userDefinedGroupPage.SelectEffectDateRange();
            selectEffectiveDateRange.AcademicYear = effectiveDateRange[0];
            selectEffectiveDateRange.From = effectiveDateRange[1];
            selectEffectiveDateRange.To = effectiveDateRange[2];
            userDefinedGroupPage = selectEffectiveDateRange.ClickOK<ManageUserDefinedPage>();
            userDefinedGroupPage = userDefinedGroupPage.SaveValue();

            //Adding new Supervisors
            var supervisorTriplet = userDefinedGroupPage.ClickAddSupervisor();
            supervisorTriplet.SearchCriteria.SurName = staffInformation[0];
            supervisorTriplet.SearchCriteria.Role = superviserInformation[0];
            var supervisorDetailDialog = supervisorTriplet.SearchCriteria.Search<AddSupervisorsDialogDetail>();

            //Confirm Staff Member with a Leaving date in this Academic Year is available to Select as a Member
            Assert.AreEqual(true, supervisorDetailDialog.SearchResult.Any(p => p.GetText().Contains(staffInformation[0])), "Staff with leaving date is not available to select");

            //Select above staff to add to supervisor grid
            supervisorDetailDialog.SearchResult.FirstOrDefault(p => p.GetText().Contains(staffInformation[0])).ClickByJS();
            supervisorDetailDialog.AddSelectedSupervisor();
            userDefinedGroupPage = supervisorDetailDialog.ClickOK<ManageUserDefinedPage>();

            //Input detail information for supervisor
            userDefinedGroupPage.SupervisorTable[0].Role = superviserInformation[1];

            //Click Save
            userDefinedGroupPage.SaveValue();

            //Confirm User Defined Group is successfully created
            userDefinedGroupTriplet = new ManageUserDefinedTriplet();
            userDefinedGroupTriplet.SearchCriteria.FullName = definedGroupDetails[0];
            var groupTile = userDefinedGroupTriplet.SearchCriteria.Search().FirstOrDefault(x => x.GroupName.Equals(definedGroupDetails[0]));
            Assert.AreNotEqual(null, groupTile, "User Defined Group is not successfully created");

            #endregion

            #region End-Condition

            //Delete data in supervisor grid
            userDefinedGroupPage = groupTile.Click<ManageUserDefinedPage>();
            userDefinedGroupPage.SupervisorTable[0].DeleteRow();
            userDefinedGroupPage.SaveValue();

            //Delete User Defined Group was created
            userDefinedGroupPage.Delete();

            //Delete staff added
            SeleniumHelper.Logout();
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.PersonnelOfficer);
            SeleniumHelper.NavigateMenu("Tasks", "Staff", "Delete Staff Record");
            var deleteStaffRecordTriplet = new DeleteStaffRecordTriplet();
            deleteStaffRecordTriplet.SearchCriteria.StaffName = String.Format("{0}, {1}", staffInformation[0], staffInformation[1]);
            deleteStaffRecordTriplet.SearchCriteria.IsCurrent = true;
            deleteStaffRecordTriplet.SearchCriteria.IsLeaver = true;
            var staffTiles = deleteStaffRecordTriplet.SearchCriteria.Search();
            var staffRecord = staffTiles.FirstOrDefault(x => x.Name.Equals(String.Format("{0}, {1}", staffInformation[0], staffInformation[1]))).Click<StaffRecordPage>();
            staffRecord.DeleteStaff();

            #endregion

        }
        /// <summary>
        /// TC UDG-28
        /// Au : Y Ta
        /// Description: Amend a User Defined Group: Ensure a Member cannot be input twice with overlapping date ranges for the Current academic Year
        /// Role: School Administrator
        /// Status : Pending by bug #5 : Can not add new Member in User Defined Group Page - Only Chrome
        /// </summary>
        [WebDriverTest(TimeoutSeconds = 2000, Enabled = false, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { "g1" }, DataProvider = "TC_UDG28_Data")]
        public void TC_UDG28_Amend_User_Defined_Group_OverLap_Date_Range_Pupil_Current_Academic_Year(string fullName, string shortName, string purpose, string note, string pupilName,
            string fromDateBefore, string toDateBefore, string fromDateAfter, string toDateAfter, string pupilNameDisplay)
        {

            #region Pre-condtion

            // Login
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);

            // Navigate to Manage User Defined Group
            SeleniumHelper.NavigateMenu("Tasks", "School Groups", "Manage User Defined Groups");

            var manageUserDefinedTriplet = new ManageUserDefinedTriplet();

            // Create new Manage User Defined Group
            var manageUserDefinedPage = manageUserDefinedTriplet.ClickCreateNew();

            // Add group details
            manageUserDefinedPage.FullName = fullName;
            manageUserDefinedPage.ShortName = shortName;
            manageUserDefinedPage.Purpose = purpose;
            manageUserDefinedPage.Visibility = true;
            manageUserDefinedPage.Notes = note;

            // Save values
            manageUserDefinedPage.SaveValue();

            // Scroll to Member
            manageUserDefinedPage.Refresh();
            manageUserDefinedPage.ScrollToMember();

            // Add new pupil
            var pupilDialogTriplet = manageUserDefinedPage.ClickAddPupil();
            pupilDialogTriplet.SearchCriteria.SearchPupilName = pupilName;

            var pupilDialogPage = pupilDialogTriplet.SearchCriteria.Search<AddPupilsDetailsPage>();

            // Click to record
            pupilDialogPage.Pupils.FirstOrDefault(p => p.Text.Contains(pupilName)).ClickByJS();
            pupilDialogPage.AddSelectedPupils();

            // Click OK
            pupilDialogTriplet.Refresh();
            pupilDialogTriplet.ClickOk();

            // Change from date and to date
            manageUserDefinedPage.Refresh();
            var memberTable = manageUserDefinedPage.MemberTable;
            var row = memberTable.Rows.FirstOrDefault(x => x.Name.Contains(pupilNameDisplay));
            row.From = fromDateBefore;
            row.To = toDateBefore;

            // Save values
            manageUserDefinedPage.Refresh();
            manageUserDefinedPage.SaveValue();

            #endregion

            #region Test steps

            // Search record again
            manageUserDefinedTriplet = new ManageUserDefinedTriplet();
            manageUserDefinedTriplet.SearchCriteria.FullName = fullName;
            var results = manageUserDefinedTriplet.SearchCriteria.Search().FirstOrDefault(x => x.GroupName.Equals(fullName));
            manageUserDefinedPage = results.Click<ManageUserDefinedPage>();

            // Scroll to member
            manageUserDefinedPage.Refresh();
            manageUserDefinedPage.ScrollToMember();
            pupilDialogTriplet = manageUserDefinedPage.ClickAddPupil();

            // Enter value to search
            pupilDialogTriplet.SearchCriteria.SearchPupilName = pupilName.Split(',')[0];

            pupilDialogPage = pupilDialogTriplet.SearchCriteria.Search<AddPupilsDetailsPage>();

            // Click to record
            pupilDialogPage.Pupils.FirstOrDefault(x => x.Text.Contains(pupilName)).ClickByJS();
            pupilDialogPage.AddSelectedPupils();

            // Click OK
            pupilDialogTriplet.Refresh();
            pupilDialogTriplet.ClickOk();

            // Change from date and to date to make it overlape
            manageUserDefinedPage.Refresh();
            memberTable = manageUserDefinedPage.MemberTable;

            memberTable[1].From = memberTable[0].From;


            // Save values
            manageUserDefinedPage.Refresh();
            manageUserDefinedPage.SaveValue();

            // VP : Verify overlap message displays
            Assert.AreEqual(true, manageUserDefinedPage.IsErrorMessageDisplay(), "Overlap message does not display");

            #endregion

            #region Post-condition

            manageUserDefinedPage.Refresh();
            memberTable.Refresh();

            // Delete all member
            memberTable[0].DeleteRow();
            memberTable[1].DeleteRow();

            // Save value
            manageUserDefinedPage.Refresh();
            manageUserDefinedPage.SaveValue();

            // Delete record
            manageUserDefinedPage.Refresh();
            manageUserDefinedPage.Delete();

            #endregion
        }

        /// <summary>
        /// TC UDG-29
        /// Au : Hieu Pham
        /// Description: Amend a User Defined Group: Ensure a Member cannot be input twice with overlapping date ranges for the Current academic Year
        /// Role: School Administrator
        /// Status : Pending by bug #5 : Can not add new Member in User Defined Group Page - Only Chrome
        /// </summary>
        [WebDriverTest(TimeoutSeconds = 2000, Enabled = false, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { "g1" }, DataProvider = "TC_UDG29_Data")]
        public void TC_UDG29_Amend_User_Defined_Group_OverLap_Date_Range_Member_Current_Academic_Year(string fullName, string shortName, string purpose, string note, string roleSearch, string staffName,
            string fromDateBefore, string toDateBefore, string fromDateAfter, string toDateAfter)
        {

            #region Pre-condtion

            // Login
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);

            // Navigate to Manage User Defined Group
            SeleniumHelper.NavigateMenu("Tasks", "School Groups", "Manage User Defined Groups");

            var manageUserDefinedTriplet = new ManageUserDefinedTriplet();

            // Create new Manage User Defined Group
            var manageUserDefinedPage = manageUserDefinedTriplet.ClickCreateNew();

            // Add group details
            manageUserDefinedPage.FullName = fullName;
            manageUserDefinedPage.ShortName = shortName;
            manageUserDefinedPage.Purpose = purpose;
            manageUserDefinedPage.Visibility = true;
            manageUserDefinedPage.Notes = note;

            // Save values
            manageUserDefinedPage.SaveValue();

            // Scroll to Member
            manageUserDefinedPage.Refresh();
            manageUserDefinedPage.ScrollToMember();

            // Add new member
            var memberDialogTriplet = manageUserDefinedPage.ClickAddMember();
            memberDialogTriplet.SearchCriteria.SurName = staffName.Split(',')[0];
            memberDialogTriplet.SearchCriteria.Role = roleSearch;
            var memberDialogPage = memberDialogTriplet.SearchCriteria.Search<AddMemberDialogDetail>();

            // Click to record
            memberDialogPage.Members.FirstOrDefault(x => x.Text.Replace("  ", " ").Equals(staffName)).ClickByJS();
            memberDialogPage.AddSelectedMembers();

            // Click OK
            memberDialogTriplet.Refresh();
            memberDialogTriplet.ClickOk();

            // Change from date and to date
            manageUserDefinedPage.Refresh();
            var memberTable = manageUserDefinedPage.MemberTable;
            var row = memberTable.Rows.FirstOrDefault(x => x.Name.Equals(staffName));
            row.From = fromDateBefore;
            row.To = toDateBefore;

            // Save values
            manageUserDefinedPage.Refresh();
            manageUserDefinedPage.SaveValue();

            #endregion

            #region Test steps

            // Search record again
            manageUserDefinedTriplet = new ManageUserDefinedTriplet();
            manageUserDefinedTriplet.SearchCriteria.FullName = fullName;
            var results = manageUserDefinedTriplet.SearchCriteria.Search().FirstOrDefault(x => x.GroupName.Equals(fullName));
            manageUserDefinedPage = results.Click<ManageUserDefinedPage>();

            // Scroll to member
            manageUserDefinedPage.Refresh();
            manageUserDefinedPage.ScrollToMember();
            memberDialogTriplet = manageUserDefinedPage.ClickAddMember();

            // Enter value to search
            memberDialogTriplet.SearchCriteria.SurName = staffName.Split(',')[0];
            memberDialogTriplet.SearchCriteria.Role = roleSearch;
            memberDialogPage = memberDialogTriplet.SearchCriteria.Search<AddMemberDialogDetail>();

            // Click to record
            memberDialogPage.Members.FirstOrDefault(x => x.Text.Replace("  ", " ").Equals(staffName)).ClickByJS();
            memberDialogPage.AddSelectedMembers();

            // Click OK
            memberDialogTriplet.Refresh();
            memberDialogTriplet.ClickOk();

            // Change from date and to date
            manageUserDefinedPage.Refresh();
            memberTable = manageUserDefinedPage.MemberTable;
            row = memberTable.Rows.FirstOrDefault(x => x.To.Equals(toDateAfter));
            row.From = fromDateAfter;
            row.To = toDateAfter;

            // Save values
            manageUserDefinedPage.Refresh();
            manageUserDefinedPage.SaveValue();

            // VP : Verify overlap message displays
            Assert.AreEqual(true, manageUserDefinedPage.IsErrorMessageDisplay(), "Overlap message does not display");

            #endregion

            #region Post-condition

            manageUserDefinedPage.Refresh();
            memberTable.Refresh();

            // Delete all member
            row = memberTable.Rows.FirstOrDefault(x => x.From.Equals(fromDateAfter));
            row.DeleteRow();
            row = memberTable.Rows.FirstOrDefault(x => x.To.Equals(toDateBefore));
            row.DeleteRow();

            // Save value
            manageUserDefinedPage.Refresh();
            manageUserDefinedPage.SaveValue();

            // Delete record
            manageUserDefinedPage.Refresh();
            manageUserDefinedPage.Delete();

            #endregion
        }


        /// <summary>
        /// TC UDG- 30
        /// Au : Huy Vo
        /// Description: Amend a User Defined Group: Ensure the same Pupil can be input twice with non overlapping date ranges for the Current academic Year
        /// Role: School Administrator
        /// Status : Pending by bug #5 : Can not add new member in User Defined Group Page - both Chrome and IE
        /// </summary>
        [WebDriverTest(TimeoutSeconds = 1000, Enabled = false, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { "g1" }, DataProvider = "TC_UDG30_Data")]
        public void TC_UDG30_Amend_UDG_Ensure_The_Same_Pupil_Can_Be_Input_Twice_Non_Overlapping_Date_Ranges_Current_Academic_Year(
            string fullName, string shortName, string purpose, string note, string supervisorName,
            string pupilName, string fromDate1, string toDate1, string fromDate2, string toDate2)
        {
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);

            #region Test steps

            //Navigate to Manage User Defined Group
            SeleniumHelper.NavigateMenu("Tasks", "School Groups", "Manage User Defined Groups");
            var manageUserDefinedTriplet = new ManageUserDefinedTriplet();

            //Create new Manage User Defined Group
            var manageUserDefinedPage = manageUserDefinedTriplet.ClickCreateNew();

            //Add group details
            manageUserDefinedPage.FullName = fullName;
            manageUserDefinedPage.ShortName = shortName;
            manageUserDefinedPage.Purpose = purpose;
            manageUserDefinedPage.Visibility = true;
            manageUserDefinedPage.Notes = note;

            //Save infor
            manageUserDefinedPage.SaveValue();

            //Add 1 supervisors
            var addSupervisors = manageUserDefinedPage.ClickAddSupervisor();
            addSupervisors.SearchCriteria.Role = "Staff";
            var addSupervisorsDetail = addSupervisors.SearchCriteria.Search<AddSupervisorsDialogDetail>();
            var supervisorsSearchResult = addSupervisorsDetail.SearchResult;
            var supervisor = supervisorsSearchResult.FirstOrDefault(t => t.GetText().Equals(supervisorName));
            supervisor.ClickByJS();

            // Select Supervisor
            addSupervisorsDetail.AddSelectedSupervisor();
            addSupervisors.ClickOk();
            var supervisorTable = manageUserDefinedPage.SupervisorTable;
            supervisorTable.WaitUntilRowAppear(t => t.Name.Equals(supervisorName));
            supervisorTable[0].Role = "Supervisor";

            //Add pupil
            manageUserDefinedPage.ScrollToMember();
            var addPupils = manageUserDefinedPage.ClickAddPupil();
            addPupils.SearchCriteria.SearchPupilName = pupilName;
            var addPupilDetail = addPupils.SearchCriteria.Search<AddPupilsDialogDetail>();
            var pupilsSearchResult = addPupilDetail.Pupils;
            var pupil = pupilsSearchResult.FirstOrDefault(t => t.GetText().Equals(pupilName));
            pupil.ClickByJS();
            addPupilDetail.AddSelectedPupils();
            addPupils.ClickOk();

            var memberTable = manageUserDefinedPage.MemberTable;
            memberTable.WaitUntilRowAppear(t => t.Name.Contains(pupilName));

            //Add From date and to date for pupil
            manageUserDefinedPage.ScrollToMember();
            supervisorTable = manageUserDefinedPage.SupervisorTable;
            var pupilRow = memberTable.Rows.FirstOrDefault(t => t.Name.Contains(pupilName));
            pupilRow.From = fromDate1;
            pupilRow.To = toDate1;

            //Save
            manageUserDefinedPage.SaveValue();
            manageUserDefinedPage.Refresh();

            //Search User Define Group that has just created to add more pupil
            manageUserDefinedTriplet.Refresh();
            manageUserDefinedTriplet.SearchCriteria.FullName = fullName;
            var userDefinedSearchResult = manageUserDefinedTriplet.SearchCriteria.Search();
            var userDefinedTile = userDefinedSearchResult.FirstOrDefault(t => t.GroupName.Equals(fullName));
            Assert.AreNotEqual(null, userDefinedTile, "Cannot create new user defined group");
            manageUserDefinedPage = userDefinedTile.Click<ManageUserDefinedPage>();

            //Add the same pupil with non overlap from date and to date
            manageUserDefinedPage.ScrollToMember();
            addPupils = manageUserDefinedPage.ClickAddPupil();
            addPupils.SearchCriteria.SearchPupilName = pupilName;
            addPupilDetail = addPupils.SearchCriteria.Search<AddPupilsDialogDetail>();
            pupilsSearchResult = addPupilDetail.Pupils;
            pupil = pupilsSearchResult.FirstOrDefault(t => t.GetText().Equals(pupilName));
            pupil.ClickByJS();
            addPupilDetail.AddSelectedPupils();
            addPupils.ClickOk();

            memberTable = manageUserDefinedPage.MemberTable;
            memberTable.WaitUntilRowAppear(t => t.Name.Contains(pupilName));
            supervisorTable = manageUserDefinedPage.SupervisorTable;
            memberTable[1].From = fromDate2;
            memberTable[1].To = toDate2;

            //Save
            manageUserDefinedPage.SaveValue();
            manageUserDefinedPage.Refresh();

            // Verify that success message exist
            Assert.AreEqual(true, manageUserDefinedTriplet.IsSuccessMessageExist(), "Pupil can not be selected as a member twice with a non overlapping dates in this Academic Year");

            // Search and verify that update UDG successfully
            manageUserDefinedTriplet.Refresh();
            manageUserDefinedTriplet.SearchCriteria.FullName = fullName;
            userDefinedSearchResult = manageUserDefinedTriplet.SearchCriteria.Search();
            userDefinedTile = userDefinedSearchResult.FirstOrDefault(t => t.GroupName.Equals(fullName));
            manageUserDefinedPage = userDefinedTile.Click<ManageUserDefinedPage>();

            manageUserDefinedPage.ScrollToMember();
            memberTable = manageUserDefinedPage.MemberTable;
            var memberRow = memberTable.Rows.FirstOrDefault(t => t.Name.Contains(pupilName) && t.From.Equals(fromDate1) && t.To.Equals(toDate1));
            Assert.AreNotEqual(null, memberRow, "Pupil can not be selected as a member twice with a non overlapping dates in this Academic Year");
            memberRow = memberTable.Rows.FirstOrDefault(t => t.Name.Contains(pupilName) && t.From.Equals(fromDate2) && t.To.Equals(toDate2));
            Assert.AreNotEqual(null, memberRow, "Pupil can not be selected as a member twice with a non overlapping dates in this Academic Year");

            #endregion

            #region Post-Condition : Delete User defined group

            //Delete pupil
            memberTable = manageUserDefinedPage.MemberTable;
            memberRow = memberTable.Rows.FirstOrDefault(t => t.Name.Contains(pupilName) && t.From.Equals(fromDate1) && t.To.Equals(toDate1));
            memberRow.DeleteRow();
            memberTable.Refresh();
            memberRow = memberTable.Rows.FirstOrDefault(t => t.Name.Contains(pupilName) && t.From.Equals(fromDate2) && t.To.Equals(toDate2));
            memberRow.DeleteRow();
            memberTable.Refresh();

            //Delete supervisor
            supervisorTable = manageUserDefinedPage.SupervisorTable;
            var supervisorRow = supervisorTable.Rows.FirstOrDefault(t => t.Name.Equals(supervisorName));
            supervisorRow.DeleteRow();
            supervisorTable.Refresh();

            //Save
            manageUserDefinedPage.SaveValue();
            manageUserDefinedPage.Refresh();
            manageUserDefinedPage.Delete();

            #endregion
        }


        /// <summary>
        /// TC UDG- 31
        /// Au : Huy Vo
        /// Description: Amend a User Defined Group: Ensure the same Member can be input twice with non overlapping date ranges for the Current academic Year
        /// Status : Pending by bug #5 : Can not add new member in User Defined Group Page - both Chrome and IE
        /// Role: School Administrator
        /// </summary>
        [WebDriverTest(TimeoutSeconds = 1000, Enabled = false, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { "g1" }, DataProvider = "TC_UDG31_Data")]
        public void TC_UDG31_Amend_UDG_Ensure_The_Same_Member_Can_Be_Input_Twice_With_Non_Overlapping_Date_Ranges_For_Current_Academic_Year(
            string fullName, string shortName, string purpose, string note,
            string supervisorName,
            string memberName, string fromDate1, string toDate1, string fromDate2, string toDate2)
        {
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);

            #region Test steps

            //Navigate to Manage User Defined Group
            SeleniumHelper.NavigateMenu("Tasks", "School Groups", "Manage User Defined Groups");
            var manageUserDefinedTriplet = new ManageUserDefinedTriplet();

            //Create new Manage User Defined Group
            var manageUserDefinedPage = manageUserDefinedTriplet.ClickCreateNew();

            //Add group details
            manageUserDefinedPage.FullName = fullName;
            manageUserDefinedPage.ShortName = shortName;
            manageUserDefinedPage.Purpose = purpose;
            manageUserDefinedPage.Visibility = true;
            manageUserDefinedPage.Notes = note;

            //Save infor
            manageUserDefinedPage.SaveValue();

            //Add 1 supervisors
            var addSupervisors = manageUserDefinedPage.ClickAddSupervisor();
            addSupervisors.SearchCriteria.Role = "Staff";
            var addSupervisorsDetail = addSupervisors.SearchCriteria.Search<AddSupervisorsDialogDetail>();
            var supervisorsSearchResult = addSupervisorsDetail.SearchResult;
            var supervisor = supervisorsSearchResult.FirstOrDefault(t => t.GetText().Equals(supervisorName));
            supervisor.ClickByJS();

            // Select Supervisor
            addSupervisorsDetail.AddSelectedSupervisor();
            addSupervisors.ClickOk();
            var supervisorTable = manageUserDefinedPage.SupervisorTable;
            supervisorTable.WaitUntilRowAppear(t => t.Name.Equals(supervisorName));
            supervisorTable[0].Role = "Supervisor";

            //Add member
            var addMembers = manageUserDefinedPage.ClickAddMember();
            addMembers.SearchCriteria.Role = "Staff";
            var addMemberDetail = addMembers.SearchCriteria.Search<AddMemberDialogDetail>();
            var membersSearchResult = addMemberDetail.Members;
            var member = membersSearchResult.FirstOrDefault(t => t.GetText().Replace("  ", " ").Equals(memberName));
            member.ClickByJS();
            addMemberDetail.AddSelectedMembers();
            addMembers.ClickOk();

            var memberTable = manageUserDefinedPage.MemberTable;
            memberTable.WaitUntilRowAppear(t => t.Name.Equals(memberName));

            //Add From date and To date for member
            memberTable[0].From = fromDate1;
            memberTable[0].To = toDate1;

            //Save
            manageUserDefinedPage.SaveValue();
            manageUserDefinedPage.Refresh();

            //Search User Define Group that has just created to add more member
            manageUserDefinedTriplet.Refresh();
            manageUserDefinedTriplet.SearchCriteria.FullName = fullName;
            var userDefinedSearchResult = manageUserDefinedTriplet.SearchCriteria.Search();
            var userDefinedTile = userDefinedSearchResult.FirstOrDefault(t => t.GroupName.Equals(fullName));
            Assert.AreNotEqual(null, userDefinedTile, "Cannot create new user defined group");
            manageUserDefinedPage = userDefinedTile.Click<ManageUserDefinedPage>();

            //Add the same pupil with non overlap from date and to date
            addMembers = manageUserDefinedPage.ClickAddMember();
            addMembers.SearchCriteria.Role = "Staff";
            addMemberDetail = addMembers.SearchCriteria.Search<AddMemberDialogDetail>();
            membersSearchResult = addMemberDetail.Members;
            member = membersSearchResult.FirstOrDefault(t => t.GetText().Equals(memberName));
            member.ClickByJS();
            addMemberDetail.AddSelectedMembers();
            addMembers.ClickOk();
            memberTable = manageUserDefinedPage.MemberTable;
            memberTable.WaitUntilRowAppear(t => t.Name.Equals(memberName));

            memberTable = manageUserDefinedPage.MemberTable;
            memberTable[1].From = fromDate2;
            memberTable[1].To = toDate2;

            //Save
            manageUserDefinedPage.SaveValue();
            manageUserDefinedPage.Refresh();

            // Verify that success message exist
            Assert.AreEqual(true, manageUserDefinedTriplet.IsSuccessMessageExist(), "Pupil can not be selected as a member twice with a non overlapping dates in this Academic Year");

            // Search and verify that update UDG successfully
            manageUserDefinedTriplet.Refresh();
            manageUserDefinedTriplet.SearchCriteria.FullName = fullName;
            userDefinedSearchResult = manageUserDefinedTriplet.SearchCriteria.Search();
            userDefinedTile = userDefinedSearchResult.FirstOrDefault(t => t.GroupName.Equals(fullName));
            manageUserDefinedPage = userDefinedTile.Click<ManageUserDefinedPage>();

            manageUserDefinedPage.ScrollToMember();
            memberTable = manageUserDefinedPage.MemberTable;
            var memberRow = memberTable.Rows.FirstOrDefault(t => t.Name.Contains(memberName) && t.From.Equals(fromDate1) && t.To.Equals(toDate1));
            Assert.AreNotEqual(null, memberRow, "Pupil can not be selected as a member twice with a non overlapping dates in this Academic Year");
            memberRow = memberTable.Rows.FirstOrDefault(t => t.Name.Contains(memberName) && t.From.Equals(fromDate2) && t.To.Equals(toDate2));
            Assert.AreNotEqual(null, memberRow, "Pupil can not be selected as a member twice with a non overlapping dates in this Academic Year");

            #endregion

            #region Post-Condition : Delete User defined group

            //Delete pupil
            memberTable = manageUserDefinedPage.MemberTable;
            memberRow = memberTable.Rows.FirstOrDefault(t => t.Name.Contains(memberName) && t.From.Equals(fromDate1) && t.To.Equals(toDate1));
            memberRow.DeleteRow();
            memberTable.Refresh();
            memberRow = memberTable.Rows.FirstOrDefault(t => t.Name.Contains(memberName) && t.From.Equals(fromDate2) && t.To.Equals(toDate2));
            memberRow.DeleteRow();
            memberTable.Refresh();

            //Delete supervisor
            supervisorTable = manageUserDefinedPage.SupervisorTable;
            var supervisorRow = supervisorTable.Rows.FirstOrDefault(t => t.Name.Equals(supervisorName));
            supervisorRow.DeleteRow();
            supervisorTable.Refresh();

            //Save
            manageUserDefinedPage.SaveValue();
            manageUserDefinedPage.Refresh();
            manageUserDefinedPage.Delete();

            #endregion
        }


        /// <summary>
        /// TC UDG-41
        /// Au : Hieu Pham
        /// Description: Amend a User Defined Group: For a Pupil Leaving in the Current Academic Year, ensure they can still be selected as a  Supervisor for a User Defined Group before they leave in the Current Academic Year 
        /// Role: School Administrator
        /// Status : Pending by bug #4 : Can not add new Supervisor in User Defined Group Page.
        /// </summary>
        [WebDriverTest(TimeoutSeconds = 2000, Enabled = false, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { "g1" }, DataProvider = "TC_UDG41_Data")]
        public void TC_UDG41_Amend_User_Defined_Group_For_Pupil_Leaving_In_Current_Academic_Year_As_A_Supervisor(string[] pupil, string fullName, string shortName, string purpose, string note, string academicYear, string fromDate,
            string toDate, string roleSearch, string pupilRole)
        {

            #region Pre-condtion

            // Login
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);

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
            registrationDetailDialog.YearGroup = pupil[5];
            registrationDetailDialog.CreateRecord();

            // Confirm create new pupil
            var confirmRequiredDialog = new POM.Components.Common.ConfirmRequiredDialog();
            confirmRequiredDialog.ClickOk();

            // Save values
            var pupilRecord = PupilRecordPage.Create();
            pupilRecord.SavePupil();

            // Navigate to pupil leaving detail
            var pupilLeavingDetail = SeleniumHelper.NavigateViaAction<PupilLeavingDetailsPage>("Pupil Leaving Details");

            // Enter values
            pupilLeavingDetail.DOL = pupil[6];
            pupilLeavingDetail.ReasonForLeaving = pupil[7];

            // Save values
            var confirmDialog = pupilLeavingDetail.ClickSave();
            confirmDialog.ClickOk();
            var leaverBackgroundDialog = new LeaverBackgroundProcessSubmitDialog();
            leaverBackgroundDialog.ClickOk();

            #endregion

            #region Test steps

            // Navigate to Manage User Defined Group
            SeleniumHelper.NavigateMenu("Tasks", "School Groups", "Manage User Defined Groups");

            var manageUserDefinedTriplet = new ManageUserDefinedTriplet();

            // Click Search to see all record
            manageUserDefinedTriplet.SearchCriteria.Search();

            // Create new Manage User Defined Group
            var manageUserDefinedPage = manageUserDefinedTriplet.ClickCreateNew();

            // Add group details
            manageUserDefinedPage.FullName = fullName;
            manageUserDefinedPage.ShortName = shortName;
            manageUserDefinedPage.Purpose = purpose;
            manageUserDefinedPage.Visibility = true;
            manageUserDefinedPage.Notes = note;

            // Save values
            manageUserDefinedPage.SaveValue();

            // Select Effective Date Range
            manageUserDefinedPage.Refresh();
            var effectiveDateRangeDialog = manageUserDefinedPage.SelectEffectDateRange();
            effectiveDateRangeDialog.From = fromDate;
            effectiveDateRangeDialog.To = toDate;
            effectiveDateRangeDialog.ClickOk();

            // Add supervisor
            manageUserDefinedPage.Refresh();
            manageUserDefinedPage.ScrollToSupervisor();
            var supervisorDialogTriplet = manageUserDefinedPage.ClickAddSupervisor();
            supervisorDialogTriplet.SearchCriteria.SurName = pupil[1];
            supervisorDialogTriplet.SearchCriteria.Role = roleSearch;
            var supervisorDialogPage = supervisorDialogTriplet.SearchCriteria.Search<AddSupervisorsDialogDetail>();

            // Verify search result is correct
            Assert.AreNotEqual(0, supervisorDialogPage.SearchResult.Count, "Search Pupil function is incorrect.");

            // Click to record
            supervisorDialogPage.SearchResult.FirstOrDefault(x => x.Text.Replace("  ", " ").Equals(String.Format("{0}, {1}", pupil[1], pupil[0]))).ClickByJS();
            supervisorDialogPage.AddSelectedSupervisor();

            // Click OK
            supervisorDialogTriplet.Refresh();
            supervisorDialogTriplet.ClickOk();

            // Select role for new record
            manageUserDefinedPage.Refresh();
            var supervisorTable = manageUserDefinedPage.SupervisorTable;
            var lastInsertRow = supervisorTable.Rows.FirstOrDefault(x => x.Role.Equals(String.Empty));
            lastInsertRow.Role = pupilRole;
            lastInsertRow.Main = true;
            lastInsertRow.From = fromDate;
            lastInsertRow.To = toDate;

            // Save values
            manageUserDefinedPage.SaveValue();

            // Search record again
            manageUserDefinedTriplet.Refresh();
            manageUserDefinedTriplet.SearchCriteria.FullName = fullName;
            var searchResult = manageUserDefinedTriplet.SearchCriteria.Search();
            manageUserDefinedPage = searchResult.FirstOrDefault(x => x.GroupName.Equals(fullName)).Click<ManageUserDefinedPage>();

            // VP : User Defined Group is successfully created
            Assert.AreEqual(fullName, manageUserDefinedPage.FullName, "User Defined Group is not successfully created");

            // Scroll to supervisor
            manageUserDefinedPage.ScrollToSupervisor();
            supervisorTable = manageUserDefinedPage.SupervisorTable;
            var row = supervisorTable.Rows.FirstOrDefault(x => x.Name.Equals(String.Format("{0}, {1}", pupil[1], pupil[0])) && x.Role.Equals(pupilRole) && x.From.Equals(fromDate) && x.To.Equals(toDate));

            // VP : Pupil with a Leaving date in this Academic Year is available to Select as a Supervisor
            Assert.AreNotEqual(null, row, "Pupil with a Leaving date in this Academic Year is not available to Select as a Supervisor");

            #endregion

            #region Post-condition

            // Delete all supervisor
            row.DeleteRow();

            // Save value
            manageUserDefinedPage.Refresh();
            manageUserDefinedPage.SaveValue();

            // Delete user defined record
            manageUserDefinedPage.Refresh();
            manageUserDefinedPage.Delete();

            // Delete pupil if existed
            SeleniumHelper.NavigateMenu("Tasks", "Pupils", "Delete Pupil");
            deletePupilRecordTriplet = new DeletePupilRecordTriplet();
            deletePupilRecordTriplet.SearchCriteria.PupilName = String.Format("{0}, {1}", pupil[1], pupil[0]);
            resultPupils = deletePupilRecordTriplet.SearchCriteria.Search();
            pupilSearchTile = resultPupils.SingleOrDefault(t => t.Name.Equals(String.Format("{0}, {1}", pupil[1], pupil[0])));
            deletePupilRecord = pupilSearchTile == null ? new DeletePupilRecordPage() : pupilSearchTile.Click<DeletePupilRecordPage>();
            deletePupilRecord.Delete();



            #endregion
        }

        /// <summary>
        /// Author: Ba.Truong
        /// Description: School Administrator,  For a Pupil Leaving in the Current Academic Year, 
        ///              ensure they can still be selected as a  Pupil for a User Defined Group before they leave in the Current Academic Year 
        /// Status : Pending by bug #5 : Can not add new Pupils in User Defined Group Page
        /// </summary>
        [WebDriverTest(TimeoutSeconds = 3000, Enabled = false, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { "g1" }, DataProvider = "TC_UDG042_Data")]
        public void TC_UDG042_Ensure_Pupil_Can_Be_Selected_As_A_Pupil_For_A_User_Defined_Group_Before_They_Leave_In_The_Current_Academic_Year(string[] pupilRecords, string[] leavingDetails,
                                                                                                                                              string[] definedGroupDetails, string[] effectiveDateRange)
        {
            #region Pre-Condition: Create a pupil and mark this pupil as leaver

            //Login as SchoolAdministrator and navigate to "Pupil Records"
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);
            SeleniumHelper.NavigateMenu("Tasks", "Pupils", "Pupil Records");

            //Create a pupil
            var pupilRecordTriplet = new PupilRecordTriplet();
            var addNewPupilDialog = pupilRecordTriplet.AddNewPupil();
            addNewPupilDialog.Forename = pupilRecords[0];
            addNewPupilDialog.SurName = pupilRecords[1];
            addNewPupilDialog.Gender = pupilRecords[2];
            addNewPupilDialog.DateOfBirth = pupilRecords[3];

            var registrationDetailDialog = addNewPupilDialog.Continue();
            registrationDetailDialog.DateOfAdmission = pupilRecords[4];
            registrationDetailDialog.EnrolmentStatus = pupilRecords[5];
            registrationDetailDialog.YearGroup = pupilRecords[6];
            registrationDetailDialog.ClassName = pupilRecords[7];
            registrationDetailDialog.CreateRecord();

            var confirmRequiredDialog = new POM.Components.Common.ConfirmRequiredDialog();
            confirmRequiredDialog.ClickOk();

            var pupilRecord = new PupilRecordPage();
            pupilRecord.ClickSave();

            //Mark this pupil as leaver
            var pupilLeavingDetails = SeleniumHelper.NavigateViaAction<PupilLeavingDetailsPage>("Pupil Leaving Details");
            pupilLeavingDetails.DOL = leavingDetails[0];
            pupilLeavingDetails.ReasonForLeaving = leavingDetails[1];
            pupilLeavingDetails.Destination = leavingDetails[2];
            var confirmDialog = pupilLeavingDetails.ClickSave();

            //Select the next two continue options in order to make this pupil a future dated leaver
            confirmDialog.ClickOk();
            var confirmLeaver = new LeaverBackgroundProcessSubmitDialog();
            confirmLeaver.ClickOk();

            //Save value for this pupil
            pupilRecord = new PupilRecordPage();
            pupilRecord.ClickSave();

            #endregion

            #region Steps

            //Navigate to 'Manage User Defined Groups'
            SeleniumHelper.NavigateMenu("Tasks", "School Groups", "Manage User Defined Groups");

            //Click Create
            var userDefinedGroupTriplet = new ManageUserDefinedTriplet();
            var userDefinedGroupPage = userDefinedGroupTriplet.ClickCreateNew();

            //Add basic information
            userDefinedGroupPage.FullName = definedGroupDetails[0];
            userDefinedGroupPage.ShortName = definedGroupDetails[1];
            userDefinedGroupPage.Purpose = definedGroupDetails[2];
            userDefinedGroupPage.Notes = definedGroupDetails[3];
            userDefinedGroupPage.Visibility = true;
            userDefinedGroupPage = userDefinedGroupPage.SaveValue();

            //Add Effective Date Range
            var selectEffectiveDateRange = userDefinedGroupPage.SelectEffectDateRange();
            selectEffectiveDateRange.AcademicYear = effectiveDateRange[0];
            selectEffectiveDateRange.From = effectiveDateRange[1];
            selectEffectiveDateRange.To = effectiveDateRange[2];
            userDefinedGroupPage = selectEffectiveDateRange.ClickOK<ManageUserDefinedPage>();
            userDefinedGroupPage = userDefinedGroupPage.SaveValue();

            //Adding new Pupils
            var pupilTriplet = userDefinedGroupPage.ClickAddPupil();
            pupilTriplet.SearchCriteria.SearchPupilName = pupilRecords[0];
            pupilTriplet.SearchCriteria.YearGroup = pupilRecords[6];
            pupilTriplet.SearchCriteria.Class = pupilRecords[7];
            pupilTriplet.SearchCriteria.IsLeaver = true;
            var pupilDetailDialog = pupilTriplet.SearchCriteria.Search<AddPupilsDialogDetail>();

            //Confirm Pupil with a Leaving date in this Academic Year is available to Select as a Member
            Assert.AreEqual(true, pupilDetailDialog.Pupils.Any(p => p.GetText().Contains(pupilRecords[0])), "Pupil with leaving date is not available to select");

            //Select above staff to add to supervisor grid
            pupilDetailDialog.Pupils.FirstOrDefault(p => p.GetText().Contains(pupilRecords[0])).ClickByJS();
            pupilDetailDialog.AddSelectedPupils();
            userDefinedGroupPage = pupilDetailDialog.ClickOK<ManageUserDefinedPage>();

            //Click Save
            userDefinedGroupPage.SaveValue();

            //Confirm User Defined Group is successfully created
            userDefinedGroupTriplet = new ManageUserDefinedTriplet();
            userDefinedGroupTriplet.SearchCriteria.FullName = definedGroupDetails[0];
            var groupTile = userDefinedGroupTriplet.SearchCriteria.Search().FirstOrDefault(x => x.GroupName.Equals(definedGroupDetails[0]));
            Assert.AreNotEqual(null, groupTile, "User Defined Group is not successfully created");

            #endregion

            #region End-Condition

            //Delete data in pupil grid
            userDefinedGroupPage = groupTile.Click<ManageUserDefinedPage>();
            userDefinedGroupPage.MemberTable[0].DeleteRow();
            userDefinedGroupPage.SaveValue();

            //Delete User Defined Group was created
            userDefinedGroupPage.Delete();

            //Delete pupil was added
            SeleniumHelper.NavigateMenu("Tasks", "Pupils", "Delete Pupil");
            var deletePupilRecordTriplet = new DeletePupilRecordTriplet();
            deletePupilRecordTriplet.SearchCriteria.PupilName = pupilRecords[0];
            deletePupilRecordTriplet.SearchCriteria.IsCurrent = true;
            deletePupilRecordTriplet.SearchCriteria.IsLeaver = true;
            var pupilSearchTile = deletePupilRecordTriplet.SearchCriteria.Search().FirstOrDefault(x => x.Name.Equals(String.Format("{0}, {1}", pupilRecords[0], pupilRecords[1])));
            var pupilRecordDelete = pupilSearchTile.Click<DeletePupilRecordPage>();
            pupilRecordDelete.Delete();

            #endregion

        }

        /// <summary>
        /// TC UDG-43
        /// Au : Hieu Pham
        /// Description: Amend a User Defined Group: For a Pupil Leaving in the Current Academic Year, ensure they can still be selected as a  Member for a User Defined Group before they leave in the Current Academic Year 
        /// Role: School Administrator
        /// Status : Pending by Bug #5 : [USER DEFINED GROUP] Can not add new Member in User Defined Group Page.
        /// </summary>
        [WebDriverTest(TimeoutSeconds = 2000, Enabled = false, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { "g1" }, DataProvider = "TC_UDG43_Data")]
        public void TC_UDG43_Amend_User_Defined_Group_For_Pupil_Leaving_In_Current_Academic_Year_As_A_Member(string[] pupil, string fullName, string shortName, string purpose, string note, string fromDate,
            string toDate, string roleSearch)
        {

            #region Pre-condtion

            // Login
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);

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
            registrationDetailDialog.YearGroup = pupil[5];
            registrationDetailDialog.CreateRecord();

            // Confirm create new pupil
            var confirmRequiredDialog = new POM.Components.Common.ConfirmRequiredDialog();
            confirmRequiredDialog.ClickOk();

            // Save values
            var pupilRecord = PupilRecordPage.Create();
            pupilRecord.SavePupil();

            // Navigate to pupil leaving detail
            var pupilLeavingDetail = SeleniumHelper.NavigateViaAction<PupilLeavingDetailsPage>("Pupil Leaving Details");

            // Enter values
            pupilLeavingDetail.DOL = pupil[6];
            pupilLeavingDetail.ReasonForLeaving = pupil[7];

            // Save values
            var confirmDialog = pupilLeavingDetail.ClickSave();
            confirmDialog.ClickOk();
            var leaverBackgroundDialog = new LeaverBackgroundProcessSubmitDialog();
            leaverBackgroundDialog.ClickOk();

            #endregion

            #region Test steps

            // Navigate to Manage User Defined Group
            SeleniumHelper.NavigateMenu("Tasks", "School Groups", "Manage User Defined Groups");

            var manageUserDefinedTriplet = new ManageUserDefinedTriplet();

            // Click Search to see all record
            manageUserDefinedTriplet.SearchCriteria.Search();

            // Create new Manage User Defined Group
            var manageUserDefinedPage = manageUserDefinedTriplet.ClickCreateNew();

            // Add group details
            manageUserDefinedPage.FullName = fullName;
            manageUserDefinedPage.ShortName = shortName;
            manageUserDefinedPage.Purpose = purpose;
            manageUserDefinedPage.Visibility = true;
            manageUserDefinedPage.Notes = note;

            // Save values
            manageUserDefinedPage.SaveValue();

            // Select Effective Date Range
            manageUserDefinedPage.Refresh();
            var effectiveDateRangeDialog = manageUserDefinedPage.SelectEffectDateRange();
            effectiveDateRangeDialog.From = fromDate;
            effectiveDateRangeDialog.To = toDate;
            effectiveDateRangeDialog.ClickOk();

            // Add member
            manageUserDefinedPage.Refresh();
            manageUserDefinedPage.ScrollToMember();
            var memberDialogTriplet = manageUserDefinedPage.ClickAddMember();
            memberDialogTriplet.SearchCriteria.SurName = pupil[1];
            memberDialogTriplet.SearchCriteria.Role = roleSearch;
            var memberDialogPage = memberDialogTriplet.SearchCriteria.Search<AddMemberDialogDetail>();

            // PENDING BY BUG #5 : [USER DEFINED GROUP] Can not add new Member in User Defined Group Page.
            Assert.AreNotEqual(0, memberDialogPage.Members.Count, "Search pupil function is not correct with pupil leaving in current academic year");

            // Click to record
            memberDialogPage.Members.FirstOrDefault(x => x.Text.Replace("  ", " ").Equals(String.Format("{0}, {1}", pupil[1], pupil[0]))).ClickByJS();
            memberDialogPage.AddSelectedMembers();

            // Click OK
            memberDialogTriplet.Refresh();
            memberDialogTriplet.ClickOk();

            // Enter From Date and To Date
            manageUserDefinedPage.Refresh();
            var memberTable = manageUserDefinedPage.MemberTable;
            var memberRow = memberTable.Rows.FirstOrDefault(x => x.Name.Equals(String.Format("{0}, {1}", pupil[1], pupil[0])));
            memberRow.From = fromDate;
            memberRow.To = toDate;

            // Save values
            manageUserDefinedPage.SaveValue();

            // Search record again
            manageUserDefinedTriplet.Refresh();
            manageUserDefinedTriplet.SearchCriteria.FullName = fullName;
            var searchResult = manageUserDefinedTriplet.SearchCriteria.Search();
            manageUserDefinedPage = searchResult.FirstOrDefault(x => x.GroupName.Equals(fullName)).Click<ManageUserDefinedPage>();

            // VP : User Defined Group is successfully created
            Assert.AreEqual(fullName, manageUserDefinedPage.FullName, "User Defined Group is not successfully created");

            // Scroll to member
            manageUserDefinedPage.ScrollToMember();
            memberTable = manageUserDefinedPage.MemberTable;
            memberRow = memberTable.Rows.FirstOrDefault(x => x.Name.Equals(String.Format("{0}, {1}", pupil[1], pupil[0])) && x.From.Equals(fromDate) && x.To.Equals(toDate));

            // VP : Pupil with a Leaving date in this Academic Year is available to Select as a Member
            Assert.AreNotEqual(null, memberRow, "Pupil with a Leaving date in this Academic Year is not available to Select as a Supervisor");

            #endregion

            #region Post-condition

            // Delete all supervisor
            memberRow.DeleteRow();

            // Save value
            manageUserDefinedPage.Refresh();
            manageUserDefinedPage.SaveValue();

            // Delete user defined record
            manageUserDefinedPage.Refresh();
            manageUserDefinedPage.Delete();

            // Delete pupil if existed
            SeleniumHelper.NavigateMenu("Tasks", "Pupils", "Delete Pupil");
            deletePupilRecordTriplet = new DeletePupilRecordTriplet();
            deletePupilRecordTriplet.SearchCriteria.PupilName = String.Format("{0}, {1}", pupil[1], pupil[0]);
            resultPupils = deletePupilRecordTriplet.SearchCriteria.Search();
            pupilSearchTile = resultPupils.SingleOrDefault(t => t.Name.Equals(String.Format("{0}, {1}", pupil[1], pupil[0])));
            deletePupilRecord = pupilSearchTile == null ? new DeletePupilRecordPage() : pupilSearchTile.Click<DeletePupilRecordPage>();
            deletePupilRecord.Delete();



            #endregion
        }


        #region DATA

        public List<object[]> TC_UDG01_DATA()
        {
            string groupFullNameNoAvailable = "Unavailable Name";
            string groupFullName = "Music Club - School Choir";
            string groupShortName = "MCSC";
            string purpose = "Uncategorised";
            bool visibility = true;

            var data = new List<Object[]>
            {
                new object[] {groupFullName, groupShortName, purpose, visibility, groupFullNameNoAvailable }
            };
            return data;
        }

        public List<object[]> TC_UDG02_DATA()
        {
            string unavailableSurname = "Unavailable Name";
            string surName = "Brennan";
            string foreName = "Teagan";
            string role = "Staff";

            var data = new List<Object[]>
            {
                new object[] {surName, foreName, role, unavailableSurname }
            };
            return data;
        }

        public List<object[]> TC_UDG03_DATA()
        {
            string pattern = "M/d/yyyy";
            string surNameCurrent = "Current_Sur" + SeleniumHelper.GenerateRandomString(8);
            string foreNameCurrent = "Current_Fore" + SeleniumHelper.GenerateRandomString(8);
            string surNameFuture = "Future_Sur" + SeleniumHelper.GenerateRandomString(9);
            string foreNameFuture = "Future_Fore" + SeleniumHelper.GenerateRandomString(9);
            string surNameLeaver = "Leaver_Sur" + SeleniumHelper.GenerateRandomString(10);
            string foreNameLeaver = "Leaver_Fore" + SeleniumHelper.GenerateRandomString(10);
            string gender = "Male";
            string dateOfBirth = DateTime.ParseExact("10/16/2006", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            string dateOfAdmissionCurrent = DateTime.ParseExact("10/19/2010", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            string dateOfAdmissionFuture = DateTime.ParseExact(SeleniumHelper.GetDayAfter(SeleniumHelper.GetToDay(), 7), pattern, CultureInfo.InvariantCulture).ToString(pattern);
            string dateOfLeaving = DateTime.ParseExact(SeleniumHelper.GetDayBeforeToday(), pattern, CultureInfo.InvariantCulture).ToString(pattern);
            string reasonForLeaving = "Not Known";
            string pupilName = String.Format("{0}, {1}", surNameCurrent, foreNameCurrent);
            string yearGroup = "Year 5";
            string className = "1A";
            string unavailablePupilName = "Unavailable Name";

            var data = new List<Object[]>
            {
                new object[] { pupilName, yearGroup, className, unavailablePupilName, surNameCurrent, foreNameCurrent, surNameFuture, foreNameFuture, surNameLeaver,
                foreNameLeaver, gender, dateOfBirth, dateOfAdmissionCurrent, dateOfAdmissionFuture, dateOfLeaving, reasonForLeaving }
            };
            return data;
        }

        public List<object[]> TC_UDG04_DATA()
        {
            string unavailableSurname = "Unavailable Name";
            string surName = "Brennan";
            string foreName = "Teagan";
            string role = "Staff";

            var data = new List<Object[]>
            {
                new object[] {surName, foreName, role, unavailableSurname }
            };
            return data;
        }

        public List<object[]> TC_UDG05_Data()
        {
            string fullName = String.Format("{0}_{1}", "Avn", SeleniumHelper.GenerateRandomString(8));
            string shortName = String.Format("{0}{1}", "A", SeleniumHelper.GenerateRandomString(4));
            string note = String.Format("{0}_{1}", "Note", SeleniumHelper.GenerateRandomString(20));
            var res = new List<Object[]>
            {                
                new object[] 
                {
                    fullName, shortName, "Uncategorised", note,
                    "Brennan, Ms Teagan", "Brooks, Mr Colm", "Brown, Mrs Pauline", "Healey, Miss Noreen", "Joyner, Mr Oliver",
                    "Abdullah, Tamwar", "Fortune, Miss Sheila"
                }
            };
            return res;
        }

        public List<object[]> TC_UDG06_Data()
        {
            string fullName = String.Format("{0}_{1}", "YTa", SeleniumHelper.GenerateRandomString(8));
            string shortName = String.Format("{0}{1}", "Y", SeleniumHelper.GenerateRandomString(4));
            string note = String.Format("{0}_{1}", "Note", SeleniumHelper.GenerateRandomString(20));

            string fullNameUpdate = String.Format("{0}_{1}", "Edit", SeleniumHelper.GenerateRandomString(6));
            string shortNameUpdate = String.Format("{0}{1}", "E", SeleniumHelper.GenerateRandomString(4));

            string currentAcademicYear = String.Format("Academic Year {0}/{1}", (DateTime.Now.Year).ToString(), (DateTime.Now.Year + 1).ToString());

            var res = new List<Object[]>
            {                
                new object[] 
                {
                    fullName, shortName, "Uncategorised", note,
                    "Brennan, Ms Teagan",
                    "Abdullah, Tamwar",
                    "Fortune, Miss Sheila",
                    fullNameUpdate,shortNameUpdate,"Edit","Other",
                    currentAcademicYear,
                    //"Brooks, Mr Colm",
                    "Aaron, Elizabeth - 1A",
                    "Aaron, Liz - 1A",

                    "Brown, Mrs Pauline",
                    "Aaron, Liz",
                    "Aaron, Elizabeth - 1A"
                }
            };
            return res;
        }

        public List<object[]> TC_UDG07_Data()
        {
            string fullName = String.Format("{0}_{1}", "Avn", SeleniumHelper.GenerateRandomString(8));
            string shortName = String.Format("{0}{1}", "A", SeleniumHelper.GenerateRandomString(4));
            string note = String.Format("{0}_{1}", "Note", SeleniumHelper.GenerateRandomString(20));
            string futureAcademicYear = String.Format("Academic Year {0}/{1}", (DateTime.Now.Year + 1).ToString(), (DateTime.Now.Year + 2).ToString());
            var theFromDate = new DateTime(DateTime.Today.Year + 1, 9, 1);
            var theToDate = new DateTime(DateTime.Today.Year + 2, 7, 30);
            var res = new List<Object[]>
            {                
                new object[] 
                {
                    fullName, shortName, "Uncategorised", note,
                    //"Brennan, Ms Teagan", 
                    "Ellis, Paul",
                    "Abdullah, Tamwar", "Fortune, Miss Sheila",
                    futureAcademicYear,
                    theFromDate.ToString("M/d/yyyy"),
                    theToDate.ToString("M/d/yyyy")
                }
            };
            return res;
        }

        public List<object[]> TC_UDG008_Data()
        {
            string effectiveDate = String.Format("Academic Year {0}/{1}", (DateTime.Now.Year + 1).ToString(), (DateTime.Now.Year + 2).ToString());

            var res = new List<Object[]>
            {                
                new object[] {
                    new string[]{String.Format("UDG8_{0}", SeleniumHelper.GenerateRandomString(10)), SeleniumHelper.GenerateRandomString(10), "Analysis", "Test amending user undefined group"},
                    new string[]{effectiveDate},
                    new string[]{"Staff", "Brennan, Ms Teagan", "Brooks, Mr Colm", "Supervisor"},
                    new string[]{"Year 1", "1A", "Akeman, Richard"},
                    new string[]{"Brennan, Ms Teagan"},
                    //Update information
                    new string[]{String.Format("Update_UDG8_{0}", SeleniumHelper.GenerateRandomString(10)), String.Format("Update_{0}",SeleniumHelper.GenerateRandomString(3)), "Other", "Test amending user undefined group"},
                    new string[]{effectiveDate},
                    new string[]{"Staff", "Joyner, Mr Oliver", "Activity Leader"},
                    new string[]{"Year 1", "1A", "Benson, Justin"},
                    new string[]{"Healey, Miss Noreen"}
                },
            };
            return res;
        }

        public List<object[]> TC_UDG09_Data()
        {
            string pattern = "M/d/yyyy";
            string fullName = String.Format("{0}_{1}", "Avn", SeleniumHelper.GenerateRandomString(8));
            string shortName = String.Format("{0}{1}", "A", SeleniumHelper.GenerateRandomString(4));
            string note = String.Format("{0}_{1}", "Note", SeleniumHelper.GenerateRandomString(20));
            string startDate = DateTime.Now.ToString(pattern);
            string endDate = DateTime.Now.AddDays(1).ToString(pattern);
            var res = new List<Object[]>
            {                
                new object[] 
                {
                    fullName, shortName, "Uncategorised", note,
                    "Brennan, Ms Teagan", startDate, endDate,
                }
            };
            return res;
        }

        public List<object[]> TC_UDG10_Data()
        {
            string pattern = "M/d/yyyy";
            string fullName = String.Format("{0}_{1}", "Avn", SeleniumHelper.GenerateRandomString(8));
            string shortName = String.Format("{0}{1}", "A", SeleniumHelper.GenerateRandomString(4));
            string note = String.Format("{0}_{1}", "Note", SeleniumHelper.GenerateRandomString(20));
            string startDate = DateTime.Now.AddDays(1).ToString(pattern);
            string endDate = DateTime.Now.ToString(pattern);
            var res = new List<Object[]>
            {                
                new object[] 
                {
                    fullName, shortName, "Uncategorised", note,
                    "Brennan, Ms Teagan", startDate, endDate,
                }
            };
            return res;
        }

        public List<object[]> TC_UDG11_Data()
        {
            string patern = "M/d/yyyy";
            string fullName = String.Format("{0}_{1}", "Logigear", SeleniumHelper.GenerateRandomString(8));
            string shortName = String.Format("{0}{1}", "LGG", SeleniumHelper.GenerateRandomString(4));
            string note = String.Format("{0}_{1}", "Note", SeleniumHelper.GenerateRandomString(20));
            string academicYear = String.Format("Academic Year {0}/{1}", DateTime.Now.Year + 1, DateTime.Now.Year + 2);
            string fromDateBefore = new DateTime(DateTime.Today.Year + 1, 8, 1).ToString(patern);
            string toDateBefore = SeleniumHelper.GetDayAfter(fromDateBefore, 180);
            string fromDateAfter = SeleniumHelper.GetDayAfter(fromDateBefore, 180);
            string toDateAfter = new DateTime(DateTime.Today.Year + 2, 7, 31).ToString(patern);

            var res = new List<Object[]>
            {                
                new object[] 
                {
                    fullName, shortName, "Uncategorised", note, "Staff", academicYear,
                    "Brennan, Ms Teagan", "Activity Leader", "Supervisor", fromDateBefore, toDateBefore, fromDateAfter, toDateAfter
                }
            };
            return res;

        }

        public List<object[]> TC_UDG12_Data()
        {
            string patern = "M/d/yyyy";
            string fullName = String.Format("{0}_{1}", "Logigear", SeleniumHelper.GenerateRandomString(8));
            string shortName = String.Format("{0}{1}", "LGG", SeleniumHelper.GenerateRandomString(4));
            string note = String.Format("{0}_{1}", "Note", SeleniumHelper.GenerateRandomString(20));
            string fromDateBefore = new DateTime(DateTime.Today.Year, 8, 1).ToString(patern);
            string toDateBefore = SeleniumHelper.GetDayAfter(fromDateBefore, 180);
            string fromDateAfter = SeleniumHelper.GetDayAfter(fromDateBefore, 180);
            string toDateAfter = new DateTime(DateTime.Today.Year + 1, 7, 31).ToString(patern);

            var res = new List<Object[]>
            {                
                new object[] 
                {
                    fullName, shortName, "Uncategorised", note, "Staff",
                    "Brennan, Ms Teagan", "Activity Leader", "Supervisor", fromDateBefore, toDateBefore, fromDateAfter, toDateAfter
                }
            };
            return res;

        }

        public List<object[]> TC_UDG13_Data()
        {
            string fullName = String.Format("{0}_{1}", "Avn", SeleniumHelper.GenerateRandomString(8));
            string shortName = String.Format("{0}{1}", "A", SeleniumHelper.GenerateRandomString(4));
            string note = String.Format("{0}_{1}", "Note", SeleniumHelper.GenerateRandomString(20));
            string purpose = "Uncategorised";
            string futureAcademicYear = "Academic Year 2016/2017";
            string supervisorName = "Brennan, Ms Teagan";
            string fromDate = DateTime.ParseExact("12/11/2016", "M/d/yyyy", CultureInfo.InvariantCulture).ToString("M/d/yyyy");
            string toDate = DateTime.ParseExact("12/12/2016", "M/d/yyyy", CultureInfo.InvariantCulture).ToString("M/d/yyyy");
            string pupilName = "Ackton, Stan";
            string memberName = "Brown, Mrs  Pauline";
            var res = new List<Object[]>
            {                
                new object[] 
                {
                    fullName, shortName,purpose, note,futureAcademicYear,supervisorName,fromDate, toDate,pupilName,memberName
                }
            };
            return res;
        }

        public List<object[]> TC_UDG14_Data()
        {
            string pattern = "M/d/yyyy";
            string fullName = String.Format("{0}_{1}", "Avn", SeleniumHelper.GenerateRandomString(8));
            string shortName = String.Format("{0}{1}", "A", SeleniumHelper.GenerateRandomString(4));
            string note = String.Format("{0}_{1}", "Note", SeleniumHelper.GenerateRandomString(20));
            string purpose = "Uncategorised";
            string futureAcademicYear = "Academic Year 2016/2017";
            string supervisor1 = "Brennan, Ms Teagan";
            string supervisor2 = "Brooks, Mr Colm";
            string supervisor3 = "Brown, Mrs Pauline";
            string pupilName = "Ackton, Stan";
            string memberName = "Brown, Mrs  Pauline";

            var fromDate = new DateTime(DateTime.Today.Year + 1, 05, 10).ToString(pattern);
            var toDate = new DateTime(DateTime.Today.Year + 2, 05, 30).ToString(pattern);

            var res = new List<Object[]>
            {                
                new object[] 
                {
                    fullName, shortName, purpose, note,futureAcademicYear,supervisor1,supervisor2,supervisor3,
                    pupilName,memberName,fromDate,toDate
                }
            };
            return res;
        }

        public List<object[]> TC_UDG15_Data()
        {
            string pattern = "M/d/yyyy";
            string fullName = String.Format("{0}_{1}", "Avn", SeleniumHelper.GenerateRandomString(8));
            string shortName = String.Format("{0}{1}", "A", SeleniumHelper.GenerateRandomString(4));
            string note = String.Format("{0}_{1}", "Note", SeleniumHelper.GenerateRandomString(20));
            string endDate1 = DateTime.Now.ToString(pattern);
            string startDate2 = DateTime.Now.Subtract(TimeSpan.FromDays(1)).ToString(pattern);
            string startDate3 = DateTime.Now.AddDays(2).ToString(pattern);
            var res = new List<Object[]>
            {                
                new object[] 
                {
                    fullName, shortName, "Uncategorised", note,
                    "Brennan, Ms Teagan", "Brooks, Mr Colm", "Brown, Mrs Pauline",
                    endDate1, startDate2, startDate3
                }
            };
            return res;
        }

        public List<object[]> TC_UDG26_Data()
        {
            string pattern = "M/d/yyyy";
            string foreName = String.Format("{0}_{1}", SeleniumHelper.GenerateRandomString(7), SeleniumHelper.GenerateRandomNumberUsingDateTime());
            string surName = String.Format("{0}_{1}", "Avn", SeleniumHelper.GenerateRandomString(8));
            string dateOfBirth = DateTime.ParseExact("1/16/1991", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            string dateOfArrival = DateTime.ParseExact("1/1/2010", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            string dateOfLeaving = DateTime.Now.AddDays(2).ToString(pattern);
            string fullName = String.Format("{0}_{1}", "Avn", SeleniumHelper.GenerateRandomString(8));
            string shortName = String.Format("{0}{1}", "A", SeleniumHelper.GenerateRandomString(4));
            string note = String.Format("{0}_{1}", "Note", SeleniumHelper.GenerateRandomString(20));
            string supervisorName = String.Format("{0}, {1}", surName, foreName);

            var res = new List<Object[]>
            {                
                new object[] 
                {
                    foreName, surName, "Male", dateOfBirth, dateOfArrival, dateOfLeaving, "Other Reason",
                    fullName, shortName, "Uncategorised", note, supervisorName
                }
            };
            return res;
        }

        public List<object[]> TC_UDG027_Data()
        {

            string staffName = String.Format("UDG27_{0}", SeleniumHelper.GenerateRandomString(10));
            string ServiceDetailDateArrival = (new DateTime(DateTime.Today.Year - 1, 10, 10)).ToString("M/d/yyyy");
            string dateOfLeaving = DateTime.Now.ToString("M/d/yyyy");
            string effectiveFromDate = DateTime.Now.AddDays(-10).ToString("M/d/yyyy");
            string effectiveToDate = DateTime.Now.AddDays(-5).ToString("M/d/yyyy");
            string currentAcademicYear = String.Format("Academic Year {0}/{1}", DateTime.Now.Year.ToString(), (DateTime.Now.Year + 1).ToString());

            var res = new List<Object[]>
            {                
                new object[] {
                    new string[]{staffName, staffName, "Male"},
                    new string[]{ServiceDetailDateArrival},
                    new string[]{dateOfLeaving, "Retirement"},
                    new string[]{String.Format("UDG27_{0}", SeleniumHelper.GenerateRandomString(10)), SeleniumHelper.GenerateRandomString(10), "Analysis", "Test amending user undefined group"},
                    new string[]{currentAcademicYear, effectiveFromDate, effectiveToDate},
                    new string[]{"Staff", "Supervisor"}
                },
            };
            return res;
        }

        public List<object[]> TC_UDG28_Data()
        {
            string patern = "MM/dd/yyyy";
            string fullName = String.Format("{0}_{1}", "Logigear", SeleniumHelper.GenerateRandomString(8));
            string shortName = String.Format("{0}{1}", "LGG", SeleniumHelper.GenerateRandomString(4));
            string note = String.Format("{0}_{1}", "Note", SeleniumHelper.GenerateRandomString(20));
            string fromDateBefore = new DateTime(DateTime.Today.Year, 1, 8).ToString(patern);
            string toDateBefore = SeleniumHelper.GetDayAfter(fromDateBefore, 180);
            string fromDateAfter = SeleniumHelper.GetDayAfter(fromDateBefore, 180);
            string toDateAfter = new DateTime(DateTime.Today.Year + 1, 7, 31).ToString(patern);

            var res = new List<Object[]>
            {                
                new object[] 
                {
                    fullName, shortName, "Uncategorised", note,
                    "Aaron, Liz", fromDateBefore, toDateBefore, fromDateAfter, toDateAfter,
                    "Aaron, Elizabeth - 1A"
                }
            };
            return res;
        }

        public List<object[]> TC_UDG29_Data()
        {
            string patern = "M/d/yyyy";
            string fullName = String.Format("{0}_{1}", "Logigear", SeleniumHelper.GenerateRandomString(8));
            string shortName = String.Format("{0}{1}", "LGG", SeleniumHelper.GenerateRandomString(4));
            string note = String.Format("{0}_{1}", "Note", SeleniumHelper.GenerateRandomString(20));
            string fromDateBefore = new DateTime(DateTime.Today.Year, 8, 1).ToString(patern);
            string toDateBefore = SeleniumHelper.GetDayAfter(fromDateBefore, 180);
            string fromDateAfter = SeleniumHelper.GetDayAfter(fromDateBefore, 180);
            string toDateAfter = new DateTime(DateTime.Today.Year + 1, 7, 31).ToString(patern);

            var res = new List<Object[]>
            {                
                new object[] 
                {
                    fullName, shortName, "Uncategorised", note, "Staff",
                    "Brennan, Ms Teagan", fromDateBefore, toDateBefore, fromDateAfter, toDateAfter
                }
            };
            return res;
        }

        public List<object[]> TC_UDG30_Data()
        {
            string pattern = "M/d/yyyy";
            string fullName = String.Format("{0}_{1}", "Avn", SeleniumHelper.GenerateRandomString(8));
            string shortName = String.Format("{0}{1}", "A", SeleniumHelper.GenerateRandomString(4));
            string note = String.Format("{0}_{1}", "Note", SeleniumHelper.GenerateRandomString(20));
            string purpose = "Uncategorised";

            // string futureAcademicYear = "Academic Year 2016/2017";
            string supervisorName = "Brennan, Ms Teagan";
            string pupilName = "Ackton, Stan";

            var fromDate1 = new DateTime(DateTime.Today.Year, 10, 10).ToString(pattern);
            var toDate1 = new DateTime(DateTime.Today.Year, 10, 30).ToString(pattern);
            var fromDate2 = new DateTime(DateTime.Today.Year, 11, 11).ToString(pattern);
            var toDate2 = new DateTime(DateTime.Today.Year, 12, 12).ToString(pattern);

            var res = new List<Object[]>
            {                
                new object[] 
                {
                    fullName, shortName,purpose, note,supervisorName,pupilName,fromDate1, toDate1,fromDate2,toDate2
                }
            };
            return res;
        }

        public List<object[]> TC_UDG31_Data()
        {
            string pattern = "M/d/yyyy";
            string fullName = String.Format("{0}_{1}", "Avn", SeleniumHelper.GenerateRandomString(8));
            string shortName = String.Format("{0}{1}", "A", SeleniumHelper.GenerateRandomString(4));
            string note = String.Format("{0}_{1}", "Note", SeleniumHelper.GenerateRandomString(20));
            string purpose = "Uncategorised";
            string supervisorName = "Brennan, Ms Teagan";
            string memberName = "Brown, Mrs Pauline";

            var fromDate1 = new DateTime(DateTime.Today.Year, 10, 10).ToString(pattern);
            var toDate1 = new DateTime(DateTime.Today.Year, 10, 30).ToString(pattern);
            var fromDate2 = new DateTime(DateTime.Today.Year, 11, 11).ToString(pattern);
            var toDate2 = new DateTime(DateTime.Today.Year, 12, 12).ToString(pattern);

            var res = new List<Object[]>
            {                
                new object[] 
                {
                    fullName, shortName,purpose, note,supervisorName,memberName,fromDate1, toDate1,fromDate2,toDate2
                }
            };
            return res;
        }

        public List<object[]> TC_UDG41_Data()
        {
            string pattern = "M/d/yyyy";
            string fullName = String.Format("{0}_{1}", "Logigear", SeleniumHelper.GenerateRandomString(8));
            string shortName = String.Format("{0}{1}", "LGG", SeleniumHelper.GenerateRandomString(4));
            string note = String.Format("{0}_{1}", "Note", SeleniumHelper.GenerateRandomString(20));
            string academicYear = String.Format("Academic Year {0}/{1}", DateTime.Now.Year, DateTime.Now.Year + 1);
            string fromDate = new DateTime(DateTime.Today.Year, 8, 1).ToString(pattern);
            string toDate = new DateTime(DateTime.Today.Year + 1, 7, 31).ToString(pattern);

            // Pupil Data
            string randomString = SeleniumHelper.GenerateRandomString(8);
            string surName = "SUR_" + randomString;
            string foreName = "FORE_" + randomString;
            string gender = "Male";
            string dateOfBirth = DateTime.ParseExact("10/16/2006", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            string DateOfAdmission = DateTime.ParseExact("10/19/2010", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            string yearGroup = "Year 2";
            string dateOfLeaving = SeleniumHelper.GetDayAfter(SeleniumHelper.GetToDay(), 7);

            var res = new List<Object[]>
            {                
                new object[] 
                {
                    // Pupil
                    new string[]{ foreName, surName, gender, dateOfBirth, DateOfAdmission, yearGroup, dateOfLeaving, "Not Known" },
                    // User Defined Group
                    fullName, shortName, "Uncategorised", note, academicYear, fromDate, toDate, "Pupil", "Activity Leader"
                }
            };
            return res;

        }

        public List<object[]> TC_UDG042_Data()
        {
            string pupilName = String.Format("UDG42_{0}_{1}", SeleniumHelper.GenerateRandomString(10), SeleniumHelper.GenerateRandomNumberUsingDateTime(5));
            string dateOfLeaving = DateTime.Now.ToString("M/d/yyyy");
            string effectiveFromDate = DateTime.Now.AddDays(-10).ToString("M/d/yyyy");
            string effectiveToDate = DateTime.Now.AddDays(-5).ToString("M/d/yyyy");
            string currentAcademicYear = String.Format("Academic Year {0}/{1}", DateTime.Now.Year.ToString(), (DateTime.Now.Year + 1).ToString());

            var res = new List<Object[]>
            {                
                new object[] {
                    new string[]{pupilName, pupilName,
                                "Female", DateTime.ParseExact("1/1/2000", "M/d/yyyy", CultureInfo.InvariantCulture).ToString("M/d/yyyy"), 
                                DateTime.ParseExact("10/10/2013", "M/d/yyyy", CultureInfo.InvariantCulture).ToString("M/d/yyyy"), "Guest Pupil", "Year 2", "2A"},
                    new string[]{dateOfLeaving, "Not Known", "Elective Home Education"},
                    new string[]{String.Format("UDG42_{0}", SeleniumHelper.GenerateRandomString(10)), SeleniumHelper.GenerateRandomString(10), "Analysis", "Test add pupil with leaving detail"},
                    new string[]{currentAcademicYear, effectiveFromDate, effectiveToDate},
                },
            };
            return res;
        }

        public List<object[]> TC_UDG43_Data()
        {
            string pattern = "M/d/yyyy";
            string fullName = String.Format("{0}_{1}", "Logigear", SeleniumHelper.GenerateRandomString(8));
            string shortName = String.Format("{0}{1}", "LGG", SeleniumHelper.GenerateRandomString(4));
            string note = String.Format("{0}_{1}", "Note", SeleniumHelper.GenerateRandomString(20));
            string fromDate = new DateTime(DateTime.Today.Year, 8, 1).ToString(pattern);
            string toDate = new DateTime(DateTime.Today.Year + 1, 7, 31).ToString(pattern);

            // Pupil Data
            string randomString = SeleniumHelper.GenerateRandomString(8);
            string surName = "SUR_" + randomString;
            string foreName = "FORE_" + randomString;
            string gender = "Male";
            string dateOfBirth = DateTime.ParseExact("10/16/2006", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            string DateOfAdmission = DateTime.ParseExact("10/19/2010", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            string yearGroup = "Year 2";
            string dateOfLeaving = SeleniumHelper.GetDayAfter(SeleniumHelper.GetToDay(), 7);

            var res = new List<Object[]>
            {                
                new object[] 
                {
                    // Pupil
                    new string[]{ foreName, surName, gender, dateOfBirth, DateOfAdmission, yearGroup, dateOfLeaving, "Not Known" },
                    // User Defined Group
                    fullName, shortName, "Uncategorised", note, fromDate, toDate, "Pupil"
                }
            };
            return res;

        }

        #endregion
    }
}
