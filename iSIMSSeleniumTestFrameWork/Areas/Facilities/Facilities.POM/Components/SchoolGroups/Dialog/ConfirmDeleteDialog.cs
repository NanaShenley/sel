using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using POM.Base;
using POM.Helper;

using System;


namespace POM.Components.SchoolGroups
{
    public class ConfirmDeleteDialog : BaseDialogComponent
    {
        public override By DialogIdentifier
        {
            get { return SimsBy.AutomationId("confirm_delete_dialog"); }
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

        public static void ContinueDelete()
        {
            try
            {
                IWebElement continueButton = SeleniumHelper.FindElement(SimsBy.AutomationId("continue_with_delete_button"));
                continueButton.Click();
                Wait.WaitForAjaxReady(By.CssSelector(".locking-mask"));
            }
            catch(Exception e)
            {}
        }
    }
}
