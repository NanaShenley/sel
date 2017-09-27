using System;
using SeSugar.Data;

namespace Pupil.Data
{
    public static class PurgeLinkedData
    {
        public static void DeletePupilLogNotesForLearner(Guid learnerId)
        {
            var sql = string.Format(
              "DELETE FROM dbo.PupilLogNoteStandard " +
               "where Learner = '" + learnerId + "'");
            DataAccessHelpers.Execute(sql);
        }

        public static void DeletePupil(string forename, string surname)
        {
            string sql = String.Format(
                "DELETE FROM dbo.Learner " +
                "where LegalForename = '" + forename + "'" +
                " and LegalSurname = '" + surname + "'");

            DataAccessHelpers.Execute(sql);
        }

        public static void DeletePrimaryClassMembershipForLearner(Guid learnerId)
        {
            string sql = String.Format(
                "DELETE FROM dbo.LearnerPrimaryClassMembership " +
                "where Learner = '" + learnerId + "'");
            DataAccessHelpers.Execute(sql);
        }

        public static void DeleteYearGroupMembershipForPupil(Guid learnerId)
        {
            string sql = String.Format(
              "DELETE FROM dbo.LearnerYearGroupMembership " +
               "where Learner = '" + learnerId + "'");
            DataAccessHelpers.Execute(sql);
        }

        public static void DeleteAttendanceForLearner(Guid learnerId)
        {
            string sql = String.Format(
             "DELETE FROM dbo.AttendanceRecordHistory " +
             "Where AttendanceRecord in " +
             "(select AttendanceRecord.ID from AttendanceRecord " +
             " where learner = '" + learnerId + "')");
            DataAccessHelpers.Execute(sql);

            sql = String.Format(
              "DELETE FROM dbo.AttendanceRecord " +
               "where Learner = '" + learnerId + "'");
            DataAccessHelpers.Execute(sql);
        }

        public static void DeleteAttendanceModelForLearner(Guid learnerId)
        {
            string sql = String.Format(
                "DELETE FROM dbo.LearnerAttendanceMode " +
                 "where Learner = '" + learnerId + "'");

            DataAccessHelpers.Execute(sql);
        }

        public static void DeleteBorderStatusForLearner(Guid learnerId)
        {
            string sql = String.Format(
                "DELETE FROM dbo.LearnerBoarderStatus " +
                 "where Learner = '" + learnerId + "'");

            DataAccessHelpers.Execute(sql);
        }

        public static void DeleteSuspensionForLearner(Guid learnerId)
        {
            string sql = String.Format(
             "DELETE FROM dbo.ExclusionMeeting " +
             "Where LearnerExclusion in " +
             "(select LearnerExclusion.ID from LearnerExclusion " +
             " where learner = '" + learnerId + "')");
            DataAccessHelpers.Execute(sql);

            sql = String.Format(
             "DELETE FROM dbo.LearnerExclusionStatus " +
             "Where Exclusion in " +
             "(select LearnerExclusion.ID from LearnerExclusion " +
             " where learner = '" + learnerId + "')");
            DataAccessHelpers.Execute(sql);

            sql = String.Format(
            "DELETE FROM dbo.ExclusionNote " +
            "Where LearnerExclusion in " +
            "(select LearnerExclusion.ID from LearnerExclusion " +
            " where learner = '" + learnerId + "')");
            DataAccessHelpers.Execute(sql);


            sql = String.Format(
              "DELETE FROM dbo.LearnerExclusion " +
               "where Learner = '" + learnerId + "'");
            DataAccessHelpers.Execute(sql);
        }
        public static void DeleteLearnerEnrolmentForPupil(Guid learnerId)
        {
            string sql = String.Format(
              "DELETE FROM dbo.LearnerEnrolment " +
               "where Learner = '" + learnerId + "'");
            DataAccessHelpers.Execute(sql);
        }

        public static void DeleteLearnerEnrolmentStatusForPupil(Guid learnerId)
        {
            string sql = String.Format(
                "Delete es " +
                " from dbo.LearnerEnrolmentStatus es inner join dbo.LearnerEnrolment le " +
                "  on es.LearnerEnrolment = le.ID " +
                "and le.Learner = '" + learnerId + "'");
            DataAccessHelpers.Execute(sql);
        }

        public static void DeleteNcYearMembershipForPupil(Guid learnerId)
        {
            string sql = String.Format(
                "DELETE FROM dbo.LearnerNCYearMembership " +
                 "where Learner = '" + learnerId + "'");

            DataAccessHelpers.Execute(sql);
        }

