using TestSettings;
using WebDriverRunner.webdriver;
using Assessment.Components.Common;
using SharedComponents.BaseFolder;
using SharedComponents.Helpers;
using System;
using OpenQA.Selenium.Support.UI;
using SeSugar.Automation;
using Selene.Support.Attributes;
using OpenQA.Selenium.Support.PageObjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Assessment.Components.PageObject;
using System.Collections.Generic;
using System.Threading;
using Assessment.Components;
using OpenQA.Selenium;
using POM.Components.HomePages;

namespace Assessment.CNR.Other.Assessment.Screens.Tests
{
    [TestClass]
    public class ManageTopics : BaseSeleniumComponents
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

        [FindsBy(How = How.CssSelector, Using = "span[data-automation-id='topic_header_title']")]
        private IWebElement ManageExpectationsTitle;
        public const string SchemeSelected = "DFE National Curriculum";
        public const string YearGroupSelected = "Year 1";
        public const string TermSelected = "Autumn";
        public const string Year4GroupSelected = "Year 4";
        public const string KeyStageTermSelected = "End of Key Stage";
        public const string FilterTest = "English: Reading";
        public const string FilterSubject = "Geography";
        public const string SelectedStatement = "En Reading Comp Stat  1.01";
        public const string TopicSetup = "setuptopic";
        /// <summary>
        /// Story -  - Maintain Aspects
        /// Search for Aspects by name
        /// </summary>
        [Variant(Variant.EnglishStatePrimary)]
        [TestMethod]
        [WebDriverTest(Enabled = true, Groups = new[] { "NavigateToManageTopics", "Assessment CNR", "ManageTopics", "P1" }, Browsers = new[] { BrowserDefaults.Chrome })]
        
        public void NavigateToManageTopics()
        {
            WebDriverWait waiter = new WebDriverWait(WebContext.WebDriver, TimeSpan.FromSeconds(MarksheetConstants.Timeout));
            //Login
            String[] featureList = { "Curriculum" };
            FeatureBee.FeatureBeeLogin.LoginWithFeatureBee(featureList, FeatureBee.FeatureBeeLogin.iSIMSUserType.SchoolAdministrator);
            //Going to desired path            
            AutomationSugar.NavigateMenu("Tasks", "Assessment", "Manage Curriculum Topics");
            TopicDataMaintainance topicDataMaintainance = new TopicDataMaintainance();

            string TopicTitle = topicDataMaintainance.GetTopicTitle();
            Assert.AreEqual(TopicTitle, "Topic");
        }


        /// <summary>
        /// Test to demonstrate that apply filter
        /// Story 28460-Assessment - Scheme - Display Existing scheme based on filter criteria
        /// </summary>
        [TestMethod]
        [WebDriverTest(Enabled = true, Groups = new[] { "DisplayExistingSchemeforTopic", "Scheme", "P2" }, Browsers = new[] { BrowserDefaults.Chrome })]
        [Variant(Variant.EnglishStatePrimary)]
        public void DisplayExistingSchemeforTopic()
        {
            NavigateToManageTopics();
            TopicDataMaintainance panel = new TopicDataMaintainance();
            //uncomment this once save scheme is done
            //SeleniumHelper.ChooseSelectorOption(panel.SchemesDropdownInitiator, SchemeSelected);
            panel.SetFilter(FilterTest);
            List<string> SchemeData = panel.GetFilteredSchemeData();
            Assert.IsTrue(SchemeData.Contains("English: Reading"));
            Assert.IsTrue(SchemeData.Contains("Comprehension"));
        }

