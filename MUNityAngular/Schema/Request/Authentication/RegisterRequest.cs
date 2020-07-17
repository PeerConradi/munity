using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MUNityAngular.Schema.Request.Authentication
{
    public class RegisterRequest
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Forename { get; set; }

        [Required]
        public string Lastname { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string Mail { get; set; }

        [Required]
        public DateTime Birthday { get; set; }
    }
}
