using System;
using System.Linq;
using System.Threading;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using POM.Components.Pupil;
using Pupil.Components;
using Pupil.Components.Common;
using Pupil.Data;
using SeSugar;
using SharedComponents.BaseFolder;
using SharedComponents.CRUD;
using SharedComponents.Helpers;
using TestSettings;
using WebDriverRunner.webdriver;
using SeSugar.Data;
using Selene.Support.Attributes;
using SeSugar.Automation;

namespace Pupil.PupilLog.Tests
{
    public class PupilLogSortAndFilterPanel
    {
        //private SeleniumHelper.iSIMSUserType LoginAs = SeleniumHelper.iSIMSUserType.ClassTeacher;
        private readonly SeleniumHelper.iSIMSUserType LoginAs = SeleniumHelper.iSIMSUserType.SchoolAdministrator;

        /// <exception cref="StaleElementReferenceException">Thrown when the target element is no longer valid in the document DOM.</exception>
        [WebDriverTest(Enabled = true, Groups = new[] { PupilTestGroups.PupilLog.Page, PupilTestGroups.PupilLog.FilterPanel, PupilTestGroups.Priority.Priority2 }, Browsers = new[] { BrowserDefaults.Chrome })]
        public void On_Click_Of_PupilLogFilterButton_Check_For_SortAndFilter_Panel()
        {
            //Arrange
            var surname = Utilities.GenerateRandomString(10, "Plog");
            var forename = Utilities.GenerateRandomString(10, "Plog");
            var dataPackage = this.BuildDataPackage();
            var learnerId = Guid.NewGuid();
            dataPackage.AddBasicLearner(learnerId, surname, forename, new DateTime(2011, 02, 02), new DateTime(2015, 02, 02));

            using (new DataSetup(purgeBeforeInsert: false, purgeAfterTest: true, packages: dataPackage))
            {
                var pupilLogNavigation = new PupilLogNavigation();
                pupilLogNavigation.NavigateToPupilLog_PupilLogDetails(learnerId.ToString(), LoginAs);

                //Click the 'Sort & Filter' button in the toolbar
                SeleniumHelper.FindAndClick(PupilElements.PupilLog.Detail.PupilLogSortFilterButton);

                // Wait for the time it takes for the panel to disappear
                WebDriverWait wait = new WebDriverWait(WebContext.WebDriver, TimeSpan.FromSeconds(10));
                wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("#pane-sort-and-filter")));

                // Check the Filter Panel appears
                var sortAndFilterPanel = SeleniumHelper.GetVisible(By.CssSelector("#pane-sort-and-filter"));

