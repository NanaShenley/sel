using OpenQA.Selenium;
using POM.Base;
using POM.Helper;

namespace POM.Components.Staff
{
    public class ApplySalaryUpdateTriplet : BaseComponent
    {
        public override By ComponentIdentifier
        {
            get { return SimsBy.Id("workspace"); }
        }

        public ApplySalaryUpdateTriplet()
        {
            _searchCriteria = new ApplySalaryUpdateSeachPage(this);
        }

        #region Search

        private readonly ApplySalaryUpdateSeachPage _searchCriteria;
        public ApplySalaryUpdateSeachPage SearchCriteria { get { return _searchCriteria; } }

        #endregion

    }
}
