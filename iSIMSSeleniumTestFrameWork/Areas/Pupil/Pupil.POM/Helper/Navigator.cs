﻿using OpenQA.Selenium;
using WebDriverRunner.webdriver;

namespace POM.Helper
{
    public class Navigator
    {
        public static void Open(string url)
        {
            WebContext.WebDriver.Url = url;
            IeCertificateCheck();
        }


        public static void CloseBrowser()
        {
            IWebDriver _webDriver = WebContext.WebDriver;
            _webDriver.Quit();
        }

        public static void IeCertificateCheck()
        {
            if ("internet explorer".Equals(WebContext.Browser))
            {
                if (WebContext.WebDriver.Title.Equals("Certificate Error: Navigation Blocked"))
                {
                    WebContext.WebDriver.Url = "javascript:document.getElementById('overridelink').click();";
                }
            }
        }

	    public static void CloseSideMenu()
	    {
			var menuCloseButton =
						SharedComponents.Helpers.SeleniumHelper.GetVisible(By.CssSelector("button.btn-close-taskmenu"));
			if (menuCloseButton != null)
			{
				SharedComponents.Helpers.SeleniumHelper.FindAndClick(By.CssSelector("button.btn-close-taskmenu"));
			}
		}
    }
}
