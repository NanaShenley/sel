using System;
using System.Threading;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
using Admissions.Component;
//using Admissions.Component.Common;
using SharedComponents.Helpers;
using TestSettings;
using WebDriverRunner.internals;
using WebDriverRunner.webdriver;
using Selene.Support.Attributes;

namespace Admissions.Smoke.Tests
{
    public class AdmissionsSmokeTests
    {
        //Already done in previous tests
        //[WebDriverTest(Groups = new[] { "SmokeTests", PupilTestGroups.Severities.Priority3 }, Browsers = new[] { BrowserDefaults.Chrome, BrowserDefaults.Ie })]
        /// <summary>
        /// Duplicate tests. Hence P3
        /// </summary>
        //public void CanNavigateToAndSavePupil()
        //{
        //    var rnd = new Random();
        //    SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);
        //    var pupilRecordNavigation = new PupilRecordNavigation();
        //    pupilRecordNavigation.NavigateToPupilRecord_PupilDetailPage("07121a29-cc8a-4ac0-b846-2161f76427d7");
        //    pupilRecordNavigation.NavigateAndSetQuickNote("This is a test note, random number =" +  rnd.Next(99999));
        //    pupilRecordNavigation.SaveRecord();
        //    Assert.IsTrue(pupilRecordNavigation.FindSaveSuccessMessage());
        //}

        [WebDriverTest(Enabled = true, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { "P1" })]
        public void SmokeTests_AddSchoolIntake_AddApplication_DeletePupil_DeleteSchoolInTake()
        {
            WebContext.WebDriver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(10));
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.AdmissionsOfficer);

            //Navigate to school intake screen
            var schoolIntakeNavigator = new SchoolIntakeRecordNavigation();
            schoolIntakeNavigator.NavgateToSchoolIntake();
            Thread.Sleep(2000);

            //Create a new school intake
            string schoolIntakeId = schoolIntakeNavigator.CreateNewSchoolIntake("2015/2016", "Autumn", "Year N", "10", "TestSchoolInTake1516AUT3", "AG1516AUT3", "01/02/2013", "10");

            Thread.Sleep(2000);

            //Navigate to application screen
            var applicationNavigator = new ApplicationRecordNavigation();
            applicationNavigator.NavgateToApplicationsDirectly();

            //Create a new application along with underlying pupil record.
            var learner_id = applicationNavigator.CreateNewApplication("S256", "J256", "Male", "01/01/2000", "AG1516AUT3");

            //Set the application as admitted
            applicationNavigator.SetApplicationToAdmitted();

            Thread.Sleep(2000);

            //Delete the pupil and it's links
            var deletePupilNavigator = new DeletePupilRecordNavigation();
            deletePupilNavigator.DeleteLearner(learner_id);

            //Delete school intake
            schoolIntakeNavigator.DeleteSchoolIntake(schoolIntakeId, "2015/2016");
            WebContext.WebDriver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(0));
        }
    }
}
