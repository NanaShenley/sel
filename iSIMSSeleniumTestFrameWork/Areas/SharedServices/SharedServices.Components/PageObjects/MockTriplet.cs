using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using SeSugar;
using SeSugar.Automation;
using SharedComponents.BaseFolder;
using SharedComponents.CRUD;
using SharedComponents.Helpers;
using SharedServices.Components.Common;
using SharedServices.Components.Properties;
using WebDriverRunner.webdriver;
using SimsBy = SeSugar.Automation.SimsBy;

#pragma warning disable 649

namespace SharedServices.Components.PageObjects
{
    public class MockTriplet : BaseSeleniumComponents
    {
        public By ComponentIdentifier
        {
            get { return SimsBy.CssSelector("[data-section-id='detail']"); }
        }

        [FindsBy(How = How.CssSelector, Using = "input[name='LegalSurname']")]
        private IWebElement _surNameElement;

        public MockTriplet()
        {
            AutomationSugar.WaitFor(SimsBy.AutomationId("search_criteria_submit"));
            PageFactory.InitElements(WebContext.WebDriver, this);
        }

        public MockTriplet SearchFor(string term)
        {
            _surNameElement.SetText(term);

            System.Threading.Thread.Sleep(10000); // Why don't results appear without a reasonable wait?
            _surNameElement.SendKeys(Keys.Enter);
            AutomationSugar.WaitForAjaxCompletion();

            if (!SearchResults.HasResults()) return this;
            SearchResults.SelectSearchResult(0);
            AutomationSugar.WaitFor(SimsBy.AutomationId("well_know_action_save"));
            return this;
        }

        public GridComponent<DocumentRow> DocumentsGrid
        {
            get
            {
                return new GridComponent<DocumentRow>(By.CssSelector("[data-maintenance-container='LearnerNotes']"));
            }
        }

        public DocumentsButton DocumentsButton {
            get { return new DocumentsButton();}
        }

        public DocumentRow AddDocumentNote()
        {
            AutomationSugar.ExpandAccordionPanel("section_menu_Learner Notes");

            AutomationSugarHelpers.WaitForAndClickOn(SimsBy.AutomationId("add_document_button"));
            AutomationSugar.WaitForAjaxCompletion();

            var documentsGrid = DocumentsGrid;
            var row = documentsGrid.GetLastRow();

            row.Summary = Utilities.GenerateRandomString(10, "Note ");
            return row;
        }

        public bool Save()
        {
            AutomationSugar.WaitFor("well_know_action_save");
            AutomationSugar.ClickOn("well_know_action_save");
            WaitUntillAjaxRequestCompleted();
            return AutomationSugar.SuccessMessagePresent(ComponentIdentifier);
        }

        public void DeleteDocument()
        {
            AutomationSugar.ExpandAccordionPanel("section_menu_Documents");
            GridComponent<DocumentRow> documentsGrid = DocumentsGrid;
            
            DocumentRow row = documentsGrid.GetLastRow();
            row.OpenDocuments();

            ViewDocumentsPageObject viewDocuments = new ViewDocumentsPageObject();

            viewDocuments.ClickDeleteCheckBox();
            viewDocuments.ClickDeleteDocumentButton();
            viewDocuments.ClickYesButton();
            viewDocuments.ClickOkButton();
        }

        public void Close()
        {
            AutomationSugar.ClickOn(By.CssSelector("button[data-close-current-page]"));
        }
    }

    public class DocumentRow : GridRow
    {
        [FindsBy(How = How.CssSelector, Using = "[name$='.Summary']")] 
        private IWebElement _summary;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='documents_(-)_button']")]
        private IWebElement _documentButton;

        public string Summary
        {
            set { _summary.SetText(value); }
            get { return _summary.GetValue(); }
        }

        public ViewDocumentsPageObject OpenDocuments()
        {
            _documentButton.ClickByJs();
            AutomationSugar.WaitForAjaxCompletion();

            return new ViewDocumentsPageObject();
        }

        public void Remove()
        {
            ClickDelete();
            var confirmButton = SeleniumHelper.Get(By.CssSelector("[data-automation-id='Yes_button']"));
            SeSugar.Automation.Retry.Do(confirmButton.Click);
        }
    }

    public class DocumentsButton
    {
        private IWebElement _documentButton;
        
        public ViewDocumentsPageObject OpenDocuments()
        {
            _documentButton.ClickByJs();
            AutomationSugar.WaitForAjaxCompletion();

            return new ViewDocumentsPageObject();
        }

        public DocumentsButton()
        {
            _documentButton = WebContext.WebDriver.FindElement(
                By.CssSelector("div[data-fieldprefix] > button[data-automation-id='documents_(-)_button']"));
        }

       
    }
}
