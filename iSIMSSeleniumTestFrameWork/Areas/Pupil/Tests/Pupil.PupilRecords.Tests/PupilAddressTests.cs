using POM.Components.Pupil;
using POM.Helper;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using Pupil.Components.Common;
using Pupil.Data;
using SeSugar.Automation;
using SeSugar.Data;
using TestSettings;
using Selene.Support.Attributes;
using SeSugar;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using POM.Components.Pupil.Dialogs;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using POM.Components.Common;

namespace Pupil.Pupil.Tests
{
	public class PupilAddressTests
	{

        private readonly string coresidentMatchedAutomationID = "ok_button";
        /// <summary>
        /// TC PU20
        /// Au : An Nguyen
        /// Description: Exercise ability to link to, view and to edit a pupil's legal name changes.
        /// Role: School Administrator
        /// </summary>
        [WebDriverTest(TimeoutSeconds = 2000, Enabled = true, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { PupilTestGroups.PupilRecord.Address, PupilTestGroups.PupilRecord.Page, PupilTestGroups.Priority.Priority2, "TESTME" }, DataProvider = "TC_PU021_Data")]
		public void Link_pupil_address_changes(string foreName, string surName, string gender, string DOB, string dateOfAdmission, string yearGroup, string className,
					string[] pastAddress1, string pastStart1, string pastEnd1,
					string[] pastAddress2, string pastStart2, string pastEnd2,
					string[] currentAddress)
		{
			SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);
			AutomationSugar.Log("Logged in to the system as School Administrator");


			#region Data Preparation
			string forenameSetup = String.Format("{0}{1}{2}", foreName, SeleniumHelper.GenerateRandomString(5), SeleniumHelper.GenerateRandomNumberUsingDateTime());
			string surnameSetup = String.Format("{0}{1}{2}", surName, SeleniumHelper.GenerateRandomString(5), SeleniumHelper.GenerateRandomNumberUsingDateTime());

			DateTime dobSetup = Convert.ToDateTime(DOB);
			DateTime dateOfAdmissionSetup = Convert.ToDateTime(dateOfAdmission);

			var learnerIdSetup = Guid.NewGuid();
			var BuildPupilRecord = this.BuildDataPackage();
			#endregion

			BuildPupilRecord.AddBasicLearner(learnerIdSetup, surnameSetup, forenameSetup, dobSetup, dateOfAdmissionSetup, genderCode: "1", enrolStatus: "C");

