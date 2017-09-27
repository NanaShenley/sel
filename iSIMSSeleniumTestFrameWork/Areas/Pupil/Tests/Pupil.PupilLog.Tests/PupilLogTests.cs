using System;
using System.Threading;
using NUnit.Framework;
using POM.Components.Pupil;
using POM.Helper;
using Pupil.Components.Common;
using Pupil.Data;
using SeSugar;
using SeSugar.Data;
using TestSettings;
using Selene.Support.Attributes;
using SeSugar.Automation;

namespace Pupil.PupilLog.Tests
{
    public class PupilLogTests
    {
        //****moved from Logigear Pupil.Pupil.Tests****

        //private SeleniumHelper.iSIMSUserType LoginAs = SeleniumHelper.iSIMSUserType.ClassTeacher;
        private readonly SeleniumHelper.iSIMSUserType LoginAs = SeleniumHelper.iSIMSUserType.SchoolAdministrator;

        /// <summary>
        /// Author: Luong.Mai
        /// Description: PU-034 :
        /// </summary>
        [WebDriverTest(TimeoutSeconds = 1400, Enabled = true, Browsers = new[] { BrowserDefaults.Chrome },
            Groups = new[] { PupilTestGroups.PupilLog.Page, PupilTestGroups.PupilLog.ActionLinks, PupilTestGroups.Priority.Priority2, "sinu" })]
        public void PupilLinks_WithinPupilLogExist()
        {
            // Arrange
            // Add a learner record for the learner
            var learnerId = Guid.NewGuid();
            var plogDataPackage = this.BuildDataPackage();
            var forename = "PlogSelenium";
            var surname = Utilities.GenerateRandomString(10);
            var pupilName = string.Format("{0} {1}", surname, forename);

            plogDataPackage.AddBasicLearner(learnerId, surname, forename, dateOfBirth: new DateTime(2005, 05, 30), dateOfAdmission: new DateTime(2012, 10, 03))
                .AddStandardPupilLogNote(learnerId, "General Note 1");

            #region STEPS

            using (new DataSetup(purgeBeforeInsert: false, purgeAfterTest: true, packages: plogDataPackage))
            {
                // Login
                SeleniumHelper.Login(LoginAs); //SeleniumHelper.iSIMSUserType.ClassTeacher);

                // Navigate to Pupil Log
                AutomationSugar.NavigateMenu("Tasks", "Pupils", "Pupil Log");
                AutomationSugar.WaitForAjaxCompletion();

                // Search pupil
                var pupilLogTriplet = new PupilLogTriplet();
                pupilLogTriplet.SearchCriteria.PupilName = pupilName;
                pupilLogTriplet.SearchCriteria.IsCurrent = true;
                pupilLogTriplet.SearchCriteria.IsLeaver = false;
                var pupilResults = pupilLogTriplet.SearchCriteria.Search();

                // Select pupil
                var pupilLogDetailPage = pupilResults[0].Click<PupilLogDetailPage>();

                AutomationSugar.WaitForAjaxCompletion();
                // Close error window due to report widget loading issue
                if (UnExpectedProblemDialog.DoesExist())
                {
                    UnExpectedProblemDialog.Create().Dismiss();
                }

                // Link to Pupil Record via Action panel
                var pupilRecordPage = SeleniumHelper.NavigateViaAction<PupilRecordPage>("Pupil Record");
                AutomationSugar.WaitForAjaxCompletion();

                // Confirm the pupil record screen is displayed for same pupil as selected in the 'Pupil Log'
                Assert.AreEqual(true, pupilRecordPage.LegalForeName.Equals(forename)
                                      && pupilRecordPage.LegalSurname.Equals(surname),
                    "A pupil record screen isn't displayed for same pupil");

                // Return to Pupil Log
                AutomationSugar.NavigateMenu("Tasks", "Pupils", "Pupil Log");

                // Wait for Report widgets loading to complete
                AutomationSugar.WaitForAjaxCompletion();

                // Close error window due to report widget loading issue
                if (UnExpectedProblemDialog.DoesExist())
                {
                    UnExpectedProblemDialog.Create().Dismiss();
                }

                // Search pupil
                pupilLogTriplet = new PupilLogTriplet();
                pupilLogTriplet.SearchCriteria.PupilName = pupilName;
                pupilLogTriplet.SearchCriteria.IsCurrent = true;
                pupilLogTriplet.SearchCriteria.IsLeaver = false;
                pupilResults = pupilLogTriplet.SearchCriteria.Search();

                // Select pupil
                //Thread.Sleep(5000);
                pupilResults[0].Click<PupilLogDetailPage>();

                AutomationSugar.WaitForAjaxCompletion();
                // Close error window due to report widget loading issue
                if (UnExpectedProblemDialog.DoesExist())
                {
                    UnExpectedProblemDialog.Create().Dismiss();
                }

                // Link to SEN Details via Action panel
                SeleniumHelper.NavigateViaAction<SenRecordDetailPage>("SEN Record");
                


                // Close SEN Record tab
                SeleniumHelper.CloseTab("SEN Record");

                // Verify the pupil log is displayed
                pupilLogDetailPage = new PupilLogDetailPage();

                // Wait for Report widgets loading to complete
                AutomationSugar.WaitForAjaxCompletion();

                if (UnExpectedProblemDialog.DoesExist())
                {
                    UnExpectedProblemDialog.Create().Dismiss();
                }

                Assert.AreEqual(true, pupilLogDetailPage.PupilSurname.Equals(surname)
                                      && pupilLogDetailPage.PupilForename.Equals(forename), "Pupil Log isn't displayed.");
            }

            #endregion
        }

