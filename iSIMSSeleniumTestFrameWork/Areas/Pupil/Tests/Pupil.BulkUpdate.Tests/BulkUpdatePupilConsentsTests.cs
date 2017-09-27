using System;
using System.Threading;
using NUnit.Framework;
using Pupil.Components.BulkUpdate;
using Pupil.Components.Common;
using Pupil.Data;
using Selene.Support.Attributes;
using SeSugar;
using SeSugar.Data;
using TestSettings;

namespace Pupil.BulkUpdate.Tests
{
    public class BulkUpdatePupilConsentsTests
    {
        #region setup
        private readonly object _lockObject = new Object();

        // TODO data-automation-id is screwy when menu item has spaces e.g. "Photograph Student"
        private readonly string _consentMenuDescription = Queries.GetFirstConsentType().Description.Split(" ".ToCharArray())[0];

        #endregion setup

        #region Detail Tests

        /// <summary>
        /// Add Bulk Photo Consent
        /// </summary>
        [WebDriverTest(Enabled = true, Groups =
            new[]
            {
                PupilTestGroups.PupilBulkUpdate.PupilBulkUpdateConsentsDetail,
                PupilTestGroups.Priority.Priority2,
                "BU_CBAOUFAR"
            },
            Browsers =
            new[]
            {
                BrowserDefaults.Chrome
            })]
        public void Can_Bulk_Add_Or_Update_PhotographConsents_For_All_Results()
        {
            Guid pupil1Id = Guid.NewGuid();
            Guid pupil2Id = Guid.NewGuid();
            string yearGroupFullName = Utilities.GenerateRandomString(10, "FNBuT1");
            DataPackage dataPackage = GetDataPackage("BuT1", pupil1Id, pupil2Id, yearGroupFullName);

            using (new DataSetup(purgeBeforeInsert: false, purgeAfterTest: true, packages: dataPackage))
            {
                try
                {
                    //Arrange
                    var bulkUpdatePupilConsentsDetail = BulkUpdateMenuLinks.Consents
                            .Search
                            .WithYearGroupsAs(yearGroupFullName)
                            .SearchAndReturnDetail();


                    //Act
                    bulkUpdatePupilConsentsDetail
                        .SelectConsentType(_consentMenuDescription)
                        .WithFloodFillActiveAs(true)
                        .WithFloodFillConsentStatusAs("Refused")
                        .WithFloodFillConsentDateAsDefault()
                        .Save();

                    //Assert
                    Assert.IsTrue(bulkUpdatePupilConsentsDetail.HasSavedSuccessfully);
                }
                finally
                {
                    PurgeLinkedData.DeleteLearnerConsentType(pupil1Id);
                    PurgeLinkedData.DeleteLearnerConsentType(pupil2Id);
                }
            }
        }

        /// <summary>
        /// Delete Bulk Photo Consent
        /// </summary>
        [WebDriverTest(Enabled = true, Groups =
            new[]
            {
                PupilTestGroups.PupilBulkUpdate.PupilBulkUpdateConsentsDetail,
                PupilTestGroups.Priority.Priority2,
                "BU_CBAOUFAR"
            },
            Browsers =
            new[]
            {
                BrowserDefaults.Chrome
            })]
        public void Can_Bulk_Delete_PhotographConsents_For_All_Results()
        {
            Guid pupil1Id = Guid.NewGuid();
            Guid pupil2Id = Guid.NewGuid();
            string yearGroupFullName = Utilities.GenerateRandomString(10, "FNBuT2");

            DataPackage dataPackage = GetDataPackage("BuT2", pupil1Id, pupil2Id, yearGroupFullName);

            using (new DataSetup(purgeBeforeInsert: false, purgeAfterTest: true, packages: dataPackage))
            {
                try
                {
                    //Arrange
                    var bulkUpdatePupilConsentsDetail = BulkUpdateMenuLinks.Consents
                        .Search
                        .WithYearGroupsAs(yearGroupFullName)
                        .SearchAndReturnDetail();

                    // Add a record
                    bulkUpdatePupilConsentsDetail
                        .SelectConsentType(_consentMenuDescription)
                        .WithFloodFillActiveAs(true)
                        .Save();
                    //Act - delete
                    bulkUpdatePupilConsentsDetail
                        .SelectConsentType(_consentMenuDescription)
                        .WithFloodFillActiveAs(false)
                        .Save();
                    //Assert
                    Assert.IsTrue(bulkUpdatePupilConsentsDetail.HasSavedSuccessfully);
                }
                finally
                {
                    PurgeLinkedData.DeleteLearnerConsentType(pupil1Id);
                    PurgeLinkedData.DeleteLearnerConsentType(pupil2Id);
                }
            }
        }

