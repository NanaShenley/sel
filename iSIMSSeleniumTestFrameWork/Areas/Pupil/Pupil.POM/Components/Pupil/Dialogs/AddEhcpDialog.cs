using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using POM.Base;
using POM.Helper;


namespace POM.Components.Pupil
{
    public class AddEhcpDialog : BaseDialogComponent
    {
        public override By DialogIdentifier
        {
            get { return SimsBy.AutomationId("ehcp-detail"); }
        }

        #region Page properties

        [FindsBy(How = How.Name, Using = "DateRequestedforAssessment")]
        private IWebElement _dateRequestedTextBox;

        [FindsBy(How = How.Name, Using = "ParentConsultationDate")]
        private IWebElement _dateConsultedTextBox;

        
        public string DateRequested
        {
            set { _dateRequestedTextBox.SetDateTime(value); }
            get { return _dateRequestedTextBox.GetDateTime(); }
        }

        public string DateConsulted
        {
            set { _dateConsultedTextBox.SetDateTime(value); }
            get { return _dateConsultedTextBox.GetDateTime(); }
        }
        #endregion

    }
}
