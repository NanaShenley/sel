using PageObjectModel.Helper;

namespace PageObjectModel.Components.HomePages
{
    /// <summary>
    /// Implements selenium helper for interacting with sub menu items eg: Bulk Update Sub menus
    /// </summary>
    public class SubMenuActions
    {
        /// <summary>
        /// Click sub menu item
        /// </summary>
        /// <param name="link">menu item unique id</param>
        public static void ClickMenuItem(string link)
        {
            var cssFormat = SeleniumHelper.AutomationId(link);
            SeleniumHelper.FindAndClick(cssFormat);
        }
    }
}