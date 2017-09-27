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
//    public class DeleteDocumentStaffScreen : BaseSeleniumComponents
//    {
//        [WebDriverTest(Groups = new[] { "Story 6995" }, Enabled = false)]
//        public void DeleteStaffDocumentFromMedicalNotes()
//        {
//            //1.Log on
//            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.PersonnelOfficer);

//            QuickLinksBar quickLinks = new QuickLinksBar();
//            quickLinks.StaffRecords();

//            StaffScreen staffScreen = new StaffScreen();
//            staffScreen.ClickSearch();
//            SearchResults.WaitForResults();
//            SearchResults.SelectSearchResult(1);

//            Detail.WaitForDetail();
//           // staffScreen = new StaffScreen();

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

//            Detail.WaitForDetail();
//            //staffScreen = new StaffScreen();
            
//            staffScreen.ClickMedicalAccordion();
//            viewDocuments = staffScreen.ClickViewDocumentsButton(0, NoteType.MedicalNote);
           
//            //viewDocuments.WaitForGrid();
//            viewDocuments.ClickDeleteCheckBox();
//            viewDocuments.ClickDeleteDocumentButton();
//            viewDocuments.ClickYesButton();
//            viewDocuments.ClickOkButton();
//            Detail.WaitForDetail();

//            staffScreen.SaveRecord();
//            Assert.IsNotNull(staffScreen.GetSuccessMessage());
//            //verify there are no documents by clicking view documents button
//            staffScreen.ClickSearch();

//            SearchResults.WaitForResults();
//            SearchResults.SelectSearchResult(1);

//            Detail.WaitForDetail();
//            // staffScreen = new StaffScreen();

//            staffScreen.ClickMedicalAccordion();
//            viewDocuments = staffScreen.ClickViewDocumentsButton(0, NoteType.MedicalNote);
//            viewDocuments.WaitForGrid();
//            viewDocuments.CheckForEmptyGrid();
//        }

//        //Below case is not working & need to investigate. - Amit R.
//        [WebDriverTest(Groups = new[] { "Story 6995" }, Enabled = false)]
//        public void DeleteStaffDocumentFromGeneralDocuments()
//        {
//            //1.Log on
//            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.PersonnelOfficer);

//            QuickLinksBar quickLinks = new QuickLinksBar();
//            quickLinks.StaffRecords();


//            StaffScreen staffScreen = new StaffScreen();
//            staffScreen.ClickSearch();
//            SearchResults.WaitForResults();
//            SearchResults.SelectSearchResult(1);

//            Detail.WaitForDetail();
//            staffScreen = new StaffScreen();

//            staffScreen.ClickDropdownSectionAccordion();
//            staffScreen.ClickDocumentSectionAccordion();

//            staffScreen.ClickDeleteRowButton(0, NoteType.Document);

//            //Add & Save Medical Note
//            staffScreen = new StaffScreen();
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

//            Assert.IsNotNull(staffScreen.GetSuccessMessage());

//            staffScreen = new StaffScreen();
//            staffScreen.ClickSearch();
//            SearchResults.WaitForResults();
//            SearchResults.SelectSearchResult(1);
//            Detail.WaitForDetail();
//            staffScreen = new StaffScreen();
//            staffScreen.ClickDropdownSectionAccordion();
//            staffScreen.ClickDocumentSectionAccordion();
//            viewDocuments = staffScreen.ClickViewDocumentsButton(0, NoteType.Document);

//            //reinitialize
//            viewDocuments.WaitForGrid();
//            viewDocuments.ClickDeleteCheckBox();
//            viewDocuments.ClickDeleteDocumentButton();
//            viewDocuments.ClickYesButton();
//            viewDocuments.ClickOkButton();
//            Detail.WaitForDetail();

//            staffScreen.SaveRecord();
//            Assert.IsNotNull(staffScreen.GetSuccessMessage());

//            //Verify document attached.
//            staffScreen = new StaffScreen();
//            staffScreen.ClickSearch();
//            SearchResults.WaitForResults();
//            SearchResults.SelectSearchResult(1);
//            Detail.WaitForDetail();
//            staffScreen = staffScreen.ClickDropdownSectionAccordion();
//            staffScreen.ClickDocumentSectionAccordion();
//            //verify there are no documents by clicking view documents button
//            viewDocuments = staffScreen.ClickViewDocumentsButton(0, NoteType.Document);

//           // ViewDocumentsPageObject viewDocuments1 = new ViewDocumentsPageObject();
//            viewDocuments.WaitForGrid();
//            viewDocuments.CheckForEmptyGrid();
//        }
//    }
//}
