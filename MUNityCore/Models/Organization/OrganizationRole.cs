using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace MUNityCore.Models.Organization
{

    public class OrganizationRole
    {
        public int OrganizationRoleId { get; set; }

        [MaxLength(150)]
        public string RoleName { get; set; }

        public Organization Organization { get; set; }

        public List<OrganizationMember> MembersWithRole { get; set; }

        public bool CanCreateProject { get; set; }
    }
}
