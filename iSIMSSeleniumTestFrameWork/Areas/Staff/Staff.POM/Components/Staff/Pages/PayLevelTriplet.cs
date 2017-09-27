using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Staff.POM.Helper;
using Staff.POM.Base;
using SeSugar.Automation;

namespace Staff.POM.Components.Staff
{
    public class PayLevelTriplet : BaseComponent
    {
        public override By ComponentIdentifier
        {
            get { return SimsBy.AutomationId("lookup_double"); }
        }

        public PayLevelTriplet()
        {
            _searchCriteria = new PayLevelSearch(this);
        }

        #region Search

        private readonly PayLevelSearch _searchCriteria;
        public PayLevelSearch SearchCriteria { get { return _searchCriteria; } }

        #endregion

        #region Public actions

        public static PayLevelDetailsPage Create()
        {
            SeleniumHelper.Get(SimsBy.AutomationId("create_service_PayLevel")).ClickByJS();
            Wait.WaitForAjaxReady(SimsBy.CssSelector(".locking-mask"));
            return new PayLevelDetailsPage();
        }

        public void Delete(POM.Components.Staff.PayLevelDetailsPage.PayLevelRow record)
        {
            if (record != null)
            {
                record.ClickDelete();
                SeleniumHelper.Get(SimsBy.AutomationId("Yes_button")).ClickByJS();
                SeleniumHelper.Get(SimsBy.AutomationId("well_know_action_save")).ClickByJS();
                Wait.WaitForAjaxReady(SimsBy.CssSelector(".locking-mask"));
            }
        }

        #endregion
    }

    public class PayLevelSearch : SearchTableCriteriaComponent
    {
        public PayLevelSearch(BaseComponent parent) : base(parent) { }

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
