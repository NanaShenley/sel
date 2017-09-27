using Assessment.Components.Common;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.PageObjects;
using OpenQA.Selenium.Support.UI;
using SharedComponents.BaseFolder;
using SharedComponents.Helpers;
using SharedComponents.HomePages;
using SharedComponents.LoginPages;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using TestSettings;
using WebDriverRunner.webdriver;
using SeSugar.Automation;
using System.Text;

namespace Assessment.Components
{
    public class MarksheetGridHelper : BaseSeleniumComponents
    {
        //selectors


        public const string GridMenueItemCaretSelector = "[class=\"fa fa-angle-down fa-fw\"]";
        public const string GridMenueItemSortSelector = "[data-grid-id=\"marksheetGrid\"]";
        public const string DistributionGraphPopoverSelector = "[data-show-distribution-chart ='']";
        public const string ColumndetailsPopoverSelector = "[data-show-column-details-popover='']";
        public const string Optionselector = "[data-toggle-top-panel=\"\"]";
        public const string CoulmnsdivSelector = "[class=\"webix_hs_center\"]";
        public const string CellsDivSelector = "[class=\"webix_ss_center\"]";
        public const string EditableCellsSelector = "[class='webix_column ']";
        public const string TextRightCellSelector = "[class='webix_column text-right']";
        public const string CellSelector = "[class*='webix_cell']";
        public const string SelectedCellSelector = "[class*='webix_cell_select']";
        public const string FormulaBarSelector = "[class=\"form-control formula-box\"]";
        public const string HorizontalScrollBarSelector = "[class='webix_ss_hscroll webix_vscroll_x']";
        public const string AdditionalColumnsButtonSelector = "[data-show-additional-columns-modal='']";
        public const string AdditionalColumnPercentageAttendanceCheckboxselector = "div[webix_tm_id='Learner.ExtendedFields.PercentAttendance'] > input[type='checkbox']";
        public const string AdditionalColumnAsylumStatusCheckboxselector = "div[webix_tm_id='Learner.AsylumSeeker'] > input[type='checkbox']";
        public const string AdditionalColumnLearnerInCareCheckboxselector = "div[webix_tm_id='Learner.ExtendedFields.LearnerInCareDetail'] > input[type='checkbox']";
        public const string propertycolumnsselection = "[class=\"webix_ss_center_scroll\"]";
        public const string PersonalDetailsCheckBoxSelector = "div[webix_tm_id='Personal Details'] > input[type='checkbox']";
        public const string PropertiesGridId = "marksheetproperties";
        public const string OkAdditionalColumsButtonId = "savePreferences";
        //public const string EditorSelector ="[class='webix_dt_editor'] > input[type='text']";
        public const string EditorSelector = "//div[contains(@class,'webix_dt_editor')]/input[@type='text']";
        public const string ResultHistorySelector = "//div[contains(@class,'webix_dt_editor')]/div[contains(@class,'btn-group')]/button";
        public const string EditorAreaSelector = "//div[contains(@class,'webix_dt_editor')]/textarea";
        public const string RecordingYear2Marksheet = "Recording Year 2 - Year 2";
        public const string RecordingYear1Marksheet = "Recording Year 1 - Year 1";
        public const string RecordingMistYear2Marksheet = "Recording MIST Year 2 - Year 2";
        public const string AnalysisMathsYear3Marksheet = "Analysis Maths Year on Year - Year 4";
        public const string AnalysisEnglishYear2Marksheet = "Analysis English Year on Year - Year 2";
        public const string LoPYear4Marksheet = "Levels of Progression Key Stage 1 - Year 4";
        public const string AnnualCommentsYear1Markshett = "Annual Pupil Profile Comments Year 1 - Year 1";
        public const string ExtendedDropdown = "[class='dropdown']";
        public const string VerticalColumnSwitch = "[name='IsColumnNarrowed']";
        public const string MArksheetLIst = "data-target='[data-crud -screen]'";
        public const string FiltersButton = "[data-action-show-search='']";
             

