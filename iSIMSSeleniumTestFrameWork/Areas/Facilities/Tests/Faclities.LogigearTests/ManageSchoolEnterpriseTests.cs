using NUnit.Framework;
using POM.Components.Common;
using POM.Components.Admission;
using POM.Components.HomePages;
using POM.Components.Pupil;
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
using POM.Components.Attendance;
using Selene.Support.Attributes;
using SeSugar.Automation;
using SeSugar.Data;
using Facilities.Data;

namespace Faclities.LogigearTests
{
    class ManageSchoolEnterpriseTests
    {

        /// <summary>
        /// Author: Hieu Pham
        /// Description: As School Administrator : Add details of a new school so that it is available for selection at the local school only.
        /// Status: PASS
        /// </summary>
        [WebDriverTest(TimeoutSeconds = 1200, Enabled = true, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { "g1" }, DataProvider = "TC_MSC01_Data")]
        public void TC_MSC01_Add_Detail_Of_New_School(string[] basicDetails, string[] addressDetails)
        {

            #region PRE-CONDITIONS

            // Login as School Adminstrator
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);
            Wait.WaitForDocumentReady();
            // Navigate to Tasks->School Management -> Other School Details
            AutomationSugar.NavigateMenu("Tasks", "School Management", "Other School Details");
            Wait.WaitForDocumentReady();

            // Search record if existed
            var otherSchoolDetailTriplet = new OtherSchoolDetailTriplet();
            otherSchoolDetailTriplet.SearchCriteria.SchoolName = basicDetails[0];
            var searchResults = otherSchoolDetailTriplet.SearchCriteria.Search().FirstOrDefault(x => x.SchoolName.Equals(basicDetails[0]));
            var otherSchoolDetailPage = searchResults == null ? otherSchoolDetailTriplet.Create() : searchResults.Click<OtherSchoolDetailPage>();

            // Delete if exist
            otherSchoolDetailPage.Delete();

            // Click create after record is deleted
            otherSchoolDetailTriplet.Refresh();
            otherSchoolDetailTriplet.Create();

            #endregion

            #region STEPS

            // Input the basic details
            otherSchoolDetailPage.Refresh();
            otherSchoolDetailPage.Name = basicDetails[0];
            otherSchoolDetailPage.HeadTeacher = basicDetails[1];

            // Scroll to School Address then click Add New button
            otherSchoolDetailPage.ScrollToSchoolAddress();
            var addressDialogPage = otherSchoolDetailPage.AddNewAddress();

            // Add new address
            addressDialogPage.BuildingNo = addressDetails[0];
            addressDialogPage.BuildingName = addressDetails[1];
            addressDialogPage.PostCode = addressDetails[2];
            addressDialogPage.CountryPostCode = addressDetails[3];

            // Click Ok button
            var addAddressDialogTriplet = new AddNewAddressTriplet();
            addAddressDialogTriplet.ClickOk();
            // Click Save button.
            otherSchoolDetailPage.Refresh();
            otherSchoolDetailPage.Save();
            // VP : Success message displays
            //Assert.AreEqual(true, otherSchoolDetailPage.IsSuccessMessageDisplay(), "Success message doesn't display");
            // Search and select new record
            otherSchoolDetailTriplet = new OtherSchoolDetailTriplet();
            otherSchoolDetailTriplet.SearchCriteria.SchoolName = basicDetails[0];
            var groupTile = otherSchoolDetailTriplet.SearchCriteria.Search().FirstOrDefault(x => x.SchoolName.Equals(basicDetails[0]));
            otherSchoolDetailPage = groupTile == null ? null : groupTile.Click<OtherSchoolDetailPage>();
            // VP : Verify Other School is created successfully.
            Assert.AreNotEqual(null, groupTile, "There are no result when searching with Group Name value");
            Assert.AreEqual(basicDetails[0], otherSchoolDetailPage.Name, "School Name is not correct");

            // Scroll to Address
           // otherSchoolDetailPage.ScrollToSchoolAddress();
         //   Assert.AreEqual(String.Format("{0} {1} {2} {3}", addressDetails[0], addressDetails[1], addressDetails[2], addressDetails[3]), otherSchoolDetailPage.Address, "Address is not correct");

            #endregion

            #region POST-CONDITION

            // Click Delete button
            otherSchoolDetailTriplet.Delete();
          //  otherSchoolDetailPage.Delete();

            #endregion

        }

        /// <summary>
        /// Author: Hieu Pham
        /// Description: As School Administrator : Check New Created 'Other' School is not available to schools other that the one its was added on
        /// Status: FAILED BY BUG
        /// </summary>
        [WebDriverTest(TimeoutSeconds = 1800, Enabled = false, Browsers = new[] { BrowserDefaults.Ie }, Groups = new[] { "g1" }, DataProvider = "TC_MSC02_Data")]
        public void TC_MSC02_Check_New_Created_Other_School_Is_Not_Available_To_School_Other(string[] basicDetails, string pupilName)
        {

            #region PRE-CONDITIONS

            // Login as School Adminstrator
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);

            // Navigate to Tasks->School Management -> Other School Details
            AutomationSugar.NavigateMenu("Tasks", "School Management", "Other School Details");

            // Search record if existed
            var otherSchoolDetailTriplet = new OtherSchoolDetailTriplet();
            otherSchoolDetailTriplet.SearchCriteria.SchoolName = basicDetails[0];
            var searchResults = otherSchoolDetailTriplet.SearchCriteria.Search().FirstOrDefault(x => x.SchoolName.Equals(basicDetails[0]));
            var otherSchoolDetailPage = searchResults == null ? otherSchoolDetailTriplet.Create() : searchResults.Click<OtherSchoolDetailPage>();

            // Delete if exist
            otherSchoolDetailPage.Delete();

            // Click create after record is deleted
            otherSchoolDetailTriplet.Refresh();
            otherSchoolDetailTriplet.Create();

            // Input the basic details
            otherSchoolDetailPage.Refresh();
            otherSchoolDetailPage.Name = basicDetails[0];
            otherSchoolDetailPage.HeadTeacher = basicDetails[1];

            // Click Save button.
            otherSchoolDetailPage.Refresh();
            otherSchoolDetailPage.Save();

            // VP : Success message displays
            Assert.AreEqual(true, otherSchoolDetailPage.IsSuccessMessageDisplay(), "Success message doesn't display");

            #endregion

            #region STEPS

            // Navigate to Pupil Record
            SeleniumHelper.NavigateQuickLink("Pupil Records");

            // Search and select a pupil
            var pupilRecordTriplet = new PupilRecordTriplet();
            pupilRecordTriplet.SearchCriteria.PupilName = pupilName;
            pupilRecordTriplet.SearchCriteria.IsCurrent = true;
            pupilRecordTriplet.SearchCriteria.IsLeaver = true;
            var pupilRecordPage = pupilRecordTriplet.SearchCriteria.Search().FirstOrDefault(x => x.Name.Equals(pupilName)).Click<PupilRecordPage>();

            // Scroll to School History
            pupilRecordPage.SelectSchoolHistoryTab();

            // Click Add School
            var selectSchoolHistoryDialog = pupilRecordPage.OpenSchoolHistoryDialog();
            var selectSchoolDialogTriplet = selectSchoolHistoryDialog.select();
            selectSchoolDialogTriplet.SearchCriteria.SchoolName = basicDetails[0];
            var searchResult = selectSchoolDialogTriplet.SearchCriteria.Search().FirstOrDefault(x => x.SchoolName.Equals(basicDetails[0]));

            // VP : Verify Search result is empty.
            Assert.AreEqual(null, searchResult, "Search result is not empty.");

            #endregion

            #region POST-CONDITION

            // Close dialog
            selectSchoolDialogTriplet.ClickCancel();

            // Navigate to Tasks->School Management -> Other School Details
            AutomationSugar.NavigateMenu("Tasks", "School Management", "Other School Details");

            // Search record if existed
            otherSchoolDetailTriplet.Refresh();
            otherSchoolDetailTriplet.SearchCriteria.SchoolName = basicDetails[0];
            searchResults = otherSchoolDetailTriplet.SearchCriteria.Search().FirstOrDefault(x => x.SchoolName.Equals(basicDetails[0]));
            otherSchoolDetailPage = searchResults.Click<OtherSchoolDetailPage>();

            // Click Delete button
            otherSchoolDetailPage.Delete();

