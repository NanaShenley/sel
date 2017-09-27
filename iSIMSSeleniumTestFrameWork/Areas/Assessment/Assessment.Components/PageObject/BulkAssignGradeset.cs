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
    public class BulkAssignGradeset
    {
        public BulkAssignGradeset()
        {
            WebContext.WebDriver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(MarksheetConstants.Timeout));
            PageFactory.InitElements(WebContext.WebDriver, this);
        }

        [FindsBy(How = How.CssSelector, Using = "div[data-grid-id='gridProperties'] input[type='text']")]
        private readonly IWebElement FloodFillTextBox = null;

        [FindsBy(How = How.CssSelector, Using = "div[data-grid-id='gridProperties'] input[type='checkbox']")]
        private readonly IWebElement OverwriteCheckBox = null;

        [FindsBy(How = How.CssSelector, Using = "div[data-grid-id='gridProperties'] span[data-apply-bulkupdate]")]
        private readonly IWebElement ApplyButton = null;

        [FindsBy(How = How.CssSelector, Using = "div[data-grid-id='gridProperties'] span[data-hide-grid-menu]")]
        private readonly IWebElement CancelButton = null;

        [FindsBy(How = How.CssSelector, Using = "div[data-grid-id='gridProperties'] button[data-ajax-url*='/AddGradeset']")]
        private readonly IWebElement AddGradesetButton = null;

        [FindsBy(How = How.CssSelector, Using = "div[data-gradeset-comment='col3'] ")]
        private readonly IWebElement ApplyComment = null;

        WebDriverWait waiter = new WebDriverWait(WebContext.WebDriver, TimeSpan.FromSeconds(MarksheetConstants.Timeout));

        /// <summary>
        /// Click Apply Button
        /// </summary>
        public MarksheetTemplateProperties ClickApplyButton()
        {
            ApplyButton.Click();
            // This method allows user to wait until the results are getting displayed after click of serach button
            while (true)
            {
                if (ApplyButton.GetAttribute("disabled") != "true")
                    break;
            }
            return new MarksheetTemplateProperties();
        }

        /// <summary>
        /// Click Cancel Button
        /// </summary>
        public MarksheetTemplateProperties ClickCancelButton()
        {
            CancelButton.Click();
            // This method allows user to wait until the results are getting displayed after click of serach button
            while (true)
            {
                if (CancelButton.GetAttribute("disabled") != "true")
                    break;
            }
            return new MarksheetTemplateProperties();
        }

        /// <summary>
        /// Enter Gradeset Value in the Flood Fill Text Box
        /// </summary>
        public void EnterGradesetName(string gradesetname)
        {
            FloodFillTextBox.Clear();
            FloodFillTextBox.SetText(gradesetname);
        }

        /// <summary>
        /// Enter Gradeset Value in the Flood Fill Text Box
        /// </summary>
        public void CheckOverwriteCheckBox(bool checkboxstatus)
        {
            try
            {
                if (checkboxstatus == true)
                {
                    if (OverwriteCheckBox.GetAttribute("checked") == "checked")
                        return;
                }
                else if (checkboxstatus == false)
                {
                    if (OverwriteCheckBox.GetAttribute("checked") == "checked")
                        OverwriteCheckBox.Click();
                }
            }
            catch (Exception)
            {
                if (checkboxstatus == true)
                    OverwriteCheckBox.Click();
                else if (checkboxstatus == false)
                    return;
            }
        }

        /// <summary>
        /// Click Add GradeSet Button
        /// </summary>
        public GradesetSearchPanel ClickAddGradeSetButton()
        {
            AddGradesetButton.Click();
            // This method allows user to wait until the results are getting displayed after click of serach button
            while (true)
            {
                if (AddGradesetButton.GetAttribute("disabled") != "true")
                    break;
            }
            Thread.Sleep(2000);
            return new GradesetSearchPanel();
        }
        /// <summary>
        /// Click ApplyGradeset button
        /// </summary>
        public void ApplyGradeset()
        {
            ApplyButton.Click();
            // This method allows user to wait until the results are getting displayed after click of serach button
            while (true)
            {
                if (AddGradesetButton.GetAttribute("disabled") != "true")
                    break;
            }
            
        }
        /// <summary>
        /// Click comment
        /// </summary>
        public MarksheetTemplateProperties SelectComment()
        {
            ApplyComment.Click();
            Thread.Sleep(2000);
            return new MarksheetTemplateProperties();
        }
        /// <summary>
        /// Click comment
        /// </summary>
        public void SelectOverwrite()
        {
            OverwriteCheckBox.Click();
        }


    }
}
