using NUnit.Framework;
using POM.Components.SchoolGroups;
using POM.Components.Staff;
using POM.Components.Pupil;
using POM.Helper;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestSettings;
using WebDriverRunner.internals;
using Selene.Support.Attributes;
using SeSugar.Automation;

namespace Faclities.LogigearTests
{
    public class TeachingGroupsTests
    {
        /// <summary>
        /// Author: Y.Ta
        /// Des: Verify that search function return correct result
        /// </summary>        
        [WebDriverTest(TimeoutSeconds = 800, Enabled = true, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { "g1" }, DataProvider = "TC_TG001_Data")]
        public void TC_TG001_Verify_That_Search_Function_Return_Correct_Result(string[] schoolGroup)
        {
            #region Pre-Condition: Create new School Group

            //Log in as a School Administrator
            //SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.TestUserWaterEdgePrimary, true);
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);
            Wait.WaitForDocumentReady();
            // Navigate to Manage Teaching Groups
            AutomationSugar.NavigateMenu("Tasks", "School Groups", "Manage Teaching Groups");
            Wait.WaitForDocumentReady();
            var teachingGroupTriplet = new TeachingGroupTriplet();

            teachingGroupTriplet.SearchCriteria.GroupFullName = schoolGroup[0];
            var listSearchResult = teachingGroupTriplet.SearchCriteria.Search();
            var schoolGroupResult = listSearchResult.FirstOrDefault(t => t.Name.Equals(schoolGroup[0]));
            var teachingGroupDetail = schoolGroupResult == null ? null : schoolGroupResult.Click<TeachingGroupPage>();
            teachingGroupTriplet.Delete();

            teachingGroupDetail = teachingGroupTriplet.ClickCreate();
            teachingGroupDetail.FullName = schoolGroup[0];
            teachingGroupDetail.ShortName = schoolGroup[1];
            teachingGroupDetail.Subject = schoolGroup[2];
            teachingGroupTriplet.ClickSave();

            #endregion

            #region Steps

            //Ensure Search field functions in a way a Business user would expect.
            listSearchResult = teachingGroupTriplet.SearchCriteria.Search();
            Assert.AreNotEqual(null, listSearchResult, "There are no result display when using Search function");

            teachingGroupTriplet.SearchCriteria.GroupFullName = schoolGroup[0];
            teachingGroupTriplet.SearchCriteria.GroupShortName = schoolGroup[1];
            teachingGroupTriplet.SearchCriteria.Subject = schoolGroup[2];

            listSearchResult = teachingGroupTriplet.SearchCriteria.Search();
            schoolGroupResult = listSearchResult.FirstOrDefault(t => t.Name.Equals(schoolGroup[0]));
            teachingGroupDetail = schoolGroupResult == null ? null : schoolGroupResult.Click<TeachingGroupPage>();

            Assert.AreNotEqual(null, schoolGroupResult, "There are no result when searching with Group Name value");
            Assert.AreEqual(schoolGroup[0], teachingGroupDetail.FullName, "The Group Name displays not correctly");
            Assert.AreEqual(schoolGroup[1], teachingGroupDetail.ShortName, "The Short Name display not correctly");
            Assert.AreEqual(schoolGroup[2], teachingGroupDetail.Subject, "The Subject displays not correctly");

            #endregion

            #region End-Condition: Delete school group

            teachingGroupTriplet.SearchCriteria.GroupFullName = schoolGroup[0];
            listSearchResult = teachingGroupTriplet.SearchCriteria.Search();
            schoolGroupResult = listSearchResult.FirstOrDefault(t => t.Name.Equals(schoolGroup[0]));
            teachingGroupDetail = listSearchResult == null ? null : schoolGroupResult.Click<TeachingGroupPage>();
            teachingGroupTriplet.Delete();

            #endregion
        }

        /// <summary>
        /// Author: Y.Ta
        /// Des: Verify that search function of SuperVisor dialog work correctly
        /// Issue: There are no result when searching any SuperVisor.
        /// </summary>        
        [WebDriverTest(TimeoutSeconds = 800, Enabled = false, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { "g1" }, DataProvider = "TC_TG002_Data")]
        public void TC_TG002_Verify_That_Search_SuperVisor_Return_Correct_Result(string[] schoolGroup, string[] staff)
        {

            #region Pre-Condition: Create new School Group

            // Log in as a School Administrator
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);
            // Navigate to Manage Teaching Groups
            SeleniumHelper.NavigateMenu("Tasks", "School Groups", "Manage Teaching Groups");
            var teachingGroupTriplet = new TeachingGroupTriplet();

            teachingGroupTriplet.SearchCriteria.GroupFullName = schoolGroup[0];
            var listSearchResult = teachingGroupTriplet.SearchCriteria.Search();
            var schoolGroupResult = listSearchResult.FirstOrDefault(t => t.Name.Equals(schoolGroup[0]));
            var teachingGroupDetail = schoolGroupResult == null ? null : schoolGroupResult.Click<TeachingGroupPage>();
            teachingGroupTriplet.Delete();

            teachingGroupDetail = teachingGroupTriplet.ClickCreate();
            teachingGroupDetail.FullName = schoolGroup[0];
            teachingGroupDetail.ShortName = schoolGroup[1];
            teachingGroupDetail.Subject = schoolGroup[2];
            teachingGroupTriplet.ClickSave();

            #endregion

            #region Steps
            teachingGroupDetail.Refresh();
            var supervisorTripletDialog = teachingGroupDetail.OpenAddSupervisorDialog();
            supervisorTripletDialog.SearchCriteria.LegalSurName = staff[0];
            supervisorTripletDialog.SearchCriteria.StaffRole = staff[1];
            var searchSupervisorPage = supervisorTripletDialog.SearchCriteria;

            // Issue: There are no result when searching supervisors            
            // Search and Add the Supervisor into School Groups
            teachingGroupDetail.Refresh();
            var addSupervisorsDialogTriplet = new AddSupervisorsDialogTriplet();
            addSupervisorsDialogTriplet.SearchCriteria.LegalSurName = staff[0];
            addSupervisorsDialogTriplet.SearchCriteria.StaffRole = staff[1];

            // Click to add this staff
            var addSupervisorsDialogDetail = addSupervisorsDialogTriplet.SearchCriteria.Search<AddSupervisorsDialogDetail>();

            Assert.AreNotEqual(0, addSupervisorsDialogDetail.SearchResult.Count(), "There are no result when searching supervisor");

            var staffItem = addSupervisorsDialogDetail.SearchResult.FirstOrDefault(p => p.GetText().Contains(staff[0]));
            staffItem.ClickByJS();
            addSupervisorsDialogDetail.AddSelectedSupervisor();
            addSupervisorsDialogTriplet.ClickOk();

            // Verify the staff display
            teachingGroupDetail.SupervisorTable.WaitUntilRowAppear(t => t.Name.Contains(staff[0]));
            teachingGroupDetail.SupervisorTable[0].Role = staff[1];

            teachingGroupTriplet.ClickSave();
            Assert.AreEqual(true, teachingGroupTriplet.DoesMessageSuccessDisplay(), "The success message does not display");
            var supervisorRow = teachingGroupDetail.SupervisorTable.Rows.Any(p => p.Name.Contains(staff[0]));
            Assert.AreEqual(true, supervisorRow, "The row does not display after saving School Group");

            #endregion

            #region End-Condition: Delete school group
            // Remove supervisor row            
            var deleteSupervisorRow = teachingGroupDetail.SupervisorTable.Rows.FirstOrDefault(p => p.Name.Contains(staff[0]));
            deleteSupervisorRow.DeleteRow();

            teachingGroupTriplet.ClickSave();

            teachingGroupTriplet.SearchCriteria.GroupFullName = schoolGroup[0];
            listSearchResult = teachingGroupTriplet.SearchCriteria.Search();
            schoolGroupResult = listSearchResult.FirstOrDefault(t => t.Name.Equals(schoolGroup[0]));
            teachingGroupDetail = listSearchResult == null ? null : schoolGroupResult.Click<TeachingGroupPage>();
            teachingGroupTriplet.Delete();

