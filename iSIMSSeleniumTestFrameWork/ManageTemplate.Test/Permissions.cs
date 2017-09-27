using NUnit.Framework;
using SharedComponents.Helpers;
using WebDriverRunner.internals;
using WebDriverRunner.webdriver;
using SharedComponents.HomePages;
using TestSettings;
using OpenQA.Selenium;
using System;
using SharedComponents.BaseFolder;
using Template.Components;
using System.Collections.ObjectModel;
using Selene.Support.Attributes;

namespace ManageTemplate.Test
{
    class Permissions
    {
        #region story 18208 Permissions
        //Assessment Co-ordinator should not have access to Manage Message Template Menu. It should not be visible in Communication submenu.
          [NotDone]
        [WebDriverTest(Enabled = true, Groups = new[] { "Templates" }, Browsers = new[] { BrowserDefaults.Chrome })]
        public void ValManageMsgTemplateAccessAssessmentCoord()
        {
            String[] featureList = { "SendMessage", "ManageTemplate" };
            TemplateScreenNavigation.LoginWithFeatureBee(featureList, POM.Helper.SeleniumHelper.iSIMSUserType.AssessmentCoordinator);
            Assert.False(TemplateScreenNavigation.ValidateManageMessageTemplateMenu());
        }
        #endregion

        #region story 18208 Permissions
        //Class Teacher should not have access to Manage Message Template Menu. It should not be visible in Communication submenu. 
          [NotDone]
        [WebDriverTest(Enabled = true, Groups = new[] { "Templates" }, Browsers = new[] { BrowserDefaults.Chrome })]
        public void ValManageMsgTemplateAccessClassTeacher()
        {
            String[] featureList = { "SendMessage", "ManageTemplate" };
            TemplateScreenNavigation.LoginWithFeatureBee(featureList, POM.Helper.SeleniumHelper.iSIMSUserType.ClassTeacher);
            Assert.False(TemplateScreenNavigation.ValidateManageMessageTemplateMenu());
        }
        #endregion

        #region story 18208 Permissions
        //Personnel Officer should not have access to Manage Message Template Menu. It should not be visible in Communication submenu. 
          [NotDone]
        [WebDriverTest(Enabled = true, Groups = new[] { "Templates" }, Browsers = new[] { BrowserDefaults.Chrome })]
        public void ValManageMsgTemplateAccessPersonnelOfficer ()
        {
            String[] featureList = { "SendMessage", "ManageTemplate" };
            TemplateScreenNavigation.LoginWithFeatureBee(featureList, POM.Helper.SeleniumHelper.iSIMSUserType.PersonnelOfficer);
            Assert.False(TemplateScreenNavigation.ValidateManageMessageTemplateMenu());
        }
        #endregion

        #region story 18208 Permissions
        //School Administrator should have access to Manage Message Template Menu. It should be visible in Communication submenu.
          [NotDone]
        [WebDriverTest(Enabled = true, Groups = new[] { "Templates" }, Browsers = new[] { BrowserDefaults.Chrome })]
        public void ValManageMsgTemplateAccessSchoolAdmin()
        {
            String[] featureList = { "SendMessage", "ManageTemplate" };
            TemplateScreenNavigation.LoginWithFeatureBee(featureList, POM.Helper.SeleniumHelper.iSIMSUserType.SchoolAdministrator);
            Assert.True(TemplateScreenNavigation.ValidateManageMessageTemplateMenu());
        }
        #endregion

        #region story 18208 Permissions
        //SEN Coordinator should have access to Manage Message Template Menu. It should be visible in Communication submenu. 
          [NotDone]
        [WebDriverTest(Enabled = true, Groups = new[] { "Templates" }, Browsers = new[] { BrowserDefaults.Chrome })]
        public void ValManageMsgTemplateAccessSENCoord()
        {
            String[] featureList = { "SendMessage", "ManageTemplate" };
            TemplateScreenNavigation.LoginWithFeatureBee(featureList, POM.Helper.SeleniumHelper.iSIMSUserType.SENCoordinator);
            Assert.True(TemplateScreenNavigation.ValidateManageMessageTemplateMenu());
        }
        #endregion

        #region story 18208 Permissions
        //Senior Management Team should have access to Manage Message Template Menu. It should be visible in Communication submenu. 
          [NotDone]
        [WebDriverTest(Enabled = true, Groups = new[] { "Templates" }, Browsers = new[] { BrowserDefaults.Chrome })]
        public void ValManageMsgTemplateAccessSeniorManagement()
        {
            String[] featureList = { "SendMessage", "ManageTemplate" };
            TemplateScreenNavigation.LoginWithFeatureBee(featureList, POM.Helper.SeleniumHelper.iSIMSUserType.SeniorManagementTeam);
            Assert.True(TemplateScreenNavigation.ValidateManageMessageTemplateMenu());
        }
        #endregion

        #region story 18208 Permissions
        //System Manager should not have access to Manage Message Template Menu. It should not be visible in Communication submenu. 
          [NotDone]
        [WebDriverTest(Enabled = true, Groups = new[] { "Templates" }, Browsers = new[] { BrowserDefaults.Chrome })]
        public void ValManageMsgTemplateAccessSysManager()
        {
            String[] featureList = { "SendMessage", "ManageTemplate" };
            TemplateScreenNavigation.LoginWithFeatureBee(featureList, POM.Helper.SeleniumHelper.iSIMSUserType.SystemManger);
            Assert.False(TemplateScreenNavigation.ValidateManageMessageTemplateMenu());
        }
        #endregion

        #region story 18208 Permissions
        //Returns Manager should not have access to Manage Message Template Menu. It should not be visible in Communication submenu. 
          [NotDone]
        [WebDriverTest(Enabled = true, Groups = new[] { "Templates" }, Browsers = new[] { BrowserDefaults.Chrome })]
        public void ValManageMsgTemplateAccessReturnsManager()
        {
            String[] featureList = { "SendMessage", "ManageTemplate" };
            TemplateScreenNavigation.LoginWithFeatureBee(featureList, POM.Helper.SeleniumHelper.iSIMSUserType.ReturnsManager);
            Assert.False(TemplateScreenNavigation.ValidateManageMessageTemplateMenu());
        }
        #endregion

        #region story 18208 Permissions
        //Curricular Manager should not have access to Manage Message Template Menu. It should not be visible in Communication submenu. 
          [NotDone]
        [WebDriverTest(Enabled = true, Groups = new[] { "Templates" }, Browsers = new[] { BrowserDefaults.Chrome })]
        public void ValManageMsgTemplateAccessCurricularManager()
        {
            String[] featureList = { "SendMessage", "ManageTemplate" };
            TemplateScreenNavigation.LoginWithFeatureBee(featureList, POM.Helper.SeleniumHelper.iSIMSUserType.CurricularManager);
            Assert.False(TemplateScreenNavigation.ValidateManageMessageTemplateMenu());
        }
        #endregion

        #region story 18208 Permissions
        //Admission Officer should not have access to Manage Message Template Menu. It should not be visible in Communication submenu. 
          [NotDone]
        [WebDriverTest(Enabled = true, Groups = new[] { "Templates" }, Browsers = new[] { BrowserDefaults.Chrome })]
        public void ValManageMsgTemplateAccessAdmissionOfficer()
        {
            String[] featureList = { "SendMessage", "ManageTemplate" };
            TemplateScreenNavigation.LoginWithFeatureBee(featureList, POM.Helper.SeleniumHelper.iSIMSUserType.AdmissionsOfficer);
            Assert.False(TemplateScreenNavigation.ValidateManageMessageTemplateMenu());
        }
        #endregion

    }
}
