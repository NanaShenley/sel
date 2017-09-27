//using System.IO;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using SharedComponents.CRUD;
//using SharedComponents.Helpers;
//using SharedComponents.HomePages;
//using SharedServices.Components.PageObjects;
//using SharedServices.Components.Properties;
//using WebDriverRunner.internals;
//using Selene.Support.Attributes;

//namespace SharedServices.Tests.Documents
//{
//    public class ViewDocumentSENScreen
//    {
//        /// <summary>
//        /// Add and View documents for Pupil SEN
//        /// </summary>
//        [WebDriverTest(Groups = new[] { "ViewSENDocument" }, Enabled = false)]
//        public void AddAndViewDocumentWorkingInSENScreen()
//        {
//            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);
            
//            SENScreen senScreen = new SENScreen();
//            SeleniumHelper.NavigateMenu("Tasks", "Pupils", "Pupil Records");

//            PupilScreen pupilScreen = new PupilScreen();
//            pupilScreen.ClickSearch();
//            SearchResults.WaitForResults();

//           // Assert.IsTrue(SearchResults.HasResults());
//            SearchResults.SelectSearchResult(3);

//            Detail.WaitForDetail();

//            //click SEN tab
//            senScreen.ClickHiddenTabs();
//            senScreen.ClickSenAccordion();
//            senScreen.ClickDeleteRowButton(0, NoteType.SenNeed);

//            //Add first row and save the record            
//            senScreen.AddSelectorColumn(0, 2, NoteType.SenNeed, "Cognitive and Learning", SENScreen.NeedTypeSelector);
//            senScreen.AddDateColumn(0, 5, NoteType.SenNeed, "2/9/2015");
            
//            //Add and upload document
//            senScreen.ClickDocumentsButton(0, NoteType.SenNeed);

//            ViewDocumentsPageObject viewDocuments = new ViewDocumentsPageObject();
//            viewDocuments.WaitForGrid();
//            viewDocuments.ClickAddDocumentButton();

//            UploadFileDialog uploadFile = new UploadFileDialog();
//            uploadFile.WaitForUploadDialog();
//            uploadFile.SelectFile();
//            uploadFile.ClickUploadButton();

//            viewDocuments = new ViewDocumentsPageObject();
//            viewDocuments.ClickOkButton();

//            Detail.WaitForDetail();
//            pupilScreen.SaveRecord();

//            Assert.IsNotNull(pupilScreen.GetSuccessMessage());
//            Assert.IsTrue(SearchResults.HasResults());
            
//            //Open SEN Record screen
//            ShellAction.OpenTaskMenu();
            
//            TaskMenuActions.ClickMenuItem("task_menu_section_pupil_sen_records");

//            senScreen = new SENScreen();
//            senScreen.SetCheckboxForCriteria(SENScreen.NoSenStageSelect);

//            senScreen.ClickSearch();
//            SearchResults.WaitForResults();

//            Assert.IsTrue(SearchResults.HasResults());
//            SearchResults.SelectSearchResult(3);

//            Detail.WaitForDetail();

//            //View documents from SEN Record
//            senScreen.ClickDocumentsButton(0, NoteType.SenNeed);
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
