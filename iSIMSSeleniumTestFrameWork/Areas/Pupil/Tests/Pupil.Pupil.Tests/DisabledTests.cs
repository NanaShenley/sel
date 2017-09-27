using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using OpenQA.Selenium;
using POM.Components.Attendance;
using POM.Components.Common;
using POM.Components.Conduct;
using POM.Components.Pupil;
using POM.Helper;
using Pupil.Components.Common;
using TestSettings;
using WebDriverRunner.internals;

namespace Pupil.Pupil.Tests
{
    public class DisabledTests
    {

        #region PupilAddress

        /// <summary>
        /// TC PU20
        /// Au : An Nguyen
        /// Description: Exercise ability to change a Pupils address
        /// Role: School Administrator
        /// </summary>
        //[WebDriverTest(TimeoutSeconds = 1000, Enabled = true, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { PupilTestGroups.PupilRecord.AddressTests, PupilTestGroups.Priority.Priority3 }, DataProvider = "TC_PU020_Data")]
        public void TC_PU020_Exercise_ability_to_change_a_Pupils_address(string foreName, string surName, string gender, string DOB, string dateOfAdmission, string yearGroup, string className, string[] address, string[] newAddress)
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

            //Add address
            pupilRecord.SelectAddressesTab();
            var addAddress = pupilRecord.ClickAddAddress();
            addAddress.BuildingNo = address[0];
            addAddress.BuildingName = address[1];
            addAddress.Flat = address[2];
            addAddress.Street = address[3];
            addAddress.District = address[4];
            addAddress.City = address[5];
            addAddress.County = address[6];
            addAddress.PostCode = address[7];
            addAddress.CountryPostCode = address[8];
            addAddress.ClickOk(5);

            //Save
            pupilRecord.SavePupil();

            //Verify add address
            pupilRecord.SelectAddressesTab();
            var addressRow = pupilRecord.AddressTable.Rows.FirstOrDefault(t => t.Address.Replace("\r\n", " ").Trim()
                                                .Equals(String.Format("{0}, {1} {2} {3} {4} {5} {6} {7} {8}",
                                                        address[2], address[0], address[1], address[3],
                                                        address[4], address[5], address[6], address[7], address[8])));
            Assert.AreNotEqual(null, addressRow, "Add address unsuccessfull");

            //Edit address
            addressRow.Edit();
            var editAddress = new EditContactAddressDialog();
            editAddress.BuildingNo = newAddress[0];
            editAddress.BuildingName = newAddress[1];
            editAddress.Flat = newAddress[2];
            editAddress.Street = newAddress[3];
            editAddress.District = newAddress[4];
            editAddress.City = newAddress[5];
            editAddress.County = newAddress[6];
            editAddress.PostCode = newAddress[7];
            editAddress.CountryPostCode = newAddress[8];
            editAddress.ClickOk(5);

            //Save
            pupilRecord.SavePupil();

            //Verify edit
            pupilRecord.SelectAddressesTab();
            addressRow = pupilRecord.AddressTable.Rows.FirstOrDefault(t => t.Address.Replace("\r\n", " ").Trim()
                                                .Equals(String.Format("{0}, {1} {2} {3} {4} {5} {6} {7} {8}",
                                                        newAddress[2], newAddress[0], newAddress[1], newAddress[3],
                                                        newAddress[4], newAddress[5], newAddress[6], newAddress[7], newAddress[8])));
            Assert.AreNotEqual(null, addressRow, "Edit address unsuccessfull");

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


        /// <summary>
        /// TC PU22
        /// Au : An Nguyen
        /// Description: Record a change of address and contact details for an entire family (i.e. contacts and pupil/s).
        /// Role: School Administrator
        /// </summary>
        //[WebDriverTest(TimeoutSeconds = 1000, Enabled = true, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { PupilTestGroups.PupilRecord.AddressTests, PupilTestGroups.Priority.Priority3 }, DataProvider = "TC_PU022_Data")]
        public void TC_PU022_Record_a_change_of_address_and_contact_details_for_an_entire_family(string foreName, string surName, string gender, string DOB, string dateOfAdmission, string yearGroup, string className,
                    string title, string contactSurname, string contactForeName, string[] address, string[] newAddress)
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

            //Add Contact
            pupilRecord.SelectFamilyHomeTab();
            var addContactTriplet = pupilRecord.ClickAddContact();
            var addContact = addContactTriplet.CreateContact();
            addContact.Title = title;
            addContact.SurName = contactSurname;
            addContact.ForeName = contactForeName;

            //Add Contact Address
            addContact.ScrollToAddressPanel();
            addContact.HasSameAddressPupil = false;
            var addAddress = addContact.ClickAddAddress();
            addAddress.BuildingNo = address[0];
            addAddress.BuildingName = address[1];
            addAddress.Flat = address[2];
            addAddress.Street = address[3];
            addAddress.District = address[4];
            addAddress.City = address[5];
            addAddress.County = address[6];
            addAddress.PostCode = address[7];
            addAddress.CountryPostCode = address[8];
            addAddress.ClickOk(5);
            addContactTriplet.ClickOk(5);

            //Save pupil
            pupilRecord.SavePupil();
            SeleniumHelper.CloseTab("Pupil Record");

            #endregion

            #region Test steps

            //Search pupil
            SeleniumHelper.NavigateQuickLink("Pupil Records");
            pupilRecords = new PupilRecordTriplet();
            pupilRecords.SearchCriteria.PupilName = String.Format("{0}, {1}", surName, foreName);
            pupilRecords.SearchCriteria.IsCurrent = true;
            var pupilSearchResults = pupilRecords.SearchCriteria.Search();
            var pupilTile = pupilSearchResults.FirstOrDefault(t => t.Name.Equals(String.Format("{0}, {1}", surName, foreName)));
            pupilRecord = pupilTile.Click<PupilRecordPage>();

            //Edit Contact
            pupilRecord.SelectFamilyHomeTab();
            var contactRow = pupilRecord.ContactTable.Rows.FirstOrDefault(t => t.Name.Equals(String.Format("{0} {1} {2}", title, contactForeName, contactSurname)));
            contactRow.Edit();
            var editContact = new EditContactDialog();

            //Edit Address
            editContact.ScrollToAddressPanel();
            var addressRow = editContact.AddressTable.Rows.FirstOrDefault(t => t.Address.Replace("\r\n", " ").Trim()
                                                .Equals(String.Format("{0}, {1} {2} {3} {4} {5} {6} {7} {8}",
                                                        address[2], address[0], address[1], address[3],
                                                        address[4], address[5], address[6], address[7], address[8])));
            addressRow.Edit();
            var editAddress = new EditContactAddressDialog();
            editAddress.BuildingNo = newAddress[0];
            editAddress.BuildingName = newAddress[1];
            editAddress.Flat = newAddress[2];
            editAddress.Street = newAddress[3];
            editAddress.District = newAddress[4];
            editAddress.City = newAddress[5];
            editAddress.County = newAddress[6];
            editAddress.PostCode = newAddress[7];
            editAddress.CountryPostCode = newAddress[8];
            editAddress.ClickOk(5);
            editContact.ClickOk(5);

            //Save pupil
            pupilRecord.SavePupil();

            //Search pupil
            pupilRecords = new PupilRecordTriplet();
            pupilRecords.SearchCriteria.PupilName = String.Format("{0}, {1}", surName, foreName);
            pupilRecords.SearchCriteria.IsCurrent = true;
            pupilSearchResults = pupilRecords.SearchCriteria.Search();
            pupilTile = pupilSearchResults.FirstOrDefault(t => t.Name.Equals(String.Format("{0}, {1}", surName, foreName)));
            pupilRecord = pupilTile.Click<PupilRecordPage>();

            //Verify data
            pupilRecord.SelectFamilyHomeTab();
            contactRow = pupilRecord.ContactTable.Rows.FirstOrDefault(t => t.Name.Equals(String.Format("{0} {1} {2}", title, contactForeName, contactSurname)));
            contactRow.Edit();
            editContact = new EditContactDialog();
            editContact.ScrollToAddressPanel();
            addressRow = editContact.AddressTable.Rows.FirstOrDefault(t => t.Address.Replace("\r\n", " ").Trim()
                                                .Equals(String.Format("{0}, {1} {2} {3} {4} {5} {6} {7} {8}",
                                                        newAddress[2], newAddress[0], newAddress[1], newAddress[3],
                                                        newAddress[4], newAddress[5], newAddress[6], newAddress[7], newAddress[8])));
            Assert.AreNotEqual(null, addressRow, "Address of family contact can not edit");
            editContact.ClickCancel();

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

        
        #endregion

        #region BulkUpdate


        /// <summary>
        /// Author: Ba.Truong
        /// Description: PU74: Check exercise ability to view a selection of pupils with a specific 'Identifier' column and a specific 'Data Item'.
        /// </summary>
        //TODO: Already covered in previous tests
        //[WebDriverTest(TimeoutSeconds = 1000, Enabled = true, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { PupilTestGroups.PupilRecord.Page, PupilTestGroups.Priority.Priority3 }, DataProvider = "TC_PU074_Data")]
        public void TC_PU074_Exercise_Ability_To_View_A_Selection_Of_Pupils_With_Specific_Identifier_Column_And_Data_Item(string yearGroup, string className, string identifierValue, string dataItemValue)
        {
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);
            SeleniumHelper.NavigateMenu("TASKS", "Pupils", "Bulk Update");

            //Select the top 'Year Group' and the top 'Class' and perform 'Search' action
            var bulkUpdateTriplet = BulkUpdateTriplet.SelectBasicDetails();
            bulkUpdateTriplet.SearchCriteria.Selector(group: "Year Group", value: yearGroup);
            bulkUpdateTriplet.SearchCriteria.Selector(group: "Class", value: className);
            var bulkUpdatePage = bulkUpdateTriplet.SearchCriteria.Search<BulkUpdatePage>();

            //Select the Pupil Identifier Column of 'Gender'
            var identifierColumn = bulkUpdatePage.IndentifierColumn();
            identifierColumn.Gender = true;
            bulkUpdatePage = identifierColumn.ClickOK();

            //Select the Pupil Data Item of 'Mode of travel'
            var dataItem = bulkUpdatePage.DataItem();
            dataItem.ModeOfTravel = true;
            bulkUpdatePage = dataItem.ClickOK();

            //VP: Confirm Pupil Identifier Column = 'Gender'
            var bulkUpdateColumns = bulkUpdatePage.BulkUpdateTable.Columns;
            Assert.AreEqual(true, bulkUpdateColumns.Any(x => x.HeaderText.Equals(identifierValue)), String.Format("Column '{0}' doesn't exist", identifierValue));

            //VP: Confirm Pupil Data Item = 'Mode of Travel'
            Assert.AreEqual(true, bulkUpdateColumns.Any(x => x.HeaderText.Equals(dataItemValue)), String.Format("Column '{0}' doesn't exist", dataItemValue));
        }

        /// <summary>
        /// Author: Ba.Truong
        /// Description: PU75: Check exercise ability to modify a selection of pupils with a specific 'Identifier' column and a specific 'Data Item'.
        /// </summary>
        //TODO: Already covered in previous tests
        //[WebDriverTest(TimeoutSeconds = 1000, Enabled = true, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { PupilTestGroups.PupilRecord.Page, PupilTestGroups.Priority.Priority3 }, DataProvider = "TC_PU075_Data")]
        public void TC_PU075_Exercise_Ability_To_Modify_A_Selection_Of_Pupils_With_Specific_Identifier_Column_And_Data_Item(string yearGroup, string className, string[] AllModeOfTravels, string modeOfTravel)
        {
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);
            SeleniumHelper.NavigateMenu("TASKS", "Pupils", "Bulk Update");

            //Select the top 'Year Group' and the top 'Class' and perform 'Search' action
            var bulkUpdateTriplet = BulkUpdateTriplet.SelectBasicDetails();
            bulkUpdateTriplet.SearchCriteria.Selector(group: "Year Group", value: yearGroup);
            bulkUpdateTriplet.SearchCriteria.Selector(group: "Class", value: className);
            var bulkUpdatePage = bulkUpdateTriplet.SearchCriteria.Search<BulkUpdatePage>();

            //Select the Pupil Identifier Column of 'Gender'
            var identifierColumn = bulkUpdatePage.IndentifierColumn();
            identifierColumn.Gender = true;
            bulkUpdatePage = identifierColumn.ClickOK();

            //Select the Pupil Data Item of 'Mode of travel'
            var dataItem = bulkUpdatePage.DataItem();
            dataItem.ModeOfTravel = true;
            bulkUpdatePage = dataItem.ClickOK();

            //Select the 'Mode of Travel'column and click down-arrow
            var bulkUpdateTable = bulkUpdatePage.BulkUpdateTable;
            bulkUpdateTable.Columns[3].Select();
            bulkUpdateTable.Columns[3].ClickDownArrow();

            //Confirm that the selection listed in 'Flood Fill' is appropriate as a 'Mode of Travel' 
            Assert.AreEqual(true, bulkUpdatePage.VerifyModeOfTravel(AllModeOfTravels.ToList()), "List of mode of travel in Flood Fill is not correct");

            //Select entry 'ELB Bus', then set checkbox "Overwrite existing value" as true and apply it
            bulkUpdateTable.FloodFillModeOfTravel = modeOfTravel;
            bulkUpdateTable.OverrideModeOfTravel = true;
            bulkUpdateTable.ApplySelectedModeOfTravel();

            //Get a pupil to use in pupil record page search
            string pupilName = bulkUpdatePage.BulkUpdateTable[0][0].Text;

            //Confirming the selected data value is listed for all pupils
            int numberRecord = bulkUpdatePage.BulkUpdateTable.RowNumber();
            Assert.AreEqual(modeOfTravel, bulkUpdatePage.BulkUpdateTable[0][3].Text, "Flood Fill updated incorrectly");
            Assert.AreEqual(modeOfTravel, bulkUpdatePage.BulkUpdateTable[Convert.ToInt16(numberRecord / 2)][3].Text, "Flood Fill updated incorrectly");
            Assert.AreEqual(modeOfTravel, bulkUpdatePage.BulkUpdateTable[numberRecord - 1][3].Text, "Flood Fill updated incorrectly");

            //Save
            bulkUpdatePage.Save();

            //Confirming the display of a successful save message
            Assert.AreEqual(true, bulkUpdatePage.IsMessageSuccessAppear(), "Success message does not appear");

            //Navigate to Pupil Record
            SeleniumHelper.NavigateMenu("TASKS", "Pupils", "Pupil Records");

            //Search and select a pupil that had their mode of travel changes via a bulk action
            var pupilRecordTriplet = new PupilRecordTriplet();
            pupilRecordTriplet.SearchCriteria.PupilName = pupilName;
            var pupilTile = pupilRecordTriplet.SearchCriteria.Search().FirstOrDefault(x => x.Name.Equals(pupilName));
            var pupilDetails = pupilTile.Click<PupilRecordPage>();

