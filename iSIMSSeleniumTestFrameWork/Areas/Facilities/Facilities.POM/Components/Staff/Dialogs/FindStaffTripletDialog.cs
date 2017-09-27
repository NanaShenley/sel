using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using POM.Base;
using POM.Helper;


namespace POM.Components.Staff
{
    public class FindStaffTripletDialog : BaseDialogComponent
    {
        public override By DialogIdentifier
        {
            get { return SimsBy.AutomationId("staff_simple_search_palette_triplet"); }
        }

        public FindStaffTripletDialog()
        {
            _searchCriteria = new StaffSearch(this);
        }

        #region Page properties

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='ok_button']")]
        private IWebElement _okButton;

        #endregion

        #region Public methods

        public TrainingCourseEventDialog ClickOk()
        {
            _okButton.ClickByJS();
            Wait.WaitForAjaxReady(SimsBy.CssSelector(".locking-mask"));

            //Handle error page not response )
            Wait.WaitLoading();
            return new TrainingCourseEventDialog();
        }

        #endregion

        #region Search

        private readonly StaffSearch _searchCriteria;
        public StaffSearch SearchCriteria { get { return _searchCriteria; } }

        public class StaffSearchResultTile : SearchResultTileBase
        {
            [FindsBy(How = How.CssSelector, Using = "[data-automation-id='resultTile']")]
            private IWebElement _name;

            public string Name
            {
                get { return _name.Text; }
            }
        }

        #endregion
    }
    public class StaffSearch : SearchListCriteriaComponent<FindStaffTripletDialog.StaffSearchResultTile>
    {
        public StaffSearch(BaseComponent parent) : base(parent) { }

        #region Search section properties

        [FindsBy(How = How.Name, Using = "LegalSurname")]
        private IWebElement _staffNameTextBox;

        [FindsBy(How = How.Id, Using = "tri_chkbox_StatusCurrentCriterion")]
        private IWebElement _statusCurrentCheckbox;

        [FindsBy(How = How.Id, Using = "tri_chkbox_StatusFutureCriterion")]
        private IWebElement _statusFutureCheckbox;

        public string StaffName
        {
            set { _staffNameTextBox.SetText(value); }
            get { return _staffNameTextBox.GetValue(); }
        }

        public bool StatusCurrent
        {
            get { return _statusCurrentCheckbox.IsChecked(); }
            set { _statusCurrentCheckbox.Set(value); }
        }

        #endregion
    }
}