        /// <summary>
        /// Test that validates that we can save a topic with no statements associated.
        /// </summary>
        [TestMethod]
        [WebDriverTest(Enabled = true, Groups = new[] { "TopicBasicDetailSave", "ManageTopics" ,"test", "P2"}, Browsers = new[] { BrowserDefaults.Chrome })]
        [Variant(Variant.EnglishStatePrimary | Variant.WelshStatePrimary)]
        public void CreateTopicBasicDetails()
        {
            NavigateToManageTopics();
            TopicDataMaintainance topicdatamaintainance = new TopicDataMaintainance();

            topicdatamaintainance = topicdatamaintainance.ClickCreateButton();
            // Generating Basice Details
            string NewTopicName = "Selenium Test Topic " + topicdatamaintainance.GenerateRandomString(10);
            topicdatamaintainance.SetTopicName(NewTopicName);

            topicdatamaintainance.SetTopicDescription(NewTopicName + " Description");

            SeleniumHelper.ChooseSelectorOption(topicdatamaintainance.TopicYearDropdownInitiator, YearGroupSelected);
            SeleniumHelper.ChooseSelectorOption(topicdatamaintainance.TopicTermDropdownInitiator, TermSelected);

            //Saving the Topic
            topicdatamaintainance = topicdatamaintainance.ClickDialogOkButton();
            topicdatamaintainance.SaveTopicSuccess();

            Thread.Sleep(3000);
            List<string> topicNames = topicdatamaintainance.GetTopicNamesFromPicker();
            //assert to verify that the Topic is listed in the right hand side picker
            Assert.IsTrue(topicNames.Contains(NewTopicName.Replace(" ", string.Empty).ToLower()));
       //     topicdatamaintainance.ClickDeleteButton(NewTopicName);
       //     topicdatamaintainance.ContinueButtonClick();
        }

        /// <summary>
        /// Test to demonstrate that a Topic can be associated with a colour.
        /// </summary>
        [TestMethod]
        [WebDriverTest(Enabled = true, Groups = new[] { "CreateTopicWithColour", "ManageTopics" }, Browsers = new[] { BrowserDefaults.Chrome })]
        [Variant(Variant.EnglishStatePrimary | Variant.WelshStatePrimary)]
        public void CreateTopicWithColour()
        {
            //Login
            NavigateToManageTopics();
            TopicDataMaintainance topicdatamaintainance = new TopicDataMaintainance();
            List<string> topicNames = topicdatamaintainance.GetTopicNamesFromPicker();
            string NewTopicName;

            //Create topic if not exists
            if (topicNames[0] == TopicSetup)
            {
                //Click on Setup new Topic Button.
                topicdatamaintainance = topicdatamaintainance.ClickSetupNewTopicButton();
                // Enter Basic Details
                NewTopicName = "Selenium Test Topic " + topicdatamaintainance.GenerateRandomString(10);
                topicdatamaintainance.SetTopicName(NewTopicName);
                topicdatamaintainance.SetTopicDescription(NewTopicName + " Description");
                topicdatamaintainance.AssignColour();
                //Click on dialog Ok button and save the Topic
                topicdatamaintainance = topicdatamaintainance.ClickDialogOkButton();
                topicdatamaintainance.SaveTopicSuccess();
            }
            else
            {
                NewTopicName = topicNames[0];
            }
            topicdatamaintainance = topicdatamaintainance.SelectTopicDropDown();
            topicdatamaintainance.ClickDeleteButton(NewTopicName);
            topicdatamaintainance.ClickDeleteDialogOkButton();
        }

