using System;
using System.Collections.Generic;
using System.Globalization;
using System.IdentityModel.Tokens;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading;
using GetDataViaWebServices.DataEntityIO;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using SharedComponents.BaseFolder;
using Staff.Components.StaffQueries;
using TestSettings;
using WebDriverRunner.webdriver;
using StaffHelper = SharedComponents.Helpers.SeleniumHelper;

namespace Staff.Components.StaffRecord
{
    public class StaffDetail : BaseSeleniumComponents
    {
        private const string _legalForenameBy = "[name='LegalForename']";
        private const string _legalSurnameBy = "[name='LegalSurname']";
        private const string _DOLBy = "[name=\"DateOfLeaving\"]";
        private const string _DOABy = "[name=\"DateOfArrival\"]";
        private const string _addServiceRecordButtonBy = "[title=\"Add Service Record\"]";

        private const string AbsencesAccordianPanelXPATH = "//*[text()='Absences']";
        private const string AddAbsenceButtonSelector = "button[title='Add New Linked Staff Absence']";

        private const string _staffLeaverContextualActionBy = "[data-loading-text=\"Staff Leaving Details\"]";

        private const string _saveButtonBy = "a[title=\"Save Record\"]";
        private const string _createButtonBy = "a[title=\"Create the Record\"]";

        [FindsBy(How = How.CssSelector, Using = _legalForenameBy)]
        public IWebElement LegalForeName;

        [FindsBy(How = How.CssSelector, Using = _legalSurnameBy)]
        public IWebElement LegalSurname;

        [FindsBy(How = How.CssSelector, Using = _DOABy)]
        public IWebElement DOA;

        [FindsBy(How = How.CssSelector, Using = _DOLBy)]
        public IWebElement DOL;

        [FindsBy(How = How.CssSelector, Using = _addServiceRecordButtonBy)]
        public IWebElement AddServiceRecordButton;

        [FindsBy(How = How.XPath, Using = AbsencesAccordianPanelXPATH)]
        public IWebElement AbsencesAccordianPanel;

        [FindsBy(How = How.CssSelector, Using = AddAbsenceButtonSelector)]
        public IWebElement AddAbsenceButton;

        [FindsBy(How = How.CssSelector, Using = _staffLeaverContextualActionBy)]
        public IWebElement StaffLeaverContextualAction;

        [FindsBy(How = How.CssSelector, Using = _saveButtonBy)]
        public IWebElement SaveButton;

        [FindsBy(How = How.CssSelector, Using = _createButtonBy)]
        public IWebElement CreateButton;

        public StaffDetail()
        {
            PageFactory.InitElements(WebContext.WebDriver, this);
        }

        public void ValidateElements()
        {
            Assert.IsTrue(LegalForeName.Displayed);
            Assert.IsTrue(AbsencesAccordianPanel.Displayed);
        }

        public void EnterLegalForename(string value)
        {
            LegalForeName = WaitForAndGet(By.CssSelector(_legalForenameBy));
            LegalForeName.Clear();
            LegalForeName.SendKeys(value);
        }

        public void EnterLegalSurname(string value)
        {
            LegalSurname = WaitForAndGet(By.CssSelector(_legalSurnameBy));
            LegalSurname.Clear();
            LegalSurname.SendKeys(value);
        }

        public StaffDetail Create()
        {
            CreateButton = WaitForAndGet(By.CssSelector(_createButtonBy));
            CreateButton.Click();
            return new StaffDetail();
        }

        public StaffDetail Save()
        {
            SaveButton = WaitForAndGet(By.CssSelector(_saveButtonBy));
            SaveButton.Click();
            return new StaffDetail();
        }

        public StaffServiceRecordScreen AddServiceRecord()
        {
            AddServiceRecordButton.Click();
            return new StaffServiceRecordScreen();
        }

        public StaffLeaverScreen SelectLeaverContextualAction()
        {
            StaffLeaverContextualAction = WaitForAndGet(By.CssSelector(_staffLeaverContextualActionBy));
            StaffLeaverContextualAction.Click();
            return new StaffLeaverScreen();
        }

