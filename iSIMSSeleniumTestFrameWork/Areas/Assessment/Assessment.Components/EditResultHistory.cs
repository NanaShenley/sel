using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using SharedComponents.BaseFolder;
using SharedComponents.Helpers;
using System.Collections.Generic;
using System.Linq;
using TestSettings;
using WebDriverRunner.webdriver;

namespace Assessment.Components
{
    public class EditResultHistory : BaseSeleniumComponents
    {
        public const string ResultHistoryDialog = "[data-show-result-history-dialog='']";
        public const string historydialogid = "[data-grid-table='']";
        public const string historyresultinputlist = "//div[contains(@class,'grid-cell-control')]/input";
        public const string historyresultdelete = "btn-link";
        public const string ok_button = "[data-automation-id='ok_button']";
        public const string delete_confirmation = "[data-automation-id='Yes_button']";

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='Button_DropdownRadio']")]
        private IWebElement _AssessmentYear;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='2014/2015']")]
        private IWebElement _SelectAssessmentYEar;

        public EditResultHistory()
        {
            PageFactory.InitElements(WebContext.WebDriver, this);
        }

        public void Login_Navigate_Marksheet(SeleniumHelper.iSIMSUserType userType = SeleniumHelper.iSIMSUserType.AssessmentCoordinator)
        {
            WebContext.WebDriver.Manage().Window.Maximize();
            PageFactory.InitElements(WebContext.WebDriver, this);

            SeleniumHelper.Login(userType);
            SeleniumHelper.NavigateMenu("Tasks", "Assessment", "Marksheets");
            PageFactory.InitElements(WebContext.WebDriver, this);
        }

        public EditResultHistory OpenMarksheet(string marksheetName)
        {
            WaitForAndClick(BrowserDefaults.TimeOut, By.LinkText(marksheetName));
            return this;
        }

        public  void OpenResultHistoryDialog()
        {
            WaitForAndClick(BrowserDefaults.TimeOut, By.CssSelector(ResultHistoryDialog));

        }

        public List<IWebElement> GetResultelements()
        {
            List<IWebElement> resultsList = WaitForAndGet(By.CssSelector(historydialogid)).FindElements(By.XPath(historyresultinputlist)).ToList();
          
            return resultsList;
        }

        public List<IWebElement> GetResultelementdeletebutton()
        {
            List<IWebElement> deletbutton = WaitForAndGet(By.CssSelector(historydialogid)).FindElements(By.ClassName(historyresultdelete)).ToList();
           
            return deletbutton;
        }

        public void ClickOkButton()
        {
            WaitForElement(By.CssSelector("[data-automation-id='ok_button']"));
           WebContext.WebDriver.FindElements(By.CssSelector("[data-automation-id='ok_button']")).Where(x => x.Displayed == true).FirstOrDefault().Click();
           

        }

        public void ClickDeleteConfirmButton()
        {
            WaitForElement(By.CssSelector(delete_confirmation));
            WebContext.WebDriver.FindElement(By.CssSelector(delete_confirmation)).Click();

        }

        public void OpenClickAssessmentYear()
        {
            WaitUntilDisplayed(By.CssSelector("[data-automation-id='Button_DropdownRadio']"));
            _AssessmentYear.Click();

            WaitUntilDisplayed(By.CssSelector("[data-automation-id='2014/2015']"));
            _SelectAssessmentYEar.Click();

        }



    }
}
