using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using POM.Base;
using POM.Helper;


namespace POM.Components.Pupil
{
    public class AddSenStatementDialog : BaseDialogComponent
    {
        public override By DialogIdentifier
        {
            get { return SimsBy.AutomationId("sen_statement_record_detail"); }
        }

        #region Page properties

        [FindsBy(How = How.Name, Using = "DateRequestedforAssessment")]
        private IWebElement _dateRequestedTextBox;

        [FindsBy(How = How.Name, Using = "ParentConsultationDate")]
        private IWebElement _dateConsultedTextBox;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='select_button']")]
        private IWebElement _selectOfficerButton;

        [FindsBy(How = How.Name, Using = "SENStatutoryAssessment.dropdownImitator")]
        private IWebElement _elbResponseDropdown;

        [FindsBy(How = How.Name, Using = "SENStatutoryStatement.dropdownImitator")]
        private IWebElement _statemenOutComeDropdown;

        [FindsBy(How = How.Name, Using = "StatementDateFinalised")]
        private IWebElement _dateFinalisedTextBox;

        [FindsBy(How = How.Name, Using = "StatementDateCeased")]
        private IWebElement _dateCeasedTextBox;

        public string DateRequested
        {
            set { _dateRequestedTextBox.SetDateTime(value); }
            get { return _dateRequestedTextBox.GetDateTime(); }
        }

        public string DateConsulted
        {
            set { _dateConsultedTextBox.SetDateTime(value); }
            get { return _dateConsultedTextBox.GetDateTime(); }
        }

        public string ELBReponse
        {
            set
            {
                _elbResponseDropdown.EnterForDropDown(value);
                Wait.WaitForAjaxReady(By.CssSelector(".locking-mask-loading"));
                Refresh();
            }
            get { return _elbResponseDropdown.GetValue(); }
        }

        public string StatementOutcome
        {
            set
            {
                _statemenOutComeDropdown.EnterForDropDown(value);
                Wait.WaitForAjaxReady(By.CssSelector(".locking-mask-loading"));
                Refresh();
            }
            get { return _statemenOutComeDropdown.GetValue(); }
        }

        public string DateFinalised
        {
            set { _dateFinalisedTextBox.SetDateTime(value); }
            get { return _dateFinalisedTextBox.GetDateTime(); }
        }

        public string DateCeased
        {
            set { _dateCeasedTextBox.SetDateTime(value); }
            get { return _dateCeasedTextBox.GetDateTime(); }
        }

        #endregion

        #region Public actions

        public SelectOfficerDialog ClickSelectOfficer()
        {
            _selectOfficerButton.Click();
            Wait.WaitForAjaxReady(By.CssSelector(".locking-mask"));
            return new SelectOfficerDialog();
        }
        #endregion
    }
}
