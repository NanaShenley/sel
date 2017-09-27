using System;
using Assessment.Components.Common;
using OpenQA.Selenium;
using SharedComponents.BaseFolder;
using OpenQA.Selenium.Support.PageObjects;
using OpenQA.Selenium.Support.UI;
using SharedComponents.Helpers;
using WebDriverRunner.webdriver;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
using System.Text;
using System.Diagnostics;
using TestSettings;

namespace Assessment.Components.PageObject
{
    public class SchemeSearchPanel: BaseSeleniumComponents
    {
        public SchemeSearchPanel()
        {
            WebContext.WebDriver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(MarksheetConstants.Timeout));
            PageFactory.InitElements(WebContext.WebDriver, this);
        }

        [FindsBy(How = How.CssSelector, Using = "input[name='LearningLevelSelector.dropdownImitator']")]
        private IWebElement LearninglevelSelector;

        [FindsBy(How = How.CssSelector, Using = "input[name='CurriculumPhaseSelector.dropdownImitator']")]
        private IWebElement PhaseSelector;

        [FindsBy(How = How.CssSelector, Using = "input[name='SubjectSelector.dropdownImitator']")]
        private IWebElement SubjectSelector;

        [FindsBy(How = How.CssSelector, Using = "input[name='StrandSelector.dropdownImitator']")]
        private IWebElement StrandSelector;

        [FindsBy(How = How.CssSelector, Using = "button[name='filterViewBy']")]
        public IWebElement FilterColumn;

        [FindsBy(How = How.CssSelector, Using = "button[data-automation-id=AddSubject]")]
        public IWebElement AddSubjectButton;

        [FindsBy(How = How.CssSelector, Using = "button[data-automation-id=AddYear]")]
        public IWebElement AddLevelButton;

        [FindsBy(How = How.CssSelector, Using = "a[data-automation-id=AddFromExistingYear]")]
        public IWebElement AddExistingLevelButton;

        [FindsBy(How = How.CssSelector, Using = "button[data-automation-id=AddStrand]")]
        public IWebElement AddStrandButton;

        [FindsBy(How = How.CssSelector, Using = "a[data-automation-id=AddFromExistingStrand]")]
        public IWebElement AddExistingStrandButton;

        [FindsBy(How = How.CssSelector, Using = "button[data-automation-id=AddStatement]")]
        public IWebElement AddStatementButton;

        [FindsBy(How = How.CssSelector, Using = "a[data-automation-id=AddFromExistingStatement]")]
        public IWebElement AddExistingStatementButton;

        [FindsBy(How = How.CssSelector, Using = "a[data-automation-id=AddFromExistingSuject]")]
        public IWebElement AddExistingSubjectButton;

        [FindsBy(How = How.CssSelector, Using = "[data-apply-scheme-filters='']")]
        public IWebElement Applyfilter;

        [FindsBy(How = How.CssSelector, Using = "div[id='container']")]
        private IWebElement container;

        [FindsBy(How = How.CssSelector, Using = "div[data-automation-id='Schemes-selector']:first-child button[data-automation-id='AddSubject']")]
        private IWebElement addSubjectButton;

        [FindsBy(How = How.CssSelector, Using = "div[data-automation-id='Schemes-selector']:first-child button[data-automation-id='AddSubject']  ~ ul > li > a[data-automation-id='CreateNewSubject']")]
        private IWebElement createNewSubjectButton;

        [FindsBy(How = How.CssSelector, Using = "div[data-automation-id='Schemes-selector']:first-child button[data-automation-id='AddSubject']  ~ ul > li > a[data-automation-id='GenerateTemplate']")]
        private IWebElement generateTemplateButton;

        public static By newNameField = By.CssSelector("input[name='name']");        

        public static By newDescriptionField = By.CssSelector("textarea[name='description']");

        public static By shortNameField = By.CssSelector("input[name='shortName']");

        [FindsBy(How = How.CssSelector, Using = "button[data-automation-id='CreateNew_Ok_Button']")]
        private IWebElement newOKButton;

