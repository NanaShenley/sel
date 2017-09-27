using System;
using System.Threading;
using NUnit.Framework;
using OpenQA.Selenium;
using POM.Components.Conduct.Pages.Exclusions;
using POM.Components.Pupil;
using POM.Helper;
using Pupil.Components.Common;
using Pupil.Data;
using Pupil.Data.Entities;
using Selene.Support.Attributes;
using SeSugar;
using SeSugar.Automation;
using SeSugar.Data;
using TestSettings;
using SimsBy = SeSugar.Automation.SimsBy;
using WebDriverRunner.webdriver;
using POM.Base;
//using static POM.Components.Conduct.Pages.Exclusions.ExclusionDetail;
// ReSharper disable SuggestVarOrType_BuiltInTypes
// ReSharper disable ExceptionNotDocumentedOptional
// ReSharper disable UseStringInterpolation

namespace Pupil.Conduct.Tests
{
    /// <summary>
    /// Implements Exclusions Tests
    /// </summary>
    public class ExclusionsTests
    {
        /// <summary>
        /// The login as
        /// </summary>
        private const SeleniumHelper.iSIMSUserType LoginAs = SeleniumHelper.iSIMSUserType.SchoolAdministrator;

        /// <summary>
        /// Check 'Senior Leadership Team' permissions.
        /// </summary>
        [WebDriverTest(TimeoutSeconds = 2000, Enabled = true, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { PupilTestGroups.Suspensions.Page, PupilTestGroups.Priority.Priority2, "Exclusion_1" })]
        public void Exclusions_SeniorLeadershipPermissions()
        {
            //Arrange
            var learnerId = Guid.NewGuid();
            var dataPackage = this.BuildDataPackage();
            const string forename = "SeniorLeadershipPermissions";
            var surname = Utilities.GenerateRandomString(10, "SeniorLeadershipPermissions");
            var pupilName = string.Format("{0}, {1}", surname, forename);

            //Add learner with suspension
            dataPackage.AddBasicLearner(learnerId, surname, forename, dateOfBirth: new DateTime(2008, 05, 30), dateOfAdmission: new DateTime(2015, 10, 03))
                         .AddBasicSuspension(learnerId, new DateTime(2016, 02, 01), new DateTime(2016, 02, 05), 5);
            //Act
            using (new DataSetup(purgeBeforeInsert: false, purgeAfterTest: true, packages: dataPackage))
            {
                try
                {
                    //Arrange
                    // Login

                    String[] featureList = { "Conduct Exclusions" };
                    FeatureBee.FeatureBeeLogin.LoginWithFeatureBee(featureList, FeatureBee.FeatureBeeLogin.iSIMSUserType.SeniorManagementTeam);

                    //SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SeniorManagementTeam); //TODO: Once feeture bee is released

                    // Navigate to suspensions
                    AutomationSugar.NavigateMenu("Tasks", "Pupil Conduct", "Pupil Suspensions"); //TODO: Exclusions

                    //Act
                    var exclusionTriplet = new ExclusionTriplet();
                    exclusionTriplet.SearchCriteria.PupilName = pupilName;
                    var exclusionRecordPage =
                        exclusionTriplet.SearchCriteria.Search()
                            .SingleOrDefault(x => x.Name.Equals(pupilName))
                            .Click<ExclusionRecordPage>();

                    //Assert
                    Assert.IsTrue(exclusionRecordPage.HeaderTitle.Text.Contains(pupilName), "Header title mismatch");
                }
                finally
                {
                    // Teardown
                    PurgeLinkedData.DeleteAttendanceForLearner(learnerId);
                    PurgeLinkedData.DeleteSuspensionForLearner(learnerId);
                }
            }
        }

