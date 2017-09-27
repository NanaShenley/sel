using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using SeSugar.Automation;
using Staff.POM.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Staff.POM.Helper;

namespace Staff.POM.Components.Staff.Pages
{
    public class SalaryRangeTriple : BaseComponent
    {
        public override By ComponentIdentifier
        {
            get { return SimsBy.AutomationId("SalaryRange_triplet"); }
        }

        public SalaryRangeTriple()
        {
            _searchCriteria = new SalaryRangeSearch(this);
        }

        #region Search

        private readonly SalaryRangeSearch _searchCriteria;
        public SalaryRangeSearch SearchCriteria { get { return _searchCriteria; } }

        #endregion


        public class SalaryRangeSearchResultTile : SearchResultTileBase
        {
            [FindsBy(How = How.CssSelector, Using = "[data-automation-id='resultTile']")]
            private IWebElement _name;

            public string Name
            {
                get { return _name.Text; }
            }
        }

        public class SalaryRangeSearch : SearchCriteriaComponent<SalaryRangeTriple.SalaryRangeSearchResultTile>
        {
            public SalaryRangeSearch(BaseComponent parent) : base(parent) { }

            #region Properties

            [FindsBy(How = How.Name, Using = "Code")]
            private IWebElement _searchTextBox;

            public string Code
            {
                get { return _searchTextBox.GetValue(); }
                set { _searchTextBox.SetText(value); }
            }

            #endregion
        }
    }
}


