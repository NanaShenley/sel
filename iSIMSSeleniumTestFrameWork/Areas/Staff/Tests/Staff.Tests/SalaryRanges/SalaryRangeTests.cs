using Microsoft.VisualStudio.TestTools.UnitTesting;
using SeSugar;
using SeSugar.Automation;
using SeSugar.Data;
using Staff.POM.Components.Staff.Pages;
using Staff.POM.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using WebDriverRunner.internals;
using Selene.Support.Attributes;

namespace Staff.Tests.SalaryRanges
{
    /// <summary>
    /// Salary Range Tests
    /// </summary>
    [TestClass]
    public class SalaryRangeTests
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
        #region Tests - SSJ - Temporarily removed as we can only run against Lab-Two:NI variant

        /// <summary>
        /// Chrome test - create new service agreement for existing staff as po.
        /// </summary>
        //[TestMethod][ChromeUiTest("Chrome", "SalaryRange", "P1", "Chrome_Create_new_Salary_Range_as_PO")]
        public void Chrome_Create_New_Salary_Range_As_PO()
        {
            Create_New_Salary_Range_As_PO();
        }

        /// <summary>
        /// Chrome_s the read_ new_ salary_ range_ as_ po.
        /// </summary>
        //[TestMethod][ChromeUiTest("Chrome", "SalaryRange", "P1", "Chrome_Read_New_Salary_Range_As_PO")]
        public void Chrome_Read_New_Salary_Range_As_PO()
        {
            Read_Salary_Range_As_PO();
        }

        /// <summary>
        /// Chrome_s the update_ salary_ range_ as_ po.
        /// </summary>
        //[TestMethod][ChromeUiTest("Chrome", "SalaryRange", "P1", "Chrome_Update_Salary_Range_As_PO")]
        public void Chrome_Update_Salary_Range_As_PO()
        {
            Update_Salary_Range_As_PO();
        }

        /// <summary>
        /// TODO - This is a placeholder for when we do the delete functionality.
        /// </summary>
        //[TestMethod][ChromeUiTest("Chrome", "SalaryRange", "P1", "Chrome_Delete_Salary_Range_As_PO")]
        //public void Chrome_Delete_Salary_Range_As_PO()
        //{
        //    Delete_Salary_Range_As_PO();
        //}

        //[IeUiTest("IE", "SalaryRange", "P1", "IE_Create_new_Salary_Range_as_PO")]
        public void Ie_Create_New_Salary_Range_As_PO()
        {
            Create_New_Salary_Range_As_PO();
        }

        /// <summary>
        /// Ie_s the read_ new_ salary_ range_ as_ po.
        /// </summary>
        //[IeUiTest("IE", "SalaryRange", "P1", "IE_Read_New_Salary_Range_As_PO")]
        public void Ie_Read_New_Salary_Range_As_PO()
        {
            Read_Salary_Range_As_PO();
        }

        /// <summary>
        /// Ie_s the update_ salary_ range_ as_ po.
        /// </summary>
        //[IeUiTest("IE", "SalaryRange", "P1", "IE_Update_Salary_Range_As_PO")]
        public void Ie_Update_Salary_Range_As_PO()
        {
            Update_Salary_Range_As_PO();
        }
        /// <summary>
        /// TODO - This is a placeholder for when we do the delete functionality.
        /// </summary>
        /// <param name="code">The code.</param>
        /// <param name="description">The description.</param>
        /// <param name="startDate">The start date.</param>
        /// <param name="awardDate">The award date.</param>
        /// <param name="maximumAmount">The maximum amount.</param>
        /// <param name="minimumAmount">The minimum amount.</param>
        /// <returns></returns>
        //[IeUiTest("IE", "SalaryRange", "P1", "IE_Delete_Salary_Range_As_PO")]
        //public void Chrome_Delete_Salary_Range_As_PO()
        //{
        //    Delete_Salary_Range_As_PO();
        //}

        #region Validation Tests

        //[TestMethod][ChromeUiTest("18100", "SalaryRange", "P1", "PersonnelOfficer",
        //    "EngStPri", "EngStSec", "EngStMult",
        //    "WelStPri", "WelStSec", "WelStMult")]
        public void Validate_new_Salary_Range_as_PO()
        {
            //Arrange
            //Act
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.PersonnelOfficer, "SalaryRange");
            AutomationSugar.NavigateMenu("Tasks", "Staff", "Salary Ranges");
            AutomationSugar.WaitForAjaxCompletion();