        [TestMethod]
        [WebDriverTest(Enabled = true, Groups = new[] { "AllocateStatement", "ManageTopics", "Assessment CNR", "P2" }, Browsers = new[] { BrowserDefaults.Chrome })]
        [Variant(Variant.EnglishStatePrimary | Variant.WelshStatePrimary)]
        public void AllocateStatement()
        {
            NavigateToManageTopics();
            TopicDataMaintainance topicdatamaintainance = new TopicDataMaintainance();                                 

            List<string> topicNames = topicdatamaintainance.GetTopicNamesFromPicker();
            string NewTopicName;

            //Create topic if not exists
            if (topicNames[0] == TopicSetup)
            {
                //Click on Setup new Topic Button.
                topicdatamaintainance = topicdatamaintainance.ClickSetupNewTopicButton();
                // Enter Basic Details
                NewTopicName = "Selenium Test Topic " + topicdatamaintainance.GenerateRandomString(10);
                topicdatamaintainance.SetTopicName(NewTopicName);
                topicdatamaintainance.SetTopicDescription(NewTopicName + " Description");
                topicdatamaintainance.AssignColour();
                //Click on dialog Ok button and save the Topic
                topicdatamaintainance = topicdatamaintainance.ClickDialogOkButton();
                topicdatamaintainance.SaveTopicSuccess();
            }
            else
            {
                NewTopicName = topicNames[0];
            }            

            //Select Statement   
            String selectedStatement = topicdatamaintainance.SelectStatement(5);

            //Allocate Statement
            topicdatamaintainance.AllocateStatment();

            //Check for Unallocated statements
           // topicdatamaintainance.ClickAllocatedStatements(5, SelectedStatement);

            //Edit Existing Topic Part                       
            topicdatamaintainance = topicdatamaintainance.SelectTopicDropDown();
            topicdatamaintainance = topicdatamaintainance.OpenTopicButtonClick();            
         //   Assert.IsTrue(topicdatamaintainance.StatementExist(selectedStatement));

            NewTopicName = "Selenium Test Topic " + topicdatamaintainance.GenerateRandomString(10);
            topicdatamaintainance.SetTopicName(NewTopicName);
            topicdatamaintainance = topicdatamaintainance.ClickDialogOkButton();
            topicdatamaintainance.SaveTopicSuccess();

            Thread.Sleep(3000);
            topicNames = topicdatamaintainance.GetTopicNamesFromPicker();

            //assert to verify that the Topic is listed in the right hand side picker
            Assert.IsTrue(topicNames.Contains(NewTopicName.Replace(" ", string.Empty).ToLower()));

            // Delete Edited Topic
            topicdatamaintainance = topicdatamaintainance.SelectTopicDropDown();
             topicdatamaintainance.ClickDeleteButton(NewTopicName);          
            topicdatamaintainance = topicdatamaintainance.ClickDeleteDialogOkButton();
            topicdatamaintainance.DeleteTopicSuccess();

            //Confirm Topic Deletion 
            Thread.Sleep(3000);
             topicNames = topicdatamaintainance.GetTopicNamesFromPicker();

            //Verify Deleted Topic not present
            Assert.IsFalse(topicNames.Contains(NewTopicName.Replace(" ", string.Empty).ToLower()));
        }

        /// <summary>
        /// Story 20044 :Assessment - Topic - Create New topic
        /// </summary>
        [TestMethod]
        [WebDriverTest(Enabled = true, Groups = new[] { "SetupNewTopic", "ManageTopics", "P2" }, Browsers = new[] { BrowserDefaults.Chrome })]
        [Variant(Variant.EnglishStatePrimary | Variant.WelshStatePrimary)]
        public void SetupNewTopic()
        {
            NavigateToManageTopics();

            TopicDataMaintainance topicdatamaintainance = new TopicDataMaintainance();
            // Click on Setup new Topic Button.
            topicdatamaintainance = topicdatamaintainance.ClickSetupNewTopicButton();
            // Enter Basic Details
            string NewtopicName = "Selenium Test Topic " + topicdatamaintainance.GenerateRandomString(10);
            topicdatamaintainance.SetTopicName(NewtopicName);

            topicdatamaintainance.SetTopicDescription(NewtopicName + " Description");

            //Click on dialog Ok button and save the Topic
            topicdatamaintainance = topicdatamaintainance.ClickDialogOkButton();
            topicdatamaintainance.SaveTopicSuccess();

     //       List<string> topicNames = topicdatamaintainance.GetTopicNamesFromPicker();
            //assert to verify that the Topic is listed in the right hand side picker
      //      Assert.IsTrue(topicNames.Contains(NewtopicName.Replace(" ", string.Empty).ToLower()));
        //    topicdatamaintainance = topicdatamaintainance.SelectTopicDropDown();
       //     topicdatamaintainance.ClickDeleteButton(NewtopicName);
       //     topicdatamaintainance.ContinueButtonClick();

        }


