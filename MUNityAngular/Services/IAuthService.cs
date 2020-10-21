using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using MUNityCore.Models.Resolution;
using MUNityCore.Models.Core;
using MUNityCore.Models.Resolution.V2;
using MUNityCore.Schema.Request.Authentication;
using MUNityCore.Schema.Response.Authentication;

namespace MUNityCore.Services
{
    public interface IAuthService
    {
        public bool CanUserEditResolution(User user, ResolutionV2 resolution);

        public User GetUserOfClaimPrincipal(ClaimsPrincipal principal);

        AuthenticationResponse Authenticate(AuthenticateRequest model);
    }
}
