using MUNityCore.Models.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static MUNityCore.Models.User.MunityUserAuth;

namespace MUNityCore.Schema.Response.User
{
    public class UserAuthSchema
    {
        public int UserAuthId { get; set; }

        public string UserAuthName { get; set; }

        public bool CanCreateOrganisation { get; set; }

        public EAuthLevel AuthLevel { get; set; }

        public UserAuthSchema(MunityUserAuth auth)
        {
            this.UserAuthId = auth.MunityUserAuthId;
            this.UserAuthName = auth.UserAuthName;
            this.CanCreateOrganisation = auth.CanCreateOrganization;
            this.AuthLevel = auth.AuthLevel;
        }

        public static implicit operator UserAuthSchema (MunityUserAuth auth)
        {
            return new UserAuthSchema(auth);
        }
    }
}
