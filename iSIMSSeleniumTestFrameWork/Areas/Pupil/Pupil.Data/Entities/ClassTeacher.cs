using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pupil.Data.Entities
{
    public class ClassTeacher
    {
        public Guid? Staff { get; set; }
        public string LegalForename { get; set; }
        public string LegalSurname { get; set; }
        public Guid PrimaryClass { get; set; }
        public string ClassName { get; set; }
    }
}