        /// <summary>
        /// Check 'SEN Coordinator' Pemissions (view)
        /// </summary>
        [WebDriverTest(TimeoutSeconds = 2000, Enabled = true, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { PupilTestGroups.Suspensions.Page, PupilTestGroups.Priority.Priority2, "Exclusion_2" })]
        public void Exclusions_SENCoordPermissions()
        {
            //Arrange
            var learnerId = Guid.NewGuid();
            var dataPackage = this.BuildDataPackage();
            var forename = "SENCoordPermissions";
            var surname = Utilities.GenerateRandomString(10, "SENCoordPermissions");
            var pupilName = string.Format("{0}, {1}", surname, forename);

            //Add learner with suspension
            dataPackage.AddBasicLearner(learnerId, surname, forename, dateOfBirth: new DateTime(2008, 05, 30), dateOfAdmission: new DateTime(2015, 10, 03))
                         .AddBasicSuspension(learnerId, new DateTime(2016, 02, 01), new DateTime(2016, 02, 05), 5);
            //Act
            using (new DataSetup(purgeBeforeInsert: false, purgeAfterTest: true, packages: dataPackage))
            {
                try
                {                    
                    // Login
                    String[] featureList = { "Conduct Exclusions" };
                    FeatureBee.FeatureBeeLogin.LoginWithFeatureBee(featureList, FeatureBee.FeatureBeeLogin.iSIMSUserType.TestUser); //TODO: once permissions are done

                    //SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SeniorManagementTeam); //TODO: Once feeture bee is released

                    // Navigate to suspensions
                    AutomationSugar.NavigateMenu("Tasks", "Pupil Conduct", "Pupil Suspensions"); //TODO: Exclusions

                    // Search suspension record again and verify record exists and diplays correct values
                    var exclusionTriplet = new ExclusionTriplet();
                    exclusionTriplet.SearchCriteria.PupilName = pupilName;
                    var exclusionRecordPage =
                        exclusionTriplet.SearchCriteria.Search()
                            .SingleOrDefault(x => x.Name.Equals(pupilName))
                            .Click<ExclusionRecordPage>();

                    //Assert
                    //Assert
                    Assert.IsTrue(exclusionRecordPage.HeaderTitle.Text.Contains(pupilName), "Header title mismatch");
                }
                finally
                {
                    // Teardown
                    PurgeLinkedData.DeleteAttendanceForLearner(learnerId);
                    PurgeLinkedData.DeleteSuspensionForLearner(learnerId);
                }
            }
        }

        /// <summary>
        /// Description: delete a pupil who has suspension record
        /// </summary>
        [WebDriverTest(TimeoutSeconds = 2000, Enabled = true, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { PupilTestGroups.Suspensions.Page, PupilTestGroups.Priority.Priority2, "Exclusion_3" })]
        public void Exclusions_DeletePupil()
        {
            //Arrange
            var learnerId = Guid.NewGuid();
            var dataPackage = this.BuildDataPackage();
            var forename = "DeletePupil";
            var surname = Utilities.GenerateRandomString(10, "DeletePupil");
            var pupilName = string.Format("{0}, {1}", surname, forename);

            //Add learner with suspension
            dataPackage.AddBasicLearner(learnerId, surname, forename, dateOfBirth: new DateTime(2008, 05, 30), dateOfAdmission: new DateTime(2015, 10, 03))
                         .AddBasicSuspension(learnerId, new DateTime(2016, 02, 01), new DateTime(2016, 02, 05), 5);
            //Act
            using (new DataSetup(purgeBeforeInsert: false, purgeAfterTest: true, packages: dataPackage))
            {
                // Login
                SeleniumHelper.Login(LoginAs);

                // Navigate to delete pupil
                AutomationSugar.NavigateMenu("Tasks", "Pupils", "Delete Pupil");
                var deletePupilTriplet = new DeletePupilRecordTriplet();
                deletePupilTriplet.SearchCriteria.PupilName = pupilName;
                var deletePupilRecordPage = deletePupilTriplet.SearchCriteria.Search().SingleOrDefault(x => x.Name.Equals(pupilName)).Click<DeletePupilRecordPage>();
                deletePupilRecordPage.Delete();

                // Verify message displays.
                var _successMessage = SeleniumHelper.FindElement(SimsBy.AutomationId("status_success"));
                Assert.IsTrue(_successMessage.IsExist(), "Message success does not display");
            }
        }



