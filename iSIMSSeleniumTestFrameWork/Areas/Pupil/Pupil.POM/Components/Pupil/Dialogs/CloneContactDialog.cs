using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using POM.Base;
using POM.Helper;


namespace POM.Components.Pupil
{
    public class CloneContactDialog : BaseComponent
    {
        public override By ComponentIdentifier
        {
            get { return SimsBy.CssSelector("[data-section-id='dialog-detail']"); }
        }

        #region PROPERTIES

        [FindsBy(How = How.Name, Using = "Title.dropdownImitator")]
        private IWebElement _titleDropDown;

        [FindsBy(How = How.Name, Using = "Forename")]
        private IWebElement _forenameTextBox;

        [FindsBy(How = How.Name, Using = "Gender.dropdownImitator")]
        private IWebElement _genderDropDown;

        [FindsBy(How = How.Name, Using = "GenerateSalutation")]
        private IWebElement _generateSalutationButton;

        [FindsBy(How = How.Name, Using = "GenerateAddressee")]
        private IWebElement _generateAddresseeButton;

        public string Title
        {
            set { _titleDropDown.ChooseSelectorOption(value); }
            get { return _titleDropDown.GetValue(); }
        }

        public string Gender
        {
            set { _genderDropDown.ChooseSelectorOption(value); }
            get { return _genderDropDown.GetValue(); }
        }

        public string Forename
        {
            set { _forenameTextBox.SetText(value); }
            get { return _forenameTextBox.GetValue(); }
        }

        #endregion

        #region ACTIONS

        public void GenerateAddressee()
        {
            _generateAddresseeButton.ClickByJS();
            Wait.WaitForAjaxReady(SimsBy.CssSelector(".locking-mask-loading"));
        }

        public void GenerateParentalSalutation()
        {
            _generateSalutationButton.ClickByJS();
            Wait.WaitForAjaxReady(SimsBy.CssSelector(".locking-mask-loading"));
        }

        #endregion

    }
}
