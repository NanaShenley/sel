using System;
using System.Collections.Generic;
using TestSettings;
using NUnit.Framework;
using Selene.Support.Attributes;
using SeSugar.Automation;
using POM.Helper;
using Facilities.POM.Components.SchoolGroups.Page;


namespace CurriculumSchemeDetailsTest
{
    class BandSchemeDetailTest
    {
        #region Story ID : 11693 :- Creating the Curriculum Scheme Details for the current Academic Year.
        [WebDriverTest(TimeoutSeconds = 1200, Enabled = false, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { "P2" }, DataProvider = "TC_CS01_Data")]
        public void CreateScheme(string[] basicDetails, string[] structuralGroup)
        {
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator, "Curriculum Structure");
            AutomationSugar.NavigateMenu("Tasks", "School Groups", "Curriculum Structure");
            Wait.WaitForDocumentReady();

            Random rnd = new Random();
            int displayOrder = rnd.Next(1, 50);

            var curriculumStructureTriplet = new CurriculumStructureTriplet();
            curriculumStructureTriplet.Create();
            var bandSchemePage = curriculumStructureTriplet.CreateBandSchemePage();
            bandSchemePage.BandSchemeName = basicDetails[0];
            bandSchemePage.AcademicYearDropDown = basicDetails[1];
            var structuralGroupDialog = bandSchemePage.ClickAddTeachingGroup();
            structuralGroupDialog.ShortName = structuralGroup[0];
            structuralGroupDialog.DisplayOrder = displayOrder;
            structuralGroupDialog.Save();
            Wait.WaitForDocumentReady();

            //DataPackage dataPackage = this.BuildDataPackage();
            ////Create School NC Year
            //var schoolNcYearId = Guid.NewGuid();
            //dataPackage.AddSchoolNCYearLookup(schoolNcYearId);
            ////Create YearGroup and its set memberships
            //var yearGroupId = Guid.NewGuid();
            //var yearGroupShortName = Utilities.GenerateRandomString(3, "SNADD");
            //var yearGroupFullName = Utilities.GenerateRandomString(10, "AYGMT");
            //dataPackage.AddYearGroupLookup(yearGroupId, schoolNcYearId, yearGroupShortName, yearGroupFullName, 1);

            var sourceYearDialog = bandSchemePage.ClickAddSourceClass();
            sourceYearDialog.SearchResult[0].ClickByJS();
            sourceYearDialog.AddSelectedClass();
            sourceYearDialog.OkButton();
            curriculumStructureTriplet.Save();
            Assert.AreEqual(false, bandSchemePage.IsSuccessMessageDisplayed(), "Structural Group Record Saved");

        }
        #endregion

        [WebDriverTest(TimeoutSeconds = 1200, Enabled = false, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { "P2" }, DataProvider = "TC_CS01_Data")]
        public void SearchBandSchemeByAcademicYear(string[] basicDetails, string[] structuralGroup)
        {
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator, "Curriculum Structure");
            AutomationSugar.NavigateMenu("Tasks", "School Groups", "Curriculum Structure");
            Wait.WaitForDocumentReady();

            Random rnd = new Random();
            int displayOrder = rnd.Next(1, 50);

