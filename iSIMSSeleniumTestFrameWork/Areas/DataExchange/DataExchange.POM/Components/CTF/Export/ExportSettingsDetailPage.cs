using DataExchange.POM.Base;
using DataExchange.POM.Helper;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using OpenQA.Selenium.Support.UI;

namespace DataExchange.POM.Components.CTF.Export
{
    public class ExportSettingsDetailPage : BaseComponent
    {
        public override By ComponentIdentifier
        {
            get { return By.Id("screen");}
        }


        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='section_menu_CTF Alternative Destination']")]
        private IWebElement _ctfAlternativeDestinationPanel;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='add_alternative_destination_button']")]
        private IWebElement _addButton;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='well_know_action_save']")]
        private IWebElement _saveButton;
       
        public IWebElement CTFAlternativeDestinationPanel
        {
            get
            {
                WebDriverWait wait = new WebDriverWait(SeSugar.Environment.WebContext.WebDriver, SeSugar.Environment.Settings.ElementRetrievalTimeout);
                wait.Until(ExpectedConditions.ElementToBeClickable(_ctfAlternativeDestinationPanel));
                return _ctfAlternativeDestinationPanel;
            }
        }

        /// <summary>
        /// Check if Panel Exist
        /// </summary>
        /// <returns></returns>
        public bool IsSectionExist()
        {
            return CTFAlternativeDestinationPanel.IsElementDisplayed();
        }

        public void AddButton()
        {
            WebDriverWait wait = new WebDriverWait(SeSugar.Environment.WebContext.WebDriver, SeSugar.Environment.Settings.ElementRetrievalTimeout);
            wait.Until(ExpectedConditions.ElementToBeClickable(_addButton));
            _addButton.Click();
        }
      
        public class CtfAlternativeDestinationRow : GridRow
        {
            [FindsBy(How = How.CssSelector, Using = "[name$='.Code']")]
            private IWebElement _code;

            [FindsBy(How = How.CssSelector, Using = "[name$='.Description']")]
            private IWebElement _description;
          
            public string Code
            {
                set
                {
                    _code.SetText(value);
                    Wait.WaitForAjaxReady(By.CssSelector(".locking-mask-loading"));
                }
                get { return _code.GetAttribute("value"); }
            }

            public string Description
            {
                set
                {
                    _description.SetText(value);
                    Wait.WaitForAjaxReady(By.CssSelector(".locking-mask-loading"));
                }
                get { return _code.GetAttribute("value"); }
            }
        }

        public GridComponent<CtfAlternativeDestinationRow> CtfAlternativeDestinationGrid
        {
            get
            {
                GridComponent<CtfAlternativeDestinationRow> returnValue = null;
                Retry.Do(() =>
                {
                    returnValue = new GridComponent<CtfAlternativeDestinationRow>(By.CssSelector("[data-maintenance-container='CtfAlternativeDestinations']"), ComponentIdentifier);
                });
                return returnValue;
            }
        }

        public void SaveRecords()
        {
            Wait.WaitUntilDisplayed(By.CssSelector("[data-automation-id='well_know_action_save']"));
            _saveButton.ClickByJS();
            Wait.WaitForAjaxReady(By.CssSelector(".locking-mask")); 
            Refresh();        
        }
    }
}
