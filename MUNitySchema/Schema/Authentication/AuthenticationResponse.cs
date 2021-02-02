using System;
using System.Collections.Generic;
using System.Text;

namespace MUNity.Schema.Authentication
{
    /// <summary>
    /// The authentication response sent by the MUNity API when the login was successful or when you validate
    /// a token at the api.
    /// </summary>
    public class AuthenticationResponse
    {
        /// <summary>
        /// The username sent back to the user.
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// The full first name of the user.
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// The full lastname of the user
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// A baerer token for this user and session.
        /// </summary>
        public string Token { get; set; }
    }
}
