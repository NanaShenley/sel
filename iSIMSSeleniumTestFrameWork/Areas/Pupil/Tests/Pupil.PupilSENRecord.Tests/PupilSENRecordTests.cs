using System;
using System.Linq;
using System.Threading;
using NUnit.Framework;
using OpenQA.Selenium;
using POM.Components.Common;
using POM.Components.Pupil;
using POM.Helper;
using Pupil.Components.Common;
using Pupil.Data;
using SeSugar.Data;
using SharedComponents.CRUD;
using TestSettings;
using Selene.Support.Attributes;
using SeSugar.Automation;

namespace Pupil.PupilSENRecord.Tests
{
    public class SenRecordTests
    {
        private SeleniumHelper.iSIMSUserType LoginAs = SeleniumHelper.iSIMSUserType.SchoolAdministrator;

        /// <summary>
        /// Description: Exercise ability to record a 'SEN Stage'
        /// Role: School Administrator
        /// </summary>
        /// <exception cref="ElementNotVisibleException">Thrown when the target element is not visible.</exception>
        /// <exception cref="StaleElementReferenceException">Thrown when the target element is no longer valid in the document DOM.</exception>
        /// <exception cref="Exception">Condition.</exception>
        [WebDriverTest(Enabled = true, Browsers = new[] { BrowserDefaults.Chrome },
            Groups = new[] { PupilTestGroups.SENRecord.Page, PupilTestGroups.Priority.Priority2 })]
        public void Add_SEN_Stage_To_Pupil_NIVariant()
        {
            //Arrange
            var learnerId = Guid.NewGuid();
            var senDataPackage = this.BuildDataPackage();

            //Add random string surname
            var surname = "Add_SEN_Stage" + SeleniumHelper.GenerateRandomString(6);

            senDataPackage.AddBasicLearner(learnerId, surname, "Add_SEN_Stage", dateOfBirth: new DateTime(2005, 05, 05),
                dateOfAdmission: new DateTime(2012, 06, 06));

            //Act
            using (new DataSetup(purgeBeforeInsert: false, purgeAfterTest: true, packages: senDataPackage))
            {
                try
                {
                    SeleniumHelper.Login(LoginAs);
                    SeleniumHelper.NavigateQuickLink(PupilElements.PupilRecordsQuickLink);
                    SearchCriteria.Search();
                    SearchResults.WaitForResults();
                    SearchResults.Click(learnerId.ToString());

                    var pupilRecordTriplet = new PupilSearchTriplet();
                    var searchResults = pupilRecordTriplet.SearchCriteria.Search();
                    var pupilSearchTile = searchResults.Single(t => t.Name.Equals(surname + ", Add_SEN_Stage"));
                    var pupilRecord = pupilSearchTile.Click<PupilRecordPage>();

                    //Navigate to and expand the 'Statutory SEN' section.
                    AutomationSugar.ExpandAccordionPanel("section_menu_Statutory SEN");
                    SeleniumHelper.WaitForElementClickableThenClick(By.CssSelector(SeleniumHelper.AutomationId("add_sen_stage_button")));
                    Thread.Sleep(500);

                    //Add data to SEN Stage Table
                    pupilRecord.SenStages[0].Stage = Queries.GetLookupDescription("SENStatus");
                    pupilRecord.SenStages[0].StartDay = "05/10/2016";

                    //Verify success message has displayed
                    pupilRecord.SavePupil();

                    Assert.AreEqual(true, pupilRecord.IsSuccessMessageDisplayed(), "Success message is not displayed");
                }
                finally
                {
                    // Tear down linked records before clean up
                    PurgeLinkedData.DeleteSENStageForLearner(learnerId);
                }

                // close pupil, test complete
                SeleniumHelper.CloseTab("Pupil Record");
            }
        }

