using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using POM.Helper;
using POM.Base;


namespace POM.Components.SchoolManagement
{
    public class AddNewAddressTriplet : BaseDialogComponent
    {
        public override By DialogIdentifier
        {
            get { return SimsBy.AutomationId("find_address_triplet"); }
        }

        #region Properties

        private readonly AddressSearchDialog _searchCriteria;

        public AddressSearchDialog SearchCriteria
        {
            get { return _searchCriteria; }
        }

        public AddNewAddressTriplet()
        {
            _searchCriteria = new AddressSearchDialog(this);
        }

        #endregion


        public class SearchResultTite : SearchResultTileBase 
        {
            [FindsBy(How = How.CssSelector, Using = "[data-automation-id='resultTile']")]
            private IWebElement _name;

            public string Name
            {
                get { return _name.Text; }
            }
        }
    }

    public class AddressSearchDialog : SearchCriteriaComponent<AddNewAddressTriplet.SearchResultTite>
    {
        public AddressSearchDialog(BaseComponent component) : base(component) { }

        #region Properties

        [FindsBy(How = How.Name, Using = "PostCode")]
        private IWebElement _fullPostCodeTextbox;

        [FindsBy(How = How.Name, Using = "Country.dropdownImitator")]
        private IWebElement _countryDropdown;

        public string FullPostCode
        {
            set { _fullPostCodeTextbox.SetText(value); }
        }

        public string Country
        {
            set { _countryDropdown.EnterForDropDown(value); }
        }

        #endregion
    }
}
