using SharedComponents.Helpers;

namespace SharedComponents.HomePages
{
    /// <summary>
    /// Implements TabActions
    /// </summary>
    public class TabActions
    {
        /// <summary>
        /// Clicks the tab item
        /// </summary>
        /// <param name="link">unique tab identifier</param>
        public static void ClickTabItem(string link)
        {
            var cssFormat = SeleniumHelper.AutomationId(link);
            SeleniumHelper.FindAndClick(cssFormat);
        }
    }
}