        /// <summary>
        /// Description: Exercise ability to record a 'SEN Stage'
        /// Role: School Administrator
        /// </summary>
        /// <exception cref="ElementNotVisibleException">Thrown when the target element is not visible.</exception>
        /// <exception cref="StaleElementReferenceException">Thrown when the target element is no longer valid in the document DOM.</exception>
        /// <exception cref="Exception">Condition.</exception>
        [WebDriverTest(Enabled = true, Browsers = new[] { BrowserDefaults.Chrome },
            Groups = new[] { PupilTestGroups.SENRecord.Page, PupilTestGroups.Priority.Priority2 })]
        [Variant(Variant.AllEnglish | Variant.AllIndependant | Variant.AllMultiphase)]
        public void Add_SEN_Stage_To_Pupil_NonNIVariant()
        {
            //Arrange
            var learnerId = Guid.NewGuid();
            var senDataPackage = this.BuildDataPackage();

            //Add random string surname
            var surname = "Add_SEN_Stage" + SeleniumHelper.GenerateRandomString(6);

            senDataPackage.AddBasicLearner(learnerId, surname, "Add_SEN_Stage", dateOfBirth: new DateTime(2005, 05, 05),
                dateOfAdmission: new DateTime(2012, 06, 06));

            //Act
            using (new DataSetup(purgeBeforeInsert: false, purgeAfterTest: true, packages: senDataPackage))
            {
                try
                {
                    SeleniumHelper.Login(LoginAs);
                    SeleniumHelper.NavigateQuickLink(PupilElements.PupilRecordsQuickLink);
                    SearchCriteria.Search();
                    SearchResults.WaitForResults();
                    SearchResults.Click(learnerId.ToString());

                    var pupilRecordTriplet = new PupilSearchTriplet();
                    var searchResults = pupilRecordTriplet.SearchCriteria.Search();
                    var pupilSearchTile = searchResults.Single(t => t.Name.Equals(surname + ", Add_SEN_Stage"));
                    var pupilRecord = pupilSearchTile.Click<PupilRecordPage>();

                    //Navigate to and expand the 'Statutory SEN' section.
                    AutomationSugar.ExpandAccordionPanel("section_menu_Statutory SEN");
                    SeleniumHelper.WaitForElementClickableThenClick(By.CssSelector(SeleniumHelper.AutomationId("add_sen_status_button")));
                    Thread.Sleep(500);

                    //Add data to SEN Stage Table
                    pupilRecord.SenStages[0].Stage = "No Special Educational Need";
                    pupilRecord.SenStages[0].StartDay = "05/10/2016";

                    //Verify success message has displayed
                    pupilRecord.SavePupil();

                    Assert.AreEqual(true, pupilRecord.IsSuccessMessageDisplayed(), "Success message is not displayed");
                }
                finally
                {
                    // Tear down linked records before clean up
                    PurgeLinkedData.DeleteSENStageForLearner(learnerId);
                }

                // close pupil, test complete
                SeleniumHelper.CloseTab("Pupil Record");
            }
        }

        /// <summary>
        /// Description: Exercise ability to record a 'SEN Need' record whilst in the 'Statutory SEN' section of a selected pupil record.
        /// Role: School Administrator
        /// </summary>
        /// <exception cref="StaleElementReferenceException">Thrown when the target element is no longer valid in the document DOM.</exception>
        /// <exception cref="ElementNotVisibleException">Thrown when the target element is not visible.</exception>
        [WebDriverTest(Enabled = true, Browsers = new[] { BrowserDefaults.Chrome },
            Groups = new[] { PupilTestGroups.SENRecord.Page, PupilTestGroups.Priority.Priority2, "PSR_ASNTP" })]
        public void Add_SEN_Need_To_Pupil()
        {
            //Arrange
            var learnerId = Guid.NewGuid();
            var senDataPackage = this.BuildDataPackage();

            //Add random string surname
            var surname = "Add_SEN_Need" + SeleniumHelper.GenerateRandomString(6);

            senDataPackage.AddBasicLearner(learnerId, surname, "Add_SEN_Need", dateOfBirth: new DateTime(2005, 05, 30),
                dateOfAdmission: new DateTime(2012, 10, 03));

            //Act
            using (new DataSetup(purgeBeforeInsert: false, purgeAfterTest: true, packages: senDataPackage))
            {
                try
                {
                    SeleniumHelper.Login(LoginAs);
                    SeleniumHelper.NavigateQuickLink(PupilElements.PupilRecordsQuickLink);
                    SearchCriteria.Search();
                    SearchResults.WaitForResults();
                    SearchResults.Click(learnerId.ToString());

                    var pupilRecordTriplet = new PupilSearchTriplet();
                    var searchResults = pupilRecordTriplet.SearchCriteria.Search();
                    var pupilSearchTile = searchResults.Single(t => t.Name.Equals(surname + ", Add_SEN_Need"));
                    var pupilRecord = pupilSearchTile.Click<PupilRecordPage>();

                    //Navigate to and expand the 'Statutory SEN' section.
                    AutomationSugar.ExpandAccordionPanel("section_menu_Statutory SEN");
                    SeleniumHelper.WaitForElementClickableThenClick(By.CssSelector(SeleniumHelper.AutomationId("add_sen_need_button")));

                    //Add data to SEN Need Table
                    pupilRecord.SenNeeds[0].NeedType = Queries.GetLookupDescriptionByCode("SENNeedType", "MLD", isVisibleOnly: true);
                    pupilRecord.SenNeeds[0].Notes = "Added as part of Selenium automated testing";
                    pupilRecord.SenNeeds[0].StartDay = "01/06/2016";


                    //Verify success message has displayed
                    pupilRecord.SavePupil();

                    Assert.AreEqual(true, pupilRecord.IsSuccessMessageDisplayed(), "Success message is not displayed");
                }
                finally
                {
                    // Tear down the linked SEN Need before deleting the pupil
                    PurgeLinkedData.DeleteSENNeedForLearner(learnerId);
                }

                //Exit the 'SEN Record' screen, thus returning focus back to the 'Pupil Record' screen.
                SeleniumHelper.CloseTab("SEN Record");
            }
        }

