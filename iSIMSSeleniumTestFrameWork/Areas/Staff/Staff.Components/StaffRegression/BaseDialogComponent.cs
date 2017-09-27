using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using System.Threading;
using SharedComponents.Helpers;

namespace Staff.Components.StaffRegression
{
    public abstract class BaseDialogComponent : BaseComponent
    {
        public override By ComponentIdentifier
        {
            get { return DialogIdentifier; }
        }

        public abstract By DialogIdentifier { get; }

        #region Buttons
        [FindsBy(How = How.CssSelector, Using = "[data-automation-id=\"ok_button\"]")]
        private IWebElement _okButton;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id=\"cancel_button\"]")]
        private IWebElement _cancelButton;

        public void ClickOk()
        {
            Retry.Do(_okButton.Click);
            Thread.Sleep(2000);
        }

        public void ClickCancel()
        {
            Retry.Do(_cancelButton.Click);
        }
        #endregion
    }
}
