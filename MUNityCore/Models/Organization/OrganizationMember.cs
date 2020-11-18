using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace MUNityCore.Models.Organization
{

    /// <summary>
    /// The membership of a registered user inside an organization that is hosting model united nations conferences.
    /// </summary>
    [DataContract]
    public class OrganizationMember
    {
        [DataMember]
        public int OrganizationMemberId { get; set; }

        [IgnoreDataMember]
        public Core.User User { get; set; }

        [DataMember]
        [NotMapped]
        public string Username => User?.Username ?? "";

        [IgnoreDataMember]
        public Organization Organization { get; set; }

        [DataMember]
        [NotMapped]
        public string OrganizationId => Organization?.OrganizationId ?? "";

        [IgnoreDataMember]
        public OrganizationRole Role { get; set; }

        [DataMember]
        [NotMapped]
        public int RoleId => Role?.OrganizationRoleId ?? -1;
    }
}
