using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using MUNityCore.Models.User;
using MUNity.Schema.Authentication;
using MUNity.Schema.User;
using MUNity.Models.Resolution;
using MUNity.Database.Models.User;
using MUNity.Database.Models.Conference;

namespace MUNity.Services
{
    public interface IAuthService
    {
        public bool CanUserEditResolution(MunityUser user, Resolution resolution);

        public MunityUser GetUserOfClaimPrincipal(ClaimsPrincipal principal);

        //AuthenticationResponse Authenticate(AuthenticateRequest model);

        Task<int> SetUserAuth(MunityUser user, MunityUserAuth auth);

        MunityUser GetUserWithAuthByClaimPrincipal(ClaimsPrincipal principal);

        bool IsUserPrincipalAdmin(ClaimsPrincipal principal);

        Task<MunityUserAuth> GetAuth(int authid);

        MunityUserAuth CreateAuth(string name);

        string GenerateToken(MunityUser user);

        bool CanUserEditConference(MunityUser user, Conference conference);

        List<MUNityCore.Models.User.MunityRole> Roles();

        bool CreateAdminRole();

        MunityUser GetUser(string mail);
    }
}
