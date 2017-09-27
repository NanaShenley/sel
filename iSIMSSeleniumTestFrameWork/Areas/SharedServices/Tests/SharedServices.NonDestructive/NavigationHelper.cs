using SeSugar.Automation;
using SharedComponents.Helpers;
using SharedServices.Components.Common;

namespace SharedServices.NonDestructive
{
    public static class NavigationHelper
    {
        public const string MenuId = "task_menu_section_ss_variant";

        public static void Login(SeleniumHelper.iSIMSUserType userType = SeleniumHelper.iSIMSUserType.SchoolAdministrator)
        {
            SeleniumHelper.Login(userType, enabledFeatures: Constants.SeleniumOnlyFeature);
            AutomationSugar.WaitForAjaxCompletion();
        }

        public static void NavigateToTestPage(SeleniumHelper.iSIMSUserType userType = SeleniumHelper.iSIMSUserType.SchoolAdministrator)
        {
            Login(userType);
            SeleniumHelper.NavigateMenu(MenuId);
        }
    }
}
