using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using POM.Base;
using POM.Helper;

namespace POM.Components.Pupil
{
    public class DoNotDiscloseConfirmDialog : BaseDialogComponent
    {
        public override By DialogIdentifier => SimsBy.CssSelector("[data-section-id='generic-confirm-dialog']");

        #region Page properties
        
        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='save_button']")]
        private IWebElement _saveButton;
        
        #endregion

        #region Public actions

        public void ClickSave()
        {
            _saveButton.ClickByJS();
            Wait.WaitForAjaxReady(SimsBy.CssSelector(".locking-mask"));
            Wait.WaitLoading();
        }

        #endregion
    }
}
