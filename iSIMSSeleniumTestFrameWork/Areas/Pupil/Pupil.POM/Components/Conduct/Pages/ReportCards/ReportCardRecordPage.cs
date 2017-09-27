using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using POM.Base;
using POM.Helper;
using WebDriverRunner.webdriver;
using SimsBy = POM.Helper.SimsBy;
// ReSharper disable ConvertPropertyToExpressionBody
#pragma warning disable 169
#pragma warning disable 649


namespace POM.Components.Conduct.Pages.ReportCards
{
    /// <summary>
    /// Implements ReportCardRecordPage
    /// </summary>
    public class ReportCardRecordPage : BaseComponent
    {


        /// <summary>
        /// Initializes a new instance of the <see cref="ReportCardTriplet" /> class.
        /// </summary>
        public ReportCardRecordPage()
        {
            _searchResults= new ReportCardSearchResults(this);
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
        /// The sub slider toggle button
        /// </summary>
        [FindsBy(How = How.CssSelector, Using = "[data-subslider-control]")]
        private IWebElement _toggleSearchResultButton;

        [FindsBy(How = How.CssSelector, Using = "input[name='SliderState']")]
        private IWebElement _sliderState;

        /// <summary>
        /// The _header title
        /// </summary>
        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='report_card_header_display_name']")]
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

        public IWebElement ToggleSearchResultButton {
            get { return _toggleSearchResultButton; }
        }

        public string GetSliderState()
        {
            Wait.WaitUntilDisplayed(SimsBy.CssSelector("[data-subslider-control]"));
            return WebContext.WebDriver.FindElement(By.CssSelector("input[name='SliderState']")).GetValue();
        }

        /// <summary>
        /// Creates this instance.
        /// </summary>
        /// <returns></returns>
        public static ReportCardRecordPage Create()
        {
            Wait.WaitUntilDisplayed(SimsBy.AutomationId("well_know_action_save"));
            return new ReportCardRecordPage();
        }

        public void ClickAdd()
        {
            _addButton.ClickByJS();
        }

        #endregion


        #region Search

        /// <summary>
        /// Implements ReportCardSearchResultTile
        /// </summary>
        public class ReportCardSearchResultTile : SearchResultTileBase
        {
            /// <summary>
            /// The _type
            /// </summary>
            [FindsBy(How = How.CssSelector, Using = "[data-automation-id='resultTile']")]
            private IWebElement _reportCardType;


            /// <summary>
            /// The _type
            /// </summary>
            [FindsBy(How = How.CssSelector, Using = "[class='search-result h2-result']")]
            private IWebElement _reportCardDates;

            /// <summary>
            /// Gets the type.
            /// </summary>
            /// <value>
            /// The type.
            /// </value>
            public string ReportCardType
            {
                get { return _reportCardType.Text; }
            }

            /// <summary>
            /// Gets the dates.
            /// </summary>
            /// <value>
            /// The dates.
            /// </value>
            public string ReportCardDates
            {
                get { return _reportCardDates.Text; }
            }
        }

        /// <summary>
        /// The _search criteria
        /// </summary>
        private readonly ReportCardSearchResults _searchResults;
        /// <summary>
        /// Gets the search results.
        /// </summary>
        /// <value>
        /// The search results.
        /// </value>
        public ReportCardSearchResults SearchResults
        {
            get { return _searchResults; }
        }

        #endregion
    }
}
