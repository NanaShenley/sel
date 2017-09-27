using OpenQA.Selenium;
using SharedComponents.BaseFolder;
using SharedComponents.CRUD;
using OpenQA.Selenium.Support.PageObjects;
using SharedComponents.Helpers;
using SharedComponents.HomePages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WebDriverRunner.webdriver;
using System.Collections.ObjectModel;

namespace Agents.Components
{
    public class MatchingAgentScreen
    {

        [FindsBy(How = How.CssSelector, Using = "div.alert.alert-info.message-control.clearfix")]
        public IWebElement DivForAlert;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='back_button']")]
        public IWebElement backButton;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='no,_this_is_a_new_agent_button']")]
        public IWebElement NewAgentButton;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='cancel_button']")]
        public IWebElement cancelButton;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='yes,_open_agent_record_button']")]
        public IWebElement Open_Matching_Agent_Record;

        [FindsBy(How = How.CssSelector, Using = "")]
        public IWebElement tableId;

        public MatchingAgentScreen()
        {
            BaseSeleniumComponents.WaitUntilDisplayed(AgentElements.MatchingAgentScreen.New_Agent_Button);
            PageFactory.InitElements(WebContext.WebDriver, this);
        }

        public bool IsAlertAppearing()
        {
            return DivForAlert.Text.Contains("The following matches have been found");
        }

        public AgentSearchScreen ClickMatchingAgentRecord()
        {
            Open_Matching_Agent_Record.Click();
            return new AgentSearchScreen();
        }

        public AddNewAgentDialog BackKeyClick()
        {
            backButton.Click();
            return new AddNewAgentDialog();
        }

        public void CancelKeyClick()
        {
            cancelButton.Click();
            Thread.Sleep(1000);
        }

        public bool BackButtonCount()
        {
            IReadOnlyCollection<IWebElement> list = WebContext.WebDriver.FindElements(By.CssSelector("[data-automation-id='back_button']"));
            bool val = (list.Count == 0);
            return val;
        }

        public AgentServicesProvided ClickOnNewAgentButton()
        {
            NewAgentButton.Click();
            return new AgentServicesProvided();
        }

        private IWebElement AgentTable
        {
            get
            {
                return WebContext.WebDriver.FindElement(By.CssSelector("table.table.grid"));
            }
        }

        public IWebElement FindRow(int rownum)
        {
            ReadOnlyCollection<IWebElement> rows = AgentTable.FindElements(By.TagName("tr"));
            return rows[rownum];
        }

        public IWebElement FindCell(int rownum, int cellnumber)
        {
            IWebElement row = FindRow(rownum);
            ReadOnlyCollection<IWebElement> cells = row.FindElements(By.TagName("td"));
            return cells[cellnumber];
        }

    }
}
