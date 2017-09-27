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
    public class UploadDocumentDialog : BaseDialogComponent
    {
        public override By DialogIdentifier
        {
            get { return SimsBy.AutomationId("upload_document_dialog"); }
        }

        #region Properties

        [FindsBy(How = How.CssSelector, Using = "data-automation-id='upload_document_button']")]
        private IWebElement _uploadButton;

        #endregion

        #region Actions

        public ViewDocumentDialog ClickUpload()
        {
            _uploadButton.Click();
            return new ViewDocumentDialog();
        }

        #endregion
    }
}
