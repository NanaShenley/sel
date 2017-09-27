using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using NUnit.Framework;
using OpenQA.Selenium;
using POM.Components.Pupil;
using POM.Components.Pupil.Pages;
using Pupil.Components;
using Pupil.Components.Common;
using TestSettings;
using WebDriverRunner.internals;
using POM.Helper;
using Pupil.Data;
using Pupil.Data.Entities;
using Selene.Support.Attributes;
using SeSugar;
using SeSugar.Automation;

namespace Pupil.ClassLog.Tests
{
    public class ClassLogPupilGridTests
    {
        private const string ClassLogFeature = "Class Log";

       // [WebDriverTest(Enabled = true, Browsers = new[] { BrowserDefaults.Chrome },   Groups = new[] { PupilTestGroups.ClassLog.Page, PupilTestGroups.ClassLog.ClassLogNote, PupilTestGroups.Priority.Priority2 })]
        public void CanAddANoteToOneOrMoreSelectedPupilsWithinClassLog()
        {
           
            //Arrange
            ClassTeacher teacher = ClassLogData.GetClassTeacherWithClassAndLearners();
            AuthorisedUser authUser =  ClassLogData.GetAuthorisedUserDetailsForClassTeacherUser(TestDefaults.Default.ClassTeacher);
            ClassLogData.UpdateClassTeacherWithInitialAuthUserValues(authUser, TestDefaults.Default.ClassTeacher);
            ClassLogData.UpdateClassTeacherUserWithStaffDetails(teacher, TestDefaults.Default.ClassTeacher);

            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.ClassTeacher, enabledFeatures: ClassLogFeature);
            Wait.WaitForDocumentReady();
            //Act
            var classLogNavigate = new ClassLogNavigation();
         
            Wait.WaitForDocumentReady();
            classLogNavigate.NavigateToPupilClassLogFromMenu();
            Wait.WaitForDocumentReady();
            ClassLogPage clogPage = new ClassLogPage();
            // select the first two pupils
            List<string> pupilList = clogPage.SelectFristTwoPupils();
            Wait.WaitForDocumentReady();

            // add a general note
            // select a note
            clogPage.SelectAddNoteItem();
            Wait.WaitForDocumentReady();
            var noteDialog = clogPage.SelelectGeneralNoteType();
            Wait.WaitForDocumentReady();
            string randomTitle = Utilities.GenerateRandomString(5, "General Notice");
            // Create note General
            // populate text
            noteDialog.Note = "General Notice";
            noteDialog.Title = randomTitle;
            noteDialog.SubCategory = "General";
            noteDialog.PinThisNote = false;
            noteDialog.ClickSave();
            Wait.WaitForDocumentReady();
            // make sure our submenu is  visible

            List<bool> actualResults = new List<bool>();

            foreach (var pupil in pupilList)
            {
                accessPupilLog(pupil);
                AutomationSugar.WaitForAjaxCompletion();
                clogPage.SelectPupil();
                AutomationSugar.WaitForAjaxCompletion();
                var notes = clogPage.TimeLine;
                AutomationSugar.WaitForAjaxCompletion();

                var note = notes.GetNote(randomTitle);
                AutomationSugar.WaitForAjaxCompletion();
                if (note != null)
                {
                    if (note.NoteName != null)
                    {
                        actualResults.Add(true);
                    }

                    note.Delete();
                }
            }

            Assert.AreEqual(2.0, (double)actualResults.Count);

        }

        private void accessPupilLog(string searchPupil)
        {

            // Navigate to Pupil Log
            AutomationSugar.NavigateMenu("Tasks", "Pupils", "Pupil Log");
            AutomationSugar.WaitForAjaxCompletion();

            // Search all pupils
            Thread.Sleep(5000);
            var pupilLogTriplet = new PupilLogTriplet();
            pupilLogTriplet.SearchCriteria.PupilName = searchPupil;
            pupilLogTriplet.SearchCriteria.IsCurrent = true;
            pupilLogTriplet.SearchCriteria.IsLeaver = false;
            var pupilResuls = pupilLogTriplet.SearchCriteria.Search();
        }

    }
}
