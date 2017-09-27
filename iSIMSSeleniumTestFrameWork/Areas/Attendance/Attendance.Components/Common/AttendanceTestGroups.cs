using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Configuration;

namespace Attendance.Components.Common
{
    // <summary>
    /// These determine which tests are run by the TestRunner. Syntax is command line parameter --include "name-of-group". 
    /// Can be specified > 1 times to allow several groups to run.
    /// </summary>
    public class AttendanceTestGroups
    {
        public static readonly int Timeout = 60;
    }


    public struct EditMarksGroup
    {
        public const string EditMarksTests = "EditMarksTest";
        public const string EditMarksSearchTests = "EditMarksSearchTest";
        public const string EditMarksPermissionsTests = "EditMarksPermissionsTests";
    }

    public struct ExceptionalCircumstanceGroup
    {
        public const string ExpectedTitle = "Exceptional Circumstance Details";
        public const string ExceptionalCircumstanceWholeSchoolGroup = "Whole School";
        public const string ExceptionalCircumstanceSelectedPupilGroup = "Selected Pupil";
        public static readonly int Timeout = 60;
    }

    public struct AttendancePatternGroup
    {
        public const string AttendancePatternTests = "AttendancePattern";
        public const string AttendancePatternPermissionTests = "AttendancePatternPermissions";
    }
}


