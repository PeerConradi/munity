using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MUNityCore.Schema.Request.Authentication
{
    // Ported to MUNityData NPM
    public class AuthenticateRequest
    {
        [Required]
        [MaxLength(40)]
        public string Username { get; set; }

        [Required]
        [MaxLength(300)]
        public string Password { get; set; }
    }
}
