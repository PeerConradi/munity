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
    [DataContract]
    public class VisitorRole : AbstractRole
    {
        [MaxLength(100)]
        [DataMember]
        public string Organization { get; set; }

    }
}
