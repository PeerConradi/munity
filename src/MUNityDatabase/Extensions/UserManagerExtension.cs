using Microsoft.AspNetCore.Identity;
using MUNity.Database.Models.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MUNity.Database.Extensions
{
    public static class UserManagerExtension
    {
        public static async Task<(IdentityResult Result, MunityUser User)> CreateShadowUser(this UserManager<MunityUser> userManager, string mail)
        {
            var virtualUser = new MunityUser()
            {
                Email = mail,
                UserName = "v-" + Util.IdGenerator.RandomString(6),   // possibility of collission: 1/26^6 = 1/308.915.776
                IsShadowUser = true,
                Forename = "-",
                Lastname = "-",
                InviteSecret = Util.IdGenerator.RandomString(32)    // 1,9 * 10^45
            };
            var result = await userManager.CreateAsync(virtualUser);

            return (result, virtualUser);
        }
    }
}
