using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using SharedComponents.BaseFolder;
using WebDriverRunner.webdriver;

namespace DataExchange.Components
{
    public class DataExchange : BaseSeleniumComponents
    {
        private const string ExampleCssSelectorToFind = "a[class='Example']";

        [FindsBy(How = How.CssSelector, Using = ExampleCssSelectorToFind)]
        public IWebElement ExampleWebElement;

        public DataExchange()
        {
            WaitForElement(By.CssSelector(ExampleCssSelectorToFind));
            PageFactory.InitElements(WebContext.WebDriver, this);
        }
    }
}