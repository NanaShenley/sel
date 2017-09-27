using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Pupil.Components.DataEntityIO;
using TestSettings;

namespace Pupil.Components.PupilQueries
{
    public class PupilQuery : WebServiceBase
    {
        private readonly string _applicationServerAddress;

        public PupilQuery()
        {
            _applicationServerAddress = Configuration.GetSutUrl() + TestDefaults.Default.ApplicationServerPath;
        }

        public DataEntityDTO CreatePupil(
            Guid ID, 
            string legalForename, 
            string legalSurname, 
            Date dateOfBirth,
            Guid genderID,
            Guid schoolID, 
            int referenceID, 
            ExtensionDataObject extensionData)
        {
            DataEntityDTO.SimplePropertyDTOGuid IDProperty = new DataEntityDTO.SimplePropertyDTOGuid { Value = ID };
            DataEntityDTO.SimplePropertyDTOString legalForenameProperty = new DataEntityDTO.SimplePropertyDTOString { Value = legalForename };
            DataEntityDTO.SimplePropertyDTOString legalSurnameProperty = new DataEntityDTO.SimplePropertyDTOString { Value = legalSurname };
            DataEntityDTO.SimplePropertyDTODate dateOfBirthProperty = new DataEntityDTO.SimplePropertyDTODate { Value = dateOfBirth };
            DataEntityDTO.ReferencePropertyDTO schoolProperty = new DataEntityDTO.ReferencePropertyDTO { EntityPrimaryKey = schoolID };
            DataEntityDTO.ReferencePropertyDTO genderProperty = new DataEntityDTO.ReferencePropertyDTO { EntityPrimaryKey = genderID };
            DataEntityDTO entity = new DataEntityDTO
            {
                EntityName = "Learner",
                Values = new Dictionary<string, DataEntityDTO.SimplePropertyDTO>
                {
                    {"ID", IDProperty},
                    {"LegalForename", legalForenameProperty},
                    {"LegalSurname", legalSurnameProperty},
                    {"DateOfBirth", dateOfBirthProperty},
                    {"Gender", genderProperty},
                    {"School", schoolProperty}
                }
            };

            return PupilDetails.EntitySettings(entity, ID, referenceID, extensionData);
        }

        public DataEntityDTO CreateStandardPupilLogNote(
            Guid ID, 
            string title, 
            string noteText, 
            Guid learnerID, 
            Guid categoryID, 
            Guid userID,
            bool pinned, 
            int collectionCount, 
            int referenceID, 
            ExtensionDataObject extensionData)
        {
            DataEntityDTO.SimplePropertyDTOGuid IDProperty = new DataEntityDTO.SimplePropertyDTOGuid { Value = ID };
            DataEntityDTO.SimplePropertyDTOString titleProperty = new DataEntityDTO.SimplePropertyDTOString { Value = title };
            DataEntityDTO.SimplePropertyDTOString noteTextProperty = new DataEntityDTO.SimplePropertyDTOString { Value = noteText };
            DataEntityDTO.SimplePropertyDTOBool pinnedProperty = new DataEntityDTO.SimplePropertyDTOBool { Value = pinned };
            DataEntityDTO.SimplePropertyDTODateTime createdOnProperty = new DataEntityDTO.SimplePropertyDTODateTime { Value = DateTime.Now };
            DataEntityDTO.SimplePropertyDTOGuid createdByProperty = new DataEntityDTO.SimplePropertyDTOGuid { Value = userID };
            DataEntityDTO.ReferencePropertyDTO learnerProperty = new DataEntityDTO.ReferencePropertyDTO { EntityPrimaryKey = learnerID };
            DataEntityDTO.ReferencePropertyDTO categoryProperty = new DataEntityDTO.ReferencePropertyDTO { EntityPrimaryKey = categoryID };

            DataEntityDTO entity = new DataEntityDTO
            {
                EntityName = "PupilLogNoteStandard",
                Values = new Dictionary<string, DataEntityDTO.SimplePropertyDTO>
                {
                    {"ID", IDProperty},
                    {"Title", titleProperty},
                    {"NoteText", noteTextProperty},
                    {"Pinned", pinnedProperty},
                    {"CreatedOn", createdOnProperty},
                    {"CreatedByUserId", createdByProperty},
                    {"Learner", learnerProperty},
                    {"PupilLogNoteCategory", categoryProperty}
                }
            };

            return PupilDetails.EntitySettings(entity, ID, referenceID, extensionData);
        }

        private static List<string> GetFields(string entityName)
        {
            List<string> fields = new List<string>();

            switch (entityName)
            {
                case "Learner":
                    fields = new List<string>(new[]
                    {
                        "Learner.LegalForename",
                        "Learner.LegalSurname",
                        "Learner.DateOfBirth"
                    });
                    break;
                case "PupilLogNoteStandard":
                    fields = new List<string>(new[]
                    {
                        "PupilLogNoteStandard.CreatedOn",
                        "PupilLogNoteStandard.CreatedByUserId",
                        "PupilLogNoteStandard.Pinned",
                        "PupilLogNoteStandard.Title",
                        "PupilLogNoteStandard.NoteText",
                        "PupilLogNoteStandard.Learner",
                        "PupilLogNoteStandard.PupilLogNoteCategory"
                    });
                    break;
            }

            return fields;
        }

        public DataEntityCollectionDTO RetrieveEntityById(string entityName, Guid entityId, SecurityToken sessionToken)
        {
            SecuritySummary securitySummary = new SecuritySummary();
            CallStatus callStatus = new CallStatus();

            using (ChannelFactory<IDataEntityIO> factory = GetApplicationServerChannelFactory(_applicationServerAddress))
            {
                factory.Credentials.UseIdentityConfiguration = true;
                IDataEntityIO secureConnection = factory.CreateChannelWithIssuedToken(sessionToken);

                List<string> fieldNames = GetFields(entityName);

                DataEntityCollectionDTO entities = secureConnection.ReadEntity(
                   entityId,
                   entityName,
                   fieldNames, ref securitySummary, ref callStatus);

                if (callStatus.Result == CallStatusenumCallResult.Success) return entities;

                StringBuilder sb = new StringBuilder();
                foreach (ValidationError error in callStatus.Messages)
                {
                    sb.AppendLine(error.Message);
                }
                throw new Exception("Call did not complete successfully" + Environment.NewLine + sb);
            }
        }

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