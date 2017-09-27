using System;
using System.Globalization;
using System.Linq;
using System.Threading;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using POM.Components.Common;
using POM.Components.Pupil;
using POM.Helper;
using Pupil.Components.Common;
using Pupil.Data;
using Selene.Support.Attributes;
using SeSugar.Automation;
using SeSugar.Data;
using TestSettings;
using WebDriverRunner.webdriver;
using SimsBy = POM.Helper.SimsBy;

// ReSharper disable once CheckNamespace
namespace Pupil.Pupil.Tests
{
    public class PupilRecordTests
    {
        private string _pattern = "d/M/yyyy";

        /// <summary>
        /// Author: Y.Ta
        /// Descriptions: Verify Add PersonalDetail information successfully
        /// </summary>
        /// 
        [WebDriverTest(Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { PupilTestGroups.PupilRecord.AddNewPupil, PupilTestGroups.PupilRecord.Page, PupilTestGroups.Priority.Priority1, })]
        public void Verify_Add_PersonalDetail()
        {
            //Arrange
            string forename = "SelPersonal" + SeleniumHelper.GenerateRandomString(5) + SeleniumHelper.GenerateRandomNumberUsingDateTime();
            string surname = "SelPersonal" + SeleniumHelper.GenerateRandomString(5) + SeleniumHelper.GenerateRandomNumberUsingDateTime();
            DateTime dob = new DateTime(2011, 02, 02);

            var learnerId = Guid.NewGuid();
            bool birthCertificateSeen = true;
            string quickNote = "Quick note for pupil added by test";

            var pupil = this.BuildDataPackage()
                        .AddBasicLearner(learnerId, surname, forename, dob, dateOfAdmission: new DateTime(2011, 10, 05));

            using (new DataSetup(false, true, pupil))
            {
                //Act
                // Login as school admin
                SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);

                // Navigate to Pupil Record
                AutomationSugar.NavigateMenu("Tasks", "Pupils", "Pupil Records");

                var pupilRecordTriplet = new PupilSearchTriplet();

                pupilRecordTriplet.SearchCriteria.PupilName = String.Format("{0}, {1}", surname, forename);
                var resultPupils = pupilRecordTriplet.SearchCriteria.Search();
                var pupilSearchTile = resultPupils.SingleOrDefault(t => t.Name.Equals(String.Format("{0}, {1}", surname, forename)));

                var pupilRecord = pupilSearchTile?.Click<PupilRecordPage>();
                Assert.AreNotEqual(null, pupilRecord, "Does not found pupil");

                // Add value for Quick Note
                pupilRecord.QuickNote = quickNote;
                // Add value for BirthCertificateSeen
                pupilRecord.BirthCertificateSeen = birthCertificateSeen;

                pupilRecord = PupilRecordPage.Create();
                pupilRecord.SavePupil();

                //Assert
                // Verify data is saved Success
                Assert.AreEqual(true, pupilRecord.IsSuccessMessageDisplayed(), "Success message is not display");
                Assert.AreEqual(birthCertificateSeen, pupilRecord.BirthCertificateSeen, "BirthCertificateSeen is not equal");
                Assert.AreEqual(quickNote, pupilRecord.QuickNote, "QuickNote is not equal");
            }
        }

        /// <summary>
        /// Author: Y.Ta
        /// Descriptions: Verify Add Registration information sucessfully
        /// </summary>
        /// 
        [WebDriverTest(Browsers = new[] { BrowserDefaults.Chrome },
           Groups = new[] { PupilTestGroups.PupilRecord.Page, PupilTestGroups.PupilRecord.AddNewPupil, PupilTestGroups.Priority.Priority1 })]
        public void PupilRecord_AddRegistration()
        {
            //Arrange
            string forename = "AddRegistration" + SeleniumHelper.GenerateRandomString(5) + SeleniumHelper.GenerateRandomNumberUsingDateTime();
            string surname = "AddRegistration" + SeleniumHelper.GenerateRandomString(5) + SeleniumHelper.GenerateRandomNumberUsingDateTime();
            string middleName = "Middle" + SeleniumHelper.GenerateRandomString(5) + SeleniumHelper.GenerateRandomNumberUsingDateTime();
            string gender = "Male";
            var dob = DateTime.ParseExact("02/02/1990", _pattern, CultureInfo.InvariantCulture).ToString(_pattern);
            var doa = DateTime.ParseExact("02/02/2016", _pattern, CultureInfo.InvariantCulture).ToString(_pattern);
            var yearGroup = Queries.GetFirstYearGroup();
            var learnerId = Guid.NewGuid();

            //Act
            // Login as school admin
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);

            // Navigate to Pupil Record
            AutomationSugar.NavigateMenu("Tasks", "Pupils", "Pupil Records");
            Thread.Sleep(5);

            var pupilRecordTriplet = new PupilSearchTriplet();
            var addNewPupilDialog = pupilRecordTriplet.AddNewPupil();

            addNewPupilDialog.Forename = forename;
            addNewPupilDialog.MiddleName = middleName;
            addNewPupilDialog.SurName = surname;
            addNewPupilDialog.Gender = gender;
            addNewPupilDialog.DateOfBirth = dob;

            var registrationDetailDialog = addNewPupilDialog.Continue();
            Thread.Sleep(5);
            registrationDetailDialog.DateOfAdmission = doa;
            registrationDetailDialog.YearGroup = yearGroup.FullName;

            registrationDetailDialog.CreateRecord();
            Thread.Sleep(5);
            var confirmRequiredDialog = new ConfirmRequiredDialog();
            confirmRequiredDialog.ClickOk();

            var pupilRecord = PupilRecordPage.Create();
            pupilRecord.SavePupil();

            AutomationSugar.ExpandAccordionPanel("section_menu_Registration");
            pupilRecord.ClickGenerateUPN();

            pupilRecord = PupilRecordPage.Create();
            pupilRecord.SavePupil();

