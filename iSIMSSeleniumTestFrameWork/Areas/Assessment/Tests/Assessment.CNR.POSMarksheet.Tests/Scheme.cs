using TestSettings;
using WebDriverRunner.webdriver;
using Assessment.Components.Common;
using SharedComponents.BaseFolder;
using System;
using OpenQA.Selenium.Support.UI;
using SeSugar.Automation;
using Selene.Support.Attributes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Assessment.Components.PageObject;
using System.Collections.Generic;
using System.Threading;
using OpenQA.Selenium;
using POM.Components.HomePages;
using SharedComponents.Helpers;

namespace Assessment.CNR.Other.Assessment.Screens.Tests
{
    [TestClass]
    public class Scheme : BaseSeleniumComponents
    {

        #region MS Unit Testing support
        public TestContext TestContext { get; set; }
        [TestInitialize]
        public void Init()
        {
            TestRunner.VSSeleniumTest.Init(this, TestContext);
        }
        [TestCleanup]
        public void Cleanup()
        {
            TestRunner.VSSeleniumTest.Cleanup(TestContext);
        }
        #endregion

        [TestMethod]
        [WebDriverTest(Enabled = true, Groups = new[] { "NavigateToScheme", "Scheme", "P1" }, Browsers = new[] { BrowserDefaults.Chrome })]
        [Variant(Variant.EnglishStatePrimary | Variant.WelshStatePrimary)]
        public void NavigateToScheme()
        {
            WebDriverWait waiter = new WebDriverWait(WebContext.WebDriver, TimeSpan.FromSeconds(MarksheetConstants.Timeout));
            String[] featureList = { "Curriculum" };
            FeatureBee.FeatureBeeLogin.LoginWithFeatureBee(featureList, FeatureBee.FeatureBeeLogin.iSIMSUserType.AssessmentCoordinator);
            AutomationSugar.NavigateMenu("Tasks", "Assessment", "Manage Curriculum Schemes");
        }

        public void NavigateToModifyExistingScheme()
        {
            WebDriverWait waiter = new WebDriverWait(WebContext.WebDriver, TimeSpan.FromSeconds(MarksheetConstants.Timeout));
            String[] featureList = { "Curriculum" };
            FeatureBee.FeatureBeeLogin.LoginWithFeatureBee(featureList, FeatureBee.FeatureBeeLogin.iSIMSUserType.AssessmentCoordinator);
            AutomationSugar.NavigateMenu("Tasks", "Assessment", "Manage Curriculum Schemes");
            SchemeSearchPanel schemeSearchPanel = new SchemeSearchPanel();
            schemeSearchPanel.NavigateToModifyExistingScheme();
        }

        public void NavigateToNewFromExistingScheme()
        {
            WebDriverWait waiter = new WebDriverWait(WebContext.WebDriver, TimeSpan.FromSeconds(MarksheetConstants.Timeout));
            String[] featureList = { "Curriculum" };
            FeatureBee.FeatureBeeLogin.LoginWithFeatureBee(featureList, FeatureBee.FeatureBeeLogin.iSIMSUserType.AssessmentCoordinator);
            AutomationSugar.NavigateMenu("Tasks", "Assessment", "Manage Curriculum Schemes");
            SchemeSearchPanel schemeSearchPanel = new SchemeSearchPanel();
            schemeSearchPanel.NavigateToNewFromExistingScheme();
        }

        [TestMethod]
        [WebDriverTest(Enabled = true, Groups = new[] { "CreateScheme", "Scheme", "P2" }, Browsers = new[] { BrowserDefaults.Chrome })]
        [Variant(Variant.EnglishStatePrimary | Variant.WelshStatePrimary)]
        public void CreateScheme()
        {
            AddNewScheme();
            // following fuction will add Subject, Strand , Year and Statement.
            AddStatement();
        }

        [TestMethod]
        [WebDriverTest(Enabled = true, Groups = new[] { "GenerateTemplateFromScheme", "Scheme" }, Browsers = new[] { BrowserDefaults.Chrome })]
        [Variant(Variant.EnglishStatePrimary | Variant.WelshStatePrimary)]
        public void GenerateTemplateFromScheme()
        {
            NavigateToCreateScheme();
            SchemeSearchPanel schemeSearchPanel = new SchemeSearchPanel();
            string schemeName = SchemeSearchPanel.GenerateRandomString(10);
            string schemeDescription = SchemeSearchPanel.GenerateRandomString(20);
            string shortName = SchemeSearchPanel.GenerateRandomString(10);
            schemeSearchPanel.SetNameAndDescription(schemeName, schemeDescription, shortName);
            schemeSearchPanel = schemeSearchPanel.ClickOKButton();

            AddStatement();

            schemeSearchPanel.ClickSaveButton();
            schemeSearchPanel.waitforSavemessagetoAppear();
            schemeSearchPanel.SaveMessageAssertionSuccess();

            schemeSearchPanel = schemeSearchPanel.ClickCreateSubjectButton();
            schemeSearchPanel = schemeSearchPanel.ClickGenerateTemplateLink();
            schemeSearchPanel.waitforSavemessagetoAppear();
            schemeSearchPanel.GenerateTemplateMessageAssertionSuccess();
            schemeSearchPanel.OpenPosTemplate();
        }

