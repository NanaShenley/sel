using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
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
using SeSugar;
using POM.Helper;

namespace Agents.Components
{
    public class AgentServicesProvided
    {

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='Audiometrist']")]
        public IWebElement CheckBoxAudiometrist;


        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='add_record_button']")]
        public IWebElement CreateRecordButton;

        [FindsBy(How = How.CssSelector, Using = ".validation-summary-errors")]
        public IWebElement AlertMessage;


        public AgentServicesProvided()
        {
            BaseSeleniumComponents.WaitUntilDisplayed(AgentElements.ServicesProvidedScreen.CreateRecordButton);
            PageFactory.InitElements(WebContext.WebDriver, this);
        }

        public void CreateButtonClick()
        {
            CreateRecordButton.Click();
            BaseSeleniumComponents.WaitUntilDisplayed(AgentElements.ServicesProvidedScreen.AlertMessage);
        }

        public AgentSearchScreen SelectAgentService()
        {
            Thread.Sleep(1000);
            BaseSeleniumComponents.WaitUntilDisplayed(AgentElements.ServicesProvidedScreen.CheckBoxAudiometrist);
            
            Actions action = new Actions(WebContext.WebDriver);
            action.MoveToElement(CheckBoxAudiometrist);
            action.SendKeys(Keys.Space);            
            action.Build().Perform();
            //CheckBoxAudiometrist.Click();
            Thread.Sleep(500);
            CreateRecordButton.Click();
            Wait.WaitForAjaxReady(By.CssSelector(".locking-mask-loading"));
            
            return new  AgentSearchScreen();
        }

        public bool IsAlertMessageAppearingOnKeepingServiceEmpty()
        {
            POM.Helper.SeleniumHelper.Sleep(1);
            return AlertMessage.Displayed;
        }

    }
}
