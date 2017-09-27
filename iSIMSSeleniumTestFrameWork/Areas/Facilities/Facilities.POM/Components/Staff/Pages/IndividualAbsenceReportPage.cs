using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using Staff.POM.Base;
using Staff.POM.Helper;

namespace Staff.POM.Components.Staff
{
    class IndividualAbsenceReportPage : BaseComponent
    {
        public override By ComponentIdentifier
        {
            get
            {
                return SimsBy.CssSelector("");
            }
        }

        #region Page properties

        [FindsBy(How = How.Id, Using = "ReportViewerControl_ctl04_ctl03_txtValue")]
        private IWebElement _startDateTextBox;

        [FindsBy(How = How.Id, Using = "ReportViewerControl_ctl04_ctl07_ddValue")]
        private IWebElement _absenceTypeDropdown;

        [FindsBy(How = How.Id, Using = "ReportViewerControl_ctl04_ctl11_txtValue")]
        private IWebElement _staffDropdown;

        [FindsBy(How = How.Id, Using = "ReportViewerControl_ctl04_ctl05_txtValue")]
        private IWebElement _endDateTextBox;

        [FindsBy(How = How.Id, Using = "ReportViewerControl_ctl04_ctl09_ddValue")]
        private IWebElement _staffStatusDropdown;

        [FindsBy(How = How.Id, Using = "ReportViewerControl_ctl04_ctl13_ddValue")]
        private IWebElement _showAbsenceNoteDropdown;

        public string StartDate
        {
            set { _startDateTextBox.SetText(value); }
            get { return _startDateTextBox.GetValue(); }
        }

        public string EndDate
        {
            set { _endDateTextBox.SetText(value); }
            get { return _endDateTextBox.GetValue(); }
        }

        #endregion
    }
}
