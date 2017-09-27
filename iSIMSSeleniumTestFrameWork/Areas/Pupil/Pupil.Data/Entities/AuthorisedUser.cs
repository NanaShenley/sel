using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pupil.Data.Entities
{
    public class AuthorisedUser
    {
        public string UserName { get; set; }
        public Guid? InstanceID { get; set; }
        public Guid? UserType { get; set; }
    }
}
