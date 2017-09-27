using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using POM.Base;
using POM.Helper;


namespace POM.Components.Staff
{
    public class UnExpectedProblemDialog : BaseDialogComponent
    {
        public override By DialogIdentifier
        {
            get { return SimsBy.CssSelector("#fixed-dialog-container"); }
        }

        #region Page properties

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='dismiss_button']")]
        private IWebElement dissmissButton;

        #endregion

        #region Page action

        public StaffRecordPage Dismiss()
        {
            if (SeleniumHelper.Get(By.CssSelector("[data-automation-id='dismiss_button']")).Displayed)
            {
                dissmissButton.Click();
                Wait.WaitForAjaxReady(By.CssSelector(".locking-mask"));
            }
            return new StaffRecordPage();
        }

        public bool DoesExist()
        {
            if (SeleniumHelper.Get(By.CssSelector("#fixed-dialog-container")).Displayed)
            {
                return true;
            }
            return false;
        }
        public static UnExpectedProblemDialog Create()
        {
            return new UnExpectedProblemDialog();
        }
        #endregion

    }
}