        /// <summary>
        /// Test to demonstrate that delete Scheme
        /// Story 27743-Assessment - Scheme - Filter criteria for scheme selection
        /// </summary>
        [TestMethod]
        [WebDriverTest(Enabled = true, Groups = new[] { "DeleteScheme", "Scheme", "P2" }, Browsers = new[] { BrowserDefaults.Chrome })]
        [Variant(Variant.EnglishStatePrimary | Variant.WelshStatePrimary)]
        public void DeleteScheme()
        {
            NavigateToCreateScheme();
            SchemeSearchPanel schemeSearchPanel = new SchemeSearchPanel();
            string schemeName = SchemeSearchPanel.GenerateRandomString(10);
            string schemeDescription = SchemeSearchPanel.GenerateRandomString(20);
            string shortName = SchemeSearchPanel.GenerateRandomString(10);
            schemeSearchPanel.SetNameAndDescription(schemeName, schemeDescription, shortName);
            schemeSearchPanel = schemeSearchPanel.ClickOKButton();

            AddStatement();

            schemeSearchPanel.ClickSaveButton();
            schemeSearchPanel.waitforSavemessagetoAppear();
            schemeSearchPanel.SaveMessageAssertionSuccess();

            schemeSearchPanel = schemeSearchPanel.ClickDeleteSchemeButton();
            schemeSearchPanel = schemeSearchPanel.ClickDeleteOKButton();
            schemeSearchPanel.DeleteMessageAssertionSuccess(schemeName);
        }


        [TestMethod]
        [WebDriverTest(Enabled = true, Groups = new[] { "AddScheme", "Scheme" }, Browsers = new[] { BrowserDefaults.Chrome })]
        [Variant(Variant.EnglishStatePrimary | Variant.WelshStatePrimary)]
        public void AddNewScheme()
        {
            NavigateToCreateScheme();
            SchemeSearchPanel schemeSearchPanel = new SchemeSearchPanel();
            string schemeName = SchemeSearchPanel.GenerateRandomString(10);
            string schemeDescription = SchemeSearchPanel.GenerateRandomString(20);
            string shortName = SchemeSearchPanel.GenerateRandomString(10);
            schemeSearchPanel.SetNameAndDescription(schemeName, schemeDescription, shortName);
            schemeSearchPanel = schemeSearchPanel.ClickOKButton();
        }

        public void ClickOkButton()
        {
            IWebElement _element = WebContext.WebDriver.FindElement(By.CssSelector("button[title='OK']"));
            _element.Click();
        }

        /// <summary>
        /// Navigation to create scheme quick link
        /// </summary>
        public void NavigateToCreateScheme()
        {
            WebDriverWait waiter = new WebDriverWait(WebContext.WebDriver, TimeSpan.FromSeconds(MarksheetConstants.Timeout));
            String[] featureList = { "Curriculum" };
            FeatureBee.FeatureBeeLogin.LoginWithFeatureBee(featureList, FeatureBee.FeatureBeeLogin.iSIMSUserType.AssessmentCoordinator);
            AutomationSugar.NavigateMenu("Tasks", "Assessment", "Manage Curriculum Schemes");
            SchemeSearchPanel schemeSearchPanel = new SchemeSearchPanel();
            schemeSearchPanel.NavigateToCreateNewScheme();
            //var assessmentQuickLinks = new AssessmentQuickLinks();
            //// Open Assessment Quick Links Dropdown for assessment coordinator
            //assessmentQuickLinks.OpenAssessmentQuickLinksDropdown(SeleniumHelper.iSIMSUserType.AssessmentCoordinator);

            //// Wait for the ajax request to complete
            //WaitUntillAjaxRequestCompleted();

            //SeleniumHelper.WaitForElementClickableThenClick(SeleniumHelper.SelectByDataAutomationID("quick-link-create-scheme"));
        }

        /// <summary>
        /// Test to demonstrate that apply filter
        /// Story 27743-Assessment - Scheme - Filter criteria for scheme selection
        /// </summary>
        [TestMethod]
        [WebDriverTest(Enabled = true, Groups = new[] { "SchemeApplyFilter", "Scheme", "P2" }, Browsers = new[] { BrowserDefaults.Chrome })]
        [Variant(Variant.EnglishStatePrimary)]
        public void SchemeApplyFilter()
        {
            NavigateToModifyExistingScheme();
            SchemeSearchPanel schemeSearchPanel = new SchemeSearchPanel();
            //Open the Filter tab
            schemeSearchPanel.OpenFilter();
            //Select a Group
            schemeSearchPanel = schemeSearchPanel.SelectGroup("Year 1");
            //Select a Subject
            schemeSearchPanel = schemeSearchPanel.SelectSubject("English: Reading");
            //Select a Strand
            schemeSearchPanel = schemeSearchPanel.SelectStrand("Comprehension");

            schemeSearchPanel.Search();
            List<string> SchemeData = schemeSearchPanel.GetDataInTab();

            //        String Columnname = MarksheetGridHelper.GetColumnName("En Read Comp Overall");
            Assert.IsTrue(SchemeData.Contains("Year 1"));
            Assert.IsTrue(SchemeData.Contains("English: Reading"));
            Assert.IsTrue(SchemeData.Contains("Comprehension"));
        }

