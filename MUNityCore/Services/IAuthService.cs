using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using MUNityCore.Models.Conference;
using MUNityCore.Models.Resolution;
using MUNityCore.Models.User;
using MUNityCore.Models.Resolution.V2;
using MUNityCore.Schema.Request;
using MUNityCore.Schema.Request.Authentication;
using MUNityCore.Schema.Response.Authentication;

namespace MUNityCore.Services
{
    public interface IAuthService
    {
        public bool CanUserEditResolution(MunityUser user, ResolutionV2 resolution);

        public MunityUser GetUserOfClaimPrincipal(ClaimsPrincipal principal);

        AuthenticationResponse Authenticate(AuthenticateRequest model);

        Task<int> SetUserAuth(MunityUser user, MunityUserAuth auth);

        MunityUser GetUserWithAuthByClaimPrincipal(ClaimsPrincipal principal);

        bool IsUserPrincipalAdmin(ClaimsPrincipal principal);

        Task<MunityUserAuth> GetAuth(int authid);

        MunityUserAuth CreateAuth(string name);


        string GenerateToken(MunityUser user);

        bool CanUserEditConference(MunityUser user, Conference conference);
        MunityUserAuth CreateUserAuth(AdminSchema.CreateUserAuthBody body);
    }
}
