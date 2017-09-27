using System;
using System.Threading;
using NUnit.Framework;
using OpenQA.Selenium;
using POM.Helper;
using Pupil.BulkUpdate.Tests.ParentalSalutationAndAddresseeComponents;
using Pupil.Components;
using Pupil.Components.Common;
using Pupil.Data;
using Selene.Support.Attributes;
using SeSugar;
using SeSugar.Automation;
using SeSugar.Data;
using TestSettings;
using SeleniumHelper = SharedComponents.Helpers.SeleniumHelper;
using OpenQA.Selenium.Support.UI;
using WebDriverRunner.webdriver;

namespace Pupil.BulkUpdate.Tests
{
    public class BulkUpdateParentalSalutationAndAddresseeTests
    {
        private SeleniumHelper.iSIMSUserType LoginAs = SeleniumHelper.iSIMSUserType.SchoolAdministrator;

        #region Bulk Update Parental Salutation And Addressee Identifier columns

        [WebDriverTest(Enabled = true,
             Groups = new[]
            {
                PupilTestGroups.PupilBulkUpdate.PupilBulkUpdateSalutationAndAddresseeIdentifierColumns,
                PupilTestGroups.Priority.Priority2,
                "BU_CVAIIBPSA"
            },
             Browsers = new[]
            {
                BrowserDefaults.Chrome
            })]
        public void Can_View_All_IdentifierColumns_In_BulkUpdatePupil_Parental_Salutation_Addressee()
        {
            lock (_lockObject)
            {
                try
                {
                    //Arrange
                    DataPackage dataPackage = GetDataPackage("BU_PUP_PSA_T1", 1);
                    using (new DataSetup(purgeBeforeInsert: false, purgeAfterTest: false, packages: dataPackage))
                    {
                        Select_Year_And_NavigateToBulkUpdateDetailScreen();

                        //Act
                        SelectAllColumnsinIndentifierDialog();

                        //Assert
                        SeleniumHelper.Get(ParentalSalutationAndAddresseeDetail.Detail.GridColumns.DateOfBirth);
                        SeleniumHelper.Get(ParentalSalutationAndAddresseeDetail.Detail.GridColumns.LegalName);
                        SeleniumHelper.Get(ParentalSalutationAndAddresseeDetail.Detail.GridColumns.Gender);
                        SeleniumHelper.Get(ParentalSalutationAndAddresseeDetail.Detail.GridColumns.AdmissionNumber);
                        SeleniumHelper.Get(ParentalSalutationAndAddresseeDetail.Detail.GridColumns.Class);
                        SeleniumHelper.Get(ParentalSalutationAndAddresseeDetail.Detail.GridColumns.DateOfAmission);
                        SeleniumHelper.Get(ParentalSalutationAndAddresseeDetail.Detail.GridColumns.YearGroup);
                        SeleniumHelper.Get(ParentalSalutationAndAddresseeDetail.Detail.GridColumns.ParentalSalutation);
                        SeleniumHelper.Get(ParentalSalutationAndAddresseeDetail.Detail.GridColumns.ParentalAddressee);
                        Assert.IsTrue(true);
                    }
                }
                finally
                {
                    //
                }
            }
        }