        /// <summary>
        /// Check search
        /// </summary>
        [WebDriverTest(TimeoutSeconds = 2000, Enabled = true, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { PupilTestGroups.Suspensions.SuspensionsSearch, PupilTestGroups.Priority.Priority2, "Exclusion_4" })]
        public void Exclusions_ExclusionSearch()
        {
            //Arrange
            var learnerId = Guid.NewGuid();
            var dataPackage = this.BuildDataPackage();
            const string forename = "Exclusions_4";
            var surname = Utilities.GenerateRandomString(10, "Exclusions_4");
            var pupilName = string.Format("{0}, {1}", surname, forename);

            ExclusionType e = (Queries.GetFirstExclusionType());

            //Add learner with suspension
            dataPackage.AddBasicLearner(learnerId, surname, forename, dateOfBirth: new DateTime(2008, 05, 30), dateOfAdmission: new DateTime(2015, 10, 03))
                         .AddSuspensionForType(learnerId, e.ID, new DateTime(2016, 01, 01), new DateTime(2016, 02, 02), 5);
            //Act
            using (new DataSetup(purgeBeforeInsert: false, purgeAfterTest: true, packages: dataPackage))
            {
                try
                {
                    //Arrange
                    // Login

                    String[] featureList = { "Conduct Exclusions" };
                    FeatureBee.FeatureBeeLogin.LoginWithFeatureBee(featureList, FeatureBee.FeatureBeeLogin.iSIMSUserType.TestUser); //TODO: once permissions are done

                    //SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SeniorManagementTeam); //TODO: Once feeture bee is released

                    // Navigate to suspensions
                    AutomationSugar.NavigateMenu("Tasks", "Pupil Conduct", "Pupil Suspensions"); //TODO: Exclusions

                    //Act
                    var exclusionTriplet = new ExclusionTriplet();
                    exclusionTriplet.SearchCriteria.PupilName = pupilName;
                    var exclusionRecordPage = exclusionTriplet.SearchCriteria.Search()
                                                .SingleOrDefault(x => x.Name.Equals(pupilName))
                                                .Click<ExclusionRecordPage>();

                    var count = exclusionRecordPage.SearchResults.Count();
                    ExclusionRecordPage.ExclusionSearchResultTile tile = exclusionRecordPage.SearchResults.FirstOrDefault();

                    //Assert
                    Assert.IsTrue(count == 1);
                    Assert.IsTrue(tile.ExclusionType == e.Description);
                    tile.Click();
                }
                finally
                {
                    // Teardown
                    PurgeLinkedData.DeleteAttendanceForLearner(learnerId);
                    PurgeLinkedData.DeleteSuspensionForLearner(learnerId);
                }
            }
        }


        /// <summary>
        /// Check search
        /// </summary>
        [WebDriverTest(TimeoutSeconds = 2000, Enabled = true, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { PupilTestGroups.Suspensions.SuspensionsSearch, PupilTestGroups.Priority.Priority2, "Exclusion_5" })]
        public void Exclusions_ExclusionDetailSearch()
        {
            //Arrange
            var learnerId = Guid.NewGuid();
            var dataPackage = this.BuildDataPackage();
            const string forename = "Exclusions_5";
            var surname = Utilities.GenerateRandomString(10, "Exclusions_5");
            var pupilName = string.Format("{0}, {1}", surname, forename);

            ExclusionType e = (Queries.GetFirstExclusionType());

            //Add learner with suspension
            dataPackage.AddBasicLearner(learnerId, surname, forename, dateOfBirth: new DateTime(2008, 05, 30), dateOfAdmission: new DateTime(2015, 10, 03))
                         .AddSuspensionForType(learnerId, e.ID, new DateTime(2016, 01, 01), new DateTime(2016, 02, 02), 5);
            //Act
            using (new DataSetup(purgeBeforeInsert: false, purgeAfterTest: true, packages: dataPackage))
            {
                try
                {
                    //Arrange
                    // Login

                    String[] featureList = { "Conduct Exclusions" };
                    FeatureBee.FeatureBeeLogin.LoginWithFeatureBee(featureList, FeatureBee.FeatureBeeLogin.iSIMSUserType.TestUser); //TODO: once permissions are done

                    //SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SeniorManagementTeam); //TODO: Once feeture bee is released

                    // Navigate to suspensions
                    AutomationSugar.NavigateMenu("Tasks", "Pupil Conduct", "Pupil Suspensions"); //TODO: Exclusions

                    //Act

                    
                    var exclusionTriplet = new ExclusionTriplet(); //Get the list of exclusions 
                    exclusionTriplet.SearchCriteria.PupilName = pupilName;
                    var exclusionRecordPage = exclusionTriplet.SearchCriteria.Search()
                                                .SingleOrDefault(x => x.Name.Equals(pupilName))
                                                .Click<ExclusionRecordPage>();

                    Thread.Sleep(TimeSpan.FromSeconds(10));

                    var count = exclusionRecordPage.SearchResults.Count();
                    ExclusionRecordPage.ExclusionSearchResultTile tile = exclusionRecordPage.SearchResults.FirstOrDefault();

                    //click the first exclusion in the list to load detail
                    //Click event shall load "ExclusionDetail" 
                    var exclusionDetail = tile.Click<ExclusionDetail>(); 

                    //var exclusionDetail = new ExclusionDetail();
                    var exclusionDays = exclusionDetail.Length;

                    //Assert
                    Assert.IsTrue(exclusionDetail.Length == (5).ToString());
                    

                }
                finally
                {
                    // Teardown
                    PurgeLinkedData.DeleteAttendanceForLearner(learnerId);
                    PurgeLinkedData.DeleteSuspensionForLearner(learnerId);
                }
            }
        }



