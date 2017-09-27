using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using POM.Base;
using POM.Helper;

namespace POM.Components.Admission
{
    public class ConfirmRequiredChangeStatus : BaseDialogComponent
    {
        public override By DialogIdentifier
        {
            get { return SimsBy.AutomationId("confirmation_required_dialog"); }
        }

        #region Properties
        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='cancel_button']")]
        private IWebElement _noCancelAndReturn;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='save_&_continue_button']")]
        private IWebElement _yesContinueSavingButton;

        #endregion

        #region Actions

        public ApplicationPage CancelChangeStatus()
        {
            if (_noCancelAndReturn.IsElementExists())
            {
                _noCancelAndReturn.ClickByJS();
                Wait.WaitForAjaxReady(SimsBy.CssSelector(".locking-mask"));
            }
            return new ApplicationPage();
        }

        public ApplicationPage ConfirmContinueChangeStatus()
        {
            if (_yesContinueSavingButton.IsElementExists())
            {
                _yesContinueSavingButton.ClickByJS();
                Wait.WaitForAjaxReady(SimsBy.CssSelector(".locking-mask"));
            }
            return new ApplicationPage();
        }




        #endregion
    }
}
