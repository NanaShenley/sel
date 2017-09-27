using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebDriverRunner.webdriver;

namespace Facilities.Components
{
    public class SchoolDetailsPage
    {
#pragma warning disable 0649
        [FindsBy(How = How.LinkText, Using = "School Sites and Buildings")]
        private IWebElement siteAndBuildingLink;
      
        
        
        public SchoolDetailsPage()
        {       
            // wait for the page to be fully loaded
            WebDriverWait waiter = new WebDriverWait(WebContext.WebDriver, TimeSpan.FromSeconds(30));
            waiter.Until<Boolean>(d =>
            {
               IWebElement el = WebContext.WebDriver.FindElement(By.Id("html"));
               string classes = el.GetAttribute("class");
                if (classes.Contains("nprogress-busy")){
                    return false;
                }
                return true;
            });
            PageFactory.InitElements(WebContext.WebDriver, this);
        }


        private IWebElement GetSiteAndBuilding()
        {
            return WebContext.WebDriver.FindElement(By.CssSelector("button[title='Add Site and Buildings']"));
        }

        public void OpenSitesAndBuildings()
        {
            siteAndBuildingLink.Click();
            WebDriverWait waiter = new WebDriverWait(WebContext.WebDriver, TimeSpan.FromSeconds(30));
            IWebElement element = waiter.Until<IWebElement>(d =>
            {
                IWebElement link = GetSiteAndBuilding();
                if (link.Displayed)
                {
                    return link;
                }
                return null;
            });

        }

        public void AddSiteAndBuilding()
        {
            IWebElement link = GetSiteAndBuilding();
            link.Click();
        }
  

        public void AddSiteAndBuilding_Save()
        {
            //CLICK ON SAVE BUTTON.
            var clickSavebutton = WebContext.WebDriver.FindElement(By.CssSelector("a[title='Save Record']"));
            clickSavebutton.Click();
            WebContext.Screenshot();
        }
    }
}

