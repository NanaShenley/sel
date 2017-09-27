using System.Threading;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using POM.Base;
using POM.Helper;

namespace POM.Components.Pupil
{
    public class NameChangeHistoryPage : BaseComponent
    {
        public override By ComponentIdentifier
        {
            get { return SimsBy.Id("screen"); }
        }

        #region Properties

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='well_know_action_save']")]
        private IWebElement _saveButton;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='add_previous_legal_name_button']")]
        private IWebElement _addPreviousLegalNameButtton;

        public class NameHistory : GridRow
        {
            [FindsBy(How = How.CssSelector, Using = "[name$='LegalForename']")]
            private IWebElement _legalForeName;

            [FindsBy(How = How.CssSelector, Using = "[name$='LegalMiddleNames']")]
            private IWebElement _legalMiddleName;

            [FindsBy(How = How.CssSelector, Using = "[name$='LegalSurname']")]
            private IWebElement _legalSurname;

            [FindsBy(How = How.CssSelector, Using = "[name$='ReasonForChange.dropdownImitator']")]
            private IWebElement _reason;

            [FindsBy(How = How.CssSelector, Using = "[name$='ChangeDate']")]
            private IWebElement _dateOfChange;

            public string LegalForeName
            {
                set { _legalForeName.SetText(value); }
                get { return _legalForeName.GetAttribute("value"); }
            }

            public string LegalMiddleName
            {
                set { _legalMiddleName.SetText(value); }
                get { return _legalMiddleName.GetAttribute("value"); }
            }

            public string LegalSurName
            {
                set { _legalSurname.SetText(value); }
                get { return _legalSurname.GetAttribute("value"); }
            }

            public string Reason
            {
                set { _reason.EnterForDropDown(value); }
                get { return _reason.GetAttribute("value"); }
            }

            public string DateOfChange
            {
                set { _dateOfChange.SetDateTime(value); }
                get { return _dateOfChange.GetDateTime(); }
            }
        }

        public GridComponent<NameHistory> NameHistoryTable
        {
            get
            {
                GridComponent<NameHistory> returnValue = null;
                Retry.Do(() =>
                {
                    returnValue = new GridComponent<NameHistory>(By.CssSelector("[data-maintenance-container='LearnerPreviousNames']"), ComponentIdentifier);
                });
                return returnValue;
            }
        }

        #endregion Properties

        #region Actions

        public void Save()
        {
            _saveButton.Click();
            Wait.WaitForAjaxReady(By.CssSelector(".locking-mask"));
            Refresh();
        }

        /// <summary>
        /// Au : Hieu Pham
        /// Check Name change history page display for pupil name.
        /// </summary>
        /// <param name="pupilName"></param>
        /// <returns></returns>
        public bool IsNameChangeHistoryForPupilName(string pupilName)
        {
            try
            {
                IWebElement titleElement = SeleniumHelper.FindElement(SimsBy.AutomationId("name_history_header_display_name"));
                if (titleElement.GetText().Equals(pupilName))
                {
                    return true;
                }
                return false;
            }
            catch (NoSuchElementException)
            {
                return false;
            }
        }

        public void DeleteRow(GridRow row)
        {
            if (row != null)
            {
                row.DeleteRow();
                Wait.WaitLoading();
                Save();
            }
        }

        public void AddPreviousLegalName()
        {
            _addPreviousLegalNameButtton.ClickByJS();
            Wait.WaitForAjaxReady(By.CssSelector(".locking-mask-loading"));
        }

        #endregion Actions
    }
}