using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MUNityCore.Models.User;
using MUNity.Schema.User;

namespace MUNityCore.Extensions.CastExtensions
{
    public static class UserCast
    {
        public static UserInformation AsInformation(this MunityUser user)
        {
            var mdl = new UserInformation();
            mdl.Username = user.Username;
            mdl.LastOnline = user.LastOnline;
            switch (user.PrivacySettings.PublicNameDisplayMode)
            {
                case UserPrivacySettings.ENameDisplayMode.FullName:
                    mdl.Forename = user.Forename;
                    mdl.Lastname = user.Lastname;
                    break;
                case UserPrivacySettings.ENameDisplayMode.FullForenameAndFirstLetterLastName:
                    mdl.Forename = user.Forename;
                    mdl.Lastname = user.Lastname.First() + ".";
                    break;
                case UserPrivacySettings.ENameDisplayMode.FirstLetterForenameFullLastName:
                    mdl.Forename = user.Forename.First() + ".";
                    mdl.Lastname = user.Lastname;
                    break;
                case UserPrivacySettings.ENameDisplayMode.Initals:
                    mdl.Forename = user.Forename.First() + ".";
                    mdl.Lastname = user.Lastname.First() + ".";
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            return mdl;
        }

        public static UserAuthSchema AsAuthSchema(this MunityUserAuth auth)
        {
            var mdl = new UserAuthSchema()
            {
                AuthLevel = auth.AuthLevel,
                CanCreateOrganization = auth.CanCreateOrganization,
                UserAuthId = auth.MunityUserAuthId,
                UserAuthName = auth.UserAuthName
            };
            return mdl;
        }
    }
}
