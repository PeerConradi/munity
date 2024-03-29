﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using MUNity.Base;
using MUNity.Database.Models.User;
using MUNityCore.Models.User;


namespace MUNity.Database.Models.User
{

    /// <summary>
    /// a registered user on the platform. Do not use this Model
    /// inside any of the controllers unless you use it to validate something.
    /// When sending out information, always use the UserSchema
    /// </summary>
    public class MunityUser : IdentityUser, IUserPrivacySettings
    {

        [MaxLength(100)]
        public string Title { get; set; }

        [MaxLength(250)]
        public string Forename { get; set; }

        [MaxLength(250)]
        public string Lastname { get; set; }

        [MaxLength(250)]
        public string Gender { get; set; }

        public DateOnly Birthday { get; set; }  // TODO: Change to DateOnly

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

        public EUserState UserState { get; set; }

        public ICollection<Resolution.ResolutionAuth> CreatedResolutions { get; set; }

        //public ICollection<MunityUserRole> UserRoles { get; set; }

        public ICollection<UserNotification> Notifications { get; set; }

        public ICollection<UserFriend> Friends { get; set; } = new List<UserFriend>();

        public ICollection<UserFriend> InverseFriends { get; set; } = new List<UserFriend>();

        public ICollection<UserBlocked> BlockedUsers { get; set; } = new List<UserBlocked>();

        public ICollection<UserBlocked> BlockedBy { get; set; } = new List<UserBlocked>();

        public ENameDisplayMode PublicNameDisplayMode { get; set; }
        public ENameDisplayMode InternalNameDisplayMode { get; set; }
        public ENameDisplayMode ConferenceNameDisplayMode { get; set; }
        public EDisplayAuthMode ConferenceHistoryDisplayMode { get; set; }
        public EDisplayAuthMode FriendslistDisplayMode { get; set; }
        public EDisplayAuthMode PinboardDisplayMode { get; set; }
        public EDisplayAuthMode AgeDisplayMode { get; set; }

        public bool IsShadowUser { get; set; }

        [MaxLength(32)]
        public string InviteSecret { get; set; }

        [NotMapped]
        public string GetDisplayNamePublic
        {
            get
            {
                switch (PublicNameDisplayMode)
                {
                    case ENameDisplayMode.FullName:
                        return $"{Forename} {Lastname}";
                    case ENameDisplayMode.FullForenameAndFirstLetterLastName:
                        return $"{Forename} " + (Lastname.Length > 0 ? Lastname[0] + "." : "");
                    case ENameDisplayMode.FirstLetterForenameFullLastName:
                        return (Forename.Length > 0 ? Forename[0] + "." : "") + $" {Lastname}";
                    case ENameDisplayMode.Initals:
                        return (Forename.Length > 0 ? Forename[0] + "." : "") + " " + (Lastname.Length > 0 ? Lastname[0] + "." : "");
                    default:
                        throw new NotImplementedException();
                }
            }
        }

        public MunityUser()
        {
            //this.Id = Guid.NewGuid().ToString();
        }

        public MunityUser(string username, string email)
        {
            //Id = Guid.NewGuid().ToString();
            this.UserName = username;
            this.NormalizedUserName = username.ToUpper();
            this.Email = email;
            this.NormalizedEmail = email.ToUpper();
        }


    }
}
