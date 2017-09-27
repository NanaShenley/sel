using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Admissions.Component;
using TestSettings;
using Selene.Support.Attributes;
using SeSugar.Automation;
using NUnit.Framework;
using Admissions.Data;
using SeSugar.Data;

namespace Admissions.Policy.Tests
{
    public class Criteria
    {

        /// <summary>
        /// Exercise ability to navigate to Criteria screen.
        /// </summary>
        [WebDriverTest(TimeoutSeconds = 1200, Enabled = true, Browsers = new[] { BrowserDefaults.Chrome },
            Groups = new[] { "P2", "Criteria", "Navigatetocrietria" })]
        private readonly int tenantID = SeSugar.Environment.Settings.TenantId;

        public void Navigate_To_Criteria()
        {
            //Login
            String[] featureList = { "Admission PoliciesandCriteria" };
            FeatureBee.FeatureBeeLogin.LoginWithFeatureBee(featureList, FeatureBee.FeatureBeeLogin.iSIMSUserType.AdmissionsOfficer);
            AutomationSugar.NavigateMenu("Tasks", "Admissions", "Manage Criteria");
            CriteriaDetails criteriadetails = new CriteriaDetails();
            String criteriaTitle = criteriadetails.getCriteriaTitle();
            Assert.IsTrue(criteriaTitle.Contains("Manage Criteria"));
        }

        /// <summary>
        /// Exercise ability to navigate to Policy screen and create a new Criteria with name and description.
        /// </summary>
        [WebDriverTest(TimeoutSeconds = 1200, Enabled = true, Browsers = new[] { BrowserDefaults.Chrome },
            Groups = new[] { "P2", "Criteria", "CreateBasicCriteriawithName&Description" })]
        public void Create_Basic_Criteria()
        {
            //Login
            Navigate_To_Criteria();
            CriteriaDetails criteriadetails = new CriteriaDetails();
            criteriadetails.ClickAddCriteriaButton();
            String criteriaName = criteriadetails.setCriteriaName();
            String criteriaDescription = criteriadetails.setCriteriaDescription();
            criteriadetails.ClickSaveButton();
            criteriadetails.WaitForStatus();
            PurgeLinkedData.DeletAdmissionCriteria(criteriaName);
        }

        /// <summary>
        /// Exercise ability to search Criteria by Name.
        /// </summary>
        [WebDriverTest(TimeoutSeconds = 1200, Enabled = true, Browsers = new[] { BrowserDefaults.Chrome },
            Groups = new[] { "P2", "Criteria", "SearchCriteriaByName" })]
        public void Search_Criteria_ByName()
        {
            
            //Insert a Criteria into database
            Guid criteriaId = Guid.NewGuid();
            string criteriaName = CoreQueries.GetColumnUniqueString("AdmissionsCriteria", "Name", 10, tenantID);
            string criteriaDescription = CoreQueries.GetColumnUniqueString("AdmissionsCriteria", "Description", 10, tenantID);
            using (new DataSetup(GetCriteriaRecord(criteriaId, criteriaName, criteriaDescription)))
            {
                //Act
                //Login
                Navigate_To_Criteria();
                CriteriaDetails criteriadetails = new CriteriaDetails();
                criteriadetails.SearchByName(criteriaName);
            }
            //Purge created data
            PurgeLinkedData.DeletAdmissionCriteria(criteriaId);
        }

        /// <summary>
        /// Exercise ability to search Criteria by Name.
        /// </summary>
        [WebDriverTest(TimeoutSeconds = 1200, Enabled = true, Browsers = new[] { BrowserDefaults.Chrome },
            Groups = new[] { "P2", "Criteria", "Delete_Criteria_without_policy" })]
        public void Delete_Criteria_without_policy()
        {
            //Login
            //Insert a Criteria into database
            Guid criteriaId = Guid.NewGuid();
            string criteriaName = CoreQueries.GetColumnUniqueString("AdmissionsCriteria", "Name", 10, tenantID);
            string criteriaDescription = CoreQueries.GetColumnUniqueString("AdmissionsCriteria", "Description", 10, tenantID);

            using (new DataSetup(GetCriteriaRecord(criteriaId, criteriaName, criteriaDescription)))
            {
                //Act
                Navigate_To_Criteria();
                CriteriaDetails criteriadetails = new CriteriaDetails();
                criteriadetails.SearchByName(criteriaName);
                criteriadetails.ClickDeleteButton();
            }            
        }

        
        private DataPackage GetCriteriaRecord(Guid criteriaId, string criteriaName, string criteriaDescription)
        {
            Guid criteriaType = Queries.GetCriteriaType("ID", "AdmissionsCriteriaType", tenantID);

            return this.BuildDataPackage()
               .AddData("AdmissionsCriteria", new
               {
                   Id = criteriaId,
                   TenantID = tenantID,
                   Name = criteriaName,
                   Description = criteriaDescription,
                   IsActive = 1,
                   AdmissionsCriteriaType = criteriaType
               });
        }

    }
}
