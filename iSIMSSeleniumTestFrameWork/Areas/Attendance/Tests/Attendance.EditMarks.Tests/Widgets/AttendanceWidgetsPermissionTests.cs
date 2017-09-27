using NUnit.Framework;
using POM.Helper;
using Selene.Support.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using POM.Components.Attendance;
using TestSettings;

namespace Attendance.EditMarks.Tests.Widgets
{
    public class AttendanceWidgetsPermissionTests
    {
        [WebDriverTest(TimeoutSeconds = 1000, Enabled = true, Browsers = new[] { BrowserDefaults.Chrome })]
        [Variant(Variant.EnglishStatePrimary | Variant.WelshStatePrimary | Variant.NorthernIrelandStatePrimary)]
        public void ShouldDisplay_MissingRegister_UnexplainedAbsences_PupilLateWidget_to_Administrator()
        {
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);
            MissingRegisterTodayWidget missingRegisterWidget = new MissingRegisterTodayWidget();
            PupilLateTodayWidget pupilLateWidget = new PupilLateTodayWidget();
            UnexplainedAbsencesTodayWidget unexplainedWidget = new UnexplainedAbsencesTodayWidget();
            Assert.IsTrue(missingRegisterWidget.IsDisplayedWidget() && pupilLateWidget.IsDisplayedWidget() && unexplainedWidget.IsDisplayedWidget());
        }

