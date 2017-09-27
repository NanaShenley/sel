//using System;
//using NUnit.Framework;
//using OpenQA.Selenium;
//using POM.Components.Pupil;
//using POM.Helper;
//using Pupil.Components.Common;
//using Pupil.Data;
//using Selene.Support.Attributes;
//using SeSugar;
//using SeSugar.Automation;
//using SeSugar.Data;
//using TestSettings;

//namespace Pupil.PupilPremium.Tests
//{
//    public class ContextualActionsPermissionsTests
//    {
//        private const string PupilPremiumFeature = "Pupil Premium";

//        [WebDriverTest(Enabled = true, Browsers = new[] { BrowserDefaults.Chrome },
//            Groups = new[] { PupilTestGroups.PupilPremium.Page, PupilTestGroups.PupilPremium.Permissions, PupilTestGroups.Priority.Priority2, "PP" })]
//        [Variant(Variant.AllEnglish | Variant.AllWelsh | Variant.AllIndependant)]
//        public void CannotOpenPupilPremiumFromPupilRecordAsClassTeacher()
//        {
//			//Arrange
//			// Add Basic Learner
//			var pupilRecord = this.BuildDataPackage();
//			var learnerId = Guid.NewGuid();
//			var surname = "PupilPremium" + Utilities.GenerateRandomString(5);
//			var forename = Utilities.GenerateRandomString(15);
//			var dateOfBirth = new DateTime(2009, 05, 31);
//			var dateOfAdmission = new DateTime(2011, 10, 10);

//			pupilRecord.AddBasicLearner(learnerId, surname, forename, dateOfBirth, dateOfAdmission);

//			// Act
//            using (new DataSetup(false, true, pupilRecord))
//            {
//                try
//                {
//                    // Login as school admin
//                    SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.AdmissionsOfficer, false, PupilPremiumFeature);
//                    AutomationSugar.WaitForAjaxCompletion();

//                    AutomationSugar.NavigateMenu("Tasks", "Pupils", "Pupil Records");
//                    AutomationSugar.WaitForAjaxCompletion();

//                    // Search a pupil
//                    var pupilRecordTriplet = new PupilRecordTriplet();
//                    pupilRecordTriplet.SearchCriteria.PupilName = String.Format("{0}, {1}", surname, forename);
//                    pupilRecordTriplet.SearchCriteria.IsCurrent = true;
//                    pupilRecordTriplet.SearchCriteria.IsFuture = false;
//                    pupilRecordTriplet.SearchCriteria.IsLeaver = false;
//                    var pupilResults = pupilRecordTriplet.SearchCriteria.Search();
//                    AutomationSugar.WaitForAjaxCompletion();

//                    // Open pupil page
//                    pupilResults.FirstOrDefault(x => x.Name.Trim().Equals(surname + ", " + forename))
//                        .Click<PupilRecordPage>();
//                    AutomationSugar.WaitForAjaxCompletion();

//                    Assert.Throws(typeof(AssertionException), delegate
//                    {
//                        SeleniumHelper.GetVisible(By.CssSelector(SeleniumHelper.AutomationId("service_navigation_contextual_link_Pupil_Premium")));
//                    });
//                }
//                finally
//                {
//                    //cleanup code goes here
//                }
//            }
//        }

//        [WebDriverTest(Enabled = true, Browsers = new[] { BrowserDefaults.Chrome },
//            Groups = new[] { PupilTestGroups.ClassLog.Page, PupilTestGroups.ClassLog.ClassLogPermissions, PupilTestGroups.Priority.Priority2, "PP" })]
//        [Variant(Variant.AllEnglish | Variant.AllWelsh | Variant.AllIndependant)]
//        public void CannotOpenPupilPremiumFromPupilRecordAsSchoolAdministrator()
//        {
//            //Arrange
//			// Add Basic Learner
//			var pupilRecord = this.BuildDataPackage();
//			var learnerId = Guid.NewGuid();
//			var surname = "PupilPremium" + Utilities.GenerateRandomString(5);
//			var forename = Utilities.GenerateRandomString(15);
//			var dateOfBirth = new DateTime(2009, 05, 31);
//			var dateOfAdmission = new DateTime(2011, 10, 10);

