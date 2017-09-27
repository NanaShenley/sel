using NUnit.Framework;
using POM.Components.Common;
using POM.Components.Pupil;
using POM.Helper;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using Pupil.Components.Common;
using TestSettings;
using WebDriverRunner.internals;

namespace Pupil.Pupil.Tests
{
    public class PupilAddressTests
    {

       
        /// <summary>
        /// TC PU20
        /// Au : An Nguyen
        /// Description: Exercise ability to link to, view and to edit a pupil's legal name changes.
        /// Role: School Administrator
        /// </summary>
        [WebDriverTest(TimeoutSeconds = 2000, Enabled = true, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { PupilTestGroups.PupilRecord.Address, PupilTestGroups.Priority.Priority2 }, DataProvider = "TC_PU021_Data")]
        public void TC_PU021_Exercise_ability_to_link_pupil_address_changes(string foreName, string surName, string gender, string DOB, string dateOfAdmission, string yearGroup, string className,
                    string[] pastAddress1, string pastStart1, string pastEnd1,
                    string[] pastAddress2, string pastStart2, string pastEnd2,
                    string[] currentAddress)
        {
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);

            #region Pre-Condition

            //Delete the pupil if it exist before
            SeleniumHelper.NavigateMenu("Tasks", "Pupils", "Delete Pupil");
            var deletePupilRecords = new DeletePupilRecordTriplet();
            deletePupilRecords.SearchCriteria.PupilName = String.Format("{0}, {1}", surName, foreName);
            deletePupilRecords.SearchCriteria.IsCurrent = true;
            deletePupilRecords.SearchCriteria.IsLeaver = true;
            deletePupilRecords.SearchCriteria.IsFuture = true;
            var deleteResultPupils = deletePupilRecords.SearchCriteria.Search();
            var deletePupilSearchTile = deleteResultPupils.FirstOrDefault(t => t.Name.Equals(String.Format("{0}, {1}", surName, foreName)));
            var deletePupilRecord = deletePupilSearchTile == null ? new DeletePupilRecordPage() : deletePupilSearchTile.Click<DeletePupilRecordPage>();
            deletePupilRecord.Delete();
            SeleniumHelper.CloseTab("Delete Pupil");

            //Navigate to Pupil Record
            SeleniumHelper.NavigateQuickLink("Pupil Records");
            var pupilRecords = new PupilRecordTriplet();

            //Add new pupil
            var addNewPupilDialog = pupilRecords.AddNewPupil();
            addNewPupilDialog.Forename = foreName;
            addNewPupilDialog.SurName = surName;
            addNewPupilDialog.Gender = gender;
            addNewPupilDialog.DateOfBirth = DOB;
            var registrationDetailDialog = addNewPupilDialog.Continue();
            registrationDetailDialog.DateOfAdmission = dateOfAdmission;
            registrationDetailDialog.YearGroup = yearGroup;
            registrationDetailDialog.ClassName = className;
            registrationDetailDialog.CreateRecord();
            var confirmRequiredDialog = new POM.Components.Common.ConfirmRequiredDialog();
            confirmRequiredDialog.ClickOk();

            //Save pupil
            var pupilRecord = PupilRecordPage.Create();
            pupilRecord.SavePupil();
            SeleniumHelper.CloseTab("Pupil Record");

            #endregion

            #region Test steps

            //Search with forename and surname
            SeleniumHelper.NavigateQuickLink("Pupil Records");
            pupilRecords = new PupilRecordTriplet();
            pupilRecords.SearchCriteria.PupilName = String.Format("{0}, {1}", surName, foreName);
            pupilRecords.SearchCriteria.IsCurrent = true;
            var pupilSearchResults = pupilRecords.SearchCriteria.Search();
            var pupilTile = pupilSearchResults.FirstOrDefault(t => t.Name.Equals(String.Format("{0}, {1}", surName, foreName)));
            pupilRecord = pupilTile.Click<PupilRecordPage>();

            //Add past address 1
            pupilRecord.SelectAddressesTab();
            var addAddress = pupilRecord.ClickAddAddress();
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
            var addressRow = pupilRecord.AddressTable.Rows.FirstOrDefault(t => t.Address.Replace("\r\n", " ").Trim()
                                                .Equals(String.Format("{0}, {1} {2} {3} {4} {5} {6} {7} {8}",
                                                        pastAddress1[2], pastAddress1[0], pastAddress1[1], pastAddress1[3],
                                                        pastAddress1[4], pastAddress1[5], pastAddress1[6], pastAddress1[7], pastAddress1[8])));
            addressRow.StartDate = pastStart1;
            addressRow.EndDate = pastEnd2;

            //Add past address 2
            addAddress = pupilRecord.ClickAddAddress();
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
            addressRow = pupilRecord.AddressTable.Rows.FirstOrDefault(t => t.Address.Replace("\r\n", " ").Trim()
                                                .Equals(String.Format("{0}, {1} {2} {3} {4} {5} {6} {7} {8}",
                                                        pastAddress1[2], pastAddress1[0], pastAddress1[1], pastAddress1[3],
                                                        pastAddress1[4], pastAddress1[5], pastAddress1[6], pastAddress1[7], pastAddress1[8])));
            addressRow.EndDate = pastEnd1;
            addressRow = pupilRecord.AddressTable.Rows.FirstOrDefault(t => t.Address.Replace("\r\n", " ").Trim()
                                                .Equals(String.Format("{0}, {1} {2} {3} {4} {5} {6} {7} {8}",
                                                        pastAddress2[2], pastAddress2[0], pastAddress2[1], pastAddress2[3],
                                                        pastAddress2[4], pastAddress2[5], pastAddress2[6], pastAddress2[7], pastAddress2[8])));
            addressRow.StartDate = pastStart2;
            addressRow.EndDate = pastEnd2;

