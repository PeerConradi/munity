using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MUNityAngular.Models.Conference.Roles
{

    [DataContract]
    public class TeamRole : AbstractRole
    {

        [IgnoreDataMember]
        [Newtonsoft.Json.JsonIgnore]
        [JsonIgnore]
        public TeamRole ParentTeamRole { get; set; }

        [DataMember]
        public int TeamRoleLevel { get; set; }

        [DataMember]
        public override bool AllowMultipleParticipations => true;

        [DataMember]
        [NotMapped]
        public int ParentTeamRoleId => ParentTeamRole?.RoleId ?? -1;

        [DataMember]
        [NotMapped] 
        public int TeamRoleGroupId => TeamRoleGroup?.TeamRoleGroupId ?? -1;

        [JsonIgnore]
        [Newtonsoft.Json.JsonIgnore]
        [IgnoreDataMember]
        public TeamRoleGroup TeamRoleGroup { get; set; }
    }
}