        /// <summary>
        /// Description: Exercise ability to record a 'SEN Provision' record whilst in the 'Statutory SEN' section of a selected pupil record.
        /// Role: School Administrator
        /// </summary>
        /// <exception cref="StaleElementReferenceException">Thrown when the target element is no longer valid in the document DOM.</exception>
        /// <exception cref="ElementNotVisibleException">Thrown when the target element is not visible.</exception>
        [WebDriverTest(TimeoutSeconds = 1500, Enabled = true, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { PupilTestGroups.SENRecord.Page, PupilTestGroups.Priority.Priority2, "PSR_ASPTP" })]
        public void Add_SEN_Provision_To_Pupil()
        {
            //Arrange
            var learnerId = Guid.NewGuid();
            var senDataPackage = this.BuildDataPackage();

            //Add random string surname
            var surname = "Add_SEN_Provision" + SeleniumHelper.GenerateRandomString(6);

            senDataPackage.AddBasicLearner(learnerId, surname, "Add_SEN_Provision", dateOfBirth: new DateTime(2005, 05, 30),
                dateOfAdmission: new DateTime(2012, 10, 03));

            //Act
            using (new DataSetup(purgeBeforeInsert: false, purgeAfterTest: true, packages: senDataPackage))
            {
                try
                {
                    SeleniumHelper.Login(LoginAs);
                    SeleniumHelper.NavigateQuickLink(PupilElements.PupilRecordsQuickLink);
                    SearchCriteria.Search();
                    SearchResults.WaitForResults();
                    SearchResults.Click(learnerId.ToString());

                    var pupilRecordTriplet = new PupilSearchTriplet();
                    var searchResults = pupilRecordTriplet.SearchCriteria.Search();
                    var pupilSearchTile = searchResults.Single(t => t.Name.Equals(surname + ", Add_SEN_Provision"));
                    var pupilRecord = pupilSearchTile.Click<PupilRecordPage>();

                    //Navigate to and expand the 'Statutory SEN' section.
                    AutomationSugar.ExpandAccordionPanel("section_menu_Statutory SEN");
                    SeleniumHelper.WaitForElementClickableThenClick(By.CssSelector(SeleniumHelper.AutomationId("add_sen_provision_button")));

                    //Enter data to SEN Provision Table
                    pupilRecord.SenProvisions[0].ProvisionType = "Not Specified";
                    pupilRecord.SenProvisions[0].StartDay = "01/06/2016";
                    pupilRecord.SenProvisions[0].EndDay = "06/06/2016";

                    //Verify success message has displayed
                    pupilRecord.SavePupil();

                    Assert.AreEqual(true, pupilRecord.IsSuccessMessageDisplayed(), "Success message is not displayed");
                }
                finally
                {
                    // Tear down the linked SEN Need before deleting the pupil
                    PurgeLinkedData.DeleteSENProvisionForLearner(learnerId);
                }

                //Exit the 'SEN Record' screen, thus returning focus back to the 'Pupil Record' screen.
                SeleniumHelper.CloseTab("SEN Record");
            }
        }

