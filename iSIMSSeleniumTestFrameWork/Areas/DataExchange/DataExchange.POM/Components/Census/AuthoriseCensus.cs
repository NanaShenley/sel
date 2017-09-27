using DataExchange.POM.Helper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using WebDriverRunner.webdriver;

namespace DataExchange.POM.Components.Census
{
    public class AuthoriseCensus
    {
        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='ActivateCustomBehaviourButton-Authorise']")]
        private readonly IWebElement _authoriseButton;

        public AuthoriseCensus()
        {           
            Wait.WaitUntilDisplayed(By.CssSelector("[data-section-id='detail']"));
            PageFactory.InitElements(WebContext.WebDriver, this);
        }

       //To authorise particular return
        public void ClickAuthoriseButton()
        {
            By loc = SimsBy.AutomationId("section_menu_Census Details");
            Wait.WaitUntilDisplayed(loc);
            Wait.WaitForElementDisplayed(SimsBy.AutomationId("ActivateCustomBehaviourButton-Authorise"));
            Assert.IsTrue(_authoriseButton.Displayed);
            _authoriseButton.Click();
        }

        public void AuthoriseDialog()
        {
            AuthoriseConfirmationDialog dialogCheck = new AuthoriseConfirmationDialog();
            dialogCheck.Clickok();            
        }      
    }
}
