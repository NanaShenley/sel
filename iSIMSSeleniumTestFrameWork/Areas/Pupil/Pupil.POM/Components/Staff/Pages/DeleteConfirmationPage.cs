﻿using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using POM.Base;
using POM.Helper;

namespace POM.Components.Staff
{
    public class DeleteConfirmationPage : BaseComponent
    {
        public override By ComponentIdentifier
        {
            get { return SimsBy.AutomationId("confirm_delete_dialog"); }
        }

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='continue_with_delete_button']")]
        private IWebElement _continueDeleteButton;

        public void ConfirmDelete()
        {
            if (_continueDeleteButton.IsExist())
            {
                _continueDeleteButton.ClickByJS();
                Wait.WaitForAjaxReady(SimsBy.CssSelector(".locking-mask"));
            }
        }
    }
}