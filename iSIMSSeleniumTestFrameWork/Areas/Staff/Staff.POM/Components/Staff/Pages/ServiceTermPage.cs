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
using SeSugar.Automation;
using Staff.POM.Components.Staff.Pages;

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
        private IWebElement _hoursWorkedPerWeekTextBox;

        [FindsBy(How = How.Name, Using = "WeeksWorkedPerYear")]
        private IWebElement _weeksWorkedPerYearTextBox;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='add_allowances_button']")]
        private IWebElement _addAllowanceLink;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='add_superannuation_schemes_button']")]
        private IWebElement _addSuperannuationSchemeLink;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='add__button']")]
        private IWebElement _addFinalcialButton;

        [FindsBy(How = How.CssSelector, Using = "[data-maintenance-container='PayRange']")]
        private IWebElement _payRangeTable;

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

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='section_menu_Pay']")]
        private IWebElement _payPatternTab;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='section_menu_Financial']")]
        private IWebElement _financialSubGroupsTab;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='well_know_action_save']")]
        private IWebElement _saveButton;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='status_success']")]
        private IWebElement _statusSuccess;

        [FindsBy(How = How.Name, Using = "Salaried")]
        private IWebElement _salaried;

        [FindsBy(How = How.Name, Using = "PayPatternApr")]
        private IWebElement _payPatternApr;
        [FindsBy(How = How.Name, Using = "PayPatternMay")]
        private IWebElement _payPatternMay;
        [FindsBy(How = How.Name, Using = "PayPatternJun")]
        private IWebElement _payPatternJun;
        [FindsBy(How = How.Name, Using = "PayPatternJul")]
        private IWebElement _payPatternJul;
        [FindsBy(How = How.Name, Using = "PayPatternAug")]
        private IWebElement _payPatternAug;
        [FindsBy(How = How.Name, Using = "PayPatternSep")]
        private IWebElement _payPatternSep;
        [FindsBy(How = How.Name, Using = "PayPatternOct")]
        private IWebElement _payPatternOct;
        [FindsBy(How = How.Name, Using = "PayPatternNov")]
        private IWebElement _payPatternNov;
        [FindsBy(How = How.Name, Using = "PayPatternDec")]
        private IWebElement _payPatternDec;
        [FindsBy(How = How.Name, Using = "PayPatternJan")]
        private IWebElement _payPatternJan;
        [FindsBy(How = How.Name, Using = "PayPatternFeb")]
        private IWebElement _payPatternFeb;
        [FindsBy(How = How.Name, Using = "PayPatternMar")]
        private IWebElement _payPatternMar;

        [FindsBy(How = How.Name, Using = "TotalPayPattern")]
        private IWebElement _total;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='add_pay_scale_button']")]
        private IWebElement _addPayScaleButton;

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

        public string HoursWorkedPerWeek
        {
            set { _hoursWorkedPerWeekTextBox.SetText(value); }
            get { return _hoursWorkedPerWeekTextBox.GetValue(); }
        }

        public string WeeksWorkedPerYear
        {
            set { _weeksWorkedPerYearTextBox.SetText(value); }
            get { return _weeksWorkedPerYearTextBox.GetValue(); }
        }

        public bool Salaried
        {
            set { _salaried.Set(value); }
            get { return _salaried.IsChecked(); }
        }

        public string PayPatternApr
        {
            set { _payPatternApr.SetText(value); }
            get { return _payPatternApr.GetValue(); }
        }

        public string PayPatternMay
        {
            set { _payPatternMay.SetText(value); }
            get { return _payPatternMay.GetValue(); }
        }

        public string PayPatternJun
        {
            set { _payPatternJun.SetText(value); }
            get { return _payPatternJun.GetValue(); }
        }

        public string PayPatternJul
        {
            set { _payPatternJul.SetText(value); }
            get { return _payPatternJul.GetValue(); }
        }

        public string PayPatternAug
        {
            set { _payPatternAug.SetText(value); }
            get { return _payPatternAug.GetValue(); }
        }

        public string PayPatternSep
        {
            set { _payPatternSep.SetText(value); }
            get { return _payPatternSep.GetValue(); }
        }

        public string PayPatternOct
        {
            set { _payPatternOct.SetText(value); }
            get { return _payPatternOct.GetValue(); }
        }

        public string PayPatternNov
        {
            set { _payPatternNov.SetText(value); }
            get { return _payPatternNov.GetValue(); }
        }

        public string PayPatternDec
        {
            set { _payPatternDec.SetText(value); }
            get { return _payPatternDec.GetValue(); }
        }

        public string PayPatternJan
        {
            set { _payPatternJan.SetText(value); }
            get { return _payPatternJan.GetValue(); }
        }

        public string PayPatternFeb
        {
            set { _payPatternFeb.SetText(value); }
            get { return _payPatternFeb.GetValue(); }
        }

        public string PayPatternMar
        {
            set { _payPatternMar.SetText(value); }
            get { return _payPatternMar.GetValue(); }
        }

        public string TotalWeeks
        {
            set { _total.SetText(value); }
            get { return _total.GetValue(); }
        }

        public bool AprEnabled()
        {
            return _payPatternApr.Enabled;
        }
        public bool MayEnabled()
        {
            return _payPatternMay.Enabled;
        }
        public bool JunEnabled()
        {
            return _payPatternJun.Enabled;
        }
        public bool JulEnabled()
        {
            return _payPatternJul.Enabled;
        }
        public bool AugEnabled()
        {
            return _payPatternAug.Enabled;
        }
        public bool SepEnabled()
        {
            return _payPatternSep.Enabled;
        }
        public bool OctEnabled()
        {
            return _payPatternOct.Enabled;
        }
        public bool NovEnabled()
        {
            return _payPatternNov.Enabled;
        }
        public bool DecEnabled()
        {
            return _payPatternDec.Enabled;
        }
        public bool JanEnabled()
        {
            return _payPatternJan.Enabled;
        }
        public bool FebEnabled()
        {
            return _payPatternFeb.Enabled;
        }
        public bool MarEnabled()
        {
            return _payPatternMar.Enabled;
        }


        public bool PayScaleGridAddButtonEnabled
        {
            get 
            {
                string addPayScaleButton = _addPayScaleButton.GetAttribute("class").ToLower();
                return !addPayScaleButton.Contains("disabled"); 
            }
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

        public class PayScale : GridRow
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

        #region SalaryRangeGrid
        public GridComponent<SalaryRangeTableRow> SalaryRangeTable
        {
            get
            {
                return new GridComponent<SalaryRangeTableRow>(By.CssSelector("[data-maintenance-container='ServiceTermSalaryRanges']"), ComponentIdentifier);
            }
        }

        // TODO 
        public class SalaryRangeTableRow : GridRow
        {
            [FindsBy(How = How.CssSelector, Using = "[name$='SalaryRangeCode']")]
            private IWebElement _code;

            [FindsBy(How = How.CssSelector, Using = "[name$='SalaryRangeDescription']")]
            private IWebElement _description;

            [FindsBy(How = How.CssSelector, Using = "[name$='SalaryRangeMinimumAmount']")]
            private IWebElement _minimumValue;

            [FindsBy(How = How.CssSelector, Using = "[name$='SalaryRangeMaximumAmount']")]
            private IWebElement _maximumValue;

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
            public string MinimumValue
            {
                set { _minimumValue.SetText(value); }
                get { return _minimumValue.GetValue(); }
            }
            public string MaximumValue
            {
                set { _maximumValue.SetText(value); }
                get { return _maximumValue.GetValue(); }
            }
        
            //public PayRangeDialog Edit()
            //{
            //    _editButton.ClickByJS();
            //    return new PayRangeDialog();
            //}
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

        public class PostType : GridRow
        {
            [FindsBy(How = How.CssSelector, Using = "[name$=PostTypeCode]")]
            private IWebElement _code;

            [FindsBy(How = How.CssSelector, Using = "[name$=PostTypeDescription]")]
            private IWebElement _description;

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
        }

        /// <summary>
        /// Au : Huy.Quoc.Vo
        /// Des : Select Allowance Tab
        /// </summary>
        public void SelectAllowanceTab()
        {
            AutomationSugar.ExpandAccordionPanel("section_menu_Allowances");
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

        public void SelectPayPatternTab()
        {
            _payPatternTab.Click();
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
            AutomationSugar.WaitFor(new ByChained(this.ComponentIdentifier, SimsBy.AutomationId("add_pay_scale_button")));
            AutomationSugar.ClickOn(new ByChained(this.ComponentIdentifier, SimsBy.AutomationId("add_pay_scale_button")));
            AutomationSugar.WaitForAjaxCompletion();
            return new PayScaleDialog();
        }

        public SalaryRangeTriple AddSalaryRange()
        {
            SelectPayAdwardsTab();
            AutomationSugar.WaitFor(new ByChained(this.ComponentIdentifier, SimsBy.AutomationId("add_salary_range_button")));
            AutomationSugar.ClickOn(new ByChained(this.ComponentIdentifier, SimsBy.AutomationId("add_salary_range_button")));
            AutomationSugar.WaitForAjaxCompletion();
            return new SalaryRangeTriple();
        }

        public AllowanceTripletDialog AddAllowance()
        {
            SelectAllowanceTab();
            AutomationSugar.WaitFor(new ByChained(this.ComponentIdentifier, SimsBy.AutomationId("add_allowance_button")));
            AutomationSugar.ClickOn(new ByChained(this.ComponentIdentifier, SimsBy.AutomationId("add_allowance_button")));
            AutomationSugar.WaitForAjaxCompletion();
            return new AllowanceTripletDialog();
        }

        public PostTypeTripletDialog AddPostType()
        {
            SelectPostTypesTab();
            AutomationSugar.WaitFor(new ByChained(this.ComponentIdentifier, SimsBy.AutomationId("add_post_type_button")));
            AutomationSugar.ClickOn(new ByChained(this.ComponentIdentifier, SimsBy.AutomationId("add_post_type_button")));
            AutomationSugar.WaitForAjaxCompletion();
            return new PostTypeTripletDialog();
        }

        public SupperannuationSchemeTripletDialog AddSuperannuationScheme()
        {
            SelectSupperannuationSchemeTab();
            AutomationSugar.WaitFor(new ByChained(this.ComponentIdentifier, SimsBy.AutomationId("add_superannuation_scheme_button")));
            AutomationSugar.ClickOn(new ByChained(this.ComponentIdentifier, SimsBy.AutomationId("add_superannuation_scheme_button")));
            AutomationSugar.WaitForAjaxCompletion();
            return new SupperannuationSchemeTripletDialog();
        }

        public void AddFinalcialSubGroup()
        {
            _financialSubGroupsTab.Click();
            AutomationSugar.WaitFor(new ByChained(this.ComponentIdentifier, SimsBy.AutomationId("add_financial_sub-group_button")));
            AutomationSugar.ClickOn(new ByChained(this.ComponentIdentifier, SimsBy.AutomationId("add_financial_sub-group_button")));
            AutomationSugar.WaitForAjaxCompletion();
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
