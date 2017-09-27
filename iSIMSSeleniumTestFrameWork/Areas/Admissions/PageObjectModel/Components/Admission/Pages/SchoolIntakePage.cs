using System;
using System.Reflection;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using PageObjectModel.Base;
using PageObjectModel.Helper;



namespace PageObjectModel.Components.Admission
{
    public class SchoolIntakePage : BaseComponent
    {
        public override By ComponentIdentifier
        {
            get { return SimsBy.CssSelector("[data-section-id='detail']"); }
        }

        #region Page properties

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='well_know_action_save']")]
        private IWebElement _saveButton;

        [FindsBy(How = How.Name, Using = "YearOfAdmission.dropdownImitator")]
        private IWebElement _admissionYearTextBox;

        [FindsBy(How = How.Name, Using = "AdmissionTerm.dropdownImitator")]
        private IWebElement _admissionTermTextBox;

        [FindsBy(How = How.Name, Using = "YearGroup.dropdownImitator")]
        private IWebElement _yearGroupTextBox;

        [FindsBy(How = How.Name, Using = "PlannedAdmissionNumber")]
        private IWebElement _numberOfPlannedAdmissions;

        [FindsBy(How = How.Name, Using = "IntakeName")]
        private IWebElement _nameTextBox;

        [FindsBy(How = How.Name, Using = "IsActive")]
        private IWebElement _activeCheckBox;

        [FindsBy(How = How.CssSelector, Using = "[data-maintenance-container='AdmissionGroups']")]
        private IWebElement _admissionGroupTable;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='status_success']")]
        private IWebElement _statusMessage;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='delete_button']")]
        private IWebElement _deleteButton;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='add_admission_group_button']")]
        private IWebElement _addAdmissionGroupButton;

        // Define Admission Table
        public GridComponent<AdmissionGroups> AdmissionGrid
        {
            get
            {
                GridComponent<AdmissionGroups> returnValue = null;
                Retry.Do(() =>
                {
                    returnValue = new GridComponent<AdmissionGroups>(By.CssSelector("[data-maintenance-container='AdmissionGroups']"), ComponentIdentifier);
                });
                return returnValue;
            }
        }

        public class AdmissionGroups
        {
            [FindsBy(How = How.CssSelector, Using = "[name$='Name']")]
            private IWebElement _nameTextBox;

            [FindsBy(How = How.CssSelector, Using = "[name$='DateOfAdmission']")]
            private IWebElement _dateOfAmissionTextBox;

            [FindsBy(How = How.CssSelector, Using = "[name$='Capacity']")]
            private IWebElement _capacityTextBox;

            [FindsBy(How = How.CssSelector, Using = "[name$='IsActive']")]
            private IWebElement _activeCheckBox;

            public string Name
            {
                set { _nameTextBox.SetText(value); }
                get { return _nameTextBox.GetValue(); }
            }

            public string DateOfAdmission
            {
                set { _dateOfAmissionTextBox.SetDateTime(value); }
                get { return _dateOfAmissionTextBox.GetDateTime(); }
            }

            public string Capacity
            {
                set { _capacityTextBox.SetText(value); }
                get { return _capacityTextBox.GetValue(); }
            }

            public bool Active
            {
                set { _activeCheckBox.Set(value); }
                get { return _activeCheckBox.IsChecked(); }
            }
        }

        public string AdmissionYear
        {
            set
            {
                _admissionYearTextBox.EnterForDropDown(value);
                Wait.WaitForAjaxReady(By.CssSelector(".locking-mask-loading"));
            }

            get { return _admissionYearTextBox.GetValue(); }
        }

        public string AdmissionTerm
        {
            set
            {
                _admissionTermTextBox.EnterForDropDown(value);
                Wait.WaitForAjaxReady(By.CssSelector(".locking-mask-loading"));
            }
            get { return _admissionTermTextBox.GetValue(); }
        }

        public string YearGroup
        {
            set
            {
                _yearGroupTextBox.EnterForDropDown(value);
                Wait.WaitForAjaxReady(By.CssSelector(".locking-mask-loading"));
            }
            get { return _yearGroupTextBox.GetValue(); }
        }

        public string NumberOfPlannedAdmissions
        {
            set
            {
                _numberOfPlannedAdmissions.SetText(value);
                Wait.WaitForAjaxReady(By.CssSelector(".locking-mask-loading"));
            }
            get { return _numberOfPlannedAdmissions.GetValue(); }
        }

        public string Name
        {
            set
            {
                _nameTextBox.SetText(value);
                Wait.WaitForAjaxReady(By.CssSelector(".locking-mask-loading"));
            }
            get { return _nameTextBox.GetValue(); }
        }

        public bool Active
        {
            set
            {
                _activeCheckBox.Set(value);
                Wait.WaitForAjaxReady(By.CssSelector(".locking-mask-loading"));
            }
            get { return _activeCheckBox.IsChecked(); }
        }

        #endregion Page properties

        #region Actions

        public DeleteConfirmationPage ClickDelete()
        {
            if (_deleteButton.IsExist())
            {
                _deleteButton.ClickByJS();
                Wait.WaitForAjaxReady(SimsBy.CssSelector(".locking-mask"));
            }
            return new DeleteConfirmationPage();
        }

        public ConfirmRequiredSchoolIntakeDialog ClickSave()
        {
            Action clickAction = () => _saveButton.Click();
            ClickWhenElementIsReady(clickAction, By.CssSelector("[data-automation-id = 'well_know_action_save']"));
            return new ConfirmRequiredSchoolIntakeDialog();
        }

        public void ClickSaveWithNoConfirmation()
        {
            Action clickAction = () => _saveButton.Click();
            ClickWhenElementIsReady(clickAction, By.CssSelector("[data-automation-id = 'well_know_action_save']"));
        }

        public void SaveUpdate()
        {
            _saveButton.Click();
            Wait.WaitForAjaxReady(SimsBy.CssSelector(".locking-mask"));
        }

        public bool IsSuccessMessageIsDisplayed()
        {
            Wait.WaitForControl(SimsBy.CssSelector("[data-automation-id='status_success']"));
            return SeleniumHelper.DoesWebElementExist(SimsBy.CssSelector("[data-automation-id='status_success']"));
        }

        public void ClickOnAddNewAdmissionGroupButton()
        {
            Action clickAction = () => _addAdmissionGroupButton.Click();
            ClickWhenElementIsReady(clickAction, By.CssSelector("[data-automation-id='add_admission_group_button']"));
        }

        private void ClickWhenElementIsReady(Action action, By by)
        {
            try
            {
                action();
            }
            catch (TargetInvocationException ex)
            {
                Console.WriteLine("Element is noy yet ready to be clicked. Please wait and try again later.", ex);
                Wait.WaitUntilEnabled(by);
                Wait.WaitForDocumentReady();
                action();
            }
            finally
            {
                Wait.WaitForAjaxReady(SimsBy.CssSelector(".locking-mask"));
            }
        }

        #endregion Actions
    }
}