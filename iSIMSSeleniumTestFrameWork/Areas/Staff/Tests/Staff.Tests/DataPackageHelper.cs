using SeSugar;
using SeSugar.Data;
using System;

namespace Staff.Tests
{
    public static class DataPackageHelper
    {
        private static readonly int tenantID = SeSugar.Environment.Settings.TenantId;

        public static object GenerateServiceTerm(out Guid id, string code = null, string description = null, decimal? weeksWorkedPerYear = 52.00000m, decimal? hoursWorkedPerWeek = 30.0000m)
        {
            return new
            {
                ID = id = Guid.NewGuid(),
                Code = !String.IsNullOrEmpty(code) ? code : Utilities.GenerateRandomString(2),
                Description = !String.IsNullOrEmpty(description) ? description : Utilities.GenerateRandomString(20),
                IsVisible = true,
                IncrementMonth = 1,
                IncrementDay = 1,
                Salaried = true,
                SpinalProgression = true,
                MonthlyReconciliation = false,
                WeeksWorkedPerYear = weeksWorkedPerYear,
                HoursWorkedPerWeek = hoursWorkedPerWeek,
                TermTimeOnly = false,
                ResourceProvider = CoreQueries.GetSchoolId(),
                TenantID = tenantID
            };
        }

        public static object GeneratePostType(out Guid id, Guid? category = null, string code = null, string description = null, Guid? statutoryPostTypeId = null)
        {
            return new
            {
                ID = id = Guid.NewGuid(),
                Code = !string.IsNullOrEmpty(code) ? code : Utilities.GenerateRandomString(4),
                Description = !string.IsNullOrEmpty(description) ? description : Utilities.GenerateRandomString(20),
                IsVisible = true,
                StatutoryPostType = statutoryPostTypeId.HasValue ? statutoryPostTypeId : CoreQueries.GetLookupItem("StatutoryPostType"),
                ResourceProvider = CoreQueries.GetSchoolId(),
                TenantID = tenantID
            };
        }

        public static object GenerateStatutoryPostType(out Guid id, string code = null, string description = null)
        {
            return new
            {
                ID = id = Guid.NewGuid(),
                Code = !String.IsNullOrEmpty(code) ? code : CoreQueries.GetColumnUniqueString("StatutoryPostType", "Code", 4),
                Description = !String.IsNullOrEmpty(description) ? description : CoreQueries.GetColumnUniqueString("StatutoryPostType", "Description", 20),
                IsVisible = true,
                ResourceProvider = CoreQueries.GetSchoolId(),
                TenantID = tenantID
            };
        }

        public static object GenerateServiceTermPostType(out Guid Id, Guid postTypeId, Guid serviceTermId)
        {
            return new
            {
                ID = Id = Guid.NewGuid(),
                PostType = postTypeId,
                ServiceTerm = serviceTermId,
                TenantID = tenantID
            };
        }

        public static object GeneratePaySpine(out Guid id, decimal minimumPoint, decimal maximumPoint, decimal interval, string code = null)
        {
            return new
            {
                ID = id = Guid.NewGuid(),
                Code = !String.IsNullOrEmpty(code) ? code : Utilities.GenerateRandomString(4),
                MinimumPoint = minimumPoint,
                MaximumPoint = maximumPoint,
                Interval = interval,
                ResourceProvider = CoreQueries.GetSchoolId(),
                TenantID = tenantID
            };
        }

        public static object GeneratePayAward(out Guid id, Guid paySpineId, decimal scalePoint, decimal scaleAmount, DateTime? date = null)
        {
            return new
            {
                ID = id = Guid.NewGuid(),
                PaySpine = paySpineId,
                ScalePoint = scalePoint,
                ScaleAmount = scaleAmount,
                Date = date.HasValue ? date : DateTime.Today,
                TenantID = tenantID
            };
        }