            var curriculumStructureTriplet = new CurriculumStructureTriplet();
            curriculumStructureTriplet.Create();
            var bandSchemePage = curriculumStructureTriplet.CreateBandSchemePage();
            bandSchemePage.BandSchemeName = basicDetails[0];
            bandSchemePage.AcademicYearDropDown = basicDetails[1];
            var structuralGroupDialog = bandSchemePage.ClickAddTeachingGroup();
            structuralGroupDialog.ShortName = structuralGroup[1];
            structuralGroupDialog.DisplayOrder = displayOrder;
            structuralGroupDialog.Save();
            Wait.WaitForDocumentReady();
            var sourceClassDialog = bandSchemePage.ClickAddSourceClass();
            sourceClassDialog.SearchResult[0].ClickByJS();
            sourceClassDialog.AddSelectedClass();
            sourceClassDialog.OkButton();
            curriculumStructureTriplet.Save();
            var searchResult = curriculumStructureTriplet.SearchCriteria.Search().FirstOrDefault();
        }

        [WebDriverTest(TimeoutSeconds = 1200, Enabled = false, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { "P2" }, DataProvider = "TC_CS01_Data")]
        public void SearchBandSchemeByName(string[] basicDetails, string[] structuralGroup)
        {
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator, "Curriculum Structure");
            AutomationSugar.NavigateMenu("Tasks", "School Groups", "Curriculum Structure");
            Wait.WaitForDocumentReady();

            Random rnd = new Random();
            int displayOrder = rnd.Next(1, 50);

            var curriculumStructureTriplet = new CurriculumStructureTriplet();
            curriculumStructureTriplet.Create();
            var bandSchemePage = curriculumStructureTriplet.CreateBandSchemePage();
            bandSchemePage.BandSchemeName = basicDetails[0];
            bandSchemePage.AcademicYearDropDown = basicDetails[1];
            var structuralGroupDialog = bandSchemePage.ClickAddTeachingGroup();
            structuralGroupDialog.ShortName = structuralGroup[1];
            structuralGroupDialog.DisplayOrder = displayOrder;
            structuralGroupDialog.Save();
            Wait.WaitForDocumentReady();
            var sourceClassDialog = bandSchemePage.ClickAddSourceClass();
            sourceClassDialog.SearchResult[0].ClickByJS();
            sourceClassDialog.AddSelectedClass();
            sourceClassDialog.OkButton();
            curriculumStructureTriplet.Save();
            curriculumStructureTriplet.SearchCriteria.SearchBySchemeName = basicDetails[0];
            var searchResult = curriculumStructureTriplet.SearchCriteria.Search().FirstOrDefault();
        }

        [WebDriverTest(TimeoutSeconds = 1200, Enabled = false, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { "P2" }, DataProvider = "TC_CS01_Data")]
        public void DeleteBandScheme(string[] basicDetails, string[] structuralGroup)
        {
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator, "Curriculum Structure");
            AutomationSugar.NavigateMenu("Tasks", "School Groups", "Curriculum Structure");
            Wait.WaitForDocumentReady();

            Random rnd = new Random();
            int displayOrder = rnd.Next(1, 50);

            var curriculumStructureTriplet = new CurriculumStructureTriplet();
            curriculumStructureTriplet.Create();
            var bandSchemePage = curriculumStructureTriplet.CreateBandSchemePage();
            bandSchemePage.BandSchemeName = basicDetails[0];
            bandSchemePage.AcademicYearDropDown = basicDetails[1];
            var structuralGroupDialog = bandSchemePage.ClickAddTeachingGroup();
            structuralGroupDialog.ShortName = structuralGroup[1];
            structuralGroupDialog.DisplayOrder = displayOrder;
            structuralGroupDialog.Save();
            Wait.WaitForDocumentReady();
            var sourceClassDialog = bandSchemePage.ClickAddSourceClass();
            sourceClassDialog.SearchResult[0].ClickByJS();
            sourceClassDialog.AddSelectedClass();
            sourceClassDialog.OkButton();
            curriculumStructureTriplet.Save();
            Assert.AreEqual(false, bandSchemePage.IsSuccessMessageDisplayed(), "Structural Group Record Saved");
            curriculumStructureTriplet.SearchCriteria.SearchBySchemeName = basicDetails[0];
            var searchResult = curriculumStructureTriplet.SearchCriteria.Search().FirstOrDefault();
            var BandSchemePage = searchResult.Click<BandSchemePage>();
            curriculumStructureTriplet.Delete();
        }

        [WebDriverTest(TimeoutSeconds = 1200, Enabled = false, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { "P3" }, DataProvider = "TC_CS01_Data")]
        public void BandNameValidation(string[] basicDetails, string[] structuralGroup)
        {
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator, "Curriculum Structure");
            AutomationSugar.NavigateMenu("Tasks", "School Groups", "Curriculum Structure");
            Wait.WaitForDocumentReady();

            Random rnd = new Random();
            int displayOrder = rnd.Next(1, 50);

            var curriculumStructureTriplet = new CurriculumStructureTriplet();
            curriculumStructureTriplet.Create();
            var bandSchemePage = curriculumStructureTriplet.CreateBandSchemePage();
            bandSchemePage.BandSchemeName = "";
            bandSchemePage.AcademicYearDropDown = basicDetails[1];
            var structuralGroupDialog = bandSchemePage.ClickAddTeachingGroup();
            structuralGroupDialog.ShortName = structuralGroup[1];
            structuralGroupDialog.DisplayOrder = displayOrder;
            structuralGroupDialog.Save();
            Wait.WaitForDocumentReady();
            var sourceClassDialog = bandSchemePage.ClickAddSourceClass();
            sourceClassDialog.SearchResult[0].ClickByJS();
            sourceClassDialog.AddSelectedClass();
            sourceClassDialog.OkButton();
            curriculumStructureTriplet.Save();
            var ValidationWarning = SeleniumHelper.Get(BandSchemePage.ValidationWarning);
            Assert.IsTrue(ValidationWarning.Displayed, "Validation Warning");

        }

        [WebDriverTest(TimeoutSeconds = 1200, Enabled = false, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { "P3" }, DataProvider = "TC_CS01_Data")]
        public void BandSchemeBeginValidation(string[] basicDetails, string[] structuralGroup)
        {
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator, "Curriculum Structure");
            AutomationSugar.NavigateMenu("Tasks", "School Groups", "Curriculum Structure");
            Wait.WaitForDocumentReady();

            Random rnd = new Random();
            int displayOrder = rnd.Next(1, 50);

            var curriculumStructureTriplet = new CurriculumStructureTriplet();
            curriculumStructureTriplet.Create();
            var bandSchemePage = curriculumStructureTriplet.CreateBandSchemePage();
            var structuralGroupDialog = bandSchemePage.ClickAddTeachingGroup();
            structuralGroupDialog.ShortName = structuralGroup[1];
            structuralGroupDialog.DisplayOrder = displayOrder;
            structuralGroupDialog.Save();
            Wait.WaitForDocumentReady();
            var sourceClassDialog = bandSchemePage.ClickAddSourceClass();
            sourceClassDialog.SearchResult[0].ClickByJS();
            sourceClassDialog.AddSelectedClass();
            sourceClassDialog.OkButton();
            bandSchemePage.BandSchemeName = basicDetails[0];
            bandSchemePage.AcademicYearDropDown = null;
            curriculumStructureTriplet.Save();
            var ValidationWarning = SeleniumHelper.Get(BandSchemePage.ValidationWarning);
            Assert.IsTrue(ValidationWarning.Displayed, "Validation Warning");

        }

        [WebDriverTest(TimeoutSeconds = 1200, Enabled = false, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { "P2" }, DataProvider = "TC_CS01_Data")]
        public void SourceGroupValidation(string[] basicDetails, string[] structuralGroup)
        {
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator, "Curriculum Structure");
            AutomationSugar.NavigateMenu("Tasks", "School Groups", "Curriculum Structure");
            Wait.WaitForDocumentReady();

            Random rnd = new Random();
            int displayOrder = rnd.Next(1, 50);

            var curriculumStructureTriplet = new CurriculumStructureTriplet();
            curriculumStructureTriplet.Create();
            var bandSchemePage = curriculumStructureTriplet.CreateBandSchemePage();
            bandSchemePage.BandSchemeName = basicDetails[0];
            bandSchemePage.AcademicYearDropDown = basicDetails[1];
            var structuralGroupDialog = bandSchemePage.ClickAddTeachingGroup();
            structuralGroupDialog.ShortName = structuralGroup[1];
            structuralGroupDialog.DisplayOrder = displayOrder;
            structuralGroupDialog.Save();
            Wait.WaitForDocumentReady();
            curriculumStructureTriplet.Save();
            var ValidationWarning = SeleniumHelper.Get(BandSchemePage.ValidationWarning);
            Assert.IsTrue(ValidationWarning.Displayed, "Validation Warning");

        }

        [WebDriverTest(TimeoutSeconds = 1200, Enabled = false, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { "P2" }, DataProvider = "TC_CS01_Data")]
        public void StructuralGroupValidation(string[] basicDetails, string[] structuralGroup)
        {
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator, "Curriculum Structure");
            AutomationSugar.NavigateMenu("Tasks", "School Groups", "Curriculum Structure");
            Wait.WaitForDocumentReady();

            var curriculumStructureTriplet = new CurriculumStructureTriplet();
            curriculumStructureTriplet.Create();
            var bandSchemePage = curriculumStructureTriplet.CreateBandSchemePage();
            bandSchemePage.BandSchemeName = basicDetails[0];
            bandSchemePage.AcademicYearDropDown = basicDetails[1];
            var sourceClassDialog = bandSchemePage.ClickAddSourceClass();
            sourceClassDialog.SearchResult[0].ClickByJS();
            sourceClassDialog.AddSelectedClass();
            sourceClassDialog.OkButton();
            curriculumStructureTriplet.Save();
            var ValidationWarning = SeleniumHelper.Get(BandSchemePage.ValidationWarning);
            Assert.IsTrue(ValidationWarning.Displayed, "Validation Warning");

        }


        [WebDriverTest(TimeoutSeconds = 1200, Enabled = false, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { "P2" }, DataProvider = "TC_CS01_Data")]
        public void DeleteAllScheme(string[] basicDetails, string[] structuralGroup)
        {
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator, "Curriculum Structure");
            AutomationSugar.NavigateMenu("Tasks", "School Groups", "Curriculum Structure");
            Wait.WaitForDocumentReady();

            var curriculumStructureTriplet = new CurriculumStructureTriplet();
            curriculumStructureTriplet.SearchCriteria.SearchBySchemeName = "Logigear_";
            var searchResultList = curriculumStructureTriplet.SearchCriteria.Search().ToList();
            foreach (var schemeResult in searchResultList)
            {
                var searchResult = curriculumStructureTriplet.SearchCriteria.Search().FirstOrDefault();
                var CurriculumStructurePage = searchResult.Click<CurriculumStructurePage>();
                curriculumStructureTriplet.Delete();
                curriculumStructureTriplet.Refresh();
            }
        }




        public List<object[]> TC_CS01_Data()
        {
            string random = SeleniumHelper.GenerateRandomString(8);
            string schemeName = "Logigear_" + random;
            string schemeBegins = String.Format("Academic Year {0}/{1}", DateTime.Now.Year - 1, DateTime.Now.Year);
            string schemeEnds = String.Format("Academic Year {0}/{1}", DateTime.Now.Year, DateTime.Now.Year + 1);
            string displayOrder = SeleniumHelper.GenerateRandomNumber(2).ToString();

            string randomTG = SeleniumHelper.GenerateRandomString(5);
            string shortName = "Short_" + randomTG;
            string fullName = "Full_" + randomTG;
            string subject = "English";

            var res = new List<Object[]>
            {
                new object[]
                {
                    // Basic Detail
                    new string[]{ schemeName,schemeBegins,schemeEnds},
                    // Structural Group
                    new string[]{ shortName, fullName, subject, displayOrder }
                }
            };
            return res;
        }


    }
}