using System;
using System.Linq;
using System.Threading;
using OpenQA.Selenium;
using SharedComponents.BaseFolder;
using SharedComponents.Helpers;

namespace Admissions.Component
{
	public class ApplicationRecordNavigation
	{
		public void NavgateToApplications(SeleniumHelper.iSIMSUserType userType = SeleniumHelper.iSIMSUserType.SchoolAdministrator)
		{
			Console.WriteLine("Opening Application Details to Create Applicant");
			SeleniumHelper.FindAndClick("[data-automation-id='task_menu']");
			Thread.Sleep(1000);
			SeleniumHelper.FindAndClick("[data-automation-id='section_menu_Admissions']");
			Thread.Sleep(1000);
			SeleniumHelper.FindAndClick("[data-ajax-url$='Pupils/Application/Details']");
		}

        public void NavgateToApplicationsDirectly(SeleniumHelper.iSIMSUserType userType = SeleniumHelper.iSIMSUserType.SchoolAdministrator)
        {
            Console.WriteLine("Opening Application Details to Create Applicant");
            SeleniumHelper.FindAndClick("[data-automation-id='task_menu']");
            Thread.Sleep(1000);
            SeleniumHelper.FindAndClick("[data-ajax-url$='Pupils/Application/Details']");
        }

		public void SetApplicationToAdmitted()
		{
			var loc = By.Name("LearnerApplication.ApplicationStatusSelector.dropdownImitator");
			BaseSeleniumComponents.WaitForAndGet(loc).ChooseSelectorOption("Admitted");
			Thread.Sleep(1000);
			SeleniumHelper.Get(By.CssSelector(SeleniumHelper.AutomationId("well_know_action_save"))).Click();
			Thread.Sleep(5000);
            SeleniumHelper.Get(By.CssSelector(SeleniumHelper.AutomationId("save_&_continue_button"))).Click();
			Thread.Sleep(5000);
		}

		public string CreateNewApplication(string forename, string surname, string gender, string dateOfBirth, string admissionGroup)
		{
			SeleniumHelper.Get(By.CssSelector("a[title=\"Add New Application\"] > span.toolbar-text")).Click();
			Thread.Sleep(2000);
			SeleniumHelper.Get(By.Name("LegalForename")).Clear();
			SeleniumHelper.Get(By.Name("LegalForename")).SendKeys(forename);

			SeleniumHelper.Get(By.Name("LegalSurname")).Clear();
			SeleniumHelper.Get(By.Name("LegalSurname")).SendKeys(surname);

			By loc = By.Name("Gender.dropdownImitator");
			BaseSeleniumComponents.WaitForAndGet(loc);
			SeleniumHelper.Get(loc).ChooseSelectorOption(gender);

			SeleniumHelper.Get(By.Name("DateOfBirth")).Clear();
			SeleniumHelper.Get(By.Name("DateOfBirth")).SendKeys(dateOfBirth);

			SeleniumHelper.Get(By.CssSelector(SeleniumHelper.AutomationId("continue_button"))).Click();
			Thread.Sleep(1000);

			loc = By.CssSelector("div[data-section-id='dialog-detail'] [name='AdmissionGroup.dropdownImitator']");
			BaseSeleniumComponents.WaitForAndGet(loc).ChooseSelectorOption(admissionGroup);

			loc = By.Name("EnrolmentStatus.dropdownImitator");
			BaseSeleniumComponents.WaitForAndGet(loc).ChooseSelectorOption("Single Registration");

			SeleniumHelper.Get(By.CssSelector(SeleniumHelper.AutomationId("create_record_button"))).Click();
			Thread.Sleep(5000);

			var afterSpliItems = SeleniumHelper.Get(By.CssSelector(SeleniumHelper.AutomationId("well_know_action_save")))
				 .GetAttribute("data-ajax-url")
				 .Split('/')
				 .ToList();
			string learner_id = afterSpliItems.LastOrDefault();

			return learner_id;
		}
	}
}
