using System;
using System.Threading;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using OpenQA.Selenium.Support.UI;
using SharedComponents.BaseFolder;
using SharedComponents.Helpers;
using SharedServices.Components.Common;
using WebDriverRunner.webdriver;

namespace SharedServices.Components.PageObjects
{
    public class StaffScreen : BaseSeleniumComponents
    {
        private const string MedicalSectionAccordion = "section_menu_Medical";
        private const string PersonalDetailSectionAccordion = "section_menu_Personal Details";
        private const string SuccessMessageBanner = "status_success";
        private const string SaveButton = "well_know_action_save";
        private const string DeleteButton = "remove_button";
        public static readonly int Timeout = 30;
        private const string DropdownSectionAccordions = "section_menu_more";
        private const string DocumentsSectionAccordion = "section_menu_hidden_Documents";
        private const string DocumentsAccordion = "section_menu_Documents";
        private const string NotesColumnCSS = "input[name *= '.Summary']";
        private const string CurrentStatus = "input[name='tri_chkbox_StatusCurrentCriterion']";
        private const string SenNeedGrid = "table[data-maintenance-container='LearnerSENNeedTypes']";
        private const string SearchCriteria = "search_criteria_submit";
        private const string StaffTitle = "[name=\"Title.dropdownImitator\"]";
        private const string note = "[data-maintenance-container='StaffMedicalNotes']";
        private const string document = "[data-maintenance-container='StaffNotes']";
        private const string SenNeed = "[data-maintenance-container='LearnerSENNeedTypes']";
        private const string documentslink = "documents_(-)_button";
        public const string Staff = "staff_record_header_title";

        [FindsBy(How = How.CssSelector, Using = StaffTitle)]
        public IWebElement staffTitle;

        [FindsBy(How = How.Name, Using = "LegalSurname")]
        private IWebElement _legalSurname;

        //[FindsBy(How = How.CssSelector, Using = SharedServicesElements.StaffRecord.MedicalNotesGrid)]
        //private IWebElement medicalNotesGrid;

        //[FindsBy(How = How.CssSelector, Using = SharedServicesElements.StaffRecord.DocumentNotesGrid)]
        //private IWebElement documentNotesGrid;
        private IWebElement medicalNotesGrid
        {

            get
            {

                return SeleniumHelper.Get(By.CssSelector(note));
            }
        }

        private IWebElement documentNotesGrid
        {

            get
            {

                return SeleniumHelper.Get(By.CssSelector(document));
            }
        }

        private IWebElement senNeedGrids
        {

            get
            {

                return SeleniumHelper.Get(By.CssSelector(SenNeed));
            }
        }

        //[FindsBy(How = How.CssSelector, Using = SenNeedGrid)]
        //private IWebElement senNeedGrids;

        private IWebElement medicalAccordion
        {
            get
            {
                WaitUntilEnabled(SeleniumHelper.SelectByDataAutomationID(MedicalSectionAccordion));
                return SeleniumHelper.Get(SeleniumHelper.SelectByDataAutomationID(MedicalSectionAccordion));
            }
        }

        private IWebElement NewdocumentAccordion
        {
            get
            {
                WaitUntilEnabled(SeleniumHelper.SelectByDataAutomationID(DocumentsAccordion));
                return SeleniumHelper.Get(SeleniumHelper.SelectByDataAutomationID(DocumentsAccordion));
            }
        }

        private IWebElement personalDetailAccordion
        {
            get
            {
                WaitUntilEnabled(SeleniumHelper.SelectByDataAutomationID(PersonalDetailSectionAccordion));
                return SeleniumHelper.Get(SeleniumHelper.SelectByDataAutomationID(PersonalDetailSectionAccordion));
            }
        }

        private IWebElement CurrentStatusCheckBox
        {
            get { return SeleniumHelper.Get(By.CssSelector(CurrentStatus)); }
        }

        private IWebElement dropdownSectionAccordion
        {
            get
            {
                WaitUntilDisplayed(SeleniumHelper.SelectByDataAutomationID(DropdownSectionAccordions));
                return SeleniumHelper.Get(SeleniumHelper.SelectByDataAutomationID(DropdownSectionAccordions));
            }
        }