        public static object GenerateStatutoryPayScale(out Guid id)
        {
            return new
            {
                ID = id = Guid.NewGuid(),
                Code = Utilities.GenerateRandomString(4),
                Description = Utilities.GenerateRandomString(5, "Test Statutory Pay Scale"),
                DisplayOrder = 1,
                IsVisible = true,
                ResourceProvider = CoreQueries.GetSchoolId(),
                TenantID = tenantID
            };
        }

        public static object GenerateAdditionalPaymentCategory(out Guid id, string code = null, string description = null)
        {
            return new
            {
                ID = id = Guid.NewGuid(),
                Code = !String.IsNullOrEmpty(code) ? code : Utilities.GenerateRandomString(4),
                Description = !String.IsNullOrEmpty(description) ? description : Utilities.GenerateRandomString(5, "Test AP Category"),
                DisplayOrder = 1,
                IsVisible = true,
                ResourceProvider = CoreQueries.GetSchoolId(),
                TenantID = tenantID
            };
        }

        public static object GenerateSuperannuationScheme(out Guid id, string code = null, string description = null)
        {
            return new
            {
                ID = id = Guid.NewGuid(),
                Code = !String.IsNullOrEmpty(code) ? code : Utilities.GenerateRandomString(4),
                Description = !String.IsNullOrEmpty(description) ? description : Utilities.GenerateRandomString(10),
                ResourceProvider = CoreQueries.GetSchoolId(),
                TenantID = SeSugar.Environment.Settings.TenantId
            };
        }

        public static object GenerateSuperannuationSchemeDetail(out Guid id, Guid superannuationSchemeId, DateTime applicationDate, decimal value)
        {
            return new
            {
                ID = id = Guid.NewGuid(),
                ApplicationDate = applicationDate,
                Value = value,
                SuperannuationScheme = superannuationSchemeId,
                TenantID = SeSugar.Environment.Settings.TenantId
            };
        }

        public static object GenerateServiceTermSuperannuationScheme(out Guid Id, Guid superannuationSchemeId, Guid serviceTermId)
        {
            return new
            {
                ID = Id = Guid.NewGuid(),
                SuperannuationScheme = superannuationSchemeId,
                ServiceTerm = serviceTermId,
                TenantID = SeSugar.Environment.Settings.TenantId
            };
        }

        public static object GenerateFinancialSubGroup(out Guid id, Guid serviceTermId, string code = null, string description = null)
        {
            return new
            {
                ID = id = Guid.NewGuid(),
                ServiceTerm = serviceTermId,
                Code = !String.IsNullOrEmpty(code) ? code : Utilities.GenerateRandomString(4),
                Description = !String.IsNullOrEmpty(description) ? description : Utilities.GenerateRandomString(20),
                TenantID = SeSugar.Environment.Settings.TenantId
            };
        }

        public static object GenerateAllowance(out Guid id, Guid? category = null, string code = null, string description = null)
        {
            return new
            {
                ID = id = Guid.NewGuid(),
                Code = !String.IsNullOrEmpty(code) ? code : Utilities.GenerateRandomString(4),
                Description = !String.IsNullOrEmpty(description) ? description : Utilities.GenerateRandomString(5, "Test Allowance"),
                DisplayOrder = 1,
                IsVisible = true,
                AllowanceAwardAttached = 0,
                AdditionalPaymentCategory = category.HasValue ? category : CoreQueries.GetLookupItem("AdditionalPaymentCategory", "OTH"),
                ResourceProvider = CoreQueries.GetSchoolId(),
                TenantID = tenantID
            };
        }

        public static object GenerateFixedAllowance(out Guid id, Guid? category = null, string code = null, string description = null)
        {
            return new
            {
                ID = id = Guid.NewGuid(),
                Code = !String.IsNullOrEmpty(code) ? code : Utilities.GenerateRandomString(4),
                Description = !String.IsNullOrEmpty(description) ? description : Utilities.GenerateRandomString(5, "Test Allowance"),
                DisplayOrder = 1,
                IsVisible = true,
                AllowanceAwardAttached = 1,
                AdditionalPaymentCategory = category.HasValue ? category : CoreQueries.GetLookupItem("AdditionalPaymentCategory", "OTH"),
                ResourceProvider = CoreQueries.GetSchoolId(),
                TenantID = tenantID
            };
        }

