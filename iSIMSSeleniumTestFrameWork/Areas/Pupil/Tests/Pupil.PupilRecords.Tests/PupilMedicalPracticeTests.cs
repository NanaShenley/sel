using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using POM.Base;
using POM.Components.Pupil;
using POM.Components.Pupil.Dialogs;
using POM.Helper;
using Pupil.Components.Common;
using Pupil.Data;
using Selene.Support.Attributes;
using SeSugar;
using SeSugar.Automation;
using SeSugar.Data;
using System;
using TestSettings;

namespace Pupil.PupilRecords.Tests
{
    public class PupilMedicalPracticeTests
    {
        #region Private Parameters

        private readonly int tenantID = SeSugar.Environment.Settings.TenantId;
        private readonly DateTime startDate = DateTime.Today.AddDays(-1);
        private readonly By findAddressSection = By.CssSelector("#dialog-dialog-editableData [data-automation-id='section_menu_Addresses']");
        private readonly By clickAddAddress = By.CssSelector("#dialog-dialog-editableData [data-automation-id='add_an_address_button']");

        #endregion

        #region Edit Medical Practice Popup

        [WebDriverTest(TimeoutSeconds = 1200, Enabled = true, Browsers = new[] { BrowserDefaults.Chrome },
            Groups = new[]
            {
                PupilTestGroups.PupilRecord.Page,
                PupilTestGroups.Priority.Priority1,
                "DoctorAddress",
                "Edit_Medical_Practice_Add_Doctor_Address_Local"
            })]
        [Variant(Variant.NorthernIrelandStatePrimary | Variant.IndependentPrimary )]
        public void Edit_Medical_Practice_Add_Doctor_Address_Local()
        {
            #region Arrange

            Guid pupilId, medicalPracticeId, doctorId, pupilMedicalPracticeId, addressId;

            string medicalPracticeName;

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

            #endregion

            using (new DataSetup(CreatePupilData(out pupilId),
                CreateMedicalPracticeData(out medicalPracticeId, out doctorId, out medicalPracticeName),
                CreatePupilMedicalPracticeData(out pupilMedicalPracticeId, medicalPracticeId, pupilId),
                GetAddress(out addressId, UPRN, PAONDescription, PAONRange, SAON, Street, Locality, Town, AdministrativeArea, PostCode, Country)))
            {
                LoginAndNavigate(SeleniumHelper.iSIMSUserType.SchoolAdministrator, "Pupil Records", "Addresses");

                //Get Pupil record
                PupilRecordPage pupilRecordPage = LoadPupil(pupilId);
                pupilRecordPage.SelectMedicalTab();
                var gridRow = pupilRecordPage.MedicalPractice.Rows[0];
                gridRow.ClickEdit();

                EditMedicalPracticeDialog medicalPractice = new EditMedicalPracticeDialog();

                var gridRow2 = medicalPractice.Doctors.Rows[0];
                gridRow2.ClickEdit();

                DoctorsDialog doctor = new DoctorsDialog();

                SeleniumHelper.FindAndClick(findAddressSection);
                SeleniumHelper.FindAndClick(clickAddAddress);

                AddAddressPopup addAddress = new AddAddressPopup();
                addAddress.PAONRangeSearch = PAONRange;
                addAddress.PostCodeSearch = PostCode;
                addAddress.ClickSearch();

                addAddress.Addresses = addressDisplaySmall;

                Assert.AreEqual(PAONRange, addAddress.BuildingNo);
                Assert.AreEqual(Street, addAddress.Street);
                Assert.AreEqual(Town, addAddress.Town);
                Assert.AreEqual(AdministrativeArea, addAddress.County);
                Assert.AreEqual(PostCode, addAddress.PostCode);

                addAddress.ClickOk();
                doctor.OK();
                medicalPractice.ClickOk();
                pupilRecordPage.ClickSave();

                pupilRecordPage = LoadPupil(pupilId);
                pupilRecordPage.SelectMedicalTab();
                gridRow = pupilRecordPage.MedicalPractice.Rows[0];
                gridRow.ClickEdit();

                gridRow2 = medicalPractice.Doctors.Rows[0];
                gridRow2.ClickEdit();

                doctor = new DoctorsDialog();

                SeleniumHelper.FindAndClick(findAddressSection);

                var gridRow3 = doctor.DoctorAddressTable.Rows[0];

                Assert.AreEqual(addressDisplayLarge, gridRow3.Address);
            }
        }