        // Commented following test as Statement View has been moved to Right hand side and Implement and uncomment it later.
        /// <summary>
        /// Test to demonstrate that apply filter
        /// Story 28460-Assessment - Scheme - Display Existing scheme based on filter criteria
        /// </summary>
        ///
        [TestMethod]
        [WebDriverTest(Enabled = true, Groups = new[] { "DisplayExistingScheme", "Scheme", "P2" }, Browsers = new[] { BrowserDefaults.Chrome })]
        [Variant(Variant.EnglishStatePrimary)]
        public void DisplayExistingScheme()
        {
            NavigateToModifyExistingScheme();
            SchemeSearchPanel schemeSearchPanel = new SchemeSearchPanel();
            //Open the Filter tab
            schemeSearchPanel.OpenFilter();
            //Select a Group
            schemeSearchPanel = schemeSearchPanel.SelectGroup("Year 1");
            //Select a Subject
            schemeSearchPanel = schemeSearchPanel.SelectSubject("English: Reading");
            //Select a Strand
            schemeSearchPanel = schemeSearchPanel.SelectStrand("Comprehension");

            schemeSearchPanel.Search();


            List<string> SchemeData = schemeSearchPanel.GetTreeData();

            //        String Columnname = MarksheetGridHelper.GetColumnName("En Read Comp Overall");
            Assert.IsTrue(SchemeData.Contains("Year 1"));
            Assert.IsTrue(SchemeData.Contains("English: Reading"));
            Assert.IsTrue(SchemeData.Contains("Comprehension"));

        }

        // Commented following test as Statement View has been moved to Right hand side and Implement and uncomment it later.
        /// <summary>
        /// Story 28472 : Assessment - Scheme - Add Subject, Year 1, Strand and statements  from existing resources
        /// </summary>
        [TestMethod]
        [WebDriverTest(Enabled = true, Groups = new[] { "AddExistingResourcesToScheme", "Scheme", "P2" }, Browsers = new[] { BrowserDefaults.Chrome })]
        [Variant(Variant.EnglishStatePrimary)]
        public void AddExistingResourcesToScheme()
        {
            NavigateToModifyExistingScheme();
            SchemeSearchPanel schemeSearchPanel = new SchemeSearchPanel();
            //Open the Filter tab
            schemeSearchPanel.OpenFilter();
            //Select a Group
            schemeSearchPanel = schemeSearchPanel.SelectGroup("Year 1");
            //Select a Subject
            schemeSearchPanel = schemeSearchPanel.SelectSubject("English: Reading");
            //Select a Strand
            schemeSearchPanel = schemeSearchPanel.SelectStrand("Comprehension");

            schemeSearchPanel.Search();
            List<string> SchemeData = schemeSearchPanel.GetTreeData();

            Assert.IsTrue(SchemeData.Contains("Year 1"));
            Assert.IsTrue(SchemeData.Contains("English: Reading"));
            Assert.IsTrue(SchemeData.Contains("Comprehension"));
            //Add existing subjects
            schemeSearchPanel.ClickAddSubjectButton();

            schemeSearchPanel.ClickAddExistingSubjectButton();

            schemeSearchPanel = schemeSearchPanel.SelectPhase("Year 1");

            schemeSearchPanel = schemeSearchPanel.ClickSearchSubjectButton();

            List<string> subjects = TestData.CreateDataList("select [AssessmentSubject].[Name] as Name from [AssessmentSubject] inner join [SubjectLevel] on [AssessmentSubject].[ID] = [SubjectLevel].[AssessmentSubject] inner join [LearningLevel] on [SubjectLevel].[LearningLevel] = [LearningLevel].[ID] where [LearningLevel].[Name]='Year 1' AND [AssessmentSubject].[TenantID]=" + TestDefaults.Default.TenantId, "Name");
            List<string> subjectSearchResults = schemeSearchPanel.GetExistingData();

            Assert.AreEqual(subjects.Count.ToString(), subjectSearchResults.Count.ToString());

            foreach (string eachelement in subjects)
            {
                Assert.IsTrue(subjectSearchResults.Contains(eachelement.Replace(" ", string.Empty).ToLower()));
            }
            //Add existing levels
            schemeSearchPanel.ClickAddLevelButton();

            schemeSearchPanel.ClickAddExistingLevelButton();
            List<string> levels = TestData.CreateDataList("SELECT distinct [LearningLevel].[Name] [LearningLevel.Name] FROM dbo.[LearningLevel] [LearningLevel] inner JOIN dbo.[NCYearLearningLevel] [LearningLevel.NCYearLearningLevels] ON ([LearningLevel].[ID] = [LearningLevel.NCYearLearningLevels].[LearningLevel] AND [LearningLevel.NCYearLearningLevels].[TenantID] =" + TestDefaults.Default.TenantId + " ) WHERE ([LearningLevel].[TenantID] = " + TestDefaults.Default.TenantId + ")Group by [LearningLevel].[ID],[LearningLevel.NCYearLearningLevels].[LearningLevel],[LearningLevel].[Name] having count([LearningLevel.NCYearLearningLevels].[LearningLevel])=1", "LearningLevel.Name");
            List<string> levelSearchResults = schemeSearchPanel.GetExistingData();

            Assert.AreEqual(levels.Count.ToString(), levelSearchResults.Count.ToString());

            foreach (string eachelement in levels)
            {
                Assert.IsTrue(levelSearchResults.Contains(eachelement.Replace(" ", string.Empty).ToLower()));
            }

            //Add existing Strands
            schemeSearchPanel.ClickAddStrandButton();
            schemeSearchPanel.ClickAddExistingStrandButton();
            List<string> strands = TestData.CreateDataList("select Name from Strand where id in(select Strand from SubjectLevelStatement where SubjectLevel in(select id from SubjectLevel where AssessmentSubject in (select id from AssessmentSubject where name ='English: Reading') and learningLevel in(select id from learningLevel where name ='Year 1'))) and TenantID=" + TestDefaults.Default.TenantId, "Name");
            List<string> strandSearchResults = schemeSearchPanel.GetExistingData();

            Assert.AreEqual(strands.Count.ToString(), strandSearchResults.Count.ToString());

            foreach (string eachelement in strands)
            {
                Assert.IsTrue(strandSearchResults.Contains(eachelement.Replace(" ", string.Empty).ToLower()));
            }

            //Add existing statement
            schemeSearchPanel = schemeSearchPanel.CancelPickerFormButton();
            schemeSearchPanel = schemeSearchPanel.ClickExistingLevelNode();
            schemeSearchPanel.ClickAddStatementButton();
            schemeSearchPanel.ClickAddExistingStatementButton();
            List<string> statements = TestData.CreateDataList("select name from Statement where id in(select Statement from SubjectLevelStatement where SubjectLevel in(select id from SubjectLevel where AssessmentSubject in (select id from AssessmentSubject where name ='English: Reading') and learningLevel in(select id from learningLevel where name ='Year 1') and Strand in(select id from Strand where name ='Comprehension'))) and TenantID=" + TestDefaults.Default.TenantId, "Name");
            List<string> statementSearchResults = schemeSearchPanel.GetExistingData();

            Assert.AreEqual(statements.Count.ToString(), statementSearchResults.Count.ToString());
        }

