using System;
using System.Collections.Generic;
using System.Globalization;
using System.IdentityModel.Tokens;
using Facilities.Components.FacilitiesQueries;
using GetDataViaWebServices.DataEntityIO;
using TestSettings;

namespace Facilities.Components.FacilitiesTestData
{
    public class RoomTestData
    {
        public static List<Guid> AddRoom(string roomShortName, string roomLongName)
        {
            var query = new RoomQuery();
            var sessionToken = query.Login(TestDefaults.Default.TestUser,
                TestDefaults.Default.TestUserPassword,
                TestDefaults.Default.SchoolID,
                TestDefaults.Default.TenantId.ToString(CultureInfo.InvariantCulture),
                Configuration.GetSutUrl() + TestDefaults.Default.ApplicationServerPath,
                Configuration.GetSutUrl() + TestDefaults.Default.SecurityServerPath);

            //Retrieved collection of Staff Records
            DataEntityCollectionDTO staffCollection = query.RetrieveEntityByNameQuery("SIMS8SchoolRoomSearchQuery", new Dictionary<string, object>(), sessionToken);
            return null;
        }

        public static void DeleteRoom(Guid roomId)
        {
            
        }
    }
}
