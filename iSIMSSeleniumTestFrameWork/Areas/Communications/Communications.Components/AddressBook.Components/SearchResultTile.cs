using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using SharedComponents.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebDriverRunner.webdriver;

namespace AddressBook.Components
{
    public class SearchResultTile
    {
        public SearchResultTile()
        {
            //TODO put Wait
            PageFactory.InitElements(WebContext.WebDriver, this);
        }

        private System.Collections.ObjectModel.ReadOnlyCollection<IWebElement> Tiles
        {
            get
            {
                return WebContext.WebDriver.FindElements(By.CssSelector(SeleniumHelper.AutomationId("search_result_tile_Learner")));
            }
        }

        public int tileCount()
        {
            return Tiles.Count;
        }

        public string getClassForStrongname(int tilenum){
            IWebElement tile = FindTile(tilenum);
            return tile.GetAttribute("class");

        }

        public IWebElement FindTile(int tilenum)
        {          
            return Tiles[tilenum];
        }


    }
}
