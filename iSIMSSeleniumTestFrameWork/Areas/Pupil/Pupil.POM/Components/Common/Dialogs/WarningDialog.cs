using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using POM.Base;
using POM.Helper;


namespace POM.Components.Common
{
    public class WarningDialog : BaseComponent
    {
        public override By ComponentIdentifier
        {
            get { return SimsBy.CssSelector("[data-section-id='confirm-commit-dialog'] .modal-content"); }
        }

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='save_continue_commit_dialog']")]
        private IWebElement _saveButton;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='ignore_commit_dialog']")]
        private IWebElement _dontSaveButton;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='cancel_commit_dialog']")]
        private IWebElement _cancelButton;


        public void Save()
        {
            _saveButton.Click();
            Wait.WaitForAjaxReady(By.CssSelector(".locking-mask"));
            Wait.WaitForAjaxReady(By.Id("nprogress"));
        }

        public void DontSave()
        {
            _dontSaveButton.Click();
            Wait.WaitForAjaxReady(By.Id("nprogress"));
        }

        public void Cancel()
        {
            _cancelButton.Click();
        }



    }
}
