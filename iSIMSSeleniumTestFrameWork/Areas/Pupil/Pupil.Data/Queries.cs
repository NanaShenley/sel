using System;
using System.Linq;
using System.Runtime.InteropServices;
using Pupil.Data.Entities;
using SeSugar.Data;
using Environment = SeSugar.Environment;

namespace Pupil.Data
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

        public static PrimaryClass GetFirstPrimaryClass(int? tenantId = null, DateTime? dateOfAdmission = null)
        {
            if (dateOfAdmission == null)
            {
                dateOfAdmission = DateTime.Today;
            }

            var schoolId = CoreQueries.GetSchoolId();
            tenantId = tenantId ?? Environment.Settings.TenantId;
            string sql = String.Format(
                @"SELECT TOP 1 pc.ID, pc.FullName
                  FROM dbo.[PrimaryClass] pc INNER JOIN dbo.[YearGroupPrimaryClassAssociation] pca
                  ON pc.ID = pca.PrimaryClass 
                WHERE pc.School = '" + schoolId + "'" +
                  " and pc.TenantID = '{0}'" +
                  " and pc.Active = 1" +
                "AND pca.StartDate <= '{1}' and (pca.EndDate IS NULL OR pca.EndDate>='{1}')", tenantId, dateOfAdmission);

            var primaryClass = DataAccessHelpers.GetEntities<PrimaryClass>(sql).FirstOrDefault();

            return primaryClass;
        }

        public static LookupData GetFirstLookupEntry(string lookupName, int? tenantId = null)
        {
            tenantId = tenantId ?? Environment.Settings.TenantId;

            string sql = String.Format(
                @"SELECT TOP 1 ID, Code, Description
                 FROM dbo.[{0}] 
                WHERE TenantID = '{1}'
                  AND IsVisible = 1", lookupName, tenantId);

            var lookupData = DataAccessHelpers.GetEntities<LookupData>(sql).FirstOrDefault();

            return lookupData;
        }

        public static LookupData GetFirstReasonForLeavingLookup(int? tenantId = null)
        {
            tenantId = tenantId ?? Environment.Settings.TenantId;

            string sql = String.Format(
                "SELECT TOP 1 ID, Code, Description" +
                " FROM dbo.[ReasonForLeaving] " +
                "WHERE TenantID = '{0}'" +
                "  AND IsVisible = 1", tenantId);

            var reasonForLeaving = DataAccessHelpers.GetEntities<LookupData>(sql).FirstOrDefault();

            return reasonForLeaving;
        }

        public static LookupData GetEnrolmentStatus(string code = "C", int? tenantId = null)
        {
            tenantId = tenantId ?? Environment.Settings.TenantId;
            string sql = String.Format(
                "SELECT TOP 1 id, code, description" +
                "  FROM dbo.[EnrolmentStatus] " +
                " WHERE code ='" + code + "'" +
                " and TenantID = '{0}'" +
                " and IsVisible = 1", tenantId);

            var enrolmentStatus = DataAccessHelpers.GetEntities<LookupData>(sql).FirstOrDefault();

            return enrolmentStatus;
        }

        public static LookupData GetFirstAttendanceMode(int? tenantId = null)
        {
            tenantId = tenantId ?? Environment.Settings.TenantId;
            string sql = String.Format(
                "SELECT TOP 1 id, code, description " +
                "  FROM dbo.[AttendanceMode] " +
                " where TenantID = '{0}'" +
                " and IsVisible = 1", tenantId);

            var attendanceMode = DataAccessHelpers.GetEntities<LookupData>(sql).FirstOrDefault();

            return attendanceMode;
        }

        public static LookupData GetFirstBorderStatus(int? tenantId = null)
        {
            tenantId = tenantId ?? Environment.Settings.TenantId;
            string sql = String.Format(
                "SELECT TOP 1 id, code, description " +
                "  FROM dbo.[BoarderStatus] " +
                " where TenantID = '{0}'" +
                " and IsVisible = 1", tenantId);

            var borderStatus = DataAccessHelpers.GetEntities<LookupData>(sql).FirstOrDefault();

            return borderStatus;
        }


        public static ExclusionType GetFirstExclusionType(string excludeDescription = "", int? tenantId = null)
        {
            tenantId = tenantId ?? Environment.Settings.TenantId;
            string sql = String.Format(
                "SELECT TOP 1 et.ID, et.Description" +
                 " FROM dbo.[ExclusionType] et" +
                  " WHERE et.IsVisible = 1 and et.description <>  '" + excludeDescription + "' " +
                "    and et.TenantID = '{0}'" +
                " and IsVisible = 1", tenantId);

            var exclusionType = DataAccessHelpers.GetEntities<ExclusionType>(sql).FirstOrDefault();

            return exclusionType;
        }


        public static ExclusionReason GetFirstExclusionReason(string excludeDescription = "", int? tenantId = null)
        {
            tenantId = tenantId ?? Environment.Settings.TenantId;
            string sql = String.Format(
                "SELECT TOP 1 er.ID, er.Description" +
                 " FROM dbo.[ExclusionReason] er" +
                  " WHERE er.IsVisible = 1 and er.description <> '" + excludeDescription + "' " +
                "     and er.TenantID = '{0}'" +
                " and IsVisible = 1", tenantId);

            var exclusionReason = DataAccessHelpers.GetEntities<ExclusionReason>(sql).FirstOrDefault();

            return exclusionReason;
        }

        /// <summary>
        /// Return a specific lookup / entity description by code
        /// </summary>
        /// <param name="entityName">Name of the entity.</param>
        /// <param name="isVisibleOnly">if set to <c>true</c> [is visible only].</param>
        /// <param name="tenantId">The tenant identifier.</param>
        /// <returns></returns>
        public static string GetLookupDescription(string entityName, bool isVisibleOnly = true, int? tenantId = null)
        {
            tenantId = tenantId ?? Environment.Settings.TenantId;
            string sql = $"SELECT TOP 1 Description FROM dbo.{entityName} WHERE TenantID = '{tenantId}'";

            if (isVisibleOnly)
            {
                sql += " and IsVisible = 1";
            }

            string lookupEntry = DataAccessHelpers.GetValue<string>(sql);
            return lookupEntry;
        }

        /// <summary>
        /// Return a specific lookup / entity description by code
        /// </summary>
        /// <param name="entityName">Name of the entity.</param>
        /// <param name="code">The code.</param>
        /// <param name="isVisibleOnly">if set to <c>true</c> [is visible only].</param>
        /// <param name="tenantId">The tenant identifier.</param>
        /// <returns></returns>
        public static string GetLookupDescriptionByCode(string entityName, string code, bool isVisibleOnly = false, int? tenantId = null)
        {
            tenantId = tenantId ?? Environment.Settings.TenantId;
            string sql = $"SELECT TOP 1 Description FROM dbo.{entityName} WHERE Code LIKE '{code}' and TenantID = '{tenantId}'";

            if (isVisibleOnly)
            {
                sql += " and IsVisible = 1";
            }

            string lookupEntry = DataAccessHelpers.GetValue<string>(sql);
            return lookupEntry;
        }

        /// <summary>
        /// Gets the first visible consent type for the tenant
        /// </summary>
        /// <param name="tenantId"></param>
        /// <returns></returns>
        public static LookupData GetFirstConsentType(int? tenantId = null)
        {
            tenantId = tenantId ?? Environment.Settings.TenantId;
            string sql = String.Format(
                "SELECT TOP 1 id, code, description " +
                "  FROM dbo.[ConsentType] " +
                " where TenantID = '{0}'" +
                " and IsVisible = 1", tenantId);
            var consentType = DataAccessHelpers.GetEntities<LookupData>(sql).FirstOrDefault();

            return consentType;

        }

        public static LookupData GetFirstConsentStatus(int? tenantId = null)
        {
            tenantId = tenantId ?? Environment.Settings.TenantId;
            string sql = String.Format(
                "SELECT TOP 1 id, code, description " +
                "  FROM dbo.[ConsentStatus] " +
                " where TenantID = '{0}'" +
                " and IsVisible = 1", tenantId);
            var consentStatus = DataAccessHelpers.GetEntities<LookupData>(sql).FirstOrDefault();

            return consentStatus;
        }

        public static Guid GetLearnerId(string legalSurname, string legalforename, string dob)
        {
            var schoolId = CoreQueries.GetSchoolId();

            string sql = String.Format(
                "SELECT TOP 1 l.ID " +
                "FROM dbo.Learner L " +
                "WHERE L.School = '" + schoolId + "' " +
                "AND L.LegalSurname = '" + legalSurname + "' " +
                "AND L.LegalForename = '" + legalforename + "' " +
                "AND L.DateOfBirth = '" + dob + "' ");

            return DataAccessHelpers.GetValue<Guid>(sql);
        }

        public static Guid GetApplicationId(Guid learnerId)
        {
            string sql = String.Format(
                "SELECT TOP 1 A.ID " +
                "FROM dbo.Application A " +
                "WHERE A.Learner = '" + learnerId + "' ");

            return DataAccessHelpers.GetValue<Guid>(sql);
        }

        /// <summary>
        /// Gets achievement lookup data for the first event with an event level equal to 1
        /// (i.e. would require the quick event form)
        /// </summary>
        /// <param name="tenantId"></param>
        /// <returns></returns>
        public static LookupData GetFirstQuickConductEventTypeLookup(string conductType, int? tenantId = null)
        {
            tenantId = tenantId ?? Environment.Settings.TenantId;

            string sql = String.Format(@"
                SELECT TOP 1 aec.ID, aec.[Description]
                FROM {1}EventCategory aec
                INNER JOIN {1}EventCategoryLevel aecl
                    ON(aecl.ID = aec.{1}EventCategoryLevel)
                WHERE aec.TenantID = {0}
                AND aecl.[Level] = 1
                ORDER BY aec.DisplayOrder", tenantId, conductType);

            return DataAccessHelpers.GetEntities<LookupData>(sql).FirstOrDefault();
        }

        /// <summary>
        /// Gets achievement lookup data for the first event with an event level higher than 1
        /// (i.e. would require the full event form)
        /// </summary>
        /// <param name="tenantId"></param>
        /// <returns></returns>
        public static LookupData GetFirstFullConductEventLookup(string conductType, int? tenantId = null)
        {
            tenantId = tenantId ?? Environment.Settings.TenantId;

            string sql = String.Format(@"
                SELECT TOP 1 aec.ID, aec.[Description]
                FROM {1}EventCategory aec
                INNER JOIN {1}EventCategoryLevel aecl
                    ON(aecl.ID = aec.{1}EventCategoryLevel)
                WHERE aec.TenantID = {0}
                AND aecl.[Level] > 1
                ORDER BY aec.DisplayOrder", tenantId, conductType);

            return DataAccessHelpers.GetEntities<LookupData>(sql).FirstOrDefault();
        }

        // TODO: This method will be called from ConductNavigation before navigating to Achivement/Behaviour Events menu item.
        public static string GetConfiguredConductLabel(string conductType, int? tenantId = null)
        {
            tenantId = tenantId ?? Environment.Settings.TenantId;

            string sql = string.Format(@"
                SELECT TOP 1 {1}Label
                FROM dbo.ConductServiceSetup
                WHERE TenantID = {0}", tenantId, conductType);

            return DataAccessHelpers.GetValue<string>(sql);
        }

        /// <summary>
        /// Gets achievement lookup data for the first event with an event level higher than 1
        /// (i.e. would require the full event form)
        /// </summary>
        /// <param name="tenantId"></param>
        /// <returns></returns>
        public static decimal GetDefaultPointsForFirstFullConductEventCategoryLookup(string conductType, int? tenantId = null)
        {
            tenantId = tenantId ?? Environment.Settings.TenantId;

            string sql = String.Format(@"
                SELECT TOP 1 aecl.MinimumPoints
                FROM {1}EventCategory aec
                INNER JOIN {1}EventCategoryLevel aecl
                    ON(aecl.ID = aec.{1}EventCategoryLevel)
                WHERE aec.TenantID = {0}
                AND aecl.[Level] > 1
                ORDER BY aec.DisplayOrder", tenantId, conductType);

            return DataAccessHelpers.GetEntities<decimal>(sql).FirstOrDefault();
        }

        public static string GetFirstLookupField(string lookupName, string fieldName, int? tenantId = null)
        {
            tenantId = tenantId ?? Environment.Settings.TenantId;

            string sql = String.Format(
                @"SELECT TOP 1 {0}
                 FROM dbo.[{1}] 
                WHERE TenantID = '{2}'
                  AND IsVisible = 1", fieldName, lookupName, tenantId);

            var lookupData = DataAccessHelpers.GetEntities<string>(sql).FirstOrDefault();

            return lookupData;
        }

        #region Report Card Queries

       

        #endregion

    }
}