        [WebDriverTest(Enabled = true,
             Groups = new[]
            {
                PupilTestGroups.PupilBulkUpdate.PupilBulkUpdateSalutationAndAddresseeIdentifierColumns,
                PupilTestGroups.Priority.Priority2, "Sinu2"
            },
             Browsers = new[]
            {
                BrowserDefaults.Chrome
            })]
        public void Can_Clear_IdentifierColumnsin_In_BulkUpdatePupil_Parental_Salutation_Addressee()
        {
            lock (_lockObject)
            {
                try
                {
                    //Arrange
                    DataPackage dataPackage = GetDataPackage("BU_PUP_PSA_T2", 2);
                    using (new DataSetup(purgeBeforeInsert: false, purgeAfterTest: false, packages: dataPackage))
                    {
                        Select_Year_And_NavigateToBulkUpdateDetailScreen();

                        //Act
                        // Now click on clear selection within the Identifier Dialog to remove the selection and click on OK button
                        SeleniumHelper.FindAndClick(PupilBulkUpdateElements.BulkUpdate.Detail.IdentifierButton);
                        SeleniumHelper.FindAndClick(PupilBulkUpdateElements.BulkUpdate.Detail.IdentifierDialog.ClearSelection);
                        Wait.WaitForAjaxReady(By.ClassName("locking-mask-loading"));

                        SeleniumHelper.FindAndClick(PupilBulkUpdateElements.BulkUpdate.Detail.IdentifierDialog.OkButton);
                        Wait.WaitForAjaxReady(By.ClassName("locking-mask-loading"));

                        IWebElement grid = SeleniumHelper.Get(By.CssSelector("[data-section-id=\"searchResults\"]"));
                        var columns = grid.FindElements(By.CssSelector("[data-menu-column-id]"));

                        // Only the Pupil Name column and the two editable columns (Parental saluation and Addreesee) should be present in the grid
                        Assert.IsTrue(columns.Count == 3);
                    }
                }
                finally
                {
                    //
                }
            }
        }

        #endregion Bulk Update Parental Salutation And Addressee Identifier columns

        #region Bulk Update Parental Salutation And Addressee Generate

        #region Salutation

        [WebDriverTest(Enabled = true,
             Browsers = new[]
            {
                BrowserDefaults.Chrome
            },
             Groups = new[]
            {
                PupilTestGroups.PupilBulkUpdate.PupilBulkUpdateSalutationGenerate,
                PupilTestGroups.Priority.Priority2
            })]
        public void Can_FloodFill_Pupil_Parental_Salutation()
        {
            lock (_lockObject)
            {
                try
                {
                    //Arrange
                    DataPackage dataPackage = GetDataPackage("BU_PUP_PSA_T3", 3);
                    using (new DataSetup(purgeBeforeInsert: false, purgeAfterTest: false, packages: dataPackage))
                    {
                        Select_Year_And_NavigateToBulkUpdateDetailScreen();
                        verifyBeforeActSalutationCells();

                        //Act
                        ParentalSalutationAndAddresseeDetail.DeletePupilParentalSalutationColumnValues();
                        ParentalSalutationAndAddresseeDetail.ExecuteJavaScriptToBulkSelectParentalSalutation();
                        ParentalSalutationAndAddresseeDetail.FloodFillSalutationColumnWithOverride();

                        //Assert
                        verifyAfterActSalutationCells();
                    }
                }
                finally
                {
                    //
                }
            }
        }

        [WebDriverTest(Enabled = true,
             Browsers = new[]
            {
                BrowserDefaults.Chrome
            },
             Groups = new[]
            {
                PupilTestGroups.PupilBulkUpdate.PupilBulkUpdateSalutationGenerate,
                PupilTestGroups.Priority.Priority2,
                "BU_CMIPPS"
            })]
        public void Can_Modify_Individual_Pupil_Parental_Salutation()
        {
            lock (_lockObject)
            {
                try
                {
                    //Arrange
                    DataPackage dataPackage = GetDataPackage("BU_PUP_PSA_T4", 6);
                    using (new DataSetup(purgeBeforeInsert: false, purgeAfterTest: false, packages: dataPackage))
                    {
                        Select_Year_And_NavigateToBulkUpdateDetailScreen();
                        verifyBeforeActSalutationCells();

                        //Act
                        ParentalSalutationAndAddresseeDetail.DeletePupilParentalSalutationColumnValues();
                        ParentalSalutationAndAddresseeDetail.ClickFirstCellofColumn(DefaultSalutationColumn);
                        Wait.WaitForAjaxReady(By.ClassName("locking-mask-loading"));
                        ParentalSalutationAndAddresseeDetail.ExecuteJavaScriptToBulkParentalSalutationFloodFillMenuClick();
                        Wait.WaitForAjaxReady(By.ClassName("locking-mask-loading"));
                        ParentalSalutationAndAddresseeDetail.FloodFillSalutationColumnWithOverride();

                        //Assert
                        var cells = (ParentalSalutationAndAddresseeDetail.GetCellText(DefaultSalutationColumn));
                        Assert.IsTrue(cells[0] == FirstSalutationName);
                        IWebElement IsDirtyIndicator = SeleniumHelper.Get(ParentalSalutationAndAddresseeDetail.IsDirtyIndicator);
                        Assert.IsTrue(IsDirtyIndicator.Displayed);
                    }
                }
                finally
                {
                    //
                }
            }
        }

