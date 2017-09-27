using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using POM.Base;
using POM.Helper;

namespace POM.Components.Staff
{
    public class AdditionalPaymentCategoryTriplet : BaseComponent
    {
        public override By ComponentIdentifier
        {
            get { return SimsBy.AutomationId("lookup_double"); }
        }

        public AdditionalPaymentCategoryTriplet()
        {
            _searchCriteria = new AdditionalPaymentCategorySearch(this);
        }

        #region Search

        private readonly AdditionalPaymentCategorySearch _searchCriteria;
        public AdditionalPaymentCategorySearch SearchCriteria { get { return _searchCriteria; } }

        #endregion

        #region Public actions

        /// <summary>
        /// Author: Ba.Truong
        /// Description: Init page Additional Payment Category Search
        /// </summary>
        /// <returns></returns>
        public static AdditionalPaymentCategoryDetailsPage Create()
        {
            SeleniumHelper.Get(SimsBy.AutomationId("create_service_AdditionalPaymentCategory")).ClickByJS();
            Wait.WaitForAjaxReady(SimsBy.CssSelector(".locking-mask"));
            return new AdditionalPaymentCategoryDetailsPage();
        }

        /// <summary>
        /// Author: Ba.Truong
        /// Description: Click "Delete" button on the record of category need to be deleted
        /// </summary>
        /// <param name="record">The row in 'additional payment category' table need to be deleted</param>
        public void Delete(POM.Components.Staff.AdditionalPaymentCategoryDetailsPage.AdditionalPaymentCategoriesRow record)
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

    public class AdditionalPaymentCategorySearch : SearchTableCriteriaComponent
    {
        public AdditionalPaymentCategorySearch(BaseComponent parent) : base(parent) { }

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
