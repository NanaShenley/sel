using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using POM.Base;
using POM.Components.Common;
using POM.Helper;

namespace POM.Components.Conduct
{
    public class SuspensionTriplet : BaseComponent
    {
        public override By ComponentIdentifier
        {
            get { return SimsBy.Id("screen"); }
        }

        public SuspensionTriplet()
        {
            _searchCriteria = new SuspensionSearchPage(this);
        }

        public WarningDialog ExistSuspensionPage()
        {
            IWebElement _existButton = SeleniumHelper.Get(SimsBy.AutomationId("tab_Suspensions_close_button"));
            _existButton.Click();
            return new WarningDialog();
        }


        #region Search

        public class SuspensionSearchResultTile : SearchResultTileBase
        {
            [FindsBy(How = How.CssSelector, Using = "[data-automation-id='resultTile']")]
            private IWebElement _name;

            public string Name
            {
                get { return _name.Text; }
            }
        }

        private readonly SuspensionSearchPage _searchCriteria;
        public SuspensionSearchPage SearchCriteria
        {
            get { return _searchCriteria; }
        }

        #endregion


    }



}
