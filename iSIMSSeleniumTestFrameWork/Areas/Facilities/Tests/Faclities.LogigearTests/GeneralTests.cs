using NUnit.Framework;
using POM.Components.Help;
using POM.Components.HomePages;
using POM.Components.SchoolGroups;
using POM.Helper;
using Selene.Support.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestSettings;
using WebDriverRunner.internals;

namespace Faclities.LogigearTests
{
    public class GeneralTests
    {
        /// <summary>
        /// TC HP-01
        /// Au : An Nguyen
        /// Description: Exercise ability to perform a task search via the task action search function.
        /// Role: School Administrator
        /// </summary>

        [WebDriverTest(TimeoutSeconds = 400, Enabled = true, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { "g1" })]
        public void TC_HP001_Exercise_ability_search_function_Allocate_Future_Pupils_the_common_search_criteria()
        {
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);
            Wait.WaitForDocumentReady();
            var homePage = new HomePage();

            //Search with 'Allocate Future Pupils'
            var taskSearch = homePage.TaskSearch();
            taskSearch.Search = "Allocate Future";
            //Verify searching result
            var searchResultList = taskSearch.SearchResult;
            //Verify 'Allocate Future Pupils' displays on searching result
            var searchResult = searchResultList.Items.FirstOrDefault(t => t.TaskAction.Equals("Allocate Future Pupils"));
            Assert.AreNotEqual(null, searchResult, "'Allocate Future Pupils' screen does not display on search result");
            Assert.AreEqual("Allocate Future", searchResult.TaskActionHighlight, "'Group' is not underline on 'Allocate Future Pupils'");
            Assert.AreNotEqual(String.Empty, searchResult.FuntionalArea, "Functional area does not display on 'Allocate Future Pupils'");
        }

        [WebDriverTest(TimeoutSeconds = 400, Enabled = true, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { "g1" })]
        public void TC_HP002_Exercise_ability_search_function_Allocate_Pupils_to_Groups_the_common_search_criteria()
        {
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);
            Wait.WaitForDocumentReady();
            var homePage = new HomePage();

