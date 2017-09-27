using Assessment.Components.Common;
using Assessment.Components.PageObject;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using OpenQA.Selenium.Support.UI;
using SharedComponents.BaseFolder;
using SharedComponents.Helpers;
using SharedComponents.HomePages;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using TestSettings;
using WebDriverRunner.webdriver;

namespace Assessment.Components
{
    public class Mymarksheets : BaseSeleniumComponents
    {
        WebDriverWait waiter = new WebDriverWait(WebContext.WebDriver, TimeSpan.FromSeconds(MarksheetConstants.Timeout));
        [FindsBy(How = How.CssSelector, Using = "div[data-slide-panel-id='groups'] div.slider-header-title.form-body")]
        private IWebElement GroupHeader;

        public Mymarksheets(SeleniumHelper.iSIMSUserType userType = SeleniumHelper.iSIMSUserType.TestUser)
        {
            WebContext.WebDriver.Manage().Window.Maximize();
            PageFactory.InitElements(WebContext.WebDriver, this);

            SeleniumHelper.Login(userType);

        }

        public Mymarksheets OpenMarksheetTask()
        {
            //SeleniumHelper.NavigateMenu("Tasks", "Assessment", "Marksheets");
            CommonFunctions.GotToMarksheetMenu();
            SeSugar.Automation.AutomationSugar.WaitForAjaxCompletion();
            PageFactory.InitElements(WebContext.WebDriver, this);
            return this;
        }

        public void CreateMarksheetForSchoolGroup(string className, string year)
        {
            SeleniumHelper.NavigateMenu("Tasks", "Assessment", "Set Up Marksheets and Parental Reports");
            //Create page object of marksheet home
            CreateMarksheetTypeSelectionPage marksheetTypeMenuPage = new CreateMarksheetTypeSelectionPage();
            MarksheetBuilder marksheetBuilder = (MarksheetBuilder)marksheetTypeMenuPage.MarksheetTypeSelection("New Template");

            //Randomly generate Unique Marksheet Name
            var MarksheetTemplateName = marksheetBuilder.RandomString(8);
            //Verifying the saved marksheet     
            marksheetBuilder.setMarksheetProperties(MarksheetTemplateName, "Description", true);
            AddAssessments addAssessments = marksheetBuilder.NavigateAssessments();
            AddAspects addAspects = addAssessments.NavigateAssessmentsviaAssessment();

            addAspects.SelectNReturnSelectedAssessments(2);
            AddAssessmentPeriod addAssessmentPeriod = addAspects.AspectNextButton();
            addAssessmentPeriod.AspectAssessmentPeriodSelection(2);
            addAssessmentPeriod.ClickAspectAssessmentPeriodDone();
            WaitUntilDisplayed(BrowserDefaults.TimeOut, By.LinkText("School Groups"));
            WaitForAndClick(BrowserDefaults.TimeOut, By.LinkText("School Groups"));
            ReadOnlyCollection<IWebElement> YearGroupCheckboxlistYG = WebContext.WebDriver.FindElements(By.CssSelector("input[name='YearGroups.SelectedIds']"));
            foreach (IWebElement eachelement in YearGroupCheckboxlistYG)
            {
                if (
                    WebContext.WebDriver.FindElement(
                        By.CssSelector("label[for='" + eachelement.GetAttribute("id") + "']")).Text == year)
                {
                    eachelement.Click();
                    break;
                }
            }

            if (!string.IsNullOrEmpty(className))
            {
                ReadOnlyCollection<IWebElement> classesList = WebContext.WebDriver.FindElements(By.CssSelector("input[name='Classes.SelectedIds']"));
                foreach (IWebElement classElement in classesList)
                {
                    if (
                        WebContext.WebDriver.FindElement(
                            By.CssSelector("label[for='" + classElement.GetAttribute("id") + "']")).Text == className)
                    {
                        classElement.Click();
                        break;
                    }
                }
            }

            WaitForAndClick(BrowserDefaults.TimeOut, By.CssSelector("button[data-automation-id='next-group']"));
            WebContext.WebDriver.FindElement(By.CssSelector("[data-automation-id='save_button']")).Click();
        }


