using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using SharedComponents.BaseFolder;
using SharedComponents.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WebDriverRunner.webdriver;


namespace Agents.Components
{
    public class AgentSearchScreen
    {
        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='add_new_agent_button']")]
        public IWebElement addNewAgentButton;

        [FindsBy(How = How.Name, Using = "LegalForename")]
        public IWebElement AgentForeName;

        [FindsBy(How = How.Name, Using = "LegalSurname")]
        public IWebElement AgentSurName;

        [FindsBy(How = How.Name, Using = "LegalForenameOfAgent")]
        public IWebElement MatchingAgentForeName;

        [FindsBy(How = How.Name, Using = "LegalSurnameOfAgent")]
        public IWebElement MatchingAgentSurName;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='continue_button']")]
        public IWebElement continuebutton;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='cancel_button']")]
        public IWebElement cancelButton;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='status_error']")]
        public IWebElement validationmessage;

        [FindsBy(How = How.CssSelector, Using = ".validation-summary-errors")]
        public IWebElement onevalidationmessage;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='status_success']")]
        public IWebElement SuccessMessage;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='search_criteria_submit']")]
        public IWebElement searchButton;

        [FindsBy(How = How.Name, Using = "NameSearchText")]
        public IWebElement searchVal;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='delete_button']")]
        public IWebElement deleteAgentButton;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='continue_with_delete_button']")]
        public IWebElement deleteAgentContinueButton;

        public AgentSearchScreen()
        {
            PageFactory.InitElements(WebContext.WebDriver, this);
        }

        public bool IsAddAnewAgentDisplayed_OnNavigationToAgentScreen()
        {
            return addNewAgentButton.Displayed;
        }

        public bool IsSuccessMessageDisplayed()
        {
            BaseSeleniumComponents.WaitUntilEnabled(AgentElements.MainScreen.SuccessMessage);
            return SuccessMessage.Displayed;
        }

        public bool IsAddNewAgentDialogDisplayed()
        {
            addNewAgentButton.Click();           
            POM.Helper.Wait.WaitForElementDisplayed(By.Name("LegalForename"));
            POM.Helper.SeleniumHelper.Sleep(1);
            bool returnVal =  AgentForeName.Displayed;
            return returnVal;
        }

        public bool CheckValidation(string foreName, string surName)
        {
            if (foreName == "" && surName != "")
            {
                AgentForeName.Clear();
                AgentSurName.SendKeys(surName);
            }
            else if (surName == "" && foreName != "")
            {
                AgentSurName.Clear();
                AgentForeName.SendKeys(foreName);
            }
            else if (foreName == "" && surName == "")
            {
                AgentForeName.Clear();
                AgentSurName.Clear();
                Thread.Sleep(1000);
                continuebutton.Click();
                BaseSeleniumComponents.WaitUntilDisplayed(By.CssSelector("[data-automation-id='status_error']"));
                return validationmessage.Displayed;
            }
            else if (foreName != "" && surName != "")
            {
                AgentForeName.SendKeys(foreName);
                AgentSurName.SendKeys(surName);
            }
            else return false;

            continuebutton.Click();
            Thread.Sleep(1000);
            string val = onevalidationmessage.GetAttribute("textContent");

            if (val.Contains("The Surname field is required."))
                return true;
            else if (val.Contains("The Forename field is required"))
                return true;
            else
                return false;
        }

        public bool DialogDisappeared()
        {
            addNewAgentButton.Click();
            BaseSeleniumComponents.WaitUntilDisplayed(AgentElements.MainScreen.AgentForename);
            BaseSeleniumComponents.WaitUntilDisplayed(By.CssSelector("[data-automation-id='cancel_button']"));
            POM.Helper.SeleniumHelper.ClickByJS(cancelButton);            
            Thread.Sleep(2000);
            IReadOnlyCollection<IWebElement> list = WebContext.WebDriver.FindElements(By.Name("LegalForename"));
            bool val = (list.Count == 0);
            return val;
        }

        public AddNewAgentDialog ClickAddNewAgent()
        {
            addNewAgentButton.Click();
            return new AddNewAgentDialog();
        }

        public bool SelectAgentRecordFromSearch(string fname, string lname)
        {
            searchVal.SendKeys(fname + " " + lname);
            searchButton.Click();
            BaseSeleniumComponents.WaitUntilDisplayed(By.CssSelector("[data-automation-id='resultTile']"));
            IWebElement searchRecord = WebContext.WebDriver.FindElement(By.CssSelector("[data-automation-id='resultTile']"));
            searchRecord.Click();
            return true;
        }

        public bool DeleteAgentRecord()
        {
            try
            {
                BaseSeleniumComponents.WaitUntilDisplayed(By.CssSelector("[data-automation-id='delete_button']"));
                deleteAgentButton.Click();
                BaseSeleniumComponents.WaitUntilDisplayed(By.CssSelector("[data-automation-id='continue_with_delete_button']"));                
                //Thread.Sleep(500);
                deleteAgentContinueButton.Click();
                return true;

            }
            catch (Exception)
            {
                return false;
            }

        }
    }
}
