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
    public class AddNewAgentDialog
    {
        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='add_new_agent_button']")]
        public IWebElement addNewAgentButton;

        [FindsBy(How = How.Name, Using = "LegalForename")]
        public IWebElement AgentForeName;


        [FindsBy(How = How.Name, Using = "LegalSurname")]
        public IWebElement AgentSurName;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='continue_button']")]
        public IWebElement continuebutton;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='cancel_button']")]
        public IWebElement cancelButton;


        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='back_button']")]
        public IWebElement backButton;

        [FindsBy(How = How.TagName, Using = "small")]
        public IWebElement headerTitleName;

        public AddNewAgentDialog()
        {

            BaseSeleniumComponents.WaitUntilDisplayed(AgentElements.MainScreen.AgentForename);
            PageFactory.InitElements(WebContext.WebDriver, this);
        }

        public bool BackKeyFunctioningProperly(string foreName, string surName)
        {
            Thread.Sleep(2000);
            backButton.Click();
            Thread.Sleep(2000);
         
            bool assert1 = AgentForeName.GetAttribute("value") == foreName;
            bool assert2 = AgentSurName.GetAttribute("value") == surName;

            return (assert1 && assert2);
                    
        }

        public bool IsNameAppended(string fname, string lname)
        {
            AgentForeName.SendKeys(fname);
            AgentSurName.SendKeys(lname);
            continuebutton.Click();
            Thread.Sleep(1000);
            return(headerTitleName.Text == fname + " " + lname); 

        }

        public MatchingAgentScreen PassingNames(string forename, string surname)
        {
            AgentForeName.SendKeys(forename);
            AgentSurName.SendKeys(surname);
            continuebutton.Click();
            Thread.Sleep(1000);
            return new MatchingAgentScreen();
        }

        public AgentServicesProvided EnterNames(string forename, string surname)
        {
            AgentForeName.SendKeys(forename);
            AgentSurName.SendKeys(surname);
            continuebutton.Click();
            Thread.Sleep(1000);
            return new AgentServicesProvided();
        }

        public static string RandomString(int length)
        {
            Thread.Sleep(20);
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

      
     

    }
}
