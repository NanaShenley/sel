using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using NUnit.Framework;
using POM.Components.Admission;
using POM.Components.Pupil;
using POM.Helper;

namespace Pupil.Admissions.Tests
{
    public class DisabledTests
    {

        #region Applications

        /// <summary>
        /// Author: Huy.Vo
        /// Des: Exercise ability to update an application including advancing their status from 'Applied' to 'Offered'.
        ///      Confirm the correct 'Applicants' are output in a report based on their current 'Application Status'.
        /// </summary>
        /// TODO: This test duplicates the TC_AD08. Hence, P3
        //[WebDriverTest(TimeoutSeconds = 3000, Enabled = true, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { PupilTestGroups.Application.ApplicationTests, PupilTestGroups.Severities.Priority3 }, DataProvider = "TC_AD09_Data")]
        public void TC_AD09_Update_Applicant(string sureName, string middleName, string foreName, string gender, string dateOfBirth,
                                             string admissionGroup, string enrolmentStatus, string Class, string attendanceMode, string boarderStatus,
                                             string dateOfBirthUpdate, bool birthCertificateSeen, string applicationStatus)
        {

            // Login as Admission 
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.AdmissionsOfficer);

            // Navigate to Application page
            SeleniumHelper.NavigateMenu("Tasks", "Admissions", "Applications");

            #region PRE-CONDITIONS:

            var applicantTriplet = new ApplicationTriplet();

            // Search to change to Admitted if existing Applicant
            applicantTriplet.SearchCriteria.SetStatus("Applied", true);
            applicantTriplet.SearchCriteria.SetStatus("Offered", true);
            applicantTriplet.SearchCriteria.SetStatus("Accepted", true);
            applicantTriplet.SearchCriteria.SearchByName = sureName;
            var searchResults = applicantTriplet.SearchCriteria.Search();
            var applicantTile = searchResults.SingleOrDefault(t => t.Code.Equals(sureName + ", " + foreName));
            applicantTriplet.ChangeToAdmit(applicantTile);

            // Create new an Applicant
            var addNewApplicationDialog = applicantTriplet.AddNewApplicationDialog();
            addNewApplicationDialog.ForeName = foreName;
            addNewApplicationDialog.MiddleName = middleName;
            addNewApplicationDialog.SureName = sureName;
            addNewApplicationDialog.Gender = gender;
            addNewApplicationDialog.DateofBirth = dateOfBirth;
            addNewApplicationDialog.Continue();
            //addNewApplicationDialog.ContiueCreateApplicant();
            var registrationDetailsDialog = new RegistrationDetailsDialog();
            registrationDetailsDialog.AdmissionGroup = admissionGroup;
            registrationDetailsDialog.EnrolmentStatus = enrolmentStatus;
            registrationDetailsDialog.Class = Class;
            registrationDetailsDialog.AttendanceMode = attendanceMode;
            registrationDetailsDialog.BoarderStatus = boarderStatus;
            registrationDetailsDialog.CreateRecord();
            ApplicationPage.Create();

            // Search Applicant that has just created
            applicantTriplet.SearchCriteria.SearchByName = sureName;
            searchResults = applicantTriplet.SearchCriteria.Search();
            applicantTile = searchResults.SingleOrDefault(t => t.Code.Equals(sureName + ", " + foreName));
            ApplicationPage applicationPage = applicantTile.Click<ApplicationPage>();

            #endregion

            #region STEPS:

            // Update Applicant
            applicationPage.DateOfBirth = dateOfBirthUpdate;
            applicationPage.BirthCertificateSeen = birthCertificateSeen;
            applicationPage.SelectRegistrationSection();
            applicationPage.GenerateUPN();
            applicationPage.ClickSave();

            // Verify Applicant after updating
            Assert.AreEqual(dateOfBirthUpdate, applicationPage.DateOfBirth, "Date of Birth is not equal");
            Assert.AreEqual(SeleniumHelper.GetMonthsAndYears(dateOfBirthUpdate), applicationPage.Age, "Ages is not equal");
            Assert.AreEqual(birthCertificateSeen, applicationPage.BirthCertificateSeen, "Date of Birth is not equal");
            Assert.AreEqual(false, applicationPage.UniquePupilNumber.Equals(""), "Unique Pupil Number s not equal");

            // Update Application status
            applicationPage.ApplicationStatus = applicationStatus;
            applicationPage.ClickSave();
            Assert.AreEqual(true, applicationPage.IsSuccessMessageIsDisplayed(), "Success message is not display");

            // Search Applicant was created (Blocked by data-automation id)
            applicantTriplet.SearchCriteria.SetStatus("Offered", false);
            applicantTriplet.SearchCriteria.SetStatus("Accepted", false);
            searchResults = applicantTriplet.SearchCriteria.Search();
            applicantTile = searchResults.SingleOrDefault(t => t.Code.Equals(sureName + ", " + foreName));

            // Verify that Appicant was changed to Offered status.
            Assert.AreEqual(null, applicantTile, "Change status to Offered was failed");

            // Re-search Applicant with Offered status
            applicantTriplet.SearchCriteria.SetStatus("Applied", false);
            applicantTriplet.SearchCriteria.SetStatus("Offered", true);
            searchResults = applicantTriplet.SearchCriteria.Search();
            applicantTile = searchResults.SingleOrDefault(t => t.Code.Equals(sureName + ", " + foreName));
            Assert.AreNotEqual(null, applicantTile, "Change status to Offered was failed");

            //Verify that Application Status was changed to Offered
            applicationPage = applicantTile.Click<ApplicationPage>();
            Assert.AreEqual(applicationStatus, applicationPage.ApplicationStatus, "Status is not equal");

            // Change Applicant to Admitted
            applicationPage.ApplicationStatus = "Admitted";
            applicationPage.ClickSave();
            var confirmChangeStatusDialog = new ConfirmRequiredChangeStatus();
            confirmChangeStatusDialog.ConfirmContinueChangeStatus();

            // Missing report functional

            #endregion STEPS
        }

        /// <summary>
        /// Author: Huy Vo
        /// Des: Exercise ability to update an application including advancing their status from 'Applied' to 'Offered'.
        /// Exercise ability to update an application including advancing their status from 'Offered' to 'Accepted'.
        /// </summary>
        /// <returns></returns>
        /// TODO: This test duplicates the TC_AD08. Hence, P3
        //[WebDriverTest(TimeoutSeconds = 3500, Enabled = true, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { PupilTestGroups.Application.ApplicationTests, PupilTestGroups.Severities.Priority3 }, DataProvider = "TC_AD10_Data")]
        public void TC_AD10_Update_Applicant(string sureName1, string middleName1, string foreName1, string gender1, string dateOfBirth1,
                                            string admissionGroup1, string enrolmentStatus1, string class1, string attendanceMode1, string boarderStatus1,
                                            string sureName2, string middleName2, string foreName2, string gender2, string dateOfBirth2,
                                            string admissionGroup2, string enrolmentStatus2, string class2, string attendanceMode2, string boarderStatus2, string applicationStatusUpdate)
        {
            // Login as Admission 
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.AdmissionsOfficer);

            // Navigate to Application page
            SeleniumHelper.NavigateMenu("Tasks", "Admissions", "Applications");

            #region PRE-CONDITIONS:
            var applicantTriplet = new ApplicationTriplet();

            // Search to change to Admitted if existing Applicant
            applicantTriplet.SearchCriteria.SetStatus("Applied", true);
            applicantTriplet.SearchCriteria.SetStatus("Offered", true);
            applicantTriplet.SearchCriteria.SetStatus("Accepted", true);
            applicantTriplet.SearchCriteria.SearchByName = sureName1;
            var searchResults = applicantTriplet.SearchCriteria.Search();
            var applicantTile = searchResults.SingleOrDefault(t => t.Code.Equals(sureName1 + ", " + foreName1));
            applicantTriplet.ChangeToAdmit(applicantTile);

            // Create new an Applicant 1
            var addNewApplicationDialog = applicantTriplet.AddNewApplicationDialog();
            addNewApplicationDialog.ForeName = foreName1;
            addNewApplicationDialog.MiddleName = middleName1;
            addNewApplicationDialog.SureName = sureName1;
            addNewApplicationDialog.Gender = gender1;
            addNewApplicationDialog.DateofBirth = dateOfBirth1;
            addNewApplicationDialog.Continue();
            // addNewApplicationDialog.ContiueCreateApplicant();
            var registrationDetailsDialog = new RegistrationDetailsDialog();
            registrationDetailsDialog.AdmissionGroup = admissionGroup1;
            registrationDetailsDialog.EnrolmentStatus = enrolmentStatus1;
            registrationDetailsDialog.Class = class1;
            registrationDetailsDialog.AttendanceMode = attendanceMode1;
            registrationDetailsDialog.BoarderStatus = boarderStatus1;
            registrationDetailsDialog.CreateRecord();

            // Search an change to Admitted for Applicant 2
            applicantTriplet.SearchCriteria.SetStatus("Applied", true);
            applicantTriplet.SearchCriteria.SetStatus("Offered", true);
            applicantTriplet.SearchCriteria.SetStatus("Accepted", true);
            applicantTriplet.SearchCriteria.SearchByName = sureName2;
            searchResults = applicantTriplet.SearchCriteria.Search();
            applicantTile = searchResults.SingleOrDefault(t => t.Code.Equals(sureName2 + ", " + foreName2));
            applicantTriplet.ChangeToAdmit(applicantTile);

