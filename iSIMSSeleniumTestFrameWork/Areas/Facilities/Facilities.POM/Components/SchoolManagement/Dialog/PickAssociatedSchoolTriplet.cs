using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using POM.Helper;
using POM.Base;


namespace POM.Components.SchoolManagement
{
    public class PickAssociatedSchoolTriplet : BaseDialogComponent
    {
        public override By DialogIdentifier
        {
            get { return SimsBy.AutomationId("Associated_School_Pallete_School_detail"); }
        }

        #region Properties

        private readonly AssociatedSchoolSearch _searchCriteria;

        public AssociatedSchoolSearch SearchCriteria
        {
            get { return _searchCriteria; }
        }

        public PickAssociatedSchoolTriplet()
        {
            _searchCriteria = new AssociatedSchoolSearch(this);
        }

        #endregion


        public class SearchResultTite : SearchResultTileBase 
        {
            [FindsBy(How = How.CssSelector, Using = "[data-automation-id='resultTile']")]
            private IWebElement _name;

            public string Name
            {
                get { return _name.Text; }
            }
        }
    }

    public class AssociatedSchoolSearch : SearchCriteriaComponent<PickAssociatedSchoolTriplet.SearchResultTite>
    {
        public AssociatedSchoolSearch(BaseComponent component) : base(component) { }

        #region Properties

        [FindsBy(How = How.Name, Using = "Name")]
        private IWebElement _schoolName;

        public string SchoolName
        {
            set { _schoolName.SetText(value); }
        }

        #endregion
    }
}
