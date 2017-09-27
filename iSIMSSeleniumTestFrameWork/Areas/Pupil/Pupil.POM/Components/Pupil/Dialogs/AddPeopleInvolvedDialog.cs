using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using POM.Base;
using POM.Helper;

namespace POM.Components.Pupil
{
    public class AddPeopleInvolvedDialog : BaseDialogComponent
    {
        public override By DialogIdentifier
        {
            get { return SimsBy.AutomationId("sen_review_people_involved_participant_detail"); }
        }

        public AddPeopleInvolvedDialog()
        {
            _searchCriteria = new StaffSearch(this);
        }

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

        #endregion Search
    }
}