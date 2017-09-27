using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SharedComponents.BaseFolder;
using SharedComponents.Helpers;
using TestSettings;
using WebDriverRunner.webdriver;

namespace Admissions.Component
{
    public class ApplicationStatusConfirmDialogOnSave : BaseSeleniumComponents
    {
        private static readonly By ButtonCancel = By.CssSelector("div[data-invoke-dialog-name='application-status-admit-confirm-dialog'] button[data-automation-id='cancel_button']");
        private static readonly By ButtonSaveContinue = By.CssSelector("div[data-automation-id='confirm_commit_dialog'] button[data-automation-id='save_continue_commit_dialog']");
        private static readonly By ButtonDontSave = By.CssSelector("div[data-automation-id='confirm_commit_dialog'] button[data-automation-id='ignore_commit_dialog']");

        public void ClickOnCancelButton()
        {
            SeleniumHelper.FindAndClick(ButtonCancel);
            var wait = new WebDriverWait(WebContext.WebDriver, TestDefaults.Default.TimeOut);
            wait.Until(ExpectedConditions.InvisibilityOfElementLocated(ButtonCancel));
        }
        public void ClickOnSaveandContinueButton()
        {
            SeleniumHelper.FindAndClick(ButtonSaveContinue);
            var wait = new WebDriverWait(WebContext.WebDriver, TestDefaults.Default.TimeOut);
            wait.Until(ExpectedConditions.InvisibilityOfElementLocated(ButtonSaveContinue));
        }
        public void ClickOnButtonDontSave()
        {
            SeleniumHelper.FindAndClick(ButtonDontSave);
            var wait = new WebDriverWait(WebContext.WebDriver, TestDefaults.Default.TimeOut);
            wait.Until(ExpectedConditions.InvisibilityOfElementLocated(ButtonDontSave));
        }
    }
}

