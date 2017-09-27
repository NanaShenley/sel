using OpenQA.Selenium;

using SharedComponents.Helpers;

namespace Pupil.Components.Common
{
	public struct PupilRecordElements
	{
		public struct Menu
		{
            public static By PupilRecordsMenuItem = By.CssSelector(SeleniumHelper.AutomationId("task_menu_section_pupils_pupil_record"));
		}

		public struct PupilRecord
		{
			public struct Search
			{
				public static By SearchCriteria = By.CssSelector(SeleniumHelper.AutomationId("search_criteria"));
				public static By SearchButton = By.CssSelector(SeleniumHelper.AutomationId("search_criteria_submit"));
			}

			public struct Detail
			{
                public static By SearchResults = By.CssSelector("[data-section-id='searchResults']");
                public static By LegalName = By.CssSelector("[name='LegalForename']");
			}
		}
	}
}
