using MUNityAngular.Models.Facades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MUNityAngular.DataHandlers.EntityFramework.Models
{
    public class User : IUserFacade
    {
        public int UserId { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public string Salt { get; set; }

        public string Title { get; set; }

        public string Forename { get; set; }

        public string Lastname { get; set; }

        public string Gender { get; set; }

        public DateTime Birthday { get; set; }

        public string Street { get; set; }

        public string Zipcode { get; set; }

        public string City { get; set; }

        public string Housenumber { get; set; }

        public string ProfileImageName { get; set; }

        public DateTime RegistrationDate { get; set; }

        public DateTime LastOnline { get; set; }
    }
}
