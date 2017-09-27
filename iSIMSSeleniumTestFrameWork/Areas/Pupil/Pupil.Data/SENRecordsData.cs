using System;
using SeSugar.Data;
using TestSettings;
using Environment = SeSugar.Environment;

namespace Pupil.Data
{
	public static class SENRecordData
	{
		public static DataPackage AddSENSStagetoLearner(this DataPackage dataPackage, Guid learnerId, string senStage = "1", int? tenantId = null)
		{
            tenantId = tenantId ?? Environment.Settings.TenantId;
            dataPackage.AddData("LearnerSENStatus", new
			{
				Id = Guid.NewGuid(),
				StartDate = new DateTime(2015, 05, 05),
				Learner = learnerId,
				SENStatus = CoreQueries.GetLookupItem("SENStatus", code: senStage),
                TenantID = tenantId
			});
			return dataPackage;
		}
	}
}