        private readonly static string UrlUndertest = Configuration.GetSutUrl();
        private readonly static string Testuser = TestDefaults.Default.TestUser;
        public readonly static string Password = TestDefaults.Default.TestUserPassword;
        private readonly static string SchoolName = TestDefaults.Default.SchoolName;
        private static readonly int TenantId = TestDefaults.Default.TenantId;
        public readonly static string SENCordUser = TestDefaults.Default.SENCoordinator;
        public readonly static string SchoolAdminUser = TestDefaults.Default.SchoolAdministratorUser;
        public readonly static string AssessmentCoordUser = TestDefaults.Default.AssessmentCoordinator;
        public readonly static string ClassTeacherUser = TestDefaults.Default.ClassTeacher;
        [FindsBy(How = How.LinkText, Using = "Marksheets")]
        private IWebElement _marksheetsLink;
        [FindsBy(How = How.LinkText, Using = "Analysis English Year on Year - Year 4")]
        private IWebElement _marksheetsName;  
        public MarksheetGridHelper()
        {
            PageFactory.InitElements(WebContext.WebDriver, this);
        }
        public MarksheetGridHelper NavigatetoMarksheets()
        {
            WaitForElement(By.LinkText("Marksheets"));
            _marksheetsLink.Click();
            //WaitForElement(By.LinkText("Analysis English Year on Year - Year 4"));
            return this;
        }
        public MarksheetGridHelper SearchForMarksheet(string marksheetName = "")
        {
            Thread.Sleep(2000);
            if (!string.IsNullOrEmpty(marksheetName))
            {
                WaitForElement(By.LinkText(marksheetName));
                _marksheetsName = WebContext.WebDriver.FindElement(By.LinkText(marksheetName));
            }
            _marksheetsName.Click();
            return this;
        }
        //To Sign in with other user
        public static void SignInAsUser(string user, string pwd)
        {
            var page = SignInPage.NavigateTo(UrlUndertest, "iSIMSMVCClientFarm1/");
            page.EnterUserId(user);
            page.EnterPassword(pwd);
            page.SignIn();
            //Reporter.Log("Sign in ok with " + Testuser);

            var tenantPage = new SelectTenantPage();
            //tenantPage.ValidateElements();
            tenantPage.EnterTenant(TestDefaults.Default.TenantId.ToString()).Submit();

            //var school = new SelectSchool();
            //school.SelectBySchoolName("Abbey Hill Primary School");
            //school.SignIn();
            TaskMenuBar bar = new TaskMenuBar();
            bar.WaitFor().ClickTaskMenuBar().ClickAssessmentLink();
        }
        //Opens the given marksheet
        public static void OpenMarksheet(string marksheet)
        {
            WebContext.WebDriver.Manage().Window.Maximize();
            const SeleniumHelper.iSIMSUserType userType = SeleniumHelper.iSIMSUserType.AssessmentCoordinator;
            SeleniumHelper.Login(userType);
            //SeleniumHelper.NavigateMenu("Tasks", "Assessment", "Marksheets");
            CommonFunctions.GotToMarksheetMenu();
            WaitForAndClick(BrowserDefaults.TimeOut, By.LinkText(marksheet));
        }

        //Gets first column of marksheet
        public static IWebElement FindFirtColumn()
        {
            return WebContext.WebDriver.FindElement(By.CssSelector("[class=\"webix_first\"]"));
        }



        public static IWebElement FindColumnByColumnNumber(string columnNumber)
        {
            return BaseSeleniumComponents.WaitForAndGet(BrowserDefaults.TimeOut, By.CssSelector("[column='" + columnNumber + "']"));
        }


        public static IWebElement FindColumnByColumnName(string columnName)
        {
            List<IWebElement> allColumns = MarksheetGridHelper.FindAllColumns();
            IWebElement getColumn = allColumns.FirstOrDefault(col => col.Text.Contains(columnName));

            return getColumn;
        }
        public static List<IWebElement> FindCellsOfColumnByColumnName(string columnHeaderText)
        {
            List<IWebElement> allColumns = MarksheetGridHelper.FindAllColumns();
            IWebElement ageColumn = allColumns.FirstOrDefault(col => col.Text.Contains(columnHeaderText));
            string columnNo = ageColumn.GetAttribute("column");

            return MarksheetGridHelper.FindcellsForColumn(columnNo);
        }

