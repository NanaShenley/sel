using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using GetDataViaWebServices.DataEntityIO;
using Staff.Components.StaffRecord;
using TestSettings;

namespace Staff.Components.StaffQueries
{
    public class StaffQuery : WebServiceBase
    {
        private readonly string _applicationServerAddress;

        public StaffQuery()
        {
            _applicationServerAddress = Configuration.GetSutUrl() + TestDefaults.Default.ApplicationServerPath;
        }

        public DataEntityDTO CreateStaff(Guid ID, string legalForename, string legalSurname, Guid schoolID, int referenceID, ExtensionDataObject extensionData)
        {
            DataEntityDTO.SimplePropertyDTOGuid IDProperty = new DataEntityDTO.SimplePropertyDTOGuid { Value = ID };
            DataEntityDTO.SimplePropertyDTOString legalForenameProperty = new DataEntityDTO.SimplePropertyDTOString { Value = legalForename };
            DataEntityDTO.SimplePropertyDTOString legalSurnameProperty = new DataEntityDTO.SimplePropertyDTOString { Value = legalSurname };
            DataEntityDTO.ReferencePropertyDTO schoolProperty = new DataEntityDTO.ReferencePropertyDTO { EntityPrimaryKey = schoolID };

            DataEntityDTO entity = new DataEntityDTO
            {
                EntityName = "Staff",
                Values = new Dictionary<string, DataEntityDTO.SimplePropertyDTO>
                {
                    {"ID", IDProperty},
                    {"LegalForename", legalForenameProperty},
                    {"LegalSurname", legalSurnameProperty},
                    {"School", schoolProperty},
                }
            };

            return StaffDetail.EntitySettings(entity, ID, referenceID, extensionData);
        }

        public DataEntityDTO CreateStaffService(Guid ID, string DOA, string DOL, Guid staffID, int collectionCount, int referenceID, ExtensionDataObject extensionData)
        {

            DataEntityDTO.SimplePropertyDTOGuid IDProperty = new DataEntityDTO.SimplePropertyDTOGuid { Value = ID };
            DataEntityDTO.SimplePropertyDTODate DOAProperty = new DataEntityDTO.SimplePropertyDTODate { Value = new Date { internalDateTime = DateTime.Parse(DOA) } };
            
            DataEntityDTO.ReferencePropertyDTO StaffProperty = new DataEntityDTO.ReferencePropertyDTO
            {
                EntityPrimaryKey = staffID,
                InternalReferenceID = (short?)collectionCount
            };

            DataEntityDTO entity = new DataEntityDTO
            {
                EntityName = "StaffServiceRecord",
                Values = new Dictionary<string, DataEntityDTO.SimplePropertyDTO>
                {
                    {"ID", IDProperty},
                    {"DOA", DOAProperty},
                    {"Staff", StaffProperty}
                }
            };

            if (string.IsNullOrEmpty(DOL)) return StaffDetail.EntitySettings(entity, ID, referenceID, extensionData);
            DataEntityDTO.SimplePropertyDTODate DOLProperty = new DataEntityDTO.SimplePropertyDTODate
            {
                Value = new Date {internalDateTime = DateTime.Parse(DOL)}
            };
            entity.Values.Add("DOL", DOLProperty);

            return StaffDetail.EntitySettings(entity, ID, referenceID, extensionData);
        }

        private List<string> GetFields(string entityName)
        {
            List<string> fields = new List<string>();

            switch (entityName)
            {
                case "Staff":
                    fields = new List<string>(new[]
                    {
                        "Staff.LegalForename",
                        "Staff.LegalSurname"
                    });
                    break;
                case "StaffServiceRecord":
                    fields = new List<string>(new[]
                    {
                        "StaffServiceRecord.DOA",
                        "StaffServiceRecord.DOL",
                        "StaffServiceRecord.ContinuousServiceStartDate",
                        "StaffServiceRecord.LocalAuthorityStartDate",
                        "StaffServiceRecord.Destination",
                        "StaffServiceRecord.PreviousEmployer",
                        "StaffServiceRecord.NextEmployer",
                        "StaffServiceRecord.Notes",
                        "StaffServiceRecord.StaffReasonForLeaving",
                        "StaffServiceRecord.Staff"
                    });
                    break;
                case "StaffRoleAssignment":
                    fields = new List<string>(new[]
                    {
                        "StaffRoleAssignment.StartDate",
                        "StaffRoleAssignment.EndDate",
                        "StaffRoleAssignment.Staff",
                        "StaffRoleAssignment.StaffRole"
                    });
                    break;
                case "PrimaryClassStaff":
                    fields = new List<string>(new[]
                    {
                        "PrimaryClassStaff.StartDate",
                        "PrimaryClassStaff.EndDate",
                        "PrimaryClassStaff.Staff",
                        "PrimaryClassStaff.PrimaryClass",
                        "PrimaryClassStaff.StaffRole"
                    });
                    break;
                case "PrimaryClassTeacher":
                    fields = new List<string>(new[]
                    {
                        "PrimaryClassTeacher.StartDate",
                        "PrimaryClassTeacher.EndDate",
                        "PrimaryClassTeacher.Staff",
                        "PrimaryClassTeacher.PrimaryClass",
                        "PrimaryClassTeacher.StaffRole"
                    });
                    break;
                case "YearGroupStaff":
                    fields = new List<string>(new[]
                    {
                        "YearGroupStaff.StartDate",
                        "YearGroupStaff.EndDate",
                        "YearGroupStaff.Staff",
                        "YearGroupStaff.YearGroup",
                        "YearGroupStaff.StaffRole"
                    });
                    break;
                case "HeadOfYear":
                    fields = new List<string>(new[]
                    {
                        "HeadOfYear.StartDate",
                        "HeadOfYear.EndDate",
                        "HeadOfYear.Staff",
                        "HeadOfYear.YearGroup",
                        "HeadOfYear.StaffRole"
                    });
                    break;
                case "TeachingGroupStaff":
                    fields = new List<string>(new[]
                    {
                        "TeachingGroupStaff.StartDate",
                        "TeachingGroupStaff.EndDate",
                        "TeachingGroupStaff.Staff",
                        "TeachingGroupStaff.TeachingGroup",
                        "TeachingGroupStaff.StaffRole"
                    });
                    break;
                case "UserDefinedGroupSupervisor":
                    fields = new List<string>(new[]
                    {
                        "UserDefinedGroupSupervisor.StartDate",
                        "UserDefinedGroupSupervisor.EndDate",
                        "UserDefinedGroupSupervisor.SupervisorRole",
                        "UserDefinedGroupSupervisor.GroupSupervisor"

                    });
                    break;
                case "UserDefinedGroupMembership":
                    fields = new List<string>(new[]
                    {
                        "UserDefinedGroupMembership.StartDate",
                        "UserDefinedGroupMembership.EndDate",
                        "UserDefinedGroupMembership.GroupMember"
                    });
                    break;
                case "EmploymentContract":
                    fields = new List<string>(new[]
                    {
                        "EmploymentContract.StartDate",
                        "EmploymentContract.EndDate",
                        "EmploymentContract.ServiceTerm",
                        "EmploymentContract.Employee",
                        "EmploymentContract.EmploymentContractPayScales",
                        "EmploymentContract.EmploymentContractPayScales.EndDate",
                        "EmploymentContract.EmploymentContractAllowances",
                        "EmploymentContract.EmploymentContractAllowances.EndDate",
                        "EmploymentContract.EmploymentContractRoles",
                        "EmploymentContract.EmploymentContractRoles.EndDate"
                    });
                    break;
            }

            return fields;
        }

