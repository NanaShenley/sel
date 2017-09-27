using System;
using System.Linq;
using System.Threading;
using NUnit.Framework;
using PageObjectModel.Components.Admission;
using PageObjectModel.Helper;
using Admissions.Data;
using Admissions.Component;
using SeSugar;
using TestSettings;
using Selene.Support.Attributes;
using WebDriverRunner.webdriver;
using OpenQA.Selenium.Support;
using OpenQA.Selenium.Support.UI;
using SeSugar.Automation;
using OpenQA.Selenium;
using SharedComponents.CRUD;

namespace Admissions.Enquiries.Tests
{
    public class Enquiry
    {



        /// <summary>
        /// Exercise ability to navigate to Enquiry screen.
        /// </summary>
        [WebDriverTest(TimeoutSeconds = 1200, Enabled = true, Browsers = new[] { BrowserDefaults.Chrome },
            Groups = new[] { "P2", "Enquiry", "Navigatetoenquiryscreen" })]
        public void Navigate_To_Enquiry()
        {
            //Login
            String[] featureList = { "Enquiries" };
            FeatureBee.FeatureBeeLogin.LoginWithFeatureBee(featureList, FeatureBee.FeatureBeeLogin.iSIMSUserType.AdmissionsOfficer);
            AutomationSugar.NavigateMenu("Tasks", "Admissions", "Enquiries");
            EnquiryDetails enquirydetails = new EnquiryDetails();
            String enquirytitle = enquirydetails.GetEnquiryitle();
            Assert.AreEqual(enquirytitle, "Enquiries");
        }

        /// <summary>
        /// Create a new enquiry with basic enquirer details
        /// </summary>
        [WebDriverTest(TimeoutSeconds = 1200, Enabled = true, Browsers = new[] { BrowserDefaults.Chrome },
            Groups = new[] { "P2", "Enquiry", "CreateNewEnquirywithEnquirer" })]
        public void Create_New_Enquiry_with_Enquirer()
        {
            //Login
            Navigate_To_Enquiry();
            EnquiryDetails enquirydetails = new EnquiryDetails();
            enquirydetails.AddButtonClick();

            ////Add Enquirer details
            enquirydetails.AddEnquiryContactButtonClick();
            enquirydetails.AddNewEnquirerContactButtonClick();
            enquirydetails.SetEnquirerForename();
            String surname = enquirydetails.SetEnquirerSurname();
            enquirydetails = enquirydetails.ClickOkButton();

            enquirydetails.ClickSaveButton();
            enquirydetails.WaitForStatus();
            enquirydetails.SearchByName(surname);
        }

        /// <summary>
        /// Create a new enquiry with basic enquirer details
        /// </summary>
        [WebDriverTest(TimeoutSeconds = 1200, Enabled = true, Browsers = new[] { BrowserDefaults.Chrome },
            Groups = new[] { "P2", "Enquiry", "CreateNewEnquirywithExistingEnquirer" })]
        public void Create_New_Enquiry_with_Existing_Enquirer()
        {
            //Login
            Navigate_To_Enquiry();
            EnquiryDetails enquirydetails = new EnquiryDetails();
            enquirydetails.AddButtonClick();

            ////Add Enquirer details
            enquirydetails.AddEnquiryContactButtonClick();
            enquirydetails.AddNewEnquirerContactButtonClick();
            string forname = enquirydetails.SetEnquirerForename();
            String surname = enquirydetails.SetEnquirerSurname();
            enquirydetails = enquirydetails.ClickOkButton();

            enquirydetails.ClickSaveButton();
            enquirydetails.WaitForStatus();

            EnquiryDetails newEnquirydetails = new EnquiryDetails();
            newEnquirydetails.AddButtonClick();
            newEnquirydetails.AddEnquiryContactButtonClick();
            newEnquirydetails = newEnquirydetails.SearchEnquirerByName(surname);
            newEnquirydetails.SetEnquirerTitle("Mr");
            newEnquirydetails.SetEnquirerGender("Male");
            newEnquirydetails.SetEnquirerOccupation("Accounting");
            newEnquirydetails = newEnquirydetails.ClickOkButton();
            newEnquirydetails.ClickSaveButton();
            newEnquirydetails.WaitForStatus();
            newEnquirydetails.SearchByName(surname);
        }

