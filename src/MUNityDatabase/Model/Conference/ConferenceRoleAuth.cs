using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MUNity.Database.Models.Conference
{

    /// <summary>
    /// The authorizations of a role inside a conference.
    /// </summary>
    public class ConferenceRoleAuth
    {
        [Key]
        public int ConferenceRoleAuthId { get; set; }

        [MaxLength(150)]
        public string RoleAuthName { get; set; }

        /// <summary>
        /// The Pwer Level gives a basic idea of what power the
        /// user should have. This can also be used when working
        /// with the API outside of this context. For example
        /// another application that wants to use this framework/API
        /// </summary>
        public int PowerLevel { get; set; }

        /// <summary>
        /// Every Role auth is linked to a conference, to protect other Conferences
        /// from changing them but also be able to reuse the same role inside of
        /// the some conference again. This will create a lot of duplicate data
        /// because a lot of conferences will share the same structure but that's
        /// not a problem
        /// </summary>
        public Conference Conference { get; set; }

        /// <summary>
        /// Can change the Settings of the conference for example the date, name etc.
        /// It also allows to change the structure of the conference like Committees,
        /// delegations, roles etc.
        /// </summary>
        public bool CanEditConferenceSettings { get; set; }

        /// <summary>
        /// The user can see all applications for roles
        /// </summary>
        public bool CanSeeApplications { get; set; }

        /// <summary>
        /// The user can change participations for example accept or deny
        /// an application for a role, or set someone into the team etc.
        /// </summary>
        public bool CanEditParticipations { get; set; }


    }
}
