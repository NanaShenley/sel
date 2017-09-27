using NUnit.Framework;
using POM.Components.Pupil.Pages;
using POM.Helper;
using Pupil.Components;
using Pupil.Components.Common;
using Pupil.Data;
using Pupil.Data.Entities;
using Selene.Support.Attributes;
using TestSettings;

namespace Pupil.ClassLog.Tests
{
    public class ClassLogLoadTests
    {
        private const string ClassLogFeature = "Class Log";

        [WebDriverTest(Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { PupilTestGroups.ClassLog.Page, PupilTestGroups.Priority.Priority2, "clog" })]
        public void Can_View_PupilDetails_When_Login_As_ClassTeacher()
        {
            //Arrange
            ClassTeacher teacher = ClassLogData.GetClassTeacherWithClassAndLearners();
            AuthorisedUser authUser = ClassLogData.GetAuthorisedUserDetailsForClassTeacherUser(TestDefaults.Default.ClassTeacher);
            ClassLogData.UpdateClassTeacherWithInitialAuthUserValues(authUser, TestDefaults.Default.ClassTeacher);
            ClassLogData.UpdateClassTeacherUserWithStaffDetails(teacher, TestDefaults.Default.ClassTeacher);

            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.ClassTeacher, enabledFeatures: ClassLogFeature);

            //Act
            var classLogNavigate = new ClassLogNavigation();
            classLogNavigate.NavigateToPupilClassLogFromMenu();
            var clog = new ClassLogPage();

            //Assert
            Assert.AreEqual(teacher.ClassName, clog.DefaultClass());
            Assert.IsTrue(clog.HasPupilsLoaded());
        }
    }
}