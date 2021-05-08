using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MUNityCore.Dtos.Users
{
    public class UserWithRolesDto
    {
        public string UserId { get; set; }

        public string Username { get; set; }

        public List<MunityRoleDto> Roles { get; set; }
    }

    public class MunityRoleDto
    {
        public string RoleId { get; set; }

        public string Name { get; set; }
    }
}
