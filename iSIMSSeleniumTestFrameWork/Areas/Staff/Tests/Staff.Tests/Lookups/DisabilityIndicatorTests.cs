using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SeSugar;
using SeSugar.Automation;
using SeSugar.Data;
using Staff.POM.Components.Staff;
using Staff.POM.Helper;
using TestSettings;
using WebDriverRunner.internals;
using Environment = SeSugar.Environment;
using Selene.Support.Attributes;

namespace Staff.Tests.Lookups
{
    [TestClass]
    public class DisabilityIndicatorTests
    {
        private int tenantID { get { return Environment.Settings.TenantId; } }
        private readonly string menuRoute = "Disability Indicator";

        #region Read existing Disability Indicator

        [TestMethod]
        [ChromeUiTest("DisabilityIndicator", "P1", "Read_existing_Disability_Indicator_as_PO")]
        public void Read_existing_disability_Indicator_Lookup_as_PO()
        {
            //Arrange
            string code = Utilities.GenerateRandomString(5);
            string description = Utilities.GenerateRandomString(10);
            const string displayOrder = "1";

            using (new DataSetup(GetDisabilityIndicator(code, description, displayOrder)))
            {
                //Act
                LoginAndNavigate(SeleniumHelper.iSIMSUserType.PersonnelOfficer, menuRoute);
                var disabilityIndicatorDetail = Search(code);
                var gridRow = disabilityIndicatorDetail.DisabilityIndicatorRows.Rows.First(x => x.Code == code);

                //Assert
                Assert.AreEqual(code, gridRow.Code);
                Assert.AreEqual(description, gridRow.Description);
                Assert.AreEqual(displayOrder, gridRow.DisplayOrder);
                Assert.AreEqual(true, gridRow.IsVisible);
            }
        }

        #endregion

        #region Data Setup

        private DataPackage GetDisabilityIndicator(string code, string description, string displayOrder)
        {
            return this.BuildDataPackage()
                .AddData("DisabilityIndicator", new
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

        private static void LoginAndNavigate(SeleniumHelper.iSIMSUserType profile, string menuRoute)
        {
            SeleniumHelper.Login(profile);
            AutomationSugar.NavigateMenu("Lookups", "Staff", menuRoute);
        }

        private static DisabilityIndicatorDetailsPage Search(string code)
        {
            DisabilityIndicatorTriplet disabilityIndicatorTriplet = new DisabilityIndicatorTriplet();
            disabilityIndicatorTriplet.SearchCriteria.CodeOrDescription = code;
            DisabilityIndicatorDetailsPage disabilityIndicatorDetail = disabilityIndicatorTriplet.SearchCriteria.Search<DisabilityIndicatorDetailsPage>();
            return disabilityIndicatorDetail;
        }

        #endregion
    }
}