        [WebDriverTest(TimeoutSeconds = 1200, Enabled = true, Browsers = new[] { BrowserDefaults.Chrome },
            Groups = new[]
            {
                PupilTestGroups.PupilRecord.Page,
                PupilTestGroups.Priority.Priority1,
                "DoctorAddress",
                "Edit_Medical_Practice_Add_Doctor_Address_WAV"
            })]
        [Variant(Variant.EnglishStatePrimary )]
        public void Edit_Medical_Practice_Add_Doctor_Address_WAV()
        {
            #region Arrange

            Guid pupilId, medicalPracticeId, doctorId, pupilMedicalPracticeId;

            string medicalPracticeName;

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

            #endregion

            using (new DataSetup(CreatePupilData(out pupilId),
                CreateMedicalPracticeData(out medicalPracticeId, out doctorId, out medicalPracticeName),
                CreatePupilMedicalPracticeData(out pupilMedicalPracticeId, medicalPracticeId, pupilId)))
            {
                LoginAndNavigate(SeleniumHelper.iSIMSUserType.SchoolAdministrator, "Pupil Records", "Addresses");

                //Get Pupil record
                PupilRecordPage pupilRecordPage = LoadPupil(pupilId);
                pupilRecordPage.SelectMedicalTab();
                var gridRow = pupilRecordPage.MedicalPractice.Rows[0];
                gridRow.ClickEdit();

                EditMedicalPracticeDialog medicalPractice = new EditMedicalPracticeDialog();

                var gridRow2 = medicalPractice.Doctors.Rows[0];
                gridRow2.ClickEdit();

                DoctorsDialog doctor = new DoctorsDialog();

                SeleniumHelper.FindAndClick(findAddressSection);
                SeleniumHelper.FindAndClick(clickAddAddress);

                AddAddressPopup addAddress = new AddAddressPopup();
                addAddress.PAONRangeSearch = PAONRange;
                addAddress.PostCodeSearch = PostCode;
                addAddress.ClickSearch();

                addAddress.Addresses = addressDisplaySmall;

                Assert.AreEqual(PAONRange, addAddress.BuildingNo);
                Assert.AreEqual(Street, addAddress.Street);
                Assert.AreEqual(Town, addAddress.Town);
                Assert.AreEqual(Town, addAddress.County);
                Assert.AreEqual(PostCode, addAddress.PostCode);

                addAddress.ClickOk();
                doctor.ClickOk();
                medicalPractice.ClickOk();
                pupilRecordPage.ClickSave();

                pupilRecordPage = LoadPupil(pupilId);
                pupilRecordPage.SelectMedicalTab();
                gridRow = pupilRecordPage.MedicalPractice.Rows[0];
                gridRow.ClickEdit();

                gridRow2 = medicalPractice.Doctors.Rows[0];
                gridRow2.ClickEdit();

                doctor = new DoctorsDialog();

                SeleniumHelper.FindAndClick(findAddressSection);

                var gridRow3 = doctor.DoctorAddressTable.Rows[0];

                Assert.AreEqual(addressDisplayLarge, gridRow3.Address);
            }
        }

        [WebDriverTest(TimeoutSeconds = 1200, Enabled = true, Browsers = new[] { BrowserDefaults.Chrome },
            Groups = new[]
            {
                PupilTestGroups.PupilRecord.Page,
                PupilTestGroups.Priority.Priority1,
                "DoctorAddress",
                "Edit_Medical_Practice_Edit_Doctor_Address_Fields"
            })]
        [Variant(Variant.EnglishStatePrimary )]
        public void Edit_Medical_Practice_Edit_Doctor_Address_Fields()
        {
            #region Arrange

            Guid pupilId, medicalPracticeId, doctorId, pupilMedicalPracticeId, addressId, doctorAddressId;

            string medicalPracticeName;

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

            #endregion

            using (new DataSetup(CreatePupilData(out pupilId),
                CreateMedicalPracticeData(out medicalPracticeId, out doctorId, out medicalPracticeName),
                CreatePupilMedicalPracticeData(out pupilMedicalPracticeId, medicalPracticeId, pupilId),
                GetAddress(out addressId, UPRN, PAONDescription, PAONRange, SAON, Street, Locality, Town, AdministrativeArea, PostCode, Country),
                CreateDoctorAddresseData(out doctorAddressId, DateTime.Today, "H", doctorId, addressId)))
            {
                LoginAndNavigate(SeleniumHelper.iSIMSUserType.SchoolAdministrator, "Pupil Records", "Addresses");

                PupilRecordPage pupilRecordPage = LoadPupil(pupilId);
                pupilRecordPage.SelectMedicalTab();
                var gridRow = pupilRecordPage.MedicalPractice.Rows[0];
                gridRow.ClickEdit();

                EditMedicalPracticeDialog medicalPractice = new EditMedicalPracticeDialog();

                var gridRow2 = medicalPractice.Doctors.Rows[0];
                gridRow2.ClickEdit();

                DoctorsDialog doctor = new DoctorsDialog();

                SeleniumHelper.FindAndClick(findAddressSection);

                var gridRow3 = doctor.DoctorAddressTable.Rows[0];
                gridRow3.Edit();

                AddAddressPopup addAddress = new AddAddressPopup();
                addAddress.District = newLocality;

                addAddress.ClickOk();
                doctor.ClickOk();
                medicalPractice.ClickOk();
                pupilRecordPage.ClickSave();

                pupilRecordPage = LoadPupil(pupilId);
                pupilRecordPage.SelectMedicalTab();
                gridRow = pupilRecordPage.MedicalPractice.Rows[0];
                gridRow.ClickEdit();

                gridRow2 = medicalPractice.Doctors.Rows[0];
                gridRow2.ClickEdit();

                doctor = new DoctorsDialog();

                SeleniumHelper.FindAndClick(findAddressSection);

                gridRow3 = doctor.DoctorAddressTable.Rows[0];
                gridRow3.Edit();

                addAddress = new AddAddressPopup();
                Assert.AreEqual(newLocality, addAddress.District);
            }
        }

