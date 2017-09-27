using System;
using System.Threading;
using OpenQA.Selenium;
using Admissions.Component;
using SeSugar.Automation;
using SharedComponents.Helpers;

namespace Admissions.Component
{
    public class AdmissionsBulkUpdateNavigation
    {
        //private SeleniumHelper.iSIMSUserType LoginAs = SeleniumHelper.iSIMSUserType.SchoolAdministrator;
        private const SeleniumHelper.iSIMSUserType LoginAs = SeleniumHelper.iSIMSUserType.AdmissionsOfficer;
        /// <summary>
        /// Navigates to Bulk update page from Pupil Menu.
        /// </summary>
        /// <param name="userType">The user type to login as.</param>
        /// <param name="schoolSelection">show school selection?</param>
        public void NavigateToPupilBulkUpdateMenuPage(SeleniumHelper.iSIMSUserType userType = LoginAs, bool schoolSelection = true)
        {
            SeleniumHelper.Login(userType, schoolSelection);
            Thread.Sleep(1000);
           
            AutomationSugar.NavigateMenu("Tasks","Admissions","Bulk Update");
           
            Thread.Sleep(1000);
          
        }

        public void NavigateToAdmissionsBulkUpdateMenuPage(SeleniumHelper.iSIMSUserType userType = LoginAs, bool schoolSelection = true)
        {
            SeleniumHelper.Login(userType, schoolSelection);
            SeleniumHelper.FindAndClick("[data-automation-id='task_menu']");
            Thread.Sleep(1000);
            SeleniumHelper.FindAndClick("[data-automation-id='section_menu_Admissions']");
            Thread.Sleep(1000);
            SeleniumHelper.FindAndClick(AdmissionsBulkUpdateElements.Menu.MenuBulkUpdateApplicationDetail);
            Thread.Sleep(1000);
        }

        /// <summary>
        /// Navigates to Bulk update page from Pupil Menu.
        /// </summary>
        public void NavigateToPupilBulkUpdateMenuPageNoLogin()
        {
            AutomationSugar.NavigateMenu("Tasks", "Pupils", "Bulk Update");
            SeleniumHelper.FindAndClick(AdmissionsBulkUpdateElements.BulkUpdate.MenuItems.ApplicantApplicationStatusMenuItem);
        }

        /// <summary>
        /// Navgates to Search on Pupils / Bulk Update Screens
        /// </summary>
        /// <param name="subMenu"></param>
        /// <param name="userType">The user type to login as.</param>
        public void NavgateToPupilBulkUpdate_SubMenu(By subMenu, SeleniumHelper.iSIMSUserType userType = LoginAs)
        {
            NavigateToAdmissionsBulkUpdateMenuPage(userType);
            SeleniumHelper.FindAndClick(subMenu, new TimeSpan(0, 0, 60));
        }

        /// <summary>
        /// Determines whether the Bulk Update 'Back' button is visible
        /// </summary>
        /// <returns></returns>
        public bool BackButtonIsVisible()
        {
            return SeleniumHelper.GetVisible(AdmissionsBulkUpdateElements.BulkUpdate.MenuItems.PupilBulkUpdateBackButton) != null;
        }

        /// <summary>
        /// Clicks the visible Back button
        /// </summary>
        public void BackButtonClick()
        {
            SeleniumHelper.FindAndClick(AdmissionsBulkUpdateElements.BulkUpdate.MenuItems.PupilBulkUpdateBackButton);
        }

        public void NavigateToBulkUpdateApplicationStatus(SeleniumHelper.iSIMSUserType userType = LoginAs)
        {
            NavigateToAdmissionsBulkUpdateMenuPage(userType);
            SeleniumHelper.FindAndClick(AdmissionsBulkUpdateElements.BulkUpdate.MenuItems.ApplicantApplicationStatusMenuItem);
        }

        public void NavigateToBulkUpdateApplicantSalutationAndAddressee(SeleniumHelper.iSIMSUserType userType = LoginAs)
        {
            NavigateToAdmissionsBulkUpdateMenuPage(userType);
            SeleniumHelper.FindAndClick(AdmissionsBulkUpdateElements.BulkUpdate.MenuItems.ApplicantSalutationAddresseeMenuItem);
        }
    }
}