        public void CreateMarksheetForYearGroup(int NoOfYearGroups)
        {
            SeleniumHelper.NavigateMenu("Tasks", "Assessment", "Set Up Marksheets and Parental Reports");
            WebDriverWait waiter = new WebDriverWait(WebContext.WebDriver, TimeSpan.FromSeconds(MarksheetConstants.Timeout));
            waiter.Until(ExpectedConditions.ElementExists(By.LinkText("Create Marksheet")));
            IWebElement createMarksheetLink = WebContext.WebDriver.FindElement(By.LinkText("Create Marksheet"));
            createMarksheetLink.Click();
            WaitUntilDisplayed(MarksheetConstants.MarksheetWithLevels);
            WaitForAndClick(BrowserDefaults.TimeOut, MarksheetConstants.MarksheetWithLevels);
            WaitUntilDisplayed(BrowserDefaults.TimeOut, By.CssSelector("[name='MarksheetTemplateName']"));
            WebContext.WebDriver.FindElement(By.CssSelector("[name='MarksheetTemplateName']")).SendKeys("hoy marksheet1");
            WaitUntilDisplayed(BrowserDefaults.TimeOut, By.LinkText("School Groups"));
            WaitForAndClick(BrowserDefaults.TimeOut, By.LinkText("School Groups"));
            // waiter.Until(ExpectedConditions.TextToBePresentInElement(GroupHeader, "School Groups"));
            ReadOnlyCollection<IWebElement> YearGroupCheckboxlistYG = WebContext.WebDriver.FindElements(By.CssSelector("input[name='YearGroups.SelectedIds']"));

            int i = 0;
            foreach (IWebElement YearGroupElem in YearGroupCheckboxlistYG)
            {
                if (YearGroupElem.Displayed == true && NoOfYearGroups != 0)
                {
                    waiter.Until(ExpectedConditions.ElementToBeClickable(YearGroupElem));
                    YearGroupElem.Click();

                    i++;
                }
                if (i == NoOfYearGroups)
                    break;
            }


            WebContext.WebDriver.FindElement(By.CssSelector("[data-automation-id='save_button']")).Click();
        }

        public bool IsMyMarksheetsQuickLink()
        {
            WaitUntilDisplayed(BrowserDefaults.TimeOut, By.CssSelector("[data-automation-id='quicklinks_top_level_assessment_submenu_marksheets']"));
            return WebContext.WebDriver.FindElement(By.CssSelector("[data-automation-id='quicklinks_top_level_assessment_submenu_marksheets']")) !=
            null;
        }

        public Mymarksheets OpenMymarksheets()
        {
            WaitUntilDisplayed(BrowserDefaults.TimeOut, By.CssSelector("[data-automation-id='quicklinks_top_level_assessment_submenu_marksheets']"));
            WaitForAndClick(BrowserDefaults.TimeOut, By.CssSelector("[data-automation-id='quicklinks_top_level_assessment_submenu_marksheets']"));
            return this;
        }

        public Mymarksheets FilterMarksheets()
        {
            WaitForAndClick(BrowserDefaults.TimeOut, By.CssSelector("[data-automation-id='search_criteria_submit']"));
            WaitForAndClick(BrowserDefaults.TimeOut, By.CssSelector("[data-automation-id='control_checkboxlist_rootnode_checkbox_Year_Group']"));
            WaitForAndClick(BrowserDefaults.TimeOut, By.CssSelector("[data-automation-id='search_criteria_submit']"));
            WaitForAndClick(BrowserDefaults.TimeOut, By.CssSelector("[data-automation-id='section_menu_Class']"));
            WebContext.WebDriver.FindElements(By.CssSelector("[name='Classes.SelectedIds']")).First().Click();
            WaitForAndClick(BrowserDefaults.TimeOut, By.CssSelector("[data-automation-id='search_criteria_submit']"));

            return this;
        }

        public Mymarksheets IsMyMarksheet()
        {
            WaitUntilDisplayed(By.CssSelector("[name='IsMyMarksheet']"));
            WaitForAndClick(BrowserDefaults.TimeOut, By.CssSelector("[name='IsMyMarksheet']"));
            WaitForAndClick(BrowserDefaults.TimeOut, By.CssSelector("[data-automation-id='search_criteria_submit']"));

            return this;
        }

        public bool IsMyMarksheetsContextualLink()
        {
            WebContext.WebDriver.FindElement(By.CssSelector("[data-toggle='right-panel']"));
            return false;
        }

        public void LoginByNewCreatedUser(string userid, string password, string newPassword)
        {
            SeleniumHelper.Login(
                        userid,
                        password,
                        TestDefaults.Default.TenantId,
                        TestDefaults.Default.SchoolName,
                        Configuration.GetSutUrl(),
                        false);

            WaitUntilDisplayed(BrowserDefaults.TimeOut, By.CssSelector("[name='Password']"));
            WebContext.WebDriver.FindElement(By.CssSelector("[name='CurrentPassword']")).SendKeys(password);
            WebContext.WebDriver.FindElement(By.CssSelector("[name='Password']")).SendKeys(newPassword);
            WebContext.WebDriver.FindElement(By.CssSelector("[name='ConfirmPassword']")).SendKeys(newPassword);
            WaitForAndClick(BrowserDefaults.TimeOut, By.CssSelector("[class='btn btn-default btn-block btn-lg']"));
        }

