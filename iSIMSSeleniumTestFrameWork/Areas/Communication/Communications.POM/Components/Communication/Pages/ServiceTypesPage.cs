using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using POM.Base;
using POM.Helper;


namespace POM.Components.Communication
{
    public class ServiceTypesPage : BaseComponent
    {
        public override By ComponentIdentifier
        {
            get { return SimsBy.CssSelector("[data-automation-id='lookup_detail_provider']"); }
        }

        #region Page properties

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='well_know_action_save']")]
        private IWebElement _saveButton;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='status_success']")]
        private IWebElement _statusMessage;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='create_service_ServiceType']")]
        private IWebElement _addServiceProvidedButton;

        public GridComponent<ServiceTypeRow> ServiceTypes
        {
            get
            {
                GridComponent<ServiceTypeRow> returnValue = null;
                Retry.Do(() =>
                {
                    returnValue = new GridComponent<ServiceTypeRow>(By.CssSelector("[data-maintenance-grid-id='LookupsWithProviderGrid1']"), ComponentIdentifier);
                });
                return returnValue;
            }
        }

        public class ServiceTypeRow : GridRow
        {
            [FindsBy(How = How.CssSelector, Using = "[name$='Code']")]
            private IWebElement _code;

            public string Code
            {
                get { return _code.GetValue(); }
                set { _code.SetText(value); }
            }

            [FindsBy(How = How.CssSelector, Using = "[name$='Description']")]
            private IWebElement _description;

            public string Description
            {
                get { return _description.GetValue(); }
                set { _description.SetText(value); }
            }

            [FindsBy(How = How.CssSelector, Using = "[name$='DisplayOrder']")]
            private IWebElement _displayOrder;

            public string DisplayOrder
            {
                get { return _displayOrder.GetValue(); }
                set { _displayOrder.SetText(value); }
            }

            [FindsBy(How = How.CssSelector, Using = "[name$='IsVisible']")]
            private IWebElement _isVisible;

            public bool IsVisible
            {
                get { return _isVisible.IsChecked(); }
                set { _isVisible.Set(value); }
            }

            [FindsBy(How = How.CssSelector, Using = "[name$='ResourceProviderName']")]
            private IWebElement _resourceProviderName;

            public string ResourceProviderName
            {
                get { return _resourceProviderName.GetValue(); }
                set { _resourceProviderName.SetText(value); }
            }

        }
                  
        #endregion

        #region Actions

       public void Save()
        {
            _saveButton.Click();
            Wait.WaitForAjaxReady(SimsBy.CssSelector(".locking-mask"));
            System.Threading.Thread.Sleep(2000);
            
        }
     
        public bool IsSuccessMessageIsDisplayed()
        {
            Wait.WaitForControl(SimsBy.CssSelector("[data-automation-id='status_success']"));
            return SeleniumHelper.DoesWebElementExist(SimsBy.CssSelector("[data-automation-id='status_success']"));
        }

        public bool IsWarningMessageIsDisplayed()
        {
            Wait.WaitForControl(SimsBy.CssSelector("[data-automation-id='status_error']"));
            return SeleniumHelper.DoesWebElementExist(SimsBy.CssSelector("[data-automation-id='status_error']"));
        }

        public void AddServiceProvided()
        {
            _addServiceProvidedButton.ClickByJS();
            Wait.WaitForAjaxReady(By.CssSelector(".locking-mask-loading"));
           // ServiceTypes.Refresh();
        }

        #endregion

    }
}
