using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using POM.Base;
using POM.Helper;


namespace POM.Components.Pupil
{
    public class AddDoctorDialog : BaseDialogComponent
    {
        public override By DialogIdentifier
        {
            get { return SimsBy.Id("dialog-dialog-palette-editor"); }
        }

        #region Properties

        [FindsBy(How = How.Name, Using = "Title.dropdownImitator")]
        private IWebElement _titleDropdown;

        [FindsBy(How = How.Name, Using = "Forename")]
        private IWebElement _foreNameTextBox;

        [FindsBy(How = How.Name, Using = "MiddleName")]
        private IWebElement _middleNameTextBox;

        [FindsBy(How = How.Name, Using = "Surname")]
        private IWebElement _sureNameTextBox;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='ok_button']")]
        private IWebElement _okButton;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='cancel_button']")]
        private IWebElement _cancelButton;

        #endregion

        #region Actions

        public string Title
        {
            set { _titleDropdown.EnterForDropDown(value); }
            get { return _titleDropdown.GetValue(); }
        }
        public string ForeName
        {
            set { _foreNameTextBox.SetText(value); }
            get { return _foreNameTextBox.GetValue(); }
        }

        public string MiddleName
        {
            set { _middleNameTextBox.SetText(value); }
            get { return _middleNameTextBox.GetValue(); }
        }

        public string SureName
        {
            set { _sureNameTextBox.SetText(value); }
            get { return _sureNameTextBox.GetValue(); }
        }

        public MedicalPracticeDialog OK()
        {
            if (_okButton.IsExist())
            {
                _okButton.ClickByJS();
                Wait.WaitForAjaxReady(SimsBy.CssSelector(".locking-mask"));
                SeleniumHelper.Sleep(3);
            }
            return new MedicalPracticeDialog();
            Refresh();
        }

        public EditMedicalPracticeDialog ClickOK()
        {
            if (_okButton.IsExist())
            {
                _okButton.ClickByJS();
                Wait.WaitForAjaxReady(SimsBy.CssSelector(".locking-mask"));
                SeleniumHelper.Sleep(3);
            }
            return new EditMedicalPracticeDialog();
            Refresh();
        }

        public MedicalPracticeDialog Cancel()
        {
            if (_cancelButton.IsExist())
            {
                _cancelButton.ClickByJS();
                Wait.WaitForAjaxReady(SimsBy.CssSelector(".locking-mask"));
            }
            return new MedicalPracticeDialog();
            Refresh();
        }
        #endregion
    }
}