        public static void DeleteLearnerPreviousSchool(Guid learnerId)
        {
            var sql = string.Format(
                 @" BEGIN 
                  DECLARE @tmpPreviousSchoolToRemove Table (ID uniqueidentifier) 
                  INSERT INTO @tmpPreviousSchoolToRemove (ID) 
                  SELECT [ID] 
                  FROM  [dbo].[LearnerPreviousSchool] 
                  WHERE [Learner] = '{0}' 
                  DELETE [LearnerPreviousSchoolAttendanceSummary] WHERE [LearnerPreviousSchool] in (SELECT * FROM @tmpPreviousSchoolToRemove) 
                  DELETE [LearnerPreviousSchool] where Learner = '{0}' 
                  END "
                , learnerId);

            DataAccessHelpers.Execute(sql);
        }

        public static void DeleteSENStageForLearner(Guid learnerId)
        {
            var sql = string.Format(
                "DELETE FROM dbo.LearnerSENStatus " +
                "WHERE Learner = '{0}'"
                , learnerId);

            DataAccessHelpers.Execute(sql);
        }

        public static void DeleteSENNeedForLearner(Guid learnerId)
        {
            var sql = string.Format(
                "DELETE FROM dbo.LearnerSENNeedType " +
                "WHERE Learner = '{0}'"
                , learnerId);

            DataAccessHelpers.Execute(sql);
        }

        public static void DeleteSENProvisionForLearner(Guid learnerId)
        {
            var sql = string.Format(
                "DELETE FROM dbo.LearnerSENProvisionType " +
                "WHERE Learner = '{0}'"
                , learnerId);

            DataAccessHelpers.Execute(sql);
        }

        /// <exception cref="TargetException">This member was invoked with a late-binding mechanism.</exception>
        public static void DeleteSENReviewForLearner(Guid learnerId)
        {
            var sql = string.Format(
                "DELETE FROM dbo.LearnerSENReview " +
                "WHERE Learner = '{0}'"
                , learnerId);

            var methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
            Console.WriteLine("{0}: {1} Rows affected.", methodName, DataAccessHelpers.Execute(sql));
        }

        /// <exception cref="TargetException">This member was invoked with a late-binding mechanism.</exception>
        public static void DeleteSENReviewParticipantForLearner(Guid learnerId)
        {
            var sql = string.Format(
                "DELETE FROM dbo.SENReviewParticipant " +
                "WHERE LearnerReviewID IN (SELECT ID FROM dbo.LearnerSENReview WHERE Learner = '{0}')"
                , learnerId);

            var methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
            Console.WriteLine("{0}: {1} Rows affected.", methodName, DataAccessHelpers.Execute(sql));
        }

        /// <exception cref="TargetException">This member was invoked with a late-binding mechanism.</exception>
        public static void DeleteSENStatementForLearner(Guid learnerId)
        {
            var sql = string.Format(
                "DELETE FROM dbo.SENStatement " +
                "WHERE Learner = '{0}'"
                , learnerId);

            var methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
            Console.WriteLine("{0}: {1} Rows affected.", methodName, DataAccessHelpers.Execute(sql));
        }

        public static void DeleteEhcpForLearner(Guid learnerId)
        {
            var sql = string.Format(
                "DELETE FROM dbo.EHCP " +
                "WHERE Learner = '{0}'"
                , learnerId);

            var methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
            Console.WriteLine("{0}: {1} Rows affected.", methodName, DataAccessHelpers.Execute(sql));
        }


        public static void DeleteSenNeedType(string code)
        {
            var sql = string.Format("DELETE FROM dbo.SENNeedType WHERE Code='{0}'", code);
            DataAccessHelpers.Execute(sql);
        }

        public static void DeleteSenProvisionType(string code)
        {
            var sql = string.Format("DELETE FROM dbo.SENProvisionType WHERE Code='{0}'", code);
            DataAccessHelpers.Execute(sql);
        }

        public static void DeleteSenStatus(string code)
        {
            var sql = string.Format("DELETE FROM dbo.SENStatus WHERE Code='{0}'", code);
            DataAccessHelpers.Execute(sql);
        }

