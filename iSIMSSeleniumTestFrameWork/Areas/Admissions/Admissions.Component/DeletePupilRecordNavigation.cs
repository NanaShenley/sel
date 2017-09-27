using System.Threading;
using OpenQA.Selenium;
using SharedComponents.CRUD;
using SharedComponents.Helpers;

namespace Admissions.Component
{
    public class DeletePupilRecordNavigation
    {
        public void NavigateToDeletePupil()
        {
            Thread.Sleep(1000);
            SeleniumHelper.FindAndClick("[data-automation-id='task_menu']");
            Thread.Sleep(2000);
            SeleniumHelper.FindAndClick("[data-automation-id='section_menu_Pupils']");
            Thread.Sleep(2000);
            SeleniumHelper.FindAndClick(
                "[data-ajax-url*='/Pupils/SIMS8DeletePupilScreenLearner/Details']");
        }

        public void DeleteLearner(string learnerId)
        {
            NavigateToDeletePupil();

            Thread.Sleep(1000);
            SearchCriteria.Search();
            SearchResults.WaitForResults();
            Thread.Sleep(2000);
            SearchResults.Click(learnerId);
            Thread.Sleep(2000);
            SeleniumHelper.Get(By.CssSelector("[data-ajax-url*='/Pupils/SIMS8DeletePupilScreenLearner/ConfirmDeleteDetail']")).Click();
            Thread.Sleep(2000);
            SeleniumHelper.Get(By.CssSelector(SeleniumHelper.AutomationId("continue_with_delete_button"))).Click();
            Thread.Sleep(3000);
        }
    }
}
