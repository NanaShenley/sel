using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using SeSugar.Automation;
using Staff.POM.Base;
using Staff.POM.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Staff.POM.Components.Staff
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
