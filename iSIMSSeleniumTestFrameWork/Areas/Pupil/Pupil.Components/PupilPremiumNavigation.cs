using OpenQA.Selenium;
using SeSugar;
using SeSugar.Automation;

namespace Pupil.Components
{
    public class PupilPremiumNavigation
    {
        /// <summary>
        /// Navigates to Pupil Premium page from Pupil Task Menu.
        /// </summary>
        public void NavigateToPupilPremiumFromMenu()
        {
            NavigateMenu("Tasks", "Pupils", "Pupil Premium");
        }

        /// <summary>
        /// Navigates to the page using the side menu
        /// </summary>
        /// <param name="topLevel">e.g.: Tasks or Lookups</param>
        /// <param name="category">e.g.: Pupils</param>
        /// <param name="item">e.g: Service Terms</param>
        public static void NavigateMenu(string topLevel, string category, string item)
        {
            var jsExecutor = (IJavaScriptExecutor)Environment.WebContext.WebDriver;

            var topLevelFilter = CreateJsFilter("h2", topLevel);
            var categoryFilter = CreateJsFilter("[data-accordian-panel-header-title]", category);
            var itemFilter = CreateJsFilter("a", item);

            var menuUrlJsFormat = $"$('#task-menu').{topLevelFilter}.next().{categoryFilter}.closest('.panel-heading').next().{itemFilter}.data('ajaxUrl')";
            var navigationJsFormat = $"sims_commander.OpenPage('#task-menu',{menuUrlJsFormat}, '')";

            Retry.Do(() => { jsExecutor.ExecuteScript(navigationJsFormat); });

            AutomationSugar.WaitForAjaxCompletion();
        }

        private static string CreateJsFilter(string selector, string filterText)
        {
            return $"find('{selector}:contains(\"{filterText}\")').filter(function(index){{return $(this).text().trim() === \"{filterText}\";}})";
        }
    }
}

