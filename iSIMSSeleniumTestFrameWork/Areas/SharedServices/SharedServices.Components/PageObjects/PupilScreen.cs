using System.Threading;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using OpenQA.Selenium.Support.UI;
using SharedComponents.BaseFolder;
using SharedComponents.Helpers;
using SharedServices.Components.Common;
using System;
using WebDriverRunner.webdriver;
#pragma warning disable 649

namespace SharedServices.Components.PageObjects
{
    public class PupilScreen : BaseSeleniumComponents
    {
        #region Variables
        private const string EthnicSectionAccordion = "section_menu_Ethnic/Cultural";
        private const string DocumentSectionAccordion = "section_menu_Documents";
        private const string DropdownSectionAccordions = "section_menu_more";
        private const string SaveButton = "well_know_action_save";
        private const string DocumentsSectionAccordion = "section_menu_hidden_Documents";
        private const string SearchButton = "[data-automation-id='search_criteria_submit']";
        private const string SuccessNotification = "status_success";
        public static readonly int Timeout = 30;
        private const string NotesColumnCSS = "input[name *= '.Summary']";
        private const string note = "[data-maintenance-container='LearnerMedicalNotes']";
        private const string documentslink = "documents_(-)_button";
        public const string Pupil = "add_new_pupil_button"; 
        #endregion

        [FindsBy(How = How.CssSelector, Using = SearchButton)]
        private IWebElement _searchButtonElement;

        [FindsBy(How = How.CssSelector, Using = SharedServicesElements.PupilRecord.DocumentNotesGrid)]
        private IWebElement _documentNotesGrid;

        [FindsBy(How = How.Name, Using = "LegalSurname")]
        private IWebElement _legalSurname;

        public PupilScreen()
        {
            WebContext.WebDriver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(1000));
            PageFactory.InitElements(WebContext.WebDriver, this);
        }

        private IWebElement medicalNotesGrid
        {
            get { return SeleniumHelper.Get(By.CssSelector(note)); }
        }

        
        private IWebElement dropdownSectionAccordion
        {
            get
            {
                WaitUntilEnabled(SimsBy.AutomationId(DropdownSectionAccordions));
                return SeleniumHelper.Get(SimsBy.AutomationId(DropdownSectionAccordions));
            }
        }

        private IWebElement documentsAccordion
        {
            get
            {
                WaitUntilEnabled(SimsBy.AutomationId(DocumentsSectionAccordion));
                return SeleniumHelper.Get(SimsBy.AutomationId(DocumentsSectionAccordion));
            }
        }

        public void ClickDocumentSectionAccordion()
        {
            IWebElement element = SeleniumHelper.Get(DocumentSectionAccordion);
            if (element != null && element.Displayed)
            {
                documentAccordion.Click();
            }
            else
            {
                WaitUntilDisplayed(SimsBy.AutomationId(DropdownSectionAccordions));
                dropdownSectionAccordion.Click();

                WaitUntilDisplayed(SimsBy.AutomationId(DocumentsSectionAccordion));
                documentsAccordion.Click();
            }
        }
        
        public void ClickMedicalAccordion()
        {
            medicalAccordion.Click();
        }

        public void ClickDocuemntAccordion()
        {
            WaitForElement(By.CssSelector(SeleniumHelper.AutomationId(DocumentSectionAccordion)));
            documentAccordion.Click();
        }

        private IWebElement medicalAccordion
        {
            get
            {
                Thread.Sleep(2000);
                WaitUntilEnabled(SimsBy.AutomationId("section_menu_Medical"));
                return SeleniumHelper.Get(SimsBy.AutomationId("section_menu_Medical"));
            }
        }

        private IWebElement ethnicCulturalAccordion
        {
            get
            {
                WaitUntilDisplayed(SimsBy.AutomationId(EthnicSectionAccordion));
                return SeleniumHelper.Get(SimsBy.AutomationId(EthnicSectionAccordion));
            }
        }

        private IWebElement documentAccordion
        {
            get
            {
                WaitUntilEnabled(SimsBy.AutomationId(DocumentSectionAccordion));
                return SeleniumHelper.Get(SimsBy.AutomationId(DocumentSectionAccordion));
            }
        }

        private IWebElement _saveButtonElement
        {
            get
            {
                WaitUntilEnabled(SimsBy.AutomationId(SaveButton));
                return SeleniumHelper.Get(SimsBy.AutomationId(SaveButton));
            }
        }

        public void SaveRecord()
        {
            WaitUntilEnabled(SimsBy.AutomationId(SaveButton));
            _saveButtonElement.Click();
        }

