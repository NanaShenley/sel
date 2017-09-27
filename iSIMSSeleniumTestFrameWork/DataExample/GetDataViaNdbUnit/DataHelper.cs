using System.Collections.Generic;
using System.Collections.Specialized;
using System.Xml.Linq;
using NUnit.Framework;

namespace GetDataViaNdbUnit
{
    public class DataHelper
    {
        /// <summary>
        /// Compare data from Database With data in an XML file
        /// </summary>
        /// <param name="dbm">The Database Manager to Use</param>
        /// <param name="expectedDataFile">XML DataFile</param>
        /// <param name="excludedTagsList">List of tags to remove</param>
        public void CompareData(DatabaseManager dbm, string expectedDataFile, List<string> excludedTagsList)
        {
            StringCollection tablesInXml = dbm.GetTablesInXml(expectedDataFile, dbm.Testdb);
            KeyValuePair<string, string> actualAndExpected = GetActualAndExpected(dbm, excludedTagsList, tablesInXml);
            Assert.That(actualAndExpected.Key, Is.EqualTo(actualAndExpected.Value));
        }

        /// <summary>
        /// Compare data from Database With data in an XML file
        /// </summary>
        /// <param name="dbm">The Database Manager to Use</param>
        /// <param name="expectedDataFile">XML DataFile</param>
        public void CompareData(DatabaseManager dbm, string expectedDataFile)
        {
            StringCollection tablesInXml = dbm.GetTablesInXml(expectedDataFile, dbm.Testdb);
            KeyValuePair<string, string> actualAndExpected = GetActualAndExpected(dbm, tablesInXml);
            Assert.That(actualAndExpected.Key, Is.EqualTo(actualAndExpected.Value));
        }


        /// <summary>
        /// Returns a set of key value pairs of the given Xml file.
        /// </summary>
        /// <param name="dbm">Database manager / connection that will be used.</param>
        /// <param name="tablesInXml">Tables that are in the Xml file.</param>
        /// <returns></returns>
        private KeyValuePair<string, string> GetActualAndExpected(DatabaseManager dbm, StringCollection tablesInXml)
        {
            string expectedXml = dbm.Testdb.DataSet.GetXml();
            string actualXml = dbm.GetXmlFromDataBase(tablesInXml, dbm.Testdb);
            return new KeyValuePair<string, string>(actualXml,expectedXml);
        }

        /// <summary>
        /// Returns a set of key value pairs of the given Xml file.
        /// </summary>
        /// <param name="dbm">Database manager / connection that will be used.</param>
        /// <param name="excludedTagsList">List of tags that are to be used in the Xml for find</param>
        /// <param name="tablesInXml">Tables that are in the Xml file.</param>
        /// <returns></returns>
        private KeyValuePair<string, string> GetActualAndExpected(DatabaseManager dbm, List<string> excludedTagsList, StringCollection tablesInXml)
        {
            string expectedXml = dbm.Testdb.DataSet.GetXml();
            string actualXml = dbm.GetXmlFromDataBase(tablesInXml, dbm.Testdb);
            return new KeyValuePair<string, string>(RemoveTagsFromXml(actualXml, tablesInXml, excludedTagsList),
                                                    RemoveTagsFromXml(expectedXml, tablesInXml, excludedTagsList));
        }


        /// <summary>
        /// Remove the Xml Tags from the Xml file that are in the list collection.
        /// </summary>
        /// <param name="expectedXml">Xml file that will be amended.</param>
        /// <param name="tablesInXml">Tables that are in the Xml file that may need amending.</param>
        /// <param name="excludedTagsList">List of tags that are to be removed from the Xml file.</param>
        /// <returns>A string of the Xml data.</returns>
        private string RemoveTagsFromXml(string expectedXml, StringCollection tablesInXml, List<string> excludedTagsList)
        {
            var xDocument = XDocument.Parse(expectedXml);
            if (xDocument.Root != null)
            {
                XNamespace ns = xDocument.Root.Name.Namespace;
                foreach (var table in tablesInXml)
                {
                    foreach (var excludedTag in excludedTagsList)
                    {
                        if (xDocument.Descendants().Elements(ns + table).Elements(ns + excludedTag) != null)
                        {
                            xDocument.Descendants().Elements(ns + table).Elements(ns + excludedTag).Remove();
                        }
                    }
                }
            }
            return xDocument.ToString();
        }
    }
}