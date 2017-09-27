using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using PageObjectModel.Base;
using PageObjectModel.Helper;


namespace PageObjectModel.Components.Admission
{
    public class ConfirmRequiredAddNewApplicantDialog : BaseDialogComponent
    {
        public override By DialogIdentifier
        {
            get { return SimsBy.AutomationId("add_new_pupil_wizard"); }
        }

        #region Properties



        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='yes,_update_any_associated_applications_button']")]
        private IWebElement _yesUpdateanyassociatedapplicationsButton;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='yes,_save_without_any_admission_groups_defined_button']")]
        private IWebElement _yesSaveWithoutAnyAdmissionGroupDefined;


        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='cancel_button']")]
        private IWebElement _cancelButton;

        #endregion

        #region Actions

        public SchoolIntakePage ConfirmAdNewAdmission()
        {
            if (_yesUpdateanyassociatedapplicationsButton.IsExist())
            {
                _yesUpdateanyassociatedapplicationsButton.ClickByJS();
                Wait.WaitForAjaxReady(SimsBy.CssSelector(".locking-mask"));
            }
            return new SchoolIntakePage();
            Refresh();
        }

        public SchoolIntakePage ConfirmSaveWithoutAnyAdmissionGroupDefined()
        {
            if (_yesSaveWithoutAnyAdmissionGroupDefined.IsExist())
            {
                _yesSaveWithoutAnyAdmissionGroupDefined.ClickByJS();
                Wait.WaitForAjaxReady(SimsBy.CssSelector(".locking-mask"));
            }
            return new SchoolIntakePage();
            Refresh();
        }

        public SchoolIntakePage CancelProcessAddNewAdmission()
        {
            if (_cancelButton.IsExist())
            {
                _cancelButton.ClickByJS();
                Wait.WaitForAjaxReady(SimsBy.CssSelector(".locking-mask"));
            }
            return new SchoolIntakePage();
            Refresh();
        }



        #endregion
    }
}
