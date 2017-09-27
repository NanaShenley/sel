using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using POM.Helper;
using POM.Base;
using SeSugar.Automation;

namespace POM.Components.Communication
{
    public class AddNewAddressDialog : BaseDialogComponent
    {
        public override By DialogIdentifier
        {
            get { return SeSugar.Automation.SimsBy.AutomationId("find_address_detail"); }
        }

        #region Properties
        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='find_address_jobstep_button']")]
        private IWebElement _addManualAddressButton;

        [FindsBy(How = How.CssSelector, Using = "input[name='Addresses.dropdownImitator']")]
        private IWebElement Addresses;
        //s2id_autogen672_search

        [FindsBy(How = How.Name, Using = "PostCodeSearch")]
        private IWebElement _postCodeTextBox;

        public string PostCode
        {
            set { _postCodeTextBox.SetText(value); }
            get { return _postCodeTextBox.GetValue(); }
        }

        public void ClickManualAddAddress()
        {
            //_addManualAddressButton.Click(); 
            AutomationSugar.WaitFor(new ByChained(this.ComponentIdentifier, SeSugar.Automation.SimsBy.AutomationId("find_address_jobstep_button")));
            AutomationSugar.ClickOn(new ByChained(this.ComponentIdentifier, SeSugar.Automation.SimsBy.AutomationId("find_address_jobstep_button")));
            AutomationSugar.WaitForAjaxCompletion();
        }

        public void SetAddressDropdownValue(string character)
        {
            //Addresses.Clear();
            Addresses.SendKeys(character);
            Addresses.SendKeys(Keys.Enter);
        }

        private readonly AddressSearchDialog _searchCriteria;

        public AddressSearchDialog SearchCriteria
        {
            get { return _searchCriteria; }
        }

        public AddNewAddressDialog()
        {
            //_searchCriteria = new AddressSearchDialog(this);
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

    public class AddressSearchDialog // : SearchCriteriaComponent<AddNewAddressDialog.SearchResultTite>
    {
        //public AddressSearchDialog(BaseComponent component) : base(component) { }

        #region Properties
        [FindsBy(How = How.Name, Using = "PostCode")]
        private IWebElement _fullPostCodeTextbox;

        public string FullPostCode
        {
            set { _fullPostCodeTextbox.SetText(value); }
        }

        #endregion
    }
}
