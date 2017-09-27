using DataExchange.POM.Components.Common;
using DataExchange.POM.Components.LOP.Export;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Selene.Support.Attributes;
using SharedComponents.BaseFolder;
using TestSettings;
using WebDriverRunner.webdriver;

namespace DataExchange.Tests.LOP.Export.AutoRefresh
{
    public class LopExportTest : BaseSeleniumComponents
    {
        [WebDriverTest(Groups = new[] { Constants.LOPAutoRefresh }, Browsers = new[] { BrowserDefaults.Ie, BrowserDefaults.Chrome }, Enabled = false)]
        public void GenerateLopAutorefresh()
        {
            var export = new LopExportRefresh();
            export.SelectRecordAndGenerate("Academic Year 2013/2014");
            string message = export.CheckIsProcessing();
            Assert.AreEqual(message, "File Processing");
            string text = export.CheckAfterRefresh();
            //returns either success or failed message (file not created)
            Assert.AreNotEqual("", text);
        }

        [WebDriverTest(Groups = new[] { Constants.LOPAutoRefresh }, Browsers = new[] { BrowserDefaults.Ie, BrowserDefaults.Chrome }, Enabled = false)]
        public void GenerateLopAutorefreshWithOtherScreenLoad()
        {
            const string academicyear = "Academic Year 2014/2015";
            var export = new LopExportRefresh();
            export.SelectRecordAndGenerate(academicyear);
            export.ClickOnOtherScreen();
            export.WaitForProcessingOnOtherScreen();
            string messageText = export.ReadAndReturnMessageText();
            Assert.AreEqual(messageText, "LOP Export Process");
            WebContext.Screenshot();
        }
    }
}