        public static object GenerateAllowanceAward(out Guid id, Guid allowanceId)
        {
            return new
            {
                ID = id = Guid.NewGuid(),
                Amount = 99.99m,
                AwardDate = DateTime.Today,
                Allowance = allowanceId,
                TenantID = tenantID
            };
        }

        public static object GenerateServiceTermAllowance(out Guid Id, Guid allowanceId, Guid serviceTermId)
        {
            return new
            {
                ID = Id = Guid.NewGuid(),
                Allowance = allowanceId,
                ServiceTerm = serviceTermId,
                TenantID = tenantID
            };
        }

        public static object GeneratePayScale(out Guid Id, Guid serviceTermId, Guid paySpineId, Guid? statutoryPayScaleId = null, string description = null)
        {
            return new
            {
                ID = Id = Guid.NewGuid(),
                Code = Utilities.GenerateRandomString(3),
                Description = !String.IsNullOrEmpty(description) ? description : Utilities.GenerateRandomString(20),
                IsVisible = true,
                MinimumPoint = 1.0m,
                MaximumPoint = 2.0m,
                ServiceTerm = serviceTermId,
                PaySpine = paySpineId,
                StatutoryPayScale = statutoryPayScaleId.HasValue ? statutoryPayScaleId : CoreQueries.GetLookupItem("StatutoryPayScale", code: "OT"),
                ResourceProvider = CoreQueries.GetSchoolId(),
                TenantID = tenantID
            };
        }

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
                TenantID = tenantID
            };
        }

        public static object GernateStaffReasonForLeaving(out Guid id, string code, string description, bool isVisible = true, int displayOrder = 1)
        {
            return new
            {
                ID = id = Guid.NewGuid(),
                Code = !String.IsNullOrEmpty(code) ? code : Utilities.GenerateRandomString(4),
                Description = !String.IsNullOrEmpty(description) ? description : Utilities.GenerateRandomString(20),
                DisplayOrder = displayOrder,
                IsVisible = isVisible,
                ResourceProvider = CoreQueries.GetSchoolId(),
                TenantID = tenantID
            };
        }

        public static object GeneratePayLevel(out Guid id, string code, string description, bool isVisible = true, int displayOrder = 1)
        {
            return new
            {
                ID = id = Guid.NewGuid(),
                TenantID = tenantID,
                Code = code,
                Description = description,
                IsVisible = isVisible,
                DisplayOrder = displayOrder,
                ResourceProvider = CoreQueries.GetSchoolId()
            };
        }

        public static object GenerateRegionalWeighting(out Guid id, string code, string description, bool isVisible = true, int displayOrder = 1)
        {
            return new
            {
                ID = id = Guid.NewGuid(),
                TenantID = tenantID,
                Code = code,
                Description = description,
                IsVisible = isVisible,
                DisplayOrder = displayOrder,
                ResourceProvider = CoreQueries.GetSchoolId()
            };
        }

        public static object GenerateSalaryRange(out Guid id, string code, string description, Guid payLevel, Guid regionalWeighting, bool isVisible = true, int displayOrder = 1)
        {
            return new
            {
                ID = id = Guid.NewGuid(),
                TenantID = tenantID,
                Code = code,
                Description = description,
                IsVisible = isVisible,
                PayLevel = payLevel,
                RegionalWeighting = regionalWeighting,
                ResourceProvider = CoreQueries.GetSchoolId()
            };
        }

        public static object GenerateSalaryAward(out Guid id, DateTime awardDate, decimal maximumAmount, decimal minimumAmount, Guid salaryRange)
        {
            return new
            {
                ID = id = Guid.NewGuid(),
                TenantID = tenantID,
                AwardDate = awardDate,
                MaximumAmount = maximumAmount,
                MinimumAmount = minimumAmount,
                SalaryRange = salaryRange
            };
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
                TenantID = tenantID
            };
        }

        public static object GenerateServiceAgreementReason(out Guid id, string code, string description, bool isVisible = true, int displayOrder = 1)
        {
            return new
            {
                ID = id = Guid.NewGuid(),
                TenantID = tenantID,
                Code = code,
                Description = description,
                IsVisible = isVisible,
                DisplayOrder = displayOrder, 
                ResourceProvider = CoreQueries.GetSchoolId()
            };
        }


        public static object GenerateServiceAgreement(out Guid id, Guid staffId, Guid? postTypeId = null)
        {
            return new
            {
                ID = id = Guid.NewGuid(),
                ServiceAgreementReason = CoreQueries.GetLookupItem("ServiceAgreementReason", code: "GOVN"),
                PostType = postTypeId,
                Staff = staffId,
                Notes = "Notes",
                WeeksPerYear = 48,
                FTEHoursPerWeek = 48,
                AgreementHoursPerWeek = 1,
                DailyRate = 1,
                SourceName = "X4group_03_03_16",
                AcceptedDate = DateTime.Now.AddDays(-1),
                OfferedDate = DateTime.Now.AddDays(-2),
                EndDate = DateTime.Now.AddDays(30),
                StartDate = DateTime.Now,
                ServiceAgreementSource = CoreQueries.GetLookupItem("ServiceAgreementSource", code: "LA"),
                ServiceAgreementType = CoreQueries.GetLookupItem("ServiceAgreementType", code: "SLA"),
                TenantID = tenantID
            };
        }

        public static object GenerateServiceAgreementRole(out Guid id, Guid serviceAgreement, DateTime startDate, DateTime endDate)
        {
            return new
            {
                ID = id = Guid.NewGuid(),
                TenantID = tenantID,
                StartDate = startDate,
                EndDate = endDate,
                StaffRole = CoreQueries.GetLookupItem("StaffRole", "COOK"),
                ServiceAgreement = serviceAgreement
            };
        }
        
        public static object GenerateEmployee(out Guid id)
        {
            return new
            {
                ID = id = Guid.NewGuid(),
                TenantID = tenantID
            };
        }

        public static object GenerateEmploymentContract(out Guid id, Guid serviceTermId, Guid employeeID, DateTime startDate, DateTime? endDate = null, string employmentTypeCode = "PRM", string statutoryOriginCode = "OTHERR", string statutoryDestinationCode = "OTHERR", Guid? postTypeId = null)
        {
            return new
            {
                ID = id = Guid.NewGuid(),
                ServiceTerm = serviceTermId,
                Employee = employeeID,
                PostType = postTypeId,
                EmploymentType = CoreQueries.GetLookupItem("EmploymentType", employmentTypeCode),
                ServiceTermWeeksPerYear = 52.0000m,
                WeeksPerYear = 52.0000m,
                ServiceTermHoursPerWeek = 30.0000m,
                HoursPerWeek = 30.0000m,
                FTE = 1.0000m,
                ProRata = 1.0000m,
                AnnualLeaveEntitlementDays = 0.0000m,
                StartDate = startDate,
                EndDate = endDate,
                IncrementDay = 1,
                IncrementMonth = 4,
                TenantID = tenantID,
                EmploymentContractOrigin = CoreQueries.GetLookupItem("EmploymentContractOrigin", statutoryOriginCode),
                EmploymentContractDestination = CoreQueries.GetLookupItem("EmploymentContractDestination", statutoryDestinationCode)
            };
        }

        public static object GenerateEmploymentContractSalaryRange
            (
            out Guid id,
            Guid employmentContractID,
            Guid salaryRangeID,
            decimal annualSalary,
            DateTime? startDate,
            DateTime? endDate = null,
            bool superannuation = false,
            bool nationalInsuranceStatus = false,
            string notes = ""
            )
        {
            return new
            {
                ID = id = Guid.NewGuid(),
                TenantID = tenantID,
                StartDate = startDate,
                EndDate = endDate,
                AnnualSalary = annualSalary,
                Superannuation = superannuation,
                NationalInsuranceStatus = nationalInsuranceStatus,
                Notes = notes,
                EmploymentContract = employmentContractID,
                SalaryRange = salaryRangeID
            };
        }

        public static object GenerateEmploymentContractPayScale(out Guid id, Guid payScaleID, Guid employmentContractID, DateTime startDate, DateTime? endDate = null, decimal point = 1m)
        {
            return new
            {
                ID = id = Guid.NewGuid(),
                PayScale = payScaleID,
                EmploymentContract = employmentContractID,
                StartDate = startDate,
                EndDate = endDate,
                Point = point,
                TenantID = tenantID
            };
        }

        public static object GenerateEmploymentContractStaffRole(out Guid id, Guid employmentContractID, DateTime startDate, DateTime? endDate = null, string roleCode = "OCSU")
        {
            return new
            {
                ID = id = Guid.NewGuid(),
                StaffRole = CoreQueries.GetLookupItem("StaffRole", roleCode),
                EmploymentContract = employmentContractID,
                StartDate = startDate,
                EndDate = endDate,
                TenantID = tenantID
            };
        }

        public static object GenerateEmploymentContractAllowance(out Guid id, Guid allowanceID, Guid employmentContractID, DateTime startDate, DateTime? endDate = null)
        {
            return new
            {
                ID = id = Guid.NewGuid(),
                Allowance = allowanceID,
                EmploymentContract = employmentContractID,
                StartDate = startDate,
                EndDate = endDate,
                PayFactor = 1.0000m,
                Amount = 10.00m,
                AllowanceType = CoreQueries.GetLookupItem("AllowanceType", "P"),
                TenantID = tenantID
            };
        }

        public static object GenerateServiceTermSalaryRange(out Guid id, Guid serviceTermId, Guid salaryRangeId)
        {
            return new {
                ID = id = Guid.NewGuid(),
                ServiceTerm = serviceTermId,
                SalaryRange = salaryRangeId,
                TenantID = tenantID
            };
        }

        public static object GenerateLocationType(out Guid id, string code, string description, bool isVisible = true, int displayOrder = 1)
        {
            return new
            {
                ID = id = Guid.NewGuid(),
                TenantID = tenantID,
                Code = code,
                Description = description,
                IsVisible = isVisible,
                DisplayOrder = displayOrder,
                ResourceProvider = CoreQueries.GetSchoolId()
            };
        }

        public static object GenerateEmailLocationType(out Guid id, string code, string description, bool isVisible = true, int displayOrder = 1)
        {
            return new
            {
                ID = id = Guid.NewGuid(),
                TenantID = tenantID,
                Code = code,
                Description = description,
                IsVisible = isVisible,
                DisplayOrder = displayOrder,
                ResourceProvider = CoreQueries.GetSchoolId()
            };
        }

        public static object GeneratePupil(out Guid id, string forename, string surname, DateTime dateOfBirth, string genderCode = "1")
        {
            return new
            {
                ID = id = Guid.NewGuid(),
                School = CoreQueries.GetSchoolId(),
                Gender = CoreQueries.GetLookupItem("Gender", code: genderCode),
                LegalForename = forename,
                LegalSurname = surname,
                DateOfBirth = dateOfBirth,
                TenantID = tenantID
            };
        }

        public static object GeneratePupilEnrolment(out Guid id, Guid learnerID, DateTime doa)
        {
            return new
            {
                ID = id = Guid.NewGuid(),
                School = CoreQueries.GetSchoolId(),
                Learner = learnerID,
                DOA = doa,
                TenantID = tenantID
            };
        }

        public static object GeneratePupilEnrolmentStatus(out Guid id, Guid learnerEnrolmentID, DateTime doa)
        {
            return new
            {
                ID = id = Guid.NewGuid(),
                LearnerEnrolment = learnerEnrolmentID,
                StartDate = doa,
                EnrolmentStatus = CoreQueries.GetLookupItem("EnrolmentStatus", code: "C"),
                TenantID = tenantID
            };
        }

        public static object GeneratePupilTelephone(out Guid id, string telephoneNumber, Guid pupilID, Guid locationTypeID)
        {
            return new
            {
                ID = id = Guid.NewGuid(),
                TelephoneNumber = telephoneNumber,
                IsFirstPointOfContact = 1,
                LocationType = locationTypeID,
                Learner = pupilID,
                TenantID = tenantID
            };
        }

        public static object GeneratePupilEmail(out Guid id, string emailAddress, Guid pupilID, Guid emailLocationTypeID)
        {
            return new
            {
                ID = id = Guid.NewGuid(),
                EmailAddress = emailAddress,
                IsMainEmail = 1,
                LocationType = emailLocationTypeID,
                Learner = pupilID,
                TenantID = tenantID
            };
        }

        public static object GeneratePupilContact(out Guid id, string surname)
        {
            return new
            {
                ID = id = Guid.NewGuid(),
                Surname = surname,
                School = CoreQueries.GetSchoolId(),
                TenantID = tenantID
            };
        }

        public static object GeneratePupilContactTelephone(out Guid id, string telephoneNumber, Guid pupilContactID, Guid locationTypeID)
        {
            return new
            {
                ID = id = Guid.NewGuid(),
                TelephoneNumber = telephoneNumber,
                IsFirstPointOfContact = 1,
                LocationType = locationTypeID,
                LearnerContact = pupilContactID,
                TenantID = tenantID
            };
        }

        public static object GeneratePupilContactEmail(out Guid id, string emailAddress, Guid pupilContactID, Guid emailLocationTypeID)
        {
            return new
            {
                ID = id = Guid.NewGuid(),
                EmailAddress = emailAddress,
                IsMainEmail = 1,
                LocationType = emailLocationTypeID,
                LearnerContact = pupilContactID,
                TenantID = tenantID
            };
        }

        public static object GenerateStaffContact(out Guid id, string surname)
        {
            return new
            {
                ID = id = Guid.NewGuid(),
                Surname = surname,
                School = CoreQueries.GetSchoolId(),
                TenantID = tenantID
            };
        }

        public static object GenerateStaffContactRelationship(out Guid id, Guid staffID, Guid staffContactID)
        {
            return new
            {
                ID = id = Guid.NewGuid(),
                Staff = staffID,
                StaffContact = staffContactID,
                TenantID = tenantID
            };
        }

        public static object GeneratePupilContactRelationship(out Guid id, Guid pupilID, Guid pupilContactID)
        {
            return new
            {
                ID = id = Guid.NewGuid(),
                Learner = pupilID,
                LearnerContact = pupilContactID,
                TenantID = tenantID
            };
        }

        public static object GenerateStaffContactTelephone(out Guid id, string telephoneNumber, Guid staffContactID, Guid locationTypeID)
        {
            return new
            {
                ID = id = Guid.NewGuid(),
                TelephoneNumber = telephoneNumber,
                IsFirstPointOfContact = 1,
                LocationType = locationTypeID,
                StaffContact = staffContactID,
                TenantID = tenantID
            };
        }

        public static object GenerateStaffContactEmail(out Guid id, string emailAddress, Guid staffContactID, Guid emailLocationTypeID)
        {
            return new
            {
                ID = id = Guid.NewGuid(),
                EmailAddress = emailAddress,
                IsMainEmail = 1,
                LocationType = emailLocationTypeID,
                StaffContact = staffContactID,
                TenantID = tenantID
            };
        }

        public static object GenerateStaffTelephone(out Guid id, string telephoneNumber, Guid staffID, Guid locationTypeID)
        {
            return new {
                ID = id = Guid.NewGuid(),
                TelephoneNumber = telephoneNumber,
                IsMainTelephone = 1,
                LocationType = locationTypeID,
                Staff = staffID,
                TenantID = tenantID
            };
        }

        public static object GenerateStaffEmail(out Guid id, string emailAddress, Guid staffID, Guid emailLocationTypeID)
        {
            return new
            {
                ID = id = Guid.NewGuid(),
                EmailAddress = emailAddress,
                IsMainEmail = 1,
                LocationType = emailLocationTypeID,
                Staff = staffID,
                TenantID = tenantID
            };
        }
    }
}
