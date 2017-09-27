//using System;
//using System.Collections.Generic;
//using System.Threading;
//using NUnit.Framework;
//using OpenQA.Selenium;
//using Pupil.BulkUpdate.Tests.ApplicationStatusComponents;
//using Pupil.BulkUpdate.Tests.ParentalSalutationAndAddresseeComponents;
//using Pupil.Components;
//using Pupil.Components.BulkUpdate.ApplicantParentalSalutationAndAddressee;
//using Pupil.Components.Common;
//using SharedComponents.Helpers;
//using WebDriverRunner.webdriver;

//namespace Pupil.BulkUpdate.Tests
//{
//    public class DisabledTests
//    {

//        #region General

//        //[WebDriverTest(Enabled = true, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { ApplicantBulkUpdate.ApplicantBulkUpdateSalutationAddresseeSearchTests, PupilTestGroups.Priority.Priority3 })]
//        public void Search_Controls_Should_Be_Visible()
//        {
//            WebContext.WebDriver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(10));
//            var bulkUpdateNavigation = new PupilBulkUpdateNavigation();
//            bulkUpdateNavigation.NavigateToBulkUpdateApplicantSalutationAndAddressee();
//            var searchCriteriaScreen = new ApplicantParentalSalutationAndAddresseeSearch();

//            // Year Group Text
//            var dropDownValue = searchCriteriaScreen.GetYearGroupValue();
//            Assert.IsTrue(dropDownValue == "");

//            // Intake Group Text
//            Thread.Sleep(1000);
//            dropDownValue = searchCriteriaScreen.GetIntakeGroupValue();
//            Assert.IsTrue(dropDownValue == "");

//            // Admission Group Text
//            Thread.Sleep(1000);
//            dropDownValue = searchCriteriaScreen.GetAdmissionGroupValue();
//            Assert.IsTrue(dropDownValue == "");
//            WebContext.WebDriver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(0));
//        }

//        //[WebDriverTest(Enabled = true, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { ApplicantBulkUpdate.ApplicantBulkUpdateSalutationAddresseeSearchTests, PupilTestGroups.Priority.Priority3 })]
//        public void Search_Controls_Should_Be_Hierarchical()
//        {
//            WebContext.WebDriver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(10));
//            var bulkUpdateNavigation = new PupilBulkUpdateNavigation();
//            bulkUpdateNavigation.NavigateToBulkUpdateApplicantSalutationAndAddressee();
//            var searchCriteriaScreen = new ApplicantParentalSalutationAndAddresseeSearch();

//            // Year Group
//            Thread.Sleep(1000);
//            var yearGroupReply = searchCriteriaScreen.FindYearGroupIselectorDropDown();

//            // Intake Group
//            Thread.Sleep(1000);
//            var intakeGroupReply = searchCriteriaScreen.FindIntakeGroupIselectorDropDown();

//            // Admission Group
//            Thread.Sleep(1000);
//            var admissionGroupReply = searchCriteriaScreen.FindAdmissionGroupIselectorDropDown();
//            Thread.Sleep(1000);

//            Assert.IsTrue(yearGroupReply == "2015/2016", "Bad value: '" + yearGroupReply + "'");
//            Assert.IsTrue(intakeGroupReply == "2015/2016 - Summer Year 2", "Bad value: '" + intakeGroupReply + "'");
//            Assert.IsTrue(admissionGroupReply == "Admission Group 3-1 Inactive", "Bad value: '" + admissionGroupReply + "'");
//            WebContext.WebDriver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(0));
//        }

//        //[WebDriverTest(Enabled = true, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { ApplicantBulkUpdate.ApplicantBulkUpdateSalutationAddresseeSearchTests, PupilTestGroups.Priority.Priority3 })]
//        public void Search_Controls_Control_Test_For_Invalid_Selection()
//        {
//            WebContext.WebDriver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(10));
//            var bulkUpdateNavigation = new PupilBulkUpdateNavigation();
//            bulkUpdateNavigation.NavigateToBulkUpdateApplicantSalutationAndAddressee();
//            var searchCriteriaScreen = new ApplicantParentalSalutationAndAddresseeSearch();

//            // Control test
//            var badAdmissionGroupReply = searchCriteriaScreen.NeverFindAdmissionGroupIselectorDropDown();

//            Assert.IsTrue(badAdmissionGroupReply == "");
//            WebContext.WebDriver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(0));
//        }

//        #endregion

//        #region ApplicationStatus

//        //[WebDriverTest(Enabled = true, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { ApplicantBulkUpdate.ApplicationStatusIdentifierColumns, PupilTestGroups.Priority.Priority4 })]
//        public void ShouldDisplayApplicantIdentifierDialog()
//        {
//            WebContext.WebDriver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(10));
//            var bulkUpdateNavigation = new PupilBulkUpdateNavigation();
//            bulkUpdateNavigation.NavigateToBulkUpdateApplicationStatus();
//            var appStatusScreen = new ApplicationStatusSearch();
//            var detail = appStatusScreen.ClickOnSearch();