        /// <summary>
        /// Test to add, edit and delete new subject strand and statement.
        /// Story 28466-Assessment - Add , Edit and Delete Subject, Strand and Statement
        /// </summary>
        [TestMethod]
        [WebDriverTest(Enabled = true, Groups = new[] { "AddNewSubjectStrandStatement", "Scheme", "AddEditDeleteSubjectStrandStatement", "P2" }, Browsers = new[] { BrowserDefaults.Chrome })]
        [Variant(Variant.EnglishStatePrimary)]
        public void AddNewSubjectStrandStatement()
        {
            NavigateToModifyExistingScheme();
            SchemeSearchPanel schemeSearchPanel = new SchemeSearchPanel();
            // create Subject
            schemeSearchPanel = schemeSearchPanel.ClickCreateSubjectButton();
            schemeSearchPanel = schemeSearchPanel.ClickCreateNewSubjectLink();
            string SubjectName = SchemeSearchPanel.GenerateRandomString(10);
            string SubjectDescription = SchemeSearchPanel.GenerateRandomString(20);
            string SubjectShortName = SchemeSearchPanel.GenerateRandomString(10);
            schemeSearchPanel.SetNameAndDescription(SubjectName, SubjectDescription, SubjectShortName);
            schemeSearchPanel = schemeSearchPanel.ClickOKButton();
            // create Strand
            schemeSearchPanel = schemeSearchPanel.ClickCreateStrandButton();
            schemeSearchPanel = schemeSearchPanel.ClickCreateNewStrandLink();
            string StrandName = SchemeSearchPanel.GenerateRandomString(10);
            string StrandDescription = SchemeSearchPanel.GenerateRandomString(20);
            string StrandShortName = SchemeSearchPanel.GenerateRandomString(20);
            schemeSearchPanel.SetNameAndDescription(StrandName, StrandDescription, StrandShortName);
            schemeSearchPanel = schemeSearchPanel.ClickOKButton();

            // add Levels
            schemeSearchPanel = schemeSearchPanel.ClickAssignExistingLevelButton();
            schemeSearchPanel = schemeSearchPanel.ClickAddExistingLevelLink();
            schemeSearchPanel.SelectLevel(1);
            schemeSearchPanel.SelectLevel(2);
            schemeSearchPanel = schemeSearchPanel.AddExistingLevelOKButton();
            // create Statement
            schemeSearchPanel = schemeSearchPanel.ClickLevelNode();
            schemeSearchPanel = schemeSearchPanel.ClickCreateStatementButton();
            schemeSearchPanel = schemeSearchPanel.ClickCreateNewStatementLink();
            string StatementName = SchemeSearchPanel.GenerateRandomString(10);
            string StatementDescription = SchemeSearchPanel.GenerateRandomString(20);
            string StatementShortName = SchemeSearchPanel.GenerateRandomString(20);
            schemeSearchPanel.SetNameAndDescription(StatementName, StatementDescription, StatementShortName);
            schemeSearchPanel = schemeSearchPanel.ClickOKButton();

            // edit subject
            schemeSearchPanel = schemeSearchPanel.ClickEditSubjectButton();
            schemeSearchPanel.SetNameAndDescription(SubjectName + ". Edited", SubjectDescription + ". Edited", SubjectShortName + ". Edited");
            schemeSearchPanel = schemeSearchPanel.ClickOKButton();

            // edit strand
            schemeSearchPanel = schemeSearchPanel.ClickEditStrandButton();
            schemeSearchPanel.SetNameAndDescription(StrandName + ". Edited", StrandDescription + ". Edited", StrandShortName + ". Edited");
            schemeSearchPanel = schemeSearchPanel.ClickOKButton();

            // edit statement
            schemeSearchPanel = schemeSearchPanel.ClickEditStatementButton();
            schemeSearchPanel.SetNameAndDescription(StatementName + ". Edited", StatementDescription + ". Edited", StatementShortName + ". Edited");
            schemeSearchPanel = schemeSearchPanel.ClickOKButton();

            // delete statement
            schemeSearchPanel = schemeSearchPanel.ClickDeleteStatementButton();
            schemeSearchPanel = schemeSearchPanel.ClickDeleteOKButton();

            // delete year
            schemeSearchPanel = schemeSearchPanel.ClickDeleteYearButton();
            schemeSearchPanel = schemeSearchPanel.ClickDeleteOKButton();

            // delete strand
            schemeSearchPanel = schemeSearchPanel.ClickDeleteStrandButton();
            schemeSearchPanel = schemeSearchPanel.ClickDeleteOKButton();

            // delete subject
            schemeSearchPanel = schemeSearchPanel.ClickDeleteSubjectButton();
            schemeSearchPanel = schemeSearchPanel.ClickDeleteOKButton();
        }