        [WebDriverTest(TimeoutSeconds = 1200, Enabled = true, Browsers = new[] { BrowserDefaults.Chrome },
            Groups = new[]
            {
                PupilTestGroups.PupilRecord.Page,
                PupilTestGroups.Priority.Priority1,
                "DoctorAddress",
                "Edit_Medical_Practice_Edit_Doctor_Address_New_Address"
            })]
        [Variant(Variant.EnglishStatePrimary)]
        public void Edit_Medical_Practice_Edit_Doctor_Address_New_Address()
        {
            #region Arrange

            Guid pupilId, medicalPracticeId, doctorId, pupilMedicalPracticeId, addressId, doctorAddressId;

            string medicalPracticeName;

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

            #endregion

            using (new DataSetup(CreatePupilData(out pupilId),
                CreateMedicalPracticeData(out medicalPracticeId, out doctorId, out medicalPracticeName),
                CreatePupilMedicalPracticeData(out pupilMedicalPracticeId, medicalPracticeId, pupilId),
                GetAddress(out addressId, UPRN, PAONDescription, PAONRange, SAON, Street, Locality, Town, AdministrativeArea, PostCode, Country),
                CreateDoctorAddresseData(out doctorAddressId, DateTime.Today, "H", doctorId, addressId)))
            {
                LoginAndNavigate(SeleniumHelper.iSIMSUserType.SchoolAdministrator, "Pupil Records", "Addresses");

                PupilRecordPage pupilRecordPage = LoadPupil(pupilId);
                pupilRecordPage.SelectMedicalTab();
                var gridRow = pupilRecordPage.MedicalPractice.Rows[0];
                gridRow.ClickEdit();

                EditMedicalPracticeDialog medicalPractice = new EditMedicalPracticeDialog();

                var gridRow2 = medicalPractice.Doctors.Rows[0];
                gridRow2.ClickEdit();

                DoctorsDialog doctor = new DoctorsDialog();

                SeleniumHelper.FindAndClick(findAddressSection);

                var gridRow3 = doctor.DoctorAddressTable.Rows[0];
                gridRow3.Edit();

                AddAddressPopup addAddress = new AddAddressPopup();
                addAddress.PAONRangeSearch = WAVPAONRange;
                addAddress.PostCodeSearch = WAVPostCode;
                addAddress.ClickSearch();

                addAddress.ClickOk();
                doctor.ClickOk();
                medicalPractice.ClickOk();
                pupilRecordPage.ClickSave();

                pupilRecordPage = LoadPupil(pupilId);
                pupilRecordPage.SelectMedicalTab();
                gridRow = pupilRecordPage.MedicalPractice.Rows[0];
                gridRow.ClickEdit();

                gridRow2 = medicalPractice.Doctors.Rows[0];
                gridRow2.ClickEdit();

                doctor = new DoctorsDialog();

                SeleniumHelper.FindAndClick(findAddressSection);

                gridRow3 = doctor.DoctorAddressTable.Rows[0];
                gridRow3.Edit();

                addAddress = new AddAddressPopup();

                Assert.AreEqual(WAVPAONRange, addAddress.BuildingNo);
                Assert.AreEqual(WAVStreet, addAddress.Street);
                Assert.AreEqual(WAVTown, addAddress.Town);
                Assert.AreEqual(WAVPostCode, addAddress.PostCode);
            }
        }

        [WebDriverTest(TimeoutSeconds = 1200, Enabled = true, Browsers = new[] { BrowserDefaults.Chrome },
            Groups = new[]
            {
                PupilTestGroups.PupilRecord.Page,
                PupilTestGroups.Priority.Priority1,
                "DoctorAddress",
                "Edit_Medical_Practice_Delete_Doctor_Address"
            })]
        [Variant(Variant.EnglishStatePrimary )]
        public void Edit_Medical_Practice_Delete_Doctor_Address()
        {
            #region Arrange

            Guid pupilId, medicalPracticeId, doctorId, pupilMedicalPracticeId, addressId, doctorAddressId;

            string medicalPracticeName;

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

            #endregion

            using (new DataSetup(CreatePupilData(out pupilId),
                CreateMedicalPracticeData(out medicalPracticeId, out doctorId, out medicalPracticeName),
                CreatePupilMedicalPracticeData(out pupilMedicalPracticeId, medicalPracticeId, pupilId),
                GetAddress(out addressId, UPRN, PAONDescription, PAONRange, SAON, Street, Locality, Town, AdministrativeArea, PostCode, Country),
                CreateDoctorAddresseData(out doctorAddressId, DateTime.Today, "H", doctorId, addressId)))
            {
                LoginAndNavigate(SeleniumHelper.iSIMSUserType.SchoolAdministrator, "Pupil Records", "Addresses");

                PupilRecordPage pupilRecordPage = LoadPupil(pupilId);
                pupilRecordPage.SelectMedicalTab();
                var gridRow = pupilRecordPage.MedicalPractice.Rows[0];
                gridRow.ClickEdit();

                EditMedicalPracticeDialog medicalPractice = new EditMedicalPracticeDialog();

                var gridRow2 = medicalPractice.Doctors.Rows[0];
                gridRow2.ClickEdit();

                DoctorsDialog doctor = new DoctorsDialog();

                SeleniumHelper.FindAndClick(findAddressSection);

                var gridRow3 = doctor.DoctorAddressTable.Rows[0];
                gridRow3.DeleteRow();

                doctor.ClickOk();
                medicalPractice.ClickOk();
                pupilRecordPage.ClickSave();

                pupilRecordPage = LoadPupil(pupilId);
                pupilRecordPage.SelectMedicalTab();
                gridRow = pupilRecordPage.MedicalPractice.Rows[0];
                gridRow.ClickEdit();

                gridRow2 = medicalPractice.Doctors.Rows[0];
                gridRow2.ClickEdit();

                doctor = new DoctorsDialog();

                SeleniumHelper.FindAndClick(findAddressSection);

                int count = doctor.DoctorAddressTable.Rows.Count;

                Assert.AreEqual(0, count);
            }
        }

