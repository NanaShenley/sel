using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Facilities.Components.Facilities_Pages;
using SharedComponents.HomePages;
using Facilities.Components.FacilitiesPages;
using SharedComponents.BaseFolder;
using OpenQA.Selenium;
using POM.Helper;
using OpenQA.Selenium.Support.PageObjects;
using WebDriverRunner.webdriver;
using SeSugar.Automation;

namespace Facilities.Components.Common
{
    public class FacilitiesNavigation : BaseSeleniumComponents
    {
        public FacilitiesNavigation()
        {
            PageFactory.InitElements(WebContext.WebDriver, this);
        }

        public static AttendanceKPIPages NavigateToManageKPIPage()
        {
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);
            AutomationSugar.NavigateMenu("Tasks", "School Management", "Key Performance Indicators");
            Wait.WaitForAjaxReady(By.CssSelector(".locking-mask"));
            WaitUntilDisplayed(AttendanceKPIElement.Save);
            return new AttendanceKPIPages();
     
        }
        public static RoomPage NavigateToRoomPage()
        {
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);
            Wait.WaitForDocumentReady();
            AutomationSugar.NavigateMenu("Tasks", "School Management", "Rooms");
            Wait.WaitLoading();
            return new RoomPage();
        }
        public static MySchoolDetailsPage NavigatetoMySchoolDetailPage()
        {
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);
            Wait.WaitForDocumentReady();
            AutomationSugar.NavigateMenu("Tasks", "School Management", "My School Details");
            Wait.WaitLoading();
            //Wait.WaitForAjaxReady(By.CssSelector(".locking-mask"));
            //WaitUntilDisplayed(MySchoolDetailsElements.Save);
            return new MySchoolDetailsPage();
        }

        public static MySchoolDetailsPage NavigatetoMySchoolDetailPageWithFeatureAddresses()
        {
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator, enabledFeatures: "Addresses");
            Wait.WaitForDocumentReady();
            AutomationSugar.NavigateMenu("Tasks", "School Management", "My School Details");
            Wait.WaitLoading();
            //Wait.WaitForAjaxReady(By.CssSelector(".locking-mask"));
            //WaitUntilDisplayed(MySchoolDetailsElements.Save);
            return new MySchoolDetailsPage();
        }

        public static SchemeDetailsPage NavigatetoCurriculumSchemeDetailPage()
        {
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.TestUser, "Curriculum Structure");
            AutomationSugar.NavigateMenu("Tasks", "School Groups", "Curriculum Structure");
            Wait.WaitForAjaxReady(By.CssSelector(".locking-mask"));
            WaitUntilDisplayed(FacilitiesCommonElements.Createbutton);
            return new SchemeDetailsPage();
        }

    }
}
