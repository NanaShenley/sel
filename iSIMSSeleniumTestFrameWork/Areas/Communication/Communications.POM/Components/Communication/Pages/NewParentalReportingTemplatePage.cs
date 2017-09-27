using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using POM.Base;
using POM.Helper;

namespace Communications.POM.Components.Communication.Pages
{
    public class NewParentalReportingTemplatePage : BaseComponent
    {
        public override By ComponentIdentifier
        {
            get { return SimsBy.Id("screen"); }
        }

        [FindsBy(How = How.CssSelector, Using = "button[data-automation-id='pos']")]
        public IWebElement Assessment_Conduct_Review_Button;

        public AttendenceConductReviewPage _ClickAssessmentButton()
        {
            Assessment_Conduct_Review_Button.ClickByJS();
            Wait.WaitForAjaxReady(SimsBy.CssSelector(".locking-mask"));
            return new AttendenceConductReviewPage();
        }
    }
}
