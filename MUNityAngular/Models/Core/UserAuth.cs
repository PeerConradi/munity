using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MUNityCore.Models.Core
{

    [DataContract]
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

        [DataMember]
        public int UserAuthId { get; set; }

        [DataMember]
        [MaxLength(150)]
        public string UserAuthName { get; set; }

        [IgnoreDataMember]
        [JsonIgnore]
        [Newtonsoft.Json.JsonIgnore]
        public List<User> Users { get; set; }

        [DataMember]
        public bool CanCreateOrganisation { get; set; }

        [DataMember]
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
