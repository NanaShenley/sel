using Pupil.Data;
using SeSugar.Automation;

namespace Pupil.Components
{
    public class ConductNavigation
    {
        public void NavigateToConductConfigurationFromMenu()
        {
            AutomationSugar.NavigateMenu("Tasks", "Pupil Conduct", "Conduct Configuration");
        }

        public void NavigateToAchievementEventsFromMenu()
        {
            AutomationSugar.NavigateMenu("Tasks", "Pupil Conduct", AchievementEventData.AchievementLabel + " Events");
        }

        public void NavigateToBehaviourEventsFromMenu()
        {
            AutomationSugar.NavigateMenu("Tasks", "Pupil Conduct", BehaviourEventData.BehaviourLabel + " Events");
        }

        public void NavigateToConductSummaryFromMenu()
        {
            AutomationSugar.NavigateMenu("Tasks", "Pupil Conduct", "Conduct Summary");
        }
    }
}

