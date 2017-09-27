using System;
using NUnit.Framework;
using OpenQA.Selenium.Support.UI;
using POM.Components.Pupil;
using POM.Helper;
using Pupil.Components.Common;
using Pupil.Data;
using SeSugar;
using SeSugar.Data;
using TestSettings;
using WebDriverRunner.internals;
using WebDriverRunner.webdriver;

namespace Pupil.Pupil.Tests
{
    public class PupilDeletePupilTests
    {
        [WebDriverTest(TimeoutSeconds = 1800, Enabled = true, Browsers = new[] { BrowserDefaults.Chrome }, 
            Groups = new[] { PupilTestGroups.PupilDelete.Page, PupilTestGroups.PupilDelete.Delete, PupilTestGroups.Priority.Priority2 })]
        public void Ability_to_delete_a_Pupil_Record()
        {
            // Arrange
            var learnerId = Guid.NewGuid();
            var surname = Utilities.GenerateRandomString(10, "Selenium");
            var forename = Utilities.GenerateRandomString(10, "Selenium");
            var pupilName = String.Format("{0}, {1}", surname, forename);

            var dataPackage = this.BuildDataPackage();
            dataPackage.AddBasicLearner(learnerId, surname, forename, dateOfBirth: new DateTime(2005, 05, 30), dateOfAdmission: new DateTime(2012, 10, 03));
           
            // Act
            using (new DataSetup(purgeBeforeInsert: false, purgeAfterTest: true, packages: dataPackage))
            {
                // Login as school admin
                SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);

                // Navigate to Delete Pupil
                SeleniumHelper.NavigateBySearch("Delete Pupil", true);

                // Wait for screen to load
                WebDriverWait wait = new WebDriverWait(WebContext.WebDriver, TimeSpan.FromSeconds(10));
                wait.Until(ExpectedConditions.ElementIsVisible(SimsBy.AutomationId("search_criteria_submit")));

                // Search pupil that has been added
                var deletePupilTriplet = new DeletePupilRecordTriplet();

                deletePupilTriplet.SearchCriteria.PupilName = pupilName;
                deletePupilTriplet.SearchCriteria.IsCurrent = true;
                deletePupilTriplet.SearchCriteria.IsFuture = false;
                deletePupilTriplet.SearchCriteria.IsLeaver = false;
                var pupilResults = deletePupilTriplet.SearchCriteria.Search();
                var deletePupilRecordPage =
                    pupilResults.FirstOrDefault(x => x.Name.Trim().Equals(pupilName))
                        .Click<DeletePupilRecordPage>();

                // Delete pupil
                deletePupilRecordPage.Delete();

                // Re-search
                deletePupilTriplet.SearchCriteria.PupilName = pupilName;
                deletePupilTriplet.SearchCriteria.IsCurrent = false;
                deletePupilTriplet.SearchCriteria.IsFuture = false;
                deletePupilTriplet.SearchCriteria.IsLeaver = true;
                pupilResults = deletePupilTriplet.SearchCriteria.Search();

                // Verify that the deleted pupil is NOT included in the search results list.
                Assert.AreEqual(null, pupilResults.SingleOrDefault(x => x.Name.Equals(pupilName)),
                    "Fail: The deleted pupil is included in the pupil search results list.");
            }
        }
    }
}
