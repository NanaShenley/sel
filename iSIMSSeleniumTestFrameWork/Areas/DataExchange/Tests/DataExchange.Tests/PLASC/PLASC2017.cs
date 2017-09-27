using TestSettings;
using DataExchange.POM.Helper;
using SeSugar.Automation;
using NUnit.Framework;
using DataExchange.POM.Components.PLASC;
using Selene.Support.Attributes;

namespace DataExchange.Tests.PLASC
{
    public class PLASC2017
    {
        public const string Return_Type = "Wales PLASC Return";
        public const string Return_Version = "PLASC 2017";
        
        [Variant(Variant.WelshStatePrimary)]
        [WebDriverTest(Enabled = false, Browsers = new[] {BrowserDefaults.Chrome }, Groups = new[] { "PLASC" })]
        public void Plasc_Create_New_Return_Test()
        {
            //Arrange
            LoginAndNavigateToPlascScreen();

            PlascTripletPage plascTripletPage = new PlascTripletPage();

            //Act - Open add dialog and create new return
            PlascCreateDialog plascCreateDialog = plascTripletPage.OpenPlascCreateDialog();
            plascCreateDialog.ReturnTypeDropdown = PLASC2017.Return_Type;
            plascCreateDialog.ReturnTypeVersionDropdown = PLASC2017.Return_Version;
            plascTripletPage = plascCreateDialog.ClickOkButtonAndWaitAjaxForCompletion();

            #region commented code - Alternate way to assert
            /*
            //Search and verify if return is created
            plascTripletPage.SearchCriteria.ReturnTypeDropdown = this.Return_Type;
            plascTripletPage.SearchCriteria.ReturnTypeVersionDropdown = this.Return_Version;
            Wait.WaitTillAllAjaxComplete();

            var plascSearchTiles = plascTripletPage.SearchCriteria.Search();

            Assert.IsTrue(plascSearchTiles.Count() > 0);
            */
            #endregion

            //Assert - Let detail view refresh
            Assert.IsTrue(plascTripletPage.WaitForDetailsViewAutoRefresh(120));

        }

        [Variant(Variant.WelshStatePrimary)]
        [WebDriverTest(Enabled = false, Browsers = new[] { BrowserDefaults.Chrome, }, Groups = new[] { "PLASC" })]
        public void Plasc_Census_Details_Test()
        {
            //Arrange
            LoadReturnDetailPanel();

            //Act
            CensusDetailsSectionPanel basicDetailsSectionPanel = new CensusDetailsSectionPanel();
            ActAssertSectionPanelTest(basicDetailsSectionPanel);
        }

        [Variant(Variant.WelshStatePrimary)]
        [WebDriverTest(Enabled = false, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { "PLASC" })]
        public void Plasc_School_Information_Test()
        {
            //Arrange
            LoadReturnDetailPanel();

            //Act
            SchoolInformationPanel schoolInformationPanel = new SchoolInformationPanel();
            ActAssertSectionPanelTest(schoolInformationPanel);
        }

        [Variant(Variant.WelshStatePrimary)]
        [WebDriverTest(Enabled = false, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { "PLASC" })]
        public void Plasc_Onroll_Pupils_Test()
        {
            //Arrange
            LoadReturnDetailPanel();

            //Act
            OnRollPupilsSectionPanel onRollPupilsSectionPanel = new OnRollPupilsSectionPanel();
            ActAssertSectionPanelTest(onRollPupilsSectionPanel);
        }

        [Variant(Variant.WelshStatePrimary)]
        [WebDriverTest(Enabled = false, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { "PLASC" })]
        public void Plasc_All_Teachers_Test()
        {
            //Arrange
            LoadReturnDetailPanel();

            //Act
            AllTeachersPanel allTeachersPanel = new AllTeachersPanel();
            ActAssertSectionPanelTest(allTeachersPanel);
        }
       
