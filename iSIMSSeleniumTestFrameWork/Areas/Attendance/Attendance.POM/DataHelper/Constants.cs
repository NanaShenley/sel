using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.POM.DataHelper
{
    public static class Constants
    {
        public const string SqlDateFormat = "yyyyMMdd";
        public struct Tables
        {
            public const string Learner = "Learner";
            public const string Application = "Application";
            public const string ApplicationStatusLog = "ApplicationStatusLog";
            public const string SchoolIntake = "SchoolIntake";
            public const string AdmissionTerm = "AdmissionTerm";
            public const string ReferencedAcademicYear = "ReferenceAcademicYear";
            public const string AdmissionGroup = "AdmissionGroup";
            public const string LearnerContact = "LearnerContact";
            public const string Gender = "Gender";
            public const string Title = "Title";
            public const string Occupation = "Occupation";
            public const string LearnerContactRelationship = "LearnerContactRelationship";
            public const string LearnerContactRelationshipType = "LearnerContactRelationshipType";
            public const string LearnerContactAddress = "LearnerContactAddress";
            public const string LearnerAddress = "LearnerAddress";
            public const string AddressType = "AddressType";
            public const string ApplicationStatus = "ApplicationStatus";
            public const string EnrolmentStatus = "EnrolmentStatus";
            public const string Address = "Address";
            public const string YearGroup = "YearGroup";
            public const string SchoolNCYear = "SchoolNCYear";
            public const string YearGroupSetMembership = "YearGroupSetMembership";
        }
    }
}
