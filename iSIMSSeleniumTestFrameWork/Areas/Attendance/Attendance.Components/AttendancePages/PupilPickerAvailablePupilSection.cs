using Attendance.Components.Common;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using SharedComponents.BaseFolder;
using TestSettings;
using WebDriverRunner.webdriver;

namespace Attendance.Components.AttendancePages
{
    public class PupilPickerAvailablePupilSection : BaseSeleniumComponents
    {
#pragma warning disable 0649
        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='ok_button']")]
        public readonly IWebElement PupilPickerOkButton;
        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='cancel_button']")]
        public readonly IWebElement PupilPickerCancelButton;
        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='add_all_button']")]
        public readonly IWebElement AddAllPupilButton;
        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='add_selected_button']")]
        public readonly IWebElement AddSelectedPupilButton;
        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='search_result']")]
        public readonly IWebElement searchResults;
        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='search_results_counter']")]
        public readonly IWebElement searchResultsCounter;

        public PupilPickerAvailablePupilSection()
        {
            var loc = By.CssSelector("[data-automation-id='ok_button']");
            WaitForElement(loc);
            PageFactory.InitElements(WebContext.WebDriver, this);
        }

        public void GetAvailablePupils()
        {
            WaitForAndClick(BrowserDefaults.TimeOut, AttendanceElements.AddPupilPopUpElements.SearchRecordsToFindtext);
        }

        public PupilPickerSelectedPupilSection AddAllPupil()
        {
            AddAllPupilButton.Click();
            return new PupilPickerSelectedPupilSection();
        }

        public PupilPickerSelectedPupilSection AddSelectedPupil()
        {
            AddSelectedPupilButton.Click();
            return new PupilPickerSelectedPupilSection();
        }

        //private IWebElement GetPupils(string p)
        //{
        //    return WebContext.WebDriver.FindElement(By.CssSelector("[data-automation-id='" + p + "']"));
        //}
    }
}
