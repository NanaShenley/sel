using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using POM.Base;
using POM.Helper;


namespace POM.Components.Pupil
{
    public class MedicalPracticeTripletDialog : BaseDialogComponent
    {
        public override By DialogIdentifier
        {
            get { return SimsBy.Id("palette-editor-container"); }
        }

        #region Page Properties

        private readonly MedicalPracticeSearchDialog _searchCriteria;

        public MedicalPracticeTripletDialog()
        {
            _searchCriteria = new MedicalPracticeSearchDialog(this);
        }

        #endregion

        #region Search

        public MedicalPracticeSearchDialog SearchCriteria { get { return _searchCriteria; } }

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='create_button']")]
        private IWebElement _createButton;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='ok_button']")]
        private IWebElement _okButton;

        public class MedicalPracticeSearchResultTile : SearchResultTileBase
        {
            [FindsBy(How = How.CssSelector, Using = "[data-automation-id='resultTile']")]
            private IWebElement _name;

            [FindsBy(How = How.CssSelector, Using = "[title ='School Name'][data-automation-id='resultTile']")]
            private IWebElement _schoolName;

            public string Name
            {
                get { return _name.Text; }
            }

            public string SchoolName
            {
                get { return _schoolName.GetText(); }
            }
        }

        #endregion

        #region Page Action
        public MedicalPracticeDialog Create()
        {
            _createButton.ClickByJS();
            Wait.WaitForAjaxReady(By.CssSelector(".locking-mask"));
            return new MedicalPracticeDialog();
        }

        public PupilRecordPage OK()
        {
            _okButton.ClickByJS();
            Wait.WaitForAjaxReady(By.CssSelector(".locking-mask"));
            SeleniumHelper.Sleep(5);
            return new PupilRecordPage();
        }


        #endregion
    }
}
