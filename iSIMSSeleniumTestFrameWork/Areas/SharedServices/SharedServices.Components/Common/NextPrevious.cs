using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.PageObjects;
using SeSugar.Automation;
using SharedComponents.BaseFolder;
using SharedComponents.Helpers;
using WebDriverRunner.webdriver;
using Retry = SeSugar.Automation.Retry;
using SimsBy = SeSugar.Automation.SimsBy;

namespace SharedServices.Components.Common
{
    public class NextPrevious : BaseSeleniumComponents
    {
        private const string ReadPupilDetail = "[data-section-id='search_results_list']>div:nth-child(1)>div:nth-child(1)>a";
        private const string PupilSearchResultDiv = "[data-section-id='search_results_list']>div:nth-child({0})>div[class='search-result-tile-detail loaded']";

        private readonly By _thirdPupilSearchResultRecord = By.CssSelector("[data-section-id='search_results_list']>div:nth-child(3)>div");

        public NextPrevious()
        {
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator, enabledFeatures: Constants.SeleniumOnlyFeature);
            PageFactory.InitElements(WebContext.WebDriver, this);
            WaitUntillAjaxRequestCompleted();
            AutomationSugar.NavigateMenu("Tasks", "Shared Services", "Learner");
        }

        public void SearchPupilAction()
        {
            AutomationSugar.SearchAndWaitForResults(SimsBy.AutomationId("search_criteria"));
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
