using System.Collections.Specialized;
using System.Data;
using NDbUnit.Core.SqlClient;

namespace GetDataViaNdbUnit
{
    public class SqlUnitTest : SqlDbUnitTest
    {
        public SqlUnitTest(string connectionString)
            : base(connectionString)
        {
        }

        public DataSet DataSet
        {
            get { return DS; }
        }

        public StringCollection TablesInDataSet
        {
            get
            {
                var result = new StringCollection();
                foreach (DataTable table in DS.Tables)
                {
                    result.Add(table.TableName);
                }

                return result;
            }
        }
    }
}