using OpenQA.Selenium;
using SharedComponents;
using SharedComponents.BaseFolder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AddressBook.Components.Pages
{
    public class AddressBookElements : BaseSeleniumComponents
    {
        public static readonly int Timeout = 60;
      
        public static By TextSearch = By.CssSelector("input[data-automation-id='shell_global_search_input']");
        public static By TileTitlePupil = By.CssSelector("span[data-automation-id='global_search_heading_Learner']");
        public static By TileTitlePupilContact = By.CssSelector("span[data-automation-id='global_search_heading_LearnerContact']");
        public static By TileTitleStaff = By.CssSelector("[data-automation-id='global_search_heading_Staff']");
        public static By ClearButton = By.CssSelector("button[id='shell_global_search_clear']");
        public static By ResultsPersonName = By.CssSelector("div[data-automation-id='PreferredListName_Learner']");
        public static By ResultsPersonClassYear = By.CssSelector("div[data-automation-id='Learner_YearGroupClass']");
        public static By NoElementtitle = By.CssSelector("div[data-automation-id='result-counter']");
        public static By DisplayPopup = By.CssSelector("div[data-section-id='global-search-detail']");
        public static By SelectedElement = By.CssSelector("div[data-automation-id='search-result-tile']");
        public static By PlaceHolderForResults = By.CssSelector("div[data-automation-id='result_tile_scroll']");
        public static By PupilName = By.CssSelector("[data-automation-id='detail_displayname']");
        public static By LinkAddressBook = By.CssSelector("a[data-automation-id='globalSearch']");
        public static By HomeScreen = By.Id("workspace");
        public static By PupilRecordQuickLink = By.CssSelector("li[data-automation-id='quicklinks_top_level_pupil_submenu_pupilrecords']");
        public static By AddNewPupilButton = By.CssSelector("a[data-automation-id='add_new_pupil_button']");
        public static By LegalForename = By.CssSelector("input[name='LegalForename']");
        public static By LegalSurname = By.CssSelector("input[name='LegalSurname']");

        public static By Gender = By.CssSelector("select[name='Gender.Binding']");
        public static By MaleOption = By.CssSelector("span[id='select2-chosen-12']");







        public static By TGScreenLink = By.LinkText("School Management");

        public static By OpenedPupilRecordTab = By.CssSelector("li[data-automation-id='tab_Pupil_Record']");
        public static By OpenedPupilLogTab = By.CssSelector("li[data-automation-id='tab_Pupil_Log']");
        public static By OpenedPupilContactTab = By.CssSelector("li[data-automation-id='tab_Pupil_Contact']");
        public static By OpenedStaffRecordTab = By.CssSelector("li[data-automation-id='tab_Staff_Record']");
        public static By TelephoneElement = By.CssSelector("[data-automation-id='communication_telephone_basic']");
        public static By TelephoneIconElement = By.CssSelector("span[data-automation-id='communication_telephone_icon_basic']");
        public static By EmailElement = By.CssSelector("a[data-automation-id='communication_email_basic']");
        public static By AddressElement = By.CssSelector("span[data-automation-id='meaning_home_icon_basic']");
          
    }

    public class AddressBookConstants
    {
        public const double MaxAcceptableSearchTimeInMillisecs = 6000;
        public const String TitleForNoResultsfound = "No results found";
        public const String TitleForResultsFound = "CURRENT PUPILS";
        public const String TitleForPupilContactsFound = "PUPIL CONTACTS";
        public const String TitleForStaffFound = "CURRENT STAFF";
    }
}


