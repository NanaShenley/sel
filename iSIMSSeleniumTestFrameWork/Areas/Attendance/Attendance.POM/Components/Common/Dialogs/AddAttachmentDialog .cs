using OpenQA.Selenium;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Support.PageObjects;
using POM.Base;
using POM.Components.Staff;
using POM.Helper;

using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Automation;
using System.Windows.Forms;
using WebDriverRunner.webdriver;

namespace POM.Components.Common
{
    public class AddAttachmentDialog : BaseDialogComponent
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

        public static AddAttachmentDialog Create()
        {
            return new AddAttachmentDialog();
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

        public StaffViewDocumentDialog StaffUploadDocument()
        {
            Refresh();
            Retry.Do(_uploadButton.ClickByJS, 100, 20, _uploadButton.Click);
            Wait.WaitForAjaxReady(By.CssSelector(".locking-mask"));
            return new StaffViewDocumentDialog();
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

        private AutomationElement OpenTheUploadDialog(int timeout = 20)
        {
            bool isExist = false;
            AutomationElement window = null;
            AutomationElement fileNameTextBox = null;

            ICapabilities capabilities = ((RemoteWebDriver)WebContext.WebDriver).Capabilities;
            string browserName = capabilities.BrowserName.ToLower();
            Refresh();

            while (timeout > 0 && isExist == false)
            {
                //Click Browser button
                _browserButton.Click();
                Wait.WaitLoading();

                // Detect upload dialog
                if (browserName.Contains("chrome"))
                {
                    window = AutomationElement.RootElement.FindFirst(TreeScope.Children, new PropertyCondition(AutomationElement.NameProperty, "SIMS - Google Chrome"));
                }
                else
                {
                    window = AutomationElement.RootElement.FindFirst(TreeScope.Children, new PropertyCondition(AutomationElement.NameProperty, "SIMS - Internet Explorer"));
                }

                if (window != null)
                {
                    Condition condition = new AndCondition(new PropertyCondition(AutomationElement.LocalizedControlTypeProperty, "edit"),
                                                           new PropertyCondition(AutomationElement.NameProperty, "File name:"));

                    fileNameTextBox = window.FindFirst(TreeScope.Descendants, condition);
                    if (fileNameTextBox != null)
                    {
                        isExist = true;
                        break;
                    }
                }
                timeout--;
            }

            //Re-try to open Upload dialog by using ClickByJS on Browser button
            if (fileNameTextBox == null)
            {
                timeout = 20;
                while (timeout > 0 && isExist == false)
                {
                    //Click Browser button
                    _browserButton.ClickByJS();
                    Wait.WaitLoading();

                    // Detect upload dialog
                    if (browserName.Contains("chrome"))
                    {
                        window = AutomationElement.RootElement.FindFirst(TreeScope.Children, new PropertyCondition(AutomationElement.NameProperty, "SIMS - Google Chrome"));
                    }
                    else
                    {
                        window = AutomationElement.RootElement.FindFirst(TreeScope.Children, new PropertyCondition(AutomationElement.NameProperty, "SIMS - Internet Explorer"));
                    }

                    if (window != null)
                    {
                        Condition condition = new AndCondition( new PropertyCondition(AutomationElement.LocalizedControlTypeProperty, "edit"),
                                                                new PropertyCondition(AutomationElement.NameProperty, "File name:"));

                        fileNameTextBox = window.FindFirst(TreeScope.Descendants, condition);
                        if (fileNameTextBox != null)
                        {
                            isExist = true;
                            break;
                        }
                    }
                    timeout--;
                }
            }
            return fileNameTextBox;
        }

        public void BrowserToDocument(string fileName = "document.txt")
        {
            string pathFile = Path.GetDirectoryName(Path.GetDirectoryName(System.IO.Directory.GetCurrentDirectory())) + "\\Resource\\" + fileName;
            pathFile = pathFile.Replace("\\TestRunner", "");
            UIAutoEnterValueIntoOpenDialog(pathFile);
        }

        public void UIAutoEnterValueIntoOpenDialog(string value)
        {
            ICapabilities capabilities = ((RemoteWebDriver)WebContext.WebDriver).Capabilities;
            AutomationElement fileNameTextBox = OpenTheUploadDialog();

            if (fileNameTextBox != null)
            {
                fileNameTextBox.SetFocus();
                ValuePattern valuePatternA =
                fileNameTextBox.GetCurrentPattern(ValuePattern.Pattern) as ValuePattern;
                valuePatternA.SetValue(value);
                SendKeys.SendWait("{Enter}");
            }
        }

        #endregion
    }
}
