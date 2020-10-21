using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MUNityCore.Models.Conference
{

    [DataContract]
    public class Delegation
    {
        [DataMember]
        public string DelegationId { get; set; }

        [DataMember]
        [MaxLength(150)]
        [Required]
        public string Name { get; set; }

        [DataMember]
        [MaxLength(250)]
        public string FullName { get; set; }

        [DataMember]
        [MaxLength(10)]
        public string Abbreviation { get; set; }

        [IgnoreDataMember]
        [JsonIgnore]
        [Newtonsoft.Json.JsonIgnore]
        public Conference Conference { get; set; }

        [NotMapped] [DataMember] public string ConferenceId => Conference?.ConferenceId ?? "";

        public Delegation()
        {
            DelegationId = Guid.NewGuid().ToString();
        }
    }
}
