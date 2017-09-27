using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using OpenQA.Selenium.Support.UI;
using Pupil.Components.Common;
using SharedComponents.BaseFolder;
using SharedComponents.CRUD;
using TestSettings;
using WebDriverRunner.webdriver;

namespace Pupil.Components.BulkUpdate.ApplicantParentalSalutationAndAddressee
{
    /// <summary>
    /// PageObject class for bulk update applicant parental salutation and addressee search section
    /// </summary>
    public class ApplicantParentalSalutationAndAddresseeSearch : BaseSeleniumComponents
    {

        /// <summary>
        /// Represents search criteria
        /// </summary>
        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='search_criteria']")]        
        private IWebElement _searchCriteria;

        // Year Group 
        [FindsBy(How = How.CssSelector, Using = "input[name='YearGroup.dropdownImitator']")]
        private IWebElement _yearGroupValue;

        // Intake Group 
        [FindsBy(How = How.CssSelector, Using = "input[name='IntakeGroup.dropdownImitator']")]
        private IWebElement _intakeGroupValue;

        // Admission Group 
        [FindsBy(How = How.CssSelector, Using = "input[name='AdmissionGroup.dropdownImitator']")]
        private IWebElement _admissionGroupValue;

        /// <summary>
        /// Constructor implemented to initialise the elements in this page object
        /// </summary>
        public ApplicantParentalSalutationAndAddresseeSearch()
        {
            PageFactory.InitElements(WebContext.WebDriver, this);
            //Adding a timespan was not working, not needed as the tests run fine
            //WaitForElement(PupilBulkUpdateElements.BulkUpdate.Search.SearchButton);
        }

        public string GetYearGroupValue()
        {
            return _yearGroupValue.Text;
        }

        public string GetIntakeGroupValue()
        {
            return _intakeGroupValue.Text;
        }

        public string GetAdmissionGroupValue()
        {
            return _admissionGroupValue.Text;
        }


        /// <summary>
        ///  Year Group dropdown
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
        ///  Intake Group dropdown
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
        /// Admission Group dropdown
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