        [WebDriverTest(Enabled = true,
             Browsers = new[]
            {
                BrowserDefaults.Chrome
            },
             Groups = new[]
            {
                PupilTestGroups.PupilBulkUpdate.PupilBulkUpdateSalutationGenerate,
                PupilTestGroups.Priority.Priority2,
                "BU_CFPPSWONC"
            })]
        public void Can_FloodFill_Pupil_Parental_Salutation_With_Override_Not_Checked()
        {
            lock (_lockObject)
            {
                try
                {
                    //Arrange
                    DataPackage dataPackage = GetDataPackage("BU_PUP_PSA_T1", 9);
                    using (new DataSetup(purgeBeforeInsert: false, purgeAfterTest: false, packages: dataPackage))
                    {
                        Select_Year_And_NavigateToBulkUpdateDetailScreen();
                        verifyBeforeActSalutationCells();

                        //Act
                        ParentalSalutationAndAddresseeDetail.ExecuteJavaScriptToBulkSelectParentalSalutation();
                        Wait.WaitForAjaxReady(By.ClassName("locking-mask"));
                        IWebElement applyToSelected = SeleniumHelper.Get(ParentalSalutationAndAddresseeDetail.FloodFillGenerateForSelected);
                        applyToSelected.Click();
                        Wait.WaitForAjaxReady(By.ClassName("locking-mask"));
                        IWebElement IsDirtyIndicator = SeleniumHelper.Get(ParentalSalutationAndAddresseeDetail.IsDirtyIndicator);

                        //Assert
                        Assert.IsFalse(IsDirtyIndicator.Displayed);
                    }
                }
                finally
                {
                    //
                }
            }
        }

        #endregion Salutation

        #region Addressee

        [WebDriverTest(Enabled = true, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { PupilTestGroups.PupilBulkUpdate.PupilBulkUpdateAddresseeGenerate, PupilTestGroups.Priority.Priority2 })]
        public void Can_FloodFill_Pupil_Parental_Addressee()
        {
            lock (_lockObject)
            {
                try
                {
                    //Arrange
                    DataPackage dataPackage = GetDataPackage("BU_PUP_PSA_T2", 15);
                    using (new DataSetup(purgeBeforeInsert: false, purgeAfterTest: false, packages: dataPackage))
                    {
                        Select_Year_And_NavigateToBulkUpdateDetailScreen();
                        verifyBeforeActAddresseeCells();

                        //Act
                        ParentalSalutationAndAddresseeDetail.DeletePupilParentalAddresseeColumnValues();
                        ParentalSalutationAndAddresseeDetail.ExecuteJavaScriptToBulkSelectParentalAddressee();
                        ParentalSalutationAndAddresseeDetail.FloodFillAddresseeColumnWithOverride();

                        //Assert
                        verifyAfterActAddresseeCells();
                    }
                }
                finally
                {
                    //
                }
            }
        }