        [FindsBy(How = How.CssSelector, Using = "div[node-type='Subject'] > button[data-automation-id='AddStrand']")]
        private IWebElement addStrandButton;

        [FindsBy(How = How.CssSelector, Using = "div[node-type='Subject'] > ul > li > a[data-automation-id='CreateNewStrand']")]
        private IWebElement createNewStrandButton;

        [FindsBy(How = How.CssSelector, Using = "div[node-type='Strand'] > button[data-automation-id='AddYear']")]
        private IWebElement addLevelButton;

        [FindsBy(How = How.CssSelector, Using = "div[node-type='Year']")]
        private IWebElement newLevelNode;

        [FindsBy(How = How.CssSelector, Using = "div[node-type='year-node']")]
        private IWebElement existingLevelNode;

        [FindsBy(How = How.CssSelector, Using = "div[node-type='Strand'] > ul > li > a[data-automation-id='AddFromExistingYear']")]
        private IWebElement addExistingLevelButton;

        [FindsBy(How = How.CssSelector, Using = "div[id='Level_description']")]
        public IWebElement RootElement;

        public string checkboxTag = "input[type='checkbox']";

        [FindsBy(How = How.CssSelector, Using = "button[data-automation-id='Add_Button']")]
        private IWebElement addLevelOKButton;

        [FindsBy(How = How.CssSelector, Using = "button[data-automation-id='Cancel_Button']")]
        private IWebElement cancelPickerFormButton;

        [FindsBy(How = How.CssSelector, Using = "button[data-automation-id='AddStatement']")]
        private IWebElement addStatementButton;

        [FindsBy(How = How.CssSelector, Using = "a[data-automation-id='CreateStatement']")]
        private IWebElement createNewStatementButton;

        [FindsBy(How = How.CssSelector, Using = "button[action-type-node='Subject'] >  i[data-automation-id='edit_button']")]
        private IWebElement editSubjectButton;

        [FindsBy(How = How.CssSelector, Using = "button[action-type-node='Strand'] >  i[data-automation-id='edit_button']")]
        private IWebElement editStrandButton;

        [FindsBy(How = How.CssSelector, Using = "button[action-type-node='Statement'] >  i[data-automation-id='edit_button']")]
        private IWebElement editStatementButton;

        [FindsBy(How = How.CssSelector, Using = "button[action-type-node='Subject'] >  i[data-automation-id='delete_button']")]
        private IWebElement deleteSubjectButton;

        [FindsBy(How = How.CssSelector, Using = "button[action-type-node='Strand'] >  i[data-automation-id='delete_button']")]
        private IWebElement deleteStrandButton;

        [FindsBy(How = How.CssSelector, Using = "button[action-type-node='Statement'] >  i[data-automation-id='delete_button']")]
        private IWebElement deleteStatementButton;


        [FindsBy(How = How.CssSelector, Using = "i[data-automation-id='Schemedelete_button']")]
        private IWebElement deleteSchemeButton; 

        [FindsBy(How = How.CssSelector, Using = "button[action-type-node='Year'] >  i[data-automation-id='delete_button']")]
        private IWebElement deleteYearButton;

        [FindsBy(How = How.CssSelector, Using = "div[data-automation-id='delete-popover'] button[data-automation-id='Ok_button']")]
        private IWebElement deleteOKButton;

        [FindsBy(How = How.CssSelector, Using = "a[data-automation-id=modify_existing_scheme]")]
        public IWebElement ModifyExistingSchemeButton;

        [FindsBy(How = How.CssSelector, Using = "a[data-automation-id=new_from_existing_scheme]")]
        public IWebElement NewFromExistingSchemeButton;

        [FindsBy(How = How.CssSelector, Using = "a[data-automation-id=create_new_scheme]")]
        public IWebElement CreateNewSchemeButton;

        [FindsBy(How = How.CssSelector, Using = "a[data-automation-id='Save_Scheme']")]
        private IWebElement SaveButton;
        
        [FindsBy(How = How.CssSelector, Using = "a[data-scheme-cancel-handler]")]
        private IWebElement CancelButton;

