using DataExchange.POM.Base;
using OpenQA.Selenium;
using SeSugar.Automation;
using OpenQA.Selenium.Support.PageObjects;
using DataExchange.POM.Helper;
using WebDriverRunner.webdriver;

namespace DataExchange.POM.Components.Census
{
    /// <summary>
    /// Create Census Dialog Page object
    /// </summary>
    public class CreateCensusDialog : BaseDialogComponent
    {
        public override By DialogIdentifier
        {
            get
            {
                 return SeSugar.Automation.SimsBy.AutomationId("create_Deni_dialog");
            }
        }

        public CreateCensusDialog()
        {
            PageFactory.InitElements(WebContext.WebDriver, this);
        }

        [FindsBy(How = How.CssSelector, Using = "form[id='dialog-editableData'] input[name='StatutoryReturnType.dropdownImitator']")]
        private IWebElement _returnTypeDropdown;

        [FindsBy(How = How.CssSelector, Using = "form[id='dialog-editableData'] input[name='StatutoryReturnVersion.dropdownImitator']")]
        private IWebElement _returnTypeVersionDropdown;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='ok_button']")]
        private IWebElement _okButton;

        public string ReturnTypeDropdown
        {
            get { return _returnTypeDropdown.GetAttribute("value"); }
            set { _returnTypeDropdown.EnterForDropDown(value); }
        }
        
        public string ReturnTypeVersionDropdown
        {
            get { return _returnTypeVersionDropdown.GetAttribute("value"); }
            set { _returnTypeVersionDropdown.EnterForDropDown(value); }
        }

        public CensusPage OKButton()
        {
            _okButton.Click();
            AutomationSugar.WaitForAjaxCompletion();           
            return new CensusPage();
        }       
    }
}
