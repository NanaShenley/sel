using System.Linq;
using System.Threading;
using OpenQA.Selenium;
using SharedComponents.BaseFolder;
using SharedComponents.CRUD;
using SharedComponents.Helpers;
using WebDriverRunner.webdriver;

namespace Admissions.Component
{
	public class SchoolIntakeRecordNavigation
	{
		public void NavgateToSchoolIntake(SeleniumHelper.iSIMSUserType userType = SeleniumHelper.iSIMSUserType.SchoolAdministrator)
		{
            Thread.Sleep(1000);
            SeleniumHelper.FindAndClick("[data-automation-id='task_menu']");
			Thread.Sleep(2000);
			SeleniumHelper.FindAndClick("[data-automation-id='section_menu_Admissions']");
			Thread.Sleep(1000);
			SeleniumHelper.FindAndClick(
				 "a[data-ajax-url='/iSIMSMVCClientFarm1/School/SchoolIntakeMaintenance/Details']");

		}

		public void NavgateToSchoolIntakeDirectly(SeleniumHelper.iSIMSUserType userType = SeleniumHelper.iSIMSUserType.SchoolAdministrator)
		{
			Thread.Sleep(1000);
            SeleniumHelper.FindAndClick("[data-automation-id='task_menu']");
            Thread.Sleep(1000);
			SeleniumHelper.FindAndClick(
				 "[data-ajax-url='/iSIMSMVCClientFarm1/School/SchoolIntakeMaintenance/Details']");
		}

		public string CreateNewSchoolIntake(string admissionYear, string admissionTerm, string yearGroup, string plannedAdmissions, string schoolIntakeName, string admissionGroupName, string dateOfAdmission, string capacity)
		{
			// Intake Group
			Thread.Sleep(1000);
			SeleniumHelper.Get(By.CssSelector(SeleniumHelper.AutomationId("add_button"))).Click();
			Thread.Sleep(1000);

			var loc = By.CssSelector("div[data-section-id='IntakeDetailsDependenciesSection'] [name='YearOfAdmission.dropdownImitator']");
			BaseSeleniumComponents.WaitForAndGet(loc).ChooseSelectorOption(admissionYear);
			Thread.Sleep(1000);

			loc = By.Name("AdmissionTerm.dropdownImitator");
			BaseSeleniumComponents.WaitForAndGet(loc).ChooseSelectorOption(admissionTerm);
			Thread.Sleep(1000);

			loc = By.CssSelector("div[data-section-id='IntakeDetailsDependenciesSection'] [name='YearGroup.dropdownImitator']");
			BaseSeleniumComponents.WaitForAndGet(loc).ChooseSelectorOption(yearGroup);
			Thread.Sleep(1000);

			var plannedAdmissionNumber = SeleniumHelper.Get(By.Name("PlannedAdmissionNumber"));
			plannedAdmissionNumber.Clear();
			plannedAdmissionNumber.SendKeys(plannedAdmissions);

			Thread.Sleep(1000);
			loc = By.CssSelector("div[data-section-id='IntakeDetailsDependenciesSection'] [name='Name']");
			var name = SeleniumHelper.Get(loc);
			name.Clear();
			name.SendKeys(schoolIntakeName);


			// Admission Group
			Thread.Sleep(1000);
			loc = By.CssSelector("input[name$='.Name'");
			var admissionGroupGridName = SeleniumHelper.Get(loc);
			admissionGroupGridName.Clear();
			admissionGroupGridName.SendKeys(admissionGroupName);

			loc = By.CssSelector("input[name$='.DateOfAdmission'");
			var admissionGroupGridDateOfAdmission = SeleniumHelper.Get(loc);
			admissionGroupGridDateOfAdmission.Clear();
			admissionGroupGridDateOfAdmission.SendKeys(dateOfAdmission);

			loc = By.CssSelector("input[name$='.Capacity'");
			var admissionGroupGridCapacity = SeleniumHelper.Get(loc);
			admissionGroupGridCapacity.Clear();
			admissionGroupGridCapacity.SendKeys(capacity);

			Thread.Sleep(1000);
			SeleniumHelper.Get(By.CssSelector(SeleniumHelper.AutomationId("well_know_action_save"))).Click();

			// New confirmation dialog box
			Thread.Sleep(2000);
			SeleniumHelper.Get(By.CssSelector(SeleniumHelper.AutomationId("yes,_update_any_associated_applications_button")))
				.Click();

			var afterSpliItems = SeleniumHelper.Get(By.CssSelector(SeleniumHelper.AutomationId("well_know_action_save")))
				 .GetAttribute("data-ajax-url")
				 .Split('/')
				 .ToList();
			string schoolIntakeId = afterSpliItems.LastOrDefault();

			return schoolIntakeId;
		}

        public void DeleteSchoolIntake(string schoolIntakeId, string admissionYear)
		{
			NavgateToSchoolIntakeDirectly();

			Thread.Sleep(1000);
            var loc = By.CssSelector("form[data-section-id='searchCriteria'] [name='YearOfAdmission.dropdownImitator']");
            BaseSeleniumComponents.WaitForAndGet(loc).ChooseSelectorOption(admissionYear);
            Thread.Sleep(1000);
			SearchCriteria.Search();
			SearchResults.WaitForResults();
			Thread.Sleep(1000);
			SearchResults.Click(schoolIntakeId);
			Thread.Sleep(2000);
            SeleniumHelper.Get(By.CssSelector("[data-ajax-url*='/iSIMSMVCClientFarm1/School/SchoolIntakeMaintenance/ConfirmDeleteDetail']")).Click();
			Thread.Sleep(2000);
			SeleniumHelper.Get(By.CssSelector(SeleniumHelper.AutomationId("continue_with_delete_button"))).Click();
			Thread.Sleep(2000);
		}
	}
}