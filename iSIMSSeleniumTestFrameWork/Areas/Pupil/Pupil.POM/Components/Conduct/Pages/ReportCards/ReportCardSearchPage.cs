using System.Collections.Generic;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using POM.Base;
using POM.Helper;
// ReSharper disable PrivateMembersMustHaveComments
#pragma warning disable 649

namespace POM.Components.Conduct.Pages.ReportCards
{
    /// <summary>
    /// Implements ReportCardSearchPage
    /// </summary>
    public class ReportCardSearchPage : SearchCriteriaComponent<ReportCardTriplet.ReportCardSearchResultTile>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ReportCardSearchPage"/> class.
        /// </summary>
        /// <param name="component">The component.</param>
        public ReportCardSearchPage(BaseComponent component) : base(component)
        {
        }

        #region Page properties

        /// <summary>
        /// The _pupil name text box
        /// </summary>
        [FindsBy(How = How.Name, Using = "LegalSurname")]
        private IWebElement _pupilNameTextBox;

        /// <summary>
        /// The _current CheckBox
        /// </summary>
        [FindsBy(How = How.Name, Using = "CurrentlyOnReport")]
        private IWebElement _onReportCheckBox;

        /// <summary>
        /// The NoOutcome CheckBox
        /// </summary>
        [FindsBy(How = How.Name, Using = "NoOutcome")]
        private IWebElement _noOutcomeCheckBox;


        /// <summary>
        /// The _current CheckBox
        /// </summary>
        [FindsBy(How = How.Name, Using = "StatusCurrentCriterion")]
        private IWebElement _currentCheckBox;

        /// <summary>
        /// The _future CheckBox
        /// </summary>
        [FindsBy(How = How.Name, Using = "StatusFutureCriterion")]
        private IWebElement _futureCheckBox;

        /// <summary>
        /// The _leaver CheckBox
        /// </summary>
        [FindsBy(How = How.Name, Using = "StatusFormerCriterion")]
        private IWebElement _leaverCheckBox;

        /// <summary>
        /// The _show more button
        /// </summary>
        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='search_criteria_advanced']")]
        private IWebElement _showMoreButton;

        /// <summary>
        /// Drop down for Class
        /// </summary>
        [FindsBy(How = How.Name, Using = "PrimaryClass.dropdownImitator")]
        private IWebElement _classDropDown;

        /// <summary>
        /// Drop down for YearGroup
        /// </summary>
        [FindsBy(How = How.Name, Using = "YearGroup.dropdownImitator")]
        private IWebElement _yearGroupDropDown;

        /// <summary>
        /// Drop down for EnrolmentStatus
        /// </summary>
        [FindsBy(How = How.Name, Using = "EnrolmentStatus.dropdownImitator")]
        private IWebElement _enrolmentDropDown;


        /// <summary>
        /// Gets or sets the name of the pupil.
        /// </summary>
        /// <value>
        /// The name of the pupil.
        /// </value>
        public string PupilName
        {
            set { _pupilNameTextBox.SetText(value); }
            get { return _pupilNameTextBox.GetValue(); }
        }

        public bool IsOnReport
        {
            set { _onReportCheckBox.Set(value); }
            get { return _onReportCheckBox.IsChecked(); }
        }

        public bool NoOutcome
        {
            set { _noOutcomeCheckBox.Set(value); }
            get { return _noOutcomeCheckBox.IsChecked(); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is current.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is current; otherwise, <c>false</c>.
        /// </value>
        public bool IsCurrent
        {
            set { _currentCheckBox.Set(value); }
            get { return _currentCheckBox.IsChecked(); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is future.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is future; otherwise, <c>false</c>.
        /// </value>
        public bool IsFuture
        {
            set { _futureCheckBox.Set(value); }
            get { return _futureCheckBox.IsChecked(); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is leaver.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is leaver; otherwise, <c>false</c>.
        /// </value>
        public bool IsLeaver
        {
            set { _leaverCheckBox.Set(value); }
            get { return _leaverCheckBox.IsChecked(); }
        }

        /// <summary>
        /// Gets or Sets the Class Dropdown value for advanced pupil search
        /// </summary>
        public string Class
        {
            set { _classDropDown.EnterForDropDown(value); }
            get { return _classDropDown.GetText(); }
        }

        /// <summary>
        /// Gets or Sets the YearGroup Dropdown value for advanced pupil search
        /// </summary>
        public string YearGroup
        {
            set { _yearGroupDropDown.EnterForDropDown(value); }
            get { return _yearGroupDropDown.GetText(); }
        }

        /// <summary>
        /// Gets or Sets the EnrolmentStatus Dropdown value for advanced pupil search
        /// </summary>
        public string EnrolmentStatus
        {
            set { _enrolmentDropDown.EnterForDropDown(value); }
            get { return _enrolmentDropDown.GetText(); }
        }

        #endregion Page properties

        #region Action

        /// <summary>
        /// Clicks the show more.
        /// </summary>
        public void ClickShowMore(bool showMore)
        {
            bool isExpanded = _showMoreButton.GetText().Trim().Equals("Show Less");
            if ((showMore && !isExpanded) || (!showMore && isExpanded))
            {
                _showMoreButton.Click();
                Wait.WaitForControl(SimsBy.Name("PrimaryClass.dropdownImitator"));
            }
            _showMoreButton.Click();
        }

        #endregion Action
    }
}