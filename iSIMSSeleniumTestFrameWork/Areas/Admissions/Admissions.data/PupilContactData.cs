using System;
using SeSugar.Data;
using TestSettings;
using Environment = SeSugar.Environment;

namespace Admissions.Data
{
    public static class PupilContactData
    {
        public static DataPackage AddPupilContact(this DataPackage dataPackage, 
            Guid pupilContactId, 
            string surname, 
            string forename,             
            string salutation = null,
            string addressee = null,
            string jobTitle = null,
            string titleCode = "Mr",
            string genderCode = "1",
            string occupation = "ACC",
            int?    tenantId = null
            )
        {
            tenantId = tenantId ?? Environment.Settings.TenantId;
            dataPackage.AddData(Constants.Tables.LearnerContact, new
            {
                ID = pupilContactId,
                Surname = surname,
                Forename = forename,
                School = CoreQueries.GetSchoolId(),
                Gender = CoreQueries.GetLookupItem(Constants.Tables.Gender, code: genderCode),
                Title = CoreQueries.GetLookupItem(Constants.Tables.Title, code: titleCode),
                Salutation = salutation,
                JobTitle = jobTitle,
                Addressee = addressee,
                Occupation = CoreQueries.GetLookupItem(Constants.Tables.Occupation, code: occupation),
                TenantID = tenantId
            });
            return dataPackage;
        }

        public static DataPackage AddPupilContactRelationship(this DataPackage dataPackage,
            Guid pupilContactRelationshipId,
            Guid learnerId,
            Guid pupilContactId,
            string relationshipType = "OTH",
            int priority = 1,
            bool hasParentalResponsibility = false,
            bool receivesCorrespondance = false,
            bool hasSchoolReport = false,
            bool hasCourtOrder = false,
            int? tenantId = null)
        {
            tenantId = tenantId ?? Environment.Settings.TenantId;
            dataPackage.AddData(Constants.Tables.LearnerContactRelationship, new
            {
                ID = pupilContactRelationshipId,
                Priority = priority,
                Learner = learnerId,
                LearnerContact = pupilContactId,
                LearnerContactRelationshipType = CoreQueries.GetLookupItem(Constants.Tables.LearnerContactRelationshipType, relationshipType),
                HasParentalResponsibility = hasParentalResponsibility,
                ReceivesCorrespondance = receivesCorrespondance,
                HasCourtOrder = hasCourtOrder,
                ReceivesSchoolReport = hasSchoolReport,
                TenantID = tenantId
            });

            return dataPackage;
        }
    }
}
