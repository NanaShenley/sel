using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using POM.Components.Common;
using POM.Components.Pupil;
using Pupil.Components;
using Pupil.Components.Common;
using SharedComponents.BaseFolder;
using SharedComponents.CRUD;
using SharedComponents.Helpers;
using TestSettings;
using WebDriverRunner.internals;
using WebDriverRunner.webdriver;

namespace Pupil.PupilLog.Tests
{

    public class DisabledTests
    {
        private Guid _learnerID = new Guid("3cb5791c-95eb-4d06-a5c9-60c32751418e");
        private Guid _learner1ID = new Guid("965D5B7A-6E99-474A-9617-58315DC33CDD");  //Aaron, Chris
        private Guid _learner2ID = new Guid("07121A29-CC8A-4AC0-B846-2161F76427D7"); //Acton, Stan
        private Guid _learner3ID = new Guid("77FA0C97-BC7E-4B86-9288-62649ADA28B6"); //Aaron, Sophie

        #region Note Ids

        private Guid _generalNote1;
        private Guid _generalNote2;
        private Guid _generalNote3;
        private Guid _achievementNote1;
        private Guid _achievementNote2;
        private Guid _assessmentNote1;
        private Guid _assessmentNote2;
        private Guid _attendanceNote1;

        #endregion