        public static List<IWebElement> FindCellsOfColumnByColumnNamePOSExpectation(string columnHeaderText, string columnHeaderFooterText)
        {
            List<IWebElement> allColumns = MarksheetGridHelper.FindAllColumns();
            IWebElement ageColumn = allColumns.FirstOrDefault(col => col.Text.Contains(columnHeaderText));
            string columnNo = ageColumn.GetAttribute("column");

            return MarksheetGridHelper.FindcellsForColumn(columnNo);
        }

        public static String GetColumnName(string columnHeaderText)
        {
            List<IWebElement> allColumns = MarksheetGridHelper.FindAllColumns();
            IWebElement ageColumn = allColumns.FirstOrDefault(col => col.Text.Contains(columnHeaderText));
            String ColumnName = ageColumn.GetText();
            return ColumnName;
        }



        public static List<IWebElement> FindCellsOfColumnByColumnNameForPOS(string columnHeaderText)
        {
            string textTocompare = columnHeaderText.Replace(" ", string.Empty).ToLower();
            List<IWebElement> allColumns = MarksheetGridHelper.FindAllColumns();
            IWebElement ageColumn = allColumns.FirstOrDefault(col => col.Text.Replace(" ", string.Empty).ToLower().StartsWith(textTocompare)) ??
                                    allColumns.FirstOrDefault(col => col.Text.Replace(" ", string.Empty).ToLower().Contains(textTocompare));
            string columnNo = ageColumn.GetAttribute("column");

            return MarksheetGridHelper.FindcellsForColumn(columnNo);
        }

        //Gets last column of marksheet : fails if last column will not be visible
        public static IWebElement FindLastColumn()
        {
            return BaseSeleniumComponents.WaitForAndGet(BrowserDefaults.TimeOut, By.CssSelector("[class=\" webix_last\"]"));
        }

        public static IWebElement GetElementByCssSelector(string Cssselector)
        {
            return BaseSeleniumComponents.WaitForAndGet(BrowserDefaults.TimeOut, By.CssSelector(Cssselector));
        }

        //Gets all columns of marksheet
        public static List<IWebElement> FindAllColumns()
        {
            WaitUntilDisplayed(BrowserDefaults.TimeOut, By.CssSelector(CoulmnsdivSelector));
            IWebElement element = WebContext.WebDriver.FindElement(By.CssSelector(CoulmnsdivSelector));
            return element.FindChild(By.TagName("table")).FindElements(By.TagName("td")).ToList();
        }


        //Gets all columns of marksheet
        public static List<IWebElement> FindAllEditablecells()
        {
            var wait = new WebDriverWait(WebContext.WebDriver, BrowserDefaults.TimeOut);
            WaitLogic(wait, By.CssSelector(CellsDivSelector));
            return WebContext.WebDriver.FindElements(By.CssSelector(TextRightCellSelector)).ToList();
        }

        public static List<IWebElement> FindAllcells()
        {
            var wait = new WebDriverWait(WebContext.WebDriver, BrowserDefaults.TimeOut);
            WaitLogic(wait, By.CssSelector(CellsDivSelector));
            return WebContext.WebDriver.FindElements(By.CssSelector(EditableCellsSelector)).ToList();
        }
        public static List<IWebElement> FindcellsForColumn(string columnNo)
        {
            var wait = new WebDriverWait(WebContext.WebDriver, BrowserDefaults.TimeOut);
            WaitLogic(wait, By.CssSelector(CellsDivSelector));
            IWebElement cellDiv = WebContext.WebDriver.FindElement(By.CssSelector(CellsDivSelector));
            IWebElement parnetDiv = cellDiv.FindChild(By.CssSelector("[class='webix_ss_center_scroll']"));

            return
                parnetDiv.FindElement(By.CssSelector("[column='" + columnNo + "']"))
                    .FindElements(By.CssSelector(MarksheetGridHelper.CellSelector))
                    .ToList();
        }



        public static void ScrollToRight(string columnNo)
        {
            List<IWebElement> editableCells = MarksheetGridHelper.FindAllEditablecells();
            Actions action = new Actions(WebContext.WebDriver);
            if (editableCells.Any(cell => cell.GetAttribute("column") == columnNo))
            {
                editableCells.Find(cell => cell.GetAttribute("column") == columnNo).Click();
                action.SendKeys(Keys.Right).Perform();
            }
            else
            {
                string lastColNo = editableCells.LastOrDefault().GetAttribute("column");
                editableCells.LastOrDefault().Click();
                for (int i = Convert.ToInt16(lastColNo); i < Convert.ToInt16(columnNo); i++)
                {
                    action.SendKeys(Keys.Right).Perform();
                }
            }
        }