        public void SaveNotes(int rowNumber, NoteType type)
        {
            String mednote = "notes";
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
                addmedicalnotesummary.SendKeys(mednote);
                addmedicalnotesummary.SendKeys(Keys.Tab);
            }
        }

        public void ClickSearch()
        {
            _searchButtonElement.Click();
        }

        public string WaitAndGetSaveNotification()
        {
            return WaitForAndGet(By.CssSelector(SeleniumHelper.AutomationId(SuccessNotification))).Text;
        }

        public string GetDocumentText()
        {
            WebDriverWait waiter = new WebDriverWait(WebContext.WebDriver, TimeSpan.FromSeconds(120));
            waiter.Until(ExpectedConditions.ElementIsVisible(SharedServicesElements.StaffRecord.MedicalViewDocGrid));
            return WebContext.WebDriver.FindElement(SharedServicesElements.StaffRecord.MedicalViewDocGrid).Text;
        }

        public void AttachmentDialog()
        {
            WaitForElement(By.CssSelector("[data-section-id='dialog-palette-editor']"));
            WaitForAndClick(TimeSpan.FromMinutes(60), By.CssSelector(SeleniumHelper.AutomationId("add_document_button")));
        }

        public void ViewAttachmentDialog()
        {
            WebDriverWait waiter = new WebDriverWait(WebContext.WebDriver, TimeSpan.FromSeconds(120));
            waiter.Until(ExpectedConditions.TextToBePresentInElementLocated(By.CssSelector("[data-section-id='dialog-palette-editor']"), "Documents"));
        }

        public void ClickDocumentsButton(int rowNumber, NoteType type)
        {
            IWebElement gridRow = GetGridRow(rowNumber, type);
            if (gridRow != null)
            {
                IWebElement documentsButton = gridRow.FindElement(SimsBy.AutomationId(documentslink));
                Assert.IsNotNull(documentsButton);
                WaitUntilEnabled(SimsBy.AutomationId(documentslink));
                documentsButton.Click();
            }
        }

        public void ClickViewDocumentsButton(int rowNumber, NoteType type)
        {
            IWebElement gridRow = GetGridRow(rowNumber, type);
            if (gridRow != null)
            {
                IWebElement viewDocumentButton = gridRow.FindElement(SimsBy.AutomationId(documentslink));
                Assert.IsNotNull(viewDocumentButton);
                viewDocumentButton.Click();
            }
        }

        public IWebElement GetGridRow(int row, NoteType type)
        {
            Thread.Sleep(3000);
            IWebElement gridRow;
            switch (type)
            {
                case NoteType.MedicalNote:
                    gridRow = medicalNotesGrid.GetGridRow(row);
                    break;

                   
                default:
                    gridRow = _documentNotesGrid.GetGridRow(row);
                    break;
            }
            return gridRow;
        }

        public void ClickEthnicAccordion()
        {
            ethnicCulturalAccordion.Click();
        }

        public void ClickEthnicityDropdown()
        {
            ClickSelectorDropdown("Ethnicity.dropdownImitator");
        }

        private void ClickSelectorDropdown(string elementSelector)
        {
            IWebElement element = SeleniumHelper.Get(By.Name(elementSelector));
            element.WaitUntilState(ElementState.Displayed);
            Retry.Do(element.Click);
            Thread.Sleep(1000);
        }

        public IWebElement GetSuccessMessage()
        {
            return successMessageBanner;
        }

        private IWebElement successMessageBanner
        {
            get
            {
                WaitUntilEnabled(SimsBy.AutomationId(SuccessNotification));
                return SeleniumHelper.Get(SimsBy.AutomationId(SuccessNotification));
            }
        }

        public void ClickYesButton()
        {
            DeleteConfirmationDialog deletedialog = new DeleteConfirmationDialog();
            deletedialog.ClickYesButton();
        }

        public void ClickDeleteRowButton(int rowNumber, NoteType type)
        {
            IWebElement gridRow = GetGridRow(rowNumber, type);
            if (gridRow != null)
            {
                IWebElement DeleteRowButton = gridRow.FindElement(SimsBy.AutomationId("remove_button"));

                if (DeleteRowButton.Enabled == true)
                {
                    Thread.Sleep(1000);
                    DeleteRowButton.Click();
                    ClickYesButton();
                }
            }
        }

        public void SetLegalSurname(string legalSurname)
        {
            _legalSurname.SendKeys(legalSurname);
        }
    }
}