        public static void DeletePupilAddress(Guid learnerIdSetup)
        {
            var sql = string.Format(
                 " BEGIN " +
                 " DECLARE @tmpAddressToRemove Table (ID uniqueidentifier) " +
                 " INSERT INTO @tmpAddressToRemove (ID) " +
                 " SELECT [Address] " +
                 " FROM  [dbo].[LearnerAddress] " +
                 " WHERE [Learner] = '{0}' " +
                 " SELECT * FROM @tmpAddressToRemove " +
                 " DELETE [LearnerAddress] WHERE [Learner] = '{0}' " +
                 " DELETE [Address] where ID in (SELECT * FROM @tmpAddressToRemove) " +
                 " END "
                , learnerIdSetup);

            DataAccessHelpers.Execute(sql);
        }

        public static void DeleleLearnerTel(Guid learnerId)
        {
            var sql = string.Format(
                "DELETE FROM dbo.LearnerTelephone where learner = '{0}'"
                , learnerId);
            DataAccessHelpers.Execute(sql);
        }

        public static void DeleleLearnerEmail(Guid learnerId)
        {
            var sql = string.Format(
                "DELETE FROM dbo.LearnerEmail where learner = '{0}'"
                , learnerId);
            DataAccessHelpers.Execute(sql);
        }

        public static void DeleleLearnerPreviousName(Guid learnerId)
        {
            var sql = string.Format(
                "DELETE FROM dbo.LearnerPreviousName where learner = '{0}'"
                , learnerId);
            DataAccessHelpers.Execute(sql);
        }

        public static void DeleleLearnerAddress(Guid learnerId)
        {
            var sql = string.Format(
                "DELETE FROM dbo.LearnerAddress where learner = '{0}'"
                , learnerId);
            DataAccessHelpers.Execute(sql);
        }


        public static void DeleteLearnerContactRelationship(Guid learnerId)
        {
            var sql = String.Format(
                "DELETE from LearnerContactRelationship where learner = '{0}'"
                , learnerId);
            DataAccessHelpers.Execute(sql);
        }

        public static void DeleteLearnerConsents(Guid learnerId)
        {
            var sql = String.Format(
                "DELETE from [dbo].[LearnerConsentType] where learner = '{0}'"
                , learnerId);
            DataAccessHelpers.Execute(sql);
        }

        public static void DeleteLearnerSenData(Guid learnerId)
        {
            var sql = String.Format(
                "BEGIN" +
                " DECLARE @tmpIDToRemove Table (ID uniqueidentifier) " +
                " INSERT INTO @tmpIDToRemove (ID)" +
                " SELECT id" +
                "   FROM [dbo].[LearnerSENNeedType] " +
                "  WHERE learner = '{0}' " +

                " DELETE from [dbo].[LearnerSENNeedRank] " +
                "  where LearnerSENNeedType in (SELECT ID FROM @tmpIDToRemove) " +

                " DELETE from [dbo].[LearnerSENNeedType] where learner = '{0}' " +
                " DELETE from [dbo].[LearnerSENProvisionType] where learner = '{0}' " +
                " DELETE from [dbo].[LearnerSENStatus] where learner = '{0}' " +
                "END"
                , learnerId);
            DataAccessHelpers.Execute(sql);
        }

        public static void DeleteLearnerInCareDetails(Guid learnerId)
        {
            var sql = String.Format(
                @"DECLARE @inCareId Table (ID uniqueidentifier) 
                    INSERT INTO @inCareId (ID)
                    SELECT ID FROM [dbo].[LearnerInCareDetail] WHERE Learner = '{0}'
                DECLARE @pepId Table (ID uniqueidentifier) 
                    INSERT INTO @pepId (ID)
                    SELECT ID FROM [dbo].[PersonalEducationPlan] WHERE LearnerCareDetail in (SELECT ID FROM @inCareId)
                DELETE from [dbo].[PersonalEducationPlanContributor] where PersonalEducationPlan in (SELECT ID FROM @pepId)
                DELETE from [dbo].[PersonalEducationPlan] where LearnerCareDetail in (SELECT ID FROM @inCareId)
                DELETE from [dbo].[LearnerInCareDetail] where Learner='{0}'"
                , learnerId);
            DataAccessHelpers.Execute(sql);
        }

        public static void DeleteApplicationStatusLog(Guid applicationId)
        {
            string sql = String.Format(
                "DELETE FROM dbo.ApplicationStatusLog " +
                "WHERE Application = '" + applicationId + "'");

            DataAccessHelpers.Execute(sql);
        }

