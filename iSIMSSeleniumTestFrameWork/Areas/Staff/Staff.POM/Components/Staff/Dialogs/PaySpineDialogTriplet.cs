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
using OpenQA.Selenium.Support.UI;

namespace Staff.POM.Components.Staff
{
    public class PaySpineDialogTriplet : BaseDialogComponent
    {
        public override By DialogIdentifier
        {
            get { return SimsBy.AutomationId("service_terms_pay_scales_pay_spine_dialog"); }
        }

        public PaySpineDialogTriplet()
        {
            _searchCriteria = new PaySpineSearchDialog(this);
        }

        #region Search

        private readonly PaySpineSearchDialog _searchCriteria;
        public PaySpineSearchDialog SearchCriteria { get { return _searchCriteria; } }

        public class PaySpineDialogSearchResultTile : SearchResultTileBase
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

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='save_button']")]
        private IWebElement _saveButton;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='pay_spines_create_button']")]
        private IWebElement _createButton;

        #endregion

        #region Public methods

        public void SelectSearchTile(PaySpineDialogSearchResultTile paySpineTile)
        {
            if (paySpineTile != null)
            {
                paySpineTile.Click();
            }
        }

        public void ClickSavePaySpine()
        {
            _saveButton.ClickByJS();
            AutomationSugar.WaitForAjaxCompletion();
        }

        public void ClickSavePaySpineStale()
        {
            _saveButton.ClickByJS();
            AutomationSugar.WaitUntilStale("pay_spines_detail");
            AutomationSugar.WaitForAjaxCompletion();
        }

        public PaySpineDetail ClickCreatePaySpine()
        {
            _createButton.ClickByJS();
            AutomationSugar.WaitForAjaxCompletion();
            return new PaySpineDetail();
        }

        #endregion

        public class PaySpineDetail : BaseComponent
        {
            public override By ComponentIdentifier
            {
                get { return SimsBy.AutomationId("pay_spines_detail"); }
            }

            [FindsBy(How = How.CssSelector, Using = "[data-automation-id='well_know_action_save']")]
            private IWebElement _saveButton;

            [FindsBy(How = How.CssSelector, Using = "[data-automation-id='delete_button']")]
            private IWebElement _deleteButton;

            [FindsBy(How = How.CssSelector, Using = "[data-automation-id='status_success']")]
            private IWebElement _statusSuccess;

            [FindsBy(How = How.Name, Using = "Code")]
            private IWebElement _codeTextBox;

            [FindsBy(How = How.Name, Using = "Interval")]
            private IWebElement _intervalTextBox;

            [FindsBy(How = How.Name, Using = "MaximumPoint")]
            private IWebElement _maximumPointTextBox;

            [FindsBy(How = How.Name, Using = "MinimumPoint")]
            private IWebElement _minimumPointTextBox;

            [FindsBy(How = How.Name, Using = "AwardDate")]
            private IWebElement _awardDateTextBox;

            [FindsBy(How = How.Name, Using = "GeneratePayAwards")]
            private IWebElement _addPayAdwardsButton;

            [FindsBy(How = How.CssSelector, Using = "[data-automation-id='generate_scale_awards_jobstep_button']")]
            private IWebElement _addButton;

            [FindsBy(How = How.CssSelector, Using = "[data-maintenance-container='PayAwards']")]
            private IWebElement _scaleAwardTable;

            public string Code
            {
                set { _codeTextBox.SetText(value); }
                get { return _codeTextBox.GetValue(); }
            }

            public string MinimumPoint
            {
                set { _minimumPointTextBox.SetText(value); }
                get { return _minimumPointTextBox.GetValue(); }
            }

            public string MaximumPoint
            {
                set { _maximumPointTextBox.SetText(value); }
                get { return _maximumPointTextBox.GetValue(); }
            }

            public string InterVal
            {
                set { _intervalTextBox.SetText(value); }
                get { return _intervalTextBox.GetValue(); }
            }

            public string AwardDate
            {
                set { _awardDateTextBox.SetText(value); }
                get { return _awardDateTextBox.GetValue(); }
            }

            public void ClickAddScaleAwards(int expectedRows = -1)
            {
                AutomationSugar.WaitFor(new ByChained(this.ComponentIdentifier, SimsBy.AutomationId("generate_scale_awards_jobstep_button")));
                AutomationSugar.ClickOn(new ByChained(this.ComponentIdentifier, SimsBy.AutomationId("generate_scale_awards_jobstep_button")));
                AutomationSugar.WaitForAjaxCompletion();

                if (expectedRows > -1)
                {
                    WebDriverWait wait = new WebDriverWait(SeSugar.Environment.WebContext.WebDriver, SeSugar.Environment.Settings.ElementRetrievalTimeout);
                    wait.Until(x => x.FindElements(By.CssSelector("[data-maintenance-container='PayAwards'] tbody tr")).Count == expectedRows);
                }
            }

            public GridComponent<ScaleAward> ScaleAwards
            {
                get
                {
                    return new GridComponent<ScaleAward>(By.CssSelector("[data-maintenance-container='PayAwards']"), ComponentIdentifier);
                }
            }

            public class ScaleAward : GridRow
            {
                [FindsBy(How = How.CssSelector, Using = "[name $='ScaleAmount']")]
                private IWebElement _scaleAmount;

                [FindsBy(How = How.CssSelector, Using = "[name$='ScalePoint']")]
                private IWebElement _scalePoint;

                [FindsBy(How = How.CssSelector, Using = "[name$='Date']")]
                private IWebElement _awardDate;

                public string ScaleAmount
                {
                    set { _scaleAmount.SetText(value); }
                    get { return _scaleAmount.GetValue(); }
                }
                public string ScalePoint
                {
                    set { _scalePoint.SetText(value); }
                    get { return _scalePoint.GetValue(); }
                }
                public string AwardDate
                {
                    set { _awardDate.SetText(value); }
                    get { return _awardDate.GetValue(); }
                }
            }
        }
    }

    public class PaySpineSearchDialog : SearchCriteriaComponent<PaySpineDialogTriplet.PaySpineDialogSearchResultTile>
    {
        public PaySpineSearchDialog(BaseComponent parent) : base(parent) { }

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