        /// <summary>
        /// Author: Luong.Mai
        /// Description: PU-036a :
        /// </summary>
        [WebDriverTest(TimeoutSeconds = 1200, Enabled = true, Browsers = new[] { BrowserDefaults.Chrome },
            Groups = new[] { PupilTestGroups.PupilLog.Page, PupilTestGroups.PupilLog.CreateNotes, PupilTestGroups.Priority.Priority2, })]
        public void Exercise_ability_to_create_notes()
        {
            #region Pre-Condition: Create a new pupil for test

            var surname = Utilities.GenerateRandomString(10, "Plog");
            var forename = Utilities.GenerateRandomString(10, "Plog");
            var dataPackage = this.BuildDataPackage();
            var learnerId = Guid.NewGuid();
            dataPackage.AddBasicLearner(learnerId, surname, forename, new DateTime(2011, 02, 02), new DateTime(2015, 02, 02));

            #endregion

            using (new DataSetup(purgeBeforeInsert: false, purgeAfterTest: true, packages: dataPackage))
            {
                try
                {
                    // Login as school admin
                    SeleniumHelper.Login(LoginAs);

                    // Navigate to Pupil Log
                    AutomationSugar.NavigateMenu("Tasks", "Pupils", "Pupil Log");
                    AutomationSugar.WaitForAjaxCompletion();

                    // Search all pupils
                    var pupilLogTriplet = new PupilLogTriplet();
                    pupilLogTriplet.SearchCriteria.PupilName = surname + ", " + forename;
                    pupilLogTriplet.SearchCriteria.IsCurrent = true;
                    pupilLogTriplet.SearchCriteria.IsLeaver = false;
                    var pupilResuls = pupilLogTriplet.SearchCriteria.Search();

                    // Select pupils
                    var pupilLogDetailPage = pupilResuls[0].Click<PupilLogDetailPage>();

                    // Wait for Report widgets loading to complete
                    AutomationSugar.WaitForAjaxCompletion();

                    if (UnExpectedProblemDialog.DoesExist())
                    {
                        UnExpectedProblemDialog.Create().Dismiss();
                    }

                    // Expand the 'Create Notes' selector, click on type 'General'.
                    pupilLogDetailPage.ClickCreateNote();
                    var noteDialog = pupilLogDetailPage.SelelectGeneralNoteType();

                    // Create note General
                    noteDialog.Note = "General Notice";
                    noteDialog.Title = "General Notice";
                    noteDialog.SubCategory = "General";
                    noteDialog.PinThisNote = false;
                    noteDialog.ClickSave();

                    // Verify that creating note is successfully
                    var notes = pupilLogDetailPage.TimeLine;
                    Assert.AreNotEqual(null, notes["General Notice"], "Creating note is unsuccessfully.");

                    // Expand the 'Create Notes' selector, click on type 'Assessment'.
                    pupilLogDetailPage.ClickCreateNote();
                    noteDialog = pupilLogDetailPage.SelelectAssessmentNoteType();

                    // Create note Assessment
                    noteDialog.Note = "Assessment Notice";
                    noteDialog.Title = "Assessment Notice";
                    noteDialog.SubCategory = "English";
                    noteDialog.PinThisNote = false;
                    noteDialog.ClickSave();

                    // Verify that creating note is successfully
                    notes = pupilLogDetailPage.TimeLine;
                    Assert.AreNotEqual(null, notes["Assessment Notice"], "Creating note is unsuccessfully.");

                    // Expand the 'Create Notes' selector, click on type 'Achievement'.
                    pupilLogDetailPage.ClickCreateNote();
                    noteDialog = pupilLogDetailPage.SelelectSENNoteType();

                    // Create SEN note 
                    noteDialog.Note = "SEN Notice";
                    noteDialog.Title = "SEN Notice";
                    noteDialog.SubCategory = "General";
                    noteDialog.PinThisNote = false;
                    noteDialog.ClickSave();

                    // Verify that creating note is successfully
                    notes = pupilLogDetailPage.TimeLine;
                    Assert.AreNotEqual(null, notes["SEN Notice"], "Creating note is unsuccessfully.");
                }
                finally
                {
                    // Teardown
                    PurgeLinkedData.DeletePupilLogNotesForLearner(learnerId);
                }
            }
        }
    }
}