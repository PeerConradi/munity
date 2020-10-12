using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using System.Text.Json.Serialization;

namespace MUNityAngular.Models.Conference.Roles
{
    public class DelegateRole : AbstractRole
    {
        [JsonIgnore]
        [Newtonsoft.Json.JsonIgnore]
        public Country DelegateState { get; set; }

        public bool IsDelegationLeader { get; set; }

        [Column(TypeName = "varchar(200)")]
        public string Title { get; set; }

        [JsonIgnore]
        [Newtonsoft.Json.JsonIgnore]
        public Delegation Delegation { get; set; }

        [JsonIgnore]
        [Newtonsoft.Json.JsonIgnore]
        public Committee Committee { get; set; }

        [NotMapped]
        public string DelegationId => Delegation?.DelegationId ?? "";

        [NotMapped]
        public string CommitteeId => Committee?.CommitteeId ?? "";

        [NotMapped]
        public int DelegateStateId => DelegateState?.CountryId ?? -1;


    }
}
