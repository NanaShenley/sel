using System;
using System.Linq;
using SeSugar.Data;
using Attendance.POM.Entities;
using Environment = SeSugar.Environment;
using System.Collections.Generic;

namespace Attendance.POM.DataHelper
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

        public static YearGroup GetYearGroup(string yeargroup,int? tenantId = null)
        {
            tenantId = tenantId ?? Environment.Settings.TenantId;
            var schoolId = CoreQueries.GetSchoolId();
            string sql = String.Format(
                "SELECT TOP 1 yg.ID, yg.ShortName, yg.FullName" +
                 " FROM dbo.YearGroup yg  where FullName = '"+yeargroup+"'"+
                           "    and yg.School = '" + schoolId + "'" +
                "    and yg.TenantID = '{0}'", tenantId);
            var yearGroup = DataAccessHelpers.GetEntities<YearGroup>(sql).FirstOrDefault();
            return yearGroup;
        }

        public static PrimaryClass GetFirstPrimaryClass(int? tenantId = null)
        {
            tenantId = tenantId ?? Environment.Settings.TenantId;
            var schoolId = CoreQueries.GetSchoolId();
            var sqlEffectiveDate = DateTime.Now.ToString(SqlDateFormat);
            string sql = String.Format(
                "SELECT TOP 1 pc.ID, pc.ShortName, pc.FullName" +
                " FROM dbo.PrimaryClass pc inner join PrimaryClassSetMembership pcm" +
                "   on pc.id = pcm.PrimaryClass" +
                "   and pcm.StartDate < '" + sqlEffectiveDate + "' " +
                "  and (pcm.EndDate >= '" + sqlEffectiveDate + "' " +
                "           or pcm.EndDate is null)" +
                 " and pc.school = '" + schoolId + "'" +
                  " and pc.TenantID = '{0}'", tenantId);

            var primaryClass = DataAccessHelpers.GetEntities<PrimaryClass>(sql).FirstOrDefault();
            if (primaryClass == null) throw new NullReferenceException("Could not retrieve YearGroup/YearGroupMembership");
            sql = String.Format("SELECT SchoolNCYear FROM dbo.YearGroup where ID ='{0}'", primaryClass.ID);
            primaryClass.SchoolNCYear = DataAccessHelpers.GetValue<Guid>(sql);

            return primaryClass;
        }

        public static PrimaryClass GetPrimaryClass(string primaryclass, int? tenantId = null)
        {
            tenantId = tenantId ?? Environment.Settings.TenantId;
            var schoolId = CoreQueries.GetSchoolId();
            string sql = String.Format(
                "SELECT TOP 1 pc.ID, pc.ShortName, pc.FullName" +
                 " FROM dbo.PrimaryClass pc  where FullName = '" + primaryclass + "'" +
                           "    and pc.School = '" + schoolId + "'" +
                "    and pc.TenantID = '{0}'", tenantId);
            var primaryClass = DataAccessHelpers.GetEntities<PrimaryClass>(sql).FirstOrDefault();
            return primaryClass;
        }

        public static AcademicYear GetAdmissionYear(string academicYearName, int? tenantId = null)
        {
            tenantId = tenantId ?? Environment.Settings.TenantId;
            string sql = String.Format(
                "SELECT TOP 1 ac.ID, ac.Name" +
                 " FROM dbo.[ReferenceAcademicYear] ac" +
                 " WHERE ac.Name = '" + academicYearName + "'" +
                " and ac.TenantID = '{0}'", tenantId);


            var academicYear = DataAccessHelpers.GetEntities<AcademicYear>(sql).FirstOrDefault();

            return academicYear;
        }

        public static LookupData GetEnrolmentStatus(string code = "C", int? tenantId = null)
        {
            tenantId = tenantId ?? Environment.Settings.TenantId;
            string sql = String.Format(
                "SELECT TOP 1 id, code, description" +
                "  FROM dbo.[EnrolmentStatus] " +
                " WHERE code ='" + code + "'" +
                " and TenantID = '{0}'", tenantId);

            var enrolmentStatus = DataAccessHelpers.GetEntities<LookupData>(sql).FirstOrDefault();

            return enrolmentStatus;
        }


        public static Guid GetExceptionalCircumstanceTypeID(string code, int? tenantId = null)
        {
            tenantId = tenantId ?? Environment.Settings.TenantId;

            string sql = String.Format(
                "SELECT TOP 1 id" +
                "  FROM dbo.[exceptionalcircumstancetype] " +
                " WHERE code ='" + code + "'" +
                " and TenantID = '{0}'", tenantId);
            return DataAccessHelpers.GetValue<Guid>(sql);
        }

        public static Guid GetStartSessionid(string name = "AM", int? tenantId = null)
        {
            tenantId = tenantId ?? Environment.Settings.TenantId;
            string sql = String.Format(
                      "SELECT TOP 1 id" +
                "  FROM dbo.[workingweeksession] " +
                " WHERE Name ='" + name + "'" +
                " and TenantID = '{0}'", tenantId);
            return DataAccessHelpers.GetValue<Guid>(sql);
        }

        public static Guid GetEndSessionid(string name = "PM", int? tenantId = null)
        {
            tenantId = tenantId ?? Environment.Settings.TenantId;
            string sql = String.Format(
                      "SELECT TOP 1 id" +
                "  FROM dbo.[workingweeksession] " +
                " WHERE Name ='" + name + "'" +
                " and TenantID = '{0}'", tenantId);
            return DataAccessHelpers.GetValue<Guid>(sql);
        }

        public static IEnumerable<SchoolAttendanceCode> GetAllAttendanceCodes(int? tenantId = null)
        {
            tenantId = tenantId ?? Environment.Settings.TenantId;
            string sql = String.Format(
                      "SELECT Code" +
                "  FROM dbo.[attendancecodecategory] "
               + " WHERE TenantID = '{0}'", tenantId);
            return DataAccessHelpers.GetEntities<SchoolAttendanceCode>(sql);
        }

        internal static Guid GetSchoolAttendanceCode(string code = "Y", int? tenantId = null)
        {
            tenantId = tenantId ?? Environment.Settings.TenantId;
            string sql = String.Format(
                      "SELECT TOP 1 id" +
                "  FROM dbo.[schoolattendancecode] " +
                " WHERE Code ='" + code + "'" +
                " and TenantID = '{0}'", tenantId);
            return DataAccessHelpers.GetValue<Guid>(sql);
        }

        public static IEnumerable<SchoolAttendanceCode> GetCodesAvailableOnDealWithSpecificMarks(int? tenantId = null)
        {
            tenantId = tenantId ?? Environment.Settings.TenantId;
            string sql = String.Format(
                      "SELECT Code, Description" +
                "  FROM dbo.[schoolattendancecode] " +
                " WHERE AttendanceCodeCategory IN (Select id FROM dbo.[AttendanceCodeCategory] where active = '1' and ispresent ='0' and statisticalcategory NOT IN (select id from statisticalcategory where code = 'ANR'))"
               + " and TenantID = '{0}'", tenantId);
            return DataAccessHelpers.GetEntities<SchoolAttendanceCode>(sql);
        }

        public static IEnumerable<SchoolAttendanceCode> GetCodesAvailableOnApplyMarkPage(int? tenantId = null)
        {
            tenantId = tenantId ?? Environment.Settings.TenantId;
            string sql = String.Format(
                      "SELECT Code, Description" +
                "  FROM dbo.[schoolattendancecode] " +
                " WHERE AttendanceCodeCategory IN (Select id FROM dbo.[AttendanceCodeCategory] where ForAdminUse='1')"
               + " and TenantID = '{0}'", tenantId);
            return DataAccessHelpers.GetEntities<SchoolAttendanceCode>(sql);
        }

        public static IEnumerable<SchoolAttendanceCode> GetCodesAvailableOnAttendancePatternPage(int? tenantId = null)
        {
            tenantId = tenantId ?? Environment.Settings.TenantId;
            string sql = String.Format(
                      "SELECT Code, Description" +
                "  FROM dbo.[schoolattendancecode] " +
                " WHERE AttendanceCodeCategory IN (Select id FROM dbo.[AttendanceCodeCategory] where ForAdminUse = '1') and AttendanceCodeCategory NOt IN (select id from AttendanceCodeCategory where IsPresent ='1')"
               + " and TenantID = '{0}'", tenantId);
            return DataAccessHelpers.GetEntities<SchoolAttendanceCode>(sql);
        }

        public static IEnumerable<SchoolAttendanceCode> GetAttendanceNotRequiredCodes(int? tenantId = null)
        {
            tenantId = tenantId ?? Environment.Settings.TenantId;
            string sql = String.Format(
                      "SELECT Code" +
                "  FROM dbo.[attendancecodecategory] " +
                " WHERE statisticalcategory IN (select id from statisticalcategory where code = 'ANR')"
               + " and TenantID = '{0}'", tenantId);
            return DataAccessHelpers.GetEntities<SchoolAttendanceCode>(sql);
        }

        public static IEnumerable<SchoolAttendanceCode> GetHolidaysAndInsetDays(int? tenantId = null)
        {
            tenantId = tenantId ?? Environment.Settings.TenantId;
            string sql = String.Format(
                      "SELECT Code" +
                "  FROM dbo.[attendancecodecategory] " +
                " WHERE (IsHoliday=1 or IsInsetDay=1)"
               + " and TenantID = '{0}'", tenantId);
            return DataAccessHelpers.GetEntities<SchoolAttendanceCode>(sql);
        }

        public static IEnumerable<SchoolAttendanceCode> GetHolidays(int? tenantId = null)
        {
            tenantId = tenantId ?? Environment.Settings.TenantId;
            string sql = String.Format(
                      "SELECT Code" +
                "  FROM dbo.[attendancecodecategory] " +
                " WHERE IsHoliday=1"
               + " and TenantID = '{0}'", tenantId);
            return DataAccessHelpers.GetEntities<SchoolAttendanceCode>(sql);
        }

        public static IEnumerable<SchoolAttendanceCode> GetInsetDay(int? tenantId = null)
        {
            tenantId = tenantId ?? Environment.Settings.TenantId;
            string sql = String.Format(
                      "SELECT Code" +
                "  FROM dbo.[attendancecodecategory] " +
                " WHERE IsInsetDay=1"
               + " and TenantID = '{0}'", tenantId);
            return DataAccessHelpers.GetEntities<SchoolAttendanceCode>(sql);
        }
    }
}