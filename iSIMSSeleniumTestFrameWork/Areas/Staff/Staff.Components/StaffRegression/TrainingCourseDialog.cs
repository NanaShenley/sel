using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using SharedComponents.Helpers;

namespace Staff.Components.StaffRegression
{
    public class TrainingCourseDialog : BaseDialogComponent
    {
        public override By DialogIdentifier
        {
            get { return SimsBy.AutomationId("staff_training_course_dialog"); }
        }

        public class StaffRecordTrainingCourseResultTile : SearchResultTileBase
        {
            [FindsBy(How = How.CssSelector, Using = ".h1-result")]
            private IWebElement _title;

            public string Title
            {
                get { return _title.Text; }
            }
        }

        private readonly SearchCriteriaComponent<StaffRecordTrainingCourseResultTile> _searchCriteria;
        public SearchCriteriaComponent<StaffRecordTrainingCourseResultTile> SearchCriteria { get { return _searchCriteria; } }
        
        public TrainingCourseDialog()
        {

            _searchCriteria = new SearchCriteriaComponent<StaffRecordTrainingCourseResultTile>(this);
        }
    }
}
