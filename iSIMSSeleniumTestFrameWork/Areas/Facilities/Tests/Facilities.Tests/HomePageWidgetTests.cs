using Facilities.Components.Facilities_Pages;
using NUnit.Framework;
using OpenQA.Selenium;
using Selene.Support.Attributes;
using SharedComponents.Helpers;
using System;
using TestSettings;
using WebDriverRunner.internals;

namespace Facilities.Tests
{
  
    public class HomePageWidgetTests
    {

         #region Story ID 7881:- Show Staff Absence Summary Widget
       [WebDriverTest(Enabled = true, Groups = new[] { "All" }, Browsers = new[] { BrowserDefaults.Chrome})]

        public void StaffAbsentWidget()
        {
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);
            HomePageWidgetPage HomePageWidgetTest = new HomePageWidgetPage();
            Assert.IsTrue(HomePageWidgetTest.IsDisplayedWidget());
        }

    #endregion

    }
}