                //Assert
                Assert.AreEqual(true, sortAndFilterPanel.Size.Height > 0);
            }
        }

        /// <exception cref="NoSuchElementException">If no element matches the criteria.</exception>
        /// <exception cref="StaleElementReferenceException">Thrown when the target element is no longer valid in the document DOM.</exception>
        [WebDriverTest(Enabled = true, Groups = new[] { PupilTestGroups.PupilLog.Page, PupilTestGroups.PupilLog.FilterPanel, PupilTestGroups.Priority.Priority2 }, Browsers = new[] { BrowserDefaults.Chrome })]
        public void Editing_A_Note_Keeps_Sort_And_Filter()
        {
            //Arrange
            var learnerId = Guid.NewGuid();
            var plogDataPackage = this.BuildDataPackage();

            //Add random string surname
            plogDataPackage.AddBasicLearner(learnerId, Utilities.GenerateRandomString(10, "PlogAddTest-"), "Selenium Test", dateOfBirth: new DateTime(2005, 05, 30), dateOfAdmission: new DateTime(2012, 10, 03))
                           .AddStandardPupilLogNote(learnerId, "General Note 1")
                           .AddStandardPupilLogNote(learnerId, "General Note 2")
                           .AddStandardPupilLogNote(learnerId, "General Note 3");

            //Act
            using (new DataSetup(purgeBeforeInsert: false, purgeAfterTest: true, packages: plogDataPackage))
            {
                SeleniumHelper.Login(LoginAs);
                SeleniumHelper.FindAndClick(SeleniumHelper.AutomationId(PupilElements.PupilLogQuickLink));

                SearchCriteria.Search();
                SearchResults.WaitForResults();
                SearchResults.Click(learnerId.ToString());

                // Wait for Report widgets loading to complete
                AutomationSugar.WaitForAjaxCompletion();

                if (UnExpectedProblemDialog.DoesExist())
                {
                    UnExpectedProblemDialog.Create().Dismiss();
                }

                Thread.Sleep(1000);

                BaseSeleniumComponents.WaitUntilDisplayed(new TimeSpan(0, 0, 0, 5), PupilElements.PupilLog.Detail.PupilLogSortFilterButton);

                // Click the 'Sort & Filter' button in the toolbar
                SeleniumHelper.WaitForElementClickableThenClick(PupilElements.PupilLog.Detail.PupilLogSortFilterButton);

                // Filter on General notes
                SeleniumHelper.WaitForElementClickableThenClick(
                    PupilElements.PupilLog.Detail.PupilLogSortFilterPanel.InActiveGeneralNoteButton);

                // Sort by newest first
                SeleniumHelper.WaitForElementClickableThenClick(PupilElements.PupilLog.Detail.PupilLogSortFilterPanel.NewestFirstButton);

                WaitUntilNotesReloaded();

                // Assert count is 3
                var notes =
                    WebContext.WebDriver.FindElements(By.CssSelector(SeleniumHelper.AutomationId("log-timeline-entry")));
                Assert.AreEqual(3, notes.Count, "Notes Count is 3");

                // Edit General Note 2
                var generalNoteTwo = (from webElement in notes
                    where webElement.Text.StartsWith("General Note 2")
                    select webElement).FirstOrDefault();

                Assert.IsNotNull(generalNoteTwo, "General Note 2 is null");                  

                var noteEditBtn = generalNoteTwo.FindChild(By.CssSelector(SeleniumHelper.AutomationId("view/edit_note_and_attachments_button")));
                noteEditBtn.Click();

                Thread.Sleep(1000);

                ControlsHelper.UpdateValue(PupilElements.PupilLog.Detail.Note.NoteText, " edited");
                SeleniumHelper.WaitForElementClickableThenClick(By.CssSelector(PupilElements.PupilLog.Detail.Note.SaveBtn));

                WaitUntilNotesReloaded();

                Thread.Sleep(1000);

                // Assert edited note is first
                notes = WebContext.WebDriver.FindElements(By.CssSelector(SeleniumHelper.AutomationId("log-timeline-entry")));
                var noteHeader = notes.First().FindElement(By.CssSelector(SeleniumHelper.AutomationId("log-note-header-general")));
                Assert.IsNotNull(noteHeader, "Note Header is not null");

                var heading = noteHeader.FindElement(By.CssSelector(SeleniumHelper.AutomationId("log-event-heading")));
                Assert.IsTrue(heading.Text.StartsWith("General Note 2"),
                    string.Format("Heading starts with 'General Note 2' but was actually {0}", heading.Text));
            }
        }

        /// <exception cref="NoSuchElementException">If no element matches the criteria.</exception>
        [WebDriverTest(Enabled = true, Groups = new[] { PupilTestGroups.PupilLog.Page, PupilTestGroups.PupilLog.FilterPanel, PupilTestGroups.Priority.Priority2 }, Browsers = new[] { BrowserDefaults.Chrome })]
        public void Deleting_A_Note_Keeps_Sort_And_Filter()
        {
            //Arrange
            var learnerId = Guid.NewGuid();
            var plogDataPackage = this.BuildDataPackage();
            plogDataPackage.AddBasicLearner(learnerId, Utilities.GenerateRandomString(10, "PlogAddTest-"), "Selenium", dateOfBirth: new DateTime(2005, 05, 30), dateOfAdmission: new DateTime(2012, 10, 03))
                           .AddStandardPupilLogNote(learnerId, "General Note 1")
                           .AddStandardPupilLogNote(learnerId, "General Note 2")
                           .AddStandardPupilLogNote(learnerId, "General Note 3");

            //Act
            using (new DataSetup(purgeBeforeInsert: false, purgeAfterTest: true, packages: plogDataPackage))
            {
                SeleniumHelper.Login(LoginAs);
                AutomationSugar.WaitForAjaxCompletion();
                SeleniumHelper.FindAndClick(SeleniumHelper.AutomationId(PupilElements.PupilLogQuickLink));

                SearchCriteria.Search();
                SearchResults.WaitForResults();
                SearchResults.Click(learnerId.ToString());

                // Wait for Report widgets loading to complete
                AutomationSugar.WaitForAjaxCompletion();

                if (UnExpectedProblemDialog.DoesExist())
                {
                    UnExpectedProblemDialog.Create().Dismiss();
                }

                Thread.Sleep(1000);

                BaseSeleniumComponents.WaitUntilDisplayed(new TimeSpan(0, 0, 0, 5), PupilElements.PupilLog.Detail.PupilLogSortFilterButton);

                // Click the 'Sort & Filter' button in the toolbar
                SeleniumHelper.WaitForElementClickableThenClick(PupilElements.PupilLog.Detail.PupilLogSortFilterButton);

                // Filter on General notes
                SeleniumHelper.WaitForElementClickableThenClick(PupilElements.PupilLog.Detail.PupilLogSortFilterPanel.InActiveGeneralNoteButton);

                // Sort by newest first
                SeleniumHelper.WaitForElementClickableThenClick(PupilElements.PupilLog.Detail.PupilLogSortFilterPanel.OldestFirstButton);

                WaitUntilNotesReloaded();

                // Assert count is 3
                var notes = WebContext.WebDriver.FindElements(By.CssSelector(SeleniumHelper.AutomationId("log-timeline-entry")));
                Assert.AreEqual(3, notes.Count, "Notes count not equal");

                // Delete General Note 1
                var noteDeleteBtn = notes[1].FindChild(By.CssSelector(PupilElements.PupilLog.Detail.Note.DeleteBtn));
                noteDeleteBtn.Click();

                // Confirm delete
                SeleniumHelper.FindAndClick(By.CssSelector(PupilElements.PupilLog.Detail.Note.DeleteConfirmationBtn));

                WaitUntilNotesReloaded();

                // Assert count is now 2
                notes = WebContext.WebDriver.FindElements(By.CssSelector(SeleniumHelper.AutomationId("log-timeline-entry")));
                Assert.AreEqual(2, notes.Count, "Notes count not equal");

                // Assert General Note 2 first
                notes = WebContext.WebDriver.FindElements(By.CssSelector(SeleniumHelper.AutomationId("log-timeline-entry")));
                var noteHeader = notes.First().FindElement(By.CssSelector(SeleniumHelper.AutomationId("log-note-header-general")));
                Assert.IsNotNull(noteHeader, "Note header is null");

                var heading = noteHeader.FindElement(By.CssSelector(SeleniumHelper.AutomationId("log-event-heading")));
                Assert.IsTrue(heading.Text.StartsWith("General Note 1"), "Note header starts with: 'General Note 1'");
            }
        }

        /// <summary>
        /// Wait until the locking mask disappears i.e. the notes have been reloaded
        /// </summary>
        private void WaitUntilNotesReloaded()
        {
            POM.Helper.Wait.WaitForDocumentReady();
            POM.Helper.Wait.WaitForAjaxReady(By.CssSelector(PupilElements.PupilLog.Detail.LockingMask));
            POM.Helper.Wait.WaitForDocumentReady();
        }
    }
}