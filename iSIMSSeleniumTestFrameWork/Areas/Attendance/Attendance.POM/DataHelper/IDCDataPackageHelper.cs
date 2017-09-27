using SeSugar.Data;
using System;
using TestSettings;
using Environment = SeSugar.Environment;

namespace Attendance.POM.DataHelper
{
    public static class IDCDataPackageHelper
    {
        private static readonly int tenantId = SeSugar.Environment.Settings.TenantId;

        public static object GenerateStaff(out Guid id, string surname, Guid? employeeId = null, string forename = null)
        {
            return new
            {
                ID = id = Guid.NewGuid(),
                LegalForename = forename,
                LegalSurname = surname,
                LegalMiddleNames = "Middle Names",
                PreferredForename = forename,
                PreferredSurname = surname,
                DateOfBirth = new DateTime(2000, 1, 1),
                Gender = CoreQueries.GetLookupItem("Gender", description: "Male"),
                PolicyACLID = CoreQueries.GetPolicyAclId("Staff"),
                Employee = employeeId,
                School = CoreQueries.GetSchoolId(),
                TenantID = tenantId
            };
        }

        public static DataPackage GetStaffRecord(this DataPackage dataPackage, Guid staffId, string forename, string surname)
        {
            //return this.BuildDataPackage()
               dataPackage.AddData("Staff", new
               {
                   Id = staffId,
                   LegalForename = forename,
                   LegalSurname = surname,
                   LegalMiddleNames = "Middle",
                   PreferredForename = forename,
                   PreferredSurname = surname,
                   DateOfBirth = new DateTime(2000, 1, 1),
                   Gender = CoreQueries.GetLookupItem("Gender", description: "Female"),
                   PolicyACLID = CoreQueries.GetPolicyAclId("Staff"),
                   School = CoreQueries.GetSchoolId(),
                   TenantID = tenantId
               })
               .AddData("StaffServiceRecord", new
               {
                   Id = Guid.NewGuid(),
                   DOA = DateTime.Today.AddDays(-1),
                   ContinuousServiceStartDate = DateTime.Today.AddDays(-1),
                   LocalAuthorityStartDate = DateTime.Today.AddDays(-1),
                   Staff = staffId,
                   TenantID = tenantId
               });
            return dataPackage;
        }

        public static DataPackage GenerateClass(this DataPackage datapackage, Guid classid, string classname, string shortname)
        {
            datapackage.AddData("PrimaryClass", new
            {
                ID = classid = Guid.NewGuid(),
                FullName = classname,
                ShortName = shortname,
                Active = "1",
                School = CoreQueries.GetSchoolId(),
                TenantID = SeSugar.Environment.Settings.TenantId,
            });
            return datapackage;
        }

