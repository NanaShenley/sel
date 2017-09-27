using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using POM.Helper;
using POM.Base;
using POM.Components.Common;


namespace POM.Components.SchoolManagement
{
    public class EditAddressDialog : BaseDialogComponent
    {
        public override By DialogIdentifier
        {
            get { return SimsBy.AutomationId("edit_address_record_detail"); }
        }

        #region Page Properties

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='find_or_create_an_address_button']")]
        private IWebElement _createButton;

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
        private IWebElement _countryTextBox;

        [FindsBy(How = How.Name, Using = "Address.PostCode")]
        private IWebElement _postCodeTextBox;

        [FindsBy(How = How.Name, Using = "Address.Country.dropdownImitator")]
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

        public string City
        {
            set { _cityTextBox.SetText(value); }
            get { return _cityTextBox.GetValue(); }
        }

        public string County
        {
            set { _countryTextBox.SetText(value); }
            get { return _countryTextBox.GetValue(); }
        }

        public string PostCode
        {
            set { _postCodeTextBox.SetText(value); }
            get { return _postCodeTextBox.GetValue(); }
        }

        public string CountryPostCode
        {
            get { return _countryDropdown.GetAttribute("value"); }
            set { _countryDropdown.EnterForDropDown(value); }
        }

        #endregion
    }
}