        [WebDriverTest(TimeoutSeconds = 1500, Enabled = true, Browsers = new[] { BrowserDefaults.Chrome },
            Groups = new[] { PupilTestGroups.SENRecord.Page, PupilTestGroups.Priority.Priority2 })]
        public void Add_SEN_Review_To_Learner()
        {
            //Arrange
            var learnerId = Guid.NewGuid();
            var senDataPackage = this.BuildDataPackage();

            //Add random string surname
            var surname = "7-SEN" + SeleniumHelper.GenerateRandomString(6);

            senDataPackage.AddBasicLearner(learnerId, surname, "SEN TEST", dateOfBirth: new DateTime(2005, 05, 30),
                dateOfAdmission: new DateTime(2012, 10, 03));

            senDataPackage.AddSENSStagetoLearner(learnerId);

            //Act
            using (new DataSetup(purgeBeforeInsert: false, purgeAfterTest: true, packages: senDataPackage))
            {
                try
                {
                    SeleniumHelper.Login(LoginAs);
                    AutomationSugar.NavigateMenu("Tasks", "Pupils", "SEN Records");

                    var SENRecordTriplet = new SenRecordTriplet();
                    var searchResults = SENRecordTriplet.SearchCriteria.Search();
                    var pupilSearchTile = searchResults.Single(t => t.Name.Equals(surname + ", SEN TEST"));
                    var SenRecord = pupilSearchTile.Click<SenRecordDetailPage>();

                    //Add "SEN Review"
                    var addSenReviewDialog = SenRecord.ClickAddSenReview();
                    addSenReviewDialog.ReviewType = "Annual";
                    addSenReviewDialog.ReviewStatus = "Planned";
                    addSenReviewDialog.StartDate = "06/06/2016";

                    //Add People Involved
                    var addPeopleInvolved = addSenReviewDialog.ClickAddPeopleInvolved();
                    var searchStaffResults = addPeopleInvolved.SearchCriteria.Search();
                    var staffTile = searchStaffResults[0];
                    staffTile.Click();

                    //Close Add Person Involved dialog box
                    addPeopleInvolved.ClickOk(5);

                    // Add relationship to SEN Review
                    // (cannot be added as part of Person Involved as bug in SIMS8 does not allow it)
                    var PeepsInvolvedRelationship = SeleniumHelper.FindElement(By.CssSelector("[data-maintenance-container = 'SENReviewParticipants'] input[id^='SENReviewParticipant']:first-of-type:not([type= hidden]"));
                    PeepsInvolvedRelationship.SetText(Queries.GetLookupDescriptionByCode("SENPeopleInvolvedRelationship", "SENC") + Environment.NewLine);

                    // Close Add SEN review dialog box & Save
                    addSenReviewDialog.ClickOk(5);
                    SenRecord.Save();

                    //Verify new "SEN Review" record
                    // TODO FIND OUT WHY THIS RETURNS STRANGE FORMAT DATES

                    var senReviewRow = SenRecord.SenReviews.Rows.FirstOrDefault(t => t.StartDate.Equals("6/6/2016"));
                    Assert.IsNotNull(senReviewRow);
                    Assert.AreEqual(true, SenRecord.IsMessageSuccessAppear(), "Success message is not displayed");
                }
                finally
                {
                    // Tear down linked records before clean up
                    PurgeLinkedData.DeleteSENReviewParticipantForLearner(learnerId);
                    PurgeLinkedData.DeleteSENStageForLearner(learnerId);
                    PurgeLinkedData.DeleteSENReviewForLearner(learnerId);
                }

                // close tab, test complete
                SeleniumHelper.CloseTab("SEN Record");
            }
        }

