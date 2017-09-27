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
using Assessment.Components.PageObject;

namespace Assessment.Components
{
    public class MarksheetTemplateDetails: BaseSeleniumComponents
    {
        public MarksheetTemplateDetails()
        {
            WebContext.WebDriver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(MarksheetConstants.Timeout));
            PageFactory.InitElements(WebContext.WebDriver, this);
        }

        WebDriverWait waiter = new WebDriverWait(WebContext.WebDriver, TimeSpan.FromSeconds(MarksheetConstants.Timeout));

        [FindsBy(How = How.CssSelector, Using = "input[name='MarksheetTemplateName']")]
        private IWebElement MarksheetTemplateName;

        [FindsBy(How = How.CssSelector, Using = "textarea[name='Description']")]
        private IWebElement MarksheetDescription;

        [FindsBy(How = How.CssSelector, Using = "button[title='Remove']")]
        private IWebElement RemoveLink;

        [FindsBy(How = How.CssSelector, Using = "a[id='openproperties']")]
        private readonly IWebElement PropertiesTab = null;


        //List of Grid Elements
        private static By MarksheetNameColumn = By.CssSelector("div[class='webix_ss_body'] div[column='1'] div[class='webix_cell']");
        private static By CheckBox = By.CssSelector("div[class='webix_ss_body'] div[column='0'] input[type='checkbox']");
        private static By IsAvailableCheckBox = By.CssSelector("div[class='webix_ss_body'] div[column='3'] input[type='checkbox']");

        //Single Checkbox Element
        private static By MarksheetActive = By.CssSelector("input[id='tri_chkbox_Active']");

        /// <summary>
        /// sets the value of description property
        /// </summary>
        public void SetDescription(string description)
        {
            MarksheetDescription.WaitUntilState(ElementState.Displayed);
            MarksheetDescription.Clear();
            MarksheetDescription.SendKeys(description);           
        }

        /// <summary>
        /// Returns a New Page Object for Marksheet Template Details
        /// </summary>
        public MarksheetTemplateDetails MarksheetTemplateDetailsPageObject()
        {
            return new MarksheetTemplateDetails();
        }

        /// <summary>
        /// Sets the MarksheetTemplateName
        /// </summary>
        /// <param name="name"></param>
        public void SetMarksheetTemplateName(string name)
        {
            MarksheetTemplateName.WaitUntilState(ElementState.Displayed);
            MarksheetTemplateName.Clear();
            MarksheetTemplateName.SendKeys(name);
            Actions actions = new Actions(WebContext.WebDriver);
            actions.SendKeys(Keys.Tab).Build().Perform();
        }

        /// <summary>
        /// Sets the IsAvailable flag value
        /// </summary>
        /// <param name="active"></param>
        public void SetIsAvailable(bool active)
        {
            SeleniumHelper.Get(General.DataMaintenance).SetCheckBox(MarksheetActive, active);
        }


        /// <summary>
        /// This function will concatenate the Marksheet Template Name and the Year Group to give us the expected Marksheet Name which will be available under the Marksheet Name Column in the Grid
        /// </summary>
        /// <param marksheetTemplateName="Recording Year"></param>
        /// <param selectedGroupsList="Year 1","Year 2"></param>
        public List<string> GetExpectedMarksheetNameList(string marksheetTemplateName, List<string> selectedGroupsList)
        {
            if(string.IsNullOrEmpty(marksheetTemplateName))
                marksheetTemplateName = "New Marksheet";
            return selectedGroupsList.Select(@group => marksheetTemplateName + " - " + @group).ToList();
        }


        /// <summary>
        /// Gets or Returns the Marksheet Template Name
        /// </summary>
        public string GetMarksheetTemplateName()
        {
           return MarksheetTemplateName.GetAttribute("value");
        }

        /// <summary>
        /// Gets all the Marksheet Names from the 'Markhseet Name' column
        /// </summary>
        /// <param name="active"></param>
        public List<string> GetMarksheetName()
        {
            List<string> MarksheetNameResults = new List<string>();
            ReadOnlyCollection<IWebElement> MarksheetNameList = WebContext.WebDriver.FindElements(MarksheetNameColumn);
            if (MarksheetNameList.Count == 0)
            {
                MarksheetNameResults.Add("No Marksheet Name Found");
                return MarksheetNameResults;
            }

            else
            {
                foreach (IWebElement eachelement in MarksheetNameList)
                {
                    MarksheetNameResults.Add(eachelement.Text);
                }
                return MarksheetNameResults;
            }
        }

        /// <summary>
        /// Clicks on the Remove Button
        /// </summary>
        /// <param name="active"></param>
        public MarksheetTemplateDetails Remove()
        {
            waiter.Until(ExpectedConditions.ElementToBeClickable(RemoveLink)).Click();
            return new MarksheetTemplateDetails();
        }

        /// <summary>
        /// Select a Marksheet Name based on Row Number
        /// </summary>
        /// <param name="active"></param>
        public void SelectMarksheetNameCheckbox(int RowNum)
        {
            ReadOnlyCollection<IWebElement> CheckBoxList = WebContext.WebDriver.FindElements(CheckBox);
            int Counter = 1;
            foreach (IWebElement eachelement in CheckBoxList)
            {
                if (RowNum == Counter)
                {
                    eachelement.Click();
                    break;
                }
                Counter++;
            }         
        }

        /// <summary>
        /// Open Properties Tab
        /// </summary>
        public MarksheetTemplateProperties OpenPropertiesTab()
        {
            PropertiesTab.Click();
            while (true)
            {
                if (PropertiesTab.GetAttribute("aria-expanded") == "true")
                    break;
            }
            return new MarksheetTemplateProperties();
        }

    }
}
