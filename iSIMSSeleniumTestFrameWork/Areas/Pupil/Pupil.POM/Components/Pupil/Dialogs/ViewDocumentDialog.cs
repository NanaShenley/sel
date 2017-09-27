using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using POM.Base;
using POM.Helper;
using Staff.Selenium.Components.Helpers;

namespace POM.Components.Pupil
{
    public class ViewDocumentDialog : BaseDialogComponent
    {
        public override By DialogIdentifier
        {
            get { return SimsBy.Id("dialog-palette-editor"); }
        }

        #region Page properties

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='add_document_button']")]
        private IWebElement _addButton;

        [FindsBy(How = How.Id, Using = "Documents")]
        private IWebElement _documentViewTable;

        public WebixComponent Documents
        {
            get { return new WebixComponent(_documentViewTable); }
        }
        #endregion

        #region Page Action

        public AddAttchmentDialog ClickAddAttachment()
        {
            _addButton.ClickByJS();
            Wait.WaitForAjaxReady(By.CssSelector(".locking-mask"));
            return new AddAttchmentDialog();
        }

        #endregion
    }
}
