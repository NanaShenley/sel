using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SeSugar.Data;
using SeSugar.Interfaces;
using System.Collections.Concurrent;

namespace Staff.POM.Helper
{
    /// <summary>
    /// Data access helper class
    /// </summary>
    public static class DataAccessor
    {
        private static object _locker = new object();
        private const string DateKeyFormat = "{0}.{1}_{2}";
        private static ConcurrentBag<string> GeneratedDates = new ConcurrentBag<string>();

        /// <summary>
        /// Returns an enumerable list of lookup items for the lookedup entity. Tenant aware.
        /// </summary>
        /// <param name="entityName">The lookup entity.</param>
        /// <returns>Enumerable list of lookup items consisting of ID, Code and Description ordered by the DisplayOrder column.</returns>
        public static IEnumerable<LookupItem> GetLookupItems(string entityName)
        {
            int tenantId = SeSugar.Environment.Settings.TenantId;
            const string sqlDefault = "SELECT ID, Code, Description FROM dbo.{0} WHERE TenantID = @TenantId ORDER BY DisplayOrder";
            return DataAccessHelpers.GetEntities<LookupItem>(string.Format(sqlDefault, entityName), new { TenantId = tenantId });
        }

        public static DateTime GetClosestAvailableDateBefore(DateTime date, string table, string column)
        {
            return GetClosestAvailableDate(date, table, column, true);
        }

        public static DateTime GetClosestAvailableDateAfter(DateTime date, string table, string column)
        {
            return GetClosestAvailableDate(date, table, column, false);
        }

        private static DateTime GetClosestAvailableDate(DateTime date, string table, string column, bool before)
        {
            lock (_locker)
            {
                int attempts = 0;
                int maxAttempts = 100;
                DateTime previousDate = date;
                DateTime resultantDate = date;

                while ((CoreQueries.ValueExistsInColumn(resultantDate, table, column) || GeneratedDates.Contains(string.Format(DateKeyFormat, table, column, resultantDate.ToShortDateString()))) && attempts < maxAttempts)
                {
                    attempts++;
                    previousDate = resultantDate;

                    if (before)
                        resultantDate = date.AddDays(-attempts);
                    else
                        resultantDate = date.AddDays(attempts);

                    SeSugar.Environment.Logger.LogLine("The date {0} is not unique for {1}.{2}, checking {3}.", previousDate.ToShortDateString(), table, column, resultantDate.ToShortDateString());
                }

                if (attempts < maxAttempts)
                {
                    SeSugar.Environment.Logger.LogLine("The date {0} is unique for {1}.{2}.", resultantDate.ToShortDateString(), table, column);
                    GeneratedDates.Add(string.Format(DateKeyFormat, table, column, resultantDate.ToShortDateString()));
                    return resultantDate;
                }
                else
                    throw new Exception(string.Format("Failed getting the closest available date on or {4} {0} for {1}.{2} after {3} attempts.", date.ToShortDateString(), table, column, maxAttempts, before ? "before" : "after"));
            }
        }

    }

    /// <summary>
    /// Class containing a lookup data item
    /// </summary>
    public class LookupItem
    {
        /// <summary>
        /// ID of the lookup item
        /// </summary>
        public Guid ID { get; set; }

        /// <summary>
        /// The code of the lookup item
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Description of the lookup item
        /// </summary>
        public string Description { get; set; }
    }
}
