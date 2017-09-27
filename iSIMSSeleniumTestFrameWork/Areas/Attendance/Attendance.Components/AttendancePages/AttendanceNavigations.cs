using Attendance.Components.Common;
using POM.Helper;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using OpenQA.Selenium.Support.UI;
using SharedComponents.BaseFolder;
using SharedComponents.CRUD;
using System;
using System.Collections.ObjectModel;
using WebDriverRunner.webdriver;
using OpenQA.Selenium.Interactions;
using POM.Components.Attendance;
using POM.Components.HomePages;
using SeSugar.Automation;

namespace Attendance.Components.AttendancePages
{
    public class AttendanceNavigations : BaseSeleniumComponents
    {
#pragma warning disable 0649
        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='task_menu_section_attendance_AttendancePattern-']")]
        public IWebElement attendancePatternSubMenu;
        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='task_menu_section_attendance_ExceptionalCircumstance']")]
        public IWebElement exceptionalCircumstanceSubMenu;
        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='task_menu_section_attendance_EarlyYearSetup']")]
        public IWebElement earlyYearsProvisionsSubMenu;
        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='task_menu_section_attendance_ApplyMarkOverDateRange-']")]
        public IWebElement ApplyMarkOverDateRange;

        public AttendanceNavigations()
        {
            PageFactory.InitElements(WebContext.WebDriver, this);
        }

        #region Edit Marks Navigation
        public static AttendanceSearchPanel NavigateToEditMarksMenuPage()
        {
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);
            Wait.WaitForDocumentReady();
            AutomationSugar.NavigateMenu("Tasks", "Attendance", "Edit Marks");
            Wait.WaitLoading();
            return new AttendanceSearchPanel();
        }
        #endregion

        #region Attendance Pattern Navigation
        public static AttendancePatternPage NavigateToAttendancePatternFromTaskMenu()
        {
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);
            Wait.WaitForDocumentReady();
            ShellAction.OpenTaskMenu();
            TaskMenuActions.OpenMenuSection("section_menu_Attendance");
            SubMenuActions.ClickMenuItem("task_menu_section_attendance_AttendancePattern-");
            Wait.WaitLoading();
            return new AttendancePatternPage();
        }

        public static void NavigateToAttendancePattern()
        {
            Wait.WaitForDocumentReady();
            ShellAction.OpenTaskMenu();
            TaskMenuActions.OpenMenuSection("section_menu_Attendance");
            SubMenuActions.ClickMenuItem("task_menu_section_attendance_AttendancePattern-");
            Wait.WaitLoading();
        }

        public static AttendancePatternPage NavigateToAttendancePatternFromQuickSearch()
        {
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);
            ShellAction.OpenTaskMenu();
            SeleniumHelper.Get(AttendanceElements.TaskMenuSearch).SendKeys("Attendance Pattern");
            SeleniumHelper.ClickByJS(SeleniumHelper.Get(AttendanceElements.TaskMenuSearchOption));
            Wait.WaitLoading();
            return new AttendancePatternPage();
        }

        public static AttendancePatternPage NavigateToAttendancePatternPage_OnPupilRecordPage()
        {
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);
            Wait.WaitForDocumentReady();
            var homePage = new HomePage();
            var menubar = homePage.MenuBar();
            menubar.PupilRecords();
            Wait.WaitLoading();
            SearchCriteria.SetCriteria("LegalSurname", "a");
            SearchCriteria.Search();
            SearchResults.WaitForResults();
            SearchResults.SelectSearchResult(1);
            Wait.WaitLoading();
            Wait.WaitForAjaxReady(By.CssSelector(".locking-mask"));
            IWebElement loc = WebContext.WebDriver.FindElement(By.CssSelector("[data-section-id='contextual-actions'] a[data-ajax-workspace='task_menu_section_attendance_AttendancePattern-']"));
            Actions action = new Actions(WebContext.WebDriver);
            action.MoveToElement(loc).Click().Build().Perform();
            Wait.WaitLoading();
            return new AttendancePatternPage();
        }
        #endregion

        #region Exceptional Circumstances Navigation
        public static ExceptionalCircumstancesTriplet NavigateToExceptionalCircumstancePageFromTaskMenu()
        {
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);
            Wait.WaitForDocumentReady();
            AutomationSugar.NavigateMenu("Tasks", "Attendance", "Exceptional Circumstances");
            Wait.WaitLoading();
            return new ExceptionalCircumstancesTriplet();
        }

        public static ExceptionalCircumstancePage NavigateToExceptionalCircumstancePageFromTaskMenu1()
        {
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);
            Wait.WaitForDocumentReady();
            AutomationSugar.NavigateMenu("Tasks", "Attendance", "Exceptional Circumstances");
            Wait.WaitLoading();
            return new ExceptionalCircumstancePage();
        }

        public static ExceptionalCircumstanceSearchPage NavigateToExceptionalCircumstancePage()
        {
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);
            Wait.WaitForDocumentReady();
            AutomationSugar.NavigateMenu("Tasks", "Attendance", "Exceptional Circumstances");
            Wait.WaitLoading();
            return new ExceptionalCircumstanceSearchPage();
        }
        #endregion

        #region Early Years Provisions Navigation
        public static EarlyYearsProvisionsPage NavigateToEarlyYearsProvisionsFromTaskMenu()
        {
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator, "Early Years Provisions");
            Wait.WaitForDocumentReady();
            AutomationSugar.NavigateMenu("Tasks", "Attendance", "Early Years Provisions");
            Wait.WaitLoading();
            return new EarlyYearsProvisionsPage();
        }

        #endregion

        #region ApplyMarkOverDateRange
        public static ApplyMarkOverDateRangePage NavigateToApplyMarkOverDateRangeFromTaskMenu()
        {
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);
            Wait.WaitForDocumentReady();
            ShellAction.OpenTaskMenu();
            TaskMenuActions.OpenMenuSection("section_menu_Attendance");
            SubMenuActions.ClickMenuItem("task_menu_section_attendance_EnterMarkOverDateRange-");
            Wait.WaitLoading();
            return new ApplyMarkOverDateRangePage();
        }

        public static void NavigateToApplyMarkOverDateRange()
        {
            Wait.WaitForDocumentReady();
            ShellAction.OpenTaskMenu();
            TaskMenuActions.OpenMenuSection("section_menu_Attendance");
            SubMenuActions.ClickMenuItem("task_menu_section_attendance_EnterMarkOverDateRange-");
            Wait.WaitLoading();
        }

        #endregion

        #region Deal With Specific Marks Navigation
        public static DealWithSpecifcMarksTriplet NavigateToDealWithSpecificMarksMenuPage()
        {
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);
            Wait.WaitForDocumentReady();
            AutomationSugar.NavigateMenu("Tasks", "Attendance", "Deal with Specific Marks");
            Wait.WaitLoading();
            return new DealWithSpecifcMarksTriplet();
        }
        #endregion

        #region Early Year Session Pattern Navigation
        public static EarlyYearsSessionPatternDialog NavigateToEarlyYearsSessionPatternFromTaskMenu()
        {
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);
            Wait.WaitForDocumentReady();
            ShellAction.OpenTaskMenu();
            TaskMenuActions.OpenMenuSection("section_menu_Attendance");
            SubMenuActions.ClickMenuItem("task_menu_section_attendance_EarlyYear-");
            Wait.WaitLoading();
            return new EarlyYearsSessionPatternDialog();
        }
        #endregion

        #region Common Navigations
        public void NavigateToAttendanceMenu()
        {
            ShellAction.OpenTaskMenu();
            TaskMenuActions.OpenMenuSection("section_menu_Attendance");
            Wait.WaitForDocumentReady();
            Wait.WaitLoading();
        }
    
        public bool SubmenuNotVisibleForClassTeacher(string loc)
        {
            WebDriverWait wait = new WebDriverWait(WebContext.WebDriver, TimeSpan.FromSeconds(5));
            ReadOnlyCollection<IWebElement> list = WebContext.WebDriver.FindElements(By.CssSelector(loc));
            return list.Count == 0;
        }

        public static void ClickDayOrWeekRadioButton(string labelText)
        {
            string xpath = string.Format(".//input[following-sibling::span[contains(., '{0}')]]", labelText);
            WebContext.WebDriver.FindElement(By.XPath(xpath)).Click();
        }

        public static void SelectClass(string className)
        {
            string xpath = string.Format(".//input[following-sibling::label[contains(., '{0}')]]", className);
            WebContext.WebDriver.FindElement(By.XPath(xpath)).Click();
        }

        public static void SelectYear(string yearGroup)
        {
            SeleniumHelper.FindElement(By.XPath("//*[@id='ui - id - 18']/div/div[6]/div/div[1]/div/div[1]/h4/a")).Click();
            SeleniumHelper.Sleep(2);
            string xpath = string.Format(".//input[following-sibling::label[contains(., '{0}')]]", yearGroup);
            WebContext.WebDriver.FindElement(By.XPath(xpath)).Click();
        }

        public static void ClickEditMarksSearchButton()
        {
            SeleniumHelper.FindElement(EditMarksElements.SearchPanel.SearchButton).Click();
        }

        #endregion


    }
}
