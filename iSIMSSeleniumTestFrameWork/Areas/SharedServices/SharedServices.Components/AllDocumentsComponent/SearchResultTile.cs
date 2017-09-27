using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using WebDriverRunner.webdriver;

namespace SharedServices.Components.AllDocumentsComponent
{
    public class SearchResultTile
    {       
        public IWebElement PupilTile()
        {
            return WebContext.WebDriver.FindElement(By.CssSelector("[data-section-id='searchResults'] > div > div:nth-child(2) > div:nth-child(1) > div:nth-child(1) > a[title='Name']"));
        }

        public void SelectFirstPupil()
        {
            PupilTile().Click();
        }
    }
}
