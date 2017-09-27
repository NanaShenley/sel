using Facilities.Components.Common;
using NUnit.Framework;
using SharedComponents.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebDriverRunner.internals;
using WebDriverRunner.webdriver;
using Facilities.Components.FacilitiesPages;
using Facilities.Components.Facilities_Pages;
using TestSettings;
using Selene.Support.Attributes;

namespace Facilities.Tests
{
    public class HomePageWidgetPermissionTest
    
    { 
        #region Story ID :- 7883 :- School Admin can see the Staff Absence Summary Widget.
        [WebDriverTest(Enabled = true, Groups = new[] { "P2" }, Browsers = new[] { BrowserDefaults.Chrome })]
        public void StaffAbsenceWidgetSchoolAdmin()
        {
            //Login to SIMS8 Application
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);
            HomePageWidgetPage HomePageWidgetTest = new HomePageWidgetPage();
            Assert.IsTrue(HomePageWidgetTest.IsDisplayedWidget());
        }
        #endregion

        #region Story ID :- 7883 :- Sen Coordinator can see the Staff Absence Summary Widget.
        [WebDriverTest(Enabled = true, Groups = new[] { "P2" }, Browsers = new[] { BrowserDefaults.Chrome })]
        public void StaffAbsenceWidgetSenCoordinator()
        {
            //Login to SIMS8 Application
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SENCoordinator);
            HomePageWidgetPage HomePageWidgetTest = new HomePageWidgetPage();
            Assert.IsTrue(HomePageWidgetTest.IsDisplayedWidget());
        }
        #endregion

        #region Story ID :- 7883 :- Admissions Officer can see the Staff Absence Summary Widget.
        [WebDriverTest(Enabled = true, Groups = new[] { "P2" }, Browsers = new[] { BrowserDefaults.Chrome })]
        public void StaffAbsenceWidgetAdmissionOfficer()
        {
            //Login to SIMS8 Application
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.AdmissionsOfficer);
            HomePageWidgetPage HomePageWidgetTest = new HomePageWidgetPage();
            Assert.IsTrue(HomePageWidgetTest.IsDisplayedWidget());
        }
        #endregion

        #region Story ID :- 7883 :- Assessment Coordinator can see the Staff Absence Summary Widget.
        [WebDriverTest(Enabled = true, Groups = new[] { "P2" }, Browsers = new[] { BrowserDefaults.Chrome})]
        public void StaffAbsenceWidgetAssessmentCoordinator()
        {
            //Login to SIMS8 Application
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.AssessmentCoordinator);
            HomePageWidgetPage HomePageWidgetTest = new HomePageWidgetPage();
            Assert.IsTrue(HomePageWidgetTest.IsDisplayedWidget());
        }
        #endregion

        #region Story ID :- 7883 :- System Manger can see the Staff Absence Summary Widget.
        [WebDriverTest(Enabled = true, Groups = new[] { "P2" }, Browsers = new[] { BrowserDefaults.Chrome})]
        public void StaffAbsenceWidgetSystemManger()
        {
            //Login to SIMS8 Application
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SystemManger);
            HomePageWidgetPage HomePageWidgetTest = new HomePageWidgetPage();
            Assert.IsTrue(HomePageWidgetTest.IsDisplayedWidget());
        }
        #endregion

        #region Story ID :- 7883 :- Curricula Manager can see the Staff Absence Summary Widget.
        [WebDriverTest(Enabled = true, Groups = new[] { "P2" }, Browsers = new[] { BrowserDefaults.Chrome })]
        public void StaffAbsenceWidgetCurricularManager()
        {
            //Login to SIMS8 Application
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.CurricularManager);
            HomePageWidgetPage HomePageWidgetTest = new HomePageWidgetPage();
            Assert.IsTrue(HomePageWidgetTest.IsDisplayedWidget());
        }
        #endregion

        #region Story ID :- 7883 :- Class Teacher can see the Staff Absence Summary Widget.
        [WebDriverTest(Enabled = true, Groups = new[] { "P2" }, Browsers = new[] { BrowserDefaults.Chrome })]
        public void StaffAbsenceWidgetClassTeacher()
        {
            //Login to SIMS8 Application
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.ClassTeacher);
            HomePageWidgetPage HomePageWidgetTest = new HomePageWidgetPage();
            Assert.IsTrue(HomePageWidgetTest.IsDisplayedWidget());
        }
        #endregion

        #region Story ID :- 7883 :- Senior Management Team can see the Staff Absence Summary Widget.
        [WebDriverTest(Enabled = true, Groups = new[] { "P2" }, Browsers = new[] { BrowserDefaults.Chrome })]
        public void StaffAbsenceWidgetSeniorManagementTeam()
        {
            //Login to SIMS8 Application
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SeniorManagementTeam);
            HomePageWidgetPage HomePageWidgetTest = new HomePageWidgetPage();
            Assert.IsTrue(HomePageWidgetTest.IsDisplayedWidget());
        }
        #endregion

        #region  Story ID :- 7883 :- ReturnsManager Team can see the Staff Absence Summary Widget.
        [WebDriverTest(Enabled = true, Groups = new[] { "P2" }, Browsers = new[] { BrowserDefaults.Chrome })]
        public void StaffAbsenceWidgetReturnsManager()
        {
            //Login to SIMS8 Application
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.ReturnsManager);
            HomePageWidgetPage HomePageWidgetTest = new HomePageWidgetPage();
            Assert.IsTrue(HomePageWidgetTest.IsDisplayedWidget());
        }
        #endregion
    }
}
