using MUNity.Database.Models.General;
using MUNity.Database.Models.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MUNity.Database.Models.User
{
    public class UserInSchool
    {
        public long UserInSchoolId { get; set; }

        public MunityUser User { get; set; }

        public School School { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime? UntilDate { get; set; }
    }
}