        public bool CreateClassTeacher(string userId, string password)
        {
            SeleniumHelper.NavigateMenu("Tasks", "System Manager", "Manage Users");
            PageFactory.InitElements(WebContext.WebDriver, this);

            WaitUntilDisplayed(BrowserDefaults.TimeOut, By.CssSelector("[data-automation-id='create_button']"));
            WaitForAndClick(BrowserDefaults.TimeOut, By.CssSelector("[data-automation-id='create_button']")); //
            WaitForAndClick(BrowserDefaults.TimeOut, By.CssSelector("[data-automation-id='create_a_new_account_button']"));
            WaitUntilDisplayed(BrowserDefaults.TimeOut, By.CssSelector("[name='LoginName']"));

            WebContext.WebDriver.FindElement(By.CssSelector("[name='LoginName']")).SendKeys(userId);
            WebContext.WebDriver.FindElement(By.CssSelector("[name='Password']")).SendKeys(password);
            WebContext.WebDriver.FindElement(By.CssSelector("[name='ConfirmPassword']")).SendKeys(password);
            WaitForAndClick(BrowserDefaults.TimeOut, By.CssSelector("[data-dialog-action-set-focus='']"));
            WaitForAndClick(BrowserDefaults.TimeOut, By.CssSelector("[data-automation-id='well_know_action_save']"));

            WaitUntilDisplayed(BrowserDefaults.TimeOut, By.CssSelector("[data-automation-id='status_success']"));
            return WebContext.WebDriver.FindElement(By.CssSelector("[data-automation-id='status_success']")) != null;
        }

        public string FindStaffForClass(out string className)
        {
            SeleniumHelper.NavigateMenu("Tasks", "School Groups", "Manage Classes");
            PageFactory.InitElements(WebContext.WebDriver, this);

            WaitUntilDisplayed(BrowserDefaults.TimeOut, By.CssSelector("[data-automation-id='search_criteria_submit']"));
            WaitForAndClick(BrowserDefaults.TimeOut, By.CssSelector("[data-automation-id='search_criteria_submit']"));
            WaitUntilDisplayed(BrowserDefaults.TimeOut, By.CssSelector("[data-automation-id='resultTile']"));
            IWebElement firstClass = WebContext.WebDriver.FindElements(By.CssSelector("[data-automation-id='resultTile']")).FirstOrDefault();
            className = firstClass.Text;
            firstClass.Click();
            WaitUntilDisplayed(BrowserDefaults.TimeOut, By.CssSelector("[data-automation-id='section_menu_Staff Details']"));
            WaitForAndClick(BrowserDefaults.TimeOut, By.CssSelector("[data-automation-id='section_menu_Staff Details']"));

            IWebElement classTeacherTablElement = WebContext.WebDriver.FindElements(By.CssSelector("[data-row-name='PrimaryClassTeachers']")).First();
            IWebElement classTeacherRowElement = classTeacherTablElement.FindElements(By.CssSelector("[class='grid-cell']"))[1];
            IWebElement classTeacherCellElement = classTeacherRowElement.FindElement(By.CssSelector("[class='form-group grid-cell-control']"));
            IWebElement classTeacherTextElement = classTeacherTablElement.FindChild(By.CssSelector("[data-select-imitator='']"));

            string staff = classTeacherTextElement.GetAttribute("value");
            return staff;
        }


        public string FindHeadOfYear(string year)
        {
            SeleniumHelper.NavigateMenu("Tasks", "School Groups", "Manage Year Groups");
            PageFactory.InitElements(WebContext.WebDriver, this);

            WaitUntilDisplayed(BrowserDefaults.TimeOut, By.CssSelector("[data-automation-id='search_criteria_submit']"));
            WaitForAndClick(BrowserDefaults.TimeOut, By.CssSelector("[data-automation-id='search_criteria_submit']"));
            WaitUntilDisplayed(BrowserDefaults.TimeOut, By.LinkText(year));
            WaitForAndClick(BrowserDefaults.TimeOut, By.LinkText(year));//
            WaitUntilDisplayed(BrowserDefaults.TimeOut, By.CssSelector("[data-automation-id='section_menu_Staff Details']"));
            WaitForAndClick(BrowserDefaults.TimeOut, By.CssSelector("[data-automation-id='section_menu_Staff Details']"));

            IWebElement headOfYearTablElement = WebContext.WebDriver.FindElements(By.CssSelector("[data-row-name='HeadOfYears']")).First();
            IWebElement headOfYearRowElement =
                headOfYearTablElement.FindElements(By.CssSelector("[class='grid-cell']"))[1];
            IWebElement classTeacherCellElement =
                headOfYearRowElement.FindElement(By.CssSelector("[class='form-group grid-cell-control']"));
            IWebElement classTeacherTextElement =
                headOfYearTablElement.FindChild(By.CssSelector("[data-select-imitator='']"));
            string hearOfYear = classTeacherTextElement.GetAttribute("value");
            return hearOfYear;
        }

