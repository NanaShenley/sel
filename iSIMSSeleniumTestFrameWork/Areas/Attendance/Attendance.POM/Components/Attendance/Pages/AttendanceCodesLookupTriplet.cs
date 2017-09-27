using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SeSugar.Automation;
using POM.Base;
using POM.Helper;

namespace POM.Components.Attendance
{
    public class AttendanceCodesLookupTriplet : BaseComponent
    {
        public override By ComponentIdentifier
        {
            get { return SeSugar.Automation.SimsBy.AutomationId("lookup_double"); }
        }

        public AttendanceCodesLookupTriplet()
        {
            _searchCriteria = new AttendanceCodesSearch(this);
        }

        #region Search

        private readonly AttendanceCodesSearch _searchCriteria;
        public AttendanceCodesSearch SearchCriteria { get { return _searchCriteria; } }

        #endregion
    }

    public class AttendanceCodesSearch : SearchTableCriteriaComponent
    {
        public AttendanceCodesSearch(BaseComponent parent) : base(parent) { }

        #region Properties

        [FindsBy(How = How.Name, Using = "CodeOrDescription")]
        private IWebElement _searchTextBox;

        public string CodeOrDescription
        {
            get { return _searchTextBox.GetValue(); }
            set { _searchTextBox.SetText(value); }
        }

        #endregion
    }
}
