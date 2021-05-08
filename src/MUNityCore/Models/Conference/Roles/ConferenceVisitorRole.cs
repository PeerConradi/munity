using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace MUNityCore.Models.Conference.Roles
{

    /// <summary>
    /// A visitor role is for someone who just wants to come to the conference
    /// without being part of any of the other roles.
    /// </summary>
    public class ConferenceVisitorRole : AbstractConferenceRole
    {
        [MaxLength(100)]
        public string Organization { get; set; }

    }
}
