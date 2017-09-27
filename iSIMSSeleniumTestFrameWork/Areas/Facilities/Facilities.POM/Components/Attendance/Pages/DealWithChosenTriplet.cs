using OpenQA.Selenium;
using POM.Base;
using POM.Helper;

namespace POM.Components.Attendance
{
    public class DealWithChosenTriplet : BaseComponent
    {
        public override By ComponentIdentifier
        {
            get { return SimsBy.CssSelector("[data-detail-section-name='searchResults']"); }
        }

        public DealWithChosenTriplet()
        {
            _searchCriteria = new DealWithChosenSearchPage(this);
        }

        #region Search

        private readonly DealWithChosenSearchPage _searchCriteria;
        public DealWithChosenSearchPage SearchCriteria { get { return _searchCriteria; } }

        #endregion
    }
}