        private IWebElement documentsAccordion
        {
            get
            {
                WaitUntilDisplayed(SeleniumHelper.SelectByDataAutomationID(DocumentsSectionAccordion));
                return SeleniumHelper.Get(SeleniumHelper.SelectByDataAutomationID(DocumentsSectionAccordion));
            }
        }

        private IWebElement _searchButton
        {
            get
            {
                WaitUntilDisplayed(SeleniumHelper.SelectByDataAutomationID(SearchCriteria));
                return SeleniumHelper.Get(SeleniumHelper.SelectByDataAutomationID(SearchCriteria));
            }
        }

        private IWebElement _saveButtonElement
        {
            get
            {
                WaitUntilEnabled(SeleniumHelper.SelectByDataAutomationID(SaveButton));
                return SeleniumHelper.Get(SeleniumHelper.SelectByDataAutomationID(SaveButton));
            }
        }

        private IWebElement successMessageBanner
        {
            get
            {
                WaitUntilEnabled(SeleniumHelper.SelectByDataAutomationID(SuccessMessageBanner));
                return SeleniumHelper.Get(SeleniumHelper.SelectByDataAutomationID(SuccessMessageBanner));
            }
        }

        public StaffScreen()
        {
            //Thread.Sleep(1000);
            BaseSeleniumComponents.WaitUntilDisplayed(By.CssSelector(SeleniumHelper.AutomationId(Staff)));
            PageFactory.InitElements(WebContext.WebDriver, this);
        }

        public void SaveNotes(int rowNumber, NoteType type)
        {
            WebDriverWait waiter = new WebDriverWait(WebContext.WebDriver, TimeSpan.FromSeconds(Timeout));
            IWebElement gridRow = GetGridRow(rowNumber, type);
            if (gridRow != null)
            {
                IWebElement addmedicalnotesummary = gridRow.FindElement(By.CssSelector(NotesColumnCSS));
                Assert.IsNotNull(addmedicalnotesummary);
                addmedicalnotesummary.Clear();
                //If the element has been replaced with an identical one, a useful strategy is to look up the element again
                gridRow = GetGridRow(rowNumber, type);
                addmedicalnotesummary = gridRow.FindElement(By.CssSelector(NotesColumnCSS));
                waiter.Until(ExpectedConditions.ElementToBeClickable(addmedicalnotesummary));
                addmedicalnotesummary.SendKeys("Notes");
                addmedicalnotesummary.SendKeys(Keys.Tab);
            }
        }

        public void MedicalNote(int rowNumber, NoteType type)
        {
            WebDriverWait waiter = new WebDriverWait(WebContext.WebDriver, TimeSpan.FromSeconds(Timeout));
            IWebElement gridRow = GetGridRow(rowNumber, type);
            if (gridRow != null)
            {
                IWebElement addmedicalnotesummary = gridRow.FindElement(By.CssSelector(NotesColumnCSS));
                Assert.IsNotNull(addmedicalnotesummary);
                
            }
        }


        public void ClickSearch()
        {
            _searchButton.Click();
        }

        public void ClickCurrentStatus()
        {
            CurrentStatusCheckBox.Click();
        }

        public void ClickDocumentsButton(int rowNumber, NoteType type)
        {
            IWebElement gridRow = GetGridRow(rowNumber, type);
            if (gridRow != null)
            {
                IWebElement documentsButton = gridRow.FindElement(SeleniumHelper.SelectByDataAutomationID(documentslink));
                Assert.IsNotNull(documentsButton);
                documentsButton.Click();
            }
        }

        public void ClickDeleteRowButton(int rowNumber, NoteType type)
        {
            IWebElement gridRow = GetGridRow(rowNumber, type);
            if (gridRow != null)
            {
                IWebElement DeleteRowButton = gridRow.FindElement(SeleniumHelper.SelectByDataAutomationID("remove_button"));
               
                if (DeleteRowButton.Enabled==true)
                {
                    Thread.Sleep(1000);
                    DeleteRowButton.Click();
                    ClickYesButton();
                }
            }
        }

