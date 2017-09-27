using OpenQA.Selenium;
using POM.Base;
using POM.Helper;

namespace POM.Components.Attendance
{
    public class TakeRegisterTriplet : BaseComponent
    {
        public override By ComponentIdentifier
        {
            get { return SimsBy.CssSelector("[data-detail-section-name='searchResults']"); }
        }

        public TakeRegisterTriplet()
        {
            _searchCriteria = new TakeRegisterSearchPage(this);
        }

        #region Search

        private readonly TakeRegisterSearchPage _searchCriteria;
        public TakeRegisterSearchPage SearchCriteria { get { return _searchCriteria; } }

        #endregion
    }
}
