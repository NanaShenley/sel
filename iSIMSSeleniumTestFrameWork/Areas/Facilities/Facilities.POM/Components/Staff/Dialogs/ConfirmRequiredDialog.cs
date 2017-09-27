using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using POM.Base;
using POM.Helper;


namespace POM.Components.Staff
{
    public class ConfirmRequiredDialog : BaseDialogComponent
    {
        public override By DialogIdentifier
        {
            get { return SimsBy.AutomationId("confirmation_required_dialog"); }
        }

        #region Properties

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='ok_button']")]
        private IWebElement _OkButton;

        #endregion

        #region Actions

        public StaffRecordPage ClickOk()
        {
            _OkButton.Click();
            Wait.WaitForAjaxReady(SimsBy.CssSelector(".locking-mask"));
            Wait.WaitForAjaxReady(By.Id("nprogress"));
            try
            {
                if (SeleniumHelper.FindElement(SimsBy.CssSelector("[data-section-id='detail']")).IsElementDisplayed())
                {
                    return new StaffRecordPage();
                }
            }
            catch (NoSuchElementException e)
            {
                return null;
            }
            return null;
        }

        #endregion
    }
}