			using (new DataSetup(purgeBeforeInsert: false, purgeAfterTest: true, packages: BuildPupilRecord))
			{
				try
				{
					#region Test steps

					//Search with forename and surname
					AutomationSugar.NavigateMenu("Tasks", "Pupils", "Pupil Records");
					AutomationSugar.WaitForAjaxCompletion();

					var pupilRecords = new PupilSearchTriplet();
					pupilRecords.SearchCriteria.PupilName = String.Format("{0}, {1}", surnameSetup, forenameSetup);
					pupilRecords.SearchCriteria.IsCurrent = true;

					Thread.Sleep(1);
					var pupilSearchResults = pupilRecords.SearchCriteria.Search();
					AutomationSugar.WaitForAjaxCompletion();

					// This sometimes takes an eternity
					PupilSearchTriplet.PupilSearchResultTile pupilTile = null;
					for (var cnt = 0; cnt < 10; cnt++)
					{
						Thread.Sleep(1000);
						pupilTile =
							pupilSearchResults.FirstOrDefault(t => t.Name.Equals(String.Format("{0}, {1}", surnameSetup, forenameSetup)));
						if (pupilTile != null) break;

					}
					var pupilRecord = pupilTile.Click<PupilRecordPage>();
					AutomationSugar.WaitForAjaxCompletion();

					// ----------------------------------------------------------
					// Add past address 1
					// ----------------------------------------------------------
					pupilRecord.SelectAddressesTab();
					pupilRecord.ClickAddAddress();

					AddAddressDialog addAddress = new AddAddressDialog();
					addAddress.BuildingNo = pastAddress1[0];
					addAddress.BuildingName = pastAddress1[1];
					addAddress.Flat = pastAddress1[2];
					addAddress.Street = pastAddress1[3];
					addAddress.District = pastAddress1[4];
					addAddress.City = pastAddress1[5];
					addAddress.County = pastAddress1[6];
					addAddress.PostCode = pastAddress1[7];
					addAddress.CountryPostCode = pastAddress1[8];
					addAddress.ClickOk(5);
					AutomationSugar.WaitForAjaxCompletion();

					// As there can only be one address at this stage just pick first address
					var addressRow = pupilRecord.AddressTable.Rows.FirstOrDefault();

					// Update start and end dates for this address to the past							
					addressRow.StartDate = pastStart1;
					addressRow.EndDate = pastEnd1;

					// ----------------------------------------------------------
					// Add past address 2
					// ----------------------------------------------------------
					pupilRecord.ClickAddAddress();
					AutomationSugar.WaitForAjaxCompletion();

					addAddress = new AddAddressDialog();
					addAddress.BuildingNo = pastAddress2[0];
					addAddress.BuildingName = pastAddress2[1];
					addAddress.Flat = pastAddress2[2];
					addAddress.Street = pastAddress2[3];
					addAddress.District = pastAddress2[4];
					addAddress.City = pastAddress2[5];
					addAddress.County = pastAddress2[6];
					addAddress.PostCode = pastAddress2[7];
					addAddress.CountryPostCode = pastAddress2[8];
					addAddress.ClickOk(5);
					AutomationSugar.WaitForAjaxCompletion();

					foreach (var addr in pupilRecord.AddressTable.Rows)
					{
						var addressString = addr.Address.Replace("\r\n", " ");
						var addressToMatch = String.Format("{0}, {1} {2} {3} {4} {5} {6} {7} {8}",
							pastAddress2[2], pastAddress2[1], pastAddress2[0], pastAddress2[3],
							pastAddress2[4], pastAddress2[5], pastAddress2[6], pastAddress2[7], pastAddress2[8]);

						if (addressString != addressToMatch) continue;
						addr.StartDate = pastStart2;
						addr.EndDate = pastEnd2;
						break;
					}

					// ----------------------------------------------------------
					// Add current address - will have today's date as start date
					// ----------------------------------------------------------
					pupilRecord.ClickAddAddress();
					addAddress = new AddAddressDialog();
					addAddress.BuildingNo = currentAddress[0];
					addAddress.BuildingName = currentAddress[1];
					addAddress.Flat = currentAddress[2];
					addAddress.Street = currentAddress[3];
					addAddress.District = currentAddress[4];
					addAddress.City = currentAddress[5];
					addAddress.County = currentAddress[6];
					addAddress.PostCode = currentAddress[7];
					addAddress.CountryPostCode = currentAddress[8];
					addAddress.ClickOk(5);
					AutomationSugar.WaitForAjaxCompletion();

					// ----------------------------------------------------------
					// Save
					// ----------------------------------------------------------
					pupilRecord.SavePupil();
					AutomationSugar.WaitForAjaxCompletion();

					// ----------------------------------------------------------
					// Verify current address
					// ----------------------------------------------------------
					addressRow = pupilRecord.AddressTable.Rows.FirstOrDefault(t => t.Address.Replace("\r\n", " ").Trim()
						.Equals(String.Format("{0}, {1} {2} {3} {4} {5} {6} {7} {8}",
							currentAddress[2], currentAddress[1], currentAddress[0], currentAddress[3],
							currentAddress[4], currentAddress[5], currentAddress[6], currentAddress[7], currentAddress[8])));
					NUnit.Framework.Assert.AreEqual("Current Address", addressRow.AddressStatus, "Current address is incorrect");

					//Navigate to Previous Address function
					var previousAddress = SeleniumHelper.NavigateViaAction<PreviousAddressPage>("Address History");
					var previousRows = previousAddress.PreviousAddress.Rows;

					//Verify past address 1
					var previousRow = previousRows.FirstOrDefault(t => t.Address.Replace("\r\n", " ").Trim()
						.Equals(String.Format("{0}, {1} {2} {3} {4} {5} {6} {7} {8}",
							pastAddress1[2], pastAddress1[0], pastAddress1[1], pastAddress1[3],
							pastAddress1[4], pastAddress1[5], pastAddress1[6], pastAddress1[7], pastAddress1[8])));
					NUnit.Framework.Assert.AreEqual(pastStart1, previousRow.StartDate, "Start Date of first past address is incorrect");
					NUnit.Framework.Assert.AreEqual(pastEnd1, previousRow.EndDate, "End Date of first past address is incorrect");

					//Verify past address 2
					previousRow = previousRows.FirstOrDefault(t => t.Address.Replace("\r\n", " ").Trim()
						.Equals(String.Format("{0}, {1} {2} {3} {4} {5} {6} {7} {8}",
							pastAddress2[2], pastAddress2[0], pastAddress2[1], pastAddress2[3],
							pastAddress2[4], pastAddress2[5], pastAddress2[6], pastAddress2[7], pastAddress2[8])));
					NUnit.Framework.Assert.AreEqual(pastStart2, previousRow.StartDate, "Start Date of second past address is incorrect");
					NUnit.Framework.Assert.AreEqual(pastEnd2, previousRow.EndDate, "End Date of second past address is incorrect");

					#endregion
				}
				finally
				{
					PurgeLinkedData.DeletePupilAddress(learnerIdSetup);
				}
			}
		}


