using SharedComponents.BaseFolder;
using OpenQA.Selenium.Support.PageObjects;
using SharedComponents.Helpers;
using SharedComponents;
using WebDriverRunner.webdriver;
using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using TestSettings;
using SharedComponents.HomePages;
using Assessment.Components.Common;

namespace FeatureBee
{
    public class FeatureBeeLogin : BaseSeleniumComponents
    {
        public FeatureBeeLogin()
        {
            PageFactory.InitElements(WebContext.WebDriver, this);
        }

        WebDriverWait wait = new WebDriverWait(WebContext.WebDriver, TimeSpan.FromSeconds(MarksheetConstants.Timeout));

        //Method to sign in through specific user and enable featureBee
        public static void LoginWithFeatureBee(string[] featureList, iSIMSUserType userType = iSIMSUserType.TestUser)
        {
            Login(userType, featureList);
        }

        /// <summary>
        /// The types of iSIMS user.
        /// </summary>
        public enum iSIMSUserType
        {
            TestUser,
            PersonnelOfficer,
            SchoolAdministrator,
            SENCoordinator,
            AdmissionsOfficer,
            HeadTeacher,
            SystemManger,
            AssessmentCoordinator,
            CurricularManager,
            SeniorManagementTeam,
            ClassTeacher,
            ReturnsManager

        }

        /// <summary>
        /// Log in as a given user.
        /// </summary>
        /// <param name="userType">The user type to login as.</param>
        /// <param name="enabledFeatures">The FeatureBee features to enable.</param>
        public static void Login(iSIMSUserType userType, params string[] enabledFeatures)
        {
            if (TestSettings.Configuration.ForceTestUserLogin) userType = iSIMSUserType.TestUser;

            switch (userType)
            {
                case iSIMSUserType.TestUser:
                    Login(
                        TestDefaults.Default.TestUser,
                        TestDefaults.Default.TestUserPassword,
                        TestDefaults.Default.TenantId,
                        TestDefaults.Default.SchoolName,
                        Configuration.GetSutUrl(),
                        true,
                        enabledFeatures);
                    break;
                case iSIMSUserType.SchoolAdministrator:
                    Login(
                        TestDefaults.Default.SchoolAdministratorUser,
                        TestDefaults.Default.SchoolAdministratorPassword,
                        TestDefaults.Default.TenantId,
                        TestDefaults.Default.SchoolName,
                        Configuration.GetSutUrl(),
                        false,
                        enabledFeatures);
                    break;
                case iSIMSUserType.SENCoordinator:
                    Login(
                        TestDefaults.Default.SENCoordinator,
                        TestDefaults.Default.SENCoordinatorPassword,
                        TestDefaults.Default.TenantId,
                        TestDefaults.Default.SchoolName,
                        Configuration.GetSutUrl(),
                        false,
                        enabledFeatures);
                    break;
                case iSIMSUserType.AdmissionsOfficer:
                    Login(
                        TestDefaults.Default.AdmissionsOfficer,
                        TestDefaults.Default.AdmissionsOfficerPassword,
                        TestDefaults.Default.TenantId,
                        TestDefaults.Default.SchoolName,
                        Configuration.GetSutUrl(),
                        false,
                        enabledFeatures);
                    break;
                case iSIMSUserType.HeadTeacher:
                    Login(
                        TestDefaults.Default.HeadTeacher,
                        TestDefaults.Default.HeadTeacherPassword,
                        TestDefaults.Default.TenantId,
                        TestDefaults.Default.SchoolName,
                        Configuration.GetSutUrl(),
                        false,
                        enabledFeatures);
                    break;
                case iSIMSUserType.SystemManger:
                    Login(
                        TestDefaults.Default.SystemManger,
                        TestDefaults.Default.SystemMangerPassword,
                        TestDefaults.Default.TenantId,
                        TestDefaults.Default.SchoolName,
                        Configuration.GetSutUrl(),
                        false,
                        enabledFeatures);
                    break;
                case iSIMSUserType.AssessmentCoordinator:
                    Login(
                        TestDefaults.Default.AssessmentCoordinator,
                        TestDefaults.Default.AssessmentCoordinatorPassword,
                        TestDefaults.Default.TenantId,
                        TestDefaults.Default.SchoolName,
                        Configuration.GetSutUrl(),
                        false,
                        enabledFeatures);
                    break;
                case iSIMSUserType.CurricularManager:
                    Login(
                        TestDefaults.Default.CurricularManager,
                        TestDefaults.Default.CurricularManagerPassword,
                        TestDefaults.Default.TenantId,
                        TestDefaults.Default.SchoolName,
                        Configuration.GetSutUrl(),
                        false,
                        enabledFeatures);
                    break;
                case iSIMSUserType.SeniorManagementTeam:
                    Login(
                        TestDefaults.Default.SeniorManagementTeam,
                        TestDefaults.Default.SeniorManagementTeamPassword,
                        TestDefaults.Default.TenantId,
                        TestDefaults.Default.SchoolName,
                        Configuration.GetSutUrl(),
                        false,
                        enabledFeatures);
                    break;
                case iSIMSUserType.ClassTeacher:
                    Login(
                        TestDefaults.Default.ClassTeacher,
                        TestDefaults.Default.ClassTeacherPassword,
                        TestDefaults.Default.TenantId,
                        TestDefaults.Default.SchoolName,
                        Configuration.GetSutUrl(),
                        false,
                        enabledFeatures);
                    break;
                case iSIMSUserType.ReturnsManager:
                    Login(
                        TestDefaults.Default.ReturnsManager,
                        TestDefaults.Default.ReturnsManagerPassword,
                        TestDefaults.Default.TenantId,
                        TestDefaults.Default.SchoolName,
                        Configuration.GetSutUrl(),
                        false,
                        enabledFeatures);
                    break;
                case iSIMSUserType.PersonnelOfficer:
                    Login(
                        TestDefaults.Default.PersonnelOfficer,
                        TestDefaults.Default.PersonnelOfficerPassword,
                        TestDefaults.Default.TenantId,
                        TestDefaults.Default.SchoolName,
                        Configuration.GetSutUrl(),
                        false,
                        enabledFeatures);
                    break;
                default:
                    throw new ArgumentOutOfRangeException("userType");
            }
        }

