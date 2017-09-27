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

namespace Pupil.BulkUpdate.Tests
{
    public class ApplicationStatusConfirmDialogOnSave : BaseSeleniumComponents
    {
        private static readonly By ButtonCancel = By.CssSelector("div[data-invoke-dialog-name='application-status-admit-confirm-dialog'] button[data-automation-id='cancel_button']");

        public void ClickOnCancelButton()
        {
            SeleniumHelper.FindAndClick(ButtonCancel);
            var wait = new WebDriverWait(WebContext.WebDriver, TestDefaults.Default.TimeOut);
            wait.Until(ExpectedConditions.InvisibilityOfElementLocated(ButtonCancel));
        }
    }
}

