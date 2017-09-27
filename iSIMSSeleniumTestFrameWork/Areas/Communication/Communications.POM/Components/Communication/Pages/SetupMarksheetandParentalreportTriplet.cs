using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using POM.Base;
using POM.Helper;


namespace Communications.POM.Components.Communication.Pages
{
    public class SetupMarksheetandParentalreportTriplet : BaseComponent
    {
        public override By ComponentIdentifier
        {
            get { return SimsBy.Id("screen"); }
        }


        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='parental_report_with_level_new']")]
        private IWebElement _CreatenewParentalReportingTemplateButton;

        [FindsBy(How = How.CssSelector, Using = "[title='Set up Parental Reports']")]
        private IWebElement _CreatenewParentalReportingTemplateDropdownButton;

        public ParentalReportingTypeSelectionPage CreateNewParentalReportingTemplate()
        {
            _CreatenewParentalReportingTemplateButton.ClickByJS();
            Wait.WaitForAjaxReady(SimsBy.CssSelector(".locking-mask"));
            return new ParentalReportingTypeSelectionPage();
        }
        public void SetupParentalReportButton()
        {
            _CreatenewParentalReportingTemplateDropdownButton.ClickByJS();
            Wait.WaitForAjaxReady(SimsBy.CssSelector(".locking-mask"));
        }

    }
}
