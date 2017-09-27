using SeSugar.Data;
using System;

namespace SharedServices.TestData
{
    public static class PupilRecordData
    {
        public static DataPackage AddBasicLearner(
            this DataPackage dataPackage, 
            Guid learnerId, 
            string surname, 
            string forename, 
            DateTime dateOfBirth, 
            DateTime dateOfAdmission, 
            string genderCode = "1", 
            string enrolStatus = "C", 
            Guid? uniqueLearnerEnrolmentId = null, 
            int? tenantId = null, 
            string salutation = null, 
            string addressee = null, 
            Guid? yearGroupId = null, 
            Guid? schoolNCYearId = null
            )
        {
            tenantId = tenantId ?? SeSugar.Environment.Settings.TenantId;

            Guid learnerEnrolmentId;
            var yearGroup = Queries.GetFirstYearGroup();
            dataPackage
                .AddData("Learner", new
                {
                    Id = learnerId,
                    School = CoreQueries.GetSchoolId(),
                    Gender = CoreQueries.GetLookupItem("Gender", code: genderCode),
                    LegalForename = forename,
                    LegalSurname = surname,
                    DateOfBirth = dateOfBirth,
                    TenantID = tenantId,
                    ParentalSalutation = salutation,
                    ParentalAddressee = addressee,
                    PolicyAclId = CoreQueries.GetPolicyAclId("Learner")
                })

                .AddData("LearnerEnrolment", new
                {
                    Id = learnerEnrolmentId = uniqueLearnerEnrolmentId ?? Guid.NewGuid(),
                    School = CoreQueries.GetSchoolId(),
                    Learner = learnerId,
                    DOA = dateOfAdmission,
                    TenantID = tenantId

                })

                .AddData("LearnerEnrolmentStatus", new
                {
                    Id = Guid.NewGuid(),
                    LearnerEnrolment = learnerEnrolmentId,
                    StartDate = dateOfAdmission,
                    EnrolmentStatus = CoreQueries.GetLookupItem("EnrolmentStatus", code: enrolStatus),
                    TenantID = tenantId
                })

                .AddData("LearnerYearGroupMembership", new
                {
                    Id = Guid.NewGuid(),
                    Learner = learnerId,
                    YearGroup = yearGroupId ?? yearGroup.ID,
                    StartDate = dateOfAdmission,
                    TenantID = tenantId
                })

                .AddData("LearnerNCYearMembership", new
                {
                    Id = Guid.NewGuid(),
                    Learner = learnerId,
                    SchoolNCYear = schoolNCYearId ?? yearGroup.SchoolNCYear,
                    StartDate = dateOfAdmission,
                    TenantID = tenantId
                });

            return dataPackage;
        }
    }
}
