using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using SharedComponents.Helpers;
using OpenQA.Selenium.Support.PageObjects;

namespace Staff.Components.StaffRegression
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
            set { _absenceType.ChooseSelectorOption(value); }
            get { return _absenceType.GetValue(); }
        }

        public string IllnessCategory
        {
            set { _illnessCategory.ChooseSelectorOption(value); }
            get { return _illnessCategory.GetValue(); }
        }

        public string AbsencePayRate
        {
            set { _absencePayRate.ChooseSelectorOption(value); }
            get { return _absencePayRate.GetValue(); }
        }

        public string PayrollAbsenceCategory
        {
            set { _payrollAbsenceCategory.ChooseSelectorOption(value); }
            get { return _payrollAbsenceCategory.GetValue(); }
        }


        public bool AnnualLeave
        {
            set { _annualLeave.SetCheckBox(value); }
            get { return _annualLeave.IsCheckboxChecked(); }
        }

        public bool IndustrialInjury
        {
            set { _industrialInjury.SetCheckBox(value); }
            get { return _industrialInjury.IsCheckboxChecked(); }
        }

        public bool SSPExclusion
        {
            set { _sspExclusion.SetCheckBox(value); }
            get { return _sspExclusion.IsCheckboxChecked(); }
        }
        #endregion
    }
}
