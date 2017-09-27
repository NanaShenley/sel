using System.Threading;
using Assessment.Components.PageObject;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestSettings;
using WebDriverRunner.webdriver;
using Assessment.Components.Common;
using SharedComponents.BaseFolder;
using SharedComponents.Helpers;
using System.Collections.Generic;
using System;
using OpenQA.Selenium.Support.UI;
using System.Linq;
using SeSugar.Automation;
using Selene.Support.Attributes;

namespace Assessment.DEA.Tests
{
    [TestClass]
    public class Aspects : BaseSeleniumComponents
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

        #region Assessment - Aspect Search
        /// <summary>
        /// Story - 11596 - Maintain Aspects
        /// Search for Aspects by name
        /// Story - 29103 - Enhance Aspects search
        /// </summary>       
        [TestMethod]
        [WebDriverTest(Enabled = true, Groups = new[] { "Marksheet Builder", "Aspect", "AspectDetailsSearch" }, Browsers = new[] { BrowserDefaults.Chrome })]
        public void AspectDetailsSearch()
        {
            WebDriverWait waiter = new WebDriverWait(WebContext.WebDriver, TimeSpan.FromSeconds(MarksheetConstants.Timeout));
            //Login
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.AssessmentCoordinator);
            //Going to desired path
            AutomationSugar.NavigateMenu("Tasks", "Assessment", "Aspects");
            AspectDetails aspectdetails = new AspectDetails();
            aspectdetails.SearchByAspectName("Key Stage 1 TA COMENG ACT");
            aspectdetails.Search();
            String ResultCounter = aspectdetails.NumberofSearchResults();
            Assert.AreEqual("1 Match", ResultCounter);
            //String AspectName = aspectdetails.getAspectNameInSearchresults();          
        }

        /// <summary>
        /// Story - 11596 - Maintain Aspects
        /// Search for Aspects by Gradeset Type
        /// Story - 29103 - Enhance Aspects search
        /// </summary>        
        [TestMethod]
        [WebDriverTest(Enabled = true, Groups = new[] { "Marksheet Builder", "Aspect", "AspectDetailsSearchByGradesetType" }, Browsers = new[] { BrowserDefaults.Chrome })]
        public void AspectDetailsSearchByGradesetType()
        {
            WebDriverWait waiter = new WebDriverWait(WebContext.WebDriver, TimeSpan.FromSeconds(MarksheetConstants.Timeout));
            //Login
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.AssessmentCoordinator);
            //Going to desired path
            AutomationSugar.NavigateMenu("Tasks", "Assessment", "Aspects");

            AspectDetails aspectdetails = new AspectDetails();
            List<string> AspectResults = TestData.CreateDataList("select Name from aspect where tenantid = '" + TestDefaults.Default.TenantId + "' and aspecttype is not null and aspecttype not in (select id from aspect where code = 'ADT' or code = 'AND') and assessmentgradeset in (select id from AssessmentGradeset where tenantid = '" + TestDefaults.Default.TenantId + "' and AssessmentGradesetType in (select id from AssessmentGradesetType where tenantid = '" + TestDefaults.Default.TenantId + "' and code = 'G'))", "Name");

            aspectdetails.SearchByAspectName(AspectResults.FirstOrDefault());
            SeleniumHelper.ChooseSelectorOption(aspectdetails.SearchByGradesetTypeDropdown, "Grade");
            aspectdetails.Search();
            String ResultCounter = aspectdetails.NumberofSearchResults();
            Assert.AreEqual("1 Match", ResultCounter);
            //String AspectName = aspectdetails.getAspectNameInSearchresults();
            //Assert.IsTrue(AspectName.Contains("Age"));
        }

        /// <summary>
        /// Story - 11596 - Maintain Aspects
        /// Search for Aspects by Subject
        /// Story - 29103 - Enhance Aspects search
        /// </summary>
        [Variant(Variant.EnglishStatePrimary)]
        [TestMethod]
        [WebDriverTest(Enabled = true, Groups = new[] { "Marksheet Builder", "Aspect", "AspectSearchBySubject" }, Browsers = new[] { BrowserDefaults.Chrome })]
        public void AspectSearchBySubject()
        {
            WebDriverWait waiter = new WebDriverWait(WebContext.WebDriver, TimeSpan.FromSeconds(MarksheetConstants.Timeout));
            //Login
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.AssessmentCoordinator);
            //Going to desired path
            AutomationSugar.NavigateMenu("Tasks", "Assessment", "Aspects");

            AspectDetails aspectdetails = new AspectDetails();
            WaitUntillAjaxRequestCompleted();
            aspectdetails.SearchByAspectName("SC P Scale: Science");// ("Key Stage 1 TA COMENG ACT");
            SeleniumHelper.ChooseSelectorOption(aspectdetails.SearchBySubjectDropdown, "Science"); //("English");
            aspectdetails.Search();
            Thread.Sleep(8000);
            String AspectName = aspectdetails.getAspectNameInSearchresults();
            //    Assert.AreEqual("PRIMARY:CAT:Non Verbal EXT ATN ACT", AspectName);
            String ResultCounter = aspectdetails.NumberofSearchResults();
            Assert.AreEqual("1 Match", ResultCounter);
        }

        /// <summary>        
        /// Story - 29103 - Enhance Aspects search
        /// Search for Aspects by Gradeset        
        /// </summary>      
        [TestMethod]
        [WebDriverTest(Enabled = true, Groups = new[] { "Marksheet Builder", "Aspect", "AspectSearchByGradeset" }, Browsers = new[] { BrowserDefaults.Chrome })]
        public void AspectSearchByGradeset()
        {
            WebDriverWait waiter = new WebDriverWait(WebContext.WebDriver, TimeSpan.FromSeconds(MarksheetConstants.Timeout));
            //Login
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.AssessmentCoordinator);
            //Going to desired path
            AutomationSugar.NavigateMenu("Tasks", "Assessment", "Aspects");

            AspectDetails aspectdetails = new AspectDetails();

            List<string> AspectResults = TestData.CreateDataList("select Name from aspect where tenantid = '" + TestDefaults.Default.TenantId + "' and aspecttype is not null and aspecttype not in (select id from aspect where code = 'ADT' or code = 'AND') and assessmentgradeset in (select id from AssessmentGradeset where tenantid = '" + TestDefaults.Default.TenantId + "' and AssessmentGradesetType in (select id from AssessmentGradesetType where tenantid = '" + TestDefaults.Default.TenantId + "' and code = 'A'))", "Name");
            aspectdetails.SearchByAspectName(AspectResults.FirstOrDefault());

            SeleniumHelper.ChooseSelectorOption(aspectdetails.SearchByGradesetDropdown, "Age -99/11-99/11");
            aspectdetails.Search();
            Thread.Sleep(1000);

            string ResultCounter = aspectdetails.NumberofSearchResults();
            Assert.AreEqual("1 Match", ResultCounter);
        }

        /// <summary>        
        /// Story - 29103 - Enhance Aspects search
        /// Search for Aspects by Resource Provider
        /// </summary>

        [TestMethod]
        [WebDriverTest(Enabled = true, Groups = new[] { "Marksheet Builder", "Aspect", "AspectSearchByResourceProvider" }, Browsers = new[] { BrowserDefaults.Chrome })]
        public void AspectSearchByResourceProvider()
        {
            WebDriverWait waiter = new WebDriverWait(WebContext.WebDriver, TimeSpan.FromSeconds(MarksheetConstants.Timeout));
            //Login
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.AssessmentCoordinator);
            //Going to desired path
            AutomationSugar.NavigateMenu("Tasks", "Assessment", "Aspects");
            AspectDetails aspectdetails = new AspectDetails();

            //WaitUntillAjaxRequestCompleted();
            //List<string> AspectResults = TestData.CreateDataList("select Name from aspect where tenantid = '" + TestDefaults.Default.TenantId + "' and ResourceProvider in (select ID from ResourceProvider where Name = 'CAPITA SIMS')", "Name");
            //aspectdetails.SearchByAspectName(AspectResults.FirstOrDefault());

            SeleniumHelper.ChooseSelectorOption(aspectdetails.SearchByResourceProviderDropdown, "CAPITA SIMS");
            aspectdetails.Search();
            Thread.Sleep(1000);
            //String ResultCounter = aspectdetails.NumberofSearchResults();
            //Assert.AreEqual("1 Match", ResultCounter);
        }

        /// <summary>        
        /// Story - 29103 - Enhance Aspects search
        /// Search for Aspects by IsStatutory
        /// </summary>       
        [TestMethod]
        [WebDriverTest(Enabled = true, Groups = new[] { "Marksheet Builder", "Aspect", "AspectSearchByIsStatutory" }, Browsers = new[] { BrowserDefaults.Chrome })]
        public void AspectSearchByIsStatutory()
        {
            //Login
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.AssessmentCoordinator);
            //Going to desired path
            AutomationSugar.NavigateMenu("Tasks", "Assessment", "Aspects");
            AspectDetails aspectdetails = new AspectDetails();
            List<string> AspectResults = TestData.CreateDataList("select Name from aspect where tenantid = '" + TestDefaults.Default.TenantId + "' and IsStatutory = 1", "Name");
            aspectdetails.SearchByAspectName(AspectResults.FirstOrDefault());
            aspectdetails.IsStatutory.Click();
            aspectdetails.Search();
            Thread.Sleep(1000);
            String ResultCounter = aspectdetails.NumberofSearchResults();
            Assert.AreEqual("1 Match", ResultCounter);
        }
        #endregion

        #region Aspect Creation
        /// <summary>
        /// Story - 11596 - Maintain Aspects
        /// Create Aspect with Gradeset Type
        /// Story - 18020 - Changes to options for Assessment Purpose and Method
        /// </summary>        
        [TestMethod]
        [WebDriverTest(Enabled = true, Groups = new[] { "Marksheet Builder", "Aspect", "CreateAspectWithGradesetType" }, Browsers = new[] { BrowserDefaults.Chrome })]
        public void CreateAspectWithGradesetType()
        {
            WebDriverWait waiter = new WebDriverWait(WebContext.WebDriver, TimeSpan.FromSeconds(MarksheetConstants.Timeout));
            //Login
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.AssessmentCoordinator);
            //Going to desired path
            SeleniumHelper.EnableFeature("AssessmentDate");
            AutomationSugar.NavigateMenu("Tasks", "Assessment", "Aspects");

            AspectDetails aspectdetails = new AspectDetails();
            aspectdetails.SearchPanelShowHideClick();
            Thread.Sleep(2000);

            aspectdetails.CreateAspect();
            AutomationSugar.WaitForAjaxCompletion();

            GradesetDataMaintenance gradesetDataMaintenance = new GradesetDataMaintenance();
            String AspectName = gradesetDataMaintenance.GenerateRandomString(10);
            aspectdetails.setAspectName(AspectName);
            AutomationSugar.WaitForAjaxCompletion();
            aspectdetails.setAspectDescription("Test Description");
            SeleniumHelper.ChooseSelectorOption(aspectdetails.PhaseSelector, "Key Stage 1");
            AutomationSugar.WaitForAjaxCompletion();
            SeleniumHelper.ChooseSelectorOption(aspectdetails.Subject, "L&L:English");
            aspectdetails.SetAssessmentDateFlag();

            SeleniumHelper.ChooseSelectorOption(aspectdetails.AspectType, "Grade");
            AutomationSugar.WaitForAjaxCompletion();
            GradesetSearchPanel gradesetSearchPanel = aspectdetails.GradesetButtonClick();
            AutomationSugar.WaitForAjaxCompletion();
            gradesetSearchPanel.Search(true);
            WaitUntillAjaxRequestCompleted();
            Thread.Sleep(1000);
            gradesetDataMaintenance = gradesetSearchPanel.SelectGradesetByName("TA Grades KS1 Cross Curric");
            gradesetSearchPanel.ClickOkButton(true, "TA Grades KS1 Cross Curric");
            AutomationSugar.WaitForAjaxCompletion();
            aspectdetails.SaveButtonClick();
            AutomationSugar.WaitForAjaxCompletion();
            aspectdetails.DeleteButtonClick();
            AutomationSugar.WaitForAjaxCompletion();
            aspectdetails.ContinueButtonClick();
        }

        /// <summary>
        /// Story - 11596 - Maintain Aspects
        /// Create Aspect with Comment Type
        /// Story - 18020 - Changes to options for Assessment Purpose and Method
        /// </summary>      
        [TestMethod]
        [WebDriverTest(Enabled = true, Groups = new[] { "Marksheet Builder", "Aspect", "CreateAspectComment" }, Browsers = new[] { BrowserDefaults.Chrome })]
        public void CreateAspectWithCommentType()
        {
            WebDriverWait waiter = new WebDriverWait(WebContext.WebDriver, TimeSpan.FromSeconds(MarksheetConstants.Timeout));
            //Login
            //FeatureBee.FeatureBeeLogin.LoginWithFeatureBee(new[] { "AssessmentDate" }, FeatureBee.FeatureBeeLogin.iSIMSUserType.AssessmentCoordinator);
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.AssessmentCoordinator);
            //Going to desired path
            AutomationSugar.NavigateMenu("Tasks", "Assessment", "Aspects");

            AspectDetails aspectdetails = new AspectDetails();
            aspectdetails.SearchPanelShowHideClick();
            Thread.Sleep(2000);

            aspectdetails.CreateAspect();
            GradesetDataMaintenance gradesetDataMaintenance = new GradesetDataMaintenance();
            String AspectName = gradesetDataMaintenance.GenerateRandomString(10);
            aspectdetails.setAspectName(AspectName);
            aspectdetails.setAspectDescription("Test Description");
            SeleniumHelper.ChooseSelectorOption(aspectdetails.PhaseSelector, "Key Stage 1");
            AutomationSugar.WaitForAjaxCompletion();
            SeleniumHelper.ChooseSelectorOption(aspectdetails.Subject, "L&L:English");
            SeleniumHelper.ChooseSelectorOption(aspectdetails.AspectType, "Comment");
            Thread.Sleep(2000);
            aspectdetails.SaveButtonClick();
            aspectdetails.DeleteButtonClick();
            aspectdetails.ContinueButtonClick();
        }
        #endregion

    }
}