        /// <summary>
        /// Check search
        /// </summary>
        [WebDriverTest(TimeoutSeconds = 2000, Enabled = true, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { PupilTestGroups.Suspensions.SuspensionsSearch, PupilTestGroups.Priority.Priority2, "Exclusion_6" })]
        public void Exclusions_ExclusionDetailSave()
        {
            //Arrange
            var learnerId = Guid.NewGuid();
            var dataPackage = this.BuildDataPackage();
            const string forename = "Exclusions_6";
            var surname = Utilities.GenerateRandomString(10, "Exclusions_6");
            var pupilName = string.Format("{0}, {1}", surname, forename);

            ExclusionType e = (Queries.GetFirstExclusionType());

            //Add learner with suspension
            dataPackage.AddBasicLearner(learnerId, surname, forename, dateOfBirth: new DateTime(2008, 05, 30), dateOfAdmission: new DateTime(2015, 10, 03))
                         .AddSuspensionForType(learnerId, e.ID, new DateTime(2016, 01, 01), new DateTime(2016, 02, 02), 5);
            //Act
            using (new DataSetup(purgeBeforeInsert: false, purgeAfterTest: true, packages: dataPackage))
            {
                try
                {
                    //Arrange
                    // Login

                    String[] featureList = { "Conduct Exclusions" };
                    FeatureBee.FeatureBeeLogin.LoginWithFeatureBee(featureList, FeatureBee.FeatureBeeLogin.iSIMSUserType.TestUser); //TODO: once permissions are done

                    //SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SeniorManagementTeam); //TODO: Once feeture bee is released

                    // Navigate to suspensions
                    AutomationSugar.NavigateMenu("Tasks", "Pupil Conduct", "Pupil Suspensions"); //TODO: Exclusions

                    //Act


                    var exclusionTriplet = new ExclusionTriplet(); //Get the list of exclusions 
                    exclusionTriplet.SearchCriteria.PupilName = pupilName;
                    var exclusionRecordPage = exclusionTriplet.SearchCriteria.Search()
                                                .SingleOrDefault(x => x.Name.Equals(pupilName))
                                                .Click<ExclusionRecordPage>();

                    Thread.Sleep(TimeSpan.FromSeconds(5));

                    var count = exclusionRecordPage.SearchResults.Count();
                    ExclusionRecordPage.ExclusionSearchResultTile tile = exclusionRecordPage.SearchResults.FirstOrDefault();

                    tile.Click<ExclusionDetail>();
                    
                    //click Save 
                    WebContext.WebDriver.FindElement(By.CssSelector("[data-automation-id='save_button']")).Click();
                    Thread.Sleep(TimeSpan.FromSeconds(5));

                    //data-automation-id="status_success"
                    var _alertMessage = WebContext.WebDriver.FindElement(By.CssSelector("[data-automation-id='status_success']")).IsExist();
                    
                    //Assert
                    Assert.IsTrue(_alertMessage == true);


                }
                finally
                {
                    // Teardown
                    PurgeLinkedData.DeleteAttendanceForLearner(learnerId);
                    PurgeLinkedData.DeleteSuspensionForLearner(learnerId);
                }
            }
        }



