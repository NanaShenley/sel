using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using POM.Base;
using POM.Helper;
using System;
using System.Collections.Generic;
using POM.Components.Pupil;
using POM.Components.Attendance;
using WebDriverRunner.webdriver;

namespace POM.Components.Attendance
{
    public class ApplyMarksOverDateRangeDialog : BaseDialogComponent
    {
        public override By DialogIdentifier
        {
            get { return SimsBy.Id("task_menu_section_attendance_EnterMarkOverDateRange-palette-editor"); }
        }

        #region Page properties

        [FindsBy(How = How.Name, Using = "StartDate")]
        private IWebElement _startDateTextbox;
        [FindsBy(How = How.Name, Using = "EndDate")]
        private IWebElement _endDateTextbox;
        [FindsBy(How = How.Name, Using = "AcademicYear.dropdownImitator")]
        private IWebElement _academicYearDropdown;
        [FindsBy(How = How.Name, Using = "Mark.dropdownImitator")]
        private IWebElement _selectMark;
        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='apply_mark_button']")]
        private IWebElement _applyButton;
        [FindsBy(How = How.CssSelector, Using = ("[data-automation-id='close_button']"))]
        private IWebElement _closeButton;
        [FindsBy(How = How.CssSelector, Using = ("[data-automation-id='add_pupils_button']"))]
        private IWebElement _addPupil;
        [FindsBy(How = How.CssSelector, Using = ("[view_id='cxgridMarksOverDateRangeLearnerLearner'] input"))]
        private IWebElement _selectCheckBox;
        [FindsBy(How = How.CssSelector, Using = "[view_id='cxgridMarksOverDateRangeLearnerLearner'] .webix_ss_body")]
        private IWebElement _selectPupilTable;
        public static readonly By ValidationWarning = By.CssSelector("[data-automation-id='status_error']");

        public string StartDate
        {
            set { _startDateTextbox.SetDateTime(value); }
        }

        public string EndDate
        {
            set { _endDateTextbox.SetDateTime(value); }
        }

        public bool SelectPupil
        {
            set { _selectCheckBox.Set(value); }
            get { return _selectCheckBox.IsChecked(); }
        }

        public WebixComponent<WebixCell> SelectPupilsTable
        {
            get { return new WebixComponent<WebixCell>(_selectPupilTable); }
        }

        public bool IsPreserve
        {
            set
            {
                string javascript = "document.getElementsByName('PreserveOverwrite')[0].checked = '{0}';";
                SeleniumHelper.ExecuteJavascript(String.Format(javascript, value));
            }
        }

        public bool IsOverwrite
        {
            set
            {
                string javascript = "document.getElementsByName('PreserveOverwrite')[1].checked = '{0}';";
                SeleniumHelper.ExecuteJavascript(String.Format(javascript, value));
            }
        }
       
        public string SelectMark
        {
            set {_selectMark.EnterForDropDown(value);}
            get {return _selectMark.GetValue();}
        }

        public string SelectAcademicYear
        {
            set {_academicYearDropdown.EnterForDropDown(value);}
            get {return _academicYearDropdown.GetValue();}
        }

        #endregion

        #region Page actions

        public POM.Components.Common.ConfirmRequiredDialog ClickApply()
        {
            _applyButton.Click();
            Wait.WaitForAjaxReady(By.CssSelector(".locking-mask"));
            return new POM.Components.Common.ConfirmRequiredDialog();
        }

        public virtual void ClickApplyThisMark()
        {
            _applyButton.ClickByJS();
            Wait.WaitForAjaxReady(By.CssSelector(".locking-mask"));

            Wait.WaitForDocumentReady();
            //SeleniumHelper.Sleep(sleep);
        }

        public AddPupilsDialogTriplet AddPupil()
        {
            _addPupil.Click();
            Wait.WaitForAjaxReady(By.CssSelector(".locking-mask"));
            return new AddPupilsDialogTriplet();
        }

        public PupilRecordPage ClickClose()
        {
            _closeButton.Click();
            return new PupilRecordPage();

        }
        public void ClosePatternDialog()
        {
            _closeButton.Click();

        }

        public bool IsValidationWarningDisplayed()
        {
            Wait.WaitLoading();      
            bool value = WebContext.WebDriver.FindElement(ValidationWarning).Displayed;
            return value;
        }

        #endregion

    }
}
