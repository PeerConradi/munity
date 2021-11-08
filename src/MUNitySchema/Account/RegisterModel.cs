using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MUNity.Schema.Account
{
    public class RegisterModel
    {
        [MinLength(3)]
        [MaxLength(30)]
        [Required]
        public string Username { get; set; }

        [EmailAddress]
        [DataType(DataType.EmailAddress)]
        [Required]
        public string Mail { get; set; }

        [MinLength(8)]
        [DataType(DataType.Password)]
        [Required]
        public string Password { get; set; }

        [MinLength(8)]
        [Compare(nameof(Password))]
        [Required]
        public string ConfirmPassword { get; set; }
    }

    public class RegistrationResult
    {
        public enum RegistrationStatusCodes
        {
            Success,
            Exception,
            UsernameTaken,
            EMailTaken,
            PasswordWeak,
            PasswordsNotMatching
        }

        public RegistrationStatusCodes Status { get; set; }

        public string UserId { get; set; }

    }
}
