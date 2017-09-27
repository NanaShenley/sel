using System;
using SeSugar.Data;
using Environment = SeSugar.Environment;

namespace Pupil.Data
{
    public static class BehaviourEventData
    {
        private static string _behaviourLabel;

        public static string BehaviourLabel
        {
            get
            {
                if (_behaviourLabel == null)
                {
                    string sql = string.Format(@"
                                    SELECT TOP 1 BehaviourLabel
                                    FROM dbo.ConductServiceSetup
                                    WHERE TenantID = {0}", Environment.Settings.TenantId);

                    _behaviourLabel = DataAccessHelpers.GetValue<string>(sql);

                }

                return _behaviourLabel;
            }
        }

        public static DataPackage AddBehaviourEvent(this DataPackage dataPackage, Guid behaviourEventId, Guid learnerId, Guid learnerBehaviourEventId, Guid followUpId, Guid behaviourEventCategoryId, Guid behaviourEventFollowUpActionId, int? tenantId = null)
        {
            tenantId = tenantId ?? Environment.Settings.TenantId;

            dataPackage.Add("BehaviourEvent", new
            {
                ID = behaviourEventId,
                TenantId = tenantId,
                EventDateTime = DateTime.Now,
                BehaviourEventCategory = behaviourEventCategoryId
            });

            dataPackage.Add("LearnerBehaviourEvent", new
            {
                ID = learnerBehaviourEventId,
                TenantId = tenantId,
                Learner = learnerId,
                BehaviourEvent = behaviourEventId,
                Points = Queries.GetDefaultPointsForFirstFullConductEventCategoryLookup("Behaviour")
            });

            dataPackage.Add("BehaviourEventFollowUp", new
            {
                ID = followUpId,
                TenantId = tenantId,
                DueDate = DateTime.Now.AddDays(1),
                LearnerBehaviourEvent = learnerBehaviourEventId,
                BehaviourEventFollowUpAction = behaviourEventFollowUpActionId
            });

            return dataPackage;
        }
    }
}