		#region Add Address

		//[TestMethod]
		//[ChromeUiTest("PupilAddress", "P1", "Add", "Add_Pupil_Address_Local")]
		//[Variant(Variant.NorthernIrelandStatePrimary | Variant.NorthernIrelandStateSecondary | Variant.NorthernIrelandStateMultiphase |
		//    Variant.IndependentPrimary | Variant.IndependentSecondary | Variant.IndependentMultiphase)]
		//public void Add_Pupil_Address_Local()
		//{
		//    #region Arrange

		//    Guid pupilId = Guid.NewGuid();
		//    Guid addressID;

		//    string forename = Utilities.GenerateRandomString(6, "Selenium");
		//    string surname = Utilities.GenerateRandomString(6, "Selenium");

		//    string UPRN = Utilities.GenerateRandomString(6);
		//    string PAONDescription = Utilities.GenerateRandomString(6);
		//    string PAONRange = Utilities.GenerateRandomString(6);
		//    string SAON = Utilities.GenerateRandomString(6);
		//    string Street = Utilities.GenerateRandomString(6);
		//    string Locality = Utilities.GenerateRandomString(6);
		//    string Town = Utilities.GenerateRandomString(6);
		//    string AdministrativeArea = Utilities.GenerateRandomString(6);
		//    string PostCode = Utilities.GenerateRandomString(6);
		//    string Country = "United Kingdom";

		//    const string _space = " ";
		//    const string _seperator = ", ";
		//    const string _lineSeperator = "\r\n";

		//    var addressDisplaySmall = string.Concat(SAON, _seperator,
		//        PAONDescription, _seperator,
		//        PAONRange, _seperator, Street, _seperator, Town);

		//    var addressDisplayLarge = string.Concat(SAON, _seperator,
		//        PAONDescription, _lineSeperator,
		//        PAONRange, _space, Street, _lineSeperator,
		//        Locality, _lineSeperator,
		//        Town, _lineSeperator,
		//        AdministrativeArea, _lineSeperator,
		//        PostCode, _lineSeperator,
		//        Country);

		//    #endregion

		//    using (new DataSetup(GetPupilRecord_current(out pupilId),
		//        GetAddress(out addressID, UPRN, PAONDescription, PAONRange, SAON, Street, Locality, Town, AdministrativeArea, PostCode, Country)))
		//    {
		//        LoginAndNavigate(SeleniumHelper.iSIMSUserType.SchoolAdministrator, "Addresses");

		//        //Get Pupil record
		//        var PupilRecordPage = LoadPupil(pupilId);
		//        PupilRecordPage.SelectAddressesTab();
		//        PupilRecordPage.ClickAddAddress();

		//        AddAddressPopup addAddress = new AddAddressPopup();
		//        addAddress.PAONRangeSearch = PAONRange;
		//        addAddress.PostCodeSearch = PostCode;
		//        addAddress.ClickSearch();

		//        addAddress.Addresses = addressDisplaySmall;

		//        Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreEqual(PAONDescription, addAddress.BuildingName);
		//        Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreEqual(PAONRange, addAddress.BuildingNo);
		//        Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreEqual(SAON, addAddress.Flat);
		//        Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreEqual(Street, addAddress.Street);
		//        Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreEqual(Locality, addAddress.District);
		//        Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreEqual(Town, addAddress.Town);
		//        Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreEqual(AdministrativeArea, addAddress.County);
		//        Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreEqual(PostCode, addAddress.PostCode);
		//        Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreEqual(Country, addAddress.Country);

		//        addAddress.ClickOk();
		//        PupilRecordPage.ClickSave();

		//        PupilRecordPage = new PupilRecordPage();
		//        PupilRecordPage.SelectAddressesTab();

		//        var gridRow = PupilRecordPage.AddressTable.Rows[0];
		//        Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreEqual(gridRow.Address, addressDisplayLarge);
		//    }
		//}

