using OpenQA.Selenium;
using SeSugar.Automation;
using SeleniumHelper = SharedComponents.Helpers.SeleniumHelper;

namespace Pupil.Components
{
    public class ClassLogNavigation
    {
        /// <summary>
        /// Navigates to Class Log page from Pupil Menu.
        /// </summary>
        public void NavigateToPupilClassLogFromMenu()
        {
            AutomationSugar.NavigateMenu("Tasks", "Pupils", "Class Log");
        }

        /// <summary>
        /// Navigates to Class Log page from Pupil Menu.
        /// </summary>
        public void NavigateToPupilClassLogFromQuickLink()
        {
            SeleniumHelper.FindAndClick(By.CssSelector(SeleniumHelper.AutomationId("quicklinks_top_level_pupil_submenu_classlog")));
            AutomationSugar.WaitForAjaxCompletion();
        }

    }
}