        /// <summary>
        /// Test to demonstrate that we can edit a topic by changing the statements associated with the topic.
        /// We will select a statement for a topic, save the topic. While editing we will remove the statement and select a statement for a different subject 
        /// </summary>
        [TestMethod]
        [WebDriverTest(Enabled = false, Groups = new[] { "EditTopic", "ManageTopics", "P2" }, Browsers = new[] { BrowserDefaults.Chrome })]
        [Variant(Variant.EnglishStatePrimary)]
        public void EditTopic()
        {
            //Login
            NavigateToManageTopics();
            TopicDataMaintainance topicdatamaintainance = new TopicDataMaintainance();
            topicdatamaintainance = topicdatamaintainance.ClickCreateButton();
            // Generating Basice Details
            string NewtopicName = "Selenium Test Topic " + topicdatamaintainance.GenerateRandomString(10);
            topicdatamaintainance.SetTopicName(NewtopicName);

            topicdatamaintainance.SetTopicDescription(NewtopicName + " Description");

            //Selecting NC Year And assessment Period
            SeleniumHelper.ChooseSelectorOption(topicdatamaintainance.TopicYearDropdownInitiator, YearGroupSelected);
            SeleniumHelper.ChooseSelectorOption(topicdatamaintainance.TopicTermDropdownInitiator, TermSelected);

            //Choose Colour.
            topicdatamaintainance.AssignColour();

            topicdatamaintainance.SelectStatement();
            topicdatamaintainance = topicdatamaintainance.ClickSaveButton();
            topicdatamaintainance.SaveTopicSuccess();

            TopicSearchPanel topicsearchpanel = new TopicSearchPanel();
            topicsearchpanel.setTopicName(NewtopicName);
            Thread.Sleep(2000);
            topicsearchpanel = topicsearchpanel.Search();
            Assert.AreEqual(topicsearchpanel.GetSearchResultCount(), "1");
            //select the topic.
            Thread.Sleep(1000);
            topicsearchpanel.SelectResult();
            Thread.Sleep(2000);
            //Clear Statements.
            topicdatamaintainance.ClearStatement();
            //filter by name - subject 
            topicdatamaintainance.SetFilter(FilterSubject);
            //After selecting another statement
            topicdatamaintainance.SelectStatement();
            //Saving the Topic
            topicdatamaintainance = topicdatamaintainance.ClickSaveButton();
            topicdatamaintainance.SaveTopicSuccess();
            // run the delete.
            topicdatamaintainance = topicdatamaintainance.SelectTopicDropDown();
            topicdatamaintainance.ClickDeleteButton(NewtopicName);
            topicdatamaintainance.ContinueButtonClick();
        }

