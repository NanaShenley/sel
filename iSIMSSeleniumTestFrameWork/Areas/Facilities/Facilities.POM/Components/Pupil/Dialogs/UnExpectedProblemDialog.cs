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

        #endregion

        #region Page action

        public PupilRecordPage Dismiss()
        {
            if (SeleniumHelper.Get(By.CssSelector("[data-automation-id='dismiss_button']")).Displayed)
            {
                dissmissButton.Click();
                Wait.WaitForAjaxReady(By.CssSelector(".locking-mask"));
            }
            return new PupilRecordPage();
        }

        public bool DoesExist()
        {
            return (SeleniumHelper.Get(By.CssSelector("[data-section-id='error-dialog']")).GetAttribute("aria-hidden") == "false");
            //return SeleniumHelper.Get(By.CssSelector("[data-section-id='error-dialog']")).Displayed;
        }

        public static UnExpectedProblemDialog Create()
        {
            return new UnExpectedProblemDialog();
        }
        #endregion

    }
}
