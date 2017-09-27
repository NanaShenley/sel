using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using SharedComponents.BaseFolder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WebDriverRunner.webdriver;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium.Interactions;
using SharedComponents.HomePages;
using POM.Helper;
using System.Collections.ObjectModel;
using NUnit.Framework;

namespace Template.Components
{
    public class TemplateSearchScreen
    {
        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='add_button']")]
        public IWebElement createNewTemplate;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='clone_button']")]
        public IWebElement cloneTemplate;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='well_know_action_save']")]
        public IWebElement saveTemplate;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='well_know_action_Cancel']")]
        public IWebElement cancelTemplate;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='ignore_commit_dialog']")]
        public IWebElement cancelTemplateDontSave;

        [FindsBy(How = How.Name, Using = "NameOfTemplate")]
        public IWebElement templateName;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='status_error']")]
        public IWebElement statusError;

        [FindsBy(How = How.Name, Using = "DocumentTemplateType.dropdownImitator")]
        public IWebElement documentTypeList;

        [FindsBy(How = How.Id, Using = "select2-chosen-1")]
        public IWebElement documentTypeListSelected;

        [FindsBy(How = How.Name, Using = "tri_chkbox_IsVisible")]
        public IWebElement activeCheckBox;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='tab_Manage_Message_Templates_close_button']")]
        public IWebElement manageMsgCloseTab;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='ignore_commit_dialog']")]
        public IWebElement manageMsgWarningDontSave;

        [FindsBy(How = How.Name, Using = "Description")]
        public IWebElement templateDescription;

        [FindsBy(How = How.Name, Using = "EmailSubject")]
        public IWebElement emailSubject;

        [FindsBy(How = How.Name, Using = "TemplateNameforSearchCriteria")]
        public IWebElement templateNameForSearch;

        [FindsBy(How = How.Name, Using = "TemplatePurposeforSearchCriteria.dropdownImitator")]
        public IWebElement templatePurposeForSearch;

        [FindsBy(How = How.Name, Using = "tri_chkbox_IncludeInactiveMessageTemplates")]
        public IWebElement templateInactiveChkBoxForSearch;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='search_criteria_submit']")]
        public IWebElement searchButton;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='search_results_counter']")]
        public IWebElement searchResultCounter;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='delete_button']")]
        public IWebElement deleteButton;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='continue_with_delete_button']")]
        public IWebElement continueWithDeleteButton;

        public TemplateSearchScreen()
        {
            PageFactory.InitElements(WebContext.WebDriver, this);
            BaseSeleniumComponents.WaitUntilDisplayed(By.Name("IncludeInactiveMessageTemplates"));
        }

        public bool IsCreateNewTemplateDisplayed()
        {
            return createNewTemplate.Displayed;
        }

        public bool IsCloneTemplateDisplayed()
        {
            return cloneTemplate.Displayed;
        }


        public bool IsSaveTemplateDisplayed()
        {
            try
            {
                return saveTemplate.Displayed;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public bool IsCancelTemplateDisplayed()
        {
            try
            {
                return cancelTemplate.Displayed;
            }
            catch (Exception e)
            {
                return false;
            }

        }

        //Method to check whether Template Name text field is accessbile
        public bool IsTemplateNameAccessible()
        {
            if (new TemplateSearchScreen().IsCreateNewTemplateDisplayed())
            {
                Thread.Sleep(200);
                createNewTemplate.Click();
                BaseSeleniumComponents.WaitUntilDisplayed(By.Name("NameOfTemplate"));
                String randomName = TemplateSearchScreen.RandomString(5);
                templateName.SendKeys(randomName);

                bool returnVal = templateName.GetAttribute("value") == randomName;
                //TemplateSearchScreen.closeManageMsgTab(manageMsgCloseTab, manageMsgWarningDontSave);
                return returnVal;
            }
            else
                return false;
        }

        //Method to validate error message thrown when template is saved with blank template name
        public bool IsTemplateNameMandatory()
        {
            BaseSeleniumComponents.WaitUntilDisplayed(By.Name("NameOfTemplate"));
            templateName.Clear();
            Thread.Sleep(500);
            saveTemplate.Click();
            BaseSeleniumComponents.WaitUntilDisplayed(By.CssSelector("[data-automation-id='status_error']"));
            bool returnVal = statusError.Displayed;
            //TemplateSearchScreen.closeManageMsgTab(manageMsgCloseTab, manageMsgWarningDontSave);
            return returnVal;
        }

        //Method to validate selection of template type.
        public bool IsTemplateTypeSelectable(String selectType)
        {
            bool temp1 = documentTypeList.Displayed;

            documentTypeList.Click();

            Actions action = new Actions(WebContext.WebDriver);
            action.MoveToElement(documentTypeList).Click();
            action.SendKeys(selectType);
            action.SendKeys(Keys.Enter);
            action.Build().Perform();

            String str = documentTypeListSelected.GetAttribute("innerHTML");
            return str == selectType;

        }

        public bool IsTemplateTypeMandatory()
        {
            templateName.Clear();
            Thread.Sleep(100);
            templateName.SendKeys(TemplateSearchScreen.RandomString(6));
            Thread.Sleep(100);
            SeleniumHelper.ClickByJS(SeleniumHelper.FindElement(By.CssSelector(SeleniumHelper.AutomationId("well_know_action_save"))));
            Wait.WaitForElementDisplayed(By.CssSelector("[data-automation-id='status_error']"));
            bool returnVal = statusError.Displayed;
            return returnVal;
        }

        public bool IsTemplateMarkedActive()
        {
            bool defaultAssert = activeCheckBox.Selected;
            Thread.Sleep(500);
            activeCheckBox.Click();
            bool modifiedAssert = activeCheckBox.Selected;

            return (defaultAssert && !modifiedAssert);
        }




        //Method to create new Message Template
        public bool CreateMessageTemplate(string tName, string tPurpose, bool activeFlag = true)
        {
            if (new TemplateSearchScreen().IsCreateNewTemplateDisplayed())
            {
                Thread.Sleep(500);
                createNewTemplate.Click();

                //Set template name
                BaseSeleniumComponents.WaitUntilDisplayed(By.Name("NameOfTemplate"));
                templateName.SendKeys(tName);

                //set template type
                Actions action = new Actions(WebContext.WebDriver);
                action.MoveToElement(documentTypeList).Click();
                action.SendKeys(Keys.Enter);
                action.Build().Perform();
                Thread.Sleep(300);
                action.SendKeys(tPurpose);
                action.SendKeys(Keys.Enter);
                action.Build().Perform();


                //set template as inactive.
                if (activeFlag == false && activeCheckBox.Selected)
                {
                    SeleniumHelper.ClickByJS(activeCheckBox);
                }

                //set template description. 
                templateDescription.SendKeys(RandomString(15));

                //set template email subject
                emailSubject.SendKeys(RandomString(8));

                //save the template.
                SeleniumHelper.ClickByJS(saveTemplate);
                Wait.WaitForAjaxReady(By.CssSelector(".locking-mask-loading"));
                SeleniumHelper.Sleep(3);

                try
                {
                    if (statusError.Displayed)
                    {
                        //Add code to cancel the template creation.
                        cancelTemplate.Click();
                        //Add click event for dont save button on dialog box
                        return false;
                    }
                    else
                        return false;
                }
                catch
                {
                    //It throws an NoSuchElement exception when status error message is not displayed. 
                    //Hence record creation successful
                    return true;
                }

            }
            else
                return false;


        }

        //Method to search template already exists by name 
        public string CheckResultsInTemplateSearch(string tName, bool inactiveTemplateFlag = false)
        {
            //BaseSeleniumComponents.WaitUntilDisplayed(By.Name("TemplateNameforSearchCriteria")); can be removed
            templateNameForSearch.Clear();
            templateNameForSearch.SendKeys(tName);

            if (inactiveTemplateFlag)
                SeleniumHelper.ClickByJS(templateInactiveChkBoxForSearch);
            //templateInactiveChkBoxForSearch.Click();

            SeleniumHelper.ClickByJS(searchButton);
            Wait.WaitForAjaxReady(By.CssSelector(".locking-mask-loading"));
            //BaseSeleniumComponents.WaitUntilDisplayed(By.CssSelector(POM.Helper.SeleniumHelper.AutomationId("search_results_counter")));
            if (searchResultCounter.GetAttribute("innerHTML").Contains("No Matches"))
            {
                return "No Records Found";
            }

            else
            {
                if ((WebContext.WebDriver.FindElements(By.CssSelector("[data-automation-id='resultTile']")).Count) / 3 > 0)
                    return "Record Found";
                else
                    return "Error in search operation";
            }
        }

        //Method to generate Random String
        public static string RandomString(int length)
        {
            Thread.Sleep(20);
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        //Method to delete template opened in maintenance mode
        public string DeleteTemplate(string tName, bool includeInvisible = false)
        {
            string str = this.CheckResultsInTemplateSearch(tName, includeInvisible);

            IWebElement resultTile = WebContext.WebDriver.FindElement(By.CssSelector("[data-automation-id='resultTile']"));
            SeleniumHelper.ClickByJS(resultTile);

            BaseSeleniumComponents.WaitUntilDisplayed(By.CssSelector("[data-automation-id='delete_button']"));
            SeleniumHelper.ClickByJS(deleteButton);

            BaseSeleniumComponents.WaitUntilDisplayed(By.CssSelector("[data-automation-id='continue_with_delete_button']"));
            SeleniumHelper.ClickByJS(continueWithDeleteButton);
            Wait.WaitForAjaxReady(By.CssSelector(".locking-mask-loading"));
            //Search the same template to check successful deletion
            return this.CheckResultsInTemplateSearch(tName, includeInvisible);

        }

        public void CloneTemplateFromToolbar(string tName, string tPurpose, bool includeInvisible = true)
        {
            string str = this.CheckResultsInTemplateSearch(tName, includeInvisible);

            IWebElement resultTile = WebContext.WebDriver.FindElement(By.CssSelector("[data-automation-id='resultTile']"));
            //    POM.Helper.SeleniumHelper.ClickByJS(resultTile);

            SeleniumHelper.ClickByJS(resultTile);
            Wait.WaitForAjaxReady(By.CssSelector(".locking-mask-loading"));
            if (new TemplateSearchScreen().IsCloneTemplateDisplayed())
            {
                cloneTemplate.ClickByJS();
                Wait.WaitForAjaxReady(By.CssSelector(".locking-mask-loading"));
                POM.Helper.Wait.WaitForElement(By.CssSelector(SeleniumHelper.AutomationId("ignore_commit_dialog")));
                IWebElement dontSaveButton = WebContext.WebDriver.FindElement(By.CssSelector(SeleniumHelper.AutomationId("ignore_commit_dialog")));
                POM.Helper.SeleniumHelper.ClickByJS(dontSaveButton);
            }
            Wait.WaitForAjaxReady(By.CssSelector(".locking-mask-loading"));
            saveTemplate.Click();
        }

        public void CheckForClonedTemplate(string tName, string tPurpose, bool includeInvisible = true)
        {
            string str = this.CheckResultsInTemplateSearch(tName, includeInvisible);
            Assert.AreEqual(str, "Record Found");
            IWebElement resultTile = WebContext.WebDriver.FindElement(By.CssSelector("[data-automation-id='resultTile']"));
            SeleniumHelper.ClickByJS(resultTile);
            POM.Helper.Wait.WaitForElement(By.CssSelector(SeleniumHelper.AutomationId("ignore_commit_dialog")));
            IWebElement dontSaveButton = WebContext.WebDriver.FindElement(By.CssSelector(SeleniumHelper.AutomationId("ignore_commit_dialog")));
            POM.Helper.SeleniumHelper.ClickByJS(dontSaveButton);

        }


    }


}

