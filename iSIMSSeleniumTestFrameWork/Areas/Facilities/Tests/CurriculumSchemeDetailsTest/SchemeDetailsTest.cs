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
    public class SchemeDetailsTest
    {
        #region Story ID : 11693 :- Creating the Curriculum Scheme Details for the current Academic Year.
        [WebDriverTest(TimeoutSeconds = 1200, Enabled = true, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { "P1" }, DataProvider = "TC_CS01_Data")]
        public void CreateScheme(string[] basicDetails, string[] teachingGroup)
        {
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator, "Curriculum Structure");
            AutomationSugar.NavigateMenu("Tasks", "School Groups", "Curriculum Structure");
            Wait.WaitForDocumentReady();

            Random rnd = new Random();
            string displayOrder = rnd.Next(1, 50).ToString();

            var curriculumStructureTriplet = new CurriculumStructureTriplet();
            curriculumStructureTriplet.Create();
            var curriculumStructurePage = curriculumStructureTriplet.CreateTeachingGroupSchemePage();
            curriculumStructurePage.SchemeName = basicDetails[0];
            curriculumStructurePage.AcademicYearBeginDropDown = basicDetails[1];
            curriculumStructurePage.AcademicYearEndDropDown = basicDetails[2];
            var teachingGroupDialog = curriculumStructurePage.ClickAddTeachingGroup();
            teachingGroupDialog.FullName = teachingGroup[0];
            teachingGroupDialog.ShortName = teachingGroup[1];
            teachingGroupDialog.Subject = teachingGroup[2];
            teachingGroupDialog.DisplayOrder = displayOrder;
            teachingGroupDialog.Save();
            Wait.WaitForDocumentReady();
            var sourceClassDialog = curriculumStructurePage.ClickAddSourceClass();
            sourceClassDialog.SearchResult[0].ClickByJS();
            sourceClassDialog.AddSelectedClass();
            sourceClassDialog.OkButton();
            curriculumStructureTriplet.Save();
            Assert.AreEqual(false, curriculumStructurePage.IsSuccessMessageDisplayed(), "Group Scheme Record Saved");

        }
        #endregion

        [WebDriverTest(TimeoutSeconds = 1200, Enabled = true, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { "P2" }, DataProvider = "TC_CS01_Data")]
        public void SearchSchemeByAcademicYearandName(string[] basicDetails, string[] teachingGroup)
        {
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator, "Curriculum Structure");
            AutomationSugar.NavigateMenu("Tasks", "School Groups", "Curriculum Structure");
            Wait.WaitForDocumentReady();

            Random rnd = new Random();
            string displayOrder = rnd.Next(1, 50).ToString();

            var curriculumStructureTriplet = new CurriculumStructureTriplet();
            curriculumStructureTriplet.Create();
            var curriculumStructurePage = curriculumStructureTriplet.CreateTeachingGroupSchemePage();
            curriculumStructurePage.SchemeName = basicDetails[0];
            curriculumStructurePage.AcademicYearBeginDropDown = basicDetails[1];
            curriculumStructurePage.AcademicYearEndDropDown = basicDetails[2];
            var teachingGroupDialog = curriculumStructurePage.ClickAddTeachingGroup();
            teachingGroupDialog.FullName = teachingGroup[0];
            teachingGroupDialog.ShortName = teachingGroup[1];
            teachingGroupDialog.Subject = teachingGroup[2];
            teachingGroupDialog.DisplayOrder = displayOrder;
            teachingGroupDialog.Save();
            Wait.WaitForDocumentReady();
            var sourceClassDialog = curriculumStructurePage.ClickAddSourceClass();
            sourceClassDialog.SearchResult[0].ClickByJS();
            sourceClassDialog.AddSelectedClass();
            sourceClassDialog.OkButton();
            curriculumStructureTriplet.Save();
            Assert.AreEqual(false, curriculumStructurePage.IsSuccessMessageDisplayed(), "Group Scheme Record Saved");

            var nameresult = curriculumStructureTriplet.SearchCriteria.SearchBySchemeName = (basicDetails[0]);
            var result = curriculumStructureTriplet.SearchCriteria.SearchByAcademicYear = (basicDetails[1]);
            var searchResult = curriculumStructureTriplet.SearchCriteria.Search().FirstOrDefault();
        }

        [WebDriverTest(TimeoutSeconds = 1200, Enabled = true, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { "P1" }, DataProvider = "TC_CS01_Data")]
        public void DeleteScheme(string[] basicDetails, string[] teachingGroup)
        {
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator, "Curriculum Structure");
            AutomationSugar.NavigateMenu("Tasks", "School Groups", "Curriculum Structure");
            Wait.WaitForDocumentReady();

            Random rnd = new Random();
            string displayOrder = rnd.Next(1, 50).ToString();

            var curriculumStructureTriplet = new CurriculumStructureTriplet();
            curriculumStructureTriplet.Create();
            var curriculumStructurePage = curriculumStructureTriplet.CreateTeachingGroupSchemePage();
            curriculumStructurePage.SchemeName = basicDetails[0];
            curriculumStructurePage.AcademicYearBeginDropDown = basicDetails[1];
            curriculumStructurePage.AcademicYearEndDropDown = basicDetails[2];
            var teachingGroupDialog = curriculumStructurePage.ClickAddTeachingGroup();
            teachingGroupDialog.FullName = teachingGroup[0];
            teachingGroupDialog.ShortName = teachingGroup[1];
            teachingGroupDialog.Subject = teachingGroup[2];
            teachingGroupDialog.DisplayOrder = displayOrder;
            teachingGroupDialog.Save();
            Wait.WaitForDocumentReady();
            var sourceClassDialog = curriculumStructurePage.ClickAddSourceClass();
            sourceClassDialog.SearchResult[0].ClickByJS();
            sourceClassDialog.AddSelectedClass();
            sourceClassDialog.OkButton();
            curriculumStructureTriplet.Save();
            Assert.AreEqual(false, curriculumStructurePage.IsSuccessMessageDisplayed(), "Group Scheme Record Saved");
            curriculumStructureTriplet.SearchCriteria.SearchBySchemeName = basicDetails[0];
            curriculumStructureTriplet.SearchCriteria.SearchByAcademicYear = basicDetails[1];
            var searchResult = curriculumStructureTriplet.SearchCriteria.Search().FirstOrDefault();
            var CurriculumStructurePage = searchResult.Click<CurriculumStructurePage>();
            curriculumStructureTriplet.Delete();
        }

        [WebDriverTest(TimeoutSeconds = 1200, Enabled = true, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { "P2" }, DataProvider = "TC_CS01_Data")]
        public void NameValidation(string[] basicDetails, string[] teachingGroup)
        {
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator, "Curriculum Structure");
            AutomationSugar.NavigateMenu("Tasks", "School Groups", "Curriculum Structure");
            Wait.WaitForDocumentReady();

            Random rnd = new Random();
            string displayOrder = rnd.Next(1, 50).ToString();

            var curriculumStructureTriplet = new CurriculumStructureTriplet();
            curriculumStructureTriplet.Create();
            var curriculumStructurePage = curriculumStructureTriplet.CreateTeachingGroupSchemePage();
            curriculumStructurePage.SchemeName = "";
            curriculumStructurePage.AcademicYearBeginDropDown = basicDetails[1];
            curriculumStructurePage.AcademicYearEndDropDown = basicDetails[2];
            var teachingGroupDialog = curriculumStructurePage.ClickAddTeachingGroup();
            teachingGroupDialog.FullName = teachingGroup[0];
            teachingGroupDialog.ShortName = teachingGroup[1];
            teachingGroupDialog.Subject = teachingGroup[2];
            teachingGroupDialog.DisplayOrder = displayOrder;
            teachingGroupDialog.Save();
            Wait.WaitForDocumentReady();
            var sourceClassDialog = curriculumStructurePage.ClickAddSourceClass();
            sourceClassDialog.SearchResult[0].ClickByJS();
            sourceClassDialog.AddSelectedClass();
            sourceClassDialog.OkButton();
            curriculumStructureTriplet.Save();
            var ValidationWarning = SeleniumHelper.Get(CurriculumStructurePage.ValidationWarning);
            Assert.IsTrue(ValidationWarning.Displayed, "Validation Warning");

        }

        [WebDriverTest(TimeoutSeconds = 1200, Enabled = true, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { "P2" }, DataProvider = "TC_CS01_Data")]
        public void SchemeBeginValidation(string[] basicDetails, string[] teachingGroup)
        {
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator, "Curriculum Structure");
            AutomationSugar.NavigateMenu("Tasks", "School Groups", "Curriculum Structure");
            Wait.WaitForDocumentReady();

            Random rnd = new Random();
            string displayOrder = rnd.Next(1, 50).ToString();

            var curriculumStructureTriplet = new CurriculumStructureTriplet();
            curriculumStructureTriplet.Create();
            var curriculumStructurePage = curriculumStructureTriplet.CreateTeachingGroupSchemePage();
            var teachingGroupDialog = curriculumStructurePage.ClickAddTeachingGroup();
            teachingGroupDialog.FullName = teachingGroup[0];
            teachingGroupDialog.ShortName = teachingGroup[1];
            teachingGroupDialog.Subject = teachingGroup[2];
            teachingGroupDialog.DisplayOrder = displayOrder;
            teachingGroupDialog.Save();
            Wait.WaitForDocumentReady();
            var sourceClassDialog = curriculumStructurePage.ClickAddSourceClass();
            sourceClassDialog.SearchResult[0].ClickByJS();
            sourceClassDialog.AddSelectedClass();
            sourceClassDialog.OkButton();
            curriculumStructurePage.SchemeName = basicDetails[0];
            curriculumStructurePage.AcademicYearEndDropDown = basicDetails[2];
            curriculumStructurePage.AcademicYearBeginDropDown = null;
            curriculumStructureTriplet.Save();
            var ValidationWarning = SeleniumHelper.Get(CurriculumStructurePage.ValidationWarning);
            Assert.IsTrue(ValidationWarning.Displayed, "Validation Warning");

        }

        [WebDriverTest(TimeoutSeconds = 1200, Enabled = true, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { "P2" }, DataProvider = "TC_CS01_Data")]
        public void SourceClassValidation(string[] basicDetails, string[] teachingGroup)
        {
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator, "Curriculum Structure");
            AutomationSugar.NavigateMenu("Tasks", "School Groups", "Curriculum Structure");
            Wait.WaitForDocumentReady();

            Random rnd = new Random();
            string displayOrder = rnd.Next(1, 50).ToString();

            var curriculumStructureTriplet = new CurriculumStructureTriplet();
            curriculumStructureTriplet.Create();
            var curriculumStructurePage = curriculumStructureTriplet.CreateTeachingGroupSchemePage();
            curriculumStructurePage.SchemeName = basicDetails[0];
            curriculumStructurePage.AcademicYearBeginDropDown = basicDetails[1];
            curriculumStructurePage.AcademicYearEndDropDown = basicDetails[2];
            var teachingGroupDialog = curriculumStructurePage.ClickAddTeachingGroup();
            teachingGroupDialog.FullName = teachingGroup[0];
            teachingGroupDialog.ShortName = teachingGroup[1];
            teachingGroupDialog.Subject = teachingGroup[2];
            teachingGroupDialog.DisplayOrder = displayOrder;
            teachingGroupDialog.Save();
            Wait.WaitForDocumentReady();
            curriculumStructureTriplet.Save();
            var ValidationWarning = SeleniumHelper.Get(CurriculumStructurePage.ValidationWarning);
            Assert.IsTrue(ValidationWarning.Displayed, "Validation Warning");

        }

        [WebDriverTest(TimeoutSeconds = 1200, Enabled = true, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { "P2" }, DataProvider = "TC_CS01_Data")]
        public void TeachingGroupValidation(string[] basicDetails, string[] teachingGroup)
        {
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator, "Curriculum Structure");
            AutomationSugar.NavigateMenu("Tasks", "School Groups", "Curriculum Structure");
            Wait.WaitForDocumentReady();

            var curriculumStructureTriplet = new CurriculumStructureTriplet();
            curriculumStructureTriplet.Create();
            var curriculumStructurePage = curriculumStructureTriplet.CreateTeachingGroupSchemePage();
            curriculumStructurePage.SchemeName = basicDetails[0];
            curriculumStructurePage.AcademicYearBeginDropDown = basicDetails[1];
            curriculumStructurePage.AcademicYearEndDropDown = basicDetails[2];
            var sourceClassDialog = curriculumStructurePage.ClickAddSourceClass();
            sourceClassDialog.SearchResult[0].ClickByJS();
            sourceClassDialog.AddSelectedClass();
            sourceClassDialog.OkButton();
            curriculumStructureTriplet.Save();
            var ValidationWarning = SeleniumHelper.Get(CurriculumStructurePage.ValidationWarning);
            Assert.IsTrue(ValidationWarning.Displayed, "Validation Warning");

        }

        [WebDriverTest(TimeoutSeconds = 1200, Enabled = true, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { "P1" }, DataProvider = "TC_CS01_Data")]
        public void DeleteAllScheme(string[] basicDetails, string[] teachingGroup)
        {
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator, "Curriculum Structure");
            AutomationSugar.NavigateMenu("Tasks", "School Groups", "Curriculum Structure");
            Wait.WaitForDocumentReady();

            Random rnd = new Random();
            string displayOrder = rnd.Next(1, 50).ToString();

            var curriculumStructureTriplet = new CurriculumStructureTriplet();
            curriculumStructureTriplet.SearchCriteria.SearchBySchemeName = "Logigear_";
            //curriculumStructureTriplet.SearchCriteria.SearchByAcademicYear = null;
            var searchResultList = curriculumStructureTriplet.SearchCriteria.Search().ToList();
            if (searchResultList.Count > 0)
            {
                foreach (var schemeResult in searchResultList)
                {
                    var searchResult = curriculumStructureTriplet.SearchCriteria.Search().FirstOrDefault();
                    var CurriculumStructurePage = searchResult.Click<CurriculumStructurePage>();
                    curriculumStructureTriplet.Delete();
                    curriculumStructureTriplet.Refresh();
                }
            }

        }


        public List<object[]> TC_CS01_Data()
        {
            string random = SeleniumHelper.GenerateRandomString(8);
            string schemeName = "Logigear_" + random;
            string schemeBegins = String.Format("Academic Year {0}/{1}", DateTime.Now.Year - 1, DateTime.Now.Year);
            string schemeEnds = String.Format("Academic Year {0}/{1}", DateTime.Now.Year, DateTime.Now.Year + 1);
            int displayOrder = SeleniumHelper.GenerateRandomNumber(2);

            string randomTG = SeleniumHelper.GenerateRandomString(5);
            string shortName = "Short_" + randomTG;
            string fullName = "Full_" + randomTG;
            string subject = "L&L:English";

            var res = new List<Object[]>
            {
                new object[]
                {
                    // Basic Detail
                    new string[]{ schemeName,schemeBegins,schemeEnds},
                    // Teaching Group
                    new string[]{ shortName, fullName, subject, displayOrder.ToString()}
                }
            };
            return res;
        }


    }
}