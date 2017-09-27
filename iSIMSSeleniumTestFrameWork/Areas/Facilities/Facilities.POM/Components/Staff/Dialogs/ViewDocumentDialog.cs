using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using POM.Base;
using POM.Components.Common;
using POM.Helper;


namespace POM.Components.Staff
{
    public class StaffViewDocumentDialog : BaseDialogComponent
    {
        public override By DialogIdentifier
        {
            get { return SimsBy.AutomationId("view_document_dialog"); }
        }

        #region Page properties

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='add_document_button']")]
        private IWebElement _addButton;

        #endregion

        #region Page Action

        public AddAttachmentDialog ClickAddAttachment()
        {
            _addButton.ClickByJS();
            Wait.WaitForAjaxReady(By.CssSelector(".locking-mask"));
            return new AddAttachmentDialog();
        }

        #endregion
    }
}
