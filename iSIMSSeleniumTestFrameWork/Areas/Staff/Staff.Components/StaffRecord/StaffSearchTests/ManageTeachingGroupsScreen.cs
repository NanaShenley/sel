﻿using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using SharedComponents.BaseFolder;
using System;
using WebDriverRunner.webdriver;
using SharedComponents.Helpers;
using System.IO;
using Staff.Components.StaffRecord.Enumerations;

namespace Staff.Components.StaffRecord
{
    public class ManageTeachingGroupsScreen : StaffSearchBase
    {
        public static readonly By AddSupervisorsButton = By.CssSelector("[title=\"Add Supervisors\"]");
    }
}