        /// <summary>
        /// Test that validates that we a topic can be searched by Name and Year
        /// </summary>
        [TestMethod]
        [WebDriverTest(Enabled = false, Groups = new[] { "TopicSearchByNameAndYear", "ManageTopics" }, Browsers = new[] { BrowserDefaults.Chrome })]
        [Variant(Variant.EnglishStatePrimary)]
        public void SearchTopicWithNameAndYear()
        {
            NavigateToManageTopics();
            // Click on Create Button.
            TopicDataMaintainance topicdatamaintainance = new TopicDataMaintainance();
            topicdatamaintainance = topicdatamaintainance.ClickCreateButton();
            // Generating Basice Details
            string NewtopicName = "Selenium Test Topic " + topicdatamaintainance.GenerateRandomString(10);
            topicdatamaintainance.SetTopicName(NewtopicName);

            topicdatamaintainance.SetTopicDescription(NewtopicName + " Description");

            //SeleniumHelper.ChooseSelectorOption(topicdatamaintainance.TopicYearDropdownInitiator, "Curriculum Year 1");
            SeleniumHelper.ChooseSelectorOption(topicdatamaintainance.TopicYearDropdownInitiator, YearGroupSelected);
            SeleniumHelper.ChooseSelectorOption(topicdatamaintainance.TopicTermDropdownInitiator, TermSelected);

            //Saving the Topic
            topicdatamaintainance = topicdatamaintainance.ClickSaveButton();
            topicdatamaintainance.SaveTopicSuccess();

            //search for saved topic
            TopicSearchPanel topicsearchpanel = new TopicSearchPanel();
            topicsearchpanel.setTopicName(NewtopicName);
            SeleniumHelper.ChooseSelectorOption(topicdatamaintainance.TopicSearchYearDropdownInitiator, YearGroupSelected);

            Thread.Sleep(3000);
            topicsearchpanel = topicsearchpanel.Search();
            Assert.AreEqual(topicsearchpanel.GetSearchResultCount(), "1");
            // run the delete
            topicdatamaintainance = topicdatamaintainance.SelectTopicDropDown();
            topicdatamaintainance.ClickDeleteButton(NewtopicName);
            topicdatamaintainance.ContinueButtonClick();
        }

        /// <summary>
        /// Test that validates that a term is populated based on the selection done for the year.
        /// </summary>
        [TestMethod]
        [WebDriverTest(Enabled = false, Groups = new[] { "DefaultTermOnYear", "ManageTopics" }, Browsers = new[] { BrowserDefaults.Chrome })]
        [Variant(Variant.EnglishStatePrimary)]
        public void DefaultTermOnYearSelection()
        {
            NavigateToManageTopics();
            // Click on Create Button.
            TopicDataMaintainance topicdatamaintainance = new TopicDataMaintainance();
            topicdatamaintainance = topicdatamaintainance.ClickCreateButton();
            // Generating Basice Details
            string topicName = "Selenium Test Topic " + topicdatamaintainance.GenerateRandomString(10);
            topicdatamaintainance.SetTopicName(topicName);

            topicdatamaintainance.SetTopicDescription(topicName + " Description");

            SeleniumHelper.ChooseSelectorOption(topicdatamaintainance.TopicYearDropdownInitiator, Year4GroupSelected);
            WaitForElement(TimeSpan.FromSeconds(MarksheetConstants.Timeout), By.CssSelector("input[name='NCYear.dropdownImitator']"));
            Thread.Sleep(1000);
            bool foundMatchingTerm = topicdatamaintainance.MatchTermOption(KeyStageTermSelected);

            Assert.IsTrue(foundMatchingTerm, "Matching Term Key Stage was not found");
        }

        /// <summary>
        /// Test that validates that we can filter on the Tree control to retreive matching strands.
        /// </summary>
        [TestMethod]
        [WebDriverTest(Enabled = false, Groups = new[] { "FilterTreeControlTopicScreen", "ManageTopics" }, Browsers = new[] { BrowserDefaults.Chrome })]
        [Variant(Variant.EnglishStatePrimary)]
        public void FilterTreeControlTopicScreen()
        {
            NavigateToManageTopics();
            // Click on Create Button.
            TopicDataMaintainance topicdatamaintainance = new TopicDataMaintainance();
            topicdatamaintainance = topicdatamaintainance.ClickCreateButton();
            // Generating Basice Details
            string topicName = "Selenium Test Topic " + topicdatamaintainance.GenerateRandomString(10);
            topicdatamaintainance.SetTopicName(topicName);

            topicdatamaintainance.SetTopicDescription(topicName + " Description");

            SeleniumHelper.ChooseSelectorOption(topicdatamaintainance.TopicYearDropdownInitiator, YearGroupSelected);
            SeleniumHelper.ChooseSelectorOption(topicdatamaintainance.TopicTermDropdownInitiator, TermSelected);

            //filter by name - subject 
            topicdatamaintainance.SetFilter(FilterSubject);
            //Select first statement.
            topicdatamaintainance.SelectStatement();
            //find matching statement descriptions.
            bool returnValue = topicdatamaintainance.MatchOnStatementDescription(FilterSubject);
            Assert.IsTrue(returnValue, "Statement Description does not match subject selected.");
        }