        [FindsBy(How = How.CssSelector, Using = "button[data-automation-id='scheme-link-pos-marksheet']")]
        private IWebElement OpenPosTemplateButton;

        [FindsBy(How = How.CssSelector, Using = "button[data-automation-id=SearchSubject]")]
        public IWebElement searchSubject;

        private static By saveSuccessMessage_New = By.CssSelector("div[data-automation-id='status_success']");

        [FindsBy(How = How.CssSelector, Using = "div[data-automation-id='status_success']")]
        private IWebElement SaveSuccessMessage;

        WebDriverWait waiter = new WebDriverWait(WebContext.WebDriver, TimeSpan.FromSeconds(MarksheetConstants.Timeout));

        public static  By filtertext = By.CssSelector("span[class='sims-pill__label']");

        public static By dataexist = By.CssSelector("span[class='panel-title']");

        public static By existResourceData = By.CssSelector("div[class='statement-group-item']");

        List<string> ColumnName = new List<string>();
        int count = 0;
        public SchemeSearchPanel Search()
        {
            waiter.Until(ExpectedConditions.ElementToBeClickable(Applyfilter));
            Applyfilter.Click();
            WaitUntillAjaxRequestCompleted();
            return new SchemeSearchPanel();
        }

        //Opne the Filter to select the criteria
        public SchemeSearchPanel OpenFilter()
        {
            waiter.Until(ExpectedConditions.ElementToBeClickable(FilterColumn));
            FilterColumn.Click();
            return new SchemeSearchPanel();
        }

        public SchemeSearchPanel ClickAddSubjectButton()
        {
            waiter.Until(ExpectedConditions.ElementToBeClickable(AddSubjectButton));
            AddSubjectButton.Click();
            return new SchemeSearchPanel();
        }
        public SchemeSearchPanel ClickAddExistingSubjectButton()
        {
            waiter.Until(ExpectedConditions.ElementToBeClickable(AddExistingSubjectButton));
            AddExistingSubjectButton.Click();
            return new SchemeSearchPanel();
        }
        public SchemeSearchPanel ClickAddLevelButton()
        {
            waiter.Until(ExpectedConditions.ElementToBeClickable(AddLevelButton));
            AddLevelButton.Click();
            return new SchemeSearchPanel();
        }
        public SchemeSearchPanel ClickAddExistingLevelButton()
        {
            waiter.Until(ExpectedConditions.ElementToBeClickable(AddExistingLevelButton));
            AddExistingLevelButton.Click();
            return new SchemeSearchPanel();
        }
        public SchemeSearchPanel ClickAddStrandButton()
        {
            waiter.Until(ExpectedConditions.ElementToBeClickable(AddStrandButton));
            AddStrandButton.Click();
            return new SchemeSearchPanel();
        }
        public SchemeSearchPanel ClickAddExistingStrandButton()
        {
            waiter.Until(ExpectedConditions.ElementToBeClickable(AddExistingStrandButton));
            AddExistingStrandButton.Click();
            return new SchemeSearchPanel();
        }

        public SchemeSearchPanel ClickAddStatementButton()
        {
            waiter.Until(ExpectedConditions.ElementToBeClickable(AddStatementButton));
            AddStatementButton.Click();
            return new SchemeSearchPanel();
        }
        public SchemeSearchPanel ClickAddExistingStatementButton()
        {
            waiter.Until(ExpectedConditions.ElementToBeClickable(AddExistingStatementButton));
            AddExistingStatementButton.Click();
            return new SchemeSearchPanel();
        }
        /// <summary>
        /// Select a Group on Scheme Filter
        /// </summary>
        public SchemeSearchPanel SelectGroup(string learninglevel)
        {
            SeleniumHelper.ChooseSelectorOption(LearninglevelSelector, learninglevel);
            return new SchemeSearchPanel();
        }
        
        /// <summary>
        /// Select a Subject on Scheme Filter
        /// </summary>
        public SchemeSearchPanel SelectSubject(string SubjectName)
        {
            SeleniumHelper.ChooseSelectorOption(SubjectSelector, SubjectName);
            return new SchemeSearchPanel();
        }

