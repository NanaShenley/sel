using System.Collections.ObjectModel;
using System.Linq;
using OpenQA.Selenium;
using SharedComponents.Helpers;

namespace SharedServices.Components.Common
{
    public class CommonHelpers
    {
        public bool VerifyDropdownContainsValue(string dropdownSelector, string value)
        {
            var dropdownOptions = RetrieveDropdownOptions(dropdownSelector);
            return dropdownOptions != null && dropdownOptions.Any(option => option.GetValue().EndsWith(":" + value));
        }

        private static ReadOnlyCollection<IWebElement> RetrieveDropdownOptions(string elementSelector)
        {
            var element = SeleniumHelper.Get(By.Name(elementSelector));
            var options = element.FindElements(By.CssSelector("option"));
            return options;
        }
    }
}
