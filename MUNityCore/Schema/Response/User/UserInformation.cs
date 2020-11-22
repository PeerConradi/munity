using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MUNityCore.Models.User;

namespace MUNityCore.Schema.Response.User
{
    public class UserInformation
    {
        public string Username { get; set; }

        public string Forename { get; set; }

        public string Lastname { get; set; }

        public DateTime LastOnline { get; set; }

        public UserInformation(Models.Core.User user)
        {
            this.Username = user.Username;
            switch (user.PrivacySettings.PublicNameDisplayMode)
            {
                case UserPrivacySettings.ENameDisplayMode.FullName:
                    this.Forename = user.Forename;
                    this.Lastname = user.Lastname;
                    break;
                case UserPrivacySettings.ENameDisplayMode.FullForenameAndFirstLetterLastName:
                    this.Forename = user.Forename;
                    this.Lastname = user.Lastname.First() + ".";
                    break;
                case UserPrivacySettings.ENameDisplayMode.FirstLetterForenameFullLastName:
                    this.Forename = user.Forename.First() + ".";
                    this.Lastname = user.Lastname;
                    break;
                case UserPrivacySettings.ENameDisplayMode.Initals:
                    this.Forename = user.Forename.First() + ".";
                    this.Lastname = user.Lastname.First() + ".";
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            this.LastOnline = user.LastOnline;
            
        }
    }
}
