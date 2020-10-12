using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using MongoDB.Bson.Serialization.Attributes;
using MUNityAngular.Models.Conference;


namespace MUNityAngular.Models.Core
{
    [DataContract]
    public class User : IUserInformation
    {
        public enum EUserState
        {
            OK,
            BANNED
        }


        [DataMember]
        [MaxLength(80)]
        public int UserId { get; set; }

        [DataMember]
        [MaxLength(40)]
        public string Username { get; set; }

        [JsonIgnore]
        [IgnoreDataMember]
        [MaxLength(250)]
        public string Password { get; set; }

        [DataMember]
        [MaxLength(250)]
        public string Mail { get; set; }

        [JsonIgnore]
        [IgnoreDataMember]
        [MaxLength(250)]
        public string Salt { get; set; }

        [DataMember]
        [MaxLength(100)]
        public string Title { get; set; }

        [DataMember]
        [MaxLength(250)]
        public string Forename { get; set; }

        [DataMember]
        [MaxLength(250)]
        public string Lastname { get; set; }

        [DataMember]
        [MaxLength(250)]
        public string Gender { get; set; }

        [DataMember]
        public DateTime Birthday { get; set; }

        //[DataMember]
        //public string Country { get; set; }

        [DataMember]
        [MaxLength(300)]
        public string Street { get; set; }

        [DataMember]
        [MaxLength(50)]
        public string Zipcode { get; set; }

        [DataMember]
        [MaxLength(300)]
        public string City { get; set; }

        [DataMember]
        [MaxLength(20)]
        public string Housenumber { get; set; }

        [DataMember]
        [MaxLength(250)]
        public string ProfileImageName { get; set; }

        [DataMember]
        public DateTime RegistrationDate { get; set; }

        [DataMember]
        public DateTime LastOnline { get; set; }

        [IgnoreDataMember]
        [JsonIgnore]
        [Newtonsoft.Json.JsonIgnore]
        public UserAuth Auth { get; set; }

        [IgnoreDataMember]
        [JsonIgnore]
        [Newtonsoft.Json.JsonIgnore]
        public EUserState UserState { get; set; }

        [Timestamp]
        public byte[] UserTimestamp { get; set; }
    }
}
