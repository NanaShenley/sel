using System;
using SeSugar;
using SeSugar.Data;
using TestSettings;
using Environment = SeSugar.Environment;

namespace Admissions.Data
{
    public static class PupilRecordData
    {        
        public static DataPackage AddBasicLearner(this DataPackage dataPackage, Guid learnerId, string surname, string forename, DateTime dateOfBirth, DateTime dateOfAdmission, string genderCode = "1", string enrolStatus ="C", Guid? uniqueLearnerEnrolmentId = null, int? tenantId = null, string salutation = null, string addressee = null, Guid? yearGroupId = null, Guid? schoolNCYearId = null)
        {
            tenantId = tenantId ?? Environment.Settings.TenantId;

			string sql = String.Format(
				"UPDATE School SET LastAdmissionNumber='B05000' " +
				"WHERE TenantID='" + tenantId + "' " +
                "AND IsRegistered=1 " +
				"AND (LastAdmissionNumber IS NULL " +
				"OR CAST(SUBSTRING(LastAdmissionNumber,2,10) AS INT) < 5000)");

			DataAccessHelpers.Execute(sql);

			Guid learnerEnrolmentId;
            var yearGroup = Queries.GetFirstYearGroup();
            dataPackage.AddData("Learner", new
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
			});
			dataPackage.AddData("LearnerEnrolment", new
                {
                    Id = learnerEnrolmentId = uniqueLearnerEnrolmentId ?? Guid.NewGuid(),
                    School = CoreQueries.GetSchoolId(),
                    Learner = learnerId,
                    DOA = dateOfAdmission,
                    TenantID = tenantId

			});
			dataPackage.AddData("LearnerEnrolmentStatus", new
                {
                    Id = Guid.NewGuid(),
                    LearnerEnrolment = learnerEnrolmentId,
                    StartDate = dateOfAdmission,
                    EnrolmentStatus = CoreQueries.GetLookupItem("EnrolmentStatus", code: enrolStatus),
                    TenantID = tenantId
			});
			dataPackage.AddData("LearnerYearGroupMembership", new
                {
                    Id = Guid.NewGuid(),
                    Learner = learnerId,
                    YearGroup = yearGroupId ?? yearGroup.ID,
                    StartDate = dateOfAdmission,
                    TenantID = tenantId
			});
			dataPackage.AddData("LearnerNCYearMembership", new
                {
                    Id = Guid.NewGuid(),
                    Learner = learnerId,
                    SchoolNCYear = schoolNCYearId ?? yearGroup.SchoolNCYear,
                    StartDate = dateOfAdmission,
                    TenantID = tenantId
                });
            return dataPackage;
        }

        public static DataPackage AddLeaver(this DataPackage dataPackage, Guid learnerId, string surname,
             string forename, DateTime dateOfBirth, DateTime dateOfAdmission, DateTime dateOfLeaving, string genderCode = "1",
             string enrolStatus = "C", string reasonForLeaving = "OT", string tenantId = null)
        {
            Guid learnerEnrolmentId;
            var yearGroup = Queries.GetFirstYearGroup();
            tenantId = tenantId ?? TestDefaults.Default.TenantId.ToString();
            dataPackage.AddData("Learner", new
            {
                Id = learnerId,
                School = CoreQueries.GetSchoolId(),
                Gender = CoreQueries.GetLookupItem("Gender", code: genderCode),
                LegalForename = forename,
                LegalSurname = surname,
                DateOfBirth = dateOfBirth,
                PolicyAclId = CoreQueries.GetPolicyAclId("Learner"),
                TenantID = tenantId
            })
                .AddData("LearnerEnrolment", new
                {
                    Id = learnerEnrolmentId = Guid.NewGuid(),
                    School = CoreQueries.GetSchoolId(),
                    Learner = learnerId,
                    DOA = dateOfAdmission,
                    DOL = dateOfLeaving,
                    ReasonForLeaving = CoreQueries.GetLookupItem("ReasonForLeaving", code: reasonForLeaving),
                    TenantID = tenantId
                })
                .AddData("LearnerEnrolmentStatus", new
                {
                    Id = Guid.NewGuid(),
                    LearnerEnrolment = learnerEnrolmentId,
                    StartDate = dateOfAdmission,
                    EndDate = dateOfLeaving,
                    EnrolmentStatus = CoreQueries.GetLookupItem("EnrolmentStatus", code: enrolStatus),
                    TenantID = tenantId
                })
                .AddData("LearnerYearGroupMembership", new
                {
                    Id = Guid.NewGuid(),
                    Learner = learnerId,
                    YearGroup = yearGroup.ID,
                    StartDate = dateOfAdmission,
                    EndDate = dateOfLeaving,
                    TenantID = tenantId
                })
                .AddData("LearnerNCYearMembership", new
                {
                    Id = Guid.NewGuid(),
                    Learner = learnerId,
                    SchoolNCYear = yearGroup.SchoolNCYear,
                    StartDate = dateOfAdmission,
                    EndDate = dateOfLeaving,
                    TenantID = tenantId
                });
            return dataPackage;
        }
    }
}
