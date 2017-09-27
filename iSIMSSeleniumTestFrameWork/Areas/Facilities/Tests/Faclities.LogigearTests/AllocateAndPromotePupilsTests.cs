using NUnit.Framework;
using POM.Components.HomePages;
using POM.Components.SchoolGroups;
using POM.Components.SchoolManagement;
using POM.Helper;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestSettings;
using WebDriverRunner.internals;
using POM.Components.Pupil;
using POM.Components.Admission;
using Selene.Support.Attributes;

namespace Faclities.LogigearTests
{
    public class AllocateAndPromotePupilsTests
    {
        /// <summary>
        /// TC AP-01
        /// Au : An Nguyen
        /// Description: Exercise the ability to 'Add New Current Pupils' with only 'Year Group' membership and with a 'Date of Admission' that falls within the dates of the Current Academic Year.
        /// Role: School Administrator
        /// </summary>
        [WebDriverTest(TimeoutSeconds = 1800, Enabled = false, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { "g1" }, DataProvider = "TC_AP01_Data")]
        public void TC_AP01_Exercise_the_ability_to_Add_New_Current_Pupils(string[] pupil1, string[] pupil2, string[] pupil3, string[] pupil4, string[] pupil5,
                    string[] pupil6, string[] pupil7, string[] pupil8, string[] pupil9, string[] pupil10, string dateOfAdmission, string yearGroup)
        {
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);

            #region Test steps

            //Navigate to Adminssion Settings
            SeleniumHelper.NavigateMenu("Tasks", "Admission Settings", "Admission Settings");
            var admissionSetting = new AdmissionSettingsPage();

            //Edit last admission number
            admissionSetting.LastAdmissionNumber = admissionSetting.GetNextAvailableAdmissionNumber();
            admissionSetting.Save();
            Assert.AreEqual(true, admissionSetting.IsSuccessMessageDisplay(), "Can not change Last Admission Number");

            //Navigate to Pupil Records
            SeleniumHelper.NavigateQuickLink("Pupil Records");
            var pupilRecords = new PupilRecordTriplet();

            //Add the first pupil
            var addNewPupilDialog = pupilRecords.AddNewPupil();
            addNewPupilDialog.Forename = pupil1[0];
            addNewPupilDialog.SurName = pupil1[1];
            addNewPupilDialog.Gender = pupil1[2];
            addNewPupilDialog.DateOfBirth = pupil1[3];
            var registrationDetailDialog = addNewPupilDialog.Continue();
            registrationDetailDialog.DateOfAdmission = dateOfAdmission;
            registrationDetailDialog.YearGroup = yearGroup;
            registrationDetailDialog.CreateRecord();
            var confirmRequiredDialog = new POM.Components.Common.ConfirmRequiredDialog();
            confirmRequiredDialog.ClickOk();

            //Save the first pupil
            var pupilRecord = PupilRecordPage.Create();
            pupilRecord.SavePupil();

            //Add the second pupil
            pupilRecords.Refresh();
            addNewPupilDialog = pupilRecords.AddNewPupil();
            addNewPupilDialog.Forename = pupil2[0];
            addNewPupilDialog.SurName = pupil2[1];
            addNewPupilDialog.Gender = pupil2[2];
            addNewPupilDialog.DateOfBirth = pupil2[3];
            registrationDetailDialog = addNewPupilDialog.Continue();
            registrationDetailDialog.DateOfAdmission = dateOfAdmission;
            registrationDetailDialog.YearGroup = yearGroup;
            registrationDetailDialog.CreateRecord();
            confirmRequiredDialog = new POM.Components.Common.ConfirmRequiredDialog();
            confirmRequiredDialog.ClickOk();

            //Save the second pupil
            pupilRecord = PupilRecordPage.Create();
            pupilRecord.SavePupil();

            //Add the third pupil
            pupilRecords.Refresh();
            addNewPupilDialog = pupilRecords.AddNewPupil();
            addNewPupilDialog.Forename = pupil3[0];
            addNewPupilDialog.SurName = pupil3[1];
            addNewPupilDialog.Gender = pupil3[2];
            addNewPupilDialog.DateOfBirth = pupil3[3];
            registrationDetailDialog = addNewPupilDialog.Continue();
            registrationDetailDialog.DateOfAdmission = dateOfAdmission;
            registrationDetailDialog.YearGroup = yearGroup;
            registrationDetailDialog.CreateRecord();
            confirmRequiredDialog = new POM.Components.Common.ConfirmRequiredDialog();
            confirmRequiredDialog.ClickOk();

            //Save the third pupil
            pupilRecord = PupilRecordPage.Create();
            pupilRecord.SavePupil();

            //Add the fourth pupil
            pupilRecords.Refresh();
            addNewPupilDialog = pupilRecords.AddNewPupil();
            addNewPupilDialog.Forename = pupil4[0];
            addNewPupilDialog.SurName = pupil4[1];
            addNewPupilDialog.Gender = pupil4[2];
            addNewPupilDialog.DateOfBirth = pupil4[3];
            registrationDetailDialog = addNewPupilDialog.Continue();
            registrationDetailDialog.DateOfAdmission = dateOfAdmission;
            registrationDetailDialog.YearGroup = yearGroup;
            registrationDetailDialog.CreateRecord();
            confirmRequiredDialog = new POM.Components.Common.ConfirmRequiredDialog();
            confirmRequiredDialog.ClickOk();

            //Save the fourth pupil
            pupilRecord = PupilRecordPage.Create();
            pupilRecord.SavePupil();

            //Add the fifth pupil
            pupilRecords.Refresh();
            addNewPupilDialog = pupilRecords.AddNewPupil();
            addNewPupilDialog.Forename = pupil5[0];
            addNewPupilDialog.SurName = pupil5[1];
            addNewPupilDialog.Gender = pupil5[2];
            addNewPupilDialog.DateOfBirth = pupil5[3];
            registrationDetailDialog = addNewPupilDialog.Continue();
            registrationDetailDialog.DateOfAdmission = dateOfAdmission;
            registrationDetailDialog.YearGroup = yearGroup;
            registrationDetailDialog.CreateRecord();
            confirmRequiredDialog = new POM.Components.Common.ConfirmRequiredDialog();
            confirmRequiredDialog.ClickOk();

            //Save the fifth pupil
            pupilRecord = PupilRecordPage.Create();
            pupilRecord.SavePupil();

            //Add the sixth pupil
            pupilRecords.Refresh();
            addNewPupilDialog = pupilRecords.AddNewPupil();
            addNewPupilDialog.Forename = pupil6[0];
            addNewPupilDialog.SurName = pupil6[1];
            addNewPupilDialog.Gender = pupil6[2];
            addNewPupilDialog.DateOfBirth = pupil6[3];
            registrationDetailDialog = addNewPupilDialog.Continue();
            registrationDetailDialog.DateOfAdmission = dateOfAdmission;
            registrationDetailDialog.YearGroup = yearGroup;
            registrationDetailDialog.CreateRecord();
            confirmRequiredDialog = new POM.Components.Common.ConfirmRequiredDialog();
            confirmRequiredDialog.ClickOk();

            //Save the sixth pupil
            pupilRecord = PupilRecordPage.Create();
            pupilRecord.SavePupil();

            //Add the seventh pupil
            pupilRecords.Refresh();
            addNewPupilDialog = pupilRecords.AddNewPupil();
            addNewPupilDialog.Forename = pupil7[0];
            addNewPupilDialog.SurName = pupil7[1];
            addNewPupilDialog.Gender = pupil7[2];
            addNewPupilDialog.DateOfBirth = pupil7[3];
            registrationDetailDialog = addNewPupilDialog.Continue();
            registrationDetailDialog.DateOfAdmission = dateOfAdmission;
            registrationDetailDialog.YearGroup = yearGroup;
            registrationDetailDialog.CreateRecord();
            confirmRequiredDialog = new POM.Components.Common.ConfirmRequiredDialog();
            confirmRequiredDialog.ClickOk();

            //Save the seventh pupil
            pupilRecord = PupilRecordPage.Create();
            pupilRecord.SavePupil();

            //Add the eighth pupil
            pupilRecords.Refresh();
            addNewPupilDialog = pupilRecords.AddNewPupil();
            addNewPupilDialog.Forename = pupil8[0];
            addNewPupilDialog.SurName = pupil8[1];
            addNewPupilDialog.Gender = pupil8[2];
            addNewPupilDialog.DateOfBirth = pupil8[3];
            registrationDetailDialog = addNewPupilDialog.Continue();
            registrationDetailDialog.DateOfAdmission = dateOfAdmission;
            registrationDetailDialog.YearGroup = yearGroup;
            registrationDetailDialog.CreateRecord();
            confirmRequiredDialog = new POM.Components.Common.ConfirmRequiredDialog();
            confirmRequiredDialog.ClickOk();

            //Save the eighth pupil
            pupilRecord = PupilRecordPage.Create();
            pupilRecord.SavePupil();

            //Add the nineth pupil
            pupilRecords.Refresh();
            addNewPupilDialog = pupilRecords.AddNewPupil();
            addNewPupilDialog.Forename = pupil9[0];
            addNewPupilDialog.SurName = pupil9[1];
            addNewPupilDialog.Gender = pupil9[2];
            addNewPupilDialog.DateOfBirth = pupil9[3];
            registrationDetailDialog = addNewPupilDialog.Continue();
            registrationDetailDialog.DateOfAdmission = dateOfAdmission;
            registrationDetailDialog.YearGroup = yearGroup;
            registrationDetailDialog.CreateRecord();
            confirmRequiredDialog = new POM.Components.Common.ConfirmRequiredDialog();
            confirmRequiredDialog.ClickOk();

            //Save the nineth pupil
            pupilRecord = PupilRecordPage.Create();
            pupilRecord.SavePupil();

            //Add the tenth pupil
            pupilRecords.Refresh();
            addNewPupilDialog = pupilRecords.AddNewPupil();
            addNewPupilDialog.Forename = pupil10[0];
            addNewPupilDialog.SurName = pupil10[1];
            addNewPupilDialog.Gender = pupil10[2];
            addNewPupilDialog.DateOfBirth = pupil10[3];
            registrationDetailDialog = addNewPupilDialog.Continue();
            registrationDetailDialog.DateOfAdmission = dateOfAdmission;
            registrationDetailDialog.YearGroup = yearGroup;
            registrationDetailDialog.CreateRecord();
            confirmRequiredDialog = new POM.Components.Common.ConfirmRequiredDialog();
            confirmRequiredDialog.ClickOk();

            //Save the tenth pupil
            pupilRecord = PupilRecordPage.Create();
            pupilRecord.SavePupil();

            //Search pupils by year group
            pupilRecords.Refresh();
            pupilRecords.SearchCriteria.IsCurrent = true;
            pupilRecords.SearchCriteria.IsFuture = false;
            pupilRecords.SearchCriteria.IsLeaver = false;
            pupilRecords.SearchCriteria.ClickSearchAdvanced(true);
            pupilRecords.SearchCriteria.YearGroup = yearGroup;
            var pupilSearchResult = pupilRecords.SearchCriteria.Search();

            //Verify the first pupil
            var pupilTile = pupilSearchResult.FirstOrDefault(t => t.Name.Equals(String.Format("{0}, {1}", pupil1[1], pupil1[0])));
            Assert.AreNotEqual(null, pupilTile, "Can not search the first pupil by year group");

            //Verify the second pupil
            pupilTile = pupilSearchResult.FirstOrDefault(t => t.Name.Equals(String.Format("{0}, {1}", pupil2[1], pupil2[0])));
            Assert.AreNotEqual(null, pupilTile, "Can not search the second pupil by year group");

            //Verify the third pupil
            pupilTile = pupilSearchResult.FirstOrDefault(t => t.Name.Equals(String.Format("{0}, {1}", pupil3[1], pupil3[0])));
            Assert.AreNotEqual(null, pupilTile, "Can not search the third pupil by year group");

            //Verify the fourth pupil
            pupilTile = pupilSearchResult.FirstOrDefault(t => t.Name.Equals(String.Format("{0}, {1}", pupil4[1], pupil4[0])));
            Assert.AreNotEqual(null, pupilTile, "Can not search the fourth pupil by year group");

            //Verify the fifth pupil
            pupilTile = pupilSearchResult.FirstOrDefault(t => t.Name.Equals(String.Format("{0}, {1}", pupil5[1], pupil5[0])));
            Assert.AreNotEqual(null, pupilTile, "Can not search the fifth pupil by year group");

            //Verify the sixth pupil
            pupilTile = pupilSearchResult.FirstOrDefault(t => t.Name.Equals(String.Format("{0}, {1}", pupil6[1], pupil6[0])));
            Assert.AreNotEqual(null, pupilTile, "Can not search the sixth pupil by year group");

            //Verify the seventh pupil
            pupilTile = pupilSearchResult.FirstOrDefault(t => t.Name.Equals(String.Format("{0}, {1}", pupil7[1], pupil7[0])));
            Assert.AreNotEqual(null, pupilTile, "Can not search the seventh pupil by year group");

            //Verify the eighth pupil
            pupilTile = pupilSearchResult.FirstOrDefault(t => t.Name.Equals(String.Format("{0}, {1}", pupil8[1], pupil8[0])));
            Assert.AreNotEqual(null, pupilTile, "Can not search the eighth pupil by year group");

            //Verify the nineth pupil
            pupilTile = pupilSearchResult.FirstOrDefault(t => t.Name.Equals(String.Format("{0}, {1}", pupil9[1], pupil9[0])));
            Assert.AreNotEqual(null, pupilTile, "Can not search the nineth pupil by year group");

            //Verify the tenth pupil
            pupilTile = pupilSearchResult.FirstOrDefault(t => t.Name.Equals(String.Format("{0}, {1}", pupil10[1], pupil10[0])));
            Assert.AreNotEqual(null, pupilTile, "Can not search the tenth pupil by year group");

            #endregion

            #region Post-Condition : Delete 10 pupil

            SeleniumHelper.NavigateMenu("Tasks", "Pupils", "Delete Pupil");
            var deletePupilRecords = new DeletePupilRecordTriplet();
            deletePupilRecords.SearchCriteria.IsCurrent = true;
            deletePupilRecords.SearchCriteria.IsLeaver = true;
            deletePupilRecords.SearchCriteria.IsFuture = true;

            //Delete the first pupil
            deletePupilRecords.SearchCriteria.PupilName = String.Format("{0}, {1}", pupil1[1], pupil1[0]);
            var deleteResultPupils = deletePupilRecords.SearchCriteria.Search();
            var deletePupilSearchTile = deleteResultPupils.FirstOrDefault(t => t.Name.Equals(String.Format("{0}, {1}", pupil1[1], pupil1[0])));
            var deletePupilRecord = deletePupilSearchTile.Click<DeletePupilRecordPage>();
            deletePupilRecord.Delete();

            //Delete the second pupil
            deletePupilRecords.Refresh();
            deletePupilRecords.SearchCriteria.PupilName = String.Format("{0}, {1}", pupil2[1], pupil2[0]);
            deleteResultPupils = deletePupilRecords.SearchCriteria.Search();
            deletePupilSearchTile = deleteResultPupils.FirstOrDefault(t => t.Name.Equals(String.Format("{0}, {1}", pupil2[1], pupil2[0])));
            deletePupilRecord = deletePupilSearchTile.Click<DeletePupilRecordPage>();
            deletePupilRecord.Delete();

            //Delete the third pupil
            deletePupilRecords.Refresh();
            deletePupilRecords.SearchCriteria.PupilName = String.Format("{0}, {1}", pupil3[1], pupil3[0]);
            deleteResultPupils = deletePupilRecords.SearchCriteria.Search();
            deletePupilSearchTile = deleteResultPupils.FirstOrDefault(t => t.Name.Equals(String.Format("{0}, {1}", pupil3[1], pupil3[0])));
            deletePupilRecord = deletePupilSearchTile.Click<DeletePupilRecordPage>();
            deletePupilRecord.Delete();

            //Delete the fourth pupil
            deletePupilRecords.Refresh();
            deletePupilRecords.SearchCriteria.PupilName = String.Format("{0}, {1}", pupil4[1], pupil4[0]);
            deleteResultPupils = deletePupilRecords.SearchCriteria.Search();
            deletePupilSearchTile = deleteResultPupils.FirstOrDefault(t => t.Name.Equals(String.Format("{0}, {1}", pupil4[1], pupil4[0])));
            deletePupilRecord = deletePupilSearchTile.Click<DeletePupilRecordPage>();
            deletePupilRecord.Delete();

            //Delete the fifth pupil
            deletePupilRecords.Refresh();
            deletePupilRecords.SearchCriteria.PupilName = String.Format("{0}, {1}", pupil5[1], pupil5[0]);
            deleteResultPupils = deletePupilRecords.SearchCriteria.Search();
            deletePupilSearchTile = deleteResultPupils.FirstOrDefault(t => t.Name.Equals(String.Format("{0}, {1}", pupil5[1], pupil5[0])));
            deletePupilRecord = deletePupilSearchTile.Click<DeletePupilRecordPage>();
            deletePupilRecord.Delete();

            //Delete the sixth pupil
            deletePupilRecords.Refresh();
            deletePupilRecords.SearchCriteria.PupilName = String.Format("{0}, {1}", pupil6[1], pupil6[0]);
            deleteResultPupils = deletePupilRecords.SearchCriteria.Search();
            deletePupilSearchTile = deleteResultPupils.FirstOrDefault(t => t.Name.Equals(String.Format("{0}, {1}", pupil6[1], pupil6[0])));
            deletePupilRecord = deletePupilSearchTile.Click<DeletePupilRecordPage>();
            deletePupilRecord.Delete();

            //Delete the seventh pupil
            deletePupilRecords.Refresh();
            deletePupilRecords.SearchCriteria.PupilName = String.Format("{0}, {1}", pupil7[1], pupil7[0]);
            deleteResultPupils = deletePupilRecords.SearchCriteria.Search();
            deletePupilSearchTile = deleteResultPupils.FirstOrDefault(t => t.Name.Equals(String.Format("{0}, {1}", pupil7[1], pupil7[0])));
            deletePupilRecord = deletePupilSearchTile.Click<DeletePupilRecordPage>();
            deletePupilRecord.Delete();

            //Delete the eighth pupil
            deletePupilRecords.Refresh();
            deletePupilRecords.SearchCriteria.PupilName = String.Format("{0}, {1}", pupil8[1], pupil8[0]);
            deleteResultPupils = deletePupilRecords.SearchCriteria.Search();
            deletePupilSearchTile = deleteResultPupils.FirstOrDefault(t => t.Name.Equals(String.Format("{0}, {1}", pupil8[1], pupil8[0])));
            deletePupilRecord = deletePupilSearchTile.Click<DeletePupilRecordPage>();
            deletePupilRecord.Delete();

            //Delete the nineth pupil
            deletePupilRecords.Refresh();
            deletePupilRecords.SearchCriteria.PupilName = String.Format("{0}, {1}", pupil9[1], pupil9[0]);
            deleteResultPupils = deletePupilRecords.SearchCriteria.Search();
            deletePupilSearchTile = deleteResultPupils.FirstOrDefault(t => t.Name.Equals(String.Format("{0}, {1}", pupil9[1], pupil9[0])));
            deletePupilRecord = deletePupilSearchTile.Click<DeletePupilRecordPage>();
            deletePupilRecord.Delete();

            //Delete the tenth pupil
            deletePupilRecords.Refresh();
            deletePupilRecords.SearchCriteria.PupilName = String.Format("{0}, {1}", pupil10[1], pupil10[0]);
            deleteResultPupils = deletePupilRecords.SearchCriteria.Search();
            deletePupilSearchTile = deleteResultPupils.FirstOrDefault(t => t.Name.Equals(String.Format("{0}, {1}", pupil10[1], pupil10[0])));
            deletePupilRecord = deletePupilSearchTile.Click<DeletePupilRecordPage>();
            deletePupilRecord.Delete();

            #endregion
        }

