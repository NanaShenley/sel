using Attendance.Components.Common;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using POM.Helper;
using SharedComponents.BaseFolder;
using SharedComponents.Helpers;
using WebDriverRunner.webdriver;

namespace Attendance.Components.AttendancePages
{
    public class PupilPickerSelectedPupilSection : BaseSeleniumComponents
    {
#pragma warning disable 0649
        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='ok_button']")]
        public readonly IWebElement PupilPickerOkButton;
        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='cancel_button']")]
        public readonly IWebElement PupilPickerCancelButton;

        public PupilPickerSelectedPupilSection()
        {
            var loc = By.CssSelector("[data-automation-id='ok_button']");
            WaitForElement(loc);
            PageFactory.InitElements(WebContext.WebDriver, this);
        }

        public AttendancePatternPage ClickAttendancePattern_PupilPickerOkButton()
        {
            PupilPickerOkButton.Click();
            Wait.WaitLoading();
            return new AttendancePatternPage();
        }

        public ApplyMarkOverDateRangePage ClickApplyMarkOverDateRange_PupilPickerOkButton()
        {
            PupilPickerOkButton.Click();
            Wait.WaitLoading();
            return new ApplyMarkOverDateRangePage();
        }

        public CreateExceptionalCircumstancesPage ClickExceptionalCircumstances_PupilPickerOkButton()
        {
            PupilPickerOkButton.Click();
            Wait.WaitLoading();
            return new CreateExceptionalCircumstancesPage();
        }
    }
}