        public static DataPackage AddBasicLearner(this DataPackage dataPackage, Guid learnerId, string surname, string forename, DateTime dateOfBirth, DateTime dateOfAdmission, string genderCode = "1", string enrolStatus = "C", Guid? uniqueLearnerEnrolmentId = null, int? tenantId = null, string salutation = null, string addressee = null, Guid? yearGroupId = null, Guid? primaryClassId = null, Guid? schoolNCYearId = null)
        {
            tenantId = tenantId ?? Environment.Settings.TenantId;

            string sql = String.Format(
               "UPDATE School SET LastAdmissionNumber='D05000' " +
                "WHERE TenantID='" + tenantId + "' " +
                "AND IsRegistered=1 " +
                "AND (LastAdmissionNumber IS NULL " +
                "OR CAST(SUBSTRING(LastAdmissionNumber,2,10) AS INT) < 5000)");

            DataAccessHelpers.Execute(sql);

            Guid learnerEnrolmentId;
            var yearGroup = Queries.GetFirstYearGroup();
            var primaryClass = Queries.GetFirstPrimaryClass();

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
            dataPackage.AddData("LearnerPrimaryClassMembership", new
            {
                Id = Guid.NewGuid(),
                Learner = learnerId,
                PrimaryClass = primaryClassId ?? primaryClass.ID,
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

        public static DataPackage CreatePupil(this DataPackage dataPackage, Guid learnerId, string surname, string forename, DateTime dateOfBirth, DateTime dateOfAdmission, string YearGroup, string genderCode = "1", string enrolStatus = "C", Guid? uniqueLearnerEnrolmentId = null, int? tenantId = null, string salutation = null, string addressee = null, Guid? yearGroupId = null, Guid? primaryClassId = null, Guid? schoolNCYearId = null)
        {
            tenantId = tenantId ?? Environment.Settings.TenantId;
            string sql = String.Format(
                "UPDATE School SET LastAdmissionNumber='C05000' " +
                "WHERE TenantID='" + tenantId + "' " +
                "AND IsRegistered=1 " +
                "AND (LastAdmissionNumber IS NULL " +
                "OR CAST(SUBSTRING(LastAdmissionNumber,2,10) AS INT) < 5000)");

            DataAccessHelpers.Execute(sql);

            Guid learnerEnrolmentId;
            var yearGroup = Queries.GetYearGroup(YearGroup);
            var primaryClass = Queries.GetFirstPrimaryClass();

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
            dataPackage.AddData("LearnerPrimaryClassMembership", new
            {
                Id = Guid.NewGuid(),
                Learner = learnerId,
                PrimaryClass = primaryClassId ?? primaryClass.ID,
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

        public static DataPackage AddLeaver(this DataPackage dataPackage, Guid learnerId, string surname,
             string forename, DateTime dateOfBirth, DateTime dateOfAdmission, DateTime dateOfLeaving, string YearGroup, string Class, string genderCode = "1",
             string enrolStatus = "C", string reasonForLeaving = "F", string tenantId = null)
        {
            Guid learnerEnrolmentId;
            var yearGroup = Queries.GetYearGroup(YearGroup);
            var primaryClass = Queries.GetPrimaryClass(Class);
            tenantId = tenantId ?? TestDefaults.Default.TenantId.ToString();
            dataPackage.AddData("Learner", new
            {
                Id = learnerId,
                School = CoreQueries.GetSchoolId(),
                Gender = CoreQueries.GetLookupItem("Gender", code: genderCode),
                LegalForename = forename,
                LegalSurname = surname,
                DateOfBirth = dateOfBirth,
                PolicyAclId = CoreQueries.GetPolicyAclId("Learner"),
                TenantID = tenantId
            })
                .AddData("LearnerEnrolment", new
                {
                    Id = learnerEnrolmentId = Guid.NewGuid(),
                    School = CoreQueries.GetSchoolId(),
                    Learner = learnerId,
                    DOA = dateOfAdmission,
                    DOL = dateOfLeaving,
                    ReasonForLeaving = CoreQueries.GetLookupItem("ReasonForLeaving", code: reasonForLeaving),
                    TenantID = tenantId
                })
                .AddData("LearnerEnrolmentStatus", new
                {
                    Id = Guid.NewGuid(),
                    LearnerEnrolment = learnerEnrolmentId,
                    StartDate = dateOfAdmission,
                    EndDate = dateOfLeaving,
                    EnrolmentStatus = CoreQueries.GetLookupItem("EnrolmentStatus", code: enrolStatus),
                    TenantID = tenantId
                })
                .AddData("LearnerYearGroupMembership", new
                {
                    Id = Guid.NewGuid(),
                    Learner = learnerId,
                    YearGroup = yearGroup.ID,
                    StartDate = dateOfAdmission,
                    EndDate = dateOfLeaving,
                    TenantID = tenantId
                })
                .AddData("LearnerPrimaryClassMembership", new
                {
                    Id = Guid.NewGuid(),
                    Learner = learnerId,
                    PrimaryClass = primaryClass.ID,
                    StartDate = dateOfAdmission,
                    EndDate = dateOfLeaving,
                    TenantID = tenantId
                })
                .AddData("LearnerNCYearMembership", new
                {
                    Id = Guid.NewGuid(),
                    Learner = learnerId,
                    SchoolNCYear = yearGroup.SchoolNCYear,
                    StartDate = dateOfAdmission,
                    EndDate = dateOfLeaving,
                    TenantID = tenantId
                });
            return dataPackage;
        }

        public static DataPackage AddYearGroupLookup(this DataPackage dataPackage, Guid yearGroupId, Guid schoolNCYearId, string shortName, string fullName, int displayOrder, int? tenantId = null)
        {
            //tenantId = tenantId ?? Environment.Settings.TenantId;
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
                StartDate = new DateTime(2015, 01, 01)
            });
            return dataPackage;
        }


        public static DataPackage GenerateEarlyYearProvision(this DataPackage datapackage, Guid provisionId, string provisionname, string provisionshortname, string startdate, string starttime, string endtime)
        {
            datapackage.AddData("EarlyYearsSessionType", new
            {
                ID = provisionId,
                School = CoreQueries.GetSchoolId(),
                TenantID = tenantId,
                ProvisionName = provisionname,
                ShortName = provisionshortname,
                StartDate = startdate,
                //EndDate = enddate,
                StartTime = starttime,
                EndTime = endtime,
            });
            return datapackage;
        }

        public static DataPackage AddExceptionalCircumstance(this DataPackage datapackage, Guid id, Guid learnerid, string forename, string surname, Guid exceptionId, string description, string startdate, string enddate, string code)
        {
            var ExceptionalCircumstanceTypeid = Queries.GetExceptionalCircumstanceTypeID(code);
            var startsessionid = Queries.GetStartSessionid();
            var endsessionid = Queries.GetEndSessionid();
            var schoolattendancecodeid = Queries.GetSchoolAttendanceCode();          

            datapackage.AddData("exceptionalcircumstance", new
            {
                ID = exceptionId,
                School = CoreQueries.GetSchoolId(),
                TenantID = tenantId,
                Description = description,
                StartDate = startdate,
                EndDate = enddate,
                ExceptionalCircumstanceType = ExceptionalCircumstanceTypeid,
                StartSession = startsessionid,
                EndSession = endsessionid,
                SchoolAttendanceCode = schoolattendancecodeid,
            });

            datapackage.AddData("learnerexceptionalcircumstance", new
            {
                ID = id = Guid.NewGuid(),
                TenantID = tenantId,
                ExceptionalCircumstance = exceptionId,
                Learner = learnerid,
            });
            return datapackage;
        }

        public static DataPackage GenerateExceptionalCircumstance(this DataPackage datapackage, Guid exceptionId, string description, string startdate, string enddate, string code)
        {
            var ExceptionalCircumstanceTypeid = Queries.GetExceptionalCircumstanceTypeID(code);
            var startsessionid = Queries.GetStartSessionid();
            var endsessionid = Queries.GetEndSessionid();
            var schoolattendancecodeid = Queries.GetSchoolAttendanceCode();
            datapackage.AddData("exceptionalcircumstance", new
            {
                ID = exceptionId,
                School = CoreQueries.GetSchoolId(),
                TenantID = tenantId,
                Description = description,
                StartDate = startdate,
                EndDate = enddate,
                ExceptionalCircumstanceType = ExceptionalCircumstanceTypeid,
                StartSession = startsessionid,
                EndSession = endsessionid,
                SchoolAttendanceCode = schoolattendancecodeid,
            });
            return datapackage;
        }

        public static object GenerateAgency(out Guid id, string AgencyName)
        {
            return new
            {
                ID = id = Guid.NewGuid(),
                AgencyName = AgencyName,
                ResourceProvider = CoreQueries.GetSchoolId(),
                TenantID = tenantId
            };
        }

        public static object GenerateAgent(out Guid id, string AgencyForName, string AgencySurname)
        {
            return new
            {
                ID = id = Guid.NewGuid(),
                LegalForename = AgencyForName,
                LegalSurname = AgencySurname,
                ResourceProvider = CoreQueries.GetSchoolId(),
                TenantID = tenantId
            };
        }

        public static object GenerateMedicalPractice(out Guid id, string MedicalPracticeName)
        {
            return new
            {
                ID = id = Guid.NewGuid(),
                Name = MedicalPracticeName,
                ResourceProvider = CoreQueries.GetSchoolId(),
                TenantID = tenantId
            };
        }

         public static DataPackage AddPupilContact(this DataPackage dataPackage, 
            Guid pupilContactId, 
            string surname, 
            string forename,             
            string salutation = null,
            string addressee = null,
            string jobTitle = null,
            string titleCode = "Mr",
            string genderCode = "1",
            string occupation = "ACC",
            int?    tenantId = null
            )
        {
            tenantId = tenantId ?? Environment.Settings.TenantId;
            dataPackage.AddData(Constants.Tables.LearnerContact, new
            {
                ID = pupilContactId,
                Surname = surname,
                Forename = forename,
                School = CoreQueries.GetSchoolId(),
                Gender = CoreQueries.GetLookupItem(Constants.Tables.Gender, code: genderCode),
                Title = CoreQueries.GetLookupItem(Constants.Tables.Title, code: titleCode),
                Salutation = salutation,
                JobTitle = jobTitle,
                Addressee = addressee,
                Occupation = CoreQueries.GetLookupItem(Constants.Tables.Occupation, code: occupation),
                TenantID = tenantId
            });
            return dataPackage;
        }

        public static DataPackage AddPupilContactRelationship(this DataPackage dataPackage,
            Guid pupilContactRelationshipId,
            Guid learnerId,
            Guid pupilContactId,
            string relationshipType = "PAR",
            int priority = 1,
            bool hasParentalResponsibility = false,
            bool receivesCorrespondance = false,
            bool hasSchoolReport = false,
            bool hasCourtOrder = false,
            int? tenantId = null)
        {
            tenantId = tenantId ?? Environment.Settings.TenantId;
            dataPackage.AddData(Constants.Tables.LearnerContactRelationship, new
            {
                ID = pupilContactRelationshipId,
                Priority = priority,
                Learner = learnerId,
                LearnerContact = pupilContactId,
                LearnerContactRelationshipType = CoreQueries.GetLookupItem(Constants.Tables.LearnerContactRelationshipType, relationshipType),
                HasParentalResponsibility = hasParentalResponsibility,
                ReceivesCorrespondance = receivesCorrespondance,
                HasCourtOrder = hasCourtOrder,
                ReceivesSchoolReport = hasSchoolReport,
                TenantID = tenantId
            });

            return dataPackage;
        }

        public static DataPackage AddPupilDateOfLeaving(this DataPackage datapackage, Guid learnerEnrolmentId, Guid learnerId, string leavingdate)
        {
            datapackage.AddData("LearnerEnrolment", new
            {
                ID = learnerEnrolmentId,
                Learner = learnerId,
                School = CoreQueries.GetSchoolId(),
                TenantID = tenantId,
                DOL = leavingdate,
            });
            return datapackage;
        }


        public static object GenerateServiceRecord(out Guid id, Guid staffId, DateTime doa, DateTime? dol = null)
        {
            return new
            {
                ID = id = Guid.NewGuid(),
                DOA = doa,
                DOL = dol,
                ContinuousServiceStartDate = doa,
                LocalAuthorityStartDate = doa,
                Staff = staffId,
                TenantID = tenantId
            };
        }


    }
    }

