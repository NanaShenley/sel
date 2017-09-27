using System;
using Admissions.Data.Entities;
using SeSugar.Data;
using TestSettings;
using Environment = SeSugar.Environment;

namespace Admissions.Data
{
    public static class SchoolIntakeData
    {
        public static DataPackage AddSchoolIntake(
            this DataPackage dataPackage, 
            Guid schoolIntakeId, 
            string admissionYear, 
            string admissionTerm,
            YearGroup yearGroup,
            int numberOfPlannedAdmission, 
            string admissionGroupName,
            DateTime dateOfAdmission,
            int capacity,
            Guid? admissionGroupId = null,
            int? tenantId = null,
            string schoolInTakeName = null)
        {
            tenantId = tenantId ?? Environment.Settings.TenantId;
            dataPackage.AddData("SchoolIntake", new
            {
                Id = schoolIntakeId,
                School = CoreQueries.GetSchoolId(),
                Name = schoolInTakeName ?? admissionYear + " - " + admissionTerm + " " + yearGroup.FullName,
                YearOfAdmission = Queries.GetAdmissionYear(admissionYear).ID,
                AdmissionTerm = CoreQueries.GetLookupItem("AdmissionTerm", code: admissionTerm),
                YearGroup = yearGroup.ID,
                PlannedAdmissionNumber = numberOfPlannedAdmission,
                IsActive = true,
                TenantID = tenantId
            })
            .AddData("AdmissionGroup", new
            {
                Id = admissionGroupId ?? Guid.NewGuid(),
                Name = admissionGroupName,
                SchoolIntake = schoolIntakeId,
                DateOfAdmission = dateOfAdmission,
                Capacity = capacity,
                IsActive = true,
                TenantID = tenantId
            });
                
            return dataPackage;
        }
    }
}
