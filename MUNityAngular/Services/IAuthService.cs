using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using MUNityAngular.Models.Core;
using MUNityAngular.Models.Resolution;
using MUNityAngular.Models.Resolution.V2;
using MUNityAngular.Schema.Request.Authentication;
using MUNityAngular.Schema.Response.Authentication;

namespace MUNityAngular.Services
{
    public interface IAuthService
    {
        public bool CanUserEditResolution(User user, ResolutionV2 resolution);

        public User GetUserOfClaimPrincipal(ClaimsPrincipal principal);

        AuthenticationResponse Authenticate(AuthenticateRequest model);
    }
}
