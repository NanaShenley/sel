using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;

namespace Facilities.Components.Common
{
    public class RoomElements
    {
        public static readonly By CreateRoom = By.CssSelector("[data-automation-id='add_button']");
        public static readonly By RoomTelephonenumber = By.Name("TelephoneNumber");
        public static readonly By Roomarea = By.Name("Area");
        public static readonly By Roommaxgroupsize = By.Name("MaximumGroupSize");
        public static readonly By Save = By.CssSelector("[data-automation-id='well_know_action_save']");
        public static readonly By Roomselectuser = By.CssSelector("[data-automation-id='select_button']");
        public static readonly By Savesuccessfuldisplayed = By.CssSelector("[data-automation-id='status_success']");
        public static readonly By DeleteButton = By.CssSelector("[data-automation-id='delete_button']");
        public static readonly By ContinueWithDelete = By.CssSelector("[data-automation-id='continue_with_delete_button']");
        public static readonly By CancelButton = By.CssSelector("[data-automation-id='well_know_action_Cancel']");
        public static readonly By Dntsavebutton = By.CssSelector("[data-automation-id='ignore_commit_dialog']");
        public static readonly By RoomValidationWarning = By.CssSelector("[data-automation-id='status_error']");
        public static readonly By CheckBoxActive = By.Name("IsActive");
        public static readonly By IncludeInactiveRooms = By.Name("IsActive");

        //  *******************************************SEARCH PANEL*******************************************
        public static readonly By Roomsearchbutton = By.CssSelector("[data-automation-id='search_criteria_submit']");
        public static readonly By SearchResults = By.CssSelector("[data-automation-id='search_results']");
        public static readonly By RoomselectSearchResults = By.CssSelector("[data-automation-id='search_results']");
    }
}