        [WebDriverTest(TimeoutSeconds = 1200, Enabled = true, Browsers = new[] { BrowserDefaults.Chrome },
            Groups = new[]
            {
                PupilTestGroups.PupilRecord.Page,
                PupilTestGroups.Priority.Priority1,
                "DoctorAddress",
                "Edit_Medical_Practice_Move_Doctor_Address"
            })]
        [Variant(Variant.EnglishStatePrimary )]
        public void Edit_Medical_Practice_Move_Doctor_Address()
        {
            #region Arrange

            Guid pupilId, medicalPracticeId, doctorId, pupilMedicalPracticeId, addressId, doctorAddressId;

            string medicalPracticeName;

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

            #endregion

            using (new DataSetup(CreatePupilData(out pupilId),
                CreateMedicalPracticeData(out medicalPracticeId, out doctorId, out medicalPracticeName),
                CreatePupilMedicalPracticeData(out pupilMedicalPracticeId, medicalPracticeId, pupilId),
                GetAddress(out addressId, UPRN, PAONDescription, PAONRange, SAON, Street, Locality, Town, AdministrativeArea, PostCode, Country),
                CreateDoctorAddresseData(out doctorAddressId, DateTime.Today, "H", doctorId, addressId)))
            {
                LoginAndNavigate(SeleniumHelper.iSIMSUserType.SchoolAdministrator, "Pupil Records", "Addresses");

                PupilRecordPage pupilRecordPage = LoadPupil(pupilId);
                pupilRecordPage.SelectMedicalTab();
                var gridRow = pupilRecordPage.MedicalPractice.Rows[0];
                gridRow.ClickEdit();

                EditMedicalPracticeDialog medicalPractice = new EditMedicalPracticeDialog();

                var gridRow2 = medicalPractice.Doctors.Rows[0];
                gridRow2.ClickEdit();

                DoctorsDialog doctor = new DoctorsDialog();

                SeleniumHelper.FindAndClick(findAddressSection);

                var gridRow3 = doctor.DoctorAddressTable.Rows[0];
                gridRow3.Move();

                AddAddressPopup addAddress = new AddAddressPopup();
                addAddress.MoveDate = moveDate.ToShortDateString();
                addAddress.PAONRangeSearch = WAVPAONRange;
                addAddress.PostCodeSearch = WAVPostCode;
                addAddress.ClickSearch();

                addAddress.ClickOk();
                doctor.ClickOk();
                medicalPractice.ClickOk();
                pupilRecordPage.ClickSave();

                pupilRecordPage = LoadPupil(pupilId);
                pupilRecordPage.SelectMedicalTab();
                gridRow = pupilRecordPage.MedicalPractice.Rows[0];
                gridRow.ClickEdit();

                gridRow2 = medicalPractice.Doctors.Rows[0];
                gridRow2.ClickEdit();

                doctor = new DoctorsDialog();

                SeleniumHelper.FindAndClick(findAddressSection);

                gridRow3 = doctor.DoctorAddressTable.Rows[0];
                var gridRow4 = doctor.DoctorAddressTable.Rows[1];

                Assert.AreEqual(moveDate.AddDays(-1).ToShortDateString(), gridRow3.EndDate);
                Assert.AreEqual(moveDate.ToShortDateString(), gridRow4.StartDate);
            }
        }

        #endregion

        #region Add Medical Practice Popup