//			pupilRecord.AddBasicLearner(learnerId, surname, forename, dateOfBirth, dateOfAdmission);

//			// Act
//            using (new DataSetup(false, true, pupilRecord))
//            {
//                try
//                {
//                    // Login as school admin
//                    SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator, false, PupilPremiumFeature);
//                    AutomationSugar.WaitForAjaxCompletion();

//                    AutomationSugar.NavigateMenu("Tasks", "Pupils", "Pupil Records");
//                    AutomationSugar.WaitForAjaxCompletion();

//                    // Search a pupil
//                    var pupilRecordTriplet = new PupilRecordTriplet();
//                    pupilRecordTriplet.SearchCriteria.PupilName = String.Format("{0}, {1}", surname, forename);
//                    pupilRecordTriplet.SearchCriteria.IsCurrent = true;
//                    pupilRecordTriplet.SearchCriteria.IsFuture = false;
//                    pupilRecordTriplet.SearchCriteria.IsLeaver = false;
//                    var pupilResults = pupilRecordTriplet.SearchCriteria.Search();
//                    AutomationSugar.WaitForAjaxCompletion();

//                    // Open pupil page
//                    pupilResults.FirstOrDefault(x => x.Name.Trim().Equals(surname + ", " + forename))
//                        .Click<PupilRecordPage>();
//                    AutomationSugar.WaitForAjaxCompletion();

//                    Assert.Throws(typeof(AssertionException), delegate
//                    {
//                        SeleniumHelper.GetVisible(By.CssSelector(SeleniumHelper.AutomationId("service_navigation_contextual_link_Pupil_Premium")));
//                    });
//                }
//                finally
//                {
//                    //cleanup code goes here
//                }
//            }
//        }

//        [WebDriverTest(Enabled = true, Browsers = new[] { BrowserDefaults.Chrome },
//            Groups = new[] { PupilTestGroups.PupilPremium.Page, PupilTestGroups.PupilPremium.Permissions, PupilTestGroups.Priority.Priority2, "PP" })]
//        [Variant(Variant.AllEnglish | Variant.AllWelsh | Variant.AllIndependant)]
//        public void CannotOpenPupilPremiumFromPupilRecordAsSenCoordinator()
//        {
//            //Arrange
//            // Add Basic Learner
//            var pupilRecord = this.BuildDataPackage();
//            var learnerId = Guid.NewGuid();
//            var surname = "PupilPremium" + Utilities.GenerateRandomString(5);
//            var forename = Utilities.GenerateRandomString(15);
//            var dateOfBirth = new DateTime(2009, 05, 31);
//            var dateOfAdmission = new DateTime(2011, 10, 10);

//            pupilRecord.AddBasicLearner(learnerId, surname, forename, dateOfBirth, dateOfAdmission);

//            // Act
//            using (new DataSetup(false, true, pupilRecord))
//            {
//                try
//                {
//                    // Login as school admin
//                    SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SENCoordinator, false, PupilPremiumFeature);
//                    AutomationSugar.WaitForAjaxCompletion();

//                    AutomationSugar.NavigateMenu("Tasks", "Pupils", "Pupil Records");
//                    AutomationSugar.WaitForAjaxCompletion();

//                    // Search a pupil
//                    var pupilRecordTriplet = new PupilRecordTriplet();
//                    pupilRecordTriplet.SearchCriteria.PupilName = String.Format("{0}, {1}", surname, forename);
//                    pupilRecordTriplet.SearchCriteria.IsCurrent = true;
//                    pupilRecordTriplet.SearchCriteria.IsFuture = false;
//                    pupilRecordTriplet.SearchCriteria.IsLeaver = false;
//                    var pupilResults = pupilRecordTriplet.SearchCriteria.Search();
//                    AutomationSugar.WaitForAjaxCompletion();

//                    // Open pupil page
//                    pupilResults.FirstOrDefault(x => x.Name.Trim().Equals(surname + ", " + forename))
//                        .Click<PupilRecordPage>();
//                    AutomationSugar.WaitForAjaxCompletion();

