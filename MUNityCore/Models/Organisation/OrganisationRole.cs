using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace MUNityCore.Models.Organisation
{

    [DataContract]
    public class OrganisationRole
    {
        public int OrganisationRoleId { get; set; }

        [MaxLength(150)]
        public string RoleName { get; set; }

        [IgnoreDataMember]
        public Organisation Organisation { get; set; }

        public List<OrganisationMember> MembersWithRole { get; set; }

        [NotMapped] public string OrganisationId => Organisation?.OrganisationId ?? "";
        
        public bool CanCreateProject { get; set; }
    }
}
