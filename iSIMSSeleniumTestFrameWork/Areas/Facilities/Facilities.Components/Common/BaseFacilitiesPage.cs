using System;
using OpenQA.Selenium;
using SharedComponents.BaseFolder;
using SharedComponents.CRUD;

namespace Facilities.Components.Common
{
    public class BaseFacilitiesPage: BaseSeleniumComponents
    {
        public bool HasConfirmedSave(string expectedSaveMessage)
        {
            var actualSaveMessage = WaitForAndGet(By.CssSelector("div[data-automation-id='status_success']"));
            return string.Equals(expectedSaveMessage, actualSaveMessage.Text,
                StringComparison.InvariantCultureIgnoreCase);
        }

        public void Save()
        {
            Detail.Save();
        }
    }
}
