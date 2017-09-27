using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SeSugar;
using SeSugar.Data;
using TestSettings;

namespace SeSugar.Data
{
    public static class PupilLogDataPackageHelper
    {
        public static DataPackage AddStandardPupilLogNote(this DataPackage dataPackage, Guid learnerId, string title, int? tenantId = null)
        {
            tenantId = tenantId ?? Environment.Settings.TenantId;
            dataPackage.AddData("PupilLogNoteStandard", new
            {
                Id = Guid.NewGuid(),
                Learner = learnerId,
                PupilLogNoteCategory = CoreQueries.GetLookupItem("PupilLogNoteCategory", code: "GENNA"),
                Title = title,
                NoteText = Utilities.GenerateRandomString(20, "Selenium"),
                Pinned = false,
                CreatedOn = new DateTime(2015, 01, 10),
                TenantID = tenantId
            });
            return dataPackage;
        }
    }
}
