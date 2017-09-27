using OpenQA.Selenium;
using POM.Base;
using POM.Helper;

namespace POM.Components.Staff
{
    public class DeleteStaffRecordTriplet : BaseComponent
    {
        public override By ComponentIdentifier
        {
            get { return SimsBy.AutomationId("staff_record_triplet"); }
        }

        public DeleteStaffRecordTriplet()
        {
            _searchCriteria = new DeleteStaffRecordSearchPage(this);
        }

        #region Search

        private readonly DeleteStaffRecordSearchPage _searchCriteria;
        public DeleteStaffRecordSearchPage SearchCriteria { get { return _searchCriteria; } }

        #endregion
    }

}