        /// <summary>
        /// Check search
        /// </summary>
        [WebDriverTest(TimeoutSeconds = 2000, Enabled = true, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { PupilTestGroups.Suspensions.SuspensionsSearch, PupilTestGroups.Priority.Priority2, "Exclusion_7" })]
        public void Exclusions_ExclusionDetailEditAndSave()
        {
            //Arrange
            var learnerId = Guid.NewGuid();
            var dataPackage = this.BuildDataPackage();
            const string forename = "Exclusions_7";
            var surname = Utilities.GenerateRandomString(10, "Exclusions_7");
            var pupilName = string.Format("{0}, {1}", surname, forename);

            ExclusionType e = (Queries.GetFirstExclusionType());

            //Add learner with suspension
            dataPackage.AddBasicLearner(learnerId, surname, forename, dateOfBirth: new DateTime(2008, 05, 30), dateOfAdmission: new DateTime(2015, 10, 03))
                         .AddSuspensionForType(learnerId, e.ID, new DateTime(2016, 01, 01), new DateTime(2016, 02, 02), 5);
            //Act
            using (new DataSetup(purgeBeforeInsert: false, purgeAfterTest: true, packages: dataPackage))
            {
                try
                {
                    //Arrange
                    // Login

                    String[] featureList = { "Conduct Exclusions" };
                    FeatureBee.FeatureBeeLogin.LoginWithFeatureBee(featureList, FeatureBee.FeatureBeeLogin.iSIMSUserType.TestUser); //TODO: once permissions are done

                    //SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SeniorManagementTeam); //TODO: Once feeture bee is released

                    // Navigate to suspensions
                    AutomationSugar.NavigateMenu("Tasks", "Pupil Conduct", "Pupil Suspensions"); //TODO: Exclusions

                    //Act


                    var exclusionTriplet = new ExclusionTriplet(); //Get the list of exclusions 
                    exclusionTriplet.SearchCriteria.PupilName = pupilName;
                    var exclusionRecordPage = exclusionTriplet.SearchCriteria.Search()
                                                .SingleOrDefault(x => x.Name.Equals(pupilName))
                                                .Click<ExclusionRecordPage>();
                    Thread.Sleep(TimeSpan.FromSeconds(10));

                    var count = exclusionRecordPage.SearchResults.Count();
                    ExclusionRecordPage.ExclusionSearchResultTile tile = exclusionRecordPage.SearchResults.FirstOrDefault();

                    var _detail = tile.Click<ExclusionDetail>(); //click the first exclusion in the list to load detail also map it to our model
                    _detail.Length = "11";
                    
                    //click Save 
                    WebContext.WebDriver.FindElement(By.CssSelector("[data-automation-id='save_button']")).Click();
                    Thread.Sleep(TimeSpan.FromSeconds(10));

                    //data-automation-id="status_success"
                    var _alertMessage = WebContext.WebDriver.FindElement(By.CssSelector("[data-automation-id='status_success']")).IsExist();

                    //Assert
                    Assert.IsTrue(_alertMessage == true);


                }
                finally
                {
                    // Teardown
                    PurgeLinkedData.DeleteAttendanceForLearner(learnerId);
                    PurgeLinkedData.DeleteSuspensionForLearner(learnerId);
                }
            }
        }




