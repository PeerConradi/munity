using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using MUNityCore.Models.User;

namespace MUNityCore.Schema.Request
{
    public class UserRequests
    {
        public class UpdatePrivacySettingsBody
        {
            [Required]
            public string Username { get; set; }

            [Required]
            public UserPrivacySettings.ENameDisplayMode PublicNameDisplayMode { get; set; }

            [Required]
            public UserPrivacySettings.ENameDisplayMode InternalNameDisplayMode { get; set; }

            [Required]
            public UserPrivacySettings.ENameDisplayMode ConferenceNameDisplayMode { get; set; }

            [Required]
            public UserPrivacySettings.EDisplayAuthMode ConferenceHistory { get; set; }

            [Required]
            public UserPrivacySettings.EDisplayAuthMode Friendslist { get; set; }

            [Required]
            public UserPrivacySettings.EDisplayAuthMode Pinboard { get; set; }

            [Required]
            public UserPrivacySettings.EDisplayAuthMode Age { get; set; }
        }
    }
}
