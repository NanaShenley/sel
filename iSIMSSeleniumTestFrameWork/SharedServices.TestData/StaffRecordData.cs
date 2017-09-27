using SeSugar.Data;
using System;

namespace SharedServices.TestData
{
    public static class StaffRecordData
    {
        public static DataPackage AddBasicStaff(this DataPackage dataPackage, Guid staffId, string forename, string surname)
        {
            return dataPackage
               .AddData("Staff", new
               {
                   Id = staffId,
                   LegalForename = forename,
                   LegalSurname = surname,
                   LegalMiddleNames = "Middle",
                   PreferredForename = forename,
                   PreferredSurname = surname,
                   DateOfBirth = new DateTime(2000, 1, 1),
                   Gender = CoreQueries.GetLookupItem("Gender", description: "Male"),
                   PolicyACLID = CoreQueries.GetPolicyAclId("Staff"),
                   School = CoreQueries.GetSchoolId(),
                   TenantID = SeSugar.Environment.Settings.TenantId,
                   Title = CoreQueries.GetLookupItem("Title", description: "Mr"),
               })
                .AddData("StaffServiceRecord", new
                {
                    Id = Guid.NewGuid(),
                    DOA = DateTime.Today.AddDays(-1),
                    ContinuousServiceStartDate = DateTime.Today.AddDays(-1),
                    LocalAuthorityStartDate = DateTime.Today.AddDays(-1),
                    Staff = staffId,
                    TenantID = SeSugar.Environment.Settings.TenantId
                }); 
        }
    }
}
