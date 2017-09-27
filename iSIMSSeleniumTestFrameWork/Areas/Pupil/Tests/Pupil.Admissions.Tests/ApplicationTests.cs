using System;
using System.Collections.Generic;
using System.Linq;
using POM.Helper;
using Pupil.Components.Common;
using WebDriverRunner.internals;
using NUnit.Framework;
using System.Globalization;
using TestSettings;
using POM.Components.Admission;
using POM.Components.Pupil;

namespace Pupil.Admissions.Tests
{
    public class ApplicationTests
    {
       
        /// <summary/>
        /// Author: Huy.Vo
        /// Des: Exercise ability to add 1st new 'Application' to an admission group, checking defaults and validation.
        [WebDriverTest(TimeoutSeconds = 2000, Enabled = true, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { PupilTestGroups.Application.Page, PupilTestGroups.Priority.Priority1 }, DataProvider = "TC_AD07_Data")]
        public void TC_AD07_Add_New_Applicant(string sureName, string middleName, string foreName, string gender, string dateOfBirth,
                                              string admissionGroup, string enrolmentStatus, string Class, string attendanceMode, string boarderStatus)
        {

            #region PRE-CONDITIONS:

            // Login as Admission 
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.AdmissionsOfficer);

            // Navigate to Application page
            SeleniumHelper.NavigateMenu("Tasks", "Admissions", "Applications");
            var applicantTriplet = new ApplicationTriplet();

            // Search to change to Admitted if existing Applicant
            applicantTriplet.SearchCriteria.SetStatus("Applied", true);
            applicantTriplet.SearchCriteria.SetStatus("Offered", true);
            applicantTriplet.SearchCriteria.SetStatus("Accepted", true);
            applicantTriplet.SearchCriteria.SearchByName = sureName;
            var searchResults = applicantTriplet.SearchCriteria.Search();
            var applicantTile = searchResults.SingleOrDefault(t => t.Code.Equals(sureName + ", " + foreName));
            applicantTriplet.ChangeToAdmit(applicantTile);

            #endregion

            #region STEPS:

            // Create new an Applicant
            applicantTriplet = new ApplicationTriplet();
            var addNewApplicationDialog = applicantTriplet.AddNewApplicationDialog();
            addNewApplicationDialog.ForeName = foreName;
            addNewApplicationDialog.MiddleName = middleName;
            addNewApplicationDialog.SureName = sureName;
            addNewApplicationDialog.Gender = gender;
            addNewApplicationDialog.DateofBirth = dateOfBirth;
            addNewApplicationDialog.Continue();

            // Navigate to Registration Details dialog
            var registrationDetailsDialog = new RegistrationDetailsDialog();
            registrationDetailsDialog.AdmissionGroup = admissionGroup;
            registrationDetailsDialog.EnrolmentStatus = enrolmentStatus;
            registrationDetailsDialog.Class = Class;
            registrationDetailsDialog.AttendanceMode = attendanceMode;
            registrationDetailsDialog.BoarderStatus = boarderStatus;
            registrationDetailsDialog.CreateRecord();
            
            var applicationPage = ApplicationPage.Create();
            
            // Search Applicant that has just created
            applicantTriplet.SearchCriteria.SearchByName = sureName;
            searchResults = applicantTriplet.SearchCriteria.Search();
            applicantTile = searchResults.SingleOrDefault(t => t.Code.Equals(sureName + ", " + foreName));
            applicationPage = applicantTile.Click<ApplicationPage>();

            // Verify that Applicant was created successfully
            Assert.AreEqual(foreName, applicationPage.LegalForeName, "LegalForName is not equal");
            Assert.AreEqual(sureName, applicationPage.LegalSureName, "LegalSurName is not equal");
            Assert.AreEqual(gender, applicationPage.Gender, "Gender is not equal");
            Assert.AreEqual(dateOfBirth, applicationPage.DateOfBirth, "Date of Birth is not equal");
            Assert.AreEqual(SeleniumHelper.GetMonthsAndYears(dateOfBirth), applicationPage.Age, "Ages is not equal");
            Assert.AreEqual("Applied", applicationPage.ApplicationStatus, "Application Status is not equal");
            Assert.AreEqual(admissionGroup, applicationPage.AdmissionGroup, "Admission Group is not equal");
            Assert.AreEqual("", applicationPage.AdmissionNumber, "Admission number is not equal");
            Assert.AreEqual(true, applicationPage.VerifyDisable(), "Age and Age On Entry are not disable");

            //Change Applicant to Admitted
            applicationPage.ApplicationStatus = "Admitted";
            applicationPage.ClickSave();
            var confirmChangeStatusDialog = new ConfirmRequiredChangeStatus();
            confirmChangeStatusDialog.ConfirmContinueChangeStatus();

            #endregion

        }

        /// <summary>
        /// Author: Huy.Vo
        /// Des: Confirm the correct 'Applicants' are output in a report based on their current 'Application Status'.
        /// </summary>
        ///
        [WebDriverTest(TimeoutSeconds = 2000, Enabled = true, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { PupilTestGroups.Application.Page, PupilTestGroups.Priority.Priority1 }, DataProvider = "TC_AD08_Data")]
        public void TC_AD08_Add_New_Applicant_View_Report(string sureName, string middleName, string foreName, string gender, string dateOfBirth,
                                                          string admissionGroup, string enrolmentStatus, string Class, string attendanceMode, string boarderStatus)
        {
            #region PRE-CONDITIONS:

            // Login as Admission 
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.AdmissionsOfficer);

            // Navigate to Application page
            SeleniumHelper.NavigateMenu("Tasks", "Admissions", "Applications");
            var applicantTriplet = new ApplicationTriplet();