            #endregion

        }

        /// <summary>
        /// Author: Y.Ta
        /// Des: Verify the search pupils return correct result
        /// Issue: The pupil record does not display when saving page.
        /// </summary>
        [WebDriverTest(TimeoutSeconds = 800, Enabled = false, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { "g1" }, DataProvider = "TC_TG003_Data")]
        public void TC_TG003_Verify_That_Search_Pupils_Return_Correct_Result(string[] schoolGroup, string[] pupil)
        {

            #region Pre-Condition: Create new School Group

            // Log in as a School Administrator
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);
            // Navigate to Manage Teaching Groups
            SeleniumHelper.NavigateMenu("Tasks", "School Groups", "Manage Teaching Groups");
            var teachingGroupTriplet = new TeachingGroupTriplet();

            teachingGroupTriplet.SearchCriteria.GroupFullName = schoolGroup[0];
            var listSearchResult = teachingGroupTriplet.SearchCriteria.Search();
            var schoolGroupResult = listSearchResult.FirstOrDefault(t => t.Name.Equals(schoolGroup[0]));
            var teachingGroupDetail = schoolGroupResult == null ? null : schoolGroupResult.Click<TeachingGroupPage>();
            teachingGroupTriplet.Delete();

            teachingGroupDetail = teachingGroupTriplet.ClickCreate();
            teachingGroupDetail.FullName = schoolGroup[0];
            teachingGroupDetail.ShortName = schoolGroup[1];
            teachingGroupDetail.Subject = schoolGroup[2];
            teachingGroupTriplet.ClickSave();

            #endregion

            #region Steps

            teachingGroupDetail.Refresh();
            var addPupilsDialogTriplet = teachingGroupDetail.OpenAddPupilsDialog();
            // Verify that search function return correctly when clicking search button
            var addPupilDialogDetail = addPupilsDialogTriplet.SearchCriteria.Search<AddPupilsDialogDetail>();
            Assert.AreNotEqual(null, addPupilDialogDetail.Pupils, "The search function of Add Pupils does not work correctly");

            // Verify that search function return correctly when enter value before searching.
            addPupilsDialogTriplet.SearchCriteria.PupilName = pupil[0];
            addPupilsDialogTriplet.SearchCriteria.YearGroup = pupil[1];
            addPupilsDialogTriplet.SearchCriteria.Class = pupil[2];
            addPupilDialogDetail = addPupilsDialogTriplet.SearchCriteria.Search<AddPupilsDialogDetail>();

            addPupilDialogDetail.Refresh();
            Assert.AreEqual(pupil[0], addPupilDialogDetail.Pupils.FirstOrDefault().GetText(), "The search function of Add Pupils does not work correctly");

            // Verify that search function return empty value when searching filter not correctly.
            addPupilsDialogTriplet.SearchCriteria.IsCurrent = false;
            addPupilsDialogTriplet.SearchCriteria.IsLeaver = true;
            addPupilDialogDetail = addPupilsDialogTriplet.SearchCriteria.Search<AddPupilsDialogDetail>();
            addPupilDialogDetail.Refresh();
            Assert.AreEqual(null, addPupilDialogDetail.Pupils, "The search function of Add Pupils does not work correctly");

            addPupilsDialogTriplet.Refresh();
            addPupilsDialogTriplet.ClickCancel();

            #endregion

            #region End-Condition: Delete school group

            teachingGroupTriplet.SearchCriteria.GroupFullName = schoolGroup[0];
            listSearchResult = teachingGroupTriplet.SearchCriteria.Search();
            schoolGroupResult = listSearchResult.FirstOrDefault(t => t.Name.Equals(schoolGroup[0]));
            teachingGroupDetail = listSearchResult == null ? null : schoolGroupResult.Click<TeachingGroupPage>();
            teachingGroupTriplet.Delete();

            #endregion

        }

        /// <summary>
        /// Author: Y.Ta
        /// Des:Create a Teaching Group and allocate Supervisors and Pupils for an effective date range in the current academic Year
        /// </summary>

        [WebDriverTest(TimeoutSeconds = 800, Enabled = false, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { "g1" }, DataProvider = "TC_TG004_Data")]
        public void TC_TG004_Verify_That_Creat_Teaching_Group_And_Allocate_Supervisor_Pupil_Current_Academic_Year(string[] schoolGroup, string[] pupil, string[] staff)
        {

            #region Pre-Condition: Create new School Group

            // Log in as a School Administrator
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);

            // Navigate to Manage Teaching Groups
            SeleniumHelper.NavigateMenu("Tasks", "School Groups", "Manage Teaching Groups");
            var teachingGroupTriplet = new TeachingGroupTriplet();

            teachingGroupTriplet.SearchCriteria.GroupFullName = schoolGroup[0];
            var listSearchResult = teachingGroupTriplet.SearchCriteria.Search();
            var schoolGroupResult = listSearchResult.FirstOrDefault(t => t.Name.Equals(schoolGroup[0]));
            var teachingGroupDetail = schoolGroupResult == null ? null : schoolGroupResult.Click<TeachingGroupPage>();
            teachingGroupTriplet.Delete();

            teachingGroupDetail = teachingGroupTriplet.ClickCreate();
            teachingGroupDetail.FullName = schoolGroup[0];
            teachingGroupDetail.ShortName = schoolGroup[1];
            teachingGroupDetail.Subject = schoolGroup[2];
            teachingGroupTriplet.ClickSave();

            // Effective date range in the  current academic Year
            teachingGroupDetail.Refresh();
            var selectEffectDateRange = teachingGroupDetail.ClickSelectEffectDateRange();
            selectEffectDateRange.AcademicYear = schoolGroup[3];
            selectEffectDateRange.ClickOk();
            teachingGroupTriplet.ClickSave();

            #endregion

            #region Steps

            // Search and Add the pupil into School Groups
            teachingGroupDetail.Refresh();
            var addPupilsDialogTriplet = teachingGroupDetail.OpenAddPupilsDialog();

            // Search specific pupil
            addPupilsDialogTriplet.SearchCriteria.PupilName = pupil[0];
            addPupilsDialogTriplet.SearchCriteria.YearGroup = pupil[1];
            addPupilsDialogTriplet.SearchCriteria.Class = pupil[2];
            var addPupilDialogDetail = addPupilsDialogTriplet.SearchCriteria.Search<AddPupilsDialogDetail>();
            addPupilDialogDetail.Refresh();
            Assert.AreEqual(pupil[0], addPupilDialogDetail.Pupils.FirstOrDefault().GetText(), "The search function of Add Pupils does not work correctly");

            // Click to add this pupil
            var pupilItem = addPupilDialogDetail.Pupils.FirstOrDefault(p => p.GetText().Equals(pupil[0]));
            pupilItem.ClickByJS();
            addPupilDialogDetail.AddSelectedPupils();
            addPupilsDialogTriplet.ClickOk();

            // Verify the pupil display correctly.
            teachingGroupDetail.PupilsTable.WaitUntilRowAppear(p => p.Name.Contains(pupil[0]));
            teachingGroupTriplet.ClickSave();
            Assert.AreEqual(true, teachingGroupTriplet.DoesMessageSuccessDisplay(), "The success message does not display");
            var row = teachingGroupDetail.PupilsTable.Rows.Any(p => p.Name.Equals(String.Format("{0} - {1}", pupil[0], pupil[2])));
            Assert.AreEqual(true, row, "The row does not display after saving School Group");

            // Search and Add the Supervisor into School Groups
            teachingGroupDetail.Refresh();
            var addSupervisorsDialogTriplet = teachingGroupDetail.OpenAddSupervisorDialog();
            addSupervisorsDialogTriplet.SearchCriteria.LegalSurName = staff[0];
            addSupervisorsDialogTriplet.SearchCriteria.StaffRole = staff[1];

            // Click to add this staff
            var addSupervisorsDialogDetail = addSupervisorsDialogTriplet.SearchCriteria.Search<AddSupervisorsDialogDetail>();

            // Issue: There are no result when searching supervisor
            Assert.AreNotEqual(0, addSupervisorsDialogDetail.SearchResult.Count(), "There are no result when searching supervisor");

            var staffItem = addSupervisorsDialogDetail.SearchResult.FirstOrDefault(p => p.GetText().Contains(staff[0]));
            staffItem.ClickByJS();
            addSupervisorsDialogDetail.AddSelectedSupervisor();
            addSupervisorsDialogTriplet.ClickOk();


            // Verify the staff display
            teachingGroupDetail.SupervisorTable.WaitUntilRowAppear(p => p.Name.Contains(staff[0]));
            teachingGroupDetail.SupervisorTable[0].Role = staff[1];

            teachingGroupTriplet.ClickSave();
            Assert.AreEqual(true, teachingGroupTriplet.DoesMessageSuccessDisplay(), "The success message does not display");
            var supervisorRow = teachingGroupDetail.SupervisorTable.Rows.Any(p => p.Name.Contains(staff[0]));
            Assert.AreEqual(true, supervisorRow, "The row does not display after saving School Group");


            #endregion

            #region End-Condition: Delete school group
            // Remove pupil row
            var deletePupilRow = teachingGroupDetail.PupilsTable.Rows.FirstOrDefault(p => p.Name.Equals(String.Format("{0} - {1}", pupil[0], pupil[2])));
            deletePupilRow.DeleteRow();

            // Remove supervisor row            
            var deleteSupervisorRow = teachingGroupDetail.SupervisorTable.Rows.FirstOrDefault(p => p.Name.Contains(staff[0]));
            deleteSupervisorRow.DeleteRow();

            teachingGroupTriplet.ClickSave();
            teachingGroupTriplet.SearchCriteria.GroupFullName = schoolGroup[0];
            listSearchResult = teachingGroupTriplet.SearchCriteria.Search();
            schoolGroupResult = listSearchResult.FirstOrDefault(t => t.Name.Equals(schoolGroup[0]));
            teachingGroupDetail = listSearchResult == null ? null : schoolGroupResult.Click<TeachingGroupPage>();
            teachingGroupTriplet.Delete();

            #endregion

        }

        /// <summary>
        /// Author: Y.Ta
        /// Des : Amend a Teaching Group, and the allocation of Supervisors and Pupils for an effective date range in the  current academic Year
        /// </summary>

        [WebDriverTest(TimeoutSeconds = 800, Enabled = false, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { "g1" }, DataProvider = "TC_TG005_Data")]
        public void TC_TG005_Verify_That_Amend_Teaching_Group_And_Allocate_Supervisor_Pupil_Current_Academic_Year(string[] schoolGroup, string[] pupil, string[] staff, string[] schoolGroupUpdate, string[] pupilUpdate, string[] staffUpdate)
        {

            #region Pre-Condition: Create new School Group for currrent academic year

            // Log in as a School Administrator
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);

            // Navigate to Manage Teaching Groups
            SeleniumHelper.NavigateMenu("Tasks", "School Groups", "Manage Teaching Groups");
            var teachingGroupTriplet = new TeachingGroupTriplet();

            teachingGroupTriplet.SearchCriteria.GroupFullName = schoolGroup[0];
            var listSearchResult = teachingGroupTriplet.SearchCriteria.Search();
            var schoolGroupResult = listSearchResult.FirstOrDefault(t => t.Name.Equals(schoolGroup[0]));
            var teachingGroupDetail = schoolGroupResult == null ? null : schoolGroupResult.Click<TeachingGroupPage>();
            teachingGroupTriplet.Delete();

            teachingGroupDetail = teachingGroupTriplet.ClickCreate();
            teachingGroupDetail.FullName = schoolGroup[0];
            teachingGroupDetail.ShortName = schoolGroup[1];
            teachingGroupDetail.Subject = schoolGroup[2];
            teachingGroupTriplet.ClickSave();

            // Effective date range in the  current academic Year
            teachingGroupDetail.Refresh();
            var selectEffectDateRange = teachingGroupDetail.ClickSelectEffectDateRange();
            selectEffectDateRange.AcademicYear = schoolGroup[3];
            selectEffectDateRange.ClickOk();
            teachingGroupTriplet.ClickSave();

            // Search and Add the pupil into School Groups
            teachingGroupDetail.Refresh();
            var addPupilsDialogTriplet = teachingGroupDetail.OpenAddPupilsDialog();

            // Search specific pupil
            addPupilsDialogTriplet.SearchCriteria.PupilName = pupil[0];
            addPupilsDialogTriplet.SearchCriteria.YearGroup = pupil[1];
            addPupilsDialogTriplet.SearchCriteria.Class = pupil[2];
            var addPupilDialogDetail = addPupilsDialogTriplet.SearchCriteria.Search<AddPupilsDialogDetail>();
            addPupilDialogDetail.Refresh();

            // Click to add this pupil
            var pupilItem = addPupilDialogDetail.Pupils.FirstOrDefault(p => p.GetText().Equals(pupil[0]));
            pupilItem.ClickByJS();
            addPupilDialogDetail.AddSelectedPupils();
            addPupilsDialogTriplet.ClickOk();

            teachingGroupDetail.PupilsTable.WaitUntilRowAppear(p => p.Name.Contains(pupil[0]));
            teachingGroupTriplet.ClickSave();

            // Issue: Cannot search to add any Supervisor
            // Search and Add the Supervisor into School Groups
            teachingGroupDetail.Refresh();
            var addSupervisorsDialogTriplet = teachingGroupDetail.OpenAddSupervisorDialog();
            addSupervisorsDialogTriplet.SearchCriteria.LegalSurName = staff[0];
            addSupervisorsDialogTriplet.SearchCriteria.StaffRole = staff[1];

            // Click to add this staff
            var addSupervisorsDialogDetail = addSupervisorsDialogTriplet.SearchCriteria.Search<AddSupervisorsDialogDetail>();

            Assert.AreNotEqual(0, addSupervisorsDialogDetail.SearchResult.Count(), "There are no result when searching supervisor");

            var staffItem = addSupervisorsDialogDetail.SearchResult.FirstOrDefault(p => p.GetText().Contains(staff[0]));
            staffItem.ClickByJS();
            addSupervisorsDialogDetail.AddSelectedSupervisor();

            addSupervisorsDialogTriplet.Refresh();
            addSupervisorsDialogTriplet.ClickOk();

            teachingGroupDetail.SupervisorTable.WaitUntilRowAppear(p => p.Name.Contains(staff[0]));
            teachingGroupDetail.SupervisorTable[0].Role = staff[1];

            teachingGroupTriplet.ClickSave();
            #endregion

            #region Steps

            #region Edit Fields
            // Search the specific School Group
            teachingGroupTriplet.Refresh();
            teachingGroupTriplet.SearchCriteria.GroupFullName = schoolGroup[0];
            listSearchResult = teachingGroupTriplet.SearchCriteria.Search();
            schoolGroupResult = listSearchResult.FirstOrDefault(t => t.Name.Equals(schoolGroup[0]));
            teachingGroupDetail = schoolGroupResult == null ? null : schoolGroupResult.Click<TeachingGroupPage>();

            //Amend the Title in  field 'Full Name'
            teachingGroupDetail.FullName = schoolGroupUpdate[0];

            //Select a different value from Drop Down List box 'Visibility'
            teachingGroupDetail.Visibility = true;

            //Amend text into field 'Notes'
            teachingGroupDetail.Notes = schoolGroupUpdate[1];

            //Amend the short description in field 'Short Name'
            teachingGroupDetail.ShortName = schoolGroupUpdate[2];

            //Select a different value from Drop Down List Box 'Subject'
            teachingGroupDetail.Subject = schoolGroupUpdate[3];

            // 'Select Effective Date Range' and change the  current academic year
            teachingGroupTriplet.ClickSave();

            teachingGroupDetail.Refresh();
            selectEffectDateRange = teachingGroupDetail.ClickSelectEffectDateRange();
            selectEffectDateRange.AcademicYear = schoolGroupUpdate[4];
            selectEffectDateRange.ClickOk();
            teachingGroupTriplet.ClickSave();

            // Verify Amend school group information successfully
            teachingGroupDetail.Refresh();
            Assert.AreEqual(schoolGroupUpdate[0], teachingGroupDetail.FullName, "The FullName off teaching group displays incorrectly");
            Assert.AreEqual(schoolGroupUpdate[1], teachingGroupDetail.Notes, "The Note of teaching group display incorrectly");
            Assert.AreEqual(schoolGroupUpdate[2], teachingGroupDetail.ShortName, "The Short Name of teaching group display incorrectly");
            Assert.AreEqual(schoolGroupUpdate[3], teachingGroupDetail.Subject, "The Subject of teaching group display incorrectly");
            Assert.AreEqual(true, teachingGroupDetail.Visibility, "The Visibility of teaching group display incorrectly");

            #endregion

            #region Edit Leanrner
            //Scroll to Learners
            //Select a Pupil record a nd then Click Icon 'Delete this Row?'            
            // Issue : The row of taboe Pupil is disappear after saving school group
            var row = teachingGroupDetail.PupilsTable.Rows.Any(p => p.Name.Equals(String.Format("{0} - {1}", pupil[0], pupil[2])));
            Assert.AreEqual(true, row, "The row does not display after saving School Group");

            var currentPupilRow = teachingGroupDetail.PupilsTable.Rows.FirstOrDefault(p => p.Name.Equals(String.Format("{0} - {1}", pupil[0], pupil[2])));

            currentPupilRow.DeleteRow();

            // Search and Add the pupil into School Groups
            teachingGroupDetail.Refresh();
            addPupilsDialogTriplet = teachingGroupDetail.OpenAddPupilsDialog();

            // Search specific pupil
            addPupilsDialogTriplet.SearchCriteria.PupilName = pupilUpdate[0];
            addPupilsDialogTriplet.SearchCriteria.YearGroup = pupilUpdate[1];
            addPupilsDialogTriplet.SearchCriteria.Class = pupilUpdate[2];
            addPupilDialogDetail = addPupilsDialogTriplet.SearchCriteria.Search<AddPupilsDialogDetail>();
            addPupilDialogDetail.Refresh();

            // Click to add this pupil
            pupilItem = addPupilDialogDetail.Pupils.FirstOrDefault(p => p.GetText().Equals(pupilUpdate[0]));
            pupilItem.ClickByJS();
            addPupilDialogDetail.AddSelectedPupils();
            addPupilsDialogTriplet.ClickOk();

            teachingGroupDetail.PupilsTable.WaitUntilRowAppear(p => p.Name.Contains(pupilUpdate[0]));
            teachingGroupTriplet.ClickSave();

            //Check the Pupils selected corresponds exactly with the values displayed in table Learners 
            Assert.AreEqual(true, teachingGroupTriplet.DoesMessageSuccessDisplay(), "The success message does not display");
            var rowCheck = teachingGroupDetail.PupilsTable.Rows.Any(p => p.Name.Equals(String.Format("{0} - {1}", pupilUpdate[0], pupilUpdate[2])));
            Assert.AreEqual(true, rowCheck, "The row does not display after saving School Group");

            #endregion

            #region Edit Supervisor
            // Scroll to Supervisors
            // Remove current supervisor
            // Issue: The function search does not return any supervisors.
            var currentSupervisorRow = teachingGroupDetail.SupervisorTable.Rows.FirstOrDefault(p => p.Name.Contains(staff[0]));
            currentSupervisorRow.DeleteRow();


            // Search and Add the Supervisor into School Groups
            teachingGroupDetail.Refresh();
            addSupervisorsDialogTriplet = teachingGroupDetail.OpenAddSupervisorDialog();
            addSupervisorsDialogTriplet.SearchCriteria.LegalSurName = staffUpdate[0];
            addSupervisorsDialogTriplet.SearchCriteria.StaffRole = staffUpdate[1];

            // Click to add this staff
            addSupervisorsDialogDetail = addSupervisorsDialogTriplet.SearchCriteria.Search<AddSupervisorsDialogDetail>();

            Assert.AreNotEqual(0, addSupervisorsDialogDetail.SearchResult.Count(), "There are no result when searching supervisor");

            staffItem = addSupervisorsDialogDetail.SearchResult.FirstOrDefault(p => p.GetText().Contains(staffUpdate[0]));
            staffItem.ClickByJS();
            addSupervisorsDialogDetail.AddSelectedSupervisor();

            addSupervisorsDialogTriplet.Refresh();
            addSupervisorsDialogTriplet.ClickOk();

            teachingGroupDetail.SupervisorTable.WaitUntilRowAppear(p => p.Name.Contains(staffUpdate[0]));
            var rowUpdate = teachingGroupDetail.SupervisorTable.Rows.FirstOrDefault(p => p.Name.Contains(staffUpdate[0]));
            rowUpdate.Role = staffUpdate[1];
            // Verify the staff display

            teachingGroupTriplet.ClickSave();
            Assert.AreEqual(true, teachingGroupTriplet.DoesMessageSuccessDisplay(), "The success message does not display");
            var supervisorRow = teachingGroupDetail.SupervisorTable.Rows.Any(p => p.Name.Contains(staffUpdate[0]));
            Assert.AreEqual(true, supervisorRow, "The row does not display after saving School Group");

            #endregion

            #endregion

            #region End-Condition: Delete school group

            // Remove pupil row
            var deletePupilRow = teachingGroupDetail.PupilsTable.Rows.FirstOrDefault(p => p.Name.Equals(String.Format("{0} - {1}", pupilUpdate[0], pupilUpdate[2])));
            deletePupilRow.DeleteRow();

            // Remove supervisor row            
            var deleteSupervisorRow = teachingGroupDetail.SupervisorTable.Rows.FirstOrDefault(p => p.Name.Contains(staffUpdate[0]));
            deleteSupervisorRow.DeleteRow();
            teachingGroupTriplet.ClickSave();

            teachingGroupTriplet.SearchCriteria.GroupFullName = schoolGroup[0];
            listSearchResult = teachingGroupTriplet.SearchCriteria.Search();
            schoolGroupResult = listSearchResult.FirstOrDefault(t => t.Name.Equals(schoolGroup[0]));
            teachingGroupDetail = listSearchResult == null ? null : schoolGroupResult.Click<TeachingGroupPage>();
            teachingGroupTriplet.Delete();

            #endregion

        }

        /// <summary>
        /// Author: Y.Ta
        /// Create a Teaching Group and allocate Supervisors and Pupils for an effective date range in a Future academic Year
        /// </summary>        
        [WebDriverTest(TimeoutSeconds = 800, Enabled = false, Browsers = new[] { BrowserDefaults.Ie }, Groups = new[] { "g1" }, DataProvider = "TC_TG006_Data")]
        public void TC_TG006_Verify_That_Creat_Teaching_Group_And_Allocate_Supervisor_Pupil_Future_Academic_Year(string[] schoolGroup, string[] pupil, string[] staff)
        {
            #region Pre-Condition: Create new School Group for future academic year

            // Log in as a School Administrator
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);
            // Navigate to Manage Teaching Groups
            SeleniumHelper.NavigateMenu("Tasks", "School Groups", "Manage Teaching Groups");
            var teachingGroupTriplet = new TeachingGroupTriplet();

            teachingGroupTriplet.SearchCriteria.GroupFullName = schoolGroup[0];
            var listSearchResult = teachingGroupTriplet.SearchCriteria.Search();
            var schoolGroupResult = listSearchResult.FirstOrDefault(t => t.Name.Equals(schoolGroup[0]));
            var teachingGroupDetail = schoolGroupResult == null ? null : schoolGroupResult.Click<TeachingGroupPage>();
            teachingGroupTriplet.Delete();

            teachingGroupDetail = teachingGroupTriplet.ClickCreate();

            teachingGroupDetail.FullName = schoolGroup[0];
            teachingGroupDetail.ShortName = schoolGroup[1];
            teachingGroupDetail.Subject = schoolGroup[2];
            teachingGroupTriplet.ClickSave();

            // Effective date range in the future academic Year
            teachingGroupDetail.Refresh();
            var selectEffectDateRange = teachingGroupDetail.ClickSelectEffectDateRange();
            selectEffectDateRange.AcademicYear = schoolGroup[3];
            selectEffectDateRange.ClickOk();

            #endregion

            #region Steps

            // Search and Add the pupil into School Groups
            teachingGroupDetail.Refresh();
            var addPupilsDialogTriplet = teachingGroupDetail.OpenAddPupilsDialog();

            // Search specific pupil
            addPupilsDialogTriplet.SearchCriteria.PupilName = pupil[0];
            addPupilsDialogTriplet.SearchCriteria.YearGroup = pupil[1];
            addPupilsDialogTriplet.SearchCriteria.Class = pupil[2];
            var addPupilDialogDetail = addPupilsDialogTriplet.SearchCriteria.Search<AddPupilsDialogDetail>();
            addPupilDialogDetail.Refresh();
            Assert.AreEqual(pupil[0], addPupilDialogDetail.Pupils.FirstOrDefault().GetText(), "The search function of Add Pupils does not work correctly");

            // Click to add this pupil
            var pupilItem = addPupilDialogDetail.Pupils.FirstOrDefault(p => p.GetText().Equals(pupil[0]));
            pupilItem.ClickByJS();
            addPupilDialogDetail.AddSelectedPupils();
            addPupilsDialogTriplet.ClickOk();

            // Verify the pupil display correctly.
            teachingGroupDetail.PupilsTable.WaitUntilRowAppear(p => p.Name.Contains(pupil[0]));
            teachingGroupTriplet.ClickSave();
            Assert.AreEqual(true, teachingGroupTriplet.DoesMessageSuccessDisplay(), "The success message does not display");
            var row = teachingGroupDetail.PupilsTable.Rows.Any(p => p.Name.Equals(String.Format("{0} - {1}", pupil[0], pupil[2])));
            Assert.AreEqual(true, row, "The row does not display after saving School Group");

            teachingGroupDetail.Refresh();
            Assert.AreEqual(schoolGroup[0], teachingGroupDetail.FullName, "The FullName off teaching group displays incorrectly");
            Assert.AreEqual(schoolGroup[1], teachingGroupDetail.ShortName, "The Short Name of teaching group display incorrectly");
            Assert.AreEqual(schoolGroup[2], teachingGroupDetail.Subject, "The Subject of teaching group display incorrectly");
            //Assert.AreEqual(schoolGroup[4], teachingGroupDetail.EffectiveDate, "The effect date displays not correctly");


            // Issue: The function search does not return any supervisors.
            // Search and Add the Supervisor into School Groups
            teachingGroupDetail.Refresh();
            var addSupervisorsDialogTriplet = teachingGroupDetail.OpenAddSupervisorDialog();
            addSupervisorsDialogTriplet.SearchCriteria.LegalSurName = staff[0];
            addSupervisorsDialogTriplet.SearchCriteria.StaffRole = staff[1];

            // Click to add this staff
            var addSupervisorsDialogDetail = addSupervisorsDialogTriplet.SearchCriteria.Search<AddSupervisorsDialogDetail>();

            Assert.AreNotEqual(0, addSupervisorsDialogDetail.SearchResult.Count(), "There are no result when searching supervisor");

            var staffItem = addSupervisorsDialogDetail.SearchResult.FirstOrDefault(p => p.GetText().Contains(staff[0]));
            staffItem.ClickByJS();
            addSupervisorsDialogDetail.AddSelectedSupervisor();
            addSupervisorsDialogTriplet.ClickOk();

            // Verify the staff display
            teachingGroupDetail.SupervisorTable.WaitUntilRowAppear(p => p.Name.Contains(staff[0]));
            teachingGroupDetail.SupervisorTable[0].Role = staff[1];
            teachingGroupTriplet.ClickSave();

            Assert.AreEqual(true, teachingGroupTriplet.DoesMessageSuccessDisplay(), "The success message does not display");
            var supervisorRow = teachingGroupDetail.SupervisorTable.Rows.Any(p => p.Name.Contains(staff[0]));
            Assert.AreEqual(true, supervisorRow, "The row does not display after saving School Group");

            #endregion

            #region End-Condition: Delete school group

            // Remove pupil row
            var deletePupilRow = teachingGroupDetail.PupilsTable.Rows.FirstOrDefault(p => p.Name.Equals(String.Format("{0} - {1}", pupil[0], pupil[2])));
            deletePupilRow.DeleteRow();

            // Remove supervisor row            
            var deleteSupervisorRow = teachingGroupDetail.SupervisorTable.Rows.FirstOrDefault(p => p.Name.Contains(staff[0]));
            deleteSupervisorRow.DeleteRow();

            teachingGroupTriplet.ClickSave();

            teachingGroupTriplet.SearchCriteria.GroupFullName = schoolGroup[0];
            listSearchResult = teachingGroupTriplet.SearchCriteria.Search();
            schoolGroupResult = listSearchResult.FirstOrDefault(t => t.Name.Equals(schoolGroup[0]));
            teachingGroupDetail = listSearchResult == null ? null : schoolGroupResult.Click<TeachingGroupPage>();
            teachingGroupTriplet.Delete();

            #endregion

        }

        /// <summary>
        /// TC TG-07
        /// Au : An Nguyen
        /// Description: Amend a Teaching Group, and the allocation of Supervisors and Pupils for an effective date range in a Future academic Year.
        /// Role: School Administrator
        /// Issue : Pupil disappear after save (IE)
        /// </summary>
        [WebDriverTest(TimeoutSeconds = 1000, Enabled = false, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { "g1" }, DataProvider = "TC_TG007_Data")]
        public void TC_TG007_Amend_Teaching_Group_and_the_allocation_of_Supervisors_Pupils_in_a_Future_Academic_Year(string[] supervisor1, string[] supervisor2, string fullName, string shortName, string subject, string note, string staffRole, string supervisorName, string pupilName,
                    string editFullName, string editShortName, string editSubject, string editNote, string nextAcademic, string newSupervisorName, string newPupilName)
        {

            #region Pre-Condition : Add new staff

            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.PersonnelOfficer);

            //Delete exist record            
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

            //Delete the first staff if exist
            staffRecord.DeleteStaff();
            staffRecord = staffRecord.ContinueDeleteStaff();

            //Search exists staff
            deleteStaffRecords.Refresh();
            deleteStaffRecords.SearchCriteria.StaffName = newSupervisorName;
            staffSearchResults = deleteStaffRecords.SearchCriteria.Search();
            staffSearchTile = staffSearchResults.SingleOrDefault(t => t.Name.Equals(newSupervisorName));
            staffRecord = staffSearchTile == null ? StaffRecordPage.Create() : staffSearchTile.Click<StaffRecordPage>();

            //Delete the second staff if exist
            staffRecord.DeleteStaff();
            staffRecord = staffRecord.ContinueDeleteStaff();

            //Create new staff
            SeleniumHelper.NavigateMenu("Tasks", "Staff", "Staff Records");
            var staffRecords = new StaffRecordTriplet();
            var addNewStaffDialog = staffRecords.CreateStaff();

            //Fill Add New Staff Dialog                           
            addNewStaffDialog.Forename = supervisor1[0];
            addNewStaffDialog.SurName = supervisor1[1];
            addNewStaffDialog.Gender = supervisor1[2];
            addNewStaffDialog.DateOfBirth = supervisor1[3];

            //Fill Service Detail Dialog
            var serviceDetailDialog = addNewStaffDialog.Continue();
            serviceDetailDialog.DateOfArrival = supervisor1[4];
            serviceDetailDialog.CreateRecord();

            //Add staff role
            staffRecord = StaffRecordPage.Create();
            staffRecord.SelectServiceDetailsTab();
            var staffRoleRow = staffRecord.StaffRoleStandardTable.Rows.Last();
            staffRoleRow.StaffRole = staffRole;
            staffRoleRow = staffRecord.StaffRoleStandardTable.LastInsertRow;
            staffRoleRow.StaffStartDate = "01/01/2015";

            //Save staff
            staffRecord.SaveStaff();

            //Create the second staff
            staffRecords.Refresh();
            addNewStaffDialog = staffRecords.CreateStaff();

            //Fill Add New Staff Dialog                           
            addNewStaffDialog.Forename = supervisor2[0];
            addNewStaffDialog.SurName = supervisor2[1];
            addNewStaffDialog.Gender = supervisor2[2];
            addNewStaffDialog.DateOfBirth = supervisor2[3];

            //Fill Service Detail Dialog
            serviceDetailDialog = addNewStaffDialog.Continue();
            serviceDetailDialog.DateOfArrival = supervisor2[4];
            serviceDetailDialog.CreateRecord();

            //Add staff role
            staffRecord = StaffRecordPage.Create();
            staffRecord.SelectServiceDetailsTab();
            staffRoleRow = staffRecord.StaffRoleStandardTable.Rows.Last();
            staffRoleRow.StaffRole = staffRole;
            staffRoleRow = staffRecord.StaffRoleStandardTable.LastInsertRow;
            staffRoleRow.StaffStartDate = "01/01/2015";

            //Save staff
            staffRecord.SaveStaff();
            SeleniumHelper.Logout();

            #endregion

            #region Pre-Condition : Add new teaching group

            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);

            //Navigate to Manage Teaching Groups
            SeleniumHelper.NavigateMenu("Tasks", "School Groups", "Manage Teaching Groups");
            var teachingGroupTriplet = new TeachingGroupTriplet();

            //Delete old teaching group if exist
            teachingGroupTriplet.SearchCriteria.GroupFullName = fullName;
            var listSearchResult = teachingGroupTriplet.SearchCriteria.Search();
            var schoolGroupResult = listSearchResult.FirstOrDefault(t => t.Name.Equals(fullName));
            var teachingGroupDetail = schoolGroupResult == null ? null : schoolGroupResult.Click<TeachingGroupPage>();
            teachingGroupTriplet.Delete();

            //Create new teaching group
            teachingGroupDetail = teachingGroupTriplet.ClickCreate();

            //Add group details
            teachingGroupDetail.FullName = fullName;
            teachingGroupDetail.ShortName = shortName;
            teachingGroupDetail.Subject = subject;
            teachingGroupDetail.Visibility = true;
            teachingGroupDetail.Notes = note;

            //Add supervisor
            var addSupervisors = teachingGroupDetail.OpenAddSupervisorDialog();
            addSupervisors.SearchCriteria.StaffRole = staffRole;
            var addSupervisor = addSupervisors.SearchCriteria.Search<AddSupervisorsDialogDetail>();
            var supervisors = addSupervisor.SearchResult;
            var supervisor = supervisors.FirstOrDefault(t => t.Text.Contains(supervisorName));
            supervisor.ClickByJS();
            addSupervisor.AddSelectedSupervisor();
            addSupervisors.ClickOk();
            var supervisorTable = teachingGroupDetail.SupervisorTable;
            supervisorTable.WaitUntilRowAppear(t => t.Name.Contains(supervisorName));
            var supervisorRow = supervisorTable.Rows.FirstOrDefault(t => t.Name.Contains(supervisorName));
            supervisorRow.Role = staffRole;

            //Add pupils
            var addPupils = teachingGroupDetail.OpenAddPupilsDialog();
            addPupils.SearchCriteria.PupilName = pupilName;
            var addPupil = addPupils.SearchCriteria.Search<AddPupilsDialogDetail>();
            var pupil = addPupil.Pupils.FirstOrDefault(t => t.GetText().Equals(pupilName));
            pupil.ClickByJS();
            addPupil.AddSelectedPupils();
            addPupils.ClickOk();
            var pupilTable = teachingGroupDetail.PupilsTable;
            pupilTable.WaitUntilRowAppear(t => t.Name.Contains(pupilName));
            teachingGroupTriplet.ClickSave();

            #endregion

            #region Test steps

            //Search teaching group
            teachingGroupTriplet.Refresh();
            teachingGroupTriplet.SearchCriteria.GroupFullName = fullName;
            var teachingGroupSearchResult = teachingGroupTriplet.SearchCriteria.Search();
            var teachingGroupTile = teachingGroupSearchResult.FirstOrDefault(t => t.Name.Equals(fullName));
            teachingGroupDetail = teachingGroupTile.Click<TeachingGroupPage>();

            //Edit group details
            teachingGroupDetail.FullName = editFullName;
            teachingGroupDetail.ShortName = editShortName;
            teachingGroupDetail.Subject = editSubject;
            teachingGroupDetail.Visibility = false;
            teachingGroupDetail.Notes = editNote;

            //Delete old supervisor
            supervisorTable = teachingGroupDetail.SupervisorTable;
            supervisorRow = supervisorTable.Rows.FirstOrDefault(t => t.Name.Contains(supervisorName));
            Assert.AreNotEqual(null, supervisorRow, "Supervisor does not display on the grid");
            supervisorRow.DeleteRow();

            //Delete old pupil
            pupilTable = teachingGroupDetail.PupilsTable;
            var pupilRow = pupilTable.Rows.FirstOrDefault(t => t.Name.Contains(pupilName));
            Assert.AreNotEqual(null, pupilRow, "Pupil does not display on the grid");
            pupilRow.DeleteRow();
            teachingGroupTriplet.ClickSave();

            //Change date range to next academic year
            teachingGroupDetail.Refresh();
            var selectDateRange = teachingGroupDetail.ClickSelectEffectDateRange();
            selectDateRange.AcademicYear = nextAcademic;
            selectDateRange.ClickOk(5);
            teachingGroupDetail.Refresh();

            //Add supervisor
            addSupervisors = teachingGroupDetail.OpenAddSupervisorDialog();
            addSupervisors.SearchCriteria.StaffRole = staffRole;
            addSupervisor = addSupervisors.SearchCriteria.Search<AddSupervisorsDialogDetail>();
            supervisors = addSupervisor.SearchResult;
            supervisor = supervisors.FirstOrDefault(t => t.Text.Contains(newSupervisorName));
            supervisor.ClickByJS();
            addSupervisor.AddSelectedSupervisor();
            addSupervisors.ClickOk();
            supervisorTable = teachingGroupDetail.SupervisorTable;
            supervisorTable.WaitUntilRowAppear(t => t.Name.Contains(newSupervisorName));
            supervisorRow = supervisorTable.Rows.FirstOrDefault(t => t.Name.Contains(newSupervisorName));
            supervisorRow.Role = staffRole;

            //Add new pupil
            addPupils = teachingGroupDetail.OpenAddPupilsDialog();
            addPupils.SearchCriteria.PupilName = newPupilName;
            addPupil = addPupils.SearchCriteria.Search<AddPupilsDialogDetail>();
            pupil = addPupil.Pupils.FirstOrDefault(t => t.GetText().Equals(newPupilName));
            pupil.ClickByJS();
            addPupil.AddSelectedPupils();
            addPupils.ClickOk();
            teachingGroupDetail.PupilsTable.WaitUntilRowAppear(t => t.Name.Contains(newPupilName));

            //Save
            teachingGroupTriplet.ClickSave();
            teachingGroupTriplet.Refresh();

            //Search teaching group with edited name
            teachingGroupTriplet.SearchCriteria.GroupFullName = editFullName;
            teachingGroupTriplet.SearchCriteria.Visibility = false;
            teachingGroupSearchResult = teachingGroupTriplet.SearchCriteria.Search();
            teachingGroupTile = teachingGroupSearchResult.FirstOrDefault(t => t.Name.Equals(editFullName));
            Assert.AreNotEqual(null, teachingGroupTile, "Cannot edit full name of teaching group");

            //Verify group details data
            teachingGroupDetail = teachingGroupTile.Click<TeachingGroupPage>();
            Assert.AreEqual(editFullName, teachingGroupDetail.FullName, "Cannot edit full name of teaching group");
            Assert.AreEqual(editShortName, teachingGroupDetail.ShortName, "Cannot edit short name of teaching group");
            Assert.AreEqual(editSubject, teachingGroupDetail.Subject, "Cannot edit subject of teaching group");
            Assert.AreEqual(false, teachingGroupDetail.Visibility, "Cannot edit visibility of teaching group");
            Assert.AreEqual(editNote, teachingGroupDetail.Notes, "Cannot edit notes of teaching group");

            //Verify supervisor of current academic year
            supervisorTable = teachingGroupDetail.SupervisorTable;
            supervisorRow = supervisorTable.Rows.FirstOrDefault(t => t.Name.Contains(supervisorName));
            Assert.AreEqual(null, supervisorRow, "Cannot delete supervisor of current academic year");

            //Verify pupil of current academic year
            pupilTable = teachingGroupDetail.PupilsTable;
            pupilRow = pupilTable.Rows.FirstOrDefault(t => t.Name.Contains(pupilName));
            Assert.AreEqual(null, pupilRow, "Cannot delete old pupil of current academic year");

            //Verify next academic year
            selectDateRange = teachingGroupDetail.ClickSelectEffectDateRange();
            selectDateRange.AcademicYear = nextAcademic;
            selectDateRange.ClickOk(5);
            teachingGroupDetail.Refresh();

            //Verify supervisor of future academic year
            supervisorTable = teachingGroupDetail.SupervisorTable;
            supervisorRow = supervisorTable.Rows.FirstOrDefault(t => t.Name.Contains(newSupervisorName));
            Assert.AreNotEqual(null, supervisorRow, "Cannot add supervisor to next academic year");

            //Verify pupil of future academic year
            pupilTable = teachingGroupDetail.PupilsTable;
            pupilRow = pupilTable.Rows.FirstOrDefault(t => t.Name.Contains(newPupilName));
            Assert.AreNotEqual(null, pupilRow, "Cannot add pupil to next academic year");

            #endregion

            #region Post-Condition : Delete Teaching Group

            pupilRow.DeleteRow();
            supervisorRow.DeleteRow();
            teachingGroupTriplet.ClickSave();
            teachingGroupDetail.Refresh();
            teachingGroupTriplet.Refresh();
            teachingGroupTriplet.Delete();
            SeleniumHelper.Logout();

            #endregion

            #region Post-Condition : Delete Staff

            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.PersonnelOfficer);

            //Delete exist record            
            SeleniumHelper.NavigateMenu("Tasks", "Staff", "Delete Staff Record");
            deleteStaffRecords = new DeleteStaffRecordTriplet();

            //Search exists staff
            deleteStaffRecords.SearchCriteria.StaffName = supervisorName;
            deleteStaffRecords.SearchCriteria.IsCurrent = true;
            deleteStaffRecords.SearchCriteria.IsFuture = true;
            deleteStaffRecords.SearchCriteria.IsLeaver = true;
            staffSearchResults = deleteStaffRecords.SearchCriteria.Search();
            staffSearchTile = staffSearchResults.SingleOrDefault(t => t.Name.Equals(supervisorName));
            staffRecord = staffSearchTile == null ? StaffRecordPage.Create() : staffSearchTile.Click<StaffRecordPage>();

            //Delete the first staff if exist
            staffRecord.DeleteStaff();
            staffRecord = staffRecord.ContinueDeleteStaff();

            //Search exists staff
            deleteStaffRecords.Refresh();
            deleteStaffRecords.SearchCriteria.StaffName = newSupervisorName;
            staffSearchResults = deleteStaffRecords.SearchCriteria.Search();
            staffSearchTile = staffSearchResults.SingleOrDefault(t => t.Name.Equals(newSupervisorName));
            staffRecord = staffSearchTile == null ? StaffRecordPage.Create() : staffSearchTile.Click<StaffRecordPage>();

            //Delete the second staff if exist
            staffRecord.DeleteStaff();
            staffRecord = staffRecord.ContinueDeleteStaff();

            #endregion
        }

        /// <summary>
        /// TC TG-08
        /// Au : An Nguyen
        /// Description: Amend a Teaching Group: Ensure a Supervisor cannot be input twice for the same role with overlapping date ranges with the Current academic Year
        /// Role: School Administrator
        /// Issue : Pupil disappear after save (IE)
        /// </summary>
        [WebDriverTest(TimeoutSeconds = 600, Enabled = false, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { "g1" }, DataProvider = "TC_TG008_Data")]
        public void TC_TG008_Ensure_a_Supervisor_cannot_be_input_twice_for_the_same_role_with_overlapping_date_ranges_with_the_Current_academic_Year(string fullName, string shortName, string subject, string note,
                    string supervisorName, string staffRole, string startDate, string endDate)
        {
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);

            #region Pre-Condition : Add new teaching group

            //Navigate to Manage Teaching Groups
            SeleniumHelper.NavigateMenu("Tasks", "School Groups", "Manage Teaching Groups");
            var teachingGroupTriplet = new TeachingGroupTriplet();

            //Delete old teaching group if exist
            teachingGroupTriplet.SearchCriteria.GroupFullName = fullName;
            var listSearchResult = teachingGroupTriplet.SearchCriteria.Search();
            var schoolGroupResult = listSearchResult.FirstOrDefault(t => t.Name.Equals(fullName));
            var teachingGroupDetail = schoolGroupResult == null ? null : schoolGroupResult.Click<TeachingGroupPage>();
            teachingGroupTriplet.Delete();

            //Create new teaching group
            teachingGroupDetail = teachingGroupTriplet.ClickCreate();

            //Add group details
            teachingGroupDetail.FullName = fullName;
            teachingGroupDetail.ShortName = shortName;
            teachingGroupDetail.Subject = subject;
            teachingGroupDetail.Visibility = true;
            teachingGroupDetail.Notes = note;

            //Add supervisor
            var addSupervisors = teachingGroupDetail.OpenAddSupervisorDialog();
            addSupervisors.SearchCriteria.StaffRole = staffRole;
            var addSupervisor = addSupervisors.SearchCriteria.Search<AddSupervisorsDialogDetail>();
            var supervisors = addSupervisor.SearchResult;
            var supervisor = supervisors.FirstOrDefault(t => t.Text.Contains(supervisorName));
            supervisor.ClickByJS();
            addSupervisor.AddSelectedSupervisor();
            addSupervisors.ClickOk();
            var supervisorTable = teachingGroupDetail.SupervisorTable;
            supervisorTable.WaitUntilRowAppear(t => t.Name.Contains(supervisorName));
            var supervisorRow = supervisorTable.Rows.FirstOrDefault(t => t.Name.Contains(supervisorName));
            supervisorRow.Role = staffRole;

            //Save
            teachingGroupTriplet.ClickSave();

            #endregion

            #region Test steps

            //Search teaching group
            teachingGroupTriplet.Refresh();
            teachingGroupTriplet.SearchCriteria.GroupFullName = fullName;
            var teachingGroupSearchResult = teachingGroupTriplet.SearchCriteria.Search();
            var teachingGroupTile = teachingGroupSearchResult.FirstOrDefault(t => t.Name.Equals(fullName));
            teachingGroupDetail = teachingGroupTile.Click<TeachingGroupPage>();

            //Edit end date of the first supervisor
            supervisorTable = teachingGroupDetail.SupervisorTable;
            supervisorRow = supervisorTable.Rows.FirstOrDefault(t => t.Name.Contains(supervisorName));
            Assert.AreNotEqual(null, supervisorRow, "Supervisor does not display on the grid");
            supervisorRow.To = endDate;
            teachingGroupDetail.Save();

            //Add supervisor
            addSupervisors = teachingGroupDetail.OpenAddSupervisorDialog();
            addSupervisors.SearchCriteria.StaffRole = staffRole;
            addSupervisor = addSupervisors.SearchCriteria.Search<AddSupervisorsDialogDetail>();
            supervisors = addSupervisor.SearchResult;
            supervisor = supervisors.FirstOrDefault(t => t.Text.Contains(supervisorName));
            supervisor.ClickByJS();
            addSupervisor.AddSelectedSupervisor();
            addSupervisors.ClickOk();
            supervisorTable = teachingGroupDetail.SupervisorTable;
            supervisorTable.WaitUntilRowAppear(t => t.Name.Contains(supervisorName) && t.Role.Trim().Equals(String.Empty));
            supervisorRow = supervisorTable.Rows.FirstOrDefault(t => t.Name.Contains(supervisorName) && t.Role.Trim().Equals(String.Empty));
            supervisorRow.Role = staffRole;
            supervisorRow.From = startDate;

            //Save
            teachingGroupDetail.Save();

            //Verify error message display
            teachingGroupTriplet.Refresh();
            Assert.AreEqual(true, teachingGroupTriplet.IsErrorMessageDisplay(), "Warning error does not display");

            #endregion

            #region Post-Condition : Delete Teaching Group

            supervisorTable = teachingGroupDetail.SupervisorTable;
            supervisorRow = supervisorTable.Rows.FirstOrDefault(t => t.Name.Contains(supervisorName));
            supervisorRow.DeleteRow();
            supervisorTable.Refresh();
            supervisorRow = supervisorTable.Rows.FirstOrDefault(t => t.Name.Contains(supervisorName));
            supervisorRow.DeleteRow();
            teachingGroupDetail.Save();
            teachingGroupTriplet.Refresh();
            teachingGroupTriplet.Delete();

            #endregion
        }

        /// <summary>
        /// Author: Hieu Pham
        /// Description : Amend a Teaching Group: Ensure the same Supervisor can be input twice for the same role with non overlapping date ranges for the Current academic Year
        /// Status : Pending by Bug #13: [TEACHING GROUP - USER DEFINED GROUP] There are no result when searching  any Supervisor with specific role on 'Add Supervisor Dialog'
        /// </summary>        
        [WebDriverTest(TimeoutSeconds = 1200, Enabled = false, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { "g1" }, DataProvider = "TC_TG009_Data")]
        public void TC_TG009_Amend_Teaching_Group(string[] teachingGroup, string[] staff, string[] pupil)
        {
            #region Pre-Condition: Create new Teaching Group for current academic year

            //Log in as a Test User
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.TestUserWaterEdgePrimary);

            // Navigate to Manage Teaching Groups
            SeleniumHelper.NavigateMenu("Tasks", "School Groups", "Manage Teaching Groups");
            var teachingGroupTriplet = new TeachingGroupTriplet();

            // Delete if existed
            teachingGroupTriplet.SearchCriteria.GroupFullName = teachingGroup[0];
            var listSearchResult = teachingGroupTriplet.SearchCriteria.Search();
            var schoolGroupResult = listSearchResult.FirstOrDefault(t => t.Name.Equals(teachingGroup[0]));
            var teachingGroupDetail = schoolGroupResult == null ? null : schoolGroupResult.Click<TeachingGroupPage>();
            teachingGroupTriplet.Delete();

            // Create a new teaching group
            teachingGroupDetail = teachingGroupTriplet.ClickCreate();

            // Enter values
            teachingGroupDetail.FullName = teachingGroup[0];
            teachingGroupDetail.ShortName = teachingGroup[1];
            teachingGroupDetail.Subject = teachingGroup[2];

            // Scroll to supervisor
            teachingGroupDetail.ScrollToSupervisor();

            // Click add supervisor and search
            var supervisorDialogTriplet = teachingGroupDetail.OpenAddSupervisorDialog();
            supervisorDialogTriplet.SearchCriteria.LegalSurName = staff[0];
            var supervisorDialogDetail = supervisorDialogTriplet.SearchCriteria.Search<AddSupervisorsDialogDetail>();

            // Select Supervisor then click OK
            var supervisorSearchResult = supervisorDialogDetail.SearchResult.FirstOrDefault(x => x.Text.Equals(staff[6]));

            Assert.AreNotEqual(null, supervisorSearchResult, "Search supervisor function is not correct or record is not existed");
            supervisorSearchResult.Click();
            supervisorDialogDetail.AddSelectedSupervisor();

            // Click OK
            supervisorDialogTriplet.Refresh();
            supervisorDialogTriplet.ClickOk();

            // Select role and start date, end date for new record
            teachingGroupDetail.Refresh();
            var supervisorTable = teachingGroupDetail.SupervisorTable;
            supervisorTable.WaitUntilRowAppear(x => x.Name.Equals(staff[6]));
            var supervisorRow = supervisorTable.Rows.FirstOrDefault(x => x.Name.Equals(staff[6]));
            supervisorRow.Role = staff[1];
            supervisorRow.From = staff[2];
            supervisorRow.To = staff[3];

            // Scroll to Pupil
            teachingGroupDetail.Refresh();
            teachingGroupDetail.ScrollToPupil();

            // Click add new pupil
            var addPupilDialogTriplet = teachingGroupDetail.OpenAddPupilsDialog();
            addPupilDialogTriplet.SearchCriteria.PupilName = pupil[0];
            var addPupilDialogDetail = addPupilDialogTriplet.SearchCriteria.Search<AddPupilsDialogDetail>();

            // Select pupil
            addPupilDialogDetail.Pupils.FirstOrDefault(x => x.Text.Equals(pupil[0])).Click();
            addPupilDialogDetail.AddSelectedPupils();

            // Click OK
            addPupilDialogTriplet.Refresh();
            addPupilDialogTriplet.ClickOk();

            // Select start date and end date for pupil record
            teachingGroupDetail.Refresh();
            var pupilTable = teachingGroupDetail.PupilsTable;
            pupilTable.WaitUntilRowAppear(x => x.Name.Contains(pupil[0]));
            var pupilRow = pupilTable.Rows.FirstOrDefault(x => x.Name.Contains(pupil[0]));
            pupilRow.From = pupil[1];
            pupilRow.To = pupil[2];

            // Save value
            teachingGroupDetail.Save();

            #endregion

            #region Steps

            // Search teaching group again
            teachingGroupTriplet.Refresh();
            teachingGroupTriplet.SearchCriteria.GroupFullName = teachingGroup[0];
            listSearchResult = teachingGroupTriplet.SearchCriteria.Search();
            schoolGroupResult = listSearchResult.FirstOrDefault(t => t.Name.Equals(teachingGroup[0]));
            teachingGroupDetail = schoolGroupResult.Click<TeachingGroupPage>();

            // Scroll to Supervisor
            teachingGroupDetail.ScrollToSupervisor();

            // Verify supervisor is existed
            supervisorTable = teachingGroupDetail.SupervisorTable;
            supervisorRow = supervisorTable.Rows.FirstOrDefault(x => x.Name.Equals(staff[6]));
            Assert.AreNotEqual(null, supervisorRow, "Supervisor row is not existed");

            // Scroll to pupil
            teachingGroupDetail.ScrollToPupil();

            // Verify pupil is existed
            pupilTable = teachingGroupDetail.PupilsTable;
            pupilRow = pupilTable.Rows.FirstOrDefault(x => x.Name.Contains(pupil[0]));
            Assert.AreNotEqual(null, pupilRow, "Pupil row is not existed");

            // Click add supervisor
            supervisorDialogTriplet = teachingGroupDetail.OpenAddSupervisorDialog();

            // Enter Staff member name
            supervisorDialogTriplet.SearchCriteria.LegalSurName = staff[0];
            supervisorDialogDetail = supervisorDialogTriplet.SearchCriteria.Search<AddSupervisorsDialogDetail>();

            // Select Supervisor then click OK
            supervisorSearchResult = supervisorDialogDetail.SearchResult.FirstOrDefault(x => x.Text.Equals(staff[6]));
            Assert.AreNotEqual(null, supervisorSearchResult, "Search supervisor function is not correct or record is not existed");
            supervisorSearchResult.Click();
            supervisorDialogDetail.AddSelectedSupervisor();

            // Click OK
            supervisorDialogTriplet.Refresh();
            supervisorDialogTriplet.ClickOk();

            // Select role for new record in supervisor
            supervisorTable = teachingGroupDetail.SupervisorTable;
            supervisorTable.WaitUntilRowAppear(x => x.Role.Equals(String.Empty));
            var row = supervisorTable.Rows.FirstOrDefault(x => x.Role.Equals(String.Empty));
            row.Role = staff[1];
            row.From = staff[4];
            row.To = staff[5];

            // Save record
            teachingGroupDetail.Refresh();
            teachingGroupDetail.Save();

            // VP : Ensure the Selected Supervisors and Pupils matches those stored on the Teaching Group and the reflects the Date Ranges selected
            // Search teaching group again
            teachingGroupTriplet.Refresh();
            teachingGroupTriplet.SearchCriteria.GroupFullName = teachingGroup[0];
            schoolGroupResult = listSearchResult.FirstOrDefault(t => t.Name.Equals(teachingGroup[0]));
            teachingGroupDetail = schoolGroupResult.Click<TeachingGroupPage>();

            // VP : Supervisor is correct.
            teachingGroupDetail.ScrollToSupervisor();
            supervisorTable = teachingGroupDetail.SupervisorTable;
            supervisorRow = supervisorTable.Rows.FirstOrDefault(x => x.Name.Equals(staff[6]) && x.Role.Equals(staff[1]) && x.From.Equals(staff[2]) && x.To.Equals(staff[3]));
            Assert.AreNotEqual(null, supervisorRow, "Supervisor row is not correct");
            supervisorRow = supervisorTable.Rows.FirstOrDefault(x => x.Name.Equals(staff[6]) && x.Role.Equals(staff[1]) && x.From.Equals(staff[4]) && x.To.Equals(staff[5]));
            Assert.AreNotEqual(null, supervisorRow, "Supervisor row is not correct");

            // VP : Pupil is correct
            teachingGroupDetail.ScrollToPupil();
            pupilTable = teachingGroupDetail.PupilsTable;
            pupilRow = pupilTable.Rows.FirstOrDefault(x => x.Name.Contains(pupil[0]) && x.From.Equals(pupil[1]) && x.To.Equals(pupil[2]));
            Assert.AreNotEqual(null, pupilRow, "Pupil is not correct");


            #endregion

            #region Post-Condition: Delete teaching group

            // Delete supervisor
            supervisorTable.Refresh();
            supervisorRow = supervisorTable.Rows.FirstOrDefault(x => x.Name.Equals(staff[6]) && x.Role.Equals(staff[1]) && x.From.Equals(staff[2]) && x.To.Equals(staff[3]));
            supervisorRow.DeleteRow();
            supervisorTable.Refresh();
            supervisorRow = supervisorTable.Rows.FirstOrDefault(x => x.Name.Equals(staff[6]) && x.Role.Equals(staff[1]) && x.From.Equals(staff[4]) && x.To.Equals(staff[5]));
            supervisorRow.DeleteRow();

            // Delete pupil
            pupilTable.Refresh();
            pupilRow = pupilTable.Rows.FirstOrDefault(x => x.Name.Contains(pupil[0]) && x.From.Equals(pupil[1]) && x.To.Equals(pupil[2]));
            pupilRow.DeleteRow();

            // Save value
            teachingGroupDetail.Refresh();
            teachingGroupDetail.Save();

            // Delete teaching group
            teachingGroupDetail.Refresh();
            teachingGroupTriplet.Delete();

            #endregion

        }

        /// <summary>
        /// Author: Y.Ta
        /// Amend a Teaching Group: Ensure a Supervisor can be in a different role with overlapping date ranges for a Future academic Year
        /// </summary>        
        [WebDriverTest(TimeoutSeconds = 800, Enabled = false, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { "g1" }, DataProvider = "TC_TG010_Data")]
        public void TC_TG010_Verify_Supervisor_Different_Role_Group_Overlapping_Date_Ranges_Future_Academic_Year(string[] schoolGroup, string[] staff, string roleUpdate)
        {
            #region Pre-Condition: Create new School Group for future academic year with contains the supervisor

            // Add new role into exist staff
            // Log in as a School PersonnelOfficer
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.PersonnelOfficer);
            // Navigate to Manage Staff
            SeleniumHelper.NavigateMenu("Tasks", "Staff", "Staff Records");
            var staffRecordTriplet = new StaffRecordTriplet();
            staffRecordTriplet.SearchCriteria.StaffName = staff[0];
            var staffRecordListItem = staffRecordTriplet.SearchCriteria.Search();
            var staffRecordEdit = staffRecordListItem.FirstOrDefault(p => p.Name.Contains(staff[0]));
            var staffRecordPage = staffRecordEdit.Click<StaffRecordPage>();
            var rowUpdate = staffRecordPage.StaffRoleStandardTable.Rows.FirstOrDefault(p => p.StaffRole.Contains(roleUpdate));
            var temp = roleUpdate == null ? staffRecordPage.StaffRoleStandardTable.GetLastRow().StaffRole = roleUpdate : null;

            staffRecordPage.Refresh();
            staffRecordPage.SaveStaff();

            SeleniumHelper.Logout();
            // Log in as a School Administrator
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);
            // Navigate to Manage Teaching Groups
            SeleniumHelper.NavigateMenu("Tasks", "School Groups", "Manage Teaching Groups");
            var teachingGroupTriplet = new TeachingGroupTriplet();

            teachingGroupTriplet.SearchCriteria.GroupFullName = schoolGroup[0];
            var listSearchResult = teachingGroupTriplet.SearchCriteria.Search();
            var schoolGroupResult = listSearchResult.FirstOrDefault(t => t.Name.Equals(schoolGroup[0]));
            var teachingGroupDetail = schoolGroupResult == null ? null : schoolGroupResult.Click<TeachingGroupPage>();
            teachingGroupTriplet.Delete();

            teachingGroupDetail = teachingGroupTriplet.ClickCreate();

            teachingGroupDetail.FullName = schoolGroup[0];
            teachingGroupDetail.ShortName = schoolGroup[1];
            teachingGroupDetail.Subject = schoolGroup[2];
            teachingGroupTriplet.ClickSave();

            // Effective date range in the future academic Year
            teachingGroupDetail.Refresh();
            var selectEffectDateRange = teachingGroupDetail.ClickSelectEffectDateRange();
            selectEffectDateRange.AcademicYear = schoolGroup[3];
            selectEffectDateRange.ClickOk();

            teachingGroupDetail.Refresh();

            // Issue: The function search does not return any supervisors.
            // Search and Add the Supervisor into School Groups
            teachingGroupDetail.Refresh();
            var addSupervisorsDialogTriplet = teachingGroupDetail.OpenAddSupervisorDialog();
            addSupervisorsDialogTriplet.SearchCriteria.LegalSurName = staff[0];
            addSupervisorsDialogTriplet.SearchCriteria.StaffRole = staff[1];

            // Click to add staff teacher
            var addSupervisorsDialogDetail = addSupervisorsDialogTriplet.SearchCriteria.Search<AddSupervisorsDialogDetail>();

            Assert.AreNotEqual(0, addSupervisorsDialogDetail.SearchResult.Count(), "There are no result when searching supervisor");

            var staffItem = addSupervisorsDialogDetail.SearchResult.FirstOrDefault(p => p.GetText().Contains(staff[0]));
            staffItem.ClickByJS();
            addSupervisorsDialogDetail.AddSelectedSupervisor();
            addSupervisorsDialogTriplet.ClickOk();

            teachingGroupDetail.Refresh();
            teachingGroupDetail.SupervisorTable.WaitUntilRowAppear(p => p.Name.Contains(staff[0]));
            teachingGroupDetail.SupervisorTable[0].Role = staff[1];

            teachingGroupTriplet.ClickSave();
            #endregion

            #region Steps
            //Enter the Name Staff member noted above            
            // Issue: The function search does not return any supervisors.
            // Search and Add the Supervisor into School Groups
            teachingGroupDetail.Refresh();
            addSupervisorsDialogTriplet = teachingGroupDetail.OpenAddSupervisorDialog();
            addSupervisorsDialogTriplet.SearchCriteria.LegalSurName = staff[0];
            addSupervisorsDialogTriplet.SearchCriteria.StaffRole = staff[1];

            // Click to add this staff
            addSupervisorsDialogDetail = addSupervisorsDialogTriplet.SearchCriteria.Search<AddSupervisorsDialogDetail>();
            // Issue: The function search does not return any supervisors.
            Assert.AreNotEqual(0, addSupervisorsDialogDetail.SearchResult.Count(), "There are no result when searching supervisor");
            staffItem = addSupervisorsDialogDetail.SearchResult.FirstOrDefault(p => p.GetText().Contains(staff[0]));
            staffItem.ClickByJS();
            addSupervisorsDialogDetail.AddSelectedSupervisor();
            addSupervisorsDialogTriplet.ClickOk();


            teachingGroupDetail.SupervisorTable.WaitForNewRowAppear();

            // Select value from Drop Down List Box 'Role' that matches the Role the Staff Member is already allocated to
            teachingGroupDetail.SupervisorTable[1].Role = roleUpdate;
            // Amend the Dates for the 2 records to that the Start / End Dates Overlap by 1 day
            var fromDate = teachingGroupDetail.SupervisorTable[0].From;
            teachingGroupDetail.SupervisorTable[1].From = fromDate;
            var toDate = teachingGroupDetail.SupervisorTable[0].To;
            teachingGroupDetail.SupervisorTable[1].To = toDate;

            // Verify the staff display            
            teachingGroupTriplet.ClickSave();
            Assert.AreEqual(true, teachingGroupTriplet.DoesMessageSuccessDisplay(), "The success message does not display");
            var supervisorRow = teachingGroupDetail.SupervisorTable.Rows.Any(p => p.Role.Equals(roleUpdate) && p.Name.Contains(staff[0]) && p.From.Equals(fromDate) && p.To.Equals(toDate));
            Assert.AreEqual(true, supervisorRow, "The row does not display after saving School Group");

            // Verify the reflects the Date Ranges selected
            //teachingGroupDetail.Refresh();
            //Assert.AreEqual(schoolGroup[4], teachingGroupDetail.EffectiveDate, "The effect date range is not correct");

            #endregion

            #region End-Condition: Delete school group
            // Remove supervisor row       

            var delRow = teachingGroupDetail.SupervisorTable.Rows.FirstOrDefault(p => p.Name.Contains(staff[0]));
            delRow.DeleteRow();

            delRow = teachingGroupDetail.SupervisorTable.Rows.FirstOrDefault(p => p.Name.Contains(staff[0]));
            delRow.DeleteRow();

            teachingGroupTriplet.ClickSave();

            teachingGroupTriplet.SearchCriteria.GroupFullName = schoolGroup[0];
            listSearchResult = teachingGroupTriplet.SearchCriteria.Search();
            schoolGroupResult = listSearchResult.FirstOrDefault(t => t.Name.Equals(schoolGroup[0]));
            teachingGroupDetail = listSearchResult == null ? null : schoolGroupResult.Click<TeachingGroupPage>();
            teachingGroupTriplet.Delete();

            #endregion

        }



        /// <summary>
        /// Author: Y.Ta
        /// Amend a Teaching Group: Ensure a Supervisor can be in a different role with overlapping date ranges for a current academic Year
        /// </summary>        
        [WebDriverTest(TimeoutSeconds = 800, Enabled = false, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { "g1" }, DataProvider = "TC_TG011_Data")]
        public void TC_TG011_Verify_Supervisor_Different_Role_Group_Overlapping_Date_Ranges_Current_Academic_Year(string[] schoolGroup, string[] staff, string roleUpdate)
        {
            #region Pre-Condition: Create new School Group for future academic year with contains the supervisor
            // Add new role into exist staff
            // Log in as a School PersonnelOfficer
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.PersonnelOfficer);
            // Navigate to Manage Staff
            SeleniumHelper.NavigateMenu("Tasks", "Staff", "Staff Records");
            var staffRecordTriplet = new StaffRecordTriplet();
            staffRecordTriplet.SearchCriteria.StaffName = staff[0];
            var staffRecordListItem = staffRecordTriplet.SearchCriteria.Search();
            var staffRecordEdit = staffRecordListItem.FirstOrDefault(p => p.Name.Contains(staff[0]));
            var staffRecordPage = staffRecordEdit.Click<StaffRecordPage>();
            var rowUpdate = staffRecordPage.StaffRoleStandardTable.Rows.FirstOrDefault(p => p.StaffRole.Contains(roleUpdate));
            var temp = roleUpdate == null ? staffRecordPage.StaffRoleStandardTable.GetLastRow().StaffRole = roleUpdate : null;

            staffRecordPage.Refresh();
            staffRecordPage.SaveStaff();

            SeleniumHelper.Logout();
            //Log in as a School Administrator
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);
            // Navigate to Manage Teaching Groups
            SeleniumHelper.NavigateMenu("Tasks", "School Groups", "Manage Teaching Groups");
            var teachingGroupTriplet = new TeachingGroupTriplet();

            teachingGroupTriplet.SearchCriteria.GroupFullName = schoolGroup[0];
            var listSearchResult = teachingGroupTriplet.SearchCriteria.Search();
            var schoolGroupResult = listSearchResult.FirstOrDefault(t => t.Name.Equals(schoolGroup[0]));
            var teachingGroupDetail = schoolGroupResult == null ? null : schoolGroupResult.Click<TeachingGroupPage>();
            teachingGroupTriplet.Delete();

            teachingGroupDetail = teachingGroupTriplet.ClickCreate();

            teachingGroupDetail.FullName = schoolGroup[0];
            teachingGroupDetail.ShortName = schoolGroup[1];
            teachingGroupDetail.Subject = schoolGroup[2];
            teachingGroupTriplet.ClickSave();

            // Effective date range in the future academic Year
            teachingGroupDetail.Refresh();
            var selectEffectDateRange = teachingGroupDetail.ClickSelectEffectDateRange();
            selectEffectDateRange.AcademicYear = schoolGroup[3];
            selectEffectDateRange.ClickOk();

            teachingGroupDetail.Refresh();
            // Issue: The function search does not return any supervisors.
            // Search and Add the Supervisor into School Groups
            teachingGroupDetail.Refresh();
            var addSupervisorsDialogTriplet = teachingGroupDetail.OpenAddSupervisorDialog();
            addSupervisorsDialogTriplet.SearchCriteria.LegalSurName = staff[0];
            addSupervisorsDialogTriplet.SearchCriteria.StaffRole = staff[1];

            // Click to add staff teacher
            var addSupervisorsDialogDetail = addSupervisorsDialogTriplet.SearchCriteria.Search<AddSupervisorsDialogDetail>();

            Assert.AreNotEqual(0, addSupervisorsDialogDetail.SearchResult.Count(), "There are no result when searching supervisor");

            var staffItem = addSupervisorsDialogDetail.SearchResult.FirstOrDefault(p => p.GetText().Contains(staff[0]));
            staffItem.ClickByJS();
            addSupervisorsDialogDetail.AddSelectedSupervisor();
            addSupervisorsDialogTriplet.ClickOk();

            teachingGroupDetail.Refresh();
            teachingGroupDetail.SupervisorTable.WaitUntilRowAppear(p => p.Name.Contains(staff[0]));
            teachingGroupDetail.SupervisorTable[0].Role = staff[1];

            teachingGroupTriplet.ClickSave();
            #endregion

            #region Steps
            //Enter the Name Staff member noted above
            teachingGroupDetail.Refresh();
            // Issue: The function search does not return any supervisors.
            // Search and Add the Supervisor into School Groups
            teachingGroupDetail.Refresh();
            addSupervisorsDialogTriplet = teachingGroupDetail.OpenAddSupervisorDialog();
            addSupervisorsDialogTriplet.SearchCriteria.LegalSurName = staff[0];
            addSupervisorsDialogTriplet.SearchCriteria.StaffRole = staff[1];

            // Click to add this staff
            addSupervisorsDialogDetail = addSupervisorsDialogTriplet.SearchCriteria.Search<AddSupervisorsDialogDetail>();
            // Issue: The function search does not return any supervisors.
            Assert.AreNotEqual(0, addSupervisorsDialogDetail.SearchResult.Count(), "There are no result when searching supervisor");
            staffItem = addSupervisorsDialogDetail.SearchResult.FirstOrDefault(p => p.GetText().Contains(staff[0]));
            staffItem.ClickByJS();
            addSupervisorsDialogDetail.AddSelectedSupervisor();
            addSupervisorsDialogTriplet.ClickOk();

            // Select value from Drop Down List Box 'Role' that matches the Role the Staff Member is already allocated to
            teachingGroupDetail.SupervisorTable[1].Role = roleUpdate;
            // Amend the Dates for the 2 records to that the Start / End Dates Overlap by 1 day
            var fromDate = teachingGroupDetail.SupervisorTable[0].From;
            teachingGroupDetail.SupervisorTable[1].From = fromDate;
            var toDate = teachingGroupDetail.SupervisorTable[0].To;
            teachingGroupDetail.SupervisorTable[1].To = toDate;

            // Verify the staff display            
            teachingGroupTriplet.ClickSave();
            Assert.AreEqual(true, teachingGroupTriplet.DoesMessageSuccessDisplay(), "The success message does not display");
            var supervisorRow = teachingGroupDetail.SupervisorTable.Rows.Any(p => p.Role.Equals(roleUpdate) && p.Name.Contains(staff[0]) && p.From.Equals(fromDate) && p.To.Equals(toDate));
            Assert.AreEqual(true, supervisorRow, "The row does not display after saving School Group");

            // Verify the reflects the Date Ranges selected
            //teachingGroupDetail.Refresh();
            //Assert.AreEqual(schoolGroup[4], teachingGroupDetail.EffectiveDate, "The effect date range is not correct");
            #endregion

            #region End-Condition: Delete school group
            // Remove supervisor row            
            var deleteSupervisorRow = teachingGroupDetail.SupervisorTable.Rows.FirstOrDefault(p => p.Name.Contains(staff[0]));
            deleteSupervisorRow.DeleteRow();

            deleteSupervisorRow = teachingGroupDetail.SupervisorTable.Rows.FirstOrDefault(p => p.Name.Contains(staff[0]));
            deleteSupervisorRow.DeleteRow();

            teachingGroupTriplet.ClickSave();

            teachingGroupTriplet.SearchCriteria.GroupFullName = schoolGroup[0];
            listSearchResult = teachingGroupTriplet.SearchCriteria.Search();
            schoolGroupResult = listSearchResult.FirstOrDefault(t => t.Name.Equals(schoolGroup[0]));
            teachingGroupDetail = listSearchResult == null ? null : schoolGroupResult.Click<TeachingGroupPage>();
            teachingGroupTriplet.Delete();

            #endregion

        }

        /// <summary>
        /// Author: Huy.Vo
        /// Des: Amend a Teaching Group: Ensure a Supervisor cannot be in a the same role with overlapping date ranges for a  Future academic Year 
        /// </summary>        
        [WebDriverTest(TimeoutSeconds = 1200, Enabled = false, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { "g1" }, DataProvider = "TC_TG012_Data")]
        public void TC_TG012_Amend_A_Teaching_Ensure_Supervisor_Cannot_Be_The_Same_Role_With_Future_Academic_Year(
                    string groupFullName, string groupShortName, string subject,
                    string futureAcademicYear, string staffName, string staffRole, string fromDate, string toDate,
                    string pupilName, string yearGroup, string classes)
        {

            #region Pre-Condition: Create new School Group for currrent academic year

            //Log in as a School Administrator
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);

            // Navigate to Manage Teaching Groups
            SeleniumHelper.NavigateMenu("Tasks", "School Groups", "Manage Teaching Groups");

            //Search Teaching group to delete if it is existing
            var teachingGroupTriplet = new TeachingGroupTriplet();
            teachingGroupTriplet.SearchCriteria.GroupFullName = groupFullName;
            var listSearchResult = teachingGroupTriplet.SearchCriteria.Search();
            var schoolGroupResult = listSearchResult.FirstOrDefault(t => t.Name.Equals(groupFullName));
            var teachingGroupDetail = schoolGroupResult == null ? null : schoolGroupResult.Click<TeachingGroupPage>();
            teachingGroupTriplet.Delete();

            //Create new Teaching Group
            teachingGroupDetail = teachingGroupTriplet.ClickCreate();
            teachingGroupDetail.FullName = groupFullName;
            teachingGroupDetail.ShortName = groupShortName;
            teachingGroupDetail.Subject = subject;
            teachingGroupTriplet.ClickSave();

            // Effective date range in the  current academic Year
            teachingGroupDetail.Refresh();
            var selectEffectDateRange = teachingGroupDetail.ClickSelectEffectDateRange();
            selectEffectDateRange.AcademicYear = futureAcademicYear;
            selectEffectDateRange.ClickOk();
            teachingGroupTriplet.ClickSave();

            // Search and Add the Supervisor 1 into School Groups
            teachingGroupDetail.Refresh();
            var addSupervisorsDialogTriplet = teachingGroupDetail.OpenAddSupervisorDialog();
            addSupervisorsDialogTriplet.SearchCriteria.StaffRole = staffRole;

            // Click to add this staff 1
            var addSupervisorsDialogDetail = addSupervisorsDialogTriplet.SearchCriteria.Search<AddSupervisorsDialogDetail>();
            Assert.AreNotEqual(0, addSupervisorsDialogDetail.SearchResult.Count(), "There are no result when searching supervisor 1");
            var staffItem = addSupervisorsDialogDetail.SearchResult.FirstOrDefault(p => p.GetText().Contains(staffName));
            staffItem.ClickByJS();
            addSupervisorsDialogDetail.AddSelectedSupervisor();
            addSupervisorsDialogTriplet.ClickOk();

            //Set From date and To date for staff 1
            var supervisorTable = teachingGroupDetail.SupervisorTable;
            supervisorTable[0].Role = staffRole;
            supervisorTable[0].From = fromDate;
            supervisorTable[0].To = toDate;

            teachingGroupTriplet.ClickSave();

            // Search and Add the pupil into School Groups
            teachingGroupDetail.Refresh();
            var addPupilsDialogTriplet = teachingGroupDetail.OpenAddPupilsDialog();

            // Search specific Pupil to add to Teaching Group
            addPupilsDialogTriplet.SearchCriteria.PupilName = pupilName;
            addPupilsDialogTriplet.SearchCriteria.YearGroup = yearGroup;
            addPupilsDialogTriplet.SearchCriteria.Class = classes;
            var addPupilDialogDetail = addPupilsDialogTriplet.SearchCriteria.Search<AddPupilsDialogDetail>();
            addPupilDialogDetail.Refresh();

            // Click to add this Pupil
            var pupilItem = addPupilDialogDetail.Pupils.FirstOrDefault(p => p.GetText().Contains(pupilName));
            pupilItem.ClickByJS();
            addPupilDialogDetail.AddSelectedPupils();
            addPupilsDialogTriplet.ClickOk();

            teachingGroupTriplet.ClickSave();

            #endregion

            #region Edit Supervisor select twice staffs that is the same Supervisor role and  Start / End Dates Overlap

            //Searching and add Staff 2
            teachingGroupTriplet.SearchCriteria.GroupFullName = groupFullName;
            listSearchResult = teachingGroupTriplet.SearchCriteria.Search();
            schoolGroupResult = listSearchResult.FirstOrDefault(t => t.Name.Equals(groupFullName));
            teachingGroupDetail = schoolGroupResult == null ? null : schoolGroupResult.Click<TeachingGroupPage>();

            //Set next Academic Year
            teachingGroupDetail.Refresh();
            selectEffectDateRange = teachingGroupDetail.ClickSelectEffectDateRange();
            selectEffectDateRange.AcademicYear = futureAcademicYear;
            selectEffectDateRange.ClickOk();
            teachingGroupTriplet.ClickSave();
            teachingGroupDetail.Refresh();

            // Click to add this staff 2
            addSupervisorsDialogTriplet = teachingGroupDetail.OpenAddSupervisorDialog();
            addSupervisorsDialogTriplet.SearchCriteria.StaffRole = staffRole;

            // Click to add this staff 2
            addSupervisorsDialogDetail = addSupervisorsDialogTriplet.SearchCriteria.Search<AddSupervisorsDialogDetail>();

            Assert.AreNotEqual(0, addSupervisorsDialogDetail.SearchResult.Count(), "There are no result when searching supervisor 2");
            staffItem = addSupervisorsDialogDetail.SearchResult.FirstOrDefault(p => p.GetText().Contains(staffName));
            staffItem.ClickByJS();
            addSupervisorsDialogDetail.AddSelectedSupervisor();
            addSupervisorsDialogTriplet.ClickOk();

            supervisorTable = teachingGroupDetail.SupervisorTable;
            supervisorTable[1].Role = staffRole;
            supervisorTable[1].From = fromDate;
            supervisorTable[1].To = toDate;

            teachingGroupTriplet.ClickSave();

            //Verify a error massage is displayed 
            Assert.AreEqual(true, teachingGroupTriplet.IsErrorMessageDisplay(), "The error message is not displayed");

            #endregion

            #region End-Condition: Delete school group

            // Remove pupil row
            var deletePupilRow = teachingGroupDetail.PupilsTable.Rows.FirstOrDefault(p => p.Name.Contains(pupilName));
            deletePupilRow.DeleteRow();

            // Remove supervisor row
            var deleteSupervisorRow = teachingGroupDetail.SupervisorTable.Rows.FirstOrDefault(p => p.Name.Contains(staffName));
            deleteSupervisorRow.DeleteRow();
            deleteSupervisorRow = teachingGroupDetail.SupervisorTable.Rows.FirstOrDefault(p => p.Name.Contains(staffName));
            deleteSupervisorRow.DeleteRow();

            //Delete Teaching group
            teachingGroupTriplet.ClickSave();
            teachingGroupTriplet.Delete();
            #endregion

        }


        /// <summary>
        /// Author: Y.Ta
        /// Amend a Teaching Group: Ensure creating more than 1  Main Supervisor on any given day is prevented in a future Academic Year 
        /// </summary>        
        [WebDriverTest(TimeoutSeconds = 800, Enabled = false, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { "g1" }, DataProvider = "TC_TG013_Data")]
        public void TC_TG013_Verify_Creating_More_Than_One_Supervisor_On_Given_Day_Is_Prevented_In_Future_Academic_Year(string[] schoolGroup, string[] staff, string otherStaffName)
        {
            #region Pre-Condition: Create new School Group for future academic year with multiple Staff

            // Add new role into exist staff
            // Log in as a School PersonnelOfficer
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.PersonnelOfficer);
            // Navigate to Manage Staff
            SeleniumHelper.NavigateMenu("Tasks", "Staff", "Staff Records");
            var staffRecordTriplet = new StaffRecordTriplet();
            staffRecordTriplet.SearchCriteria.StaffName = staff[0];
            var staffRecordListItem = staffRecordTriplet.SearchCriteria.Search();
            var staffRecordEdit = staffRecordListItem.FirstOrDefault(p => p.Name.Contains(staff[0]));
            var staffRecordPage = staffRecordEdit.Click<StaffRecordPage>();
            var rowUpdate = staffRecordPage.StaffRoleStandardTable.Rows.FirstOrDefault(p => p.StaffRole.Contains(staff[1]));
            var temp = rowUpdate == null ? staffRecordPage.StaffRoleStandardTable.GetLastRow().StaffRole = staff[1] : null;

            staffRecordPage.Refresh();
            staffRecordPage.SaveStaff();

            // Add role for other staff
            staffRecordTriplet.SearchCriteria.StaffName = otherStaffName;
            staffRecordListItem = staffRecordTriplet.SearchCriteria.Search();
            staffRecordEdit = staffRecordListItem.FirstOrDefault(p => p.Name.Contains(otherStaffName));
            staffRecordPage = staffRecordEdit.Click<StaffRecordPage>();
            rowUpdate = staffRecordPage.StaffRoleStandardTable.Rows.FirstOrDefault(p => p.StaffRole.Contains(staff[1]));
            temp = rowUpdate == null ? staffRecordPage.StaffRoleStandardTable.GetLastRow().StaffRole = staff[1] : null;

            staffRecordPage.Refresh();
            staffRecordPage.SaveStaff();


            SeleniumHelper.Logout();
            //Log in as a School Administrator
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);
            // Navigate to Manage Teaching Groups
            SeleniumHelper.NavigateMenu("Tasks", "School Groups", "Manage Teaching Groups");
            var teachingGroupTriplet = new TeachingGroupTriplet();

            teachingGroupTriplet.SearchCriteria.GroupFullName = schoolGroup[0];
            var listSearchResult = teachingGroupTriplet.SearchCriteria.Search();
            var schoolGroupResult = listSearchResult.FirstOrDefault(t => t.Name.Equals(schoolGroup[0]));
            var teachingGroupDetail = schoolGroupResult == null ? null : schoolGroupResult.Click<TeachingGroupPage>();
            teachingGroupTriplet.Delete();

            teachingGroupDetail = teachingGroupTriplet.ClickCreate();

            teachingGroupDetail.FullName = schoolGroup[0];
            teachingGroupDetail.ShortName = schoolGroup[1];
            teachingGroupDetail.Subject = schoolGroup[2];
            teachingGroupTriplet.ClickSave();

            // Effective date range in the future academic Year
            teachingGroupDetail.Refresh();
            var selectEffectDateRange = teachingGroupDetail.ClickSelectEffectDateRange();
            selectEffectDateRange.AcademicYear = schoolGroup[3];
            selectEffectDateRange.ClickOk();


            // Issue: The function search does not return any supervisors.
            // Search and Add the Supervisor into School Groups
            teachingGroupDetail.Refresh();
            var addSupervisorsDialogTriplet = teachingGroupDetail.OpenAddSupervisorDialog();

            // Click to add staff teacher
            var addSupervisorsDialogDetail = addSupervisorsDialogTriplet.SearchCriteria.Search<AddSupervisorsDialogDetail>();

            Assert.AreNotEqual(0, addSupervisorsDialogDetail.SearchResult.Count(), "There are no result when searching supervisor");

            var staffItem = addSupervisorsDialogDetail.SearchResult.FirstOrDefault(p => p.GetText().Contains(staff[0]));
            staffItem.ClickByJS();

            var otherStaffItem = addSupervisorsDialogDetail.SearchResult.FirstOrDefault(p => p.GetText().Contains(otherStaffName));
            otherStaffItem.ClickByJS();

            addSupervisorsDialogDetail.AddSelectedSupervisor();
            addSupervisorsDialogTriplet.ClickOk();

            teachingGroupDetail.SupervisorTable.WaitUntilRowAppear(p => p.Name.Contains(staff[0]));
            teachingGroupDetail.SupervisorTable.WaitUntilRowAppear(p => p.Name.Contains(otherStaffName));

            #endregion

            #region Steps

            teachingGroupDetail.SupervisorTable[0].Role = staff[1];
            teachingGroupDetail.SupervisorTable[1].Role = staff[1];

            //Click Field 'Main' until a 'Tick' appears for a number of  Supervisors 
            teachingGroupDetail.SupervisorTable[0].Main = true;
            teachingGroupDetail.SupervisorTable[1].Main = true;

            // Amend the Dates for the 2 records to that the Start / End Dates Overlap by 1 day
            var fromDate = teachingGroupDetail.SupervisorTable[0].From;
            teachingGroupDetail.SupervisorTable[1].From = fromDate;
            var toDate = teachingGroupDetail.SupervisorTable[0].To;
            teachingGroupDetail.SupervisorTable[1].To = toDate;
            teachingGroupTriplet.ClickSave();

            //Message returned advising that only 1 Staff Member can be a Main Supervisor on any given day
            // This step is skip because Issue: The function search does not return any supervisors.

            #endregion

            #region End-Condition: Delete school group
            // Remove supervisor row            
            var delRow = teachingGroupDetail.SupervisorTable.Rows.FirstOrDefault(p => p.Name.Contains(otherStaffName));
            delRow.DeleteRow();
            delRow = teachingGroupDetail.SupervisorTable.Rows.FirstOrDefault(p => p.Name.Contains(staff[0]));
            delRow.DeleteRow();
            teachingGroupTriplet.ClickSave();

            teachingGroupTriplet.SearchCriteria.GroupFullName = schoolGroup[0];
            listSearchResult = teachingGroupTriplet.SearchCriteria.Search();
            schoolGroupResult = listSearchResult.FirstOrDefault(t => t.Name.Equals(schoolGroup[0]));
            teachingGroupDetail = listSearchResult == null ? null : schoolGroupResult.Click<TeachingGroupPage>();
            teachingGroupTriplet.Delete();

            #endregion
        }

        /// <summary>
        /// Author: Huy.Vo
        /// Des: Amend a Teaching Group: Ensure a Staff Member can marked as the Main Supervisor on any given day for an effective date range in the  Current Academic Year 
        /// </summary>        
        [WebDriverTest(TimeoutSeconds = 1200, Enabled = false, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { "g1" }, DataProvider = "TC_TG014_Data")]
        public void TC_TG014_Amend_A_Teaching_Group_Ensure_A_Staff_Member_Can_Marked_As_The_Main(
            string groupFullName, string groupShortName, string subject,
            string staffName, string staffRole, string fromDate1, string toDate1,
            string pupilName, string yearGroup, string classes, string fromDate2, string toDate2)
        {


            #region Pre-Condition: Create new School Group for currrent academic year

            //Log in as a School Administrator
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);
            // Navigate to Manage Teaching Groups
            SeleniumHelper.NavigateMenu("Tasks", "School Groups", "Manage Teaching Groups");

            //Search Teaching group to delete if it is existing
            var teachingGroupTriplet = new TeachingGroupTriplet();
            teachingGroupTriplet.SearchCriteria.GroupFullName = groupFullName;
            var listSearchResult = teachingGroupTriplet.SearchCriteria.Search();
            var schoolGroupResult = listSearchResult.FirstOrDefault(t => t.Name.Equals(groupFullName));
            var teachingGroupDetail = schoolGroupResult == null ? null : schoolGroupResult.Click<TeachingGroupPage>();
            teachingGroupTriplet.Delete();

            //Create new Teaching Group
            teachingGroupDetail = teachingGroupTriplet.ClickCreate();
            teachingGroupDetail.FullName = groupFullName;
            teachingGroupDetail.ShortName = groupShortName;
            teachingGroupDetail.Subject = subject;
            teachingGroupTriplet.ClickSave();

            teachingGroupTriplet.ClickSave();

            // Search and Add the Supervisor into School Groups
            teachingGroupDetail.Refresh();
            var addSupervisorsDialogTriplet = teachingGroupDetail.OpenAddSupervisorDialog();
            addSupervisorsDialogTriplet.SearchCriteria.StaffRole = staffRole;

            // Click to add this staff
            var addSupervisorsDialogDetail = addSupervisorsDialogTriplet.SearchCriteria.Search<AddSupervisorsDialogDetail>();
            Assert.AreNotEqual(0, addSupervisorsDialogDetail.SearchResult.Count(), "There are no result when searching supervisor");
            var staffItem = addSupervisorsDialogDetail.SearchResult.FirstOrDefault(p => p.GetText().Contains(staffName));
            staffItem.ClickByJS();
            addSupervisorsDialogDetail.AddSelectedSupervisor();
            addSupervisorsDialogTriplet.ClickOk();

            var supervisorTable = teachingGroupDetail.SupervisorTable;
            supervisorTable[0].Role = staffRole;
            supervisorTable[0].Main = true;
            supervisorTable[0].From = fromDate1;
            supervisorTable[0].To = toDate1;

            teachingGroupTriplet.ClickSave();

            // Search and Add the pupil into School Groups
            teachingGroupDetail.Refresh();
            var addPupilsDialogTriplet = teachingGroupDetail.OpenAddPupilsDialog();

            // Search specific Pupil to add to Teaching Group
            addPupilsDialogTriplet.SearchCriteria.PupilName = pupilName;
            addPupilsDialogTriplet.SearchCriteria.YearGroup = yearGroup;
            addPupilsDialogTriplet.SearchCriteria.Class = classes;
            var addPupilDialogDetail = addPupilsDialogTriplet.SearchCriteria.Search<AddPupilsDialogDetail>();
            addPupilDialogDetail.Refresh();

            // Click to add this Pupil
            var pupilItem = addPupilDialogDetail.Pupils.FirstOrDefault(p => p.GetText().Contains(pupilName));
            pupilItem.ClickByJS();
            addPupilDialogDetail.AddSelectedPupils();
            addPupilsDialogTriplet.ClickOk();

            teachingGroupDetail.PupilsTable.WaitUntilRowAppear(t => t.Name.Contains(pupilName));
            teachingGroupTriplet.ClickSave();

            #endregion

            #region Search and Edit Supervisor

            teachingGroupTriplet.SearchCriteria.GroupFullName = groupFullName;
            listSearchResult = teachingGroupTriplet.SearchCriteria.Search();
            schoolGroupResult = listSearchResult.FirstOrDefault(t => t.Name.Equals(groupFullName));
            teachingGroupDetail = schoolGroupResult == null ? null : schoolGroupResult.Click<TeachingGroupPage>();

            // Click to add this staff 2
            addSupervisorsDialogTriplet = teachingGroupDetail.OpenAddSupervisorDialog();
            addSupervisorsDialogTriplet.SearchCriteria.StaffRole = staffRole;

            // Click to add this staff 2
            addSupervisorsDialogDetail = addSupervisorsDialogTriplet.SearchCriteria.Search<AddSupervisorsDialogDetail>();
            Assert.AreNotEqual(0, addSupervisorsDialogDetail.SearchResult.Count(), "There are no result when searching supervisor 2");
            staffItem = addSupervisorsDialogDetail.SearchResult.FirstOrDefault(p => p.GetText().Contains(staffName));
            staffItem.ClickByJS();
            addSupervisorsDialogDetail.AddSelectedSupervisor();
            addSupervisorsDialogTriplet.ClickOk();

            supervisorTable = teachingGroupDetail.SupervisorTable;
            supervisorTable[1].Role = staffRole;
            supervisorTable[1].Main = true;
            supervisorTable[1].From = fromDate2;
            supervisorTable[1].To = toDate2;

            teachingGroupTriplet.ClickSave();

            //Verify update Supervisor 
            Assert.AreEqual(true, teachingGroupTriplet.DoesMessageSuccessDisplay(), "The success message does not display");

            //Searching and verify for updating main Supervisor for non overlapping Start / End dates
            teachingGroupTriplet.SearchCriteria.GroupFullName = groupFullName;
            listSearchResult = teachingGroupTriplet.SearchCriteria.Search();
            schoolGroupResult = listSearchResult.FirstOrDefault(t => t.Name.Equals(groupFullName));
            teachingGroupDetail = schoolGroupResult == null ? null : schoolGroupResult.Click<TeachingGroupPage>();

            // Verify update main Supervisor for non overlapping Start / End dates
            supervisorTable = teachingGroupDetail.SupervisorTable;
            Assert.AreNotEqual(true, supervisorTable.Rows.FirstOrDefault(p => p.Name.Equals(staffName) && p.Role.Contains(staffRole) &&
             p.From.Equals(fromDate1) && p.To.Equals(toDate1)), "Set main Supervisor is not effected for non overlapping Start / End dates");
            Assert.AreNotEqual(true, supervisorTable.Rows.FirstOrDefault(p => p.Name.Equals(staffName) && p.Role.Contains(staffRole) &&
             p.From.Equals(fromDate2) && p.To.Equals(toDate2)), "Set main Supervisor is not effected for non overlapping Start / End dates");

            #endregion

            #region End-Condition: Delete school group

            // Remove pupil row
            var deletePupilRow = teachingGroupDetail.PupilsTable.Rows.FirstOrDefault(p => p.Name.Contains(pupilName));
            deletePupilRow.DeleteRow();

            // Remove supervisor row
            var deleteSupervisorRow = teachingGroupDetail.SupervisorTable.Rows.FirstOrDefault(p => p.Name.Contains(staffName));
            deleteSupervisorRow.DeleteRow();
            deleteSupervisorRow = teachingGroupDetail.SupervisorTable.Rows.FirstOrDefault(p => p.Name.Contains(staffName));
            deleteSupervisorRow.DeleteRow();

            //Delete Teaching group
            teachingGroupTriplet.ClickSave();
            teachingGroupTriplet.Delete();
            #endregion

        }

        /// <summary>
        /// Author: Ba.Truong
        /// Description: School Administrator, for a Staff member Leaving in the Current Academic Year, 
        ///              ensure they can be selected as a  Supervisor for a Teaching Group until they leave in the Current Academic Year 
        ///              as No End Date is specified on the Effective Date Range
        /// Status: Pending by bug #13: There are no result when searching  any Supervisor with specific role on 'Add Supervisor Dialog'. This issue just happens on Chrome
        ///                    bug #11: Can not select a leaver staff to supervisor grid if we leave the End Date of "Select Effective Date Range" empty
        /// </summary>
        [WebDriverTest(TimeoutSeconds = 2000, Enabled = false, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { "g1" }, DataProvider = "TC_TG019_Data")]
        public void TC_TG019_Verify_That_Creat_Teaching_Group_And_Allocate_Supervisor_Pupil_Current_Academic_Year(string[] newStaffName, string[] newServiceDetail,
                                                                                                                  string[] staffRole, string[] leavingDetails,
                                                                                                                  string[] schoolGroup, string[] effectiveDateRanges)
        {
            #region Pre-Condition: Create a staff and mark this staff as leaver

            //Login and navigate to Staff Records
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.PersonnelOfficer);
            SeleniumHelper.NavigateMenu("Tasks", "Staff", "Staff Records");

            //Create a staff
            var staffRecordTriplet = new StaffRecordTriplet();
            var addNewStaffDialog = staffRecordTriplet.CreateStaff();

            //Fill Add New Staff Dialog                           
            addNewStaffDialog.Forename = newStaffName[0];
            addNewStaffDialog.SurName = newStaffName[1];
            addNewStaffDialog.Gender = newStaffName[2];

            //Fill Service Detail Dialog
            var serviceDetailDialog = addNewStaffDialog.Continue();
            serviceDetailDialog.DateOfArrival = newServiceDetail[0];
            var staffRecordPage = serviceDetailDialog.CreateRecord();

            //Add Staff Role
            staffRecordPage.SelectServiceDetailsTab();
            staffRecordPage.StaffRoleStandardTable[0].StaffRole = staffRole[0];
            staffRecordPage.StaffRoleStandardTable[0].StaffStartDate = staffRole[1];
            staffRecordPage.StaffRoleStandardTable[0].StaffEndDate = String.Empty;

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

            #region Pre-Condition: Create new School Group

            //Log in as a School Administrator
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);

            //Navigate to Manage Teaching Groups
            SeleniumHelper.NavigateMenu("Tasks", "School Groups", "Manage Teaching Groups");

            //Clean the data before adding
            var teachingGroupTriplet = new TeachingGroupTriplet();
            teachingGroupTriplet.SearchCriteria.GroupFullName = schoolGroup[0];
            var listSearchResult = teachingGroupTriplet.SearchCriteria.Search();
            var schoolGroupResult = listSearchResult.FirstOrDefault(t => t.Name.Equals(schoolGroup[0]));
            var teachingGroupDetail = schoolGroupResult == null ? null : schoolGroupResult.Click<TeachingGroupPage>();
            teachingGroupTriplet.Delete();

            #endregion

            #region Steps

            //Click Button 'Create'
            teachingGroupDetail = teachingGroupTriplet.ClickCreate();

            //Input a Title into field 'Full Name'
            teachingGroupDetail.FullName = schoolGroup[0];

            //Tick box 'Visibility'
            teachingGroupDetail.Visibility = true;

            //Input a short description into field 'Short Name'
            teachingGroupDetail.ShortName = schoolGroup[1];

            //Select a value from Drop Down List Box 'Subject'
            teachingGroupDetail.Subject = schoolGroup[2];

            //Enter text into field 'Notes'
            teachingGroupDetail.Notes = schoolGroup[3];

            //Save
            teachingGroupDetail.Save();

            //Click Button 'Select Effective Date Range'
            var selectEffectiveDateRange = teachingGroupDetail.ClickSelectEffectDateRange();

            //Select Start Dates prior to the Staff Leavers End Date - leave the End Date Blank
            selectEffectiveDateRange.AcademicYear = effectiveDateRanges[0];
            selectEffectiveDateRange.From = effectiveDateRanges[1];
            selectEffectiveDateRange.To = effectiveDateRanges[2];
            selectEffectiveDateRange.ClickOk();

            //Click Button 'Add' Supervisors
            teachingGroupDetail.Refresh();
            var suppervisorTriplet = teachingGroupDetail.OpenAddSupervisorDialog();

            //Click Button 'Search' in Pop up Window 'Select Supervisor'
            suppervisorTriplet.SearchCriteria.LegalSurName = newStaffName[0];
            var addSupervisorsDetail = suppervisorTriplet.SearchCriteria.Search<AddSupervisorsDialogDetail>();
            var staffLeaver = addSupervisorsDetail.SearchResult.FirstOrDefault(p => p.GetText().Contains(newStaffName[0]));

            //Confirm that the Staff Member with a Leaving date in this Academic Year is available to Select
            Assert.AreNotEqual(null, staffLeaver, "Staff Member with a Leaving date in this Academic Year is not available to Select");

            //Select above staff
            staffLeaver.ClickByJS();
            addSupervisorsDetail.AddSelectedSupervisor();
            teachingGroupDetail = addSupervisorsDetail.ClickOK<TeachingGroupPage>();

            //Add role for supervisor
            teachingGroupDetail.SupervisorTable[0].Role = staffRole[0];

            //Save teaching group
            teachingGroupDetail.Save();

            //Confirm teaching group was created successfully
            teachingGroupTriplet = new TeachingGroupTriplet();
            teachingGroupTriplet.SearchCriteria.GroupFullName = schoolGroup[0];
            listSearchResult = teachingGroupTriplet.SearchCriteria.Search();
            schoolGroupResult = listSearchResult.FirstOrDefault(t => t.Name.Equals(schoolGroup[0]));
            Assert.AreNotEqual(null, schoolGroupResult, "Teaching group is not saved");

            #endregion

            #region End-Condition

            //Delete record in Supervisors Grid
            teachingGroupDetail = schoolGroupResult.Click<TeachingGroupPage>();
            teachingGroupDetail.SupervisorTable[0].DeleteRow();
            teachingGroupDetail.Save();

            //Delete teaching group
            teachingGroupTriplet.Delete();

            //Delete staff
            SeleniumHelper.Logout();
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.PersonnelOfficer);
            SeleniumHelper.NavigateMenu("Tasks", "Staff", "Delete Staff Record");

            var deleteStaffRecordTriplet = new DeleteStaffRecordTriplet();
            deleteStaffRecordTriplet.SearchCriteria.StaffName = String.Format("{0}, {1}", newStaffName[0], newStaffName[1]);
            deleteStaffRecordTriplet.SearchCriteria.IsLeaver = true;
            var staffTiles = deleteStaffRecordTriplet.SearchCriteria.Search();
            var staffRecord = staffTiles.FirstOrDefault(x => x.Name.Contains(newStaffName[0])).Click<StaffRecordPage>();
            staffRecord.DeleteStaff();

            #endregion

        }

        /// <summary>
        /// Author: Ba.Truong
        /// Description: School Administrator,  for a Staff member Leaving in the Current Academic Year, 
        ///              ensure they can still be selected as a  Supervisor for a Teaching Group before they leave in the Current Academic Year
        /// Status: Pending by bug #13: There are no result when searching  any Supervisor with specific role on 'Add Supervisor Dialog'
        /// </summary>
        [WebDriverTest(TimeoutSeconds = 2000, Enabled = false, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { "g1" }, DataProvider = "TC_TG020_Data")]
        public void TC_TG020_Verify_That_Creat_Teaching_Group_And_Allocate_Supervisor_Pupil_Current_Academic_Year(string[] newStaffName, string[] newServiceDetail,
                                                                                                                  string[] staffRole, string[] leavingDetails,
                                                                                                                  string[] schoolGroup, string[] effectiveDateRanges)
        {
            #region Pre-Condition: Create a staff and mark this staff as leaver

            //Login as School Personnel Officer
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.PersonnelOfficer);
            SeleniumHelper.NavigateMenu("Tasks", "Staff", "Staff Records");

            //Create a staff
            var staffRecordTriplet = new StaffRecordTriplet();
            var addNewStaffDialog = staffRecordTriplet.CreateStaff();

            //Fill Add New Staff Dialog                           
            addNewStaffDialog.Forename = newStaffName[0];
            addNewStaffDialog.SurName = newStaffName[1];
            addNewStaffDialog.Gender = newStaffName[2];

            //Fill Service Detail Dialog
            var serviceDetailDialog = addNewStaffDialog.Continue();
            serviceDetailDialog.DateOfArrival = newServiceDetail[0];
            var staffRecordPage = serviceDetailDialog.CreateRecord();

            //Add Staff Role
            staffRecordPage.SelectServiceDetailsTab();
            staffRecordPage.StaffRoleStandardTable[0].StaffRole = staffRole[0];
            staffRecordPage.StaffRoleStandardTable[0].StaffStartDate = staffRole[1];
            staffRecordPage.StaffRoleStandardTable[0].StaffEndDate = String.Empty;

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

            #region Pre-Condition: Create new School Group

            //Log in as a School Administrator
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);

            //Navigate to Manage Teaching Groups
            SeleniumHelper.NavigateMenu("Tasks", "School Groups", "Manage Teaching Groups");

            //Clean the data before adding
            var teachingGroupTriplet = new TeachingGroupTriplet();
            teachingGroupTriplet.SearchCriteria.GroupFullName = schoolGroup[0];
            var listSearchResult = teachingGroupTriplet.SearchCriteria.Search();
            var schoolGroupResult = listSearchResult.FirstOrDefault(t => t.Name.Equals(schoolGroup[0]));
            var teachingGroupDetail = schoolGroupResult == null ? null : schoolGroupResult.Click<TeachingGroupPage>();
            teachingGroupTriplet.Delete();

            #endregion

            #region Steps

            //Click Button 'Create'
            teachingGroupDetail = teachingGroupTriplet.ClickCreate();

            //Input a Title into field 'Full Name'
            teachingGroupDetail.FullName = schoolGroup[0];

            //Tick box 'Visibility'
            teachingGroupDetail.Visibility = true;

            //Input a short description into field 'Short Name'
            teachingGroupDetail.ShortName = schoolGroup[1];

            //Select a value from Drop Down List Box 'Subject'
            teachingGroupDetail.Subject = schoolGroup[2];

            //Enter text into field 'Notes'
            teachingGroupDetail.Notes = schoolGroup[3];

            //Save
            teachingGroupDetail.Save();

            //Click Button 'Select Effective Date Range'
            var selectEffectiveDateRange = teachingGroupDetail.ClickSelectEffectDateRange();

            //Select Start Dates prior to the Staff Leavers End Date - leave the End Date Blank
            selectEffectiveDateRange.AcademicYear = effectiveDateRanges[0];
            selectEffectiveDateRange.From = effectiveDateRanges[1];
            selectEffectiveDateRange.To = effectiveDateRanges[2];
            selectEffectiveDateRange.ClickOk();

            //Click Button 'Add' Supervisors
            teachingGroupDetail.Refresh();
            var suppervisorTriplet = teachingGroupDetail.OpenAddSupervisorDialog();

            //Click Button 'Search' in Pop up Window 'Select Supervisor'
            suppervisorTriplet.SearchCriteria.LegalSurName = newStaffName[0];
            var addSupervisorsDetail = suppervisorTriplet.SearchCriteria.Search<AddSupervisorsDialogDetail>();
            var staffLeaver = addSupervisorsDetail.SearchResult.FirstOrDefault(p => p.GetText().Contains(newStaffName[0]));

            //Confirm that the Staff Member with a Leaving date in this Academic Year is available to Select
            Assert.AreNotEqual(null, staffLeaver, "Staff Member with a Leaving date in this Academic Year is not available to Select");

            //Select above staff
            staffLeaver.ClickByJS();
            addSupervisorsDetail.AddSelectedSupervisor();
            teachingGroupDetail = addSupervisorsDetail.ClickOK<TeachingGroupPage>();

            //Add role for supervisor
            teachingGroupDetail.SupervisorTable[0].Role = staffRole[0];

            //Save teaching group
            teachingGroupDetail.Save();

            //Confirm teaching group was created successfully
            teachingGroupTriplet = new TeachingGroupTriplet();
            teachingGroupTriplet.SearchCriteria.GroupFullName = schoolGroup[0];
            listSearchResult = teachingGroupTriplet.SearchCriteria.Search();
            schoolGroupResult = listSearchResult.FirstOrDefault(t => t.Name.Equals(schoolGroup[0]));
            Assert.AreNotEqual(null, schoolGroupResult, "Teaching group is not saved");

            #endregion

            #region End-Condition

            //Delete record in Supervisors Grid
            teachingGroupDetail = schoolGroupResult.Click<TeachingGroupPage>();
            teachingGroupDetail.SupervisorTable[0].DeleteRow();
            teachingGroupDetail.Save();

            //Delete teaching group
            teachingGroupTriplet.Delete();

            //Delete staff
            SeleniumHelper.Logout();
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.PersonnelOfficer);
            SeleniumHelper.NavigateMenu("Tasks", "Staff", "Delete Staff Record");

            var deleteStaffRecordTriplet = new DeleteStaffRecordTriplet();
            deleteStaffRecordTriplet.SearchCriteria.StaffName = String.Format("{0}, {1}", newStaffName[0], newStaffName[1]);
            deleteStaffRecordTriplet.SearchCriteria.IsLeaver = true;
            var staffTiles = deleteStaffRecordTriplet.SearchCriteria.Search();
            var staffRecord = staffTiles.FirstOrDefault(x => x.Name.Contains(newStaffName[0])).Click<StaffRecordPage>();
            staffRecord.DeleteStaff();

            #endregion

        }

        /// <summary>
        /// Author: Huy.Vo
        /// Des: Amend a Teaching Group: Ensure a Learner cannot be input twice for the same role with overlapping date ranges for the Current academic Year 
        /// Issues: The function search does not return any supervisors.
        /// </summary>        
        [WebDriverTest(TimeoutSeconds = 1200, Enabled = false, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { "g1" }, DataProvider = "TC_TG021_Data")]
        public void TC_TG021_Amend_A_Teaching_Learner_Twice_The_Same_Role_With_Overlapping_Date_Ranges_Current_AcademicYear(
            string groupFullName, string groupShortName, string subject,
            string staffName, string staffRole,
            string pupilName, string yearGroup, string classes,
            string fromDate, string toDate)
        {

            #region Pre-Condition: Create new School Group for currrent academic year

            //Log in as a School Administrator
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);

            // Navigate to Manage Teaching Groups
            SeleniumHelper.NavigateMenu("Tasks", "School Groups", "Manage Teaching Groups");

            //Search Teaching group to delete if it is existing
            var teachingGroupTriplet = new TeachingGroupTriplet();
            teachingGroupTriplet.SearchCriteria.GroupFullName = groupFullName;
            var listSearchResult = teachingGroupTriplet.SearchCriteria.Search();
            var schoolGroupResult = listSearchResult.FirstOrDefault(t => t.Name.Equals(groupFullName));
            var teachingGroupDetail = schoolGroupResult == null ? null : schoolGroupResult.Click<TeachingGroupPage>();
            teachingGroupTriplet.Delete();

            //Create new Teaching Group
            teachingGroupDetail = teachingGroupTriplet.ClickCreate();
            teachingGroupDetail.FullName = groupFullName;
            teachingGroupDetail.ShortName = groupShortName;
            teachingGroupDetail.Subject = subject;
            teachingGroupTriplet.ClickSave();

            // Search and Add the Supervisor into School Groups
            teachingGroupDetail.Refresh();
            var addSupervisorsDialogTriplet = teachingGroupDetail.OpenAddSupervisorDialog();
            addSupervisorsDialogTriplet.SearchCriteria.StaffRole = staffRole;

            // Click to add this Staff
            var addSupervisorsDialogDetail = addSupervisorsDialogTriplet.SearchCriteria.Search<AddSupervisorsDialogDetail>();
            Assert.AreNotEqual(0, addSupervisorsDialogDetail.SearchResult.Count(), "There are no result when searching supervisor");
            var staffItem = addSupervisorsDialogDetail.SearchResult.FirstOrDefault(p => p.GetText().Contains(staffName));
            staffItem.ClickByJS();
            addSupervisorsDialogDetail.AddSelectedSupervisor();
            addSupervisorsDialogTriplet.ClickOk();

            var supervisorTable = teachingGroupDetail.SupervisorTable;
            supervisorTable[0].Role = staffRole;

            teachingGroupTriplet.ClickSave();

            #endregion  Pre-Condition: Create new School Group for currrent academic year

            #region TEST
            // Search and Add the pupil 1 into School Groups
            teachingGroupDetail.Refresh();
            var addPupilsDialogTriplet = teachingGroupDetail.OpenAddPupilsDialog();

            // Search specific Pupil to add to Teaching Group
            addPupilsDialogTriplet.SearchCriteria.PupilName = pupilName;
            addPupilsDialogTriplet.SearchCriteria.YearGroup = yearGroup;
            addPupilsDialogTriplet.SearchCriteria.Class = classes;
            var addPupilDialogDetail = addPupilsDialogTriplet.SearchCriteria.Search<AddPupilsDialogDetail>();
            addPupilDialogDetail.Refresh();

            // Click to add this Pupil 
            var pupilItem = addPupilDialogDetail.Pupils.FirstOrDefault(p => p.GetText().Contains(pupilName));
            pupilItem.ClickByJS();
            addPupilDialogDetail.AddSelectedPupils();
            addPupilsDialogTriplet.ClickOk();

            teachingGroupDetail.Refresh();
            // Add from date and to date for pupil 1
            var pupilTable = teachingGroupDetail.PupilsTable;
            pupilTable[0].From = fromDate;
            pupilTable[0].To = toDate;

            teachingGroupDetail.PupilsTable.WaitUntilRowAppear(t => t.Name.Contains(pupilName));
            teachingGroupTriplet.ClickSave();

            //Searching and Update Pupil the Dates for the 2 records to that the Start / End Dates overlap by 1 day
            teachingGroupTriplet.SearchCriteria.GroupFullName = groupFullName;
            listSearchResult = teachingGroupTriplet.SearchCriteria.Search();
            schoolGroupResult = listSearchResult.FirstOrDefault(t => t.Name.Equals(groupFullName));
            teachingGroupDetail = schoolGroupResult == null ? null : schoolGroupResult.Click<TeachingGroupPage>();

            addPupilsDialogTriplet = teachingGroupDetail.OpenAddPupilsDialog();

            // Search specific Pupil to add to Teaching Group
            addPupilsDialogTriplet.SearchCriteria.PupilName = pupilName;
            addPupilsDialogTriplet.SearchCriteria.YearGroup = yearGroup;
            addPupilsDialogTriplet.SearchCriteria.Class = classes;
            addPupilDialogDetail = addPupilsDialogTriplet.SearchCriteria.Search<AddPupilsDialogDetail>();
            addPupilDialogDetail.Refresh();

            // Click to add this Pupil
            pupilItem = addPupilDialogDetail.Pupils.FirstOrDefault(p => p.GetText().Contains(pupilName));
            pupilItem.ClickByJS();
            addPupilDialogDetail.AddSelectedPupils();
            addPupilsDialogTriplet.ClickOk();

            pupilTable = teachingGroupDetail.PupilsTable;
            pupilTable[1].From = fromDate;
            pupilTable[1].To = toDate;

            teachingGroupTriplet.ClickSave();

            //Verify a error massage is displayed 
            Assert.AreEqual(true, teachingGroupTriplet.IsErrorMessageDisplay(), "The error message is not displayed");

            #endregion TEST

            #region End-Condition: Delete school group

            // Remove pupil row before deleting teaching group
            var deletePupilRow = teachingGroupDetail.PupilsTable.Rows.FirstOrDefault(p => p.Name.Contains(pupilName));
            deletePupilRow.DeleteRow();

            deletePupilRow = teachingGroupDetail.PupilsTable.Rows.FirstOrDefault(p => p.Name.Contains(pupilName));
            deletePupilRow.DeleteRow();

            // Remove supervisor row
            var deleteSupervisorRow = teachingGroupDetail.SupervisorTable.Rows.FirstOrDefault(p => p.Name.Contains(staffName));
            deleteSupervisorRow.DeleteRow();
            teachingGroupTriplet.ClickSave();
            //Delete Teaching group
            teachingGroupTriplet.Delete();
            #endregion

        }

        /// <summary>
        /// Author: Huy.Vo
        /// Des: Amend a Teaching Group: Ensure the same Learner can be input twice for the same role with non overlapping date ranges for the Current academic Year 
        /// Issue: The function search does not return any supervisors.
        /// </summary>        
        [WebDriverTest(TimeoutSeconds = 800, Enabled = false, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { "g1" }, DataProvider = "TC_TG022_Data")]
        public void TC_TG022_Amend_A_Teaching_Same_Learner_Input_Twice_The_Same_Role_For_The_Current_Academic_Year(
                        string groupFullName, string groupShortName, string subject,
                        string futureAcademicYear, string fromDate, string toDate,
                        string staffCode, string sureName, string staffRole,
                        string pupilName, string yearGroup, string classes,
                        string fromDate1, string toDate1,
                        string fromDate2, string toDate2)
        {


            #region Pre-Condition: Create new School Group for currrent academic year

            //Log in as a School Administrator
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);

            // Navigate to Manage Teaching Groups
            SeleniumHelper.NavigateMenu("Tasks", "School Groups", "Manage Teaching Groups");

            //Search Teaching group to delete if it is existing
            var teachingGroupTriplet = new TeachingGroupTriplet();
            teachingGroupTriplet.SearchCriteria.GroupFullName = groupFullName;
            var listSearchResult = teachingGroupTriplet.SearchCriteria.Search();
            var schoolGroupResult = listSearchResult.FirstOrDefault(t => t.Name.Equals(groupFullName));
            var teachingGroupDetail = schoolGroupResult == null ? null : schoolGroupResult.Click<TeachingGroupPage>();
            teachingGroupTriplet.Delete();

            //Create new Teaching Group
            teachingGroupDetail = teachingGroupTriplet.ClickCreate();
            teachingGroupDetail.FullName = groupFullName;
            teachingGroupDetail.ShortName = groupShortName;
            teachingGroupDetail.Subject = subject;
            teachingGroupTriplet.ClickSave();

            // Effective date range in the  current academic Year
            teachingGroupDetail.Refresh();
            var selectEffectDateRange = teachingGroupDetail.ClickSelectEffectDateRange();
            selectEffectDateRange.AcademicYear = futureAcademicYear;
            selectEffectDateRange.ClickOk();
            teachingGroupTriplet.ClickSave();

            // Search and Add the pupil 1 into School Groups
            teachingGroupDetail.Refresh();
            var addPupilsDialogTriplet = teachingGroupDetail.OpenAddPupilsDialog();

            // Search specific Pupil to add to Teaching Group
            addPupilsDialogTriplet.SearchCriteria.PupilName = pupilName;
            addPupilsDialogTriplet.SearchCriteria.YearGroup = yearGroup;
            addPupilsDialogTriplet.SearchCriteria.Class = classes;
            var addPupilDialogDetail = addPupilsDialogTriplet.SearchCriteria.Search<AddPupilsDialogDetail>();
            addPupilDialogDetail.Refresh();

            // Click to add this Pupil 
            var pupilItem = addPupilDialogDetail.Pupils.FirstOrDefault(p => p.GetText().Contains(pupilName));
            pupilItem.ClickByJS();
            addPupilDialogDetail.AddSelectedPupils();
            addPupilsDialogTriplet.ClickOk();

            // Add from date and to date for pupil 1
            teachingGroupDetail.Refresh();

            var pupilTable = teachingGroupDetail.PupilsTable;
            pupilTable[0].From = fromDate1;
            pupilTable[0].To = toDate1;

            //teachingGroupDetail.PupilsTable.WaitForNewRowAppear();
            teachingGroupTriplet.ClickSave();

            #endregion PRO-CONDIONS
            #region STEPS
            //Searching and Update Pupil the Dates for the 2 records to that the Start / End Dates overlap by 1 day
            teachingGroupTriplet.SearchCriteria.GroupFullName = groupFullName;
            listSearchResult = teachingGroupTriplet.SearchCriteria.Search();
            schoolGroupResult = listSearchResult.FirstOrDefault(t => t.Name.Equals(groupFullName));
            teachingGroupDetail = schoolGroupResult == null ? null : schoolGroupResult.Click<TeachingGroupPage>();

            addPupilsDialogTriplet = teachingGroupDetail.OpenAddPupilsDialog();

            // Search specific Pupil to add to Teaching Group
            addPupilsDialogTriplet.SearchCriteria.PupilName = pupilName;
            addPupilsDialogTriplet.SearchCriteria.YearGroup = yearGroup;
            addPupilsDialogTriplet.SearchCriteria.Class = classes;
            addPupilDialogDetail = addPupilsDialogTriplet.SearchCriteria.Search<AddPupilsDialogDetail>();
            addPupilDialogDetail.Refresh();

            // Click to add this Pupil
            pupilItem = addPupilDialogDetail.Pupils.FirstOrDefault(p => p.GetText().Contains(pupilName));
            pupilItem.ClickByJS();
            addPupilDialogDetail.AddSelectedPupils();
            addPupilsDialogTriplet.ClickOk();

            pupilTable = teachingGroupDetail.PupilsTable;
            pupilTable[1].From = fromDate2;
            pupilTable[1].To = toDate2;

            teachingGroupTriplet.ClickSave();

            //Verify a error massage is displayed 
            Assert.AreEqual(true, teachingGroupTriplet.DoesMessageSuccessDisplay(), "The error message is not displayed");

            // Search to verify 
            teachingGroupTriplet.SearchCriteria.GroupFullName = groupFullName;
            listSearchResult = teachingGroupTriplet.SearchCriteria.Search();
            schoolGroupResult = listSearchResult.FirstOrDefault(t => t.Name.Equals(groupFullName));
            teachingGroupDetail = listSearchResult == null ? null : schoolGroupResult.Click<TeachingGroupPage>();

            pupilTable = teachingGroupDetail.PupilsTable;

            Assert.AreNotEqual(null, pupilTable.Rows.SingleOrDefault(p => p.Name.Contains(pupilName) && p.From.Equals(fromDate1)
                && p.To.Equals(toDate1)), "The Pupils with non overlapping date ranges for a current academic Year is not effected");
            Assert.AreNotEqual(null, pupilTable.Rows.SingleOrDefault(p => p.Name.Contains(pupilName) && p.From.Equals(fromDate2) && p.To.Equals(toDate2)),
                "The Pupils with non overlapping date ranges for a current academic Year is  not effected");

            #endregion STEPS

            #region POS-CONDIONS

            //Delete Teaching group
            // Remove pupil row before deleting teaching group
            var deletePupilRow = teachingGroupDetail.PupilsTable.Rows.FirstOrDefault(p => p.Name.Contains(pupilName));
            deletePupilRow.DeleteRow();
            pupilTable.Refresh();
            deletePupilRow = teachingGroupDetail.PupilsTable.Rows.FirstOrDefault(p => p.Name.Contains(pupilName));
            deletePupilRow.DeleteRow();
            pupilTable.Refresh();

            // Remove supervisor row
            // Skip by issue
            //var deleteSupervisorRow = teachingGroupDetail.SupervisorTable.Rows.FirstOrDefault(p => p.Name.Contains(sureName1));
            //deleteSupervisorRow.DeleteRow();
            //deleteSupervisorRow = teachingGroupDetail.SupervisorTable.Rows.FirstOrDefault(p => p.Name.Contains(sureName2));
            //deleteSupervisorRow.DeleteRow();

            teachingGroupTriplet.ClickSave();
            teachingGroupTriplet.Delete();

            #endregion POS-CONDIONS

        }

        /// <summary>
        /// Author: Hieu Pham
        /// Description : Amend a Teaching Group: For a Pupil Leaving in the Current Academic Year, ensure they can be selected as a  Learner for a Teaching Group until they leave in the Current Academic Year as No End Date is specified on the Effective Date Range
        /// STATUS : PASS
        /// </summary>        
        [WebDriverTest(TimeoutSeconds = 2000, Enabled = false, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { "g1" }, DataProvider = "TC_TG028_Data")]
        public void TC_TG028_Amend_Teaching_Group_For_Pupil_Leaving(string[] teachingGroup, string[] pupil)
        {
            #region Pre-Condition: Add a new pupil

            //Log in as a School Administrator
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

            // Navigate to Manage Teaching Groups to delete old record
            SeleniumHelper.NavigateMenu("Tasks", "School Groups", "Manage Teaching Groups");
            var teachingGroupTriplet = new TeachingGroupTriplet();

            // Search old record if existed
            teachingGroupTriplet.SearchCriteria.GroupFullName = teachingGroup[0];
            var listSearchResult = teachingGroupTriplet.SearchCriteria.Search();
            var schoolGroupResult = listSearchResult.FirstOrDefault(t => t.Name.Equals(teachingGroup[0]));
            var teachingGroupDetail = schoolGroupResult == null ? null : schoolGroupResult.Click<TeachingGroupPage>();
            teachingGroupTriplet.Delete();

            #endregion

            #region Steps

            SeleniumHelper.NavigateMenu("Tasks", "School Groups", "Manage Teaching Groups");
            // Click Search to see all records
            teachingGroupTriplet.Refresh();
            teachingGroupTriplet = new TeachingGroupTriplet();
            teachingGroupTriplet.SearchCriteria.GroupFullName = String.Empty;
            listSearchResult = teachingGroupTriplet.SearchCriteria.Search();

            // Add new teaching group
            teachingGroupDetail = teachingGroupTriplet.ClickCreate();

            // Enter values
            teachingGroupDetail.FullName = teachingGroup[0];
            teachingGroupDetail.ShortName = teachingGroup[1];
            teachingGroupDetail.Subject = teachingGroup[2];
            teachingGroupDetail.Save();

            // Save values
            teachingGroupDetail.Refresh();
            teachingGroupDetail.Save();

            // Effective date range in the future academic Year
            teachingGroupDetail.Refresh();
            var selectEffectDateRange = teachingGroupDetail.ClickSelectEffectDateRange();
            selectEffectDateRange.From = teachingGroup[3];
            selectEffectDateRange.To = String.Empty;
            selectEffectDateRange.ClickOk();

            // Search and Add the pupil into Teaching Groups
            teachingGroupDetail.Refresh();
            teachingGroupDetail.ScrollToPupil();
            var addPupilsDialogTriplet = teachingGroupDetail.OpenAddPupilsDialog();

            // Search all pupil
            addPupilsDialogTriplet.SearchCriteria.PupilName = String.Format("{0}, {1}", pupil[1], pupil[0]);
            addPupilsDialogTriplet.SearchCriteria.IsCurrent = true;
            addPupilsDialogTriplet.SearchCriteria.IsLeaver = true;
            var addPupilDialogDetail = addPupilsDialogTriplet.SearchCriteria.Search<AddPupilsDialogDetail>();
            var pupilResult = addPupilDialogDetail.Pupils.FirstOrDefault(x => x.GetText().Trim().Equals(String.Format("{0}, {1}", pupil[1], pupil[0])));

            // VP : Pupil with a Leaving date in this Academic Year is available
            Assert.AreNotEqual(null, pupilResult, "Pupil with a leaving date is not available");

            // Select pupil
            pupilResult.Click();
            addPupilDialogDetail.AddSelectedPupils();

            // Click OK
            addPupilsDialogTriplet.Refresh();
            addPupilsDialogTriplet.ClickOk();

            // Save value
            teachingGroupDetail.Refresh();
            teachingGroupDetail.Save();

            // Search teaching group again
            teachingGroupTriplet.Refresh();
            teachingGroupTriplet.SearchCriteria.GroupFullName = teachingGroup[0];
            teachingGroupDetail = teachingGroupTriplet.SearchCriteria.Search().FirstOrDefault(x => x.Name.Equals(teachingGroup[0])).Click<TeachingGroupPage>();

            // Effective date range in the future academic Year
            selectEffectDateRange = teachingGroupDetail.ClickSelectEffectDateRange();
            selectEffectDateRange.From = teachingGroup[3];
            selectEffectDateRange.To = String.Empty;
            selectEffectDateRange.ClickOk();

            // VP : Pupil with a Leaving date in this Academic Year is available to Select
            teachingGroupDetail.Refresh();
            teachingGroupDetail.ScrollToPupil();
            var pupilTable = teachingGroupDetail.PupilsTable;
            var pupilRow = pupilTable.Rows.FirstOrDefault(x => x.Name.Equals(String.Format("{0}, {1}", pupil[1], pupil[0])));
            Assert.AreNotEqual(null, pupilRow, "Pupil with a Leaving date in this Academic Year is not available to Select");

            #endregion

            #region Post-Condition: Delete school group

            // Remove pupil row
            pupilRow.DeleteRow();

            // Save record
            teachingGroupTriplet.ClickSave();

            // Delete record
            teachingGroupTriplet.Refresh();
            teachingGroupTriplet.SearchCriteria.GroupFullName = teachingGroup[0];
            listSearchResult = teachingGroupTriplet.SearchCriteria.Search();
            schoolGroupResult = listSearchResult.FirstOrDefault(t => t.Name.Equals(teachingGroup[0]));
            teachingGroupDetail = listSearchResult == null ? null : schoolGroupResult.Click<TeachingGroupPage>();
            teachingGroupTriplet.Delete();

            // Delete pupil if existed
            SeleniumHelper.NavigateMenu("Tasks", "Pupils", "Delete Pupil");
            deletePupilRecordTriplet = new DeletePupilRecordTriplet();
            deletePupilRecordTriplet.SearchCriteria.PupilName = String.Format("{0}, {1}", pupil[1], pupil[0]);
            deletePupilRecordTriplet.SearchCriteria.IsLeaver = true;
            resultPupils = deletePupilRecordTriplet.SearchCriteria.Search();
            pupilSearchTile = resultPupils.SingleOrDefault(t => t.Name.Equals(String.Format("{0}, {1}", pupil[1], pupil[0])));
            deletePupilRecord = pupilSearchTile == null ? new DeletePupilRecordPage() : pupilSearchTile.Click<DeletePupilRecordPage>();
            deletePupilRecord.Delete();

            #endregion

        }

        #region DATA

        public List<object[]> TC_TG001_Data()
        {
            string random = SeleniumHelper.GenerateRandomString(6);
            var res = new List<Object[]>
            {                
                new object[] {
                    new string[]{ "Teaching Group "+random,random, "English"}
                },
            };
            return res;
        }

        public List<object[]> TC_TG002_Data()
        {
            string random = SeleniumHelper.GenerateRandomString(6);
            var res = new List<Object[]>
            {                
                new object[] {
                    new string[]{ "Teaching Group "+random,random, "English"},
                    new string[]{"Lawson","Teacher: Classroom Teacher"}
                },
            };
            return res;
        }

        public List<object[]> TC_TG003_Data()
        {
            string random = SeleniumHelper.GenerateRandomString(6);
            var res = new List<Object[]>
            {                
                new object[] {
                    new string[]{ "Teaching Group "+random,random, "English"},
                    new string[]{"Aaron, Liz","Year 1", "1A"}
                },
            };
            return res;
        }

        public List<object[]> TC_TG004_Data()
        {
            string random = SeleniumHelper.GenerateRandomString(6);
            string academicYear = String.Format("Academic Year {0}/{1}", DateTime.Now.Year.ToString(), (DateTime.Now.Year + 1).ToString());

            var res = new List<Object[]>
            {                
                new object[] {
                    new string[]{ "Teaching Group "+random,random, "English",academicYear},
                    new string[]{"Aaron, Liz","Year 1", "1A"},
                    new string[]{"Lawson","Teacher: Classroom Teacher"}
                },
            };
            return res;
        }

        public List<object[]> TC_TG005_Data()
        {
            string random = SeleniumHelper.GenerateRandomString(6);
            string academicYear = String.Format("Academic Year {0}/{1}", DateTime.Now.Year.ToString(), (DateTime.Now.Year + 1).ToString());
            string randomUpdate = SeleniumHelper.GenerateRandomString(6);

            var res = new List<Object[]>
            {                
                new object[] {
                    new string[]{ "Teaching Group "+random,random, "English",academicYear},
                    new string[]{"Aaron, Liz","Year 1", "1A"},
                    new string[]{"Lawson","Teacher: Classroom Teacher"},
                    //School Group Update
                    new string[]{ "Teaching Group "+randomUpdate,"Notes "+randomUpdate,randomUpdate, "Cross Curricular",academicYear},
                    // Pupil Update
                    new string[]{"Akeman, Richard","Year 1", "1A"},
                    // Staff Update
                    new string[]{"Watkins","Learning Mentor"}
                },
            };
            return res;
        }

        public List<object[]> TC_TG006_Data()
        {
            string random = SeleniumHelper.GenerateRandomString(6);
            string academicYear = String.Format("Academic Year {0}/{1}", (DateTime.Now.Year).ToString(), (DateTime.Now.Year+1).ToString());

            string sysFormat = CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern;

            var theFromDate = new DateTime(DateTime.Today.Year + 1, 8, 1);
            var theToDate = new DateTime(DateTime.Today.Year + 2, 7, 31);

            var res = new List<Object[]>
            {                
                new object[] {
                    new string[]{ "Teaching Group "+random,random, "English",academicYear},
                    new string[]{"Aaron, Liz","Year 1", "1A"},
                    new string[]{"Lawson","Teacher: Classroom Teacher"}                    
                },
            };
            return res;
        }

        public List<object[]> TC_TG007_Data()
        {
            string pattern = "M/d/yyyy";
            string fullName = String.Format("{0}_{1}", "Avn", SeleniumHelper.GenerateRandomString(8));
            string shortName = String.Format("{0}{1}", "A", SeleniumHelper.GenerateRandomString(4));
            string note = String.Format("{0}_{1}", "Note", SeleniumHelper.GenerateRandomString(20));
            string pupilName = "Akeman, Richard";
            string editFullName = String.Format("{0}_{1}_{2}", "Avn", "Edit", SeleniumHelper.GenerateRandomString(8));
            string editShortName = String.Format("{0}{1}{2}", "A", "Edit", SeleniumHelper.GenerateRandomString(4));
            string editNote = String.Format("{0}_{1}{2}", "Note", "Edit", SeleniumHelper.GenerateRandomString(20));
            string newPupilName = "Abdullah, Tamwar";
            string futureAcademicYear = SeleniumHelper.GetAcademicYear(DateTime.Now.AddYears(1));
            string foreName1 = String.Format("{0}_{1}{2}", SeleniumHelper.GenerateRandomString(7), SeleniumHelper.GenerateRandomNumberUsingDateTime(), 1);
            string surName1 = String.Format("{0}_{1}{2}", "Avn", SeleniumHelper.GenerateRandomString(8), 1);
            string foreName2 = String.Format("{0}_{1}{2}", SeleniumHelper.GenerateRandomString(7), SeleniumHelper.GenerateRandomNumberUsingDateTime(), 2);
            string surName2 = String.Format("{0}_{1}{2}", "Avn", SeleniumHelper.GenerateRandomString(8), 2);
            string dateOfBirth = DateTime.ParseExact("1/16/1991", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            string dateArrival = DateTime.ParseExact("1/1/2010", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            var res = new List<Object[]>
            {                
                new object[] 
                {
                    new string[]{foreName1, surName1, "Male", dateOfBirth, dateArrival },
                    new string[]{foreName2, surName2, "Male", dateOfBirth, dateArrival },
                    fullName, shortName, "English", note, 
                    "Teacher: Classroom Teacher", String.Format("{0}, {1}", surName1, foreName1), pupilName,
                    editFullName, editShortName, "Mathematics", editNote, 
                    futureAcademicYear, String.Format("{0}, {1}", surName2, foreName2), newPupilName
                }
            };
            return res;
        }

        public List<object[]> TC_TG008_Data()
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
                    fullName, shortName, "English", note,
                    "Lawson, Ms Regan", "Teacher: Classroom Teacher", startDate, endDate
                }
            };
            return res;
        }

        public List<object[]> TC_TG009_Data()
        {
            string pattern = "M/d/yyyy";
            string random = SeleniumHelper.GenerateRandomString(6);
            string academicYear = String.Format("Academic Year {0}/{1}", (DateTime.Now.Year).ToString(), (DateTime.Now.Year + 1).ToString());

            string sysFormat = CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern;

            var theFromDate = new DateTime(DateTime.Today.Year, 8, 1);
            var theToDate = new DateTime(DateTime.Today.Year + 1, 7, 31);
            string academicLength = String.Format("{0} - {1}", theFromDate.ToString(pattern), theToDate.ToString(pattern));

            // Supervisor data
            string startDateBefore = SeleniumHelper.GetToDay();
            string endDateBefore = SeleniumHelper.GetDayAfter(SeleniumHelper.GetToDay(), 7);
            string startDateAfter = SeleniumHelper.GetDayAfter(SeleniumHelper.GetToDay(), 8);
            string endDateAfter = SeleniumHelper.GetDayAfter(SeleniumHelper.GetToDay(), 15);

            // Pupil data
            string pupilStartDate = theFromDate.ToString(pattern);
            string pupilEndDate = theToDate.ToString(pattern);

            var res = new List<Object[]>
            {                
                new object[] {
                    // Teaching group
                    new string[]{ "Teaching Group " + random, random, "English", academicYear, academicLength},
                    // Staff
                    new string[]{ "Lawson", "Teacher: Classroom Teacher", startDateBefore, endDateBefore, startDateAfter, endDateAfter, "Lawson, Ms Regan" },
                    // Pupil
                    new string[]{ "Adams, Laura", pupilStartDate, pupilEndDate }                    
                },
            };
            return res;
        }

        public List<object[]> TC_TG010_Data()
        {
            string random = SeleniumHelper.GenerateRandomString(6);
            string academicYear = String.Format("Academic Year {0}/{1}", (DateTime.Now.Year + 1).ToString(), (DateTime.Now.Year + 2).ToString());

            var theFromDate = new DateTime(DateTime.Today.Year + 1, 8, 1);
            var theToDate = new DateTime(DateTime.Today.Year + 2, 7, 31);
            // Future


            var res = new List<Object[]>
            {                
                new object[] {
                    new string[]{ "Teaching Group "+random,random, "English",academicYear},                    
                    new string[]{"Lawson","Teacher: Classroom Teacher"},
                    "Administrator / Clerk"
                },
            };
            return res;
        }

        public List<object[]> TC_TG011_Data()
        {
            string random = SeleniumHelper.GenerateRandomString(6);
            string academicYear = String.Format("Academic Year {0}/{1}", (DateTime.Now.Year).ToString(), (DateTime.Now.Year + 1).ToString());

            var theFromDate = new DateTime(DateTime.Today.Year, 8, 1);
            var theToDate = new DateTime(DateTime.Today.Year + 1, 7, 31);
            // Current            

            var res = new List<Object[]>
            {                
                new object[] {
                    new string[]{ "Teaching Group "+random,random, "English",academicYear},                    
                    new string[]{"Lawson","Teacher: Classroom Teacher"},
                    "Administrator / Clerk"
                },
            };
            return res;
        }

        public List<object[]> TC_TG012_Data()
        {
            string pattern = "M/d/yyyy";
            string futureAcademicYear = String.Format("Academic Year {0}/{1}", (DateTime.Now.Year + 1).ToString(), (DateTime.Now.Year + 2).ToString());

            var fromDate = new DateTime(DateTime.Today.Year + 1, 11, 12).ToString(pattern);
            var toDate = new DateTime(DateTime.Today.Year + 1, 11, 12).ToString(pattern);

            string groupFullName = "GFN " + SeleniumHelper.GenerateRandomString(5);
            string groupShortName = "GSN" + SeleniumHelper.GenerateRandomString(5);
            string subject = "English";

            // Supervisors 1 data
            string staffName = "Lawson, Ms Regan";
            string staffRole = "Teacher: Classroom Teacher";

            // Pupil data
            string pupilName = "Caster";
            string yearGroup = "Year 1";
            string classes = "1A";

            var data = new List<Object[]>
            {                
                new object[] {
                    groupFullName,groupShortName,subject,
                    futureAcademicYear,staffName,staffRole,fromDate,toDate,pupilName,yearGroup,classes}
                };
            return data;
        }

        public List<object[]> TC_TG013_Data()
        {
            string random = SeleniumHelper.GenerateRandomString(6);
            string academicYear = String.Format("Academic Year {0}/{1}", (DateTime.Now.Year + 1).ToString(), (DateTime.Now.Year + 2).ToString());

            string sysFormat = CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern;

            var theFromDate = new DateTime(DateTime.Today.Year + 1, 8, 1);
            var theToDate = new DateTime(DateTime.Today.Year + 2, 7, 31);
            // Future
            string academicLength = String.Format("{0} - {1}", theFromDate.ToString(sysFormat), theToDate.ToString(sysFormat));

            var res = new List<Object[]>
            {                
                new object[] {
                    new string[]{ "Teaching Group "+random,random, "English",academicYear, academicLength},                    
                    new string[]{"Lawson","Teacher: Classroom Teacher"},
                    "Brooks"
                },
            };
            return res;
        }

        public List<object[]> TC_TG014_Data()
        {

            string pattern = "M/d/yyyy";
            string currentAcademicYear = String.Format("Academic Year {0}/{1}", DateTime.Now.Year.ToString(), (DateTime.Now.Year + 1).ToString());
            string groupFullName = "GFN " + SeleniumHelper.GenerateRandomString(5);
            string groupShortName = "GSN" + SeleniumHelper.GenerateRandomString(5);
            string subject = "English";

            // Supervisors 1 data
            string staffName = "Lawson, Ms Regan";
            string staffRole = "Teacher: Classroom Teacher"; ;
            var fromDate1 = new DateTime(DateTime.Today.Year, 10, 10).ToString(pattern);
            var toDate1 = new DateTime(DateTime.Today.Year, 10, 30).ToString(pattern);

            // Pupil data
            string pupilName = "Caster";
            string yearGroup = "Year 1";
            string classes = "1A";

            var fromDate2 = new DateTime(DateTime.Today.Year, 11, 11).ToString(pattern);
            var toDate2 = new DateTime(DateTime.Today.Year, 12, 12).ToString(pattern);

            var data = new List<Object[]>
            {                
                new object[] {
                    groupFullName,groupShortName,subject,
                    staffName,staffRole,fromDate1,toDate1,
                    pupilName,yearGroup,classes,fromDate2,toDate2}
                };
            return data;
        }

        public List<object[]> TC_TG019_Data()
        {

            string staffName = String.Format("TG19_Staff_{0}", SeleniumHelper.GenerateRandomString(5));
            string ServiceDetailDateArrival = (new DateTime(DateTime.Today.Year - 1, 10, 10)).ToString("M/d/yyyy");
            string dateOfLeaving = DateTime.Now.ToString("M/d/yyyy");
            string effectiveFromDate = DateTime.Now.AddDays(-5).ToString("M/d/yyyy");
            string currentAcademicYear = String.Format("Academic Year {0}/{1}", DateTime.Now.Year.ToString(), (DateTime.Now.Year + 1).ToString());

            var res = new List<Object[]>
            {                
                new object[] {
                    new string[]{staffName, staffName, "Male"},
                    new string[]{ServiceDetailDateArrival},
                    new string[]{"Supervisor", DateTime.Now.AddMonths(-2).ToString("M/d/yyyy")},
                    new string[]{dateOfLeaving, "Retirement"},
                    new string[]{ String.Format("TG19_Group_{0}",SeleniumHelper.GenerateRandomString(5)), SeleniumHelper.GenerateRandomString(10), "English", "Test For TG 19"},
                    new string[]{currentAcademicYear, effectiveFromDate, String.Empty}
                },
            };
            return res;
        }

        public List<object[]> TC_TG020_Data()
        {

            string staffName = String.Format("TG20_Staff_{0}", SeleniumHelper.GenerateRandomString(5));
            string ServiceDetailDateArrival = (new DateTime(DateTime.Today.Year - 1, 10, 10)).ToString("M/d/yyyy");
            string dateOfLeaving = DateTime.Now.ToString("M/d/yyyy");
            string effectiveFromDate = DateTime.Now.AddDays(-5).ToString("M/d/yyyy");
            string effectiveToDate = DateTime.Now.AddDays(-1).ToString("M/d/yyyy");
            string currentAcademicYear = String.Format("Academic Year {0}/{1}", DateTime.Now.Year.ToString(), (DateTime.Now.Year + 1).ToString());

            var res = new List<Object[]>
            {                
                new object[] {
                    new string[]{staffName, staffName, "Male"},
                    new string[]{ServiceDetailDateArrival},
                    new string[]{"Supervisor",  DateTime.Now.AddMonths(-2).ToString("M/d/yyyy")},
                    new string[]{dateOfLeaving, "Retirement"},
                    new string[]{ String.Format("TG20_Group_{0}",SeleniumHelper.GenerateRandomString(5)), SeleniumHelper.GenerateRandomString(10), "English", "Test For TG 20"},
                    new string[]{currentAcademicYear, effectiveFromDate, effectiveToDate}
                },
            };
            return res;
        }

        public List<object[]> TC_TG021_Data()
        {

            string currentAcademicYear = String.Format("Academic Year {0}/{1}", DateTime.Now.Year.ToString(), (DateTime.Now.Year + 1).ToString());
            string groupFullName = "GFN " + SeleniumHelper.GenerateRandomString(5);
            string groupShortName = "GSN" + SeleniumHelper.GenerateRandomString(5);
            string subject = "English";

            string pattern = "M/d/yyyy";

            // Supervisors data
            string staffName = "Lawson, Ms Regan";
            string staffRole = "Teacher: Classroom Teacher";

            var fromDate = new DateTime(DateTime.Today.Year, 11, 10).ToString(pattern);
            var toDate = new DateTime(DateTime.Today.Year, 11, 12).ToString(pattern); ;

            // Pupil 1 data
            string pupilName = "Caster";
            string yearGroup = "Year 1";
            string classes = "1A";

            var data = new List<Object[]>
            {                
                new object[] {
                    groupFullName,groupShortName,subject,
                    staffName,staffRole,
                    pupilName,yearGroup,classes,fromDate,toDate
                    }
                };
            return data;
        }

        public List<object[]> TC_TG022_Data()
        {

            string currentAcademicYear = String.Format("Academic Year {0}/{1}", DateTime.Now.Year.ToString(), (DateTime.Now.Year + 1).ToString());
            string fromDate = DateTime.ParseExact("08/01/2015", "M/d/yyyy", CultureInfo.InvariantCulture).ToString("M/d/yyyy");
            string toDate = DateTime.ParseExact("09/01/2015", "M/d/yyyy", CultureInfo.InvariantCulture).ToString("M/d/yyyy");

            string fromDate1 = DateTime.ParseExact("08/01/2015", "M/d/yyyy", CultureInfo.InvariantCulture).ToString("M/d/yyyy");
            string toDate1 = DateTime.ParseExact("09/01/2015", "M/d/yyyy", CultureInfo.InvariantCulture).ToString("M/d/yyyy");

            string fromDate2 = DateTime.ParseExact("10/01/2015", "M/d/yyyy", CultureInfo.InvariantCulture).ToString("M/d/yyyy");
            string toDate2 = DateTime.ParseExact("11/11/2015", "M/d/yyyy", CultureInfo.InvariantCulture).ToString("M/d/yyyy");

            string groupFullName = "GFN " + SeleniumHelper.GenerateRandomString(5);
            string groupShortName = "GSN" + SeleniumHelper.GenerateRandomString(5);
            string subject = "English";

            // Supervisors data
            string staffCode = "";
            string sureName = "";
            string staffRole = "Supervisor";

            // Pupil 1 data
            string pupilName = "Caster";
            string yearGroup = "Year 1";
            string classes = "1A";

            var data = new List<Object[]>
            {                
                new object[] {
                    groupFullName,groupShortName,subject,
                    currentAcademicYear,fromDate,toDate,
                    staffCode,sureName,staffRole,
                    pupilName,yearGroup,classes,fromDate1,toDate1,
                    fromDate2,toDate2}
                };
            return data;
        }

        public List<object[]> TC_TG028_Data()
        {
            string pattern = "M/d/yyyy";
            string randomString = SeleniumHelper.GenerateRandomString(8);
            string surName = "SUR_" + randomString;
            string foreName = "FORE_" + randomString;
            string gender = "Male";
            string dateOfBirth = DateTime.ParseExact("10/16/2006", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            string DateOfAdmission = DateTime.ParseExact("10/19/2010", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            string yearGroup = "Year 2";
            string theFromDate = new DateTime(DateTime.Today.Year, 8, 1).ToString(pattern);

            string dateOfLeaving = SeleniumHelper.GetDayAfter(theFromDate, 7);


            var res = new List<Object[]>
            {                
                new object[] {
                    // Teaching group
                    new string[]{ "Teaching Group "+ randomString, randomString, "English", theFromDate },
                    // Pupil
                    new string[]{ foreName, surName, gender, dateOfBirth, DateOfAdmission, yearGroup, dateOfLeaving, "Not Known" }
                    }                    
                
            };
            return res;
        }

        #endregion DATA
    }
}