        /// <summary>
        /// Select a Strand on Scheme Filter
        /// </summary>
        public SchemeSearchPanel SelectStrand(string StrandName)
        {
            SeleniumHelper.ChooseSelectorOption(StrandSelector, StrandName);
            return new SchemeSearchPanel();
        }

        /// <summary>
        /// Select a Phase on Scheme Filter
        /// </summary>
        public SchemeSearchPanel SelectPhase(string PhaseName)
        {
            SeleniumHelper.ChooseSelectorOption(PhaseSelector, PhaseName);
            return new SchemeSearchPanel();
        }

        /// <summary>
        /// Click on Add Subject Button
        /// </summary>
        public SchemeSearchPanel ClickCreateSubjectButton()
        {
            waiter.Until(ExpectedConditions.ElementToBeClickable(addSubjectButton));
            addSubjectButton.Click();
            return new SchemeSearchPanel();
        }

        /// <summary>
        /// Click on Add Strand Button
        /// </summary>
        public SchemeSearchPanel ClickCreateStrandButton()
        {
            waiter.Until(ExpectedConditions.ElementToBeClickable(addStrandButton));
            addStrandButton.Click();
            return new SchemeSearchPanel();
        }

        /// <summary>
        /// Click on Edit Subject Button
        /// </summary>
        public SchemeSearchPanel ClickEditSubjectButton()
        {
            waiter.Until(ExpectedConditions.ElementToBeClickable(editSubjectButton));
            WaitForAjaxReady(By.CssSelector(".locking-mask"));
            editSubjectButton.Click();
            return new SchemeSearchPanel();
        }

        /// <summary>
        /// Click on Delete Subject Button
        /// </summary>
        public SchemeSearchPanel ClickDeleteSubjectButton()
        {
            waiter.Until(ExpectedConditions.ElementToBeClickable(deleteSubjectButton));
            deleteSubjectButton.Click();
            return new SchemeSearchPanel();
        }
        /// <summary>
        /// Click on Delete Strand Button
        /// </summary>
        public SchemeSearchPanel ClickDeleteStrandButton()
        {
            waiter.Until(ExpectedConditions.ElementToBeClickable(deleteStrandButton));
            deleteStrandButton.Click();
            return new SchemeSearchPanel();
        }

        /// <summary>
        /// Click on Delete Statement Button
        /// </summary>
        public SchemeSearchPanel ClickDeleteStatementButton()
        {
            waiter.Until(ExpectedConditions.ElementToBeClickable(deleteStatementButton));
            WaitForAjaxReady(By.CssSelector(".locking-mask"));
            deleteStatementButton.Click();
            return new SchemeSearchPanel();
        }

        /// Click on Delete Statement Button
        /// </summary>
        public SchemeSearchPanel ClickDeleteSchemeButton()
        {
            waiter.Until(ExpectedConditions.ElementToBeClickable(deleteSchemeButton));
            deleteSchemeButton.Click();
            return new SchemeSearchPanel();
        }

        /// <summary>
        /// Click on Delete Year Button
        /// </summary>
        public SchemeSearchPanel ClickDeleteYearButton()
        {
            waiter.Until(ExpectedConditions.ElementToBeClickable(deleteYearButton));
            deleteYearButton.Click();
            return new SchemeSearchPanel();
        }

        /// <summary>
        /// Click on Delete Year Button
        /// </summary>
        public SchemeSearchPanel ClickSearchSubjectButton()
        {
            waiter.Until(ExpectedConditions.ElementToBeClickable(searchSubject));
            searchSubject.Click();
            return new SchemeSearchPanel();
        }

        /// <summary>
        /// Click on Edit Strand Button
        /// </summary>
        public SchemeSearchPanel ClickEditStrandButton()
        {
            waiter.Until(ExpectedConditions.ElementToBeClickable(editStrandButton));
            WaitForAjaxReady(By.CssSelector(".locking-mask"));
            editStrandButton.Click();
            return new SchemeSearchPanel();
        }

