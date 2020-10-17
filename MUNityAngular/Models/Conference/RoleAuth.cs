using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using MongoDB.Bson.IO;

namespace MUNityAngular.Models.Conference
{

    [DataContract]
    public class RoleAuth
    {
        [DataMember]
        public int RoleAuthId { get; set; }

        [MaxLength(150)]
        [DataMember]
        public string RoleAuthName { get; set; }

        /// <summary>
        /// The Pwer Level gives a basic idea of what power the
        /// user should have. This can also be used when working
        /// with the API outside of this context. For example
        /// another application that wants to use this framework/API
        /// </summary>
        [DataMember]
        public int PowerLevel { get; set; }

        /// <summary>
        /// Every Role auth is linked to a conference, to protect other Conferences
        /// from changing them but also be able to reuse the same role inside of
        /// the some conference again. This will create a lot of douplicate data
        /// because a lot of conferences will share the same structure but thats
        /// not a problem
        /// </summary>
        [JsonIgnore]
        [Newtonsoft.Json.JsonIgnore]
        [IgnoreDataMember]
        public Conference Conference { get; set; }

        [NotMapped]
        public string ConferenceId => Conference?.ConferenceId ?? "";

        /// <summary>
        /// Can change the Settings of the conference for exmaple the date, name etc.
        /// It also allows to change the structure of the conference like Committees,
        /// delegations, roles etc.
        /// </summary>
        [DataMember]
        public bool CanEditConferenceSettings { get; set; }

        /// <summary>
        /// The user can see all applications for roles
        /// </summary>
        [DataMember]
        public bool CanSeeApplications { get; set; }

        /// <summary>
        /// The user can change participations for example accept or deny
        /// an application for a role, or set someone into the team etc.
        /// </summary>
        [DataMember]
        public bool CanEditParticipations { get; set; }

        [Timestamp]
        [JsonIgnore]
        [IgnoreDataMember]
        [Newtonsoft.Json.JsonIgnore]
        public byte[] RoleAuthTimestamp { get; set; }
    }
}
