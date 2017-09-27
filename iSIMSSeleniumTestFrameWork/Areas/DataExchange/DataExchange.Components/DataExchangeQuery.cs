using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using GetDataViaWebServices;
using GetDataViaWebServices.DataEntityIO;
using TestSettings;

namespace DataExchange.Components
{
   public class DataExchangeQuery : WebServiceBase
    {
        private readonly string _applicationServerAddress;

        public DataExchangeQuery()
        {
            _applicationServerAddress = Configuration.GetSutUrl() + TestDefaults.Default.ApplicationServerPath;
        }
        
       /// <summary>
       /// Create pupil data
       /// </summary>
       /// <param name="ID"></param>
       /// <param name="legalForename"></param>
       /// <param name="legalSurname"></param>
       /// <param name="dateOfBirth"></param>
       /// <param name="genderID"></param>
       /// <param name="UPN"></param>
       /// <param name="admissionNo"></param>
       /// <param name="schoolID"></param>
       /// <param name="referenceID"></param>
       /// <param name="extensionData"></param>
       /// <returns></returns>
        public DataEntityDTO CreatePupil(Guid ID,string legalForename,string legalSurname,string dateOfBirth,Guid genderID,Guid yeargroupID,string UPN,string admissionNo,Guid schoolID,int referenceID,ExtensionDataObject extensionData)
        {
            DataEntityDTO.SimplePropertyDTOGuid IDProperty = new DataEntityDTO.SimplePropertyDTOGuid { Value = ID };
            DataEntityDTO.SimplePropertyDTOString legalForenameProperty = new DataEntityDTO.SimplePropertyDTOString { Value = legalForename };
            DataEntityDTO.SimplePropertyDTOString legalSurnameProperty = new DataEntityDTO.SimplePropertyDTOString { Value = legalSurname };
            DataEntityDTO.SimplePropertyDTODate dateOfBirthProperty = new DataEntityDTO.SimplePropertyDTODate { Value = new Date { internalDateTime = DateTime.Parse(dateOfBirth) } };
            DataEntityDTO.ReferencePropertyDTO schoolProperty = new DataEntityDTO.ReferencePropertyDTO { EntityPrimaryKey = schoolID };
            DataEntityDTO.ReferencePropertyDTO genderProperty = new DataEntityDTO.ReferencePropertyDTO { EntityPrimaryKey = genderID };
            DataEntityDTO.ReferencePropertyDTO yeargroupProperty = new DataEntityDTO.ReferencePropertyDTO { EntityPrimaryKey = yeargroupID };
            DataEntityDTO.SimplePropertyDTOString admissionNoProperty = new DataEntityDTO.SimplePropertyDTOString{Value = admissionNo};
            DataEntityDTO.SimplePropertyDTOString UPNProperty = new DataEntityDTO.SimplePropertyDTOString{Value = UPN};

            DataEntityDTO entity = new DataEntityDTO
            {
                EntityName = "Learner",
                Values = new Dictionary<string, DataEntityDTO.SimplePropertyDTO>
                {
                    {"ID", IDProperty},
                    {"LegalForename", legalForenameProperty},
                    {"LegalSurname", legalSurnameProperty},
                    {"DateOfBirth", dateOfBirthProperty},
                    {"AdmissionNumber", admissionNoProperty},
                    {"UPN", UPNProperty},
                    {"Gender", genderProperty},
                    {"School", schoolProperty},
                    {"YearGroup", yeargroupProperty}
                }
            };

            return DataExchangeDetail.EntitySettings(entity, ID, referenceID, extensionData);
        }

       /// <summary>
       /// Create SEN Status entity
       /// </summary>
       /// <param name="ID"></param>
       /// <param name="SENStatusID"></param>
       /// <param name="startDate"></param>
       /// <param name="learnerID"></param>
       /// <param name="collectionCount"></param>
       /// <param name="referenceID"></param>
       /// <param name="extensionData"></param>
       /// <returns></returns>
        public DataEntityDTO CreateSENStatus(Guid ID, Guid SENStatusID, string startDate, Guid learnerID, int referenceID, int collectionCount, ExtensionDataObject extensionData)
        {

            DataEntityDTO.SimplePropertyDTOGuid IDProperty = new DataEntityDTO.SimplePropertyDTOGuid { Value = ID };
           DataEntityDTO.SimplePropertyDTODate StartDateProperty = new DataEntityDTO.SimplePropertyDTODate { Value = new Date { internalDateTime = DateTime.Parse(startDate) } };
            DataEntityDTO.ReferencePropertyDTO senStatusProperty = new DataEntityDTO.ReferencePropertyDTO { EntityPrimaryKey = SENStatusID };

            DataEntityDTO.ReferencePropertyDTO LearnerProperty = new DataEntityDTO.ReferencePropertyDTO
            {
                EntityPrimaryKey = learnerID,
                InternalReferenceID = (short?)collectionCount
            };

            DataEntityDTO entity = new DataEntityDTO
            {
                EntityName = "LearnerSENStatus",
                Values = new Dictionary<string, DataEntityDTO.SimplePropertyDTO>
                {
                    {"ID", IDProperty},
                    {"StartDate", StartDateProperty},
                    {"Learner", LearnerProperty},
                    {"SENStatus", senStatusProperty}
                }
            };

            
            return DataExchangeDetail.EntitySettings(entity, ID, referenceID, extensionData);
        }