//            // Check if the Identifier button is displayed
//            Assert.IsTrue(detail.IdentifierMenuButton.Text == "Identifier Columns");
//            WebContext.WebDriver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(0));
//        }

//        //[WebDriverTest(Enabled = true, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { ApplicantBulkUpdate.ApplicationStatusIdentifierColumns, PupilTestGroups.Priority.Priority4 })]
//        public void ShouldDisplayDialogWithIdentifierColumns_OnClickOfIdentifiersButton()
//        {
//            WebContext.WebDriver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(10));
//            var bulkUpdateNavigation = new PupilBulkUpdateNavigation();
//            bulkUpdateNavigation.NavigateToBulkUpdateApplicationStatus();
//            ApplicationStatusSearch appStatusScreen = new ApplicationStatusSearch();
//            ApplicationStatusDetail detail = appStatusScreen.ClickOnSearch();

//            var identifierDialog = detail.ClickOnIdentifierDialogButton();

//            Assert.IsTrue((identifierDialog.PersonalDetailIdentifierCheckBox != null) &&
//                              (identifierDialog.DateOfBirthCheckBox != null) &&
//                              (identifierDialog.GenderCheckBox != null) &&
//                              (identifierDialog.RegisterationDetailCheckBox != null) &&
//                              (identifierDialog.AdmissionGroupCheckBox != null) &&
//                              (identifierDialog.ApplicationStatusCheckBox != null) &&
//                              (identifierDialog.ProposedDoaCheckBox != null) &&
//                              (identifierDialog.SchoolIntakeCheckBox != null) &&
//                              (identifierDialog.YearGroupCheckBox != null) &&
//                              (identifierDialog.ClassCheckBox != null)
//                 );
//            WebContext.WebDriver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(0));
//        }

//        //[WebDriverTest(Enabled = true, Groups = new[] { ApplicantBulkUpdate.ApplicationStatusIdentifierColumns, PupilTestGroups.Priority.Priority4 }, Browsers = new[] { BrowserDefaults.Chrome })]
//        public void IdentifierDialog_CheckDefaults()
//        {
//            WebContext.WebDriver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(10));
//            var bulkUpdateNavigation = new PupilBulkUpdateNavigation();
//            bulkUpdateNavigation.NavigateToBulkUpdateApplicationStatus();
//            var appStatusScreen = new ApplicationStatusSearch();
//            ApplicationStatusDetail detail = appStatusScreen.ClickOnSearch();
//            ApplicationStatusIdentifierDialog identifierDialog = detail.ClickOnIdentifierDialogButton();

//            //Are the default identifier columns checked 
//            Assert.IsTrue((identifierDialog.PersonalDetailIdentifierCheckBox.IsCheckboxChecked()) &&
//                              (identifierDialog.DateOfBirthCheckBox.IsCheckboxChecked()) &&
//                              (identifierDialog.GenderCheckBox.IsCheckboxChecked()) &&
//                              (identifierDialog.AdmissionGroupCheckBox.IsCheckboxChecked()) &&
//                              (identifierDialog.ApplicationStatusCheckBox.IsCheckboxChecked()));
//            Assert.IsFalse(identifierDialog.ProposedDoaCheckBox.IsCheckboxChecked());
//            Assert.IsFalse(identifierDialog.SchoolIntakeCheckBox.IsCheckboxChecked());
//            Assert.IsFalse(identifierDialog.YearGroupCheckBox.IsCheckboxChecked());
//            Assert.IsFalse(identifierDialog.ClassCheckBox.IsCheckboxChecked());

//            //Are the default selected identifier columns visible on the grid
//            SeleniumHelper.Get(ApplicationStatusDetail.Detail.GridColumns.DateOfBirth);
//            SeleniumHelper.Get(ApplicationStatusDetail.Detail.GridColumns.Gender);
//            SeleniumHelper.Get(ApplicationStatusDetail.Detail.GridColumns.AdmissionGroup);
//            SeleniumHelper.Get(ApplicationStatusDetail.Detail.GridColumns.ApplicantApplicationStatus);
//            SeleniumHelper.Get(ApplicationStatusDetail.Detail.GridColumns.CurrentApplicationStatus); // Editable column always visible
//            SeleniumHelper.Get(ApplicationStatusDetail.Detail.GridColumns.ApplicantName); //  column always visible
//            Assert.IsTrue(true);

//            WebContext.WebDriver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(0));

//        }

