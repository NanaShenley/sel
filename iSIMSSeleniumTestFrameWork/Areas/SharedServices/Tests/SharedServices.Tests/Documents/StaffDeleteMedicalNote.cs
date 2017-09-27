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
//    public class StaffDeleteMedicalNote : BaseSeleniumComponents
//    {
//        /// <summary>
//        /// Test to verify the Delete Document functionality
//        /// </summary>
//        [WebDriverTest(Groups = new[] { "DeletNotewithattachments" }, Enabled = false)]
//        public void DeleteNotefromMedicalNotes()
//        {
//            //1.Log on
//            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);
           
//            QuickLinksBar quickLinks = new QuickLinksBar();
//            quickLinks.StaffRecords();

//            StaffScreen staffScreen = new StaffScreen();
//            staffScreen.ClickSearch();
//            SearchResults.WaitForResults();
//            SearchResults.SelectSearchResult(1);

//            Detail.WaitForDetail();
//            staffScreen = new StaffScreen();

//            staffScreen.ClickMedicalAccordion();
//            staffScreen.ClickDeleteRowButton(0, NoteType.MedicalNote);

//            //Add  Medical Note & Attachment
//            staffScreen = new StaffScreen();
//            staffScreen.SaveNotes(0, NoteType.MedicalNote);

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

//            staffScreen = new StaffScreen();
//            staffScreen.ClickSearch();
//            SearchResults.WaitForResults();
//            SearchResults.SelectSearchResult(1);
//            Detail.WaitForDetail();
//            staffScreen = new StaffScreen();
//            staffScreen.ClickMedicalAccordion();
//            staffScreen.ClickDeleteRowButton(0, NoteType.MedicalNote);
        
//            Detail.WaitForDetail();
//            staffScreen.SaveRecord();

//            Assert.IsNotNull(staffScreen.GetSuccessMessage());

//            ////verify there are no documents by clicking view documents button
//            staffScreen.ClickMedicalAccordion();
            
//            //viewDocuments1.WaitForGrid();
//            staffScreen.CheckForEmptyGrid();
//        }


//        [WebDriverTest(Groups = new[] { "DeletNotewithattachments" }, Enabled = false)]
//        public void DeleteDocumentFromDocuments()
//        {
//            //1.Log on
//            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);

//            QuickLinksBar quickLinks = new QuickLinksBar();
//            quickLinks.StaffRecords();

//            StaffScreen staffScreen = new StaffScreen();
//            staffScreen.ClickSearch();
//            SearchResults.WaitForResults();
//            SearchResults.SelectSearchResult(1);

//            Detail.WaitForDetail();
//            staffScreen = new StaffScreen();

//            staffScreen.ClicDocumentAccordion();


//            staffScreen.ClickDeleteRowButton(0, NoteType.Document);

//            //Add & Save Medical Note
//            staffScreen.SaveNotes(0, NoteType.Document);
          
//            ViewDocumentsPageObject viewDocuments = staffScreen.ClickViewDocumentsButton(0, NoteType.Document);
//            viewDocuments.ClickAddDocumentButton();

//            UploadFileDialog uploadFile = new UploadFileDialog();
//            uploadFile.WaitForUploadDialog();
//            uploadFile.SelectFile();
//            uploadFile.ClickUploadButton();
//            viewDocuments.ClickOkButton();
//            Detail.WaitForDetail();
//            staffScreen.SaveRecord();

//            staffScreen = new StaffScreen();
//            staffScreen.ClickSearch();
//            SearchResults.WaitForResults();
//            SearchResults.SelectSearchResult(1);
//            Detail.WaitForDetail();
//            staffScreen.ClicDocumentAccordion();

//            staffScreen.ClickDeleteRowButton(0, NoteType.Document);
//            Detail.WaitForDetail();
//            staffScreen.SaveRecord();

//            Assert.IsNotNull(staffScreen.GetSuccessMessage());

//            //verify there are no documents by clicking view documents button
//            staffScreen = new StaffScreen();
//            staffScreen.ClickSearch();
//            SearchResults.WaitForResults();
//            SearchResults.SelectSearchResult(1);

//            Detail.WaitForDetail();
//            staffScreen = new StaffScreen();
//            staffScreen.ClicDocumentAccordion();

//            staffScreen.CheckForEmptyGrid();
//        }
//    }
//}
