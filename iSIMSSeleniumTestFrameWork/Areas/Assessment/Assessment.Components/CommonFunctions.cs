using SeSugar.Automation;

namespace Assessment.Components
{
    public static class CommonFunctions
    {
        public static void GotToMarksheetMenu()
        {
            AutomationSugar.NavigateMenu("Tasks", "Assessment", "Marksheets");
        }
    }
}