        /// <summary>
        /// TC AP-02
        /// Au : An Nguyen
        /// Description: Exercise ability to 'Allocate Pupils to Groups' whilst the pupils are already in a 'Year Group' but not in a Class.
        /// Role: School Administrator
        /// Status : Issue Current Class does not update
        /// </summary>
        [WebDriverTest(TimeoutSeconds = 2000, Enabled = false, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { "g1" }, DataProvider = "TC_AP02_Data")]
        public void TC_AP02_Exercise_ability_to_Allocate_Pupils_to_Groups_whilst_the_pupils_are_already_in_a_Year_Group_but_not_in_a_Class(string[] pupil1, string[] pupil2, string[] pupil3, string[] pupil4, string[] pupil5,
                    string[] pupil6, string[] pupil7, string[] pupil8, string[] pupil9, string[] pupil10, string dateOfAdmission, string yearGroup, string effectiveDate, string className)
        {
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);

            #region Pre-condition : Create 10 pupils

            //Navigate to Adminssion Settings
            SeleniumHelper.NavigateMenu("Tasks", "Admission Settings", "Admission Settings");
            var admissionSetting = new AdmissionSettingsPage();

            //Edit last admission number
            admissionSetting.LastAdmissionNumber = admissionSetting.GetNextAvailableAdmissionNumber();
            admissionSetting.Save();
            Assert.AreEqual(true, admissionSetting.IsSuccessMessageDisplay(), "Can not change Last Admission Number");

            //Navigate to Pupil Records
            SeleniumHelper.NavigateQuickLink("Pupil Records");
            var pupilRecords = new PupilRecordTriplet();

            //Add the first pupil
            var addNewPupilDialog = pupilRecords.AddNewPupil();
            addNewPupilDialog.Forename = pupil1[0];
            addNewPupilDialog.SurName = pupil1[1];
            addNewPupilDialog.Gender = pupil1[2];
            addNewPupilDialog.DateOfBirth = pupil1[3];
            var registrationDetailDialog = addNewPupilDialog.Continue();
            registrationDetailDialog.DateOfAdmission = dateOfAdmission;
            registrationDetailDialog.YearGroup = yearGroup;
            registrationDetailDialog.CreateRecord();
            var confirmRequiredDialog = new POM.Components.Common.ConfirmRequiredDialog();
            confirmRequiredDialog.ClickOk();

            //Save the first pupil
            var pupilRecord = PupilRecordPage.Create();
            pupilRecord.SavePupil();

            //Add the second pupil
            pupilRecords.Refresh();
            addNewPupilDialog = pupilRecords.AddNewPupil();
            addNewPupilDialog.Forename = pupil2[0];
            addNewPupilDialog.SurName = pupil2[1];
            addNewPupilDialog.Gender = pupil2[2];
            addNewPupilDialog.DateOfBirth = pupil2[3];
            registrationDetailDialog = addNewPupilDialog.Continue();
            registrationDetailDialog.DateOfAdmission = dateOfAdmission;
            registrationDetailDialog.YearGroup = yearGroup;
            registrationDetailDialog.CreateRecord();
            confirmRequiredDialog = new POM.Components.Common.ConfirmRequiredDialog();
            confirmRequiredDialog.ClickOk();

            //Save the second pupil
            pupilRecord = PupilRecordPage.Create();
            pupilRecord.SavePupil();

            //Add the third pupil
            pupilRecords.Refresh();
            addNewPupilDialog = pupilRecords.AddNewPupil();
            addNewPupilDialog.Forename = pupil3[0];
            addNewPupilDialog.SurName = pupil3[1];
            addNewPupilDialog.Gender = pupil3[2];
            addNewPupilDialog.DateOfBirth = pupil3[3];
            registrationDetailDialog = addNewPupilDialog.Continue();
            registrationDetailDialog.DateOfAdmission = dateOfAdmission;
            registrationDetailDialog.YearGroup = yearGroup;
            registrationDetailDialog.CreateRecord();
            confirmRequiredDialog = new POM.Components.Common.ConfirmRequiredDialog();
            confirmRequiredDialog.ClickOk();

            //Save the third pupil
            pupilRecord = PupilRecordPage.Create();
            pupilRecord.SavePupil();

            //Add the fourth pupil
            pupilRecords.Refresh();
            addNewPupilDialog = pupilRecords.AddNewPupil();
            addNewPupilDialog.Forename = pupil4[0];
            addNewPupilDialog.SurName = pupil4[1];
            addNewPupilDialog.Gender = pupil4[2];
            addNewPupilDialog.DateOfBirth = pupil4[3];
            registrationDetailDialog = addNewPupilDialog.Continue();
            registrationDetailDialog.DateOfAdmission = dateOfAdmission;
            registrationDetailDialog.YearGroup = yearGroup;
            registrationDetailDialog.CreateRecord();
            confirmRequiredDialog = new POM.Components.Common.ConfirmRequiredDialog();
            confirmRequiredDialog.ClickOk();

            //Save the fourth pupil
            pupilRecord = PupilRecordPage.Create();
            pupilRecord.SavePupil();

            //Add the fifth pupil
            pupilRecords.Refresh();
            addNewPupilDialog = pupilRecords.AddNewPupil();
            addNewPupilDialog.Forename = pupil5[0];
            addNewPupilDialog.SurName = pupil5[1];
            addNewPupilDialog.Gender = pupil5[2];
            addNewPupilDialog.DateOfBirth = pupil5[3];
            registrationDetailDialog = addNewPupilDialog.Continue();
            registrationDetailDialog.DateOfAdmission = dateOfAdmission;
            registrationDetailDialog.YearGroup = yearGroup;
            registrationDetailDialog.CreateRecord();
            confirmRequiredDialog = new POM.Components.Common.ConfirmRequiredDialog();
            confirmRequiredDialog.ClickOk();

            //Save the fifth pupil
            pupilRecord = PupilRecordPage.Create();
            pupilRecord.SavePupil();

            //Add the sixth pupil
            pupilRecords.Refresh();
            addNewPupilDialog = pupilRecords.AddNewPupil();
            addNewPupilDialog.Forename = pupil6[0];
            addNewPupilDialog.SurName = pupil6[1];
            addNewPupilDialog.Gender = pupil6[2];
            addNewPupilDialog.DateOfBirth = pupil6[3];
            registrationDetailDialog = addNewPupilDialog.Continue();
            registrationDetailDialog.DateOfAdmission = dateOfAdmission;
            registrationDetailDialog.YearGroup = yearGroup;
            registrationDetailDialog.CreateRecord();
            confirmRequiredDialog = new POM.Components.Common.ConfirmRequiredDialog();
            confirmRequiredDialog.ClickOk();

            //Save the sixth pupil
            pupilRecord = PupilRecordPage.Create();
            pupilRecord.SavePupil();

            //Add the seventh pupil
            pupilRecords.Refresh();
            addNewPupilDialog = pupilRecords.AddNewPupil();
            addNewPupilDialog.Forename = pupil7[0];
            addNewPupilDialog.SurName = pupil7[1];
            addNewPupilDialog.Gender = pupil7[2];
            addNewPupilDialog.DateOfBirth = pupil7[3];
            registrationDetailDialog = addNewPupilDialog.Continue();
            registrationDetailDialog.DateOfAdmission = dateOfAdmission;
            registrationDetailDialog.YearGroup = yearGroup;
            registrationDetailDialog.CreateRecord();
            confirmRequiredDialog = new POM.Components.Common.ConfirmRequiredDialog();
            confirmRequiredDialog.ClickOk();

            //Save the seventh pupil
            pupilRecord = PupilRecordPage.Create();
            pupilRecord.SavePupil();

            //Add the eighth pupil
            pupilRecords.Refresh();
            addNewPupilDialog = pupilRecords.AddNewPupil();
            addNewPupilDialog.Forename = pupil8[0];
            addNewPupilDialog.SurName = pupil8[1];
            addNewPupilDialog.Gender = pupil8[2];
            addNewPupilDialog.DateOfBirth = pupil8[3];
            registrationDetailDialog = addNewPupilDialog.Continue();
            registrationDetailDialog.DateOfAdmission = dateOfAdmission;
            registrationDetailDialog.YearGroup = yearGroup;
            registrationDetailDialog.CreateRecord();
            confirmRequiredDialog = new POM.Components.Common.ConfirmRequiredDialog();
            confirmRequiredDialog.ClickOk();

            //Save the eighth pupil
            pupilRecord = PupilRecordPage.Create();
            pupilRecord.SavePupil();

            //Add the nineth pupil
            pupilRecords.Refresh();
            addNewPupilDialog = pupilRecords.AddNewPupil();
            addNewPupilDialog.Forename = pupil9[0];
            addNewPupilDialog.SurName = pupil9[1];
            addNewPupilDialog.Gender = pupil9[2];
            addNewPupilDialog.DateOfBirth = pupil9[3];
            registrationDetailDialog = addNewPupilDialog.Continue();
            registrationDetailDialog.DateOfAdmission = dateOfAdmission;
            registrationDetailDialog.YearGroup = yearGroup;
            registrationDetailDialog.CreateRecord();
            confirmRequiredDialog = new POM.Components.Common.ConfirmRequiredDialog();
            confirmRequiredDialog.ClickOk();

            //Save the nineth pupil
            pupilRecord = PupilRecordPage.Create();
            pupilRecord.SavePupil();

            //Add the tenth pupil
            pupilRecords.Refresh();
            addNewPupilDialog = pupilRecords.AddNewPupil();
            addNewPupilDialog.Forename = pupil10[0];
            addNewPupilDialog.SurName = pupil10[1];
            addNewPupilDialog.Gender = pupil10[2];
            addNewPupilDialog.DateOfBirth = pupil10[3];
            registrationDetailDialog = addNewPupilDialog.Continue();
            registrationDetailDialog.DateOfAdmission = dateOfAdmission;
            registrationDetailDialog.YearGroup = yearGroup;
            registrationDetailDialog.CreateRecord();
            confirmRequiredDialog = new POM.Components.Common.ConfirmRequiredDialog();
            confirmRequiredDialog.ClickOk();

            //Save the tenth pupil
            pupilRecord = PupilRecordPage.Create();
            pupilRecord.SavePupil();

            #endregion

            #region Test steps

            //Verify can navigate to Allocate Pupils To Groups by search
            var homePage = new HomePage();
            var taskSearch = homePage.TaskSearch();
            taskSearch.Search = "Allocate";
            var searchResults = taskSearch.SearchResult;
            var searchItem = searchResults.Items.FirstOrDefault(t => t.TaskAction.Equals("Allocate Pupils To Groups"));
            Assert.AreNotEqual(null, searchItem, "Cannot find Allocate Pupils To Groups page from search task");

            //Navigate to Allocate Pupils To Groups
            searchItem.Click();
            var allocatePupilToGroupTriplet = new AllocatePupilsToGroupsTriplet();
            allocatePupilToGroupTriplet.SearchCriteria.YearGroups[yearGroup].Select = true;

            //Search pupil
            var allocatePupilToGroupPage = allocatePupilToGroupTriplet.SearchCriteria.Search<AllocatePupilsToGroupsPage>();
            allocatePupilToGroupPage.EffectiveDate = effectiveDate;

            //Add class to ten pupils
            allocatePupilToGroupPage.AllocateTable[String.Format("{0}, {1}", pupil1[1], pupil1[0])][7].ValueDropDown = className;
            allocatePupilToGroupPage.AllocateTable[String.Format("{0}, {1}", pupil2[1], pupil2[0])][7].ValueDropDown = className;
            allocatePupilToGroupPage.AllocateTable[String.Format("{0}, {1}", pupil3[1], pupil3[0])][7].ValueDropDown = className;
            allocatePupilToGroupPage.AllocateTable[String.Format("{0}, {1}", pupil4[1], pupil4[0])][7].ValueDropDown = className;
            allocatePupilToGroupPage.AllocateTable[String.Format("{0}, {1}", pupil5[1], pupil5[0])][7].ValueDropDown = className;
            allocatePupilToGroupPage.AllocateTable[String.Format("{0}, {1}", pupil6[1], pupil6[0])][7].ValueDropDown = className;
            allocatePupilToGroupPage.AllocateTable[String.Format("{0}, {1}", pupil7[1], pupil7[0])][7].ValueDropDown = className;
            allocatePupilToGroupPage.AllocateTable[String.Format("{0}, {1}", pupil8[1], pupil8[0])][7].ValueDropDown = className;
            allocatePupilToGroupPage.AllocateTable[String.Format("{0}, {1}", pupil9[1], pupil9[0])][7].ValueDropDown = className;
            allocatePupilToGroupPage.AllocateTable[String.Format("{0}, {1}", pupil10[1], pupil10[0])][7].ValueDropDown = className;

            //Save
            allocatePupilToGroupPage.Save();

            //Search pupil with year group and class
            allocatePupilToGroupTriplet.Refresh();
            allocatePupilToGroupTriplet.SearchCriteria.Classes[className].Select = true;
            allocatePupilToGroupPage = allocatePupilToGroupTriplet.SearchCriteria.Search<AllocatePupilsToGroupsPage>();

            //Verify the first pupil
            var allocateRow = allocatePupilToGroupPage.AllocateTable[String.Format("{0}, {1}", pupil1[1], pupil1[0])];
            Assert.AreNotEqual(null, allocateRow, "Add class to the first pupil unsuccessfull");
            Assert.AreEqual(className, allocateRow[6].Text, "Current class to the first pupil is incorrect");

            //Verify the second pupil
            allocateRow = allocatePupilToGroupPage.AllocateTable[String.Format("{0}, {1}", pupil2[1], pupil2[0])];
            Assert.AreNotEqual(null, allocateRow, "Add class to the second pupil unsuccessfull");
            Assert.AreEqual(className, allocateRow[6].Text, "Current class to the second pupil is incorrect");

            //Verify the third pupil
            allocateRow = allocatePupilToGroupPage.AllocateTable[String.Format("{0}, {1}", pupil3[1], pupil3[0])];
            Assert.AreNotEqual(null, allocateRow, "Add class to the third pupil unsuccessfull");
            Assert.AreEqual(className, allocateRow[6].Text, "Current class to the third pupil is incorrect");

            //Verify the fourth pupil
            allocateRow = allocatePupilToGroupPage.AllocateTable[String.Format("{0}, {1}", pupil4[1], pupil4[0])];
            Assert.AreNotEqual(null, allocateRow, "Add class to the fourth pupil unsuccessfull");
            Assert.AreEqual(className, allocateRow[6].Text, "Current class to the fourth pupil is incorrect");

            //Verify the fifth pupil
            allocateRow = allocatePupilToGroupPage.AllocateTable[String.Format("{0}, {1}", pupil5[1], pupil5[0])];
            Assert.AreNotEqual(null, allocateRow, "Add class to the fifth pupil unsuccessfull");
            Assert.AreEqual(className, allocateRow[6].Text, "Current class to the fifth pupil is incorrect");

            //Verify the sixth pupil
            allocateRow = allocatePupilToGroupPage.AllocateTable[String.Format("{0}, {1}", pupil6[1], pupil6[0])];
            Assert.AreNotEqual(null, allocateRow, "Add class to the sixth pupil unsuccessfull");
            Assert.AreEqual(className, allocateRow[6].Text, "Current class to the sixth pupil is incorrect");

            //Verify the seventh pupil
            allocateRow = allocatePupilToGroupPage.AllocateTable[String.Format("{0}, {1}", pupil7[1], pupil7[0])];
            Assert.AreNotEqual(null, allocateRow, "Add class to the seventh pupil unsuccessfull");
            Assert.AreEqual(className, allocateRow[6].Text, "Current class to the seventh pupil is incorrect");

            //Verify the eighth pupil
            allocateRow = allocatePupilToGroupPage.AllocateTable[String.Format("{0}, {1}", pupil8[1], pupil8[0])];
            Assert.AreNotEqual(null, allocateRow, "Add class to the eighth pupil unsuccessfull");
            Assert.AreEqual(className, allocateRow[6].Text, "Current class to the eighth pupil is incorrect");

            //Verify the nineth pupil
            allocateRow = allocatePupilToGroupPage.AllocateTable[String.Format("{0}, {1}", pupil9[1], pupil9[0])];
            Assert.AreNotEqual(null, allocateRow, "Add class to the nineth pupil unsuccessfull");
            Assert.AreEqual(className, allocateRow[6].Text, "Current class to the nineth pupil is incorrect");

            //Verify the tenth pupil
            allocateRow = allocatePupilToGroupPage.AllocateTable[String.Format("{0}, {1}", pupil10[1], pupil10[0])];
            Assert.AreNotEqual(null, allocateRow, "Add class to the tenth pupil unsuccessfull");
            Assert.AreEqual(className, allocateRow[6].Text, "Current class to the tenth pupil is incorrect");

            #endregion

            #region Post-Condition : Delete 10 pupil

            SeleniumHelper.NavigateMenu("Tasks", "Pupils", "Delete Pupil");
            var deletePupilRecords = new DeletePupilRecordTriplet();
            deletePupilRecords.SearchCriteria.IsCurrent = true;
            deletePupilRecords.SearchCriteria.IsLeaver = true;
            deletePupilRecords.SearchCriteria.IsFuture = true;

            //Delete the first pupil
            deletePupilRecords.SearchCriteria.PupilName = String.Format("{0}, {1}", pupil1[1], pupil1[0]);
            var deleteResultPupils = deletePupilRecords.SearchCriteria.Search();
            var deletePupilSearchTile = deleteResultPupils.FirstOrDefault(t => t.Name.Equals(String.Format("{0}, {1}", pupil1[1], pupil1[0])));
            var deletePupilRecord = deletePupilSearchTile.Click<DeletePupilRecordPage>();
            deletePupilRecord.Delete();

            //Delete the second pupil
            deletePupilRecords.Refresh();
            deletePupilRecords.SearchCriteria.PupilName = String.Format("{0}, {1}", pupil2[1], pupil2[0]);
            deleteResultPupils = deletePupilRecords.SearchCriteria.Search();
            deletePupilSearchTile = deleteResultPupils.FirstOrDefault(t => t.Name.Equals(String.Format("{0}, {1}", pupil2[1], pupil2[0])));
            deletePupilRecord = deletePupilSearchTile.Click<DeletePupilRecordPage>();
            deletePupilRecord.Delete();

            //Delete the third pupil
            deletePupilRecords.Refresh();
            deletePupilRecords.SearchCriteria.PupilName = String.Format("{0}, {1}", pupil3[1], pupil3[0]);
            deleteResultPupils = deletePupilRecords.SearchCriteria.Search();
            deletePupilSearchTile = deleteResultPupils.FirstOrDefault(t => t.Name.Equals(String.Format("{0}, {1}", pupil3[1], pupil3[0])));
            deletePupilRecord = deletePupilSearchTile.Click<DeletePupilRecordPage>();
            deletePupilRecord.Delete();

            //Delete the fourth pupil
            deletePupilRecords.Refresh();
            deletePupilRecords.SearchCriteria.PupilName = String.Format("{0}, {1}", pupil4[1], pupil4[0]);
            deleteResultPupils = deletePupilRecords.SearchCriteria.Search();
            deletePupilSearchTile = deleteResultPupils.FirstOrDefault(t => t.Name.Equals(String.Format("{0}, {1}", pupil4[1], pupil4[0])));
            deletePupilRecord = deletePupilSearchTile.Click<DeletePupilRecordPage>();
            deletePupilRecord.Delete();

            //Delete the fifth pupil
            deletePupilRecords.Refresh();
            deletePupilRecords.SearchCriteria.PupilName = String.Format("{0}, {1}", pupil5[1], pupil5[0]);
            deleteResultPupils = deletePupilRecords.SearchCriteria.Search();
            deletePupilSearchTile = deleteResultPupils.FirstOrDefault(t => t.Name.Equals(String.Format("{0}, {1}", pupil5[1], pupil5[0])));
            deletePupilRecord = deletePupilSearchTile.Click<DeletePupilRecordPage>();
            deletePupilRecord.Delete();