        /// <summary>
        /// Test to add new subject
        /// Story 28466-Assessment - Add , Edit and Delete Subject, Strand and Statement
        /// </summary>
        [TestMethod]
        [WebDriverTest(Enabled = true, Groups = new[] { "AddNewSubject", "Scheme", "AddEditDeleteSubjectStrandStatement" }, Browsers = new[] { BrowserDefaults.Chrome })]
        [Variant(Variant.EnglishStatePrimary)]
        public void AddNewSubject()
        {
            NavigateToModifyExistingScheme();
            AddSubject();
        }

        private void AddSubject()
        {
            SchemeSearchPanel schemeSearchPanel = new SchemeSearchPanel();
            // create Subject
            schemeSearchPanel = schemeSearchPanel.ClickCreateSubjectButton();
            schemeSearchPanel = schemeSearchPanel.ClickCreateNewSubjectLink();
            string SubjectName = SchemeSearchPanel.GenerateRandomString(10);
            string SubjectDescription = SchemeSearchPanel.GenerateRandomString(20);
            string SubjectShortName = SchemeSearchPanel.GenerateRandomString(10);
            schemeSearchPanel.SetNameAndDescription(SubjectName, SubjectDescription, SubjectShortName);
            schemeSearchPanel = schemeSearchPanel.ClickOKButton();
        }

        /// <summary>
        /// Test to add strand.
        /// Story 28466-Assessment - Add , Edit and Delete Subject, Strand and Statement
        /// </summary>
        [TestMethod]
        [WebDriverTest(Enabled = true, Groups = new[] { "AddNewStrand", "Scheme", "AddEditDeleteSubjectStrandStatement" }, Browsers = new[] { BrowserDefaults.Chrome })]
        [Variant(Variant.EnglishStatePrimary)]
        public void AddNewStrand()
        {
            NavigateToModifyExistingScheme();
            AddStrand();
        }

        private void AddStrand()
        {
            SchemeSearchPanel schemeSearchPanel = new SchemeSearchPanel();
            // create Subject
            schemeSearchPanel = schemeSearchPanel.ClickCreateSubjectButton();
            schemeSearchPanel = schemeSearchPanel.ClickCreateNewSubjectLink();
            string SubjectName = SchemeSearchPanel.GenerateRandomString(10);
            string SubjectDescription = SchemeSearchPanel.GenerateRandomString(20);
            string SubjectShortName = SchemeSearchPanel.GenerateRandomString(10);
            schemeSearchPanel.SetNameAndDescription(SubjectName, SubjectDescription, SubjectShortName);
            schemeSearchPanel = schemeSearchPanel.ClickOKButton();
            // create Strand
            schemeSearchPanel = schemeSearchPanel.ClickCreateStrandButton();
            schemeSearchPanel = schemeSearchPanel.ClickCreateNewStrandLink();
            string StrandName = SchemeSearchPanel.GenerateRandomString(10);
            string StrandDescription = SchemeSearchPanel.GenerateRandomString(20);
            string StrandShortName = SchemeSearchPanel.GenerateRandomString(10);
            schemeSearchPanel.SetNameAndDescription(StrandName, StrandDescription, StrandShortName);
            schemeSearchPanel = schemeSearchPanel.ClickOKButton();
        }

        /// <summary>
        /// Test to add statement.
        /// Story 28466-Assessment - Add , Edit and Delete Subject, Strand and Statement
        /// </summary>
        [TestMethod]
        [WebDriverTest(Enabled = true, Groups = new[] { "AddNewStatement", "Scheme", "AddEditDeleteSubjectStrandStatement" }, Browsers = new[] { BrowserDefaults.Chrome })]
        [Variant(Variant.EnglishStatePrimary)]
        public void AddNewStatement()
        {
            NavigateToModifyExistingScheme();
            AddStatement();
        }

        private void AddStatement()
        {
            SchemeSearchPanel schemeSearchPanel = new SchemeSearchPanel();
            // create Subject
            schemeSearchPanel = schemeSearchPanel.ClickCreateSubjectButton();
            schemeSearchPanel = schemeSearchPanel.ClickCreateNewSubjectLink();
            string SubjectName = SchemeSearchPanel.GenerateRandomString(10);
            string SubjectDescription = SchemeSearchPanel.GenerateRandomString(20);
            string SubjectShortName = SchemeSearchPanel.GenerateRandomString(10);
            schemeSearchPanel.SetNameAndDescription(SubjectName, SubjectDescription, SubjectShortName);
            schemeSearchPanel = schemeSearchPanel.ClickOKButton();
            // create Strand
            schemeSearchPanel = schemeSearchPanel.ClickCreateStrandButton();
            schemeSearchPanel = schemeSearchPanel.ClickCreateNewStrandLink();
            string StrandName = SchemeSearchPanel.GenerateRandomString(10);
            string StrandDescription = SchemeSearchPanel.GenerateRandomString(20);
            string StrandShortName = SchemeSearchPanel.GenerateRandomString(10);
            schemeSearchPanel.SetNameAndDescription(StrandName, StrandDescription, StrandShortName);
            schemeSearchPanel = schemeSearchPanel.ClickOKButton();

            // add Levels
            schemeSearchPanel = schemeSearchPanel.ClickAssignExistingLevelButton();
            schemeSearchPanel = schemeSearchPanel.ClickAddExistingLevelLink();
            schemeSearchPanel.SelectLevel(1);
            //schemeSearchPanel.SelectLevel(2);
            schemeSearchPanel = schemeSearchPanel.AddExistingLevelOKButton();
            // create Statement
            schemeSearchPanel = schemeSearchPanel.ClickLevelNode();
            schemeSearchPanel = schemeSearchPanel.ClickCreateStatementButton();
            schemeSearchPanel = schemeSearchPanel.ClickCreateNewStatementLink();
            string StatementName = SchemeSearchPanel.GenerateRandomString(10);
            string StatementDescription = SchemeSearchPanel.GenerateRandomString(20);
            string StatementShortName = SchemeSearchPanel.GenerateRandomString(10);
            schemeSearchPanel.SetNameAndDescription(StatementName, StatementDescription, StatementShortName);
            schemeSearchPanel = schemeSearchPanel.ClickOKButton();
        }

