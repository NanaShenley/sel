using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium.Support.PageObjects;
using OpenQA.Selenium;
using NUnit.Framework;
using POM.Base;
using POM.Helper;




namespace Attendance.POM.Components.Communication
{
    public class ServiceTypesTriplet : BaseComponent
    {
        public override By ComponentIdentifier
        {
            get { return SimsBy.Id("screen"); }
        }

        public ServiceTypesTriplet()
        {
            _searchCriteria = new ServiceTypesSearchPage(this);
        }

        #region Search

        private readonly ServiceTypesSearchPage _searchCriteria;
        public ServiceTypesSearchPage SearchCriteria { get { return _searchCriteria; } }

        #endregion

        #region Properties

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='create_service_ServiceType']")]
        private IWebElement _addServiceProvided;
        
        #endregion

    }

    public class ServiceTypesSearchPage : SearchTableCriteriaComponent
    {
        public ServiceTypesSearchPage(BaseComponent parent) : base(parent) { }

        #region Search properties

        [FindsBy(How = How.Name, Using = "CodeOrDescription")]
        private IWebElement _searchByName;

        public string SearchByName
        {
            get { return _searchByName.GetValue(); }
            set { _searchByName.SetText(value); }
        }
     
        #endregion
    }
}
