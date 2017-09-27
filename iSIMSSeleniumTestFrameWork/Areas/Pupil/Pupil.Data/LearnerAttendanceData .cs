using System;
using SeSugar;
using SeSugar.Data;
using TestSettings;
using Environment = SeSugar.Environment;

namespace Pupil.Data
{
    public static class LearnerAttendanceData
    {
        public static DataPackage AddLearnerAttendanceMode(this DataPackage dataPackage, Guid learnerId, DateTime startDate)
        {
            int tenantId = Environment.Settings.TenantId;
            var attendanceMode = Queries.GetFirstAttendanceMode();

            dataPackage.AddData("LearnerAttendanceMode", new
            {
                Learner = learnerId,
                TenantID = tenantId,
                StartDate = startDate,
                AttendanceMode = attendanceMode.Id
			});
            return dataPackage;
        }
    }
}