        [WebDriverTest(Enabled = true, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { PupilTestGroups.PupilBulkUpdate.PupilBulkUpdateAddresseeGenerate, PupilTestGroups.Priority.Priority2, "BU_CUIPA" })]
        public void Can_Update_individual_Parental_Addressee()
        {
            lock (_lockObject)
            {
                try
                {
                    //Arrange
                    DataPackage dataPackage = GetDataPackage("BU_PUP_PSA_T3", 3);
                    using (new DataSetup(purgeBeforeInsert: false, purgeAfterTest: false, packages: dataPackage))
                    {
                        Select_Year_And_NavigateToBulkUpdateDetailScreen();
                        verifyBeforeActAddresseeCells();

                        //Act
                        ParentalSalutationAndAddresseeDetail.DeletePupilParentalAddresseeColumnValues();
                        ParentalSalutationAndAddresseeDetail.ClickFirstCellofColumn(DefaultAddresseeColumn);
                        Wait.WaitForAjaxReady(By.ClassName("locking-mask"));
                        ParentalSalutationAndAddresseeDetail.ExecuteJavaScriptToBulkParentalAddresseeFloodFillMenuClick();
                        Wait.WaitForAjaxReady(By.ClassName("locking-mask"));
                        ParentalSalutationAndAddresseeDetail.FloodFillAddresseeColumnWithOverride();

                        //Assert
                        var cells = (ParentalSalutationAndAddresseeDetail.GetCellText(DefaultAddresseeColumn));
                        Assert.IsTrue(cells[0] == FirstAddresseeName);
                        IWebElement IsDirtyIndicator = SeleniumHelper.Get(ParentalSalutationAndAddresseeDetail.IsDirtyIndicator);
                        Assert.IsTrue(IsDirtyIndicator.Displayed);
                    }
                }
                finally
                {
                    //
                }
            }
        }

        [WebDriverTest(Enabled = true, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { PupilTestGroups.PupilBulkUpdate.PupilBulkUpdateAddresseeGenerate, PupilTestGroups.Priority.Priority2 })]
        public void Can_FloodFill_Pupil_Parental_Addressee_With_Override_Not_Checked()
        {
            lock (_lockObject)
            {
                try
                {
                    //Arrange
                    DataPackage dataPackage = GetDataPackage("BU_PUP_PSA_T4", 6);
                    using (new DataSetup(purgeBeforeInsert: false, purgeAfterTest: false, packages: dataPackage))
                    {
                        Select_Year_And_NavigateToBulkUpdateDetailScreen();
                        verifyBeforeActAddresseeCells();

                        //Act
                        ParentalSalutationAndAddresseeDetail.ExecuteJavaScriptToBulkSelectParentalAddressee();
                        Wait.WaitForAjaxReady(By.ClassName("locking-mask"));
                        IWebElement applyToSelected = SeleniumHelper.Get(ParentalSalutationAndAddresseeDetail.ParentalAddresseeFloodFillGenerateForSelected);
                        applyToSelected.Click();
                        Wait.WaitForAjaxReady(By.ClassName("locking-mask"));
                        IWebElement IsDirtyIndicator = SeleniumHelper.Get(ParentalSalutationAndAddresseeDetail.IsDirtyIndicator);

                        //Assert
                        Assert.IsFalse(IsDirtyIndicator.Displayed);
                    }
                }
                finally
                {
                    //
                }
            }
        }

        #endregion Addressee

        #endregion Bulk Update Parental Salutation And Addressee Generate

        #region Common

        private string contactSurname1;
        private string contactSurname2;
        private string yearGroupFullName;
        private const string DefaultSalutationColumn = "2";
        private const string DefaultAddresseeColumn = "3";
        private string FirstSalutationName;
        private string FirstAddresseeName;
        private string LastSalutationName;
        private string LastAddresseeName;
        private Guid schoolNCYearId;
        private Guid yearGroupId;
        private readonly object _lockObject = new Object();

