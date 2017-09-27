using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using POM.Base;
using POM.Helper;
using SeSugar.Automation;
using Retry = POM.Helper.Retry;
using SimsBy = POM.Helper.SimsBy;

namespace POM.Components.Communication
{
    public class AddAddressDialog : BaseDialogComponent
    {
        public override By DialogIdentifier
        {
            get { return SimsBy.AutomationId("find_address_detail"); }
        }

        #region JobStep Buttons

        public void ClickSearch()
        {
            AutomationSugar.WaitFor(new ByChained(this.ComponentIdentifier, SimsBy.AutomationId("find_address_jobstep_button")));
            AutomationSugar.ClickOn(new ByChained(this.ComponentIdentifier, SimsBy.AutomationId("find_address_jobstep_button")));
            AutomationSugar.WaitForAjaxCompletion();
        }

        public void ClickInput()
        {
            AutomationSugar.WaitFor(new ByChained(this.ComponentIdentifier, SimsBy.AutomationId("input_jobstep_button")));
            AutomationSugar.ClickOn(new ByChained(this.ComponentIdentifier, SimsBy.AutomationId("input_jobstep_button")));
            AutomationSugar.WaitForAjaxCompletion();
        }

        public void ClickClear()
        {
            AutomationSugar.WaitFor(new ByChained(this.ComponentIdentifier, SimsBy.AutomationId("clear_jobstep_button")));
            AutomationSugar.ClickOn(new ByChained(this.ComponentIdentifier, SimsBy.AutomationId("clear_jobstep_button")));
            AutomationSugar.WaitForAjaxCompletion();
        }

        #endregion

        #region Search Fields

        [FindsBy(How = How.Name, Using = "MoveDate")]
        private IWebElement _moveDate;

        [FindsBy(How = How.Name, Using = "PAONRangeSearch")]
        private IWebElement _paonRangeSearch;

        [FindsBy(How = How.Name, Using = "PostCodeSearch")]
        private IWebElement _postCodeSearch;

        public string MoveDate
        {
            set { _moveDate.SetText(value); }
            get { return _moveDate.GetValue(); }
        }

        public string PAONRangeSearch
        {
            set { _paonRangeSearch.SetText(value); }
            get { return _paonRangeSearch.GetValue(); }
        }

        public string PostCodeSearch
        {
            set { _postCodeSearch.SetText(value); }
            get { return _postCodeSearch.GetValue(); }
        }

        #endregion

        #region Search Results

        [FindsBy(How = How.Name, Using = "Addresses.dropdownImitator")]
        private IWebElement _addressesDropdown;

        public string Addresses
        {
            get { return _addressesDropdown.GetAttribute("value"); }
            set { _addressesDropdown.EnterForDropDown(value); }
        }

        #endregion

        #region Address Fields

        [FindsBy(How = How.Name, Using = "Address.PAONRange")]
        private IWebElement _buildingNoTextBox;

        [FindsBy(How = How.Name, Using = "Address.PAONDescription")]
        private IWebElement _buildingNameTextBox;

        [FindsBy(How = How.Name, Using = "Address.SAON")]
        private IWebElement _flatTextBox;

        [FindsBy(How = How.Name, Using = "Address.Street")]
        private IWebElement _streetTextBox;

        [FindsBy(How = How.Name, Using = "Address.Locality")]
        private IWebElement _districtTextBox;

        [FindsBy(How = How.Name, Using = "Address.Town")]
        private IWebElement _cityTextBox;

        [FindsBy(How = How.Name, Using = "Address.AdministrativeArea")]
        private IWebElement _countyTextBox;

        [FindsBy(How = How.Name, Using = "Address.PostCode")]
        private IWebElement _postCodeTextBox;

        [FindsBy(How = How.Name, Using = "Country.dropdownImitator")]
        private IWebElement _countryDropdown;

        public string BuildingNo
        {
            set { _buildingNoTextBox.SetText(value); }
            get { return _buildingNoTextBox.GetValue(); }
        }

        public string BuildingName
        {
            set { _buildingNameTextBox.SetText(value); }
            get { return _buildingNameTextBox.GetValue(); }
        }

        public string Flat
        {
            set { _flatTextBox.SetText(value); }
            get { return _flatTextBox.GetValue(); }
        }

        public string Street
        {
            set { _streetTextBox.SetText(value); }
            get { return _streetTextBox.GetValue(); }
        }

        public string District
        {
            set { _districtTextBox.SetText(value); }
            get { return _districtTextBox.GetValue(); }
        }

        public string Town
        {
            set { _cityTextBox.SetText(value); }
            get { return _cityTextBox.GetValue(); }
        }

        public string County
        {
            set { _countyTextBox.SetText(value); }
            get { return _countyTextBox.GetValue(); }
        }

        public string PostCode
        {
            set { _postCodeTextBox.SetText(value); }
            get { return _postCodeTextBox.GetValue(); }
        }

        public string Country
        {
            get { return _countryDropdown.GetAttribute("value"); }
            set { _countryDropdown.EnterForDropDown(value); }
        }

        #endregion
    }
}
