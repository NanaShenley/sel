using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assessment.Components.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using OpenQA.Selenium.Support.UI;
using SharedComponents.Helpers;
using WebDriverRunner.webdriver;
using System.Threading;
using System.Data.SqlClient;
using TestSettings;

namespace Assessment.Components.PageObject
{
    public class TestData
    {

        //Gradeset Screen
        public const string ExpectedTotalGradeSetCount = "16";
        public static List<string> MISTGradesetResults = new List<string>
        {
            "MIST Grades"
        };

        public static List<string> AgeGradesetResults = new List<string>
        {
            "Age -99/11-99/11"
        };

        public static List<string> GradeDetails = new List<string>
        {
            "MIST Grades",
            "MIST",
            "Grade",
            "1.00",
            "999.00",
            "01/01/2012",
            "",
            "Above Average",
            "4.00",
            "Average",
            "3.00",
            "Below Average",
            "2.00",
            "Well Below Average",
            "1.00"
        };

        public static List<string> PurposeList = new List<string>
        {
            "Attainment ( ATN )",
            "Behaviour ( BHV )",
            "Class Contribution ( CONT )",
            "Effort ( EFT )",
            "Progress ( PROGRESS )",
            
        };

        public static List<string> ModeList = new List<string>
        {
          "Actual ( ACT )",
          "Target ( TGT )",
          "Prediction ( PREDICTION )",
          "Estimeated ( EST )",
          "Progress ( PRG )"
            
        };
        public static List<string> MethodList = new List<string>
        {
        
            "Teacher Assessment ( TA )",
            "Internal Task/Test ( TT )",
            "External Task/Test ( EXT )",
            "Not Applicable ( NA )"
            
        };

        //This function creates a data list based on the SQL query and the desired column name
        public static List<string> CreateDataList(string query,string tablecolumnname)
        {
            List<string> DataList = new List<string>();
            Thread.Sleep(2000);
            using (SqlConnection connection = new SqlConnection(Configuration.GetSutDbConnStr()))
            //connectionString="Data Source=lab-two.sims8.com;Initial Catalog=SIMS8_Farm1_Business_1019999;Persist Security Info=True;User ID=sa;Password=Pa$$w0rd"
            using (SqlCommand cmd = new SqlCommand(query, connection))
            {
                connection.Open();
                Thread.Sleep(8000);
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    // Check is the reader has any rows at all before starting to read.
                    if (reader.HasRows)
                    {
                        // Read advances to the next row.
                        while (reader.Read())
                        {
                            DataList.Add(reader.GetString(reader.GetOrdinal(tablecolumnname)));
                        }
                    }
                }
                connection.Close();
            }
            Thread.Sleep(2000);
            return DataList;
        }

        //This function creates a integer list based on the SQL query and the desired column name
        public static List<int> CreateIntegerList(string query, string tablecolumnname)
        {
            List<int> DataList = new List<int>();
            Thread.Sleep(2000);
            using (SqlConnection connection = new SqlConnection(Configuration.GetSutDbConnStr()))
            //connectionString="Data Source=lab-two.sims8.com;Initial Catalog=SIMS8_Farm1_Business_1019999;Persist Security Info=True;User ID=sa;Password=Pa$$w0rd"
            using (SqlCommand cmd = new SqlCommand(query, connection))
            {
                connection.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    // Check is the reader has any rows at all before starting to read.
                    if (reader.HasRows)
                    {
                        // Read advances to the next row.
                        while (reader.Read())
                        {
                            DataList.Add(reader.GetInt32(reader.GetOrdinal(tablecolumnname)));
                        }
                    }
                }
                connection.Close();
            }
            Thread.Sleep(2000);
            return DataList;
        }

        //Returns a School ID
        public static List<Guid> CreateGuidList(string query, string tablecolumnname)
        {
            List<Guid> DataList = new List<Guid>();
            Thread.Sleep(2000);
            using (SqlConnection connection = new SqlConnection(Configuration.GetSutDbConnStr()))
            //connectionString="Data Source=lab-two.sims8.com;Initial Catalog=SIMS8_Farm1_Business_1019999;Persist Security Info=True;User ID=sa;Password=Pa$$w0rd"
            using (SqlCommand cmd = new SqlCommand(query, connection))
            {
                connection.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    // Check is the reader has any rows at all before starting to read.
                    if (reader.HasRows)
                    {
                        // Read advances to the next row.
                        while (reader.Read())
                        {
                            DataList.Add(reader.GetGuid(reader.GetOrdinal(tablecolumnname)));
                        }
                    }
                }
                connection.Close();
            }
            return DataList;
        }


        //Returns School ID
        public static Guid GetSchoolID()
        {
            List<Guid> SchoolIID = new List<Guid>();
            SchoolIID = TestData.CreateGuidList("Select ID from ResourceProvider Where Code ='"+ TestDefaults.Default.TenantId +"'", "ID");
            return SchoolIID[0];
        }

        //Returns School ID
        public static Guid GetCAPITASIMSID()
        {
            List<Guid> CAPITASIMSID = new List<Guid>();
            CAPITASIMSID = TestData.CreateGuidList("Select ID From ResourceProvider Where Name Like '%CAPITA SIMS%'", "ID");
            return CAPITASIMSID[0];
        }

        public static Guid GetCAPITASIMSIDByTenantId()
        {
            List<Guid> CAPITASIMSID = new List<Guid>();
            CAPITASIMSID = TestData.CreateGuidList("Select ID From ResourceProvider Where Name Like '%CAPITA SIMS%' and TenantId ='" + TestDefaults.Default.TenantId + "'", "ID");
            return CAPITASIMSID[0];
        }

        //Returns DENI ID
        public static Guid GetDENIID()
        {
            List<Guid> DENIID = new List<Guid>();
            DENIID = TestData.CreateGuidList("Select ID from ResourceProvider Where Code ='DENI'", "ID");
            return DENIID[0];
        }

    }
}