            //Delete the sixth pupil
            deletePupilRecords.Refresh();
            deletePupilRecords.SearchCriteria.PupilName = String.Format("{0}, {1}", pupil6[1], pupil6[0]);
            deleteResultPupils = deletePupilRecords.SearchCriteria.Search();
            deletePupilSearchTile = deleteResultPupils.FirstOrDefault(t => t.Name.Equals(String.Format("{0}, {1}", pupil6[1], pupil6[0])));
            deletePupilRecord = deletePupilSearchTile.Click<DeletePupilRecordPage>();
            deletePupilRecord.Delete();

            //Delete the seventh pupil
            deletePupilRecords.Refresh();
            deletePupilRecords.SearchCriteria.PupilName = String.Format("{0}, {1}", pupil7[1], pupil7[0]);
            deleteResultPupils = deletePupilRecords.SearchCriteria.Search();
            deletePupilSearchTile = deleteResultPupils.FirstOrDefault(t => t.Name.Equals(String.Format("{0}, {1}", pupil7[1], pupil7[0])));
            deletePupilRecord = deletePupilSearchTile.Click<DeletePupilRecordPage>();
            deletePupilRecord.Delete();

            //Delete the eighth pupil
            deletePupilRecords.Refresh();
            deletePupilRecords.SearchCriteria.PupilName = String.Format("{0}, {1}", pupil8[1], pupil8[0]);
            deleteResultPupils = deletePupilRecords.SearchCriteria.Search();
            deletePupilSearchTile = deleteResultPupils.FirstOrDefault(t => t.Name.Equals(String.Format("{0}, {1}", pupil8[1], pupil8[0])));
            deletePupilRecord = deletePupilSearchTile.Click<DeletePupilRecordPage>();
            deletePupilRecord.Delete();

            //Delete the nineth pupil
            deletePupilRecords.Refresh();
            deletePupilRecords.SearchCriteria.PupilName = String.Format("{0}, {1}", pupil9[1], pupil9[0]);
            deleteResultPupils = deletePupilRecords.SearchCriteria.Search();
            deletePupilSearchTile = deleteResultPupils.FirstOrDefault(t => t.Name.Equals(String.Format("{0}, {1}", pupil9[1], pupil9[0])));
            deletePupilRecord = deletePupilSearchTile.Click<DeletePupilRecordPage>();
            deletePupilRecord.Delete();

            //Delete the tenth pupil
            deletePupilRecords.Refresh();
            deletePupilRecords.SearchCriteria.PupilName = String.Format("{0}, {1}", pupil10[1], pupil10[0]);
            deleteResultPupils = deletePupilRecords.SearchCriteria.Search();
            deletePupilSearchTile = deleteResultPupils.FirstOrDefault(t => t.Name.Equals(String.Format("{0}, {1}", pupil10[1], pupil10[0])));
            deletePupilRecord = deletePupilSearchTile.Click<DeletePupilRecordPage>();
            deletePupilRecord.Delete();

