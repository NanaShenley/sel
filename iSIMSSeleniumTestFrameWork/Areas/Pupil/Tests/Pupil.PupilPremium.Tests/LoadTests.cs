using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using OpenQA.Selenium;
using POM.Components.Pupil;
using POM.Helper;
using Pupil.Components.Common;
using Pupil.Data;
using Selene.Support.Attributes;
using SeSugar;
using SeSugar.Automation;
using SeSugar.Data;
using TestSettings;

namespace Pupil.PupilPremium.Tests
{
    public class LoadTests
    {
        private const string PupilPremiumFeature = "Pupil Premium";

        [WebDriverTest(Enabled = true, Browsers = new[] { BrowserDefaults.Chrome },
            Groups = new[] { PupilTestGroups.PupilPremium.Page, PupilTestGroups.PupilPremium.Permissions, PupilTestGroups.Priority.Priority2, "PP2" })]
        [Variant(Variant.AllEnglish | Variant.AllIndependant)]
        public void CanOpenPupilPremiumRecordAsSeniorManagementTeam()
        {
            //Arrange
            // Add Basic Learner
            var pupilRecord = this.BuildDataPackage();
            var learnerId = Guid.NewGuid();
            var surname = "PupilPremium" + Utilities.GenerateRandomString(5);
            var forename = Utilities.GenerateRandomString(15);
            var dateOfBirth = new DateTime(2009, 05, 31);
            var dateOfAdmission = new DateTime(2011, 10, 10);

            pupilRecord.AddBasicLearner(learnerId, surname, forename, dateOfBirth, dateOfAdmission);

            // Act
            using (new DataSetup(false, true, pupilRecord))
            {
                try
                {
                    // Login as school admin
                    SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SeniorManagementTeam, false, PupilPremiumFeature);
                    AutomationSugar.WaitForAjaxCompletion();

                    AutomationSugar.NavigateMenu("Tasks", "Pupils", "Pupil Premium");
                    AutomationSugar.WaitForAjaxCompletion();

                    // Search a pupil
                    var pupilPremiumRecordTriplet = new PupilPremiumRecordTriplet();
                    pupilPremiumRecordTriplet.SearchCriteria.PupilName = String.Format("{0}, {1}", surname, forename);
                    pupilPremiumRecordTriplet.SearchCriteria.IsCurrent = true;
                    pupilPremiumRecordTriplet.SearchCriteria.IsFuture = false;
                    pupilPremiumRecordTriplet.SearchCriteria.IsLeaver = false;
                    pupilPremiumRecordTriplet.SearchCriteria.HasPupilPremium = false;

                    var pupilResults = pupilPremiumRecordTriplet.SearchCriteria.Search();
                    AutomationSugar.WaitForAjaxCompletion();

                    // Open pupil page
                    pupilResults.FirstOrDefault(x => x.Name.Trim().Equals(surname + ", " + forename))
                        .Click<PupilPremiumRecordPage>();
                    AutomationSugar.WaitForAjaxCompletion();

                    Assert.IsNotNull(
                        SeleniumHelper.GetVisible(
                            By.CssSelector(SeleniumHelper.AutomationId("service_navigation_contextual_link_Pupil_Record"))));
                }
                finally
                {
                    //cleanup code goes here
                }
            }
        }

        [WebDriverTest(Enabled = true, Browsers = new[] { BrowserDefaults.Chrome },
            Groups = new[] { PupilTestGroups.PupilPremium.Page, PupilTestGroups.PupilPremium.Permissions, PupilTestGroups.Priority.Priority2, "PP2" })]
        [Variant(Variant.AllEnglish | Variant.AllIndependant)]
        public void CanOpenPupilPremiumRecordAsReturnsManager()
        {
            //Arrange
            // Add Basic Learner
            var pupilRecord = this.BuildDataPackage();
            var learnerId = Guid.NewGuid();
            var surname = "PupilPremium" + Utilities.GenerateRandomString(5);
            var forename = Utilities.GenerateRandomString(15);
            var dateOfBirth = new DateTime(2009, 05, 31);
            var dateOfAdmission = new DateTime(2011, 10, 10);

            pupilRecord.AddBasicLearner(learnerId, surname, forename, dateOfBirth, dateOfAdmission);

            // Act
            using (new DataSetup(false, true, pupilRecord))
            {
                try
                {
                    // Login as school admin
                    SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.ReturnsManager, false, PupilPremiumFeature);
                    AutomationSugar.WaitForAjaxCompletion();

                    AutomationSugar.NavigateMenu("Tasks", "Pupils", "Pupil Premium");
                    AutomationSugar.WaitForAjaxCompletion();

                    // Search a pupil
                    var pupilPremiumRecordTriplet = new PupilPremiumRecordTriplet();
                    pupilPremiumRecordTriplet.SearchCriteria.PupilName = String.Format("{0}, {1}", surname, forename);
                    pupilPremiumRecordTriplet.SearchCriteria.IsCurrent = true;
                    pupilPremiumRecordTriplet.SearchCriteria.IsFuture = false;
                    pupilPremiumRecordTriplet.SearchCriteria.IsLeaver = false;
                    pupilPremiumRecordTriplet.SearchCriteria.HasPupilPremium = false;

                    var pupilResults = pupilPremiumRecordTriplet.SearchCriteria.Search();
                    AutomationSugar.WaitForAjaxCompletion();

                    // Open pupil page
                    pupilResults.FirstOrDefault(x => x.Name.Trim().Equals(surname + ", " + forename))
                        .Click<PupilPremiumRecordPage>();
                    AutomationSugar.WaitForAjaxCompletion();

                    Assert.IsNotNull(
                        SeleniumHelper.GetVisible(
                            By.CssSelector(SeleniumHelper.AutomationId("service_navigation_contextual_link_Pupil_Record"))));
                }
                finally
                {
                    //cleanup code goes here
                }
            }
        }
    }
}