        /// <summary>
        /// Check search
        /// </summary>
        [WebDriverTest(TimeoutSeconds = 2000, Enabled = true, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { PupilTestGroups.Suspensions.SuspensionsSearch, PupilTestGroups.Priority.Priority2, "Exclusion_8" })]
        public void Exclusions_ExclusionCreateNewAndSave()
        {
            //Arrange
            var learnerId = Guid.NewGuid();
            var dataPackage = this.BuildDataPackage();
            const string forename = "Exclusions_8";
            var surname = Utilities.GenerateRandomString(10, "Exclusions_8");
            var pupilName = string.Format("{0}, {1}", surname, forename);

            ExclusionType e = (Queries.GetFirstExclusionType());

            //Add learner
            dataPackage.AddBasicLearner(learnerId, surname, forename, dateOfBirth: new DateTime(2008, 05, 30), dateOfAdmission: new DateTime(2015, 10, 03));

            //Act
            using (new DataSetup(purgeBeforeInsert: false, purgeAfterTest: true, packages: dataPackage))
            {
                try
                {
                    //Arrange
                    // Login

                    String[] featureList = { "Conduct Exclusions" };
                    FeatureBee.FeatureBeeLogin.LoginWithFeatureBee(featureList, FeatureBee.FeatureBeeLogin.iSIMSUserType.TestUser); //TODO: once permissions are done

                    //SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SeniorManagementTeam); //TODO: Once feeture bee is released

                    // Navigate to suspensions
                    AutomationSugar.NavigateMenu("Tasks", "Pupil Conduct", "Pupil Suspensions"); //TODO: Exclusions

                    //Act


                    var exclusionTriplet = new ExclusionTriplet(); //Get the list of exclusions 
                    exclusionTriplet.SearchCriteria.PupilName = pupilName;
                    var exclusionRecordPage = exclusionTriplet.SearchCriteria.Search()
                                                .SingleOrDefault(x => x.Name.Equals(pupilName))
                                                .Click<ExclusionRecordPage>();

                    Thread.Sleep(TimeSpan.FromSeconds(5));
                    //var count = exclusionRecordPage.SearchResults.Count();
                    //ExclusionRecordPage.ExclusionSearchResultTile tile = exclusionRecordPage.SearchResults.FirstOrDefault();

                    exclusionRecordPage.ClickAdd();
                    Thread.Sleep(TimeSpan.FromSeconds(10));
                    string _pattern = "d/M/yyyy";
                    var exclusionType = Queries.GetFirstExclusionType();
                    var exclusionReason = Queries.GetFirstExclusionReason();

                    //populate the basic details on new exclusions screen
                    var _detail = new ExclusionDetail();
                    _detail.Type = exclusionType.Description;
                    Thread.Sleep(TimeSpan.FromSeconds(5));
                    _detail.Reason = exclusionReason.Description;
                    Thread.Sleep(TimeSpan.FromSeconds(5));
                    _detail.StartDate = new DateTime(DateTime.Today.Year, 2, 2).ToString(_pattern);
                    _detail.EndDate = new DateTime(DateTime.Today.Year, 3, 3).ToString(_pattern);
                    _detail.StartTime = "09:00";
                    _detail.EndTime = "16:00";
                    _detail.Length = "25";
                    _detail.Note = Utilities.GenerateRandomString(10, "AddSuspension");
                    _detail.SessionsMissed = "5";


                    //click Save 
                    WebContext.WebDriver.FindElement(By.CssSelector("[data-automation-id='save_button']")).Click();
                    Thread.Sleep(TimeSpan.FromSeconds(10));

                    //data-automation-id="status_success"
                    var _alertMessage = WebContext.WebDriver.FindElement(By.CssSelector("[data-automation-id='status_success']")).IsExist();

                    //Assert
                    Assert.IsTrue(_alertMessage == true);


                }
                finally
                {
                    // Teardown
                    PurgeLinkedData.DeleteAttendanceForLearner(learnerId);
                    PurgeLinkedData.DeleteSuspensionForLearner(learnerId);
                }
            }
        }



        /// <summary>
        /// Check search
        /// </summary>
        //[WebDriverTest(TimeoutSeconds = 2000, Enabled = true, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { PupilTestGroups.Suspensions.SuspensionsSearch, PupilTestGroups.Priority.Priority2, "Exclusion_9" })]
        //public void Exclusions_ExclusionAddStatusAndSave()
        //{
        //    //Arrange
        //    var learnerId = Guid.NewGuid();
        //    var dataPackage = this.BuildDataPackage();
        //    const string forename = "Exclusions_5";
        //    var surname = Utilities.GenerateRandomString(10, "Exclusions_5");
        //    var pupilName = string.Format("{0}, {1}", surname, forename);

