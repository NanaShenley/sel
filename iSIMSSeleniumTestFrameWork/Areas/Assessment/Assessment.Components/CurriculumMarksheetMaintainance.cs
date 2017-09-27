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
    public class CurriculumMarksheetMaintainance : BaseSeleniumComponents
    {
        public CurriculumMarksheetMaintainance()
        {
            WebContext.WebDriver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(MarksheetConstants.Timeout));
            PageFactory.InitElements(WebContext.WebDriver, this);

        }

        WebDriverWait waiter = new WebDriverWait(WebContext.WebDriver, TimeSpan.FromSeconds(MarksheetConstants.Timeout));

        [FindsBy(How = How.CssSelector, Using = "button[data-automation-id='pos']")]
        private IWebElement CurricullumButton;

        [FindsBy(How = How.CssSelector, Using = "div[id='pickerLayout']")]
        public IWebElement TreeRootElement;

        public string checkboxTag = "#curriculumInfoTree input[type='checkbox']";

        public string topiccheckboxTag = "#topicInfoTree input[type='checkbox']";

        [FindsBy(How = How.CssSelector, Using = "button[data-automation-id='curriculum_columns_done']")]
        private IWebElement AddSelectedColumnsbutton;

        [FindsBy(How = How.CssSelector, Using = "button[data-automation-id='savetemplate']")]
        private IWebElement SaveTemplate;

        //[FindsBy(How = How.CssSelector, Using = "i[class='fa fa-plus-square-o btn-rowexpand-icon']")]
        //private IWebElement TogglePlusbutton;

        private static By TogglePlusbutton = By.CssSelector("i[class='fa fa-plus-square-o btn-rowexpand-icon']");

        //    [FindsBy(How = How.CssSelector, Using = "i[class='fa fa-minus-square-o btn-rowexpand-icon']")]
        //  private IWebElement ToggleMinusbutton;
        private static By ToggleMinusbutton = By.CssSelector("i[class='fa fa-minus-square-o btn-rowexpand-icon']");

        //[FindsBy(How = How.CssSelector, Using = "i[class='fa fa-pencil']")]
        //private IWebElement EditButton;
        private static By EditButton = By.CssSelector("i[class='fa fa-pencil']");

        [FindsBy(How = How.CssSelector, Using = "div[class='wizard-controls-prev'] .btn-default")]
        private IWebElement CancelButton;

        [FindsBy(How = How.CssSelector, Using = "button[data-automation-id='remove_button2'] i[class='fa fa-trash-o']")]
        private IWebElement DeleteButton;


        [FindsBy(How = How.CssSelector, Using = "div[class='popover bottom in'] button[data-automation-id='Yes_button']")]
        private IWebElement DeleteYesButton;

        [FindsBy(How = How.CssSelector, Using = "a[data-cancel-button]")]
        private IWebElement CancelTemplateButton;

        [FindsBy(How = How.CssSelector, Using = "a[data-automation-id='Button_Dropdown']")]
        private IWebElement CreateMarksheetButton;

        //public static By ModifyMarksheet = By.CssSelector("a[data-automation-id='marksheet_with_level_modify']");

        [FindsBy(How = How.CssSelector, Using = "a[data-automation-id='marksheet_with_level_modify']")]
        private IWebElement modifyMarksheet;

        [FindsBy(How = How.CssSelector, Using = "a[data-automation-id='marksheet_with_level_new']")]
        private IWebElement newMarksheet;

        [FindsBy(How = How.CssSelector, Using = "div[id='marksheetcreation-wrapper'] input[name='MarksheetTemplateName']")]
        private IWebElement templateNameText;

        [FindsBy(How = How.CssSelector, Using = "div[id='marksheetcreation-wrapper'] input[name='Description']")]
        private IWebElement templateDescriptionText;

        //[FindsBy(How = How.CssSelector, Using = "#curriculumInfoTree input[type='checkbox'][checked]")]
        //private IWebElement SelectedCheckboxes;

        private static By SelectedRow = By.CssSelector("#curriculumStatmentsDesc div.statement-group-item");

        private static By SelectedStatementRow = By.CssSelector("#curriculumStatmentsDesc div.statement-group-item");

        private static By SelectedCheckboxes = By.CssSelector("#curriculumInfoTree input[type='checkbox'][checked]");

        private static By CurricullumButtonTooltip = By.CssSelector("button[data-automation-id='pos'] strong[class='tooltip-title']");

        [FindsBy(How = How.CssSelector, Using = "a[data-automation-id='marksheet_with_level_copy']")]
        private IWebElement copyMarksheet;

        [FindsBy(How = How.CssSelector, Using = "div[id='marksheetcreation-wrapper'] textarea[name='Description']")]
        public IWebElement MarksheetTemplateDescription;

        [FindsBy(How = How.CssSelector, Using = "input[name='filter']")]
        public IWebElement schemeFilter;

        [FindsBy(How = How.CssSelector, Using = "input[data-automation-id='Topic']")]
        public IWebElement Option;

        //private static By Selected = By.CssSelector("div[class='webix_tree_item']");


        public CurriculumMarksheetMaintainance FillTemplateDetails(string input)
        {
            Thread.Sleep(2000);
            schemeFilter.WaitUntilState(ElementState.Displayed);
            schemeFilter.Clear();
            schemeFilter.SendKeys(input);
            return this;
        }

        //public const string EditorSelector = "//div[contains(@class,'webix_dt_editor')]/input[@type='text']";
        public CurriculumMarksheetMaintainance SelectCurriculum()
        {
            waiter.Until(ExpectedConditions.ElementToBeClickable(CurricullumButton));
            CurricullumButton.Click();
            return new CurriculumMarksheetMaintainance();
        }
        public CurriculumMarksheetMaintainance SelectTopic()
        {
            waiter.Until(ExpectedConditions.ElementToBeClickable(Option));
            Option.Click();
            return new CurriculumMarksheetMaintainance();
        }
        //public const string EditorSelector = "//div[contains(@class,'webix_dt_editor')]/input[@type='text']";
        public String GetCurriculumButtonTooltip()
        {
            waiter.Until(ExpectedConditions.ElementToBeClickable(CurricullumButton));
            String CurriculumTooltip = WebContext.WebDriver.FindElement(CurricullumButtonTooltip).GetValue();

            return CurriculumTooltip;
        }

        public void SelectStatement(int checkedboxIndexNo)
        {
            waiter.Until(ExpectedConditions.ElementToBeClickable(TreeRootElement));
            WaitForElement(By.CssSelector(checkboxTag));
            ReadOnlyCollection<IWebElement> treenodElement = TreeRootElement.FindElements((By.CssSelector(checkboxTag)));
            int[] checkedboxIndex = { checkedboxIndexNo };
            foreach (var index in checkedboxIndex)
            {
                var element = treenodElement[index];
                if (element != null)
                {
                    element.Click();
                }
                Thread.Sleep(500);
            }

        }
        public void SelectTopicStatement(int checkedboxIndexNo)
        {
            waiter.Until(ExpectedConditions.ElementToBeClickable(TreeRootElement));
            WaitForElement(By.CssSelector(topiccheckboxTag));
            ReadOnlyCollection<IWebElement> treenodElement = TreeRootElement.FindElements((By.CssSelector(topiccheckboxTag)));
            int[] checkedboxIndex = { checkedboxIndexNo };
            foreach (var index in checkedboxIndex)
            {
                var element = treenodElement[index];
                if (element != null)
                {
                    element.Click();
                }
                Thread.Sleep(500);
            }

        }
        public void UnSelectStatement(int checkedboxIndexNo)
        {
            waiter.Until(ExpectedConditions.ElementToBeClickable(TreeRootElement));
            ReadOnlyCollection<IWebElement> treenodElement = TreeRootElement.FindElements((By.CssSelector(checkboxTag)));
            int[] checkedboxIndex = { checkedboxIndexNo };
            foreach (var index in checkedboxIndex)
            {
                var element = treenodElement[index];
                if (element != null)
                {
                    if (element.Selected)
                    {
                        element.Click();
                    }

                }
                Thread.Sleep(500);
            }

        }

        public CurriculumMarksheetMaintainance EditTemplateDescription(string Description)
        {
            MarksheetTemplateDescription.WaitUntilState(ElementState.Displayed);
            MarksheetTemplateDescription.Clear();
            MarksheetTemplateDescription.SendKeys(Description);
            return this;
        }

        public EditMarksheetTemplate ClickCopyFromExistingButton()
        {
            waiter.Until(ExpectedConditions.ElementToBeClickable(copyMarksheet));
            copyMarksheet.Click();
            return new EditMarksheetTemplate();
        }

        public bool IsCreateMarksheetTemplateVisible()
        {
            CreateMarksheetButton.WaitUntilState(ElementState.Displayed);
            return CreateMarksheetButton.Displayed;
        }

        /// <summary>
        /// It sets the given Aspect name in the Name text field
        /// </summary>
        public void SetTemplateName(string TemplateName)
        {
            templateNameText.Clear();
            templateNameText.SendKeys(TemplateName);
        }

        public int GetSelectedStatementCount()
        {
            return (WebContext.WebDriver.FindElements(SelectedStatementRow)).Count();
        }

        /// <summary>
        /// It sets the given Aspect name in the Name text field
        /// </summary>
        public void SetTemplateDescription(string Description)
        {
            templateDescriptionText.Clear();
            templateDescriptionText.SendKeys(Description);
        }

        public void CheckStatement(int index)
        {
            waiter.Until(ExpectedConditions.ElementToBeClickable(TreeRootElement));
            ReadOnlyCollection<IWebElement> treenodElement = TreeRootElement.FindElements((By.CssSelector(checkboxTag)));
            var element = treenodElement[index];
            if (element != null)
            {
                element.Click();
            }
            Thread.Sleep(500);
        }



        public CurriculumMarksheetMaintainance Save()
        {
            waiter.Until(ExpectedConditions.ElementToBeClickable(SaveTemplate));
            SaveTemplate.Click();
            return new CurriculumMarksheetMaintainance();
        }

        public CurriculumMarksheetMaintainance CreateMarksheet()
        {
            waiter.Until(ExpectedConditions.ElementToBeClickable(CreateMarksheetButton));
            CreateMarksheetButton.Click();
            return new CurriculumMarksheetMaintainance();
        }

        public CurriculumMarksheetMaintainance CancelTemplate()
        {
            waiter.Until(ExpectedConditions.ElementToBeClickable(CancelTemplateButton));
            CancelTemplateButton.Click();
            return new CurriculumMarksheetMaintainance();
        }



        public string GenerateRandomString(int length)
        {
            Random _random = new Random((int)DateTime.Now.Ticks);
            string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuwxyz";
            StringBuilder builder = new StringBuilder(length);

            for (int i = 0; i < length; ++i)
                builder.Append(chars[_random.Next(chars.Length)]);

            return builder.ToString();
        }

        public CurriculumMarksheetMaintainance AddSelectedColumns()
        {
            AddSelectedColumnsbutton.Click();
            waiter.Until(ExpectedConditions.ElementToBeClickable(EditButton));
            return new CurriculumMarksheetMaintainance();
        }

        public CurriculumMarksheetMaintainance Expand()
        {
            WaitForAndClick(TimeSpan.FromSeconds(MarksheetConstants.Timeout), TogglePlusbutton);
            return new CurriculumMarksheetMaintainance();
        }

        public CurriculumMarksheetMaintainance Collapse()
        {
            WaitForAndClick(TimeSpan.FromSeconds(MarksheetConstants.Timeout), ToggleMinusbutton);
            return new CurriculumMarksheetMaintainance();
        }


        /// <summary>
        /// clicks to open pop up for adding grades.
        /// </summary>
        /// <returns></returns>
        public EditMarksheetTemplate ClickModifyExistingButton()
        {
            modifyMarksheet.Click();
            return new EditMarksheetTemplate();
        }

        /// <summary>
        /// clicks to open pop up for adding grades.
        /// </summary>
        /// <returns></returns>
        public CurriculumMarksheetMaintainance ClickNewMarksheetButton()
        {
            newMarksheet.Click();
            return new CurriculumMarksheetMaintainance();
        }

        public CurriculumMarksheetMaintainance Edit()
        {
            Thread.Sleep(3000);
            WaitForAndClick(TimeSpan.FromSeconds(MarksheetConstants.Timeout), EditButton);
            return new CurriculumMarksheetMaintainance();
        }

        public CurriculumMarksheetMaintainance Cancel()
        {
            CancelButton.Click();
            return new CurriculumMarksheetMaintainance();
        }

        public CurriculumMarksheetMaintainance Delete()
        {
            waiter.Until(ExpectedConditions.ElementToBeClickable(DeleteButton));
            DeleteButton.Click();
            waiter.Until(ExpectedConditions.ElementToBeClickable(DeleteYesButton));
            DeleteYesButton.Click();
            return new CurriculumMarksheetMaintainance();
        }

        public int GetSelectedCount()
        {
            return (WebContext.WebDriver.FindElements(SelectedRow)).Count();
        }


        public int GetCheckedCount()
        {
            return (WebContext.WebDriver.FindElements(SelectedCheckboxes)).Count();
        }
    }
}
