using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MUNity.Schema.Authentication
{
    /// <summary>
    /// The schema for the login used by the MUNity API.
    /// </summary>
    public class AuthenticateRequest
    {
        /// <summary>
        /// The username
        /// </summary>
        [Required]
        [MaxLength(40)]
        public string Username { get; set; }

        /// <summary>
        /// The password
        /// </summary>
        [Required]
        [MaxLength(300)]
        public string Password { get; set; }
    }
}
