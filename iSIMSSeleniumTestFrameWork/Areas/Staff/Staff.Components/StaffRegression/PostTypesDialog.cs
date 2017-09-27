using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using SharedComponents.Helpers;

namespace Staff.Components.StaffRegression
{
    public class PostTypesDialog : BaseDialogComponent
    {
        public override By DialogIdentifier
        {
            get { return SimsBy.AutomationId("service_terms_post_types_dialog"); }
        }

        public class PostTypeSearchResultTile : SearchResultTileBase
        {
            [FindsBy(How = How.CssSelector, Using = "[title='Code']")]
            private IWebElement _code;

            public string Code
            {
                get { return _code.Text; }
            }
        }

        private readonly SearchCriteriaComponent<PostTypeSearchResultTile> _searchCriteria;
        public SearchCriteriaComponent<PostTypeSearchResultTile> SearchCriteria { get { return _searchCriteria; } }

        public PostTypesDialog()
        {
            _searchCriteria = new SearchCriteriaComponent<PostTypeSearchResultTile>(this);
        }
    }
}