            SalaryRangePage salaryRangePage = new SalaryRangePage();
            salaryRangePage.AddSalaryRangeButtonClick();
            salaryRangePage.ClickSave();

            List<string> validation = salaryRangePage.Validation.ToList();

            //Assert
            Assert.IsTrue(validation.Contains("Code is required."));
            Assert.IsTrue(validation.Contains("Description is required."));
            Assert.IsTrue(validation.Contains("Pay Level is required."));
        }

        //[TestMethod][ChromeUiTest("18100", "SalaryRange", "P1", "SchoolAdministrator",
        //    "EngStPri", "EngStSec", "EngStMult",
        //    "WelStPri", "WelStSec", "WelStMult")]
        public void Validate_new_Salary_Range_as_SA()
        {
            //Arrange
            //Act
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator, "SalaryRange");
            AutomationSugar.NavigateMenu("Tasks", "Staff", "Salary Ranges");
            AutomationSugar.WaitForAjaxCompletion();

            SalaryRangePage salaryRangePage = new SalaryRangePage();
            salaryRangePage.AddSalaryRangeButtonClick();
            salaryRangePage.ClickSave();

            List<string> validation = salaryRangePage.Validation.ToList();

            //Assert
            Assert.IsTrue(validation.Contains("Code is required."));
            Assert.IsTrue(validation.Contains("Description is required."));
            Assert.IsTrue(validation.Contains("Pay Level is required."));
        }

        //[TestMethod][ChromeUiTest("18100", "SalaryRange", "P1", "PersonnelOfficer",
        //   "EngStPri", "EngStSec", "EngStMult")]
        public void Validate_Regional_Weighting_as_PO()
        {
            //Arrange
            //Act
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.PersonnelOfficer, "SalaryRange");
            AutomationSugar.NavigateMenu("Tasks", "Staff", "Salary Ranges");
            AutomationSugar.WaitForAjaxCompletion();

            SalaryRangePage salaryRangePage = new SalaryRangePage();
            salaryRangePage.AddSalaryRangeButtonClick();
            salaryRangePage.ClickSave();

            List<string> validation = salaryRangePage.Validation.ToList();

            //Assert
            Assert.IsTrue(validation.Contains("Regional Weighting is required."));
        }

        //[TestMethod][ChromeUiTest("18100", "SalaryRange", "P1", "SchoolAdministrator",
        //   "EngStPri", "EngStSec", "EngStMult")]
        public void Validate_Regional_Weighting_as_SA()
        {
            //Arrange
            //Act
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator, "SalaryRange");
            AutomationSugar.NavigateMenu("Tasks", "Staff", "Salary Ranges");
            AutomationSugar.WaitForAjaxCompletion();

            SalaryRangePage salaryRangePage = new SalaryRangePage();
            salaryRangePage.AddSalaryRangeButtonClick();
            salaryRangePage.ClickSave();

            List<string> validation = salaryRangePage.Validation.ToList();

            //Assert
            Assert.IsTrue(validation.Contains("Regional Weighting is required."));
        }

        //[TestMethod][ChromeUiTest("18100", "SalaryRange", "P1", "PersonnelOfficer",
        //    "EngStPri", "EngStSec", "EngStMult",
        //    "WelStPri", "WelStSec", "WelStMult")]
        public void Validate_at_least_one_Salary_Award_as_PO()
        {
            //Arrange
            string code = Utilities.GenerateRandomString(5, "CB");
            string description = Utilities.GenerateRandomString(10, "CB");

            //Act
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.PersonnelOfficer, "SalaryRange");
            AutomationSugar.NavigateMenu("Tasks", "Staff", "Salary Ranges");
            AutomationSugar.WaitForAjaxCompletion();

            SalaryRangePage salaryRangePage = new SalaryRangePage();
            salaryRangePage.AddSalaryRangeButtonClick();
            salaryRangePage.Code = code;
            salaryRangePage.Description = description;
            salaryRangePage.PayLevel = "Leadership";
            salaryRangePage.RegionalWeighting = "Inner London";
            salaryRangePage.ClickSave();

            List<string> validation = salaryRangePage.Validation.ToList();

            //Assert
            Assert.IsTrue(validation.Contains("Salary Range must have at least one Salary Award."));
        }

        //[TestMethod][ChromeUiTest("18100", "SalaryRange", "P1", "SchoolAdministrator",
        //    "EngStPri", "EngStSec", "EngStMult",
        //    "WelStPri", "WelStSec", "WelStMult")]
        public void Validate_at_least_one_Salary_Award_as_SA()
        {
            //Arrange
            string code = Utilities.GenerateRandomString(5, "CB");
            string description = Utilities.GenerateRandomString(10, "CB");

            //Act
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator, "SalaryRange");
            AutomationSugar.NavigateMenu("Tasks", "Staff", "Salary Ranges");
            AutomationSugar.WaitForAjaxCompletion();

            SalaryRangePage salaryRangePage = new SalaryRangePage();
            salaryRangePage.AddSalaryRangeButtonClick();
            salaryRangePage.Code = code;
            salaryRangePage.Description = description;
            salaryRangePage.PayLevel = "Leadership";
            salaryRangePage.RegionalWeighting = "Inner London";
            salaryRangePage.ClickSave();

            List<string> validation = salaryRangePage.Validation.ToList();

            //Assert
            Assert.IsTrue(validation.Contains("Salary Range must have at least one Salary Award."));
        }

        //[TestMethod][ChromeUiTest("18100", "SalaryAward", "P1", "PersonnelOfficer",
        //    "EngStPri", "EngStSec", "EngStMult",
        //    "WelStPri", "WelStSec", "WelStMult")]
        public void Validate_new_Salary_Award_as_PO()
        {
            //Arrange
            //Act
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.PersonnelOfficer, "SalaryRange");
            AutomationSugar.NavigateMenu("Tasks", "Staff", "Salary Ranges");
            AutomationSugar.WaitForAjaxCompletion();

            SalaryRangePage salaryRangePage = new SalaryRangePage();
            salaryRangePage.AddSalaryRangeButtonClick();
            salaryRangePage.AddSalaryAwardButtonClick();

            var gridRow = salaryRangePage.SalaryAwardStandardTable.Rows.First();
            gridRow.AwardDate = "";
            gridRow.MinimumAmount = "";
            gridRow.MaximumAmount = "";
            salaryRangePage.ClickSave();

            List<string> validation = salaryRangePage.Validation.ToList();

            //Assert
            Assert.IsTrue(validation.Contains("Salary Award Award Date is required."));
            Assert.IsTrue(validation.Contains("Salary Award Minimum Amount is required."));
            Assert.IsTrue(validation.Contains("Salary Award Maximum Amount is required."));
        }

        //[TestMethod][ChromeUiTest("18100", "SalaryAward", "P1", "SchoolAdministrator",
        //    "EngStPri", "EngStSec", "EngStMult",
        //    "WelStPri", "WelStSec", "WelStMult")]
        public void Validate_new_Salary_Award_as_SA()
        {
            //Arrange
            //Act
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator, "SalaryRange");
            AutomationSugar.NavigateMenu("Tasks", "Staff", "Salary Ranges");
            AutomationSugar.WaitForAjaxCompletion();

            SalaryRangePage salaryRangePage = new SalaryRangePage();
            salaryRangePage.AddSalaryRangeButtonClick();
            salaryRangePage.AddSalaryAwardButtonClick();

            var gridRow = salaryRangePage.SalaryAwardStandardTable.Rows.First();
            gridRow.AwardDate = "";
            gridRow.MinimumAmount = "";
            gridRow.MaximumAmount = "";
            salaryRangePage.ClickSave();

            List<string> validation = salaryRangePage.Validation.ToList();

            //Assert
            Assert.IsTrue(validation.Contains("Salary Award Award Date is required."));
            Assert.IsTrue(validation.Contains("Salary Award Minimum Amount is required."));
            Assert.IsTrue(validation.Contains("Salary Award Maximum Amount is required."));
        }

        //[TestMethod][ChromeUiTest("18100", "SalaryAward", "P1", "PersonnelOfficer",
        //   "EngStPri", "EngStSec", "EngStMult",
        //   "WelStPri", "WelStSec", "WelStMult")]
        public void Validate_Salary_Award_Minimum_Amount_Decimal_as_PO()
        {
            //Arrange
            //Act
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.PersonnelOfficer, "SalaryRange");
            AutomationSugar.NavigateMenu("Tasks", "Staff", "Salary Ranges");
            AutomationSugar.WaitForAjaxCompletion();

            SalaryRangePage salaryRangePage = new SalaryRangePage();
            salaryRangePage.AddSalaryRangeButtonClick();
            salaryRangePage.AddSalaryAwardButtonClick();

            var gridRow = salaryRangePage.SalaryAwardStandardTable.Rows.First();
            gridRow.MinimumAmount = "99999999999999999999.999";
            salaryRangePage.ClickSave();

            List<string> validation = salaryRangePage.Validation.ToList();

            //Assert
            Assert.IsTrue(validation.Contains("Salary Award Minimum Amount may have only 2 figure(s) after the decimal point."));
        }

        //[TestMethod][ChromeUiTest("18100", "SalaryAward", "P1", "PersonnelOfficer",
        //    "EngStPri", "EngStSec", "EngStMult",
        //    "WelStPri", "WelStSec", "WelStMult")]
        public void Validate_Salary_Award_Minimum_Amount_as_PO()
        {
            //Arrange
            //Act
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.PersonnelOfficer, "SalaryRange");
            AutomationSugar.NavigateMenu("Tasks", "Staff", "Salary Ranges");
            AutomationSugar.WaitForAjaxCompletion();

            SalaryRangePage salaryRangePage = new SalaryRangePage();
            salaryRangePage.AddSalaryRangeButtonClick();
            salaryRangePage.AddSalaryAwardButtonClick();

            var gridRow = salaryRangePage.SalaryAwardStandardTable.Rows.First();
            gridRow.MinimumAmount = "99999999999999999999.99";
            salaryRangePage.ClickSave();

            List<string> validation = salaryRangePage.Validation.ToList();

            //Assert
            Assert.IsTrue(validation.Contains("Salary Award Minimum Amount cannot be more than 9999999999999999999.99."));
        }

        //[TestMethod][ChromeUiTest("18100", "SalaryAward", "P1", "SchoolAdministrator",
        //"EngStPri", "EngStSec", "EngStMult",
        //"WelStPri", "WelStSec", "WelStMult")]
        public void Validate_Salary_Award_Minimum_Amount_Decimal_as_SA()
        {
            //Arrange
            //Act
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator, "SalaryRange");
            AutomationSugar.NavigateMenu("Tasks", "Staff", "Salary Ranges");
            AutomationSugar.WaitForAjaxCompletion();

            SalaryRangePage salaryRangePage = new SalaryRangePage();
            salaryRangePage.AddSalaryRangeButtonClick();
            salaryRangePage.AddSalaryAwardButtonClick();

            var gridRow = salaryRangePage.SalaryAwardStandardTable.Rows.First();
            gridRow.MinimumAmount = "99999999999999999999.999";
            salaryRangePage.ClickSave();

            List<string> validation = salaryRangePage.Validation.ToList();

            //Assert
            Assert.IsTrue(validation.Contains("Salary Award Minimum Amount may have only 2 figure(s) after the decimal point."));
        }

        //[TestMethod][ChromeUiTest("18100", "SalaryAward", "P1", "SchoolAdministrator",
        //    "EngStPri", "EngStSec", "EngStMult",
        //    "WelStPri", "WelStSec", "WelStMult")]
        public void Validate_Salary_Award_Minimum_Amount_as_SA()
        {
            //Arrange
            //Act
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator, "SalaryRange");
            AutomationSugar.NavigateMenu("Tasks", "Staff", "Salary Ranges");
            AutomationSugar.WaitForAjaxCompletion();

            SalaryRangePage salaryRangePage = new SalaryRangePage();
            salaryRangePage.AddSalaryRangeButtonClick();
            salaryRangePage.AddSalaryAwardButtonClick();

            var gridRow = salaryRangePage.SalaryAwardStandardTable.Rows.First();
            gridRow.MinimumAmount = "99999999999999999999.99";
            salaryRangePage.ClickSave();

            List<string> validation = salaryRangePage.Validation.ToList();

            //Assert
            Assert.IsTrue(validation.Contains("Salary Award Minimum Amount cannot be more than 9999999999999999999.99."));
        }

        //[TestMethod][ChromeUiTest("18100", "SalaryAward", "P1", "PersonnelOfficer",
        // "EngStPri", "EngStSec", "EngStMult",
        // "WelStPri", "WelStSec", "WelStMult")]
        public void Validate_Salary_Award_Maximum_Amount_Decimal_as_PO()
        {
            //Arrange
            //Act
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.PersonnelOfficer, "SalaryRange");
            AutomationSugar.NavigateMenu("Tasks", "Staff", "Salary Ranges");
            AutomationSugar.WaitForAjaxCompletion();

            SalaryRangePage salaryRangePage = new SalaryRangePage();
            salaryRangePage.AddSalaryRangeButtonClick();
            salaryRangePage.AddSalaryAwardButtonClick();

            var gridRow = salaryRangePage.SalaryAwardStandardTable.Rows.First();
            gridRow.MaximumAmount = "99999999999999999999.999";
            salaryRangePage.ClickSave();

            List<string> validation = salaryRangePage.Validation.ToList();

            //Assert
            Assert.IsTrue(validation.Contains("Salary Award Maximum Amount may have only 2 figure(s) after the decimal point."));
        }

        //[TestMethod][ChromeUiTest("18100", "SalaryAward", "P1", "PersonnelOfficer",
        //    "EngStPri", "EngStSec", "EngStMult",
        //    "WelStPri", "WelStSec", "WelStMult")]
        public void Validate_Salary_Award_Maximum_Amount_as_PO()
        {
            //Arrange
            //Act
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.PersonnelOfficer, "SalaryRange");
            AutomationSugar.NavigateMenu("Tasks", "Staff", "Salary Ranges");
            AutomationSugar.WaitForAjaxCompletion();

            SalaryRangePage salaryRangePage = new SalaryRangePage();
            salaryRangePage.AddSalaryRangeButtonClick();
            salaryRangePage.AddSalaryAwardButtonClick();

            var gridRow = salaryRangePage.SalaryAwardStandardTable.Rows.First();
            gridRow.MaximumAmount = "99999999999999999999.99";
            salaryRangePage.ClickSave();

            List<string> validation = salaryRangePage.Validation.ToList();

            //Assert
            Assert.IsTrue(validation.Contains("Salary Award Maximum Amount cannot be more than 9999999999999999999.99."));
        }

        //[TestMethod][ChromeUiTest("18100", "SalaryAward", "P1", "SchoolAdministrator",
        //"EngStPri", "EngStSec", "EngStMult",
        //"WelStPri", "WelStSec", "WelStMult")]
        public void Validate_Salary_Award_Maximum_Amount_Decimal_as_SA()
        {
            //Arrange
            //Act
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator, "SalaryRange");
            AutomationSugar.NavigateMenu("Tasks", "Staff", "Salary Ranges");
            AutomationSugar.WaitForAjaxCompletion();

            SalaryRangePage salaryRangePage = new SalaryRangePage();
            salaryRangePage.AddSalaryRangeButtonClick();
            salaryRangePage.AddSalaryAwardButtonClick();

            var gridRow = salaryRangePage.SalaryAwardStandardTable.Rows.First();
            gridRow.MaximumAmount = "99999999999999999999.999";
            salaryRangePage.ClickSave();

            List<string> validation = salaryRangePage.Validation.ToList();

            //Assert
            Assert.IsTrue(validation.Contains("Salary Award Maximum Amount may have only 2 figure(s) after the decimal point."));
        }

        //[TestMethod][ChromeUiTest("18100", "SalaryAward", "P1", "SchoolAdministrator",
        //    "EngStPri", "EngStSec", "EngStMult",
        //    "WelStPri", "WelStSec", "WelStMult")]
        public void Validate_Salary_Award_Maximum_Amount_as_SA()
        {
            //Arrange
            //Act
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator, "SalaryRange");
            AutomationSugar.NavigateMenu("Tasks", "Staff", "Salary Ranges");
            AutomationSugar.WaitForAjaxCompletion();

            SalaryRangePage salaryRangePage = new SalaryRangePage();
            salaryRangePage.AddSalaryRangeButtonClick();
            salaryRangePage.AddSalaryAwardButtonClick();

            var gridRow = salaryRangePage.SalaryAwardStandardTable.Rows.First();
            gridRow.MaximumAmount = "99999999999999999999.99";
            salaryRangePage.ClickSave();

            List<string> validation = salaryRangePage.Validation.ToList();

            //Assert
            Assert.IsTrue(validation.Contains("Salary Award Maximum Amount cannot be more than 9999999999999999999.99."));
        }

        //[TestMethod][ChromeUiTest("18100", "SalaryAward", "P1", "PersonnelOfficer",
        //    "EngStPri", "EngStSec", "EngStMult",
        //    "WelStPri", "WelStSec", "WelStMult")]
        public void Validate_Salary_Award_Maximum_Amount_greater_than_Minimum_Amount_as_PO()
        {
            //Arrange
            string code = Utilities.GenerateRandomString(5, "CB");
            string description = Utilities.GenerateRandomString(10, "CB");

            //Act
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.PersonnelOfficer, "SalaryRange");
            AutomationSugar.NavigateMenu("Tasks", "Staff", "Salary Ranges");
            AutomationSugar.WaitForAjaxCompletion();

            SalaryRangePage salaryRangePage = new SalaryRangePage();
            salaryRangePage.AddSalaryRangeButtonClick();
            salaryRangePage.Code = code;
            salaryRangePage.Description = description;
            salaryRangePage.PayLevel = "Leadership";
            salaryRangePage.RegionalWeighting = "Inner London";

            salaryRangePage.AddSalaryAwardButtonClick();

            var gridRow = salaryRangePage.SalaryAwardStandardTable.Rows.First();
            gridRow.MinimumAmount = "99.99";
            gridRow.MaximumAmount = "88.88";
            salaryRangePage.ClickSave();

            List<string> validation = salaryRangePage.Validation.ToList();

            //Assert
            Assert.IsTrue(validation.Contains("Salary Award Maximum Amount must be greater than the Minimum Amount."));
        }

        //[TestMethod][ChromeUiTest("18100", "SalaryAward", "P1", "SchoolAdministrator",
        //    "EngStPri", "EngStSec", "EngStMult",
        //    "WelStPri", "WelStSec", "WelStMult")]
        public void Validate_Salary_Award_Maximum_Amount_greater_than_Minimum_Amount_as_SA()
        {
            //Arrange
            string code = Utilities.GenerateRandomString(5, "CB");
            string description = Utilities.GenerateRandomString(10, "CB");

            //Act
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator, "SalaryRange");
            AutomationSugar.NavigateMenu("Tasks", "Staff", "Salary Ranges");
            AutomationSugar.WaitForAjaxCompletion();

            SalaryRangePage salaryRangePage = new SalaryRangePage();
            salaryRangePage.AddSalaryRangeButtonClick();
            salaryRangePage.Code = code;
            salaryRangePage.Description = description;
            salaryRangePage.PayLevel = "Leadership";
            salaryRangePage.RegionalWeighting = "Inner London";

            salaryRangePage.AddSalaryAwardButtonClick();

            var gridRow = salaryRangePage.SalaryAwardStandardTable.Rows.First();
            gridRow.MinimumAmount = "99.99";
            gridRow.MaximumAmount = "88.88";
            salaryRangePage.ClickSave();

            List<string> validation = salaryRangePage.Validation.ToList();

            //Assert
            Assert.IsTrue(validation.Contains("Salary Award Maximum Amount must be greater than the Minimum Amount."));
        }

        #endregion
      
        #endregion

        #region TestImplementation


        private DataPackage CreatePayLevelAndRegionalWeighting(
            out Guid payLevelId, 
            out Guid regionalWeightingId, 
            string payLevelCode = "",
            string payLevelDescription = "",
            string regionalWeightingCode = "",
            string regionalWeightingDescription = ""
            )
        {
            DataPackage testData = new DataPackage();

            if ( (string.IsNullOrWhiteSpace(payLevelCode))) payLevelCode = Utilities.GenerateRandomString(20);
            if ((string.IsNullOrWhiteSpace(payLevelDescription))) payLevelDescription = Utilities.GenerateRandomString(200);

            if ((string.IsNullOrWhiteSpace(regionalWeightingCode))) regionalWeightingCode = Utilities.GenerateRandomString(20);
            if ((string.IsNullOrWhiteSpace(regionalWeightingDescription))) regionalWeightingDescription = Utilities.GenerateRandomString(200);

            testData.AddData("PayLevel", DataPackageHelper.GeneratePayLevel(out payLevelId, payLevelCode, payLevelDescription));

            testData.AddData("RegionalWeighting", DataPackageHelper.GenerateRegionalWeighting(out regionalWeightingId, regionalWeightingCode, regionalWeightingDescription));

            return testData;
        }

        private DataPackage CreateSalaryRangeAndAward(
            string code, 
            string description, 
            DateTime startDate, 
            DateTime awardDate, 
            decimal maximumAmount, 
            decimal minimumAmount,
            string payLevelCode = "",
            string payLevelDescription = "",
            string regionalWeightingCode = "",
            string regionalWeightingDescription = ""
            )
        {
            DataPackage testData = new DataPackage();

            Guid payLevelId;
            Guid regionalWeightingId;
            Guid salaryRangeId;
            Guid salaryAwardid;

            testData = CreatePayLevelAndRegionalWeighting(out  payLevelId, out  regionalWeightingId, payLevelCode, payLevelDescription, regionalWeightingCode, regionalWeightingDescription);
                
            testData.AddData("SalaryRange", DataPackageHelper.GenerateSalaryRange(out salaryRangeId, code, description, payLevelId, regionalWeightingId));
            
            testData.AddData("SalaryAward", DataPackageHelper.GenerateSalaryAward(out salaryAwardid, startDate,  maximumAmount, minimumAmount, salaryRangeId));

            return testData;
        }

        /// <summary>
        /// Create_s the new_ salary_ range_ as_ po.
        /// </summary>
        private void Create_New_Salary_Range_As_PO()
        {
            InitialiseToSalaryRangePage();
            Guid payLevelId, regionalId;

            string code = Utilities.GenerateRandomString(5, "KH");
            string description = Utilities.GenerateRandomString(97, "KH");
            string payLevel = Utilities.GenerateRandomString(11,"payLevel");
            string regionalWeighting = Utilities.GenerateRandomString(5,"regionalweight");
            string awardDate = DateTime.Now.AddDays(1).ToString("dd/MM/yyyy");
            string maxAmount = "200";
            string minAmount = "20";

            using (new DataSetup(CreatePayLevelAndRegionalWeighting(out payLevelId, out regionalId, payLevel, payLevel, regionalWeighting, regionalWeighting)))
            {
                SalaryRangePage salaryRangePage = new SalaryRangePage();
                salaryRangePage.AddSalaryRangeButtonClick();
                salaryRangePage.Refresh();
                salaryRangePage.Code = code;
                salaryRangePage.Description = description;
                salaryRangePage.PayLevel = payLevel;
                salaryRangePage.RegionalWeighting = regionalWeighting;
                salaryRangePage.AddSalaryAwardButtonClick();
                var row = salaryRangePage.SalaryAwardStandardTable.Rows[0];
                row.AwardDate = awardDate;
                row.MinimumAmount = minAmount;
                row.MaximumAmount = maxAmount;

                salaryRangePage.ClickSave();

                SalaryRangeTriple triple = new SalaryRangeTriple();
                triple.SearchCriteria.Code = code;

                var tiles = triple.SearchCriteria.Search();

                var tile = tiles.Single(x => x.Name == code);

                tile.Click();

                SalaryRangePage salaryRangePageReopened = new SalaryRangePage();

                Assert.AreEqual(salaryRangePageReopened.Code, salaryRangePage.Code);
                Assert.AreEqual(salaryRangePageReopened.Description, salaryRangePage.Description);
                Assert.AreEqual(salaryRangePageReopened.PayLevel, salaryRangePage.PayLevel);
                Assert.AreEqual(salaryRangePageReopened.RegionalWeighting, salaryRangePage.RegionalWeighting);
                Assert.AreEqual(salaryRangePageReopened.SalaryAwardStandardTable.Rows[0].AwardDate, salaryRangePage.SalaryAwardStandardTable.Rows[0].AwardDate);
                Assert.AreEqual(salaryRangePageReopened.SalaryAwardStandardTable.Rows[0].MinimumAmount, salaryRangePage.SalaryAwardStandardTable.Rows[0].MinimumAmount);
                Assert.AreEqual(salaryRangePageReopened.SalaryAwardStandardTable.Rows[0].MaximumAmount, salaryRangePage.SalaryAwardStandardTable.Rows[0].MaximumAmount);
            }
        }
        /// <summary>
        /// Read_s the salary_ range_ as_ po.
        /// </summary>
        private void Read_Salary_Range_As_PO()
        {
            InitialiseToSalaryRangePage();

            string code = Utilities.GenerateRandomString(5, "KH");
            string description = Utilities.GenerateRandomString(97, "KH");
            string payLevel = Utilities.GenerateRandomString(10, "pay Level");
            string regionalWeighting = Utilities.GenerateRandomString(9, "regional w");
            DateTime awardDateTime = DateTime.Now.AddDays(1);
            string awardDate = awardDateTime.ToString("dd/MM/yyyy");
            decimal maximumAmount = 200;
            decimal minimumAmount = 20;
            string maxAmount = maximumAmount.ToString();
            string minAmount = minimumAmount.ToString();

            using (new DataSetup(CreateSalaryRangeAndAward(code, description, awardDateTime, awardDateTime, maximumAmount, minimumAmount)))
            {
                SalaryRangeTriple triple = new SalaryRangeTriple();
                triple.SearchCriteria.Code = code;
                var tiles = triple.SearchCriteria.Search().ToList();

                Assert.AreNotEqual(tiles.Count, 0);

                var tile = tiles.First();
                tile.Click();

                SalaryRangePage salaryRangePage = new SalaryRangePage();

                Assert.IsFalse(string.IsNullOrWhiteSpace(salaryRangePage.Code));
                Assert.AreEqual(salaryRangePage.Code, code);
            }
        }

        /// <summary>
        /// Update_s the salary_ range_ as_ po.
        /// </summary>
        private void Update_Salary_Range_As_PO()
        {
            InitialiseToSalaryRangePage();

            string code = Utilities.GenerateRandomString(5, "KH");
            string description = Utilities.GenerateRandomString(97, "KH");
            string payLevel = Utilities.GenerateRandomString(10, "pay Level");
            string regionalWeighting = Utilities.GenerateRandomString(9, "regional w");
            DateTime awardDateTime = DateTime.Now.AddDays(1);
            string awardDate = awardDateTime.ToString("dd/MM/yyyy");
            decimal maximumAmount = 200;
            decimal minimumAmount = 20;
            string maxAmount = maximumAmount.ToString();

            string minAmount = minimumAmount.ToString();

            using (new DataSetup(CreateSalaryRangeAndAward(code, description, awardDateTime, awardDateTime, maximumAmount, minimumAmount)))
            {
                SalaryRangeTriple triple = new SalaryRangeTriple();
                triple.SearchCriteria.Code = code;

                var tiles = triple.SearchCriteria.Search();
                var tile = tiles.Single(x => x.Name == code);
                tile.Click();

                SalaryRangePage salaryRangePage = new SalaryRangePage();

                string codeUpdate = Utilities.GenerateRandomString(5, "NH");
                string descriptionUpdate = Utilities.GenerateRandomString(97, "NH");

                salaryRangePage.Code = codeUpdate;
                salaryRangePage.Description = descriptionUpdate;
                salaryRangePage.ClickSave();

                triple.Refresh();

                triple.SearchCriteria.Code = string.Empty;
                var tilesUpdated = triple.SearchCriteria.Search();
                var tileUpdated = tilesUpdated.Where(x => x.Name == codeUpdate).First();

                tileUpdated.Click();

                triple.Refresh();

                SalaryRangePage salaryRangePageRefresh = new SalaryRangePage();

                Assert.AreEqual(salaryRangePageRefresh.Code, codeUpdate);
                Assert.AreEqual(salaryRangePageRefresh.Description, descriptionUpdate);
            }
        }

        /// <summary>
        /// TODO - This is a placeholder for when we do the delete functionality.
        /// </summary>
        private void Delete_Salary_Range_As_PO()
        {
            // InitialiseToSalaryRangePage();
        }

        #endregion

        #region helpers
        /// <summary>
        /// Initialises to salary range page.
        /// </summary>
        private void InitialiseToSalaryRangePage()
        {
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.PersonnelOfficer, "SalaryRange");
            AutomationSugar.NavigateMenu("Tasks", "Staff", "Salary Ranges"); 
            AutomationSugar.WaitForAjaxCompletion();
        }

        #endregion
    }
}
