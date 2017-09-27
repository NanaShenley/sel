//using System;
//using System.Globalization;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using SharedComponents.BaseFolder;
//using SharedComponents.CRUD;
//using SharedComponents.Helpers;
//using SharedComponents.HomePages;
//using SharedServices.Components.PageObjects;
//using WebDriverRunner.internals;
//using Selene.Support.Attributes;

//namespace SharedServices.Tests.Documents
//{
//    public class CountofDocuments : BaseSeleniumComponents
//    {
//        /// <summary>
//        /// Test to verify the Document Count Feature
//        /// </summary>
//        [WebDriverTest(Groups = new[] {"CountOfDocuments"}, Enabled = false)]
//        public void CountOfDocuments()
//        {
//            //1.Log on
//            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.PersonnelOfficer);

//            //HomePage homePage = new HomePage();
//            QuickLinksBar quickLinks = new QuickLinksBar();
//            quickLinks.StaffRecords();

//            StaffScreen staffScreen = new StaffScreen();
//            staffScreen.ClickSearch();
//            SearchResults.WaitForResults();
//            SearchResults.SelectSearchResult(2);

//            Detail.WaitForDetail();
//            staffScreen = new StaffScreen();

//            staffScreen.ClickMedicalAccordion();
//            staffScreen.ClickDeleteRowButton(0, NoteType.MedicalNote);

//            //Add  Medical Note & Attachment
//            staffScreen = new StaffScreen();
//            staffScreen.SaveNotes(0, NoteType.MedicalNote);

//            //viewdocustaffScreen.ClickViewDocumentsButton(0, NoteType.MedicalNote);

//            ViewDocumentsPageObject viewDocuments = staffScreen.ClickViewDocumentsButton(0, NoteType.MedicalNote);
//            viewDocuments.ClickAddDocumentButton();

//            UploadFileDialog uploadFile = new UploadFileDialog();
//            uploadFile.WaitForUploadDialog();
//            uploadFile.SelectFile();
//            uploadFile.ClickUploadButton();
//            int ExpCount = viewDocuments.CheckForDocumentCount();
//            viewDocuments.ClickOkButton();
//            Detail.WaitForDetail();
//            staffScreen.SaveRecord();

//            Assert.IsNotNull(staffScreen.GetSuccessMessage());

//            //Verify count document attached.
//            staffScreen = new StaffScreen();
//            staffScreen.ClickSearch();
//            SearchResults.WaitForResults();
//            SearchResults.SelectSearchResult(2);
//            Detail.WaitForDetail();
//            staffScreen = new StaffScreen();
//            staffScreen.ClickMedicalAccordion();

//            staffScreen.MedicalNote(0, NoteType.MedicalNote);
//            ViewDocumentsPageObject viewDocuments1 = new ViewDocumentsPageObject();
//            String count = viewDocuments1.getdocumentcount();

//            Assert.IsTrue(count.Contains(ExpCount.ToString(CultureInfo.InvariantCulture)));
//        }
//    }
//}