        /// <summary>
        /// TC PU66b
        /// Au : An Nguyen
        /// Description: Exercise ability to add a single 'SEN Statement' to a pupil that has SEN Details.
        /// Role: School Administrator
        /// </summary>
        [WebDriverTest(TimeoutSeconds = 1500, Enabled = true, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { PupilTestGroups.SENRecord.Page, PupilTestGroups.Priority.Priority2 })]
        public void Add_SEN_Statement_To_Learner()
        {
            //Arrange
            var learnerId = Guid.NewGuid();
            var senDataPackage = this.BuildDataPackage();

            //Add random string surname
            var surname = "8-SEN_" + SeleniumHelper.GenerateRandomString(7);

            senDataPackage.AddBasicLearner(learnerId, surname, "SEN TEST", dateOfBirth: new DateTime(2005, 05, 05), dateOfAdmission: new DateTime(2012, 10, 03));

            senDataPackage.AddSENSStagetoLearner(learnerId);

            //Act
            using (new DataSetup(purgeBeforeInsert: false, purgeAfterTest: true, packages: senDataPackage))
            {
                try
                {
                    SeleniumHelper.Login(LoginAs);
                    AutomationSugar.NavigateMenu("Tasks", "Pupils", "SEN Records");

                    var SENRecordTriplet = new SenRecordTriplet();
                    var searchResults = SENRecordTriplet.SearchCriteria.Search();
                    var pupilSearchTile = searchResults.Single(t => t.Name.Equals(surname + ", SEN TEST"));
                    var SenRecord = pupilSearchTile.Click<SenRecordDetailPage>();

                    //Add "SEN Statement"
                    var addSenStatement = SenRecord.ClickAddSenStatement();

                    //Add Date Requested and Date Parent Consulted
                    addSenStatement.DateRequested = "05/05/2016";
                    addSenStatement.DateConsulted = "05/05/2016";

                    //Add ELB Officer
                    var elbOfficer = addSenStatement.ClickSelectOfficer();
                    var officerTile = elbOfficer.SearchCriteria.Search()[0];
                    officerTile.Click();
                    elbOfficer.ClickOk(5);

                    //Add ELB Response and Statement Outcome
                    var statutoryResponse = Queries.GetLookupDescriptionByCode("SENStatutoryAssessment", "AGREE", isVisibleOnly: true);
                    var statutoryStatement = Queries.GetLookupDescriptionByCode("SenStatutoryStatement", "STATE", isVisibleOnly: true);
                    addSenStatement.ELBReponse = statutoryResponse;
                    Thread.Sleep(1000);
                    addSenStatement.StatementOutcome = statutoryStatement;
                    Thread.Sleep(1000);

                    //Save "SEN Statement"
                    addSenStatement.ClickOk(5);
                    SenRecord.Save();

                    //Verify new "SEN Statement" record
                    var senStatementRow = SenRecord.SenStatements.Rows.FirstOrDefault(
                    t => t.DateRequested.Equals("5/5/2016") && t.ELBResponse.Equals(statutoryResponse));

                    Assert.AreNotEqual(null, senStatementRow, "Add SEN Statement unsuccessfull");
                    Assert.AreEqual(statutoryStatement, senStatementRow.StatementOutcome, "Statement Outcome is incorrect");
                }
                finally
                {
                    // Teardown
                    PurgeLinkedData.DeleteSENStatementForLearner(learnerId);
                    PurgeLinkedData.DeleteSENStageForLearner(learnerId);
                    PurgeLinkedData.DeleteSENReviewForLearner(learnerId);
                }
            }
        }

        #region EHCP 
        /// <summary>
        /// Test ability to add EHCP
        /// </summary>
        [WebDriverTest(TimeoutSeconds = 1500, Enabled = true, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { "EHCP", PupilTestGroups.SENRecord.Page, PupilTestGroups.Priority.Priority2, "PSR_AETL" })]
        [Variant(Variant.AllEnglish | Variant.AllIndependant)]
        public void Add_EHCP_ToLearner()
        {
            //Arrange
            var learnerId = Guid.NewGuid();
            var senDataPackage = this.BuildDataPackage();

            //Add random string surname
            var surname = "8-SEN_" + SeleniumHelper.GenerateRandomString(7);

            senDataPackage.AddBasicLearner(learnerId, surname, "SEN TEST", dateOfBirth: new DateTime(2005, 05, 05), dateOfAdmission: new DateTime(2012, 10, 03));

            senDataPackage.AddSENSStagetoLearner(learnerId);

            //Act
            using (new DataSetup(purgeBeforeInsert: false, purgeAfterTest: true, packages: senDataPackage))
            {
                try
                {
                    SeleniumHelper.Login(LoginAs);
                    AutomationSugar.NavigateMenu("Tasks", "Pupils", "SEN Records");

                    var senRecordTriplet = new SenRecordTriplet();
                    var searchResults = senRecordTriplet.SearchCriteria.Search();
                    var pupilSearchTile = searchResults.Single(t => t.Name.Equals(surname + ", SEN TEST"));
                    var senRecord = pupilSearchTile.Click<SenRecordDetailPage>();

                    //Add "EHCP"
                    var addEhcpDialog = senRecord.ClickAddEhcp();

                    //Add Date Requested and Date Parent Consulted
                    addEhcpDialog.DateRequested = "05/05/2016";
                    addEhcpDialog.DateConsulted = "05/05/2016";

                    //Save "SEN Statement"
                    addEhcpDialog.ClickOk(5);
                    senRecord.Save();

                    //Verify new "SEN Statement" record
                    var senStatementRow = senRecord.Ehcps.Rows.FirstOrDefault(
                    t => t.DateRequested.Equals("5/5/2016"));

                    Assert.AreNotEqual(null, senStatementRow, "Add SEN EHCP unsuccessfull");
                }
                finally
                {
                    // Teardown
                    PurgeLinkedData.DeleteEhcpForLearner(learnerId);
                    PurgeLinkedData.DeleteSENStageForLearner(learnerId);
                    PurgeLinkedData.DeleteSENReviewForLearner(learnerId);
                }
            }
        }
        #endregion

