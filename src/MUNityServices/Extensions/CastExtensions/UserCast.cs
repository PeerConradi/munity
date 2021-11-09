using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MUNityCore.Models.User;
using MUNity.Schema.User;
using MUNity.Database.Models.User;
using MUNity.Base;

namespace MUNity.Services.Extensions.CastExtensions;

public static class UserCast
{
    public static UserInformation AsInformation(this MunityUser user)
    {
        var mdl = new UserInformation();
        mdl.Username = user.UserName;
        mdl.LastOnline = user.LastOnline;
        switch (user.PublicNameDisplayMode)
        {
            case ENameDisplayMode.FullName:
                mdl.Forename = user.Forename;
                mdl.Lastname = user.Lastname;
                break;
            case ENameDisplayMode.FullForenameAndFirstLetterLastName:
                mdl.Forename = user.Forename;
                mdl.Lastname = user.Lastname.First() + ".";
                break;
            case ENameDisplayMode.FirstLetterForenameFullLastName:
                mdl.Forename = user.Forename.First() + ".";
                mdl.Lastname = user.Lastname;
                break;
            case ENameDisplayMode.Initals:
                mdl.Forename = user.Forename.First() + ".";
                mdl.Lastname = user.Lastname.First() + ".";
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        return mdl;
    }
}
