using AddressBook.Components;
using CommunicationLog.Components.Component.Dialogs;
using CommunicationLog.Components.Component.Pages;
using Communications.POM.Components.Communication.Pages;
using NUnit.Framework;
using POM.Helper;
using Selene.Support.Attributes;
using SeSugar.Automation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestSettings;

namespace CommunicationLog.Test
{
    class CommunicationLog
    {
        [WebDriverTest(Enabled = true, Groups = new[] { AddressBookTestGroups.CurrentPupilQuickSearch }, Browsers = new[] { BrowserDefaults.Chrome })]
        public void AddNewManualCommunicationLog()
        {
            String[] featureList = { "CommunicationLogs" };
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator, featureList);

            AddressBookSearchPage searchBox = new AddressBookSearchPage();
            searchBox.ClearText();
            searchBox.EnterSearchTextForPupils("ad");
            AddressBookPopup popup = searchBox.ClickOnFirstPupilRecord();
            popup.ClickCommunicationLogLink();
            Wait.WaitForDocumentReady();
            CommunicationLogDetail log = new CommunicationLogDetail();
            AddLogDialog logDialog = log.AddNewCommunicationLog();
            logDialog.SetMessageFormatType();
            SeleniumHelper.Sleep(2);
            logDialog.SetCategory();
            
            logDialog.Date = DateTime.Now.ToShortDateString();
            logDialog.Description = "Test description";

            logDialog.Refresh();
            logDialog.Save();
        }
    }
}
