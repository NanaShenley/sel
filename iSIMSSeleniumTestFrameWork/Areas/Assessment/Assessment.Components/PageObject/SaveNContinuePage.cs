using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assessment.Components.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using SharedComponents.BaseFolder;
using OpenQA.Selenium.Support.PageObjects;
using OpenQA.Selenium.Support.UI;
using SharedComponents.Helpers;
using WebDriverRunner.webdriver;
using System.Threading;
using OpenQA.Selenium.Interactions;

namespace Assessment.Components.PageObject
{
    public class SaveNContinuePage
    {
        public SaveNContinuePage()
        {
            WebContext.WebDriver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(MarksheetConstants.Timeout));
            PageFactory.InitElements(WebContext.WebDriver, this);
        }

        [FindsBy(How = How.CssSelector, Using = "div[data-section-id='confirm-commit-dialog']")]
        private readonly IWebElement SaveNContinuePopUpWindow = null;

        [FindsBy(How = How.CssSelector, Using = "div[data-automation-id='confirm-commit-dialog'] div.modal-body")]
        private readonly IWebElement SaveNContinuePopUpWindowHeading = null;

        [FindsBy(How = How.CssSelector, Using = "button[data-automation-id='save_continue_commit_dialog']")]
        private readonly IWebElement SaveNContinueButton = null;

        [FindsBy(How = How.CssSelector, Using = "button[data-automation-id='ignore_commit_dialog']")]
        private readonly IWebElement DontSaveButton = null;

        [FindsBy(How = How.CssSelector, Using = "button[data-automation-id='cancel_commit_dialog']")]
        private readonly IWebElement CancelButton = null;

        [FindsBy(How = How.CssSelector, Using = "a[data-automation-id='tab_Create_Marksheet_close_button']")]
        private readonly IWebElement CreateMarksheetCloseButton = null;

        //Hydration Logic web Elements

        [FindsBy(How = How.CssSelector, Using = "li[data-automation-id='quicklinks_top_level_pupil_submenu_pupilrecords']")]
        private IWebElement PupilRecordButton;

        [FindsBy(How = How.CssSelector, Using = "a[data-automation-id='add_new_pupil_button']")]
        private IWebElement AddNewPupilButton;

        [FindsBy(How = How.CssSelector, Using = "button[data-automation-id='task_menu']")]
        private IWebElement TaskMenu;

        [FindsBy(How = How.CssSelector, Using = "a[data-ajax-url*='CreateMarksheet/Details']")]
        private IWebElement ManageTemplatesLink;

        [FindsBy(How = How.CssSelector, Using = "button[title='Template Name']")]
        private IWebElement EnterMarksheetTemplateNameLabel;


        WebDriverWait waiter = new WebDriverWait(WebContext.WebDriver, TimeSpan.FromSeconds(MarksheetConstants.Timeout));


        /// <summary>
        /// Waits till the Save & Continue Pop Up gets Open
        /// </summary>
        public void WaitUntilSaveNContinuePopUpOpens()
        {
            //waiter.Until(ExpectedConditions.TextToBePresentInElement(SaveNContinuePopUpWindowHeading, "There are unsaved changes"));
            while (true)
            {
                if (SaveNContinuePopUpWindow.GetAttribute("aria-hidden") == "false")
                    break;                
            }
        }

        /// <summary>
        /// Clicks the Save & Continue Button
        /// </summary>
        public void ClickSaveNContinueButton()
        {
            waiter.Until(ExpectedConditions.TextToBePresentInElement(SaveNContinueButton, "Save and Continue"));
            SaveNContinueButton.Click();
        }

        /// <summary>
        /// Clicks the Dont Save Button
        /// </summary>
        public void ClickDontSaveButton()
        {
            waiter.Until(ExpectedConditions.TextToBePresentInElement(DontSaveButton, "Don't Save"));
            DontSaveButton.Click();
        }

        /// <summary>
        /// Clicks the Cancel Button
        /// </summary>
        public void ClickCancelButton()
        {
            waiter.Until(ExpectedConditions.TextToBePresentInElement(CancelButton, "Cancel"));
            CancelButton.Click();
        }

        /// <summary>
        /// Clicking the Pupil Record Button
        /// </summary>
        public SaveNContinuePage NavigatetoPupilRecordScreen()
        {
            waiter.Until(ExpectedConditions.ElementToBeClickable(PupilRecordButton));
            PupilRecordButton.Click();
            return new SaveNContinuePage();
        }

        /// <summary>
        /// Clicking the Pupil Record Button and then coming back to the Marksheet Template Screen just to check if the 
        /// </summary>
        public MarksheetTemplateDetails NavigatetoCreateMarksheetTemplateScreen()
        {
            waiter.Until(ExpectedConditions.TextToBePresentInElement(AddNewPupilButton, "Add New Pupil"));
            TaskMenu.Click();
            waiter.Until(ExpectedConditions.ElementToBeClickable(ManageTemplatesLink)).Click();
            waiter.Until(ExpectedConditions.TextToBePresentInElement(EnterMarksheetTemplateNameLabel, "Template Name"));
            return new MarksheetTemplateDetails();
        }

        /// <summary>
        /// Clicks the Create Marksheet Tab Close Button 
        /// </summary>
        public void ClickCreateMarksheetCloseButton()
        {
            CreateMarksheetCloseButton.Click();
        }

    }
}
