//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using Selene.Support.Attributes;
//using SharedComponents.BaseFolder;
//using SharedComponents.CRUD;
//using SharedComponents.Helpers;
//using SharedComponents.HomePages;
//using SharedServices.Components.PageObjects;
//using WebDriverRunner.internals;

//namespace SharedServices.Tests.Documents
//{
//    public class AddAndViewDocumentStaffScreen : BaseSeleniumComponents
//    {
//        /// <summary>
//        /// Test to verify the Add document functionality
//        /// </summary>
//        [WebDriverTest(Groups = new[] {"Story 3478"}, Enabled = false)]
//        public void AddAndViewDocumentInStaffMedicalNotes()
//        {
//            //1.Log on
//            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.PersonnelOfficer);

//            HomePage homePage = new HomePage();
//            QuickLinksBar quickLinks = homePage.MenuBar();
//            quickLinks.StaffRecords();
//            StaffScreen staffScreen = new StaffScreen();
//            staffScreen.ClickSearch();
//            SearchResults.WaitForResults();
//            Assert.IsTrue(SearchResults.HasResults());
//            SearchResults.SelectSearchResult(0);

//            Detail.WaitForDetail();
//            staffScreen = new StaffScreen();
//            staffScreen.ClickMedicalAccordion();
//            staffScreen.ClickDeleteRowButton(0, NoteType.MedicalNote);
//            //Add & Save Medical Note
//            staffScreen.SaveNotes(0, NoteType.MedicalNote);
//            staffScreen = new StaffScreen();
//            ViewDocumentsPageObject viewDocuments = staffScreen.ClickViewDocumentsButton(0, NoteType.MedicalNote);
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
//            //staffScreen = new StaffScreen();
//            staffScreen.ClickMedicalAccordion();
//            ViewDocumentsPageObject viewDocuments1 = staffScreen.ClickViewDocumentsButton(0, NoteType.MedicalNote);
//            viewDocuments1.CheckForGrid();
//        }

//        [WebDriverTest(Groups = new[] {"Story 3478"}, Enabled = false)]
//        public void AddDocumentInStaffGeneralNotes()
//        {
//            //1.Log on
//            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.PersonnelOfficer);

//            HomePage homePage = new HomePage();
//            QuickLinksBar quickLinks = homePage.MenuBar();
//            quickLinks.StaffRecords();
//            StaffScreen staffScreen = new StaffScreen();
//            staffScreen.ClickSearch();
//            SearchResults.WaitForResults();
//            Assert.IsTrue(SearchResults.HasResults());
//            SearchResults.SelectSearchResult(0);
//            staffScreen = new StaffScreen();
//            Detail.WaitForDetail();

//            staffScreen.ClicDocumentAccordion();
//            //staffScreen = staffScreen.ClickDropdownSectionAccordion();
//            //staffScreen.ClickDocumentSectionAccordion();

//            staffScreen.ClickDeleteRowButton(0, NoteType.Document);
//            //Add & Save Medical Note
//            staffScreen.SaveNotes(0, NoteType.Document);
//            //staffScreen.ClickViewDocumentsButton(0, NoteType.Document);

//            ViewDocumentsPageObject viewDocuments = staffScreen.ClickViewDocumentsButton(0, NoteType.Document);
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
//            //staffScreen = new StaffScreen();
//            //staffScreen = staffScreen.ClickDropdownSectionAccordion();
//            //staffScreen.ClickDocumentSectionAccordion();
//            staffScreen.ClicDocumentAccordion();
//            viewDocuments = staffScreen.ClickViewDocumentsButton(0, NoteType.Document);

//            viewDocuments.CheckForGrid();
//        }
//    }
//}
