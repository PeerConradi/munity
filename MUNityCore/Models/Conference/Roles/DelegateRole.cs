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

    [DataContract]
    public class DelegateRole : AbstractRole
    {
        [JsonIgnore]
        [Newtonsoft.Json.JsonIgnore]
        [IgnoreDataMember]
        public Country DelegateState { get; set; }

        [DataMember]
        public bool IsDelegationLeader { get; set; }

        [DataMember]
        [MaxLength(200)]
        public string Title { get; set; }

        [JsonIgnore]
        [Newtonsoft.Json.JsonIgnore]
        [IgnoreDataMember]
        public Delegation Delegation { get; set; }

        [JsonIgnore]
        [Newtonsoft.Json.JsonIgnore]
        [IgnoreDataMember]
        public Committee Committee { get; set; }

        [DataMember]
        [NotMapped]
        public string DelegationId => Delegation?.DelegationId ?? "";

        [DataMember]
        [NotMapped]
        public string CommitteeId => Committee?.CommitteeId ?? "";

        [DataMember]
        [NotMapped]
        public int DelegateStateId => DelegateState?.CountryId ?? -1;


    }
}
