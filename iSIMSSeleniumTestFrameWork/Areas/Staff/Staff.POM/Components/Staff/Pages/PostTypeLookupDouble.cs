using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using Staff.POM.Base;
using Staff.POM.Components.Staff;
using SeSugar.Automation;
using Staff.POM.Helper;

namespace Staff.Tests.Lookups
{
    public class PostTypeLookupDouble : BaseComponent
    {
        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='add_post_type_button']")]
        private IWebElement _addButton;
        private PostTypeLookupSearch _searchCriteria;

        public PostTypeLookupDouble()
        {
            _searchCriteria = new PostTypeLookupSearch(this);
        }

        public PostTypeLookupSearch SearchCriteria
        {
            get
            {
                return _searchCriteria;
            }
        }

        public override By ComponentIdentifier
        {
            get
            {
                return SimsBy.AutomationId("lookup_double");
            }
        }

        public PostTypeLookupDialog ClickAddButton()
        {
            _addButton.Click();
            return new PostTypeLookupDialog();
        }
    }

    public class PostTypeLookupSearch : SearchTableCriteriaComponent
    {
        public PostTypeLookupSearch(BaseComponent parent) : base(parent) { }

        #region Properties

        [FindsBy(How = How.Name, Using = "CodeOrDescription")]
        private IWebElement _searchTextBox;

        public string CodeOrDescription
        {
            get { return _searchTextBox.GetValue(); }
            set { _searchTextBox.SetText(value); }
        }

        #endregion
    }
}
