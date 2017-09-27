using OpenQA.Selenium;
using TestSettings;

namespace Pupil.Components.Common
{
    public struct PupilElements
    {
        /// <summary>
        /// The pupil record quick link
        /// </summary>
        public const string PupilRecordQuickLink = "#quick-links > div > ul > li:nth-child(3) > a";

        public const string PupilLogQuickLink = "quicklinks_top_level_pupil_submenu_pupillog";
        public const string PupilRecordsQuickLink = "Pupil Records";

        public struct PupilRecord
        {
            // Search panel fields and checkboxes
            public struct PupilSearchPanel
            {
                public static readonly By PupilRecordSearchBox = By.CssSelector("form[class='search-criteria-form'] input[title='Search']");
                public static readonly By PupilRecordCurrentCheckBox = By.Name("StatusCurrentCriterion");
                public static readonly By PupilRecordFutureCheckBox = By.Name("StatusFutureCriterion");
                public static readonly By PupilRecordLeaverCheckBox = By.Name("StatusFormerCriterion");
                public static readonly By SearchButton = By.CssSelector("button[type='submit']");
            }

            // Contextual Actions
            public struct ContextualActions
            {
                public static readonly string SENDetail = "a[data-ajax-url='/{0}/Pupils/SIMS8SENDataMaintenanceLearner/Details/{1}']";
                public static readonly string RecordAsDeceased = "a[data-ajax-url='/{0}/Pupils/LeaverDeceasedDate/Details/{1}']";
            }

            // Pupil detail record
            public static string ReadPupilDetail = "div[data-ajax-url='/{0}/Pupils/SIMS8LearnerMaintenanceSimpleLearner/ReadDetail/{1}']";

            // Basic Details
            public static readonly By LegalForename = By.CssSelector("[name='LegalForename']");

            public static readonly By LegalSurname = By.CssSelector("#editableData input[name='LegalSurname']");
            public static By LegalMiddleName = By.CssSelector("[name='LegalMiddleNames']");

            // Registration
            public static readonly By DateOfAdmission = By.CssSelector("[name='DateOfAdmission']");

            public static readonly By LearnerYearGroupMemberships = By.CssSelector("[id='LearnerYearGroupMemberships']");

            public static readonly By SaveRecord = By.CssSelector("a[title='Save Record']");
            public static By QuickNote = By.Name("QuickNote");

            // Deceased Pupil date screen
            public struct PupilDeceased
            {
                public static readonly By DateOfDeath = By.CssSelector("input[name='DateOfDeath']");
                public static readonly By SaveSuccessMessage = By.CssSelector("div[class^='alert-success']");
                public static readonly By SaveDeceasedDateValidationWarning = By.CssSelector("div[class^='alert-danger']");
            }
        }

        public struct PupilLog
        {
            public static readonly By SearchButton = By.CssSelector("button[type='submit']");
            public static readonly string SearchResultFormat = "div[data-ajax-url=\"/{0}/Learner/PupilLog/ReadDetail/{1}\"]";
            public static readonly By ShowPupilSummary = By.CssSelector("[title=\"Pupil Summary\"]");
            public static readonly string PupilLogTab = "tab_Pupil_Log";

            public static readonly By PupilLogAvatarName = By.CssSelector(".stats-avatar-name dd");

            /// <summary>
            ///
            /// </summary>
            public struct PupilSummaryPopup
            {
                public static readonly By DateOfBirth = By.CssSelector("[name=\"Date Of Birth\"]");
            }

            public struct Detail
            {
                public static readonly string LockingMask = ".locking-mask";
                public static readonly string CreateNoteDropDownBtn = "a[title=\"Add a note about this pupil\"]";
                public static readonly By PupilLogSortFilterButton = By.Id("pupilLogSortFilter");

                public static readonly string DataConfirmationContainer = "div[data-section-id='confirmation-container']";

                public struct TimeLine
                {
                    public static readonly string TimeLineEnntry = "log-timeline-entry";
                }

                public struct Note
                {
                    public static readonly string NoteText = "[name='NoteText']";
                    public static readonly string SaveBtn = "button[title='Save Record']";
                    public static readonly string CancelBtn = "button[title='Cancel the Record']";
                    public static readonly string DeleteBtn = "button[title='Delete this note']";

                    // First button is Yes button. Use Automation Id when available from platform
                    public static readonly string DeleteConfirmationBtn = "div[data-delete-row-buttons] button";

                    public static readonly string NoteEventHeading = "log-event-heading";
                    public static readonly string NoteHeaderGeneral = "log-note-header-general";
                }

                public struct PupilLogSortFilterPanel
                {
                    //Filter/Sort Panel
                    public static readonly By FilterPanelSlideDownState = By.CssSelector("#pane-sort-and-filter.slidedown-filter-panel");

                    //Filter
                    public static readonly By CategoryFilter = By.Name("ParentNoteCategoryFilter");

                    //Sorter
                    public static readonly By Sorter = By.Name("PupilLogSorter");

                    //Filter buttons
                    public static readonly By InActiveGeneralNoteButton = By.CssSelector("label[title='Show General Notes']");

                    public static readonly By ActiveGeneralNoteButton = By.CssSelector("label[title='Show General Notes'].active");

                    public static readonly By InActiveAssessmentNoteButton = By.CssSelector("label[title='Show Assessment Notes']");
                    public static readonly By ActiveAssessmentNoteButton = By.CssSelector("label[title='Show Assessment Notes'].active");

                    public static readonly By InActiveAchievementsNoteButton = By.CssSelector("label[title='Show Achievement Notes']");
                    public static readonly By ActiveAchievementsNoteButton = By.CssSelector("label[title='Show Achievement Notes'].active");

                    public static readonly By InActiveAttendanceNoteButton = By.CssSelector("label[title='Show Attendance Notes']");
                    public static readonly By ActiveAttendanceNoteButton = By.CssSelector("label[title='Show Attendance Notes'].active");

                    public static readonly By InActiveSENNoteButton = By.CssSelector("label[title='Show SEN Notes']");
                    public static readonly By ActiveSENNoteButton = By.CssSelector("label[title='Show SEN Notes'].active");

                    //Sort buttons
                    public static readonly By NewestFirstButton = By.CssSelector("label[title='Newest First']");

                    public static readonly By ActiveNewestFirstButton = By.CssSelector("label[title='Newest First'].active");
                    public static readonly By OldestFirstButton = By.CssSelector("label[title='Oldest First']");
                    public static readonly By ActiveOldestFirstButton = By.CssSelector("label[title='Oldest First'].active");
                    public static readonly By CategoryButton = By.CssSelector("label[title='Sort by Note Category']");
                    public static readonly By ActiveCategoryButton = By.CssSelector("label[title='Sort by Note Category'].active");
                }
            }
        }
    }
}