        //public static void OpenAdditionalColumn(string MarksheetName, string AdditionalColumnName, bool takeScreenPrint = false)
        //{
        //     MarksheetGridHelper.OpenMarksheet(MarksheetName);
        //     WebDriverWait waiter = new WebDriverWait(WebContext.WebDriver, TimeSpan.FromSeconds(MarksheetConstants.Timeout));
        //    WaitForAndClick(BrowserDefaults.TimeOut, By.CssSelector(MarksheetGridHelper.AdditionalColumnsButtonSelector));
        //    waiter.Until(ExpectedConditions.ElementExists(By.Id(MarksheetGridHelper.OkAdditionalColumsButtonId)));
        //    IWebElement checkbox = WebContext.WebDriver.FindElement(By.CssSelector(AdditionalColumnName));
        //    waiter.Until(ExpectedConditions.ElementIsVisible(By.CssSelector(AdditionalColumnName)));
        //    if (!checkbox.Selected)
        //    {
        //        checkbox.Click();
        //    }
        //    else
        //    {
        //        checkbox.Click();
        //        //WaitForAndClick(BrowserDefaults.TimeOut, By.Id(MarksheetGridHelper.OkAdditionalColumsButtonId));
        //        //Thread.Sleep(4000); //TODO: replace by the something like next line
        //        //waiter.Until(ExpectedConditions.InvisibilityOfElementLocated(By.CssSelector("[data-locking-mask class='locking-mask']")));                
        //        WaitForAndClick(BrowserDefaults.TimeOut, By.CssSelector(MarksheetGridHelper.AdditionalColumnsButtonSelector));
        //        WaitForAndClick(BrowserDefaults.TimeOut, By.CssSelector(AdditionalColumnName));
        //    }
        //    WaitForAndClick(BrowserDefaults.TimeOut, By.CssSelector("[data-automation-id='ok_button']"));

        //    if (takeScreenPrint)
        //        WebContext.Screenshot();            
        //}

        public static void OpenDistributionDetails(string columnHeader, string columnPeriod)
        {
            WaitForAndClick(BrowserDefaults.TimeOut, SeleniumHelper.SelectByDataAutomationID("header_menu_" + columnHeader + columnPeriod));
            WaitForAndClick(BrowserDefaults.TimeOut, By.CssSelector("[data-model-data-columnname=\"" + columnHeader + "\"]"));
        }
        public static void ExportDisriDistributionGraph()
        {
            WaitForAndClick(BrowserDefaults.TimeOut, By.ClassName("highcharts-button"));
        }
        public static void ExportDisriDistributionTable()
        {
            WaitForAndClick(BrowserDefaults.TimeOut, By.XPath("//div[contains(@class,'btn-group')]/button[contains(@class,'dropdown-toggle')]"));
            WaitForAndClick(BrowserDefaults.TimeOut, By.XPath("//div[contains(.,'To Excel (xlsx)')]"));
            Thread.Sleep(2000);
            WaitForAndClick(BrowserDefaults.TimeOut, By.XPath("//div[contains(.,'To CSV')]"));
        }
        public static void WaitFor(int seconds)
        {
            WebDriverWait waiter = new WebDriverWait(WebContext.WebDriver, TimeSpan.FromSeconds(seconds));
            waiter.Until(
                d =>
                {
                    IJavaScriptExecutor js = d as IJavaScriptExecutor;
                    string state = (string)js.ExecuteScript("return document.readyState");
                    if ("complete".Equals(state))
                    {
                        return true;
                    }
                    return false;
                });
        }
        public static IWebElement GetEditor()
        {
            Thread.Sleep(2000);    
            //return WebContext.WebDriver.FindElement(By.CssSelector(EditorSelector));
            return WebContext.WebDriver.FindElement(By.XPath(EditorSelector));
        }