        public static List<Guid> AddStaff(string legalForename, string legalSurname, string startDate, string endDate = null)
        {
            //New Instance of StaffQuery
            StaffQuery sq = new StaffQuery();

            //Security Token
            SecurityToken sessionToken = sq.Login(TestDefaults.Default.TestUser,
                TestDefaults.Default.TestUserPassword,
                TestDefaults.Default.SchoolID,
                TestDefaults.Default.TenantId.ToString(CultureInfo.InvariantCulture),
                Configuration.GetSutUrl() + TestDefaults.Default.ApplicationServerPath,
                Configuration.GetSutUrl() + TestDefaults.Default.SecurityServerPath);

            //Retrieved collection of Staff Records
            DataEntityCollectionDTO staffCollection = sq.RetrieveEntityByNameQuery("SIMS8StaffSearch", new Dictionary<string, object>(), sessionToken);

            int staffCollectionCount = staffCollection.DataEntityDtos.Count;

            Guid staffID = Guid.NewGuid();
            Guid staffServiceID = Guid.NewGuid();

            //Create the Staff Record
            DataEntityDTO staff = sq.CreateStaff(staffID,
                legalForename,
                legalSurname, 
                Guid.Parse(TestDefaults.Default.SchoolID), 
                staffCollectionCount, 
                staffCollection.ExtensionData);

            //Add the Staff Record to the collection
            staffCollection.DataEntityDtos.Add(staffCollectionCount, staff);
            staffCollection.TopLevelDtoIDs.Add(staffCollectionCount);

            staffCollectionCount = staffCollection.DataEntityDtos.Count;
            //Create the Service Record
            DataEntityDTO staffService = sq.CreateStaffService(staffServiceID,
                startDate,
                endDate,
                staffID,
                staff.ReferenceID,
                staffCollectionCount,
                staffCollection.ExtensionData);

            //Add the Service Record to the collection
            staffCollection.DataEntityDtos.Add(staffCollectionCount, staffService);
            staffCollection.TopLevelDtoIDs.Add(staffCollectionCount);

            //Add the Service Record to the Staff Record as a reference property 
            DataEntityDTO.ReferencePropertyDTO staffServiceProperty = new DataEntityDTO.ReferencePropertyDTO
            {
                EntityPrimaryKey = staffServiceID,
                InternalReferenceID = (short?)staffCollectionCount
            };

            DataEntityDTO.ReferencePropertyDTOArray staffServices = new DataEntityDTO.ReferencePropertyDTOArray
            {
                ReferenceProperties = new List<DataEntityDTO.ReferencePropertyDTO> {staffServiceProperty}
            };

            staff.Values.Add("StaffServiceRecords", staffServices);


            //Save Scope for Staff and Service Record
            List<string> staffSaveScope = new List<string>
            {
                "Staff",           
                "Staff.LegalForename",
                "Staff.LegalSurname",
                "Staff.School",
                "Staff.StaffServiceRecords",
                "StaffServiceRecord",
                "StaffServiceRecord.DOA",
                "StaffServiceRecord.DOL",
                "StaffServiceRecord.Staff"
            };

            //Save the modified collection
            sq.Save(staffCollection, sessionToken, Configuration.GetSutUrl() + TestDefaults.Default.ApplicationServerPath, staffSaveScope);

            return new List<Guid> {staffID, staffServiceID};
        }

