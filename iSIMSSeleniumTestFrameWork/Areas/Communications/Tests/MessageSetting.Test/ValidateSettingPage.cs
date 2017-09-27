using NUnit.Framework;
using Selene.Support.Attributes;
using TestSettings;
using MessageSetting.Components;

namespace MessageSetting.Test
{
    public class ValidateSettingPage
    {
        [WebDriverTest(Enabled = true, Groups = new[] { "MessageSettingScreen" }, Browsers = new[] { BrowserDefaults.Chrome })]
        public void CanNavigateToMessageSettingScreen()
        {
            MessageSettingScreen MSSObj = MessageSettingScreenNavigation.navigateToMessageSettingPage();
            Assert.True(MSSObj.isPageDisplayed());            
        }

        [WebDriverTest(Enabled = true, Groups = new[] { "MessageSettingScreen" }, Browsers = new[] { BrowserDefaults.Chrome })]
        public void ConfigureAndValidateGmailSettings()
        {
            MessageSettingScreen MSSObj = MessageSettingScreenNavigation.navigateToMessageSettingPage();

            bool flag = MessageSettingScreenNavigation.configureEmailSettings("Gmail", "capitao365india@gmail.com", "Pa$$w0rd01");
            Assert.True(MessageSettingScreenNavigation.testEmailConnection());

        }

        [WebDriverTest(Enabled = true, Groups = new[] { "MessageSettingScreen" }, Browsers = new[] { BrowserDefaults.Chrome })]
        public void ConfigureAndValidateO365Settings()
        {
            MessageSettingScreen MSSObj = MessageSettingScreenNavigation.navigateToMessageSettingPage();

            bool flag = MessageSettingScreenNavigation.configureEmailSettings("Office365", "capitao365india@gmail.com", "Pa$$w0rd01");
            Assert.True(MessageSettingScreenNavigation.testEmailConnection());
        }

        [WebDriverTest(Enabled = true, Groups = new[] { "MessageSettingScreen" }, Browsers = new[] { BrowserDefaults.Chrome })]
        public void ConfigureAndValidateSendGridSettings()
        {
            MessageSettingScreen MSSObj = MessageSettingScreenNavigation.navigateToMessageSettingPage();

            bool flag = MessageSettingScreenNavigation.configureEmailSettings("SendGrid", "capitao365india@gmail.com", "kaushik12");
            Assert.True(MessageSettingScreenNavigation.testEmailConnection());    
        }

        [WebDriverTest(Enabled = true, Groups = new[] { "MessageSettingScreen" }, Browsers = new[] { BrowserDefaults.Chrome })]
        public void ConfigureAndValidateSMSSetting()
        {
            MessageSettingScreen MSSObj = MessageSettingScreenNavigation.navigateToMessageSettingPage();
            bool flag = MessageSettingScreenNavigation.configureSMSSetting("10000055", "F8CIhOmVywNlWCNNK/XhBu2/iP+1N69MSb327+blbeX9BVMmdLFdw0qDVgpQ+54RM9c5/haUPyF+MsOSa/KN8Q==");
            Assert.True(MessageSettingScreenNavigation.testSMSConnection());
            //10000055 
            //F8CIhOmVywNlWCNNK/XhBu2/iP+1N69MSb327+blbeX9BVMmdLFdw0qDVgpQ+54RM9c5/haUPyF+MsOSa/KN8Q== 
            //bool flag = MessageSettingScreenNavigation.testSMSConnection();

        }

    }
}