//        //[WebDriverTest(Enabled = true, Groups = new[] { ApplicantBulkUpdate.ApplicationStatusIdentifierColumns, PupilTestGroups.Priority.Priority4 }, Browsers = new[] { BrowserDefaults.Chrome })]
//        public void ShouldDisplayIdentifierColumnsinGrid_OnClickOfPupilIdentifiersOKButton()
//        {
//            WebContext.WebDriver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(10));
//            var bulkUpdateNavigation = new PupilBulkUpdateNavigation();
//            bulkUpdateNavigation.NavigateToBulkUpdateApplicationStatus();
//            var appStatusScreen = new ApplicationStatusSearch();
//            ApplicationStatusDetail detail = appStatusScreen.ClickOnSearch();
//            ApplicationStatusIdentifierDialog identifierDialog = detail.ClickOnIdentifierDialogButton();

//            SelectAllColumnsinIndentifierDialog(identifierDialog);

//            // Check the columns are added to the grid
//            SeleniumHelper.Get(ApplicationStatusDetail.Detail.GridColumns.DateOfBirth);
//            SeleniumHelper.Get(ApplicationStatusDetail.Detail.GridColumns.Gender);
//            SeleniumHelper.Get(ApplicationStatusDetail.Detail.GridColumns.AdmissionGroup);
//            SeleniumHelper.Get(ApplicationStatusDetail.Detail.GridColumns.ApplicantApplicationStatus);
//            SeleniumHelper.Get(ApplicationStatusDetail.Detail.GridColumns.CurrentApplicationStatus); // Editable column always visible
//            SeleniumHelper.Get(ApplicationStatusDetail.Detail.GridColumns.DateOfAmission);
//            SeleniumHelper.Get(ApplicationStatusDetail.Detail.GridColumns.SchoolIntake);
//            SeleniumHelper.Get(ApplicationStatusDetail.Detail.GridColumns.YearGroup);
//            SeleniumHelper.Get(ApplicationStatusDetail.Detail.GridColumns.Class);
//            SeleniumHelper.Get(ApplicationStatusDetail.Detail.GridColumns.ApplicantName); //  column always visible
//            Assert.IsTrue(true);
//            WebContext.WebDriver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(0));
//        }

//        /// <summary>
//        /// Should remove the column from grid on click of clear selection in the identifier dialog
//        /// </summary>
//        //[WebDriverTest(Enabled = true, Groups = new[] { ApplicantBulkUpdate.ApplicationStatusIdentifierColumns, PupilTestGroups.Priority.Priority4 }, Browsers = new[] { BrowserDefaults.Chrome })]
//        public void ShouldRemoveColumnsinGrid_OnClickOfClearSelection_PupilIdentifiersOKButton()
//        {
//            WebContext.WebDriver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(10));
//            var bulkUpdateNavigation = new PupilBulkUpdateNavigation();
//            bulkUpdateNavigation.NavigateToBulkUpdateApplicationStatus();
//            var appStatusScreen = new ApplicationStatusSearch();
//            ApplicationStatusDetail detail = appStatusScreen.ClickOnSearch();
//            ApplicationStatusIdentifierDialog identifierDialog = detail.ClickOnIdentifierDialogButton();

//            // Now click on clear selection within the Identifier Dialog to remove the selection and click on OK button
//            Thread.Sleep(2000);
//            identifierDialog.ClickOnClearSelection();
//            Thread.Sleep(2000);
//            identifierDialog.ClickOnIdentifierDialogOKButton();
//            Thread.Sleep(2000);

//            IWebElement grid = SeleniumHelper.Get(By.CssSelector("[data-section-id=\"searchResults\"]"));
//            var columns = grid.FindElements(By.CssSelector("[data-menu-column-id]"));

//            // Only the Applicant Name and Current Application Status column should be present in the grid
//            Assert.IsTrue(columns.Count == 2);
//            WebContext.WebDriver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(0));
//        }

//        private void SelectAllColumnsinIndentifierDialog(ApplicationStatusIdentifierDialog identifierDialog)
//        {
//            WebContext.WebDriver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(10));
//            //Click on Personal details and registration details so that all the columns are included in the grid
//            identifierDialog.PersonalDetailIdentifierCheckBox.SetCheckBox(true);
//            Thread.Sleep(2000);
//            identifierDialog.RegisterationDetailCheckBox.SetCheckBox(true);
//            Thread.Sleep(2000);
//            // Click on the ok button so that the columns are included in the grid
//            identifierDialog.ClickOnIdentifierDialogOKButton();
//            Thread.Sleep(2000);
//            WebContext.WebDriver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(0));
//        }

//        #endregion

//        #region MenuPage

