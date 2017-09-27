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
   public class AddSiteAddressPopupPage : BaseSeleniumComponents
   {
    #pragma warning disable 0649
       [FindsBy(How = How.Id, Using = "dialog-dialog-editableData")]
       private readonly IWebElement _main;
       [FindsBy(How = How.CssSelector, Using = "[data-automation-id='ok_button']")]
       public IWebElement okButton;

       public AddSiteAddressPopupPage()
        {
            WaitUntilDisplayed(By.Name("PAONRange"));
            PageFactory.InitElements(WebContext.WebDriver, this);
        }
      
       private IWebElement HouseNumber
       {
           get
           {
               return _main.FindElementSafe(By.Name("PAONRange"));
           }
       }
       private IWebElement HouseName
       {
           get
           {
               return _main.FindElementSafe(By.Name("PAONDescription"));
           }
       }
       private IWebElement HouseType
       {
           get
           {
               return _main.FindElementSafe(By.Name("SAON"));
           }
       }
       private IWebElement StreetAddress
       {
           get
           {
               return _main.FindElementSafe(By.Name("Street"));
           }
       }
       private IWebElement Locality
       {
           get
           {
               return _main.FindElementSafe(By.Name("Locality"));
           }
       }
       private IWebElement TownName
       {
           get
           {
               return _main.FindElementSafe(By.Name("Town"));
           }
       }
       private IWebElement CountryName
       {
           get
           {
               return _main.FindElementSafe(By.Name("AdministrativeArea"));
           }
       }
       private IWebElement Postcode
       {
           get
           {
               return _main.FindElementSafe(By.Name("PostCode"));
           }
       }

       public void EnterHouseNumber(string housenumber)
       {
           HouseNumber.Clear();
           HouseNumber.SendKeys(housenumber);
       }

       public void EnterHouseName(string housename)
       {
           HouseName.Clear();
           HouseName.SendKeys(housename);
       }

       public void EnterHousetype(string flat)
       {
           HouseType.Clear();
           HouseType.SendKeys(flat);
       }

       public void EnterStreetAddress(string street)
       {
           StreetAddress.Clear();
           StreetAddress.SendKeys(street);
       }


       public void EnterLocalitySite(string district)
       {
           Locality.Clear();
           Locality.SendKeys(district);
       }


       public void EntertownName(string town)
       {
           TownName.Clear();
           TownName.SendKeys(town);
       }

       public void EnterCountryName(string country)
       {
           CountryName.Clear();
           CountryName.SendKeys(country);
       }


       public void EnterPostCodeSite(string postcode)
       {
           Postcode.Clear();
           Postcode.SendKeys(postcode);
       }

       public AddSchoolSitepopupPage OkButtonAddress()
       {
           okButton.Click();
           return new AddSchoolSitepopupPage();
       
       }
    }
}
