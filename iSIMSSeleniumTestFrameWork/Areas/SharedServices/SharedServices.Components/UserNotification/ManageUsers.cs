using System;
using System.Linq;
using OpenQA.Selenium.Support.PageObjects;
using SharedComponents.BaseFolder;
using SharedComponents.Helpers;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeSugar.Automation;
using SharedServices.Components.Common;
using WebDriverRunner.webdriver;
using SimsBy = SeSugar.Automation.SimsBy;

#pragma warning disable 649

namespace SharedServices.Components.UserNotification
{
    public class ManageUsers : BaseSeleniumComponents
    {
        public enum UserScreenElements
        {
            EnableButton,
            DisableButton,
            PopUpEnableButton,
            PopUpDisableButton,
            PopUpCancelButton
        }

        private string _name;

        private const string SaveButton = "well_know_action_save";
        private const string SearchButton = "[data-automation-id='search_criteria_submit']";
        private const string MoreFiltersButton = "[data-automation-id='search_criteria_advanced']";
        private const string SearchCriteriaTextbox = "input[name='Name']";

        private const string SearchResultCount = "result-count";

        private const string EnableUserButton = "[title='Enable User']";
        private const string DisableUserButton = "[title='Disable User']";
        private const string PopUpEnableButton = "div.modal-footer > button[data-automation-id='enable_user_button']";
        private const string PopUpDisableButton = "div.modal-footer > button[data-automation-id='disable_user_button']";
        private const string PopUpCancelButton = "[data-automation-id='cancel_button']";

        private const string NotYetInvitedCheckbox = "StatusNotYetInvitedCriterion";
        private const string InvitedCheckbox = "StatusInvitedCriterion";
        private const string ActivatedCheckbox = "StatusActivatedCriterion";
        private const string DisabledCheckbox = "StatusDisabledCriterion";

        private string[] UserStatus = { NotYetInvitedCheckbox, InvitedCheckbox, InvitedCheckbox, ActivatedCheckbox, DisabledCheckbox};

        [FindsBy(How = How.Name, Using = "PreferredName")]
        private IWebElement _pname;

        [FindsBy(How = How.Name, Using = "Forename")]
        private IWebElement _forename;

        [FindsBy(How = How.Name, Using = "Surname")]
        private IWebElement _surname;

        [FindsBy(How = How.CssSelector, Using = SearchButton)]
        private IWebElement _searchButtonElement;

        [FindsBy(How = How.CssSelector, Using = SearchCriteriaTextbox)]
        private IWebElement _nameElement;

        WebDriverWait waiter = new WebDriverWait(WebContext.WebDriver, TimeSpan.FromSeconds(10));

        public string TestUserName
        {
            get
            {
                if (string.IsNullOrEmpty(_name))
                    _name = SeSugar.Utilities.GenerateRandomString(5, "User_");
                return _name;
            }
        }

        public ManageUsers()
        {
            WebContext.WebDriver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(10));
            PageFactory.InitElements(WebContext.WebDriver, this);
        }

        public void CheckStatusSearchBox(int status)
        {
            var checkBox = SeleniumHelper.Get(By.Name(UserStatus[status]));
            checkBox.SetCheckBox(true);
        }

        public void ShowMoreStatusFilters()
        {
            var moreFiltersButton = SeleniumHelper.Get(By.CssSelector(MoreFiltersButton));
            moreFiltersButton.Click();
        }

        public void SetAllStatusSearchBoxes(bool on)
        {
            //AutomationSugar.WaitFor(By.CssSelector(SearchButton));

            var checkBox = SeleniumHelper.Get(By.CssSelector("input.checkbox[name=" +  NotYetInvitedCheckbox + "]"));
            checkBox.SetCheckBox(on);
            checkBox = SeleniumHelper.Get(By.CssSelector("input.checkbox[name=" + InvitedCheckbox + "]"));
            checkBox.SetCheckBox(on);
            checkBox = SeleniumHelper.Get(By.CssSelector("input.checkbox[name=" + ActivatedCheckbox + "]"));
            checkBox.SetCheckBox(on);
            checkBox = SeleniumHelper.Get(By.CssSelector("input.checkbox[name=" + DisabledCheckbox + "]"));
            checkBox.SetCheckBox(on);
        }

