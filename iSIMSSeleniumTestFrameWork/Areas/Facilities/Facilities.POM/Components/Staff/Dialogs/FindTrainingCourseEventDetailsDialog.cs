using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using POM.Base;
using POM.Helper;


namespace POM.Components.Staff
{
    public class FindTrainingCourseEventDetailsDialog : BaseDialogComponent
    {
        public override By DialogIdentifier
        {
            get { return SimsBy.Id("dialog-editableData"); }
        }

        #region Page properties

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='ok_button']")]
        private IWebElement _addCourseButton;

        #endregion

        #region Public methods

        public StaffRecordPage AddCourseButton()
        {
            _addCourseButton.ClickByJS();
            Wait.WaitForAjaxReady(By.CssSelector(".locking-mask"));
            return new StaffRecordPage();
        }

        #endregion


    }
}
