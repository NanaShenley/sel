using System;
using SeSugar.Data;
using Environment = SeSugar.Environment;

namespace Pupil.Data
{
    public static class YearGroupData
    {
        public static DataPackage AddYearGroupLookup(this DataPackage dataPackage, Guid yearGroupId, Guid schoolNCYearId, string shortName, string fullName, int displayOrder, int? tenantId = null)
        {
            tenantId = tenantId ?? Environment.Settings.TenantId;           
            dataPackage.AddData(Constants.Tables.YearGroup, new
            {
                ID = yearGroupId,
                TenantID = tenantId,
                ShortName = shortName,
                FullName = fullName,
                School = CoreQueries.GetSchoolId(),
                SchoolNCYear = schoolNCYearId
            })
            .Add(Constants.Tables.YearGroupSetMembership, new
            {
                ID = Guid.NewGuid(),
                YearGroup = yearGroupId,
                TenantID = tenantId,
                StartDate = new DateTime(2015,01,01)
            });
            return dataPackage;
        }
    }
}
