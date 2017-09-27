using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using SharedComponents.BaseFolder;
using System;
using WebDriverRunner.webdriver;
using SharedComponents.Helpers;
using System.IO;
using Staff.Components.StaffRecord.Enumerations;

namespace Staff.Components.StaffRecord
{
    public class ManageUserDefinedGroupsScreen : StaffSearchBase
    {
        public static readonly By AddMembersButton = By.CssSelector("[data-automation-id=\"add_members_button\"]");
        public static readonly By AddSupervisorsButton = By.CssSelector("[data-automation-id=\"add_supervisors_button\"]"); 
    }
}