            #endregion

        }

        /// <summary>
        /// Author: Hieu Pham
        /// Description: As School Administrator : Check that the locally-created school created previously can be selected on a pupil record as a 'previously attended school' by a School Administrator user. 
        /// Status: PASS
        /// </summary>
        [WebDriverTest(TimeoutSeconds = 1800, Enabled = false, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { "g1" }, DataProvider = "TC_MSC03_Data")]
        public void TC_MSC03_Check_That_Locally_Created_School_Created_Previously_Can_Be_Selected(string[] basicDetails, string pupilName, string academicYear)
        {
            //Pupil Data
            string forename = "SelPersonal" + SeleniumHelper.GenerateRandomString(5) + SeleniumHelper.GenerateRandomNumberUsingDateTime();
            string surname = "SelPersonal" + SeleniumHelper.GenerateRandomString(5) + SeleniumHelper.GenerateRandomNumberUsingDateTime();
            DateTime dob = new DateTime(2011, 02, 02);

            var learnerId = Guid.NewGuid();
            var pupil = this.BuildDataPackage()
                        .AddBasicLearner(learnerId, surname, forename, dob, dateOfAdmission: new DateTime(2011, 10, 05));

            using (new DataSetup(false, true, pupil))
            {
                #region PRE-CONDITIONS

                // Login as School Adminstrator
                SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);
                Wait.WaitForDocumentReady();
                // Navigate to Tasks->School Management -> Other School Details
                AutomationSugar.NavigateMenu("Tasks", "School Management", "Other School Details");
                Wait.WaitForDocumentReady();

                // Search record if existed
                var otherSchoolDetailTriplet = new OtherSchoolDetailTriplet();
                otherSchoolDetailTriplet.SearchCriteria.SchoolName = basicDetails[0];
                var searchResults = otherSchoolDetailTriplet.SearchCriteria.Search().FirstOrDefault(x => x.SchoolName.Equals(basicDetails[0]));
                var otherSchoolDetailPage = searchResults == null ? otherSchoolDetailTriplet.Create() : searchResults.Click<OtherSchoolDetailPage>();

                // Delete if exist
                otherSchoolDetailPage.Delete();

                // Click create after record is deleted
                otherSchoolDetailTriplet.Refresh();
                otherSchoolDetailTriplet.Create();

                // Input the basic details
                otherSchoolDetailPage.Refresh();
                otherSchoolDetailPage.Name = basicDetails[0];
                otherSchoolDetailPage.HeadTeacher = basicDetails[1];

                // Click Save button.
                otherSchoolDetailPage.Refresh();
                otherSchoolDetailPage.Save();
                Wait.WaitLoading();

                // VP : Success message displays
                //Assert.AreEqual(true, otherSchoolDetailPage.IsSuccessMessageDisplay(), "Success message doesn't display");

                #endregion

                #region STEPS

                // Navigate to Pupil Record
                SeleniumHelper.NavigateQuickLink("Pupil Records");

                // Search and select a pupil
                var pupilRecordTriplet = new PupilRecordTriplet();
                pupilRecordTriplet.SearchCriteria.PupilName = String.Format("{0}, {1}", surname, forename);
                pupilRecordTriplet.SearchCriteria.IsCurrent = true;
                pupilRecordTriplet.SearchCriteria.IsLeaver = true;
                var pupilRecordPage = pupilRecordTriplet.SearchCriteria.Search().FirstOrDefault(x => x.Name.Equals(String.Format("{0}, {1}", surname, forename))).Click<PupilRecordPage>();

                // Scroll to School History
                pupilRecordPage.SelectSchoolHistoryTab();

                // Click Add School
                var selectSchoolHistoryDialog = pupilRecordPage.OpenSchoolHistoryDialog();
                var selectSchoolDialogTriplet = selectSchoolHistoryDialog.select();
                selectSchoolDialogTriplet.SearchCriteria.SchoolName = basicDetails[0];
                var searchResult = selectSchoolDialogTriplet.SearchCriteria.Search().FirstOrDefault(x => x.SchoolName.Equals(basicDetails[0]));

                // Select school
                searchResult.Click();

                // Click OK button
                selectSchoolDialogTriplet.Refresh();
                selectSchoolDialogTriplet.ClickOk();

                // Open Attendance Summary dialog
                var schoolHistoryTable = pupilRecordPage.LearnerPreviousSchools;
                var row = schoolHistoryTable.Rows.FirstOrDefault(x => x.SchoolName.Equals(basicDetails[0]));
                var attendanceSummaryDialog = row.Edit();

                // Enter detail for attendance summary dialog
                var attendanceTable = attendanceSummaryDialog.PreviousSchoolAttendanceSummary;
                attendanceSummaryDialog.ClickAddAttendanceSummaryLink();
                var rowIndex = attendanceTable.Rows.Count;
                //  var attendanceRow = attendanceTable.Rows.FirstOrDefault(x => x.Year.Equals(String.Empty));
                attendanceTable[rowIndex].Year = academicYear;
                Wait.WaitLoading();

                // Click OK 
                attendanceSummaryDialog.ClickOk();

                // Save value
                pupilRecordPage.Refresh();
                pupilRecordPage.SavePupil();

                // Search record again
                pupilRecordTriplet.Refresh();
                pupilRecordTriplet.SearchCriteria.PupilName = String.Format("{0}, {1}", surname, forename);
                pupilRecordTriplet.SearchCriteria.IsCurrent = true;
                pupilRecordTriplet.SearchCriteria.IsLeaver = true;
                pupilRecordPage = pupilRecordTriplet.SearchCriteria.Search().FirstOrDefault(x => x.Name.Equals(String.Format("{0}, {1}", surname, forename))).Click<PupilRecordPage>();

                // Scroll to School History
                pupilRecordPage.SelectSchoolHistoryTab();

                // VP : Locally-created school to be selected and saved on pupil record
                schoolHistoryTable = pupilRecordPage.LearnerPreviousSchools;
                row = schoolHistoryTable.Rows.FirstOrDefault(x => x.SchoolName.Equals(basicDetails[0]));
                Assert.AreNotEqual(null, row, "Previous school has not been saved");

                // Open Attendance Summary
                attendanceSummaryDialog = row.Edit();
                attendanceTable = attendanceSummaryDialog.PreviousSchoolAttendanceSummary;
                var attendanceRow = attendanceTable.Rows.FirstOrDefault(x => x.Year.Equals(academicYear));

                Assert.AreNotEqual(null, attendanceRow, "Save Attendance Summary is unsuccess");

                #endregion

                #region POST-CONDITION

                // Close dialog
                attendanceSummaryDialog.ClickCancel();

                // Delete previous school
                schoolHistoryTable.Refresh();
                row = schoolHistoryTable.Rows.FirstOrDefault(x => x.SchoolName.Equals(basicDetails[0]));
                row.DeleteRow();

                // Save value
                pupilRecordPage.Refresh();
                pupilRecordPage.SavePupil();
                               
                // Navigate to Tasks->School Management -> Other School Details
                AutomationSugar.NavigateMenu("Tasks", "School Management", "Other School Details");
                Wait.WaitForDocumentReady();
                // Search record if existed
                otherSchoolDetailTriplet = new OtherSchoolDetailTriplet();
                otherSchoolDetailTriplet.SearchCriteria.SchoolName = basicDetails[0];
                searchResults = otherSchoolDetailTriplet.SearchCriteria.Search().FirstOrDefault(x => x.SchoolName.Equals(basicDetails[0]));
                otherSchoolDetailPage = searchResults.Click<OtherSchoolDetailPage>();

                // Click Delete button
                otherSchoolDetailPage.Delete();

                #endregion
            }
        }

        /// <summary>
        /// Author: Hieu Pham
        /// Description: As School Administrator : Edit details of locally-created school created previously
        /// Status: PASS
        /// </summary>
        [WebDriverTest(TimeoutSeconds = 1800, Enabled = false, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { "g1" }, DataProvider = "TC_MSC04_Data")]
        public void TC_MSC04_Edit_Detail_Locally_Created_School_Created_Previously(string[] basicDetails, string[] addressDetails, string[] basicDetailsAmended, string[] addressDetailsAmended,
            string[] contactDetailsAmended, string pupilName)
        {
            //Pupil Data
            string forename = "SelPersonal" + SeleniumHelper.GenerateRandomString(5) + SeleniumHelper.GenerateRandomNumberUsingDateTime();
            string surname = "SelPersonal" + SeleniumHelper.GenerateRandomString(5) + SeleniumHelper.GenerateRandomNumberUsingDateTime();
            DateTime dob = new DateTime(2011, 02, 02);

            var learnerId = Guid.NewGuid();
            var pupil = this.BuildDataPackage()
                        .AddBasicLearner(learnerId, surname, forename, dob, dateOfAdmission: new DateTime(2011, 10, 05));

            using (new DataSetup(false, true, pupil))
            {

                #region PRE-CONDITIONS

                // Login as School Adminstrator
                SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);
                Wait.WaitForDocumentReady();
                // Navigate to Tasks->School Management -> Other School Details
                AutomationSugar.NavigateMenu("Tasks", "School Management", "Other School Details");
                Wait.WaitForDocumentReady();
                // Search record if existed
                var otherSchoolDetailTriplet = new OtherSchoolDetailTriplet();
                otherSchoolDetailTriplet.SearchCriteria.SchoolName = basicDetails[0];
                var searchResults = otherSchoolDetailTriplet.SearchCriteria.Search().FirstOrDefault(x => x.SchoolName.Equals(basicDetails[0]));
                var otherSchoolDetailPage = searchResults == null ? otherSchoolDetailTriplet.Create() : searchResults.Click<OtherSchoolDetailPage>();

                // Delete if exist
                otherSchoolDetailPage.Delete();

                // Click create after record is deleted
                otherSchoolDetailTriplet.Refresh();
                otherSchoolDetailTriplet.Create();

                // Input the basic details
                otherSchoolDetailPage.Refresh();
                otherSchoolDetailPage.Name = basicDetails[0];
                otherSchoolDetailPage.HeadTeacher = basicDetails[1];

                // Scroll to School Address then click Add New button
                otherSchoolDetailPage.ScrollToSchoolAddress();
                var addressDialogPage = otherSchoolDetailPage.AddNewAddress();

                // Add new address
                addressDialogPage.BuildingNo = addressDetails[0];
                addressDialogPage.BuildingName = addressDetails[1];
                addressDialogPage.PostCode = addressDetails[2];
                addressDialogPage.CountryPostCode = addressDetails[3];

                // Click Ok button
                var addAddressDialogTriplet = new AddNewAddressTriplet();
                addAddressDialogTriplet.ClickOk();

                // Click Save button.
                otherSchoolDetailPage.Refresh();
                otherSchoolDetailPage.Save();
                Wait.WaitLoading();

                // VP : Success message displays
                //Assert.AreEqual(true, otherSchoolDetailPage.IsSuccessMessageDisplay(), "Success message doesn't display");

                // Navigate to Pupil Record
                SeleniumHelper.NavigateQuickLink("Pupil Records");
                
                // Search and select a pupil
                var pupilRecordTriplet = new PupilRecordTriplet();
                pupilRecordTriplet.SearchCriteria.PupilName = String.Format("{0}, {1}", surname, forename);
                pupilRecordTriplet.SearchCriteria.IsCurrent = true;
                pupilRecordTriplet.SearchCriteria.IsLeaver = true;
                var pupilRecordPage = pupilRecordTriplet.SearchCriteria.Search().FirstOrDefault(x => x.Name.Equals(String.Format("{0}, {1}", surname, forename))).Click<PupilRecordPage>();

                // Scroll to School History
                pupilRecordPage.SelectSchoolHistoryTab();

                // Click Add School
                var selectSchoolHistoryDialog = pupilRecordPage.OpenSchoolHistoryDialog();
                var selectSchoolDialogTriplet = selectSchoolHistoryDialog.select();
                selectSchoolDialogTriplet.SearchCriteria.SchoolName = basicDetails[0];
                var searchResult = selectSchoolDialogTriplet.SearchCriteria.Search().FirstOrDefault(x => x.SchoolName.Equals(basicDetails[0]));

                // Select school
                searchResult.Click();

                // Click OK button
                selectSchoolDialogTriplet.Refresh();
                selectSchoolDialogTriplet.ClickOk();

                // Save value
                pupilRecordPage.Refresh();
                pupilRecordPage.SavePupil();

                #endregion

                #region STEPS

                // Navigate to Tasks->School Management -> Other School Details
                AutomationSugar.NavigateMenu("Tasks", "School Management", "Other School Details");
                Wait.WaitForDocumentReady();
                // Search record again
                otherSchoolDetailTriplet = new OtherSchoolDetailTriplet();
                otherSchoolDetailTriplet.SearchCriteria.SchoolName = basicDetails[0];
                searchResults = otherSchoolDetailTriplet.SearchCriteria.Search().FirstOrDefault(x => x.SchoolName.Equals(basicDetails[0]));
                otherSchoolDetailPage = searchResults.Click<OtherSchoolDetailPage>();

                // Amend basic detail
                otherSchoolDetailPage.Name = basicDetailsAmended[0];
                otherSchoolDetailPage.HeadTeacher = basicDetailsAmended[1];

                // Scroll to School Address then click Edit button
                otherSchoolDetailPage.ScrollToSchoolAddress();
                var editaddressDialogPage = otherSchoolDetailPage.EditAddress();

                // Amend Address
                editaddressDialogPage.BuildingNo = addressDetailsAmended[0];
                editaddressDialogPage.BuildingName = addressDetailsAmended[1];
                editaddressDialogPage.PostCode = addressDetailsAmended[2];
                editaddressDialogPage.CountryPostCode = addressDetailsAmended[3];

                // Click OK
                editaddressDialogPage.ClickOk();

                // Scroll to contact detail
                otherSchoolDetailPage.Refresh();
                otherSchoolDetailPage.ScrollToContactDetail();
                otherSchoolDetailPage.TelephoneNumber = contactDetailsAmended[0];
                otherSchoolDetailPage.FaxNumber = contactDetailsAmended[1];
                otherSchoolDetailPage.EmailAddress = contactDetailsAmended[2];
                otherSchoolDetailPage.WebsiteAddess = contactDetailsAmended[3];

                // Save values 
                otherSchoolDetailPage.Save();

                // Search record again
                otherSchoolDetailTriplet = new OtherSchoolDetailTriplet();
                otherSchoolDetailTriplet.SearchCriteria.SchoolName = basicDetailsAmended[0];
                searchResults = otherSchoolDetailTriplet.SearchCriteria.Search().FirstOrDefault(x => x.SchoolName.Equals(basicDetailsAmended[0]));
                otherSchoolDetailPage = searchResults.Click<OtherSchoolDetailPage>();

                // VP : Basic detail is updated.
                Assert.AreEqual(true, otherSchoolDetailPage.Name.Equals(basicDetailsAmended[0]) && otherSchoolDetailPage.HeadTeacher.Equals(basicDetailsAmended[1]), "Basic detail is not correct.");

                // VP : Address is updated
                otherSchoolDetailPage.ScrollToSchoolAddress();
                Assert.AreEqual(String.Format("{0} {1} {2} {3}", addressDetailsAmended[0], addressDetailsAmended[1], addressDetailsAmended[2], addressDetailsAmended[3]), otherSchoolDetailPage.Address, "Address is not correct");

                // VP : Contact detail is updated
                otherSchoolDetailPage.ScrollToContactDetail();
                Assert.AreEqual(false, otherSchoolDetailPage.TelephoneNumber.Equals(contactDetailsAmended[0]) && otherSchoolDetailPage.FaxNumber.Equals(contactDetailsAmended[1])
                    && otherSchoolDetailPage.EmailAddress.Equals(contactDetailsAmended[2]) && otherSchoolDetailPage.WebsiteAddess.Equals(contactDetailsAmended[3]), "Contact Detail is not correct. ");

                // Navigate to Pupil Record
                SeleniumHelper.NavigateQuickLink("Pupil Records");

                // Search and select a pupil
                pupilRecordTriplet = new PupilRecordTriplet();
                pupilRecordTriplet.SearchCriteria.PupilName = String.Format("{0}, {1}", surname, forename);
                pupilRecordTriplet.SearchCriteria.IsCurrent = true;
                pupilRecordTriplet.SearchCriteria.IsLeaver = true;
                pupilRecordPage = pupilRecordTriplet.SearchCriteria.Search().FirstOrDefault(x => x.Name.Equals(String.Format("{0}, {1}", surname, forename))).Click<PupilRecordPage>();

                // Scroll to School History
                pupilRecordPage.SelectSchoolHistoryTab();

                // VP : School History table is updated.
                var schoolHistoryTable = pupilRecordPage.LearnerPreviousSchools;
                var row = schoolHistoryTable.Rows.FirstOrDefault(x => x.SchoolName.Equals(basicDetailsAmended[0]));
                Assert.AreNotEqual(null, row, "School history table is not updated");

                #endregion

                #region POST-CONDITION

                // Delete previous school
                schoolHistoryTable.Refresh();
                row = schoolHistoryTable.Rows.FirstOrDefault(x => x.SchoolName.Equals(basicDetailsAmended[0]));
                row.DeleteRow();

                // Save value
                pupilRecordPage.Refresh();
                pupilRecordPage.SavePupil();
                               
                // Navigate to Tasks->School Management -> Other School Details
                AutomationSugar.NavigateMenu("Tasks", "School Management", "Other School Details");

                // Search record if existed
                otherSchoolDetailTriplet = new OtherSchoolDetailTriplet();
                otherSchoolDetailTriplet.SearchCriteria.SchoolName = basicDetailsAmended[0];
                searchResults = otherSchoolDetailTriplet.SearchCriteria.Search().FirstOrDefault(x => x.SchoolName.Equals(basicDetailsAmended[0]));
                otherSchoolDetailPage = searchResults.Click<OtherSchoolDetailPage>();

                // Click Delete button
                otherSchoolDetailPage.Delete();

                #endregion
            }
        }

        /// <summary>
        /// Author: Hieu Pham
        /// Description: As School Administrator : Select to delete the locally-created school created previously.
        /// Status: PASS
        /// </summary>
        [WebDriverTest(TimeoutSeconds = 1800, Enabled = false, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { "g1" }, DataProvider = "TC_MSC05_Data")]
        public void TC_MSC05_Select_To_Delete_The_Locally_Created_School_Created_Previously(string[] basicDetails, string pupilName)
        {
            //Pupil Data
            string forename = "SelPersonal" + SeleniumHelper.GenerateRandomString(5) + SeleniumHelper.GenerateRandomNumberUsingDateTime();
            string surname = "SelPersonal" + SeleniumHelper.GenerateRandomString(5) + SeleniumHelper.GenerateRandomNumberUsingDateTime();
            DateTime dob = new DateTime(2011, 02, 02);

            var learnerId = Guid.NewGuid();
            var pupil = this.BuildDataPackage()
                        .AddBasicLearner(learnerId, surname, forename, dob, dateOfAdmission: new DateTime(2011, 10, 05));

            using (new DataSetup(false, true, pupil))
            {
                #region PRE-CONDITIONS

                // Login as School Adminstrator
                SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);
                Wait.WaitForDocumentReady();

                // Navigate to Tasks->School Management -> Other School Details
                AutomationSugar.NavigateMenu("Tasks", "School Management", "Other School Details");
                Wait.WaitForDocumentReady();

                // Search record if existed
                var otherSchoolDetailTriplet = new OtherSchoolDetailTriplet();
                otherSchoolDetailTriplet.SearchCriteria.SchoolName = basicDetails[0];
                var searchResults = otherSchoolDetailTriplet.SearchCriteria.Search().FirstOrDefault(x => x.SchoolName.Equals(basicDetails[0]));
                var otherSchoolDetailPage = searchResults == null ? otherSchoolDetailTriplet.Create() : searchResults.Click<OtherSchoolDetailPage>();

                // Delete if exist
                otherSchoolDetailPage.Delete();

                // Click create after record is deleted
                otherSchoolDetailTriplet.Refresh();
                otherSchoolDetailTriplet.Create();

                // Input the basic details
                otherSchoolDetailPage.Refresh();
                otherSchoolDetailPage.Name = basicDetails[0];
                otherSchoolDetailPage.HeadTeacher = basicDetails[1];

                // Click Save button.
                otherSchoolDetailPage.Save();
                Wait.WaitLoading();

                // VP : Success message displays
                //Assert.AreEqual(true, otherSchoolDetailPage.IsSuccessMessageDisplay(), "Success message doesn't display");

                // Navigate to Pupil Record
                SeleniumHelper.NavigateQuickLink("Pupil Records");

                // Search and select a pupil
                var pupilRecordTriplet = new PupilRecordTriplet();
                pupilRecordTriplet.SearchCriteria.PupilName = String.Format("{0}, {1}", surname, forename);
                pupilRecordTriplet.SearchCriteria.IsCurrent = true;
                pupilRecordTriplet.SearchCriteria.IsLeaver = true;
                var pupilRecordPage = pupilRecordTriplet.SearchCriteria.Search().FirstOrDefault(x => x.Name.Equals(String.Format("{0}, {1}", surname, forename))).Click<PupilRecordPage>();

                // Scroll to School History
                pupilRecordPage.SelectSchoolHistoryTab();

                // Click Add School
                var selectSchoolHistoryDialog = pupilRecordPage.OpenSchoolHistoryDialog();
                var selectSchoolDialogTriplet = selectSchoolHistoryDialog.select();
                selectSchoolDialogTriplet.SearchCriteria.SchoolName = basicDetails[0];
                var searchResult = selectSchoolDialogTriplet.SearchCriteria.Search().FirstOrDefault(x => x.SchoolName.Equals(basicDetails[0]));

                // Select school
                searchResult.Click();

                // Click OK button
                selectSchoolDialogTriplet.Refresh();
                selectSchoolDialogTriplet.ClickOk();

                // Save value
                pupilRecordPage.Refresh();
                pupilRecordPage.SavePupil();

                #endregion

                #region STEPS

                // Navigate to Tasks->School Management -> Other School Details
                AutomationSugar.NavigateMenu("Tasks", "School Management", "Other School Details");
                Wait.WaitForDocumentReady();

                // Search record again
                otherSchoolDetailTriplet = new OtherSchoolDetailTriplet();
                otherSchoolDetailTriplet.SearchCriteria.SchoolName = basicDetails[0];
                searchResults = otherSchoolDetailTriplet.SearchCriteria.Search().FirstOrDefault(x => x.SchoolName.Equals(basicDetails[0]));
                otherSchoolDetailPage = searchResults.Click<OtherSchoolDetailPage>();

                // Click delete button
                otherSchoolDetailPage.Delete();

                // VP : Locally-created school should not be deletable
                otherSchoolDetailPage.IsWarningMessageDisplay();

                #endregion

                #region POST-CONDITION

                // Navigate to Pupil Record
                SeleniumHelper.NavigateQuickLink("Pupil Records");

                // Search and select a pupil
                pupilRecordTriplet = new PupilRecordTriplet();
                pupilRecordTriplet.SearchCriteria.PupilName = String.Format("{0}, {1}", surname, forename);
                pupilRecordTriplet.SearchCriteria.IsCurrent = true;
                pupilRecordTriplet.SearchCriteria.IsLeaver = true;
                pupilRecordPage = pupilRecordTriplet.SearchCriteria.Search().FirstOrDefault(x => x.Name.Equals(String.Format("{0}, {1}", surname, forename))).Click<PupilRecordPage>();

                // Scroll to School History
                pupilRecordPage.SelectSchoolHistoryTab();

                // Delete previous school
                var schoolHistoryTable = pupilRecordPage.LearnerPreviousSchools;
                var row = schoolHistoryTable.Rows.FirstOrDefault(x => x.SchoolName.Equals(basicDetails[0]));
                row.DeleteRow();

                // Save value
                pupilRecordPage.Refresh();
                pupilRecordPage.SavePupil();

                // Navigate to Tasks->School Management -> Other School Details
                AutomationSugar.NavigateMenu("Tasks", "School Management", "Other School Details");

                // Search record if existed
                otherSchoolDetailTriplet = new OtherSchoolDetailTriplet();
                otherSchoolDetailTriplet.SearchCriteria.SchoolName = basicDetails[0];
                searchResults = otherSchoolDetailTriplet.SearchCriteria.Search().FirstOrDefault(x => x.SchoolName.Equals(basicDetails[0]));
                otherSchoolDetailPage = searchResults.Click<OtherSchoolDetailPage>();

                // Click Delete button
                otherSchoolDetailPage.Delete();

                #endregion
            }
        }

        /// <summary>
        /// Author: Ba.Truong
        /// Description: As School Administrator, Select to delete a locally-created school not currently selected on any pupil records
        /// </summary>
        [WebDriverTest(TimeoutSeconds = 2000, Enabled = false, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { "g1" }, DataProvider = "TC_MSC06_Data")]
        public void TC_MSC06_School_Management_Other_School_Details(string[] basicDetails, string[] address)
        {
            //Pupil Data
            string forename = "SelPersonal" + SeleniumHelper.GenerateRandomString(5) + SeleniumHelper.GenerateRandomNumberUsingDateTime();
            string surname = "SelPersonal" + SeleniumHelper.GenerateRandomString(5) + SeleniumHelper.GenerateRandomNumberUsingDateTime();
            DateTime dob = new DateTime(2011, 02, 02);

            var learnerId = Guid.NewGuid();
            var pupil = this.BuildDataPackage()
                        .AddBasicLearner(learnerId, surname, forename, dob, dateOfAdmission: new DateTime(2011, 10, 05));

            using (new DataSetup(false, true, pupil))
            {
                //Login as School Adminstrator
                SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);
                Wait.WaitForDocumentReady();

                //Navigate to Tasks->School Management -> Other School Details
                AutomationSugar.NavigateMenu("Tasks", "School Management", "Other School Details");
                Wait.WaitForDocumentReady();

                //Click Create button
                var otherSchoolDetailTriplet = new OtherSchoolDetailTriplet();
                var otherSchoolDetailPage = otherSchoolDetailTriplet.Create();

                //Input the basic details
                otherSchoolDetailPage.Name = basicDetails[0];
                otherSchoolDetailPage.HeadTeacher = basicDetails[1];

                //Scroll to School Address then click Add New button
                otherSchoolDetailPage.ScrollToSchoolAddress();
                var addressDialog = otherSchoolDetailPage.AddNewAddress();

                //Add required fileds
                addressDialog.BuildingNo = address[0];
                addressDialog.BuildingNo = address[1];
                addressDialog.PostCode = address[2];
                otherSchoolDetailPage = addressDialog.ClickOK<OtherSchoolDetailPage>();


                //Click Save button
                otherSchoolDetailPage.Save();
                Wait.WaitLoading();

                //VP: Success message displays
                //Assert.AreEqual(true, otherSchoolDetailPage.IsSuccessMessageDisplay(), "Success message doesn't display");

                //Search and select new record
                otherSchoolDetailTriplet = new OtherSchoolDetailTriplet();
                otherSchoolDetailTriplet.SearchCriteria.SchoolName = basicDetails[0];
                var groupTile = otherSchoolDetailTriplet.SearchCriteria.Search().FirstOrDefault(x => x.SchoolName.Equals(basicDetails[0]));
                otherSchoolDetailPage = groupTile.Click<OtherSchoolDetailPage>();
                Wait.WaitForDocumentReady();

                //Click Delete button
                otherSchoolDetailPage.schoolDelete();
                Wait.WaitLoading();
                //Navigate to Home Page
                SeleniumHelper.ClickFooterTab("home page");
                Wait.WaitForDocumentReady();

                //Select Tab Pupil Records
                SeleniumHelper.NavigateQuickLink("Pupil Records");
                Wait.WaitForDocumentReady();

                //Click button Search and select a pupil
                var pupilRecordTriplet = new PupilRecordTriplet();
                var _pupil = pupilRecordTriplet.SearchCriteria.Search().FirstOrDefault();
                var pupilRecordPage = _pupil.Click<PupilRecordPage>();

                //Scroll and select School History.
                pupilRecordPage.SelectSchoolHistoryTab();

                //Click Add School button
                var selectSchoolHistoryDialog = pupilRecordPage.OpenSchoolHistoryDialog();
                var selectSchoolDialog = selectSchoolHistoryDialog.select();

                //Enter school name into School Name textbox and click Search
                selectSchoolDialog.SearchCriteria.SchoolName = basicDetails[0];
                var schoolResult = selectSchoolDialog.SearchCriteria.Search().FirstOrDefault(x => x.Name.Equals(basicDetails[0]));

                //VP: Verify Search result is empty
                Assert.AreEqual(null, schoolResult, "School still exists after deleting");
                               
            }
        }

        /// <summary>
        /// Author: Ba.Truong
        /// Description: As School Administrator, Edit the school details for your own school 
        /// Note: Currently, the school details can not create, so I am using the existing data. Please run this test case once per time to
        ///       avoid conflicting the data.
        /// </summary>
        [WebDriverTest(TimeoutSeconds = 2000, Enabled = true, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { "g1" }, DataProvider = "TC_MSC07_Data")]
        public void TC_MSC07_School_Management_My_School_OtherDetails_All(string[] originalBasicDetails, string[] originalSchoolAddress, string[] originalContactDetails,
                                                                 string[] updateBasicDetails, string[] updateSchoolAddress, string[] updateContractDetails,
                                                                 string[] updateAssociatedSchools,string[] updateCurriculumYears)
        {
            //Login as School Adminstrator
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);

            //Navigate to Tasks -> School Management -> My School Details
            AutomationSugar.NavigateMenu("Tasks", "School Management", "My School Details");

            //Amend values in Basic Detail section
            var mySchoolDetailPage = new MySchoolDetailsPage();
            //mySchoolDetailPage.Name = updateBasicDetails[0];
            mySchoolDetailPage.HeadTeacher = updateBasicDetails[1];
            mySchoolDetailPage.IntakeType = updateBasicDetails[2];
            mySchoolDetailPage.TeachingMedium = updateBasicDetails[3];
            mySchoolDetailPage.PupilGender = updateBasicDetails[4];

            //Select School Address section
            mySchoolDetailPage.ScrollToSchoolAddress();

            //Click Add/Edit button
            if (mySchoolDetailPage._addNewAddressButton.IsElementDisplayed())
            {
                var addressDialog = mySchoolDetailPage.AddAddress();
                //Amend values in Add Address dialog
                addressDialog.Street = updateSchoolAddress[0];
                addressDialog.City = updateSchoolAddress[1];
                mySchoolDetailPage = addressDialog.ClickOK<MySchoolDetailsPage>();
            }
            else
            {
            var addressDialog = mySchoolDetailPage.EditAddress();
            //Amend values in Edit Address dialog
            addressDialog.Street = updateSchoolAddress[0];
            addressDialog.City = updateSchoolAddress[1];
            mySchoolDetailPage = addressDialog.ClickOK<MySchoolDetailsPage>();
            }


            

            //Select Contact Detail section
            mySchoolDetailPage.ScrollToContactDetails();

            //Amend values in Contact Details
            mySchoolDetailPage.TelephoneNumber = updateContractDetails[0];
            mySchoolDetailPage.FaxNumber = updateContractDetails[1];
            mySchoolDetailPage.EmailAddress = updateContractDetails[2];
           // Wait.WaitLoading();
            //Select 'Associated Schools' section
            //mySchoolDetailPage.ScrollToAssociatedSchool();

            //Click Add an Associated School button
            // var pickAssociatedSchoolDialog = mySchoolDetailPage.AddAssociatedSchool();

            //Click Search button then select a school
            //var associatedSchools = pickAssociatedSchoolDialog.SearchCriteria.Search();
            //var associatedSchool = associatedSchools[Convert.ToInt16(updateAssociatedSchools[0])];
            //var schoolName = associatedSchool.Name;
            //associatedSchool.Click();
            //mySchoolDetailPage = pickAssociatedSchoolDialog.ClickOK<MySchoolDetailsPage>();

            //Add Association Type for associated school
            // var recordAssociatedUpdate = mySchoolDetailPage.AssociatedSchools.Rows.FirstOrDefault(x => x.AssociatedSchoolValue.Equals(schoolName));
            // recordAssociatedUpdate.AssociationType = updateAssociatedSchools[1];

            //Add Curriculum Years
            mySchoolDetailPage.ScrollToCurriculumYear();
            var row1 = mySchoolDetailPage.CurriculumYears.Rows.FirstOrDefault(x => (x.NCYear.Equals(updateCurriculumYears[0]))); //Delete if NCYear already exists
            if (row1 != null)
            {
                mySchoolDetailPage.DeleteGridRowIfExist(row1);
                mySchoolDetailPage.Save();
            }
            
            mySchoolDetailPage.ClickAddCurriculumYearLink();
            var rowIndex = mySchoolDetailPage.CurriculumYears.Rows.Count - 1;
            mySchoolDetailPage.CurriculumYears[rowIndex].NCYear = updateCurriculumYears[0];
            mySchoolDetailPage.CurriculumYears[rowIndex].StartDate = updateCurriculumYears[1];

            //Click Save button
            mySchoolDetailPage.Save();

            //VP: Success message displays
           // Assert.AreEqual(true, mySchoolDetailPage.IsSuccessMessageDisplay(), "Success message doesn't display");

            //Select 'Associated Schools' section
            //mySchoolDetailPage.ScrollToAssociatedSchool();

            //Click Delete row button
           // recordAssociatedUpdate = mySchoolDetailPage.AssociatedSchools.Rows.FirstOrDefault(x => x.AssociatedSchoolValue.Equals(schoolName));
            //recordAssociatedUpdate.DeleteRow();

            //Click Save button
            //mySchoolDetailPage.Save();

            //VP: Success message displays
            //Assert.AreEqual(true, mySchoolDetailPage.IsSuccessMessageDisplay(), "Success message doesn't display");

            //Log out system
            SeleniumHelper.Logout();

            //Log in again
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);

            //Navigate to Tasks -> School Management -> My School Details
            AutomationSugar.NavigateMenu("Tasks", "School Management", "My School Details");

            //VP: Verify the basic details changes have been saved
            mySchoolDetailPage = new MySchoolDetailsPage();
            //Assert.AreEqual(updateBasicDetails[0], mySchoolDetailPage.Name, "School Name is not updated");
            //Assert.AreEqual(updateBasicDetails[1], mySchoolDetailPage.HeadTeacher, "Head Teacher is not updated");
            //Assert.AreEqual(updateBasicDetails[2], mySchoolDetailPage.IntakeType, "Intake Type is not updated");
            //Assert.AreEqual(updateBasicDetails[3], mySchoolDetailPage.TeachingMedium, "Teaching Medium is not updated");
            //Assert.AreEqual(updateBasicDetails[4], mySchoolDetailPage.PupilGender, "Pupil Gender is not updated");

            //VP: Verify the school address changes have been saved
            mySchoolDetailPage.ScrollToSchoolAddress();
            var testAddressDialog = mySchoolDetailPage.EditAddress();
            //Assert.AreEqual(updateSchoolAddress[0], testAddressDialog.Street, "Street is not updated");
            //Assert.AreEqual(updateSchoolAddress[1], testAddressDialog.City, "City is not updated");
            mySchoolDetailPage = testAddressDialog.ClickOK<MySchoolDetailsPage>();

            //VP: Verify contract details changes have been saved
            mySchoolDetailPage.ScrollToContactDetails();
            //Assert.AreEqual(updateContractDetails[0], mySchoolDetailPage.TelephoneNumber, "Telephone Number is not updated");
            //Assert.AreEqual(updateContractDetails[1], mySchoolDetailPage.FaxNumber, "Fax Number is not updated");
            //Assert.AreEqual(updateContractDetails[2], mySchoolDetailPage.EmailAddress, "Email Address is not updated");

            //VP: Verify Curriculum Year details changes have been saved
            mySchoolDetailPage.ScrollToCurriculumYear();
            var _rowIndex = mySchoolDetailPage.CurriculumYears.Rows.Count - 1;
           // Assert.AreEqual("Curriculum Year 8", mySchoolDetailPage.CurriculumYears[_rowIndex].NCYear, "The NCYear is not updated");
           // Assert.AreEqual(DateTime.Now.ToString("M/d/yyyy"), mySchoolDetailPage.CurriculumYears[_rowIndex].StartDate, "The Curriculum Year StartDate is not updated");
            //Assert.AreNotEqual(null,
            //       (mySchoolDetailPage.CurriculumYears.Rows.SingleOrDefault(
            //           x => (x.NCYear.Equals(updateCurriculumYears[0]) && x.StartDate.Equals(updateCurriculumYears[1])))),"The Curriculum Year is not updated");

            #region Pos-condition

            //Revert Curriculum Years Detail
            var row = mySchoolDetailPage.CurriculumYears.Rows.FirstOrDefault(x => (x.NCYear.Equals(updateCurriculumYears[0]) && x.StartDate.Equals(updateCurriculumYears[1])));
            mySchoolDetailPage.DeleteGridRowIfExist(row);

            //Revert contract details data
            mySchoolDetailPage.TelephoneNumber = originalContactDetails[0];
            mySchoolDetailPage.FaxNumber = originalContactDetails[1];
            mySchoolDetailPage.EmailAddress = originalContactDetails[2];

            //Revert school address data
            mySchoolDetailPage.ScrollToSchoolAddress();
            testAddressDialog = mySchoolDetailPage.EditAddress();
            testAddressDialog.Street = originalSchoolAddress[0];
            testAddressDialog.City = originalSchoolAddress[1];
            mySchoolDetailPage = testAddressDialog.ClickOK<MySchoolDetailsPage>();

            //Revert basic details data
            mySchoolDetailPage.ExpandBasicDetails();
            //mySchoolDetailPage.Name = originalBasicDetails[0];
            mySchoolDetailPage.HeadTeacher = originalBasicDetails[1];
            mySchoolDetailPage.IntakeType = originalBasicDetails[2];
            mySchoolDetailPage.TeachingMedium = originalBasicDetails[3];
            mySchoolDetailPage.PupilGender = originalBasicDetails[4];

            //Save
            mySchoolDetailPage.Save();

            #endregion
        }


        /// <summary>
        /// Author: Priyanka Jaiswal
        /// Description: As School Administrator, Edit the school details for your own school 
        /// Note: Currently, the school details can not create, so I am using the existing data. Please run this test case once per time to
        ///       avoid conflicting the data.
        /// </summary>
        [WebDriverTest(TimeoutSeconds = 2000, Enabled = false, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { "g1" }, DataProvider = "TC_MSC07_Data_NI")]
        [Variant(Variant.NorthernIrelandStatePrimary )]
        public void TC_MSC07_School_Management_My_School_BasicDetails_NI(string[] updateBasicDetails)
        {
            //Login as School Adminstrator
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);

            //Navigate to Tasks -> School Management -> My School Details
            AutomationSugar.NavigateMenu("Tasks", "School Management", "My School Details");

            //Amend values in Basic Detail section
            var mySchoolDetailPage = new MySchoolDetailsPage();
            mySchoolDetailPage.Name = updateBasicDetails[0];
            mySchoolDetailPage.HeadTeacher = updateBasicDetails[1];
            mySchoolDetailPage.IntakeType = updateBasicDetails[2];
            mySchoolDetailPage.TeachingMedium = updateBasicDetails[3];
            mySchoolDetailPage.PupilGender = updateBasicDetails[4];


            Assert.IsTrue(mySchoolDetailPage._DENINumberTextBox.Displayed
                            && mySchoolDetailPage._EducationLibraryBoardTextBox.Displayed);
 

            //Click Save button
            mySchoolDetailPage.Save();

            SeleniumHelper.Logout();

            //Log in again
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);

            //Navigate to Tasks -> School Management -> My School Details
            AutomationSugar.NavigateMenu("Tasks", "School Management", "My School Details");


            mySchoolDetailPage = new MySchoolDetailsPage();
            Assert.AreEqual(updateBasicDetails[0], mySchoolDetailPage.Name, "School Name is not updated");
            Assert.AreEqual(updateBasicDetails[1], mySchoolDetailPage.HeadTeacher, "Head Teacher is not updated");
            Assert.AreEqual(updateBasicDetails[2], mySchoolDetailPage.IntakeType, "Intake Type is not updated");
            Assert.AreEqual(updateBasicDetails[3], mySchoolDetailPage.TeachingMedium, "Teaching Medium is not updated");
            Assert.AreEqual(updateBasicDetails[4], mySchoolDetailPage.PupilGender, "Pupil Gender is not updated");

            Assert.IsTrue(mySchoolDetailPage._DENINumberTextBox.Displayed
                           && mySchoolDetailPage._EducationLibraryBoardTextBox.Displayed);

            //Save
            mySchoolDetailPage.Save();

             

        }

        /// <summary>
        /// Author: Priyanka Jaiswal
        /// Description: As School Administrator, Edit the school details for your own school 
        /// Note: Currently, the school details can not create, so I am using the existing data. Please run this test case once per time to
        ///       avoid conflicting the data.
        /// </summary>
        [WebDriverTest(TimeoutSeconds = 2000, Enabled = false, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { "g1" }, DataProvider = "TC_MSC07_Data_Eng")]
        [Variant(Variant.EnglishStatePrimary| Variant.WelshStatePrimary)]
        public void TC_MSC07_School_Management_My_School_BasicDetails_EngWales(string[] updateBasicDetails)
        {
            //Login as School Adminstrator
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);

            //Navigate to Tasks -> School Management -> My School Details
            AutomationSugar.NavigateMenu("Tasks", "School Management", "My School Details");

            //Amend values in Basic Detail section
            var mySchoolDetailPage = new MySchoolDetailsPage();
            mySchoolDetailPage.Name = updateBasicDetails[0];
            mySchoolDetailPage.HeadTeacher = updateBasicDetails[1];
            mySchoolDetailPage.EstablishmentNumber = updateBasicDetails[2];
            mySchoolDetailPage.SchoolPhase = updateBasicDetails[3];
            mySchoolDetailPage.SchoolType =updateBasicDetails[4];  
            mySchoolDetailPage.IntakeType = updateBasicDetails[5];
            mySchoolDetailPage.ChildcarePlaces = updateBasicDetails[6];
            mySchoolDetailPage.ChildcareNurseryPlaces = updateBasicDetails[7]; 
           // mySchoolDetailPage.TeachingMedium = updateBasicDetails[8];
            mySchoolDetailPage.PupilGender = updateBasicDetails[8];

            //Click Save button
            mySchoolDetailPage.Save();

            SeleniumHelper.Logout();

            //Log in again
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);

            //Navigate to Tasks -> School Management -> My School Details
            AutomationSugar.NavigateMenu("Tasks", "School Management", "My School Details");

            //VP: Verify the basic details changes have been saved
            mySchoolDetailPage = new MySchoolDetailsPage();
            Assert.AreEqual(updateBasicDetails[0], mySchoolDetailPage.Name, "School Name is not updated");
            Assert.AreEqual(updateBasicDetails[1], mySchoolDetailPage.HeadTeacher, "Head Teacher is not updated");
            Assert.AreEqual(updateBasicDetails[2],mySchoolDetailPage.EstablishmentNumber,"Establishment Number is not updated");
            Assert.AreEqual(updateBasicDetails[3], mySchoolDetailPage.SchoolPhase, "School Phase is not updated");
            Assert.AreEqual(updateBasicDetails[4], mySchoolDetailPage.SchoolType, "School Type is not updated");
            Assert.AreEqual(updateBasicDetails[5], mySchoolDetailPage.IntakeType, "Intake Type is not updated");
            Assert.AreEqual(updateBasicDetails[6], mySchoolDetailPage.ChildcarePlaces, "Childcare Places is not updated");
            Assert.AreEqual(updateBasicDetails[7], mySchoolDetailPage.ChildcareNurseryPlaces, "Childcare Nursery Places is not updated");
           // Assert.AreEqual(updateBasicDetails[3], mySchoolDetailPage.TeachingMedium, "Teaching Medium is not updated");
            Assert.AreEqual(updateBasicDetails[8], mySchoolDetailPage.PupilGender, "Pupil Gender is not updated");
            Assert.IsTrue(mySchoolDetailPage._establishmentNumberTextBox.Displayed 
                           && mySchoolDetailPage._schoolPhaseDropdown.Displayed);


            //Save
            mySchoolDetailPage.Save();

        }


        /// <summary>
        /// Author: Priyanka Jaiswal
        /// Description: As School Administrator, Edit the school details for your own school 
        /// Note: Currently, the school details can not create, so I am using the existing data. Please run this test case once per time to
        ///       avoid conflicting the data.
        /// </summary>
        [WebDriverTest(TimeoutSeconds = 2000, Enabled = false, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { "g1" }, DataProvider = "TC_MSC07_Data_Wel")]
        [Variant( Variant.WelshStatePrimary)]
        public void TC_MSC07_School_Management_My_School_BasicDetails_Wel(string[] updateBasicDetails)
        {
            //Login as School Adminstrator
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);

            //Navigate to Tasks -> School Management -> My School Details
            AutomationSugar.NavigateMenu("Tasks", "School Management", "My School Details");

            //Amend values in Basic Detail section
            var mySchoolDetailPage = new MySchoolDetailsPage();
            mySchoolDetailPage.Name = updateBasicDetails[0];
            mySchoolDetailPage.HeadTeacher = updateBasicDetails[1];
            mySchoolDetailPage.EstablishmentNumber = updateBasicDetails[2];
            mySchoolDetailPage.SchoolPhase = updateBasicDetails[3];
            mySchoolDetailPage.SchoolType = updateBasicDetails[4];
            mySchoolDetailPage.IntakeType = updateBasicDetails[5];
            mySchoolDetailPage.GoverningBodyNumber = updateBasicDetails[6];
            mySchoolDetailPage.ChildcarePlaces = updateBasicDetails[7];
            mySchoolDetailPage.ChildcareNurseryPlaces = updateBasicDetails[8];
            mySchoolDetailPage.PupilGender = updateBasicDetails[9];

            //Click Save button
            mySchoolDetailPage.Save();

            SeleniumHelper.Logout();

            //Log in again
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);

            //Navigate to Tasks -> School Management -> My School Details
            AutomationSugar.NavigateMenu("Tasks", "School Management", "My School Details");

            //VP: Verify the basic details changes have been saved
            mySchoolDetailPage = new MySchoolDetailsPage();
            Assert.AreEqual(updateBasicDetails[0], mySchoolDetailPage.Name, "School Name is not updated");
            Assert.AreEqual(updateBasicDetails[1], mySchoolDetailPage.HeadTeacher, "Head Teacher is not updated");
            Assert.AreEqual(updateBasicDetails[2], mySchoolDetailPage.EstablishmentNumber, "Establishment Number is not updated");
            Assert.AreEqual(updateBasicDetails[3], mySchoolDetailPage.SchoolPhase, "School Phase is not updated");
            Assert.AreEqual(updateBasicDetails[4], mySchoolDetailPage.SchoolType, "School Type is not updated");
            Assert.AreEqual(updateBasicDetails[5], mySchoolDetailPage.IntakeType, "Intake Type is not updated");
            Assert.AreEqual(updateBasicDetails[6], mySchoolDetailPage.GoverningBodyNumber, "Governing Body Number is not updated");
            Assert.AreEqual(updateBasicDetails[7], mySchoolDetailPage.ChildcarePlaces, "Childcare Places is not updated");
            Assert.AreEqual(updateBasicDetails[8], mySchoolDetailPage.ChildcareNurseryPlaces, "Childcare Nursery Places is not updated");
            Assert.AreEqual(updateBasicDetails[9], mySchoolDetailPage.PupilGender, "Pupil Gender is not updated");
            Assert.IsTrue(mySchoolDetailPage._establishmentNumberTextBox.Displayed
                           && mySchoolDetailPage._schoolPhaseDropdown.Displayed);


            //Save
            mySchoolDetailPage.Save();

        }




        /// <summary>
        /// Author: Ba.Truong
        /// Description: As School Administrator, Record the last admission number used
        /// </summary>
        [WebDriverTest(TimeoutSeconds = 2000, Enabled = false, Browsers = new[] { BrowserDefaults.Ie }, Groups = new[] { "g1" }, DataProvider = "TC_MSC08_Data")]
        public void TC_MSC08_School_Management_Admission_Settings(string[] pupilRecords)
        {
            #region Test steps

            //Login as School Adminstrator
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);

            //Navigate to Tasks-> Admission Settings -> Admission Settings 
            AutomationSugar.NavigateMenu("Tasks", "Admission Settings", "Admission Settings");

            //Get last admission number
            var admissionSettingPage = new AdmissionSettingPage();
            var admissionNumber = admissionSettingPage.LastAdmissionNumber;

            //Confirm Admission Number matches the value in SIMS 7

            //Add a pupil
            AutomationSugar.NavigateMenu("Tasks", "Pupils", "Pupil Records");
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

            //Confirm Admission Number increases as we add Pupil
            AutomationSugar.NavigateMenu("Tasks", "Admission Settings", "Admission Settings");
            admissionSettingPage = new AdmissionSettingPage();
            Assert.AreEqual(true, Convert.ToInt16(admissionNumber) < Convert.ToInt16(admissionSettingPage.LastAdmissionNumber), "Admission Number doesn't increase as we add Pupil");

            #endregion

            #region Pos-condition

            //Remove the pupil was added
            AutomationSugar.NavigateMenu("Tasks", "Pupils", "Delete Pupil");
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
        /// TC MSC09
        /// Au : An Nguyen
        /// Description: Check admission number is updated
        /// Role: School Administrator
        /// </summary>
        [WebDriverTest(TimeoutSeconds = 600, Enabled = false, Browsers = new[] { BrowserDefaults.Ie }, Groups = new[] { "g1" }, DataProvider = "TC_MSC09_Data")]
        public void TC_MSC09_Check_admission_number_is_updated(string foreName, string surName, string gender, string DOB, string dateOfAdmission, string yearGroup, string className)
        {
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);

            #region Test

            //Navigate to Admission Settings and record last admission number
            AutomationSugar.NavigateMenu("Tasks", "Admission Settings", "Admission Settings");
            var admissionSetting = new AdmissionSettingsPage();
            admissionSetting.RecordAdmissionNumber();

            //Delete the pupil if it exist before
            AutomationSugar.NavigateMenu("Tasks", "Pupils", "Delete Pupil");
            var deletePupilRecords = new DeletePupilRecordTriplet();
            deletePupilRecords.SearchCriteria.PupilName = String.Format("{0}, {1}", surName, foreName);
            deletePupilRecords.SearchCriteria.IsCurrent = true;
            deletePupilRecords.SearchCriteria.IsLeaver = true;
            deletePupilRecords.SearchCriteria.IsFuture = true;
            var deleteResultPupils = deletePupilRecords.SearchCriteria.Search();
            var deletePupilSearchTile = deleteResultPupils.FirstOrDefault(t => t.Name.Equals(String.Format("{0}, {1}", surName, foreName)));
            var deletePupilRecord = deletePupilSearchTile == null ? new DeletePupilRecordPage() : deletePupilSearchTile.Click<DeletePupilRecordPage>();
            deletePupilRecord.Delete();

            //Navigate to Pupil Record
            AutomationSugar.NavigateMenu("Tasks", "Pupils", "Pupil Records");
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
            registrationDetailDialog.ClassName = className;
            registrationDetailDialog.CreateRecord();
            var confirmRequiredDialog = new POM.Components.Common.ConfirmRequiredDialog();
            confirmRequiredDialog.ClickOk();

            //Save pupil
            var pupilRecord = PupilRecordPage.Create();
            pupilRecord.SavePupil();

            //Navigate to Admission Settings
            AutomationSugar.NavigateMenu("Tasks", "Admission Settings", "Admission Settings");
            admissionSetting = new AdmissionSettingsPage();

            //Verify admission number is update
            Assert.AreEqual(admissionSetting.BeforeAdmissionNumber, admissionSetting.AdmissionNumber - 1, "Admission Number is incorrect");

            #endregion

            #region Post-Condition : Delete pupil

            AutomationSugar.NavigateMenu("Tasks", "Pupils", "Delete Pupil");
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
        /// Author: Y.Ta
        /// Description: Academic Year has been created and saved successfully.
        /// </summary>
        [WebDriverTest(TimeoutSeconds = 1600, Enabled = true, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { "g1" }, DataProvider = "TC_MSC12_Data")]
        public void TC_MSC12_Create_Academic_Year_Successfully(string academicYearName, string year, string[] SchoolTerm, string[] SchoolHoliday, string[] publicHoliday, string[] Inset, string[] genHolidayDate)
        {
            #region Preconditio-Delete a exist academic year

            // Login as School Adminstrator
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);
            Wait.WaitForDocumentReady();
            AutomationSugar.NavigateMenu("Tasks", "School Management", "Academic Years");
            Wait.WaitLoading();

            var academicYearTriplet = new AcademicYearTriplet();
            var academicYearDetail = new AcademicYearDetailPage();
            academicYearTriplet.SearchCriteria.AcademicYearName = "Logigear";
            var academicYearResult = academicYearTriplet.SearchCriteria.Search();
            var academicYearTarget = academicYearResult.FirstOrDefault(p => p.AcademicYear.Contains("Logigear"));
            var academicYearDetailPage = academicYearTarget == null ? academicYearDetail.CreateAcademicYear() : academicYearTarget.Click<AcademicYearDetailPage>();

            academicYearDetailPage.Delete();
            #endregion

            academicYearDetailPage.CreateAcademicYear();

            academicYearDetailPage.Name = academicYearName;
            academicYearDetailPage.AssessmentYear = year;

            // School Term
            academicYearDetailPage.ClickAddSchoolTermLink();

            academicYearDetailPage.SchoolTermsTable[0].Name = SchoolTerm[0];
            academicYearDetailPage.SchoolTermsTable[0].StartDate = SchoolTerm[1];
            Wait.WaitLoading();
            academicYearDetailPage.SchoolTermsTable[0].EndDate = SchoolTerm[2];
            //Wait.WaitLoading();
          

            academicYearDetailPage.ClickAddSchoolTermLink();
            academicYearDetailPage.SchoolTermsTable[1].Name = SchoolTerm[3];
            academicYearDetailPage.SchoolTermsTable[1].StartDate = SchoolTerm[4];
            Wait.WaitLoading();
            academicYearDetailPage.SchoolTermsTable[1].EndDate = SchoolTerm[5];
            Wait.WaitLoading();
           

            // School Holiday            

            academicYearDetailPage.ClickAddSchoolHolidayLink();
            academicYearDetailPage.SchoolHolidayTable[0].Name = SchoolHoliday[0];
            academicYearDetailPage.SchoolHolidayTable[0].StartDate = SchoolHoliday[1];
            Wait.WaitLoading();
            academicYearDetailPage.SchoolHolidayTable[0].EndDate = SchoolHoliday[2];
            Wait.WaitLoading();

            // Public Holiday
            academicYearDetailPage.ClickAddPublicHolidayLink();
            academicYearDetailPage.PublicHolidayTable[0].Name = publicHoliday[0];
            academicYearDetailPage.PublicHolidayTable[0].Date = publicHoliday[1];

            academicYearDetailPage.ClickCalculateSchoolHoliday();

            // Inset Day
            academicYearDetailPage.ClickInsetDayLink();
            academicYearDetailPage.InsetDayTable[0].Name = Inset[0];
            Wait.WaitLoading();
            academicYearDetailPage.InsetDayTable[0].Date = Inset[1];
            //Wait.WaitLoading();
            academicYearDetailPage.InsetDayTable[0].AM = true;
            Wait.WaitLoading();
            academicYearDetailPage.InsetDayTable[0].PM = true;

            //Select TAB 'Working Week'
            //Set up the working week as required

            Wait.WaitLoading();
            academicYearDetailPage.WorkingWeekGrid[0].WorkingWeekSession = "AM";
            academicYearDetailPage.WorkingWeekGrid[0].Monday = true;
            academicYearDetailPage.WorkingWeekGrid[0].Tuesday = false;
            academicYearDetailPage.WorkingWeekGrid[0].Wednesday = true;
            academicYearDetailPage.WorkingWeekGrid[0].Thurday = true;
            academicYearDetailPage.WorkingWeekGrid[0].Friday = true;

            Wait.WaitLoading();
            academicYearDetailPage.WorkingWeekGrid[1].WorkingWeekSession = "PM";
            academicYearDetailPage.WorkingWeekGrid[1].Monday = true;
            academicYearDetailPage.WorkingWeekGrid[1].Tuesday = true;
            academicYearDetailPage.WorkingWeekGrid[1].Wednesday = true;
            academicYearDetailPage.WorkingWeekGrid[1].Thurday = false;
            academicYearDetailPage.WorkingWeekGrid[1].Friday = true;

            Wait.WaitLoading();
            academicYearDetailPage.Save();
            Assert.AreEqual(true, academicYearDetailPage.IsSuccessMessageDisplay(), "The success message does not display");
            Wait.WaitForDocumentReady();
            academicYearTriplet.SearchCriteria.AcademicYearName = academicYearName;
            academicYearResult = academicYearTriplet.SearchCriteria.Search();
            academicYearTarget = academicYearResult.SingleOrDefault(p => p.AcademicYear.Contains(academicYearName));
            Assert.AreNotEqual(null, academicYearTarget, "The academic year does not found");

            academicYearDetailPage = academicYearTarget.Click<AcademicYearDetailPage>();

            // Verify School Term
            Assert.AreEqual(SchoolTerm[0], academicYearDetailPage.SchoolTermsTable[0].Name, "The Name of School term is not correct");
            Assert.AreEqual(SchoolTerm[1], academicYearDetailPage.SchoolTermsTable[0].StartDate, "The Start Date of School term is not correct");
            Assert.AreEqual(SchoolTerm[2], academicYearDetailPage.SchoolTermsTable[0].EndDate, "The End Date of School term is not correct");

            // Verify School Holiday            
            Assert.AreEqual(SchoolHoliday[0], academicYearDetailPage.SchoolHolidayTable[0].Name, "The Name of School Holiday is not correct ");
            Assert.AreEqual(SchoolHoliday[1], academicYearDetailPage.SchoolHolidayTable[0].StartDate, "The Start Date of School Holiday is not correct ");
            Assert.AreEqual(SchoolHoliday[2], academicYearDetailPage.SchoolHolidayTable[0].EndDate, "The End Date of School Holiday is not correct ");

            Assert.AreEqual(genHolidayDate[0], academicYearDetailPage.SchoolHolidayTable[1].Name, "The Name of School Holiday is not correct ");
            Assert.AreEqual(genHolidayDate[1], academicYearDetailPage.SchoolHolidayTable[1].StartDate, "The Start Date of School Holiday is not correct ");
            Assert.AreEqual(genHolidayDate[2], academicYearDetailPage.SchoolHolidayTable[1].EndDate, "The End Date of School Holiday is not correct ");


            // Verify Public Holiday
            Assert.AreEqual(publicHoliday[0], academicYearDetailPage.PublicHolidayTable[0].Name, "The Name of public holiday is not correct");
            Assert.AreEqual(publicHoliday[1], academicYearDetailPage.PublicHolidayTable[0].Date, "The Date of public holiday is not correct");

            // Verify Inset day
            Assert.AreEqual(Inset[0], academicYearDetailPage.InsetDayTable[0].Name, "The Name of inset Date is not correctly");
            Assert.AreEqual(Inset[1], academicYearDetailPage.InsetDayTable[0].Date, "The Date of inset Date is not correctly");
            Assert.AreEqual(true, academicYearDetailPage.InsetDayTable[0].AM, "The AM session of inset Date is not correctly");
            Assert.AreEqual(true, academicYearDetailPage.InsetDayTable[0].PM, "The PM session of inset Date is not correctly");

            // Verify working day week            
            Assert.AreEqual("AM", academicYearDetailPage.WorkingWeekGrid[0].WorkingWeekSession, "The working week session is not correct");
            Assert.AreEqual("PM", academicYearDetailPage.WorkingWeekGrid[1].WorkingWeekSession, "The working week session is not correct");

            #region Delete record
            academicYearDetail = academicYearTarget.Click<AcademicYearDetailPage>();
            academicYearDetail.Delete();

            #endregion

        }

        /// <summary>
        /// TC MSC13
        /// Au : An Nguyen
        /// Description: Delete the future academic year created previously
        /// Role: School Administrator
        /// </summary>
        [WebDriverTest(TimeoutSeconds = 600, Enabled = false, Browsers = new[] { BrowserDefaults.Ie }, Groups = new[] { "g1" }, DataProvider = "TC_MSC13_Data")]
        public void TC_MSC13_Delete_the_future_academic_year_created_previously(string fullName, string academicYear, string[] schoolTerm)
        {
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);

            #region Pre-Condition : Add new future academic year

            //Navigate to Academic Years
            AutomationSugar.NavigateMenu("Tasks", "School Management", "Academic Years");
            var academicYearTriplet = new AcademicYearTriplet();

            //Delete old Academic Year if it exists
            academicYearTriplet.SearchCriteria.AcademicYearName = fullName;
            var academicSearchResult = academicYearTriplet.SearchCriteria.Search();
            var academicYearTile = academicSearchResult.FirstOrDefault(t => t.AcademicYear.Equals(fullName));
            var academicYearPage = academicYearTile == null ? new AcademicYearDetailPage() : academicYearTile.Click<AcademicYearDetailPage>();
            academicYearPage.Delete();

            //Create new Academic Year
            academicYearTriplet.Refresh();
            academicYearPage = academicYearTriplet.Create();
            academicYearPage.Name = fullName;
            academicYearPage.AssessmentYear = academicYear;

            //Add School Term
            academicYearPage.ClickAddSchoolTermLink();
            var schoolTermTable = academicYearPage.SchoolTermsTable;
            schoolTermTable[0].Name = schoolTerm[0];
            schoolTermTable.Refresh();
            schoolTermTable[0].StartDate = schoolTerm[1];
            schoolTermTable.Refresh();
            schoolTermTable[0].EndDate = schoolTerm[2];

            //Save
            academicYearPage.Save();

            #endregion

            #region Test

            //Search academic year
            academicYearTriplet.Refresh();
            academicYearTriplet.SearchCriteria.AcademicYearName = fullName;
            academicSearchResult = academicYearTriplet.SearchCriteria.Search();
            academicYearTile = academicSearchResult.FirstOrDefault(t => t.AcademicYear.Equals(fullName));
            academicYearPage = academicYearTile.Click<AcademicYearDetailPage>();

            //Delete academic year
            academicYearPage.Delete();

            //Verify academic year does not display on search result
            academicYearTriplet.Refresh();
            academicYearTriplet.SearchCriteria.AcademicYearName = fullName;
            academicSearchResult = academicYearTriplet.SearchCriteria.Search();
            academicYearTile = academicSearchResult.FirstOrDefault(t => t.AcademicYear.Equals(fullName));
            Assert.AreEqual(null, academicYearTile, fullName + " still displays on search result");

            #endregion
        }

        /// <summary>
        /// Author: Y.Ta
        /// Description: Recreate the future academic year deleted previously.
        /// </summary>

        [WebDriverTest(TimeoutSeconds = 1600, Enabled = true, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { "g1" }, DataProvider = "TC_MSC14_Data")]
        public void TC_MSC14_Create_Academic_Year_Successfully(string academicYearName, string year, string[] SchoolTerm, string[] SchoolHoliday, string[] publicHoliday, string[] Inset, string[] genHolidayDate)
        {
            #region Preconditio-Delete a exist academic year

            // Login as School Adminstrator
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);
            AutomationSugar.NavigateMenu("Tasks", "School Management", "Academic Years");

            var academicYearTriplet = new AcademicYearTriplet();
            var academicYearDetail = new AcademicYearDetailPage();
            academicYearTriplet.SearchCriteria.AcademicYearName = "Logigear";
            var academicYearResult = academicYearTriplet.SearchCriteria.Search();
            var academicYearTarget = academicYearResult.FirstOrDefault(p => p.AcademicYear.Contains("Logigear"));
            var academicYearDetailPage = academicYearTarget == null ? academicYearDetail.CreateAcademicYear() : academicYearTarget.Click<AcademicYearDetailPage>();

            academicYearDetailPage.Delete();

            academicYearDetailPage.CreateAcademicYear();

            academicYearDetailPage.Name = academicYearName;
            academicYearDetailPage.AssessmentYear = year;

            // School Term
            academicYearDetailPage.ClickAddSchoolTermLink();

            academicYearDetailPage.SchoolTermsTable[0].Name = SchoolTerm[0];
            academicYearDetailPage.SchoolTermsTable[0].StartDate = SchoolTerm[1];
            Wait.WaitLoading();
            academicYearDetailPage.SchoolTermsTable[0].EndDate = SchoolTerm[2];

            academicYearDetailPage.ClickAddSchoolTermLink();
            academicYearDetailPage.SchoolTermsTable[1].Name = SchoolTerm[3];
            academicYearDetailPage.SchoolTermsTable[1].StartDate = SchoolTerm[4];
            Wait.WaitLoading();
            academicYearDetailPage.SchoolTermsTable[1].EndDate = SchoolTerm[5];

            // School Holiday            
            Wait.WaitLoading();
            academicYearDetailPage.ClickAddSchoolHolidayLink();
            academicYearDetailPage.SchoolHolidayTable[0].Name = SchoolHoliday[0];
            academicYearDetailPage.SchoolHolidayTable[0].StartDate = SchoolHoliday[1];
            Wait.WaitLoading();
            academicYearDetailPage.SchoolHolidayTable[0].EndDate = SchoolHoliday[2];

            academicYearDetail.ClickCalculateSchoolHoliday();

            // Public Holiday
            academicYearDetailPage.ClickAddPublicHolidayLink();
            academicYearDetailPage.PublicHolidayTable[0].Name = publicHoliday[0];
            academicYearDetailPage.PublicHolidayTable[0].Date = publicHoliday[1];

            // Inset Day
            academicYearDetailPage.ClickInsetDayLink();
            academicYearDetailPage.InsetDayTable[0].Name = Inset[0];
            academicYearDetailPage.InsetDayTable[0].Date = Inset[1];
            academicYearDetailPage.InsetDayTable[0].AM = true;
            Wait.WaitLoading();
            academicYearDetailPage.InsetDayTable[0].PM = true;

            //Select TAB 'Working Week'
            //Set up the working week as required
            Wait.WaitLoading();
            academicYearDetailPage.WorkingWeekGrid[0].WorkingWeekSession = "AM";
            academicYearDetailPage.WorkingWeekGrid[0].Monday = true;
            academicYearDetailPage.WorkingWeekGrid[0].Tuesday = false;
            academicYearDetailPage.WorkingWeekGrid[0].Wednesday = true;
            academicYearDetailPage.WorkingWeekGrid[0].Thurday = true;
            academicYearDetailPage.WorkingWeekGrid[0].Friday = true;

            Wait.WaitLoading();
            academicYearDetailPage.WorkingWeekGrid[1].WorkingWeekSession = "PM";
            academicYearDetailPage.WorkingWeekGrid[1].Monday = true;
            academicYearDetailPage.WorkingWeekGrid[1].Tuesday = true;
            academicYearDetailPage.WorkingWeekGrid[1].Wednesday = false;
            academicYearDetailPage.WorkingWeekGrid[1].Thurday = true;
            academicYearDetailPage.WorkingWeekGrid[1].Friday = true;

            academicYearDetailPage.Save();

            academicYearTriplet.SearchCriteria.AcademicYearName = academicYearName;
            academicYearResult = academicYearTriplet.SearchCriteria.Search();
            academicYearTarget = academicYearResult.SingleOrDefault(p => p.AcademicYear.Contains(academicYearName));

            academicYearDetail = academicYearTarget.Click<AcademicYearDetailPage>();
            academicYearDetail.Delete();

            #endregion

            #region Step: ReCreate academic year

            academicYearDetailPage.CreateAcademicYear();

            academicYearDetailPage.Name = academicYearName;
            academicYearDetailPage.AssessmentYear = year;

            // School Term
            academicYearDetailPage.ClickAddSchoolTermLink();

            academicYearDetailPage.SchoolTermsTable[0].Name = SchoolTerm[0];
            academicYearDetailPage.SchoolTermsTable[0].StartDate = SchoolTerm[1];
            Wait.WaitLoading();
            academicYearDetailPage.SchoolTermsTable[0].EndDate = SchoolTerm[2];

            // School Holiday            

            academicYearDetailPage.ClickAddSchoolTermLink();
            academicYearDetailPage.SchoolTermsTable[1].Name = SchoolTerm[3];
            academicYearDetailPage.SchoolTermsTable[1].StartDate = SchoolTerm[4];
            Wait.WaitLoading();
            academicYearDetailPage.SchoolTermsTable[1].EndDate = SchoolTerm[5];

            academicYearDetail.ClickCalculateSchoolHoliday();

            // Public Holiday
            academicYearDetailPage.ClickAddPublicHolidayLink();
            academicYearDetailPage.PublicHolidayTable[0].Name = publicHoliday[0];
            academicYearDetailPage.PublicHolidayTable[0].Date = publicHoliday[1];

            // Inset Day
            academicYearDetailPage.ClickInsetDayLink();
            academicYearDetailPage.InsetDayTable[0].Name = Inset[0];
            academicYearDetailPage.InsetDayTable[0].Date = Inset[1];
            academicYearDetailPage.InsetDayTable[0].AM = true;
            Wait.WaitLoading();
            academicYearDetailPage.InsetDayTable[0].PM = true;

            //Select TAB 'Working Week'
            //Set up the working week as required
            Wait.WaitLoading();
            academicYearDetailPage.WorkingWeekGrid[0].WorkingWeekSession = "AM";
            academicYearDetailPage.WorkingWeekGrid[0].Monday = true;
            academicYearDetailPage.WorkingWeekGrid[0].Tuesday = true;
            academicYearDetailPage.WorkingWeekGrid[0].Wednesday = false;
            academicYearDetailPage.WorkingWeekGrid[0].Thurday = true;
            academicYearDetailPage.WorkingWeekGrid[0].Friday = true;

            Wait.WaitLoading();
            academicYearDetailPage.WorkingWeekGrid[1].WorkingWeekSession = "PM";
            academicYearDetailPage.WorkingWeekGrid[1].Monday = true;
            academicYearDetailPage.WorkingWeekGrid[1].Tuesday = false;
            academicYearDetailPage.WorkingWeekGrid[1].Wednesday = true;
            academicYearDetailPage.WorkingWeekGrid[1].Thurday = true;
            academicYearDetailPage.WorkingWeekGrid[1].Friday = true;

            academicYearDetailPage.Save();
            Assert.AreEqual(true, academicYearDetailPage.IsSuccessMessageDisplay(), "The success message does not display");
            academicYearTriplet.SearchCriteria.AcademicYearName = academicYearName;
            academicYearResult = academicYearTriplet.SearchCriteria.Search();
            academicYearTarget = academicYearResult.SingleOrDefault(p => p.AcademicYear.Contains(academicYearName));
            Assert.AreNotEqual(null, academicYearTarget, "The academic year does not found");

            // Verify School Term
            Assert.AreEqual(SchoolTerm[0], academicYearDetailPage.SchoolTermsTable[0].Name, "The Name of School term is not correct");
            Assert.AreEqual(SchoolTerm[1], academicYearDetailPage.SchoolTermsTable[0].StartDate, "The Start Date of School term is not correct");
            Assert.AreEqual(SchoolTerm[2], academicYearDetailPage.SchoolTermsTable[0].EndDate, "The End Date of School term is not correct");

            // Verify School Holiday            
            Assert.AreEqual(genHolidayDate[0], academicYearDetailPage.SchoolHolidayTable[0].Name, "The Name of School Holiday is not correct ");
            Assert.AreEqual(genHolidayDate[1], academicYearDetailPage.SchoolHolidayTable[0].StartDate, "The Start Date of School Holiday is not correct ");
            Assert.AreEqual(genHolidayDate[2], academicYearDetailPage.SchoolHolidayTable[0].EndDate, "The End Date of School Holiday is not correct ");

            // Verify Public Holiday
            Assert.AreEqual(publicHoliday[0], academicYearDetailPage.PublicHolidayTable[0].Name, "The Name of public holiday is not correct");
            Assert.AreEqual(publicHoliday[1], academicYearDetailPage.PublicHolidayTable[0].Date, "The Date of public holiday is not correct");

            // Verify Inset day
            Assert.AreEqual(Inset[0], academicYearDetailPage.InsetDayTable[0].Name, "The Name of inset Date is not correctly");
            Assert.AreEqual(Inset[1], academicYearDetailPage.InsetDayTable[0].Date, "The Date of inset Date is not correctly");
            Assert.AreEqual(true, academicYearDetailPage.InsetDayTable[0].AM, "The AM session of inset Date is not correctly");
            Assert.AreEqual(true, academicYearDetailPage.InsetDayTable[0].PM, "The PM session of inset Date is not correctly");

            // Verify working day week            
            Assert.AreEqual("AM", academicYearDetailPage.WorkingWeekGrid[0].WorkingWeekSession, "The working week session is not correct");
            Assert.AreEqual("PM", academicYearDetailPage.WorkingWeekGrid[1].WorkingWeekSession, "The working week session is not correct");

            #endregion


            #region Delete record
            academicYearDetail = academicYearTarget.Click<AcademicYearDetailPage>();
            academicYearDetail.Delete();

            #endregion

        }

        /// <summary>
        /// Author: Y.Ta
        /// Description: Academic Year has been created and saved successfully.
        /// </summary>
        [WebDriverTest(TimeoutSeconds = 1600, Enabled = true, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { "g1" }, DataProvider = "TC_MSC15_Data")]
        public void TC_MSC15_Create_Academic_Year_Successfully(string academicYearName, string year, string[] SchoolTerm, string[] SchoolHoliday, string[] publicHoliday, string[] Inset, string academicYearNameUpdate, string yearUpdate, string[] SchoolTermUpdate, string[] SchoolHolidayUpdate, string[] publicHolidayUpdate, string[] InsetUpdate, string[] genHolidayDate)

        
        {
            #region Preconditio-Create academic year

            // Login as School Adminstrator
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);
            AutomationSugar.NavigateMenu("Tasks", "School Management", "Academic Years");

            Wait.WaitLoading();
            var academicYearTriplet = new AcademicYearTriplet();
            Wait.WaitLoading();
            var academicYearDetail = new AcademicYearDetailPage();
            academicYearTriplet.SearchCriteria.AcademicYearName = "Logigear";
            var academicYearResult = academicYearTriplet.SearchCriteria.Search();
            var academicYearTarget = academicYearResult.FirstOrDefault(p => p.AcademicYear.Contains("Logigear"));
            var academicYearDetailPage = academicYearTarget == null ? academicYearDetail.CreateAcademicYear() : academicYearTarget.Click<AcademicYearDetailPage>();

            academicYearDetailPage.Delete();

            academicYearDetailPage.CreateAcademicYear();

            academicYearDetailPage.Name = academicYearName;
            academicYearDetailPage.AssessmentYear = year;

            // School Term
            academicYearDetailPage.ClickAddSchoolTermLink();

            academicYearDetailPage.SchoolTermsTable[0].Name = SchoolTerm[0];
            academicYearDetailPage.SchoolTermsTable[0].StartDate = SchoolTerm[1];
            Wait.WaitLoading();
            academicYearDetailPage.SchoolTermsTable[0].EndDate = SchoolTerm[2];

            academicYearDetailPage.ClickAddSchoolTermLink();

            academicYearDetailPage.SchoolTermsTable[1].Name = SchoolTerm[3];
            academicYearDetailPage.SchoolTermsTable[1].StartDate = SchoolTerm[4];
            Wait.WaitLoading();
            academicYearDetailPage.SchoolTermsTable[1].EndDate = SchoolTerm[5];

            // School Holiday            

            academicYearDetailPage.ClickAddSchoolHolidayLink();
            academicYearDetailPage.SchoolHolidayTable[0].Name = SchoolHoliday[0];
            academicYearDetailPage.SchoolHolidayTable[0].StartDate = SchoolHoliday[1];
            Wait.WaitLoading();
            academicYearDetailPage.SchoolHolidayTable[0].EndDate = SchoolHoliday[2];
            Wait.WaitLoading();

            academicYearDetailPage.ClickCalculateSchoolHoliday();

            // Public Holiday
            academicYearDetailPage.ClickAddPublicHolidayLink();
            academicYearDetailPage.PublicHolidayTable[0].Name = publicHoliday[0];
            academicYearDetailPage.PublicHolidayTable[0].Date = publicHoliday[1];

            // Inset Day
            academicYearDetailPage.ClickInsetDayLink();
            academicYearDetailPage.InsetDayTable[0].Name = Inset[0];
            Wait.WaitLoading();
            academicYearDetailPage.InsetDayTable[0].Date = Inset[1];
            Wait.WaitLoading();
            academicYearDetailPage.InsetDayTable[0].AM = true;
            Wait.WaitLoading();
            academicYearDetailPage.InsetDayTable[0].PM = true;

            //Select TAB 'Working Week'
            //Set up the working week as required
            Wait.WaitLoading();
            academicYearDetailPage.WorkingWeekGrid[0].WorkingWeekSession = "AM";
            academicYearDetailPage.WorkingWeekGrid[0].Monday = true;
            academicYearDetailPage.WorkingWeekGrid[0].Tuesday = true;
            academicYearDetailPage.WorkingWeekGrid[0].Wednesday = false;
            academicYearDetailPage.WorkingWeekGrid[0].Thurday = true;
            academicYearDetailPage.WorkingWeekGrid[0].Friday = true;


            academicYearDetailPage.WorkingWeekGrid[1].WorkingWeekSession = "PM";
            academicYearDetailPage.WorkingWeekGrid[1].Monday = true;
            academicYearDetailPage.WorkingWeekGrid[1].Tuesday = false;
            academicYearDetailPage.WorkingWeekGrid[1].Wednesday = true;
            academicYearDetailPage.WorkingWeekGrid[1].Thurday = true;
            academicYearDetailPage.WorkingWeekGrid[1].Friday = true;

            academicYearDetailPage.Save();

            academicYearTriplet.SearchCriteria.AcademicYearName = academicYearName;
            academicYearResult = academicYearTriplet.SearchCriteria.Search();
            academicYearTarget = academicYearResult.SingleOrDefault(p => p.AcademicYear.Contains(academicYearName));

            academicYearDetail = academicYearTarget.Click<AcademicYearDetailPage>();
            academicYearDetail.Delete();

            #endregion

        }

        /// <summary>
        /// Author: Ba.Truong
        /// Description: As School Administrator, Add Term dates and generate School holidays
        /// </summary>
        [WebDriverTest(TimeoutSeconds = 2000, Enabled = true, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { "g1" }, DataProvider = "TC_MSC16_Data")]
        public void TC_MSC16_School_Management_Academic_Years(string currentAcademicYear, string[] schoolTerm, string[] schoolHoliday)
        {
            //Login as School Adminstrator
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);

            //Navigate to Tasks->School Management -> Academic Years
            AutomationSugar.NavigateMenu("Tasks", "School Management", "Academic Years");

            //Select current academic year
            var academicYearTriplet = new AcademicYearTriplet();
            academicYearTriplet.SearchCriteria.AcademicYearName = currentAcademicYear;
            var academicYearTile = academicYearTriplet.SearchCriteria.Search().FirstOrDefault(x => x.AcademicYear.Equals(currentAcademicYear));
            var academicYearDetailPage = academicYearTile.Click<AcademicYearDetailPage>();

            //Select TAB 'School Terms'
            academicYearDetailPage.ScrollToSchoolTerms();

            //Add Term information for the Year Selected
            academicYearDetailPage.ClickAddSchoolTermLink();
            var rowIndex = academicYearDetailPage.SchoolTermsTable.Rows.Count - 1;
            academicYearDetailPage.SchoolTermsTable[rowIndex].Name = schoolTerm[0];
            Wait.WaitLoading();
            academicYearDetailPage.SchoolTermsTable[rowIndex].StartDate = schoolTerm[1];
            Wait.WaitLoading();
            academicYearDetailPage.SchoolTermsTable[rowIndex].EndDate = schoolTerm[2];

            //Add 'Calculate School Holidays'
            academicYearDetailPage.ClickCalculateSchoolHoliday();
            Wait.WaitLoading();
            rowIndex = academicYearDetailPage.SchoolHolidayTable.Rows.Count - 1;
            academicYearDetailPage.SchoolHolidayTable[rowIndex].Name = schoolHoliday[0];
            academicYearDetailPage.SchoolHolidayTable[rowIndex].StartDate = schoolHoliday[1];
            academicYearDetailPage.SchoolHolidayTable[rowIndex].EndDate = schoolHoliday[2];

            //Click Button 'Save'
            academicYearDetailPage.Save();

            //Re-select current academic year to verify results
            academicYearTriplet = new AcademicYearTriplet();
            academicYearTriplet.SearchCriteria.AcademicYearName = currentAcademicYear;
            academicYearTile = academicYearTriplet.SearchCriteria.Search().FirstOrDefault(x => x.AcademicYear.Equals(currentAcademicYear));
            academicYearDetailPage = academicYearTile.Click<AcademicYearDetailPage>();

            //Confirm the School Terms were updated
            Assert.AreEqual(true, academicYearDetailPage.SchoolTermsTable.Rows.Any(x => x.Name.Equals(schoolTerm[0])), "School Tearm was not added");

            //Confirm the School Holidays were updated
            Assert.AreEqual(true, academicYearDetailPage.SchoolHolidayTable.Rows.Any(x => x.Name.Equals(schoolHoliday[0])), "School Holiday was not added");
        }

        /// <summary>
        /// Author: Huy Vo
        /// Description: Add public holiday to current academic year for a date in the future
        /// </summary>
        [WebDriverTest(TimeoutSeconds = 800, Enabled = true, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { "g1" }, DataProvider = "TC_MSC19_Data")]
        public void TC_MSC19_Add_Public_Holiday_To_Current_Academic_Year_For_A_Date_In_The_Future(string academicYearName, string assessmentYear, string schoolTermName,
                                                                                                  string schoolTermStartDate, string schoolTermEndDate, string publicHolidayName,
                                                                                                  string publicHolidayDate, string publicHolidayNameUpdate, string publicHolidayDateInFuture)
        {
            #region Pre-condition: Delete a exist academic year

            // Login as School Adminstrator
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);
            AutomationSugar.NavigateMenu("Tasks", "School Management", "Academic Years");

            var academicYearTriplet = new AcademicYearTriplet();
            var academicYearDetail = new AcademicYearDetailPage();
            academicYearTriplet.SearchCriteria.AcademicYearName = academicYearName;
            var academicYearResult = academicYearTriplet.SearchCriteria.Search();
            var academicYearTarget = academicYearResult.FirstOrDefault(p => p.AcademicYear.Contains(academicYearName));
            var academicYearDetailPage = academicYearTarget == null ? academicYearDetail.CreateAcademicYear() : academicYearTarget.Click<AcademicYearDetailPage>();

            academicYearDetailPage.Delete();
            academicYearDetailPage.CreateAcademicYear();
            academicYearDetailPage.Name = academicYearName;
            academicYearDetailPage.AssessmentYear = assessmentYear;

            // School Term
            academicYearDetailPage.ClickAddSchoolTermLink();
            academicYearDetailPage.SchoolTermsTable[0].Name = schoolTermName;
            Wait.WaitLoading();
            academicYearDetailPage.SchoolTermsTable[0].StartDate = schoolTermStartDate;
            Wait.WaitLoading();
            academicYearDetailPage.SchoolTermsTable[0].EndDate = schoolTermEndDate;

            // Public Holiday
            academicYearDetailPage.ClickAddPublicHolidayLink();
            academicYearDetailPage.PublicHolidayTable[0].Name = publicHolidayName;
            Wait.WaitLoading();
            academicYearDetailPage.PublicHolidayTable[0].Date = publicHolidayDate;
            academicYearDetailPage.Save();

            //Search to update Date in future for Public holiday 
            academicYearTriplet.SearchCriteria.AcademicYearName = academicYearName;
            academicYearResult = academicYearTriplet.SearchCriteria.Search();
            academicYearTarget = academicYearResult.SingleOrDefault(p => p.AcademicYear.Contains(academicYearName));
            academicYearDetailPage = academicYearTarget.Click<AcademicYearDetailPage>();

            // Select public holiday to update future date
            academicYearDetailPage.ClickAddPublicHolidayLink();
            academicYearDetailPage.PublicHolidayTable.GetLastRow().Name = publicHolidayNameUpdate;
            academicYearDetailPage.PublicHolidayTable.GetLastRow().Date = publicHolidayDateInFuture;
            academicYearDetailPage.Save();
            academicYearDetailPage.Refresh();

            //Verify update successfully
            Assert.AreEqual(true, academicYearDetailPage.IsSuccessMessageDisplay(), "Update academic unsuccessfully");

            //Search academic year was created
            academicYearTriplet.SearchCriteria.AcademicYearName = academicYearName;
            academicYearResult = academicYearTriplet.SearchCriteria.Search();
            academicYearTarget = academicYearResult.SingleOrDefault(p => p.AcademicYear.Contains(academicYearName));
            academicYearDetailPage = academicYearTarget.Click<AcademicYearDetailPage>();

            //Verify that update academic year succesfully
            Assert.AreEqual(academicYearName, academicYearDetailPage.Name, "Academic year name is not equals");
            Assert.AreEqual(assessmentYear, academicYearDetailPage.AssessmentYear, "Assessment year is not equals");

            //Verify that record in School Term is correct
            var schoolTermTable = academicYearDetailPage.SchoolTermsTable;
            Assert.AreNotEqual(null, (schoolTermTable.Rows.SingleOrDefault(x => x.Name == schoolTermName && x.StartDate == schoolTermStartDate &&
                x.EndDate == schoolTermEndDate)), "School Term data is not equal");

            //Verify that record in Public Holiday is correct
            var publicHoliday = academicYearDetailPage.PublicHolidayTable;
            Assert.AreNotEqual(null, (publicHoliday.Rows.SingleOrDefault(x => x.Name == publicHolidayNameUpdate && x.Date == publicHolidayDateInFuture))
                , "Public Holiday data is not equal");

            // Delete academic year
            academicYearDetailPage.Delete();

            #endregion
        }

        /// <summary>
        /// Author: Huy Vo
        /// Description: Add INSET day to current academic year for a date in the future
        /// </summary>
        [WebDriverTest(TimeoutSeconds = 800, Enabled = false, Browsers = new[] { BrowserDefaults.Ie }, Groups = new[] { "g1" }, DataProvider = "TC_MSC20_Data")]
        public void TC_MSC20_Add_Day_To_Current_Academic_Year_For_Date_In_Future(string academicYearName, string assessmentYear, string schoolTermName,
                                                                                 string schoolTermStartDate, string schoolTermEndDate, string insertDayName,
                                                                                 string insertDayDate, string insertDayNameUpdate, string insertDayDateFuture)
        {
            #region Pre-condition: Delete a exist academic year

            // Login as School Adminstrator
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);
            AutomationSugar.NavigateMenu("Tasks", "School Management", "Academic Years");

            var academicYearTriplet = new AcademicYearTriplet();
            var academicYearDetail = new AcademicYearDetailPage();
            academicYearTriplet.SearchCriteria.AcademicYearName = academicYearName;
            var academicYearResult = academicYearTriplet.SearchCriteria.Search();
            var academicYearTarget = academicYearResult.FirstOrDefault(p => p.AcademicYear.Contains(academicYearName));
            var academicYearDetailPage = academicYearTarget == null ? academicYearDetail.CreateAcademicYear() : academicYearTarget.Click<AcademicYearDetailPage>();

            academicYearDetailPage.Delete();
            academicYearDetailPage.CreateAcademicYear();
            academicYearDetailPage.Name = academicYearName;
            academicYearDetailPage.AssessmentYear = assessmentYear;

            // School Term
            academicYearDetailPage.ClickAddSchoolTermLink();
            academicYearDetailPage.SchoolTermsTable[0].Name = schoolTermName;
            academicYearDetailPage.SchoolTermsTable[0].StartDate = schoolTermStartDate;
            academicYearDetailPage.SchoolTermsTable[0].EndDate = schoolTermEndDate;

            // Add record for INSERT Days
            academicYearDetailPage.ClickInsetDayLink();
            academicYearDetailPage.InsetDayTable[0].Name = insertDayName;
            academicYearDetailPage.InsetDayTable[0].Date = insertDayDate;
            academicYearDetailPage.InsetDayTable[0].AM = true;
            academicYearDetailPage.InsetDayTable[0].PM = true;
            academicYearDetailPage.Save();

            //Search to update Date in future for Public holiday 
            academicYearTriplet.SearchCriteria.AcademicYearName = academicYearName;
            academicYearResult = academicYearTriplet.SearchCriteria.Search();
            academicYearTarget = academicYearResult.SingleOrDefault(p => p.AcademicYear.Contains(academicYearName));
            academicYearDetailPage = academicYearTarget.Click<AcademicYearDetailPage>();

            // Select public holiday to update future date
            academicYearDetailPage.ClickInsetDayLink();
            academicYearDetailPage.InsetDayTable.GetLastRow().Name = insertDayNameUpdate;
            academicYearDetailPage.InsetDayTable.GetLastRow().Date = insertDayDateFuture;
            academicYearDetailPage.InsetDayTable.GetLastRow().AM = true;
            academicYearDetailPage.InsetDayTable.GetLastRow().PM = true;
            academicYearDetailPage.Save();
            academicYearDetailPage.Refresh();

            //Verify update successfully
            Assert.AreEqual(true, academicYearDetailPage.IsSuccessMessageDisplay(), "Update academic unsuccessfully");

            //Search academic year was created
            academicYearTriplet.SearchCriteria.AcademicYearName = academicYearName;
            academicYearResult = academicYearTriplet.SearchCriteria.Search();
            academicYearTarget = academicYearResult.SingleOrDefault(p => p.AcademicYear.Contains(academicYearName));
            academicYearDetailPage = academicYearTarget.Click<AcademicYearDetailPage>();

            //Verify that update academic year succesfully
            Assert.AreEqual(academicYearName, academicYearDetailPage.Name, "Academic year name is not equals");
            Assert.AreEqual(assessmentYear, academicYearDetailPage.AssessmentYear, "Assessment year is not equals");

            //Verify that record in School Term is correct
            var schoolTermTable = academicYearDetailPage.SchoolTermsTable;
            Assert.AreNotEqual(null, (schoolTermTable.Rows.SingleOrDefault(x => x.Name == schoolTermName && x.StartDate == schoolTermStartDate &&
                x.EndDate == schoolTermEndDate)), "School Term data is not equal");

            //Verify that record in INSERT Day is correct
            var insetDayTable = academicYearDetailPage.InsetDayTable;
            Assert.AreNotEqual(null, (insetDayTable.Rows.SingleOrDefault(x => x.Name == insertDayNameUpdate && x.Date == insertDayDateFuture))
                , "INSERT DAY data is not equal");

            // Delete academic year
            academicYearDetailPage.Delete();

            #endregion
        }

        /// <summary>
        /// TC MSC21
        /// Au : An Nguyen
        /// Description: Change Working Week pattern for current academic year
        /// Role: School Administrator
        /// </summary>
        [WebDriverTest(TimeoutSeconds = 600, Enabled = false, Browsers = new[] { BrowserDefaults.Ie }, Groups = new[] { "g1" }, DataProvider = "TC_MSC21_Data")]
        public void TC_MSC21_Change_Working_Week_pattern_for_current_academic_year(string currentAcademic, string firstDay)
        {
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);

            #region Test

            //Navigate to Academic Years
            AutomationSugar.NavigateMenu("Tasks", "School Management", "Academic Years");
            var academicYearTriplet = new AcademicYearTriplet();

            //Search current academic year
            academicYearTriplet.Refresh();
            academicYearTriplet.SearchCriteria.AcademicYearName = currentAcademic;
            var academicSearchResult = academicYearTriplet.SearchCriteria.Search();
            var academicYearTile = academicSearchResult.FirstOrDefault(t => t.AcademicYear.Equals(currentAcademic));
            var academicYearPage = academicYearTile.Click<AcademicYearDetailPage>();

            //Scroll to working week
            academicYearPage.ScrollToWorkingWeek();

            //Change first day of working week
            academicYearPage.FirstDayOfWorkingWeek = firstDay;

            //Change value of working week table
            var workingWeekTable = academicYearPage.WorkingWeekGrid;
            var workingWeekAM = workingWeekTable.Rows.FirstOrDefault(t => t.WorkingWeekSession.Equals("AM"));
            workingWeekAM.Monday = false;
            workingWeekAM.Saturday = true;
            var workingWeekPM = workingWeekTable.Rows.FirstOrDefault(t => t.WorkingWeekSession.Equals("PM"));
            workingWeekPM.Tuesday = false;
            workingWeekPM.Saturday = true;

            //Save
            academicYearPage.Save();
            academicYearPage.Refresh();
            Assert.AreEqual(true, academicYearPage.IsSuccessMessageDisplay(), "Save changed Academic year unsuccessfull");

            //Run searching again
            academicYearTriplet.Refresh();
            academicYearTriplet.SearchCriteria.AcademicYearName = currentAcademic;
            academicSearchResult = academicYearTriplet.SearchCriteria.Search();
            academicYearTile = academicSearchResult.FirstOrDefault(t => t.AcademicYear.Equals(currentAcademic));
            academicYearPage = academicYearTile.Click<AcademicYearDetailPage>();

            //Verify first of working week
            academicYearPage.ScrollToWorkingWeek();
            Assert.AreEqual(firstDay, academicYearPage.FirstDayOfWorkingWeek, "First Day of Working Week does not change");

            //Verify data of worrking week table
            workingWeekTable = academicYearPage.WorkingWeekGrid;
            workingWeekAM = workingWeekTable.Rows.FirstOrDefault(t => t.WorkingWeekSession.Equals("AM"));
            Assert.AreEqual(false, workingWeekAM.Monday, "AM session on Monday does not change");
            Assert.AreEqual(true, workingWeekAM.Saturday, "AM session on Satturday does not change");
            workingWeekPM = workingWeekTable.Rows.FirstOrDefault(t => t.WorkingWeekSession.Equals("PM"));
            Assert.AreEqual(false, workingWeekPM.Tuesday, "PM session on Tuesday does not change");
            Assert.AreEqual(true, workingWeekPM.Saturday, "PM session on Satturday does not change");

            #endregion

            #region Post-Condition

            //Resume data
            workingWeekAM.Monday = true;
            workingWeekAM.Saturday = false;
            workingWeekPM.Tuesday = true;
            workingWeekPM.Saturday = false;
            academicYearPage.FirstDayOfWorkingWeek = "Monday";
            academicYearPage.Save();

            #endregion
        }

        /// <summary>
        /// Author: Hieu Pham
        /// Description: As School Administrator : Add details of new Curriculum Years to be taught at the school from a future academic year
        /// Status: PASS
        /// </summary>
        [WebDriverTest(TimeoutSeconds = 1800, Enabled = false, Browsers = new[] { BrowserDefaults.Ie }, Groups = new[] { "g1" }, DataProvider = "TC_MSC22_Data")]
        public void TC_MSC22_Add_Detail_New_Curriculum_Year_To_Be_Taught_At_The_School_From_Future_Academic_Year(string NCYear, string startDate)
        {
            #region Steps

            // Login as School Adminstrator
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);

            // Navigate to Tasks->School Management -> Other School Details
            AutomationSugar.NavigateMenu("Tasks", "School Management", "My School Details");

            var mySchoolDetailPage = new MySchoolDetailsPage();

            // Scroll to Curriculum Year
            mySchoolDetailPage.ScrollToCurriculumYear();

            var curriculumTable = mySchoolDetailPage.CurriculumYears;

            // Delete Curriculum if existed
            var existedRow = curriculumTable.Rows.FirstOrDefault(x => x.NCYear.Equals(NCYear));
            mySchoolDetailPage.DeleteGridRowIfExist(existedRow);

            // Save record
            mySchoolDetailPage.Save();

            // Scroll to Curriculum Year
            mySchoolDetailPage.Refresh();
            mySchoolDetailPage.ScrollToCurriculumYear();

            // Enter value to empty row
            curriculumTable.Refresh();
            var emptyRow = curriculumTable.Rows.FirstOrDefault(x => x.NCYear.Equals(String.Empty));
            emptyRow.NCYear = NCYear;
            emptyRow.StartDate = startDate;

            // Save value
            mySchoolDetailPage.Save();

            // VP : Curriculum years to be added and saved successfully for the next academic year
            Assert.AreEqual(true, mySchoolDetailPage.IsSuccessMessageDisplay(), "Success message does not display");

            #endregion

            #region Post-condition

            // Scroll to Curriculum Year
            mySchoolDetailPage.Refresh();
            mySchoolDetailPage.ScrollToCurriculumYear();

            // Delete new row in Curriculum Year
            curriculumTable.Refresh();
            existedRow = curriculumTable.Rows.FirstOrDefault(x => x.NCYear.Equals(NCYear));
            mySchoolDetailPage.DeleteGridRowIfExist(existedRow);

            // Save record
            mySchoolDetailPage.Save();

            #endregion
        }

        /// <summary>
        /// Author: Hieu Pham
        /// Description: As School Administrator : Ensure that when changes are made to academic years in regard to terms dates, holiday dates, inset days, they are correctly and automatically reflected in the  Attendance Register
        /// Status: PASS
        /// </summary>
        [WebDriverTest(TimeoutSeconds = 1800, Enabled = false, Browsers = new[] { BrowserDefaults.Ie }, Groups = new[] { "g1" }, DataProvider = "TC_MSC23_Data")]
        public void TC_MSC23_Ensure_That_Changes_Are_Made_To_Academic_Years_In_Regard_Automatic_Reflected_In_Attendance_Register(string academicYear, string holidayName, string holidayDate, string className, string pupilName)
        {
            #region Pre-condition

            // Login as School Adminstrator
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);

            // Navigate to Tasks->School Management -> Other School Details
            AutomationSugar.NavigateMenu("Tasks", "School Management", "Academic Years");

            // Search academic year
            var academicYearTriplet = new AcademicYearTriplet();
            academicYearTriplet.SearchCriteria.AcademicYearName = academicYear;
            var academicYearPage = academicYearTriplet.SearchCriteria.Search().FirstOrDefault(x => x.AcademicYear.Equals(academicYear)).Click<AcademicYearDetailPage>();

            // Delete Holiday if exist
            academicYearPage.ScrollToHoliday();
            var schoolHolidayTable = academicYearPage.SchoolHolidayTable;
            var row = schoolHolidayTable.Rows.FirstOrDefault(x => x.StartDate.Equals(holidayDate));
            academicYearPage.DeleteRowIfExisted(row);

            // Add an school holiday           
            academicYearPage.ClickAddSchoolHolidayLink();
            schoolHolidayTable.Refresh();
            row = schoolHolidayTable.Rows.FirstOrDefault(x => x.Name.Equals(String.Empty));
            row.Name = holidayName;
            row.StartDate = holidayDate;
            schoolHolidayTable.Refresh();
            row = schoolHolidayTable.Rows.FirstOrDefault(x => x.Name.Equals(holidayName));
            row.EndDate = holidayDate;

            // Save values
            academicYearPage.Save();

            #endregion

            #region Steps

            // Navigate to Edit mark
            SeleniumHelper.NavigateQuickLink("Edit Marks");

            var editMarkTriplet = new EditMarksTriplet();

            // Search an edit mark record for a date
            editMarkTriplet.SearchCriteria.StartDate = holidayDate;
            editMarkTriplet.SearchCriteria.Day = true;
            editMarkTriplet.SearchCriteria.Class[className].Select = true;
            var editMarkPage = editMarkTriplet.SearchCriteria.Search<EditMarksPage>();
            var editMarkTable = editMarkPage.Marks;

            // VP : There is "#" on inset date
            Assert.AreEqual("#", editMarkTable[pupilName][1].Text, "Inset Date does not have '#' character");
            Assert.AreEqual("#", editMarkTable[pupilName][2].Text, "Inset Date does not have '#' character");

            // Navigate to Tasks->School Management -> Other School Details
            AutomationSugar.NavigateMenu("Tasks", "School Management", "Academic Years");

            // Search academic year
            academicYearTriplet = new AcademicYearTriplet();
            academicYearTriplet.SearchCriteria.AcademicYearName = academicYear;
            academicYearPage = academicYearTriplet.SearchCriteria.Search().FirstOrDefault(x => x.AcademicYear.Equals(academicYear)).Click<AcademicYearDetailPage>();

            // Delete an holiday
            academicYearPage.ScrollToHoliday();
            schoolHolidayTable = academicYearPage.SchoolHolidayTable;
            row = schoolHolidayTable.Rows.FirstOrDefault(x => x.Name.Equals(holidayName));
            row.DeleteRow();

            // Save value
            academicYearPage.Save();

            // Navigate to Edit mark
            SeleniumHelper.NavigateQuickLink("Edit Marks");

            editMarkTriplet = new EditMarksTriplet();

            // Search an edit mark record for a date
            editMarkTriplet.SearchCriteria.StartDate = holidayDate;
            editMarkTriplet.SearchCriteria.Day = true;
            editMarkTriplet.SearchCriteria.Class[className].Select = true;
            editMarkPage = editMarkTriplet.SearchCriteria.Search<EditMarksPage>();
            editMarkTable = editMarkPage.Marks;

            // VP : Check that the changes made to Academic Years that affects the days registration should be taken for (e.g. delete an inset day already, Extend a school holiday) automatically and correctly reflect in the Attendance Register 
            Assert.AreNotEqual("#", editMarkTable[pupilName][1].Text.Trim(), "Attendance Register does not update");
            Assert.AreNotEqual("#", editMarkTable[pupilName][2].Text.Trim(), "Attendance Register does not update");

            #endregion
        }

        #region DATA
        public List<object[]> TC_MSC01_Data()
        {
            string random = SeleniumHelper.GenerateRandomString(8);
            string schoolName = "Logigear_" + random;
            string teacher = "Teacher_" + random;

            string buildingNo = SeleniumHelper.GenerateRandomNumberInString(5);
            string buildingName = "Building_" + random;
            string postCode = "EC1A 1BB";
            string country = "United Kingdom";

            var res = new List<Object[]>
            {                
                new object[] 
                {
                    // Basic Detail
                    new string[]{ schoolName, teacher},
                    // Address Detail
                    new string[]{ buildingNo, buildingName, postCode, country }
                }
            };
            return res;
        }

        public List<object[]> TC_MSC02_Data()
        {
            string random = SeleniumHelper.GenerateRandomString(8);
            string schoolName = "Logigear_" + random;
            string teacher = "Teacher_" + random;

            var res = new List<Object[]>
            {                
                new object[] 
                {
                    // Basic Detail
                    new string[]{ schoolName, teacher},
                    "Adams, Laura"
                }
            };
            return res;
        }

        public List<object[]> TC_MSC03_Data()
        {
            string random = SeleniumHelper.GenerateRandomString(8);
            string schoolName = "Logigear_" + random;
            string teacher = "Teacher_" + random;

            var res = new List<Object[]>
            {                
                new object[] 
                {
                    // Basic Detail
                    new string[]{ schoolName, teacher },
                    "Adams, Laura", "2015/2016"
                }
            };
            return res;
        }

        public List<object[]> TC_MSC04_Data()
        {

            string random = SeleniumHelper.GenerateRandomString(8);
            string schoolName = "Logigear_" + random;
            string teacher = "Teacher_" + random;

            string schoolNameAmend = "Logigear_Amended_" + random;
            string teacherAmend = "Teacher_Amended_" + random;

            string buildingNo = SeleniumHelper.GenerateRandomNumberInString(5);
            string buildingName = "Bulding_" + SeleniumHelper.GenerateRandomString(8);
            string postcode = "EC1A 1BB";
            string country = "United Kingdom";

            string buildingNoAmend = "999" + SeleniumHelper.GenerateRandomNumberInString(5);
            string buildingNameAmend = "Bulding_Amend_" + SeleniumHelper.GenerateRandomString(8);

            string telephoneNumber = SeleniumHelper.GenerateRandomNumberInString(10);
            string email = random + "@capita.co.uk";
            string fax = "Fax_" + random;
            string website = "http://www.capita.co.uk";

            var res = new List<Object[]>
            {                
                new object[] 
                {
                    // Basic Detail
                    new string[]{ schoolName, teacher },
                    // Address Detail
                    new string[] { buildingNo, buildingName, postcode, country },
                    // Basic Detail amended
                    new string[] { schoolNameAmend, teacherAmend },
                    // Address Detail Amended
                    new string[] { buildingNoAmend, buildingNameAmend, postcode, country },
                    // Contact Detail
                    new string[] { telephoneNumber, fax, email, website },
                    "Adams, Laura"
                }
            };
            return res;
        }

        public List<object[]> TC_MSC05_Data()
        {

            string random = SeleniumHelper.GenerateRandomString(8);
            string schoolName = "Logigear_" + random;
            string teacher = "Teacher_" + random;

            var res = new List<Object[]>
            {                
                new object[] 
                {
                    // Basic Detail
                    new string[]{ schoolName, teacher },
                    "Adams, Laura"
                }
            };
            return res;
        }

        public List<object[]> TC_MSC06_Data()
        {
            var res = new List<Object[]>
            {                
                new object[] 
                {
                    new string[]{String.Format("Logigear_{0}", SeleniumHelper.GenerateRandomString(10)), String.Format("Head Teacher {0}", SeleniumHelper.GenerateRandomString(5))},
                    new string[]{"Abcd1", "VNPT", "EC1A 1BB"}
                }
            };
            return res;
        }

        public List<object[]> TC_MSC07_Data()
        {
            var res = new List<Object[]>
            {                
                new object[] 
                {
                    new string[]{"Water's Edge Primary School", String.Empty,"Special", "English Medium","Coeducational"},
                    new string[]{"Causeway Road", "BUSHMILLS"},
                    new string[]{"028 2073 7890", String.Empty, String.Empty},
                    new string[]{"Water's Edge Primary School", "Update", "Comprehensive", "Irish Medium","Male (only)"},
                    new string[]{"Causeway Road_Update", "BUSHMILLS_Update"},
                    new string[]{"0973378623", "12345678", "logigear@gmail.com"},
                    new string[]{SeleniumHelper.GenerateRandomNumberInString(2), "Destination"},
                    new string[]{ "Curriculum Year 8",DateTime.Now.ToString("M/d/yyyy")},
                }
            };
            return res;
        }

        public List<object[]> TC_MSC07_Data_NI()
        {
            var res = new List<Object[]>
            {                
                new object[] 
                {
                    //new string[]{"Water's Edge Primary School", String.Empty, "1019999","Belfast","English Medium","Coeducational"},  
                     //new string[]{"Water's Edge Primary School", String.Empty,"English Medium","Coeducational"}, 
                     new string[]{"Water's Edge Primary School", String.Empty,"Special","Irish Medium","Coeducational"},
           
      
                }
            };
            return res;
        }

        public List<object[]> TC_MSC07_Data_Eng()
        {
            var res = new List<Object[]>
            {                
                new object[] 
                {
                    //new string[]{"ABryn Primary School", String.Empty, "6789","Primary Test","Maintained Nursery","2","3","English Medium","Coeducational"},
                     new string[]{"WATERS EDGE PRIMARY SCHOOL", String.Empty, String.Empty,"Primary","Maintained Nursery","Comprehensive","2","3","Coeducational"},
               
                    //new string[]{"ABryn Primary School", "Update", "Comprehensive", "Irish Medium","Male (only)"},
                }
            };
            return res;
        }

        public List<object[]> TC_MSC07_Data_Wel()
        {
            var res = new List<Object[]>
            {                
                new object[] 
                {
                    //new string[]{"ABryn Primary School", String.Empty, "6789","Primary Test","Maintained Nursery","2","3","English Medium","Coeducational"},
                     new string[]{"YSGOL WATERS EDGE", "Gillian Grosvenor", "2999","Primary","Nursery School","Comprehensive","F0001","2","3","Coeducational"},
               
                    //new string[]{"ABryn Primary School", "Update", "Comprehensive", "Irish Medium","Male (only)"},
                }
            };
            return res;
        }


        public List<object[]> TC_MSC08_Data()
        {
            string pupilName = String.Format("MSC08_{0}_{1}", SeleniumHelper.GenerateRandomString(10), SeleniumHelper.GenerateRandomNumberUsingDateTime(5));
            var res = new List<Object[]>
            {                
                new object[] 
                {
                    new string[]{pupilName, pupilName,
                                "Female", DateTime.Now.AddYears(-10).ToString("M/d/yyyy"), 
                                DateTime.Now.ToString("M/d/yyyy"), "Guest Pupil", "Year 1"},
                }
            };
            return res;
        }

        public List<object[]> TC_MSC09_Data()
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

        public List<object[]> TC_MSC12_Data()
        {
            string pattern = "M/d/yyyy";
            string startSchoolTerm = DateTime.ParseExact("01/01/2024", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            string endSchoolTerm = DateTime.ParseExact("08/08/2024", pattern, CultureInfo.InvariantCulture).ToString(pattern);

            string startSchoolTerm2 = DateTime.ParseExact("08/16/2024", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            string endSchoolTerm2 = DateTime.ParseExact("10/20/2024", pattern, CultureInfo.InvariantCulture).ToString(pattern);

            string startSchoolHoliday = DateTime.ParseExact("03/03/2024", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            string endSchoolHoliday = DateTime.ParseExact("04/04/2024", pattern, CultureInfo.InvariantCulture).ToString(pattern);

            string publicHolidayDate = DateTime.ParseExact("05/05/2024", pattern, CultureInfo.InvariantCulture).ToString(pattern);

            string InsetDay = DateTime.ParseExact("06/06/2024", pattern, CultureInfo.InvariantCulture).ToString(pattern);

            string startGenDate = DateTime.ParseExact("08/09/2024", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            string endGenDate = DateTime.ParseExact("08/15/2024", pattern, CultureInfo.InvariantCulture).ToString(pattern);

            var res = new List<Object[]>
            {                
                new object[] 
                {
                    String.Format("LogigearYTa2425_{0}", SeleniumHelper.GenerateRandomString(6)),
                    "2024/2025",
                     new string[]{"SchoolTerm1", startSchoolTerm, endSchoolTerm, "SchoolTerm2", startSchoolTerm2, endSchoolTerm2},
                     new string[]{"SchoolHoliday", startSchoolHoliday, endSchoolHoliday},
                     new string[]{"PublicHoliday", publicHolidayDate},
                     new string[]{"INSET Day", InsetDay},
                     new string[]{"School Holiday",startGenDate, endGenDate}
                }
            };
            return res;
        }

        public List<object[]> TC_MSC13_Data()
        {
            string pattern = "M/d/yyyy";
            string fullName = SeleniumHelper.GetAcademicYear(DateTime.Now.AddYears(15));
            string academicYear = SeleniumHelper.GetAcademicYear(DateTime.Now.AddYears(15)).Replace("Academic Year", "").Trim();
            string schoolTerm = "Year Term";
            string schoolTermStart = SeleniumHelper.GetStartDateAcademicYear(DateTime.Now.AddYears(15)).ToString(pattern);
            string schoolTermEnd = SeleniumHelper.GetFinishDateAcademicYear(DateTime.Now.AddYears(16)).ToString(pattern);
            var data = new List<Object[]>
            {    
                new object[]{
                    fullName, academicYear, 
                    new string[]{schoolTerm, schoolTermStart, schoolTermEnd},
                }
            };
            return data;
        }

        public List<object[]> TC_MSC14_Data()
        {
            string pattern = "M/d/yyyy";
            string startSchoolTerm = DateTime.ParseExact("01/01/2025", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            string endSchoolTerm = DateTime.ParseExact("08/08/2025", pattern, CultureInfo.InvariantCulture).ToString(pattern);

            string startSchoolTerm2 = DateTime.ParseExact("08/16/2025", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            string endSchoolTerm2 = DateTime.ParseExact("10/20/2025", pattern, CultureInfo.InvariantCulture).ToString(pattern);

            string startSchoolHoliday = DateTime.ParseExact("03/03/2025", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            string endSchoolHoliday = DateTime.ParseExact("04/04/2025", pattern, CultureInfo.InvariantCulture).ToString(pattern);

            string publicHolidayDate = DateTime.ParseExact("05/05/2025", pattern, CultureInfo.InvariantCulture).ToString(pattern);

            string InsetDay = DateTime.ParseExact("06/06/2025", pattern, CultureInfo.InvariantCulture).ToString(pattern);

            string startGenDate = DateTime.ParseExact("08/09/2025", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            string endGenDate = DateTime.ParseExact("08/15/2025", pattern, CultureInfo.InvariantCulture).ToString(pattern);

            var res = new List<Object[]>
            {                
                new object[] 
                {
                    String.Format("LogigearYTa2526_{0}", SeleniumHelper.GenerateRandomString(6)),
                    "2025/2026",
                     new string[]{"SchoolTerm", startSchoolTerm, endSchoolTerm , "SchoolTerm2", startSchoolTerm2, endSchoolTerm2},
                     new string[]{"SchoolHoliday", startSchoolHoliday, endSchoolHoliday},
                     new string[]{"PublicHoliday", publicHolidayDate},
                     new string[]{"INSET Day", InsetDay},
                     new string[]{"School Holiday",startGenDate, endGenDate}
                }
            };
            return res;
        }

        public List<object[]> TC_MSC15_Data()
        {
            string pattern = "M/d/yyyy";
            string startSchoolTerm = DateTime.ParseExact("01/01/2026", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            string endSchoolTerm = DateTime.ParseExact("08/08/2026", pattern, CultureInfo.InvariantCulture).ToString(pattern);

            string startSchoolTerm2 = DateTime.ParseExact("08/16/2026", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            string endSchoolTerm2 = DateTime.ParseExact("10/20/2026", pattern, CultureInfo.InvariantCulture).ToString(pattern);

            string startSchoolHoliday = DateTime.ParseExact("03/03/2026", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            string endSchoolHoliday = DateTime.ParseExact("04/04/2026", pattern, CultureInfo.InvariantCulture).ToString(pattern);

            string publicHolidayDate = DateTime.ParseExact("05/05/2026", pattern, CultureInfo.InvariantCulture).ToString(pattern);

            string InsetDay = DateTime.ParseExact("06/06/2026", pattern, CultureInfo.InvariantCulture).ToString(pattern);

            string startGenDate = DateTime.ParseExact("08/09/2025", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            string endGenDate = DateTime.ParseExact("08/15/2025", pattern, CultureInfo.InvariantCulture).ToString(pattern);

            // Update fields
            string startSchoolTermUpdate = DateTime.ParseExact("10/21/2026", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            string endSchoolTermUpdate = DateTime.ParseExact("08/08/2027", pattern, CultureInfo.InvariantCulture).ToString(pattern);

            string startSchoolHolidayUpdate = DateTime.ParseExact("03/03/2027", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            string endSchoolHolidayUpdate = DateTime.ParseExact("04/04/2027", pattern, CultureInfo.InvariantCulture).ToString(pattern);

            string publicHolidayDateUpdate = DateTime.ParseExact("05/05/2027", pattern, CultureInfo.InvariantCulture).ToString(pattern);

            string InsetDayUpdate = DateTime.ParseExact("06/06/2027", pattern, CultureInfo.InvariantCulture).ToString(pattern);

            var res = new List<Object[]>
            {                
                new object[] 
                {
                    String.Format("LogigearYTa2627_{0}", SeleniumHelper.GenerateRandomString(6)),
                    "2026/2027",
                     new string[]{"SchoolTerm", startSchoolTerm, endSchoolTerm ,"SchoolTerm2", startSchoolTerm2, endSchoolTerm2},
                     new string[]{"SchoolHoliday", startSchoolHoliday, endSchoolHoliday},
                     new string[]{"PublicHoliday", publicHolidayDate},
                     new string[]{"INSET Day", InsetDay},

                      String.Format("LogigearYTaUpdate2728_{0}", SeleniumHelper.GenerateRandomString(6)),
                    "2027/2028",
                     new string[]{"SchoolTermUpdate", startSchoolTermUpdate, endSchoolTermUpdate},
                     new string[]{"SchoolHolidayUpdate", startSchoolHolidayUpdate, endSchoolHolidayUpdate},
                     new string[]{"PublicHolidayUpdate", publicHolidayDateUpdate},
                     new string[]{"INSET Day update", InsetDayUpdate},
                     new string[]{"School Holiday",startGenDate, endGenDate}
                }
            };
            return res;
        }

        public List<object[]> TC_MSC16_Data()
        {
            string dateOfSchoolTerm = SeleniumHelper.GetRandomDate(new DateTime(DateTime.Now.Year + 1, 7, 1), new DateTime(DateTime.Now.Year + 1, 7, 30)).ToString("M/d/yyyy");
            string dateOfSchoolHoliday = SeleniumHelper.GetRandomDate(new DateTime(DateTime.Now.Year + 1, 7, 1), new DateTime(DateTime.Now.Year + 1, 7, 30)).ToString("M/d/yyyy");

            var res = new List<Object[]>
            {                
                new object[] 
                {
                    String.Format("Academic Year {0}/{1}", DateTime.Now.Year, DateTime.Now.Year + 1),
                    new string[]{ String.Format("{0} Term", SeleniumHelper.GenerateRandomString(5)), dateOfSchoolTerm, dateOfSchoolTerm},
                    new string[]{ String.Format("{0} Holiday", SeleniumHelper.GenerateRandomString(5)), dateOfSchoolTerm, dateOfSchoolTerm}
                }
            };
            return res;
        }

        public List<object[]> TC_MSC19_Data()
        {
            string pattern = "M/d/yyyy";
            string academicYearName = SeleniumHelper.GenerateRandomString(5) + String.Format("{0}/{1}", (DateTime.Now.Year + 3).ToString(), (DateTime.Now.Year + 4).ToString());
            string assessmentYear = String.Format("{0}/{1}", (DateTime.Now.Year + 3).ToString(), (DateTime.Now.Year + 4).ToString());
            string schoolTermName = SeleniumHelper.GenerateRandomString(20);
            string schoolTermStartDate = new DateTime(DateTime.Today.Year + 3, 01, 11).ToString(pattern);
            string schoolTermEndDate = new DateTime(DateTime.Today.Year + 4, 01, 15).ToString(pattern);

            //Data for Public holiday
            string publicHolidayName = SeleniumHelper.GenerateRandomString(10);
            var publicHolidayStartDate = new DateTime(DateTime.Today.Year + 3, 01, 11).ToString(pattern);
            string publicHolidayNameUpdate = SeleniumHelper.GenerateRandomString(15);
            var publicHolidayEndDateFuture = new DateTime(DateTime.Today.Year + 3, 12, 12).ToString(pattern);


            var res = new List<Object[]>
            {                
                new object[] 
                {
                    academicYearName,assessmentYear,schoolTermName,schoolTermStartDate,schoolTermEndDate,
                    publicHolidayName,publicHolidayStartDate,publicHolidayNameUpdate,publicHolidayEndDateFuture
                }
            };
            return res;
        }

        public List<object[]> TC_MSC20_Data()
        {
            string pattern = "M/d/yyyy";

            string academicYearName = SeleniumHelper.GenerateRandomString(5) + String.Format("{0}/{1}", (DateTime.Now.Year + 3).ToString(), (DateTime.Now.Year + 4).ToString());
            string assessmentYear = String.Format("{0}/{1}", (DateTime.Now.Year + 3).ToString(), (DateTime.Now.Year + 4).ToString());
            string schoolTermName = SeleniumHelper.GenerateRandomString(20);
            string schoolTermStartDate = new DateTime(DateTime.Today.Year + 3, 01, 11).ToString(pattern);
            string schoolTermEndDate = new DateTime(DateTime.Today.Year + 4, 01, 15).ToString(pattern);

            //Data for INSERT DAY
            string insertDayName = SeleniumHelper.GenerateRandomString(10);
            var insertDayDate = new DateTime(DateTime.Today.Year + 3, 01, 11).ToString(pattern);
            string insertDayNameUpdate = SeleniumHelper.GenerateRandomString(15);
            var insertDayDateFuture = new DateTime(DateTime.Today.Year + 3, 12, 12).ToString(pattern);


            var res = new List<Object[]>
            {                
                new object[] 
                {
                    academicYearName,assessmentYear,schoolTermName,schoolTermStartDate,schoolTermEndDate,
                    insertDayName,insertDayDate,insertDayNameUpdate,insertDayDateFuture
                }
            };
            return res;
        }

        public List<object[]> TC_MSC21_Data()
        {
            string currentAcademic = SeleniumHelper.GetAcademicYear(DateTime.Now);
            string firstDay = "Wednesday";
            var data = new List<Object[]>
            {    
                new object[]{
                    currentAcademic, firstDay,
                }
            };
            return data;
        }

        public List<object[]> TC_MSC22_Data()
        {
            string NCYear = "Curriculum Year 8";
            string startDate = new DateTime(DateTime.Now.Year + 1, 8, 31).ToString("M/d/yyyy");

            var res = new List<Object[]>
            {                
                new object[] 
                {
                    NCYear, startDate
                }
            };
            return res;
        }

        public List<object[]> TC_MSC23_Data()
        {

            string pattern = "M/d/yyyy";
            string academicYear = String.Format("Academic Year {0}/{1}", DateTime.Now.Year, DateTime.Now.Year + 1);
            string holidayName = "Holiday_" + SeleniumHelper.GenerateRandomString(6);
            string holidayDate = new DateTime(DateTime.Now.Year, 11, 11).ToString(pattern);
            string className = "1A";
            string pupilName = "Harris, Noel";

            var res = new List<Object[]>
            {                
                new object[] 
                {
                    academicYear, holidayName, holidayDate, className, pupilName
                }
            };
            return res;
        }

        #endregion
    }
}
