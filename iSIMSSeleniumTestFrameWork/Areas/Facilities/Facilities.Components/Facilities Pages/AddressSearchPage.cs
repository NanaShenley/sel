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
using Facilities.Components.Facilities_Pages;

public class AddressSearchPage : BaseSeleniumComponents
{

    [FindsBy(How = How.CssSelector, Using = "[data-automation-id='search_criteria']")]
    public IWebElement main;
    [FindsBy(How = How.CssSelector, Using = "[data-automation-id='search_criteria_submit']")]
    public IWebElement searchButton;


    public AddressSearchPage()
    {
        PageFactory.InitElements(WebContext.WebDriver, this);
    }
    private IWebElement FullPostCode
    {
        get
        {
            return main.FindElementSafe(By.Name("PostCode"));
        }
    }
    private IWebElement HouseNumber
    {
        get
        {
            return main.FindElementSafe(By.Name("PAONRange"));
        }
    }
    private IWebElement HouseName
    {
        get
        {
            return main.FindElementSafe(By.Name("PAONDescription"));
        }
    }
    public void EnterPostNumber(string postcode)
    {
        FullPostCode.Clear();
        FullPostCode.SendKeys(postcode);
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
    public AddressResultsPage ClickSearchButton()
    {
        searchButton.Click();
        return new AddressResultsPage();
    }
}