        /// <summary>
        /// Click on Edit Strand Button
        /// </summary>
        public SchemeSearchPanel ClickEditStatementButton()
        {
            waiter.Until(ExpectedConditions.ElementToBeClickable(editStatementButton));
            WaitForAjaxReady(By.CssSelector(".locking-mask"));
            editStatementButton.Click();
            return new SchemeSearchPanel();
        }

        /// <summary>
        /// Click on Add Statement Button
        /// </summary>
        public SchemeSearchPanel ClickCreateStatementButton()
        {            
            waiter.Until(ExpectedConditions.ElementToBeClickable(addStatementButton));
            addStatementButton.Click();
            return new SchemeSearchPanel();
        }

        /// <summary>
        /// Click on Add Existing Level Button
        /// </summary>
        public SchemeSearchPanel ClickAssignExistingLevelButton()
        {
            waiter.Until(ExpectedConditions.ElementToBeClickable(addLevelButton));
            addLevelButton.Click();
            return new SchemeSearchPanel();
        }

        /// <summary>
        /// Click on Level Node
        /// </summary>
        public SchemeSearchPanel ClickLevelNode()
        {
            waiter.Until(ExpectedConditions.ElementToBeClickable(newLevelNode));
            newLevelNode.Click();
            return new SchemeSearchPanel();
        }

        /// <summary>
        /// Click on Level Node
        /// </summary>
        public SchemeSearchPanel ClickExistingLevelNode()
        {
            waiter.Until(ExpectedConditions.ElementToBeClickable(existingLevelNode));
            existingLevelNode.Click();
            return new SchemeSearchPanel();
        }

        /// <summary>
        /// Click on Create new subject link
        /// </summary>
        public SchemeSearchPanel ClickCreateNewSubjectLink()
        {
            waiter.Until(ExpectedConditions.ElementToBeClickable(createNewSubjectButton));
            createNewSubjectButton.Click();
            return new SchemeSearchPanel();
        }

        /// <summary>
        /// Click on Create new subject link
        /// </summary>
        public SchemeSearchPanel ClickGenerateTemplateLink()
        {
            waiter.Until(ExpectedConditions.ElementToBeClickable(generateTemplateButton));
            generateTemplateButton.Click();
            return new SchemeSearchPanel();
        }

        /// <summary>
        /// Click on Create new subject link
        /// </summary>
        public SchemeSearchPanel ClickCreateNewStrandLink()
        {
            waiter.Until(ExpectedConditions.ElementToBeClickable(createNewStrandButton));
            createNewStrandButton.Click();
            return new SchemeSearchPanel();
        }

        /// <summary>
        /// Click on Create new statement link
        /// </summary>
        public SchemeSearchPanel ClickCreateNewStatementLink()
        {
            waiter.Until(ExpectedConditions.ElementToBeClickable(createNewStatementButton));
            createNewStatementButton.Click();
            return new SchemeSearchPanel();
        }

        public void SelectLevel(int checkedboxIndexNo)
        {
            waiter.Until(ExpectedConditions.ElementToBeClickable(RootElement));
            WaitForElement(By.CssSelector(checkboxTag));
            ReadOnlyCollection<IWebElement> treenodElement = RootElement.FindElements((By.CssSelector(checkboxTag)));
            int[] checkedboxIndex = { checkedboxIndexNo };
            foreach (var index in checkedboxIndex)
            {
                var element = treenodElement[index];
                waiter.Until(ExpectedConditions.ElementToBeClickable(element));
                if (element != null)
                {
                    if (!element.Selected)
                    {
                        element.Click();
                    }
                }
                Thread.Sleep(500);
            }

        }

        /// <summary>
        /// Click on add existing levels link
        /// </summary>
        public SchemeSearchPanel ClickAddExistingLevelLink()
        {
            waiter.Until(ExpectedConditions.ElementToBeClickable(addExistingLevelButton));
            addExistingLevelButton.Click();
            return new SchemeSearchPanel();
        }

