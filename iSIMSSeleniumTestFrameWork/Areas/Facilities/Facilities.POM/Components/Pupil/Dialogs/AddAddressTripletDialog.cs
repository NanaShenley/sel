using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using POM.Base;
using POM.Helper;

namespace POM.Components.Pupil
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
