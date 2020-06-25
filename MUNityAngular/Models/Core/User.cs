using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using MongoDB.Bson.Serialization.Attributes;


namespace MUNityAngular.Models.Core
{
    [DataContract]
    public class User
    {

        [DataMember]
        public int UserId { get; set; }

        [DataMember]
        public string Username { get; set; }

        [JsonIgnore]
        [IgnoreDataMember]
        public string Password { get; set; }

        [DataMember]
        public string Mail { get; set; }

        [JsonIgnore]
        [IgnoreDataMember]
        public string Salt { get; set; }

        [DataMember]
        public string Title { get; set; }

        [DataMember]
        public string Forename { get; set; }

        [DataMember]
        public string Lastname { get; set; }

        [DataMember]
        public string Gender { get; set; }

        [DataMember]
        public DateTime Birthday { get; set; }

        [DataMember]
        public string Street { get; set; }

        [DataMember]
        public string Zipcode { get; set; }

        [DataMember]
        public string City { get; set; }

        [DataMember]
        public string Housenumber { get; set; }

        [DataMember]
        public string ProfileImageName { get; set; }

        [DataMember]
        public DateTime RegistrationDate { get; set; }

        [DataMember]
        public DateTime LastOnline { get; set; }
    }
}
