using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace MUNityCore.Models.Resolution.V2
{

    [DataContract]
    public class ResolutionUser
    {
        // TODO: The primary key should be a shared key of CoreUserId and the Auth
        public int ResolutionUserId { get; set; }

        public Core.User User { get; set; }

        public bool CanRead { get; set; } = true;

        public bool CanWrite { get; set; } = false;

        public bool CanAddUsers { get; set; } = false;

        public ResolutionAuth Auth { get; set; }
    }
}
