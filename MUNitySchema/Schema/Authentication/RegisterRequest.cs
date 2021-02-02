using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MUNity.Schema.Authentication
{
    /// <summary>
    /// The full request to create a new account at the munity API. Note that this will be reworked to
    /// only needing a Username, Password and Mail in the next implementations.
    /// </summary>
    public class RegisterRequest
    {
        /// <summary>
        /// The username that should be used to login and will be used as display name until the user
        /// types in a Forename and/or lastname.
        /// </summary>
        [Required]
        [MaxLength(40)]
        public string Username { get; set; }

        /// <summary>
        /// The forname of the user. Is required in this version but will not be used in future implementations
        /// of the munity api.
        /// </summary>
        [Required]
        [MaxLength(200)]
        public string Forename { get; set; }

        /// <summary>
        /// the lastname of the user. Is required in this version but will not be used in future implementations
        /// of the munity api.
        /// </summary>
        [Required]
        [MaxLength(200)]
        public string Lastname { get; set; }

        /// <summary>
        /// the password.
        /// </summary>
        [Required]
        [MaxLength(300)]
        public string Password { get; set; }

        /// <summary>
        /// The mail address.
        /// </summary>
        [Required]
        [MaxLength(300)]
        public string Mail { get; set; }

        /// <summary>
        /// The birtdate of the user.
        /// </summary>
        [Required]
        public DateTime Birthday { get; set; }
    }
}