//                    Assert.Throws(typeof(AssertionException), delegate
//                    {
//                        SeleniumHelper.GetVisible(By.CssSelector(SeleniumHelper.AutomationId("service_navigation_contextual_link_Pupil_Premium")));
//                    });
//                }
//                finally
//                {
//                    //cleanup code goes here
//                }
//            }
//        }

//        [WebDriverTest(Enabled = true, Browsers = new[] { BrowserDefaults.Chrome },
//            Groups = new[] { PupilTestGroups.PupilPremium.Page, PupilTestGroups.PupilPremium.Permissions, PupilTestGroups.Priority.Priority2, "PP" })]
//        [Variant(Variant.AllEnglish | Variant.AllWelsh | Variant.AllIndependant)]
//        public void CannotOpenPupilPremiumFromPupilRecordAsAssessmentCoordinator()
//        {
//            //Arrange
//            // Add Basic Learner
//            var pupilRecord = this.BuildDataPackage();
//            var learnerId = Guid.NewGuid();
//            var surname = "PupilPremium" + Utilities.GenerateRandomString(5);
//            var forename = Utilities.GenerateRandomString(15);
//            var dateOfBirth = new DateTime(2009, 05, 31);
//            var dateOfAdmission = new DateTime(2011, 10, 10);

//            pupilRecord.AddBasicLearner(learnerId, surname, forename, dateOfBirth, dateOfAdmission);

//            // Act
//            using (new DataSetup(false, true, pupilRecord))
//            {
//                try
//                {
//                    // Login as school admin
//                    SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.AssessmentCoordinator, false, PupilPremiumFeature);
//                    AutomationSugar.WaitForAjaxCompletion();

//                    AutomationSugar.NavigateMenu("Tasks", "Pupils", "Pupil Records");
//                    AutomationSugar.WaitForAjaxCompletion();

//                    // Search a pupil
//                    var pupilRecordTriplet = new PupilRecordTriplet();
//                    pupilRecordTriplet.SearchCriteria.PupilName = String.Format("{0}, {1}", surname, forename);
//                    pupilRecordTriplet.SearchCriteria.IsCurrent = true;
//                    pupilRecordTriplet.SearchCriteria.IsFuture = false;
//                    pupilRecordTriplet.SearchCriteria.IsLeaver = false;
//                    var pupilResults = pupilRecordTriplet.SearchCriteria.Search();
//                    AutomationSugar.WaitForAjaxCompletion();

//                    // Open pupil page
//                    pupilResults.FirstOrDefault(x => x.Name.Trim().Equals(surname + ", " + forename))
//                        .Click<PupilRecordPage>();
//                    AutomationSugar.WaitForAjaxCompletion();

//                    Assert.Throws(typeof(AssertionException), delegate
//                    {
//                        SeleniumHelper.GetVisible(By.CssSelector(SeleniumHelper.AutomationId("service_navigation_contextual_link_Pupil_Premium")));
//                    });
//                }
//                finally
//                {
//                    //cleanup code goes here
//                }
//            }
//        }

//        [WebDriverTest(Enabled = true, Browsers = new[] { BrowserDefaults.Chrome },
//            Groups = new[] { PupilTestGroups.PupilPremium.Page, PupilTestGroups.PupilPremium.Permissions, PupilTestGroups.Priority.Priority2, "PP" })]
//        [Variant(Variant.AllEnglish | Variant.AllWelsh | Variant.AllIndependant)]
//        public void CanOpenPupilPremiumFromPupilRecordAsSeniorManagementTeam()
//        {
//            //Arrange
//            // Add Basic Learner
//            var pupilRecord = this.BuildDataPackage();
//            var learnerId = Guid.NewGuid();
//            var surname = "PupilPremium" + Utilities.GenerateRandomString(5);
//            var forename = Utilities.GenerateRandomString(15);
//            var dateOfBirth = new DateTime(2009, 05, 31);
//            var dateOfAdmission = new DateTime(2011, 10, 10);

//            pupilRecord.AddBasicLearner(learnerId, surname, forename, dateOfBirth, dateOfAdmission);