        [Variant(Variant.WelshStatePrimary)]
        [WebDriverTest(Enabled = false, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { "PLASC" })]
        public void Plasc_Welsh_Teachers_Test()
        {
            //Arrange
            LoadReturnDetailPanel();

            //Act
            WelshTeachersPanel welshTeachersPanel = new WelshTeachersPanel();
            ActAssertSectionPanelTest(welshTeachersPanel);
        }

        [Variant(Variant.WelshStatePrimary)]
        [WebDriverTest(Enabled = false, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { "PLASC" })]
        public void Plasc_Survey_Completion_Time_Test()
        {
            //Arrange
            LoadReturnDetailPanel();

            //Act
            SurveyCompletionTimePanel surveyCompletionTimePanel = new SurveyCompletionTimePanel();
            ActAssertSectionPanelTest(surveyCompletionTimePanel);
        }

        [Variant(Variant.WelshStatePrimary)]
        [WebDriverTest(Enabled = false, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { "PLASC" })]
        public void Plasc_Recruitment_Test()
        {
            //Arrange
            LoadReturnDetailPanel();

            //Act
            RecruitmentPanel recruitmentPanel = new RecruitmentPanel();
            ActAssertSectionPanelTest(recruitmentPanel);
        }

        [Variant(Variant.WelshStatePrimary)]
        [WebDriverTest(Enabled = false, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { "PLASC" })]
        public void Plasc_Retention_Test()
        {
            //Arrange
            LoadReturnDetailPanel();

            //Act
            RetentionPanel retentionPanel = new RetentionPanel();
            ActAssertSectionPanelTest(retentionPanel);
        }

        [Variant(Variant.WelshStatePrimary)]
        [WebDriverTest(Enabled = false, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { "PLASC" })]
        public void Plasc_Support_Staff_Test()
        {
            //Arrange
            LoadReturnDetailPanel();

            //Act
            SupportSatffPanel supportStaffPanel = new SupportSatffPanel();
            ActAssertSectionPanelTest(supportStaffPanel);
        }

        [Variant(Variant.WelshStatePrimary)]
        [WebDriverTest(Enabled = false, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { "PLASC" })]
        public void Plasc_Teachers_Contracts_Test()
        {
            //Arrange
            LoadReturnDetailPanel();

            //Act
            TeachersContractsPanel teachersContractsPanel = new TeachersContractsPanel();
            ActAssertSectionPanelTest(teachersContractsPanel);
        }

        [Variant(Variant.WelshStatePrimary)]
        [WebDriverTest(Enabled = false, Browsers = new[] { BrowserDefaults.Chrome, }, Groups = new[] { "PLASC" })]
        public void Plasc_General_Section_Test()
        {
            //Arrange
            LoadReturnDetailPanel();

            //Act
            GeneralSectionPanel generalSectionPanel = new GeneralSectionPanel();
            ActAssertSectionPanelTest(generalSectionPanel);
        }

        [Variant(Variant.WelshStatePrimary)]
        [WebDriverTest(Enabled = false, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { "PLASC" })]
        public void Plasc_Ethnicity_Language_Test()
        {
            //Arrange
            LoadReturnDetailPanel();

            //Act
            EthnicityLanguagePanel ethnicityLanguagePanel = new EthnicityLanguagePanel();
            ActAssertSectionPanelTest(ethnicityLanguagePanel);
        }

        [Variant(Variant.WelshStatePrimary)]
        [WebDriverTest(Enabled = false, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { "PLASC" })]
        public void Plasc_Pupil_Exclusions_Test()
        {
            //Arrange
            LoadReturnDetailPanel();

            //Act
            PupilExclusionsPanel pupilExclusionsPanel = new PupilExclusionsPanel();
            ActAssertSectionPanelTest(pupilExclusionsPanel);
        }

