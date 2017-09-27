using DataExchange.POM.Helper;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using WebDriverRunner.webdriver;

namespace DataExchange.POM.Components.PLASC
{
    public class AuthoriseReturn 
    {
        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='ActivateCustomBehaviourButton-Authorise']")]
        private readonly  IWebElement _authoriseButton;

        public AuthoriseReturn()
        {
            Wait.WaitUntilDisplayed(By.CssSelector("[data-section-id='detail']"));
            PageFactory.InitElements(WebContext.WebDriver, this);
        }

        //To authorise particular return
        public void ClickAuthoriseButton()
        {
            By loc = SimsBy.AutomationId("section_menu_Census Details");
            Wait.WaitUntilDisplayed(loc);
            WebContext.WebDriver.FindElement(By.CssSelector("[data-automation-id='ActivateCustomBehaviourButton-Authorise']")).SendKeys(Keys.Enter);         
        }

        public void AuthoriseDialog()
        {
            AuthoriseConfirmationDialog dialogCheck = new AuthoriseConfirmationDialog();
            dialogCheck.Clickok();
        }
    }
}
