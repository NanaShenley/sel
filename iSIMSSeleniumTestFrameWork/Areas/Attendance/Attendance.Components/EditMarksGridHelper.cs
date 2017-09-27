using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.PageObjects;
using OpenQA.Selenium.Support.UI;
using SharedComponents.BaseFolder;
using SharedComponents.Helpers;
using SharedComponents.HomePages;
using TestSettings;
using WebDriverRunner.webdriver;
using System.Threading;
using SharedComponents.LoginPages;
using Attendance.Components.Common;
using iSIMSSeleniumHelper = SharedComponents.SeleniumHelperObsolete;

namespace Attendance.Components
{
    public class EditMarksGridHelper : BaseSeleniumComponents
    {
        //selectors
        public const string GridMenueItemCaretSelector = "[class=\"fa fa-angle-down fa-fw\"]";
        public const string GridMenueItemSortSelector = "#editmarksgrid";
        public const string DistributionGraphPopoverSelector = "[data-show-distribution-chart ='']";
        public const string CoulmnsdivSelector = "[class=\"webix_hs_center\"]";
        public const string CellsDivSelector = "[class=\"webix_ss_center\"]";
        public const string EditableCellsSelector = "[class='webix_column ']";
        public const string TextRightCellSelector = "[class='webix_column text-right']";
        public const string CellSelector = "[class=\"webix_cell\"]";
        public const string HorizontalScrollBarSelector = "[class='webix_ss_hscroll webix_vscroll_x']";
        public const string AdditionalColumnsButtonSelector = "[data-show-additional-columns-modal='']";
        public const string OkAdditionalColumsButtonId = "savePreferences";
        public const string EditorSelector = "//div[contains(@class,'webix_dt_editor')]/input[@type='text']";