            //Search the Applicant is existing to change to Admitted
            applicantTriplet.SearchCriteria.SetStatus("Applied", true);
            applicantTriplet.SearchCriteria.SetStatus("Offered", true);
            applicantTriplet.SearchCriteria.SetStatus("Accepted", true);
            applicantTriplet.SearchCriteria.SearchByName = sureName;
            var searchResults = applicantTriplet.SearchCriteria.Search();
            var applicantTile = searchResults.SingleOrDefault(t => t.Code.Equals(sureName + ", " + foreName));
            applicantTriplet.ChangeToAdmit(applicantTile);

            #endregion

            #region STEPS:

            // Create new an Applicant
            var addNewApplicationDialog = applicantTriplet.AddNewApplicationDialog();
            addNewApplicationDialog.ForeName = foreName;
            addNewApplicationDialog.MiddleName = middleName;
            addNewApplicationDialog.SureName = sureName;
            addNewApplicationDialog.Gender = gender;
            addNewApplicationDialog.DateofBirth = dateOfBirth;

            // Add new informations on RegistrationDetails Dialog
            addNewApplicationDialog.Continue();
            // addNewApplicationDialog.ContiueCreateApplicant();
            var registrationDetailsDialog = new RegistrationDetailsDialog();
            registrationDetailsDialog.AdmissionGroup = admissionGroup;
            registrationDetailsDialog.EnrolmentStatus = enrolmentStatus;
            registrationDetailsDialog.Class = Class;
            registrationDetailsDialog.AttendanceMode = attendanceMode;
            registrationDetailsDialog.BoarderStatus = boarderStatus;
            registrationDetailsDialog.CreateRecord();
            var applicationPage = ApplicationPage.Create();
           
            applicantTriplet.SearchCriteria.SetStatus("Offered", false);
            applicantTriplet.SearchCriteria.SetStatus("Accepted", false);
            applicantTriplet.SearchCriteria.SearchByName = sureName;
            searchResults = applicantTriplet.SearchCriteria.Search();
            applicantTile = searchResults.SingleOrDefault(t => t.Code.Equals(sureName + ", " + foreName));
            applicationPage = applicantTile.Click<ApplicationPage>();

            // Verify that Applicant was created successfully
            Assert.AreEqual(foreName, applicationPage.LegalForeName, "LegalForName is not equal");
            Assert.AreEqual(sureName, applicationPage.LegalSureName, "LegalSurName is not equal");
            Assert.AreEqual(gender, applicationPage.Gender, "Gender is not equal");
            Assert.AreEqual(dateOfBirth, applicationPage.DateOfBirth, "Date of Birth is not equal");
            Assert.AreEqual(SeleniumHelper.GetMonthsAndYears(dateOfBirth), applicationPage.Age, "Ages is not equal");
            Assert.AreEqual("Applied", applicationPage.ApplicationStatus, "Application Status is not equal");
            Assert.AreEqual(admissionGroup, applicationPage.AdmissionGroup, "Admission Group is not equal");
            Assert.AreEqual(enrolmentStatus, applicationPage.ProposedEnrolmentStatus, "Enrolment Status is not equal");
            Assert.AreEqual("", applicationPage.AdmissionNumber, "Admission number is not equal");
            Assert.AreEqual(true, applicationPage.VerifyDisable(), "Age and Age On Entry are not disable");

            //Change Applicant to Admitted
            applicationPage.ApplicationStatus = "Admitted";
            applicationPage.ClickSave();
            var confirmChangeStatusDialog = new ConfirmRequiredChangeStatus();
            confirmChangeStatusDialog.ConfirmContinueChangeStatus();

            // Missing report functional

            #endregion
        }

       
        #region DATA

        public List<object[]> TC_AD07_Data()
        {
            string pattern = "M/d/yyyy";
            string dateOfBirth = DateTime.ParseExact("07/29/1982", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            string sureName = "SureNameTC07 " + SeleniumHelper.GenerateRandomString(10);
            string middleName = "MiddleNameTC07 " + SeleniumHelper.GenerateRandomString(10);
            string foreName = "ForeNameTC07 " + SeleniumHelper.GenerateRandomString(10);
            var res = new List<Object[]>
            {                
                new object[] 
                {
                    // SurName
                    sureName,
                    // Middle Name
                    middleName,
                    //ForeName
                   foreName,
                    // Gender
                    "Male",
                    // Date of Birth
                    dateOfBirth,
                    // Admission Group
                    "CES Hold N 15/16",
                    //Enrolment Status
                    "Guest Pupil",
                    //Class
                    "1A",
                    //Attendance Mode
                    "Full-Time",
                    //Boarder Status
                    "Boarder, nights per week not specified"
                }
            };
            return res;
        }
        public List<object[]> TC_AD08_Data()
        {
            string pattern = "M/d/yyyy";
            string sureName = "SureNameTC08 " + SeleniumHelper.GenerateRandomString(10);
            string middleName = "MiddleNameTC08 " + SeleniumHelper.GenerateRandomString(10);
            string foreName = "ForeNameTC08 " + SeleniumHelper.GenerateRandomString(10);

            string dateOfBirth = DateTime.ParseExact("06/12/2006", pattern, CultureInfo.InvariantCulture).ToString(pattern);

            var res = new List<Object[]>
            {                
                new object[] 
                {
                    // SurName
						sureName,
                    // Middle Name
						middleName,
                    //ForeName
						foreName,
                    // Gender
                    "Female",
                    // Date of Birth
                    dateOfBirth,
                    // Admission Group
                    "CES Hold N 15/16",
                    //Enrolment Status
                    "Single Registration",
                    //Class
                    "2A",
                    //Attendance Mode
                    "AM only",
                    //Boarder Status
                    "Not a Boarder"
                
                }
                
            };
            return res;
        }
        
        #endregion
    }
}