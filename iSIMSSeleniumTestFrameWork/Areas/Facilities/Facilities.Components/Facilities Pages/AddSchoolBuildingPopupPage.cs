using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using SharedComponents.BaseFolder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebDriverRunner.webdriver;
using SharedComponents.Helpers;
using Facilities.Components.Common;
using TestSettings;
using System.Threading;

namespace Facilities.Components.Facilities_Pages
{
    public class AddSchoolBuildingPopupPage : BaseSeleniumComponents
  
    {
#pragma warning disable 0649
     [FindsBy(How = How.Id, Using = "dialog-dialog-editableData")]
     private readonly IWebElement _main;
     [FindsBy(How = How.CssSelector, Using = "button[data-automation-id='ok_button']")]
     private IWebElement _OKButtonbldng;
     [FindsBy(How = How.CssSelector, Using = "[data-section-id='dialog-dialog-detail'] [data-automation-id='add_new_button']")]
     private readonly IWebElement _addnewaddress;

     public AddSchoolBuildingPopupPage()
        {
            PageFactory.InitElements(WebContext.WebDriver, this);
            
        }
        public IWebElement Shortname
        {
            get
            {
                return _main.FindElementSafe(By.Name("BuildingShortName"));
            }
        }
        public IWebElement Longname
        {
            get
            {
                return _main.FindElementSafe(By.Name("BuildingName"));
            }
        }
        public IWebElement Telephonenumber
        {
            get
            {
                return _main.FindElementSafe(By.Name("TelephoneNumber"));
            }
        }
        public IWebElement Faxnumber
        {
            get
            {
                return _main.FindElementSafe(By.Name("FaxNumber"));
            }
        }
        public IWebElement Emailaddress
        {
            get
            {
                return _main.FindElementSafe(By.Name("EmailAddress"));
            }
        }

     public void EnterSiteShortName(string siteshortname)
        {
            Shortname.Clear();
            Shortname.SendKeys(siteshortname);
        }
        public void EnterSiteLongName(string siteLongname)
        {
            Longname.Clear();
            Longname.SendKeys(siteLongname);
        }
        public void ClickOkButtonBldng()
        {
            _OKButtonbldng.Click();
        }
          public AddBuildingAddressPopupPage ClickAddNewAddress()
        {
            _addnewaddress.Click();
            WaitUntilDisplayed(MySchoolDetailsElements.AddAddressPopupTitle);
            return new AddBuildingAddressPopupPage();
        }
 }
}