        /// <summary>
        /// Create a new enquiry with basic details
        /// </summary>
        [WebDriverTest(TimeoutSeconds = 1200, Enabled = true, Browsers = new[] { BrowserDefaults.Chrome },
            Groups = new[] { "P2", "Enquiry", "CreateNewEnquirywithchildBasicDetails" })]
        public void Create_New_Enquiry_with_Child_Basic_Details()
        {
            //Login
            Navigate_To_Enquiry();
            EnquiryDetails enquirydetails = new EnquiryDetails();
            enquirydetails.AddButtonClick();

            ////Add Enquirer details
            enquirydetails.AddEnquiryContactButtonClick();
            enquirydetails.AddNewEnquirerContactButtonClick();
            enquirydetails.SetEnquirerForename();
            String Surname = enquirydetails.SetEnquirerSurname();
            enquirydetails = enquirydetails.ClickOkButton();

            ////Add Child details
            enquirydetails.AddChildButtonClick();
            enquirydetails.SetChildForename();
            enquirydetails.SetChildSurname();
            enquirydetails.SetChildGender("Male");
            enquirydetails.SetChildDOB("01/01/2009");
            enquirydetails.ClickOkButton();
            enquirydetails.ClickSaveButton();
            enquirydetails.WaitForStatus();
            enquirydetails.SearchByName(Surname);
        }

        /// <summary>
        /// Create a new enquiry with basic details
        /// </summary>
        [WebDriverTest(TimeoutSeconds = 1200, Enabled = true, Browsers = new[] { BrowserDefaults.Chrome },
            Groups = new[] { "P3", "Enquiry", "CreateNewEnquirywithChildRealationship" })]
        public void Create_New_Enquiry_with_Child_Realationship()
        {
            //Login
            Navigate_To_Enquiry();
            EnquiryDetails enquirydetails = new EnquiryDetails();
            enquirydetails.AddButtonClick();

            ////Add Enquirer details
            enquirydetails.AddEnquiryContactButtonClick();
            enquirydetails.AddNewEnquirerContactButtonClick();
            enquirydetails.SetEnquirerForename();
            String Surname = enquirydetails.SetEnquirerSurname();
            enquirydetails = enquirydetails.ClickOkButton();

            ////Add Child details
            enquirydetails.AddChildButtonClick();
            enquirydetails.SetChildForename();
            enquirydetails.SetChildSurname();
            enquirydetails.SetChildGender("Male");
            enquirydetails.SetChildDOB("01/01/2009");
            enquirydetails.SetChildRelationship("Parent");
            enquirydetails.ClickOkButton();
            enquirydetails.ClickSaveButton();
            enquirydetails.WaitForStatus();
            enquirydetails.SearchByName(Surname);
        }

        /// <summary>
        /// Create a new enquiry with basic details
        /// </summary>
        [WebDriverTest(TimeoutSeconds = 1200, Enabled = true, Browsers = new[] { BrowserDefaults.Chrome },
            Groups = new[] { "P3", "Enquiry", "CreateNewEnquirywithNotes" })]
        public void Create_New_Enquiry_with_Notes()
        {
            //Login
            Navigate_To_Enquiry();
            EnquiryDetails enquirydetails = new EnquiryDetails();
            enquirydetails.AddButtonClick();

            ////Add Enquirer details
            enquirydetails.AddEnquiryContactButtonClick();
            enquirydetails.AddNewEnquirerContactButtonClick();
            enquirydetails.SetEnquirerForename();
            String Surname = enquirydetails.SetEnquirerSurname();
            enquirydetails = enquirydetails.ClickOkButton();
            String NotesEntered = enquirydetails.SetEnquirerNotes();
            enquirydetails.ClickSaveButton();
            enquirydetails.WaitForStatus();
            enquirydetails.SearchByName(Surname);
            String getNotes = enquirydetails.getEnquirerNotes();
            Assert.AreEqual(NotesEntered, getNotes);
        }


