﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using MUNityCore.Models.User;


namespace MUNity.Database.Models.User
{

    /// <summary>
    /// a registered user on the platform. Do not use this Model
    /// inside any of the controllers unless you use it to validate something.
    /// When sending out information, always use the UserSchema
    /// </summary>
    public class MunityUser : IdentityUser
    {
        public enum EUserState
        {
            OK,
            BANNED
        }

        [MaxLength(100)]
        public string Title { get; set; }

        [MaxLength(250)]
        public string Forename { get; set; }

        [MaxLength(250)]
        public string Lastname { get; set; }

        [MaxLength(250)]
        public string Gender { get; set; }

        public DateTime Birthday { get; set; }

        [MaxLength(300)]
        public string Country { get; set; }

        [MaxLength(300)]
        public string Street { get; set; }

        [MaxLength(50)]
        public string Zipcode { get; set; }

        [MaxLength(300)]
        public string City { get; set; }

        [MaxLength(20)]
        public string HouseNumber { get; set; }

        [MaxLength(250)]
        public string ProfileImageName { get; set; }

        public DateTime RegistrationDate { get; set; }

        public DateTime LastOnline { get; set; }

        public MunityUserAuth Auth { get; set; }

        public EUserState UserState { get; set; }

        public List<MunityUser> Friends { get; set; }

        public UserPrivacySettings PrivacySettings { get; set; }

        public List<Resolution.V2.ResolutionAuth> CreatedResolutions { get; set; }

        [Timestamp]
        public byte[] UserTimestamp { get; set; }

        public MunityUser()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        public MunityUser(string id)
        {
            this.Id = id ?? Guid.NewGuid().ToString();
        }
    }
}