//using DataExchange.POM.Components.Common;
//using DataExchange.POM.Components.DENI;
//using DataExchange.POM.Helper;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using OpenQA.Selenium;
//using Selene.Support.Attributes;
//using SharedComponents.CRUD;
//using TestSettings;

//namespace DENI.Tests.AutoRefresh
//{
//    public class DeniTests
//    {
//        [WebDriverTest(Groups = new[] { Constants.DeniAutoRefresh }, Browsers = new[] { BrowserDefaults.Chrome, BrowserDefaults.Ie }, Enabled = false)]
//        public void CreateDeniAutoRefreshWhenOnOtherscreen()
//        {
//            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.ReturnsManager);

//            SeleniumHelper.NavigateMenu("Tasks", "Statutory Return", "Manage Statutory Returns");

//            StatutoryReturnPageObject deni = new StatutoryReturnPageObject();
//            deni.CreateDeni();
//            string message = deni.CheckIsProcessing();
//            Assert.AreEqual(message, "Processing Return");
//            deni.ClickOnOtherScreen();
//            deni.WaitForProcessingOnOtherScreen();
//            string messageText = deni.ReadAndReturnMessageText();
//            Assert.AreEqual(messageText, "Statutory Return Process");
//        }

//        [WebDriverTest(Groups = new[] { Constants.DeniAutoRefresh }, Browsers = new[] { BrowserDefaults.Chrome, BrowserDefaults.Ie }, Enabled = false)]
//        public void CreateDeniAutoRefresh()
//        {
//            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.ReturnsManager);

//            SeleniumHelper.NavigateMenu("Tasks", "Statutory Return", "Manage Statutory Returns");

//            StatutoryReturnPageObject deni = new StatutoryReturnPageObject();
//            deni.CreateDeni();
//            string message = deni.CheckIsProcessing();
//            Assert.AreEqual(message, "Processing Return");
//            string text = deni.CheckAfterRefresh();
//            Assert.AreEqual("Basic Parameters", text);
//        }

//        [WebDriverTest(Groups = new[] { Constants.DeniAutoRefresh }, Browsers = new[] { BrowserDefaults.Chrome, BrowserDefaults.Ie }, Enabled = false)]
//        public void DeniValidate()
//        {
//            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.ReturnsManager);

//            SeleniumHelper.NavigateMenu("Tasks", "Statutory Return", "Manage Statutory Returns");

//            StatutoryReturnPageObject deni = new StatutoryReturnPageObject();
//            deni.SearchForResults("2015");
//            IWebElement searchResultTile = deni.GetResultElement("Validated with Issues");
//            Assert.IsNotNull(searchResultTile);
//            searchResultTile.Click();

//            Detail.WaitForDetail();
//            deni.ValidateReturn();
//            string message = deni.CheckIsProcessing();
//            Assert.AreEqual(message, "Processing Return");
//            string text = deni.CheckAfterRefresh();
//            Assert.AreEqual("Basic Parameters", text);
//        }
//    }
//}