        public string FindStaff(string year)
        {
            SeleniumHelper.NavigateMenu("Tasks", "School Groups", "Manage Year Groups");
            PageFactory.InitElements(WebContext.WebDriver, this);

            WaitUntilDisplayed(BrowserDefaults.TimeOut, By.CssSelector("[data-automation-id='search_criteria_submit']"));
            WaitForAndClick(BrowserDefaults.TimeOut, By.CssSelector("[data-automation-id='search_criteria_submit']"));
            WaitUntilDisplayed(BrowserDefaults.TimeOut, By.LinkText(year));
            WaitForAndClick(BrowserDefaults.TimeOut, By.LinkText(year));//
            WaitUntilDisplayed(BrowserDefaults.TimeOut, By.CssSelector("[data-automation-id='section_menu_Staff Details']"));
            WaitForAndClick(BrowserDefaults.TimeOut, By.CssSelector("[data-automation-id='section_menu_Staff Details']"));

            IWebElement headOfYearTablElement = WebContext.WebDriver.FindElements(By.CssSelector("[data-row-name='YearGroupStaffs']")).First();
            IWebElement headOfYearRowElement =
                headOfYearTablElement.FindElements(By.CssSelector("[class='grid-cell']"))[1];
            IWebElement classTeacherCellElement =
                headOfYearRowElement.FindElement(By.CssSelector("[class='form-group grid-cell-control']"));
            IWebElement classTeacherTextElement =
                headOfYearTablElement.FindChild(By.CssSelector("[data-select-imitator='']"));
            string hearOfYear = classTeacherTextElement.GetAttribute("value");
            return hearOfYear;
        }

        public void AllocateStaffToUser(string staffName)
        {
            SeleniumHelper.NavigateMenu("Tasks", "System Manager", "Manage Users");
            PageFactory.InitElements(WebContext.WebDriver, this);
            WaitUntilDisplayed(BrowserDefaults.TimeOut, By.CssSelector("[data-automation-id='search_criteria_submit']"));
            WaitForAndClick(BrowserDefaults.TimeOut, By.CssSelector("[data-automation-id='search_criteria_submit']"));
            WaitUntilDisplayed(BrowserDefaults.TimeOut, By.LinkText("Teacher@capita.co.uk"));
            WaitForAndClick(BrowserDefaults.TimeOut, By.LinkText("Teacher@capita.co.uk"));
            WaitUntilDisplayed(BrowserDefaults.TimeOut, By.CssSelector("[data-automation-id='section_menu_Personal Details']"));
            //WaitForAndClick(BrowserDefaults.TimeOut, By.CssSelector("[data-automation-id='section_menu_Personal Details']"));
            WaitForAndClick(BrowserDefaults.TimeOut, By.CssSelector("[data-automation-id='find_a_member_of_staff_button']"));
            WaitUntilDisplayed(BrowserDefaults.TimeOut, By.CssSelector("[name='LegalSurname']"));
            WebContext.WebDriver.FindElement(By.CssSelector("[name='LegalSurname']")).SendKeys(staffName);
            WaitForAndClick(BrowserDefaults.TimeOut, By.CssSelector("[data-ajax-url='/iSIMSMVCClientFarm1/SystemManager/Staff/Search']"));
            Thread.Sleep(1000);
            WebContext.WebDriver.FindElements(By.CssSelector("[data-section-id='dialog-searchResults']"))
                .FirstOrDefault(ele => ele.Displayed)
                .FindChild(By.CssSelector("[data-automation-id='search_results']"))
                .FindChild(By.CssSelector("[data-automation-id='search_results_list']"))
                .FindElements(By.CssSelector("[data-automation-id='search_result']")).First().Click();
            WebContext.WebDriver.FindElements(By.CssSelector("[data-dialog-action-set-focus='']"))
                .FirstOrDefault(ele => ele.Displayed)
                .Click();
            WaitForAndClick(BrowserDefaults.TimeOut, By.CssSelector("[data-automation-id='well_know_action_save']"));
            Thread.Sleep(1000);
        }

        public void SignOut()
        {
            WaitForAndClick(BrowserDefaults.TimeOut, By.CssSelector("[data-automation-id='TopMenuUserName']"));
            WaitForAndClick(BrowserDefaults.TimeOut, By.CssSelector("[data-automation-id='TopMenuSignOut']"));
            WaitUntilDisplayed(BrowserDefaults.TimeOut, By.CssSelector("[data-automation-id='return-sign-out']"));
            WaitForAndClick(BrowserDefaults.TimeOut, By.CssSelector("[data-automation-id='return-sign-out']"));
        }
    }
}
