using Selene.Support.Attributes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Xml.Linq;
using WebDriverRunner.internals;

namespace GetDataViaNdbUnit
{

    public class NdbUnitExampleTest
    {
        private const string DataFilePath = @"..\..\..\DataExample\GetDataViaNdbUnit\";
        private const string ConnectionString ="Data Source=lab-two.sims8.com;Initial Catalog=SIMS8_Farm1_Business_1019999;User ID=sa;Password=Pa$$w0rd";
        private const string StaffXsd = DataFilePath + "Staff.xsd";
        private const string Staff = DataFilePath + "Staff.xml";

        [UnitTest(Enabled = true, Groups = new[] {"DataExample"})]
        public void GetStaffDataUseingNdbunit()
        {
            var dbm = new DatabaseManager(StaffXsd, ConnectionString);
            
            DataSet ds = dbm.GetDataSetFromDb();
            var xDocument = XDocument.Parse(ds.GetXml());
            if (xDocument.Root != null)
            {
                XNamespace ns = xDocument.Root.Name.Namespace;
                IEnumerable<XElement> elements = xDocument.Descendants().Elements(ns + "Staff");

                foreach (XElement element in elements)
                {
                    // ReSharper disable PossibleNullReferenceException
                    Console.WriteLine("Staff ID ={0}", element.Element(ns + "ID").Value);
                    Console.WriteLine("PreferredForename ={0}", element.Element(ns + "PreferredForename").Value);
                    Console.WriteLine("PreferredSurname ={0}", element.Element(ns + "PreferredSurname").Value);
                    // ReSharper restore PossibleNullReferenceException
                }
            }
        }

        [UnitTest(Enabled = true, Groups = new[] { "DataExample" })]
        public void InsertStaffDataUseingNdbunit()
        {
            var dbm = new DatabaseManager(StaffXsd, ConnectionString);
            dbm.InsertIdentity(Staff);

        }

        [UnitTest(Enabled = true, Groups = new[] { "DataExample" })]
        public void DeleteStaffDataUseingNdbunit()
        {
            var dbm = new DatabaseManager(StaffXsd, ConnectionString);
            dbm.Delete(Staff);
        }
    }
}
     