        public static Guid AddService(Guid staffID, string startDate)
        {
            //New Instance of StaffQuery
            StaffQuery sq = new StaffQuery();

            //Security Token
            SecurityToken sessionToken = sq.Login(TestDefaults.Default.TestUser,
                TestDefaults.Default.TestUserPassword,
                TestDefaults.Default.SchoolID,
                TestDefaults.Default.TenantId.ToString(CultureInfo.InvariantCulture),
                Configuration.GetSutUrl() + TestDefaults.Default.ApplicationServerPath,
                Configuration.GetSutUrl() + TestDefaults.Default.SecurityServerPath);

            //Retrieved collection of Staff Records
            DataEntityCollectionDTO staffCollection = sq.RetrieveEntityById("Staff", staffID, sessionToken);

            DataEntityDTO staff = staffCollection.DataEntityDtos.FirstOrDefault().Value;

            int staffServiceCollectionCount = staffCollection.DataEntityDtos.Count;

            Guid staffServiceID = Guid.NewGuid();

            //Create the Service Record
            DataEntityDTO staffService = sq.CreateStaffService(staffServiceID, 
                startDate, 
                null,
                staffID, 
                staff.ReferenceID, 
                staffServiceCollectionCount, 
                staffCollection.ExtensionData);

            //Add the Service Record to the collection
            staffCollection.DataEntityDtos.Add(staffServiceCollectionCount, staffService);
            staffCollection.TopLevelDtoIDs.Add(staffServiceCollectionCount);

            //Add the Service Record to the Staff Record as a reference property 
            DataEntityDTO.ReferencePropertyDTO staffServiceProperty = new DataEntityDTO.ReferencePropertyDTO
            {
                EntityPrimaryKey = staffServiceID,
                InternalReferenceID = (short?)staffServiceCollectionCount
            };

            DataEntityDTO.ReferencePropertyDTOArray staffServices = new DataEntityDTO.ReferencePropertyDTOArray
            {
                ReferenceProperties = new List<DataEntityDTO.ReferencePropertyDTO> {staffServiceProperty}
            };

            staff.Values.Add("StaffServiceRecords", staffServices);

            //Save Scope for Service Record
            List<string> staffSaveScope = new List<string>
            {
                "Staff.StaffServiceRecords",
                "StaffServiceRecord",
                "StaffServiceRecord.DOA",
                "StaffServiceRecord.Staff"
            };

            //Save the modified collection
            sq.Save(staffCollection, sessionToken, Configuration.GetSutUrl() + TestDefaults.Default.ApplicationServerPath, staffSaveScope);

            return staffServiceID;
        }

        public static void DeleteStaff(Guid staffID)
        {
            // Delete
            StaffHelper.NavigateMenu("Tasks", "Staff", "Delete Staff Record");

            StaffHelper.FindAndClick(StaffElements.StaffRecord.DeleteStaffSearchButton);
            StaffHelper.FindAndClick(StaffElements.StaffRecord.DeleteStaffSearchResult(staffID.ToString()));
            StaffHelper.FindAndClick(StaffElements.StaffRecord.DeleteButton);
            StaffHelper.FindAndClick(StaffElements.StaffRecord.ConfirmDeleteButton);

            Thread.Sleep(5000);
            StaffHelper.FindAndClick(StaffElements.Tabs.CloseTabButton);
        }

        public static SecurityToken CreateSessionToken(StaffQuery sq)
        {
            SecurityToken sessionToken = sq.Login(TestDefaults.Default.TestUser,
                TestDefaults.Default.TestUserPassword,
                TestDefaults.Default.SchoolID,
                TestDefaults.Default.TenantId.ToString(CultureInfo.InvariantCulture),
                Configuration.GetSutUrl() + TestDefaults.Default.ApplicationServerPath,
                Configuration.GetSutUrl() + TestDefaults.Default.SecurityServerPath);

            return sessionToken;
        }

        public static DataEntityDTO EntitySettings(DataEntityDTO entity, Guid ID, int reference, ExtensionDataObject extensionData)
        {
            entity.ReferenceID = reference;
            entity.ID = ID;
            entity.DataModelContextID = "sims8";
            entity.DataModelType = new DataEntityDTO.DataModelTypeDTO
            {
                SchemaName = "dbo",
                DataModelPurpose = "BusinessDataModel",
                ExtensionData = extensionData
            };
            entity.ExtensionData = extensionData;

            return entity;
        }

        public static DataEntityDTO NewEntitySettings(Guid ID, int reference, ExtensionDataObject extensionData, string entityName)
        {
            DataEntityDTO entity = new DataEntityDTO
            {
                ReferenceID = reference,
                ID = ID,
                EntityName = entityName,
                DataModelContextID = "sims8",
                DataModelType = new DataEntityDTO.DataModelTypeDTO
                {
                    SchemaName = "dbo",
                    DataModelPurpose = "BusinessDataModel",
                    ExtensionData = extensionData
                },
                ExtensionData = extensionData,
                Values =  new Dictionary<string, DataEntityDTO.SimplePropertyDTO>()
            };

            return entity;
        }
    }
}
