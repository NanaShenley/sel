using System;
using SeSugar.Data;
using Admissions.Data;
namespace Admissions.Component
{
    public static class AddressData
    {
        public static DataPackage AddBasicLearnerContactAddress(this DataPackage dataPackage, Guid learnerId, Guid learnerContactId, Guid addressId, Guid learnerContactAddressId, Guid learnerAddressId, DateTime startDate, int? tenantId = null, string postCode = null, string addressType = "H")
        {
            tenantId = tenantId ?? SeSugar.Environment.Settings.TenantId;
            dataPackage.AddData(Constants.Tables.Address, new
            {
                ID = addressId,
                TenantID = tenantId,
            })
            .AddData(Constants.Tables.LearnerAddress, new
            {
                ID = learnerAddressId,
                StartDate = startDate,
                Learner = learnerId,
                Address = addressId,
                AddressType = CoreQueries.GetLookupItem(Constants.Tables.AddressType, code: addressType),
                TenantID = tenantId
            })
            .AddData(Constants.Tables.LearnerContactAddress, new
            {
                ID = learnerContactAddressId,
                StartDate = startDate,
                LearnerContact = learnerContactId,
                Address = addressId,
                AddressType = CoreQueries.GetLookupItem(Constants.Tables.AddressType, code: addressType),
                TenantID = tenantId
            });
            return dataPackage;
        }
    }
}
