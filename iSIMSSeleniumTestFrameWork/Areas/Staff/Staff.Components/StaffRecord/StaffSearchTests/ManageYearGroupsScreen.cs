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
    public class ManageYearGroupsScreen : StaffSearchBase
    {
        #region Const
        private const string _currentStaff = "option[value=\"c03db4ef-682f-747f-e8e5-7007b831294e:Smyth, Macey\"]";
        private const string _previousStaff = "option[value=\"c03db4ef-682f-747f-e8e5-7007b831294e:Smyth, Macey\"]";
        private const string _headOfYearBy = "[name=\"Staff.dropdownImitator\"]";
        #endregion

        #region Actions      
        public void SelectHeadOfYear(string value)
        {
            By loc = By.CssSelector(_headOfYearBy);
            WaitForElement(loc);
        }

        public IWebElement SelectHeadOfYearId(string staffGuid)
        {
            By loc = By.CssSelector(string.Format("[value^=\"{0}\"]", staffGuid));
            return WaitForAndGet(loc);
        }
        #endregion
    }
}
