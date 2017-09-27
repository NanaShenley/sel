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
    public class AddSchoolSitepopupPage : BaseSeleniumComponents
    {
#pragma warning disable 0649
        [FindsBy(How = How.Id, Using = "dialog-editableData")]
        private readonly IWebElement _main;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='ok_button']")]
        private readonly IWebElement _OKButton;
             [FindsBy(How = How.CssSelector, Using = "[data-section-id='dialog-detail'] [data-automation-id='add_new_button']")]
     private readonly IWebElement _addsiteaddressbutton;


        public AddSchoolSitepopupPage()
        {
            PageFactory.InitElements(WebContext.WebDriver, this);
            WaitUntilDisplayed(By.Name("ShortName"));
        }

        public IWebElement SiteShortName
        {
            get
            {
                return _main.FindElementSafe(By.Name("ShortName"));
            }
        }
        public IWebElement SiteLongName
        {
            get
            {
                return _main.FindElementSafe(By.Name("Name"));
            }
        }

        public IWebElement SiteContactName
        {
            get
            {
                return _main.FindElementSafe(By.Name("SiteContactName"));
            }
        }
        public IWebElement SiteTelephoneNumber
        {
            get
            {
                return _main.FindElementSafe(By.Name("Telephone"));
            }
        }
        public IWebElement SiteMobileNumber
        {
            get
            {
                return _main.FindElementSafe(By.Name("MobileNumber"));
            }
        }
        public IWebElement SiteFaxNumber
        {
            get
            {
                return _main.FindElementSafe(By.Name("FaxNumber"));
            }
        }
        public IWebElement SiteEmailAddress
        {
            get
            {
                return _main.FindElementSafe(By.Name("EmailAddress"));
            }
        }
        public IWebElement SiteWebsiteAddress
        {
            get
            {
                return _main.FindElementSafe(By.Name("WebsiteAddress"));
            }
        }

        public IWebElement AddBldnglink
        {
            get
            {
                return _main.FindElement(By.CssSelector("[data-automation-id='add_building_button']"));
            }
        }




        public void EnterSiteShortName(string siteshortname)
        {
            SiteShortName.Clear();
            SiteShortName.SendKeys(siteshortname);
        }


        public void EnterSiteLongName(string sitelongname)
        {

            SiteLongName.Clear();
            SiteLongName.SendKeys(sitelongname);
        }

        public void EnterContactName(string sitecontactname)
        {
            SiteContactName.Clear();
            SiteContactName.SendKeys(sitecontactname);
        }
        public void EnterTelephoneNumber(string telenumber)
        {
            SiteTelephoneNumber.Clear();
            SiteTelephoneNumber.SendKeys(telenumber);
        }
        public void EnterMobileNumber(string mobilenumber)
        {
            SiteMobileNumber.Clear();
            SiteMobileNumber.SendKeys(mobilenumber);
        }
        public void EnterFaxNumber(string faxNumber)
        {
            SiteFaxNumber.Clear();
            SiteFaxNumber.SendKeys(faxNumber);
        }
        public void EnterEmailAddress(string emailaddress)
        {
            SiteEmailAddress.Clear();
            SiteEmailAddress.SendKeys(emailaddress);
        }
        public void EnterWebsiteAddress(string webaddress)
        {
            SiteWebsiteAddress.Clear();
            SiteWebsiteAddress.SendKeys(webaddress);
        }

        public void ClickOkButton()
        {
            _OKButton.Click();
            Thread.Sleep(4000); // TODO Finding better way to wait the element to be appeared in the grid.
        }
        public void DeleteSiteDetails()
        {
            WaitForAndClick(BrowserDefaults.TimeOut, MySchoolDetailsElements.DeleteButton);
            WaitForAndClick(BrowserDefaults.TimeOut, MySchoolDetailsElements.YesDelete);
            WaitForAndClick(BrowserDefaults.TimeOut, MySchoolDetailsElements.ContinueWithDelete);
        }

        public IWebElement AddSchoolSitebldnglink
        {
            get
            {
                return _main.FindElement(By.CssSelector("[data-automation-id='add_building_button']"));
            }
        }


        public AddSchoolBuildingPopupPage ClickAddBuilding()
        {

            AddSchoolSitebldnglink.Click();
            WaitUntilDisplayed(MySchoolDetailsElements.AddBuildingPopupTitle);
            return new AddSchoolBuildingPopupPage();

        }

        public AddSiteAddressPopupPage ClickAddSiteAddressBtn()
        {
            _addsiteaddressbutton.Click();
            return new AddSiteAddressPopupPage();
        }

    }
}
