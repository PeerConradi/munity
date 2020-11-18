using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace MUNityCore.Models.Organization
{

    [DataContract]
    public class OrganizationRole
    {
        public int OrganizationRoleId { get; set; }

        [MaxLength(150)]
        public string RoleName { get; set; }

        [IgnoreDataMember]
        public Organization Organization { get; set; }

        public List<OrganizationMember> MembersWithRole { get; set; }

        [NotMapped] public string OrganizationId => Organization?.OrganizationId ?? "";
        
        public bool CanCreateProject { get; set; }
    }
}
