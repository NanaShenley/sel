using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using POM.Base;
using POM.Helper;


namespace POM.Components.Pupil
{
    public class ContactDetailsDialog : BaseDialogComponent
    {
        public override By DialogIdentifier
        {
            get { return SimsBy.CssSelector("[role = 'tooltip']"); }
        }

        #region Page properties

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='pupil_record_contacts_details_button']")]
        private IWebElement _contactDetailsLink;

        [FindsBy(How = How.CssSelector, Using = ".popover-title")]
        private IWebElement _titleLabel;

        #endregion

        #region Public actions

        public PupilRecordPage ClickContactDetailsLink()
        {
            _contactDetailsLink.ClickByJS();
            Wait.WaitForAjaxReady(By.Id("nprogress"));

            return new PupilRecordPage();
        }

        public bool IsContactDetailsPopupDisplayed()
        {
            if (_titleLabel.IsExist())
            {
                return true;
            }

            return false;
        }

        #endregion
    }
}
