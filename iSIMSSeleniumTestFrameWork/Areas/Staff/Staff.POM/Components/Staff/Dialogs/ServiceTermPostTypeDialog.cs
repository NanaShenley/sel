using OpenQA.Selenium.Support.PageObjects;
using OpenQA.Selenium;
using Staff.POM.Helper;
using Staff.POM.Base;
using SeSugar.Automation;

namespace Staff.POM.Components.Staff
{
    public class ServiceTermPostTypeDialog : BaseDialogComponent
    {
        public override By DialogIdentifier
        {
            get { return SimsBy.AutomationId("service_term_post_types_dialog"); }
        }

        #region Page properties

        [FindsBy(How = How.CssSelector, Using = "[name$='Code']")]
        private IWebElement _code;

        [FindsBy(How = How.CssSelector, Using = "[name$='Description']")]
        private IWebElement _Description;

        [FindsBy(How = How.CssSelector, Using = "[name$='DisplayOrder']")]
        private IWebElement _DisplayOrder;

        [FindsBy(How = How.CssSelector, Using = "[name$='IsVisible']")]
        private IWebElement _IsVisible;

        [FindsBy(How = How.CssSelector, Using = "[name$='PostType.StatutoryPostType.dropdownImitator']")]
        private IWebElement _SWCPostType;

        [FindsBy(How = How.CssSelector, Using = "[name$='PostType.CatholicEducationServicePostType.dropdownImitator']")]
        private IWebElement _CESPostType;

        public string Code
        {
            set { _code.SetText(value); }
            get { return _code.GetAttribute("Value"); }
        }

        public string Description
        {
            set { _Description.SetText(value); }
            get { return _Description.GetAttribute("Value"); }
        }

        public string DisplayOrder
        {
            set { _DisplayOrder.SetText(value); }
            get { return _DisplayOrder.GetAttribute("Value"); }
        }

        public bool IsVisible
        {
            set { _IsVisible.Set(value); }
            get { return _IsVisible.IsChecked(); }
        }

        public string SWCPostType
        {
            set { _SWCPostType.EnterForDropDown(value); }
            get { return _SWCPostType.GetAttribute("Value"); }
        }

        public string CESPostType
        {
            set { _CESPostType.EnterForDropDown(value); }
            get { return _CESPostType.GetAttribute("Value"); }
        }

        #endregion
    }
}