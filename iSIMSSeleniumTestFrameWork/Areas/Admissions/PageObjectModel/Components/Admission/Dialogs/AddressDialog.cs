using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using PageObjectModel.Base;
using PageObjectModel.Helper;


namespace PageObjectModel.Components.Admission
{
    public class AddressDialog : BaseDialogComponent
    {
        public override By DialogIdentifier
        {
            get { return SimsBy.AutomationId("additional_address_record_detail"); }
        }

        #region Properties

        [FindsBy(How = How.Name, Using = "Address.PAONRange")]
        private IWebElement _houseBuildingNoTextBox;

        [FindsBy(How = How.Name, Using = "Address.PAONDescription")]
        private IWebElement _houseBuildingNameTextBox;

        [FindsBy(How = How.Name, Using = "Address.SAON")]
        private IWebElement _flatApartmentOfficeTextBox;

        [FindsBy(How = How.Name, Using = "Address.Street")]
        private IWebElement _streetTextBox;

        [FindsBy(How = How.Name, Using = "Address.Locality")]
        private IWebElement _districtTextBox;

        [FindsBy(How = How.Name, Using = "Address.Town")]
        private IWebElement _townCityTextBox;

        [FindsBy(How = How.Name, Using = "Address.AdministrativeArea")]
        private IWebElement _countryTextBox;

        [FindsBy(How = How.Name, Using = "Address.PostCode")]
        private IWebElement _postCodeTextBox;

        [FindsBy(How = How.Name, Using = "Address.County.dropdownImitator")]
        private IWebElement _countryDropDown;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='ok_button']")]
        private IWebElement _okButton;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='cancel_button']")]
        private IWebElement _cancelButton;

        #endregion

        #region Actions

        public string HouseBuildingNo
        {
            set { _houseBuildingNoTextBox.SetText(value); }
            get { return _houseBuildingNoTextBox.GetValue(); }
        }

        public string HouseBuildingName
        {
            set { _houseBuildingNameTextBox.SetText(value); }
            get { return _houseBuildingNameTextBox.GetValue(); }
        }
        public string FlatApartmentOffice
        {
            set { _flatApartmentOfficeTextBox.SetText(value); }
            get { return _flatApartmentOfficeTextBox.GetValue(); }
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
        public string TownCity
        {
            set { _townCityTextBox.SetText(value); }
            get { return _townCityTextBox.GetValue(); }
        }

        public string Country
        {
            set { _countryTextBox.SetText(value); }
            get { return _countryTextBox.GetValue(); }
        }

        public string PostCode
        {
            set { _postCodeTextBox.SetText(value); }
            get { return _postCodeTextBox.GetValue(); }
        }


        public string CountryDR
        {
            set { _countryDropDown.EnterForDropDown(value); }
            get { return _countryDropDown.GetValue(); }
        }

        public ApplicationPage OK()
        {
            if (_okButton.IsExist())
            {
                _okButton.ClickByJS();
                Wait.WaitForAjaxReady(SimsBy.CssSelector(".locking-mask"));
            }
            return new ApplicationPage();
            Refresh();
        }

        public ApplicationPage Cancel()
        {
            if (_cancelButton.IsExist())
            {
                _cancelButton.ClickByJS();
                Wait.WaitForAjaxReady(SimsBy.CssSelector(".locking-mask"));
            }
            return new ApplicationPage();
            Refresh();
        }
        #endregion
    }
}
