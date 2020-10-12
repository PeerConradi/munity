using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MUNityAngular.Models.Conference.Roles
{
    public class TeamRole : AbstractRole
    {
        [Newtonsoft.Json.JsonIgnore]
        [JsonIgnore]
        public TeamRole ParentTeamRole { get; set; }

        public int TeamRoleLevel { get; set; }

        public override bool AllowMultipleParticipations => true;

        [NotMapped]
        public int ParentTeamRoleId => ParentTeamRole?.RoleId ?? -1;

        [NotMapped] public int TeamRoleGroupId => TeamRoleGroup?.TeamRoleGroupId ?? -1;

        [JsonIgnore]
        [Newtonsoft.Json.JsonIgnore]
        public TeamRoleGroup TeamRoleGroup { get; set; }
    }
}
