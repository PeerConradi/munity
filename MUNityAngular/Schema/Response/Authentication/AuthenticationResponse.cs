using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MUNityCore.Models.Core;

namespace MUNityCore.Schema.Response.Authentication
{
    public class AuthenticationResponse
    {
        public string Username { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Token { get; set; }

        public AuthenticationResponse(User user, string token)
        {
            this.Username = user.Username;
            this.FirstName = user.Forename;
            this.LastName = user.Lastname;
            this.Token = token;
        }
    }
}
