using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using POM.Base;
using POM.Helper;

namespace POM.Components.Common
{
    public class ConfirmRequiredDialog : BaseDialogComponent
    {
        public override By DialogIdentifier
        {

            get { return SimsBy.CssSelector("[data-automation-id='confirmation_required_dialog']"); }
        }

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='yes_button']")]
        public IWebElement _yesButton;

        public void ClickYes()
        {
            if (_yesButton.IsExist())
            {
                _yesButton.ClickByJS();
            }
        }
    }
}
