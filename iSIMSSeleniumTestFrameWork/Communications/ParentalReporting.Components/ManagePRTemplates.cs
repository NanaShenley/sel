using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharedComponents.BaseFolder;
using WebDriverRunner.webdriver;
using TestSettings;
using OpenQA.Selenium.Support.PageObjects;
using OpenQA.Selenium;
using Attendance.POM.DataHelper;
using POM.Helper;
using System.Threading;
using SeSugar.Automation;
using OpenQA.Selenium.Interactions;


namespace ParentalReporting.Components
{
    public class ManagePRTemplates
    {
        #region Page Properties
        [FindsBy(How = How.CssSelector, Using = "a[data-automation-id='add_button']")]
        private IWebElement _addButton;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='well_know_action_save']")]
        private IWebElement _saveButton;

        [FindsBy(How = How.Name, Using = "NameOfTemplate")]
        private IWebElement _templateName;

        public void ClickAdd()
        {
            AutomationSugar.WaitFor(new ByChained(SeSugar.Automation.SimsBy.AutomationId("add_button")));
            AutomationSugar.ClickOn(new ByChained(SeSugar.Automation.SimsBy.AutomationId("add_button")));
            //AutomationSugar.WaitForAjaxCompletion();
        }

        public void SetTemplateName(String name)
        {
            SeleniumHelper.Get(By.Name("NameOfTemplate")).Clear();
            //SeleniumHelper.Get(By.Name("NameOfTemplate")).SendKeys(name);
            IWebElement temp = SeleniumHelper.Get(By.Name("NameOfTemplate"));
            

            
            Actions action = new Actions(WebContext.WebDriver);
            foreach(char ch in name)
            {
                action.SendKeys(ch.ToString()).Perform();
            }

            
            //SeleniumHelper.Get(By.Name("NameOfTemplate")).
        }

        public void SavePRTemplate()
        {
            IWebElement _saveButton = WebContext.WebDriver.FindElement(By.CssSelector("[data-automation-id='save_button']"));

          //  SeleniumHelper.Get(By.CssSelector("[data-automation-id='well_know_action_save']")).Click();
            _saveButton.Click();
        }

        #endregion

    }
}