        [Variant(Variant.WelshStatePrimary)]
        [WebDriverTest(Enabled = false, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { "PLASC" })]
        public void Plasc_SignOff_Test()
        {
            //Arrange
            LoadReturnDetailPanel();

            AuthoriseReturn authorise = new AuthoriseReturn();
            authorise.ClickAuthoriseButton();
            authorise.AuthoriseDialog();
   
            SignOffPanel signOffPanel = new SignOffPanel();
            ActAssertSectionPanelTest(signOffPanel);
        }

        [Variant(Variant.WelshStatePrimary)]
        [WebDriverTest(Enabled = false, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { "PLASC" })]
        public void Plasc_School_ClassSection_Test()
        {
            //Arrange
            LoadReturnDetailPanel();

            //Act
            ClassSectionPanel classSectionPanel = new ClassSectionPanel();
            ActAssertSectionPanelTest(classSectionPanel);
        }
        [Variant(Variant.WelshStatePrimary)]
        [WebDriverTest(Enabled = false, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { "PLASC" })]
        public void Plasc_OnRollPupil_SEN_Section_Test()
        {
            //Arrange
            LoadReturnDetailPanel();

            //Act
            OnRollPupilSENSection onRollPupilSENSection = new OnRollPupilSENSection();
            ActAssertSectionPanelTest(onRollPupilSENSection);
        }

        private void LoginAndNavigateToPlascScreen()
        {            
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.ReturnsManager, true, "Stats_Return_Plasc2017");

            AutomationSugar.NavigateMenu("Tasks", "Statutory Return", "Manage Statutory Returns");
        }

        /// <summary>
        /// Loads return page, searches return, creates a return if needed and populates detail screen panel
        /// </summary>
        private void LoadReturnDetailPanel()
        {
            //Navigate to statutory return screen
            LoginAndNavigateToPlascScreen();

            PlascTripletPage plascTripletPage = new PlascTripletPage();

            //Perform search
            plascTripletPage.SearchCriteria.ReturnTypeDropdown = PLASC2017.Return_Type;
            plascTripletPage.SearchCriteria.ReturnTypeVersionDropdown = PLASC2017.Return_Version;
            Wait.WaitTillAllAjaxCallsComplete();

            var plascSearchTiles = plascTripletPage.SearchCriteria.Search();

            if (plascTripletPage.SearchCriteria.ClickSearchResultItemIfAny())
            {
                //wait till details are loaded
                Wait.WaitTillAllAjaxCallsComplete();
            }
            else
            {
                /**no Serach results, let's create one**/
                
                //Open add dialog and create new return
                PlascCreateDialog plascCreateDialog = plascTripletPage.OpenPlascCreateDialog();
                plascCreateDialog.ReturnTypeDropdown = PLASC2017.Return_Type;
                plascCreateDialog.ReturnTypeVersionDropdown = PLASC2017.Return_Version;
                plascTripletPage = plascCreateDialog.ClickOkButtonAndWaitAjaxForCompletion();

                //Let search result refresh
                plascTripletPage.WaitForDetailsViewAutoRefresh(120);

                //wait till details are loaded
                Wait.WaitTillAllAjaxCallsComplete();
            }


        }

        /// <summary>
        /// Acts & Asserts for section panel test
        /// </summary>
        /// <param name="plascSectionPanel"></param>
        private void ActAssertSectionPanelTest(PlascSectionPanelBase plascSectionPanel)
        {
            if (plascSectionPanel == null)
            {
                return;
            }

            //Act
            if (plascSectionPanel.IsSectionPanelExists())
            {
                if (!plascSectionPanel.IsSectionPanelOpen())
                {
                    plascSectionPanel.ToggleSectionPanel();
                    Wait.WaitTillAllAjaxCallsComplete();
                }

                //Assert
                Assert.IsTrue(plascSectionPanel.CheckIfValidDataExist());
            }
            else
            {
                throw new System.Exception(string.Format("{0} Panel does not exist.", plascSectionPanel.PanelName));
            }
        }       
    }
}
