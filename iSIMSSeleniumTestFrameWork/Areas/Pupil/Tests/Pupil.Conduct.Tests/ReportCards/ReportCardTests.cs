using System;
using System.Threading;
using NUnit.Framework;
using POM.Components.Conduct.Pages.ReportCards;
using POM.Helper;
using Pupil.Components.Common;
using Pupil.Data;
using Selene.Support.Attributes;
using SeSugar;
using SeSugar.Automation;
using SeSugar.Data;
using TestSettings;
using POM.Base;

// ReSharper disable once CheckNamespace
namespace Pupil.Conduct.Tests
{
    public class ReportCardTests
    {
        private static readonly string[] FeatureList = new []{ "ReportCards" };

        /// <summary>
        /// Check search
        /// </summary>
        [WebDriverTest(TimeoutSeconds = 2000, Enabled = true, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { PupilTestGroups.ReportCards.ReportCardSearch, PupilTestGroups.Priority.Priority2, "ReportCards_1" })]
        public void Test_ReportCardSearch()
        {
            //Arrange
            DataPackage dataPackage;
            string pupilName;
            var learnerId = CreateBasicReportCardPackage(out dataPackage, out pupilName);
            //Act
            using (new DataSetup(purgeBeforeInsert: false, purgeAfterTest: true, packages: dataPackage))
            {
                try
                {
                    //Arrange
                    SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.TestUser, enabledFeatures: FeatureList);//TODO: once permissions are done
                    //SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SeniorManagementTeam); //TODO: Once feeture bee is released
                    AutomationSugar.NavigateMenu("Tasks", "Pupil Conduct", "Report Cards");

                    //Act
                    var reportCardTriplet = new ReportCardTriplet();
                    reportCardTriplet.SearchCriteria.PupilName = pupilName;
                    var reportCardRecordPage = reportCardTriplet.SearchCriteria.Search();
                    var searchResultCount = reportCardRecordPage.ToList().Count;

                    //Assert
                    Assert.IsTrue(searchResultCount == 1);
                }
                finally
                {
                    // Teardown
                    PurgeLinkedData.DeleteReportCardsForLearner(learnerId);
                }
            }
        }

