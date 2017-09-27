using System;
using System.Collections.Generic;
using System.Globalization;
using System.IdentityModel.Tokens;
using System.Linq;
using System.Runtime.Serialization;
using Pupil.Components.DataEntityIO;
using Pupil.Components.PupilQueries;
using SharedComponents.BaseFolder;
using TestSettings;

namespace Pupil.Components
{
    public class PupilDetails : BaseSeleniumComponents
    {
        public static DataEntityDTO EntitySettings(DataEntityDTO entity, Guid ID, int reference, ExtensionDataObject extensionData)
        {
            entity.ReferenceID = reference;
            entity.ID = ID;
            entity.DataModelContextID = "sims8_team1";
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
                DataModelContextID = "sims8_team1",
                DataModelType = new DataEntityDTO.DataModelTypeDTO
                {
                    SchemaName = "dbo",
                    DataModelPurpose = "BusinessDataModel",
                    ExtensionData = extensionData
                },
                ExtensionData = extensionData,
                Values = new Dictionary<string, DataEntityDTO.SimplePropertyDTO>()
            };

            return entity;
        }

        public static List<Guid> AddPupil(string legalForename, string legalSurname, Date dateOfBirth, string genderCode)
        {
            //New Instance of PupilQuery
            PupilQuery pupilQuery = new PupilQuery();

            //Security Token
            SecurityToken sessionToken = pupilQuery.Login(TestDefaults.Default.SchoolAdministrator,
                TestDefaults.Default.SchoolAdministratorPassword,
                TestDefaults.Default.SchoolID,
                TestDefaults.Default.TenantId.ToString(CultureInfo.InvariantCulture),
                Configuration.GetSutUrl() + TestDefaults.Default.ApplicationServerPath,
                Configuration.GetSutUrl() + TestDefaults.Default.SecurityServerPath);

            //Retrieved collection of Pupil Records
            DataEntityCollectionDTO pupilCollection = pupilQuery.RetrieveEntityByNameQuery("SIMS8PupilSearchQuery", new Dictionary<string, object>(), sessionToken);

            int pupilCollectionCount = pupilCollection.DataEntityDtos.Count;

            Guid pupilID = Guid.NewGuid();

            //Retrieve collection of Genders
            DataQuery genderQuery = CreateStandardQuery("Gender", "dbo");
            genderQuery.SelectedElements = new Dictionary<string, QuerySelectionElementBase>
            {
                { "Gender.Code", new QuerySelectionField { SymbolName = "Gender.Code" } }
            };
            genderQuery.Filter = new AndExpression
            {
                Expressions = new List<object> {
                    new EqualsExpression
                    {
                        CurrentValue = new ParameterValue
                        {
                            SymbolName = "Gender.Code",
                            SymbolTypeName = "iSIMS.Common.DataTypes.String"
                        },
                       TestValue = new InputParameterValue
                        {
                            SymbolName = "GenderCode", 
                            SymbolTypeName = typeof(string).FullName
                        }
                    }
                }
            };

            var parameters = new Dictionary<string, object>()
            {
                { "GenderCode", genderCode }
            };

            DataEntityCollectionDTO genderCollection = pupilQuery.RetrieveEntityByQuery(genderQuery, parameters, sessionToken);
            DataEntityDTO gender = genderCollection.DataEntityDtos.FirstOrDefault().Value;

            //Create the Pupil Record
            DataEntityDTO pupil = pupilQuery.CreatePupil(pupilID,
                legalForename,
                legalSurname,
                dateOfBirth,
                gender.ID,
                Guid.Parse(TestDefaults.Default.SchoolID),
                pupilCollectionCount,
                pupilCollection.ExtensionData);

            //Add the Pupil Record to the collection
            pupilCollection.DataEntityDtos.Add(pupilCollectionCount, pupil);
            pupilCollection.TopLevelDtoIDs.Add(pupilCollectionCount);

            //Save Scope for Pupil 
            List<string> pupilSaveScope = new List<string>
            {
                "Learner.ID",           
                "Learner.LegalForename",
                "Learner.LegalSurname",
                "Learner.DateOfBirth",
                "Learner.Gender",
                "Learner.School"
            };

            //Save the modified collection
            pupilQuery.Save(pupilCollection, sessionToken, Configuration.GetSutUrl() + TestDefaults.Default.ApplicationServerPath, pupilSaveScope);

            return new List<Guid> { pupilID };
        }