        [TestMethod]
        [WebDriverTest(Enabled = false, Groups = new[] { "CreatenDeleteTopic", "ManageTopics" , "P1"}, Browsers = new[] { BrowserDefaults.Chrome })]
        [Variant(Variant.EnglishStatePrimary)]
        public void CreatenDeleteTopic()
        {
            NavigateToManageTopics();

            TopicDataMaintainance topicdatamaintainance = new TopicDataMaintainance();
            topicdatamaintainance = topicdatamaintainance.ClickCreateButton();

            // Generating Basice Details
            string topicName = topicdatamaintainance.GenerateRandomString(40);
            topicdatamaintainance.SetTopicName(topicName);


            topicdatamaintainance.SetTopicDescription(topicName + " Description");

            //Selecting NC Year And assessment Period
            SeleniumHelper.ChooseSelectorOption(topicdatamaintainance.TopicYearDropdownInitiator, YearGroupSelected);
            SeleniumHelper.ChooseSelectorOption(topicdatamaintainance.TopicTermDropdownInitiator, TermSelected);


            //Saving the Topic
            topicdatamaintainance = topicdatamaintainance.ClickSaveButton();
            topicdatamaintainance.SaveTopicSuccess();

            TopicSearchPanel topicsearchpanel = new TopicSearchPanel();
            topicsearchpanel.setTopicName(topicName);
            SeleniumHelper.ChooseSelectorOption(topicdatamaintainance.TopicSearchYearDropdownInitiator, YearGroupSelected);


            Thread.Sleep(3000);
            topicsearchpanel = topicsearchpanel.Search();
            Assert.AreEqual(topicsearchpanel.GetSearchResultCount(), "1");
            topicdatamaintainance = topicdatamaintainance.SelectTopicDropDown();

            topicdatamaintainance.DeleteButtonClick();
            topicdatamaintainance.ContinueButtonClick();

        }

        //Test to verify additional columns work.
        [Variant(Variant.EnglishStatePrimary)]
        [TestMethod]
        [WebDriverTest(Enabled = false, Groups = new[] { "PopulateStatement", "ManageTopics" }, Browsers = new[] { BrowserDefaults.Chrome })]

        public void PopulateStatement()
        {
            //Login
            NavigateToManageTopics();

            //            SeleniumHelper.NavigateMenu("Tasks", "Curriculum", "Manage Curriculum Topics");
            TopicDataMaintainance topicdatamaintainance = new TopicDataMaintainance();
            topicdatamaintainance = topicdatamaintainance.ClickCreateButton();
            // Generating Basice Details
            string topicName = topicdatamaintainance.GenerateRandomString(40);
            topicdatamaintainance.SetTopicName(topicName);


            topicdatamaintainance.SetTopicDescription(topicName + " Description");

            //Selecting NC Year And assessment Period
            SeleniumHelper.ChooseSelectorOption(topicdatamaintainance.TopicYearDropdownInitiator, YearGroupSelected);
            SeleniumHelper.ChooseSelectorOption(topicdatamaintainance.TopicTermDropdownInitiator, TermSelected);

            topicdatamaintainance.SelectStatement();
            topicdatamaintainance = topicdatamaintainance.ClickSaveButton();
            topicdatamaintainance.SaveTopicSuccess();

            TopicSearchPanel topicsearchpanel = new TopicSearchPanel();
            topicsearchpanel.setTopicName(topicName);
            Thread.Sleep(3000);
            topicsearchpanel = topicsearchpanel.Search();
            Assert.AreEqual(topicsearchpanel.GetSearchResultCount(), "1");

        }
        /// <summary>
        /// Story 29239 Generate Template
        /// </summary>
        //To Generate Marksheet Topic Templae and Open POS Marksheet to check template 
        [Variant(Variant.EnglishStatePrimary)]
        [TestMethod]
        [WebDriverTest(Enabled = true, Groups = new[] { "GenerateTopicTemplate", "ManageTopics" }, Browsers = new[] { BrowserDefaults.Chrome })]

