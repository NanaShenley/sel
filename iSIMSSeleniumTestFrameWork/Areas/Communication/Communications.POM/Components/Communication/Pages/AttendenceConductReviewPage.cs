using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using POM.Base;
using POM.Helper;

namespace Communications.POM.Components.Communication.Pages
{
    public class AttendenceConductReviewPage : BaseComponent
    {
        public override By ComponentIdentifier
        {
            get { return SimsBy.Id("screen"); }
        }

        [FindsBy(How = How.Id, Using = "Teacher")]
        private IWebElement _teacherCheckBox;

        // adhoc_Done

        [FindsBy(How = How.CssSelector, Using = "button[data-automation-id='adhoc_Done']")]
        public IWebElement _saveButton;

        //savetemplate

        [FindsBy(How = How.CssSelector, Using = "button[data-automation-id='savetemplate']")]
        public IWebElement _saveTemplateButton;

        public bool IsActive
        {
            get { return _teacherCheckBox.IsChecked(); }
            set { _teacherCheckBox.Set(value); }
        }

        public AttendenceConductReviewPage Save()
        {
            _saveButton.ClickByJS();
            Wait.WaitForAjaxReady(SimsBy.CssSelector(".locking-mask"));
            return this;
        }

        public SaveTemplatePage saveTemplate()

        {
            _saveTemplateButton.ClickByJS();
            Wait.WaitForAjaxReady(SimsBy.CssSelector(".locking-mask"));
            return new SaveTemplatePage();
        }

    }
}
