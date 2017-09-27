using OpenQA.Selenium;
using POM.Base;
using POM.Helper;


namespace POM.Components.Staff
{
    public class WarningSaveDataDialog : BaseDialogComponent
    {
        public override By DialogIdentifier
        {
            get { return SimsBy.CssSelector("[data-section-id='confirm-dialog']"); }
        }

        #region Page properties

        #endregion

        #region Page action

        public void Continue()
        {
            IWebElement _continueButton = null;
            bool isExisted = true;
            try
            {
                _continueButton = SeleniumHelper.FindElement(By.CssSelector("[data-section-id='confirm-Continue']"));
            }
            catch (NoSuchElementException)
            {
                isExisted = false;
            }

            if (isExisted == true)
            {
                _continueButton.Click();
                Wait.WaitForAjaxReady(By.CssSelector(".locking-mask"));
            }

        }

        public static WarningSaveDataDialog Create()
        {
            return new WarningSaveDataDialog();
        }
        #endregion

    }
}
