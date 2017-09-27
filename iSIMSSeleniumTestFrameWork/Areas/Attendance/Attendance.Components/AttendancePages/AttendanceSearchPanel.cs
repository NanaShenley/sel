using System.Collections.Generic;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using TestSettings;
using WebDriverRunner.webdriver;
using SharedComponents.BaseFolder;
using POM.Helper;
using System.Threading;
using Attendance.Components.Common;
using OpenQA.Selenium.Support.PageObjects;

namespace Attendance.Components.AttendancePages
{
    public class AttendanceSearchPanel : BaseSeleniumComponents
    {
#pragma warning disable 0649
        private readonly By _criteria = By.CssSelector("[data-automation-id='search_criteria']");
        private readonly List<Tree> _trees = new List<Tree>();

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='search_criteria']")]
        public IWebElement searchPanel;
        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='search_criteria_submit']")]
        public IWebElement searchButton;
        [FindsBy(How = How.Name, Using = "LegalSurname")]
        public IWebElement pupilNameFilter;
        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='section_menu_Class']")]
        public IWebElement classFilter;
        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='section_menu_Year Group']")]
        public IWebElement yeargroupFilter;


        public AttendanceSearchPanel()
        {
            PageFactory.InitElements(WebContext.WebDriver, this);
            var wait = new WebDriverWait(WebContext.WebDriver, BrowserDefaults.TimeOut);
            var section = wait.Until(driver => driver.FindElement(_criteria));
            IReadOnlyCollection<IWebElement> elements = section.FindElements(By.CssSelector(".checkbox-tree.wrap"));

            foreach (var webElement in elements)
            {
                var t = (new Tree(webElement));
                _trees.Add(t);
            }
        }

        public void SelectEntireTree(string name)
        {
            foreach (var tree in _trees)
            {
                if (tree.GetParentText().Equals(name))
                {
                    tree.SelectParent();
                }
            }
        }

        public void Select(string treeName, params string[]  leaves)
        {
            foreach (var tree in _trees)
            {
                if (tree.GetParentText().Contains(treeName))
                {
                    ensureExpended(tree);
                    foreach (var leaf in leaves){
                        tree.Select(leaf);
                    }
                }
            }
        }

        private void ensureExpended(Tree tree)
        {
            if (!tree.IsExpanded())
            {
                tree.Expand();
            }
        }
     
        //Click on Radiobutton
        public void ClickRadioButton(string labelText)
        {
            var radio = WebContext.WebDriver.FindElements(By.CssSelector(".radio-text"));

            for (int i = 0; i < radio.Count; i++)
            {
                if(radio[i].Text == labelText)
                {                   
                    radio[i].Click();
                }
            }
            Thread.Sleep(3000); 
        }

        public IWebElement RadioButton(string labelText)
        {
            var radio = WebContext.WebDriver.FindElements(By.Name("IsDaily"));
            var radioText = WebContext.WebDriver.FindElements(By.CssSelector(".radio-text"));

            for (int i = 0; i < radioText.Count; i++)
            {
                if (radioText[i].Text == labelText)
                {
                    return radio[i];
                }
            }
            return null;
        }

        public void EnterDate(string date)
        {
            var Date = WebContext.WebDriver.FindElement(By.Name("StartDate"));
            Date.Clear();
            Date.SendKeys(date);
        }

        //Click on Search
        public AttendanceDetails EditMarksSearchButton()
        {
            var searchButton = WebContext.WebDriver.FindElement(EditMarksElements.SearchPanel.SearchButton);
            searchButton.Click();
            WaitUntilDisplayed(EditMarksElements.Toolbar.AdditionalColumnButton);
            return new AttendanceDetails();
        }

        public PupilPickerAvailablePupilSection PupilPickerSearchButton()
        {
            WaitForAndClick(BrowserDefaults.TimeOut, AttendanceElements.AddPupilPopUpElements.PupilPickerSearchButton);
            WaitUntilDisplayed(AttendanceElements.AddPupilPopUpElements.SearchRecordsToFindtext);
            return new PupilPickerAvailablePupilSection();
        }

            
    }
}