        //[WebDriverTest(Groups = new[] { PupilTestGroups.PupilLog.StatsTests }, Browsers = new[] { BrowserDefaults.Chrome, PupilTestGroups.Priority.Priority4 })]
        public void FindPupilLogById()
        {
            WebContext.WebDriver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(10));
            var pupilLogNavigation = new PupilLogNavigation();
            pupilLogNavigation.NavigateToPupilLog_PupilLogDetails(PupilAreaConstants.PupilLogId,
                SeleniumHelper.iSIMSUserType.ClassTeacher);
            Assert.AreEqual(PupilAreaConstants.PupilLogDisplayName,
                SeleniumHelper.Get(PupilElements.PupilLog.PupilLogAvatarName).Text);
            WebContext.WebDriver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(0));
        }


        //[WebDriverTest(Enabled = false, Groups = new[] { PupilTestGroups.PupilLog.FilterPanelTests, PupilTestGroups.Priority.Priority3 }, Browsers = new[] { BrowserDefaults.Chrome })]
        public void On_Second_Click_Of_PupilLogFilterButton_Check_SortAndFilter_Panel_Hides()
        {
            // Rerun above test that shows the filter panel
            On_Click_Of_PupilLogFilterButton_Check_For_SortAndFilter_Panel();

            //Click the 'Sort & Filter' button in the toolbar again
            SeleniumHelper.FindAndClick(PupilElements.PupilLog.Detail.PupilLogSortFilterButton);

            // Wait for the time it takes for the panel to disappear
            Thread.Sleep(2000);

            // Check the Filter Panel disappears (hides)
            var sortAndFilterPanel = SeleniumHelper.GetVisible(By.CssSelector("#pane-sort-and-filter"));

            //Assert
            Assert.AreEqual(true, sortAndFilterPanel == null);
        }

        //[WebDriverTest(Enabled = false, Groups = new[] { PupilTestGroups.PupilLog.FilterPanelTests, PupilTestGroups.Priority.Priority3 }, Browsers = new[] { BrowserDefaults.Chrome })]
        public void On_Click_Of_PupilLogFilterButton_And_Checks_Filter_Buttons()
        {
            //Arrange
            On_Click_Of_PupilLogFilterButton_Check_For_SortAndFilter_Panel();

            //Act (Active checks)
            SeleniumHelper.FindAndClick(PupilElements.PupilLog.Detail.PupilLogSortFilterPanel.InActiveGeneralNoteButton);
            var activeGeneralFilterButton = SeleniumHelper.Get(PupilElements.PupilLog.Detail.PupilLogSortFilterPanel.ActiveGeneralNoteButton);
            Assert.IsNotNull(activeGeneralFilterButton);

            SeleniumHelper.FindAndClick(PupilElements.PupilLog.Detail.PupilLogSortFilterPanel.InActiveAssessmentNoteButton);
            var activeAssessmentFilterButton = SeleniumHelper.Get(PupilElements.PupilLog.Detail.PupilLogSortFilterPanel.ActiveAssessmentNoteButton);
            Assert.IsNotNull(activeAssessmentFilterButton);

            SeleniumHelper.FindAndClick(PupilElements.PupilLog.Detail.PupilLogSortFilterPanel.InActiveAchievementsNoteButton);
            var activeAchievementsFilterButton = SeleniumHelper.Get(PupilElements.PupilLog.Detail.PupilLogSortFilterPanel.ActiveAchievementsNoteButton);
            Assert.IsNotNull(activeAchievementsFilterButton);

            SeleniumHelper.FindAndClick(PupilElements.PupilLog.Detail.PupilLogSortFilterPanel.InActiveAttendanceNoteButton);
            var activeAttendanceFilterButton = SeleniumHelper.Get(PupilElements.PupilLog.Detail.PupilLogSortFilterPanel.ActiveAttendanceNoteButton);
            Assert.IsNotNull(activeAttendanceFilterButton);

            SeleniumHelper.FindAndClick(PupilElements.PupilLog.Detail.PupilLogSortFilterPanel.InActiveSENNoteButton);
            var activeSENFilterButton = SeleniumHelper.Get(PupilElements.PupilLog.Detail.PupilLogSortFilterPanel.ActiveSENNoteButton);
            Assert.IsNotNull(activeSENFilterButton);
        }

        //[WebDriverTest(Enabled = false, Groups = new[] { PupilTestGroups.PupilLog.FilterPanelTests, PupilTestGroups.Priority.Priority3 }, Browsers = new[] { BrowserDefaults.Chrome })]
        public void On_Click_Of_PupilLogFilterButton_And_Checks_Sort_Buttons()
        {
            //Arrange
            On_Click_Of_PupilLogFilterButton_Check_For_SortAndFilter_Panel();

            //Oldest first click
            SeleniumHelper.FindAndClick(PupilElements.PupilLog.Detail.PupilLogSortFilterPanel.OldestFirstButton);
            var oldestFirstButton = SeleniumHelper.Get(PupilElements.PupilLog.Detail.PupilLogSortFilterPanel.ActiveOldestFirstButton);
            Assert.AreEqual(true, oldestFirstButton != null);

            Thread.Sleep(1000);

            //Newest first click
            SeleniumHelper.FindAndClick(PupilElements.PupilLog.Detail.PupilLogSortFilterPanel.NewestFirstButton);
            var newestFirstButton = SeleniumHelper.Get(PupilElements.PupilLog.Detail.PupilLogSortFilterPanel.ActiveNewestFirstButton);
            Assert.AreEqual(true, newestFirstButton != null);

            Thread.Sleep(1000);

            //Category first click
            SeleniumHelper.FindAndClick(PupilElements.PupilLog.Detail.PupilLogSortFilterPanel.CategoryButton);
            var categoryButton = SeleniumHelper.Get(PupilElements.PupilLog.Detail.PupilLogSortFilterPanel.ActiveCategoryButton);
            Assert.AreEqual(true, categoryButton != null);
        }

        //[WebDriverTest(Enabled = false, Groups = new[] { PupilTestGroups.PupilLog.FilterTests, PupilTestGroups.Priority.Priority3 }, Browsers = new[] { BrowserDefaults.Chrome })]
        public void On_Click_Of_Filter_Buttons_Notes_Are_Filtered()
        {
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.ClassTeacher);
            SeleniumHelper.FindAndClick(SeleniumHelper.AutomationId("quicklinks_top_level_pupil_submenu_pupillog"));

            SearchCriteria.Search();
            SearchResults.WaitForResults();
            SearchResults.Click(_learnerID.ToString());

            WebContext.WebDriver.Manage().Window.Maximize();

            BaseSeleniumComponents.WaitUntilDisplayed(new TimeSpan(0, 0, 0, 5), PupilElements.PupilLog.Detail.PupilLogSortFilterButton);

            // Click the 'Sort & Filter' button in the toolbar
            SeleniumHelper.FindAndClick(PupilElements.PupilLog.Detail.PupilLogSortFilterButton);

            // Filter on General notes
            SeleniumHelper.FindAndClick(PupilElements.PupilLog.Detail.PupilLogSortFilterPanel.InActiveGeneralNoteButton);

            WaitUntilNotesReloaded();

            // Assert 3 General notes only
            var genNotes = WebContext.WebDriver.FindElements(By.CssSelector(SeleniumHelper.AutomationId("log-note-header-general")));
            Assert.AreEqual(3, genNotes.Count);
            var assessmentNotes = WebContext.WebDriver.FindElements(By.CssSelector(SeleniumHelper.AutomationId("log-note-header-assessment")));
            Assert.AreEqual(0, assessmentNotes.Count);
            var attNotes = WebContext.WebDriver.FindElements(By.CssSelector(SeleniumHelper.AutomationId("log-note-header-attendance")));
            Assert.AreEqual(0, attNotes.Count);

            // Filter on Attendance notes
            SeleniumHelper.FindAndClick(PupilElements.PupilLog.Detail.PupilLogSortFilterPanel.InActiveAttendanceNoteButton);

            // Wait until the locking mask disappears i.e. the notes have been reloaded
            (new WebDriverWait(WebContext.WebDriver, new TimeSpan(0, 0, 1, 0)))
                .Until(ExpectedConditions.InvisibilityOfElementLocated(By.CssSelector(PupilElements.PupilLog.Detail.LockingMask)));

            // Assert 1 Attendance note only
            attNotes = WebContext.WebDriver.FindElements(By.CssSelector(SeleniumHelper.AutomationId("log-note-header-attendance")));
            Assert.AreEqual(1, attNotes.Count);
            genNotes = WebContext.WebDriver.FindElements(By.CssSelector(SeleniumHelper.AutomationId("log-note-header-general")));
            Assert.AreEqual(0, genNotes.Count);
            assessmentNotes = WebContext.WebDriver.FindElements(By.CssSelector(SeleniumHelper.AutomationId("log-note-header-assessment")));
            Assert.AreEqual(0, assessmentNotes.Count);

            // Filter on SEN notes
            SeleniumHelper.FindAndClick(PupilElements.PupilLog.Detail.PupilLogSortFilterPanel.InActiveSENNoteButton);

            WaitUntilNotesReloaded();

            // Assert No notes
            var senNotes = WebContext.WebDriver.FindElements(By.CssSelector(SeleniumHelper.AutomationId("log-note-header-sen")));
            Assert.AreEqual(0, senNotes.Count);
            genNotes = WebContext.WebDriver.FindElements(By.CssSelector(SeleniumHelper.AutomationId("log-note-header-general")));
            Assert.AreEqual(0, genNotes.Count);
            assessmentNotes = WebContext.WebDriver.FindElements(By.CssSelector(SeleniumHelper.AutomationId("log-note-header-assessment")));
            Assert.AreEqual(0, assessmentNotes.Count);
        }

        //[WebDriverTest(Enabled = false, Groups = new[] { PupilTestGroups.PupilLog.SorterTests, PupilTestGroups.Priority.Priority3 }, Browsers = new[] { BrowserDefaults.Chrome })]
        public void On_Click_Of_Sorting_Buttons_Notes_Are_Sorted()
        {
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.HeadTeacher);
            SeleniumHelper.FindAndClick(SeleniumHelper.AutomationId("quicklinks_top_level_pupil_submenu_pupillog"));

            SearchCriteria.Search();
            SearchResults.WaitForResults();
            SearchResults.Click(_learnerID.ToString());

            WebContext.WebDriver.Manage().Window.Maximize();

            BaseSeleniumComponents.WaitUntilDisplayed(new TimeSpan(0, 0, 0, 5), PupilElements.PupilLog.Detail.PupilLogSortFilterButton);

            // Click the 'Sort & Filter' button in the toolbar
            SeleniumHelper.FindAndClick(PupilElements.PupilLog.Detail.PupilLogSortFilterButton);

            // Sort by newest first
            SeleniumHelper.FindAndClick(PupilElements.PupilLog.Detail.PupilLogSortFilterPanel.OldestFirstButton);

            WaitUntilNotesReloaded();

            // Assert Pinned General Note 3 is first and Attendance note 1 is last
            var allNotes = WebContext.WebDriver.FindElements(By.CssSelector(SeleniumHelper.AutomationId("log-timeline-entry")));
            var noteHeader = allNotes.First().FindElement(By.CssSelector(SeleniumHelper.AutomationId("log-note-header-general")));
            Assert.IsNotNull(noteHeader);
            var heading = noteHeader.FindElement(By.CssSelector(SeleniumHelper.AutomationId("log-event-heading")));
            Assert.IsTrue(heading.Text.StartsWith("General Note 3"));

            // Assert Attendance Note 1 is last
            noteHeader = allNotes.Last().FindElement(By.CssSelector(SeleniumHelper.AutomationId("log-note-header-attendance")));
            Assert.IsNotNull(noteHeader);
            heading = noteHeader.FindElement(By.CssSelector(SeleniumHelper.AutomationId("log-event-heading")));
            Assert.IsTrue(heading.Text.StartsWith("Attendance Note 1"));
        }

        //[WebDriverTest(Enabled = false, Groups = new[] { PupilTestGroups.PupilLog.FilterTests, PupilTestGroups.PupilLog.SorterTests, PupilTestGroups.Priority.Priority3 }, Browsers = new[] { BrowserDefaults.Chrome })]
        public void Creating_A_Note_Keeps_Sort_And_Filter()
        {
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);
            SeleniumHelper.FindAndClick(SeleniumHelper.AutomationId("quicklinks_top_level_pupil_submenu_pupillog"));

            SearchCriteria.Search();
            SearchResults.WaitForResults();
            SearchResults.Click(_learnerID.ToString());

            WebContext.WebDriver.Manage().Window.Maximize();

            BaseSeleniumComponents.WaitUntilDisplayed(new TimeSpan(0, 0, 0, 5), PupilElements.PupilLog.Detail.PupilLogSortFilterButton);

            // Click the 'Sort & Filter' button in the toolbar
            SeleniumHelper.FindAndClick(PupilElements.PupilLog.Detail.PupilLogSortFilterButton);

            // Filter on General notes
            SeleniumHelper.FindAndClick(PupilElements.PupilLog.Detail.PupilLogSortFilterPanel.InActiveGeneralNoteButton);

            // Sort by newest first
            SeleniumHelper.FindAndClick(PupilElements.PupilLog.Detail.PupilLogSortFilterPanel.OldestFirstButton);

            WaitUntilNotesReloaded();

            // Use Automation Id once implemented in platform
            //SeleniumHelper.FindAndClick(SeleniumHelper.AutomationId("create-plog-contextual-link"));
            SeleniumHelper.FindAndClick(By.CssSelector(PupilElements.PupilLog.Detail.CreateNoteDropDownBtn));
            SeleniumHelper.FindAndClick(SeleniumHelper.AutomationId("plog-generalcategory-contextlink"));
            BaseSeleniumComponents.WaitUntilDisplayed(new TimeSpan(0, 0, 0, 5), By.CssSelector(PupilElements.PupilLog.Detail.Note.NoteText));

            ControlsHelper.UpdateValue(PupilElements.PupilLog.Detail.Note.NoteText, "General Note added by test");
            SeleniumHelper.FindAndClick(By.CssSelector(PupilElements.PupilLog.Detail.Note.SaveBtn));

            WaitUntilNotesReloaded();

            // Assert count is 4
            var allNotes = WebContext.WebDriver.FindElements(By.CssSelector(SeleniumHelper.AutomationId("log-timeline-entry")));
            Assert.AreEqual(4, allNotes.Count);

            // Assert added note is last
            var noteHeader = allNotes.Last().FindElement(By.CssSelector(SeleniumHelper.AutomationId("log-note-header-general")));
            Assert.IsNotNull(noteHeader);
            var heading = noteHeader.FindElement(By.CssSelector(SeleniumHelper.AutomationId("log-event-heading")));
            Assert.IsTrue(heading.Text.StartsWith("General Note added by test"));
        }


        //[WebDriverTest(Enabled = true, Groups = new[] { PupilTestGroups.PupilLog.FilterTests, PupilTestGroups.Priority.Priority3 }, Browsers = new[] { BrowserDefaults.Chrome })]
        public void On_Click_Of_GenerealFilterWithDescending_Buttons_TabOut_And_BackIn()
        {
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.ClassTeacher);
            SeleniumHelper.FindAndClick(SeleniumHelper.AutomationId(PupilElements.PupilLogQuickLink));

            SearchCriteria.Search();
            SearchResults.WaitForResults();
            SearchResults.Click(_learnerID.ToString());

            WebContext.WebDriver.Manage().Window.Maximize();

            BaseSeleniumComponents.WaitUntilDisplayed(new TimeSpan(0, 0, 0, 5), PupilElements.PupilLog.Detail.PupilLogSortFilterButton);

            // Click the 'Sort & Filter' button in the toolbar
            SeleniumHelper.FindAndClick(PupilElements.PupilLog.Detail.PupilLogSortFilterButton);

            // Filter on General notes
            SeleniumHelper.FindAndClick(PupilElements.PupilLog.Detail.PupilLogSortFilterPanel.InActiveGeneralNoteButton);

            WaitUntilNotesReloaded();
            Thread.Sleep(2000);
            //Navigate to application screen
            var pupilRecordNavigator = new PupilRecordNavigation();
            pupilRecordNavigator.NavigateToPupilRecord_SearchPupil();
            SeleniumHelper.FindAndClick(By.CssSelector(SeleniumHelper.AutomationId(PupilElements.PupilLog.PupilLogTab)), TimeSpan.FromSeconds(2));
            WaitUntilNotesReloaded();
            Thread.Sleep(2000);
            var filterPanelWithSlideDownPosition = SeleniumHelper.Get(PupilElements.PupilLog.Detail.PupilLogSortFilterPanel.FilterPanelSlideDownState);
            Assert.IsNotNull(filterPanelWithSlideDownPosition);
            //Check filter
            var filterButtons = WebContext.WebDriver.FindElements(PupilElements.PupilLog.Detail.PupilLogSortFilterPanel.CategoryFilter);
            var actualFilter = filterButtons.Where(filterButton => filterButton.GetAttribute("value") == "GEN").Any(filterButton => filterButton.Selected);
            Assert.AreEqual(true, actualFilter);
            //Check sorter
            var sorterButtons = WebContext.WebDriver.FindElements(PupilElements.PupilLog.Detail.PupilLogSortFilterPanel.Sorter);
            var actualSorter = sorterButtons.Where(sorterButton => sorterButton.GetAttribute("value") == "DESC").Any(sorterButton => sorterButton.Selected);
            Assert.AreEqual(true, actualSorter);
        }


        //[WebDriverTest(Enabled = true, Groups = new[] { PupilTestGroups.PupilLog.MaintainStateTests, PupilTestGroups.Priority.Priority3 }, Browsers = new[] { BrowserDefaults.Chrome })]
        public void On_SelectAnotherPupil_StateMaintained()
        {
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.ClassTeacher);
            SeleniumHelper.FindAndClick(SeleniumHelper.AutomationId(PupilElements.PupilLogQuickLink));

            SearchCriteria.Search();
            SearchResults.WaitForResults();
            SearchResults.Click(_learner1ID.ToString());

            WebContext.WebDriver.Manage().Window.Maximize();

            BaseSeleniumComponents.WaitUntilDisplayed(new TimeSpan(0, 0, 0, 5), PupilElements.PupilLog.Detail.PupilLogSortFilterButton);

            // Click the 'Sort & Filter' button in the toolbar
            SeleniumHelper.FindAndClick(PupilElements.PupilLog.Detail.PupilLogSortFilterButton);

            // Filter on General notes
            SeleniumHelper.FindAndClick(PupilElements.PupilLog.Detail.PupilLogSortFilterPanel.InActiveAssessmentNoteButton);

            WaitUntilNotesReloaded();

            //Navigate to another pupil
            SearchResults.Click(_learner2ID.ToString());
            WebContext.WebDriver.Manage().Window.Maximize();
            BaseSeleniumComponents.WaitUntilDisplayed(new TimeSpan(0, 0, 0, 5), PupilElements.PupilLog.Detail.PupilLogSortFilterButton);

            //Check filter
            var filterButtons = WebContext.WebDriver.FindElements(PupilElements.PupilLog.Detail.PupilLogSortFilterPanel.CategoryFilter);
            filterButtons = filterButtons.DeStaler(PupilElements.PupilLog.Detail.PupilLogSortFilterPanel.CategoryFilter);
            var actualFilter = filterButtons.Where(filterButton => filterButton.GetAttribute("value") == "ASSM").Any(filterButton => filterButton.Selected);
            Assert.AreEqual(true, actualFilter);
            //Check sorter
            var sorterButtons = WebContext.WebDriver.FindElements(PupilElements.PupilLog.Detail.PupilLogSortFilterPanel.Sorter);
            sorterButtons = sorterButtons.DeStaler(PupilElements.PupilLog.Detail.PupilLogSortFilterPanel.Sorter);
            var actualSorter = sorterButtons.Where(sorterButton => sorterButton.GetAttribute("value") == "DESC").Any(sorterButton => sorterButton.Selected);
            Assert.AreEqual(true, actualSorter);
        }

        //[WebDriverTest(Enabled = true, Groups = new[] { PupilTestGroups.PupilLog.MaintainStateTests, PupilTestGroups.Priority.Priority3 }, Browsers = new[] { BrowserDefaults.Chrome })]
        public void On_SelectAnotherPupilWithNoNotes_StateMaintained()
        {
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.ClassTeacher);
            SeleniumHelper.FindAndClick(SeleniumHelper.AutomationId(PupilElements.PupilLogQuickLink));

            SearchCriteria.Search();
            SearchResults.WaitForResults();
            SearchResults.Click(_learner1ID.ToString());

            WebContext.WebDriver.Manage().Window.Maximize();

            BaseSeleniumComponents.WaitUntilDisplayed(new TimeSpan(0, 0, 0, 5), PupilElements.PupilLog.Detail.PupilLogSortFilterButton);

            // Click the 'Sort & Filter' button in the toolbar
            SeleniumHelper.FindAndClick(PupilElements.PupilLog.Detail.PupilLogSortFilterButton);

            // Filter on General notes
            SeleniumHelper.FindAndClick(PupilElements.PupilLog.Detail.PupilLogSortFilterPanel.InActiveAssessmentNoteButton);

            WaitUntilNotesReloaded();

            //Navigate to another pupil
            SearchResults.Click(_learner3ID.ToString());
            WebContext.WebDriver.Manage().Window.Maximize();
            BaseSeleniumComponents.WaitUntilDisplayed(new TimeSpan(0, 0, 0, 5), PupilElements.PupilLog.Detail.PupilLogSortFilterButton);

            //Check filter
            var filterButtons = WebContext.WebDriver.FindElements(PupilElements.PupilLog.Detail.PupilLogSortFilterPanel.CategoryFilter);
            var actualFilter = filterButtons.Where(filterButton => filterButton.GetAttribute("value") == "ASSM").Any(filterButton => filterButton.Selected);
            Assert.AreEqual(true, actualFilter);
            //Check sorter
            var sorterButtons = WebContext.WebDriver.FindElements(PupilElements.PupilLog.Detail.PupilLogSortFilterPanel.Sorter);
            var actualSorter = sorterButtons.Where(sorterButton => sorterButton.GetAttribute("value") == "DESC").Any(sorterButton => sorterButton.Selected);
            Assert.AreEqual(true, actualSorter);
        }

        /// <summary>
        /// Wait until the locking mask disappears i.e. the notes have been reloaded
        /// </summary>
        private void WaitUntilNotesReloaded()
        {
            (new WebDriverWait(WebContext.WebDriver, new TimeSpan(0, 0, 1, 0)))
            .Until(ExpectedConditions.InvisibilityOfElementLocated(By.CssSelector(PupilElements.PupilLog.Detail.LockingMask)));
        }

        private void On_Click_Of_PupilLogFilterButton_Check_For_SortAndFilter_Panel()
        {
            //Arrange
            const string studentId = "965d5b7a-6e99-474a-9617-58315dc33cdd";

            var pupilLogNavigation = new PupilLogNavigation();
            pupilLogNavigation.NavigateToPupilLog_PupilLogDetails(studentId, SeleniumHelper.iSIMSUserType.ClassTeacher);

            //Click the 'Sort & Filter' button in the toolbar
            SeleniumHelper.FindAndClick(PupilElements.PupilLog.Detail.PupilLogSortFilterButton);

            // Wait for the time it takes for the panel to disappear
            Thread.Sleep(2000);

            // Check the Filter Panel appears
            var sortAndFilterPanel = SeleniumHelper.GetVisible(By.CssSelector("#pane-sort-and-filter"));

            //Assert
            Assert.AreEqual(true, sortAndFilterPanel.Size.Height > 0);
        }


        /// <summary>
        /// Author: Luong.Mai
        /// Description: PU-036c : 
        /// </summary>
        //[WebDriverTest(TimeoutSeconds = 1200, Enabled = true, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { PupilTestGroups.PupilLog.Page, PupilTestGroups.Priority.Priority2 }, DataProvider = "TC_PU036c_Data")]
        public void TC_PU036c_Exercise_ability_to_Pin_each_displayed_notice_within_a_Pupil_Log(string forenamePP, string surnamePP)
        {

            // Login as school admin
            POM.Helper.SeleniumHelper.Login(POM.Helper.SeleniumHelper.iSIMSUserType.SchoolAdministrator);

            #region Pre-Condition: Create a new pupil for test

            // Navigate to Pupil Record
            POM.Helper.SeleniumHelper.NavigateMenu("Tasks", "Pupils", "Pupil Records");

            var pupilRecordTriplet = new PupilSearchTriplet();
            var addNewPupilDialog = pupilRecordTriplet.AddNewPupil();
            addNewPupilDialog.Forename = forenamePP;
            addNewPupilDialog.SurName = surnamePP;
            addNewPupilDialog.Gender = "Male";
            addNewPupilDialog.DateOfBirth = "2/2/2011";

            var registrationDetailDialog = addNewPupilDialog.Continue();
            registrationDetailDialog.DateOfAdmission = "2/2/2015";
            registrationDetailDialog.YearGroup = "Year 1";
            registrationDetailDialog.CreateRecord();

            var confirmRequiredDialog = new ConfirmRequiredDialog();
            confirmRequiredDialog.ClickOk();

            var pupilRecordPage = PupilRecordPage.Create();
            pupilRecordPage.SavePupil();

            #endregion

            #region Pre-Condition: Create notes

            // Navigate to Pupil Log
            POM.Helper.SeleniumHelper.NavigateBySearch("Pupil Log", true);

            // Search all pupils
            var pupilLogTriplet = new PupilLogTriplet();
            pupilLogTriplet.SearchCriteria.PupilName = surnamePP + ", " + forenamePP;
            pupilLogTriplet.SearchCriteria.IsCurrent = true;
            pupilLogTriplet.SearchCriteria.IsLeaver = false;
            var pupilResuls = pupilLogTriplet.SearchCriteria.Search();

            // Select pupils
            var pupilLogDetailPage = pupilResuls[0].Click<PupilLogDetailPage>();

            // Expand the 'Create Notes' selector, click on type 'General'.
            pupilLogDetailPage.ClickCreateNote();
            var noteDialog = pupilLogDetailPage.SelelectGeneralNoteType();

            // Create note General
            noteDialog.Note = "General Notice";
            noteDialog.Title = "General Notice";
            noteDialog.SubCategory = "General";
            noteDialog.PinThisNote = false;
            noteDialog.ClickSave();

            // Expand the 'Create Notes' selector, click on type 'Assessment'.
            pupilLogDetailPage.ClickCreateNote();
            noteDialog = pupilLogDetailPage.SelelectAssessmentNoteType();

            // Create note Assessment
            noteDialog.Note = "Assessment Notice";
            noteDialog.Title = "Assessment Notice";
            noteDialog.SubCategory = "English";
            noteDialog.PinThisNote = false;
            noteDialog.ClickSave();

            // Expand the 'Create Notes' selector, click on type 'Achievement'.
            //pupilLogDetailPage.ClickCreateNote();
            //noteDialog = pupilLogDetailPage.SelelectAchievementsNoteType();

            //// Create note Achievement
            //noteDialog.Note = "Achievement Notice";
            //noteDialog.Title = "Achievement Notice";
            //noteDialog.SubCategory = "Academic";
            //noteDialog.PinThisNote = false;
            //noteDialog.ClickSave();

            // Expand the 'Create Notes' selector, click on type 'Attendance'.
            pupilLogDetailPage.ClickCreateNote();
            noteDialog = pupilLogDetailPage.SelelectAttendanceNoteType();

            // Create note Attendance
            noteDialog.Note = "Attendance Notice";
            noteDialog.Title = "Attendance Notice";
            noteDialog.SubCategory = "Authorised";
            noteDialog.PinThisNote = false;
            noteDialog.ClickSave();

            // Expand the 'Create Notes' selector, click on type 'SEN'.
            pupilLogDetailPage.ClickCreateNote();
            noteDialog = pupilLogDetailPage.SelelectSENNoteType();

            // Create note Assessment
            noteDialog.Note = "SEN Notice";
            noteDialog.Title = "SEN Notice";
            noteDialog.SubCategory = "General";
            noteDialog.PinThisNote = false;
            noteDialog.ClickSave();

            #endregion

            #region STEPS

            // Edit SEN note
            var notes = pupilLogDetailPage.TimeLine;
            noteDialog = notes["SEN Notice"].Edit();
            noteDialog.PinThisNote = true;
            noteDialog.ClickSave();

            // Edit General note
            noteDialog = notes["General Notice"].Edit();
            noteDialog.PinThisNote = true;
            noteDialog.ClickSave();

            // Edit Assessment note
            noteDialog = notes["Assessment Notice"].Edit();
            noteDialog.PinThisNote = true;
            noteDialog.ClickSave();

            // Edit Achievement note
            noteDialog = notes["Achievement Notice"].Edit();
            noteDialog.PinThisNote = true;
            noteDialog.ClickSave();

            // Edit Attendance note
            noteDialog = notes["Attendance Notice"].Edit();
            noteDialog.PinThisNote = true;
            noteDialog.ClickSave();

            // Search pupil
            pupilLogTriplet = new PupilLogTriplet();
            pupilLogTriplet.SearchCriteria.PupilName = surnamePP + ", " + forenamePP;
            pupilLogTriplet.SearchCriteria.IsCurrent = true;
            pupilLogTriplet.SearchCriteria.IsLeaver = false;
            pupilResuls = pupilLogTriplet.SearchCriteria.Search();

            // Select pupil
            pupilLogDetailPage = pupilResuls[0].Click<PupilLogDetailPage>();

            // Verify that SEN note is edited
            notes = pupilLogDetailPage.TimeLine;
            noteDialog = notes["SEN Notice"].Edit();
            Assert.AreEqual(true, noteDialog.PinThisNote, "Edit the SEN note unsuccessfully.");

            // Verify that General note is edited
            noteDialog = notes["General Notice"].Edit();
            Assert.AreEqual(true, noteDialog.PinThisNote, "Edit the General note unsuccessfully.");

            // Verify that Assessment note is edited
            noteDialog = notes["Assessment Notice"].Edit();
            Assert.AreEqual(true, noteDialog.PinThisNote, "Edit the Assessment note unsuccessfully.");

            // Verify that Achievement note is edited
            noteDialog = notes["Achievement Notice"].Edit();
            Assert.AreEqual(true, noteDialog.PinThisNote, "Edit the Achievement note unsuccessfully.");

            // Verify that Attendance note is edited
            noteDialog = notes["Attendance Notice"].Edit();
            Assert.AreEqual(true, noteDialog.PinThisNote, "Edit the Attendance note unsuccessfully.");
            noteDialog.ClickSave();

            #endregion

            #region Post-Condition: Delete the pupil if existed

            // Navigate to Pupil Record
            POM.Helper.SeleniumHelper.NavigateMenu("Tasks", "Pupils", "Delete Pupil");
            var deletePupilRecordTriplet = new DeletePupilRecordTriplet();
            deletePupilRecordTriplet.SearchCriteria.PupilName = String.Format("{0}, {1}", surnamePP, forenamePP);
            deletePupilRecordTriplet.SearchCriteria.IsCurrent = true;
            deletePupilRecordTriplet.SearchCriteria.IsFuture = true;
            deletePupilRecordTriplet.SearchCriteria.IsLeaver = true;
            var resultPupils = deletePupilRecordTriplet.SearchCriteria.Search();

            var pupilSearchTile = resultPupils.SingleOrDefault(t => t.Name.Equals(String.Format("{0}, {1}", surnamePP, forenamePP)));
            var deletePupilRecordPage = pupilSearchTile == null ? new DeletePupilRecordPage() : pupilSearchTile.Click<DeletePupilRecordPage>();

            deletePupilRecordPage.Delete();

            #endregion

        }

        #region DATA

        public List<object[]> TC_PU034_Data()
        {
            var res = new List<Object[]>
            {
                new object[]
                {
                    // pupil name,
                    "Bains, Kirk",
                    // Forename pupil
                    "Bains", 
                    // Surname pupil
                    "Kirk"
                }

            };
            return res;
        }

        public List<object[]> TC_PU036a_Data()
        {
            var randomName = "Luong" + POM.Helper.SeleniumHelper.GenerateRandomString(4) + POM.Helper.SeleniumHelper.GenerateRandomNumberUsingDateTime();

            var res = new List<Object[]>
            {
                new object[]
                {
                    // forename,
                    randomName,
                    // surname
                    randomName
                }

            };
            return res;
        }

        public List<object[]> TC_PU036c_Data()
        {
            var randomName = "Luong" + POM.Helper.SeleniumHelper.GenerateRandomString(4) + POM.Helper.SeleniumHelper.GenerateRandomNumberUsingDateTime();

            var res = new List<Object[]>
            {
                new object[]
                {
                    // forename,
                    randomName,
                    // surname
                    randomName
                }

            };
            return res;
        }

        public List<object[]> TC_PU041_Data()
        {
            var res = new List<Object[]>
            {
                new object[]
                {
                    // pupil name,
                    "Bains, Kirk"
                }

            };
            return res;
        }

        #endregion


    }
}
