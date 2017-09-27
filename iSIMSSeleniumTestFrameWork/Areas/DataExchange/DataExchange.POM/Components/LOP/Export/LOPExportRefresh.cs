using System.Threading;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using SharedComponents.CRUD;
using SharedComponents.Helpers;
using TestSettings;
using WebDriverRunner.webdriver;
using DataExchange.POM.Components.Common;

namespace DataExchange.POM.Components.LOP.Export
{
    public class LopExportRefresh : AutoRefreshSeleniumComponents
    {
        private const string SearchButtonToFind = "button[type='submit']";

        [FindsBy(How = How.CssSelector, Using = SearchButtonToFind)]
        private IWebElement _searchButton;

        public LopExportRefresh()
        {
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.AssessmentCoordinator); 
            Thread.Sleep(1000);
            SeleniumHelper.NavigateMenu("Tasks", "Pupils", "Levels of Progression File Export");
            Thread.Sleep(1000);
            PageFactory.InitElements(WebContext.WebDriver, this);
        }

        public void SelectRecordAndGenerate(string academicYear)
        {
            IWebElement element = SeleniumHelper.Get(DataExchangeElements.LopExport.SearchArea);
            element.ChooseSelectorOption(DataExchangeElements.LopExport.AcademicYearSelector, academicYear);
            _searchButton.Click();

            WaitUntilDisplayed(DataExchangeElements.LopExport.SearchRecordsToFindtext);
            WaitForAndClick(BrowserDefaults.TimeOut, DataExchangeElements.LopExport.SearchRecordsToFindtext);
            Detail.WaitForDetail();
            if (SeleniumHelper.Get(DataExchangeElements.LopExport.LopStatus).Text == "Pending with Errors")
            {
                WaitUntilValueGetsPopulated(DataExchangeElements.LopExport.TotalPupilsTextBox);
                //clikc on genetrate
                WaitForAndClick(BrowserDefaults.TimeOut, DataExchangeElements.LopExport.GenerateLop);
            }
            else
            {
                WaitElementToBeClickable(DataExchangeElements.LopExport.CreateLop);
                WaitForAndClick(BrowserDefaults.TimeOut, DataExchangeElements.LopExport.CreateLop);
                //clickon create and wait
                WaitForAndClick(BrowserDefaults.TimeOut, DataExchangeElements.LopExport.GenerateLop);
            }
        }
    }
}
