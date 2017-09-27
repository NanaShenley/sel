using iSIMSSeleniumHelper = SharedComponents.SeleniumHelperObsolete;

namespace SharedComponents.CRUD
{
    public static class SearchCriteria
    {
        public const string SearchCriteriaName = "search_criteria";

        public static string SearchCriteriaSection
        {
            get { return iSIMSSeleniumHelper.AutomationId(SearchCriteriaName); }
        }

        public static void SetCriteria(string property, object value)
        {
            var css = string.Format("{0} [name='{1}']", SearchCriteriaSection, property);

            ControlsHelper.UpdateValue(css, value);
        }

        public static void ShowAdvanced()
        {
            iSIMSSeleniumHelper.FindAndClick(iSIMSSeleniumHelper.AutomationId("search_criteria_advanced"));
        }

        public static void Search()
        {
            iSIMSSeleniumHelper.FindAndClick(iSIMSSeleniumHelper.AutomationId("search_criteria_submit"));
        }
    }
}