        public static Guid AddStandardPupilLogNote(Guid learnerID, string title, string noteText, string categoryCode, bool pinned, string userName = "SchoolAdmin")
        {
            //New Instance of PupilQuery
            PupilQuery pupilQuery = new PupilQuery();

            //Security Token
            SecurityToken sessionToken = pupilQuery.Login(TestDefaults.Default.SchoolAdministrator,
                TestDefaults.Default.SchoolAdministratorPassword,
                TestDefaults.Default.SchoolID,
                TestDefaults.Default.TenantId.ToString(CultureInfo.InvariantCulture),
                Configuration.GetSutUrl() + TestDefaults.Default.ApplicationServerPath,
                Configuration.GetSutUrl() + TestDefaults.Default.SecurityServerPath);

            //Retrieve collection of Pupil Log Notes
            DataQuery noteQuery = CreateStandardQuery("PupilLogNoteStandard", "dbo");
            noteQuery.SelectedElements = new Dictionary<string, QuerySelectionElementBase>
            {
                {"PupilLogNoteStandard.Title", new QuerySelectionField {SymbolName = "PupilLogNoteStandard.Title"}},
                {"PupilLogNoteStandard.NoteText", new QuerySelectionField {SymbolName = "PupilLogNoteStandard.NoteText"}},
                {"PupilLogNoteStandard.Pinned", new QuerySelectionField {SymbolName = "PupilLogNoteStandard.Pinned"}},
                {"PupilLogNoteStandard.CreatedOn", new QuerySelectionField {SymbolName = "PupilLogNoteStandard.CreatedOn"}},
                {"PupilLogNoteStandard.CreatedByUserId", new QuerySelectionField {SymbolName = "PupilLogNoteStandard.CreatedByUserId"}},
                {"PupilLogNoteStandard.Learner", new QuerySelectionField {SymbolName = "PupilLogNoteStandard.Learner"}},
                {"PupilLogNoteStandard.PupilLogNoteCategory", new QuerySelectionField {SymbolName = "PupilLogNoteStandard.PupilLogNoteCategory"}}
            };
            noteQuery.Filter = new AndExpression { Expressions = new List<object>() };
            var parameters = new Dictionary<string, object>();
            DataEntityCollectionDTO noteCollection = pupilQuery.RetrieveEntityByQuery(noteQuery, parameters, sessionToken);
            
            var category = GetNoteCategory(pupilQuery, sessionToken, categoryCode);
            var createdByUser = GetUser(pupilQuery, sessionToken, userName);
            int pupilLogNoteCollectionCount = noteCollection.DataEntityDtos.Count; 
            
            Guid pupilLogNoteID = Guid.NewGuid();

            //Create the PupilLog Note
            DataEntityDTO pupilLogNote = pupilQuery.CreateStandardPupilLogNote(pupilLogNoteID,
                title,
                noteText,
                learnerID,
                category.ID,
                createdByUser.ID,
                pinned,
                category.ReferenceID,
                pupilLogNoteCollectionCount,
                noteCollection.ExtensionData);

            noteCollection.DataEntityDtos.Add(pupilLogNoteCollectionCount, pupilLogNote);
            noteCollection.TopLevelDtoIDs.Add(pupilLogNoteCollectionCount);

            //Save Scope for Pupil Log Note
            List<string> pupilLogNoteSaveScope = new List<string>
            {
                "PupilLogNoteStandard.ID",
                "PupilLogNoteStandard.Title",
                "PupilLogNoteStandard.NoteText",
                "PupilLogNoteStandard.Pinned",
                "PupilLogNoteStandard.CreatedOn",
                "PupilLogNoteStandard.CreatedByUserId",
                "PupilLogNoteStandard.Learner",
                "PupilLogNoteStandard.PupilLogNoteCategory"
            };

            //Save the modified collection
            pupilQuery.Save(noteCollection, sessionToken, Configuration.GetSutUrl() + TestDefaults.Default.ApplicationServerPath, pupilLogNoteSaveScope);

            return pupilLogNoteID;
        }

