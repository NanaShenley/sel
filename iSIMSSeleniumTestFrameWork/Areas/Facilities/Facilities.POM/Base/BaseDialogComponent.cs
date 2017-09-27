
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using POM.Helper;


namespace POM.Base
{
    public abstract class BaseDialogComponent : BaseComponent
    {
        public override By ComponentIdentifier
        {
            get { return DialogIdentifier; }
        }

        public abstract By DialogIdentifier { get; }

        #region Buttons
        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='ok_button']")]
        private IWebElement _okButton;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='cancel_button']")]
        private IWebElement _cancelButton;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='continue_button']")]
        private IWebElement _continueButton;


        public virtual void ClickOk(int sleep = 2)
        {
            _okButton.ClickByJS();
            Wait.WaitForAjaxReady(By.CssSelector(".locking-mask"));

            Wait.WaitForDocumentReady();
            SeleniumHelper.Sleep(sleep);
        }

        public T ClickOK<T>() where T : BaseComponent, new()
        {
            Retry.Do(SeleniumHelper.Get(SimsBy.AutomationId("ok_button")).ClickByJS);
            Wait.WaitForAjaxReady(By.CssSelector(".locking-mask"));
            Wait.WaitLoading();

            var page = default(T);
            Retry.Do(() =>
            {
                page = new T();
            });
            page.Refresh();
            return page;
        }

        public void ClickCancel()
        {
            _cancelButton.ClickByJS();
        }

        public void ClickContinue()
        {
            _continueButton.Click();
            Wait.WaitForAjaxReady(By.CssSelector(".locking-mask"));
        }
        #endregion
    }
}
