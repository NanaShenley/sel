using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using NUnit.Framework;
using PageObjectModel.Components.Admission;
using PageObjectModel.Helper;
using SeSugar;
using SeSugar.Data;
using TestSettings;
using Selene.Support.Attributes;
using SeSugar.Automation;
using PageObjectModel.Components.Common;
using Admissions.Data;

namespace Admissions.Application.Tests
{
    public class Tier4Lookups
    {
        /// <summary>
        /// Description: Exercise ability to add a Tier 4 Category lookup.
        /// Role: Admissions Officer
        /// </summary>

        [Variant(Variant.EnglishStateSecondary)]

        [WebDriverTest(TimeoutSeconds = 600, Enabled = true, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { "Verify_Tier4_Category_Lookup", "Tier4_Lookups" })]
        public void Verify_Tier4_Category_Lookup()
        {
            var code = SeleniumHelper.GenerateRandomString(3);

            try
            {
                FeatureBee.FeatureBeeLogin.LoginWithFeatureBee(new[] { "Tier4" }, FeatureBee.FeatureBeeLogin.iSIMSUserType.AdmissionsOfficer);
                AutomationSugar.NavigateMenu("Lookups", "Admissions", "Tier 4 Category");

                //Add new Reason Admission Rejected
                var tier4CategoryTriplet = new LookupTriplet();
                var tier4CategoryPage = tier4CategoryTriplet.AddLookupRow("Tier4Category");
                var tier4CategoryRow = tier4CategoryPage.TableRow.GetLastRow();

                tier4CategoryRow.Code = code;
                tier4CategoryRow.Description = string.Format("Selenium Test Entry - {0}", code);
                tier4CategoryRow.DisplayOrder = "999";
                tier4CategoryRow.IsVisible = true;

                //Save Reason Admission Rejected record
                tier4CategoryPage.ClickSave();

                //Verify success message
                Assert.AreEqual(true, tier4CategoryPage.IsSuccessMessageDisplayed(), "Success message do not display");
            }
            finally
            {
                // Tear down
                PurgeLinkedData.DeleteTier4Category(code);
            }
        }

        [Variant(Variant.EnglishStateSecondary)]
        /// <summary>
        /// Description: Exercise ability to add a Tier 4 Region lookup.
        /// Role: Admissions Officer
        /// </summary>
        [WebDriverTest(TimeoutSeconds = 600, Enabled = true, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { "Verify_Tier4_Region_Lookup", "Tier4_Lookups" })]
        public void Verify_Tier4_Region_Lookup()
        {
            var code = SeleniumHelper.GenerateRandomString(3);

            try
            {
                FeatureBee.FeatureBeeLogin.LoginWithFeatureBee(new[] { "Tier4" }, FeatureBee.FeatureBeeLogin.iSIMSUserType.AdmissionsOfficer);
                AutomationSugar.NavigateMenu("Lookups", "Admissions", "Tier 4 Region");

                //Add new Reason Admission Rejected
                var tier4RegionTriplet = new LookupTriplet();
                var tier4RegionPage = tier4RegionTriplet.AddLookupRow("Tier4Region");
                var tier4RegionRow = tier4RegionPage.TableRow.GetLastRow();

                tier4RegionRow.Code = code;
                tier4RegionRow.Description = string.Format("Selenium Test Entry - {0}", code);
                tier4RegionRow.DisplayOrder = "999";
                tier4RegionRow.IsVisible = true;

                //Save Reason Admission Rejected record
                tier4RegionPage.ClickSave();

                //Verify success message
                Assert.AreEqual(true, tier4RegionPage.IsSuccessMessageDisplayed(), "Success message do not display");
            }
            finally
            {
                // Tear down
                PurgeLinkedData.DeleteTier4Region(code);
            }
        }


        [Variant(Variant.EnglishStateSecondary)]
        /// <summary>
        /// Description: Exercise ability to add a Tier 4 VisaType lookup.
        /// Role: Admissions Officer
        /// </summary>
        [WebDriverTest(TimeoutSeconds = 600, Enabled = true, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { "Verify_Tier4_VisaType_Lookup", "Tier4_Lookups","Applications" })]
        public void Verify_Tier4_VisaType_Lookup()
        {
            var code = SeleniumHelper.GenerateRandomString(3);

            try
            {
                FeatureBee.FeatureBeeLogin.LoginWithFeatureBee(new[] { "Tier4" }, FeatureBee.FeatureBeeLogin.iSIMSUserType.AdmissionsOfficer);
                AutomationSugar.NavigateMenu("Lookups", "Admissions", "Tier 4 Visa Type");

                //Add new Reason Admission Rejected
                var tier4VisaTypeTriplet = new LookupTriplet();
                var tier4VisaTypePage = tier4VisaTypeTriplet.AddLookupRow("Tier4VisaType");
                var tier4VisaTypeRow = tier4VisaTypePage.TableRow.GetLastRow();

                tier4VisaTypeRow.Code = code;
                tier4VisaTypeRow.Description = string.Format("Selenium Test Entry - {0}", code);
                tier4VisaTypeRow.DisplayOrder = "999";
                tier4VisaTypeRow.IsVisible = true;

                //Save Reason Admission Rejected record
                tier4VisaTypePage.ClickSave();

                //Verify success message
                Assert.AreEqual(true, tier4VisaTypePage.IsSuccessMessageDisplayed(), "Success message do not display");
            }
            finally
            {
                // Tear down
                PurgeLinkedData.DeleteTier4VisaType(code);
            }
        }


