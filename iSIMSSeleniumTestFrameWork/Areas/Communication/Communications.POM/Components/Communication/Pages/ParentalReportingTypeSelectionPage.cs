using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using POM.Base;
using POM.Helper;

namespace Communications.POM.Components.Communication.Pages
{
    public class ParentalReportingTypeSelectionPage : BaseComponent
    {
        public override By ComponentIdentifier
        {
            get { return SimsBy.Id("screen"); }
        }

        [FindsBy(How = How.Name, Using = "TemplateName")]
        private IWebElement _parentalReportTemplateName;

        [FindsBy(How = How.CssSelector, Using = "button[data-automation-id='next_add_columns']")]
        public IWebElement Next_Add_Columns_Button;

        [FindsBy(How = How.CssSelector, Using = "button[data-automation-id='savetemplate']")]
        public IWebElement Save_Template_button;


        public string ParentalReportTemplateName
        {
            set { _parentalReportTemplateName.SetText(value); }
            get { return _parentalReportTemplateName.GetValue(); }
        }

        public ParentalReportingTypeSelectionPage FillTemplateDetails(string TemplateName)
        {
            _parentalReportTemplateName.WaitUntilState(ElementState.Displayed);
            _parentalReportTemplateName.Clear();
            _parentalReportTemplateName.SendKeys(TemplateName);

            Wait.WaitForDocumentReady();
            return this;
        }
        public NewParentalReportingTemplatePage _clickNextButton()
        {
            Next_Add_Columns_Button.ClickByJS();
            Wait.WaitForAjaxReady(SimsBy.CssSelector(".locking-mask"));
            return new NewParentalReportingTemplatePage();
        }
    }
}
