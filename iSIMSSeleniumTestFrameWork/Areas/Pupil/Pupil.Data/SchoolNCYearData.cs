using System;
using SeSugar.Data;
using Environment = SeSugar.Environment;

namespace Pupil.Data
{
    public static class SchoolNCYearData
    {
        public static DataPackage AddSchoolNCYearLookup(this DataPackage dataPackage, Guid schoolNCYearId, int? tenantId = null)
        {
            tenantId = tenantId ?? Environment.Settings.TenantId;

            string sql = string.Format("SELECT TOP 1 ID FROM dbo.NCYear where TenantID ='{0}'", tenantId);
            Guid nCYearID = DataAccessHelpers.GetValue<Guid>(sql);
            dataPackage.AddData(Constants.Tables.SchoolNCYear, new
            {
                ID = schoolNCYearId,
                TenantID = tenantId,
                School = CoreQueries.GetSchoolId(),
                NCYear = nCYearID
            });
            return dataPackage;
        }
    }
}
