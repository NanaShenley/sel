using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using Staff.POM.Helper;
using Staff.POM.Base;
using SeSugar.Automation;

namespace Staff.POM.Components.Staff
{
    public class QTSRouteDetailsPage : BaseComponent
    {
        public override By ComponentIdentifier
        {
            get { return SimsBy.AutomationId("lookup_provider_entity_detail"); }
        }

        #region Page properties

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='well_know_action_save']")]
        private IWebElement _saveButton;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='status_success']")]
        private IWebElement _statusSuccess;

        public GridComponent<QTSRouteRow> QTSRoutes
        {
            get
            {
                return new GridComponent<QTSRouteRow>(By.CssSelector("[data-maintenance-grid-id='LookupsWithProviderGrid1']"), ComponentIdentifier);
            }
        }

        public class QTSRouteRow : GridRow
        {
            [FindsBy(How = How.CssSelector, Using = "[name$='Code']")]
            private IWebElement _code;

            [FindsBy(How = How.CssSelector, Using = "[name$='Description']")]
            private IWebElement _Description;

            [FindsBy(How = How.CssSelector, Using = "[name$='DisplayOrder']")]
            private IWebElement _DisplayOrder;

            [FindsBy(How = How.CssSelector, Using = "[name$='IsVisible']")]
            private IWebElement _IsVisible;

            [FindsBy(How = How.CssSelector, Using = "[name$='ResourceProviderName']")]
            private IWebElement _ResourceProvider;

            public string Code
            {
                set { _code.SetText(value); }
                get { return _code.GetValue(); }
            }

            public string Description
            {
                set { _Description.SetText(value); }
                get { return _Description.GetValue(); }
            }

            public string DisplayOrder
            {
                set { _DisplayOrder.SetText(value); }
                get { return _DisplayOrder.GetValue(); }
            }

            public bool IsVisible
            {
                set { _IsVisible.Set(value); }
                get { return _IsVisible.IsChecked(); }
            }

            public string ResourceProvider
            {
                get { return _ResourceProvider.GetValue(); }
            }
        }

        #endregion
    }
}
