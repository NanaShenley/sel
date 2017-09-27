using System;
using SeSugar.Data;
using TestSettings;
using Environment = SeSugar.Environment;

namespace Pupil.Data
{
    public static class ApplicantRecordData
    {
        public static DataPackage AddBasicApplicant(this DataPackage dataPackage, 
            Guid applicationId,
            Guid learnerId, 
            Guid learnerEnrolmentId,
            Guid admissionGroupId,
            DateTime dateOfApplication, 
            string applicationStatus = "Applied",
            string proposedEnrolStatus = "C",
            int? tenantId = null)
        {
            tenantId = tenantId ?? Environment.Settings.TenantId;

            Guid applicationStatusId;
            Guid applicationStatusLogId = Guid.NewGuid();
            dataPackage.AddData(Constants.Tables.Application, new
            {
                ID = applicationId,
                Learner = learnerId,
                LearnerEnrolment = learnerEnrolmentId,
                CurrentApplicationStatus = applicationStatusId = CoreQueries.GetLookupItem(Constants.Tables.ApplicationStatus, code: applicationStatus),
                ProposedEnrolmentStatus = CoreQueries.GetLookupItem(Constants.Tables.EnrolmentStatus, code: proposedEnrolStatus),
                DateOfApplication = dateOfApplication,
                AdmissionGroup = admissionGroupId,
                TenantID = tenantId
            })
            .AddData(Constants.Tables.ApplicationStatusLog, new
            {
                ID = applicationStatusLogId,
                Application = applicationId,
                TenantID = tenantId,
                ApplicationStatus = applicationStatusId
            });
            return dataPackage;
        }
    }
}
