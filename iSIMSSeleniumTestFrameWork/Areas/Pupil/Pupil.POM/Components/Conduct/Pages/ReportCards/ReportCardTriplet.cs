using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using POM.Base;
using POM.Components.Common;
using POM.Helper;
// ReSharper disable ConvertPropertyToExpressionBody
#pragma warning disable 649

namespace POM.Components.Conduct.Pages.ReportCards
{
    /// <summary>
    /// Implements ReportCardTriplet
    /// </summary>
    public class ReportCardTriplet : BaseComponent
    {
        /// <summary>
        /// Gets the component identifier.
        /// </summary>
        /// <value>
        /// The component identifier.
        /// </value>
        public override By ComponentIdentifier
        {
            get { return SimsBy.Id("screen"); }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ReportCardTriplet" /> class.
        /// </summary>
        public ReportCardTriplet()
        {
            _searchCriteria = new ReportCardSearchPage(this);
        }

        /// <summary>
        /// Exists the exclusion page.
        /// </summary>
        /// <returns></returns>
        public WarningDialog ExistReportCardPage()
        {
            IWebElement existButton = SeleniumHelper.Get(SimsBy.AutomationId("tab_Report_Card_close_button"));
            existButton.Click();
            return new WarningDialog();
        }

        #region Search

        /// <summary>
        /// Implements ExclusionSearchResultTile
        /// </summary>
        public class ReportCardSearchResultTile : SearchResultTileBase
        {
            /// <summary>
            /// The _name
            /// </summary>
            [FindsBy(How = How.CssSelector, Using = "[data-automation-id='resultTile']")]
            private IWebElement _name;

            /// <summary>
            /// Gets the name.
            /// </summary>
            /// <value>
            /// The name.
            /// </value>
            public string Name
            {
                get { return _name.Text; }
            }
        }

        /// <summary>
        /// The _search criteria
        /// </summary>
        private readonly ReportCardSearchPage _searchCriteria;
        /// <summary>
        /// Gets the search criteria.
        /// </summary>
        /// <value>
        /// The search criteria.
        /// </value>
        public ReportCardSearchPage SearchCriteria
        {
            get { return _searchCriteria; }
        }

        #endregion
    }    
}