        /// <summary>
        /// TC PU69
        /// Au : An Nguyen
        /// Description: Exercise ability to add a school level SEN Need Type.
        /// Role: School Administrator
        /// </summary>
        [WebDriverTest(TimeoutSeconds = 600, Enabled = true, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { PupilTestGroups.SENRecord.Page, PupilTestGroups.Priority.Priority2 })]
        public void Add_SEN_Need_Type()
        {
            var senCode = SeleniumHelper.GenerateRandomString(3);

            try
            {
                SeleniumHelper.Login(LoginAs);
                AutomationSugar.NavigateMenu("Lookups", "Pupils", "SEN Need Type");

                //Add new SEN Need Type
                var senNeedTypeTriplet = new SenNeedTypeTriplet();
                var senNeedTypePage = senNeedTypeTriplet.AddSenNeedType();
                var senNeedTypeRow = senNeedTypePage.SenNeedTypeTable.GetLastRow();

                senNeedTypeRow.Code = senCode;
                senNeedTypeRow.Description = string.Format("Selenium Test Entry - {0}", senCode);
                senNeedTypeRow.DisplayOrder = "999";
                senNeedTypeRow.IsVisible = true;
                senNeedTypeRow.Category = "Other";

                //Save SEN Need Type
                senNeedTypePage.ClickSave();

                //Verify success message
                Assert.AreEqual(true, senNeedTypePage.IsSuccessMessageDisplayed(), "Success message do not display");
            }
            finally
            {
                // Tear down
                PurgeLinkedData.DeleteSenNeedType(senCode);
            }
        }

        /// <summary>
        /// TC PU70
        /// Au : An Nguyen
        /// Description: Exercise ability to add a school level SEN Provision Type.
        /// Role: School Administrator
        /// </summary>
        [WebDriverTest(TimeoutSeconds = 600, Enabled = true, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { PupilTestGroups.SENRecord.Page, PupilTestGroups.Priority.Priority2 })]
        public void Add_a_school_level_SEN_Provision_Type()
        {
            try
            {
                SeleniumHelper.Login(LoginAs);
                AutomationSugar.NavigateMenu("Lookups", "Pupils", "SEN Provision Type");

                //Add new SEN Provision Type
                var senProvisionTypeTriplet = new SenProvisionTypeTriplet();
                var senProvisionTypePage = senProvisionTypeTriplet.AddSenProvisionType();
                var senProvisionTypeRow = senProvisionTypePage.SenProvisionTable.GetLastRow();
                senProvisionTypeRow.Code = "XXX";
                senProvisionTypeRow.Description = "Selenium Test Entry";
                senProvisionTypeRow.DisplayOrder = "999";
                senProvisionTypeRow.IsVisible = true;

                //Save SEN Provision Type
                senProvisionTypePage.ClickSave();

                //Verify success message
                Assert.AreEqual(true, senProvisionTypePage.IsSuccessMessageDisplayed(), "Success message do not display");
            }
            finally
            {
                // Tear down
                PurgeLinkedData.DeleteSenProvisionType("XXX");
            }
        }

        /// <summary>
        /// TC PU71
        /// Au : An Nguyen
        /// Description: Exercise ability to add a school level SEN Status.
        /// Role: School Administrator
        /// </summary>
        [WebDriverTest(TimeoutSeconds = 600, Enabled = true, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { PupilTestGroups.SENRecord.Page, PupilTestGroups.Priority.Priority2 })]
        [Variant(Variant.AllEnglish | Variant.AllIndependant | Variant.AllWelsh | Variant.AllMultiphase)]
        public void Add_SEN_Status_NonNIVariant()
        {
            try
            {
                SeleniumHelper.Login(LoginAs);
                AutomationSugar.NavigateMenu("Lookups", "Pupils", "SEN Status");

                //Add new SEN Status
                var senStatusTriplet = new SenStatusTriplet();
                var senStatusPage = senStatusTriplet.AddSenStatus();
                var senStatusRow = senStatusPage.SenStatusTable.GetLastRow();
                senStatusRow.Code = "999";
                senStatusRow.Description = "Selenium Test Entry";
                senStatusRow.DisplayOrder = "999";
                senStatusRow.IsVisible = true;
                senStatusRow.Category = Queries.GetLookupDescription("SENStatus");

                //Save SEN Status
                senStatusPage.ClickSave();

                //Verify success message
                Assert.AreEqual(true, senStatusPage.IsSuccessMessageDisplayed(), "Success message do not display");
            }
            finally
            {
                // Tear down
                PurgeLinkedData.DeleteSenStatus("999");
            }
        }

