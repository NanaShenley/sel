using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facilities.Components.Common
{
   public class MySchoolDetailsElements
    {
        public static readonly By Save = By.CssSelector("[data-automation-id='well_know_action_save']");
        public static readonly By Savesuccessfuldisplayed = By.CssSelector("[data-automation-id='status_success']");
        public static readonly By DeleteButton = By.CssSelector("[data-automation-id='remove_button']");
        public static readonly By ContinueWithDelete = By.CssSelector("[data-automation-id='continue_with_delete_button']");
        public static readonly By CancelButton = By.CssSelector("[data-automation-id='well_know_action_Cancel']");
        public static readonly By ValidationWarning = By.CssSelector("[data-automation-id='status_error']");
        public static readonly By Addschoolsitepopup = By.CssSelector("[data-automation-id='add_site_and_buildings_popup_header_title']");
        public static readonly By AddschoolsiteLink = By.CssSelector("[data-automation-id='add_site_and_buildings_button']");
        public static readonly By MySchoolAddress = By.CssSelector("[data-automation-id='data_section_schooladdres']");
        public static readonly By OkButton = By.CssSelector("[data-automation-id='ok_button']");
        public static readonly By YesDelete = By.CssSelector("[data-automation-id='Yes_button']");
        public static readonly By EditButton = By.CssSelector("[data-automation-id='edit..._button']");
       public static readonly By AddAddressPopupTitle = By.CssSelector("[data-automation-id='add_address_popup_header_title']");
       public static readonly By AddBuildingPopupTitle = By.CssSelector("[data-automation-id='add_building_popup_header_title']");
    }
}