        /// <summary>
        /// Click on OK Button to add new Subject
        /// </summary>
        public SchemeSearchPanel ClickOKButton()
        {
            waiter.Until(ExpectedConditions.ElementToBeClickable(newOKButton));
            newOKButton.Click();
            return new SchemeSearchPanel();
        }

        /// <summary>
        /// Click on OK Button to add new Subject
        /// </summary>
        public SchemeSearchPanel ClickDeleteOKButton()
        {
            waiter.Until(ExpectedConditions.ElementToBeClickable(deleteOKButton));
            deleteOKButton.Click();
            return new SchemeSearchPanel();
        }

        /// <summary>
        /// Click on OK Button to add new Subject
        /// </summary>
        public SchemeSearchPanel AddExistingLevelOKButton()
        {
            waiter.Until(ExpectedConditions.ElementToBeClickable(addLevelOKButton));
            addLevelOKButton.Click();
            return new SchemeSearchPanel();
        }

        /// <summary>
        /// Click on Cancel Button 
        /// </summary>
        public SchemeSearchPanel CancelPickerFormButton()
        {
            waiter.Until(ExpectedConditions.ElementToBeClickable(cancelPickerFormButton));
            cancelPickerFormButton.Click();
            return new SchemeSearchPanel();
        }

        /// <summary>
        /// It sets the given Aspect name in the Name text field
        /// </summary>
        public void SetNameAndDescription(string Name,string Description,string shortName)
        {
            WaitForAndSetValue(TimeSpan.FromSeconds(MarksheetConstants.Timeout), newNameField, Name, true);
            WaitForAndSetValue(TimeSpan.FromSeconds(MarksheetConstants.Timeout), shortNameField, shortName, true);
            WaitForAndSetValue(TimeSpan.FromSeconds(MarksheetConstants.Timeout), newDescriptionField, Description, true);            
        }

        public static string GenerateRandomString(int length)
        {
            Random _random = new Random((int)DateTime.Now.Ticks);
            string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuwxyz";
            StringBuilder builder = new StringBuilder(length);

            for (int i = 0; i < length; ++i)
                builder.Append(chars[_random.Next(chars.Length)]);

            return builder.ToString();
        }

        public List<string> GetDataInTab()
        {
            ReadOnlyCollection<IWebElement> ColumnNameElementList = WebContext.WebDriver.FindElements(filtertext);
            foreach (IWebElement eachelement in ColumnNameElementList)
            {
                if (count > 0)
                {

                    if (eachelement.Displayed == true && ColumnName.Contains(eachelement.Text) != true)
                    {
                        ColumnName.Add(eachelement.Text);
                    }
                }
                if (eachelement.Displayed == true && ColumnName.Contains(eachelement.Text) != true)
                {
                    ColumnName.Add(eachelement.Text);
                    count++;
                }
            }
            if (ColumnName.Count <= 4)
                GetDataInTab();
            return ColumnName;
        }

        /// <summary>
        /// Verifies if the column is present based on its Column Name
        /// </summary>


        public List<string> GetTreeData()
        {
            ReadOnlyCollection<IWebElement> ColumnNameElementList = WebContext.WebDriver.FindElements(dataexist);
            foreach (IWebElement eachelement in ColumnNameElementList)
            {
                if (count > 0)
                {
                 
                    if (eachelement.Displayed == true && ColumnName.Contains(eachelement.Text) != true)
                    {
                        ColumnName.Add(eachelement.Text);
                        waiter.Until(ExpectedConditions.ElementToBeClickable(eachelement));
                    //    eachelement.Click();
                    }
                }
                if (eachelement.Displayed == true && ColumnName.Contains(eachelement.Text) != true)
                {
                    ColumnName.Add(eachelement.Text);
                    count++;
                }
            }
            if (ColumnName.Count <= 3)
                GetTreeData();
            return ColumnName;
        }

