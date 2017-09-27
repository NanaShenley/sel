using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using SharedComponents.BaseFolder;
using WebDriverRunner.webdriver;

namespace Pupil.Components
{
    public class SENDetails : BaseSeleniumComponents
    {
        private const string  cssSelectorToFind = "h3[class=\"section-header-title\"] span:nth-child(3)";

        [FindsBy(How = How.CssSelector, Using = cssSelectorToFind)]
        public IWebElement PreferredListName;

        public SENDetails()
        {
            WaitForElement(By.CssSelector(cssSelectorToFind));
            PageFactory.InitElements(WebContext.WebDriver, this);
        }
    }
}