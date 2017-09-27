using SharedComponents.HomePages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebDriverRunner.webdriver;
using Pupil.Components;
using Pupil.Components.Common;
using SharedComponents.Helpers;
using SharedComponents.HomePages;

namespace AddressBook.Components.Pages
{
    public class NavigateToOtherScreen
    {
        public static void GoToRoomScreen()
        {
            var menu = new TaskMenuBar();
            menu.ClickCommunicationTaskMenuBar();
            SeleniumHelper.WaitForElementClickableThenClick(AddressBookElements.TGScreenLink);
         //   menu.ClickSchoolManagementLink();
            menu.ClickRoomLink();
            TestResultReporter.Log("Successfully Navigated to Rooms Page");
        }

        public static void GoToPupilRecordScreen()
        {
            SeleniumHelper.FindAndClick(SeleniumHelper.AutomationId("quicklinks_top_level_pupil_submenu_pupilrecords"));
        }

        public static void PupilScreenOnTaskMenu(bool senFlag = false)
        {
            var menu = new TaskMenuBar();
            menu.ClickCommunicationTaskMenuBar();
            POM.Helper.SeleniumHelper.Sleep(2);
            menu.ClickPupilLinkWithoutWait();
            menu.ClickPupilRecordsLink(senFlag);
        }

        public static void GoToTGScreen()
        {
            var menu = new TaskMenuBar();
            menu.ClickCommunicationTaskMenuBar();
            POM.Helper.SeleniumHelper.Sleep(2);
            menu.ClickSchoolGroupLinkWithoutWait();
            menu.ClickTeachingGroupLink();
        }


    }
}
