using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using Staff.POM.Base;
using Staff.POM.Helper;
using SeSugar.Automation;


namespace Staff.POM.Components.Staff
{
    public class StaffAbsenceCertificateDialog : BaseDialogComponent
    {
        public override By DialogIdentifier
        {
            get { return SimsBy.AutomationId("staff_absence_certification_dialog"); }
        }

        private const string pfwAID = "A Phased Return to Work";
        private const string adAID = "Amended Duties";
        private const string ahAID = "Altered Hours";
        private const string waAID = "Workplace Adaptations";

        #region Page properties

        [FindsBy(How = How.Name, Using = "SignatoryType.dropdownImitator")]
        private IWebElement _signatoryType;

        [FindsBy(How = How.Name, Using = "DateReceived")]
        private IWebElement _dateRecieved;

        [FindsBy(How = How.Name, Using = "DateSigned")]
        private IWebElement _dateSigned;

        [FindsBy(How = How.Name, Using = "SignedBy")]
        private IWebElement _signedBy;

        [FindsBy(How = How.Name, Using = "StartDate")]
        private IWebElement _startDate;

        [FindsBy(How = How.Name, Using = "EndDate")]
        private IWebElement _endDate;

        [FindsBy(How = How.Name, Using = "Duration")]
        private IWebElement _duration;

        [FindsBy(How = How.Name, Using = "CertificateAdvice.dropdownImitator")]
        private IWebElement _certificateAdvice;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='" + pfwAID + "']")]
        private IWebElement _phasedReturn;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='" + adAID + "']")]
        private IWebElement _amendedDuties;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='" + ahAID + "']")]
        private IWebElement _alteredHours;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='" + waAID + "']")]
        private IWebElement _workplaceAdaptations;

        [FindsBy(How = How.Name, Using = "IsReAssessmentRequired")]
        private IWebElement _isReAssessmentRequired;

        #endregion

        #region Page action

        public string SignatoryType
        {
            get { return _signatoryType.GetValue(); }
        }

        public string DateRecieved
        {
            set { _dateRecieved.SetText(value); }
            get { return _dateRecieved.GetValue(); }
        }

        public string DateSigned
        {
            set { _dateSigned.SetText(value); }
            get { return _dateSigned.GetValue(); }
        }

        public string SignedBy
        {
            set { _signedBy.SetText(value); }
            get { return _signedBy.GetValue(); }
        }

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

        public string Duration
        {
            get { return _duration.GetValue(); }
        }

        public string CertificateAdvice
        {
            set{ _certificateAdvice.EnterForDropDown(value);}
            get { return _certificateAdvice.GetValue(); }
        }

        public bool PhasedReturn
        {
            set
            {
                AutomationSugar.WaitFor(new ByChained(ComponentIdentifier, SimsBy.AutomationId(pfwAID)));
                System.Threading.Thread.Sleep(250);
                _phasedReturn.Set(value);
            }
            get { return _phasedReturn.IsChecked(); }
        }

        public bool AmendedDuties
        {
            set
            {
                AutomationSugar.WaitFor(new ByChained(ComponentIdentifier, SimsBy.AutomationId(adAID)));
                System.Threading.Thread.Sleep(250);
                _amendedDuties.Set(value);
            }
            get { return _amendedDuties.IsChecked(); }
        }

        public bool AlteredHours
        {
            set
            {
                AutomationSugar.WaitFor(new ByChained(ComponentIdentifier, SimsBy.AutomationId(ahAID)));
                System.Threading.Thread.Sleep(250);
                _alteredHours.Set(value);
            }
            get { return _alteredHours.IsChecked(); }
        }

        public bool WorkplaceAdaptations
        {
            set
            {
                AutomationSugar.WaitFor(new ByChained(ComponentIdentifier, SimsBy.AutomationId(waAID)));
                System.Threading.Thread.Sleep(250);
                _workplaceAdaptations.Set(value);
            }
            get { return _workplaceAdaptations.IsChecked(); }
        }

        public bool IsReAssessmentRequired
        {
            set { _isReAssessmentRequired.Set(value); }
            get { return _isReAssessmentRequired.IsChecked(); }
        }

        #endregion
    }
}