        /// <summary>
        /// TC PU71
        /// Au : An Nguyen
        /// Description: Exercise ability to add a school level SEN Status.
        /// Role: School Administrator
        /// </summary>
        [WebDriverTest(TimeoutSeconds = 600, Enabled = true, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { PupilTestGroups.SENRecord.Page, PupilTestGroups.Priority.Priority2 })]
        [Variant(Variant.AllNI)]
        public void Add_SEN_Stage_NIVariant()
        {
            try
            {
                SeleniumHelper.Login(LoginAs);
                AutomationSugar.NavigateMenu("Lookups", "Pupils", "SEN Stage");

                //Add new SEN Status
                var senStatusTriplet = new SenStatusTriplet();
                var senStatusPage = senStatusTriplet.AddSenStatus();
                var senStatusRow = senStatusPage.SenStatusTable.GetLastRow();
                senStatusRow.Code = "999";
                senStatusRow.Description = "Selenium Test Entry";
                senStatusRow.DisplayOrder = "999";
                senStatusRow.IsVisible = true;
                senStatusRow.Category = Queries.GetLookupDescriptionByCode("SENStatus", "0", isVisibleOnly: true);

                //Save SEN Status
                senStatusPage.ClickSave();

                //Verify success message
                Assert.AreEqual(true, senStatusPage.IsSuccessMessageDisplayed(), "Success message do not display");
            }
            finally
            {
                // Tear down
                PurgeLinkedData.DeleteSenStatus("999");
            }
        }

        /// <summary>
        /// Author: Ba.Truong
        /// Description: PU72: Check exercise ability to create a new SEN Record for a pupil, based on use of the newly added 'SEN Lookup' Records.
        /// </summary>
        [WebDriverTest(TimeoutSeconds = 2000, Enabled = true, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { PupilTestGroups.SENRecord.Page, PupilTestGroups.Priority.Priority2 })]
        [Variant(Variant.AllNI)]
        public void Create_New_SEN_Record_For_Pupil_Via_SenRecords_Menu_Route()
        {
            //Arrange
            var learnerId = Guid.NewGuid();
            var senDataPackage = this.BuildDataPackage();

            //Add random string surname
            var surname = "10-SEN" + SeleniumHelper.GenerateRandomString(6);

            senDataPackage.AddBasicLearner(learnerId, surname, "SEN TEST", dateOfBirth: new DateTime(2005, 05, 30),
                dateOfAdmission: new DateTime(2012, 10, 03));

            //Act
            using (new DataSetup(purgeBeforeInsert: false, purgeAfterTest: true, packages: senDataPackage))
            {
                try
                {
                    SeleniumHelper.Login(LoginAs);
                    AutomationSugar.NavigateMenu("Tasks", "Pupils", "SEN Records");

                    var SENRecordTriplet = new SenRecordTriplet();

                    // Search criteria: only pupils with NO SEN assigned
                    SENRecordTriplet.SearchCriteria.NoSenStageAssigned = true;
                    var searchResults = SENRecordTriplet.SearchCriteria.Search();
                    var pupilSearchTile = searchResults.Single(t => t.Name.Equals(surname + ", SEN TEST"));
                    var SenRecordDetail = pupilSearchTile.Click<SenRecordDetailPage>();

                    // click the Add button for SEN Stage add_sen_stage_button
                    SeleniumHelper.Get(By.CssSelector(SeleniumHelper.AutomationId("add_sen_stage_button"))).Click();
                    Thread.Sleep(1000);
                    var senStage = SenRecordDetail.SenStages.Rows[0];

                    //Add "SEN Stage" (date is automatically added)
                    senStage.Stage = Queries.GetLookupDescriptionByCode("SENStatus", "1");

                    //Save
                    SenRecordDetail.Save();

                    Assert.AreEqual(true, SenRecordDetail.IsMessageSuccessAppear(), "Success message does not appear");
                }
                finally
                {
                    // Tear down linked records before clean up
                    PurgeLinkedData.DeleteSENStageForLearner(learnerId);
                }
            }
        }