            #endregion
        }

        /// <summary>
        /// TC AP-03
        /// Au : An Nguyen
        /// Description: Exercise ability to 'Allocate Pupils to Groups' whilst the pupils are already assigned to Year Groups and Classes.
        /// Role: School Administrator
        /// Status : Issue Current Year Group and Class do not update
        /// </summary>
        [WebDriverTest(TimeoutSeconds = 3000, Enabled = false, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { "g1" }, DataProvider = "TC_AP03_Data")]
        public void TC_AP03_Exercise_ability_to_Allocate_Pupils_to_Groups_whilst_the_pupils_are_already_assigned_to_Year_Groups_and_Classes(string[] pupil1, string[] pupil2, string[] pupil3, string[] pupil4, string[] pupil5,
                    string[] pupil6, string[] pupil7, string[] pupil8, string[] pupil9, string[] pupil10, string dateOfAdmission, string yearGroup, string effectiveDate, string className, string newYearGroup, string newClassName)
        {
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);

            #region Pre-condition : Create 10 pupils

            //Navigate to Adminssion Settings
            SeleniumHelper.NavigateMenu("Tasks", "Admission Settings", "Admission Settings");
            var admissionSetting = new AdmissionSettingsPage();

            //Edit last admission number
            admissionSetting.LastAdmissionNumber = admissionSetting.GetNextAvailableAdmissionNumber();
            admissionSetting.Save();
            Assert.AreEqual(true, admissionSetting.IsSuccessMessageDisplay(), "Can not change Last Admission Number");

            //Navigate to Pupil Records
            SeleniumHelper.NavigateQuickLink("Pupil Records");
            var pupilRecords = new PupilRecordTriplet();

            //Add the first pupil
            var addNewPupilDialog = pupilRecords.AddNewPupil();
            addNewPupilDialog.Forename = pupil1[0];
            addNewPupilDialog.SurName = pupil1[1];
            addNewPupilDialog.Gender = pupil1[2];
            addNewPupilDialog.DateOfBirth = pupil1[3];
            var registrationDetailDialog = addNewPupilDialog.Continue();
            registrationDetailDialog.DateOfAdmission = dateOfAdmission;
            registrationDetailDialog.YearGroup = yearGroup;
            registrationDetailDialog.CreateRecord();
            var confirmRequiredDialog = new POM.Components.Common.ConfirmRequiredDialog();
            confirmRequiredDialog.ClickOk();

            //Save the first pupil
            var pupilRecord = PupilRecordPage.Create();
            pupilRecord.SavePupil();

            //Add the second pupil
            pupilRecords.Refresh();
            addNewPupilDialog = pupilRecords.AddNewPupil();
            addNewPupilDialog.Forename = pupil2[0];
            addNewPupilDialog.SurName = pupil2[1];
            addNewPupilDialog.Gender = pupil2[2];
            addNewPupilDialog.DateOfBirth = pupil2[3];
            registrationDetailDialog = addNewPupilDialog.Continue();
            registrationDetailDialog.DateOfAdmission = dateOfAdmission;
            registrationDetailDialog.YearGroup = yearGroup;
            registrationDetailDialog.CreateRecord();
            confirmRequiredDialog = new POM.Components.Common.ConfirmRequiredDialog();
            confirmRequiredDialog.ClickOk();

            //Save the second pupil
            pupilRecord = PupilRecordPage.Create();
            pupilRecord.SavePupil();

            //Add the third pupil
            pupilRecords.Refresh();
            addNewPupilDialog = pupilRecords.AddNewPupil();
            addNewPupilDialog.Forename = pupil3[0];
            addNewPupilDialog.SurName = pupil3[1];
            addNewPupilDialog.Gender = pupil3[2];
            addNewPupilDialog.DateOfBirth = pupil3[3];
            registrationDetailDialog = addNewPupilDialog.Continue();
            registrationDetailDialog.DateOfAdmission = dateOfAdmission;
            registrationDetailDialog.YearGroup = yearGroup;
            registrationDetailDialog.CreateRecord();
            confirmRequiredDialog = new POM.Components.Common.ConfirmRequiredDialog();
            confirmRequiredDialog.ClickOk();

            //Save the third pupil
            pupilRecord = PupilRecordPage.Create();
            pupilRecord.SavePupil();

            //Add the fourth pupil
            pupilRecords.Refresh();
            addNewPupilDialog = pupilRecords.AddNewPupil();
            addNewPupilDialog.Forename = pupil4[0];
            addNewPupilDialog.SurName = pupil4[1];
            addNewPupilDialog.Gender = pupil4[2];
            addNewPupilDialog.DateOfBirth = pupil4[3];
            registrationDetailDialog = addNewPupilDialog.Continue();
            registrationDetailDialog.DateOfAdmission = dateOfAdmission;
            registrationDetailDialog.YearGroup = yearGroup;
            registrationDetailDialog.CreateRecord();
            confirmRequiredDialog = new POM.Components.Common.ConfirmRequiredDialog();
            confirmRequiredDialog.ClickOk();

            //Save the fourth pupil
            pupilRecord = PupilRecordPage.Create();
            pupilRecord.SavePupil();

            //Add the fifth pupil
            pupilRecords.Refresh();
            addNewPupilDialog = pupilRecords.AddNewPupil();
            addNewPupilDialog.Forename = pupil5[0];
            addNewPupilDialog.SurName = pupil5[1];
            addNewPupilDialog.Gender = pupil5[2];
            addNewPupilDialog.DateOfBirth = pupil5[3];
            registrationDetailDialog = addNewPupilDialog.Continue();
            registrationDetailDialog.DateOfAdmission = dateOfAdmission;
            registrationDetailDialog.YearGroup = yearGroup;
            registrationDetailDialog.CreateRecord();
            confirmRequiredDialog = new POM.Components.Common.ConfirmRequiredDialog();
            confirmRequiredDialog.ClickOk();

            //Save the fifth pupil
            pupilRecord = PupilRecordPage.Create();
            pupilRecord.SavePupil();

            //Add the sixth pupil
            pupilRecords.Refresh();
            addNewPupilDialog = pupilRecords.AddNewPupil();
            addNewPupilDialog.Forename = pupil6[0];
            addNewPupilDialog.SurName = pupil6[1];
            addNewPupilDialog.Gender = pupil6[2];
            addNewPupilDialog.DateOfBirth = pupil6[3];
            registrationDetailDialog = addNewPupilDialog.Continue();
            registrationDetailDialog.DateOfAdmission = dateOfAdmission;
            registrationDetailDialog.YearGroup = yearGroup;
            registrationDetailDialog.CreateRecord();
            confirmRequiredDialog = new POM.Components.Common.ConfirmRequiredDialog();
            confirmRequiredDialog.ClickOk();

            //Save the sixth pupil
            pupilRecord = PupilRecordPage.Create();
            pupilRecord.SavePupil();

            //Add the seventh pupil
            pupilRecords.Refresh();
            addNewPupilDialog = pupilRecords.AddNewPupil();
            addNewPupilDialog.Forename = pupil7[0];
            addNewPupilDialog.SurName = pupil7[1];
            addNewPupilDialog.Gender = pupil7[2];
            addNewPupilDialog.DateOfBirth = pupil7[3];
            registrationDetailDialog = addNewPupilDialog.Continue();
            registrationDetailDialog.DateOfAdmission = dateOfAdmission;
            registrationDetailDialog.YearGroup = yearGroup;
            registrationDetailDialog.CreateRecord();
            confirmRequiredDialog = new POM.Components.Common.ConfirmRequiredDialog();
            confirmRequiredDialog.ClickOk();

            //Save the seventh pupil
            pupilRecord = PupilRecordPage.Create();
            pupilRecord.SavePupil();

            //Add the eighth pupil
            pupilRecords.Refresh();
            addNewPupilDialog = pupilRecords.AddNewPupil();
            addNewPupilDialog.Forename = pupil8[0];
            addNewPupilDialog.SurName = pupil8[1];
            addNewPupilDialog.Gender = pupil8[2];
            addNewPupilDialog.DateOfBirth = pupil8[3];
            registrationDetailDialog = addNewPupilDialog.Continue();
            registrationDetailDialog.DateOfAdmission = dateOfAdmission;
            registrationDetailDialog.YearGroup = yearGroup;
            registrationDetailDialog.CreateRecord();
            confirmRequiredDialog = new POM.Components.Common.ConfirmRequiredDialog();
            confirmRequiredDialog.ClickOk();

            //Save the eighth pupil
            pupilRecord = PupilRecordPage.Create();
            pupilRecord.SavePupil();

            //Add the nineth pupil
            pupilRecords.Refresh();
            addNewPupilDialog = pupilRecords.AddNewPupil();
            addNewPupilDialog.Forename = pupil9[0];
            addNewPupilDialog.SurName = pupil9[1];
            addNewPupilDialog.Gender = pupil9[2];
            addNewPupilDialog.DateOfBirth = pupil9[3];
            registrationDetailDialog = addNewPupilDialog.Continue();
            registrationDetailDialog.DateOfAdmission = dateOfAdmission;
            registrationDetailDialog.YearGroup = yearGroup;
            registrationDetailDialog.CreateRecord();
            confirmRequiredDialog = new POM.Components.Common.ConfirmRequiredDialog();
            confirmRequiredDialog.ClickOk();

            //Save the nineth pupil
            pupilRecord = PupilRecordPage.Create();
            pupilRecord.SavePupil();

            //Add the tenth pupil
            pupilRecords.Refresh();
            addNewPupilDialog = pupilRecords.AddNewPupil();
            addNewPupilDialog.Forename = pupil10[0];
            addNewPupilDialog.SurName = pupil10[1];
            addNewPupilDialog.Gender = pupil10[2];
            addNewPupilDialog.DateOfBirth = pupil10[3];
            registrationDetailDialog = addNewPupilDialog.Continue();
            registrationDetailDialog.DateOfAdmission = dateOfAdmission;
            registrationDetailDialog.YearGroup = yearGroup;
            registrationDetailDialog.CreateRecord();
            confirmRequiredDialog = new POM.Components.Common.ConfirmRequiredDialog();
            confirmRequiredDialog.ClickOk();

            //Save the tenth pupil
            pupilRecord = PupilRecordPage.Create();
            pupilRecord.SavePupil();

            #endregion

            #region Pre-condition : Allocate 10 pupils to class

            //Navigate to Allocate Pupils To Groups
            SeleniumHelper.NavigateMenu("Tasks", "School Groups", "Allocate Pupils To Groups");
            var allocatePupilToGroupTriplet = new AllocatePupilsToGroupsTriplet();
            allocatePupilToGroupTriplet.SearchCriteria.YearGroups[yearGroup].Select = true;

            //Search pupil
            var allocatePupilToGroupPage = allocatePupilToGroupTriplet.SearchCriteria.Search<AllocatePupilsToGroupsPage>();
            allocatePupilToGroupPage.EffectiveDate = effectiveDate;

            //Add class to ten pupils
            allocatePupilToGroupPage.AllocateTable[String.Format("{0}, {1}", pupil1[1], pupil1[0])][7].ValueDropDown = className;
            allocatePupilToGroupPage.AllocateTable[String.Format("{0}, {1}", pupil2[1], pupil2[0])][7].ValueDropDown = className;
            allocatePupilToGroupPage.AllocateTable[String.Format("{0}, {1}", pupil3[1], pupil3[0])][7].ValueDropDown = className;
            allocatePupilToGroupPage.AllocateTable[String.Format("{0}, {1}", pupil4[1], pupil4[0])][7].ValueDropDown = className;
            allocatePupilToGroupPage.AllocateTable[String.Format("{0}, {1}", pupil5[1], pupil5[0])][7].ValueDropDown = className;
            allocatePupilToGroupPage.AllocateTable[String.Format("{0}, {1}", pupil6[1], pupil6[0])][7].ValueDropDown = className;
            allocatePupilToGroupPage.AllocateTable[String.Format("{0}, {1}", pupil7[1], pupil7[0])][7].ValueDropDown = className;
            allocatePupilToGroupPage.AllocateTable[String.Format("{0}, {1}", pupil8[1], pupil8[0])][7].ValueDropDown = className;
            allocatePupilToGroupPage.AllocateTable[String.Format("{0}, {1}", pupil9[1], pupil9[0])][7].ValueDropDown = className;
            allocatePupilToGroupPage.AllocateTable[String.Format("{0}, {1}", pupil10[1], pupil10[0])][7].ValueDropDown = className;

            //Save
            allocatePupilToGroupPage.Save();

            #endregion

            #region Test steps

            //Search pupil with year group and class
            allocatePupilToGroupTriplet.Refresh();
            allocatePupilToGroupTriplet.SearchCriteria.Classes[className].Select = true;
            allocatePupilToGroupPage = allocatePupilToGroupTriplet.SearchCriteria.Search<AllocatePupilsToGroupsPage>();
            allocatePupilToGroupPage.EffectiveDate = effectiveDate;
            
            //Allocate to new year group and class
            allocatePupilToGroupPage.AllocateTable[String.Format("{0}, {1}", pupil1[1], pupil1[0])][5].ValueDropDown = newYearGroup;
            allocatePupilToGroupPage.AllocateTable[String.Format("{0}, {1}", pupil1[1], pupil1[0])][7].ValueDropDown = newClassName;
            allocatePupilToGroupPage.AllocateTable[String.Format("{0}, {1}", pupil2[1], pupil2[0])][5].ValueDropDown = newYearGroup;
            allocatePupilToGroupPage.AllocateTable[String.Format("{0}, {1}", pupil2[1], pupil2[0])][7].ValueDropDown = newClassName;
            allocatePupilToGroupPage.AllocateTable[String.Format("{0}, {1}", pupil3[1], pupil3[0])][5].ValueDropDown = newYearGroup;
            allocatePupilToGroupPage.AllocateTable[String.Format("{0}, {1}", pupil3[1], pupil3[0])][7].ValueDropDown = newClassName;
            allocatePupilToGroupPage.AllocateTable[String.Format("{0}, {1}", pupil4[1], pupil4[0])][5].ValueDropDown = newYearGroup;
            allocatePupilToGroupPage.AllocateTable[String.Format("{0}, {1}", pupil4[1], pupil4[0])][7].ValueDropDown = newClassName;
            allocatePupilToGroupPage.AllocateTable[String.Format("{0}, {1}", pupil5[1], pupil5[0])][5].ValueDropDown = newYearGroup;
            allocatePupilToGroupPage.AllocateTable[String.Format("{0}, {1}", pupil5[1], pupil5[0])][7].ValueDropDown = newClassName;
            allocatePupilToGroupPage.AllocateTable[String.Format("{0}, {1}", pupil6[1], pupil6[0])][5].ValueDropDown = newYearGroup;
            allocatePupilToGroupPage.AllocateTable[String.Format("{0}, {1}", pupil6[1], pupil6[0])][7].ValueDropDown = newClassName;
            allocatePupilToGroupPage.AllocateTable[String.Format("{0}, {1}", pupil7[1], pupil7[0])][5].ValueDropDown = newYearGroup;
            allocatePupilToGroupPage.AllocateTable[String.Format("{0}, {1}", pupil7[1], pupil7[0])][7].ValueDropDown = newClassName;
            allocatePupilToGroupPage.AllocateTable[String.Format("{0}, {1}", pupil8[1], pupil8[0])][5].ValueDropDown = newYearGroup;
            allocatePupilToGroupPage.AllocateTable[String.Format("{0}, {1}", pupil8[1], pupil8[0])][7].ValueDropDown = newClassName;
            allocatePupilToGroupPage.AllocateTable[String.Format("{0}, {1}", pupil9[1], pupil9[0])][5].ValueDropDown = newYearGroup;
            allocatePupilToGroupPage.AllocateTable[String.Format("{0}, {1}", pupil9[1], pupil9[0])][7].ValueDropDown = newClassName;
            allocatePupilToGroupPage.AllocateTable[String.Format("{0}, {1}", pupil10[1], pupil10[0])][5].ValueDropDown = newYearGroup;
            allocatePupilToGroupPage.AllocateTable[String.Format("{0}, {1}", pupil10[1], pupil10[0])][7].ValueDropDown = newClassName;

            //Save
            allocatePupilToGroupPage.Save();
            
            //Verify 10 pupils disappear
            var allocateRow = allocatePupilToGroupPage.AllocateTable[String.Format("{0}, {1}", pupil1[1], pupil1[0])];
            Assert.AreEqual(null, allocateRow, "The first pupil still appear on old year group list");
            allocateRow = allocatePupilToGroupPage.AllocateTable[String.Format("{0}, {1}", pupil2[1], pupil2[0])];
            Assert.AreEqual(null, allocateRow, "The second pupil still appear on old year group list");
            allocateRow = allocatePupilToGroupPage.AllocateTable[String.Format("{0}, {1}", pupil3[1], pupil3[0])];
            Assert.AreEqual(null, allocateRow, "The third pupil still appear on old year group list");
            allocateRow = allocatePupilToGroupPage.AllocateTable[String.Format("{0}, {1}", pupil4[1], pupil4[0])];
            Assert.AreEqual(null, allocateRow, "The fourth pupil still appear on old year group list");
            allocateRow = allocatePupilToGroupPage.AllocateTable[String.Format("{0}, {1}", pupil5[1], pupil5[0])];
            Assert.AreEqual(null, allocateRow, "The fifth pupil still appear on old year group list");
            allocateRow = allocatePupilToGroupPage.AllocateTable[String.Format("{0}, {1}", pupil6[1], pupil6[0])];
            Assert.AreEqual(null, allocateRow, "The sixth pupil still appear on old year group list");
            allocateRow = allocatePupilToGroupPage.AllocateTable[String.Format("{0}, {1}", pupil7[1], pupil7[0])];
            Assert.AreEqual(null, allocateRow, "The seventh pupil still appear on old year group list");
            allocateRow = allocatePupilToGroupPage.AllocateTable[String.Format("{0}, {1}", pupil8[1], pupil8[0])];
            Assert.AreEqual(null, allocateRow, "The eighth pupil still appear on old year group list");
            allocateRow = allocatePupilToGroupPage.AllocateTable[String.Format("{0}, {1}", pupil9[1], pupil9[0])];
            Assert.AreEqual(null, allocateRow, "The nineth pupil still appear on old year group list");
            allocateRow = allocatePupilToGroupPage.AllocateTable[String.Format("{0}, {1}", pupil10[1], pupil10[0])];
            Assert.AreEqual(null, allocateRow, "The tenth pupil still appear on old year group list");

            //Search with new year group and new class
            allocatePupilToGroupTriplet.Refresh();
            allocatePupilToGroupTriplet.SearchCriteria.YearGroups[yearGroup].Select = false;
            allocatePupilToGroupTriplet.SearchCriteria.YearGroups[newYearGroup].Select = true;
            allocatePupilToGroupTriplet.SearchCriteria.Classes[className].Select = false;
            allocatePupilToGroupTriplet.SearchCriteria.Classes[newClassName].Select = true;
            allocatePupilToGroupPage = allocatePupilToGroupTriplet.SearchCriteria.Search<AllocatePupilsToGroupsPage>();

            //Verify the first pupil
            allocateRow = allocatePupilToGroupPage.AllocateTable[String.Format("{0}, {1}", pupil1[1], pupil1[0])];
            Assert.AreNotEqual(null, allocateRow, "Change year group and class of the first pupil unsuccessfull");
            Assert.AreEqual(newYearGroup, allocateRow[4].Text, "Current year group to the first pupil is incorrect");
            Assert.AreEqual(newClassName, allocateRow[6].Text, "Current class to the first pupil is incorrect");

            //Verify the second pupil
            allocateRow = allocatePupilToGroupPage.AllocateTable[String.Format("{0}, {1}", pupil2[1], pupil2[0])];
            Assert.AreNotEqual(null, allocateRow, "Change year group and class of the second pupil unsuccessfull");
            Assert.AreEqual(newYearGroup, allocateRow[4].Text, "Current year group to the second pupil is incorrect");
            Assert.AreEqual(newClassName, allocateRow[6].Text, "Current class to the second pupil is incorrect");

            //Verify the third pupil
            allocateRow = allocatePupilToGroupPage.AllocateTable[String.Format("{0}, {1}", pupil3[1], pupil3[0])];
            Assert.AreNotEqual(null, allocateRow, "Change year group and class of the third pupil unsuccessfull");
            Assert.AreEqual(newYearGroup, allocateRow[4].Text, "Current year group to the third pupil is incorrect");
            Assert.AreEqual(newClassName, allocateRow[6].Text, "Current class to the third pupil is incorrect");

            //Verify the fourth pupil
            allocateRow = allocatePupilToGroupPage.AllocateTable[String.Format("{0}, {1}", pupil4[1], pupil4[0])];
            Assert.AreNotEqual(null, allocateRow, "Change year group and class of the fourth pupil unsuccessfull");
            Assert.AreEqual(newYearGroup, allocateRow[4].Text, "Current year group to the fourth pupil is incorrect");
            Assert.AreEqual(newClassName, allocateRow[6].Text, "Current class to the fourth pupil is incorrect");

            //Verify the fifth pupil
            allocateRow = allocatePupilToGroupPage.AllocateTable[String.Format("{0}, {1}", pupil5[1], pupil5[0])];
            Assert.AreNotEqual(null, allocateRow, "Change year group and class of the fifth pupil unsuccessfull");
            Assert.AreEqual(newYearGroup, allocateRow[4].Text, "Current year group to the fifth pupil is incorrect");
            Assert.AreEqual(newClassName, allocateRow[6].Text, "Current class to the fifth pupil is incorrect");

            //Verify the sixth pupil
            allocateRow = allocatePupilToGroupPage.AllocateTable[String.Format("{0}, {1}", pupil6[1], pupil6[0])];
            Assert.AreNotEqual(null, allocateRow, "Change year group and class of the sixth pupil unsuccessfull");
            Assert.AreEqual(newYearGroup, allocateRow[4].Text, "Current year group to the sixth pupil is incorrect");
            Assert.AreEqual(newClassName, allocateRow[6].Text, "Current class to the sixth pupil is incorrect");

            //Verify the seventh pupil
            allocateRow = allocatePupilToGroupPage.AllocateTable[String.Format("{0}, {1}", pupil7[1], pupil7[0])];
            Assert.AreNotEqual(null, allocateRow, "Change year group and class of the seventh pupil unsuccessfull");
            Assert.AreEqual(newYearGroup, allocateRow[4].Text, "Current year group to the seventh pupil is incorrect");
            Assert.AreEqual(newClassName, allocateRow[6].Text, "Current class to the seventh pupil is incorrect");

            //Verify the eighth pupil
            allocateRow = allocatePupilToGroupPage.AllocateTable[String.Format("{0}, {1}", pupil8[1], pupil8[0])];
            Assert.AreNotEqual(null, allocateRow, "Change year group and class of the eighth pupil unsuccessfull");
            Assert.AreEqual(newYearGroup, allocateRow[4].Text, "Current year group to the eighth pupil is incorrect");
            Assert.AreEqual(newClassName, allocateRow[6].Text, "Current class to the eighth pupil is incorrect");

            //Verify the nineth pupil
            allocateRow = allocatePupilToGroupPage.AllocateTable[String.Format("{0}, {1}", pupil9[1], pupil9[0])];
            Assert.AreNotEqual(null, allocateRow, "Change year group and class of the nineth pupil unsuccessfull");
            Assert.AreEqual(newYearGroup, allocateRow[4].Text, "Current year group to the nineth pupil is incorrect");
            Assert.AreEqual(newClassName, allocateRow[6].Text, "Current class to the nineth pupil is incorrect");

            //Verify the tenth pupil
            allocateRow = allocatePupilToGroupPage.AllocateTable[String.Format("{0}, {1}", pupil10[1], pupil10[0])];
            Assert.AreNotEqual(null, allocateRow, "Change year group and class of the tenth pupil unsuccessfull");
            Assert.AreEqual(newYearGroup, allocateRow[4].Text, "Current year group to the tenth pupil is incorrect");
            Assert.AreEqual(newClassName, allocateRow[6].Text, "Current class to the tenth pupil is incorrect");

            #endregion

            #region Post-Condition : Delete 10 pupil

            SeleniumHelper.NavigateMenu("Tasks", "Pupils", "Delete Pupil");
            var deletePupilRecords = new DeletePupilRecordTriplet();
            deletePupilRecords.SearchCriteria.IsCurrent = true;
            deletePupilRecords.SearchCriteria.IsLeaver = true;
            deletePupilRecords.SearchCriteria.IsFuture = true;

            //Delete the first pupil
            deletePupilRecords.SearchCriteria.PupilName = String.Format("{0}, {1}", pupil1[1], pupil1[0]);
            var deleteResultPupils = deletePupilRecords.SearchCriteria.Search();
            var deletePupilSearchTile = deleteResultPupils.FirstOrDefault(t => t.Name.Equals(String.Format("{0}, {1}", pupil1[1], pupil1[0])));
            var deletePupilRecord = deletePupilSearchTile.Click<DeletePupilRecordPage>();
            deletePupilRecord.Delete();

            //Delete the second pupil
            deletePupilRecords.Refresh();
            deletePupilRecords.SearchCriteria.PupilName = String.Format("{0}, {1}", pupil2[1], pupil2[0]);
            deleteResultPupils = deletePupilRecords.SearchCriteria.Search();
            deletePupilSearchTile = deleteResultPupils.FirstOrDefault(t => t.Name.Equals(String.Format("{0}, {1}", pupil2[1], pupil2[0])));
            deletePupilRecord = deletePupilSearchTile.Click<DeletePupilRecordPage>();
            deletePupilRecord.Delete();

            //Delete the third pupil
            deletePupilRecords.Refresh();
            deletePupilRecords.SearchCriteria.PupilName = String.Format("{0}, {1}", pupil3[1], pupil3[0]);
            deleteResultPupils = deletePupilRecords.SearchCriteria.Search();
            deletePupilSearchTile = deleteResultPupils.FirstOrDefault(t => t.Name.Equals(String.Format("{0}, {1}", pupil3[1], pupil3[0])));
            deletePupilRecord = deletePupilSearchTile.Click<DeletePupilRecordPage>();
            deletePupilRecord.Delete();

            //Delete the fourth pupil
            deletePupilRecords.Refresh();
            deletePupilRecords.SearchCriteria.PupilName = String.Format("{0}, {1}", pupil4[1], pupil4[0]);
            deleteResultPupils = deletePupilRecords.SearchCriteria.Search();
            deletePupilSearchTile = deleteResultPupils.FirstOrDefault(t => t.Name.Equals(String.Format("{0}, {1}", pupil4[1], pupil4[0])));
            deletePupilRecord = deletePupilSearchTile.Click<DeletePupilRecordPage>();
            deletePupilRecord.Delete();

            //Delete the fifth pupil
            deletePupilRecords.Refresh();
            deletePupilRecords.SearchCriteria.PupilName = String.Format("{0}, {1}", pupil5[1], pupil5[0]);
            deleteResultPupils = deletePupilRecords.SearchCriteria.Search();
            deletePupilSearchTile = deleteResultPupils.FirstOrDefault(t => t.Name.Equals(String.Format("{0}, {1}", pupil5[1], pupil5[0])));
            deletePupilRecord = deletePupilSearchTile.Click<DeletePupilRecordPage>();
            deletePupilRecord.Delete();

            //Delete the sixth pupil
            deletePupilRecords.Refresh();
            deletePupilRecords.SearchCriteria.PupilName = String.Format("{0}, {1}", pupil6[1], pupil6[0]);
            deleteResultPupils = deletePupilRecords.SearchCriteria.Search();
            deletePupilSearchTile = deleteResultPupils.FirstOrDefault(t => t.Name.Equals(String.Format("{0}, {1}", pupil6[1], pupil6[0])));
            deletePupilRecord = deletePupilSearchTile.Click<DeletePupilRecordPage>();
            deletePupilRecord.Delete();

            //Delete the seventh pupil
            deletePupilRecords.Refresh();
            deletePupilRecords.SearchCriteria.PupilName = String.Format("{0}, {1}", pupil7[1], pupil7[0]);
            deleteResultPupils = deletePupilRecords.SearchCriteria.Search();
            deletePupilSearchTile = deleteResultPupils.FirstOrDefault(t => t.Name.Equals(String.Format("{0}, {1}", pupil7[1], pupil7[0])));
            deletePupilRecord = deletePupilSearchTile.Click<DeletePupilRecordPage>();
            deletePupilRecord.Delete();

            //Delete the eighth pupil
            deletePupilRecords.Refresh();
            deletePupilRecords.SearchCriteria.PupilName = String.Format("{0}, {1}", pupil8[1], pupil8[0]);
            deleteResultPupils = deletePupilRecords.SearchCriteria.Search();
            deletePupilSearchTile = deleteResultPupils.FirstOrDefault(t => t.Name.Equals(String.Format("{0}, {1}", pupil8[1], pupil8[0])));
            deletePupilRecord = deletePupilSearchTile.Click<DeletePupilRecordPage>();
            deletePupilRecord.Delete();

            //Delete the nineth pupil
            deletePupilRecords.Refresh();
            deletePupilRecords.SearchCriteria.PupilName = String.Format("{0}, {1}", pupil9[1], pupil9[0]);
            deleteResultPupils = deletePupilRecords.SearchCriteria.Search();
            deletePupilSearchTile = deleteResultPupils.FirstOrDefault(t => t.Name.Equals(String.Format("{0}, {1}", pupil9[1], pupil9[0])));
            deletePupilRecord = deletePupilSearchTile.Click<DeletePupilRecordPage>();
            deletePupilRecord.Delete();

            //Delete the tenth pupil
            deletePupilRecords.Refresh();
            deletePupilRecords.SearchCriteria.PupilName = String.Format("{0}, {1}", pupil10[1], pupil10[0]);
            deleteResultPupils = deletePupilRecords.SearchCriteria.Search();
            deletePupilSearchTile = deleteResultPupils.FirstOrDefault(t => t.Name.Equals(String.Format("{0}, {1}", pupil10[1], pupil10[0])));
            deletePupilRecord = deletePupilSearchTile.Click<DeletePupilRecordPage>();
            deletePupilRecord.Delete();

            #endregion
        }


        /// <summary>
        /// TC AP-05
        /// Au : An Nguyen
        /// Description: Exercise ability to add one extra 'Curriculum Year' with a start date on 1st day of the Next Academic Year.
        /// Role: School Administrator
        /// </summary>
        [WebDriverTest(TimeoutSeconds = 600, Enabled = false, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { "g1" }, DataProvider = "TC_AP05_Data")]
        public void TC_AP05_Exercise_ability_to_add_one_extra_Curriculum_Year_with_a_start_date_on_the_Next_Academic_Year(string curriculumYear, string startDate)
        {
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);

            #region Test steps

            //Verify can navigate to My School Details by search
            var homePage = new HomePage();
            var taskSearch = homePage.TaskSearch();
            taskSearch.Search = "My";
            var searchResults = taskSearch.SearchResult;
            var searchItem = searchResults.Items.FirstOrDefault(t => t.TaskAction.Equals("My School Details"));
            Assert.AreNotEqual(null, searchItem, "Cannot find My School Details page from search task");

            //Navigate to My School Details
            searchItem.Click();
            var mySchoolDetails = new MySchoolDetailsPage();
            mySchoolDetails.ScrollToCurriculumYear();

            //Delete old curriculum year if exist
            var yearRow = mySchoolDetails.CurriculumYears.Rows.FirstOrDefault(t => t.NCYear.Equals(curriculumYear));
            mySchoolDetails.DeleteGridRowIfExist(yearRow);
            mySchoolDetails.Save();

            //Add new curriculum year
            mySchoolDetails.Refresh();
            yearRow = mySchoolDetails.CurriculumYears.Rows.Last();
            yearRow.NCYear = curriculumYear;
            yearRow.StartDate = startDate;

            //Save
            mySchoolDetails.Save();

            //Verify message
            Assert.AreEqual(true, mySchoolDetails.IsSuccessMessageDisplay(), "Success message does not display");

            //Verify data
            yearRow = mySchoolDetails.CurriculumYears.Rows.FirstOrDefault(t => t.NCYear.Equals(curriculumYear));
            Assert.AreNotEqual(null, yearRow, "Cannot add new Curriculum Year");
            Assert.AreEqual(startDate, yearRow.StartDate, "Start date of new Curriculum Year is incorrect");

            #endregion

            #region Post-Condition

            //Delete curriculum year
            yearRow.DeleteRow();
            mySchoolDetails.Save();

            #endregion
        }

        /// <summary>
        /// TC AP-06
        /// Au : An Nguyen
        /// Description: Exercise the ability to create one new 'Year Group' with a start date on 1st day of the Next Academic Year.
        /// Role: School Administrator
        /// </summary>
        [WebDriverTest(TimeoutSeconds = 1200, Enabled = false, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { "g1" }, DataProvider = "TC_AP06_Data")]
        public void TC_AP06_Exercise_the_ability_to_create_one_new_Year_Group_with_a_start_date_on_1st_day_of_the_Next_Academic_Year(string curriculumYear, string startDate,
                    string fullName, string shortName, string displayOrder, string nextAcademicYear)
        {
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);

            #region Pre-condition : Add new Curriculum Year

            //Navigate to My School Details
            SeleniumHelper.NavigateMenu("Tasks", "School Management", "My School Details");
            var mySchoolDetails = new MySchoolDetailsPage();
            mySchoolDetails.ScrollToCurriculumYear();

            //Delete old curriculum year if exist
            var yearRow = mySchoolDetails.CurriculumYears.Rows.FirstOrDefault(t => t.NCYear.Equals(curriculumYear));
            mySchoolDetails.DeleteGridRowIfExist(yearRow);
            mySchoolDetails.Save();

            //Add new curriculum year
            mySchoolDetails.Refresh();
            yearRow = mySchoolDetails.CurriculumYears.Rows.Last();
            yearRow.NCYear = curriculumYear;
            yearRow.StartDate = startDate;

            //Save
            mySchoolDetails.Save();

            #endregion

            #region Test steps

            //Verify can navigate to Manage Year Groups by search
            var homePage = new HomePage();
            var taskSearch = homePage.TaskSearch();
            taskSearch.Search = "Manage";
            var searchResults = taskSearch.SearchResult;
            var searchItem = searchResults.Items.FirstOrDefault(t => t.TaskAction.Equals("Manage Year Groups"));
            Assert.AreNotEqual(null, searchItem, "Cannot find Manage Year Groups page from search task");

            //Navigate to Manage Year Groups
            searchItem.Click();
            var manageYearGroups = new ManageYearGroupsTriplet();

            //Search and delete old year group if exist
            manageYearGroups.Refresh();
            manageYearGroups.SearchCriteria.AcademicYear = nextAcademicYear;
            var yearGroupSearchResult = manageYearGroups.SearchCriteria.Search();
            var yearGroupTile = yearGroupSearchResult.FirstOrDefault(t => t.FullName.Equals(fullName));
            var manageYearGroup = yearGroupTile == null ? new ManageYearGroupsPage() : yearGroupTile.Click<ManageYearGroupsPage>();
            manageYearGroup.Delete();

            //Add new year group
            manageYearGroups.Refresh();
            manageYearGroup = manageYearGroups.AddYearGroup();

            //Add Year Group details
            manageYearGroup.FullName = fullName;
            manageYearGroup.ShortName = shortName;
            manageYearGroup.DisplayOrder = displayOrder;
            var activeHistoryRow = manageYearGroup.ActiveHistory.Rows.Last();
            activeHistoryRow.StartDate = startDate;

            //Add Associated Groups
            manageYearGroup.ScrollToAssociatedGroup();
            manageYearGroup.CurriculumYear = curriculumYear;

            //Save
            manageYearGroup.Save();

            //Verify success messsage display
            Assert.AreEqual(true, manageYearGroup.IsMessageSuccessAppear(), "Success message does not display");

            //Search next academic year
            manageYearGroups.Refresh();
            manageYearGroups.SearchCriteria.AcademicYear = nextAcademicYear;
            yearGroupSearchResult = manageYearGroups.SearchCriteria.Search();
            yearGroupTile = yearGroupSearchResult.FirstOrDefault(t => t.FullName.Equals(fullName));
            Assert.AreNotEqual(null, yearGroupTile, "Add Year Group to next Academic year unsuccessfull");

            #endregion

            #region Post-Condition

            //Delete Year group
            manageYearGroup = yearGroupTile.Click<ManageYearGroupsPage>();
            manageYearGroup.Delete();

            //Delete curriculum year
            SeleniumHelper.NavigateMenu("Tasks", "School Management", "My School Details");
            mySchoolDetails = new MySchoolDetailsPage();
            yearRow = mySchoolDetails.CurriculumYears.Rows.FirstOrDefault(t => t.NCYear.Equals(curriculumYear));
            yearRow.DeleteRow();
            mySchoolDetails.Save();

            #endregion
        }

        /// <summary>
        /// TC AP-07
        /// Au : An Nguyen
        /// Description: Exercise the ability to create four new 'Classes' each with a start date on 1st day of the Next Academic Year.
        /// Role: School Administrator
        /// </summary>
        [WebDriverTest(TimeoutSeconds = 1200, Enabled = false, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { "g1" }, DataProvider = "TC_AP07_Data")]
        public void TC_AP07_Exercise_the_ability_to_create_four_new_Classes_each_with_a_start_date_on_1st_day_of_the_Next_Academic_Year(string[] yearGroup, string startDate,
                    string[] class1, string[] class2, string[] class3, string[] class4, string nextAcademic)
        {
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);

            #region Pre-Condition : Create new year group

            //Navigate to Manage Year Groups
            SeleniumHelper.NavigateMenu("Tasks", "School Groups", "Manage Year Groups");
            var manageYearGroups = new ManageYearGroupsTriplet();

            //Search and delete old year group if exist
            manageYearGroups.Refresh();
            manageYearGroups.SearchCriteria.AcademicYear = nextAcademic;
            var yearGroupSearchResult = manageYearGroups.SearchCriteria.Search();
            var yearGroupTile = yearGroupSearchResult.FirstOrDefault(t => t.FullName.Equals(yearGroup[0]));
            var manageYearGroup = yearGroupTile == null ? new ManageYearGroupsPage() : yearGroupTile.Click<ManageYearGroupsPage>();
            manageYearGroup.Delete();

            //Add new year group
            manageYearGroups.Refresh();
            manageYearGroup = manageYearGroups.AddYearGroup();

            //Add Year Group details
            manageYearGroup.FullName = yearGroup[0];
            manageYearGroup.ShortName = yearGroup[1];
            manageYearGroup.DisplayOrder = yearGroup[2];
            var yearGroupActiveHistoryRow = manageYearGroup.ActiveHistory.Rows.Last();
            yearGroupActiveHistoryRow.StartDate = startDate;

            //Add Associated Groups
            manageYearGroup.ScrollToAssociatedGroup();
            manageYearGroup.CurriculumYear = yearGroup[3];

            //Save
            manageYearGroup.Save();

            #endregion

            #region Test steps

            //Verify can navigate to Manage Classes by search
            var homePage = new HomePage();
            var taskSearch = homePage.TaskSearch();
            taskSearch.Search = "Manage";
            var searchResults = taskSearch.SearchResult;
            var searchItem = searchResults.Items.FirstOrDefault(t => t.TaskAction.Equals("Manage Classes"));
            Assert.AreNotEqual(null, searchItem, "Cannot find Manage Classes page from search task");

            //Navigate to Manage Year Groups
            searchItem.Click();
            var manageClasses = new ManageClassesTriplet();

            //Add the first class
            manageClasses.Refresh();
            var manageClass = manageClasses.Create();
            manageClass.ClassFullName = class1[0];
            manageClass.ClassShortName = class1[1];
            manageClass.DisplayOrder = class1[2];
            var activeHistoryRow = manageClass.ActiveHistoryTable.Rows.Last();
            activeHistoryRow.StartDate = startDate;
            manageClass.ScrollToAssociatedGroup();
            var yearGroupRow = manageClass.YearGroupsTable.Rows.Last();
            yearGroupRow.YearGroup = yearGroup[0];
            yearGroupRow.StartDate = startDate;

            //Save and verify the first class
            manageClass.Save();

            //Add the second class
            manageClasses.Refresh();
            manageClass = manageClasses.Create();
            manageClass.ClassFullName = class2[0];
            manageClass.ClassShortName = class2[1];
            manageClass.DisplayOrder = class2[2];
            activeHistoryRow = manageClass.ActiveHistoryTable.Rows.Last();
            activeHistoryRow.StartDate = startDate;
            manageClass.ScrollToAssociatedGroup();
            yearGroupRow = manageClass.YearGroupsTable.Rows.Last();
            yearGroupRow.YearGroup = yearGroup[0];
            yearGroupRow.StartDate = startDate;

            //Save and verify the second class
            manageClass.Save();

            //Add the third class
            manageClasses.Refresh();
            manageClass = manageClasses.Create();
            manageClass.ClassFullName = class3[0];
            manageClass.ClassShortName = class3[1];
            manageClass.DisplayOrder = class3[2];
            activeHistoryRow = manageClass.ActiveHistoryTable.Rows.Last();
            activeHistoryRow.StartDate = startDate;
            manageClass.ScrollToAssociatedGroup();
            yearGroupRow = manageClass.YearGroupsTable.Rows.Last();
            yearGroupRow.YearGroup = yearGroup[0];
            yearGroupRow.StartDate = startDate;

            //Save and verify the second class
            manageClass.Save();

            //Add the fourth class
            manageClasses.Refresh();
            manageClass = manageClasses.Create();
            manageClass.ClassFullName = class4[0];
            manageClass.ClassShortName = class4[1];
            manageClass.DisplayOrder = class4[2];
            activeHistoryRow = manageClass.ActiveHistoryTable.Rows.Last();
            activeHistoryRow.StartDate = startDate;
            manageClass.ScrollToAssociatedGroup();
            yearGroupRow = manageClass.YearGroupsTable.Rows.Last();
            yearGroupRow.YearGroup = yearGroup[0];
            yearGroupRow.StartDate = startDate;

            //Save and verify the second class
            manageClass.Save();

            //Search and verify class display
            manageClasses.Refresh();
            manageClasses.SearchCriteria.SearchByAcademicYear = nextAcademic;
            var searchClassResult = manageClasses.SearchCriteria.Search();

            //The first class
            var classTile = searchClassResult.FirstOrDefault(t => t.ClassFullName.Equals(class1[0]));
            Assert.AreNotEqual(null, classTile, "Can not search the first class in next academic year");

            //The second class
            classTile = searchClassResult.FirstOrDefault(t => t.ClassFullName.Equals(class2[0]));
            Assert.AreNotEqual(null, classTile, "Can not search the second class in next academic year");

            //The third class
            classTile = searchClassResult.FirstOrDefault(t => t.ClassFullName.Equals(class3[0]));
            Assert.AreNotEqual(null, classTile, "Can not search the third class in next academic year");

            //The fourth class
            classTile = searchClassResult.FirstOrDefault(t => t.ClassFullName.Equals(class4[0]));
            Assert.AreNotEqual(null, classTile, "Can not search the fourth class in next academic year");

            #endregion

            #region Post-condition
            
            //Delete the first class
            classTile = searchClassResult.FirstOrDefault(t => t.ClassFullName.Equals(class1[0]));
            manageClass = classTile.Click<ManageClassesPage>();
            yearGroupRow = manageClass.YearGroupsTable.Rows[0];
            yearGroupRow.DeleteRow();
            manageClass.Save();
            manageClass.DeleteRecord();

            //Delete the second class
            manageClasses.Refresh();
            classTile = searchClassResult.FirstOrDefault(t => t.ClassFullName.Equals(class2[0]));
            manageClass = classTile.Click<ManageClassesPage>();
            yearGroupRow = manageClass.YearGroupsTable.Rows[0];
            yearGroupRow.DeleteRow();
            manageClass.Save();
            manageClass.DeleteRecord();

            //Delete the third class
            manageClasses.Refresh();
            classTile = searchClassResult.FirstOrDefault(t => t.ClassFullName.Equals(class3[0]));
            manageClass = classTile.Click<ManageClassesPage>();
            yearGroupRow = manageClass.YearGroupsTable.Rows[0];
            yearGroupRow.DeleteRow();
            manageClass.Save();
            manageClass.DeleteRecord();

            //Delete the fourth class
            manageClasses.Refresh();
            classTile = searchClassResult.FirstOrDefault(t => t.ClassFullName.Equals(class4[0]));
            manageClass = classTile.Click<ManageClassesPage>();
            yearGroupRow = manageClass.YearGroupsTable.Rows[0];
            yearGroupRow.DeleteRow();
            manageClass.Save();
            manageClass.DeleteRecord();

            //Delete Year Group
            SeleniumHelper.NavigateMenu("Tasks", "School Groups", "Manage Year Groups");
            manageYearGroups = new ManageYearGroupsTriplet();
            manageYearGroups.SearchCriteria.AcademicYear = nextAcademic;
            yearGroupSearchResult = manageYearGroups.SearchCriteria.Search();
            yearGroupTile = yearGroupSearchResult.FirstOrDefault(t => t.FullName.Equals(yearGroup[0]));
            manageYearGroup = yearGroupTile.Click<ManageYearGroupsPage>();
            manageYearGroup.Delete();

            #endregion
        }

        /// <summary>
        /// TC AP-08
        /// Au : An Nguyen
        /// Description: Exercise the ability to 'Add New Future Pupils' with only 'Year Group' membership and with a 'Date of Admission' that falls on the first day of the Next Academic Year.
        /// Role: School Administrator
        /// </summary>
        [WebDriverTest(TimeoutSeconds = 2400, Enabled = false, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { "g1" }, DataProvider = "TC_AP08_Data")]
        public void TC_AP08_Exercise_the_ability_to_Add_New_Future_Pupils(string[] pupil1, string[] pupil2, string[] pupil3, string[] pupil4, string[] pupil5,
                    string[] pupil6, string[] pupil7, string[] pupil8, string dateOfAdmission, string yearGroup)
        {
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);

            #region Test steps

            //Navigate to Pupil Records
            SeleniumHelper.NavigateQuickLink("Pupil Records");
            var pupilRecords = new PupilRecordTriplet();

            //Add the first pupil
            var addNewPupilDialog = pupilRecords.AddNewPupil();
            addNewPupilDialog.Forename = pupil1[0];
            addNewPupilDialog.SurName = pupil1[1];
            addNewPupilDialog.Gender = pupil1[2];
            addNewPupilDialog.DateOfBirth = pupil1[3];
            var registrationDetailDialog = addNewPupilDialog.Continue();
            registrationDetailDialog.DateOfAdmission = dateOfAdmission;
            registrationDetailDialog.YearGroup = yearGroup;
            registrationDetailDialog.CreateRecord();
            var confirmRequiredDialog = new POM.Components.Common.ConfirmRequiredDialog();
            confirmRequiredDialog.ClickOk();

            //Save the first pupil
            var pupilRecord = PupilRecordPage.Create();
            pupilRecord.SavePupil();

            //Add the second pupil
            pupilRecords.Refresh();
            addNewPupilDialog = pupilRecords.AddNewPupil();
            addNewPupilDialog.Forename = pupil2[0];
            addNewPupilDialog.SurName = pupil2[1];
            addNewPupilDialog.Gender = pupil2[2];
            addNewPupilDialog.DateOfBirth = pupil2[3];
            registrationDetailDialog = addNewPupilDialog.Continue();
            registrationDetailDialog.DateOfAdmission = dateOfAdmission;
            registrationDetailDialog.YearGroup = yearGroup;
            registrationDetailDialog.CreateRecord();
            confirmRequiredDialog = new POM.Components.Common.ConfirmRequiredDialog();
            confirmRequiredDialog.ClickOk();

            //Save the second pupil
            pupilRecord = PupilRecordPage.Create();
            pupilRecord.SavePupil();

            //Add the third pupil
            pupilRecords.Refresh();
            addNewPupilDialog = pupilRecords.AddNewPupil();
            addNewPupilDialog.Forename = pupil3[0];
            addNewPupilDialog.SurName = pupil3[1];
            addNewPupilDialog.Gender = pupil3[2];
            addNewPupilDialog.DateOfBirth = pupil3[3];
            registrationDetailDialog = addNewPupilDialog.Continue();
            registrationDetailDialog.DateOfAdmission = dateOfAdmission;
            registrationDetailDialog.YearGroup = yearGroup;
            registrationDetailDialog.CreateRecord();
            confirmRequiredDialog = new POM.Components.Common.ConfirmRequiredDialog();
            confirmRequiredDialog.ClickOk();

            //Save the third pupil
            pupilRecord = PupilRecordPage.Create();
            pupilRecord.SavePupil();

            //Add the fourth pupil
            pupilRecords.Refresh();
            addNewPupilDialog = pupilRecords.AddNewPupil();
            addNewPupilDialog.Forename = pupil4[0];
            addNewPupilDialog.SurName = pupil4[1];
            addNewPupilDialog.Gender = pupil4[2];
            addNewPupilDialog.DateOfBirth = pupil4[3];
            registrationDetailDialog = addNewPupilDialog.Continue();
            registrationDetailDialog.DateOfAdmission = dateOfAdmission;
            registrationDetailDialog.YearGroup = yearGroup;
            registrationDetailDialog.CreateRecord();
            confirmRequiredDialog = new POM.Components.Common.ConfirmRequiredDialog();
            confirmRequiredDialog.ClickOk();

            //Save the fourth pupil
            pupilRecord = PupilRecordPage.Create();
            pupilRecord.SavePupil();

            //Add the fifth pupil
            pupilRecords.Refresh();
            addNewPupilDialog = pupilRecords.AddNewPupil();
            addNewPupilDialog.Forename = pupil5[0];
            addNewPupilDialog.SurName = pupil5[1];
            addNewPupilDialog.Gender = pupil5[2];
            addNewPupilDialog.DateOfBirth = pupil5[3];
            registrationDetailDialog = addNewPupilDialog.Continue();
            registrationDetailDialog.DateOfAdmission = dateOfAdmission;
            registrationDetailDialog.YearGroup = yearGroup;
            registrationDetailDialog.CreateRecord();
            confirmRequiredDialog = new POM.Components.Common.ConfirmRequiredDialog();
            confirmRequiredDialog.ClickOk();

            //Save the fifth pupil
            pupilRecord = PupilRecordPage.Create();
            pupilRecord.SavePupil();

            //Add the sixth pupil
            pupilRecords.Refresh();
            addNewPupilDialog = pupilRecords.AddNewPupil();
            addNewPupilDialog.Forename = pupil6[0];
            addNewPupilDialog.SurName = pupil6[1];
            addNewPupilDialog.Gender = pupil6[2];
            addNewPupilDialog.DateOfBirth = pupil6[3];
            registrationDetailDialog = addNewPupilDialog.Continue();
            registrationDetailDialog.DateOfAdmission = dateOfAdmission;
            registrationDetailDialog.YearGroup = yearGroup;
            registrationDetailDialog.CreateRecord();
            confirmRequiredDialog = new POM.Components.Common.ConfirmRequiredDialog();
            confirmRequiredDialog.ClickOk();

            //Save the sixth pupil
            pupilRecord = PupilRecordPage.Create();
            pupilRecord.SavePupil();

            //Add the seventh pupil
            pupilRecords.Refresh();
            addNewPupilDialog = pupilRecords.AddNewPupil();
            addNewPupilDialog.Forename = pupil7[0];
            addNewPupilDialog.SurName = pupil7[1];
            addNewPupilDialog.Gender = pupil7[2];
            addNewPupilDialog.DateOfBirth = pupil7[3];
            registrationDetailDialog = addNewPupilDialog.Continue();
            registrationDetailDialog.DateOfAdmission = dateOfAdmission;
            registrationDetailDialog.YearGroup = yearGroup;
            registrationDetailDialog.CreateRecord();
            confirmRequiredDialog = new POM.Components.Common.ConfirmRequiredDialog();
            confirmRequiredDialog.ClickOk();

            //Save the seventh pupil
            pupilRecord = PupilRecordPage.Create();
            pupilRecord.SavePupil();

            //Add the eighth pupil
            pupilRecords.Refresh();
            addNewPupilDialog = pupilRecords.AddNewPupil();
            addNewPupilDialog.Forename = pupil8[0];
            addNewPupilDialog.SurName = pupil8[1];
            addNewPupilDialog.Gender = pupil8[2];
            addNewPupilDialog.DateOfBirth = pupil8[3];
            registrationDetailDialog = addNewPupilDialog.Continue();
            registrationDetailDialog.DateOfAdmission = dateOfAdmission;
            registrationDetailDialog.YearGroup = yearGroup;
            registrationDetailDialog.CreateRecord();
            confirmRequiredDialog = new POM.Components.Common.ConfirmRequiredDialog();
            confirmRequiredDialog.ClickOk();

            //Save the eighth pupil
            pupilRecord = PupilRecordPage.Create();
            pupilRecord.SavePupil();

            //Search pupils by year group
            pupilRecords.Refresh();
            pupilRecords.SearchCriteria.IsCurrent = false;
            pupilRecords.SearchCriteria.IsFuture = true;
            pupilRecords.SearchCriteria.IsLeaver = false;
            pupilRecords.SearchCriteria.ClickSearchAdvanced(true);
            pupilRecords.SearchCriteria.YearGroup = yearGroup;
            var pupilSearchResult = pupilRecords.SearchCriteria.Search();

            //Verify the first pupil
            var pupilTile = pupilSearchResult.FirstOrDefault(t => t.Name.Equals(String.Format("{0}, {1}", pupil1[1], pupil1[0])));
            Assert.AreNotEqual(null, pupilTile, "Can not search the first pupil by year group");

            //Verify the second pupil
            pupilTile = pupilSearchResult.FirstOrDefault(t => t.Name.Equals(String.Format("{0}, {1}", pupil2[1], pupil2[0])));
            Assert.AreNotEqual(null, pupilTile, "Can not search the second pupil by year group");

            //Verify the third pupil
            pupilTile = pupilSearchResult.FirstOrDefault(t => t.Name.Equals(String.Format("{0}, {1}", pupil3[1], pupil3[0])));
            Assert.AreNotEqual(null, pupilTile, "Can not search the third pupil by year group");

            //Verify the fourth pupil
            pupilTile = pupilSearchResult.FirstOrDefault(t => t.Name.Equals(String.Format("{0}, {1}", pupil4[1], pupil4[0])));
            Assert.AreNotEqual(null, pupilTile, "Can not search the fourth pupil by year group");

            //Verify the fifth pupil
            pupilTile = pupilSearchResult.FirstOrDefault(t => t.Name.Equals(String.Format("{0}, {1}", pupil5[1], pupil5[0])));
            Assert.AreNotEqual(null, pupilTile, "Can not search the fifth pupil by year group");

            //Verify the sixth pupil
            pupilTile = pupilSearchResult.FirstOrDefault(t => t.Name.Equals(String.Format("{0}, {1}", pupil6[1], pupil6[0])));
            Assert.AreNotEqual(null, pupilTile, "Can not search the sixth pupil by year group");

            //Verify the seventh pupil
            pupilTile = pupilSearchResult.FirstOrDefault(t => t.Name.Equals(String.Format("{0}, {1}", pupil7[1], pupil7[0])));
            Assert.AreNotEqual(null, pupilTile, "Can not search the seventh pupil by year group");

            //Verify the eighth pupil
            pupilTile = pupilSearchResult.FirstOrDefault(t => t.Name.Equals(String.Format("{0}, {1}", pupil8[1], pupil8[0])));
            Assert.AreNotEqual(null, pupilTile, "Can not search the eighth pupil by year group");

            #endregion

            #region Post-Condition : Delete 8 future pupil

            SeleniumHelper.NavigateMenu("Tasks", "Pupils", "Delete Pupil");
            var deletePupilRecords = new DeletePupilRecordTriplet();
            deletePupilRecords.SearchCriteria.IsFuture = true;

            //Delete the first pupil
            deletePupilRecords.SearchCriteria.PupilName = String.Format("{0}, {1}", pupil1[1], pupil1[0]);
            var deleteResultPupils = deletePupilRecords.SearchCriteria.Search();
            var deletePupilSearchTile = deleteResultPupils.FirstOrDefault(t => t.Name.Equals(String.Format("{0}, {1}", pupil1[1], pupil1[0])));
            var deletePupilRecord = deletePupilSearchTile.Click<DeletePupilRecordPage>();
            deletePupilRecord.Delete();

            //Delete the second pupil
            deletePupilRecords.Refresh();
            deletePupilRecords.SearchCriteria.PupilName = String.Format("{0}, {1}", pupil2[1], pupil2[0]);
            deleteResultPupils = deletePupilRecords.SearchCriteria.Search();
            deletePupilSearchTile = deleteResultPupils.FirstOrDefault(t => t.Name.Equals(String.Format("{0}, {1}", pupil2[1], pupil2[0])));
            deletePupilRecord = deletePupilSearchTile.Click<DeletePupilRecordPage>();
            deletePupilRecord.Delete();

            //Delete the third pupil
            deletePupilRecords.Refresh();
            deletePupilRecords.SearchCriteria.PupilName = String.Format("{0}, {1}", pupil3[1], pupil3[0]);
            deleteResultPupils = deletePupilRecords.SearchCriteria.Search();
            deletePupilSearchTile = deleteResultPupils.FirstOrDefault(t => t.Name.Equals(String.Format("{0}, {1}", pupil3[1], pupil3[0])));
            deletePupilRecord = deletePupilSearchTile.Click<DeletePupilRecordPage>();
            deletePupilRecord.Delete();

            //Delete the fourth pupil
            deletePupilRecords.Refresh();
            deletePupilRecords.SearchCriteria.PupilName = String.Format("{0}, {1}", pupil4[1], pupil4[0]);
            deleteResultPupils = deletePupilRecords.SearchCriteria.Search();
            deletePupilSearchTile = deleteResultPupils.FirstOrDefault(t => t.Name.Equals(String.Format("{0}, {1}", pupil4[1], pupil4[0])));
            deletePupilRecord = deletePupilSearchTile.Click<DeletePupilRecordPage>();
            deletePupilRecord.Delete();

            //Delete the fifth pupil
            deletePupilRecords.Refresh();
            deletePupilRecords.SearchCriteria.PupilName = String.Format("{0}, {1}", pupil5[1], pupil5[0]);
            deleteResultPupils = deletePupilRecords.SearchCriteria.Search();
            deletePupilSearchTile = deleteResultPupils.FirstOrDefault(t => t.Name.Equals(String.Format("{0}, {1}", pupil5[1], pupil5[0])));
            deletePupilRecord = deletePupilSearchTile.Click<DeletePupilRecordPage>();
            deletePupilRecord.Delete();

            //Delete the sixth pupil
            deletePupilRecords.Refresh();
            deletePupilRecords.SearchCriteria.PupilName = String.Format("{0}, {1}", pupil6[1], pupil6[0]);
            deleteResultPupils = deletePupilRecords.SearchCriteria.Search();
            deletePupilSearchTile = deleteResultPupils.FirstOrDefault(t => t.Name.Equals(String.Format("{0}, {1}", pupil6[1], pupil6[0])));
            deletePupilRecord = deletePupilSearchTile.Click<DeletePupilRecordPage>();
            deletePupilRecord.Delete();

            //Delete the seventh pupil
            deletePupilRecords.Refresh();
            deletePupilRecords.SearchCriteria.PupilName = String.Format("{0}, {1}", pupil7[1], pupil7[0]);
            deleteResultPupils = deletePupilRecords.SearchCriteria.Search();
            deletePupilSearchTile = deleteResultPupils.FirstOrDefault(t => t.Name.Equals(String.Format("{0}, {1}", pupil7[1], pupil7[0])));
            deletePupilRecord = deletePupilSearchTile.Click<DeletePupilRecordPage>();
            deletePupilRecord.Delete();

            //Delete the eighth pupil
            deletePupilRecords.Refresh();
            deletePupilRecords.SearchCriteria.PupilName = String.Format("{0}, {1}", pupil8[1], pupil8[0]);
            deleteResultPupils = deletePupilRecords.SearchCriteria.Search();
            deletePupilSearchTile = deleteResultPupils.FirstOrDefault(t => t.Name.Equals(String.Format("{0}, {1}", pupil8[1], pupil8[0])));
            deletePupilRecord = deletePupilSearchTile.Click<DeletePupilRecordPage>();
            deletePupilRecord.Delete();

            #endregion
        }

        /// <summary>
        /// TC AP-09
        /// Au : An Nguyen
        /// Description: Exercise ability to 'Allocate Future Pupils to Classes' for pupils who have only been assigned to a 'Year Group'.
        /// Role: School Administrator
        /// </summary>
        [WebDriverTest(TimeoutSeconds = 2400, Enabled = false, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { "g1" }, DataProvider = "TC_AP09_Data")]
        public void TC_AP09_Exercise_ability_to_Allocate_Future_Pupils_to_Classes(string[] pupil1, string[] pupil2, string[] pupil3, string[] pupil4, string[] pupil5,
                    string[] pupil6, string[] pupil7, string[] pupil8, string dateOfAdmission, string yearGroup, string className1, string className2)
        {
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);

            #region Pre-Condition : Add new 8 future pupils

            //Navigate to Pupil Records
            SeleniumHelper.NavigateQuickLink("Pupil Records");
            var pupilRecords = new PupilRecordTriplet();

            //Add the first pupil
            var addNewPupilDialog = pupilRecords.AddNewPupil();
            addNewPupilDialog.Forename = pupil1[0];
            addNewPupilDialog.SurName = pupil1[1];
            addNewPupilDialog.Gender = pupil1[2];
            addNewPupilDialog.DateOfBirth = pupil1[3];
            var registrationDetailDialog = addNewPupilDialog.Continue();
            registrationDetailDialog.DateOfAdmission = dateOfAdmission;
            registrationDetailDialog.YearGroup = yearGroup;
            registrationDetailDialog.CreateRecord();
            var confirmRequiredDialog = new POM.Components.Common.ConfirmRequiredDialog();
            confirmRequiredDialog.ClickOk();

            //Save the first pupil
            var pupilRecord = PupilRecordPage.Create();
            pupilRecord.SavePupil();

            //Add the second pupil
            pupilRecords.Refresh();
            addNewPupilDialog = pupilRecords.AddNewPupil();
            addNewPupilDialog.Forename = pupil2[0];
            addNewPupilDialog.SurName = pupil2[1];
            addNewPupilDialog.Gender = pupil2[2];
            addNewPupilDialog.DateOfBirth = pupil2[3];
            registrationDetailDialog = addNewPupilDialog.Continue();
            registrationDetailDialog.DateOfAdmission = dateOfAdmission;
            registrationDetailDialog.YearGroup = yearGroup;
            registrationDetailDialog.CreateRecord();
            confirmRequiredDialog = new POM.Components.Common.ConfirmRequiredDialog();
            confirmRequiredDialog.ClickOk();

            //Save the second pupil
            pupilRecord = PupilRecordPage.Create();
            pupilRecord.SavePupil();

            //Add the third pupil
            pupilRecords.Refresh();
            addNewPupilDialog = pupilRecords.AddNewPupil();
            addNewPupilDialog.Forename = pupil3[0];
            addNewPupilDialog.SurName = pupil3[1];
            addNewPupilDialog.Gender = pupil3[2];
            addNewPupilDialog.DateOfBirth = pupil3[3];
            registrationDetailDialog = addNewPupilDialog.Continue();
            registrationDetailDialog.DateOfAdmission = dateOfAdmission;
            registrationDetailDialog.YearGroup = yearGroup;
            registrationDetailDialog.CreateRecord();
            confirmRequiredDialog = new POM.Components.Common.ConfirmRequiredDialog();
            confirmRequiredDialog.ClickOk();

            //Save the third pupil
            pupilRecord = PupilRecordPage.Create();
            pupilRecord.SavePupil();

            //Add the fourth pupil
            pupilRecords.Refresh();
            addNewPupilDialog = pupilRecords.AddNewPupil();
            addNewPupilDialog.Forename = pupil4[0];
            addNewPupilDialog.SurName = pupil4[1];
            addNewPupilDialog.Gender = pupil4[2];
            addNewPupilDialog.DateOfBirth = pupil4[3];
            registrationDetailDialog = addNewPupilDialog.Continue();
            registrationDetailDialog.DateOfAdmission = dateOfAdmission;
            registrationDetailDialog.YearGroup = yearGroup;
            registrationDetailDialog.CreateRecord();
            confirmRequiredDialog = new POM.Components.Common.ConfirmRequiredDialog();
            confirmRequiredDialog.ClickOk();

            //Save the fourth pupil
            pupilRecord = PupilRecordPage.Create();
            pupilRecord.SavePupil();

            //Add the fifth pupil
            pupilRecords.Refresh();
            addNewPupilDialog = pupilRecords.AddNewPupil();
            addNewPupilDialog.Forename = pupil5[0];
            addNewPupilDialog.SurName = pupil5[1];
            addNewPupilDialog.Gender = pupil5[2];
            addNewPupilDialog.DateOfBirth = pupil5[3];
            registrationDetailDialog = addNewPupilDialog.Continue();
            registrationDetailDialog.DateOfAdmission = dateOfAdmission;
            registrationDetailDialog.YearGroup = yearGroup;
            registrationDetailDialog.CreateRecord();
            confirmRequiredDialog = new POM.Components.Common.ConfirmRequiredDialog();
            confirmRequiredDialog.ClickOk();

            //Save the fifth pupil
            pupilRecord = PupilRecordPage.Create();
            pupilRecord.SavePupil();

            //Add the sixth pupil
            pupilRecords.Refresh();
            addNewPupilDialog = pupilRecords.AddNewPupil();
            addNewPupilDialog.Forename = pupil6[0];
            addNewPupilDialog.SurName = pupil6[1];
            addNewPupilDialog.Gender = pupil6[2];
            addNewPupilDialog.DateOfBirth = pupil6[3];
            registrationDetailDialog = addNewPupilDialog.Continue();
            registrationDetailDialog.DateOfAdmission = dateOfAdmission;
            registrationDetailDialog.YearGroup = yearGroup;
            registrationDetailDialog.CreateRecord();
            confirmRequiredDialog = new POM.Components.Common.ConfirmRequiredDialog();
            confirmRequiredDialog.ClickOk();

            //Save the sixth pupil
            pupilRecord = PupilRecordPage.Create();
            pupilRecord.SavePupil();

            //Add the seventh pupil
            pupilRecords.Refresh();
            addNewPupilDialog = pupilRecords.AddNewPupil();
            addNewPupilDialog.Forename = pupil7[0];
            addNewPupilDialog.SurName = pupil7[1];
            addNewPupilDialog.Gender = pupil7[2];
            addNewPupilDialog.DateOfBirth = pupil7[3];
            registrationDetailDialog = addNewPupilDialog.Continue();
            registrationDetailDialog.DateOfAdmission = dateOfAdmission;
            registrationDetailDialog.YearGroup = yearGroup;
            registrationDetailDialog.CreateRecord();
            confirmRequiredDialog = new POM.Components.Common.ConfirmRequiredDialog();
            confirmRequiredDialog.ClickOk();

            //Save the seventh pupil
            pupilRecord = PupilRecordPage.Create();
            pupilRecord.SavePupil();

            //Add the eighth pupil
            pupilRecords.Refresh();
            addNewPupilDialog = pupilRecords.AddNewPupil();
            addNewPupilDialog.Forename = pupil8[0];
            addNewPupilDialog.SurName = pupil8[1];
            addNewPupilDialog.Gender = pupil8[2];
            addNewPupilDialog.DateOfBirth = pupil8[3];
            registrationDetailDialog = addNewPupilDialog.Continue();
            registrationDetailDialog.DateOfAdmission = dateOfAdmission;
            registrationDetailDialog.YearGroup = yearGroup;
            registrationDetailDialog.CreateRecord();
            confirmRequiredDialog = new POM.Components.Common.ConfirmRequiredDialog();
            confirmRequiredDialog.ClickOk();

            //Save the eighth pupil
            pupilRecord = PupilRecordPage.Create();
            pupilRecord.SavePupil();

            #endregion

            #region Test steps

            //Navigate to "Allocate Future Pupils"
            SeleniumHelper.NavigateMenu("Tasks", "School Groups", "Allocate Future Pupils");
            var allocateFuturePupils = new AllocateFuturePupilsTriplet();

            //Search future pupils with year group
            allocateFuturePupils.SearchCriteria.YearGroups[yearGroup].Select = true;
            var allocateFutureDetail = allocateFuturePupils.SearchCriteria.Search<AllocateFuturePupilsPage>();
            
            //Add class to 4 top pupils
            allocateFutureDetail.AllocateTable[String.Format("{0}, {1}", pupil1[1], pupil1[0])][5].ValueDropDown = className1;
            allocateFutureDetail.AllocateTable[String.Format("{0}, {1}", pupil2[1], pupil2[0])][5].ValueDropDown = className1;
            allocateFutureDetail.AllocateTable[String.Format("{0}, {1}", pupil3[1], pupil3[0])][5].ValueDropDown = className1;
            allocateFutureDetail.AllocateTable[String.Format("{0}, {1}", pupil4[1], pupil4[0])][5].ValueDropDown = className1;

            //Add class to 4 bottom pupils
            allocateFutureDetail.AllocateTable[String.Format("{0}, {1}", pupil5[1], pupil5[0])][5].ValueDropDown = className2;
            allocateFutureDetail.AllocateTable[String.Format("{0}, {1}", pupil6[1], pupil6[0])][5].ValueDropDown = className2;
            allocateFutureDetail.AllocateTable[String.Format("{0}, {1}", pupil7[1], pupil7[0])][5].ValueDropDown = className2;
            allocateFutureDetail.AllocateTable[String.Format("{0}, {1}", pupil8[1], pupil8[0])][5].ValueDropDown = className2;

            //Save
            allocateFutureDetail.Save();

            //Search future pupils after add class
            allocateFuturePupils.Refresh();
            allocateFuturePupils.SearchCriteria.YearGroups[yearGroup].Select = true;
            allocateFutureDetail = allocateFuturePupils.SearchCriteria.Search<AllocateFuturePupilsPage>();

            //Verify class of 4 top pupils
            Assert.AreEqual(className1, allocateFutureDetail.AllocateTable[String.Format("{0}, {1}", pupil1[1], pupil1[0])][5].ValueDropDown, "Class of the first pupil is incorrect");
            Assert.AreEqual(className1, allocateFutureDetail.AllocateTable[String.Format("{0}, {1}", pupil2[1], pupil2[0])][5].ValueDropDown, "Class of the second pupil is incorrect");
            Assert.AreEqual(className1, allocateFutureDetail.AllocateTable[String.Format("{0}, {1}", pupil3[1], pupil3[0])][5].ValueDropDown, "Class of the third pupil is incorrect");
            Assert.AreEqual(className1, allocateFutureDetail.AllocateTable[String.Format("{0}, {1}", pupil4[1], pupil4[0])][5].ValueDropDown, "Class of the fourth pupil is incorrect");

            //Verify class of 4 bottom pupils
            Assert.AreEqual(className2, allocateFutureDetail.AllocateTable[String.Format("{0}, {1}", pupil5[1], pupil5[0])][5].ValueDropDown, "Class of the fifth pupil is incorrect");
            Assert.AreEqual(className2, allocateFutureDetail.AllocateTable[String.Format("{0}, {1}", pupil6[1], pupil6[0])][5].ValueDropDown, "Class of the sixth pupil is incorrect");
            Assert.AreEqual(className2, allocateFutureDetail.AllocateTable[String.Format("{0}, {1}", pupil7[1], pupil7[0])][5].ValueDropDown, "Class of the seventh pupil is incorrect");
            Assert.AreEqual(className2, allocateFutureDetail.AllocateTable[String.Format("{0}, {1}", pupil8[1], pupil8[0])][5].ValueDropDown, "Class of the eighth pupil is incorrect");
            
            #endregion

            #region Post-Condition : Delete 8 future pupil

            SeleniumHelper.NavigateMenu("Tasks", "Pupils", "Delete Pupil");
            var deletePupilRecords = new DeletePupilRecordTriplet();
            deletePupilRecords.SearchCriteria.IsFuture = true;

            //Delete the first pupil
            deletePupilRecords.SearchCriteria.PupilName = String.Format("{0}, {1}", pupil1[1], pupil1[0]);
            var deleteResultPupils = deletePupilRecords.SearchCriteria.Search();
            var deletePupilSearchTile = deleteResultPupils.FirstOrDefault(t => t.Name.Equals(String.Format("{0}, {1}", pupil1[1], pupil1[0])));
            var deletePupilRecord = deletePupilSearchTile.Click<DeletePupilRecordPage>();
            deletePupilRecord.Delete();

            //Delete the second pupil
            deletePupilRecords.Refresh();
            deletePupilRecords.SearchCriteria.PupilName = String.Format("{0}, {1}", pupil2[1], pupil2[0]);
            deleteResultPupils = deletePupilRecords.SearchCriteria.Search();
            deletePupilSearchTile = deleteResultPupils.FirstOrDefault(t => t.Name.Equals(String.Format("{0}, {1}", pupil2[1], pupil2[0])));
            deletePupilRecord = deletePupilSearchTile.Click<DeletePupilRecordPage>();
            deletePupilRecord.Delete();

            //Delete the third pupil
            deletePupilRecords.Refresh();
            deletePupilRecords.SearchCriteria.PupilName = String.Format("{0}, {1}", pupil3[1], pupil3[0]);
            deleteResultPupils = deletePupilRecords.SearchCriteria.Search();
            deletePupilSearchTile = deleteResultPupils.FirstOrDefault(t => t.Name.Equals(String.Format("{0}, {1}", pupil3[1], pupil3[0])));
            deletePupilRecord = deletePupilSearchTile.Click<DeletePupilRecordPage>();
            deletePupilRecord.Delete();

            //Delete the fourth pupil
            deletePupilRecords.Refresh();
            deletePupilRecords.SearchCriteria.PupilName = String.Format("{0}, {1}", pupil4[1], pupil4[0]);
            deleteResultPupils = deletePupilRecords.SearchCriteria.Search();
            deletePupilSearchTile = deleteResultPupils.FirstOrDefault(t => t.Name.Equals(String.Format("{0}, {1}", pupil4[1], pupil4[0])));
            deletePupilRecord = deletePupilSearchTile.Click<DeletePupilRecordPage>();
            deletePupilRecord.Delete();

            //Delete the fifth pupil
            deletePupilRecords.Refresh();
            deletePupilRecords.SearchCriteria.PupilName = String.Format("{0}, {1}", pupil5[1], pupil5[0]);
            deleteResultPupils = deletePupilRecords.SearchCriteria.Search();
            deletePupilSearchTile = deleteResultPupils.FirstOrDefault(t => t.Name.Equals(String.Format("{0}, {1}", pupil5[1], pupil5[0])));
            deletePupilRecord = deletePupilSearchTile.Click<DeletePupilRecordPage>();
            deletePupilRecord.Delete();

            //Delete the sixth pupil
            deletePupilRecords.Refresh();
            deletePupilRecords.SearchCriteria.PupilName = String.Format("{0}, {1}", pupil6[1], pupil6[0]);
            deleteResultPupils = deletePupilRecords.SearchCriteria.Search();
            deletePupilSearchTile = deleteResultPupils.FirstOrDefault(t => t.Name.Equals(String.Format("{0}, {1}", pupil6[1], pupil6[0])));
            deletePupilRecord = deletePupilSearchTile.Click<DeletePupilRecordPage>();
            deletePupilRecord.Delete();

            //Delete the seventh pupil
            deletePupilRecords.Refresh();
            deletePupilRecords.SearchCriteria.PupilName = String.Format("{0}, {1}", pupil7[1], pupil7[0]);
            deleteResultPupils = deletePupilRecords.SearchCriteria.Search();
            deletePupilSearchTile = deleteResultPupils.FirstOrDefault(t => t.Name.Equals(String.Format("{0}, {1}", pupil7[1], pupil7[0])));
            deletePupilRecord = deletePupilSearchTile.Click<DeletePupilRecordPage>();
            deletePupilRecord.Delete();

            //Delete the eighth pupil
            deletePupilRecords.Refresh();
            deletePupilRecords.SearchCriteria.PupilName = String.Format("{0}, {1}", pupil8[1], pupil8[0]);
            deleteResultPupils = deletePupilRecords.SearchCriteria.Search();
            deletePupilSearchTile = deleteResultPupils.FirstOrDefault(t => t.Name.Equals(String.Format("{0}, {1}", pupil8[1], pupil8[0])));
            deletePupilRecord = deletePupilSearchTile.Click<DeletePupilRecordPage>();
            deletePupilRecord.Delete();

            #endregion
        }

        /// <summary>
        /// TC AP-10
        /// Au : An Nguyen
        /// Description: Exercise ability to 'Allocate Future Pupils to Groups' who have already been assigned to Year Groups and Classes.
        /// Role: School Administrator
        /// </summary>
        [WebDriverTest(TimeoutSeconds = 2400, Enabled = false, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { "g1" }, DataProvider = "TC_AP10_Data")]
        public void TC_AP10_Exercise_ability_to_Allocate_Future_Pupils_to_Classes_have_already_been_assigned(string[] pupil1, string[] pupil2, string[] pupil3, string[] pupil4, string[] pupil5,
                    string[] pupil6, string[] pupil7, string[] pupil8, string dateOfAdmission, string yearGroup, string className1, string className2, string className3, string className4)
        {
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);

            #region Pre-Condition : Add new 8 future pupils

            //Navigate to Pupil Records
            SeleniumHelper.NavigateQuickLink("Pupil Records");
            var pupilRecords = new PupilRecordTriplet();

            //Add the first pupil
            var addNewPupilDialog = pupilRecords.AddNewPupil();
            addNewPupilDialog.Forename = pupil1[0];
            addNewPupilDialog.SurName = pupil1[1];
            addNewPupilDialog.Gender = pupil1[2];
            addNewPupilDialog.DateOfBirth = pupil1[3];
            var registrationDetailDialog = addNewPupilDialog.Continue();
            registrationDetailDialog.DateOfAdmission = dateOfAdmission;
            registrationDetailDialog.YearGroup = yearGroup;
            registrationDetailDialog.CreateRecord();
            var confirmRequiredDialog = new POM.Components.Common.ConfirmRequiredDialog();
            confirmRequiredDialog.ClickOk();

            //Save the first pupil
            var pupilRecord = PupilRecordPage.Create();
            pupilRecord.SavePupil();

            //Add the second pupil
            pupilRecords.Refresh();
            addNewPupilDialog = pupilRecords.AddNewPupil();
            addNewPupilDialog.Forename = pupil2[0];
            addNewPupilDialog.SurName = pupil2[1];
            addNewPupilDialog.Gender = pupil2[2];
            addNewPupilDialog.DateOfBirth = pupil2[3];
            registrationDetailDialog = addNewPupilDialog.Continue();
            registrationDetailDialog.DateOfAdmission = dateOfAdmission;
            registrationDetailDialog.YearGroup = yearGroup;
            registrationDetailDialog.CreateRecord();
            confirmRequiredDialog = new POM.Components.Common.ConfirmRequiredDialog();
            confirmRequiredDialog.ClickOk();

            //Save the second pupil
            pupilRecord = PupilRecordPage.Create();
            pupilRecord.SavePupil();

            //Add the third pupil
            pupilRecords.Refresh();
            addNewPupilDialog = pupilRecords.AddNewPupil();
            addNewPupilDialog.Forename = pupil3[0];
            addNewPupilDialog.SurName = pupil3[1];
            addNewPupilDialog.Gender = pupil3[2];
            addNewPupilDialog.DateOfBirth = pupil3[3];
            registrationDetailDialog = addNewPupilDialog.Continue();
            registrationDetailDialog.DateOfAdmission = dateOfAdmission;
            registrationDetailDialog.YearGroup = yearGroup;
            registrationDetailDialog.CreateRecord();
            confirmRequiredDialog = new POM.Components.Common.ConfirmRequiredDialog();
            confirmRequiredDialog.ClickOk();

            //Save the third pupil
            pupilRecord = PupilRecordPage.Create();
            pupilRecord.SavePupil();

            //Add the fourth pupil
            pupilRecords.Refresh();
            addNewPupilDialog = pupilRecords.AddNewPupil();
            addNewPupilDialog.Forename = pupil4[0];
            addNewPupilDialog.SurName = pupil4[1];
            addNewPupilDialog.Gender = pupil4[2];
            addNewPupilDialog.DateOfBirth = pupil4[3];
            registrationDetailDialog = addNewPupilDialog.Continue();
            registrationDetailDialog.DateOfAdmission = dateOfAdmission;
            registrationDetailDialog.YearGroup = yearGroup;
            registrationDetailDialog.CreateRecord();
            confirmRequiredDialog = new POM.Components.Common.ConfirmRequiredDialog();
            confirmRequiredDialog.ClickOk();

            //Save the fourth pupil
            pupilRecord = PupilRecordPage.Create();
            pupilRecord.SavePupil();

            //Add the fifth pupil
            pupilRecords.Refresh();
            addNewPupilDialog = pupilRecords.AddNewPupil();
            addNewPupilDialog.Forename = pupil5[0];
            addNewPupilDialog.SurName = pupil5[1];
            addNewPupilDialog.Gender = pupil5[2];
            addNewPupilDialog.DateOfBirth = pupil5[3];
            registrationDetailDialog = addNewPupilDialog.Continue();
            registrationDetailDialog.DateOfAdmission = dateOfAdmission;
            registrationDetailDialog.YearGroup = yearGroup;
            registrationDetailDialog.CreateRecord();
            confirmRequiredDialog = new POM.Components.Common.ConfirmRequiredDialog();
            confirmRequiredDialog.ClickOk();

            //Save the fifth pupil
            pupilRecord = PupilRecordPage.Create();
            pupilRecord.SavePupil();

            //Add the sixth pupil
            pupilRecords.Refresh();
            addNewPupilDialog = pupilRecords.AddNewPupil();
            addNewPupilDialog.Forename = pupil6[0];
            addNewPupilDialog.SurName = pupil6[1];
            addNewPupilDialog.Gender = pupil6[2];
            addNewPupilDialog.DateOfBirth = pupil6[3];
            registrationDetailDialog = addNewPupilDialog.Continue();
            registrationDetailDialog.DateOfAdmission = dateOfAdmission;
            registrationDetailDialog.YearGroup = yearGroup;
            registrationDetailDialog.CreateRecord();
            confirmRequiredDialog = new POM.Components.Common.ConfirmRequiredDialog();
            confirmRequiredDialog.ClickOk();

            //Save the sixth pupil
            pupilRecord = PupilRecordPage.Create();
            pupilRecord.SavePupil();

            //Add the seventh pupil
            pupilRecords.Refresh();
            addNewPupilDialog = pupilRecords.AddNewPupil();
            addNewPupilDialog.Forename = pupil7[0];
            addNewPupilDialog.SurName = pupil7[1];
            addNewPupilDialog.Gender = pupil7[2];
            addNewPupilDialog.DateOfBirth = pupil7[3];
            registrationDetailDialog = addNewPupilDialog.Continue();
            registrationDetailDialog.DateOfAdmission = dateOfAdmission;
            registrationDetailDialog.YearGroup = yearGroup;
            registrationDetailDialog.CreateRecord();
            confirmRequiredDialog = new POM.Components.Common.ConfirmRequiredDialog();
            confirmRequiredDialog.ClickOk();

            //Save the seventh pupil
            pupilRecord = PupilRecordPage.Create();
            pupilRecord.SavePupil();

            //Add the eighth pupil
            pupilRecords.Refresh();
            addNewPupilDialog = pupilRecords.AddNewPupil();
            addNewPupilDialog.Forename = pupil8[0];
            addNewPupilDialog.SurName = pupil8[1];
            addNewPupilDialog.Gender = pupil8[2];
            addNewPupilDialog.DateOfBirth = pupil8[3];
            registrationDetailDialog = addNewPupilDialog.Continue();
            registrationDetailDialog.DateOfAdmission = dateOfAdmission;
            registrationDetailDialog.YearGroup = yearGroup;
            registrationDetailDialog.CreateRecord();
            confirmRequiredDialog = new POM.Components.Common.ConfirmRequiredDialog();
            confirmRequiredDialog.ClickOk();

            //Save the eighth pupil
            pupilRecord = PupilRecordPage.Create();
            pupilRecord.SavePupil();

            #endregion

            #region Test steps

            //Navigate to "Allocate Future Pupils"
            SeleniumHelper.NavigateMenu("Tasks", "School Groups", "Allocate Future Pupils");
            var allocateFuturePupils = new AllocateFuturePupilsTriplet();

            //Search future pupils with year group
            allocateFuturePupils.SearchCriteria.YearGroups[yearGroup].Select = true;
            var allocateFutureDetail = allocateFuturePupils.SearchCriteria.Search<AllocateFuturePupilsPage>();

            //Add class to 4 top pupils
            allocateFutureDetail.AllocateTable[String.Format("{0}, {1}", pupil1[1], pupil1[0])][5].ValueDropDown = className1;
            allocateFutureDetail.AllocateTable[String.Format("{0}, {1}", pupil2[1], pupil2[0])][5].ValueDropDown = className1;
            allocateFutureDetail.AllocateTable[String.Format("{0}, {1}", pupil3[1], pupil3[0])][5].ValueDropDown = className1;
            allocateFutureDetail.AllocateTable[String.Format("{0}, {1}", pupil4[1], pupil4[0])][5].ValueDropDown = className1;

            //Add class to 4 bottom pupils
            allocateFutureDetail.AllocateTable[String.Format("{0}, {1}", pupil5[1], pupil5[0])][5].ValueDropDown = className2;
            allocateFutureDetail.AllocateTable[String.Format("{0}, {1}", pupil6[1], pupil6[0])][5].ValueDropDown = className2;
            allocateFutureDetail.AllocateTable[String.Format("{0}, {1}", pupil7[1], pupil7[0])][5].ValueDropDown = className2;
            allocateFutureDetail.AllocateTable[String.Format("{0}, {1}", pupil8[1], pupil8[0])][5].ValueDropDown = className2;

            //Save
            allocateFutureDetail.Save();

            //Search future pupils after add class
            allocateFuturePupils.Refresh();
            allocateFuturePupils.SearchCriteria.YearGroups[yearGroup].Select = true;
            allocateFutureDetail = allocateFuturePupils.SearchCriteria.Search<AllocateFuturePupilsPage>();

            //Change class of 4 top pupils
            allocateFutureDetail.AllocateTable[String.Format("{0}, {1}", pupil1[1], pupil1[0])][5].ValueDropDown = className3;
            allocateFutureDetail.AllocateTable[String.Format("{0}, {1}", pupil2[1], pupil2[0])][5].ValueDropDown = className3;
            allocateFutureDetail.AllocateTable[String.Format("{0}, {1}", pupil3[1], pupil3[0])][5].ValueDropDown = className3;
            allocateFutureDetail.AllocateTable[String.Format("{0}, {1}", pupil4[1], pupil4[0])][5].ValueDropDown = className3;

            //Change class of 4 bottom pupils
            allocateFutureDetail.AllocateTable[String.Format("{0}, {1}", pupil5[1], pupil5[0])][5].ValueDropDown = className4;
            allocateFutureDetail.AllocateTable[String.Format("{0}, {1}", pupil6[1], pupil6[0])][5].ValueDropDown = className4;
            allocateFutureDetail.AllocateTable[String.Format("{0}, {1}", pupil7[1], pupil7[0])][5].ValueDropDown = className4;
            allocateFutureDetail.AllocateTable[String.Format("{0}, {1}", pupil8[1], pupil8[0])][5].ValueDropDown = className4;

            //Save
            allocateFutureDetail.Save();

            //Search future pupils after change class
            allocateFuturePupils.Refresh();
            allocateFuturePupils.SearchCriteria.YearGroups[yearGroup].Select = true;
            allocateFutureDetail = allocateFuturePupils.SearchCriteria.Search<AllocateFuturePupilsPage>();

            //Verify class of 4 top pupils
            Assert.AreEqual(className3, allocateFutureDetail.AllocateTable[String.Format("{0}, {1}", pupil1[1], pupil1[0])][5].ValueDropDown, "Class of the first pupil is incorrect");
            Assert.AreEqual(className3, allocateFutureDetail.AllocateTable[String.Format("{0}, {1}", pupil2[1], pupil2[0])][5].ValueDropDown, "Class of the second pupil is incorrect");
            Assert.AreEqual(className3, allocateFutureDetail.AllocateTable[String.Format("{0}, {1}", pupil3[1], pupil3[0])][5].ValueDropDown, "Class of the third pupil is incorrect");
            Assert.AreEqual(className3, allocateFutureDetail.AllocateTable[String.Format("{0}, {1}", pupil4[1], pupil4[0])][5].ValueDropDown, "Class of the fourth pupil is incorrect");

            //Verify class of 4 bottom pupils
            Assert.AreEqual(className4, allocateFutureDetail.AllocateTable[String.Format("{0}, {1}", pupil5[1], pupil5[0])][5].ValueDropDown, "Class of the fifth pupil is incorrect");
            Assert.AreEqual(className4, allocateFutureDetail.AllocateTable[String.Format("{0}, {1}", pupil6[1], pupil6[0])][5].ValueDropDown, "Class of the sixth pupil is incorrect");
            Assert.AreEqual(className4, allocateFutureDetail.AllocateTable[String.Format("{0}, {1}", pupil7[1], pupil7[0])][5].ValueDropDown, "Class of the seventh pupil is incorrect");
            Assert.AreEqual(className4, allocateFutureDetail.AllocateTable[String.Format("{0}, {1}", pupil8[1], pupil8[0])][5].ValueDropDown, "Class of the eighth pupil is incorrect");

            #endregion

            #region Post-Condition : Delete 8 future pupil

            SeleniumHelper.NavigateMenu("Tasks", "Pupils", "Delete Pupil");
            var deletePupilRecords = new DeletePupilRecordTriplet();
            deletePupilRecords.SearchCriteria.IsFuture = true;

            //Delete the first pupil
            deletePupilRecords.SearchCriteria.PupilName = String.Format("{0}, {1}", pupil1[1], pupil1[0]);
            var deleteResultPupils = deletePupilRecords.SearchCriteria.Search();
            var deletePupilSearchTile = deleteResultPupils.FirstOrDefault(t => t.Name.Equals(String.Format("{0}, {1}", pupil1[1], pupil1[0])));
            var deletePupilRecord = deletePupilSearchTile.Click<DeletePupilRecordPage>();
            deletePupilRecord.Delete();

            //Delete the second pupil
            deletePupilRecords.Refresh();
            deletePupilRecords.SearchCriteria.PupilName = String.Format("{0}, {1}", pupil2[1], pupil2[0]);
            deleteResultPupils = deletePupilRecords.SearchCriteria.Search();
            deletePupilSearchTile = deleteResultPupils.FirstOrDefault(t => t.Name.Equals(String.Format("{0}, {1}", pupil2[1], pupil2[0])));
            deletePupilRecord = deletePupilSearchTile.Click<DeletePupilRecordPage>();
            deletePupilRecord.Delete();

            //Delete the third pupil
            deletePupilRecords.Refresh();
            deletePupilRecords.SearchCriteria.PupilName = String.Format("{0}, {1}", pupil3[1], pupil3[0]);
            deleteResultPupils = deletePupilRecords.SearchCriteria.Search();
            deletePupilSearchTile = deleteResultPupils.FirstOrDefault(t => t.Name.Equals(String.Format("{0}, {1}", pupil3[1], pupil3[0])));
            deletePupilRecord = deletePupilSearchTile.Click<DeletePupilRecordPage>();
            deletePupilRecord.Delete();

            //Delete the fourth pupil
            deletePupilRecords.Refresh();
            deletePupilRecords.SearchCriteria.PupilName = String.Format("{0}, {1}", pupil4[1], pupil4[0]);
            deleteResultPupils = deletePupilRecords.SearchCriteria.Search();
            deletePupilSearchTile = deleteResultPupils.FirstOrDefault(t => t.Name.Equals(String.Format("{0}, {1}", pupil4[1], pupil4[0])));
            deletePupilRecord = deletePupilSearchTile.Click<DeletePupilRecordPage>();
            deletePupilRecord.Delete();

            //Delete the fifth pupil
            deletePupilRecords.Refresh();
            deletePupilRecords.SearchCriteria.PupilName = String.Format("{0}, {1}", pupil5[1], pupil5[0]);
            deleteResultPupils = deletePupilRecords.SearchCriteria.Search();
            deletePupilSearchTile = deleteResultPupils.FirstOrDefault(t => t.Name.Equals(String.Format("{0}, {1}", pupil5[1], pupil5[0])));
            deletePupilRecord = deletePupilSearchTile.Click<DeletePupilRecordPage>();
            deletePupilRecord.Delete();

            //Delete the sixth pupil
            deletePupilRecords.Refresh();
            deletePupilRecords.SearchCriteria.PupilName = String.Format("{0}, {1}", pupil6[1], pupil6[0]);
            deleteResultPupils = deletePupilRecords.SearchCriteria.Search();
            deletePupilSearchTile = deleteResultPupils.FirstOrDefault(t => t.Name.Equals(String.Format("{0}, {1}", pupil6[1], pupil6[0])));
            deletePupilRecord = deletePupilSearchTile.Click<DeletePupilRecordPage>();
            deletePupilRecord.Delete();

            //Delete the seventh pupil
            deletePupilRecords.Refresh();
            deletePupilRecords.SearchCriteria.PupilName = String.Format("{0}, {1}", pupil7[1], pupil7[0]);
            deleteResultPupils = deletePupilRecords.SearchCriteria.Search();
            deletePupilSearchTile = deleteResultPupils.FirstOrDefault(t => t.Name.Equals(String.Format("{0}, {1}", pupil7[1], pupil7[0])));
            deletePupilRecord = deletePupilSearchTile.Click<DeletePupilRecordPage>();
            deletePupilRecord.Delete();

            //Delete the eighth pupil
            deletePupilRecords.Refresh();
            deletePupilRecords.SearchCriteria.PupilName = String.Format("{0}, {1}", pupil8[1], pupil8[0]);
            deleteResultPupils = deletePupilRecords.SearchCriteria.Search();
            deletePupilSearchTile = deleteResultPupils.FirstOrDefault(t => t.Name.Equals(String.Format("{0}, {1}", pupil8[1], pupil8[0])));
            deletePupilRecord = deletePupilSearchTile.Click<DeletePupilRecordPage>();
            deletePupilRecord.Delete();

            #endregion
        }
        
        /// <summary>
        /// TC AP-11
        /// Au : Ba.Truong
        /// Description: Exercise ability to 'Promote Current Pupils' into classes with a membership start date of the Next Academic Year.
        /// Role: School Administrator
        /// </summary>        
        [WebDriverTest(TimeoutSeconds = 3000, Enabled = false, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { "g1" }, DataProvider = "TC_AP11_Data")]
        public void TC_AP11_Exercise_Ability_To_Promote_Current_Pupils_Into_Classes(string[] pupils, string newGroup, string newClass)
        {
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);

            #region Test steps

            //Enter to search "Promote Pupils", then verify "Promote Pupils" displays
            var homePage = new HomePage();
            var taskSearch = homePage.TaskSearch();
            taskSearch.Search = "Promote Pupils";
            var searchResults = taskSearch.SearchResult;
            var searchItem = searchResults.Items.FirstOrDefault(t => t.TaskAction.Equals("Promote Pupils"));
            Assert.AreNotEqual(null, searchItem, "'Promote Pupils' doesn't display in search results");

            //Navigate to "Promote Pupils", then verify "Promote Pupils" screen displays
            searchItem.Click();
            Assert.AreEqual(true, PromotePupilsTriplet.IsPageExist(), "'Promote Pupils' page doesn't display");

            //Select 'Year 6' of "Year Group"
            var promotePupilTriplet = new PromotePupilsTriplet();
            promotePupilTriplet.SearchCriteria.YearGroups["Year 6"].Select = true;
            promotePupilTriplet.Refresh();
            var promotePupilDetailPage = promotePupilTriplet.SearchCriteria.Search<PromotePupilsPage>();
            
            //Verify pupil display
            Assert.AreEqual(true, promotePupilDetailPage.PromoteTable.RowCount > 0, "List of pupil doesn't display");

            //Hide some columns
            var additionalColumnDialog = promotePupilDetailPage.ClickAdditionalColumnButton();
            additionalColumnDialog.ClearSelection();
            promotePupilDetailPage = additionalColumnDialog.ClickOK<PromotePupilsPage>();

            //Input "Year 7" to "New Year Group" of the pupil
            promotePupilDetailPage.PromoteTable[pupils[0]][2].CellText = newGroup;
            promotePupilDetailPage.PromoteTable[pupils[1]][2].CellText = newGroup;
            promotePupilDetailPage.PromoteTable[pupils[2]][2].CellText = newGroup;

            //Input "7A" to "New Class" of the pupil
            promotePupilDetailPage.PromoteTable[pupils[0]][4].CellText = newClass;
            promotePupilDetailPage.PromoteTable[pupils[1]][4].CellText = newClass;
            promotePupilDetailPage.PromoteTable[pupils[2]][4].CellText = newClass;
            
            //Click Save
            promotePupilDetailPage.Save();

            //Verify "New Year Group" of the pupil was updated
            Assert.AreEqual(newGroup, promotePupilDetailPage.PromoteTable[pupils[0]][2].CellText, "New Year Group is not updated");
            Assert.AreEqual(newGroup, promotePupilDetailPage.PromoteTable[pupils[1]][2].CellText, "New Year Group is not updated");
            Assert.AreEqual(newGroup, promotePupilDetailPage.PromoteTable[pupils[2]][2].CellText, "New Year Group is not updated");

            //Verify "New Class" of the pupil was updated
            Assert.AreEqual(newClass, promotePupilDetailPage.PromoteTable[pupils[0]][4].CellText, "New Class is not updated");
            Assert.AreEqual(newClass, promotePupilDetailPage.PromoteTable[pupils[1]][4].CellText, "New Class is not updated");
            Assert.AreEqual(newClass, promotePupilDetailPage.PromoteTable[pupils[2]][4].CellText, "New Class is not updated");

            #endregion

            #region Post-Condition

            //Change data at "New Year Group" column for running test at next time
            promotePupilDetailPage.PromoteTable[pupils[0]][2].CellText = "Year 1";
            promotePupilDetailPage.PromoteTable[pupils[1]][2].CellText = "Year 1";
            promotePupilDetailPage.PromoteTable[pupils[2]][2].CellText = "Year 1";

            //Change data at "New Class" column for running test at next time
            promotePupilDetailPage.PromoteTable[pupils[0]][4].CellText = "SEN";
            promotePupilDetailPage.PromoteTable[pupils[1]][4].CellText = "SEN";
            promotePupilDetailPage.PromoteTable[pupils[2]][4].CellText = "SEN";

            //Click Save
            promotePupilDetailPage.Save();

            #endregion
        }

        /// <summary>
        /// TC AP-12
        /// Au : Ba.Truong
        /// Description: Exercise ability to 'Change Pupil Promotions' from the 'Classes' they are already assigned to other 'Classes'.
        /// Role: School Administrator
        /// </summary>
        [WebDriverTest(TimeoutSeconds = 3000, Enabled = false, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { "g1" }, DataProvider = "TC_AP12_Data")]
        public void TC_AP12_Exercise_Ability_To_Change_Pupil_Promotions_From_Classes_They_Assigned_To_Other_Classes(string[] pupils, string originalNewClass, string updateNewClass)
        {
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);

            #region Pre-Conditions

            //Navigate to "Promote Pupils" page
            SeleniumHelper.NavigateBySearch("Promote Pupils", true);

            //Select 'Year 6' of "Year Group"
            var promotePupilTriplet = new PromotePupilsTriplet();
            promotePupilTriplet.SearchCriteria.YearGroups["Year 6"].Select = true;
            promotePupilTriplet.Refresh();
            var promotePupilDetailPage = promotePupilTriplet.SearchCriteria.Search<PromotePupilsPage>();
            
            //Hide some columns
            var additionalColumnDialog = promotePupilDetailPage.ClickAdditionalColumnButton();
            additionalColumnDialog.ClearSelection();
            promotePupilDetailPage = additionalColumnDialog.ClickOK<PromotePupilsPage>();

            //Input "7A" to "New Class" of the pupil
            promotePupilDetailPage.PromoteTable[pupils[0]][4].CellText = originalNewClass;
            promotePupilDetailPage.PromoteTable[pupils[1]][4].CellText = originalNewClass;
            promotePupilDetailPage.PromoteTable[pupils[2]][4].CellText = originalNewClass;

            //Click Save
            promotePupilDetailPage.Save();

            #endregion

            #region Test steps

            //Change "New Class" of the pupil from "7A" to "6A"
            promotePupilDetailPage.PromoteTable[pupils[0]][4].CellText = updateNewClass;
            promotePupilDetailPage.PromoteTable[pupils[1]][4].CellText = updateNewClass;
            promotePupilDetailPage.PromoteTable[pupils[2]][4].CellText = updateNewClass;

            //Click Save
            promotePupilDetailPage.Save();

            //Perform search action to check information was saved or not
            promotePupilDetailPage = promotePupilTriplet.SearchCriteria.Search<PromotePupilsPage>();

            //Verify "New Class" of the pupil was updated
            Assert.AreEqual(updateNewClass, promotePupilDetailPage.PromoteTable[pupils[0]][4].CellText, "New Class is not updated");
            Assert.AreEqual(updateNewClass, promotePupilDetailPage.PromoteTable[pupils[1]][4].CellText, "New Class is not updated");
            Assert.AreEqual(updateNewClass, promotePupilDetailPage.PromoteTable[pupils[2]][4].CellText, "New Class is not updated");

            #endregion

            #region Post-Condition

            //Change data at "New Class" column for running test at next time
            promotePupilDetailPage.PromoteTable[pupils[0]][4].CellText = originalNewClass;
            promotePupilDetailPage.PromoteTable[pupils[1]][4].CellText = originalNewClass;
            promotePupilDetailPage.PromoteTable[pupils[2]][4].CellText = originalNewClass;

            //Click Save
            promotePupilDetailPage.Save();

            #endregion
        }

        #region DATA

        public List<object[]> TC_AP01_Data()
        {
            string pattern = "M/d/yyyy";
            string dateOfBirth = DateTime.ParseExact("06/01/2006", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            string dateOfAdmission = DateTime.Now.Subtract(TimeSpan.FromDays(30)).ToString(pattern);
            string foreName = String.Format("{0}_{1}", SeleniumHelper.GenerateRandomString(7), SeleniumHelper.GenerateRandomNumberUsingDateTime());
            string surName = String.Format("{0}_{1}", "Avn", SeleniumHelper.GenerateRandomString(8));
            var res = new List<Object[]>
            {                
                new object[] 
                {
                    new string[]{String.Format("{0}{1}", foreName, 1), String.Format("{0}{1}", surName, 1), "Male", dateOfBirth},
                    new string[]{String.Format("{0}{1}", foreName, 2), String.Format("{0}{1}", surName, 2), "Male", dateOfBirth},
                    new string[]{String.Format("{0}{1}", foreName, 3), String.Format("{0}{1}", surName, 3), "Male", dateOfBirth},
                    new string[]{String.Format("{0}{1}", foreName, 4), String.Format("{0}{1}", surName, 4), "Male", dateOfBirth},
                    new string[]{String.Format("{0}{1}", foreName, 5), String.Format("{0}{1}", surName, 5), "Male", dateOfBirth},
                    new string[]{String.Format("{0}{1}", foreName, 6), String.Format("{0}{1}", surName, 6), "Male", dateOfBirth},
                    new string[]{String.Format("{0}{1}", foreName, 7), String.Format("{0}{1}", surName, 7), "Male", dateOfBirth},
                    new string[]{String.Format("{0}{1}", foreName, 8), String.Format("{0}{1}", surName, 8), "Male", dateOfBirth},
                    new string[]{String.Format("{0}{1}", foreName, 9), String.Format("{0}{1}", surName, 9), "Male", dateOfBirth},
                    new string[]{String.Format("{0}_{1}", foreName, 10), String.Format("{0}_{1}", surName, 10), "Male", dateOfBirth},
                    dateOfAdmission, "Year 2",
                }
                
            };
            return res;
        }

        public List<object[]> TC_AP02_Data()
        {
            string pattern = "M/d/yyyy";
            string dateOfBirth = DateTime.ParseExact("06/01/2006", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            string dateOfAdmission = DateTime.Now.Subtract(TimeSpan.FromDays(30)).ToString(pattern);
            string foreName = String.Format("{0}_{1}", SeleniumHelper.GenerateRandomString(7), SeleniumHelper.GenerateRandomNumberUsingDateTime());
            string surName = String.Format("{0}_{1}", "Avn", SeleniumHelper.GenerateRandomString(8));
            string effectiveDate = DateTime.Now.ToString(pattern);
            var res = new List<Object[]>
            {                
                new object[] 
                {
                    new string[]{String.Format("{0}{1}", foreName, 1), String.Format("{0}{1}", surName, 1), "Male", dateOfBirth},
                    new string[]{String.Format("{0}{1}", foreName, 2), String.Format("{0}{1}", surName, 2), "Male", dateOfBirth},
                    new string[]{String.Format("{0}{1}", foreName, 3), String.Format("{0}{1}", surName, 3), "Male", dateOfBirth},
                    new string[]{String.Format("{0}{1}", foreName, 4), String.Format("{0}{1}", surName, 4), "Male", dateOfBirth},
                    new string[]{String.Format("{0}{1}", foreName, 5), String.Format("{0}{1}", surName, 5), "Male", dateOfBirth},
                    new string[]{String.Format("{0}{1}", foreName, 6), String.Format("{0}{1}", surName, 6), "Male", dateOfBirth},
                    new string[]{String.Format("{0}{1}", foreName, 7), String.Format("{0}{1}", surName, 7), "Male", dateOfBirth},
                    new string[]{String.Format("{0}{1}", foreName, 8), String.Format("{0}{1}", surName, 8), "Male", dateOfBirth},
                    new string[]{String.Format("{0}{1}", foreName, 9), String.Format("{0}{1}", surName, 9), "Male", dateOfBirth},
                    new string[]{String.Format("{0}_{1}", foreName, 10), String.Format("{0}_{1}", surName, 10), "Male", dateOfBirth},
                    dateOfAdmission, "Year 2", effectiveDate, "2A"
                }
                
            };
            return res;
        }

        public List<object[]> TC_AP03_Data()
        {
            string pattern = "M/d/yyyy";
            string dateOfBirth = DateTime.ParseExact("06/01/2006", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            string dateOfAdmission = DateTime.Now.Subtract(TimeSpan.FromDays(30)).ToString(pattern);
            string foreName = String.Format("{0}_{1}", SeleniumHelper.GenerateRandomString(7), SeleniumHelper.GenerateRandomNumberUsingDateTime());
            string surName = String.Format("{0}_{1}", "Avn", SeleniumHelper.GenerateRandomString(8));
            string effectiveDate = DateTime.Now.ToString(pattern);
            var res = new List<Object[]>
            {                
                new object[] 
                {
                    new string[]{String.Format("{0}{1}", foreName, 1), String.Format("{0}{1}", surName, 1), "Male", dateOfBirth},
                    new string[]{String.Format("{0}{1}", foreName, 2), String.Format("{0}{1}", surName, 2), "Male", dateOfBirth},
                    new string[]{String.Format("{0}{1}", foreName, 3), String.Format("{0}{1}", surName, 3), "Male", dateOfBirth},
                    new string[]{String.Format("{0}{1}", foreName, 4), String.Format("{0}{1}", surName, 4), "Male", dateOfBirth},
                    new string[]{String.Format("{0}{1}", foreName, 5), String.Format("{0}{1}", surName, 5), "Male", dateOfBirth},
                    new string[]{String.Format("{0}{1}", foreName, 6), String.Format("{0}{1}", surName, 6), "Male", dateOfBirth},
                    new string[]{String.Format("{0}{1}", foreName, 7), String.Format("{0}{1}", surName, 7), "Male", dateOfBirth},
                    new string[]{String.Format("{0}{1}", foreName, 8), String.Format("{0}{1}", surName, 8), "Male", dateOfBirth},
                    new string[]{String.Format("{0}{1}", foreName, 9), String.Format("{0}{1}", surName, 9), "Male", dateOfBirth},
                    new string[]{String.Format("{0}_{1}", foreName, 10), String.Format("{0}_{1}", surName, 10), "Male", dateOfBirth},
                    dateOfAdmission, "Year 2", effectiveDate, "2A", "Year 3", "3A",
                }
                
            };
            return res;
        }

        public List<object[]> TC_AP05_Data()
        {
            string pattern = "M/d/yyyy";
            string nextAcademicDate = DateTime.ParseExact("08/01/2016", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            var res = new List<Object[]>
            {                
                new object[] 
                {
                    "Curriculum Year 8", nextAcademicDate
                }
            };
            return res;
        }

        public List<object[]> TC_AP06_Data()
        {
            string pattern = "M/d/yyyy";
            string nextAcademicDate = SeleniumHelper.GetStartDateAcademicYear(DateTime.Now.AddYears(1)).ToString(pattern);
            string yearGroupName = String.Format("{0}_{1}","Year Eight", SeleniumHelper.GenerateRandomString(10));
            string yearGroupShortName = String.Format("{0}_{1}","YR8", SeleniumHelper.GenerateRandomString(3));
            string displayOrder = "31";
            string academicYear = SeleniumHelper.GetAcademicYear(DateTime.Now.AddYears(1));
            var res = new List<Object[]>
            {                
                new object[] 
                {
                    "Curriculum Year 8", nextAcademicDate, yearGroupName, yearGroupShortName, displayOrder, academicYear,
                }
            };
            return res;
        }

        public List<object[]> TC_AP07_Data()
        {
            string pattern = "M/d/yyyy";
            string nextAcademicDate = SeleniumHelper.GetStartDateAcademicYear(DateTime.Now.AddYears(1)).ToString(pattern);
            string nextAcademic = SeleniumHelper.GetAcademicYear(DateTime.Now.AddYears(1));
            string randomString = SeleniumHelper.GenerateRandomString(6);
            var res = new List<Object[]>
            {                
                new object[] 
                {
                    new string[]{String.Format("{0}_{1}", "Year Eight", randomString),String.Format("{0}_{1}", "YR8", randomString), "31", "Curriculum Year 7"},
                    nextAcademicDate,
                    new string[]{String.Format("{0}_{1}", "8A", randomString), String.Format("{0}_{1}", "8A", randomString), "30"},
                    new string[]{String.Format("{0}_{1}", "8B", randomString), String.Format("{0}_{1}", "8B", randomString), "31"},
                    new string[]{String.Format("{0}_{1}", "8C", randomString), String.Format("{0}_{1}", "8C", randomString), "32"},
                    new string[]{String.Format("{0}_{1}", "8D", randomString), String.Format("{0}_{1}", "8D", randomString), "33"},
                    nextAcademic,
                }
            };

            return res;
        }

        public List<object[]> TC_AP08_Data()
        {
            string pattern = "M/d/yyyy";
            string dateOfBirth = DateTime.ParseExact("06/01/2006", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            string dateOfAdmission = DateTime.ParseExact("08/01/2016", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            string foreName = String.Format("{0}_{1}", SeleniumHelper.GenerateRandomString(7), SeleniumHelper.GenerateRandomNumberUsingDateTime());
            string surName = String.Format("{0}_{1}", "Avn", SeleniumHelper.GenerateRandomString(8));
            var res = new List<Object[]>
            {                
                new object[] 
                {
                    new string[]{String.Format("{0}{1}", foreName, 1), String.Format("{0}{1}", surName, 1), "Male", dateOfBirth},
                    new string[]{String.Format("{0}{1}", foreName, 2), String.Format("{0}{1}", surName, 2), "Male", dateOfBirth},
                    new string[]{String.Format("{0}{1}", foreName, 3), String.Format("{0}{1}", surName, 3), "Male", dateOfBirth},
                    new string[]{String.Format("{0}{1}", foreName, 4), String.Format("{0}{1}", surName, 4), "Male", dateOfBirth},
                    new string[]{String.Format("{0}{1}", foreName, 5), String.Format("{0}{1}", surName, 5), "Male", dateOfBirth},
                    new string[]{String.Format("{0}{1}", foreName, 6), String.Format("{0}{1}", surName, 6), "Male", dateOfBirth},
                    new string[]{String.Format("{0}{1}", foreName, 7), String.Format("{0}{1}", surName, 7), "Male", dateOfBirth},
                    new string[]{String.Format("{0}{1}", foreName, 8), String.Format("{0}{1}", surName, 8), "Male", dateOfBirth},
                    dateOfAdmission, "Year 2",
                }
                
            };
            return res;
        }

        public List<object[]> TC_AP09_Data()
        {
            string pattern = "M/d/yyyy";
            string dateOfBirth = DateTime.ParseExact("06/01/2006", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            string dateOfAdmission = DateTime.ParseExact("08/01/2016", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            string foreName = String.Format("{0}_{1}", SeleniumHelper.GenerateRandomString(7), SeleniumHelper.GenerateRandomNumberUsingDateTime());
            string surName = String.Format("{0}_{1}", "Avn", SeleniumHelper.GenerateRandomString(8));
            var res = new List<Object[]>
            {                
                new object[] 
                {
                    new string[]{String.Format("{0}{1}", foreName, 1), String.Format("{0}{1}", surName, 1), "Male", dateOfBirth},
                    new string[]{String.Format("{0}{1}", foreName, 2), String.Format("{0}{1}", surName, 2), "Male", dateOfBirth},
                    new string[]{String.Format("{0}{1}", foreName, 3), String.Format("{0}{1}", surName, 3), "Male", dateOfBirth},
                    new string[]{String.Format("{0}{1}", foreName, 4), String.Format("{0}{1}", surName, 4), "Male", dateOfBirth},
                    new string[]{String.Format("{0}{1}", foreName, 5), String.Format("{0}{1}", surName, 5), "Male", dateOfBirth},
                    new string[]{String.Format("{0}{1}", foreName, 6), String.Format("{0}{1}", surName, 6), "Male", dateOfBirth},
                    new string[]{String.Format("{0}{1}", foreName, 7), String.Format("{0}{1}", surName, 7), "Male", dateOfBirth},
                    new string[]{String.Format("{0}{1}", foreName, 8), String.Format("{0}{1}", surName, 8), "Male", dateOfBirth},
                    dateOfAdmission, "Year 2", "2A", "SEN"
                }
                
            };
            return res;
        }

        public List<object[]> TC_AP10_Data()
        {
            string pattern = "M/d/yyyy";
            string dateOfBirth = DateTime.ParseExact("06/01/2006", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            string dateOfAdmission = DateTime.ParseExact("08/01/2016", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            string foreName = String.Format("{0}_{1}", SeleniumHelper.GenerateRandomString(7), SeleniumHelper.GenerateRandomNumberUsingDateTime());
            string surName = String.Format("{0}_{1}", "Avn", SeleniumHelper.GenerateRandomString(8));
            var res = new List<Object[]>
            {                
                new object[] 
                {
                    new string[]{String.Format("{0}{1}", foreName, 1), String.Format("{0}{1}", surName, 1), "Male", dateOfBirth},
                    new string[]{String.Format("{0}{1}", foreName, 2), String.Format("{0}{1}", surName, 2), "Male", dateOfBirth},
                    new string[]{String.Format("{0}{1}", foreName, 3), String.Format("{0}{1}", surName, 3), "Male", dateOfBirth},
                    new string[]{String.Format("{0}{1}", foreName, 4), String.Format("{0}{1}", surName, 4), "Male", dateOfBirth},
                    new string[]{String.Format("{0}{1}", foreName, 5), String.Format("{0}{1}", surName, 5), "Male", dateOfBirth},
                    new string[]{String.Format("{0}{1}", foreName, 6), String.Format("{0}{1}", surName, 6), "Male", dateOfBirth},
                    new string[]{String.Format("{0}{1}", foreName, 7), String.Format("{0}{1}", surName, 7), "Male", dateOfBirth},
                    new string[]{String.Format("{0}{1}", foreName, 8), String.Format("{0}{1}", surName, 8), "Male", dateOfBirth},
                    dateOfAdmission, "Year 2", "2A", "SEN", "3A", "7A"
                }
                
            };
            return res;
        }

        public List<object[]> TC_AP11_Data()
        {
            var res = new List<Object[]>
            {                
                new object[] 
                {
                    new string[]{"Harris, Dylan", "Jones, Becky", "Long, Gemma"},
                    "Year 7",
                    "7A"
                }
            };
            return res;
        }

        public List<object[]> TC_AP12_Data()
        {
            var res = new List<Object[]>
            {                
                new object[] 
                {
                    new string[]{"Harris, Dylan", "Jones, Becky", "Long, Gemma"},
                    "7A",
                    "6A"
                }
            };
            return res;
        }
        
        #endregion
    }
}
