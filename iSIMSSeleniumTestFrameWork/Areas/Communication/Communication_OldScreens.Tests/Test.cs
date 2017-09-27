using POM.Components.Pupil;
using POM.Helper;
using Attendance.POM.DataHelper;
using POM.Base;
using TestSettings;
using WebDriverRunner.internals;
using System.Linq;
using System.Collections.Generic;
using System;
using POM.Components.HomePages;
using System.Globalization;
using POM.Components.Common;
using Attendance.POM.Components.Communications;
using POM.Components.Communications;
using POM.Components.Communication;
using Attendance.POM.Components.Communication;
using NUnit.Framework;
using System.Threading;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using WebDriverRunner.webdriver;
using SeSugar;
using SeSugar.Automation;
using SeSugar.Data;
using Selene.Support.Attributes;



namespace Communication_OldScreens.Tests
{
    class Test
    {

        /// <summary>
        /// Author: Huy.Vo
        /// Description: Record a new local medical practice so it is available for selection at the local school only.
        /// </summary>
        [WebDriverTest(TimeoutSeconds = 2400, Enabled = true, DataProvider = "TC_COM001_Data", Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { "g1", "All", "P2" })]
        public void TC_COM001_Add_New_Medical_Practice(string medicalPracticeName, string telephoneNumber, string doctorTitle, string doctorSureName, string doctorMiddleName, string doctorForeName)
        {
            //Login as SchoolAdministrator and navigate to Medical Practices page
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);
            AutomationSugar.NavigateMenu("Tasks", "Communications", "Medical Practices");


            #region STEPS

            //  Search and delete if Medical practice is existing
            var medicalPracticeTriplet = new MedicalPracticeTriplet();
            medicalPracticeTriplet.SearchCriteria.SearchByName = medicalPracticeName;
            var medicalPracticeResults = medicalPracticeTriplet.SearchCriteria.Search();
            var medicalPracticeTile = medicalPracticeResults.SingleOrDefault(t => t.Name.Equals(medicalPracticeName));
            medicalPracticeTriplet.SelectSearchTile(medicalPracticeTile);
            medicalPracticeTriplet.Delete();

            // Create new Medical Practice 
            var medicalPracticePage = medicalPracticeTriplet.Create();
            medicalPracticePage.PracticeName = medicalPracticeName;
            //      var telephoneNumberTable = medicalPracticePage.TelephoneNumbers;
            //      telephoneNumberTable[0].TelephoneNumbers = telephoneNumber;

            // Add new Doctor's informations
            medicalPracticePage.ScrollToAssociated();
            var doctorDialog = medicalPracticePage.AddDoctor();
            doctorDialog.Title = doctorTitle;
            doctorDialog.ForeName = doctorForeName;
            doctorDialog.MiddleName = doctorMiddleName;
            doctorDialog.SureName = doctorSureName;

            doctorDialog.OK();
            medicalPracticePage.Save();
            //  Wait.WaitForAjaxReady(By.CssSelector(".locking-mask-loading"));
            // Verify that save message succesfully display
            Assert.AreEqual(true, medicalPracticePage.IsSuccessMessageIsDisplayed(), "Create new Medical practic unsuccessfully");

            // Re-search new Medical Practice to verify create new successfully
            medicalPracticeTriplet.SearchCriteria.SearchByName = medicalPracticeName;
            medicalPracticeResults = medicalPracticeTriplet.SearchCriteria.Search();
            medicalPracticeTile = medicalPracticeResults.SingleOrDefault(t => t.Name.Equals(medicalPracticeName));
            medicalPracticePage = medicalPracticeTile.Click<MedicalPracticePage>();

            // Verify that new informations of Medical practice are displayed on screen
            Assert.AreEqual(medicalPracticeName, medicalPracticePage.PracticeName, "Medical Practice nam is not exist");
            //      telephoneNumberTable = medicalPracticePage.TelephoneNumbers;
            //       Assert.AreNotEqual(null, (telephoneNumberTable.Rows.SingleOrDefault(x => x.TelephoneNumbers == telephoneNumber)), "Creating Medical Practice was failed");

            var doctorTable = medicalPracticePage.Doctors;
            Wait.WaitForAjaxReady(By.CssSelector(".locking-mask-loading"));
            //Assert.AreNotEqual(null, (doctorTable.Rows.SingleOrDefault(x => x.Name == doctorTitle + " " + doctorForeName[0].ToString().ToUpper() + " " + doctorSureName)), "Creating Medical Practice was failed");

            #endregion

            #region POS-CONDITIONS
            // Delete Medical Practice
            var confirmDeleteDialog = medicalPracticePage.Delete();
            confirmDeleteDialog.ConfirmDelete();
            #endregion POS-CONDITIONS

        }

