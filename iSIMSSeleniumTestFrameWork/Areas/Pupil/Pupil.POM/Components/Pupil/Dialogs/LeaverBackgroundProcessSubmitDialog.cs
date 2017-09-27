using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using POM.Base;
using POM.Helper;


namespace POM.Components.Pupil
{
    public class LeaverBackgroundProcessSubmitDialog : BaseDialogComponent
    {
        // luong.mai missing data-automation-id
        public override By DialogIdentifier
        {
            get { return SimsBy.CssSelector("[data-automation-id='leaver_background_process_submitted_dialog']"); }
        }

        #region Properties

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='ok,_return_me_to_the_pupil_record_button']")]
        private IWebElement _okButton;

        #endregion

        #region Actions

        public void ClickOk()
        {
            _okButton.ClickByJS();
            Wait.WaitForAjaxReady(SimsBy.CssSelector(".locking-mask"));
            Wait.WaitForAjaxReady(By.Id("nprogress"));
        }

        #endregion
    }
}