		[TestMethod]
		[ChromeUiTest("PupilAddress", "P1", "Add", "Add_Pupil_Address_WAV")]
		[Variant(Variant.EnglishStatePrimary)]
		public void Add_Pupil_Address_WAV()
		{
			#region Arrange

			Guid pupilId = Guid.NewGuid();
			string forename = Utilities.GenerateRandomString(6, "Selenium");
			string surname = Utilities.GenerateRandomString(6, "Selenium");

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

			using (new DataSetup(GetPupilRecord_current(out pupilId)))
			{
				LoginAndNavigate(SeleniumHelper.iSIMSUserType.SchoolAdministrator, "Addresses");

				//Get Pupil record
				var PupilRecordPage = LoadPupil(pupilId);
				PupilRecordPage.SelectAddressesTab();
				PupilRecordPage.ClickAddAddress();

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
				PupilRecordPage.ClickSave();

				PupilRecordPage = new PupilRecordPage();
				PupilRecordPage.SelectAddressesTab();

				var gridRow = PupilRecordPage.AddressTable.Rows[0];
				Assert.AreEqual(addressDisplayLarge, gridRow.Address);
			}
		}

		#endregion

		#region Edit Address

		[TestMethod]
		[ChromeUiTest("PupilAddress", "P1", "Edit", "Edit_Pupil_Address_Fields")]
		[Variant(Variant.EnglishStatePrimary)]
		public void Edit_Pupil_Address_Fields()
		{
			#region Arrange

			Guid pupilId = Guid.NewGuid();
			Guid addressID, PupilAddressID;

			string forename = Utilities.GenerateRandomString(6, "Selenium");
			string surname = Utilities.GenerateRandomString(6, "Selenium");

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

			using (new DataSetup(
				GetPupilRecord_current(out pupilId),
				GetAddress(out addressID, UPRN, PAONDescription, PAONRange, SAON, Street, Locality, Town, AdministrativeArea, PostCode, Country),
				GetPupilAddress(out PupilAddressID, DateTime.Today, "H", addressID, pupilId)))
			{
				LoginAndNavigate(SeleniumHelper.iSIMSUserType.SchoolAdministrator, "Addresses");

				//Get Pupil record
				var PupilRecordPage = LoadPupil(pupilId);
				PupilRecordPage.SelectAddressesTab();
				var gridRow = PupilRecordPage.AddressTable.Rows[0];
				gridRow.ClickEditAddress();

				AddAddressPopup addAddress = new AddAddressPopup();
				addAddress.District = newLocality;
				addAddress.ClickOk();
				PupilRecordPage.ClickSave();

				PupilRecordPage = new PupilRecordPage();
				PupilRecordPage.SelectAddressesTab();
				gridRow = PupilRecordPage.AddressTable.Rows[0];
				gridRow.ClickEditAddress();

				addAddress = new AddAddressPopup();
				Assert.AreEqual(newLocality, addAddress.District);
			}
		}

		[TestMethod]
		[ChromeUiTest("PupilAddress", "P1", "Edit", "Edit_Pupil_Address_New_Address")]
		[Variant(Variant.EnglishStatePrimary)]
		public void Edit_Pupil_Address_New_Address()
		{
			#region Arrange

			Guid pupilId = Guid.NewGuid();
			Guid addressID, PupilAddressID;

			string forename = Utilities.GenerateRandomString(6, "Selenium");
			string surname = Utilities.GenerateRandomString(6, "Selenium");

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

			using (new DataSetup(
				GetPupilRecord_current(out pupilId),
				GetAddress(out addressID, UPRN, PAONDescription, PAONRange, SAON, Street, Locality, Town, AdministrativeArea, PostCode, Country),
				GetPupilAddress(out PupilAddressID, DateTime.Today, "H", addressID, pupilId)))
			{
				LoginAndNavigate(SeleniumHelper.iSIMSUserType.SchoolAdministrator, "Addresses");

				//Get Pupil record
				var PupilRecordPage = LoadPupil(pupilId);
				PupilRecordPage.SelectAddressesTab();
				var gridRow = PupilRecordPage.AddressTable.Rows[0];
				gridRow.ClickEditAddress();

				AddAddressPopup addAddress = new AddAddressPopup();
				addAddress.PAONRangeSearch = WAVPAONRange;
				addAddress.PostCodeSearch = WAVPostCode;
				addAddress.ClickSearch();

				addAddress.ClickOk();
				PupilRecordPage.ClickSave();

				PupilRecordPage = new PupilRecordPage();
				PupilRecordPage.SelectAddressesTab();
				gridRow = PupilRecordPage.AddressTable.Rows[0];
				gridRow.ClickEditAddress();

				addAddress = new AddAddressPopup();

				Assert.AreEqual(WAVPAONRange, addAddress.BuildingNo);
				Assert.AreEqual(WAVStreet, addAddress.Street);
				Assert.AreEqual(WAVTown, addAddress.Town);
				Assert.AreEqual(WAVPostCode, addAddress.PostCode);
			}
		}