        [WebDriverTest(TimeoutSeconds = 1200, Enabled = true, Browsers = new[] { BrowserDefaults.Chrome },
            Groups = new[]
            {
                PupilTestGroups.PupilRecord.Page,
                PupilTestGroups.Priority.Priority1,
                "DoctorAddress",
                "Add_Medical_Practice_Add_Doctor_Address_WAV"
            })]
        [Variant(Variant.EnglishStatePrimary )]
        public void Add_Medical_Practice_Add_Doctor_Address_WAV()
        {
            #region Arrange

            Guid pupilId, medicalPracticeId, doctorId;

            string medicalPracticeName;

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

            #endregion

            using (new DataSetup(CreatePupilData(out pupilId),
                CreateMedicalPracticeData(out medicalPracticeId, out doctorId, out medicalPracticeName)))
            {
                LoginAndNavigate(SeleniumHelper.iSIMSUserType.SchoolAdministrator, "Pupil Records", "Addresses");

                //Get Pupil record
                PupilRecordPage pupilRecordPage = LoadPupil(pupilId);
                pupilRecordPage.SelectMedicalTab();
                pupilRecordPage.ClickMedicalPractice();

                MedicalPracticeTripletDialog medicalPracticeTriplet = new MedicalPracticeTripletDialog();
                medicalPracticeTriplet.SearchCriteria.MedicalPracticeName = medicalPracticeName;
                SearchResultsComponent<MedicalPracticeTripletDialog.MedicalPracticeSearchResultTile> medicalPracticeSearchResultTiles = medicalPracticeTriplet.SearchCriteria.Search();
                MedicalPracticeTripletDialog.MedicalPracticeSearchResultTile medicalPracticeSearchResultTile = medicalPracticeSearchResultTiles.Single();
                medicalPracticeSearchResultTile.Click();

                MedicalPracticeDialog medicalPractice = new MedicalPracticeDialog();

                var gridRow = medicalPractice.Doctors.Rows[0];
                gridRow.ClickEdit();

                DoctorsDialog doctor = new DoctorsDialog();

                SeleniumHelper.FindAndClick(findAddressSection);
                SeleniumHelper.FindAndClick(clickAddAddress);

                AddAddressPopup addAddress = new AddAddressPopup();
                addAddress.PAONRangeSearch = PAONRange;
                addAddress.PostCodeSearch = PostCode;
                addAddress.ClickSearch();

                addAddress.Addresses = addressDisplaySmall;

                Assert.AreEqual(PAONRange, addAddress.BuildingNo);
                Assert.AreEqual(Street, addAddress.Street);
                Assert.AreEqual(Town, addAddress.Town);
                Assert.AreEqual(Town, addAddress.County);
                Assert.AreEqual(PostCode, addAddress.PostCode);

                addAddress.ClickOk();
                doctor.OK();
                medicalPracticeTriplet.ClickOk();
                pupilRecordPage.ClickSave();

                pupilRecordPage = LoadPupil(pupilId);
                pupilRecordPage.SelectMedicalTab();

                var gridRow2 = pupilRecordPage.MedicalPractice.Rows[0];
                gridRow2.ClickEdit();

                EditMedicalPracticeDialog editMedicalPractice = new EditMedicalPracticeDialog();
                var gridRow3 = editMedicalPractice.Doctors.Rows[0];
                gridRow3.ClickEdit();

                doctor = new DoctorsDialog();

                SeleniumHelper.FindAndClick(findAddressSection);

                var gridRow4 = doctor.DoctorAddressTable.Rows[0];

                Assert.AreEqual(addressDisplayLarge, gridRow4.Address);
            }
        }

        [WebDriverTest(TimeoutSeconds = 1200, Enabled = true, Browsers = new[] { BrowserDefaults.Chrome },
            Groups = new[]
            {
                PupilTestGroups.PupilRecord.Page,
                PupilTestGroups.Priority.Priority1,
                "DoctorAddress",
                "Add_Medical_Practice_Add_Doctor_Address_Local"
            })]
        [Variant(Variant.NorthernIrelandStatePrimary | Variant.IndependentPrimary )]
        public void Add_Medical_Practice_Add_Doctor_Address_Local()
        {
            #region Arrange

            Guid pupilId, medicalPracticeId, doctorId, addressId;

            string medicalPracticeName;

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

            #endregion

            using (new DataSetup(CreatePupilData(out pupilId),
                CreateMedicalPracticeData(out medicalPracticeId, out doctorId, out medicalPracticeName),
                GetAddress(out addressId, UPRN, PAONDescription, PAONRange, SAON, Street, Locality, Town, AdministrativeArea, PostCode, Country)))
            {
                LoginAndNavigate(SeleniumHelper.iSIMSUserType.SchoolAdministrator, "Pupil Records", "Addresses");

                //Get Pupil record
                PupilRecordPage pupilRecordPage = LoadPupil(pupilId);
                pupilRecordPage.SelectMedicalTab();
                pupilRecordPage.ClickMedicalPractice();

                MedicalPracticeTripletDialog medicalPracticeTriplet = new MedicalPracticeTripletDialog();
                medicalPracticeTriplet.SearchCriteria.MedicalPracticeName = medicalPracticeName;
                SearchResultsComponent<MedicalPracticeTripletDialog.MedicalPracticeSearchResultTile> medicalPracticeSearchResultTiles = medicalPracticeTriplet.SearchCriteria.Search();
                MedicalPracticeTripletDialog.MedicalPracticeSearchResultTile medicalPracticeSearchResultTile = medicalPracticeSearchResultTiles.Single();
                medicalPracticeSearchResultTile.Click();

                MedicalPracticeDialog medicalPractice = new MedicalPracticeDialog();

                var gridRow = medicalPractice.Doctors.Rows[0];
                gridRow.ClickEdit();

                DoctorsDialog doctor = new DoctorsDialog();

                SeleniumHelper.FindAndClick(findAddressSection);
                SeleniumHelper.FindAndClick(clickAddAddress);

                AddAddressPopup addAddress = new AddAddressPopup();
                addAddress.PAONRangeSearch = PAONRange;
                addAddress.PostCodeSearch = PostCode;
                addAddress.ClickSearch();

                addAddress.Addresses = addressDisplaySmall;

                Assert.AreEqual(PAONRange, addAddress.BuildingNo);
                Assert.AreEqual(Street, addAddress.Street);
                Assert.AreEqual(Town, addAddress.Town);
                Assert.AreEqual(AdministrativeArea, addAddress.County);
                Assert.AreEqual(PostCode, addAddress.PostCode);

                addAddress.ClickOk();
                doctor.OK();
                medicalPracticeTriplet.ClickOk();
                pupilRecordPage.ClickSave();

                pupilRecordPage = LoadPupil(pupilId);
                pupilRecordPage.SelectMedicalTab();

                var gridRow2 = pupilRecordPage.MedicalPractice.Rows[0];
                gridRow2.ClickEdit();

                EditMedicalPracticeDialog editMedicalPractice = new EditMedicalPracticeDialog();
                var gridRow3 = editMedicalPractice.Doctors.Rows[0];
                gridRow3.ClickEdit();

                doctor = new DoctorsDialog();

                SeleniumHelper.FindAndClick(findAddressSection);

                var gridRow4 = doctor.DoctorAddressTable.Rows[0];

                Assert.AreEqual(addressDisplayLarge, gridRow4.Address);
            }
        }

