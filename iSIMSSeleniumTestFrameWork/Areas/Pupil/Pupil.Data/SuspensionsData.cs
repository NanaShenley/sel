using System;
using SeSugar.Data;
using TestSettings;
using Environment = SeSugar.Environment;

namespace Pupil.Data
{
    public static class SuspensionsDataPackageHelper
    {

        public static DataPackage AddBasicSuspension(this DataPackage dataPackage, Guid learnerId, DateTime startDate, DateTime endDate, int days, int? tenantId = null)
        {
            tenantId = tenantId ?? Environment.Settings.TenantId;
            dataPackage.AddData("LearnerExclusion", new
            {
                Id = Guid.NewGuid(),
                Learner = learnerId,
                StartTime = "09:00",
                EndTime = "16:00",
                StartDate = startDate,
                EndDate = endDate,
                ExclusionType = (Queries.GetFirstExclusionType()).ID,
                ExclusionReason = (Queries.GetFirstExclusionReason()).ID,
                NumberOfSchoolDays = days,
                TenantID = tenantId
            });
            return dataPackage;
        }


        public static DataPackage AddSuspensionForType(this DataPackage dataPackage, Guid learnerId, Guid typeId, DateTime startDate, DateTime endDate, int days, int? tenantId = null)
        {
            tenantId = tenantId ?? Environment.Settings.TenantId;
            dataPackage.AddData("LearnerExclusion", new
            {
                Id = Guid.NewGuid(),
                Learner = learnerId,
                StartTime = "09:00",
                EndTime = "16:00",
                StartDate = startDate,
                EndDate = endDate,
                ExclusionType = typeId,
                ExclusionReason = (Queries.GetFirstExclusionReason()).ID,
                NumberOfSchoolDays = days,
                SessionsMissed = days,
                TenantID = tenantId
            });
            return dataPackage;
        }

        public static DataPackage AddSuspensionWithMeeting(this DataPackage dataPackage, Guid learnerId, Guid suspensionId, DateTime startDate, DateTime endDate, int days, Guid exclusionType, Guid exclusionReason, string tenantId = null)
        {
            tenantId = tenantId ?? TestDefaults.Default.TenantId.ToString();
            dataPackage.AddData("LearnerExclusion", new
            {
                Id = suspensionId,
                Learner = learnerId,
                StartTime = "09:00",
                EndTime = "16:00",
                StartDate = startDate,
                EndDate = endDate,
                ExclusionType = exclusionType,
                ExclusionReason = exclusionReason,
                NumberOfSchoolDays = days,
                SessionsMissed = days,
                TenantID = tenantId
            })
            .AddData("ExclusionMeeting", new
            {
                Id = Guid.NewGuid(),
                LearnerExclusion = suspensionId,
                StartDate = startDate,
                EndDate = endDate,
                StartTime = "09:00",
                EndTime = "16:00",
                ExclusionMeetingType = CoreQueries.GetLookupItem("ExclusionMeetingType", description: "Discipline Committee"),
                TenantID = tenantId
            });
            return dataPackage;
        }
    }
}
