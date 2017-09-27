using System;
using System.Collections.ObjectModel;
using Assessment.Components.Common;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using OpenQA.Selenium.Support.UI;
using SharedComponents.Helpers;
using WebDriverRunner.webdriver;
using System.Threading;

namespace Assessment.Components.PageObject
{
    public class Customstatements
    {
        public Customstatements()
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


        [FindsBy(How = How.CssSelector, Using = "div[class='webix_cell webix_cell_select'] input[class='webix_table_checkbox']")]
        public IWebElement UseSchoolDescription;

        WebDriverWait waiter = new WebDriverWait(WebContext.WebDriver, TimeSpan.FromSeconds(MarksheetConstants.Timeout));

        /// <summary>
        /// Clicks on the Search Button
        /// </summary>
        public Customstatements Search()
        {
            SearchButton.Click();
            // This method allows user to wait until the results are getting displayed after click of serach button
            while (true)
            {
                if (SearchButton.GetAttribute("disabled") != "true")
                    break;
            }
            Thread.Sleep(4000);
            return new Customstatements();
        }

        /// <summary>
        /// Clicks on the Save Button
        /// </summary>
        public Customstatements Save()
        {
            waiter.Until(ExpectedConditions.ElementToBeClickable(SaveButton));
            SaveButton.Click();
            return new Customstatements();
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

        public void UseSchoolDescriptionClick(bool isChecked)
        {
            waiter.Until(ExpectedConditions.ElementToBeClickable(UseSchoolDescription));
            Thread.Sleep(2000);
            UseSchoolDescription.SetCheckBox(isChecked);
        }
      
    }
}