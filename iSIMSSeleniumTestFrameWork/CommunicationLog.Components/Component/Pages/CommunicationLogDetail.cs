using CommunicationLog.Components.Component.Dialogs;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using POM.Base;
using POM.Helper;

namespace CommunicationLog.Components.Component.Pages
{
    public class CommunicationLogDetail : BaseComponent
    {
        public override By ComponentIdentifier
        {
            get { return SimsBy.Id("screen"); }
        }

        //add_new_log_button
        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='add_new_log_button']")]
        private IWebElement _AddNewLogButton;

       

        public AddLogDialog AddNewCommunicationLog()
        {
            _AddNewLogButton.Click();
            Wait.WaitForAjaxReady(SimsBy.CssSelector(".locking-mask"));
            return new AddLogDialog();
        }

        
    }
}
