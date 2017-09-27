using System;
using DataExchange.POM.Base;
using OpenQA.Selenium;
using SeSugar.Automation;
using OpenQA.Selenium.Support.PageObjects;
using DataExchange.POM.Helper;
using OpenQA.Selenium.Support.UI;
using WebDriverRunner.webdriver;

namespace DataExchange.POM.Components.DENI
{
    /// <summary>
    /// Create Deni Dialog Page object
    /// </summary>
    public class CreateDeniDialog : BaseDialogComponent
    {
            public override By DialogIdentifier
            {
                get
                {
                // return SeSugar.Automation.SimsBy.AutomationId("create_Deni_dialog");
                return By.Id("screen");
            }
            }

            public CreateDeniDialog()
            {
                PageFactory.InitElements(WebContext.WebDriver, this);
            }

            [FindsBy(How = How.CssSelector, Using = "form[data-section-id='searchCriteria'] input[name='StatutoryReturnType.dropdownImitator']")]
            private IWebElement _returnTypeDropdown;

            [FindsBy(How = How.CssSelector, Using = "form[data-section-id='searchCriteria'] input[name='StatutoryReturnVersion.dropdownImitator']")]
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


        public DeniTripletPage OKButton()
        {
            var okButton = By.CssSelector("[data-automation-id='ok_button']");
            WebDriverWait wait = new WebDriverWait(WebContext.WebDriver, TimeSpan.FromSeconds(10));
            wait.Until(ExpectedConditions.ElementIsVisible(okButton)).Click();
            AutomationSugar.WaitForAjaxCompletion();
            return new DeniTripletPage();
        }
    }
    }
