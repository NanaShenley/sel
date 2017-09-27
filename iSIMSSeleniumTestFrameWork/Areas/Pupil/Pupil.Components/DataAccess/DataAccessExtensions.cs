using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pupil.Components.DataAccess
{
    public static class DataAccessExtentions
    {
        /// <summary>
        /// Generates an INSERT SQL statement from this object.
        /// </summary>
        /// <param name="data">The object to generate the INSERT SQL statement from.</param>
        /// <param name="table">The database table to which the data will be inserted.</param>
        /// <param name="properties">The list of properties to use in the INSERT SQL statement.</param>
        /// <returns>An INSERT SQL statement with parameter placeholders eg. "INSERT INTO dbo.Table (Field) VALUES (@Field)".</returns>
        public static string GetInsertSql(string table, List<string> properties)
        {
            const string insertFormat = "INSERT INTO dbo.{0} ({1}) VALUES ({2})";
            const string attributeFormat = "{0}, ";
            const string attributeFormatLast = "{0}";
            const string parameterFormat = "@{0}, ";
            const string parameterFormatLast = "@{0}";

            var attributes = new StringBuilder();
            var parameters = new StringBuilder();

            var lastProperty = properties.Last();

            foreach (var property in properties)
            {
                attributes.AppendFormat(property != lastProperty ? attributeFormat : attributeFormatLast, property);
                parameters.AppendFormat(property != lastProperty ? parameterFormat : parameterFormatLast, property);
            }

            return string.Format(insertFormat, table, attributes, parameters);
        }

        /// <summary>
        /// Generates a DELETE SQL statement from this object.
        /// </summary>
        /// <param name="data">The object to generate the DELETE SQL statement from.</param>
        /// <param name="table">The database table from which the data will be removed.</param>
        /// <param name="properties">The list of properties to use in the DELETE SQL statement.</param>
        /// <returns>A DELETE SQL statement with parameter placeholders eg. "DELETE FROM dbo.Table WHERE (ID = @ID)".</returns>
        public static string GetDeleteSql(string table, List<string> properties)
        {
            const string id = "id";
            const string deleteFormat = @"DELETE FROM dbo.{0} WHERE ({1} = @{1})";
            var idProperty = properties.Single(x => x.Equals(id, StringComparison.OrdinalIgnoreCase));

            return string.Format(deleteFormat, table, idProperty);
        }
    }


    


    
}
