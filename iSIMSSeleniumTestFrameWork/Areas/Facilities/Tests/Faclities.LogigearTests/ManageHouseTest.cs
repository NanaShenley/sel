using System;
using POM.Helper;
using NUnit.Framework;
using TestSettings;
using Selene.Support.Attributes;
using SeSugar.Automation;
using Facilities.POM.Components.ManageHouse;
using System.Collections.Generic;

namespace Faclities.LogigearTests
{
    public class ManageHouseTest
    {
        [WebDriverTest(TimeoutSeconds = 2000, Enabled = true, Groups = new[] { "All", "P2" }, Browsers = new[] { BrowserDefaults.Chrome }, DataProvider = "TC_MH_Data")]
        public void Add_House(string[] basicDetails)
        {
            // Login as School Adminstrator
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);
            Wait.WaitForDocumentReady();
            // Navigate to Manage Houses Screen
            AutomationSugar.NavigateMenu("Tasks", "School Groups", "Manage Houses");
            Wait.WaitForDocumentReady();
            //Adding New House
            var houseTriplet = new ManageHouseTriplet();
            var houseDetailPage = houseTriplet.Create();
            houseDetailPage.HouseFullName = basicDetails[0];
            houseDetailPage.HouseShortName = basicDetails[1];
            houseDetailPage.HouseColour = basicDetails[2];
            //Saving new house
            houseDetailPage.Save();
            Assert.AreEqual(false, houseDetailPage.IsSuccessMessageDisplayed(), "House record saved");
        }

        [WebDriverTest(TimeoutSeconds = 2000, Enabled = true, Groups = new[] { "All", "P2" }, Browsers = new[] { BrowserDefaults.Chrome }, DataProvider = "TC_MH_Data")]
        public void Search_House(string[] basicDetails)
        {
            // Login as School Adminstrator
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);
            Wait.WaitForDocumentReady();
            // Navigate to Manage Houses Screen
            AutomationSugar.NavigateMenu("Tasks", "School Groups", "Manage Houses");
            Wait.WaitForDocumentReady();
            //Triplet House
            var houseTriplet = new ManageHouseTriplet();
            //House Search
            houseTriplet.SearchCriteria.SearchByHouseName = ("House_");
            var searchResult = houseTriplet.SearchCriteria.Search().FirstOrDefault();
        }

        [WebDriverTest(TimeoutSeconds = 2000, Enabled = true, Groups = new[] { "All", "P2" }, Browsers = new[] { BrowserDefaults.Chrome }, DataProvider = "TC_MH_Data")]
        public void Delete_House(string[] basicDetails)
        {
            // Login as School Adminstrator
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);
            Wait.WaitForDocumentReady();
            // Navigate to Manage Houses Screen
            AutomationSugar.NavigateMenu("Tasks", "School Groups", "Manage Houses");
            Wait.WaitForDocumentReady();
            //Search House
            var houseTriplet = new ManageHouseTriplet();
            houseTriplet.SearchCriteria.SearchByHouseName = ("House_");
            var searchResult = houseTriplet.SearchCriteria.Search().FirstOrDefault();
            var manageHousePage = searchResult.Click<ManageHouseDetailPage>();
            //Deleting the House
            houseTriplet.Delete();
        }



        [WebDriverTest(TimeoutSeconds = 2000, Enabled = true, Groups = new[] { "All", "P2" }, Browsers = new[] { BrowserDefaults.Chrome })]
        public void House_E2E()
        {
            // Login as School Adminstrator
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);
            Wait.WaitForDocumentReady();
            // Navigate to Manage Houses Screen
            AutomationSugar.NavigateMenu("Tasks", "School Groups", "Manage Houses");
            Wait.WaitForDocumentReady();
            //Adding New House
            var houseTriplet = new ManageHouseTriplet();
            var houseDetailPage = houseTriplet.Create();
            houseDetailPage.HouseFullName = "To Be Deleted House";
            houseDetailPage.HouseShortName = "HS_Name";
            //Saving new house
            houseDetailPage.Save();
            Assert.AreEqual(false, houseDetailPage.IsSuccessMessageDisplayed(), "House record saved");
            //Searching the House
            houseTriplet.SearchCriteria.SearchByHouseName = ("To Be Deleted House");
            var searchResult = houseTriplet.SearchCriteria.Search().FirstOrDefault();
            var manageHousePage = searchResult.Click<ManageHouseDetailPage>();
            //Deleting the House
            houseTriplet.Delete();
        }

        [WebDriverTest(TimeoutSeconds = 2000, Enabled = true, Groups = new[] { "All", "P2" }, Browsers = new[] { BrowserDefaults.Chrome })]
        public void House_FullName_Validation()
        {
            // Login as School Adminstrator
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);
            Wait.WaitForDocumentReady();
            // Navigate to Manage Houses Screen
            AutomationSugar.NavigateMenu("Tasks", "School Groups", "Manage Houses");
            Wait.WaitForDocumentReady();
            //Adding New House
            var houseTriplet = new ManageHouseTriplet();
            var houseDetailPage = houseTriplet.Create();
            houseDetailPage.HouseFullName = "";
            houseDetailPage.HouseShortName = "HS_Name";
            //Saving new house
            houseDetailPage.Save();
            var ValidationWarning = SeleniumHelper.Get(ManageHouseDetailPage.ValidationWarning);
            Assert.IsTrue(ValidationWarning.Displayed, "Validation Warning");
        }

        [WebDriverTest(TimeoutSeconds = 2000, Enabled = true, Browsers = new[] { BrowserDefaults.Chrome })]
        public void House_Short_Name_Validation()
        {
            // Login as School Adminstrator
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);
            Wait.WaitForDocumentReady();
            // Navigate to Manage Houses Screen
            AutomationSugar.NavigateMenu("Tasks", "School Groups", "Manage Houses");
            Wait.WaitForDocumentReady();
            //Adding New House
            var houseTriplet = new ManageHouseTriplet();
            var houseDetailPage = houseTriplet.Create();
            houseDetailPage.HouseFullName = "House_1";
            houseDetailPage.HouseShortName = "";
            //Saving new house
            houseDetailPage.Save();
            var ValidationWarning = SeleniumHelper.Get(ManageHouseDetailPage.ValidationWarning);
            Assert.IsTrue(ValidationWarning.Displayed, "Validation Warning");
        }
        public List<object[]> TC_MH_Data()
        {
            string houseFullName = "House_" + SeleniumHelper.GenerateRandomString(9);
            string houseShortName = "HS_" + SeleniumHelper.GenerateRandomString(7);
            string colour = "Blue";            

            var res = new List<Object[]>
            {
                new object[]
                {
                    // Basic Detail
                    new string[]{ houseFullName, houseShortName, colour},
                }
            };
            return res;
        }
    }
}
