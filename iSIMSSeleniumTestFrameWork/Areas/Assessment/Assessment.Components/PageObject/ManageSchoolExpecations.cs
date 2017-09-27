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
using SharedComponents.Helpers;

namespace Assessment.Components.PageObject
{
    public class ManageSchoolExpecations
    {
        public ManageSchoolExpecations()
        {
            WebContext.WebDriver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(MarksheetConstants.Timeout));
            PageFactory.InitElements(WebContext.WebDriver, this);
        }

        [FindsBy(How = How.CssSelector, Using = "button[data-automation-id='search_criteria_submit']")]
        private IWebElement SearchButton;

        [FindsBy(How = How.CssSelector, Using = "input[name='LearningLevel.dropdownImitator']")]
        private IWebElement GroupSelector;

        [FindsBy(How = How.CssSelector, Using = "input[name='Subject.dropdownImitator']")]
        private IWebElement SubjectSelector;

        [FindsBy(How = How.CssSelector, Using = "input[name='Strands.dropdownImitator']")]
        private IWebElement StrandSelector;

        [FindsBy(How = How.CssSelector, Using = "a[data-automation-id='well_know_action_save']")]
        private IWebElement SaveButton;

        By EditorCell = By.CssSelector("div[class='webix_dt_editor'] input[type='text']");


        WebDriverWait waiter = new WebDriverWait(WebContext.WebDriver, TimeSpan.FromSeconds(MarksheetConstants.Timeout));

        /// <summary>
        /// Clicks on the Search Button
        /// </summary>
        public ManageSchoolExpecations Search()
        {
            SearchButton.Click();
            // This method allows user to wait until the results are getting displayed after click of serach button
            while (true)
            {
                if (SearchButton.GetAttribute("disabled") != "true")
                    break;
            }
            return new ManageSchoolExpecations();
        }

        /// <summary>
        /// Clicks on the Save Button
        /// </summary>
        public ManageSchoolExpecations Save()
        {
            SaveButton.Click();
            // This method allows user to wait until the results are getting displayed after click of serach button
            while (true)
            {
                if (SearchButton.GetAttribute("disabled") != "true")
                    break;
            }
            return new ManageSchoolExpecations();
        }


        public void waitforSavemessagetoAppear()
        {
            WebDriverWait waiter = new WebDriverWait(WebContext.WebDriver, TimeSpan.FromSeconds(MarksheetConstants.Timeout));
            waiter.Until<Boolean>(d =>
            {
                IWebElement saveMessage = WebContext.WebDriver.FindElement(By.CssSelector("[class='inline-alert-title']"));
                if (saveMessage.Displayed)
                {
                    return true;
                }
                return false;
            }
            );
        }



        /// <summary>
        /// Select a Group on Gradeset Pannel
        /// </summary>
        public void SelectGroup(string GroupName)
        {
            SeleniumHelper.ChooseSelectorOption(GroupSelector, GroupName);
        }

        /// <summary>
        /// Select a Subject on Gradeset Pannel
        /// </summary>
        public void SelectSubject(string SubjectName)
        {
            SeleniumHelper.ChooseSelectorOption(SubjectSelector, SubjectName);
        }

        /// <summary>
        /// Select a Strand on Gradeset Pannel
        /// </summary>
        public void SelectStrand(string StrandName)
        {
            SeleniumHelper.ChooseSelectorOption(StrandSelector, StrandName);
        }

        /// <summary>
        /// Returs a Webelement list for the specifed column
        /// </summary>
        public ReadOnlyCollection<IWebElement> WebelementsList(By ColumnName)
        {
            waiter.Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(ColumnName));
            return WebContext.WebDriver.FindElements(ColumnName);
        }

        /// <summary>
        /// Set a specific value based on the Row Number and column Locator
        /// </summary>
        public ManageSchoolExpecations SetAValue(By colelementlocator, int RowNumber, string cellvalue)
        {
            ReadOnlyCollection<IWebElement> ColumnElementList = WebelementsList(colelementlocator);
            int count = 1;
            foreach (IWebElement eachelement in ColumnElementList)
            {
                if (count == RowNumber)
                {
                    waiter.Until(ExpectedConditions.ElementToBeClickable(eachelement));
                    //eachelement.Click();
                    eachelement.Click();
                    eachelement.SetText(cellvalue);
                    //eachelement.SendKeys(cellvalue);
                    waiter.Until(ExpectedConditions.ElementExists(EditorCell)).SendKeys(cellvalue);
                    break;
                }
                count++;
            }
            return new ManageSchoolExpecations();
        }

    }
}