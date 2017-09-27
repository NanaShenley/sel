using System.Threading;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

namespace Staff.Components.StaffRegression
{
    public class BackgroundUpdateDialog : BaseDialogComponent
    {
        public override By DialogIdentifier
        {
            get { return By.CssSelector("[data-section-id=\"generic-confirm-dialog\"]"); }
        }

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id=\"ok,_return_me_to_the_staff_record_button\"]")]
        private IWebElement _okButton;

        public new void ClickOk()
        {
            Thread.Sleep(250);
            _okButton.Click();
            Thread.Sleep(250);
        }
    }
}