//        //[WebDriverTest(Enabled = true, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { PupilTestGroups.PupilBulkUpdate.PupilBulkUpdateBackButtonSearchTests, PupilTestGroups.PupilBulkUpdate.PupilBulkUpdateTests, PupilTestGroups.Priority.Priority3 })]
//        public void BulkUpdate_BackButton_Should_Not_Be_Visible_On_Bulk_Update_Menu()
//        {
//            WebContext.WebDriver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(10));
//            var bulkUpdateNavigate = new PupilBulkUpdateNavigation();
//            bulkUpdateNavigate.NavigateToPupilBulkUpdateMenuPage();
//            Assert.IsFalse(bulkUpdateNavigate.BackButtonIsVisible());
//            WebContext.WebDriver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(0));
//        }

//        //[WebDriverTest(Enabled = true, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { PupilTestGroups.PupilBulkUpdate.PupilBulkUpdateBackButtonSearchTests, PupilTestGroups.PupilBulkUpdate.PupilBulkUpdateTests, PupilTestGroups.Priority.Priority3 })]
//        public void BulkUpdate_BackButton_Should_Be_Visible_On_Bulk_Search_Criteria()
//        {
//            WebContext.WebDriver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(10));
//            var bulkUpdateNavigate = new PupilBulkUpdateNavigation();
//            bulkUpdateNavigate.NavgateToPupilBulkUpdate_SubMenu(PupilBulkUpdateElements.BulkUpdate.MenuItems.PupilBasicDetailsMenuItem);
//            SeleniumHelper.Get(PupilBulkUpdateElements.BulkUpdate.Search.SearchCriteria);
//            Assert.IsTrue(bulkUpdateNavigate.BackButtonIsVisible());
//            WebContext.WebDriver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(0));
//        }

//        //[WebDriverTest(Enabled = true, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { PupilTestGroups.PupilBulkUpdate.PupilBulkUpdateBackButtonSearchTests, PupilTestGroups.PupilBulkUpdate.PupilBulkUpdateTests, PupilTestGroups.Priority.Priority3 })]
//        public void BulkUpdate_BackButton_Returns_To_Menu_When_Clicked()
//        {
//            WebContext.WebDriver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(10));
//            var bulkUpdateNavigate = new PupilBulkUpdateNavigation();
//            bulkUpdateNavigate.NavgateToPupilBulkUpdate_SubMenu(PupilBulkUpdateElements.BulkUpdate.MenuItems.PupilBasicDetailsMenuItem);
//            bulkUpdateNavigate.BackButtonClick();
//            Thread.Sleep(2000);
//            Assert.IsFalse(bulkUpdateNavigate.BackButtonIsVisible());
//            WebContext.WebDriver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(0));
//        }

//        #endregion

//        #region PupilParentalSalutation


//        //[WebDriverTest(Enabled = true, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { PupilBulkUpdateParentalSalutationAndAddressee.PupilBulkUpdateSalutationAndAddresseeIdentifierColumns, PupilTestGroups.Priority.Priority4 })]
//        public void PupilParentalSalutationShouldDisplayDialogWithIdentifierColumns_OnClickOfIdentifiersButton()
//        {
//            Select_Year_And_NavigateToBulkUpdateDetailScreen();

//            SeleniumHelper.FindAndClick(PupilBulkUpdateElements.BulkUpdate.Detail.IdentifierButton);
//            Thread.Sleep(2000);

//            //Check all the columns are present
//            SeleniumHelper.Get(PupilBulkUpdateElements.BulkUpdate.Detail.IdentifierDialog.PersonalDetails).Click();
//            SeleniumHelper.Get(PupilBulkUpdateElements.BulkUpdate.Detail.IdentifierDialog.DateOfBirth).Click();
//            SeleniumHelper.Get(PupilBulkUpdateElements.BulkUpdate.Detail.IdentifierDialog.LegalName).Click();
//            SeleniumHelper.Get(PupilBulkUpdateElements.BulkUpdate.Detail.IdentifierDialog.Gender).Click();

//            SeleniumHelper.Get(PupilBulkUpdateElements.BulkUpdate.Detail.IdentifierDialog.RegistrationDetails).Click();
//            SeleniumHelper.Get(PupilBulkUpdateElements.BulkUpdate.Detail.IdentifierDialog.AdmissionNumber).Click();
//            SeleniumHelper.Get(PupilBulkUpdateElements.BulkUpdate.Detail.IdentifierDialog.Class).Click();
//            SeleniumHelper.Get(PupilBulkUpdateElements.BulkUpdate.Detail.IdentifierDialog.DOA).Click();
//            SeleniumHelper.Get(PupilBulkUpdateElements.BulkUpdate.Detail.IdentifierDialog.YearGroup).Click();

//            Assert.IsTrue(true);
//        }