        [WebDriverTest(TimeoutSeconds = 1200, Enabled = true, Browsers = new[] { BrowserDefaults.Chrome },
            Groups = new[]
            {
                PupilTestGroups.PupilRecord.Page,
                PupilTestGroups.Priority.Priority1,
                "DoctorAddress",
                "Add_Medical_Practice_Edit_Doctor_Address_Fields"
            })]
        [Variant(Variant.NorthernIrelandStatePrimary )]
        public void Add_Medical_Practice_Edit_Doctor_Address_Fields()
        {
            #region Arrange

            Guid pupilId, medicalPracticeId, doctorId, addressId, doctorAddressId;

            string medicalPracticeName;

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

            #endregion

            using (new DataSetup(CreatePupilData(out pupilId),
                CreateMedicalPracticeData(out medicalPracticeId, out doctorId, out medicalPracticeName),
                GetAddress(out addressId, UPRN, PAONDescription, PAONRange, SAON, Street, Locality, Town, AdministrativeArea, PostCode, Country),
                CreateDoctorAddresseData(out doctorAddressId, DateTime.Today, "H", doctorId, addressId)))
            {
                LoginAndNavigate(SeleniumHelper.iSIMSUserType.SchoolAdministrator, "Pupil Records", "Addresses");

                //Get Pupil record
                PupilRecordPage pupilRecordPage = LoadPupil(pupilId);
                pupilRecordPage.SelectMedicalTab();
                pupilRecordPage.ClickMedicalPractice();

                MedicalPracticeTripletDialog medicalPracticeTriplet = new MedicalPracticeTripletDialog();
                medicalPracticeTriplet.SearchCriteria.MedicalPracticeName = medicalPracticeName;
                SearchResultsComponent<MedicalPracticeTripletDialog.MedicalPracticeSearchResultTile> medicalPracticeSearchResultTiles = medicalPracticeTriplet.SearchCriteria.Search();
                MedicalPracticeTripletDialog.MedicalPracticeSearchResultTile medicalPracticeSearchResultTile = medicalPracticeSearchResultTiles.Single();
                medicalPracticeSearchResultTile.Click();

                MedicalPracticeDialog medicalPractice = new MedicalPracticeDialog();

                var gridRow = medicalPractice.Doctors.Rows[0];
                gridRow.ClickEdit();

                DoctorsDialog doctor = new DoctorsDialog();

                SeleniumHelper.FindAndClick(findAddressSection);

                var gridRow2 = doctor.DoctorAddressTable.Rows[0];
                gridRow2.Edit();

                AddAddressPopup addAddress = new AddAddressPopup();
                addAddress.District = newLocality;

                addAddress.ClickOk();
                doctor.OK();
                medicalPracticeTriplet.ClickOk();
                pupilRecordPage.ClickSave();

                pupilRecordPage = LoadPupil(pupilId);
                pupilRecordPage.SelectMedicalTab();

                var gridRow3 = pupilRecordPage.MedicalPractice.Rows[0];
                gridRow3.ClickEdit();

                EditMedicalPracticeDialog editMedicalPractice = new EditMedicalPracticeDialog();
                var gridRow4 = editMedicalPractice.Doctors.Rows[0];
                gridRow4.ClickEdit();

                doctor = new DoctorsDialog();

                SeleniumHelper.FindAndClick(findAddressSection);

                gridRow2 = doctor.DoctorAddressTable.Rows[0];
                gridRow2.Edit();

                addAddress = new AddAddressPopup();
                Assert.AreEqual(newLocality, addAddress.District);
            }
        }

