using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MUNity.Database.Models.Conference.Roles
{

    /// <summary>
    /// A team role is any role for organization purposes. Note that anyone that is assigned
    /// this type of role will have access to the inside area of the conference.
    /// </summary>
    public class ConferenceTeamRole : AbstractConferenceRole
    {

        /// <summary>
        /// A parent role that is above this role.
        /// </summary>
        public ConferenceTeamRole ParentTeamRole { get; set; }

        /// <summary>
        /// The Level of this row. This is not assigned from the amount of roles above this roles.
        /// You can create different custom PowerLevels for team roles to allow different types of access
        /// to the role. With 1 as the highest rank, 2 as the next powerful rank and so on.
        /// </summary>
        public int TeamRoleLevel { get; set; }


        /// <summary>
        /// Every role is a part of a group, for example could there be different roles for
        /// substantive planing, framework planing, team coordination, equipment coordinator etc.
        /// and all of this role could share a group of: "head of organization".
        /// </summary>
        public TeamRoleGroup TeamRoleGroup { get; set; }

        public ConferenceTeamRole()
        {
            this.AllowMultipleParticipations = true;
        }
    }
}
