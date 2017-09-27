using System.Collections.Generic;
using System.Data.SqlClient;
using System.Transactions;
using Dapper;
using TestSettings;

namespace Pupil.Components.DataAccess
{
    public class DataAccessHelpers
    {
        /// <summary>
        /// Gets and opens a connection to the database specified in the settings file.
        /// </summary>
        /// <param name="transaction">The transation to use for all operations for this connection instance (optional).</param>
        public static SqlConnection GetConnection(Transaction transaction = null)
        {
            string connectionString = Configuration.GetSutDbConnStr();
            var connection = new SqlConnection(connectionString);

            connection.Open();
            if (transaction != null)
            {
                connection.EnlistTransaction(transaction);
            }

            return connection;
        }

        /// <summary>
        /// Retrieves a single value from the database specified in the settings file.
        /// </summary>
        /// <typeparam name="TProp">The type to cast the value to.</typeparam>
        /// <param name="sqlCommand">The parameterised SQL statement to get the value.</param>
        /// <param name="parameters">The parameter(s) to apply when executing the SQL command.</param>
        /// <returns></returns>
        public static TProp GetValue<TProp>(string sqlCommand, object parameters = null)
        {
            TProp data;
            using (var connection = GetConnection())
            {
                data = connection.ExecuteScalar<TProp>(sqlCommand, parameters);
            }

            return data;
        }

        /// <summary>
        /// Retrieves a collection of entities from the database specified in the settings file.
        /// </summary>
        /// <typeparam name="TEntity">The type to cast each entity to. The properties MUST match the SELECTed columns in the SQL statement.</typeparam>
        /// <param name="sqlCommand">The parameterised SQL statement to get the value. eg. SELECT LegalForename FROM dbo.Staff WHERE (Id = @Id).</param>
        /// <param name="parameters">The parameter(s) to apply when executing the SQL command. eg. new {Id = 1}</param>
        /// <returns></returns>
        public static IEnumerable<TEntity> GetEntities<TEntity>(string sqlCommand, object parameters = null)
        {
            IEnumerable<TEntity> data;
            using (var connection = GetConnection())
            {
                data = connection.Query<TEntity>(sqlCommand, parameters);
            }

            return data;
        }

        /// <summary>
        /// Executes an SQL statement.
        /// </summary>
        /// <param name="sqlCommand">The parameterised SQL statement to get the value. eg. SELECT LegalForename FROM dbo.Staff WHERE (Id = @Id).</param>
        /// <param name="parameters">The parameter(s) to apply when executing the SQL command. eg. new {Id = 1}</param>
        /// <returns>The number of affected rows.</returns>
        public static int Execute(string sqlCommand, object parameters = null)
        {
            int affectedRows;
            using (var connection = GetConnection())
            {
                affectedRows = connection.Execute(sqlCommand, parameters);
            }

            return affectedRows;
        }
    }
}
