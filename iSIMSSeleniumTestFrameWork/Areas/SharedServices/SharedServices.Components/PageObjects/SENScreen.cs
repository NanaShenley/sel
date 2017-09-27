using System.Collections.ObjectModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium.Support.PageObjects;
using SharedComponents.BaseFolder;
using OpenQA.Selenium;
using WebDriverRunner.webdriver;
using SharedComponents.Helpers;
using System.Threading;
using OpenQA.Selenium.Support.UI;
using System;

namespace SharedServices.Components.PageObjects
{
    public class SENScreen : BaseSeleniumComponents
    {
        private const string HiddenLink = "section_menu_more";
        private const string SenSectionAccordion = "section_menu_hidden_Statutory";       
        public const string NoSenStageSelect = "[id='tri_chkbox_SENNeverExistCriterion']";
        public const string SenNeedGrid = "table[data-maintenance-container='LearnerSENNeedTypes']";                
        private const string DateCss = "input[name*= '.StartDate']";
        private const string CloseFirstTabId = "tab_Pupil";        
        private const string DateCol = "input[name$='StartDate']";
        public const string NeedTypeSelector = "input[name$='NeedType.dropdownImitator']";
        public const string chkbox = "[id='tri_chkbox_SENNeverExistCriterion']";
        private const string SenNeed = "[data-maintenance-container='LearnerSENNeedTypes']";
        private const string documentslink = "documents_(-)_button";
        public string rowid;

        WebDriverWait waiter = new WebDriverWait(WebContext.WebDriver, TimeSpan.FromSeconds(30));

        public SENScreen()
        {
            WebContext.WebDriver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(1000));

            PageFactory.InitElements(WebContext.WebDriver, this);
        }

        private IWebElement _searchButton
        {
            get { return SeleniumHelper.Get(SeleniumHelper.SelectByDataAutomationID("search_criteria_submit")); }
        }

        public void ClickSearch()
        {
            _searchButton.Click();
        }
      
        private IWebElement hiddenLink
        {
            get { return SeleniumHelper.Get(SeleniumHelper.SelectByDataAutomationID(HiddenLink)); }
        }

        private IWebElement senAccordion
        {
            get { return SeleniumHelper.Get(SeleniumHelper.SelectByDataAutomationID(SenSectionAccordion)); }
        }

        private IWebElement closeFirstTab
        {
            get { return SeleniumHelper.Get(SeleniumHelper.SelectByDataAutomationID(CloseFirstTabId)); }
        }       

        public void CloseFirstTab()
        {
            closeFirstTab.Click();
        }     

        /// <summary>
        /// Click to view hidden links
        /// </summary>
        public void ClickHiddenTabs()
        {
            hiddenLink.Click();
        }

        public void ClickSenAccordion()
        {
            senAccordion.Click();
        }

        //[FindsBy(How = How.CssSelector, Using = SenNeedGrid)]
        //public IWebElement senNeedGrids;

        private IWebElement senNeedGrids
        {

            get
            {

                return SeleniumHelper.Get(By.CssSelector(SenNeed));
            }
        }

        //[FindsBy(How = How.CssSelector, Using = DateCss)]
        //private IWebElement dateElement;

        public IWebElement GetGridRow(int row, NoteType type)
        {
            Thread.Sleep(2000);
            IWebElement gridRow = null;
            switch (type)
            {
                case NoteType.SenNeed:
                    gridRow = senNeedGrids.GetGridRow(row);
                    break;                   
            }
            return gridRow;
        }

        public void SetCheckboxForCriteria(string checkBox)
        {
           
            IWebElement checkbox = WebContext.WebDriver.FindElement(By.CssSelector(chkbox));
            if (!checkbox.Selected)
            {
                checkbox.Click();
            }            
        }
      
        /// <summary>
        /// Method to find cell in a grid
        /// </summary>
        public IWebElement FindCell(int rowNum, int cellNum, NoteType type)
        {
            
            IWebElement row = GetGridRow(rowNum, type);
            ReadOnlyCollection<IWebElement> cells = row.FindElements(By.TagName("td"));            
            return cells[cellNum];
        }

        public void AddSelectorColumn(int rowno, int cellno, NoteType type, string needType, string selector)
        {
            IWebElement selectorColumn = FindCell(rowno, cellno, NoteType.SenNeed);
            Assert.IsNotNull(selectorColumn);
            selectorColumn.ChooseSelectorOption(By.CssSelector(selector),
               needType);                       
        }

        public void AddDateColumn(int rowno, int cellno, NoteType type, string date)
        {           
            IWebElement dateColumn = FindCell(rowno, cellno, NoteType.SenNeed);   
            Assert.IsNotNull(dateColumn);
            
            dateColumn.SetDateTime(By.CssSelector(DateCol), date);        
        }
        public void ClickDocumentsButton(int rowNumber, NoteType type)
        {
            IWebElement gridRow = GetGridRow(rowNumber, type);
            if (gridRow != null)
            {
                IWebElement documentsButton = gridRow.FindElement(SeleniumHelper.SelectByDataAutomationID(documentslink));
                Assert.IsNotNull(documentsButton);
                documentsButton.Click();
            }
        }
        public void ClickYesButton()
        {
            DeleteConfirmationDialog deletedialog = new DeleteConfirmationDialog();
            deletedialog.ClickYesButton();
        }

        
        public void ClickDeleteRowButton(int rowNumber, NoteType type)
        {
            IWebElement gridRow = GetGridRow(rowNumber, type);
            if (gridRow != null)
            {
                IWebElement DeleteRowButton = gridRow.FindElement(SeleniumHelper.SelectByDataAutomationID("remove_button"));

                if (DeleteRowButton.Enabled == true)
                {
                    Thread.Sleep(1000);
                    DeleteRowButton.Click();
                    ClickYesButton();
                }
            }
        }
    }
}


