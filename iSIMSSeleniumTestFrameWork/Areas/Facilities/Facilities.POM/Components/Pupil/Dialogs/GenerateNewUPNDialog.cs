using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using POM.Base;
using POM.Helper;


namespace POM.Components.Pupil
{
    public class GenerateNewUPNDialog : BaseDialogComponent
    {
        public override By DialogIdentifier
        {
            get { return SimsBy.CssSelector("[data-section-id='generic-confirm-dialog']"); }
        }

        #region Properties

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='yes_button']")]
        private IWebElement _yesButton;

        #endregion

        #region Actions

        public PupilRecordPage ClickYes()
        {
            if (_yesButton.IsElementExists())
            {
                _yesButton.ClickByJS();
                Wait.WaitForAjaxReady(SimsBy.CssSelector(".locking-mask"));
                try
                {
                    if (SeleniumHelper.FindElement(SimsBy.CssSelector("[data-section-id='detail']")).IsElementDisplayed())
                    {
                        return new PupilRecordPage();
                    }
                }
                catch (NoSuchElementException e)
                {
                    return null;
                }
            }
            return new PupilRecordPage();
        }

        #endregion
    }
}