        public void ClickYesButton()
        {
            DeleteConfirmationDialog deletedialog = new DeleteConfirmationDialog();
            deletedialog.ClickYesButton();
        }

        public ViewDocumentsPageObject ClickViewDocumentsButton(int rowNumber, NoteType type)
        {
            IWebElement gridRow = GetGridRow(rowNumber, type);

            if (gridRow != null)
            {
                IWebElement viewDocumentButton = gridRow.FindElement(SeleniumHelper.SelectByDataAutomationID(documentslink));
                Assert.IsNotNull(viewDocumentButton);
                viewDocumentButton.Click();
            }

            return new ViewDocumentsPageObject();
        }


        public void GetDocumentsCount(int rowNumber, NoteType type)
        {
            IWebElement gridRow = GetGridRow(rowNumber, type);

            if (gridRow != null)
            {
                IWebElement viewDocumentButton = gridRow.FindElement(SeleniumHelper.SelectByDataAutomationID(documentslink));
                Assert.IsNotNull(viewDocumentButton);
                viewDocumentButton.Click();
            }
        }

        /// <summary>
        /// Click on medical section accordion
        /// </summary>
        public void ClickMedicalAccordion()
        {
            Thread.Sleep(2000);
            medicalAccordion.Click();
        }

        public void ClickPersonalDetailsAccordion()
        {
            personalDetailAccordion.Click();
        }

        public StaffScreen ClickDropdownSectionAccordion()
        {
           Thread.Sleep(4000);
            WaitUntilDisplayed(SeleniumHelper.SelectByDataAutomationID(DropdownSectionAccordions));
            dropdownSectionAccordion.Click();
            return new StaffScreen();
        }

        public void ClicDocumentAccordion()
        {
            Thread.Sleep(2000);
            WaitUntilDisplayed(SeleniumHelper.SelectByDataAutomationID(DocumentsAccordion));
            NewdocumentAccordion.Click();
            
        }
        public IWebElement GetGridRow(int row, NoteType type)
        {
            IWebElement gridRow;
            switch (type)
            {
                case NoteType.MedicalNote:
                    gridRow = medicalNotesGrid.GetGridRow(row);
                    break;
                case NoteType.SenNeed:
                    gridRow = senNeedGrids.GetGridRow(row);
                    break;
                default:
                    gridRow = documentNotesGrid.GetGridRow(row);
                    break;
            }

            return gridRow;
        }

        public void SaveRecord()
        {
            WaitUntilEnabled(SeleniumHelper.SelectByDataAutomationID(SaveButton));
            _saveButtonElement.Click();
        }

        /// <summary>
        /// Gets the success message banner
        /// </summary>
        /// <returns></returns>
        public IWebElement GetSuccessMessage()
        {
            return successMessageBanner;
        }

        public void ClickDocumentSectionAccordion()
        {
             WaitUntilDisplayed(SeleniumHelper.SelectByDataAutomationID(DocumentsSectionAccordion));
            documentsAccordion.Click();
        }

        public static void ClickDeleteRowButton()
        {
            throw new NotImplementedException();
        }

        public void CheckForEmptyGrid()
        {
            Assert.IsNotNull(medicalNotesGrid);
        }

        public void ClickTitleDropdown()
        {
            ClickSelectorDropdown(staffTitle);
        }

        private void ClickSelectorDropdown(IWebElement element)
        {
            element.WaitUntilState(ElementState.Displayed);
            Retry.Do(element.Click);
            const string selectorSearchBox = "[id*=s2id_autogen][id$=_search]";
            var selectorSearchElement = ElementRetriever.GetOnceLoaded(By.CssSelector(selectorSearchBox));
            selectorSearchElement.WaitUntilState(ElementState.Displayed);
            selectorSearchElement.Click();
        }

        public void SetLegalSurname(string legalSurname)
        {
            _legalSurname.SendKeys(legalSurname);
        }
    }

    public enum NoteType
    {
        MedicalNote,
        Document,
        Attachment,
        SenNeed
    }
}
