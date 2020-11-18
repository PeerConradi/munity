using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MUNityCore.Models.Conference.Roles
{

    /// <summary>
    /// A team role is any role for organization purposes. Note that anyone that is assigned
    /// this type of role will have access to the inside area of the conference.
    /// </summary>
    [DataContract]
    public class TeamRole : AbstractRole
    {

        /// <summary>
        /// A parent role that is above this role.
        /// </summary>
        [IgnoreDataMember]
        [Newtonsoft.Json.JsonIgnore]
        [JsonIgnore]
        public TeamRole ParentTeamRole { get; set; }

        /// <summary>
        /// The Level of this row. This is not assigned from the amount of roles above this roles.
        /// You can create different custom PowerLevels for team roles to allow different types of access
        /// to the role. With 1 as the highest rank, 2 as the next powerful rank and so on.
        /// </summary>
        [DataMember]
        public int TeamRoleLevel { get; set; }

        /// <summary>
        /// The parent role id for the json package
        /// </summary>
        [DataMember]
        [NotMapped]
        public int ParentTeamRoleId => ParentTeamRole?.RoleId ?? -1;

        /// <summary>
        /// The team role groupId for the json package
        /// </summary>
        [DataMember]
        [NotMapped] 
        public int TeamRoleGroupId => TeamRoleGroup?.TeamRoleGroupId ?? -1;

        /// <summary>
        /// Every role is a part of a group, for example could there be different roles for
        /// substantive planing, framework planing, team coordination, equipment coordinator etc.
        /// and all of this role could share a group of: "head of organization".
        /// </summary>
        [JsonIgnore]
        [Newtonsoft.Json.JsonIgnore]
        [IgnoreDataMember]
        public TeamRoleGroup TeamRoleGroup { get; set; }

        public TeamRole()
        {
            this.AllowMultipleParticipations = true;
        }
    }
}