        private DataPackage GetDataPackage(string suffix, int sleep)
        {
            lock (_lockObject)
            {
                Thread.Sleep(TimeSpan.FromSeconds(sleep));
                var dataPackage = this.BuildDataPackage();
                //Create School NC Year
                schoolNCYearId = Guid.NewGuid();
                dataPackage.AddSchoolNCYearLookup(schoolNCYearId);
                //Create YearGroup and its set memberships
                yearGroupId = Guid.NewGuid();
                var yearGroupShortName = Utilities.GenerateRandomString(3, "SN");
                yearGroupFullName = Utilities.GenerateRandomString(10, "FN");
                dataPackage.AddYearGroupLookup(yearGroupId, schoolNCYearId, yearGroupShortName, yearGroupFullName, 1);

                //Create first pupil
                Guid pupilId = Guid.NewGuid();
                Guid learnerEnrolmentId = Guid.NewGuid();
                Guid applicantId = Guid.NewGuid();
                var pupilSurname = Utilities.GenerateRandomString(10, "TestPupil1_Surname" + suffix);
                var pupilForename = Utilities.GenerateRandomString(10, "TestPupil1_Forename" + suffix);

                contactSurname1 = Utilities.GenerateRandomString(10, "TestPupilContact1_Surname" + suffix);
                var contactForename1 = Utilities.GenerateRandomString(10, "TestPupilContact1_Forename" + suffix);
                var contactForename2 = Utilities.GenerateRandomString(10, "TestPupilContact2_Forename" + suffix);

                FirstSalutationName = string.Format("Mr {0}", contactSurname1);
                FirstAddresseeName = string.Format("Mr {0} {1}", contactForename1[0], contactSurname1);

                dataPackage.AddBasicLearner(pupilId, pupilSurname, pupilForename, new DateTime(2010, 01, 01),
                    new DateTime(2015, 09, 01), enrolStatus: "C", uniqueLearnerEnrolmentId: learnerEnrolmentId,
                    salutation: FirstSalutationName, addressee: FirstAddresseeName, yearGroupId: yearGroupId,
                    schoolNCYearId: schoolNCYearId);

                #region Pre-Condition: Create new contact 1 and refer to pupil

                //Arrange
                AutomationSugar.Log("Create new contact1 and refer to pupil");
                Guid pupilContactId1 = Guid.NewGuid();
                Guid pupilContactRelationshipId1 = Guid.NewGuid();
                //Add pupil contact
                dataPackage.AddPupilContact(pupilContactId1, contactSurname1, contactForename1);
                dataPackage.AddPupilContactRelationship(pupilContactRelationshipId1, pupilId, pupilContactId1,
                    hasParentalResponsibility: true);
                dataPackage.AddBasicLearnerContactAddress(pupilId, pupilContactId1, Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(),
                    new DateTime(2016, 01, 01));

                #endregion Pre-Condition: Create new contact 1 and refer to pupil

                //Create second pupil
                pupilId = Guid.NewGuid();
                learnerEnrolmentId = Guid.NewGuid();
                applicantId = Guid.NewGuid();
                pupilSurname = Utilities.GenerateRandomString(10, "TestPupil2_Surname" + suffix);
                pupilForename = Utilities.GenerateRandomString(10, "TestPupil2_Forename" + suffix);

                contactSurname2 = Utilities.GenerateRandomString(10, "TestPupilContact2_Surname" + suffix);
                LastSalutationName = string.Format("Mr {0}", contactSurname2);
                LastAddresseeName = string.Format("Mr {0} {1}", contactForename2[0], contactSurname2);
                dataPackage.AddBasicLearner(pupilId, pupilSurname, pupilForename, new DateTime(2010, 01, 02),
                    new DateTime(2015, 09, 02), enrolStatus: "C", uniqueLearnerEnrolmentId: learnerEnrolmentId,
                    salutation: LastSalutationName, addressee: LastAddresseeName, yearGroupId: yearGroupId,
                    schoolNCYearId: schoolNCYearId);

                #region Pre-Condition: Create new contact 2 and refer to pupil

                AutomationSugar.Log("Create new contact2 and refer to pupil");
                Guid pupilContactId2 = Guid.NewGuid();
                Guid pupilContactRelationshipId2 = Guid.NewGuid();
                //Add pupil contact

                dataPackage.AddPupilContact(pupilContactId2, contactSurname2, contactForename2);
                dataPackage.AddPupilContactRelationship(pupilContactRelationshipId2, pupilId, pupilContactId2,
                    hasParentalResponsibility: true);
                dataPackage.AddBasicLearnerContactAddress(pupilId, pupilContactId2, Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(),
                    new DateTime(2016, 01, 01));

                #endregion Pre-Condition: Create new contact 2 and refer to pupil

                return dataPackage;
            }
        }

