using System.Collections.Generic;
using MUNity.Database.Models.User;

namespace MUNity.Database.BaseData
{
    public class DefaultAuthorizations
    {
        public static IEnumerable<MunityRole> UserRoles
        {
            get
            {
                yield return new MunityRole()
                {
                    Name = "Head-Admin",
                    NormalizedName = "HEAD-ADMIN",
                    CanCreateNewOrganizations = true,
                    IsPlatformAdmin = true
                };

                yield return new MunityRole()
                {
                    Name = "Admin", 
                    NormalizedName = "ADMIN", 
                    CanCreateNewOrganizations = true,
                    IsPlatformAdmin = true
                };

                yield return new MunityRole()
                {
                    Name = "Trusted-User",
                    NormalizedName = "TRUSTED-USER",
                    CanCreateNewOrganizations = true,
                    IsPlatformAdmin = false
                };
            }
        }
    }
}