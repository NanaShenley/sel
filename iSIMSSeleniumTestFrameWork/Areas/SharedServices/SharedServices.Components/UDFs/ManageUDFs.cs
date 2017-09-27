using OpenQA.Selenium.Support.PageObjects;
using System;
using WebDriverRunner.webdriver;

namespace SharedServices.Components.UDFs
{
    public class ManageUDFs
    {
        private readonly SearchComponent _searchComponent = new SearchComponent();
        private readonly DetailComponent _detailComponent = new DetailComponent();

        public SearchComponent SearchComponent => _searchComponent;

        public DetailComponent DetailComponent => _detailComponent;

        public ManageUDFs()
        {
            WebContext.WebDriver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(10));
            PageFactory.InitElements(WebContext.WebDriver, _searchComponent);
            PageFactory.InitElements(WebContext.WebDriver, _detailComponent);
        }

        public bool HasSearchResultFor(string udfDefinition)
        {
            return SearchComponent.HasSearchResultFor(udfDefinition);
        }
    }
}
