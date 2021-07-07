using MUNity.Database.Models.Conference.Roles;
using MUNity.Database.Models.User;
using MUNityCore.Models.User;
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
    /// The participation of a user for a role inside of a conference.
    /// </summary>
    public class Participation
    {

        public int ParticipationId { get; set; }

        public AbstractConferenceRole Role { get; set; }

        public MunityUser User { get; set; }

        public bool IsMainRole { get; set; }

        public double Cost { get; set; }

        public double Paid { get; set; }

        /// <summary>
        /// The ParticipationSecret is a Key that can identify the user as a participant
        /// it should be Unique and be usable and can be used as a shared password between the
        /// team and a user to check the users identity when at the conference.
        /// </summary>
        [MaxLength(200)]
        public string ParticipationSecret { get; set; }

        [Timestamp]
        public byte[] ParticipationTimestamp { get; set; }
    }
}