        public static IWebElement GetTextAreEditor()
        {
            Thread.Sleep(2000);
             return WebContext.WebDriver.FindElement(By.XPath(EditorAreaSelector));
        }

        public static void ClickResultHistoryDropdown()
        {
            WaitForAndClick(BrowserDefaults.TimeOut, By.XPath(ResultHistorySelector));

        }

        public static List<IWebElement> GetPropertiesHeaderList(IWebElement parnetDiv)
        {
            List<IWebElement> PropertyColumnHeaderList =
                   parnetDiv
                       .FindElements(By.CssSelector(MarksheetGridHelper.CellSelector))
                       .ToList();

            return PropertyColumnHeaderList;
        }

        public static void ClickFirstCellofColumn(string columnposition)
        {

            WaitForAndClick(BrowserDefaults.TimeOut, By.XPath("//div[@column='" + columnposition + "']/div"));


        }


        public static IWebElement GetGridDetails(string name)
        {
            return WebContext.WebDriver.FindElement(By.Id(name));

        }

        public static ICollection<IWebElement> FindNumberofRows()
        {
            return WebContext.WebDriver.FindElements(By.XPath("//div[@column='1']/div"));
        }

        public static void EnterValueinLoop(int loop, string[] value)
        {
            for (int j = 0; j < loop; j++)
            {
                for (int i = 0; i < value.Length; i++)
                {
                    SeleniumHelper.Get(By.XPath(EditorSelector)).SendKeys(value[i]);
                    PerformEnterKeyBehavior();
                    Thread.Sleep(500);
                }
            }
        }

        public static void PerformEnterKeyBehavior()
        {
            Actions action = new Actions(WebContext.WebDriver);
            action.SendKeys(Keys.Enter).Perform();
        }

        public static void PerformTabKeyBehavior()
        {
            Actions action = new Actions(WebContext.WebDriver);
            action.SendKeys(Keys.Tab).Perform();
        }

        public static void UpArrowKeyBehavior()
        {
            Actions action = new Actions(WebContext.WebDriver);
            action.SendKeys(Keys.Up).Perform();
        }

        public static void DownArrowKeyBehavior()
        {
            Actions action = new Actions(WebContext.WebDriver);
            action.SendKeys(Keys.Down).Perform();
        }

        public static void LeftArrowKeyBehavior()
        {
            Actions action = new Actions(WebContext.WebDriver);
            action.SendKeys(Keys.Left).Perform();
        }

        public static void RightArrowKeyBehavior()
        {
            Actions action = new Actions(WebContext.WebDriver);
            action.SendKeys(Keys.Right).Perform();
        }

        public static void BackspaceKeyBehavior()
        {
            Actions action = new Actions(WebContext.WebDriver);
            action.SendKeys(Keys.Backspace).Perform();
        }


        public static void GradesetDetailsSelector(string gradesetName)
        {
            WebDriverWait waiter = new WebDriverWait(WebContext.WebDriver, TimeSpan.FromSeconds(MarksheetConstants.Timeout));
            IWebElement modalDailog = WebContext.WebDriver.FindElement(MarksheetConstants.ModalDailog);

            IWebElement gradesetSearch = modalDailog.FindElement(MarksheetConstants.SearchCriteriaButton);
            waiter.Until(ExpectedConditions.ElementToBeClickable(gradesetSearch));
            gradesetSearch.Click();
            ReadOnlyCollection<IWebElement> GradeSetNameResultList = modalDailog.FindElements(MarksheetConstants.SearchResultNameList);

            foreach (IWebElement eachelement in GradeSetNameResultList)
            {
                if (eachelement.Text.Contains(gradesetName))
                {
                    eachelement.Click();
                    break;
                }
            }
            waiter.Until(ExpectedConditions.ElementToBeClickable(modalDailog.FindElement(MarksheetConstants.AssessmentName)));
            WebContext.WebDriver.FindElements(MarksheetConstants.GenericOkButton)[1].Click();

        }

        public static string GenerateRandomString(int length)
        {
            Random _random = new Random((int)DateTime.Now.Ticks);
            string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuwxyz";
            StringBuilder builder = new StringBuilder(length);

            for (int i = 0; i < length; ++i)
                builder.Append(chars[_random.Next(chars.Length)]);

            return builder.ToString();
        }
    }


}