//        //[WebDriverTest(Enabled = true, Groups = new[] { PupilBulkUpdateParentalSalutationAndAddressee.PupilBulkUpdateSalutationAndAddresseeIdentifierColumns, PupilTestGroups.Priority.Priority4 }, Browsers = new[] { BrowserDefaults.Chrome })]
//        public void PupilParentalSalutationIdentifierDialog_CheckDefaults()
//        {
//            Select_Year_And_NavigateToBulkUpdateDetailScreen();

//            SeleniumHelper.FindAndClick(PupilBulkUpdateElements.BulkUpdate.Detail.IdentifierButton);
//            Thread.Sleep(2000);

//            //Are the default identifier columns checked 
//            Assert.IsTrue(SeleniumHelper.Get(PupilBulkUpdateElements.BulkUpdate.Detail.IdentifierDialog.DateOfBirthCheckBox).IsCheckboxChecked());
//            Assert.IsFalse(SeleniumHelper.Get(PupilBulkUpdateElements.BulkUpdate.Detail.IdentifierDialog.LegalNameCheckBox).IsCheckboxChecked());
//            Assert.IsFalse(SeleniumHelper.Get(PupilBulkUpdateElements.BulkUpdate.Detail.IdentifierDialog.GenderCheckBox).IsCheckboxChecked());
//            Assert.IsFalse(SeleniumHelper.Get(PupilBulkUpdateElements.BulkUpdate.Detail.IdentifierDialog.AdmissionNumberCheckBox).IsCheckboxChecked());
//            Assert.IsFalse(SeleniumHelper.Get(PupilBulkUpdateElements.BulkUpdate.Detail.IdentifierDialog.ClassCheckBox).IsCheckboxChecked());
//            Assert.IsFalse(SeleniumHelper.Get(PupilBulkUpdateElements.BulkUpdate.Detail.IdentifierDialog.DOACheckBox).IsCheckboxChecked());
//            Assert.IsFalse(SeleniumHelper.Get(PupilBulkUpdateElements.BulkUpdate.Detail.IdentifierDialog.YearGroupCheckBox).IsCheckboxChecked());

//            //Are the default selected identifier columns visible on the grid
//            SeleniumHelper.Get(ParentalSalutationAndAddresseeDetail.Detail.GridColumns.DateOfBirth);

//            Assert.IsTrue(true);
//        }

//        private void Select_Year_And_NavigateToBulkUpdateDetailScreen()
//        {
//            var bulkUpdateNavigation = new PupilBulkUpdateNavigation();
//            bulkUpdateNavigation.NavgateToPupilBulkUpdate_SubMenu(PupilBulkUpdateElements.BulkUpdate.MenuItems.PupilSalutationAddresseeMenuItem);

//            // Unselect Missing Salutation and Addressee checkboxes
//            SeleniumHelper.WaitForElementClickableThenClick(PupilBulkUpdateElements.BulkUpdate.Search.MissingSalutationCheckbox);
//            SeleniumHelper.WaitForElementClickableThenClick(PupilBulkUpdateElements.BulkUpdate.Search.MissingAddresseeCheckbox);

//            // Select Year 1 and Year 3 in the Search criteria
//            var searchCriteria = SeleniumHelper.Get(PupilBulkUpdateElements.BulkUpdate.Search.SearchCriteria);
//            Thread.Sleep(2000);
//            searchCriteria.FindCheckBoxAndClick("Year Group", new List<string> { "Year 1" });
//            Thread.Sleep(2000);

//            // Click on the Search button 
//            SeleniumHelper.FindAndClick(PupilBulkUpdateElements.BulkUpdate.Search.SearchButton, TimeSpan.FromSeconds(20));
//            Thread.Sleep(2000);
//        }

//        #endregion

//        //[WebDriverTest(Enabled = true, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { PupilTestGroups.PupilBulkUpdate.PupilBulkUpdateBasicDetailsSearchTests, PupilTestGroups.Priority.Priority4 })]
//        public void BasicDetails_ShouldDisplayBulkUpdatePupilDetails_OnClickOfMenuPupilDetail()//TODO: Already covered in previous tests. Hence P4
//        {
//            var bulkUpdateNavigate = new PupilBulkUpdateNavigation();
//            bulkUpdateNavigate.NavgateToPupilBulkUpdate_SubMenu(PupilBulkUpdateElements.BulkUpdate.MenuItems.PupilBasicDetailsMenuItem);

//            IWebElement searchCriteria = SeleniumHelper.Get(PupilBulkUpdateElements.BulkUpdate.Search.SearchCriteria);

//            Assert.IsTrue(searchCriteria.Displayed);
//        }