       /// <summary>
       /// Create SEN need data
       /// </summary>
       /// <param name="ID"></param>
       /// <param name="needTypeID"></param>
       /// <param name="rank"></param>
       /// <param name="startDate"></param>
       /// <param name="learnerID"></param>
       /// <param name="collectionCount"></param>
       /// <param name="referenceID"></param>
       /// <param name="extensionData"></param>
       /// <returns></returns>
        public DataEntityDTO CreateSENSNeed(Guid ID, Guid needTypeID, int rank, string startDate, Guid learnerID, int referenceID, int collectionCount, ExtensionDataObject extensionData)
        {

            DataEntityDTO.SimplePropertyDTOGuid IDProperty = new DataEntityDTO.SimplePropertyDTOGuid { Value = ID };
            DataEntityDTO.SimplePropertyDTOInt rankProperty = new DataEntityDTO.SimplePropertyDTOInt() { Value = rank };
           DataEntityDTO.SimplePropertyDTODate StartDateProperty = new DataEntityDTO.SimplePropertyDTODate { Value = new Date { internalDateTime = DateTime.Parse(startDate) } };
            DataEntityDTO.ReferencePropertyDTO needTypeProperty = new DataEntityDTO.ReferencePropertyDTO { EntityPrimaryKey = needTypeID };

            DataEntityDTO.ReferencePropertyDTO LearnerProperty = new DataEntityDTO.ReferencePropertyDTO
            {
                EntityPrimaryKey = learnerID,
                InternalReferenceID = (short?)collectionCount
            };

            DataEntityDTO entity = new DataEntityDTO
            {
                EntityName = "LearnerSENNeedType",
                Values = new Dictionary<string, DataEntityDTO.SimplePropertyDTO>
                {
                    {"ID", IDProperty},
                    {"StartDate", StartDateProperty},
                    {"Learner", LearnerProperty},
                    {"NeedType", needTypeProperty},
                    {"Rank", rankProperty}

                }
            };

            return DataExchangeDetail.EntitySettings(entity, ID, referenceID, extensionData);
        }
       
        /// <summary>
        /// Create enrolment details for pupil
        /// </summary>
        /// <param name="id"></param>
        /// <param name="learnerEnrolmentId"></param>
        /// <param name="enrolmentStatusID"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="referenceID"></param>
        /// <param name="collectionCount"></param>
        /// <param name="extensionData"></param>
        /// <returns></returns>
      
        public DataEntityDTO CreateLearnerEnrolmentStatus(Guid id, Guid learnerEnrolmentId, Guid enrolmentStatusID,string startDate, string endDate, int referenceID, int collectionCount, ExtensionDataObject extensionData)
        {

            DataEntityDTO.SimplePropertyDTOGuid IDProperty = new DataEntityDTO.SimplePropertyDTOGuid { Value = id };
            DataEntityDTO.SimplePropertyDTODate StartDateProperty = new DataEntityDTO.SimplePropertyDTODate { Value = new Date { internalDateTime = DateTime.Parse(startDate) } };
            DataEntityDTO.ReferencePropertyDTO learnerEnrolmentProperty = new DataEntityDTO.ReferencePropertyDTO { EntityPrimaryKey = learnerEnrolmentId };


            DataEntityDTO.ReferencePropertyDTO enrolmentStatusProperty = new DataEntityDTO.ReferencePropertyDTO { EntityPrimaryKey = enrolmentStatusID };

            DataEntityDTO entity = new DataEntityDTO
            {
                EntityName = "LearnerEnrolmentStatus",
                Values = new Dictionary<string, DataEntityDTO.SimplePropertyDTO>
                {
                    {"ID", IDProperty},
                    {"StartDate", StartDateProperty},
                    {"LearnerEnrolment", learnerEnrolmentProperty},
                    {"EnrolmentStatus", enrolmentStatusProperty}
                }
            };

            if (string.IsNullOrEmpty(endDate)) return DataExchangeDetail.EntitySettings(entity, id, referenceID, extensionData);
            DataEntityDTO.SimplePropertyDTODate endDateProperty = new DataEntityDTO.SimplePropertyDTODate
            {
                Value = new Date { internalDateTime = DateTime.Parse(endDate) }
            };
            entity.Values.Add("EndDate", endDateProperty);

            return DataExchangeDetail.EntitySettings(entity, id, referenceID, extensionData);
        }