        public int GetNumberOfSearchResults()
        {
            int result = 0;

            AutomationSugar.WaitFor(By.ClassName(SearchResultCount));
            var resultLabel = SeleniumHelper.Get(By.ClassName(SearchResultCount));

            var resultText = resultLabel.Text;
            resultText = resultText.Replace("Matches", "").Replace("Match", "").Trim();
            Int32.TryParse(resultText, out result);

            return result;
        }

        //HasAccount checkbox click in search criteria
        public void EnterSearchCriteria(string name)
        {
            AutomationSugar.WaitFor(By.CssSelector(SearchCriteriaTextbox));

            _nameElement.SendKeys(name);
        }

        public void SetName(string foreName, string surName)
        {
            _forename.SendKeys(foreName);
            _surname.SendKeys(surName);
            _pname.SendKeys(foreName);
        }

        public string GetUserAccountMessage()
        {
            return WebContext.WebDriver.FindElement(By.CssSelector("span.inline-alert-message")).Text;
        }

        public void ClickButton(UserScreenElements element)
        {
            string selector = GetSelectorFromElement(element);
            AutomationSugarHelpers.WaitForAndClickOn(By.CssSelector(selector));
            WaitUntillAjaxRequestCompleted();
        }

        public void ClickCreateButton()
        {
            AutomationSugarHelpers.WaitForAndClickOn("add_user_account_button");
            WaitUntillAjaxRequestCompleted();
        }

        public void ClickInviteButton()
        {
            AutomationSugarHelpers.WaitForAndClickOn("invite_button");
            WaitUntillAjaxRequestCompleted();
        }

        public void ClickReInviteButton()
        {
            AutomationSugarHelpers.WaitForAndClickOn("re-invite_button");
            WaitUntillAjaxRequestCompleted();
        }

        public void ClickSendInviteButton()
        {
            AutomationSugarHelpers.WaitForAndClickOn("send_invite_button");
            WaitUntillAjaxRequestCompleted();
        }

        public bool CheckIfElementExists(UserScreenElements element)
        {
            string selector = GetSelectorFromElement(element);
            
            try
            {
                return SeSugar.Environment.WebContext.WebDriver.FindElements(By.CssSelector(selector)).Any();
            }
            catch(NoSuchElementException ex)
            {
                return false;
            }
        }

        public void WaitForElement(UserScreenElements element)
        {
            AutomationSugar.WaitFor(By.CssSelector(GetSelectorFromElement(element)));
        }

        //Save user
        public void SaveRecord()
        {
            AutomationSugarHelpers.WaitForAndClickOn(SaveButton);
            AutomationSugar.WaitForAjaxCompletion();
        }

        public void Search()
        {
            _searchButtonElement.Click();
            AutomationSugar.WaitForAjaxCompletion();
        }

        public void SelectFirstRecord()
        {
            AutomationSugar.ClickOn(SimsBy.CssSelector("[data-section-id='search_results_list']>div:nth-child(1)>div:nth-child(1)>a"));
            AutomationSugar.WaitForAjaxCompletion();
        }

        public void SetInviteEmail(string emailId)
        {
            var element = SeleniumHelper.Get(By.CssSelector("div[data-section-id='dialog-detail']"));
            element.SetText(By.CssSelector("input[name='InviteEmail']"), emailId);
        }

        public string ReadStatusMessage()
        {
            var element = SeleniumHelper.Get(SimsBy.AutomationId("invite-information-detail"));
            return element.Text.Split(' ').First();
        }

        private string GetSelectorFromElement(UserScreenElements element)
        {
            string selector = string.Empty;
            switch (element)
            {
                case UserScreenElements.EnableButton:
                    selector = EnableUserButton;
                    break;
                case UserScreenElements.DisableButton:
                    selector = DisableUserButton;
                    break;
                case UserScreenElements.PopUpEnableButton:
                    selector = PopUpEnableButton;
                    break;
                case UserScreenElements.PopUpDisableButton:
                    selector = PopUpDisableButton;
                    break;
                case UserScreenElements.PopUpCancelButton:
                    selector = PopUpCancelButton;
                    break;
            }

            return selector;
        }
    }
}