            // Create new an Applicant 2
            addNewApplicationDialog = applicantTriplet.AddNewApplicationDialog();
            addNewApplicationDialog.ForeName = foreName2;
            addNewApplicationDialog.MiddleName = middleName2;
            addNewApplicationDialog.SureName = sureName2;
            addNewApplicationDialog.Gender = gender2;
            addNewApplicationDialog.DateofBirth = dateOfBirth2;

            // Add new informations on RegistrationDetails Dialog
            addNewApplicationDialog.Continue();
            //addNewApplicationDialog.ContiueCreateApplicant();

            registrationDetailsDialog = new RegistrationDetailsDialog();
            registrationDetailsDialog.AdmissionGroup = admissionGroup2;
            registrationDetailsDialog.EnrolmentStatus = enrolmentStatus2;
            registrationDetailsDialog.Class = class2;
            registrationDetailsDialog.AttendanceMode = attendanceMode2;
            registrationDetailsDialog.BoarderStatus = boarderStatus2;
            registrationDetailsDialog.CreateRecord();
            ApplicationPage.Create();


            // Change Applicant 2 from Applied to Offered
            applicantTriplet.SearchCriteria.SearchByName = sureName2;
            searchResults = applicantTriplet.SearchCriteria.Search();
            applicantTile = searchResults.SingleOrDefault(t => t.Code.Equals(sureName2 + ", " + foreName2));
            ApplicationPage applicationPage = applicantTile.Click<ApplicationPage>();
            applicationPage.ApplicationStatus = "Offered";
            applicationPage.ClickSave();

            #endregion PRE-CONDITIONS

            #region STEPS
            // Change Application status for Applicant 1          
            applicantTriplet.SearchCriteria.SetStatus("Offered", false);
            applicantTriplet.SearchCriteria.SetStatus("Accepted", false);
            applicantTriplet.SearchCriteria.SearchByName = sureName1;
            searchResults = applicantTriplet.SearchCriteria.Search();
            applicantTile = searchResults.SingleOrDefault(t => t.Code.Equals(sureName1 + ", " + foreName1));
            applicationPage = applicantTile.Click<ApplicationPage>();

            // Update Application status to Accepted
            applicationPage.ApplicationStatus = applicationStatusUpdate;
            applicationPage.ClickSave();

            // Verify change successfully
            Assert.AreEqual(true, applicationPage.IsSuccessMessageIsDisplayed(), "Success message is not display");

            // Change Application status of Applicant 2 to Offer to Accepted
            applicantTriplet.SearchCriteria.SetStatus("Applied", false);
            applicantTriplet.SearchCriteria.SetStatus("Offered", true);
            applicantTriplet.SearchCriteria.SetStatus("Accepted", false);
            applicantTriplet.SearchCriteria.SearchByName = sureName2;
            searchResults = applicantTriplet.SearchCriteria.Search();
            applicantTile = searchResults.SingleOrDefault(t => t.Code.Equals(sureName2 + ", " + foreName2));
            applicationPage = applicantTile.Click<ApplicationPage>();
            applicationPage.ApplicationStatus = applicationStatusUpdate;
            applicationPage.ClickSave();

            // Verify change successfully
            Assert.AreEqual(true, applicationPage.IsSuccessMessageIsDisplayed(), "Success message is not display");

            // Search Applicant by Accepted status
            applicantTriplet.SearchCriteria.SetStatus("Applied", false);
            applicantTriplet.SearchCriteria.SetStatus("Offered", false);
            applicantTriplet.SearchCriteria.SetStatus("Accepted", true);

            // Verify twice Applicants were changed to Accepted
            applicantTriplet.SearchCriteria.SearchByName = sureName1;
            searchResults = applicantTriplet.SearchCriteria.Search();
            applicantTile = searchResults.SingleOrDefault(t => t.Code.Equals(sureName1 + ", " + foreName1));
            Assert.AreNotEqual(null, applicantTile, "Change status to Offered was failed");
            applicationPage = applicantTile.Click<ApplicationPage>();
            Assert.AreEqual(applicationStatusUpdate, applicationPage.ApplicationStatus, "Status is not equal Accepted");

            // Change to Admitted for Applicant 1
            applicationPage.ApplicationStatus = "Admitted";
            applicationPage.ClickSave();

            // Continue confirm to change status
            var confirmChangeStatusDialog = new ConfirmRequiredChangeStatus();
            confirmChangeStatusDialog.ConfirmContinueChangeStatus();

            applicantTriplet.SearchCriteria.SearchByName = sureName2;
            searchResults = applicantTriplet.SearchCriteria.Search();
            applicantTile = searchResults.SingleOrDefault(t => t.Code.Equals(sureName2 + ", " + foreName2));
            Assert.AreNotEqual(null, applicantTile, "Change status to Offered was failed");
            applicationPage = applicantTile.Click<ApplicationPage>();
            Assert.AreEqual(applicationStatusUpdate, applicationPage.ApplicationStatus, "Status is not equal Accepted");

            // Change to Admitted for Applicant 2
            applicationPage.ApplicationStatus = "Admitted";
            applicationPage.ClickSave();
            // Continue confirm to change status
            confirmChangeStatusDialog = new ConfirmRequiredChangeStatus();
            confirmChangeStatusDialog.ConfirmContinueChangeStatus();

            // Missing report functional

