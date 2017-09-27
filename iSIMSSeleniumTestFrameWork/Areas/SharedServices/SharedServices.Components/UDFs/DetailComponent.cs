using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using SeSugar.Automation;
using SharedComponents.Helpers;
using System;
using System.Linq;
using OpenQA.Selenium.Support.UI;
using WebDriverRunner.webdriver;

namespace SharedServices.Components.UDFs
{
    public class DetailComponent
    {
        private const string AddNewButton = "[data-automation-id='add_button']";
        private const string SaveButton = "[data-automation-id='well_know_action_save']";
        private const string DeleteButton = "[data-automation-id='delete_button']";
        private const string ContinueWithDeleteButton = "[data-automation-id='continue_with_delete_button']";

        private const string SuccessMessageBanner = "status_success";

        private const string DetailDescription = "input[name='Description']";
        private const string DetailFieldTypesSelector = "input[name=\"FieldTypes.dropdownImitator\"]";
        private const string DetailFieldTypesBindingSelector = "select[name=\"FieldTypes.Binding\"]";
        private const string DetailScreenSelector = "input[name=\"Screen.dropdownImitator\"]";
        private const string DetailScreenBindingSelector = "select[name=\"Screen.Binding\"]";
        private const string DetailSectionSelector = "input[name=\"Section.dropdownImitator\"]";
        private const string DetailSectionBindingSelector = "select[name=\"Section.Binding\"]";
        private const string DetailIsVisibleCheckbox = "input[name='IsVisible']";

        [FindsBy(How = How.CssSelector, Using = AddNewButton)]
        private IWebElement _addButton;

        [FindsBy(How = How.CssSelector, Using = SaveButton)]
        private IWebElement _saveButton;

        [FindsBy(How = How.CssSelector, Using = DeleteButton)]
        private IWebElement _deleteButton;

        [FindsBy(How = How.CssSelector, Using = ContinueWithDeleteButton)]
        private IWebElement _continueWithDeleteButton;

        [FindsBy(How = How.CssSelector, Using = DetailDescription)]
        private IWebElement _descriptionElement;

        [FindsBy(How = How.CssSelector, Using = DetailFieldTypesSelector)]
        private IWebElement _fieldTypesElement;

        [FindsBy(How = How.CssSelector, Using = DetailFieldTypesBindingSelector)]
        private IWebElement _fieldTypesBindingElement;

        [FindsBy(How = How.CssSelector, Using = DetailScreenSelector)]
        private IWebElement _screenElement;

        [FindsBy(How = How.CssSelector, Using = DetailScreenBindingSelector)]
        private IWebElement _screenBindingElement;

        [FindsBy(How = How.CssSelector, Using = DetailSectionSelector)]
        private IWebElement _sectionElement;

        [FindsBy(How = How.CssSelector, Using = DetailSectionBindingSelector)]
        private IWebElement _sectionBindingElement;

        [FindsBy(How = How.CssSelector, Using = DetailIsVisibleCheckbox)]
        private IWebElement _isActiveElement;

        private IWebElement successMessageBanner => SeleniumHelper.Get(SeleniumHelper.SelectByDataAutomationID(SuccessMessageBanner));

        public bool DescriptionEnabled => !(_descriptionElement.GetAttribute("readonly")?.Equals("true") ?? false);
        public bool FieldTypesEnabled => _fieldTypesElement.Enabled;
        public bool ScreenEnabled => _screenElement.Enabled;
        public bool SectionEnabled => _sectionElement.Enabled;
        public bool SaveButtonAvailable => WebContext.WebDriver.FindElements(By.CssSelector(SaveButton)).Any();
        public bool DeleteButtonAvailable => WebContext.WebDriver.FindElements(By.CssSelector(DeleteButton)).Any();

        public void AddNew()
        {
            _addButton.Click();
            AutomationSugar.WaitForAjaxCompletion();
        }

        public void Populate(string description)
        {
            _descriptionElement.SetText(description);

            var fieldType = _fieldTypesBindingElement.FindElements(By.TagName("option")).First();
            _fieldTypesElement.ChooseSelectorOption(fieldType.Text);

            var screen = _screenBindingElement.FindElements(By.TagName("option")).First();
            _screenElement.ChooseSelectorOption(screen.Text);

            AutomationSugar.WaitForAjaxCompletion();

            var section = _sectionBindingElement.FindElements(By.TagName("option")).Last();
            _sectionElement.ChooseSelectorOption(section.Text);

            _isActiveElement.SetCheckBox(true);
        }

        public void Save()
        {
            _saveButton.Click();
            AutomationSugar.WaitForAjaxCompletion();
        }

        public void Delete()
        {
            _deleteButton.Click();
            AutomationSugar.WaitForAjaxCompletion();
            WebDriverWait wait = new WebDriverWait(WebContext.WebDriver, TimeSpan.FromSeconds(5));
            wait.Until(ExpectedConditions.ElementToBeClickable(_continueWithDeleteButton));
            _continueWithDeleteButton.Click();
            AutomationSugar.WaitForAjaxCompletion();
        }

        /// <summary>
        /// Gets the success message banner
        /// </summary>
        /// <returns></returns>
        public IWebElement GetSuccessMessage()
        {
            return successMessageBanner;
        }
    }
}
