using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using Staff.POM.Base;
using Staff.POM.Helper;
using SeSugar.Automation;

namespace Staff.POM.Components.Staff
{
    public class SupperannuationSchemeTripletDialog : BaseDialogComponent
    {
        public override By DialogIdentifier
        {
            get { return SimsBy.AutomationId("superannuation_scheme_palette_triplet"); }
        }

        public SupperannuationSchemeTripletDialog()
        {
            _searchCriteria = new SuperannuationSchemeSearchDialog(this);
        }

        #region Search

        public class SupperannuationSchemeSearchResultTile : SearchResultTileBase
        {
            [FindsBy(How = How.CssSelector, Using = "[title='Code']")]
            private IWebElement _code;

            public string Code
            {
                get { return _code.Text; }
            }
        }

        private readonly SuperannuationSchemeSearchDialog _searchCriteria;
        public SuperannuationSchemeSearchDialog SearchCriteria { get { return _searchCriteria; } }

        #endregion

        #region Properties

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='ok_button']")]
        private IWebElement _okButton;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='cancel_button']")]
        private IWebElement _cancelButton;

        #endregion

        #region Actions

        /// <summary>
        /// Author: Huy.Vo
        /// Description: Init page
        /// </summary>
        /// <returns></returns>

        public void SelectSearchTile(SupperannuationSchemeSearchResultTile supperannuationSchemeTile)
        {
            if (supperannuationSchemeTile != null)
            {
                supperannuationSchemeTile.Click();
            }
        }

        public void ClickOK()
        {
            _okButton.Click();
            Wait.WaitForAjaxReady(SimsBy.CssSelector(".locking-mask"));
        }

        #endregion
    }

    public class SuperannuationSchemeSearchDialog : SearchCriteriaComponent<SupperannuationSchemeTripletDialog.SupperannuationSchemeSearchResultTile>
    {
        public SuperannuationSchemeSearchDialog(BaseComponent parent) : base(parent) { }

        #region Page properties

        [FindsBy(How = How.Name, Using = "Description")]
        private IWebElement _code;

        public string SearchByCodeOrDescriptions
        {
            set { _code.SetText(value); }
            get { return _code.GetValue(); }
        }

        #endregion
    }

    public class SuperannuationSchemesDetailsDialog : BaseDialogComponent
    {
        public override By DialogIdentifier
        {
            get { return SimsBy.AutomationId("superannuation_scheme_palette_detail"); }
        }

        #region Page properties

        [FindsBy(How = How.Name, Using = "Code")] 
        private IWebElement _codeTextBox;

        [FindsBy(How = How.Name, Using = "Description")] 
        private IWebElement _descriptionTextBox;

        public string Code
        {
            set { _codeTextBox.SetText(value); }
            get { return _codeTextBox.GetValue(); }
        }

        public string Description
        {
            set { _descriptionTextBox.SetText(value); }
            get { return _descriptionTextBox.GetValue(); }
        }

        #endregion

        #region SchemeValue Grid

        public GridComponent<SchemeValueGridRow> SchemeValues
        {
            get
            {
                return
                    new GridComponent<SchemeValueGridRow>(
                        By.CssSelector("[data-maintenance-container='SuperannuationSchemeDetails']"),
                        ComponentIdentifier);
            }
        }

        public class SchemeValueGridRow : GridRow
        {
            [FindsBy(How = How.CssSelector, Using = "[name$='ApplicationDate']")] private IWebElement _applicationDate;

            [FindsBy(How = How.CssSelector, Using = "[name$='Value']")] private IWebElement _value;

            public string ApplicationDate
            {
                set { _applicationDate.SetText(value); }
                get { return _applicationDate.GetValue(); }
            }

            public string Value
            {
                set { _value.SetText(value); }
                get { return _value.GetValue(); }
            }
        }

        #endregion

        #region Public methods

        public void ClickAddSchemeValues()
        {
            AutomationSugar.WaitFor("add_scheme_values_button");
            AutomationSugar.ClickOn("add_scheme_values_button");
            AutomationSugar.WaitForAjaxCompletion();
        }

        #endregion
    }
}
