using OpenQA.Selenium;
using POM.Base;
using WebDriverRunner.webdriver;
// ReSharper disable PrivateMembersMustHaveComments
#pragma warning disable 649

namespace POM.Components.Conduct.Pages.Exclusions
{
    /// <summary>
    /// Implements ExclusionSearchPage
    /// </summary>
    public class ExclusionSearchResults : SearchResultsComponent<ExclusionRecordPage.ExclusionSearchResultTile>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ExclusionSearchResults"/> class.
        /// </summary>
        /// <param name="component">The component.</param>
        public ExclusionSearchResults(BaseComponent component) : base(component)
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