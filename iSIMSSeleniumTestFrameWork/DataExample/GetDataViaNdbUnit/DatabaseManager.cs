using System.Collections.Specialized;
using System.Data;
using NDbUnit.Core;

namespace GetDataViaNdbUnit
{
    public class DatabaseManager
    {
        public SqlUnitTest Testdb { get; set; }
        private readonly string _xmlSchema;
        private readonly string _connectionString;

        /// <summary>
        ///  DatabaseManager with custom schema and connection string
        /// </summary>
        /// <param name="xmlSchema">Schema to be used</param>
        /// <param name="connectionString">Connection stirng</param>
        public DatabaseManager(string xmlSchema, string connectionString)
        {
            Testdb = new SqlUnitTest(connectionString);
            _xmlSchema = xmlSchema;
            _connectionString = connectionString;
        }

        /// <summary>
        /// Insert data into the database using CleanInsertIdentity
        /// </summary>
        /// <param name="dataFile">XML file to be inserted</param>
        public void CleanInsertIdentity(string dataFile)
        {
            PerformDbOperation(dataFile, _xmlSchema, DbOperationFlag.CleanInsertIdentity);
        }

        /// <summary>
        /// Insert data into the database using Insert
        /// </summary>
        /// <param name="dataFile">XML file to be inserted</param>
        public void Insert(string dataFile)
        {
            PerformDbOperation(dataFile, _xmlSchema, DbOperationFlag.Insert);
        }

        /// <summary>
        /// Insert data into the database using CleanInsertIdentity
        /// </summary>
        /// <param name="dataFile">XML file to be inserted</param>
        public void InsertIdentity(string dataFile)
        {
            PerformDbOperation(dataFile, _xmlSchema, DbOperationFlag.InsertIdentity);
        }

        /// <summary>
        /// Update passed in data into the table
        /// </summary>
        /// <param name="dataFile">XML file to be inserted</param>
        public void Update(string dataFile)
        {
            PerformDbOperation(dataFile, _xmlSchema, DbOperationFlag.Update);
        }

        /// <summary>
        /// CleanDownTheDatabase using the default schema
        /// </summary>
        public void DeleteAll()
        {
            Testdb.ReadXmlSchema(_xmlSchema);
            Testdb.PerformDbOperation(DbOperationFlag.DeleteAll);
        }

        /// <summary>
        /// Delete the data from the database using delete
        /// </summary>
        /// <param name="dataFile">XML file to be deleted</param>
        public void Delete(string dataFile)
        {
            PerformDbOperation(dataFile, _xmlSchema, DbOperationFlag.Delete);
        }

        /// <summary>
        /// Does the database operation on the database server.
        /// </summary>
        /// <param name="dataFile">The Xml file that is to be used on the database.</param>
        /// <param name="schemaFile">The Xsd file that is to be used on the database.</param>
        /// <param name="dbOperationFlag">The database operation to perform on the database server</param>
        private void PerformDbOperation(string dataFile, string schemaFile, DbOperationFlag dbOperationFlag)
        {
            var sqlUnitTest = new SqlUnitTest(_connectionString);
            sqlUnitTest.ReadXmlSchema(schemaFile);
            sqlUnitTest.ReadXml(dataFile);
            sqlUnitTest.PerformDbOperation(dbOperationFlag);
        }


        /// <summary>
        /// Returns the DataBAse XML as a data set use dataset.writexml to write to a file
        /// </summary>
        /// <returns></returns>
        public DataSet GetDataSetFromDb()
        {
            var sqldb = new SqlUnitTest(_connectionString);
            sqldb.ReadXmlSchema(_xmlSchema);
            return sqldb.GetDataSetFromDb();
        }

        /// <summary>
        /// Returns a string collection of table names from the selected xmlfile.
        /// </summary>
        /// <param name="expectedDataFile">The Xml file to be used for the table names.</param>
        /// <param name="sqlDatabase">The sqlDatabase to be used.</param>
        /// <returns>A string collection of tables.</returns>
        public StringCollection GetTablesInXml(string expectedDataFile, SqlUnitTest sqlDatabase)
        {
            sqlDatabase.ReadXmlSchema(_xmlSchema);
            sqlDatabase.ReadXml(expectedDataFile);
            StringCollection tablesInXml = sqlDatabase.TablesInDataSet;
            return tablesInXml;
        }

        /// <summary>
        /// Get an Xml string of the passed in list of tables.
        /// </summary>
        /// <param name="tablesInXml">The Xml file of tables that is to be used.</param>
        /// <param name="sqlDatabase">The sqlDatabase to be used.</param>
        /// <returns>a string of the data from the tables requested.</returns>
        public string GetXmlFromDataBase(StringCollection tablesInXml, SqlUnitTest sqlDatabase)
        {
            return sqlDatabase.GetDataSetFromDb(tablesInXml).GetXml();
        }
    }
}
