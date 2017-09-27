using Staff.POM.Base;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Staff.POM.Helper;

namespace Staff.POM.Components.Staff
{
    public class NewAllowancePage : BaseComponent
    {
        public override By ComponentIdentifier
        {
            get { return SimsBy.AutomationId("staff_allowance_dialog"); }
        }

        #region Page properties

        [FindsBy(How = How.Name, Using = "Code")]
        private IWebElement _codeTextBox;

        [FindsBy(How = How.Name, Using = "Description")]
        private IWebElement _descriptionTextBox;

        [FindsBy(How = How.Id, Using = "tri_chkbox_IsVisible")]
        private IWebElement _isVisibleCheckbox;

        [FindsBy(How = How.Name, Using = "DisplayOrder")]
        private IWebElement _displayOrderTexbox;

        [FindsBy(How = How.Name, Using = "AdditionalPaymentCategory.dropdownImitator")]
        private IWebElement _categoryCombobox;

        [FindsBy(How = How.Name, Using = "AllowanceAwardAttached")]
        private IList<IWebElement> _allowanceType;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='ok_button']")]
        private IWebElement _okButton;


        public string Code
        {
            set { _codeTextBox.SetText(value); }
        }

        public string Description
        {
            set {  _descriptionTextBox.SetText(value); }
        }

        public bool IsVisible
        {
            set { _isVisibleCheckbox.Set(true); }
        }

        public string DisplayOrder
        {
            set { _displayOrderTexbox.SetText(value); }
        }

        public string Category
        {
            set { _categoryCombobox.EnterForDropDown(value); }
        }

        public bool PersonalAllowance
        {
            set { _allowanceType[0].Set(value) ; }
        }

        public bool FixedAllowance
        {
            set { _allowanceType[1].Set(value); }
        }




        public GridComponent<AwardsRow> Awards
        {
            get
            {
                GridComponent<AwardsRow> returnValue = null;
                Retry.Do(() =>
                {
                    returnValue = new GridComponent<AwardsRow>(By.CssSelector("[data-maintenance-container='AllowanceAwards']"), ComponentIdentifier);
                });
                return returnValue;
            }
        }


        public class AwardsRow : GridRow
        {
            [FindsBy(How = How.CssSelector, Using = "[name$='Date']")]
            private IWebElement _AwardDate;

            [FindsBy(How = How.CssSelector, Using = "[name$='Amount']")]
            private IWebElement _Amount;

            public string AwardDate
            {
                set { _AwardDate.SetDateTime(value); }
                get { return _AwardDate.GetDateTime(); }
            }

            public string Amount
            {
                set { _Amount.SetText(value); }
                get { return _Amount.GetAttribute("Value"); }
            }
        }

        #endregion

        #region Public methods

        public AllowanceDetailsPage ClickOK()
        {
            _okButton.ClickByJS();
            Wait.WaitForAjaxReady(SimsBy.CssSelector(".locking-mask"));

            //Wait for loading data into table
            Wait.WaitLoading();
            return new AllowanceDetailsPage();
        }

        #endregion
    }
}
