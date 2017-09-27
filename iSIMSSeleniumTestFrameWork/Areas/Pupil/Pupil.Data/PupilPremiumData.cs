using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using SeSugar.Data;
using Environment = SeSugar.Environment;

namespace Pupil.Data
{
    public static class PupilPremiumData
    {
        public static DataPackage AddPupilPremiumEligibitity(this DataPackage dataPackage, Guid learnerId)
        {
            int tenantId = Environment.Settings.TenantId;
            dataPackage.AddData("LearnerPupilPremiumEligibility", new
            {
                Id = Guid.NewGuid(),
                Learner = learnerId,
                StartDate = DateTime.Now.Add(TimeSpan.FromDays(60)),
                TenantID = tenantId
            });

            return dataPackage;
        }

        public static DataPackage AddPupilPremiumGrant(this DataPackage dataPackage, Guid learnerId)
        {
            int tenantId = Environment.Settings.TenantId;
            dataPackage.AddData("LearnerPupilPremiumGrant", new
            {
                Id = Guid.NewGuid(),
                Learner = learnerId,
                StartDate = DateTime.Now.Add(TimeSpan.FromDays(60)),
                TenantID = tenantId,
                Amount = 1235.00,
                ServicePupil = 1,
                EarlyYears = 1
            });

            return dataPackage;
        }
    }
}
