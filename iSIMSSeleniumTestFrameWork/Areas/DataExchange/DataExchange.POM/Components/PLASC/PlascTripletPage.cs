using DataExchange.POM.Base;
using OpenQA.Selenium;
using SeSugar.Automation;
using System;
using DataExchange.POM.Helper;

namespace DataExchange.POM.Components.PLASC
{
    public class PlascTripletPage : BaseComponent
    {
        public override By ComponentIdentifier
        {
            get
            {
                return Helper.SimsBy.AutomationId("DENI_DETAIL_PAGE");
            }
        }
        private readonly PlascSearchPanel _plascSearchPanel;

        public PlascSearchPanel SearchCriteria { get { return _plascSearchPanel; } }


        public PlascTripletPage()
        {
            _plascSearchPanel = new PlascSearchPanel(this);

        }

        /// <summary>
        /// Create Census
        /// </summary>
        /// <returns></returns>
        public PlascCreateDialog OpenPlascCreateDialog()
        {
            AutomationSugar.ClickOnAndWaitFor("create_button", "create_Deni_dialog");
            return new PlascCreateDialog();
        }

        /// <summary>
        /// Waits till a success notification is shown
        /// </summary>
        /// <param name="timeOutInSeconds"></param>
        /// <returns>true if notification is shown</returns>
        public bool WaitForSuccessNotification(int timeOutInSeconds)
        {
            string jsPredicate = "return $(\"[data-section-id = 'toast-container']\").find(\"[role='alert']\").length > 0";

            Console.WriteLine("Waiting for notification");

            return Wait.WaitTillConditionIsMet(jsPredicate, timeOutInSeconds);
        }

        /// <summary>
        /// Waits till a success notification is shown
        /// </summary>
        /// <param name="timeOutInSeconds"></param>
        /// <returns>true if notification is shown</returns>
        public bool WaitForDetailsViewAutoRefresh(int timeOutInSeconds)
        {
            CensusDetailsSectionPanel censusDetailsSectionPanel = new CensusDetailsSectionPanel();
            string jsPredicate = "return $(\"[data-automation-id = '"+ censusDetailsSectionPanel.AutomationId + "']\").length > 0;";

            Console.WriteLine("Waiting for deatail view refresh");

            return Wait.WaitTillConditionIsMet(jsPredicate, timeOutInSeconds);
        }
        

    }
}