		#endregion

		#region Delete Address

		[TestMethod]
		[ChromeUiTest("PupilAddress", "P1", "Delete", "Delete_Pupil_Address")]
		[Variant(Variant.EnglishStatePrimary)]
		public void Delete_Pupil_Address()
		{
			#region Arrange

			Guid pupilId = Guid.NewGuid();
			Guid addressID, PupilAddressID;

			string forename = Utilities.GenerateRandomString(6, "Selenium");
			string surname = Utilities.GenerateRandomString(6, "Selenium");

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

			using (new DataSetup(
				GetPupilRecord_current(out pupilId),
				GetAddress(out addressID, UPRN, PAONDescription, PAONRange, SAON, Street, Locality, Town, AdministrativeArea, PostCode, Country),
				GetPupilAddress(out PupilAddressID, DateTime.Today, "H", addressID, pupilId)))
			{
				LoginAndNavigate(SeleniumHelper.iSIMSUserType.SchoolAdministrator, "Addresses");

				//Get Pupil record
				var PupilRecordPage = LoadPupil(pupilId);
				PupilRecordPage.SelectAddressesTab();
				var gridRow = PupilRecordPage.AddressTable.Rows[0];
				gridRow.DeleteRow();

				PupilRecordPage.ClickSave();

				PupilRecordPage = new PupilRecordPage();
				PupilRecordPage.SelectAddressesTab();

				int count = PupilRecordPage.AddressTable.Rows.Count;

				Assert.AreEqual(0, count);
			}
		}

		#endregion

		#region Move Address

		[TestMethod]
		[ChromeUiTest("PupilAddress", "P1", "Move", "Move_Pupil_Address", "RSB")]
		[Variant(Variant.EnglishStatePrimary)]
		public void Move_Pupil_Address()
		{
			#region Arrange

			Guid pupilId = Guid.NewGuid();
			Guid addressID, PupilAddressID;

			string forename = Utilities.GenerateRandomString(6, "Selenium");
			string surname = Utilities.GenerateRandomString(6, "Selenium");

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

			using (new DataSetup(
				GetPupilRecord_current(out pupilId),
				GetAddress(out addressID, UPRN, PAONDescription, PAONRange, SAON, Street, Locality, Town, AdministrativeArea, PostCode, Country),
				GetPupilAddress(out PupilAddressID, DateTime.Today, "H", addressID, pupilId)))
			{
				LoginAndNavigate(SeleniumHelper.iSIMSUserType.SchoolAdministrator, "Addresses");

				//Get Pupil record
				var pupilRecordPage = LoadPupil(pupilId);
				pupilRecordPage.SelectAddressesTab();
				var gridRow = pupilRecordPage.AddressTable.Rows[0];
				gridRow.ClickMoveAddress();

				AddAddressPopup addAddress = new AddAddressPopup
				{
					MoveDate = moveDate.ToShortDateString(),
					PAONRangeSearch = WAVPAONRange,
					PostCodeSearch = WAVPostCode
				};
				addAddress.ClickSearch();

				addAddress.ClickOk();
				pupilRecordPage.ClickSave();

				pupilRecordPage = new PupilRecordPage();
				pupilRecordPage.SelectAddressesTab();

				gridRow = pupilRecordPage.AddressTable.Rows[0];
				var newGridRow = pupilRecordPage.AddressTable.Rows[1];

				Assert.AreEqual(moveDate.AddDays(-1).ToShortDateString(), gridRow.EndDate);
				Assert.AreEqual(moveDate.ToShortDateString(), newGridRow.StartDate);
			}
		}