            //Confirming that the pupils 'Mode of Travel' = ELB Bus
            pupilDetails.SelectAdditionalTab();
            Assert.AreEqual(modeOfTravel, pupilDetails.ModeOfTravel, "Mode of Travel in 'Pupil Records' pages is not updated");
        }

        /// <summary>
        /// Author: Ba.Truong
        /// Description: PU76: Check exercise ability to update a specific 'Data Item' which was already updated by a previous bulk update action.
        /// </summary>
        //TODO: Already covered in previous tests
        //[WebDriverTest(TimeoutSeconds = 1000, Enabled = true, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { PupilTestGroups.PupilRecord.Page, PupilTestGroups.Priority.Priority3 }, DataProvider = "TC_PU076_Data")]
        public void TC_PU076_Exercise_Ability_To_Update_A_Specific_Data_Item_Which_Updated(string yearGroup, string className, string[] AllModeOfTravels, string modeOfTravel, string oldModeOfTravel)
        {
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);
            SeleniumHelper.NavigateMenu("TASKS", "Pupils", "Bulk Update");

            //Select the top 'Year Group' and the top 'Class' and perform 'Search' action
            var bulkUpdateTriplet = BulkUpdateTriplet.SelectBasicDetails();
            bulkUpdateTriplet.SearchCriteria.Selector(group: "Year Group", value: yearGroup);
            bulkUpdateTriplet.SearchCriteria.Selector(group: "Class", value: className);
            var bulkUpdatePage = bulkUpdateTriplet.SearchCriteria.Search<BulkUpdatePage>();

            //Select the Pupil Identifier Column of 'Gender'
            var identifierColumn = bulkUpdatePage.IndentifierColumn();
            identifierColumn.Gender = true;
            bulkUpdatePage = identifierColumn.ClickOK();

            //Select the Pupil Data Item of 'Mode of travel'
            var dataItem = bulkUpdatePage.DataItem();
            dataItem.ModeOfTravel = true;
            bulkUpdatePage = dataItem.ClickOK();

            //Select the 'Gender'column
            var bulkUpdateTable = bulkUpdatePage.BulkUpdateTable;

            //In Gender column, Filter "Female"
            bulkUpdateTable.Columns[2].ClickFilter();
            bulkUpdateTable.GenderFilter = "Female";

            //Confirm that only 'Female' pupils remain on display
            int numberRecord = bulkUpdatePage.BulkUpdateTable.RowNumber();
            Assert.AreEqual("Female", bulkUpdatePage.BulkUpdateTable[0][2].Text, "Gender column is not updated after filter");
            Assert.AreEqual("Female", bulkUpdatePage.BulkUpdateTable[Convert.ToInt16(numberRecord / 2)][2].Text, "Gender column is not updated after filter");
            Assert.AreEqual("Female", bulkUpdatePage.BulkUpdateTable[numberRecord - 1][2].Text, "Gender column is not updated after filter");

            //Select the 'Mode of Travel'column and click down-arrow
            bulkUpdateTable.Columns[3].Select();
            bulkUpdateTable.Columns[3].ClickDownArrow();

            //Confirm that the selection listed in 'Flood Fill' is appropriate as a 'Mode of Travel' 
            Assert.AreEqual(true, bulkUpdatePage.VerifyModeOfTravel(AllModeOfTravels.ToList()), "List of mode of travel in Flood Fill is not correct");

            //Select entry 'School Coach', then set checkbox "Overwrite existing value" as true and apply it
            bulkUpdateTable.FloodFillModeOfTravel = modeOfTravel;
            bulkUpdateTable.OverrideModeOfTravel = true;
            bulkUpdateTable.ApplySelectedModeOfTravel();

            //Confirming that the 'Female' pupils now have a mode of travel of 'School Coach'
            Assert.AreEqual(modeOfTravel, bulkUpdatePage.BulkUpdateTable[0][3].Text, "Mode of Travel of Female is not updated");
            Assert.AreEqual(modeOfTravel, bulkUpdatePage.BulkUpdateTable[Convert.ToInt16(numberRecord / 2)][3].Text, "Mode of Travel of Female is not updated");
            Assert.AreEqual(modeOfTravel, bulkUpdatePage.BulkUpdateTable[numberRecord - 1][3].Text, "Mode of Travel of Female is not updated");

            //Save
            bulkUpdatePage.Save();

            //Confirming the display of a successful save message
            Assert.AreEqual(true, bulkUpdatePage.IsMessageSuccessAppear(), "Success message does not appear");

            //Confirm that the male pupils mode of travel remains as 'ELB Bus'
            bulkUpdateTable = bulkUpdatePage.BulkUpdateTable;
            bulkUpdateTable.Columns[2].ClickFilter();
            bulkUpdateTable.GenderFilter = "Male";
            numberRecord = bulkUpdatePage.BulkUpdateTable.RowNumber();

            Assert.AreEqual(oldModeOfTravel, bulkUpdatePage.BulkUpdateTable[0][3].Text, "Mode of Travel of Female is updated incorrectly");
            Assert.AreEqual(oldModeOfTravel, bulkUpdatePage.BulkUpdateTable[Convert.ToInt16(numberRecord / 2)][3].Text, "Mode of Travel of Female is updated incorrectly");
            Assert.AreEqual(oldModeOfTravel, bulkUpdatePage.BulkUpdateTable[numberRecord - 1][3].Text, "Mode of Travel of Female is updated incorrectly");
        }

        /// <summary>
        /// Author: Ba.Truong
        /// Description: PU77: Check exercise ability to update a specific 'Data Item' and 'Service Children' and 'Service Children Source'
        /// </summary>
        //TODO: Already covered in previous tests
        //[WebDriverTest(TimeoutSeconds = 1000, Enabled = true, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { PupilTestGroups.PupilRecord.Page, PupilTestGroups.Priority.Priority3 }, DataProvider = "TC_PU077_Data")]
        public void TC_PU077_Exercise_Ability_To_Update_A_Specific_Data_Item(string yearGroup, string className, string[] dataItems1, string[] dataItems2)
        {
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);
            SeleniumHelper.NavigateMenu("TASKS", "Pupils", "Bulk Update");

            //Select the top 'Year Group' and the top 'Class' and perform 'Search' action
            var bulkUpdateTriplet = BulkUpdateTriplet.SelectBasicDetails();
            bulkUpdateTriplet.SearchCriteria.Selector(group: "Year Group", value: yearGroup);
            bulkUpdateTriplet.SearchCriteria.Selector(group: "Class", value: className);
            var bulkUpdatePage = bulkUpdateTriplet.SearchCriteria.Search<BulkUpdatePage>();

            //Select the Pupil Identifier Column of 'Gender'
            var identifierColumn = bulkUpdatePage.IndentifierColumn();
            identifierColumn.Gender = true;
            bulkUpdatePage = identifierColumn.ClickOK();

            //Select the Pupil Data Item of 'Service Children' and 'Service Children Source'
            var dataItem = bulkUpdatePage.DataItem();
            dataItem.ModeOfTravel = false;
            dataItem.ServiceChildren = true;
            dataItem.ServiceChildrenSource = true;
            bulkUpdatePage = dataItem.ClickOK();

            //Get a pupil to use in pupul record search page at later step
            var bulkUpdateTable = bulkUpdatePage.BulkUpdateTable;
            string pupilName = bulkUpdateTable[0][0].Text;

            //Highlight the 'Service Children' column and apply value 'Yes' to all pupils. 
            bulkUpdateTable.Columns[3].Select();
            bulkUpdateTable.Columns[3].ClickDownArrow();
            bulkUpdateTable.FloodFillServiceChildren = dataItems1[1];
            bulkUpdateTable.ApplySelectedServiceChildren();

            //Highlight the 'Service Children Source' column, and apply value 'Provided by the parent' to all pupils
            bulkUpdateTable.Columns[4].Select();
            bulkUpdateTable.Columns[4].ClickDownArrow();
            bulkUpdateTable.FloodFillServiceSource = dataItems2[1];
            bulkUpdateTable.ApplySelectedServiceSource();

            //Perform a 'Save' action
            bulkUpdatePage.Save();

            //Confirming the display of a successful save message
            bulkUpdatePage.IsMessageSuccessAppear();

            //Cofirming pupils listed now have 'Service Children' = Yes
            int numberRecord = bulkUpdatePage.BulkUpdateTable.RowNumber();
            Assert.AreEqual(dataItems1[1], bulkUpdatePage.BulkUpdateTable[0][3].Text, "Service Children column is not updated after filter");
            Assert.AreEqual(dataItems1[1], bulkUpdatePage.BulkUpdateTable[Convert.ToInt16(numberRecord / 2)][3].Text, "Service Children column is not updated after filter");
            Assert.AreEqual(dataItems1[1], bulkUpdatePage.BulkUpdateTable[numberRecord - 1][3].Text, "Service Children column is not updated after filter");

            //Cofirming pupils listed now have 'Service Children Source' = Provided by the parent
            Assert.AreEqual(dataItems2[1], bulkUpdatePage.BulkUpdateTable[0][4].Text, "Service Children Source column is not updated after filter");
            Assert.AreEqual(dataItems2[1], bulkUpdatePage.BulkUpdateTable[Convert.ToInt16(numberRecord / 2)][4].Text, "Service Children Source column is not updated after filter");
            Assert.AreEqual(dataItems2[1], bulkUpdatePage.BulkUpdateTable[numberRecord - 1][4].Text, "Service Children Source column is not updated after filter");

            //Close Bulk Update page
            SeleniumHelper.CloseTab("Bulk Update");

            //Navigate to Pupil Records and search a pupil was updated value in above steps
            SeleniumHelper.NavigateMenu("TASKS", "Pupils", "Pupil Records");
            var pupilRecordTriplet = new PupilRecordTriplet();
            pupilRecordTriplet.SearchCriteria.PupilName = pupilName;
            var pupilTile = pupilRecordTriplet.SearchCriteria.Search().FirstOrDefault(x => x.Name.Equals(pupilName));
            var pupilRecords = pupilTile.Click<PupilRecordPage>();

            //Navigate to 'Additional' section and check 'Service Children' = Yes and 'Service Children Source' = Provided by the parent
            pupilRecords.SelectAdditionalTab();
            Assert.AreEqual(dataItems1[1], pupilRecords.ServiceChildren, "Value of 'Service Children' in Pupil Record page is not updated");
            Assert.AreEqual(dataItems2[1], pupilRecords.ServiceChildrenSource, "Value of 'Service Children Source' in Pupil Record page is not updated");
        }

        #region DATA

        public List<object[]> TC_PU074_Data()
        {
            var res = new List<Object[]>
            {
                new object[] {
                    "Year 1", "1A", "Gender", "Mode of Travel"
                },
            };
            return res;
        }

        public List<object[]> TC_PU075_Data()
        {
            var res = new List<Object[]>
            {
                new object[] {
                    "Year 1", "1A",
                    //List of 'Mode Of Travel' items
                    new string[]{"Bicycle", "Car", "ELB Bus", "Ferry", "Public Road Transport", "School Coach", "Walks", "Taxi", "Train"}, 
                    //Type mode of travel
                    "ELB Bus"
                },
            };
            return res;
        }

        public List<object[]> TC_PU076_Data()
        {
            var res = new List<Object[]>
            {
                new object[] {
                    "Year 1", "1A",
                    //List of 'Mode Of Travel' items
                    new string[]{"Bicycle", "Car", "ELB Bus", "Ferry", "Public Road Transport", "School Coach", "Walks", "Taxi", "Train"},
                    //Type mode of travel
                    "School Coach",
                    "ELB Bus"
                },
            };
            return res;
        }

        public List<object[]> TC_PU077_Data()
        {
            var res = new List<Object[]>
            {
                new object[] {
                    "Year 1", "1A",
                    new string[]{"Service Children", "Yes"},
                    new string[]{"Service Children Source", "Provided by the parent"}
                },
            };
            return res;
        }

        #endregion


        #endregion

        #region PupilContact


        /// <summary>
        /// Author: Luong.Mai
        /// Description: PU-031a : 
        /// </summary>
        //[WebDriverTest(TimeoutSeconds = 1200, Enabled = true, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { PupilTestGroups.PupilRecord.Contact, PupilTestGroups.Priority.Priority3 }, DataProvider = "TC_PU031a_Data")]
        public void TC_PU031a_Exercise_ability_to_Clone_a_Pupil_Contact(
            string forenameContact, string surnameContract, string forenameContact2, string surnameContract2, string pupilName,
            string title, string gender, string priority, string relationShip, string salutation,
            string addressee)
        {
            // Login as school admin
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);

            #region Pre-Condition: Delete "Stephen, Baker" contact if existed

            // Navigate to Pupil Contracts
            SeleniumHelper.NavigateMenu("Tasks", "Pupils", "Pupil Contacts");

            // Search the contact
            var pupilContactTriplet = new PupilContactTriplet();
            pupilContactTriplet.SearchCriteria.Forename = forenameContact;
            pupilContactTriplet.SearchCriteria.Surname = surnameContract;
            var pupilContactResults = pupilContactTriplet.SearchCriteria.Search();
            var pupilContact = pupilContactResults.SingleOrDefault(x => x.Name.Trim().Equals(surnameContract + ", " + forenameContact));
            var pupilContactPage = pupilContact == null ? new PupilContactPage() : pupilContact.Click<PupilContactPage>();
            pupilContactPage.ClickDelete();
            pupilContactPage.ContinueDelete();

            #endregion

            #region Steps:

            // Navigate to Pupil Contracts
            SeleniumHelper.NavigateMenu("Tasks", "Pupils", "Pupil Records");

            // Search a pupil
            var pupilRecordTriplet = new PupilRecordTriplet();
            pupilRecordTriplet.SearchCriteria.IsCurrent = true;
            pupilRecordTriplet.SearchCriteria.IsFuture = true;
            pupilRecordTriplet.SearchCriteria.IsLeaver = false;
            pupilRecordTriplet.SearchCriteria.PupilName = pupilName;
            var pupilResults = pupilRecordTriplet.SearchCriteria.Search();
            var pupilRecordPage = pupilResults.FirstOrDefault(x => x.Name.Trim().Equals(pupilName)).Click<PupilRecordPage>();

            // Go to Family/Home section
            pupilRecordPage.SelectFamilyHomeTab();

            // Clone a contact
            var cloneContactTripletDialog = pupilRecordPage.CloneContact();
            var contactResults = cloneContactTripletDialog.SearchCriteria.Search();

            // Select a contact
            var cloneContactDialog = contactResults.FirstOrDefault(x => x.Name.Trim().Equals(surnameContract2 + ", " + forenameContact2)).Click<CloneContactDialog>();
            cloneContactDialog.Title = title;
            cloneContactDialog.Forename = forenameContact;
            cloneContactDialog.Gender = gender;
            cloneContactDialog.GenerateParentalSalutation();
            cloneContactDialog.GenerateAddressee();

            // Click OK button
            cloneContactTripletDialog.ClickOk();

            // Edit data of the contact on Contact table
            var contacts = pupilRecordPage.ContactTable;
            contacts.Refresh();
            var contactEdited = contacts.Rows.FirstOrDefault(x => x.Name.Trim().Contains(title + " " + forenameContact));
            contactEdited.Priority = priority;
            contactEdited.RelationShip = relationShip;
            contactEdited.ParentalReponsibility = true;
            pupilRecordPage.GenerateParentalSalutation();
            pupilRecordPage.GenerateAddressee();

            // Save
            pupilRecordPage.SavePupil();

            // For verify result
            // Navigate to Pupil Records
            SeleniumHelper.NavigateQuickLink("Pupil Records");

            // Search a pupil to verify
            pupilRecordTriplet = new PupilRecordTriplet();
            pupilRecordTriplet.SearchCriteria.IsCurrent = true;
            pupilRecordTriplet.SearchCriteria.IsFuture = true;
            pupilRecordTriplet.SearchCriteria.IsLeaver = false;
            pupilRecordTriplet.SearchCriteria.PupilName = pupilName;
            pupilResults = pupilRecordTriplet.SearchCriteria.Search();
            pupilRecordPage = pupilResults.FirstOrDefault(x => x.Name.Trim().Equals(pupilName)).Click<PupilRecordPage>();

            // Go to Family/Home section
            pupilRecordPage.SelectFamilyHomeTab();

            // Verify that the selected pupil now has an additional contact record, that was produced via the 'Clone Contact' process.
            contacts = pupilRecordPage.ContactTable;
            var contactAdded = contacts.Rows.FirstOrDefault(x => x.Name.Trim().Contains(title + " " + forenameContact));
            Assert.AreNotEqual(null, contactAdded, "Clone the contact unsuccessfully.");

            #endregion

            #region Pre-Condition: Delete "Stephen, Baker"

            // Navigate to Pupil Contracts
            SeleniumHelper.NavigateMenu("Tasks", "Pupils", "Pupil Contacts");

            // Search the contact
            pupilContactTriplet = new PupilContactTriplet();
            pupilContactTriplet.SearchCriteria.Forename = forenameContact;
            pupilContactTriplet.SearchCriteria.Surname = surnameContract;
            pupilContactResults = pupilContactTriplet.SearchCriteria.Search();
            pupilContact = pupilContactResults.SingleOrDefault(x => x.Name.Trim().Equals(surnameContract + ", " + forenameContact));
            pupilContactPage = pupilContact == null ? new PupilContactPage() : pupilContact.Click<PupilContactPage>();
            pupilContactPage.ClickDelete();
            pupilContactPage.ContinueDelete();

            #endregion
        }

        /// <summary>
        /// Author: Luong.Mai
        /// Description: PU-031b : 
        /// </summary>
        //[WebDriverTest(TimeoutSeconds = 1200, Enabled = true, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { PupilTestGroups.PupilRecord.Contact, PupilTestGroups.Priority.Priority3 }, DataProvider = "TC_PU031b_Data")]
        public void TC_PU031b_Exercise_ability_to_Copy_a_Pupil_Contact(
            string forenamePP, string surnamePP, string fullnameContact1, string title, string forenameContact, string surnameContact, string gender)
        {

            // Login as school admin
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);

            #region Pre-Condition: Create a new pupil for test

            // Navigate to Pupil Record
            SeleniumHelper.NavigateMenu("Tasks", "Pupils", "Pupil Records");

            var pupilRecordTriplet = new PupilRecordTriplet();
            var addNewPupilDialog = pupilRecordTriplet.AddNewPupil();
            addNewPupilDialog.Forename = forenamePP;
            addNewPupilDialog.SurName = surnamePP;
            addNewPupilDialog.Gender = "Male";
            addNewPupilDialog.DateOfBirth = "2/2/2011";

            var registrationDetailDialog = addNewPupilDialog.Continue();
            registrationDetailDialog.DateOfAdmission = "2/2/2015";
            registrationDetailDialog.YearGroup = "Year 1";
            registrationDetailDialog.CreateRecord();

            var confirmRequiredDialog = new ConfirmRequiredDialog();
            confirmRequiredDialog.ClickOk();

            var pupilRecordPage = PupilRecordPage.Create();
            pupilRecordPage.SavePupil();

            #endregion

            #region Pre-Condition: Create new contact and refer to pupil
            Console.WriteLine("***Pre-Condition: Create new contact and refer to pupil");

            // Navigate to Pupil Contracts
            SeleniumHelper.NavigateMenu("Tasks", "Pupils", "Pupil Contacts");

            // Create new pupil contract
            var pupilContactTriplet = new PupilContactTriplet();
            var pupilContactPage = pupilContactTriplet.ClickCreate();

            // Add data for Personal Details
            pupilContactPage.SelectPersonalDetailsTab();
            pupilContactPage.Title = title;
            pupilContactPage.Forename = forenameContact;
            pupilContactPage.Surname = surnameContact;
            pupilContactPage.Gender = gender;

            // Save
            pupilContactTriplet.ClickSave();

            // Select Associated Pupils tab
            pupilContactPage.SelectAssociatedPupilsTab();
            var addAssociatedPupilsTripletDialog = pupilContactPage.ClickAddPupilLink();
            addAssociatedPupilsTripletDialog.SearchCriteria.PupilName = forenamePP;
            var associatedPupilResults = addAssociatedPupilsTripletDialog.SearchCriteria.Search();
            associatedPupilResults.SingleOrDefault(x => x.Name.Trim().Equals(surnamePP + ", " + forenamePP)).Click(); ;
            addAssociatedPupilsTripletDialog.ClickOk(7);

            // Record the data for the associated pupil
            var pupilTable = pupilContactPage.PupilTable;
            var pupilRow = pupilTable.Rows.SingleOrDefault(x => x.LegalForename.Trim().Equals(forenamePP)
                && x.LegalSurname.Trim().Equals(surnamePP));
            pupilRow.Priority = "1";
            pupilRow.Relationship = "Parent";
            pupilRow.ParentalResponsibility = true;
            pupilRow.ReceivesCorrespondance = true;
            pupilRow.SchoolReport = true;
            pupilRow.CourtOrder = false;

            // Save
            pupilContactTriplet.ClickSave();

            #endregion

            #region Steps:

            // Navigate to Pupil Records
            SeleniumHelper.NavigateMenu("Tasks", "Pupils", "Pupil Records");

            // Search a pupil
            pupilRecordTriplet = new PupilRecordTriplet();
            pupilRecordTriplet.SearchCriteria.IsCurrent = true;
            pupilRecordTriplet.SearchCriteria.IsFuture = true;
            pupilRecordTriplet.SearchCriteria.IsLeaver = false;
            pupilRecordTriplet.SearchCriteria.PupilName = forenamePP + ", " + surnamePP;
            var pupilResults = pupilRecordTriplet.SearchCriteria.Search();
            pupilRecordPage = pupilResults.FirstOrDefault(x => x.Name.Trim().Equals(surnamePP + ", " + forenamePP)).Click<PupilRecordPage>();

            // Go to Family/Home section
            pupilRecordPage.SelectFamilyHomeTab();

            // Search contact
            var copyContactTripletDialog = pupilRecordPage.CopyContact();
            copyContactTripletDialog.SearchCriteria.LegalSurname = surnamePP;
            var contactResults = copyContactTripletDialog.SearchCriteria.Search();
            var copyContactDialog = contactResults.FirstOrDefault(x => x.Name.Trim().Equals(forenamePP + ", " + surnamePP)).Click<CopyContactDialog>();

            // Click OK
            copyContactTripletDialog.ClickOk(7);

            // Delete existed contacts
            var contacts = pupilRecordPage.ContactTable;
            var contactDeleted = contacts.Rows.FirstOrDefault(x => x.Name.Trim().Equals(fullnameContact1));
            contactDeleted.DeleteRow();

            // Save pupil record
            pupilRecordPage.SavePupil();

            // Navigate to Pupil Contracts
            SeleniumHelper.NavigateMenu("Tasks", "Pupils", "Pupil Records");

            // Search a pupil
            pupilRecordTriplet = new PupilRecordTriplet();
            pupilRecordTriplet.SearchCriteria.IsCurrent = true;
            pupilRecordTriplet.SearchCriteria.IsFuture = true;
            pupilRecordTriplet.SearchCriteria.IsLeaver = false;
            pupilRecordTriplet.SearchCriteria.PupilName = forenamePP + ", " + surnamePP;
            pupilResults = pupilRecordTriplet.SearchCriteria.Search();
            pupilRecordPage = pupilResults.FirstOrDefault(x => x.Name.Trim().Equals(surnamePP + ", " + forenamePP)).Click<PupilRecordPage>();

            // Go to Family/Home section
            pupilRecordPage.SelectFamilyHomeTab();

            // Verify that selected pupil has Copied Contact record(s)
            // that are EXACTLY the same as they had, prior to performing the  'Copy Contacts' process.
            contacts = pupilRecordPage.ContactTable;
            Assert.AreEqual(true, contacts.Rows.Any(x => x.Name.Trim().Equals(fullnameContact1)), "Copy Contacts is unsuccessfully");

            #endregion

            #region Post-Condition: Delete the pupil if existed

            // Navigate to Pupil Record
            SeleniumHelper.NavigateMenu("Tasks", "Pupils", "Delete Pupil");
            var deletePupilRecordTriplet = new DeletePupilRecordTriplet();
            deletePupilRecordTriplet.SearchCriteria.PupilName = String.Format("{0}, {1}", surnamePP, forenamePP);
            deletePupilRecordTriplet.SearchCriteria.IsCurrent = true;
            deletePupilRecordTriplet.SearchCriteria.IsFuture = true;
            deletePupilRecordTriplet.SearchCriteria.IsLeaver = true;
            var resultPupils = deletePupilRecordTriplet.SearchCriteria.Search();

            var pupilSearchTile = resultPupils.SingleOrDefault(t => t.Name.Equals(String.Format("{0}, {1}", surnamePP, forenamePP)));
            var deletePupilRecordPage = pupilSearchTile == null ? new DeletePupilRecordPage() : pupilSearchTile.Click<DeletePupilRecordPage>();

            deletePupilRecordPage.Delete();

            #endregion

            #region Post-Condition: Delete the contact if existed
            Console.WriteLine("***Post-Condition: Delete the contact if existed");

            // Navigate to Pupil Contracts
            SeleniumHelper.NavigateMenu("Tasks", "Pupils", "Pupil Contacts");

            // Search the contact
            pupilContactTriplet = new PupilContactTriplet();
            pupilContactTriplet.SearchCriteria.Forename = forenameContact;
            pupilContactTriplet.SearchCriteria.Surname = surnameContact;
            var pupilContactResults = pupilContactTriplet.SearchCriteria.Search();

            var pupilContact = pupilContactResults.SingleOrDefault(x => x.Name.Trim().Equals(surnameContact + ", " + forenameContact));
            pupilContactPage = pupilContact == null ? new PupilContactPage() : pupilContact.Click<PupilContactPage>();
            pupilContactPage.ClickDelete();
            pupilContactPage.ContinueDelete();

            #endregion
        }

        #endregion

        #region PupilLeaver

        /// <summary>
        /// TC PU25
        /// Au : An Nguyen
        /// Description: Exercise ability to make a specific set of pupils 'leavers' (in bulk) by allocating to them the same leaving details. (Past Date)
        /// Role: School Administrator
        /// </summary>
        //TODO: Duplicate to TC_PU026
        //[WebDriverTest(TimeoutSeconds = 2000, Enabled = true, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { PupilTestGroups.PupilRecord.LeaverTests, PupilTestGroups.Priority.Priority4 }, DataProvider = "TC_PU025_Data")]
        public void TC_PU025_Exercise_ability_to_make_a_specific_set_of_pupils_leavers_in_past(string[] firstPupil, string[] secondPupil,
                    string enrolmentStatus, string yearGroup, string className, string dateOfLeaving, string reasonLeaving, string destination)
        {
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);

            #region Pre-Condition

            //Delete first pupil
            SeleniumHelper.NavigateMenu("Tasks", "Pupils", "Delete Pupil");
            var deletePupilRecords = new DeletePupilRecordTriplet();
            deletePupilRecords.SearchCriteria.PupilName = String.Format("{0}, {1}", firstPupil[1], firstPupil[0]);
            deletePupilRecords.SearchCriteria.IsCurrent = true;
            deletePupilRecords.SearchCriteria.IsLeaver = true;
            deletePupilRecords.SearchCriteria.IsFuture = true;
            var deleteResultPupils = deletePupilRecords.SearchCriteria.Search();
            var deletePupilSearchTile = deleteResultPupils.FirstOrDefault(t => t.Name.Equals(String.Format("{0}, {1}", firstPupil[1], firstPupil[0])));
            var deletePupilRecord = deletePupilSearchTile == null ? new DeletePupilRecordPage() : deletePupilSearchTile.Click<DeletePupilRecordPage>();
            deletePupilRecord.Delete();

            //Delete second pupil
            deletePupilRecords.SearchCriteria.PupilName = String.Format("{0}, {1}", secondPupil[1], secondPupil[0]);
            deleteResultPupils = deletePupilRecords.SearchCriteria.Search();
            deletePupilSearchTile = deleteResultPupils.FirstOrDefault(t => t.Name.Equals(String.Format("{0}, {1}", secondPupil[1], secondPupil[0])));
            deletePupilRecord = deletePupilSearchTile == null ? new DeletePupilRecordPage() : deletePupilSearchTile.Click<DeletePupilRecordPage>();
            deletePupilRecord.Delete();
            SeleniumHelper.CloseTab("Delete Pupil");

            //Add first pupil
            SeleniumHelper.NavigateQuickLink("Pupil Records");
            var pupilRecords = new PupilRecordTriplet();
            var addNewPupilDialog = pupilRecords.AddNewPupil();
            addNewPupilDialog.Forename = firstPupil[0];
            addNewPupilDialog.SurName = firstPupil[1];
            addNewPupilDialog.Gender = firstPupil[2];
            addNewPupilDialog.DateOfBirth = firstPupil[3];
            var registrationDetailDialog = addNewPupilDialog.Continue();
            registrationDetailDialog.DateOfAdmission = firstPupil[4];
            registrationDetailDialog.EnrolmentStatus = enrolmentStatus;
            registrationDetailDialog.YearGroup = yearGroup;
            registrationDetailDialog.ClassName = className;
            registrationDetailDialog.CreateRecord();
            var confirmRequiredDialog = new POM.Components.Common.ConfirmRequiredDialog();
            confirmRequiredDialog.ClickOk();
            var pupilRecord = PupilRecordPage.Create();
            pupilRecord.SavePupil();

            //Add second pupil
            pupilRecords = new PupilRecordTriplet();
            addNewPupilDialog = pupilRecords.AddNewPupil();
            addNewPupilDialog.Forename = secondPupil[0];
            addNewPupilDialog.SurName = secondPupil[1];
            addNewPupilDialog.Gender = secondPupil[2];
            addNewPupilDialog.DateOfBirth = secondPupil[3];
            registrationDetailDialog = addNewPupilDialog.Continue();
            registrationDetailDialog.DateOfAdmission = secondPupil[4];
            registrationDetailDialog.EnrolmentStatus = enrolmentStatus;
            registrationDetailDialog.YearGroup = yearGroup;
            registrationDetailDialog.ClassName = className;
            registrationDetailDialog.CreateRecord();
            confirmRequiredDialog = new POM.Components.Common.ConfirmRequiredDialog();
            confirmRequiredDialog.ClickOk();
            pupilRecord = PupilRecordPage.Create();
            pupilRecord.SavePupil();

            #endregion

            #region Test steps

            //Navigate Manage Leaver
            SeleniumHelper.NavigateBySearch("Manage Leavers");

            //Search
            var manageLeaverTriplet = new ManageLeaversTriplet();
            manageLeaverTriplet.SearchCriteria.Class = className;
            manageLeaverTriplet.SearchCriteria.YearGroup = yearGroup;
            var manageLeaver = manageLeaverTriplet.SearchCriteria.Search<ManageLeaversPage>();

            //Enter value for leaving and leaving pupil
            manageLeaver.DateOfLeaving = dateOfLeaving;
            manageLeaver.ReasonForLeaving = reasonLeaving;
            manageLeaver.DestinationDetails = destination;

            //Click check 
            manageLeaver.ManageLeaverTable[String.Format("{0}, {1}", firstPupil[1], firstPupil[0])].ClickCheckBox(0);
            manageLeaver.ManageLeaverTable[String.Format("{0}, {1}", secondPupil[1], secondPupil[0])].ClickCheckBox(0);

            //Save and confirm
            manageLeaver.ClickApplyToSelectedPupil();
            manageLeaver.Save();
            var confirmDialog = new POM.Components.Common.ConfirmRequiredDialog();
            confirmDialog.ClickOk();

            //Search pupil
            manageLeaverTriplet = new ManageLeaversTriplet();
            manageLeaverTriplet.SearchCriteria.Class = className;
            manageLeaverTriplet.SearchCriteria.YearGroup = yearGroup;
            manageLeaverTriplet.SearchCriteria.IsLeaver = true;
            manageLeaver = manageLeaverTriplet.SearchCriteria.Search<ManageLeaversPage>();

            //Verify data
            //First pupil
            manageLeaver.DateOfLeaving = dateOfLeaving;
            var manageLeaverRow = manageLeaver.ManageLeaverTable[String.Format("{0}, {1}", firstPupil[1], firstPupil[0])];
            Assert.AreEqual(manageLeaver.DateOfLeaving, manageLeaverRow[6].Text, "Date of leaving is incorrect");
            Assert.AreEqual(reasonLeaving, manageLeaverRow[7].Text, "Reason of leaving is incorrect");
            Assert.AreEqual(destination, manageLeaverRow[8].Text, "Destination is incorrect");

            //Second pupil
            manageLeaverRow = manageLeaver.ManageLeaverTable[String.Format("{0}, {1}", secondPupil[1], secondPupil[0])];
            Assert.AreEqual(manageLeaver.DateOfLeaving, manageLeaverRow[6].Text, "Date of leaving is incorrect");
            Assert.AreEqual(reasonLeaving, manageLeaverRow[7].Text, "Reason of leaving is incorrect");
            Assert.AreEqual(destination, manageLeaverRow[8].Text, "Destination is incorrect");
            SeleniumHelper.CloseTab("Manage Leavers");

            //Navigate to pupil record
            SeleniumHelper.NavigateQuickLink("Pupil Records");

            //Search and verify data of pupil
            //First pupil
            pupilRecords = new PupilRecordTriplet();
            pupilRecords.SearchCriteria.PupilName = String.Format("{0}, {1}", firstPupil[1], firstPupil[0]);
            pupilRecords.SearchCriteria.IsCurrent = false;
            pupilRecords.SearchCriteria.IsFuture = false;
            pupilRecords.SearchCriteria.IsLeaver = true;
            var pupilSearchResults = pupilRecords.SearchCriteria.Search();
            var pupilTile = pupilSearchResults.FirstOrDefault(t => t.Name.Equals(String.Format("{0}, {1}", firstPupil[1], firstPupil[0])));
            pupilRecord = pupilTile.Click<PupilRecordPage>();
            pupilRecord.SelectRegistrationTab();
            var enrolmentHistoryRow = pupilRecord.EnrolmentStatusHistoryTable.Rows.FirstOrDefault(t => t.EnrolmentStatus.Equals(enrolmentStatus));
            Assert.AreEqual(dateOfLeaving, enrolmentHistoryRow.EndDate, "Destination date on pupil record is incorrect");

            //Second Pupil
            pupilRecords = new PupilRecordTriplet();
            pupilRecords.SearchCriteria.PupilName = String.Format("{0}, {1}", secondPupil[1], secondPupil[0]);
            pupilRecords.SearchCriteria.IsCurrent = false;
            pupilRecords.SearchCriteria.IsFuture = false;
            pupilRecords.SearchCriteria.IsLeaver = true;
            pupilSearchResults = pupilRecords.SearchCriteria.Search();
            pupilTile = pupilSearchResults.FirstOrDefault(t => t.Name.Equals(String.Format("{0}, {1}", secondPupil[1], secondPupil[0])));
            pupilRecord = pupilTile.Click<PupilRecordPage>();
            pupilRecord.SelectRegistrationTab();
            enrolmentHistoryRow = pupilRecord.EnrolmentStatusHistoryTable.Rows.FirstOrDefault(t => t.EnrolmentStatus.Equals(enrolmentStatus));
            Assert.AreEqual(dateOfLeaving, enrolmentHistoryRow.EndDate, "Destination date on pupil record is incorrect");
            SeleniumHelper.CloseTab("Pupil Record");

            #endregion

            #region Post-Condition

            //Delete first pupil
            SeleniumHelper.NavigateMenu("Tasks", "Pupils", "Delete Pupil");
            deletePupilRecords = new DeletePupilRecordTriplet();
            deletePupilRecords.SearchCriteria.PupilName = String.Format("{0}, {1}", firstPupil[1], firstPupil[0]);
            deletePupilRecords.SearchCriteria.IsCurrent = true;
            deletePupilRecords.SearchCriteria.IsLeaver = true;
            deletePupilRecords.SearchCriteria.IsFuture = true;
            deleteResultPupils = deletePupilRecords.SearchCriteria.Search();
            deletePupilSearchTile = deleteResultPupils.FirstOrDefault(t => t.Name.Equals(String.Format("{0}, {1}", firstPupil[1], firstPupil[0])));
            deletePupilRecord = deletePupilSearchTile == null ? new DeletePupilRecordPage() : deletePupilSearchTile.Click<DeletePupilRecordPage>();
            deletePupilRecord.Delete();

            //Delete second pupil
            deletePupilRecords.SearchCriteria.PupilName = String.Format("{0}, {1}", secondPupil[1], secondPupil[0]);
            deleteResultPupils = deletePupilRecords.SearchCriteria.Search();
            deletePupilSearchTile = deleteResultPupils.FirstOrDefault(t => t.Name.Equals(String.Format("{0}, {1}", secondPupil[1], secondPupil[0])));
            deletePupilRecord = deletePupilSearchTile == null ? new DeletePupilRecordPage() : deletePupilSearchTile.Click<DeletePupilRecordPage>();
            deletePupilRecord.Delete();

            #endregion
        }


        /// <summary>
        /// Author: Hieu Pham
        /// Descriptions: Exercise ability to invoke the listed 'Actions' via contextual links within the 'Pupil Record' screen.
        /// </summary>
        //TODO: Duplicate to TC_PU026
        //[WebDriverTest(TimeoutSeconds = 1800, Enabled = true, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { PupilTestGroups.PupilRecord.LeaverTests, PupilTestGroups.Priority.Priority4 }, DataProvider = "TC_PU043_DATA")]
        public void TC_PU043_Adminstrator_Pupil_SEN_Record_Leaver(string pupilSurName, string pupilForeName, string gender, string dateOfBirth,
            string DateOfAdmission, string YearGroup, string className, string pupilName, string dateOfLeaving, string reasonLeaving, string destination)
        {
            #region Pre-condition

            // Login
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);

            // Navigate to Pupil Record
            SeleniumHelper.NavigateMenu("Tasks", "Pupils", "Pupil Records");

            var pupilRecordTriplet = new PupilRecordTriplet();
            var addNewPupilDialog = pupilRecordTriplet.AddNewPupil();

            // Enter values
            addNewPupilDialog.Forename = pupilForeName;
            addNewPupilDialog.SurName = pupilSurName;
            addNewPupilDialog.Gender = gender;
            addNewPupilDialog.DateOfBirth = dateOfBirth;
            var registrationDetailDialog = addNewPupilDialog.Continue();
            registrationDetailDialog.DateOfAdmission = DateOfAdmission;
            registrationDetailDialog.YearGroup = YearGroup;
            registrationDetailDialog.ClassName = className;
            registrationDetailDialog.CreateRecord();

            // Confirm create new pupil
            var confirmRequiredDialog = new ConfirmRequiredDialog();
            confirmRequiredDialog.ClickOk();

            // Save values
            var pupilRecord = new PupilRecordPage();
            pupilRecord.SavePupil();

            #endregion

            #region Steps

            // Search pupil record
            pupilRecordTriplet = new PupilRecordTriplet();
            pupilRecordTriplet.SearchCriteria.PupilName = pupilName;
            pupilRecordTriplet.SearchCriteria.IsCurrent = true;
            var pupilRecordPage = pupilRecordTriplet.SearchCriteria.Search().FirstOrDefault(x => x.Name.Equals(pupilName)).Click<PupilRecordPage>();

            // Navigate to SEN record
            var senRecordPage = SeleniumHelper.NavigateViaAction<SenRecordDetailPage>("SEN Record");

            // Verify Sen record display current pupil
            Assert.AreEqual(true, senRecordPage.IsSenRecordForPupilName(pupilName), "SEN record display pupil name incorrect");
            // Exit SEN record
            SeleniumHelper.CloseTab("SEN Record");

            //Search pupil again
            pupilRecordTriplet = new PupilRecordTriplet();
            pupilRecordTriplet.SearchCriteria.PupilName = pupilName;
            pupilRecordTriplet.SearchCriteria.IsCurrent = true;
            pupilRecordTriplet.SearchCriteria.IsLeaver = true;
            pupilRecordPage = pupilRecordTriplet.SearchCriteria.Search().FirstOrDefault(x => x.Name.Equals(pupilName)).Click<PupilRecordPage>();

            // Navigate to pupil leaving detail and verify pupil leaving detail page displays for this pupil
            var pupilLeavingDetail = SeleniumHelper.NavigateViaAction<PupilLeavingDetailsPage>("Pupil Leaving Details");
            Assert.AreEqual(true, pupilLeavingDetail.IsPupilLeavingDetailForPupilName(pupilName), "Pupil Leaving Detail displays pupil name incorrect");

            // Enter values
            pupilLeavingDetail.DOL = dateOfLeaving;
            pupilLeavingDetail.ReasonForLeaving = reasonLeaving;
            pupilLeavingDetail.Destination = destination;

            // Save values
            confirmRequiredDialog = pupilLeavingDetail.ClickSave();
            confirmRequiredDialog.ClickOk();
            var backgroundLeaverDialog = new LeaverBackgroundProcessSubmitDialog();
            backgroundLeaverDialog.ClickOk();

            //Search pupil again
            pupilRecordTriplet = new PupilRecordTriplet();
            pupilRecordTriplet.SearchCriteria.PupilName = pupilName;
            pupilRecordTriplet.SearchCriteria.IsCurrent = true;
            pupilRecordPage = pupilRecordTriplet.SearchCriteria.Search().FirstOrDefault(x => x.Name.Equals(pupilName)).Click<PupilRecordPage>();
            pupilRecordPage.SelectRegistrationTab();

            // Verify  the leaving details provided above are accurately listed in the 'Enrolment History' record for this pupil in the 'Registration' section of the  'Pupil Record' screen.
            var enroleHistoryGrid = pupilRecordPage.EnrolmentStatusHistoryTable;
            Assert.AreNotEqual(null, enroleHistoryGrid.Rows.FirstOrDefault(x => x.EndDate.Equals(dateOfLeaving)), "Missing end date or end date is incorrect.");

            #endregion

            #region Post-condition

            // Delete a pupil
            SeleniumHelper.NavigateMenu("Tasks", "Pupils", "Delete Pupil");
            var deletePupilRecordTriplet = new DeletePupilRecordTriplet();
            deletePupilRecordTriplet.SearchCriteria.PupilName = String.Format("{0}, {1}", pupilSurName, pupilForeName);
            var resultPupils = deletePupilRecordTriplet.SearchCriteria.Search();
            var pupilSearchTile = resultPupils.SingleOrDefault(t => t.Name.Equals(String.Format("{0}, {1}", pupilSurName, pupilForeName)));
            var deletePupilRecord = pupilSearchTile == null ? new DeletePupilRecordPage() : pupilSearchTile.Click<DeletePupilRecordPage>();
            deletePupilRecord.Delete();

            #endregion
        }

        /// <summary>
        /// Author: Ba.Truong
        /// Description: PU78: Check exercise ability to make a pupil a leaver
        /// </summary>
        //TODO: Duplicate to TC_PU026
        [WebDriverTest(TimeoutSeconds = 2000, Enabled = true, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { PupilTestGroups.PupilRecord.Leaver, PupilTestGroups.Priority.Priority4 }, DataProvider = "TC_PU078_Data")]
        public void TC_PU078_Exercise_Ability_To_Make_A_Pupil_A_Leaver(string[] pupilRecords, string dateOfLeaving, string reasonForLeaving, string destination)
        {
            #region PRE-CONDITIONS

            //Create a pupil
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);
            SeleniumHelper.NavigateMenu("Tasks", "Pupils", "Pupil Records");
            var pupilRecordTriplet = new PupilRecordTriplet();
            var addNewPupilDialog = pupilRecordTriplet.AddNewPupil();
            addNewPupilDialog.Forename = pupilRecords[0];
            addNewPupilDialog.SurName = pupilRecords[1];
            addNewPupilDialog.Gender = pupilRecords[2];
            addNewPupilDialog.DateOfBirth = pupilRecords[3];

            var registrationDetailDialog = addNewPupilDialog.Continue();
            registrationDetailDialog.DateOfAdmission = pupilRecords[4];
            registrationDetailDialog.EnrolmentStatus = pupilRecords[5];
            registrationDetailDialog.YearGroup = pupilRecords[6];
            registrationDetailDialog.CreateRecord();

            var confirmRequiredDialog = new ConfirmRequiredDialog();
            confirmRequiredDialog.ClickOk();

            var pupilRecord = new PupilRecordPage();
            pupilRecord.ClickSave();

            #endregion

            #region TEST STEPS

            //Select 'Pupil Leaving Details'
            var pupilLeavingDetails = SeleniumHelper.NavigateViaAction<PupilLeavingDetailsPage>("Pupil Leaving Details");
            pupilLeavingDetails.DOL = dateOfLeaving;
            pupilLeavingDetails.ReasonForLeaving = reasonForLeaving;
            pupilLeavingDetails.Destination = destination;
            var confirmDialog = pupilLeavingDetails.ClickSave();

            //Select the next two continue options in order to make this pupil a future dated leaver
            confirmDialog.ClickOk();
            var confirmLeaver = new LeaverBackgroundProcessSubmitDialog();
            confirmLeaver.ClickOk();

            //VP: Verify Registration Section:"Date of Leaving" field displays
            pupilRecord.Refresh();
            pupilRecord.SelectRegistrationTab();
            Assert.AreEqual(dateOfLeaving, pupilRecord.DateOfLeaving, "Date of Leaving is not updated");

            //VP: Verify in Enrolment Status grid 'End Date' field also displays the 'date of leaving' date
            Assert.AreEqual(dateOfLeaving, pupilRecord.EnrolmentStatusHistoryTable[0].EndDate, "Date of Leaving is not updated");

            #endregion

            #region POS-CONDITION

            //Delete the pupil added
            SeleniumHelper.NavigateMenu("Tasks", "Pupils", "Delete Pupil");
            var deletePupilRecordTriplet = new DeletePupilRecordTriplet();
            deletePupilRecordTriplet.SearchCriteria.PupilName = pupilRecords[0];
            deletePupilRecordTriplet.SearchCriteria.IsCurrent = true;
            deletePupilRecordTriplet.SearchCriteria.IsLeaver = true;
            var pupilSearchTile = deletePupilRecordTriplet.SearchCriteria.Search().FirstOrDefault(x => x.Name.Equals(String.Format("{0}, {1}", pupilRecords[0], pupilRecords[1])));
            var pupilRecordDelete = pupilSearchTile.Click<DeletePupilRecordPage>();
            pupilRecordDelete.Delete();

            #endregion
        }

        //[WebDriverTest(TimeoutSeconds = 2000, Enabled = true, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { PupilTestGroups.PupilRecord.PupilUndoLeaverTests, PupilTestGroups.Priority.Priority3 })]
        public void UndoMakeLeaver()
        {
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);
            //SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.TestUser);

            // Make a leaver...
            SeleniumHelper.NavigateQuickLink("Pupil Records");

            //Search for current non-leaver
            var pupilSearch = new PupilRecordTriplet();
            pupilSearch.SearchCriteria.IsCurrent = true;
            var pupilSearchResults = pupilSearch.SearchCriteria.Search();

            //Select random pupil (nth in list)
            var pupilTile = pupilSearchResults[1];
            var pupilRecord = pupilTile.Click<PupilRecordPage>();

            //Go to Leaver Details contextual link page
            var pupilLeavingDetailsPage = SeleniumHelper.NavigateViaAction<PupilLeavingDetailsPage>("Pupil Leaving Details");
            pupilLeavingDetailsPage.DOL = "01/12/2015";
            pupilLeavingDetailsPage.ReasonForLeaving = "Special Unit";
            pupilLeavingDetailsPage.ClickSave();

            // Confirmation screen pop-up
            SeleniumHelper.FindAndClick(SeleniumHelper.SelectByDataAutomationID("ok_button"));

            // Second Confirmation screen pop-up. Give background task a chance to run first
            Thread.Sleep(5000);
            SeleniumHelper.FindAndClick(SeleniumHelper.SelectByDataAutomationID("ok,_return_me_to_the_pupil_record_button"));

            // Get the current pupil and check leaving date is as expected
            pupilRecord = PupilRecordPage.Create();
            var leavingDate = pupilRecord.DateOfLeaving;
            Assert.AreEqual(leavingDate, "1/12/2015");

            //Search for current non-leaver
            pupilSearch = new PupilRecordTriplet();
            pupilSearch.SearchCriteria.IsCurrent = false;
            pupilSearch.SearchCriteria.IsLeaver = true;
            pupilSearchResults = pupilSearch.SearchCriteria.Search();

            //Go to leaver details for first pupil in list
            pupilTile = pupilSearchResults[0];
            pupilRecord = pupilTile.Click<PupilRecordPage>();

            // Now undo that leaving
            pupilLeavingDetailsPage = SeleniumHelper.NavigateViaAction<PupilLeavingDetailsPage>("Pupil Leaving Details");
            SeleniumHelper.FindAndClick(SeleniumHelper.SelectByDataAutomationID("ActivateCustomBehaviourButton-Undo-Leaver"));

            // Confirmation screen pop-up, will return to pupil page
            Thread.Sleep(15000);
            SeleniumHelper.FindAndClick(SeleniumHelper.SelectByDataAutomationID("ok_button"));

            // No longer a leaver? Reload pupil and check
            pupilRecord = PupilRecordPage.Create();
            var DOL = SeleniumHelper.DoesWebElementExist(By.CssSelector("[name='DateOfLeaving']"));
            Assert.IsFalse(DOL);
        }

        #endregion

        #region PupilLog

        /// <summary>
        /// Author: Luong.Mai
        /// Description: PU-033 : 
        /// FAIL
        /// </summary>
        //TODO: Duplication of TC_PU034
        //[WebDriverTest(TimeoutSeconds = 1200, Enabled = true, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { PupilTestGroups.PupilLog.PupilLogTests, PupilTestGroups.Priority.Priority3 })]
        public void TC_PU033_Exercise_ability_to_view_a_selection_of_Pupil_logs()
        {
            #region STEPS

            // Login as school admin
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);

            // Navigate to Pupil Log
            SeleniumHelper.NavigateQuickLink("Pupil Log");

            // Search all pupils
            var pupilLogTriplet = new PupilLogTriplet();
            pupilLogTriplet.SearchCriteria.IsCurrent = true;
            pupilLogTriplet.SearchCriteria.IsLeaver = false;
            var pupilResuls = pupilLogTriplet.SearchCriteria.Search();

            // SELECT 4TH PUPIL
            var pupilYearGroup = pupilResuls[3].YearGroup;
            var pupilClass = pupilResuls[3].Class;
            var pupilName = pupilResuls[3].Name;

            // Navigate to Pupil Records
            SeleniumHelper.NavigateQuickLink("Pupil Records");
            var pupilRecordTriplet = new PupilRecordTriplet();
            pupilRecordTriplet.SearchCriteria.IsCurrent = true;
            pupilRecordTriplet.SearchCriteria.IsFuture = true;
            pupilRecordTriplet.SearchCriteria.IsLeaver = false;
            pupilRecordTriplet.SearchCriteria.PupilName = pupilName;
            var pupilResults = pupilRecordTriplet.SearchCriteria.Search();
            var pupilRecordPage = pupilResults.FirstOrDefault(x => x.Name.Trim().Equals(pupilName)).Click<PupilRecordPage>();

            // Verify that Once the 'Pupil Log' panel displays, 
            // visually identify the 'Year/Class' membership combination for this pupil.
            Assert.AreEqual(true, pupilRecordPage.YearGroup.Trim().Contains(pupilYearGroup), "Content of 'Year/Class' isn't extractly");
            Assert.AreEqual(true, pupilRecordPage.Class.Trim().Contains(pupilClass), "Content of 'Year/Class' isn't extractly");

            // Navigate to Pupil Log
            SeleniumHelper.NavigateQuickLink("Pupil Log");
            pupilLogTriplet = new PupilLogTriplet();

            // Search with more options
            pupilLogTriplet.SearchCriteria.ClickSearchAdvanced(true);
            pupilLogTriplet.SearchCriteria.Class = pupilClass;
            pupilLogTriplet.SearchCriteria.YearGroup = pupilYearGroup;
            pupilResuls = pupilLogTriplet.SearchCriteria.Search();

            // Confirm the pupil originally selected from a non-fileted search (as performed above) 
            // is listed / selectable in the now filtered 'Year/Class' combination results list.
            Assert.AreNotEqual(null, pupilResuls.SingleOrDefault
                (x => x.Name.Trim().Equals(pupilName))
                , "The pupil originally selected from a non-fileted search isn't displayed in the new result.");

            // SELECT 1TH PUPIL
            pupilYearGroup = pupilResuls[0].YearGroup;
            pupilClass = pupilResuls[0].Class;
            pupilName = pupilResuls[0].Name;

            // Navigate to Pupil Records
            SeleniumHelper.NavigateQuickLink("Pupil Records");
            pupilRecordTriplet = new PupilRecordTriplet();
            pupilRecordTriplet.SearchCriteria.IsCurrent = true;
            pupilRecordTriplet.SearchCriteria.IsFuture = true;
            pupilRecordTriplet.SearchCriteria.IsLeaver = false;
            pupilRecordTriplet.SearchCriteria.PupilName = pupilName;
            pupilResults = pupilRecordTriplet.SearchCriteria.Search();
            pupilRecordPage = pupilResults.FirstOrDefault(x => x.Name.Trim().Equals(pupilName)).Click<PupilRecordPage>();

            // Verify that Once the 'Pupil Log' panel displays, 
            // visually identify the 'Year/Class' membership combination for this pupil.
            Assert.AreEqual(true, pupilRecordPage.YearGroup.Trim().Contains(pupilYearGroup), "Content of 'Year/Class' isn't extractly");
            Assert.AreEqual(true, pupilRecordPage.Class.Trim().Contains(pupilClass), "Content of 'Year/Class' isn't extractly");

            // SELECT 2TH PUPIL
            SeleniumHelper.NavigateQuickLink("Pupil Log");
            pupilLogTriplet = new PupilLogTriplet();
            pupilLogTriplet.SearchCriteria.IsCurrent = true;
            pupilLogTriplet.SearchCriteria.IsLeaver = false;
            pupilResuls = pupilLogTriplet.SearchCriteria.Search();

            pupilYearGroup = pupilResuls[1].YearGroup;
            pupilClass = pupilResuls[1].Class;
            pupilName = pupilResuls[1].Name;

            // Navigate to Pupil Records
            SeleniumHelper.NavigateQuickLink("Pupil Records");
            pupilRecordTriplet = new PupilRecordTriplet();
            pupilRecordTriplet.SearchCriteria.IsCurrent = true;
            pupilRecordTriplet.SearchCriteria.IsFuture = true;
            pupilRecordTriplet.SearchCriteria.IsLeaver = false;
            pupilRecordTriplet.SearchCriteria.PupilName = pupilName;
            pupilResults = pupilRecordTriplet.SearchCriteria.Search();
            pupilRecordPage = pupilResults.FirstOrDefault(x => x.Name.Trim().Equals(pupilName)).Click<PupilRecordPage>();

            // Verify that Once the 'Pupil Log' panel displays, 
            // visually identify the 'Year/Class' membership combination for this pupil.
            Assert.AreEqual(true, pupilRecordPage.YearGroup.Trim().Contains(pupilYearGroup), "Content of 'Year/Class' isn't extractly");
            Assert.AreEqual(true, pupilRecordPage.Class.Trim().Contains(pupilClass), "Content of 'Year/Class' isn't extractly");

            // SELECT 3TH PUPIL
            SeleniumHelper.NavigateQuickLink("Pupil Log");
            pupilLogTriplet = new PupilLogTriplet();
            pupilLogTriplet.SearchCriteria.IsCurrent = true;
            pupilLogTriplet.SearchCriteria.IsLeaver = false;
            pupilResuls = pupilLogTriplet.SearchCriteria.Search();

            pupilYearGroup = pupilResuls[2].YearGroup;
            pupilClass = pupilResuls[2].Class;
            pupilName = pupilResuls[2].Name;

            // Navigate to Pupil Records
            SeleniumHelper.NavigateQuickLink("Pupil Records");
            pupilRecordTriplet = new PupilRecordTriplet();
            pupilRecordTriplet.SearchCriteria.IsCurrent = true;
            pupilRecordTriplet.SearchCriteria.IsFuture = true;
            pupilRecordTriplet.SearchCriteria.IsLeaver = false;
            pupilRecordTriplet.SearchCriteria.PupilName = pupilName;
            pupilResults = pupilRecordTriplet.SearchCriteria.Search();
            pupilRecordPage = pupilResults.FirstOrDefault(x => x.Name.Trim().Equals(pupilName)).Click<PupilRecordPage>();

            // Verify that Once the 'Pupil Log' panel displays, 
            // visually identify the 'Year/Class' membership combination for this pupil.
            Assert.AreEqual(true, pupilRecordPage.YearGroup.Trim().Contains(pupilYearGroup), "Content of 'Year/Class' isn't extractly");
            Assert.AreEqual(true, pupilRecordPage.Class.Trim().Contains(pupilClass), "Content of 'Year/Class' isn't extractly");

            // SELECT 4TH PUPIL
            SeleniumHelper.NavigateQuickLink("Pupil Log");
            pupilLogTriplet = new PupilLogTriplet();
            pupilLogTriplet.SearchCriteria.IsCurrent = true;
            pupilLogTriplet.SearchCriteria.IsLeaver = false;
            pupilResuls = pupilLogTriplet.SearchCriteria.Search();

            pupilYearGroup = pupilResuls[3].YearGroup;
            pupilClass = pupilResuls[3].Class;
            pupilName = pupilResuls[3].Name;

            // Navigate to Pupil Records
            SeleniumHelper.NavigateQuickLink("Pupil Records");
            pupilRecordTriplet = new PupilRecordTriplet();
            pupilRecordTriplet.SearchCriteria.IsCurrent = true;
            pupilRecordTriplet.SearchCriteria.IsFuture = true;
            pupilRecordTriplet.SearchCriteria.IsLeaver = false;
            pupilRecordTriplet.SearchCriteria.PupilName = pupilName;
            pupilResults = pupilRecordTriplet.SearchCriteria.Search();
            pupilRecordPage = pupilResults.FirstOrDefault(x => x.Name.Trim().Equals(pupilName)).Click<PupilRecordPage>();

            // Verify that Once the 'Pupil Log' panel displays, 
            // visually identify the 'Year/Class' membership combination for this pupil.
            Assert.AreEqual(true, pupilRecordPage.YearGroup.Trim().Contains(pupilYearGroup), "Content of 'Year/Class' isn't extractly");
            Assert.AreEqual(true, pupilRecordPage.Class.Trim().Contains(pupilClass), "Content of 'Year/Class' isn't extractly");


            #endregion
        }

        /// <summary>
        /// Author: Luong.Mai
        /// Description: PU-036b : 
        /// </summary>
        //[WebDriverTest(TimeoutSeconds = 1200, Enabled = true, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { PupilTestGroups.PupilLog.PupilLogTests, PupilTestGroups.Priority.Priority3 }, DataProvider = "TC_PU036b_Data")]
        public void TC_PU036b_Exercise_ability_to_create_notes(string forenamePP, string surnamePP)
        {
            // Login as school admin
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);

            #region Pre-Condition: Create a new pupil for test

            // Navigate to Pupil Record
            SeleniumHelper.NavigateMenu("Tasks", "Pupils", "Pupil Records");

            var pupilRecordTriplet = new PupilRecordTriplet();
            var addNewPupilDialog = pupilRecordTriplet.AddNewPupil();
            addNewPupilDialog.Forename = forenamePP;
            addNewPupilDialog.SurName = surnamePP;
            addNewPupilDialog.Gender = "Male";
            addNewPupilDialog.DateOfBirth = "2/2/2011";

            var registrationDetailDialog = addNewPupilDialog.Continue();
            registrationDetailDialog.DateOfAdmission = "2/2/2015";
            registrationDetailDialog.YearGroup = "Year 1";
            registrationDetailDialog.CreateRecord();

            var confirmRequiredDialog = new ConfirmRequiredDialog();
            confirmRequiredDialog.ClickOk();

            var pupilRecordPage = PupilRecordPage.Create();
            pupilRecordPage.SavePupil();

            #endregion

            #region STEPS

            // Navigate to Pupil Log
            SeleniumHelper.NavigateBySearch("Pupil Log", true);

            // Search all pupils
            var pupilLogTriplet = new PupilLogTriplet();
            pupilLogTriplet.SearchCriteria.PupilName = surnamePP + ", " + forenamePP;
            pupilLogTriplet.SearchCriteria.IsCurrent = true;
            pupilLogTriplet.SearchCriteria.IsLeaver = false;
            var pupilResuls = pupilLogTriplet.SearchCriteria.Search();

            // Select pupils
            var pupilLogDetailPage = pupilResuls[0].Click<PupilLogDetailPage>();

            // Expand the 'Create Notes' selector, click on type 'Attendance'.
            pupilLogDetailPage.ClickCreateNote();
            var noteDialog = pupilLogDetailPage.SelelectAttendanceNoteType();

            // Create note Attendance
            noteDialog.Note = "Attendance Notice";
            noteDialog.Title = "Attendance Notice";
            noteDialog.SubCategory = "Authorised";
            noteDialog.PinThisNote = false;
            noteDialog.ClickSave();

            // Verify that creating note is successfully  
            var notes = pupilLogDetailPage.TimeLine;
            Assert.AreNotEqual(null, notes["Attendance Notice"], "Creating note is unsuccessfully.");

            // Expand the 'Create Notes' selector, click on type 'SEN'.
            pupilLogDetailPage.ClickCreateNote();
            noteDialog = pupilLogDetailPage.SelelectSENNoteType();

            // Create note Assessment
            noteDialog.Note = "SEN Notice";
            noteDialog.Title = "SEN Notice";
            noteDialog.SubCategory = "General";
            noteDialog.PinThisNote = false;
            noteDialog.ClickSave();

            // Verify that creating note is successfully
            notes = pupilLogDetailPage.TimeLine;
            Assert.AreNotEqual(null, notes["SEN Notice"], "Creating note is unsuccessfully.");

            #endregion

            #region Post-Condition: Delete the pupil if existed

            // Navigate to Pupil Record
            SeleniumHelper.NavigateMenu("Tasks", "Pupils", "Delete Pupil");
            var deletePupilRecordTriplet = new DeletePupilRecordTriplet();
            deletePupilRecordTriplet.SearchCriteria.PupilName = String.Format("{0}, {1}", surnamePP, forenamePP);
            deletePupilRecordTriplet.SearchCriteria.IsCurrent = true;
            deletePupilRecordTriplet.SearchCriteria.IsFuture = true;
            deletePupilRecordTriplet.SearchCriteria.IsLeaver = true;
            var resultPupils = deletePupilRecordTriplet.SearchCriteria.Search();

            var pupilSearchTile = resultPupils.SingleOrDefault(t => t.Name.Equals(String.Format("{0}, {1}", surnamePP, forenamePP)));
            var deletePupilRecordPage = pupilSearchTile == null ? new DeletePupilRecordPage() : pupilSearchTile.Click<DeletePupilRecordPage>();

            deletePupilRecordPage.Delete();

            #endregion

        }


        /// <summary>
        /// Author: Luong.Mai
        /// Description: PU-037 : 
        /// </summary>
        //[WebDriverTest(TimeoutSeconds = 1200, Enabled = true, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { PupilTestGroups.PupilLog.PupilLogTests, PupilTestGroups.Priority.Priority3 }, DataProvider = "TC_PU037_Data")]
        public void TC_PU037_Exercise_ability_to_view_a_pupil_summary_dialog(string pupilName, string forename)
        {
            // Login as school admin
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);

            // Navigate to Pupil Log
            SeleniumHelper.NavigateBySearch("Pupil Log", true);

            // Search pupil
            var pupilLogTriplet = new PupilLogTriplet();
            pupilLogTriplet.SearchCriteria.PupilName = pupilName;
            pupilLogTriplet.SearchCriteria.IsCurrent = true;
            pupilLogTriplet.SearchCriteria.IsLeaver = false;
            var pupilResuls = pupilLogTriplet.SearchCriteria.Search();

            // Select pupil
            var pupilLogDetailPage = pupilResuls[0].Click<PupilLogDetailPage>();

            // Click on "Pupil Summary" icon
            var pupilSummaryDialog = pupilLogDetailPage.ClickPupilSummaryIcon();

            // Confirming a predefined set of summarily information is displayed
            Assert.AreEqual(true, pupilSummaryDialog.IsPupilSummaryPopupDisplayed(), "Pupil Summary dialog isn't displayed.");

            // Click on "Pupil Record" link
            var pupilRecordDetailPage = pupilSummaryDialog.ClickPupilRecordLink();

            // Close "Pupil Record" tab
            SeleniumHelper.CloseTab("Pupil Record");

            // Verify that Pupil Log screen is displayed
            pupilLogDetailPage = new PupilLogDetailPage();
            Assert.AreEqual(true, pupilLogDetailPage.PupilForename.Equals(forename), "Pupil Log screen isn't displayed");

        }

        /// <summary>
        /// Author: Luong.Mai
        /// Description: PU-038 : 
        /// </summary>
        //[WebDriverTest(TimeoutSeconds = 1200, Enabled = true, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { PupilTestGroups.PupilLog.PupilLogTests, PupilTestGroups.Priority.Priority3 }, DataProvider = "TC_PU038_Data")]
        public void TC_PU038_Exercise_ability_to_view_a_medical_details_dialog(string pupilName, string forename)
        {
            // Login as school admin
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);

            // Navigate to Pupil Log
            SeleniumHelper.NavigateBySearch("Pupil Log", true);

            // Search pupil
            var pupilLogTriplet = new PupilLogTriplet();
            pupilLogTriplet.SearchCriteria.PupilName = pupilName;
            pupilLogTriplet.SearchCriteria.IsCurrent = true;
            pupilLogTriplet.SearchCriteria.IsLeaver = false;
            var pupilResuls = pupilLogTriplet.SearchCriteria.Search();

            // Select pupil
            var pupilLogDetailPage = pupilResuls[0].Click<PupilLogDetailPage>();

            // Click on "Medical Details" icon
            var medicalDetailsDialog = pupilLogDetailPage.ClickMedicalDetailsIcon();

            // Confirming a predefined set of summarily information is displayed
            Assert.AreEqual(true, medicalDetailsDialog.IsMedicalDetailsPopupDisplayed(), "Medical Details dialog isn't displayed.");

            // Click on "Pupil Record Medical Details" link
            var pupilRecordDetailPage = medicalDetailsDialog.ClickMedicalDetailsLink();

            // Close "Pupil Record" tab
            SeleniumHelper.CloseTab("Pupil Record");

            // Verify that Pupil Log screen is displayed
            pupilLogDetailPage = new PupilLogDetailPage();
            Assert.AreEqual(true, pupilLogDetailPage.PupilForename.Equals(forename), "Pupil Log screen isn't displayed");
        }

        /// <summary>
        /// Author: Luong.Mai
        /// Description: PU-039 : 
        /// </summary>
        //[WebDriverTest(TimeoutSeconds = 1200, Enabled = true, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { PupilTestGroups.PupilLog.PupilLogTests, PupilTestGroups.Priority.Priority3 }, DataProvider = "TC_PU039_Data")]
        public void TC_PU039_Exercise_ability_to_view_a_contact_details_dialog(string pupilName, string forename)
        {
            // Login as school admin
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);

            // Navigate to Pupil Log
            SeleniumHelper.NavigateBySearch("Pupil Log", true);

            // Search pupil
            var pupilLogTriplet = new PupilLogTriplet();
            pupilLogTriplet.SearchCriteria.PupilName = pupilName;
            pupilLogTriplet.SearchCriteria.IsCurrent = true;
            pupilLogTriplet.SearchCriteria.IsLeaver = false;
            var pupilResuls = pupilLogTriplet.SearchCriteria.Search();

            // Select pupil
            var pupilLogDetailPage = pupilResuls[0].Click<PupilLogDetailPage>();

            // Click on "Contact Details" icon
            var contactDetailsDialog = pupilLogDetailPage.ClickContactDetailsIcon();

            // Confirming a predefined set of summarily information is displayed
            Assert.AreEqual(true, contactDetailsDialog.IsContactDetailsPopupDisplayed(), "Contact Details dialog isn't displayed.");

            // Click on "Pupil Record Contact Details" link
            var pupilRecordDetailPage = contactDetailsDialog.ClickContactDetailsLink();

            // Close "Pupil Record" tab
            SeleniumHelper.CloseTab("Pupil Record");

            // Verify that Pupil Log screen is displayed
            pupilLogDetailPage = new PupilLogDetailPage();
            Assert.AreEqual(true, pupilLogDetailPage.PupilForename.Equals(forename), "Pupil Log screen isn't displayed");
        }

        /// <summary>
        /// Author: Luong.Mai
        /// Description: PU-040 : 
        /// </summary>
        //[WebDriverTest(TimeoutSeconds = 1200, Enabled = true, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { PupilTestGroups.PupilLog.PupilLogTests, PupilTestGroups.Priority.Priority3 }, DataProvider = "TC_PU040_Data")]
        public void TC_PU040_Exercise_ability_to_view_a_linked_pupils_dialog(string pupilName)
        {
            // Login as school admin
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);

            // Navigate to Pupil Log
            SeleniumHelper.NavigateBySearch("Pupil Log", true);

            // Search pupil
            var pupilLogTriplet = new PupilLogTriplet();
            pupilLogTriplet.SearchCriteria.PupilName = pupilName;
            pupilLogTriplet.SearchCriteria.IsCurrent = true;
            pupilLogTriplet.SearchCriteria.IsLeaver = false;
            var pupilResuls = pupilLogTriplet.SearchCriteria.Search();

            // Select pupil
            var pupilLogDetailPage = pupilResuls[0].Click<PupilLogDetailPage>();

            // Click on "Linked Pupils" icon
            var linkedPupilsDialog = pupilLogDetailPage.ClickLinkedPupilsIcon();

            // Confirming a predefined set of summarily information is displayed
            Assert.AreEqual(true, linkedPupilsDialog.IsLinkedPupilsPopupDisplayed(), "Linked Pupils dialog isn't displayed.");

            // Click on "Pupil Record Family" link
            var pupilRecordDetailPage = linkedPupilsDialog.ClickFamilyLink();

            // Close "Pupil Record" tab
            SeleniumHelper.CloseTab("Pupil Record");

            // Verify that Pupil Log screen is displayed
            pupilLogDetailPage = new PupilLogDetailPage();
            Assert.AreEqual(true, pupilLogDetailPage.PupilForename.Equals("Bains"), "Pupil Log screen isn't displayed");
        }
        
        /// <summary>
        /// Author: Luong.Mai
        /// Description: PU-041 : 
        /// </summary>
        //[WebDriverTest(TimeoutSeconds = 1200, Enabled = true, Browsers = new[] { BrowserDefaults.Chrome },
        //    Groups = new[] { PupilTestGroups.PupilLog.PupilLogTests, PupilTestGroups.Priority.Priority2 }, DataProvider = "TC_PU041_Data")]
        public void TC_PU041_Exercise_ability_to_view_the_Latest_Assessment_Results(string pupilName)
        {
            // Login as school admin
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);

            // Navigate to Pupil Log
            SeleniumHelper.NavigateBySearch("Pupil Log", true);

            // Search pupil
            var pupilLogTriplet = new PupilLogTriplet();
            pupilLogTriplet.SearchCriteria.PupilName = pupilName;
            pupilLogTriplet.SearchCriteria.IsCurrent = true;
            pupilLogTriplet.SearchCriteria.IsLeaver = false;
            var pupilResuls = pupilLogTriplet.SearchCriteria.Search();

            // Select pupil
            var pupilLogDetailPage = pupilResuls[0].Click<PupilLogDetailPage>();

            // Confirm the 'Latest Assessment Results' panel lists the 'Aspect' and 'Result' records for their assessment results. 
            Assert.AreEqual(true, pupilLogDetailPage.IsAssessmentResultsDisplayed(), "Fail: 'Latest Assessment Results' panel isn't displayed.");

        }
        
        /// <summary>
        /// Author: Ba.Truong
        /// Description: PU87: Check exercise ability to view a Pupil's log when the pupil is a current member of a specific 'Class'.
        /// </summary>
        //[WebDriverTest(TimeoutSeconds = 1500, Enabled = true, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { PupilTestGroups.PupilLog.PupilLogTests, PupilTestGroups.Priority.Priority3 }, DataProvider = "TC_PU087_Data")]
        public void TC_PU087_Exercise_Ability_View_A_Pupil_Log_When_The_Pupil_Is_Current_Member_Of_Specific_Class(string className)
        {
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);
            SeleniumHelper.NavigateQuickLink("Pupil Log");

            //Search 'Current' pupils following class and are 'Single registration' pupils
            var pupilLogTrilet = new PupilLogTriplet();
            pupilLogTrilet.SearchCriteria.ClickSearchAdvanced(true);
            pupilLogTrilet.SearchCriteria.IsCurrent = true;
            pupilLogTrilet.SearchCriteria.Class = className;
            pupilLogTrilet.SearchCriteria.EnrolmentStatus = "Single Registration";
            var pupilTiles = pupilLogTrilet.SearchCriteria.Search();

            //Confirm that the items in result panel is belonging to the selected 'Class'
            //Check the first item
            var pupilLogDetails = pupilTiles[0].Click<PupilLogDetailPage>();
            var pupilDetails = SeleniumHelper.NavigateViaAction<PupilRecordPage>("Pupil Record");
            pupilDetails.SelectRegistrationTab();
            Assert.AreEqual(true, pupilDetails.Class.Contains(className), "Class of pupil in search results is not correct");
            SeleniumHelper.CloseTab("Pupil Record");

            //Check the middle item
            pupilLogTrilet = new PupilLogTriplet();
            pupilTiles = pupilLogTrilet.SearchCriteria.Search();
            pupilLogDetails = pupilTiles[Convert.ToInt16(pupilTiles.Count() / 2)].Click<PupilLogDetailPage>();
            pupilDetails = SeleniumHelper.NavigateViaAction<PupilRecordPage>("Pupil Record");
            pupilDetails.SelectRegistrationTab();
            Assert.AreEqual(true, pupilDetails.Class.Contains(className), "Class of pupil in search results is not correct");
            SeleniumHelper.CloseTab("Pupil Record");

            //Check the last item
            pupilLogTrilet = new PupilLogTriplet();
            pupilTiles = pupilLogTrilet.SearchCriteria.Search();
            pupilLogDetails = pupilTiles[pupilTiles.Count() - 1].Click<PupilLogDetailPage>();
            pupilDetails = SeleniumHelper.NavigateViaAction<PupilRecordPage>("Pupil Record");
            pupilDetails.SelectRegistrationTab();
            Assert.AreEqual(true, pupilDetails.Class.Contains(className), "Class of pupil in search results is not correct");
            SeleniumHelper.CloseTab("Pupil Record");

            //Select the pupil who is 3rd up from the very bottom of the results list
            pupilLogTrilet = new PupilLogTriplet();
            pupilTiles = pupilLogTrilet.SearchCriteria.Search();
            var pupilTile = pupilTiles[pupilTiles.Count() - 3];
            pupilLogDetails = pupilTile.Click<PupilLogDetailPage>();

            //Check latest assessment results are displayed in the 'Latest Assessment Result's panel
            Assert.AreEqual(true, pupilLogDetails.IsAssessmentResultsDisplayed(), "Assessment results are not displayed");
        }

        /// <summary>
        /// Author: Ba.Truong
        /// Description: PU88: Check exercise ability to view a Pupil's log when the pupil is a current member of a specific 'Year Group' (School Year).
        /// </summary>
        //[WebDriverTest(TimeoutSeconds = 1500, Enabled = true, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { PupilTestGroups.PupilLog.PupilLogTests, PupilTestGroups.Priority.Priority3 }, DataProvider = "TC_PU088_Data")]
        public void TC_PU088_Exercise_Ability_View_A_Pupil_Log_When_The_Pupil_Is_Current_Member_Of_Specific_Year_Group(string yearGroup)
        {
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);
            SeleniumHelper.NavigateQuickLink("Pupil Log");

            //Search 'Current' pupils are 'Single registration' pupils
            var pupilLogTrilet = new PupilLogTriplet();
            pupilLogTrilet.SearchCriteria.ClickSearchAdvanced(true);
            pupilLogTrilet.SearchCriteria.IsCurrent = true;
            pupilLogTrilet.SearchCriteria.YearGroup = yearGroup;
            pupilLogTrilet.SearchCriteria.EnrolmentStatus = "Single Registration";
            var pupilTiles = pupilLogTrilet.SearchCriteria.Search();

            //Confirm that the item in result panel is belonging to the selected 'Year Group'
            //Check the first item
            var pupilLogDetails = pupilTiles[0].Click<PupilLogDetailPage>();
            var pupilDetails = SeleniumHelper.NavigateViaAction<PupilRecordPage>("Pupil Record");
            pupilDetails.SelectRegistrationTab();
            Assert.AreEqual(true, pupilDetails.YearGroup.Contains(yearGroup), "Year group of pupil in search results is not correct");
            SeleniumHelper.CloseTab("Pupil Record");

            //Check the middle item
            pupilLogTrilet = new PupilLogTriplet();
            pupilTiles = pupilLogTrilet.SearchCriteria.Search();
            pupilLogDetails = pupilTiles[Convert.ToInt16(pupilTiles.Count() / 2)].Click<PupilLogDetailPage>();
            pupilDetails = SeleniumHelper.NavigateViaAction<PupilRecordPage>("Pupil Record");
            pupilDetails.SelectRegistrationTab();
            Assert.AreEqual(true, pupilDetails.YearGroup.Contains(yearGroup), "Year group of pupil in search results is not correct");
            SeleniumHelper.CloseTab("Pupil Record");

            //Check the last item
            pupilLogTrilet = new PupilLogTriplet();
            pupilTiles = pupilLogTrilet.SearchCriteria.Search();
            pupilLogDetails = pupilTiles[pupilTiles.Count() - 1].Click<PupilLogDetailPage>();
            pupilDetails = SeleniumHelper.NavigateViaAction<PupilRecordPage>("Pupil Record");
            pupilDetails.SelectRegistrationTab();
            Assert.AreEqual(true, pupilDetails.YearGroup.Contains(yearGroup), "Year group of pupil in search results is not correct");
            SeleniumHelper.CloseTab("Pupil Record");

            //Select the pupil at the very bottom of the results list
            pupilLogTrilet = new PupilLogTriplet();
            pupilTiles = pupilLogTrilet.SearchCriteria.Search();
            var pupilTile = pupilTiles[pupilTiles.Count() - 1];
            pupilLogDetails = pupilTile.Click<PupilLogDetailPage>();

            //Check latest assessment results are displayed in the 'Latest Assessment Result's panel
            Assert.AreEqual(true, pupilLogDetails.IsAssessmentResultsDisplayed(), "Assessment results are not displayed");
        }

        /// <summary>
        /// Author: Ba.Truong
        /// Description: PU89: Check exercise ability to view a Pupil's log when the pupil is a current member of a 'Year Group' + 'Class' combination.
        /// </summary>
        //[WebDriverTest(TimeoutSeconds = 1500, Enabled = true, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { PupilTestGroups.PupilLog.PupilLogTests, PupilTestGroups.Priority.Priority3 }, DataProvider = "TC_PU089_Data")]
        public void TC_PU089_Exercise_Ability_View_A_Pupil_Log_When_The_Pupil_Is_Current_Member_Of_Specific_Year_Group(string className, string yearGroup)
        {
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);
            SeleniumHelper.NavigateQuickLink("Pupil Log");

            //Search 'Current' pupils are 'Single registration' pupils
            var pupilLogTrilet = new PupilLogTriplet();
            pupilLogTrilet.SearchCriteria.ClickSearchAdvanced(true);
            pupilLogTrilet.SearchCriteria.IsCurrent = true;
            pupilLogTrilet.SearchCriteria.Class = className;
            pupilLogTrilet.SearchCriteria.YearGroup = yearGroup;
            pupilLogTrilet.SearchCriteria.EnrolmentStatus = "Single Registration";
            var pupilTiles = pupilLogTrilet.SearchCriteria.Search();

            //Confirm that the item in result panel is belonging to the selected 'Class' and 'Year Group'
            //Check the first item
            var pupilLogDetails = pupilTiles[0].Click<PupilLogDetailPage>();
            var pupilDetails = SeleniumHelper.NavigateViaAction<PupilRecordPage>("Pupil Record");
            pupilDetails.SelectRegistrationTab();
            Assert.AreEqual(true, pupilDetails.Class.Contains(className), "Class of pupil in search results is not correct");
            Assert.AreEqual(true, pupilDetails.YearGroup.Contains(yearGroup), "Year group of pupil in search results is not correct");
            SeleniumHelper.CloseTab("Pupil Record");

            //Check the middle item
            pupilLogTrilet = new PupilLogTriplet();
            pupilTiles = pupilLogTrilet.SearchCriteria.Search();
            pupilLogDetails = pupilTiles[Convert.ToInt16(pupilTiles.Count() / 2)].Click<PupilLogDetailPage>();
            pupilDetails = SeleniumHelper.NavigateViaAction<PupilRecordPage>("Pupil Record");
            pupilDetails.SelectRegistrationTab();
            Assert.AreEqual(true, pupilDetails.Class.Contains(className), "Class of pupil in search results is not correct");
            Assert.AreEqual(true, pupilDetails.YearGroup.Contains(yearGroup), "Year group of pupil in search results is not correct");
            SeleniumHelper.CloseTab("Pupil Record");

            //Check the last item
            pupilLogTrilet = new PupilLogTriplet();
            pupilTiles = pupilLogTrilet.SearchCriteria.Search();
            pupilLogDetails = pupilTiles[pupilTiles.Count() - 1].Click<PupilLogDetailPage>();
            pupilDetails = SeleniumHelper.NavigateViaAction<PupilRecordPage>("Pupil Record");
            pupilDetails.SelectRegistrationTab();
            Assert.AreEqual(true, pupilDetails.Class.Contains(className), "Class of pupil in search results is not correct");
            Assert.AreEqual(true, pupilDetails.YearGroup.Contains(yearGroup), "Year group of pupil in search results is not correct");
            SeleniumHelper.CloseTab("Pupil Record");

            //Select  the 2nd pupil from the top of the results list
            pupilLogTrilet = new PupilLogTriplet();
            pupilTiles = pupilLogTrilet.SearchCriteria.Search();
            var pupilTile = pupilTiles[1];
            pupilLogDetails = pupilTile.Click<PupilLogDetailPage>();

            //Check latest assessment results are displayed in the 'Latest Assessment Result's panel           
            Assert.AreEqual(true, pupilLogDetails.IsAssessmentResultsDisplayed(), "Assessment results are not displayed");

        }

        /// <summary>
        /// Author: Ba.Truong
        /// Description: PU90: Check exercise ability to view a Pupil's log when the pupil is a former member of a 'Year Group' + 'Class' combination.
        /// </summary>
        //[WebDriverTest(TimeoutSeconds = 1500, Enabled = true, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { PupilTestGroups.PupilLog.PupilLogTests, PupilTestGroups.Priority.Priority3 }, DataProvider = "TC_PU090_Data")]
        public void TC_PU090_Exercise_Ability_View_A_Pupil_Log_When_The_Pupil_Is_Current_Member_Of_Specific_Year_Group(string className, string yearGroup)
        {
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);
            SeleniumHelper.NavigateQuickLink("Pupil Log");

            //Search 'Leaver' pupils are 'Single registration' pupils
            var pupilLogTrilet = new PupilLogTriplet();
            pupilLogTrilet.SearchCriteria.ClickSearchAdvanced(true);
            pupilLogTrilet.SearchCriteria.IsCurrent = false;
            pupilLogTrilet.SearchCriteria.IsLeaver = true;
            pupilLogTrilet.SearchCriteria.Class = className;
            pupilLogTrilet.SearchCriteria.YearGroup = yearGroup;
            pupilLogTrilet.SearchCriteria.EnrolmentStatus = "Single Registration";
            var pupilTiles = pupilLogTrilet.SearchCriteria.Search();

            //Confirm that the item in result panel is belonging to the selected 'Class' and 'Year Group'
            //Check the first item
            var pupilLogDetails = pupilTiles[0].Click<PupilLogDetailPage>();
            var pupilDetails = SeleniumHelper.NavigateViaAction<PupilRecordPage>("Pupil Record");
            pupilDetails.SelectRegistrationTab();

            var classMembershipDialog = pupilDetails.ViewClassHistory();
            var classes = classMembershipDialog.Classes;
            Assert.AreEqual(true, classes.Rows.Any(x => x.Class.Contains(className)), "Class of pupil in search results is not correct");
            classMembershipDialog.ClickOK();

            var yearGroupMembershipDialog = pupilDetails.ViewYearGroupHistory();
            var yearGroups = yearGroupMembershipDialog.YearGroups;
            Assert.AreEqual(true, yearGroups.Rows.Any(x => x.YearGroup.Contains(yearGroup)), "Year group of pupil in search results is not correct");
            yearGroupMembershipDialog.ClickOK();

            SeleniumHelper.CloseTab("Pupil Record");

            //Check the middle item
            pupilLogTrilet = new PupilLogTriplet();
            pupilTiles = pupilLogTrilet.SearchCriteria.Search();
            pupilLogDetails = pupilTiles[Convert.ToInt16(pupilTiles.Count() / 2)].Click<PupilLogDetailPage>();
            pupilDetails = SeleniumHelper.NavigateViaAction<PupilRecordPage>("Pupil Record");
            pupilDetails.SelectRegistrationTab();

            classMembershipDialog = pupilDetails.ViewClassHistory();
            classes = classMembershipDialog.Classes;
            Assert.AreEqual(true, classes.Rows.Any(x => x.Class.Contains(className)), "Class of pupil in search results is not correct");
            classMembershipDialog.ClickOK();

            yearGroupMembershipDialog = pupilDetails.ViewYearGroupHistory();
            yearGroups = yearGroupMembershipDialog.YearGroups;
            Assert.AreEqual(true, yearGroups.Rows.Any(x => x.YearGroup.Contains(yearGroup)), "Year group of pupil in search results is not correct");
            yearGroupMembershipDialog.ClickOK();

            SeleniumHelper.CloseTab("Pupil Record");

            //Check the last item
            pupilLogTrilet = new PupilLogTriplet();
            pupilTiles = pupilLogTrilet.SearchCriteria.Search();
            pupilLogDetails = pupilTiles[pupilTiles.Count() - 1].Click<PupilLogDetailPage>();
            pupilDetails = SeleniumHelper.NavigateViaAction<PupilRecordPage>("Pupil Record");
            pupilDetails.SelectRegistrationTab();

            classMembershipDialog = pupilDetails.ViewClassHistory();
            classes = classMembershipDialog.Classes;
            Assert.AreEqual(true, classes.Rows.Any(x => x.Class.Contains(className)), "Class of pupil in search results is not correct");
            classMembershipDialog.ClickOK();

            yearGroupMembershipDialog = pupilDetails.ViewYearGroupHistory();
            yearGroups = yearGroupMembershipDialog.YearGroups;
            Assert.AreEqual(true, yearGroups.Rows.Any(x => x.YearGroup.Contains(yearGroup)), "Year group of pupil in search results is not correct");
            yearGroupMembershipDialog.ClickOK();

            SeleniumHelper.CloseTab("Pupil Record");

            //Select the 4th pupil from the top in the results list
            pupilLogTrilet = new PupilLogTriplet();
            pupilTiles = pupilLogTrilet.SearchCriteria.Search();
            var pupilTile = pupilTiles[3];
            pupilLogDetails = pupilTile.Click<PupilLogDetailPage>();

            //Check latest assessment results are displayed in the 'Latest Assessment Result's panel
            Assert.AreEqual(true, pupilLogDetails.IsAssessmentResultsDisplayed(), "Assessment results are not displayed");

        }

        /// <summary>
        /// Author: Ba.Truong
        /// Description: PU91: Check exercise ability to view a Pupil's log when the current pupil has 'SEN' details recorded.
        /// </summary>
        //[WebDriverTest(TimeoutSeconds = 1500, Enabled = true, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { PupilTestGroups.PupilLog.PupilLogTests, PupilTestGroups.Priority.Priority3 })]
        public void TC_PU091_Exercise_Ability_View_A_Pupil_Log_When_The_Current_Pupil_Has_SEN_Details_Recorded(string[] topSENDetails, string[] bottomSENDetails)
        {
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);
            SeleniumHelper.NavigateQuickLink("Pupil Log");

            //Search pupils with SEN value is true
            var pupilLogTrilet = new PupilLogTriplet();
            pupilLogTrilet.SearchCriteria.PupilName = topSENDetails[0];
            pupilLogTrilet.SearchCriteria.ClickSearchAdvanced(true);
            pupilLogTrilet.SearchCriteria.IsSen = true;
            var pupilTiles = pupilLogTrilet.SearchCriteria.Search();

            //Select a pupil 
            var pupilTile = pupilTiles.FirstOrDefault(x => x.Name.Equals(topSENDetails[0]));
            var pupilDetails = pupilTile.Click<PupilLogDetailPage>();

            //Confirm 'SEN' details display in SEN panel
            Assert.AreEqual(false, string.IsNullOrEmpty(pupilDetails.SendStage), "SEN State on SEN panel is empty");
            Assert.AreEqual(topSENDetails[1], pupilDetails.SendStage, "SEN State value on SEN panel is not correct");


            //Click 'SEN Record' to open SEN details
            var senDetails = pupilDetails.ClickSenRecord();

            //Confirm the 'SEN Record' screen displays this pupil's SEN information
            //Verify SEN Stages
            var senStage = senDetails.SenStages.Rows.FirstOrDefault(x => x.Stage.Equals(topSENDetails[1]));
            Assert.AreNotEqual(null, senStage, "SEN Stage on SEN Record page is empty ");

            //Return HOME page to re-verify with pupil at bottom of the results list
            SeleniumHelper.CloseTab("SEN Record");
            SeleniumHelper.CloseTab("Pupil Log");

            //Re-navigate to 'Pupil Log' to re-do with pupil at bottom of the results list
            SeleniumHelper.NavigateQuickLink("Pupil Log");
            SeleniumHelper.NavigateQuickLink("Pupil Log");

            //Search pupils with SEN value is true
            pupilLogTrilet = new PupilLogTriplet();
            pupilLogTrilet.SearchCriteria.PupilName = bottomSENDetails[0];
            pupilLogTrilet.SearchCriteria.ClickSearchAdvanced(true);
            pupilLogTrilet.SearchCriteria.IsSen = true;
            pupilTiles = pupilLogTrilet.SearchCriteria.Search();

            //Select pupil at the bottom of the results list
            pupilTile = pupilTiles.FirstOrDefault(x => x.Name.Equals(bottomSENDetails[0]));
            pupilDetails = pupilTile.Click<PupilLogDetailPage>();

            //Confirm 'SEN' details display in SEN panel
            Assert.AreEqual(false, string.IsNullOrEmpty(pupilDetails.SendStage), "SEN Stage on SEN panel is empty");
            Assert.AreEqual(bottomSENDetails[1], pupilDetails.SendStage, "SEN Stage value on SEN panel is not correct");

            //Click 'SEN Record' to open SEN details
            senDetails = pupilDetails.ClickSenRecord();

            //Confirm the 'SEN Record' screen displays this pupil's SEN information
            //Verify SEN Stages
            senStage = senDetails.SenStages.Rows.FirstOrDefault();
            Assert.AreEqual(false, string.IsNullOrEmpty(senStage.Stage), "SEN State on SEN Record page is empty ");
            Assert.AreEqual(false, string.IsNullOrEmpty(senStage.StartDay), "SEN State on SEN Record page is empty ");

            //Verify SEN Needs
            senStage = senDetails.SenStages.Rows.FirstOrDefault(x => x.Stage.Equals(bottomSENDetails[1]));
            Assert.AreNotEqual(null, senStage, "SEN Stage on SEN Record page is empty ");

        }
        
        #endregion

        #region PupilRecord


        /// <summary>
        /// Author: Y.Ta
        /// Descriptions: Verify Add 'Meal' data  into pupil sucessfully
        /// </summary>
        /// 
        //[WebDriverTest(TimeoutSeconds = 2000, Enabled = true, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { PupilTestGroups.PupilRecord.Page, PupilTestGroups.Priority.Priority3 }, DataProvider = "TC_PU07_Data")]
        public void TC_PU007_Verify_Add_Meal_Section(string[] pupilInfo, string[] EligibleFreeMeal, string[] PatternMeal)
        {
            #region Pre-Condition: Create new pupil
            // Login as school admin
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);

            // Prepare to delete: Remove the Note if it exist

            // Navigate to Pupil Record
            SeleniumHelper.NavigateMenu("Tasks", "Pupils", "Pupil Records");
            var pupilRecordTriplet = new PupilRecordTriplet();

            pupilRecordTriplet.SearchCriteria.PupilName = String.Format("{0}, {1}", pupilInfo[2], pupilInfo[0]);
            var resultPupils = pupilRecordTriplet.SearchCriteria.Search();
            var pupilSearchTile = resultPupils.SingleOrDefault(t => t.Name.Equals(String.Format("{0}, {1}", pupilInfo[2], pupilInfo[0])));

            var pupilRecord = pupilSearchTile == null ? null : pupilSearchTile.Click<PupilRecordPage>();
            var temp = pupilRecord == null ? new PupilRecordPage() : pupilRecord.RemoveNoteDocument();

            // Pre-condition: delete the pupil if it exist before
            // Navigate to Pupil Record
            SeleniumHelper.NavigateMenu("Tasks", "Pupils", "Delete Pupil");

            // Open Specific pupil
            var deletePupilRecordTriplet = new DeletePupilRecordTriplet();
            deletePupilRecordTriplet.SearchCriteria.PupilName = String.Format("{0}, {1}", pupilInfo[2], pupilInfo[0]);
            resultPupils = deletePupilRecordTriplet.SearchCriteria.Search();

            pupilSearchTile = resultPupils.SingleOrDefault(t => t.Name.Equals(String.Format("{0}, {1}", pupilInfo[0], pupilInfo[2])));
            var deletePupilRecord = pupilSearchTile == null ? new DeletePupilRecordPage() : pupilSearchTile.Click<DeletePupilRecordPage>();

            deletePupilRecord.Delete();

            // Navigate to Pupil Record
            SeleniumHelper.NavigateMenu("Tasks", "Pupils", "Pupil Records");

            pupilRecordTriplet = new PupilRecordTriplet();
            var addNewPupilDialog = pupilRecordTriplet.AddNewPupil();

            addNewPupilDialog.Forename = pupilInfo[0];
            addNewPupilDialog.MiddleName = pupilInfo[1];
            addNewPupilDialog.SurName = pupilInfo[2];
            addNewPupilDialog.Gender = pupilInfo[3];
            addNewPupilDialog.DateOfBirth = pupilInfo[4];

            var registrationDetailDialog = addNewPupilDialog.Continue();
            registrationDetailDialog.DateOfAdmission = pupilInfo[5];
            registrationDetailDialog.YearGroup = pupilInfo[6];


            registrationDetailDialog.CreateRecord();

            var confirmRequiredDialog = new ConfirmRequiredDialog();
            confirmRequiredDialog.ClickOk();

            pupilRecord = PupilRecordPage.Create();
            pupilRecord.SavePupil();
            #endregion


            // Navigate to Pupil Record
            SeleniumHelper.NavigateMenu("Tasks", "Pupils", "Pupil Records");
            pupilRecordTriplet = new PupilRecordTriplet();

            // Open specific pupil record
            pupilRecordTriplet.SearchCriteria.PupilName = String.Format("{0}, {1}", pupilInfo[2], pupilInfo[0]);
            resultPupils = pupilRecordTriplet.SearchCriteria.Search();
            pupilSearchTile = resultPupils.SingleOrDefault(t => t.Name.Equals(String.Format("{0}, {1}", pupilInfo[2], pupilInfo[0])));

            pupilRecord = pupilSearchTile == null ? null : pupilSearchTile.Click<PupilRecordPage>();
            Assert.AreNotEqual(null, pupilRecord, "Does not found pupil");

            // Open Meals section
            pupilRecord.SelectMealsTab();

            // Enter first column for Eligible Free Meal table
            pupilRecord.ClickAddFreeSchoolMealEligibility();
            pupilRecord.EligibleFreeMeal[0].StartDate = EligibleFreeMeal[0];
            pupilRecord.EligibleFreeMeal[0].EndDate = EligibleFreeMeal[1];
            pupilRecord.EligibleFreeMeal[0].Note = EligibleFreeMeal[4];

            // Save Pupil
            pupilRecord = PupilRecordPage.Create();
            pupilRecord.SavePupil();

            // Enter first column for Meal Pattern Table and save it
            pupilRecord.ClickAddMealPattern();
            pupilRecord.MealPattern[0].StartDate = PatternMeal[0];
            pupilRecord.MealPattern[0].EndDate = PatternMeal[1];
            pupilRecord = PupilRecordPage.Create();
            pupilRecord.SavePupil();

            //add one more record in each of the 'Eligible for Free School Meals' and 'Meal Pattern' grids
            pupilRecord.ClickAddFreeSchoolMealEligibility();
            pupilRecord.EligibleFreeMeal[1].StartDate = EligibleFreeMeal[2];
            pupilRecord.EligibleFreeMeal[1].EndDate = EligibleFreeMeal[3];
            pupilRecord.EligibleFreeMeal[1].Note = EligibleFreeMeal[4];

            pupilRecord.ClickAddMealPattern();
            pupilRecord.MealPattern[1].StartDate = PatternMeal[2];
            pupilRecord.MealPattern[1].EndDate = PatternMeal[3];
            pupilRecord = PupilRecordPage.Create();
            pupilRecord.SavePupil();

            // Verify data is saved Success
            Assert.AreEqual(true, pupilRecord.IsSuccessMessageDisplayed(), "Success message is not display");
            //Pupil now has two recorded entries in each of the 'Eligible for Free School Meals' and 'Meal Pattern' grids.
            Assert.AreEqual(2, pupilRecord.EligibleFreeMeal.Rows.Count() - 1);
            Assert.AreEqual(2, pupilRecord.MealPattern.Rows.Count() - 1);

            //Verify there is one past dated record in table Eligible Free Meal and Meal Pattern

            var rowEligibleFreeMeal = pupilRecord.EligibleFreeMeal.Rows.SingleOrDefault(p => p.EndDate.Equals(EligibleFreeMeal[1]));
            Assert.AreEqual(EligibleFreeMeal[1], rowEligibleFreeMeal.EndDate, "There is not one past dated record in EligibleFreeMeal");
            var rowMealPattern = pupilRecord.MealPattern.Rows.SingleOrDefault(p => p.EndDate.Equals(PatternMeal[1]));
            Assert.AreEqual(PatternMeal[1], rowMealPattern.EndDate, "There is not one past dated record in PatternMeal");

            rowEligibleFreeMeal = pupilRecord.EligibleFreeMeal.Rows.SingleOrDefault(p => p.EndDate.Equals(EligibleFreeMeal[3]));
            Assert.AreEqual(EligibleFreeMeal[3], rowEligibleFreeMeal.EndDate, "There is not one current record in EligibleFreeMeal");
            rowMealPattern = pupilRecord.MealPattern.Rows.SingleOrDefault(p => p.EndDate.Equals(PatternMeal[3]));
            Assert.AreEqual(PatternMeal[3], rowMealPattern.EndDate, "There is not one current record in PatternMeal");

            //Verify The 'Eligible for Free School Meals' records have notes
            Assert.AreEqual(EligibleFreeMeal[4], pupilRecord.EligibleFreeMeal[0].Note, "The note is incorrect");
            Assert.AreEqual(EligibleFreeMeal[4], pupilRecord.EligibleFreeMeal[1].Note, "The note is incorrect");

            #region End Condition :Delete pupil
            // Prepare to delete: Remove the Note if it exist

            // Navigate to Pupil Record
            SeleniumHelper.NavigateMenu("Tasks", "Pupils", "Pupil Records");
            pupilRecordTriplet = new PupilRecordTriplet();

            pupilRecordTriplet.SearchCriteria.PupilName = String.Format("{0}, {1}", pupilInfo[2], pupilInfo[0]);
            resultPupils = pupilRecordTriplet.SearchCriteria.Search();
            pupilSearchTile = resultPupils.SingleOrDefault(t => t.Name.Equals(String.Format("{0}, {1}", pupilInfo[2], pupilInfo[0])));

            pupilRecord = pupilSearchTile == null ? null : pupilSearchTile.Click<PupilRecordPage>();
            temp = pupilRecord == null ? new PupilRecordPage() : pupilRecord.RemoveNoteDocument();

            // Pre-condition: delete the pupil if it exist before
            // Navigate to Pupil Record
            SeleniumHelper.NavigateMenu("Tasks", "Pupils", "Delete Pupil");

            // Open Specific pupil
            deletePupilRecordTriplet = new DeletePupilRecordTriplet();
            deletePupilRecordTriplet.SearchCriteria.PupilName = String.Format("{0}, {1}", pupilInfo[2], pupilInfo[0]);
            resultPupils = deletePupilRecordTriplet.SearchCriteria.Search();

            pupilSearchTile = resultPupils.SingleOrDefault(t => t.Name.Equals(String.Format("{0}, {1}", pupilInfo[2], pupilInfo[0])));
            deletePupilRecord = pupilSearchTile == null ? new DeletePupilRecordPage() : pupilSearchTile.Click<DeletePupilRecordPage>();

            deletePupilRecord.Delete();
            #endregion
        }

        /// <summary>
        ///  Author: Y.Ta
        ///  Description: Verify add medical information sucessfully
        ///  SERIAL ONLY
        /// </summary>
        //[WebDriverTest(TimeoutSeconds = 2000, Enabled = false, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { PupilTestGroups.PupilRecord.Page, PupilTestGroups.Priority.Priority3 }, DataProvider = "TC_PU08_Data")]
        public void TC_PU008_Verify_Add_Medical_Section(string[] pupilInfo, string NSHNumber, string[] medicalPractice, string SummrayNote, string MedicalDescription, string[] MedicalEvent)
        {
            #region Pre-Condition: Create new pupil
            // Login as school admin
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);

            // Prepare to delete: Remove the Note if it exist

            // Navigate to Pupil Record
            SeleniumHelper.NavigateMenu("Tasks", "Pupils", "Pupil Records");
            var pupilRecordTriplet = new PupilRecordTriplet();

            pupilRecordTriplet.SearchCriteria.PupilName = String.Format("{0}, {1}", pupilInfo[2], pupilInfo[0]);
            var resultPupils = pupilRecordTriplet.SearchCriteria.Search();
            var pupilSearchTile = resultPupils.SingleOrDefault(t => t.Name.Equals(String.Format("{0}, {1}", pupilInfo[2], pupilInfo[0])));

            var pupilRecord = pupilSearchTile == null ? null : pupilSearchTile.Click<PupilRecordPage>();
            var temp = pupilRecord == null ? new PupilRecordPage() : pupilRecord.RemoveNoteDocument();

            // Pre-condition: delete the pupil if it exist before
            // Navigate to Pupil Record
            SeleniumHelper.NavigateMenu("Tasks", "Pupils", "Delete Pupil");

            // Open Specific pupil
            var deletePupilRecordTriplet = new DeletePupilRecordTriplet();
            deletePupilRecordTriplet.SearchCriteria.PupilName = String.Format("{0}, {1}", pupilInfo[2], pupilInfo[0]);
            resultPupils = deletePupilRecordTriplet.SearchCriteria.Search();

            pupilSearchTile = resultPupils.SingleOrDefault(t => t.Name.Equals(String.Format("{0}, {1}", pupilInfo[0], pupilInfo[2])));
            var deletePupilRecord = pupilSearchTile == null ? new DeletePupilRecordPage() : pupilSearchTile.Click<DeletePupilRecordPage>();

            deletePupilRecord.Delete();

            // Navigate to Pupil Record
            SeleniumHelper.NavigateMenu("Tasks", "Pupils", "Pupil Records");

            pupilRecordTriplet = new PupilRecordTriplet();
            var addNewPupilDialog = pupilRecordTriplet.AddNewPupil();

            addNewPupilDialog.Forename = pupilInfo[0];
            addNewPupilDialog.MiddleName = pupilInfo[1];
            addNewPupilDialog.SurName = pupilInfo[2];
            addNewPupilDialog.Gender = pupilInfo[3];
            addNewPupilDialog.DateOfBirth = pupilInfo[4];

            var registrationDetailDialog = addNewPupilDialog.Continue();
            registrationDetailDialog.DateOfAdmission = pupilInfo[5];
            registrationDetailDialog.YearGroup = pupilInfo[6];


            registrationDetailDialog.CreateRecord();

            var confirmRequiredDialog = new ConfirmRequiredDialog();
            confirmRequiredDialog.ClickOk();

            pupilRecord = PupilRecordPage.Create();
            pupilRecord.SavePupil();
            #endregion

            #region Steps

            // Navigate to Pupil Record
            SeleniumHelper.NavigateMenu("Tasks", "Pupils", "Pupil Records");
            pupilRecordTriplet = new PupilRecordTriplet();

            // Open specific pupil record
            pupilRecordTriplet.SearchCriteria.PupilName = String.Format("{0}, {1}", pupilInfo[2], pupilInfo[0]);
            resultPupils = pupilRecordTriplet.SearchCriteria.Search();
            pupilSearchTile = resultPupils.SingleOrDefault(t => t.Name.Equals(String.Format("{0}, {1}", pupilInfo[2], pupilInfo[0])));

            pupilRecord = pupilSearchTile == null ? null : pupilSearchTile.Click<PupilRecordPage>();
            Assert.AreNotEqual(null, pupilRecord, "Does not found pupil");

            // Open Medical section
            pupilRecord.SelectMedicalTab();
            //'Add' an 'NHS Number' and indicate pupil is 'Disabled'.
            pupilRecord.NHSNumber = NSHNumber;
            pupilRecord.Assessed = true;

            //Allocate  pupil a 'Medical Practice' and 'Doctor', via  medical practice search and selection
            var medicalTripletDialog = pupilRecord.ClickMedicalPractice();

            medicalTripletDialog.SearchCriteria.MedicalPracticeName = medicalPractice[0];

            var medicalResults = medicalTripletDialog.SearchCriteria.Search();
            medicalTripletDialog = medicalResults[0] == null ? null : medicalResults[0].Click<MedicalPracticeTripletDialog>();
            medicalTripletDialog.ClickOk();
            pupilRecord.SavePupil();

            pupilRecord.MedicalPractice[0].Doctor = medicalPractice[1];

            //Select from 'Dietary Needs' two specific entries, namely 'Seafood allergy' and 'Vegetarian'.
            pupilRecord.SeafoodAllergy = true;
            pupilRecord.Vegetarian = true;

            //Record some text in 'Medical Notes/Documents' fields:
            // Add document to Medical Notes.
            //TODO: Note grid not yet ready
            //pupilRecord.MedicalNote[0].Summary = SummrayNote; 
            pupilRecord = PupilRecordPage.Create();
            pupilRecord.SavePupil();
            pupilRecord = new PupilRecordPage();

            //var medicalRow = pupilRecord.MedicalNote.LastInsertRow;
            //medicalRow.AddDocument();
            ViewDocumentDialog viewDocument = new ViewDocumentDialog();
            AddAttachmentDialog addAttchmentDialog = viewDocument.ClickAddAttachment();
            //addAttchmentDialog.BrowserToDocument();
            //viewDocument = addAttchmentDialog.UploadDocument();
            //viewDocument.ClickOk();

            //In the 'Medical Conditions' and 'Medical Events' grids, record a single record
            //upload a 'Document' for each record added in each of these two grids, and then 'Save'.
            pupilRecord.ClickAddMedicalCondition();
            pupilRecord.MedicalCondition[0].Description = "None";
            pupilRecord = PupilRecordPage.Create();
            pupilRecord.SavePupil();

            pupilRecord = new PupilRecordPage();
            var medicalConditionRow = pupilRecord.MedicalCondition.LastInsertRow;
            medicalConditionRow.AddDocument();
            viewDocument = new ViewDocumentDialog();
            addAttchmentDialog = viewDocument.ClickAddAttachment();
            addAttchmentDialog.BrowserToDocument();
            viewDocument = addAttchmentDialog.UploadDocument();
            viewDocument.ClickOk();

            pupilRecord.ClickAddMedicalEvent();
            pupilRecord.MedicalEvent[0].Type = MedicalEvent[0];
            pupilRecord.MedicalEvent[0].Description = MedicalEvent[1];
            pupilRecord.MedicalEvent[0].EventDate = MedicalEvent[2];

            pupilRecord = PupilRecordPage.Create();
            pupilRecord.SavePupil();

            pupilRecord = new PupilRecordPage();
            var medicalEventRow = pupilRecord.MedicalEvent.LastInsertRow;
            medicalEventRow.AddDocument();
            viewDocument = new ViewDocumentDialog();
            addAttchmentDialog = viewDocument.ClickAddAttachment();
            addAttchmentDialog.BrowserToDocument();
            viewDocument = addAttchmentDialog.UploadDocument();
            viewDocument.ClickOk();

            pupilRecord = PupilRecordPage.Create();
            pupilRecord.SavePupil();

            // Verify data is saved Success
            Assert.AreEqual(true, pupilRecord.IsSuccessMessageDisplayed(), "Success message is not display");
            Assert.AreEqual(NSHNumber, pupilRecord.NHSNumber, "The NHS Number is not equal");
            Assert.AreEqual(true, pupilRecord.Assessed, "The assesed is not set");

            Assert.AreEqual(true, pupilRecord.SeafoodAllergy, "The SeafoodAllergy is not set");
            Assert.AreEqual(true, pupilRecord.Vegetarian, "The Vegetarian is not set");

            //Assert.AreEqual(SummrayNote, pupilRecord.MedicalNote[0].Summary, "The Summary field is not equal"); TODO:
            Assert.AreEqual(MedicalDescription, pupilRecord.MedicalCondition[0].Description, "The Description field is not equal");
            Assert.AreEqual(MedicalEvent[0], pupilRecord.MedicalEvent[0].Type, "The Type field is not equal");

            #endregion

            #region End Condition :Delete pupil
            // Prepare to delete: Remove the Note if it exist

            // Navigate to Pupil Record
            SeleniumHelper.NavigateMenu("Tasks", "Pupils", "Pupil Records");
            pupilRecordTriplet = new PupilRecordTriplet();

            pupilRecordTriplet.SearchCriteria.PupilName = String.Format("{0}, {1}", pupilInfo[2], pupilInfo[0]);
            resultPupils = pupilRecordTriplet.SearchCriteria.Search();
            pupilSearchTile = resultPupils.SingleOrDefault(t => t.Name.Equals(String.Format("{0}, {1}", pupilInfo[2], pupilInfo[0])));

            pupilRecord = pupilSearchTile == null ? null : pupilSearchTile.Click<PupilRecordPage>();
            temp = pupilRecord == null ? new PupilRecordPage() : pupilRecord.RemoveNoteDocument();

            // Pre-condition: delete the pupil if it exist before
            // Navigate to Pupil Record
            SeleniumHelper.NavigateMenu("Tasks", "Pupils", "Delete Pupil");

            // Open Specific pupil
            deletePupilRecordTriplet = new DeletePupilRecordTriplet();
            deletePupilRecordTriplet.SearchCriteria.PupilName = String.Format("{0}, {1}", pupilInfo[2], pupilInfo[0]);
            resultPupils = deletePupilRecordTriplet.SearchCriteria.Search();

            pupilSearchTile = resultPupils.SingleOrDefault(t => t.Name.Equals(String.Format("{0}, {1}", pupilInfo[2], pupilInfo[0])));
            deletePupilRecord = pupilSearchTile == null ? new DeletePupilRecordPage() : pupilSearchTile.Click<DeletePupilRecordPage>();

            deletePupilRecord.Delete();
            #endregion
        }

        /// <summary>
        ///  Author: Y.Ta
        ///  Description: Verify add ethnic culture information sucessfully
        /// </summary>
        ///
        //[WebDriverTest(TimeoutSeconds = 2000, Enabled = true, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { PupilTestGroups.PupilRecord.Page, PupilTestGroups.Priority.Priority3 }, DataProvider = "TC_PU09_Data")]
        public void TC_PU009_Verify_Add_Ethnic_Culture_Section(string[] pupilInfo, string Ethnicity, string HomeLanguage, string religion, string AccommodationType, string AsylumStatus, string[] NewcomerPeriods, string AccomodationStartDate)
        {
            #region Pre-Condition: Create new pupil
            // Login as school admin
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);

            // Prepare to delete: Remove the Note if it exist

            // Navigate to Pupil Record
            SeleniumHelper.NavigateMenu("Tasks", "Pupils", "Pupil Records");
            var pupilRecordTriplet = new PupilRecordTriplet();

            pupilRecordTriplet.SearchCriteria.PupilName = String.Format("{0}, {1}", pupilInfo[2], pupilInfo[0]);
            var resultPupils = pupilRecordTriplet.SearchCriteria.Search();
            var pupilSearchTile = resultPupils.SingleOrDefault(t => t.Name.Equals(String.Format("{0}, {1}", pupilInfo[2], pupilInfo[0])));

            var pupilRecord = pupilSearchTile == null ? null : pupilSearchTile.Click<PupilRecordPage>();
            var temp = pupilRecord == null ? new PupilRecordPage() : pupilRecord.RemoveNoteDocument();

            // Pre-condition: delete the pupil if it exist before
            // Navigate to Pupil Record
            SeleniumHelper.NavigateMenu("Tasks", "Pupils", "Delete Pupil");

            // Open Specific pupil
            var deletePupilRecordTriplet = new DeletePupilRecordTriplet();
            deletePupilRecordTriplet.SearchCriteria.PupilName = String.Format("{0}, {1}", pupilInfo[2], pupilInfo[0]);
            resultPupils = deletePupilRecordTriplet.SearchCriteria.Search();

            pupilSearchTile = resultPupils.SingleOrDefault(t => t.Name.Equals(String.Format("{0}, {1}", pupilInfo[0], pupilInfo[2])));
            var deletePupilRecord = pupilSearchTile == null ? new DeletePupilRecordPage() : pupilSearchTile.Click<DeletePupilRecordPage>();

            deletePupilRecord.Delete();

            // Navigate to Pupil Record
            SeleniumHelper.NavigateMenu("Tasks", "Pupils", "Pupil Records");

            pupilRecordTriplet = new PupilRecordTriplet();
            var addNewPupilDialog = pupilRecordTriplet.AddNewPupil();

            addNewPupilDialog.Forename = pupilInfo[0];
            addNewPupilDialog.MiddleName = pupilInfo[1];
            addNewPupilDialog.SurName = pupilInfo[2];
            addNewPupilDialog.Gender = pupilInfo[3];
            addNewPupilDialog.DateOfBirth = pupilInfo[4];

            var registrationDetailDialog = addNewPupilDialog.Continue();
            registrationDetailDialog.DateOfAdmission = pupilInfo[5];
            registrationDetailDialog.YearGroup = pupilInfo[6];


            registrationDetailDialog.CreateRecord();

            var confirmRequiredDialog = new ConfirmRequiredDialog();
            confirmRequiredDialog.ClickOk();

            pupilRecord = PupilRecordPage.Create();
            pupilRecord.SavePupil();
            #endregion


            // Navigate to Pupil Record
            SeleniumHelper.NavigateMenu("Tasks", "Pupils", "Pupil Records");
            pupilRecordTriplet = new PupilRecordTriplet();

            // Open specific pupil record
            pupilRecordTriplet.SearchCriteria.PupilName = String.Format("{0}, {1}", pupilInfo[2], pupilInfo[0]);
            resultPupils = pupilRecordTriplet.SearchCriteria.Search();
            pupilSearchTile = resultPupils.SingleOrDefault(t => t.Name.Equals(String.Format("{0}, {1}", pupilInfo[2], pupilInfo[0])));

            pupilRecord = pupilSearchTile == null ? null : pupilSearchTile.Click<PupilRecordPage>();
            Assert.AreNotEqual(null, pupilRecord, "Does not found pupil");

            // Open Medical section
            pupilRecord.SelectEthnicCulturalTab();

            pupilRecord.Ethnicity = Ethnicity;
            pupilRecord.HomeLanguage = HomeLanguage;
            pupilRecord.Religion = religion;

            var accommodationDialog = pupilRecord.CreateAccommodationType();
            accommodationDialog.AddNewAccomodationType();
            accommodationDialog.Accommodations[0].AccommodationType = AccommodationType;
            accommodationDialog.Accommodations[0].StartDate = AccomodationStartDate;
            accommodationDialog.ClickOk();

            pupilRecord.AsylumStatus = AsylumStatus;
            pupilRecord.IsTaughtMedium = true;

            pupilRecord.LearnerNewcomerPeriods[0].StartDate = NewcomerPeriods[0];
            pupilRecord.LearnerNewcomerPeriods[0].EndDate = NewcomerPeriods[1];

            pupilRecord = PupilRecordPage.Create();
            pupilRecord.SavePupil();
            // Verify data is saved Success
            Assert.AreEqual(true, pupilRecord.IsSuccessMessageDisplayed(), "Success message is not display");

            //Pupil now has a recorded:Ethnicity.- Home Language.- Religion.
            Assert.AreEqual(Ethnicity, pupilRecord.Ethnicity, "The Ethnicity value is not correct");
            Assert.AreEqual(HomeLanguage, pupilRecord.HomeLanguage, "The HomeLanguage value is not correct");
            Assert.AreEqual(religion, pupilRecord.Religion, "The Religion value is not correct");

            Assert.AreEqual(true, pupilRecord.IsTaughtMedium, "The value of Is Taught Medium check box is not correct");

            accommodationDialog = pupilRecord.CreateAccommodationType();
            Assert.AreEqual(AccommodationType, accommodationDialog.Accommodations[0].AccommodationType, "The AccommodationType value is not correct");
            accommodationDialog.ClickOk();

            Assert.AreEqual(AsylumStatus, pupilRecord.AsylumStatus, "The AccommodationType value is not correct");

            var row = pupilRecord.LearnerNewcomerPeriods.Rows.SingleOrDefault(p => p.StartDate.Equals(NewcomerPeriods[0]));
            Assert.AreEqual(NewcomerPeriods[1], row.EndDate, "The New Commer Periods is displayed correctly");

            #region End Condition :Delete pupil
            // Prepare to delete: Remove the Note if it exist

            // Navigate to Pupil Record
            SeleniumHelper.NavigateMenu("Tasks", "Pupils", "Pupil Records");
            pupilRecordTriplet = new PupilRecordTriplet();

            pupilRecordTriplet.SearchCriteria.PupilName = String.Format("{0}, {1}", pupilInfo[2], pupilInfo[0]);
            resultPupils = pupilRecordTriplet.SearchCriteria.Search();
            pupilSearchTile = resultPupils.SingleOrDefault(t => t.Name.Equals(String.Format("{0}, {1}", pupilInfo[2], pupilInfo[0])));

            pupilRecord = pupilSearchTile == null ? null : pupilSearchTile.Click<PupilRecordPage>();
            temp = pupilRecord == null ? new PupilRecordPage() : pupilRecord.RemoveNoteDocument();

            // Pre-condition: delete the pupil if it exist before
            // Navigate to Pupil Record
            SeleniumHelper.NavigateMenu("Tasks", "Pupils", "Delete Pupil");

            // Open Specific pupil
            deletePupilRecordTriplet = new DeletePupilRecordTriplet();
            deletePupilRecordTriplet.SearchCriteria.PupilName = String.Format("{0}, {1}", pupilInfo[2], pupilInfo[0]);
            resultPupils = deletePupilRecordTriplet.SearchCriteria.Search();

            pupilSearchTile = resultPupils.SingleOrDefault(t => t.Name.Equals(String.Format("{0}, {1}", pupilInfo[2], pupilInfo[0])));
            deletePupilRecord = pupilSearchTile == null ? new DeletePupilRecordPage() : pupilSearchTile.Click<DeletePupilRecordPage>();

            deletePupilRecord.Delete();
            #endregion
        }

        /// <summary>
        ///  Author: Y.Ta
        ///  Description: Verify add Additional information sucessfully
        /// </summary>
        ///
        //[WebDriverTest(TimeoutSeconds = 2000, Enabled = true, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { "SerialTC10", PupilTestGroups.Priority.Priority3 }, DataProvider = "TC_PU10_Data")]
        public void TC_PU010_Verify_Add_Addtional_Section(string[] pupilInfo, string ServiceChildren, string ServiceChildrenSource, string ModeTravel, string TravelRoute, string AcademicYear, string[] LearnerUniformGrantEligibilities)
        {

            #region Pre-Condition: Create new pupil
            // Login as school admin
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);

            // Prepare to delete: Remove the Note if it exist

            // Navigate to Pupil Record
            SeleniumHelper.NavigateMenu("Tasks", "Pupils", "Pupil Records");
            var pupilRecordTriplet = new PupilRecordTriplet();

            pupilRecordTriplet.SearchCriteria.PupilName = String.Format("{0}, {1}", pupilInfo[2], pupilInfo[0]);
            var resultPupils = pupilRecordTriplet.SearchCriteria.Search();
            var pupilSearchTile = resultPupils.SingleOrDefault(t => t.Name.Equals(String.Format("{0}, {1}", pupilInfo[2], pupilInfo[0])));

            var pupilRecord = pupilSearchTile == null ? null : pupilSearchTile.Click<PupilRecordPage>();
            var temp = pupilRecord == null ? new PupilRecordPage() : pupilRecord.RemoveNoteDocument();

            // Pre-condition: delete the pupil if it exist before
            // Navigate to Pupil Record
            SeleniumHelper.NavigateMenu("Tasks", "Pupils", "Delete Pupil");

            // Open Specific pupil
            var deletePupilRecordTriplet = new DeletePupilRecordTriplet();
            deletePupilRecordTriplet.SearchCriteria.PupilName = String.Format("{0}, {1}", pupilInfo[2], pupilInfo[0]);
            resultPupils = deletePupilRecordTriplet.SearchCriteria.Search();

            pupilSearchTile = resultPupils.SingleOrDefault(t => t.Name.Equals(String.Format("{0}, {1}", pupilInfo[0], pupilInfo[2])));
            var deletePupilRecord = pupilSearchTile == null ? new DeletePupilRecordPage() : pupilSearchTile.Click<DeletePupilRecordPage>();

            deletePupilRecord.Delete();

            // Navigate to Pupil Record
            SeleniumHelper.NavigateMenu("Tasks", "Pupils", "Pupil Records");

            pupilRecordTriplet = new PupilRecordTriplet();
            var addNewPupilDialog = pupilRecordTriplet.AddNewPupil();

            addNewPupilDialog.Forename = pupilInfo[0];
            addNewPupilDialog.MiddleName = pupilInfo[1];
            addNewPupilDialog.SurName = pupilInfo[2];
            addNewPupilDialog.Gender = pupilInfo[3];
            addNewPupilDialog.DateOfBirth = pupilInfo[4];

            var registrationDetailDialog = addNewPupilDialog.Continue();
            registrationDetailDialog.DateOfAdmission = pupilInfo[5];
            registrationDetailDialog.YearGroup = pupilInfo[6];


            registrationDetailDialog.CreateRecord();

            var confirmRequiredDialog = new ConfirmRequiredDialog();
            confirmRequiredDialog.ClickOk();

            pupilRecord = PupilRecordPage.Create();
            pupilRecord.SavePupil();
            #endregion
            // Navigate to Pupil Record
            SeleniumHelper.NavigateMenu("Tasks", "Pupils", "Pupil Records");
            pupilRecordTriplet = new PupilRecordTriplet();

            // Open specific pupil record
            pupilRecordTriplet.SearchCriteria.PupilName = String.Format("{0}, {1}", pupilInfo[2], pupilInfo[0]);
            resultPupils = pupilRecordTriplet.SearchCriteria.Search();
            pupilSearchTile = resultPupils.SingleOrDefault(t => t.Name.Equals(String.Format("{0}, {1}", pupilInfo[2], pupilInfo[0])));

            pupilRecord = pupilSearchTile == null ? null : pupilSearchTile.Click<PupilRecordPage>();
            Assert.AreNotEqual(null, pupilRecord, "Does not found pupil");

            // Open Aditional section
            pupilRecord.SelectAdditionalTab();

            pupilRecord.ServiceChildren = ServiceChildren;
            pupilRecord.ServiceChildrenSource = ServiceChildrenSource;
            pupilRecord.ModeOfTravel = ModeTravel;
            //pupilRecord.TravelRoute=TravelRoute;

            pupilRecord.JobSeekerAllowance = true;
            pupilRecord.ELBProvidesTransport = true;

            //pupilRecord.LearnerZeroRatedInformationsTable[0].AcademicYear = AcademicYear; TODO: not sure of this table
            pupilRecord.ClickAddUnfiormGrantEligibility();
            pupilRecord.LearnerUniformGrantEligibilities[0].StartDate = LearnerUniformGrantEligibilities[0];
            pupilRecord.LearnerUniformGrantEligibilities[0].EndDate = LearnerUniformGrantEligibilities[1];

            pupilRecord = PupilRecordPage.Create();
            pupilRecord.SavePupil();
            // Verify data is saved Success
            Assert.AreEqual(true, pupilRecord.IsSuccessMessageDisplayed(), "Success message is not display");

            Assert.AreEqual(ServiceChildren, pupilRecord.ServiceChildren, "The service children is not correctly");
            Assert.AreEqual(ServiceChildrenSource, pupilRecord.ServiceChildrenSource, "The service children source is not correctly");
            Assert.AreEqual(ModeTravel, pupilRecord.ModeOfTravel, "The ModeOfTravel field is not correctly");
            Assert.AreEqual(TravelRoute, pupilRecord.TravelRoute, "The TravelRoute field is not correctly");

            Assert.AreEqual(true, pupilRecord.JobSeekerAllowance, "The JobSeekerAllowance check box is unchecked");
            Assert.AreEqual(true, pupilRecord.ELBProvidesTransport, "The TravelRoute check box is unchecked");

            //Assert.AreEqual(AcademicYear, pupilRecord.LearnerZeroRatedInformationsTable[0].AcademicYear, "The AcademicYear cell is not correctly"); TODO: not sure of this table

            Assert.AreEqual(LearnerUniformGrantEligibilities[0], pupilRecord.LearnerUniformGrantEligibilities[0].StartDate, "The Value of Start Date is not correctly");
            Assert.AreEqual(LearnerUniformGrantEligibilities[1], pupilRecord.LearnerUniformGrantEligibilities[0].EndDate, "The Value of Start Date is not correctly");

            #region End Condition :Delete pupil
            // Prepare to delete: Remove the Note if it exist

            // Navigate to Pupil Record
            SeleniumHelper.NavigateMenu("Tasks", "Pupils", "Pupil Records");
            pupilRecordTriplet = new PupilRecordTriplet();

            pupilRecordTriplet.SearchCriteria.PupilName = String.Format("{0}, {1}", pupilInfo[2], pupilInfo[0]);
            resultPupils = pupilRecordTriplet.SearchCriteria.Search();
            pupilSearchTile = resultPupils.SingleOrDefault(t => t.Name.Equals(String.Format("{0}, {1}", pupilInfo[2], pupilInfo[0])));

            pupilRecord = pupilSearchTile == null ? null : pupilSearchTile.Click<PupilRecordPage>();
            temp = pupilRecord == null ? new PupilRecordPage() : pupilRecord.RemoveNoteDocument();

            // Pre-condition: delete the pupil if it exist before
            // Navigate to Pupil Record
            SeleniumHelper.NavigateMenu("Tasks", "Pupils", "Delete Pupil");

            // Open Specific pupil
            deletePupilRecordTriplet = new DeletePupilRecordTriplet();
            deletePupilRecordTriplet.SearchCriteria.PupilName = String.Format("{0}, {1}", pupilInfo[2], pupilInfo[0]);
            resultPupils = deletePupilRecordTriplet.SearchCriteria.Search();

            pupilSearchTile = resultPupils.SingleOrDefault(t => t.Name.Equals(String.Format("{0}, {1}", pupilInfo[2], pupilInfo[0])));
            deletePupilRecord = pupilSearchTile == null ? new DeletePupilRecordPage() : pupilSearchTile.Click<DeletePupilRecordPage>();

            deletePupilRecord.Delete();
            #endregion
        }

        /// <summary>
        ///  Author: Y.Ta
        ///  Description: Verify add welfare information sucessfully
        /// </summary>
        ///
        //[WebDriverTest(TimeoutSeconds = 2000, Enabled = true, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { PupilTestGroups.PupilRecord.Page, PupilTestGroups.Priority.Priority3 }, DataProvider = "TC_PU11_Data")]
        public void TC_PU011_Verify_Add_WelfareSection(string[] pupilInfo, string CareAuthority, string LivingArrangement, string StartDate, string EndDate)
        {

            #region Pre-Condition: Create new pupil
            // Login as school admin
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);

            // Prepare to delete: Remove the Note if it exist

            // Navigate to Pupil Record
            SeleniumHelper.NavigateMenu("Tasks", "Pupils", "Pupil Records");
            var pupilRecordTriplet = new PupilRecordTriplet();

            pupilRecordTriplet.SearchCriteria.PupilName = String.Format("{0}, {1}", pupilInfo[2], pupilInfo[0]);
            var resultPupils = pupilRecordTriplet.SearchCriteria.Search();
            var pupilSearchTile = resultPupils.SingleOrDefault(t => t.Name.Equals(String.Format("{0}, {1}", pupilInfo[2], pupilInfo[0])));

            var pupilRecord = pupilSearchTile == null ? null : pupilSearchTile.Click<PupilRecordPage>();
            var temp = pupilRecord == null ? new PupilRecordPage() : pupilRecord.RemoveNoteDocument();

            // Pre-condition: delete the pupil if it exist before
            // Navigate to Pupil Record
            SeleniumHelper.NavigateMenu("Tasks", "Pupils", "Delete Pupil");

            // Open Specific pupil
            var deletePupilRecordTriplet = new DeletePupilRecordTriplet();
            deletePupilRecordTriplet.SearchCriteria.PupilName = String.Format("{0}, {1}", pupilInfo[2], pupilInfo[0]);
            resultPupils = deletePupilRecordTriplet.SearchCriteria.Search();

            pupilSearchTile = resultPupils.SingleOrDefault(t => t.Name.Equals(String.Format("{0}, {1}", pupilInfo[0], pupilInfo[2])));
            var deletePupilRecord = pupilSearchTile == null ? new DeletePupilRecordPage() : pupilSearchTile.Click<DeletePupilRecordPage>();

            deletePupilRecord.Delete();

            // Navigate to Pupil Record
            SeleniumHelper.NavigateMenu("Tasks", "Pupils", "Pupil Records");

            pupilRecordTriplet = new PupilRecordTriplet();
            var addNewPupilDialog = pupilRecordTriplet.AddNewPupil();

            addNewPupilDialog.Forename = pupilInfo[0];
            addNewPupilDialog.MiddleName = pupilInfo[1];
            addNewPupilDialog.SurName = pupilInfo[2];
            addNewPupilDialog.Gender = pupilInfo[3];
            addNewPupilDialog.DateOfBirth = pupilInfo[4];

            var registrationDetailDialog = addNewPupilDialog.Continue();
            registrationDetailDialog.DateOfAdmission = pupilInfo[5];
            registrationDetailDialog.YearGroup = pupilInfo[6];


            registrationDetailDialog.CreateRecord();

            var confirmRequiredDialog = new ConfirmRequiredDialog();
            confirmRequiredDialog.ClickOk();

            pupilRecord = PupilRecordPage.Create();
            pupilRecord.SavePupil();
            #endregion
            // Navigate to Pupil Record
            SeleniumHelper.NavigateMenu("Tasks", "Pupils", "Pupil Records");
            pupilRecordTriplet = new PupilRecordTriplet();

            // Open specific pupil record
            pupilRecordTriplet.SearchCriteria.PupilName = String.Format("{0}, {1}", pupilInfo[2], pupilInfo[0]);
            resultPupils = pupilRecordTriplet.SearchCriteria.Search();
            pupilSearchTile = resultPupils.SingleOrDefault(t => t.Name.Equals(String.Format("{0}, {1}", pupilInfo[2], pupilInfo[0])));

            pupilRecord = pupilSearchTile == null ? null : pupilSearchTile.Click<PupilRecordPage>();
            Assert.AreNotEqual(null, pupilRecord, "Does not found pupil");

            // Open Aditional section
            pupilRecord.SelectWelfareTab();
            pupilRecord.ClickAddCareArrangment();
            pupilRecord.LearnerInCareDetails[0].CareAuthority = CareAuthority;
            pupilRecord.LearnerInCareDetails[0].LivingArrangement = LivingArrangement;
            pupilRecord.LearnerInCareDetails[0].StartDate = StartDate;
            pupilRecord.LearnerInCareDetails[0].EndDate = EndDate;

            pupilRecord = PupilRecordPage.Create();
            pupilRecord.SavePupil();
            // Verify data is saved Success
            Assert.AreEqual(true, pupilRecord.IsSuccessMessageDisplayed(), "Success message is not display");
            Assert.AreEqual(CareAuthority, pupilRecord.LearnerInCareDetails[0].CareAuthority, "The CareAuthority is not correct");
            Assert.AreEqual(LivingArrangement, pupilRecord.LearnerInCareDetails[0].LivingArrangement, "The LivingArrangement is not correct");
            Assert.AreEqual(StartDate, pupilRecord.LearnerInCareDetails[0].StartDate, "The StartDate is not correct");
            Assert.AreEqual(EndDate, pupilRecord.LearnerInCareDetails[0].EndDate, "The EndDate is not correct");

            #region End Condition :Delete pupil
            // Prepare to delete: Remove the Note if it exist

            // Navigate to Pupil Record
            SeleniumHelper.NavigateMenu("Tasks", "Pupils", "Pupil Records");
            pupilRecordTriplet = new PupilRecordTriplet();

            pupilRecordTriplet.SearchCriteria.PupilName = String.Format("{0}, {1}", pupilInfo[2], pupilInfo[0]);
            resultPupils = pupilRecordTriplet.SearchCriteria.Search();
            pupilSearchTile = resultPupils.SingleOrDefault(t => t.Name.Equals(String.Format("{0}, {1}", pupilInfo[2], pupilInfo[0])));

            pupilRecord = pupilSearchTile == null ? null : pupilSearchTile.Click<PupilRecordPage>();
            temp = pupilRecord == null ? new PupilRecordPage() : pupilRecord.RemoveNoteDocument();

            // Pre-condition: delete the pupil if it exist before
            // Navigate to Pupil Record
            SeleniumHelper.NavigateMenu("Tasks", "Pupils", "Delete Pupil");

            // Open Specific pupil
            deletePupilRecordTriplet = new DeletePupilRecordTriplet();
            deletePupilRecordTriplet.SearchCriteria.PupilName = String.Format("{0}, {1}", pupilInfo[2], pupilInfo[0]);
            resultPupils = deletePupilRecordTriplet.SearchCriteria.Search();

            pupilSearchTile = resultPupils.SingleOrDefault(t => t.Name.Equals(String.Format("{0}, {1}", pupilInfo[2], pupilInfo[0])));
            deletePupilRecord = pupilSearchTile == null ? new DeletePupilRecordPage() : pupilSearchTile.Click<DeletePupilRecordPage>();

            deletePupilRecord.Delete();
            #endregion
        }

        /// <summary>
        ///  Author: Y.Ta
        ///  Description: Verify add welfare education plan information sucessfully
        /// </summary>
        ///
        //[WebDriverTest(TimeoutSeconds = 2000, Enabled = false, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { PupilTestGroups.PupilRecord.Page, PupilTestGroups.Priority.Priority3 }, DataProvider = "TC_PU12_Data")]
        public void TC_PU012_Verify_Add_Welfare_Education_Plan(string[] pupilInfo, string StartDate, string EndDate, string notes, string NextStartDate, string NextEndDate, string CareAuthority, string LivingArrangement)
        {
            #region Pre-Condition: Create new pupil
            // Login as school admin
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);

            // Prepare to delete: Remove the Note if it exist

            // Navigate to Pupil Record
            SeleniumHelper.NavigateMenu("Tasks", "Pupils", "Pupil Records");
            var pupilRecordTriplet = new PupilRecordTriplet();

            pupilRecordTriplet.SearchCriteria.PupilName = String.Format("{0}, {1}", pupilInfo[2], pupilInfo[0]);
            var resultPupils = pupilRecordTriplet.SearchCriteria.Search();
            var pupilSearchTile = resultPupils.SingleOrDefault(t => t.Name.Equals(String.Format("{0}, {1}", pupilInfo[2], pupilInfo[0])));

            var pupilRecord = pupilSearchTile == null ? null : pupilSearchTile.Click<PupilRecordPage>();
            var temp = pupilRecord == null ? new PupilRecordPage() : pupilRecord.RemoveNoteDocument();

            // Pre-condition: delete the pupil if it exist before
            // Navigate to Pupil Record
            SeleniumHelper.NavigateMenu("Tasks", "Pupils", "Delete Pupil");

            // Open Specific pupil
            var deletePupilRecordTriplet = new DeletePupilRecordTriplet();
            deletePupilRecordTriplet.SearchCriteria.PupilName = String.Format("{0}, {1}", pupilInfo[2], pupilInfo[0]);
            resultPupils = deletePupilRecordTriplet.SearchCriteria.Search();

            pupilSearchTile = resultPupils.SingleOrDefault(t => t.Name.Equals(String.Format("{0}, {1}", pupilInfo[0], pupilInfo[2])));
            var deletePupilRecord = pupilSearchTile == null ? new DeletePupilRecordPage() : pupilSearchTile.Click<DeletePupilRecordPage>();

            deletePupilRecord.Delete();

            // Navigate to Pupil Record
            SeleniumHelper.NavigateMenu("Tasks", "Pupils", "Pupil Records");

            pupilRecordTriplet = new PupilRecordTriplet();
            var addNewPupilDialog = pupilRecordTriplet.AddNewPupil();

            addNewPupilDialog.Forename = pupilInfo[0];
            addNewPupilDialog.MiddleName = pupilInfo[1];
            addNewPupilDialog.SurName = pupilInfo[2];
            addNewPupilDialog.Gender = pupilInfo[3];
            addNewPupilDialog.DateOfBirth = pupilInfo[4];

            var registrationDetailDialog = addNewPupilDialog.Continue();
            registrationDetailDialog.DateOfAdmission = pupilInfo[5];
            registrationDetailDialog.YearGroup = pupilInfo[6];


            registrationDetailDialog.CreateRecord();

            var confirmRequiredDialog = new ConfirmRequiredDialog();
            confirmRequiredDialog.ClickOk();

            pupilRecord = PupilRecordPage.Create();
            pupilRecord.SavePupil();
            #endregion

            #region Create Welfare Education
            // Navigate to Pupil Record
            SeleniumHelper.NavigateMenu("Tasks", "Pupils", "Pupil Records");
            pupilRecordTriplet = new PupilRecordTriplet();

            // Open specific pupil record
            pupilRecordTriplet.SearchCriteria.PupilName = String.Format("{0}, {1}", pupilInfo[2], pupilInfo[0]);
            resultPupils = pupilRecordTriplet.SearchCriteria.Search();
            pupilSearchTile = resultPupils.SingleOrDefault(t => t.Name.Equals(String.Format("{0}, {1}", pupilInfo[2], pupilInfo[0])));

            pupilRecord = pupilSearchTile == null ? null : pupilSearchTile.Click<PupilRecordPage>();
            Assert.AreNotEqual(null, pupilRecord, "Does not found pupil");

            // Open Aditional section
            pupilRecord.SelectWelfareTab();
            pupilRecord.ClickAddCareArrangment();
            pupilRecord.LearnerInCareDetails[0].CareAuthority = CareAuthority;
            //pupilRecord.LearnerInCareDetails[0].LivingArrangement = LivingArrangement;
            pupilRecord.LearnerInCareDetails[0].StartDate = StartDate;
            pupilRecord.LearnerInCareDetails[0].EndDate = EndDate;

            pupilRecord = PupilRecordPage.Create();
            pupilRecord.SavePupil();
            #endregion

            // Navigate to Pupil Record
            SeleniumHelper.NavigateMenu("Tasks", "Pupils", "Pupil Records");
            pupilRecordTriplet = new PupilRecordTriplet();

            // Open specific pupil record
            pupilRecordTriplet.SearchCriteria.PupilName = String.Format("{0}, {1}", pupilInfo[2], pupilInfo[0]);
            resultPupils = pupilRecordTriplet.SearchCriteria.Search();
            pupilSearchTile = resultPupils.SingleOrDefault(t => t.Name.Equals(String.Format("{0}, {1}", pupilInfo[2], pupilInfo[0])));

            pupilRecord = pupilSearchTile == null ? null : pupilSearchTile.Click<PupilRecordPage>();
            Assert.AreNotEqual(null, pupilRecord, "Does not found pupil");

            // Open Aditional section
            pupilRecord.SelectWelfareTab();

            var careArrangementsDialog = pupilRecord.LearnerInCareDetails[0].OpenPEP();
            careArrangementsDialog.ClickAddPersonalEducationPlan();
            careArrangementsDialog.PersonalEducationPlans[0].StartDate = StartDate;
            careArrangementsDialog.PersonalEducationPlans[0].EndDate = EndDate;
            careArrangementsDialog.PersonalEducationPlans[0].Notes = notes;
            careArrangementsDialog.ClickOk();
            pupilRecord = PupilRecordPage.Create();
            pupilRecord.SavePupil();

            careArrangementsDialog = pupilRecord.LearnerInCareDetails[0].OpenPEP();
            careArrangementsDialog.PersonalEducationPlans[1].StartDate = NextStartDate;
            careArrangementsDialog.PersonalEducationPlans[1].EndDate = NextEndDate;

            careArrangementsDialog.ClickOk();

            pupilRecord = PupilRecordPage.Create();
            pupilRecord.SavePupil();
            // Verify data is saved Success
            Assert.AreEqual(true, pupilRecord.IsSuccessMessageDisplayed(), "Success message is not display");
            careArrangementsDialog = pupilRecord.LearnerInCareDetails[0].OpenPEP();

            var careArrangementsRow = careArrangementsDialog.PersonalEducationPlans.Rows.SingleOrDefault(p => p.StartDate.Equals(StartDate));
            Assert.AreEqual(EndDate, careArrangementsRow.EndDate, "The start date and end date value is not correct");

            var row = careArrangementsDialog.PersonalEducationPlans.Rows.SingleOrDefault(p => p.StartDate.Equals(NextStartDate));
            Assert.AreEqual(NextEndDate, row.EndDate, "The start date and end date value is not correct");
            careArrangementsDialog.ClickOk();

            pupilRecord = PupilRecordPage.Create();
            pupilRecord.SavePupil();

            #region End Condition :Delete pupil
            // Prepare to delete: Remove the Note if it exist

            // Navigate to Pupil Record
            SeleniumHelper.NavigateMenu("Tasks", "Pupils", "Pupil Records");
            pupilRecordTriplet = new PupilRecordTriplet();

            pupilRecordTriplet.SearchCriteria.PupilName = String.Format("{0}, {1}", pupilInfo[2], pupilInfo[0]);
            resultPupils = pupilRecordTriplet.SearchCriteria.Search();
            pupilSearchTile = resultPupils.SingleOrDefault(t => t.Name.Equals(String.Format("{0}, {1}", pupilInfo[2], pupilInfo[0])));

            pupilRecord = pupilSearchTile == null ? null : pupilSearchTile.Click<PupilRecordPage>();
            temp = pupilRecord == null ? new PupilRecordPage() : pupilRecord.RemoveNoteDocument();

            // Pre-condition: delete the pupil if it exist before
            // Navigate to Pupil Record
            SeleniumHelper.NavigateMenu("Tasks", "Pupils", "Delete Pupil");

            // Open Specific pupil
            deletePupilRecordTriplet = new DeletePupilRecordTriplet();
            deletePupilRecordTriplet.SearchCriteria.PupilName = String.Format("{0}, {1}", pupilInfo[2], pupilInfo[0]);
            resultPupils = deletePupilRecordTriplet.SearchCriteria.Search();

            pupilSearchTile = resultPupils.SingleOrDefault(t => t.Name.Equals(String.Format("{0}, {1}", pupilInfo[2], pupilInfo[0])));
            deletePupilRecord = pupilSearchTile == null ? new DeletePupilRecordPage() : pupilSearchTile.Click<DeletePupilRecordPage>();

            deletePupilRecord.Delete();
            #endregion
        }


        /// <summary>
        ///  Description: Verify add welfare education plan contributor information sucessfully
        /// </summary>
        ///
        //[WebDriverTest(TimeoutSeconds = 2000, Enabled = false, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { PupilTestGroups.PupilRecord.Page, PupilTestGroups.Priority.Priority3 }, DataProvider = "TC_PU12a_Data")]
        public void TC_PU012a_Verify_Add_Welfare_Education_Plan_Contributor(string[] pupilInfo, string startDate, string endDate, string notes, string careAuthority, string livingArrangement)
        {
            #region Pre-Condition: Create new pupil
            // Login as school admin
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);

            // Prepare to delete: Remove the Note if it exist

            // Navigate to Pupil Record
            SeleniumHelper.NavigateMenu("Tasks", "Pupils", "Pupil Records");
            var pupilRecordTriplet = new PupilRecordTriplet();

            pupilRecordTriplet.SearchCriteria.PupilName = String.Format("{0}, {1}", pupilInfo[2], pupilInfo[0]);
            var resultPupils = pupilRecordTriplet.SearchCriteria.Search();
            var pupilSearchTile = resultPupils.SingleOrDefault(t => t.Name.Equals(String.Format("{0}, {1}", pupilInfo[2], pupilInfo[0])));

            var pupilRecord = pupilSearchTile == null ? null : pupilSearchTile.Click<PupilRecordPage>();
            var temp = pupilRecord == null ? new PupilRecordPage() : pupilRecord.RemoveNoteDocument();

            // Pre-condition: delete the pupil if it exist before
            // Navigate to Pupil Record
            SeleniumHelper.NavigateMenu("Tasks", "Pupils", "Delete Pupil");

            // Open Specific pupil
            var deletePupilRecordTriplet = new DeletePupilRecordTriplet();
            deletePupilRecordTriplet.SearchCriteria.PupilName = String.Format("{0}, {1}", pupilInfo[2], pupilInfo[0]);
            resultPupils = deletePupilRecordTriplet.SearchCriteria.Search();

            pupilSearchTile = resultPupils.SingleOrDefault(t => t.Name.Equals(String.Format("{0}, {1}", pupilInfo[0], pupilInfo[2])));
            var deletePupilRecord = pupilSearchTile == null ? new DeletePupilRecordPage() : pupilSearchTile.Click<DeletePupilRecordPage>();

            deletePupilRecord.Delete();

            // Navigate to Pupil Record
            SeleniumHelper.NavigateMenu("Tasks", "Pupils", "Pupil Records");

            pupilRecordTriplet = new PupilRecordTriplet();
            var addNewPupilDialog = pupilRecordTriplet.AddNewPupil();

            addNewPupilDialog.Forename = pupilInfo[0];
            addNewPupilDialog.MiddleName = pupilInfo[1];
            addNewPupilDialog.SurName = pupilInfo[2];
            addNewPupilDialog.Gender = pupilInfo[3];
            addNewPupilDialog.DateOfBirth = pupilInfo[4];

            var registrationDetailDialog = addNewPupilDialog.Continue();
            registrationDetailDialog.DateOfAdmission = pupilInfo[5];
            registrationDetailDialog.YearGroup = pupilInfo[6];


            registrationDetailDialog.CreateRecord();

            var confirmRequiredDialog = new ConfirmRequiredDialog();
            confirmRequiredDialog.ClickOk();

            pupilRecord = PupilRecordPage.Create();
            pupilRecord.SavePupil();
            #endregion

            #region Create Welfare Education
            // Navigate to Pupil Record
            SeleniumHelper.NavigateMenu("Tasks", "Pupils", "Pupil Records");
            pupilRecordTriplet = new PupilRecordTriplet();

            // Open specific pupil record
            pupilRecordTriplet.SearchCriteria.PupilName = String.Format("{0}, {1}", pupilInfo[2], pupilInfo[0]);
            resultPupils = pupilRecordTriplet.SearchCriteria.Search();

            pupilSearchTile = resultPupils.SingleOrDefault(t => t.Name.Equals(String.Format("{0}, {1}", pupilInfo[2], pupilInfo[0])));

            pupilRecord = pupilSearchTile == null ? null : pupilSearchTile.Click<PupilRecordPage>();
            Assert.AreNotEqual(null, pupilRecord, "Does not found pupil");

            // Open Aditional section
            pupilRecord.SelectWelfareTab();

            pupilRecord.ClickAddCareArrangment();

            pupilRecord.LearnerInCareDetails[0].CareAuthority = careAuthority;
            pupilRecord.LearnerInCareDetails[0].LivingArrangement = livingArrangement;
            pupilRecord.LearnerInCareDetails[0].StartDate = startDate;
            pupilRecord.LearnerInCareDetails[0].EndDate = endDate;

            pupilRecord = PupilRecordPage.Create();
            pupilRecord.SavePupil();
            #endregion

            var careArrangementsDialog = pupilRecord.LearnerInCareDetails[0].OpenPEP();

            careArrangementsDialog.ClickAddPersonalEducationPlan();

            careArrangementsDialog.PersonalEducationPlans[0].StartDate = startDate;
            careArrangementsDialog.PersonalEducationPlans[0].EndDate = endDate;
            careArrangementsDialog.PersonalEducationPlans[0].Notes = notes;
            careArrangementsDialog.ClickOk();
            pupilRecord = PupilRecordPage.Create();
            pupilRecord.SavePupil();

            // Plan Contributors Dialog
            careArrangementsDialog = pupilRecord.LearnerInCareDetails[0].OpenPEP();
            var pepContributorsDialog = careArrangementsDialog.PersonalEducationPlans[0].OpenPEPContributors();

            var contributorContactDialog = pepContributorsDialog.OpenAddPepContributorContactDialog();
            contributorContactDialog.SearchCriteria.SurName = "a";

            var searchStaffResults = contributorContactDialog.SearchCriteria.Search();
            var staffTile = searchStaffResults[0];
            staffTile.Click();

            pepContributorsDialog = contributorContactDialog.ClickPepContributorsOk();

            pepContributorsDialog.ClickOk();
            careArrangementsDialog.ClickOk();
            pupilRecord = PupilRecordPage.Create();
            pupilRecord.SavePupil();

            #region End Condition :Delete pupil
            // Prepare to delete: Remove the Note if it exist

            // Navigate to Pupil Record
            SeleniumHelper.NavigateMenu("Tasks", "Pupils", "Pupil Records");
            pupilRecordTriplet = new PupilRecordTriplet();

            pupilRecordTriplet.SearchCriteria.PupilName = String.Format("{0}, {1}", pupilInfo[2], pupilInfo[0]);
            resultPupils = pupilRecordTriplet.SearchCriteria.Search();
            pupilSearchTile = resultPupils.SingleOrDefault(t => t.Name.Equals(String.Format("{0}, {1}", pupilInfo[2], pupilInfo[0])));

            pupilRecord = pupilSearchTile == null ? null : pupilSearchTile.Click<PupilRecordPage>();
            temp = pupilRecord == null ? new PupilRecordPage() : pupilRecord.RemoveNoteDocument();

            // Pre-condition: delete the pupil if it exist before
            // Navigate to Pupil Record
            SeleniumHelper.NavigateMenu("Tasks", "Pupils", "Delete Pupil");

            // Open Specific pupil
            deletePupilRecordTriplet = new DeletePupilRecordTriplet();
            deletePupilRecordTriplet.SearchCriteria.PupilName = String.Format("{0}, {1}", pupilInfo[2], pupilInfo[0]);
            resultPupils = deletePupilRecordTriplet.SearchCriteria.Search();

            pupilSearchTile = resultPupils.FirstOrDefault();
            var name = pupilSearchTile.Name;
            if (name == String.Format("{0}, {1}", pupilInfo[2], pupilInfo[0]))
            {
                pupilSearchTile = resultPupils.SingleOrDefault(t => t.Name.Equals(String.Format("{0}, {1}", pupilInfo[2], pupilInfo[0])));
                deletePupilRecord = pupilSearchTile == null ? new DeletePupilRecordPage() : pupilSearchTile.Click<DeletePupilRecordPage>();

                deletePupilRecord.Delete();
            }


            #endregion
        }

        /// <summary>
        /// Author: Karim
        /// Description: Verify add young carer periods sucessfully
        /// NOTE: English only data
        /// </summary>
        ///
        //[WebDriverTest(TimeoutSeconds = 2000, Enabled = true, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { PupilTestGroups.PupilRecord.Page, PupilTestGroups.Priority.Priority3 }, DataProvider = "TC_PU12b_Data")]
        public void TC_PU012b_Verify_Add_Welfare_YoungCarer(string[] pupilInfo, string StartDate, string EndDate, string notes)
        {

            #region Pre-Condition: Create new pupil
            // Login as school admin
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);

            // Prepare to delete: Remove the Note if it exist

            // Navigate to Pupil Record
            SeleniumHelper.NavigateMenu("Tasks", "Pupils", "Pupil Records");
            var pupilRecordTriplet = new PupilRecordTriplet();

            pupilRecordTriplet.SearchCriteria.PupilName = String.Format("{0}, {1}", pupilInfo[2], pupilInfo[0]);
            var resultPupils = pupilRecordTriplet.SearchCriteria.Search();
            var pupilSearchTile = resultPupils.SingleOrDefault(t => t.Name.Equals(String.Format("{0}, {1}", pupilInfo[2], pupilInfo[0])));

            var pupilRecord = pupilSearchTile == null ? null : pupilSearchTile.Click<PupilRecordPage>();
            var temp = pupilRecord == null ? new PupilRecordPage() : pupilRecord.RemoveNoteDocument();

            // Pre-condition: delete the pupil if it exist before
            // Navigate to Pupil Record
            SeleniumHelper.NavigateMenu("Tasks", "Pupils", "Delete Pupil");

            // Open Specific pupil
            var deletePupilRecordTriplet = new DeletePupilRecordTriplet();
            deletePupilRecordTriplet.SearchCriteria.PupilName = String.Format("{0}, {1}", pupilInfo[2], pupilInfo[0]);
            resultPupils = deletePupilRecordTriplet.SearchCriteria.Search();

            pupilSearchTile = resultPupils.SingleOrDefault(t => t.Name.Equals(String.Format("{0}, {1}", pupilInfo[0], pupilInfo[2])));
            var deletePupilRecord = pupilSearchTile == null ? new DeletePupilRecordPage() : pupilSearchTile.Click<DeletePupilRecordPage>();

            deletePupilRecord.Delete();

            // Navigate to Pupil Record
            SeleniumHelper.NavigateMenu("Tasks", "Pupils", "Pupil Records");

            pupilRecordTriplet = new PupilRecordTriplet();
            var addNewPupilDialog = pupilRecordTriplet.AddNewPupil();

            addNewPupilDialog.Forename = pupilInfo[0];
            addNewPupilDialog.MiddleName = pupilInfo[1];
            addNewPupilDialog.SurName = pupilInfo[2];
            addNewPupilDialog.Gender = pupilInfo[3];
            addNewPupilDialog.DateOfBirth = pupilInfo[4];

            var registrationDetailDialog = addNewPupilDialog.Continue();
            registrationDetailDialog.DateOfAdmission = pupilInfo[5];
            registrationDetailDialog.YearGroup = pupilInfo[6];


            registrationDetailDialog.CreateRecord();

            var confirmRequiredDialog = new ConfirmRequiredDialog();
            confirmRequiredDialog.ClickOk();

            pupilRecord = PupilRecordPage.Create();
            pupilRecord.SavePupil();
            #endregion
            // Navigate to Pupil Record
            SeleniumHelper.NavigateMenu("Tasks", "Pupils", "Pupil Records");
            pupilRecordTriplet = new PupilRecordTriplet();

            // Open specific pupil record
            pupilRecordTriplet.SearchCriteria.PupilName = String.Format("{0}, {1}", pupilInfo[2], pupilInfo[0]);
            resultPupils = pupilRecordTriplet.SearchCriteria.Search();
            pupilSearchTile = resultPupils.SingleOrDefault(t => t.Name.Equals(String.Format("{0}, {1}", pupilInfo[2], pupilInfo[0])));

            pupilRecord = pupilSearchTile == null ? null : pupilSearchTile.Click<PupilRecordPage>();
            Assert.AreNotEqual(null, pupilRecord, "Does not found pupil");

            // Open Welfare section
            pupilRecord.SelectWelfareTab();

            pupilRecord.ClickAddYoungCarerPeriod();
            pupilRecord.LearnerYoungCarerPeriods[0].StartDate = StartDate;
            pupilRecord.LearnerYoungCarerPeriods[0].EndDate = EndDate;
            pupilRecord.LearnerYoungCarerPeriods[0].Notes = notes;

            pupilRecord = PupilRecordPage.Create();
            pupilRecord.SavePupil();
            // Verify data is saved Success
            Assert.AreEqual(true, pupilRecord.IsSuccessMessageDisplayed(), "Success message is not display");
            Assert.AreEqual(StartDate, pupilRecord.LearnerYoungCarerPeriods[0].StartDate, "The StartDate is not correct");
            Assert.AreEqual(EndDate, pupilRecord.LearnerYoungCarerPeriods[0].EndDate, "The EndDate is not correct");

            #region End Condition :Delete pupil
            // Prepare to delete: Remove the Note if it exist

            // Navigate to Pupil Record
            SeleniumHelper.NavigateMenu("Tasks", "Pupils", "Pupil Records");
            pupilRecordTriplet = new PupilRecordTriplet();

            pupilRecordTriplet.SearchCriteria.PupilName = String.Format("{0}, {1}", pupilInfo[2], pupilInfo[0]);
            resultPupils = pupilRecordTriplet.SearchCriteria.Search();
            pupilSearchTile = resultPupils.SingleOrDefault(t => t.Name.Equals(String.Format("{0}, {1}", pupilInfo[2], pupilInfo[0])));

            pupilRecord = pupilSearchTile == null ? null : pupilSearchTile.Click<PupilRecordPage>();
            temp = pupilRecord == null ? new PupilRecordPage() : pupilRecord.RemoveNoteDocument();

            // Pre-condition: delete the pupil if it exist before
            // Navigate to Pupil Record
            SeleniumHelper.NavigateMenu("Tasks", "Pupils", "Delete Pupil");

            // Open Specific pupil
            deletePupilRecordTriplet = new DeletePupilRecordTriplet();
            deletePupilRecordTriplet.SearchCriteria.PupilName = String.Format("{0}, {1}", pupilInfo[2], pupilInfo[0]);
            resultPupils = deletePupilRecordTriplet.SearchCriteria.Search();

            pupilSearchTile = resultPupils.SingleOrDefault(t => t.Name.Equals(String.Format("{0}, {1}", pupilInfo[2], pupilInfo[0])));
            deletePupilRecord = pupilSearchTile == null ? new DeletePupilRecordPage() : pupilSearchTile.Click<DeletePupilRecordPage>();

            deletePupilRecord.Delete();
            #endregion
        }

        /// <summary>
        /// Author: Karim
        /// Description: Verify add pupil disability records sucessfully
        /// NOTE: English only data
        /// </summary>
        ///
        //[WebDriverTest(TimeoutSeconds = 2000, Enabled = true, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { PupilTestGroups.PupilRecord.Page, PupilTestGroups.Priority.Priority3 }, DataProvider = "TC_PU12c_Data")]
        public void TC_PU012c_Verify_Add_Welfare_Disabilities(string[] pupilInfo, string StartDate, string EndDate, string notes)
        {

            #region Pre-Condition: Create new pupil
            // Login as school admin
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);

            // Prepare to delete: Remove the Note if it exist

            // Navigate to Pupil Record
            SeleniumHelper.NavigateMenu("Tasks", "Pupils", "Pupil Records");
            var pupilRecordTriplet = new PupilRecordTriplet();

            pupilRecordTriplet.SearchCriteria.PupilName = String.Format("{0}, {1}", pupilInfo[2], pupilInfo[0]);
            var resultPupils = pupilRecordTriplet.SearchCriteria.Search();
            var pupilSearchTile = resultPupils.SingleOrDefault(t => t.Name.Equals(String.Format("{0}, {1}", pupilInfo[2], pupilInfo[0])));

            var pupilRecord = pupilSearchTile == null ? null : pupilSearchTile.Click<PupilRecordPage>();
            var temp = pupilRecord == null ? new PupilRecordPage() : pupilRecord.RemoveNoteDocument();

            // Pre-condition: delete the pupil if it exist before
            // Navigate to Pupil Record
            SeleniumHelper.NavigateMenu("Tasks", "Pupils", "Delete Pupil");

            // Open Specific pupil
            var deletePupilRecordTriplet = new DeletePupilRecordTriplet();
            deletePupilRecordTriplet.SearchCriteria.PupilName = String.Format("{0}, {1}", pupilInfo[2], pupilInfo[0]);
            resultPupils = deletePupilRecordTriplet.SearchCriteria.Search();

            pupilSearchTile = resultPupils.SingleOrDefault(t => t.Name.Equals(String.Format("{0}, {1}", pupilInfo[0], pupilInfo[2])));
            var deletePupilRecord = pupilSearchTile == null ? new DeletePupilRecordPage() : pupilSearchTile.Click<DeletePupilRecordPage>();

            deletePupilRecord.Delete();

            // Navigate to Pupil Record
            SeleniumHelper.NavigateMenu("Tasks", "Pupils", "Pupil Records");

            pupilRecordTriplet = new PupilRecordTriplet();
            var addNewPupilDialog = pupilRecordTriplet.AddNewPupil();

            addNewPupilDialog.Forename = pupilInfo[0];
            addNewPupilDialog.MiddleName = pupilInfo[1];
            addNewPupilDialog.SurName = pupilInfo[2];
            addNewPupilDialog.Gender = pupilInfo[3];
            addNewPupilDialog.DateOfBirth = pupilInfo[4];

            var registrationDetailDialog = addNewPupilDialog.Continue();
            registrationDetailDialog.DateOfAdmission = pupilInfo[5];
            registrationDetailDialog.YearGroup = pupilInfo[6];


            registrationDetailDialog.CreateRecord();

            var confirmRequiredDialog = new ConfirmRequiredDialog();
            confirmRequiredDialog.ClickOk();

            pupilRecord = PupilRecordPage.Create();
            pupilRecord.SavePupil();
            #endregion
            // Navigate to Pupil Record
            SeleniumHelper.NavigateMenu("Tasks", "Pupils", "Pupil Records");
            pupilRecordTriplet = new PupilRecordTriplet();

            // Open specific pupil record
            pupilRecordTriplet.SearchCriteria.PupilName = String.Format("{0}, {1}", pupilInfo[2], pupilInfo[0]);
            resultPupils = pupilRecordTriplet.SearchCriteria.Search();
            pupilSearchTile = resultPupils.SingleOrDefault(t => t.Name.Equals(String.Format("{0}, {1}", pupilInfo[2], pupilInfo[0])));

            pupilRecord = pupilSearchTile == null ? null : pupilSearchTile.Click<PupilRecordPage>();
            Assert.AreNotEqual(null, pupilRecord, "Does not found pupil");

            // Open Welfare section
            pupilRecord.SelectWelfareTab();

            pupilRecord.ClickAddYoungCarerPeriod();
            pupilRecord.LearnerDisabilities[0].StartDate = StartDate;
            pupilRecord.LearnerDisabilities[0].EndDate = EndDate;
            pupilRecord.LearnerDisabilities[0].Notes = notes;

            pupilRecord = PupilRecordPage.Create();
            pupilRecord.SavePupil();
            // Verify data is saved Success
            Assert.AreEqual(true, pupilRecord.IsSuccessMessageDisplayed(), "Success message is not display");
            Assert.AreEqual(StartDate, pupilRecord.LearnerDisabilities[0].StartDate, "The StartDate is not correct");
            Assert.AreEqual(EndDate, pupilRecord.LearnerDisabilities[0].EndDate, "The EndDate is not correct");

            #region End Condition :Delete pupil
            // Prepare to delete: Remove the Note if it exist

            // Navigate to Pupil Record
            SeleniumHelper.NavigateMenu("Tasks", "Pupils", "Pupil Records");
            pupilRecordTriplet = new PupilRecordTriplet();

            pupilRecordTriplet.SearchCriteria.PupilName = String.Format("{0}, {1}", pupilInfo[2], pupilInfo[0]);
            resultPupils = pupilRecordTriplet.SearchCriteria.Search();
            pupilSearchTile = resultPupils.SingleOrDefault(t => t.Name.Equals(String.Format("{0}, {1}", pupilInfo[2], pupilInfo[0])));

            pupilRecord = pupilSearchTile == null ? null : pupilSearchTile.Click<PupilRecordPage>();
            temp = pupilRecord == null ? new PupilRecordPage() : pupilRecord.RemoveNoteDocument();

            // Pre-condition: delete the pupil if it exist before
            // Navigate to Pupil Record
            SeleniumHelper.NavigateMenu("Tasks", "Pupils", "Delete Pupil");

            // Open Specific pupil
            deletePupilRecordTriplet = new DeletePupilRecordTriplet();
            deletePupilRecordTriplet.SearchCriteria.PupilName = String.Format("{0}, {1}", pupilInfo[2], pupilInfo[0]);
            resultPupils = deletePupilRecordTriplet.SearchCriteria.Search();

            pupilSearchTile = resultPupils.SingleOrDefault(t => t.Name.Equals(String.Format("{0}, {1}", pupilInfo[2], pupilInfo[0])));
            deletePupilRecord = pupilSearchTile == null ? new DeletePupilRecordPage() : pupilSearchTile.Click<DeletePupilRecordPage>();

            deletePupilRecord.Delete();
            #endregion
        }

        /// <summary>
        ///  Author: Y.Ta
        ///  Description: Verify add medical information sucessfully
        /// </summary>
        ///
        //[WebDriverTest(TimeoutSeconds = 2000, Enabled = false, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { PupilTestGroups.PupilRecord.Page, PupilTestGroups.Priority.Priority3 }, DataProvider = "TC_PU13_Data")]
        public void TC_PU013_Verify_Add_School_History_Section(string[] pupilInfo)
        {

            #region Pre-Condition: Create new pupil
            // Login as school admin
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);

            // Prepare to delete: Remove the Note if it exist

            // Navigate to Pupil Record
            SeleniumHelper.NavigateMenu("Tasks", "Pupils", "Pupil Records");
            var pupilRecordTriplet = new PupilRecordTriplet();

            pupilRecordTriplet.SearchCriteria.PupilName = String.Format("{0}, {1}", pupilInfo[2], pupilInfo[0]);
            var resultPupils = pupilRecordTriplet.SearchCriteria.Search();
            var pupilSearchTile = resultPupils.SingleOrDefault(t => t.Name.Equals(String.Format("{0}, {1}", pupilInfo[2], pupilInfo[0])));

            var pupilRecord = pupilSearchTile == null ? null : pupilSearchTile.Click<PupilRecordPage>();
            var temp = pupilRecord == null ? new PupilRecordPage() : pupilRecord.RemoveNoteDocument();

            // Pre-condition: delete the pupil if it exist before
            // Navigate to Pupil Record
            SeleniumHelper.NavigateMenu("Tasks", "Pupils", "Delete Pupil");

            // Open Specific pupil
            var deletePupilRecordTriplet = new DeletePupilRecordTriplet();
            deletePupilRecordTriplet.SearchCriteria.PupilName = String.Format("{0}, {1}", pupilInfo[2], pupilInfo[0]);
            resultPupils = deletePupilRecordTriplet.SearchCriteria.Search();

            pupilSearchTile = resultPupils.SingleOrDefault(t => t.Name.Equals(String.Format("{0}, {1}", pupilInfo[0], pupilInfo[2])));
            var deletePupilRecord = pupilSearchTile == null ? new DeletePupilRecordPage() : pupilSearchTile.Click<DeletePupilRecordPage>();

            deletePupilRecord.Delete();

            // Navigate to Pupil Record
            SeleniumHelper.NavigateMenu("Tasks", "Pupils", "Pupil Records");

            pupilRecordTriplet = new PupilRecordTriplet();
            var addNewPupilDialog = pupilRecordTriplet.AddNewPupil();

            addNewPupilDialog.Forename = pupilInfo[0];
            addNewPupilDialog.MiddleName = pupilInfo[1];
            addNewPupilDialog.SurName = pupilInfo[2];
            addNewPupilDialog.Gender = pupilInfo[3];
            addNewPupilDialog.DateOfBirth = pupilInfo[4];

            var registrationDetailDialog = addNewPupilDialog.Continue();
            registrationDetailDialog.DateOfAdmission = pupilInfo[5];
            registrationDetailDialog.YearGroup = pupilInfo[6];


            registrationDetailDialog.CreateRecord();

            var confirmRequiredDialog = new ConfirmRequiredDialog();
            confirmRequiredDialog.ClickOk();

            pupilRecord = PupilRecordPage.Create();
            pupilRecord.SavePupil();
            #endregion
            // Navigate to Pupil Record
            SeleniumHelper.NavigateMenu("Tasks", "Pupils", "Pupil Records");
            pupilRecordTriplet = new PupilRecordTriplet();

            // Open specific pupil record
            pupilRecordTriplet.SearchCriteria.PupilName = String.Format("{0}, {1}", pupilInfo[2], pupilInfo[0]);
            resultPupils = pupilRecordTriplet.SearchCriteria.Search();
            pupilSearchTile = resultPupils.SingleOrDefault(t => t.Name.Equals(String.Format("{0}, {1}", pupilInfo[2], pupilInfo[0])));

            pupilRecord = pupilSearchTile == null ? null : pupilSearchTile.Click<PupilRecordPage>();
            Assert.AreNotEqual(null, pupilRecord, "Does not found pupil");

            // Open Select School History section
            pupilRecord.SelectSchoolHistoryTab();

            pupilRecord.PreSchoolExperience = "Nursery Unit within a Special School";
            pupilRecord.RegisteredSureStart = "Yes";
            pupilRecord.AttendedSureStart = "Yes";

            var selectSchoolDialog = pupilRecord.OpenAddSchoolDialog();
            selectSchoolDialog.SearchCriteria.SchoolName = "Q";

            var resultListItems = selectSchoolDialog.SearchCriteria.Search();
            var selectSchoolTripletDialog = resultListItems[1].Click<SelectSchoolTripletDialog>();
            selectSchoolTripletDialog.ClickOk();

            pupilRecord = PupilRecordPage.Create();
            pupilRecord.SavePupil();
            // Verify data is saved Success
            Assert.AreEqual(true, pupilRecord.IsSuccessMessageDisplayed(), "Success message is not display");
            Assert.AreEqual("Nursery Unit within a Special School", pupilRecord.PreSchoolExperience, "The Pre School Experience value is not correct");
            Assert.AreEqual("Yes", pupilRecord.RegisteredSureStart, "The Registered SureStart value is not correct");
            Assert.AreEqual("Yes", pupilRecord.AttendedSureStart, "The Attended SureStart value is not correct");

            var resultRow = pupilRecord.LearnerPreviousSchools.Rows.Count() > 0 ? true : false;
            Assert.AreEqual(true, resultRow, "The Previous School does not display");

            #region End Condition :Delete pupil
            // Prepare to delete: Remove the Note if it exist

            // Navigate to Pupil Record
            SeleniumHelper.NavigateMenu("Tasks", "Pupils", "Pupil Records");
            pupilRecordTriplet = new PupilRecordTriplet();

            pupilRecordTriplet.SearchCriteria.PupilName = String.Format("{0}, {1}", pupilInfo[2], pupilInfo[0]);
            resultPupils = pupilRecordTriplet.SearchCriteria.Search();
            pupilSearchTile = resultPupils.SingleOrDefault(t => t.Name.Equals(String.Format("{0}, {1}", pupilInfo[2], pupilInfo[0])));

            pupilRecord = pupilSearchTile == null ? null : pupilSearchTile.Click<PupilRecordPage>();
            temp = pupilRecord == null ? new PupilRecordPage() : pupilRecord.RemoveNoteDocument();

            // Pre-condition: delete the pupil if it exist before
            // Navigate to Pupil Record
            SeleniumHelper.NavigateMenu("Tasks", "Pupils", "Delete Pupil");

            // Open Specific pupil
            deletePupilRecordTriplet = new DeletePupilRecordTriplet();
            deletePupilRecordTriplet.SearchCriteria.PupilName = String.Format("{0}, {1}", pupilInfo[2], pupilInfo[0]);
            resultPupils = deletePupilRecordTriplet.SearchCriteria.Search();

            pupilSearchTile = resultPupils.SingleOrDefault(t => t.Name.Equals(String.Format("{0}, {1}", pupilInfo[2], pupilInfo[0])));
            deletePupilRecord = pupilSearchTile == null ? new DeletePupilRecordPage() : pupilSearchTile.Click<DeletePupilRecordPage>();

            deletePupilRecord.Delete();
            #endregion
        }

        /// <summary>
        ///  Author: Y.Ta
        ///  Description: Verify edit  school hictory information sucessfully
        /// </summary>
        ///

        //[WebDriverTest(TimeoutSeconds = 2000, Enabled = false, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { PupilTestGroups.PupilRecord.Page, PupilTestGroups.Priority.Priority3 }, DataProvider = "TC_PU14_Data")]
        public void TC_PU014_Verify_Edit_School_History_Section(string[] pupilInfo, string YearEdit)
        {

            #region Pre-Condition: Create new pupil
            // Login as school admin
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);

            // Prepare to delete: Remove the Note if it exist

            // Navigate to Pupil Record
            SeleniumHelper.NavigateMenu("Tasks", "Pupils", "Pupil Records");
            var pupilRecordTriplet = new PupilRecordTriplet();

            pupilRecordTriplet.SearchCriteria.PupilName = String.Format("{0}, {1}", pupilInfo[2], pupilInfo[0]);
            var resultPupils = pupilRecordTriplet.SearchCriteria.Search();
            var pupilSearchTile = resultPupils.SingleOrDefault(t => t.Name.Equals(String.Format("{0}, {1}", pupilInfo[2], pupilInfo[0])));

            var pupilRecord = pupilSearchTile == null ? null : pupilSearchTile.Click<PupilRecordPage>();
            var temp = pupilRecord == null ? new PupilRecordPage() : pupilRecord.RemoveNoteDocument();

            // Pre-condition: delete the pupil if it exist before
            // Navigate to Pupil Record
            SeleniumHelper.NavigateMenu("Tasks", "Pupils", "Delete Pupil");

            // Open Specific pupil
            var deletePupilRecordTriplet = new DeletePupilRecordTriplet();
            deletePupilRecordTriplet.SearchCriteria.PupilName = String.Format("{0}, {1}", pupilInfo[2], pupilInfo[0]);
            resultPupils = deletePupilRecordTriplet.SearchCriteria.Search();

            pupilSearchTile = resultPupils.SingleOrDefault(t => t.Name.Equals(String.Format("{0}, {1}", pupilInfo[0], pupilInfo[2])));
            var deletePupilRecord = pupilSearchTile == null ? new DeletePupilRecordPage() : pupilSearchTile.Click<DeletePupilRecordPage>();

            deletePupilRecord.Delete();

            // Navigate to Pupil Record
            SeleniumHelper.NavigateMenu("Tasks", "Pupils", "Pupil Records");

            pupilRecordTriplet = new PupilRecordTriplet();
            var addNewPupilDialog = pupilRecordTriplet.AddNewPupil();

            addNewPupilDialog.Forename = pupilInfo[0];
            addNewPupilDialog.MiddleName = pupilInfo[1];
            addNewPupilDialog.SurName = pupilInfo[2];
            addNewPupilDialog.Gender = pupilInfo[3];
            addNewPupilDialog.DateOfBirth = pupilInfo[4];

            var registrationDetailDialog = addNewPupilDialog.Continue();
            registrationDetailDialog.DateOfAdmission = pupilInfo[5];
            registrationDetailDialog.YearGroup = pupilInfo[6];


            registrationDetailDialog.CreateRecord();

            var confirmRequiredDialog = new ConfirmRequiredDialog();
            confirmRequiredDialog.ClickOk();

            pupilRecord = PupilRecordPage.Create();
            pupilRecord.SavePupil();
            #endregion

            #region Add School History
            // Navigate to Pupil Record
            SeleniumHelper.NavigateMenu("Tasks", "Pupils", "Pupil Records");
            pupilRecordTriplet = new PupilRecordTriplet();

            // Open specific pupil record
            pupilRecordTriplet.SearchCriteria.PupilName = String.Format("{0}, {1}", pupilInfo[2], pupilInfo[0]);
            resultPupils = pupilRecordTriplet.SearchCriteria.Search();
            pupilSearchTile = resultPupils.SingleOrDefault(t => t.Name.Equals(String.Format("{0}, {1}", pupilInfo[2], pupilInfo[0])));

            pupilRecord = pupilSearchTile == null ? null : pupilSearchTile.Click<PupilRecordPage>();
            Assert.AreNotEqual(null, pupilRecord, "Does not found pupil");

            // Open Select School History section
            pupilRecord.SelectSchoolHistoryTab();

            pupilRecord.PreSchoolExperience = "Nursery Unit within a Special School";
            pupilRecord.RegisteredSureStart = "Yes";
            pupilRecord.AttendedSureStart = "Yes";

            var selectSchoolDialog = pupilRecord.OpenAddSchoolDialog();
            selectSchoolDialog.SearchCriteria.SchoolName = "Q";

            var resultListItems = selectSchoolDialog.SearchCriteria.Search();
            var selectSchoolTripletDialog = resultListItems[1].Click<SelectSchoolTripletDialog>();
            selectSchoolTripletDialog.ClickOk();

            pupilRecord = PupilRecordPage.Create();
            pupilRecord.SavePupil();
            #endregion

            // Navigate to Pupil Record
            SeleniumHelper.NavigateMenu("Tasks", "Pupils", "Pupil Records");
            pupilRecordTriplet = new PupilRecordTriplet();

            // Open specific pupil record
            pupilRecordTriplet.SearchCriteria.PupilName = String.Format("{0}, {1}", pupilInfo[2], pupilInfo[0]);
            resultPupils = pupilRecordTriplet.SearchCriteria.Search();
            pupilSearchTile = resultPupils.SingleOrDefault(t => t.Name.Equals(String.Format("{0}, {1}", pupilInfo[2], pupilInfo[0])));

            pupilRecord = pupilSearchTile == null ? null : pupilSearchTile.Click<PupilRecordPage>();
            Assert.AreNotEqual(null, pupilRecord, "Does not found pupil");

            // Open Aditional section
            pupilRecord.SelectSchoolHistoryTab();
            var addAttendanceSummaryDialog = pupilRecord.LearnerPreviousSchools[0].Edit();
            //var addAttendanceSummaryDialog = attendanceSummaryDialog.OpenAddAttendanceSummaryDialog();
            addAttendanceSummaryDialog.Year = YearEdit;
            addAttendanceSummaryDialog.PossibleSessions = "432";
            addAttendanceSummaryDialog.AttendedSessions = "423";
            addAttendanceSummaryDialog.AuthorisedSessions = "9";
            addAttendanceSummaryDialog.UnauthorisedSessions = "0";

            addAttendanceSummaryDialog.ClickOk(4);
            //attendanceSummaryDialog.ClickOk(4);

            // Block by issue
            //Assert.Fail("Block by issue of TC_PU13: Cannot add the Scholl History");
            pupilRecord = PupilRecordPage.Create();
            pupilRecord.SavePupil();
            // Verify data is saved Success
            Assert.AreEqual(true, pupilRecord.IsSuccessMessageDisplayed(), "Success message is not display");

            //attendanceSummaryDialog = pupilRecord.LearnerPreviousSchools[0].Edit();
            //Assert.AreEqual(YearEdit, addAttendanceSummaryDialog.PreviousSchoolAttendanceSummary[0].Year, "The year value is not correct");
            //attendanceSummaryDialog.ClickOk(4);
            pupilRecord = PupilRecordPage.Create();
            pupilRecord.SavePupil();

            #region End Condition :Delete pupil
            // Prepare to delete: Remove the Note if it exist

            // Navigate to Pupil Record
            SeleniumHelper.NavigateMenu("Tasks", "Pupils", "Pupil Records");
            pupilRecordTriplet = new PupilRecordTriplet();

            pupilRecordTriplet.SearchCriteria.PupilName = String.Format("{0}, {1}", pupilInfo[2], pupilInfo[0]);
            resultPupils = pupilRecordTriplet.SearchCriteria.Search();
            pupilSearchTile = resultPupils.SingleOrDefault(t => t.Name.Equals(String.Format("{0}, {1}", pupilInfo[2], pupilInfo[0])));

            pupilRecord = pupilSearchTile == null ? null : pupilSearchTile.Click<PupilRecordPage>();
            temp = pupilRecord == null ? new PupilRecordPage() : pupilRecord.RemoveNoteDocument();

            // Pre-condition: delete the pupil if it exist before
            // Navigate to Pupil Record
            SeleniumHelper.NavigateMenu("Tasks", "Pupils", "Delete Pupil");

            // Open Specific pupil
            deletePupilRecordTriplet = new DeletePupilRecordTriplet();
            deletePupilRecordTriplet.SearchCriteria.PupilName = String.Format("{0}, {1}", pupilInfo[2], pupilInfo[0]);
            resultPupils = deletePupilRecordTriplet.SearchCriteria.Search();

            pupilSearchTile = resultPupils.SingleOrDefault(t => t.Name.Equals(String.Format("{0}, {1}", pupilInfo[2], pupilInfo[0])));
            deletePupilRecord = pupilSearchTile == null ? new DeletePupilRecordPage() : pupilSearchTile.Click<DeletePupilRecordPage>();

            deletePupilRecord.Delete();
            #endregion
        }

        /// <summary>
        /// Author: Hieu Pham
        /// Descriptions: Exercise ability to invoke a 'Related Link' namely -'Part Time Attendance Pattern'
        /// </summary>
        //[WebDriverTest(TimeoutSeconds = 3000, Enabled = false, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { PupilTestGroups.PupilRecord.Page, PupilTestGroups.Priority.Priority3 }, DataProvider = "TC_PU044C_DATA")]
        public void TC_PU044C_Adminstrator_Part_Time_Attendance(string pupilSurName, string pupilForeName, string gender, string dateOfBirth,
            string DateOfAdmission, string pupilName, string registerDate, string endDate, string className, string yearGroupName, string markForAM, string markForPM)
        {
            #region Pre-condition
            // Login
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);

            // Navigate to Pupil Record
            SeleniumHelper.NavigateMenu("Tasks", "Pupils", "Pupil Records");

            var pupilRecordTriplet = new PupilRecordTriplet();
            var addNewPupilDialog = pupilRecordTriplet.AddNewPupil();

            // Enter values
            addNewPupilDialog.Forename = pupilForeName;
            addNewPupilDialog.SurName = pupilSurName;
            addNewPupilDialog.Gender = gender;
            addNewPupilDialog.DateOfBirth = dateOfBirth;
            var registrationDetailDialog = addNewPupilDialog.Continue();
            registrationDetailDialog.DateOfAdmission = DateOfAdmission;
            registrationDetailDialog.YearGroup = yearGroupName;
            registrationDetailDialog.ClassName = className;
            registrationDetailDialog.CreateRecord();

            // Confirm create new pupil
            var confirmRequiredDialog = new ConfirmRequiredDialog();
            confirmRequiredDialog.ClickOk();

            // Save values
            var pupilRecord = new PupilRecordPage();
            pupilRecord.SavePupil();

            #endregion

            #region Steps

            // Navigate to attendance register
            SeleniumHelper.NavigateMenu("Tasks", "Attendance", "Edit Marks");

            // Search attendance record
            var editMarkTriplet = new EditMarksTriplet();
            editMarkTriplet.SearchCriteria.StartDate = registerDate;
            editMarkTriplet.SearchCriteria.Week = true;
            editMarkTriplet.SearchCriteria.SelectClass(className);
            editMarkTriplet.SearchCriteria.SelectYearGroup(yearGroupName);
            var editMarkPage = editMarkTriplet.SearchCriteria.Search<EditMarksPage>();
            editMarkPage.ModePreserve = false;
            var editMarkTable = editMarkPage.Marks;

            // Enter values
            editMarkTable[pupilName][1].Text = "/";
            editMarkTable[pupilName][2].Text = @"\";
            editMarkTable[pupilName][3].Text = "/";
            editMarkTable[pupilName][4].Text = @"\";
            editMarkTable[pupilName][5].Text = "/";
            editMarkTable[pupilName][6].Text = @"\";
            editMarkTable[pupilName][7].Text = "/";
            editMarkTable[pupilName][8].Text = @"\";
            editMarkTable[pupilName][9].Text = "/";
            editMarkTable[pupilName][10].Text = @"\";
            editMarkPage.Save();

            // Search attendance record again
            editMarkTriplet = new EditMarksTriplet();
            editMarkTriplet.SearchCriteria.StartDate = registerDate;
            editMarkTriplet.SearchCriteria.Week = true;
            editMarkPage = editMarkTriplet.SearchCriteria.Search<EditMarksPage>();
            editMarkPage.ModePreserve = false;
            editMarkTable = editMarkPage.Marks;

            // Enter values
            editMarkTable[pupilName][1].Text = "";
            editMarkTable[pupilName][2].Text = "";
            editMarkTable[pupilName][3].Text = "";
            editMarkTable[pupilName][4].Text = "";
            editMarkTable[pupilName][5].Text = "";
            editMarkTable[pupilName][6].Text = "";
            editMarkTable[pupilName][7].Text = "";
            editMarkTable[pupilName][8].Text = "";
            editMarkTable[pupilName][9].Text = "";
            editMarkTable[pupilName][10].Text = "";

            // Save values
            editMarkPage.Save();
            var warningDialog = new POM.Components.Attendance.WarningConfirmDialog();
            warningDialog.ClickContinueDelete();

            // Navigate to pupil record
            SeleniumHelper.NavigateMenu("Tasks", "Pupils", "Pupil Records");
            pupilRecordTriplet = new PupilRecordTriplet();
            pupilRecordTriplet.SearchCriteria.PupilName = pupilName;
            var pupilResults = pupilRecordTriplet.SearchCriteria.Search();
            var pupilRecordPage = pupilResults.FirstOrDefault(x => x.Name.Equals(pupilName)).Click<PupilRecordPage>();

            // Navigate to part time attendance pattern
            var attendancePaternDialog = SeleniumHelper.NavigateViaAction<POM.Components.Attendance.AttendancePatternDialog>("Attendance Pattern");
            attendancePaternDialog.StartDate = registerDate;
            attendancePaternDialog.EndDate = endDate;

            // Enter values
            var selectPatternTable = attendancePaternDialog.Table;
            var row = selectPatternTable.Rows.First();
            row.MonAM = markForAM;
            row.MonPM = markForPM;
            row.TueAM = markForAM;
            row.TuePM = markForPM;
            row.WedAM = markForAM;
            row.WedPM = markForPM;
            row.ThuAM = markForAM;
            row.ThuPM = markForPM;
            row.FriAM = markForAM;
            row.FriPM = markForPM;

            // Apply pattern
            confirmRequiredDialog = attendancePaternDialog.ClickApply();
            confirmRequiredDialog.ClickOk();
            attendancePaternDialog.Refresh();

            // Click cancel to return pupil record page
            pupilRecordPage = attendancePaternDialog.ClickClose();

            // Navigate to attendance register
            SeleniumHelper.NavigateMenu("Tasks", "Attendance", "Edit Marks");

            // Search attendance record
            editMarkTriplet = new EditMarksTriplet();
            editMarkTriplet.SearchCriteria.StartDate = registerDate;
            editMarkTriplet.SearchCriteria.Week = true;
            editMarkPage = editMarkTriplet.SearchCriteria.Search<EditMarksPage>();
            editMarkTable = editMarkPage.Marks;

            // Verify mark is correct
            Assert.AreEqual(markForAM, editMarkTable[pupilName][1].Text, "Monday AM mark is incorrect");
            Assert.AreEqual(markForPM, editMarkTable[pupilName][2].Text, "Monday PM mark is incorrect");
            Assert.AreEqual(markForAM, editMarkTable[pupilName][3].Text, "Tuesday AM mark is incorrect");
            Assert.AreEqual(markForPM, editMarkTable[pupilName][4].Text, "Tuesday PM mark is incorrect");
            Assert.AreEqual(markForAM, editMarkTable[pupilName][5].Text, "Wednesday AM mark is incorrect");
            Assert.AreEqual(markForPM, editMarkTable[pupilName][6].Text, "Wednesday PM mark is incorrect");
            Assert.AreEqual(markForAM, editMarkTable[pupilName][7].Text, "Thursday AM mark is incorrect");
            Assert.AreEqual(markForPM, editMarkTable[pupilName][8].Text, "Thursday PM mark is incorrect");
            Assert.AreEqual(markForAM, editMarkTable[pupilName][9].Text, "Friday AM mark is incorrect");
            Assert.AreEqual(markForPM, editMarkTable[pupilName][10].Text, "Friday PM mark is incorrect");
            #endregion

            #region Post-condition

            // Delete a pupil
            SeleniumHelper.NavigateMenu("Tasks", "Pupils", "Delete Pupil");
            var deletePupilRecordTriplet = new DeletePupilRecordTriplet();
            deletePupilRecordTriplet.SearchCriteria.PupilName = String.Format("{0}, {1}", pupilSurName, pupilForeName);
            var resultPupils = deletePupilRecordTriplet.SearchCriteria.Search();
            var pupilSearchTile = resultPupils.SingleOrDefault(t => t.Name.Equals(String.Format("{0}, {1}", pupilSurName, pupilForeName)));
            var deletePupilRecord = pupilSearchTile == null ? new DeletePupilRecordPage() : pupilSearchTile.Click<DeletePupilRecordPage>();
            deletePupilRecord.Delete();

            #endregion
        }

        /// <summary>
        /// Author: Hieu Pham
        /// Descriptions: Exercise ability to invoke a 'Related Link' namely -'Suspensions and Expulsions'.
        /// </summary>
        //[WebDriverTest(TimeoutSeconds = 1800, Enabled = false, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { PupilTestGroups.PupilRecord.Page, PupilTestGroups.Priority.Priority3 }, DataProvider = "TC_PU044D_DATA")]
        public void TC_PU044D_Adminstrator_Suspension(string pupilSurName, string pupilForeName, string gender, string dateOfBirth,
            string DateOfAdmission, string yearGroup, string className, string pupilName, string otherPupiName, string type, string reason, string startDate, string endDate, string startTime, string endTime, string length)
        {
            #region Pre-condition
            // Login
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);

            // Navigate to Pupil Record
            SeleniumHelper.NavigateMenu("Tasks", "Pupils", "Pupil Records");

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
            registrationDetailDialog.ClassName = className;
            registrationDetailDialog.CreateRecord();

            // Confirm create new pupil
            var confirmRequiredDialog = new ConfirmRequiredDialog();
            confirmRequiredDialog.ClickOk();

            // Save values
            var pupilRecord = new PupilRecordPage();
            pupilRecord.SavePupil();

            #endregion

            #region Steps

            pupilRecordTriplet = new PupilRecordTriplet();
            pupilRecordTriplet.SearchCriteria.PupilName = pupilName;
            pupilRecordTriplet.SearchCriteria.IsCurrent = true;
            pupilRecordTriplet.SearchCriteria.IsLeaver = true;
            var pupilRecordPage = pupilRecordTriplet.SearchCriteria.Search().SingleOrDefault(x => x.Name.Equals(pupilName)).Click<PupilRecordPage>();

            // Navigate to suspension & expulsion
            var suspensionTriplet = SeleniumHelper.NavigateViaAction<SuspensionTriplet>("Suspensions and Expulsions");
            var suspensionPage = new SuspensionRecordPage();
            var suspensionGrid = suspensionPage.SuspensionExpulsionGrid;

            // Delete row if exist
            var row = suspensionGrid.Rows.FirstOrDefault(x => x.Type.Equals(type) && x.Reason.Equals(reason));
            suspensionPage.ClickDeleteRow(row);

            // Add new suspension
            var addSuspensionDialog = suspensionPage.ClickAddNewRecord();
            addSuspensionDialog.Type = type;
            addSuspensionDialog.Reason = reason;
            addSuspensionDialog.StartDate = startDate;
            addSuspensionDialog.EndDate = endDate;
            addSuspensionDialog.StartTime = startTime;
            addSuspensionDialog.EndTime = endTime;
            addSuspensionDialog.Lenght = length;
            suspensionPage = addSuspensionDialog.SaveValue();

            // Save value
            suspensionPage = suspensionPage.SaveValues();
            Assert.AreEqual(true, suspensionPage.IsSuccessMessageDisplay(), "Message success does not display");

            // Close screen
            SeleniumHelper.CloseTab("Suspensions");

            // Search any pupil
            pupilRecordTriplet = new PupilRecordTriplet();
            pupilRecordTriplet.SearchCriteria.PupilName = otherPupiName;
            pupilRecordTriplet.SearchCriteria.IsCurrent = true;
            pupilRecordTriplet.SearchCriteria.IsLeaver = true;
            pupilRecordPage = pupilRecordTriplet.SearchCriteria.Search().SingleOrDefault(x => x.Name.Equals(otherPupiName)).Click<PupilRecordPage>();

            // Search pupil that has the 1 day 'Suspension'
            pupilRecordTriplet.SearchCriteria.PupilName = pupilName;
            pupilRecordTriplet.SearchCriteria.IsCurrent = true;
            pupilRecordTriplet.SearchCriteria.IsLeaver = true;
            pupilRecordPage = pupilRecordTriplet.SearchCriteria.Search().SingleOrDefault(x => x.Name.Equals(pupilName)).Click<PupilRecordPage>();

            // Navigate suspension
            suspensionTriplet = SeleniumHelper.NavigateViaAction<SuspensionTriplet>("Suspensions and Expulsions");
            suspensionPage = new SuspensionRecordPage();
            suspensionGrid = suspensionPage.SuspensionExpulsionGrid;

            // Confirm that record is existed
            Assert.AreNotEqual(null, suspensionGrid.Rows.FirstOrDefault(x => x.Type.Equals(type) && x.Reason.Equals(reason)));

            // Delete suspension
            suspensionPage = suspensionPage.ClickDeleteRow(suspensionGrid.Rows.FirstOrDefault(x => x.Type.Equals(type) && x.Reason.Equals(reason)));
            Assert.AreEqual(true, suspensionPage.IsSuccessMessageDisplay(), "Message success does not display");

            #endregion

            #region Post-condition

            // Delete a pupil
            SeleniumHelper.NavigateMenu("Tasks", "Pupils", "Delete Pupil");
            var deletePupilRecordTriplet = new DeletePupilRecordTriplet();
            deletePupilRecordTriplet.SearchCriteria.PupilName = String.Format("{0}, {1}", pupilSurName, pupilForeName);
            var resultPupils = deletePupilRecordTriplet.SearchCriteria.Search();
            var pupilSearchTile = resultPupils.SingleOrDefault(t => t.Name.Equals(String.Format("{0}, {1}", pupilSurName, pupilForeName)));
            var deletePupilRecord = pupilSearchTile == null ? new DeletePupilRecordPage() : pupilSearchTile.Click<DeletePupilRecordPage>();
            deletePupilRecord.Delete();

            #endregion


        }

        /// <summary>
        /// Author: Ba.Truong
        /// Description: PU81: Check exercise ability to add documents to a pupil record
        /// </summary>
        //[WebDriverTest(TimeoutSeconds = 2000, Enabled = false, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { PupilTestGroups.PupilRecord.Page, PupilTestGroups.Priority.Priority3 }, DataProvider = "TC_PU081_Data")]
        public void TC_PU081_Exercise_Ability_Add_Document_To_Pupil_Record(string[] pupilRecords, string[] documentNote1, string[] documentNote2, string[] documentNote3)
        {
            //TODO: disabled as document tab is not ready
            #region PRE-CONDITIONS

            //Create a pupil
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);
            SeleniumHelper.NavigateMenu("Tasks", "Pupils", "Pupil Records");
            var pupilRecordTriplet = new PupilRecordTriplet();
            var addNewPupilDialog = pupilRecordTriplet.AddNewPupil();
            addNewPupilDialog.Forename = pupilRecords[0];
            addNewPupilDialog.SurName = pupilRecords[1];
            addNewPupilDialog.Gender = pupilRecords[2];
            addNewPupilDialog.DateOfBirth = pupilRecords[3];

            var registrationDetailDialog = addNewPupilDialog.Continue();
            registrationDetailDialog.DateOfAdmission = pupilRecords[4];
            registrationDetailDialog.EnrolmentStatus = pupilRecords[5];
            registrationDetailDialog.YearGroup = pupilRecords[6];
            registrationDetailDialog.CreateRecord();

            var confirmRequiredDialog = new ConfirmRequiredDialog();
            confirmRequiredDialog.ClickOk();

            var pupilRecord = new PupilRecordPage();
            pupilRecord.ClickSave();

            #endregion

            #region TEST STEPS

            //Navigate to the 'Document' section then add information
            pupilRecord = new PupilRecordPage();
            pupilRecord.SelectDocumentsTab();

            //Add information
            pupilRecord.DocumentNote[0].Summary = documentNote1[0];
            pupilRecord.DocumentNote[0].Note = documentNote1[1];

            pupilRecord.DocumentNote[1].Summary = documentNote2[0];
            pupilRecord.DocumentNote[1].Note = documentNote2[1];

            pupilRecord.DocumentNote[2].Summary = documentNote3[0];
            pupilRecord.DocumentNote[2].Note = documentNote3[1];

            //Perform save
            pupilRecord.ClickSave();

            //Add Document
            var viewDocument = pupilRecord.DocumentNote[0].AddDocument();
            AddAttachmentDialog addAttchmentDialog = viewDocument.ClickAddAttachment();
            addAttchmentDialog.BrowserToDocument(documentNote1[2]);
            viewDocument = addAttchmentDialog.UploadDocument();
            viewDocument.ClickOk();

            viewDocument = pupilRecord.DocumentNote[1].AddDocument();
            addAttchmentDialog = viewDocument.ClickAddAttachment();
            addAttchmentDialog.BrowserToDocument(documentNote2[2]);
            viewDocument = addAttchmentDialog.UploadDocument();
            viewDocument.ClickOk();

            viewDocument = pupilRecord.DocumentNote[2].AddDocument();
            addAttchmentDialog = viewDocument.ClickAddAttachment();
            addAttchmentDialog.BrowserToDocument(documentNote3[2]);
            viewDocument = addAttchmentDialog.UploadDocument();
            viewDocument.ClickOk();

            //Perform Save
            pupilRecord = new PupilRecordPage();
            pupilRecord.ClickSave();

            //Return Home screen
            SeleniumHelper.CloseTab("Pupil Record");

            //Re-invoke the 'Pupil Record' screen, check data added
            SeleniumHelper.NavigateMenu("Tasks", "Pupils", "Pupil Records");
            pupilRecordTriplet = new PupilRecordTriplet();
            pupilRecordTriplet.SearchCriteria.PupilName = String.Format("{0}, {1}", pupilRecords[0], pupilRecords[1]);
            pupilRecordTriplet.SearchCriteria.IsCurrent = true;
            var pupilTile = pupilRecordTriplet.SearchCriteria.Search().FirstOrDefault(x => x.Name.Equals(String.Format("{0}, {1}", pupilRecords[0], pupilRecords[1])));
            pupilRecord = pupilTile.Click<PupilRecordPage>();

            //Confirm that the uploaded document remains linked
            pupilRecord.SelectDocumentsTab();

            //Check first record
            viewDocument = pupilRecord.DocumentNote[0].AddDocument();
            Assert.AreEqual(documentNote1[2], viewDocument.Documents[0][1].Text, "The uploaded document does not remain linked at record 1");
            viewDocument.ClickOk();

            //Check second record
            viewDocument = pupilRecord.DocumentNote[1].AddDocument();
            Assert.AreEqual(documentNote2[2], viewDocument.Documents[0][1].Text, "The uploaded document does not remain linked at record 2");
            viewDocument.ClickOk();

            //Check third record
            viewDocument = pupilRecord.DocumentNote[2].AddDocument();
            Assert.AreEqual(documentNote3[2], viewDocument.Documents[0][1].Text, "The uploaded document does not remain linked at record 3");
            viewDocument.ClickOk();

            #endregion

            #region POS-CONDITION

            //Remove data added
            pupilRecord.RemoveNoteDocument();

            //Delete pupil added
            SeleniumHelper.NavigateMenu("Tasks", "Pupils", "Delete Pupil");
            var deletePupilRecordTriplet = new DeletePupilRecordTriplet();
            deletePupilRecordTriplet.SearchCriteria.PupilName = pupilRecords[0];
            deletePupilRecordTriplet.SearchCriteria.IsCurrent = true;
            deletePupilRecordTriplet.SearchCriteria.IsLeaver = true;
            var pupilSearchTile = deletePupilRecordTriplet.SearchCriteria.Search().FirstOrDefault(x => x.Name.Equals(String.Format("{0}, {1}", pupilRecords[0], pupilRecords[1])));
            var pupilRecordDelete = pupilSearchTile.Click<DeletePupilRecordPage>();
            pupilRecordDelete.Delete();

            #endregion
        }

        /// <summary>
        /// Author: Ba.Truong
        /// Description: PU79: Exercise ability to view and update a pupils enrolment via the pupils enrolment history
        /// </summary>
        //TODO: Duplication. Hence P4
        //[WebDriverTest(TimeoutSeconds = 3000, Enabled = false, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { PupilTestGroups.PupilRecord.Page, PupilTestGroups.Priority.Priority4 }, DataProvider = "TC_PU079_Data")]
        public void TC_PU079_Exercise_Ability_View_And_Update_Pupils_Enrolment_Via_The_Pupils_Enrolment_History(string[] pupilRecords1, string[] leavingDetails1,
                                                                                                                string[] pupilRecords2, string[] leavingDetails2,
                                                                                                                string[] pupilRecords3, string[] leavingDetails3)
        {
            #region TEST STEPS

            //Add a new ‘Guest Pupil’ 
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);
            SeleniumHelper.NavigateMenu("Tasks", "Pupils", "Pupil Records");
            var pupilRecordTriplet = new PupilRecordTriplet();
            var addNewPupilDialog = pupilRecordTriplet.AddNewPupil();
            addNewPupilDialog.Forename = pupilRecords1[0];
            addNewPupilDialog.SurName = pupilRecords1[1];
            addNewPupilDialog.Gender = pupilRecords1[2];
            addNewPupilDialog.DateOfBirth = pupilRecords1[3];

            var registrationDetailDialog = addNewPupilDialog.Continue();
            registrationDetailDialog.DateOfAdmission = pupilRecords1[4];
            registrationDetailDialog.EnrolmentStatus = pupilRecords1[5];
            registrationDetailDialog.YearGroup = pupilRecords1[6];
            registrationDetailDialog.CreateRecord();

            var confirmRequiredDialog = new ConfirmRequiredDialog();
            confirmRequiredDialog.ClickOk();

            var pupilRecord = new PupilRecordPage();
            pupilRecord.ClickSave();

            //Add pupil leaving details
            var pupilLeavingDetails = SeleniumHelper.NavigateViaAction<PupilLeavingDetailsPage>("Pupil Leaving Details");
            pupilLeavingDetails.DOL = leavingDetails1[0];
            pupilLeavingDetails.ReasonForLeaving = leavingDetails1[1];
            pupilLeavingDetails.Destination = leavingDetails1[2];
            var confirmDialog = pupilLeavingDetails.ClickSave();

            //Select the next two continue options in order to make this pupil a future dated leaver
            confirmDialog.ClickOk();
            var confirmLeaver = new LeaverBackgroundProcessSubmitDialog();
            confirmLeaver.ClickOk();

            //Verify there is a new record listed in the 'Enrolment History' grid with the above values
            pupilRecordTriplet = new PupilRecordTriplet();
            pupilRecordTriplet.SearchCriteria.PupilName = String.Format("{0}, {1}", pupilRecords1[0], pupilRecords1[1]);
            pupilRecordTriplet.SearchCriteria.IsCurrent = false;
            pupilRecordTriplet.SearchCriteria.IsFuture = false;
            pupilRecordTriplet.SearchCriteria.IsLeaver = true;
            var pupilResult = pupilRecordTriplet.SearchCriteria.Search().FirstOrDefault(x => x.Name.Equals(String.Format("{0}, {1}", pupilRecords1[0], pupilRecords1[1])));

            //Pupil sometimes was not changed to 'Leaver' and 'End Date' in Enrolment status grid is not updated although ‘Pupil Leaving Details’ added successfully
            Assert.AreNotEqual(null, pupilResult, "Enrolment status is not updated after adding ‘Pupil Leaving Details’");
            var pupilRecordPage = pupilResult.Click<PupilRecordPage>();
            pupilRecordPage.SelectRegistrationTab();
            var enrolment = pupilRecordPage.EnrolmentStatusHistoryTable.Rows.FirstOrDefault(x => x.EnrolmentStatus.Equals(pupilRecords1[5]));
            Assert.AreEqual(leavingDetails1[0], enrolment.EndDate, "EnrolmentStatusHistoryTable's information is not updated");

            // Re-admit the former pupil
            addNewPupilDialog = pupilRecordTriplet.AddNewPupil();
            addNewPupilDialog.Forename = pupilRecords1[0];
            addNewPupilDialog.SurName = pupilRecords1[1];
            addNewPupilDialog.Gender = pupilRecords1[2];
            addNewPupilDialog.DateOfBirth = pupilRecords1[3];
            var didYouMeanDialog = addNewPupilDialog.ContinueReAdmit();
            var formers = didYouMeanDialog.FormerPupils;
            var formerPupil = formers.Rows
                .FirstOrDefault(x => x.PupilName.Trim().Equals(String.Format("{0}, {1}", pupilRecords1[0], pupilRecords1[1]))
                && x.DOB.Equals(pupilRecords1[3]));
            var registrationDetailsDialog = formerPupil.ClickReEnrolPupilLink();

            // Enter data for registration details dialog
            registrationDetailsDialog.DateOfAdmission = pupilRecords2[0];
            registrationDetailsDialog.EnrolmentStatus = pupilRecords2[1];
            registrationDetailsDialog.YearGroup = pupilRecords2[2];
            registrationDetailsDialog.CreateRecord();

            //Via ‘Pupil Leaving Details’ make this subsidiary registration pupil a leaver with a ‘Date of Leaving’ of ‘30/04/2014’
            pupilLeavingDetails = SeleniumHelper.NavigateViaAction<PupilLeavingDetailsPage>("Pupil Leaving Details");
            pupilLeavingDetails.DOL = leavingDetails2[0];
            pupilLeavingDetails.ReasonForLeaving = leavingDetails2[1];
            pupilLeavingDetails.Destination = leavingDetails2[2];
            confirmDialog = pupilLeavingDetails.ClickSave();
            confirmDialog.ClickOk();
            confirmLeaver = new LeaverBackgroundProcessSubmitDialog();
            confirmLeaver.ClickOk();

            //Re-use ‘Add Pupil’ process again, search for the former subsidiary registration pupil leaver
            pupilRecordTriplet = new PupilRecordTriplet();
            pupilRecordTriplet.SearchCriteria.PupilName = String.Format("{0}, {1}", pupilRecords1[0], pupilRecords1[1]);
            pupilRecordTriplet.SearchCriteria.IsCurrent = false;
            pupilRecordTriplet.SearchCriteria.IsFuture = false;
            pupilRecordTriplet.SearchCriteria.IsLeaver = true;
            pupilResult = pupilRecordTriplet.SearchCriteria.Search().FirstOrDefault(x => x.Name.Equals(String.Format("{0}, {1}", pupilRecords1[0], pupilRecords1[1])));

            //Pupil sometimes was not changed to 'Leaver' and 'End Date' in Enrolment status grid is not updated although ‘Pupil Leaving Details’ added successfully
            Assert.AreNotEqual(null, pupilResult, "Enrolment status is not updated after adding ‘Pupil Leaving Details’");
            pupilRecordPage = pupilResult.Click<PupilRecordPage>();
            pupilRecordPage.SelectRegistrationTab();
            enrolment = pupilRecordPage.EnrolmentStatusHistoryTable.Rows.FirstOrDefault(x => x.EnrolmentStatus.Equals(pupilRecords2[1]));
            Assert.AreEqual(leavingDetails2[0], enrolment.EndDate, "EnrolmentStatusHistoryTable's information is not updated");

            // Re-admit again the former pupil
            addNewPupilDialog = pupilRecordTriplet.AddNewPupil();
            addNewPupilDialog.Forename = pupilRecords1[0];
            addNewPupilDialog.SurName = pupilRecords1[1];
            addNewPupilDialog.Gender = pupilRecords1[2];
            addNewPupilDialog.DateOfBirth = pupilRecords1[3];
            didYouMeanDialog = addNewPupilDialog.ContinueReAdmit();
            formers = didYouMeanDialog.FormerPupils;
            formerPupil = formers.Rows
                .FirstOrDefault(x => x.PupilName.Trim().Equals(String.Format("{0}, {1}", pupilRecords1[0], pupilRecords1[1]))
                && x.DOB.Equals(pupilRecords1[3]));
            registrationDetailsDialog = formerPupil.ClickReEnrolPupilLink();

            // Enter data for registration details dialog
            registrationDetailsDialog.DateOfAdmission = pupilRecords3[0];
            registrationDetailsDialog.EnrolmentStatus = pupilRecords3[1];
            registrationDetailsDialog.YearGroup = pupilRecords3[2];
            pupilRecordPage = registrationDetailsDialog.CreateRecord();

            //Using ‘Enrolment History’ Edit the 'Main - Dual Registration' record, add ‘End Date’ of ‘01/02/2015’
            pupilRecordPage.SelectRegistrationTab();
            enrolment = pupilRecordPage.EnrolmentStatusHistoryTable.Rows.FirstOrDefault(x => x.EnrolmentStatus.Equals("Main – Dual Registration"));
            enrolment.EndDate = leavingDetails3[0];

            //Add a ‘Single Registration’ record
            enrolment = pupilRecordPage.EnrolmentStatusHistoryTable.Rows.FirstOrDefault(x => x.EnrolmentStatus.Equals(""));
            enrolment.EnrolmentStatus = leavingDetails3[1];
            enrolment.StartDate = leavingDetails3[2];

            //Perform Save
            pupilRecordPage = new PupilRecordPage();
            pupilRecordPage.ClickSave();

            //Confirming a save message
            pupilRecordPage.IsSuccessMessageDisplayed();

            //Confirm that for this pupil in the ‘Registration’ section the ‘Enrolment History’ grid contains two records
            var enrolmentRecords = pupilRecordPage.EnrolmentStatusHistoryTable.Rows;
            Assert.AreEqual(true, enrolmentRecords.Any(x => x.EnrolmentStatus.Equals("Main – Dual Registration")), "Value in Enrolment History is incorrect");
            Assert.AreEqual(true, enrolmentRecords.Any(x => x.EnrolmentStatus.Equals("Single Registration")), "Value in Enrolment History is incorrect");

            #endregion

            #region POS-CONDITIONS

            //Delete the pupil added
            SeleniumHelper.NavigateMenu("Tasks", "Pupils", "Delete Pupil");
            var deletePupilRecordTriplet = new DeletePupilRecordTriplet();
            deletePupilRecordTriplet.SearchCriteria.PupilName = String.Format("{0}, {1}", pupilRecords1[0], pupilRecords1[1]);
            deletePupilRecordTriplet.SearchCriteria.IsCurrent = true;
            deletePupilRecordTriplet.SearchCriteria.IsLeaver = true;
            var pupilSearchTile = deletePupilRecordTriplet.SearchCriteria.Search().FirstOrDefault(x => x.Name.Equals(String.Format("{0}, {1}", pupilRecords1[0], pupilRecords1[1])));
            var pupilRecordDetail = pupilSearchTile.Click<DeletePupilRecordPage>();
            pupilRecordDetail.Delete();

            #endregion
        }

        /// <summary>
        ///  Author: Y.Ta
        ///  Description: Verify add document information sucessfully
        ///  SERIAL ONLY
        /// </summary>
        ///
        //[WebDriverTest(TimeoutSeconds = 2000, Enabled = false, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { PupilTestGroups.PupilRecord.Page, PupilTestGroups.Priority.Priority3 }, DataProvider = "TC_PU17_Data")]
        public void TC_PU017_Verify_Add_Document_Section(string[] pupilInfo, string Note)
        {
            #region Pre-Condition: Create new pupil
            // Login as school admin
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);

            // Prepare to delete: Remove the Note if it exist

            // Navigate to Pupil Record
            SeleniumHelper.NavigateMenu("Tasks", "Pupils", "Pupil Records");
            var pupilRecordTriplet = new PupilRecordTriplet();

            pupilRecordTriplet.SearchCriteria.PupilName = String.Format("{0}, {1}", pupilInfo[2], pupilInfo[0]);
            var resultPupils = pupilRecordTriplet.SearchCriteria.Search();
            var pupilSearchTile = resultPupils.SingleOrDefault(t => t.Name.Equals(String.Format("{0}, {1}", pupilInfo[2], pupilInfo[0])));

            var pupilRecord = pupilSearchTile == null ? null : pupilSearchTile.Click<PupilRecordPage>();
            var temp = pupilRecord == null ? new PupilRecordPage() : pupilRecord.RemoveNoteDocument();

            // Pre-condition: delete the pupil if it exist before
            // Navigate to Pupil Record
            SeleniumHelper.NavigateMenu("Tasks", "Pupils", "Delete Pupil");

            // Open Specific pupil
            var deletePupilRecordTriplet = new DeletePupilRecordTriplet();
            deletePupilRecordTriplet.SearchCriteria.PupilName = String.Format("{0}, {1}", pupilInfo[2], pupilInfo[0]);
            resultPupils = deletePupilRecordTriplet.SearchCriteria.Search();

            pupilSearchTile = resultPupils.SingleOrDefault(t => t.Name.Equals(String.Format("{0}, {1}", pupilInfo[0], pupilInfo[2])));
            var deletePupilRecord = pupilSearchTile == null ? new DeletePupilRecordPage() : pupilSearchTile.Click<DeletePupilRecordPage>();

            deletePupilRecord.Delete();

            // Navigate to Pupil Record
            SeleniumHelper.NavigateMenu("Tasks", "Pupils", "Pupil Records");

            pupilRecordTriplet = new PupilRecordTriplet();
            var addNewPupilDialog = pupilRecordTriplet.AddNewPupil();

            addNewPupilDialog.Forename = pupilInfo[0];
            addNewPupilDialog.MiddleName = pupilInfo[1];
            addNewPupilDialog.SurName = pupilInfo[2];
            addNewPupilDialog.Gender = pupilInfo[3];
            addNewPupilDialog.DateOfBirth = pupilInfo[4];

            var registrationDetailDialog = addNewPupilDialog.Continue();
            registrationDetailDialog.DateOfAdmission = pupilInfo[5];
            registrationDetailDialog.YearGroup = pupilInfo[6];


            registrationDetailDialog.CreateRecord();

            var confirmRequiredDialog = new ConfirmRequiredDialog();
            confirmRequiredDialog.ClickOk();

            pupilRecord = PupilRecordPage.Create();
            pupilRecord.SavePupil();
            #endregion

            // Navigate to Pupil Record
            SeleniumHelper.NavigateMenu("Tasks", "Pupils", "Pupil Records");
            pupilRecordTriplet = new PupilRecordTriplet();

            // Open specific pupil record
            pupilRecordTriplet.SearchCriteria.PupilName = String.Format("{0}, {1}", pupilInfo[2], pupilInfo[0]);
            resultPupils = pupilRecordTriplet.SearchCriteria.Search();
            pupilSearchTile = resultPupils.SingleOrDefault(t => t.Name.Equals(String.Format("{0}, {1}", pupilInfo[2], pupilInfo[0])));

            pupilRecord = pupilSearchTile == null ? null : pupilSearchTile.Click<PupilRecordPage>();
            Assert.AreNotEqual(null, pupilRecord, "Does not found pupil");

            // Open Document section
            pupilRecord.SelectDocumentsTab();

            //TODO: As document tab is not ready
            //pupilRecord.DocumentNote[0].Summary = Note;
            //pupilRecord.SavePupil();
            //pupilRecord = new PupilRecordPage();

            //pupilRecord.DocumentNote[0].AddDocument();
            //ViewDocumentDialog viewDocument = new ViewDocumentDialog();
            //AddAttachmentDialog addAttchmentDialog = viewDocument.ClickAddAttachment();
            //addAttchmentDialog.BrowserToDocument();
            //viewDocument = addAttchmentDialog.UploadDocument();
            //viewDocument.ClickOk();

            pupilRecord.SavePupil();

            // Verify data is saved Success
            Assert.AreEqual(true, pupilRecord.IsSuccessMessageDisplayed(), "Success message is not display");

            // The steps below is block by issue 'Unexpected dialog problem'.
            //'Add' another document to the pupil record via the same document search and select functionality and in the summary enter "To Be Replaced".
            //pupilRecord.DocumentNote[1].Summary = "To Be Replaced";
            //pupilRecord.SavePupil();
            //pupilRecord = new PupilRecordPage();

            //pupilRecord.DocumentNote[1].AddDocument();
            //viewDocument = new ViewDocumentDialog();
            //addAttchmentDialog = viewDocument.ClickAddAttachment();
            //addAttchmentDialog.BrowserToDocument();
            //viewDocument = addAttchmentDialog.UploadDocument();
            //viewDocument.ClickOk();

            ////'Remove' the document initially added to the pupil record.
            //var rowRemove = pupilRecord.DocumentNote.Rows.SingleOrDefault(p => p.Note.Equals(Note));
            //rowRemove.DeleteRow();

            ////'Replace' the document
            ////'Notes' field enter the text "Pupil Record General Document".
            //var rowReplace = pupilRecord.DocumentNote.Rows.SingleOrDefault(p => p.Note.Equals("To Be Replaced"));
            //rowReplace.Note = "Pupil Record General Document";

            //pupilRecord.SavePupil();
            //Assert.AreEqual(true, pupilRecord.IsSuccessMessageDisplayed(), "Success message is not display");

            #region End Condition :Delete pupil
            // Prepare to delete: Remove the Note if it exist

            // Navigate to Pupil Record
            SeleniumHelper.NavigateMenu("Tasks", "Pupils", "Pupil Records");
            pupilRecordTriplet = new PupilRecordTriplet();

            pupilRecordTriplet.SearchCriteria.PupilName = String.Format("{0}, {1}", pupilInfo[2], pupilInfo[0]);
            resultPupils = pupilRecordTriplet.SearchCriteria.Search();
            pupilSearchTile = resultPupils.SingleOrDefault(t => t.Name.Equals(String.Format("{0}, {1}", pupilInfo[2], pupilInfo[0])));

            pupilRecord = pupilSearchTile == null ? null : pupilSearchTile.Click<PupilRecordPage>();
            temp = pupilRecord == null ? new PupilRecordPage() : pupilRecord.RemoveNoteDocument();

            // Pre-condition: delete the pupil if it exist before
            // Navigate to Pupil Record
            SeleniumHelper.NavigateMenu("Tasks", "Pupils", "Delete Pupil");

            // Open Specific pupil
            deletePupilRecordTriplet = new DeletePupilRecordTriplet();
            deletePupilRecordTriplet.SearchCriteria.PupilName = String.Format("{0}, {1}", pupilInfo[2], pupilInfo[0]);
            resultPupils = deletePupilRecordTriplet.SearchCriteria.Search();

            pupilSearchTile = resultPupils.SingleOrDefault(t => t.Name.Equals(String.Format("{0}, {1}", pupilInfo[2], pupilInfo[0])));
            deletePupilRecord = pupilSearchTile == null ? new DeletePupilRecordPage() : pupilSearchTile.Click<DeletePupilRecordPage>();

            deletePupilRecord.Delete();
            #endregion
        }


        #region DATA

        public List<object[]> TC_PU01_Data()
        {
            string pattern = "M/d/yyyy";
            string PupilDateOfBirth = DateTime.ParseExact("06/01/2006", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            string PupilDateOfAdmission = DateTime.ParseExact("06/01/2010", pattern, CultureInfo.InvariantCulture).ToString(pattern);

            string random = SeleniumHelper.GenerateRandomNumberUsingDateTime() + "_TC_PU01";

            string randomCharacter = SeleniumHelper.GenerateRandomString(6) + "_TC_PU01";

            var res = new List<Object[]>
            {
                new object[]
                {
                    //ForeName
                    "aPupilForeName"+randomCharacter+random.ToString(),
                    // Middle Name
                    randomCharacter+"aPupilMiddleName"+randomCharacter+random.ToString(),
                    // SurName
                    randomCharacter+"aPupilSurName"+randomCharacter+random.ToString(),
                    // Gender
                    "Male",
                    // DOB
                    PupilDateOfBirth,
                    PupilDateOfAdmission,
                    // Year Group
                    "Year 2",
                    "2A",
                    "Full-Time",
                    "Not a Boarder"
                }

            };
            return res;
        }

        public List<object[]> TC_PU02_Data()
        {
            string pattern = "M/d/yyyy";
            string PupilDateOfBirth = DateTime.ParseExact("06/01/2006", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            string PupilDateOfAdmission = DateTime.ParseExact("06/01/2010", pattern, CultureInfo.InvariantCulture).ToString(pattern);

            string random = SeleniumHelper.GenerateRandomNumberUsingDateTime() + "_TC_PU02";
            string randomCharacter = SeleniumHelper.GenerateRandomString(6) + "_TC_PU02";

            var res = new List<Object[]>
            {
                new object[]
                {
                    new string[]{"aForeName"+randomCharacter+random.ToString(),"aMiddleName"+randomCharacter+random.ToString(),"aSurName"+randomCharacter+random.ToString(),"Male",PupilDateOfBirth,PupilDateOfAdmission,"Year 2","2A","Full-Time","Not a Boarder"},
                    true,
                    // Quick Note
                    "This is Quick Note"
                }

            };
            return res;
        }

        public List<object[]> TC_PU03_Data()
        {
            string pattern = "M/d/yyyy";
            string PupilDateOfBirth = DateTime.ParseExact("06/01/2006", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            string PupilDateOfAdmission = DateTime.ParseExact("06/01/2010", pattern, CultureInfo.InvariantCulture).ToString(pattern);

            string random = SeleniumHelper.GenerateRandomNumberUsingDateTime() + "_TC_PU03";
            string randomCharacter = SeleniumHelper.GenerateRandomString(6) + "_TC_PU03";

            var res = new List<Object[]>
            {
                new object[]
                {
                     new string[]{"aForeName"+randomCharacter+randomCharacter+random.ToString(),"aMiddleName"+randomCharacter+random.ToString(),"aSurName"+randomCharacter+random.ToString(),"Male",PupilDateOfBirth,PupilDateOfAdmission,"Year 2","2A","Full-Time","Not a Boarder"},
                }

            };
            return res;
        }
        public List<object[]> TC_PU04_Data()
        {
            string pattern = "M/d/yyyy";
            string PupilDateOfBirth = DateTime.ParseExact("06/01/2006", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            string PupilDateOfAdmission = DateTime.ParseExact("06/01/2010", pattern, CultureInfo.InvariantCulture).ToString(pattern);

            string random = SeleniumHelper.GenerateRandomNumberUsingDateTime() + "_TC_PU04";
            string randomCharacter = SeleniumHelper.GenerateRandomString(6) + "_TC_PU04";
            var res = new List<Object[]>
            {
                new object[]
                {
                    new string[]{"aForeName"+randomCharacter+random.ToString(),"aMiddleName"+randomCharacter+random.ToString(),"aSurName"+randomCharacter+random.ToString(),"Male",PupilDateOfBirth,PupilDateOfAdmission,"Year 2","2A","Full-Time","Not a Boarder"},
                    // New Address
                    new string[]{"123", "House Name", "Flat", "Street", "District", "City", "United Kingdom", "EC1A 1BB", "United Kingdom"}
                }

            };
            return res;
        }


        public List<object[]> TC_PU05_Data()
        {
            string pattern = "M/d/yyyy";
            string PupilDateOfBirth = DateTime.ParseExact("06/01/2006", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            string PupilDateOfAdmission = DateTime.ParseExact("06/01/2010", pattern, CultureInfo.InvariantCulture).ToString(pattern);

            string random = SeleniumHelper.GenerateRandomNumberUsingDateTime() + "_TC_PU05";
            string randomCharacter = SeleniumHelper.GenerateRandomString(6) + "_TC_PU05";
            var res = new List<Object[]>
            {
                new object[]
                {
                    new string[]{"aForeName"+randomCharacter+random.ToString(),"aMiddleName"+randomCharacter+random.ToString(),"aSurName"+randomCharacter+random.ToString(),"Male",PupilDateOfBirth,PupilDateOfAdmission,"Year 2","2A","Full-Time","Not a Boarder"},
                    // New Address
                    new string[]{"0123456789", "Other", "Note for Phone Number"},
                    new string[]{"abcd@c2kni.org.uk", "Other", "Note for Email Address"},
                }

            };
            return res;
        }

        public List<object[]> TC_PU06_Data()
        {
            string pattern = "M/d/yyyy";
            string PupilDateOfBirth = DateTime.ParseExact("06/01/2006", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            string PupilDateOfAdmission = DateTime.ParseExact("06/01/2010", pattern, CultureInfo.InvariantCulture).ToString(pattern);

            string random = SeleniumHelper.GenerateRandomNumberUsingDateTime() + "_TC_PU06";
            string randomCharacter = SeleniumHelper.GenerateRandomString(6) + "_TC_PU06";
            var res = new List<Object[]>
            {
                new object[]
                {
                    new string[]{"aForeName"+randomCharacter+random.ToString(),"aMiddleName"+randomCharacter+random.ToString(),"aSurName"+randomCharacter+random.ToString(),"Male",PupilDateOfBirth,PupilDateOfAdmission,"Year 2","2A","Full-Time","Not a Boarder"},
                                        // New Address
                    new string[]{"123", "House Name", "Flat", "Street", "District", "City", "United Kingdom", "EC1A 1BB", "United Kingdom"}

                }

            };
            return res;
        }

        public List<object[]> TC_PU07_Data()
        {
            string pattern = "M/d/yyyy";
            string EligibleFreeMealStartDatePast = DateTime.ParseExact("06/01/2006", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            string EligibleFreeMealEndDatePast = DateTime.ParseExact("06/01/2008", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            string EligibleFreeMealStartDateToday = DateTime.ParseExact("06/01/2010", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            string EligibleFreeMealEndDateToday = DateTime.ParseExact("06/01/2015", pattern, CultureInfo.InvariantCulture).ToString(pattern);

            // Meal Pattern
            string MealPatternStartDate = DateTime.ParseExact("06/01/2006", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            string MealPatternEndDate = DateTime.ParseExact("06/01/2008", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            string MealPatternStartToday = DateTime.ParseExact("06/01/2010", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            string MealPatternEndToday = DateTime.ParseExact("06/01/2015", pattern, CultureInfo.InvariantCulture).ToString(pattern);

            string PupilDateOfBirth = DateTime.ParseExact("06/01/2006", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            string PupilDateOfAdmission = DateTime.ParseExact("06/01/2010", pattern, CultureInfo.InvariantCulture).ToString(pattern);

            string random = SeleniumHelper.GenerateRandomNumberUsingDateTime() + "_TC_PU07";
            string randomCharacter = SeleniumHelper.GenerateRandomString(6) + "_TC_PU07";

            var res = new List<Object[]>
            {
                new object[]
                {
                   new string[]{"aForeName"+randomCharacter+random.ToString(),"aMiddleName"+randomCharacter+random.ToString(),"aSurName"+randomCharacter+random.ToString(),"Male",PupilDateOfBirth,PupilDateOfAdmission,"Year 2","2A","Full-Time","Not a Boarder"},
                    // Eligible Free Meal
                    new string[]{ EligibleFreeMealStartDatePast, EligibleFreeMealEndDatePast, EligibleFreeMealStartDateToday,EligibleFreeMealEndDateToday,"This is note"},
                    // Meal Pattern
                    new string[]{MealPatternStartDate,MealPatternEndDate,MealPatternStartToday,MealPatternEndToday}
                }

            };
            return res;
        }

        public List<object[]> TC_PU08_Data()
        {
            string pattern = "M/d/yyyy";
            string dateEvent = DateTime.ParseExact("06/06/2010", pattern, CultureInfo.InvariantCulture).ToString(pattern);

            string PupilDateOfBirth = DateTime.ParseExact("06/01/2006", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            string PupilDateOfAdmission = DateTime.ParseExact("06/01/2010", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            string random = SeleniumHelper.GenerateRandomNumberUsingDateTime() + "_TC_PU08";
            string randomCharacter = SeleniumHelper.GenerateRandomString(6) + "_TC_PU08";
            var res = new List<Object[]>
            {

                new object[]
                {
                    new string[]{"aForeName"+randomCharacter+random.ToString(),"aMiddleName"+randomCharacter+random.ToString(),"aSurName"+randomCharacter+random.ToString(),"Male",PupilDateOfBirth,PupilDateOfAdmission,"Year 2","2A","Full-Time","Not a Boarder"},
                    //NSHNumber
                    "123456",
                    // Medical Practice
                    new string [] {"Bushmills Medical Centre","Dr T Brown"},
                    // Summary Note
                    "Summary",
                    // Medical Description
                    "None",
                    // Medical Event
                    new string [] {"Accident","Accident",dateEvent}
                }

            };
            return res;
        }

        public List<object[]> TC_PU09_Data()
        {
            string pattern = "M/d/yyyy";
            string dateStart = DateTime.ParseExact("06/06/2010", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            string dateEnd = DateTime.ParseExact("06/06/2015", pattern, CultureInfo.InvariantCulture).ToString(pattern);

            string PupilDateOfBirth = DateTime.ParseExact("06/01/2006", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            string PupilDateOfAdmission = DateTime.ParseExact("06/01/2010", pattern, CultureInfo.InvariantCulture).ToString(pattern);

            string random = SeleniumHelper.GenerateRandomNumberUsingDateTime() + "_TC_PU09_Data";
            string randomCharacter = SeleniumHelper.GenerateRandomString(6) + "_TC_PU09_Data";
            var res = new List<Object[]>
            {
                new object[]
                {
                    new string[]{"aForeName"+randomCharacter+random.ToString(),"aMiddleName"+randomCharacter+random.ToString(),"aSurName"+randomCharacter+random.ToString(),"Male",PupilDateOfBirth,PupilDateOfAdmission,"Year 2","2A","Full-Time","Not a Boarder"},
                    //Ethnicity
                    "Vietnamese",
                    //HomeLanguage
                    "English",
                    //religion
                    "Church of God",
                    //AccommodationType
                    "Other",
                    //AsylumStatus
                    "Refugee",
                    //NewcomerPeriods
                    new string [] {dateStart,dateEnd},
                    dateStart
                }

            };
            return res;
        }

        public List<object[]> TC_PU10_Data()
        {
            string pattern = "M/d/yyyy";
            string dateStart = DateTime.ParseExact("06/06/2010", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            string dateEnd = DateTime.ParseExact("06/06/2015", pattern, CultureInfo.InvariantCulture).ToString(pattern);

            string PupilDateOfBirth = DateTime.ParseExact("06/01/2006", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            string PupilDateOfAdmission = DateTime.ParseExact("06/01/2010", pattern, CultureInfo.InvariantCulture).ToString(pattern);

            string random = SeleniumHelper.GenerateRandomNumberUsingDateTime() + "_TC_PU010";
            string randomCharacter = SeleniumHelper.GenerateRandomString(6) + "TC_PU010";
            var res = new List<Object[]>
            {
                new object[]
                {
                    new string[]{"aForeName"+randomCharacter+random.ToString(),"aMiddleName"+randomCharacter+random.ToString(),"aSurName"+randomCharacter+random.ToString(),"Male",PupilDateOfBirth,PupilDateOfAdmission,"Year 2","2A","Full-Time","Not a Boarder"},
                    //ServiceChildren
                    "Unknown",
                    //ServiceChildrenSource
                    "Other",
                    //ModeTravel
                    "Taxi",
                    //TravelRoute
                    "",
                    //AcademicYear                    
                    "Academic Year 2016/2017",
                    //LearnerUniformGrantEligibilities
                    new string [] {dateStart,dateEnd}
                }

            };
            return res;
        }

        public List<object[]> TC_PU11_Data()
        {
            string pattern = "M/d/yyyy";
            var now = DateTime.Now;
            var firstDay = new DateTime(now.Year, now.Month, 1);
            var lastDay = new DateTime(now.Year, 12, 31);
            string dateStart = firstDay.ToString(pattern);
            string dateEnd = lastDay.ToString(pattern);

            string PupilDateOfBirth = DateTime.ParseExact("06/01/2006", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            string PupilDateOfAdmission = DateTime.ParseExact("06/01/2010", pattern, CultureInfo.InvariantCulture).ToString(pattern);

            string random = SeleniumHelper.GenerateRandomNumberUsingDateTime() + "_TC_PU11_Data";
            string randomCharacter = SeleniumHelper.GenerateRandomString(6) + "_TC_PU11_Data";

            var res = new List<Object[]>
            {
                new object[]
                {
                    new string[]{"aForeName"+randomCharacter+random.ToString(),"aMiddleName"+randomCharacter+random.ToString(),"aSurName"+randomCharacter+random.ToString(),"Male",PupilDateOfBirth,PupilDateOfAdmission,"Year 2","2A","Full-Time","Not a Boarder"},
                    //CareAuthority
                    "Northern Health and Social Care Trust",
                    //LivingArrangement
                   "Foster Care",
                    dateStart,
                    dateEnd
                }

            };
            return res;
        }

        public List<object[]> TC_PU12_Data()
        {
            string pattern = "M/d/yyyy";
            var now = DateTime.Now;
            var firstStartDay = new DateTime(now.Year, now.Month, now.Day);
            var firstEndDay = firstStartDay.AddMonths(3);
            string dateStart = firstStartDay.ToString(pattern);
            string dateEnd = firstEndDay.ToString(pattern);
            string notes = "Personal Education Plan Note";

            var secondStartDate = firstEndDay.AddDays(1);
            var secondEndDate = new DateTime(firstEndDay.Year, 12, 25);

            string PupilDateOfBirth = DateTime.ParseExact("06/01/2006", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            string PupilDateOfAdmission = DateTime.ParseExact("06/01/2010", pattern, CultureInfo.InvariantCulture).ToString(pattern);

            string random = SeleniumHelper.GenerateRandomNumberUsingDateTime() + "_TC_PU12_Data";
            string randomCharacter = SeleniumHelper.GenerateRandomString(6) + "_TC_PU12_Data";
            var res = new List<Object[]>
            {
                new object[]
                {
                    new string[]{"aForeName"+randomCharacter+random.ToString(),"aMiddleName"+randomCharacter+random.ToString(),"aSurName"+randomCharacter+random.ToString(),"Male",PupilDateOfBirth,PupilDateOfAdmission,"Year 2","2A","Full-Time","Not a Boarder"},
                    dateStart,
                    dateEnd,
                    notes,
                    secondStartDate.ToString(pattern),
                    secondEndDate.ToString(pattern),
                    //CareAuthority
                    "Northern Health and Social Care Trust",
                    //LivingArrangement
                   "Foster Care",
                }

            };
            return res;
        }


        public List<object[]> TC_PU12a_Data()
        {
            string pattern = "M/d/yyyy";
            var now = DateTime.Now;
            var firstStartDay = new DateTime(now.Year, now.Month, now.Day);
            var firstEndDay = firstStartDay.AddMonths(3);
            string dateStart = firstStartDay.ToString(pattern);
            string dateEnd = firstEndDay.ToString(pattern);
            string notes = "Personal Education Plan Note";

            string pupilDateOfBirth = DateTime.ParseExact("06/01/2006", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            string pupilDateOfAdmission = DateTime.ParseExact("06/01/2010", pattern, CultureInfo.InvariantCulture).ToString(pattern);

            string random = SeleniumHelper.GenerateRandomNumberUsingDateTime() + "_TC_PU012a";
            string randomCharacter = SeleniumHelper.GenerateRandomString(6) + "_TC_PU012a";
            var res = new List<Object[]>
            {
                new object[]
                {
                    new string[]{"aForeName"+randomCharacter+random.ToString(),"aMiddleName"+randomCharacter+random.ToString(),"aSurName"+randomCharacter+random.ToString(),"Male",pupilDateOfBirth,pupilDateOfAdmission,"Year 2","2A","Full-Time","Not a Boarder"},
                    dateStart,
                    dateEnd,
                    notes,
                    //CareAuthority
                    "Northern Health and Social Care Trust",
                    //LivingArrangement
                   "Foster Care"
                }

            };
            return res;
        }

        public List<object[]> TC_PU12b_Data()
        {
            string pattern = "M/d/yyyy";
            var now = DateTime.Now;
            var firstStartDay = new DateTime(now.Year, now.Month, now.Day);
            var firstEndDay = firstStartDay.AddMonths(3);
            string dateStart = firstStartDay.ToString(pattern);
            string dateEnd = firstEndDay.ToString(pattern);
            string notes = "Young Carer Notes";

            var secondStartDate = firstEndDay.AddDays(1);
            var secondEndDate = new DateTime(firstEndDay.Year, 12, 25);

            string PupilDateOfBirth = DateTime.ParseExact("06/01/2006", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            string PupilDateOfAdmission = DateTime.ParseExact("06/01/2010", pattern, CultureInfo.InvariantCulture).ToString(pattern);

            string random = SeleniumHelper.GenerateRandomNumberUsingDateTime() + "_TC_PU12b_Data";
            string randomCharacter = SeleniumHelper.GenerateRandomString(6) + "_TC_PU12b_Data";
            var res = new List<Object[]>
            {
                new object[]
                {
                    new string[]{"aForeName"+randomCharacter+random.ToString(),"aMiddleName"+randomCharacter+random.ToString(),"aSurName"+randomCharacter+random.ToString(),"Male",PupilDateOfBirth,PupilDateOfAdmission,"Year  2","2A","Full-Time","Not a Boarder"},
                    dateStart,
                    dateEnd,
                    notes
                }

            };
            return res;
        }


        public List<object[]> TC_PU12c_Data()
        {
            string pattern = "M/d/yyyy";
            var now = DateTime.Now;
            var firstStartDay = new DateTime(now.Year, now.Month, now.Day);
            var firstEndDay = firstStartDay.AddMonths(3);
            string dateStart = firstStartDay.ToString(pattern);
            string dateEnd = firstEndDay.ToString(pattern);
            string notes = "Disability Notes";

            var secondStartDate = firstEndDay.AddDays(1);
            var secondEndDate = new DateTime(firstEndDay.Year, 12, 25);

            string PupilDateOfBirth = DateTime.ParseExact("06/01/2006", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            string PupilDateOfAdmission = DateTime.ParseExact("06/01/2010", pattern, CultureInfo.InvariantCulture).ToString(pattern);

            string random = SeleniumHelper.GenerateRandomNumberUsingDateTime() + "_TC_PU12c_Data";
            string randomCharacter = SeleniumHelper.GenerateRandomString(6) + "_TC_PU12c_Data";
            var res = new List<Object[]>
            {
                new object[]
                {
                    new string[]{"aForeName"+randomCharacter+random.ToString(),"aMiddleName"+randomCharacter+random.ToString(),"aSurName"+randomCharacter+random.ToString(),"Male",PupilDateOfBirth,PupilDateOfAdmission,"Year  2","2A","Full-Time","Not a Boarder"},
                    dateStart,
                    dateEnd,
                    notes
                }

            };
            return res;
        }

        public List<object[]> TC_PU13_Data()
        {
            string pattern = "M/d/yyyy";
            string PupilDateOfBirth = DateTime.ParseExact("06/01/2006", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            string PupilDateOfAdmission = DateTime.ParseExact("06/01/2010", pattern, CultureInfo.InvariantCulture).ToString(pattern);

            string random = SeleniumHelper.GenerateRandomNumberUsingDateTime() + "_TC_PU13_Data";
            string randomCharacter = SeleniumHelper.GenerateRandomString(6) + "_TC_PU13_Data";
            var res = new List<Object[]>
            {
                new object[]
                {
                    new string[]{"aForeName"+randomCharacter+random.ToString(),"aMiddleName"+randomCharacter+random.ToString(),"aSurName"+randomCharacter+random.ToString(),"Male",PupilDateOfBirth,PupilDateOfAdmission,"Year 2","2A","Full-Time","Not a Boarder"},
                }

            };
            return res;
        }

        public List<object[]> TC_PU14_Data()
        {
            string pattern = "M/d/yyyy";
            var year = DateTime.Now.Year;
            var newYear = year++;
            string yearEdit = String.Format("{0}/{1}", year, newYear);
            string PupilDateOfBirth = DateTime.ParseExact("06/01/2006", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            string PupilDateOfAdmission = DateTime.ParseExact("06/01/2010", pattern, CultureInfo.InvariantCulture).ToString(pattern);

            string random = SeleniumHelper.GenerateRandomNumberUsingDateTime() + "_TC_PU014";
            string randomCharacter = SeleniumHelper.GenerateRandomString(6) + "_TC_PU014";
            var res = new List<Object[]>
            {
                new object[]
                {
                    new string[]{"aForeName"+randomCharacter+random.ToString(),"aMiddleName"+randomCharacter+random.ToString(),"aSurName"+randomCharacter+random.ToString(),"Male",PupilDateOfBirth,PupilDateOfAdmission,"Year 2","2A","Full-Time","Not a Boarder"},
                    yearEdit
                }

            };
            return res;
        }

        public List<object[]> TC_PU15_Data()
        {
            string pattern = "M/d/yyyy";
            string PupilDateOfAdmission = DateTime.ParseExact("06/01/2010", pattern, CultureInfo.InvariantCulture).ToString(pattern);

            string PupilDateOfBirth = DateTime.ParseExact("06/01/2006", pattern, CultureInfo.InvariantCulture).ToString(pattern);

            string random = SeleniumHelper.GenerateRandomNumberUsingDateTime() + "_TC_PU015";
            string randomCharacter = SeleniumHelper.GenerateRandomString(6) + "_TC_PU015";
            var res = new List<Object[]>
            {
                new object[]
                {
                    new string[]{"aForeName"+randomCharacter+random.ToString(),"aMiddleName"+randomCharacter+random.ToString(),"aSurName"+randomCharacter+random.ToString(),"Male",PupilDateOfBirth,PupilDateOfAdmission,"Year 2","2A","Full-Time","Not a Boarder"},
                    PupilDateOfAdmission,
                    //ConsentSignatory
                    "Parent Signatory Received",
                    //Note
                    "Parental consent status duly recorded"
                }

            };
            return res;
        }

        public List<object[]> TC_PU16_Data()
        {
            string pattern = "M/d/yyyy";
            var now = DateTime.Now;
            var toDay = new DateTime(now.Year, now.Month, now.Day);

            var endDate = new DateTime(toDay.Year, 12, 25);

            string PupilDateOfBirth = DateTime.ParseExact("06/01/2006", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            string PupilDateOfAdmission = DateTime.ParseExact("06/01/2010", pattern, CultureInfo.InvariantCulture).ToString(pattern);

            string random = SeleniumHelper.GenerateRandomNumberUsingDateTime() + "_TC_PU016";
            string randomCharacter = SeleniumHelper.GenerateRandomString(6) + "_TC_PU016";
            var res = new List<Object[]>
            {
                new object[]
                {
                    new string[]{"aForeName"+randomCharacter+random.ToString(),"aMiddleName"+randomCharacter+random.ToString(),"aSurName"+randomCharacter+random.ToString(),"Male",PupilDateOfBirth,PupilDateOfAdmission,"Year 2","2A","Full-Time","Not a Boarder"},
                   toDay.ToString(pattern),
                   endDate.ToString(pattern),
                   //SenStagesStage
                   "Identify need",
                   // SenNeedType
                   "Mild Learning Difficulties",
                   //SenNeedsRank
                   "1",
                   //Sen Description
                   "GP",
                   //SenProvisionsType
                   "Time in Specialist Unit",
                   // Comment
                   "Requested Time in Special Unit"
                }

            };
            return res;
        }

        public List<object[]> TC_PU17_Data()
        {
            string pattern = "M/d/yyyy";
            string PupilDateOfBirth = DateTime.ParseExact("06/01/2006", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            string PupilDateOfAdmission = DateTime.ParseExact("06/01/2010", pattern, CultureInfo.InvariantCulture).ToString(pattern);

            string random = SeleniumHelper.GenerateRandomNumberUsingDateTime() + "_TC_PU017";
            string randomCharacter = SeleniumHelper.GenerateRandomString(6) + "_TC_PU017";

            var res = new List<Object[]>
            {
                new object[]
                {
                    new string[]{"aForeName"+randomCharacter+random.ToString(),"aMiddleName"+randomCharacter+random.ToString(),"aSurName"+randomCharacter+random.ToString(),"Male",PupilDateOfBirth,PupilDateOfAdmission,"Year 2","2A","Full-Time","Not a Boarder"},         
                    // Note
                    "Remove"
                }

            };
            return res;
        }

        public List<object[]> TC_PU019_Data()
        {
            string pattern = "M/d/yyyy";
            string PupilDateOfBirth = DateTime.ParseExact("06/01/2006", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            string PupilDateOfAdmission = DateTime.ParseExact("06/01/2010", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            string foreName = String.Format("{0}_{1}", SeleniumHelper.GenerateRandomString(7), SeleniumHelper.GenerateRandomNumberUsingDateTime());
            string surName = String.Format("{0}_{1}", "Avn", SeleniumHelper.GenerateRandomString(8));
            var res = new List<Object[]>
            {
                new object[]
                {
                    foreName, surName, "Male", PupilDateOfBirth, PupilDateOfAdmission, "Year 2", "SEN",
                }

            };
            return res;
        }

        public List<object[]> TC_PU020_Data()
        {
            string pattern = "M/d/yyyy";
            string PupilDateOfBirth = DateTime.ParseExact("06/01/2006", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            string PupilDateOfAdmission = DateTime.ParseExact("06/01/2010", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            string foreName = String.Format("{0}_{1}", SeleniumHelper.GenerateRandomString(7), SeleniumHelper.GenerateRandomNumberUsingDateTime());
            string surName = String.Format("{0}_{1}", "Avn", SeleniumHelper.GenerateRandomString(8));
            var res = new List<Object[]>
            {
                new object[]
                {
                    foreName, surName, "Male", PupilDateOfBirth, PupilDateOfAdmission, "Year 2", "SEN",
                    new string[]{"1234", "House", "Flat", "Street", "District", "City", "County", "EC1A 1BB", "United Kingdom"},
                    new string[]{"567", "House Name", "Flat2", "Street2", "District2", "City2", "Conty2", "EC1A 1BB", "United Kingdom"},
                }

            };
            return res;
        }

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

        public List<object[]> TC_PU022_Data()
        {
            string pattern = "M/d/yyyy";
            string PupilDateOfBirth = DateTime.ParseExact("06/01/2006", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            string PupilDateOfAdmission = DateTime.ParseExact("06/01/2010", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            string foreName = String.Format("{0}_{1}", SeleniumHelper.GenerateRandomString(7), SeleniumHelper.GenerateRandomNumberUsingDateTime());
            string surName = String.Format("{0}_{1}", "Avn", SeleniumHelper.GenerateRandomString(8));
            var res = new List<Object[]>
            {
                new object[]
                {
                    foreName, surName, "Male", PupilDateOfBirth, PupilDateOfAdmission, "Year 2", "SEN",
                    "Mr", "LG", "Contact",
                    new string[]{"1234", "House", "Flat", "Street", "District", "City", "County", "EC1A 1BB", "United Kingdom"},
                    new string[]{"567", "House Name", "Flat2", "Street2", "District2", "City2", "Conty2", "EC1A 1BB", "United Kingdom"},
                }

            };
            return res;
        }

        public List<object[]> TC_PU023_Data()
        {
            string pattern = "M/d/yyyy";
            string dateOfLeaving = SeleniumHelper.GetFirstDayOfWeek(DateTime.Now.Subtract(TimeSpan.FromDays(14))).ToString(pattern);
            string dateOfBirth = DateTime.ParseExact("06/01/2006", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            string PupilDateOfAdmission = DateTime.ParseExact("06/01/2010", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            string dateOfAdmission = SeleniumHelper.GetFirstDayOfWeek(DateTime.Now.Add(TimeSpan.FromDays(14))).ToString(pattern);
            string foreName = String.Format("{0}_{1}", SeleniumHelper.GenerateRandomString(7), SeleniumHelper.GenerateRandomNumberUsingDateTime());
            string surName = String.Format("{0}_{1}", "Avn", SeleniumHelper.GenerateRandomString(8));
            var res = new List<Object[]>
            {
                new object[]
                {
                    foreName, surName, "Male", dateOfBirth, PupilDateOfAdmission, "Year 2", "SEN", dateOfLeaving, "Not Known", dateOfAdmission
                }

            };
            return res;
        }

        public List<object[]> TC_PU024_Data()
        {
            string pattern = "M/d/yyyy";
            string dateOfBirth = DateTime.ParseExact("06/01/2006", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            string dateOfAdmission = DateTime.ParseExact("06/01/2010", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            string patternStart = SeleniumHelper.GetFirstDayOfWeek(DateTime.Now).ToString(pattern);
            string patternEnd = SeleniumHelper.GetLastDayOfWeek(DateTime.Now).ToString(pattern);
            string foreName = String.Format("{0}_{1}_{2}", "Avn", SeleniumHelper.GenerateRandomString(7), SeleniumHelper.GenerateRandomNumberUsingDateTime());
            string surName = String.Format("{0}_{1}", "Aa", SeleniumHelper.GenerateRandomString(7));
            var res = new List<Object[]>
            {
                new object[]
                {
                    foreName, surName, "Male", dateOfBirth, dateOfAdmission, "Year 2", "SEN", "AM only", patternStart, patternEnd,
                }

            };
            return res;
        }

        public List<object[]> TC_PU025_Data()
        {
            string pattern = "M/d/yyyy";
            string dateOfBirth = DateTime.ParseExact("06/01/2006", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            string dateOfAdmission = DateTime.ParseExact("06/01/2010", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            string dateOfLeaving = SeleniumHelper.GetFirstDayOfWeek(DateTime.Now.Subtract(TimeSpan.FromDays(7))).ToString(pattern);
            string foreName1 = String.Format("{0}_{1}_{2}", "Avn", SeleniumHelper.GenerateRandomString(7), SeleniumHelper.GenerateRandomNumberUsingDateTime());
            string surName1 = String.Format("{0}_{1}", "Aa", SeleniumHelper.GenerateRandomString(7));
            string foreName2 = String.Format("{0}_{1}_{2}", "Avn", SeleniumHelper.GenerateRandomString(7), SeleniumHelper.GenerateRandomNumberUsingDateTime());
            string surName2 = String.Format("{0}_{1}", "Ab", SeleniumHelper.GenerateRandomString(7));
            var res = new List<Object[]>
            {
                new object[]
                {
                    new string[]{foreName1, surName1, "Male", dateOfBirth, dateOfAdmission},
                    new string[]{foreName2, surName2, "Female", dateOfBirth, dateOfAdmission},
                    "Single Registration", "Year 2", "SEN", dateOfLeaving, "Voluntary transfer", "Queens Secondary School"
                }

            };
            return res;
        }


        public List<object[]> TC_PU026_Data()
        {
            string pattern = "M/d/yyyy";
            string dateOfBirth = DateTime.ParseExact("06/01/2006", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            string dateOfAdmission = DateTime.ParseExact("06/01/2010", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            string dateOfLeaving = SeleniumHelper.GetFirstDayOfWeek(DateTime.Now.Add(TimeSpan.FromDays(7))).ToString(pattern);

            string random = SeleniumHelper.GenerateRandomNumberUsingDateTime() + "_TC_PU026";
            string randomCharacter = SeleniumHelper.GenerateRandomString(6) + "_TC_PU026";

            var res = new List<Object[]>
            {
                new object[]
                {
                    new string[]{"AFutureTest"+randomCharacter+random, "Aaa", "Male", dateOfBirth, dateOfAdmission},
                    new string[]{"AFutureTest"+randomCharacter+random, "Aab", "Female", dateOfBirth, dateOfAdmission},
                    "Single Registration", "Year 5", "5A", dateOfLeaving, "Not Known", "Kings Secondary School"
                }

            };
            return res;
        }


        public List<object[]> TC_PU027_Data()
        {
            var randomName = "Luong" + SeleniumHelper.GenerateRandomString(5) + SeleniumHelper.GenerateRandomNumberUsingDateTime();
            var res = new List<Object[]>
            {
                new object[]
                {
                    // Forename
                    randomName,
                    // Surname
                    randomName,
                    // Gender
                    "Male",
                    // DOB
                    "2/2/2011",
                    //DateOfAdmission
                    "10/5/2014",
                    // YearGroup
                    "Year 1",
                    // DOL
                    "11/5/2014",
                    // ReasonForLeaving
                    "Not Known",
                    // EnrolmentStatus
                    "Single Registration",
                    //re-admit date
                    "10/5/2015",
                }

            };
            return res;
        }

        public List<object[]> TC_PU028_Data()
        {
            var randomName = "Luong" + SeleniumHelper.GenerateRandomString(5) + SeleniumHelper.GenerateRandomNumberUsingDateTime();

            var res = new List<Object[]>
            {
                new object[]
                {
                    // Forename
                    randomName,
                    // Surname
                    randomName,
                    // Gender
                    "Male",
                    // DOB
                    "2/2/2011",
                    //DateOfAdmission
                    "10/5/2014",
                    // YearGroup
                    "Year 1",
                    // DOL
                    "11/5/2014",
                    // ReasonForLeaving
                    "Not Known",
                    // EnrolmentStatus
                    "Single Registration",
                    //re-admit date
                    "10/5/2015",
                }

            };
            return res;
        }

        public List<object[]> TC_PU029_Data()
        {
            var randomName = Thread.CurrentThread.ManagedThreadId + "Luong" + SeleniumHelper.GenerateRandomString(5) + SeleniumHelper.GenerateRandomNumberUsingDateTime();

            var res = new List<Object[]>
            {
                new object[]
                {
                    // Forename contact
                    randomName,
                    // Surname contact
                    randomName,
                    // Title
                    "Mr",
                    // Gender
                    "Male",
                    // Salutation,
                    "Prof Aaron",
                    // Addressee
                    "Prof J Aaron",
                    // BuildingNo
                    "20",
                    // Street
                    "Bushfoot Road",
                    // District
                    "Portballintrae",
                    // City
                    "Bushmills",
                    // County
                    "",
                    // PostCode
                    "BT57 8RR",
                    // CountryPostCode
                    "United Kingdom",
                    // Language
                    "English",
                    // PlaceOfWork
                    "Northern Ireland",
                    // JobTitle
                    "",
                    // Occupation
                    "",
                    // Forename pupil
                    "Fiona",
                    // Surname pupil
                    "Baker",
                    // Priority
                    "1",
                    // Relationship
                    "Parent"
                }

            };
            return res;
        }

        public List<object[]> TC_PU030_Data()
        {
            var randomName = "Luong" + SeleniumHelper.GenerateRandomString(5) + SeleniumHelper.GenerateRandomNumberUsingDateTime();

            var res = new List<Object[]>
            {
                new object[]
                {
                    // Forename contact
                    randomName,
                    // Surname contact
                    randomName,
                    // Title
                    "Mr",
                    // Gender
                    "Male",
                    // Salutation,
                    "Prof Aaron",
                    // Addressee
                    "Prof J Aaron"
                }

            };
            return res;
        }

        public List<object[]> TC_PU031a_Data()
        {

            var res = new List<Object[]>
            {
                new object[]
                {
                    // Forename contact
                    "Stephen",
                    // Surname contact
                    "Baker",
                    // Forename contact 2
                    "Gareth",
                    // Surname contact 2
                    "Baker",
                    // Pupil name
                    "Baker, Fiona",
                    // Title
                    "Rev",
                    // Gender
                    "Male",
                    // Priority
                    "2",
                    // Relationship
                    "Parent",
                    // Salutation,
                    "Prof Aaron",
                    // Addressee
                    "Prof J Aaron"
                }

            };
            return res;
        }

        public List<object[]> TC_PU031b_Data()
        {
            var randomPupil = "Luong" + SeleniumHelper.GenerateRandomString(5) + SeleniumHelper.GenerateRandomNumberUsingDateTime();
            var randomContact = "Luong" + SeleniumHelper.GenerateRandomString(4) + SeleniumHelper.GenerateRandomNumberUsingDateTime();

            var res = new List<Object[]>
            {
                new object[]
                {
                    // Forename pupil
                    randomPupil,
                    // Surname pupil
                    randomPupil,
                    // Full name contact 1
                    "Mr " + randomContact + " " + randomContact,
                    // Title Contact 1
                    "Mr",
                    // Forename contact 1
                    randomContact,
                    // Surname contact 1
                    randomContact,
                    // Gender contact 1
                    "Male"
                }

            };
            return res;
        }

        public List<object[]> TC_PU032_Data()
        {
            var randomContact1 = "Luong" + SeleniumHelper.GenerateRandomString(4) + SeleniumHelper.GenerateRandomNumberUsingDateTime();
            var randomContact2 = "Luong" + SeleniumHelper.GenerateRandomString(6) + SeleniumHelper.GenerateRandomNumberUsingDateTime();

            var res = new List<Object[]>
            {
                new object[]
                {
                    // Forename pupil
                    "Laura", 
                    // Surname pupil
                    "Adams",
                    // Title 1,
                    "Rev",
                    // Forename 1
                    randomContact1,
                    // Surname 1
                    randomContact1,
                    // Title 2,
                    "Mr",
                    // Forename 2
                    randomContact2,
                    // Surname 2
                    randomContact2,
                    // Forename pupil 2
                    "Carlton", 
                    // Surname pupil 2
                    "Allcroft",

                }

            };
            return res;
        }

        public List<object[]> TC_PU034_Data()
        {
            var res = new List<Object[]>
            {
                new object[]
                {
                    // pupil name,
                    "Bains, Kirk",
                    // Forename pupil
                    "Bains", 
                    // Surname pupil
                    "Kirk"
                }

            };
            return res;
        }

        public List<object[]> TC_PU036a_Data()
        {
            var randomName = "Luong" + SeleniumHelper.GenerateRandomString(4) + SeleniumHelper.GenerateRandomNumberUsingDateTime();

            var res = new List<Object[]>
            {
                new object[]
                {
                    // forename,
                    randomName,
                    // surname
                    randomName
                }

            };
            return res;
        }

        public List<object[]> TC_PU036b_Data()
        {
            var randomName = "Luong" + SeleniumHelper.GenerateRandomString(6) + SeleniumHelper.GenerateRandomNumberUsingDateTime();

            var res = new List<Object[]>
            {
                new object[]
                {
                    // forename,
                    randomName,
                    // surname
                    randomName
                }

            };
            return res;
        }

        public List<object[]> TC_PU036c_Data()
        {
            var randomName = "Luong" + SeleniumHelper.GenerateRandomString(4) + SeleniumHelper.GenerateRandomNumberUsingDateTime();

            var res = new List<Object[]>
            {
                new object[]
                {
                    // forename,
                    randomName,
                    // surname
                    randomName
                }

            };
            return res;
        }

        public List<object[]> TC_PU037_Data()
        {
            var res = new List<Object[]>
            {
                new object[]
                {
                    // pupil name,
                    "Bains, Kirk",
                    // fore name
                    "Bains"
                }

            };
            return res;
        }

        public List<object[]> TC_PU038_Data()
        {
            var res = new List<Object[]>
            {
                new object[]
                {
                    // pupil name,
                    "Bains, Kirk",
                    // fore name
                    "Bains"
                }

            };
            return res;
        }

        public List<object[]> TC_PU039_Data()
        {
            var res = new List<Object[]>
            {
                new object[]
                {
                    // pupil name,
                    "Bains, Kirk",
                    // fore name
                    "Bains"
                }

            };
            return res;
        }

        public List<object[]> TC_PU040_Data()
        {
            var res = new List<Object[]>
            {
                new object[]
                {
                    // pupil name,
                    "Bains, Kirk"
                }

            };
            return res;
        }

        public List<object[]> TC_PU041_Data()
        {
            var res = new List<Object[]>
            {
                new object[]
                {
                    // pupil name,
                    "Bains, Kirk"
                }

            };
            return res;
        }

        public List<object[]> TC_PU043_DATA()
        {
            string pattern = "M/d/yyyy";
            string surName = String.Format("Logigear_{0}", SeleniumHelper.GenerateRandomString(8));
            string foreName = SeleniumHelper.GenerateRandomString(8);
            string pupilName = String.Format("{0}, {1}", surName, foreName);
            string dateOfBirth = DateTime.ParseExact("10/16/2006", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            string DateOfAdmission = DateTime.ParseExact("10/19/2010", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            string dateOfLeaving = SeleniumHelper.GetDateFromDayOfWeek((int)DayOfWeek.Monday);

            var data = new List<Object[]>
                {
                    new object[] {surName, foreName, "Male", dateOfBirth, DateOfAdmission, "Year 2", "SEN", pupilName, dateOfLeaving, "Not Known", "Outside Local District"}

                };
            return data;
        }

        public List<object[]> TC_PU044A_DATA()
        {
            string pattern = "M/d/yyyy";
            string surName = String.Format("Logigear_{0}", SeleniumHelper.GenerateRandomString(8));
            string foreName = SeleniumHelper.GenerateRandomString(8);
            string pupilName = String.Format("{0}, {1}", surName, foreName);
            string dateOfBirth = DateTime.ParseExact("10/16/2006", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            string DateOfAdmission = DateTime.ParseExact("10/19/2010", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            string date = SeleniumHelper.GetDayAfter(SeleniumHelper.GetDateFromDayOfWeek((int)DayOfWeek.Monday, true), -7);

            var data = new List<Object[]>
                {
                    new object[] {surName, foreName, "Male", dateOfBirth, DateOfAdmission, "Year 2", "SEN", pupilName, "Charles", "Jon",
                        "Man-Warning", "Deed Poll", date }

                };
            return data;
        }

        public List<object[]> TC_PU044B_DATA()
        {
            string pattern = "M/d/yyyy";
            string surName = String.Format("Logigear_{0}", SeleniumHelper.GenerateRandomString(8));
            string foreName = SeleniumHelper.GenerateRandomString(8);
            string pupilName = String.Format("{0}, {1}", surName, foreName);
            string dateOfBirth = DateTime.ParseExact("10/16/2006", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            string DateOfAdmission = DateTime.ParseExact("10/19/2010", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            string startDate = SeleniumHelper.GetDayAfter(SeleniumHelper.GetDateFromDayOfWeek((int)DayOfWeek.Monday, true), -14);
            string endDate = SeleniumHelper.GetDayAfter(SeleniumHelper.GetDateFromDayOfWeek((int)DayOfWeek.Monday, true), -7);



            var data = new List<Object[]>
                {
                    new object[] {surName, foreName, "Male", dateOfBirth, DateOfAdmission, "Year 2", "SEN", pupilName, startDate, endDate, "This is now a former address"}

                };
            return data;
        }

        public List<object[]> TC_PU044C_DATA()
        {

            string pattern = "M/d/yyyy";
            string surName = String.Format("Logigear_{0}", SeleniumHelper.GenerateRandomString(8));
            string foreName = SeleniumHelper.GenerateRandomString(8);
            string pupilName = String.Format("{0}, {1}", surName, foreName);
            string dateOfBirth = DateTime.ParseExact("10/16/2006", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            string DateOfAdmission = DateTime.ParseExact("10/19/2010", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            string registerDate = SeleniumHelper.GetDayAfter(SeleniumHelper.GetDateFromDayOfWeek((int)DayOfWeek.Monday, true), -7);
            string endDate = SeleniumHelper.GetDayAfter(SeleniumHelper.GetDateFromDayOfWeek((int)DayOfWeek.Friday, true), -7);

            var data = new List<Object[]>
                {
                    new object[] {surName, foreName, "Male", dateOfBirth, DateOfAdmission, pupilName, registerDate, endDate
                    , "SEN", "Year 2", "2", "4"}

                };
            return data;
        }

        public List<object[]> TC_PU044D_DATA()
        {
            string pattern = "M/d/yyyy";
            string surName = String.Format("Logigear_{0}", SeleniumHelper.GenerateRandomString(8));
            string foreName = SeleniumHelper.GenerateRandomString(8);
            string pupilName = String.Format("{0}, {1}", surName, foreName);
            string dateOfBirth = DateTime.ParseExact("10/16/2006", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            string DateOfAdmission = DateTime.ParseExact("10/19/2010", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            string startDate = SeleniumHelper.GetToDay();
            string endDate = SeleniumHelper.GetDayAfter(SeleniumHelper.GetToDay(), 1);

            var data = new List<Object[]>
                {
                    new object[] {surName, foreName, "Male", dateOfBirth, DateOfAdmission, "Year 2", "SEN", pupilName, "Baker, Jade",
                        "New Suspension", "Substance Abuse", startDate, endDate, "8:30 AM", "4:45 PM", "1"}

                };
            return data;
        }


        public List<object[]> TC_PU062_Data()
        {
            string pattern = "M/d/yyyy";
            string PupilDateOfBirth = DateTime.ParseExact("06/01/2006", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            string PupilDateOfAdmission = DateTime.ParseExact("06/01/2010", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            string foreName = String.Format("{0}_{1}", SeleniumHelper.GenerateRandomString(7), SeleniumHelper.GenerateRandomNumberUsingDateTime());
            string surName = String.Format("{0}_{1}", "Avn", SeleniumHelper.GenerateRandomString(8));
            var res = new List<Object[]>
            {
                new object[]
                {
                    foreName, surName, "Male", PupilDateOfBirth, PupilDateOfAdmission, "Year 2", "LGG Test",
                }

            };
            return res;
        }

        public List<object[]> TC_PU063_Data()
        {
            string pattern = "M/d/yyyy";
            string PupilDateOfBirth = DateTime.ParseExact("06/01/2006", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            string PupilDateOfAdmission = DateTime.ParseExact("06/01/2010", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            string startDate = SeleniumHelper.GetFirstDayOfWeek(DateTime.Now.Subtract(TimeSpan.FromDays(7))).ToString(pattern);
            string foreName = String.Format("{0}_{1}", SeleniumHelper.GenerateRandomString(7), SeleniumHelper.GenerateRandomNumberUsingDateTime());
            string surName = String.Format("{0}_{1}", "Avn", SeleniumHelper.GenerateRandomString(8));
            var res = new List<Object[]>
            {
                new object[] {foreName, surName, "Male", PupilDateOfBirth, PupilDateOfAdmission, "Year 2",
                    "In-school provision", startDate},
            };
            return res;
        }

        public List<object[]> TC_PU064_Data()
        {
            string pattern = "M/d/yyyy";
            string PupilDateOfBirth = DateTime.ParseExact("06/01/2006", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            string PupilDateOfAdmission = DateTime.ParseExact("06/01/2010", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            string startDate = SeleniumHelper.GetFirstDayOfWeek(DateTime.Now.Subtract(TimeSpan.FromDays(7))).ToString(pattern);
            string foreName = String.Format("{0}_{1}", SeleniumHelper.GenerateRandomString(7), SeleniumHelper.GenerateRandomNumberUsingDateTime());
            string surName = String.Format("{0}_{1}", "Avn", SeleniumHelper.GenerateRandomString(8));
            var res = new List<Object[]>
            {
                new object[] {foreName, surName, "Male", PupilDateOfBirth, PupilDateOfAdmission, "Year 2",
                    "Severe Learning Difficulties", startDate, "SLD Confirmed by family GP"},
            };
            return res;
        }

        public List<object[]> TC_PU065_Data()
        {
            string pattern = "M/d/yyyy";
            string PupilDateOfBirth = DateTime.ParseExact("06/01/2006", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            string PupilDateOfAdmission = DateTime.ParseExact("06/01/2010", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            string startDate = SeleniumHelper.GetFirstDayOfWeek(DateTime.Now.Subtract(TimeSpan.FromDays(7))).ToString(pattern);
            string endDate = DateTime.Now.Add(TimeSpan.FromDays(365)).ToString(pattern);
            string foreName = String.Format("{0}_{1}", SeleniumHelper.GenerateRandomString(7), SeleniumHelper.GenerateRandomNumberUsingDateTime());
            string surName = String.Format("{0}_{1}", "Avn", SeleniumHelper.GenerateRandomString(8));
            var res = new List<Object[]>
            {
                new object[] {foreName, surName, "Male", PupilDateOfBirth, PupilDateOfAdmission, "Year 2",
                    "Time in Specialist Unit", startDate, endDate, "Time in special unit approved by family GP"},
            };
            return res;
        }

        public List<object[]> TC_PU066a_Data()
        {
            string pattern = "M/d/yyyy";
            string PupilDateOfBirth = DateTime.ParseExact("06/01/2006", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            string PupilDateOfAdmission = DateTime.ParseExact("06/01/2010", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            string previousMonday = SeleniumHelper.GetFirstDayOfWeek(DateTime.Now.Subtract(TimeSpan.FromDays(7))).ToString(pattern);
            string startTime = DateTime.ParseExact("10:30", "h:mm", CultureInfo.InvariantCulture).ToString("h:mm tt");
            string foreName = String.Format("{0}_{1}", SeleniumHelper.GenerateRandomString(7), SeleniumHelper.GenerateRandomNumberUsingDateTime());
            string surName = String.Format("{0}_{1}", "Avn", SeleniumHelper.GenerateRandomString(8));
            int staff = SeleniumHelper.GenerateRandomNumber(15);
            var res = new List<Object[]>
            {
                new object[] {foreName, surName, "Male", PupilDateOfBirth, PupilDateOfAdmission, "Year 2",
                        "In-school provision",
                       "Initial", "Meeting Completed", previousMonday, startTime, "Math Room 11",
                       staff, "Test", "Test", "SEN Co-ordinator", previousMonday},
            };
            return res;
        }

        public List<object[]> TC_PU066b_Data()
        {
            string pattern = "M/d/yyyy";
            string PupilDateOfBirth = DateTime.ParseExact("06/01/2006", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            string PupilDateOfAdmission = DateTime.ParseExact("06/01/2010", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            string previousMonday = SeleniumHelper.GetFirstDayOfWeek(DateTime.Now.Subtract(TimeSpan.FromDays(7))).ToString(pattern);
            string foreName = String.Format("{0}_{1}", SeleniumHelper.GenerateRandomString(7), SeleniumHelper.GenerateRandomNumberUsingDateTime());
            string surName = String.Format("{0}_{1}", "Avn", SeleniumHelper.GenerateRandomString(8));
            var res = new List<Object[]>
            {
                new object[] {foreName, surName, "Male", PupilDateOfBirth, PupilDateOfAdmission, "Year 2",
                        "In-school provision",
                       previousMonday, previousMonday,
                       "ELB Agreed", "Proposed Statement made",
                       previousMonday, previousMonday},
            };
            return res;
        }

        public List<object[]> TC_PU067_Data()
        {
            string pattern = "M/d/yyyy";
            string PupilDateOfBirth = DateTime.ParseExact("06/01/2006", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            string PupilDateOfAdmission = DateTime.ParseExact("06/01/2010", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            string past3Month = DateTime.Now.Subtract(TimeSpan.FromDays(90)).ToString(pattern);
            string past2Month = DateTime.Now.Subtract(TimeSpan.FromDays(60)).ToString(pattern);
            string foreName = String.Format("{0}_{1}", SeleniumHelper.GenerateRandomString(7), SeleniumHelper.GenerateRandomNumberUsingDateTime());
            string surName = String.Format("{0}_{1}", "Avn", SeleniumHelper.GenerateRandomString(8));
            var res = new List<Object[]>
            {
                new object[] {
                    foreName, surName, "Male", PupilDateOfBirth, PupilDateOfAdmission, "Year 2",
                    new string[]{"Charlie", "Sage", "River", "Deed Poll", past3Month},
                    new string[]{"Taylor", "Logan", "Kerry", "Adoption", past2Month},
                    new string[]{"Jessie", "Casey", "Harley", "Marriage", past2Month}
                },
            };
            return res;
        }

        public List<object[]> TC_PU068_Data()
        {

            string pattern = "M/d/yyyy";
            string PupilDateOfBirth = DateTime.ParseExact("06/01/2006", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            string PupilDateOfAdmission = DateTime.ParseExact("06/01/2010", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            DateTime twoYearPast = SeleniumHelper.GetFirstDayOfWeek(DateTime.Now.Subtract(TimeSpan.FromDays(730)));
            string endGuest = twoYearPast.Subtract(TimeSpan.FromDays(1)).ToString(pattern);
            string startSub = twoYearPast.ToString(pattern);
            string endSub = twoYearPast.Add(TimeSpan.FromDays(1)).ToString(pattern);
            string startMain = twoYearPast.Add(TimeSpan.FromDays(2)).ToString(pattern);
            string endMain = twoYearPast.Add(TimeSpan.FromDays(20)).ToString(pattern);
            string startSingle = twoYearPast.Add(TimeSpan.FromDays(21)).ToString(pattern);
            string foreName = String.Format("{0}_{1}", SeleniumHelper.GenerateRandomString(7), SeleniumHelper.GenerateRandomNumberUsingDateTime());
            string surName = String.Format("{0}_{1}", "Avn", SeleniumHelper.GenerateRandomString(8));
            var res = new List<Object[]>
            {
                new object[] {
                    foreName, surName, "Male", PupilDateOfBirth, PupilDateOfAdmission, "Year 2",
                     endGuest, startSub, endSub, startMain, endMain, startSingle,
                }
            };
            return res;
        }

        public List<object[]> TC_PU069_Data()
        {
            var code = String.Format("{0}_{1}", SeleniumHelper.GenerateRandomString(4), SeleniumHelper.GenerateRandomNumber(999));
            var description = String.Format("{0}_{1}_{2}", "Description", SeleniumHelper.GenerateRandomString(10), SeleniumHelper.GenerateRandomNumberUsingDateTime());
            var displayOrder = SeleniumHelper.GenerateRandomNumberUsingDateTime();
            var res = new List<Object[]>
            {
                new object[] {code, description, displayOrder, "Physical"},
            };
            return res;
        }

        public List<object[]> TC_PU070_Data()
        {
            var code = String.Format("{0}_{1}", SeleniumHelper.GenerateRandomString(4), SeleniumHelper.GenerateRandomNumber(999));
            var description = String.Format("{0}_{1}_{2}", "Description", SeleniumHelper.GenerateRandomString(10), SeleniumHelper.GenerateRandomNumberUsingDateTime());
            var displayOrder = SeleniumHelper.GenerateRandomNumberUsingDateTime();
            var res = new List<Object[]>
            {
                new object[] {code, description, displayOrder},
            };
            return res;
        }

        public List<object[]> TC_PU071_Data()
        {
            var code = String.Format("{0}_{1}", "11", SeleniumHelper.GenerateRandomNumber(999));
            var description = String.Format("{0}_{1}_{2}", "Description", SeleniumHelper.GenerateRandomString(10), SeleniumHelper.GenerateRandomNumberUsingDateTime());
            var displayOrder = SeleniumHelper.GenerateRandomNumberUsingDateTime();
            var res = new List<Object[]>
            {
                new object[] {code, description, displayOrder, "In-school provision"},
            };
            return res;
        }

        public List<object[]> TC_PU072_Data()
        {
            string pupilName = String.Format("PU072_{0}_{1}", SeleniumHelper.GenerateRandomString(10), SeleniumHelper.GenerateRandomNumberUsingDateTime(5));

            var res = new List<Object[]>
            {
                new object[] {
                    new string[]{pupilName, pupilName,
                                "Female", DateTime.ParseExact("1/1/2000", "M/d/yyyy", CultureInfo.InvariantCulture).ToString("M/d/yyyy"),
                                DateTime.ParseExact("10/10/2013", "M/d/yyyy", CultureInfo.InvariantCulture).ToString("M/d/yyyy"), "Guest Pupil", "Year 1"},
                    //SenStage name
                    "In-school provision",
                    //SenNeeds
                    new string[]{"Mild Learning Difficulties", "Dangerous Fluid Loss", DateTime.Today.ToString("M/d/yyyy"), (new DateTime(DateTime.Today.Year, 12, 25)).ToString("M/d/yyyy") },
                    //Provisions
                    new string[]{"IT Provision", (new DateTime(DateTime.Today.Year, 12, 25)).ToString("M/d/yyyy") },
                },
            };
            return res;
        }

        public List<object[]> TC_PU073_Data()
        {
            string pupilName = String.Format("PU073_{0}_{1}", SeleniumHelper.GenerateRandomString(10), SeleniumHelper.GenerateRandomNumberUsingDateTime(5));
            var res = new List<Object[]>
            {
                new object[] {
                    new string[]{pupilName, pupilName,
                                "Female", DateTime.ParseExact("1/1/2000", "M/d/yyyy", CultureInfo.InvariantCulture).ToString("M/d/yyyy"),
                                DateTime.ParseExact("10/10/2013", "M/d/yyyy", CultureInfo.InvariantCulture).ToString("M/d/yyyy"), "Guest Pupil", "Year 1"},
                    //SenStage name
                    "In-school provision",
                    //SenNeeds
                    new string[]{"Mild Learning Difficulties", "Dangerous Fluid Loss", DateTime.Today.ToString("M/d/yyyy"), (new DateTime(DateTime.Today.Year, 12, 25)).ToString("M/d/yyyy") },
                    //Provisions
                    new string[]{"IT Provision", (new DateTime(DateTime.Today.Year, 12, 25)).ToString("M/d/yyyy") },
                },
            };
            return res;
        }

        public List<object[]> TC_PU078_Data()
        {
            string pupilName = String.Format("PU078_{0}_{1}", SeleniumHelper.GenerateRandomString(10), SeleniumHelper.GenerateRandomNumberUsingDateTime(5));
            var res = new List<Object[]>
            {
                new object[] {
                    new string[]{pupilName, pupilName,
                                "Female", DateTime.ParseExact("1/1/2000", "M/d/yyyy", CultureInfo.InvariantCulture).ToString("M/d/yyyy"),
                                DateTime.ParseExact("10/10/2013", "M/d/yyyy", CultureInfo.InvariantCulture).ToString("M/d/yyyy"), "Guest Pupil", "Year 1"},
                    SeleniumHelper.GetDateFromDayOfWeek((int)DayOfWeek.Friday), "Not Known", "Elective Home Education"
                },
            };
            return res;
        }

        public List<object[]> TC_PU079_Data()
        {
            string pupilName = String.Format("PU079_{0}_{1}", SeleniumHelper.GenerateRandomString(10), SeleniumHelper.GenerateRandomNumberUsingDateTime(5));

            var res = new List<Object[]>
            {
                new object[] {
                    new string[]{pupilName, pupilName,
                                "Female", DateTime.ParseExact("1/1/2000", "M/d/yyyy", CultureInfo.InvariantCulture).ToString("M/d/yyyy"),
                                DateTime.ParseExact("10/10/2013", "M/d/yyyy", CultureInfo.InvariantCulture).ToString("M/d/yyyy"), "Guest Pupil", "Year 1"},
                    new string[]{DateTime.ParseExact("12/12/2013", "M/d/yyyy", CultureInfo.InvariantCulture).ToString("M/d/yyyy"), "Not Known", "Elective Home Education"},
                    new string[]{DateTime.ParseExact("2/12/2014", "M/d/yyyy", CultureInfo.InvariantCulture).ToString("M/d/yyyy"), "Subsidiary – Dual Registration", "Year 1"},
                    new string[]{DateTime.ParseExact("4/30/2014", "M/d/yyyy", CultureInfo.InvariantCulture).ToString("M/d/yyyy"), "Not Known", "Elective Home Education"},
                    new string[]{DateTime.ParseExact("5/8/2014", "M/d/yyyy", CultureInfo.InvariantCulture).ToString("M/d/yyyy"), "Main – Dual Registration", "Year 1"},
                    new string[]{DateTime.ParseExact("2/01/2015", "M/d/yyyy", CultureInfo.InvariantCulture).ToString("M/d/yyyy"), "Single Registration",
                                 DateTime.ParseExact("2/02/2015", "M/d/yyyy", CultureInfo.InvariantCulture).ToString("M/d/yyyy")}
                },
            };
            return res;
        }

        public List<object[]> TC_PU081_Data()
        {
            string pupilName = String.Format("PU081_{0}_{1}", SeleniumHelper.GenerateRandomString(10), SeleniumHelper.GenerateRandomNumberUsingDateTime(5));
            var res = new List<Object[]>
            {
                new object[] {
                    new string[]{pupilName, pupilName,
                                "Female", DateTime.ParseExact("1/1/2000", "M/d/yyyy", CultureInfo.InvariantCulture).ToString("M/d/yyyy"),
                                DateTime.ParseExact("10/10/2013", "M/d/yyyy", CultureInfo.InvariantCulture).ToString("M/d/yyyy"), "Guest Pupil", "Year 1"},
                    new string[]{"Family", "Family Documents", "document.txt"},
                    new string[]{"Welfare", "Welfare Documents", "document_1.txt"},
                    new string[]{"Consent", "Consent Documents", "document_2.txt"}
                },
            };
            return res;
        }

        public List<object[]> TC_PU087_Data()
        {
            var res = new List<Object[]>
            {
                new object[] {
                    "3A"
                },
            };
            return res;
        }

        public List<object[]> TC_PU088_Data()
        {
            var res = new List<Object[]>
            {
                new object[] {
                    "Year 1"
                },
            };
            return res;
        }

        public List<object[]> TC_PU089_Data()
        {
            var res = new List<Object[]>
            {
                new object[] {
                    "1A", "Year 1"
                },
            };
            return res;
        }

        public List<object[]> TC_PU090_Data()
        {
            var res = new List<Object[]>
            {
                new object[] {
                    "2A", "Year 2"
                },
            };
            return res;
        }


        #endregion

        #endregion

    }
}
