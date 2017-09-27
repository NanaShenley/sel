//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using Selene.Support.Attributes;
//using SharedComponents.BaseFolder;
//using SharedComponents.CRUD;
//using SharedComponents.Helpers;
//using SharedComponents.HomePages;
//using SharedServices.Components.PageObjects;
//using TestSettings;
//using WebDriverRunner.internals;

//namespace SharedServices.Tests.Documents
//{
//    public class AddNoteAndDocumentInOneTx : BaseSeleniumComponents
//    {
//        [WebDriverTest(Groups = new[] { "Story3490" }, Browsers = new[] { BrowserDefaults.Ie, BrowserDefaults.Chrome }, Enabled = false)]
//        public void AddStaffMedicalNotesInSingleTransaction()
//        {
//            //1.Log on
//            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.PersonnelOfficer);

//            HomePage homePage = new HomePage();
//            QuickLinksBar quickLinks = homePage.MenuBar();
//            quickLinks.StaffRecords();

//            StaffScreen staffScreen = new StaffScreen();
//            staffScreen.ClickCurrentStatus();
//            staffScreen.ClickSearch();
//            SearchResults.WaitForResults();
//            SearchResults.SelectSearchResult(0);

//            Detail.WaitForDetail();

//            staffScreen.ClickMedicalAccordion();
//            //Add  Medical Note & Attachment
//            staffScreen.SaveNotes(0, NoteType.MedicalNote);
//            staffScreen.ClickViewDocumentsButton(0, NoteType.MedicalNote);

//            ViewDocumentsPageObject viewDocuments = new ViewDocumentsPageObject();
//            viewDocuments.ClickAddDocumentButton();

//            UploadFileDialog uploadFile = new UploadFileDialog();
//            uploadFile.WaitForUploadDialog();
//            uploadFile.SelectFile();
//            uploadFile.ClickUploadButton();
//            viewDocuments.ClickOkButton();
//            Detail.WaitForDetail();
//            staffScreen.SaveRecord();

//            Assert.IsNotNull(staffScreen.GetSuccessMessage());

//            //Verify document attached.
//            staffScreen = new StaffScreen();
//            SearchResults.SelectSearchResult(0);
//            Detail.WaitForDetail();
//            staffScreen.ClickMedicalAccordion();
//            staffScreen.ClickViewDocumentsButton(0, NoteType.MedicalNote);

//            ViewDocumentsPageObject viewDocuments1 = new ViewDocumentsPageObject();
//            viewDocuments1.CheckForGrid();
//        }
//    }
//}