//        //[WebDriverTest(Enabled = false, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { PupilTestGroups.PupilBulkUpdate.PupilBulkUpdateConsentsSearchTests, PupilTestGroups.Priority.Priority4 })]
//        public void PupilConsents_ShouldDisplayBulkUpdatePupilDetails_OnClickOfMenuPupilDetail()
//        {
//            var bulkUpdateNavigate = new PupilBulkUpdateNavigation();
//            bulkUpdateNavigate.NavgateToPupilBulkUpdate_SubMenu(PupilBulkUpdateElements.BulkUpdate.MenuItems.PupilConsentsMenuItem);

//            IWebElement searchCriteria = SeleniumHelper.Get(PupilBulkUpdateElements.BulkUpdate.Search.SearchCriteria);

//            Assert.IsTrue(searchCriteria.Displayed);
//        }//TODO: Already covered in previous tests. Hence P4

//        //[WebDriverTest(Enabled = true, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { PupilTestGroups.PupilBulkUpdate.PupilBulkUpdateSalutationAddresseeSearchTests, PupilTestGroups.Priority.Priority4 })]
//        public void PupilSalutationAddressee_ShouldDisplayBulkUpdatePupilDetails_OnClickOfMenuPupilDetail()//TODO: Already covered in previous tests. Hence P4
//        {
//            var bulkUpdateNavigate = new PupilBulkUpdateNavigation();
//            bulkUpdateNavigate.NavgateToPupilBulkUpdate_SubMenu(PupilBulkUpdateElements.BulkUpdate.MenuItems.PupilSalutationAddresseeMenuItem);

//            IWebElement searchCriteria = SeleniumHelper.Get(PupilBulkUpdateElements.BulkUpdate.Search.SearchCriteria);

//            Assert.IsTrue(searchCriteria.Displayed);
//        }

//        //[WebDriverTest(Enabled = false, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { PupilTestGroups.PupilBulkUpdate.PupilBulkUpdateBasicDetailsSearchTests, PupilTestGroups.Priority.Priority4 })]
//        public void BasicDetails_SearchPanelShouldContain_Year_Class_And_SearchButton()
//        {
//            var bulkUpdateNavidate = new PupilBulkUpdateNavigation();
//            bulkUpdateNavidate.NavgateToPupilBulkUpdate_SubMenu(PupilBulkUpdateElements.BulkUpdate.MenuItems.PupilBasicDetailsMenuItem);

//            // Select Year 1 and Year 3 in the Search criteria
//            var searchCriteria = SeleniumHelper.Get(PupilBulkUpdateElements.BulkUpdate.Search.SearchCriteria);
//            Thread.Sleep(2000);

//            searchCriteria.FindCheckBoxAndClick("Year Group", new List<string> { "Year 1", "Year 3" });
//            Thread.Sleep(2000);

//            searchCriteria.FindCheckBoxAndClick("Class", new List<string> { "Robin", "Wren" });
//            Thread.Sleep(2000);

//            // Look for the Search button 
//            Assert.IsTrue(SeleniumHelper.Get(PupilBulkUpdateElements.BulkUpdate.Search.SearchButton).Text == "Search");
//        }

//        //[WebDriverTest(Enabled = false, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { PupilTestGroups.PupilBulkUpdate.PupilBulkUpdateConsentsSearchTests, PupilTestGroups.Priority.Priority4 })]
//        public void PupilConsents_SearchPanelShouldContain_Year_Class_And_SearchButton()
//        {
//            var bulkUpdateNavidate = new PupilBulkUpdateNavigation();
//            bulkUpdateNavidate.NavgateToPupilBulkUpdate_SubMenu(PupilBulkUpdateElements.BulkUpdate.MenuItems.PupilConsentsMenuItem);

//            // Select Year 1 and Year 3 in the Search criteria
//            var searchCriteria = SeleniumHelper.Get(PupilBulkUpdateElements.BulkUpdate.Search.SearchCriteria);
//            Thread.Sleep(2000);

//            searchCriteria.FindCheckBoxAndClick("Year Group", new List<string> { "Year 1", "Year 3" });
//            Thread.Sleep(2000);

//            searchCriteria.FindCheckBoxAndClick("Class", new List<string> { "Robin", "Wren" });
//            Thread.Sleep(2000);

//            // Look for the Search button 
//            Assert.IsTrue(SeleniumHelper.Get(PupilBulkUpdateElements.BulkUpdate.Search.SearchButton).Text == "Search");
//        }

//        //[WebDriverTest(Enabled = true, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { PupilTestGroups.PupilBulkUpdate.PupilBulkUpdateSalutationAddresseeSearchTests, PupilTestGroups.Priority.Priority4 })]
//        public void PupilSalutationAddressee_SearchPanelShouldContain_2Checkboxes_Year_Class_And_SearchButton()
//        {
//            var bulkUpdateNavidate = new PupilBulkUpdateNavigation();
//            bulkUpdateNavidate.NavgateToPupilBulkUpdate_SubMenu(PupilBulkUpdateElements.BulkUpdate.MenuItems.PupilSalutationAddresseeMenuItem);

