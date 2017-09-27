using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using POM.Base;
using POM.Helper;


namespace POM.Components.Pupil
{
    public class MedicalDetailsDialog : BaseDialogComponent
    {
        public override By DialogIdentifier
        {
            get { return SimsBy.CssSelector("[role = 'tooltip']"); }
        }

        #region Page properties

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='pupil_record_medical_details_button']")]
        private IWebElement _medicalDetailsLink;

        [FindsBy(How = How.CssSelector, Using = ".popover-title")]
        private IWebElement _titleLabel;

        #endregion

        #region Public actions

        public PupilRecordPage ClickMedicalDetailsLink()
        {
            _medicalDetailsLink.ClickByJS();
            Wait.WaitForAjaxReady(By.Id("nprogress"));

            return new PupilRecordPage();
        }

        public bool IsMedicalDetailsPopupDisplayed()
        {
            return _titleLabel.IsExist();
        }

        #endregion
    }
}
