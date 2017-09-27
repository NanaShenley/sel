using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pupil.Data.Entities;
using SeSugar.Data;

namespace Pupil.Data
{
    public class ClassLogData
    {
        private const string SqlDateFormat = "yyyyMMdd";
        public static ClassTeacher GetClassTeacherWithClassAndLearners()
        {
            int tenantId = SeSugar.Environment.Settings.TenantId;
            var sqlEffectiveDate = DateTime.Now.ToString(SqlDateFormat);

            string sql = String.Format(
                "SELECT pc.[Staff], st.LegalForename, st.LegalSurname, pc.PrimaryClass, c.FullName as ClassName"+
                "  FROM PrimaryClassTeacher pc inner join Staff st on pc.Staff = st.Id " +
                "       inner join PrimaryClass c on pc.PrimaryClass = c.ID " +
                "  where StartDate < GETDATE()  and(EndDate is null or EndDate > GETDATE()) " +
                "    and exists(Select 1 from LearnerPrimaryClassMembership lpc             "+
                "                where lpc.PrimaryClass = pc.PrimaryClass "+
                "                  and StartDate < GETDATE() "+                  
                "                  and(EndDate is null or EndDate > GETDATE())    "+
                "               group by lpc.PrimaryClass                 "+
                "                 having count(*) > 0) " +
                "    and pc.TenantID = " + tenantId);

            var classTeacher = DataAccessHelpers.GetEntities<ClassTeacher>(sql).FirstOrDefault();

            if (classTeacher == null) throw new NullReferenceException(" Could not retreive class and staff details");

            return classTeacher;
        }

        public static AuthorisedUser GetAuthorisedUserDetailsForClassTeacherUser(string teacherUserName)
        {
            int tenantId = SeSugar.Environment.Settings.TenantId;

            string sql = String.Format(
                "Select UserName, InstanceID, UserType " +
                "  from app.AuthorisedUser " +
                " where UserName = '" + teacherUserName + "'" +
                "   and TenantID = " + tenantId);

            var authorisedUser = DataAccessHelpers.GetEntities<AuthorisedUser>(sql).FirstOrDefault();

            return authorisedUser;
        }

        public static void UpdateClassTeacherUserWithStaffDetails(ClassTeacher classTeacher, string teacherUserName)
        {
            int tenantId = SeSugar.Environment.Settings.TenantId;

            string sql = String.Format(
                " Select ID " +
                "  from app.UserType " +
                " where code = 'Staff' ");

            var staffUserType = DataAccessHelpers.GetEntities<Guid>(sql).FirstOrDefault();

            sql = String.Format(
                "Update app.AuthorisedUser " +
                "   set InstanceId = '" + classTeacher.Staff + "' " +
                "     , UserType = '" + staffUserType + "' " +
                " where username = '" + teacherUserName + "'");

            DataAccessHelpers.Execute(sql);
        }

        public static void UpdateClassTeacherWithInitialAuthUserValues(AuthorisedUser authUser, string teacherUserName)
        {
            string setInstanceId, userType;
            if (authUser.InstanceID == null)
                setInstanceId = "set InstanceId = null";
            else
                setInstanceId = "set InstanceId ='" + authUser.InstanceID + "'";

            if (authUser.UserType == null)
                userType = "UserType = null";
            else
                userType = "UserType ='" + authUser.UserType + "'";


            string sql = String.Format(
               "Update app.AuthorisedUser " +
               setInstanceId + ", " +
               userType +
               " where username = '" + teacherUserName + "'");

            DataAccessHelpers.Execute(sql);
        }
    }
}