        /// <summary>
        /// Test to edit subject.
        /// Story 28466-Assessment - Add , Edit and Delete Subject, Strand and Statement
        /// </summary>
        [TestMethod]
        [WebDriverTest(Enabled = true, Groups = new[] { "EditSubject", "Scheme", "AddEditDeleteSubjectStrandStatement" }, Browsers = new[] { BrowserDefaults.Chrome })]
        [Variant(Variant.EnglishStatePrimary)]
        public void EditSubject()
        {
            NavigateToModifyExistingScheme();
            SchemeSearchPanel schemeSearchPanel = new SchemeSearchPanel();
            // create Subject
            schemeSearchPanel = schemeSearchPanel.ClickCreateSubjectButton();
            schemeSearchPanel = schemeSearchPanel.ClickCreateNewSubjectLink();
            string SubjectName = SchemeSearchPanel.GenerateRandomString(10);
            string SubjectDescription = SchemeSearchPanel.GenerateRandomString(20);
            string SubjectShortName = SchemeSearchPanel.GenerateRandomString(10);
            schemeSearchPanel.SetNameAndDescription(SubjectName, SubjectDescription, SubjectShortName);
            schemeSearchPanel = schemeSearchPanel.ClickOKButton();

            // edit subject
            schemeSearchPanel = schemeSearchPanel.ClickEditSubjectButton();
            schemeSearchPanel.SetNameAndDescription(SubjectName + ". Edited", SubjectDescription + ". Edited", SubjectShortName + ". Edited");
            schemeSearchPanel = schemeSearchPanel.ClickOKButton();
        }

        /// <summary>
        /// Test to edit strand.
        /// Story 28466-Assessment - Add , Edit and Delete Subject, Strand and Statement
        /// </summary>
        [TestMethod]
        [WebDriverTest(Enabled = true, Groups = new[] { "EditStrand", "Scheme", "AddEditDeleteSubjectStrandStatement" }, Browsers = new[] { BrowserDefaults.Chrome })]
        [Variant(Variant.EnglishStatePrimary)]
        public void EditStrand()
        {
            NavigateToModifyExistingScheme();
            SchemeSearchPanel schemeSearchPanel = new SchemeSearchPanel();
            // create Subject
            schemeSearchPanel = schemeSearchPanel.ClickCreateSubjectButton();
            schemeSearchPanel = schemeSearchPanel.ClickCreateNewSubjectLink();
            string SubjectName = SchemeSearchPanel.GenerateRandomString(10);
            string SubjectDescription = SchemeSearchPanel.GenerateRandomString(20);
            string SubjectShortName = SchemeSearchPanel.GenerateRandomString(10);
            schemeSearchPanel.SetNameAndDescription(SubjectName, SubjectDescription, SubjectShortName);
            schemeSearchPanel = schemeSearchPanel.ClickOKButton();
            // create Strand
            schemeSearchPanel = schemeSearchPanel.ClickCreateStrandButton();
            schemeSearchPanel = schemeSearchPanel.ClickCreateNewStrandLink();
            string StrandName = SchemeSearchPanel.GenerateRandomString(10);
            string StrandDescription = SchemeSearchPanel.GenerateRandomString(20);
            string StrandShortName = SchemeSearchPanel.GenerateRandomString(10);
            schemeSearchPanel.SetNameAndDescription(StrandName, StrandDescription, StrandShortName);
            schemeSearchPanel = schemeSearchPanel.ClickOKButton();
            // edit strand
            schemeSearchPanel = schemeSearchPanel.ClickEditStrandButton();
            schemeSearchPanel.SetNameAndDescription(StrandName + ". Edited", StrandDescription + ". Edited", StrandShortName + ". Edited");
            schemeSearchPanel = schemeSearchPanel.ClickOKButton();
        }

