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
    public class TopicDataMaintainance
    {

        public TopicDataMaintainance()
        {
            WebContext.WebDriver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(MarksheetConstants.Timeout));
            PageFactory.InitElements(WebContext.WebDriver, this);
        }

        WebDriverWait waiter = new WebDriverWait(WebContext.WebDriver, TimeSpan.FromSeconds(MarksheetConstants.Timeout));

        //[FindsBy(How = How.CssSelector, Using = "a[data-automation-id='Button_Dropdown']")]
        //private IWebElement CreateButton;

        [FindsBy(How = How.CssSelector, Using = "a[data-automation-id='create_topic_button']")]
        public IWebElement CreateButton;

        [FindsBy(How = How.CssSelector, Using = "a[data-automation-id='Node-Statement-Name']")]
        public IWebElement Statementnode;


        private static By StatementDesc = By.CssSelector("textarea[data-automation-id='Node-Statement-Desc']");

        [FindsBy(How = How.CssSelector, Using = "a[data-automation-id='save_button']")]
        public IWebElement TopicSaveButton;

        public static By StatmentAllocator = By.CssSelector("div[data-automation-id = 'allocator-section-topics-Allocator']");
        private static By saveSuccessMessage_New = By.CssSelector("div[data-automation-id='status_success']");

        [FindsBy(How = How.CssSelector, Using = "div[data-automation-id='status_success']")]
        private IWebElement SaveSuccessMessage;
       
        [FindsBy(How = How.CssSelector, Using = "div[id='statementcontainer']")]
        private IWebElement AllocatedStatement;
        

        [FindsBy(How = How.CssSelector, Using = "a[data-automation-id='generate_template_button']")]
        private IWebElement GenerateTemplateButton;

        [FindsBy(How = How.CssSelector, Using = "input[name='Name']")]
        private IWebElement Name;

        [FindsBy(How = How.CssSelector, Using = "textarea[name='Description']")]
        private IWebElement Description;

        [FindsBy(How = How.CssSelector, Using = "input[name='Schemes.dropdownImitator']")]
        public IWebElement SchemesDropdownInitiator;

        [FindsBy(How = How.CssSelector, Using = "input[name='Year.dropdownImitator']")]
        public IWebElement TopicYearDropdownInitiator;

        [FindsBy(How = How.CssSelector, Using = "input[name='NCYear.dropdownImitator']")]
        public IWebElement TopicSearchYearDropdownInitiator;

        [FindsBy(How = How.CssSelector, Using = "input[name='Term.dropdownImitator']")]
        public IWebElement TopicTermDropdownInitiator;

        [FindsBy(How = How.CssSelector, Using = "a[data-automation-id='delete_button']")]
        private IWebElement DeleteButton;

        [FindsBy(How = How.CssSelector, Using = "button[data-automation-id='delete_ok_button']")]
        private readonly IWebElement ContinueButton = null;

        [FindsBy(How = How.ClassName, Using = "btn-picker")]
        private readonly IWebElement ColourPickerButton = null;

        private static By TermResultList = By.CssSelector("div.select2-result-label");

        private static By YearResultList = By.CssSelector("div.select2-result-label");

        private static By ColourPickerList = By.ClassName("stylepicker-swatch-wrapper");
        
        [FindsBy(How = How.CssSelector, Using = "div[data-section-id='SchemeSection']")]
        public IWebElement SchemeTreeElement;

        [FindsBy(How = How.CssSelector, Using = "[view_id='filterTree']")]
        public IWebElement TreeRootElement;

        public string checkboxTag = "input";

        [FindsBy(How = How.CssSelector, Using = "button[data-automation-id='setup_new_topic']")]
        public IWebElement SetupNewTopicButton;

        [FindsBy(How = How.CssSelector, Using = "div[data-setup-topic-dialog] button[data-automation-id='ok_button']")]
        public IWebElement DialogOKButton;

        public string columnNameSelector = "[webix_tm_id='{0}']";

       
        [FindsBy(How = How.CssSelector, Using = "button[data-automation-id='allocate-button']")]
        public IWebElement AllocateButton;

        [FindsBy(How = How.CssSelector, Using = "button[data-automation-id='button-Generate']")]
        public IWebElement TopicDropdown;
       

    
        [FindsBy(How = How.CssSelector, Using = "a[data-automation-id='button-DeleteTopic']")]
        public IWebElement DeleteTopicButton;
     

        [FindsBy(How = How.CssSelector, Using = "a[data-automation-id='button-OpenTopic']")]
        public IWebElement OpenTopicButton; 

        [FindsBy(How = How.CssSelector, Using = "a[data-automation-id='button-GenerateTemplate']")]
        public IWebElement GenerateTemplate;

        [FindsBy(How = How.CssSelector, Using = "a[data-automation-id='button-PrintTopic']")]
        public IWebElement PrintTopicButton; 

        [FindsBy(How = How.CssSelector, Using = "button[data-automation-id='delete_ok_button']")]
        public IWebElement DeleteTopicOkButton;
               
       
               
        #region newscreen
        public static By dataexist = By.CssSelector("span[class='panel-title']");
        List<string> ColumnName = new List<string>();
        int headercount = 0;

        #endregion
        /// <summary>
        /// Identifies the filter text box on the Tree control.
        /// </summary>
        [FindsBy(How = How.CssSelector, Using = "input[name='tree-filter']")]
        public IWebElement FilterTextBox;

        [FindsBy(How = How.CssSelector, Using = "div.statement-group")]
        private IWebElement StatementResults;

        private string StatementItemTag = "div.statement-group-item";

        public static By TopicData = By.CssSelector("div[class='allocator-tile-container']");

        public static By StatementData = By.ClassName("selectable-item-h2");


        [FindsBy(How = How.CssSelector, Using = "input[name='selectUnallocatedStatements']")]
        public IWebElement selectUnallocatedStatements;


        [FindsBy(How = How.CssSelector, Using = "input[name='selectAllStatements']")]
        public IWebElement selectAllStatements;
        /// <summary>
        /// clicks the Create button on the gradeset data maintenance screen
        /// </summary>
        public TopicDataMaintainance ClickCreateButton()
        {
            CreateButton.Click();
            return new TopicDataMaintainance();
        }
        public TopicDataMaintainance ClickSetupNewTopicButton()
        {
            SetupNewTopicButton.Click();
            return new TopicDataMaintainance();
        }
        public string GenerateRandomString(int length)
        {
            Random _random = new Random((int)DateTime.Now.Ticks);
            string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuwxyz";
            StringBuilder builder = new StringBuilder(length);

            for (int i = 0; i < length; ++i)
                builder.Append(chars[_random.Next(chars.Length)]);

            return builder.ToString();
        }

        public TopicDataMaintainance ClickSaveButton()
        {
            TopicSaveButton.Click();
            return new TopicDataMaintainance();
        }

        public TopicDataMaintainance ClickDialogOkButton()
        {
            waiter.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("input[name='Name']")));
            waiter.Until(ExpectedConditions.ElementToBeClickable(DialogOKButton));
            DialogOKButton.Click();
            while (true)
            {
                if (DialogOKButton.GetAttribute("disabled") != "true")
                    break;
            }
            waiter.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("input[name='Schemes.dropdownImitator']")));
            return new TopicDataMaintainance();
        }
        public List<string> GetTopicNamesFromPicker()
        {
            ReadOnlyCollection<IWebElement> PickerElementList = WebContext.WebDriver.FindElements(TopicData);
            List<string> topicNames = new List<string>();
            foreach (IWebElement eachelement in PickerElementList)
            {

                if (eachelement.Displayed == true && topicNames.Contains(eachelement.Text) != true)
                {
                    topicNames.Add(eachelement.Text.Replace(" ", string.Empty).ToLower());
                }


            }
            return topicNames;
        }



        public string StatementDescripiton()
        {            
            waiter.Until(ExpectedConditions.ElementIsVisible(StatementDesc));
            IWebElement PickerElement = WebContext.WebDriver.FindElement(StatementDesc);
            return PickerElement.Text;
        }




        public bool DeleteTopicSuccess()
        {
            waiter.Until(ExpectedConditions.ElementIsVisible(saveSuccessMessage_New));
            return waiter.Until(ExpectedConditions.TextToBePresentInElement(SaveSuccessMessage, "Topic record deleted"));
        }

        public TopicDataMaintainance ClickDeleteDialogOkButton()
        {            
            waiter.Until(ExpectedConditions.ElementToBeClickable(DeleteTopicOkButton));
            DeleteTopicOkButton.Click();                      
            return new TopicDataMaintainance();
        }

        public bool StatementExist(string selectedStatement)
        {            
            waiter.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("input[name='Name']")));            
            ReadOnlyCollection<IWebElement> SelectedStatementList = WebContext.WebDriver.FindElements(StatementData);                                    
            
            List<string> topicNames = new List<string>();
            foreach (IWebElement eachelement in SelectedStatementList)
            {
                string Statement = eachelement.Text.Replace(" ", string.Empty).ToLower();
                if (Statement == selectedStatement.Replace(" ", string.Empty).ToLower())
                    return true;

            }
            return false;

        }

        public string GetSeletedStatement(int v)
        {
            throw new NotImplementedException();
        }

        public TopicDataMaintainance OpenTopicButtonClick()
        {
            waiter.Until(ExpectedConditions.ElementToBeClickable(OpenTopicButton));
            OpenTopicButton.Click();
            return new TopicDataMaintainance();
        }

        public TopicDataMaintainance PrintTopicButtonClick()
        {
            waiter.Until(ExpectedConditions.ElementToBeClickable(OpenTopicButton));
            PrintTopicButton.Click();
            return new TopicDataMaintainance();
        }


        public bool GenerateTemplateButtonClick()
        {

            // Check if Generate Template option Present
            bool GenerateButtonExist = GenerateTempalateExist("a[data-automation-id = 'button-GenerateTemplate']");

            //If yes Check its is Enabled
            if(GenerateButtonExist)
            {
                ReadOnlyCollection<IWebElement> PickerElementList = WebContext.WebDriver.FindElements(By.CssSelector("a[data-automation-id = 'button-GenerateTemplate']"));
                if (PickerElementList[0].Enabled)
                {
                    PickerElementList[0].Click();
                    waiter.Until(ExpectedConditions.ElementIsVisible(saveSuccessMessage_New));
                    return waiter.Until(ExpectedConditions.TextToBePresentInElement(SaveSuccessMessage, "MarksheetTemplate record saved"));
                }
            }
            return true;
        }

        public void OpenPosTemplate(string TopicName)
        {
            ReadOnlyCollection<IWebElement> AllocatorList = WebContext.WebDriver.FindElements(StatmentAllocator);

            foreach (IWebElement Element in AllocatorList)
            {
                if (Element.Text.Trim().Contains(TopicName))
                {                    
                    Element.FindChild(By.CssSelector("button[data-automation-id='topic-link-pos-marksheet']")).Click(); ;
                    Thread.Sleep(3000);
                    return;
                }
            }
            return;
        }

        public bool GenerateTemplateSelecteTopicClick(string TopicName)
        {
            ReadOnlyCollection<IWebElement> AllocatorList = WebContext.WebDriver.FindElements(StatmentAllocator);
            foreach (IWebElement Element in AllocatorList)
            {
                if (Element.Text.Trim().Contains(TopicName))
                {
                    bool GenerateButtonExist = ChildExists(Element,"a[data-automation-id = 'button-GenerateTemplate']");
                    if (GenerateButtonExist)
                    {
                        IWebElement Child = Element.FindChild(By.CssSelector("a[data-automation-id = 'button-GenerateTemplate']"));
                        Child.Click();
                        Thread.Sleep(3000);
                         return true;
                    }
                }
            }

            return true;                                                     
        }

        private bool ChildExists(IWebElement element, string ChildSelector)
        {
            try
            {
                IWebElement E = element.FindChild(By.CssSelector(ChildSelector));
                return true;
            }
            catch
            {
                return false;
            }
        }

        private bool GenerateTempalateExist(string GeneratButonSelector)
        {
            try
            {
                ReadOnlyCollection<IWebElement> PickerElementList = WebContext.WebDriver.FindElements(By.CssSelector(GeneratButonSelector));
                if (PickerElementList.Count > 0)
                    return true;
                else return false;
            }
            catch (Exception)
            {
                return false;

            }
        }

        public TopicDataMaintainance SelectTopicDropDown()
        {
            waiter.Until(ExpectedConditions.ElementToBeClickable(TopicDropdown));
            TopicDropdown.Click();
            return new TopicDataMaintainance();
        }


        public TopicDataMaintainance SelectTopicDropDownByName(string TopicName)
        {
            waiter.Until(ExpectedConditions.ElementToBeClickable(TopicDropdown));
            ReadOnlyCollection<IWebElement> AllocatorList = WebContext.WebDriver.FindElements(StatmentAllocator);
            foreach (IWebElement Element in AllocatorList)
            {
                if (Element.Text.Trim().Contains(TopicName))
                {
                    Element.FindChild(By.CssSelector("button[data-automation-id='button-Generate']")).Click(); ;
                    Thread.Sleep(3000);
                    return new  TopicDataMaintainance();
                }
            }
            return new TopicDataMaintainance();
        }


        public TopicDataMaintainance  ClickDeleteButton()
        {
            waiter.Until(ExpectedConditions.ElementToBeClickable(DeleteTopicButton));
            Thread.Sleep(3000);
            DeleteTopicButton.Click();
            return new  TopicDataMaintainance();
        }

        public bool ClickDeleteButton(string TopicName)
        {
            ReadOnlyCollection<IWebElement> AllocatorList = WebContext.WebDriver.FindElements(StatmentAllocator);
            foreach (IWebElement Element in AllocatorList)
            {
                if (Element.Text.Trim().Contains(TopicName))
                {
                    bool delete = ChildExists(Element, "a[data-automation-id='button-DeleteTopic");
                    if (delete)
                    {
                        IWebElement Child = Element.FindChild(By.CssSelector("a[data-automation-id='button-DeleteTopic"));
                        Child.Click();
                        Thread.Sleep(3000);
                        return true;
                    }
                }
            }

            return true;
        }

        public void ClickStatement()
        {

            Statementnode.Click();
            
        }

        public TopicDataMaintainance ClickGenerateTemplateButton()
        {
            waiter.Until(ExpectedConditions.ElementToBeClickable(GenerateTemplateButton));
            GenerateTemplateButton.Click();
            return new TopicDataMaintainance();
        }


        public bool SaveTopicSuccess()
        {
            waiter.Until(ExpectedConditions.ElementIsVisible(saveSuccessMessage_New));
            return waiter.Until(ExpectedConditions.TextToBePresentInElement(SaveSuccessMessage, "Topic record saved"));
        }


        public bool GenerateTemplateSuccess()
        {
            waiter.Until(ExpectedConditions.ElementIsVisible(saveSuccessMessage_New));
            return waiter.Until(ExpectedConditions.TextToBePresentInElement(SaveSuccessMessage, "MarksheetTemplate record saved"));
        }

        public void AllocateStatment()
        {
            waiter.Until(ExpectedConditions.ElementToBeClickable(AllocateButton));
            AllocateButton.Click();
            //Wait till the time button is clickable again
            waiter.Until(ExpectedConditions.ElementToBeClickable(AllocateButton));
            Thread.Sleep(3000);
        }


        public void AllocateStatmentByName(string topicName)
        {
            
            ReadOnlyCollection<IWebElement> AllocatorList = WebContext.WebDriver.FindElements(StatmentAllocator);

            foreach (IWebElement Element in AllocatorList)
            {
                if (Element.Text.Trim().Contains(topicName.Trim()))
                {                    
                    Element.FindChild(By.CssSelector("button[data-automation-id='allocate-button']")).Click(); ;
                    Thread.Sleep(3000);
                    return;
                }
            }
        }           
        public void SelectYearOption(string TopicYear)
        {
            TopicYearDropdownInitiator.Click();
            ReadOnlyCollection<IWebElement> List = WebContext.WebDriver.FindElements(YearResultList);
            foreach (IWebElement eachelement in List)
            {
                if (eachelement.Text == TopicYear)
                {
                    eachelement.Click();
                    break;
                }
            }
        }


        /// <summary>
        /// Clicks on the Delete 
        /// </summary>
        public void DeleteButtonClick()
        {
            DeleteButton.Click();
            waiter.Until(ExpectedConditions.ElementToBeClickable(ContinueButton));      
        }

        public void AssignColour()
        {
            ColourPickerButton.Click();
            //get the colour picker by class name.
            ReadOnlyCollection<IWebElement> List = WebContext.WebDriver.FindElements(ColourPickerList);
            int count = 0;
            foreach (IWebElement eachelement in List)
            {
                //skip the first one - the default is no colour.
                if (count != 0)
                {
                    eachelement.Click();
                    break;
                }
                count++;
            }
        }

        /// <summary>
        /// Clicks on the Delete 
        /// </summary>
        public void ContinueButtonClick()
        {
            ContinueButton.Click();

        }
        public void SetTopicDescription(string topicdescription)
        {
            Description.Clear();
            Description.SendKeys(topicdescription);
        }

        public bool MatchTermOption(string TopicTerm)
        {
            bool foundMatchingTerm = false;
            TopicTermDropdownInitiator.Click();
            ReadOnlyCollection<IWebElement> List = WebContext.WebDriver.FindElements(TermResultList);
            foreach (IWebElement eachelement in List)
            {
                if (eachelement.Text == TopicTerm)
                {
                    foundMatchingTerm = true;
                    break;
                }
            }
            return foundMatchingTerm;
        }

        public void SelectTermOption(string TopicTerm)
        {
            TopicTermDropdownInitiator.Click();
            ReadOnlyCollection<IWebElement> List = WebContext.WebDriver.FindElements(TermResultList);
            foreach (IWebElement eachelement in List)
            {
                if (eachelement.Text == TopicTerm)
                {
                    eachelement.Click();
                    break;
                }
            }
        }
        public void SetTopicName(string topicName)
        {
            waiter.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("input[name='Name']")));
            Name.Clear();
            Name.SendKeys(topicName);
        }

        public void SetFilter(string key)
        {
            FilterTextBox.Clear();
            FilterTextBox.SendKeys(key);
        }

        public bool MatchOnStatementDescription(string key)
        {
            bool statementlinkedToFilter = true;
            ReadOnlyCollection<IWebElement> selectedStatements = StatementResults.FindElements((By.CssSelector(StatementItemTag)));
            foreach (var item in selectedStatements)
            {
                if (!item.Text.Contains(key))
                {
                    statementlinkedToFilter = false;
                    break;
                }
            }
            return statementlinkedToFilter;
        }

        public void ClearStatement()
        {
            int count = 0;
            waiter.Until(ExpectedConditions.ElementToBeClickable(TreeRootElement));
            ReadOnlyCollection<IWebElement> treenodElement = TreeRootElement.FindElements((By.TagName(checkboxTag)));
            foreach (IWebElement e in treenodElement)
            {
                if (e.Selected && count == 4)
                {

                    e.Click();
                    break;
                }
                count++;
            }
            Thread.Sleep(1000);
        }

        public void SelectStatement()
        {
            int count = 0;
            waiter.Until(ExpectedConditions.ElementToBeClickable(TreeRootElement));
            ReadOnlyCollection<IWebElement> treenodElement = TreeRootElement.FindElements((By.TagName(checkboxTag)));
            foreach (IWebElement e in treenodElement)
            {
                if (!e.Selected && count == 4)
                {

                    e.Click();
                    break;

                }

                count++;
            }
            Thread.Sleep(1000);
        }

        public String SelectStatement(int Select)
        {
            int count = 0;
            String selectedStatement = "";
            waiter.Until(ExpectedConditions.ElementToBeClickable(SchemeTreeElement));
            ReadOnlyCollection<IWebElement> treenodElement = SchemeTreeElement.FindElements((By.TagName(checkboxTag)));
            foreach (IWebElement e in treenodElement)
            {
                if (!e.Selected && count == Select)
                {
                    e.Click();
                    selectedStatement = e.Text;
                    break;

                }

                count++;
            }
            Thread.Sleep(1000);
            return selectedStatement;
        }

        public String GetTopicTitle()
        {
            String TopicText = WebContext.WebDriver.FindElement(By.CssSelector("span[data-automation-id = 'topic_header_title']")).Text;
            return TopicText;
        }

        /// <summary>
        /// Verifies if the column is present based on its Column Name
        /// </summary>


        public List<string> GetFilteredSchemeData()
        {
            ReadOnlyCollection<IWebElement> ColumnNameElementList = WebContext.WebDriver.FindElements(dataexist);
            foreach (IWebElement eachelement in ColumnNameElementList)
            {
                if (headercount > 0)
                {

                    if (eachelement.Displayed == true && ColumnName.Contains(eachelement.Text) != true)
                    {
                        ColumnName.Add(eachelement.Text);
                        waiter.Until(ExpectedConditions.ElementToBeClickable(eachelement));
                     //   eachelement.Click();
                    }
                }
                if (eachelement.Displayed == true && ColumnName.Contains(eachelement.Text) != true)
                {
                    ColumnName.Add(eachelement.Text);
                    headercount++;
                }
            }
            if (ColumnName.Count <= 5)
                GetFilteredSchemeData();
            return ColumnName;
        }

        public TopicDataMaintainance ClickUnallocated()
        {
            waiter.Until(ExpectedConditions.ElementToBeClickable(selectAllStatements));
            selectAllStatements.Click();

            return new TopicDataMaintainance();
        }
        public TopicDataMaintainance ClickAllStatement()
        {
            waiter.Until(ExpectedConditions.ElementToBeClickable(selectAllStatements));
            selectAllStatements.Click();
            return new TopicDataMaintainance();
        }

        public void ClickAllocatedStatements(int statementcount,string selectedStatement)
        {
            int count = 0;
           List<string> statements = new List<string>();
           ReadOnlyCollection<IWebElement> SelectedStatementList = WebContext.WebDriver.FindElements(dataexist);
            foreach (IWebElement eachelement in SelectedStatementList)
            {
                if (count <= statementcount)
                {

                    if (eachelement.Displayed == true && statements.Contains(eachelement.Text) != true)
                    {
                        statements.Add(eachelement.Text);
                        count++;
                    }
                }
                else
                    break;

            }

            ClickUnallocated();
            if (statements.Contains(selectedStatement))
                Assert.IsTrue(true);
            else
                Assert.IsTrue(false);
            
            ClickAllStatement();
            Thread.Sleep(1000);
        }



    }
}