        public List<string> GetExistingData()
        {
            ReadOnlyCollection<IWebElement> SearchResultElementList = WebContext.WebDriver.FindElements(existResourceData);
            List<string> SearchResults = new List<string>();
            foreach (IWebElement eachelement in SearchResultElementList)
            {

                    if (eachelement.Displayed == true && SearchResults.Contains(eachelement.Text) != true)
                    {
                        SearchResults.Add(eachelement.Text.Replace(" ", string.Empty).ToLower());
                    }
                

            }
            return SearchResults;
        }

        public SchemeSearchPanel NavigateToModifyExistingScheme()
        {
            waiter.Until(ExpectedConditions.ElementToBeClickable(ModifyExistingSchemeButton));
            ModifyExistingSchemeButton.Click();
            return new SchemeSearchPanel();
        }

        public SchemeSearchPanel NavigateToNewFromExistingScheme()
        {
            waiter.Until(ExpectedConditions.ElementToBeClickable(NewFromExistingSchemeButton));
            NewFromExistingSchemeButton.Click();
            return new SchemeSearchPanel();
        }

        public SchemeSearchPanel NavigateToCreateNewScheme()
        {
            waiter.Until(ExpectedConditions.ElementToBeClickable(CreateNewSchemeButton));
            CreateNewSchemeButton.Click();
            return new SchemeSearchPanel();
        }

        /// <summary>
        /// clicks the Save button on the data maintenance screen
        /// </summary>
        public void ClickSaveButton()
        {  
            WaitForAjaxReady(By.CssSelector(".locking-mask"));            
            SaveButton.Click();
        }

        /// <summary>
        /// clicks the Cancel toolbar button 
        /// </summary>
        public void ClickCancelButton()
        {
            CancelButton.Click();
        }

        /// <summary>
        /// save the scheme.         
        /// </summary>
        /// <returns></returns>
        public bool SaveMessageAssertionSuccess()
        {
            waiter.Until(ExpectedConditions.ElementIsVisible(saveSuccessMessage_New));
            return waiter.Until(ExpectedConditions.TextToBePresentInElement(SaveSuccessMessage, "Scheme record saved"));

        }

        /// <summary>
        /// generate template from scheme.         
        /// </summary>
        public void OpenPosTemplate()
        {

            waiter.Until(ExpectedConditions.ElementToBeClickable(OpenPosTemplateButton));
        
            OpenPosTemplateButton.Click();
                    
            Thread.Sleep(3000);
                
          
        }


        /// <summary>
        /// generate template from scheme.         
        /// </summary>
        /// <returns></returns>
        public void GenerateTemplateMessageAssertionSuccess()
        {
            waiter.Until(ExpectedConditions.ElementIsVisible(saveSuccessMessage_New));
           // return waiter.Until(ExpectedConditions.TextToBePresentInElement(SaveSuccessMessage, "MarksheetTemplate record saved"));

        }


        public void DeleteMessageAssertionSuccess(string schemename)
        {
            waiter.Until(ExpectedConditions.ElementIsVisible(saveSuccessMessage_New));
           // return waiter.Until(ExpectedConditions.TextToBePresentInElement(SaveSuccessMessage, "Scheme record " + schemename + " deleted"));

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

        public bool WaitForAjaxReady(By ajaxProgressElement)
        {
            Console.WriteLine("Waiting for Ajax request to complete");
            bool ajaxIsComplete = false;
            Stopwatch waiting = new Stopwatch();
            var timeOut = BrowserDefaults.TimeOut;
            waiting.Start();
            while (waiting.Elapsed <= timeOut)
            {
                string openConnections = (WebContext.WebDriver as IJavaScriptExecutor).ExecuteScript("return jQuery.active").ToString();

                ajaxIsComplete = openConnections == "0";
                if (ajaxIsComplete)
                    break;

                Thread.Sleep(10);
            }
            waiting.Stop();

            if (ajaxIsComplete)
                Console.WriteLine("Ajax request completed in approximately {0}ms.", waiting.ElapsedMilliseconds);
            else
                throw new WebDriverTimeoutException(string.Format("5. Waiting for connections to close timed out after {0}ms", timeOut.Milliseconds));

            return true;
        }
    }
}
