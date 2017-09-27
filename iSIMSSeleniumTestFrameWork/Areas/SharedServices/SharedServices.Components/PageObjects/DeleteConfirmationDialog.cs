using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using SharedComponents.BaseFolder;
using SharedComponents.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebDriverRunner.webdriver;

namespace SharedServices.Components.PageObjects
{
    public class DeleteConfirmationDialog : BaseSeleniumComponents
    {

        public DeleteConfirmationDialog()
        {
            PageFactory.InitElements(WebContext.WebDriver, this);
        }

        private const string DeleteYesButton = "Yes_button";

        public IWebElement YesButton
        {
            get { return SeleniumHelper.Get(SeleniumHelper.SelectByDataAutomationID(DeleteYesButton)); }
        }

        public void ClickYesButton()
        {
            WaitForElement(By.CssSelector(SeleniumHelper.AutomationId(DeleteYesButton)));
          
            YesButton.Click();

        }

    }
}
