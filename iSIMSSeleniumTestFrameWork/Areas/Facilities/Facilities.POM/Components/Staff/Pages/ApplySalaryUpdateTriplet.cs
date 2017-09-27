using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using Staff.POM.Base;
using Staff.POM.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Staff.POM.Components.Staff
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
