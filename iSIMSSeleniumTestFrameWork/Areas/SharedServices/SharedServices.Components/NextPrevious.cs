using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.PageObjects;
using SharedComponents.BaseFolder;
using SharedComponents.Helpers;
using SharedServices.Components.PageObjects;
using SeSugar.Automation;
using WebDriverRunner.webdriver;
using Retry = SeSugar.Automation.Retry;
using SimsBy = SeSugar.Automation.SimsBy;

namespace SharedServices.Components
{
    public class NextPrevious : BaseSeleniumComponents
    {
        private const string ReadPupilDetail = "[data-section-id='search_results_list']>div:nth-child(1)>div:nth-child(1)>a";
        private const string PupilSearchResultDiv = "[data-section-id='search_results_list']>div:nth-child({0})>div[class='search-result-tile-detail loaded']";

        private readonly By _thirdPupilSearchResultRecord = By.CssSelector("[data-section-id='search_results_list']>div:nth-child(3)>div");

        public NextPrevious()
        {
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);
            PageFactory.InitElements(WebContext.WebDriver, this);
            WaitUntillAjaxRequestCompleted();
            AutomationSugar.NavigateMenu("Tasks", "Pupils", "Pupil Records");
        }

        public void SearchPupilAction()
        {
            var pupilscreen = new PupilScreen();
            Retry.Do(pupilscreen.ClickSearch);

            AutomationSugar.WaitFor(SimsBy.AutomationId("search_results_list"));
        }

        public void NavigateToFirstPupil()
        {
            AutomationSugar.ClickOn(SimsBy.CssSelector(ReadPupilDetail));
            AutomationSugar.WaitFor(SimsBy.CssSelector(string.Format(PupilSearchResultDiv, 1)));
        }

        public void NavigateToNextPupils()
        {
            var action = new Actions(WebContext.WebDriver);
            for (int counter = 2; counter <= 3; counter++)
            {
                action.SendKeys(Keys.ArrowDown).Perform();
                AutomationSugar.WaitFor(SimsBy.CssSelector(string.Format(PupilSearchResultDiv, counter)));
            }
        }

        public string GetCurrentSelectedPupilRecordsClassValue()
        {
            var lastRecord = SeleniumHelper.Get(_thirdPupilSearchResultRecord);

            return lastRecord != null ? lastRecord.GetAttribute("class") : string.Empty;
        }
    }
}
