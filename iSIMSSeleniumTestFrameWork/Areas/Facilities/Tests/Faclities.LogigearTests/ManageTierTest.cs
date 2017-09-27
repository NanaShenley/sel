using System;
using POM.Helper;
using NUnit.Framework;
using TestSettings;
using Selene.Support.Attributes;
using SeSugar.Automation;
using System.Collections.Generic;
using Facilities.POM.Components.ManageTier;
using SeSugar.Data;
using SeSugar;
using Facilities.Data;

namespace Faclities.LogigearTests
{
    public class ManageTierTest
    {
        [WebDriverTest(TimeoutSeconds = 2000, Enabled = true, Groups = new[] { "All", "P2" }, Browsers = new[] { BrowserDefaults.Chrome }, DataProvider = "TC_MT_Data")]
        public void Add_Tier(string[] basicDetails)
        {
            // Login as School Adminstrator
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator, "ManageTiers");
            AutomationSugar.NavigateMenu("Tasks", "School Groups", "Manage Tiers");
            Wait.WaitForDocumentReady();

            DataPackage dataPackage = this.BuildDataPackage();
            //Employee Details Data Injection
            var employeeId = Guid.NewGuid();
            dataPackage.AddEmployee(employeeId);
            //Staff Details Data Injection
            var staffId = Guid.NewGuid();
            var surname = Utilities.GenerateRandomString(10, "Surname");
            var forename = Utilities.GenerateRandomString(3, "Forename");
            dataPackage.AddStaff(staffId, employeeId, surname, forename);

            var serviceRecordId = Guid.NewGuid();
            var staffDOA = DateTime.Now;
            dataPackage.AddServiceRecord(serviceRecordId, staffId, staffDOA, null);

            var tierTriplet = new ManageTierTriplet();
            //Clear already present data
            var searchResult = tierTriplet.SearchCriteria.Search().FirstOrDefault().Click<ManageTierPage>();
            tierTriplet.Delete();
            Wait.WaitForDocumentReady();
            var tierpage = tierTriplet.Create();
            tierpage.TierFullName = basicDetails[0];
            tierpage.TierShortName = basicDetails[1];
            tierpage.AddActivehistory();
            Wait.WaitForDocumentReady();
            tierpage.ActiveHistoryTable[0].StartDate = "01/01/2016";
            tierpage.ActiveHistoryTable[0].EndDate = "12/12/2017";
            tierpage.AddYearGroup();
            Wait.WaitForDocumentReady();
            tierpage.YearGroupsTable[0].YearGroup = "Year 1";
            tierpage.YearGroupsTable[0].StartDate = "01/01/2016";
            tierpage.YearGroupsTable[0].EndDate = "12/12/2017";
            //tierpage.AddTierManager();
            //var tierStaff = string.Concat(surname, ", ", forename);
            //tierpage.TierManagerTable[0].SelectTierManager = tierStaff;
            //tierpage.TierManagerTable[0].StartDate = "01/01/2016";
            //tierpage.TierManagerTable[0].EndDate = "12/12/2017";
            tierpage.Save();
            Assert.AreEqual(false, tierpage.IsSuccessMessageDisplayed(), "Tier record saved");
        }

        [WebDriverTest(TimeoutSeconds = 2000, Enabled = true, Groups = new[] { "All", "P2" }, Browsers = new[] { BrowserDefaults.Chrome }, DataProvider = "TC_MT_Data")]
        public void Search_Tier(string[] basicDetails)
        {
            // Login as School Adminstrator
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator, "ManageTiers");
            AutomationSugar.NavigateMenu("Tasks", "School Groups", "Manage Tiers");
            Wait.WaitForDocumentReady();

