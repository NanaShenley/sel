using System;

namespace Pupil.Data.Entities
{
    public class YearGroup
    {
        public Guid? ID { get; set; }
        public Guid? SchoolNCYear { get; set; }
        public string ShortName { get; set; }
        public string FullName { get; set; }
    }
}
