using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace MUNityAngular.Models.Organisation
{

    [DataContract]
    public class OrganisationMember
    {
        [DataMember]
        public int OrganisationMemberId { get; set; }

        [IgnoreDataMember]
        public Core.User User { get; set; }

        [DataMember]
        [NotMapped]
        public string Username => User?.Username ?? "";

        [IgnoreDataMember]
        public Organisation Organisation { get; set; }

        [DataMember]
        [NotMapped]
        public string OrganisationId => Organisation?.OrganisationId ?? "";

        [IgnoreDataMember]
        public OrganisationRole Role { get; set; }

        [DataMember]
        [NotMapped]
        public int RoleId => Role?.OrganisationRoleId ?? -1;
    }
}
