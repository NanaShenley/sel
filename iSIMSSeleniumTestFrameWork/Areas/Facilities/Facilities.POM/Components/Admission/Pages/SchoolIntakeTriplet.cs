using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using POM.Base;
using POM.Helper;



namespace POM.Components.Admission
{
    public class SchoolIntakeTriplet : BaseComponent
    {
        public override By ComponentIdentifier
        {
            get { return SimsBy.Id("screen"); }
        }

        public SchoolIntakeTriplet()
        {
            _searchCriteria = new SchoolIntakeSearchPage(this);
        }

        #region Search

        private readonly SchoolIntakeSearchPage _searchCriteria;
        public SchoolIntakeSearchPage SearchCriteria { get { return _searchCriteria; } }

        public class SchoolIntakeSearchResultTile : SearchResultTileBase
        {
            [FindsBy(How = How.CssSelector, Using = "[data-automation-id='resultTile']")]
            private IWebElement _code;

            public string Code
            {
                get { return _code.Text; }
            }
        }

        #endregion

        #region Properties

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='create_button']")]
        private IWebElement _createButton;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='delete_button']")]
        private IWebElement _deleteButton;

        #endregion

        #region Public methods

        /// <summary>
        /// Author: Huy.Vo
        /// Description: Init page
        /// </summary>
        /// <returns></returns>
        public SchoolIntakePage Create()
        {
            SeleniumHelper.Get(SimsBy.AutomationId("create_button")).ClickByJS();
            Wait.WaitForAjaxReady(SimsBy.CssSelector(".locking-mask"));
            return new SchoolIntakePage();
        }

        /// <summary>
        /// Author: Huy.Vo
        /// Description: click Delete button to delete an existing School intake
        /// </summary>

        public void SelectSearchTile(SchoolIntakeSearchResultTile schoolIntakeTile)
        {
            if (schoolIntakeTile != null)
            {
                schoolIntakeTile.Click();
            }
        }

        public void Delete()
        {
            if (_deleteButton.IsExist())
            {
                _deleteButton.ClickByJS();
                Wait.WaitForAjaxReady(SimsBy.CssSelector(".locking-mask"));
                var confirmDialog = new DeleteConfirmationPage();
                confirmDialog.ConfirmDelete();
            }
        }
        public SchoolIntakePage CancelDeleteSchoolIntake()
        {
            if (_deleteButton.IsExist())
            {
                _deleteButton.ClickByJS();
                Wait.WaitForAjaxReady(SimsBy.CssSelector(".locking-mask"));
                var confirmDialog = new DeleteConfirmationPage();
                confirmDialog.CancelDelete();
            }
            return new SchoolIntakePage();
        }

        #endregion
    }

    public class SchoolIntakeSearchPage : SearchCriteriaComponent<SchoolIntakeTriplet.SchoolIntakeSearchResultTile>
    {
        public SchoolIntakeSearchPage(BaseComponent parent) : base(parent) { }

        #region Search properties

        [FindsBy(How = How.Name, Using = "Name")]
        private IWebElement _searchByName;

        [FindsBy(How = How.Name, Using = "YearOfAdmission.dropdownImitator")]
        private IWebElement _searchByAdmissionYear;

        [FindsBy(How = How.Name, Using = "YearGroup.dropdownImitator")]
        private IWebElement _searchYearGroup;

        [FindsBy(How = How.Id, Using = "tri_chkbox_IncludeInactiveSchoolIntakes")]
        private IWebElement _activeOrInActive;

        public string SearchByName
        {
            get { return _searchByName.GetValue(); }
            set { _searchByName.SetText(value); }
        }
        public string SearchYearAdmissionYear
        {
            get { return _searchByAdmissionYear.GetValue(); }
            set { _searchByAdmissionYear.EnterForDropDown(value); }
        }
        public string SearchByYearGroup
        {
            get { return _searchYearGroup.GetValue(); }
            set { _searchYearGroup.EnterForDropDown(value); }
        }

        public bool SetActiveOrInActive
        {
            set { _activeOrInActive.Set(value); }
            get { return _activeOrInActive.IsChecked(); }
        }
        #endregion
    }
}
