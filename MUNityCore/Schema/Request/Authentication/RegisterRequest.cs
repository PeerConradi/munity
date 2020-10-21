using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MUNityCore.Schema.Request.Authentication
{
    public class RegisterRequest
    {
        [Required]
        [MaxLength(40)]
        public string Username { get; set; }

        [Required]
        [MaxLength(200)]
        public string Forename { get; set; }

        [Required]
        [MaxLength(200)]
        public string Lastname { get; set; }

        [Required]
        [MaxLength(300)]
        public string Password { get; set; }

        [Required]
        [MaxLength(300)]
        public string Mail { get; set; }

        [Required]
        public DateTime Birthday { get; set; }
    }
}
