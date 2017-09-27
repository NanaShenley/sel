using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using POM.Base;
using POM.Helper;

using System.Collections;
using System.Collections.Generic;

namespace POM.Components.Common
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

        public WebixComponent<WebixCell> Documents
        {
            get { return new WebixComponent<WebixCell>(_documentViewTable); }
        }
        #endregion

        #region Page Action

        public AddAttachmentDialog ClickAddAttachment()
        {
            _addButton.ClickByJS();
            Wait.WaitForAjaxReady(By.CssSelector(".locking-mask"));
            return new AddAttachmentDialog();
        }

        public bool IsDocumentDisplayed(string documentName)
        {
            bool isExisted = false;
            IWebElement documentTable = SeleniumHelper.FindElement(SimsBy.CssSelector(".webix_dtable"));
            IList<IWebElement> documentNameElements = documentTable.FindElements(SimsBy.CssSelector(".webix_ss_center .webix_ss_center_scroll [column='1'] .webix_cell"));
            foreach (var documentElement in documentNameElements)
            {
                if (documentElement.GetText().Equals(documentName))
                {
                    isExisted = true;
                }
            }

            return isExisted;

        }

        #endregion
    }
}