        /// <summary>
        /// Create a new enquiry with additional enquirer details
        /// </summary>
        [WebDriverTest(TimeoutSeconds = 1200, Enabled = true, Browsers = new[] { BrowserDefaults.Chrome },
            Groups = new[] { "P3", "Enquiry", "CreateNewEnquirywithadditionaldetails" })]
        public void Create_New_Enquiry_With_Additional_Details()
        {
            //Login
            Navigate_To_Enquiry();
            EnquiryDetails enquirydetails = new EnquiryDetails();
            enquirydetails.AddButtonClick();

            //Add Enquirer details
            enquirydetails.AddEnquiryContactButtonClick();
            enquirydetails.AddNewEnquirerContactButtonClick();
            enquirydetails.SetEnquirerForename();
            String Surname = enquirydetails.SetEnquirerSurname();
            enquirydetails.SetEnquirerTitle("Mr");
            enquirydetails.SetEnquirerGender("Male");
            enquirydetails.SetEnquirerOccupation("Accounting");
            enquirydetails.ClickPhoneButton();
            enquirydetails.SetPhone();
            enquirydetails.SetPhoneLocationType("Home");
            enquirydetails.ClickEmailButton();
            enquirydetails.SetEmailAddress();
            enquirydetails.SetEmailType("Home");
            enquirydetails.ClickOkButton();

            //Add Child details
            enquirydetails.AddChildButtonClick();
            enquirydetails.SetChildForename();
            enquirydetails.SetChildSurname();
            enquirydetails.SetChildGender("Male");
            enquirydetails.SetChildDOB("01/01/2009");
            enquirydetails.ClickOkButton();
            enquirydetails.ClickSaveButton();
            enquirydetails.WaitForStatus();
            enquirydetails.SearchByName(Surname);

        }


        /// <summary>
        /// Create a new enquiry with high commitment and search through advanced filter option
        /// </summary>
        [WebDriverTest(TimeoutSeconds = 1200, Enabled = true, Browsers = new[] { BrowserDefaults.Chrome },
            Groups = new[] { "P3", "Enquiry", "Create_New_Enquiry_With_High_Commitment" })]
        public void Create_New_Enquiry_With_High_Commitment()
        {
            //Login
            Navigate_To_Enquiry();
            EnquiryDetails enquirydetails = new EnquiryDetails();
            enquirydetails.AddButtonClick();

            //Add Enquirer details
            enquirydetails.AddEnquiryContactButtonClick();
            enquirydetails.AddNewEnquirerContactButtonClick();
            String Surname = enquirydetails.EnquirerBasicDetails();

            enquirydetails.IsHighCommitment();
            enquirydetails.ClickSaveButton();
            enquirydetails.WaitForStatus();
            enquirydetails.SelectAdvancedfilter("HighCommitment", Surname);

        }

        /// <summary>
        /// Create a new enquiry with high commitment and search through advanced filter option
        /// </summary>
        [WebDriverTest(TimeoutSeconds = 1200, Enabled = true, Browsers = new[] { BrowserDefaults.Chrome },
            Groups = new[] { "P3", "Enquiry", "Create_New_Enquiry_With_FollowUpRequired" })]
        public void Create_New_Enquiry_With_FollowUpRequired()
        {
            //Login
            Navigate_To_Enquiry();
            EnquiryDetails enquirydetails = new EnquiryDetails();
            enquirydetails.AddButtonClick();

            //Add Enquirer details
            enquirydetails.AddEnquiryContactButtonClick();
            enquirydetails.AddNewEnquirerContactButtonClick();
            String Surname = enquirydetails.EnquirerBasicDetails();

            enquirydetails.IsFollowuprequired();
            enquirydetails.ClickSaveButton();
            enquirydetails.WaitForStatus();
            enquirydetails.SelectAdvancedfilter("FollowupRequired", Surname);

        }

        /// <summary>
        /// Create a new enquiry with high commitment and search through advanced filter option
        /// </summary>
        [WebDriverTest(TimeoutSeconds = 1200, Enabled = true, Browsers = new[] { BrowserDefaults.Chrome },
            Groups = new[] { "P3", "Enquiry", "Create_New_Enquiry_With_Inactive_Status" })]
        public void Create_New_Enquiry_With_Inactive_Status()
        {
            //Login
            Navigate_To_Enquiry();
            EnquiryDetails enquirydetails = new EnquiryDetails();
            enquirydetails.AddButtonClick();

            //Add Enquirer details
            enquirydetails.AddEnquiryContactButtonClick();
            enquirydetails.AddNewEnquirerContactButtonClick();
            String Surname = enquirydetails.EnquirerBasicDetails();

            enquirydetails.ClickSaveButton();
            enquirydetails.WaitForStatus();
            enquirydetails.SelectAdvancedfilter("Inactive", Surname);

        }

