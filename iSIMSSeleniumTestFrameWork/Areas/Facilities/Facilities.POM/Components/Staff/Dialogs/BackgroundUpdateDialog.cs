using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using POM.Base;
using POM.Helper;
using System.Threading;

namespace POM.Components.Staff
{
    public class BackgroundUpdateDialog : BaseDialogComponent
    {
        public override By DialogIdentifier
        {
            get { return By.CssSelector("[data-section-id=\"generic-confirm-dialog\"]"); }
        }

        #region Properties

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id=\"ok,_return_me_to_the_staff_record_button\"]")]
        private IWebElement _okButton;

        #endregion

        #region Page Actions

        public void ClickOk()
        {
            Wait.WaitLoading();
            _okButton.Click();
            Wait.WaitLoading();
        }
        #endregion
    }
}