       /// <summary>
        /// Create enrolment details for pupil
       /// </summary>
       /// <param name="ID"></param>
       /// <param name="learnerId"></param>
       /// <param name="startDate"></param>
       /// <param name="leavingDate"></param>
       /// <param name="schoolId"></param>
       /// <param name="referenceID"></param>
       /// <param name="collectionCount"></param>
       /// <param name="extensionData"></param>
       /// <returns></returns>
        public DataEntityDTO CreateLearnerEnrolment(Guid ID, Guid learnerId, string startDate, string leavingDate, Guid schoolId, int referenceID, int collectionCount, ExtensionDataObject extensionData)
        {

            DataEntityDTO.SimplePropertyDTOGuid IDProperty = new DataEntityDTO.SimplePropertyDTOGuid { Value = ID };
            DataEntityDTO.SimplePropertyDTODate StartDateProperty = new DataEntityDTO.SimplePropertyDTODate { Value = new Date { internalDateTime = DateTime.Parse(startDate) } };
            DataEntityDTO.ReferencePropertyDTO schoolProperty = new DataEntityDTO.ReferencePropertyDTO { EntityPrimaryKey = schoolId };


            DataEntityDTO.ReferencePropertyDTO LearnerProperty = new DataEntityDTO.ReferencePropertyDTO { EntityPrimaryKey = learnerId };

            DataEntityDTO entity = new DataEntityDTO
            {
                EntityName = "LearnerEnrolment",
                Values = new Dictionary<string, DataEntityDTO.SimplePropertyDTO>
                {
                    {"ID", IDProperty},
                    {"DOA", StartDateProperty},
                    {"Learner", LearnerProperty},
                    {"School", schoolProperty}
                }
            };

            if (string.IsNullOrEmpty(leavingDate)) return DataExchangeDetail.EntitySettings(entity, ID, referenceID, extensionData);
            DataEntityDTO.SimplePropertyDTODate DOLProperty = new DataEntityDTO.SimplePropertyDTODate
            {
                Value = new Date { internalDateTime = DateTime.Parse(leavingDate) }
            };
            entity.Values.Add("DOL", DOLProperty);

            return DataExchangeDetail.EntitySettings(entity, ID, referenceID, extensionData);
        }

       /// <summary>
       /// Create Learner NC year DataEntity
       /// </summary>
       /// <param name="ID"></param>
       /// <param name="learnerId"></param>
       /// <param name="startDate"></param>
       /// <param name="schoolCYear"></param>
       /// <param name="collectionCount"></param>
       /// <param name="referenceID"></param>
       /// <param name="extensionData"></param>
       /// <returns></returns>
        public DataEntityDTO CreateLearnerNCYear(Guid ID, Guid learnerId, string startDate, Guid schoolCYear, int referenceID, int collectionCount, ExtensionDataObject extensionData)
       {
           DataEntityDTO.SimplePropertyDTOGuid IDProperty = new DataEntityDTO.SimplePropertyDTOGuid { Value = ID };
           DataEntityDTO.SimplePropertyDTODate StartDateProperty = new DataEntityDTO.SimplePropertyDTODate { Value = new Date { internalDateTime = DateTime.Parse(startDate) } };
           DataEntityDTO.ReferencePropertyDTO schoolncyearProperty = new DataEntityDTO.ReferencePropertyDTO { EntityPrimaryKey = schoolCYear };

           DataEntityDTO.ReferencePropertyDTO LearnerProperty = new DataEntityDTO.ReferencePropertyDTO
           {
               EntityPrimaryKey = learnerId,
               InternalReferenceID = (short?)collectionCount
           };

           DataEntityDTO entity = new DataEntityDTO
           {
               EntityName = "LearnerNCYearMembership",
               Values = new Dictionary<string, DataEntityDTO.SimplePropertyDTO>
                {
                    {"ID", IDProperty},
                    {"StartDate", StartDateProperty},
                    {"Learner", LearnerProperty},
                    {"SchoolNCYear", schoolncyearProperty}
                }
           };

           return DataExchangeDetail.EntitySettings(entity, ID, referenceID, extensionData);
       }