            //Search with 'Allocate Pupils to Groups'
            var taskSearch = homePage.TaskSearch();
            taskSearch.Search = "Allocate Pupils to";
            //Verify searching result
            var searchResultList = taskSearch.SearchResult;
            //Verify 'Allocate Pupils to Groups' displays on searching result
            var searchResult = searchResultList.Items.FirstOrDefault(t => t.TaskAction.Equals("Allocate Pupils to Groups"));
            Assert.AreNotEqual(null, searchResult, "'Allocate Pupils to Groups' screen does not display on search result");
            Assert.AreEqual("Allocate Pupils to", searchResult.TaskActionHighlight, "'Group' is not underline on 'Allocate Pupils to Groups'");
            Assert.AreNotEqual(String.Empty, searchResult.FuntionalArea, "Functional area does not display on 'Allocate Pupils to Groups'");
        }

        [WebDriverTest(TimeoutSeconds = 400, Enabled = true, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { "g1" })]
        public void TC_HP003_Exercise_ability_search_function_Manage_Classes_the_common_search_criteria()
        {
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);
            Wait.WaitForDocumentReady();
            var homePage = new HomePage();

            //Search with 'Manage Classes'
            var taskSearch = homePage.TaskSearch();
            taskSearch.Search = "Manage Classes";
            //Verify searching result
            var searchResultList = taskSearch.SearchResult;
            //Verify 'Manage User Defined Groups' displays on searching result
            var searchResult = searchResultList.Items.FirstOrDefault(t => t.TaskAction.Equals("Manage Classes"));
            Assert.AreNotEqual(null, searchResult, "'Manage Classes' screen does not display on search result");
            Assert.AreEqual("Manage Classes", searchResult.TaskActionHighlight, "'Manage Classes' is not underline on 'Manage Classes'");
            Assert.AreNotEqual(String.Empty, searchResult.FuntionalArea, "Functional area does not display on 'Manage Classes'");
        }

        [WebDriverTest(TimeoutSeconds = 400, Enabled = true, Browsers = new[] { BrowserDefaults.Chrome}, Groups = new[] { "g1" })]
        public void TC_HP004_Exercise_ability_search_function_Manage_Teaching_Groups_the_common_search_criteria()
        {
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);
            Wait.WaitForDocumentReady();
            var homePage = new HomePage();

            //Search with 'Manage Teaching Groups'
            var taskSearch = homePage.TaskSearch();
            taskSearch.Search = "Manage Teaching";
            //Verify searching result
            var searchResultList = taskSearch.SearchResult;
            //Verify 'Manage Teaching Groups' displays on searching result
            var searchResult = searchResultList.Items.FirstOrDefault(t => t.TaskAction.Equals("Manage Teaching Groups"));
            Assert.AreNotEqual(null, searchResult, "'Manage Teaching Groups' screen does not display on search result");
            Assert.AreEqual("Manage Teaching", searchResult.TaskActionHighlight, "'Group' is not underline on 'Manage Teaching Groups'");
            Assert.AreNotEqual(String.Empty, searchResult.FuntionalArea, "Functional area does not display on 'Manage Teaching Groups'");
        }



        [WebDriverTest(TimeoutSeconds = 400, Enabled = true, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { "g1" })]
        public void TC_HP005_Exercise_ability_search_function_Manage_User_Defined_Groups_the_common_search_criteria()
        {
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);
            Wait.WaitForDocumentReady();
            var homePage = new HomePage();

            //Search with 'Manage User Defined'
            var taskSearch = homePage.TaskSearch();
            taskSearch.Search = "Manage User Defined";
            //Verify searching result
            var searchResultList = taskSearch.SearchResult;
            //Verify 'Manage User Defined Groups' displays on searching result
            var searchResult = searchResultList.Items.FirstOrDefault(t => t.TaskAction.Equals("Manage User Defined Groups"));
            Assert.AreNotEqual(null, searchResult, "'Manage User Defined Groups' screen does not display on search result");
            Assert.AreEqual("Manage User Defined", searchResult.TaskActionHighlight, "'Group' is not underline on 'Manage User Defined Groups'");
            Assert.AreNotEqual(String.Empty, searchResult.FuntionalArea, "Functional area does not display on 'Manage User Defined Groups'");
        }


        [WebDriverTest(TimeoutSeconds = 400, Enabled = true, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { "g1" })]
        public void TC_HP006_Exercise_ability_search_function_Manage_Year_Groups_common_search_criteria()
        {
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);
            Wait.WaitForDocumentReady();
            var homePage = new HomePage();

            //Search with 'Manage Year Groups'
            var taskSearch = homePage.TaskSearch();
            taskSearch.Search = "Manage Year";
            //Verify searching result
            var searchResultList = taskSearch.SearchResult;
            //Verify 'Manage Year Groups' displays on searching result
            var searchResult = searchResultList.Items.FirstOrDefault(t => t.TaskAction.Equals("Manage Year Groups"));
            Assert.AreNotEqual(null, searchResult, "'Manage Year Groups' screen does not display on search result");
            Assert.AreEqual("Manage Year", searchResult.TaskActionHighlight, "'Manage Year Groups' is not underline on 'Manage Year Groups'");
            Assert.AreNotEqual(String.Empty, searchResult.FuntionalArea, "Functional area does not display on 'Manage Year Groups'");
        }

        [WebDriverTest(TimeoutSeconds = 400, Enabled = true, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { "g1" })]
        public void TC_HP007_Exercise_ability_search_function_Promote_Pupils_the_common_search_criteria()
        {
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);
            Wait.WaitForDocumentReady();
            var homePage = new HomePage();

            //Search with 'Promote Pupils'
            var taskSearch = homePage.TaskSearch();
            taskSearch.Refresh();
            taskSearch.Search = "Promote";
            //Verify searching result
            var searchResultList = taskSearch.SearchResult;
            //Verify 'Promote Pupils' displays on searching result
            var searchResult = searchResultList.Items.FirstOrDefault(t => t.TaskAction.Equals("Promote Pupils"));
            Assert.AreNotEqual(null, searchResult, "'Promote Pupils' screen does not display on search result");
            Assert.AreEqual("Promote", searchResult.TaskActionHighlight, "'Promote' is not underline on 'Promote Pupils'");
            Assert.AreNotEqual(String.Empty, searchResult.FuntionalArea, "Functional area does not display on 'Promote Pupils'");
        }


        [WebDriverTest(TimeoutSeconds = 400, Enabled = true, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { "g1" })]
        public void TC_HP008_Exercise_ability_search_function_My_School_Details_the_common_search_criteria()
        {
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);
            Wait.WaitForDocumentReady();
            var homePage = new HomePage();

            //Search with 'My School Details'
            var taskSearch = homePage.TaskSearch();
            taskSearch.Refresh();
            taskSearch.Search = "My School";
            //Verify searching result
            var searchResultList = taskSearch.SearchResult;
            //Verify 'My School Details' displays on searching result
            var searchResult = searchResultList.Items.FirstOrDefault(t => t.TaskAction.Equals("My School Details"));
            Assert.AreNotEqual(null, searchResult, "'My School Details' screen does not display on search result");
            Assert.AreEqual("My School", searchResult.TaskActionHighlight, "'My School' is not underline on 'My School Details'");
            Assert.AreNotEqual(String.Empty, searchResult.FuntionalArea, "Functional area does not display on 'My School Details'");
        }

        [WebDriverTest(TimeoutSeconds = 400, Enabled = true, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { "g1" })]
        public void TC_HP009_Exercise_ability_search_function_Other_School_Details_the_common_search_criteria()
        {
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);
            Wait.WaitForDocumentReady();
            var homePage = new HomePage();

            //Search with 'Other School Details'
            var taskSearch = homePage.TaskSearch();
            taskSearch.Search = "Other School";
            //Verify searching result
            var searchResultList = taskSearch.SearchResult;
            //Verify 'Other School Details' displays on searching result
            var searchResult = searchResultList.Items.FirstOrDefault(t => t.TaskAction.Equals("Other School Details"));
            Assert.AreNotEqual(null, searchResult, "'My School Details' screen does not display on search result");
            Assert.AreEqual("Other School", searchResult.TaskActionHighlight, "'Other School Details' is not underline on 'Promote Pupils'");
            Assert.AreNotEqual(String.Empty, searchResult.FuntionalArea, "Functional area does not display on 'Promote Pupils'");
        }

        [WebDriverTest(TimeoutSeconds = 400, Enabled = true, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { "g1" })]
        public void TC_HP0010_Exercise_ability_search_function_Rooms_match_the_common_search_criteria()
        {
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);
            Wait.WaitForDocumentReady();
            var homePage = new HomePage();

            //Search with 'Rooms'
            var taskSearch = homePage.TaskSearch();
            taskSearch.Refresh();
            taskSearch.Search = "Rooms";
            //Verify searching result
            var searchResultList = taskSearch.SearchResult;
            //Verify 'Rooms' displays on searching result
            var searchResult = searchResultList.Items.FirstOrDefault(t => t.TaskAction.Equals("Rooms"));
            Assert.AreNotEqual(null, searchResult, "'Rooms' screen does not display on search result");
            Assert.AreEqual("Rooms", searchResult.TaskActionHighlight, "'Rooms' is not underline on 'Rooms'");
            Assert.AreNotEqual(String.Empty, searchResult.FuntionalArea, "Functional area does not display on 'Rooms'");
        }

        [WebDriverTest(TimeoutSeconds = 400, Enabled = true, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { "g1" })]
        public void TC_HP0011_Exercise_ability_search_function_Key_Performance_Indicators_the_common_search_criteria()
        {
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);
            Wait.WaitForDocumentReady();
            var homePage = new HomePage();

            //Search with 'Key Performance Indicators '
            var taskSearch = homePage.TaskSearch();
            taskSearch.Search = "Key Performance";
            //Verify searching result
            var searchResultList = taskSearch.SearchResult;
            //Verify 'Key Performance Indicators ' displays on searching result
            var searchResult = searchResultList.Items.FirstOrDefault(t => t.TaskAction.Equals("Key Performance Indicators"));
            Assert.AreNotEqual(null, searchResult, "'Key Performance Indicators' screen does not display on search result");
            Assert.AreEqual("Key Performance", searchResult.TaskActionHighlight, "'Key Performance Indicators' is not underline on 'Key Performance Indicators'");
            Assert.AreNotEqual(String.Empty, searchResult.FuntionalArea, "Functional area does not display on 'Key Performance Indicators'");
        }

        [WebDriverTest(TimeoutSeconds = 400, Enabled = true, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { "g1" })]
        public void TC_HP0012_Exercise_ability_search_function_Academic_Years_match_the_common_search_criteria()
        {
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);
            Wait.WaitForDocumentReady();
            var homePage = new HomePage();

            //Search with ' Academic Years'
            var taskSearch = homePage.TaskSearch();
            taskSearch.Search = "Academic Years";
            //Verify searching result
            var searchResultList = taskSearch.SearchResult;
            //Verify 'Academic Years ' displays on searching result
            var searchResult = searchResultList.Items.FirstOrDefault(t => t.TaskAction.Equals("Academic Years"));
            Assert.AreNotEqual(null, searchResult, "'Academic Years' screen does not display on search result");
            Assert.AreEqual("Academic Years", searchResult.TaskActionHighlight, "'Academic Years' is not underline on 'Academic Years'");
            Assert.AreNotEqual(String.Empty, searchResult.FuntionalArea, "Functional area does not display on 'Academic Years'");
        }

    }
}
