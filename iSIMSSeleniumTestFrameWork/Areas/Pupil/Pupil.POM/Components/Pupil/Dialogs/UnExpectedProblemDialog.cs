using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using POM.Base;
using POM.Helper;


namespace POM.Components.Pupil
{
    public class UnExpectedProblemDialog : BaseDialogComponent
    {
        public override By DialogIdentifier
        {
            //data-section-id='error-dialog'
            get { return SimsBy.CssSelector("[data-section-id='error-dialog']"); }
        }

        #region Page properties

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='dismiss_button']")]
        private IWebElement dissmissButton;

        [FindsBy(How = How.CssSelector, Using = "div.error-message-details button")]
        private IWebElement _showDetailsButton;

        #endregion

        #region Page action

        public void Dismiss()
        {
            if (SeleniumHelper.Get(By.CssSelector("[data-automation-id='dismiss_button']")).Displayed)
            {
                dissmissButton.Click();
                Wait.WaitForAjaxReady(By.CssSelector(".locking-mask"));
            }
        }

        public void ShowDetails()
        {
            if (SeleniumHelper.Get(By.CssSelector("div.error-message-details button")).GetAttribute("aria-expanded") != "true")
            {
                _showDetailsButton.Click();
            }
        }

        public static bool DoesExist()
        {
            try
            {
                Wait.WaitUntilDisplayed(By.CssSelector("[data-section-id='error-dialog']"));

                return true;
            }
            catch (WebDriverTimeoutException)
            {
                return false;
            }                      
        }

        public static UnExpectedProblemDialog Create()
        {
            return new UnExpectedProblemDialog();
        }
        #endregion

    }
}
