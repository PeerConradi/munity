using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using MUNityCore.Models.Conference;
using MUNityCore.Models.Resolution;
using MUNityCore.Models.User;
using MUNityCore.Models.Resolution.V2;
using MUNitySchema.Schema.Authentication;
using MUNitySchema.Schema.User;
using MUNitySchema.Models.Resolution;

namespace MUNityCore.Services
{
    public interface IAuthService
    {
        public bool CanUserEditResolution(MunityUser user, Resolution resolution);

        public MunityUser GetUserOfClaimPrincipal(ClaimsPrincipal principal);

        AuthenticationResponse Authenticate(MUNitySchema.Schema.Authentication.AuthenticateRequest model);

        Task<int> SetUserAuth(MunityUser user, MunityUserAuth auth);

        MunityUser GetUserWithAuthByClaimPrincipal(ClaimsPrincipal principal);

        bool IsUserPrincipalAdmin(ClaimsPrincipal principal);

        Task<MunityUserAuth> GetAuth(int authid);

        MunityUserAuth CreateAuth(string name);

        string GenerateToken(MunityUser user);

        bool CanUserEditConference(MunityUser user, Conference conference);
    }
}