        [WebDriverTest(TimeoutSeconds = 1000, Enabled = true, Browsers = new[] { BrowserDefaults.Chrome })]
        [Variant(Variant.EnglishStatePrimary | Variant.WelshStatePrimary | Variant.NorthernIrelandStatePrimary)]
        public void ShouldDisplay_MissingRegister_UnexplainedAbsences_PupilLateWidget_to_AssessmentCoordinator()
        {
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.AssessmentCoordinator);
            MissingRegisterTodayWidget missingRegisterWidget = new MissingRegisterTodayWidget();
            PupilLateTodayWidget pupilLateWidget = new PupilLateTodayWidget();
            UnexplainedAbsencesTodayWidget unexplainedWidget = new UnexplainedAbsencesTodayWidget();
            Assert.IsTrue(missingRegisterWidget.IsDisplayedWidget() && pupilLateWidget.IsDisplayedWidget() && unexplainedWidget.IsDisplayedWidget());
        }

        [WebDriverTest(TimeoutSeconds = 1000, Enabled = false, Browsers = new[] { BrowserDefaults.Chrome })]
        [Variant(Variant.EnglishStatePrimary | Variant.WelshStatePrimary | Variant.NorthernIrelandStatePrimary)]
        public void ShouldDisplay_MissingRegister_UnexplainedAbsences_PupilLateWidget_to_SystemManager()
        {
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SystemManger);
            MissingRegisterTodayWidget missingRegisterWidget = new MissingRegisterTodayWidget();
            PupilLateTodayWidget pupilLateWidget = new PupilLateTodayWidget();
            UnexplainedAbsencesTodayWidget unexplainedWidget = new UnexplainedAbsencesTodayWidget();
            Assert.IsTrue(missingRegisterWidget.IsDisplayedWidget() && pupilLateWidget.IsDisplayedWidget() && unexplainedWidget.IsDisplayedWidget());
        }

        [WebDriverTest(TimeoutSeconds = 1000, Enabled = true, Browsers = new[] { BrowserDefaults.Chrome })]
        public void ShouldDisplay_MissingRegister_UnexplainedAbsences_PupilLateWidget_to_to_CurricularManager()
        {
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.CurricularManager);
            MissingRegisterTodayWidget missingRegisterWidget = new MissingRegisterTodayWidget();
            PupilLateTodayWidget pupilLateWidget = new PupilLateTodayWidget();
            UnexplainedAbsencesTodayWidget unexplainedWidget = new UnexplainedAbsencesTodayWidget();
            Assert.IsTrue(missingRegisterWidget.IsDisplayedWidget() && pupilLateWidget.IsDisplayedWidget() && unexplainedWidget.IsDisplayedWidget());
        }

        [WebDriverTest(TimeoutSeconds = 1000, Enabled = true, Browsers = new[] { BrowserDefaults.Chrome })]
        [Variant(Variant.EnglishStatePrimary | Variant.WelshStatePrimary | Variant.NorthernIrelandStatePrimary)]
        public void ShouldDisplay_MissingRegister_UnexplainedAbsences_PupilLateWidget_to_ReturnsManager()
        {
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.ReturnsManager);
            MissingRegisterTodayWidget missingRegisterWidget = new MissingRegisterTodayWidget();
            PupilLateTodayWidget pupilLateWidget = new PupilLateTodayWidget();
            UnexplainedAbsencesTodayWidget unexplainedWidget = new UnexplainedAbsencesTodayWidget();
            Assert.IsTrue(missingRegisterWidget.IsDisplayedWidget() && pupilLateWidget.IsDisplayedWidget() && unexplainedWidget.IsDisplayedWidget());
        }

        [WebDriverTest(TimeoutSeconds = 1000, Enabled = true, Browsers = new[] { BrowserDefaults.Chrome })]
        [Variant(Variant.EnglishStatePrimary | Variant.WelshStatePrimary | Variant.NorthernIrelandStatePrimary)]
        public void ShouldDisplay_MissingRegister_UnexplainedAbsences_PupilLateWidget_to_PersonalOfficer()
        {
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.PersonnelOfficer);
            MissingRegisterTodayWidget missingRegisterWidget = new MissingRegisterTodayWidget();
            PupilLateTodayWidget pupilLateWidget = new PupilLateTodayWidget();
            UnexplainedAbsencesTodayWidget unexplainedWidget = new UnexplainedAbsencesTodayWidget();
            Assert.IsTrue(missingRegisterWidget.IsDisplayedWidget() && pupilLateWidget.IsDisplayedWidget() && unexplainedWidget.IsDisplayedWidget());
        }

        [WebDriverTest(TimeoutSeconds = 1000, Enabled = true, Browsers = new[] { BrowserDefaults.Chrome })]
        [Variant(Variant.EnglishStatePrimary | Variant.WelshStatePrimary | Variant.NorthernIrelandStatePrimary)]
        public void ShouldDisplay_MissingRegister_UnexplainedAbsences_PupilLateWidget_to_AdmissionOfficer()
        {
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.AdmissionsOfficer);
            MissingRegisterTodayWidget missingRegisterWidget = new MissingRegisterTodayWidget();
            PupilLateTodayWidget pupilLateWidget = new PupilLateTodayWidget();
            UnexplainedAbsencesTodayWidget unexplainedWidget = new UnexplainedAbsencesTodayWidget();
            Assert.IsTrue(missingRegisterWidget.IsDisplayedWidget() && pupilLateWidget.IsDisplayedWidget() && unexplainedWidget.IsDisplayedWidget());
        }

        [WebDriverTest(TimeoutSeconds = 1000, Enabled = false, Browsers = new[] { BrowserDefaults.Chrome })]
        [Variant(Variant.EnglishStatePrimary | Variant.WelshStatePrimary | Variant.NorthernIrelandStatePrimary)]
        public void Should_Not_Display_MissingRegister_UnexplainedAbsences_PupilLateWidget_to_Teacher()
        {
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.ClassTeacher);
            MissingRegisterTodayWidget missingRegisterWidget = new MissingRegisterTodayWidget();
            PupilLateTodayWidget pupilLateWidget = new PupilLateTodayWidget();
            UnexplainedAbsencesTodayWidget unexplainedWidget = new UnexplainedAbsencesTodayWidget();
            Assert.IsFalse(missingRegisterWidget.IsDisplayedWidget() && pupilLateWidget.IsDisplayedWidget() && unexplainedWidget.IsDisplayedWidget());
        }

        [WebDriverTest(TimeoutSeconds = 1000, Enabled = false, Browsers = new[] { BrowserDefaults.Chrome })]
        [Variant(Variant.EnglishStatePrimary | Variant.WelshStatePrimary | Variant.NorthernIrelandStatePrimary)]
        public void Should_Not_Display_MissingRegister_UnexplainedAbsences_PupilLateWidget_to_SeniorManager()
        {
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SeniorManagementTeam);
            MissingRegisterTodayWidget missingRegisterWidget = new MissingRegisterTodayWidget();
            PupilLateTodayWidget pupilLateWidget = new PupilLateTodayWidget();
            UnexplainedAbsencesTodayWidget unexplainedWidget = new UnexplainedAbsencesTodayWidget();
            Assert.IsFalse(missingRegisterWidget.IsDisplayedWidget() && pupilLateWidget.IsDisplayedWidget() && unexplainedWidget.IsDisplayedWidget());
        }
    }
}