        [TestMethod]
        [ChromeUiTest("PupilAddress", "P1", "Move", "Move_Pupil_Address_Coresident", "RSB")]
        [Variant(Variant.EnglishStatePrimary)]
        public void Move_Pupil_Address_Coresident()
        {
            #region Arrange

            Guid pupilId_1,pupilId_2, pupilContactId_One;

            Guid addressID_1, addressID_2, PupilAddressID, addressID_One, pupilContactAddressID_One;

            string forename = Utilities.GenerateRandomString(6, "Selenium");
            string surname = Utilities.GenerateRandomString(6, "Selenium");

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
            string surname_One = Utilities.GenerateRandomString(6, "Selenium_123");

            string WAVPAONRange = "22";
            string WAVPostCode = "MK41 8HS";

            DateTime moveDate = DateTime.Today.AddDays(5);

            #endregion

            using (new DataSetup(
                
                GetPupilRecord_current(out pupilId_1),
                GetPupilRecord_current(out pupilId_2),
                GetAddress(out addressID_1, UPRN, PAONDescription, PAONRange, SAON, Street, Locality, Town, AdministrativeArea, PostCode, Country),
                GetPupilAddress(out PupilAddressID, DateTime.Today, "H", addressID_1, pupilId_1),
                 GetAddress(out addressID_2, UPRN, PAONDescription, PAONRange, SAON, Street, Locality, Town, AdministrativeArea, PostCode, Country),
                GetPupilAddress(out PupilAddressID, DateTime.Today, "H", addressID_2, pupilId_2),
                 CreatePupilContact(out pupilContactId_One, surname_One),
                GetAddress(out addressID_One, UPRN, PAONDescription, PAONRange, SAON, Street, Locality, Town, AdministrativeArea, PostCode, Country),
                GetPupilContactAddress(out pupilContactAddressID_One, DateTime.Today, "H", addressID_One, pupilContactId_One)
                ))
            {
                LoginAndNavigate(SeleniumHelper.iSIMSUserType.SchoolAdministrator, "Addresses");

                //Get Pupil record
                var pupilRecordPage = LoadPupil(pupilId_1);
                pupilRecordPage.SelectAddressesTab();
                var gridRow = pupilRecordPage.AddressTable.Rows[0];
                gridRow.ClickMoveAddress();

                AddAddressPopup addAddress = new AddAddressPopup
                {
                    MoveDate = moveDate.ToShortDateString(),
                    PAONRangeSearch = WAVPAONRange,
                    PostCodeSearch = WAVPostCode
                };
                addAddress.ClickSearch();
                addAddress.Addresses = WAVPAONRange;
                Wait.WaitLoading();
                addAddress.ClickOk();
                // co residents match dialog
                var matchesDialog = new POM.Components.Pupil.Dialogs.SharedAddressDetailsMatchesDialog();
                matchesDialog.Matches.Rows[0].Selected = true;
                matchesDialog.Matches.Rows[1].Selected = true;
                matchesDialog.ClickSave(coresidentMatchedAutomationID);
                //  pupilRecordPage.ClickSave();

                pupilRecordPage = new PupilRecordPage();
                pupilRecordPage.SelectAddressesTab();

                gridRow = pupilRecordPage.AddressTable.Rows[0];
                var newGridRow = pupilRecordPage.AddressTable.Rows[1];

                Assert.AreEqual(moveDate.AddDays(-1).ToShortDateString(), gridRow.EndDate);
                Assert.AreEqual(moveDate.ToShortDateString(), newGridRow.StartDate);
                pupilRecordPage = LoadPupil(pupilId_2);
                pupilRecordPage.SelectAddressesTab();
                gridRow = pupilRecordPage.AddressTable.Rows[0];
                 newGridRow = pupilRecordPage.AddressTable.Rows[1];

                Assert.AreEqual(moveDate.AddDays(-1).ToShortDateString(), gridRow.EndDate);
                Assert.AreEqual(moveDate.ToShortDateString(), newGridRow.StartDate);


                AutomationSugar.NavigateMenu("Tasks", "Pupils", "Pupil Contacts");
                var loadPupilContact= LoadPupilContact(pupilContactId_One);
                loadPupilContact.SelectAddressesTab();

                var gridRowContact = loadPupilContact.AddressTable.Rows[0];
                var newGridRowContact = loadPupilContact.AddressTable.Rows[1];

                Assert.AreEqual(moveDate.AddDays(-1).ToShortDateString(), gridRowContact.EndDate);
                Assert.AreEqual(moveDate.ToShortDateString(), newGridRowContact.StartDate);

            }
        }
        #endregion
        // Pupil Co-Resident Test
        [TestMethod]
        [ChromeUiTest("PupilAddress", "P1", "Edit", "Edit_Pupil_Address_Fields_Co_Resident_Match")]
        [Variant(Variant.NorthernIrelandStatePrimary | Variant.NorthernIrelandStateSecondary | Variant.NorthernIrelandStateMultiphase |
        Variant.IndependentPrimary | Variant.IndependentSecondary | Variant.IndependentMultiphase |
        Variant.EnglishStatePrimary | Variant.EnglishStateSecondary | Variant.EnglishStateMultiphase |
        Variant.WelshStatePrimary | Variant.WelshStateSecondary | Variant.WelshStateMultiphase)]
        public void Edit_Pupil_Address_Fields_Co_Resident_Match()
        {
            #region Arrange

            Guid pupilId = Guid.NewGuid();
            Guid addressID, staffAddressID;

            string forename = Utilities.GenerateRandomString(6, "Selenium");
            string surname = Utilities.GenerateRandomString(6, "Selenium");

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

            using (new DataSetup(
                GetPupilRecord_current(out pupilId),
                GetAddress(out addressID, UPRN, PAONDescription, PAONRange, SAON, Street, Locality, Town, AdministrativeArea, PostCode, Country),
                GetPupilAddress(out staffAddressID, DateTime.Today, "H", addressID, pupilId, DateTime.Today.AddDays(1)),
                GetAddress(out addressID, UPRN, PAONDescription, PAONRange, SAON, Street, Locality, Town, AdministrativeArea, PostCode, Country),
                GetPupilAddress(out staffAddressID, DateTime.Today.AddDays(2), "H", addressID, pupilId, DateTime.Today.AddDays(3))
                ))
            {
                LoginAndNavigate(SeleniumHelper.iSIMSUserType.SchoolAdministrator, "Addresses");

                //Get staff record
                var pupilRecordPage = LoadPupil(pupilId);
                pupilRecordPage.SelectAddressesTab();
                var gridRow = pupilRecordPage.AddressTable.Rows[0];
                gridRow.ClickEditAddress();

                AddAddressPopup addAddress = new AddAddressPopup();
                addAddress.District = newLocality;
                addAddress.ClickOk();
                // co residents match dialog
                var matchesDialog = new POM.Components.Pupil.Dialogs.SharedAddressDetailsMatchesDialog();
                matchesDialog.Matches.Rows[0].Selected = true;
                matchesDialog.ClickSave(coresidentMatchedAutomationID);
                //staffRecordPage.ClickSave();

                pupilRecordPage = new PupilRecordPage();
                pupilRecordPage.SelectAddressesTab();
                gridRow = pupilRecordPage.AddressTable.Rows[0];
                gridRow.ClickEditAddress();

                addAddress = new AddAddressPopup();
                Assert.AreEqual(newLocality, addAddress.District);
            }
        }
        //
        #region DATA