        public DataEntityDTO CreateLookupWithResourceProvider(DataEntityCollectionDTO collection, string code, string description, string entityName)
        {
            Guid lookupID = Guid.NewGuid();

            DataEntityDTO.SimplePropertyDTOGuid lookupIDProperty = new DataEntityDTO.SimplePropertyDTOGuid { Value = lookupID };
            DataEntityDTO.SimplePropertyDTOString lookupCodeProperty = new DataEntityDTO.SimplePropertyDTOString { Value = code };
            DataEntityDTO.SimplePropertyDTOString lookupDescriptionProperty = new DataEntityDTO.SimplePropertyDTOString { Value = description };
            DataEntityDTO.SimplePropertyDTOBool lookupIsVisibleProperty = new DataEntityDTO.SimplePropertyDTOBool { Value = true };
            DataEntityDTO.SimplePropertyDTOInt lookupDisplayOrderProperty = new DataEntityDTO.SimplePropertyDTOInt { Value = 1 };

            DataEntityDTO.ReferencePropertyDTO schoolProperty = new DataEntityDTO.ReferencePropertyDTO
            {
                EntityPrimaryKey = Guid.Parse(TestDefaults.Default.SchoolID),
            };

            DataEntityDTO lookup = StaffDetail.NewEntitySettings(lookupID, collection.DataEntityDtos.Count, collection.ExtensionData, entityName);
            lookup.Values.Add("ID", lookupIDProperty);
            lookup.Values.Add("Code", lookupCodeProperty);
            lookup.Values.Add("Description", lookupDescriptionProperty);
            lookup.Values.Add("IsVisible", lookupIsVisibleProperty);
            lookup.Values.Add("DisplayOrder", lookupDisplayOrderProperty);
            lookup.Values.Add("ResourceProvider", schoolProperty);

            return lookup;
        }

        public DataEntityDTO CreateLookup(DataEntityCollectionDTO collection, string code, string description, string entityName)
        {
            Guid lookupID = Guid.NewGuid();

            DataEntityDTO.SimplePropertyDTOGuid lookupIDProperty = new DataEntityDTO.SimplePropertyDTOGuid { Value = lookupID };
            DataEntityDTO.SimplePropertyDTOString lookupCodeProperty = new DataEntityDTO.SimplePropertyDTOString { Value = code };
            DataEntityDTO.SimplePropertyDTOString lookupDescriptionProperty = new DataEntityDTO.SimplePropertyDTOString { Value = description };
            DataEntityDTO.SimplePropertyDTOBool lookupIsVisibleProperty = new DataEntityDTO.SimplePropertyDTOBool { Value = true };
            DataEntityDTO.SimplePropertyDTOInt lookupDisplayOrderProperty = new DataEntityDTO.SimplePropertyDTOInt { Value = 1 };

            DataEntityDTO lookup = StaffDetail.NewEntitySettings(lookupID, collection.DataEntityDtos.Count, collection.ExtensionData, entityName);
            lookup.Values.Add("ID", lookupIDProperty);
            lookup.Values.Add("Code", lookupCodeProperty);
            lookup.Values.Add("Description", lookupDescriptionProperty);
            lookup.Values.Add("IsVisible", lookupIsVisibleProperty);
            lookup.Values.Add("DisplayOrder", lookupDisplayOrderProperty);

            return lookup;
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
    }
}