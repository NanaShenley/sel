using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using POM.Base;
using POM.Helper;

namespace POM.Components.Staff
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

        [FindsBy(How = How.Name, Using = "tri_chkbox_AnnualLeave")]
        private IWebElement _annualLeave;

        [FindsBy(How = How.Name, Using = "tri_chkbox_IndustrialInjury")]
        private IWebElement _industrialInjury;

        [FindsBy(How = How.Name, Using = "tri_chkbox_SSPExclusion")]
        private IWebElement _sspExclusion;

        #endregion

        #region Page action

        public string StartDate
        {
            set { _startDate.SetDateTime(value, true); }
            get { return _startDate.GetDateTime(); }
        }

        public string EndDate
        {
            set { _endDate.SetDateTime(value, true); }
            get { return _endDate.GetDateTime(); }
        }

        public string ExpectedReturnDate
        {
            set { _expectedReturnDate.SetDateTime(value, true); }
            get { return _expectedReturnDate.GetDateTime(); }
        }

        public string ActualReturnDate
        {
            set { _actualReturnDate.SetDateTime(value, true); }
            get { return _actualReturnDate.GetDateTime(); }
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

        #endregion

    }
}
