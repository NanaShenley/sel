using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using SharedComponents.BaseFolder;
using WebDriverRunner.webdriver;

namespace Staff.Components.StaffRecord
{
    public class SearchStaffRecords : BaseSeleniumComponents
    {
        private const string CssSelectorToFind = "button[type='submit']";

        [FindsBy(How = How.CssSelector, Using = CssSelectorToFind)]
        public IWebElement SearchStaff;

        public SearchStaffRecords()
        {
            WaitForElement(By.CssSelector(CssSelectorToFind));
            PageFactory.InitElements(WebContext.WebDriver, this);
        }

        public void ClickSearchStaff()
        {
            SearchStaff.Click();
            WaitForElement(By.CssSelector("[class='list-group-item search-result-tile']"));
        }
    }
}