        [WebDriverTest(TimeoutSeconds = 1200, Enabled = true, Browsers = new[] { BrowserDefaults.Chrome },
            Groups = new[]
            {
                PupilTestGroups.PupilRecord.Page,
                PupilTestGroups.Priority.Priority1,
                "DoctorAddress",
                "Add_Medical_Practice_Delete_Doctor_Address"
            })]
        [Variant(Variant.NorthernIrelandStatePrimary)]
        public void Add_Medical_Practice_Delete_Doctor_Address()
        {
            #region Arrange

            Guid pupilId, medicalPracticeId, doctorId, addressId, doctorAddressId;

            string medicalPracticeName;

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

            #endregion

            using (new DataSetup(CreatePupilData(out pupilId),
                CreateMedicalPracticeData(out medicalPracticeId, out doctorId, out medicalPracticeName),
                GetAddress(out addressId, UPRN, PAONDescription, PAONRange, SAON, Street, Locality, Town, AdministrativeArea, PostCode, Country),
                CreateDoctorAddresseData(out doctorAddressId, DateTime.Today, "H", doctorId, addressId)))
            {
                LoginAndNavigate(SeleniumHelper.iSIMSUserType.SchoolAdministrator, "Pupil Records", "Addresses");

                //Get Pupil record
                PupilRecordPage pupilRecordPage = LoadPupil(pupilId);
                pupilRecordPage.SelectMedicalTab();
                pupilRecordPage.ClickMedicalPractice();

                MedicalPracticeTripletDialog medicalPracticeTriplet = new MedicalPracticeTripletDialog();
                medicalPracticeTriplet.SearchCriteria.MedicalPracticeName = medicalPracticeName;
                SearchResultsComponent<MedicalPracticeTripletDialog.MedicalPracticeSearchResultTile> medicalPracticeSearchResultTiles = medicalPracticeTriplet.SearchCriteria.Search();
                MedicalPracticeTripletDialog.MedicalPracticeSearchResultTile medicalPracticeSearchResultTile = medicalPracticeSearchResultTiles.Single();
                medicalPracticeSearchResultTile.Click();

                MedicalPracticeDialog medicalPractice = new MedicalPracticeDialog();

                var gridRow = medicalPractice.Doctors.Rows[0];
                gridRow.ClickEdit();

                DoctorsDialog doctor = new DoctorsDialog();

                SeleniumHelper.FindAndClick(findAddressSection);

                var gridRow2 = doctor.DoctorAddressTable.Rows[0];
                gridRow2.DeleteRow();

                doctor.OK();
                medicalPracticeTriplet.ClickOk();
                pupilRecordPage.ClickSave();

                pupilRecordPage = LoadPupil(pupilId);
                pupilRecordPage.SelectMedicalTab();

                var gridRow3 = pupilRecordPage.MedicalPractice.Rows[0];
                gridRow3.ClickEdit();

                EditMedicalPracticeDialog editMedicalPractice = new EditMedicalPracticeDialog();
                var gridRow4 = editMedicalPractice.Doctors.Rows[0];
                gridRow4.ClickEdit();

                doctor = new DoctorsDialog();

                SeleniumHelper.FindAndClick(findAddressSection);

                int count = doctor.DoctorAddressTable.Rows.Count;

                Assert.AreEqual(0, count);
            }
        }

        [WebDriverTest(TimeoutSeconds = 1200, Enabled = true, Browsers = new[] { BrowserDefaults.Chrome },
            Groups = new[]
            {
                PupilTestGroups.PupilRecord.Page,
                PupilTestGroups.Priority.Priority1,
                "DoctorAddress",
                "Add_Medical_Practice_Move_Doctor_Address_Fields"
            })]
        [Variant(Variant.EnglishStatePrimary )]
        public void Add_Medical_Practice_Move_Doctor_Address_Fields()
        {
            #region Arrange

            Guid pupilId, medicalPracticeId, doctorId, addressId, doctorAddressId;

            string medicalPracticeName;

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

            #endregion

            using (new DataSetup(CreatePupilData(out pupilId),
                CreateMedicalPracticeData(out medicalPracticeId, out doctorId, out medicalPracticeName),
                GetAddress(out addressId, UPRN, PAONDescription, PAONRange, SAON, Street, Locality, Town, AdministrativeArea, PostCode, Country),
                CreateDoctorAddresseData(out doctorAddressId, DateTime.Today, "H", doctorId, addressId)))
            {
                LoginAndNavigate(SeleniumHelper.iSIMSUserType.SchoolAdministrator, "Pupil Records", "Addresses");

                //Get Pupil record
                PupilRecordPage pupilRecordPage = LoadPupil(pupilId);
                pupilRecordPage.SelectMedicalTab();
                pupilRecordPage.ClickMedicalPractice();

                MedicalPracticeTripletDialog medicalPracticeTriplet = new MedicalPracticeTripletDialog();
                medicalPracticeTriplet.SearchCriteria.MedicalPracticeName = medicalPracticeName;
                SearchResultsComponent<MedicalPracticeTripletDialog.MedicalPracticeSearchResultTile> medicalPracticeSearchResultTiles = medicalPracticeTriplet.SearchCriteria.Search();
                MedicalPracticeTripletDialog.MedicalPracticeSearchResultTile medicalPracticeSearchResultTile = medicalPracticeSearchResultTiles.Single();
                medicalPracticeSearchResultTile.Click();

                MedicalPracticeDialog medicalPractice = new MedicalPracticeDialog();

                var gridRow = medicalPractice.Doctors.Rows[0];
                gridRow.ClickEdit();

                DoctorsDialog doctor = new DoctorsDialog();

                SeleniumHelper.FindAndClick(findAddressSection);

                var gridRow2 = doctor.DoctorAddressTable.Rows[0];
                gridRow2.Move();

                AddAddressPopup addAddress = new AddAddressPopup();
                addAddress.MoveDate = moveDate.ToShortDateString();
                addAddress.PAONRangeSearch = WAVPAONRange;
                addAddress.PostCodeSearch = WAVPostCode;
                addAddress.ClickSearch();

                addAddress.ClickOk();
                doctor.OK();
                medicalPracticeTriplet.ClickOk();
                pupilRecordPage.ClickSave();

                pupilRecordPage = LoadPupil(pupilId);
                pupilRecordPage.SelectMedicalTab();

                var gridRow3 = pupilRecordPage.MedicalPractice.Rows[0];
                gridRow3.ClickEdit();

                EditMedicalPracticeDialog editMedicalPractice = new EditMedicalPracticeDialog();
                var gridRow4 = editMedicalPractice.Doctors.Rows[0];
                gridRow4.ClickEdit();

                doctor = new DoctorsDialog();

                SeleniumHelper.FindAndClick(findAddressSection);

                gridRow2 = doctor.DoctorAddressTable.Rows[0];
                var gridRow5 = doctor.DoctorAddressTable.Rows[1];

                Assert.AreEqual(moveDate.AddDays(-1).ToShortDateString(), gridRow2.EndDate);
                Assert.AreEqual(moveDate.ToShortDateString(), gridRow5.StartDate);
            }
        }

