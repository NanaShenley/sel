using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using POM.Base;
using POM.Helper;

namespace POM.Components.Pupil
{
    public class SenRecordTriplet : BaseComponent
    {
        public override By ComponentIdentifier
        {
            get { return SimsBy.Id("screen"); }
        }

        public SenRecordTriplet()
        {
            _searchCriteria = new SenRecordSearch(this);
        }

        #region Search

        private readonly SenRecordSearch _searchCriteria;
        public SenRecordSearch SearchCriteria { get { return _searchCriteria; } }

        public class SenRecordSearchResultTile : SearchResultTileBase
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

    public class SenRecordSearch : SearchListCriteriaComponent<SenRecordTriplet.SenRecordSearchResultTile>
    {
        public SenRecordSearch(BaseComponent parent) : base(parent) { }

        #region Search section properties

        [FindsBy(How = How.Name, Using = "LegalSurname")]
        private IWebElement _nameTextbox;

        [FindsBy(How = How.Id, Using = "tri_chkbox_SENNeverExistCriterion")]
        private IWebElement _noSenStageAssignedCheckbox;

        [FindsBy(How = How.Name, Using = "SENStatus.dropdownImitator")]
        private IWebElement _senStageDropdown;

        public string Name
        {
            set { _nameTextbox.SetText(value); }
        }

        public bool NoSenStageAssigned
        {
            set { _noSenStageAssignedCheckbox.Set(value); }
        }

        public string SenStage
        {
            set { _senStageDropdown.EnterForDropDown(value); }
        }

        #endregion
    }
}
