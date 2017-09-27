using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using POM.Helper;
using POM.Base;


namespace POM.Components.Attendance
{
    public class ViewDocumentDialog : BaseDialogComponent
    {
        public override By DialogIdentifier
        {
            get { return SimsBy.AutomationId("view_document_dialog"); }
        }

        #region Properties

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='add_document_button']")]
        private IWebElement _addDocumentButton;

        #endregion

        #region Actions

        public void ClickAddDocument()
        {
            _addDocumentButton.Click();
            Wait.WaitForAjaxReady(SimsBy.CssSelector(".locking-mask"));
        }

        #endregion
    }
}
