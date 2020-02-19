using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MUNityAngular.Models.Conference
{
    public class AddTeamMemberModel
    {
        public string Username { get; set; }

        public TeamRoleModel Role { get; set; }
    }
}
