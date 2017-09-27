using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using PageObjectModel.Base;
using PageObjectModel.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PageObjectModel.Components.HomePages
{
    public class AboutPopover : BaseComponent
    {
        public override By ComponentIdentifier
        {
            get { return SimsBy.AutomationId("popover-custom-id"); }
        }

        [FindsBy(How = How.CssSelector, Using = ".popover-title")]
        private IWebElement _title;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='Shell_Brand_Popup_Version']")]
        private IWebElement _version;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='Shell_Brand_Popup_Shell']")]
        private IWebElement _shell;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='Shell_Brand_Popup_Name']")]
        private IWebElement _signedInUser;

        [FindsBy(How = How.CssSelector, Using = ".popover-content dd:nth-child(8)")]
        private IWebElement _currentAcademicYear;

        [FindsBy(How = How.CssSelector, Using = ".btn-close")]
        private IWebElement _closeButton;

        public string Title
        {
            get { return _title.GetText(); }
        }

        public string Verion
        {
            get { return _version.GetText(); }
        }

        public string Shell
        {
            get { return _shell.GetText(); }
        }

        public string SignedInUser
        {
            get { return _signedInUser.GetText(); }
        }

        public string CurrentAcademicYear
        {
            get { return _currentAcademicYear.GetText(); }
        }

        public void ClosePopup()
        {
            _closeButton.Click();
        }
    }
}
