using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using POM.Base;
using POM.Helper;


namespace POM.Components.Pupil
{
    public class SelectSchoolTripletDialog : BaseDialogComponent
    {
        public override By DialogIdentifier
        {
            get { return SimsBy.Id("dialog-dialog-palette-editor"); }
        }

        #region Search

        private readonly SelectSchoolSearchDialog _searchCriteria;

        public SelectSchoolTripletDialog()
        {
            _searchCriteria = new SelectSchoolSearchDialog(this);
        }
        public SelectSchoolSearchDialog SearchCriteria { get { return _searchCriteria; } }


        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='create_button']")]
        private IWebElement _createButton;

        #endregion

        #region Page Action
        public MedicalPracticeDialog Create()
        {
            _createButton.ClickByJS();
            Wait.WaitForAjaxReady(By.CssSelector(".locking-mask"));
            return new MedicalPracticeDialog();
        }

        public void ClickOk()
        {
            base.ClickOk();
            Wait.WaitLoading();
        }
        #endregion
    }
}
