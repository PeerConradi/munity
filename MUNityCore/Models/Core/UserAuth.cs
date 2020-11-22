using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using MUNityCore.Schema.Request;

namespace MUNityCore.Models.Core
{
    /// <summary>
    /// The Authorization of one or more users.
    /// </summary>
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

        public bool CanCreateOrganization { get; set; }

        public EAuthLevel AuthLevel { get; set; }

        public UserAuth()
        {
            Users = new List<User>();
            CanCreateOrganization = false;
            AuthLevel = EAuthLevel.New;
        }

        public UserAuth(string name)
        {
            Users = new List<User>();
            CanCreateOrganization = false;
            AuthLevel = EAuthLevel.New;
            UserAuthName = name;
        }

        public UserAuth(AdminSchema.CreateUserAuthBody request)
        {
            Users = new List<User>();
            CanCreateOrganization = request.CanCreateOrganisation;
            AuthLevel = EAuthLevel.New;
            UserAuthName = request.Name;
        }
    }
}
