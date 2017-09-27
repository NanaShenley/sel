using Pupil.Components.BulkUpdate.Consents;
using Pupil.Components.Common;

namespace Pupil.Components.BulkUpdate
{
    public static class BulkUpdateMenuLinks
    {
        public static class Consents 
        {
            public static PupilConsentsSearch Search
            {
                get
                {
                    new PupilBulkUpdateNavigation().NavgateToPupilBulkUpdate_SubMenu(PupilBulkUpdateElements.BulkUpdate.MenuItems.PupilConsentsMenuItem);
                    return new PupilConsentsSearch();
                }
            }
        }
    }
}
