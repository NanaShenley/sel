using NUnit.Framework;
using POM.Components.Common;
using POM.Components.Pupil;
using POM.Helper;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using Pupil.Components.Common;
using TestSettings;
using WebDriverRunner.internals;

namespace Pupil.Pupil.Tests
{
    public class SenRecordTests
    {


        /// <summary>
        /// TC PU63
        /// Au : An Nguyen
        /// Description: Exercise ability to record a 'SEN Stage' record whilst in the 'Statutory SEN' section of a selected pupil record.
        /// Role: School Administrator
        /// </summary>
        [WebDriverTest(TimeoutSeconds = 1500, Enabled = true, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { PupilTestGroups.SenRecord.Page, PupilTestGroups.Priority.Priority2 }, DataProvider = "TC_PU063_Data")]
        public void TC_PU063_Exercise_ability_to_record_a_SEN_Stage_record_in_Statutory_SEN_section_of_pupil_record(string foreName, string surName, string gender, string DOB, string dateOfAdmission, string yearGroup,
                    string stage, string startDate)
        {
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);

            #region Pre-Condition

            //Delete the pupil if it exist before
            SeleniumHelper.NavigateMenu("Tasks", "Pupils", "Delete Pupil");
            var deletePupilRecords = new DeletePupilRecordTriplet();
            deletePupilRecords.SearchCriteria.PupilName = String.Format("{0}, {1}", surName, foreName);
            deletePupilRecords.SearchCriteria.IsCurrent = true;
            deletePupilRecords.SearchCriteria.IsLeaver = true;
            deletePupilRecords.SearchCriteria.IsFuture = true;
            var deleteResultPupils = deletePupilRecords.SearchCriteria.Search();
            var deletePupilSearchTile = deleteResultPupils.FirstOrDefault(t => t.Name.Equals(String.Format("{0}, {1}", surName, foreName)));
            var deletePupilRecord = deletePupilSearchTile == null ? new DeletePupilRecordPage() : deletePupilSearchTile.Click<DeletePupilRecordPage>();
            deletePupilRecord.Delete();
            SeleniumHelper.CloseTab("Delete Pupil");

            //Navigate to Pupil Record
            SeleniumHelper.NavigateQuickLink("Pupil Records");
            var pupilRecords = new PupilRecordTriplet();

            //Add new pupil
            var addNewPupilDialog = pupilRecords.AddNewPupil();
            addNewPupilDialog.Forename = foreName;
            addNewPupilDialog.SurName = surName;
            addNewPupilDialog.Gender = gender;
            addNewPupilDialog.DateOfBirth = DOB;
            var registrationDetailDialog = addNewPupilDialog.Continue();
            registrationDetailDialog.DateOfAdmission = dateOfAdmission;
            registrationDetailDialog.YearGroup = yearGroup;
            registrationDetailDialog.CreateRecord();
            var confirmRequiredDialog = new POM.Components.Common.ConfirmRequiredDialog();
            confirmRequiredDialog.ClickOk();

            //Save pupil
            var pupilRecord = PupilRecordPage.Create();
            pupilRecord.SavePupil();
            SeleniumHelper.CloseTab("Pupil Record");

            #endregion

            #region Test steps

            //Invoke the 'Pupil Record' screen via the Quick Link
            SeleniumHelper.NavigateQuickLink("Pupil Records");
            pupilRecords = new PupilRecordTriplet();

            //Perform an advance search (Show more selected) and perform a search for pupils by 'Enrolment Status' of "Single Registration"
            pupilRecords.SearchCriteria.PupilName = String.Format("{0}, {1}", surName, foreName);
            pupilRecords.SearchCriteria.ClickSearchAdvanced(true);
            pupilRecords.SearchCriteria.YearGroup = yearGroup;
            pupilRecords.SearchCriteria.EnrolmentStatus = "Single Registration";
            var pupilSearchResults = pupilRecords.SearchCriteria.Search();

            //Select the 'Pupil'
            var pupilTile = pupilSearchResults.FirstOrDefault(t => t.Name.Equals(String.Format("{0}, {1}", surName, foreName)));
            pupilRecord = pupilTile.Click<PupilRecordPage>();

            //Navigate to and expand the 'Statutory SEN' section.
            pupilRecord.SelectStatutorySenTab();

            //Add data to SEN Stage Table
            pupilRecord.SenStages[0].Stage = stage;
            pupilRecord.SenStages[0].StartDay = startDate;

            //Verify success message has displayed 
            pupilRecord.SavePupil();
            Assert.AreEqual(true, pupilRecord.IsSuccessMessageDisplayed(), "Success message is not displayed");

            //Select the 'SEN Record' contextual link 
            var SenRecord = SeleniumHelper.NavigateViaAction<SenRecordDetailPage>("SEN Record");

            //Confirm the 'SEN Stage' record just added displays.
            var senStageRow = SenRecord.SenStages.Rows.FirstOrDefault(t => t.Stage.Equals(stage) && t.StartDay.Equals(startDate));
            Assert.AreNotEqual(null, senStageRow, "SEN stage is not added to SEN Record");

            //Exit the 'SEN Record' screen, thus returning focus back to the 'Pupil Record' screen.
            SeleniumHelper.CloseTab("SEN Record");
            SeleniumHelper.NavigateQuickLink("Pupil Records");
            pupilRecords = new PupilRecordTriplet();
            pupilRecords.SearchCriteria.PupilName = String.Format("{0}, {1}", surName, foreName);
            pupilSearchResults = pupilRecords.SearchCriteria.Search();
            pupilTile = pupilSearchResults.FirstOrDefault(t => t.Name.Equals(String.Format("{0}, {1}", surName, foreName)));
            pupilRecord = pupilTile.Click<PupilRecordPage>();

            //Verify 'SEN Stage' data
            pupilRecord.SelectStatutorySenTab();
            var senStagePupilRow = pupilRecord.SenStages.Rows.FirstOrDefault(t => t.Stage.Equals(stage) && t.StartDay.Equals(startDate));
            Assert.AreNotEqual(null, senStagePupilRow, "SEN stage is not added to SEN Record");

            #endregion

            #region Post-Condition

            //Delete the pupil
            SeleniumHelper.NavigateMenu("Tasks", "Pupils", "Delete Pupil");
            deletePupilRecords = new DeletePupilRecordTriplet();
            deletePupilRecords.SearchCriteria.PupilName = String.Format("{0}, {1}", surName, foreName);
            deletePupilRecords.SearchCriteria.IsCurrent = true;
            deletePupilRecords.SearchCriteria.IsLeaver = true;
            deletePupilRecords.SearchCriteria.IsFuture = true;
            deleteResultPupils = deletePupilRecords.SearchCriteria.Search();
            deletePupilSearchTile = deleteResultPupils.FirstOrDefault(t => t.Name.Equals(String.Format("{0}, {1}", surName, foreName)));
            deletePupilRecord = deletePupilSearchTile.Click<DeletePupilRecordPage>();
            deletePupilRecord.Delete();

            #endregion
        }

        /// <summary>
        /// TC PU64
        /// Au : An Nguyen
        /// Description: Exercise ability to record a 'SEN Need' record whilst in the 'Statutory SEN' section of a selected pupil record.
        /// Role: School Administrator
        /// </summary>
        [WebDriverTest(TimeoutSeconds = 1500, Enabled = true, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { PupilTestGroups.SenRecord.Page, PupilTestGroups.Priority.Priority2 }, DataProvider = "TC_PU064_Data")]
        public void TC_PU064_Exercise_ability_to_record_a_SEN_Need_record_in_Statutory_SEN_section_of_pupil_record(string foreName, string surName, string gender, string DOB, string dateOfAdmission, string yearGroup,
                    string needType, string startDate, string description)
        {
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);

            #region Pre-Condition

            //Delete the pupil if it exist before
            SeleniumHelper.NavigateMenu("Tasks", "Pupils", "Delete Pupil");
            var deletePupilRecords = new DeletePupilRecordTriplet();
            deletePupilRecords.SearchCriteria.PupilName = String.Format("{0}, {1}", surName, foreName);
            deletePupilRecords.SearchCriteria.IsCurrent = true;
            deletePupilRecords.SearchCriteria.IsLeaver = true;
            deletePupilRecords.SearchCriteria.IsFuture = true;
            var deleteResultPupils = deletePupilRecords.SearchCriteria.Search();
            var deletePupilSearchTile = deleteResultPupils.FirstOrDefault(t => t.Name.Equals(String.Format("{0}, {1}", surName, foreName)));
            var deletePupilRecord = deletePupilSearchTile == null ? new DeletePupilRecordPage() : deletePupilSearchTile.Click<DeletePupilRecordPage>();
            deletePupilRecord.Delete();
            SeleniumHelper.CloseTab("Delete Pupil");

            //Navigate to Pupil Record
            SeleniumHelper.NavigateQuickLink("Pupil Records");
            var pupilRecords = new PupilRecordTriplet();

            //Add new pupil
            var addNewPupilDialog = pupilRecords.AddNewPupil();
            addNewPupilDialog.Forename = foreName;
            addNewPupilDialog.SurName = surName;
            addNewPupilDialog.Gender = gender;
            addNewPupilDialog.DateOfBirth = DOB;
            var registrationDetailDialog = addNewPupilDialog.Continue();
            registrationDetailDialog.DateOfAdmission = dateOfAdmission;
            registrationDetailDialog.YearGroup = yearGroup;
            registrationDetailDialog.CreateRecord();
            var confirmRequiredDialog = new POM.Components.Common.ConfirmRequiredDialog();
            confirmRequiredDialog.ClickOk();

            //Save pupil
            var pupilRecord = PupilRecordPage.Create();
            pupilRecord.SavePupil();
            SeleniumHelper.CloseTab("Pupil Record");

            #endregion

            #region Test steps

            //Invoke the 'Pupil Record' screen via the Quick Link
            SeleniumHelper.NavigateQuickLink("Pupil Records");
            pupilRecords = new PupilRecordTriplet();

            //Perform an advance search (Show more selected) and perform a search for pupils by 'Enrolment Status' of "Single Registration" 
            pupilRecords.SearchCriteria.PupilName = String.Format("{0}, {1}", surName, foreName);
            pupilRecords.SearchCriteria.ClickSearchAdvanced(true);
            pupilRecords.SearchCriteria.YearGroup = yearGroup;
            pupilRecords.SearchCriteria.EnrolmentStatus = "Single Registration";
            var pupilSearchResults = pupilRecords.SearchCriteria.Search();

            //Select the 'Pupil'
            var pupilTile = pupilSearchResults.FirstOrDefault(t => t.Name.Equals(String.Format("{0}, {1}", surName, foreName)));
            pupilRecord = pupilTile.Click<PupilRecordPage>();

            //Navigate to and expand the 'Statutory SEN' section.
            pupilRecord.SelectStatutorySenTab();

            //Add data to SEN Need Table
            pupilRecord.SenNeeds[0].NeedType = needType;
            pupilRecord.SenNeeds[0].StartDay = startDate;
            pupilRecord.SenNeeds[0].Description = description;

            //Verify success message has displayed 
            pupilRecord.SavePupil();
            Assert.AreEqual(true, pupilRecord.IsSuccessMessageDisplayed(), "Success message is not displayed");

            //Select the 'SEN Record' contextual link 
            var SenRecord = SeleniumHelper.NavigateViaAction<SenRecordDetailPage>("SEN Record");

            //Confirm the 'SEN Need' record just added displays.
            var senNeedRow = SenRecord.SenNeeds.Rows.FirstOrDefault(t => t.NeedType.Equals(needType) && t.StartDay.Equals(startDate));
            Assert.AreEqual(description, senNeedRow.Description, "SEN Need is not added to SEN Record");

            //Exit the 'SEN Record' screen, thus returning focus back to the 'Pupil Record' screen.
            SeleniumHelper.CloseTab("SEN Record");
            SeleniumHelper.NavigateQuickLink("Pupil Records");
            pupilRecords = new PupilRecordTriplet();
            pupilRecords.SearchCriteria.PupilName = String.Format("{0}, {1}", surName, foreName);
            pupilSearchResults = pupilRecords.SearchCriteria.Search();
            pupilTile = pupilSearchResults.FirstOrDefault(t => t.Name.Equals(String.Format("{0}, {1}", surName, foreName)));
            pupilRecord = pupilTile.Click<PupilRecordPage>();

            //Verify 'SEN Need' data
            pupilRecord.SelectStatutorySenTab();
            var senNeedPupilRow = pupilRecord.SenNeeds.Rows.FirstOrDefault(t => t.NeedType.Equals(needType) && t.StartDay.Equals(startDate));
            Assert.AreEqual(description, senNeedPupilRow.Description, "SEN Need is not added to SEN Record");

            #endregion

            #region Post-Condition

            //Delete the pupil
            SeleniumHelper.NavigateMenu("Tasks", "Pupils", "Delete Pupil");
            deletePupilRecords = new DeletePupilRecordTriplet();
            deletePupilRecords.SearchCriteria.PupilName = String.Format("{0}, {1}", surName, foreName);
            deletePupilRecords.SearchCriteria.IsCurrent = true;
            deletePupilRecords.SearchCriteria.IsLeaver = true;
            deletePupilRecords.SearchCriteria.IsFuture = true;
            deleteResultPupils = deletePupilRecords.SearchCriteria.Search();
            deletePupilSearchTile = deleteResultPupils.FirstOrDefault(t => t.Name.Equals(String.Format("{0}, {1}", surName, foreName)));
            deletePupilRecord = deletePupilSearchTile.Click<DeletePupilRecordPage>();
            deletePupilRecord.Delete();