            #endregion STEPS
        }

        /// <summary>
        /// Author: Huy Vo
        /// Des: Exercise ability to link applicants to a new contact so that the applicants and contact share an address.
        /// Exercise ability to establish applicant sibling links with a newly created shared parental responsibility contact.
        /// </summary>
        /// <returns></returns>
        /// TODO: New functionality to be implemented later. Hence P4
        //[WebDriverTest(TimeoutSeconds = 3000, Enabled = true, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { PupilTestGroups.Application.ApplicationTests, PupilTestGroups.Severities.Priority4 }, DataProvider = "TC_AD11_Data")]
        public void TC_AD11_Update_Applicant_Address_Home_Family(string sureName1, string middleName1, string foreName1, string gender1, string dateOfBirth1,
                                            string admissionGroup1, string enrolmentStatus1, string class1, string attendanceMode1, string boarderStatus1,
                                            string sureName2, string middleName2, string foreName2, string gender2, string dateOfBirth2,
                                            string admissionGroup2, string enrolmentStatus2, string class2, string attendanceMode2, string boarderStatus2,
                                            string houseBuildingNo, string houseBuildingName, string flatApartmentOffice, string street, string district, string townCity, string country, string postCode, string country1,
                                            string familySureName, string relationShip, bool parentalResponsibility)
        {
            // Login as Admission 
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.AdmissionsOfficer);

            #region PRE-CONDITIONS
            // Select Pupil record page to delete Pupil contact
            SeleniumHelper.NavigateQuickLink("Pupil Contacts");
            var pupilRecordTriplet = new PupilContactTriplet();
            pupilRecordTriplet.SearchCriteria.Surname = familySureName;
            var pupilContactResults = pupilRecordTriplet.SearchCriteria.Search();
            var pupilContactTile = pupilContactResults.FirstOrDefault(t => t.Name.Equals(familySureName));
            var pupilContactPage = pupilContactTile != null ? pupilContactTile.Click<PupilContactPage>() : new PupilContactPage();
            pupilContactPage.ClickDelete();
            pupilContactPage.ContinueDelete();

            // Navigate to Application page
            SeleniumHelper.NavigateMenu("Tasks", "Admissions", "Applications");
            var applicantTriplet = new ApplicationTriplet();

            // Search to change to Admitted if existing Applicant
            applicantTriplet.SearchCriteria.SetStatus("Applied", true);
            applicantTriplet.SearchCriteria.SetStatus("Offered", true);
            applicantTriplet.SearchCriteria.SetStatus("Accepted", true);
            applicantTriplet.SearchCriteria.SearchByName = sureName1;
            var searchResults = applicantTriplet.SearchCriteria.Search();
            var applicantTile = searchResults.SingleOrDefault(t => t.Code.Equals(sureName1 + ", " + foreName1));
            applicantTriplet.ChangeToAdmit(applicantTile);

            // Create new an Applicant 1
            var addNewApplicationDialog = applicantTriplet.AddNewApplicationDialog();
            addNewApplicationDialog.ForeName = foreName1;
            addNewApplicationDialog.MiddleName = middleName1;
            addNewApplicationDialog.SureName = sureName1;
            addNewApplicationDialog.Gender = gender1;
            addNewApplicationDialog.DateofBirth = dateOfBirth1;
            addNewApplicationDialog.Continue();
            // addNewApplicationDialog.ContiueCreateApplicant();
            var registrationDetailsDialog = new RegistrationDetailsDialog();
            registrationDetailsDialog.AdmissionGroup = admissionGroup1;
            registrationDetailsDialog.EnrolmentStatus = enrolmentStatus1;
            registrationDetailsDialog.Class = class1;
            registrationDetailsDialog.AttendanceMode = attendanceMode1;
            registrationDetailsDialog.BoarderStatus = boarderStatus1;
            registrationDetailsDialog.CreateRecord();
            var applicationPage = ApplicationPage.Create();
            //applicationPage.ApplicationStatus = "Accepted";
            applicationPage.ClickSave();

            // Create Applicant 2
            applicantTriplet.SearchCriteria.SetStatus("Applied", true);
            applicantTriplet.SearchCriteria.SetStatus("Offered", true);
            applicantTriplet.SearchCriteria.SetStatus("Accepted", true);
            applicantTriplet.SearchCriteria.SearchByName = sureName2;
            searchResults = applicantTriplet.SearchCriteria.Search();
            applicantTile = searchResults.SingleOrDefault(t => t.Code.Equals(sureName2 + ", " + foreName2));
            applicantTriplet.ChangeToAdmit(applicantTile);

            // Create new an Applicant 2
            addNewApplicationDialog = applicantTriplet.AddNewApplicationDialog();
            addNewApplicationDialog.ForeName = foreName2;
            addNewApplicationDialog.MiddleName = middleName2;
            addNewApplicationDialog.SureName = sureName2;
            addNewApplicationDialog.Gender = gender2;
            addNewApplicationDialog.DateofBirth = dateOfBirth2;
            #endregion PRO-CONDITIONS
            #region STEPS

            // Add new informations on RegistrationDetails Dialog
            addNewApplicationDialog.Continue();
            //  addNewApplicationDialog.ContiueCreateApplicant();

            registrationDetailsDialog = new RegistrationDetailsDialog();
            registrationDetailsDialog.AdmissionGroup = admissionGroup2;
            registrationDetailsDialog.EnrolmentStatus = enrolmentStatus2;
            registrationDetailsDialog.Class = class2;
            registrationDetailsDialog.AttendanceMode = attendanceMode2;
            registrationDetailsDialog.BoarderStatus = boarderStatus2;
            registrationDetailsDialog.CreateRecord();
            applicationPage = ApplicationPage.Create();
            //   applicationPage.ApplicationStatus = "Accepted";
            applicationPage.ClickSave();

            // Search Applicant 1 to update address
            applicantTriplet.SearchCriteria.SetStatus("Applied", true);
            applicantTriplet.SearchCriteria.SetStatus("Offered", false);
            applicantTriplet.SearchCriteria.SetStatus("Accepted", false);
            applicantTriplet.SearchCriteria.SearchByName = sureName1;
            searchResults = applicantTriplet.SearchCriteria.Search();
            applicantTile = searchResults.SingleOrDefault(t => t.Code.Equals(sureName1 + ", " + foreName1));

            applicationPage = applicantTile.Click<ApplicationPage>();

            // Select Applicant 1 and update Address informations
            var addAddressDialog = applicationPage.AddAddress();
            addAddressDialog.HouseBuildingNo = houseBuildingNo;
            addAddressDialog.HouseBuildingName = houseBuildingName;
            addAddressDialog.FlatApartmentOffice = flatApartmentOffice;
            addAddressDialog.Street = street;
            addAddressDialog.District = district;
            addAddressDialog.TownCity = townCity;
            addAddressDialog.Country = country;
            addAddressDialog.PostCode = postCode;
            addAddressDialog.CountryDR = country1;
            applicationPage = addAddressDialog.OK();
            applicationPage.ClickSave();

            // Search and select Applicant 2 and update Address informations
            applicantTriplet.SearchCriteria.SetStatus("Applied", true);
            applicantTriplet.SearchCriteria.SetStatus("Offered", false);
            applicantTriplet.SearchCriteria.SetStatus("Accepted", false);
            applicantTriplet.SearchCriteria.SearchByName = sureName2;
            searchResults = applicantTriplet.SearchCriteria.Search();
            applicantTile = searchResults.SingleOrDefault(t => t.Code.Equals(sureName2 + ", " + foreName2));
            applicationPage = applicantTile.Click<ApplicationPage>();

            // Add new address informations
            addAddressDialog = applicationPage.AddAddress();
            addAddressDialog.HouseBuildingNo = houseBuildingNo;
            addAddressDialog.HouseBuildingName = houseBuildingName;
            addAddressDialog.FlatApartmentOffice = flatApartmentOffice;
            addAddressDialog.Street = street;
            addAddressDialog.District = district;
            addAddressDialog.TownCity = townCity;
            addAddressDialog.Country = country;
            addAddressDialog.PostCode = postCode;
            addAddressDialog.CountryDR = country1;
            applicationPage = addAddressDialog.OK();
            applicationPage.ClickSave();

            // Update Family Home informations for Applicant 2
            var contactDialogTriplet = applicationPage.AddFamilyHome();
            var contactDialog = contactDialogTriplet.Create();
            contactDialog.SureName = familySureName;
            applicationPage = contactDialogTriplet.OK();

            var familyHomeTable = applicationPage.ContactsGrid;
            familyHomeTable[0].RelationShip = relationShip;
            familyHomeTable[0].ParentalResponsibility = parentalResponsibility;
            applicationPage.ClickSave();

            //Select Applicant 1 to update Contact
            applicantTriplet.SearchCriteria.SearchByName = sureName1;
            searchResults = applicantTriplet.SearchCriteria.Search();
            applicantTile = searchResults.SingleOrDefault(t => t.Code.Equals(sureName1 + ", " + foreName1));
            applicationPage = applicantTile.Click<ApplicationPage>();

            contactDialogTriplet = applicationPage.AddFamilyHome();
            contactDialogTriplet.SearchCriteria.SearchBySureName = familySureName;
            var searchSurename = contactDialogTriplet.SearchCriteria.Search();
            var sureNameTile = searchSurename.FirstOrDefault(t => t.Code.Equals(familySureName));
            sureNameTile.Click<ContactDialog>();

            // ContactDialogTriplet.SelectSearchTile(sureNameTile);
            applicationPage = contactDialogTriplet.OK();
            familyHomeTable = applicationPage.ContactsGrid;
            familyHomeTable[0].RelationShip = relationShip;
            familyHomeTable[0].ParentalResponsibility = parentalResponsibility;
            applicationPage.ClickSave();

            //Verify Family links between applicants has been established
            var familyLinkTable = applicationPage.FamilyLinksGrid;
            Assert.AreNotEqual(null, (familyLinkTable.Rows.FirstOrDefault(x => x.Name.Equals(sureName2 + ", " + foreName2))), "Family links between applicants has been establishedwas failed");

            // Re-select Applicant 1 to add Contact
            applicantTriplet.SearchCriteria.SearchByName = sureName2;
            searchResults = applicantTriplet.SearchCriteria.Search();
            applicantTile = searchResults.FirstOrDefault(t => t.Code.Equals(sureName2 + ", " + foreName2));
            applicationPage = applicantTile.Click<ApplicationPage>();

            // Verify Family link is existed
            familyLinkTable = applicationPage.FamilyLinksGrid;
            Assert.AreNotEqual(null, (familyLinkTable.Rows.FirstOrDefault(x => x.Name.Equals(sureName1 + ", " + foreName1))), "Family links between applicants has been establishedwas failed");



            // Missing report functional
            #endregion STEPS
        }

        /// <summary>
        /// Author: Huy Vo
        /// Des: Exercise ability to update an application including advancing their status from 'Accepted' to "Admitted".
        /// Confirm the correct 'Applicants' are output in a report based on their current 'Application Status'.
        /// </summary>
        /// <returns></returns>
        /// TODO: This test duplicates the TC_AD08. Hence, P3
        //[WebDriverTest(TimeoutSeconds = 3000, Enabled = true, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { PupilTestGroups.Application.ApplicationTests, PupilTestGroups.Severities.Priority3 }, DataProvider = "TC_AD13_Data")]
        public void TC_AD13_Update_Applicant(string sureName1, string middleName1, string foreName1, string gender1, string dateOfBirth1,
                                             string admissionGroup1, string enrolmentStatus1, string class1, string attendanceMode1, string boarderStatus1,
                                             string sureName2, string middleName2, string foreName2, string gender2, string dateOfBirth2,
                                             string admissionGroup2, string enrolmentStatus2, string class2, string attendanceMode2, string boarderStatus2, string applicationStatus)
        {

            // Login as Admission 
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.AdmissionsOfficer);

            // Navigate to Application page
            SeleniumHelper.NavigateMenu("Tasks", "Admissions", "Applications");

            #region PRE-CONDITIONS
            var applicantTriplet = new ApplicationTriplet();

            // Search Applicant 1 and change to Admitted
            applicantTriplet.SearchCriteria.SetStatus("Applied", true);
            applicantTriplet.SearchCriteria.SetStatus("Offered", true);
            applicantTriplet.SearchCriteria.SetStatus("Accepted", true);
            applicantTriplet.SearchCriteria.SearchByName = sureName1;
            var searchResults = applicantTriplet.SearchCriteria.Search();
            var applicantTile = searchResults.SingleOrDefault(t => t.Code.Equals(sureName1 + ", " + foreName1));
            applicantTriplet.ChangeToAdmit(applicantTile);

            // Create new an Applicant 1
            var addNewApplicationDialog = applicantTriplet.AddNewApplicationDialog();
            addNewApplicationDialog.ForeName = foreName1;
            addNewApplicationDialog.MiddleName = middleName1;
            addNewApplicationDialog.SureName = sureName1;
            addNewApplicationDialog.Gender = gender1;
            addNewApplicationDialog.DateofBirth = dateOfBirth1;
            addNewApplicationDialog.Continue();

            // addNewApplicationDialog.ContiueCreateApplicant();

            var registrationDetailsDialog = new RegistrationDetailsDialog();
            registrationDetailsDialog.AdmissionGroup = admissionGroup1;
            registrationDetailsDialog.EnrolmentStatus = enrolmentStatus1;
            registrationDetailsDialog.Class = class1;
            registrationDetailsDialog.AttendanceMode = attendanceMode1;
            registrationDetailsDialog.BoarderStatus = boarderStatus1;
            registrationDetailsDialog.CreateRecord();

            // Search Applicant 2 and change to Admitted
            applicantTriplet.SearchCriteria.SetStatus("Applied", true);
            applicantTriplet.SearchCriteria.SetStatus("Offered", true);
            applicantTriplet.SearchCriteria.SetStatus("Accepted", true);
            applicantTriplet.SearchCriteria.SearchByName = sureName2;
            searchResults = applicantTriplet.SearchCriteria.Search();
            applicantTile = searchResults.SingleOrDefault(t => t.Code.Equals(sureName2 + ", " + foreName2));
            applicantTriplet.ChangeToAdmit(applicantTile);

            // Create new an Applicant 2
            addNewApplicationDialog = applicantTriplet.AddNewApplicationDialog();
            addNewApplicationDialog.ForeName = foreName2;
            addNewApplicationDialog.MiddleName = middleName2;
            addNewApplicationDialog.SureName = sureName2;
            addNewApplicationDialog.Gender = gender2;
            addNewApplicationDialog.DateofBirth = dateOfBirth2;

            // Add new informations on RegistrationDetails Dialog
            addNewApplicationDialog.Continue();
            //   addNewApplicationDialog.ContiueCreateApplicant();

            registrationDetailsDialog = new RegistrationDetailsDialog
            {
                AdmissionGroup = admissionGroup2,
                EnrolmentStatus = enrolmentStatus2,
                Class = class2,
                AttendanceMode = attendanceMode2,
                BoarderStatus = boarderStatus2
            };
            registrationDetailsDialog.CreateRecord();

            applicantTriplet.SearchCriteria.SetStatus("Applied", true);
            applicantTriplet.SearchCriteria.SetStatus("Offered", true);
            applicantTriplet.SearchCriteria.SetStatus("Accepted", true);
            applicantTriplet.SearchCriteria.SearchByName = sureName1;
            searchResults = applicantTriplet.SearchCriteria.Search();
            applicantTile = searchResults.SingleOrDefault(t => t.Code.Equals(sureName1 + ", " + foreName1));
            ApplicationPage applicationPage = applicantTile.Click<ApplicationPage>();

            // Update Application 1 status to Accepted
            applicationPage.ApplicationStatus = "Accepted";
            applicationPage.ClickSave();

            // Update Application 1 status to Accepted
            applicantTriplet.SearchCriteria.SearchByName = sureName2;
            searchResults = applicantTriplet.SearchCriteria.Search();
            applicantTile = searchResults.SingleOrDefault(t => t.Code.Equals(sureName2 + ", " + foreName2));
            applicationPage = applicantTile.Click<ApplicationPage>();
            // Update Application 2 status to Admitted
            applicationPage.ApplicationStatus = "Accepted";
            applicationPage.ClickSave();

            #endregion PRE-CONDITIONS
            #region STEPS
            // Change Application status of Applicant 1 to Accepted to Admitted
            applicantTriplet.SearchCriteria.SetStatus("Applied", false);
            applicantTriplet.SearchCriteria.SetStatus("Offered", false);
            applicantTriplet.SearchCriteria.SetStatus("Accepted", true);
            applicantTriplet.SearchCriteria.SearchByName = sureName1;
            searchResults = applicantTriplet.SearchCriteria.Search();
            applicantTile = searchResults.SingleOrDefault(t => t.Code.Equals(sureName1 + ", " + foreName1));
            applicationPage = applicantTile.Click<ApplicationPage>();
            applicationPage.ApplicationStatus = applicationStatus;
            applicationPage.ClickSave();

            // Continue confirm to change to Admitted
            var confirmChangeStatusDialog = new ConfirmRequiredChangeStatus();
            confirmChangeStatusDialog.ConfirmContinueChangeStatus();

            // Change Application status of Applicant 2 to Accepted to Admitted
            applicantTriplet.SearchCriteria.SearchByName = sureName2;
            searchResults = applicantTriplet.SearchCriteria.Search();
            applicantTile = searchResults.SingleOrDefault(t => t.Code.Equals(sureName2 + ", " + foreName2));
            applicationPage = applicantTile.Click<ApplicationPage>();
            applicationPage.ApplicationStatus = applicationStatus;
            applicationPage.ClickSave();

            // Continue confirm to change Admitted
            confirmChangeStatusDialog = new ConfirmRequiredChangeStatus();
            confirmChangeStatusDialog.ConfirmContinueChangeStatus();

            // Verify change to Admitted successfully
            applicantTriplet.SearchCriteria.SetStatus("Applied", false);
            applicantTriplet.SearchCriteria.SetStatus("Offered", false);
            applicantTriplet.SearchCriteria.SetStatus("Accepted", false);
            applicantTriplet.SearchCriteria.SetStatus("Admitted", true);
            applicantTriplet.SearchCriteria.SearchByName = sureName1;
            searchResults = applicantTriplet.SearchCriteria.Search();

            // Verify twice Applicant 1 was changed to Admitted
            applicantTile = searchResults.FirstOrDefault(t => t.Code.Equals(sureName1 + ", " + foreName1));
            Assert.AreNotEqual(null, applicantTile, "Change status to Offered was failed");

            applicationPage = applicantTile.Click<ApplicationPage>();
            Assert.AreEqual(applicationStatus, applicationPage.ApplicationStatus, "Status is not equal Accepted");

            // Search Applicant 2 and verify that status was changed to Admitted
            applicantTriplet.SearchCriteria.SearchByName = sureName2;
            searchResults = applicantTriplet.SearchCriteria.Search();
            applicantTile = searchResults.FirstOrDefault(t => t.Code.Equals(sureName2 + ", " + foreName2));
            Assert.AreNotEqual(null, applicantTile, "Change status to Offered was failed");
            applicationPage = applicantTile.Click<ApplicationPage>();
            Assert.AreEqual(applicationStatus, applicationPage.ApplicationStatus, "Status is not equal Accepted");

            // Missing report functional
            #endregion STEPS
        }

        #region data

        public List<object[]> TC_AD09_Data()
        {
            string pattern = "M/d/yyyy";
            string sureName = "SureNameTC09 " + SeleniumHelper.GenerateRandomString(10);
            string middleName = "MiddleNameTC09 " + SeleniumHelper.GenerateRandomString(10);
            string foreName = "ForeNameTC09 " + SeleniumHelper.GenerateRandomString(10);
            string dateOfBirth = DateTime.ParseExact("06/12/2006", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            string dateOfBirthUpdate = DateTime.ParseExact("01/08/2010", pattern, CultureInfo.InvariantCulture).ToString(pattern);
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
                    "Not a Boarder",
                    // Date of Birth update
                    dateOfBirthUpdate,
                    //Birth Certificate Seen
                    true,
                    //Application Status
                    "Offered"

                }

            };
            return res;
        }
        public List<object[]> TC_AD10_Data()
        {
            string pattern = "M/d/yyyy";
            string sureName1 = "SureNameTC10_01 " + SeleniumHelper.GenerateRandomString(10);
            string middleName1 = "MiddleNameTC10_01 " + SeleniumHelper.GenerateRandomString(10);
            string foreName1 = "ForeNameTC10_01 " + SeleniumHelper.GenerateRandomString(10);
            string dateOfBirth1 = DateTime.ParseExact("07/29/1982", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            string sureName2 = "SureNameTC10_02 " + SeleniumHelper.GenerateRandomString(10);
            string middleName2 = "MiddleNameTC10_02 " + SeleniumHelper.GenerateRandomString(10);
            string foreName2 = "ForeNameTC10_02 " + SeleniumHelper.GenerateRandomString(10);

            string dateOfBirth2 = DateTime.ParseExact("06/12/2006", pattern, CultureInfo.InvariantCulture).ToString(pattern);

            var res = new List<Object[]>
            {
                new object[]
                {
                     // SurName
                sureName1,
                    // Middle Name
                middleName1,
                    //ForeName
                foreName1,
                    // Gender
                    "Male",
                    // Date of Birth
                    dateOfBirth1,
                    // Admission Group
                    "CES Hold N 15/16",
                    //Enrolment Status
                    "Guest Pupil",
                    //Class
                    "1A",
                    //Attendance Mode
                    "Full-Time",
                    //Boarder Status
                    "Boarder, nights per week not specified",
					//Surename 2
					sureName2,
                    // Middle Name
					middleName2,
                    //ForeName
					foreName2,
                    // Gender
                    "Female",
                    // Date of Birth
                    dateOfBirth2,
                    // Admission Group
                    "CES Hold N 15/16",
                    //Enrolment Status
                    "Single Registration",
                    //Class
                    "2A",
                    //Attendance Mode
                    "AM only",
                    //Boarder Status
                    "Not a Boarder",
                    "Accepted"

                }

            };
            return res;
        }
        public List<object[]> TC_AD11_Data()
        {
            string pattern = "M/d/yyyy";
            string sureName1 = "SureNameTC11_01 " + SeleniumHelper.GenerateRandomString(10);
            string middleName1 = "MiddleNameTC11_01 " + SeleniumHelper.GenerateRandomString(10);
            string foreName1 = "ForeNameTC11_01 " + SeleniumHelper.GenerateRandomString(10);
            string dateOfBirth1 = DateTime.ParseExact("07/29/1982", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            string sureName2 = "SureNameTC11_02 " + SeleniumHelper.GenerateRandomString(10);
            string middleName2 = "MiddleNameTC11_02 " + SeleniumHelper.GenerateRandomString(10);
            string foreName2 = "ForeNameTC11_02 " + SeleniumHelper.GenerateRandomString(10);
            string dateOfBirth2 = DateTime.ParseExact("06/12/2006", pattern, CultureInfo.InvariantCulture).ToString(pattern);

            var res = new List<Object[]>
            {
                new object[]
                {
                    // SurName1
                   sureName1,
                    // Middle Name1
                   middleName1,
                    //ForeName1
                   foreName1,
                    // Gender
                    "Male",
                    // Date of Birth
                    dateOfBirth1,
                    // Admission Group
                    "CES Hold N 15/16",
                    //Enrolment Status
                    "Guest Pupil",
                    //Class
                    "1A",
                    //Attendance Mode
                    "Full-Time",
                    //Boarder Status
                    "Boarder, nights per week not specified",
					// Surename 2
                 sureName2,
                    // Middle Name
                 middleName2,
                    //ForeName
                 foreName2,
                    // Gender
                    "Female",
                    // Date of Birth
                    dateOfBirth2,
                    // Admission Group
                    "CES Hold N 15/16",
                    //Enrolment Status
                    "Single Registration",
                    //Class
                    "2A",
                    //Attendance Mode
                    "AM only",
                    //Boarder Status
                    "Not a Boarder",
                   //Address 
                   //House/Building No
                    "123 ",
                   // House/Building Name
                   "House building name",
                   //Flat/Apartment/Office
                   "Flat - Apartment - Office",
                   // Street
                   "Street sample",
                   //District
                   "District sample",
                   // Town/City
                   "Town - City sample",
                   //Country
                   "Country sample",
                   // PostCode
                   "SW1A 2AA",
                   // Country(UK)
                   "United Kingdom",
                   // Family
                   //Surename
                   "Surename relationship 101_Chrome",
                   //Relationship
                   "Parent",
                   //Parental Responsibility 
                   true

                }

            };
            return res;
        }
        public List<object[]> TC_AD13_Data()
        {
            string pattern = "M/d/yyyy";
            string sureName1 = "SureNameTC13_01 " + SeleniumHelper.GenerateRandomString(10);
            string middleName1 = "MiddleNameTC13_01 " + SeleniumHelper.GenerateRandomString(10);
            string foreName1 = "ForeNameTC13_01 " + SeleniumHelper.GenerateRandomString(10);
            string dateOfBirth1 = DateTime.ParseExact("07/29/1982", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            string sureName2 = "SureNameTC13_02 " + SeleniumHelper.GenerateRandomString(10);
            string middleName2 = "MiddleNameTC13_02 " + SeleniumHelper.GenerateRandomString(10);
            string foreName2 = "ForeNameTC13_02 " + SeleniumHelper.GenerateRandomString(10);
            string dateOfBirth2 = DateTime.ParseExact("06/12/2006", pattern, CultureInfo.InvariantCulture).ToString(pattern);

            var res = new List<Object[]>
            {
                new object[]
                {
                    // SurName
                    sureName1,
                    // Middle Name
                    middleName1,
                    //ForeName
                    foreName1,
                    // Gender
                    "Male",
                    // Date of Birth
                    dateOfBirth1,
                    // Admission Group
                    "CES Hold N 15/16",
                    //Enrolment Status
                    "Guest Pupil",
                    //Class
                    "1A",
                    //Attendance Mode
                    "Full-Time",
                    //Boarder Status
                    "Boarder, nights per week not specified",
					//Surename 2
                    sureName2,
                    // Middle Name
                    middleName2,
                    //ForeName
                    foreName2,
                    // Gender
                    "Female",
                    // Date of Birth
                    dateOfBirth2,
                    // Admission Group
                    "CES Hold N 15/16",
                    //Enrolment Status
                    "Single Registration",
                    //Class
                    "2A",
                    //Attendance Mode
                    "AM only",
                    //Boarder Status
                    "Not a Boarder",
                    //Application Status
                    "Admitted"

                }

            };
            return res;
        }

        #endregion

        #endregion

        #region School Intake

        /// <summary>
        /// Author: Huy.Vo
        /// Description: Create 1st new active 'School Intake' with 'Admission Groups' in the next academic year
        /// TODO: This test has been triaged as P4 due to duplication
        /// </summary>
        //[WebDriverTest(TimeoutSeconds = 1200, Enabled = true, DataProvider = "TC_AD02_Data", Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { PupilTestGroups.Application.SchoolIntakeTests, PupilTestGroups.Severities.Priority4 })]
        public void TC_AD_02_Create_New_Active_School_Intake(string admissionYear, string admissionTerm, string yearGroup,
                       string numberOfPlannedAdmission, string admissionGroupName, string dateOfAdmission, string capacity)
        {
            #region PRE-CONDITIONS:

            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.AdmissionsOfficer);
            SeleniumHelper.NavigateMenu("Tasks", "Admissions", "School Intakes");

            // Search and delete School intake by Year group if it is existing in database
            var schoolIntakeTriplet = new SchoolIntakeTriplet();
            schoolIntakeTriplet.SearchCriteria.SearchYearAdmissionYear = admissionYear;
            schoolIntakeTriplet.SearchCriteria.SearchByYearGroup = yearGroup;
            var searchResults = schoolIntakeTriplet.SearchCriteria.Search();
            var schoolIntakeTile = searchResults.SingleOrDefault(t => t.Code.Equals(admissionYear + " - " + admissionTerm + " " + yearGroup));
            schoolIntakeTriplet.SelectSearchTile(schoolIntakeTile);
            schoolIntakeTriplet.Delete();

            #endregion

            #region STEPS:

            // Create new School Intake
            var schoolIntakePage = schoolIntakeTriplet.Create();
            schoolIntakePage.AdmissionYear = admissionYear;
            schoolIntakePage.AdmissionTerm = admissionTerm;
            Thread.Sleep(1000);
            schoolIntakePage.YearGroup = yearGroup;
            Thread.Sleep(1000);
            schoolIntakePage.NumberOfPlannedAdmissions = numberOfPlannedAdmission;
            Thread.Sleep(1000);
            schoolIntakePage.ClickOnAddNewAdmissionGroupButton();
            var admissionGroupTable = schoolIntakePage.AdmissionGrid;
            admissionGroupTable[0].Name = admissionGroupName;
            admissionGroupTable[0].DateOfAdmission = dateOfAdmission;
            admissionGroupTable[0].Capacity = capacity;
            schoolIntakePage.ClickSaveWithNoConfirmation();
            Assert.AreEqual(true, schoolIntakePage.IsSuccessMessageIsDisplayed(), "Creating school intake was failed");

            // Search for new school intake that has just created
            schoolIntakeTriplet.SearchCriteria.SearchYearAdmissionYear = admissionYear;
            schoolIntakeTriplet.SearchCriteria.SearchByYearGroup = yearGroup;
            searchResults = schoolIntakeTriplet.SearchCriteria.Search();
            schoolIntakeTile = searchResults.SingleOrDefault(t => t.Code.Equals(admissionYear + " - " + admissionTerm + " " + yearGroup));
            schoolIntakePage = schoolIntakeTile.Click<SchoolIntakePage>();

            // Verify that a new school intake was added succesffully
            Assert.AreEqual(admissionYear, schoolIntakePage.AdmissionYear, "Creating school intake was failed");
            Assert.AreEqual(admissionTerm, schoolIntakePage.AdmissionTerm, "Creating school intake was failed");
            Assert.AreEqual(yearGroup, schoolIntakePage.YearGroup, "Creating school intake was failed");
            Assert.AreEqual(numberOfPlannedAdmission, schoolIntakePage.NumberOfPlannedAdmissions, "Creating school intake was failed");
            Assert.AreEqual((admissionYear + " - " + admissionTerm + " " + yearGroup), schoolIntakePage.Name, "Creating school intake was failed");
            Assert.AreEqual(true, schoolIntakePage.Active, "Creating school intake was failed");

            // Verify that Admission Group informations were added successfully
            admissionGroupTable = schoolIntakePage.AdmissionGrid;
            Assert.AreNotEqual(null, (admissionGroupTable.Rows.SingleOrDefault(x => x.Name == admissionGroupName && x.DateOfAdmission == dateOfAdmission &&
                                       x.Capacity == capacity && x.Active)), "Creating school intake was failed");
            schoolIntakeTriplet.Delete();
            #endregion

        }

        /// <summary>
        /// Author: Huy.Vo
        /// Description: Exercise ability to create an inactive 'School Intake' with 'Admission Groups' in the next academic year.
        /// </summary>
        /// TODO: This test has been triaged as P4 due to duplication
        //[WebDriverTest(TimeoutSeconds = 1200, Enabled = true, DataProvider = "TC_AD03_Data", Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { PupilTestGroups.Application.SchoolIntakeTests, PupilTestGroups.Severities.Priority4 })]

        public void TC_AD_03_Create_New_In_Active_School_Intake(string admissionYear, string admissionTerm, string yearGroup,
                                    string numberOfPlannedAdmission, string admissionGroupName, string dateOfAdmission, string capacity)
        {
            #region PRE-CONDITIONS:

            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.AdmissionsOfficer);
            SeleniumHelper.NavigateMenu("Tasks", "Admissions", "School Intakes");

            // Search and delete School intake if it is existing in database
            var schoolIntakeTriplet = new SchoolIntakeTriplet();
            schoolIntakeTriplet.SearchCriteria.SearchYearAdmissionYear = admissionYear;
            schoolIntakeTriplet.SearchCriteria.SearchByName = admissionYear + " - " + admissionTerm + " " + yearGroup;
            schoolIntakeTriplet.SearchCriteria.SetActiveOrInActive = true;

            //schoolIntakeTriplet.SearchCriteria.SearchYearAdmissionYear = admissionYear;
            var searchResults = schoolIntakeTriplet.SearchCriteria.Search();
            var schoolIntakeTile = searchResults.SingleOrDefault(t => t.Code.Equals(admissionYear + " - " + admissionTerm + " " + yearGroup));
            schoolIntakeTriplet.SelectSearchTile(schoolIntakeTile);
            schoolIntakeTriplet.Delete();

            #endregion

            #region STEPS:

            // Create new School Intake
            var schoolIntakePage = schoolIntakeTriplet.Create();
            schoolIntakePage.AdmissionYear = admissionYear;
            schoolIntakePage.Active = false;
            schoolIntakePage.AdmissionTerm = admissionTerm;
            Thread.Sleep(1000);
            schoolIntakePage.YearGroup = yearGroup;
            Thread.Sleep(1000);
            schoolIntakePage.NumberOfPlannedAdmissions = numberOfPlannedAdmission;
            Thread.Sleep(1000);
            var confirmDialog = schoolIntakePage.ClickSave();
            confirmDialog.CancelProcessAddNewAdmission();
            schoolIntakePage.Refresh();
            confirmDialog = schoolIntakePage.ClickSave();
            confirmDialog.ConfirmSaveWithoutAnyAdmissionGroupDefined();

            // Verify that save successufully
            Assert.AreEqual(true, schoolIntakePage.IsSuccessMessageIsDisplayed(), "Creating school intake was failed");
            Assert.AreEqual(admissionYear, schoolIntakePage.AdmissionYear, "Creating school intake was failed");
            Assert.AreEqual(admissionTerm, schoolIntakePage.AdmissionTerm, "Creating school intake was failed");
            Assert.AreEqual(yearGroup, schoolIntakePage.YearGroup, "Creating school intake was failed");
            Assert.AreEqual(numberOfPlannedAdmission, schoolIntakePage.NumberOfPlannedAdmissions, "Creating school intake was failed");
            Assert.AreEqual((admissionYear + " - " + admissionTerm + " " + yearGroup), schoolIntakePage.Name, "Creating school intake was failed");

            // Add datas for Admission Group
            schoolIntakePage.ClickOnAddNewAdmissionGroupButton();
            var admissionGroupTable = schoolIntakePage.AdmissionGrid;
            Thread.Sleep(TimeSpan.FromSeconds(1));
            admissionGroupTable[0].Name = admissionGroupName;
            admissionGroupTable[0].DateOfAdmission = dateOfAdmission;
            admissionGroupTable[0].Capacity = capacity;
            admissionGroupTable[0].Active = false;
            schoolIntakePage.ClickSaveWithNoConfirmation();
            Assert.AreEqual(true, schoolIntakePage.IsSuccessMessageIsDisplayed(), "Creating school intake was failed");

            // Search for new school intake
            schoolIntakeTriplet.SearchCriteria.SearchYearAdmissionYear = admissionYear;
            schoolIntakeTriplet.SearchCriteria.SearchByName = admissionYear + " - " + admissionTerm + " " + yearGroup;
            schoolIntakeTriplet.SearchCriteria.SetActiveOrInActive = true;
            searchResults = schoolIntakeTriplet.SearchCriteria.Search();
            schoolIntakeTile = searchResults.SingleOrDefault(t => t.Code.Equals(admissionYear + " - " + admissionTerm + " " + yearGroup));
            schoolIntakePage = schoolIntakeTile.Click<SchoolIntakePage>();

            // Verify that a new schoolintake was added succesffully
            Assert.AreEqual(admissionYear, schoolIntakePage.AdmissionYear, "Creating school intake was failed");
            Assert.AreEqual(admissionTerm, schoolIntakePage.AdmissionTerm, "Creating school intake was failed");
            Assert.AreEqual(yearGroup, schoolIntakePage.YearGroup, "Creating school intake was failed");
            Assert.AreEqual(numberOfPlannedAdmission, schoolIntakePage.NumberOfPlannedAdmissions, "Creating school intake was failed");
            Assert.AreEqual((admissionYear + " - " + admissionTerm + " " + yearGroup), schoolIntakePage.Name, "Creating school intake was failed");
            Assert.AreEqual(false, schoolIntakePage.Active, "Creating school intake was failed");

            // Check School intake are created
            admissionGroupTable = schoolIntakePage.AdmissionGrid;
            Assert.AreNotEqual(null, (admissionGroupTable.Rows.SingleOrDefault(x => x.Name == admissionGroupName && x.DateOfAdmission == dateOfAdmission &&
                                       x.Capacity == capacity && x.Active == false)), "Creating school intake was failed");
            schoolIntakeTriplet.Delete();
            #endregion

        }
        /// <summary>
        /// Author: Huy.Vo
        /// Description: Exercise ability to edit an active 'School Intake' and the 'Admission Groups' associated with this school intake.
        /// </summary>
        /// TODO: This test has been triaged as P3 because failure of the test will not result in P1 or P2 according to the SLA
        //[WebDriverTest(TimeoutSeconds = 1200, Enabled = true, DataProvider = "TC_AD04_Data", Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { PupilTestGroups.Application.SchoolIntakeTests, PupilTestGroups.Severities.Priority3 })]

        public void TC_AD_04_Update_Active_School_Intake(string admissionYear, string admissionTerm, string yearGroup,
                                                         string numberOfPlannedAdmission, string admissionGroupName, string dateOfAdmission, string capacity,
                                                         string numberOfPlannedAdmissionUpdate, string admissionNameUpdate, string admissionGroupNameUpdate1, string admissionGroupcapacity1,
                                                         string admissionGroupNameUpdate2, string dateOfAdmissionUpdate, string admissionGroupcapacity2)
        {
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.AdmissionsOfficer);
            SeleniumHelper.NavigateMenu("Tasks", "Admissions", "School Intakes");

            #region PRE-CONDITIONS
            // Search and update School intake if it is existing in database
            var schoolIntakeTriplet = new SchoolIntakeTriplet();
            schoolIntakeTriplet.SearchCriteria.SearchByName = admissionYear + " - " + admissionTerm + " " + yearGroup;
            schoolIntakeTriplet.SearchCriteria.SearchYearAdmissionYear = admissionYear;
            var searchResults = schoolIntakeTriplet.SearchCriteria.Search();
            var schoolIntakeTile = searchResults.SingleOrDefault(t => t.Code.Equals(admissionYear + " - " + admissionTerm + " " + yearGroup));
            schoolIntakeTriplet.SelectSearchTile(schoolIntakeTile);
            schoolIntakeTriplet.Delete();

            // Create new School Intake
            var schoolIntakePage = schoolIntakeTriplet.Create();
            schoolIntakePage.AdmissionYear = admissionYear;
            schoolIntakePage.AdmissionTerm = admissionTerm;
            Thread.Sleep(1000);
            schoolIntakePage.YearGroup = yearGroup;
            Thread.Sleep(1000);
            schoolIntakePage.NumberOfPlannedAdmissions = numberOfPlannedAdmission;
            Thread.Sleep(1000);
            schoolIntakePage.ClickOnAddNewAdmissionGroupButton();
            var admissionGroupTable = schoolIntakePage.AdmissionGrid;
            admissionGroupTable[0].Name = admissionGroupName;
            admissionGroupTable[0].DateOfAdmission = dateOfAdmission;
            admissionGroupTable[0].Capacity = capacity;
            schoolIntakePage.ClickSaveWithNoConfirmation();

            #endregion PRE-CONDITIONS

            #region STEPS
            // Search for new chool intake
            schoolIntakeTriplet.SearchCriteria.SearchByName = admissionYear + " - " + admissionTerm + " " + yearGroup;
            searchResults = schoolIntakeTriplet.SearchCriteria.Search();
            schoolIntakeTriplet.SearchCriteria.SearchYearAdmissionYear = admissionYear;
            schoolIntakeTile = searchResults.SingleOrDefault(t => t.Code.Equals(admissionYear + " - " + admissionTerm + " " + yearGroup));
            schoolIntakePage = schoolIntakeTile.Click<SchoolIntakePage>();

            // Update new School Intake
            schoolIntakePage.NumberOfPlannedAdmissions = numberOfPlannedAdmissionUpdate;
            schoolIntakePage.Name = admissionNameUpdate;
            admissionGroupTable = schoolIntakePage.AdmissionGrid;
            admissionGroupTable[0].Name = admissionGroupNameUpdate1;
            admissionGroupTable[0].Capacity = admissionGroupcapacity1;
            schoolIntakePage.SaveUpdate();

            Assert.AreEqual(true, schoolIntakePage.IsSuccessMessageIsDisplayed(), "Updating school intake was failed");

            // Search for new chool intake
            schoolIntakeTriplet.SearchCriteria.SearchByName = admissionNameUpdate;
            searchResults = schoolIntakeTriplet.SearchCriteria.Search();
            schoolIntakeTile = searchResults.SingleOrDefault(t => t.Code.Equals(admissionNameUpdate));
            schoolIntakePage = schoolIntakeTile.Click<SchoolIntakePage>();

            // Verify that a new schoolintake was updated succesffully
            Assert.AreEqual(numberOfPlannedAdmissionUpdate, schoolIntakePage.NumberOfPlannedAdmissions, "Updating school intake was failed");
            Assert.AreEqual((admissionNameUpdate), schoolIntakePage.Name, "Updating school intake was failed");
            Assert.AreEqual(true, schoolIntakePage.Active, "Updating school intake was failed");

            schoolIntakePage.ClickOnAddNewAdmissionGroupButton();
            admissionGroupTable = schoolIntakePage.AdmissionGrid;
            Assert.AreNotEqual(null, (admissionGroupTable.Rows.SingleOrDefault(x => x.Name == admissionGroupNameUpdate1 &&
                                      x.Capacity == admissionGroupcapacity1)), "Updating school intake was failed");

            // Add new Admission Group data
            admissionGroupTable[1].Name = admissionGroupNameUpdate2;
            admissionGroupTable[1].DateOfAdmission = dateOfAdmissionUpdate;
            admissionGroupTable[1].Capacity = admissionGroupcapacity2;
            schoolIntakePage.ClickSaveWithNoConfirmation();

            // Search for new school intake
            schoolIntakeTriplet.SearchCriteria.SearchByName = "";
            schoolIntakeTriplet.SearchCriteria.SearchByYearGroup = yearGroup;
            searchResults = schoolIntakeTriplet.SearchCriteria.Search();
            schoolIntakeTile = searchResults.SingleOrDefault(t => t.Code.Equals(admissionNameUpdate));
            schoolIntakePage = schoolIntakeTile.Click<SchoolIntakePage>();

            admissionGroupTable = schoolIntakePage.AdmissionGrid;
            Assert.AreNotEqual(null, (admissionGroupTable.Rows.SingleOrDefault(x => x.Name == admissionGroupNameUpdate2 && x.DateOfAdmission == dateOfAdmissionUpdate &&
                                       x.Capacity == admissionGroupcapacity2)), "Updating school intake was failed");
            schoolIntakeTriplet.Delete();

            #endregion STEPS
        }
        /// <summary>
        /// Author: Huy.Vo
        /// Description: Exercise ability to edit an inactive 'School Intake' and the 'Admission Groups' associated with this school intake.
        /// </summary>
        /// TODO: This test has been triaged as P3 because failure of the test will not result in P1 or P2 according to the SLA
        //[WebDriverTest(TimeoutSeconds = 1200, Enabled = true, DataProvider = "TC_AD05A_Data", Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { PupilTestGroups.Application.SchoolIntakeTests, PupilTestGroups.Severities.Priority3 })]
        public void TC_AD_05A_Update_In_Active_School_Intake(string admissionYear, string admissionTerm, string yearGroup,
            string numberOfPlannedAdmission, string admissionGroupName, string dateOfAdmission, string capacity,
            string numberOfPlannedAdmissionUpdate, string admissionNameUpdate, string admissionGroupNameUpdate1, string admissionGroupcapacity1,
            string admissionGroupNameUpdate2, string dateOfAdmissionUpdate, string admissionGroupcapacity2)
        {

            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.AdmissionsOfficer);
            SeleniumHelper.NavigateMenu("Tasks", "Admissions", "School Intakes");

            #region PRE-CONDITIONS
            // Search and update School intake if it is existing in database
            var schoolIntakeTriplet = new SchoolIntakeTriplet();
            schoolIntakeTriplet.SearchCriteria.SearchByName = admissionYear + " - " + admissionTerm + " " + yearGroup;
            schoolIntakeTriplet.SearchCriteria.SetActiveOrInActive = true;
            schoolIntakeTriplet.SearchCriteria.SearchYearAdmissionYear = admissionYear;
            var searchResults = schoolIntakeTriplet.SearchCriteria.Search();
            var schoolIntakeTile = searchResults.SingleOrDefault(t => t.Code.Equals(admissionYear + " - " + admissionTerm + " " + yearGroup));
            schoolIntakeTriplet.SelectSearchTile(schoolIntakeTile);
            schoolIntakeTriplet.Delete();

            // Create new School Intake
            var schoolIntakePage = schoolIntakeTriplet.Create();
            schoolIntakePage.Active = false;
            schoolIntakePage.AdmissionYear = admissionYear;
            schoolIntakePage.AdmissionTerm = admissionTerm;
            Thread.Sleep(1000);
            schoolIntakePage.YearGroup = yearGroup;
            Thread.Sleep(1000);
            schoolIntakePage.NumberOfPlannedAdmissions = numberOfPlannedAdmission;
            Thread.Sleep(1000);
            schoolIntakePage.ClickOnAddNewAdmissionGroupButton();
            var admissionGroupTable = schoolIntakePage.AdmissionGrid;
            admissionGroupTable[0].Name = admissionGroupName;
            admissionGroupTable[0].DateOfAdmission = dateOfAdmission;
            admissionGroupTable[0].Capacity = capacity;
            schoolIntakePage.ClickSaveWithNoConfirmation();

            // Search
            schoolIntakeTriplet = new SchoolIntakeTriplet();
            schoolIntakeTriplet.SearchCriteria.SearchByName = admissionYear + " - " + admissionTerm + " " + yearGroup;
            schoolIntakeTriplet.SearchCriteria.SearchYearAdmissionYear = admissionYear;
            schoolIntakeTriplet.SearchCriteria.SearchByYearGroup = yearGroup;
            schoolIntakeTriplet.SearchCriteria.SetActiveOrInActive = true;
            searchResults = schoolIntakeTriplet.SearchCriteria.Search();
            schoolIntakeTile = searchResults.SingleOrDefault(t => t.Code.Equals(admissionYear + " - " + admissionTerm + " " + yearGroup));
            schoolIntakePage = schoolIntakeTile.Click<SchoolIntakePage>();

            #endregion PRE-CONDITIONS

            #region STEPS
            // Update new School Intake
            schoolIntakePage.Active = true;
            schoolIntakePage.NumberOfPlannedAdmissions = numberOfPlannedAdmissionUpdate;
            schoolIntakePage.Name = admissionNameUpdate;
            admissionGroupTable = schoolIntakePage.AdmissionGrid;
            admissionGroupTable[0].Name = admissionGroupNameUpdate1;
            admissionGroupTable[0].Capacity = admissionGroupcapacity1;
            admissionGroupTable[0].Active = true;
            schoolIntakePage.SaveUpdate();

            // confirmDialog.ConfirmAdNewAdmission();
            Assert.AreEqual(true, schoolIntakePage.IsSuccessMessageIsDisplayed(), "Updating school intake was failed");

            // Search for new school intake
            schoolIntakeTriplet.SearchCriteria.SearchByName = admissionNameUpdate;
            schoolIntakeTriplet.SearchCriteria.SetActiveOrInActive = false;
            searchResults = schoolIntakeTriplet.SearchCriteria.Search();
            schoolIntakeTile = searchResults.SingleOrDefault(t => t.Code.Equals(admissionNameUpdate));
            schoolIntakePage = schoolIntakeTile.Click<SchoolIntakePage>();

            // Verify that a new schoolintake was updated succesffully
            Assert.AreEqual(numberOfPlannedAdmissionUpdate, schoolIntakePage.NumberOfPlannedAdmissions, "Updating school intake was failed");
            Assert.AreEqual((admissionNameUpdate), schoolIntakePage.Name, "Updating school intake was failed");
            Assert.AreEqual(true, schoolIntakePage.Active, "Updating school intake was failed");

            schoolIntakePage.ClickOnAddNewAdmissionGroupButton();
            admissionGroupTable = schoolIntakePage.AdmissionGrid;
            Assert.AreNotEqual(null, (admissionGroupTable.Rows.SingleOrDefault(x => x.Name == admissionGroupNameUpdate1 &&
                                      x.Capacity == admissionGroupcapacity1 && x.Active)), "Updating school intake was failed");

            // Add new Admission Group data
            admissionGroupTable[1].Name = admissionGroupNameUpdate2;
            admissionGroupTable[1].DateOfAdmission = dateOfAdmissionUpdate;
            admissionGroupTable[1].Capacity = admissionGroupcapacity2;
            schoolIntakePage.ClickSaveWithNoConfirmation();
            Assert.AreEqual(true, schoolIntakePage.IsSuccessMessageIsDisplayed(), "Update school intake was failed");
            // Search for new school intake
            schoolIntakeTriplet.SearchCriteria.SearchByName = "";
            schoolIntakeTriplet.SearchCriteria.SearchByYearGroup = yearGroup;
            searchResults = schoolIntakeTriplet.SearchCriteria.Search();
            schoolIntakeTile = searchResults.SingleOrDefault(t => t.Code.Equals(admissionNameUpdate));
            schoolIntakePage = schoolIntakeTile.Click<SchoolIntakePage>();

            //Verify that add new Admission Group successfully
            admissionGroupTable = schoolIntakePage.AdmissionGrid;
            Assert.AreNotEqual(null, (admissionGroupTable.Rows.SingleOrDefault(x => x.Name == admissionGroupNameUpdate2 && x.DateOfAdmission == dateOfAdmissionUpdate &&
                                       x.Capacity == admissionGroupcapacity2 && x.Active)), "Updating school intake was failed");

            schoolIntakeTriplet.Delete();
            #endregion STEPS
        }

        /// <summary>
        /// Author: Huy.Vo
        /// Description: Exercise ability to update an active 'School Intake' with active 'Admission Groups' so that they become inactive.
        /// </summary>
        /// TODO: This test has been triaged as P3 because failure of the test will not result in P1 or P2 according to the SLA
        //[WebDriverTest(TimeoutSeconds = 1200, Enabled = true, DataProvider = "TC_AD05B_Data", Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { PupilTestGroups.Application.SchoolIntakeTests, PupilTestGroups.Severities.Priority3 })]
        public void TC_AD_05B_Update_Active_To_InActive_School_Intake(string admissionYear, string admissionTerm, string yearGroup, string numberOfPlannedAdmission,
                                                                      string admissionGroupName, string dateOfAdmission, string capacity,
                                                                      bool inActive, bool inActive1)
        {
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.AdmissionsOfficer);
            SeleniumHelper.NavigateMenu("Tasks", "Admissions", "School Intakes");

            #region PRE-CONDITIONS

            // Search and delete School intake if it is existing in database
            var schoolIntakeTriplet = new SchoolIntakeTriplet();
            schoolIntakeTriplet.SearchCriteria.SearchYearAdmissionYear = admissionYear;
            schoolIntakeTriplet.SearchCriteria.SearchByName = admissionYear + " - " + admissionTerm + " " + yearGroup;
            schoolIntakeTriplet.SearchCriteria.SetActiveOrInActive = true;
            schoolIntakeTriplet.SearchCriteria.SearchYearAdmissionYear = admissionYear;
            var searchResults = schoolIntakeTriplet.SearchCriteria.Search();
            var schoolIntakeTile = searchResults.SingleOrDefault(t => t.Code.Equals(admissionYear + " - " + admissionTerm + " " + yearGroup));
            schoolIntakeTriplet.SelectSearchTile(schoolIntakeTile);
            schoolIntakeTriplet.Delete();

            // Create new School Intake
            var schoolIntakePage = schoolIntakeTriplet.Create();
            schoolIntakePage.AdmissionYear = admissionYear;
            schoolIntakePage.Active = false;
            schoolIntakePage.AdmissionTerm = admissionTerm;
            Thread.Sleep(1000);
            schoolIntakePage.YearGroup = yearGroup;
            Thread.Sleep(1000);
            schoolIntakePage.NumberOfPlannedAdmissions = numberOfPlannedAdmission;
            Thread.Sleep(1000);
            schoolIntakePage.ClickOnAddNewAdmissionGroupButton();
            var admissionGroupTable = schoolIntakePage.AdmissionGrid;
            Thread.Sleep(1000);
            admissionGroupTable[0].Name = admissionGroupName;
            admissionGroupTable[0].DateOfAdmission = dateOfAdmission;
            admissionGroupTable[0].Capacity = capacity;
            schoolIntakePage.ClickSaveWithNoConfirmation();

            #endregion PRE-CONDITIONS

            #region STEPS
            // Search to update
            schoolIntakeTriplet.SearchCriteria.SearchByName = (admissionYear + " - " + admissionTerm + " " + yearGroup);
            schoolIntakeTriplet.SearchCriteria.SearchYearAdmissionYear = admissionYear;
            schoolIntakeTriplet.SearchCriteria.SearchByYearGroup = yearGroup;
            schoolIntakeTriplet.SearchCriteria.SetActiveOrInActive = true;
            searchResults = schoolIntakeTriplet.SearchCriteria.Search();
            schoolIntakeTile = searchResults.SingleOrDefault(t => t.Code.Equals(admissionYear + " - " + admissionTerm + " " + yearGroup));
            schoolIntakeTile.Click<SchoolIntakePage>();

            // Update School Intake
            schoolIntakePage.Active = inActive;
            admissionGroupTable = schoolIntakePage.AdmissionGrid;
            admissionGroupTable[0].Active = inActive1;
            schoolIntakePage.SaveUpdate();

            Assert.AreEqual(true, schoolIntakePage.IsSuccessMessageIsDisplayed(), "Updating status of school intake was failed");

            // Search and verify that School intake is not displayed with active status
            schoolIntakeTriplet.SearchCriteria.SearchByName = (admissionYear + " - " + admissionTerm + " " + yearGroup);
            schoolIntakeTriplet.SearchCriteria.SetActiveOrInActive = false;
            searchResults = schoolIntakeTriplet.SearchCriteria.Search();
            schoolIntakeTile = searchResults.SingleOrDefault(t => t.Code.Equals(admissionYear + " - " + admissionTerm + " " + yearGroup));
            Assert.AreEqual(null, schoolIntakeTile, "Updating status of school intake was failed");

            //Verify that School intake is displayed with unactive status
            schoolIntakeTriplet.SearchCriteria.SearchByName = "";
            schoolIntakeTriplet.SearchCriteria.SetActiveOrInActive = true;
            searchResults = schoolIntakeTriplet.SearchCriteria.Search();
            schoolIntakeTile = searchResults.SingleOrDefault(t => t.Code.Equals(admissionYear + " - " + admissionTerm + " " + yearGroup));
            schoolIntakeTriplet.SelectSearchTile(schoolIntakeTile);

            Assert.AreEqual(inActive, schoolIntakePage.Active, "Updating status of school intake was failed");
            admissionGroupTable = schoolIntakePage.AdmissionGrid;
            Assert.AreEqual(true, (admissionGroupTable.Rows.All(x => x.Active == false)), "Updating school intake was failed");
            schoolIntakeTriplet.Delete();
            #endregion STEPS
        }


        #region Data

        public List<object[]> TC_AD02_Data()
        {
            string nextAcademicYear = String.Format("{0}/{1}", (DateTime.Now.Year).ToString(), (DateTime.Now.Year + 1).ToString());
            string dateOfAdmission = new DateTime(DateTime.Today.Year, 8, 1).ToString("M/d/yyyy");

            string admissionGroupName = "TC02" + SeleniumHelper.GenerateRandomString(20);
            var data = new List<Object[]>
            {
                new object[] { nextAcademicYear,"Spring","Year 2","32",admissionGroupName,dateOfAdmission, "15"}
            };
            return data;
        }
        public List<object[]> TC_AD03_Data()
        {
            string nextAcademicYear = String.Format("{0}/{1}", (DateTime.Now.Year).ToString(), (DateTime.Now.Year + 1).ToString());
            string dateOfAdmission = new DateTime(DateTime.Today.Year, 8, 1).ToString("M/d/yyyy");
            string admissionGroupName = "TC03" + SeleniumHelper.GenerateRandomString(20);

            var data = new List<Object[]>
            {
                new object[] { nextAcademicYear,"Autumn","Year 3","33", admissionGroupName,dateOfAdmission, "30"}
            };
            return data;
        }
        public List<object[]> TC_AD04_Data()
        {
            string academicYear = String.Format("{0}/{1}", (DateTime.Now.Year - 1).ToString(), (DateTime.Now.Year).ToString());
            string dateOfAdmission = new DateTime(DateTime.Today.Year - 1, 7, 1).ToString("M/d/yyyy");
            string dateOfAdmissionUpdate = new DateTime(DateTime.Today.Year - 1, 10, 10).ToString("M/d/yyyy");
            string nameUpdate = "TC04" + SeleniumHelper.GenerateRandomString(30);
            string admissionGroupName1 = "TC04" + SeleniumHelper.GenerateRandomString(50) + "update 1";
            string admissionGroupName2 = "TC04" + SeleniumHelper.GenerateRandomString(50) + "update 2";
            var data = new List<Object[]>
            {
                new object[] { academicYear,"Autumn","Year 1","20","IE" + SeleniumHelper.GenerateRandomString(20)+"test case 4",dateOfAdmission, "10",
                    "30",nameUpdate,admissionGroupName1, "25", admissionGroupName2, dateOfAdmissionUpdate,"50"}
            };
            return data;
        }
        public List<object[]> TC_AD05A_Data()
        {
            string academicYear = String.Format("{0}/{1}", (DateTime.Now.Year - 1).ToString(), (DateTime.Now.Year).ToString());
            string dateOfAdmission = new DateTime(DateTime.Today.Year - 1, 7, 1).ToString("M/d/yyyy");
            string nameUpdate = "TC5A" + SeleniumHelper.GenerateRandomString(30);
            string admissionGroupName1 = "TC5A" + SeleniumHelper.GenerateRandomString(50) + "update 1";
            string admissionGroupName2 = "TC5A" + SeleniumHelper.GenerateRandomString(50) + "update 2";
            string dateOfAdmissionUpdate = new DateTime(DateTime.Today.Year - 1, 10, 1).ToString("M/d/yyyy");
            var data = new List<Object[]>
            {
                new object[] { academicYear,"Summer","Year 3","50",nameUpdate, dateOfAdmission,"5",
                               "20", nameUpdate,
                               admissionGroupName1,"20",
                               admissionGroupName2,
                               dateOfAdmissionUpdate, "30"}
            };
            return data;
        }
        public List<object[]> TC_AD05B_Data()
        {
            string academicYear = String.Format("{0}/{1}", (DateTime.Now.Year - 1).ToString(), (DateTime.Now.Year).ToString());
            string dateOfAdmission = new DateTime(DateTime.Today.Year - 1, 7, 16).ToString("M/d/yyyy");
            var data = new List<Object[]>
            {
                new object[] { academicYear,"Autumn","Year 3","33","IE" + SeleniumHelper.GenerateRandomString(15)+ "Admission name test case 5B",dateOfAdmission, "30",
                                false,false}
            };
            return data;
        }

        #endregion

        #endregion

    }
}
