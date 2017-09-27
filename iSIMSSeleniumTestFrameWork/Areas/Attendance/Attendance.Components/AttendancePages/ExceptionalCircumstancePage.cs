using Attendance.Components.Common;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using OpenQA.Selenium.Support.UI;
using SharedComponents.BaseFolder;
using POM.Helper;
using TestSettings;
using WebDriverRunner.webdriver;

namespace Attendance.Components.AttendancePages
{
    public class ExceptionalCircumstancePage : BaseSeleniumComponents
    {
#pragma warning disable 0649
        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='Button_Dropdown']")]
        public readonly IWebElement createButton;
        [FindsBy(How = How.CssSelector, Using = ".layout-col .main .pane")]
        public readonly IWebElement _mainPage;
        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='Whole']")]
        public readonly IWebElement wholeSchoolOption;
        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='Selected']")]
        public readonly IWebElement selectedPupilOption;


        public ExceptionalCircumstancePage()
        {
            WaitUntilDisplayed(AttendanceElements.ExceptionalCircumstanceElements.CreateExceptionalCircumstances);
            PageFactory.InitElements(WebContext.WebDriver, this);
        }


        public void ClickCreate()
        {
            WaitForElement(AttendanceElements.ExceptionalCircumstanceElements.CreateExceptionalCircumstances);
            createButton.Click();
        }

        public CreateExceptionalCircumstancesPage ClickWholeSchoolOption()
        {
            WaitElementToBeClickable(AttendanceElements.ExceptionalCircumstanceElements.WholeSchoolOption);
            wholeSchoolOption.Click();
            Wait.WaitLoading();
            return new CreateExceptionalCircumstancesPage();
        }

        public CreateExceptionalCircumstancesPage ClickSelectedPupilOption()
        {
            WaitElementToBeClickable(AttendanceElements.ExceptionalCircumstanceElements.SelectedPupilOption);
            selectedPupilOption.Click();
            Wait.WaitLoading();
            return new CreateExceptionalCircumstancesPage();
        }

        public void WaitElementToBeClickable(By locator)
        {
            WebDriverWait wait = new WebDriverWait(WebContext.WebDriver, BrowserDefaults.TimeOut);
            wait.Until(ExpectedConditions.ElementToBeClickable(locator));
        }

    }
}
