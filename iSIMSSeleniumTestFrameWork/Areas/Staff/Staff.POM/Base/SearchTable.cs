using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using System.Threading;
using Staff.POM.Helper;
using SeSugar.Automation;


namespace Staff.POM.Base
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
            Wait.WaitUntilEnabled(By.CssSelector("[data-automation-id='search_criteria_submit']"));

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
