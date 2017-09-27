using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using Staff.POM.Base;
using Staff.POM.Helper;
using System.Threading;

namespace Staff.POM.Components.Staff
{
    public class ServiceTermPage : BaseComponent
    {
        public override By ComponentIdentifier
        {
            get { return SimsBy.AutomationId("service_terms_detail"); }
        }

        #region Page properties

        [FindsBy(How = How.Name, Using = "Code")]
        private IWebElement _codeTextBox;

        [FindsBy(How = How.Name, Using = "Description")]
        private IWebElement _descriptionTextBox;

        [FindsBy(How = How.Name, Using = "IncrementMonthSelector.dropdownImitator")]
        private IWebElement _incrementMonthDropDownList;

        [FindsBy(How = How.Name, Using = "HoursWorkedPerWeek")]
        private IWebElement _hourWorkPerWeekTextBox;

        [FindsBy(How = How.Name, Using = "WeeksWorkedPerYear")]
        private IWebElement _weekWordPerYearTextBox;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='add_pay_scale_button']")]
        private IWebElement _addPayScaleLink;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='add_allowances_button']")]
        private IWebElement _addAllowanceLink;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='add_post_types_button']")]
        private IWebElement _addPostTypeLink;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='add_superannuation_schemes_button']")]
        private IWebElement _addSuperannuationSchemeLink;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='add__button']")]
        private IWebElement _addFinalcialButton;

        [FindsBy(How = How.CssSelector, Using = "[data-maintenance-container='PayScales']")]
        private IWebElement _payScalesTable;

        [FindsBy(How = How.CssSelector, Using = "[data-maintenance-container='ServiceTermPostTypes']")]
        private IWebElement _postTypesTable;


        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='section_menu_Pay']")]
        private IWebElement _payAdwardsTab;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='section_menu_Allowances']")]
        private IWebElement _allowanceTab;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='section_menu_Post']")]
        private IWebElement _postTypeTab;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='section_menu_Superannuation']")]
        private IWebElement _superannuationSchemesTab;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='section_menu_Financial']")]
        private IWebElement _financialSubGroupsTab;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='well_know_action_save']")]
        private IWebElement _saveButton;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='status_success']")]
        private IWebElement _statusSuccess;

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

        public string IncrementMonth
        {
            set { _incrementMonthDropDownList.EnterForDropDown(value); }
            get { return _incrementMonthDropDownList.GetValue(); }
        }

        public string HourWorkPerWeek
        {
            set { _hourWorkPerWeekTextBox.SetText(value); }
            get { return _hourWorkPerWeekTextBox.GetValue(); }
        }

        public string WeekWordPerYear
        {
            set { _weekWordPerYearTextBox.SetText(value); }
            get { return _weekWordPerYearTextBox.GetValue(); }
        }

        #endregion

        #region PayScale Grid

        public GridComponent<PayScale> PayScaleTable
        {
            get
            {
                GridComponent<PayScale> returnValue = null;
                Retry.Do(() =>
                {
                    returnValue = new GridComponent<PayScale>(By.CssSelector("[data-maintenance-container='PayScales']"), ComponentIdentifier);
                });
                return returnValue;
            }
        }

        public class PayScale
        {
            [FindsBy(How = How.CssSelector, Using = "[name$='Code']")]
            private IWebElement _code;

            [FindsBy(How = How.CssSelector, Using = "[name$='Description']")]
            private IWebElement _description;

            [FindsBy(How = How.CssSelector, Using = "[name$='StatutoryPayScaleDescription']")]
            private IWebElement _statutoryPayScaleDescription;

            [FindsBy(How = How.CssSelector, Using = "[name$='MinimumPoint']")]
            private IWebElement _minimumPoint;

            [FindsBy(How = How.CssSelector, Using = "[name$='MaximumPoint']")]
            private IWebElement _maximumPoint;

            [FindsBy(How = How.CssSelector, Using = "[name$='PaySpineCode']")]
            private IWebElement _paySpineCode;

            [FindsBy(How = How.CssSelector, Using = "[data-automation-id='edit..._button']")]
            private IWebElement _editButton;

            public string Code
            {
                set { _code.SetText(value); }
                get { return _code.GetValue(); }
            }

            public string Description
            {
                set { _description.SetText(value); }
                get { return _description.GetValue(); }
            }
            public string StatutoryPayScale
            {
                set { _statutoryPayScaleDescription.SetText(value); }
                get { return _statutoryPayScaleDescription.GetValue(); }
            }

            public string MinimumPoint
            {
                set { _minimumPoint.SetText(value); }
                get { return _minimumPoint.GetValue(); }
            }
            public string MaximumPoint
            {
                set { _maximumPoint.SetText(value); }
                get { return _maximumPoint.GetValue(); }
            }
            public string PaySpine
            {
                set { _paySpineCode.SetText(value); }
                get { return _paySpineCode.GetValue(); }
            }

            public PayScaleDialog Edit()
            {
                _editButton.ClickByJS();
                return new PayScaleDialog();
            }
        }

        #endregion

        #region PostType Grid

        public GridComponent<PostType> PostTypeTable
        {
            get
            {
                GridComponent<PostType> returnValue = null;
                Retry.Do(() =>
                {
                    returnValue = new GridComponent<PostType>(By.CssSelector("[data-maintenance-container='ServiceTermPostTypes']"), ComponentIdentifier);
                });
                return returnValue;
            }
        }

        public class PostType
        {
            [FindsBy(How = How.CssSelector, Using = "[name$=PostTypeCode]")]
            private IWebElement _code;

            public string Code
            {
                set { _code.SetText(value); }
                get { return _code.GetValue(); }
            }
        }
        #endregion

        #region Tables

        public GridComponent<AllowanceTableRow> AllowanceTable
        {
            get
            {
                GridComponent<AllowanceTableRow> returnValue = null;
                Retry.Do(() =>
                {
                    returnValue = new GridComponent<AllowanceTableRow>(By.CssSelector("[data-maintenance-container='ServiceTermAllowances']"), ComponentIdentifier);
                });
                return returnValue;
            }
        }

        public class AllowanceTableRow : GridRow
        {
            [FindsBy(How = How.CssSelector, Using = "[name$='Code']")]
            private IWebElement _code;

            [FindsBy(How = How.CssSelector, Using = "[name$='Description']")]
            private IWebElement _Description;

            [FindsBy(How = How.CssSelector, Using = "[name$='DisplayOrder']")]
            private IWebElement _DisplayOrder;

            [FindsBy(How = How.CssSelector, Using = "[name$='IsVisible']")]
            private IWebElement _IsVisible;

            [FindsBy(How = How.CssSelector, Using = "[name$='AdditionalPaymentCategory.dropdownImitator']")]
            private IWebElement _Category;

            public string Code
            {
                set { _code.SetText(value); }
                get { return _code.GetAttribute("Value"); }
            }

            public string Description
            {
                set { _Description.SetText(value); }
                get { return _Description.GetAttribute("Value"); }
            }

            public string DisplayOrder
            {
                set { _DisplayOrder.SetText(value); }
                get { return _DisplayOrder.GetAttribute("Value"); }
            }

            public bool IsVisible
            {
                set { _IsVisible.Set(value); }
                get { return _IsVisible.IsChecked(); }
            }

            public string Category
            {
                set { _Category.EnterForDropDown(value); }
                get { return _Category.GetAttribute("Value"); }
            }
        }

        public GridComponent<SuperannuationScheme> SuperannuationSchemesTable
        {
            get
            {
                GridComponent<SuperannuationScheme> returnValue = null;
                Retry.Do(() =>
                {
                    returnValue = new GridComponent<SuperannuationScheme>(By.CssSelector("[data-maintenance-container='ServiceTermSuperannuationSchemes']"), ComponentIdentifier);
                });
                return returnValue;
            }
        }

        public class SuperannuationScheme : GridRow
        {
            [FindsBy(How = How.CssSelector, Using = "[name$='Code']")]
            private IWebElement _code;

            #region Properties

            public string Code
            {
                set { _code.SetText(value); }
                get { return _code.GetAttribute("value"); }
            }

            #endregion
        }


        public GridComponent<FinalcialSubGroupTableRow> FinalcialSubGroupTable
        {
            get
            {
                GridComponent<FinalcialSubGroupTableRow> returnValue = null;
                Retry.Do(() =>
                {
                    returnValue = new GridComponent<FinalcialSubGroupTableRow>(By.CssSelector("[data-maintenance-container='ServiceTermFinancialSubGroup']"), ComponentIdentifier);
                });
                return returnValue;
            }
        }


        public class FinalcialSubGroupTableRow : GridRow
        {
            [FindsBy(How = How.CssSelector, Using = "[name$='Code']")]
            private IWebElement _code;

            [FindsBy(How = How.CssSelector, Using = "[name$='Description']")]
            private IWebElement _Description;

            public string Group
            {
                set { _code.SetText(value); }
                get { return _code.GetAttribute("Value"); }
            }

            public string Descriptions
            {
                set { _Description.SetText(value); }
                get { return _Description.GetAttribute("Value"); }
            }
        }



        #endregion

        #region Methods

        /// <summary>
        /// Au : Luong.Mai
        /// Des : Select Pay Adwards Tab
        /// </summary>
        public void SelectPayAdwardsTab()
        {
            _payAdwardsTab.Click();
            Wait.WaitLoading();
        }

        /// <summary>
        /// Au : Huy.Quoc.Vo
        /// Des : Select Allowance Tab
        /// </summary>
        public void SelectAllowanceTab()
        {
            _allowanceTab.ClickByJS();
            Wait.WaitLoading();
        }
        /// <summary>
        /// Au : Huy.Quoc.Vo
        /// Des : Select Post Type Tab
        /// </summary>
        public void SelectPostTypesTab()
        {
            _postTypeTab.Click();
        }
        /// <summary>
        /// Au : Huy.Quoc.Vo
        /// Des : Select Supperannuation Scheme Tab
        /// </summary>
        public void SelectSupperannuationSchemeTab()
        {
            _superannuationSchemesTab.Click();
        }
        /// <summary>
        /// Au : Huy.Quoc.Vo
        /// Des : Select Finalcial Sub Groups Tab
        /// </summary>
        public void SelectFinalcialSubGroupTab()
        {
            _financialSubGroupsTab.Click();
        }

        public PayScaleDialog AddPayScale()
        {
            SelectPayAdwardsTab();
            _addPayScaleLink.ClickByJS();
            Wait.WaitForAjaxReady(SimsBy.CssSelector(".locking-mask"));
            return new PayScaleDialog();
        }

        public AllowanceTripletDialog AddAllowance()
        {
            SelectAllowanceTab();
            _addAllowanceLink.ClickByJS();
            Wait.WaitForAjaxReady(SimsBy.CssSelector(".locking-mask"));
            return new AllowanceTripletDialog();
        }

        public PostTypeTripletDialog AddPostType()
        {
            SelectPostTypesTab();
            _addPostTypeLink.ClickByJS();
            Wait.WaitForAjaxReady(SimsBy.CssSelector(".locking-mask"));
            return new PostTypeTripletDialog();
        }

        public SupperannuationSchemeTripletDialog AddSuperannuationScheme()
        {
            SelectSupperannuationSchemeTab();
            _addSuperannuationSchemeLink.ClickByJS();
            Wait.WaitForAjaxReady(SimsBy.CssSelector(".locking-mask"));
            return new SupperannuationSchemeTripletDialog();
        }

        public void AddFinalcialSubGroup()
        {
            SelectFinalcialSubGroupTab();
            _addFinalcialButton.ClickByJS();
            Wait.WaitForAjaxReady(SimsBy.CssSelector("[data-automation-id='add__button'] button[disabled='disabled']"));
        }

        public void ClickSave()
        {
            _saveButton.ClickByJS();
            Wait.WaitForAjaxReady(SimsBy.CssSelector(".locking-mask"));
            Refresh();
        }

        public bool IsSuccessMessageDisplay()
        {
            Wait.WaitForControl(SimsBy.CssSelector("[data-automation-id='status_success']"));
            return SeleniumHelper.DoesWebElementExist(_statusSuccess);
        }

        public void ClickDeleteTableRow(GridRow record)
        {
            if (record != null)
            {
                record.DeleteRow();
            }
        }
        #endregion
    }
}
