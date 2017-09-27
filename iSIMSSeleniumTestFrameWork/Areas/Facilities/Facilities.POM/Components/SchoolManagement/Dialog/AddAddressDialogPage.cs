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
    public class AddAddressDialogPage : BaseDialogComponent
    {
        public override By DialogIdentifier
        {
            get { return SimsBy.AutomationId("find_address_record_detail"); }
        }

        #region Page Properties

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='find_or_create_an_address_button']")]
        private IWebElement _createButton;

        [FindsBy(How = How.Name, Using = "PAONRange")]
        private IWebElement _buildingNoTextBox;

        [FindsBy(How = How.Name, Using = "PAONDescription")]
        private IWebElement _buildingNameTextBox;

        [FindsBy(How = How.Name, Using = "SAON")]
        private IWebElement _flatTextBox;

        [FindsBy(How = How.Name, Using = "Street")]
        private IWebElement _streetTextBox;

        [FindsBy(How = How.Name, Using = "Locality")]
        private IWebElement _districtTextBox;

        [FindsBy(How = How.Name, Using = "Town")]
        private IWebElement _cityTextBox;

        [FindsBy(How = How.Name, Using = "AdministrativeArea")]
        private IWebElement _countryTextBox;

        [FindsBy(How = How.Name, Using = "PostCode")]
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
