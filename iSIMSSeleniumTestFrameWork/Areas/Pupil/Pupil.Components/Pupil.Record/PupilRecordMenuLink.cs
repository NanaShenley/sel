using Pupil.Components.Common;
using SharedComponents.Helpers;

namespace Pupil.Components.PupilRecord
{
    public class PupilRecordMenuLink
    {
        public static PupilRecordSearch Search
        {
            get
            {
                new PupilRecordNavigation().NavigateToPupilRecordMenuPage();
                return new PupilRecordSearch();
            }
        }
    }
}