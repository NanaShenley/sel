using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using SeSugar.Automation;
using Staff.POM.Base;
using Staff.POM.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Staff.POM.Components.Staff
{
    public class AddAddressTripletDialog : BaseDialogComponent
    {
        public override By DialogIdentifier
        {
            get { return SimsBy.Id("dialog-palette-editor"); }
        }

        #region Page Properties

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='create_new_address_button']")]
        private IWebElement _createButton;

        #endregion

        #region Page Action

        public AddAddressDialog CreateAddress()
        {
            _createButton.Click();
            return new AddAddressDialog();
        }

        #endregion
    }
}
