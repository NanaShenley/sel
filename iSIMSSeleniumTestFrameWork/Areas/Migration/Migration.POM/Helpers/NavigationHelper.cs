
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System;
using System.Linq;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.Threading;
using OpenQA.Selenium.Remote;
using WebDriverRunner.webdriver;

namespace Migration.POM.Helpers
{


    public class NavigationHelper
    {
        public static void NavigateToMangeUpgrades()
        {
            NavigateTo("task_menu", "section_menu_Upgrades", "task_menu_section_onboarding_manageonboardings");
        }

        public static void NavigateTo(string menuLevelAutomationId, string sectionAutomationId, string subSectionAutomationId)
        {
            AttributeHelper.WaitForClickByAttribute("button", "data-automation-id", menuLevelAutomationId, 30000);
            AttributeHelper.WaitForClickByAttribute("a", "data-automation-id", sectionAutomationId, 30000);
            AttributeHelper.WaitForClickByAttribute("a", "data-automation-id", subSectionAutomationId, 30000);
        }

    }
}
