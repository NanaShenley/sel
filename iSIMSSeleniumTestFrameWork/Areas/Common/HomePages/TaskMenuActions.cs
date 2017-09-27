using SharedComponents.BaseFolder;
using SharedComponents.Helpers;

namespace SharedComponents.HomePages
{
    public class TaskMenuActions : BaseSeleniumComponents
    {
        public string SectionID { get; set; }

        public string ChildLinkID { get; set; }

        public const string TaskMenuId = "#task-menu";

        public static void OpenMenuSection(string section)
        {
            var cssFormat = string.Format("{0} {1}", TaskMenuId, SeleniumHelper.AutomationId(section));

            SeleniumHelper.FindAndClick(cssFormat);
        }

        public static void ClickMenuItem(string link)
        {
            var cssFormat = string.Format("{0} {1}", TaskMenuId, SeleniumHelper.AutomationId(link));

            SeleniumHelper.FindAndClick(cssFormat);
        }

        public void OpenSection()
        {
            SeleniumHelper.WaitForElementClickableThenClick(SeleniumHelper.SelectByDataAutomationID(SectionID));
            WaitForElement(SeleniumHelper.SelectByDataAutomationID(ChildLinkID));
        }

        public void CloseSection()
        {
            SeleniumHelper.WaitForElementClickableThenClick(SeleniumHelper.SelectByDataAutomationID(SectionID));
        }

        public void ClickSection()
        {
            WaitUntilDisplayed(SeleniumHelper.SelectByDataAutomationID(SectionID));
            SeleniumHelper.WaitForElementClickableThenClick(SeleniumHelper.SelectByDataAutomationID(SectionID));
            WaitForElement(SeleniumHelper.SelectByDataAutomationID(ChildLinkID));
        }

        public void ClickChildLink()
        {
            WaitUntilDisplayed(SeleniumHelper.SelectByDataAutomationID(ChildLinkID));
            SeleniumHelper.WaitForElementClickableThenClick(SeleniumHelper.SelectByDataAutomationID(ChildLinkID));
        }
    }
}