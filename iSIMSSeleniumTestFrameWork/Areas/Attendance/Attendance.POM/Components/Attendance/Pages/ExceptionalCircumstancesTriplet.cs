using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using POM.Base;
using POM.Components.Common;
using POM.Helper;
using System.Threading;
using WebDriverRunner.webdriver;

namespace POM.Components.Attendance
{
    public class ExceptionalCircumstancesTriplet : BaseComponent
    {
        public override By ComponentIdentifier
        {
            get { return SimsBy.Id("screen"); }
        }

        public ExceptionalCircumstancesTriplet()
        {
            _searchCriteria = new ExceptionalCircumstancesSearchPage(this);
        }

        public class ExceptionalCircumstancesSearchResultTile : SearchResultTileBase
        {
            [FindsBy(How = How.CssSelector, Using = "[data-automation-id='resultTile']")]
            private IWebElement _name;

            public string Name
            {
                get { return _name.Text; }
            }
        }


        #region Properties

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='Button_Dropdown']")]
        private IWebElement _createButton;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='well_know_action_save']")]
        private IWebElement _saveButton;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='Selected']")]
        public IWebElement selectedPupilMenuItem;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='Whole']")]
        public IWebElement wholeSchoolMenuItem;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='yes_button']")]
        public IWebElement _yesButton;
        #endregion


        #region Search

        private readonly ExceptionalCircumstancesSearchPage _searchCriteria;
        public ExceptionalCircumstancesSearchPage SearchCriteria { get { return _searchCriteria; } }

        #endregion

        #region Actions

        public void Create()
        {
            _createButton.ClickByJS();
            Wait.WaitForControl(SimsBy.CssSelector("[data-automation-id='Selected']"));
        }

        public void Save()
        {
            _saveButton.ClickByJS();
            Wait.WaitForAjaxReady(SimsBy.CssSelector(".locking-mask"));
        }

        public void ConfirmAndSave()
        {
            _saveButton.ClickByJS();
            if (WebContext.WebDriver.FindElements(By.CssSelector("[data-automation-id='confirmation_required_dialog']")).Count > 0)
            {
                var confirmDialog = new ConfirmRequiredDialog();
                confirmDialog.ClickYes();
            }      
        }

        public ExceptionalCircumstancesDetailPage SelectSelectedPupils()
        {
            selectedPupilMenuItem.ClickByJS();
            Wait.WaitForAjaxReady(SimsBy.CssSelector(".locking-mask"));

            return new ExceptionalCircumstancesDetailPage();
        }

        public ExceptionalCircumstancesDetailPage SelectWholeSchool()
        {
            wholeSchoolMenuItem.ClickByJS();
            Wait.WaitForAjaxReady(SimsBy.CssSelector(".locking-mask"));

            return new ExceptionalCircumstancesDetailPage();
        }

        public bool HasConfirmedSave()
        {
            var css = string.Format("{0}", SeleniumHelper.AutomationId("status_success"));
            Thread.Sleep(1000);// //TODO: Find better alternative for wait.
            bool value = WebContext.WebDriver.FindElement(By.CssSelector(css)).Displayed;
            return value;
        }

        //public bool IsDisplayedValidationWarning()
        //{
        //    WaitUntilDisplayed(AttendanceElements.ValidationWarning);
        //    bool value = WebContext.WebDriver.FindElement(AttendanceElements.ValidationWarning).Displayed;
        //    return value;
        //}

        #endregion
    }
}
