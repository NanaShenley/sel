using Microsoft.VisualStudio.TestTools.UnitTesting;
using SeSugar;
using SeSugar.Automation;
using SeSugar.Data;
using Staff.POM.Components.Staff;
using Staff.POM.Helper;
using System;
using System.Linq;
using WebDriverRunner.internals;
using Environment = SeSugar.Environment;

namespace Staff.Tests
{
    public class PayLevelTests
    {
        #region Private Parameters

        private int tenantID { get { return Environment.Settings.TenantId; } }

        #endregion

        #region Tests

        //[TestMethod][ChromeUiTest(new[] { "PayLevel", "P1", "Create_New_PayLevel_Lookup_as_PO",
        //  "EngStPri", "EngStSec", "EngStMult", "WelStPri", "WelStSec", "WelStMult" })]
        public void Create_New_PayLevel_Lookup_as_PO()
        {
            //Arrange
            string code = Utilities.GenerateRandomString(4);
            string description = Utilities.GenerateRandomString(6);
            const string displayOrder = "1";
            const string category = "Not known";

            //Act
            LoginAndNavigate();
            PayLevelDetailsPage page = PayLevelTriplet.Create();
            PayLevelDetailsPage.PayLevelRow gridRow = page.LookupGrid.Rows.Single(x => x.Code == "");

            gridRow.Code = code;
            gridRow.Description = description;
            gridRow.DisplayOrder = displayOrder;
            gridRow.IsVisible = true;

            page.ClickSave();
            var details = Search(code);
            gridRow = details.LookupGrid.Rows.First(x => x.Code == code);

            //Assert
            Assert.AreEqual(code, gridRow.Code);
            Assert.AreEqual(description, gridRow.Description);
            Assert.AreEqual(displayOrder, gridRow.DisplayOrder);
        }

        //[TestMethod][ChromeUiTest(new[] { "PayLevel", "P1", "Read_PayLevel_Lookup_as_PO",
        //    "EngStPri", "EngStSec", "EngStMult", "WelStPri", "WelStSec", "WelStMult" })]
        public void Read_PayLevel_Lookup_as_PO()
        {
            //Arrange
            string code = CoreQueries.GetColumnUniqueString("PayLevel", "Code", 4, tenantID);
            string description = CoreQueries.GetColumnUniqueString("PayLevel", "Description", 10, tenantID);
            const string displayOrder = "2";

            using (new DataSetup(GetDataPackage(code, description, displayOrder)))
            {
                //Act
                LoginAndNavigate();
                var details = Search(code);
                var gridRow = details.LookupGrid.Rows.FirstOrDefault(x => x.Code == code);

                //Assert
                Assert.AreEqual(code, gridRow.Code);
                Assert.AreEqual(description, gridRow.Description);
                Assert.AreEqual(displayOrder, gridRow.DisplayOrder);
            }
        }

        //[TestMethod][ChromeUiTest(new[] { "PayLevel", "P1", "Update_PayLevel_Lookup_as_PO",
        //    "EngStPri", "EngStSec", "EngStMult", "WelStPri", "WelStSec", "WelStMult" })]
        public void Update_PayLevel_Lookup_as_PO()
        {
            //Arrange
            string code = CoreQueries.GetColumnUniqueString("PayLevel", "Code", 4, tenantID);
            string description = CoreQueries.GetColumnUniqueString("PayLevel", "Description", 10, tenantID);
            const string displayOrder = "3";

            string newCode = CoreQueries.GetColumnUniqueString("PayLevel", "Code", 4, tenantID);
            string newDescription = CoreQueries.GetColumnUniqueString("PayLevel", "Description", 10, tenantID);
            const string newDisplayOrder = "4";

            using (new DataSetup(GetDataPackage(code, description, displayOrder)))
            {
                //Act
                LoginAndNavigate();
                var details = Search(code);
                var gridRow = details.LookupGrid.Rows.FirstOrDefault(x => x.Code == code && x.Description == description);

                gridRow.Code = newCode;
                gridRow.Description = newDescription;
                gridRow.DisplayOrder = newDisplayOrder;

                details.ClickSave();
                details = Search(newCode);
                gridRow = details.LookupGrid.Rows.FirstOrDefault(x => x.Code == newCode);

                //Assert
                Assert.AreEqual(newCode, gridRow.Code);
                Assert.AreEqual(newDescription, gridRow.Description);
                Assert.AreEqual(newDisplayOrder, gridRow.DisplayOrder);
            }
        }

        //[TestMethod][ChromeUiTest(new[] { "PayLevel", "P1", "Delete_PayLevel_Lookup_as_PO",
        //    "EngStPri", "EngStSec", "EngStMult", "WelStPri", "WelStSec", "WelStMult" })]
        public void Delete_PayLevel_Lookup_as_PO()
        {
            //Arrange
            string code = CoreQueries.GetColumnUniqueString("PayLevel", "Code", 4, tenantID);
            string description = CoreQueries.GetColumnUniqueString("PayLevel", "Description", 10, tenantID);
            const string displayOrder = "4";

            using (new DataSetup(GetDataPackage(code, description, displayOrder)))
            {
                //Act
                LoginAndNavigate();
                var details = Search(code);
                var gridRow = details.LookupGrid.Rows.FirstOrDefault(x => x.Code == code);
                gridRow.DeleteRow();
                details.ClickSave();
                details = Search(code);
                gridRow = details.LookupGrid.Rows.FirstOrDefault(x => x.Code == code);

                //Assert
                Assert.AreEqual(null, gridRow);
            }
        }

        #endregion

        #region Helpers

        private static void LoginAndNavigate()
        {
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.PersonnelOfficer);
            AutomationSugar.NavigateMenu("Lookups", "Staff", "Salary Range Pay Level");
        }

        private static PayLevelDetailsPage Search(string code)
        {
            PayLevelTriplet triplet = new PayLevelTriplet();
            triplet.SearchCriteria.CodeOrDescription = code;
            PayLevelDetailsPage details = triplet.SearchCriteria.Search<PayLevelDetailsPage>();
            return details;
        }

        #endregion

        #region Data Setup

        private DataPackage GetDataPackage(string code, string description, string displayOrder)
        {
            return this.BuildDataPackage()
                .AddData("PayLevel", new
                {
                    ID = Guid.NewGuid(),
                    Code = code,
                    Description = description,
                    DisplayOrder = displayOrder,
                    IsVisible = true,
                    ResourceProvider = CoreQueries.GetSchoolId(),
                    TenantID = tenantID
                });
        }

        #endregion
    }
}
