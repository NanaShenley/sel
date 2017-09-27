using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using NUnit.Framework;
using POM.Components.Common;
using POM.Components.Pupil;
using POM.Helper;
using Pupil.Components.Common;
using Selene.Support.Attributes;
using TestSettings;
using WebDriverRunner.internals;
using SeSugar.Automation;

namespace Pupil.Pupil.Tests
{
    public class PupilLeaverTests
    {
        /// <summary>
        /// TC PU25
        /// Au : An Nguyen
        /// Description: Exercise ability to make a specific set of pupils 'leavers' (in bulk) by allocating to them the same leaving details. (Past Date)
        /// Role: School Administrator
        /// </summary>
        [WebDriverTest(TimeoutSeconds = 1000, Enabled = true, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { PupilTestGroups.PupilRecord.Leaver, PupilTestGroups.Priority.Priority2 }, DataProvider = "TC_PU026_Data")]
        public void TC_PU026_Exercise_ability_to_make_a_specific_set_of_pupils_leavers_in_past(string[] firstPupil, string[] secondPupil,
            string enrolmentStatus, string yearGroup, string className, string dateOfLeaving, string reasonLeaving, string destination)
        {
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);

            #region Pre-Condition

            //Delete first pupil
            AutomationSugar.NavigateMenu("Tasks", "Pupils", "Delete Pupil");
            var deletePupilRecords = new DeletePupilRecordTriplet();
            deletePupilRecords.SearchCriteria.PupilName = String.Format("{0}, {1}", firstPupil[1], firstPupil[0]);
            deletePupilRecords.SearchCriteria.IsCurrent = true;
            deletePupilRecords.SearchCriteria.IsLeaver = true;
            deletePupilRecords.SearchCriteria.IsFuture = true;
            var resultPupils = deletePupilRecords.SearchCriteria.Search();
            var pupilSearchTile = resultPupils.SingleOrDefault(t => t.Name.Equals(String.Format("{0}, {1}", firstPupil[1], firstPupil[0])));
            var deletePupilRecord = pupilSearchTile == null ? new DeletePupilRecordPage() : pupilSearchTile.Click<DeletePupilRecordPage>();

            deletePupilRecord.Delete();

            //Delete second pupil
            deletePupilRecords.SearchCriteria.PupilName = String.Format("{0}, {1}", secondPupil[1], secondPupil[0]);
            resultPupils = deletePupilRecords.SearchCriteria.Search();
            pupilSearchTile = resultPupils.SingleOrDefault(t => t.Name.Equals(String.Format("{0}, {1}", secondPupil[1], secondPupil[0])));
            deletePupilRecord = pupilSearchTile == null ? new DeletePupilRecordPage() : pupilSearchTile.Click<DeletePupilRecordPage>();

            deletePupilRecord.Delete();
            SeleniumHelper.CloseTab("Delete Pupil");

            //Add first pupil
            AutomationSugar.NavigateMenu("Tasks", "Pupils", "Pupil Records");
            var pupilRecords = new PupilRecordTriplet();
            var addNewPupilDialog = pupilRecords.AddNewPupil();
            addNewPupilDialog.Forename = firstPupil[0];
            addNewPupilDialog.SurName = firstPupil[1];
            addNewPupilDialog.Gender = firstPupil[2];
            addNewPupilDialog.DateOfBirth = firstPupil[3];
            var registrationDetailDialog = addNewPupilDialog.Continue();
            registrationDetailDialog.DateOfAdmission = firstPupil[4];
            registrationDetailDialog.EnrolmentStatus = enrolmentStatus;
            registrationDetailDialog.YearGroup = yearGroup;
            registrationDetailDialog.ClassName = className;
            registrationDetailDialog.CreateRecord();
            var confirmRequiredDialog = new POM.Components.Common.ConfirmRequiredDialog();
            confirmRequiredDialog.ClickOk();
            var pupilRecord = PupilRecordPage.Create();
            pupilRecord.SavePupil();

            //Add second pupil
            pupilRecords = new PupilRecordTriplet();
            addNewPupilDialog = pupilRecords.AddNewPupil();
            addNewPupilDialog.Forename = secondPupil[0];
            addNewPupilDialog.SurName = secondPupil[1];
            addNewPupilDialog.Gender = secondPupil[2];
            addNewPupilDialog.DateOfBirth = secondPupil[3];
            registrationDetailDialog = addNewPupilDialog.Continue();
            registrationDetailDialog.DateOfAdmission = secondPupil[4];
            registrationDetailDialog.EnrolmentStatus = enrolmentStatus;
            registrationDetailDialog.YearGroup = yearGroup;
            registrationDetailDialog.ClassName = className;
            registrationDetailDialog.CreateRecord();
            confirmRequiredDialog = new POM.Components.Common.ConfirmRequiredDialog();
            confirmRequiredDialog.ClickOk();
            pupilRecord = PupilRecordPage.Create();
            pupilRecord.SavePupil();

            #endregion Pre-Condition

            #region Test steps

            //Navigate Manage Leaver
            AutomationSugar.NavigateMenu("Tasks", "Pupils", "Manage Leavers");

            //Search
            var manageLeaverTriplet = new ManageLeaversTriplet();
            manageLeaverTriplet.SearchCriteria.Class = className;
            manageLeaverTriplet.SearchCriteria.YearGroup = yearGroup;
            var manageLeaver = manageLeaverTriplet.SearchCriteria.Search<ManageLeaversPage>();

            //Enter value for leaving and leaving pupil
            manageLeaver.DateOfLeaving = dateOfLeaving;
            manageLeaver.ReasonForLeaving = reasonLeaving;
            manageLeaver.DestinationDetails = destination;