//            // Act
//            using (new DataSetup(false, true, pupilRecord))
//            {
//                try
//                {
//                    // Login as school admin
//                    SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SeniorManagementTeam, false, PupilPremiumFeature);
//                    AutomationSugar.WaitForAjaxCompletion();

//                    AutomationSugar.NavigateMenu("Tasks", "Pupils", "Pupil Records");
//                    AutomationSugar.WaitForAjaxCompletion();

//                    // Search a pupil
//                    var pupilRecordTriplet = new PupilRecordTriplet();
//                    pupilRecordTriplet.SearchCriteria.PupilName = String.Format("{0}, {1}", surname, forename);
//                    pupilRecordTriplet.SearchCriteria.IsCurrent = true;
//                    pupilRecordTriplet.SearchCriteria.IsFuture = false;
//                    pupilRecordTriplet.SearchCriteria.IsLeaver = false;
//                    var pupilResults = pupilRecordTriplet.SearchCriteria.Search();
//                    AutomationSugar.WaitForAjaxCompletion();

//                    // Open pupil page
//                    pupilResults.FirstOrDefault(x => x.Name.Trim().Equals(surname + ", " + forename))
//                        .Click<PupilRecordPage>();
//                    AutomationSugar.WaitForAjaxCompletion();

//                    Assert.IsNotNull(
//                        SeleniumHelper.GetVisible(
//                            By.CssSelector(SeleniumHelper.AutomationId("service_navigation_contextual_link_Pupil_Premium"))));
//                }
//                finally
//                {
//                    //cleanup code goes here
//                }
//            }
//        }

//        [WebDriverTest(Enabled = true, Browsers = new[] { BrowserDefaults.Chrome },
//            Groups = new[] { PupilTestGroups.PupilPremium.Page, PupilTestGroups.PupilPremium.Permissions, PupilTestGroups.Priority.Priority2, "PP" })]
//        [Variant(Variant.AllEnglish | Variant.AllWelsh | Variant.AllIndependant)]
//        public void CanOpenPupilPremiumFromPupilRecordAsReturnsManager()
//        {
//            //Arrange
//            // Add Basic Learner
//            var pupilRecord = this.BuildDataPackage();
//            var learnerId = Guid.NewGuid();
//            var surname = "PupilPremium" + Utilities.GenerateRandomString(5);
//            var forename = Utilities.GenerateRandomString(15);
//            var dateOfBirth = new DateTime(2009, 05, 31);
//            var dateOfAdmission = new DateTime(2011, 10, 10);

//            pupilRecord.AddBasicLearner(learnerId, surname, forename, dateOfBirth, dateOfAdmission);

//            // Act
//            using (new DataSetup(false, true, pupilRecord))
//            {
//                try
//                {
//                    // Login as school admin
//                    SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.ReturnsManager, false, PupilPremiumFeature);
//                    AutomationSugar.WaitForAjaxCompletion();

//                    AutomationSugar.NavigateMenu("Tasks", "Pupils", "Pupil Records");
//                    AutomationSugar.WaitForAjaxCompletion();

//                    // Search a pupil
//                    var pupilRecordTriplet = new PupilRecordTriplet();
//                    pupilRecordTriplet.SearchCriteria.PupilName = String.Format("{0}, {1}", surname, forename);
//                    pupilRecordTriplet.SearchCriteria.IsCurrent = true;
//                    pupilRecordTriplet.SearchCriteria.IsFuture = false;
//                    pupilRecordTriplet.SearchCriteria.IsLeaver = false;
//                    var pupilResults = pupilRecordTriplet.SearchCriteria.Search();
//                    AutomationSugar.WaitForAjaxCompletion();

//                    // Open pupil page
//                    pupilResults.FirstOrDefault(x => x.Name.Trim().Equals(surname + ", " + forename))
//                        .Click<PupilRecordPage>();
//                    AutomationSugar.WaitForAjaxCompletion();

//                    Assert.IsNotNull(
//                        SeleniumHelper.GetVisible(
//                            By.CssSelector(SeleniumHelper.AutomationId("service_navigation_contextual_link_Pupil_Premium"))));
//                }
//                finally
//                {
//                    //cleanup code goes here
//                }
//            }
//        }
//    }
//}
