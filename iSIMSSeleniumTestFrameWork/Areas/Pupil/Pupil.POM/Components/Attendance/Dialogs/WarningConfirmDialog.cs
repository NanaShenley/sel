using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using POM.Base;
using POM.Helper;


namespace POM.Components.Attendance
{
    public class WarningConfirmDialog : BaseDialogComponent
    {
        public override By DialogIdentifier
        {
            get { return SimsBy.CssSelector("[data-section-id='custom-confirm-dialog']"); }
        }

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='continue_with_delete_button']")]
        private IWebElement _continueWithDeleteButton;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='cancel_button']")]
        private IWebElement _cancelButton;

        public void ClickContinueDelete()
        {
            _continueWithDeleteButton.Click();
            Wait.WaitForAjaxReady(By.CssSelector(".locking-mask"));
        }
    }
}