        /// <summary>
        /// Test to edit statement.
        /// Story 28466-Assessment - Add , Edit and Delete Subject, Strand and Statement
        /// </summary>
        [TestMethod]
        [WebDriverTest(Enabled = true, Groups = new[] { "EditStatement", "Scheme", "AddEditDeleteSubjectStrandStatement" }, Browsers = new[] { BrowserDefaults.Chrome })]
        [Variant(Variant.EnglishStatePrimary)]
        public void EditStatement()
        {
            NavigateToModifyExistingScheme();
            SchemeSearchPanel schemeSearchPanel = new SchemeSearchPanel();
            // create Subject
            schemeSearchPanel = schemeSearchPanel.ClickCreateSubjectButton();
            schemeSearchPanel = schemeSearchPanel.ClickCreateNewSubjectLink();
            string SubjectName = SchemeSearchPanel.GenerateRandomString(10);
            string SubjectDescription = SchemeSearchPanel.GenerateRandomString(20);
            string SubjectShortName = SchemeSearchPanel.GenerateRandomString(10);
            schemeSearchPanel.SetNameAndDescription(SubjectName, SubjectDescription, SubjectShortName);
            schemeSearchPanel = schemeSearchPanel.ClickOKButton();
            // create Strand
            schemeSearchPanel = schemeSearchPanel.ClickCreateStrandButton();
            schemeSearchPanel = schemeSearchPanel.ClickCreateNewStrandLink();
            string StrandName = SchemeSearchPanel.GenerateRandomString(10);
            string StrandDescription = SchemeSearchPanel.GenerateRandomString(20);
            string StrandShortName = SchemeSearchPanel.GenerateRandomString(10);
            schemeSearchPanel.SetNameAndDescription(StrandName, StrandDescription, StrandShortName);
            schemeSearchPanel = schemeSearchPanel.ClickOKButton();

            //add Levels
            schemeSearchPanel = schemeSearchPanel.ClickAssignExistingLevelButton();
            schemeSearchPanel = schemeSearchPanel.ClickAddExistingLevelLink();
            schemeSearchPanel.SelectLevel(1);
            schemeSearchPanel.SelectLevel(2);
            schemeSearchPanel = schemeSearchPanel.AddExistingLevelOKButton();
            //create Statement
            schemeSearchPanel = schemeSearchPanel.ClickLevelNode();
            schemeSearchPanel = schemeSearchPanel.ClickCreateStatementButton();
            schemeSearchPanel = schemeSearchPanel.ClickCreateNewStatementLink();
            string StatementName = SchemeSearchPanel.GenerateRandomString(10);
            string StatementDescription = SchemeSearchPanel.GenerateRandomString(20);
            string StatementShortName = SchemeSearchPanel.GenerateRandomString(10);
            schemeSearchPanel.SetNameAndDescription(StatementName, StatementDescription, StatementShortName);
            schemeSearchPanel = schemeSearchPanel.ClickOKButton();

            // edit statement
            schemeSearchPanel = schemeSearchPanel.ClickEditStatementButton();
            schemeSearchPanel.SetNameAndDescription(StatementName + ". Edited", StatementDescription + ". Edited", StatementShortName + ". Edited");
            schemeSearchPanel = schemeSearchPanel.ClickOKButton();
        }

        /// <summary>
        /// Test to delete subject .
        /// Story 28466-Assessment - Add , Edit and Delete Subject, Strand and Statement
        /// </summary>
        [TestMethod]
        [WebDriverTest(Enabled = true, Groups = new[] { "DeleteSubject", "Scheme", "AddEditDeleteSubjectStrandStatement" }, Browsers = new[] { BrowserDefaults.Chrome })]
        [Variant(Variant.EnglishStatePrimary)]
        public void DeleteSubject()
        {
            NavigateToModifyExistingScheme();
            SchemeSearchPanel schemeSearchPanel = new SchemeSearchPanel();
            // create Subject
            schemeSearchPanel = schemeSearchPanel.ClickCreateSubjectButton();
            schemeSearchPanel = schemeSearchPanel.ClickCreateNewSubjectLink();
            string SubjectName = SchemeSearchPanel.GenerateRandomString(10);
            string SubjectDescription = SchemeSearchPanel.GenerateRandomString(20);
            string SubjectShortName = SchemeSearchPanel.GenerateRandomString(10);
            schemeSearchPanel.SetNameAndDescription(SubjectName, SubjectDescription, SubjectShortName);
            schemeSearchPanel = schemeSearchPanel.ClickOKButton();

            // delete subject
            schemeSearchPanel = schemeSearchPanel.ClickDeleteSubjectButton();
            schemeSearchPanel = schemeSearchPanel.ClickDeleteOKButton();
            Thread.Sleep(1000);
        }

        /// <summary>
        /// Test to delete strand .
        /// Story 28466-Assessment - Add , Edit and Delete Subject, Strand and Statement
        /// </summary>
        [TestMethod]
        [WebDriverTest(Enabled = true, Groups = new[] { "DeleteStrand", "Scheme", "AddEditDeleteSubjectStrandStatement" }, Browsers = new[] { BrowserDefaults.Chrome })]
        [Variant(Variant.EnglishStatePrimary)]
        public void DeleteStrand()
        {
            NavigateToModifyExistingScheme();
            SchemeSearchPanel schemeSearchPanel = new SchemeSearchPanel();
            // create Subject
            schemeSearchPanel = schemeSearchPanel.ClickCreateSubjectButton();
            schemeSearchPanel = schemeSearchPanel.ClickCreateNewSubjectLink();
            string SubjectName = SchemeSearchPanel.GenerateRandomString(10);
            string SubjectDescription = SchemeSearchPanel.GenerateRandomString(20);
            string SubjectShortName = SchemeSearchPanel.GenerateRandomString(10);
            schemeSearchPanel.SetNameAndDescription(SubjectName, SubjectDescription, SubjectShortName);
            schemeSearchPanel = schemeSearchPanel.ClickOKButton();
            // create Strand
            schemeSearchPanel = schemeSearchPanel.ClickCreateStrandButton();
            schemeSearchPanel = schemeSearchPanel.ClickCreateNewStrandLink();
            string StrandName = SchemeSearchPanel.GenerateRandomString(10);
            string StrandDescription = SchemeSearchPanel.GenerateRandomString(20);
            string StrandShortName = SchemeSearchPanel.GenerateRandomString(10);
            schemeSearchPanel.SetNameAndDescription(StrandName, StrandDescription, StrandShortName);
            schemeSearchPanel = schemeSearchPanel.ClickOKButton();

            // delete strand
            schemeSearchPanel = schemeSearchPanel.ClickDeleteStrandButton();
            schemeSearchPanel = schemeSearchPanel.ClickDeleteOKButton();
            Thread.Sleep(1000);
        }

