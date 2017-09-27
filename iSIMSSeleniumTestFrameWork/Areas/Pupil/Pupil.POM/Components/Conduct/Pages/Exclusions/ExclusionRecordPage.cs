using System.Collections.Generic;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using POM.Base;
using POM.Helper;
using SeSugar.Automation;
using Retry = POM.Helper.Retry;
using SimsBy = POM.Helper.SimsBy;
// ReSharper disable ConvertPropertyToExpressionBody
#pragma warning disable 169
#pragma warning disable 649


namespace POM.Components.Conduct.Pages.Exclusions
{
    /// <summary>
    /// Implements ExclusionRecordPage
    /// </summary>
    public class ExclusionRecordPage : BaseComponent
    {


        /// <summary>
        /// Initializes a new instance of the <see cref="ExclusionTriplet" /> class.
        /// </summary>
        public ExclusionRecordPage()
        {
            _searchResults= new ExclusionSearchResults(this);
        }


        /// <summary>
        /// Gets the component identifier.
        /// </summary>
        /// <value>
        /// The component identifier.
        /// </value>
        public override By ComponentIdentifier
        {
            get { return SimsBy.CssSelector("[data-two-column-detail]"); }
        }

        #region Page Properties

        /// <summary>
        /// The _save button
        /// </summary>
        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='well_know_action_save']")]
        private IWebElement _saveButton;

        /// <summary>
        /// The _add button
        /// </summary>
        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='add_button']")]
        private IWebElement _addButton;

        /// <summary>
        /// The _action button
        /// </summary>
        [FindsBy(How = How.CssSelector, Using = "[title='Actions']")]
        private IWebElement _actionButton;

        /// <summary>
        /// The _header title
        /// </summary>
        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='suspension_records_header_display_name']")]
        private IWebElement _headerTitle;

        /// <summary>
        /// Gets the header title.
        /// </summary>
        /// <value>
        /// The header title.
        /// </value>
        public IWebElement HeaderTitle
        {
            get
            {
                return _headerTitle;
            }
        }

        /// <summary>
        /// Creates this instance.
        /// </summary>
        /// <returns></returns>
        public static ExclusionRecordPage Create()
        {
            Wait.WaitUntilDisplayed(SimsBy.AutomationId("well_know_action_save"));
            return new ExclusionRecordPage();
        }

        public void ClickAdd()
        {
            _addButton.ClickByJS();
        }

        #endregion


        #region Search

        /// <summary>
        /// Implements ExclusionSearchResultTile
        /// </summary>
        public class ExclusionSearchResultTile : SearchResultTileBase
        {
            /// <summary>
            /// The _type
            /// </summary>
            [FindsBy(How = How.CssSelector, Using = "[data-automation-id='resultTile']")]
            private IWebElement _exlclusionType;


            /// <summary>
            /// The _type
            /// </summary>
            [FindsBy(How = How.CssSelector, Using = "[class='search-result h2-result']")]
            private IWebElement _exlclusionDates;

            /// <summary>
            /// Gets the type.
            /// </summary>
            /// <value>
            /// The type.
            /// </value>
            public string ExclusionType
            {
                get { return _exlclusionType.Text; }
            }

            /// <summary>
            /// Gets the dates.
            /// </summary>
            /// <value>
            /// The dates.
            /// </value>
            public string ExclusionDates
            {
                get { return _exlclusionDates.Text; }
            }
        }

        /// <summary>
        /// The _search criteria
        /// </summary>
        private readonly ExclusionSearchResults _searchResults;
        /// <summary>
        /// Gets the search results.
        /// </summary>
        /// <value>
        /// The search results.
        /// </value>
        public ExclusionSearchResults SearchResults
        {
            get { return _searchResults; }
        }

        #endregion
    }
}
