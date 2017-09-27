using OpenQA.Selenium;
using POM.Base;
using WebDriverRunner.webdriver;
// ReSharper disable PrivateMembersMustHaveComments
#pragma warning disable 649

namespace POM.Components.Conduct.Pages.ReportCards
{
    /// <summary>
    /// Implements RepordCardSearchPage
    /// </summary>
    public class ReportCardSearchResults : SearchResultsComponent<ReportCardRecordPage.ReportCardSearchResultTile>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ReportCardSearchResults"/> class.
        /// </summary>
        /// <param name="component">The component.</param>
        public ReportCardSearchResults(BaseComponent component) : base(component)
        {
        }

        #region Page properties

        public string SearchResultCount()
        {
            return WebContext.WebDriver.FindElement(By.CssSelector("[data-automation-id='search_results_counter']")).Text;
        }

        #endregion Page properties

        #region Action



        #endregion Action
    }
}