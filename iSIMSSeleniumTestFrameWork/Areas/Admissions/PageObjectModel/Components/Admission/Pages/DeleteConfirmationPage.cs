using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using PageObjectModel.Base;
using PageObjectModel.Helper;


namespace PageObjectModel.Components.Admission
{
    public class DeleteConfirmationPage : BaseComponent
    {
        public override By ComponentIdentifier
        {
            get { return SimsBy.AutomationId("confirm_delete_dialog"); }
        }

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='continue_with_delete_button']")]
        private IWebElement _continueDeleteButton;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='cancel_button']")]
        private IWebElement _cancelButton;

        public void ConfirmDelete()
        {
            if (_continueDeleteButton.IsExist())
            {
                _continueDeleteButton.ClickByJS();
                Wait.WaitForAjaxReady(SimsBy.CssSelector(".locking-mask"));
            }
        }
        public void CancelDelete()
        {
            if (_cancelButton.IsExist())
            {
                _cancelButton.ClickByJS();
                Wait.WaitForAjaxReady(SimsBy.CssSelector(".locking-mask"));
            }
        }
    }
}
