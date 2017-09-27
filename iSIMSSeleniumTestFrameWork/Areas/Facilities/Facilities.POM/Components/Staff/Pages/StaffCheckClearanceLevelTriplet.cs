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
    public class StaffCheckClearanceLevelTriplet : BaseComponent
    {
        public override By ComponentIdentifier
        {
            get { return SimsBy.Id("screen"); }
        }

        public StaffCheckClearanceLevelTriplet()
        {
            _searchCriteria = new StaffCheckClearanceLevelSearchPage(this);
        }

        #region Search

        private readonly StaffCheckClearanceLevelSearchPage _searchCriteria;

        public StaffCheckClearanceLevelSearchPage SearchCriteria
        {
            get { return _searchCriteria; }
        }

        #endregion
    }

    public class StaffCheckClearanceLevelSearchPage : SearchTableCriteriaComponent
    {
        public StaffCheckClearanceLevelSearchPage(BaseComponent context) : base(context) { }

        #region Page properties

        [FindsBy(How = How.Name, Using = "CodeOrDescription")]
        private IWebElement _searchTextBox;

        public string SearchCodeOrDescription
        {
            get { return _searchTextBox.GetValue(); }
            set { _searchTextBox.SetText(value); }
        }

        #endregion
    }

}
