using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using POM.Base;
using POM.Helper;

namespace POM.Components.Pupil
{
    public class SelectOfficerDialog : BaseDialogComponent
    {
        public override By DialogIdentifier
        {
            get { return SimsBy.AutomationId("sen_statement_elb_officer_detail"); }
        }

        public SelectOfficerDialog()
        {
            _searchCriteria = new OfficerSearch(this);
        }

        #region Search

        private readonly OfficerSearch _searchCriteria;

        public OfficerSearch SearchCriteria { get { return _searchCriteria; } }

        public class OfficerSearchResultTile : SearchResultTileBase
        {
            [FindsBy(How = How.CssSelector, Using = "[title='Name']")]
            private IWebElement _name;

            public string Name
            {
                get { return _name.Text; }
            }
        }

        #endregion
    }

    public class OfficerSearch : SearchListCriteriaComponent<SelectOfficerDialog.OfficerSearchResultTile>
    {
        public OfficerSearch(BaseComponent parent) : base(parent) { }

        #region Search section properties

        [FindsBy(How = How.Name, Using = "Surname")]
        private IWebElement _surnameTextBox;

        [FindsBy(How = How.Name, Using = "Forename")]
        private IWebElement _forenameTextBox;

        public string SurName
        {
            set { _surnameTextBox.SetText(value); }
            get { return _surnameTextBox.GetValue(); }
        }

        public string ForeName
        {
            set { _forenameTextBox.SetText(value); }
            get { return _forenameTextBox.GetValue(); }
        }
        #endregion
    }
}