        [Variant(Variant.EnglishStateSecondary)]
        /// <summary>
        /// Description: Exercise ability to add a Tier 4 VisaStatus lookup.
        /// Role: Admissions Officer
        /// </summary>
        [WebDriverTest(TimeoutSeconds = 600, Enabled = true, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { "Verify_Tier4_VisaStatus_Lookup", "Tier4_Lookups", "Applications" })]
        public void Verify_Tier4_VisaStatus_Lookup()
        {
            var code = SeleniumHelper.GenerateRandomString(3);

            try
            {
                FeatureBee.FeatureBeeLogin.LoginWithFeatureBee(new[] { "Tier4" }, FeatureBee.FeatureBeeLogin.iSIMSUserType.AdmissionsOfficer);
                AutomationSugar.NavigateMenu("Lookups", "Admissions", "Tier 4 Visa Status");

                //Add new Reason Admission Rejected
                var tier4VisaStatusTriplet = new LookupTriplet();
                var tier4VisaStatusPage = tier4VisaStatusTriplet.AddLookupRow("Tier4VisaStatus");
                var tier4VisaStatusRow = tier4VisaStatusPage.TableRow.GetLastRow();

                tier4VisaStatusRow.Code = code;
                tier4VisaStatusRow.Description = string.Format("Selenium Test Entry - {0}", code);
                tier4VisaStatusRow.DisplayOrder = "999";
                tier4VisaStatusRow.IsVisible = true;

                //Save Reason Admission Rejected record
                tier4VisaStatusPage.ClickSave();

                //Verify success message
                Assert.AreEqual(true, tier4VisaStatusPage.IsSuccessMessageDisplayed(), "Success message do not display");
            }
            finally
            {
                // Tear down
                PurgeLinkedData.DeleteTier4VisaStatus(code);
            }
        }


        [Variant(Variant.EnglishStateSecondary)]
        /// <summary>
        /// Description: Exercise ability to add a Tier 4 Evidence lookup.
        /// Role: Admissions Officer
        /// </summary>
        [WebDriverTest(TimeoutSeconds = 600, Enabled = true, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { "Verify_Tier4_Evidence_Lookup", "Tier4_Lookups" })]
        public void Verify_Tier4_Evidence_Lookup()
        {
            var code = SeleniumHelper.GenerateRandomString(3);

            try
            {
                FeatureBee.FeatureBeeLogin.LoginWithFeatureBee(new[] { "Tier4" }, FeatureBee.FeatureBeeLogin.iSIMSUserType.AdmissionsOfficer);
                AutomationSugar.NavigateMenu("Lookups", "Admissions", "Tier 4 Evidence");

                //Add new Reason Admission Rejected
                var tier4EvidenceTriplet = new LookupTriplet();
                var tier4EvidencePage = tier4EvidenceTriplet.AddLookupRow("Tier4Evidence");
                var tier4EvidenceRow = tier4EvidencePage.TableRow.GetLastRow();

                tier4EvidenceRow.Code = code;
                tier4EvidenceRow.Description = string.Format("Selenium Test Entry - {0}", code);
                tier4EvidenceRow.DisplayOrder = "999";
                tier4EvidenceRow.IsVisible = true;

                //Save Reason Admission Rejected record
                tier4EvidencePage.ClickSave();

                //Verify success message
                Assert.AreEqual(true, tier4EvidencePage.IsSuccessMessageDisplayed(), "Success message do not display");
            }
            finally
            {
                // Tear down
                PurgeLinkedData.DeleteTier4Evidence(code);
            }
        }


        [Variant(Variant.EnglishStateSecondary)]
        /// <summary>
        /// Description: Exercise ability to add a Tier 4 Reason lookup.
        /// Role: Admissions Officer
        /// </summary>
        [WebDriverTest(TimeoutSeconds = 600, Enabled = true, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { "Verify_Tier4_Reason_Lookup", "Tier4_Lookups" })]
        public void Verify_Tier4_Reason_Lookup()
        {
            var code = SeleniumHelper.GenerateRandomString(3);

            try
            {
                FeatureBee.FeatureBeeLogin.LoginWithFeatureBee(new[] { "Tier4" }, FeatureBee.FeatureBeeLogin.iSIMSUserType.AdmissionsOfficer);
                AutomationSugar.NavigateMenu("Lookups", "Admissions", "Tier 4 Reason");

                //Add new Reason Admission Rejected
                var tier4ReasonTriplet = new LookupTriplet();
                var tier4ReasonPage = tier4ReasonTriplet.AddLookupRow("Tier4Reason");
                var tier4ReasonRow = tier4ReasonPage.TableRow.GetLastRow();

                tier4ReasonRow.Code = code;
                tier4ReasonRow.Description = string.Format("Selenium Test Entry - {0}", code);
                tier4ReasonRow.DisplayOrder = "999";
                tier4ReasonRow.IsVisible = true;

                //Save Reason Admission Rejected record
                tier4ReasonPage.ClickSave();

                //Verify success message
                Assert.AreEqual(true, tier4ReasonPage.IsSuccessMessageDisplayed(), "Success message do not display");
            }
            finally
            {
                // Tear down
                PurgeLinkedData.DeleteTier4Reason(code);
            }
        }
    }
}