        //    ExclusionType e = (Queries.GetFirstExclusionType());

        //    //Add learner
        //    dataPackage.AddBasicLearner(learnerId, surname, forename, dateOfBirth: new DateTime(2008, 05, 30), dateOfAdmission: new DateTime(2015, 10, 03));

        //    //Act
        //    using (new DataSetup(purgeBeforeInsert: false, purgeAfterTest: true, packages: dataPackage))
        //    {
        //        try
        //        {
        //            //Arrange
        //            // Login

        //            String[] featureList = { "Conduct Exclusions" };
        //            FeatureBee.FeatureBeeLogin.LoginWithFeatureBee(featureList, FeatureBee.FeatureBeeLogin.iSIMSUserType.TestUser); //TODO: once permissions are done

        //            //SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SeniorManagementTeam); //TODO: Once feeture bee is released

        //            // Navigate to suspensions
        //            AutomationSugar.NavigateMenu("Tasks", "Pupil Conduct", "Pupil Exclusions"); //TODO: Exclusions

        //            //Act


        //            var exclusionTriplet = new ExclusionTriplet(); //Get the list of exclusions 
        //            exclusionTriplet.SearchCriteria.PupilName = pupilName;
        //            var exclusionRecordPage = exclusionTriplet.SearchCriteria.Search()
        //                                        .SingleOrDefault(x => x.Name.Equals(pupilName))
        //                                        .Click<ExclusionRecordPage>();

        //            Thread.Sleep(TimeSpan.FromSeconds(5));
        //            //var count = exclusionRecordPage.SearchResults.Count();
        //            //ExclusionRecordPage.ExclusionSearchResultTile tile = exclusionRecordPage.SearchResults.FirstOrDefault();

        //            WebContext.WebDriver.FindElement(By.CssSelector("[data-automation-id='create_button']")).Click();
        //            Thread.Sleep(TimeSpan.FromSeconds(10));
        //            string _pattern = "d/M/yyyy";
        //            var exclusionType = Queries.GetFirstExclusionType();
        //            var exclusionReason = Queries.GetFirstExclusionReason();

        //            //populate the basic details on new exclusions screen
        //            var _detail = new ExclusionDetail();
        //            _detail.Type = exclusionType.Description;
        //            Thread.Sleep(TimeSpan.FromSeconds(5));
        //            _detail.Reason = exclusionReason.Description;
        //            Thread.Sleep(TimeSpan.FromSeconds(5));
        //            _detail.StartDate = new DateTime(DateTime.Today.Year, 2, 2).ToString(_pattern);
        //            _detail.EndDate = new DateTime(DateTime.Today.Year, 3, 3).ToString(_pattern);
        //            _detail.StartTime = "09:00";
        //            _detail.EndTime = "16:00";
        //            _detail.Length = "25";
        //            _detail.Note = Utilities.GenerateRandomString(10, "AddSuspension");
        //            _detail.SessionsMissed = "5";

        //            //add status 
        //            WebContext.WebDriver.FindElement(By.CssSelector("[data-automation-id='add_exclusion_status_button']")).Click();
        //            Thread.Sleep(TimeSpan.FromSeconds(5));
                    
        //            _detail.StatusGrid[0].Status = "Exclusion Stands";
        //            Thread.Sleep(TimeSpan.FromSeconds(10));
        //            _detail.StatusGrid[0].Date = new DateTime(DateTime.Today.Year, 3, 3).ToString(_pattern);
        //            _detail.StatusGrid[0].Reason = "Parent Request";
        //            Thread.Sleep(TimeSpan.FromSeconds(10));


        //            //click Save 
        //            WebContext.WebDriver.FindElement(By.CssSelector("[data-automation-id='save_button']")).Click();
        //            Thread.Sleep(TimeSpan.FromSeconds(10));

