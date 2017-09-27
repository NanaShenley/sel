//using System.IO;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using SharedComponents.BaseFolder;
//using SharedComponents.CRUD;
//using SharedComponents.Helpers;
//using SharedServices.Components.PageObjects;
//using SharedServices.Components.Properties;
//using TestSettings;
//using WebDriverRunner.internals;
//using Selene.Support.Attributes;

//namespace SharedServices.Tests.Documents
//{
//    public class AddAndViewDocumentPupilScreen : BaseSeleniumComponents
//    {
//        const string PupilRecordQuickLink = "quicklinks_top_level_pupil_submenu_pupilrecords";

//        [WebDriverTest(Groups = new[] { "Story5081" }, Enabled = false)]
//        public void AddAndViewDocumentInPupilMedicalNotes()
//        {
//            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);
//            WaitForAndClick(TestDefaults.Default.TimeOut, SeleniumHelper.SelectByDataAutomationID(PupilRecordQuickLink));
           
//            PupilScreen pupilScreen = new PupilScreen();
//            pupilScreen.ClickSearch();
//            SearchResults.WaitForResults();
//            Assert.IsTrue(SearchResults.HasResults());
//            SearchResults.SelectSearchResult(5);

//            Detail.WaitForDetail();
//            pupilScreen = new PupilScreen();
//            pupilScreen.ClickMedicalAccordion();
//            //Add & Save Medical Note
//            pupilScreen.SaveNotes(0, NoteType.MedicalNote);
//            pupilScreen.ClickViewDocumentsButton(0, NoteType.MedicalNote);

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

//            //Verify document attached.
//            pupilScreen = new PupilScreen();
//            SearchResults.SelectSearchResult(5);
//            Detail.WaitForDetail();
//            pupilScreen.ClickMedicalAccordion();
//            pupilScreen.ClickDocumentsButton(0, NoteType.MedicalNote);

//            viewDocuments = new ViewDocumentsPageObject();
//            viewDocuments.WaitForGrid();

//            string text = viewDocuments.GetRowText(0);

//            string file = Path.GetFileName(Settings.Default.FilePath);
//            Assert.IsFalse(string.IsNullOrEmpty(file));
//            string fileName = file.ToLower();
//            Assert.AreEqual(fileName, text);
//        }

//        [WebDriverTest(Groups = new[] { "Story5081" }, Enabled = false)]
//        public void AddDocumentInPupilMedicalCondition()
//        {
//            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);
//            SeleniumHelper.FindAndClick(SeleniumHelper.SelectByDataAutomationID(PupilRecordQuickLink));
          
//            PupilScreen pupilScreen = new PupilScreen();
//            pupilScreen.ClickSearch();
//            SearchResults.WaitForResults();
//            Assert.IsTrue(SearchResults.HasResults());
//            SearchResults.SelectSearchResult(0);

//            Detail.WaitForDetail();

//            pupilScreen.ClickDocumentSectionAccordion();
//            //Add & Save Medical Note
//            pupilScreen.SaveNotes(0, NoteType.Document);
//            pupilScreen.ClickDocumentsButton(0, NoteType.Document);

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

//            pupilScreen.ClickDocumentSectionAccordion();
//            pupilScreen.ClickDocumentsButton(0, NoteType.Document);

//            viewDocuments = new ViewDocumentsPageObject();
//            viewDocuments.WaitForGrid();

//            string text = viewDocuments.GetRowText(0);

//            string file = Path.GetFileName(Settings.Default.FilePath);
//            Assert.IsFalse(string.IsNullOrEmpty(file));
//            string fileName = file.ToLower();
//            Assert.AreEqual(fileName, text);
//        }
//    }
//}
