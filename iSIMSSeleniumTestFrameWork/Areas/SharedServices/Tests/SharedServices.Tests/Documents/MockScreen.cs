using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Selene.Support.Attributes;
using SeSugar;
using SeSugar.Automation;
using SeSugar.Data;
using SharedComponents.Helpers;
using SharedServices.Components.Common;
using SharedServices.Components.PageObjects;
using SharedServices.Components.Properties;
using SharedServices.TestData;
using TestSettings;

namespace SharedServices.Tests.Documents
{
    [TestClass]
    public class MockScreen
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

        [WebDriverTest(Groups = new[] { "NewDocStoreForTopLevelEntity" }, Browsers = new[] { BrowserDefaults.Ie, BrowserDefaults.Chrome }, TimeoutSeconds = 5000)]
        public void Add_Documents_ToTopLevelEntity()
        {
            // Arrange
            bool actual;
            var fileName = Settings.Default.FilePath;
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator, enabledFeatures: "SharedServiceMockUI");
            var personData = new PersonData("Note1");

            using (new DataSetup(false, false, personData.ToPackage(this)))
            {
                // Act
                AutomationSugar.NavigateMenu("Tasks", "Shared Services", "Learner");
                var triplet = new MockTriplet().SearchFor(personData.Surname);
                var docs = triplet.DocumentsButton.OpenDocuments();

                docs.AttachDocument(fileName);
                docs.ClickOkButton();
                actual = triplet.Save();
                triplet.Close();
            }
            Assert.IsTrue(actual);

