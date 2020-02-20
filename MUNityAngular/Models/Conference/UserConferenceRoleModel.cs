using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MUNityAngular.Models.Conference
{
    public class UserConferenceRoleModel
    {
        public User.UserModel User { get; set; }

        public TeamRoleModel Role { get; set; }
    }
}