        #endregion Detail Tests

        private DataPackage GetDataPackage(string suffix, Guid pupil1Id, Guid pupil2Id, string yearGroupFullName)
        {
            lock (_lockObject)
            {
                var _learnerEnrolmentId = Guid.NewGuid();

                var dataPackage = this.BuildDataPackage();

                //Create School NC Year
                var _schoolNCYearId = Guid.NewGuid();
                dataPackage.AddSchoolNCYearLookup(_schoolNCYearId);

                //Create YearGroup and its set memberships
                var _yearGroupId = Guid.NewGuid();
                var yearGroupShortName = Utilities.GenerateRandomString(3, "SN" + suffix);
                dataPackage.AddYearGroupLookup(_yearGroupId, _schoolNCYearId, yearGroupShortName, yearGroupFullName, 1);

                var pupilSurname = Utilities.GenerateRandomString(10, "TestPupil1_Surname" + suffix);
                var pupilForename = Utilities.GenerateRandomString(10, "TestPupil1_Forename" + suffix);

                var _contactSurname1 = Utilities.GenerateRandomString(10, "TestPupilContact1_Surname" + suffix);
                var contactForename1 = Utilities.GenerateRandomString(10, "TestPupilContact1_Forename" + suffix);
                var contactForename2 = Utilities.GenerateRandomString(10, "TestPupilContact2_Forename" + suffix);

                var _firstSalutationName = string.Format("Mr {0}", _contactSurname1);
                var _firstAddresseeName = string.Format("Mr {0} {1}", contactForename1[0], _contactSurname1);

                dataPackage.AddBasicLearner(pupil1Id, pupilSurname, pupilForename, new DateTime(2010, 01, 01),
                    new DateTime(2015, 09, 01), enrolStatus: "C", uniqueLearnerEnrolmentId: _learnerEnrolmentId,
                    salutation: _firstSalutationName, addressee: _firstAddresseeName, yearGroupId: _yearGroupId,
                    schoolNCYearId: _schoolNCYearId);

                //Create second pupil
                _learnerEnrolmentId = Guid.NewGuid();
                pupilSurname = Utilities.GenerateRandomString(10, "TestPupil2_Surname" + suffix);
                pupilForename = Utilities.GenerateRandomString(10, "TestPupil2_Forename" + suffix);

                var _contactSurname2 = Utilities.GenerateRandomString(10, "TestPupilContact2_Surname" + suffix);
                var _lastSalutationName = string.Format("Mr {0}", _contactSurname2);
                var _lastAddresseeName = string.Format("Mr {0} {1}", contactForename2[0], _contactSurname2);
                dataPackage.AddBasicLearner(pupil2Id, pupilSurname, pupilForename, new DateTime(2010, 01, 02),
                    new DateTime(2015, 09, 02), enrolStatus: "C", uniqueLearnerEnrolmentId: _learnerEnrolmentId,
                    salutation: _lastSalutationName, addressee: _lastAddresseeName, yearGroupId: _yearGroupId,
                    schoolNCYearId: _schoolNCYearId);

                return dataPackage;
            }
        }
    }
}