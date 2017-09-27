using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using POM.Helper;

namespace POM.Base
{
    public class SearchTableCriteriaComponent : BaseComponent
    {
        private readonly BaseComponent _parent;
        public override By ComponentIdentifier
        {
            get { return SimsBy.AutomationId("search_criteria"); }
        }

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='search_criteria_submit']")]
        private IWebElement _searchButton;

        public TDetail Search<TDetail>() where TDetail : BaseComponent, new()
        {
            Retry.Do(_searchButton.Click);

            Wait.WaitUntilEnabled(By.CssSelector("[data-automation-id='search_criteria_submit']"));

            var detail = default(TDetail);

            Retry.Do(() =>
            {
                detail = new TDetail();
            });

            Wait.WaitForAjaxReady(By.CssSelector(".locking-mask"));
            Wait.WaitForAjaxReady(By.CssSelector("[data-automation-id='search_criteria_submit'][disabled='disabled']"));

            detail.Refresh();
            return detail;
        }

        public SearchTableCriteriaComponent(BaseComponent parent)
            : base(parent)
        {
            _parent = parent;
        }
    }
}