            //Add current address
            addAddress = pupilRecord.ClickAddAddress();
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

            //Save
            pupilRecord.SavePupil();

            //Verify current address
            addressRow = pupilRecord.AddressTable.Rows.FirstOrDefault(t => t.Address.Replace("\r\n", " ").Trim()
                                                .Equals(String.Format("{0}, {1} {2} {3} {4} {5} {6} {7} {8}",
                                                        currentAddress[2], currentAddress[0], currentAddress[1], currentAddress[3],
                                                        currentAddress[4], currentAddress[5], currentAddress[6], currentAddress[7], currentAddress[8])));
            Assert.AreEqual("Current Address", addressRow.AddressStatus, "Current address is incorrect");

            //Navigate to Previous Address
            var previousAddress = SeleniumHelper.NavigateViaAction<PreviousAddressPage>("Previous Address History");
            var previousRows = previousAddress.PreviousAddress.Rows;

            //Verify past address 1
            var previousRow = previousRows.FirstOrDefault(t => t.Address.Replace("\r\n", " ").Trim()
                                                .Equals(String.Format("{0}, {1} {2} {3} {4} {5} {6} {7} {8}",
                                                        pastAddress1[2], pastAddress1[0], pastAddress1[1], pastAddress1[3],
                                                        pastAddress1[4], pastAddress1[5], pastAddress1[6], pastAddress1[7], pastAddress1[8])));
            Assert.AreEqual(pastStart1, previousRow.StartDate, "Start Date of first past address is incorrect");
            Assert.AreEqual(pastEnd1, previousRow.EndDate, "End Date of first past address is incorrect");

            //Verify past address 2
            previousRow = previousRows.FirstOrDefault(t => t.Address.Replace("\r\n", " ").Trim()
                                                .Equals(String.Format("{0}, {1} {2} {3} {4} {5} {6} {7} {8}",
                                                        pastAddress2[2], pastAddress2[0], pastAddress2[1], pastAddress2[3],
                                                        pastAddress2[4], pastAddress2[5], pastAddress2[6], pastAddress2[7], pastAddress2[8])));
            Assert.AreEqual(pastStart2, previousRow.StartDate, "Start Date of second past address is incorrect");
            Assert.AreEqual(pastEnd2, previousRow.EndDate, "End Date of second past address is incorrect");

            #endregion

            #region Post-Condition

            //Delete the pupil
            SeleniumHelper.NavigateMenu("Tasks", "Pupils", "Delete Pupil");
            deletePupilRecords = new DeletePupilRecordTriplet();
            deletePupilRecords.SearchCriteria.PupilName = String.Format("{0}, {1}", surName, foreName);
            deletePupilRecords.SearchCriteria.IsCurrent = true;
            deletePupilRecords.SearchCriteria.IsLeaver = true;
            deletePupilRecords.SearchCriteria.IsFuture = true;
            deleteResultPupils = deletePupilRecords.SearchCriteria.Search();
            deletePupilSearchTile = deleteResultPupils.FirstOrDefault(t => t.Name.Equals(String.Format("{0}, {1}", surName, foreName)));
            deletePupilRecord = deletePupilSearchTile.Click<DeletePupilRecordPage>();
            deletePupilRecord.Delete();

            #endregion
        }


        #region DATA

        public List<object[]> TC_PU021_Data()
        {
            string pattern = "M/d/yyyy";
            string PupilDateOfBirth = DateTime.ParseExact("06/01/2006", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            string PupilDateOfAdmission = DateTime.ParseExact("06/01/2010", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            string foreName = String.Format("{0}_{1}", SeleniumHelper.GenerateRandomString(7), SeleniumHelper.GenerateRandomNumberUsingDateTime());
            string surName = String.Format("{0}_{1}", "Avn", SeleniumHelper.GenerateRandomString(8));
            string pastStart1 = DateTime.ParseExact("01/01/2015", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            string pastEnd1 = DateTime.ParseExact("05/05/2015", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            string pastStart2 = DateTime.ParseExact("06/05/2015", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            string pastEnd2 = DateTime.Now.Subtract(TimeSpan.FromDays(1)).ToString(pattern);
            var res = new List<Object[]>
            {                
                new object[] 
                {
                    foreName, surName, "Male", PupilDateOfBirth, PupilDateOfAdmission, "Year 2", "SEN",
                    new string[]{"123", "House", "Flat", "Street", "District", "City", "County", "EC1A 1BB", "United Kingdom"}, pastStart1, pastEnd1,
                    new string[]{"234", "House Name", "Flat2", "Street2", "District2", "City2", "County2", "EC1A 1BB", "United Kingdom"}, pastStart2, pastEnd2,
                    new string[]{"567", "House Name", "Flat3", "Street3", "District3", "City3", "County2", "EC1A 1BB", "United Kingdom"},
                }
                
            };
            return res;
        }
        
        #endregion


    }
}