        private void Select_Year_And_NavigateToBulkUpdateDetailScreen()
        {
            var bulkUpdateNavigation = new PupilBulkUpdateNavigation();
            bulkUpdateNavigation.NavgateToPupilBulkUpdate_SubMenu(PupilBulkUpdateElements.BulkUpdate.MenuItems.PupilSalutationAddresseeMenuItem, LoginAs);

            Wait.WaitForAjaxReady(By.ClassName("locking-mask"));

            SeleniumHelper.ToggleCheckbox(PupilBulkUpdateElements.BulkUpdate.Search.MissingSalutationCheckboxName);
            SeleniumHelper.ToggleCheckbox(PupilBulkUpdateElements.BulkUpdate.Search.MissingAddresseeCheckboxName);

            SeleniumHelper.ToggleCheckboxForLabel("section_menu_Year Group", yearGroupFullName);

            Wait.WaitForAjaxReady(By.ClassName("locking-mask"));

            SeleniumHelper.FindAndClick(PupilBulkUpdateElements.BulkUpdate.Search.SearchButton, TimeSpan.FromSeconds(20));
            Wait.WaitForAjaxReady(By.ClassName("locking-mask"));
        }

        private void SelectAllColumnsinIndentifierDialog()
        {
            SeleniumHelper.FindAndClick(PupilBulkUpdateElements.BulkUpdate.Detail.IdentifierButton);
            Wait.WaitForAjaxReady(By.ClassName("locking-mask"));

            //Click on Personal details and registration details so that all the columns are included in the grid
            WebDriverWait wait = new WebDriverWait(WebContext.WebDriver, BrowserDefaults.TimeOut);

            wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("div[webix_tm_id='Personal Details']:first-of-type + div.webix_tree_leaves input[type='checkbox']")));

            SeleniumHelper.Get(PupilBulkUpdateElements.BulkUpdate.Detail.IdentifierDialog.PersonalDetails).Click();
            Wait.WaitForAjaxReady(By.ClassName("locking-mask"));

            SeleniumHelper.Get(PupilBulkUpdateElements.BulkUpdate.Detail.IdentifierDialog.RegistrationDetails).Click();
            Wait.WaitForAjaxReady(By.ClassName("locking-mask"));

            // Click on the ok button so that the columns are included in the grid
            SeleniumHelper.FindAndClick(PupilBulkUpdateElements.BulkUpdate.Detail.IdentifierDialog.OkButton);
            Wait.WaitForAjaxReady(By.ClassName("locking-mask"));
        }

        private void verifyBeforeActSalutationCells()
        {
            var cells = ParentalSalutationAndAddresseeDetail.GetCellText(DefaultSalutationColumn);
            Assert.AreEqual(cells[0], FirstSalutationName);
            Assert.AreEqual(cells[cells.Count - 1], LastSalutationName);
        }

        private void verifyBeforeActAddresseeCells()
        {
            var cells = ParentalSalutationAndAddresseeDetail.GetCellText(DefaultAddresseeColumn);
            Assert.AreEqual(cells[0], FirstAddresseeName);
            Assert.AreEqual(cells[cells.Count - 1], LastAddresseeName);
        }

        private void verifyAfterActSalutationCells()
        {
            var cells = ParentalSalutationAndAddresseeDetail.GetCellText(DefaultSalutationColumn);
            Assert.IsTrue(cells[0] == FirstSalutationName);
            Assert.IsTrue(cells[cells.Count - 1] == LastSalutationName);
            IWebElement IsDirtyIndicator = SeleniumHelper.Get(ParentalSalutationAndAddresseeDetail.IsDirtyIndicator);
            Assert.IsTrue(IsDirtyIndicator.Displayed, "Dirty indicator expected");
        }

        private void verifyAfterActAddresseeCells()
        {
            var cells = ParentalSalutationAndAddresseeDetail.GetCellText(DefaultAddresseeColumn);
            Assert.IsTrue(cells[0] == FirstAddresseeName);
            Assert.IsTrue(cells[cells.Count - 1] == LastAddresseeName);
            IWebElement IsDirtyIndicator = SeleniumHelper.Get(ParentalSalutationAndAddresseeDetail.IsDirtyIndicator);
            Assert.IsTrue(IsDirtyIndicator.Displayed, "Dirty indicator expected");
        }

        #endregion Common
    }
}