            var tierTriplet = new ManageTierTriplet();
            var searchResult = tierTriplet.SearchCriteria.Search().FirstOrDefault().Click<ManageTierPage>();
        }

        [WebDriverTest(TimeoutSeconds = 2000, Enabled = true, Groups = new[] { "All", "P2" }, Browsers = new[] { BrowserDefaults.Chrome }, DataProvider = "TC_MT_Data")]
        public void Delete_Tier(string[] basicDetails)
        {
            // Login as School Adminstrator
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator, "ManageTiers");
            AutomationSugar.NavigateMenu("Tasks", "School Groups", "Manage Tiers");
            Wait.WaitForDocumentReady();

            var tierTriplet = new ManageTierTriplet();
            var searchResult = tierTriplet.SearchCriteria.Search().FirstOrDefault().Click<ManageTierPage>();
            tierTriplet.Delete();

        }

        [WebDriverTest(TimeoutSeconds = 2000, Enabled = true, Groups = new[] { "All", "P2" }, Browsers = new[] { BrowserDefaults.Chrome }, DataProvider = "TC_MT_Data")]
        public void Add_Tier_E2E(string[] basicDetails)
        {
            // Login as School Adminstrator
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator, "ManageTiers");
            AutomationSugar.NavigateMenu("Tasks", "School Groups", "Manage Tiers");
            Wait.WaitForDocumentReady();

            var tierTriplet = new ManageTierTriplet();
            var tierpage = tierTriplet.Create();
            tierpage.TierFullName = basicDetails[0];
            tierpage.TierShortName = basicDetails[1];
            tierpage.AddActivehistory();
            Wait.WaitForDocumentReady();
            tierpage.ActiveHistoryTable[0].StartDate = "01/01/2017";
            tierpage.ActiveHistoryTable[0].EndDate = "12/12/2018";
            tierpage.AddYearGroup();
            Wait.WaitForDocumentReady();
            tierpage.YearGroupsTable[0].YearGroup = "Year 3";
            tierpage.YearGroupsTable[0].StartDate = "01/01/2017";
            tierpage.YearGroupsTable[0].EndDate = "12/12/2018";
            tierpage.Save();
            Assert.AreEqual(false, tierpage.IsSuccessMessageDisplayed(), "Tier record saved");
            Wait.WaitForDocumentReady();
            var searchResult = tierTriplet.SearchCriteria.Search().FirstOrDefault().Click<ManageTierPage>();
            tierTriplet.Delete();
        }

        [WebDriverTest(TimeoutSeconds = 2000, Enabled = true, Groups = new[] { "All", "P2" }, Browsers = new[] { BrowserDefaults.Chrome }, DataProvider = "TC_MT_Data")]
        public void Tier_ActiveHistory_Validation(string[] basicDetails)
        {
            // Login as School Adminstrator
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator, "ManageTiers");
            AutomationSugar.NavigateMenu("Tasks", "School Groups", "Manage Tiers");
            Wait.WaitForDocumentReady();

            var tierTriplet = new ManageTierTriplet();
            var tierpage = tierTriplet.Create();
            tierpage.TierFullName = basicDetails[0];
            tierpage.TierShortName = basicDetails[1];
            tierpage.Save();
            var ValidationWarning = SeleniumHelper.Get(ManageTierPage.ValidationWarning);
            Assert.IsTrue(ValidationWarning.Displayed, "Validation Warning");
        }

        [WebDriverTest(TimeoutSeconds = 2000, Enabled = true, Groups = new[] { "All", "P2" }, Browsers = new[] { BrowserDefaults.Chrome }, DataProvider = "TC_MT_Data")]
        public void Tier_DateInRange_Validation(string[] basicDetails)
        {
            // Login as School Adminstrator
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator, "ManageTiers");
            AutomationSugar.NavigateMenu("Tasks", "School Groups", "Manage Tiers");
            Wait.WaitForDocumentReady();

            var tierTriplet = new ManageTierTriplet();
            var tierpage = tierTriplet.Create();
            tierpage.TierFullName = basicDetails[0];
            tierpage.TierShortName = basicDetails[1];
            tierpage.AddActivehistory();
            Wait.WaitForDocumentReady();
            tierpage.ActiveHistoryTable[0].StartDate = "01/01/2017";
            tierpage.ActiveHistoryTable[0].EndDate = "12/12/2018";
            tierpage.AddYearGroup();
            Wait.WaitForDocumentReady();
            tierpage.YearGroupsTable[0].YearGroup = "Year 3";
            tierpage.YearGroupsTable[0].StartDate = "01/01/2018";
            tierpage.YearGroupsTable[0].EndDate = "12/12/2019";
            tierpage.Save();
            var ValidationWarning = SeleniumHelper.Get(ManageTierPage.ValidationWarning);
            Assert.IsTrue(ValidationWarning.Displayed, "Validation Warning");
        }

        public List<object[]> TC_MT_Data()
        {
            string tierFullName = "Tier" + SeleniumHelper.GenerateRandomString(9);
            string tierShortName = "TS_" + SeleniumHelper.GenerateRandomString(7);

            var res = new List<Object[]>
            {
                new object[]
                {
                    // Basic Detail
                    new string[]{ tierFullName, tierShortName},
                }
            };
            return res;
        }
    }
}
