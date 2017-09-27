using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using PageObjectModel.Base;
using PageObjectModel.Helper;

namespace PageObjectModel.Components.Admission
{
    public class ContactDialog : BaseDialogComponent
    {
        public override By DialogIdentifier
        {
            get { return SimsBy.AutomationId("pupil_contact_record_detail"); }
        }

        #region Properties

        [FindsBy(How = How.Name, Using = "Forename")]
        private IWebElement _foreNameTextBox;

        [FindsBy(How = How.Name, Using = "MiddleName")]
        private IWebElement _middleNameTextBox;

        [FindsBy(How = How.Name, Using = "Surname")]
        private IWebElement _sureNameTextBox;

        [FindsBy(How = How.Name, Using = "Gender.dropdownImitator")]
        private IWebElement _genderTextBox;



        #endregion

        #region Actions

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
        public string Gender
        {
            set { _genderTextBox.EnterForDropDown(value); }
            get { return _sureNameTextBox.GetValue(); }
        }


        #endregion
    }
}