        private static DataEntityDTO GetNoteCategory(PupilQuery pupilQuery, SecurityToken sessionToken, string categoryCode)
        {
            //Retrieve collection of Pupil Log Note Categories
            DataQuery categoryQuery = CreateStandardQuery("PupilLogNoteCategory", "dbo");
            categoryQuery.SelectedElements = new Dictionary<string, QuerySelectionElementBase>
            {
                {"PupilLogNoteCategory.Code", new QuerySelectionField {SymbolName = "PupilLogNoteCategory.Code"}}
            };
            categoryQuery.Filter = new AndExpression
            {
                Expressions = new List<object> {
                    new EqualsExpression
                    {
                        CurrentValue = new ParameterValue
                        {
                            SymbolName = "PupilLogNoteCategory.Code",
                            SymbolTypeName = "iSIMS.Common.DataTypes.String"
                        },
                        TestValue = new InputParameterValue
                        {
                            SymbolName = "CategoryCode", 
                            SymbolTypeName = typeof(string).FullName
                        }
                    }
                }
            };

            var parameters = new Dictionary<string, object>()
            {
                { "CategoryCode", categoryCode }
            };
            DataEntityCollectionDTO categoryCollection = pupilQuery.RetrieveEntityByQuery(categoryQuery, parameters, sessionToken);
            return categoryCollection.DataEntityDtos.FirstOrDefault().Value;
        }

        private static DataEntityDTO GetUser(PupilQuery pupilQuery, SecurityToken sessionToken, string userName)
        {
            //Retrieve collection of Authorised Users
            DataQuery userQuery = CreateStandardQuery("AuthorisedUser", "app");
            userQuery.SelectedElements = new Dictionary<string, QuerySelectionElementBase>
            {
                {"AuthorisedUser.UserName", new QuerySelectionField {SymbolName = "AuthorisedUser.UserName"}}
            };
            userQuery.Filter = new AndExpression
            {
                Expressions = new List<object> {
                    new EqualsExpression
                    {
                        CurrentValue = new ParameterValue
                        {
                            SymbolName = "AuthorisedUser.UserName",
                            SymbolTypeName = "iSIMS.Common.DataTypes.String"
                        },
                        TestValue = new InputParameterValue
                        {
                            SymbolName = "UserName", 
                            SymbolTypeName = typeof(string).FullName
                        }
                    }
                }
            };

            var parameters = new Dictionary<string, object>()
            {
                { "UserName", userName }
            };
            DataEntityCollectionDTO userCollection = pupilQuery.RetrieveEntityByQuery(userQuery, parameters, sessionToken);
            return userCollection.DataEntityDtos.FirstOrDefault().Value;
        }

        private static DataQuery CreateStandardQuery(string entityName, string schemaName)
        {
            var query = new DataQuery
            {
                EntityName = entityName,
                CountQuery = false,
                DataModelType = new enumDataModelType { SchemaName = schemaName, DataModelPurpose = enumDataModelPurpose.BusinessDataModel },
                DataQueryType = DataQueryTypeEnum.Query,
                DocumentContentFields = null,
                IgnoreNullParameters = true,
                PermitDatabaseTypeCasting = false,
                ProceduralInputParameters = null,
                ProcedureBehaviour = null,
                QueryName = "QRY-Get" + entityName,
                QueryRowLimit = 0
            };

            return query;
        }
    }
}