       /// <summary>
       /// Create learner yeargroup Data entity
       /// </summary>
       /// <param name="ID"></param>
       /// <param name="learnerId"></param>
       /// <param name="startDate"></param>
       /// <param name="yeargroup"></param>
       /// <param name="collectionCount"></param>
       /// <param name="referenceID"></param>
       /// <param name="extensionData"></param>
       /// <returns></returns>
       public DataEntityDTO CreateLearnerYearGroup(Guid ID, Guid learnerId, string startDate, Guid yeargroup, int referenceID, int collectionCount, ExtensionDataObject extensionData)
       {
           DataEntityDTO.SimplePropertyDTOGuid IDProperty = new DataEntityDTO.SimplePropertyDTOGuid { Value = ID };
          DataEntityDTO.SimplePropertyDTODate StartDateProperty = new DataEntityDTO.SimplePropertyDTODate { Value = new Date { internalDateTime = DateTime.Parse(startDate) } };
           DataEntityDTO.ReferencePropertyDTO yearGroupProperty = new DataEntityDTO.ReferencePropertyDTO { EntityPrimaryKey = yeargroup };

           DataEntityDTO.ReferencePropertyDTO LearnerProperty = new DataEntityDTO.ReferencePropertyDTO
           {
               EntityPrimaryKey = learnerId,
               InternalReferenceID = (short?)collectionCount
           };

           DataEntityDTO entity = new DataEntityDTO
           {
               EntityName = "LearnerYearGroupMembership",
               Values = new Dictionary<string, DataEntityDTO.SimplePropertyDTO>
                {
                    {"ID", IDProperty},
                    {"StartDate", StartDateProperty},
                    {"Learner", LearnerProperty},
                    {"YearGroup", yearGroupProperty}
                }
           };
           
           return DataExchangeDetail.EntitySettings(entity, ID, referenceID, extensionData);
       }


       /// <summary>
       /// RetrieveEntityByNameQuery
       /// </summary>
       /// <param name="queryName"></param>
       /// <param name="parameters"></param>
       /// <param name="sessionToken"></param>
       /// <returns></returns>
       public DataEntityCollectionDTO RetrieveEntityByNameQuery(string queryName, Dictionary<string, object> parameters, SecurityToken sessionToken)
        {
            SecuritySummary securitySummary = new SecuritySummary();
            CallStatus callStatus = new CallStatus();

            using (ChannelFactory<IDataEntityIO> factory = GetApplicationServerChannelFactory(_applicationServerAddress))
            {
                factory.Credentials.UseIdentityConfiguration = true;
                IDataEntityIO secureConnection = factory.CreateChannelWithIssuedToken(sessionToken);

                DataEntityCollectionDTO entities = secureConnection.RunNamedQuery(queryName, parameters,
                    ref securitySummary, ref callStatus);

                if (callStatus.Result == CallStatusenumCallResult.Success) return entities;

                StringBuilder sb = new StringBuilder();
                foreach (ValidationError error in callStatus.Messages)
                {
                    sb.AppendLine(error.Message);
                }
                throw new Exception("Call did not complete successfully" + Environment.NewLine + sb);
            }
        }

       /// <summary>
       /// RetrieveEntityByQuery
       /// </summary>
       /// <param name="query"></param>
       /// <param name="parameters"></param>
       /// <param name="sessionToken"></param>
       /// <returns></returns>
        public DataEntityCollectionDTO RetrieveEntityByQuery(DataQuery query, Dictionary<string, object> parameters, SecurityToken sessionToken)
        {
            SecuritySummary securitySummary = new SecuritySummary();
            CallStatus callStatus = new CallStatus();

            using (ChannelFactory<IDataEntityIO> factory = GetApplicationServerChannelFactory(_applicationServerAddress))
            {
                factory.Credentials.UseIdentityConfiguration = true;
                IDataEntityIO secureConnection = factory.CreateChannelWithIssuedToken(sessionToken);

                DataEntityCollectionDTO entities = secureConnection.RunQuery(query, parameters,
                    ref securitySummary, ref callStatus);

                if (callStatus.Result == CallStatusenumCallResult.Success) return entities;

                StringBuilder sb = new StringBuilder();
                foreach (ValidationError error in callStatus.Messages)
                {
                    sb.AppendLine(error.Message);
                }
                throw new Exception("Call did not complete successfully" + Environment.NewLine + sb);
            }
        }

    }
}