        /// <summary>
        /// Author: Ba.Truong
        /// Description: PU72: Check exercise ability to create a new SEN Record for a pupil, based on use of the newly added 'SEN Lookup' Records.
        /// </summary>
        [WebDriverTest(TimeoutSeconds = 2000, Enabled = true, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { PupilTestGroups.SENRecord.Page, PupilTestGroups.Priority.Priority2 })]
        [Variant(Variant.AllEnglish | Variant.AllIndependant | Variant.AllWelsh | Variant.AllMultiphase)]
        public void Create_New_SEN_Record_For_Pupil_Via_SenRecords_Menu_Route_NonNIVariant()
        {
            //Arrange
            var learnerId = Guid.NewGuid();
            var senDataPackage = this.BuildDataPackage();

            //Add random string surname
            var surname = "10-SEN" + SeleniumHelper.GenerateRandomString(6);

            senDataPackage.AddBasicLearner(learnerId, surname, "SEN TEST", dateOfBirth: new DateTime(2005, 05, 30),
                dateOfAdmission: new DateTime(2012, 10, 03));

            //Act
            using (new DataSetup(purgeBeforeInsert: false, purgeAfterTest: true, packages: senDataPackage))
            {
                try
                {
                    SeleniumHelper.Login(LoginAs);
                    AutomationSugar.NavigateMenu("Tasks", "Pupils", "SEN Records");

                    var SENRecordTriplet = new SenRecordTriplet();

                    // Search criteria: only pupils with NO SEN assigned
                    SENRecordTriplet.SearchCriteria.NoSenStageAssigned = true;
                    var searchResults = SENRecordTriplet.SearchCriteria.Search();
                    var pupilSearchTile = searchResults.Single(t => t.Name.Equals(surname + ", SEN TEST"));
                    var SenRecordDetail = pupilSearchTile.Click<SenRecordDetailPage>();

                    // click the Add button for SEN Stage add_sen_stage_button
                    SeleniumHelper.Get(By.CssSelector(SeleniumHelper.AutomationId("add_sen_status_button"))).Click();
                    Thread.Sleep(1000);
                    var senStage = SenRecordDetail.SenStages.Rows[0];

                    //Add "SEN Stage" (date is automatically added)
                    senStage.Stage = Queries.GetLookupDescriptionByCode("SENStatus", "1");

                    //Save
                    SenRecordDetail.Save();

                    Assert.AreEqual(true, SenRecordDetail.IsMessageSuccessAppear(), "Success message does not appear");
                }
                finally
                {
                    // Tear down linked records before clean up
                    PurgeLinkedData.DeleteSENStageForLearner(learnerId);
                }
            }
        }

        /// <summary>
        /// Author: Ba.Truong
        /// Description: PU73: Check exercise ability to view a pupil's SEN Details as created by a SEN Coordinator
        /// </summary>
        [WebDriverTest(TimeoutSeconds = 2000, Enabled = true, Browsers = new[] { BrowserDefaults.Chrome },
            Groups = new[] { PupilTestGroups.SENRecord.Page, PupilTestGroups.Priority.Priority2, "PSR_VPSDAOUT" })]
        public void View_Pupil_SEN_Details_As_Other_User_Type()
        {
            //Arrange
            var learnerId = Guid.NewGuid();
            var senDataPackage = this.BuildDataPackage();

            //Add random string surname
            var surname = "11-SEN_" + SeleniumHelper.GenerateRandomString(6);

            senDataPackage.AddBasicLearner(learnerId, surname, "SEN TEST", dateOfBirth: new DateTime(2005, 05, 05),
                dateOfAdmission: new DateTime(2012, 06, 06));

            senDataPackage.AddSENSStagetoLearner(learnerId, senStage: "1"); // Identify need

            //Act
            using (new DataSetup(purgeBeforeInsert: false, purgeAfterTest: true, packages: senDataPackage))
            {
                try
                {
                    SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.ClassTeacher);
                    AutomationSugar.NavigateMenu("Tasks", "Pupils", "Pupil Records");
                    SearchCriteria.Search();
                    SearchResults.WaitForResults();
                    SearchResults.Click(learnerId.ToString());

                    var pupilRecordTriplet = new PupilSearchTriplet();
                    var searchResults = pupilRecordTriplet.SearchCriteria.Search();
                    var pupilSearchTile = searchResults.Single(t => t.Name.Equals(surname + ", SEN TEST"));
                    var pupilRecord = pupilSearchTile.Click<PupilRecordPage>();

                    //Navigate to and expand the 'Statutory SEN' section.
                    AutomationSugar.ExpandAccordionPanel("section_menu_Statutory SEN");

                    //Validate data on SEN Stage Table
                    Assert.AreEqual(Queries.GetLookupDescriptionByCode("SenStatus", "1"), pupilRecord.SenStages[0].Stage);
                    Assert.AreEqual("5/5/2015", pupilRecord.SenStages[0].StartDay);

                    // close pupil, test complete
                    SeleniumHelper.CloseTab("Pupil Record");
                }
                finally
                {
                    // Tear down linked records before clean up
                    PurgeLinkedData.DeleteSENStageForLearner(learnerId);
                }
            }
        }
    }
}