        /// <summary>
        /// Check search
        /// </summary>
        [WebDriverTest(TimeoutSeconds = 2000, Enabled = true, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { PupilTestGroups.ReportCards.ReportCardSearch, PupilTestGroups.Priority.Priority2, "ReportCards_11"})]
        public void Test_ReportCardSearch_WithReport()
        {
            //Arrange
            DataPackage dataPackage;
            string pupilName;
            var reportCardId = CreateBasicReportCardPackageWithCurrentReport(out dataPackage, out pupilName);
            //Act
            
            try
            {
                //Arrange
                SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.TestUser, enabledFeatures: FeatureList);//TODO: once permissions are done
                //SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SeniorManagementTeam); //TODO: Once feeture bee is released
                AutomationSugar.NavigateMenu("Tasks", "Pupil Conduct", "Report Cards");
                using (new DataSetup(purgeBeforeInsert: false, purgeAfterTest: true, packages: dataPackage))
                {
                    Thread.Sleep(TimeSpan.FromSeconds(2));
                    //Act
                    var reportCardTriplet = new ReportCardTriplet();
                    reportCardTriplet.SearchCriteria.IsOnReport = true;
                    reportCardTriplet.SearchCriteria.PupilName = pupilName;

                    var reportCardRecordPage = reportCardTriplet.SearchCriteria.Search()
                        .SingleOrDefault(x => x.Name.Equals(pupilName))
                        .Click<ReportCardRecordPage>();

                    var reportCardSearchCount = reportCardRecordPage.SearchResults.Count();
                    ReportCardRecordPage.ReportCardSearchResultTile tile = reportCardRecordPage.SearchResults.FirstOrDefault();
                    var reportCardDetail = tile.Click<ReportCardDetail>();

                    //Assert
                    Assert.IsTrue(reportCardDetail != null);
                    Assert.IsTrue(reportCardSearchCount == 1);

                    Assert.IsTrue(reportCardDetail.Name == "Test Report Card");

                }
            }
            finally
            {
                // Teardown
                PurgeLinkedData.DeleteReportCard(reportCardId);
            }
            
        }

        /// <summary>
        /// Check search
        /// </summary>
        [WebDriverTest(TimeoutSeconds = 2000, Enabled = true, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { PupilTestGroups.ReportCards.ReportCardSearch, PupilTestGroups.Priority.Priority2, "ReportCards_12" })]
        public void Test_ReportCardSearch_WithNoOutcome()
        {
            //Arrange
            DataPackage dataPackage;
            string pupilName;
            Guid reportCardId = CreateBasicReportCardPackageWithPastReportWithoutOutcome(out dataPackage, out pupilName);
            //Act

            try
            {
                //Arrange
                SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.TestUser, enabledFeatures: FeatureList);//TODO: once permissions are done
                //SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SeniorManagementTeam); //TODO: Once feeture bee is released
                AutomationSugar.NavigateMenu("Tasks", "Pupil Conduct", "Report Cards");
                using (new DataSetup(purgeBeforeInsert: false, purgeAfterTest: true, packages: dataPackage))
                {
                    Thread.Sleep(TimeSpan.FromSeconds(2));
                    //Act
                    ReportCardTriplet reportCardTriplet = new ReportCardTriplet();
                    reportCardTriplet.SearchCriteria.NoOutcome = true;
                    reportCardTriplet.SearchCriteria.PupilName = pupilName;

                    Thread.Sleep(500);

                    SearchResultsComponent<ReportCardTriplet.ReportCardSearchResultTile> tileObject = new SearchResultsComponent<ReportCardTriplet.ReportCardSearchResultTile>(reportCardTriplet);

                    ReportCardRecordPage reportCardRecordPage = tileObject
                        .SingleOrDefault(x => x.Name.Equals(pupilName))
                        .Click<ReportCardRecordPage>();

                    int reportCardSearchCount = reportCardRecordPage.SearchResults.Count();
                    ReportCardRecordPage.ReportCardSearchResultTile tile = reportCardRecordPage.SearchResults.FirstOrDefault();
                    ReportCardDetail reportCardDetail = tile.Click<ReportCardDetail>();

                    //Assert
                    Assert.IsTrue(reportCardDetail != null);
                    Assert.IsTrue(reportCardSearchCount == 1);

                    Assert.IsTrue(reportCardDetail.Name == "Test Report Card");

                }
            }
            finally
            {
                // Teardown
                PurgeLinkedData.DeleteReportCard(reportCardId);
            }

        }

        /// <summary>
        /// Check search
        /// </summary>
        [WebDriverTest(TimeoutSeconds = 2000, Enabled = true, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { PupilTestGroups.ReportCards.ReportCardSearch, PupilTestGroups.Priority.Priority2, "ReportCards_12" })]
        public void Test_ReportCardSearch_WithNoOutcomeAndCurrent()
        {
            //Arrange
            DataPackage dataPackage1, dataPackage2;
            string pupilName1, pupilName2;
            Guid reportCardId1 = CreateBasicReportCardPackageWithPastReportWithoutOutcome(out dataPackage1, out pupilName1);
            Guid reportCardId2 = CreateBasicReportCardPackageWithCurrentReport(out dataPackage2, out pupilName2);
            //Act

            try
            {
                //Arrange
                SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.TestUser, enabledFeatures: FeatureList);//TODO: once permissions are done
                //SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SeniorManagementTeam); //TODO: Once feeture bee is released
                AutomationSugar.NavigateMenu("Tasks", "Pupil Conduct", "Report Cards");
                using (new DataSetup(purgeBeforeInsert: false, purgeAfterTest: true, packages: new DataPackage[] { dataPackage1, dataPackage2 }))
                {
                    Thread.Sleep(TimeSpan.FromSeconds(2));
                    //Act
                    ReportCardTriplet reportCardTriplet = new ReportCardTriplet();
                    reportCardTriplet.SearchCriteria.IsOnReport = true;
                    reportCardTriplet.SearchCriteria.NoOutcome = true;

                    // wait for auto search to populate results
                    Thread.Sleep(1000);

                    SearchResultsComponent<ReportCardTriplet.ReportCardSearchResultTile> tileObject = new SearchResultsComponent<ReportCardTriplet.ReportCardSearchResultTile>(reportCardTriplet);

                    ReportCardRecordPage reportCardRecordPage = tileObject
                        .SingleOrDefault(x => x.Name.Equals(pupilName1))
                        .Click<ReportCardRecordPage>();

                    int reportCardSearchCount = reportCardRecordPage.SearchResults.Count();
                    Assert.IsTrue(reportCardSearchCount == 1);

                    ReportCardRecordPage.ReportCardSearchResultTile tile = reportCardRecordPage.SearchResults.FirstOrDefault();
                    ReportCardDetail reportCardDetail = tile.Click<ReportCardDetail>();

                    //Assert
                    Assert.IsTrue(reportCardDetail != null);

                    Assert.IsTrue(reportCardDetail.Name == "Test Report Card");

                    ReportCardRecordPage reportCardRecordPage2 = tileObject
                        .SingleOrDefault(x => x.Name.Equals(pupilName2))
                        .Click<ReportCardRecordPage>();

                    int reportCardSearchCount2 = reportCardRecordPage2.SearchResults.Count();
                    Assert.IsTrue(reportCardSearchCount2 == 1);

                    ReportCardRecordPage.ReportCardSearchResultTile tile2 = reportCardRecordPage2.SearchResults.FirstOrDefault();
                    ReportCardDetail reportCardDetail2 = tile2.Click<ReportCardDetail>();

                    //Assert
                    Assert.IsTrue(reportCardDetail2 != null);

                    Assert.IsTrue(reportCardDetail2.Name == "Test Report Card");

                }
            }
            finally
            {
                // Teardown
                PurgeLinkedData.DeleteReportCard(reportCardId1);
                PurgeLinkedData.DeleteReportCard(reportCardId2);
            }

        }

        //[WebDriverTest(TimeoutSeconds = 2000, Enabled = true, Browsers = new[] {BrowserDefaults.Chrome},
        //    Groups = new[]
        //    {
        //        PupilTestGroups.ReportCards.ReportCardSearch, PupilTestGroups.Priority.Priority2, "ReportCards_2"
        //    })]
        public void Test_ReportCardSubPanel_Collapsable()
        {
            DataPackage dataPackage;
            string pupilName;
            var learnerId = CreateBasicReportCardPackage(out dataPackage, out pupilName);

            using (new DataSetup(purgeBeforeInsert: false, purgeAfterTest: true, packages: dataPackage))
            {
                try
                {
                    //Arrange
                    SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.TestUser, enabledFeatures: FeatureList);//TODO: once permissions are done
                    //SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SeniorManagementTeam); //TODO: Once feeture bee is released
                    AutomationSugar.NavigateMenu("Tasks", "Pupil Conduct", "Report Cards");

                    //Act
                    var reportCardTriplet = new ReportCardTriplet();
                    reportCardTriplet.SearchCriteria.PupilName = pupilName;
                    var reportCardRecordPage = reportCardTriplet.SearchCriteria.Search()
                        .SingleOrDefault(x => x.Name.Equals(pupilName))
                        .Click<ReportCardRecordPage>(); 

                    reportCardRecordPage.ToggleSearchResultButton.Click();
                    var sliderState = reportCardRecordPage.GetSliderState();

                    //Assert
                    Assert.IsTrue(sliderState == "closed");
                }
                finally
                {
                    // Teardown
                    PurgeLinkedData.DeleteReportCardsForLearner(learnerId);
                }
            }

        }


        #region Test Menu Permission

        [WebDriverTest(TimeoutSeconds = 2000, Enabled = true, Browsers = new[] {BrowserDefaults.Chrome},
            Groups = new[]
            {
                PupilTestGroups.ReportCards.ReportCardSearch, PupilTestGroups.Priority.Priority2, "ReportCards_2", "ReportCardSample3"
            })]
        public void SchoolAdministratorCanAccessReportCard()
        {
            bool isAccessible = SeleniumHelper.HasMenuPermission("Automation_Report_Card_Menu", FeatureList, SeleniumHelper.iSIMSUserType.SchoolAdministrator);
            Assert.AreEqual(true, isAccessible);
        }

        [WebDriverTest(TimeoutSeconds = 2000, Enabled = true, Browsers = new[] { BrowserDefaults.Chrome },
            Groups = new[]
            {
                PupilTestGroups.ReportCards.ReportCardSearch, PupilTestGroups.Priority.Priority2, "ReportCards_2", "ReportCardSample3"
            })]
        public void SeniorManagementCanAccessReportCard()
        {
            bool isAccessible = SeleniumHelper.HasMenuPermission("Automation_Report_Card_Menu", FeatureList, SeleniumHelper.iSIMSUserType.SeniorManagementTeam);
            Assert.AreEqual(true, isAccessible);
        }

        [WebDriverTest(TimeoutSeconds = 2000, Enabled = true, Browsers = new[] { BrowserDefaults.Chrome },
            Groups = new[]
            {
                PupilTestGroups.ReportCards.ReportCardSearch, PupilTestGroups.Priority.Priority2, "ReportCards_2", "ReportCardSample3"
            })]
        public void ClassTeacherCanAccessReportCard()
        {
            bool isAccessible = SeleniumHelper.HasMenuPermission("Automation_Report_Card_Menu", FeatureList, SeleniumHelper.iSIMSUserType.ClassTeacher);
            Assert.AreEqual(true, isAccessible);
        }

        [WebDriverTest(TimeoutSeconds = 2000, Enabled = true, Browsers = new[] { BrowserDefaults.Chrome },
            Groups = new[]
            {
                PupilTestGroups.ReportCards.ReportCardSearch, PupilTestGroups.Priority.Priority2, "ReportCards_2", "ReportCardSample3"
            })]
        public void SenCoordinatorCanAccessReportCard()
        {
            bool isAccessible = SeleniumHelper.HasMenuPermission("Automation_Report_Card_Menu", FeatureList, SeleniumHelper.iSIMSUserType.SENCoordinator);
            Assert.AreEqual(true, isAccessible);
        }
        #endregion


        private Guid CreateBasicReportCardPackage(out DataPackage dataPackage, out string pupilName)
        {
            var learnerId = Guid.NewGuid();
            dataPackage = this.BuildDataPackage();
            const string forename = "reportCard_Fname";
            var surname = Utilities.GenerateRandomString(10, "ReportCard_1");
            pupilName = string.Format("{0}, {1}", surname, forename);
            
            //Add learner with Report Card
            dataPackage.AddBasicLearner(learnerId, surname, forename, dateOfBirth: new DateTime(2008, 05, 30),
                    dateOfAdmission: new DateTime(2015, 10, 03))
                .AddReportCard(learnerId, "Test Report Card", new DateTime(2016, 01, 01),
                    new DateTime(2016, 02, 02));
            return learnerId;
        }


        private Guid CreateBasicReportCardPackageWithCurrentReport(out DataPackage dataPackage, out string pupilName)
        {
            var learnerId = Guid.NewGuid();
            var reportCardId = Guid.NewGuid();
            dataPackage = this.BuildDataPackage();
            const string forename = "reportCard_Fname";
            var surname = Utilities.GenerateRandomString(10, "ReportCard_1");
            pupilName = string.Format("{0}, {1}", surname, forename);
            
            //Add learner with Report Card
            dataPackage.AddBasicLearner(learnerId, surname, forename, dateOfBirth: new DateTime(2008, 05, 30),
                    dateOfAdmission: new DateTime(2015, 10, 03), tenantId: SeSugar.Environment.Settings.TenantId)
                .AddReportCard(learnerId, "Test Report Card", DateTime.Today.AddDays(-3),
                    DateTime.Today.AddDays(+5),  reportCardId:reportCardId)
                .AddReportCardTarget(reportCardId);
            return reportCardId;
        }

        private Guid CreateBasicReportCardPackageWithPastReportWithoutOutcome(out DataPackage dataPackage, out string pupilName)
        {
            Guid learnerId = Guid.NewGuid();
            Guid reportCardId = Guid.NewGuid();
            dataPackage = this.BuildDataPackage();
            const string forename = "noOutcome_Fname";
            var surname = Utilities.GenerateRandomString(10, "ReportCardNoOutcome_1");
            pupilName = string.Format("{0}, {1}", surname, forename);

            //Add learner with Report Card
            dataPackage.AddBasicLearner(learnerId, surname, forename, dateOfBirth: new DateTime(2008, 05, 30),
                    dateOfAdmission: new DateTime(2015, 10, 03), tenantId: SeSugar.Environment.Settings.TenantId)
                .AddReportCard(learnerId, "Test Report Card", DateTime.Today.AddDays(-30),
                    DateTime.Today.AddDays(-1), reportCardId: reportCardId)
                .AddReportCardTarget(reportCardId);
            return reportCardId;
        }
    }
}
