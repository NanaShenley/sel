using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assessment.Components.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using SharedComponents.BaseFolder;
using OpenQA.Selenium.Support.PageObjects;
using OpenQA.Selenium.Support.UI;
using SharedComponents.Helpers;
using WebDriverRunner.webdriver;
using System.Threading;
using OpenQA.Selenium.Interactions;
namespace Assessment.Components.PageObject
{
    public class MarksheetTemplateProperties
    {
        public MarksheetTemplateProperties()
        {
            WebContext.WebDriver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(MarksheetConstants.Timeout));
            PageFactory.InitElements(WebContext.WebDriver, this);
        }


        [FindsBy(How = How.CssSelector, Using = "a[id='opendetails']")]
        private readonly IWebElement DetailsTab = null;

        [FindsBy(How = How.CssSelector, Using = "div[data-automation-id='ColumnDetailsPopover']")]
        private readonly IWebElement AssignGradePopover = null;

        [FindsBy(How = How.CssSelector, Using = "button[onclick='sims_popover.HidePopups();']")]
        private readonly IWebElement GradesetPopoverClose = null;

        [FindsBy(How = How.CssSelector, Using = "i[data-automation-id='header_menu_Grade set']")]
        private readonly IWebElement BulkAllocateDropdownButton = null;

        [FindsBy(How = How.CssSelector, Using = "div[data-grid-id='gridProperties']")]
        private readonly IWebElement BulkAllocateDropdownHeader = null;
        [FindsBy(How = How.CssSelector, Using = "button[data-automation-id='clone_row']")]
        private readonly IWebElement CloneRowInMarksheetProperties = null;

        public static By Applygradeset = By.CssSelector("[data-apply-bulkupdate]");

        [FindsBy(How = How.CssSelector, Using = "div[column='7']")]
        private readonly IWebElement LevelColumn = null;

        public static By ColumnCell = By.CssSelector("div[class*=\"webix_cell\"]");

        private static readonly By GetMarksheetTemplateRows = By.CssSelector("div[class='webix_cell  topItem'] span[class='marksheet-properties-column-header']");

        WebDriverWait waiter = new WebDriverWait(WebContext.WebDriver, TimeSpan.FromSeconds(MarksheetConstants.Timeout));

        private static By AssignGradesetLink = By.CssSelector("span[title='Assign a gradeset']");

        /// <summary>
        /// Returns a New Page Object for Marksheet Template Properties
        /// </summary>
        public MarksheetTemplateProperties MarksheetTemplatePropertiesPageObject()
        {
            return new MarksheetTemplateProperties();
        }

        /// <summary>
        /// Assigns a Grade Set to a unique subject with single column
        /// </summary>
        public GradesetSearchPanel ClickAssignGradeSet(string SubjectName)
        {
            AddSubjects addsubject = new AddSubjects();
            //Getting the Subject ID
            List<Guid> SubjectID = new List<Guid>();
            SubjectID = TestData.CreateGuidList("Select ID From AssessmentSubject Where Name like '%" + SubjectName + "%'", "ID");
            WebContext.WebDriver.FindElement(By.CssSelector("button[data-ajax-url*='" + SubjectID[0] + "']")).Click();
            waiter.Until(ExpectedConditions.TextToBePresentInElementLocated(By.CssSelector("h4[data-automation-id='select_gradeset_popup_header_title']"), "Select Gradeset"));
            return new GradesetSearchPanel();
        }


        /// <summary>
        /// Open Details Tab
        /// </summary>
        public MarksheetTemplateDetails OpenDetailsTab()
        {
            Thread.Sleep(2000);
            DetailsTab.Click();
            while (true)
            {
                if (DetailsTab.GetAttribute("aria-expanded") == "true")
                    break;
            }
            return new MarksheetTemplateDetails();
        }


