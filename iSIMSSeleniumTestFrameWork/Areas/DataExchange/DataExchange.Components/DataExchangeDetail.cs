using System;
using System.Collections.Generic;
using System.Globalization;
using System.IdentityModel.Tokens;
using System.Linq;
using System.Runtime.Serialization;
using DataExchange.Components.Common;
using GetDataViaWebServices.DataEntityIO;
using OpenQA.Selenium.Support.PageObjects;
using SharedComponents.BaseFolder;
using SharedComponents.CRUD;
using SharedComponents.Helpers;
using TestSettings;
using WebDriverRunner.webdriver;
using SeSugar.Data;

namespace DataExchange.Components
{
    /// <summary>
    /// Class is responsible for creating test data for pupil details
    /// </summary>
    public class DataExchangeDetail : BaseSeleniumComponents
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public DataExchangeDetail()
        {
            PageFactory.InitElements(WebContext.WebDriver, this);
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
        /// <summary>
        /// Add pupil data with SEN details
        /// </summary>
        /// <param name="legalForename"></param>
        /// <param name="legalSurname"></param>
        /// <param name="dateOfBirth"></param>
        /// <param name="genderCode"></param>
        /// <param name="UPN"></param>
        /// <param name="admissionno"></param>
        /// <param name="startDate"></param>
        /// <param name="senStatusCode"></param>
        /// <param name="senNeedTypeCode"></param>
        /// <param name="rank"></param>
        /// <param name="leavingDate"></param>
        /// <returns></returns>
        public static Guid AddPupilWithSEN(string legalForename, string legalSurname, string dateOfBirth, string genderCode, string shortNameYearGroup, string UPN, string admissionno, string startDate, string senStatusCode, string senNeedTypeCode, int rank, string shortNameNCYear, string enrolmentStatusCode, string leavingDate = null)
        {
            //New Instance of DataExchangeQuery
            DataExchangeQuery dataExchangeQuery = new DataExchangeQuery();

            string sql = "SELECT id FROM school WHERE Name ='" +  TestDefaults.Default.SchoolName.Replace("'","''") +"'";
            Guid schoolid = DataAccessHelpers.GetValue<Guid>(sql);

            
            //Security Token
            SecurityToken sessionToken = dataExchangeQuery.Login(TestDefaults.Default.TestUser,
                TestDefaults.Default.TestUserPassword,
                schoolid.ToString(),
                TestDefaults.Default.TenantId.ToString(CultureInfo.InvariantCulture),
                Configuration.GetSutUrl() + TestDefaults.Default.ApplicationServerPath,
                Configuration.GetSutUrl() + TestDefaults.Default.SecurityServerPath);

            //create collection of Pupil Records
            DataEntityCollectionDTO pupilCollection = new DataEntityCollectionDTO();
            pupilCollection.DataEntityDtos = new Dictionary<int, DataEntityDTO>();
            pupilCollection.TopLevelDtoIDs = new List<int>();
           
            int pupilCollectionCount = pupilCollection.DataEntityDtos.Count;

            Guid pupilID = Guid.NewGuid();
            Guid learnerSENStatusID = Guid.NewGuid();
            Guid learnerSENneedTypeID = Guid.NewGuid();
            Guid learnerEnrolmentID = Guid.NewGuid();
            Guid learnerNCyearID = Guid.NewGuid();
            Guid learneryearID = Guid.NewGuid();
            Guid learnerenrolmentStatusID = Guid.NewGuid();

            //get basic details to save pupil
            DataEntityDTO gender = getGenderDetails(genderCode, sessionToken);
            DataEntityDTO schoolncyear = getSchoolNCyearDetails(shortNameNCYear, sessionToken);

            DataEntityDTO yeargroup = getyearGroupDetails(shortNameYearGroup, sessionToken);
            Guid yeargroupguid;
            if (yeargroup == null)
            {
                string yeargroupQuery = "SELECT TOP 1 ID FROM dbo.yeargroup WHERE school = '" + schoolid + "'";
                yeargroupguid = DataAccessHelpers.GetValue<Guid>(yeargroupQuery);
            }
            else
            {
                yeargroupguid = yeargroup.ID;
            }
            DataEntityDTO SENStatus = getSENStatusDetails(senStatusCode, sessionToken);
            DataEntityDTO needType = getNeedTypeDetails(senNeedTypeCode, sessionToken);
            DataEntityDTO learnerEnrolmentStatus = getEnrolmentStatus(enrolmentStatusCode, sessionToken);
            
            //Create the Pupil Record
            DataEntityDTO pupil = dataExchangeQuery.CreatePupil(pupilID,
                legalForename,
                legalSurname,
                dateOfBirth,
                gender.ID,
                yeargroupguid,
                UPN,
                admissionno,
                schoolid,
                pupilCollectionCount,
                pupilCollection.ExtensionData);

            //Add the Pupil Record to the collection
            pupilCollection.DataEntityDtos.Add(pupilCollectionCount, pupil);
            pupilCollection.TopLevelDtoIDs.Add(pupilCollectionCount);

            pupilCollectionCount = pupilCollection.DataEntityDtos.Count;

            DataEntityDTO enrolment = dataExchangeQuery.CreateLearnerEnrolment(learnerEnrolmentID,
             pupilID,
             startDate,
             leavingDate,
             schoolid,
             pupil.ReferenceID,
             pupilCollectionCount,
             pupilCollection.ExtensionData);

            //Add the enrolment Record to the collection
            pupilCollection.DataEntityDtos.Add(pupilCollectionCount, enrolment);
            pupilCollection.TopLevelDtoIDs.Add(pupilCollectionCount);

            //Add sen status and needtype as a reference property 
            DataEntityDTO.ReferencePropertyDTO learnerEnrolmentProperty = new DataEntityDTO.ReferencePropertyDTO
            {
                EntityPrimaryKey = learnerEnrolmentID,
                InternalReferenceID = (short?)pupilCollectionCount
            };


            pupilCollectionCount = pupilCollection.DataEntityDtos.Count;


            DataEntityDTO learnerEnrolmentStatusEntity = dataExchangeQuery.CreateLearnerEnrolmentStatus(learnerenrolmentStatusID,
            learnerEnrolmentID,
             learnerEnrolmentStatus.ID,
             startDate,
             leavingDate,
             pupil.ReferenceID,
             pupilCollectionCount,
             pupilCollection.ExtensionData);

            //Add the enrolment Record to the collection
            pupilCollection.DataEntityDtos.Add(pupilCollectionCount, learnerEnrolmentStatusEntity);
            pupilCollection.TopLevelDtoIDs.Add(pupilCollectionCount);

            //Add sen status and needtype as a reference property 
            DataEntityDTO.ReferencePropertyDTO learnerEnrolmentStatusProperty = new DataEntityDTO.ReferencePropertyDTO
            {
                EntityPrimaryKey = learnerenrolmentStatusID,
                InternalReferenceID = (short?)pupilCollectionCount
            };

           
            pupilCollectionCount = pupilCollection.DataEntityDtos.Count;


            //Create the learner year Record
            DataEntityDTO learnerYeargroup = dataExchangeQuery.CreateLearnerYearGroup(learneryearID,
                pupilID,
                startDate,
                yeargroupguid,
                pupil.ReferenceID,
                pupilCollectionCount,
                pupilCollection.ExtensionData);

            //Add the yeargroup Record to the collection
            pupilCollection.DataEntityDtos.Add(pupilCollectionCount, learnerYeargroup);
            pupilCollection.TopLevelDtoIDs.Add(pupilCollectionCount);


            DataEntityDTO.ReferencePropertyDTO learnerYearGroupProperty = new DataEntityDTO.ReferencePropertyDTO
            {
                EntityPrimaryKey = learneryearID,
                InternalReferenceID = (short?)pupilCollectionCount
            };

            pupilCollectionCount = pupilCollection.DataEntityDtos.Count;

            //Create the pupil ncyear Record
            DataEntityDTO learnerncyear = dataExchangeQuery.CreateLearnerNCYear(learnerNCyearID,
                pupilID,
                startDate,
                schoolncyear.ID,
                pupil.ReferenceID,
                pupilCollectionCount,
                pupilCollection.ExtensionData);

            //Add the nc year Record to the collection
            pupilCollection.DataEntityDtos.Add(pupilCollectionCount, learnerncyear);
            pupilCollection.TopLevelDtoIDs.Add(pupilCollectionCount);

            //Add sen status and needtype as a reference property 
            DataEntityDTO.ReferencePropertyDTO learnerNCYearProperty = new DataEntityDTO.ReferencePropertyDTO
            {
                EntityPrimaryKey = learnerNCyearID,
                InternalReferenceID = (short?)pupilCollectionCount
            };


            pupilCollectionCount = pupilCollection.DataEntityDtos.Count;

            //Create the SENnStatus Record
            DataEntityDTO learnerSenStatus = dataExchangeQuery.CreateSENStatus(learnerSENStatusID,
                SENStatus.ID,
                startDate,
                pupilID,
                pupil.ReferenceID,
                pupilCollectionCount,
                pupilCollection.ExtensionData);

            //Add sen status and needtype as a reference property 
            DataEntityDTO.ReferencePropertyDTO learnerSENStatusProperty = new DataEntityDTO.ReferencePropertyDTO
            {
                EntityPrimaryKey = learnerSENStatusID,
                InternalReferenceID = (short?)pupilCollectionCount
            };

            //Add the SEN details to the collection
            pupilCollection.DataEntityDtos.Add(pupilCollectionCount, learnerSenStatus);
            pupilCollection.TopLevelDtoIDs.Add(pupilCollectionCount);

            pupilCollectionCount = pupilCollection.DataEntityDtos.Count;

            //Create the SEN need type
            DataEntityDTO learnerSenNeedType = dataExchangeQuery.CreateSENSNeed(learnerSENneedTypeID,
                needType.ID,
                rank,
                startDate,
                pupilID,
                pupil.ReferenceID,
                pupilCollectionCount,
                pupilCollection.ExtensionData);

            //Add the Service Record to the collection
            pupilCollection.DataEntityDtos.Add(pupilCollectionCount, learnerSenNeedType);
            pupilCollection.TopLevelDtoIDs.Add(pupilCollectionCount);

            DataEntityDTO.ReferencePropertyDTO learnerSENSNeedProperty = new DataEntityDTO.ReferencePropertyDTO
            {
                EntityPrimaryKey = learnerSENneedTypeID,
                InternalReferenceID = (short?)pupilCollectionCount
            };

            DataEntityDTO.ReferencePropertyDTOArray learnerSENStatus = new DataEntityDTO.ReferencePropertyDTOArray
            {
                ReferenceProperties = new List<DataEntityDTO.ReferencePropertyDTO> { learnerSENStatusProperty }
            };


            DataEntityDTO.ReferencePropertyDTOArray learnerNeedType = new DataEntityDTO.ReferencePropertyDTOArray
            {
                ReferenceProperties = new List<DataEntityDTO.ReferencePropertyDTO> { learnerSENSNeedProperty }
            };

            DataEntityDTO.ReferencePropertyDTOArray enrolmentReference = new DataEntityDTO.ReferencePropertyDTOArray
            {
                ReferenceProperties = new List<DataEntityDTO.ReferencePropertyDTO> { learnerEnrolmentProperty }
            };

            DataEntityDTO.ReferencePropertyDTOArray enrolmentStatusReference = new DataEntityDTO.ReferencePropertyDTOArray
            {
                ReferenceProperties = new List<DataEntityDTO.ReferencePropertyDTO> { learnerEnrolmentStatusProperty }
            };


            DataEntityDTO.ReferencePropertyDTOArray ncyearReference = new DataEntityDTO.ReferencePropertyDTOArray
            {
                ReferenceProperties = new List<DataEntityDTO.ReferencePropertyDTO> { learnerNCYearProperty }
            };

            DataEntityDTO.ReferencePropertyDTOArray yearReference = new DataEntityDTO.ReferencePropertyDTOArray
            {
                ReferenceProperties = new List<DataEntityDTO.ReferencePropertyDTO> { learnerYearGroupProperty }
            };

            enrolment.Values.Add("MulitpleLearnerEnrolmentStatus", enrolmentStatusReference);

            //Add references in pupil entity
            pupil.Values.Add("LearnerEnrolments", enrolmentReference);
            //pupil.Values.Add("LearnerEnrolments.MulitpleLearnerEnrolmentStatus", enrolmentStatusReference);
            pupil.Values.Add("LearnerYearGroupMemberships", yearReference);
            pupil.Values.Add("LearnerNCYearMemberships", ncyearReference);
            pupil.Values.Add("LearnerSENStatuses", learnerSENStatus);
            pupil.Values.Add("LearnerSENNeedTypes", learnerNeedType);


            //Save Scope for Pupil 
            List<string> pupilSaveScope = new List<string>
            {
                "Learner.ID",           
                "Learner.LegalForename",
                "Learner.LegalSurname",
                "Learner.DateOfBirth",
                "Learner.Gender",
                "Learner.School",
                "Learner.AdmissionNumber",
                "Learner.UPN" ,
                "Learner.YearGroup",
                "Learner.LearnerEnrolments.DOA",
                "Learner.LearnerEnrolments.DOL",
                "Learner.LearnerEnrolments.School",
                "Learner.LearnerEnrolments.Learner",
                "Learner.LearnerEnrolments.MulitpleLearnerEnrolmentStatus.EnrolmentStatus",
                "Learner.LearnerEnrolments.MulitpleLearnerEnrolmentStatus.LearnerEnrolment",
                "Learner.LearnerEnrolments.MulitpleLearnerEnrolmentStatus.StartDate",
                "Learner.LearnerEnrolments.MulitpleLearnerEnrolmentStatus.EndDate",
                "Learner.LearnerYearGroupMemberships.StartDate",
                "Learner.LearnerYearGroupMemberships.Learner",
                "Learner.LearnerYearGroupMemberships.YearGroup",
                "Learner.LearnerNCYearMemberships.StartDate",
                "Learner.LearnerNCYearMemberships.Learner",
                "Learner.LearnerNCYearMemberships.SchoolNCYear",
                "Learner.LearnerSENStatuses.StartDate",
                 "Learner.LearnerSENStatuses.Learner",
                 "Learner.LearnerSENStatuses.SENStatus",
                 "Learner.LearnerSENNeedTypes.NeedType",
                 "Learner.LearnerSENNeedTypes.StartDate",
                 "Learner.LearnerSENNeedTypes.Rank",
                 "Learner.LearnerSENNeedTypes.Learner"
            };

            //Save the modified collection
            dataExchangeQuery.Save(pupilCollection, sessionToken, Configuration.GetSutUrl() + TestDefaults.Default.ApplicationServerPath, pupilSaveScope);

            return pupilID;
        }

        /// <summary>
        /// Get School NC year Details for pupil
        /// </summary>
        /// <param name="shortName"></param>
        /// <param name="sessionToken"></param>
        /// <returns></returns>
        private static DataEntityDTO getSchoolNCyearDetails(string shortName, SecurityToken sessionToken)
        {
            //New Instance of DataExchangeQuery
            DataExchangeQuery dataExchangeQuery = new DataExchangeQuery();

            DataQuery schoolNcYearQuery = CreateStandardQuery("SchoolNCYear", "dbo");
            schoolNcYearQuery.SelectedElements = new Dictionary<string, QuerySelectionElementBase>
            {
                { "SchoolNCYear.NCYear", new QuerySelectionField { SymbolName = "SchoolNCYear.NCYear" } }
            };
            schoolNcYearQuery.Filter = new AndExpression
            {
                Expressions = new List<object> {
                    new EqualsExpression
                    {
                        CurrentValue = new ParameterValue
                        {
                            SymbolName = "SchoolNCYear.NCYear.ShortName",
                            SymbolTypeName = "iSIMS.Common.DataTypes.String"
                        },
                       TestValue = new InputParameterValue()
                        {
                            SymbolName = "shortname", 
                            SymbolTypeName = typeof(string).FullName
                        }
                    }
                }
            };

            //schoolNcYearQuery.Filter = new AndExpression
            //{
            //    Expressions = new List<object> {
            //        new EqualsExpression
            //        {
            //            CurrentValue = new ParameterValue
            //            {
            //                SymbolName = "SchoolNCYear.School",
            //                SymbolTypeName = "iSIMS.Common.DataTypes.Guischoolring"
            //            },
            //           TestValue = new InputParameterValue
            //            {
            //                SymbolName = "schoolId", 
            //                SymbolTypeName = typeof(string).FullName
            //            }
            //        }
            //    }
            //};

            var parameters = new Dictionary<string, object>()
            {
                {
                    "Shortname", shortName
                }
            };

            DataEntityCollectionDTO schoolNCyearCollection = dataExchangeQuery.RetrieveEntityByQuery(schoolNcYearQuery, parameters, sessionToken);
            return schoolNCyearCollection.DataEntityDtos.FirstOrDefault().Value;
        }

        /// <summary>
        /// Get year group details
        /// </summary>
        /// <param name="shortName"></param>
        /// <param name="sessionToken"></param>
        /// <returns></returns>
        private static DataEntityDTO getyearGroupDetails(string shortName, SecurityToken sessionToken)
        {
            //New Instance of DataExchangeQuery
            DataExchangeQuery dataExchangeQuery = new DataExchangeQuery();

            DataQuery yearGroupQuery = CreateStandardQuery("YearGroup", "dbo");
            yearGroupQuery.SelectedElements = new Dictionary<string, QuerySelectionElementBase>
            {
                { "YearGroup.ShortName", new QuerySelectionField { SymbolName = "YearGroup.ShortName" } }
            };
            yearGroupQuery.Filter = new AndExpression
            {
                Expressions = new List<object> {
                    new EqualsExpression
                    {
                        CurrentValue = new ParameterValue
                        {
                            SymbolName = "YearGroup.ShortName",
                            SymbolTypeName = "iSIMS.Common.DataTypes.String"
                        },
                       TestValue = new InputParameterValue()
                        {
                            SymbolName = "ShortName", 
                            SymbolTypeName = typeof(string).FullName
                        }
                    }
                }
            };

            var parameters = new Dictionary<string, object>()
            {
                {
                    "ShortName", shortName
                }
            };

            DataEntityCollectionDTO yearGroupCollection = dataExchangeQuery.RetrieveEntityByQuery(yearGroupQuery, parameters, sessionToken);
            return yearGroupCollection.DataEntityDtos.FirstOrDefault().Value;
        }

        /// <summary>
        /// Get gender details
        /// </summary>
        /// <param name="genderCode"></param>
        /// <param name="sessionToken"></param>
        /// <returns></returns>
        private static DataEntityDTO getGenderDetails(string genderCode, SecurityToken sessionToken)
        {
            //New Instance of DataExchangeQuery
            DataExchangeQuery dataExchangeQuery = new DataExchangeQuery();
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

            DataEntityCollectionDTO genderCollection = dataExchangeQuery.RetrieveEntityByQuery(genderQuery, parameters, sessionToken);
            return genderCollection.DataEntityDtos.FirstOrDefault().Value;
        }

        /// <summary>
        /// Get SEN details
        /// </summary>
        /// <param name="SENStatusCode"></param>
        /// <param name="sessionToken"></param>
        /// <returns></returns>
        private static DataEntityDTO getSENStatusDetails(string SENStatusCode, SecurityToken sessionToken)
        {
            //New Instance of DataExchangeQuery
            DataExchangeQuery dataExchangeQuery = new DataExchangeQuery();
            //Retrieve collection of Genders
            DataQuery genderQuery = CreateStandardQuery("SENStatus", "dbo");
            genderQuery.SelectedElements = new Dictionary<string, QuerySelectionElementBase>
            {
                { "SENStatus.Code", new QuerySelectionField { SymbolName = "SENStatus.Code" } }
            };
            genderQuery.Filter = new AndExpression
            {
                Expressions = new List<object> {
                    new EqualsExpression
                    {
                        CurrentValue = new ParameterValue
                        {
                            SymbolName = "SENStatus.Code",
                            SymbolTypeName = "iSIMS.Common.DataTypes.String"
                        },
                       TestValue = new InputParameterValue
                        {
                            SymbolName = "SENStatusCode", 
                            SymbolTypeName = typeof(string).FullName
                        }
                    }
                }
            };

            var parameters = new Dictionary<string, object>()
            {
                { "SENStatusCode", SENStatusCode }
            };

            DataEntityCollectionDTO seStatusrCollection = dataExchangeQuery.RetrieveEntityByQuery(genderQuery, parameters, sessionToken);
            return seStatusrCollection.DataEntityDtos.FirstOrDefault().Value;
        }

        /// <summary>
        /// Get need type details
        /// </summary>
        /// <param name="needTypeCode"></param>
        /// <param name="sessionToken"></param>
        /// <returns></returns>
        private static DataEntityDTO getNeedTypeDetails(string needTypeCode, SecurityToken sessionToken)
        {
            //New Instance of DataExchangeQuery
            DataExchangeQuery dataExchangeQuery = new DataExchangeQuery();
            //Retrieve collection of Genders
            DataQuery genderQuery = CreateStandardQuery("SENNeedType", "dbo");
            genderQuery.SelectedElements = new Dictionary<string, QuerySelectionElementBase>
            {
                { "SENNeedType.Code", new QuerySelectionField { SymbolName = "SENNeedType.Code" } }
            };
            genderQuery.Filter = new AndExpression
            {
                Expressions = new List<object> {
                    new EqualsExpression
                    {
                        CurrentValue = new ParameterValue
                        {
                            SymbolName = "SENNeedType.Code",
                            SymbolTypeName = "iSIMS.Common.DataTypes.String"
                        },
                       TestValue = new InputParameterValue
                        {
                            SymbolName = "needTypeCode", 
                            SymbolTypeName = typeof(string).FullName
                        }
                    }
                }
            };

            var parameters = new Dictionary<string, object>()
            {
                { "needTypeCode", needTypeCode }
            };

            DataEntityCollectionDTO genderCollection = dataExchangeQuery.RetrieveEntityByQuery(genderQuery, parameters, sessionToken);
            return genderCollection.DataEntityDtos.FirstOrDefault().Value;
        }


        /// <summary>
        /// Get Learner Enrolment Status details
        /// </summary>
        /// <param name="enrolmentStatusCode"></param>
        /// <param name="sessionToken"></param>
        /// <returns></returns>
        private static DataEntityDTO getEnrolmentStatus(string enrolmentStatusCode, SecurityToken sessionToken)
        {
            //New Instance of DataExchangeQuery
            DataExchangeQuery dataExchangeQuery = new DataExchangeQuery();
            //Retrieve collection of Genders
            DataQuery enrolmentStatusQuery = CreateStandardQuery("EnrolmentStatus", "dbo");
            enrolmentStatusQuery.SelectedElements = new Dictionary<string, QuerySelectionElementBase>
            {
                { "EnrolmentStatus.Code", new QuerySelectionField { SymbolName = "EnrolmentStatus.Code" } }
            };
            enrolmentStatusQuery.Filter = new AndExpression
            {
                Expressions = new List<object> {
                    new EqualsExpression
                    {
                        CurrentValue = new ParameterValue
                        {
                            SymbolName = "EnrolmentStatus.Code",
                            SymbolTypeName = "iSIMS.Common.DataTypes.String"
                        },
                       TestValue = new InputParameterValue
                        {
                            SymbolName = "enrolmentStatusCode", 
                            SymbolTypeName = typeof(string).FullName
                        }
                    }
                }
            };

            var parameters = new Dictionary<string, object>()
            {
                { "enrolmentStatusCode", enrolmentStatusCode }
            };

            DataEntityCollectionDTO enrolmentStatusCollection = dataExchangeQuery.RetrieveEntityByQuery(enrolmentStatusQuery, parameters, sessionToken);
            return enrolmentStatusCollection.DataEntityDtos.FirstOrDefault().Value;
        }

        /// <summary>
        /// Delete pupil
        /// </summary>
        /// <param name="pupilID"></param>
        public static void DeletePupil(List<Guid> pupilIDs)
        {
            SeleniumHelper.NavigateMenu("Tasks", "Pupils", "Delete Pupil");
            WebContext.Screenshot();
            SeleniumHelper.FindAndClick(DataExchangeElement.PupilRecordLeaverCheckBox);
            SearchCriteria.Search();
            WebContext.Screenshot();
            foreach (Guid pupilID in pupilIDs)
            {
                SeleniumHelper.FindAndClick(DataExchangeElement.DeletePupilResult(TestDefaults.Default.Path,
                    pupilID.ToString()));
                WebContext.Screenshot();
                SeleniumHelper.FindAndClick(DataExchangeElement.PupilDeleteButton);
                WebContext.Screenshot();
                BaseSeleniumComponents.WaitForAndClick(BrowserDefaults.TimeOut,
                    DataExchangeElement.PupilConfirmationButton);
            }
            //SeleniumHelper.FindAndClick(DataExchangeElement.CloseTabButton);
            //WebContext.Screenshot();
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