        public static void DeleteApplication(Guid applicationId)
        {
            string sql = String.Format(
                "DELETE FROM dbo.Application " +
                "WHERE ID = '" + applicationId + "'");

            DataAccessHelpers.Execute(sql);
        }

        public static void DeleteLearnerConsentType(Guid learnerId)
        {
            string sql = String.Format("DELETE FROM dbo.LearnerConsentType WHERE Learner = '{0}' ", learnerId.ToString().ToUpper());
            DataAccessHelpers.Execute(sql);
        }
        public static void DeleteApplicationStatus(string code)
        {
            var sql = string.Format("DELETE FROM dbo.ApplicationStatus WHERE Code='{0}'", code);
            DataAccessHelpers.Execute(sql);
        }

        public static void DeleteAppealResult(string code)
        {
            var sql = string.Format("DELETE FROM dbo.AdmissionsAppealHearingOutcome WHERE Code='{0}'", code);
            DataAccessHelpers.Execute(sql);
        }

        public static void DeleteAppealOutcome(string code)
        {
            var sql = string.Format("DELETE FROM dbo.AdmissionsAppealStatus WHERE Code='{0}'", code);
            DataAccessHelpers.Execute(sql);
        }
        public static void DeleteReasonAdmissionRejected(string code)
        {
            var sql = string.Format("DELETE FROM dbo.ApplicationRejectionReason WHERE Code='{0}'", code);
            DataAccessHelpers.Execute(sql);
        }

        public static void DeleteBehaviourEventForLearner(Guid learnerId)
        {
            var sql = string.Format(@"DECLARE @events TABLE(Id uniqueidentifier)
                        INSERT INTO @events SELECT BehaviourEvent FROM dbo.LearnerBehaviourEvent WHERE Learner='{0}'
                        DELETE bef FROM dbo.BehaviourEventFollowUp bef inner join dbo.LearnerBehaviourEvent lbe
		                                        on bef.LearnerBehaviourEvent = lbe.ID
		                                      AND lbe.BehaviourEvent IN (SELECT Id FROM @events)
                        DELETE FROM dbo.LearnerBehaviourEvent WHERE BehaviourEvent IN (SELECT Id FROM @events)
                        DELETE FROM dbo.BehaviourEventDocument WHERE BehaviourEvent IN (SELECT Id FROM @events)
                        DELETE FROM dbo.BehaviourEventStaffInvolved WHERE BehaviourEvent IN (SELECT Id FROM @events)
                        DELETE FROM dbo.BehaviourEvent WHERE ID IN (SELECT Id FROM @events)", learnerId);

            DataAccessHelpers.Execute(sql);
        }

        public static void DeleteAchievementEventForLearner(Guid learnerId)
        {
            var sql = string.Format(@"DECLARE @events TABLE(Id uniqueidentifier)
                        INSERT INTO @events SELECT AchievementEvent FROM dbo.LearnerAchievementEvent WHERE Learner='{0}'
                        DELETE FROM dbo.LearnerAchievementEvent WHERE AchievementEvent IN (SELECT Id FROM @events)
                        DELETE FROM dbo.AchievementEventDocument WHERE AchievementEvent IN (SELECT Id FROM @events)
                        DELETE FROM dbo.AchievementEvent WHERE ID IN (SELECT Id FROM @events)", learnerId);

            DataAccessHelpers.Execute(sql);
        }

        #region Report Cards

        public static void DeleteReportCardsForLearner(Guid learnerId)
        {
            string sql = String.Format(
                "DELETE FROM dbo.ConductReportCard " +
                "Where Learner ='" + learnerId + "'");
            DataAccessHelpers.Execute(sql);
            
        }

        public static void DeleteReportCard(Guid reportCardId)
        {
            string sql = String.Format(
                "DELETE FROM dbo.ConductReportCardTarget " +
                "Where ReportCard = '" + reportCardId + "'");
            DataAccessHelpers.Execute(sql);

            sql = String.Format(
                "DELETE FROM dbo.ConductReportCard " +
                "Where ID = '" + reportCardId + "'");
            DataAccessHelpers.Execute(sql);
        }

        public static void DeleteDoNotDiscloseFlagForLearner(Guid learnerId)
        {
            string sql = String.Format(
                "DELETE FROM dbo.LearnerInfoDomainRestriction where RootID = '" + learnerId + "'");
            DataAccessHelpers.Execute(sql);
        }

        #endregion

    }
}
