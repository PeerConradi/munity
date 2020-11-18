using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using MongoDB.Bson;
using System.Text.Json.Serialization;

namespace MUNityCore.Models.Conference.Roles
{

    /// <summary>
    /// A Delegate Role is an attendee role that everyone can have that is part of a delegation
    /// and represents some sort of state/country at the conference. The delegation can be
    /// the same country/state but is not necessarily. You can also create multiply Delegation Roles
    /// of different states and put them together into a Delegation named Presidents of Europe.
    ///
    /// Every Delegate Role takes a seat inside a Committee
    /// </summary>
    [DataContract]
    public class DelegateRole : AbstractRole
    {

        /// <summary>
        /// The Country that this Delegate Role represents.
        /// </summary>
        [JsonIgnore]
        [Newtonsoft.Json.JsonIgnore]
        [IgnoreDataMember]
        public Country DelegateState { get; set; }

        /// <summary>
        /// Is this Role a Leader Role of the Delegation
        /// </summary>
        [DataMember]
        public bool IsDelegationLeader { get; set; }

        /// <summary>
        /// The Title of this Delegation Role. This is not the Role Name itself
        /// The RoleName can be something like: Delegate and the Title is Delegate of Germany in the Assembly General
        /// </summary>
        [DataMember]
        [MaxLength(200)]
        public string Title { get; set; }

        /// <summary>
        /// A Delegation that this Role is Part for example the Delegation of Germany.
        /// </summary>
        [JsonIgnore]
        [Newtonsoft.Json.JsonIgnore]
        [IgnoreDataMember]
        public Delegation Delegation { get; set; }

        /// <summary>
        /// The committee that this Role is seated in.
        /// </summary>
        [JsonIgnore]
        [Newtonsoft.Json.JsonIgnore]
        [IgnoreDataMember]
        public Committee Committee { get; set; }

        /// <summary>
        /// DelegationId for the Json Data Package
        /// </summary>
        [DataMember]
        [NotMapped]
        public string DelegationId => Delegation?.DelegationId ?? "";

        /// <summary>
        /// CommitteeId for the Json Data Package
        /// </summary>
        [DataMember]
        [NotMapped]
        public string CommitteeId => Committee?.CommitteeId ?? "";

        /// <summary>
        /// DelegateStateId for the Json Data Package
        /// </summary>
        [DataMember]
        [NotMapped]
        public int DelegateStateId => DelegateState?.CountryId ?? -1;


    }
}