        [WebDriverTest(TimeoutSeconds = 2400, Enabled = true, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { "Add", "MedicalPracticeAddress", "Add_Medical_Practice_Address", "All", "P2" })]
        [Variant(Variant.EnglishStatePrimary | Variant.EnglishStateSecondary | Variant.EnglishStateMultiphase |
            Variant.WelshStatePrimary | Variant.WelshStateSecondary | Variant.WelshStateMultiphase)]
        public void Add_Update_Delete_Address()
        {
            #region Arrange

            Guid medicalPracticeID, doctorID;
            string practiceName = CoreQueries.GetColumnUniqueString("MedicalPractice", "Name", 10, SeSugar.Environment.Settings.TenantId);
            string doctorName = CoreQueries.GetColumnUniqueString("Doctor", "Surname", 10, SeSugar.Environment.Settings.TenantId);

            string PAONRange = "22";
            string Street = "SUDELEY WALK";
            string Town = "BEDFORD";
            string PostCode = "MK41 8HS";
            string Country = "United Kingdom";
            string NewStreet = "SUDELEY NEW WALK";
            const string _space = " ";
            const string _seperator = ", ";
            const string _lineSeperator = "\r\n";

            var addressDisplaySmall = string.Concat(
                PAONRange, _seperator, Street, _seperator, Town);

            var addressDisplayLarge = string.Concat(
                PAONRange, _lineSeperator, 
                Street, _lineSeperator,
                Town, _lineSeperator,
                Town, _lineSeperator,
                PostCode, _lineSeperator,
                Country);

            var packages = new DataPackage[]
            {
                GetMedicalPractice(out medicalPracticeID, out doctorID, practiceName, doctorName)
            };

            #endregion

            using (new DataSetup(packages))
            {
                SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator, enabledFeatures: "Addresses");
                AutomationSugar.NavigateMenu("Tasks", "Communications", "Medical Practices");

                MedicalPracticeTriplet medicalPracticeTriplet = new MedicalPracticeTriplet();
                medicalPracticeTriplet.SearchCriteria.SearchByName = practiceName;
                var medicalPracticeResults = medicalPracticeTriplet.SearchCriteria.Search();
                var medicalPracticeTile = medicalPracticeResults.SingleOrDefault(t => t.Name.Equals(practiceName));
                MedicalPracticePage medicalPracticePage = medicalPracticeTile.Click<MedicalPracticePage>();
                Wait.WaitForDocumentReady();

                //Add Address
                medicalPracticePage.ClickAddAddrss();
                var addressDialog = new POM.Components.Communication.AddAddressDialog();
                addressDialog.PAONRangeSearch = PAONRange;
                addressDialog.PostCodeSearch = PostCode;
                addressDialog.ClickSearch();
                //addressDialog.Addresses = addressDialog.Addresses;
                addressDialog.Street = Street;
                addressDialog.District = Town;
                addressDialog.Town = Town;
                addressDialog.PostCode = PostCode;
                addressDialog.ClickOk();
                medicalPracticePage.Save();
                Assert.AreEqual(addressDisplayLarge, medicalPracticePage.MedicalPracticeAddresss);
                
                //Edit Address
                addressDisplayLarge = string.Concat(
                PAONRange, _lineSeperator,
                NewStreet, _lineSeperator,
                Town, _lineSeperator,
                Town, _lineSeperator,
                PostCode, _lineSeperator,
                Country);
                medicalPracticePage.ClickEditAddrss();
                var editAddressDialog = new POM.Components.Communication.AddAddressDialog();
                editAddressDialog.PAONRangeSearch = PAONRange;
                editAddressDialog.PostCodeSearch = PostCode;
                editAddressDialog.ClickSearch();
                editAddressDialog.Street = NewStreet;
                editAddressDialog.ClickOk();
                medicalPracticePage.Save();
                Assert.AreEqual(addressDisplayLarge, medicalPracticePage.MedicalPracticeAddresss);

                //Delete Address
                const string emptyAddress = "Address Not Defined";
                medicalPracticePage.ClickDeleteAddrss();
                Assert.AreEqual(emptyAddress, medicalPracticePage.MedicalPracticeAddresss);
            }
        }

        [WebDriverTest(TimeoutSeconds = 2400, Enabled = true, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { "Add", "AgencyAddress", "Add_Update_Delete_Agency_Address", "All", "P2" })]
        [Variant(Variant.EnglishStatePrimary | Variant.EnglishStateSecondary | Variant.EnglishStateMultiphase |
            Variant.WelshStatePrimary | Variant.WelshStateSecondary | Variant.WelshStateMultiphase)]
        public void Add_Update_Delete_Agency_Address()
        {
            #region Arrange

            Guid agencyID;
            string agencyName = CoreQueries.GetColumnUniqueString("Agency", "AgencyName", 10, SeSugar.Environment.Settings.TenantId);

            string newLocality = Utilities.GenerateRandomString(6);

            string PAONRange = "22";
            string Street = "SUDELEY WALK";
            string Town = "BEDFORD";
            string PostCode = "MK41 8HS";
            string Country = "United Kingdom";

            const string _space = " ";
            const string _seperator = ", ";
            const string _lineSeperator = "\r\n";

            var addressDisplaySmall = string.Concat(
                PAONRange, _seperator, Street, _seperator, Town);

            var addressDisplayLarge = string.Concat(
                PAONRange, _space, Street, _lineSeperator,
                Town, _lineSeperator,
                Town, _lineSeperator,
                PostCode, _lineSeperator,
                Country);

            var packages = new DataPackage[]
            {
                GetAgency(out agencyID, agencyName)
            };

            #endregion

            using (new DataSetup(packages))
            {
                SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator, enabledFeatures: "Addresses");
                AutomationSugar.NavigateMenu("Tasks", "Communications", "Agencies");

                AgencyTriplet agencyTriplet = new AgencyTriplet();
                agencyTriplet.SearchCriteria.AgencyName = agencyName;
                var agencyResults = agencyTriplet.SearchCriteria.Search();
                var agencyResultTile = agencyResults.SingleOrDefault(t => t.AgencyName.Equals(agencyName));
                AgencyDetailPage agencyPage = agencyResultTile.Click<AgencyDetailPage>();
                Wait.WaitForDocumentReady();

                //Add Address
                agencyPage.AddAddress();
                var addressDialog = new POM.Components.Communication.AddAddressDialog();
                addressDialog.PAONRangeSearch = PAONRange;
                addressDialog.PostCodeSearch = PostCode;
                addressDialog.ClickSearch();

                addressDialog.Addresses = addressDisplaySmall;

                Assert.AreEqual(PAONRange, addressDialog.BuildingNo);
                Assert.AreEqual(Street, addressDialog.Street);
                Assert.AreEqual(Town, addressDialog.Town);
                Assert.AreEqual(Town, addressDialog.County);
                Assert.AreEqual(PostCode, addressDialog.PostCode);

                addressDialog.ClickOk();
                agencyPage.Save();
                Assert.AreEqual(addressDisplayLarge, agencyPage.Address);

                //Edit Address
                agencyPage.EditAddress();
                addressDialog = new POM.Components.Communication.AddAddressDialog();
                addressDialog.District = newLocality;
                addressDialog.ClickOk();
                agencyPage.Save();

                addressDisplayLarge = string.Concat(
                PAONRange, _space, Street, _lineSeperator,
                newLocality, _lineSeperator,
                Town, _lineSeperator,
                Town, _lineSeperator,
                PostCode, _lineSeperator,
                Country);

                Assert.AreEqual(addressDisplayLarge, agencyPage.Address);

                //Delete Address
                agencyPage.DeleteAddress();
                agencyPage.Save();

                Assert.AreEqual("Address Not Defined", agencyPage.Address);
            }
        }

        [WebDriverTest(TimeoutSeconds = 2400, Enabled = true, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { "Add", "AgentAddress", "Add_Update_Delete_Agent_Address", "All", "P2" })]
        [Variant(Variant.EnglishStatePrimary | Variant.EnglishStateSecondary | Variant.EnglishStateMultiphase |
         Variant.WelshStatePrimary | Variant.WelshStateSecondary | Variant.WelshStateMultiphase)]
        public void Add_Update_Delete_Agent_Address()
        {
            #region Arrange

            Guid agentID;
            string agentForeName = CoreQueries.GetColumnUniqueString("Agent", "LegalForename", 10, SeSugar.Environment.Settings.TenantId);
            string agentSurName = CoreQueries.GetColumnUniqueString("Agent", "LegalSurname", 10, SeSugar.Environment.Settings.TenantId);
            string agentName = agentSurName + ", " + agentForeName;
            string newLocality = Utilities.GenerateRandomString(6);

            string PAONRange = "22";
            string Street = "SUDELEY WALK";
            string Town = "BEDFORD";
            string PostCode = "MK41 8HS";
            string Country = "United Kingdom";

            const string _space = " ";
            const string _seperator = ", ";
            const string _lineSeperator = "\r\n";

            var addressDisplaySmall = string.Concat(
                PAONRange, _seperator, Street, _seperator, Town);

            var addressDisplayLarge = string.Concat(
                PAONRange, _lineSeperator, Street, _lineSeperator,
                Town, _lineSeperator,
                Town, _lineSeperator,
                PostCode, _lineSeperator,
                Country);

            var packages = new DataPackage[]
            {
                GetAgent(out agentID, agentForeName, agentSurName)
            };

            #endregion

            using (new DataSetup(packages))
            {
                SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator, enabledFeatures: "Addresses");
                AutomationSugar.NavigateMenu("Tasks", "Communications", "Agents");

                AgentDetailTriplet agentTriplet = new AgentDetailTriplet();
                agentTriplet.SearchCriteria.AgentName = agentName;
                var agentResults = agentTriplet.SearchCriteria.Search();
                var agentResultTile = agentResults.SingleOrDefault(t => t.Name.Equals(agentName));
                AgentDetailPage agentPage = agentResultTile.Click<AgentDetailPage>();
                Wait.WaitForDocumentReady();

                //Add Address
                agentPage.AddAddress();
                var addressDialog = new POM.Components.Communication.AddAddressDialog();
                addressDialog.PAONRangeSearch = PAONRange;
                addressDialog.PostCodeSearch = PostCode;
                addressDialog.ClickSearch();

                addressDialog.Addresses = addressDisplaySmall;

                Assert.AreEqual(PAONRange, addressDialog.BuildingNo);
                Assert.AreEqual(Street, addressDialog.Street);
                Assert.AreEqual(Town, addressDialog.Town);
                Assert.AreEqual(Town, addressDialog.County);
                Assert.AreEqual(PostCode, addressDialog.PostCode);

                addressDialog.ClickOk();
                agentPage.Save();
                Assert.AreEqual(addressDisplayLarge, agentPage.Address);

                //Edit Address
                agentPage.EditNewAddress();
                addressDialog = new POM.Components.Communication.AddAddressDialog();
                addressDialog.District = newLocality;
                addressDialog.ClickOk();
                agentPage.Save();

                addressDisplayLarge = string.Concat(
                PAONRange, _lineSeperator, Street, _lineSeperator,
                newLocality, _lineSeperator,
                Town, _lineSeperator,
                Town, _lineSeperator,
                PostCode, _lineSeperator,
                Country);

                Assert.AreEqual(addressDisplayLarge, agentPage.Address);

                //Delete Address
                agentPage.DeleteAddress();
                agentPage.Save();

                Assert.AreEqual("Address Not Defined", agentPage.Address);
            }
        }


        /// <summary>
        /// Author: Huy.Vo
        /// Description: Check that the local medical practice created previously can be selected on a pupil record by a School Administrator user. 
        /// </summary>
        [WebDriverTest(TimeoutSeconds = 3000, Enabled = true, DataProvider = "TC_COM002_Data", Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { "g1", "All", "P2" })]
        public void TC_COM002_Select_Medical_Practice_For_Pupils(string medicalPracticeName, string telephoneNumber, string doctorTitle, string doctorSureName, string doctorMiddleName, string doctorForeName)
        {

            
            //Login as SchoolAdministrator and navigate to Medical Practices page
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);

          

            AutomationSugar.Log("Data Creation started");
            Guid pupilId = Guid.NewGuid();
            DataPackage dataPackage = this.BuildDataPackage();
            var sureName = Utilities.GenerateRandomString(10, "TC02_Surname");
            var foreName = Utilities.GenerateRandomString(10, "TC02_Forename" + Thread.CurrentThread.ManagedThreadId);
            dataPackage.AddBasicLearner(pupilId, sureName, foreName, new DateTime(2005, 10, 01), new DateTime(2012, 08, 01));

        

            DataSetup DataSetPupil = new DataSetup(purgeBeforeInsert: false, purgeAfterTest: true, packages: dataPackage);

            AutomationSugar.NavigateMenu("Tasks", "Communications", "Medical Practices");


         
            #region PRE-CONDITIONS

            //  Search and delete if Medical practice is existing
            var medicalPracticeTriplet = new MedicalPracticeTriplet();
            medicalPracticeTriplet.SearchCriteria.SearchByName = medicalPracticeName;
            var medicalPracticeResults = medicalPracticeTriplet.SearchCriteria.Search();
            var medicalPracticeTile = medicalPracticeResults.SingleOrDefault(t => t.Name.Equals(medicalPracticeName));
            medicalPracticeTriplet.SelectSearchTile(medicalPracticeTile);
            medicalPracticeTriplet.Delete();

            // Create new Medical Practice 
            var medicalPracticePage = medicalPracticeTriplet.Create();
            medicalPracticePage.PracticeName = medicalPracticeName;
            //        var telephoneNumberTable = medicalPracticePage.TelephoneNumbers;
            //       telephoneNumberTable[0].TelephoneNumbers = telephoneNumber;

            //   medicalPracticePage.ScrollToAssociated();

            // Add new Doctor's informations
            var doctorDialog = medicalPracticePage.AddDoctor();
            doctorDialog.Title = doctorTitle;
            doctorDialog.ForeName = doctorForeName;
            doctorDialog.MiddleName = doctorMiddleName;
            doctorDialog.SureName = doctorSureName;
            doctorDialog.OK();
            medicalPracticePage.Save();

            #endregion PRE-CONDITIONS

            #region STEPS
            //     Wait.WaitForAjaxReady(By.CssSelector(".locking-mask-loading"));
            SeleniumHelper.NavigateQuickLink("Pupil Records");
            var pupilRecordsTriplet = new PupilRecordTriplet();

            //Select an existing pupil to update Medical
            pupilRecordsTriplet.SearchCriteria.PupilName = sureName;
            pupilRecordsTriplet.SearchCriteria.IsCurrent = true;
            var pupilsearchresults = pupilRecordsTriplet.SearchCriteria.Search();
            var pupilTile = pupilsearchresults.FirstOrDefault(t => t.Name.Equals(String.Format("{0}, {1}", sureName, foreName)));
            var pupilRecordPage = pupilTile.Click<PupilRecordPage>();

            // Add new Medical Practice
            pupilRecordPage.SelectMedicalTab();
            var medicalPracticeDialogTriplet = pupilRecordPage.ClickMedicalPractice();
            medicalPracticeDialogTriplet.SearchCriteria.MedicalPracticeName = medicalPracticeName;
            var medicalPracticeDialogResults = medicalPracticeDialogTriplet.SearchCriteria.Search();
            var medicalPracticeDialogTile = medicalPracticeDialogResults.SingleOrDefault(t => t.Name.Equals(medicalPracticeName));
            var medicalPracticeDialogDetails = medicalPracticeDialogTile.Click<MedicalPracticeDialog>();
            medicalPracticeDialogTriplet.OK();

            // Save Pupil records
            pupilRecordPage.ClickSave();

            //Re-search Pupil to verify add Medical Practice successufully
            pupilRecordsTriplet.SearchCriteria.PupilName = string.Format("{0}, {1}", sureName, foreName);
            pupilRecordsTriplet.SearchCriteria.IsCurrent = true;
            pupilsearchresults = pupilRecordsTriplet.SearchCriteria.Search();
            pupilTile = pupilsearchresults.FirstOrDefault(t => t.Name.Equals(String.Format("{0}, {1}", sureName, foreName)));
            pupilRecordPage = pupilTile.Click<PupilRecordPage>();

            // Verify that add new Medical Practice is displayed on table
            //pupilRecordPage.SelectMedicalTab();
            var medicalPracticeTable = pupilRecordPage.MedicalPractice;
            Assert.AreNotEqual(null, (medicalPracticeTable.Rows.FirstOrDefault(x => x.Name == medicalPracticeName)), "Add Medical Practice was failed");

            #endregion STEPS

            #region POS-CONDITIONS

            medicalPracticeTable.Rows.FirstOrDefault(x => x.Name.Equals(medicalPracticeName)).DeleteRow();
            pupilRecordPage.ClickSave();

            // Re-select Practice Name to Delete
            AutomationSugar.NavigateMenu("Tasks", "Communications", "Medical Practices");
            medicalPracticeTriplet = new MedicalPracticeTriplet();
            medicalPracticeTriplet.SearchCriteria.SearchByName = medicalPracticeName;
            medicalPracticeResults = medicalPracticeTriplet.SearchCriteria.Search();
            medicalPracticeTile = medicalPracticeResults.SingleOrDefault(t => t.Name.Equals(medicalPracticeName));

            medicalPracticePage = medicalPracticeTile.Click<MedicalPracticePage>();
            var confirmDeleteDialog = medicalPracticePage.Delete();
            confirmDeleteDialog.ConfirmDelete();

            #endregion POS-CONDITIONS
        }

        /// <summary>
        /// Author: Huy.Vo
        /// Description: Record a new local medical practice so it is available for selection at the local school only.
        /// </summary>
        [WebDriverTest(TimeoutSeconds = 2400, Enabled = true, DataProvider = "TC_COM003_Data", Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { "g1", "All", "P2" })]
        public void TC_COM003_Update_Medical_Practice(string medicalPracticeName, string telephoneNumber, string doctorTitle, string doctorSureName, string doctorMiddleName, string doctorForeName,
                       string practiceNameUpdate, string telephoneNumberUpdate,
                       string doctorTitleAddMore, string doctorSureNameAddMore, string doctorMiddleNameAddMore, string doctorForeNameAddMore)
        {
            //Login as SchoolAdministrator and navigate to Medical Practices page
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);
            AutomationSugar.NavigateMenu("Tasks", "Communications", "Medical Practices");

            #region STEPS

            //Search and delete if Medical practice is existing
            var medicalPracticeTriplet = new MedicalPracticeTriplet();
            medicalPracticeTriplet.SearchCriteria.SearchByName = medicalPracticeName;
            var medicalPracticeResults = medicalPracticeTriplet.SearchCriteria.Search();
            var medicalPracticeTile = medicalPracticeResults.SingleOrDefault(t => t.Name.Equals(medicalPracticeName));
            medicalPracticeTriplet.SelectSearchTile(medicalPracticeTile);
            medicalPracticeTriplet.Delete();
            Console.WriteLine('1');
            //Create new Medical Practice 
            var medicalPracticePage = medicalPracticeTriplet.Create();
            medicalPracticePage.PracticeName = medicalPracticeName;
            //     var telephoneNumberTable = medicalPracticePage.TelephoneNumbers;
            //    telephoneNumberTable[0].TelephoneNumbers = telephoneNumber;

            //Add new Doctor's informations
            //     medicalPracticePage.ScrollToAssociated();
            var doctorDialog = medicalPracticePage.AddDoctor();
            doctorDialog.Title = doctorTitle;
            doctorDialog.ForeName = doctorForeName;
            doctorDialog.MiddleName = doctorMiddleName;
            doctorDialog.SureName = doctorSureName;
            doctorDialog.OK();
            medicalPracticePage.Save();
            Console.WriteLine('2');
            //Select and update Medical Practice
            medicalPracticeTriplet.SearchCriteria.SearchByName = medicalPracticeName;
            medicalPracticeTriplet.SearchCriteria.Search();
            medicalPracticeResults = medicalPracticeTriplet.SearchCriteria.Search();
            medicalPracticeTile = medicalPracticeResults.SingleOrDefault(t => t.Name.Equals(medicalPracticeName));
            medicalPracticePage = medicalPracticeTile.Click<MedicalPracticePage>();

            //Update Medical Practice Name
            medicalPracticePage.PracticeName = practiceNameUpdate;
            //      telephoneNumberTable = medicalPracticePage.TelephoneNumbers;
            //      telephoneNumberTable[0].TelephoneNumbers = telephoneNumberUpdate;
            medicalPracticePage.Save();
            Console.WriteLine('3');
            //Delete current Doctor on table
            var doctorTable = medicalPracticePage.Doctors;
            doctorTable.Rows.SingleOrDefault(x => x.Name == doctorTitle + " " + doctorForeName[0].ToString().ToUpper() + " " + doctorSureName).DeleteRow();
            medicalPracticePage.Save();
            //  Wait.WaitForAjaxReady(By.CssSelector(".locking-mask-loading"));
            //Add new Doctor
            doctorDialog = medicalPracticePage.AddDoctor();
            doctorDialog.Title = doctorTitleAddMore;
            doctorDialog.ForeName = doctorForeNameAddMore;
            doctorDialog.MiddleName = doctorMiddleNameAddMore;
            doctorDialog.SureName = doctorSureNameAddMore;
            doctorDialog.OK();
            medicalPracticePage.Save();

            //Verify that save message succesfully display
            Assert.AreEqual(true, medicalPracticePage.IsSuccessMessageIsDisplayed(), "Create new Medical practic unsuccessfully");

            //Re-search new Medical Practice to verify create new successfully
            medicalPracticeTriplet.SearchCriteria.SearchByName = practiceNameUpdate;
            medicalPracticeTriplet.SearchCriteria.Search();
            medicalPracticeResults = medicalPracticeTriplet.SearchCriteria.Search();
            var medicalPracticeUpdateTile = medicalPracticeResults.SingleOrDefault(t => t.Name.Equals(practiceNameUpdate));
            medicalPracticePage = medicalPracticeUpdateTile.Click<MedicalPracticePage>();
            Wait.WaitForDocumentReady();
            //Verify that new informations of Medical practice update are displayed on screen
            Assert.AreEqual(practiceNameUpdate, medicalPracticePage.PracticeName, "Medical Practice nam is not exist");
            //      telephoneNumberTable = medicalPracticePage.TelephoneNumbers;
            //      Assert.AreNotEqual(null, (telephoneNumberTable.Rows.SingleOrDefault(x => x.TelephoneNumbers == telephoneNumberUpdate)), "Update Phone numbers was failed");
            doctorTable = medicalPracticePage.Doctors;
            Assert.AreNotEqual(null, (doctorTable.Rows.SingleOrDefault(x => x.Name == doctorTitleAddMore + " " + doctorForeNameAddMore[0].ToString().ToUpper() + " " + doctorSureNameAddMore)), "Update Doctors was failed");

            #endregion

            #region POS-CONDITIONS

            // Delete Medical Practice
            var confirmDeleteDialog = medicalPracticePage.Delete();
            confirmDeleteDialog.ConfirmDelete();

            #endregion POS-CONDITIONS
        }

        #region Doctor Address Tests

        [WebDriverTest(TimeoutSeconds = 2400, Enabled = true, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { "Add", "DoctorAddress", "Add_Doctor_Address_Local", "All", "P2" })]
        [Variant(Variant.NorthernIrelandStatePrimary | Variant.NorthernIrelandStateSecondary | Variant.NorthernIrelandStateMultiphase |
            Variant.IndependentPrimary | Variant.IndependentSecondary | Variant.IndependentMultiphase)]
        public void Add_Doctor_Address_Local()
        {
            #region Arrange

            Guid medicalPracticeID, doctorID, addressID;
            string practiceName = CoreQueries.GetColumnUniqueString("MedicalPractice", "Name", 10, SeSugar.Environment.Settings.TenantId);
            string doctorName = CoreQueries.GetColumnUniqueString("Doctor", "Surname", 10, SeSugar.Environment.Settings.TenantId);

            string UPRN = Utilities.GenerateRandomString(6);
            string PAONDescription = Utilities.GenerateRandomString(6);
            string PAONRange = Utilities.GenerateRandomString(6);
            string SAON = Utilities.GenerateRandomString(6);
            string Street = Utilities.GenerateRandomString(6);
            string Locality = Utilities.GenerateRandomString(6);
            string Town = Utilities.GenerateRandomString(6);
            string AdministrativeArea = Utilities.GenerateRandomString(6);
            string PostCode = Utilities.GenerateRandomString(6);
            string Country = "United Kingdom";

            const string _space = " ";
            const string _seperator = ", ";
            const string _lineSeperator = "\r\n";

            var addressDisplaySmall = string.Concat(SAON, _seperator,
                PAONDescription, _seperator,
                PAONRange, _seperator, Street, _seperator, Town);

            var addressDisplayLarge = string.Concat(SAON, _seperator,
                PAONDescription, _lineSeperator,
                PAONRange, _space, Street, _lineSeperator,
                Locality, _lineSeperator,
                Town, _lineSeperator,
                AdministrativeArea, _lineSeperator,
                PostCode, _lineSeperator,
                Country);

            var packages = new DataPackage[]
            {
                GetMedicalPractice(out medicalPracticeID, out doctorID, practiceName, doctorName),
                GetAddress(out addressID, UPRN, PAONDescription, PAONRange, SAON, Street, Locality, Town, AdministrativeArea, PostCode, Country)
            };

            string buildingNo = Utilities.GenerateRandomString(6);
            string street = Utilities.GenerateRandomString(6);
            string district = Utilities.GenerateRandomString(6);
            string city = Utilities.GenerateRandomString(6);
            string county = Utilities.GenerateRandomString(6);
            string postCode = Utilities.GenerateRandomString(6);
            string countryPostCode = "BT52 1JB";
            #endregion

            using (new DataSetup(packages))
            {
                SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator, enabledFeatures: "Addresses");
                AutomationSugar.NavigateMenu("Tasks", "Communications", "Medical Practices");

                MedicalPracticeTriplet medicalPracticeTriplet = new MedicalPracticeTriplet();
                medicalPracticeTriplet.SearchCriteria.SearchByName = practiceName;
                var medicalPracticeResults = medicalPracticeTriplet.SearchCriteria.Search();
                var medicalPracticeTile = medicalPracticeResults.SingleOrDefault(t => t.Name.Equals(practiceName));
                MedicalPracticePage medicalPracticePage = medicalPracticeTile.Click<MedicalPracticePage>();
                Wait.WaitForDocumentReady();

                var gridRow = medicalPracticePage.Doctors.Rows[0];
                gridRow.ClickEdit();

                var doctorDialog = new DoctorsDialog();
                doctorDialog.SelectAddressesTab();
                doctorDialog.ClickAddanAdditionalAddressLink();

                var addressDialog = new POM.Components.Communication.AddDoctorAddressDialog();
                addressDialog.ClickManualAddAddress();
                addressDialog.BuildingNo = buildingNo;
                addressDialog.Street = street;
                addressDialog.District = district;
                addressDialog.City = city;
                addressDialog.County = county;
                addressDialog.PostCode = postCode;
                addressDialog.CountryPostCode = countryPostCode;
                addressDialog.ClickOk();
                AutomationSugar.Log("Created a new address to the pupil contact");

                doctorDialog.OK();
                medicalPracticePage.Save();

                medicalPracticePage = new MedicalPracticePage();
                gridRow = medicalPracticePage.Doctors.Rows[0];
                gridRow.ClickEdit();

                doctorDialog = new DoctorsDialog();
                doctorDialog.SelectAddressesTab();

                var gridRow2 = doctorDialog.DoctorAddressTable.Rows[0];
                Assert.AreEqual(addressDisplayLarge, gridRow2.Address);
            }
        }

        [WebDriverTest(TimeoutSeconds = 2400, Enabled = true, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { "Add", "DoctorAddress", "Add_Doctor_Address_WAV", "All", "P2" })]
        [Variant(Variant.EnglishStatePrimary | Variant.EnglishStateSecondary | Variant.EnglishStateMultiphase |
            Variant.WelshStatePrimary | Variant.WelshStateSecondary | Variant.WelshStateMultiphase)]
        public void Add_Doctor_Address_WAV()
        {
            #region Arrange

            Guid medicalPracticeID, doctorID;
            string practiceName = CoreQueries.GetColumnUniqueString("MedicalPractice", "Name", 10, SeSugar.Environment.Settings.TenantId);
            string doctorName = CoreQueries.GetColumnUniqueString("Doctor", "Surname", 10, SeSugar.Environment.Settings.TenantId);

            string PAONRange = "22";
            string Street = "SUDELEY WALK";
            string Town = "BEDFORD";
            string PostCode = "MK41 8HS";
            string Country = "United Kingdom";

            const string _space = " ";
            const string _seperator = ", ";
            const string _lineSeperator = "\r\n";

            var addressDisplaySmall = string.Concat(
                PAONRange, _seperator, Street, _seperator, Town);

            var addressDisplayLarge = string.Concat(
                PAONRange, _space, Street, _lineSeperator,
                Town, _lineSeperator,
                Town, _lineSeperator,
                PostCode, _lineSeperator,
                Country);

            var packages = new DataPackage[]
            {
                GetMedicalPractice(out medicalPracticeID, out doctorID, practiceName, doctorName)
            };

            #endregion

            using (new DataSetup(packages))
            {
                SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator, enabledFeatures: "Addresses");
                AutomationSugar.NavigateMenu("Tasks", "Communications", "Medical Practices");

                MedicalPracticeTriplet medicalPracticeTriplet = new MedicalPracticeTriplet();
                medicalPracticeTriplet.SearchCriteria.SearchByName = practiceName;
                var medicalPracticeResults = medicalPracticeTriplet.SearchCriteria.Search();
                var medicalPracticeTile = medicalPracticeResults.SingleOrDefault(t => t.Name.Equals(practiceName));
                MedicalPracticePage medicalPracticePage = medicalPracticeTile.Click<MedicalPracticePage>();
                Wait.WaitForDocumentReady();

                var gridRow = medicalPracticePage.Doctors.Rows[0];
                gridRow.ClickEdit();

                var doctorDialog = new DoctorsDialog();
                doctorDialog.SelectAddressesTab();
                doctorDialog.ClickAddAddress();

                var addressDialog = new POM.Components.Communication.AddAddressDialog();
                addressDialog.PAONRangeSearch = PAONRange;
                addressDialog.PostCodeSearch = PostCode;
                addressDialog.ClickSearch();

                addressDialog.Addresses = addressDisplaySmall;

                Assert.AreEqual(PAONRange, addressDialog.BuildingNo);
                Assert.AreEqual(Street, addressDialog.Street);
                Assert.AreEqual(Town, addressDialog.Town);
                Assert.AreEqual(Town, addressDialog.County);
                Assert.AreEqual(PostCode, addressDialog.PostCode);

                addressDialog.ClickOk();
                doctorDialog.OK();
                medicalPracticePage.Save();

                medicalPracticePage = new MedicalPracticePage();
                gridRow = medicalPracticePage.Doctors.Rows[0];
                gridRow.ClickEdit();

                doctorDialog = new DoctorsDialog();
                doctorDialog.SelectAddressesTab();

                var gridRow2 = doctorDialog.DoctorAddressTable.Rows[0];
                Assert.AreEqual(addressDisplayLarge, gridRow2.Address);
            }
        }

        [WebDriverTest(TimeoutSeconds = 2400, Enabled = true, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { "Edit", "DoctorAddress", "Edit_Doctor_Address_Fields", "All", "P2" })]
        [Variant(Variant.NorthernIrelandStatePrimary | Variant.NorthernIrelandStateSecondary | Variant.NorthernIrelandStateMultiphase |
            Variant.IndependentPrimary | Variant.IndependentSecondary | Variant.IndependentMultiphase |
            Variant.EnglishStatePrimary | Variant.EnglishStateSecondary | Variant.EnglishStateMultiphase |
            Variant.WelshStatePrimary | Variant.WelshStateSecondary | Variant.WelshStateMultiphase)]
        public void Edit_Doctor_Address_Fields()
        {
            #region Arrange

            Guid medicalPracticeID, doctorID, addressID, doctorAddressID;
            string practiceName = CoreQueries.GetColumnUniqueString("MedicalPractice", "Name", 10, SeSugar.Environment.Settings.TenantId);
            string doctorName = CoreQueries.GetColumnUniqueString("Doctor", "Surname", 10, SeSugar.Environment.Settings.TenantId);

            string UPRN = Utilities.GenerateRandomString(6);
            string PAONDescription = Utilities.GenerateRandomString(6);
            string PAONRange = Utilities.GenerateRandomString(6);
            string SAON = Utilities.GenerateRandomString(6);
            string Street = Utilities.GenerateRandomString(6);
            string Locality = Utilities.GenerateRandomString(6);
            string Town = Utilities.GenerateRandomString(6);
            string AdministrativeArea = Utilities.GenerateRandomString(6);
            string PostCode = Utilities.GenerateRandomString(6);
            string Country = "United Kingdom";

            string newLocality = Utilities.GenerateRandomString(6);

            var packages = new DataPackage[]
            {
                GetMedicalPractice(out medicalPracticeID, out doctorID, practiceName, doctorName),
                GetAddress(out addressID, UPRN, PAONDescription, PAONRange, SAON, Street, Locality, Town, AdministrativeArea, PostCode, Country),
                GetDoctorAddress(out doctorAddressID, doctorID, DateTime.Today, "H", addressID)
            };

            #endregion

            using (new DataSetup(packages))
            {
                SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator, enabledFeatures: "Addresses");
                AutomationSugar.NavigateMenu("Tasks", "Communications", "Medical Practices");

                MedicalPracticeTriplet medicalPracticeTriplet = new MedicalPracticeTriplet();
                medicalPracticeTriplet.SearchCriteria.SearchByName = practiceName;
                var medicalPracticeResults = medicalPracticeTriplet.SearchCriteria.Search();
                var medicalPracticeTile = medicalPracticeResults.SingleOrDefault(t => t.Name.Equals(practiceName));
                MedicalPracticePage medicalPracticePage = medicalPracticeTile.Click<MedicalPracticePage>();
                Wait.WaitForDocumentReady();

                var gridRow = medicalPracticePage.Doctors.Rows[0];
                gridRow.ClickEdit();

                var doctorDialog = new DoctorsDialog();
                doctorDialog.SelectAddressesTab();

                var gridRow2 = doctorDialog.DoctorAddressTable.Rows[0];
                gridRow2.Edit();

                var addressDialog = new POM.Components.Communication.AddAddressDialog();
                addressDialog.District = newLocality;
                addressDialog.ClickOk();
                doctorDialog.OK();
                medicalPracticePage.Save();

                medicalPracticePage = new MedicalPracticePage();
                gridRow = medicalPracticePage.Doctors.Rows[0];
                gridRow.ClickEdit();

                doctorDialog = new DoctorsDialog();
                doctorDialog.SelectAddressesTab();

                gridRow2 = doctorDialog.DoctorAddressTable.Rows[0];
                gridRow2.Edit();

                addressDialog = new POM.Components.Communication.AddAddressDialog();
                Assert.AreEqual(newLocality, addressDialog.District);
            }
        }

        [WebDriverTest(TimeoutSeconds = 2400, Enabled = true, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { "Edit", "DoctorAddress", "Edit_Doctor_Address_New_Address", "All", "P2" })]
        [Variant(Variant.EnglishStatePrimary | Variant.EnglishStateSecondary | Variant.EnglishStateMultiphase |
            Variant.WelshStatePrimary | Variant.WelshStateSecondary | Variant.WelshStateMultiphase)]
        public void Edit_Doctor_Address_New_Address()
        {
            #region Arrange

            Guid medicalPracticeID, doctorID, addressID, doctorAddressID;
            string practiceName = CoreQueries.GetColumnUniqueString("MedicalPractice", "Name", 10, SeSugar.Environment.Settings.TenantId);
            string doctorName = CoreQueries.GetColumnUniqueString("Doctor", "Surname", 10, SeSugar.Environment.Settings.TenantId);

            string UPRN = Utilities.GenerateRandomString(6);
            string PAONDescription = Utilities.GenerateRandomString(6);
            string PAONRange = Utilities.GenerateRandomString(6);
            string SAON = Utilities.GenerateRandomString(6);
            string Street = Utilities.GenerateRandomString(6);
            string Locality = Utilities.GenerateRandomString(6);
            string Town = Utilities.GenerateRandomString(6);
            string AdministrativeArea = Utilities.GenerateRandomString(6);
            string PostCode = Utilities.GenerateRandomString(6);
            string Country = "United Kingdom";

            string WAVPAONRange = "22";
            string WAVStreet = "SUDELEY WALK";
            string WAVTown = "BEDFORD";
            string WAVPostCode = "MK41 8HS";

            var packages = new DataPackage[]
            {
                GetMedicalPractice(out medicalPracticeID, out doctorID, practiceName, doctorName),
                GetAddress(out addressID, UPRN, PAONDescription, PAONRange, SAON, Street, Locality, Town, AdministrativeArea, PostCode, Country),
                GetDoctorAddress(out doctorAddressID, doctorID, DateTime.Today, "H", addressID)
            };

            #endregion

            using (new DataSetup(packages))
            {
                SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator, enabledFeatures: "Addresses");
                AutomationSugar.NavigateMenu("Tasks", "Communications", "Medical Practices");

                MedicalPracticeTriplet medicalPracticeTriplet = new MedicalPracticeTriplet();
                medicalPracticeTriplet.SearchCriteria.SearchByName = practiceName;
                var medicalPracticeResults = medicalPracticeTriplet.SearchCriteria.Search();
                var medicalPracticeTile = medicalPracticeResults.SingleOrDefault(t => t.Name.Equals(practiceName));
                MedicalPracticePage medicalPracticePage = medicalPracticeTile.Click<MedicalPracticePage>();
                Wait.WaitForDocumentReady();

                var gridRow = medicalPracticePage.Doctors.Rows[0];
                gridRow.ClickEdit();

                var doctorDialog = new DoctorsDialog();
                doctorDialog.SelectAddressesTab();

                var gridRow2 = doctorDialog.DoctorAddressTable.Rows[0];
                gridRow2.Edit();

                var addressDialog = new POM.Components.Communication.AddAddressDialog();
                addressDialog.PAONRangeSearch = WAVPAONRange;
                addressDialog.PostCodeSearch = WAVPostCode;
                addressDialog.ClickSearch();

                addressDialog.ClickOk();
                doctorDialog.OK();
                medicalPracticePage.Save();

                medicalPracticePage = new MedicalPracticePage();
                gridRow = medicalPracticePage.Doctors.Rows[0];
                gridRow.ClickEdit();

                doctorDialog = new DoctorsDialog();
                doctorDialog.SelectAddressesTab();

                gridRow2 = doctorDialog.DoctorAddressTable.Rows[0];
                gridRow2.Edit();

                addressDialog = new POM.Components.Communication.AddAddressDialog();

                Assert.AreEqual(WAVPAONRange, addressDialog.BuildingNo);
                Assert.AreEqual(WAVStreet, addressDialog.Street);
                Assert.AreEqual(WAVTown, addressDialog.Town);
                Assert.AreEqual(WAVPostCode, addressDialog.PostCode);
            }
        }

        [WebDriverTest(TimeoutSeconds = 2400, Enabled = true, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { "Delete", "DoctorAddress", "Delete_Doctor_Address", "All", "P2" })]
        [Variant(Variant.NorthernIrelandStatePrimary | Variant.NorthernIrelandStateSecondary | Variant.NorthernIrelandStateMultiphase |
            Variant.IndependentPrimary | Variant.IndependentSecondary | Variant.IndependentMultiphase |
            Variant.EnglishStatePrimary | Variant.EnglishStateSecondary | Variant.EnglishStateMultiphase |
            Variant.WelshStatePrimary | Variant.WelshStateSecondary | Variant.WelshStateMultiphase)]
        public void Delete_Doctor_Address()
        {
            #region Arrange

            Guid medicalPracticeID, doctorID, addressID, doctorAddressID;
            string practiceName = CoreQueries.GetColumnUniqueString("MedicalPractice", "Name", 10, SeSugar.Environment.Settings.TenantId);
            string doctorName = CoreQueries.GetColumnUniqueString("Doctor", "Surname", 10, SeSugar.Environment.Settings.TenantId);

            string UPRN = Utilities.GenerateRandomString(6);
            string PAONDescription = Utilities.GenerateRandomString(6);
            string PAONRange = Utilities.GenerateRandomString(6);
            string SAON = Utilities.GenerateRandomString(6);
            string Street = Utilities.GenerateRandomString(6);
            string Locality = Utilities.GenerateRandomString(6);
            string Town = Utilities.GenerateRandomString(6);
            string AdministrativeArea = Utilities.GenerateRandomString(6);
            string PostCode = Utilities.GenerateRandomString(6);
            string Country = "United Kingdom";

            var packages = new DataPackage[]
            {
                GetMedicalPractice(out medicalPracticeID, out doctorID, practiceName, doctorName),
                GetAddress(out addressID, UPRN, PAONDescription, PAONRange, SAON, Street, Locality, Town, AdministrativeArea, PostCode, Country),
                GetDoctorAddress(out doctorAddressID, doctorID, DateTime.Today, "H", addressID)
            };

            #endregion

            using (new DataSetup(packages))
            {
                SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator, enabledFeatures: "Addresses");
                AutomationSugar.NavigateMenu("Tasks", "Communications", "Medical Practices");

                MedicalPracticeTriplet medicalPracticeTriplet = new MedicalPracticeTriplet();
                medicalPracticeTriplet.SearchCriteria.SearchByName = practiceName;
                var medicalPracticeResults = medicalPracticeTriplet.SearchCriteria.Search();
                var medicalPracticeTile = medicalPracticeResults.SingleOrDefault(t => t.Name.Equals(practiceName));
                MedicalPracticePage medicalPracticePage = medicalPracticeTile.Click<MedicalPracticePage>();
                Wait.WaitForDocumentReady();

                var gridRow = medicalPracticePage.Doctors.Rows[0];
                gridRow.ClickEdit();

                var doctorDialog = new DoctorsDialog();
                doctorDialog.SelectAddressesTab();

                var gridRow2 = doctorDialog.DoctorAddressTable.Rows[0];
                gridRow2.DeleteRow();

                doctorDialog.OK();
                medicalPracticePage.Save();

                medicalPracticePage = new MedicalPracticePage();
                gridRow = medicalPracticePage.Doctors.Rows[0];
                gridRow.ClickEdit();

                doctorDialog = new DoctorsDialog();
                doctorDialog.SelectAddressesTab();

                int count = doctorDialog.DoctorAddressTable.Rows.Count;

                Assert.AreEqual(0, count);
            }
        }

        [WebDriverTest(TimeoutSeconds = 2400, Enabled = true, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { "Move", "DoctorAddress", "Move_Doctor_Address", "All", "P2" })]
        [Variant(Variant.EnglishStatePrimary | Variant.EnglishStateSecondary | Variant.EnglishStateMultiphase |
             Variant.WelshStatePrimary | Variant.WelshStateSecondary | Variant.WelshStateMultiphase)]
        public void Move_Doctor_Address()
        {
            #region Arrange

            Guid medicalPracticeID, doctorID, addressID, doctorAddressID;
            string practiceName = CoreQueries.GetColumnUniqueString("MedicalPractice", "Name", 10, SeSugar.Environment.Settings.TenantId);
            string doctorName = CoreQueries.GetColumnUniqueString("Doctor", "Surname", 10, SeSugar.Environment.Settings.TenantId);

            string UPRN = Utilities.GenerateRandomString(6);
            string PAONDescription = Utilities.GenerateRandomString(6);
            string PAONRange = Utilities.GenerateRandomString(6);
            string SAON = Utilities.GenerateRandomString(6);
            string Street = Utilities.GenerateRandomString(6);
            string Locality = Utilities.GenerateRandomString(6);
            string Town = Utilities.GenerateRandomString(6);
            string AdministrativeArea = Utilities.GenerateRandomString(6);
            string PostCode = Utilities.GenerateRandomString(6);
            string Country = "United Kingdom";

            string WAVPAONRange = "22";
            string WAVPostCode = "MK41 8HS";

            DateTime moveDate = DateTime.Today.AddDays(5);

            var packages = new DataPackage[]
            {
                GetMedicalPractice(out medicalPracticeID, out doctorID, practiceName, doctorName),
                GetAddress(out addressID, UPRN, PAONDescription, PAONRange, SAON, Street, Locality, Town, AdministrativeArea, PostCode, Country),
                GetDoctorAddress(out doctorAddressID, doctorID, DateTime.Today, "H", addressID)
            };

            #endregion

            using (new DataSetup(packages))
            {
                SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator, enabledFeatures: "Addresses");
                AutomationSugar.NavigateMenu("Tasks", "Communications", "Medical Practices");

                MedicalPracticeTriplet medicalPracticeTriplet = new MedicalPracticeTriplet();
                medicalPracticeTriplet.SearchCriteria.SearchByName = practiceName;
                var medicalPracticeResults = medicalPracticeTriplet.SearchCriteria.Search();
                var medicalPracticeTile = medicalPracticeResults.SingleOrDefault(t => t.Name.Equals(practiceName));
                MedicalPracticePage medicalPracticePage = medicalPracticeTile.Click<MedicalPracticePage>();
                Wait.WaitForDocumentReady();

                var gridRow = medicalPracticePage.Doctors.Rows[0];
                gridRow.ClickEdit();

                var doctorDialog = new DoctorsDialog();
                doctorDialog.SelectAddressesTab();

                var gridRow2 = doctorDialog.DoctorAddressTable.Rows[0];
                gridRow2.Move();

                var addressDialog = new POM.Components.Communication.AddAddressDialog();
                addressDialog.MoveDate = moveDate.ToShortDateString();
                addressDialog.PAONRangeSearch = WAVPAONRange;
                addressDialog.PostCodeSearch = WAVPostCode;
                addressDialog.ClickSearch();

                addressDialog.ClickOk();
                doctorDialog.OK();
                medicalPracticePage.Save();

                medicalPracticePage = new MedicalPracticePage();
                gridRow = medicalPracticePage.Doctors.Rows[0];
                gridRow.ClickEdit();

                doctorDialog = new DoctorsDialog();
                doctorDialog.SelectAddressesTab();

                gridRow2 = doctorDialog.DoctorAddressTable.Rows[0];
                var gridRow3 = doctorDialog.DoctorAddressTable.Rows[1];

                Assert.AreEqual(moveDate.AddDays(-1).ToShortDateString(), gridRow2.EndDate);
                Assert.AreEqual(moveDate.ToShortDateString(), gridRow3.StartDate);
            }
        }

        private DataPackage GetMedicalPractice(out Guid medicalPracticeID, out Guid doctorID, string medicalPracticeName, string doctorName)
        {
            return this.BuildDataPackage()
               .AddData("MedicalPractice", new
               {
                   Id = medicalPracticeID = Guid.NewGuid(),
                   Name = medicalPracticeName,
                   ResourceProvider = CoreQueries.GetSchoolId(),
                   TenantID = SeSugar.Environment.Settings.TenantId
               })
               .AddData("Doctor", new
               {
                   Id = doctorID = Guid.NewGuid(),
                   Surname = doctorName,
                   MedicalPractice = medicalPracticeID,
                   ResourceProvider = CoreQueries.GetSchoolId(),
                   TenantID = SeSugar.Environment.Settings.TenantId
               });
        }

        private DataPackage GetAgency(out Guid agencyID, string agencyName)
        {
            return this.BuildDataPackage()
               .AddData("Agency", new
               {
                   Id = agencyID = Guid.NewGuid(),
                   AgencyName = agencyName,
                   ResourceProvider = CoreQueries.GetSchoolId(),
                   TenantID = SeSugar.Environment.Settings.TenantId
               });
        }
        private DataPackage GetAgent(out Guid agentID, string agentForeName, string agentSurName)
        {
            return this.BuildDataPackage()
               .AddData("Agent", new
               {
                   Id = agentID = Guid.NewGuid(),
                   LegalForename = agentForeName,
                   LegalSurname = agentSurName,
                   ResourceProvider = CoreQueries.GetSchoolId(),
                   TenantID = SeSugar.Environment.Settings.TenantId
               });
        }

        private DataPackage GetAddress(out Guid addressID, string UPRN,
                    string PAONDescription,
                    string PAONRange,
                    string SAON,
                    string Street,
                    string Locality,
                    string Town,
                    string AdministrativeArea,
                    string PostCode,
                    string Country)
        {
            return this.BuildDataPackage()
                .AddData("Address", new
                {
                    ID = addressID = Guid.NewGuid(),
                    UPRN = UPRN,
                    PAONDescription = PAONDescription,
                    PAONRange = PAONRange,
                    SAON = SAON,
                    Street = Street,
                    Locality = Locality,
                    Town = Town,
                    AdministrativeArea = AdministrativeArea,
                    PostCode = PostCode,
                    Country = CoreQueries.GetLookupItem("Country", description: Country),
                    ResourceProvider = CoreQueries.GetSchoolId(),
                    TenantID = SeSugar.Environment.Settings.TenantId
                });
        }

        private DataPackage GetDoctorAddress(out Guid doctorAddressID, Guid doctorID, DateTime startDate, string addressTypeCode, Guid address, DateTime? endDate = null)
        {
            return this.BuildDataPackage()
               .AddData("DoctorAddress", new
               {
                   Id = doctorAddressID = Guid.NewGuid(),
                   Doctor = doctorID,
                   StartDate = startDate,
                   EndDate = endDate,
                   AddressType = CoreQueries.GetLookupItem("AddressType", code: addressTypeCode),
                   Address = address,
                   TenantID = SeSugar.Environment.Settings.TenantId
               });
        }

        #endregion

        /// <summary>
        /// Author: Huy.Vo
        /// Description: Check that Edited Medical Practice details changes are pulled through to the Pupil record automatically
        /// </summary>
        [WebDriverTest(TimeoutSeconds = 3000, Enabled = true, DataProvider = "TC_COM004_Data", Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { "g1", "All", "P2" })]
        public void TC_COM004_Update_Medical_Practice_From_Pupils_Record(string medicalPracticeName, string telephoneNumber, string doctorTitle, string doctorSureName, string doctorMiddleName, string doctorForeName, string medicalPracticeUpdate)
        {

            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);

            AutomationSugar.Log("Data Creation started");
            Guid pupilId = Guid.NewGuid();
            DataPackage dataPackage = this.BuildDataPackage();
            var sureName = Utilities.GenerateRandomString(10, "TC04_Surname");
            var foreName = Utilities.GenerateRandomString(10, "TC04_Forename" + Thread.CurrentThread.ManagedThreadId);
            dataPackage.AddBasicLearner(pupilId, sureName, foreName, new DateTime(2005, 10, 01), new DateTime(2012, 08, 01));



            DataSetup DataSetPupil = new DataSetup(purgeBeforeInsert: false, purgeAfterTest: true, packages: dataPackage);
            AutomationSugar.NavigateMenu("Tasks", "Communications", "Medical Practices");

            #region PRE-CONDITIONS:

            //Search and delete if Medical practice is existing
            var medicalPracticeTriplet = new MedicalPracticeTriplet();
            medicalPracticeTriplet.SearchCriteria.SearchByName = medicalPracticeName;
            medicalPracticeTriplet.SearchCriteria.Search();
            var medicalPracticeResults = medicalPracticeTriplet.SearchCriteria.Search();
            var medicalPracticeTile = medicalPracticeResults.SingleOrDefault(t => t.Name.Equals(medicalPracticeName));
            medicalPracticeTriplet.SelectSearchTile(medicalPracticeTile);
            medicalPracticeTriplet.Delete();

            //Create new Medical Practice 
            var medicalPracticePage = medicalPracticeTriplet.Create();
            medicalPracticePage.PracticeName = medicalPracticeName;
            //         var telephoneNumberTable = medicalPracticePage.TelephoneNumbers;
            //        telephoneNumberTable[0].TelephoneNumbers = telephoneNumber;

            //Add new Doctor's informations
            //    medicalPracticePage.ScrollToAssociated();
            var doctorDialog = medicalPracticePage.AddDoctor();
            doctorDialog.Title = doctorTitle;
            doctorDialog.ForeName = doctorForeName;
            doctorDialog.MiddleName = doctorMiddleName;
            doctorDialog.SureName = doctorSureName;
            doctorDialog.OK();
            medicalPracticePage.Save();

            #endregion PRE-CONDITIONS

            #region STEPS
            //   Wait.WaitForAjaxReady(By.CssSelector(".locking-mask-loading"));
            SeleniumHelper.NavigateQuickLink("Pupil Records");
            var pupilRecordsTriplet = new PupilRecordTriplet();

            //Select an existing pupil to update Medical
            pupilRecordsTriplet.SearchCriteria.PupilName = sureName;
            pupilRecordsTriplet.SearchCriteria.IsCurrent = true;
            var pupilsearchresults = pupilRecordsTriplet.SearchCriteria.Search();
            var pupilTile = pupilsearchresults.FirstOrDefault(t => t.Name.Equals(String.Format("{0}, {1}", sureName, foreName)));
            var pupilRecordPage = pupilTile.Click<PupilRecordPage>();

            //Add new Medical Practice
            pupilRecordPage.SelectMedicalTab();
            var medicalPracticeDialogTriplet = pupilRecordPage.ClickMedicalPractice();
            medicalPracticeDialogTriplet.SearchCriteria.MedicalPracticeName = medicalPracticeName;
            var medicalPracticeDialogResults = medicalPracticeDialogTriplet.SearchCriteria.Search();
            var medicalPracticeDialogTile = medicalPracticeDialogResults.SingleOrDefault(t => t.Name.Equals(medicalPracticeName));
            var medicalPracticeDialogDetails = medicalPracticeDialogTile.Click<MedicalPracticeDialog>();
            medicalPracticeDialogTriplet.OK();

            //Save Pupil records
            pupilRecordPage.ClickSave();

            //Re-search Pupil to verify add Medical Practice update successufully
            pupilRecordsTriplet.SearchCriteria.PupilName = string.Format("{0}, {1}", sureName, foreName);
            pupilRecordsTriplet.SearchCriteria.IsCurrent = true;
            pupilsearchresults = pupilRecordsTriplet.SearchCriteria.Search();
            pupilTile = pupilsearchresults.FirstOrDefault(t => t.Name.Equals(String.Format("{0}, {1}", sureName, foreName)));
            pupilRecordPage = pupilTile.Click<PupilRecordPage>();

            //Add new Medical Practice
            //pupilRecordPage.SelectMedicalTab();
            var medicalPracticeTable = pupilRecordPage.MedicalPractice;
            var editMedicalPracticeDialog = medicalPracticeTable.Rows.FirstOrDefault(x => x.Name.Equals(medicalPracticeName)).ClickEdit();
            editMedicalPracticeDialog.PracticeName = medicalPracticeUpdate;
            pupilRecordPage = editMedicalPracticeDialog.OK();
            pupilRecordPage.ClickSave();

            //Re-search Pupil to verify verify Medical Practice update successufully
            pupilRecordsTriplet.SearchCriteria.PupilName = string.Format("{0}, {1}", sureName, foreName);
            pupilRecordsTriplet.SearchCriteria.IsCurrent = true;
            pupilsearchresults = pupilRecordsTriplet.SearchCriteria.Search();
            pupilTile = pupilsearchresults.FirstOrDefault(t => t.Name.Equals(String.Format("{0}, {1}", sureName, foreName)));
            pupilRecordPage = pupilTile.Click<PupilRecordPage>();
            Wait.WaitForDocumentReady();
            //Verify that add new Medical Practice is displayed on table
            //pupilRecordPage.SelectMedicalTab();
            medicalPracticeTable = pupilRecordPage.MedicalPractice;
            Assert.AreNotEqual(null, medicalPracticeTable.Rows.FirstOrDefault(x => x.Name.Equals(medicalPracticeUpdate)), "Update medical practice was failed");

            //Delete Medical Practice update out of Pupil records
            medicalPracticeTable.Rows.FirstOrDefault(x => x.Name.Equals(medicalPracticeUpdate)).DeleteRow();
            pupilRecordPage.ClickSave();

            //Re-select Practice Name to verify that update successfully
            AutomationSugar.NavigateMenu("Tasks", "Communications", "Medical Practices");
            var medicalPracticeUpdateTriplet = new MedicalPracticeTriplet();
            medicalPracticeUpdateTriplet.SearchCriteria.SearchByName = medicalPracticeUpdate;
            var medicalPracticeUpdateResults = medicalPracticeUpdateTriplet.SearchCriteria.Search();
            var medicalPracticeUpdateTile = medicalPracticeUpdateResults.SingleOrDefault(t => t.Name.Equals(medicalPracticeUpdate));

            //Verify that update Medical Practic successfully
            Assert.AreNotEqual(null, medicalPracticeUpdateTile);
            medicalPracticePage = medicalPracticeUpdateTile.Click<MedicalPracticePage>();

            Assert.AreEqual(medicalPracticeUpdate, medicalPracticePage.PracticeName, "Update medical practice was failed");

            #endregion STEPS

            #region POS-CONDITIONS

            //Re-select Practice Name to Delete
            var confirmDeleteDialog = medicalPracticePage.Delete();
            confirmDeleteDialog.ConfirmDelete();

            #endregion POS-CONDITIONS
        }

        /// <summary>
        /// Author: Huy.Vo
        /// Description: Try to delete the local medical practice created previously, which should be  prevented as it is linked to a Pupil.
        /// </summary>
        [WebDriverTest(TimeoutSeconds = 2400, Enabled = true, DataProvider = "TC_COM005_Data", Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { "g1", "All", "P2" })]
        public void TC_COM005_Delete_Medical_Practice(string medicalPracticeName, string telephoneNumber, string doctorTitle, string doctorSureName, string doctorMiddleName, string doctorForeName)
        {

            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);


            AutomationSugar.Log("Data Creation started");
            Guid pupilId = Guid.NewGuid();
            DataPackage dataPackage = this.BuildDataPackage();
            var pupilSureName = Utilities.GenerateRandomString(10, "TC05_Surname");
            var pupilForeName = Utilities.GenerateRandomString(10, "TC05_Forename" + Thread.CurrentThread.ManagedThreadId);
            dataPackage.AddBasicLearner(pupilId, pupilSureName, pupilForeName, new DateTime(2005, 10, 01), new DateTime(2012, 08, 01));
            DataSetup DataSetPupil = new DataSetup(purgeBeforeInsert: false, purgeAfterTest: true, packages: dataPackage);


            AutomationSugar.NavigateMenu("Tasks", "Communications", "Medical Practices");

            #region STEPS

            // Search and delete if Medical practice is existing
            var medicalPracticeTriplet = new MedicalPracticeTriplet();
            medicalPracticeTriplet.SearchCriteria.SearchByName = medicalPracticeName;
            var medicalPracticeResults = medicalPracticeTriplet.SearchCriteria.Search();
            var medicalPracticeTile = medicalPracticeResults.SingleOrDefault(t => t.Name.Equals(medicalPracticeName));
            medicalPracticeTriplet.SelectSearchTile(medicalPracticeTile);
            medicalPracticeTriplet.Delete();
            Console.WriteLine('1');
            // Create new Medical Practice 
            var medicalPracticePage = medicalPracticeTriplet.Create();
            medicalPracticePage.PracticeName = medicalPracticeName;
            //       var telephoneNumberTable = medicalPracticePage.TelephoneNumbers;
            //       telephoneNumberTable[0].TelephoneNumbers = telephoneNumber;

            // Add new Doctor's informations
            //  medicalPracticePage.ScrollToAssociated();
            var doctorDialog = medicalPracticePage.AddDoctor();
            doctorDialog.Title = doctorTitle;
            doctorDialog.ForeName = doctorForeName;
            doctorDialog.MiddleName = doctorMiddleName;
            doctorDialog.SureName = doctorSureName;
            doctorDialog.OK();
            medicalPracticePage.Save();
            Console.WriteLine('2');
            // Verify that save message succesfully display
            Assert.AreEqual(true, medicalPracticePage.IsSuccessMessageIsDisplayed(), "Create new Medical practic unsuccessfully");

            // Re-search new Medical Practice to verify create new successfully
            medicalPracticeTriplet.SearchCriteria.SearchByName = medicalPracticeName;
            medicalPracticeResults = medicalPracticeTriplet.SearchCriteria.Search();
            medicalPracticeTile = medicalPracticeResults.SingleOrDefault(t => t.Name.Equals(medicalPracticeName));
            medicalPracticePage = medicalPracticeTile.Click<MedicalPracticePage>();
            medicalPracticeTriplet.Delete();

            // Re-search new Medical Practice to verify create new successfully
            medicalPracticeTriplet.SearchCriteria.SearchByName = medicalPracticeName;
            medicalPracticeResults = medicalPracticeTriplet.SearchCriteria.Search();
            medicalPracticeTile = medicalPracticeResults.SingleOrDefault(t => t.Name.Equals(medicalPracticeName));
            Assert.AreEqual(null, medicalPracticeTile);
            Console.WriteLine('3');
            Wait.WaitForAjaxReady(By.CssSelector(".locking-mask-loading"));  // Line added to make the script stable. Incase of its absence, script fails randomly. --Shridhar.
            // Select Pupil record to verify that delete Medical Practice is effected
            SeleniumHelper.NavigateQuickLink("Pupil Records");
            var pupilRecordsTriplet = new PupilRecordTriplet();

            // Select an existing pupil to update Medical
            pupilRecordsTriplet.SearchCriteria.PupilName = pupilSureName;
            pupilRecordsTriplet.SearchCriteria.IsCurrent = true;
            var pupilsearchresults = pupilRecordsTriplet.SearchCriteria.Search();
            var pupilTile = pupilsearchresults.FirstOrDefault(t => t.Name.Equals(String.Format("{0}, {1}", pupilSureName, pupilForeName)));
            var pupilRecordPage = pupilTile.Click<PupilRecordPage>();
            pupilRecordPage.SelectMedicalTab();

            // Search Medical Practice was deleted
            var medicalPracticeDialogTriplet = pupilRecordPage.ClickMedicalPractice();
            medicalPracticeDialogTriplet.SearchCriteria.MedicalPracticeName = medicalPracticeName;
            var medicalPracticeDialogResults = medicalPracticeDialogTriplet.SearchCriteria.Search();
            var medicalPracticeDialogTile = medicalPracticeDialogResults.SingleOrDefault(t => t.Name.Equals(medicalPracticeName));
            Assert.AreEqual(null, medicalPracticeDialogTile, "Delelete Medical Practice is not effected");
            medicalPracticeDialogTriplet.ClickCancel();

            #endregion
        }


        /// <summary>
        /// Author: Huy.Vo
        /// Description: Create a new local medical practice from within the Pupil Record.
        /// </summary>
        [WebDriverTest(TimeoutSeconds = 2400, Enabled = true, DataProvider = "TC_COM006_Data", Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { "g1", "All", "P2" })]
        public void TC_COM006_Add_New_Medical_Practice_From_Pupils_Record(string medicalPracticeName,
            string telephoneNumber, string doctorTitle, string doctorSureName, string doctorMiddleName, string doctorForeName)
        {

            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);

            AutomationSugar.Log("Data Creation started");
            Guid pupilId = Guid.NewGuid();
            DataPackage dataPackage = this.BuildDataPackage();
            var pupilSureName = Utilities.GenerateRandomString(10, "TC06_Surname");
            var pupilForeName = Utilities.GenerateRandomString(10, "TC06_Forename" + Thread.CurrentThread.ManagedThreadId);
            dataPackage.AddBasicLearner(pupilId, pupilSureName, pupilForeName, new DateTime(2005, 10, 01), new DateTime(2012, 08, 01));
            DataSetup DataSetPupil = new DataSetup(purgeBeforeInsert: false, purgeAfterTest: true, packages: dataPackage);


            AutomationSugar.NavigateMenu("Tasks", "Communications", "Medical Practices");

            #region STEPS

            // Search and delete if Medical practice is existing
            var medicalPracticeTriplet = new MedicalPracticeTriplet();
            medicalPracticeTriplet.SearchCriteria.SearchByName = medicalPracticeName;
            var medicalPracticeResults = medicalPracticeTriplet.SearchCriteria.Search();
            var medicalPracticeTile = medicalPracticeResults.SingleOrDefault(t => t.Name.Equals(medicalPracticeName));
            medicalPracticeTriplet.SelectSearchTile(medicalPracticeTile);
            medicalPracticeTriplet.Delete();
            //  Wait.WaitForAjaxReady(By.CssSelector(".locking-mask-loading"));
            Console.WriteLine('1');
            // Select Pupil record to add new Medical practice

            SeleniumHelper.NavigateQuickLink("Pupil Records");
            var pupilRecordsTriplet = new PupilRecordTriplet();

            // Select an existing pupil to update Medical
            pupilRecordsTriplet.SearchCriteria.PupilName = pupilSureName;
            pupilRecordsTriplet.SearchCriteria.IsCurrent = true;
            var pupilsearchresults = pupilRecordsTriplet.SearchCriteria.Search();
            var pupilTile = pupilsearchresults.FirstOrDefault(t => t.Name.Equals(String.Format("{0}, {1}", pupilSureName, pupilForeName)));
            var pupilRecordPage = pupilTile.Click<PupilRecordPage>();
            pupilRecordPage.SelectMedicalTab();

            // Create new Medical Practice from Pupil Records
            var medicalPracticeDialogTriplet = pupilRecordPage.ClickMedicalPractice();
            medicalPracticeDialogTriplet.GetType();
            var medicalPracticeDialog = medicalPracticeDialogTriplet.Create();
            medicalPracticeDialog.PracticeName = medicalPracticeName;
            //       var telephoneNumbersTable = medicalPracticeDialog.TelephoneNumbers;
            //      telephoneNumbersTable[0].TelephoneNumbers = telephoneNumber;

            // Add Doctors from Dialog
            var addDoctorDialog = medicalPracticeDialog.AddDoctor();
            addDoctorDialog.Title = doctorTitle;
            addDoctorDialog.ForeName = doctorForeName;
            addDoctorDialog.MiddleName = doctorMiddleName;
            addDoctorDialog.SureName = doctorSureName;
            medicalPracticeDialog = addDoctorDialog.OK();
            Console.WriteLine('2');
            // Save from Medical Practice Dialog
            pupilRecordPage = medicalPracticeDialogTriplet.OK();

            pupilRecordPage.ClickSave();
            Console.WriteLine('3');
            //  Wait.WaitForAjaxReady(By.CssSelector(".locking-mask-loading"));
            // Verify that save message succesfully display
            //    bool val = pupilRecordPage.IsSuccessMessageDisplayed();
            //  Assert.AreEqual(true, val, "Create new Medical practice unsuccessfully");
            //   Wait.WaitForAjaxReady(By.CssSelector(".locking-mask-loading"));
            // Re-search new Medical Practice to verify create new successfully
            AutomationSugar.NavigateMenu("Tasks", "Communications", "Medical Practices");
            medicalPracticeTriplet = new MedicalPracticeTriplet();
            medicalPracticeTriplet.SearchCriteria.SearchByName = medicalPracticeName;
            medicalPracticeResults = medicalPracticeTriplet.SearchCriteria.Search();
            medicalPracticeTile = medicalPracticeResults.SingleOrDefault(t => t.Name.Equals(medicalPracticeName));
            var medicalPracticePage = medicalPracticeTile.Click<MedicalPracticePage>();
            Console.WriteLine('4');
            // Verify that new informations of Medical practice are displayed on screen
            Assert.AreEqual(medicalPracticeName, medicalPracticePage.PracticeName, "Medical Practice nam is not exist");
            //      var telephoneNumbers = medicalPracticePage.TelephoneNumbers;
            //     Assert.AreNotEqual(null, (telephoneNumbers.Rows.SingleOrDefault(x => x.TelephoneNumbers == telephoneNumber)), "Creating Medical Practice was failed");
            var doctorTable = medicalPracticePage.Doctors;
            Assert.AreNotEqual(null, (doctorTable.Rows.SingleOrDefault(x => x.Name == doctorTitle + " " + doctorForeName[0].ToString().ToUpper() + " " + doctorSureName)), "Creating Medical Practice was failed");

            #endregion

            #region POS-CONDITIONS

            // Delete Medical Practice
            var confirmDeleteDialog = medicalPracticePage.Delete();
            confirmDeleteDialog.ConfirmDelete();
            #endregion POS-CONDITIONS

        }

        /// <summary>
        /// Author: Huy.Vo
        /// Description: Select to amend a local medical practice currently selected on any pupil records via the Pupil Record.
        /// </summary>
        [WebDriverTest(TimeoutSeconds = 2400, Enabled = true, DataProvider = "TC_COM007_Data", Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { "g1", "All", "P2" })]
        public void TC_COM007_Update_Medical_Practice_From_Pupil_Records(string medicalPracticeName, string telephoneNumber,
            string doctorTitle, string doctorSureName, string doctorMiddleName, string doctorForeName,
           
            string medicalPracticeNameUpdate, string telephoneNumberUpdate, string doctorTitleUpdate, string doctorForeNameUpdate, string doctorMiddleNameUpdate, string doctorSureNameUpdate)
        {

            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);

            AutomationSugar.Log("Data Creation started");
            Guid pupilId = Guid.NewGuid();
            DataPackage dataPackage = this.BuildDataPackage();
            var pupilSureName = Utilities.GenerateRandomString(10, "TC07_Surname");
            var pupilForeName = Utilities.GenerateRandomString(10, "TC07_Forename" + Thread.CurrentThread.ManagedThreadId);
            dataPackage.AddBasicLearner(pupilId, pupilSureName, pupilForeName, new DateTime(2005, 10, 01), new DateTime(2012, 08, 01));
            DataSetup DataSetPupil = new DataSetup(purgeBeforeInsert: false, purgeAfterTest: true, packages: dataPackage);

            AutomationSugar.NavigateMenu("Tasks", "Communications", "Medical Practices");

            #region STEPS

            // Search and delete if Medical practice is existing
            var medicalPracticeTriplet = new MedicalPracticeTriplet();
            medicalPracticeTriplet.SearchCriteria.SearchByName = medicalPracticeName;
            medicalPracticeTriplet.SearchCriteria.Search();
            var medicalPracticeResults = medicalPracticeTriplet.SearchCriteria.Search();
            var medicalPracticeTile = medicalPracticeResults.SingleOrDefault(t => t.Name.Equals(medicalPracticeName));
            medicalPracticeTriplet.SelectSearchTile(medicalPracticeTile);
            medicalPracticeTriplet.Delete();

            // Create new Medical Practice 
            var medicalPracticePage = medicalPracticeTriplet.Create();
            medicalPracticePage.PracticeName = medicalPracticeName;
            //       var telephoneNumberTable = medicalPracticePage.TelephoneNumbers;
            //      telephoneNumberTable[0].TelephoneNumbers = telephoneNumber;

            // Add new Doctor's informations
            //  medicalPracticePage.ScrollToAssociated();
            var doctorDialog = medicalPracticePage.AddDoctor();
            doctorDialog.Title = doctorTitle;
            doctorDialog.ForeName = doctorForeName;
            doctorDialog.MiddleName = doctorMiddleName;
            doctorDialog.SureName = doctorSureName;
            doctorDialog.OK();
            medicalPracticePage.Save();
            Console.WriteLine('1');
            //  Wait.WaitForAjaxReady(By.CssSelector(".locking-mask-loading"));
            // Select Pupil record to add new Medical practice
            SeleniumHelper.NavigateQuickLink("Pupil Records");
            var pupilRecordsTriplet = new PupilRecordTriplet();

            // Select an existing pupil to update Medical
            pupilRecordsTriplet.SearchCriteria.PupilName = pupilSureName;
            pupilRecordsTriplet.SearchCriteria.IsCurrent = true;
            pupilRecordsTriplet.SearchCriteria.Search();
            var pupilsearchresults = pupilRecordsTriplet.SearchCriteria.Search();
            var pupilTile = pupilsearchresults.FirstOrDefault(t => t.Name.Equals(String.Format("{0}, {1}", pupilSureName, pupilForeName)));
            var pupilRecordPage = pupilTile.Click<PupilRecordPage>();
            pupilRecordPage.SelectMedicalTab();

            var medicalPracticeDialogTriplet = pupilRecordPage.ClickMedicalPractice();
            medicalPracticeDialogTriplet.SearchCriteria.MedicalPracticeName = medicalPracticeName;
            medicalPracticeDialogTriplet.SearchCriteria.Search();
            var medicalPracticeDialogResults = medicalPracticeDialogTriplet.SearchCriteria.Search();
            var medicalPracticeDialogTile = medicalPracticeDialogResults.SingleOrDefault(t => t.Name.Equals(medicalPracticeName));
            var medicalPracticeDialogDetails = medicalPracticeDialogTile.Click<MedicalPracticeDialog>();
            medicalPracticeDialogTriplet.OK();

            // Save Pupil records
            pupilRecordPage.ClickSave();
            Console.WriteLine('2');

            // Re-search Pupil to verify add Medical Practice successufully
            pupilRecordsTriplet.SearchCriteria.PupilName = string.Format("{0}, {1}", pupilSureName, pupilForeName);
            pupilRecordsTriplet.SearchCriteria.IsCurrent = true;
            pupilRecordsTriplet.SearchCriteria.Search();
            pupilsearchresults = pupilRecordsTriplet.SearchCriteria.Search();
            pupilTile = pupilsearchresults.FirstOrDefault(t => t.Name.Equals(String.Format("{0}, {1}", pupilSureName, pupilForeName)));
            pupilRecordPage = pupilTile.Click<PupilRecordPage>();
            //pupilRecordPage.SelectMedicalTab();
            Wait.WaitForDocumentReady();
            // Verify that add new Medical Practice is displayed on table
            var medicalPracticeTable = pupilRecordPage.MedicalPractice;
            var editMedicalPracticeDialog = medicalPracticeTable.Rows.FirstOrDefault(t => t.Name.Equals(medicalPracticeName)).ClickEdit();
            editMedicalPracticeDialog.PracticeName = medicalPracticeNameUpdate;
            //        var telephoneNumbersTableDialog = editMedicalPracticeDialog.TelephoneNumbersDialog;
            //        telephoneNumbersTableDialog[0].TelephoneNumbers = telephoneNumberUpdate;
            var doctorTableDialog = editMedicalPracticeDialog.Doctors;
            doctorTableDialog.Rows.FirstOrDefault(x => x.Name == doctorTitle + " " + doctorForeName[0].ToString().ToUpper() + " " + doctorSureName).DeleteRow();
            pupilRecordPage = editMedicalPracticeDialog.OK();
            pupilRecordPage.ClickSave();
            Console.WriteLine('3');
            // Re-search Pupil to add Doctor Update
            pupilRecordsTriplet.SearchCriteria.PupilName = string.Format("{0}, {1}", pupilSureName, pupilForeName);
            pupilRecordsTriplet.SearchCriteria.IsCurrent = true;
            pupilRecordsTriplet.SearchCriteria.Search();
            pupilsearchresults = pupilRecordsTriplet.SearchCriteria.Search();
            pupilTile = pupilsearchresults.FirstOrDefault(t => t.Name.Equals(String.Format("{0}, {1}", pupilSureName, pupilForeName)));
            pupilRecordPage = pupilTile.Click<PupilRecordPage>();

            //pupilRecordPage.SelectMedicalTab();
            medicalPracticeTable = pupilRecordPage.MedicalPractice;
            editMedicalPracticeDialog = medicalPracticeTable.Rows.FirstOrDefault(t => t.Name.Equals(medicalPracticeNameUpdate)).ClickEdit();
            var addDoctorDialog = editMedicalPracticeDialog.AddDoctor();
            addDoctorDialog.Title = doctorTitleUpdate;
            addDoctorDialog.ForeName = doctorForeNameUpdate;
            addDoctorDialog.MiddleName = doctorMiddleNameUpdate;
            addDoctorDialog.SureName = doctorSureNameUpdate;
            editMedicalPracticeDialog = addDoctorDialog.ClickOK();
            pupilRecordPage = editMedicalPracticeDialog.OK();
            pupilRecordPage.ClickSave();

            // Re-search Pupil to verify add Doctor successfully
            pupilRecordsTriplet.SearchCriteria.PupilName = string.Format("{0}, {1}", pupilSureName, pupilForeName);
            pupilRecordsTriplet.SearchCriteria.IsCurrent = true;
            pupilRecordsTriplet.SearchCriteria.Search();
            pupilsearchresults = pupilRecordsTriplet.SearchCriteria.Search();
            pupilTile = pupilsearchresults.FirstOrDefault(t => t.Name.Equals(String.Format("{0}, {1}", pupilSureName, pupilForeName)));
            pupilRecordPage = pupilTile.Click<PupilRecordPage>();

            // Verify that save message succesfully display
            medicalPracticeTable = pupilRecordPage.MedicalPractice;
            Assert.AreNotEqual(null, (medicalPracticeTable.Rows.FirstOrDefault(x => x.Name.Equals(medicalPracticeNameUpdate))), "Medical practice Name update is not equal");
            editMedicalPracticeDialog = medicalPracticeTable.Rows.FirstOrDefault(t => t.Name.Equals(medicalPracticeNameUpdate)).ClickEdit();
            doctorTableDialog = editMedicalPracticeDialog.Doctors;
            Assert.AreNotEqual(null, (doctorTableDialog.Rows.SingleOrDefault(x => x.Name == doctorTitleUpdate + " " + doctorForeNameUpdate[0].ToString().ToUpper() + " " + doctorSureNameUpdate)), "Add new Doctor was failed");
            editMedicalPracticeDialog.ClickCancel();

            // Navigate to Medical Practice to verify that update medical practice sussessfully
            AutomationSugar.NavigateMenu("Tasks", "Communications", "Medical Practices");

            // Search and delete if Medical practice is existing
            medicalPracticeTriplet = new MedicalPracticeTriplet();
            medicalPracticeTriplet.SearchCriteria.SearchByName = medicalPracticeNameUpdate;
            medicalPracticeTriplet.SearchCriteria.Search();
            medicalPracticeResults = medicalPracticeTriplet.SearchCriteria.Search();
            medicalPracticeTile = medicalPracticeResults.SingleOrDefault(t => t.Name.Equals(medicalPracticeNameUpdate));
            medicalPracticePage = medicalPracticeTile.Click<MedicalPracticePage>();

            // Verify update Practice name and Doctor successfully
            Assert.AreEqual(medicalPracticeNameUpdate, medicalPracticePage.PracticeName, "Medical practice update is not equal");

            //      telephoneNumberTable = medicalPracticePage.TelephoneNumbers;
            //      Assert.AreNotEqual(null, (telephoneNumberTable.Rows.SingleOrDefault(x => x.TelephoneNumbers.Equals(telephoneNumberUpdate))), "Telephone number update is not equal");

            var doctorTable = medicalPracticePage.Doctors;
            Assert.AreNotEqual(null, (doctorTable.Rows.SingleOrDefault(x => x.Name == doctorTitleUpdate + " " + doctorForeNameUpdate[0].ToString().ToUpper() + " " + doctorSureNameUpdate)), "Doctor update is not equal");

            #endregion STEPS

            #region POS-CONDITIONS

            // Delete Medical Practice
            var confirmDeleteDialog = medicalPracticePage.Delete();
            confirmDeleteDialog.ConfirmDelete();

            #endregion POS-CONDITIONS
        }

        /// <summary>
        /// TC_COM_8_Communications_Medical_Practices
        /// Author: Luong.Mai
        /// Description: Remove a local medical practice currently selected on a pupil records
        /// </summary>
        [WebDriverTest(TimeoutSeconds = 800, Enabled = true, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { "g1", "All", "P2" }, DataProvider = "TC_COM_8_Data")]
        public void TC_COM_8_Communications_Medical_Practices(string practiceName)
        {
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);


            AutomationSugar.Log("Data Creation started");
            Guid pupilId = Guid.NewGuid();
            DataPackage dataPackage = this.BuildDataPackage();
            var pupilSureName = Utilities.GenerateRandomString(10, "TC08_Surname");
            var pupilForeName = Utilities.GenerateRandomString(10, "TC08_Forename" + Thread.CurrentThread.ManagedThreadId);
            dataPackage.AddBasicLearner(pupilId, pupilSureName, pupilForeName, new DateTime(2005, 10, 01), new DateTime(2012, 08, 01));
            DataSetup DataSetPupil = new DataSetup(purgeBeforeInsert: false, purgeAfterTest: true, packages: dataPackage);


            #region Pre-Condition: Add new a medical practice to a pupil

            // Navigate to Pupil Record
            SeleniumHelper.NavigateQuickLink("Pupil Records");

            // Search and select a pupil
            var pupilRecordTriplet = new PupilRecordTriplet();
            pupilRecordTriplet.SearchCriteria.PupilName = pupilSureName;
            var results = pupilRecordTriplet.SearchCriteria.Search();
            var pupilTile = results.FirstOrDefault(t => t.Name.Equals(String.Format("{0}, {1}", pupilSureName, pupilForeName)));
          //  var pupilRecordDetailPage = results.FirstOrDefault(x => x.Name.Trim().Equals(pupilName)).Click<PupilRecordPage>();
            var pupilRecordDetailPage = pupilTile.Click<PupilRecordPage>();



            // Navigate to Medical section
            pupilRecordDetailPage.SelectMedicalTab();

            // Add new medical practice
            var medicalPracticeTriplet = pupilRecordDetailPage.ClickMedicalPractice();
            var medicalPracticeDialog = medicalPracticeTriplet.Create();
            medicalPracticeDialog.PracticeName = practiceName;
            medicalPracticeTriplet.ClickOk();

            // Save pupil
            pupilRecordDetailPage.SavePupil();

            #endregion

            #region Steps

            // Search and select a pupil
            pupilRecordTriplet.SearchCriteria.PupilName = pupilSureName;
            results = pupilRecordTriplet.SearchCriteria.Search();
            pupilTile = results.FirstOrDefault(t => t.Name.Equals(String.Format("{0}, {1}", pupilSureName, pupilForeName)));
            //  var pupilRecordDetailPage = results.FirstOrDefault(x => x.Name.Trim().Equals(pupilName)).Click<PupilRecordPage>();
            pupilRecordDetailPage = pupilTile.Click<PupilRecordPage>();


            // Delete a row
            var medicalPractices = pupilRecordDetailPage.MedicalPractice;
            medicalPractices.Refresh();
            medicalPractices.Rows.FirstOrDefault(x => x.Name.Trim().Equals(practiceName)).DeleteRow();

            // Save
            pupilRecordDetailPage.SavePupil();

            // Search and select a pupil
            pupilRecordTriplet.SearchCriteria.PupilName = pupilSureName;
            results = pupilRecordTriplet.SearchCriteria.Search();
            pupilTile = results.FirstOrDefault(t => t.Name.Equals(String.Format("{0}, {1}", pupilSureName, pupilForeName)));
            //  var pupilRecordDetailPage = results.FirstOrDefault(x => x.Name.Trim().Equals(pupilName)).Click<PupilRecordPage>();
            pupilRecordDetailPage = pupilTile.Click<PupilRecordPage>();

            // Verify that Local medical practice is no longer linked to the Pupil record
            medicalPractices = pupilRecordDetailPage.MedicalPractice;
            Assert.AreEqual(null, medicalPractices.Rows.FirstOrDefault(x => x.Name.Trim().Equals(practiceName)), "Delete medical practice unsuccessfully!");

            #endregion

            #region Post-Condition: Delete the medical practice

            // Navigate to Medical Practices
            AutomationSugar.NavigateMenu("Tasks", "Communications", "Medical Practices");

            // Search medical practice to delete it
            var medicalPracticeTripletPage = new MedicalPracticeTriplet();
            medicalPracticeTripletPage.SearchCriteria.SearchByName = practiceName;
            var resultMedicalPractices = medicalPracticeTripletPage.SearchCriteria.Search();
            var medicalPraticeDetailPage = resultMedicalPractices.FirstOrDefault(x => x.Name.Trim().Equals(practiceName)).Click<MedicalPracticePage>();

            // Delete
            var deleteConfirmationDialog = medicalPraticeDetailPage.Delete();
            deleteConfirmationDialog.ConfirmDelete();

            #endregion


        }

        /// <summary>
        /// TC_COM_9_Communications_Medical_Practices
        /// Author: Luong.Mai
        /// Description: Delete a local medical practice not currently selected on any pupil records
        /// </summary>
        [WebDriverTest(TimeoutSeconds = 800, Enabled = true, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { "g1", "All", "P2" }, DataProvider = "TC_COM_9_Data")]
        public void TC_COM_9_Communications_Medical_Practices(string practiceName)
        {
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);

            AutomationSugar.Log("Data Creation started");
            Guid pupilId = Guid.NewGuid();
            DataPackage dataPackage = this.BuildDataPackage();
            var pupilSureName = Utilities.GenerateRandomString(10, "TC08_Surname");
            var pupilForeName = Utilities.GenerateRandomString(10, "TC08_Forename" + Thread.CurrentThread.ManagedThreadId);
            dataPackage.AddBasicLearner(pupilId, pupilSureName, pupilForeName, new DateTime(2005, 10, 01), new DateTime(2012, 08, 01));
            DataSetup DataSetPupil = new DataSetup(purgeBeforeInsert: false, purgeAfterTest: true, packages: dataPackage);

            #region Pre-Condition: Add new a medical practice
            Guid medicalId;
            var medicalPractice = this.BuildDataPackage()
                                  .AddData("MedicalPractice", IDCDataPackageHelper.GenerateMedicalPractice(out medicalId, practiceName));
            DataSetup setMedicalPractice = new DataSetup(purgeBeforeInsert: false, purgeAfterTest: true, packages: medicalPractice);
            #endregion

            #region Pre-Condition: Add the medical practice to a pupil
            // Navigate to Pupil Record
            SeleniumHelper.NavigateQuickLink("Pupil Records");

            // Search and select a pupil
            var pupilRecordTriplet = new PupilRecordTriplet();
            pupilRecordTriplet.SearchCriteria.PupilName = pupilSureName;
            var results = pupilRecordTriplet.SearchCriteria.Search();
            var pupilTile = results.FirstOrDefault(t => t.Name.Equals(String.Format("{0}, {1}", pupilSureName, pupilForeName)));
            //  var pupilRecordDetailPage = results.FirstOrDefault(x => x.Name.Trim().Equals(pupilName)).Click<PupilRecordPage>();
            var pupilRecordDetailPage = pupilTile.Click<PupilRecordPage>();
            Wait.WaitForDocumentReady();
            // Navigate to Medical section
            pupilRecordDetailPage.SelectMedicalTab();

            // Add new medical practice
            var medicalPracticeTripletDialog = pupilRecordDetailPage.ClickMedicalPractice();
            medicalPracticeTripletDialog.SearchCriteria.MedicalPracticeName = practiceName;
            medicalPracticeTripletDialog.SearchCriteria.Search();
            var medicalPracticeResults = medicalPracticeTripletDialog.SearchCriteria.Search();
            var medicalDetailDialog = medicalPracticeResults.FirstOrDefault(x => x.Name.Trim().Equals(practiceName)).Click<MedicalPracticeDialog>();
            Wait.WaitForDocumentReady();
            medicalPracticeTripletDialog.ClickOk();

            // Save pupil
            pupilRecordDetailPage.SavePupil();

            #endregion

            #region Steps:
            //   Wait.WaitForAjaxReady(By.CssSelector(".locking-mask-loading"));
            // Navigate to Medical Practices
            AutomationSugar.NavigateMenu("Tasks", "Communications", "Medical Practices");

            // Search medical practice to delete it
            var medicalPracticeTriplet = new MedicalPracticeTriplet();
            medicalPracticeTriplet.SearchCriteria.SearchByName = practiceName;
            medicalPracticeTriplet.SearchCriteria.Search();
            var resultMedicalPractices = medicalPracticeTriplet.SearchCriteria.Search();
            var medicalPraticeDetailPage = resultMedicalPractices.FirstOrDefault(x => x.Name.Trim().Equals(practiceName)).Click<MedicalPracticePage>();

            // Delete
            var deleteConfirmationDialog = medicalPraticeDetailPage.Delete();
            deleteConfirmationDialog.ConfirmDelete();

            // Search again
            resultMedicalPractices = medicalPracticeTriplet.SearchCriteria.Search();

            // Verify that The Practice name is no longer returned in Search results
            Assert.AreEqual(null, resultMedicalPractices.FirstOrDefault(x => x.Name.Trim().Equals(practiceName)), "It is Deleting the medical practice unsuccessfully!");
            //   Wait.WaitForAjaxReady(By.CssSelector(".locking-mask-loading"));
            // Navigate to Pupil Record
            SeleniumHelper.NavigateQuickLink("Pupil Records");

            // Search and select a pupil
            pupilRecordTriplet = new PupilRecordTriplet();
            pupilRecordTriplet.SearchCriteria.PupilName = pupilSureName;
            pupilRecordTriplet.SearchCriteria.Search();
            results = pupilRecordTriplet.SearchCriteria.Search();
            pupilTile = results.FirstOrDefault(t => t.Name.Equals(String.Format("{0}, {1}", pupilSureName, pupilForeName)));
            //  var pupilRecordDetailPage = results.FirstOrDefault(x => x.Name.Trim().Equals(pupilName)).Click<PupilRecordPage>();
            pupilRecordDetailPage = pupilTile.Click<PupilRecordPage>();

            // Navigate to Medical section
            //pupilRecordDetailPage.SelectMedicalTab();

            // Verify that This medical practice should not longer be visible for selection on pupil records
            var medicalPractices = pupilRecordDetailPage.MedicalPractice;
            Assert.AreEqual(null, medicalPractices.Rows.FirstOrDefault(x => x.Name.Trim().Equals(practiceName)), "Delete the medical practice unsuccessfully!");

            #endregion
        }

        /// <summary>
        /// TC_COM_10_Look_Ups_School_Service_Type
        /// Author: Luong.Mai
        /// Description: Add some new lookups for 'Service Type'
        /// </summary>
        [WebDriverTest(TimeoutSeconds = 800, Enabled = true, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { "g1", "All", "P2" }, DataProvider = "TC_COM_10_Data")]
        public void TC_COM_10_Look_Ups_School_Service_Type(string sTypeCode, string sTypeDescription, string agentForename, string agentSurname)
        {
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.TestUser);

            #region Steps

            // Navigate to Service types
            AutomationSugar.NavigateMenu("Lookups", "School", "Service Type");

            // Search
            var serviceTypeTriplet = new ServiceTypesTriplet();
            var serviceTypesPage = serviceTypeTriplet.SearchCriteria.Search<ServiceTypesPage>();

            // Add Service Provided
            serviceTypesPage.AddServiceProvided();

            // Add new Service Provided record
            var serviceTypes = serviceTypesPage.ServiceTypes;
            serviceTypes.Refresh();
            var lastRow = serviceTypes.Rows.Last();
            lastRow.Code = sTypeCode;
            lastRow.Description = sTypeDescription;
            lastRow.DisplayOrder = "100";
            lastRow.IsVisible = true;

            // Verify Resource Provider defaults to 'My School'  Name
            //  Assert.AreEqual(BrowserDefaults.SchoolName, lastRow.ResourceProviderName, "Resource Provider is wrong!");

            // Save
            serviceTypesPage.Save();

            // Navigate to Agents
            AutomationSugar.NavigateMenu("Tasks", "Communications", "Agents");

            // Create new agent
            var agentTripletPage = new AgentDetailTriplet();
            var addNewAgentDialog = agentTripletPage.AddNewAgent();
            addNewAgentDialog.ForeName = agentForename;
            addNewAgentDialog.SurName = agentSurname;

            addNewAgentDialog.ClickContinue();
         
            bool assert1 = SeleniumHelper.IsElementExists(POM.Helper.SimsBy.AutomationId(sTypeDescription));

            Assert.AreEqual(true, assert1, "The new service type isnt displayed.");

            //delete agent. 
            var deleteButton = SeleniumHelper.FindElement(By.CssSelector(SeleniumHelper.AutomationId("delete_button")));
            deleteButton.Click();
            Thread.Sleep(500);
            var deleteContinue = SeleniumHelper.FindElement(By.CssSelector(SeleniumHelper.AutomationId("continue_with_delete_button")));
            deleteContinue.Click();
            Wait.WaitForAjaxReady(By.CssSelector(".locking-mask-loading"));

            #endregion

            #region Post-Condition: Delete the service type that created
            Wait.WaitForAjaxReady(By.CssSelector(".locking-mask-loading"));
            // Navigate to Service types
            AutomationSugar.NavigateMenu("Lookups", "School", "Service Type");

            // Search
            serviceTypeTriplet = new ServiceTypesTriplet();
            serviceTypesPage = serviceTypeTriplet.SearchCriteria.Search<ServiceTypesPage>();

            // Delete 
            serviceTypes = serviceTypesPage.ServiceTypes;
            serviceTypes.Rows.FirstOrDefault(x => x.Code.Trim().Equals(sTypeCode)).DeleteRow();

            #endregion

        }

        /// <summary>
        /// TC_COM_11_Look_Ups_School_Service_Type
        /// Author: Luong.Mai
        /// Description: Delete a 'Service Type' Lookup entry
        /// </summary>
        [WebDriverTest(TimeoutSeconds = 800, Enabled = true, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { "g1","All", "P2" }, DataProvider = "TC_COM_11_Data")]
        public void TC_COM_11_Look_Ups_School_Service_Type(string sTypeCode, string sTypeDescription, string agentForename, string agentSurname)
        {
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);

            #region Pre-Conditon: Create new Service Type

            // Navigate to Service types
            AutomationSugar.NavigateMenu("Lookups", "School", "Service Type");

            // Search
            var serviceTypeTriplet = new ServiceTypesTriplet();
            var serviceTypesPage = serviceTypeTriplet.SearchCriteria.Search<ServiceTypesPage>();
            Console.WriteLine("Searched");
            // Add Service Provided
            serviceTypesPage.AddServiceProvided();

            // Add new Service Provided record
            var serviceTypes = serviceTypesPage.ServiceTypes;
            //   serviceTypes.Refresh();
            var lastRow = serviceTypes.Rows.Last();
            lastRow.Code = sTypeCode;
            lastRow.Description = sTypeDescription;
            lastRow.DisplayOrder = "121";
            lastRow.IsVisible = true;

            // Save
            serviceTypesPage.Save();
            Console.WriteLine("Saved");
            #endregion

            #region Steps:

            // Search for delete
            serviceTypesPage = serviceTypeTriplet.SearchCriteria.Search<ServiceTypesPage>();

            // Delete 
            serviceTypes = serviceTypesPage.ServiceTypes;
            serviceTypes.Rows.FirstOrDefault(x => x.Code.Trim().Equals(sTypeCode)).DeleteRow();

            //   serviceTypes.Rows.First().DeleteRow();

            // Save

            serviceTypesPage.Save();

            Wait.WaitForAjaxReady(By.CssSelector(".locking-mask-loading"));
            // Navigate to Agents
            AutomationSugar.NavigateMenu("Tasks", "Communications", "Agents");

            // Create new agent
            var agentTripletPage = new AgentDetailTriplet();
            var addNewAgentDialog = agentTripletPage.AddNewAgent();
            addNewAgentDialog.ForeName = agentForename;
            addNewAgentDialog.SurName = agentSurname;

            // Continue
            addNewAgentDialog.ClickContinue();

            // Verify new Service Type is no longer listed as an available Service
            bool assert1 = SeleniumHelper.IsElementExists(POM.Helper.SimsBy.AutomationId(sTypeDescription));


            Assert.AreEqual(false, assert1, "The new Service Type is still displayed");

            //delete agent. 
            var deleteButton = SeleniumHelper.FindElement(By.CssSelector(SeleniumHelper.AutomationId("delete_button")));
            deleteButton.Click();
            Thread.Sleep(500);
            var deleteContinue = SeleniumHelper.FindElement(By.CssSelector(SeleniumHelper.AutomationId("continue_with_delete_button")));
            deleteContinue.Click();
            Wait.WaitForAjaxReady(By.CssSelector(".locking-mask-loading"));

            #endregion

        }

        /// <summary>
        /// TC_COM_12_Look_Ups_School_Service_Type
        /// Author: Luong.Mai
        /// Description: Amend a 'Service Type' lookup entry that is linked to an Agent or Agency
        /// </summary>
        [WebDriverTest(TimeoutSeconds = 800, Enabled = false, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { "g1", "All", "P2" }, DataProvider = "TC_COM_12_Data")]
        public void TC_COM_12_Look_Ups_School_Service_Type(string sTypeCode, string sTypeDescription, string sTypeCodeEdited, string sTypeDescriptionEdited, string agentForename, string agentSurname)
        {
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);

            #region Pre-Conditon: Create new Service Type

            // Navigate to Service types
            AutomationSugar.NavigateMenu("Lookups", "School", "Service Type");

            // Search
            var serviceTypeTriplet = new ServiceTypesTriplet();
            var serviceTypesPage = serviceTypeTriplet.SearchCriteria.Search<ServiceTypesPage>();

            // Add Service Provided
            serviceTypesPage.AddServiceProvided();

            // Add new Service Provided record
            var serviceTypes = serviceTypesPage.ServiceTypes;
            serviceTypes.Refresh();
            var lastRow = serviceTypes.Rows.Last();
            lastRow.Code = sTypeCode;
            lastRow.Description = sTypeDescription;
            lastRow.DisplayOrder = "165";
            lastRow.IsVisible = true;

            // Save
            serviceTypesPage.Save();

            #endregion

            #region Steps:

            // Search
            serviceTypeTriplet = new ServiceTypesTriplet();
            serviceTypesPage = serviceTypeTriplet.SearchCriteria.Search<ServiceTypesPage>();

            // Amend a service type
            serviceTypes = serviceTypesPage.ServiceTypes;
            var editedServiceType = serviceTypes.Rows.FirstOrDefault(x => x.Code.Trim().Equals(sTypeCode));
            editedServiceType.Code = sTypeCodeEdited;
            editedServiceType.Description = sTypeDescriptionEdited;
            editedServiceType.IsVisible = true;

            // Verify Resource Provider defaults to 'My School'  Name
            //    Assert.AreEqual(BrowserDefaults.SchoolName, editedServiceType.ResourceProviderName, "Resource Provider is wrong!");

            // Save
            serviceTypesPage.Save();

            // Navigate to Agents
            AutomationSugar.NavigateMenu("Tasks", "Communications", "Agents");

            // Create new agent
            var agentTripletPage = new AgentDetailTriplet();
            var addNewAgentDialog = agentTripletPage.AddNewAgent();
            addNewAgentDialog.ForeName = agentForename;
            addNewAgentDialog.SurName = agentSurname;

            // Continue
            addNewAgentDialog.ClickContinue();
            //var serviceProvidedDialog = addNewAgentDialog.ClickContinue();

            bool assert1 = SeleniumHelper.IsElementExists(POM.Helper.SimsBy.AutomationId(sTypeDescription));

            // Confirm the changes are shown on their Record
            Assert.AreEqual(true, assert1, "The Service Type isn't changed.");
         
            //delete agent. 
            var deleteButton = SeleniumHelper.FindElement(By.CssSelector(SeleniumHelper.AutomationId("delete_button")));
            deleteButton.Click();
            Thread.Sleep(500);
            var deleteContinue = SeleniumHelper.FindElement(By.CssSelector(SeleniumHelper.AutomationId("continue_with_delete_button")));
            deleteContinue.Click();
            Wait.WaitForAjaxReady(By.CssSelector(".locking-mask-loading"));

            #endregion

            #region Post-Condition: Delete the service type

            // Navigate to Service types
            AutomationSugar.NavigateMenu("Lookups", "School", "Service Type");

            // Search for delete
            serviceTypeTriplet = new ServiceTypesTriplet();
            serviceTypesPage = serviceTypeTriplet.SearchCriteria.Search<ServiceTypesPage>();

            // Delete 
            serviceTypes = serviceTypesPage.ServiceTypes;
            string str = sTypeCodeEdited.Remove(sTypeCodeEdited.Length - 1);
            //sTypeCodeEdited.Remove(sTypeCodeEdited.Length - 1);

            serviceTypes.Rows.FirstOrDefault(x => x.Code.Trim().Contains(str)).DeleteRow();

            // Save
            serviceTypesPage.Save();

            #endregion
        }


        /// <summary>
        /// TC_COM_13_Look_Ups_School_Service_Type
        /// Author: Luong.Mai
        /// Description: Delete a lookups for 'Service Type' linked to an Agent or Agency (this should be prevented)
        /// </summary>
        [WebDriverTest(TimeoutSeconds = 1200, Enabled = true, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { "g1", "All", "P2" }, DataProvider = "TC_COM_13_Data")]
        public void TC_COM_13_Look_Ups_School_Service_Type(string sTypeCode, string sTypeDescription, string agentForename, string agentSurname)
        {
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);

            #region Pre-Conditon: Create new Service Type

            // Navigate to Service types
            AutomationSugar.NavigateMenu("Lookups", "School", "Service Type");

            // Search
            var serviceTypeTriplet = new ServiceTypesTriplet();
            var serviceTypesPage = serviceTypeTriplet.SearchCriteria.Search<ServiceTypesPage>();

            // Add Service Provided
            serviceTypesPage.AddServiceProvided();

            // Add new Service Provided record
            var serviceTypes = serviceTypesPage.ServiceTypes;
            serviceTypes.Refresh();
            var lastRow = serviceTypes.Rows.Last();
            lastRow.Code = sTypeCode;
            lastRow.Description = sTypeDescription;
            lastRow.DisplayOrder = "110";
            lastRow.IsVisible = true;

            // Save
            serviceTypesPage.Save();

            #endregion

            #region Pre-Condition: Create new agent

            // Navigate to Agents
            AutomationSugar.NavigateMenu("Tasks", "Communications", "Agents");

            // Create new agent
            var agentTripletPage = new AgentDetailTriplet();
            var addNewAgentDialog = agentTripletPage.AddNewAgent();
            addNewAgentDialog.ForeName = agentForename;
            addNewAgentDialog.SurName = agentSurname;

            // Continue
            addNewAgentDialog.ClickContinue();

            //var serviceProvidedDialog = addNewAgentDialog.ClickContinue();
            SeleniumHelper.Sleep(2);
            
            
            // Select the service type
            var _serviceType = SeleniumHelper.Get(POM.Helper.SimsBy.AutomationId(sTypeDescription));
            _serviceType.Set(true);

            //Save Agent Record.
            SeleniumHelper.FindElement(By.CssSelector(SeleniumHelper.AutomationId("well_know_action_save"))).Click();
            Wait.WaitForAjaxReady(By.CssSelector(".locking-mask-loading"));

            #endregion

            #region Steps:

            // Navigate to Service types
            AutomationSugar.NavigateMenu("Lookups", "School", "Service Type");

            // Search
            serviceTypeTriplet = new ServiceTypesTriplet();
            serviceTypesPage = serviceTypeTriplet.SearchCriteria.Search<ServiceTypesPage>();

            // Delete
            serviceTypes = serviceTypesPage.ServiceTypes;
            serviceTypes.Rows.FirstOrDefault(x => x.Code.Trim().Equals(sTypeCode)).DeleteRow();
            serviceTypesPage.Save();

            // The System Returns Message "Fault Service Provided cannot be deleted 
            // as it has ServiceProvideds attached, and no re exists to permit this"
            Assert.AreEqual(true, serviceTypesPage.IsWarningMessageIsDisplayed(), "The warning message isn't displayed");

            // Navigate to Agents
            AutomationSugar.NavigateMenu("Tasks", "Communications", "Agents");

            // Passing the warning dialog
            var warningDialog = new WarningDialog();
            warningDialog.DontSave();

            // Search agent
            agentTripletPage = new AgentDetailTriplet();
            agentTripletPage.SearchCriteria.AgentName = agentForename + "," + agentSurname;
            var agentResult = agentTripletPage.SearchCriteria.Search();
            var agentDetailsPage = agentResult.FirstOrDefault(x => x.Name.Trim().Equals(agentForename + ", " + agentSurname)).Click<AgentDetailPage>();

            // Verify that the Agent or Agency linked to the Service Type still contains valid information 
            var serviceProvides = agentDetailsPage.GetCheckedServiceProvide();
            Assert.AreEqual(true, serviceProvides.Contains(sTypeDescription), "The agent contains invalid information");

            // Add new agent
            addNewAgentDialog = agentTripletPage.AddNewAgent();
            addNewAgentDialog.ForeName = agentForename + "new";
            addNewAgentDialog.SurName = agentSurname + "new";

            addNewAgentDialog.ClickContinue();
            //serviceProvidedDialog = addNewAgentDialog.ClickContinue();

            bool assert1 = SeleniumHelper.IsElementExists(POM.Helper.SimsBy.AutomationId(sTypeDescription));
            // Verify new Service Type is still listed as an available Service
            //serviceProvidedDialog.ServiceTypeId = sTypeDescription;
            Assert.AreEqual(true, assert1, "The Service Type isn't existed.");

            //delete agent. 
            var deleteButton = SeleniumHelper.FindElement(By.CssSelector(SeleniumHelper.AutomationId("delete_button")));
            deleteButton.Click();
            Thread.Sleep(500);
            var deleteContinue = SeleniumHelper.FindElement(By.CssSelector(SeleniumHelper.AutomationId("continue_with_delete_button")));
            deleteContinue.Click();
            Wait.WaitForAjaxReady(By.CssSelector(".locking-mask-loading"));

            #endregion
        }

        /// <summary>
        /// TC_COM14
        /// Au : Hieu Pham
        /// Description: Record details of a new agent
        /// Role: School Adminstrator
        /// Status: Pending by Bug #2 : Can not add document for an agent
        /// </summary>
        [WebDriverTest(TimeoutSeconds = 2000, Enabled = true, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { "g1", "All", "P2" }, DataProvider = "TC_COM14_DATA")]
        public void TC_COM14_Record_Detail_New_Agent(string surName, string foreName, string serviceProvide, string agency, string telephoneNumber,
            string location, bool isMainNumber, string note, string emailAddress, bool isMainEmail, string emailNote, string postcode, string address,
            string documentSummary, string documentNote, string country)
        {

            #region Pre-conditions

            // Login
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);

            // Navigate to Agent
            AutomationSugar.NavigateMenu("Tasks", "Communications", "Agents");

            // Search old data and delete if existed
            var agentDetailTriplet = new AgentDetailTriplet();
            agentDetailTriplet.SearchCriteria.AgentName = String.Format("{0}, {1}", surName, foreName);
            var searchResult = agentDetailTriplet.SearchCriteria.Search().FirstOrDefault(x => x.Name.Equals(String.Format("{0}, {1}", surName, foreName)));
            var agentDetailPage = searchResult == null ? new AgentDetailPage() : searchResult.Click<AgentDetailPage>();
            agentDetailPage.Delete();

            #endregion

            #region Steps

            // Create new Agent
            agentDetailTriplet.Refresh();
            var addNewDialog = agentDetailTriplet.AddNewAgent();

            // Enter surname and forename
            addNewDialog.ForeName = foreName;
            addNewDialog.SurName = surName;

            // Click Continue
            addNewDialog.ClickContinue();

            // Select the service type
            var _serviceType = SeleniumHelper.Get(POM.Helper.SimsBy.AutomationId(serviceProvide));
            _serviceType.Set(true);

            //Save Agent Record.
            SeleniumHelper.FindElement(By.CssSelector(SeleniumHelper.AutomationId("well_know_action_save"))).Click();
            Wait.WaitForAjaxReady(By.CssSelector(".locking-mask-loading"));
            
            // Add new linked agency
            var linkedAgencyDialog = agentDetailPage.AddLinkAgencies();
            //linkedAgencyDialog.SearchCriteria.AgencyName = agency;

            linkedAgencyDialog.SearchCriteria.Search();
            IWebElement resultTile = SeleniumHelper.FindElement(By.CssSelector(SeleniumHelper.AutomationId("resultTile")));
            SeleniumHelper.ClickByJS(resultTile);
            //Wait.WaitForElementDisplayed(By.CssSelector(SeleniumHelper.AutomationId("Agency")));

            // Click OK
            SeleniumHelper.Sleep(2);
            linkedAgencyDialog.ClickOk();

            // Click tab contact detail
            agentDetailPage.Refresh();

            // Click add new address
            var addAddressDialog = agentDetailPage.AddNewAddress();
            //addAddressDialog.SearchCriteria.FullPostCode = postcode;
            //addAddressDialog.SearchCriteria.Search().FirstOrDefault().Click();
            addAddressDialog.PostCode = "BT57 8RR";
            //click find address
            addAddressDialog.ClickManualAddAddress();
            //addressDialog.SetAddressDropdownValue("");

            //var addressTile = addressDialog.SearchCriteria.Search().FirstOrDefault();
            //addressTile.Click();
            // Click Ok
            addAddressDialog.ClickOk();

            // Add phone number
            IWebElement contactDetailsHeader = SeleniumHelper.FindElement(By.CssSelector(SeleniumHelper.AutomationId("section_menu_Contact Details")));
            SeleniumHelper.ClickByJS(contactDetailsHeader);
            IWebElement addTelephoneNumberHeader = SeleniumHelper.FindElement(By.CssSelector(SeleniumHelper.AutomationId("add_telephone_number_button")));
            SeleniumHelper.ClickByJS(addTelephoneNumberHeader);
            SeleniumHelper.Sleep(2);

            var telephoneNumberTable = agentDetailPage.TelephoneNumberTable;
            var telephoneNumberRow = telephoneNumberTable.GetLastRow();
            telephoneNumberRow.Number = telephoneNumber;
            telephoneNumberRow = telephoneNumberTable.GetLastInsertedRow();
            telephoneNumberRow.Location = location;
            telephoneNumberRow.MainNumber = true;

            // Add Email address
            IWebElement addEmailAddressHeader = SeleniumHelper.FindElement(By.CssSelector(SeleniumHelper.AutomationId("add_email_address_button")));
            SeleniumHelper.ClickByJS(addEmailAddressHeader);

            var emailAddressTable = agentDetailPage.EmailAddressTable;
            var emailAddressRow = emailAddressTable.GetLastRow();
            emailAddressRow.Email = emailAddress;
            emailAddressRow = emailAddressTable.GetLastInsertedRow();
            emailAddressRow.MainEmail = true;

            #region comments
            /* Confirmed by Adiganesh.Kannathason@capita.co.uk, Thursday, October 29, 2015 4:06 PM, 
             * Notes and Documents functionality is revamped and it has not yet implemented the same in Communication module screens.
             * The stories are identified and are raised, So I skip upload document tests
            /*
             
            /*
            // Upload Document
            agentDetailPage.Refresh();
            var documentTable = agentDetailPage.NoteAndDocumentTable;

            // Enter Description and click upload button
            var documentRow = documentTable.GetLastRow();
            documentRow.Summary = documentSummary;
            documentRow.Note = documentNote;
            var viewDocumentDialog = documentRow.AddDocument();

            // Click add document
            var attachmentDialog = viewDocumentDialog.ClickAddAttachment();

            // Click browser
            attachmentDialog.BrowserToDocument();

            // Click upload document
            viewDocumentDialog = attachmentDialog.UploadDocument();
            viewDocumentDialog.ClickOk();
            */
            #endregion
            // Save values 
            agentDetailPage.Refresh();
            agentDetailPage.Save();

            // Search agent again
            agentDetailTriplet = new AgentDetailTriplet();
            agentDetailTriplet.SearchCriteria.AgentName = String.Format("{0}, {1}", surName, foreName);
            agentDetailPage = agentDetailTriplet.SearchCriteria.Search().FirstOrDefault(x => x.Name.Equals(String.Format("{0}, {1}", surName, foreName))).Click<AgentDetailPage>();
            agentDetailPage.ScrollToBasicDetail();

            // Verify value is correct.
            // VP : Surname and forename
            Assert.AreEqual(surName, agentDetailPage.SurName, "Surname is not correct.");
            Assert.AreEqual(foreName, agentDetailPage.ForeName, "Forename is not correct.");

            // VP : Service provide
            Assert.AreEqual(true, agentDetailPage.GetCheckedServiceProvide().Any(x => x.Equals(serviceProvide)), "Service provide is not correct");

            // VP : Linked Agency
            var linkedAgencyTable = agentDetailPage.LinkedAgenciesTable;
            //Assert.AreNotEqual(null, linkedAgencyTable.Rows.FirstOrDefault(x => x.Agency.Equals(agency)), "Linked Agency is not correct");

            // VP: Telephone number
            agentDetailPage.ScrollToContactInformation();
            telephoneNumberTable.Refresh();

            Assert.AreNotEqual(
                null, telephoneNumberTable.Rows.FirstOrDefault(x => x.Number.Equals(telephoneNumber) && x.Location.Equals(location)
                && x.MainNumber.Equals(isMainNumber)), "Telephone number is not correct");

            // VP: Email Address
            emailAddressTable.Refresh();
            Assert.AreNotEqual(null, emailAddressTable.Rows.FirstOrDefault(x => x.Email.Equals(emailAddress) && x.MainEmail.Equals(isMainEmail)), "Email address is not correct");

            // VP : Address
            Assert.AreEqual(String.Format(address + " {0}", country), agentDetailPage.Address.Replace("\r\n", " ").Replace(",", ""), "Address is not correct");

            #region comments
            /* Confirmed by Adiganesh.Kannathason@capita.co.uk, Thursday, October 29, 2015 4:06 PM, 
             * Notes and Documents functionality is revamped and it has not yet implemented the same in Communication module screens.
             * The stories are identified and are raised, So I skip upload document tests
            /*
             
            /*
            // VP: Document
            documentTable = agentDetailPage.NoteAndDocumentTable;
            documentRow = documentTable.Rows.FirstOrDefault(x => x.Summary.Equals(documentSummary));

            // Click add document button
            viewDocumentDialog = documentRow.AddDocument();

            // VP: Document is displayed
            Assert.AreEqual(true, viewDocumentDialog.IsDocumentDisplayed("document.txt"), "Document is not uploaded");

            */
            #endregion
            #endregion

            #region Post condition

            // Search agent again
            agentDetailTriplet = new AgentDetailTriplet();
            agentDetailTriplet.SearchCriteria.AgentName = String.Format("{0}, {1}", surName, foreName);
            agentDetailPage = agentDetailTriplet.SearchCriteria.Search().FirstOrDefault(x => x.Name.Equals(String.Format("{0}, {1}", surName, foreName))).Click<AgentDetailPage>();

            // Delete agent
            agentDetailPage.Delete();

            #endregion

        }

        /// <summary>
        /// TC_COM15
        /// Au : Hieu Pham
        /// Description: Ensure the new Agent can be associated against a pupil's SEN record
        /// Role: School Adminstrator
        /// Status: Pending by Bug #1 : Can not add new Linked Organisations
        /// </summary>
        [WebDriverTest(TimeoutSeconds = 2000, Enabled = false, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { "g1" }, DataProvider = "TC_COM15_DATA")]
        public void TC_COM15_Associate_Agent_Against(string surName, string foreName, string serviceProvide, string agency, string telephoneNumber,
            string location, bool isMainNumber, string note, string emailAddress, bool isMainEmail, string emailNote, string postcode, string address, string pupilSurName, string pupilForeName,
            string gender, string dateOfBirth, string DateOfAdmission, string yearGroup)
        {

            #region Pre-Conditions:

            //DI- Agency here
            string AgencyName = Utilities.GenerateRandomString(5, "Selenium");
            Guid AgencyId;
            var Agency = this.BuildDataPackage()
                .AddData("Agency", IDCDataPackageHelper.GenerateAgency(out AgencyId, AgencyName));

            DataSetup setAgency = new DataSetup(Agency);

            //DI- Agent here
            string AgentForName = Utilities.GenerateRandomString(2, "FornameData");
            string AgentSurName = Utilities.GenerateRandomString(2, "SurnameData");
            Guid AgentId;
            var Agent = this.BuildDataPackage()
                .AddData("Agent", IDCDataPackageHelper.GenerateAgent(out AgentId, AgentForName, AgentSurName));

            DataSetup setAgent = new DataSetup(Agent);

            //Navigate to injected Agency
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.TestUser);
            Wait.WaitLoading();
            AutomationSugar.NavigateMenu("Tasks", "Communications", "Agents");
            var agentDetailTriplet = new AgentDetailTriplet();
            agentDetailTriplet.SearchCriteria.AgentName = String.Format("{0}, {1}", AgentSurName, AgentForName);
            var searchResult = agentDetailTriplet.SearchCriteria.Search().FirstOrDefault(x => x.Name.Equals(String.Format("{0}, {1}", AgentSurName, AgentForName)));
            var agentDetailPage = searchResult == null ? new AgentDetailPage() : searchResult.Click<AgentDetailPage>();

            // Enter service detail
            agentDetailPage.ServiceProvide = serviceProvide;

            // Save values
            agentDetailPage.Save();

            // Add new linked agency
            var linkedAgencyDialog = agentDetailPage.AddLinkAgencies();
            linkedAgencyDialog.SearchCriteria.AgencyName = AgencyName;
            linkedAgencyDialog.SearchCriteria.Search();
            IWebElement elt = SeleniumHelper.Get(By.CssSelector("a[title='Agency ']"));
            SeleniumHelper.Get(By.CssSelector("a[title='Agency ']")).Click();
            Wait.WaitForAjaxReady(By.CssSelector(".locking-mask-loading"));

            // Click OK
            linkedAgencyDialog.ClickOk();
            Wait.WaitForAjaxReady(By.CssSelector(".locking-mask-loading"));

            // Click add new address
            var addAddressDialog = agentDetailPage.AddNewAddress();
            addAddressDialog.PostCode = "BT57 8RR";
            addAddressDialog.ClickManualAddAddress();
            //addAddressDialog.SearchCriteria.Search().FirstOrDefault(x => x.Name.Equals(address)).Click();

            // Click Ok
            addAddressDialog.ClickOk();

            // // Add phone number
            // IWebElement contactDetailsHeader = SeleniumHelper.FindElement(By.CssSelector(SeleniumHelper.AutomationId("section_menu_Contact Details")));
            // SeleniumHelper.ClickByJS(contactDetailsHeader);
            // IWebElement addTelephoneNumberHeader = SeleniumHelper.FindElement(By.CssSelector(SeleniumHelper.AutomationId("add_telephone_number_button")));
            // SeleniumHelper.ClickByJS(addTelephoneNumberHeader);
            // SeleniumHelper.Sleep(2);

            // var telephoneNumberTable = agentDetailPage.TelephoneNumberTable;
            // var telephoneNumberRow = telephoneNumberTable.GetLastRow();
            // telephoneNumberRow.Number = telephoneNumber;
            // telephoneNumberRow = telephoneNumberTable.GetLastInsertedRow();
            // telephoneNumberRow.Location = location;
            // telephoneNumberRow.MainNumber = true;
            // IWebElement add_note_button = SeleniumHelper.FindElement(By.CssSelector(SeleniumHelper.AutomationId("add_note_button")));
            // SeleniumHelper.ClickByJS(add_note_button);
            // telephoneNumberRow.Note = note;

            // // Add Email address
            // IWebElement addEmailAddressHeader = SeleniumHelper.FindElement(By.CssSelector(SeleniumHelper.AutomationId("add_email_address_button")));
            // SeleniumHelper.ClickByJS(addEmailAddressHeader);

            // var emailAddressTable = agentDetailPage.EmailAddressTable;
            // var emailAddressRow = emailAddressTable.GetLastRow();
            // emailAddressRow.Email = emailAddress;
            // emailAddressRow = emailAddressTable.GetLastInsertedRow();
            // emailAddressRow.MainEmail = true;

            // IWebElement agentEmails = SeleniumHelper.FindElement(By.CssSelector("[data-maintenance-container='AgentEmails']"));

            // IWebElement emailNoteElement = agentEmails.FindElement(By.CssSelector(SeleniumHelper.AutomationId("add_note_button")));
            // SeleniumHelper.ClickByJS(emailNoteElement);
            // emailAddressRow.Note = emailNote;

            // // Save values 
            //agentDetailPage.Refresh();


            agentDetailPage.Save();

            #endregion

            #region Steps

            //// Navigate to Sen record
            //AutomationSugar.NavigateMenu("Tasks", "Pupils", "SEN Records");

            //// Search pupil name
            //var senRecordTriplet = new SenRecordTriplet();
            //senRecordTriplet.SearchCriteria.Name = String.Format("{0}, {1}", pupilSurName, pupilForeName);
            //senRecordTriplet.SearchCriteria.NoSenStageAssigned = true;
            //var senRecordPage = senRecordTriplet.SearchCriteria.Search().FirstOrDefault(x => x.Name.Equals(String.Format("{0}, {1}", pupilSurName, pupilForeName))).Click<SenRecordDetailPage>();

            //// Navigate to Linked pupil and organisation
            //var linkedPupilPage = SeleniumHelper.NavigateViaAction<LinkedPeopleAndOrganisationPage>("Linked People and Organisations");

            //// Click Add Agent
            //Assert.AreEqual(true, false, "Can not navigate to 'Add Agent' dialog. An unexpected dialog displays.");

            ///* Because of Bug #1, Unexpected Dialog appears, window 'Link an Agent or Agency' can not open
            // * so I can not implement the steps on 'Link an Agent or Agency' window. The missing steps :
            //    1. Click Button 'Search'
            //    2. Select Agent (use one has just been created)
            //    3. Click Button 'Ok'
            // */

            #endregion
            //   DI-Pupil Here

            #region Data Preperation
            string forenameSetup = String.Format("{0}{1}{2}", pupilForeName, SeleniumHelper.GenerateRandomString(3), SeleniumHelper.GenerateRandomNumberUsingDateTime());
            string surnameSetup = String.Format("{0}{1}{2}", pupilSurName, SeleniumHelper.GenerateRandomString(3), SeleniumHelper.GenerateRandomNumberUsingDateTime());

            DateTime dobSetup = Convert.ToDateTime(dateOfBirth);
            DateTime dateOfAdmissionSetup = Convert.ToDateTime(DateOfAdmission);

            var learnerIdSetup = Guid.NewGuid();
            var BuildPupilRecord = this.BuildDataPackage();
            #endregion

            BuildPupilRecord.AddBasicLearner(learnerIdSetup, surnameSetup, forenameSetup, dobSetup, dateOfAdmissionSetup, genderCode: "1", enrolStatus: "C");

            DataSetup DataSetPupil = new DataSetup(purgeBeforeInsert: false, purgeAfterTest: true, packages: BuildPupilRecord);

            #region Post Condition
            //Agent Page
            searchResult = agentDetailTriplet.SearchCriteria.Search().FirstOrDefault(x => x.Name.Equals(String.Format("{0}, {1}", AgentSurName, AgentForName)));
            agentDetailPage = searchResult == null ? new AgentDetailPage() : searchResult.Click<AgentDetailPage>();

            // Delete agent
            agentDetailPage.Delete();

            // Delete a pupil
            AutomationSugar.NavigateMenu("Tasks", "Pupils", "Delete Pupil");
            var deletePupilRecordTriplet = new DeletePupilRecordTriplet();
            deletePupilRecordTriplet.SearchCriteria.PupilName = String.Format("{0}, {1}", surnameSetup, forenameSetup);
            var resultPupils = deletePupilRecordTriplet.SearchCriteria.Search();
            var pupilSearchTile = resultPupils.SingleOrDefault(t => t.Name.Equals(String.Format("{0}, {1}", surnameSetup, forenameSetup)));
            var deletePupilRecord = pupilSearchTile == null ? new DeletePupilRecordPage() : pupilSearchTile.Click<DeletePupilRecordPage>();
            deletePupilRecord.Delete();

            #endregion
        }

        /// <summary>
        /// TC_COM16
        /// Au : Hieu Pham
        /// Description: Amend record details of an agent linked to a Pupil record
        /// Role: School Adminstrator
        /// Status: Pending by Bug #2 : Can not add document for an agent
        /// </summary>
        [WebDriverTest(TimeoutSeconds = 2000, Enabled = true, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { "g1", "All", "P2" }, DataProvider = "TC_COM16_DATA")]
        public void TC_COM16_Amend_Record_Detail(string surName, string foreName, string serviceProvide, string agency, string telephoneNumber,
            string location, bool isMainNumber, string note, string emailAddress, bool isMainEmail, string emailNote, string postcode, string address,
            string documentSummary, string documentNote, string newSurName, string newForeName, string newServiceProvider, string newAgency,
            string newTelephoneNumber, string newTelephoneLocation, bool newIsMainNumber, string newTelephoneNote, string newEmailAddress, bool newIsMainEmail,
            string newEmailNote, string newBuildingNo, string newBuildingName, string newDocSummary, string newDocNote, string country, string pupilSurName, string pupilForeName,
            string gender, string dateOfBirth, string DateOfAdmission, string yearGroup)
        {

            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.TestUser);
            #region Data Preparation
            string forenameSetup = String.Format("{0}{1}{2}", pupilForeName, SeleniumHelper.GenerateRandomString(3), SeleniumHelper.GenerateRandomNumberUsingDateTime());
            string surnameSetup = String.Format("{0}{1}{2}", pupilSurName, SeleniumHelper.GenerateRandomString(3), SeleniumHelper.GenerateRandomNumberUsingDateTime());

            DateTime dobSetup = Convert.ToDateTime(dateOfBirth);
            DateTime dateOfAdmissionSetup = Convert.ToDateTime(DateOfAdmission);

            var learnerIdSetup = Guid.NewGuid();
            var BuildPupilRecord = this.BuildDataPackage();


            BuildPupilRecord.AddBasicLearner(learnerIdSetup, surnameSetup, forenameSetup, dobSetup, dateOfAdmissionSetup, genderCode: "1", enrolStatus: "C");

            DataSetup DataSetPupil = new DataSetup(purgeBeforeInsert: false, purgeAfterTest: true, packages: BuildPupilRecord);
            #endregion

            #region Add agent
            //DI- Agent here
            string AgentForName = Utilities.GenerateRandomString(2, "FornameData");
            string AgentSurName = Utilities.GenerateRandomString(2, "SurnameData");
            Guid AgentId;
            var Agent = this.BuildDataPackage()
                .AddData("Agent", IDCDataPackageHelper.GenerateAgent(out AgentId, AgentForName, AgentSurName));

            DataSetup setAgent = new DataSetup(Agent);

            #endregion

            #region Agency DI
            //DI- Agency here
            string AgencyName = Utilities.GenerateRandomString(5, "Selenium");
            Guid AgencyId;
            var Agency = this.BuildDataPackage()
                .AddData("Agency", IDCDataPackageHelper.GenerateAgency(out AgencyId, AgencyName));

            DataSetup setAgency = new DataSetup(Agency);

            #endregion

            #region Pre-condition

            Wait.WaitLoading();
            AutomationSugar.NavigateMenu("Tasks", "Communications", "Agents");
            var agentDetailTriplet = new AgentDetailTriplet();
            agentDetailTriplet.SearchCriteria.AgentName = String.Format("{0}, {1}", AgentSurName, AgentForName);
            agentDetailTriplet.SearchCriteria.Search();
            SeleniumHelper.Sleep(2);
            var searchResult = agentDetailTriplet.SearchCriteria.Search().FirstOrDefault(x => x.Name.Equals(String.Format("{0}, {1}", AgentSurName, AgentForName)));
            var agentDetailPage = searchResult == null ? new AgentDetailPage() : searchResult.Click<AgentDetailPage>();

            // Enter service detail
            agentDetailPage.ServiceProvide = serviceProvide;

            // Save values
            agentDetailPage.Save();


            // Add new linked agency
            var linkedAgencyDialog = agentDetailPage.AddLinkAgencies();
            linkedAgencyDialog.SearchCriteria.AgencyName = AgencyName;
            linkedAgencyDialog.SearchCriteria.Search();

            IWebElement elt = SeleniumHelper.Get(By.CssSelector("a[title='Agency ']"));
            SeleniumHelper.Get(By.CssSelector("a[title='Agency ']")).Click();
            Wait.WaitForAjaxReady(By.CssSelector(".locking-mask-loading"));

            // Click OK
            linkedAgencyDialog.ClickOk();
            //  Wait.WaitForAjaxReady(By.CssSelector(".locking-mask-loading"));

            // Click add new address
            var addAddressDialog = agentDetailPage.AddNewAddress();
            //addAddressDialog.SearchCriteria.FullPostCode = postcode;
            //addAddressDialog.SearchCriteria.Search().FirstOrDefault(x => x.Name.Equals(address)).Click();
            addAddressDialog.PostCode = "BT57 8RR";
            //click find address
            addAddressDialog.ClickManualAddAddress();
            // Click Ok
            addAddressDialog.ClickOk();


            //// Add phone number
            //IWebElement contactDetailsHeader = SeleniumHelper.FindElement(By.CssSelector(SeleniumHelper.AutomationId("section_menu_Contact Details")));
            //SeleniumHelper.ClickByJS(contactDetailsHeader);
            //IWebElement addTelephoneNumberHeader = SeleniumHelper.FindElement(By.CssSelector(SeleniumHelper.AutomationId("add_telephone_number_button")));
            //SeleniumHelper.ClickByJS(addTelephoneNumberHeader);
            //SeleniumHelper.Sleep(2);

            //var telephoneNumberTable = agentDetailPage.TelephoneNumberTable;
            //var telephoneNumberRow = telephoneNumberTable.GetLastRow();
            //telephoneNumberRow.Number = telephoneNumber;
            //telephoneNumberRow = telephoneNumberTable.GetLastInsertedRow();
            //telephoneNumberRow.Location = location;
            //telephoneNumberRow.MainNumber = true;
            //IWebElement add_note_button = SeleniumHelper.FindElement(By.CssSelector(SeleniumHelper.AutomationId("add_note_button")));
            //SeleniumHelper.ClickByJS(add_note_button);
            //telephoneNumberRow.Note = note;

            //// Add Email address
            //IWebElement addEmailAddressHeader = SeleniumHelper.FindElement(By.CssSelector(SeleniumHelper.AutomationId("add_email_address_button")));
            //SeleniumHelper.ClickByJS(addEmailAddressHeader);

            //var emailAddressTable = agentDetailPage.EmailAddressTable;
            //var emailAddressRow = emailAddressTable.GetLastRow();
            //emailAddressRow.Email = emailAddress;
            //emailAddressRow = emailAddressTable.GetLastInsertedRow();
            //emailAddressRow.MainEmail = true;

            //IWebElement agentEmails = SeleniumHelper.FindElement(By.CssSelector("[data-maintenance-container='AgentEmails']"));

            //IWebElement emailNoteElement = agentEmails.FindElement(By.CssSelector(SeleniumHelper.AutomationId("add_note_button")));
            //SeleniumHelper.ClickByJS(emailNoteElement);
            //emailAddressRow.Note = emailNote;

            /* Confirmed by Adiganesh.Kannathason@capita.co.uk, Thursday, October 29, 2015 4:06 PM, 
             * Notes and Documents functionality is revamped and it has not yet implemented the same in Communication module screens.
             * The stories are identified and are raised, So I skip upload document tests
            /*
             
            /*
            // Upload Document
            agentDetailPage.Refresh();
            var documentTable = agentDetailPage.NoteAndDocumentTable;

            // Enter Description and click upload button
            var documentRow = documentTable.GetLastRow();
            documentRow.Summary = documentSummary;
            documentRow.Note = documentNote;
            var viewDocumentDialog = documentRow.AddDocument();

            // Click add document
            var attachmentDialog = viewDocumentDialog.ClickAddAttachment();

            // Click browser
            attachmentDialog.BrowserToDocument();

            // Click upload document
            viewDocumentDialog = attachmentDialog.UploadDocument();
            viewDocumentDialog.ClickOk();
            */

            // Save values 
            //  agentDetailPage.Refresh();
            agentDetailPage.Save();

            // Assert message displays
            //Assert.AreEqual(true, agentDetailPage.IsMessageSuccessAppear(), "Message success does not display. Save failed.");

            // Assign new agent for the pupil in Sen record page
            // Can not open 'Add Agent' in Sen record, so this script is implemented later


            agentDetailPage.Refresh();
            //agentDetailPage.Save();
            #endregion

            #region Steps

            // Search agent again

            var searchResults = agentDetailTriplet.SearchCriteria.Search().FirstOrDefault(x => x.Name.Equals(String.Format("{0}, {1}", AgentSurName, AgentForName)));
            agentDetailPage.ScrollToBasicDetail();

            // Amend surname and forename
            WebContext.WebDriver.FindElement(By.Name("LegalForename")).ClearText();
            WebContext.WebDriver.FindElement(By.Name("LegalForename")).SendKeys(newForeName);
            
            WebContext.WebDriver.FindElement(By.Name("LegalSurname")).ClearText();
            WebContext.WebDriver.FindElement(By.Name("LegalSurname")).SendKeys(newSurName);
            
            //agentDetailPage.ForeName = newForeName;
            //agentDetailPage.SurName = newSurName;

            // Amend service provide
            agentDetailPage.ServiceProvide = newServiceProvider;

            // Amend Linked Agency
            var linkedAgencyTable = agentDetailPage.LinkedAgenciesTable;
            linkedAgencyTable.DeleteRowIfExist(linkedAgencyTable.Rows.FirstOrDefault(x => x.Agency.Equals(AgencyName)));

            // VP : Row is no longer available
            Assert.AreEqual(null, linkedAgencyTable.Rows.FirstOrDefault());

            // Amend Telephone number
            //agentDetailPage.ScrollToContactInformation();
            //telephoneNumberTable = agentDetailPage.TelephoneNumberTable;
            //telephoneNumberRow = telephoneNumberTable.Rows.FirstOrDefault(x => x.Number.Equals(telephoneNumber));
            //telephoneNumberRow.Number = newTelephoneNumber;
            //telephoneNumberTable.Refresh();
            //telephoneNumberRow = telephoneNumberTable.Rows.FirstOrDefault(x => x.Number.Equals(newTelephoneNumber));
            //telephoneNumberRow.Location = newTelephoneLocation;
            //telephoneNumberTable.Refresh();
            //telephoneNumberRow.MainNumber = newIsMainNumber;
            //telephoneNumberTable.Refresh();
            //telephoneNumberRow = telephoneNumberTable.Rows.FirstOrDefault(x => x.Number.Equals(newTelephoneNumber));

            //emailAddressTable = agentDetailPage.EmailAddressTable;
            //emailAddressRow = emailAddressTable.GetLastRow();
            //emailAddressRow.Email = newEmailAddress;
            //emailAddressRow = emailAddressTable.GetLastInsertedRow();
            //emailAddressRow.MainEmail = newIsMainEmail;

            //emailAddressTable = agentDetailPage.EmailAddressTable;
            //emailAddressRow = emailAddressTable.GetLastRow();
            //emailAddressRow.Email = emailAddress;
            //emailAddressRow = emailAddressTable.GetLastInsertedRow();
            //emailAddressRow.MainEmail = true;

            //SeleniumHelper.ClickByJS(emailNoteElement);
            //emailAddressRow.Note = emailNote;

            //// Amend Email address
            //emailAddressTable = agentDetailPage.EmailAddressTable;
            //emailAddressRow = emailAddressTable.Rows.FirstOrDefault(x => x.Email.Equals(emailAddress));
            //emailAddressRow.Email = newEmailAddress;
            //emailAddressTable.Refresh();
            //emailAddressRow = emailAddressTable.Rows.FirstOrDefault(x => x.Email.Equals(newEmailAddress));
            //emailAddressRow.MainEmail = newIsMainEmail;
            //emailAddressTable.Refresh();
            //emailAddressRow = emailAddressTable.Rows.FirstOrDefault(x => x.Email.Equals(newEmailAddress));
            //emailAddressRow.Note = newEmailNote;

            // Amend Address
            var editAddressDialog = agentDetailPage.EditAddress();
            editAddressDialog.BuidingNo = newBuildingNo;
            editAddressDialog.BuildingName = newBuildingName;
            editAddressDialog.ClickOk();

            /* Confirmed by Adiganesh.Kannathason@capita.co.uk, Thursday, October 29, 2015 4:06 PM, 
             * Notes and Documents functionality is revamped and it has not yet implemented the same in Communication module screens.
             * The stories are identified and are raised, So I skip upload document tests
            /*
             
            /*
            // Amend document
            agentDetailPage.Refresh();
            documentTable = agentDetailPage.NoteAndDocumentTable;
            documentRow = documentTable.Rows.FirstOrDefault(x => x.Summary.Equals(documentSummary));
            documentRow.Summary = newDocSummary;
            documentRow.Note = newDocNote;
            */

            // Save values
            agentDetailPage.Save();

            // Search agent again
            agentDetailTriplet = new AgentDetailTriplet();
            agentDetailTriplet.SearchCriteria.AgentName = String.Format("{0}, {1}", newSurName, newForeName);
            agentDetailTriplet.SearchCriteria.Search();
            agentDetailPage = agentDetailTriplet.SearchCriteria.Search().FirstOrDefault(x => x.Name.Equals(String.Format("{0}, {1}", newSurName, newForeName))).Click<AgentDetailPage>();
            agentDetailPage.ScrollToBasicDetail();

            // Verify values is amended.
            // VP : Surname and forename
            Assert.AreEqual(newForeName, agentDetailPage.ForeName, "Forename is not updated");
            Assert.AreEqual(newSurName, agentDetailPage.SurName, "Surname is not updated");

            // VP : Service provided
            Assert.AreEqual(true, agentDetailPage.ServiceProvide.Contains(newServiceProvider), "Service provide is not updated");

            // VP : Linked Agency
            linkedAgencyTable = agentDetailPage.LinkedAgenciesTable;
            Assert.AreEqual(null, linkedAgencyTable.Rows.FirstOrDefault(x => x.Agency.Equals(AgencyName)), "Linked Agency is not updated");

            // VP: Telephone number
            //agentDetailPage.ScrollToContactInformation();
            //telephoneNumberTable = agentDetailPage.TelephoneNumberTable;
            //Assert.AreNotEqual(null, telephoneNumberTable.Rows.FirstOrDefault(x => x.Number.Equals(newTelephoneNumber) && x.Location.Equals(newTelephoneLocation)
            //    && x.MainNumber.Equals(newIsMainNumber)), "Telephone number is not updated");

            // VP : Email address
            //emailAddressTable = agentDetailPage.EmailAddressTable;
            //Assert.AreNotEqual(null, emailAddressTable.Rows.FirstOrDefault(x => x.Email.Equals(newEmailAddress) && x.MainEmail.Equals(newIsMainEmail)
            //    && x.Note.Equals(newEmailNote)), "Email is not updated");

            // VP : Address
            //Assert.AreEqual(String.Format(address.Replace(address.Split(' ')[1], newBuildingNo).Replace(address.Split(' ')[2], newBuildingName) + " {0}", country), agentDetailPage.Address.Replace("\r\n", " ").Replace(",", ""), "Address is not updated");

            /* Confirmed by Adiganesh.Kannathason@capita.co.uk, Thursday, October 29, 2015 4:06 PM, 
             * Notes and Documents functionality is revamped and it has not yet implemented the same in Communication module screens.
             * The stories are identified and are raised, So I skip upload document tests
            /*
             
            /*
            // VP : Document sumary and note is changed.
            documentTable = agentDetailPage.NoteAndDocumentTable;
            Assert.AreNotEqual(null, documentTable.Rows.FirstOrDefault(x => x.Summary.Equals(newDocSummary)), "Document summary is not updated");
            Assert.AreNotEqual(null, documentTable.Rows.FirstOrDefault(x => x.Note.Equals(newDocNote)), "Document note is not updated");
            */

            //// Navigate to Sen record
            //AutomationSugar.NavigateMenu("Tasks", "Pupils", "SEN Records");

            //// Search pupil name
            //var senRecordTriplet = new SenRecordTriplet();
            //senRecordTriplet.SearchCriteria.Name = String.Format("{0}, {1}", pupilSurName, pupilForeName);
            //senRecordTriplet.SearchCriteria.NoSenStageAssigned = true;
            //var senRecordPage = senRecordTriplet.SearchCriteria.Search().FirstOrDefault(x => x.Name.Equals(String.Format("{0}, {1}", pupilSurName, pupilForeName))).Click<SenRecordDetailPage>();

            //// Navigate to Linked pupil and organisation
            //var linkedPupilPage = SeleniumHelper.NavigateViaAction<LinkedPeopleAndOrganisationPage>("Linked People and Organisations");

            //// Click Add Agent
            //Assert.AreEqual(true, false, "Can not navigate to 'Add Agent' dialog. An unexpected dialog displays.");

            /* Because of Bug #1, Unexpected Dialog appears, window 'Link an Agent or Agency' can not open
             * so I can not implement the steps on 'Link an Agent or Agency' window. The missing steps :
                1. Click Button 'Search'
                2. Select Agent (use one has just been created)
                3. Click Button 'Ok'
             */

            #endregion

            #region Post condition

            // Search agent again
            agentDetailTriplet = new AgentDetailTriplet();
            agentDetailTriplet.SearchCriteria.AgentName = String.Format("{0}, {1}", newSurName, newForeName);
            agentDetailPage = agentDetailTriplet.SearchCriteria.Search().FirstOrDefault(x => x.Name.Equals(String.Format("{0}, {1}", newSurName, newForeName))).Click<AgentDetailPage>();

            // Delete agent
            agentDetailPage.Delete();

            // Delete a pupil
            AutomationSugar.NavigateMenu("Tasks", "Pupils", "Delete Pupil");
            var deletePupilRecordTriplet = new DeletePupilRecordTriplet();
            deletePupilRecordTriplet.SearchCriteria.PupilName = String.Format("{0}, {1}", pupilSurName, pupilForeName);
            var resultPupils = deletePupilRecordTriplet.SearchCriteria.Search();
            var pupilSearchTile = resultPupils.SingleOrDefault(t => t.Name.Equals(String.Format("{0}, {1}", pupilSurName, pupilForeName)));
            var deletePupilRecord = pupilSearchTile == null ? new DeletePupilRecordPage() : pupilSearchTile.Click<DeletePupilRecordPage>();
            deletePupilRecord.Delete();

            #endregion

        }


        /// <summary>
        /// TC_COM17
        /// Au : Hieu Pham
        /// Description: Record details of a new agent
        /// Role: SEN co-orninator
        /// Status: Pending by Bug #2 : Can not add document for an agent
        /// </summary>
        [WebDriverTest(TimeoutSeconds = 2000, Enabled = true, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { "g1" }, DataProvider = "TC_COM17_DATA")]
        public void TC_COM17_Record_Detail_New_Agent(string surName, string foreName, string serviceProvide, string agency, string telephoneNumber,
            string location, bool isMainNumber, string note, string emailAddress, bool isMainEmail, string emailNote, string postcode, string address,
            string documentSummary, string documentNote, string country)
        {

            #region Steps

            // Login
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.TestUser);

            #region Agency DI
            //DI- Agency here
            string AgencyName = agency; // Utilities.GenerateRandomString(5, "Selenium");
            Guid AgencyId;
            var Agency = this.BuildDataPackage()
                .AddData("Agency", IDCDataPackageHelper.GenerateAgency(out AgencyId, AgencyName));

            DataSetup setAgency = new DataSetup(Agency);

            #endregion

            // Navigate to Agent
            AutomationSugar.NavigateMenu("Tasks", "Communications", "Agents");

            // Search old data and delete if existed
            var agentDetailTriplet = new AgentDetailTriplet();
            agentDetailTriplet.SearchCriteria.AgentName = String.Format("{0}, {1}", surName, foreName);
            var searchResult = agentDetailTriplet.SearchCriteria.Search().FirstOrDefault(x => x.Name.Equals(String.Format("{0}, {1}", surName, foreName)));
            var agentDetailPage = searchResult == null ? new AgentDetailPage() : searchResult.Click<AgentDetailPage>();
            agentDetailPage.Delete();

            // Create new Agent
            agentDetailTriplet = new AgentDetailTriplet();
            var addNewDialog = agentDetailTriplet.AddNewAgent();

            // Enter surname and forename
            addNewDialog.ForeName = foreName;
            addNewDialog.SurName = surName;

            // Click Continue
            addNewDialog.ClickContinue();
            //addNewDialog.ClickContinue();
            SeleniumHelper.Sleep(1);
            //var serviceProvideDialog = new ServiceProvideDialog();

            // Enter service detail
            var _serviceType = SeleniumHelper.Get(POM.Helper.SimsBy.AutomationId(serviceProvide));
            _serviceType.Set(true);

            //Save Agent Record.
            SeleniumHelper.FindElement(By.CssSelector(SeleniumHelper.AutomationId("well_know_action_save"))).Click();
            Wait.WaitForAjaxReady(By.CssSelector(".locking-mask-loading"));

            // Add new linked agency
            var linkedAgencyDialog = agentDetailPage.AddLinkAgencies();
            //linkedAgencyDialog.SearchCriteria.AgencyName = agency;

            linkedAgencyDialog.SearchCriteria.Search();
            IWebElement resultTile = SeleniumHelper.FindElement(By.CssSelector(SeleniumHelper.AutomationId("resultTile")));
            SeleniumHelper.ClickByJS(resultTile);
            //Wait.WaitForElementDisplayed(By.CssSelector(SeleniumHelper.AutomationId("Agency")));

            // Click OK
            SeleniumHelper.Sleep(2);
            linkedAgencyDialog.ClickOk();

            // Click add new address
            var addAddressDialog = agentDetailPage.AddNewAddress();
            //addAddressDialog.SearchCriteria.FullPostCode = postcode;
            //addAddressDialog.SearchCriteria.Search().FirstOrDefault(x => x.Name.Equals(address)).Click();
            addAddressDialog.PostCode = "BT57 8RR";
            //click find address
            addAddressDialog.ClickManualAddAddress();
            // Click Ok
            addAddressDialog.ClickOk();

            // Add phone number
            IWebElement contactDetailsHeader = SeleniumHelper.FindElement(By.CssSelector(SeleniumHelper.AutomationId("section_menu_Contact Details")));
            SeleniumHelper.ClickByJS(contactDetailsHeader);
            IWebElement addTelephoneNumberHeader = SeleniumHelper.FindElement(By.CssSelector(SeleniumHelper.AutomationId("add_telephone_number_button")));
            SeleniumHelper.ClickByJS(addTelephoneNumberHeader);
            SeleniumHelper.Sleep(2);

            var telephoneNumberTable = agentDetailPage.TelephoneNumberTable;
            var telephoneNumberRow = telephoneNumberTable.GetLastRow();
            telephoneNumberRow.Number = telephoneNumber;
            telephoneNumberRow = telephoneNumberTable.GetLastInsertedRow();
            telephoneNumberRow.Location = location;
            telephoneNumberRow.MainNumber = true;

            // Add Email address
            IWebElement addEmailAddressHeader = SeleniumHelper.FindElement(By.CssSelector(SeleniumHelper.AutomationId("add_email_address_button")));
            SeleniumHelper.ClickByJS(addEmailAddressHeader);

            var emailAddressTable = agentDetailPage.EmailAddressTable;
            var emailAddressRow = emailAddressTable.GetLastRow();
            emailAddressRow.Email = emailAddress;
            emailAddressRow = emailAddressTable.GetLastInsertedRow();
            emailAddressRow.MainEmail = true;

            IWebElement agentEmails = SeleniumHelper.FindElement(By.CssSelector("[data-maintenance-container='AgentEmails']"));

            #region comments
            /* Confirmed by Adiganesh.Kannathason@capita.co.uk, Thursday, October 29, 2015 4:06 PM, 
             * Notes and Documents functionality is revamped and it has not yet implemented the same in Communication module screens.
             * The stories are identified and are raised, So I skip upload document tests
            /*
             
            /*
            // Upload Document
            agentDetailPage.Refresh();
            var documentTable = agentDetailPage.NoteAndDocumentTable;

            // Enter Description and click upload button
            var documentRow = documentTable.GetLastRow();
            documentRow.Summary = documentSummary;
            documentRow.Note = documentNote;
            var viewDocumentDialog = documentRow.AddDocument();

            // Click add document
            var attachmentDialog = viewDocumentDialog.ClickAddAttachment();

            // Click browser
            attachmentDialog.BrowserToDocument();

            // Click upload document
            viewDocumentDialog = attachmentDialog.UploadDocument();
            viewDocumentDialog.ClickOk();
            */
            #endregion
            // Save values 
            agentDetailPage.Refresh();
            agentDetailPage.Save();

            // Assert message displays
            //Assert.AreEqual(true, agentDetailPage.IsMessageSuccessAppear(), "Message success does not display. Save failed.");

            // Search agent again
            agentDetailTriplet = new AgentDetailTriplet();
            agentDetailTriplet.SearchCriteria.AgentName = String.Format("{0}, {1}", surName, foreName);
            agentDetailPage = agentDetailTriplet.SearchCriteria.Search().FirstOrDefault(x => x.Name.Equals(String.Format("{0}, {1}", surName, foreName))).Click<AgentDetailPage>();
            agentDetailPage.ScrollToBasicDetail();

            // Verify value is correct.
            // VP : Surname and forename
            Assert.AreEqual(surName, agentDetailPage.SurName, "Surname is not correct.");
            Assert.AreEqual(foreName, agentDetailPage.ForeName, "Forename is not correct.");

            // VP : Service provide
            Assert.AreEqual(true, agentDetailPage.GetCheckedServiceProvide().Any(x => x.Equals(serviceProvide)), "Service provide is not correct");

            // VP : Linked Agency
            var linkedAgencyTable = agentDetailPage.LinkedAgenciesTable;
            //Assert.AreNotEqual(null, linkedAgencyTable.Rows.FirstOrDefault(x => x.Agency.Equals(agency)), "Linked Agency is not correct");

            // VP: Telephone number
            agentDetailPage.ScrollToContactInformation();
            telephoneNumberTable.Refresh();
            Assert.AreNotEqual(null, telephoneNumberTable.Rows.FirstOrDefault(x => x.Number.Equals(telephoneNumber) && x.Location.Equals(location)
                && x.MainNumber.Equals(isMainNumber)), "Telephone number is not correct");

            // VP: Email Address
            emailAddressTable.Refresh();
            Assert.AreNotEqual(null, emailAddressTable.Rows.FirstOrDefault(x => x.Email.Equals(emailAddress) && x.MainEmail.Equals(isMainEmail)), "Email address is not correct");

            // VP : Address
            //Assert.AreEqual(String.Format(address + " {0}", country), agentDetailPage.Address.Replace("\r\n", " ").Replace(",", ""), "Address is not correct");
            //String addr = agentDetailPage.Address.Replace("\r\n", " ").Replace(",", "");
            //Assert.AreEqual(address +" United Kingdom", agentDetailPage.Address.Replace("\r\n", " ").Replace(",", ""), "Address is not correct");

            /* Confirmed by Adiganesh.Kannathason@capita.co.uk, Thursday, October 29, 2015 4:06 PM, 
             * Notes and Documents functionality is revamped and it has not yet implemented the same in Communication module screens.
             * The stories are identified and are raised, So I skip upload document tests
            /*
             
            /*
            // VP: Document
            documentTable = agentDetailPage.NoteAndDocumentTable;
            documentRow = documentTable.Rows.FirstOrDefault(x => x.Summary.Equals(documentSummary));

            // Click add document button
            viewDocumentDialog = documentRow.AddDocument();

            // VP: Document is displayed
            Assert.AreEqual(true, viewDocumentDialog.IsDocumentDisplayed("document.txt"), "Document is not uploaded");
            */

            #endregion

            #region Post condition

            // Search agent again
            agentDetailTriplet = new AgentDetailTriplet();
            agentDetailTriplet.SearchCriteria.AgentName = String.Format("{0}, {1}", surName, foreName);
            agentDetailPage = agentDetailTriplet.SearchCriteria.Search().FirstOrDefault(x => x.Name.Equals(String.Format("{0}, {1}", surName, foreName))).Click<AgentDetailPage>();

            // Delete agent
            agentDetailPage.Delete();

            #endregion

        }

        /// <summary>
        /// TC_COM18
        /// Au : Hieu Pham
        /// Description: Ensure the new Agent can be associated against a pupil's SEN record
        /// Role: SEN co-orninator
        /// Status: Pending by Bug #1 : Can not add new Linked Organisations
        /// </summary>
        [WebDriverTest(TimeoutSeconds = 2000, Enabled = true, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { "g1" }, DataProvider = "TC_COM18_DATA")]
        public void TC_COM18_Associate_Agent_Against(string surName, string foreName, string serviceProvide, string agency, string telephoneNumber,
            string location, bool isMainNumber, string note, string emailAddress, bool isMainEmail, string emailNote, string pupilSurName, string pupilForeName,
            string gender, string dateOfBirth, string DateOfAdmission, string yearGroup)
        {

            #region Pre-Conditions:

            // Login
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);

            // Navigate to Agent
            AutomationSugar.NavigateMenu("Tasks", "Communications", "Agents");

            // Search old data and delete if existed
            var agentDetailTriplet = new AgentDetailTriplet();
            agentDetailTriplet.SearchCriteria.AgentName = String.Format("{0}, {1}", surName, foreName);
            var searchResult = agentDetailTriplet.SearchCriteria.Search().FirstOrDefault(x => x.Name.Equals(String.Format("{0}, {1}", surName, foreName)));
            var agentDetailPage = searchResult == null ? new AgentDetailPage() : searchResult.Click<AgentDetailPage>();
            agentDetailPage.Delete();

            // Create new Agent
            agentDetailTriplet = new AgentDetailTriplet();
            var addNewDialog = agentDetailTriplet.AddNewAgent();

            // Enter surname and forename
            addNewDialog.ForeName = foreName;
            addNewDialog.SurName = surName;

            // Click Continue
            addNewDialog.ClickContinue();
            //var serviceProvideDialog = new ServiceProvideDialog();

            // Enter service detail
            var _serviceType = SeleniumHelper.Get(POM.Helper.SimsBy.AutomationId(serviceProvide));
            _serviceType.Set(true);

            //Save Agent Record.
            SeleniumHelper.FindElement(By.CssSelector(SeleniumHelper.AutomationId("well_know_action_save"))).Click();
            Wait.WaitForAjaxReady(By.CssSelector(".locking-mask-loading"));

            // Add new linked agency
            var linkedAgencyDialog = agentDetailPage.AddLinkAgencies();
            linkedAgencyDialog.SearchCriteria.AgencyName = agency;

            linkedAgencyDialog.SearchCriteria.Search();
            IWebElement resultTile = SeleniumHelper.FindElement(By.CssSelector(SeleniumHelper.AutomationId("resultTile")));
            SeleniumHelper.ClickByJS(resultTile);
            Wait.WaitForElementDisplayed(By.CssSelector(SeleniumHelper.AutomationId("Agency")));

            // Click OK
            SeleniumHelper.Sleep(2);
            linkedAgencyDialog.ClickOk();

            // Add phone number
            IWebElement contactDetailsHeader = SeleniumHelper.FindElement(By.CssSelector(SeleniumHelper.AutomationId("section_menu_Contact Details")));
            SeleniumHelper.ClickByJS(contactDetailsHeader);
            IWebElement addTelephoneNumberHeader = SeleniumHelper.FindElement(By.CssSelector(SeleniumHelper.AutomationId("add_telephone_number_button")));
            SeleniumHelper.ClickByJS(addTelephoneNumberHeader);
            SeleniumHelper.Sleep(2);

            var telephoneNumberTable = agentDetailPage.TelephoneNumberTable;
            var telephoneNumberRow = telephoneNumberTable.GetLastRow();
            telephoneNumberRow.Number = telephoneNumber;
            telephoneNumberRow = telephoneNumberTable.GetLastInsertedRow();
            telephoneNumberRow.Location = location;
            telephoneNumberRow.MainNumber = true;

            // Add Email address
            IWebElement addEmailAddressHeader = SeleniumHelper.FindElement(By.CssSelector(SeleniumHelper.AutomationId("add_email_address_button")));
            SeleniumHelper.ClickByJS(addEmailAddressHeader);

            var emailAddressTable = agentDetailPage.EmailAddressTable;
            var emailAddressRow = emailAddressTable.GetLastRow();
            emailAddressRow.Email = emailAddress;
            emailAddressRow = emailAddressTable.GetLastInsertedRow();
            emailAddressRow.MainEmail = true;

            IWebElement agentEmails = SeleniumHelper.FindElement(By.CssSelector("[data-maintenance-container='AgentEmails']"));
            // Save values 
            agentDetailPage.Refresh();
            agentDetailPage.Save();

            // Navigate to Pupil Record
            AutomationSugar.NavigateMenu("Tasks", "Pupils", "Pupil Records");

            var pupilRecordTriplet = new PupilRecordTriplet();
            var addNewPupilDialog = pupilRecordTriplet.AddNewPupil();

            // Enter values
            addNewPupilDialog.Forename = pupilForeName;
            addNewPupilDialog.SurName = pupilSurName;
            addNewPupilDialog.Gender = gender;

            addNewPupilDialog.DateOfBirth = dateOfBirth;
            var registrationDetailDialog = addNewPupilDialog.Continue();
            registrationDetailDialog.DateOfAdmission = DateOfAdmission;        
            registrationDetailDialog.YearGroup = yearGroup;
            registrationDetailDialog.CreateRecord();

            // Confirm create new pupil
            //var confirmRequiredDialog = new ConfirmRequiredDialog();
            //confirmRequiredDialog.ClickOk();
            SeleniumHelper.FindElement(By.CssSelector("[data-automation-id='ok_button']")).Click(); ;

            // Save values
            var pupilRecord = PupilRecordPage.Create();
            pupilRecord.SavePupil();

            // Log out
            SeleniumHelper.Logout();

            #endregion

            #region Steps

            // Login 
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SENCoordinator);

            //// Navigate to Sen record
            SeleniumHelper.NavigateBySearch("SEN Records", true);

            // Search pupil name
            var senRecordTriplet = new SenRecordTriplet();
            senRecordTriplet.SearchCriteria.Name = String.Format("{0}, {1}", pupilSurName, pupilForeName);
            senRecordTriplet.SearchCriteria.NoSenStageAssigned = true;
             var senRecordPage = senRecordTriplet.SearchCriteria.Search().FirstOrDefault(x => x.Name.Equals(String.Format("{0}, {1}", pupilSurName, pupilForeName))).Click<SenRecordDetailPage>();

            // Navigate to Linked pupil and organisation
            //var linkedPupilPage = SeleniumHelper.NavigateViaAction<LinkedPeopleAndOrganisationPage>("Linked People and Organisations");

            // Click Add Agent
            //Assert.AreEqual(true, false, "Can not navigate to 'Add Agent' dialog. An unexpected dialog displays.");

            /* Because of Bug #1, Unexpected Dialog appears, window 'Link an Agent or Agency' can not open
             * so I can not implement the steps on 'Link an Agent or Agency' window. The missing steps :
                1. Click Button 'Search'
                2. Select Agent (use one has just been created)
                3. Click Button 'Ok'
             */
            #endregion

            #region Post Condition
            SeleniumHelper.Sleep(3);
            SeleniumHelper.Logout();
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);
           
            // Navigate to agent page
            AutomationSugar.NavigateMenu("Tasks", "Communications", "Agents");
            // Search agent again
            agentDetailTriplet = new AgentDetailTriplet();
            agentDetailTriplet.SearchCriteria.AgentName = String.Format("{0}, {1}", surName, foreName);
            agentDetailPage = agentDetailTriplet.SearchCriteria.Search().FirstOrDefault(x => x.Name.Equals(String.Format("{0}, {1}", surName, foreName))).Click<AgentDetailPage>();

            // Delete agent
            agentDetailPage.Delete();

            // Delete a pupil
            AutomationSugar.NavigateMenu("Tasks", "Pupils", "Delete Pupil");
            var deletePupilRecordTriplet = new DeletePupilRecordTriplet();
            deletePupilRecordTriplet.SearchCriteria.PupilName = String.Format("{0}, {1}", pupilSurName, pupilForeName);
            var resultPupils = deletePupilRecordTriplet.SearchCriteria.Search();
            var pupilSearchTile = resultPupils.SingleOrDefault(t => t.Name.Equals(String.Format("{0}, {1}", pupilSurName, pupilForeName)));
            var deletePupilRecord = pupilSearchTile == null ? new DeletePupilRecordPage() : pupilSearchTile.Click<DeletePupilRecordPage>();
            deletePupilRecord.Delete();

            #endregion


        }

        [WebDriverTest(TimeoutSeconds = 2000, Enabled = true, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { "g1", "All", "P2" }, DataProvider = "TC_COM108_DATA")]
        public void TC_SearchAgentByServiceProvided(string serviceProvide)
        {
            //DI- Agent here
            string AgentForName = Utilities.GenerateRandomString(2, "FornameAgent");
            string AgentSurName = Utilities.GenerateRandomString(2, "SurnameAgent");
            Guid AgentId;
            var Agent = this.BuildDataPackage()
                .AddData("Agent", IDCDataPackageHelper.GenerateAgent(out AgentId, AgentForName, AgentSurName));

            DataSetup setAgent = new DataSetup(Agent);

            #region Pre-Conditions:

            // Login
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);

            // Navigate to Agent
            AutomationSugar.NavigateMenu("Tasks", "Communications", "Agents");

            // Search old data and delete if existed
            var agentDetailTriplet = new AgentDetailTriplet();
            agentDetailTriplet.SearchCriteria.AgentName = String.Format("{0}, {1}", AgentSurName, AgentForName);
            agentDetailTriplet.SearchCriteria.Search();
            var searchResult = agentDetailTriplet.SearchCriteria.Search().FirstOrDefault(x => x.Name.Equals(String.Format("{0}, {1}", AgentSurName, AgentForName)));

            searchResult.Click<AgentDetailPage>();
            var agentDetailPage = searchResult.Click<AgentDetailPage>();

            // Enter service detail
            agentDetailPage.ServiceProvide = serviceProvide;

            // Save values 
            agentDetailPage.Refresh();
            agentDetailPage.Save();

            #endregion

            // Search old data and delete if existed
            agentDetailTriplet = new AgentDetailTriplet();
            agentDetailTriplet.SearchCriteria.ServiceProvide = String.Format("{0}", serviceProvide);
            agentDetailTriplet.SearchCriteria.Search();
            var searchResultWithService = agentDetailTriplet.SearchCriteria.Search().Single();

            agentDetailPage = searchResultWithService.Click<AgentDetailPage>();

            Assert.True(agentDetailPage.ServiceProvide == "Doctor,");


        }

        [WebDriverTest(TimeoutSeconds = 2000, Enabled = true, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { "g1", "All", "P2" }, DataProvider = "TC_COM108_DATA")]
        public void TC_COM_Associate_Agent_Against_Agency(string serviceProvide)
        {

            //DI- Agency here
            string AgencyName = Utilities.GenerateRandomString(5, "Agency");
            Guid AgencyId;
            var Agency = this.BuildDataPackage()
                .AddData("Agency", IDCDataPackageHelper.GenerateAgency(out AgencyId, AgencyName));

            DataSetup setAgency = new DataSetup(Agency);

            //DI- Agent here
            string AgentForName = Utilities.GenerateRandomString(2, "FornameAgent");
            string AgentSurName = Utilities.GenerateRandomString(2, "SurnameAgent");
            Guid AgentId;
            var Agent = this.BuildDataPackage()
                .AddData("Agent", IDCDataPackageHelper.GenerateAgent(out AgentId, AgentForName, AgentSurName));

            DataSetup setAgent = new DataSetup(Agent);

            #region Pre-Conditions:

            // Login
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);

            // Navigate to Agent
            AutomationSugar.NavigateMenu("Tasks", "Communications", "Agents");

            // Search old data and delete if existed
            var agentDetailTriplet = new AgentDetailTriplet();
            agentDetailTriplet.SearchCriteria.AgentName = String.Format("{0}, {1}", AgentSurName, AgentForName);
            agentDetailTriplet.SearchCriteria.Search();
            var searchResult = agentDetailTriplet.SearchCriteria.Search().FirstOrDefault(x => x.Name.Equals(String.Format("{0}, {1}", AgentSurName, AgentForName)));

            searchResult.Click<AgentDetailPage>();
            var agentDetailPage = searchResult.Click<AgentDetailPage>();

            // Enter service detail
            agentDetailPage.ServiceProvide = serviceProvide;

            // Add new linked agency
            var linkedAgencyDialog = agentDetailPage.AddLinkAgencies();
            linkedAgencyDialog.SearchCriteria.AgencyName = AgencyName;
            linkedAgencyDialog.SearchCriteria.Search();
            IWebElement elt = SeleniumHelper.Get(By.CssSelector("a[title='Agency ']"));
            SeleniumHelper.Get(By.CssSelector("a[title='Agency ']")).Click();
            Wait.WaitForAjaxReady(By.CssSelector(".locking-mask-loading"));

            // Click OK
            SeleniumHelper.Sleep(2);
            linkedAgencyDialog.ClickOk();

            // Save values 
            agentDetailPage.Refresh();
            agentDetailPage.Save();

            #endregion


        }


        public List<object[]> TC_COM108_DATA()
        {



            var data = new List<Object[]>
            {
                new object[] { "Doctor" }

            };
            return data;
        }



        [WebDriverTest(TimeoutSeconds = 2000, Enabled = true, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { "g1", "All", "P2" }, DataProvider = "TC_COM108_DATA")]
        public void TC_COM_Delete_Agent(string serviceProvide)
        {


            //DI- Agent here
            string AgentForName = Utilities.GenerateRandomString(2, "FornameAgent");
            string AgentSurName = Utilities.GenerateRandomString(2, "SurnameAgent");
            Guid AgentId;
            var Agent = this.BuildDataPackage()
                .AddData("Agent", IDCDataPackageHelper.GenerateAgent(out AgentId, AgentForName, AgentSurName));

            DataSetup setAgent = new DataSetup(Agent);

            #region Pre-Conditions:

            // Login
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);

            // Navigate to Agent
            AutomationSugar.NavigateMenu("Tasks", "Communications", "Agents");


            var agentDetailTriplet = new AgentDetailTriplet();
            agentDetailTriplet.SearchCriteria.AgentName = String.Format("{0}, {1}", AgentSurName, AgentForName);
            agentDetailTriplet.SearchCriteria.Search();
            var searchResult = agentDetailTriplet.SearchCriteria.Search().FirstOrDefault(x => x.Name.Equals(String.Format("{0}, {1}", AgentSurName, AgentForName)));

            searchResult.Click<AgentDetailPage>();
            var agentDetailPage = searchResult.Click<AgentDetailPage>();

            // Enter service detail
            agentDetailPage.ServiceProvide = serviceProvide;

            // Save values 
            agentDetailPage.Refresh();
            agentDetailPage.Save();

            #endregion
            agentDetailPage.Delete();


            agentDetailTriplet.SearchCriteria.AgentName = String.Format("{0}, {1}", AgentSurName, AgentForName);
            agentDetailTriplet.SearchCriteria.Search();
            searchResult = agentDetailTriplet.SearchCriteria.Search().FirstOrDefault(x => x.Name.Equals(String.Format("{0}, {1}", AgentSurName, AgentForName)));

            Assert.True(searchResult == null);

        }


        [WebDriverTest(TimeoutSeconds = 2000, Enabled = true, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { "g1", "All", "P2" }, DataProvider = "TC_COM108_DATA")]
        public void TC_COM_Delete_Agent_With_LinkedAgency(string serviceProvide)
        {

            //DI- Agency here
            string AgencyName = Utilities.GenerateRandomString(5, "Agency");
            Guid AgencyId;
            var Agency = this.BuildDataPackage()
                .AddData("Agency", IDCDataPackageHelper.GenerateAgency(out AgencyId, AgencyName));

            DataSetup setAgency = new DataSetup(Agency);

            //DI- Agent here
            string AgentForName = Utilities.GenerateRandomString(2, "FornameAgent");
            string AgentSurName = Utilities.GenerateRandomString(2, "SurnameAgent");
            Guid AgentId;
            var Agent = this.BuildDataPackage()
                .AddData("Agent", IDCDataPackageHelper.GenerateAgent(out AgentId, AgentForName, AgentSurName));

            DataSetup setAgent = new DataSetup(Agent);

            #region Pre-Conditions:

            // Login
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);

            // Navigate to Agent
            AutomationSugar.NavigateMenu("Tasks", "Communications", "Agents");


            var agentDetailTriplet = new AgentDetailTriplet();
            agentDetailTriplet.SearchCriteria.AgentName = String.Format("{0}, {1}", AgentSurName, AgentForName);
            agentDetailTriplet.SearchCriteria.Search();
            var searchResult = agentDetailTriplet.SearchCriteria.Search().FirstOrDefault(x => x.Name.Equals(String.Format("{0}, {1}", AgentSurName, AgentForName)));

            searchResult.Click<AgentDetailPage>();
            var agentDetailPage = searchResult.Click<AgentDetailPage>();

            // Enter service detail
            agentDetailPage.ServiceProvide = serviceProvide;

            // Add new linked agency
            var linkedAgencyDialog = agentDetailPage.AddLinkAgencies();
            linkedAgencyDialog.SearchCriteria.AgencyName = AgencyName;
            linkedAgencyDialog.SearchCriteria.Search();
            IWebElement elt = SeleniumHelper.Get(By.CssSelector("a[title='Agency ']"));
            SeleniumHelper.Get(By.CssSelector("a[title='Agency ']")).Click();
            Wait.WaitForAjaxReady(By.CssSelector(".locking-mask-loading"));

            // Click OK
            SeleniumHelper.Sleep(2);
            linkedAgencyDialog.ClickOk();


            // Save values 
            agentDetailPage.Refresh();
            agentDetailPage.Save();

            #endregion
            agentDetailPage.Delete();


            agentDetailTriplet.SearchCriteria.AgentName = String.Format("{0}, {1}", AgentSurName, AgentForName);
            agentDetailTriplet.SearchCriteria.Search();
            searchResult = agentDetailTriplet.SearchCriteria.Search().FirstOrDefault(x => x.Name.Equals(String.Format("{0}, {1}", AgentSurName, AgentForName)));

            Assert.True(searchResult == null);

        }


        /// <summary>
        /// TC_COM19
        /// Au : Hieu Pham
        /// Description: Amend record details of an agent linked to a Pupil record
        /// Role: Sen co-ordinator
        /// Status: Pending by Bug #2 : Can not add document for an agent
        /// </summary>
        [WebDriverTest(TimeoutSeconds = 2000, Enabled = true, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { "g1", "All", "P2" }, DataProvider = "TC_COM19_DATA")]
        public void TC_COM19_Amend_Record_Detail(string surName, string foreName, string serviceProvide, string agency, string telephoneNumber,
            string location, bool isMainNumber, string note, string emailAddress, bool isMainEmail, string emailNote, string postcode, string address,
            string documentSummary, string documentNote, string newSurName, string newForeName, string newServiceProvider, string newAgency,
            string newTelephoneNumber, string newTelephoneLocation, bool newIsMainNumber, string newTelephoneNote, string newEmailAddress, bool newIsMainEmail,
            string newEmailNote, string newBuildingNo, string newBuildingName, string newDocSummary, string newDocNote, string country, string pupilSurName, string pupilForeName,
            string gender, string dateOfBirth, string DateOfAdmission, string yearGroup)
        {

            #region Pre-condition


            // Login
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);

            // Navigate to Pupil Record
            AutomationSugar.NavigateMenu("Tasks", "Pupils", "Pupil Records");

            var pupilRecordTriplet = new PupilRecordTriplet();
            var addNewPupilDialog = pupilRecordTriplet.AddNewPupil();

            // Enter values
            addNewPupilDialog.Forename = pupilForeName;
            addNewPupilDialog.SurName = pupilSurName;
            addNewPupilDialog.Gender = gender;
            addNewPupilDialog.DateOfBirth = dateOfBirth;
            var registrationDetailDialog = addNewPupilDialog.Continue();
            registrationDetailDialog.DateOfAdmission = DateOfAdmission;
            registrationDetailDialog.YearGroup = yearGroup;
            registrationDetailDialog.CreateRecord();

            // Confirm create new pupil
            //var confirmRequiredDialog = new ConfirmRequiredDialog();
            //confirmRequiredDialog.ClickOk();
            SeleniumHelper.FindElement(By.CssSelector("[data-automation-id='ok_button']")).Click(); ;

            // Save values
            var pupilRecord = PupilRecordPage.Create();
            pupilRecord.SavePupil();

            // Navigate to Agent
            AutomationSugar.NavigateMenu("Tasks", "Communications", "Agents");

            // Search old data and delete if existed
            var agentDetailTriplet = new AgentDetailTriplet();
            agentDetailTriplet.SearchCriteria.AgentName = String.Format("{0}, {1}", surName, foreName);
            var searchResult = agentDetailTriplet.SearchCriteria.Search().FirstOrDefault(x => x.Name.Equals(String.Format("{0}, {1}", surName, foreName)));
            var agentDetailPage = searchResult == null ? new AgentDetailPage() : searchResult.Click<AgentDetailPage>();
            agentDetailPage.Delete();

            // Create new Agent
            agentDetailTriplet = new AgentDetailTriplet();
            var addNewDialog = agentDetailTriplet.AddNewAgent();

            // Enter surname and forename
            addNewDialog.ForeName = foreName;
            addNewDialog.SurName = surName;

            // Click Continue
            addNewDialog.ClickContinue();
            
            // Enter service detail
            var _serviceType = SeleniumHelper.Get(POM.Helper.SimsBy.AutomationId(serviceProvide));
            _serviceType.Set(true);

            //Save Agent Record.
            SeleniumHelper.FindElement(By.CssSelector(SeleniumHelper.AutomationId("well_know_action_save"))).Click();
            Wait.WaitForAjaxReady(By.CssSelector(".locking-mask-loading"));


            // Add new linked agency
            var linkedAgencyDialog = agentDetailPage.AddLinkAgencies();
            //linkedAgencyDialog.SearchCriteria.AgencyName = agency;

            linkedAgencyDialog.SearchCriteria.Search();
            IWebElement resultTile = SeleniumHelper.FindElement(By.CssSelector(SeleniumHelper.AutomationId("resultTile")));
            SeleniumHelper.ClickByJS(resultTile);
            //Wait.WaitForElementDisplayed(By.CssSelector(SeleniumHelper.AutomationId("Agency")));

            // Click OK
            SeleniumHelper.Sleep(2);
            linkedAgencyDialog.ClickOk();

            // Click add new address
            var addAddressDialog = agentDetailPage.AddNewAddress();
            //addAddressDialog.SearchCriteria.FullPostCode = postcode;
            //addAddressDialog.SearchCriteria.Search().FirstOrDefault(x => x.Name.Equals(address)).Click();
            //addAddressDialog.SearchCriteria.Search().FirstOrDefault(x => x.Name.Contains(address)).Click();
            addAddressDialog.PostCode = "BT57 8RR";
            //click find address
            addAddressDialog.ClickManualAddAddress();
            // Click Ok
            addAddressDialog.ClickOk();

            // Add phone number
            IWebElement contactDetailsHeader = SeleniumHelper.FindElement(By.CssSelector(SeleniumHelper.AutomationId("section_menu_Contact Details")));
            SeleniumHelper.ClickByJS(contactDetailsHeader);
            IWebElement addTelephoneNumberHeader = SeleniumHelper.FindElement(By.CssSelector(SeleniumHelper.AutomationId("add_telephone_number_button")));
            SeleniumHelper.ClickByJS(addTelephoneNumberHeader);
            SeleniumHelper.Sleep(2);

            var telephoneNumberTable = agentDetailPage.TelephoneNumberTable;
            var telephoneNumberRow = telephoneNumberTable.GetLastRow();
            telephoneNumberRow.Number = telephoneNumber;
            telephoneNumberRow = telephoneNumberTable.GetLastInsertedRow();
            telephoneNumberRow.Location = location;
            telephoneNumberRow.MainNumber = true;
            //IWebElement add_note_button = SeleniumHelper.FindElement(By.CssSelector(SeleniumHelper.AutomationId("add_note_button")));
            //SeleniumHelper.ClickByJS(add_note_button);
            //telephoneNumberRow.Note = note;

            // Add Email address
            IWebElement addEmailAddressHeader = SeleniumHelper.FindElement(By.CssSelector(SeleniumHelper.AutomationId("add_email_address_button")));
            SeleniumHelper.ClickByJS(addEmailAddressHeader);

            var emailAddressTable = agentDetailPage.EmailAddressTable;
            var emailAddressRow = emailAddressTable.GetLastRow();
            emailAddressRow.Email = emailAddress;
            emailAddressRow = emailAddressTable.GetLastInsertedRow();
            emailAddressRow.MainEmail = true;

            IWebElement agentEmails = SeleniumHelper.FindElement(By.CssSelector("[data-maintenance-container='AgentEmails']"));

            //IWebElement emailNoteElement = agentEmails.FindElement(By.CssSelector(SeleniumHelper.AutomationId("add_note_button")));
            //SeleniumHelper.ClickByJS(emailNoteElement);
            //emailAddressRow.Note = emailNote;

            // Save values 
            agentDetailPage.Refresh();
            agentDetailPage.Save();

            // Assert message displays
            //Assert.AreEqual(true, agentDetailPage.IsMessageSuccessAppear(), "Message success does not display. Save failed.");

            // Assign new agent for the pupil in Sen record page
            // Can not open 'Add Agent' in Sen record, so this script is implemented later

            // Log out
            SeleniumHelper.Logout();


            #endregion

            #region Steps

            // Login
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);

            // Navigate to Agent
            AutomationSugar.NavigateMenu("Tasks", "Communications", "Agents");

            // Search agent again
            agentDetailTriplet = new AgentDetailTriplet();
            agentDetailTriplet.SearchCriteria.AgentName = String.Format("{0}, {1}", surName, foreName);
            agentDetailPage = agentDetailTriplet.SearchCriteria.Search().FirstOrDefault(x => x.Name.Equals(String.Format("{0}, {1}", surName, foreName))).Click<AgentDetailPage>();
            agentDetailPage.ScrollToBasicDetail();

            // Amend surname and forename
            WebContext.WebDriver.FindElement(By.Name("LegalForename")).ClearText();
            WebContext.WebDriver.FindElement(By.Name("LegalForename")).SendKeys(newForeName);

            WebContext.WebDriver.FindElement(By.Name("LegalSurname")).ClearText();
            WebContext.WebDriver.FindElement(By.Name("LegalSurname")).SendKeys(newSurName);

            // Amend service provide
            agentDetailPage.ServiceProvide = newServiceProvider;

            // Amend Linked Agency
            var linkedAgencyTable = agentDetailPage.LinkedAgenciesTable;
            linkedAgencyTable.DeleteRowIfExist(linkedAgencyTable.Rows.FirstOrDefault());

            // Amend Address
            //SeleniumHelper.FindElement(By.CssSelector("[data-automation-id='section_menu_Address']")).Click();
            var editAddressDialog = agentDetailPage.EditAddress();
            editAddressDialog.BuidingNo = newBuildingNo;
            editAddressDialog.BuildingName = newBuildingName;
            editAddressDialog.ClickOk();

            // VP : Row is no longer available
            //Assert.AreEqual(null, linkedAgencyTable.Rows.FirstOrDefault(x => x.Agency.Equals(agency)));

            // Amend Telephone number
            agentDetailPage.ScrollToContactInformation();
            telephoneNumberTable = agentDetailPage.TelephoneNumberTable;
            telephoneNumberRow = telephoneNumberTable.Rows.FirstOrDefault(x => x.Number.Equals(telephoneNumber));
            IWebElement t1 = SeleniumHelper.FindElement(By.CssSelector("[value='"+telephoneNumber+"']"));
            t1.ClearText();
            t1.SendKeys(newTelephoneNumber);
            //telephoneNumberRow.Number = newTelephoneNumber;
            telephoneNumberTable.Refresh();
            telephoneNumberRow = telephoneNumberTable.Rows.FirstOrDefault(x => x.Number.Equals(newTelephoneNumber));

            //SeleniumHelper.FindElement(By.CssSelector("[name$='.LocationType.dropdownImitator']")).SendKeys(newTelephoneLocation);
            //SeleniumHelper.Sleep(2);
            //SeleniumHelper.FindElement(By.CssSelector("[name$='.LocationType.dropdownImitator']")).SendKeys(Keys.Enter);
            //SeleniumHelper.FindElement(By.CssSelector("[name$='.LocationType.dropdownImitator']")).SendKeys(Keys.Enter);
            //telephoneNumberRow.Location = newTelephoneLocation;
            //SeleniumHelper.ClickByJS(SeleniumHelper.FindElement(By.CssSelector("[name$='.IsMainTelephone']")));
            //SeleniumHelper.FindElement(By.CssSelector("[name$='.IsMainTelephone']")).Click();
            //telephoneNumberRow.MainNumber = newIsMainNumber;
            telephoneNumberTable.Refresh();
            telephoneNumberRow = telephoneNumberTable.Rows.FirstOrDefault(x => x.Number.Equals(newTelephoneNumber));
            //telephoneNumberRow.Note = newTelephoneNote;

            // Amend Email address
            //emailAddressTable = agentDetailPage.EmailAddressTable;
            //emailAddressRow = emailAddressTable.Rows.FirstOrDefault(x => x.Email.Equals(emailAddress));
            //IWebElement e1 = SeleniumHelper.FindElement(By.CssSelector("[value='" + emailAddress + "']"));
            //e1.ClearText();
            //e1.SendKeys(newEmailAddress);
            ////emailAddressRow.Email = newEmailAddress;
            //emailAddressTable.Refresh();
            //emailAddressRow = emailAddressTable.Rows.FirstOrDefault();
            ////SeleniumHelper.FindElement(By.CssSelector("[name$='.IsMainEmail']")).Click();
            //emailAddressRow.MainEmail = newIsMainEmail;
            //emailAddressTable.Refresh();
            //emailAddressRow = emailAddressTable.Rows.FirstOrDefault(x => x.Email.Equals(newEmailAddress));
            //emailAddressRow.Note = newEmailNote;


            /* Confirmed by Adiganesh.Kannathason@capita.co.uk, Thursday, October 29, 2015 4:06 PM, 
             * Notes and Documents functionality is revamped and it has not yet implemented the same in Communication module screens.
             * The stories are identified and are raised, So I skip upload document tests
            /*
             
            /*
            // Amend document
            agentDetailPage.Refresh();
            documentTable = agentDetailPage.NoteAndDocumentTable;
            documentRow = documentTable.Rows.FirstOrDefault(x => x.Summary.Equals(documentSummary));
            documentRow.Summary = newDocSummary;
            documentRow.Note = newDocNote;
            */

            // Save values
            SeleniumHelper.Sleep(2);
            agentDetailPage.Save();

            // Search agent again
            //////agentDetailTriplet = new AgentDetailTriplet();
            //////agentDetailTriplet.SearchCriteria.AgentName = String.Format("{0}, {1}", newSurName, newForeName);
            //////agentDetailPage = agentDetailTriplet.SearchCriteria.Search().FirstOrDefault(x => x.Name.Equals(String.Format("{0}, {1}", newSurName, newForeName))).Click<AgentDetailPage>();
            //////agentDetailPage.ScrollToBasicDetail();

            //////// Verify values is amended.
            //////// VP : Surname and forename
            //////Assert.AreEqual(newForeName, agentDetailPage.ForeName, "Forename is not updated");
            //////Assert.AreEqual(newSurName, agentDetailPage.SurName, "Surname is not updated");

            //////// VP : Service provided
            //////Assert.AreEqual(true, agentDetailPage.ServiceProvide.Contains(newServiceProvider), "Service provide is not updated");

            //////// VP : Linked Agency
            //////linkedAgencyTable = agentDetailPage.LinkedAgenciesTable;
            ////////Assert.AreEqual(null, linkedAgencyTable.Rows.FirstOrDefault(x => x.Agency.Equals(agency)), "Linked Agency is not updated");

            //////// VP: Telephone number
            //////agentDetailPage.ScrollToContactInformation();
            //////telephoneNumberTable = agentDetailPage.TelephoneNumberTable;
            
            //////Assert.AreNotEqual(null, telephoneNumberTable.Rows.FirstOrDefault(x => x.Number.Equals(newTelephoneNumber) 
            //////    //&& x.Location.Equals(newTelephoneLocation)
            //////    //&& x.MainNumber.Equals(newIsMainNumber)
            //////    ), "Telephone number is not updated");

            //VP : Email address
            //emailAddressTable = agentDetailPage.EmailAddressTable;
            //Assert.AreNotEqual(null, emailAddressTable.Rows.FirstOrDefault(x => x.Email.Equals(newEmailAddress) && x.MainEmail.Equals(newIsMainEmail)), "Email is not updated");

            //VP : Address
            //string s1 = address.Replace(address.Split(' ')[1], newBuildingNo).Replace(address.Split(' ')[2], newBuildingName);
            //string s2 = agentDetailPage.Address.Replace("\r\n", " ").Replace(",", "");
            //Assert.AreEqual(address.Replace(address.Split(' ')[1], newBuildingNo).Replace(address.Split(' ')[2], newBuildingName), 
            // agentDetailPage.Address.Replace("\r\n", " ").Replace(",", ""), "Address is not updated");

            /* Confirmed by Adiganesh.Kannathason@capita.co.uk, Thursday, October 29, 2015 4:06 PM, 
             * Notes and Documents functionality is revamped and it has not yet implemented the same in Communication module screens.
             * The stories are identified and are raised, So I skip upload document tests
            /*
             
            /*
            // VP : Document sumary and note is changed.
            documentTable = agentDetailPage.NoteAndDocumentTable;
            Assert.AreNotEqual(null, documentTable.Rows.FirstOrDefault(x => x.Summary.Equals(newDocSummary)), "Document summary is not updated");
            Assert.AreNotEqual(null, documentTable.Rows.FirstOrDefault(x => x.Note.Equals(newDocNote)), "Document note is not updated");
            */

            // Navigate to Sen record
            //SeleniumHelper.NavigateBySearch("SEN Records");

            //// Search pupil name
            //var senRecordTriplet = new SenRecordTriplet();
            //senRecordTriplet.SearchCriteria.Name = String.Format("{0}, {1}", pupilSurName, pupilForeName);
            //senRecordTriplet.SearchCriteria.NoSenStageAssigned = true;
            //var senRecordPage = senRecordTriplet.SearchCriteria.Search().FirstOrDefault(x => x.Name.Equals(String.Format("{0}, {1}", pupilSurName, pupilForeName))).Click<SenRecordDetailPage>();

            //// Navigate to Linked pupil and organisation
            //var linkedPupilPage = SeleniumHelper.NavigateViaAction<LinkedPeopleAndOrganisationPage>("Linked People and Organisations");

            //// Click Add Agent
            //Assert.AreEqual(true, false, "Can not navigate to 'Add Agent' dialog. An unexpected dialog displays.");

            /* Because of Bug #1, Unexpected Dialog appears, window 'Link an Agent or Agency' can not open
             * so I can not implement the steps on 'Link an Agent or Agency' window. The missing steps :
                1. Click Button 'Search'
                2. Select Agent (use one has just been created)
                3. Click Button 'Ok'
             */
            #endregion

            #region Post condition

            // Log out
            //SeleniumHelper.Logout();

            // Login with adminstrator
            //SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);

            // Search agent again

            // Navigate to Agent
            //AutomationSugar.NavigateMenu("Tasks", "Communications", "Agents");

            agentDetailTriplet = new AgentDetailTriplet();
            agentDetailTriplet.SearchCriteria.AgentName = String.Format("{0}, {1}", newSurName, newForeName);
            agentDetailPage = agentDetailTriplet.SearchCriteria.Search().FirstOrDefault(x => x.Name.Equals(String.Format("{0}, {1}", newSurName, newForeName))).Click<AgentDetailPage>();


            // Delete agent
            agentDetailPage.Delete();

            // Delete a pupil
            AutomationSugar.NavigateMenu("Tasks", "Pupils", "Delete Pupil");
            var deletePupilRecordTriplet = new DeletePupilRecordTriplet();
            deletePupilRecordTriplet.SearchCriteria.PupilName = String.Format("{0}, {1}", pupilSurName, pupilForeName);
            var resultPupils = deletePupilRecordTriplet.SearchCriteria.Search();
            var pupilSearchTile = resultPupils.SingleOrDefault(t => t.Name.Equals(String.Format("{0}, {1}", pupilSurName, pupilForeName)));
            var deletePupilRecord = pupilSearchTile == null ? new DeletePupilRecordPage() : pupilSearchTile.Click<DeletePupilRecordPage>();
            deletePupilRecord.Delete();

            #endregion

        }

        /// <summary>
        /// Author: BaTruong
        /// Description: Amend record details of an agent created by a SEN Co-Ordinator that is linked to a Pupil record to ensure Permission allow this
        /// Status: Pending by issue: (BugID #1) Can not add new Linked Organisations
        /// </summary>
        [WebDriverTest(TimeoutSeconds = 2000, Enabled = true, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { "g1", "All", "P2" }, DataProvider = "TC_COM020_Data")]
        public void TC_COM020_Amend_Record_Details_Of_Agent_Created_By_SEN_CoOrdinator_That_Linked_To_Pupil_Record(string[] originalBasicDetails, string[] updateBasicDetails,
                                                                                                                   string[] phoneNumberInfor, string[] emailInfor,
                                                                                                                   string[] documentInfor, string pupilName)
        {
            #region PRE-CONDITION

            //Login as SEN Co-Ordinator and navigate to Agents to create an Agent
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SENCoordinator);
            AutomationSugar.NavigateMenu("Tasks", "Communications", "Agents");

            //Clean environment before creating
            var agentDetailTriplet = new AgentDetailTriplet();
            agentDetailTriplet.SearchCriteria.AgentName = originalBasicDetails[0];
            var agentTile = agentDetailTriplet.SearchCriteria.Search().FirstOrDefault(x => x.Name.Contains(originalBasicDetails[0]));
            var agentDetailPage = agentTile != null ? agentTile.Click<AgentDetailPage>() : new AgentDetailPage();
            agentDetailPage.Delete();

            //Create new agent
            var addNewAgentDialog = agentDetailTriplet.AddNewAgent();
            addNewAgentDialog.ForeName = originalBasicDetails[0];
            addNewAgentDialog.SurName = originalBasicDetails[1];

            addNewAgentDialog.ClickContinue();
            SeleniumHelper.Sleep(1);

            // Enter service detail
            var _serviceType = SeleniumHelper.Get(POM.Helper.SimsBy.AutomationId(originalBasicDetails[2]));
            _serviceType.Set(true);

            //Save Agent Record.
            SeleniumHelper.FindElement(By.CssSelector(SeleniumHelper.AutomationId("well_know_action_save"))).Click();
            Wait.WaitForAjaxReady(By.CssSelector(".locking-mask-loading"));

            //Navigate to SEN Record to create Linked Agent
            SeleniumHelper.NavigateBySearch("SEN Records", true);
            var senRecordTriplet = new SenRecordTriplet();
            senRecordTriplet.SearchCriteria.Name = pupilName;
            var recordTile = senRecordTriplet.SearchCriteria.Search().FirstOrDefault(x => x.Name.Equals(pupilName));
            var senRecordDetailPage = recordTile.Click<SenRecordDetailPage>();

            //var linkedPeoplePage = SeleniumHelper.NavigateViaAction<LinkedPeopleAndOrganisationPage>("Linked People and Organisations");
            /* Because of Bug #1, Unexpected Dialog appears, window 'Link an Agent or Agency' can not open
             * so I can not implement the steps on 'Link an Agent or Agency' window to create a linked people and organisation for the agent.
             * The missing steps :
                1. Click Button 'Search'
                2. Select Agent (use one has just been created)
                3. Click Button 'Ok'
             */
            //linkedPeoplePage.Save();

            //Logout SEN Co-Ordinator
            SeleniumHelper.Logout();

            #endregion

            #region TEST STEPS

            //Login as SchoolAdministrator and navigate to Tasks->Communication->Agent
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);

            AutomationSugar.NavigateMenu("Tasks", "Communications", "Agents");

            //Click button 'Search'and select Agent is created by SEN Co-ordinator
            agentDetailTriplet = new AgentDetailTriplet();
            agentDetailTriplet.SearchCriteria.AgentName = originalBasicDetails[0];
            agentTile = agentDetailTriplet.SearchCriteria.Search().FirstOrDefault(x => x.Name.Contains(originalBasicDetails[0]));
            agentDetailPage = agentTile.Click<AgentDetailPage>();

            //Update information for Agent In frame 'Basic Details'
            SeleniumHelper.FindElement(By.Name("LegalForename")).ClearText();
            SeleniumHelper.FindElement(By.Name("LegalForename")).SendKeys(updateBasicDetails[0]);
            SeleniumHelper.FindElement(By.Name("LegalSurname")).ClearText();
            SeleniumHelper.FindElement(By.Name("LegalSurname")).SendKeys(updateBasicDetails[1]);
            agentDetailPage.ServiceProvide = updateBasicDetails[2];

            //Scroll to frame 'Contact Information' to update information in frame 'Contact Information' 
            agentDetailPage.ScrollToContactInformation();

            // Add phone number
            IWebElement addTelephoneNumberHeader = SeleniumHelper.FindElement(By.CssSelector(SeleniumHelper.AutomationId("add_telephone_number_button")));
            SeleniumHelper.ClickByJS(addTelephoneNumberHeader);
            SeleniumHelper.Sleep(2);

            var telephoneNumberTable = agentDetailPage.TelephoneNumberTable;
            var telephoneNumberRow = telephoneNumberTable.GetLastRow();
            telephoneNumberRow.Number = phoneNumberInfor[0]; 
            telephoneNumberRow = telephoneNumberTable.GetLastInsertedRow();
            telephoneNumberRow.Location = phoneNumberInfor[1]; 
            telephoneNumberRow.MainNumber = true;

            // Add Email address
            IWebElement addEmailAddressHeader = SeleniumHelper.FindElement(By.CssSelector(SeleniumHelper.AutomationId("add_email_address_button")));
            SeleniumHelper.ClickByJS(addEmailAddressHeader);

            var emailAddressTable = agentDetailPage.EmailAddressTable;
            var emailAddressRow = emailAddressTable.GetLastRow();
            emailAddressRow.Email = emailInfor[0];
            emailAddressRow = emailAddressTable.GetLastInsertedRow();
            emailAddressRow.MainEmail = true;

            //Enter a valid address
            var addressDialog = agentDetailPage.AddNewAddress();
            //addressDialog.SearchCriteria.FullPostCode = "CF15 8AD";
            addressDialog.PostCode = "BT57 8RR";
            //click find address
            addressDialog.ClickManualAddAddress();
            //addressDialog.SetAddressDropdownValue("");

            //var addressTile = addressDialog.SearchCriteria.Search().FirstOrDefault();
            //addressTile.Click();
            addressDialog.ClickOk();

            //Scroll to Document
            agentDetailPage.ScrollToDocument();

            /* Confirmed by Adiganesh.Kannathason@capita.co.uk, Thursday, October 29, 2015 4:06 PM, 
             * Notes and Documents functionality is revamped and it has not yet implemented the same in Communication module screens.
             * The stories are identified and are raised, So I skip upload document tests
             * 
            //Update information in frame 'Document' 
            agentDetailPage.NoteAndDocumentTable[0].Summary = documentInfor[0];
            agentDetailPage.NoteAndDocumentTable[0].Note = documentInfor[1];
             
            //Add an attachment
            var viewDocument = agentDetailPage.NoteAndDocumentTable[0].AddDocument();
            AddAttachmentDialog addAttachmentDialog = viewDocument.ClickAddAttachment();
            addAttachmentDialog.BrowserToDocument(documentInfor[2]);
            viewDocument = addAttachmentDialog.UploadDocument();
            viewDocument.ClickOk();
            */

            //Click 'Save' button.
            agentDetailPage.Save();

            //Check success message appear
            //Assert.AreEqual(true, agentDetailPage.IsMessageSuccessAppear(), "Success message does not appear");

            //Search again and verify record is save successfully
            agentDetailTriplet.SearchCriteria.AgentName = updateBasicDetails[0];
            agentTile = agentDetailTriplet.SearchCriteria.Search().FirstOrDefault(x => x.Name.Contains(updateBasicDetails[0]));
            agentDetailPage = agentTile.Click<AgentDetailPage>();

            //Verify details information was updated successfully
            Assert.AreEqual(updateBasicDetails[0], agentDetailPage.ForeName, "ForeName was not updated");
            Assert.AreEqual(updateBasicDetails[1], agentDetailPage.SurName, "SurName was not updated");
            Assert.AreEqual(true, agentDetailPage.ServiceProvide.Contains(updateBasicDetails[2]), "ServiceProvide was not updated");

            //Verify 'Contact Information'
            Assert.AreEqual(phoneNumberInfor[0], agentDetailPage.TelephoneNumberTable[0].Number, "Number was not updated");
            Assert.AreEqual(phoneNumberInfor[1], agentDetailPage.TelephoneNumberTable[0].Location, "Location was not updated");
            Assert.AreEqual(true, agentDetailPage.TelephoneNumberTable[0].MainNumber, "MainNumber was not updated");        

            //Verify Address updated
            Assert.AreNotEqual("Address Not Defined", agentDetailPage.Address);

            /* Confirmed by Adiganesh.Kannathason@capita.co.uk, Thursday, October 29, 2015 4:06 PM, 
             * Notes and Documents functionality is revamped and it has not yet implemented the same in Communication module screens.
             * The stories are identified and are raised, So I skip upload document tests
             * 
            //Verify Document updated
            Assert.AreEqual(documentInfor[0], agentDetailPage.NoteAndDocumentTable[0].Summary, "Summary in document grid was not updated");
            Assert.AreEqual(documentInfor[1], agentDetailPage.NoteAndDocumentTable[0].Note, "Note in document grid was not updated");
            
            //Verify attachment file
            viewDocument = agentDetailPage.NoteAndDocumentTable[0].AddDocument();
            Assert.AreEqual(true, viewDocument.Documents.RowCount > 0, "Document attachment is not attached");
            Assert.AreEqual(documentInfor[2], viewDocument.Documents[0][1].Text, "Document attachment'information is not correct");
            viewDocument.ClickCancel();
            */

            //Confirm the changes are pulled through automatically to pupil record associated with the Agent
            //AutomationSugar.NavigateMenu("Tasks", "Pupils", "SEN Records");
            AutomationSugar.NavigateMenu("Tasks", "Pupils", "SEN Records");
            senRecordTriplet = new SenRecordTriplet();
            senRecordTriplet.SearchCriteria.Name = pupilName;
            recordTile = senRecordTriplet.SearchCriteria.Search().FirstOrDefault(x => x.Name.Equals(pupilName));
            senRecordDetailPage = recordTile.Click<SenRecordDetailPage>();
            //linkedPeoplePage = SeleniumHelper.NavigateViaAction<LinkedPeopleAndOrganisationPage>("Linked People and Organisations");
            //            Assert.AreEqual(true, linkedPeoplePage.LinkedOrganisationGrid.Rows.Any(x => x.Name.Equals(updateBasicDetails[0])),
            //                            @"Because issue at pre-condition, I can not create linked people and organisation for agent, so this confirm will be failed. 
            //                              Another reason is information was not updated to pupil record associated with the Agent");

            #endregion

            #region POS-CONDITION

            AutomationSugar.NavigateMenu("Tasks", "Communications", "Agents");
            agentDetailTriplet = new AgentDetailTriplet();
            SeleniumHelper.FindElement(By.Name("NameSearchText")).ClearText();
            SeleniumHelper.FindElement(By.Name("NameSearchText")).SendKeys(updateBasicDetails[0]);
            //agentDetailTriplet.SearchCriteria.AgentName = updateBasicDetails[0];

            //agentTile = agentDetailTriplet.SearchCriteria.Search().FirstOrDefault(x => x.Name.Contains(updateBasicDetails[0]));
            //agentDetailPage = agentTile.Click<AgentDetailPage>();
            //agentDetailPage.Delete();

            #endregion
        }

        /// <summary>
        /// Author: BaTruong
        /// Description: Record details of a new Agency with SchoolAdministrator
        [WebDriverTest(TimeoutSeconds = 2000, Enabled = true, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { "g1", "All", "P2" }, DataProvider = "TC_COM021_Data")]
        public void TC_COM021_Record_Details_Of_New_Agency(string[] basicDetails, string[] phoneDetails, string[] emailDetails, string[] documentDetails, string websiteAddress)
        {
            #region POS-CONDITION

            //Login As a School Administrator and navigate to Agencies page
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);
            AutomationSugar.NavigateMenu("Tasks", "Communications", "Agencies");

            //Clean environment before creating new agency
            var agencyTriplet = new AgencyTriplet();
            agencyTriplet.SearchCriteria.AgencyName = basicDetails[0];
            var agencyTile = agencyTriplet.SearchCriteria.Search().FirstOrDefault(x => x.AgencyName.Equals(basicDetails[0]));
            var agencyDetailPage = agencyTile != null ? agencyTile.Click<AgencyDetailPage>() : new AgencyDetailPage();
            agencyDetailPage.Delete();

            #endregion

            #region TEST STEPS

            //Click button Add new agency
            agencyDetailPage = agencyTriplet.CreateAgency();

            //Input information into frame 'Basic Details' 
            agencyDetailPage.AgencyName = basicDetails[0];
            agencyDetailPage.ServiceProvide = basicDetails[1];

            //Add linked agents
            var selectAgent = agencyDetailPage.AddLinkedAgent();
            selectAgent.SearchCriteria.AgentName = "Ellis, Paul";
            var agentTile = selectAgent.SearchCriteria.Search().FirstOrDefault(x => x.Name.Equals("Ellis, Paul"));
            agentTile.Click();
            selectAgent.ClickOk();

            //Scroll to 'Contact information'
            agencyDetailPage.ScrollToContact();

            IWebElement addTelephoneNumberHeader = SeleniumHelper.FindElement(By.CssSelector(SeleniumHelper.AutomationId("add_telephone_number_button")));
            SeleniumHelper.ClickByJS(addTelephoneNumberHeader);
            SeleniumHelper.Sleep(2);

            var telephoneNumberTable = agencyDetailPage.TelephoneNumberTable;
            var telephoneNumberRow = telephoneNumberTable.GetLastRow();
            telephoneNumberRow.Number = phoneDetails[0];
            telephoneNumberRow = telephoneNumberTable.GetLastInsertedRow();
            telephoneNumberRow.Location = phoneDetails[1];
            telephoneNumberRow.MainNumber = true;

            //Add Email information
            IWebElement addEmailAddressHeader = SeleniumHelper.FindElement(By.CssSelector(SeleniumHelper.AutomationId("add_email_address_button")));
            SeleniumHelper.ClickByJS(addEmailAddressHeader);

            var emailAddressTable = agencyDetailPage.EmailAddressTable;
            SeleniumHelper.Sleep(2);
            var emailAddressRow = emailAddressTable.GetLastRow();
            emailAddressRow.Email = emailDetails[0];
            emailAddressRow = emailAddressTable.GetLastInsertedRow();
            emailAddressRow.MainEmail = true;

            //Enter a valid Website address
            agencyDetailPage.WebsiteAddress = websiteAddress;

            //Enter a valid address
            var addressDialog = agencyDetailPage.AddNewAddress();
            addressDialog.PostCode = "BT57 8RR";
            //var addressTile = addressDialog.SearchCriteria.Search().FirstOrDefault();
            addressDialog.ClickManualAddAddress();

            //addressTile.Click();
            addressDialog.ClickOk();

            /* Confirmed by Adiganesh.Kannathason@capita.co.uk, Thursday, October 29, 2015 4:06 PM, 
             * Notes and Documents functionality is revamped and it has not yet implemented the same in Communication module screens.
             * The stories are identified and are raised, So I skip upload document tests
             *
            //Add a document
            var viewDocument = agencyDetailPage.Documents[0].AddDocument();
            AddAttachmentDialog addAttachmentDialog = viewDocument.ClickAddAttachment();
            addAttachmentDialog.BrowserToDocument(documentDetails[2]);
            viewDocument = addAttachmentDialog.UploadDocument();
            viewDocument.ClickOk();
            */

            //Click 'Save' button.
            agencyDetailPage.Save();

            //Check success message appear
            //Assert.AreEqual(true, agencyDetailPage.IsMessageSuccessAppear(), "Success message does not appear");

            //Verify Agency added successfully
            agencyTriplet.SearchCriteria.AgencyName = basicDetails[0];
            agencyTile = agencyTriplet.SearchCriteria.Search().FirstOrDefault();
            agencyDetailPage = agencyTile.Click<AgencyDetailPage>();

            //Confirm basic details
            Assert.AreEqual(basicDetails[0], agencyDetailPage.AgencyName, "Agency name is not correct");
            Assert.AreEqual(true, agencyDetailPage.ServiceProvide.Contains(basicDetails[1]), "Agency service provide is not correct");

            //Confirm linked agents
            //Assert.AreEqual("Paul", agencyDetailPage.LinkedAgents[0].ForeName, "Linked agent is not correct");

            //Confirm website address
            Assert.AreEqual(websiteAddress, agencyDetailPage.WebsiteAddress, "Website address is not correct");

            //Verify Address updated
            Assert.AreNotEqual("Address Not Defined", agencyDetailPage.Address);

            /* Confirmed by Adiganesh.Kannathason@capita.co.uk, Thursday, October 29, 2015 4:06 PM, 
             * Notes and Documents functionality is revamped and it has not yet implemented the same in Communication module screens.
             * The stories are identified and are raised, So I skip upload document tests
             *
            //Confirm document attachment
            agencyDetailPage.ScrollToDocument();
            viewDocument = agencyDetailPage.Documents[0].AddDocument();
            Assert.AreEqual(true, viewDocument.Documents.RowCount > 0, "Document attachment is not attached");
            Assert.AreEqual(documentDetails[2], viewDocument.Documents[0][1].Text, "Document attachment's information is not correct");
            */
            #endregion

            #region POS-CONDITION

            //Delete data added
            agencyDetailPage.Delete();

            #endregion
        }

        /// <summary>
        /// Author: BaTruong
        /// Description: Ensure the new Agency can be associated against a pupil's SEN record
        /// Status: Pending by 1 issue: (BugID #1) Can not add new Linked Organisations
        /// </summary>
        [WebDriverTest(TimeoutSeconds = 2000, Enabled = true, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { "g1", "All", "P2" }, DataProvider = "TC_COM022_Data")]
        public void TC_COM022_Ensure_The_New_Agency_Can_Be_Associated_Against_A_Pupil_SEN_Record(string[] basicDetails, string pupilName)
        {
            #region PRE-CONDITION

            //Login As a School Administrator and navigate to Agencies page
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);            

            #region Agency DI
            //DI- Agency here
            string AgencyName = basicDetails[0]; // Utilities.GenerateRandomString(5, "Selenium");
            Guid AgencyId;
            var Agency = this.BuildDataPackage()
                .AddData("Agency", IDCDataPackageHelper.GenerateAgency(out AgencyId, AgencyName));

            DataSetup setAgency = new DataSetup(Agency);
            #endregion
            #endregion

            #region TEST STEPS

            //Navigate to SEN Records page
            AutomationSugar.NavigateMenu("Tasks", "Pupils", "SEN Records");
            //AutomationSugar.NavigateMenu("Tasks", "Pupils", "SEN Records");

            //Search and select a pupil
            var senRecordTriplet = new SenRecordTriplet();
            senRecordTriplet.SearchCriteria.Name = pupilName;
            var recordTile = senRecordTriplet.SearchCriteria.Search().FirstOrDefault(x => x.Name.Equals(pupilName));
            var senRecordDetailPage = recordTile.Click<SenRecordDetailPage>();

            //Navigate to Linked People And Organisation page
            //var linkedPeoplePage = SeleniumHelper.NavigateViaAction<LinkedPeopleAndOrganisationPage>("Linked People and Organisations");

            //Add link agency
            //     linkedPeoplePage.AddAgent();

            /* Because of Bug #1, Unexpected Dialog appears, window 'Link an Agent or Agency' can not open
             * so I can not implement the steps on 'Link an Agent or Agency' window. The missing steps :
                1. Click Tick Box 'Agency' to set as 'Ticked'
                2. Click Button 'Search'
                3. Select Agency (use one created in an earlier Test)
                4. Click Button 'Ok'
             */

            //linkedPeoplePage.Save();

            //Confirm agency to be recorded on pupil SEN record and saved successfully
            //Assert.AreEqual(true, linkedPeoplePage.IsMessageSuccessAppear(), "Success message does not appear");
            //   Assert.AreEqual(basicDetails[0], linkedPeoplePage.LinkedOrganisationGrid[0].Name, "Agency was not recorded on SEN reocord correctly");

            #endregion
        }

        /// <summary>
        /// Author: BaTruong
        /// Description: Record details of a new Agency
        /// Status: Pending by issue: (BugID #1) Can not add new Linked Organisations
        /// </summary>
        [WebDriverTest(TimeoutSeconds = 2000, Enabled = true, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { "g1", "All", "P2" }, DataProvider = "TC_COM023_Data")]
        public void TC_COM023_Amend_Details_Of_Agency_Linked_To_Pupil_Record(string[] originalBasicDetails, string[] updateBasicDetails,
                                                                             string[] phoneNumberInfor, string[] emailInfor,
                                                                             string[] documentInfor, string pupilName)
        {
            #region PRE-CONDITION

            //Login As a School Administrator and navigate to Agencies page
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);
            AutomationSugar.NavigateMenu("Tasks", "Communications", "Agencies");

            //Clean environment before creating
            var agencyTriplet = new AgencyTriplet();
            agencyTriplet.SearchCriteria.AgencyName = originalBasicDetails[0];
            var agencyTile = agencyTriplet.SearchCriteria.Search().FirstOrDefault(x => x.AgencyName.Equals(originalBasicDetails[0]));
            var agencyDetailPage = agencyTile != null ? agencyTile.Click<AgencyDetailPage>() : new AgencyDetailPage();
            agencyDetailPage.Delete();

            //Click Add new agency button
            agencyDetailPage = agencyTriplet.CreateAgency();

            //Input information into frame 'Basic Details' 
            agencyDetailPage.AgencyName = originalBasicDetails[0];
            agencyDetailPage.ServiceProvide = originalBasicDetails[1];
            agencyDetailPage.Save();

            //Navigate to SEN Record to create Linked Agency
            AutomationSugar.NavigateMenu("Tasks", "Pupils", "SEN Records");
            var senRecordTriplet = new SenRecordTriplet();
            senRecordTriplet.SearchCriteria.Name = pupilName;
            var recordTile = senRecordTriplet.SearchCriteria.Search().FirstOrDefault(x => x.Name.Equals(pupilName));
            var senRecordDetailPage = recordTile.Click<SenRecordDetailPage>();
            SeleniumHelper.Sleep(1);
            //var linkedPeoplePage = SeleniumHelper.NavigateViaAction<LinkedPeopleAndOrganisationPage>("Linked People and Organisations");
            /* Because of Bug #1, Unexpected Dialog appears, window 'Link an Agent or Agency' can not open
             * so I can not implement the steps on 'Link an Agent or Agency' window. The missing steps :
                1. Click Button 'Search'
                2. Select Agent (use one has just been created)
                3. Click Button 'Ok'
             */
            //linkedPeoplePage.Save();

            #endregion

            #region TEST STEPS

            //Select agency that has just been created to amend
            AutomationSugar.NavigateMenu("Tasks", "Communications", "Agencies");
            
            IWebElement searchPanel = SeleniumHelper.FindElement(By.CssSelector(SeleniumHelper.AutomationId("search_criteria")));
            searchPanel.FindElement(By.Name("AgencyName")).ClearText();
            searchPanel.FindElement(By.Name("AgencyName")).SendKeys(originalBasicDetails[0]);

            SeleniumHelper.FindElement(By.CssSelector(SeleniumHelper.AutomationId("search_result"))).Click();
            Wait.WaitLoading();
            //Update information in frame 'Basic Details' 
            IWebElement maintenancePane = SeleniumHelper.FindElement(By.CssSelector(SeleniumHelper.AutomationId("Agencies_Datamaintenance_record_detail")));
            maintenancePane.FindElement(By.Name("AgencyName")).Clear();
            maintenancePane.FindElement(By.Name("AgencyName")).SendKeys(updateBasicDetails[0]);
            maintenancePane.FindElement(By.CssSelector(SeleniumHelper.AutomationId("Other"))).Click();

            //Scroll to frame 'Contact Information' to update information in frame 'Contact Information' 
            IWebElement contactDetails = maintenancePane.FindElement(By.CssSelector(SeleniumHelper.AutomationId("section_menu_Contact Details")));
            if ((contactDetails.GetAttribute("aria-expanded").Trim().Equals("false")))
                contactDetails.Click();
                

            //Enter another number in Number, enter another Location, modify tick, modify note
            SeleniumHelper.ClickByJS(maintenancePane.FindElement(By.CssSelector(SeleniumHelper.AutomationId("add_telephone_number_button"))));           
            agencyDetailPage.TelephoneNumberTable[0].Number = phoneNumberInfor[0];
            agencyDetailPage.TelephoneNumberTable[0].Location = phoneNumberInfor[1];
            agencyDetailPage.TelephoneNumberTable[0].MainNumber = true;

            //Enter another email in Email address, enter another Note, modify tick
            SeleniumHelper.ClickByJS(maintenancePane.FindElement(By.CssSelector(SeleniumHelper.AutomationId("add_email_address_button"))));           
            agencyDetailPage.EmailAddressTable[0].Email = emailInfor[0];
            agencyDetailPage.EmailAddressTable[0].MainEmail = true;

            var addressDialog = agencyDetailPage.AddNewAddress();
            addressDialog.PostCode = "BT57 8RR";
            addressDialog.ClickManualAddAddress();
            //var addressTile = addressDialog.SearchCriteria.Search().FirstOrDefault();
            //addressTile.Click();
            addressDialog.ClickOk();

            //Scroll to Document
            //agencyDetailPage.ScrollToDocument();

            //Update information in frame 'Document' 
            //maintenancePane.FindElement(By.CssSelector(SeleniumHelper.AutomationId("add_document_button"))).Click();
            //agencyDetailPage.Documents[0].Summary = documentInfor[0];
            //agencyDetailPage.Documents[0].Note = documentInfor[1];

            /* Confirmed by Adiganesh.Kannathason@capita.co.uk, Thursday, October 29, 2015 4:06 PM, 
             * Notes and Documents functionality is revamped and it has not yet implemented the same in Communication module screens.
             * The stories are identified and are raised, So I skip upload document tests
             *
            //Add a document
            var viewDocument = agencyDetailPage.Documents[0].AddDocument();
            AddAttachmentDialog addAttachmentDialog = viewDocument.ClickAddAttachment();
            addAttachmentDialog.BrowserToDocument(documentInfor[2]);
            viewDocument = addAttachmentDialog.UploadDocument();
            viewDocument.ClickOk();
            */

            //Click 'Save' button.
            SeleniumHelper.Sleep(2);
            maintenancePane.FindElement(By.CssSelector(SeleniumHelper.AutomationId("well_know_action_save"))).Click();
            Wait.WaitLoading();

            //Search again and verify record is save successfully
            searchPanel.FindElement(By.Name("AgencyName")).ClearText();
            searchPanel.FindElement(By.Name("AgencyName")).SendKeys(updateBasicDetails[0]);

            SeleniumHelper.FindElement(By.CssSelector(SeleniumHelper.AutomationId("search_result"))).Click();
            Wait.WaitLoading();

            //Verify details information was updated successfully
            maintenancePane = SeleniumHelper.FindElement(By.CssSelector(SeleniumHelper.AutomationId("Agencies_Datamaintenance_record_detail")));
            
            var agencyName = maintenancePane.FindElement(By.Name("AgencyName")).GetAttribute("value");
            Assert.AreEqual(updateBasicDetails[0], agencyName, "Agency name was not updated");

            Assert.AreEqual(true, agencyDetailPage.ServiceProvide.Contains(updateBasicDetails[1]), "ServiceProvide was not updated");

            //Verify 'Contact Information'
            Assert.AreEqual(phoneNumberInfor[0], agencyDetailPage.TelephoneNumberTable[0].Number, "Number was not updated");
            Assert.AreEqual(phoneNumberInfor[1], agencyDetailPage.TelephoneNumberTable[0].Location, "Location was not updated");
            Assert.AreEqual(true, agencyDetailPage.TelephoneNumberTable[0].MainNumber, "MainNumber was not updated");
            //Assert.AreEqual(phoneNumberInfor[2], agencyDetailPage.TelephoneNumberTable[0].Note, "Note in Contact Information was not updated");

            //Verify Address updated
            var agencyAddrDetail = SeleniumHelper.FindElement(By.Name("AddressesAddress")).Text;
            Assert.AreNotEqual("Address Not Defined", agencyAddrDetail);

            /* Confirmed by Adiganesh.Kannathason@capita.co.uk, Thursday, October 29, 2015 4:06 PM, 
             * Notes and Documents functionality is revamped and it has not yet implemented the same in Communication module screens.
             * The stories are identified and are raised, So I skip upload document tests
             *
            viewDocument = agencyDetailPage.Documents[0].AddDocument();
            Assert.AreEqual(true, viewDocument.Documents.RowCount > 0, "Document attachment was not added");
            Assert.AreEqual(documentInfor[2], viewDocument.Documents[0][1].Text, "Document attachment's information is not correct");
            viewDocument.ClickCancel();
            */

            //Confirm the changes are pulled through automatically to pupil record associated with the Agent
            //            AutomationSugar.NavigateMenu("Tasks", "Pupils", "SEN Records");
            //            senRecordTriplet = new SenRecordTriplet();
            //            senRecordTriplet.SearchCriteria.Name = pupilName;
            //            recordTile = senRecordTriplet.SearchCriteria.Search().FirstOrDefault(x => x.Name.Equals(pupilName));
            //            senRecordDetailPage = recordTile.Click<SenRecordDetailPage>();
            //            linkedPeoplePage = SeleniumHelper.NavigateViaAction<LinkedPeopleAndOrganisationPage>("Linked People and Organisations");
            //            Assert.AreEqual(true, linkedPeoplePage.LinkedOrganisationGrid.Rows.Any(x => x.Name.Equals(updateBasicDetails[0])),
            //                            @"Because issue at pre-condition, I can not create linked people and organisation for agency, so this confirm will be failed. 
            //                              Another reason is information was not updated to pupil record associated with the Agency");

            #endregion

            #region POS-CONDITION

            //Delete the data added
            agencyTriplet = new AgencyTriplet();

            searchPanel = SeleniumHelper.FindElement(By.CssSelector(SeleniumHelper.AutomationId("search_criteria")));
            searchPanel.FindElement(By.Name("AgencyName")).ClearText();
            searchPanel.FindElement(By.Name("AgencyName")).SendKeys(updateBasicDetails[0]);

            agencyTile = agencyTriplet.SearchCriteria.Search().FirstOrDefault();
            agencyDetailPage = agencyTile.Click<AgencyDetailPage>();
            agencyDetailPage.Delete();

            #endregion
        }

        /// <summary>
        /// Author: BaTruong
        /// Description: Amend record details of an Agency created by a System Administror that is linked to a Pupil record to ensure Permissions allow this
        /// Status: Pending by issue: (BugID #1) Can not add new Linked Organisations
        /// </summary>
        [WebDriverTest(TimeoutSeconds = 2000, Enabled = true, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { "g1", "All", "P2" }, DataProvider = "TC_COM024_Data")]
        public void TC_COM024_Amend_Record_Details_Of_Agency_Created_By_System_Administrator_Record(string[] originalBasicDetails, string[] updateBasicDetails,
                                                                                                    string[] phoneNumberDetails, string[] emailDetails,
                                                                                                    string[] documentDetails, string pupilName)
        {
            #region PRE-CONDITION

            //Login as SchoolAdministrator and navigate to Agencies to create an Agency
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);
            AutomationSugar.NavigateMenu("Tasks", "Communications", "Agencies");

            //Clean environment before creating
            var agencyTriplet = new AgencyTriplet();
            agencyTriplet.SearchCriteria.AgencyName = originalBasicDetails[0];
            var agencyTile = agencyTriplet.SearchCriteria.Search().FirstOrDefault(x => x.AgencyName.Equals(originalBasicDetails[0]));
            var agencyDetailPage = agencyTile != null ? agencyTile.Click<AgencyDetailPage>() : new AgencyDetailPage();
            agencyDetailPage.Delete();

            //Click Add new agency button
            agencyDetailPage = agencyTriplet.CreateAgency();

            //Input information into frame 'Basic Details' 
            agencyDetailPage.AgencyName = originalBasicDetails[0];
            agencyDetailPage.ServiceProvide = originalBasicDetails[1];
            agencyDetailPage.Save();

            //Navigate to SEN Record to create Linked Agency
            //AutomationSugar.NavigateMenu("Tasks", "Pupils", "SEN Records");

            //var senRecordTriplet = new SenRecordTriplet();
            //senRecordTriplet.SearchCriteria.Name = pupilName;
            //var recordTile = senRecordTriplet.SearchCriteria.Search().FirstOrDefault(x => x.Name.Equals(pupilName));
            //var senRecordDetailPage = recordTile.Click<SenRecordDetailPage>();

            //var linkedPeoplePage = SeleniumHelper.NavigateViaAction<LinkedPeopleAndOrganisationPage>("Linked People and Organisations");
            /* Because of Bug #1, Unexpected Dialog appears, window 'Link an Agent or Agency' can not open
            * so I can not implement the steps on 'Link an Agent or Agency' window. The missing steps :
               1. Click Button 'Search'
               2. Select Agent (use one has just been created)
               3. Click Button 'Ok'
            */
            //linkedPeoplePage.Save();

            //Logout 
            SeleniumHelper.Logout();

            #endregion

            #region TEST STEPS

            //Login as SEN Co-Ordinator and navigate to Tasks->Communication->Agencies
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SENCoordinator);
            SeleniumHelper.NavigateBySearch("Agencies", true);

            //Click button 'Search'and select Agency is created by SchoolAdministrator
            agencyTriplet = new AgencyTriplet();
            agencyTriplet.SearchCriteria.AgencyName = originalBasicDetails[0];
            agencyTile = agencyTriplet.SearchCriteria.Search().FirstOrDefault();
            //agencyTile.Click();
            //agencyTile = agencyTriplet.SearchCriteria.Search().FirstOrDefault(x => x.AgencyName.Equals(originalBasicDetails[0]));
            agencyDetailPage = agencyTile.Click<AgencyDetailPage>();

            //Update information for Agency In frame 'Basic Details' 
            IWebElement agencyMaintenance = WebContext.WebDriver.FindElement(By.CssSelector(SeleniumHelper.AutomationId("Agencies_Datamaintenance_record_detail")));
            IWebElement agencyName = agencyMaintenance.FindElement(By.Name("AgencyName"));
            agencyName.ClearText();
            agencyName.SendKeys(updateBasicDetails[0]);

            agencyDetailPage.ServiceProvide = updateBasicDetails[1];

            //Add linked agents
            var selectAgency = agencyDetailPage.AddLinkedAgent();
            selectAgency.SearchCriteria.AgentName = "Ellis, Paul";
            var pupilTile = selectAgency.SearchCriteria.Search().FirstOrDefault(x => x.Name.Equals("Ellis, Paul"));
            pupilTile.Click();
            selectAgency.ClickOk();

            //Scroll to 'Contact information'
            agencyDetailPage.ScrollToContact();

            //Add phone number information
            SeleniumHelper.FindAndClick(By.CssSelector(SeleniumHelper.AutomationId("add_telephone_number_button")));
            agencyDetailPage.TelephoneNumberTable[0].Number = phoneNumberDetails[0];
            agencyDetailPage.TelephoneNumberTable[0].Location = phoneNumberDetails[1];
            agencyDetailPage.TelephoneNumberTable[0].MainNumber = true;
            //agencyDetailPage.TelephoneNumberTable[0].Note = phoneNumberDetails[2];

            //Add Email information
            SeleniumHelper.FindAndClick(By.CssSelector(SeleniumHelper.AutomationId("add_email_address_button")));
            agencyDetailPage.EmailAddressTable[0].Email = emailDetails[0];
            agencyDetailPage.EmailAddressTable[0].MainEmail = true;
            //agencyDetailPage.EmailAddressTable[0].Note = emailDetails[1];

            //Enter a valid address
            var addressDialog = agencyDetailPage.AddNewAddress();
            addressDialog.PostCode = "BT57 8RR";
            //var addressTile = addressDialog.SearchCriteria.Search().FirstOrDefault();
            //addressTile.Click();
            addressDialog.ClickManualAddAddress();
            addressDialog.ClickOk();

            //Scroll to Document
            //agencyDetailPage.ScrollToDocument();

            //Update information in frame 'Document' 
            //agencyDetailPage.Documents[0].Summary = documentDetails[0];
            //agencyDetailPage.Documents[0].Note = documentDetails[1];

            /* Confirmed by Adiganesh.Kannathason@capita.co.uk, Thursday, October 29, 2015 4:06 PM, 
             * Notes and Documents functionality is revamped and it has not yet implemented the same in Communication module screens.
             * The stories are identified and are raised, So I skip upload document tests
             *
            //Add a document
            var viewDocument = agencyDetailPage.Documents[0].AddDocument();
            AddAttachmentDialog addAttachmentDialog = viewDocument.ClickAddAttachment();
            addAttachmentDialog.BrowserToDocument(documentDetails[2]);
            viewDocument = addAttachmentDialog.UploadDocument();
            viewDocument.ClickOk();
            */

            //Click 'Save' button.
            agencyDetailPage.Save();

            //Check success message appear
            //Assert.AreEqual(true, agencyDetailPage.IsMessageSuccessAppear(), "Success message does not appear");

            //Search again and verify record is save successfully
            IWebElement agencySearch = WebContext.WebDriver.FindElement(By.CssSelector(SeleniumHelper.AutomationId("search_criteria")));
            agencyName = agencySearch.FindElement(By.Name("AgencyName"));
            agencyName.ClearText();
            agencyName.SendKeys(updateBasicDetails[0]);
            //agencyTriplet.SearchCriteria.AgencyName = updateBasicDetails[0];
            agencyTile = agencyTriplet.SearchCriteria.Search().FirstOrDefault();
            agencyDetailPage = agencyTile.Click<AgencyDetailPage>();

            //Verify details information was updated successfully
            Assert.AreEqual(updateBasicDetails[0], agencyDetailPage.AgencyName, "AgencyName was not updated");
            Assert.AreEqual(true, agencyDetailPage.ServiceProvide.Contains(updateBasicDetails[1]), "ServiceProvide was not updated");

            //Verify 'Contact Information'
            Assert.AreEqual(phoneNumberDetails[0], agencyDetailPage.TelephoneNumberTable[0].Number, "Number was not updated");
            Assert.AreEqual(phoneNumberDetails[1], agencyDetailPage.TelephoneNumberTable[0].Location, "Location was not updated");
            Assert.AreEqual(true, agencyDetailPage.TelephoneNumberTable[0].MainNumber, "MainNumber was not updated");
            //Assert.AreEqual(phoneNumberDetails[2], agencyDetailPage.TelephoneNumberTable[0].Note, "Note in Contact Information was not updated");

            //Verify Address updated
            Assert.AreNotEqual("Address Not Defined", agencyDetailPage.Address);

            //Verify Document updated
            //Assert.AreEqual(documentDetails[0], agencyDetailPage.Documents[0].Summary, "Summary in document grid was not updated");
            //Assert.AreEqual(documentDetails[1], agencyDetailPage.Documents[0].Note, "Summary in document grid was not updated");

            /* Confirmed by Adiganesh.Kannathason@capita.co.uk, Thursday, October 29, 2015 4:06 PM, 
             * Notes and Documents functionality is revamped and it has not yet implemented the same in Communication module screens.
             * The stories are identified and are raised, So I skip upload document tests
             *
            viewDocument = agencyDetailPage.Documents[0].AddDocument();
            Assert.AreEqual(true, viewDocument.Documents.RowCount > 0, "Document attachment is not attached");
            Assert.AreEqual(documentDetails[2], viewDocument.Documents[0][1].Text, "Document attachment's information is not correct");
            viewDocument.ClickCancel();
            */

            //Confirm the changes are pulled through automatically to pupil record associated with the Agency
            //            SeleniumHelper.NavigateBySearch("SEN Records", true);

            //            senRecordTriplet = new SenRecordTriplet();
            //            senRecordTriplet.SearchCriteria.Name = pupilName;
            //            recordTile = senRecordTriplet.SearchCriteria.Search().FirstOrDefault(x => x.Name.Equals(pupilName));
            //            senRecordDetailPage = recordTile.Click<SenRecordDetailPage>();
            //            linkedPeoplePage = SeleniumHelper.NavigateViaAction<LinkedPeopleAndOrganisationPage>("Linked People and Organisations");
            //            Assert.AreEqual(true, linkedPeoplePage.LinkedOrganisationGrid.Rows.Any(x => x.Name.Equals(updateBasicDetails[0])),
            //                            @"Because issue at pre-condition, I can not create linked people and organisation for agency, so this confirm will be failed. 
            //                              Another reason is information was not updated to pupil record associated with the Agency");

            #endregion

            #region POS-CONDITION

            //Delete the data added
            SeleniumHelper.NavigateBySearch("Agencies", false);
            agencyTriplet = new AgencyTriplet();
            agencySearch = WebContext.WebDriver.FindElement(By.CssSelector(SeleniumHelper.AutomationId("search_criteria")));
            agencyName = agencySearch.FindElement(By.Name("AgencyName"));
            agencyName.ClearText();
            agencyName.SendKeys(updateBasicDetails[0]);

            //agencyTriplet.SearchCriteria.AgencyName = updateBasicDetails[0];
            agencyTile = agencyTriplet.SearchCriteria.Search().FirstOrDefault();
            agencyDetailPage = agencyTile.Click<AgencyDetailPage>();
            agencyDetailPage.Delete();

            #endregion
        }

        /// <summary>
        /// Author: BaTruong
        /// Description: Record details of a new Agency with SEN Co-Ordinator
        /// </summary>
        [WebDriverTest(TimeoutSeconds = 2000, Enabled = true, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { "g1","All", "P2" }, DataProvider = "TC_COM025_Data")]
        public void TC_COM025_Record_Details_Of_New_Agency(string[] basicDetails, string[] phoneDetails, string[] emailDetails, string[] documentDetails, string websiteAddress)
        {
            #region PRE-CONDITION

            //Login As a School Administrator and navigate to Agencies page
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);
            SeleniumHelper.NavigateBySearch("Agencies", true);

            //Clean environment before creating new agency
            var agencyTriplet = new AgencyTriplet();
            agencyTriplet.SearchCriteria.AgencyName = basicDetails[0];
            var agencyTile = agencyTriplet.SearchCriteria.Search().FirstOrDefault(x => x.AgencyName.Equals(basicDetails[0]));
            var agencyDetailPage = agencyTile != null ? agencyTile.Click<AgencyDetailPage>() : new AgencyDetailPage();
            agencyDetailPage.Delete();

            #endregion

            #region TEST STEPS

            //Click button Add new agency
            agencyDetailPage = agencyTriplet.CreateAgency();

            //Input information into frame 'Basic Details' 
            agencyDetailPage.AgencyName = basicDetails[0];
            agencyDetailPage.ServiceProvide = basicDetails[1];

            //Add linked agents
            var selectAgent = agencyDetailPage.AddLinkedAgent();
            selectAgent.SearchCriteria.AgentName = "Ellis, Paul";
            var agentTile = selectAgent.SearchCriteria.Search().FirstOrDefault(x => x.Name.Equals("Ellis, Paul"));
            agentTile.Click();
            selectAgent.ClickOk();

            //Scroll to 'Contact information'
            agencyDetailPage.ScrollToContact();
            Wait.WaitLoading();
            //Add phone number information
            SeleniumHelper.FindElement(By.CssSelector(SeleniumHelper.AutomationId("add_telephone_number_button"))).Click();
            Wait.WaitLoading();
            agencyDetailPage.TelephoneNumberTable[0].Number = phoneDetails[0];
            agencyDetailPage.TelephoneNumberTable[0].Location = phoneDetails[1];
            agencyDetailPage.TelephoneNumberTable[0].MainNumber = true;
            //agencyDetailPage.TelephoneNumberTable[0].Note = phoneDetails[2];

            //Add Email information
            SeleniumHelper.FindElement(By.CssSelector(SeleniumHelper.AutomationId("add_email_address_button"))).Click();
            Wait.WaitLoading();
            agencyDetailPage.EmailAddressTable[0].Email = emailDetails[0];
            agencyDetailPage.EmailAddressTable[0].MainEmail = true;
            //agencyDetailPage.EmailAddressTable[0].Note = emailDetails[1];

            //Enter a valid Website address
            agencyDetailPage.WebsiteAddress = websiteAddress;

            //Enter a valid address
            var addressDialog = agencyDetailPage.AddNewAddress();
            addressDialog.PostCode = "BT57 8RR";
            //var addressTile = addressDialog.SearchCriteria.Search().FirstOrDefault();
            //addressTile.Click();
            addressDialog.ClickManualAddAddress();

            addressDialog.ClickOk();

            //Scroll to Document
            //agencyDetailPage.ScrollToDocument();

            //Update information in frame 'Document' 
            //agencyDetailPage.Documents[0].Summary = documentDetails[0];
            //agencyDetailPage.Documents[0].Note = documentDetails[1];

            /* Confirmed by Adiganesh.Kannathason@capita.co.uk, Thursday, October 29, 2015 4:06 PM, 
             * Notes and Documents functionality is revamped and it has not yet implemented the same in Communication module screens.
             * The stories are identified and are raised, So I skip upload document tests
             *
            //Add a document
            var viewDocument = agencyDetailPage.Documents[0].AddDocument();
            AddAttachmentDialog addAttachmentDialog = viewDocument.ClickAddAttachment();
            addAttachmentDialog.BrowserToDocument(documentDetails[2]);
            viewDocument = addAttachmentDialog.UploadDocument();
            viewDocument.ClickOk();
            */

            //Click 'Save' button.
            agencyDetailPage.Save();

            //Check success message appear
            //Assert.AreEqual(true, agencyDetailPage.IsMessageSuccessAppear(), "Success message does not appear");

            //Verify Agency added successfully
            agencyTriplet.SearchCriteria.AgencyName = basicDetails[0];
            agencyTile = agencyTriplet.SearchCriteria.Search().FirstOrDefault();
            agencyDetailPage = agencyTile.Click<AgencyDetailPage>();

            //Confirm basic details
            Assert.AreEqual(basicDetails[0], agencyDetailPage.AgencyName, "Agency name is not correct");
            Assert.AreEqual(true, agencyDetailPage.ServiceProvide.Contains(basicDetails[1]), "Agency service provide is not correct");

            //Confirm linked agents
            var agentForeName = SeleniumHelper.FindElement(By.CssSelector("[name$='AgentLegalForename']")).GetValue();
            Assert.AreEqual("Paul", agentForeName, "Linked agent is not correct");

            //Confirm website address
            Assert.AreEqual(websiteAddress, agencyDetailPage.WebsiteAddress, "Website address is not correct");

            //Verify Address updated
            Assert.AreNotEqual("Address Not Defined", agencyDetailPage.Address);

            //Verify Document updated
            //Assert.AreEqual(documentDetails[0], agencyDetailPage.Documents[0].Summary, "Summary in document grid was not updated");
            //Assert.AreEqual(documentDetails[1], agencyDetailPage.Documents[0].Note, "Summary in document grid was not updated");

            /* Confirmed by Adiganesh.Kannathason@capita.co.uk, Thursday, October 29, 2015 4:06 PM, 
             * Notes and Documents functionality is revamped and it has not yet implemented the same in Communication module screens.
             * The stories are identified and are raised, So I skip upload document tests
             *
            //Confirm document attachment
            agencyDetailPage.ScrollToDocument();
            viewDocument = agencyDetailPage.Documents[0].AddDocument();
            Assert.AreEqual(true, viewDocument.Documents.RowCount > 0, "Document attachment is not attached");
            Assert.AreEqual(documentDetails[2], viewDocument.Documents[0][1].Text, "Document attachment's information is not correct");
            */
            #endregion

            #region POS-CONDITION

            //Delete data added
            agencyDetailPage.Delete();

            #endregion
        }

        /// <summary>
        /// Author: BaTruong
        /// Description: Ensure the new Agency can be associated against a pupil's SEN record with SEN Co-Ordinator
        /// Status: Pending by 1 issue: (BugID #1) Can not add new Linked Organisations
        /// </summary>
        [WebDriverTest(TimeoutSeconds = 2000, Enabled = true, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { "g1", "All", "P2" }, DataProvider = "TC_COM026_Data")]
        public void TC_COM026_Ensure_The_New_Agency_Can_Be_Associated_Against_A_Pupil_SEN_Record(string[] basicDetails, string pupilName)
        {
            #region PRE-CONDITION

            //Login As a SEN Co-Ordinator and navigate to Agencies page
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SENCoordinator);
            SeleniumHelper.NavigateBySearch("Agencies", true);

            //Clean environment before creating
            var agencyTriplet = new AgencyTriplet();
            agencyTriplet.SearchCriteria.AgencyName = basicDetails[0];
            var agencyTile = agencyTriplet.SearchCriteria.Search().FirstOrDefault(x => x.AgencyName.Equals(basicDetails[0]));
            var agencyDetailPage = agencyTile != null ? agencyTile.Click<AgencyDetailPage>() : new AgencyDetailPage();
            agencyDetailPage.Delete();

            //Click Add new agency button
            agencyDetailPage = agencyTriplet.CreateAgency();

            //Input information into frame 'Basic Details' 
            agencyDetailPage.AgencyName = basicDetails[0];
            agencyDetailPage.ServiceProvide = basicDetails[1];
            agencyDetailPage.Save();

            #endregion

            #region TEST STEPS

            //Navigate to SEN Records page
            SeleniumHelper.NavigateBySearch("SEN Records", false);

            //Search and select a pupil
            var senRecordTriplet = new SenRecordTriplet();
            senRecordTriplet.SearchCriteria.Name = pupilName;
            var recordTile = senRecordTriplet.SearchCriteria.Search().FirstOrDefault(x => x.Name.Equals(pupilName));
            var senRecordDetailPage = recordTile.Click<SenRecordDetailPage>();

            //Navigate to Linked People And Organisation page
            //var linkedPeoplePage = SeleniumHelper.NavigateViaAction<LinkedPeopleAndOrganisationPage>("Linked People and Organisations");

            //Add link agency
            //linkedPeoplePage.AddAgent();

            /* Because of Bug #1, Unexpected Dialog appears, window 'Link an Agent or Agency' can not open
             * so I can not implement the steps on 'Link an Agent or Agency' window. The missing steps :
                1. Click Tick Box 'Agency' to set as 'Ticked'
                2. Click Button 'Search'
                3. Select Agency (use one created in an earlier Test)
                4. Click Button 'Ok'
             */

            //linkedPeoplePage.Save();

            //Confirm agency to be recorded on pupil SEN record and saved successfully
            //Assert.AreEqual(true, linkedPeoplePage.IsMessageSuccessAppear(), "Success message does not appear");
            //Assert.AreEqual(basicDetails[0], linkedPeoplePage.LinkedOrganisationGrid[0].Name, "Agency was not recorded on SEN reocord correctly");

            #endregion

            #region POS-CONDITION

            //Delete the data added
            SeleniumHelper.NavigateBySearch("Agencies", false);
            agencyTriplet = new AgencyTriplet();
            agencyTriplet.SearchCriteria.AgencyName = basicDetails[0];
            agencyTile = agencyTriplet.SearchCriteria.Search().FirstOrDefault();
            agencyDetailPage = agencyTile.Click<AgencyDetailPage>();
            agencyDetailPage.Delete();

            #endregion
        }

        /// <summary>
        /// TC COM-27
        /// Au : An Nguyen
        /// Description: Amend details of an Agency linked to a Pupil Record
        /// Role: SEN Co-Ordinator
        /// </summary>
        [WebDriverTest(TimeoutSeconds = 1200, Enabled = true, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { "g1", "All", "P2" }, DataProvider = "TC_COM027_Data")]
        public void TC_COM027_Amend_details_of_an_Agency_linked_to_Pupil_Record(string agencyName, string serviceProvide1, string serviceProvide2, string serviceProvide3,
            string phoneNumber, string phoneLocation, string phoneNote, string emailAddress, string emailNote, string documentSumary, string documentNote,
            string editAgencyName, string editPhoneNumber, string editPhoneLocation, string editPhoneNote, string editEmailAddress, string editEmailNote,
            string editDocumentSumary, string editDocumentNote)
        {
            //Login as SENCoordinator
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SENCoordinator);

            #region Pre-Condition : Add an agency

            //Navigate to Agencies Page
            SeleniumHelper.NavigateBySearch("Agencies", true);

            //Add an new agency
            var agencies = new AgencyTriplet();
            var agency = agencies.CreateAgency();
            agency.AgencyName = agencyName;
            agency.ServiceProvide = String.Format("{0}", serviceProvide1);

            //Add first linked agent
            var selectAgents = agency.AddLinkedAgent();
            var agentSearchResult = selectAgents.SearchCriteria.Search();
            var agentTile = agentSearchResult[0];
            agentTile.Click();
            selectAgents.ClickOk(5);

            //Add second linked agent
            selectAgents = agency.AddLinkedAgent();
            agentSearchResult = selectAgents.SearchCriteria.Search();
            agentTile = agentSearchResult[3];
            agentTile.Click();
            selectAgents.ClickOk(5);

            //Add Telephone number
            agency.ScrollToContact();
            Wait.WaitLoading();
            SeleniumHelper.FindElement(By.CssSelector(SeleniumHelper.AutomationId("add_telephone_number_button"))).Click();
            Wait.WaitLoading();
            var phoneNumberRow = agency.TelephoneNumberTable.Rows.Last();
            phoneNumberRow.Number = phoneNumber;
            phoneNumberRow = agency.TelephoneNumberTable.LastInsertRow;
            phoneNumberRow.Location = phoneLocation;
            phoneNumberRow.MainNumber = true;
            //phoneNumberRow.Note = phoneNote;

            //Add Email
            SeleniumHelper.FindElement(By.CssSelector(SeleniumHelper.AutomationId("add_email_address_button"))).Click();
            Wait.WaitLoading();
            var emailRow = agency.EmailAddressTable.Rows.Last();
            emailRow.Email = emailAddress;
            emailRow = agency.EmailAddressTable.LastInsertRow;
            emailRow.MainEmail = true;
            //  emailRow.Note = emailNote;

            //Add Document
            //   agency.ScrollToDocument();
            //   var documentRow = agency.Documents.Rows.Last();
            //   documentRow.Summary = documentSumary;
            //documentRow.Note = documentNote;

            //Save Agency
            agency.Save();
            Wait.WaitForAjaxReady(By.CssSelector(".locking-mask-loading"));
            #endregion

            #region Test steps

            //Search new agency
            agencies = new AgencyTriplet();
            agencies.SearchCriteria.AgencyName = agencyName;
            var agencySearchResult = agencies.SearchCriteria.Search();
            var agencyTile = agencySearchResult.FirstOrDefault();
            agency = agencyTile.Click<AgencyDetailPage>();

            //Edit an agency name
            agency.ScrollToBasicDetail();
            Wait.WaitLoading();
            var maintenanceScreen = SeleniumHelper.FindElement(By.CssSelector(SeleniumHelper.AutomationId("Agencies_Datamaintenance_record_detail")));
            var aName = maintenanceScreen.FindElement(By.Name("AgencyName"));
            aName.ClearText();
            aName.SendKeys(editAgencyName);

            //agency.AgencyName = editAgencyName;

            //Add more service provide
            agency.ServiceProvide = String.Format("{0},{1}", serviceProvide2, serviceProvide3);

            //Delete first linked agent
            var agentRow = agency.LinkedAgents.Rows[0];
            agentRow.DeleteRow();

            //Edit Telephone number
            //agency.ScrollToContact();
            agency.TelephoneNumberTable[0].Number = editPhoneNumber;
            agency.TelephoneNumberTable[0].Location = editPhoneLocation;
            SeleniumHelper.Sleep(1);
            agency.TelephoneNumberTable[0].MainNumber = false;
            //agency.TelephoneNumberTable[0].Note = editPhoneNote;

            //Edit Email
            agency.EmailAddressTable[0].Email = editEmailAddress;
            agency.EmailAddressTable[0].MainEmail = false;
            //agency.EmailAddressTable[0].Note = editEmailNote;

            //Edit Document
            //   agency.ScrollToDocument();
            //  documentRow = agency.Documents.LastInsertRow;
            //documentRow.Summary = editDocumentSumary;
            //documentRow.Note = editDocumentNote;

            //Save Agency
            agency.Save();
            Wait.WaitForAjaxReady(By.CssSelector(".locking-mask-loading"));
            //Search agency 
            agencies = new AgencyTriplet();
            agencies.SearchCriteria.AgencyName = editAgencyName;
            agencySearchResult = agencies.SearchCriteria.Search();
            agencyTile = agencySearchResult.FirstOrDefault();

            //Verify edit name
            Assert.AreNotEqual(null, agencyTile, "Name of agency cannot edit");
            agency = agencyTile.Click<AgencyDetailPage>();

            //Verify service provide
            agency.ScrollToBasicDetail();
            var checkedServiceProvide = agency.GetCheckedServiceProvide();
            Assert.AreEqual(true, checkedServiceProvide.Contains(serviceProvide1), "Services provide is not saved");
            Assert.AreEqual(true, checkedServiceProvide.Contains(serviceProvide2), "Services provide is not saved");
            Assert.AreEqual(true, checkedServiceProvide.Contains(serviceProvide3), "Services provide is not saved");

            //Verify linked agent
            Assert.AreEqual(1, agency.LinkedAgents.Rows.Count, "Cannot delete linked agent");

            //Verify phone number
            agency.ScrollToContact();
            phoneNumberRow = agency.TelephoneNumberTable.Rows.FirstOrDefault();
            Assert.AreNotEqual(null, phoneNumberRow, "Edit phone number unsuccessfull");
            Assert.AreEqual(editPhoneLocation, phoneNumberRow.Location, "Edit phone location unsuccessfull");
            //Assert.AreEqual(editPhoneNote, phoneNumberRow.Note, "Edit phone note unsuccessfull");
            Assert.AreEqual(false, phoneNumberRow.MainNumber, "Edit phone main number unsuccessfull");

            //Verify email
            emailRow = agency.EmailAddressTable.Rows.FirstOrDefault();
            Assert.AreNotEqual(null, emailRow, "Edit email unsuccessfull");
            //Assert.AreEqual(editEmailNote, emailRow.Note, "Edit email note unsuccessfull");
            Assert.AreEqual(false, emailRow.MainEmail, "Edit main email unsuccessfull");

            ////Verify document
            //agency.ScrollToDocument();
            //documentRow = agency.Documents.Rows.FirstOrDefault(t => t.Summary.Equals(editDocumentSumary));
            //Assert.AreNotEqual(null, documentRow, "Edit document unsuccessfull");
            //Assert.AreEqual(editDocumentNote, documentRow.Note, "Edit email note unsuccessfull");

            #endregion

            #region Post-condition : Delete agency

            //Delete agency
            agency.ClickDeleteButton();
            var warningDialog = new WarningConfirmationDialog();
            warningDialog.ConfirmDelete();

            #endregion
        }

        /// <summary>
        /// TC COM-28
        /// Au : An Nguyen
        /// Description: Try delete the local Agency created previously, Warning before deleting record
        /// Role: School Administrator
        /// </summary>
        [WebDriverTest(TimeoutSeconds = 300, Enabled = true, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { "g1", "All", "P2" }, DataProvider = "TC_COM028_Data")]
        public void TC_COM028_Try_delete_the_local_Agency_created_previously_warning_before_deleting_record(string agencyName, string serviceProvide1, string serviceProvide2)
        {
            //Login as SchoolAdministrator
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);

            #region Pre-condition : Create agency

            //Navigate to Agencies Page
            AutomationSugar.NavigateMenu("Tasks", "Communications", "Agencies");

            //Add an new agency
            var agencies = new AgencyTriplet();
            var agency = agencies.CreateAgency();
            agency.AgencyName = agencyName;
            agency.ServiceProvide = String.Format("{0},{1}", serviceProvide1, serviceProvide2);

            //Save
            agency.Save();

            #endregion

            #region Test steps

            //Search and select
            agencies = new AgencyTriplet();
            agencies.SearchCriteria.AgencyName = agencyName;
            var agenciesResult = agencies.SearchCriteria.Search();
            agency = agenciesResult.FirstOrDefault().Click<AgencyDetailPage>();

            //Try delete agency
            agency.ClickDeleteButton();

            //Verify warning dialog is display
            Assert.AreEqual(true, agency.IsWarningDeleteDisplay(), "Warning Dialog is not display");

            //Close dialog
            var warningDialog = new WarningConfirmationDialog();
            warningDialog.ConfirmDelete();

            #endregion
        }

        /// <summary>
        /// TC COM-29
        /// Au : An Nguyen
        /// Description: Create a new local Agency so that it can be deleted
        /// Role: School Administrator
        /// </summary>
        [WebDriverTest(TimeoutSeconds = 1000, Enabled = true, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { "g1", "All", "P2" }, DataProvider = "TC_COM029_Data")]
        public void TC_COM029_Create_a_new_local_Agency_so_that_it_can_be_deleted(string agencyName, string serviceProvide1, string serviceProvide2, string serviceProvide3)
        {
            //Login as SchoolAdministrator
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);

            #region Test steps

            //Navigate to Agencies Page
            AutomationSugar.NavigateMenu("Tasks", "Communications", "Agencies");

            //Add an new agency
            var agencies = new AgencyTriplet();
            var agency = agencies.CreateAgency();
            agency.AgencyName = agencyName;
            agency.ServiceProvide = String.Format("{0},{1},{2}", serviceProvide1, serviceProvide2, serviceProvide3);

            //Add linked agent
            var selectAgents = agency.AddLinkedAgent();
            var agentSearchResult = selectAgents.SearchCriteria.Search();
            var agentTile = agentSearchResult[0];
            agentTile.Click();
            selectAgents.ClickOk(5);

            //Save
            agency.Save();

            //Search new agency
            agencies = new AgencyTriplet();
            agencies.SearchCriteria.AgencyName = agencyName;
            var agencySearchResult = agencies.SearchCriteria.Search();
            var agencyTile = agencySearchResult.FirstOrDefault();
            agency = agencyTile.Click<AgencyDetailPage>();

            //Verify service provide
            var checkedServiceProvide = agency.GetCheckedServiceProvide();
            Assert.AreEqual(true, checkedServiceProvide.Contains(serviceProvide1), "Services provide is not saved");
            Assert.AreEqual(true, checkedServiceProvide.Contains(serviceProvide2), "Services provide is not saved");
            Assert.AreEqual(true, checkedServiceProvide.Contains(serviceProvide3), "Services provide is not saved");

            //Verify linked agent
            Assert.AreEqual(1, agency.LinkedAgents.Rows.Count, "Linked agent is not saved");

            //Delete agency
            agency.ClickDeleteButton();
            var warningDialog = new WarningConfirmationDialog();
            warningDialog.ConfirmDelete();

            //Search and verify agency after delete
            agencies = new AgencyTriplet();
            agencies.SearchCriteria.AgencyName = agencyName;
            agencySearchResult = agencies.SearchCriteria.Search();
            agencyTile = agencySearchResult.FirstOrDefault();
            Assert.AreEqual(null, agencyTile, "Delete agency unsuccessfull");

            #endregion
        }

        /// <summary>
        /// TC COM-30
        /// Au : An Nguyen
        /// Description: Try delete the local Agent created previously, Warning before deleting record
        /// Role: School Administrator
        /// </summary>
        [WebDriverTest(TimeoutSeconds = 300, Enabled = true, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { "g1", "All", "P2" }, DataProvider = "TC_COM030_Data")]
        public void TC_COM030_Try_delete_the_local_Agent_created_previously_warning_before_deleting_record(string foreName, string surName, string serviceProvide)
        {
            //Login as SchoolAdministrator
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);

            #region Pre-condition : Create agent

            //Navigate to Agencies Page
            AutomationSugar.NavigateMenu("Tasks", "Communications", "Agents");

            //Add an new agent
            var agents = new AgentDetailTriplet();
            var addBasicDetails = agents.AddNewAgent();

            //Add basic detail
            addBasicDetails.ForeName = foreName;
            addBasicDetails.SurName = surName;

            //Add service provide
            addBasicDetails.ClickContinue();
            //var addService = addBasicDetails.ClickContinue();

            // Enter service detail
            var _serviceType = SeleniumHelper.Get(POM.Helper.SimsBy.AutomationId(serviceProvide));
            _serviceType.Set(true);

            //Save Agent Record.
            SeleniumHelper.FindElement(By.CssSelector(SeleniumHelper.AutomationId("well_know_action_save"))).Click();
            Wait.WaitForAjaxReady(By.CssSelector(".locking-mask-loading"));
            
            //addService.ServiceProvide = serviceProvide;

            ////Create record
            //var agentDetail = addService.CreateNewRecord();

            ////Save
            //agentDetail.Save();

            #endregion

            #region Test steps

            //Search and select
            agents = new AgentDetailTriplet();
            agents.SearchCriteria.AgentName = String.Format("{0}, {1}", surName, foreName);
            var agentSearchResult = agents.SearchCriteria.Search();
            var agentDetail = agentSearchResult.FirstOrDefault(t => t.Name.Equals(String.Format("{0}, {1}", surName, foreName))).Click<AgentDetailPage>();

            //Click delete button
            agentDetail.ClickDeleteButton();

            //Verify warning dialog is display
            Assert.AreEqual(true, agentDetail.IsWarningDeleteDisplay(), "Warning Dialog is not display");

            //Close dialog
            var warningDialog = new WarningConfirmationDialog();
            warningDialog.ConfirmDelete();

            #endregion
        }

        /// <summary>
        /// TC COM-31
        /// Au : An Nguyen
        /// Description: Create a new local Agent so that it can be deleted
        /// Role: School Administrator
        /// </summary>
        [WebDriverTest(TimeoutSeconds = 1000, Enabled = false, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { "g1", "All", "P2" }, DataProvider = "TC_COM031_Data")]
        public void TC_COM031_Create_a_new_local_Agent_so_that_it_can_be_deleted(string foreName, string surName,
                    string serviceProvide1, string serviceProvide2, string serviceProvide3, string agencyName)
        {
            //Login as SchoolAdministrator
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);

            //Navigate to Agents Page
            AutomationSugar.NavigateMenu("Tasks", "Communications", "Agents");

            //Add an new agent
            var agents = new AgentDetailTriplet();
            var addBasicDetails = agents.AddNewAgent();

            //Add basic detail
            addBasicDetails.ForeName = foreName;
            addBasicDetails.SurName = surName;

            //Add service provide
            addBasicDetails.ClickContinue();

            var _serviceType = SeleniumHelper.Get(POM.Helper.SimsBy.AutomationId(serviceProvide1));
            _serviceType.Set(true);

            _serviceType = SeleniumHelper.Get(POM.Helper.SimsBy.AutomationId(serviceProvide2));
            _serviceType.Set(true);

            _serviceType = SeleniumHelper.Get(POM.Helper.SimsBy.AutomationId(serviceProvide3));
            _serviceType.Set(true);

            //Create record
            var agentDetail = ServiceProvideDialog.CreateNewRecord();

            //Add linked agencies
            var selectAgencies = agentDetail.AddLinkAgencies();
            selectAgencies.SearchCriteria.AgencyName = agencyName;
            var agencySearchResult = selectAgencies.SearchCriteria.Search();
            var agencyTile = agencySearchResult.FirstOrDefault();
            agencyTile.Click();
            selectAgencies.ClickOk(5);
            agentDetail.WaitUnilAgencyRowDisplay(agencyName);

            //Save
            agentDetail.Save();

            //Search 
            agents = new AgentDetailTriplet();
            agents.SearchCriteria.AgentName = String.Format("{0}, {1}", surName, foreName);
            var agentSearchResult = agents.SearchCriteria.Search();
            var agentTile = agentSearchResult.FirstOrDefault(t => t.Name.Equals(String.Format("{0}, {1}", surName, foreName)));
            agentDetail = agentTile.Click<AgentDetailPage>();

            //Verify service provide
            var checkedServiceProvide = agentDetail.GetCheckedServiceProvide();
            Assert.AreEqual(true, checkedServiceProvide.Contains(serviceProvide1), "Services provide is not saved");
            Assert.AreEqual(true, checkedServiceProvide.Contains(serviceProvide2), "Services provide is not saved");
            Assert.AreEqual(true, checkedServiceProvide.Contains(serviceProvide3), "Services provide is not saved");

            //Verify linked agency
            var agencyRow = agentDetail.LinkedAgenciesTable.Rows.FirstOrDefault(t => t.Agency.Equals(agencyName));
            Assert.AreNotEqual(null, agencyRow, "Linked agencies is not saved");

            //Delete agent
            agentDetail.Delete();

            //Search and verify delete
            agents = new AgentDetailTriplet();
            agents.SearchCriteria.AgentName = String.Format("{0}, {1}", surName, foreName);
            agentSearchResult = agents.SearchCriteria.Search();
            agentTile = agentSearchResult.FirstOrDefault(t => t.Name.Equals(String.Format("{0}, {1}", surName, foreName)));
            Assert.AreEqual(null, agentTile, "Delete agent unsuccessfull");

        }

        /// <summary>
        /// TC COM-32
        /// Au : An Nguyen
        /// Description: Create a Staff Notice from the Home Page 
        /// Role: School Administrator
        /// Status : Pending by issue "Can not add attachment to an notice"
        /// </summary>
        [WebDriverTest(TimeoutSeconds = 1000, Enabled = false, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { "g1" }, DataProvider = "TC_COM032_Data")]
        public void TC_COM032_Create_a_Staff_Notice_from_the_Home_Page(string title, string notice, string dateStart, string dateExpiry)
        {
            //Login as SchoolAdministrator
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);

            //Create a notice from home page
            var leftPanel = new LeftMenu();
            var createNotice = leftPanel.CreateNotice();
            createNotice.Title = title;
            createNotice.Notice = notice;
            createNotice.StartDate = dateStart;
            createNotice.ExpiryDate = dateExpiry;
            createNotice.SaveNotice();

            //Verify notice on notice board
            leftPanel.Refresh();
            var noticeItem = leftPanel.NoticeBoard.Items.FirstOrDefault(t => t.Title.Equals(title));
            Assert.AreNotEqual(null, noticeItem, "Can not create a notice from home page");

            //Verify data of the notice
            var noticePopover = noticeItem.ViewDetail();
            Assert.AreEqual(title, noticePopover.Title, "Title of notice is incorrect");
            Assert.AreEqual(notice, noticePopover.Notice, "Details of notice is inccorrect");

            /* Confirmed by Adiganesh.Kannathason@capita.co.uk, Thursday, October 29, 2015 4:06 PM, 
             * Notes and Documents functionality is revamped and it has not yet implemented the same in Communication module screens.
             * The stories are identified and are raised, So I skip upload document tests
             */
            //noticePopover.ClosePopup();
            //noticeItem.AddAttachment();
            //Assert.AreEqual(false, leftPanel.IsAttachmentUnavailableExist(), "Cannot add attachment file");
        }

        /// <summary>
        /// Author: Y.Ta
        /// Des: Verify expried notice does not display
        /// </summary>
        /// <param name="NoticeBoard"></param>
        [WebDriverTest(TimeoutSeconds = 800, Enabled = false, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { "g1" }, DataProvider = "TC_COM033_Data")]
        public void TC_COM033_Verify_That_Staff_Expired_Notice_Not_Display(string[] NoticeBoard)
        {
            //Log in as a School Administrator
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);

            // Navigate to Staff Notice Board
            AutomationSugar.NavigateMenu("Tasks", "Communications", "Staff Notice Board");

            // Verify that the notice board is exist
            var staffNoticeBoardTriplet = new StaffNoticeBoardTriplet();
            staffNoticeBoardTriplet.SearchCriteria.Title = NoticeBoard[0];
            var listSearchResult = staffNoticeBoardTriplet.SearchCriteria.Search();
            var noticeResult = listSearchResult.FirstOrDefault(t => t.Name.Equals(NoticeBoard[0]));
            var staffNoticeBoardPage = noticeResult == null ? null : noticeResult.Click<StaffNoticeBoardPage>();
            //Assert.AreNotEqual(null, staffNoticeBoardPage, "Does not found the expired notice staff");

            // Verify that On the Home Page check that a Notice with a Date earlier that today is no longer available
            SeleniumHelper.ClickFooterTab("Home Page");
            var leftMenu = new LeftMenu();
            Assert.AreEqual(false, leftMenu.DoesNoticeBoardExist(NoticeBoard[0]), "The notice board still display");

        }

        /// <summary>
        /// Author: Y.Ta
        /// Des: Verify that notice can see only by created user
        /// </summary>
        /// <param name="NoticeBoard"></param>
        [WebDriverTest(TimeoutSeconds = 800, Enabled = false, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { "g1", "All", "P2" }, DataProvider = "TC_COM034_Data")]
        public void TC_COM034_Verify_That_Notice_Can_See_By_Only_Created_User(string[] NoticeBoard)
        {
            #region Pre-Condition Creat New Notice

            // Log in as School Admin and Navigate to Staff Notice Board
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);
            AutomationSugar.NavigateMenu("Tasks", "Communications", "Staff Notice Board");

            // Search to check if exist notice board
            var staffNoticeBoardTriplet = new StaffNoticeBoardTriplet();
            staffNoticeBoardTriplet.SearchCriteria.Title = NoticeBoard[0];
            var listSearchResult = staffNoticeBoardTriplet.SearchCriteria.Search();
            var noticeResult = listSearchResult.FirstOrDefault(t => t.Name.Equals(NoticeBoard[0]));
            var staffNoticeBoardPage = noticeResult == null ? staffNoticeBoardTriplet.ClickCreate() : noticeResult.Click<StaffNoticeBoardPage>();

            // Delete if exist
            staffNoticeBoardTriplet.Delete();
            staffNoticeBoardTriplet.ClickCreate();

            // Create new notice
            staffNoticeBoardPage.Title = NoticeBoard[0];
            staffNoticeBoardPage.StartDate = NoticeBoard[1];
            staffNoticeBoardPage.EndDate = NoticeBoard[2];

            // Add an Attachment
            // This step is skip by 'the button document does not work' issue.

            //Click save
            staffNoticeBoardTriplet.ClickSave();

            #endregion

            #region Steps

            // Search the notice which is created by this user
            staffNoticeBoardTriplet.SearchCriteria.Title = NoticeBoard[0];
            listSearchResult = staffNoticeBoardTriplet.SearchCriteria.Search();
            noticeResult = listSearchResult.FirstOrDefault(t => t.Name.Equals(NoticeBoard[0]));

            // Open this notice if it displays
            staffNoticeBoardPage = noticeResult == null ? staffNoticeBoardTriplet.ClickCreate() : noticeResult.Click<StaffNoticeBoardPage>();

            // Verify that the notice can be see by the created user
            Assert.AreNotEqual(null, staffNoticeBoardPage, "The created user can not search his notice");

            // Update the notice
            staffNoticeBoardPage.Title = NoticeBoard[0] + " Update";
            staffNoticeBoardTriplet.ClickSave();
            staffNoticeBoardPage = new StaffNoticeBoardPage();

            // Verify that the notice can be updated by created user
            Assert.AreEqual(NoticeBoard[0] + " Update", staffNoticeBoardPage.Title, "The created user can not update his notice");

            // Login with other role
            SeleniumHelper.Logout();
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.PersonnelOfficer);
            AutomationSugar.NavigateMenu("Tasks", "Communications", "Staff Notice Board");

            // Search and Open the notice which is created by other user
            var staffNoticeBoardTripletPerson = new StaffNoticeBoardTriplet();
            staffNoticeBoardTripletPerson.SearchCriteria.Title = NoticeBoard[0] + " Update";
            var listSearchResultPerson = staffNoticeBoardTripletPerson.SearchCriteria.Search();
            var noticeResultPerson = listSearchResultPerson.FirstOrDefault(t => t.Name.Equals(NoticeBoard[0] + " Update"));

            // Verify that the notice cannot be see by other user
            Assert.AreEqual(null, noticeResultPerson, "The other user can see other notice");

            #endregion

            #region Post-Condition: Delete Notice

            // Delete exist the staff notice board
            SeleniumHelper.Logout();
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);
            AutomationSugar.NavigateMenu("Tasks", "Communications", "Staff Notice Board");

            var staffNoticeBoardTripletAdmin = new StaffNoticeBoardTriplet();
            staffNoticeBoardTripletAdmin.SearchCriteria.Title = NoticeBoard[0] + " Update";
            var listSearchResultAdmin = staffNoticeBoardTripletAdmin.SearchCriteria.Search();
            var noticeResultAdmin = listSearchResultAdmin.FirstOrDefault(t => t.Name.Equals(NoticeBoard[0] + " Update"));
            var deletePage = noticeResultAdmin == null ? new StaffNoticeBoardPage() : noticeResultAdmin.Click<StaffNoticeBoardPage>();
            staffNoticeBoardTripletAdmin.Delete();
            #endregion

        }

        /// <summary>
        /// Author: Y.Ta
        /// Des: Verify that notice can be created successfully
        /// Status: Pending by issue: 'The button document does not work'
        /// </summary>
        /// <param name="NoticeBoard"></param>
        [WebDriverTest(TimeoutSeconds = 800, Enabled = false, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { "g1", "All", "P2" }, DataProvider = "TC_COM035_Data")]
        public void TC_COM035_Verify_That_Admin_Create_Notice_Successfully(string[] NoticeBoard)
        {
            #region Pre Condition: Creat New Notice

            // Log in as School Admin
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);
            AutomationSugar.NavigateMenu("Tasks", "Communications", "Staff Notice Board");

            // Search to check if exist notice board
            var staffNoticeBoardTriplet = new StaffNoticeBoardTriplet();
            staffNoticeBoardTriplet.SearchCriteria.Title = NoticeBoard[0];
            var listSearchResult = staffNoticeBoardTriplet.SearchCriteria.Search();
            var noticeResult = listSearchResult.FirstOrDefault(t => t.Name.Equals(NoticeBoard[0]));
            var staffNoticeBoardPage = noticeResult == null ? staffNoticeBoardTriplet.ClickCreate() : noticeResult.Click<StaffNoticeBoardPage>();

            // Delete if exist
            staffNoticeBoardTriplet.Delete();
            staffNoticeBoardTriplet.ClickCreate();

            //Create new notice
            staffNoticeBoardPage.Title = NoticeBoard[0];
            staffNoticeBoardPage.StartDate = NoticeBoard[1];
            staffNoticeBoardPage.EndDate = NoticeBoard[2];

            /* Confirmed by Adiganesh.Kannathason@capita.co.uk, Thursday, October 29, 2015 4:06 PM, 
           * Notes and Documents functionality is revamped and it has not yet implemented the same in Communication module screens.
           * The stories are identified and are raised, So I skip upload document tests
           */
            // Add an Attachment
            // This step is skip by 'the button document does not work' issue.

            staffNoticeBoardTriplet.ClickSave();
            Assert.AreEqual(true, staffNoticeBoardTriplet.DoesMessageSuccessDisplay(), "The success message is not display");

            #endregion

            #region Test steps

            // Search and Open specific the notice
            staffNoticeBoardTriplet.SearchCriteria.Title = NoticeBoard[0];
            listSearchResult = staffNoticeBoardTriplet.SearchCriteria.Search();
            noticeResult = listSearchResult.FirstOrDefault(t => t.Name.Equals(NoticeBoard[0]));
            staffNoticeBoardPage = noticeResult == null ? staffNoticeBoardTriplet.ClickCreate() : noticeResult.Click<StaffNoticeBoardPage>();

            // Verify that the notice can be see on Search Result
            Assert.AreNotEqual(null, staffNoticeBoardPage, "The created user can not search his notice");

            // Verify that the information of notice displays correctly
            Assert.AreEqual(NoticeBoard[0], staffNoticeBoardPage.Title, "The title field displays correctly");
            Assert.AreEqual(NoticeBoard[1], staffNoticeBoardPage.StartDate, "The start date field displays correctly");
            Assert.AreEqual(NoticeBoard[2], staffNoticeBoardPage.EndDate, "The end date field displays correctly");

            /* Confirmed by Adiganesh.Kannathason@capita.co.uk, Thursday, October 29, 2015 4:06 PM, 
           * Notes and Documents functionality is revamped and it has not yet implemented the same in Communication module screens.
           * The stories are identified and are raised, So I skip upload document tests
           */
            // Issue: The button document does not work
            //staffNoticeBoardPage = new StaffNoticeBoardPage();
            //staffNoticeBoardPage.ClickDocument();
            //Assert.AreNotEqual(true, staffNoticeBoardPage.DoesPopErrorExist(), "The popup error display when clicking document button");

            // Verify that the notice can be see on Home Page
            staffNoticeBoardPage = new StaffNoticeBoardPage();
            SeleniumHelper.ClickFooterTab("Home Page");
            var leftMenu = new LeftMenu();
            Assert.AreEqual(true, leftMenu.DoesNoticeBoardExist(NoticeBoard[0]), "The notice board does not display");

            #endregion

            #region Pos-condition : Delete Notice

            // Delete exist the staff notice board
            SeleniumHelper.Logout();
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);
            AutomationSugar.NavigateMenu("Tasks", "Communications", "Staff Notice Board");

            var staffNoticeBoardTripletAdmin = new StaffNoticeBoardTriplet();
            staffNoticeBoardTripletAdmin.SearchCriteria.Title = NoticeBoard[0];
            var listSearchResultAdmin = staffNoticeBoardTripletAdmin.SearchCriteria.Search();
            var noticeResultAdmin = listSearchResultAdmin.FirstOrDefault(t => t.Name.Equals(NoticeBoard[0]));
            var deletePage = noticeResultAdmin == null ? new StaffNoticeBoardPage() : noticeResultAdmin.Click<StaffNoticeBoardPage>();
            staffNoticeBoardTripletAdmin.Delete();

            #endregion

        }

        /// <summary>
        /// Author: Y.Ta
        /// Des: Verify that notice can not be see after update start date to future date
        /// Status: Pending by issue: 'The button document does not work'
        /// </summary>
        /// <param name="NoticeBoard"></param>
        [WebDriverTest(TimeoutSeconds = 800, Enabled = false, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { "g1", "All", "P2" }, DataProvider = "TC_COM037_Data")]
        public void TC_COM037_Verify_That_Notice_Update_Future_Date_Not_Show_On_Home_Page(string[] NoticeBoard, string futureDate)
        {
            #region Pre-Condition: Creat New Notice

            // Login as school admin and navigate to Staff Notice Board page
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);
            AutomationSugar.NavigateMenu("Tasks", "Communications", "Staff Notice Board");

            // Search to check if exist notice board
            var staffNoticeBoardTriplet = new StaffNoticeBoardTriplet();
            staffNoticeBoardTriplet.SearchCriteria.Title = NoticeBoard[0];
            var listSearchResult = staffNoticeBoardTriplet.SearchCriteria.Search();
            var noticeResult = listSearchResult.FirstOrDefault(t => t.Name.Equals(NoticeBoard[0]));
            var staffNoticeBoardPage = noticeResult == null ? staffNoticeBoardTriplet.ClickCreate() : noticeResult.Click<StaffNoticeBoardPage>();

            // Delete if exist
            staffNoticeBoardTriplet.Delete();
            staffNoticeBoardTriplet.ClickCreate();

            //Create new notice
            staffNoticeBoardPage.Title = NoticeBoard[0];
            staffNoticeBoardPage.StartDate = NoticeBoard[1];
            staffNoticeBoardPage.EndDate = NoticeBoard[2];

            /* Confirmed by Adiganesh.Kannathason@capita.co.uk, Thursday, October 29, 2015 4:06 PM, 
           * Notes and Documents functionality is revamped and it has not yet implemented the same in Communication module screens.
           * The stories are identified and are raised, So I skip upload document tests
           */
            // Add an Attachment
            // This step is skip by 'the button document does not work' issue.

            staffNoticeBoardTriplet.ClickSave();
            //Assert.AreEqual(true, staffNoticeBoardTriplet.DoesMessageSuccessDisplay(), "The success message is not display");

            #endregion

            #region Steps

            // Verify that the notice can be see on Search Result
            staffNoticeBoardTriplet.SearchCriteria.Title = NoticeBoard[0];
            listSearchResult = staffNoticeBoardTriplet.SearchCriteria.Search();
            noticeResult = listSearchResult.FirstOrDefault(t => t.Name.Equals(NoticeBoard[0]));
            staffNoticeBoardPage = noticeResult == null ? staffNoticeBoardTriplet.ClickCreate() : noticeResult.Click<StaffNoticeBoardPage>();

            // Update the start date to future date
            staffNoticeBoardPage.StartDate = futureDate;
            staffNoticeBoardTriplet.ClickSave();

            /* Confirmed by Adiganesh.Kannathason@capita.co.uk, Thursday, October 29, 2015 4:06 PM, 
           * Notes and Documents functionality is revamped and it has not yet implemented the same in Communication module screens.
           * The stories are identified and are raised, So I skip upload document tests
           */
            // Issue document button does not work
            //staffNoticeBoardPage = new StaffNoticeBoardPage();
            //staffNoticeBoardPage.ClickDocument();
            //Assert.AreNotEqual(true, staffNoticeBoardPage.DoesPopErrorExist(), "The popup error display when clicking document button");

            // Verify that the notice can not be see on Home Page
            SeleniumHelper.ClickFooterTab("Home Page");
            var leftMenu = new LeftMenu();
            Assert.AreNotEqual(true, leftMenu.DoesNoticeBoardExist(NoticeBoard[0]), "The notice board does not display");

            #endregion

            #region Delete Notice

            // Delete exist the staff notice board
            SeleniumHelper.Logout();
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);
            AutomationSugar.NavigateMenu("Tasks", "Communications", "Staff Notice Board");

            var staffNoticeBoardTripletAdmin = new StaffNoticeBoardTriplet();
            staffNoticeBoardTripletAdmin.SearchCriteria.Title = NoticeBoard[0];
            var listSearchResultAdmin = staffNoticeBoardTripletAdmin.SearchCriteria.Search();
            var noticeResultAdmin = listSearchResultAdmin.FirstOrDefault(t => t.Name.Equals(NoticeBoard[0]));
            var deletePage = noticeResultAdmin == null ? new StaffNoticeBoardPage() : noticeResultAdmin.Click<StaffNoticeBoardPage>();
            staffNoticeBoardTripletAdmin.Delete();

            #endregion

        }

        /// <summary>
        /// Verify that admin can delete notice Successfully
        /// </summary>
        /// <param name="NoticeBoard"></param>
        [WebDriverTest(TimeoutSeconds = 800, Enabled = false, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { "g1", "All", "P2" }, DataProvider = "TC_COM039_Data")]
        public void TC_COM039_Verify_That_Delete_Exist_Staff_Notice_(string[] NoticeBoard)
        {
            #region Pre-Condition: Creat New Notice

            // Login as school admin and navigate to Staff Notice Board page
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);
            AutomationSugar.NavigateMenu("Tasks", "Communications", "Staff Notice Board");

            // Search to check if exist notice board
            var staffNoticeBoardTriplet = new StaffNoticeBoardTriplet();
            staffNoticeBoardTriplet.SearchCriteria.Title = NoticeBoard[0];
            var listSearchResult = staffNoticeBoardTriplet.SearchCriteria.Search();
            var noticeResult = listSearchResult.FirstOrDefault(t => t.Name.Equals(NoticeBoard[0]));
            var staffNoticeBoardPage = noticeResult == null ? staffNoticeBoardTriplet.ClickCreate() : noticeResult.Click<StaffNoticeBoardPage>();

            // Delete if exist
            staffNoticeBoardTriplet.Delete();
            staffNoticeBoardTriplet.ClickCreate();

            //Create new notice
            staffNoticeBoardPage.Title = NoticeBoard[0];
            staffNoticeBoardPage.StartDate = NoticeBoard[1];
            staffNoticeBoardPage.EndDate = NoticeBoard[2];

            //Click Save
            staffNoticeBoardTriplet.ClickSave();

            #endregion

            #region Test steps

            // Delete exist the staff notice board
            staffNoticeBoardTriplet.SearchCriteria.Title = NoticeBoard[0];
            listSearchResult = staffNoticeBoardTriplet.SearchCriteria.Search();
            noticeResult = listSearchResult.FirstOrDefault(t => t.Name.Equals(NoticeBoard[0]));
            var deletePage = noticeResult == null ? new StaffNoticeBoardPage() : noticeResult.Click<StaffNoticeBoardPage>();
            staffNoticeBoardTriplet.Delete();

            // Verify the deleted staff notice board does not exist when searching
            staffNoticeBoardTriplet.SearchCriteria.Title = NoticeBoard[0];
            listSearchResult = staffNoticeBoardTriplet.SearchCriteria.Search();
            noticeResult = listSearchResult.FirstOrDefault(t => t.Name.Equals(NoticeBoard[0]));

            //Verify that the Notice is now no longer visible in search Results 
            Assert.AreEqual(null, noticeResult, "The deleted staff notice board still exist");

            //Verify that the Notice is now no longer visible on the Home Page
            SeleniumHelper.ClickFooterTab("Home Page");
            var leftMenu = new LeftMenu();
            Assert.AreEqual(false, leftMenu.DoesNoticeBoardExist(NoticeBoard[0]), "The notice board still display");

            #endregion
        }

     

        #region DATA

        public List<object[]> TC_COM001_Data()
        {
            string practiceName = "TC_COM001_Data " + SeleniumHelper.GenerateRandomString(30);
            string phoneNumber = "123456789";
            string doctorTitle = "Mr";
            string doctorSureName = SeleniumHelper.GenerateRandomString(10);
            string doctorMidleName = SeleniumHelper.GenerateRandomString(10);
            string doctorForeName = SeleniumHelper.GenerateRandomString(10);

            var data = new List<Object[]>
            {                
                new object[] {practiceName,phoneNumber,doctorTitle,doctorSureName,doctorMidleName,doctorForeName              }
            };
            return data;
        }

        public List<object[]> TC_COM002_Data()
        {
            string practiceName = "TC_COM002_Data " + SeleniumHelper.GenerateRandomString(20);
            string phoneNumber = "123456789";
            string doctorTitle = "Mr";
            string doctorSureName = SeleniumHelper.GenerateRandomString(10);
            string doctorMidleName = SeleniumHelper.GenerateRandomString(10);
            string doctorForeName = SeleniumHelper.GenerateRandomString(10);
       
            var data = new List<Object[]>
            {                
                new object[] {practiceName,phoneNumber,doctorTitle,doctorSureName,doctorMidleName,doctorForeName}
            };
            return data;
        }

        public List<object[]> TC_COM003_Data()
        {
            string practiceName = "TC03 " + SeleniumHelper.GenerateRandomString(30);
            string phoneNumber = "123456789";
            string doctorTitle = "Mr";
            string doctorSureName = SeleniumHelper.GenerateRandomString(10);
            string doctorMidleName = SeleniumHelper.GenerateRandomString(10);
            string doctorForeName = SeleniumHelper.GenerateRandomString(10);
            string practiceNameUpdate = practiceName; // "Update Practice Name " + SeleniumHelper.GenerateRandomString(10);
            string phoneNumberUpdate = "98765432";
            string doctorTitleAddMore = "Mr";
            string doctorSureNameAddMore = "SName " + SeleniumHelper.GenerateRandomString(10);
            string doctorMidleNameAddMore = "MName " + SeleniumHelper.GenerateRandomString(10);
            string doctorForeNameAddMore = "FName " + SeleniumHelper.GenerateRandomString(10);
            var data = new List<Object[]>
            {                
                new object[] {practiceName,phoneNumber,doctorTitle,doctorSureName,doctorMidleName,doctorForeName,
                practiceNameUpdate,phoneNumberUpdate,doctorTitleAddMore,doctorSureNameAddMore,doctorMidleNameAddMore,doctorForeNameAddMore}
            };
            return data;
        }

        public List<object[]> TC_COM004_Data()
        {
            string practiceName = "TC_COM004_Data " + SeleniumHelper.GenerateRandomString(20);
            string phoneNumber = "123456789";
            string doctorTitle = "Mr";
            string doctorSureName = SeleniumHelper.GenerateRandomString(10);
            string doctorMidleName = SeleniumHelper.GenerateRandomString(10);
            string doctorForeName = SeleniumHelper.GenerateRandomString(10);
        
            string practiceNameUpdate = practiceName; //"Update Medical Practice " + SeleniumHelper.GenerateRandomString(5);

            var data = new List<Object[]>
            {                
                new object[] {practiceName,phoneNumber,doctorTitle,doctorSureName,doctorMidleName,doctorForeName,practiceNameUpdate}
            };
            return data;
        }

        public List<object[]> TC_COM005_Data()
        {
            string practiceName = "TC_COM005_Data " + SeleniumHelper.GenerateRandomString(30);
            string phoneNumber = "123456789";
            string doctorTitle = "Mr";
            string doctorSureName = SeleniumHelper.GenerateRandomString(10);
            string doctorMidleName = SeleniumHelper.GenerateRandomString(10);
            string doctorForeName = SeleniumHelper.GenerateRandomString(10);
         
            var data = new List<Object[]>
            {                
                new object[] {practiceName,phoneNumber,doctorTitle,doctorSureName,doctorMidleName,doctorForeName              }
            };
            return data;
        }

        public List<object[]> TC_COM006_Data()
        {
            string practiceName = "TC06 " + SeleniumHelper.GenerateRandomString(30);
     
            string phoneNumber = "123456789";
            string doctorTitle = "Mr";
            string doctorSureName = SeleniumHelper.GenerateRandomString(10);
            string doctorMidleName = SeleniumHelper.GenerateRandomString(10);
            string doctorForeName = SeleniumHelper.GenerateRandomString(10);
            var data = new List<Object[]>
            {                
                new object[] {practiceName,phoneNumber,doctorTitle,doctorSureName,doctorMidleName,doctorForeName}
            };
            return data;
        }

        public List<object[]> TC_COM007_Data()
        {
            string practiceName = "TC07 " + SeleniumHelper.GenerateRandomString(30);
            string phoneNumber = "123456789";
            string doctorTitle = "Mr";
            string doctorSureName = SeleniumHelper.GenerateRandomString(10);
            string doctorMidleName = SeleniumHelper.GenerateRandomString(10);
            string doctorForeName = SeleniumHelper.GenerateRandomString(10);
         
            string practiceNameUpdate = practiceName; // "Update Practice Name " + SeleniumHelper.GenerateRandomString(10);
            string phoneNumberUpdate = "98765432";
            string doctorTitleUpdate = "Mr";
            string doctorSureNameUpdate = "SureName " + SeleniumHelper.GenerateRandomString(10);
            string doctorMidleNameUpdate = "MiddleName " + SeleniumHelper.GenerateRandomString(10);
            string doctorForeNameUpdate = "ForeName " + SeleniumHelper.GenerateRandomString(10);
            var data = new List<Object[]>
            {                
                new object[] {practiceName,phoneNumber,doctorTitle,doctorSureName,doctorMidleName,doctorForeName,
                    practiceNameUpdate,phoneNumberUpdate,doctorTitleUpdate,doctorSureNameUpdate,doctorMidleNameUpdate,doctorForeNameUpdate}
            };
            return data;
        }

        public List<object[]> TC_COM_8_Data()
        {
            var randomMedicalPratice = "TC08" + SeleniumHelper.GenerateRandomString(4) + SeleniumHelper.GenerateRandomNumberUsingDateTime();

            var res = new List<Object[]>
            {                
                new object[] 
                {
                    // pupil name
            
                    // medical practice name
                    randomMedicalPratice
                    
                }
                
            };
            return res;
        }

        public List<object[]> TC_COM_9_Data()
        {
            var randomMedicalPratice = "TC09" + SeleniumHelper.GenerateRandomString(4) + SeleniumHelper.GenerateRandomNumberUsingDateTime();

            var res = new List<Object[]>
            {                
                new object[] 
                {
                    // pupil name
               
                    // medical practice name
                    randomMedicalPratice
                    
                }
                
            };
            return res;
        }

        public List<object[]> TC_COM_10_Data()
        {
            var randomServiceTypeCode = "TC10" + SeleniumHelper.GenerateRandomString(3) + SeleniumHelper.GenerateRandomNumber(4);
            var randomServiceTypeDes = "TC10" + SeleniumHelper.GenerateRandomString(4) + SeleniumHelper.GenerateRandomNumber(4);
            var randomAgent = "TC10" + SeleniumHelper.GenerateRandomString(4) + SeleniumHelper.GenerateRandomNumber(4);

            var res = new List<Object[]>
            {                
                new object[] 
                {
                    // sTypeCode
                    randomServiceTypeCode,
                    // sTypeDescription
                    randomServiceTypeDes,
                    // agentForename
                    randomAgent,
                    // agentSurname
                    randomAgent,
                    
                }
                
            };
            return res;
        }

        public List<object[]> TC_COM_11_Data()
        {
            var randomServiceTypeCode = "TC11" + SeleniumHelper.GenerateRandomString(3) + SeleniumHelper.GenerateRandomNumber(4);
            var randomServiceTypeDes = "TC11" + SeleniumHelper.GenerateRandomString(4) + SeleniumHelper.GenerateRandomNumber(4);
            var randomAgent = "TC11" + SeleniumHelper.GenerateRandomString(4) + SeleniumHelper.GenerateRandomNumber(4);

            var res = new List<Object[]>
            {                
                new object[] 
                {
                    // sTypeCode
                    randomServiceTypeCode,
                    // sTypeDescription
                    randomServiceTypeDes,
                    // agentForename
                    randomAgent,
                    // agentSurname
                    randomAgent,
                    
                }
                
            };
            return res;
        }

        public List<object[]> TC_COM_12_Data()
        {
            var randomServiceTypeCode = "TC12" + SeleniumHelper.GenerateRandomString(3) + SeleniumHelper.GenerateRandomNumber(4);
            var randomServiceTypeDes = "TC12" + SeleniumHelper.GenerateRandomString(4) + SeleniumHelper.GenerateRandomNumber(3);
            var randomServiceTypeCodeEdit = "TC12" + SeleniumHelper.GenerateRandomString(3) + SeleniumHelper.GenerateRandomNumber(3);
            var randomServiceTypeDesEdit = "TC12" + SeleniumHelper.GenerateRandomString(4) + SeleniumHelper.GenerateRandomNumber(3);
            var randomAgent = "TC12Agent" + SeleniumHelper.GenerateRandomString(4) + SeleniumHelper.GenerateRandomNumber(4);

            var res = new List<Object[]>
            {                
                new object[] 
                {
                    // sTypeCode
                    randomServiceTypeCode,
                    // sTypeDescription
                    randomServiceTypeDes,
                    // sTypeCodeEdit
                    randomServiceTypeCodeEdit,
                    // sTypeDescriptionEdit
                    randomServiceTypeDesEdit,
                    // agentForename
                    randomAgent,
                    // agentSurname
                    randomAgent
                }
                
            };
            return res;
        }

        public List<object[]> TC_COM_13_Data()
        {
            var randomServiceTypeCode = "Lgb" + SeleniumHelper.GenerateRandomString(3) + SeleniumHelper.GenerateRandomNumber(4);
            var randomServiceTypeDes = "Lgb" + SeleniumHelper.GenerateRandomString(4) + SeleniumHelper.GenerateRandomNumber(4);
            var randomAgent = "LgAgent" + SeleniumHelper.GenerateRandomString(4) + SeleniumHelper.GenerateRandomNumber(4);

            var res = new List<Object[]>
            {                
                new object[] 
                {
                    // sTypeCode
                    randomServiceTypeCode,
                    // sTypeDescription
                    randomServiceTypeDes,
                    // agentForename
                    randomAgent,
                    // agentSurname
                    randomAgent,
                    
                }
                
            };
            return res;
        }

        public List<object[]> TC_COM14_DATA()
        {
            string surName = SeleniumHelper.GenerateRandomString(8);
            string foreName = SeleniumHelper.GenerateRandomString(8);
            string telephoneNumber = SeleniumHelper.GenerateRandomNumberUsingDateTime(8).ToString();
            string note = "Use this Number";
            string email = String.Format("{0}@capita.co.uk", SeleniumHelper.GenerateRandomString(8));
            string emailNote = "Use this email";
            string documentSummary = SeleniumHelper.GenerateRandomString(10);
            string documentNote = SeleniumHelper.GenerateRandomString(15);
            string country = "United Kingdom";
            string postcode = "BT57 8RR";

            var data = new List<Object[]>
            {
                new object[] { surName, foreName, "Doctor", "Assessment and Qualifications Alliance", telephoneNumber, "Home", true, 
                note, email, true, emailNote, postcode , "20 Bushfoot Road Portballintrae Bushmills BT57 8RR", documentSummary, documentNote, country }

            };
            return data;
        }

        public List<object[]> TC_COM15_DATA()
        {
            string pattern = "M/d/yyyy";
            string pupilSurName = "Logigear_" + SeleniumHelper.GenerateRandomString(8);
            string pupilForeName = SeleniumHelper.GenerateRandomString(8);
            string dateOfBirth = DateTime.ParseExact("10/01/2006", pattern, CultureInfo.InvariantCulture).ToString(pattern);

            string DateOfAdmission = DateTime.ParseExact("10/09/2010", pattern, CultureInfo.InvariantCulture).ToString(pattern);

            string surName = SeleniumHelper.GenerateRandomString(8);
            string foreName = SeleniumHelper.GenerateRandomString(8);
            string telephoneNumber = SeleniumHelper.GenerateRandomNumberUsingDateTime(8).ToString();
            string note = "Use this Number";
            string email = String.Format("{0}@capita.co.uk", SeleniumHelper.GenerateRandomString(8));
            string emailNote = "Use this email";
            string documentSummary = SeleniumHelper.GenerateRandomString(10);
            string documentNote = SeleniumHelper.GenerateRandomString(15);
            string postcode = "BT57 8QF";


            var data = new List<Object[]>
            {
                new object[] { surName, foreName, "Doctor", "Assessment and Qualifications Alliance", telephoneNumber, "Home", true, 
                note, email, true, emailNote, postcode , "29 Woodvale Park BUSHMILLS Co. Antrim BT57 8QF",pupilSurName, pupilForeName, "Male", dateOfBirth, DateOfAdmission, "Year 2" }

            };
            return data;
        }

        public List<object[]> TC_COM16_DATA()
        {

            string surName = SeleniumHelper.GenerateRandomString(8);
            string foreName = SeleniumHelper.GenerateRandomString(8);
            string telephoneNumber = SeleniumHelper.GenerateRandomNumberUsingDateTime(8).ToString();
            string note = "Use this Number";
            string email = String.Format("{0}@capita.co.uk", SeleniumHelper.GenerateRandomString(8));
            string emailNote = "Use this email";
            string documentSummary = SeleniumHelper.GenerateRandomString(10);
            string documentNote = SeleniumHelper.GenerateRandomString(15);
            string postcode = "BT57 8QF";
            string country = "United Kingdom";

            string newSurName = String.Format("New_{0}", SeleniumHelper.GenerateRandomString(8));
            string newForeName = String.Format("New_{0}", SeleniumHelper.GenerateRandomString(8));
            string newTelephoneNumber = String.Format("999 {0}", SeleniumHelper.GenerateRandomNumberUsingDateTime(8).ToString());
            string newEmailAddress = String.Format("New_{0}@capita.co.uk", SeleniumHelper.GenerateRandomString(8));
            string newBuildingNo = String.Format("999 {0}", SeleniumHelper.GenerateRandomNumberUsingDateTime(6).ToString());
            string newBuildingName = String.Format("New_{0}", SeleniumHelper.GenerateRandomString(8));
            string newDocSummary = String.Format("New_{0}", SeleniumHelper.GenerateRandomString(8));
            string newDocNote = String.Format("New_{0}", SeleniumHelper.GenerateRandomString(25));

            string pattern = "M/d/yyyy";
            string pupilSurname = "Logigear_" + SeleniumHelper.GenerateRandomString(8);
            string pupilForename = SeleniumHelper.GenerateRandomString(8);
            string dateOfBirth = DateTime.ParseExact("10/06/2006", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            string DateOfAdmission = DateTime.ParseExact("10/09/2010", pattern, CultureInfo.InvariantCulture).ToString(pattern);


            var data = new List<Object[]>
            {
                new object[] { surName, foreName, "Doctor", "Assessment and Qualifications Alliance", telephoneNumber, "Home", true, 
                note, email, true, emailNote, postcode , "29 Woodvale Park BUSHMILLS Co. Antrim BT57 8QF", documentSummary, documentNote,
                newSurName, newForeName, "Instructor", "Associated Examining Board", newTelephoneNumber, "Work", false, "New_Use this Number", newEmailAddress,
                false, "New_Use this email", newBuildingNo, newBuildingName, newDocSummary, newDocNote, country, pupilSurname, pupilForename, "Male", dateOfBirth,
                DateOfAdmission, "Year 2"}

            };
            return data;
        }

        public List<object[]> TC_COM17_DATA()
        {
            string surName = SeleniumHelper.GenerateRandomString(8);
            string foreName = SeleniumHelper.GenerateRandomString(8);
            string telephoneNumber = SeleniumHelper.GenerateRandomNumberUsingDateTime(8).ToString();
            string note = "Use this Number";
            string email = String.Format("{0}@capita.co.uk", SeleniumHelper.GenerateRandomString(8));
            string emailNote = "Use this email";
            string documentSummary = SeleniumHelper.GenerateRandomString(10);
            string documentNote = SeleniumHelper.GenerateRandomString(15);
            string country = "United Kingdom";
            string postcode = "BT57 8YL";

            var data = new List<Object[]>
            {
                new object[] { surName, foreName, "Doctor", "Assessment and Qualifications Alliance", telephoneNumber, "Home", true, 
                note, email, true, emailNote, postcode , "13 Haw Road Bushmills Co. Antrim BT57 8YL", documentSummary, documentNote, country }

            };
            return data;
        }

        public List<object[]> TC_COM18_DATA()
        {
            string pattern = "d/M/yyyy";
            string pupilSurName = "Logigear_" + SeleniumHelper.GenerateRandomString(8);
            string pupilForeName = SeleniumHelper.GenerateRandomString(8);
            string dateOfBirth = DateTime.ParseExact("16/10/2006", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            string DateOfAdmission = DateTime.ParseExact("16/10/2010", pattern, CultureInfo.InvariantCulture).ToString(pattern);

            string surName = SeleniumHelper.GenerateRandomString(8);
            string foreName = SeleniumHelper.GenerateRandomString(8);
            string telephoneNumber = SeleniumHelper.GenerateRandomNumberUsingDateTime(8).ToString();
            string note = "Use this Number";
            string email = String.Format("{0}@capita.co.uk", SeleniumHelper.GenerateRandomString(8));
            string emailNote = "Use this email";

            var data = new List<Object[]>
            {
                new object[] { surName, foreName, "Doctor", "Assessment and Qualifications Alliance", telephoneNumber, "Home", true, 
                note, email, true, emailNote, pupilSurName, pupilForeName, "Male", dateOfBirth, DateOfAdmission, "Year 2" }

            };
            return data;
        }

        public List<object[]> TC_COM19_DATA()
        {

            string surName = SeleniumHelper.GenerateRandomString(8);
            string foreName = SeleniumHelper.GenerateRandomString(8);
            string telephoneNumber = SeleniumHelper.GenerateRandomNumberUsingDateTime(8).ToString();
            string note = "Use this Number";
            string email = String.Format("{0}@capita.co.uk", SeleniumHelper.GenerateRandomString(8));
            string emailNote = "Use this email";
            string documentSummary = SeleniumHelper.GenerateRandomString(10);
            string documentNote = SeleniumHelper.GenerateRandomString(15);
            string postcode = "BT57 8YL";
            string country = "United Kingdom";

            string newSurName = String.Format("New_{0}", SeleniumHelper.GenerateRandomString(8));
            string newForeName = String.Format("New_{0}", SeleniumHelper.GenerateRandomString(8));
            string newTelephoneNumber = String.Format("999 {0}", SeleniumHelper.GenerateRandomNumberUsingDateTime(8).ToString());
            string newEmailAddress = String.Format("New_{0}@capita.co.uk", SeleniumHelper.GenerateRandomString(8));
            string newBuildingNo = String.Format("999 {0}", SeleniumHelper.GenerateRandomNumberUsingDateTime(6).ToString());
            string newBuildingName = String.Format("New_{0}", SeleniumHelper.GenerateRandomString(8));
            string newDocSummary = String.Format("New_{0}", SeleniumHelper.GenerateRandomString(8));
            string newDocNote = String.Format("New_{0}", SeleniumHelper.GenerateRandomString(25));

            string pattern = "d/M/yyyy";
            string pupilSurname = "Logigear_" + SeleniumHelper.GenerateRandomString(8);
            string pupilForename = SeleniumHelper.GenerateRandomString(8);
            string dateOfBirth = DateTime.ParseExact("16/10/2006", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            string DateOfAdmission = DateTime.ParseExact("16/10/2010", pattern, CultureInfo.InvariantCulture).ToString(pattern);


            var data = new List<Object[]>
            {
                new object[] { surName, foreName, "Doctor", "Assessment and Qualifications Alliance", telephoneNumber, "Home", true, 
                note, email, true, emailNote, postcode , "Haw Road Bushmills Co. Antrim BT57 8YL", documentSummary, documentNote,
                newSurName, newForeName, "Instructor", "Associated Examining Board", newTelephoneNumber, "Work", false, "New_Use this Number", newEmailAddress,
                false, "New_Use this email", newBuildingNo, newBuildingName, newDocSummary, newDocNote, country, pupilSurname, pupilForename, "Male", dateOfBirth,
                DateOfAdmission, "Year 2" }

            };
            return data;
        }

        public List<object[]> TC_COM020_Data()
        {
            var agentName = String.Format("TC20_{0}_{1}", SeleniumHelper.GenerateRandomString(4), SeleniumHelper.GenerateRandomNumberUsingDateTime(2));

            var res = new List<Object[]>
            {                
                new object[] {
                    new string[]{ agentName, agentName, "Teacher Agency"},
                    new string[]{ String.Format("{0}_Update", agentName), String.Format("{0}_Update", agentName), "Other"},
                    new string[]{ SeleniumHelper.GenerateRandomNumberUsingDateTime(5), "Home", "Use this Number"},
                    new string[]{ String.Format("{0}@gmail.com", SeleniumHelper.GenerateRandomString(5)), "Use this email"},
                    new string[]{ "Document", "Test upload document for an agent", "document.txt"},
                    "Akeman, Rebecca"
                },
            };
            return res;
        }

        public List<object[]> TC_COM021_Data()
        {
            var agencyName = String.Format("TC21_{0}_{1}", SeleniumHelper.GenerateRandomString(4), SeleniumHelper.GenerateRandomNumberUsingDateTime(2));

            var res = new List<Object[]>
            {                
                new object[] {
                    new string[]{ agencyName, "Teacher Agency"},
                    new string[]{ SeleniumHelper.GenerateRandomNumberUsingDateTime(5), "Home", "Use this Number"},
                    new string[]{ String.Format("{0}@gmail.com", SeleniumHelper.GenerateRandomString(5)), "Use this email"},
                    new string[]{ "Document", "Test upload document for an agent", "document.txt"},
                    "http://google.com.vn"
                },
            };
            return res;
        }

        public List<object[]> TC_COM022_Data()
        {
            var agencyName = String.Format("COM022_{0}_{1}", SeleniumHelper.GenerateRandomString(5), SeleniumHelper.GenerateRandomNumberUsingDateTime(5));

            var res = new List<Object[]>
            {                
                new object[] {
                    new string[]{ agencyName, "Teacher Agency"},
                    "Akeman, Rebecca"
                },
            };
            return res;
        }

        public List<object[]> TC_COM023_Data()
        {
            var agencyName = String.Format("COM023_{0}_{1}", SeleniumHelper.GenerateRandomString(5), SeleniumHelper.GenerateRandomNumberUsingDateTime(5));

            var res = new List<Object[]>
            {                
                new object[] {
                    new string[]{ agencyName, "Teacher Agency"},
                    new string[]{ String.Format("{0}_Update", agencyName), "Other"},
                    new string[]{ SeleniumHelper.GenerateRandomNumberUsingDateTime(5), "Home", "Use this Number"},
                    new string[]{ String.Format("{0}@gmail.com", SeleniumHelper.GenerateRandomString(8)), "Use this email"},
                    new string[]{ "Document", "Test upload document for an agent", "document.txt"},
                    "Ward, Lucy"
                },
            };
            return res;
        }

        public List<object[]> TC_COM024_Data()
        {
            var agencyName = String.Format("COM024_{0}_{1}", SeleniumHelper.GenerateRandomString(5), SeleniumHelper.GenerateRandomNumberUsingDateTime(5));

            var res = new List<Object[]>
            {                
                new object[] {
                    new string[]{ agencyName, "Teacher Agency"},
                    new string[]{ String.Format("{0}_Update", agencyName), "Other"},
                    new string[]{ SeleniumHelper.GenerateRandomNumberUsingDateTime(5), "Home", "Use this Number"},
                    new string[]{ String.Format("{0}@gmail.com", SeleniumHelper.GenerateRandomString(8)), "Use this email"},
                    new string[]{ "Document", "Test upload document for an agent", "document.txt"},
                    "Akeman, Rebecca"
                },
            };
            return res;
        }

        public List<object[]> TC_COM025_Data()
        {
            var agencyName = String.Format("COM025_{0}_{1}", SeleniumHelper.GenerateRandomString(8), SeleniumHelper.GenerateRandomNumberUsingDateTime(5));

            var res = new List<Object[]>
            {                
                new object[] {
                    new string[]{ agencyName, "Teacher Agency"},
                    new string[]{ SeleniumHelper.GenerateRandomNumberUsingDateTime(5), "Home", "Use this Number"},
                    new string[]{ String.Format("{0}@gmail.com", SeleniumHelper.GenerateRandomString(8)), "Use this email"},
                    new string[]{ "Document", "Test upload document for an agent", "document.txt"},
                    "http://google.com.vn"
                },
            };
            return res;
        }

        public List<object[]> TC_COM026_Data()
        {
            var agencyName = String.Format("COM026_{0}_{1}", SeleniumHelper.GenerateRandomString(5), SeleniumHelper.GenerateRandomNumberUsingDateTime(5));

            var res = new List<Object[]>
            {                
                new object[] {
                    new string[]{ agencyName, "Teacher Agency"},
                    "Jackson, Chloe"
                },
            };
            return res;
        }

        public List<object[]> TC_COM027_Data()
        {
            string agencyName = String.Format("{0}_{1}_{2}", "Avn", SeleniumHelper.GenerateRandomString(7), SeleniumHelper.GenerateRandomNumberUsingDateTime());
            string editAgencyName = String.Format("{0}_{1}_{2}_{3}", "Avn", "Edited", SeleniumHelper.GenerateRandomString(7), SeleniumHelper.GenerateRandomNumberUsingDateTime());
            var res = new List<Object[]>
            {                
                new object[] 
                {   
                    agencyName,  "Audiometrist", "Doctor", "Social Services", "0123456789", "Home", "Phone note",
                    "abcdefgh@c2kni.org.uk", "Email note", "Dcoument Sumary", "Document Note",
                    editAgencyName, "0123456788", "Work", "Edit phone note",
                    "abcd@c2kni.org.uk", "Edit Email note", "Edit Dcoument Sumary", "Edit Document Note",
                }
                
            };
            return res;
        }

        public List<object[]> TC_COM028_Data()
        {
            string agencyName = String.Format("{0}_{1}_{2}", "Avn", SeleniumHelper.GenerateRandomString(7), SeleniumHelper.GenerateRandomNumberUsingDateTime());
            var res = new List<Object[]>
            {                
                new object[] 
                {                    
                    agencyName, "Audiometrist", "Doctor",
                }
                
            };
            return res;
        }

        public List<object[]> TC_COM029_Data()
        {
            string agencyName = String.Format("{0}_{1}_{2}", "Avn", SeleniumHelper.GenerateRandomString(7), SeleniumHelper.GenerateRandomNumberUsingDateTime());
            var res = new List<Object[]>
            {                
                new object[] 
                {                    
                    agencyName, "Audiometrist", "Doctor", "Social Services"
                }
                
            };
            return res;
        }

        public List<object[]> TC_COM030_Data()
        {
            string foreName = String.Format("{0}_{1}", SeleniumHelper.GenerateRandomString(7), SeleniumHelper.GenerateRandomNumberUsingDateTime());
            string surName = String.Format("{0}_{1}", "Avn", SeleniumHelper.GenerateRandomString(8));
            var res = new List<Object[]>
            {                
                new object[] 
                {                    
                    foreName, surName, "Audiometrist",
                }
                
            };
            return res;
        }

        public List<object[]> TC_COM031_Data()
        {
            string foreName = String.Format("{0}_{1}", SeleniumHelper.GenerateRandomString(7), SeleniumHelper.GenerateRandomNumberUsingDateTime());
            string surName = String.Format("{0}_{1}", "Avn", SeleniumHelper.GenerateRandomString(8));
            var res = new List<Object[]>
            {                
                new object[] 
                {                    
                    foreName, surName, "Audiometrist", "Doctor", "Social Services", "Assessment and Qualifications Alliance"
                }
                
            };
            return res;
        }

        public List<object[]> TC_COM032_Data()
        {
            string title = String.Format("{0}_{1}", "Notice", SeleniumHelper.GenerateRandomString(8));
            string notice = String.Format("{0}_{1}_{2}", "Avn", SeleniumHelper.GenerateRandomString(8), SeleniumHelper.GenerateRandomNumberUsingDateTime());
            string today = DateTime.Now.ToString("M/d/yyyy");
            var res = new List<Object[]>
            {                
                new object[] 
                {                    
                    title, notice, today, today
                }
                
            };
            return res;
        }

        public List<object[]> TC_COM033_Data()
        {
            // Old Start Date                        
            var res = new List<Object[]>
            {                
                new object[] {
                    new string[]{ "Expired"}   
                },
            };
            return res;
        }

        public List<object[]> TC_COM034_Data()
        {
            string pattern = "M/d/yyyy";

            // Old Start Date            
            string stratDate = DateTime.Now.ToString(pattern);
            string endDate = DateTime.Now.AddDays(1).ToString(pattern);
            string noticeBoardTitle = "Notice " + SeleniumHelper.GenerateRandomString(6);

            var res = new List<Object[]>
            {                
                new object[] {
                    new string[]{ noticeBoardTitle,stratDate,endDate},
                },
            };
            return res;
        }

        public List<object[]> TC_COM035_Data()
        {
            string pattern = "M/d/yyyy";

            // Old Start Date            
            string stratDate = DateTime.Now.ToString(pattern);
            string endDate = DateTime.Now.AddDays(1).ToString(pattern);
            string noticeBoardTitle = "Notice " + SeleniumHelper.GenerateRandomString(6);

            var res = new List<Object[]>
            {                
                new object[] {
                    new string[]{ noticeBoardTitle,stratDate,endDate},
                },
            };
            return res;
        }

        public List<object[]> TC_COM037_Data()
        {
            string pattern = "M/d/yyyy";

            // Old Start Date            
            string stratDate = DateTime.Now.ToString(pattern);
            string endDate = DateTime.Now.AddDays(2).ToString(pattern);
            string noticeBoardTitle = "Notice " + SeleniumHelper.GenerateRandomString(6);
            string futureDate = DateTime.Now.AddDays(1).ToString(pattern);
            var res = new List<Object[]>
            {                
                new object[] {
                    new string[]{ noticeBoardTitle,stratDate,endDate},
                    futureDate
                },
            };
            return res;
        }

        public List<object[]> TC_COM039_Data()
        {
            string pattern = "M/d/yyyy";

            // Old Start Date
            string stratDate = DateTime.Now.ToString(pattern);
            string endDate = DateTime.Now.AddDays(1).ToString(pattern);
            string noticeBoardTitle = "Notice " + SeleniumHelper.GenerateRandomString(6);

            var res = new List<Object[]>
            {                
                new object[] {
                    new string[]{ noticeBoardTitle,stratDate,endDate}                    
                },
            };
            return res;
        }


        #endregion
        
    }
}

