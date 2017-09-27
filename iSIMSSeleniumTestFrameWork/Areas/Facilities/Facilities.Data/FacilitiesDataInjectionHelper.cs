using System;
using SeSugar.Data;
using Environment = SeSugar.Environment;

namespace Facilities.Data
{
    public static class FacilitiesDataInjectionHelper
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
                SchoolNCYear = schoolNCYearId,
                DisplayOrder = displayOrder
            })
            .Add(Constants.Tables.YearGroupSetMembership, new
            {
                ID = Guid.NewGuid(),
                YearGroup = yearGroupId,
                TenantID = tenantId,
                StartDate = new DateTime(2015, 01, 01)
            });
            return dataPackage;
        }

        public static DataPackage AddEmployee(this DataPackage dataPackage, Guid employeeId, int? tenantId = null)
        {
            tenantId = tenantId ?? Environment.Settings.TenantId;
            dataPackage.AddData(Constants.Tables.Employee, new
            {
                ID = employeeId,
                TenantID = tenantId
            });
            return dataPackage;
        }

        public static DataPackage AddStaff(this DataPackage dataPackage, Guid staffId, Guid employeeId, string surname, string forename, int? tenantId = null)
        {
            tenantId = tenantId ?? Environment.Settings.TenantId;
            dataPackage.AddData(Constants.Tables.Staff, new
            {
                ID = staffId,
                LegalForename = forename,
                LegalSurname = surname,
                PreferredForename = forename,
                PreferredSurname = surname,
                DateOfBirth = new DateTime(2000, 1, 1),
                Gender = CoreQueries.GetLookupItem("Gender", description: "Male"),
                PolicyACLID = CoreQueries.GetPolicyAclId("Staff"),
                Employee = employeeId,
                School = CoreQueries.GetSchoolId(),
                TenantID = tenantId
            });
            return dataPackage;
        }


        public static DataPackage AddServiceRecord(this DataPackage dataPackage, Guid serviceRecordId, Guid staffId, DateTime staffStartDate, DateTime? dol = null, int? tenantId = null)
        {
            tenantId = tenantId ?? Environment.Settings.TenantId;
            dataPackage.AddData(Constants.Tables.StaffServiceRecord, new
            {
                ID = serviceRecordId,
                DOA = staffStartDate,
                DOL = dol,
                ContinuousServiceStartDate = staffStartDate,
                LocalAuthorityStartDate = staffStartDate,
                Staff = staffId,
                TenantID = tenantId
            });
            return dataPackage;
        }

        public static DataPackage AddBasicLearner(this DataPackage dataPackage, Guid learnerId, string surname, string forename, DateTime dateOfBirth, DateTime dateOfAdmission, string genderCode = "1", string enrolStatus = "C", Guid? uniqueLearnerEnrolmentId = null, int? tenantId = null, string salutation = null, string addressee = null, Guid? yearGroupId = null, Guid? schoolNCYearId = null)
        {
            tenantId = tenantId ?? Environment.Settings.TenantId;

            string sql = String.Format(
                "UPDATE School SET LastAdmissionNumber='B05000' " +
                "WHERE TenantID='" + tenantId + "' " +
                "AND IsRegistered=1 " +
                "AND (LastAdmissionNumber IS NULL " +
                "OR CAST(SUBSTRING(LastAdmissionNumber,2,10) AS INT) < 5000)");

            DataAccessHelpers.Execute(sql);

            Guid learnerEnrolmentId;
            var yearGroup = Queries.GetFirstYearGroup();
            dataPackage.AddData("Learner", new
            {
                Id = learnerId,
                School = CoreQueries.GetSchoolId(),
                Gender = CoreQueries.GetLookupItem("Gender", code: genderCode),
                LegalForename = forename,
                LegalSurname = surname,
                DateOfBirth = dateOfBirth,
                TenantID = tenantId,
                ParentalSalutation = salutation,
                ParentalAddressee = addressee,
                PolicyAclId = CoreQueries.GetPolicyAclId("Learner")
            });
            dataPackage.AddData("LearnerEnrolment", new
            {
                Id = learnerEnrolmentId = uniqueLearnerEnrolmentId ?? Guid.NewGuid(),
                School = CoreQueries.GetSchoolId(),
                Learner = learnerId,
                DOA = dateOfAdmission,
                TenantID = tenantId

            });
            dataPackage.AddData("LearnerEnrolmentStatus", new
            {
                Id = Guid.NewGuid(),
                LearnerEnrolment = learnerEnrolmentId,
                StartDate = dateOfAdmission,
                EnrolmentStatus = CoreQueries.GetLookupItem("EnrolmentStatus", code: enrolStatus),
                TenantID = tenantId
            });
            dataPackage.AddData("LearnerYearGroupMembership", new
            {
                Id = Guid.NewGuid(),
                Learner = learnerId,
                YearGroup = yearGroupId ?? yearGroup.ID,
                StartDate = dateOfAdmission,
                TenantID = tenantId
            });
            dataPackage.AddData("LearnerNCYearMembership", new
            {
                Id = Guid.NewGuid(),
                Learner = learnerId,
                SchoolNCYear = schoolNCYearId ?? yearGroup.SchoolNCYear,
                StartDate = dateOfAdmission,
                TenantID = tenantId
            });
            return dataPackage;
        }


        public static DataPackage AddClasses(this DataPackage dataPackage, Guid classId, string shortName, string fullName, int displayOrder, int? tenantId = null)
        {
            tenantId = tenantId ?? Environment.Settings.TenantId;
            dataPackage.AddData(Constants.Tables.PrimaryClass, new
            {
                ID = classId,
                TenantID = tenantId,
                ShortName = shortName,
                FullName = fullName,
                School = CoreQueries.GetSchoolId(),
                DisplayOrder = displayOrder
            });
            return dataPackage;
        }

        public static DataPackage AddCalendar(this DataPackage dataPackage, Guid calendarId, string calendarName, string calendarDescription, int? tenantId = null)
        {
            tenantId = tenantId ?? Environment.Settings.TenantId;

            string sql = string.Format("SELECT TOP 1 ID FROM dbo.CalendarGroup where TenantID ='{0}'", tenantId);
            Guid calendarGroupId = DataAccessHelpers.GetValue<Guid>(sql);

            dataPackage.AddData(Constants.Tables.Calendar, new
            {
                ID = calendarId,
                TenantID = tenantId,
                Name = calendarName,
                Description = calendarDescription,
                IsVisible =  true,

                     CalendarGroup = calendarGroupId
            });          
            return dataPackage;
        }
    }
}
