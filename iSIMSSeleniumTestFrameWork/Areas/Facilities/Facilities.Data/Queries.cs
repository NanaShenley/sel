using System;
using System.Linq;
using System.Runtime.InteropServices;
using Facilities.Data.Entities;
using SeSugar.Data;
using Environment = SeSugar.Environment;

namespace Facilities.Data
{
    public static class Queries
    {
        private const string SqlDateFormat = "yyyyMMdd";

	    /// <exception cref="NullReferenceException">Condition.</exception>
	    public static YearGroup GetFirstYearGroup(int? tenantId = null)
        {
            tenantId = tenantId ?? Environment.Settings.TenantId;
            var schoolId = CoreQueries.GetSchoolId();
            var sqlEffectiveDate = DateTime.Now.ToString(SqlDateFormat);

            string sql = String.Format(
                "SELECT TOP 1 yg.ID, yg.ShortName, yg.FullName" +
                 " FROM dbo.YearGroup yg inner join YearGroupSetMembership ygm" +
                 "    on yg.ID = ygm.YearGroup" +
                 "   and ygm.StartDate < '" + sqlEffectiveDate + "' " +
                 "   and (ygm.EndDate >= '" + sqlEffectiveDate + "' " +
                "           or ygm.EndDate is null)" +
                "  inner join SchoolNCYear sncy" +
                "    on yg.SchoolNCYear = sncy.ID" +
                "  inner join NCYearNaturalAgeRange ncy" +
                "    on sncy.NCYear = ncy.NCYear" +
                "    and yg.School = '" + schoolId + "'" +
                "    and yg.TenantID = '{0}'", tenantId);

            var yearGroup = DataAccessHelpers.GetEntities<YearGroup>(sql).FirstOrDefault();

            if (yearGroup == null) throw new NullReferenceException("Could not retrieve YearGroup/YearGroupMembership");

            sql = String.Format("SELECT SchoolNCYear FROM dbo.YearGroup where ID ='{0}'", yearGroup.ID);
            yearGroup.SchoolNCYear = DataAccessHelpers.GetValue<Guid>(sql);

            return yearGroup;
        }
        
    }
}