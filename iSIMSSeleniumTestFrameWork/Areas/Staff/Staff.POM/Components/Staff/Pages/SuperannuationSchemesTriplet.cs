using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium.Support.PageObjects;
using OpenQA.Selenium;
using NUnit.Framework;
using Staff.POM.Helper;
using Staff.POM.Base;
using Staff.POM.Components.Staff;
using SeSugar.Automation;

namespace Staff.POM.Components.Staff
{
    public class SuperannuationSchemesTriplet : BaseComponent
    {
        public override By ComponentIdentifier
        {
            get { return SimsBy.AutomationId("superannuation_schemes_triplet"); }
        }

        public SuperannuationSchemesTriplet()
        {
            _searchCriteria = new SchemesSearch(this);
        }

        #region Properties

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='Yes_button']")]
        private IWebElement _confirmDelete;

        #endregion

        #region Search

        private readonly SchemesSearch _searchCriteria;
        public SchemesSearch SearchCriteria { get { return _searchCriteria; } }

        public class SchemeSearchResultTile : SearchResultTileBase
        {
            [FindsBy(How = How.CssSelector, Using = "[data-automation-id='resultTile']")]
            private IWebElement _code;

            public string Code
            {
                get { return _code.Text; }
            }
        }

        #endregion

        #region Properties

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='delete_button']")]
        private IWebElement _deleteButton;

        #endregion

        #region Public methods

        /// <summary>
        /// Author: Luong>Mai
        /// Description: Init page
        /// </summary>
        /// <returns></returns>
        public static SuperannuationSchemesPage Create()
        {
            SeleniumHelper.Get(SimsBy.AutomationId("superannuation_schemes_create_button")).ClickByJS();
            Wait.WaitForAjaxReady(SimsBy.CssSelector(".locking-mask"));
            return new SuperannuationSchemesPage();
        }

        public new void ClickCreate()
        {
            AutomationSugar.WaitFor(new ByChained(this.ComponentIdentifier, SimsBy.AutomationId("superannuation_schemes_create_button")));
            AutomationSugar.ClickOn(new ByChained(this.ComponentIdentifier, SimsBy.AutomationId("superannuation_schemes_create_button")));
            AutomationSugar.WaitForAjaxCompletion();
        }

        /// <summary>
        /// Author: Ba.Truong
        /// Description: click Delete button to delete an existing scheme
        /// </summary>
        public void Delete()
        {
            if(_deleteButton.IsExist())
            {
                _deleteButton.ClickByJS();
                Wait.WaitForAjaxReady(SimsBy.CssSelector(".locking-mask"));
                var confirmDialog = new DeleteConfirmationPage();
                confirmDialog.ConfirmDelete();
            }
        }

        public void SelectSearchTile(SchemeSearchResultTile schemeTile)
        {
            if(schemeTile != null)
            {
                schemeTile.Click();
            }
        }

        #endregion
    }

    public class SchemesSearch : SearchCriteriaComponent<SuperannuationSchemesTriplet.SchemeSearchResultTile>
    {
        public SchemesSearch(BaseComponent parent) : base(parent) { }

        #region Search properties

        [FindsBy(How = How.Name, Using = "Description")]
        private IWebElement _searchTextBox;

        public string CodeOrDecription
        {
            get { return _searchTextBox.GetValue(); }
            set { _searchTextBox.SetText(value); }
        }

        #endregion
    }
}