//            // Select MissingSalutation and Missing Addressee checkboxes in the Search criteria
//            SeleniumHelper.WaitForElementClickableThenClick(PupilBulkUpdateElements.BulkUpdate.Search.MissingSalutationCheckbox);
//            SeleniumHelper.WaitForElementClickableThenClick(PupilBulkUpdateElements.BulkUpdate.Search.MissingAddresseeCheckbox);

//            // Select Year 1 and Year 3 in the Search criteria
//            var searchCriteria = SeleniumHelper.Get(PupilBulkUpdateElements.BulkUpdate.Search.SearchCriteria);
//            Thread.Sleep(2000);

//            searchCriteria.FindCheckBoxAndClick("Year Group", new List<string> { "Year 1", "Year 3" });
//            Thread.Sleep(2000);

//            searchCriteria.FindCheckBoxAndClick("Class", new List<string> { "Robin", "Wren" });
//            Thread.Sleep(2000);

//            // Look for the Search button 
//            Assert.IsTrue(SeleniumHelper.Get(PupilBulkUpdateElements.BulkUpdate.Search.SearchButton).Text == "Search");
//        }

//        //[WebDriverTest(Enabled = true, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { PupilTestGroups.PupilBulkUpdate.PupilBulkUpdateIdentifierColumnsTests, PupilTestGroups.Priority.Priority4 })]
//        public void ShouldDisplayPupilIdentifiersButton()
//        {
//            Select_Year_And_NavidateToBulkUpdateDetailScreen();

//            // Click on the Search button 
//            //SeleniumHelper.FindAndClick(PupilBulkUpdateElements.BulkUpdate.Search.SearchButton, TimeSpan.FromSeconds(20));
//            //Thread.Sleep(2000);
//            // Check if the Identifier button is displayed
//            var actualButtonText = SeleniumHelper.Get(PupilBulkUpdateElements.BulkUpdate.Detail.IdentifierButton).Text;
//            Assert.IsTrue(actualButtonText == "Identifier Columns");
//        }

//        //[WebDriverTest(Enabled = true, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { PupilTestGroups.PupilBulkUpdate.PupilBulkUpdateIdentifierColumnsTests, PupilTestGroups.Priority.Priority4 })]
//        public void ShouldDisplayDialogWithIdentifierColumns_OnClickOfPupilIdentifiersButton()
//        {
//            Select_Year_And_NavidateToBulkUpdateDetailScreen();

//            SeleniumHelper.FindAndClick(PupilBulkUpdateElements.BulkUpdate.Detail.IdentifierButton);
//            Thread.Sleep(2000);

//            //Check all the columns are present
//            SeleniumHelper.Get(PupilBulkUpdateElements.BulkUpdate.Detail.IdentifierDialog.PersonalDetails).Click();
//            SeleniumHelper.Get(PupilBulkUpdateElements.BulkUpdate.Detail.IdentifierDialog.DateOfBirth).Click();
//            SeleniumHelper.Get(PupilBulkUpdateElements.BulkUpdate.Detail.IdentifierDialog.LegalName).Click();
//            SeleniumHelper.Get(PupilBulkUpdateElements.BulkUpdate.Detail.IdentifierDialog.Gender).Click();

//            SeleniumHelper.Get(PupilBulkUpdateElements.BulkUpdate.Detail.IdentifierDialog.RegistrationDetails).Click();
//            SeleniumHelper.Get(PupilBulkUpdateElements.BulkUpdate.Detail.IdentifierDialog.AdmissionNumber).Click();
//            SeleniumHelper.Get(PupilBulkUpdateElements.BulkUpdate.Detail.IdentifierDialog.Class).Click();
//            SeleniumHelper.Get(PupilBulkUpdateElements.BulkUpdate.Detail.IdentifierDialog.DOA).Click();
//            SeleniumHelper.Get(PupilBulkUpdateElements.BulkUpdate.Detail.IdentifierDialog.YearGroup).Click();

//            Assert.IsTrue(true);
//        }

//        /// <summary>
//        /// Should remove the columnsi n grid on click of clear selection in the pupil identifierdialog
//        /// </summary>
//        //[WebDriverTest(Enabled = true, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { PupilTestGroups.PupilBulkUpdate.PupilBulkUpdateIdentifierColumnsTests, PupilTestGroups.Priority.Priority4 })]
//        public void ToolbarTestsShouldRemoveColumnsinGrid_OnClickOfClearSelection_PupilIdentifiersOKButton()
//        {
//            Select_Year_And_NavidateToBulkUpdateDetailScreen();

//            SelectAllColumnsinIndentifierDialog();

