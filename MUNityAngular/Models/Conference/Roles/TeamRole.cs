using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MUNityAngular.Models.Conference.Roles
{
    public class TeamRole : AbstractRole
    {
        [JsonIgnore]
        public TeamRole ParentTeamRole { get; set; }

        public int TeamRoleGroup { get; set; }

        public override bool AllowMultipleParticipations => true;

        //NotMapped
        public int ParentTeamRoleId => ParentTeamRole?.RoleId ?? -1;
    }
}
