//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using Selene.Support.Attributes;
//using SharedComponents.BaseFolder;
//using SharedComponents.CRUD;
//using SharedComponents.Helpers;
//using SharedServices.Components.PageObjects;
//using WebDriverRunner.internals;

//namespace SharedServices.Tests.Documents
//{
//    public class DeleteDocumentPupilScreen : BaseSeleniumComponents
//    {
//        [WebDriverTest(Groups = new[] { "Story 6994" }, Enabled = false)]
//        public void DeletePupilDocumentFromMedicalNotes()
//        {
//            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);
//            SeleniumHelper.NavigateMenu("Tasks", "Pupils", "Pupil Records");
           
//            PupilScreen pupilScreen = new PupilScreen();
//            pupilScreen.ClickSearch();
//            SearchResults.WaitForResults();
//            Assert.IsTrue(SearchResults.HasResults());
//            SearchResults.SelectSearchResult(0);

//            Detail.WaitForDetail();
          
//            pupilScreen.ClickMedicalAccordion();
//            pupilScreen.ClickDeleteRowButton(0, NoteType.MedicalNote);

//            //Add & Save Medical Note
//            pupilScreen.SaveNotes(0, NoteType.MedicalNote);
//            pupilScreen = new PupilScreen();
//            pupilScreen.ClickDocumentsButton(0, NoteType.MedicalNote);

//            ViewDocumentsPageObject viewDocuments = new ViewDocumentsPageObject();
//            viewDocuments.ClickAddDocumentButton();

//            UploadFileDialog uploadFile = new UploadFileDialog();
//            uploadFile.WaitForUploadDialog();
//            uploadFile.SelectFile();
//            uploadFile.ClickUploadButton();
//            viewDocuments.ClickOkButton();
//            Detail.WaitForDetail();
//            pupilScreen.SaveRecord();

//            Assert.IsNotNull(pupilScreen.WaitAndGetSaveNotification());
            
//            pupilScreen = new PupilScreen();
//            SearchResults.SelectSearchResult(0);
//            Detail.WaitForDetail();
//            pupilScreen.ClickMedicalAccordion();
//            pupilScreen.ClickDocumentsButton(0, NoteType.MedicalNote);
//            //reinitialize
//            viewDocuments = new ViewDocumentsPageObject();
//            viewDocuments.WaitForGrid();
//            viewDocuments.ClickDeleteCheckBox();
//            viewDocuments.ClickDeleteDocumentButton();
//            viewDocuments.ClickYesButton();
//            viewDocuments.ClickOkButton();
//            Detail.WaitForDetail();

//            pupilScreen.SaveRecord();
//            Assert.IsNotNull(pupilScreen.WaitAndGetSaveNotification());
//            //verify there are no documents by clicking view documents button
//            pupilScreen.ClickDocumentsButton(0, NoteType.MedicalNote);

//            ViewDocumentsPageObject viewDocuments1 = new ViewDocumentsPageObject();
//            viewDocuments1.WaitForGrid();
//            viewDocuments1.CheckForEmptyGrid();
//        }
//    }
//}