//            // Now click on clear selection within the Identifier Dialog to remove the selection and  click on OK button
//            SeleniumHelper.FindAndClick(PupilBulkUpdateElements.BulkUpdate.Detail.IdentifierButton);
//            Thread.Sleep(2000);
//            SeleniumHelper.FindAndClick(PupilBulkUpdateElements.BulkUpdate.Detail.IdentifierDialog.ClearSelection);
//            Thread.Sleep(2000);
//            SeleniumHelper.FindAndClick(PupilBulkUpdateElements.BulkUpdate.Detail.IdentifierDialog.OkButton);
//            Thread.Sleep(2000);

//            IWebElement grid = SeleniumHelper.Get(By.CssSelector("[data-section-id=\"searchResults\"]"));
//            var columns = grid.FindElements(By.CssSelector("[data-menu-column-id]"));

//            // Only the Pupil Name column should be present in the grid
//            Assert.IsTrue(columns.Count == 1);
//        }

//        #region Data items dialog
//        //[WebDriverTest(Enabled = true, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { PupilTestGroups.PupilBulkUpdate.PupilBulkUpdateTests, PupilTestGroups.Priority.Priority4 })]
//        public void DataItemsButtonShouldOnlyDisplayDataItems()
//        {
//            var bulkUpdateNavigation = new PupilBulkUpdateNavigation();
//            bulkUpdateNavigation.NavgateToPupilBulkUpdate_SubMenu(PupilBulkUpdateElements.BulkUpdate.MenuItems.PupilBasicDetailsMenuItem);

//            // Select Year 1 and Year 3 in the Search criteria
//            var searchCriteria = SeleniumHelper.Get(PupilBulkUpdateElements.BulkUpdate.Search.SearchCriteria);
//            Thread.Sleep(2000);
//            searchCriteria.FindCheckBoxAndClick("Year Group", new List<string> { "Year 1" });
//            Thread.Sleep(2000);

//            // Click on the Search button 
//            SeleniumHelper.FindAndClick(PupilBulkUpdateElements.BulkUpdate.Search.SearchButton, TimeSpan.FromSeconds(20));
//            Thread.Sleep(2000);

//            // Click on the Data Items button
//            SeleniumHelper.FindAndClick(PupilBulkUpdateElements.BulkUpdate.Detail.DataItemsButton);
//            Thread.Sleep(2000);

//            // Check that only identifiers are displayed
//            var panelGroupTitle = SeleniumHelper.Get(PupilBulkUpdateElements.BulkUpdate.Detail.DataItemsDialog.SectionPanelsGroup).Text;

//            // Check its title is correct
//            Assert.AreEqual(panelGroupTitle, "Pupil Data Items");

//            // Click on the Cancel button
//            SeleniumHelper.FindAndClick(PupilBulkUpdateElements.BulkUpdate.Detail.DataItemsDialog.DialogCancelButton);
//        }
//        #endregion

//        private void Select_Year_And_NavidateToBulkUpdateDetailScreen()
//        {
//            var bulkUpdateNavigation = new PupilBulkUpdateNavigation();
//            bulkUpdateNavigation.NavgateToPupilBulkUpdate_SubMenu(PupilBulkUpdateElements.BulkUpdate.MenuItems.PupilBasicDetailsMenuItem);

//            // Select Year 1 and Year 3 in the Search criteria
//            var searchCriteria = SeleniumHelper.Get(PupilBulkUpdateElements.BulkUpdate.Search.SearchCriteria);
//            Thread.Sleep(2000);
//            searchCriteria.FindCheckBoxAndClick("Year Group", new List<string> { "Year 1", "Year 3" });
//            Thread.Sleep(2000);

//            // Click on the Search button 
//            SeleniumHelper.FindAndClick(PupilBulkUpdateElements.BulkUpdate.Search.SearchButton, TimeSpan.FromSeconds(20));
//            Thread.Sleep(2000);
//        }

//        private void SelectAllColumnsinIndentifierDialog()
//        {
//            SeleniumHelper.FindAndClick(PupilBulkUpdateElements.BulkUpdate.Detail.IdentifierButton);
//            Thread.Sleep(2000);

//            //Click on Personal details and registration details so that all the columns are included in the grid
//            SeleniumHelper.Get(PupilBulkUpdateElements.BulkUpdate.Detail.IdentifierDialog.PersonalDetails).Click();
//            Thread.Sleep(2000);
//            SeleniumHelper.Get(PupilBulkUpdateElements.BulkUpdate.Detail.IdentifierDialog.RegistrationDetails).Click();
//            Thread.Sleep(2000);
//            // Click on the ok button so that the columns are included in the grid
//            SeleniumHelper.FindAndClick(PupilBulkUpdateElements.BulkUpdate.Detail.IdentifierDialog.OkButton);
//            Thread.Sleep(2000);
//        }
//    }
//}
