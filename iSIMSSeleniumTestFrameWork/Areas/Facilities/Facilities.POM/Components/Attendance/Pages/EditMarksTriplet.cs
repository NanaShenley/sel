using OpenQA.Selenium;
using POM.Base;
using POM.Helper;

namespace POM.Components.Attendance
{
    public class EditMarksTriplet : BaseComponent
    {
        public override By ComponentIdentifier
        {
            get { return SimsBy.CssSelector("[data-detail-section-name='searchResults']"); }
        }

        public EditMarksTriplet()
        {
            _searchCriteria = new EditMarksSearchPage(this);
        }

        #region Search

        private readonly EditMarksSearchPage _searchCriteria;
        public EditMarksSearchPage SearchCriteria { get { return _searchCriteria; } }

        #endregion
    }
}