        public const string Summary = "[data-automation-id='summary_button']";
        
        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='task_menu_section_attendance_EditMarks']")]
        private IWebElement _editmarksLink;
        [FindsBy(How = How.CssSelector, Using = "a[title='Automatically advance down the register']")]
        public IWebElement orientationButton;
        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='Horizontal']")]
        public IWebElement horizontalMode;
        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='Vertical']")]
        public IWebElement verticalMode;
        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='Extended_Dropdown']")]
        public IWebElement preserveButton;
        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='Keep']")]
        public IWebElement preserveMode;
        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='Replace']")]
        public IWebElement overwriteMode;

        
        public EditMarksGridHelper()
        {
            PageFactory.InitElements(WebContext.WebDriver, this);
        }
        public EditMarksGridHelper NavigatetoMEditMarks()
        {
            WaitForElement(By.CssSelector("[data-automation-id='task_menu_section_attendance_EditMarks']"));
            _editmarksLink.Click();
            //WaitForElement(By.LinkText("Analysis English Year on Year - Year 4"));
            return this;
        }

        //Gets first column of EditMarks
        public static IWebElement FindFirstColumn()
        {
            return WebContext.WebDriver.FindElement(By.CssSelector("[class=\"webix_first\"]"));
        }

        public static IWebElement FindColumnByColumnNumber(string columnNumber)
        {
            return BaseSeleniumComponents.WaitForAndGet(BrowserDefaults.TimeOut, By.CssSelector("[column='" + columnNumber + "']"));
        }

        public static List<IWebElement> FindCellsOfColumnByColumnName(string columnHeaderText)
        {
            List<IWebElement> allColumns = EditMarksGridHelper.FindAllColumns();
            IWebElement ageColumn = allColumns.FirstOrDefault(col => col.Text.Contains(columnHeaderText));
            string columnNo = ageColumn.GetAttribute("column");

            return EditMarksGridHelper.FindcellsForColumn(columnNo);
        }

        //Gets last column of EditMarks : fails if last column will not be visible
        public static IWebElement FindLastColumn()
        {
            return BaseSeleniumComponents.WaitForAndGet(BrowserDefaults.TimeOut, By.CssSelector("[class=\" webix_last\"]"));
        }

        //Gets all columns of EditMarks
        public static List<IWebElement> FindAllColumns()
        {
            var wait = new WebDriverWait(WebContext.WebDriver, BrowserDefaults.TimeOut);
            WaitLogic(wait, By.CssSelector(CoulmnsdivSelector));
            IWebElement element = WebContext.WebDriver.FindElement(By.CssSelector(CoulmnsdivSelector));
            return element.FindChild(By.TagName("table")).FindElements(By.TagName("td")).ToList();
        }


        //Gets all columns of EditMarks
        public static List<IWebElement> FindAllEditablecells()
        {
            var wait = new WebDriverWait(WebContext.WebDriver, BrowserDefaults.TimeOut);
            WaitLogic(wait, By.CssSelector(CellsDivSelector));
            return WebContext.WebDriver.FindElements(By.CssSelector(TextRightCellSelector)).ToList();

            //  return element.FindElements(By.CssSelector(EditableCellsSelector)).ToList();
        }

        public static List<IWebElement> FindAllcells()
        {
            var wait = new WebDriverWait(WebContext.WebDriver, BrowserDefaults.TimeOut);
            WaitLogic(wait, By.CssSelector(CellsDivSelector));
            return WebContext.WebDriver.FindElements(By.CssSelector(EditableCellsSelector)).ToList();

            //  return element.FindElements(By.CssSelector(EditableCellsSelector)).ToList();
        }
        public static List<IWebElement> FindcellsForColumn(string columnNo)
        {
            var wait = new WebDriverWait(WebContext.WebDriver, BrowserDefaults.TimeOut);
            WaitLogic(wait, By.CssSelector(CellsDivSelector));
            IWebElement cellDiv = WebContext.WebDriver.FindElement(By.CssSelector(CellsDivSelector));
            IWebElement parnetDiv = cellDiv.FindChild(By.CssSelector("[class='webix_ss_center_scroll']"));

            return
                parnetDiv.FindElement(By.CssSelector("[column='" + columnNo + "']"))
                    .FindElements(By.CssSelector(EditMarksGridHelper.CellSelector))
                    .ToList();
        }


        public static void ScrollToRight(string columnNo)
        {
            List<IWebElement> editableCells = EditMarksGridHelper.FindAllEditablecells();
            Actions action = new Actions(WebContext.WebDriver);
            if (editableCells.Any(cell => cell.GetAttribute("column") == columnNo))
            {
                editableCells.Find(cell => cell.GetAttribute("column") == columnNo).Click();
                action.SendKeys(Keys.Right);
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

        public static void OpenAdditionalColumn(string EditMarks, string AdditionalColumnName, bool takeScreenPrint = false)
        {
            //EditMarksGridHelper.OpenEditMarks(EditMarks);
            WebDriverWait waiter = new WebDriverWait(WebContext.WebDriver, TimeSpan.FromSeconds(AttendanceTestGroups.Timeout));
            WaitForAndClick(BrowserDefaults.TimeOut, By.CssSelector(EditMarksGridHelper.AdditionalColumnsButtonSelector));
            waiter.Until(ExpectedConditions.ElementExists(By.Id(EditMarksGridHelper.OkAdditionalColumsButtonId)));
            IWebElement checkbox = WebContext.WebDriver.FindElement(By.CssSelector(AdditionalColumnName));
            waiter.Until(ExpectedConditions.ElementIsVisible(By.CssSelector(AdditionalColumnName)));
            if (!checkbox.Selected)
            {
                checkbox.Click();
            }
            else
            {
                checkbox.Click();
                WaitForAndClick(BrowserDefaults.TimeOut, By.Id(EditMarksGridHelper.OkAdditionalColumsButtonId));
                Thread.Sleep(4000); //TODO: replace by the something like next line
                //waiter.Until(ExpectedConditions.InvisibilityOfElementLocated(By.CssSelector("[data-locking-mask class='locking-mask']")));                
                WaitForAndClick(BrowserDefaults.TimeOut, By.CssSelector(EditMarksGridHelper.AdditionalColumnsButtonSelector));
                WaitForAndClick(BrowserDefaults.TimeOut, By.CssSelector(AdditionalColumnName));
            }
            WaitForAndClick(BrowserDefaults.TimeOut, By.Id(EditMarksGridHelper.OkAdditionalColumsButtonId));
            Thread.Sleep(5000);
            if (takeScreenPrint)
                WebContext.Screenshot();
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
            return WebContext.WebDriver.FindElement(By.XPath(EditorSelector));
        }

        public static void ClickFirstCellofColumn(string columnposition)
        {

            WaitForAndClick(BrowserDefaults.TimeOut, By.XPath("//div[@column='" + columnposition + "']/div"));


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

                    GetEditor().SendKeys(value[i]);
                    PerformEnterKeyBehavior();
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

        public static void Save()
        {
            iSIMSSeleniumHelper.FindAndClick(iSIMSSeleniumHelper.AutomationId("save_button"));

        }

        public void ClickOrientationbutton(IWebElement element)
        {
            //ElementAccessor.ClickByAction( element)
            Actions action = new Actions(WebContext.WebDriver);
            action.Click(element).Build().Perform();
        }


    }


}
