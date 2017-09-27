using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PageObjectModel.Components.Admission;
using PageObjectModel.Helper;
using Admissions.Data;
using Admissions.Component;
using TestSettings;
using Selene.Support.Attributes;
using WebDriverRunner.webdriver;
using OpenQA.Selenium.Support;
using OpenQA.Selenium.Support.UI;
using SeSugar.Automation;
using NUnit.Framework;
using SeSugar.Data;

namespace Admissions.Policy.Tests
{
    public class Policies
    {

        private readonly int tenantID = SeSugar.Environment.Settings.TenantId;

        /// <summary>
        /// Exercise ability to navigate to Policy screen.
        /// </summary>
        [WebDriverTest(TimeoutSeconds = 1200, Enabled = true, Browsers = new[] { BrowserDefaults.Chrome },
            Groups = new[] { "P2", "Policies", "NavigatetoPolicies" })]
        public void Navigate_To_Policy()
        {
            //Login
            String[] featureList = { "Admission PoliciesandCriteria" };
            FeatureBee.FeatureBeeLogin.LoginWithFeatureBee(featureList, FeatureBee.FeatureBeeLogin.iSIMSUserType.AdmissionsOfficer);
            AutomationSugar.NavigateMenu("Tasks", "Admissions", "Manage Admission Policies");
            PolicyDetails policydetails = new PolicyDetails();
            String policyTitle = policydetails.getPolicyTitle();
            Assert.IsTrue(policyTitle.Contains("Admission Policies"));

        }

        /// <summary>
        /// Exercise ability to navigate to Policy screen and create a new policy with name and description.
        /// </summary>
        [WebDriverTest(TimeoutSeconds = 1200, Enabled = true, Browsers = new[] { BrowserDefaults.Chrome },
            Groups = new[] { "P2", "Policies", "CreateBasicPolicywithName&Description" })]
        public void Create_Basic_Policy()
        {
            //Login

            PolicyDetails policydetails = new PolicyDetails();
            Navigate_To_Policy();
            policydetails.ClickAddPolicyButton();
            String policyName = policydetails.setPolicyName();
            String policyDescription = policydetails.setPolicyDescription();
            policydetails.ClickSaveButton();
            policydetails.ClickNoCriteriaDialogButton();
            policydetails.WaitForStatus();
            //Purge created data
            PurgeLinkedData.DeletAdmissionPolicy(policyName);

        }

        /// <summary>
        /// Exercise ability to search a policy by name.
        /// </summary>
        [WebDriverTest(TimeoutSeconds = 1200, Enabled = true, Browsers = new[] { BrowserDefaults.Chrome },
            Groups = new[] { "P3", "Policies", "SearchpolicybyName" })]
        public void Search_Policy_ByName()
        {
            //Login
            PolicyDetails policydetails = new PolicyDetails();

            //Insert a policy into database
            Guid policyId = Guid.NewGuid();
            string policyName = CoreQueries.GetColumnUniqueString("AdmissionsPolicy", "Name", 10, tenantID);
            string policyDescription = CoreQueries.GetColumnUniqueString("AdmissionsPolicy", "Description", 10, tenantID);

            using (new DataSetup(GetPolicyRecord(policyId, policyName, policyDescription)))
            {
                //Act
                Navigate_To_Policy();
                policydetails.ClickAddPolicyButton();
                policydetails.SearchByName(policyName);

            }
            //Purge created data
            PurgeLinkedData.DeletAdmissionPolicy(policyId);
        }


        /// <summary>
        /// Exercise ability to delete a policy without criteria.
        /// </summary>
        [WebDriverTest(TimeoutSeconds = 1200, Enabled = true, Browsers = new[] { BrowserDefaults.Chrome },
            Groups = new[] { "P3", "Policies", "Delete_Policy_WIthout_Criteria" })]
        public void Delete_Policy_WIthout_Criteria()
        {
            //Login
            PolicyDetails policydetails = new PolicyDetails();

            //Insert a policy into database
            Guid policyId = Guid.NewGuid();
            string policyName = CoreQueries.GetColumnUniqueString("AdmissionsPolicy", "Name", 10, tenantID);
            string policyDescription = CoreQueries.GetColumnUniqueString("AdmissionsPolicy", "Description", 10, tenantID);

            using (new DataSetup(GetPolicyRecord(policyId, policyName, policyDescription)))
            {
                //Act
                Navigate_To_Policy();
                policydetails.SearchByName(policyName);
                policydetails.ClickDeleteButton();
            }

        }

        private DataPackage GetPolicyRecord(Guid policyId, string policyName, string policyDescription)
        {
            return this.BuildDataPackage()
               .AddData("AdmissionsPolicy", new
               {
                   Id = policyId,
                   TenantID = tenantID,
                   Name = policyName,
                   Description = policyDescription,
                   IsActive = 1
               });
        }


    

    /// <summary>
    /// Exercise ability to create a policy with criteria
    /// </summary>
    [WebDriverTest(TimeoutSeconds = 1200, Enabled = true, Browsers = new[] { BrowserDefaults.Chrome },
            Groups = new[] { "P2", "Policies", "CreatePolicyWithCriteria" })]
        public void Create_Policy_WithCriteria()
        {
            //Login
            PolicyDetails policydetails = new PolicyDetails();
            Navigate_To_Policy();
            policydetails.ClickAddPolicyButton();
            String policyName = policydetails.setPolicyName();
            String policyDescription = policydetails.setPolicyDescription();
            policydetails.ClickAddCriteriaDialog();
            policydetails.ClickAddCriteriaButton();
            String CriteriaName = policydetails.setCriteriaName();
            policydetails.SetCriteriaType("Number");
            policydetails.setCriteriaDescription();
            policydetails.ClickSaveCriteriaButton();
            policydetails.ClickSelectCriteriaButton();
            policydetails.setCriteriaPriority();
            policydetails.ClickSaveButton();
            policydetails.WaitForStatus();
            policydetails.SearchByName(policyName);

            PurgeLinkedData.DeletAdmissionPolicyCriteriaRelation(policyName);
            PurgeLinkedData.DeletAdmissionCriteria(CriteriaName);
            PurgeLinkedData.DeletAdmissionPolicy(policyName);

        }


    }
}