        //            //data-automation-id="status_success"
        //            var _alertMessage = WebContext.WebDriver.FindElement(By.CssSelector("[data-automation-id='status_success']")).IsExist();

        //            //Assert
        //            Assert.IsTrue(_alertMessage == true);


        //        }
        //        finally
        //        {
        //            // Teardown
        //            PurgeLinkedData.DeleteAttendanceForLearner(learnerId);
        //            PurgeLinkedData.DeleteSuspensionForLearner(learnerId);
        //        }
        //    }
        //}




        /// <summary>
        /// Check search
        /// </summary>
        [WebDriverTest(TimeoutSeconds = 2000, Enabled = true, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { PupilTestGroups.Suspensions.SuspensionsSearch, PupilTestGroups.Priority.Priority2, "Exclusion_10" })]
        public void Exclusions_ExclusionDelete()
        {
            //Arrange
            var learnerId = Guid.NewGuid();
            var dataPackage = this.BuildDataPackage();
            const string forename = "Exclusions_10";
            var surname = Utilities.GenerateRandomString(10, "Exclusions_10");
            var pupilName = string.Format("{0}, {1}", surname, forename);

            ExclusionType e = (Queries.GetFirstExclusionType());

            //Add learner with suspension
            dataPackage.AddBasicLearner(learnerId, surname, forename, dateOfBirth: new DateTime(2008, 05, 30), dateOfAdmission: new DateTime(2015, 10, 03))
                         .AddSuspensionForType(learnerId, e.ID, new DateTime(2016, 01, 01), new DateTime(2016, 02, 02), 5);
            //Act
            using (new DataSetup(purgeBeforeInsert: false, purgeAfterTest: true, packages: dataPackage))
            {
                try
                {
                    //Arrange
                    // Login

                    String[] featureList = { "Conduct Exclusions" };
                    FeatureBee.FeatureBeeLogin.LoginWithFeatureBee(featureList, FeatureBee.FeatureBeeLogin.iSIMSUserType.TestUser); //TODO: once permissions are done

                    //SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SeniorManagementTeam); //TODO: Once feeture bee is released

                    // Navigate to suspensions
                    AutomationSugar.NavigateMenu("Tasks", "Pupil Conduct", "Pupil Suspensions"); //TODO: Exclusions

                    //Act


                    var exclusionTriplet = new ExclusionTriplet(); //Get the list of exclusions 
                    exclusionTriplet.SearchCriteria.PupilName = pupilName;
                    var exclusionRecordPage = exclusionTriplet.SearchCriteria.Search()
                                                .SingleOrDefault(x => x.Name.Equals(pupilName))
                                                .Click<ExclusionRecordPage>();

                    Thread.Sleep(TimeSpan.FromSeconds(10));

                    //count number of exclusions, it should be 1
                    var count = exclusionRecordPage.SearchResults.Count();

                    ExclusionRecordPage.ExclusionSearchResultTile tile = exclusionRecordPage.SearchResults.FirstOrDefault();

                    //click the first exclusion in the list to load detail
                    //Click event shall load "ExclusionDetail" 
                    var exclusionDetail = tile.Click<ExclusionDetail>();

                    //Delete the Exclusion
                    WebContext.WebDriver.FindElement(By.CssSelector("[data-automation-id='delete_button']")).Click();
                    Thread.Sleep(TimeSpan.FromSeconds(5));
                    WebContext.WebDriver.FindElement(By.CssSelector("[data-automation-id='continue_with_delete_button']")).Click();
                    Thread.Sleep(TimeSpan.FromSeconds(5));

                    //count again, it should be zero
                    exclusionRecordPage = exclusionTriplet.SearchCriteria.Search().SingleOrDefault(x => x.Name.Equals(pupilName)).Click<ExclusionRecordPage>();
                    Thread.Sleep(TimeSpan.FromSeconds(5));

                    count = exclusionRecordPage.SearchResults.Count();

                    //Assert
                    Assert.IsTrue(count == 0);


                }
                finally
                {
                    // Teardown
                    PurgeLinkedData.DeleteAttendanceForLearner(learnerId);
                    PurgeLinkedData.DeleteSuspensionForLearner(learnerId);
                }
            }
        }




    }
}
