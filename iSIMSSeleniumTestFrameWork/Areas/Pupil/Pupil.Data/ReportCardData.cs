using System;
using SeSugar.Data;

namespace Pupil.Data
{
    public static class ReportCardData
    {
        public static DataPackage AddReportCard(this DataPackage dataPackage, Guid learnerId, string reportName, DateTime startDate, DateTime endDate, string reason=null,string comments=null, int? tenantId = null, Guid? reportCardId= null)
        {
            tenantId = tenantId ?? SeSugar.Environment.Settings.TenantId;
            dataPackage.AddData("ConductReportCard", new
            {
                Id = reportCardId?? Guid.NewGuid(),
                Name = reportName,
                StartDate = startDate,
                EndDate = endDate,
                Reason = reason ?? "Test Reason for Report Card Creation",
                comments = comments ?? "Test Comment for Report Card Events",
                Learner = learnerId,
                TenantID = tenantId
            });
            return dataPackage;
        }

        public static DataPackage AddReportCardTarget(this DataPackage dataPackage, Guid reportCardId , int? tenantId = null)
        {
            tenantId = tenantId ?? SeSugar.Environment.Settings.TenantId;
            dataPackage.AddData("ConductReportCardTarget", new
            {
                Id = Guid.NewGuid(),
                TenantID = tenantId,
                ReportCard = reportCardId,

            });
            return dataPackage;
        }
    }
}
