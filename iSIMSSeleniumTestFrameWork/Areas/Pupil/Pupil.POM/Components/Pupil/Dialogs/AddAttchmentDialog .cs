using OpenQA.Selenium;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Support.PageObjects;
using POM.Base;
using POM.Helper;
using Staff.Selenium.Components.Helpers;
using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Automation;
using System.Windows.Forms;
using WebDriverRunner.webdriver;

namespace POM.Components.Pupil
{
    public class AddAttchmentDialog : BaseDialogComponent
    {
        public override By DialogIdentifier
        {
            get { return SimsBy.CssSelector("[data-automation-id='upload_document_dialog']"); }
        }

        #region Page properties

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='upload_document_button']")]
        private IWebElement _uploadButton;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='sharepoint_upload_browse']")]
        private IWebElement _browserButton;

        #endregion

        #region Page actions

        public static AddAttchmentDialog Create()
        {
            return new AddAttchmentDialog();
        }

        public IntPtr doesWindowExist(string wName)
        {
            IntPtr hWnd = IntPtr.Zero;
            foreach (Process pList in Process.GetProcesses())
            {
                if (pList.MainWindowTitle.Equals(wName))
                {
                    hWnd = pList.MainWindowHandle;
                }
            }
            return hWnd; //Should contain the handle but may be zero if the title doesn't match
        }

        public ViewDocumentDialog UploadDocument()
        {
            Refresh();
            Retry.Do(_uploadButton.ClickByJS, 100, 20, _uploadButton.Click);
            Wait.WaitForAjaxReady(By.CssSelector(".locking-mask"));
            return new ViewDocumentDialog();
        }

        private void ClickToAppearDialog(int number = 6)
        {
            do
            {
                Retry.Do(_browserButton.Click);
                IntPtr handle = this.doesWindowExist("Open");
                if (handle != IntPtr.Zero)
                {
                    return;
                }
                number--;
            }
            while (number > 0);
        }

        private bool OpenTheUploadDIalog(int timeout = 16)
        {
            bool isExist = false;
            do
            {
                Refresh();
                _browserButton.Click();

                ICapabilities capabilities = ((RemoteWebDriver)WebContext.WebDriver).Capabilities;

                AutomationElement window = null;
                AutomationElement FileFieldName = null;

                // Detect element
                if (capabilities.BrowserName.ToLower().Contains("chrome"))
                {
                    window = AutomationElement.RootElement.FindFirst(TreeScope.Children, new PropertyCondition(AutomationElement.NameProperty, "SIMS - Google Chrome"));
                }
                else
                {
                    window = AutomationElement.RootElement.FindFirst(TreeScope.Children, new PropertyCondition(AutomationElement.NameProperty, "SIMS - Internet Explorer"));
                }

                if (window != null)
                {
                    Condition condition = new AndCondition(
                    new PropertyCondition(AutomationElement.LocalizedControlTypeProperty, "edit"),
                    new PropertyCondition(AutomationElement.NameProperty, "File name:"));

                    FileFieldName = window.FindFirst(TreeScope.Descendants, condition);
                }

                isExist = Wait.WaitForUIAutoElementReady(FileFieldName);
                timeout--;
            }
            while (timeout > 0 && isExist == false);
            return false;
        }

        public void BrowserToDocument(string fileName = "document.txt")
        {
            string pathFile = Path.GetDirectoryName(Path.GetDirectoryName(System.IO.Directory.GetCurrentDirectory())) + "\\Resource\\" + fileName;
            pathFile = pathFile.Replace("\\TestRunner", "");
            OpenTheUploadDIalog();
            UIAutoEnterValueIntoOpenDialog(pathFile);
        }

        public void UIAutoEnterValueIntoOpenDialog(string value)
        {
            ICapabilities capabilities = ((RemoteWebDriver)WebContext.WebDriver).Capabilities;

            AutomationElement window = null;
            if (capabilities.BrowserName.ToLower().Contains("chrome"))
            {
                window = AutomationElement.RootElement.FindFirst(TreeScope.Children, new PropertyCondition(AutomationElement.NameProperty, "SIMS - Google Chrome"));
            }
            else
            {
                window = AutomationElement.RootElement.FindFirst(TreeScope.Children, new PropertyCondition(AutomationElement.NameProperty, "SIMS - Internet Explorer"));
            }

            if (window != null)
            {
                // Find Open dialog
                Condition condition = new AndCondition(
                new PropertyCondition(AutomationElement.LocalizedControlTypeProperty, "edit"),
                new PropertyCondition(AutomationElement.NameProperty, "File name:"));

                var FileFieldName = window.FindFirst(TreeScope.Descendants, condition);
                FileFieldName.SetFocus();
                ValuePattern valuePatternA =
                FileFieldName.GetCurrentPattern(ValuePattern.Pattern) as ValuePattern;
                valuePatternA.SetValue(value);
                SendKeys.SendWait("{Enter}");
            }
        }

        #endregion
    }
}
