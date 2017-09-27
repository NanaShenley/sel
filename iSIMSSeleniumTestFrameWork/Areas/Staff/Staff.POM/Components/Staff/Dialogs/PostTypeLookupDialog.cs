using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using Staff.POM.Helper;
using Staff.POM.Base;
using SeSugar.Automation;

namespace Staff.POM.Components.Staff
{
    public class PostTypeLookupDialog : BaseDialogComponent
    {
        public override By DialogIdentifier
        {
            get { return SimsBy.AutomationId("post_type_detail_dialog"); }
        }

        [FindsBy(How = How.Name, Using = "Code")]
        private IWebElement _codeTextBox;

        [FindsBy(How = How.Name, Using = "Description")]
        private IWebElement _descriptionTextBox;

        [FindsBy(How = How.CssSelector, Using = "[name='StatutoryPostType.dropdownImitator']")]
        private IWebElement _statutoryPostTypeDropDown;

        public string Code
        {
            set { _codeTextBox.SetText(value); }
            get { return _codeTextBox.GetValue(); }
        }

        public string Description
        {
            set { _descriptionTextBox.SetText(value); }
            get { return _descriptionTextBox.GetValue(); }
        }

        public string StatutoryPostType
        {
            set { _statutoryPostTypeDropDown.EnterForDropDown(value); }
            get { return _statutoryPostTypeDropDown.GetAttribute("Value"); }
        }
    }
}