            //Click check
            manageLeaver.ManageLeaverTable[String.Format("{0}, {1}", firstPupil[1], firstPupil[0])].ClickCheckBox(0);
            manageLeaver.ManageLeaverTable[String.Format("{0}, {1}", secondPupil[1], secondPupil[0])].ClickCheckBox(0);

            //Save and confirm
            manageLeaver.ClickApplyToSelectedPupil();
            manageLeaver.Save();
            var confirmDialog = new POM.Components.Common.ConfirmRequiredDialog();
            confirmDialog.ClickOk();

            //Navigate to pupil record
            AutomationSugar.NavigateMenu("Tasks", "Pupils", "Pupil Records");

            //Search and verify data of pupil
            //First pupil
            pupilRecords = new PupilRecordTriplet();
            pupilRecords.SearchCriteria.PupilName = String.Format("{0}, {1}", firstPupil[1], firstPupil[0]);
            pupilRecords.SearchCriteria.IsCurrent = true;
            pupilRecords.SearchCriteria.IsFuture = true;
            pupilRecords.SearchCriteria.IsLeaver = true;
            var pupilSearchResults = pupilRecords.SearchCriteria.Search();
            var pupilTile = pupilSearchResults.SingleOrDefault(t => t.Name.Equals(String.Format("{0}, {1}", firstPupil[1], firstPupil[0])));
            pupilRecord = pupilTile.Click<PupilRecordPage>();
            pupilRecord.SelectRegistrationTab();
            var enrolmentHistoryRow = pupilRecord.EnrolmentStatusHistoryTable.Rows.SingleOrDefault(t => t.EnrolmentStatus.Equals(enrolmentStatus));
            Assert.AreEqual(dateOfLeaving, enrolmentHistoryRow.EndDate, "Destination date on pupil record is incorrect");

            //Second Pupil
            pupilRecords = new PupilRecordTriplet();
            pupilRecords.SearchCriteria.PupilName = String.Format("{0}, {1}", secondPupil[1], secondPupil[0]);
            pupilRecords.SearchCriteria.IsCurrent = true;
            pupilRecords.SearchCriteria.IsFuture = true;
            pupilRecords.SearchCriteria.IsLeaver = true;
            pupilSearchResults = pupilRecords.SearchCriteria.Search();
            pupilTile = pupilSearchResults.SingleOrDefault(t => t.Name.Equals(String.Format("{0}, {1}", secondPupil[1], secondPupil[0])));
            pupilRecord = pupilTile.Click<PupilRecordPage>();
            pupilRecord.SelectRegistrationTab();
            enrolmentHistoryRow = pupilRecord.EnrolmentStatusHistoryTable.Rows.SingleOrDefault(t => t.EnrolmentStatus.Equals(enrolmentStatus));
            Assert.AreEqual(dateOfLeaving, enrolmentHistoryRow.EndDate, "Destination date on pupil record is incorrect");
            SeleniumHelper.CloseTab("Pupil Record");

            #endregion Test steps

            #region Post-Condition

            //Delete first pupil
            AutomationSugar.NavigateMenu("Tasks", "Pupils", "Delete Pupil");
            deletePupilRecords = new DeletePupilRecordTriplet();
            deletePupilRecords.SearchCriteria.PupilName = String.Format("{0}, {1}", firstPupil[1], firstPupil[0]);
            deletePupilRecords.SearchCriteria.IsCurrent = true;
            deletePupilRecords.SearchCriteria.IsLeaver = true;
            deletePupilRecords.SearchCriteria.IsFuture = true;
            resultPupils = deletePupilRecords.SearchCriteria.Search();
            pupilSearchTile = resultPupils.SingleOrDefault(t => t.Name.Equals(String.Format("{0}, {1}", firstPupil[1], firstPupil[0])));
            deletePupilRecord = pupilSearchTile == null ? new DeletePupilRecordPage() : pupilSearchTile.Click<DeletePupilRecordPage>();

            deletePupilRecord.Delete();

            //Delete second pupil
            deletePupilRecords.SearchCriteria.PupilName = String.Format("{0}, {1}", secondPupil[1], secondPupil[0]);
            resultPupils = deletePupilRecords.SearchCriteria.Search();
            pupilSearchTile = resultPupils.SingleOrDefault(t => t.Name.Equals(String.Format("{0}, {1}", secondPupil[1], secondPupil[0])));
            deletePupilRecord = pupilSearchTile == null ? new DeletePupilRecordPage() : pupilSearchTile.Click<DeletePupilRecordPage>();

            deletePupilRecord.Delete();

            #endregion Post-Condition
        }

        #region DATA

        public List<object[]> TC_PU026_Data()
        {
            string pattern = "M/d/yyyy";
            string dateOfBirth = DateTime.ParseExact("06/01/2006", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            string dateOfAdmission = DateTime.ParseExact("06/01/2010", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            string dateOfLeaving = SeleniumHelper.GetFirstDayOfWeek(DateTime.Now.Add(TimeSpan.FromDays(7))).ToString(pattern);

            string random = SeleniumHelper.GenerateRandomNumberUsingDateTime();
            string randomCharacter = SeleniumHelper.GenerateRandomString(6);

            var res = new List<Object[]>
            {
                new object[]
                {
                    new string[]{"AFutureTest"+randomCharacter+random, "Aaa", "Male", dateOfBirth, dateOfAdmission},
                    new string[]{"AFutureTest"+randomCharacter+random, "Aab", "Female", dateOfBirth, dateOfAdmission},
                    "Single Registration", "Year 5", "5A", dateOfLeaving, "Not Known", "Kings Secondary School"
                }
            };
            return res;
        }

        #endregion DATA
    }
}