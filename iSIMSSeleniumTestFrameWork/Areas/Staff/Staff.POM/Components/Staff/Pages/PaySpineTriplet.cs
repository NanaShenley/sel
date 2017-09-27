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
using SeSugar.Automation;

namespace Staff.POM.Components.Staff
{
    public class PaySpineTriplet : BaseComponent
    {
        public override By ComponentIdentifier
        {
            get { return SimsBy.Id("screen"); }
        }

        public PaySpineTriplet()
        {
            _searchCriteria = new PaySpineSearchPage(this);
        }

        #region Search

        private readonly PaySpineSearchPage _searchCriteria;
        public PaySpineSearchPage SearchCriteria { get { return _searchCriteria; } }

        public class PaySpineSearchResultTile : SearchResultTileBase
        {
            [FindsBy(How = How.CssSelector, Using = "[data-automation-id='resultTile']")]
            private IWebElement _code;

            public string Code
            {
                get { return _code.Text; }
            }
        }

        #endregion

        #region Public methods

        public void SelectSearchTile(PaySpineSearchResultTile paySpineTile)
        {
            if (paySpineTile != null)
            {
                paySpineTile.Click();
            }
        }

       #endregion

        #region LOGIGEAR - NOT USED

        #region Properties

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='delete_button']")]
        private IWebElement _deleteButton;

        #endregion

        /// <summary>
        /// Author: Huy.Vo
        /// Description: Init page
        /// </summary>
        /// <returns></returns>
        public PaySpinePage Create()
        {
            SeleniumHelper.Get(SimsBy.AutomationId("create_button")).ClickByJS();
            Wait.WaitForAjaxReady(SimsBy.CssSelector(".locking-mask"));
            return new PaySpinePage();
        }

        /// <summary>
        /// Author: Huy.Vo
        /// Description: click Delete button to delete an existing scheme
        /// </summary>
        public void Delete()
        {
            if (_deleteButton.IsExist())
            {
                _deleteButton.ClickByJS();
                Wait.WaitForAjaxReady(SimsBy.CssSelector(".locking-mask"));
                var confirmDialog = new DeleteConfirmationPage();
                confirmDialog.ConfirmDelete();
            }
        }

        #endregion

    }

    public class PaySpineSearchPage : SearchCriteriaComponent<PaySpineTriplet.PaySpineSearchResultTile>
    {
        public PaySpineSearchPage(BaseComponent parent) : base(parent) { }

        #region Search properties

        [FindsBy(How = How.Name, Using = "Code")]
        private IWebElement _searchTextBox;

        public string SearchByCode
        {
            get { return _searchTextBox.GetValue(); }
            set { _searchTextBox.SetText(value); }
        }

        #endregion
    }
}