        /// <summary>
        /// Test to add, edit and delete new subject strand and statement.
        /// Story 28466-Assessment - Add , Edit and Delete Subject, Strand and Statement
        /// </summary>
        [TestMethod]
        [WebDriverTest(Enabled = true, Groups = new[] { "DeleteStatement", "Scheme", "AddEditDeleteSubjectStrandStatement" }, Browsers = new[] { BrowserDefaults.Chrome })]
        [Variant(Variant.EnglishStatePrimary)]
        public void DeleteStatement()
        {
            NavigateToModifyExistingScheme();
            SchemeSearchPanel schemeSearchPanel = new SchemeSearchPanel();
            // create Subject
            schemeSearchPanel = schemeSearchPanel.ClickCreateSubjectButton();
            schemeSearchPanel = schemeSearchPanel.ClickCreateNewSubjectLink();
            string SubjectName = SchemeSearchPanel.GenerateRandomString(10);
            string SubjectDescription = SchemeSearchPanel.GenerateRandomString(20);
            string SubjectShortName = SchemeSearchPanel.GenerateRandomString(10);
            schemeSearchPanel.SetNameAndDescription(SubjectName, SubjectDescription, SubjectShortName);
            schemeSearchPanel = schemeSearchPanel.ClickOKButton();
            // create Strand
            schemeSearchPanel = schemeSearchPanel.ClickCreateStrandButton();
            schemeSearchPanel = schemeSearchPanel.ClickCreateNewStrandLink();
            string StrandName = SchemeSearchPanel.GenerateRandomString(10);
            string StrandDescription = SchemeSearchPanel.GenerateRandomString(20);
            string StrandShortName = SchemeSearchPanel.GenerateRandomString(10);
            schemeSearchPanel.SetNameAndDescription(StrandName, StrandDescription, StrandShortName);
            schemeSearchPanel = schemeSearchPanel.ClickOKButton();

            //add Levels
            schemeSearchPanel = schemeSearchPanel.ClickAssignExistingLevelButton();
            schemeSearchPanel = schemeSearchPanel.ClickAddExistingLevelLink();
            schemeSearchPanel.SelectLevel(1);
            schemeSearchPanel.SelectLevel(2);
            schemeSearchPanel = schemeSearchPanel.AddExistingLevelOKButton();
            //create Statement
            schemeSearchPanel = schemeSearchPanel.ClickLevelNode();
            schemeSearchPanel = schemeSearchPanel.ClickCreateStatementButton();
            schemeSearchPanel = schemeSearchPanel.ClickCreateNewStatementLink();
            string StatementName = SchemeSearchPanel.GenerateRandomString(10);
            string StatementDescription = SchemeSearchPanel.GenerateRandomString(20);
            string StatementShortName = SchemeSearchPanel.GenerateRandomString(10);
            schemeSearchPanel.SetNameAndDescription(StatementName, StatementDescription, StatementShortName);
            schemeSearchPanel = schemeSearchPanel.ClickOKButton();

            // delete statement
            schemeSearchPanel = schemeSearchPanel.ClickDeleteStatementButton();
            schemeSearchPanel = schemeSearchPanel.ClickDeleteOKButton();
            Thread.Sleep(1000);
        }

        /// <summary>
        /// Story 28143-Assessment - Scheme - New From Exisiting
        /// </summary>
        [TestMethod]
        [WebDriverTest(Enabled = true, Groups = new[] { "CloneScheme", "Scheme" }, Browsers = new[] { BrowserDefaults.Chrome })]
        [Variant(Variant.EnglishStatePrimary)]
        public void CloneScheme()
        {
            NavigateToCreateScheme();
            SchemeSearchPanel schemeSearchPanel = new SchemeSearchPanel();
            string schemeName = SchemeSearchPanel.GenerateRandomString(10);
            string schemeDescription = SchemeSearchPanel.GenerateRandomString(20);
            string schemeShortName = SchemeSearchPanel.GenerateRandomString(10);
            schemeSearchPanel.SetNameAndDescription(schemeName, schemeDescription, schemeShortName);
            schemeSearchPanel = schemeSearchPanel.ClickOKButton();

            AddStatement();

            schemeSearchPanel.ClickSaveButton();
            schemeSearchPanel.waitforSavemessagetoAppear();
            schemeSearchPanel.SaveMessageAssertionSuccess();

            schemeSearchPanel.ClickCancelButton();
            schemeSearchPanel.NavigateToNewFromExistingScheme();
            CloneSchemeSearchPanel cloneschemeSearchPanel = new CloneSchemeSearchPanel();
            cloneschemeSearchPanel.SetSchemeName(schemeName);
            cloneschemeSearchPanel.Search();
            cloneschemeSearchPanel.SelectSchemeByName(schemeName);
            cloneschemeSearchPanel.ClickSelectSchemeButton();
            cloneschemeSearchPanel.ClickOkButton();

            string clonedSchemeName = cloneschemeSearchPanel.GetClonedSchemeName();
            Assert.IsTrue(clonedSchemeName.Contains(schemeName));            
        }
    }
}

