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
    public class Participation
    {

        [DataMember]
        public int ParticipationId { get; set; }

        [IgnoreDataMember]
        [JsonIgnore]
        [Newtonsoft.Json.JsonIgnore]
        public AbstractRole Role { get; set; }

        [NotMapped]
        public int RoleId => Role?.RoleId ?? -1;

        [IgnoreDataMember]
        [JsonIgnore]
        [Newtonsoft.Json.JsonIgnore]
        public Core.User User { get; set; }

        [NotMapped]
        public string Username => User?.Username ?? "";

        [DataMember]
        public bool IsMainRole { get; set; }

        [DataMember]
        public double Cost { get; set; }

        [DataMember]
        public double Paid { get; set; }

        /// <summary>
        /// The ParticipationSecret is a Key that can identify the user as a praticipant
        /// it should be Unique and be usable and can be used as a shared password between the
        /// team and a user to check the users identity when at the conference.
        /// </summary>
        [IgnoreDataMember]
        [MaxLength(200)]
        public string ParticipationSecret { get; set; }

        [Timestamp]
        [IgnoreDataMember]
        [JsonIgnore]
        [Newtonsoft.Json.JsonIgnore]
        public byte[] ParticipationTimestamp { get; set; }
    }
}
