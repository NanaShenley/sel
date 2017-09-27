using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using OpenQA.Selenium.Support.UI;
using POM.Helper;
using SharedComponents.BaseFolder;
using SharedComponents.CRUD;
using TestSettings;
using WebDriverRunner.webdriver;

namespace Pupil.BulkUpdate.Tests.ApplicationStatusComponents
{
    public class ApplicationStatusSearch : BaseSeleniumComponents
    {
        public static readonly By SearchButton = By.CssSelector("button[type='submit']");

        // Year Group navigation
        private readonly By YearGroupValue = By.CssSelector("input[name='YearGroup.dropdownImitator']");

        [FindsBy(How = How.Name, Using = "YearGroup.dropdownImitator")]
        private IWebElement _admissionYear;

        // Intake Group navigation
        private readonly By IntakeGroupValue = By.CssSelector("input[name='IntakeGroup.dropdownImitator']");

        [FindsBy(How = How.Name, Using = "IntakeGroup.dropdownImitator")]
        private IWebElement _schoolIntake;

        // Admission Group navigation
        private readonly By AdmissionGroupValue = By.CssSelector("input[name='AdmissionGroup.dropdownImitator']");

        [FindsBy(How = How.Name, Using = "AdmissionGroup.dropdownImitator")]
        private IWebElement _admissionGroup;

        /// <summary>
        /// Application Bulk Update page object
        /// </summary>
        public ApplicationStatusSearch()
        {
            PageFactory.InitElements(WebContext.WebDriver, this);
        }

        public string AdmissionYear
        {
            get { return SeleniumHelper.GetValue(_admissionYear); }
            set
            {
                _admissionYear.ChooseSelectorOption(value);
                Wait.WaitForAjaxReady(By.CssSelector(".locking-mask-loading"));
            }
        }

        public string IntakeGroup
        {
            get { return SeleniumHelper.GetValue(_schoolIntake); }
            set
            {
                _schoolIntake.ChooseSelectorOption(value);
                Wait.WaitForAjaxReady(By.CssSelector(".locking-mask-loading"));
            }
        }

        public string AdmissionGroup
        {
            get { return SeleniumHelper.GetValue(_admissionGroup); }
            set
            {
                _admissionGroup.ChooseSelectorOption(value);
                Wait.WaitForAjaxReady(By.CssSelector(".locking-mask-loading"));
            }
        }

        /// <summary>
        /// Application Bulk Update SEARCH button
        /// </summary>
        /// <returns></returns>
        public ApplicationStatusDetail ClickOnSearch()
        {
            SeleniumHelper.FindAndClick(SearchButton);
            WaitUntilEnabled(SearchButton);
            return new ApplicationStatusDetail();
        }

        /// <summary>
        /// Application Bulk Update: Year Group dropdown
        /// </summary>
        /// <returns></returns>
        public string FindYearGroupIselectorDropDown()
        {
            SearchCriteria.SetCriteria("YearGroup.dropdownImitator", "2015/2016");
            waitForElementToExist(By.CssSelector("[name='YearGroup.Binding']"));
            var selectElement = WebContext.WebDriver.FindElement(By.CssSelector("[name='YearGroup.Binding']"));
            var mySelect = new SelectElement(selectElement);
            var selectedOption = mySelect.SelectedOption;
            var optionText = selectedOption.GetAttribute("innerText");

            // Return value
            return optionText;
        }

        /// <summary>
        /// Application Bulk Update: Intake Group dropdown
        /// </summary>
        /// <returns></returns>
        public string FindIntakeGroupIselectorDropDown()
        {
            SearchCriteria.SetCriteria("IntakeGroup.dropdownImitator", "2015/2016 - Summer Year 2");
            waitForElementToExist(By.CssSelector("[name='IntakeGroup.Binding']"));
            var selectElement = WebContext.WebDriver.FindElement(By.CssSelector("[name='IntakeGroup.Binding']"));
            var mySelect = new SelectElement(selectElement);
            var selectedOption = mySelect.SelectedOption;
            var optionText = selectedOption.GetAttribute("innerText");

            // Return value
            return optionText;
        }

        /// <summary>
        /// Application Bulk Update: Admission Group dropdown
        /// </summary>
        /// <returns></returns>
        public string FindAdmissionGroupIselectorDropDown()
        {
            SearchCriteria.SetCriteria("AdmissionGroup.dropdownImitator", "Admission Group 3-1 Inactive");
            waitForElementToExist(By.CssSelector("[name='AdmissionGroup.Binding']"));
            var selectElement = WebContext.WebDriver.FindElement(By.CssSelector("[name='AdmissionGroup.Binding']"));
            var mySelect = new SelectElement(selectElement);
            var selectedOption = mySelect.SelectedOption;
            var optionText = selectedOption.GetAttribute("innerText");

            // Return value
            return optionText;
        }

        /// <summary>
        /// Control test to show that trying to select a non-existant option results in failure
        /// </summary>
        /// <returns></returns>
        public string NeverFindAdmissionGroupIselectorDropDown()
        {
            SearchCriteria.SetCriteria("AdmissionGroup.dropdownImitator", "This will never be found");
            waitForElementToExist(By.CssSelector("[name='AdmissionGroup.Binding']"));
            var selectElement = WebContext.WebDriver.FindElement(By.CssSelector("[name='AdmissionGroup.Binding']"));
            var mySelect = new SelectElement(selectElement);
            var selectedOption = mySelect.SelectedOption;
            var optionText = selectedOption.GetAttribute("innerText");

            // Return value
            return optionText;
        }

        public string GetYearGroupValue()
        {
            var yearGroupDdl = POM.Helper.SeleniumHelper.Get(YearGroupValue);
            return yearGroupDdl.Text;
        }

        public string GetIntakeGroupValue()
        {
            var yearGroupDdl = POM.Helper.SeleniumHelper.Get(IntakeGroupValue);
            return yearGroupDdl.Text;
        }

        public string GetAdmissionGroupValue()
        {
            var yearGroupDdl = POM.Helper.SeleniumHelper.Get(AdmissionGroupValue);
            return yearGroupDdl.Text;
        }

        /// <summary>
        /// A simple helper to delay execution until the named element exists somewhere in the DOM.
        /// </summary>
        /// <param name="selector"></param>
        public void waitForElementToExist(By selector)
        {
            WebDriverWait wait = new WebDriverWait(WebContext.WebDriver, BrowserDefaults.TimeOut);
            wait.Until(ExpectedConditions.ElementExists(selector));
        }
    }
}