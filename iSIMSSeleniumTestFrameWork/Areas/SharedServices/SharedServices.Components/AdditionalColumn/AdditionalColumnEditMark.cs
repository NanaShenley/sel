using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using SharedComponents.BaseFolder;
using SharedComponents.Helpers;
using SharedServices.Components.Common;
using TestSettings;
using WebDriverRunner.webdriver;
using SeSugar.Automation;

namespace SharedServices.Components.AdditionalColumn
{
    public class AdditionalColumnEditMark : BaseSeleniumComponents
    {        
        public AdditionalColumnEditMark()
        {
            AutomationSugar.WaitForAjaxCompletion();
            PageFactory.InitElements(WebContext.WebDriver, this);
        }
        
        /// <summary>
        /// Returns the count of additional columns for class teacher
        /// </summary>
        /// <returns></returns>
        public int GetDialogAdditionalColumnCount()
        {
            return EditMarkProcess();
        }

        /// <summary>
        /// Opens up the additional coulumns dialog box and counts the no. of checkboxes visible
        /// </summary>
        /// <returns></returns>
        private static int EditMarkProcess()
        {
            AutomationSugarHelpers.WaitForAndClickOn(SharedServicesElements.CommonElements.WholeSchool);
            
            AutomationSugar.WaitForAjaxCompletion();

            AutomationSugarHelpers.WaitForAndClickOn(SharedServicesElements.EditMark.SearchButton);

            AutomationSugarHelpers.WaitForAndClickOn(SharedServicesElements.CommonElements.AdditionalColumnButton);

            WaitUntilDisplayed(By.CssSelector(SharedServicesElements.EditMark.Attendances));
            WaitForAndClick(BrowserDefaults.TimeOut, By.CssSelector(SharedServicesElements.EditMark.Attendances));

            var parent = SeleniumHelper.Get(SharedServicesElements.EditMark.EditableColumnTreeNode);
            var checkBoxList = parent.FindElements(SharedServicesElements.EditMark.CheckBox);

            WebContext.Screenshot();

            AutomationSugarHelpers.WaitForAndClickOn(SharedServicesElements.EditMark.OkButton);
            
            return checkBoxList.Count;
        }
    }
}