        public void GenerateTopicTemplate()
        {
            NavigateToManageTopics();

            TopicDataMaintainance topicdatamaintainance = new TopicDataMaintainance();
            // Create New Topic
            topicdatamaintainance = topicdatamaintainance.ClickCreateButton();
            // Generating Basice Details
            string topicName = "Selenium Test Topic " + topicdatamaintainance.GenerateRandomString(10);
            topicdatamaintainance.SetTopicName(topicName);

            topicdatamaintainance.SetTopicDescription(topicName + " Description");

            SeleniumHelper.ChooseSelectorOption(topicdatamaintainance.TopicYearDropdownInitiator, YearGroupSelected);
            SeleniumHelper.ChooseSelectorOption(topicdatamaintainance.TopicTermDropdownInitiator, TermSelected);

            //////Saving the Topic
            topicdatamaintainance = topicdatamaintainance.ClickDialogOkButton();
            topicdatamaintainance.SaveTopicSuccess();

            Thread.Sleep(3000);                      
            //Select Statement   
            topicdatamaintainance.SelectStatement(5);

            //Allocate Statement         
            topicdatamaintainance.AllocateStatmentByName(topicName);

            //Generate Template for created Topic 
            topicdatamaintainance = topicdatamaintainance.SelectTopicDropDownByName(topicName);
            topicdatamaintainance.GenerateTemplateSelecteTopicClick(topicName);

            //Open Marksheet
            topicdatamaintainance.OpenPosTemplate(topicName);
            POSDataMaintainanceScreen posDataMaintainanceScreen = new POSDataMaintainanceScreen();
            posDataMaintainanceScreen.SearchFilterButtonClick();
            posDataMaintainanceScreen.SelectMarksheetColumnName("En Reading Comp Stat 1.01");
            List<IWebElement> columnList = MarksheetGridHelper.FindCellsOfColumnByColumnNameForPOS("En Reading Comp Stat 1.01");

        }

        /// <summary>
        /// Story 26735 : Manage Scheme - Quick link for Topic
        /// </summary>
        [Variant(Variant.EnglishStatePrimary | Variant.WelshStatePrimary)]
        [WebDriverTest(Enabled = true, Groups = new[] { "QuickLinkForTopic", "Topic" }, Browsers = new[] { BrowserDefaults.Chrome })]
        public void QuickLinkForTopic()
        {
            WebDriverWait waiter = new WebDriverWait(WebContext.WebDriver, TimeSpan.FromSeconds(MarksheetConstants.Timeout));
            String[] featureList = { "Curriculum" };
            FeatureBee.FeatureBeeLogin.LoginWithFeatureBee(featureList, FeatureBee.FeatureBeeLogin.iSIMSUserType.AssessmentCoordinator);

            var assessmentQuickLinks = new AssessmentQuickLinks();
            // Check if Assessment Quick links exists
            assessmentQuickLinks.CheckIfAssessmentQuickLinkExists(SeleniumHelper.iSIMSUserType.AssessmentCoordinator);

            // Wait for the ajax request to complete
            WaitUntillAjaxRequestCompleted();

            // Open Assessment Quick Links Dropdown for class teacher
            assessmentQuickLinks.OpenAssessmentQuickLinksDropdown(SeleniumHelper.iSIMSUserType.AssessmentCoordinator);

            assessmentQuickLinks.ClickAndVerifyTopic(SeleniumHelper.iSIMSUserType.AssessmentCoordinator);
            TopicDataMaintainance topicDataMaintainance = new TopicDataMaintainance();
            string TopicTitle = topicDataMaintainance.GetTopicTitle();
            Assert.AreEqual(TopicTitle, "Topic");
        }

        }
    }