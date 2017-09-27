using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using POM.Base;
using POM.Components.Pupil;
using POM.Helper;

namespace Facilities.POM.Components.Pupil.Dialogs
{
    public class SchoolHistoryDialog : BaseDialogComponent
    {
        public override By DialogIdentifier
        {
            get { return SimsBy.Id("dialog-palette-editor"); }
        }

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='select_button']")]
        private IWebElement _selectButton;
        
        #region Page Action
        public SelectSchoolTripletDialog select()
        {
            _selectButton.ClickByJS();
            Wait.WaitForAjaxReady(By.CssSelector(".locking-mask"));
            return new SelectSchoolTripletDialog();
        }

        public void ClickOk()
        {
            base.ClickOk();
            Wait.WaitLoading();
        }
        #endregion
    }
}
