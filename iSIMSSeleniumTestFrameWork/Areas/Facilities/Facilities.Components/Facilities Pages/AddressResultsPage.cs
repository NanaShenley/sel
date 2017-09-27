using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using SharedComponents.BaseFolder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebDriverRunner.webdriver;

namespace Facilities.Components.Facilities_Pages
{
    public class AddressResultsPage
    {
        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='dialog-dialog-dialog-searchResults']")]
        public readonly IWebElement ResultTile;

        public AddressResultsPage()
        {

            PageFactory.InitElements(WebContext.WebDriver, this);
        
        }
    }
}