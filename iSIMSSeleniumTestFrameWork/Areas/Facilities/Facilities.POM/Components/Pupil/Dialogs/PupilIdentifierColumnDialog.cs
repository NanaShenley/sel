using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using POM.Base;
using POM.Helper;


namespace POM.Components.Pupil
{
    public class PupilIdentifierColumnDialog : BaseDialogComponent
    {
        public override By DialogIdentifier
        {
            get { return SimsBy.AutomationId("bulk_update_pupil_identifiers_columns_dialog_content"); }
        }

        #region Page properties

        [FindsBy(How = How.Id, Using = "identifiers-ok-button-id")]
        private IWebElement _okButton;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='dialog_identifier_columns_treenode_Learner.Gender.Description']")]
        private IWebElement _genderCheckbox;

        public bool Gender
        {
            set { _genderCheckbox.Set(value); }
        }

        #endregion

        #region Public actions

        public BulkUpdatePage ClickOK()
        {
            _okButton.Click();
            Wait.WaitForAjaxReady(SimsBy.CssSelector(".locking-mask"));
            return new BulkUpdatePage();
        }

        #endregion
    }
}
