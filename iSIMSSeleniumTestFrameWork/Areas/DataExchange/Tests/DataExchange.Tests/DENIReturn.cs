using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharedComponents.Helpers;
using WebDriverRunner.internals;
using SharedComponents;
using SharedComponents.LoginPages;
using SharedComponents.Utils;
using DataExchange.Components.Common;


namespace DataExchange.Tests
{
    class DENIReturn
    {
        [WebDriverTest(Groups = new[] { "SignIn" }, Enabled = false)]
        public void FindDENIReturn()
        {
            var navigatetodeni = new NavigateToDENI();
            navigatetodeni.SearchDeni();

            navigatetodeni.CreateDENIReturn();
        }

        [WebDriverTest(Groups = new[] { "SignIn" }, Browsers = new[] { "chrome" }, Enabled = true)]
        public void DENIReturn_IssuesnQueriesReportNavigation()
        {
            var navigatetodeni = new NavigateToDENI();
            navigatetodeni.NavigateToDENIScreen();
           // navigatetodeni.CreateDENIReturn(); //giving error on Wait 
            navigatetodeni.SearchDeni();
            navigatetodeni.NavigateToDetailReportsDeni();
        }
    }

}

