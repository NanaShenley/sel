using Pupil.Components.Common;
using SharedComponents.Helpers;

namespace Pupil.Components
{
    public class PupilLogNavigation
    {
        public void NavigateToPupilLog_SearchPupil(SeleniumHelper.iSIMSUserType userType = SeleniumHelper.iSIMSUserType.ClassTeacher)
        {
            SeleniumHelper.Login(userType);
            SeleniumHelper.FindAndClick(SeleniumHelper.AutomationId(PupilElements.PupilLogQuickLink));
            SeleniumHelper.FindAndClick(PupilElements.PupilLog.SearchButton);
        }

        public void NavigateToPupilLog_PupilLogDetails(string learnerId, SeleniumHelper.iSIMSUserType userType = SeleniumHelper.iSIMSUserType.ClassTeacher)
        {
            NavigateToPupilLog_SearchPupil(userType);
            SeleniumHelper.FindAndClick(PupilElements.PupilLog.SearchResultFormat, TestSettings.TestDefaults.Default.Path, learnerId);            
        }       
    }
}

