using NUnit.Framework;
using OpenQA.Selenium.Support.UI;
using POM.Components.Pupil;
using POM.Components.Pupil.Pages;
using POM.Helper;
using Pupil.Components;
using Pupil.Components.Common;
using Pupil.Data;
using Pupil.Data.Entities;
using Selene.Support.Attributes;
using SeSugar.Data;
using System;
using System.Globalization;
using TestSettings;
using WebDriverRunner.webdriver;

namespace Pupil.Achievements.Tests
{
    public class CreateTests
    {
        private const string ClassLogFeature = "Class Log";

        private const string ConductFeature = "Conduct";

        private string _pattern = "d/M/yyyy";

        [WebDriverTest(Enabled = true, Browsers = new[] { BrowserDefaults.Chrome },
             Groups =
                 new[]
                 {
                     PupilTestGroups.ClassLog.Conduct, 
                     PupilTestGroups.Priority.Priority2,
                     "A_CAFCLQF"
                 })]
        public void Can_Add_From_Class_Log_Quick_Form()
        {
            // Arrange
            ClassTeacher teacher = ClassLogData.GetClassTeacherWithClassAndLearners();
            AuthorisedUser authUser = ClassLogData.GetAuthorisedUserDetailsForClassTeacherUser(TestDefaults.Default.ClassTeacher);
            ClassLogData.UpdateClassTeacherWithInitialAuthUserValues(authUser, TestDefaults.Default.ClassTeacher);
            ClassLogData.UpdateClassTeacherUserWithStaffDetails(teacher, TestDefaults.Default.ClassTeacher);

            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.ClassTeacher, enabledFeatures: ClassLogFeature);

            Wait.WaitForAjaxReady();

            var classLogNavigate = new ClassLogNavigation();
            Wait.WaitForAjaxReady();

            //Act
            classLogNavigate.NavigateToPupilClassLogFromMenu();
            Wait.WaitForAjaxReady();

            ClassLogPage clogPage = new ClassLogPage();
            clogPage.SelectPupil();

            var achievementDialog = clogPage.OpenAchievementPopup();

            var achievementEvent = Queries.GetFirstQuickAchievementEventLookup();

            achievementDialog.AchievementType = achievementEvent.Description;

            // Increment twice and then decrement once to test slider working
            achievementDialog.ClickPointsSliderUp();
            achievementDialog.ClickPointsSliderUp();

            achievementDialog.ClickPointsSliderDown();

            achievementDialog.Comments = "Selenium test event";

            achievementDialog.Save();

            // Assert?
            SeleniumHelper.WaitUntilElementIsDisplayed("//*/div[@class='alert alert-info animated zoomInAndFade' and @role='alert']");
        }

        //[WebDriverTest(Enabled = true, Browsers = new[] { BrowserDefaults.Chrome },
        //     Groups =
        //         new[]
        //         {
        //                     PupilTestGroups.PupilRecord.PupilRecordConduct,
        //                     PupilTestGroups.Priority.Priority2,
        //                    "A_CAFPRFF" 
        //         })]
        //public void Can_Add_From_Pupil_Record_Full_Form()
        //{
        //    // Arrange
        //    string forename = "AddAchievementEvent";
        //    string surname = "AddAchievementEvent" + SeleniumHelper.GenerateRandomString(10);

        //    DateTime dob = new DateTime(2011, 02, 02);
        //    var doa = DateTime.ParseExact("02/02/2016", _pattern, CultureInfo.InvariantCulture).ToString(_pattern);
        //    var yearGroup = Queries.GetFirstYearGroup();
        //    var learnerId = Guid.NewGuid();

        //    var fullAchievmentEventLookup = Queries.GetFirstFullAchievementEventLookup();

        //    var pupil = this.BuildDataPackage()
        //               .AddBasicLearner(learnerId, surname, forename, dob, dateOfAdmission: new DateTime(2011, 10, 05));

        //    using (new DataSetup(false, true, pupil))
        //    {
        //        SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator, enabledFeatures: "Conduct");

        //        var pupilRecordNavigation = new PupilRecordNavigation();
        //        pupilRecordNavigation.NavigateToPupilRecordMenuPage();
        //        Wait.WaitForDocumentReady();

        //        var pupilRecordTriplet = new PupilRecordTriplet();

        //        pupilRecordTriplet.SearchCriteria.PupilName = String.Format("{0}, {1}", surname, forename);
        //        var resultPupils = pupilRecordTriplet.SearchCriteria.Search();
        //        var pupilSearchTile = resultPupils.SingleOrDefault(t => t.Name.Equals(String.Format("{0}, {1}", surname, forename)));

        //        var pupilRecord = pupilSearchTile == null ? null : pupilSearchTile.Click<PupilRecordPage>();
        //        Assert.IsNotNull(pupilRecord, "Test pupil record not found");

        //        //Act
        //        pupilRecord.ClickAddConductPoints();

        //        var achievementQuickDialog = pupilRecord.ClickAchievementConductPointsLink();

        //        achievementQuickDialog.AchievementType = fullAchievmentEventLookup.Description;

        //        // TODO waiting for Reshmi to complete the piece that switches the quick dialog to the full event dialog

        //        // update vlaues
        //        // Save values
        //        // achievementDialog.Save();
        //    }
        //}
    }
}