            // Verify data is saved Success
            Assert.AreEqual(true, pupilRecord.IsSuccessMessageDisplayed(), "Success message is not display");
            Assert.AreNotEqual("", pupilRecord.UniquePupilNumber, "Unique pupil number is created");
        }


        /// <summary>
        /// Author: Y.Ta
        /// Descriptions: Verify Add Phone email sucessfully
        /// </summary>
        /// 
        [WebDriverTest(Browsers = new[] { BrowserDefaults.Chrome },
            Groups = new[] { PupilTestGroups.PupilRecord.AddNewPupil, PupilTestGroups.PupilRecord.Page, PupilTestGroups.Priority.Priority2 })]
        public void TC_PU005_Verify_Add_PhoneEmail_Section()
        {
            // Arrange
            DateTime dob = new DateTime(2006, 01, 06);
            DateTime doa = new DateTime(2010, 01, 06);
            var learnerId = Guid.NewGuid();
            string random = SeleniumHelper.GenerateRandomNumberUsingDateTime();
            string randomCharacter = SeleniumHelper.GenerateRandomString(6) + "_TC_PU05";
            string forename = "aForeName" + randomCharacter + random.ToString();
            string surname = "aSurName" + randomCharacter + random.ToString();

            string telNo = "0123456789";
            string location = "Other";
            string notes = "Notes";

            string email = "abcd@c2kni.org.uk";

            var pupil = this.BuildDataPackage()
                .AddBasicLearner(learnerId, surname, forename, dob, doa);

            //Act
            using (new DataSetup(false, true, pupil))
            {
                try
                {
                    // Login as school admin
                    SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);

                    AutomationSugar.NavigateMenu("Tasks", "Pupils", "Pupil Records");
                    var pupilRecordTriplet = new PupilSearchTriplet();

                    pupilRecordTriplet.SearchCriteria.PupilName = String.Format("{0}, {1}", surname, forename);
                    var resultPupils = pupilRecordTriplet.SearchCriteria.Search();
                    var pupilSearchTile =
                        resultPupils.SingleOrDefault(t => t.Name.Equals(String.Format("{0}, {1}", surname, forename)));

                    var pupilRecord = pupilSearchTile == null ? null : pupilSearchTile.Click<PupilRecordPage>();
                    Assert.AreNotEqual(null, pupilRecord, "Does not found pupil");

                    pupilRecord.SelectPhoneEmailTab();

                    // Add Pupil Telephone number
                    pupilRecord = new PupilRecordPage();
                    pupilRecord.ClickAddTelephoneNumber();
                    pupilRecord.TelephoneNumberTable[0].TelephoneNumber = telNo;
                    pupilRecord.TelephoneNumberTable[0].Location = location;
                    pupilRecord.TelephoneNumberTable[0].Note = notes;

                    // Add Email Address
                    pupilRecord.ClickAddEmailAddress();
                    pupilRecord.EmailTable[0].EmailAddress = email;
                    pupilRecord.EmailTable[0].Location = location;
                    pupilRecord.EmailTable[0].Note = notes;

                    pupilRecord = PupilRecordPage.Create();
                    pupilRecord.SavePupil();

                    //Assert
                    Assert.AreEqual(true, pupilRecord.IsSuccessMessageDisplayed(), "Success message is not display");
                }
                finally
                {
                    PurgeLinkedData.DeleleLearnerEmail(learnerId);
                    PurgeLinkedData.DeleleLearnerTel(learnerId);
                }
            }
        }

        /// <summary>
        /// Author: Y.Ta
        /// Descriptions: Verify Add Contact into pupil record sucessfully
        /// </summary>
        /// 
        [WebDriverTest(Browsers = new[] { BrowserDefaults.Chrome },
            Groups = new[] { PupilTestGroups.PupilRecord.AddNewPupil, PupilTestGroups.PupilRecord.Page, PupilTestGroups.Priority.Priority2, "PR_VACS" })]
        public void TC_PU006_Verify_Add_Contact_Section()
        {
            // Arrange
            DateTime dob = new DateTime(2006, 01, 06);
            DateTime doa = new DateTime(2010, 01, 06);
            var learnerId = Guid.NewGuid();
            string random = SeleniumHelper.GenerateRandomNumberUsingDateTime();
            string randomCharacter = SeleniumHelper.GenerateRandomString(6) + "_TC_PU05";
            string forename = "aForeName" + randomCharacter + random;
            string surname = "aSurName" + randomCharacter + random;

            var pupil = this.BuildDataPackage()
                .AddBasicLearner(learnerId, surname, forename, dob, doa);

            //Act
            using (new DataSetup(false, true, pupil))
            {
                try
                {
                    // Login as school admin
                    SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);

                    // Navigate to Pupil Record
                    AutomationSugar.NavigateMenu("Tasks", "Pupils", "Pupil Records");
                    var pupilRecordTriplet = new PupilSearchTriplet();

                    pupilRecordTriplet.SearchCriteria.PupilName = String.Format("{0}, {1}", surname, forename);
                    var resultPupils = pupilRecordTriplet.SearchCriteria.Search();
                    var pupilSearchTile =
                        resultPupils.SingleOrDefault(
                            t => t.Name.Equals(String.Format("{0}, {1}", surname, forename)));

                    var pupilRecord = pupilSearchTile == null ? null : pupilSearchTile.Click<PupilRecordPage>();
                    Assert.AreNotEqual(null, pupilRecord, "Does not found pupil");

                    pupilRecord.SelectFamilyHomeTab();

                    var addPupilContactTripletDialog = pupilRecord.ClickAddContact();
                    var contactSearchResults = addPupilContactTripletDialog.SearchCriteria.Search();

                    // 'Add' a contact, by selecting the 2nd contact
                    var pupilContactSearchTile = contactSearchResults[1];
                    var pupilContactDialog = pupilContactSearchTile == null
                        ? null
                        : pupilContactSearchTile.Click<AddPupilContactDialog>();

                    addPupilContactTripletDialog.ClickOk();

                    // Note: this seems flawed, row is added before this code executes and internal logic doesn't identify new row and times out
                    //pupilRecord.ContactTable.WaitForNewRowAppear();
                    Thread.Sleep(2000);

                    pupilRecord = PupilRecordPage.Create();
                    pupilRecord.SavePupil();

                    Assert.AreNotEqual(0, pupilRecord.ContactTable.Rows.Count,
                        "The row contact does not display after adding it");

                    //update value into table Contact                        
                    pupilRecord.ContactTable[0].Priority = "1";
                    pupilRecord.ContactTable[0].RelationShip = "Parent";
                    pupilRecord.ContactTable[0].ParentalReponsibility = true;
                    pupilRecord.ContactTable[0].DoNotDisclose = true;

                    pupilRecord = PupilRecordPage.Create();
                    pupilRecord.SavePupil();

                    //Generate Salutation
                    pupilRecord = new PupilRecordPage();
                    pupilRecord.ClickParentalSalutation();
                    pupilRecord.ClickParentalAddressee();
                    pupilRecord.MailingPoint = true;

                    pupilRecord = PupilRecordPage.Create();
                    pupilRecord.SavePupil();

                    // Verify data is saved Success
                    Assert.AreEqual(true, pupilRecord.IsSuccessMessageDisplayed(), "Success message is not display");
                    Assert.AreNotEqual(null, pupilRecord.ContactTable[0], "The contact row is null");

                    //Verify The contact is indicated as a Mailing Point.
                    Assert.AreEqual(true, pupilRecord.MailingPoint);
                }
                finally
                {
                    PurgeLinkedData.DeleteLearnerContactRelationship(learnerId);
                }
            }
        }

        /// <summary>
        ///  Author: Y.Ta
        ///  Description: Verify add welfare information sucessfully
        /// </summary>
        ///
        [WebDriverTest(Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { PupilTestGroups.PupilRecord.AddNewPupil, PupilTestGroups.PupilRecord.Page, PupilTestGroups.Priority.Priority2 })]
        public void TC_PU011_Verify_Add_WelfareSection()
        {
            // Arrange
            DateTime dob = new DateTime(2006, 01, 06);
            DateTime doa = new DateTime(2010, 01, 06);

            var learnerId = Guid.NewGuid();
            string random = SeleniumHelper.GenerateRandomNumberUsingDateTime();
            string randomCharacter = SeleniumHelper.GenerateRandomString(6) + "_TC_PU011";
            string forename = "aForeName" + randomCharacter + random;
            string surname = "aSurName" + randomCharacter + random;

            string pattern = "d/M/yyyy";
            var now = DateTime.Now;
            var firstDay = new DateTime(now.Year, now.Month, 1);
            var lastDay = new DateTime(now.Year, 12, 12);
            string startDate = firstDay.ToString(pattern);
            string endDate = lastDay.ToString(pattern);
            var careAuthority = Queries.GetFirstLookupField("CareAuthority", "Name");
            var livingArrangement = Queries.GetFirstLookupField("LivingArrangement", "Name");

            var pupil = this.BuildDataPackage()
                    .AddBasicLearner(learnerId, surname, forename, dob, doa);

            //Act
            using (new DataSetup(false, true, pupil))
            {
                try
                {
                    // Login as school admin
                    SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);

                    // Navigate to Pupil Record
                    AutomationSugar.NavigateMenu("Tasks", "Pupils", "Pupil Records");
                    var pupilRecordTriplet = new PupilSearchTriplet();

                    // Open specific pupil record
                    pupilRecordTriplet.SearchCriteria.PupilName = String.Format("{0}, {1}", surname, forename);
                    var resultPupils = pupilRecordTriplet.SearchCriteria.Search();
                    var pupilSearchTile = resultPupils.SingleOrDefault(t => t.Name.Equals(String.Format("{0}, {1}", surname, forename)));

                    var pupilRecord = pupilSearchTile == null ? null : pupilSearchTile.Click<PupilRecordPage>();
                    Assert.AreNotEqual(null, pupilRecord, "Does not found pupil");

                    // Open Aditional section
                    pupilRecord.SelectWelfareTab();

                    var careArrangementDialog = pupilRecord.OpenAddCareArrangmentDialog();
                    careArrangementDialog.CareAuthority = careAuthority;
                    careArrangementDialog.LivingArrangement = livingArrangement;
                    careArrangementDialog.StartDate = startDate;
                    careArrangementDialog.EndDate = endDate;
                    careArrangementDialog.ClickOk();

                    pupilRecord = PupilRecordPage.Create();
                    pupilRecord.SavePupil();

                    // Verify data is saved Success
                    Assert.AreEqual(true, pupilRecord.IsSuccessMessageDisplayed(), "Success message is not display");
                    Assert.AreEqual(careAuthority, pupilRecord.LearnerInCareDetails[0].CareAuthority, "The CareAuthority is not correct");
                    Assert.AreEqual(livingArrangement, pupilRecord.LearnerInCareDetails[0].LivingArrangement, "The LivingArrangement is not correct");
                    Assert.AreEqual(startDate, pupilRecord.LearnerInCareDetails[0].StartDate, "The StartDate is not correct");
                    Assert.AreEqual(endDate, pupilRecord.LearnerInCareDetails[0].EndDate, "The EndDate is not correct");
                }
                finally
                {
                    PurgeLinkedData.DeleteLearnerInCareDetails(learnerId);
                }
            }
        }

        /// <summary>
        ///  Author: Y.Ta
        ///  Description: Verify add welfare education plan information sucessfully
        /// </summary>
        ///
        [WebDriverTest(Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { PupilTestGroups.PupilRecord.AddNewPupil, PupilTestGroups.PupilRecord.Page, PupilTestGroups.Priority.Priority2, })]
        public void TC_PU012_Verify_Add_Welfare_Education_Plan()
        {
            // Arrange
            DateTime dob = new DateTime(2006, 01, 06);
            DateTime doa = new DateTime(2010, 01, 06);

            var learnerId = Guid.NewGuid();
            string random = SeleniumHelper.GenerateRandomNumberUsingDateTime();
            string randomCharacter = SeleniumHelper.GenerateRandomString(6) + "_TC_PU012";
            string forename = "aForeName" + randomCharacter + random;
            string surname = "aSurName" + randomCharacter + random;

            var now = DateTime.Now;
            string startDate = now.ToString(_pattern);
            string endDate = now.AddMonths(3).ToString(_pattern);
            var secondStartDate = now.AddDays(1).ToString(_pattern);
            var secondEndDate = new DateTime(now.Year, 12, 25).ToString(_pattern);
            string notes = "Personal Education Plan Note";
            var careAuthority = Queries.GetFirstLookupField("CareAuthority", "Name");
            var livingArrangement = Queries.GetFirstLookupField("LivingArrangement", "Name");

            var pupil = this.BuildDataPackage()
                    .AddBasicLearner(learnerId, surname, forename, dob, doa);

            //Act
            using (new DataSetup(false, true, pupil))
            {
                try
                {
                    // Login as school admin
                    SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);

                    // Navigate to Pupil Record
                    AutomationSugar.NavigateMenu("Tasks", "Pupils", "Pupil Records");
                    Navigator.CloseSideMenu();

                    var pupilRecordTriplet = new PupilSearchTriplet();

                    // Open specific pupil record
                    pupilRecordTriplet.SearchCriteria.PupilName = String.Format("{0}, {1}", surname, forename);
                    var resultPupils = pupilRecordTriplet.SearchCriteria.Search();
                    var pupilSearchTile = resultPupils.SingleOrDefault(t => t.Name.Equals(String.Format("{0}, {1}", surname, forename)));

                    var pupilRecord = pupilSearchTile == null ? null : pupilSearchTile.Click<PupilRecordPage>();
                    Assert.AreNotEqual(null, pupilRecord, "Does not found pupil");

                    // Open Aditional section
                    pupilRecord.SelectWelfareTab();

                    var careArrangementDialog = pupilRecord.OpenAddCareArrangmentDialog();

                    careArrangementDialog.CareAuthority = careAuthority;
                    careArrangementDialog.LivingArrangement = livingArrangement;
                    careArrangementDialog.StartDate = startDate;
                    careArrangementDialog.EndDate = endDate;
                    careArrangementDialog.ClickOk();

                    pupilRecord = PupilRecordPage.Create();
                    pupilRecord.SavePupil();
                    AutomationSugar.WaitForAjaxCompletion();

                    var careArrangementsDialog = pupilRecord.LearnerInCareDetails[0].OpenEditCareArrangementsDialog();
                    var pepDialog = careArrangementsDialog.OpenAddPersonalEducationPlanDialog();
                    pepDialog.StartDate = startDate;
                    pepDialog.EndDate = endDate;
                    pepDialog.Notes = notes;
                    pepDialog.ClickOk();
                    careArrangementsDialog.ClickOk();
                    pupilRecord = PupilRecordPage.Create();
                    pupilRecord.SavePupil();
                    AutomationSugar.WaitForAjaxCompletion();

                    careArrangementsDialog = pupilRecord.LearnerInCareDetails[0].OpenEditCareArrangementsDialog();
                    careArrangementsDialog.PersonalEducationPlans[0].StartDate = secondStartDate;
                    careArrangementsDialog.PersonalEducationPlans[0].EndDate = secondEndDate;
                    careArrangementsDialog.ClickOk();

                    pupilRecord = PupilRecordPage.Create();
                    pupilRecord.SavePupil();
                    AutomationSugar.WaitForAjaxCompletion();

                    // Verify data is saved Success
                    Assert.AreEqual(true, pupilRecord.IsSuccessMessageDisplayed(), "Success message is not display");
                    careArrangementsDialog = pupilRecord.LearnerInCareDetails[0].OpenEditCareArrangementsDialog();

                    var careArrangementsRow = careArrangementsDialog.PersonalEducationPlans.Rows.SingleOrDefault(p => p.StartDate.Equals(secondStartDate));
                    Assert.AreEqual(secondEndDate, careArrangementsRow.EndDate, "The start date and end date value is not correct");
                    careArrangementsDialog.ClickOk();

                    pupilRecord = PupilRecordPage.Create();
                    pupilRecord.SavePupil();
                }
                finally
                {
                    AutomationSugar.WaitForAjaxCompletion();
                    PurgeLinkedData.DeleteLearnerInCareDetails(learnerId);
                }
            }
        }

        /// <summary>
        ///  Description: Verify add welfare education plan contributor information sucessfully
        /// </summary>
        ///
        [WebDriverTest(Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { PupilTestGroups.PupilRecord.AddNewPupil, PupilTestGroups.PupilRecord.Page, PupilTestGroups.Priority.Priority2 })]
        public void TC_PU012a_Verify_Add_Welfare_Education_Plan_Contributor()
        {
            // Arrange
            DateTime dob = new DateTime(2006, 01, 06);
            DateTime doa = new DateTime(2010, 01, 06);

            var learnerId = Guid.NewGuid();
            string random = SeleniumHelper.GenerateRandomNumberUsingDateTime();
            string randomCharacter = SeleniumHelper.GenerateRandomString(6) + "_TC_PU012a";
            string forename = "aForeName" + randomCharacter + random;
            string surname = "aSurName" + randomCharacter + random;

            var now = DateTime.Now;
            string startDate = now.ToString(_pattern);
            string endDate = now.AddMonths(3).ToString(_pattern);
            string notes = "Personal Education Plan Note";
            var careAuthority = Queries.GetFirstLookupField("CareAuthority", "Name");
            var livingArrangement = Queries.GetFirstLookupField("LivingArrangement", "Name");

            var pupil = this.BuildDataPackage()
                    .AddBasicLearner(learnerId, surname, forename, dob, doa);

            //Act
            using (new DataSetup(false, true, pupil))
            {
                try
                {
                    // Login as school admin
                    SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);

                    // Navigate to Pupil Record
                    AutomationSugar.NavigateMenu("Tasks", "Pupils", "Pupil Records");
                    var pupilRecordTriplet = new PupilSearchTriplet();

                    // Open specific pupil record
                    pupilRecordTriplet.SearchCriteria.PupilName = String.Format("{0}, {1}", surname, forename);
                    var resultPupils = pupilRecordTriplet.SearchCriteria.Search();
                    var pupilSearchTile = resultPupils.SingleOrDefault(t => t.Name.Equals(String.Format("{0}, {1}", surname, forename)));

                    var pupilRecord = pupilSearchTile == null ? null : pupilSearchTile.Click<PupilRecordPage>();
                    Assert.AreNotEqual(null, pupilRecord, "Does not found pupil");

                    // Open Aditional section
                    pupilRecord.SelectWelfareTab();

                    var careArrangementDialog = pupilRecord.OpenAddCareArrangmentDialog();

                    careArrangementDialog.CareAuthority = careAuthority;
                    careArrangementDialog.LivingArrangement = livingArrangement;
                    careArrangementDialog.StartDate = startDate;
                    careArrangementDialog.EndDate = endDate;
                    careArrangementDialog.ClickOk();

                    pupilRecord = PupilRecordPage.Create();
                    pupilRecord.SavePupil();

                    var careArrangementsDialog = pupilRecord.LearnerInCareDetails[0].OpenEditCareArrangementsDialog();
                    var pepDialog = careArrangementsDialog.OpenAddPersonalEducationPlanDialog();

                    pepDialog.StartDate = startDate;
                    pepDialog.EndDate = endDate;
                    pepDialog.Notes = notes;
                    pepDialog.ClickOk();
                    careArrangementsDialog.ClickOk();

                    pupilRecord = PupilRecordPage.Create();
                    pupilRecord.SavePupil();

                    // Plan Contributors Dialog
                    careArrangementsDialog = pupilRecord.LearnerInCareDetails[0].OpenEditCareArrangementsDialog();
                    var pepContributorsDialog = careArrangementsDialog.PersonalEducationPlans[0].OpenEditPersonalEducationPlanDialog();

                    var contributorContactDialog = pepContributorsDialog.OpenAddPepContributorContactDialog();
                    contributorContactDialog.SearchCriteria.SurName = "a";

                    var searchStaffResults = contributorContactDialog.SearchCriteria.Search();
                    var staffTile = searchStaffResults[0];
                    staffTile.Click();

                    pepContributorsDialog = contributorContactDialog.ClickPepContributorsOk();

                    pepContributorsDialog.ClickOk();
                    careArrangementsDialog.ClickOk();
                    pupilRecord = PupilRecordPage.Create();
                    pupilRecord.SavePupil();
                }
                finally
                {
                    PurgeLinkedData.DeleteLearnerInCareDetails(learnerId);
                }
            }
        }

        /// <summary>
        ///  Author: Y.Ta
        ///  Description: Verify add medical information sucessfully
        /// </summary>
        [WebDriverTest(Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { PupilTestGroups.PupilRecord.AddNewPupil, PupilTestGroups.PupilRecord.Page, PupilTestGroups.Priority.Priority2 })]
        public void TC_PU013_Verify_Add_School_History_Section()
        {
            // Arrange
            DateTime dob = new DateTime(2006, 01, 06);
            DateTime doa = new DateTime(2010, 01, 06);

            var learnerId = Guid.NewGuid();
            string random = SeleniumHelper.GenerateRandomNumberUsingDateTime();
            string randomCharacter = SeleniumHelper.GenerateRandomString(6) + "_TC_PU013";
            string forename = "aForeName" + randomCharacter + random;
            string surname = "aSurName" + randomCharacter + random;

            var endDate = DateTime.ParseExact("03/03/2016", _pattern, CultureInfo.InvariantCulture).ToString(_pattern);
            var startDate = DateTime.ParseExact("03/02/2016", _pattern, CultureInfo.InvariantCulture).ToString(_pattern);
            var enrolmentStatus = Queries.GetEnrolmentStatus().Description;
            var reasonForLeaving = Queries.GetFirstReasonForLeavingLookup().Description;

            var pupil = this.BuildDataPackage()
                    .AddBasicLearner(learnerId, surname, forename, dob, doa);

            //Act
            using (new DataSetup(false, true, pupil))
            {
                try
                {
                    // Login as school admin
                    SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);

                    // Navigate to Pupil Record
                    AutomationSugar.NavigateMenu("Tasks", "Pupils", "Pupil Records");
                    var pupilRecordTriplet = new PupilSearchTriplet();

                    // Open specific pupil record
                    pupilRecordTriplet.SearchCriteria.PupilName = String.Format("{0}, {1}", surname, forename);
                    var resultPupils = pupilRecordTriplet.SearchCriteria.Search();
                    var pupilSearchTile = resultPupils.SingleOrDefault(t => t.Name.Equals(String.Format("{0}, {1}", surname, forename)));

                    var pupilRecord = pupilSearchTile == null ? null : pupilSearchTile.Click<PupilRecordPage>();
                    Assert.AreNotEqual(null, pupilRecord, "Does not found pupil");

                    // Open Select School History section
                    pupilRecord.SelectSchoolHistoryTab();

                    var addPreviousSchoolHistoryDialog = pupilRecord.OpenAddSchoolHistoryDialog();

                    // Select school
                    var selectSchoolDialog = addPreviousSchoolHistoryDialog.ClickSelectPreviousSchool();
                    selectSchoolDialog.SearchCriteria.SchoolName = "Q";
                    var resultListItems = selectSchoolDialog.SearchCriteria.Search();
                    var selectSchoolTripletDialog = resultListItems[1].Click<SelectSchoolTripletDialog>();
                    selectSchoolTripletDialog.ClickOk();

                    addPreviousSchoolHistoryDialog.ClickOk();

                    // Edit inline SchoolHistory grid
                    pupilRecord.LearnerPreviousSchools[0].StartDate = startDate;
                    pupilRecord.LearnerPreviousSchools[0].EndDate = endDate;
                    pupilRecord.LearnerPreviousSchools[0].EnrolmentStatus = enrolmentStatus;
                    pupilRecord.LearnerPreviousSchools[0].ReasonForLeaving = reasonForLeaving;

                    pupilRecord = PupilRecordPage.Create();
                    pupilRecord.SavePupil();

                    // Verify data is saved Success
                    Assert.AreEqual(true, pupilRecord.IsSuccessMessageDisplayed(), "Success message is not display");
                    Assert.IsFalse(string.IsNullOrEmpty(pupilRecord.LearnerPreviousSchools[0].SchoolName), "School name not displayed on the School History table");
                }
                finally
                {
                    PurgeLinkedData.DeleteLearnerPreviousSchool(learnerId);
                }
            }
        }

        /// <summary>
        ///  Author: Y.Ta
        ///  Description: Verify edit  school hictory information sucessfully
        /// </summary>
        [WebDriverTest(Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { PupilTestGroups.PupilRecord.AddNewPupil, PupilTestGroups.PupilRecord.Page, PupilTestGroups.Priority.Priority2 })]
        public void TC_PU014_Verify_Edit_School_History_Section()
        {
            // Arrange
            DateTime dob = new DateTime(2006, 01, 06);
            DateTime doa = new DateTime(2010, 01, 06);

            var learnerId = Guid.NewGuid();
            string random = SeleniumHelper.GenerateRandomNumberUsingDateTime();
            string randomCharacter = SeleniumHelper.GenerateRandomString(6) + "_TC_PU014";
            string forename = "aForeName" + randomCharacter + random;
            string surname = "aSurName" + randomCharacter + random;

            var endDate = DateTime.ParseExact("03/03/2016", _pattern, CultureInfo.InvariantCulture).ToString(_pattern);
            var startDate = DateTime.ParseExact("03/02/2016", _pattern, CultureInfo.InvariantCulture).ToString(_pattern);
            var enrolmentStatus = Queries.GetEnrolmentStatus().Description;
            var reasonForLeaving = Queries.GetFirstReasonForLeavingLookup().Description;

            var pupil = this.BuildDataPackage()
                    .AddBasicLearner(learnerId, surname, forename, dob, doa);

            //Act
            using (new DataSetup(false, true, pupil))
            {
                try
                {
                    // Login as school admin
                    SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);

                    // Navigate to Pupil Record
                    AutomationSugar.NavigateMenu("Tasks", "Pupils", "Pupil Records");
                    var pupilRecordTriplet = new PupilSearchTriplet();

                    // Open specific pupil record
                    pupilRecordTriplet.SearchCriteria.PupilName = String.Format("{0}, {1}", surname, forename);
                    var resultPupils = pupilRecordTriplet.SearchCriteria.Search();
                    var pupilSearchTile = resultPupils.SingleOrDefault(t => t.Name.Equals(String.Format("{0}, {1}", surname, forename)));

                    var pupilRecord = pupilSearchTile == null ? null : pupilSearchTile.Click<PupilRecordPage>();
                    Assert.AreNotEqual(null, pupilRecord, "Does not found pupil");

                    // Open Select School History section
                    pupilRecord.SelectSchoolHistoryTab();

                    var addPreviousSchoolHistoryDialog = pupilRecord.OpenAddSchoolHistoryDialog();

                    // Select school
                    var selectSchoolDialog = addPreviousSchoolHistoryDialog.ClickSelectPreviousSchool();
                    selectSchoolDialog.SearchCriteria.SchoolName = "Q";
                    var resultListItems = selectSchoolDialog.SearchCriteria.Search();
                    var selectSchoolTripletDialog = resultListItems[1].Click<SelectSchoolTripletDialog>();
                    selectSchoolTripletDialog.ClickOk();

                    addPreviousSchoolHistoryDialog.StartDate = startDate;
                    addPreviousSchoolHistoryDialog.EndDate = endDate;
                    addPreviousSchoolHistoryDialog.EnrolmentStatus = enrolmentStatus;
                    addPreviousSchoolHistoryDialog.ReasonForLeaving = reasonForLeaving;
                    addPreviousSchoolHistoryDialog.ClickOk();

                    pupilRecord = PupilRecordPage.Create();
                    pupilRecord.SavePupil();

                    var editPreviousSchoolHistoryDialog = pupilRecord.LearnerPreviousSchools[0].OpenEditSchoolHistoryDialog();
                    editPreviousSchoolHistoryDialog.AddAttendanceSummaryRow();

                    var year = DateTime.Now.Year;
                    editPreviousSchoolHistoryDialog.AttendanceSummaryTable[0].Year = string.Format("{0}/{1}", year, year + 1);
                    editPreviousSchoolHistoryDialog.AttendanceSummaryTable[0].PossibleSessions = "432";
                    editPreviousSchoolHistoryDialog.AttendanceSummaryTable[0].AttendedSessions = "423";
                    editPreviousSchoolHistoryDialog.AttendanceSummaryTable[0].AuthorisedSessions = "9";
                    editPreviousSchoolHistoryDialog.AttendanceSummaryTable[0].UnauthorisedSessions = "0";

                    editPreviousSchoolHistoryDialog.ClickOk(4);

                    pupilRecord = PupilRecordPage.Create();
                    pupilRecord.SavePupil();

                    // Verify data is saved Success
                    Assert.AreEqual(true, pupilRecord.IsSuccessMessageDisplayed(), "Success message is not display");
                    Assert.IsFalse(string.IsNullOrEmpty(pupilRecord.LearnerPreviousSchools[0].SchoolName), "School name not displayed on the School History table");
                    Assert.AreEqual("1", pupilRecord.LearnerPreviousSchools[0].AttendanceSummaryCount, "Attendance Summary count is incorrect on the School History table");
                }
                finally
                {
                    PurgeLinkedData.DeleteLearnerPreviousSchool(learnerId);
                }
            }
        }

        /// <summary>
        ///  Author: Y.Ta
        ///  Description: Verify edit consent information sucessfully
        /// </summary>
        ///
        [WebDriverTest(Browsers = new[] { BrowserDefaults.Chrome },
            Groups = new[] { PupilTestGroups.PupilRecord.AddNewPupil, PupilTestGroups.PupilRecord.Page, PupilTestGroups.Priority.Priority2 })]
        public void TC_PU015_Verify_Edit_Consents_Section()
        {
            // Arrange
            DateTime dob = new DateTime(2006, 01, 06);
            DateTime doa = new DateTime(2010, 01, 06);

            var learnerId = Guid.NewGuid();
            string random = SeleniumHelper.GenerateRandomNumberUsingDateTime();
            string randomCharacter = SeleniumHelper.GenerateRandomString(6) + "_TC_PU05";
            string forename = "aForeName" + randomCharacter + random;
            string surname = "aSurName" + randomCharacter + random;

            string consentSignatory = "Parent Signatory Received";
            string consentNotes = "Parental consent status duly recorded";
            var consentStatus = Queries.GetFirstConsentStatus();
            var doaInput = DateTime.ParseExact(doa.ToString(_pattern), _pattern, CultureInfo.InvariantCulture).ToString(_pattern);
            var pupil = this.BuildDataPackage()
                .AddBasicLearner(learnerId, surname, forename, dob, doa);

            //Act
            using (new DataSetup(false, true, pupil))
            {
                try
                {
                    // Login as school admin
                    SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);

                    // Navigate to Pupil Record
                    AutomationSugar.NavigateMenu("Tasks", "Pupils", "Pupil Records");
                    var pupilRecordTriplet = new PupilSearchTriplet();

                    // Open specific pupil record
                    pupilRecordTriplet.SearchCriteria.PupilName = String.Format("{0}, {1}", surname, forename);
                    var resultPupils = pupilRecordTriplet.SearchCriteria.Search();
                    var pupilSearchTile = resultPupils.SingleOrDefault(t => t.Name.Equals(String.Format("{0}, {1}", surname, forename)));

                    var pupilRecord = pupilSearchTile == null ? null : pupilSearchTile.Click<PupilRecordPage>();
                    Assert.AreNotEqual(null, pupilRecord, "Does not found pupil");

                    // Open Aditional section
                    pupilRecord.SelectConsentsTab();

                    var row = pupilRecord.Consents[0];
                    row.Active = true;
                    row.ConsentStatus = consentStatus.Description;
                    row.Date = doaInput;
                    row.ConsentSignatory = consentSignatory;
                    row.Note = consentNotes;

                    pupilRecord = PupilRecordPage.Create();
                    pupilRecord.SavePupil();
                    // Verify data is saved Success
                    Assert.AreEqual(true, pupilRecord.IsSuccessMessageDisplayed(), "Success message is not display");
                }
                finally
                {
                    PurgeLinkedData.DeleteLearnerConsents(learnerId);
                }
            }
        }

        /// <summary>
        ///  Author: Y.Ta
        ///  Description: Verify add statutorySEN information sucessfully
        ///  SERIAL ONLY
        /// </summary>
        ///
        [WebDriverTest(Browsers = new[] { BrowserDefaults.Chrome },
            Groups = new[] { PupilTestGroups.PupilRecord.AddNewPupil, PupilTestGroups.PupilRecord.Page, PupilTestGroups.Priority.Priority2, "AddSEN" })]
        public void PupilRecord_Add_SEN()
        {
            // Arrange
            DateTime dob = new DateTime(2006, 06, 06);
            DateTime doa = new DateTime(2010, 06, 06);

            var learnerId = Guid.NewGuid();
            string randomCharacter = SeleniumHelper.GenerateRandomString(6);
            string forename = "AddSEN";
            string surname = "AddSEN" + randomCharacter;
            string senStage = Queries.GetLookupDescriptionByCode("SENStatus", "2", isVisibleOnly: true);
            string senNeedType = Queries.GetLookupDescriptionByCode("SENNeedType", "MLD", isVisibleOnly: true);
            //string senNeedsRank = "1";
            string senNeedsDescription = "Notes for SEN Need";
            string doaInput = DateTime.ParseExact(doa.ToString(_pattern), _pattern, CultureInfo.InvariantCulture).ToString(_pattern);

            string senProvisionsType = Queries.GetLookupDescriptionByCode("SENProvisionType", "NOTSPEC", isVisibleOnly: true);
            //string comment = "Requested Time in Special Unit";

            var pupil = this.BuildDataPackage().AddBasicLearner(learnerId, surname, forename, dob, doa);

            //Act
            using (new DataSetup(false, true, pupil))
            {
                try
                {
                    // Login as school admin
                    SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);
                    AutomationSugar.WaitForAjaxCompletion();

                    // Navigate to Pupil Record
                    AutomationSugar.NavigateMenu("Tasks", "Pupils", "Pupil Records");
                    AutomationSugar.WaitForAjaxCompletion();

                    var pupilRecordTriplet = new PupilSearchTriplet();
                    AutomationSugar.WaitForAjaxCompletion();

                    // Open specific pupil record
                    pupilRecordTriplet.SearchCriteria.PupilName = String.Format("{0}, {1}", surname, forename);
                    var resultPupils = pupilRecordTriplet.SearchCriteria.Search();
                    AutomationSugar.WaitForAjaxCompletion();

                    var pupilSearchTile =
                        resultPupils.SingleOrDefault(
                            t => t.Name.Equals(String.Format("{0}, {1}", surname, forename)));

                    var pupilRecord = pupilSearchTile == null ? null : pupilSearchTile.Click<PupilRecordPage>();
                    AutomationSugar.WaitForAjaxCompletion();

                    Assert.AreNotEqual(null, pupilRecord, "Cannot find pupil created in pre-test data");

                    // Open Aditional section
                    pupilRecord.SelectStatutorySenTab();
                    AutomationSugar.WaitForAjaxCompletion();

                    pupilRecord.ClickAddSENStage();
                    AutomationSugar.WaitForAjaxCompletion();

                    pupilRecord.SenStages[0].Stage = senStage;
                    pupilRecord.SenStages[0].StartDay = doaInput;

                    pupilRecord.SenNeeds.Refresh();

                    pupilRecord.ClickAddSENNeed();

                    pupilRecord.SenNeeds[0].NeedType = senNeedType;
                    //  pupilRecord.SenNeeds[0].Rank = senNeedsRank;
                    pupilRecord.SenNeeds[0].Notes = senNeedsDescription;
                    pupilRecord.SenNeeds[0].StartDay = doaInput;
                    pupilRecord.SenNeeds[0].EndDay = SeleniumHelper.GetToday();

                    pupilRecord = PupilRecordPage.Create();
                    AutomationSugar.WaitForAjaxCompletion();

                    pupilRecord.SavePupil();
                    AutomationSugar.WaitForAjaxCompletion();

                    pupilRecord.ClickAddSENProvision();
                    pupilRecord.SenProvisions[0].ProvisionType = senProvisionsType;
                    pupilRecord.SenProvisions[0].StartDay = doaInput;

                    pupilRecord = PupilRecordPage.Create();
                    AutomationSugar.WaitForAjaxCompletion();

                    pupilRecord.SavePupil();
                    AutomationSugar.WaitForAjaxCompletion();

                    // Add document
                    pupilRecord.SenNeeds[0].AddDocument();
                    ViewDocumentDialog viewDocument = new ViewDocumentDialog();
                    viewDocument.ClickOk();
                    AutomationSugar.WaitForAjaxCompletion();

                    pupilRecord = PupilRecordPage.Create();
                    pupilRecord.SavePupil();
                    //AutomationSugar.WaitForAjaxCompletion();

                    // Verify data is saved Success
                    Assert.AreEqual(true, pupilRecord.IsSuccessMessageDisplayed(), "Success message is not displayed");
                }
                finally
                {
                    PurgeLinkedData.DeleteLearnerSenData(learnerId);
                }
            }
        }

        /// <summary>
        /// Role: School Administrator
        /// </summary>
        [WebDriverTest(Browsers = new[] { BrowserDefaults.Chrome },
            Groups = new[] { PupilTestGroups.PupilRecord.AddNewPupil, PupilTestGroups.PupilRecord.Page, PupilTestGroups.Priority.Priority2 })]
        public void PupilRecord_SearchFuturePupil()
        {
            // Arrange
            DateTime dob = new DateTime(2006, 01, 06);
            DateTime doa = new DateTime(DateTime.Now.Year + 1, 01, 06);

            var learnerId = Guid.NewGuid();
            string randomCharacter = SeleniumHelper.GenerateRandomString(6);
            string forename = "SearchFuturePupil";
            string surname = "SearchFuturePupil" + randomCharacter;

            var pupil = this.BuildDataPackage()
                .AddBasicLearner(learnerId, surname, forename, dob, doa);

            //Act
            using (new DataSetup(false, true, pupil))
            {

                SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);

                //Search Current Pupil
                SeleniumHelper.NavigateQuickLink("Pupil Records");

                //Search Future Pupil
                var pupilRecordTriplet = new PupilSearchTriplet();
                pupilRecordTriplet.SearchCriteria.PupilName = String.Format("{0}, {1}", surname, forename);
                pupilRecordTriplet.SearchCriteria.IsCurrent = false;
                pupilRecordTriplet.SearchCriteria.IsFuture = true;
                pupilRecordTriplet.SearchCriteria.IsLeaver = false;
                var pupilSearchResults = pupilRecordTriplet.SearchCriteria.Search();

                //Verify Search Future Pupil
                var pupilTile = pupilSearchResults.FirstOrDefault(t => t.Name.Equals(String.Format("{0}, {1}", surname, forename)));
                Assert.AreNotEqual(null, pupilTile, "Search future pupil unsuccessfull");
            }
        }

        /// <summary>
        /// Role: School Administrator
        /// </summary>
        [WebDriverTest(Browsers = new[] { BrowserDefaults.Chrome },
            Groups = new[] { PupilTestGroups.PupilRecord.AddNewPupil, PupilTestGroups.PupilRecord.Page, PupilTestGroups.Priority.Priority2 })]
        public void PupilRecord_SearchFormerPupil()
        {
            // Arrange
            DateTime dob = new DateTime(2006, 01, 06);
            DateTime doa = new DateTime(2010, 01, 06);
            DateTime dol = new DateTime(2013, 07, 12);

            var learnerId = Guid.NewGuid();
            string randomCharacter = SeleniumHelper.GenerateRandomString(6);
            string forename = "SearchFormerPupil";
            string surname = "SearchFormerPupil" + randomCharacter;
            var reasonForLeaving = Queries.GetFirstReasonForLeavingLookup();

            var pupil = this.BuildDataPackage()
                .AddLeaver(learnerId, surname, forename, dob, doa, dol, reasonForLeaving: reasonForLeaving.Code);

            //Act
            using (new DataSetup(false, true, pupil))
            {

                SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);

                //Search Former Pupil
                SeleniumHelper.NavigateQuickLink("Pupil Records");
                var pupilRecordTriplet = new PupilSearchTriplet();
                pupilRecordTriplet.SearchCriteria.PupilName = String.Format("{0}, {1}", surname, forename);
                pupilRecordTriplet.SearchCriteria.IsCurrent = false;
                pupilRecordTriplet.SearchCriteria.IsFuture = false;
                pupilRecordTriplet.SearchCriteria.IsLeaver = true;
                var pupilSearchResults = pupilRecordTriplet.SearchCriteria.Search();

                //Verify Search Former Pupil
                var pupilTile = pupilSearchResults.FirstOrDefault(t => t.Name.Equals(String.Format("{0}, {1}", surname, forename)));
                Assert.AreNotEqual(null, pupilTile, "Search former pupil unsuccessfull");
            }
        }

        /// <summary>
        /// TC PU24
        /// Au : An Nguyen
        /// Description: Exercise ability to record a part-time attendance pattern for a pupil that is not 'Full Time' at the school.
        /// Role: School Administrator
        /// </summary>
        [WebDriverTest(Browsers = new[] { BrowserDefaults.Chrome },
            Groups = new[] { PupilTestGroups.PupilRecord.AddNewPupil, PupilTestGroups.PupilRecord.Page, PupilTestGroups.Priority.Priority2 })]
        public void TC_PU024_Exercise_ability_to_record_a_part_time_attendance_pattern()
        {
            //Arrange
            var learnerId = Guid.NewGuid();
            DateTime dob = new DateTime(2006, 01, 06);
            DateTime doa = new DateTime(2010, 01, 06);
            string random = SeleniumHelper.GenerateRandomNumberUsingDateTime();
            string randomCharacter = SeleniumHelper.GenerateRandomString(6) + "_TC_PU24";
            string foreName = "aForeName" + randomCharacter + random;
            string surName = "aSurName" + randomCharacter + random;

            //string attendanceMode = Queries.GetLookupDescriptionByCode("AttendanceMode", "A", isVisibleOnly: true);

            var pupil = this.BuildDataPackage()
                .AddBasicLearner(learnerId, surName, foreName, dob, doa);
            pupil.AddLearnerAttendanceMode(learnerId, doa);

            using (new DataSetup(false, false, pupil))
            {
                SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);

                // Navigate to Pupil Record
                AutomationSugar.NavigateMenu("Tasks", "Pupils", "Pupil Records");

                var pupilRecordTriplet = new PupilSearchTriplet();

                // Open specific pupil record
                pupilRecordTriplet.SearchCriteria.PupilName = String.Format("{0}, {1}", surName, foreName);
                var resultPupils = pupilRecordTriplet.SearchCriteria.Search();
                var pupilSearchTile =
                    resultPupils.SingleOrDefault(
                        t => t.Name.Equals(String.Format("{0}, {1}", surName, foreName)));

                var pupilRecord = pupilSearchTile == null ? null : pupilSearchTile.Click<PupilRecordPage>();

                var historyAttendanceModeDialog = pupilRecord.ViewAttendanceModeHistory();
                Thread.Sleep(2);
                historyAttendanceModeDialog.SelectAttendanceMode();
                Thread.Sleep(2);

                historyAttendanceModeDialog.CloseAttendanceModeOK();

                pupilRecord = new PupilRecordPage();
                Thread.Sleep(2);
                pupilRecord.ClickSave();

                Assert.AreEqual(true, pupilRecord.IsSuccessMessageDisplayed(), "Success message is displayed");
            }
        }

        /// <summary>
        /// Descriptions: Verify Previous Name History.
        /// </summary>
        [WebDriverTest(Browsers = new[] { BrowserDefaults.Chrome },
            Groups = new[] { PupilTestGroups.PupilRecord.AddNewPupil, PupilTestGroups.PupilRecord.Page, PupilTestGroups.Priority.Priority2, })]
        public void PupilRecord_VerifyNameHistory()
        {
            //Arrange
            string forename = "VerifyNameHistory";
            string surname = "VerifyNameHistory" + SeleniumHelper.GenerateRandomString(10);

            DateTime dob = new DateTime(2011, 02, 02);
            var learnerId = Guid.NewGuid();

            var pupil = this.BuildDataPackage()
                       .AddBasicLearner(learnerId, surname, forename, dob, dateOfAdmission: new DateTime(2011, 10, 05));

            using (new DataSetup(false, true, pupil))
            {
                try
                {
                    SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);

                    // Navigate to Pupil Record
                    AutomationSugar.NavigateMenu("Tasks", "Pupils", "Pupil Records");

                    var pupilRecordTriplet = new PupilSearchTriplet();
                    pupilRecordTriplet.SearchCriteria.PupilName = String.Format("{0}, {1}", surname, forename);
                    var resultPupils = pupilRecordTriplet.SearchCriteria.Search();
                    var pupilSearchTile = resultPupils.SingleOrDefault(t => t.Name.Equals(String.Format("{0}, {1}", surname, forename)));
                    var pupilRecord = pupilSearchTile == null ? null : pupilSearchTile.Click<PupilRecordPage>();

                    // Navigate to 'Previous Name History'
                    var nameChangeHistoryPage = SeleniumHelper.NavigateViaAction<NameChangeHistoryPage>("Previous Names");
                    var nameHistoryTable = nameChangeHistoryPage.NameHistoryTable;

                    // Verify 'Name change history' page displays for this pupil
                    Assert.AreEqual(true, nameChangeHistoryPage.IsNameChangeHistoryForPupilName(String.Format("{0}, {1}", surname, forename)), "Name change history page displays for another pupil");

                    var newForename = "VerifyNameHistoryF" + SeleniumHelper.GenerateRandomString(5);
                    var newSurname = "VerifyNameHistoryS" + SeleniumHelper.GenerateRandomString(5);
                    var middlename = "VerifyNameHistoryM" + SeleniumHelper.GenerateRandomString(5);
                    var reason = "Not Given";
                    var dateOfChange = DateTime.ParseExact("01/01/2016", _pattern, CultureInfo.InvariantCulture).ToString(_pattern);

                    // Enter values
                    nameChangeHistoryPage.AddPreviousLegalName();
                    nameHistoryTable = nameChangeHistoryPage.NameHistoryTable;
                    var emptyRow = nameHistoryTable.Rows.FirstOrDefault(x => x.LegalForeName.Trim().Equals(""));
                    emptyRow.LegalForeName = newForename;
                    emptyRow.LegalMiddleName = middlename;
                    emptyRow.LegalSurName = newSurname;
                    emptyRow.Reason = reason;
                    emptyRow.DateOfChange = dateOfChange;

                    // Save values
                    nameChangeHistoryPage.Save();
                }
                finally
                {
                    PurgeLinkedData.DeleleLearnerPreviousName(learnerId);
                }
            }
        }


        #region Pupil Premium
        private const string PupilPremiumFeature = "Pupil%20Premium";

        [WebDriverTest(Enabled = true, Browsers = new[] { BrowserDefaults.Chrome },
            Groups = new[] { PupilTestGroups.PupilRecord.AddNewPupil, PupilTestGroups.PupilRecord.Page, PupilTestGroups.Priority.Priority2, "RSB" })]
        [Variant(Variant.EnglishStatePrimary)]
        public void PupilRecord_PupilPremiumDetailsDisplayed_IfLoggedInAsTeacher()
        {
            //Arrange
            string forename = "Premium1";
            string surname = "PupilTest" + SeleniumHelper.GenerateRandomString(10);

            DateTime dob = new DateTime(2011, 02, 02);
            var learnerId = Guid.NewGuid();

            var pupil = this.BuildDataPackage()
                .AddBasicLearner(learnerId, surname, forename, dob, dateOfAdmission: new DateTime(2011, 10, 05))
                .AddPupilPremiumEligibitity(learnerId)
                .AddPupilPremiumGrant(learnerId);

            using (new DataSetup(false, true, pupil))
            {
                //SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.ClassTeacher, false, enabledFeatures: PupilPremiumFeature);

                SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.ClassTeacher, false, PupilPremiumFeature);
                AutomationSugar.WaitForAjaxCompletion();
                Wait.WaitForDocumentReady();

                // Click Pupil Menu Item
                AutomationSugar.NavigateMenu("Tasks", "Pupils", "Pupil Records");
                var pupilRecordTriplet = new PupilSearchTriplet();

                // Search for pupil
                pupilRecordTriplet.SearchCriteria.PupilName = $"{surname}, {forename}";
                var resultPupils = pupilRecordTriplet.SearchCriteria.Search();
                var pupilSearchTile = resultPupils.SingleOrDefault(t => t.Name.Equals($"{surname}, {forename}"));

                // Load pupil
                var pupilRecord = pupilSearchTile?.Click<PupilRecordPage>();

                // Open Welfare section
                AutomationSugar.ExpandAccordionPanel("section_menu_Welfare");

                // Allow time for accordion to expand (AJAX call)
                AutomationSugar.WaitForAjaxCompletion();
                Wait.WaitForDocumentReady();

                // Can I (Class Teacher) see the Pupil Premium Indicator
                Assert.IsTrue(!string.IsNullOrEmpty(pupilRecord?.PupilPremiumIndicator));
            }
        }
        #endregion

        /// <summary>
        /// Description: Update pupil's enrolment history. 
        /// Role: School Administrator
        /// </summary>
        [WebDriverTest(Browsers = new[] { BrowserDefaults.Chrome },
            Groups = new[] { PupilTestGroups.PupilRecord.AddNewPupil, PupilTestGroups.PupilRecord.Page, PupilTestGroups.Priority.Priority2, "TESTME" })]
        public void PupilRecord_UpdateEnrolmentHistory()
        {
            //Arrange
            string forename = "UpdateEnrolmentHistory";
            string surname = "UpdateEnrolmentHistory" + SeleniumHelper.GenerateRandomString(10);

            DateTime dob = new DateTime(2011, 02, 02);
            var learnerId = Guid.NewGuid();

            var pupil = this.BuildDataPackage()
                       .AddBasicLearner(learnerId, surname, forename, dob, dateOfAdmission: new DateTime(2011, 10, 05));

            using (new DataSetup(false, true, pupil))
            {
                try
                {
                    SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);

                    // Navigate to Pupil Record
                    AutomationSugar.NavigateMenu("Tasks", "Pupils", "Pupil Records");

                    var pupilRecordTriplet = new PupilSearchTriplet();

                    pupilRecordTriplet.SearchCriteria.PupilName = String.Format("{0}, {1}", surname, forename);
                    var resultPupils = pupilRecordTriplet.SearchCriteria.Search();
                    var pupilSearchTile = resultPupils.SingleOrDefault(t => t.Name.Equals(String.Format("{0}, {1}", surname, forename)));

                    var pupilRecord = pupilSearchTile == null ? null : pupilSearchTile.Click<PupilRecordPage>();
                    Assert.AreNotEqual(null, pupilRecord, "pupil not found");

                    //Navigate to the 'Registration' section
                    // pupilRecord.SelectRegistrationTab();
                    AutomationSugar.ExpandAccordionPanel("section_menu_Registration");

                    var endDate = DateTime.ParseExact("02/03/2016", _pattern, CultureInfo.InvariantCulture).ToString(_pattern);
                    var startDate = DateTime.ParseExact("03/03/2016", _pattern, CultureInfo.InvariantCulture).ToString(_pattern);

                    //Change 'Single Registration' to 'Guest Pupil'
                    var enrolmentRows = pupilRecord.EnrolmentStatusHistoryTable.Rows;
                    var enrolmentRow = enrolmentRows.FirstOrDefault(t => t.EnrolmentStatus.Equals("Single Registration"));
                    enrolmentRow.EnrolmentStatus = "Guest Pupil";
                    enrolmentRow.EndDate = endDate;

                    //Add new 'Subsidiary - Dual registration'
                    pupilRecord.ClickAddEnrolmentStatus();
                    enrolmentRows = pupilRecord.EnrolmentStatusHistoryTable.Rows;
                    enrolmentRow = enrolmentRows.Last();
                    enrolmentRow.EnrolmentStatus = "Subsidiary – Dual Registration";
                    enrolmentRow.StartDate = startDate;

                    //Save pupil
                    pupilRecord.SavePupil();
                    Assert.AreEqual(true, pupilRecord.IsSuccessMessageDisplayed(), "Pupil can not be saved");
                    //pupilRecord.SelectRegistrationTab();
                    AutomationSugar.ExpandAccordionPanel("section_menu_Registration");

                    enrolmentRows = pupilRecord.EnrolmentStatusHistoryTable.Rows;

                    //Verify 'Subsidiary - Dual registration'
                    enrolmentRow = enrolmentRows.FirstOrDefault(t => t.EnrolmentStatus.Equals("Subsidiary – Dual Registration"));
                    Assert.AreEqual(startDate, enrolmentRow.StartDate, "Start Date of Subsidiary – Dual Registration is incorrect");
                }
                finally
                {
                    PurgeLinkedData.DeleteLearnerEnrolmentStatusForPupil(learnerId);
                    PurgeLinkedData.DeleteLearnerEnrolmentForPupil(learnerId);
                }
            }
        }

        /// <summary>
        /// Descriptions: Add achivement using the quick add dialog.
        /// </summary>
        //[WebDriverTest(Browsers = new[] { BrowserDefaults.Chrome },
        //    Groups = new[] { PupilTestGroups.PupilRecord.PupilRecordConduct, PupilTestGroups.Priority.Priority2, })]
        public void PupilRecord_AddAchievementEvent()
        {
            //Arrange
            string forename = "AddAchievementEvent";
            string surname = "AddAchievementEvent" + SeleniumHelper.GenerateRandomString(10);

            DateTime dob = new DateTime(2011, 02, 02);
            var learnerId = Guid.NewGuid();

            var pupil = this.BuildDataPackage()
                       .AddBasicLearner(learnerId, surname, forename, dob, dateOfAdmission: new DateTime(2011, 10, 05));

            using (new DataSetup(false, true, pupil))
            {
                try
                {

                    SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator, enabledFeatures: "Conduct");

                    // Navigate to Pupil Record
                    AutomationSugar.NavigateMenu("Tasks", "Pupils", "Pupil Records");

                    var pupilRecordTriplet = new PupilSearchTriplet();
                    pupilRecordTriplet.SearchCriteria.PupilName = String.Format("{0}, {1}", surname, forename);
                    var resultPupils = pupilRecordTriplet.SearchCriteria.Search();
                    var pupilSearchTile = resultPupils.SingleOrDefault(t => t.Name.Equals(String.Format("{0}, {1}", surname, forename)));
                    var pupilRecord = pupilSearchTile == null ? null : pupilSearchTile.Click<PupilRecordPage>();

                    // Add behaiour 
                    By contextualLink = By.CssSelector(SharedComponents.Helpers.SeleniumHelper.AutomationId("create-conductpoints-contextual-link"));
                    AutomationSugar.WaitForAjaxCompletion();
                    PageFactory.InitElements(WebContext.WebDriver, this);

                    SharedComponents.Helpers.SeleniumHelper.FindAndClick(contextualLink);
                    SharedComponents.Helpers.SeleniumHelper.FindAndClick(SharedComponents.Helpers.SeleniumHelper.AutomationId("conduct-achievement-contextlink"));

                    var achievementDialog = new QuickAddAchievementDialog();
                    // update vlaues
                    // Save values
                    // achievementDialog.Save();
                }
                finally
                {
                    // 
                }
            }
        }

        /// <summary>
        /// Author: Y.Ta
        /// Descriptions: Verify Add Contact into pupil record sucessfully
        /// </summary>
        /// 
        [WebDriverTest(Browsers = new[] { BrowserDefaults.Chrome },
            Groups = new[] { PupilTestGroups.PupilRecord.Update, PupilTestGroups.PupilRecord.Page, PupilTestGroups.Priority.Priority2, "DoNotDisclose" })]
        public void DoNotDisclose_Popup_OnSave()
        {
            // Arrange
            DateTime dob = new DateTime(2006, 01, 06);
            DateTime doa = new DateTime(2010, 01, 06);
            var learnerId = Guid.NewGuid();
            string random = SeleniumHelper.GenerateRandomNumberUsingDateTime();
            string randomCharacter = SeleniumHelper.GenerateRandomString(6) + "_DoNotDisclose";
            string forename = "aForeName" + randomCharacter + random;
            string surname = "aSurName" + randomCharacter + random;

            var pupil1 = this.BuildDataPackage()
                .AddBasicLearner(learnerId, surname, forename, dob, doa, addAddress: true);

            string forename1 = "aForeName1" + randomCharacter + random;
            string surname1 = "aSurName1" + randomCharacter + random;

            var learnerId1 = Guid.NewGuid();
            var pupil2 = this.BuildDataPackage()
                .AddBasicLearner(learnerId1, surname1, forename1, dob, doa, addAddress: true);

            //Act
            using (new DataSetup(false, true, pupil1, pupil2))
            {
                try
                {
                    // Login as school admin
                    SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.TestUser, enabledFeatures: "DoNotDisclose");

                    // Navigate to Pupil Record
                    AutomationSugar.NavigateMenu("Tasks", "Pupils", "Pupil Records");
                    var pupilRecordTriplet = new PupilSearchTriplet();

                    var pupilFullName = $"{surname}, {forename}";
                    pupilRecordTriplet.SearchCriteria.PupilName = pupilFullName;
                    var resultPupils = pupilRecordTriplet.SearchCriteria.Search();
                    var pupilSearchTile =
                        resultPupils.SingleOrDefault(
                            t => t.Name.Equals(pupilFullName));

                    var pupilRecord = pupilSearchTile?.Click<PupilRecordPage>();
                    Assert.AreNotEqual(null, pupilRecord, "Does not found pupil");

                    pupilRecord.SelectAddressesTab();

                    pupilRecord.DoNotDisclose = true;

                    pupilRecord.SavePupil();

                    // Verify data is saved Success
                    Assert.AreEqual(true, pupilRecord.IsSuccessMessageDisplayed(), "Success message is not display");

                    Assert.IsTrue(SeleniumHelper.DoesWebElementExist(SimsBy.CssSelector("[data-section-id='generic-confirm-dialog']")));

                    var doNotDiscloseDialog = new DoNotDiscloseConfirmDialog();
                    doNotDiscloseDialog.ClickSave();

                    pupilRecordTriplet.SearchCriteria.PupilName = $"{surname1}, {forename1}";
                    resultPupils = pupilRecordTriplet.SearchCriteria.Search();
                    pupilSearchTile =
                        resultPupils.SingleOrDefault(
                            t => t.Name.Equals($"{surname1}, {forename1}"));

                    pupilRecord = pupilSearchTile?.Click<PupilRecordPage>();
                    Assert.AreNotEqual(null, pupilRecord, "Does not found pupil");

                    pupilRecord.SelectAddressesTab();

                    Assert.IsTrue(pupilRecord.DoNotDisclose);
                }
                finally
                {
                    PurgeLinkedData.DeleteDoNotDiscloseFlagForLearner(learnerId);
                    PurgeLinkedData.DeleteDoNotDiscloseFlagForLearner(learnerId1);
                }
            }
        }
    }
}
