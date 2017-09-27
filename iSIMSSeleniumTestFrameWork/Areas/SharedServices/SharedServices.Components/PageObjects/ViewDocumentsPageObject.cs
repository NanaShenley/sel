using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using OpenQA.Selenium.Support.UI;
using SharedComponents.BaseFolder;
using SharedComponents.Helpers;
using SharedServices.Components.Properties;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading;
using SeSugar.Automation;
using SharedServices.Components.Common;
using TestSettings;
using WebDriverRunner.webdriver;
#pragma warning disable 169

namespace SharedServices.Components.PageObjects
{
    public class ViewDocumentsPageObject : BaseSeleniumComponents
    {
        public By ComponentIdentifier
        {
            get { return SeSugar.Automation.SimsBy.AutomationId("view_document_dialog"); }
        }

        public By ConfirmationDialogIdentifier
        {
            get { return SeSugar.Automation.SimsBy.AutomationId("delete_document_confirmation_dialog"); }
        }

        private readonly By _gridSelectorBy = By.Id("Documents");
        private const string DeleteDocumentButton = "remove_document_button";
        private const string YesButton = "yes_button";
        private const string OkButton = "ok_button";
        private const string BrowseFilesLink = "browse_files";
        public const string viewdoc = "view_document_dialog";

        readonly WebDriverWait _waiter = new WebDriverWait(WebContext.WebDriver, TimeSpan.FromSeconds(2000));

        public IWebElement OkButtonElement
        {
            get
            {
                _waiter.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("div[data-section-id='dialog-detail'] button[data-automation-id='ok_button']"))).Click();
                return WebContext.WebDriver.FindElement(SeleniumHelper.SelectByDataAutomationID(OkButton));
            }
        }

        private IWebElement BrowseFileElement
        {
            get
            {
                WaitUntilEnabled(SeleniumHelper.SelectByDataAutomationID(BrowseFilesLink));
                return WebContext.WebDriver.FindElement(SeleniumHelper.SelectByDataAutomationID(BrowseFilesLink));
            }
        }

        private IWebElement DeleteButton
        {
            get
            {
                WaitUntilEnabled(SeleniumHelper.SelectByDataAutomationID(DeleteDocumentButton));
                return WebContext.WebDriver.FindElement(SeleniumHelper.SelectByDataAutomationID(DeleteDocumentButton));
            }
        }

        private IWebElement YesButtonElement
        {
            get
            {
                WaitUntilEnabled(SeleniumHelper.SelectByDataAutomationID(YesButton));
                return WebContext.WebDriver.FindElement(SeleniumHelper.SelectByDataAutomationID(YesButton));
            }
        }

        public ViewDocumentsPageObject()
        {
            PageFactory.InitElements(WebContext.WebDriver, this);
            WaitUntillAjaxRequestCompleted();
        }
        
        public void AttachDocument(string fileName)
        {
            AutomationSugar.WaitFor(new ByChained(ComponentIdentifier,SeSugar.Automation.SimsBy.AutomationId(BrowseFilesLink)));
            SelectFile(fileName);
        }

        public void DeleteDocument()
        {
            AutomationSugar.WaitFor(new ByChained(SeSugar.Automation.SimsBy.AutomationId("delete_this_row?_button")));
            AutomationSugar.ClickOn(new ByChained(SeSugar.Automation.SimsBy.AutomationId("delete_this_row?_button")));
            AutomationSugar.WaitForAjaxCompletion();

            AutomationSugar.WaitFor(new ByChained(By.CssSelector(".popover"), SeSugar.Automation.SimsBy.AutomationId("Yes_button")));
            AutomationSugar.ClickOn(new ByChained(By.CssSelector(".popover"), SeSugar.Automation.SimsBy.AutomationId("Yes_button")));
            AutomationSugar.WaitForAjaxCompletion();
        }

        public void WaitForGrid()
        {
            Thread.Sleep(1000);
            WaitUntilDisplayed(TestDefaults.Default.TimeOut, _gridSelectorBy);
        }

        public string GetRowText(int rowNumber)
        {
            Thread.Sleep(1000);
            IWebElement gridElement = SeleniumHelper.Get(_gridSelectorBy);
            Assert.IsNotNull(gridElement);
            IWebElement linkColumn = gridElement.FindElement(By.CssSelector(string.Format("[href *= '{0}']", Path.GetFileName(Settings.Default.FilePath).ToLower())));
            Assert.IsNotNull(linkColumn);
            return linkColumn.Text;
        }

        public void ClickDeleteDocumentButton()
        {
            AutomationSugar.WaitFor(new ByChained(ComponentIdentifier, SeSugar.Automation.SimsBy.AutomationId(DeleteDocumentButton)));
            AutomationSugar.ClickOn(new ByChained(ComponentIdentifier, SeSugar.Automation.SimsBy.AutomationId(DeleteDocumentButton)));
        }

        public void ClickDeleteCheckBox()
        {
            Thread.Sleep(3000);

            IWebElement gridElement = SeleniumHelper.Get(_gridSelectorBy);
            Assert.IsNotNull(gridElement);
            IWebElement checkBoxElement = gridElement.FindElement(By.CssSelector("input[type='checkbox']"));
            
            checkBoxElement.Click();
        }

        public void ClickOkButton()
        {
            AutomationSugarHelpers.WaitForAndClickOn(new ByChained(ComponentIdentifier, SeSugar.Automation.SimsBy.AutomationId("ok_button")));
        }

        public void ClickYesButton()
        {
            AutomationSugarHelpers.WaitForAndClickOn(new ByChained(ConfirmationDialogIdentifier, SeSugar.Automation.SimsBy.AutomationId(YesButton)));
        }

        public void CheckForEmptyGrid()
        {
            IWebElement gridElement = SeleniumHelper.Get(_gridSelectorBy);
            Assert.IsNotNull(gridElement);
        }

        public void CheckForGrid()
        {
            By attachmentList = By.CssSelector("div[id='DocumentStoreFiles'] div[column='1'] span.btn-text-afforded");
            ReadOnlyCollection<IWebElement> attachmentlist = WebContext.WebDriver.FindElements(attachmentList);
            foreach (IWebElement eachelement in attachmentlist)
            {
                const string expectedAttachmentName = "testfile.txt";
                string attachmentName = eachelement.Text;
                Assert.IsTrue(expectedAttachmentName.Contains(attachmentName));
            }
        }

        public int CheckForDocumentCount()
        {
            By attachmentList = By.CssSelector("div[id='Documents'] div[column='1'] ");
            ReadOnlyCollection<IWebElement> attachmentlist = WebContext.WebDriver.FindElements(attachmentList);
            return attachmentlist.Count;
        }

        public string GetDocumentCount()
        {
            Thread.Sleep(2000);
            string countstring = WebContext.WebDriver.FindElement(By.CssSelector("button[title='Documents'] span.btn-text")).Text;
            return countstring;
        }

        private void SelectFile(string fileName, bool createPath = true)
        {
            if (!createPath)
            {
                string currentDir = Directory.GetCurrentDirectory();
                string absolutePath = Path.GetFullPath(Path.Combine(currentDir, fileName));
                BrowseFileElement.SendKeys(absolutePath);
            }
            else
            {
                BrowseFileElement.SendKeys(fileName);
            }
            ((IJavaScriptExecutor)WebContext.WebDriver).ExecuteScript(
                "$(\"[data-automation-id='{0}']\").trigger(\"change\")", BrowseFilesLink);
        }
    }
}