using System;
using System.Collections.Generic;
using System.Text;

namespace MUNity.Schema.Conference
{
    public class CreateTeamRoleRequest
    {
        public int RoleGroupId { get; set; }

        public string RoleName { get; set; }

        public string RoleFullName { get; set; }

        public string RoleShort { get; set; }

        public int ParentRoleId { get; set; } = -1;
    }

    public class CreateTeamRoleResponse : AbstractResponse
    {

        public int RoleId { get; set; }
    }
}
