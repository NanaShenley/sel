using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using Staff.POM.Base;
using Staff.POM.Helper;
using SeSugar.Automation;

namespace Staff.POM.Components.Staff
{
    public class TrainingCourseTriplet : BaseComponent
    {
        public override By ComponentIdentifier
        {
            get { return SimsBy.AutomationId("staff_training_courses_triplet"); }
        }

        public TrainingCourseTriplet()
        {
            _searchCriteria = new TrainingCourseSearch(this);
        }

        #region Page properties
       
        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='delete_button']")]
        private IWebElement _deleteButton;

        #endregion

        #region Public methods

        /// <summary>
        /// Author: Ba.Truong
        /// Description: Init page Training Course 
        /// </summary>
        /// <returns></returns>
        public static TrainingCourseDetailsPage Create()
        {
            AutomationSugar.ClickOn(new ByChained(SimsBy.AutomationId("staff_training_courses_triplet"), SimsBy.AutomationId("staff_training_courses_create_button")));
            AutomationSugar.WaitForAjaxCompletion();
            return new TrainingCourseDetailsPage();
        }

        /// <summary>
        /// Author: Ba.Truong
        /// Description: click Delete button to delete an existing course
        /// </summary>
        public void Delete()
        {
            if (_deleteButton.IsExist())
            {
                _deleteButton.ClickByJS();
                Wait.WaitForAjaxReady(SimsBy.CssSelector(".locking-mask"));
                var confirmDialog = new DeleteConfirmationPage();
                confirmDialog.ConfirmDelete();
                Refresh();
            }
        }

        public void SelectSearchCourse(TrainingCourseSearchResultTile courseTile)
        {
            if (courseTile != null)
            {
                courseTile.Click();
            }
        }

        #endregion

        #region Search

        private readonly TrainingCourseSearch _searchCriteria;
        public TrainingCourseSearch SearchCriteria { get { return _searchCriteria; } }

        public class TrainingCourseSearchResultTile : SearchResultTileBase
        {
            [FindsBy(How = How.CssSelector, Using = "[data-automation-id='resultTile']")]
            private IWebElement _title;

            public string Title
            {
                get { return _title.Text; }
            }
        }

        #endregion
    }

    public class TrainingCourseSearch : SearchCriteriaComponent<TrainingCourseTriplet.TrainingCourseSearchResultTile>
    {
        public TrainingCourseSearch(BaseComponent parent) : base(parent) { }

        #region Search section properties

        [FindsBy(How = How.Name, Using = "Title")]
        private IWebElement _titleTextBox;

        [FindsBy(How = How.Name, Using = "Venue")]
        private IWebElement _venueTextBox;

        [FindsBy(How = How.Name, Using = "StartDate")]
        private IWebElement _startDateTextBox;

        [FindsBy(How = How.Name, Using = "EndDate")]
        private IWebElement _endDateTextBox;

        public string Title
        {
            set { _titleTextBox.SetText(value); }
            get { return _titleTextBox.GetValue(); }
        }

        public string Venue
        {
            set { _venueTextBox.SetText(value); }
            get { return _venueTextBox.GetValue(); }
        }

        public string StartDate
        {
            set { _startDateTextBox.SetDateTime(value); }
            get { return _startDateTextBox.GetDateTime(); }
        }

        public string EndDate
        {
            set { _endDateTextBox.SetDateTime(value); }
            get { return _endDateTextBox.GetDateTime(); }
        }

        #endregion
    }
}

