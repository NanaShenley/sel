using Microsoft.VisualStudio.TestTools.UnitTesting;
using Selene.Support.Attributes;
using SeSugar.Automation;
using SeSugar.Data;
using Staff.POM.Components.Staff;
using Staff.POM.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Staff.Tests.Lookups
{
    [TestClass]
    public class PostTypeLookupTests
    {
        #region MS Unit Testing support
        public TestContext TestContext { get; set; }
        [TestInitialize]
        public void Init()
        {
            TestRunner.VSSeleniumTest.Init(this, TestContext);
        }
        [TestCleanup]
        public void Cleanup()
        {
            TestRunner.VSSeleniumTest.Cleanup(TestContext);
        }
        #endregion
        [TestMethod]
        [ChromeUiTest("PostTypeLookupTests", "P1", "PostTypeCanCreate")]
        public void CanCreate()
        {
            DataPackage testData = new DataPackage();

            Guid statutoryPostTypeId;
            string postTypeCode = CoreQueries.GetColumnUniqueString("PostType", "Code", 4);
            string postTypeDescription = CoreQueries.GetColumnUniqueString("PostType", "Description", 20);
            string statutoryPostTypeCode = CoreQueries.GetColumnUniqueString("StatutoryPostType", "Code", 4);
            string statutoryPostTypeDescription = CoreQueries.GetColumnUniqueString("StatutoryPostType", "Description", 20);

            testData.AddData("StatutoryPostType", DataPackageHelper.GenerateStatutoryPostType(out statutoryPostTypeId, statutoryPostTypeCode, statutoryPostTypeDescription));

            using (new DataSetup(testData))
            {
                SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.PersonnelOfficer);
                AutomationSugar.NavigateMenu("Lookups", "Staff", "School Post Types");

                PostTypeLookupDouble postTypeLookupDouble = new PostTypeLookupDouble();

                PostTypeLookupDialog postTypeDialog = postTypeLookupDouble.ClickAddButton();
                postTypeDialog.Code = postTypeCode;
                postTypeDialog.Description = postTypeDescription;
                postTypeDialog.StatutoryPostType = statutoryPostTypeDescription;
                postTypeDialog.ClickOk();

                postTypeLookupDouble.ClickSave();

                // Save successful
                Assert.IsTrue(AutomationSugar.SuccessMessagePresent(postTypeLookupDouble.ComponentIdentifier), "Post Type save failed");

                postTypeLookupDouble.SearchCriteria.CodeOrDescription = postTypeDescription;
                PostTypeLookupDetailsPage searchResultsPage = postTypeLookupDouble.SearchCriteria.Search<PostTypeLookupDetailsPage>();

                int rowCount = searchResultsPage.PostTypes.Rows.Count();

                // Record found using search
                Assert.AreEqual(1, rowCount);
            }
        }
        [TestMethod]
        [ChromeUiTest("PostTypeLookupTests", "P1", "PostTypeCanRead")]
        public void CanRead()
        {
            DataPackage testData = new DataPackage();

            Guid postTypeId, statutoryPostTypeId;
            string postTypeCode = CoreQueries.GetColumnUniqueString("PostType", "Code", 4);
            string postTypeDescription = CoreQueries.GetColumnUniqueString("PostType", "Description", 20);
            string statutoryPostTypeCode = CoreQueries.GetColumnUniqueString("StatutoryPostType", "Code", 4);
            string statutoryPostTypeDescription = CoreQueries.GetColumnUniqueString("StatutoryPostType", "Description", 20);

            testData.AddData("StatutoryPostType", DataPackageHelper.GenerateStatutoryPostType(out statutoryPostTypeId, statutoryPostTypeCode, statutoryPostTypeDescription));
            testData.AddData("PostType", DataPackageHelper.GeneratePostType(out postTypeId, code: postTypeCode, description: postTypeDescription, statutoryPostTypeId: statutoryPostTypeId));

            using (new DataSetup(testData))
            {
                SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.PersonnelOfficer);
                AutomationSugar.NavigateMenu("Lookups", "Staff", "School Post Types");

                PostTypeLookupDouble postTypeLookupDouble = new PostTypeLookupDouble();

                postTypeLookupDouble.SearchCriteria.CodeOrDescription = postTypeDescription;
                PostTypeLookupDetailsPage searchResultsPage = postTypeLookupDouble.SearchCriteria.Search<PostTypeLookupDetailsPage>();

                int rowCount = searchResultsPage.PostTypes.Rows.Count();

                // Record found using search
                Assert.AreEqual(1, rowCount);
            }
        }

        [ChromeUiTest("PostTypeLookupTests", "P1", "PostTypeCanUpdate")]
        public void CanUpdate()
        {
            DataPackage testData = new DataPackage();

            Guid postTypeId, statutoryPostTypeId;
            string postTypeCode = CoreQueries.GetColumnUniqueString("PostType", "Code", 4);
            string postTypeDescription = CoreQueries.GetColumnUniqueString("PostType", "Description", 20);
            string postTypeNewDescription = CoreQueries.GetColumnUniqueString("PostType", "Description", 20);
            string statutoryPostTypeCode = CoreQueries.GetColumnUniqueString("StatutoryPostType", "Code", 4);
            string statutoryPostTypeDescription = CoreQueries.GetColumnUniqueString("StatutoryPostType", "Description", 20);

            testData.AddData("StatutoryPostType", DataPackageHelper.GenerateStatutoryPostType(out statutoryPostTypeId, statutoryPostTypeCode, statutoryPostTypeDescription));
            testData.AddData("PostType", DataPackageHelper.GeneratePostType(out postTypeId, code: postTypeCode, description: postTypeDescription, statutoryPostTypeId: statutoryPostTypeId));

            using (new DataSetup(testData))
            {
                SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.PersonnelOfficer);
                AutomationSugar.NavigateMenu("Lookups", "Staff", "School Post Types");

                PostTypeLookupDouble postTypeLookupDouble = new PostTypeLookupDouble();

                postTypeLookupDouble.SearchCriteria.CodeOrDescription = postTypeDescription;
                PostTypeLookupDetailsPage searchResultsPage = postTypeLookupDouble.SearchCriteria.Search<PostTypeLookupDetailsPage>();

                int rowCount = searchResultsPage.PostTypes.Rows.Count();

                // Record found using existing description
                Assert.AreEqual(1, rowCount);

                searchResultsPage.PostTypes.Rows[0].ClickEdit();

                PostTypeLookupDialog postTypeDialog = new PostTypeLookupDialog();

                postTypeDialog.Description = postTypeNewDescription;
                postTypeDialog.ClickOk();

                postTypeLookupDouble.ClickSave();

                // Save successful
                Assert.IsTrue(AutomationSugar.SuccessMessagePresent(postTypeLookupDouble.ComponentIdentifier), "Post Type save failed");

                postTypeLookupDouble.SearchCriteria.CodeOrDescription = postTypeNewDescription;
                searchResultsPage = postTypeLookupDouble.SearchCriteria.Search<PostTypeLookupDetailsPage>();

                rowCount = searchResultsPage.PostTypes.Rows.Count();

                // Record found using new description
                Assert.AreEqual(1, rowCount);
            }
        }

        [TestMethod]
        [ChromeUiTest("PostTypeLookupTests", "P1", "PostTypeCanDelete")]
        public void CanDelete()
        {
            DataPackage testData = new DataPackage();

            Guid postTypeId, statutoryPostTypeId;
            string postTypeCode = CoreQueries.GetColumnUniqueString("PostType", "Code", 4);
            string postTypeDescription = CoreQueries.GetColumnUniqueString("PostType", "Description", 20);
            string statutoryPostTypeCode = CoreQueries.GetColumnUniqueString("StatutoryPostType", "Code", 4);
            string statutoryPostTypeDescription = CoreQueries.GetColumnUniqueString("StatutoryPostType", "Description", 20);

            testData.AddData("StatutoryPostType", DataPackageHelper.GenerateStatutoryPostType(out statutoryPostTypeId, statutoryPostTypeCode, statutoryPostTypeDescription));
            testData.AddData("PostType", DataPackageHelper.GeneratePostType(out postTypeId, code: postTypeCode, description: postTypeDescription, statutoryPostTypeId: statutoryPostTypeId));

            using (new DataSetup(testData))
            {
                SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.PersonnelOfficer);
                AutomationSugar.NavigateMenu("Lookups", "Staff", "School Post Types");

                PostTypeLookupDouble postTypeLookupDouble = new PostTypeLookupDouble();

                postTypeLookupDouble.SearchCriteria.CodeOrDescription = postTypeDescription;
                PostTypeLookupDetailsPage searchResultsPage = postTypeLookupDouble.SearchCriteria.Search<PostTypeLookupDetailsPage>();

                int rowCount = searchResultsPage.PostTypes.Rows.Count();

                // Row exists
                Assert.AreEqual(1, rowCount);

                searchResultsPage.PostTypes.Rows[0].DeleteRow();

                postTypeLookupDouble.ClickSave();

                // Delete successful
                Assert.IsTrue(AutomationSugar.SuccessMessagePresent(postTypeLookupDouble.ComponentIdentifier), "Post Type delete failed");

                postTypeLookupDouble.SearchCriteria.CodeOrDescription = postTypeDescription;
                searchResultsPage = postTypeLookupDouble.SearchCriteria.Search<PostTypeLookupDetailsPage>();

                rowCount = searchResultsPage.PostTypes.Rows.Count();

                // Row no longer exists
                Assert.AreEqual(0, rowCount);
            }
        }
    }
}