        public List<object[]> TC_PU021_Data()
		{
			string pattern = "d/M/yyyy";
			string PupilDateOfBirth = DateTime.ParseExact("06/01/2006", pattern, CultureInfo.InvariantCulture).ToString(pattern);
			string PupilDateOfAdmission = DateTime.ParseExact("06/01/2010", pattern, CultureInfo.InvariantCulture).ToString(pattern);
			string foreName = String.Format("{0}_{1}", SeleniumHelper.GenerateRandomString(7), SeleniumHelper.GenerateRandomNumberUsingDateTime());
			string surName = String.Format("{0}_{1}", "Avn", SeleniumHelper.GenerateRandomString(8));

			string pastStart1 = DateTime.ParseExact("01/01/2015", pattern, CultureInfo.InvariantCulture).ToString(pattern);
			string pastEnd1 = DateTime.ParseExact("05/05/2015", pattern, CultureInfo.InvariantCulture).ToString(pattern);

			string pastStart2 = DateTime.ParseExact("06/05/2015", pattern, CultureInfo.InvariantCulture).ToString(pattern);
			string pastEnd2 = DateTime.Now.Subtract(TimeSpan.FromDays(1)).ToString(pattern);

			// Flat 3 is the current HOME address, other two are past addresses
			var res = new List<Object[]>
			{
				new object[]
				{
					foreName, surName, "Male", PupilDateOfBirth, PupilDateOfAdmission, "Year 2", "SEN",
					new string[]{"123", "House Name", "Flat1", "Street1", "District1", "City1", "County1", "EC1A 1BB", "United Kingdom"}, pastStart1, pastEnd1,
					new string[]{"234", "House Name", "Flat2", "Street2", "District2", "City2", "County2", "EC1A 1BB", "United Kingdom"}, pastStart2, pastEnd2,
					new string[]{"567", "House Name", "Flat3", "Street3", "District3", "City3", "County2", "EC1A 1BB", "United Kingdom"},
				}

			};
			return res;
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

        private DataPackage GetPupilContactAddress(out Guid pupilAddressID,
                          DateTime startDate,
                          string addressTypeCode,
                          Guid address,
                          Guid pupilContactID,
                          DateTime? endDate = null)
        {
            return this.BuildDataPackage()
            .AddData("LearnerContactAddress", new
            {
                ID = pupilAddressID = Guid.NewGuid(),
                StartDate = startDate,
                EndDate = endDate,
                AddressType = CoreQueries.GetLookupItem("AddressType", code: addressTypeCode),
                Address = address,
                LearnerContact = pupilContactID,
                TenantID = SeSugar.Environment.Settings.TenantId
            });
        }

        private DataPackage GetPupilAddress(out Guid pupilAddressID, DateTime startDate, string addressTypeCode, Guid address, Guid pupilID, DateTime? endDate = null)
		{
			return this.BuildDataPackage()
			.AddData("LearnerAddress", new
			{
				ID = pupilAddressID = Guid.NewGuid(),
				StartDate = startDate,
				EndDate = endDate,
				AddressType = CoreQueries.GetLookupItem("AddressType", code: addressTypeCode),
				Address = address,
				Learner = pupilID,
				TenantID = SeSugar.Environment.Settings.TenantId
			});
		}

        private DataPackage CreatePupilContact(out Guid pupilContactId, string surname)
        {
            return new DataPackage()
                .AddData("LearnerContact", new
                {
                    ID = pupilContactId = Guid.NewGuid(),
                    Surname = surname,
                    School = CoreQueries.GetSchoolId(),
                    TenantID = SeSugar.Environment.Settings.TenantId
                });
        }

        private DataPackage GetPupilRecord_current(out Guid pupilID)
		{
			Guid learnerEnrolmentID;
			int tenantID = SeSugar.Environment.Settings.TenantId;
			DateTime startDate = DateTime.Today.AddDays(-1);
			var yearGroup = Queries.GetFirstYearGroup();

			string pupilForename = CoreQueries.GetColumnUniqueString("Learner", "LegalForename", 10, tenantID);
			string pupilSurname = CoreQueries.GetColumnUniqueString("Learner", "LegalSurname", 10, tenantID);

			return this.BuildDataPackage().AddData("Learner", new
			{
				ID = pupilID = Guid.NewGuid(),
				School = CoreQueries.GetSchoolId(),
				Gender = CoreQueries.GetLookupItem("Gender", description: "Male"),
				LegalForename = pupilForename,
				LegalSurname = pupilSurname,
				DateOfBirth = startDate,
				TenantID = tenantID
			})
			.AddData("LearnerEnrolment", new
			{
				ID = learnerEnrolmentID = Guid.NewGuid(),
				School = CoreQueries.GetSchoolId(),
				Learner = pupilID,
				DOA = startDate,
				TenantID = tenantID

			})
			.AddData("LearnerEnrolmentStatus", new
			{
				Id = Guid.NewGuid(),
				LearnerEnrolment = learnerEnrolmentID,
				StartDate = startDate,
				EnrolmentStatus = CoreQueries.GetLookupItem("EnrolmentStatus", code: "C"),
				TenantID = tenantID
			})
			.AddData("LearnerYearGroupMembership", new
			{
				Id = Guid.NewGuid(),
				Learner = pupilID,
				YearGroup = yearGroup.ID,
				StartDate = startDate,
				TenantID = tenantID
			})
			.AddData("LearnerNCYearMembership", new
			{
				Id = Guid.NewGuid(),
				Learner = pupilID,
				SchoolNCYear = yearGroup.SchoolNCYear,
				StartDate = startDate,
				TenantID = tenantID
			});
		}

		#endregion

		private void LoginAndNavigate(SeleniumHelper.iSIMSUserType userType, string enabledFeatures = null)
		{
			SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator, enabledFeatures: enabledFeatures);
			AutomationSugar.NavigateMenu("Tasks", "Pupils", "Pupil Records");
		}

        private static PupilContactPage LoadPupilContact(Guid pupilContactId)
        {
            return PupilContactPage.LoadPupilContactDetail(pupilContactId);
        }

        private static PupilRecordPage LoadPupil(Guid pupilId)
		{
			return PupilRecordPage.LoadPupilDetail(pupilId);
		}
	}
}
