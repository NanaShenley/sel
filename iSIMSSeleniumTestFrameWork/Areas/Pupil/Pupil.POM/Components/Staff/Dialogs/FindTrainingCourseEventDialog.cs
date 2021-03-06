﻿using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using POM.Base;
using POM.Helper;


namespace POM.Components.Staff
{
    public class FindTrainingCourseEventDialog : BaseDialogComponent
    {
        public override By DialogIdentifier
        {
            get { return SimsBy.AutomationId("staff_training_course_dialog"); }
        }

        #region Search
        public class TrainingCourseResultTile : SearchResultTileBase
        {
            [FindsBy(How = How.CssSelector, Using = "[data-automation-id='resultTile']")]
            private IWebElement _name;

            public string Name
            {
                get { return _name.Text; }
            }
        }

        private readonly FindTrainingCourseEventSearchDialog _searchCriteria;
        public FindTrainingCourseEventSearchDialog SearchCriteria { get { return _searchCriteria; } }

        #endregion

        #region Page Actions

        public FindTrainingCourseEventDialog()
        {
            _searchCriteria = new FindTrainingCourseEventSearchDialog(this);
        }

        #endregion
    }

    public class FindTrainingCourseEventSearchDialog : SearchCriteriaComponent<FindTrainingCourseEventDialog.TrainingCourseResultTile>
    {

        #region Page properties

        [FindsBy(How = How.Name, Using = "Title")]
        private IWebElement _trainingTitleTextbox;

        public string TrainingTitle
        {
            set { _trainingTitleTextbox.SetText(value); }
            get { return _trainingTitleTextbox.GetValue(); }
        }

        #endregion

        #region Public methods

        public FindTrainingCourseEventSearchDialog(BaseComponent parent) : base(parent) { }

        #endregion
    }
}
