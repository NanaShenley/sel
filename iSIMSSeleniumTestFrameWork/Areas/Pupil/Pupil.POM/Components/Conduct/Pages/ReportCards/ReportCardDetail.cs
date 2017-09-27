using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using POM.Base;
using POM.Helper;

namespace POM.Components.Conduct.Pages.ReportCards
{
    public class ReportCardDetail : BaseComponent
    {
        #region Page Properties

        [FindsBy(How = How.Name, Using = "Name")]
        private IWebElement _nameTextBox;

        [FindsBy(How = How.Name, Using = "StartDate")]
        private IWebElement _startDateTextBox;

        [FindsBy(How = How.Name, Using = "EndDate")]
        private IWebElement _endDateTextBox;

        [FindsBy(How = How.Name, Using = "Status")]
        private IWebElement _statusTextBox;

        [FindsBy(How = How.Name, Using = "Reason")]
        private IWebElement _reasonTextBox;

        [FindsBy(How = How.Name, Using = "Outcome")]
        private IWebElement _outcomeTextBox;

        [FindsBy(How = How.Name, Using = "Comments")]
        private IWebElement _commentsTextBox;

        #endregion

        #region Page Properties Setters Getters

        public string Name => _nameTextBox.GetValue();
        public string StartDate => _startDateTextBox.GetDateTime();

        public string EndDate => _endDateTextBox.GetDateTime();

        public override By ComponentIdentifier => SimsBy.AutomationId("pupil_reportcard_detail");

        #endregion


        #region Page Actions

        #endregion

    }
}
