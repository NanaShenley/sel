using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using POM.Base;
using POM.Helper;


namespace POM.Components.Pupil
{
    public class EditMedicalPracticeDialog : BaseDialogComponent
    {
        public override By DialogIdentifier
        {
            get { return SimsBy.Id("dialog-palette-editor"); }
        }

        #region Page properties

        [FindsBy(How = How.Name, Using = "MedicalPractice.Name")]
        private IWebElement _medicalPracticeName;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='ok_button']")]
        private IWebElement _okButton;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='add_doctor_button']")]
        private IWebElement _addDoctorButton;




        public string PracticeName
        {
            set { _medicalPracticeName.SetText(value); }
            get { return _medicalPracticeName.GetValue(); }
        }

        public GridComponent<TelephoneNumbersTableDialog> TelephoneNumbersDialog
        {
            get
            {
                GridComponent<TelephoneNumbersTableDialog> returnValue = null;
                Retry.Do(() =>
                {
                    returnValue = new GridComponent<TelephoneNumbersTableDialog>(By.CssSelector("[data-maintenance-container='MedicalPractice.MedicalPracticeTelephones']"), DialogIdentifier);
                });
                return returnValue;
            }
        }

        public class TelephoneNumbersTableDialog : GridRow
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

        }

        public GridComponent<DoctorsTableDialog> Doctors
        {
            get
            {
                GridComponent<DoctorsTableDialog> returnValue = null;
                Retry.Do(() =>
                {
                    returnValue = new GridComponent<DoctorsTableDialog>(By.CssSelector("[data-maintenance-container='MedicalPractice.Doctors']"), DialogIdentifier);
                });
                return returnValue;
            }
        }
        public class DoctorsTableDialog : GridRow
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


        }
        #endregion



        #region Public actions

        public AddDoctorDialog AddDoctor()
        {
            _addDoctorButton.ClickByJS();
            Wait.WaitForAjaxReady(By.CssSelector(".locking-mask"));
            return new AddDoctorDialog();
        }

        public PupilRecordPage OK()
        {
            _okButton.ClickByJS();
            Wait.WaitForAjaxReady(By.CssSelector(".locking-mask"));
            //Handle later
            SeleniumHelper.Sleep(15);
            return new PupilRecordPage();
        }


        #endregion
    }
}
