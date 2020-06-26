using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using MUNityAngular.Models.Core;
using MUNityAngular.Models.Resolution;
using MUNityAngular.Models.Resolution.V2;

namespace MUNityAngular.Services
{
    public interface IAuthService
    {
        public bool CanUserEditResolution(User user, ResolutionV2 resolution);

        public User GetUserOfClaimPrincipal(ClaimsPrincipal principal);

    }
}