            #endregion
        }

        /// <summary>
        /// TC PU65
        /// Au : An Nguyen
        /// Description: Exercise ability to record a 'SEN Provision' record whilst in the 'Statutory SEN' section of a selected pupil record.
        /// Role: School Administrator
        /// </summary>
        [WebDriverTest(TimeoutSeconds = 1500, Enabled = true, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { PupilTestGroups.SenRecord.Page, PupilTestGroups.Priority.Priority2 }, DataProvider = "TC_PU065_Data")]
        public void TC_PU065_Exercise_ability_to_record_a_SEN_Provision_record_in_Statutory_SEN_section_of_pupil_record(string foreName, string surName, string gender, string DOB, string dateOfAdmission, string yearGroup,
                    string provisionType, string startDate, string endDate, string comment)
        {
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);

            #region Pre-Condition

            //Delete the pupil if it exist before
            SeleniumHelper.NavigateMenu("Tasks", "Pupils", "Delete Pupil");
            var deletePupilRecords = new DeletePupilRecordTriplet();
            deletePupilRecords.SearchCriteria.PupilName = String.Format("{0}, {1}", surName, foreName);
            deletePupilRecords.SearchCriteria.IsCurrent = true;
            deletePupilRecords.SearchCriteria.IsLeaver = true;
            deletePupilRecords.SearchCriteria.IsFuture = true;
            var deleteResultPupils = deletePupilRecords.SearchCriteria.Search();
            var deletePupilSearchTile = deleteResultPupils.FirstOrDefault(t => t.Name.Equals(String.Format("{0}, {1}", surName, foreName)));
            var deletePupilRecord = deletePupilSearchTile == null ? new DeletePupilRecordPage() : deletePupilSearchTile.Click<DeletePupilRecordPage>();
            deletePupilRecord.Delete();
            SeleniumHelper.CloseTab("Delete Pupil");

            //Navigate to Pupil Record
            SeleniumHelper.NavigateQuickLink("Pupil Records");
            var pupilRecords = new PupilRecordTriplet();

            //Add new pupil
            var addNewPupilDialog = pupilRecords.AddNewPupil();
            addNewPupilDialog.Forename = foreName;
            addNewPupilDialog.SurName = surName;
            addNewPupilDialog.Gender = gender;
            addNewPupilDialog.DateOfBirth = DOB;
            var registrationDetailDialog = addNewPupilDialog.Continue();
            registrationDetailDialog.DateOfAdmission = dateOfAdmission;
            registrationDetailDialog.YearGroup = yearGroup;
            registrationDetailDialog.CreateRecord();
            var confirmRequiredDialog = new POM.Components.Common.ConfirmRequiredDialog();
            confirmRequiredDialog.ClickOk();

            //Save pupil
            var pupilRecord = PupilRecordPage.Create();
            pupilRecord.SavePupil();
            SeleniumHelper.CloseTab("Pupil Record");

            #endregion

            #region Test steps

            //Invoke the 'Pupil Record' screen via the Quick Link
            SeleniumHelper.NavigateQuickLink("Pupil Records");
            pupilRecords = new PupilRecordTriplet();

            //Perform an advance search (Show more selected) and perform a search for pupils by 'Enrolment Status' of "Single Registration"
            pupilRecords.SearchCriteria.PupilName = String.Format("{0}, {1}", surName, foreName);
            pupilRecords.SearchCriteria.ClickSearchAdvanced(true);
            pupilRecords.SearchCriteria.YearGroup = yearGroup;
            pupilRecords.SearchCriteria.EnrolmentStatus = "Single Registration";
            var pupilSearchResults = pupilRecords.SearchCriteria.Search();

            //Select the 'Pupil'
            var pupilTile = pupilSearchResults.FirstOrDefault(t => t.Name.Equals(String.Format("{0}, {1}", surName, foreName)));
            pupilRecord = pupilTile.Click<PupilRecordPage>();

            //Navigate to and expand the 'Statutory SEN' section.
            pupilRecord.SelectStatutorySenTab();

            //Enter data to SEN Provision Table
            pupilRecord.SenProvisions[0].ProvisionType = provisionType;
            pupilRecord.SenProvisions[0].StartDay = startDate;
            pupilRecord.SenProvisions[0].EndDay = endDate;
            pupilRecord.SenProvisions[0].Comment = comment;

            //Verify success message has displayed 
            pupilRecord.SavePupil();
            Assert.AreEqual(true, pupilRecord.IsSuccessMessageDisplayed(), "Success message is not displayed");

            //Select the 'SEN Record' contextual link 
            var SenRecord = SeleniumHelper.NavigateViaAction<SenRecordDetailPage>("SEN Record");

            //Confirm the 'SEN Provisions' record just added displays.
            var senProvisionRow = SenRecord.SenProvisions.Rows.FirstOrDefault(t => t.ProvisionType.Equals(provisionType) && t.StartDay.Equals(startDate));
            Assert.AreEqual(comment, senProvisionRow.Comment, "SEN Provision is not added to SEN Record");

            //Exit the 'SEN Record' screen, thus returning focus back to the 'Pupil Record' screen.
            SeleniumHelper.CloseTab("SEN Record");
            SeleniumHelper.NavigateQuickLink("Pupil Records");
            pupilRecords = new PupilRecordTriplet();
            pupilRecords.SearchCriteria.ClickSearchAdvanced(true);
            pupilRecords.SearchCriteria.PupilName = String.Format("{0}, {1}", surName, foreName);
            pupilSearchResults = pupilRecords.SearchCriteria.Search();
            pupilTile = pupilSearchResults.FirstOrDefault(t => t.Name.Equals(String.Format("{0}, {1}", surName, foreName)));
            pupilRecord = pupilTile.Click<PupilRecordPage>();

            //Verify 'SEN Provisions' data
            pupilRecord.SelectStatutorySenTab();
            var senProvisionPupilRow = pupilRecord.SenProvisions.Rows.FirstOrDefault(t => t.ProvisionType.Equals(provisionType) && t.StartDay.Equals(startDate));
            Assert.AreEqual(comment, senProvisionPupilRow.Comment, "SEN Provision is not added to SEN Record");

            #endregion

            #region Post-Condition

            //Delete the pupil
            SeleniumHelper.NavigateMenu("Tasks", "Pupils", "Delete Pupil");
            deletePupilRecords = new DeletePupilRecordTriplet();
            deletePupilRecords.SearchCriteria.PupilName = String.Format("{0}, {1}", surName, foreName);
            deletePupilRecords.SearchCriteria.IsCurrent = true;
            deletePupilRecords.SearchCriteria.IsLeaver = true;
            deletePupilRecords.SearchCriteria.IsFuture = true;
            deleteResultPupils = deletePupilRecords.SearchCriteria.Search();
            deletePupilSearchTile = deleteResultPupils.FirstOrDefault(t => t.Name.Equals(String.Format("{0}, {1}", surName, foreName)));
            deletePupilRecord = deletePupilSearchTile.Click<DeletePupilRecordPage>();
            deletePupilRecord.Delete();

            #endregion
        }

        /// <summary>
        /// TC PU66a
        /// Au : An Nguyen
        /// Description: Exercise ability to add a single 'SEN Review' to a pupil that has SEN Details.
        /// Role: School Administrator
        /// </summary>
        [WebDriverTest(TimeoutSeconds = 1500, Enabled = true, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { PupilTestGroups.SenRecord.Page, PupilTestGroups.Priority.Priority2 }, DataProvider = "TC_PU066a_Data")]
        public void TC_PU066a_Exercise_ability_to_add_a_single_SEN_Review_record(string foreName, string surName, string gender, string DOB, string dateOfAdmission, string yearGroup,
                    string senStage,
                    string reviewType, string reviewStatus, string reviewStartDate, string reviewStartTime, string reviewVenue,
                    int staff, string staffForeName, string staffSurName, string relationship, string dateConsulted)
        {
            #region Pre-Condtion

            //Add staff role to staff
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.PersonnelOfficer);
            SeleniumHelper.NavigateQuickLink("Staff Records");
            var staffRecords = new POM.Components.Staff.StaffRecordTriplet();
            staffRecords.SearchCriteria.IsCurrent = true;
            staffRecords.SearchCriteria.IsFuture = false;
            staffRecords.SearchCriteria.IsLeaver = false;
            var staffSearchTile = staffRecords.SearchCriteria.Search()[staff];
            var staffRecord = staffSearchTile.Click<POM.Components.Staff.StaffRecordPage>();
            staffForeName = staffRecord.PreferForeName;
            staffSurName = staffRecord.PreferSurname;
            staffRecord.SelectServiceDetailsTab();
            var staffRoleRow = staffRecord.StaffRoleStandardTable.Rows.Last();
            staffRoleRow.StaffRole = relationship;
            staffRoleRow = staffRecord.StaffRoleStandardTable.LastInsertRow;
            staffRoleRow.StaffStartDate = "01/01/2015";
            staffRecord.SaveStaff();
            SeleniumHelper.Logout();

            //Delete the pupil if it exist before
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);
            SeleniumHelper.NavigateMenu("Tasks", "Pupils", "Delete Pupil");
            var deletePupilRecords = new DeletePupilRecordTriplet();
            deletePupilRecords.SearchCriteria.PupilName = String.Format("{0}, {1}", surName, foreName);
            deletePupilRecords.SearchCriteria.IsCurrent = true;
            deletePupilRecords.SearchCriteria.IsLeaver = true;
            deletePupilRecords.SearchCriteria.IsFuture = true;
            var deleteResultPupils = deletePupilRecords.SearchCriteria.Search();
            var deletePupilSearchTile = deleteResultPupils.FirstOrDefault(t => t.Name.Equals(String.Format("{0}, {1}", surName, foreName)));
            var deletePupilRecord = deletePupilSearchTile == null ? new DeletePupilRecordPage() : deletePupilSearchTile.Click<DeletePupilRecordPage>();
            deletePupilRecord.Delete();
            SeleniumHelper.CloseTab("Delete Pupil");

            //Navigate to Pupil Record
            SeleniumHelper.NavigateQuickLink("Pupil Records");
            var pupilRecords = new PupilRecordTriplet();

            //Add new pupil
            var addNewPupilDialog = pupilRecords.AddNewPupil();
            addNewPupilDialog.Forename = foreName;
            addNewPupilDialog.SurName = surName;
            addNewPupilDialog.Gender = gender;
            addNewPupilDialog.DateOfBirth = DOB;
            var registrationDetailDialog = addNewPupilDialog.Continue();
            registrationDetailDialog.DateOfAdmission = dateOfAdmission;
            registrationDetailDialog.YearGroup = yearGroup;
            registrationDetailDialog.CreateRecord();
            var confirmRequiredDialog = new POM.Components.Common.ConfirmRequiredDialog();
            confirmRequiredDialog.ClickOk();

            //Navigate to and expand the 'Statutory SEN' section.
            var pupilRecord = PupilRecordPage.Create();
            pupilRecord.SelectStatutorySenTab();

            //Add data to SEN Stage Table
            pupilRecord.SenStages[0].Stage = "In-school provision";
            pupilRecord.SenStages[0].StartDay = dateOfAdmission;

            //Save pupil
            pupilRecord.SavePupil();
            SeleniumHelper.CloseTab("Pupil Record");

            #endregion

            #region Test steps

            //Search by SEN Stage and name of the pupil
            SeleniumHelper.NavigateBySearch("SEN Records", true);
            var SenRecords = new SenRecordTriplet();
            SenRecords.SearchCriteria.Name = String.Format("{0}, {1}", surName, foreName);
            SenRecords.SearchCriteria.SenStage = senStage;
            var senSearchResults = SenRecords.SearchCriteria.Search();
            var senTile = senSearchResults.FirstOrDefault(t => t.Name.Equals(String.Format("{0}, {1}", surName, foreName)));
            var SenRecord = senTile.Click<SenRecordDetailPage>();

            //Add "SEN Review"
            var addSenReviewDialog = SenRecord.ClickAddSenReview();
            addSenReviewDialog.ReviewType = reviewType;
            addSenReviewDialog.ReviewStatus = reviewStatus;
            addSenReviewDialog.StartDate = reviewStartDate;
            addSenReviewDialog.StartTime = reviewStartTime;
            addSenReviewDialog.Venue = reviewVenue;

            //Add People Involved
            var addPeopleInvolved = addSenReviewDialog.ClickAddPeopleInvolved();

            //Select Staff
            var selectPeopleDialog = addPeopleInvolved.ClickSelectPeople();
            selectPeopleDialog.SearchCriteria.ForeName = staffForeName;
            selectPeopleDialog.SearchCriteria.SurName = staffSurName;
            var searchStaffResults = selectPeopleDialog.SearchCriteria.Search();
            var staffTile = searchStaffResults.FirstOrDefault(t => t.Name.Equals(String.Format("{0}, {1}", staffSurName, staffForeName)));
            staffTile.Click();
            addPeopleInvolved = selectPeopleDialog.ClickOk();

            //Add Relationship, dateConsulted, invited, accepted, attended
            addPeopleInvolved.Relationship = relationship;
            addPeopleInvolved.DateConsulted = dateConsulted;
            addPeopleInvolved.IsInvited = true;
            addPeopleInvolved.IsAccepted = true;
            addPeopleInvolved.IsAttended = true;

            //Save "SEN Review"
            addPeopleInvolved.ClickOk(5);
            addSenReviewDialog.ClickOk(5);
            SenRecord.Save();

            //Verify new "SEN Review" record
            var senReviewRow = SenRecord.SenReviews.Rows.FirstOrDefault(t => t.StartDate.Equals(reviewStartDate));
            Assert.AreNotEqual(null, senReviewRow, "Add SEN Review unsuccessfull");
            Assert.AreEqual(reviewType, senReviewRow.ReviewType, "Review Type is incorrect");
            Assert.AreEqual(reviewStatus, senReviewRow.ReviewStatus, "Review Status is incorrect");

            #endregion

            #region Post-Condtion

            //Delete SEN Review
            senReviewRow.DeleteRow();
            SenRecord.Save();

            //Delete the pupil
            SeleniumHelper.NavigateMenu("Tasks", "Pupils", "Delete Pupil");
            deletePupilRecords = new DeletePupilRecordTriplet();
            deletePupilRecords.SearchCriteria.PupilName = String.Format("{0}, {1}", surName, foreName);
            deletePupilRecords.SearchCriteria.IsCurrent = true;
            deletePupilRecords.SearchCriteria.IsLeaver = true;
            deletePupilRecords.SearchCriteria.IsFuture = true;
            deleteResultPupils = deletePupilRecords.SearchCriteria.Search();
            deletePupilSearchTile = deleteResultPupils.FirstOrDefault(t => t.Name.Equals(String.Format("{0}, {1}", surName, foreName)));
            deletePupilRecord = deletePupilSearchTile.Click<DeletePupilRecordPage>();
            deletePupilRecord.Delete();

            //Delete Role from staff
            SeleniumHelper.Logout();
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.PersonnelOfficer);
            SeleniumHelper.NavigateQuickLink("Staff Records");
            staffRecords = new POM.Components.Staff.StaffRecordTriplet();
            staffRecords.SearchCriteria.StaffName = String.Format("{0}, {1}", staffSurName, staffForeName);
            staffRecords.SearchCriteria.IsCurrent = true;
            staffRecords.SearchCriteria.IsFuture = false;
            staffRecords.SearchCriteria.IsLeaver = false;
            staffSearchTile = staffRecords.SearchCriteria.Search()[0];
            staffRecord = staffSearchTile.Click<POM.Components.Staff.StaffRecordPage>();
            staffRecord.SelectServiceDetailsTab();
            staffRoleRow = staffRecord.StaffRoleStandardTable.Rows.FirstOrDefault(t => t.StaffRole.Equals(relationship));
            staffRoleRow.DeleteRow();
            staffRecord.SaveStaff();

            #endregion
        }

        /// <summary>
        /// TC PU66b
        /// Au : An Nguyen
        /// Description: Exercise ability to add a single 'SEN Statement' to a pupil that has SEN Details.
        /// Role: School Administrator
        /// </summary>
        [WebDriverTest(TimeoutSeconds = 1500, Enabled = true, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { PupilTestGroups.SenRecord.Page, PupilTestGroups.Priority.Priority2 }, DataProvider = "TC_PU066b_Data")]
        public void TC_PU066b_Exercise_ability_to_add_a_single_SEN_Statement_record(string foreName, string surName, string gender, string DOB, string dateOfAdmission, string yearGroup,
                    string senStage,
                    string dateRequest, string dateConsulted, string elbResponse, string statementOutcome, string dateFinalised, string dateCeased)
        {
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);

            #region Pre-Condition

            //Delete the pupil if it exist before
            SeleniumHelper.NavigateMenu("Tasks", "Pupils", "Delete Pupil");
            var deletePupilRecords = new DeletePupilRecordTriplet();
            deletePupilRecords.SearchCriteria.PupilName = String.Format("{0}, {1}", surName, foreName);
            deletePupilRecords.SearchCriteria.IsCurrent = true;
            deletePupilRecords.SearchCriteria.IsLeaver = true;
            deletePupilRecords.SearchCriteria.IsFuture = true;
            var deleteResultPupils = deletePupilRecords.SearchCriteria.Search();
            var deletePupilSearchTile = deleteResultPupils.FirstOrDefault(t => t.Name.Equals(String.Format("{0}, {1}", surName, foreName)));
            var deletePupilRecord = deletePupilSearchTile == null ? new DeletePupilRecordPage() : deletePupilSearchTile.Click<DeletePupilRecordPage>();
            deletePupilRecord.Delete();
            SeleniumHelper.CloseTab("Delete Pupil");

            //Navigate to Pupil Record
            SeleniumHelper.NavigateQuickLink("Pupil Records");
            var pupilRecords = new PupilRecordTriplet();

            //Add new pupil
            var addNewPupilDialog = pupilRecords.AddNewPupil();
            addNewPupilDialog.Forename = foreName;
            addNewPupilDialog.SurName = surName;
            addNewPupilDialog.Gender = gender;
            addNewPupilDialog.DateOfBirth = DOB;
            var registrationDetailDialog = addNewPupilDialog.Continue();
            registrationDetailDialog.DateOfAdmission = dateOfAdmission;
            registrationDetailDialog.YearGroup = yearGroup;
            registrationDetailDialog.CreateRecord();
            var confirmRequiredDialog = new POM.Components.Common.ConfirmRequiredDialog();
            confirmRequiredDialog.ClickOk();

            //Navigate to and expand the 'Statutory SEN' section.
            var pupilRecord = PupilRecordPage.Create();
            pupilRecord.SelectStatutorySenTab();

            //Add data to SEN Stage Table
            pupilRecord.SenStages[0].Stage = "In-school provision";
            pupilRecord.SenStages[0].StartDay = dateOfAdmission;

            //Save pupil
            pupilRecord.SavePupil();
            SeleniumHelper.CloseTab("Pupil Record");

            #endregion

            #region Test steps

            //Search by SEN Stage and name of the pupil
            SeleniumHelper.NavigateBySearch("SEN Records", true);
            var SenRecords = new SenRecordTriplet();
            SenRecords.SearchCriteria.Name = String.Format("{0}, {1}", surName, foreName);
            SenRecords.SearchCriteria.SenStage = senStage;
            var senSearchResults = SenRecords.SearchCriteria.Search();
            var senTile = senSearchResults.FirstOrDefault(t => t.Name.Equals(String.Format("{0}, {1}", surName, foreName)));
            var SenRecord = senTile.Click<SenRecordDetailPage>();

            //Add "SEN Statement"
            var addSenStatement = SenRecord.ClickAddSenStatement();

            //Add Date Requested and Date Parent Consulted
            addSenStatement.DateRequested = dateRequest;
            addSenStatement.DateConsulted = dateConsulted;

            //Add ELB Officer
            var elbOfficer = addSenStatement.ClickSelectOfficer();
            var officerTile = elbOfficer.SearchCriteria.Search()[0];
            officerTile.Click();
            elbOfficer.ClickOk(5);

            //Add ELB Reponse and Statement Outcome
            addSenStatement.ELBReponse = elbResponse;
            addSenStatement.StatementOutcome = statementOutcome;

            //Add Date Finalised and Date Ceased
            addSenStatement.DateFinalised = dateFinalised;
            addSenStatement.DateCeased = dateCeased;

            //Save "SEN Statement"
            addSenStatement.ClickOk(5);
            SenRecord.Save();

            //Verify new "SEN Statement" record
            var senStatementRow = SenRecord.SenStatements.Rows.FirstOrDefault(t => t.DateRequested.Equals(dateRequest) && t.ELBResponse.Equals(elbResponse));
            Assert.AreNotEqual(null, senStatementRow, "Add SEN Statement unsuccessfull");
            Assert.AreEqual(statementOutcome, senStatementRow.StatementOutcome, "Statement Outcome is incorrect");
            Assert.AreEqual(dateFinalised, senStatementRow.DateFinalised, "Date Finalised is incorrect");
            Assert.AreEqual(dateCeased, senStatementRow.DateCeased, "Date Ceased is incorrect");

            #endregion

            #region Post-Condition

            //Delete SEN Statement
            senStatementRow.DeleteRow();
            SenRecord.Save();

            //Delete the pupil
            SeleniumHelper.NavigateMenu("Tasks", "Pupils", "Delete Pupil");
            deletePupilRecords = new DeletePupilRecordTriplet();
            deletePupilRecords.SearchCriteria.PupilName = String.Format("{0}, {1}", surName, foreName);
            deletePupilRecords.SearchCriteria.IsCurrent = true;
            deletePupilRecords.SearchCriteria.IsLeaver = true;
            deletePupilRecords.SearchCriteria.IsFuture = true;
            deleteResultPupils = deletePupilRecords.SearchCriteria.Search();
            deletePupilSearchTile = deleteResultPupils.FirstOrDefault(t => t.Name.Equals(String.Format("{0}, {1}", surName, foreName)));
            deletePupilRecord = deletePupilSearchTile.Click<DeletePupilRecordPage>();
            deletePupilRecord.Delete();

            #endregion
        }


        /// <summary>
        /// TC PU69
        /// Au : An Nguyen
        /// Description: Exercise ability to add a school level SEN Need Type.
        /// Role: School Administrator
        /// </summary>
        [WebDriverTest(TimeoutSeconds = 600, Enabled = true, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { PupilTestGroups.SenRecord.Page, PupilTestGroups.Priority.Priority2 }, DataProvider = "TC_PU069_Data")]
        public void TC_PU069_Exercise_ability_to_add_a_school_level_SEN_Need_Type(string code, string description, string displayOrder, string category)
        {
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);
            SeleniumHelper.NavigateBySearch("SEN Need Type", true);

            #region Pre-Condition

            //Delete Old SEN Need Type if needed
            var senNeedTypeTriplet = new SenNeedTypeTriplet();
            senNeedTypeTriplet.SearchCriteria.CodeOrDecription = code;
            var senNeedTypePage = senNeedTypeTriplet.SearchCriteria.Search<SenNeedTypePage>();
            var senNeedTypeRow = senNeedTypePage.SenNeedTypeTable.Rows.FirstOrDefault(t => t.Code.Equals(code));
            senNeedTypePage.DeleteTableRow(senNeedTypeRow);

            #endregion

            #region Test steps

            //Add new SEN Need Type
            senNeedTypeTriplet = new SenNeedTypeTriplet();
            senNeedTypePage = senNeedTypeTriplet.AddSenNeedType();
            senNeedTypeRow = senNeedTypePage.SenNeedTypeTable[0];
            senNeedTypeRow.Code = code;
            senNeedTypeRow.Description = description;
            senNeedTypeRow.DisplayOrder = displayOrder;
            senNeedTypeRow.IsVisible = true;
            senNeedTypeRow.Category = category;

            //Save SEN Need Type
            senNeedTypePage.ClickSave();

            //Verify success message
            Assert.AreEqual(true, senNeedTypePage.IsSuccessMessageDisplayed(), "Success message do not display");

            //Search SEN Need Type by code
            senNeedTypeTriplet = new SenNeedTypeTriplet();
            senNeedTypeTriplet.SearchCriteria.CodeOrDecription = code;
            senNeedTypePage = senNeedTypeTriplet.SearchCriteria.Search<SenNeedTypePage>();

            //Verify data
            senNeedTypeRow = senNeedTypePage.SenNeedTypeTable.Rows.FirstOrDefault(t => t.Code.Equals(code));
            Assert.AreNotEqual(null, senNeedTypeRow, "Add SEN Need Type unsuccessfull");
            Assert.AreEqual(description, senNeedTypeRow.Description, "Description of SEN Need Type is incorrect");
            Assert.AreEqual(displayOrder, senNeedTypeRow.DisplayOrder, "Display Order of SEN Need Type is incorrect");
            Assert.AreEqual(category, senNeedTypeRow.Category, "Category of SEN Need Type is incorrect");

            #endregion

            #region Post-Condition

            //Delete SEN Need Type
            senNeedTypePage.DeleteTableRow(senNeedTypeRow);

            #endregion
        }

        /// <summary>
        /// TC PU70
        /// Au : An Nguyen
        /// Description: Exercise ability to add a school level SEN Provision Type.
        /// Role: School Administrator
        /// </summary>
        [WebDriverTest(TimeoutSeconds = 600, Enabled = true, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { PupilTestGroups.SenRecord.Page, PupilTestGroups.Priority.Priority2 }, DataProvider = "TC_PU070_Data")]
        public void TC_PU070_Exercise_ability_to_add_a_school_level_SEN_Provision_Type(string code, string description, string displayOrder)
        {
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);
            SeleniumHelper.NavigateBySearch("SEN Provision Type", true);

            #region Pre-Condition

            //Delete Old SEN Provision Type if needed
            var senProvisionTypeTriplet = new SenProvisionTypeTriplet();
            senProvisionTypeTriplet.SearchCriteria.CodeOrDecription = code;
            var senProvisionTypePage = senProvisionTypeTriplet.SearchCriteria.Search<SenProvisionTypePage>();
            var senProvisionTypeRow = senProvisionTypePage.SenProvisionTable.Rows.FirstOrDefault(t => t.Code.Equals(code));
            senProvisionTypePage.DeleteTableRow(senProvisionTypeRow);

            #endregion

            #region Test steps

            //Add new SEN Provision Type
            senProvisionTypeTriplet = new SenProvisionTypeTriplet();
            senProvisionTypePage = senProvisionTypeTriplet.AddSenProvisionType();
            senProvisionTypeRow = senProvisionTypePage.SenProvisionTable[0];
            senProvisionTypeRow.Code = code;
            senProvisionTypeRow.Description = description;
            senProvisionTypeRow.DisplayOrder = displayOrder;
            senProvisionTypeRow.IsVisible = true;

            //Save SEN Provision Type
            senProvisionTypePage.ClickSave();

            //Verify success message
            Assert.AreEqual(true, senProvisionTypePage.IsSuccessMessageDisplayed(), "Success message do not display");

            //Search SEN Provision Type by code
            senProvisionTypeTriplet = new SenProvisionTypeTriplet();
            senProvisionTypeTriplet.SearchCriteria.CodeOrDecription = code;
            senProvisionTypePage = senProvisionTypeTriplet.SearchCriteria.Search<SenProvisionTypePage>();

            //Verify data
            senProvisionTypeRow = senProvisionTypePage.SenProvisionTable.Rows.FirstOrDefault(t => t.Code.Equals(code));
            Assert.AreNotEqual(null, senProvisionTypeRow, "Add SEN Provision Type unsuccessfull");
            Assert.AreEqual(description, senProvisionTypeRow.Description, "Description of SEN Provision Type is incorrect");
            Assert.AreEqual(displayOrder, senProvisionTypeRow.DisplayOrder, "Display Order of SEN Provision Type is incorrect");

            #endregion

            #region Post-Condition

            //Delete SEN Provision Type
            senProvisionTypePage.DeleteTableRow(senProvisionTypeRow);

            #endregion
        }

        /// <summary>
        /// TC PU71
        /// Au : An Nguyen
        /// Description: Exercise ability to add a school level SEN Status.
        /// Role: School Administrator
        /// </summary>
        [WebDriverTest(TimeoutSeconds = 600, Enabled = true, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { PupilTestGroups.SenRecord.Page, PupilTestGroups.Priority.Priority2 }, DataProvider = "TC_PU071_Data")]
        public void TC_PU071_Exercise_ability_to_add_a_school_level_SEN_Status(string code, string description, string displayOrder, string category)
        {
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);
            SeleniumHelper.NavigateBySearch("SEN Status", true);

            #region Pre-Condtion

            //Delete Old SEN Status if needed
            var senStatusTriplet = new SenStatusTriplet();
            senStatusTriplet.SearchCriteria.CodeOrDecription = code;
            var senStatusPage = senStatusTriplet.SearchCriteria.Search<SenStatusPage>();
            var senStatusRow = senStatusPage.SenStatusTable.Rows.FirstOrDefault(t => t.Code.Equals(code));
            senStatusPage.DeleteTableRow(senStatusRow);

            #endregion

            #region Test steps

            //Add new SEN Status
            senStatusTriplet = new SenStatusTriplet();
            senStatusPage = senStatusTriplet.AddSenStatus();
            senStatusRow = senStatusPage.SenStatusTable[0];
            senStatusRow.Code = code;
            senStatusRow.Description = description;
            senStatusRow.DisplayOrder = displayOrder;
            senStatusRow.IsVisible = true;
            senStatusRow.Category = category;

            //Save SEN Status
            senStatusPage.ClickSave();

            //Verify success message
            Assert.AreEqual(true, senStatusPage.IsSuccessMessageDisplayed(), "Success message do not display");

            //Search SEN Status by code
            senStatusTriplet = new SenStatusTriplet();
            senStatusTriplet.SearchCriteria.CodeOrDecription = code;
            senStatusPage = senStatusTriplet.SearchCriteria.Search<SenStatusPage>();

            //Verify data
            senStatusRow = senStatusPage.SenStatusTable.Rows.FirstOrDefault(t => t.Code.Equals(code));
            Assert.AreNotEqual(null, senStatusRow, "Add SEN Status unsuccessfull");
            Assert.AreEqual(description, senStatusRow.Description, "Description of SEN Status is incorrect");
            Assert.AreEqual(displayOrder, senStatusRow.DisplayOrder, "Display Order of SEN Status is incorrect");
            Assert.AreEqual(category, senStatusRow.Category, "Category of SEN Status is incorrect");

            #endregion

            #region Post-Condition

            //Delete SEN Status Type
            senStatusPage.DeleteTableRow(senStatusRow);

            #endregion
        }

        /// <summary>
        /// Author: Ba.Truong
        /// Description: PU72: Check exercise ability to create a new SEN Record for a pupil, based on use of the newly added 'SEN Lookup' Records.
        /// </summary>
        [WebDriverTest(TimeoutSeconds = 2000, Enabled = true, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { PupilTestGroups.SenRecord.Page, PupilTestGroups.Priority.Priority2 }, DataProvider = "TC_PU072_Data")]
        public void TC_PU072_Exercise_Ability_To_Create_New_SEN_Record_For_Pupil(string[] pupilRecords, string senStageName, string[] senNeedValues, string[] provisions)
        {
            #region PRE-CONDITION

            //Create a pupil
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);
            SeleniumHelper.NavigateMenu("Tasks", "Pupils", "Pupil Records");
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
            registrationDetailDialog.CreateRecord();

            var confirmRequiredDialog = new ConfirmRequiredDialog();
            confirmRequiredDialog.ClickOk();

            var pupilRecord = new PupilRecordPage();
            pupilRecord.ClickSave();

            //Logout
            SeleniumHelper.Logout();

            #endregion

            #region TEST STEPS

            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SENCoordinator);
            SeleniumHelper.NavigateBySearch("SEN Records", true);

            //Search "No SEN Stage Assigned" sen records 
            var sendRecordTriplet = new SenRecordTriplet();
            sendRecordTriplet.SearchCriteria.Name = String.Format("{0}, {1}", pupilRecords[0], pupilRecords[1]);
            sendRecordTriplet.SearchCriteria.NoSenStageAssigned = true;
            var SenRecordTiles = sendRecordTriplet.SearchCriteria.Search();

            //Select pupil int the results list and confirm that it does NOT have any 'SEN Stage'
            var SenRecord = SenRecordTiles.FirstOrDefault(x => x.Name.Equals(String.Format("{0}, {1}", pupilRecords[0], pupilRecords[1])));
            var SenRecordDetail = SenRecord.Click<SenRecordDetailPage>();
            var senStage = SenRecordDetail.SenStages.Rows.FirstOrDefault();
            Assert.AreEqual(true, String.IsNullOrEmpty(senStage.Stage), "'Sen Stage' of item in list of search results is not empty");

            //In the 'SEN Stage' grid select 'Local GP Consultation' and leave the generated 'Start Date' unchanged (today)
            senStage.Stage = senStageName;

            //Input data in the 'SEN Need' grid 
            SenRecordDetail.SenNeeds[0].NeedType = senNeedValues[0];

            //Update sen need grid after select need type
            SenRecordDetail.SenNeeds[0].Description = senNeedValues[1];
            SenRecordDetail.SenNeeds[0].StartDay = senNeedValues[2];
            SenRecordDetail.SenNeeds[0].EndDay = senNeedValues[3];

            //Input data in 'SEN Provision' grid
            SenRecordDetail.SenProvisions[0].ProvisionType = provisions[0];

            //Update Sen provision grid after select provision type
            SenRecordDetail.SenProvisions[0].EndDay = provisions[1];

            //Save
            SenRecordDetail.Save();
            Assert.AreEqual(true, SenRecordDetail.IsMessageSuccessAppear(), "Success message does not appear");

            //VP: Confirm that this pupil is no longer listed in the results when the "No SEN Stage Assigned" set as true
            sendRecordTriplet.SearchCriteria.NoSenStageAssigned = true;
            SenRecord = sendRecordTriplet.SearchCriteria.Search().FirstOrDefault(x => x.Name.Equals(String.Format("{0}, {1}", pupilRecords[0], pupilRecords[1])));
            Assert.AreEqual(null, SenRecord, "Item is no longer listed in the results when the 'No SEN Stage Assigned' set as true");

            //VP: Confirm that this pupil is listed in the results when the "No SEN Stage Assigned" set as false
            sendRecordTriplet.SearchCriteria.NoSenStageAssigned = false;
            SenRecord = sendRecordTriplet.SearchCriteria.Search().FirstOrDefault(x => x.Name.Equals(String.Format("{0}, {1}", pupilRecords[0], pupilRecords[1])));
            Assert.AreNotEqual(null, SenRecord, "Item is listed in the results when the 'No SEN Stage Assigned' set as false");

            #endregion

            #region POS-CONDITION

            //Re-login by SchoolAdministrator to delete the pupil added
            SeleniumHelper.Logout();
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);
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
        /// Author: Ba.Truong
        /// Description: PU73: Check exercise ability to view a pupil's SEN Details as created by a SEN Coordinator
        /// </summary>
        [WebDriverTest(TimeoutSeconds = 2000, Enabled = true, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { PupilTestGroups.SenRecord.Page, PupilTestGroups.Priority.Priority2 }, DataProvider = "TC_PU073_Data")]
        public void TC_PU073_Exercise_Ability_To_View_Pupil_SEN_Details(string[] pupilRecords, string senStageName, string[] senNeedValues, string[] provisions)
        {
            #region PRE-CONDITION

            //Create a pupil
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);
            SeleniumHelper.NavigateMenu("Tasks", "Pupils", "Pupil Records");
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
            registrationDetailDialog.CreateRecord();

            var confirmRequiredDialog = new ConfirmRequiredDialog();
            confirmRequiredDialog.ClickOk();

            var pupilRecord = new PupilRecordPage();
            pupilRecord.ClickSave();

            //Logout and re-login by SEN Coordinator to add SEN Record for the pupil has just created
            SeleniumHelper.Logout();
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SENCoordinator);

            //Search "No SEN Stage Assigned" sen records 
            SeleniumHelper.NavigateBySearch("SEN Records", true);
            var sendRecordTriplet = new SenRecordTriplet();
            sendRecordTriplet.SearchCriteria.Name = String.Format("{0}, {1}", pupilRecords[0], pupilRecords[1]);
            sendRecordTriplet.SearchCriteria.NoSenStageAssigned = true;
            var SenRecordTiles = sendRecordTriplet.SearchCriteria.Search();

            //Select pupil int the results list and confirm that it does NOT have any 'SEN Stage'
            var SenRecord = SenRecordTiles.FirstOrDefault(x => x.Name.Equals(String.Format("{0}, {1}", pupilRecords[0], pupilRecords[1])));
            var SenRecordDetail = SenRecord.Click<SenRecordDetailPage>();

            //In the 'SEN Stage' grid select 'Local GP Consultation' and leave the generated 'Start Date' unchanged (today)
            SenRecordDetail.SenStages[0].Stage = senStageName;

            //Input data in the 'SEN Need' grid 
            SenRecordDetail.SenNeeds[0].NeedType = senNeedValues[0];
            SenRecordDetail.SenNeeds[0].Description = senNeedValues[1];
            SenRecordDetail.SenNeeds[0].StartDay = senNeedValues[2];

            //Input data in 'SEN Provision' grid
            SenRecordDetail.SenProvisions[0].ProvisionType = provisions[0];

            //Save
            SenRecordDetail.Save();

            //Logout
            SeleniumHelper.Logout();

            #endregion

            #region TEST STEPS

            //Re-login by School Administrator to check SEN record added by 'SEN Coordinator'
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);
            SeleniumHelper.NavigateMenu("Tasks", "Pupils", "Pupil Records");

            //Select pupil who was added SEN Record
            pupilRecordTriplet = new PupilRecordTriplet();
            pupilRecordTriplet.SearchCriteria.PupilName = String.Format("{0}, {1}", pupilRecords[0], pupilRecords[1]);
            var pupilTile = pupilRecordTriplet.SearchCriteria.Search().FirstOrDefault(x => x.Name.Equals(String.Format("{0}, {1}", pupilRecords[0], pupilRecords[1])));
            pupilRecord = pupilTile.Click<PupilRecordPage>();

            //Navigate to the 'Statutory SEN' section of pupil has just created
            pupilRecord.SelectStatutorySenTab();

            //VP: Confirm that the SEN Stage added to this pupil as a SEN Record is displayed correctly
            Assert.AreEqual(senStageName, pupilRecord.SenStages[0].Stage);

            //VP: Confirm that the SEN Need added to this pupil as a SEN Record is displayed correctly
            Assert.AreEqual(senNeedValues[0], pupilRecord.SenNeeds[0].NeedType);
            Assert.AreEqual(senNeedValues[1], pupilRecord.SenNeeds[0].Description);

            //Confirm that the SEN Provision added to this pupil as a SEN Record is displayed correctly
            Assert.AreEqual(provisions[0], pupilRecord.SenProvisions[0].ProvisionType);

            //Select 'SEN Record' via 'Links' panel
            SenRecordDetail = SeleniumHelper.NavigateViaAction<SenRecordDetailPage>("SEN Record");

            //VP: Confirm 'SEN Stage' is displayed
            Assert.AreEqual(senStageName, SenRecordDetail.SenStages[0].Stage);

            //VP: Confirm 'SEN Need' is displayed
            Assert.AreEqual(senNeedValues[0], SenRecordDetail.SenNeeds[0].NeedType);
            Assert.AreEqual(senNeedValues[1], SenRecordDetail.SenNeeds[0].Description);

            //VP: Confirm 'SEN Provision' is displayed
            Assert.AreEqual(provisions[0], SenRecordDetail.SenProvisions[0].ProvisionType);

            #endregion

            #region POS-CONDITION

            //Delete the pupil added
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


        #region DATA

        public List<object[]> TC_PU01_Data()
        {
            string pattern = "M/d/yyyy";
            string PupilDateOfBirth = DateTime.ParseExact("06/01/2006", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            string PupilDateOfAdmission = DateTime.ParseExact("06/01/2010", pattern, CultureInfo.InvariantCulture).ToString(pattern);

            string random = SeleniumHelper.GenerateRandomNumberUsingDateTime();

            string randomCharacter = SeleniumHelper.GenerateRandomString(6);

            var res = new List<Object[]>
            {                
                new object[] 
                {
                    //ForeName
                    "aPupilForeName"+randomCharacter+random.ToString(),
                    // Middle Name
                    randomCharacter+"aPupilMiddleName"+randomCharacter+random.ToString(),
                    // SurName
                    randomCharacter+"aPupilSurName"+randomCharacter+random.ToString(),
                    // Gender
                    "Male",
                    // DOB
                    PupilDateOfBirth,
                    PupilDateOfAdmission,
                    // Year Group
                    "Year 2",
                    "2A",
                    "Full-Time",
                    "Not a Boarder"
                }
                
            };
            return res;
        }

        public List<object[]> TC_PU02_Data()
        {
            string pattern = "M/d/yyyy";
            string PupilDateOfBirth = DateTime.ParseExact("06/01/2006", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            string PupilDateOfAdmission = DateTime.ParseExact("06/01/2010", pattern, CultureInfo.InvariantCulture).ToString(pattern);

            string random = SeleniumHelper.GenerateRandomNumberUsingDateTime();
            string randomCharacter = SeleniumHelper.GenerateRandomString(6);

            var res = new List<Object[]>
            {                
                new object[] 
                {
                    new string[]{"aForeName"+randomCharacter+random.ToString(),"aMiddleName"+randomCharacter+random.ToString(),"aSurName"+randomCharacter+random.ToString(),"Male",PupilDateOfBirth,PupilDateOfAdmission,"Year 2","2A","Full-Time","Not a Boarder"},                   
                    true,
                    // Quick Note
                    "This is Quick Note"                                        
                }
                
            };
            return res;
        }

        public List<object[]> TC_PU03_Data()
        {
            string pattern = "M/d/yyyy";
            string PupilDateOfBirth = DateTime.ParseExact("06/01/2006", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            string PupilDateOfAdmission = DateTime.ParseExact("06/01/2010", pattern, CultureInfo.InvariantCulture).ToString(pattern);

            string random = SeleniumHelper.GenerateRandomNumberUsingDateTime();
            string randomCharacter = SeleniumHelper.GenerateRandomString(6);

            var res = new List<Object[]>
            {                
                new object[] 
                {
                     new string[]{"aForeName"+randomCharacter+randomCharacter+random.ToString(),"aMiddleName"+randomCharacter+random.ToString(),"aSurName"+randomCharacter+random.ToString(),"Male",PupilDateOfBirth,PupilDateOfAdmission,"Year 2","2A","Full-Time","Not a Boarder"},                                        
                }
                
            };
            return res;
        }
        public List<object[]> TC_PU04_Data()
        {
            string pattern = "M/d/yyyy";
            string PupilDateOfBirth = DateTime.ParseExact("06/01/2006", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            string PupilDateOfAdmission = DateTime.ParseExact("06/01/2010", pattern, CultureInfo.InvariantCulture).ToString(pattern);

            string random = SeleniumHelper.GenerateRandomNumberUsingDateTime();
            string randomCharacter = SeleniumHelper.GenerateRandomString(6);
            var res = new List<Object[]>
            {                
                new object[] 
                {
                    new string[]{"aForeName"+randomCharacter+random.ToString(),"aMiddleName"+randomCharacter+random.ToString(),"aSurName"+randomCharacter+random.ToString(),"Male",PupilDateOfBirth,PupilDateOfAdmission,"Year 2","2A","Full-Time","Not a Boarder"},
                    // New Address
                    new string[]{"123", "House Name", "Flat", "Street", "District", "City", "United Kingdom", "EC1A 1BB", "United Kingdom"}
                }
                
            };
            return res;
        }


        public List<object[]> TC_PU05_Data()
        {
            string pattern = "M/d/yyyy";
            string PupilDateOfBirth = DateTime.ParseExact("06/01/2006", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            string PupilDateOfAdmission = DateTime.ParseExact("06/01/2010", pattern, CultureInfo.InvariantCulture).ToString(pattern);

            string random = SeleniumHelper.GenerateRandomNumberUsingDateTime();
            string randomCharacter = SeleniumHelper.GenerateRandomString(6);
            var res = new List<Object[]>
            {                
                new object[] 
                {
                    new string[]{"aForeName"+randomCharacter+random.ToString(),"aMiddleName"+randomCharacter+random.ToString(),"aSurName"+randomCharacter+random.ToString(),"Male",PupilDateOfBirth,PupilDateOfAdmission,"Year 2","2A","Full-Time","Not a Boarder"},
                    // New Address
                    new string[]{"0123456789", "Other", "Note for Phone Number"},
                    new string[]{"abcd@c2kni.org.uk", "Other", "Note for Email Address"},
                }
                
            };
            return res;
        }

        public List<object[]> TC_PU06_Data()
        {
            string pattern = "M/d/yyyy";
            string PupilDateOfBirth = DateTime.ParseExact("06/01/2006", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            string PupilDateOfAdmission = DateTime.ParseExact("06/01/2010", pattern, CultureInfo.InvariantCulture).ToString(pattern);

            string random = SeleniumHelper.GenerateRandomNumberUsingDateTime();
            string randomCharacter = SeleniumHelper.GenerateRandomString(6);
            var res = new List<Object[]>
            {                
                new object[] 
                {
                    new string[]{"aForeName"+randomCharacter+random.ToString(),"aMiddleName"+randomCharacter+random.ToString(),"aSurName"+randomCharacter+random.ToString(),"Male",PupilDateOfBirth,PupilDateOfAdmission,"Year 2","2A","Full-Time","Not a Boarder"},
                }
                
            };
            return res;
        }
        public List<object[]> TC_PU07_Data()
        {
            string pattern = "M/d/yyyy";
            string EligibleFreeMealStartDatePast = DateTime.ParseExact("06/01/2006", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            string EligibleFreeMealEndDatePast = DateTime.ParseExact("06/01/2008", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            string EligibleFreeMealStartDateToday = DateTime.ParseExact("06/01/2010", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            string EligibleFreeMealEndDateToday = DateTime.ParseExact("06/01/2015", pattern, CultureInfo.InvariantCulture).ToString(pattern);

            // Meal Pattern
            string MealPatternStartDate = DateTime.ParseExact("06/01/2006", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            string MealPatternEndDate = DateTime.ParseExact("06/01/2008", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            string MealPatternStartToday = DateTime.ParseExact("06/01/2010", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            string MealPatternEndToday = DateTime.ParseExact("06/01/2015", pattern, CultureInfo.InvariantCulture).ToString(pattern);

            string PupilDateOfBirth = DateTime.ParseExact("06/01/2006", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            string PupilDateOfAdmission = DateTime.ParseExact("06/01/2010", pattern, CultureInfo.InvariantCulture).ToString(pattern);

            string random = SeleniumHelper.GenerateRandomNumberUsingDateTime();
            string randomCharacter = SeleniumHelper.GenerateRandomString(6);

            var res = new List<Object[]>
            {                
                new object[] 
                {
                   new string[]{"aForeName"+randomCharacter+random.ToString(),"aMiddleName"+randomCharacter+random.ToString(),"aSurName"+randomCharacter+random.ToString(),"Male",PupilDateOfBirth,PupilDateOfAdmission,"Year 2","2A","Full-Time","Not a Boarder"},
                    // Eligible Free Meal
                    new string[]{ EligibleFreeMealStartDatePast, EligibleFreeMealEndDatePast, EligibleFreeMealStartDateToday,EligibleFreeMealEndDateToday,"This is note"},
                    // Meal Pattern
                    new string[]{MealPatternStartDate,MealPatternEndDate,MealPatternStartToday,MealPatternEndToday}
                }
                
            };
            return res;
        }

        public List<object[]> TC_PU08_Data()
        {
            string pattern = "M/d/yyyy";
            string dateEvent = DateTime.ParseExact("06/06/2010", pattern, CultureInfo.InvariantCulture).ToString(pattern);

            string PupilDateOfBirth = DateTime.ParseExact("06/01/2006", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            string PupilDateOfAdmission = DateTime.ParseExact("06/01/2010", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            string random = SeleniumHelper.GenerateRandomNumberUsingDateTime();
            string randomCharacter = SeleniumHelper.GenerateRandomString(6);
            var res = new List<Object[]>
            {            
                
                new object[] 
                {
                    new string[]{"aForeName"+randomCharacter+random.ToString(),"aMiddleName"+randomCharacter+random.ToString(),"aSurName"+randomCharacter+random.ToString(),"Male",PupilDateOfBirth,PupilDateOfAdmission,"Year 2","2A","Full-Time","Not a Boarder"},
                    //NSHNumber
                    "123456",
                    // Medical Practice
                    new string [] {"Bushmills Medical Centre","Dr T Brown"},
                    // Summary Note
                    "Summary",
                    // Medical Description
                    "None",
                    // Medical Event
                    new string [] {"Accident","Accident",dateEvent}
                }
                
            };
            return res;
        }

        public List<object[]> TC_PU09_Data()
        {
            string pattern = "M/d/yyyy";
            string dateStart = DateTime.ParseExact("06/06/2010", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            string dateEnd = DateTime.ParseExact("06/06/2015", pattern, CultureInfo.InvariantCulture).ToString(pattern);

            string PupilDateOfBirth = DateTime.ParseExact("06/01/2006", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            string PupilDateOfAdmission = DateTime.ParseExact("06/01/2010", pattern, CultureInfo.InvariantCulture).ToString(pattern);

            string random = SeleniumHelper.GenerateRandomNumberUsingDateTime();
            string randomCharacter = SeleniumHelper.GenerateRandomString(6);
            var res = new List<Object[]>
            {                
                new object[] 
                {
                    new string[]{"aForeName"+randomCharacter+random.ToString(),"aMiddleName"+randomCharacter+random.ToString(),"aSurName"+randomCharacter+random.ToString(),"Male",PupilDateOfBirth,PupilDateOfAdmission,"Year 2","2A","Full-Time","Not a Boarder"},
                    //Ethnicity
                    "Vietnamese",
                    //HomeLanguage
                    "English",
                    //religion
                    "Church of God",
                    //AccommodationType
                    "Other",
                    //AsylumStatus
                    "Refugee",
                    //NewcomerPeriods
                    new string [] {dateStart,dateEnd},
                    dateStart
                }
                
            };
            return res;
        }

        public List<object[]> TC_PU10_Data()
        {
            string pattern = "M/d/yyyy";
            string dateStart = DateTime.ParseExact("06/06/2010", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            string dateEnd = DateTime.ParseExact("06/06/2015", pattern, CultureInfo.InvariantCulture).ToString(pattern);

            string PupilDateOfBirth = DateTime.ParseExact("06/01/2006", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            string PupilDateOfAdmission = DateTime.ParseExact("06/01/2010", pattern, CultureInfo.InvariantCulture).ToString(pattern);

            string random = SeleniumHelper.GenerateRandomNumberUsingDateTime();
            string randomCharacter = SeleniumHelper.GenerateRandomString(6);
            var res = new List<Object[]>
            {                
                new object[] 
                {
                    new string[]{"aForeName"+randomCharacter+random.ToString(),"aMiddleName"+randomCharacter+random.ToString(),"aSurName"+randomCharacter+random.ToString(),"Male",PupilDateOfBirth,PupilDateOfAdmission,"Year 2","2A","Full-Time","Not a Boarder"},
                    //ServiceChildren
                    "Unknown",
                    //ServiceChildrenSource
                    "Other",
                    //ModeTravel
                    "Taxi",
                    //TravelRoute
                    "",
                    //AcademicYear                    
                    "Academic Year 2016/2017",
                    //LearnerUniformGrantEligibilities
                    new string [] {dateStart,dateEnd}
                }
                
            };
            return res;
        }

        public List<object[]> TC_PU11_Data()
        {
            string pattern = "M/d/yyyy";
            var now = DateTime.Now;
            var firstDay = new DateTime(now.Year, now.Month, 1);
            var lastDay = new DateTime(now.Year, 12, 31);
            string dateStart = firstDay.ToString(pattern);
            string dateEnd = lastDay.ToString(pattern);

            string PupilDateOfBirth = DateTime.ParseExact("06/01/2006", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            string PupilDateOfAdmission = DateTime.ParseExact("06/01/2010", pattern, CultureInfo.InvariantCulture).ToString(pattern);

            string random = SeleniumHelper.GenerateRandomNumberUsingDateTime();
            string randomCharacter = SeleniumHelper.GenerateRandomString(6);

            var res = new List<Object[]>
            {                
                new object[] 
                {
                    new string[]{"aForeName"+randomCharacter+random.ToString(),"aMiddleName"+randomCharacter+random.ToString(),"aSurName"+randomCharacter+random.ToString(),"Male",PupilDateOfBirth,PupilDateOfAdmission,"Year 2","2A","Full-Time","Not a Boarder"},
                    //CareAuthority
                    "Northern Health and Social Care Trust",
                    //LivingArrangement
                   "Foster Care",
                    dateStart,
                    dateEnd
                }
                
            };
            return res;
        }

        public List<object[]> TC_PU12_Data()
        {
            string pattern = "M/d/yyyy";
            var now = DateTime.Now;
            var firstStartDay = new DateTime(now.Year, now.Month, now.Day);
            var firstEndDay = firstStartDay.AddMonths(3);
            string dateStart = firstStartDay.ToString(pattern);
            string dateEnd = firstEndDay.ToString(pattern);

            var secondStartDate = firstEndDay.AddDays(1);
            var secondEndDate = new DateTime(firstEndDay.Year, 12, 25);

            string PupilDateOfBirth = DateTime.ParseExact("06/01/2006", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            string PupilDateOfAdmission = DateTime.ParseExact("06/01/2010", pattern, CultureInfo.InvariantCulture).ToString(pattern);

            string random = SeleniumHelper.GenerateRandomNumberUsingDateTime();
            string randomCharacter = SeleniumHelper.GenerateRandomString(6);
            var res = new List<Object[]>
            {                
                new object[] 
                {
                    new string[]{"aForeName"+randomCharacter+random.ToString(),"aMiddleName"+randomCharacter+random.ToString(),"aSurName"+randomCharacter+random.ToString(),"Male",PupilDateOfBirth,PupilDateOfAdmission,"Year 2","2A","Full-Time","Not a Boarder"},           
                    dateStart,
                    dateEnd,
                    secondStartDate.ToString(pattern),
                    secondEndDate.ToString(pattern),
                    //CareAuthority
                    "Northern Health and Social Care Trust",
                    //LivingArrangement
                   "Foster Care",
                }
                
            };
            return res;
        }

        public List<object[]> TC_PU13_Data()
        {
            string pattern = "M/d/yyyy";
            string PupilDateOfBirth = DateTime.ParseExact("06/01/2006", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            string PupilDateOfAdmission = DateTime.ParseExact("06/01/2010", pattern, CultureInfo.InvariantCulture).ToString(pattern);

            string random = SeleniumHelper.GenerateRandomNumberUsingDateTime();
            string randomCharacter = SeleniumHelper.GenerateRandomString(6);
            var res = new List<Object[]>
            {                
                new object[] 
                {
                    new string[]{"aForeName"+randomCharacter+random.ToString(),"aMiddleName"+randomCharacter+random.ToString(),"aSurName"+randomCharacter+random.ToString(),"Male",PupilDateOfBirth,PupilDateOfAdmission,"Year 2","2A","Full-Time","Not a Boarder"},                       
                }
                
            };
            return res;
        }

        public List<object[]> TC_PU14_Data()
        {
            string pattern = "M/d/yyyy";
            var year = DateTime.Now.Year;
            var newYear = year++;
            string yearEdit = String.Format("{0}/{1}", year, newYear);
            string PupilDateOfBirth = DateTime.ParseExact("06/01/2006", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            string PupilDateOfAdmission = DateTime.ParseExact("06/01/2010", pattern, CultureInfo.InvariantCulture).ToString(pattern);

            string random = SeleniumHelper.GenerateRandomNumberUsingDateTime();
            string randomCharacter = SeleniumHelper.GenerateRandomString(6);
            var res = new List<Object[]>
            {                
                new object[] 
                {
                    new string[]{"aForeName"+randomCharacter+random.ToString(),"aMiddleName"+randomCharacter+random.ToString(),"aSurName"+randomCharacter+random.ToString(),"Male",PupilDateOfBirth,PupilDateOfAdmission,"Year 2","2A","Full-Time","Not a Boarder"},
                    yearEdit         
                }
                
            };
            return res;
        }

        public List<object[]> TC_PU15_Data()
        {
            string pattern = "M/d/yyyy";
            string PupilDateOfAdmission = DateTime.ParseExact("06/01/2010", pattern, CultureInfo.InvariantCulture).ToString(pattern);

            string PupilDateOfBirth = DateTime.ParseExact("06/01/2006", pattern, CultureInfo.InvariantCulture).ToString(pattern);

            string random = SeleniumHelper.GenerateRandomNumberUsingDateTime();
            string randomCharacter = SeleniumHelper.GenerateRandomString(6);
            var res = new List<Object[]>
            {                
                new object[] 
                {
                    new string[]{"aForeName"+randomCharacter+random.ToString(),"aMiddleName"+randomCharacter+random.ToString(),"aSurName"+randomCharacter+random.ToString(),"Male",PupilDateOfBirth,PupilDateOfAdmission,"Year 2","2A","Full-Time","Not a Boarder"},         
                    PupilDateOfAdmission,
                    //ConsentSignatory
                    "Parent Signatory Received",
                    //Note
                    "Parental consent status duly recorded"
                }
                
            };
            return res;
        }

        public List<object[]> TC_PU16_Data()
        {
            string pattern = "M/d/yyyy";
            var now = DateTime.Now;
            var toDay = new DateTime(now.Year, now.Month, now.Day);

            var endDate = new DateTime(toDay.Year, 12, 25);

            string PupilDateOfBirth = DateTime.ParseExact("06/01/2006", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            string PupilDateOfAdmission = DateTime.ParseExact("06/01/2010", pattern, CultureInfo.InvariantCulture).ToString(pattern);

            string random = SeleniumHelper.GenerateRandomNumberUsingDateTime();
            string randomCharacter = SeleniumHelper.GenerateRandomString(6);
            var res = new List<Object[]>
            {                
                new object[] 
                {
                    new string[]{"aForeName"+randomCharacter+random.ToString(),"aMiddleName"+randomCharacter+random.ToString(),"aSurName"+randomCharacter+random.ToString(),"Male",PupilDateOfBirth,PupilDateOfAdmission,"Year 2","2A","Full-Time","Not a Boarder"},       
                   toDay.ToString(pattern),
                   endDate.ToString(pattern),
                   //SenStagesStage
                   "Identify need",
                   // SenNeedType
                   "Mild Learning Difficulties",
                   //SenNeedsRank
                   "1",
                   //Sen Description
                   "GP",
                   //SenProvisionsType
                   "Time in Specialist Unit",
                   // Comment
                   "Requested Time in Special Unit"
                }
                
            };
            return res;
        }

        public List<object[]> TC_PU17_Data()
        {
            string pattern = "M/d/yyyy";
            string PupilDateOfBirth = DateTime.ParseExact("06/01/2006", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            string PupilDateOfAdmission = DateTime.ParseExact("06/01/2010", pattern, CultureInfo.InvariantCulture).ToString(pattern);

            string random = SeleniumHelper.GenerateRandomNumberUsingDateTime();
            string randomCharacter = SeleniumHelper.GenerateRandomString(6);

            var res = new List<Object[]>
            {                
                new object[] 
                {
                    new string[]{"aForeName"+randomCharacter+random.ToString(),"aMiddleName"+randomCharacter+random.ToString(),"aSurName"+randomCharacter+random.ToString(),"Male",PupilDateOfBirth,PupilDateOfAdmission,"Year 2","2A","Full-Time","Not a Boarder"},         
                    // Note
                    "Remove"
                }
                
            };
            return res;
        }

        public List<object[]> TC_PU019_Data()
        {
            string pattern = "M/d/yyyy";
            string PupilDateOfBirth = DateTime.ParseExact("06/01/2006", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            string PupilDateOfAdmission = DateTime.ParseExact("06/01/2010", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            string foreName = String.Format("{0}_{1}", SeleniumHelper.GenerateRandomString(7), SeleniumHelper.GenerateRandomNumberUsingDateTime());
            string surName = String.Format("{0}_{1}", "Avn", SeleniumHelper.GenerateRandomString(8));
            var res = new List<Object[]>
            {                
                new object[] 
                {                    
                    foreName, surName, "Male", PupilDateOfBirth, PupilDateOfAdmission, "Year 2", "SEN",
                }
                
            };
            return res;
        }

        public List<object[]> TC_PU020_Data()
        {
            string pattern = "M/d/yyyy";
            string PupilDateOfBirth = DateTime.ParseExact("06/01/2006", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            string PupilDateOfAdmission = DateTime.ParseExact("06/01/2010", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            string foreName = String.Format("{0}_{1}", SeleniumHelper.GenerateRandomString(7), SeleniumHelper.GenerateRandomNumberUsingDateTime());
            string surName = String.Format("{0}_{1}", "Avn", SeleniumHelper.GenerateRandomString(8));
            var res = new List<Object[]>
            {                
                new object[] 
                {
                    foreName, surName, "Male", PupilDateOfBirth, PupilDateOfAdmission, "Year 2", "SEN",
                    new string[]{"1234", "House", "Flat", "Street", "District", "City", "County", "EC1A 1BB", "United Kingdom"},
                    new string[]{"567", "House Name", "Flat2", "Street2", "District2", "City2", "Conty2", "EC1A 1BB", "United Kingdom"},
                }
                
            };
            return res;
        }

        public List<object[]> TC_PU021_Data()
        {
            string pattern = "M/d/yyyy";
            string PupilDateOfBirth = DateTime.ParseExact("06/01/2006", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            string PupilDateOfAdmission = DateTime.ParseExact("06/01/2010", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            string foreName = String.Format("{0}_{1}", SeleniumHelper.GenerateRandomString(7), SeleniumHelper.GenerateRandomNumberUsingDateTime());
            string surName = String.Format("{0}_{1}", "Avn", SeleniumHelper.GenerateRandomString(8));
            string pastStart1 = DateTime.ParseExact("01/01/2015", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            string pastEnd1 = DateTime.ParseExact("05/05/2015", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            string pastStart2 = DateTime.ParseExact("06/05/2015", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            string pastEnd2 = DateTime.Now.Subtract(TimeSpan.FromDays(1)).ToString(pattern);
            var res = new List<Object[]>
            {                
                new object[] 
                {
                    foreName, surName, "Male", PupilDateOfBirth, PupilDateOfAdmission, "Year 2", "SEN",
                    new string[]{"123", "House", "Flat", "Street", "District", "City", "County", "EC1A 1BB", "United Kingdom"}, pastStart1, pastEnd1,
                    new string[]{"234", "House Name", "Flat2", "Street2", "District2", "City2", "County2", "EC1A 1BB", "United Kingdom"}, pastStart2, pastEnd2,
                    new string[]{"567", "House Name", "Flat3", "Street3", "District3", "City3", "County2", "EC1A 1BB", "United Kingdom"},
                }
                
            };
            return res;
        }

        public List<object[]> TC_PU022_Data()
        {
            string pattern = "M/d/yyyy";
            string PupilDateOfBirth = DateTime.ParseExact("06/01/2006", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            string PupilDateOfAdmission = DateTime.ParseExact("06/01/2010", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            string foreName = String.Format("{0}_{1}", SeleniumHelper.GenerateRandomString(7), SeleniumHelper.GenerateRandomNumberUsingDateTime());
            string surName = String.Format("{0}_{1}", "Avn", SeleniumHelper.GenerateRandomString(8));
            var res = new List<Object[]>
            {                
                new object[] 
                {
                    foreName, surName, "Male", PupilDateOfBirth, PupilDateOfAdmission, "Year 2", "SEN",
                    "Mr", "LG", "Contact",
                    new string[]{"1234", "House", "Flat", "Street", "District", "City", "County", "EC1A 1BB", "United Kingdom"},
                    new string[]{"567", "House Name", "Flat2", "Street2", "District2", "City2", "Conty2", "EC1A 1BB", "United Kingdom"},
                }
                
            };
            return res;
        }

        public List<object[]> TC_PU023_Data()
        {
            string pattern = "M/d/yyyy";
            string dateOfLeaving = SeleniumHelper.GetFirstDayOfWeek(DateTime.Now.Subtract(TimeSpan.FromDays(14))).ToString(pattern);
            string dateOfBirth = DateTime.ParseExact("06/01/2006", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            string PupilDateOfAdmission = DateTime.ParseExact("06/01/2010", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            string dateOfAdmission = SeleniumHelper.GetFirstDayOfWeek(DateTime.Now.Add(TimeSpan.FromDays(14))).ToString(pattern);
            string foreName = String.Format("{0}_{1}", SeleniumHelper.GenerateRandomString(7), SeleniumHelper.GenerateRandomNumberUsingDateTime());
            string surName = String.Format("{0}_{1}", "Avn", SeleniumHelper.GenerateRandomString(8));
            var res = new List<Object[]>
            {                
                new object[] 
                {                    
                    foreName, surName, "Male", dateOfBirth, PupilDateOfAdmission, "Year 2", "SEN", dateOfLeaving, "Not Known", dateOfAdmission
                }
                
            };
            return res;
        }

        public List<object[]> TC_PU024_Data()
        {
            string pattern = "M/d/yyyy";
            string dateOfBirth = DateTime.ParseExact("06/01/2006", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            string dateOfAdmission = DateTime.ParseExact("06/01/2010", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            string patternStart = SeleniumHelper.GetFirstDayOfWeek(DateTime.Now).ToString(pattern);
            string patternEnd = SeleniumHelper.GetLastDayOfWeek(DateTime.Now).ToString(pattern);
            string foreName = String.Format("{0}_{1}_{2}", "Avn", SeleniumHelper.GenerateRandomString(7), SeleniumHelper.GenerateRandomNumberUsingDateTime());
            string surName = String.Format("{0}_{1}", "Aa", SeleniumHelper.GenerateRandomString(7));
            var res = new List<Object[]>
            {                
                new object[] 
                {                    
                    foreName, surName, "Male", dateOfBirth, dateOfAdmission, "Year 2", "SEN", "AM only", patternStart, patternEnd,
                }
                
            };
            return res;
        }

        public List<object[]> TC_PU025_Data()
        {
            string pattern = "M/d/yyyy";
            string dateOfBirth = DateTime.ParseExact("06/01/2006", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            string dateOfAdmission = DateTime.ParseExact("06/01/2010", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            string dateOfLeaving = SeleniumHelper.GetFirstDayOfWeek(DateTime.Now.Subtract(TimeSpan.FromDays(7))).ToString(pattern);
            string foreName1 = String.Format("{0}_{1}_{2}", "Avn", SeleniumHelper.GenerateRandomString(7), SeleniumHelper.GenerateRandomNumberUsingDateTime());
            string surName1 = String.Format("{0}_{1}", "Aa", SeleniumHelper.GenerateRandomString(7));
            string foreName2 = String.Format("{0}_{1}_{2}", "Avn", SeleniumHelper.GenerateRandomString(7), SeleniumHelper.GenerateRandomNumberUsingDateTime());
            string surName2 = String.Format("{0}_{1}", "Ab", SeleniumHelper.GenerateRandomString(7));
            var res = new List<Object[]>
            {                
                new object[] 
                {
                    new string[]{foreName1, surName1, "Male", dateOfBirth, dateOfAdmission}, 
                    new string[]{foreName2, surName2, "Female", dateOfBirth, dateOfAdmission},
                    "Single Registration", "Year 2", "SEN", dateOfLeaving, "Voluntary transfer", "Queens Secondary School"
                }
                
            };
            return res;
        }


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


        public List<object[]> TC_PU027_Data()
        {
            var randomName = "Luong" + SeleniumHelper.GenerateRandomString(5) + SeleniumHelper.GenerateRandomNumberUsingDateTime();
            var res = new List<Object[]>
            {                
                new object[] 
                {
                    // Forename
                    randomName,
                    // Surname
                    randomName,
                    // Gender
                    "Male",
                    // DOB
                    "2/2/2011",
                    //DateOfAdmission
                    "10/5/2014",
                    // YearGroup
                    "Year 1",
                    // DOL
                    "11/5/2014",
                    // ReasonForLeaving
                    "Not Known",
                    // EnrolmentStatus
                    "Single Registration",
                    //re-admit date
                    "10/5/2015",
                }
                
            };
            return res;
        }

        public List<object[]> TC_PU028_Data()
        {
            var randomName = "Luong" + SeleniumHelper.GenerateRandomString(5) + SeleniumHelper.GenerateRandomNumberUsingDateTime();

            var res = new List<Object[]>
            {                
                new object[] 
                {
                    // Forename
                    randomName,
                    // Surname
                    randomName,
                    // Gender
                    "Male",
                    // DOB
                    "2/2/2011",
                    //DateOfAdmission
                    "10/5/2014",
                    // YearGroup
                    "Year 1",
                    // DOL
                    "11/5/2014",
                    // ReasonForLeaving
                    "Not Known",
                    // EnrolmentStatus
                    "Single Registration",
                    //re-admit date
                    "10/5/2015",
                }
                
            };
            return res;
        }

        public List<object[]> TC_PU029_Data()
        {
            var randomName = Thread.CurrentThread.ManagedThreadId + "Luong" + SeleniumHelper.GenerateRandomString(5) + SeleniumHelper.GenerateRandomNumberUsingDateTime();

            var res = new List<Object[]>
            {                
                new object[] 
                {
                    // Forename contact
                    randomName,
                    // Surname contact
                    randomName,
                    // Title
                    "Mr",
                    // Gender
                    "Male",
                    // Salutation,
                    "Prof Aaron",
                    // Addressee
                    "Prof J Aaron",
                    // BuildingNo
                    "20",
                    // Street
                    "Bushfoot Road",
                    // District
                    "Portballintrae",
                    // City
                    "Bushmills",
                    // County
                    "",
                    // PostCode
                    "BT57 8RR",
                    // CountryPostCode
                    "United Kingdom",
                    // Language
                    "English",
                    // PlaceOfWork
                    "Northern Ireland",
                    // JobTitle
                    "",
                    // Occupation
                    "",
                    // Forename pupil
                    "Fiona",
                    // Surname pupil
                    "Baker",
                    // Priority
                    "1",
                    // Relationship
                    "Parent"
                }
                
            };
            return res;
        }

        public List<object[]> TC_PU030_Data()
        {
            var randomName = "Luong" + SeleniumHelper.GenerateRandomString(5) + SeleniumHelper.GenerateRandomNumberUsingDateTime();

            var res = new List<Object[]>
            {                
                new object[] 
                {
                    // Forename contact
                    randomName,
                    // Surname contact
                    randomName,
                    // Title
                    "Mr",
                    // Gender
                    "Male",
                    // Salutation,
                    "Prof Aaron",
                    // Addressee
                    "Prof J Aaron"
                }
                
            };
            return res;
        }

        public List<object[]> TC_PU031a_Data()
        {
            var randomContact1 = "Luong" + SeleniumHelper.GenerateRandomString(5) + SeleniumHelper.GenerateRandomNumberUsingDateTime();
            var randomContact2 = "Luong" + SeleniumHelper.GenerateRandomString(4) + SeleniumHelper.GenerateRandomNumberUsingDateTime();

            var res = new List<Object[]>
            {                
                new object[] 
                {
                    // Forename contact
                    "Stephen",
                    // Surname contact
                    "Baker",
                    // Forename contact 2
                    "Gareth",
                    // Surname contact 2
                    "Baker",
                    // Pupil name
                    "Baker, Fiona",
                    // Title
                    "Rev",
                    // Gender
                    "Male",
                    // Priority
                    "2",
                    // Relationship
                    "Parent",
                    // Salutation,
                    "Prof Aaron",
                    // Addressee
                    "Prof J Aaron"
                }
                
            };
            return res;
        }

        public List<object[]> TC_PU031b_Data()
        {
            var randomPupil = "Luong" + SeleniumHelper.GenerateRandomString(5) + SeleniumHelper.GenerateRandomNumberUsingDateTime();
            var randomContact = "Luong" + SeleniumHelper.GenerateRandomString(4) + SeleniumHelper.GenerateRandomNumberUsingDateTime();

            var res = new List<Object[]>
            {                
                new object[] 
                {
                    // Forename pupil
                    randomPupil,
                    // Surname pupil
                    randomPupil,
                    // Full name contact 1
                    "Mr " + randomContact + " " + randomContact,
                    // Title Contact 1
                    "Mr",
                    // Forename contact 1
                    randomContact,
                    // Surname contact 1
                    randomContact,
                    // Gender contact 1
                    "Male"
                }
                
            };
            return res;
        }

        public List<object[]> TC_PU032_Data()
        {
            var randomContact1 = "Luong" + SeleniumHelper.GenerateRandomString(4) + SeleniumHelper.GenerateRandomNumberUsingDateTime();
            var randomContact2 = "Luong" + SeleniumHelper.GenerateRandomString(6) + SeleniumHelper.GenerateRandomNumberUsingDateTime();

            var res = new List<Object[]>
            {                
                new object[] 
                {
                    // Forename pupil
                    "Laura", 
                    // Surname pupil
                    "Adams",
                    // Title 1,
                    "Rev",
                    // Forename 1
                    randomContact1,
                    // Surname 1
                    randomContact1,
                    // Title 2,
                    "Mr",
                    // Forename 2
                    randomContact2,
                    // Surname 2
                    randomContact2,
                    // Forename pupil 2
                    "Carlton", 
                    // Surname pupil 2
                    "Allcroft",

                }
                
            };
            return res;
        }

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
            var randomName = "Luong" + SeleniumHelper.GenerateRandomString(4) + SeleniumHelper.GenerateRandomNumberUsingDateTime();

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

        public List<object[]> TC_PU036b_Data()
        {
            var randomName = "Luong" + SeleniumHelper.GenerateRandomString(6) + SeleniumHelper.GenerateRandomNumberUsingDateTime();

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
            var randomName = "Luong" + SeleniumHelper.GenerateRandomString(4) + SeleniumHelper.GenerateRandomNumberUsingDateTime();

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

        public List<object[]> TC_PU037_Data()
        {
            var res = new List<Object[]>
            {                
                new object[] 
                {
                    // pupil name,
                    "Bains, Kirk",
                    // fore name
                    "Bains"
                }
                
            };
            return res;
        }

        public List<object[]> TC_PU038_Data()
        {
            var res = new List<Object[]>
            {                
                new object[] 
                {
                    // pupil name,
                    "Bains, Kirk",
                    // fore name
                    "Bains"
                }
                
            };
            return res;
        }

        public List<object[]> TC_PU039_Data()
        {
            var res = new List<Object[]>
            {                
                new object[] 
                {
                    // pupil name,
                    "Bains, Kirk",
                    // fore name
                    "Bains"
                }
                
            };
            return res;
        }

        public List<object[]> TC_PU040_Data()
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

        public List<object[]> TC_PU043_DATA()
        {
            string pattern = "M/d/yyyy";
            string surName = SeleniumHelper.GenerateRandomString(8);
            string foreName = SeleniumHelper.GenerateRandomString(8);
            string pupilName = String.Format("{0}, {1}", surName, foreName);
            string dateOfBirth = DateTime.ParseExact("10/16/2006", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            string DateOfAdmission = DateTime.ParseExact("10/19/2010", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            string dateOfLeaving = SeleniumHelper.GetDateFromDayOfWeek((int)DayOfWeek.Monday);

            var data = new List<Object[]>
                {
                    new object[] {surName, foreName, "Male", dateOfBirth, DateOfAdmission, "Year 2", "SEN", pupilName, dateOfLeaving, "Not Known", "Outside Local District"}

                };
            return data;
        }

        public List<object[]> TC_PU044A_DATA()
        {
            string pattern = "M/d/yyyy";
            string surName = SeleniumHelper.GenerateRandomString(8);
            string foreName = SeleniumHelper.GenerateRandomString(8);
            string pupilName = String.Format("{0}, {1}", surName, foreName);
            string dateOfBirth = DateTime.ParseExact("10/16/2006", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            string DateOfAdmission = DateTime.ParseExact("10/19/2010", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            string date = SeleniumHelper.GetDayAfter(SeleniumHelper.GetDateFromDayOfWeek((int)DayOfWeek.Monday, true), -7);

            var data = new List<Object[]>
                {
                    new object[] {surName, foreName, "Male", dateOfBirth, DateOfAdmission, "Year 2", "SEN", pupilName, "Charles", "Jon", 
                        "Man-Warning", "Deed Poll", date }

                };
            return data;
        }

        public List<object[]> TC_PU044B_DATA()
        {
            string pattern = "M/d/yyyy";
            string surName = SeleniumHelper.GenerateRandomString(8);
            string foreName = SeleniumHelper.GenerateRandomString(8);
            string pupilName = String.Format("{0}, {1}", surName, foreName);
            string dateOfBirth = DateTime.ParseExact("10/16/2006", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            string DateOfAdmission = DateTime.ParseExact("10/19/2010", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            string startDate = SeleniumHelper.GetDayAfter(SeleniumHelper.GetDateFromDayOfWeek((int)DayOfWeek.Monday, true), -14);
            string endDate = SeleniumHelper.GetDayAfter(SeleniumHelper.GetDateFromDayOfWeek((int)DayOfWeek.Monday, true), -7);



            var data = new List<Object[]>
                {
                    new object[] {surName, foreName, "Male", dateOfBirth, DateOfAdmission, "Year 2", "SEN", pupilName, startDate, endDate, "This is now a former address"}

                };
            return data;
        }

        public List<object[]> TC_PU044C_DATA()
        {

            string pattern = "M/d/yyyy";
            string surName = SeleniumHelper.GenerateRandomString(8);
            string foreName = SeleniumHelper.GenerateRandomString(8);
            string pupilName = String.Format("{0}, {1}", surName, foreName);
            string dateOfBirth = DateTime.ParseExact("10/16/2006", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            string DateOfAdmission = DateTime.ParseExact("10/19/2010", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            string registerDate = SeleniumHelper.GetDayAfter(SeleniumHelper.GetDateFromDayOfWeek((int)DayOfWeek.Monday, true), -7);
            string endDate = SeleniumHelper.GetDayAfter(SeleniumHelper.GetDateFromDayOfWeek((int)DayOfWeek.Friday, true), -7);

            var data = new List<Object[]>
                {
                    new object[] {surName, foreName, "Male", dateOfBirth, DateOfAdmission, pupilName, registerDate, endDate
                    , "SEN", "Year 2", "2", "4"}

                };
            return data;
        }

        public List<object[]> TC_PU044D_DATA()
        {
            string pattern = "M/d/yyyy";
            string surName = SeleniumHelper.GenerateRandomString(8);
            string foreName = SeleniumHelper.GenerateRandomString(8);
            string pupilName = String.Format("{0}, {1}", surName, foreName);
            string dateOfBirth = DateTime.ParseExact("10/16/2006", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            string DateOfAdmission = DateTime.ParseExact("10/19/2010", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            string startDate = SeleniumHelper.GetToDay();
            string endDate = SeleniumHelper.GetDayAfter(SeleniumHelper.GetToDay(), 1);

            var data = new List<Object[]>
                {
                    new object[] {surName, foreName, "Male", dateOfBirth, DateOfAdmission, "Year 2", "SEN", pupilName, "Baker, Jade", 
                        "New Suspension", "Substance Abuse", startDate, endDate, "8:30 AM", "4:45 PM", "1"}

                };
            return data;
        }


        public List<object[]> TC_PU062_Data()
        {
            string pattern = "M/d/yyyy";
            string PupilDateOfBirth = DateTime.ParseExact("06/01/2006", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            string PupilDateOfAdmission = DateTime.ParseExact("06/01/2010", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            string foreName = String.Format("{0}_{1}", SeleniumHelper.GenerateRandomString(7), SeleniumHelper.GenerateRandomNumberUsingDateTime());
            string surName = String.Format("{0}_{1}", "Avn", SeleniumHelper.GenerateRandomString(8));
            var res = new List<Object[]>
            {                
                new object[] 
                {                    
                    foreName, surName, "Male", PupilDateOfBirth, PupilDateOfAdmission, "Year 2", "LGG Test",
                }
                
            };
            return res;
        }

        public List<object[]> TC_PU063_Data()
        {
            string pattern = "M/d/yyyy";
            string PupilDateOfBirth = DateTime.ParseExact("06/01/2006", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            string PupilDateOfAdmission = DateTime.ParseExact("06/01/2010", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            string startDate = SeleniumHelper.GetFirstDayOfWeek(DateTime.Now.Subtract(TimeSpan.FromDays(7))).ToString(pattern);
            string foreName = String.Format("{0}_{1}", SeleniumHelper.GenerateRandomString(7), SeleniumHelper.GenerateRandomNumberUsingDateTime());
            string surName = String.Format("{0}_{1}", "Avn", SeleniumHelper.GenerateRandomString(8));
            var res = new List<Object[]>
            {                
                new object[] {foreName, surName, "Male", PupilDateOfBirth, PupilDateOfAdmission, "Year 2", 
                    "In-school provision", startDate},
            };
            return res;
        }

        public List<object[]> TC_PU064_Data()
        {
            string pattern = "M/d/yyyy";
            string PupilDateOfBirth = DateTime.ParseExact("06/01/2006", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            string PupilDateOfAdmission = DateTime.ParseExact("06/01/2010", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            string startDate = SeleniumHelper.GetFirstDayOfWeek(DateTime.Now.Subtract(TimeSpan.FromDays(7))).ToString(pattern);
            string foreName = String.Format("{0}_{1}", SeleniumHelper.GenerateRandomString(7), SeleniumHelper.GenerateRandomNumberUsingDateTime());
            string surName = String.Format("{0}_{1}", "Avn", SeleniumHelper.GenerateRandomString(8));
            var res = new List<Object[]>
            {                
                new object[] {foreName, surName, "Male", PupilDateOfBirth, PupilDateOfAdmission, "Year 2", 
                    "Severe Learning Difficulties", startDate, "SLD Confirmed by family GP"},
            };
            return res;
        }

        public List<object[]> TC_PU065_Data()
        {
            string pattern = "M/d/yyyy";
            string PupilDateOfBirth = DateTime.ParseExact("06/01/2006", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            string PupilDateOfAdmission = DateTime.ParseExact("06/01/2010", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            string startDate = SeleniumHelper.GetFirstDayOfWeek(DateTime.Now.Subtract(TimeSpan.FromDays(7))).ToString(pattern);
            string endDate = DateTime.Now.Add(TimeSpan.FromDays(365)).ToString(pattern);
            string foreName = String.Format("{0}_{1}", SeleniumHelper.GenerateRandomString(7), SeleniumHelper.GenerateRandomNumberUsingDateTime());
            string surName = String.Format("{0}_{1}", "Avn", SeleniumHelper.GenerateRandomString(8));
            var res = new List<Object[]>
            {                
                new object[] {foreName, surName, "Male", PupilDateOfBirth, PupilDateOfAdmission, "Year 2",
                    "Time in Specialist Unit", startDate, endDate, "Time in special unit approved by family GP"},
            };
            return res;
        }

        public List<object[]> TC_PU066a_Data()
        {
            string pattern = "M/d/yyyy";
            string PupilDateOfBirth = DateTime.ParseExact("06/01/2006", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            string PupilDateOfAdmission = DateTime.ParseExact("06/01/2010", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            string previousMonday = SeleniumHelper.GetFirstDayOfWeek(DateTime.Now.Subtract(TimeSpan.FromDays(7))).ToString(pattern);
            string startTime = DateTime.ParseExact("10:30", "h:mm", CultureInfo.InvariantCulture).ToString("h:mm tt");
            string foreName = String.Format("{0}_{1}", SeleniumHelper.GenerateRandomString(7), SeleniumHelper.GenerateRandomNumberUsingDateTime());
            string surName = String.Format("{0}_{1}", "Avn", SeleniumHelper.GenerateRandomString(8));
            int staff = SeleniumHelper.GenerateRandomNumber(15);
            var res = new List<Object[]>
            {                
                new object[] {foreName, surName, "Male", PupilDateOfBirth, PupilDateOfAdmission, "Year 2",
                        "In-school provision",
                       "Initial", "Meeting Completed", previousMonday, startTime, "Math Room 11",
                       staff, "Test", "Test", "SEN Co-ordinator", previousMonday},
            };
            return res;
        }

        public List<object[]> TC_PU066b_Data()
        {
            string pattern = "M/d/yyyy";
            string PupilDateOfBirth = DateTime.ParseExact("06/01/2006", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            string PupilDateOfAdmission = DateTime.ParseExact("06/01/2010", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            string previousMonday = SeleniumHelper.GetFirstDayOfWeek(DateTime.Now.Subtract(TimeSpan.FromDays(7))).ToString(pattern);
            string foreName = String.Format("{0}_{1}", SeleniumHelper.GenerateRandomString(7), SeleniumHelper.GenerateRandomNumberUsingDateTime());
            string surName = String.Format("{0}_{1}", "Avn", SeleniumHelper.GenerateRandomString(8));
            var res = new List<Object[]>
            {                
                new object[] {foreName, surName, "Male", PupilDateOfBirth, PupilDateOfAdmission, "Year 2",
                        "In-school provision",
                       previousMonday, previousMonday, 
                       "ELB Agreed", "Proposed Statement made",
                       previousMonday, previousMonday},
            };
            return res;
        }

        public List<object[]> TC_PU067_Data()
        {
            string pattern = "M/d/yyyy";
            string PupilDateOfBirth = DateTime.ParseExact("06/01/2006", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            string PupilDateOfAdmission = DateTime.ParseExact("06/01/2010", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            string past3Month = DateTime.Now.Subtract(TimeSpan.FromDays(90)).ToString(pattern);
            string past2Month = DateTime.Now.Subtract(TimeSpan.FromDays(60)).ToString(pattern);
            string foreName = String.Format("{0}_{1}", SeleniumHelper.GenerateRandomString(7), SeleniumHelper.GenerateRandomNumberUsingDateTime());
            string surName = String.Format("{0}_{1}", "Avn", SeleniumHelper.GenerateRandomString(8));
            var res = new List<Object[]>
            {
                new object[] {
                    foreName, surName, "Male", PupilDateOfBirth, PupilDateOfAdmission, "Year 2",
                    new string[]{"Charlie", "Sage", "River", "Deed Poll", past3Month},
                    new string[]{"Taylor", "Logan", "Kerry", "Adoption", past2Month},
                    new string[]{"Jessie", "Casey", "Harley", "Marriage", past2Month}
                },
            };
            return res;
        }

        public List<object[]> TC_PU068_Data()
        {

            string pattern = "M/d/yyyy";
            string PupilDateOfBirth = DateTime.ParseExact("06/01/2006", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            string PupilDateOfAdmission = DateTime.ParseExact("06/01/2010", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            DateTime twoYearPast = SeleniumHelper.GetFirstDayOfWeek(DateTime.Now.Subtract(TimeSpan.FromDays(730)));
            string endGuest = twoYearPast.Subtract(TimeSpan.FromDays(1)).ToString(pattern);
            string startSub = twoYearPast.ToString(pattern);
            string endSub = twoYearPast.Add(TimeSpan.FromDays(1)).ToString(pattern);
            string startMain = twoYearPast.Add(TimeSpan.FromDays(2)).ToString(pattern);
            string endMain = twoYearPast.Add(TimeSpan.FromDays(20)).ToString(pattern);
            string startSingle = twoYearPast.Add(TimeSpan.FromDays(21)).ToString(pattern);
            string foreName = String.Format("{0}_{1}", SeleniumHelper.GenerateRandomString(7), SeleniumHelper.GenerateRandomNumberUsingDateTime());
            string surName = String.Format("{0}_{1}", "Avn", SeleniumHelper.GenerateRandomString(8));
            var res = new List<Object[]>
            {                
                new object[] {
                    foreName, surName, "Male", PupilDateOfBirth, PupilDateOfAdmission, "Year 2",
                     endGuest, startSub, endSub, startMain, endMain, startSingle,
                }
            };
            return res;
        }

        public List<object[]> TC_PU069_Data()
        {
            var code = String.Format("{0}_{1}", SeleniumHelper.GenerateRandomString(4), SeleniumHelper.GenerateRandomNumber(999));
            var description = String.Format("{0}_{1}_{2}", "Description", SeleniumHelper.GenerateRandomString(10), SeleniumHelper.GenerateRandomNumberUsingDateTime());
            var displayOrder = SeleniumHelper.GenerateRandomNumberUsingDateTime();
            var res = new List<Object[]>
            {                
                new object[] {code, description, displayOrder, "Physical"},
            };
            return res;
        }

        public List<object[]> TC_PU070_Data()
        {
            var code = String.Format("{0}_{1}", SeleniumHelper.GenerateRandomString(4), SeleniumHelper.GenerateRandomNumber(999));
            var description = String.Format("{0}_{1}_{2}", "Description", SeleniumHelper.GenerateRandomString(10), SeleniumHelper.GenerateRandomNumberUsingDateTime());
            var displayOrder = SeleniumHelper.GenerateRandomNumberUsingDateTime();
            var res = new List<Object[]>
            {                
                new object[] {code, description, displayOrder},
            };
            return res;
        }

        public List<object[]> TC_PU071_Data()
        {
            var code = String.Format("{0}_{1}", "11", SeleniumHelper.GenerateRandomNumber(999));
            var description = String.Format("{0}_{1}_{2}", "Description", SeleniumHelper.GenerateRandomString(10), SeleniumHelper.GenerateRandomNumberUsingDateTime());
            var displayOrder = SeleniumHelper.GenerateRandomNumberUsingDateTime();
            var res = new List<Object[]>
            {                
                new object[] {code, description, displayOrder, "In-school provision"},
            };
            return res;
        }

        public List<object[]> TC_PU072_Data()
        {
            string pupilName = String.Format("PU072_{0}_{1}", SeleniumHelper.GenerateRandomString(10), SeleniumHelper.GenerateRandomNumberUsingDateTime());
            var res = new List<Object[]>
            {                
                new object[] {
                    new string[]{pupilName, pupilName,
                                "Female", DateTime.ParseExact("1/1/2000", "M/d/yyyy", CultureInfo.InvariantCulture).ToString("M/d/yyyy"), 
                                DateTime.ParseExact("10/10/2013", "M/d/yyyy", CultureInfo.InvariantCulture).ToString("M/d/yyyy"), "Guest Pupil", "Year 1"},
                    //SenStage name
                    "In-school provision",
                    //SenNeeds
                    new string[]{"Mild Learning Difficulties", "Dangerous Fluid Loss", DateTime.Today.ToString("M/d/yyyy"), (new DateTime(DateTime.Today.Year, 12, 25)).ToString("M/d/yyyy") },
                    //Provisions
                    new string[]{"IT Provision", (new DateTime(DateTime.Today.Year, 12, 25)).ToString("M/d/yyyy") },
                },
            };
            return res;
        }

        public List<object[]> TC_PU073_Data()
        {
            string pupilName = String.Format("PU073_{0}_{1}", SeleniumHelper.GenerateRandomString(10), SeleniumHelper.GenerateRandomNumberUsingDateTime());
            var res = new List<Object[]>
            {                
                new object[] {
                    new string[]{pupilName, pupilName,
                                "Female", DateTime.ParseExact("1/1/2000", "M/d/yyyy", CultureInfo.InvariantCulture).ToString("M/d/yyyy"), 
                                DateTime.ParseExact("10/10/2013", "M/d/yyyy", CultureInfo.InvariantCulture).ToString("M/d/yyyy"), "Guest Pupil", "Year 1"},
                    //SenStage name
                    "In-school provision",
                    //SenNeeds
                    new string[]{"Mild Learning Difficulties", "Dangerous Fluid Loss", DateTime.Today.ToString("M/d/yyyy"), (new DateTime(DateTime.Today.Year, 12, 25)).ToString("M/d/yyyy") },
                    //Provisions
                    new string[]{"IT Provision", (new DateTime(DateTime.Today.Year, 12, 25)).ToString("M/d/yyyy") },
                },
            };
            return res;
        }

        public List<object[]> TC_PU074_Data()
        {
            var res = new List<Object[]>
            {                
                new object[] {
                    "Year 1", "1A", "Gender", "Mode of Travel"
                },
            };
            return res;
        }

        public List<object[]> TC_PU075_Data()
        {
            var res = new List<Object[]>
            {                
                new object[] {
                    "Year 1", "1A",
                    //List of 'Mode Of Travel' items
                    new string[]{"Bicycle", "Car", "ELB Bus", "Ferry", "Public Road Transport", "School Coach", "Walks", "Taxi", "Train"}, 
                    //Type mode of travel
                    "ELB Bus"
                },
            };
            return res;
        }

        public List<object[]> TC_PU076_Data()
        {
            var res = new List<Object[]>
            {                
                new object[] {
                    "Year 1", "1A",
                    //List of 'Mode Of Travel' items
                    new string[]{"Bicycle", "Car", "ELB Bus", "Ferry", "Public Road Transport", "School Coach", "Walks", "Taxi", "Train"},
                    //Type mode of travel
                    "School Coach",
                    "ELB Bus"
                },
            };
            return res;
        }

        public List<object[]> TC_PU077_Data()
        {
            var res = new List<Object[]>
            {                
                new object[] {
                    "Year 1", "1A",
                    new string[]{"Service Children", "Yes"},
                    new string[]{"Service Children Source", "Provided by the parent"}
                },
            };
            return res;
        }

        public List<object[]> TC_PU078_Data()
        {
            string pupilName = String.Format("PU078_{0}_{1}", SeleniumHelper.GenerateRandomString(10), SeleniumHelper.GenerateRandomNumberUsingDateTime());
            var res = new List<Object[]>
            {                
                new object[] {
                    new string[]{pupilName, pupilName,
                                "Female", DateTime.ParseExact("1/1/2000", "M/d/yyyy", CultureInfo.InvariantCulture).ToString("M/d/yyyy"), 
                                DateTime.ParseExact("10/10/2013", "M/d/yyyy", CultureInfo.InvariantCulture).ToString("M/d/yyyy"), "Guest Pupil", "Year 1"},
                    SeleniumHelper.GetDateFromDayOfWeek((int)DayOfWeek.Friday), "Not Known", "Elective Home Education"
                },
            };
            return res;
        }

        public List<object[]> TC_PU079_Data()
        {
            string pupilName = String.Format("PU079_{0}_{1}", SeleniumHelper.GenerateRandomString(10), SeleniumHelper.GenerateRandomNumberUsingDateTime());

            var res = new List<Object[]>
            {           
                new object[] {
                    new string[]{pupilName, pupilName, 
                                "Female", DateTime.ParseExact("1/1/2000", "M/d/yyyy", CultureInfo.InvariantCulture).ToString("M/d/yyyy"), 
                                DateTime.ParseExact("10/10/2013", "M/d/yyyy", CultureInfo.InvariantCulture).ToString("M/d/yyyy"), "Guest Pupil", "Year 1"}, 
                    new string[]{DateTime.ParseExact("12/12/2013", "M/d/yyyy", CultureInfo.InvariantCulture).ToString("M/d/yyyy"), "Not Known", "Elective Home Education"},
                    new string[]{DateTime.ParseExact("2/12/2014", "M/d/yyyy", CultureInfo.InvariantCulture).ToString("M/d/yyyy"), "Subsidiary – Dual Registration", "Year 1"},
                    new string[]{DateTime.ParseExact("4/30/2014", "M/d/yyyy", CultureInfo.InvariantCulture).ToString("M/d/yyyy"), "Not Known", "Elective Home Education"},
                    new string[]{DateTime.ParseExact("5/8/2014", "M/d/yyyy", CultureInfo.InvariantCulture).ToString("M/d/yyyy"), "Main – Dual Registration", "Year 1"},
                    new string[]{DateTime.ParseExact("2/01/2015", "M/d/yyyy", CultureInfo.InvariantCulture).ToString("M/d/yyyy"), "Single Registration", 
                                 DateTime.ParseExact("2/02/2015", "M/d/yyyy", CultureInfo.InvariantCulture).ToString("M/d/yyyy")}
                },
            };
            return res;
        }

        public List<object[]> TC_PU081_Data()
        {
            string pupilName = String.Format("PU081_{0}_{1}", SeleniumHelper.GenerateRandomString(10), SeleniumHelper.GenerateRandomNumberUsingDateTime());
            var res = new List<Object[]>
            {                
                new object[] {
                    new string[]{pupilName, pupilName, 
                                "Female", DateTime.ParseExact("1/1/2000", "M/d/yyyy", CultureInfo.InvariantCulture).ToString("M/d/yyyy"), 
                                DateTime.ParseExact("10/10/2013", "M/d/yyyy", CultureInfo.InvariantCulture).ToString("M/d/yyyy"), "Guest Pupil", "Year 1"},
                    new string[]{"Family", "Family Documents", "document.txt"}, 
                    new string[]{"Welfare", "Welfare Documents", "document_1.txt"},
                    new string[]{"Consent", "Consent Documents", "document_2.txt"}
                },
            };
            return res;
        }

        public List<object[]> TC_PU087_Data()
        {
            var res = new List<Object[]>
            {                
                new object[] {
                    "3A"
                },
            };
            return res;
        }

        public List<object[]> TC_PU088_Data()
        {
            var res = new List<Object[]>
            {                
                new object[] {
                    "Year 1"
                },
            };
            return res;
        }

        public List<object[]> TC_PU089_Data()
        {
            var res = new List<Object[]>
            {                
                new object[] {
                    "1A", "Year 1"
                },
            };
            return res;
        }

        public List<object[]> TC_PU090_Data()
        {
            var res = new List<Object[]>
            {                
                new object[] {
                    "2A", "Year 2"
                },
            };
            return res;
        }
        

        #endregion


    }
}
