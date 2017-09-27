using OpenQA.Selenium;
using SharedComponents.BaseFolder;
using SharedComponents.Helpers;
using SharedServices.Components.Common;
using System.Collections.Generic;
using SeSugar.Automation;

namespace SharedServices.Components.AdditionalColumn
{
    public class AllocateFuturePupil : BaseSeleniumComponents
    {
        public AllocateFuturePupil()
        {
            WaitUntillAjaxRequestCompleted();
        }

        public void SearchMenu(string searchText)
        {
            SeleniumHelper.FindAndClick(SharedServicesElements.Menu.MenuButton);
            IWebElement globalSearchInput = SeleniumHelper.Get(By.Id("task-menu-search"));

            globalSearchInput.SendKeys("allocate future pupils");
            SeleniumHelper.FindAndClick(SharedServicesElements.AdditionalColumns.FuturePupilsGlobalSearchItem);
        }

        public int GetDialogAdditionalColumnCount()
        {
            var searchCriteria = SeleniumHelper.Get(SharedServicesElements.CommonElements.SearchCriteria);
            searchCriteria.FindCheckBoxAndClick("Year Group", new List<string> { "Year 1", "Year 3"  });

            AutomationSugar.ClickOn(SharedServicesElements.CommonElements.SearchButton);
            AutomationSugarHelpers.WaitForAndClickOn(SharedServicesElements.CommonElements.AdditionalColumnButton);
            AutomationSugar.WaitFor(SharedServicesElements.CommonElements.EditableColumnTreeNode);

            IWebElement additionalColumns = SeleniumHelper.Get(SharedServicesElements.CommonElements.EditableColumnTreeNode);

            return additionalColumns.FindElements(By.CssSelector(SharedServicesElements.CommonElements.WebixCheckBoxCss)).Count;
        }
    }
}