        /// <summary>
        /// Log in to iSIMS.
        /// </summary>
        public static void Login(string user, string password, int tenantId, string schoolName, string url, bool schoolSelection, params string[] enabledFeatures)
        {
            WebContext.WebDriver.Manage().Window.Maximize();
            HomePage homePage = HomePage.NavigateTo(user, password, tenantId, schoolName, url, schoolSelection, enabledFeatures);
            /*
            if (schoolSelection)
                homePage.MenuBar().WaitFor();
             */
            WaitForDocumentReady();
        }

        /// <summary>
        /// Au: Logigear
        /// Des: Wait for a document is ready. Use same WaitForPageLoad()
        /// </summary>
        public static void WaitForDocumentReady()
        {
            IWebDriver driver = WebContext.WebDriver;
            var timeout = new TimeSpan(0, 5, 0);
            var wait = new WebDriverWait(driver, timeout);

            var javascript = driver as IJavaScriptExecutor;
            if (javascript == null)
                throw new ArgumentException("driver", "Driver must support javascript execution");

            wait.Until((d) =>
            {
                try
                {
                    string readyState = javascript.ExecuteScript(
                        "if (document.readyState) return document.readyState;").ToString();
                    return readyState.ToLower() == "complete";
                }
                catch (InvalidOperationException e)
                {
                    //Window is no longer available
                    return e.Message.ToLower().Contains("unable to get browser");
                }
                catch (WebDriverException e)
                {
                    //Browser is no longer available
                    return e.Message.ToLower().Contains("unable to connect");
                }
                catch (Exception)
                {
                    return false;
                }
            });

        }

    }

   
}

