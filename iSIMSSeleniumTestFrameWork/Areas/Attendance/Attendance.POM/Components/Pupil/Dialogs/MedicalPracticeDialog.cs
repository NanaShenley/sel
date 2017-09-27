using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using POM.Base;
using POM.Helper;


namespace POM.Components.Pupil
{
    public class MedicalPracticeDialog : BaseDialogComponent
    {
        public override By DialogIdentifier
        {
            get { return SimsBy.Id("dialog-editableData"); }
        }

        #region Properties

        [FindsBy(How = How.Name, Using = "Name")]
        private IWebElement _nameTextbox;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='add_doctor_button']")]
        private IWebElement _addDoctorButton;


        public string PracticeName
        {
            set { _nameTextbox.SetText(value); }
        }

        public GridComponent<TelephoneNumbersTable> TelephoneNumbers
        {
            get
            {
                GridComponent<TelephoneNumbersTable> returnValue = null;
                Retry.Do(() =>
                {
                    returnValue = new GridComponent<TelephoneNumbersTable>(By.CssSelector("[data-maintenance-container='MedicalPracticeTelephones']"), DialogIdentifier);
                });
                return returnValue;
            }
        }

        public class TelephoneNumbersTable : GridRow
        {
            [FindsBy(How = How.CssSelector, Using = "[name$=TelephoneNumber]")]
            private IWebElement _telephoneNumbersTextBox;

            [FindsBy(How = How.CssSelector, Using = "[id$='UseForTextMessages']")]
            private IWebElement _aMSCheckBox;

            [FindsBy(How = How.CssSelector, Using = "[id$='__LocationType_dropdownImitator']")]
            private IWebElement _locationDropDown;

            [FindsBy(How = How.CssSelector, Using = "[id$='IsMainTelephone']")]
            private IWebElement _mainNumberCheckBox;

            [FindsBy(How = How.CssSelector, Using = "[data-automation-id='add_note_button']")]
            private IWebElement _noteButton;
            [FindsBy(How = How.CssSelector, Using = ".grid-col-end")]
            private IWebElement _blankCell;

            public string TelephoneNumbers
            {
                set
                {
                    _telephoneNumbersTextBox.SetAttribute("value", "");
                    _telephoneNumbersTextBox.SetText(value);
                    _telephoneNumbersTextBox.SendKeys(Keys.Tab);
                    Wait.WaitForAjaxReady(By.CssSelector(".locking-mask-loading"));
                }
                get
                {
                    return _telephoneNumbersTextBox.GetAttribute("value");
                }
            }
            public bool AMS
            {
                set { _aMSCheckBox.Set(value); }
                get { return _aMSCheckBox.IsChecked(); }
            }

            public string Location
            {
                set { _locationDropDown.EnterForDropDown(value); }
                get { return _locationDropDown.GetValue(); }
            }

            public bool MainNumber
            {
                set { _mainNumberCheckBox.Set(value); }
                get { return _mainNumberCheckBox.IsChecked(); }
            }
        }


        public GridComponent<DoctorsTable> Doctors
        {
            get
            {
                GridComponent<DoctorsTable> returnValue = null;
                Retry.Do(() =>
                {
                    returnValue = new GridComponent<DoctorsTable>(By.CssSelector("[data-maintenance-container='Doctors']"), DialogIdentifier);
                });
                return returnValue;
            }
        }

        public class DoctorsTable : GridRow
        {
            [FindsBy(How = How.CssSelector, Using = "[name$=DoctorsForename]")]
            private IWebElement _nameTextBox;


            [FindsBy(How = How.CssSelector, Using = "[data-automation-id='edit..._button']")]
            private IWebElement _editLink;
            public string Name
            {
                set { _nameTextBox.SetText(value); }
                get { return _nameTextBox.GetValue(); }
            }

        #endregion
        }
        #region ACTION
        public AddDoctorDialog AddDoctor()
        {
            _addDoctorButton.Click();
            Wait.WaitForAjaxReady(SimsBy.CssSelector(".locking-mask"));
            return new AddDoctorDialog();
        }
        #endregion ACTIONS
    }
}