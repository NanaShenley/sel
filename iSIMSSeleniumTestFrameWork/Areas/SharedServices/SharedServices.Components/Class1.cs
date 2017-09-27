using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using SharedComponents.BaseFolder;
using WebDriverRunner.webdriver;

namespace SharedServices.Components
{
    public class Class1 : BaseSeleniumComponents
    {
        private const string ExampleCssSelectorToFind = "a[class='Example']";

        [FindsBy(How = How.CssSelector, Using = ExampleCssSelectorToFind)]
        public IWebElement ExampleWebElement;

        public Class1()
        {
            WaitForElement(By.CssSelector(ExampleCssSelectorToFind));
            PageFactory.InitElements(WebContext.WebDriver, this);
        }
    }
}