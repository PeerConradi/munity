using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MUNityAngular.Models.Conference
{
    public class Participation
    {
        public int ParticipationId { get; set; }

        [JsonIgnore]
        [Newtonsoft.Json.JsonIgnore]
        public AbstractRole Role { get; set; }

        [JsonIgnore]
        [Newtonsoft.Json.JsonIgnore]
        public Core.User User { get; set; }

        public bool IsMainRole { get; set; }

        public double Cost { get; set; }

        public double Paid { get; set; }

        /// <summary>
        /// The ParticipationSecret is a Key that can identify the user as a praticipant
        /// it should be Unique and be usable and can be used as a shared password between the
        /// team and a user to check the users identity when at the conference.
        /// </summary>
        public string ParticipationSecret { get; set; }

        [Timestamp]
        [JsonIgnore]
        [Newtonsoft.Json.JsonIgnore]
        public byte[] ParticipationTimestamp { get; set; }
    }
}