        /// <summary>
        /// check assign gradeset popover is present
        /// </summary>
        public MarksheetTemplateDetails CheckAssignGradepopover()
        {
            waiter.Until(ExpectedConditions.TextToBePresentInElement(AssignGradePopover, "Assign Column Types"));
            Assert.AreEqual(WebContext.WebDriver.FindElement(By.CssSelector("div[id='gradesetPopover']")).Text, "Assign Column Types");
            return new MarksheetTemplateDetails();
        }

        /// <summary>
        /// Close assign gradeset popover is present
        /// </summary>
        public MarksheetTemplateDetails CloseAssignGradepopover()
        {
            waiter.Until(ExpectedConditions.ElementToBeClickable(GradesetPopoverClose));
            GradesetPopoverClose.Click();
            return new MarksheetTemplateDetails();
        }

        /// <summary>
        /// Open Bulk Gradeset Allocation task bar
        /// </summary>
        public BulkAssignGradeset OpenBulkGradesetAllocationMenu()
        {
            BulkAllocateDropdownButton.Click();
            waiter.Until(ExpectedConditions.TextToBePresentInElement(BulkAllocateDropdownHeader, "Bulk Assign Gradeset"));
            return new BulkAssignGradeset();
        }


        /// <summary>
        /// Get All the Gradeset Names displayed on the Marksheet Properties page
        /// </summary>
        public List<string> GetAllGradesetNames()
        {
            List<string> AssessmentGradesetNameList = new List<string>();
            ReadOnlyCollection<IWebElement> GradesetElementList = WebContext.WebDriver.FindElements(AssignGradesetLink);
            foreach (IWebElement eachelement in GradesetElementList)
            {
                if (eachelement.Text != "")
                {
                    AssessmentGradesetNameList.Add(eachelement.Text);
                }
            }
            return AssessmentGradesetNameList;
        }


        /// <summary>
        /// Close assign gradeset popover is present
        /// </summary>
        public MarksheetTemplateProperties ClickCloneRowMarksheetProperties()
        {
            waiter.Until(ExpectedConditions.ElementToBeClickable(CloneRowInMarksheetProperties));
            CloneRowInMarksheetProperties.Click();
            return new MarksheetTemplateProperties();
        }

        /// <summary>
        /// Close assign gradeset popover is present
        /// </summary>
        public List<String> GetMarksheetTemplateRowsList()
        {
            //   waiter.Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(WebContext.WebDriver.FindElements(GetMarksheetTemplateRows)));
            List<String> MarksheetRowsList = new List<string>();
            ReadOnlyCollection<IWebElement> ColumnRows = WebContext.WebDriver.FindElements(GetMarksheetTemplateRows);
            foreach (IWebElement rowItems in ColumnRows)
            {
                if (rowItems != null)
                    MarksheetRowsList.Add(rowItems.Text);
                Console.WriteLine(rowItems.Text);
            }
            return MarksheetRowsList;
        }

        /// <summary>
        /// Select Multiple Rows on the Marksheet Template Properties Tab
        /// </summary>
        public MarksheetTemplateProperties SelectMultipleRows(int rowno)
        {
            ReadOnlyCollection<IWebElement> templateRowList = LevelColumn.FindElements(ColumnCell);
            Actions action = new Actions(WebContext.WebDriver);
            if (templateRowList[rowno].Selected != true)
            {
                action.KeyDown(Keys.LeftControl)
                      .MoveToElement(templateRowList[rowno])
                      .Click()
                      .KeyUp(Keys.LeftControl)
                      .Build()
                      .Perform();
            }
            return new MarksheetTemplateProperties();
        }

        public void SelectGridRows()
        {
            ReadOnlyCollection<IWebElement> gradesetColumnCell = LevelColumn.FindElements(ColumnCell);
            foreach (IWebElement eachvalue in gradesetColumnCell)
            {
                if (eachvalue.Text != "")
                {
                    eachvalue.Click();
                    break;
                }
            }
        }
    }
}
