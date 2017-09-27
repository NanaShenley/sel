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
    public class EmploymentContractDestinationTriplet : BaseComponent
    {
        public override By ComponentIdentifier
        {
            get { return SimsBy.AutomationId("lookup_double"); }
        }

        public EmploymentContractDestinationTriplet()
        {
            _searchCriteria = new EmploymentContractDestinationSearch(this);
        }

        #region Search

        private readonly EmploymentContractDestinationSearch _searchCriteria;
        public EmploymentContractDestinationSearch SearchCriteria { get { return _searchCriteria; } }

        #endregion

        #region Public actions

        public static EmploymentContractDestinationDetailsPage Create()
        {
            SeleniumHelper.Get(SimsBy.AutomationId("create_service_EmploymentContractDestination")).ClickByJS();
            Wait.WaitForAjaxReady(SimsBy.CssSelector(".locking-mask"));
            return new EmploymentContractDestinationDetailsPage();
        }

        public void Delete(POM.Components.Staff.EmploymentContractDestinationDetailsPage.EmploymentContractDestinationRow record)
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

    public class EmploymentContractDestinationSearch : SearchTableCriteriaComponent
    {
        public EmploymentContractDestinationSearch(BaseComponent parent) : base(parent) { }

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
