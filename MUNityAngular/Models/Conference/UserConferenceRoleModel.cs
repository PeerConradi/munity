using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MUNityAngular.Models.Conference
{

    [Obsolete("Use DataHandlers.EntityFramework.Models.TeamUser")]
    public class UserConferenceRoleModel
    {
        public User.UserModel User { get; set; }

        public TeamRoleModel Role { get; set; }
    }
}
