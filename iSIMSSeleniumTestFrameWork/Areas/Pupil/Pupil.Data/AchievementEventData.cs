using System;
using SeSugar.Data;
using Environment = SeSugar.Environment;

namespace Pupil.Data
{
    public static class AchievementEventData
    {
        private static string _achievementLabel;

        public static string AchievementLabel
        {
            get
            {
                if (_achievementLabel == null)
                {
                    string sql = string.Format(@"
                                    SELECT TOP 1 AchievementLabel
                                    FROM dbo.ConductServiceSetup
                                    WHERE TenantID = {0}", Environment.Settings.TenantId);

                    _achievementLabel = DataAccessHelpers.GetValue<string>(sql);

                }

                return _achievementLabel;
            }
        }

        public static DataPackage AddAchievementEvent(this DataPackage dataPackage, Guid achievementEventId, Guid learnerId, Guid learnerAchievementEventId, Guid followUpId, Guid achievementEventCategoryId, int? tenantId = null)
        {
            tenantId = tenantId ?? Environment.Settings.TenantId;

            dataPackage.Add("AchievementEvent", new
            {
                ID = achievementEventId,
                TenantId = tenantId,
                EventDateTime = DateTime.Now,
                AchievementEventCategory = achievementEventCategoryId
            });

            dataPackage.Add("LearnerAchievementEvent", new
            {
                ID = learnerAchievementEventId,
                TenantId = tenantId,
                Learner = learnerId,
                AchievementEvent = achievementEventId,
                Points = Queries.GetDefaultPointsForFirstFullConductEventCategoryLookup("Achievement")
            });

            return dataPackage;
        }
    }
}
