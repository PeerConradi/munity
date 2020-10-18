using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace MUNityAngular.Models.Resolution.V2
{

    [DataContract]
    public class ResolutionUser
    {
        // TODO: The primary key should be a shared key of CoreUserId and the Auth
        public int ResolutionUserId { get; set; }

        [DataMember]
        public int CoreUserId { get; set; }

        [DataMember]
        [MaxLength(100)]
        public string Username { get; set; }

        [DataMember]
        [MaxLength(150)]
        public string ForeName { get; set; }

        [DataMember]
        [MaxLength(150)]
        public string LastName { get; set; }

        public bool CanRead { get; set; } = true;

        public bool CanWrite { get; set; } = false;

        public bool CanAddUsers { get; set; } = false;

        public ResolutionAuth Auth { get; set; }
    }
}
