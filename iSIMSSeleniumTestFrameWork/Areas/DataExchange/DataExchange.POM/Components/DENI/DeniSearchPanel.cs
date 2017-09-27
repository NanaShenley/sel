using DataExchange.POM.Base;
using DataExchange.POM.Helper;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using OpenQA.Selenium.Support.UI;
using System;
using WebDriverRunner.webdriver;

namespace DataExchange.POM.Components.DENI
{
    /// <summary>
    /// Deni Search Panel Page object
    /// </summary>
    public class DeniSearchPanel : SearchCriteriaComponent<DeniTripletPage.DeniSearchResultTile>
    {
        public DeniSearchPanel(BaseComponent parent) : base(parent) { }

            [FindsBy(How = How.CssSelector, Using = "form[data-section-id='searchCriteria'] input[name='StatutoryReturnType.dropdownImitator']")]
            private IWebElement _returnTypeDropdown;

            [FindsBy(How = How.CssSelector, Using = "form[data-section-id='searchCriteria'] input[name='StatutoryReturnVersion.dropdownImitator']")]
            private IWebElement _returnTypeVersionDropdown;

            WebDriverWait waiter = new WebDriverWait(WebContext.WebDriver, TimeSpan.FromSeconds(30));

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

        }
    }