            // Assert
            actual = DataHelper.BookmarkExists(personData.Id, fileName);
            Assert.IsTrue(actual);
        }

        [WebDriverTest(Groups = new[] { "NewDocStoreForTopLevelEntity" }, Browsers = new[] { BrowserDefaults.Ie, BrowserDefaults.Chrome },
            TimeoutSeconds = 5000)]
        public void Delete_Document_ToTopLevelEntity()
        {
            // Arrange
            bool actual;
            var fileName = Settings.Default.FilePath;
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator, enabledFeatures: "SharedServiceMockUI");

            var personData = new PersonData("Note1");

            using (new DataSetup(false, false, personData.ToPackage(this)))
            {
                AutomationSugar.NavigateMenu("Tasks", "Shared Services", "Learner");
                var triplet = new MockTriplet().SearchFor(personData.Surname);
                var docs = triplet.DocumentsButton.OpenDocuments();

                docs.AttachDocument(fileName);
                docs.ClickOkButton();
                actual = triplet.Save();
                triplet.Close();

                // Act
                AutomationSugar.NavigateMenu("Tasks", "Shared Services", "Learner");
                triplet = new MockTriplet().SearchFor(personData.Surname);
                docs = triplet.DocumentsButton.OpenDocuments();
                docs.DeleteDocument();
                docs.ClickOkButton();
                triplet.Save();
            }

            Assert.IsTrue(actual);
            actual = DataHelper.BookmarkExists(personData.Id, fileName);

            // Assert
            Assert.IsFalse(actual);
        }


        [WebDriverTest(Groups = new[] { "NewDocStore" }, Browsers = new[] { BrowserDefaults.Ie, BrowserDefaults.Chrome }, TimeoutSeconds = 5000)]
        public void Add_Documents()
        {
            // Arrange
            bool actual;
            var fileName = Settings.Default.FilePath;
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator, enabledFeatures: "SharedServiceMockUI");
            var personData = new PersonData("Note1");

            using (new DataSetup(false, false, personData.ToPackage(this)))
            {
                // Act
                AutomationSugar.NavigateMenu("Tasks", "Shared Services", "Learner");
                var triplet = new MockTriplet().SearchFor(personData.Surname);
                var docs = triplet.DocumentsGrid.GetLastRow().OpenDocuments();

                docs.AttachDocument(fileName);
                docs.ClickOkButton();
                actual = triplet.Save();
                triplet.Close();
            }
            Assert.IsTrue(actual);
            
            // Assert
            actual = DataHelper.BookmarkExists(personData.NoteId, fileName);
            Assert.IsTrue(actual);
        }

        [WebDriverTest(Groups = new[] { "NewDocStore" }, Browsers = new[] { BrowserDefaults.Ie, BrowserDefaults.Chrome }, TimeoutSeconds = 5000)]
        public void Delete_Document()
        {
            // Arrange
            bool actual;
            var fileName = Settings.Default.FilePath;
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator, enabledFeatures: "SharedServiceMockUI");

            var personData = new PersonData("Note1");

            using (new DataSetup(false, false, personData.ToPackage(this)))
            {
                AutomationSugar.NavigateMenu("Tasks", "Shared Services", "Learner");
                var triplet = new MockTriplet().SearchFor(personData.Surname);
                var docs = triplet.DocumentsGrid.GetLastRow().OpenDocuments();

                docs.AttachDocument(fileName);
                docs.ClickOkButton();
                actual = triplet.Save();
                triplet.Close();

                // Act
                AutomationSugar.NavigateMenu("Tasks", "Shared Services", "Learner");
                triplet = new MockTriplet().SearchFor(personData.Surname);
                docs = triplet.DocumentsGrid.GetLastRow().OpenDocuments();
                docs.DeleteDocument();
                docs.ClickOkButton();
                triplet.Save();
            }

            Assert.IsTrue(actual);
            actual = DataHelper.BookmarkExists(personData.NoteId, fileName);
            
            // Assert
            Assert.IsFalse(actual);
        }

        [WebDriverTest(Groups = new[] { "NewDocStore" }, Browsers = new[] { BrowserDefaults.Ie, BrowserDefaults.Chrome }, TimeoutSeconds = 5000)]
        public void Add_Note()
        {
            // Arrange
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator, enabledFeatures: "SharedServiceMockUI");
            var personData = new PersonData();
            bool actual;

            using (new DataSetup(false, false, personData.ToPackage(this)))
            {
                AutomationSugar.NavigateMenu("Tasks", "Shared Services", "Learner");
                var triplet = new MockTriplet().SearchFor(personData.Surname);
                triplet.AddDocumentNote();
                actual = triplet.Save();
                triplet.Close();
            }

            // Assert
            Assert.IsTrue(actual);
        }

        [WebDriverTest(Groups = new[] { "NewDocStore" }, Browsers = new[] { BrowserDefaults.Ie, BrowserDefaults.Chrome }, TimeoutSeconds = 5000)]
        public void Delete_Note()
        {
            // Arrange
            var fileName = Settings.Default.FilePath;
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator, enabledFeatures: "SharedServiceMockUI");
            var personData = new PersonData("Note1");
            bool actual;

            using (new DataSetup(false, false, personData.ToPackage(this)))
            {
                AutomationSugar.NavigateMenu("Tasks", "Shared Services", "Learner");
                var triplet = new MockTriplet().SearchFor(personData.Surname);
                var docs = triplet.DocumentsGrid.GetLastRow().OpenDocuments();

                docs.AttachDocument(fileName);
                docs.ClickOkButton();
                actual = triplet.Save();
                triplet.Close();

                //Act
                AutomationSugar.NavigateMenu("Tasks", "Shared Services", "Learner");
                triplet = new MockTriplet().SearchFor(personData.Surname);
                var note = triplet.DocumentsGrid.GetLastRow();
                note.Remove();
                triplet.Save();
            }
            Assert.IsTrue(actual);
            actual = DataHelper.BookmarkExists(personData.NoteId, fileName);

            // Assert
            Assert.IsFalse(actual);
        }

        private class PersonData
        {
            private readonly Guid _id;
            private readonly string _forename;
            private readonly DateTime _dateOfBirth;
            private readonly DateTime _dateOfAdmission;
            private readonly string _noteSummary;

            public readonly Guid NoteId;
            public readonly string Surname;

            public Guid Id {
                get { return _id; }
            }

            public PersonData(string noteSummary = null)
            {
                _id = Guid.NewGuid();
                _forename = Utilities.GenerateRandomString(10, "Selenium");
                Surname = Utilities.GenerateRandomString(10, "Selenium");

                _dateOfBirth = new DateTime(2005, 05, 30);
                _dateOfAdmission = new DateTime(2012, 10, 03);

                NoteId = Guid.NewGuid();
                _noteSummary = noteSummary;
            }

            public DataPackage ToPackage(object test)
            {
                var package = test.
                    BuildDataPackage()
                    .AddBasicLearner(_id, _forename, Surname, _dateOfBirth, _dateOfAdmission);

                if (_noteSummary != null)
                {
                    package.AddData("LearnerNote", new
                    {
                        Id = NoteId,
                        TenantID = SeSugar.Environment.Settings.TenantId,
                        Learner = _id,
                        Summary = _noteSummary,
                        LastUpdatedDate = _dateOfAdmission
                    });
                }

                return package;
            }
        }
    }
}