        /// <summary>
        /// Description: Exercise ability to add a Enquiry Marketting Source lookup.
        /// Role: Admissions Officer
        /// </summary>
        [WebDriverTest(TimeoutSeconds = 600, Enabled = true, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { "Enquiry", "Verify_Marketing_Source_Lookup", "Enquiries_Lookups", "Applications" })]
        public void Verify_Marketing_Source_Lookup()
        {
            var code = SeleniumHelper.GenerateRandomString(3);

            try
            {
                FeatureBee.FeatureBeeLogin.LoginWithFeatureBee(new[] { "Enquiries" }, FeatureBee.FeatureBeeLogin.iSIMSUserType.AdmissionsOfficer);
                AutomationSugar.NavigateMenu("Lookups", "Admissions", "Marketing Source");

                //Add new Reason Admission Rejected
                var marketingSourceTriplet = new LookupTriplet();
                var marketingSourcePage = marketingSourceTriplet.AddLookupRow("MarketingSource");
                var marketingSourceRow = marketingSourcePage.TableRow.GetLastRow();

                marketingSourceRow.Code = code;
                marketingSourceRow.Description = string.Format("Selenium Test Entry - {0}", code);
                marketingSourceRow.DisplayOrder = "999";
                marketingSourceRow.IsVisible = true;

                //Save Reason Admission Rejected record
                marketingSourcePage.ClickSave();

                //Verify success message
                Assert.AreEqual(true, marketingSourcePage.IsSuccessMessageDisplayed(), "Success message do not display");
            }
            finally
            {
                // Tear down
                PurgeLinkedData.DeleteMarketingResource(code);
            }
        }

        /// <summary>
        /// Description: Exercise ability to add a Enquiry reason lookup.
        /// Role: Admissions Officer
        /// </summary>
        [WebDriverTest(TimeoutSeconds = 600, Enabled = true, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { "Enquiry", "Verify_Enquiry_Reason_Lookup", "Enquiries_Lookups", "Applications" })]
        public void Verify_Enquiry_Reason_Lookup()
        {
            var code = SeleniumHelper.GenerateRandomString(3);

            try
            {
                FeatureBee.FeatureBeeLogin.LoginWithFeatureBee(new[] { "Enquiries" }, FeatureBee.FeatureBeeLogin.iSIMSUserType.AdmissionsOfficer);
                AutomationSugar.NavigateMenu("Lookups", "Admissions", "Enquiry Reason");

                //Add new Reason Admission Rejected
                var marketingSourceTriplet = new LookupTriplet();
                var marketingSourcePage = marketingSourceTriplet.AddLookupRow("EnquiryReason");
                var marketingSourceRow = marketingSourcePage.TableRow.GetLastRow();

                marketingSourceRow.Code = code;
                marketingSourceRow.Description = string.Format("Selenium Test Entry - {0}", code);
                marketingSourceRow.DisplayOrder = "999";
                marketingSourceRow.IsVisible = true;

                //Save Reason Admission Rejected record
                marketingSourcePage.ClickSave();

                //Verify success message
                Assert.AreEqual(true, marketingSourcePage.IsSuccessMessageDisplayed(), "Success message do not display");
            }
            finally
            {
                // Tear down
                PurgeLinkedData.DeleteEnquiryReason(code);
            }
        }
        /// <summary>
        /// Description: Exercise ability to add a Enquiry Withdrawn reason lookup.
        /// Role: Admissions Officer
        /// </summary>
        [WebDriverTest(TimeoutSeconds = 600, Enabled = true, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { "Enquiry", "Verify_Enquiry_Withdrawn_Reason_Lookup", "Enquiries_Lookups", "Applications" })]
        public void Verify_Enquiry_Withdrawn_Reason_Lookup()
        {
            var code = SeleniumHelper.GenerateRandomString(3);

            try
            {
                FeatureBee.FeatureBeeLogin.LoginWithFeatureBee(new[] { "Enquiries" }, FeatureBee.FeatureBeeLogin.iSIMSUserType.AdmissionsOfficer);
                AutomationSugar.NavigateMenu("Lookups", "Admissions", "Enquiry Withdrawn Reason");

                //Add new Reason Admission Rejected
                var marketingSourceTriplet = new LookupTriplet();
                var marketingSourcePage = marketingSourceTriplet.AddLookupRow("EnquiryWithdrawnReason");
                var marketingSourceRow = marketingSourcePage.TableRow.GetLastRow();

                marketingSourceRow.Code = code;
                marketingSourceRow.Description = string.Format("Selenium Test Entry - {0}", code);
                marketingSourceRow.DisplayOrder = "999";
                marketingSourceRow.IsVisible = true;

                //Save Reason Admission Rejected record
                marketingSourcePage.ClickSave();

                //Verify success message
                Assert.AreEqual(true, marketingSourcePage.IsSuccessMessageDisplayed(), "Success message do not display");
            }
            finally
            {
                // Tear down
                PurgeLinkedData.DeleteEnquiryWithdrawnreason(code);
            }
        }

    }
}
