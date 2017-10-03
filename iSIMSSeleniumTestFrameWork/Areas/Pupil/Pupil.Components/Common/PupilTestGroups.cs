using Pupil.Components.DataEntityIO;

namespace Pupil.Components.Common
{
    /// <summary>
    /// These determine which tests are run by the TestRunner. Syntax is command line parameter --include "name-of-group". 
    /// Can be specified > 1 times to allow several groups to run. 
    /// </summary>
    public class PupilTestGroups
    {
        // Each test should have Priority, Page and the section within the Page
        // eg PupilLog MaintainState should have the following Test Groups 
        // Groups = new[] {PupilLog.Page, PupilLog.MaintainState, Priority.Priority1}

        // Each menu route should have its own struct
        // Each element within the struct should be prefixed by the struct name
        // Each struct should have a Page variable that is the struct name

        public struct Priority
        {
            public const string Priority1 = "P1";
            public const string Priority2 = "P2";
            public const string Priority3 = "P3";
            public const string Priority4 = "P4";
        }

        public struct PupilRecord
        {
            public const string Page = "PupilRecord";

            public const string Deceased = "PupilRecordDeceased";
            public const string Search = "PupilRecordSearch";
            public const string Update = "PupilRecordUpdate";
            public const string ContextualActions = "PupilRecordContextualActions";
            public const string SaveAlert = "PupilRecordSaveAlert";
            public const string AddNewPupil = "PupilRecordAddNewPupil";
            public const string Address = "PupilRecordAddress";
            public const string Contact = "PupilRecordContact";
            public const string Leaver = "PupilRecordPupilLeaver";
            public const string PupilUndoLeaver = "PupilRecordUndoLeaver";
            public const string PupilRecordConduct = "PupilRecordConduct";
        }

        public struct Application
        {
            public const string Page = "Application";
            public const string AddNew = "ApplicationAddNew";
            public const string ViewReport = "ApplicationViewReport";

            public const string ApplicationDOAChange = "ApplicationDOAChange";
        }

        public struct SchoolIntake
        {
            public const string Page = "SchoolIntake";
            public const string Create = "SchoolIntakeCreate";
            public const string Delete = "SchoolIntakeDelete";
            public const string Clone = "SchoolIntakeClone";
        }

        public struct PupilContact
        {
            public const string Page = "PupilContact";
            public const string Create = "PupilContactCreate";
            public const string Delete = "PupilContactDelete";
        }

        public struct PupilDelete
        {
            public const string Page = "PupilDelete";
            public const string Delete = "PupilRecordDeletePupil";
        }

        public struct SENRecord
        {
            public const string Page = "SenRecord";
        }

        public struct ManageLeavers
        {
            public const string Page = "ManageLeavers";
        }

        public struct PupilLog
        {
            public const string Page = "PupilLog";

            public const string FilterPanel = "PupilLogFilterPanel";
            public const string ActionLinks = "ActionLinks";
            public const string CreateNotes = "CreateNotes";
            public const string MaintainState = "PupilLogMaintainState";
            public const string Stats = "PupilLogStats";
        }

        public struct ClassLog
        {
            public const string Page = "ClassLog";

            public const string ClassLogPermissions = "ClassLogPermissions";
            public const string Load = "ClassLogLoad";
            public const string Indicator = "ClassLogIndicator";
            public const string ClassLogNote = "ClassLogNote";

            public const string Conduct = "ClassLogConduct";
        }

        public struct PupilPremium
        {
            public const string Page = "PupilPremium";

            public const string Permissions = "PupilPremiumPermissions";
            public const string Load = "PupilPremiumLoad";
        }

        public struct PupilBulkUpdate
        {
            public const string Page = "PupilBulkUpdate";

            public const string PupilBulkUpdateBasicDetailsSearch = "PupilBulkUpdateBasicDetailsSearch";
            public const string PupilBulkUpdateConsentsSearch = "PupilBulkUpdateConsentsSearch";
            public const string PupilBulkUpdateConsentsDetail = "PupilBulkUpdateConsentsDetail";
            public const string PupilBulkUpdateSalutationAddresseeSearch = "PupilBulkUpdateSalutationAddresseeSearch";
            public const string PupilBulkUpdateIdentifierColumns = "PupilBulkUpdateIdentifierColumns";
            public const string PupilBulkUpdateBackButtonSearch = "PupilBulkUpdateBackButtonSearch";
            public const string PupilBulkUpdatePermissions = "PupilBulkUpdatePermissions";
            public const string PupilBulkUpdateMenu = "PupilBulkUpdateMenu";

            public const string PupilBulkUpdateSalutationAndAddresseeIdentifierColumns =
                "PupilBulkUpdateSalutationAndAddresseeIdentifierColumns";

            public const string PupilBulkUpdateSalutationGenerate = "PupilBulkUpdateSalutationGenerate";
            public const string PupilBulkUpdateAddresseeGenerate = "PupilBulkUpdateAddresseeGenerate";
        }

        public struct ApplicantBulkUpdate
        {
            public const string Page = "ApplicationBulkUpdate";

            public const string ApplicationStatusIdentifierColumns =
                "ApplicantBulkUpdateApplicationStatusIdentifierColumns";

            public const string ApplicationStatusGridMenu = "ApplicantBulkUpdateApplicationStatusGridMenu";
            public const string ApplicationStatusGrid = "ApplicantBulkUpdateApplicationStatusGrid";

            public const string ApplicantBulkUpdateSalutationAddresseeSearch =
                "ApplicantBulkUpdateSalutationAddresseeSearch";

            public const string ApplicantBulkUpdateSalutationGenerate = "ApplicantBulkUpdateSalutationGenerate";
            public const string ApplicantBulkUpdateAddresseeGenerate = "ApplicantBulkUpdateAddresseeGenerate";
        }

        public struct Suspensions
        {
            public const string Page = "Suspensions";
            public const string SuspensionsSearch = "SuspensionsSearch";
        }

        public struct BehaviourEvent
        {
            public const string Page = "BehaviourEvent";
            public const string AddNewBehaviourEvents = "AddNewBehaviourEvents";
            public const string UpdateBehaviourEvents = "UpdateBehaviourEvents";
        }

        public struct AchievementEvent
        {
            public const string Page = "AchievementEvent";
            public const string AddNewAchievementEvents = "AddNewAchievementEvents";
            public const string UpdateAchievementEvents = "UpdateAchievementEvents";
        }

        public struct ConductSummary
        {
            public const string Page = "ConductSummary";
        }

        public struct ConductConfiguration
        {
            public const string Page = "ConductConfiguration";
        }

        public struct ReportCards
        {
            public const string Page = "ReportCards";
            public const string ReportCardSearch = "ReportCardSearch";
            public const string RecordOutcome = "RecordOutcome";
        }
    }
}