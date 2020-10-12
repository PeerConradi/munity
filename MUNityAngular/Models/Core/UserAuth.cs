using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MUNityAngular.Models.Core
{
    public class UserAuth
    {
        public enum EAuthLevel
        {
            Headadmin,
            Admin,
            Developer,
            Moderator,
            User,
            New
        }

        public int UserAuthId { get; set; }

        [MaxLength(150)]
        public string UserAuthName { get; set; }

        public List<User> Users { get; set; }

        public bool CanCreateOrganisation { get; set; }

        public EAuthLevel AuthLevel { get; set; }

        public UserAuth()
        {
            Users = new List<User>();
            CanCreateOrganisation = false;
            AuthLevel = EAuthLevel.New;
        }

        public UserAuth(string name)
        {
            Users = new List<User>();
            CanCreateOrganisation = false;
            AuthLevel = EAuthLevel.New;
            UserAuthName = name;
        }
    }
}
