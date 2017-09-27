using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using POM.Base;
using POM.Helper;


namespace POM.Components.Pupil
{
    public class NoteDialog : BaseDialogComponent
    {
        public override By DialogIdentifier
        {
            // luong.mai missing data-automation-id
            get { return SimsBy.Id("dialog-palette-editor"); }
        }

        #region Page properties

        [FindsBy(How = How.Name, Using = "NoteText")]
        private IWebElement _noteTextBox;

        [FindsBy(How = How.Name, Using = "Title")]
        private IWebElement _titleTextBox;

        [FindsBy(How = How.Name, Using = "SubCategorySelector.dropdownImitator")]
        private IWebElement _subCategoryDropDown;

        [FindsBy(How = How.Name, Using = "Pinned")]
        private IWebElement _pinThisNoteCheckBox;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='save_button']")]
        private IWebElement _saveButton;

        public string Note
        {
            set { _noteTextBox.SetText(value); }
            get { return _noteTextBox.GetValue(); }
        }

        public string Title
        {
            set { _titleTextBox.SetText(value); }
            get { return _titleTextBox.GetValue(); }
        }

        public string SubCategory
        {
            set { _subCategoryDropDown.ChooseSelectorOption(value); }
            get { return _subCategoryDropDown.GetValue(); }
        }

        public bool PinThisNote
        {
            set { _pinThisNoteCheckBox.Set(value); }
            get { return _pinThisNoteCheckBox.IsChecked(); }
        }

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
