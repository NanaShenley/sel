using OpenQA.Selenium;
using POM.Base;
using POM.Helper;

namespace POM.Components.Attendance
{
    public class DealWithSpecifcMarksTriplet : BaseComponent
    {
        public override By ComponentIdentifier
        {
            get { return SimsBy.CssSelector("[data-detail-section-name='searchResults']"); }
        }

        public DealWithSpecifcMarksTriplet()
        {
            _searchCriteria = new DealWithSpecificMarksSearchPage(this);
        }

        #region Search

        private readonly DealWithSpecificMarksSearchPage _searchCriteria;
        public DealWithSpecificMarksSearchPage SearchCriteria { get { return _searchCriteria; } }


        #endregion
    }
}
