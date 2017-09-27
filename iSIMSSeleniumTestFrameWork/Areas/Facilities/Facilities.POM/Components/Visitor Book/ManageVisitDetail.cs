using POM.Base;
using OpenQA.Selenium;
using POM.Helper;
using OpenQA.Selenium.Support.PageObjects;
using POM.Components.Common;

namespace Facilities.POM.Components.Visitor_Book
{
    public class ManageVisitDetail : BaseComponent
    {
        public override By ComponentIdentifier
        {
            get { return SimsBy.Id("editableData"); }
        }

        #region Page properties

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='well_know_action_save']")]
        private IWebElement _saveButton;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='delete_button']")]
        private IWebElement _deleteButton;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='add_button']")]
        private IWebElement _addButton;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='status_success']")]
        private IWebElement _successMessage;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='status_error']")]
        private IWebElement _validationError;

        [FindsBy(How = How.Name, Using = "VisitorsName")]
        private IWebElement _visitorName;

        [FindsBy(How = How.Name, Using = "Note")]
        private IWebElement _visitNote;

        [FindsBy(How = How.Name, Using = "IsAllDay")]
        private IWebElement _visitAlldayCheckBox;

        [FindsBy(How = How.Name, Using = "DateOfVisit")]
        private IWebElement _visitDate;

        [FindsBy(How = How.Name, Using = "VisitStartTime")]
        private IWebElement _visitStartTime;

        [FindsBy(How = How.Name, Using = "VisitEndTime")]
        private IWebElement _visitEndTime;

        public static readonly By ValidationWarning = By.CssSelector("[data-automation-id='status_error']");

        #endregion

        #region Action

        public string VisitorName
        {
            set { _visitorName.SetText(value); }
            get { return _visitorName.GetValue(); }
        }

        public string VisitNote
        {
            set { _visitNote.SetText(value); }
            get { return _visitNote.GetValue(); }
        }

        public string StartDate
        {
            get { return _visitDate.GetValue(); }
            set { _visitDate.SetDateTime(value); }
        }

        public string StartTime
        {
            get { return _visitStartTime.GetValue(); }
            set { _visitStartTime.SetDateTime(value); }
        }

        public string EndTime
        {
            get { return _visitEndTime.GetValue(); }
            set { _visitEndTime.SetDateTime(value); }
        }


        public bool IsAllday
        {
            get { return _visitAlldayCheckBox.IsChecked(); }
            set { _visitAlldayCheckBox.Set(value); }
        }

        public bool IsSuccessMessageDisplayed()
        {
            return _successMessage.IsExist();
        }

        public void Save()
        {
            SeleniumHelper.Get(SimsBy.AutomationId("well_know_action_save")).ClickByJS();
            Wait.WaitForAjaxReady(SimsBy.CssSelector(".locking-mask"));
        }

        public DeleteConfirmationDialog Delete()
        {
            _deleteButton.ClickByJS();
            return new DeleteConfirmationDialog();
        }

        #endregion
    }
}
