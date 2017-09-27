using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using Staff.POM.Base;
using Staff.POM.Helper;
using SeSugar.Automation;


namespace Staff.POM.Components.Staff
{
    public class StaffAbsenceDialog : BaseDialogComponent
    {
        public override By DialogIdentifier
        {
            get { return SimsBy.AutomationId("staff_absence_dialog"); }
        }

        #region Page properties

        [FindsBy(How = How.Name, Using = "StartDate")]
        private IWebElement _startDate;

        [FindsBy(How = How.Name, Using = "EndDate")]
        private IWebElement _endDate;

        [FindsBy(How = How.Name, Using = "ExpectedReturnDate")]
        private IWebElement _expectedReturnDate;

        [FindsBy(How = How.Name, Using = "ActualReturnDate")]
        private IWebElement _actualReturnDate;

        [FindsBy(How = How.Name, Using = "WorkingDaysLost")]
        private IWebElement _workingDaysLost;

        [FindsBy(How = How.Name, Using = "WorkingHoursLost")]
        private IWebElement _workingHoursLost;

        [FindsBy(How = How.Name, Using = "Notes")]
        private IWebElement _notes;

        [FindsBy(How = How.Name, Using = "AbsenceType.dropdownImitator")]
        private IWebElement _absenceType;

        [FindsBy(How = How.Name, Using = "IllnessCategory.dropdownImitator")]
        private IWebElement _illnessCategory;

        [FindsBy(How = How.Name, Using = "AbsencePayRate.dropdownImitator")]
        private IWebElement _absencePayRate;

        [FindsBy(How = How.Name, Using = "PayrollAbsenceCategory.dropdownImitator")]
        private IWebElement _payrollAbsenceCategory;

        [FindsBy(How = How.Name, Using = "AnnualLeave")]
        private IWebElement _annualLeave;

        [FindsBy(How = How.Name, Using = "IndustrialInjury")]
        private IWebElement _industrialInjury;

        [FindsBy(How = How.Name, Using = "SSPExclusion")]
        private IWebElement _sspExclusion;

        #endregion

        #region Page action

        public string StartDate
        {
            set { _startDate.SetText(value); }
            get { return _startDate.GetValue(); }
        }

        public string EndDate
        {
            set { _endDate.SetText(value); }
            get { return _endDate.GetValue(); }
        }

        public string ExpectedReturnDate
        {
            set { _expectedReturnDate.SetText(value); }
            get { return _expectedReturnDate.GetValue(); }
        }

        public string ActualReturnDate
        {
            set { _actualReturnDate.SetText(value); }
            get { return _actualReturnDate.GetValue(); }
        }

        public string WorkingDaysLost
        {
            set { _workingDaysLost.SetText(value); }
            get { return _workingDaysLost.GetValue(); }
        }

        public string WorkingHoursLost
        {
            set { _workingHoursLost.SetText(value); }
            get { return _workingHoursLost.GetValue(); }
        }

        public string Notes
        {
            set { _notes.SetText(value); }
            get { return _notes.GetValue(); }
        }

        public string AbsenceType
        {
            set { _absenceType.EnterForDropDown(value); }
            get { return _absenceType.GetValue(); }
        }

        public string IllnessCategory
        {
            set { _illnessCategory.EnterForDropDown(value); }
            get { return _illnessCategory.GetValue(); }
        }

        public string AbsencePayRate
        {
            set { _absencePayRate.EnterForDropDown(value); }
            get { return _absencePayRate.GetValue(); }
        }

        public string PayrollAbsenceCategory
        {
            set { _payrollAbsenceCategory.EnterForDropDown(value); }
            get { return _payrollAbsenceCategory.GetValue(); }
        }

        public bool AnnualLeave
        {
            set { _annualLeave.Set(value); }
            get { return _annualLeave.IsChecked(); }
        }

        public bool IndustrialInjury
        {
            set { _industrialInjury.Set(value); }
            get { return _industrialInjury.IsChecked(); }
        }

        public bool SSPExclusion
        {
            set { _sspExclusion.Set(value); }
            get { return _sspExclusion.IsChecked(); }
        }

        public void ClickAddSelfCertification()
        {
            AutomationSugar.WaitFor("add_self_certification_button");
            AutomationSugar.ClickOn("add_self_certification_button");
            AutomationSugar.WaitForAjaxCompletion();
        }

        public void ClickAddDoctorCertification()
        {
            AutomationSugar.WaitFor("add_doctor_certification_button");
            AutomationSugar.ClickOn("add_doctor_certification_button");
            AutomationSugar.WaitForAjaxCompletion();
        }

        #endregion

        public GridComponent<AbsenceCertificateRow> AbsenceCertificates
        {
            get
            {
                return new GridComponent<AbsenceCertificateRow>(By.CssSelector("[data-maintenance-container='StaffAbsenceCertificates']"), ComponentIdentifier);
            }
        }

        public class AbsenceCertificateRow : GridRow
        {
            [FindsBy(How = How.CssSelector, Using = "[name$='DateReceived']")]
            private IWebElement _dateReceived;

            [FindsBy(How = How.CssSelector, Using = "[name$='DateSigned']")]
            private IWebElement _dateSigned;

            [FindsBy(How = How.CssSelector, Using = "[name$='SignatoryType.dropdownImitator']")]
            private IWebElement _signatoryType;

            [FindsBy(How = How.CssSelector, Using = "[name$='CertificateAdvice.dropdownImitator']")]
            private IWebElement _certificateAdvice;

            public string DateReceived
            {
                get { return _dateReceived.GetValue(); }
            }

            public string DateSigned
            {
                get { return _dateSigned.GetValue(); }
            }

            public string SignatoryType
            {
                get { return _signatoryType.GetValue(); }
            }

            public string CertificateAdvice
            {
                get { return _certificateAdvice.GetValue(); }
            }
        }
    }
}
