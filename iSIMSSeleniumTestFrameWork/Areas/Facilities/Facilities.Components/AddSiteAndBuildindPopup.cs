using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebDriverRunner.webdriver;

namespace Facilities.Tests
{
    public class AddSiteAndBuildindPopup
    {
#pragma warning disable 0649

        [FindsBy(How = How.CssSelector, Using = "input[name='ShortName']")]
        private IWebElement shortname;

          [FindsBy(How = How.CssSelector, Using = "input[name='Name']")]
          private IWebElement fullName;


          public AddSiteAndBuildindPopup()
        {       
            // wait for the page to be fully loaded
            WebContext.Screenshot();
            WebDriverWait waiter = new WebDriverWait(WebContext.WebDriver, TimeSpan.FromSeconds(30));
            IWebElement popup =waiter.Until<IWebElement>(d =>
            {
                IWebElement el =  WebContext.WebDriver.FindElement(By.Id("dialog-editableData"));
                if (el.Displayed)
                {
                    return el;
                }
                return null;
              
            });
            WebContext.Screenshot();
            PageFactory.InitElements(popup, this);
        }


        public void EnterShortName(string name){
            shortname.SendKeys(name);
        }

        public void EnterName(string name)
        {
            fullName.SendKeys(name);
        }

        public void clicOKBtn()
        {
            var clkOKButton = WebContext.WebDriver.FindElement(By.XPath("//div[3]/button"));
              clkOKButton.Click();
              WebContext.Screenshot();

        }
    }
}
