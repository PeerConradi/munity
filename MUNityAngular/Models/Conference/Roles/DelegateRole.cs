using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MUNityAngular.Models.Conference.Roles
{
    public class DelegateRole : AbstractRole
    {
        [JsonIgnore]
        public State DelegateState { get; set; }

        
        public bool IsDelegationLeader { get; set; }

        public string Title { get; set; }

        [JsonIgnore]
        public Delegation Delegation { get; set; }

        [JsonIgnore]
        public Committee Committee { get; set; }

        //[NotMapped]
        public string DelegationId => Delegation?.DelegationId ?? "";

        //[NotMapped]
        public string CommitteeId => Committee?.CommitteeId ?? "";

        //[NotMapped]
        public int DelegateStateId => DelegateState?.StateId ?? -1;
    }
}