        #endregion

        #region Helpers

        private DataPackage CreatePupilData(out Guid pupilID)
        {
            #region IDs

            Guid learnerEnrolmentID;

            #endregion

            #region Values

            string pupilForename = CoreQueries.GetColumnUniqueString("Learner", "LegalForename", 10, tenantID);
            string pupilSurname = CoreQueries.GetColumnUniqueString("Learner", "LegalSurname", 10, tenantID);

            #endregion

            #region Data 

            DataPackage package = new DataPackage();

            var yearGroup = Queries.GetFirstYearGroup();

            package.AddData("Learner", new
            {
                ID = pupilID = Guid.NewGuid(),
                School = CoreQueries.GetSchoolId(),
                Gender = CoreQueries.GetLookupItem("Gender", description: "Male"),
                LegalForename = pupilForename,
                LegalSurname = pupilSurname,
                DateOfBirth = startDate,
                TenantID = tenantID
            });
            package.AddData("LearnerEnrolment", new
            {
                ID = learnerEnrolmentID = Guid.NewGuid(),
                School = CoreQueries.GetSchoolId(),
                Learner = pupilID,
                DOA = startDate,
                TenantID = tenantID

            });
            package.AddData("LearnerEnrolmentStatus", new
            {
                Id = Guid.NewGuid(),
                LearnerEnrolment = learnerEnrolmentID,
                StartDate = startDate,
                EnrolmentStatus = CoreQueries.GetLookupItem("EnrolmentStatus", code: "C"),
                TenantID = tenantID
            });
            package.AddData("LearnerYearGroupMembership", new
            {
                Id = Guid.NewGuid(),
                Learner = pupilID,
                YearGroup = yearGroup.ID,
                StartDate = startDate,
                TenantID = tenantID
            });
            package.AddData("LearnerNCYearMembership", new
            {
                Id = Guid.NewGuid(),
                Learner = pupilID,
                SchoolNCYear = yearGroup.SchoolNCYear,
                StartDate = startDate,
                TenantID = tenantID
            });

            #endregion

            return package;
        }

        private DataPackage CreatePupilMedicalPracticeData(out Guid pupilMedicalPracticeID, Guid medicalPracticeID, Guid pupilID)
        {
            DataPackage package = new DataPackage();

            package.AddData("LearnerMedicalPractice", new
            {
                ID = pupilMedicalPracticeID = Guid.NewGuid(),
                Learner = pupilID,
                MedicalPractice = medicalPracticeID,
                TenantID = tenantID
            });

            return package;
        }

        private DataPackage CreateMedicalPracticeData(out Guid medicalPracticeID, out Guid doctorID, out string medicalPracticeName)
        {
            #region Values

            medicalPracticeName = CoreQueries.GetColumnUniqueString("MedicalPractice", "Name", 10, tenantID);

            string doctorForename = CoreQueries.GetColumnUniqueString("Doctor", "Forename", 10, tenantID);
            string doctorSurname = CoreQueries.GetColumnUniqueString("Doctor", "Surname", 10, tenantID);

            #endregion

            #region Data 

            DataPackage package = new DataPackage();

            package.AddData("MedicalPractice", new
            {
                ID = medicalPracticeID = Guid.NewGuid(),
                ResourceProvider = CoreQueries.GetSchoolId(),
                Name = medicalPracticeName,
                TenantID = tenantID
            });
            package.AddData("Doctor", new
            {
                ID = doctorID = Guid.NewGuid(),
                ResourceProvider = CoreQueries.GetSchoolId(),
                Forename = doctorForename,
                Surname = doctorSurname,
                MedicalPractice = medicalPracticeID,
                TenantID = tenantID
            });

            #endregion

            return package;
        }

        private DataPackage GetAddress(out Guid addressID,
                    string UPRN,
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

        private DataPackage CreateDoctorAddresseData(out Guid doctorAddressID, DateTime startDate, string addressTypeCode, Guid doctorID, Guid addressID, DateTime? endDate = null)
        {
            DataPackage package = new DataPackage();

            package.AddData("DoctorAddress", new
            {
                ID = doctorAddressID = Guid.NewGuid(),
                StartDate = startDate,
                EndDate = endDate,
                AddressType = CoreQueries.GetLookupItem("AddressType", code: addressTypeCode),
                Doctor = doctorID,
                Address = addressID,
                TenantID = tenantID
            });

            return package;
        }

        private static void LoginAndNavigate(SeleniumHelper.iSIMSUserType userType, string menuRoute, string enabledFeatures = null)
        {
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.TestUser, enabledFeatures: enabledFeatures);
            AutomationSugar.NavigateMenu("Tasks", "Pupils", "Pupil Records");
        }

        private static PupilRecordPage LoadPupil(Guid pupilId)
        {
            return PupilRecordPage.LoadPupilDetail(pupilId);
        }

        #endregion
    }
}
