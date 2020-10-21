using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MUNityCore.Util.Extenstions
{
    public static class ClaimsPrincipalExtensions
    {
        public static string UsernameClaim(this ClaimsPrincipal principal)
        {
            return principal.Claims.FirstOrDefault(n => n.Type == ClaimTypes.Name)?.Value ?? "";
        }
    }
}
